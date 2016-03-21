<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NewsBlogCategoryList.ascx.vb"
    Inherits="CLibrary_NewsBlogCategoryList" %>
<asp:Panel ID="pCategories" runat="server">
    <asp:ObjectDataSource ID="oCats" runat="server" 
        TypeName="appxCMS.NewsBlogCategoryDataSource" 
        OldValuesParameterFormatString="{0}" SelectMethod="GetList" />
    <asp:ListView ID="lvCategories" runat="server" ItemPlaceholderID="phItemTemplate"
        DataSourceID="oCats">
        <LayoutTemplate>
            <ul>
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <asp:HyperLink ID="hplCat" runat="server" Text='<%#Eval("Name") %>' NavigateUrl='<%#appxCMS.SEO.Rewrite.GetLink(Eval("Name"), Eval("CategoryId"), appxCMS.SEO.Rewrite.LinkType.NewsBlog) %>' /></li>
        </ItemTemplate>
    </asp:ListView>
</asp:Panel>
