<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Calendar.ascx.vb" Inherits="CLibrary_Calendar" %>
<asp:Panel ID="pCalendar" runat="server" CssClass="appx">
    <asp:ListView ID="lvCalendar" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <table cellpadding="5" cellspacing="0" width="100%" class="calendar" id="eventCal">
                <tr>
                    <td colspan="2" class="prevNav">
                        <asp:HyperLink ID="hplPrevMonth" runat="server" />
                    </td>
                    <td colspan="3" class="monthName">
                        <asp:Panel ID="pTitle" runat="server">
                            <asp:Literal ID="lTitle" runat="server" /></asp:Panel>
                        <div>
                            <asp:Literal ID="lMonthName" runat="server" /></div>
                    </td>
                    <td colspan="2" class="nextNav">
                        <asp:HyperLink ID="hplNextMonth" runat="server" />
                    </td>
                </tr>
                <tr class="weekdayName">
                    <th class="weekend">
                        <asp:Literal ID="lSunday" runat="server" Text="Sunday" />
                    </th>
                    <th class="weekday">
                        <asp:Literal ID="lMonday" runat="server" Text="Monday" />
                    </th>
                    <th class="weekday">
                        <asp:Literal ID="lTuesday" runat="server" Text="Tuesday" />
                    </th>
                    <th class="weekday">
                        <asp:Literal ID="lWednesday" runat="server" Text="Wednesday" />
                    </th>
                    <th class="weekday">
                        <asp:Literal ID="lThursday" runat="server" Text="Thursday" />
                    </th>
                    <th class="weekday">
                        <asp:Literal ID="lFriday" runat="server" Text="Friday" />
                    </th>
                    <th class="weekend">
                        <asp:Literal ID="lSaturday" runat="server" Text="Saturday" />
                    </th>
                </tr>
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr id="weekRow" runat="server" class="week">
                <td id="SundayCell" runat="server" rel='<%#Eval("Sunday") %>' class="day weekend firstdayofweek">
                    <div class="dayNumber">
                        <asp:Literal ID="lSundayNumber" runat="server" Text='<%#Eval("Sunday") %>' /></div>
                    &nbsp;
                </td>
                <td id="MondayCell" runat="server" rel='<%#Eval("Monday") %>' class="day weekday">
                    <div class="dayNumber">
                        <asp:Literal ID="lMondayNumber" runat="server" Text='<%#Eval("Monday") %>' /></div>
                    &nbsp;
                </td>
                <td id="TuesdayCell" runat="server" rel='<%#Eval("Tuesday") %>' class="day weekday">
                    <div class="dayNumber">
                        <asp:Literal ID="lTuesdayNumber" runat="server" Text='<%#Eval("Tuesday") %>' /></div>
                    &nbsp;
                </td>
                <td id="WednesdayCell" runat="server" rel='<%#Eval("Wednesday") %>' class="day weekday">
                    <div class="dayNumber">
                        <asp:Literal ID="lWednesdayNumber" runat="server" Text='<%#Eval("Wednesday") %>' /></div>
                    &nbsp;
                </td>
                <td id="ThursdayCell" runat="server" rel='<%#Eval("Thursday") %>' class="day weekday">
                    <div class="dayNumber">
                        <asp:Literal ID="lThursdayNumber" runat="server" Text='<%#Eval("Thursday") %>' /></div>
                    &nbsp;
                </td>
                <td id="FridayCell" runat="server" rel='<%#Eval("Friday") %>' class="day weekday">
                    <div class="dayNumber">
                        <asp:Literal ID="lFridayNumber" runat="server" Text='<%#Eval("Friday") %>' /></div>
                    &nbsp;
                </td>
                <td id="SaturdayCell" runat="server" rel='<%#Eval("Saturday") %>' class="day weekend lastdayofweek">
                    <div class="dayNumber">
                        <asp:Literal ID="lSaturdayNumber" runat="server" Text='<%#Eval("Saturday") %>' /></div>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
    
    <div id="EventInfo" style="display:none;">
        <asp:PlaceHolder ID="phEventInfo" runat="server" />    
    </div>
</asp:Panel>
