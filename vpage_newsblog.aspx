<%@ Page Language="VB" MasterPageFile="~/App_MasterPages/OneColumn.master" AutoEventWireup="false"
    CodeFile="vpage_newsblog.aspx.vb" Inherits="vpage_newsblog" Title="Untitled Page" %>
<%@ Register TagPrefix="appx" TagName="BlogPage" Src="~/CLibrary/BlogPage.ascx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="phBody" runat="Server">
    <appx:BlogPage runat="server"/>
</asp:Content>
