<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NewsBlogArchiveList.ascx.vb"
    Inherits="CLibrary_NewsBlogArchiveList" %>
<asp:ListView id="lvArchive" runat="server" ItemPlaceHolderId="phItemTemplate">
    <layouttemplate>
        <asp:PlaceHolder id="phItemTemplate" runat="server" />
    </layouttemplate>
    <itemtemplate>
        <h2 id="monthHeader" runat="server" visible="false"><%#Eval("PublishDate", "{0:MMMM yyyy}")%></h2>
        <asp:HyperLink id="hplEntry" runat="server" NavigateUrl='<%#appxCMS.SEO.Rewrite.GetLink(Eval("Headline"), Eval("NewsID"), appxCMS.SEO.Rewrite.LinkType.NewsPost) %>' style="color:#222;">
        <div class="ui-helper-clearfix">
            <div class="fleft" style="width:15%;color:#C8C8C8;">
                <div style="padding-right:10px;text-align:right;padding-top:10px;"><%#Eval("PublishDate", "{0:ddd}")%> the <asp:Literal ID="lDay" runat="server" Text='<%#appxCMS.Util.formatHelp.FormatTextCount(Eval("PublishDate", "{0:%d}"), True) %>' /></div>
            </div>
            <div class="fleft" style="width:80%;position:relative;">
                <div style="border-left:1px solid #C8C8C8;padding:10px 0 10px 40px;">
                <asp:Image ID="imgPreview" runat="server" Visible="false" style="float:right;" Width="200" Height="150" ImageUrl="/cmsimages/spacer.gif" CssClass="postPreviewImage" />
                <h3 style="margin:0; padding-bottom:14px;"><%#Eval("Headline")%></h3>
                <p><%#Eval("Summary") %></p>
                </div>
            </div>
        </div>
        </asp:HyperLink>
    </itemtemplate>
    <emptydatatemplate>
        <asp:Literal id="lEmptyMessage" runat="server" />
    </emptydatatemplate>
</asp:ListView>