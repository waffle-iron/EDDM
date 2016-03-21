<%@ control language="VB" autoeventwireup="false" codefile="StaplesLogIn.ascx.vb" inherits="StaplesLogIn" %>

<div id="loginWrapper" class="extraBottomPadding">

    <div id="userMenu">

        <div class="container">
            <div class="row">

                <div class="col-sm-4 text-left">
                    
                    <div class="hidden-xs"><img src="/cmsimages/93/header-logo-white.png" alt="Staples" /></div>

                    <a href="http://www.staples.com/" target="_blank" title="Staples Direct Mail">
                    <span class="fa fa-chevron-circle-left"></span>&nbsp;Back to Staples
                    </a>&nbsp;

                    <a href="http://www.staples.com/sbd/content/copyandprint/index.html" target="_blank" title="Staples Direct Mail">
                    <span class="fa fa-chevron-circle-left"></span>&nbsp;Back to copy&amp;print
                    </a>

                </div>


                <div class="col-sm-8">
                    
                    <%--Anonymous Menu--%>
                    <asp:PlaceHolder ID="pAnon" runat="server" Visible="false">

                        <asp:Panel ID="pSigninBox" runat="server" DefaultButton="btnSignIn">

                            <div class="clearfix">

                                <div class="text-right">

                                    <asp:PlaceHolder ID="phRegisterButton" runat="server">
                                        <div class="btn-group">
                                            <a href="/account_signin.aspx" class="btn btn-default btn-sm loginButton">
                                                <span class="fa fa-user pr-10"></span> Register
                                            </a>
                                        </div>
                                    </asp:PlaceHolder>

                                    <div class="btn-group dropdown">

                                        <button type="button" class="btn dropdown-toggle btn-default btn-sm loginButton" data-toggle="dropdown" aria-expanded="true">
                                            <span class="fa fa-lock pr-10"></span> Login
                                        </button>

                                        <ul id="loginDropDownMenu" class="dropdown-menu dropdown-menu-right">
                                            <li>

                                                <div class="margin-clear">

                                                    <p><asp:Literal ID="litWelcome" runat="server" /></p>

                                                    <div class="form-group has-feedback">
                                                        <label class="control-label">Username</label>
                                                        <asp:TextBox ID="Username" runat="server" placeholder="Username" CssClass="form-control input-sm" ClientIDMode="Static" />
                                                        <span class="fa fa-user form-control-feedback"></span>
                                                    </div>

                                                    <div class="form-group has-feedback">
                                                        <label class="control-label">Password</label>
                                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" placeholder="Password" CssClass="form-control input-sm" />
                                                        <span class="fa fa-lock form-control-feedback"></span>
                                                    </div>

                                                    <div class="form-group" style="visibility:hidden">
                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkUseStaplesAcct" runat="server" /> Log in with your Staples.com account
                                                            </label>
                                                        </div>
                                                    </div>


                                                    <div class="text-right">
                                                        <asp:LinkButton ID="btnSignIn" runat="server" CssClass="btn btn-lg btn-danger loginButton" ClientIDMode="static">
                                                            <span class="fa fa-sign-in"></span>&nbsp;Sign In
                                                        </asp:LinkButton>
                                                    </div>

                                                    <div>&nbsp;</div>

                                                    <div class="row">
                                                        <div class="col-sm-6 text-center">
                                                            <asp:HyperLink ID="hplForgot" runat="server" NavigateUrl="~/forgotpass.aspx">
                                                                <span class="fa fa-question-circle"></span>&nbsp;Forgot Password
                                                            </asp:HyperLink>
                                                        </div>
                                                        <div class="col-sm-6 text-center">
                                                            <asp:PlaceHolder ID="phRegister" runat="server">
                                                                <asp:HyperLink ID="hplSignup" runat="server" NavigateUrl="~/account_signin.aspx">
                                                                    <span class="fa fa-user-plus"></span>&nbsp;Create Account
                                                                </asp:HyperLink>
                                                            </asp:PlaceHolder>
                                                        </div>
                                                    </div>


                                                </div>

                                            </li>
                                        </ul>

                                    </div>

                                </div>

                            </div>

                        </asp:Panel>

                        <div id="logonMessage">
                            <asp:Literal ID="lLogonMsg" runat="server" />
                        </div>

                    </asp:PlaceHolder>

                    <%--Authenticated Menu--%>
                    <asp:PlaceHolder ID="pAuth" runat="server" Visible="false" ClientIDMode="Static">
        
                        <div id="authorizedBlock" class="text-right">

                            <strong><asp:Label ID="lWelcomeMessage" runat="server" /></strong>

                            <ul class="list-inline">
                                <asp:PlaceHolder ID="phMyAccount" runat="server">
                                    <li><a title="Account Settings" target="_self" href="/MyAccount/account_manage.aspx"><span class="fa fa-cog hidden-sm"></span>&nbsp;Settings</a></li>
                                    <li><a title="Order History" href="/MyAccount/account_orders.aspx"><span class="fa fa-folder-open hidden-sm"></span>&nbsp;Orders</a></li>
                                    <li><a title="Saved Maps" href="/MyAccount/account_selects.aspx"><span class="fa fa-map-marker hidden-sm"></span>&nbsp;Saved Maps</a></li>
                                    <li><a title="Saved Lists" href="/MyAccount/account_SavedLists.aspx"><span class="fa fa-list-alt hidden-sm"></span>&nbsp;Saved Lists</a></li>
                                </asp:PlaceHolder>

                                <asp:Placeholder ID="phFullCart" runat="server" Visible="False">
                                    <li><asp:HyperLink ID="hypMyCart" runat="server" ToolTip="My Cart" NavigateUrl="/Step3-Checkout.aspx"><i class="fa fa-shopping-cart hidden-sm"></i>&nbsp;My Cart</asp:HyperLink></li>
                                </asp:Placeholder>
            
                                <asp:Placeholder ID="phEmptyCart" runat="server" Visible="False">
                                    <li><asp:HyperLink ID="hypEmptyCart" runat="server" ToolTip="My Cart"><i class="fa fa-shopping-cart hidden-sm"></i>&nbsp;(empty)</asp:HyperLink></li>
                                </asp:Placeholder>

                                <li><asp:HyperLink ID="hplLogoff" runat="server" NavigateUrl="~/logout.aspx"><i class="fa fa-sign-out hidden-sm"></i>&nbsp;Sign Out</asp:HyperLink></li>
                            </ul>

                        </div>

                    </asp:PlaceHolder>

                </div>

            </div>
        </div>

    </div>



</div>


<script type="text/javascript">

    $('#loginDropDownMenu').on('click', function (event)
    {
        event.stopPropagation();
    });

</script>