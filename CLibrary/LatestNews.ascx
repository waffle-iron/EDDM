<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LatestNews.ascx.vb" Inherits="CLibrary_LatestNews" %>
<asp:Panel ID="pNews" runat="server">
    <asp:ListView ID="lvNews" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <div class="ui-helper-clearfix">
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </div>
            <%--<div class="sameHeight">
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </div>--%>
        </LayoutTemplate>
        <%--<grouptemplate>
            <div class="ui-helper-clearfix padbottom">
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </div>
        </grouptemplate>--%>
        <ItemTemplate>
            <div>
                <b>
                    <asp:HyperLink ID="hplNews" runat="server" NavigateUrl='<%#appxCMS.SEO.Rewrite.GetLink(Eval("Headline"), Eval("NewsID"), appxCMS.SEO.Rewrite.LinkType.NewsPost) %>'>
                        <asp:Literal ID="lHeadline" runat="server" Text='<%#Eval("Headline") %>' /></asp:HyperLink></b></div>
            <div>
                <asp:Literal ID="lStory" runat="server" Text='<%#Eval("Summary") & "..." %>' />
                <asp:HyperLink ID="hplReadMore" runat="server" NavigateUrl='<%#appxCMS.SEO.Rewrite.GetLink(Eval("Headline"), Eval("NewsID"), appxCMS.SEO.Rewrite.LinkType.NewsPost) %>'>read more</asp:HyperLink></div>
        </ItemTemplate>
        <ItemSeparatorTemplate>
            <hr />
        </ItemSeparatorTemplate>
        <EmptyDataTemplate>
            <p>
                <asp:Literal ID="lEmptyMessage" runat="server" /></p>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Panel>
