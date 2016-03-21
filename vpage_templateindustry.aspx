<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/OneColumn.master" AutoEventWireup="false" CodeFile="vpage_templateindustry.aspx.vb" Inherits="vpage_templateindustry" %>
<%@ Register TagPrefix="wl" TagName="TemplateBusinessLines" Src="~/CCustom/TemplateBusinessLines.ascx" %>
<%@ Register TagPrefix="wl" TagName="TemplateSearch" Src="~/CCustom/TemplateSearch.ascx" %>
<%@ Register TagPrefix="wl" TagName="PagedTemplates" Src="~/CCustom/TemplatePagedList.ascx" %>
<%@ Register TagPrefix="wl" TagName="TemplateIndustriesDD" Src="~/CCustom/TemplateIndustriesDropDown.ascx" %>


<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
       
    <div class="container">

        <div class="partialRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            <span class="subRibbonPop">Templates</span>
            <span class="subRibbon"><asp:Literal ID="litBusinessLine" runat="server" /></span>
        </div>

        <div class="partialRibbonSmall visible-sm visible-xs">
            <span class="subRibbonPopSmall">Templates</span>
            <span class="subRibbonSmall"><asp:Literal ID="litBusinessLineSmall" runat="server" /></span>
        </div>

        <div class="contentWrapper">

            <div class="row">

                <div class="col-sm-3">
                    <wl:TemplateSearch ID="FindTemplates" runat="server" />
                    <wl:TemplateBusinessLines ID="TemplateBusinessLines" runat="server" IntroText="Filter By" FullText="Business Type" />
                    <wl:TemplateIndustriesDD ID="TemplateIndustries" runat="server" />
                </div>

                <div class="col-sm-9">

                    <%--Uncomment to see what pages use this template.--%>
                    <%--<h2>This page is using vpage_templateindustry.</h2>--%>

                    <wl:PagedTemplates ID="PagedTemplateList" runat="server" />

                </div>

            </div>


        </div>

    </div>




</asp:Content>
