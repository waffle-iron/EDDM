<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MenuLinkList.ascx.vb" Inherits="CLibrary_MenuLinkList" %>
<asp:Panel ID="pMenuLinks" runat="server">
    <asp:ListView ID="lvMenu" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <table border="0" cellpadding="5" cellspacing="0" style="margin:auto;">
                <tr>
                    <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <td style="text-align:center;"><asp:HyperLink ID="hplLink" runat="server" Text='<%#Eval("InnerHtml") %>' NavigateUrl='<%#Eval("Url") %>' /></td>
        </ItemTemplate>
        <ItemSeparatorTemplate><td><%=Me.Separator%></td></ItemSeparatorTemplate>
    </asp:ListView>
</asp:Panel>