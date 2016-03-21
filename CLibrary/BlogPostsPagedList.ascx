<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BlogPostsPagedList.ascx.vb" Inherits="CLibrary_BlogPostsPagedList" %>
<%@ Register TagPrefix="bootstrap" TagName="DataPager" Src="~/usercontrols/BootstrapDataPager.ascx" %>
<div class="text-right">
    <bootstrap:DataPager runat="server" ID="blogPagerTop" />
</div>

<asp:ListView id="lvPosts" runat="server" ItemPlaceHolderId="phItemTemplate">

    <layouttemplate>
        <asp:PlaceHolder id="phItemTemplate" runat="server"/>
    </layouttemplate>

    <itemtemplate>
        <asp:Placeholder runat="server" id="phPost"/>
    </itemtemplate>

</asp:ListView>
<div class="text-right">
    <bootstrap:DataPager runat="server" ID="blogPager" />
</div>
