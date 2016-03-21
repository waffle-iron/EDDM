<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Site.master" AutoEventWireup="false" CodeFile="Logon.aspx.vb" Inherits="_Logon" Title="Authentication Required" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phForm" runat="server">
    
    <div class="container">

        <div class="fullRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            Log In
            <span class="arrowRight"></span>
        </div>        

        <div class="fullRibbonSmall visible-sm visible-xs">
            Log In
        </div>        


        <div class="contentWrapper">

            <div class="row">
                <div class="col-sm-4 col-sm-offset-4">
                    
                    <asp:Panel ID="pLogin" runat="server" CssClass="well well-sm">
                        <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/Default.aspx" OnLoad="LoadLogin" RenderOuterTable="False">
                            <LayoutTemplate>

                                <p><strong>Existing users: enter your username and your password to sign in.</strong></p>

                                <div><asp:Literal ID="FailureText" runat="server" />&nbsp;</div>
                                
                                <asp:ValidationSummary ID="vSumm" runat="server" ValidationGroup="vgLogon" />

                                    <div class="form-group">
                                        <label for="UserName" class="label formLabelRequired">UserName</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="UserName" runat="server" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="Username" Display="dynamic" CssClass="text-danger" SetFocusOnError="True" ErrorMessage="Your username is required." ValidationGroup="vgLogon">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="Password" class="label formLabelRequired">Password</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="Password" Display="dynamic" CssClass="text-danger" SetFocusOnError="True" ErrorMessage="Your password is required." ValidationGroup="vgLogon">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="text-center">
                                        <p>&nbsp;</p>
                                        <asp:Button ID="Login" CommandName="Login" runat="server" Text="Login" ValidationGroup="vgLogon" CssClass="btn btn-primary btn-lg" />
                                    </div>

                            </LayoutTemplate>
                        </asp:Login>

                        <p>&nbsp;</p>

                        <p class="text-center"><a href="/RecoverPassword.aspx">Forgot Your Password?</a></p>

                        <asp:PlaceHolder ID="pSignup" runat="server">
                            <p><a href="/Signup.aspx">Don't have an account yet?</a></p>
                        </asp:PlaceHolder>
                    </asp:Panel>
       
                </div>
            </div>





        </div>

    </div>


</asp:Content>
