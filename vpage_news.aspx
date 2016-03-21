<%@ Page Language="VB" MasterPageFile="~/App_Masterpages/Site.master" AutoEventWireup="false" CodeFile="vpage_news.aspx.vb" Inherits="vpage_news" title="Untitled Page" %>
<asp:Content ID="Content2" ContentPlaceHolderID="phBody" Runat="Server">
    <h1><asp:Literal ID="lHeadline" runat="server" /></h1>
    <p class="newsPostedDate"><asp:Literal ID="lPublishDate" runat="server" /></p>
    <div class="newsBody">
        <asp:Literal ID="lStory" runat="server"></asp:Literal>
    </div>
    <div class="newsByLine">by: <span class="newsAuthor"><asp:Literal ID="lAuthor" runat="server" /></span></div>
</asp:Content>

