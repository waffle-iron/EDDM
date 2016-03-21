<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KenshooTrackingPixels.ascx.cs" Inherits="KenshooTrackingPixels" %>

<%--This is embedded on the Receipt.aspx page.--%>

<asp:Panel ID="pnlKenshoo" runat="server" Visible="False">
    
    <asp:Literal ID="litKenshooScript" runat="server" /><br />
    <asp:Literal ID="litKenshooPixels" runat="server" />
    
</asp:Panel>
