<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TemplateIndustriesDropDown.ascx.vb" Inherits="CCustom_TemplateIndustriesDropDown" %>


<div class="panel panel-primary">

    <div class="panel-heading">
        <div class="panel-title"><asp:Literal ID="lHeader" runat="server" Text="<strong>Industry</strong> Choices" /></div>
    </div>

    <div class="panel-body">

        <div class="form-group">
            <asp:DropDownList ID="Industry" runat="server" AppendDataBoundItems="true" CssClass="form-control">
                <asp:ListItem Text="Choose Industry" Value="" />
            </asp:DropDownList>
        </div>

        <div>
            <asp:LinkButton ID="lnkChooseIndustry" runat="server" Text="Go" CssClass="btn btn-primary pull-right" />
        </div>

    </div>

</div>  

