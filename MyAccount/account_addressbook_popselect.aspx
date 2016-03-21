<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Popup.master" AutoEventWireup="false"
    CodeFile="account_addressbook_popselect.aspx.vb" Inherits="account_addressbook_popselect"
    Title="My Address Book" %>

<asp:Content ID="Content3" ContentPlaceHolderID="phBody" runat="Server">
    <h1>
        My Address Book</h1>
    <asp:ObjectDataSource ID="oShipping" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetData" TypeName="taradelCustomerTableAdapters.pnd_CustomerShippingAddressTableAdapter">
        <SelectParameters>
            <asp:Parameter Name="CustomerID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ListView ID="lvAddressBook" runat="server" DataSourceID="oShipping" GroupItemCount="2"
        GroupPlaceholderID="phGroupTemplate" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <table border="0" cellpadding="10" cellspacing="0" width="100%">
                <asp:PlaceHolder ID="phGroupTemplate" runat="server" />
            </table>
        </LayoutTemplate>
        <GroupTemplate>
            <tr>
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </tr>
        </GroupTemplate>
        <ItemTemplate>
            <td width="50%" valign="top">
                <div class="addressItem">
                    <div>
                        <asp:Label ID="lCompany" runat="server" Text='<%#Eval("Company") %>' Font-Bold="true" /></div>
                    <div>
                        Attn:
                        <asp:Label ID="lAttn" runat="Server" Text='<%#Eval("CareOf") %>' /></div>
                    <div>
                        <asp:Label ID="lAddress1" runat="Server" Text='<%#Eval("Address1") %>' /></div>
                    <asp:Panel ID="pAddress2" runat="server">
                        <asp:Label ID="lAddress2" runat="server" Text='<%#Eval("Address2") %>' /></asp:Panel>
                    <div>
                        <asp:Label ID="lCity" runat="server" Text='<%#Eval("City") %>' />,
                        <asp:Label ID="lState" runat="server" Text='<%#Eval("State") %>' />
                        <asp:Label ID="lZip" runat="server" Text='<%#Eval("ZipCode") %>' /></div>
                    <div>
                        <asp:Label ID="lCountry" runat="server" Text='<%#Eval("Country") %>' /></div>
                    <div>
                        <asp:Label ID="lPhone" runat="server" Text='<%#Eval("PhoneNumber") %>' /></div>
                    <div>
                        <asp:Label ID="lEmail" runat="Server" Text='<%#Eval("EmailAddress") %>' /></div>
                    <div class="btnalign">
                        <asp:Button ID="btnSelect" runat="server" Text="Select Address" OnClientClick='<%#String.Format("selectByValue(opener.addressFld, {0}); window.close();", Eval("ShippingAddressID")) %>' /></div>
                </div>
            </td>
        </ItemTemplate>
        <EmptyDataTemplate>
            <p>
                You have not created any shipping addresses.</p>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
