<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GetStartedMappingWithAddress.ascx.vb"
    Inherits="CCustom_GetStartedMappingWithAddress" %>
<div class="ui-helper-clearfix left ui-corner-all" style="padding:5px;">
    <div class="fifty fleft">
        <b>Street Address:</b><br />
        <asp:TextBox ID="StreetAddress" runat="server" Width="90%" />
    </div>
    <div class="twenty fleft">
        <b>Zip Code:</b>
        <asp:TextBox ID="ZipCode" runat="server" Columns="6" />
    </div>
    <div class="thirty fleft actionlow">
        <asp:LinkButton ID="lnkGetStarted" runat="server" Text="Get Started" CssClass="makeButton" />
    </div>
</div>
