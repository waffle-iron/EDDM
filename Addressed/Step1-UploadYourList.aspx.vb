
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data

Partial Class UploadYourList
    Inherits appxCMS.PageBase


    Protected ReadOnly Property ProjectId As String
        Get
            Dim s As String = appxCMS.Util.Querystring.GetString("p")
            Dim sRet As String = s
            Dim oRe As New Regex("[^A-Z0-9a-z\-]")
            sRet = oRe.Replace(s, "")
            If sRet.Length > 36 Then
                sRet = sRet.Substring(0, 36)
            End If
            '-- Original input was corrupted
            If Not sRet.Equals(s) Then
                sRet = ""
            End If
            Return sRet
        End Get
    End Property



    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        '-- Now that we are here, we know we have a project id to start working with
        If String.IsNullOrEmpty(ProjectId) Then
            Response.Redirect(Page.AppRelativeVirtualPath & "?p=" & Server.UrlEncode(System.Guid.NewGuid().ToString()))
        End If

        'SiteID
        Dim SiteID = appxCMS.Util.CMSSettings.GetSiteId()

        'Site Details Object
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


        'Set Page Header
        'Set Page Header control
        If Not (SiteDetails.UseRibbonBanners) Then
            PageHeader.headerType = "simple"
            PageHeader.simpleHeader = "Upload Your Customer List"
        Else
            PageHeader.headerType = "partial"
            PageHeader.mainHeader = "My List"
            PageHeader.subHeader = "Upload Your Customer List"
        End If


    End Sub



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Preload suggested List Name
        'postback postback postback postback 
        If Not Page.IsPostBack Then
            txtListName.Text = "Uploaded List " + DateTime.Today.ToShortDateString()
        End If

        BuildOrderSteps()


    End Sub



    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
    End Sub



    Protected Function SaveSelections() As Boolean

        Dim results As Boolean = False
        Dim insertSQL As New StringBuilder()
        Dim errorMsg As New StringBuilder()
        Dim referenceID As String = ProjectId 'Guid.NewGuid.ToString()
        Dim ipAddress As String = Request.UserHostAddress
        Dim filterUrl As String = ""
        Dim targetType As String = "Uploaded"
        Dim startAddress As String = ""
        Dim startZipCode As String = ""
        Dim startTargetedZipCodes As String = ""
        Dim radiusType As String = ""
        Dim radiusValue As Integer = 0
        Dim jsonData As String = ""
        Dim connectString As String = ConfigurationManager.ConnectionStrings("MapServerConn").ConnectionString.ToString()
        Dim connectionObj As New SqlConnection(connectString)
        Dim returnedRefID As String = ""

        Dim sSummaryFile As String = Path.Combine(Server.MapPath("~/app_data/AddressedInbound/" & ProjectId), "list-summary.json")
        If File.Exists(sSummaryFile) Then
            jsonData = File.ReadAllText(sSummaryFile)
        End If


        insertSQL.Append("EXEC sp_InsertSavedAddressedSelections ")
        insertSQL.Append("@paramReferenceID='" & referenceID & "', ")
        insertSQL.Append("@paramCreatedIP='" & ipAddress & "', ")
        insertSQL.Append("@paramFilterURL='" & filterUrl & "', ")
        insertSQL.Append("@paramTargetType='" & targetType & "', ")
        insertSQL.Append("@paramStartAddress='" & startAddress & "', ")
        insertSQL.Append("@paramZipCode='" & startZipCode & "',")
        insertSQL.Append("@paramStartTargetedZipCodes='" & startTargetedZipCodes & "', ")
        insertSQL.Append("@paramRadiusType='" & radiusType & "', ")
        insertSQL.Append("@paramRadiusValue=" & radiusValue & ", ")
        insertSQL.Append("@paramJSONData='" & jsonData & "'")


        Dim insertCommand As New SqlCommand()
        insertCommand.CommandType = CommandType.Text
        insertCommand.CommandText = insertSQL.ToString()
        insertCommand.Connection = connectionObj


        Try

            connectionObj.Open()
            returnedRefID = insertCommand.ExecuteScalar().ToString()

        Catch objException As Exception

            errorMsg.Append("The following errors occurred:<br /><br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " + objException.Message + "</li>")
            errorMsg.Append("<li>Source: " + objException.Source + "</li>")
            errorMsg.Append("<li>Stack Trace: " + objException.StackTrace + "</li>")
            errorMsg.Append("<li>Target Site: " + objException.TargetSite.Name + "</li>")
            errorMsg.Append("<li>SQL: " + insertSQL.ToString() + "</li>")
            errorMsg.Append("</ul>")

            pnlError.Visible = True
            litErrorMessage.Text = errorMsg.ToString()

        Finally
            connectionObj.Close()
        End Try

        If returnedRefID.Length > 0 Then
            results = True
        End If

        Return results

    End Function



    Public Function CreateDistribution() As Integer
        Dim sMsg As String = ""
        Dim CustomerId As Integer = 0
        If Context.Request.IsAuthenticated Then
            CustomerId = Taradel.Customers.GetCustomerId(Context.User.Identity.Name, sMsg)
        Else
            CustomerId = Taradel.Customers.CreateAnonymous(sMsg)
        End If
        Dim DistributionId As Integer = 0
        If CustomerId > 0 Then
            Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(5)
            Using oDb As New Taradel.taradelEntities
                Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistribution.CreateCustomerDistribution(0, txtListName.Text, System.DateTime.Now, ProjectId, hfListCount.Value, False)
                oDist.CustomerReference.EntityKey = New EntityKey(oDb.DefaultContainerName & ".CustomerSet", "CustomerID", CustomerId)
                oDist.USelectMethodReference.EntityKey = oUSelect.EntityKey
                oDb.AddToCustomerDistributionSet(oDist)

                Try
                    oDb.SaveChanges()
                    DistributionId = oDist.DistributionId
                Catch ex As Exception
                    sMsg = ex.Message
                End Try
            End Using
        End If
        Return DistributionId
    End Function



    Protected Sub btnContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnContinue.Click

        If SaveSelections() Then

            Dim distId As Integer = CreateDistribution()

            If distId > 0 Then
                'Response.Redirect("~/Step1-TargetReview.aspx?distid=" + distId.ToString() + "&USelect=5&Count=" + hfListCount.Value)
                Response.Redirect("~/Step1-TargetReview.aspx?distid=" + distId.ToString())
            End If

        End If

    End Sub



    Private Sub BuildOrderSteps()

        OrderSteps.numberOfSteps = 4
        OrderSteps.step1Text = "1) Upload List"
        OrderSteps.step1Url = "/Addressed/Step1-UploadYourList.aspx"
        OrderSteps.step1State = "current"
        OrderSteps.step1Icon = "fa-upload"

        OrderSteps.step2Text = "2) Choose Product"
        OrderSteps.step2Url = ""
        OrderSteps.step2State = ""
        OrderSteps.step2Icon = "fa-folder"

        OrderSteps.step3Text = "3) Define Delivery"
        OrderSteps.step3Url = 
        OrderSteps.step3State = ""
        OrderSteps.step3Icon = "fa-envelope"

        OrderSteps.step4Text = "4) Check Out"
        OrderSteps.step4Url = ""
        OrderSteps.step4State = ""
        OrderSteps.step4Icon = "fa-credit-card"

    End Sub


End Class
