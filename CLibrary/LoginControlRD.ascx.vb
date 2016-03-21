
Partial Class CLibrary_LoginControlRD
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        txtUID.Attributes.Add("placeholder", "User ID")
        txtPWD.Attributes.Add("placeholder", "Password")

    End Sub


End Class

