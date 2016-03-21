<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TemplateSearchPagedList.ascx.vb" Inherits="CCustom_TemplateSearchPagedList" %>

<asp:ListView ID="lvTemplates" runat="server" ItemPlaceholderID="phItemTemplate">
    
    <LayoutTemplate>

        <asp:Panel ID="pnlSearchPhrase" runat="server" CssClass="alert alert-info">
            <p>You searched for '<asp:Literal ID="lSearchTerm" runat="server" />'</p>
        </asp:Panel>

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

                <div class="text-center">
                    <asp:HyperLink ID="hplTemplate" runat="server">
                    <asp:Image ID="imgTemplate" runat="server" CssClass="img-thumbnail templateThumbnail" />
                    </asp:HyperLink>
                </div>

                <div class="text-center">
                    <h5>Template #<asp:Literal ID="lTemplateId" runat="server" Text='<%#Eval("TemplateId") %>' /></h5>
                    <small><asp:Literal ID="lBL" runat="server" Text='<%#Eval("BusinessLine.Name") %>' /></small><br />
                    <small><asp:Literal ID="lSize" runat="server" Text='<%#Eval("PageSize") %>' /></small>
                    <small><asp:Literal ID="Literal1" runat="server" Text='<%#Iif(Not Eval("Name").ToString().Equals("Place Holder Name"), Eval("Name"), "") %>' /></small>

                </div>

            </div>

            <div>&nbsp;</div>

        </div>

    </ItemTemplate>

    <EmptyDataTemplate>
        
        <p>&nbsp;</p>

        <div class="well well-lg">
            <p class="text-center">Sorry - No templates were found matching your search criteria.</p>
        </div>

    </EmptyDataTemplate>

</asp:ListView>