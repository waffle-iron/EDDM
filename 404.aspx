<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Site.master" AutoEventWireup="false" CodeFile="404.aspx.vb" Inherits="_404" Title="4 uh-oh 4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phForm" runat="Server">

<div class="container">

    <div class="partialRibbon hidden-sm hidden-xs">
        <span class="arrowLeft"></span>
        <span class="subRibbonPop">Sorry!</span>
        <span class="subRibbon">Page Not Found</span>
    </div>

    <div class="partialRibbonSmall visible-sm visible-xs">
        <span class="subRibbonPopSmall">Sorry!</span>
        <span class="subRibbonSmall">Page Not Found</span>
    </div>
    

    <div class="contentWrapper">
        <div class="row">

            <div class="col-sm-12">
                <div class="jumbotron">
                    <p class="lead">
                        We're sorry. The page you are looking for cannot be found.  Please feel free to contact us for assistance!
                    </p>

                    <asp:Panel ID="pErrorInfo" runat="server" Visible="false">
                        <asp:Literal ID="lErr" runat="server" />
                    </asp:Panel>

                    <asp:Panel ID="pCMSCreate" runat="server" Visible="false" CssClass="makeInlineDialog"
                        ToolTip="Create this Page">
                        <asp:Literal ID="lUpdStatus" runat="server" />
                        <cms:CreatePage ID="CreatePage" runat="server" />
                    </asp:Panel>

                </div>
            </div>
        </div>
    </div>


</div>

</asp:Content>
