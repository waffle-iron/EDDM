<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ImageFader.ascx.vb" Inherits="CLibrary_ImageFader" %>
<asp:Panel ID="pImgSingle" runat="server" Visible="false"><asp:Image ID="imgSingle" runat="server" /></asp:Panel>
<asp:Panel ID="pContainer" runat="server" Style="position: relative;" CssClass="ui-helper-clearfix">
    <asp:ListView ID="lvImageRotate" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <ul id="imgRotate" class="imgRotate" style="display: none; list-style: none; margin: 0;
                padding: 0;">
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <asp:Image ID="imgRotated" runat="server" ImageUrl='<%#Container.DataItem %>' /></li>
        </ItemTemplate>
    </asp:ListView>
</asp:Panel>
