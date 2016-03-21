<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BootstrapDataPager.ascx.vb"
    Inherits="UserControls_BootstrapDataPager" %>
<ul class="pagination">
    <li>
        <asp:HyperLink ID="hplMoveFirst" runat="server" ToolTip="First Page"></asp:HyperLink></li>
    <li>
        <asp:HyperLink ID="hplMovePrevious" runat="server" ToolTip="Previous Page"></asp:HyperLink></li>
    <asp:Repeater ID="rPages" runat="server">
        <ItemTemplate>
            <li class='<%#Iif(Eval("Key")=Me.CurPage.ToString, "active", "") %>'>
                <asp:HyperLink ID="hplTextNav" runat="server" NavigateUrl='<%#Eval("Value") %>' Text='<%#Eval("Key") %>'/></li>
        </ItemTemplate>
    </asp:Repeater>
    <li>
        <asp:HyperLink ID="hplMoveNext" runat="server" ToolTip="Next Page"></asp:HyperLink></li>
    <li>
        <asp:HyperLink ID="hplMoveLast" runat="server" ToolTip="Last Page"></asp:HyperLink></li>
</ul>
<%--<asp:PlaceHolder ID="phPageSummary" runat="server">
            <div class="summary">
                Showing
                <asp:Literal ID="lFirstItemNumber" runat="server" />
                -
                <asp:Literal ID="lLastItemNumber" runat="server" />
                out of
                <asp:Literal ID="lTotalItems" runat="server" />
            </div>
        </asp:PlaceHolder>--%>
