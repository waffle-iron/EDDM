<%@ Page Language="VB" MasterPageFile="~/App_Masterpages/Site.master" AutoEventWireup="false"
    CodeFile="vpage_event.aspx.vb" Inherits="vpage_event" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" runat="Server">
    <div>
        <h1><asp:Literal ID="lEventTitle" runat="server" /></h1>
    </div>
    <div>
        <h2>
            <asp:Literal ID="lCalendarDate" runat="server" /></h2>
    </div>
    <br />
    <div>
        <asp:Literal ID="lEventDesc" runat="server" />
    </div>
</asp:Content>
