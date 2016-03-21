<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" Trace="false" AutoEventWireup="true" CodeFile="Step1-TargetReview.aspx.vb" Inherits="Step1_TargetReview" %>
<%@ Register Src="~/CLibrary/ProductListWithQuote.ascx" TagPrefix="eddm" TagName="ProductListWithQuote" %>
<%@ Register Src="~/CLibrary/VisualWebsiteOptimizer.ascx" TagPrefix="eddm" TagName="VisualWebsiteOptimizer" %>
<%@ Register Src="~/CCustom/BoldChatConversion.ascx" TagPrefix="eddm" TagName="BoldChatConversion" %>
<%@ Register Src="~/Controls/OrderSteps.ascx" TagPrefix="eddm" TagName="OrderSteps" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="eddm" TagName="PageHeader" %>
<%@ Register Src="~/CCustom/1/FacebookTrackingPixel.ascx" TagPrefix="eddm" TagName="FacebookTrackingPixel" %>
<%@ Register Src="~/CCustom/1/EDDMHubSpot.ascx" TagPrefix="eddm" TagName="EDDMHubSpot" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <eddm:VisualWebsiteOptimizer runat="server" id="VisualWebsiteOptimizer" />
    <eddm:BoldChatConversion runat="server" id="BoldChatConversion" />
    <eddm:EDDMHubSpot runat="server" id="EDDMHubSpot" />
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="phForm" runat="Server">

    <div class="container">

        <eddm:PageHeader runat="server" id="PageHeader" />

        <%--Facebook Tracking Pixel Script for EDDM--%>
        <eddm:FacebookTrackingPixel runat="server" id="FacebookTrackingPixel" />

        <div class="contentWrapper">
    
            <%--Order Steps--%>
            <eddm:OrderSteps runat="server" id="OrderSteps" />

            <div>&nbsp;</div>

            <div class="row">

                <%--Target Review--%>
                <div class="col-sm-4">

                    <div class="panel panel-primary" id="targetReviewBlock">
                        <div class="panel-heading">Target Review</div>
                        <div class="panel-body">

                            <asp:HyperLink ID="hypGoToMap" runat="server">
                                <asp:Literal ID="lMapReview" runat="server" />
                            </asp:HyperLink>

                            <h4><asp:Literal ID="lSelectName" runat="server" /></h4>
                            
                            <p><asp:Literal ID="lSelectDescription" runat="server" /></p>
                            
                            <%--EDDM Target Review--%>
                            <asp:Panel ID="pnlEDDMMapOptions" runat="server" Visible="false">

                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-block btn-shadow" OnClick="OpenMapForEdit">
                                    <span class="glyphicon glyphicon-plus"></span>&nbsp;Add More Addresses
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnNewMap" runat="server" CssClass="btn btn-primary btn-block btn-shadow">
                                    <span class="glyphicon glyphicon-screenshot"></span>&nbsp;Create a New Map
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnCombine" runat="server" CssClass="btn btn-primary btn-block btn-shadow">
                                    <span class="glyphicon glyphicon-resize-small"></span>&nbsp;Combine With Another Map
                                </asp:LinkButton>

                            </asp:Panel>

                            <%--Addressed Target Review--%>
                            <asp:Panel ID="pnlAddressedMapOptions" runat="server" Visible="false">


                                <%--New Generated List--%>
                                <asp:LinkButton ID="btnNewGenerated" runat="server" CssClass="btn btn-primary btn-block btn-shadow" Visible="False">
                                    <span class="fa fa-cog"></span>&nbsp;Generate a New List
                                </asp:LinkButton>

                                <%--New Generated List--%>
                                <asp:LinkButton ID="btnNewUploaded" runat="server" CssClass="btn btn-primary btn-block btn-shadow" Visible="False">
                                    <span class="fa fa-upload"></span>&nbsp;Upload a New List
                                </asp:LinkButton>



                            </asp:Panel>

                            <%--TMC Target Review--%>
                            <asp:Panel ID="pnlTMCMapOptions" runat="server" Visible="false">

                                <p class="text-info">[This text is visible for TMC Distribution Lists...]</p>

                            </asp:Panel>

                        </div>
                    </div>  

                </div>

                <%--Products--%>
                <div class="col-md-8">

                    <eddm:ProductListWithQuote runat="server" id="ProdList" />

                </div>

            </div>

            <%--Debugging--%>
            <asp:Panel ID="pnlDebug" runat="server" CssClass="alert alert-danger" Visible="False">
                <asp:Label ID="lblDebug" runat="server" Text="" CssClass="" />
                <div>&nbsp;</div>
            </asp:Panel>

        </div>

    </div>


    <%--Register Modal--%>
    <section id="RegisterSection">
        <div class="modal fade" id="registerModal" tabindex="-1" role="dialog" aria-labelledby="registerModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <div class="modal-header">
                        <h4 class="modal-title" id="registerModalLabel"><asp:Literal ID="litSignInRegisterModalTitle" runat="server" /></h4>
                    </div>

                    <div class="modal-body">

                        <div class="alert alert-info" role="alert">
                            <span class="fa fa-2x fa-info-circle"></span>&nbsp;<asp:Literal ID="litSaveToContinueMsg" runat="server" /> 
                        </div>

                        <asp:Literal ID="lSignInMsg" runat="server" />

                        <asp:Panel ID="pnlAccountTypes" runat="server" Visible="True">
                            <div class="row">
                                <div class="col-sm-4 col-sm-offset-2">
                                    <div class="radio">
                                        <label>
                                            <asp:RadioButton ID="radExisting" runat="server" GroupName="AccountStatus" Checked="true" ClientIDMode="Static"  /> <strong>I am a returning customer</strong>
                                        </label>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="radio">
                                        <label>
                                            <asp:RadioButton ID="radNew" runat="server" GroupName="AccountStatus" ClientIDMode="Static" /> <strong>I am a NEW customer</strong>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>


                        <%--New Customer Panel--%>
                        <div id="newCustomerBlock">
                            <asp:Panel ID="pnlNewCustomer" runat="server" ClientIDMode="Static">

                                <%--First / Last Name--%>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group-sm">
                                            <label for="txtFirstName" class="label label-primary">First Name</label>
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" MaxLength="50" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group-sm">
                                            <label for="txtLastName" class="label label-primary">Last Name</label>
                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" MaxLength="50" />
                                        </div>
                                    </div>
                                </div>
                        
                                <div>&nbsp;</div>

                                <%--Company--%>
                                <div class="form-group-sm">
                                    <label for="txtCompanyName" class="label label-primary">Company Name</label>
                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" MaxLength="50" />
                                </div>

                                <div>&nbsp;</div>

                                <%--Phone--%>
                                <div class="row">

                                    <div class="col-sm-8">
                                        <div class="form-group-sm">
                                            <label for="txtPhoneNumber" class="label label-primary">Phone Number</label>
                                            <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" MaxLength="50" ClientIDMode="Static" />
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group-sm">
                                            <label for="txtExt" class="label label-primary">Ext</label>
                                            <asp:TextBox ID="txtExt" runat="server" CssClass="form-control" MaxLength="5" />
                                        </div>
                                    </div>

                                </div>

                                <div>&nbsp;</div>

                            </asp:Panel>
                        </div>

                        <%--Email--%>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group-sm">
                                    <label for="EmailAddress" class="label label-primary">Email</label>
                                    <asp:TextBox ID="EmailAddress" runat="server" CssClass="form-control" MaxLength="100" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>

                        <div>&nbsp;</div>

                        <%--Passwords--%>
                        <div class="row">
                            <div class="col-sm-6">
                                <%--Password--%>
                                <div class="form-group-sm">
                                    <label for="AccountPass" class="label label-primary">Password</label>
                                    <asp:TextBox ID="AccountPass" runat="server" TextMode="Password" CssClass="form-control" MaxLength="20" ClientIDMode="Static" />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <%--Confirm Password--%>
                                <div id="newPasswordBlock">
                                    <asp:Panel ID="pConfirmAcctPass" runat="server" ClientIDMode="Static">
                                        <div class="form-group-sm">
                                            <label for="ConfirmPass" class="label label-primary">Confirm Password</label>
                                            <asp:TextBox ID="ConfirmPass" TextMode="Password" runat="server" CssClass="form-control" MaxLength="50" ClientIDMode="Static" />
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>


                        <div>&nbsp;</div>

                        <%--Business and Industry--%>
                        <div class="row" id="industryBlock">
                            <div class="col-sm-6">
                                <div class="form-group-sm">
                                    <label for="ddlBusinessType" class="label label-primary">Business Type</label>
                                    <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="form-control" ClientIDMode="Static">
                                        <asp:ListItem Value="" Selected="True">Select One</asp:ListItem>
                                        <asp:ListItem Value="Agency">Agency</asp:ListItem>
                                        <asp:ListItem Value="Franchisor">Franchisor</asp:ListItem>
                                        <asp:ListItem Value="Franchisee">Franchisee</asp:ListItem>
                                        <asp:ListItem Value="Small Business">Small Business</asp:ListItem>
                                        <asp:ListItem Value="Corporate">Corporate </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group-sm">
                                    <label for="ddlIndustry" class="label label-primary">Industry</label>
                                    <asp:DropDownList ID="ddlIndustry" runat="server" CssClass="form-control" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>


                        <%--Newsletter--%>
                        <div id="newletterBlock">
                            <asp:Panel ID="pEletterSignup" runat="server" CssClass="hidden" ClientIDMode="Static">
                                <div class="checkbox">
                                    <label>
                                        <asp:CheckBox ID="Eletter" runat="server" Checked="False" Text="Send me exclusive EDDM offers and tips. (We respect your privacy, and will NEVER disclose your information to 3rd parties.)" />
                                    </label>
                                </div>
                            </asp:Panel>
                        </div>

                    </div>

                    <div class="modal-footer">

                        <div class="row">
                            <div class="col-sm-6">
                                <button type="button" class="btn btn-primary btn-lg pull-left" data-dismiss="modal">
                                    <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;Cancel
                                </button>
                            </div>
                            <div class="col-sm-6">
                                <asp:LinkButton ID="lnkSignIn" runat="server" CssClass="btn btn-danger btn-lg pull-right" OnClientClick="return ValidateForm();" ClientIDMode="Static">
                                    <span class="fa fa-sign-in"></span>&nbsp;Sign In
                                </asp:LinkButton>
                            </div>
                        </div>

                        <div>&nbsp;</div>

                        <div class="row">
                            <div class="col-sm-12">
                                <asp:HyperLink ID="hypForgotPassword" runat="server" NavigateUrl="~/forgotpass.aspx" ToolTip="Forgot Password">Forgot Password?</asp:HyperLink>&nbsp;
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </section>


    <%--More Info Modal--%>
    <div class="modal fade" id="moreInfoModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <div class="modal-title"><span id="infoTitle"></span></div>
                </div>

                <div class="modal-body">
                    <div id="infoContent"></div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-primary pull-right" data-dismiss="modal">
                        <span class="fa fa-check"></span>&nbsp;OK
                    </button>
                </div>

            </div>
        </div>
    </div>


</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" runat="Server">

    <asp:Literal ID="litRegistrationScript" runat="server" />

    <script src="/assets/javascripts/PhoneMask.js" type="text/javascript"></script>

    <script src="/assets/javascripts/Step1-TargetReview.min.js?ver=1.1.0"></script>

    <script type="text/javascript">

        //Hide intitally. Used in 'Short' Registration Form.
        $('#newCustomerBlock').hide();
        $('#newPasswordBlock').hide();
        $('#newletterBlock').hide();
        $('#industryBlock').hide();


        $(document).ready(function ()
        {
            $("#txtPhoneNumber").inputmask("mask", { "mask": "(999) 999-9999" });
        });

    </script>

</asp:Content>

