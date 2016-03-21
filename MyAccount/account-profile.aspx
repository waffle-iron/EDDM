<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="account-profile.aspx.vb" Inherits="account_profile" %>
<%@ Register Src="~/Controls/USAStatesDropDown.ascx" TagPrefix="appx" TagName="USAStatesDropDown" %>


<asp:Content ID="Content1" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="phForm" runat="Server">

<section id="PageContent">
    <div class="container">

        <div class="partialRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            <span class="subRibbonPop">My Account</span>
            <span class="subRibbon">Complete Your Customer Account Information</span>
        </div>

        <div class="partialRibbonSmall visible-sm visible-xs">
            <span class="subRibbonPopSmall">My Account</span>
            <span class="subRibbonSmall">Complete Your Customer Account Information</span>
        </div>

        <div class="contentWrapper">

            <div class="row">

                <div class="col-sm-3 hidden-xs">

                    <div class="info-board info-board-theme-primary">
                        <h4>Required Fields</h4>
                        <p>Required fields are shown with a label which <span class="label formLabelRequired">looks like this.</span></p>
                        <p>Optional fields are shown with a label which <span class="label formLabel">looks like this.</span></p>
                    </div>

                </div>

                <div class="col-sm-9 col-xs-12">

                    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false" ClientIDMode="Static">
                        <span class="glyphicon glyphicon-remove pull-left"></span>&nbsp;<asp:Label ID="lblError" runat="server" />
                    </asp:Panel>

                    <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success" Visible="false" ClientIDMode="Static">
                        <span class="glyphicon glyphicon-ok pull-left"></span>&nbsp;<asp:Label ID="lblSuccess" runat="server" />
                    </asp:Panel>

                    <asp:Panel ID="pnlGreeting" runat="server" Visible="True">
                        <div class="alert alert-info" role="alert">
                            <span class="fa fa-info-circle text-info"></span>&nbsp;We need a little more information.  Please use the form below to make complete your account.
                        </div>
                    </asp:Panel>

                    <asp:ValidationSummary ID="vsAccount" runat="server" CssClass="alert alert-danger" HeaderText="Please check for the following errors:" />

                    <div role="form">

                        <%--Company--%>
                        <div class="form-group">
                            <label for="Company" class="label formLabelRequired">Company</label>
                            <div class="row">
                                <div class="col-sm-11">
                                    <asp:TextBox ID="Company" runat="server" CssClass="form-control" Text='<%#Eval("Company") %>' MaxLength="100" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ID="rfvCompany" ControlToValidate="Company" runat="server" ErrorMessage="Company Name is a required field." Display="dynamic" CssClass="text-danger" SetFocusOnError="True">
                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <%--First Name--%>
                        <div class="form-group">
                            <label for="FirstName" class="label formLabelRequired">First Name</label>
                            <div class="row">
                                <div class="col-sm-11">
                                    <asp:TextBox ID="FirstName" runat="server" CssClass="form-control" Text='<%#Eval("FirstName") %>' MaxLength="50" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ID="rfvFirstName" ControlToValidate="FirstName" runat="server" ErrorMessage="First Name is required." Display="dynamic" CssClass="text-danger" SetFocusOnError="True">
                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>
                        </div>

                        <%--Middle Int--%>
                        <div class="form-group">
                            <label for="MI" class="label formLabel">Middle Initial</label>
                            <div class="row">
                                <div class="col-sm-4">
                                    <asp:TextBox ID="MI" runat="server" CssClass="form-control" Text='<%#Eval("MiddleInitial") %>' MaxLength="1" />
                                </div>
                            </div>
                        </div>

                        <%--Last Name--%>
                        <div class="form-group">
                            <label for="LastName" class="label formLabelRequired">Last Name</label>
                            <div class="row">
                                <div class="col-sm-11">
                                    <asp:TextBox ID="LastName" runat="server" CssClass="form-control" Text='<%#Eval("LastName") %>' MaxLength="50" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ID="rfvLastName" ControlToValidate="LastName" runat="server" ErrorMessage="Last Name is required." Display="dynamic" CssClass="text-danger" SetFocusOnError="True">
                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvLastName" runat="server" CssClass="text-center" ErrorMessage="Last name cannot be the same as first name." ControlToCompare="FirstName" ControlToValidate="LastName" Display="Dynamic" Operator="NotEqual" SetFocusOnError="True">
                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                    </asp:CompareValidator>
                                </div>
                            </div>
                        </div>

                        <%--Address 1--%>
                        <div class="form-group">
                            <label for="Address1" class="label formLabelRequired">Address 1</label>
                            <div class="row">
                                <div class="col-sm-11">
                                    <asp:TextBox ID="Address1" runat="server" CssClass="form-control" Text='<%#Eval("Address1") %>' MaxLength="100" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ID="rfvAddress1" ControlToValidate="Address1" runat="server" ErrorMessage="Address 1 is required." Display="dynamic" CssClass="text-danger" SetFocusOnError="True">
                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <%--Address2--%>
                        <div class="form-group">
                            <label for="Address2" class="label formLabelRequired">Address 2</label>
                            <div class="row">
                                <div class="col-sm-11">
                                    <asp:TextBox ID="Address2" runat="server" CssClass="form-control" Text='<%#Eval("Address2") %>' MaxLength="50" />
                                </div>
                            </div>
                        </div>

                        <%--City--%>
                        <div class="form-group">
                            <label for="City" class="label formLabelRequired">City</label>
                            <div class="row">
                                <div class="col-sm-11">
                                    <asp:TextBox ID="City" runat="server" CssClass="form-control" Text='<%#Eval("City") %>' MaxLength="50" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ID="rfvCity" ControlToValidate="City" runat="server" ErrorMessage="City is required." Display="dynamic" CssClass="text-danger" SetFocusOnError="True">
                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <%--State & Postal Code--%>
                        <div class="row">

                            <div class="col-sm-6">

                                <div class="form-group">
                                    <label class="label formLabelRequired" for="State">State</label>
                                    <div class="row">
                                        <div class="col-sm-9">
                                            <appx:USAStatesDropDown runat="server" id="USAStatesDropDown" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="USAStatesDropDown:ddlStates" ErrorMessage="Please choose a State." CssClass="text-danger" Display="Dynamic" SetFocusOnError="True">
                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="col-sm-6">

                                <div class="form-group">
                                    <label for="PostalCode" class="label formLabelRequired">Zip Code</label>
                                    <div class="row">
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="PostalCode" runat="server" CssClass="form-control col-sm-11" Text='<%#Eval("ZipCode") %>' MaxLength="10" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:RequiredFieldValidator ID="rfvPostalCode" ControlToValidate="PostalCode" runat="server" ErrorMessage="Zip Code is required." Text="" CssClass="text-danger" Display="Dynamic">
                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revPostalCode" ControlToValidate="PostalCode" runat="server" CssClass="text-danger" ErrorMessage="Please provide a valid zip code (5-10 characters)." ValidationExpression="[A-Za-z0-9\-]{5,10}" Display="Dynamic">
                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <%--Direct Line & Ext--%>
                        <div class="row">

                            <div class="col-sm-6">

                                <div class="form-group">
                                    <label class="label formLabelRequired" for="DirectLine">Direct Line</label>
                                    <div class="row">
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="DirectLine" runat="server" CssClass="form-control" Text='<%#Eval("PhoneNumber") %>' MaxLength="15" ClientIDMode="Static" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:RequiredFieldValidator ID="rfvDirectLine" runat="server" ControlToValidate="DirectLine" ErrorMessage="A phone number is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="True">
                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="label formLabel" for="DirectLineExt">Ext</label>
                                    <div class="row">
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="DirectLineExt" runat="server" MaxLength="10" CssClass="form-control" />
                                        </div>
                                        <div class="col-sm-3">
                                            &nbsp;
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <%--Company Phone & Ext--%>
                        <div class="row">

                            <div class="col-sm-6">

                                <div class="form-group">
                                    <label class="label formLabel" for="CompanyLine">Company Main Line</label>
                                    <div class="row">
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="CompanyLine" runat="server" CssClass="form-control" Text='<%#Eval("CompanyPhoneNumbere") %>' MaxLength="20" />
                                        </div>
                                        <div class="col-sm-3">&nbsp;</div>
                                    </div>
                                </div>

                            </div>

                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="label formLabel" for="CompanyLineExt">Ext</label>
                                    <div class="row">
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="CompanyLineExt" runat="server" MaxLength="10" CssClass="form-control" Text='<%#Eval("CompanyExtension") %>' />
                                        </div>
                                        <div class="col-sm-3">
                                            &nbsp;
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <%--Industry--%>
                        <div class="row">

                            <div class="col-sm-11">

                                <div class="form-group">
                                    <label class="label formLabelRequired" for="Industry">Industry</label>
                            
                                    <asp:ObjectDataSource ID="oBizClass" runat="server" OldValuesParameterFormatString="{0}" SelectMethod="GetList" TypeName="Taradel.BusinessClassDataSource" />
                                    
                                    <asp:DropDownList ID="Industry" runat="server" DataSourceID="oBizClass" AppendDataBoundItems="true" DataValueField="BusinessClassID" DataTextField="Name" CssClass="form-control">
                                        <asp:ListItem Text="Select One" Value="" />
                                    </asp:DropDownList>
                            
                                 </div>

                            </div>

                            <div class="col-sm-1">
                                <asp:RequiredFieldValidator ID="rfvIndustry" runat="server" ControlToValidate="Industry" ErrorMessage="Please choose an industry that best describes your business." CssClass="text-danger" Display="Dynamic" SetFocusOnError="True">
                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                </asp:RequiredFieldValidator>
                            </div>

                        </div>

                        <div>&nbsp;</div>

                        <%--buttons--%>
                        <div class="row">

                            <div class="col-sm-12 text-center">
                                <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-danger btn-lg" Visible="True">
                                    <span class="fa fa-edit"></span>&nbsp;Update My Account
                                </asp:LinkButton>
                            </div>

                            <div class="col-sm-12 text-center">
                                <asp:LinkButton ID="btnContinue" runat="server" CssClass="btn btn-success btn-lg" Visible="False">
                                    <span class="fa fa-arrow-right"></span>&nbsp;Continue to Order
                                </asp:LinkButton>
                            </div>

                            <asp:Panel ID="pnlDebug" runat="server" Visible="False">
                            </asp:Panel>

                        </div>

                        <div>&nbsp;</div>

                </div>

                </div>

            </div>

            <div>&nbsp;</div>

        </div>

    </div>
</section>





</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" runat="Server">

    <script src="/assets/javascripts/PhoneMask.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#DirectLine").inputmask("mask", { "mask": "(999) 999-9999" }); 
        });
    </script>


</asp:Content>


