<%@ Control Language="C#" AutoEventWireup="true" CodeFile="YourAccountUserMenu.ascx.cs" Inherits="Controls_YourAccountUserMenu" %>

<div class="panel panel-default">

    <div class="panel-heading">
        Your Account
    </div>

    <div class="panel-body">
        <div class="leftNav">
            <ul class="list-unstyled">
                <li><asp:HyperLink ID="hypAccountSettings" runat="server" NavigateUrl="~/MyAccount/account_manage.aspx" ToolTip="Account Settings"><span class="fa fa-cog fa-fw"></span>&nbsp;Account Settings</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypOrderHistory" runat="server" ToolTip="Order History" NavigateUrl="~/MyAccount/account_orders.aspx"><span class="fa fa-folder-open fa-fw"></span>&nbsp;Order History</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypSavedMaps" runat="server" ToolTip="Saved maps" NavigateUrl="~/MyAccount/account_selects.aspx"><span class="fa fa-map-marker fa-fw"></span>&nbsp;Your EDDM Maps</asp:HyperLink></li>

                <asp:PlaceHolder ID="phSavedLists" runat="server" Visible="False">
                    <li><asp:HyperLink ID="hypSavedLists" runat="server" ToolTip="Saved maps" NavigateUrl="~/MyAccount/account_SavedLists.aspx"><span class="fa fa-map-marker fa-fw"></span>&nbsp;Your Addressed Lists</asp:HyperLink></li>
                </asp:PlaceHolder>

            </ul>
        </div>
    </div>

</div>  
