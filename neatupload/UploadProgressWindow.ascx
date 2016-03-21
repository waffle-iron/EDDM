<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UploadProgressWindow.ascx.vb" Inherits="neatUpload_UploadProgressWindow" %>
<asp:Panel ID="ProgressDialog" runat="server" ToolTip="Upload Progress">
    <div align="center">
        <neatUpload:ProgressBar ID="neatProgressBar" Url="/neatUpload/Progress.aspx" Width="500" Height="75" runat="server" Inline="true" />
    </div>
    <div align="center">
        <asp:Button ID="HideButton" runat="server" Text="Close Progress Window (does not cancel upload)" CssClass="btn btn-primary"></asp:Button>
    </div>
</asp:Panel>
