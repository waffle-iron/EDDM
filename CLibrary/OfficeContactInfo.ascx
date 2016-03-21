<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OfficeContactInfo.ascx.vb"
    Inherits="CCustom_OfficeContactInfo" %>
<ul class="contactInfo">
    <asp:PlaceHolder ID="phOfficeName" runat="server" Visible="false">
        <li><b><asp:Literal ID="lOfficeName" runat="server" /></b></li>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phAddress" runat="server">
        <li class="address">
            <div>
                <asp:Literal ID="lAddress1" runat="server" /></div>
            <asp:Panel ID="pAddress2" runat="server" Visible="false">
                <asp:Literal ID="lAddress2" runat="server" />
            </asp:Panel>
            <div>
                <asp:Literal ID="lCity" runat="server" />,
                <asp:Literal ID="lState" runat="server" />&nbsp;<asp:Literal ID="lZip" runat="server" /></div>
        </li>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phEmail" runat="server" Visible="false">
        <li class="email">
            <asp:HyperLink ID="hplEmail" runat="server" /></li>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phPhone" runat="server" Visible="false">
        <li class="phone">
            <asp:Literal ID="lPhone" runat="server" /></li>
    </asp:PlaceHolder>
</ul>
