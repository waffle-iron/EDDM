
Partial Class UserControls_cmsCreatePage
    Inherits System.Web.UI.UserControl

    Public Event CreateFailed()

    Private _StatusMsg As String = ""
    Public ReadOnly Property StatusMsg() As String
        Get
            Return _StatusMsg
        End Get
    End Property

    Public Property SetPageRef() As String
        Get
            Return PageRef.Text
        End Get
        Set(ByVal value As String)
            PageRef.Text = value
        End Set
    End Property

    Protected Sub lnkAddContent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddContent.Click
        Dim bErr As Boolean = False
        Dim iContent As Integer = 0
        Dim sPageRef As String = PageRef.Text.Trim
        If Not sPageRef.StartsWith("/") Then sPageRef = "/" & sPageRef

        Dim bUnique As Boolean = appxCMS.ContentDataSource.IsUniquePageName(sPageRef)
        If Not bUnique Then
            Me._StatusMsg = "There is already a page with the same name."
            bErr = True
        Else
            Dim bRet As Boolean = appxCMS.ContentDataSource.AddVirtualPage(sPageRef, SelectedTemplate.SelectedValue, iContent, Me._StatusMsg)
            If bRet Then
                apphelp.AuditChange("appxCMS_ContentVirtual", "VContentID", iContent, "Insert", HttpContext.Current.User.Identity.Name, pageBase.LoggedOnUserID)
                Response.Redirect("~/admin/cms_page_edit.aspx?page=" & Server.UrlEncode(sPageRef))
            Else
                RaiseEvent CreateFailed()
            End If
        End If
    End Sub
End Class
