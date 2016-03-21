Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq
Imports log4net

' ReSharper disable CheckNamespace
' ReSharper disable InconsistentNaming
Partial Class CLibrary_SurveyForm

    ' ReSharper restore InconsistentNaming
    ' ReSharper restore CheckNamespace
    Inherits CLibraryBase

    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _surveyId As Integer = 0
    <appx.cms(appx.cmsAttribute.DataValueType.CMSSurvey)> _
    Public Property SurveyId() As Integer
        Get
            Return _surveyId
        End Get
        Set(ByVal value As Integer)
            _surveyId = value
        End Set
    End Property

    Private _showPreText As Boolean = True
    <appx.cms(appx.cmsAttribute.DataValueType.Bool)> _
    Public Property ShowPreText() As Boolean
        Get
            Return _showPreText
        End Get
        Set(ByVal value As Boolean)
            _showPreText = value
        End Set
    End Property

    Private _showPostText As Boolean = True
    <appx.cms(appx.cmsAttribute.DataValueType.Bool)> _
    Public Property ShowPostText() As Boolean
        Get
            Return _showPostText
        End Get
        Set(ByVal value As Boolean)
            _showPostText = value
        End Set
    End Property

    Private _enableCaptcha As Boolean = False
    <appx.cms(appx.cmsAttribute.DataValueType.Bool)> _
    Public Property EnableCaptcha() As Boolean
        Get
            Return _enableCaptcha
        End Get
        Set(ByVal value As Boolean)
            _enableCaptcha = value
        End Set
    End Property

    Private _captchaTheme As String = ""
    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property CaptchaTheme As String
        Get
            Return _captchaTheme
        End Get
        Set(value As String)
            _captchaTheme = value
        End Set
    End Property

    Private _surveyPreText As String = ""
    Protected Property SurveyPreText() As String
        Get
            Return _surveyPreText
        End Get
        Set(ByVal value As String)
            _surveyPreText = value
        End Set
    End Property

    Private _surveyPostText As String = ""
    Protected Property SurveyPostText() As String
        Get
            Return _surveyPostText
        End Get
        Set(ByVal value As String)
            _surveyPostText = value
        End Set
    End Property

    Private _validationGroup As String = "vg" & Me.ClientID
    Public Property ValidationGroup As String
        Get
            Return _validationGroup
        End Get
        Set(value As String)
            _validationGroup = value
        End Set
    End Property

    Private _inDialog As Boolean = False
    Public Property InDialog As Boolean
        Get
            Return _inDialog
        End Get
        Set(value As Boolean)
            _inDialog = value

            If value Then
                pSubmit.CssClass = ""
                pSubmit.Style.Add("text-align", "right")
                pSubmit.Style.Add("margin", "1em")
                btnSave.CssClass = "makeButton"
                btnSingleClickSave.CssClass = "makeButton"
            Else
                pSubmit.CssClass = "makeButtonPane"
                pSubmit.Style.Clear()
                btnSave.CssClass = ""
                btnSingleClickSave.CssClass = ""
            End If
        End Set
    End Property

    Private _singleClickButton As Boolean = False
    Public Property SingleClickButton As Boolean
        Get
            Return _singleClickButton
        End Get
        Set(value As Boolean)
            _singleClickButton = value
        End Set
    End Property

    Public Event FailedValidation()

    Protected SubmitButtonText As String = "Submit"
    Protected CanValidateList As New ArrayList
    Protected oJS As New StringBuilder
    Protected bHasButton As Boolean = False

    Protected Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()

        ValidationGroup = "vg" & System.Guid.NewGuid().ToString().Replace("-", "")
        vSumm.ValidationGroup = ValidationGroup
        reCaptchaValidator.ValidationGroup = ValidationGroup
        btnSave.ValidationGroup = ValidationGroup

        CanValidateList.Add("text")
        CanValidateList.Add("memo")
        CanValidateList.Add("email")
        CanValidateList.Add("phone")
        CanValidateList.Add("number")
        CanValidateList.Add("selectlist")

        Dim bNumberQ As Boolean = True
        Dim oSurvey As appxCMS.SurveyHeader = appxCMS.SurveyDataSource.GetSurvey(SurveyId)
        If oSurvey IsNot Nothing Then
            SurveyPreText = oSurvey.PreText
            SurveyPostText = oSurvey.PostText
            If Not String.IsNullOrEmpty(oSurvey.SubmitButtonText.Trim()) Then
                btnSave.Text = oSurvey.SubmitButtonText
                btnSingleClickSave.Text = oSurvey.SubmitButtonText
            End If
            SubmitButtonText = oSurvey.SubmitButtonText

            If oSurvey.NumberQuestions.HasValue Then
                bNumberQ = oSurvey.NumberQuestions.Value
            End If
        End If

        Dim oQuestions As List(Of appxCMS.SurveyQuestion) = appxCMS.SurveyQuestionDataSource.GetQuestions(SurveyId)

        If oQuestions.Count > 0 Then
            Dim sContainer As String = "ol"
            If Not bNumberQ Then
                sContainer = "ul"
            End If

            Dim sLayout As String = ""
            Dim sLayoutFile As String = Server.MapPath("/cmsimages/_surveylayout/" & SurveyId & ".htm")

            If File.Exists(sLayoutFile) Then
                sLayout = File.ReadAllText(sLayoutFile)
            End If

            If String.IsNullOrEmpty(sLayout.Trim) Then
                '-- Create our own layout file based on the questions we have
                Dim oLayoutSB As New StringBuilder
                oLayoutSB.AppendLine("<div>[SurveyPre-Text]</div>")
                oLayoutSB.AppendLine("<" & sContainer & " class=""survey"">")
                For Each oQ As appxCMS.SurveyQuestion In oQuestions
                    oLayoutSB.AppendLine("<li>[Q" & oQ.SurveyQuestionID & ":" & oQ.Question & "]</li>")
                Next
                oLayoutSB.AppendLine("</" & sContainer & ">")
                oLayoutSB.AppendLine("<div>[SurveyPost-Text]</div>")
                sLayout = oLayoutSB.ToString
            End If

            EmbedControls(sLayout, phSurvey, oQuestions)

            If bHasButton Then
                pSubmit.Visible = False
            Else
                If SingleClickButton Then
                    'Response.Write("<h1>SINGLE</h1>")
                    btnSave.Visible = False
                    btnSingleClickSave.Visible = True
                    btnSingleClickSave.ValidationGroup = ValidationGroup
                Else
                    'Response.Write("<h1>NOT SINGLE</h1>")
                    btnSave.Visible = True
                    btnSave.ValidationGroup = ValidationGroup
                    btnSingleClickSave.Visible = False
                End If
            End If

            If Me.EnableCaptcha Then
                pCaptcha.Visible = True
                If Not String.IsNullOrEmpty(Me.CaptchaTheme) Then
                    Dim oTheme As Recaptcha.BuiltInTheme = Global.Recaptcha.BuiltInTheme.None
                    Try
                        oTheme = [Enum].Parse(GetType(Recaptcha.BuiltInTheme), Me.CaptchaTheme)
                        reCaptcha.Theme = oTheme
                    Catch ex As Exception
                    End Try
                End If
                reCaptcha.PrivateKey = ConfigurationManager.AppSettings("RecaptchaPrivateKey")
                reCaptcha.PublicKey = ConfigurationManager.AppSettings("RecaptchaPublicKey")
            End If

            If oJS.Length > 0 Then
                Page.ClientScript.RegisterStartupScript(GetType(String), "showfldreq", oJS.ToString, True)
            End If

        End If
    End Sub


    Private Sub EmbedControls(ByVal sLayout As String, ByVal oPH As PlaceHolder, oQuestions As List(Of appxCMS.SurveyQuestion))
        '-- Replace all control placeholders with rendered controls
        Dim sDyn As String = "\[((Q[0-9]+|SurveyPre\-Text|SurveyPost\-Text|SubmitButton)[^\]]*)\]"
        Dim oDyn As New Regex( _
            sDyn, RegexOptions.Multiline Or _
            RegexOptions.IgnoreCase _
        )

        Dim oMatches As MatchCollection = oDyn.Matches(sLayout)
        Dim sTmpContent As String = sLayout

        Try
            For Each oMatch As Match In oMatches
                Dim sLiteralContent As String = sTmpContent.Substring(0, sTmpContent.IndexOf(oMatch.Value, StringComparison.OrdinalIgnoreCase))
                Dim oLtl As New LiteralControl(sLiteralContent)
                oPH.Controls.Add(oLtl)
                DynamicQuestionLoader(oMatch, oQuestions, oPH)
                sTmpContent = sTmpContent.Substring(sLiteralContent.Length + oMatch.Value.Length)
            Next

            If Not String.IsNullOrEmpty(sTmpContent) Then
                oPH.Controls.Add(New LiteralControl(sTmpContent))
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
            Response.Write(ex.StackTrace)
        End Try
    End Sub


    Protected Sub DynamicQuestionLoader(ByVal m As Match, ByVal oQt As List(Of appxCMS.SurveyQuestion), ByVal oPH As PlaceHolder)
        Dim sLibCall As String = m.Groups(2).Captures(0).Value

        If sLibCall.ToUpper.StartsWith("Q") Then
            '-- We are getting the question
            Dim iQ As Integer = 0
            Dim sQ As String = sLibCall.Substring(1)
            Integer.TryParse(sQ, iQ)
            If iQ > 0 Then
                '-- Load and process the question
                Dim oQuestion As appxCMS.SurveyQuestion = oQt.FirstOrDefault(Function(q) q.SurveyQuestionID = iQ)
                If oQuestion Is Nothing Then
                    If HttpContext.Current.User.IsInRole("Admin") Then
                        lLoadMsg.Text = lLoadMsg.Text & "Unable to load question id" & iQ & "<br/>"
                    End If
                    Exit Sub
                End If

                Dim sQuestion As String = oQuestion.Question
                If oQuestion.DisplayName IsNot Nothing Then
                    If Not String.IsNullOrEmpty(oQuestion.DisplayName.Trim()) Then
                        sQuestion = oQuestion.DisplayName
                    End If
                End If

                oPH.Controls.Add(New LiteralControl("<div class=""question"">" & sQuestion))
                oPH.Controls.Add(New LiteralControl("</div>"))

                Dim oAddOnValidators As New List(Of Control)

                Dim iMaxLength As Integer = 0
                If oQuestion.MaxLength.HasValue Then
                    iMaxLength = oQuestion.MaxLength.Value
                End If

                Dim iColumns As Integer = 0
                If oQuestion.Columns.HasValue Then
                    iColumns = oQuestion.Columns.Value
                End If

                Dim sPlaceholder As String = ""
                If oQuestion.PlaceholderText IsNot Nothing Then
                    sPlaceholder = oQuestion.PlaceholderText.Trim()
                End If

                Dim sClass As String = ""
                If oQuestion.InputClass IsNot Nothing Then
                    sClass = oQuestion.InputClass.Trim()
                End If

                Select Case oQuestion.FieldType.ToLower
                    Case "text"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
                        oFld.TabIndex = oQuestion.SortOrder
                        If iMaxLength > 0 Then
                            oFld.MaxLength = iMaxLength
                        End If

                        If Not String.IsNullOrEmpty(sPlaceholder) Then
                            oFld.Attributes.Add("placeholder", sPlaceholder)
                        End If

                        If iColumns > 0 Then
                            oFld.Columns = iColumns
                        End If
                        If Not String.IsNullOrEmpty(sClass) Then
                            oFld.CssClass = sClass
                        End If

                        'If iColumns = 0 AndAlso String.IsNullOrEmpty(sClass) Then
                        '    oFld.Style.Add("width", "80%")
                        'End If

                        oPH.Controls.Add(oFld)

                    Case "memo"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
                        oFld.TabIndex = oQuestion.SortOrder
                        oFld.TextMode = TextBoxMode.MultiLine
                        oFld.Rows = 5
                        If iMaxLength > 0 Then
                            oFld.MaxLength = iMaxLength
                        End If

                        If Not String.IsNullOrEmpty(sPlaceholder) Then
                            oFld.Attributes.Add("placeholder", sPlaceholder)
                        End If

                        If iColumns > 0 Then
                            oFld.Columns = iColumns
                        End If

                        If Not String.IsNullOrEmpty(sClass) Then
                            oFld.CssClass = sClass
                        End If

                        'If iColumns = 0 AndAlso String.IsNullOrEmpty(sClass) Then
                        '    oFld.Style.Add("width", "80%")
                        'End If

                        oPH.Controls.Add(oFld)

                    Case "email"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
                        oFld.TabIndex = oQuestion.SortOrder
                        If iMaxLength > 0 Then
                            oFld.MaxLength = iMaxLength
                        End If
                        If Not String.IsNullOrEmpty(sPlaceholder) Then
                            oFld.Attributes.Add("placeholder", sPlaceholder)
                        End If
                        If iColumns > 0 Then
                            oFld.Columns = iColumns
                        End If
                        If Not String.IsNullOrEmpty(sClass) Then
                            oFld.CssClass = sClass
                        End If

                        'If iColumns = 0 AndAlso String.IsNullOrEmpty(sClass) Then
                        '    oFld.Style.Add("width", "80%")
                        'End If

                        oPH.Controls.Add(oFld)

                        Dim oRegExpValidator As New RegularExpressionValidator
                        oRegExpValidator.ID = "fldinput" & oQuestion.SurveyQuestionID
                        oRegExpValidator.ControlToValidate = "fld" & oQuestion.SurveyQuestionID
                        oRegExpValidator.ValidationGroup = ValidationGroup
                        oRegExpValidator.Text = "<span class=""fa fa-exclamation-circle text-danger""></span>"
                        oRegExpValidator.ErrorMessage = "Your e-mail address is not formatted properly."
                        oRegExpValidator.EnableClientScript = True
                        oRegExpValidator.ValidationExpression = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?"
                        'oPH.Controls.Add(oRegExpValidator)
                        oAddOnValidators.Add(oRegExpValidator)

                    Case "phone"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
                        oFld.TabIndex = oQuestion.SortOrder
                        If iMaxLength > 0 Then
                            oFld.MaxLength = iMaxLength
                        End If
                        If Not String.IsNullOrEmpty(sPlaceholder) Then
                            oFld.Attributes.Add("placeholder", sPlaceholder)
                        End If
                        If iColumns > 0 Then
                            oFld.Columns = iColumns
                        End If
                        If Not String.IsNullOrEmpty(sClass) Then
                            oFld.CssClass = sClass
                        End If
                        If iColumns = 0 AndAlso String.IsNullOrEmpty(sClass) Then
                            oFld.Columns = 15
                        End If
                        oPH.Controls.Add(oFld)
                        Dim oRegExpValidator As New RegularExpressionValidator
                        oRegExpValidator.ID = "fldinput" & oQuestion.SurveyQuestionID
                        oRegExpValidator.ControlToValidate = "fld" & oQuestion.SurveyQuestionID
                        oRegExpValidator.ValidationGroup = ValidationGroup
                        oRegExpValidator.Text = "<span class=""fa fa-exclamation-circle text-danger""></span>"
                        oRegExpValidator.ErrorMessage = "The phone number is not formatted properly. Please be sure to include your area code."
                        oRegExpValidator.EnableClientScript = True
                        oRegExpValidator.ValidationExpression = "\(?\d{3}\)?(\s|-|\.)?\d{3}(-|\s|\.)?\d{4}"
                        'oPH.Controls.Add(oRegExpValidator)
                        oAddOnValidators.Add(oRegExpValidator)

                    Case "number"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
                        oFld.TabIndex = oQuestion.SortOrder
                        If iMaxLength > 0 Then
                            oFld.MaxLength = iMaxLength
                        End If
                        If Not String.IsNullOrEmpty(sPlaceholder) Then
                            oFld.Attributes.Add("placeholder", sPlaceholder)
                        End If
                        If iColumns > 0 Then
                            oFld.Columns = iColumns
                        End If
                        If Not String.IsNullOrEmpty(sClass) Then
                            oFld.CssClass = sClass
                        End If
                        If iColumns = 0 AndAlso String.IsNullOrEmpty(sClass) Then
                            oFld.Columns = 5
                        End If
                        oPH.Controls.Add(oFld)
                        Dim oRegExpValidator As New RegularExpressionValidator
                        oRegExpValidator.ID = "fldinput" & oQuestion.SurveyQuestionID
                        oRegExpValidator.ControlToValidate = "fld" & oQuestion.SurveyQuestionID
                        oRegExpValidator.ValidationGroup = ValidationGroup
                        oRegExpValidator.Text = "<span class=""fa fa-exclamation-circle text-danger""></span>"
                        oRegExpValidator.ErrorMessage = "A numeric value is required in this field."
                        oRegExpValidator.EnableClientScript = True
                        oRegExpValidator.ValidationExpression = "(\+|-)?[0-9\,\.]+"
                        'oPH.Controls.Add(oRegExpValidator)
                        oAddOnValidators.Add(oRegExpValidator)

                        If oQuestion.MaxValue.HasValue Then
                            Dim iMinVal As Integer = 0
                            If oQuestion.MinValue.HasValue Then
                                iMinVal = oQuestion.MinValue.Value
                            End If
                            Dim iMaxVal As Integer = oQuestion.MaxValue.Value

                            '-- Range validator for this field
                            Dim oRangeFldValidator As New RangeValidator
                            oRangeFldValidator.ID = "fldrange" & oQuestion.SurveyQuestionID
                            oRangeFldValidator.ValidationGroup = ValidationGroup
                            oRangeFldValidator.ControlToValidate = "fld" & oQuestion.SurveyQuestionID
                            oRangeFldValidator.MinimumValue = iMinVal.ToString()
                            oRangeFldValidator.MaximumValue = iMaxVal.ToString()
                            oRangeFldValidator.Text = " !"
                            oRangeFldValidator.ErrorMessage = "You must enter a value between " & iMinVal & " and " & iMaxVal & " for the question '" & sQuestion & "'."
                            oAddOnValidators.Add(oRangeFldValidator)
                        End If

                    Case "checkboxlist"
                        If Not String.IsNullOrEmpty(oQuestion.ResponseOptions.Trim()) Then
                            Dim oFld As New CheckBoxList
                            oFld.ID = "fld" & oQuestion.SurveyQuestionID
                            oFld.TabIndex = oQuestion.SortOrder
                            oFld.RepeatDirection = RepeatDirection.Horizontal
                            oFld.RepeatColumns = 3
                            If Not String.IsNullOrEmpty(sClass) Then
                                oFld.CssClass = sClass
                            End If

                            Dim aOptions() As String = oQuestion.ResponseOptions.Split(New Char() {"|"})

                            For iOpt As Integer = 0 To aOptions.Length - 1
                                Dim sOpt As String = aOptions(iOpt)
                                If Not String.IsNullOrEmpty(sOpt) Then
                                    Dim oLI As New ListItem
                                    oLI.Text = sOpt
                                    oLI.Value = sOpt
                                    oFld.Items.Add(oLI)
                                End If
                            Next
                            oPH.Controls.Add(oFld)
                        End If

                    Case "radiobuttonlist"
                        If Not String.IsNullOrEmpty(oQuestion.ResponseOptions.Trim()) Then
                            Dim oFld As New RadioButtonList
                            oFld.ID = "fld" & oQuestion.SurveyQuestionID
                            oFld.TabIndex = oQuestion.SortOrder
                            oFld.RepeatDirection = RepeatDirection.Horizontal
                            oFld.RepeatColumns = 4
                            If Not String.IsNullOrEmpty(sClass) Then
                                oFld.CssClass = sClass
                            End If

                            Dim aOptions() As String = oQuestion.ResponseOptions.Split(New Char() {"|"})

                            For iOpt As Integer = 0 To aOptions.Length - 1
                                Dim sOpt As String = aOptions(iOpt)
                                If Not String.IsNullOrEmpty(sOpt) Then
                                    Dim oLI As New ListItem
                                    oLI.Text = sOpt
                                    oLI.Value = sOpt
                                    oFld.Items.Add(oLI)
                                End If
                            Next
                            oPH.Controls.Add(oFld)
                        End If

                    Case "selectlist"
                        If Not String.IsNullOrEmpty(oQuestion.ResponseOptions.Trim()) Then
                            Dim oFld As New DropDownList
                            oFld.ID = "fld" & oQuestion.SurveyQuestionID
                            oFld.TabIndex = oQuestion.SortOrder
                            If Not String.IsNullOrEmpty(sClass) Then
                                oFld.CssClass = sClass
                            End If
                            oFld.Items.Add(New ListItem("Select One", ""))

                            Dim aOptions() As String = oQuestion.ResponseOptions.Split(New Char() {"|"})

                            For iOpt As Integer = 0 To aOptions.Length - 1
                                Dim sOpt As String = aOptions(iOpt)
                                If Not String.IsNullOrEmpty(sOpt) Then
                                    Dim oLI As New ListItem
                                    oLI.Text = sOpt
                                    oLI.Value = sOpt
                                    oFld.Items.Add(oLI)
                                End If
                            Next
                            oPH.Controls.Add(oFld)
                        End If
                End Select

                '-- Base required field validation
                If oQuestion.RequiredField.HasValue AndAlso oQuestion.RequiredField.Value = True AndAlso CanValidateList.Contains(oQuestion.FieldType.ToLower) Then

                    Dim sVMsg As String = ""

                    If Not String.IsNullOrEmpty(oQuestion.ValidationMessage) Then
                        sVMsg = oQuestion.ValidationMessage
                    End If

                    If String.IsNullOrEmpty(sVMsg) Then
                        sVMsg = "A response is required for the question '" & sQuestion & "'."
                    End If

                    Dim oReqFldValidator As New RequiredFieldValidator
                    oReqFldValidator.ID = "fldreq" & oQuestion.SurveyQuestionID
                    oReqFldValidator.ValidationGroup = ValidationGroup
                    oReqFldValidator.ControlToValidate = "fld" & oQuestion.SurveyQuestionID
                    oReqFldValidator.Text = "<span class=""fa fa-exclamation-circle text-danger""></span>"
                    oReqFldValidator.ErrorMessage = sVMsg
                    oPH.Controls.Add(oReqFldValidator)
                    oJS.AppendLine("showFldReq('" & oReqFldValidator.ClientID & "');")

                End If

                If oAddOnValidators.Count > 0 Then
                    For Each oControl As Control In oAddOnValidators
                        oPH.Controls.Add(oControl)
                    Next
                End If

            End If
        Else
            Select Case sLibCall.ToLower
                Case "surveypre-text"
                    'oPH.Controls.Add(New LiteralControl(Me.SurveyPreText))
                    '-- Process embedded controls
                    oPH.Controls.Add(DirectCast(Page, appxCMS.PageBase).FormatContent(Me.SurveyPreText))
                Case "surveypost-text"
                    'oPH.Controls.Add(New LiteralControl(Me.SurveyPostText))
                    '-- Process embedded controls
                    oPH.Controls.Add(DirectCast(Page, appxCMS.PageBase).FormatContent(Me.SurveyPostText))
                Case "submitbutton"
                    bHasButton = True

                    If SingleClickButton Then
                        Dim oLnk As New appxCMS.Web.SingleClickButton
                        oLnk.ID = "surveySubmitSingleClickBtn"
                        oLnk.Text = SubmitButtonText
                        oLnk.ValidationGroup = ValidationGroup
                        oLnk.ClickedText = "Processing..."
                        oLnk.ShowProcessingModal = True
                        'oLnk.ProcessingModalTitle = "Saving Your Response"
                        oLnk.ProcessingModalHtml = "<div class=""processingModal""><div class=""sectionHeader"">Processing</div><br /><br /><br /><br /><h4>Processing Your Custom Quote....</h4><br /><br /><br /><br /><p><img src=""/assets/images/loadingbar.gif"" height=""22"" width=""126"" title=""Processing..."" alt=""Processing..."" /></p></div>"
                        AddHandler oLnk.Click, AddressOf btnSave_Click
                        oLnk.CssClass = "btn btn-danger btn-lg pull-right"
                        oPH.Controls.Add(oLnk)
                    Else
                        Dim oLnk As New LinkButton
                        oLnk.ID = "surveySubmitBtn"
                        oLnk.Text = SubmitButtonText
                        oLnk.ValidationGroup = ValidationGroup
                        AddHandler oLnk.Click, AddressOf btnSave_Click
                        oLnk.CssClass = "btn btn-success btn-lg pull-right"
                        oPH.Controls.Add(oLnk)
                    End If
            End Select
        End If
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnSave.Click
        If Me.EnableCaptcha Then
            reCaptcha.Validate()
            If reCaptcha.IsValid Then
                reCaptchaValidatorShim.Text = "VOK"
            End If
        End If

        Page.Validate(btnSave.ValidationGroup)

        If Page.IsValid Then
            Dim sUser As String = HttpContext.Current.User.Identity.Name

            Dim sRedirect As String = ""
            Dim sResource As String = ""
            Dim sSurveyName As String = ""

            Dim oSurvey As appxCMS.SurveyHeader = appxCMS.SurveyDataSource.GetSurvey(SurveyId)
            If oSurvey IsNot Nothing Then
                sRedirect = oSurvey.Redirect
                sResource = oSurvey.ResponseActionResource
                sSurveyName = oSurvey.SurveyName
            End If

            Dim iRespondant As Integer = 0
            If Request.IsAuthenticated Then
                '-- Get authenticated user id
                Dim sRespondant As String = ""
                Integer.TryParse(sRespondant, iRespondant)
            End If
            Dim sIP As String = Request.UserHostAddress
            Dim sBrowser As String = Request.Browser.Browser
            If sBrowser.Length > 150 Then
                sBrowser = sBrowser.Substring(0, 150)
            End If

            '-- Used for external post commands (i.e., CRM action type)
            Dim aPostData As New Hashtable

            Dim oSb As New StringBuilder
            oSb.Append("Response From IP Address: " & sIP & ControlChars.CrLf & ControlChars.CrLf)
            oSb.Append("Response Browser Type: " & sBrowser & ControlChars.CrLf & ControlChars.CrLf)

            Dim sMsg As String = ""
            Dim iResponse As Integer = appxCMS.SurveyResponseDataSource.InsertResponse(SurveyId, iRespondant, Request.Url.Host, sIP, sBrowser, sMsg)

            Response.Cookies.Add(New HttpCookie("LastSurveyResponseId", iResponse.ToString()))

            Dim oQuestions As List(Of appxCMS.SurveyQuestion) = appxCMS.SurveyQuestionDataSource.GetQuestions(SurveyId)
            For Each oQuestion As appxCMS.SurveyQuestion In oQuestions
                If Not oQuestion.Deleted Then
                    Dim iQuestion As Integer = oQuestion.SurveyQuestionID
                    Dim sQuestion As String = oQuestion.Question
                    Dim sResponse As String = ""
                    Select Case oQuestion.FieldType.ToLower
                        Case "text", "memo", "email", "phone", "number"
                            Dim oFld As TextBox = DirectCast(phSurvey.FindControl("fld" & iQuestion), TextBox)
                            If oFld IsNot Nothing Then
                                sResponse = oFld.Text
                            Else
                                Log.Debug("Missing Survey Field " & sQuestion & " from survey " & SurveyId)
                            End If
                        Case "checkboxlist"
                            Dim oFld As CheckBoxList = DirectCast(phSurvey.FindControl("fld" & iQuestion), CheckBoxList)
                            If oFld IsNot Nothing Then
                                Dim alSelect As New List(Of String)
                                For Each oItem As ListItem In oFld.Items
                                    If oItem.Selected Then
                                        alSelect.Add(oItem.Value)
                                    End If
                                Next
                                sResponse = String.Join("|", alSelect.ToArray())
                            Else
                                Log.Debug("Missing Survey Field " & sQuestion & " from survey " & SurveyId)
                            End If

                        Case "radiobuttonlist"
                            Dim oFld As RadioButtonList = DirectCast(phSurvey.FindControl("fld" & iQuestion), RadioButtonList)
                            If oFld IsNot Nothing Then
                                sResponse = oFld.SelectedValue
                            Else
                                Log.Debug("Missing Survey Field " & sQuestion & " from survey " & SurveyId)
                            End If

                        Case "selectlist"
                            Dim oFld As DropDownList = DirectCast(phSurvey.FindControl("fld" & iQuestion), DropDownList)
                            If oFld IsNot Nothing Then
                                sResponse = oFld.SelectedValue
                            Else
                                Log.Debug("Missing Survey Field " & sQuestion & " from survey " & SurveyId)
                            End If
                    End Select

                    appxCMS.SurveyResponseDataSource.InsertResponse(iResponse, iQuestion, sResponse)
                    '-- Store the answer in the stringbuilder for e-mailing
                    oSb.Append(sQuestion & ": " & sResponse.Replace("|", ", ") & ControlChars.CrLf & ControlChars.CrLf)
                    '-- Add the answer to the postdata hashtable for external posting
                    Dim sPKey As String = sQuestion
                    If aPostData.ContainsKey(sQuestion) Then
                        sPKey = iQuestion & "-" & sQuestion
                    End If
                    aPostData.Add(sPKey, sResponse)
                End If
	    Next

            If Not String.IsNullOrEmpty(sResource) Then
                '-- Send the response data in an e-mail, specified in sResource
                Dim oContactMsg As New appxCMS.appxMessage
                oContactMsg.MessageArgs.Add("surveyname", sSurveyName)
                oContactMsg.MessageArgs.Add("responsedata", oSb.ToString)

                appxCMS.Messaging.SendEmail(oContactMsg, sResource, True)
            End If

            '-- Transmit the response data to a 3rd party CRM tool
            Dim sCRMConfigPath As String = Server.MapPath("/cmsimages/SurveyCRMConfig")
            Dim sCRMConfigFile As String = Path.Combine(sCRMConfigPath, "SurveyCRMConfig-" & SurveyId & ".xml")

            If File.Exists(sCRMConfigFile) Then

                Dim oCRMXML As New XmlDocument
                oCRMXML.Load(sCRMConfigFile)
                Dim oConfigs As XmlNodeList = oCRMXML.SelectNodes("//CRMConfig")

                For Each oConfig As XmlNode In oConfigs
                    Dim aSubmitData As Hashtable = CType(aPostData.Clone, Hashtable)

                    Dim sCRMAction As String = xmlhelp.ReadNode(oConfig.SelectSingleNode("FormAction"))
                    Dim sCRMMethod As String = xmlhelp.ReadNode(oConfig.SelectSingleNode("FormMethod"))
                    Dim sCRMHiddenFields As String = xmlhelp.ReadNode(oConfig.SelectSingleNode("HiddenFields"))
                    Dim sCRMFieldMapping As String = xmlhelp.ReadNode(oConfig.SelectSingleNode("FieldMapping"))


                    '-- Custom entries for Hubspot tracking
                    If sCRMAction.ToLower.Contains("hubspot") Then

                        If Not aSubmitData.ContainsKey("IPAddress") Then
                            aSubmitData.Add("IPAddress", Request.UserHostAddress)
                        End If


                        Dim currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()

                        'If in Production Mode
                        If (currentMode <> "dev") Then

                            'If cookie is found.
                            If Not Request.Cookies("hubspotutk") Is Nothing Then
                                If Not aSubmitData.ContainsKey("UserToken") Then
                                    aSubmitData.Add("UserToken", Request.Cookies("hubspotutk").Value)
                                Else
                                    aSubmitData("UserToken") = Request.Cookies("hubspotutk").Value
                                End If
                            End If


                        End If

                    End If
                    '-- End custom


                    If Not String.IsNullOrEmpty(sCRMAction) Then
                        If Not String.IsNullOrEmpty(sCRMHiddenFields) Then
                            Dim aHiddenFields() As String = sCRMHiddenFields.Split(New Char() {"|"})
                            For iHiddenField As Integer = 0 To aHiddenFields.Length - 1
                                Dim sHiddenField As String = aHiddenFields(iHiddenField)
                                Dim sFldName As String = sHiddenField.Substring(0, sHiddenField.IndexOf("=", StringComparison.Ordinal))
                                Dim sFldValue As String = sHiddenField.Substring(sHiddenField.IndexOf("=", StringComparison.Ordinal) + 1)

                                If Not aSubmitData.ContainsKey(sFldName) Then
                                    aSubmitData.Add(sFldName, sFldValue)
                                End If
                            Next
                        End If

                        If Not String.IsNullOrEmpty(sCRMFieldMapping) Then
                            Dim aFldMaps() As String = sCRMFieldMapping.Split(New Char() {"|"})
                            For iFldMap As Integer = 0 To aFldMaps.Length - 1
                                Dim sFldMap As String = aFldMaps(iFldMap)
                                Dim sFldOName As String = sFldMap.Substring(0, sFldMap.IndexOf("=", StringComparison.Ordinal))
                                Dim sFldTName As String = sFldMap.Substring(sFldMap.IndexOf("=", StringComparison.Ordinal) + 1)
                                If aSubmitData.ContainsKey(sFldOName) Then
                                    Dim sVal As String = aPostData(sFldOName).ToString()
                                    aSubmitData.Remove(sFldOName)
                                    aSubmitData.Add(sFldTName, sVal)
                                Else
                                    If Not aSubmitData.ContainsKey(sFldTName) Then
                                        aSubmitData.Add(sFldTName, "")
                                    End If
                                End If
                            Next
                        End If

                        'adds the URL of the form to the posted data
                        If Not aSubmitData.ContainsKey("pageurl") Then
                            aSubmitData.Add("pageurl", Request.Url.Host.ToString())
                        End If
                        'end adds the URL of the form to the posted data

                        If Log.IsInfoEnabled Then Log.Debug(sCRMAction)
                        Dim sResponse As String = appxCMS.Util.httpHelp.XMLURLPageRequest(sCRMAction, aSubmitData, sCRMMethod)
                        If Log.IsDebugEnabled Then Log.Debug(sResponse)

                        'If sCRMAction.ToLower.Contains("driven") And sCRMAction.ToLower.Contains("lead.ashx") Then
                        '    Dim LeadId As Integer
                        '    If Integer.TryParse(sResponse, LeadId) Then
                        '        Profile.DrivenCRMLeadId = LeadId
                        '    End If
                        'End If

                    Else
                        apphelp.LogErr(HttpContext.Current.User.Identity.Name, Page.AppRelativeVirtualPath, "Unable to transmit survey response to CRM system for survey ID " & SurveyId & " (CRM Action not defined)")
                    End If
                Next

                '-- Send the response data in an e-mail, specified in sResource
                Try
                    Dim oContactMsg As New appxCMS.appxMessage
                    oContactMsg.MessageArgs.Add("surveyname", sSurveyName)
                    oContactMsg.MessageArgs.Add("responsedata", oSb.ToString)
                   

                    appxCMS.Messaging.SendEmail(oContactMsg, "Survey Response Summary", True)
                Catch ex As Exception
                    Log.Error(ex.Message, ex)
                End Try
            Else
                apphelp.LogErr(HttpContext.Current.User.Identity.Name, Page.AppRelativeVirtualPath, "Unable to transmit survey response to CRM system for survey ID " & SurveyId & " (configuration file does not exist)")
            End If

            '-- Final action, redirect to landing url
            If sRedirect = "[confirmationtext]" Then
                Response.Redirect(linkHelp.SEOLink(sSurveyName, SurveyId.ToString(), linkHelp.LinkType.SurveyResponseConfirmation))
            Else
                Response.Redirect(sRedirect)
            End If

        Else
            RaiseEvent FailedValidation()
        End If

    End Sub


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        'jqueryHelper.Include(Page)

        'obsolete
        'jqueryHelper.RegisterStylesheet(Page, "~/app_styles/vpage_survey.css")

        Dim oJsSb As New StringBuilder
        oJsSb.AppendLine("function showFldReq(id) {")
        'oJsSb.AppendLine("    jQuery('#' + id).css('visibility', 'visible').show();")
        oJsSb.AppendLine("}")
        jqueryHelper.RegisterClientScript(Page, "ShowFldReq", oJsSb.ToString)

    End Sub


End Class
