
Partial Class account_exists
    Inherits appxCMS.PageBase


    Protected Sub btnRecover_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecover.Click
        Dim sEmail As String = EmailAddress.Text
        Dim oUser As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(sEmail)

        If oUser IsNot Nothing Then

            Dim oMsg As New appxCMS.appxMessage
            oMsg.MessageArgs.Add("emailaddress", oUser.EmailAddress)
            oMsg.MessageArgs.Add("token", oUser.Password)
            Dim bSent As Boolean = appxCMS.Messaging.SendEmail(oMsg, "Recover Password", True)

            If Not bSent Then
                lblError.Text = "There was a problem sending your password reminder."
                pnlSuccess.Visible = False
                pnlError.Visible = True
            Else
                lblSuccess.Text = "A password remdinder has been sent."
                pnlSuccess.Visible = True
                pnlError.Visible = False
            End If

        Else
            lblError.Text = "Account not found."
            pnlSuccess.Visible = False
            pnlError.Visible = True
        End If

    End Sub


End Class
