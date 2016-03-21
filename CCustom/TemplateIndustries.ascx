<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TemplateIndustries.ascx.vb"
    Inherits="CCustom_TemplateIndustries" %>
<asp:ListView id="lvIndustries" runat="server" ItemPlaceHolderId="phItemTemplate">
    <layouttemplate>
        <div class="contentBlock">
            <div class="contentHeader"><b>Industry</b> Choices</div>
            <ul class="arrowList">
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </ul>
        </div>
    </layouttemplate>
    <itemtemplate>
        <li><asp:HyperLink ID="hplIndustry" runat="server" Text='<%#Eval("Name") %>' /></li>
    </itemtemplate>
</asp:ListView>