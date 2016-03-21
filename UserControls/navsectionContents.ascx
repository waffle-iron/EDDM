<%@ Control Language="VB" AutoEventWireup="false" CodeFile="navsectionContents.ascx.vb" Inherits="usercontrols_navsectionContents" %>
<asp:SiteMapDataSource id="oSM" runat="server" SiteMapProvider="AdminSiteMap" StartFromCurrentNode="true" />
<div style="padding:10px;">
    <asp:TreeView id="tvSummary" runat="server" DataSourceID="oSM" MaxDataBindDepth="1" ShowExpandCollapse="false" NodeStyle-VerticalPadding="5px" ShowLines="true">
    </asp:TreeView>
</div>
