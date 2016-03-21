<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Member.master" AutoEventWireup="false"
    CodeFile="account_designs.aspx.vb" Inherits="account_designs" Title="My Designs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phBody" runat="Server">
    <h1>My Designs</h1>
    <asp:Literal ID="lCookieFrame" runat="server" />
    <asp:ListView ID="lvSaved" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <h2>
                Saved Designs</h2>
            <asp:PlaceHolder ID="phItemTemplate" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <div style="border: 1px solid #CCC; padding: 5px; margin: 5px 0px;">
                <div class="ui-helper-clearfix">
                    <asp:Image ID="imgDesign" runat="server" ImageUrl='<%#Eval("Preview") %>' Style="float: left;
                        height: 47px; padding-right: 1em;" />
                    <b>Saved Design</b> <i>(Saved on
                        <asp:Literal ID="lCreatedDate" runat="server" Text='<%#DateTime.Parse(Eval("SavedDate")).ToString("dddd, dd MMMM yyyy") %>' />)</i>
                    <asp:HyperLink ID="hplOpen" runat="server" NavigateUrl='<%#Eval("OpenLink") %>' Text="Continue Editing..." />
                </div>
            </div>
        </ItemTemplate>
        <EmptyDataTemplate>
            <h2>
                Saved Designs</h2>
            <p>
                You do not have any saved designs.</p>
        </EmptyDataTemplate>
    </asp:ListView>
    <asp:ListView ID="lvDesigns" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <h2>
                Ready to Order</h2>
            <asp:PlaceHolder ID="phItemTemplate" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <div style="border: 1px solid #CCC; padding: 5px; margin: 5px 0px;">
                <div>
                    <div>
                        <b>
                            <asp:Literal ID="lDesignCat" runat="server" />
                            Design</b> <i>(Created on
                                <asp:Literal ID="lCreatedDate" runat="server" Text='<%#DateTime.Parse(Eval("CreatedDate")).ToString("dddd, dd MMMM yyyy") %>' />)</i>
                        <asp:LinkButton ID="lnkNextSteps" runat="server" OnClick="NextSteps" CommandArgument='<%#Eval("CustomerDesignId") %>' /></div>
                </div>
                <div class="ui-helper-clearfix">
                    <asp:Image ID="imgDesign" runat="server" Style="float: left;" />
                    <asp:Image ID="imgDesignBack" runat="server" Visible="false" Style="float: left;" />
                </div>
            </div>
        </ItemTemplate>
        <EmptyDataTemplate>
            <h2>
                Ready to Order</h2>
            <p>
                You do not have any read to order designs.</p>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
