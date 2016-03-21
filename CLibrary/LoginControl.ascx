<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LoginControl.ascx.vb"
    Inherits="CLibrary_LoginControl" %>
<asp:Panel ID="pAnon" runat="server" Visible="false">
    <div>
        <asp:Label ID="lLoginMessage" runat="server" Text="Sign In" />
        <asp:PlaceHolder ID="phSignUp" runat="server">
            <asp:Label ID="lSignupSeparator" runat="server" Text="|" />
            <asp:HyperLink ID="hplSignup" runat="server" Text="New User? Start here" NavigateUrl="~/account_signin.aspx" /></asp:PlaceHolder>
    </div>
    <div>
        <asp:TextBox ID="Username" runat="server" />
        <asp:TextBox ID="Password" runat="server" TextMode="Password" />
        <span class="actionlow inputheight">
            <asp:LinkButton ID="btnSignIn" runat="server" Text="Log-In" CssClass="makeButton" /></span>
    </div>
</asp:Panel>
<asp:Panel ID="pAuth" runat="server" Visible="false">
    <ul>
        <asp:PlaceHolder ID="phMyAccount" runat="server">
            <li class="first"><a title="My Account" target="_self" href="/myaccount">
                <img width="16" height="16" src="/cmsimages/1/icon_myaccount.png" alt="My Account Icon">
                My Account</a></li>
        </asp:PlaceHolder>
        <li><a title="My Cart" target="_self" href="/Step3-Checkout.aspx">
            <img width="16" height="16" src="/cmsimages/1/icon_cart.png" alt="My Cart Icon">
            My Cart</a></li>
        <li>
            <asp:HyperLink ID="hplLogonStatus" runat="server">
                <img width="16" height="16" src="/cmsimages/1/icon_login.png" alt="Security Lock" />
                <asp:Literal ID="lLogonStatus" runat="server" />
            </asp:HyperLink></li>
        <li class="last"><a title="Contact Us" target="_self" href="/contact-us">Contact Us</a></li></ul>
</asp:Panel>
