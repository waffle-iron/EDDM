<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageHeader.ascx.cs" Inherits="CCustom_PageHeader" %>

<asp:Panel ID="pnlSimple" runat="server" Visible="False">
    <div class="simpleHeaderWrapper">
        <h1><asp:Literal ID="litSimpleHeader" runat="server" /></h1>
    </div>
</asp:Panel>


<asp:Panel ID="pnlFullPage" runat="server" Visible="False">
        
    <div class="fullRibbon hidden-sm hidden-xs">
        <span class="arrowLeft"></span>
        <asp:Literal ID="litFullHeader" runat="server" />
        <span class="arrowRight"></span>
    </div>        

    <div class="fullRibbonSmall visible-sm visible-xs">
        <asp:Literal ID="litFullHeaderSmall" runat="server" />
    </div>        

</asp:Panel>



<asp:Panel ID="pnlPartial" runat="server" Visible="False">

    <div class="partialRibbon hidden-sm hidden-xs">
        <span class="arrowLeft"></span>
        <span class="subRibbonPop"><asp:Literal ID="litMainHeader" runat="server" /></span>
        <span class="subRibbon"><asp:Literal ID="litSubHeader" runat="server" /></span>
    </div>

    <div class="partialRibbonSmall visible-sm visible-xs">
        <span class="subRibbonPopSmall"><asp:Literal ID="litMainHeaderSmall" runat="server" /></span>
        <span class="subRibbonSmall"><asp:Literal ID="litSubHeaderSmall" runat="server" /></span>
    </div>

</asp:Panel>
