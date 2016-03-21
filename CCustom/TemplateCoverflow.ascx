<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TemplateCoverflow.ascx.vb" Inherits="CCustom_TemplateCoverflow" %>

<div id="contentFlow" class="ContentFlow" style="height: 275px;">
    <!-- should be place before flow so that contained images will be loaded first -->
    <div class="loadIndicator">
        <div class="indicator"></div>
    </div>

    <div class="coverflowHeaderTxt">
        Sample some <asp:Literal ID="lPageSize" runat="server" /> templates...
    </div>

    <asp:ListView ID="lvCoverflow" runat="server" ItemPlaceholderID="phItemTemplate">
        <LayoutTemplate>
            <div class="flow">
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </div>
        </LayoutTemplate>

        <ItemTemplate>
            <asp:Panel ID="pItem" runat="server" CssClass="item">
                <asp:Image ID="imgTemplate" runat="server" CssClass="content" /><span class="caption">Something Here</span>
            </asp:Panel>
        </ItemTemplate>
    </asp:ListView>

    <div class="scrollbar">
        <div class="preButton">
        </div>
        <div class="nextButton">
        </div>
        <div class="slider">
            <div class="position">
            </div>
        </div>
    </div>

</div>


