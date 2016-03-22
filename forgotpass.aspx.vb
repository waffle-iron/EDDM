
Partial Class forgotpass
    Inherits appxCMS.PageBase

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        txtEmail.Attributes.Add("placeholder", "Enter email")
        txtEmail.Attributes.Add("type", "email")
        'test comment'
    End Sub

    Protected Sub btnRecover_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecover.Click

        Dim sEmail As String = txtEmail.Text

        If Not String.IsNullOrEmpty(sEmail) Then

            Dim oUser As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(sEmail)

            If oUser IsNot Nothing Then

                Dim oMsg As New appxCMS.appxMessage
                oMsg.MessageArgs.Add("emailaddress", oUser.EmailAddress)
                oMsg.MessageArgs.Add("token", oUser.Password)

                Dim bSent As Boolean = appxCMS.Messaging.SendEmail(oMsg, "Recover Password", True)

                If Not bSent Then
                    pnlError.Visible = True
                    lblError.Text = "There was a problem sending your password reminder."
                Else
                    pnlSuccess.Visible = True
                    lblSuccess.Text = "A password reminder has been sent. Please check your email."

                End If

            Else

                lblError.Text = "Sorry - account not found."
                pnlError.Visible = True

            End If


            Else
                pnlError.Visible = True
                lblError.Text = "You must enter the e-mail address that was used when you created your account."
            End If

    End Sub

End Class
