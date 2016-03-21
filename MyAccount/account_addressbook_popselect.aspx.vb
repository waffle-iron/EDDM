
Partial Class account_addressbook_popselect
    Inherits MyAccountBase

    Protected Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        oShipping.SelectParameters("CustomerID").DefaultValue = GetCustomerId.ToString
    End Sub
End Class
