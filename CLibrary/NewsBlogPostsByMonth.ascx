<%@ control language="VB" autoeventwireup="false" codefile="NewsBlogPostsByMonth.ascx.vb" inherits="CLibrary_NewsBlogPostsByMonth" %>
<div class="panel panel-primary">

    <div class="panel-heading">
        <h3 class="panel-title"><asp:Literal ID="lHeaderText" runat="server" /></h3>
    </div>

    <div class="panel-body">

        <div id="accordionArchives">

            <asp:ListView ID="lvYears" runat="server" ItemPlaceholderID="phItemTemplate">

                <LayoutTemplate>
                    <div id="postsByYears">

                        <p><small>(Click Year to view archives.)</small></p>

                        <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                    </div>
                </LayoutTemplate>

                <ItemTemplate>
                    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                    
                        <p><strong><asp:HyperLink runat="server" ID="hplYear" data-toggle="collapse" data-parent="#postsByYears"><%#Container.DataItem %></asp:HyperLink></strong></p>
                    
                        <asp:Panel runat="server" ID="pYear" CssClass='<%#String.Format("panel-collapse collapse{0}", Iif(Container.DataItemIndex=0, " in", "")) %>' role="tabpanel">
                            <div class="leftNavPrimary">
                                <asp:ListView ID="lvPosts" runat="server" ItemPlaceholderID="phItemTemplate">
                                    <LayoutTemplate>
                                        <ul>
                                            <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                                        </ul>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <asp:HyperLink runat="server" ID="hplPost" NavigateUrl='<%#appxCMS.SEO.Rewrite.BuildLink(Eval("Name"), Eval("Month") &"-" & Eval("Year") & "-0", "blogarchive") %>' Text='<%#Eval("Name") & " (" & Eval("Count", "{0:N0}") & ")" %>' />
                                        </li>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </asp:Panel>

                    </div>
                </ItemTemplate>
            </asp:ListView>


            <div class="leftNavPrimary">
                <asp:ListView ID="lvPosts" runat="server" ItemPlaceholderID="phItemTemplate">

                    <LayoutTemplate>
                        <ul>
                            <asp:PlaceHolder runat="server" ID="phItemTemplate" />
                        </ul>
                    </LayoutTemplate>

                    <ItemTemplate>
                        <li>
                            <asp:HyperLink runat="server" ID="hplPost" NavigateUrl='<%#appxCMS.SEO.Rewrite.BuildLink(Eval("Name"), Eval("Month") &"-" & Eval("Year") & "-0", "blogarchive") %>' Text='<%#Eval("Name") & " (" & Eval("Count", "{0:N0}") & ")" %>' />
                        </li>
                    </ItemTemplate>

                </asp:ListView>
            </div>

        </div>

    </div>

</div>


