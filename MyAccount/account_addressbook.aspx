<%@ Page Title="My Address Book" Language="VB" MasterPageFile="~/app_masterpages/Member.master"
    AutoEventWireup="false" CodeFile="account_addressbook.aspx.vb" Inherits="account_addressbook"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phBody" runat="Server">
    <h1>My Address Book</h1>
    <Taradel:AddressBook ID="AddressBook" runat="server" DisplayMode="ManagementList" />
</asp:Content>
