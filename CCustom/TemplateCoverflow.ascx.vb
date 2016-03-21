
Partial Class CCustom_TemplateCoverflow
    Inherits CLibraryBase


    Public Enum CoverflowDisplayMode
        TopNRandom = 1

    End Enum

    Private _DisplayMode As CoverflowDisplayMode = CoverflowDisplayMode.TopNRandom
    Public Property DisplayMode As CoverflowDisplayMode
        Get
            Return _DisplayMode
        End Get
        Set(value As CoverflowDisplayMode)
            _DisplayMode = value
        End Set
    End Property

    Private _NumberToShow As Integer = 21
    Public Property NumberToShow As Integer
        Get
            Return _NumberToShow
        End Get
        Set(value As Integer)
            _NumberToShow = value
        End Set
    End Property

    Private _PageSize As String = "11x17"
    Public Property PageSize As String
        Get
            Return _PageSize
        End Get
        Set(value As String)
            _PageSize = value
        End Set
    End Property

    Protected NumTemplates As Integer = 0

    Protected sTemplateServerHost As String = ""

    Protected iHost As Integer = 1







    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub


    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        appxCMS.Util.jQuery.RegisterStylesheet(Page, "~/scripts/contentflow.css")
        appxCMS.Util.jQuery.RegisterStylesheet(Page, "~/scripts/mycontentflow.css")
        appxCMS.Util.jQuery.IncludePlugin(Page, "contentflow", "~/scripts/contentflow.js")

        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    var cf = new ContentFlow('contentFlow', {reflectionColor: '#000000'});")
        oJs.AppendLine("});")
        jqueryHelper.RegisterClientScript(Page, Me.ClientID & "coverflowinit", oJs.ToString)
    End Sub


    Protected Overrides Sub BuildControl()
        lPageSize.Text = Me.PageSize
        sTemplateServerHost = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
        Select Case Me.DisplayMode
            Case CoverflowDisplayMode.TopNRandom
                Using oAPI As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetRandomNTemplatesRequest(appxCMS.Util.CMSSettings.GetSiteId, Me.PageSize, Me.NumberToShow)
                    Dim oResponse As TemplateCode.GetRandomNTemplatesResponse = oAPI.GetRandomNTemplates(oRequest)
                    Dim oTemplates As System.Collections.Generic.List(Of TemplateCode.Template1) = oResponse.GetRandomNTemplatesResult
                    NumTemplates = oTemplates.Count
                    lvCoverflow.DataSource = oTemplates
                    lvCoverflow.DataBind()
                End Using
        End Select
    End Sub


    Public Sub DoRender()
        BuildControl()
    End Sub



    Protected Sub lvCoverflow_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvCoverflow.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim oTemplate As TemplateCode.Template1 = DirectCast(oDItem.DataItem, TemplateCode.Template1)

            'new 10/30/2015
            Dim pathForSearch As String = "EDDM"

            If Request.Url.ToString().IndexOf("Addressed") > 0 Then
                pathForSearch = "Addressed"
            End If
            'end new 10/30/2015

            Dim imgTemplate As Image = DirectCast(e.Item.FindControl("imgTemplate"), Image)
            imgTemplate.ImageUrl = sTemplateServerHost & "/templates/thumb/" & oTemplate.FrontImage
            imgTemplate.AlternateText = oTemplate.Name

            Dim pItem As Panel = DirectCast(e.Item.FindControl("pItem"), Panel)
            pItem.Attributes.Add("href", appxCMS.SEO.Rewrite.BuildLink("Template" & oTemplate.TemplateId, oTemplate.TemplateId, "template") & "?loc=" & pathForSearch)

            iHost = iHost + 1
            If iHost > 4 Then
                iHost = 1
            End If
        End If
    End Sub







End Class
