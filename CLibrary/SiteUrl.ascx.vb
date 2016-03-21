
Partial Class CLibrary_SiteUrl
    Inherits CLibraryBase

    Public Property EnableLink As Boolean
        Get
            Return hplUrl.Enabled
        End Get
        Set(value As Boolean)
            hplUrl.Enabled = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim sUrl As String = appxCMS.SiteDataSource.GetAuthoritativeUrl.ToString
        If String.IsNullOrEmpty(sUrl) Then
            sUrl = "http://" & Request.Url.Host
        End If
        hplUrl.NavigateUrl = sUrl
        hplUrl.Text = sUrl
    End Sub
End Class
