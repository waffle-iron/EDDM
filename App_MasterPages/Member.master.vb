
Partial Class App_MasterPages_Member
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If HttpContext.Current.User.IsInRole("Admin") Then
            Response.Redirect("/admin")
        End If

        lFirstName.Text = HttpContext.Current.User.Identity.Name

        Select Case Page.AppRelativeVirtualPath.ToLower
            Case "~/myaccount/account_manage.aspx"
                hplAccountSettings.CssClass = "active"
            Case "~/myaccount/account_orders.aspx", "~/myaccount/account_order.aspx"
                hplOrders.CssClass = "active"
            Case "~/myaccount/account_quotes.aspx", "~/myaccount/account_quote.aspx"
                'hplQuotes.CssClass = "active"
            Case "~/myaccount/account_designs.aspx"
                'hplDesigns.CssClass = "active"
            Case "~/myaccount/account_selects.aspx"
                hplUSelect.CssClass = "active"
        End Select
    End Sub
End Class

