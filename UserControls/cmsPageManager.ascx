<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cmsPageManager.ascx.vb"
    Inherits="UserControls_cmsPageManager" %>
<asp:Panel ID="pManage" runat="server" CssClass="pgAdmin">
    <div class="ui-helper-clearfix ui-widget ui-widget-content ui-corner-top" style="margin: 0 1em 0 1em;
        padding: 0.5em; font-size:smaller;display:inline-block;float:left;">
        <asp:HyperLink ID="hplAdminHome" runat="server" Text="Admin Home" CssClass="makeButton" NavigateUrl="/admin" />
    </div>
    <asp:Panel ID="pPage" runat="server" CssClass="ui-helper-clearfix ui-widget ui-widget-content ui-corner-top" style="margin: 0 1em 0 1em;
        padding: 0.5em; font-size:smaller;display:inline-block;float:left;">
        <asp:HyperLink ID="hplManage" runat="server" Text="Edit Page" CssClass="makeEditButton" />
        <%--<asp:HyperLink ID="hplPageHeader" runat="server" Text="Edit Page Header" CssClass="makeEditButton" />
        <asp:HyperLink ID="hplPageNav" runat="server" Text="Edit Page Nav" CssClass="makeEditButton" />
        <asp:HyperLink ID="hplPageFooter" runat="server" Text="Edit Page Footer" CssClass="makeEditButton" />--%>
    </asp:Panel>
    <div class="ui-helper-clearfix ui-widget ui-widget-content ui-corner-top" style="margin: 0 1em 0 1em;
        padding: 0.5em; font-size:smaller;display:inline-block;float:left;">
        <asp:HyperLink ID="hplSiteHeader" runat="server" Text="Edit Site Header" CssClass="makeEditButton" />
        <asp:HyperLink ID="hplSiteNav" runat="server" Text="Edit Site Nav" CssClass="makeEditButton" />
        <asp:HyperLink ID="hplSiteFooter" runat="server" Text="Edit Site Footer" CssClass="makeEditButton" />        
    </div>
    <div class="ui-helper-clearfix ui-widget ui-widget-content ui-corner-top" style="margin: 0 1em 0 1em; padding: 0.5em; font-size:smaller;display:inline-block;float:left;">
        <asp:HyperLink ID="hplValidate" runat="server" Text="Validate Page HTML" CssClass="makeCheckButton" />
    </div>
</asp:Panel>
