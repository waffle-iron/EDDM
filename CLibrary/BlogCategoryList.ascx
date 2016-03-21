<%@ control language="VB" autoeventwireup="false" codefile="BlogCategoryList.ascx.vb" inherits="CLibrary_BlogCategoryList" %>
<div class="panel panel-primary">
    <asp:PlaceHolder runat="server" ID="phPanelHeader">
        <div class="panel-heading">
            <h3 class="panel-title"><asp:Literal ID="lHeaderText" runat="server" /></h3>
        </div>
    </asp:PlaceHolder>

    <div class="panel-body">
        <div class="leftNavPrimary">
            <asp:ListView ID="lvCategoryList" runat="server" ItemPlaceholderID="phItemTemplate">
                <LayoutTemplate>
                    <ul>
                        <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                    </ul>
                </LayoutTemplate>

                <ItemTemplate>
                    <li>
                        <asp:HyperLink runat="server" ID="hplPost" NavigateUrl='<%#appxCMS.SEO.Rewrite.BuildLink(Eval("Name").ToString(), Eval("CategoryId").ToString, "blogcategory") %>' Text='<%#Eval("Name") %>' /></li>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>

</div>
