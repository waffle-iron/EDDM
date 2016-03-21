<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BreadCrumbs.ascx.vb" Inherits="CLibrary_BreadCrumbs" %>
<asp:Panel ID="pBreadCrumbs" runat="server">
<asp:ListView ID="lvCrumbs" runat="server" ItemPlaceholderID="phItemTemplate">
    <LayoutTemplate>
        <div class="breadcrumbs"><asp:PlaceHolder ID="phItemTemplate" runat="server" /></div>
    </LayoutTemplate>
    <ItemTemplate>
        <asp:HyperLink ID="hplLink" runat="server" Text='<%#Eval("InnerHtml") %>' NavigateUrl='<%#Eval("Url") %>' />
    </ItemTemplate>
    <ItemSeparatorTemplate><%=Me.Separator%></ItemSeparatorTemplate>
</asp:ListView>
</asp:Panel>