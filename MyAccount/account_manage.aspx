<%@ Page Title="My Account" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="account_manage.aspx.vb" Inherits="account_manage" %>
<%@ Register Src="~/Controls/YourAccountUserMenu.ascx" TagPrefix="appx" TagName="YourAccountUserMenu" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="appx" TagName="PageHeader" %>



<asp:Content ID="Content1" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phForm" runat="Server">

    <section id="PageContent">
        <div class="container">

            <appx:PageHeader runat="server" id="PageHeader" />

            <div class="contentWrapper">

                <asp:ObjectDataSource ID="oAccount" runat="server" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="{0}" SelectMethod="GetCustomer" TypeName="Taradel.CustomerDataSource" UpdateMethod="Update">
                    <DeleteParameters>
                        <asp:Parameter Name="CustomerID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="MultiAdCID" Type="String" />
                        <asp:Parameter Name="MAID" Type="String" />
                        <asp:Parameter Name="Username" Type="String" />
                        <asp:Parameter Name="Password" Type="String" />
                        <asp:Parameter Name="BusinessClass" Type="Int32" />
                        <asp:Parameter Name="Company" Type="String" />
                        <asp:Parameter Name="FirstName" Type="String" />
                        <asp:Parameter Name="MiddleInitial" Type="String" />
                        <asp:Parameter Name="LastName" Type="String" />
                        <asp:Parameter Name="EmailAddress" Type="String" />
                        <asp:Parameter Name="PhoneNumber" Type="String" />
                        <asp:Parameter Name="Extension" Type="String" />
                        <asp:Parameter Name="CompanyPhoneNumbere" Type="String" />
                        <asp:Parameter Name="CompanyExtension" Type="String" />
                        <asp:Parameter Name="MobilePhoneNumber" Type="String" />
                        <asp:Parameter Name="FaxNumber" Type="String" />
                        <asp:Parameter Name="Address1" Type="String" />
                        <asp:Parameter Name="Address2" Type="String" />
                        <asp:Parameter Name="City" Type="String" />
                        <asp:Parameter Name="State" Type="String" />
                        <asp:Parameter Name="ZipCode" Type="String" />
                        <asp:Parameter Name="NewsletterSignup" Type="Boolean" />
                        <asp:Parameter Name="Original_CustomerID" Type="Int32" />
                        <asp:Parameter Name="CustomerID" Type="Int32" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:Parameter Name="CustomerID" Type="Int32" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="MultiAdCID" Type="String" />
                        <asp:Parameter Name="MAID" Type="String" />
                        <asp:Parameter Name="Username" Type="String" />
                        <asp:Parameter Name="Password" Type="String" />
                        <asp:Parameter Name="BusinessClass" Type="Int32" />
                        <asp:Parameter Name="Company" Type="String" />
                        <asp:Parameter Name="FirstName" Type="String" />
                        <asp:Parameter Name="MiddleInitial" Type="String" />
                        <asp:Parameter Name="LastName" Type="String" />
                        <asp:Parameter Name="EmailAddress" Type="String" />
                        <asp:Parameter Name="PhoneNumber" Type="String" />
                        <asp:Parameter Name="Extension" Type="String" />
                        <asp:Parameter Name="CompanyPhoneNumbere" Type="String" />
                        <asp:Parameter Name="CompanyExtension" Type="String" />
                        <asp:Parameter Name="MobilePhoneNumber" Type="String" />
                        <asp:Parameter Name="FaxNumber" Type="String" />
                        <asp:Parameter Name="Address1" Type="String" />
                        <asp:Parameter Name="Address2" Type="String" />
                        <asp:Parameter Name="City" Type="String" />
                        <asp:Parameter Name="State" Type="String" />
                        <asp:Parameter Name="ZipCode" Type="String" />
                        <asp:Parameter Name="NewsletterSignup" Type="Boolean" />
                    </InsertParameters>
                </asp:ObjectDataSource>

                <div class="row">

                    <%--Side bar--%>
                    <div class="col-sm-3 hidden-xs">
                                                
                        <appx:YourAccountUserMenu runat="server" id="YourAccountUserMenu" />

                        <div class="info-board info-board-theme-primary">
                            <h4>Required Fields</h4>
                            <p>Required fields are shown with a label which <span class="label formLabelRequired">looks like this.</span></p>
                            <p>Optional fields are shown with a label which <span class="label formLabel">looks like this.</span></p>
                        </div>

                        <div>&nbsp;</div>

                        <div class="alert alert-info" role="alert">
                            <span class="fa fa-info-circle"></span>&nbsp;
                            <small><strong>TIP:</strong> Your e-mail address is also used as your account login. If you change your e-mail address, 
                            your account login will change to the new e-mail address.</small>

                            <br />
                            <br />
                            <span class="fa fa-info-circle"></span>&nbsp;
                            <small><strong>TIP:</strong> You will need to enter your password to save your changes.</small>
                        </div>

                    </div>

                    <%--Main form--%>
                    <div class="col-sm-9 col-xs-12">

                        <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false" ClientIDMode="Static">
                            <span class="glyphicon glyphicon-remove pull-left"></span>&nbsp;<asp:Label ID="lblError" runat="server" />
                        </asp:Panel>

                        <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success" Visible="false" ClientIDMode="Static">
                            <span class="glyphicon glyphicon-ok pull-left"></span>&nbsp;<asp:Label ID="lblSuccess" runat="server" />
                        </asp:Panel>

                        <p>Please use the form below to make changes to your account.</p>

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
                                                <geoselect:State ID="State" runat="server" AllowInternational="false" ShowAbbrOnly="false" RequireField="true" ValidationSide="Right" SelectedValue='<%#Eval("State") %>' />
                                            </div>
                                            <div class="col-sm-3">
                                                &nbsp;
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-sm-6">

                                    <div class="form-group">
                                        <label for="txtPostalCode" class="label formLabelRequired">Postal Code</label>
                                        <div class="row">
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="PostalCode" runat="server" CssClass="form-control col-sm-11" Text='<%#Eval("ZipCode") %>' MaxLength="10" />
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:RequiredFieldValidator ID="rfvPostalCode" ControlToValidate="PostalCode" runat="server" ErrorMessage="Postal Code is required." Text="" CssClass="text-danger" Display="Dynamic">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revPostalCode" ControlToValidate="PostalCode" runat="server" CssClass="text-danger" ErrorMessage="Please provide a valid postal code (5-10 characters)." ValidationExpression="[A-Za-z0-9\-]{5,10}" Display="Dynamic">
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
                                                <asp:TextBox ID="DirectLine" ClientIDMode="Static" runat="server" CssClass="form-control" Text='<%#Eval("PhoneNumber") %>' MaxLength="15" />
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

                            <%--Fax--%>
                            <div class="row">

                                <div class="col-sm-6">

                                    <div class="form-group">
                                        <label class="label formLabel" for="Fax">Fax</label>
                                        <div class="row">
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="Fax" runat="server" CssClass="form-control" Text='<%#Eval("FaxNumber") %>' MaxLength="20" />
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:RegularExpressionValidator ID="revFax" runat="server" ValidationExpression="^\d{10}$" ControlToValidate="Fax" Display="Dynamic" ErrorMessage="Please only provide 10 numeric characters for Fax." CssClass="text-danger" SetFocusOnError="True">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-sm-6">
                                    &nbsp;
                                </div>

                            </div>

                            <%--Mobile--%>
                            <div class="row">

                                <div class="col-sm-6">

                                    <div class="form-group">
                                        <label class="label formLabel" for="Fax">Mobile Phone</label>
                                        <div class="row">
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="MobilePhone" runat="server" CssClass="form-control" Text='<%#Eval("MobilePhoneNumber") %>' MaxLength="20" />
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:RegularExpressionValidator ID="revMobilePhone" runat="server" ValidationExpression="^\d{10}$" ControlToValidate="MobilePhone" Display="Dynamic" ErrorMessage="Please only provide 10 numeric characters for Mobile Phone." CssClass="text-danger" SetFocusOnError="True">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-sm-6">
                                    &nbsp;
                                </div>

                            </div>

                            <%--Email--%>
                            <div class="form-group">
                                <label class="label formLabelRequired" for="Fax">Email</label>
                                <div class="row">
                                    <div class="col-sm-11">
                                        <asp:TextBox ID="EmailAddress" runat="server" CssClass="form-control" Text='<%#Eval("EmailAddress") %>' MaxLength="100" />
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:RequiredFieldValidator ID="rfvEmailAddress" Display="dynamic" Text="*" ControlToValidate="EmailAddress" runat="server" ErrorMessage="Email address is required." EnableClientScript="true" CssClass="text-danger" SetFocusOnError="True">
                                            <span class="fa fa-2x fa-exclamation-circle"></span>
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revEmailAddress" runat="server" ErrorMessage="A valid e-email address is required." ControlToValidate="EmailAddress" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger">
                                            <span class="fa fa-2x fa-exclamation-circle"></span>
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>

                            <%--Password--%>
                            <div class="form-group">
                                <label for="CurrentPassword" class="label formLabelRequired">Password</label>
                                <div class="row">
                                    <div class="col-sm-11">
                                        <asp:TextBox ID="CurrentPassword" runat="server" MaxLength="20" CssClass="form-control" Text="*" />
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:RequiredFieldValidator ID="rfvNewPassword" ControlToValidate="CurrentPassword" runat="server" ErrorMessage="You must supply your current password to make changes to your account." Text="" CssClass="text-danger" Display="Dynamic" SetFocusOnError="True">
                                            <span class="fa fa-2x fa-exclamation-circle"></span>
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <%--New Password & Confirm--%>
                            <div class="row">

                                <div class="col-sm-6">

                                    <div class="form-group">
                                        <label for="Password" class="label formLabel">New Password</label>
                                        <div class="row">
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="Password" runat="server" MaxLength="20" CssClass="form-control" TextMode="Password" />
                                            </div>
                                            <div class="col-sm-3">
                                                &nbsp;
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="ConfirmPassword" class="label formLabel">Confirm New Password</label>
                                        <div class="row">
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="ConfirmPassword" runat="server" MaxLength="20" CssClass="form-control" TextMode="Password" />
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:CompareValidator ID="cvPasswordConfirm" runat="server" ControlToValidate="ConfirmPassword" ControlToCompare="Password" ErrorMessage="Your passwords do not match." CssClass="text-danger" Display="Dynamic" SetFocusOnError="True">
                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                </asp:CompareValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <%--Newsletter--%>
                            <asp:Panel ID="pnlNewsletter" runat="server" Visible="True">
                                <div class="well well-sm">
                                    <div class="checkbox">
                                        <label>
                                            <asp:CheckBox ID="Eletter" runat="server" Checked="False" Text="Yes, I want to receive the Taradel Growth Solutions E-letter." />
                                        </label>
                                    </div>
                                </div>
                            </asp:Panel>


                            <div>&nbsp;</div>

                            <%--button--%>
                            <div class="row">
                                <div class="col-sm-12 text-center">
                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-danger btn-lg" OnClick="SaveChanges">
                                        <span class="fa fa-edit"></span>&nbsp;Update My Account
                                    </asp:LinkButton>

                                </div>
                            </div>


                        </div>

                        <div>&nbsp;</div>

                    </div>

                </div>

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

