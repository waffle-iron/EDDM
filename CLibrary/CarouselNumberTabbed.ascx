<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CarouselNumberTabbed.ascx.vb"
    Inherits="CLibrary_CarouselNumberTabbed" %>
<asp:Panel ID="pCarousel" runat="server" CssClass="tabbedCarousel">
    <asp:ListView ID="lvCarousel" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <asp:Literal ID="lCarouselStart" runat="server" />
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </ul>
            <asp:Panel ID="pCarouselSideBar" runat="server" CssClass="sideBar">
                <asp:HyperLink ID="hplSidebar" runat="server"><asp:Image ID="imgSidebar" runat="server" /></asp:HyperLink>
            </asp:Panel>
            <asp:Literal ID="lCarouselControl" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <asp:HyperLink ID="hplItem" runat="server" NavigateUrl='<%#Eval("NavigateUrl") %>'>
                    <asp:Image ID="imgItem" runat="server" ImageUrl='<%#Eval("ImageUrl") %>' ToolTip='<%#Eval("Title") %>' AlternateText='<%#Eval("Title") %>' /></asp:HyperLink>
            </li>
        </ItemTemplate>
    </asp:ListView>
</asp:Panel>