Imports log4net
Imports System.Collections.Generic
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports SiteUtility
Imports System.Web.Script.Serialization
Imports System.Net

Partial Class account_signin
    Inherits appxCMS.PageBase


    'Fields
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()
    Protected SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
    Protected SiteDetails As SiteUtility.SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)




    'Methods
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        Dim SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()


        If appxCMS.Util.Querystring.GetString("adminaction") = "autologon" Then
            Dim sUser As String = appxCMS.Util.Querystring.GetString("u")
            Dim sPass As String = appxCMS.Util.Querystring.GetString("p")

            Dim bLogon As Boolean = False

            '-- Set the Destination (if bt querystring exists)
            Dim sRedirect As String = ""
            If HttpContext.Current.Request.QueryString("bt") IsNot Nothing Then
                sRedirect = HttpContext.Current.Request.QueryString("bt")
            ElseIf HttpContext.Current.Request.QueryString("ReturnUrl") IsNot Nothing Then
                sRedirect = HttpContext.Current.Request.QueryString("ReturnUrl")
            End If
            Dim oAuth As New Taradel.Auth
            oAuth.Authenticate(sUser, sPass, False, bLogon)
            If bLogon Then
                Response.Redirect(sRedirect)
            End If
        End If

        Dim oSite As appxCMS.Site = appxCMS.SiteDataSource.GetSite()
        If oSite IsNot Nothing Then
            Dim bEnableSignin As Boolean = True
            If oSite.EnableSignin.HasValue Then
                bEnableSignin = oSite.EnableSignin.Value
            End If
            Dim bEnableSignup As Boolean = True
            If oSite.EnableSignup.HasValue Then
                bEnableSignup = oSite.EnableSignup.Value
            End If

            If Not bEnableSignin Then
                pLogin.Visible = False
                Dim oSSOLogonPages As List(Of appxCMS.SiteSSOLogonPage) = appxCMS.SiteDataSource.GetSSOLogonPages(appxCMS.Util.CMSSettings.GetSiteId())
                If oSSOLogonPages.Count > 0 Then
                    pSSOLogin.Visible = True
                    lvSSOLogonPages.DataSource = oSSOLogonPages
                    lvSSOLogonPages.DataBind()
                End If
            End If

            If (bEnableSignup = False) Then

                pNativeRegister.Visible = False

                'Recenter the Login panel
                loginCol.Attributes.Remove("class")
                loginCol.Attributes.Add("class", "col-md-6 col-md-offset-3")

            End If


            'Hack.  Improve later.
            If ((SiteID = 93) Or (SiteID = 83)) Then

                phLoginForm.Visible = False

                'Recenter the Login panel
                registerCol.Attributes.Remove("class")
                registerCol.Attributes.Add("class", "col-sm-10 col-sm-offset-1")

            End If


        End If


    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            'Set as page level variable & obj. 11/9/2015. DSF
            'Dim SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
            'Dim SiteDetails As SiteUtility.SiteDetails
            'SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


            If Request.Cookies("AssociationCode") IsNot Nothing Then
                Dim sAssociationCode As String = Request.Cookies("AssociationCode").Value
            End If

            If Not (SiteDetails.ShowForgotPWDLink) Then
                hypForgotPWD.Visible = False
            End If

            If (SiteDetails.HideTaradelContent) Then
                phNewsletter.Visible = False
            End If

            LoadBusinessClasses()

            litLogInMsg.Text = SiteUtility.GetStringResourceValue(SiteID, "LogInMsg")

        End If
    End Sub



    Protected Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    End Sub



    Protected Sub LoadLogin(ByVal sender As Object, ByVal e As System.EventArgs)

        '-- Get login button and make it the default submit behavior
        Dim oLogon As Login = DirectCast(sender, Login)
        Dim oLoginButton As Control = Nothing

        If oLogon.LoginButtonType = ButtonType.Button Then
            oLoginButton = oLogon.FindControl("LoginButton")
        ElseIf oLogon.LoginButtonType = ButtonType.Image Then
            oLoginButton = oLogon.FindControl("LoginImageButton")
        End If

        If oLoginButton IsNot Nothing Then
            Dim sUID As String = oLoginButton.UniqueID.Replace(oLogon.NamingContainer.UniqueID, "")
            If sUID.StartsWith("$") Then
                sUID = sUID.Substring(1)
            End If
            Dim oNC As Control = oLogon.Parent
            Dim pLogin As Panel = DirectCast(oNC, Panel)
            pLogin.DefaultButton = sUID
        End If

    End Sub



    Protected Sub Login1_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate

        Dim sUser As String = Login1.UserName
        Dim sPass As String = Login1.Password
        Dim sLogonFailMsg As New StringBuilder
        Dim bLogon As Boolean = False


        sLogonFailMsg.Append("<div class=""alert alert-danger"">")
        sLogonFailMsg.Append("<span class=""fa fa-exclamation-circle""></span>&nbsp;")
        sLogonFailMsg.Append("The username or password is invalid. Please try again.")
        sLogonFailMsg.Append("</div>")


        '-- Set the Destination (if bt querystring exists)
        Dim sRedirect As String = ""
        If HttpContext.Current.Request.QueryString("bt") IsNot Nothing Then
            sRedirect = HttpContext.Current.Request.QueryString("bt")
        ElseIf HttpContext.Current.Request.QueryString("ReturnUrl") IsNot Nothing Then
            sRedirect = HttpContext.Current.Request.QueryString("ReturnUrl")
        End If

        If Not String.IsNullOrEmpty(sRedirect) Then
            Login1.DestinationPageUrl = sRedirect
        Else
            Login1.DestinationPageUrl = "~/MyAccount/account_manage.aspx"
        End If

        Dim oAuth As New Taradel.Auth

        oAuth.Authenticate(sUser, sPass, False, bLogon)
        Login1.FailureText = sLogonFailMsg.ToString()
        e.Authenticated = bLogon

        'Staples API -- 9/15/2015 '
        Dim chkUseStaples As CheckBox = DirectCast(sender.FindControl("chkUseStaples"), CheckBox)
        Dim UserName As TextBox = DirectCast(sender.FindControl("UserName"), TextBox)
        Dim Password2 As TextBox = DirectCast(sender.FindControl("Password"), TextBox)

        If chkUseStaples IsNot Nothing AndAlso chkUseStaples.Checked Then
            Dim sCRMAction As String = "http://eddm.redesign.eddmsite.com/Resources/StaplesAPI.ashx"
            Dim sCRMMethod As String = "POST"
            Dim aSubmitData As Hashtable = New Hashtable()
            aSubmitData.Add("logonId", UserName.Text) '"rzsmith@hotmail.com")
            aSubmitData.Add("logonPassword", Password2.Text)
            Dim sResponse As String = appxCMS.Util.httpHelp.XMLURLPageRequest(sCRMAction, aSubmitData, sCRMMethod)
            Dim jss = New JavaScriptSerializer()
            Dim data = jss.Deserialize(Of RootObject2)(sResponse)
            Response.Write(sResponse)

            'For Each member As Member In data.Member
            '    'gvTestAddress.DataSource = member.address
            '    'gvTestAddress.DataBind()
            'Next


            'Dim sResponse As String = appxCMS.Util.httpHelp.XMLURLPageRequest(sCRMAction, aSubmitData, sCRMMethod)
            'Dim jss = New JavaScriptSerializer()
            'Dim data As RootObject2 = jss.Deserialize(Of RootObject2)(sResponse)

            'For Each member As Member In data.Member
            '    e.Authenticated = True 'added 9/15/2015

            '    For Each adx As Address In member.address
            '        EmailAddress.Text = UserName.Text
            '        FirstName.Text = adx.firstname
            '        LastName.Text = adx.lastname
            '        DirectLine.Text = adx.phone1
            '        Address1.Text = adx.address1
            '        Address2.Text = adx.address2
            '        City.Text = adx.city
            '        State.SelectedValue = adx.state
            '        PostalCode.Text = adx.zipcode
            '        Company.Text = adx.companyName
            '    Next
            'Next
        End If



        'end Staples API -- 9/15/2015




    End Sub



    Protected Sub Login1_LoggedIn(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoggedIn
        Dim sRedirect As String = Login1.DestinationPageUrl 'HttpContext.Current.Request.QueryString("bt")
        If String.IsNullOrEmpty(sRedirect) Then
            '-- We need to rebuild the full URL of the page
            sRedirect = apphelp.GetRequestedURL 'Page.AppRelativeVirtualPath 'HttpContext.Current.Request.RawUrl
        End If
        HttpContext.Current.Response.Redirect(sRedirect)
    End Sub



    Private Function GetParentForm(ByVal parent As Control) As Control
        Dim parent_control As Control = TryCast(parent, Control)
        '------------------------------------------------------------------------
        'Specific to a control means if you want to find only for certain control
        'If TypeOf parent_control Is myControl Then   'myControl is of UserControl
        '    Return parent_control
        'End If
        '------------------------------------------------------------------------
        If parent_control.Parent Is Nothing Then
            Return parent_control
        End If
        If parent IsNot Nothing Then
            Return GetParentForm(parent.Parent)
        End If
        Return Nothing
    End Function



    Function RecursiveFindControl(ByVal Root As Control, ByVal Name As String)
        'Response.Write(Root.ID & "|")
	If Root.ID = Name Then
            Return Root
        End If
        Dim ctl As Control
        For Each ctl In Root.Controls
            If (Not IsDBNull(RecursiveFindControl(ctl, Name))) Then
                Return RecursiveFindControl(ctl, Name)
            End If
        Next
        Return DBNull.Value
    End Function



    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click

        Dim chkUseStaples As New CheckBox()
        chkUseStaples.Checked = False

        If appxCMS.Util.CMSSettings.GetSiteId() = 93 Then
            chkUseStaples.Checked = True
        End If

        Dim login2 As Login = DirectCast(RecursiveFindControl(Page, "Login1"), Login)
        Dim UserName As TextBox = DirectCast(RecursiveFindControl(login2, "UserName"), TextBox)



        Try
            If chkUseStaples.Checked Then
                Dim sCRMAction As String = "http://eddm.redesign.eddmsite.com/Resources/StaplesAPI.ashx"
                Dim sCRMMethod As String = "POST"
                Dim aSubmitData As Hashtable = New Hashtable()
                aSubmitData.Add("logonId", UserName.Text)
                aSubmitData.Add("logonPassword", Password.Text)
                Dim sResponse As String = appxCMS.Util.httpHelp.XMLURLPageRequest(sCRMAction, aSubmitData, sCRMMethod)
                'Response.Write(sResponse)
                Dim jss = New JavaScriptSerializer()
                Dim data As RootObject2 = jss.Deserialize(Of RootObject2)(sResponse)

                For Each member As Member In data.Member
                    For Each adx As Address In member.address
                        EmailAddress.Text = UserName.Text
                        FirstName.Text = adx.firstname
                        LastName.Text = adx.lastname
                        DirectLine.Text = adx.phone1
                        Address1.Text = adx.address1
                        Address2.Text = adx.address2
                        City.Text = adx.city
                        State.SelectedValue = adx.state
                        PostalCode.Text = adx.zipcode
                        Company.Text = adx.companyName
                    Next
                Next
            End If

        Catch ex As Exception
            'eat the exception - user not in Staples
            'revisit logic when time 10/15/2015
        End Try




        '-- Add new contact data to local database
        Dim sRedirect As String = "/Account-Welcome"
        Dim sBizClass As String = ddlIndustry.SelectedValue
        Dim iBizClass As Integer = 0
        Integer.TryParse(sBizClass, iBizClass)
        Dim bExists As Boolean = False
        Dim iContactId As Integer = 0
        Dim sReturnURL As String = QStringToVal("ReturnUrl")
        Dim oCust As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(EmailAddress.Text)

        If oCust IsNot Nothing Then
            bExists = True
        End If

        If Not bExists Then
            iContactId = Taradel.CustomerDataSource.Add(appxCMS.Util.CMSSettings.GetSiteId, EmailAddress.Text, Password.Text, FirstName.Text, LastName.Text, _
                                                        EmailAddress.Text, DirectLine.Text, DirectLineExt.Text, Address1.Text, Address2.Text, City.Text, _
                                                        State.SelectedValue, PostalCode.Text, False, Eletter.Checked, iBizClass, Company.Text)

            If iContactId > 0 Then
                '-- If ad agency then give them 5% discount on order and 5% on shipping
                '-- If newspaper then give them 100% off shipping (free shipping)
                Dim bEnableDiscount As Boolean = False
                Dim bDiscountPercent As Boolean = False
                Dim dDiscountAmount As Decimal = 0
                Dim bEnableShipping As Boolean = False
                Dim bShipPercent As Boolean = False
                Dim dShipAmount As Decimal = 0

                Dim oBizClass As Taradel.BusinessClass = Taradel.BusinessClassDataSource.GetBusinessClass(iBizClass)
                If oBizClass IsNot Nothing Then
                    sBizClass = oBizClass.Name
                End If

                Taradel.CustomerDataSource.SaveCustomerSettings(iContactId, iBizClass, sBizClass, bEnableDiscount, dDiscountAmount, bDiscountPercent, False, bEnableShipping, dShipAmount, bShipPercent)
            End If


            If Request.Cookies("Staff") Is Nothing Then
                If Request.Cookies("RefSource") IsNot Nothing Then
                    If Not String.IsNullOrEmpty(Request.Cookies("RefSource").Value) Then
                        Taradel.CustomerDataSource.SaveCustomerProperty(iContactId, "ReferralSource", Request.Cookies("RefSource").Value, "")
                    End If
                End If
            End If




            '=== HUBSPOT ================================================================================================
            'The code here is identical to HubSpot logic in Step1-TargetReview.aspx.vb.  Should look into centralizing this logic.

            If currentMode <> "dev" Then

                If (SiteDetails.EnableHubSpot) Then

                    Dim hsPortalID As String = ""
                    Dim hsFormID As String = ""
                    Dim hsCookieValue As String = ""
                    Dim pageTitle As String = ""

                    'Only EDDM and TaradelDM should be using this code
                    Try
                        Select Case appxCMS.Util.CMSSettings.GetSiteId()

                            Case 1
                                hsPortalID = "212947"
                                hsFormID = "698d02fd-d517-42ca-bc6d-54a0f3638d51"
                                pageTitle = "EveryDoorDirectMail.com Account Register"

                            Case 100
                                hsPortalID = "101857"
                                hsFormID = "33e61670-ee30-4ad8-9c55-be76f536d42b"
                                pageTitle = "Taradel Direct Mail Account Register"

                            Case Else
                                hsPortalID = "212947"
                                hsFormID = "698d02fd-d517-42ca-bc6d-54a0f3638d51"
                                pageTitle = "Unknown page Account Register"

                        End Select



                        'get and populate the HubSpot object.
                        Dim hubSpotObj As New HubSpot()
                        hubSpotObj.firstName = Trim(FirstName.Text)
                        hubSpotObj.lastName = Trim(LastName.Text)
                        hubSpotObj.companyName = Trim(Company.Text)
                        hubSpotObj.emailAddress = Trim(EmailAddress.Text)
                        hubSpotObj.phoneNumber = Trim(DirectLine.Text)
                        hubSpotObj.address = Trim(Address1.Text)
                        hubSpotObj.address2 = Trim(Address2.Text)
                        hubSpotObj.city = Trim(City.Text)
                        hubSpotObj.state = State.SelectedValue
                        hubSpotObj.zipCode = Trim(PostalCode.Text)
                        hubSpotObj.industry = sBizClass
                        hubSpotObj.hsPortalID = hsPortalID
                        hubSpotObj.hsFormGUID = hsFormID
                        hubSpotObj.pageTitle = pageTitle
                        hubSpotObj.pageURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri
                        hubSpotObj.ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress

                        'Look for previous cookie and retrieve that.
                        If HttpContext.Current.Request.Cookies("hubspotutk") IsNot Nothing Then
                            hsCookieValue = HttpContext.Current.Request.Cookies("hubspotutk").Value
                        End If


                        'Post to HubSpot.  Read results.
                        Dim postResults As String = hubSpotObj.SendLeadData(hubSpotObj)


                        If (postResults = "OK") Then
                            'Post to HubSpot was successful.  Carry on.
                        Else
                            'Send email
                            EmailUtility.SendAdminEmail("There was an error sending Contact Data to HubSpot.  Check the EDDM-Log.  HubSpot response was: " & postResults & ".")
                        End If


                    Catch ex As Exception
                        EmailUtility.SendAdminEmail("There was an error attempting to send Contact Data to HubSpot.  Error was: " & ex.ToString() & ".")
                    End Try


                    '===================================================================
                    'Original code
                    'Try
                    'Dim oHubPost As New Hashtable
                    'oHubPost.Add("firstname", FirstName.Text)
                    'oHubPost.Add("lastname", LastName.Text)
                    'oHubPost.Add("email", EmailAddress.Text)
                    'oHubPost.Add("company", Company.Text)
                    'oHubPost.Add("phone", DirectLine.Text)
                    'oHubPost.Add("address", Address1.Text)
                    'oHubPost.Add("address2", Address2.Text)
                    'oHubPost.Add("city", City.Text)
                    'oHubPost.Add("state", State.SelectedValue)
                    'oHubPost.Add("zip", PostalCode.Text)
                    'oHubPost.Add("industry", sBizClass)

                    '    Taradel.Hubspot.SendLeadData("698d02fd-d517-42ca-bc6d-54a0f3638d51", oHubPost)
                    'Catch ex As Exception
                    '    log.Error(ex.Message, ex)
                    '    EmailUtility.SendAdminEmail("ERROR - There was an error trying to send to Hubspot in account_signin.aspx. Error was :<br />" & ex.Message)
                    'End Try
                    '===================================================================


                End If

            End If




            '=== SALESFORCE ================================================================================================
            If currentMode <> "dev" Then

                If appxCMS.Util.CMSSettings.GetSetting("SalesForce", "DoNotPost") = "True" Then

                Else
                    Try
                        Dim sCRMAction As String = "http://sforceapi.taradel.com/SalesForceHandlerProd.ashx"
                        Dim sCRMMethod As String = "POST"

                        Dim aSubmitData As Hashtable = New Hashtable()
                        aSubmitData.Add("firstname", FirstName.Text)
                        aSubmitData.Add("lastname", LastName.Text)
                        aSubmitData.Add("email", EmailAddress.Text)
                        aSubmitData.Add("company", Company.Text)
                        aSubmitData.Add("phone", DirectLine.Text)
                        aSubmitData.Add("address", Address1.Text & " " & Address2.Text)
                        aSubmitData.Add("city", City.Text)
                        aSubmitData.Add("state", State.SelectedValue)
                        aSubmitData.Add("zip", PostalCode.Text)
                        aSubmitData.Add("businesstype", ddlBusinessType.SelectedItem.Text)
                        aSubmitData.Add("industry", ddlIndustry.SelectedItem.Text)
                        aSubmitData.Add("source", "EDDM Account SignIn")
                        aSubmitData.Add("salesforcesubject", "Account SignUp")
                        aSubmitData.Add("pageurl", Request.Url.ToString())
                        aSubmitData.Add("SiteDescription", SiteDetails.SiteDescription)

                        Dim sResponse As String = appxCMS.Util.httpHelp.XMLURLPageRequest(sCRMAction, aSubmitData, sCRMMethod)
                    Catch ex As Exception
                        log.Error(ex.Message, ex)
                    End Try
                End If

            End If
            '===========================================================================================================




            '-- Authenticate the user
            Dim bAuth As Boolean = False
            Dim sAuthMsg As String = ""
            Dim oAuth As New Taradel.Auth

            oAuth.Authenticate(EmailAddress.Text, Password.Text, False, bAuth, sAuthMsg)





            'TEMP - Refactor.
            '-- Send an e-mail welcoming them
            Dim sEmail As String = EmailAddress.Text
            Dim oMsg As New appxCMS.appxMessage
            oMsg.MessageArgs.Add("emailaddress", sEmail)
            oMsg.MessageArgs.Add("username", sEmail)
            oMsg.MessageArgs.Add("password", Password.Text)



            'FROM [appxEmailTemplate]
            Dim oTemplate As appxCMS.EmailTemplate = Nothing
            Dim SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId



            Select Case SiteID
                Case 78 'Act Mgr
                    oTemplate = appxCMS.EmailTemplateDataSource.GetEmailTemplate("Staples Act Mgr Signup Welcome Message", True)

                Case 91 'Store
                    oTemplate = appxCMS.EmailTemplateDataSource.GetEmailTemplate("Staples Store Signup Welcome Message", True)

                Case 93 'EDDM
                    oTemplate = appxCMS.EmailTemplateDataSource.GetEmailTemplate("Staples EDDM Signup Welcome Message", True)

                Case Else
                    oTemplate = appxCMS.EmailTemplateDataSource.GetEmailTemplate("Account Signup Welcome Message", True)
            End Select


            If oTemplate IsNot Nothing Then
                oMsg.Recipient = oTemplate.ToAddress
                oMsg.SendFrom = oTemplate.FromAddress
                oMsg.ReplyTo = oTemplate.ReplyToAddress
                oMsg.CCList = oTemplate.CCList
                oMsg.BCCList = oTemplate.BCCList
                oMsg.SendHTML = oTemplate.IsHTML
                oMsg.Subject = oTemplate.Subject
                oMsg.Body = oTemplate.Body
                oMsg.SendMail()
            End If




            'REDO this
            'For some reason, the above code is not firing an email (in DEV) so this could be a temp fix.


            'EmailUtility.SendRegisterConfirmEmail(siteID, site, sEmail, FirstName.Text, LastName.Text, baseURL)
            'End of temp



            'Dim siteID As Integer = appxCMS.Util.CMSSettings.GetSiteId
            'Dim site As String = ""
            'Dim baseURL As String = ""

            'Select Case siteID
            '    Case 1
            '        site = "EveryDoorDirectMail.com"
            '        baseURL = "http://www.everydoordirectmail.com"
            '    Case 11
            '        site = "Outdoor Living Brands EDDM"
            '        baseURL = "http://olb.eddmsite.com"
            '    Case 41
            '        site = "FedEx Office EDDM"
            '        baseURL = "http://fedexoffice.everydoordirectmail.com"
            '    Case 78
            '        site = "Staples Every Door Direct Mail"
            '        baseURL = "http://staples.redesign.eddmsite.com"
            '    Case 89
            '        site = "New Mexico Marketplace Direct Mail"
            '        baseURL = "http://staples.redesign.eddmsite.com"
            '    Case Else
            '        site = "EveryDoorDirectMail.com"
            'End Select

            'EmailUtility.SendRegisterConfirmEmail(siteID, site, sEmail, FirstName.Text, LastName.Text, baseURL)
            'End of temp



            If Not String.IsNullOrEmpty(sReturnURL) Then
                Response.Redirect(sRedirect & "?ReturnUrl=" & Server.UrlEncode(sReturnURL))
            Else
                Response.Redirect(sRedirect)
            End If

        Else
            Response.Redirect("~/account_exists.aspx?ReturnUrl=" & Server.UrlEncode(sReturnURL))
        End If

    End Sub



    Private Sub LoadBusinessClasses()

        Dim objConnection As SqlConnection
        Dim getData As SqlCommand
        Dim drData As SqlDataReader
        Dim connectString As String = ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString
        Dim selectSQL As String = "EXEC usp_GetBusinessClassesForSignUp"
        Dim selectOne As New ListItem()
        Dim errorMsg As New StringBuilder()

        objConnection = New SqlConnection(connectString)
        getData = New SqlCommand(selectSQL, objConnection)


        Try

            objConnection.Open()
            drData = getData.ExecuteReader()
            ddlIndustry.DataSource = drData
            ddlIndustry.DataValueField = "BusinessClassID"
            ddlIndustry.DataTextField = "BusinessClass"
            selectOne.Value = ""
            selectOne.Text = "Select One"
            ddlIndustry.DataBind()
            ddlIndustry.Items.Insert(0, selectOne)
            drData.Close()

        Catch exceptionObj As Exception

            errorMsg.Append("The following errors occurred:<br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " & exceptionObj.Message & "</li>")
            errorMsg.Append("<li>Source: " & exceptionObj.Source & "<li>")
            errorMsg.Append("<li>Stack Trace: " & exceptionObj.StackTrace & "<li>")
            errorMsg.Append("<li>Target Site: " & exceptionObj.TargetSite.Name & "<li>")
            errorMsg.Append("<li>SQL: " & selectSQL & "<li>")
            errorMsg.Append("</ul>")

            EmailUtility.SendAdminEmail("Error in LoadBusinessClasses. (account_signin.aspx). Details:<br />" & errorMsg.ToString())


        Finally
            objConnection.Close()
        End Try


    End Sub



    ''''''''''''''''''''''''''''''''''''''
    ''Staples LOG IN OBJECT

    Public Class RootObject
        Public Property WCToken() As String
            Get
                Return m_WCToken
            End Get
            Set
                m_WCToken = Value
            End Set
        End Property
        Private m_WCToken As String
        Public Property WCTrustedToken() As String
            Get
                Return m_WCTrustedToken
            End Get
            Set
                m_WCTrustedToken = Value
            End Set
        End Property
        Private m_WCTrustedToken As String
        Public Property personalizationID() As String
            Get
                Return m_personalizationID
            End Get
            Set
                m_personalizationID = Value
            End Set
        End Property
        Private m_personalizationID As String
        Public Property userId() As String
            Get
                Return m_userId
            End Get
            Set
                m_userId = Value
            End Set
        End Property
        Private m_userId As String
    End Class

    ''' <summary>
    Public Class Address
        Public Property address1() As String
            Get
                Return m_address1
            End Get
            Set
                m_address1 = Value
            End Set
        End Property
        Private m_address1 As String
        Public Property address2() As String
            Get
                Return m_address2
            End Get
            Set
                m_address2 = Value
            End Set
        End Property
        Private m_address2 As String
        Public Property addressId() As String
            Get
                Return m_addressId
            End Get
            Set
                m_addressId = Value
            End Set
        End Property
        Private m_addressId As String
        Public Property city() As String
            Get
                Return m_city
            End Get
            Set
                m_city = Value
            End Set
        End Property
        Private m_city As String
        Public Property companyName() As String
            Get
                Return m_companyName
            End Get
            Set
                m_companyName = Value
            End Set
        End Property
        Private m_companyName As String
        Public Property extensionNumber() As String
            Get
                Return m_extensionNumber
            End Get
            Set
                m_extensionNumber = Value
            End Set
        End Property
        Private m_extensionNumber As String
        Public Property firstname() As String
            Get
                Return m_firstname
            End Get
            Set
                m_firstname = Value
            End Set
        End Property
        Private m_firstname As String
        Public Property lastname() As String
            Get
                Return m_lastname
            End Get
            Set
                m_lastname = Value
            End Set
        End Property
        Private m_lastname As String
        Public Property phone1() As String
            Get
                Return m_phone1
            End Get
            Set
                m_phone1 = Value
            End Set
        End Property
        Private m_phone1 As String
        Public Property state() As String
            Get
                Return m_state
            End Get
            Set
                m_state = Value
            End Set
        End Property
        Private m_state As String
        Public Property zipcode() As String
            Get
                Return m_zipcode
            End Get
            Set
                m_zipcode = Value
            End Set
        End Property
        Private m_zipcode As String
    End Class

    Public Class Member
        Public Property address() As List(Of Address)
            Get
                Return m_address
            End Get
            Set
                m_address = Value
            End Set
        End Property
        Private m_address As List(Of Address)
    End Class

    Public Class RootObject2
        Public Property Member() As List(Of Member)
            Get
                Return m_Member
            End Get
            Set
                m_Member = Value
            End Set
        End Property
        Private m_Member As List(Of Member)
    End Class

    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================
    ''end Staples LOG IN OBJECT


End Class
