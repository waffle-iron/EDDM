<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cmsRolePicker.ascx.vb" Inherits="UserControls_cmsRolePicker" %>
<asp:TextBox ID="SelectedRoles" runat="server" /> <asp:LinkButton ID="lnkRoleBrowser" runat="server">
    <asp:Image ID="imgRoleBrowser" runat="server" ImageUrl="~/admin/images/magnifier.png" /></asp:LinkButton>
    
<asp:Panel ID="pCMSRoleBrowser" runat="server" Style="display: none;" ToolTip="Choose Roles">
    <asp:HiddenField ID="hfSelRoles" runat="server" />
    <asp:Literal ID="lCMSRoleBrowser" runat="server" />
</asp:Panel>
