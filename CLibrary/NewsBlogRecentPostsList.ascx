<%@ control language="VB" autoeventwireup="false" codefile="NewsBlogRecentPostsList.ascx.vb" inherits="CLibrary_NewsBlogRecentPostsList" %>

<div class="panel panel-primary">

    <div class="panel-heading">
        <h3 class="panel-title"><asp:Literal ID="lHeaderText" runat="server" /></h3>
    </div>

    <div class="panel-body">
        <div class="leftNavPrimary">
            <asp:ListView ID="lvPosts" runat="server" ItemPlaceholderID="phItemTemplate">
                <LayoutTemplate>
                    <ul>
                        <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <asp:HyperLink runat="server" ID="hplPost" NavigateUrl='<%#appxCMS.SEO.Rewrite.GetLink(Me.PostPathPrefix & Eval("Headline").ToString(), Eval("NewsID").ToString(), appxCMS.SEO.Rewrite.LinkType.NewsPost) %>' Text='<%#Eval("Headline") %>' /></li>
                </ItemTemplate>
            </asp:ListView>
        </div>  
    </div>

</div>
