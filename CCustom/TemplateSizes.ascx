<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TemplateSizes.ascx.vb" Inherits="CCustom_TemplateSizes" %>
<asp:ListView id="lvSizes" runat="server" ItemPlaceHolderId="phItemTemplate">
    <layouttemplate>
        <div class="contentBlock">
            <div class="contentHeader"><b>Available</b> Sizes</div>
            <ul class="arrowList">
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </ul>
        </div>
    </layouttemplate>
    <itemtemplate>
        <li><asp:HyperLink ID="hplSize" runat="server" Text='<%#Eval("Name") %>' /></li>
    </itemtemplate>
</asp:ListView>