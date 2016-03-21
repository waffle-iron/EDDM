<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false"
    CodeFile="Unsubscribe.aspx.vb" Inherits="Unsubscribe" %>

<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
    <h2>
        Unsubscribe</h2>
    <asp:Panel ID="pSubInfo" runat="server">
        <p>
            You have received a message because of your membership in a mailing list on this
            site.
        </p>
        <p>Please unsubscribe my account from the following lists:</p>
        <div class="row">
            <div class="label">&nbsp;</div>
            <div class="aright"><asp:CheckBoxList ID="chkUnsubscribe" runat="server" /></div>
        </div>
        <div class="row">
            <div class="label">&nbsp;</div>
            <div class="aright"><asp:Button ID="btnSave" runat="server" Text="Save My Settings" /></div>
        </div>
        <div class="clear">&nbsp;</div>
    </asp:Panel>
    <asp:Panel ID="pRemoved" runat="server" Visible="false">
        <p>Your subscription changes have been saved.</p>
        <p>You can modify your account settings at any time by logging into your account.</p>
    </asp:Panel>
    <asp:Panel ID="pInvalid" runat="server" Visible="false">
        <p>
            <b>We're sorry.</b></p>
        <p>We cannot validate your unsubscription request based on the criteria
            passed in the URL. It could be that the information was truncated by your e-mail
            client. To remove yourself from our mailing list, please login to your account to
            manage your list preferences. You can also try to copy and paste the unsubscribe
            URL from the e-mail message you received to try this action again.</p>
    </asp:Panel>
</asp:Content>
