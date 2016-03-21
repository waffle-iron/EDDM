<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LoginStatus.ascx.vb"
    Inherits="CLibrary_LoginStatus" %>
<asp:Label ID="pAnon" runat="server" Visible="false">
    <asp:HyperLink ID="hplSignIn" runat="server" Text="Sign In" />
    <asp:PlaceHolder ID="phSignup" runat="server">
    |
    <asp:HyperLink ID="hplSignUp" runat="server" Text="Sign Up" />
    </asp:PlaceHolder>
</asp:Label>
<asp:Label ID="pAuth" runat="server" Visible="false">
    <asp:HyperLink ID="hplMyAccount" runat="server" NavigateUrl="~/member.aspx" Text="My Account" />
    |
    <asp:HyperLink ID="hplAdmin" runat="server" Visible="false" Text="Admin Home" /><asp:Literal ID="lAdminSep"
        runat="server" /><asp:HyperLink ID="hplSignOut" runat="server" Text="Sign Out" />
</asp:Label>
