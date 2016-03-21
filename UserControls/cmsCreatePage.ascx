<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cmsCreatePage.ascx.vb"
    Inherits="UserControls_cmsCreatePage" %>
<asp:ValidationSummary ID="vSumm" runat="server" HeaderText="Please correct the following:"
    ValidationGroup="vgAdd" />
<div class="row">
    <div class="label">
        <asp:RequiredFieldValidator ID="rfvPageRef" runat="server" ErrorMessage="The page is required."
            ControlToValidate="PageRef" InitialValue="" Text="*" ValidationGroup="vgAdd" />Page:</div>
    <div class="aright">
        <asp:TextBox ID="PageRef" runat="server" Width="90%" />
        <div class="explainer">
            Use a relative path to your virtual page, ex., /mynewpage. Avoid using spaces or
            other non-URL friendly characters in the virtual page path.</div>
    </div>
</div>
<div class="row">
    <div class="label">
        <asp:RequiredFieldValidator ID="rfvPageTemplate" runat="server" ErrorMessage="The page template is required."
            ControlToValidate="SelectedTemplate" InitialValue="" Text="*" ValidationGroup="vgAdd" />Page
        Template:</div>
    <div class="aright">
        <asp:ObjectDataSource ID="oTemplates" runat="server" SelectMethod="GetPageTemplates"
            TypeName="CMSDAL" />
        <asp:DropDownList ID="SelectedTemplate" runat="server" AppendDataBoundItems="true"
            DataSourceID="oTemplates" DataValueField="TemplateFile" DataTextField="TemplateName">
            <asp:ListItem Text="Select One" Value="" />
        </asp:DropDownList>
    </div>
</div>
<div class="clear">
</div>
<div class="makeButtonPane">
    <asp:LinkButton ID="lnkAddContent" runat="server" Text="Add Page" CssClass="makeSaveButton"
        ValidationGroup="vgAdd" /></div>
