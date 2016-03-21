<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LoginControlRD.ascx.vb" Inherits="CLibrary_LoginControlRD" %>
<%--User Menu.  Logged and Logged Out versions.--%>

<asp:Panel ID="pnlLoggedIn" runat="server" Visible="false" ClientIDMode="Static">
    <ul class="nav navbar-nav navbar-right">

        <li runat="server" id="liChat">
            <asp:HyperLink ID="hypChat" runat="server" NavigateUrl="#">
                <span class="glyphicon glyphicon-comment"></span>&nbsp;&nbsp;Chat
            </asp:HyperLink>
        </li>

        <li runat="server" id="liMyOrders">
            <asp:HyperLink ID="hypMyOrders" runat="server" NavigateUrl="#">
                <span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;My Orders
            </asp:HyperLink>
        </li>

        <li runat="server" id="liSavedMaps">
            <asp:HyperLink ID="hypSavedMaps" runat="server" NavigateUrl="#">
                <span class="glyphicon glyphicon-globe"></span>&nbsp;Saved Maps
            </asp:HyperLink>
        </li>

        <li runat="server" id="liCart">
            <asp:HyperLink ID="hypCart" runat="server" NavigateUrl="#">
                <span class="glyphicon glyphicon-shopping-cart"></span>&nbsp;My Cart
            </asp:HyperLink>
        </li>

        <li class="dropdown">
            <a href="#" class="dropdown-toggle" data-toggle="dropdown"><span class="glyphicon glyphicon-user"></span>&nbsp;My Profile<span class="caret"></span></a>
            <ul class="dropdown-menu" role="menu">
                <li><a href="#"><span class="glyphicon glyphicon-cog"></span>&nbsp;My Settings</a></li>
                <li><a href="#"><span class="glyphicon glyphicon-off"></span>&nbsp;Log Out</a></li>
            </ul>
        </li>

    </ul>
</asp:Panel>

<asp:Panel ID="pnlNotLoggedIn" runat="server" ClientIDMode="Static" Visible="true">
    <div role="form">
        <div class="nav navbar-nav navbar-right">
            <div class="row">

                <div class="col-sm-4">
                    <div class="form-group logInControls">
                        <label class="sr-only" for="txtUID">Email address</label>
                        <asp:TextBox CssClass="form-control-sm" ID="txtUID" runat="server" ClientIDMode="Static" />
                    </div>
                </div>

                <div class="col-sm-4">
                    <div class="form-group logInControls">
                        <label class="sr-only" for="txtPWD">Password</label>
                        <asp:TextBox ID="txtPWD" CssClass="form-control-sm" runat="server" TextMode="Password" ClientIDMode="Static" />
                    </div>
                </div>

                <div class="col-sm-4">
                    <asp:LinkButton ID="btnSignIn" runat="server" CssClass="btn btn-primary btn-xs" ClientIDMode="Static">
                        <span class="glyphicon glyphicon-log-in"></span>&nbsp;&nbsp;Sign In
                    </asp:LinkButton>
                </div>

            </div>

                        
            <div class="row">

                <div class="col-sm-4">
                    <div class="checkbox rememberMeChk">
                        <label>
                            <asp:CheckBox ID="chkRememberMe" runat="server" ClientIDMode="Static" />&nbsp;Remember me
                        </label>
                    </div>
                </div>

                <div class="col-sm-4">
                    <asp:HyperLink ID="hypForgotPWD" runat="server" NavigateUrl="~/forgotpassRD.aspx">Forgot Password?</asp:HyperLink>
                </div>

                <div class="col-sm-4">
                    <asp:HyperLink ID="hypRegister" runat="server" NavigateUrl="#">Register here.</asp:HyperLink>
                </div>

            </div>
               
            <div class="row">
                <div class="alert alert-danger" role="alert" id="logInErrorMsg">
                    <button type="button" class="close" data-dismiss="alert">
                    <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <strong>Oops</strong> - There was an issue with your User ID or Password. <br />
                    Please try again or click the Forgot Password link above.
                </div>
            </div>         

        </div>
    </div>
</asp:Panel>