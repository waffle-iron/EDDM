
Partial Class CLibrary_SiteName
    Inherits CLibraryBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim sName As String = appxCMS.SiteDataSource.GetSiteName
        If String.IsNullOrEmpty(sName) Then
            sName = "this site"
        End If
        lName.Text = sName
    End Sub
End Class
