<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CarouselThumbnailNav.ascx.vb"
    Inherits="CLibrary_CarouselThumbnailNav" %>
<asp:Panel ID="pCarousel" runat="server" CssClass="thumbnailCarousel">
    <div class="ui-helper-clearfix">
        <div class="carouselIconBg">
        </div>
        <div class="carouselIconNav">
            <ul>
            </ul>
        </div>
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
    </div>
</asp:Panel>
