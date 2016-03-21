<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GoogleTranslateThisPage.ascx.vb"
    Inherits="CLibrary_GoogleTranslateThisPage" %>
<asp:Panel ID="pGoogleTranslate" runat="server">
    <b>Translate this Page:</b>
    <asp:DropDownList ID="ddTargetLanguage" runat="server" AppendDataBoundItems="true">
        <asp:ListItem Text="" Value="" />
        <asp:ListItem Value="es" Text="Spanish" />
        <asp:ListItem Value="hi" Text="Hindi" />
        <asp:ListItem Value="zh-CN" Text="Chinese (Simplified)" />
        <asp:ListItem Value="zh-TW" Text="Chinese (Traditional)" />
        <asp:ListItem Value="tl" Text="Filipino" />
        <asp:ListItem Value="ja" Text="Japanese" />
        <asp:ListItem Value="ko" Text="Korean" />
        <asp:ListItem Value="vi" Text="Vietnamese" />
    </asp:DropDownList>
</asp:Panel>
