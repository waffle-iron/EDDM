<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenericDesignPage.ascx.cs" Inherits="GenericDesignPage" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="uc1" TagName="PageHeader" %>
<%@ Register Src="~/CCustom/Banner-BrowseTemplates.ascx" TagPrefix="uc1" TagName="BannerBrowseTemplates" %>




<div class="container">
  <uc1:PageHeader runat="server" id="PageHeader" />
  <div class="contentWrapper">
    <div class="row equal">
      <div class="col-sm-4">
        <div class="panel panel-primary">
          <div class="panel-heading">Professional Design Service</div>
          <div class="panel-body">
            <p><img alt="" src="/cmsimages/1/design-help.jpg" class="img-responsive" /></p>
            <p> Professional design service is available with any order, at extremely competitive rates. Get the most out of your direct mail campaign by working with veteran design specialists. The design of your products/services/offers is the most important factor in generating a reponse. If you prefer to leave the creative process to experienced professionals, this option may be right for you. </p>
            <p><a href="/Professional-Design-Service" target="_self">Learn more....</a></p>
            <p>&nbsp;</p>
          </div>
          <div class="panel-footer">
            <p class="text-center"><a href="/Resources/SelectionSetting.ashx?name=DesignOption&value=My&ReturnUrl=/Step1-Target.aspx" class="btn btn-cta btn-shadow btn-lg"> <span class="fa fa-check"></span>&nbsp;GET STARTED </a> </p>
          </div>
        </div>
      </div>
      <div class="col-sm-4">
        <div class="panel panel-primary">
          <div class="panel-heading">FREE Templates</div>
          <div class="panel-body">
            <p><img alt="" src="/cmsimages/1/thousands-templates.jpg" class="img-responsive" /></p>
            <p> FREE professionally-designed templates are available with any order. Choose a template, place your order, and one of our professional graphic designers will contact you within 1-2 business days. From there, we will customize the message and/or offers on your selected template to meet your specific requirements. </p>
          </div>
          <div class="panel-footer">
            <p class="text-center"> <a href="/Templates" class="btn btn-cta btn-shadow btn-lg"> <span class="fa fa-check"></span>&nbsp;GET STARTED </a> </p>
          </div>
        </div>
      </div>
      <div class="col-sm-4">
        <div class="panel panel-primary">
          <div class="panel-heading">My Design</div>
          <div class="panel-body">
            <p><img alt="" src="/cmsimages/1/upload_img2.jpg" class="img-responsive" /></p>
            <p> If you have print-ready artwork in high-resolution (.PDF) format, simply upload your files directly to your order. This option is perfect for those who have design experience, or, prefer to use a third party agency to handle the creative process. </p>
            <p><a href="/My-Design" target="_self">Learn more....</a></p>
            <p> <strong>Are You a Designer?</strong><br />
              <a href="/Design-Specs" target="_self">Download our EDDM Spec Templates</a> </p>
            <p>&nbsp;</p>
          </div>
          <div class="panel-footer">
            <p class="text-center"> <a href="/Resources/SelectionSetting.ashx?name=DesignOption&value=My&ReturnUrl=/Step1-Target.aspx" class="btn btn-cta btn-shadow btn-lg"> <span class="fa fa-check"></span>&nbsp;GET STARTED </a> </p>
          </div>
        </div>
      </div>
    </div>

              <uc1:BannerBrowseTemplates runat="server" id="BannerBrowseTemplates" />
  </div>
</div>
