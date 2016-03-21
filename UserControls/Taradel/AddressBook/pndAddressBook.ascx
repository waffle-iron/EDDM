<%@ Control Language="VB" AutoEventWireup="false" CodeFile="pndAddressBook.ascx.vb"
    Inherits="usercontrols_pndAddressBook" %>
<asp:Panel ID="pSelectForm" runat="server" Visible="false">
    <asp:Panel ID="pAddressBook" runat="server" CssClass="addressbook">
        <asp:ListView ID="lvContactIndex" runat="server">
            <LayoutTemplate>
                <div class="contact-index">
                    <ol>
                        <li id="itemPlaceholder" runat="server" />
                    </ol>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <li id="item" runat="server">
                    <asp:HyperLink ID="hplChar" runat="server" NavigateUrl='<%#String.Format("#{0}", Eval("Key")) %>'
                        Enabled='<%#apphelp.Iif(Eval("Value")>0, True, False) %>' Text='<%#Eval("Key") %>' /></li>
            </ItemTemplate>
        </asp:ListView>
        <asp:ListView ID="lvContacts" runat="server">
            <LayoutTemplate>
                <asp:Panel ID="pContactList" runat="server" CssClass="contact-list">
                    <ol>
                        <li id="itemPlaceholder" runat="server" />
                    </ol>
                </asp:Panel>
            </LayoutTemplate>
            <ItemTemplate>
                <li id="groupItem" runat="server" class='<%#apphelp.Iif(Eval("Value")="0", "hidden", "")%>'>
                    <h2 id='<%# Eval("Key") %>'>
                        <%# Eval("Key") %></h2>
                    <asp:ListView ID="lvItems" runat="server" OnItemDataBound="lvItems_ItemDataBound">
                        <LayoutTemplate>
                            <table cellpadding="0" cellspacing="0">
                                <tr id="itemPlaceholder" runat="server" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class="card">
                                <td class="contact" valign="top" width="25%">
                                    <h3>
                                        <a class="nolink">
                                            <%#Eval("Company")%></a></h3>
                                    <asp:HyperLink ID="hplCareOf" runat="server" NavigateUrl='<%#String.Format("mailto:{0}", Eval("EmailAddress")) %>'
                                        Text='<%#Eval("CareOf") %>' />
                                </td>
                                <td valign="top" width="40%" class="contactinfo">
                                    <div>
                                        <%#Eval("Address1")%></div>
                                    <asp:Panel ID="pAddress2" runat="server" Visible='<%#apphelp.Iif(String.IsNullOrEmpty(Eval("Address2").ToString), False, True) %>'>
                                        <asp:Literal ID="lAddress2" runat="server" Text='<%#Eval("Address2").ToString %>' /></asp:Panel>
                                    <div>
                                        <%#Eval("City") %>,
                                        <%#Eval("State")%></div>
                                    <div>
                                        <%#Eval("ZipCode")%></div>
                                </td>
                                <td valign="top" width="25%" class="contactinfo">
                                    <%#Eval("PhoneNumber")%>
                                </td>
                                <td valign="top" width="10%">
                                    <asp:LinkButton ID="lnkSelectContact" runat="server" OnClick="SelectContact" CommandArgument='<%#Eval("ShippingAddressID") %>'
                                        Text="Select" />
                                    <asp:Panel ID="pManageContact" runat="server">
                                        <asp:HyperLink ID="hplEdit" runat="server" NavigateUrl='<%#String.Format("~/account_addressbook_edit.aspx?id={0}", Eval("ShippingAddressID")) %>' CssClass="makeEditButtonIcon" Text="Edit" />
                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClick="DeleteShippingAddress" OnClientClick="var bRet=confirm('Are you sure you want to delete this address?'); if (!bRet) return false;"
                                                CommandArgument='<%#Eval("ShippingAddressId") %>' CssClass="makeDeleteButtonIcon" Text="Delete" />
                                        <div style="padding-bottom: 5px;">
                                            <%--<asp:HyperLink ID="hplEdit" runat="server" NavigateUrl='<%#String.Format("~/account_addressbook_edit.aspx?id={0}", Eval("ShippingAddressID")) %>'><img src="/resources/pencil.png" height="16" width="16" alt="Edit" />Edit</asp:HyperLink>--%>
                                            
                                        </div>
                                        <div>
                                            <%--<asp:LinkButton ID="lnkDelete" runat="server" OnClick="DeleteShippingAddress" OnClientClick="var bRet=confirm('Are you sure you want to delete this address?'); if (!bRet) return false;"
                                                CommandArgument='<%#Eval("ShippingAddressId") %>'><img src="/resources/delete.png" alt="Delete" height="16" width="16" />Delete</asp:LinkButton>--%>
                                                
                                            </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </li>
            </ItemTemplate>
            <EmptyDataTemplate>
                <p>
                    You do not have any contacts in your address book.</p>
            </EmptyDataTemplate>
        </asp:ListView>
        <div class="clear"></div>
    </asp:Panel>
