<%@ control language="VB" autoeventwireup="false" codefile="TemplatePagedList.ascx.vb" inherits="CCustom_TemplatePagedList" %>


<asp:ListView ID="lvTemplates" runat="server" ItemPlaceholderID="phItemTemplate">

    <LayoutTemplate>

        <div class="row">
            <div class="col-sm-10 col-sm-offset-1">
                <appx:DataPager ID="dpTop" runat="server" />
            </div>
        </div>

        <div class="row">
            <asp:PlaceHolder ID="phItemTemplate" runat="server" />
        </div>

    </LayoutTemplate>

    <ItemTemplate>

        <div class="col-sm-4">
            <div class="templateWrapper">

                <asp:PlaceHolder runat="server" ID="phName" Visible="<%#Me.ShowName %>">
                    <h5><asp:Literal runat="server" ID="lName" Text='<%#Eval("Name") %>'></asp:Literal></h5>
                </asp:PlaceHolder>

                <div class="text-center img-wrapper">
                    <asp:HyperLink ID="hplTemplate" runat="server">
                        <asp:Image ID="imgTemplate" runat="server" />
                    </asp:HyperLink>
                </div>

                <div class="text-center">
                    <h5>Template #<asp:Literal ID="lTemplateId" runat="server" Text='<%#Eval("TemplateId") %>' /></h5>
                    <small><asp:Literal ID="lBL" runat="server" Text='<%#Eval("BusinessLine.Name") %>' /></small><br />
                    <small><asp:Literal ID="lSize" runat="server" Text='<%#Eval("PageSize") %>' /></small>
                </div>

            </div>

            <div>&nbsp;</div>

        </div>

    </ItemTemplate>

</asp:ListView>
