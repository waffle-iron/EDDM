<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/OneColumn.master" AutoEventWireup="false" CodeFile="TemplateSearchResults.aspx.vb" Inherits="TemplateSearchResults" %>
<%@ Register TagPrefix="wl" TagName="TemplateSizes" Src="~/CCustom/TemplateSizes.ascx" %>
<%@ Register TagPrefix="wl" TagName="PagedTemplates" Src="~/CCustom/TemplateSearchPagedList.ascx" %>
<%@ Register TagPrefix="wl" TagName="TemplateIndustriesDD" Src="~/CCustom/TemplateIndustriesDropDown.ascx" %>
<%@ Register TagPrefix="wl" TagName="TemplateSearch" Src="~/CCustom/TemplateSearch.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">

    <div class="container">

        <div class="fullRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            Templates
            <span class="arrowRight"></span>
        </div>        

        <div class="fullRibbonSmall visible-sm visible-xs">
            Templates
        </div>        

        <div class="contentWrapper">

            <div class="row">

                <div class="col-sm-3">
                    <wl:TemplateSearch ID="FindTemplates" runat="server" />
                    <wl:TemplateIndustriesDD ID="TemplateIndustries" runat="server" />
                </div>

                <div class="col-sm-9">

                    <wl:PagedTemplates ID="PagedTemplateList" runat="server" />

                </div>

            </div>


        </div>

    </div>

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>