</asp:Panel>
<asp:Panel ID="pAddForm" runat="server" Visible="false" DefaultButton="NewAddrCreate">
    <asp:ValidationSummary ID="vSummNewAddr" runat="server" ShowMessageBox="true" ShowSummary="false"
        EnableClientScript="true" ValidationGroup="vgNewAddr" />
    <div class="twocolrow">
        <div class="labeli">
            Company:</div>
        <div>
            <asp:TextBox ID="NewAddrCompany" runat="server" Columns="20" />
            <asp:RequiredFieldValidator ID="rfvNewAddrCompany" runat="server" ControlToValidate="NewAddrCompany"
                ErrorMessage="The Company name is required." Text=" (*)" ValidationGroup="vgNewAddr" /></div>
    </div>
    <div class="twocolrow">
        <div class="labeli">
            Attention:</div>
        <div>
            <asp:TextBox ID="NewAddrCareOf" runat="server" Columns="20" /></div>
    </div>
    <div class="twocolrow">
        <div class="labeli">
            Address 1:</div>
        <div>
            <asp:TextBox ID="NewAddrAddress1" runat="server" />
            <asp:RequiredFieldValidator ID="rfvNewAddrAddress1" runat="server" ControlToValidate="NewAddrAddress1"
                ErrorMessage="The street address is required." Text=" (*)" ValidationGroup="vgNewAddr" /></div>
    </div>
    <div class="twocolrow">
        <div class="labeli">
            Address 2:</div>
        <div>
            <asp:TextBox ID="NewAddrAddress2" runat="server" /></div>
    </div>
    <div class="twocolrow">
        <div class="labeli">
            City:</div>
        <div>
            <asp:TextBox ID="NewAddrCity" runat="server" />
            <asp:RequiredFieldValidator ID="rfvNewAddrCity" runat="server" ControlToValidate="NewAddrCity"
                ErrorMessage="The city is required." Text=" (*)" ValidationGroup="vgNewAddr" /></div>
    </div>
    <div class="twocolrow">
        <div class="labeli">
            State:</div>
        <div>
            <asp:TextBox ID="NewAddrState" runat="server" Columns="2" MaxLength="2" />
            <asp:RequiredFieldValidator ID="rfvNewAddrState" runat="server" ControlToValidate="NewAddrState"
                ErrorMessage="The state is required." Text=" (*)" ValidationGroup="vgNewAddr" /></div>
    </div>
    <div class="twocolrow">
        <div class="labeli">
            Zip Code:</div>
        <div>
            <asp:TextBox ID="NewAddrZip" runat="server" />
            <asp:RequiredFieldValidator ID="rfvNewAddrZip" runat="server" ControlToValidate="NewAddrZip"
                ErrorMessage="The zip code is required." Text=" (*)" ValidationGroup="vgNewAddr" /></div>
    </div>
    <div class="twocolrow">
        <div class="labeli">
            Country:</div>
        <div>
            <asp:DropDownList ID="NewAddrCountry" runat="server">
                <asp:ListItem Text="US" Value="US" Selected="True" />
                <asp:ListItem Text="CA" Value="CA" />
            </asp:DropDownList>
        </div>
    </div>
    <div class="twocolrow">
        <div class="labeli">
            Phone:</div>
        <div>
            <asp:TextBox ID="NewAddrPhone" runat="server" /></div>
    </div>
    <div class="clear">
        &nbsp;</div>
    <div class="row">
        <asp:CheckBox ID="NewAddrNonCommercial" runat="server" Text="This address is NOT a commercial address" />
    </div>
    <div class="clear">
        &nbsp;</div>
    <div class="row">
        <div class="label">
            &nbsp;</div>
        <div class="aright">
            <asp:Button ID="NewAddrCreate" runat="server" Text="Save and Select" ValidationGroup="vgNewAddr" CausesValidation="true" UseSubmitBehavior="false" OnClick="CreateNewAddress" /></div>
    </div>
    <div class="clear">
        &nbsp;</div>
</asp:Panel>
