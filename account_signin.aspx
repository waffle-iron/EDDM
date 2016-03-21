<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Site.master" AutoEventWireup="false" CodeFile="account_signin.aspx.vb" Inherits="account_signin" Title="Login | Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phForm" runat="Server">

    <div class="container">

        <div class="fullRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
                Register for an Account
            <span class="arrowRight"></span>
        </div>        

        <div class="fullRibbonSmall visible-sm visible-xs">
            Register for an Account
        </div>        

        <div class="contentWrapper">

            <div class="row">

                <%--Log In Form--%>
                <div class="col-md-5" id="loginCol" runat="server">

                    <asp:PlaceHolder runat="server" ID="phLoginForm">
                        <div class="panel panel-default">

                        <div class="panel-heading">
                            <div class="panel-title">Existing Users</div>
                        </div>

                        <div class="panel-body">

                            <p><asp:Literal ID="litLogInMsg" runat="server" /></p>

                            <asp:Panel ID="pLogin" runat="server">
                                <asp:Login ID="Login1" runat="server" DisplayRememberMe="false" Width="100%" OnLoad="LoadLogin">
                                    <LayoutTemplate>

                                        <p><asp:Literal ID="FailureText" runat="server" />&nbsp;</p>
                                        
                                        <asp:ValidationSummary ID="vSumm" runat="server" ValidationGroup="vgLogon" CssClass="alert alert-danger" />

                                        <div class="form-group">
                                            <label for="UserName">User Name</label>
                                            <asp:TextBox ID="UserName" runat="server" CssClass="form-control input-sm" />
                                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="UserName" ErrorMessage="Your user name is required." ValidationGroup="vgLogon" CssClass="label label-danger">
                                                User Name is required.
                                            </asp:RequiredFieldValidator>
                                        </div>

                                        <div class="form-group">
                                            <label for="Password">Password</label>
                                            <asp:TextBox ID="Password" runat="server" CssClass="form-control input-sm" TextMode="Password" />
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ValidationGroup="vgLogon" CssClass="label label-danger">
                                                Password is required.
                                            </asp:RequiredFieldValidator>
                                        </div>

                                        <div>
                                            <asp:Button ID="Login" CommandName="Login" runat="server" Text="Login" ValidationGroup="vgLogon" CssClass="btn btn-lg btn-primary pull-right" />
                                        </div>

                                        <div>&nbsp;</div>

                                        <div>&nbsp;</div>

                                        <div>&nbsp;</div>


                                    </LayoutTemplate>
                                </asp:Login>

                                <div>
                                    <p class="text-center"><asp:HyperLink ID="hypForgotPWD" runat="server" NavigateUrl="~/forgotpass.aspx">Forgot Your Password?</asp:HyperLink></p>
                                </div>

                            </asp:Panel>

                            <%--Register--%>
                            <asp:PlaceHolder runat="server" ID="pSSOLogin" Visible="false">
                                <asp:ListView runat="server" ID="lvSSOLogonPages" ItemPlaceholderID="phItemTemplate">
                                    <LayoutTemplate>
                                        <ul class="list-group">
                                            <asp:PlaceHolder runat="server" ID="phItemTemplate"></asp:PlaceHolder>
                                        </ul>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <li class="list-group-item"><asp:HyperLink runat="server" ID="hplLogon" NavigateUrl='<%#Eval("Url") %>'><%#Eval("Name")%></asp:HyperLink></li>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:PlaceHolder>

                        </div>
                    </div>
                    </asp:PlaceHolder>

                </div>

                <%--Register Form--%>
                <div class="col-md-7" id="registerCol" runat="server">
                    
                    <asp:PlaceHolder runat="server" ID="pNativeRegister">

                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="panel-title">Register</div>
                            </div>
    
                            <div class="panel-body">
                                
                                <div class="well well-sm">
                                    
                                    <p>For access to our suite of marketing resources and tools, create your account. Don't worry, it only takes a few seconds.</p>

                                    <asp:ValidationSummary ID="vsAccount" runat="server" HeaderText="Please complete the required fields" CssClass="alert alert-danger" />

                                    <%--Email--%>
                                    <div class="form-group">
                                        <label for="EmailAddress" class="label formLabelRequired">Email</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="EmailAddress" runat="server" MaxLength="100" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvEmailAddress" CssClass="text-danger" ControlToValidate="EmailAddress" runat="server" ErrorMessage="Your email address is a required field." Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revEmailAddress" runat="server" ErrorMessage="A valid e-email address is required." Text=" (*)" SkinID="ShowOnLoad" ControlToValidate="EmailAddress" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <%--Password--%>
                                    <div class="form-group">
                                        <label for="Password" class="label formLabelRequired">Password</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="Password" TextMode="Password" runat="server" MaxLength="100" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvPassword" CssClass="text-danger" ControlToValidate="Password" runat="server" ErrorMessage="A password is required." Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <%--Confirm Password--%>
                                    <div class="form-group">
                                        <label for="ConfirmPassword" class="label formLabelRequired">Confirm Password</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="ConfirmPassword" TextMode="Password" runat="server" MaxLength="100" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvConfirmPassword" CssClass="text-danger" ControlToValidate="ConfirmPassword" runat="server" ErrorMessage="A password confirmation is required." Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="ConfirmPassword" ControlToCompare="Password" ErrorMessage="Your passwords do not match!">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:CompareValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <%--Company--%>
                                    <div class="form-group">
                                        <label for="Company" class="label formLabelRequired">Company</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="Company" runat="server" MaxLength="100" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvCompany" CssClass="text-danger" ControlToValidate="Company" runat="server" ErrorMessage="The company name is required." Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <%--First Name & Last Name--%>
                                    <div class="row">
                                
                                        <%--First Name--%>
                                        <div class="col-xs-6">
                                            <div class="form-group">
                                                <label for="FirstName" class="label formLabelRequired">First Name</label>
                                                <div class="row">
                                                    <div class="col-xs-9">
                                                        <asp:TextBox ID="FirstName" runat="server" MaxLength="50" CssClass="form-control" />
                                                    </div>
                                                    <div class="col-xs-2">
                                                        <asp:RequiredFieldValidator ID="rfvFirstName" CssClass="text-danger" ControlToValidate="FirstName" runat="server" ErrorMessage="Your first name is required." Display="Dynamic">
                                                            <span class="fa fa-2x fa-exclamation-circle"></span>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--Last Name--%>
                                        <div class="col-xs-6">
                                            <div class="form-group">
                                                <label for="LastName" class="label formLabelRequired">Last Name</label>
                                                    <div class="row">
                                                        <div class="col-xs-9">
                                                            <asp:TextBox ID="LastName" runat="server" MaxLength="50" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-xs-2">
                                                            <asp:RequiredFieldValidator ID="rfvLastName" CssClass="text-danger" ControlToValidate="LastName" runat="server" ErrorMessage="Your last name is required." Display="Dynamic">
                                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                                            </asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cvLastName" runat="server" ErrorMessage="Last name cannot be the same as first name." ControlToCompare="FirstName" ControlToValidate="LastName" Operator="NotEqual" Display="Dynamic">
                                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                                            </asp:CompareValidator>
                                                        </div>
                                                    </div>
                                            </div>
                                        </div>

                                    </div>

                                    <%--Address--%>
                                    <div class="form-group">
                                        <label for="Address1" class="label formLabelRequired">Address 1</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="Address1" runat="server" MaxLength="50" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvAddress1" CssClass="text-danger" ControlToValidate="Address1" runat="server" ErrorMessage="Your street address is required." Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <%--Address2--%>
                                    <div class="form-group">
                                        <label for="Address2" class="label formLabel">Address 2</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="Address2" runat="server" MaxLength="50" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                            </div>
                                        </div>
                                    </div>

                                    <%--City--%>
                                    <div class="form-group">
                                        <label for="City" class="label formLabelRequired">City</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="City" runat="server" MaxLength="50" CssClass="form-control" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvCity" CssClass="text-danger" ControlToValidate="City" runat="server" ErrorMessage="City is required." Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <%--State & Zip--%>
                                    <div class="row">
                                
                                        <%--State--%>
                                        <div class="col-xs-6">
                                            <div class="form-group">
                                                <label for="State" class="label formLabelRequired">State</label>
                                                <div class="row">
                                                    <div class="col-xs-9">
                                                        <geoselect:state id="State" runat="server" allowinternational="false" showabbronly="false" requirefield="true" validationside="Right" validationerrormessage="The state is required." validationtext="" />
                                                    </div>
                                                    <div class="col-xs-2">&nbsp;</div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--Zip--%>
                                        <div class="col-xs-5">
                                            <div class="form-group">
                                                <label for="PostalCode" class="label formLabelRequired">Zip Code</label>
                                                    <div class="row">
                                                        <div class="col-xs-9">
                                                            <asp:TextBox ID="PostalCode" runat="server" MaxLength="10" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-xs-2">
                                                            <asp:RequiredFieldValidator ID="rfvPostalCode" CssClass="text-danger" ControlToValidate="PostalCode" runat="server" ErrorMessage="Zip Code is required." Display="Dynamic">
                                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="revPostalCode" ControlToValidate="PostalCode" runat="server" ErrorMessage="Please provide a valid postal code (5-10 characters)." ValidationExpression="[A-Za-z0-9\-]{5,10}" Display="Dynamic">
                                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                            </div>
                                        </div>

                                    </div>

                                    <%--Phone & Ext--%>
                                    <div class="row">
                                
                                        <div class="col-xs-6">
                                            <div class="form-group">
    
                                                <label for="DirectLine" class="label formLabelRequired">Phone</label>
                                                <div class="row">
                                                    <div class="col-xs-9">
                                                        <asp:TextBox ID="DirectLine" runat="server" MaxLength="15" CssClass="form-control" ClientIDMode="Static" />
                                                    </div>
                                                    <div class="col-xs-2">
                                                        <asp:RequiredFieldValidator ID="rfvDirectLine" CssClass="text-danger" ControlToValidate="DirectLine" runat="server" ErrorMessage="A phone number is required." Display="Dynamic">
                                                            <span class="fa fa-2x fa-exclamation-circle"></span>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="col-xs-5">
                                            <label for="DirectLineExt" class="label formLabel">Extension</label>
                                            <div class="row">
                                                <div class="col-xs-9">
                                                    <asp:TextBox ID="DirectLineExt" runat="server" MaxLength="10" CssClass="form-control" />
                                                </div>
                                                <div class="col-xs-2">
                                                    &nbsp;
                                                </div>
                                            </div>
                                          
                                        </div>
                                    </div>

                                    <%--Business Type --%>
                                    <div class="form-group">
                                        <label for="BusinessType" class="label formLabelRequired">Business Type</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="form-control" ClientIDMode="Static">
                                                    <asp:ListItem Value="" Selected="True">Select One</asp:ListItem>
                                                    <asp:ListItem Value="Agency">Agency</asp:ListItem>
                                                    <asp:ListItem Value="Franchisor">Franchisor</asp:ListItem>
                                                    <asp:ListItem Value="Franchisee">Franchisee</asp:ListItem>
                                                    <asp:ListItem Value="Small Business">Small Business</asp:ListItem>
                                                    <asp:ListItem Value="Corporate">Corporate </asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvBusinessType" CssClass="text-danger" ControlToValidate="ddlBusinessType" runat="server" ErrorMessage="Please choose a Business Type." Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <%--Industry--%>
                                    <div class="form-group">
                                        <label for="Industry" class="label formLabelRequired" id="lblIndustry">Industry</label>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <asp:DropDownList ID="ddlIndustry" runat="server" CssClass="form-control" ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:RequiredFieldValidator ID="rfvIndustry" CssClass="text-danger" ControlToValidate="ddlIndustry" runat="server" ErrorMessage="Please choose a Industry." Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                     <%--Newsletter --%>        
                                    <asp:PlaceHolder ID="phNewsletter" runat="server" Visible="True">
                                        <div class="form-group">
                                            <label>
                                            <asp:CheckBox ID="Eletter" runat="server" Checked="False" />
                                            Subscribe to our mailing list. Get weekly advertising tips and discounts via e-mail.
                                            </label>
                                        </div>
                                    </asp:PlaceHolder>

                                    <%--Button--%>
                                    <div class="text-right">
                                        <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary btn-lg" Text="Create My Account" ClientIDMode="Static" />
                                    </div>

                                </div>
                           
                            </div>

                        </div>

                    </asp:PlaceHolder>

                </div>

            </div>

        </div>

    </div>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="cpScripts" runat="Server">
    
    <script src="/assets/javascripts/PhoneMask.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#DirectLine").inputmask("mask", { "mask": "(999) 999-9999" }); 
        });
    </script>

</asp:Content>




