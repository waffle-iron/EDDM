<%@ control language="VB" autoeventwireup="false" codefile="BlogTagCloud.ascx.vb" inherits="CLibrary_BlogTagCloud" %>
<div class="panel panel-primary">
    <asp:PlaceHolder runat="server" ID="phPanelHeader">
        <div class="panel-heading">
            <h3 class="panel-title"><asp:Literal ID="lHeaderText" runat="server" /></h3>
        </div>
    </asp:PlaceHolder>

    <div class="panel-body">
        <asp:Panel ID="pTags" runat="server">
            <asp:ListView ID="lvTagCloud" runat="server" ItemPlaceholderID="phItemTemplate">

                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                </LayoutTemplate>

                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hplTag" NavigateUrl='<%#appxCMS.SEO.Rewrite.BuildLink("", Eval("Name").ToString, "blogtag-") %>' Text='<%#Eval("Name") %>' />
                </ItemTemplate>

            </asp:ListView>
        </asp:Panel>
    </div>
</div>
