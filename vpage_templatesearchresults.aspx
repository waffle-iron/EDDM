<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/OneColumn.master" AutoEventWireup="false" CodeFile="vpage_templatesearchresults.aspx.vb" Inherits="vpage_templatesearchresults" %>

<%@ Register TagPrefix="wl" TagName="BannerFull" Src="~/CCustom/BannerFull.ascx" %>
<%@ Register TagPrefix="wl" TagName="TemplateSizes" Src="~/CCustom/TemplateSizes.ascx" %>
<%@ Register TagPrefix="wl" TagName="PagedTemplates" Src="~/CCustom/TemplatePagedList.ascx" %>
<%@ Register TagPrefix="wl" TagName="TemplateIndustries" Src="~/CCustom/TemplateIndustries.ascx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="phBody" Runat="Server">
<wl:BannerFull ID="BannerFull" runat="server" IntroText="Templates" />
    <div class="ui-helper-clearfix mtop">
        <div class="thirty fleft">
            <div class="padright">
                <div class="formBlock mbottom">
                    <b>Search for Templates</b>
                    <div class="stackrow">
                        <div class="label">
                            Keyword:</div>
                        <div class="aright full">
                            <asp:TextBox ID="Keyword" runat="server" /></div>
                    </div>
                    <div class="makeButtonPane">
                        <asp:Button ID="btnSearch" runat="server" Text="Find Templates" />
                    </div>
                </div>
                <wl:TemplateIndustries ID="TemplateIndustries" runat="server" />
            </div>
        </div>
        <div class="seventy fleft">
            <%--<wl:PagedTemplates ID="PagedTemplateList" runat="server" />--%>
        </div>
    </div>
</asp:Content>
