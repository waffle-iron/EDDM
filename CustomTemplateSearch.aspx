<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="CustomTemplateSearch.aspx.vb" Inherits="CustomTemplateSearch" %>
<%@ Register TagPrefix="template" TagName="Search" Src="~/CCustom/TemplateSearch.ascx" %>
<%@ Register TagPrefix="template" TagName="SearchPagedList" Src="~/CCustom/TemplateSearchPagedList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">

    <div class="container">
                 
        <div class="partialRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            <span class="subRibbonPop">Templates</span>
            <span class="subRibbon">Select a pre-made template</span>
        </div>          

        <div class="partialRibbonSmall visible-sm visible-xs">
            <span class="subRibbonPopSmall">Templates</span>
            <span class="subRibbonSmall">Select a pre-made template</span>
        </div>          

        <div class="contentWrapper">
                         
            <h2 class="text-center">All the templates shown below are easily customized just for you!</h2>

            <div>&nbsp;</div>

            <div class="row">                 
                <div class="col-sm-3">
                    <div>&nbsp;</div>
                    <div>&nbsp;</div>
                    <template:search id="Search" runat="server" SearchUrl="~/CustomTemplateSearch.aspx" />
                </div>                 


                <div class="col-sm-9">
                    
                    <template:SearchPagedList id="PagedList" runat="server" />
                   
                </div>

            </div>

        </div>

    </div>

</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>
