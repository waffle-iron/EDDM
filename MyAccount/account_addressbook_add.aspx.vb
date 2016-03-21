
Partial Class account_addressbook_add
    Inherits MyAccountBase

    Protected Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        oAddress.InsertParameters("CustomerID").DefaultValue = GetCustomerId
    End Sub

    Protected Sub GoToAddressBook(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("account_addressbook.aspx")
    End Sub

    Protected Sub fvAddress_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewInsertedEventArgs) Handles fvAddress.ItemInserted
        GoToAddressBook(sender, e)
    End Sub
End Class
