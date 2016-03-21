<%@ Page Language="VB" MasterPageFile="~/App_MasterPages/OneColumn.master" AutoEventWireup="false"
    CodeFile="vpage_blogpost.aspx.vb" Inherits="vpage_blogpost" Title="Untitled Page" %>

<asp:Content ID="Content0" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="/app_styles/Blog.css" />
    <asp:Literal ID="lRDF" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
    <asp:PlaceHolder id="phPostPage" runat="server"/>
    <%--<div class="ui-helper-clearfix">
        <asp:Panel ID="pPostNav" runat="server" CssClass="postnav">
            <asp:HyperLink ID="hplPrevPost" runat="server" CssClass="prev" />
            |
            <asp:HyperLink ID="hplNextPost" runat="server" CssClass="next" />
        </asp:Panel>
        <div class="feeds">
            <asp:HyperLink ID="hplRSS20" runat="server" CssClass="rssLink">RSS Feed</asp:HyperLink></div>
        <div class="post">
            <h1>
                <asp:Literal ID="lHeadline" runat="server" /></h1>
            <div class="byline ui-helper-clearfix">
                <span class="author">By
                    <asp:Literal ID="lAuthor" runat="server" /></span> <span class="pubdate">
                        <asp:Literal ID="lPubDate" runat="server" /></span>
            </div>
            <div class="text">
                <asp:PlaceHolder ID="phText" runat="server" />
                <asp:Literal ID="lText" runat="server" />
            </div>
        </div>
        <asp:Panel ID="pComments" runat="server">
            <div class="commentList" id="CommentStart">
                <h2>
                    Comments</h2>
                <asp:ListView ID="lvComments" runat="server" ItemPlaceholderID="phItemTemplate">
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <asp:Panel ID="pComment" runat="server" CssClass="blogComment">
                            <div class="blogCommentIndex">
                                <asp:Literal ID="lIndex" runat="server" Text='<%#Container.DataItemIndex+1 %>' /></div>
                            <div class="blogCommentBody">
                                <asp:Literal ID="lComment" runat="server" Text='<%#Eval("Comment") %>' /></div>
                            <div class="blogCommentFooter">
                                <asp:HyperLink ID="hplCommentBy" runat="server" Text='<%#String.Format("{0} {1}", Eval("FirstName"), Eval("LastName")) %>'
                                    NavigateUrl='<%#Eval("Url") %>' />
                                on <span class="blogCommentDate">
                                    <asp:Literal ID="lCommentDate" runat="server" Text='<%#String.Format("{0} at {1}", DateTime.Parse(Eval("CommentDate")).ToString("dddd, dd MMMM yyyy"), DateTime.Parse(Eval("CommentDate")).ToString("%h:mm tt")) %>' /></span></div>
                        </asp:Panel>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <p>
                            No comments yet.</p>
                    </EmptyDataTemplate>
                </asp:ListView>
                <newsblog:Comment ID="BlogComment" runat="server" />
            </div>
        </asp:Panel>
    </div>--%>
</asp:Content>
