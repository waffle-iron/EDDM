Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq
Imports log4net

' ReSharper disable InconsistentNaming
Partial Class vpage_survey
    ' ReSharper restore InconsistentNaming
    Inherits appxCMS.PageBase

    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected ReadOnly Property SurveyId() As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("id")
        End Get
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

    Private _submitButtonText As String = "Submit"
    Protected Property SubmitButtonText As String
        Get
            Return _submitButtonText
        End Get
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                _submitButtonText = value
            End If
        End Set
    End Property

    Protected CanValidateList As New ArrayList
    Protected oJS As New StringBuilder
    Protected ValidationGroup As String = "validSurvey"
    Protected bHasButton As Boolean = False

    Protected Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        CanValidateList.Add("text")
        CanValidateList.Add("memo")
        CanValidateList.Add("email")
        CanValidateList.Add("phone")
        CanValidateList.Add("number")
        CanValidateList.Add("selectlist")

        Dim bNumberQ As Boolean = True
        Dim oSurvey As appxCMS.SurveyHeader = appxCMS.SurveyDataSource.GetSurvey(SurveyId)
        If oSurvey IsNot Nothing Then
            Page.Title = oSurvey.SurveyName
            SurveyPreText = oSurvey.PreText
            SurveyPostText = oSurvey.PostText
            If Not String.IsNullOrEmpty(oSurvey.SubmitButtonText) Then
                btnSave.Text = oSurvey.SubmitButtonText
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

                oPH.Controls.Add(New LiteralControl("<div class=""surveyLabel"">" & sQuestion))
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
                            oFld.Style.Add("width", "80%")
                        End If

                        oPH.Controls.Add(oFld)

                    Case "memo"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
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
                        If iColumns = 0 AndAlso String.IsNullOrEmpty(sClass) Then
                            oFld.Style.Add("width", "80%")
                        End If
                        oPH.Controls.Add(oFld)

                    Case "email"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
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
                            oFld.Style.Add("width", "80%")
                        End If
                        oPH.Controls.Add(oFld)
                        Dim oRegExpValidator As New RegularExpressionValidator
                        oRegExpValidator.ID = "fldinput" & oQuestion.SurveyQuestionID
                        oRegExpValidator.ControlToValidate = "fld" & oQuestion.SurveyQuestionID
                        oRegExpValidator.ValidationGroup = ValidationGroup
                        oRegExpValidator.Text = " !"
                        oRegExpValidator.ErrorMessage = "Your e-mail address is not formatted properly."
                        oRegExpValidator.EnableClientScript = True
                        oRegExpValidator.ValidationExpression = "([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})"
                        oRegExpValidator.Display = ValidatorDisplay.Dynamic
                        'oPH.Controls.Add(oRegExpValidator)
                        oAddOnValidators.Add(oRegExpValidator)

                    Case "phone"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
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
                        oRegExpValidator.Text = " !"
                        oRegExpValidator.ErrorMessage = "The phone number is not formatted properly. Please be sure to include your area code."
                        oRegExpValidator.EnableClientScript = True
                        oRegExpValidator.ValidationExpression = "\(?\d{3}\)?(\s|-|\.)?\d{3}(-|\s|\.)?\d{4}"
                        oRegExpValidator.Display = ValidatorDisplay.Dynamic
                        'oPH.Controls.Add(oRegExpValidator)
                        oAddOnValidators.Add(oRegExpValidator)

                    Case "number"
                        Dim oFld As New TextBox
                        oFld.ID = "fld" & oQuestion.SurveyQuestionID
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
                        oRegExpValidator.Text = " !"
                        oRegExpValidator.ErrorMessage = "A numeric value is required in this field."
                        oRegExpValidator.EnableClientScript = True
                        oRegExpValidator.ValidationExpression = "(\+|-)?[0-9\,\.]+"
                        oRegExpValidator.Display = ValidatorDisplay.Dynamic
                        'oPH.Controls.Add(oRegExpValidator)
                        oAddOnValidators.Add(oRegExpValidator)

                        If oQuestion.MinValue.HasValue Or oQuestion.MaxValue.HasValue Then
                            Dim iMinVal As Integer = 0
                            If oQuestion.MinValue.HasValue Then
                                iMinVal = oQuestion.MinValue.Value
                            End If
                            Dim iMaxVal As Integer = 9999999
                            If oQuestion.MaxValue.HasValue Then
                                iMaxVal = oQuestion.MaxValue.Value
                            End If

                            '-- Range validator for this field
                            Dim oRangeFldValidator As New RangeValidator
                            oRangeFldValidator.ID = "fldrange" & oQuestion.SurveyQuestionID
                            oRangeFldValidator.ValidationGroup = ValidationGroup
                            oRangeFldValidator.ControlToValidate = "fld" & oQuestion.SurveyQuestionID
                            oRangeFldValidator.MinimumValue = iMinVal.ToString()
                            oRangeFldValidator.MaximumValue = iMaxVal.ToString()
                            oRangeFldValidator.Text = " !"
                            oRangeFldValidator.Type = ValidationDataType.Integer
                            oRangeFldValidator.ErrorMessage = "You must enter a value between " & iMinVal.ToString("N0") & " and " & iMaxVal.ToString("N0") & " for the question '" & sQuestion & "'."
                            oRangeFldValidator.Display = ValidatorDisplay.Dynamic
                            oAddOnValidators.Add(oRangeFldValidator)
                        End If

                    Case "checkboxlist"
                        If Not String.IsNullOrEmpty(oQuestion.ResponseOptions.Trim()) Then
                            Dim oFld As New CheckBoxList
                            oFld.ID = "fld" & oQuestion.SurveyQuestionID
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
                    oReqFldValidator.Text = " (*****)"
                    oReqFldValidator.ErrorMessage = sVMsg
                    oReqFldValidator.Display = ValidatorDisplay.Dynamic
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
                    '-- Process embedded controls
                    oPH.Controls.Add(DirectCast(Page, appxCMS.PageBase).FormatContent(Me.SurveyPreText))
                    'New LiteralControl())
                Case "surveypost-text"
                    '-- Process embedded controls
                    oPH.Controls.Add(DirectCast(Page, appxCMS.PageBase).FormatContent(Me.SurveyPostText))
                    'oPH.Controls.Add(New LiteralControl(Me.SurveyPostText))
                Case "submitbutton"
                    Dim oLnk As New appxCMS.Web.SingleClickButton
                    oLnk.ID = "surveySubmitSingleClickBtn"
                    oLnk.Text = SubmitButtonText
                    oLnk.ValidationGroup = ValidationGroup
                    oLnk.ClickedText = "Processing..."
                    oLnk.ShowProcessingModal = True
                    oLnk.ProcessingModalTitle = "Saving Your Response"
                    oLnk.ProcessingModalHtml = "<div style=""text-align:center;""><img src=""/cmsimages/indicator.gif"" width=""16"" height=""16""/> <strong>Processing...</strong></div>"
                    AddHandler oLnk.Click, AddressOf btnSave_Click
                    oLnk.CssClass = "makeButton"
                    oPH.Controls.Add(oLnk)
                    '-- We are always adding the single click button now
                    'bHasButton = True
                    'Dim oLnk As New LinkButton
                    'oLnk.Text = SubmitButtonText
                    'AddHandler oLnk.Click, AddressOf btnSave_Click
                    'oLnk.CssClass = "makeButton"
                    'oPH.Controls.Add(oLnk)
            End Select
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnSave.Click
        Page.Validate(btnSave.ValidationGroup)
        If Not Page.IsValid Then
            Exit Sub
        End If

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
                    If Not aSubmitData.ContainsKey("UserToken") Then
                        aSubmitData.Add("UserToken", Request.Cookies("hubspotutk").Value)
                    Else
                        aSubmitData("UserToken") = Request.Cookies("hubspotutk").Value
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
                    If Not aSubmitData.ContainsKey("xURL") Then
                        aSubmitData.Add("xURL", Request.Url.Host.ToString())
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
                    apphelp.LogErr(User.Identity.Name, Page.AppRelativeVirtualPath, "Unable to transmit survey response to CRM system for survey ID " & SurveyId & " (CRM Action not defined)")
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
            apphelp.LogErr(User.Identity.Name, Page.AppRelativeVirtualPath, "Unable to transmit survey response to CRM system for survey ID " & SurveyId & " (configuration file does not exist)")
        End If

        '-- Final action, redirect to landing url
        If sRedirect = "[confirmationtext]" Then
            Response.Redirect(appxCMS.SEO.Rewrite.BuildLink(sSurveyName, SurveyId.ToString(), "surveyresponse"))
        Else
            Response.Redirect(sRedirect)
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        jqueryHelper.Include(Page)
        jqueryHelper.RegisterStylesheet(Page, "~/app_styles/vpage_survey.css")
        Dim oJsSb As New StringBuilder
        oJsSb.AppendLine("function showFldReq(id) {")
        'oJsSb.AppendLine("    jQuery('#' + id).css('visibility', 'visible').show();")
        oJsSb.AppendLine("}")
        jqueryHelper.RegisterClientScript(Page, "ShowFldReq", oJsSb.ToString)
    End Sub
End Class
