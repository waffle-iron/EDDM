<%@ Control Language="VB" AutoEventWireup="false" CodeFile="YourAccountMenu.ascx.vb" Inherits="CLibrary_YourAccountMenu" %>

<div class="panel panel-default">
    <div class="panel-heading">
        Your Account
    </div>
    <div class="panel-body">
        <div class="leftNav">
            <ul class="list-unstyled">
                <li><asp:HyperLink ID="hypAccountSettings" runat="server" NavigateUrl="~/MyAccount/account_manage.aspx" ToolTip="Account Settings"><span class="fa fa-cog fa-fw"></span>&nbsp;Account Settings</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypOrderHistory" runat="server" ToolTip="Order History" NavigateUrl="~/MyAccount/account_orders.aspx"><span class="fa fa-folder-open fa-fw"></span>&nbsp;Order History</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypSavedMaps" runat="server" ToolTip="Saved maps" NavigateUrl="~/MyAccount/account_selects.aspx"><span class="fa fa-map-marker fa-fw"></span>&nbsp;Saved Maps</asp:HyperLink></li>
                <%--<li><asp:HyperLink ID="hypSavedLists" runat="server" ToolTip="Saved Lists" NavigateUrl="~/MyAccount/account_selectsList.aspx"><span class="fa fa-file-text fa-fw"></span>&nbsp;Saved Lists</asp:HyperLink></li>--%>
            </ul>
        </div>
    </div>
</div>  
