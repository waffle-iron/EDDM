<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/admin/SiteManager.master"
    AutoEventWireup="false" CodeFile="Site.aspx.vb" Inherits="admin_Site" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" runat="Server">
    <div id="SiteInfo">
        <asp:Literal ID="lMsg" runat="server" />
        <asp:ValidationSummary ID="vsEdit" runat="server" ValidationGroup="vgEdit" />
        <div class="row">
            <div class="label">
                Name</div>
            <div class="aright mid">
                <asp:TextBox ID="Name" runat="server" /></div>
        </div>
        <div class="row">
            <div class="label">
                Address 1</div>
            <div class="aright mid">
                <asp:TextBox ID="Address1" runat="server" /></div>
        </div>
        <div class="row">
            <div class="label">
                Address 2</div>
            <div class="aright mid">
                <asp:TextBox ID="Address2" runat="server" /></div>
        </div>
        <div class="row">
            <div class="label">
                City</div>
            <div class="aright mid">
                <asp:TextBox ID="City" runat="server" /></div>
        </div>
        <div class="row">
            <div class="label">
                State</div>
            <div class="aright">
                <geoselect:State ID="State" runat="server" ValidationGroup="vgEdit" RequireField="false" />
            </div>
        </div>
        <div class="row">
            <div class="label">
                Zip Code</div>
            <div class="aright">
                <asp:TextBox ID="ZipCode" runat="server" MaxLength="10" /></div>
        </div>
        <div class="row">
            <div class="label">
                Local Phone Number:</div>
            <div class="aright">
                <asp:TextBox ID="Phone" runat="server" MaxLength="20" /></div>
        </div>
        <div class="row">
            <div class="label">
                Toll-Free Number:</div>
            <div class="aright">
                <asp:TextBox ID="TollFreeNumber" runat="server" MaxLength="20" /></div>
        </div>
        <div class="row">
            <div class="label">
                Fax Number:</div>
            <div class="aright">
                <asp:TextBox ID="Fax" runat="server" MaxLength="20" /></div>
        </div>
        <div class="row">
            <div class="label">
                Email Address:</div>
            <div class="aright mid">
                <asp:TextBox ID="Email" runat="server" /></div>
        </div>
        <div class="row">
            <div class="label">
                Active</div>
            <div class="aright">
                <asp:CheckBox ID="Active" runat="server" /></div>
        </div>
        <div class="row">
            <div class="label">
                Enable Sign In</div>
            <div class="aright">
                <asp:CheckBox ID="EnableSignin" runat="server" /></div>
        </div>
        <div class="row">
            <div class="label">
                Enable Sign Up</div>
            <div class="aright">
                <asp:CheckBox ID="EnableSignup" runat="server" /></div>
        </div>
        <div class="clear">
            &nbsp;</div>
        <div class="makeButtonPane">
            <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="makeSaveButton"
                ValidationGroup="vgEdit" />
        </div>
    </div>
</asp:Content>
