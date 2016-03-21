<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cmsImageBrowser.ascx.vb"
    Inherits="UserControls_cmsImageBrowser" %>
<asp:TextBox ID="SelectedImage" runat="server" />
<asp:LinkButton ID="lnkCMSImageBrowser" runat="server">
    <asp:Image ID="imgCMSImageBrowser" runat="server" ImageUrl="~/admin/images/magnifier.png" /></asp:LinkButton>
<asp:Panel ID="pCMSImageBrowser" runat="server" Style="display: none;" ToolTip="Choose Image">
    <asp:HiddenField ID="hfSelImg" runat="server" />
    <asp:Literal ID="lCMSImageBrowser" runat="server" />
</asp:Panel>
