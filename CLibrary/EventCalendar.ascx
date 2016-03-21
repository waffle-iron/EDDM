<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EventCalendar.ascx.vb"
    Inherits="CLibrary_EventCalendar" %>
<asp:Panel ID="pCalendar" runat="server">
    <h1>
        <asp:Literal ID="lTitle" runat="server" /></h1>
    <asp:ListView ID="lvCalendar" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <asp:PlaceHolder ID="phItemTemplate" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <asp:Panel ID="pMonthHead" runat="server" Visible="false">
                <h1 class="calendarHeader">
                    <asp:Literal ID="lCalendarMonth" runat="server" /></h1>
            </asp:Panel>
            <dl class="calEntries">
                <dt>
                    <asp:Literal ID="lCalendarDate" runat="server" /></dt>
                <dd>
                    <div class="calTitle">
                        <asp:HyperLink ID="hplEventLink" runat="server" NavigateUrl='<%#appxCMS.SEO.Rewrite.GetLink(Eval("EventTitle"), Eval("EventID"), appxCMS.SEO.Rewrite.LinkType.EventEntry)%>'>
                            <asp:Literal ID="lCalendarTitle" runat="server" Text='<%#Eval("EventTitle") %>' /></asp:HyperLink></div>
                    <asp:Panel ID="pCalDesc" runat="server" CssClass="calDesc">
                        <asp:Literal ID="lCalDesc" runat="server" Text='<%#Eval("EventSummary") %>' /></asp:Panel>
                </dd>
            </dl>
        </ItemTemplate>
    </asp:ListView>
</asp:Panel>
