<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cmsPageSearch.ascx.vb"
    Inherits="usercontrols_cmsPageSearch" %>
<asp:Panel ID="pSearch" runat="server" DefaultButton="btnSearch" CssClass="right">
    <asp:ObjectDataSource ID="oTemplates" runat="server" SelectMethod="GetPageTemplates"
        TypeName="CMSDAL" />
    <div>
        <asp:TextBox ID="PageName" runat="server" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="makeSearchButton" /></div>
</asp:Panel>
