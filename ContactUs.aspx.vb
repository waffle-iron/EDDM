
Partial Class ContactUs
    Inherits appxCMS.PageBase




    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Init

        Dim siteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
        Dim siteObj = appxCMS.SiteDataSource.GetSite(siteID)

        litPhone.Text = siteObj.TollFreeNumber

    End Sub



End Class
