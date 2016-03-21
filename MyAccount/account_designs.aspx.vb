Imports System.Data
Imports System.Xml
Imports System.IO

Partial Class account_designs
    Inherits MyAccountBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.IsAuthenticated Then
            FormsAuthentication.RedirectToLoginPage()
        End If

        If Request.IsSecureConnection Then
            Response.Redirect("http://" & Request.Url.Host & VirtualPathUtility.ToAbsolute(Page.AppRelativeVirtualPath))
        End If

        Dim sUsername As String = User.Identity.Name
        Dim iCustomerId As Integer = GetCustomerId

        '-- Get any saved designs

        'TODO: Update all of this to support Tweak Integration
        'Dim oSaved As New DataTable
        'oSaved.Columns.Add(New DataColumn("SavedId", GetType(Integer)))
        'oSaved.Columns.Add(New DataColumn("SavedDate", GetType(String)))
        'oSaved.Columns.Add(New DataColumn("Preview", GetType(String)))
        'oSaved.Columns.Add(New DataColumn("OpenLink", GetType(String)))

        'Dim oCookieJar As New System.Net.CookieContainer

        'Dim aAuthData As New Hashtable
        'aAuthData.Add("cat", "-551495587")
        'aAuthData.Add("id", "1244203299")
        'aAuthData.Add("un", Server.UrlEncode(sUsername))
        'aAuthData.Add("uid", iCustomerId)

        'Dim aRequestData As New Hashtable
        'aRequestData.Add("un", sUsername)
        'aRequestData.Add("uid", iCustomerId)

        'Dim sAuthBase As String = "http://udesign.taradel.com/engineV5.asp"
        'Dim sAuthUrl As String = sAuthBase & "?cat=-551495587&id=1244203299&un=" & sUsername & "&uid=" & iCustomerId
        'lCookieFrame.Text = "<iframe src=""" & sAuthUrl & """ height=""1"" width=""1"" style=""display:none;""></iframe>"

        'Dim sAuth As String = appxCMS.Util.httpHelp.GetXMLURLPage(sAuthUrl, Nothing, "GET", oCookieJar)
        'sAuth = appHelp.GetXMLURLPage(sAuth & "&nc=" & System.DateTime.Now.Ticks + 10, Nothing, oCookieJar)

        'Dim sSaved As String = appHelp.XMLURLPageRequest("http://udesign.taradel.com/myAccount.asp", Nothing, "GET", oCookieJar)
        'If Not sSaved.ToLower.Contains("no saved works were found in your personal account") Then
        '    Response.Write(sSaved)
        '    Response.End()

        '    Dim iTable1 As Integer = sSaved.IndexOf("<table")
        '    Dim iTable2 As Integer = sSaved.IndexOf("<table", iTable1 + "<table".Length)

        '    Dim iStart As Integer = sSaved.IndexOf("<table", iTable2 + "<table".Length)
        '    Dim iEnd As Integer = sSaved.IndexOf("</table>", iStart) + "</table>".Length
        '    Dim sTable As String = sSaved.Substring(iStart, iEnd - iStart)
        '    Dim sTableXml As String = sTable
        '    'tSaved.Text = sTable
        '    Dim oSGMLReader As New Sgml.SgmlReader
        '    oSGMLReader.DocType = "XML"
        '    oSGMLReader.InputStream = New StreamReader(New MemoryStream(UTF8Encoding.Default.GetBytes(sTable)))
        '    sTableXml = oSGMLReader.ReadOuterXml
        '    Dim oXml As New XmlDocument
        '    oXml.LoadXml(sTableXml)
        '    Dim oRows As XmlNodeList = oXml.SelectNodes("//tr")

        '    For iRow As Integer = 1 To oRows.Count - 1
        '        Dim oIdCol As XmlNode = oRows(iRow).SelectSingleNode("td[1]")
        '        Dim oDateCol As XmlNode = oRows(iRow).SelectSingleNode("td[2]")
        '        Dim oImgCol As XmlNode = oRows(iRow).SelectSingleNode("td[3]")
        '        Dim oImg As XmlNode = oImgCol.SelectSingleNode("img")

        '        Dim sSaveEditUrl As String = "http://udesign.taradel.com/engineV5.asp?act=backToWork&wid=" & oIdCol.InnerText.Trim & "&un=" & Server.UrlEncode(sUsername) & "&uid=" & iCustomerId
        '        Dim sSavedUrl As String = appHelp.GetXMLURLPage(sSaveEditUrl, Nothing, oCookieJar)
        '        Dim iSavedStart As Integer = sSavedUrl.IndexOf(".cardBack {")
        '        Dim iSavedEnd As Integer = sSavedUrl.IndexOf("}", iSavedStart + 1)
        '        Dim sSavedCSS As String = sSavedUrl.Substring(iSavedStart, iSavedEnd - iSavedStart)
        '        Dim sSavedImg As String = ""
        '        Dim iJpgStart As Integer = 0
        '        Dim iJpgEnd As Integer = sSavedCSS.IndexOf(".jpg")
        '        iJpgStart = sSavedCSS.LastIndexOf("/")
        '        Dim sJpg As String = sSavedCSS.Substring(iJpgStart + 1, iJpgEnd - iJpgStart - 1)

        '        '-- Get a reference to the saved design so that we can open it in the correct context with a return url specified
        '        Using oTemplateA As New taradelDesignDIYTableAdapters.TemplateTableAdapter
        '            Using oTemplateT As taradelDesignDIY.TemplateDataTable = oTemplateA.GetByBYAID(sJpg)
        '                If oTemplateT.Rows.Count > 0 Then
        '                    Dim oTemplate As taradelDesignDIY.TemplateRow = oTemplateT.Rows(0)

        '                    sSaveEditUrl = linkHelp.SEOLink(oTemplate.Name, oTemplate.DesignDIYTemplateID & "-" & oTemplate.DesignDIYCategoryID & "-" & oIdCol.InnerText.Trim, linkHelp.LinkType.DesignDIYTool)
        '                End If
        '            End Using
        '        End Using

        '        Dim oSavedRow As DataRow = oSaved.NewRow
        '        oSavedRow("SavedId") = oIdCol.InnerText.Trim
        '        oSavedRow("Preview") = "http://udesign.taradel.com/" & xmlHelp.ReadAttribute(oImg, "src").Trim
        '        oSavedRow("SavedDate") = oDateCol.InnerText.Trim
        '        oSavedRow("OpenLink") = sSaveEditUrl

        '        oSaved.Rows.Add(oSavedRow)
        '    Next
        'End If
        'lvSaved.DataSource = oSaved
        'lvSaved.DataBind()

        'If Not Page.IsPostBack Then
        '    Using oDesignsA As New taradelCustomerTableAdapters.CustomerDesignListTableAdapter
        '        Using oDesignsT As taradelCustomer.CustomerDesignListDataTable = oDesignsA.GetData(appHelp.GetCustomerID)
        '            lvDesigns.DataSource = oDesignsT
        '            lvDesigns.DataBind()
        '        End Using
        '    End Using
        'End If
    End Sub

    Protected Sub lvDesigns_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvDesigns.ItemDataBound
        'If e.Item.ItemType = ListViewItemType.DataItem Then
        '    Dim oDItem As ListViewDataItem = e.Item
        '    Dim sArtUrl As String = DataBinder.Eval(oDItem.DataItem, "ArtUrl").ToString
        '    Dim sArtKey As String = DataBinder.Eval(oDItem.DataItem, "BuildYourArtId").ToString
        '    If String.IsNullOrEmpty(sArtUrl) And Not String.IsNullOrEmpty(sArtKey) Then
        '        Try
        '            '-- Try to download the file and update the record
        '            '-- Try to retrieve and save the art url based on the art key (again, maybe something happened when we initially saved it)
        '            Dim sBYAUrl As String = "http://taradel.buildyourart.com/WebServices/Gateway.asmx"
        '            If Not ConfigurationManager.AppSettings("buildyourart.Gateway") Is Nothing Then
        '                sBYAUrl = ConfigurationManager.AppSettings("buildyourart.Gateway").ToString
        '            End If
        '            Dim oBYA As New buildyourart.Gateway
        '            oBYA.Url = sBYAUrl
        '            sArtUrl = oBYA.GetArtworkImageLink(sArtKey, "", "")
        '            If Not String.IsNullOrEmpty(sArtUrl) Then
        '                Using oDesignA As New taradelCustomerTableAdapters.pnd_CustomerDesignTableAdapter
        '                    oDesignA.SetArtUrl(sArtUrl, appHelp.GetCustomerID, sArtKey)
        '                End Using
        '            End If
        '        Catch ex As Exception

        '        End Try
        '    End If

        '    If String.IsNullOrEmpty(sArtUrl) Then
        '        e.Item.Visible = False
        '    Else
        '        Dim sCatPlural As String = DataBinder.Eval(oDItem.DataItem, "CategoryName")
        '        Dim sCat As String = linkHelp.DePluralizeText(sCatPlural)
        '        Dim lDesignCat As Literal = DirectCast(e.Item.FindControl("lDesignCat"), Literal)
        '        lDesignCat.Text = sCat

        '        Dim imgDesign As Image = DirectCast(e.Item.FindControl("imgDesign"), Image)
        '        If imgDesign IsNot Nothing Then
        '            imgDesign.ImageUrl = sArtUrl & "&LimitWidth=200&LimitHeight=200"
        '        Else
        '            e.Item.Visible = False
        '        End If

        '        '-- Build link for this design if there are products available
        '        Dim iTemplateID As Integer = DataBinder.Eval(oDItem.DataItem, "DesignDIYTemplateID")
        '        Dim lnkNextSteps As LinkButton = DirectCast(e.Item.FindControl("lnkNextSteps"), LinkButton)
        '        'lnkNextSteps.NavigateUrl = linkHelp.ResolveSEOLink(iTemplateID, linkHelp.LinkType.DesignDIYNextSteps)
        '        lnkNextSteps.CommandName = iTemplateID
        '        lnkNextSteps.Text = "Do more with your design"
        '    End If
        'End If
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item
            Dim iTemplateId As Integer = DataBinder.Eval(oDItem.DataItem, "DesignDIYTemplateId")
            Dim iDesignId As Integer = DataBinder.Eval(oDItem.DataItem, "CustomerDesignId")
            Dim iSource As Integer = 0
            Try
                'TODO: Fix these so that they work in the Whitelabel Site
                'Using oTemplateA As New taradelDesignDIYTableAdapters.TemplateTableAdapter
                '    iSource = oTemplateA.GetSourceId(iTemplateId)
                'End Using

                'Dim oSource As pndHelp.DesignSource = [Enum].Parse(GetType(pndHelp.DesignSource), iSource)
                'Dim oSourceProvider As UDesign.IDesignServiceProvider = pndHelp.GetUDesignInstance(oSource)

                Dim sArtUrl As String = DataBinder.Eval(oDItem.DataItem, "ArtUrl").ToString
                Dim sThumb As String = "" 'oSourceProvider.GetDesignThumbnail(iDesignId)
                If String.IsNullOrEmpty(sThumb) Then
                    e.Item.Visible = False
                Else
                    Dim sCatPlural As String = DataBinder.Eval(oDItem.DataItem, "CategoryName")
                    Dim sCat As String = linkHelp.DePluralizeText(sCatPlural)
                    Dim lDesignCat As Literal = DirectCast(e.Item.FindControl("lDesignCat"), Literal)
                    lDesignCat.Text = sCat

                    Dim imgDesign As Image = DirectCast(e.Item.FindControl("imgDesign"), Image)
                    If imgDesign IsNot Nothing Then
                        imgDesign.ImageUrl = sThumb
                        imgDesign.Style.Add("max-width", "200px")
                        imgDesign.Style.Add("max-height", "200px")
                    Else
                        e.Item.Visible = False
                    End If

                    'Dim sThumbBack As String = oSourceProvider.GetDesignThumbnail(iDesignId, UDesign.IDesignServiceProvider.DesignSide.Back)
                    'If Not String.IsNullOrEmpty(sThumbBack) Then
                    '    Dim imgDesignBack As Image = DirectCast(e.Item.FindControl("imgDesignBack"), Image)
                    '    If imgDesignBack IsNot Nothing Then
                    '        imgDesignBack.ImageUrl = sThumbBack
                    '        imgDesignBack.Style.Add("max-width", "200px")
                    '        imgDesignBack.Style.Add("max-height", "200px")
                    '        imgDesignBack.Style.Add("margin-left", "1em")
                    '        imgDesignBack.Visible = True
                    '    End If
                    'End If

                    '-- Build link for this design if there are products available
                    Dim lnkNextSteps As LinkButton = DirectCast(e.Item.FindControl("lnkNextSteps"), LinkButton)
                    lnkNextSteps.CommandName = DataBinder.Eval(oDItem.DataItem, "DesignDIYTemplateId")
                    lnkNextSteps.Text = "Order this design"
                End If

            Catch ex As Exception
                e.Item.Visible = False
            End Try
        End If
    End Sub

    Protected Sub SavedSteps(sender As Object, e As System.EventArgs)

    End Sub

    Protected Sub NextSteps(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oBtn As LinkButton = DirectCast(sender, LinkButton)
        Dim sKey As String = oBtn.CommandArgument
        Dim iTemplateID As String = oBtn.CommandName
        If Not String.IsNullOrEmpty(sKey) Then
            Session("CurDesignId") = sKey
            Response.Redirect(linkHelp.ResolveSEOLink(iTemplateID, linkHelp.LinkType.DesignDIYNextSteps))
        End If
    End Sub
End Class
