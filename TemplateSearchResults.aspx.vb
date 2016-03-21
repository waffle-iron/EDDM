
Partial Class TemplateSearchResults
    Inherits appxCMS.PageBase

    Protected ReadOnly Property CurPage As Integer
        Get
            Dim iPg As Integer = appxCMS.Util.Querystring.GetInteger("pg")
            If iPg = 0 Then iPg = 1
            Return iPg
        End Get
    End Property

    Protected ReadOnly Property Keywords As String
        Get
            Return appxCMS.Util.Querystring.GetString("q")
        End Get
    End Property

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim oMeta As New HtmlMeta
        oMeta.Name = "robots"
        oMeta.Content = "NOINDEX, NOFOLLOW"
        Page.Header.Controls.Add(oMeta)

        'BannerFull.FullText = "Search Results"

        PagedTemplateList.PageCount = 21
        PagedTemplateList.CurPage = Me.CurPage
        PagedTemplateList.Keywords = Me.Keywords
        PagedTemplateList.DoRender()

        Dim SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
        If SiteID = 83 Then
            TemplateIndustries.Visible = False
        End If


        Page.Title = "Customizable Every Door Direct Mail&tm; Templates Search Results"

    End Sub
End Class
