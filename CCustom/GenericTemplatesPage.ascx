<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenericTemplatesPage.ascx.cs" Inherits="GenericDesignPage" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="uc1" TagName="PageHeader" %>
<%@ Register Src="~/CCustom/TemplateSearch.ascx" TagPrefix="uc1" TagName="TemplateSearch" %>
<%@ Register Src="~/CCustom/TemplateIndustriesDropDown.ascx" TagPrefix="uc1" TagName="TemplateIndustriesDropDown" %>
<%@ Register Src="~/CCustom/TemplateCoverflow.ascx" TagPrefix="uc1" TagName="TemplateCoverflow" %>





<div class="container">

    
      <uc1:PageHeader runat="server" id="PageHeader" />
    <div class="contentWrapper">
        <div class="row">

            <div class="col-sm-3">

                <uc1:TemplateSearch runat="server" id="TemplateSearch" />
                <uc1:TemplateIndustriesDropDown runat="server" id="TemplateIndustriesDropDown" />
            </div>

            <div class="col-sm-9">
                
                <uc1:TemplateCoverflow runat="server" id="TemplateCoverflow" />
                <p class="text-center">
                    <img class="img-responsive" title="Free EDDM Templates" alt="Free EDDM Templates" src="/assets/images/free-templates_msg.jpg" />
                </p>

            </div>
        </div>
    </div>
</div>






