<%@ control language="VB" autoeventwireup="false" codefile="AccountHeaderMenu.ascx.vb" inherits="CCustom_AccountHeaderMenu" %>

<div id="userMenu">

    <asp:PlaceHolder runat="server" ID="pSSO">
        <asp:HyperLink runat="server" ID="hplSSOLogonPage" Text="Log-In" Visible="False" CssClass="btn btn-primary btn-sm loginButton" />
        <asp:PlaceHolder runat="server" ID="phSSOMulti" Visible="False">
            <div class="form-inline">
                <div class="form-group">
                    <div class="input-group">
                        <asp:DropDownList runat="server" ID="ddSSOLogonPages" CssClass="form-control input-sm">
                        </asp:DropDownList>
                        <div class="input-group-btn">
                            <asp:LinkButton ID="btnSSOSignIn" runat="server" CssClass="btn btn-primary btn-sm loginButton">
                    <i class="fa fa-sign-in"></i>&nbsp;<span class="hidden-xs hidden-sm">Sign In</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

        </asp:PlaceHolder>
    </asp:PlaceHolder>


    <asp:PlaceHolder ID="pAnon" runat="server" Visible="false">
        <asp:Panel ID="pSigninBox" runat="server" DefaultButton="btnSignIn" CssClass="row">

            <div class="form-inline" id="loginBlock">

                <div class="extraLeftPadding"><strong><asp:Literal ID="litWelcome" runat="server" /></strong></div>

                <div class="form-group col-sm-5">
                    <asp:Label runat="server" ID="lblUsername" AssociatedControlID="Username" CssClass="sr-only" Text="Username" />
                    <asp:TextBox ID="Username" runat="server" placeholder="Username" CssClass="form-control input-sm" ClientIDMode="Static" />
                </div>

                <div class="form-group col-sm-7">
                    <asp:Label runat="server" ID="lblPassword" AssociatedControlID="Password" CssClass="sr-only" Text="Password" />
                    <div class="input-group">
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" placeholder="Password" CssClass="form-control input-sm" />
                        <div class="input-group-btn">
                            <asp:LinkButton ID="btnSignIn" runat="server" CssClass="btn btn-primary btn-sm loginButton">
                                <i class="fa fa-sign-in"></i>&nbsp;<span class="hidden-xs hidden-sm">Sign In</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>

            </div>

        </asp:Panel>

        <div class="row">
            
            <div id="signupBlock">
                <div class="col-xs-5">
                    <asp:PlaceHolder ID="phRegister" runat="server">
                        <asp:HyperLink ID="hplSignup" runat="server" Text="Register Here" NavigateUrl="~/account_signin.aspx" />
                    </asp:PlaceHolder>&nbsp;
                </div>

                <div class="col-xs-7">
                    <asp:HyperLink ID="hplForgot" runat="server" Text="Forgot Password?" NavigateUrl="~/forgotpass.aspx" />
                </div>
            </div>
            
        </div>

        <asp:Literal ID="lLogonMsg" runat="server" />

    </asp:PlaceHolder>


    <asp:PlaceHolder ID="pAuth" runat="server" Visible="false" ClientIDMode="Static">
        
        <div id="authorizedBlock">

            <strong><asp:Label ID="lWelcomeMessage" runat="server" /></strong>

            <ul class="list-inline">
                <asp:PlaceHolder ID="phMyAccount" runat="server">
                    <li class="first"><a title="Account Settings" target="_self" href="/myaccount"><span class="fa fa-cog hidden-sm"></span>&nbsp;Settings</a></li>
                    <li><a title="Order History" href="/myaccount/account_orders.aspx"><span class="fa fa-folder-open hidden-sm"></span>&nbsp;Orders</a></li>
                    <li><a title="Saved U-Select Maps" href="/myaccount/account_selects.aspx"><span class="fa fa-map-marker hidden-sm"></span>&nbsp;Maps</a></li>
                </asp:PlaceHolder>

                <asp:Placeholder ID="phFullCart" runat="server" Visible="False">
                    <li><asp:HyperLink ID="hypMyCart" runat="server" ToolTip="My Cart" NavigateUrl="/Step3-Checkout.aspx"><i class="fa fa-shopping-cart hidden-sm"></i>&nbsp;My Cart</asp:HyperLink></li>
                </asp:Placeholder>
            
                <asp:Placeholder ID="phEmptyCart" runat="server" Visible="False">
                    <li><asp:HyperLink ID="hypEmptyCart" runat="server" ToolTip="My Cart"><i class="fa fa-shopping-cart hidden-sm"></i>&nbsp;(Cart is empty)</asp:HyperLink></li>
                </asp:Placeholder>

                <li><asp:HyperLink ID="hplLogoff" runat="server" NavigateUrl="~/logout.aspx"><i class="fa fa-sign-out hidden-sm"></i>&nbsp;Sign Out</asp:HyperLink></li>
            </ul>

        </div>

    </asp:PlaceHolder>

</div>