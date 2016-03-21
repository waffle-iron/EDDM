
Partial Class CLibrary_SiteCopyrightNotice
    Inherits CLibraryBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim sName As String = appxCMS.SiteDataSource.GetSiteName
        If String.IsNullOrEmpty(sName) Then
            sName = Request.Url.Host
        End If
        lSiteName.Text = sName

        lYear.Text = Now.Year
    End Sub
End Class
