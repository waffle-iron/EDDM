<%@ Page Language="VB" MasterPageFile="~/App_Masterpages/Site.master" AutoEventWireup="false" CodeFile="vpage_displayevent.aspx.vb" Inherits="vpage_displayevent" title="Untitled Page" %>
<%@ Register Src="~/CLibrary/EventCalendar.ascx" TagName="EventCalendar" TagPrefix="uc" %>

<asp:Content ID="ContentBC" ContentPlaceHolderID="phFakeBreadCrumb" runat="server">
    <div class="breadcrumbs">
        <div class="breadcrumbs">
            <a href="/UpcomingEvents">Events</a></div>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="phLeftSideBar" Runat="Server">
    <div id="sidebarEC">
        <uc:EventCalendar ID="ecPanel" runat="server" LookAheadDays="30" />    
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phBody" Runat="Server">
   <asp:DataList ID="dlEvent" runat="server">
        <ItemTemplate>
            <div>
                <h1><%#Eval("EventTitle")%></h1>
            </div>
            <div>
                <h2>
                <asp:Literal ID="lCalendarDate" runat="server" /></h2></div>
                <br />
            <div>
                <asp:Literal ID="lEvent" runat="server" Text='<%#Eval("EventDesc")%>'></asp:Literal> 
            </div>
        </ItemTemplate>
    </asp:DataList>
</asp:Content>