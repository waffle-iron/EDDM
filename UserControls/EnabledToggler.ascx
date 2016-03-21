<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EnabledToggler.ascx.vb"
    Inherits="UserControls_EnabledToggler" %>
<div class="ui-widget" style="font-size: 0.9em; padding: 0.25em; margin-bottom: 0.25em;">
    <asp:Panel ID="pStatus" runat="server">
        <asp:Label ID="lIcon" runat="server" Style="float: left; margin-right: .3em;" />
        <div style="line-height: 16px;">
            <asp:Literal ID="lMsg" runat="server" /> <asp:LinkButton ID="lnkSetStatus" runat="server" CssClass="makeButton" style="margin:1em;" /></div>
    </asp:Panel>
</div>
