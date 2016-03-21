<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/OneColumn.master"
    AutoEventWireup="false" CodeFile="vpage_templatepagesize.aspx.vb" Inherits="vpage_templatepagesize" %>

<%@ Register TagPrefix="wl" TagName="BannerFull" Src="~/CCustom/BannerFull.ascx" %>
<%@ Register TagPrefix="wl" TagName="TemplateSizes" Src="~/CCustom/TemplateSizes.ascx" %>
<%@ Register TagPrefix="wl" TagName="PagedTemplates" Src="~/CCustom/TemplatePagedList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
    <wl:BannerFull ID="BannerFull" runat="server" IntroText="Templates" />
    <div class="ui-helper-clearfix mtop">
        <div class="thirty fleft">
            <div class="padright">
                <wl:TemplateSizes ID="TemplateSizes" runat="server" />
            </div>
        </div>
        <div class="seventy fleft">
            <div class="right mbottom">
                <asp:HyperLink ID="hplReturn" runat="server" />
            </div>
            <wl:PagedTemplates ID="PagedTemplateList" runat="server" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>
