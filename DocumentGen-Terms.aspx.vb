
Partial Class DocumentGen_Terms
    Inherits appxCMS.PageBase

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim oContent As appxCMS.Content = appxCMS.ContentDataSource.GetPageContentArea("/terms", "phBody")
        If oContent IsNot Nothing Then
            Dim sMsg As String = oContent.ContentData
            If Not String.IsNullOrEmpty(sMsg.Trim) Then
                lMsg.Text = oContent.ContentData
            Else
                lMsg.Text = "<a href=""http://" & Request.Url.Host & "/terms"">Terms & Conditions are available online</a>."
            End If
        Else
            lMsg.Text = "<a href=""http://" & Request.Url.Host & "/terms"">Terms & Conditions are available online</a>."
        End If
    End Sub
End Class
