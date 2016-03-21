
Partial Class UserControls_cmsPageManager
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If HttpContext.Current.User.IsInRole("Manage.Content") Then
            Dim sPage As String = apphelp.GetRequestedURL
            hplManage.NavigateUrl = "~/admin/cms_page_edit.aspx?page=" & sPage

            hplSiteHeader.NavigateUrl = "~/admin/cms_page_edit.aspx?page=/_allpages.aspx&carea=phHead"
            hplSiteNav.NavigateUrl = "~/admin/cms_page_edit.aspx?page=/_allpages.aspx&carea=phNav"
            hplSiteFooter.NavigateUrl = "~/admin/cms_page_edit.aspx?page=/_allpages.aspx&carea=phFoot"
            'If Page.AppRelativeVirtualPath.ToLower.StartsWith("~/vpage_") Then
            '    pPage.Visible = False
            'End If
            'hplPageHeader.NavigateUrl = "~/admin/cms_page_edit.aspx?page=" & apphelp.GetRequestedURL() & "&carea=phHead"
            'hplPageNav.NavigateUrl = "~/admin/cms_page_edit.aspx?page=" & apphelp.GetRequestedURL() & "&carea=phNav"
            'hplPageFooter.NavigateUrl = "~/admin/cms_page_edit.aspx?page=" & apphelp.GetRequestedURL() & "&carea=phFoot"

            hplValidate.NavigateUrl = String.Format("http://validator.w3.org/check?uri={0}&charset=%28detect+automatically%29&doctype=Inline&group=1", Server.HtmlEncode(Request.RawUrl))
        Else
            Me.Visible = False
        End If
    End Sub
End Class
