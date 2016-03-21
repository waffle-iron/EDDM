
Partial Class account_addressbook
    Inherits MyAccountBase

    Protected Sub AddressBook_ContactSelected() Handles AddressBook.ContactSelected
        Response.Redirect(Page.AppRelativeVirtualPath)
    End Sub
End Class
