
Partial Class LogonProxy
    Inherits appxCMS.PageBase

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim sReturn As String = QStringToVal("ReturnUrl")

        Dim sRedirect As String = "~/account_signin.aspx"
        If sReturn.ToLower.Equals("/admin") OrElse sReturn.ToLower.StartsWith("/admin/") Then
            sRedirect = "~/logon.aspx"
        End If

        Response.Redirect(sRedirect & "?ReturnUrl=" & Server.UrlEncode(sReturn))
    End Sub
End Class
