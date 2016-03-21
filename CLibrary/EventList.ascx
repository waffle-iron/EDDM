<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EventList.ascx.vb" Inherits="CLibrary_EventList" %>
<asp:Panel ID="pEventList" runat="server">
    <asp:ListView ID="lvEvents" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <asp:PlaceHolder ID="phItemTemplate" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <div class="eventWrapper ui-helper-clearfix">
                <div class="eventDate">
                    <div class="eventDayName">
                        <asp:Literal ID="lDayName" runat="server" Text='<%#DateTime.Parse(Eval("EventStartDate")).ToString("ddd")%>' /></div>
                    <div class="eventDay">
                        <asp:Literal ID="lDay" runat="server" Text='<%#DateTime.Parse(Eval("EventStartDate")).ToString("dd") %>' /></div>
                    <div class="eventMonth">
                        <asp:Literal ID="lMonth" runat="server" Text='<%#DateTime.Parse(Eval("EventStartDate")).ToString("MMM") %>' /></div>
                </div>
                <div class="eventContainer ui-helper-clearfix">
                    <div class="eventTitle">
                        <asp:Literal ID="lEventTitle" runat="server" Text='<%#Eval("EventTitle") %>' /></div>
                    <div class="eventTime">
                        <asp:Literal ID="lEventTime" runat="server" /></div>
                    <div class="eventDescription">
                        <asp:Literal ID="lEventDescription" runat="server" Text='<%#Eval("EventDesc") %>' /></div>
                    <div><asp:HyperLink ID="hplSaveCalendar" runat="server" NavigateUrl='<%#String.Format("~/resources/vEvent.ashx?id={0}", Eval("EventId")) %>'>Save to your Calendar</asp:HyperLink></div>
                </div>
            </div>
        </ItemTemplate>
        <EmptyDataTemplate>
            <p><asp:Literal ID="lNoUpcoming" runat="server" /></p>
        </EmptyDataTemplate>
    </asp:ListView>

    <div id="EventInfo" style="display:none;">
        <asp:PlaceHolder ID="phEventInfo" runat="server" />    
    </div>
</asp:Panel>
