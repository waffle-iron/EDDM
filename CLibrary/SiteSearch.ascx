<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SiteSearch.ascx.vb" Inherits="CLibrary_SiteSearch" %>
<asp:Panel ID="pSiteSearch" runat="server" DefaultButton="lnkSearch">
    <asp:TextBox ID="SearchText" runat="server" CssClass="searchText" />
    <asp:Button ID="lnkSearch" runat="server" Text="Go" CssClass="searchButton" CausesValidation="false" />
</asp:Panel>
