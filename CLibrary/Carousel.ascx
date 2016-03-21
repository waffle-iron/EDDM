<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Carousel.ascx.vb" Inherits="CLibrary_Carousel" %>
<asp:Panel ID="pCarousel" runat="server" CssClass="generalCarousel">
    <asp:ListView ID="lvCarousel" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <asp:Literal ID="lCarouselStart" runat="server" />
            <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <asp:HyperLink ID="hplItem" runat="server" NavigateUrl='<%#Eval("NavigateUrl") %>'>
                    <asp:Image ID="imgItem" runat="server" ImageUrl='<%#Eval("ImageUrl") %>' ToolTip='<%#Eval("Title") %>'
                        AlternateText='<%#Eval("Title") %>' /></asp:HyperLink>
            </li>
        </ItemTemplate>
    </asp:ListView>
</asp:Panel>
