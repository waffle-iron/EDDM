<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TemplateSearch.ascx.vb" Inherits="CCustom_TemplateSearch" %>

<asp:Panel ID="pSearch" runat="server" DefaultButton="btnSearch" CssClass="well well-sm">
    
    <div role="form">

        <div><small><strong>Search Templates</strong></small></div>

        <div class="form-group">
            <label for="Company" class="label formLabelRequired">Keyword</label>
            <asp:TextBox ID="Keyword" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="rfvKeyword" ControlToValidate="Keyword" runat="server" ErrorMessage="A keyword is required." Display="dynamic" CssClass="label label-danger" SetFocusOnError="True" ValidationGroup="templateGroup">
                A keyword is required.
            </asp:RequiredFieldValidator>
        </div>

        <div>
            <asp:Button ID="btnSearch" runat="server" Text="Find Templates" CssClass="btn btn-primary pull-right" ValidationGroup="templateGroup" />
        </div>
        
        <div>&nbsp;</div>
        <div>&nbsp;</div>

    </div>

</asp:Panel>
