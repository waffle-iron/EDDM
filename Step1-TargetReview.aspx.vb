Imports System.Collections.Generic
Imports System.Linq
Imports Taradel.EF
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization



Partial Class Step1_TargetReview

    Inherits appxCMS.PageBase

    '=======================================================================================================================================
    '   NOTES: 
    '=======================================================================================================================================


    'Fields
    Protected debug As Boolean = False
    Protected currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()
    Protected SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
    Protected SiteDetails As SiteUtility.SiteDetails = Nothing



    'Properties
    Protected ReadOnly Property DistributionId() As Integer
        Get
            Return QStringToInt("distid")
        End Get
    End Property


    Private _USelectID As Integer = 0
    Public Property USelectID As Integer
        Get
            Return _USelectID
        End Get
        Set(value As Integer)
            _USelectID = value
        End Set

    End Property


    Private _EDDMMap As Boolean = False
    Public Property EDDMMap As Boolean
        Get
            Return _EDDMMap
        End Get
        Set(value As Boolean)
            _EDDMMap = value
        End Set

    End Property


    Private _AddressedMap As Boolean = False
    Public Property AddressedMap As Boolean
        Get
            Return _AddressedMap
        End Get
        Set(value As Boolean)
            _AddressedMap = value
        End Set

    End Property


    Private _UploadedList As Boolean = False
    Public Property UploadedList As Boolean
        Get
            Return _UploadedList
        End Get
        Set(value As Boolean)
            _UploadedList = value
        End Set

    End Property


    Private _Quantity As Integer = 0
    Public Property Quantity As Integer

        Get
            Return _Quantity
        End Get
        Set(value As Integer)
            _Quantity = value
        End Set

    End Property


    Private _ReferenceID As String = ""
    Public Property ReferenceID As String
        Get
            Return _ReferenceID
        End Get
        Set(value As String)
            _ReferenceID = value
        End Set

    End Property





    'Methods
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        Dim redirectIfSingleProductURL As String = String.Empty
        Dim CategoryId As Integer = 0

        If DistributionId > 0 Then


            'Map img as a hyperlink
            hypGoToMap.NavigateUrl = "~/Step1-Target.aspx?distid=" & DistributionId


            Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistributions.GetDistribution(Me.DistributionId)


            'Set Details Obj
            SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


            'Set Page Header control
            If Not (SiteDetails.UseRibbonBanners) Then
                PageHeader.headerType = "simple"
                PageHeader.simpleHeader = "Choose your product"
            Else
                PageHeader.headerType = "partial"
                PageHeader.mainHeader = "Print"
                PageHeader.subHeader = "Choose your product"
            End If


            If oDist IsNot Nothing Then

                USelectID = oDist.USelectMethodReference.ForeignKey()

                Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(Me.USelectID)

                If oUSelect IsNot Nothing Then

                    Dim productTypes As String = ""

                    Using oDb As New Taradel.taradelEntities

                        Dim oSite As appxCMS.Site = appxCMS.SiteDataSource.GetSite(SiteID)

                        ReferenceID = DistributionUtility.RetrieveReferenceID(DistributionId)
                        Quantity = oDist.TotalDeliveries



                        'Modal settings
                        litSaveToContinueMsg.Text = SiteUtility.GetStringResourceValue(SiteID, "SaveToContinueMsg")
                        litSignInRegisterModalTitle.Text = SiteUtility.GetStringResourceValue(SiteID, "SignInRegisterModalTitle")


                        'If users are NOT allowed to register
                        If Not (oSite.EnableSignup) Then
                            pnlAccountTypes.Visible = False

                            'In some cases, we will want to also hide the forgot password links.
                            If Not (SiteDetails.ShowForgotPWDLink) Then
                                hypForgotPassword.Visible = False
                            End If

                        End If
                        'End of Modal



                        'Map Name
                        If Not String.IsNullOrEmpty(oDist.Name) Then
                            lSelectName.Text = oDist.Name
                        End If


                        'Tell rest of page how to operate
                        Dim CatalogCategoryToFind As String = String.Empty
                        'SET CatalogCategoryToFind  to the categories found here --> http://eddm.redesign.eddmsite.com/admin/Taradel/Catalog/Categories.aspx
                        Select Case USelectID


                            'EDDM
                            Case 1

                                EDDMMap = True
                                BuildEDDMTargetReview()
                                CatalogCategoryToFind = "eddm"

                            'User Uploaded List
                            Case 5
                                EDDMMap = False
                                UploadedList = True
                                AddressedMap = True
                                BuildUploadListTargetReview()
                                CatalogCategoryToFind = "uploaded"

                            'User Generated List
                            Case 6
                                EDDMMap = False
                                AddressedMap = True
                                BuildGeneratedListTargetReview()
                                CatalogCategoryToFind = "created"

                        End Select


                        Dim oCat As Taradel.WLCategory = (From c In oDb.WLCategories Where c.Name.ToLower.Contains(CatalogCategoryToFind) And c.appxSite.SiteId = SiteID).FirstOrDefault

                        If oCat IsNot Nothing Then
                            CategoryId = oCat.CategoryID
                        End If

                    End Using

                    'Pass the properties to the Product Control
                    ProdList.CategoryId = CategoryId
                    ProdList.DistributionId = DistributionId
                    ProdList.NavigateUrl = "~/Step2-ProductOptions.aspx?productid={0}&distid={1}&baseid={2}"
                    ProdList.USelectID = USelectID
                    ProdList.EDDMMap = EDDMMap
                    ProdList.AddressedMap = AddressedMap
                    ProdList.Quantity = Quantity


                    'Map image
                    lMapReview.Text = "<img src=""" & oUSelect.ReviewUrl & "?referenceid=" & oDist.ReferenceId & """ height=""250"" width=""100%""/>"

                End If

            End If


            If (debug) Then

                Dim debugStuff As New StringBuilder
                debugStuff.Append("PAGE PROPERTIES:<br />")
                debugStuff.Append("DistributionID: " & DistributionId & "<br />")
                debugStuff.Append("USelectId: " & USelectID & "<br />")
                debugStuff.Append("EDDMMap: " & CStr(EDDMMap) & "<br />")
                debugStuff.Append("AddressedMap: " & CStr(AddressedMap) & "<br />")
                debugStuff.Append("Quantity: " & Quantity & "<br />")
                debugStuff.Append("ReferenceID: " & ReferenceID & "<br />")
                'debugStuff.Append("CategooryID: " & CategoryID & "<br />")

                lblDebug.Text = debugStuff.ToString()
                pnlDebug.Visible = True

            End If

            'rs for Ramp Express 11/27/2015
            ''check for single product AND site setting to move on IF and ONLY IF true
            If Request.IsAuthenticated Then 'added this 12/2/2015 rs
                If SiteDetails.Step1TargetReviewRedirectIfSingleProduct Then
                    'replicating what the product control does - to verify that there is only one product
                    Dim oProds As List(Of Taradel.WLProductCategory) = Taradel.WLCategoryDataSource.GetProductsInCategory(CategoryId)
                    If oProds.Count = 1 Then
                        Dim oFirstProd As Taradel.WLProduct = oProds.FirstOrDefault.WLProduct
                        redirectIfSingleProductURL = "~/Step2-ProductOptions.aspx?productid=" & oFirstProd.ProductID & "&distid=" & Me.DistributionId & "&baseid=" & oFirstProd.BaseProductID
                        Response.Redirect(redirectIfSingleProductURL)
                    End If
                End If
            End If

        Else
            'get outta here...
            Response.Redirect("~/Default.aspx")
        End If

    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            'Used in Short Form Register modal
            LoadBusinessClasses()
            BuildOrderSteps()

            ddlBusinessType.Attributes.Add("onchange", "BusinessTypeChanged();")
            ddlIndustry.Attributes.Add("onchange", "IndustryChanged();")

        End If


    End Sub



    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

        If SiteDetails.ShowRegisterModal Then

            If Not Request.IsAuthenticated Then

                Dim oJs As New StringBuilder
                oJs.AppendLine("<script type=""text/javascript"">" + Environment.NewLine)
                oJs.Append("$(function () " + Environment.NewLine)
                oJs.Append("{" + Environment.NewLine)
                oJs.Append("   $('#registerModal').modal('show');" + Environment.NewLine)
                oJs.Append("});" + Environment.NewLine)
                oJs.Append("</script>" + Environment.NewLine)
                litRegistrationScript.Text = oJs.ToString()

            End If

        End If

    End Sub



    Protected Sub OpenMapForEdit(sender As Object, e As System.EventArgs)

        Dim oLnk As LinkButton = DirectCast(sender, LinkButton)

        If oLnk IsNot Nothing Then

            Dim sCmd As String = oLnk.CommandArgument
            Dim aCmd As String() = sCmd.Split("|")

            Dim sDistId As String = aCmd(0)
            Dim ReferenceId As String = aCmd(1)
            Dim DistributionId As Integer = 0
            Integer.TryParse(sDistId, DistributionId)

            Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(ReferenceId)

            If oSummary IsNot Nothing Then

                Dim sAddr As String = oSummary.StartAddress.ToString
                Dim sZip As String = oSummary.StartZipCode.ToString

                If String.IsNullOrEmpty(sAddr) Or String.IsNullOrEmpty(sZip) Then
                    Response.Redirect("~/Step1-Target.aspx?distid=" & DistributionId)
                Else
                    Response.Redirect("~/Step1-Target.aspx?distid=" & DistributionId & "&addr=" & Server.UrlEncode(sAddr) & "&zip=" & Server.UrlEncode(sZip))
                End If
            End If

        End If

    End Sub



    Protected Sub lnkSignIn_Click(sender As Object, e As System.EventArgs) Handles lnkSignIn.Click

        Dim sUser As String = EmailAddress.Text
        Dim sPass As String = AccountPass.Text
        Dim sConfirm As String = ConfirmPass.Text

        If radExisting.Checked Then

            Dim sLogonFailMsg As String = "The username or password is invalid. Please try again."
            Dim bLogon As Boolean = False

            '-- Set the Destination (if bt querystring exists)
            Dim oAuth As New Taradel.Auth
            oAuth.Authenticate(sUser, sPass, False, bLogon)
            If bLogon Then
                Response.Redirect(Request.RawUrl)
            Else
                lSignInMsg.Text = "<div class=""alert alert-danger""><span class=""fa fa-exclamation-circle""></span>&nbsp;" & sLogonFailMsg & "</div>"
            End If
        Else
            If sPass <> sConfirm Then
                lSignInMsg.Text = "<div class=""alert alert-danger""><span class=""fa fa-exclamation-circle""></span>&nbsp;The confirm password does not match.</div>"
                Exit Sub
            End If

            Dim bExists As Boolean = False
            Dim iContactId As Integer = 0
            '-- Creating a new account
            Dim oCust As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(EmailAddress.Text)

            If oCust IsNot Nothing Then
                bExists = True
            End If

            If Not bExists Then


                iContactId = Taradel.CustomerDataSource.Add(appxCMS.Util.CMSSettings.GetSiteId, sUser, sPass, txtFirstName.Text, txtLastName.Text, EmailAddress.Text, txtPhoneNumber.Text, txtExt.Text, "", "", "", "", "", False, Eletter.Checked, 0, txtCompanyName.Text)

                If Request.Cookies("Staff") Is Nothing Then
                    If Request.Cookies("RefSource") IsNot Nothing Then
                        If Not String.IsNullOrEmpty(Request.Cookies("RefSource").Value) Then
                            Taradel.CustomerDataSource.SaveCustomerProperty(iContactId, "ReferralSource", Request.Cookies("RefSource").Value, "")
                        End If
                    End If
                End If


                '=== HUBSPOT ================================================================================================
                'The code here is identical to HubSpot logic in account_signin.aspx.vb.  Should look into centralizing this logic.

                If currentMode <> "dev" Then

                    If (SiteDetails.EnableHubSpot) Then

                        Dim hsPortalID As String = ""
                        Dim hsFormID As String = ""
                        Dim hsCookieValue As String = ""
                        Dim pageTitle As String = ""

                        'Only EDDM and TaradelDM should be using this code
                        Try
                            Select Case SiteID

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
                            hubSpotObj.firstName = Trim(txtFirstName.Text)
                            hubSpotObj.lastName = Trim(txtLastName.Text)
                            hubSpotObj.companyName = Trim(txtCompanyName.Text)
                            hubSpotObj.emailAddress = Trim(EmailAddress.Text)
                            hubSpotObj.phoneNumber = Trim(txtPhoneNumber.Text)
                            hubSpotObj.industry = ddlBusinessType.SelectedItem.Text
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


                    End If

                End If



                '===================================================================
                'Original code
                'If currentMode <> "dev" Then

                '    If (SiteDetails.EnableHubSpot) Then
                '        Try
                '            Dim oHubPost As New Hashtable
                '            oHubPost.Add("email", EmailAddress.Text)

                '            Taradel.Hubspot.SendLeadData("698d02fd-d517-42ca-bc6d-54a0f3638d51", oHubPost)
                '        Catch ex As Exception
                '            EmailUtility.SendAdminEmail("Hubspot was not successful in getting updated/called. (Step1-TargetReview.aspx - lnkSignIn_Click)")
                '        End Try

                '    End If

                'End If
                '===========================================================================================================




                '=== SALESFORCE ================================================================================================
                If currentMode <> "dev" Then

                    Try

                        Dim sCRMAction As String = "http://sforceapi.taradel.com/SalesForceHandlerProd.ashx" 'could be config
                        Dim sCRMMethod As String = "POST"

                        Dim aSubmitData As Hashtable = New Hashtable()
                        aSubmitData.Add("firstname", txtFirstName.Text)
                        aSubmitData.Add("lastname", txtLastName.Text)
                        aSubmitData.Add("phone", txtPhoneNumber.Text)
                        aSubmitData.Add("company", txtCompanyName.Text)
                        aSubmitData.Add("email", EmailAddress.Text)
                        aSubmitData.Add("source", "EDDM Account SignIn")
                        aSubmitData.Add("salesforcesubject", "Account SignUp")
                        aSubmitData.Add("businesstype", ddlBusinessType.SelectedItem.Text)
                        aSubmitData.Add("industry", ddlIndustry.SelectedItem.Text)
                        aSubmitData.Add("pageurl", Request.Url.ToString())
                        aSubmitData.Add("SiteDescription", SiteDetails.SiteDescription)

                        Dim sResponse As String = appxCMS.Util.httpHelp.XMLURLPageRequest(sCRMAction, aSubmitData, sCRMMethod)

                    Catch ex As Exception
                        ''log.Error(ex.Message, ex)
                        EmailUtility.SendAdminEmail("SalesForce was not successful in getting updated/called. (Step1-TargetReview.aspx - lnkSignIn_Click)")
                    End Try

                End If
                '===========================================================================================================


                '-- Authenticate the user
                Dim bAuth As Boolean = False
                Dim sAuthMsg As String = ""
                Dim oAuth As New Taradel.Auth

                oAuth.Authenticate(sUser, sPass, False, bAuth, sAuthMsg)

                '-- Send an e-mail welcoming them
                Dim sEmail As String = sUser

                Dim oMsg As New appxCMS.appxMessage
                oMsg.MessageArgs.Add("emailaddress", sUser)
                oMsg.MessageArgs.Add("username", sEmail)
                oMsg.MessageArgs.Add("password", sPass)

                Dim oTemplate As appxCMS.EmailTemplate = appxCMS.EmailTemplateDataSource.GetEmailTemplate("Account Signup Welcome Message", True)
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

                Response.Redirect(Page.AppRelativeVirtualPath & "?distid=" & Me.DistributionId)

            Else
                'lSignInMsg.Text = UpdateStatusMsg("An account already exists with the same e-mail address!", True)
                lSignInMsg.Text = "<div class=""alert alert-danger""><span class=""glyphicon glyphicon-remove pull-left""></span>&nbsp;An account already exists with the same e-mail address.</div>"
            End If

        End If

    End Sub



    Protected Sub btnNewMap_Click(sender As Object, e As EventArgs) Handles btnNewMap.Click
        Response.Redirect("~/Step1-Target.aspx")
    End Sub



    Protected Sub btnCombine_Click(sender As Object, e As EventArgs) Handles btnCombine.Click
        Response.Redirect("~/MyAccount/account_selects.aspx")
    End Sub



    Protected Sub btnNewGenerated_Click(sender As Object, e As EventArgs) Handles btnNewGenerated.Click
        Response.Redirect("~/Addressed/Step1-BuildYourList.aspx")
    End Sub



    Protected Sub btnNewUploaded_Click(sender As Object, e As EventArgs) Handles btnNewUploaded.Click
        Response.Redirect("~/Addressed/Step1-UploadYourList.aspx")
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




    'Page Builders
    Private Sub BuildEDDMTargetReview()

        pnlEDDMMapOptions.Visible = True

        'Currently, user should only be able to 'edit' maps for EDDM distributions.
        btnEdit.CommandArgument = Me.DistributionId & "|" & ReferenceID

        Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistributions.GetDistribution(Me.DistributionId)
        Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(oDist.ReferenceId)
        Dim oSelects As List(Of Taradel.MapServer.UserData.AreaSelection) = Taradel.CustomerDistributions.GetSelections(oDist.ReferenceId)

        Dim iResTotal As Integer = 0
        Dim iBizTotal As Integer = 0
        Dim iBoxTotal As Integer = 0
        Dim iAreaCount As Integer = 0
        Dim aZips As New ArrayList
        Dim bBiz As Boolean = True
        Dim bBox As Boolean = True

        'oSummary is designed for EDDM distributions
        If oSummary IsNot Nothing Then

            bBiz = oSummary.UseBusiness
            bBox = oSummary.UsePOBox

            For Each oArea As Taradel.MapServer.UserData.AreaSelection In oSelects
                iResTotal = iResTotal + oArea.Residential

                If bBiz Then
                    iBizTotal = iBizTotal + oArea.Business
                End If

                If bBox Then
                    iBoxTotal = iBoxTotal + oArea.POBoxes
                End If

                iAreaCount = iAreaCount + 1

                If Not aZips.Contains(oArea.Name.Substring(0, 5)) Then
                    aZips.Add(oArea.Name.Substring(0, 5))
                End If
            Next

        End If

        Dim iZipCount As Integer = aZips.Count
        Dim sZipPlural As String = "s"
        If iZipCount = 1 Then sZipPlural = ""

        Dim sTargetDesc As String = ""
        If bBox And bBiz Then
            sTargetDesc = ", Business and Post Office Box"
        ElseIf bBox Then
            sTargetDesc = " and Post Office Box"
        ElseIf bBiz Then
            sTargetDesc = " and Business"
        End If

        Dim iTotal As Integer = iResTotal + iBizTotal + iBoxTotal


        lSelectDescription.Text = "Your selection of " & iAreaCount.ToString("N0") & " carrier routes, across " & iZipCount.ToString("N0") & " zip code" & sZipPlural & " targeting Residental" & sTargetDesc & " addresses will reach " & iTotal.ToString("N0") & " postal customers."


    End Sub


    Private Sub BuildGeneratedListTargetReview()

        pnlAddressedMapOptions.Visible = True
        lSelectDescription.Text = "Your specialized criteria will allow you to reach and deliver to " & DistributionUtility.RetrieveAddressedListCount(DistributionId).ToString("N0") & " targeted addresses."

        'Map should not be clickable
        hypGoToMap.Enabled = False

        btnNewGenerated.Visible = True
        btnNewUploaded.Visible = False

    End Sub


    Private Sub BuildUploadListTargetReview()

        pnlAddressedMapOptions.Visible = True
        'lSelectDescription.Text = "Your uploaded list will deliver to " & DistributionUtility.RetrieveAddressedListCount(DistributionId).ToString("N0") & " addresses."
        lSelectDescription.Text = "Your uploaded list will deliver to " & Quantity.ToString("N0") & " addresses."
        'Quantity
        'Map should not be clickable
        hypGoToMap.Enabled = False

        btnNewGenerated.Visible = False
        btnNewUploaded.Visible = True


    End Sub


    Private Sub BuildOrderSteps()

        'if this is an EDDM Map
        If (EDDMMap) Then

            OrderSteps.numberOfSteps = 5
            OrderSteps.step1Text = "1) Target Area"
            OrderSteps.step1Url = "/Step1-Target.aspx"
            OrderSteps.step1State = "visited"
            OrderSteps.step1Icon = "fa-map-marker"

            OrderSteps.step2Text = "2) Select Routes"
            OrderSteps.step2Url = "/Step1-Target.aspx"
            OrderSteps.step2State = "visited"
            OrderSteps.step2Icon = "fa-truck"

            OrderSteps.step3Text = "3) Choose Product"
            OrderSteps.step3Url = Request.Url.AbsoluteUri.ToString()
            OrderSteps.step3State = "current"
            OrderSteps.step3Icon = "fa-folder"

            OrderSteps.step4Text = "4) Define Delivery"
            OrderSteps.step4Url = ""
            OrderSteps.step4State = ""
            OrderSteps.step4Icon = "fa-envelope"

            OrderSteps.step5Text = "5) Check Out"
            OrderSteps.step5Url = ""
            OrderSteps.step5State = ""
            OrderSteps.step5Icon = "fa-credit-card"

        End If



        'If this is an AddressedList (user generated)
        If (AddressedMap) Then

            OrderSteps.numberOfSteps = 5
            OrderSteps.step1Text = "1) Define Area"
            OrderSteps.step1Url = "/Addressed/Step1-BuildYourList.aspx"
            OrderSteps.step1State = "visited"
            OrderSteps.step1Icon = "fa-map-marker"

            OrderSteps.step2Text = "2) Define Customers"
            OrderSteps.step2Url = "/Addressed/Step1-BuildYourList.aspx"
            OrderSteps.step2State = "visited"
            OrderSteps.step2Icon = "fa-user"

            OrderSteps.step3Text = "3) Choose Product"
            OrderSteps.step3Url = Request.Url.AbsoluteUri.ToString()
            OrderSteps.step3State = "current"
            OrderSteps.step3Icon = "fa-folder"

            OrderSteps.step4Text = "4) Define Delivery"
            OrderSteps.step4Url = ""
            OrderSteps.step4State = ""
            OrderSteps.step4Icon = "fa-envelope"

            OrderSteps.step5Text = "5) Check Out"
            OrderSteps.step5Url = ""
            OrderSteps.step5State = ""
            OrderSteps.step5Icon = "fa-credit-card"

        End If



        If (UploadedList) Then

            OrderSteps.numberOfSteps = 4
            OrderSteps.step1Text = "1) Upload List"
            OrderSteps.step1Url = "/Addressed/Step1-UploadYourList.aspx"
            OrderSteps.step1State = "visited"
            OrderSteps.step1Icon = "fa-upload"

            OrderSteps.step2Text = "2) Choose Product"
            OrderSteps.step2Url = Request.Url.AbsoluteUri.ToString()
            OrderSteps.step2State = "current"
            OrderSteps.step2Icon = "fa-folder"

            OrderSteps.step3Text = "3) Define Delivery"
            OrderSteps.step3Url = ""
            OrderSteps.step3State = ""
            OrderSteps.step3Icon = "fa-envelope"

            OrderSteps.step4Text = "4) Check Out"
            OrderSteps.step4Url = ""
            OrderSteps.step4State = ""
            OrderSteps.step4Icon = "fa-credit-card"


        End If

    End Sub



End Class


