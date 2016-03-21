
Partial Class account_addressbook_edit
    Inherits MyAccountBase

    Protected Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim iCustomer As Integer = GetCustomerId
        oAddress.SelectParameters("CustomerID").DefaultValue = iCustomer.ToString
        oAddress.UpdateParameters("CustomerID").DefaultValue = iCustomer.ToString
    End Sub

    Protected Sub fvAddress_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewUpdatedEventArgs) Handles fvAddress.ItemUpdated
        GoToAddressBook(sender, e)
    End Sub

    Protected Sub GoToAddressBook(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("account_addressbook.aspx")
    End Sub
End Class
