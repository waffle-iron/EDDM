<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Member.master" AutoEventWireup="false"
    CodeFile="account_addressbook_edit.aspx.vb" Inherits="account_addressbook_edit"
    Title="Edit Shipping Address" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phBody" runat="Server">
    <h1>Edit Shipping Address</h1>
    <asp:ObjectDataSource ID="oAddress" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetCustomerShippingAddress" TypeName="taradelCustomerTableAdapters.pnd_CustomerShippingAddressTableAdapter"
        UpdateMethod="Update">
        <UpdateParameters>
            <asp:Parameter Name="CustomerID" Type="Int32" />
            <asp:Parameter Name="Company" Type="String" />
            <asp:Parameter Name="CareOf" Type="String" />
            <asp:Parameter Name="Address1" Type="String" />
            <asp:Parameter Name="Address2" Type="String" />
            <asp:Parameter Name="City" Type="String" />
            <asp:Parameter Name="State" Type="String" />
            <asp:Parameter Name="ZipCode" Type="String" />
            <asp:Parameter Name="Country" Type="String" />
            <asp:Parameter Name="PhoneNumber" Type="String" />
            <asp:Parameter Name="EmailAddress" Type="String" />
            <asp:Parameter Name="DefaultAddress" Type="Boolean" DefaultValue="False" />
            <asp:Parameter Name="Deleted" Type="Boolean" DefaultValue="False" />
            <asp:Parameter Name="Original_ShippingAddressID" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="ShippingAddressID" QueryStringField="ID" Type="Int32" />
            <asp:Parameter Name="CustomerID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ValidationSummary ID="vSumm" runat="server" HeaderText="Please correct the following:" />
    <asp:FormView ID="fvAddress" runat="server" DataKeyNames="ShippingAddressID" DataSourceID="oAddress"
        DefaultMode="Edit" Width="100%">
        <EditItemTemplate>
            <div class="row">
                <div class="label">
                    Company:</div>
                <div class="aright">
                    <asp:TextBox ID="Company" runat="server" Text='<%#Bind("Company") %>' /><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="The company name is required."
                        Text="*" ControlToValidate="Company" /></div>
            </div>
            <div class="row">
                <div class="label">
                    Attn:</div>
                <div class="aright">
                    <asp:TextBox ID="CareOf" runat="server" Text='<%#Bind("CareOf") %>' /><asp:RequiredFieldValidator
                        ID="rfvCareOf" runat="server" ErrorMessage="The recipient name is required."
                        Text="*" ControlToValidate="CareOf" /></div>
            </div>
            <div class="row">
                <div class="label">
                    Address 1:</div>
                <div class="aright">
                    <asp:TextBox ID="Address1" runat="server" Text='<%#Bind("Address1") %>' /><asp:RequiredFieldValidator
                        ID="rfvAddress1" runat="server" ErrorMessage="The street address is required."
                        Text="*" ControlToValidate="Address1" /></div>
            </div>
            <div class="row">
                <div class="label">
                    Address 2:</div>
                <div class="aright">
                    <asp:TextBox ID="Address2" runat="server" Text='<%#Bind("Address2") %>' /></div>
            </div>
            <div class="row">
                <div class="label">
                    City:</div>
                <div class="aright">
                    <asp:TextBox ID="City" runat="server" Text='<%#Bind("City") %>' /><asp:RequiredFieldValidator
                        ID="rfvCity" runat="server" ErrorMessage="The city is required." Text="*" ControlToValidate="City" /></div>
            </div>
            <div class="row">
                <div class="label">
                    State:</div>
                <div class="aright">
                    <geoselect:State ID="State" runat="server" ValidationErrorMessage="Please select a state"
                        AllowInternational="true" Text="!" ValidationSide="Right" SelectedValue='<%#Bind("State") %>' />
                    <%--<asp:TextBox ID="State" runat="server" Text='<%#Bind("State") %>' />
                <asp:RequiredFieldValidator ID="rfvState" ControlToValidate="State" runat="server" ErrorMessage="Your state is a required field." Text="(*) "/><asp:RegularExpressionValidator
                        ID="revState" runat="server" ControlToValidate="State" ErrorMessage="Please provide the 2-letter state abbreviation." Text="!" ValidationExpression="[A-Za-z]{2}"></asp:RegularExpressionValidator>
                --%>
                </div>
            </div>
            <div class="row">
                <div class="label">
                    Zip Code:</div>
                <div class="aright">
                    <asp:TextBox ID="ZipCode" runat="server" Text='<%#Bind("ZipCode") %>' /><asp:RequiredFieldValidator
                        ID="rfvPostalCode" ControlToValidate="ZipCode" runat="server" ErrorMessage="The zip code is a required field."
                        Text="(*) " /></div>
            </div>
            <div class="row">
                <div class="label">
                    Country:</div>
                <div class="aright">
                    <geoselect:Country ID="Country" runat="server" SelectedValue='<%#Bind("Country") %>' />
                    <%--<asp:TextBox ID="Country" runat="server" Text='<%#Bind("Country") %>' />--%></div>
            </div>
            <div class="row">
                <div class="label">
                    Phone Number:</div>
                <div class="aright">
                    <asp:TextBox ID="PhoneNumber" runat="server" Text='<%#Bind("PhoneNumber") %>' /><asp:RequiredFieldValidator
                        ID="rfvPhone" runat="server" ErrorMessage="The phone number is required." Text="*"
                        ControlToValidate="PhoneNumber" /></div>
            </div>
            <div class="row">
                <div class="label">
                    E-mail Address:</div>
                <div class="aright">
                    <asp:TextBox ID="EmailAddress" runat="server" Text='<%#Bind("EmailAddress") %>' /><asp:RequiredFieldValidator
                        ID="rfvEmail" runat="server" ErrorMessage="The e-mail address is required." Text="*"
                        ControlToValidate="EmailAddress" /></div>
            </div>
            <div class="clear">
                &nbsp;</div>
            <div class="btnalign">
                <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                    Text="Save Changes"></asp:Button>
                <asp:Button ID="UpdateCancelButton" runat="server" CausesValidation="False" Text="Cancel"
                    OnClick="GoToAddressBook"></asp:Button>
            </div>
        </EditItemTemplate>
    </asp:FormView>
</asp:Content>
