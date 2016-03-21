
Partial Class usercontrols_cmsPageSearch
    Inherits System.Web.UI.UserControl

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim sName As String = PageName.Text
        Dim sTemplate As String = "" 'PageTemplate.SelectedValue

        Response.Redirect("~/admin/cms_pages.aspx?pgref=" & Server.UrlEncode(sName) & "&template=" & Server.UrlEncode(sTemplate))
    End Sub
End Class
