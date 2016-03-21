<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BlogComment.ascx.vb"
    Inherits="UserControls_BlogComment" %>
<div id="CommentForm">
    <asp:Panel ID="pNoComments" runat="server" Visible="false">
        <p>
            Comments are disabled for this post.</p>
    </asp:Panel>
    <asp:Panel ID="pNoAnonymous" runat="server" Visible="false">
        <p>Anonymous comments are disabled for this post. <asp:HyperLink ID="hplSignin" runat="server" Text="Sign In" /></p>
    </asp:Panel>
    <asp:Panel ID="pAddComment" runat="server" CssClass="addCommentWrapper ui-helper-clearfix" Visible="false">
        <h2>
            Add a comment</h2>
        <asp:Panel ID="pAnonymousInformation" runat="server" Visible='<%#Iif(HttpContext.Current.User.IsInRole("Member") Or HttpContext.Current.User.IsInRole("Admin"), False, True) %>'>
            <div class="row">
                <div class="label">
                    First Name</div>
                <div class="aright">
                    <asp:TextBox ID="FirstName" runat="server" Width="60%" /></div>
            </div>
            <div class="row">
                <div class="label">
                    Last Name</div>
                <div class="aright">
                    <asp:TextBox ID="LastName" runat="server" Width="60%" /></div>
            </div>
            <div class="row">
                <div class="label">
                    Email</div>
                <div class="aright">
                    <asp:TextBox ID="Email" runat="server" Width="60%" /></div>
            </div>
        </asp:Panel>
        <div class="row">
            <div class="label">
                Home Page</div>
            <div class="aright">
                <asp:TextBox ID="Url" runat="server" Width="60%" /></div>
        </div>
        <div class="row">
            <div class="label">
                Comment</div>
            <div class="aright">
                <asp:TextBox ID="Comment" runat="server" TextMode="MultiLine" Rows="5" Width="90%" /></div>
        </div>
        <div class="row">
            <div class="label">
                &nbsp;</div>
            <div class="aright">
                <asp:Button ID="btnSaveComment" runat="server" Text="Add a Comment" /></div>
        </div>
    </asp:Panel>
</div>
