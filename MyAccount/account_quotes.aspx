<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Member.master" AutoEventWireup="false"
    CodeFile="account_quotes.aspx.vb" Inherits="account_quotes" Title="My Custom Quotes"
    EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phBody" runat="Server">
    <h1>
        My Custom Quotes</h1>
    <asp:Panel ID="pnlNoQuotes" runat="server" Visible="false">
        <p>
            There are no custom quotes for your account.</p>
    </asp:Panel>
    <asp:Repeater ID="rQuotes" runat="server">
        <HeaderTemplate>
            <table class="cart" border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <th nowrap="nowrap">
                        Name
                    </th>
                    <th nowrap="nowrap">
                        Quote Date
                    </th>
                    <th nowrap="nowrap">
                        Expiration Date
                    </th>
                    <th nowrap="nowrap">
                        Total (Includes Shipping)
                    </th>
                    <th nowrap="nowrap">
                        &nbsp;
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%#Eval("Name") %>
                </td>
                <td style="text-align: center;">
                    <%#DateTime.Parse(Eval("QuoteDate")).ToString("MM/dd/yyyy")%>
                </td>
                <td style="text-align: center;">
                    <%#DateTime.Parse(Eval("QuoteExpiration")).Tostring("MM/dd/yyyy") %>
                </td>
                <td style="text-align: center;">
                    <%#String.Format("{0:c}", Eval("QuotePrice") - ((Eval("QuotePrice") * Eval("Discount")) / 100))%>
                </td>
                <td style="text-align: center;">
                    <asp:HyperLink ID="hplViewQuote" runat="server" Text="Open" NavigateUrl='<%#String.Format("/account_quote.aspx?ID={0}", Eval("QuoteID")) %>'
                        CssClass="makeButton" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
