using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Taradel.Util;
using System.Data;
using System.Data.SqlClient;




public partial class TemplateBrowser : System.Web.UI.UserControl
{

    public int ProductId = 0;
    public string SelectedTemplateField = "";
    public string SelectedProductName = "";


    //Methods
    protected void Page_Init(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        var oTemplateUtil = new TemplateUtility();
        if (oTemplateUtil.BusinessLineId > 0)
        {
            ddlIndustry.Items.Clear();
            ListItem oBizLineItem = new ListItem("--- ", oTemplateUtil.IndustryId + "-" + oTemplateUtil.BusinessLineId);
            oBizLineItem.Selected = true;
            ddlIndustry.Items.Add(oBizLineItem);

            //-- Hide the filters (they can't be removed from the page, since the jQuery call needs to read the value, but they should not be shown, since there is no category choice for this user)
            pFilter.Style.Add("display", "none");
        } 
        else if (oTemplateUtil.IndustryId > 0)
        {
            //oTemplateUtil.IndustryId > 0
            ddlIndustry.Items.Clear();
            ListItem oBizLineItem = new ListItem("--- ", oTemplateUtil.IndustryId.ToString());
            oBizLineItem.Selected = true;
            ddlIndustry.Items.Add(oBizLineItem);

            //-- Hide the filters (they can't be removed from the page, since the jQuery call needs to read the value, but they should not be shown, since there is no category choice for this user)
            pFilter.Style.Add("display", "none");
        }
        else
        {
            // We can bind industries and product lines

            int templateSizeId = Taradel.WLUtil.GetTemplateSize(ProductId);

            using (TemplateCode.TemplateAPIClient oTemplates = new TemplateCode.TemplateAPIClient())
            {
                TemplateCode.GetIndustriesAndBusinessLinesRequest oRequest = new TemplateCode.GetIndustriesAndBusinessLinesRequest(appxCMS.Util.CMSSettings.GetSiteId(), templateSizeId);
                TemplateCode.GetIndustriesAndBusinessLinesResponse oResponse = oTemplates.GetIndustriesAndBusinessLines(oRequest);
                List<TemplateCode.Industry> oIndustries = oResponse.GetIndustriesAndBusinessLinesResult;
                ddlIndustry.Items.Clear();
                ddlIndustry.Items.Add(new ListItem("Select a Category", ""));
                foreach (TemplateCode.Industry oInd in oIndustries)
                {
                    ListItem oItem = new ListItem(oInd.Name, oInd.IndustryId.ToString());
                    oItem.Attributes.CssStyle.Add("font-weight", "bold");
                    ddlIndustry.Items.Add(oItem);
                    foreach (TemplateCode.BusinessLine oBizLine in oInd.BusinessLines)
                    {
                        ListItem oBizLineItem = new ListItem("--- " + oBizLine.Name, oInd.IndustryId.ToString() + "-" + oBizLine.BusinessLineId.ToString());
                        ddlIndustry.Items.Add(oBizLineItem);
                    }
                }
            }
        }

        litProductName.Text = SelectedProductName;

        var sTemplateServerHost = appxCMS.Util.AppSettings.GetString("TemplateServerHost");

        StringBuilder oJs = new StringBuilder();
        oJs.AppendLine("jQuery(document).ready(function($) {");
        oJs.AppendLine("    var bSearch = false;");
        oJs.AppendLine("    var sSearch = '';");
        oJs.AppendLine("    var prodPages = [];");
        oJs.AppendLine("    var pageCount = 0;");
        oJs.AppendLine("    var iPg = 0;");
        oJs.AppendLine("    var sTemplateBase = '" + sTemplateServerHost + "/templates/';");

        // Get the current product id from the source page
        oJs.AppendLine("    function getProductId() {");
        oJs.AppendLine("        return " + ProductId + ";");
        oJs.AppendLine("    }");

        //Industry search
        oJs.AppendLine("    $('#" + ddlIndustry.ClientID + "').change(function(e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        bSearch = false;");
        oJs.AppendLine("        sSearch = '';");
        oJs.AppendLine("        prodPages = [];");
        oJs.AppendLine("        pageCount = 0;");
        oJs.AppendLine("        iPg = 0;");
        oJs.AppendLine("        loadProductPage(0, true, true);");
        oJs.AppendLine("    });");

        //Keyword Search
        oJs.AppendLine("    $('#" + btnSearch.ClientID + "').click(function(e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        bSearch = true;");
        oJs.AppendLine("        sSearch = $('#" + txtSearchKey.ClientID + "').val();");
        oJs.AppendLine("        prodPages = [];");
        oJs.AppendLine("        pageCount = 0;");
        oJs.AppendLine("        iPg = 0;");
        oJs.AppendLine("        searchProductPage(0, true, true);");
        oJs.AppendLine("    });");

        oJs.AppendLine("    function searchProductPage(page, bNext, bShow) {");
        oJs.AppendLine("        bSearch = true;");
        oJs.AppendLine("        var sPg = 'pg' + page;");
        oJs.AppendLine("        if (prodPages[sPg] == null || prodPages[sPg] == '') {");
        oJs.AppendLine("            var s = '';");
        oJs.AppendLine("            $.getJSON('/resources/SearchProductTemplates.ashx',{id:getProductId(),keyword:sSearch,pageNumber:page},function(msg) {");
        oJs.AppendLine("                if(msg.PageSize) { pageCount = msg.TotalPages; }");
        //oJs.AppendLine("                var bFirst = true;");
        oJs.AppendLine("                var s = '';");
        oJs.AppendLine("                $.each(msg.Templates, function() {");
        oJs.AppendLine("                    s += getItemTemplate(this);");
        oJs.AppendLine("                });");
        oJs.AppendLine("                if (s != '') {");
        oJs.AppendLine("                    prodPages[sPg] = s;");
        oJs.AppendLine("                }");
        oJs.AppendLine("                if (bShow) { $('#itemPlaceholder').html(prodPages[sPg]); iPg = page;loadPageNav(); }");
        oJs.AppendLine("                if (bNext) searchProductPage(page+1, false, false);");
        oJs.AppendLine("            });");
        oJs.AppendLine("        } else {");
        oJs.AppendLine("            if (bShow) { $('#itemPlaceholder').html(prodPages[sPg]); iPg = page;loadPageNav(); }");
        oJs.AppendLine("            if (bNext) searchProductPage(page+1, false, false);");
        oJs.AppendLine("        }");
        oJs.AppendLine("    }");

        oJs.AppendLine("    function loadProductPage(page, bNext, bShow) {");
        oJs.AppendLine("        bSearch = false;");
        oJs.AppendLine("        var sPg = 'pg' + page;");
        oJs.AppendLine("        if (prodPages[sPg] == null || prodPages[sPg] == '') {");
        oJs.AppendLine("            var s = '';");
        if (oTemplateUtil.BusinessLineId > 0)
        {
            oJs.AppendLine("            var sInd = '" + oTemplateUtil.IndustryId + "-" + oTemplateUtil.BusinessLineId +"';");
        }
        else if (oTemplateUtil.IndustryId > 0)
        {
            oJs.AppendLine("            var sInd = '" + oTemplateUtil.IndustryId + "';");
        }
        else
        {
            oJs.AppendLine("            var sInd = $('#" + ddlIndustry.ClientID + "').val();");
        }
        oJs.AppendLine("            $.getJSON('/resources/GetProductTemplates.ashx',{id:getProductId(),industryId:sInd,pageNumber:page},function(msg) {");
        oJs.AppendLine("                if(msg.PageSize) { pageCount = msg.TotalPages; }");
        oJs.AppendLine("                if (msg.Templates.length > 0) {");
        oJs.AppendLine("                    var bFirst = true;");
        oJs.AppendLine("                    var s = '';");
        oJs.AppendLine("                    $.each(msg.Templates, function() {");
        oJs.AppendLine("                        s += getItemTemplate(this);");
        oJs.AppendLine("                    });");
        oJs.AppendLine("                    if (s != '') {");
        oJs.AppendLine("                        prodPages['pg' + page] = s;");
        oJs.AppendLine("                    }");
        oJs.AppendLine("                }");
        oJs.AppendLine("                if (bShow) { $('#itemPlaceholder').html(prodPages[sPg]); iPg = page;loadPageNav(); }");
        oJs.AppendLine("                if (bNext) loadProductPage(page+1, false, false);");
        oJs.AppendLine("            });");
        oJs.AppendLine("        } else {");
        oJs.AppendLine("            if (bShow) { $('#itemPlaceholder').html(prodPages[sPg]); iPg = page;loadPageNav(); }");
        oJs.AppendLine("            if (bNext) loadProductPage(page+1, false, false);");
        oJs.AppendLine("        }");
        oJs.AppendLine("    }");

        oJs.AppendLine("    function getItemTemplate(oTemplate) {");
        oJs.AppendLine("        var sFront = sTemplateBase + 'icon/' + oTemplate.FrontImage;");
        oJs.AppendLine("        var sBack = '';");
        oJs.AppendLine("        if (oTemplate.BackImage != null && oTemplate.BackImage != '') sBack = sTemplateBase + 'icon/' + oTemplate.BackImage;");
        oJs.AppendLine("        var sInside = '';");
        oJs.AppendLine("        if (oTemplate.InsideImage != null && oTemplate.InsideImage != '') sInside = sTemplateBase + 'icon/' + oTemplate.InsideImage;");
        oJs.AppendLine("        var html = '';");
        oJs.AppendLine("        html += '<div class=\"col-md-4\">';");
        oJs.AppendLine("        html += '   <div class=\"templateWrapper\">';");
        oJs.AppendLine("        html += '       <div class=\"text-center\">';");
        oJs.AppendLine("        html += '           <img src=\"' + sFront + '\" class=\"img-thumbnail img-responive\" />';");
        oJs.AppendLine("        if (sBack != '') {");
        oJs.AppendLine("            html += '           <img src=\"' + sBack + '\" class=\"img-thumbnail img-responive\" />';");
        oJs.AppendLine("        }");
        oJs.AppendLine("        html += '       </div>';");
        oJs.AppendLine("        html += '       <div class=\"text-center templateImgWrapper\">';");
        oJs.AppendLine("        if (oTemplate.Name != null && oTemplate.Name != '') {");
        oJs.AppendLine("            html += '           <strong><em>' + oTemplate.Name + '</em></strong><br />';");
        oJs.AppendLine("        }");
        oJs.AppendLine("        html += '           Template #: ' + oTemplate.TemplateId + '<br />';");
        if (oTemplateUtil.IndustryId == 0)
        {
            oJs.AppendLine("        html += '           ' + oTemplate.BusinessLine.Name + '<br/>';");    
        }
        oJs.AppendLine("        html += '       </div>';");
        oJs.AppendLine("        html += '       <div class=\"text-center\">';");
        oJs.AppendLine("        html += '           <a data-action=\"viewTemplate\" data-toggle=\"modal\" data-target=\"#modalPreview\" class=\"btn btn-primary btn-xs\" data-templatename=\"' + oTemplate.Name + '\" data-templateid=\"' + oTemplate.TemplateId + '\" data-frontimg=\"' + sFront + '\" data-insideimg=\"' + sInside + '\" data-backimg=\"' + sBack + '\" data-bl=\"' + oTemplate.BusinessLine.Name + '\" data-pagesize=\"' + oTemplate.PageSize + '\"><span class=\"glyphicon glyphicon-zoom-in\"></span>&nbsp;View Template</a>';");
        oJs.AppendLine("        html += '           <a data-action=\"selectTemplate\" class=\"btn btn-primary btn-xs\" data-templatename=\"' + oTemplate.Name + '\" data-templateid=\"' + oTemplate.TemplateId + '\" data-frontimg=\"' + sFront + '\" data-insideimg=\"' + sInside + '\" data-backimg=\"' + sBack + '\" data-bl=\"' + oTemplate.BusinessLine.Name + '\" data-pagesize=\"' + oTemplate.PageSize + '\"><span class=\"glyphicon glyphicon-ok\"></span>&nbsp;Select Template</a>';");
        oJs.AppendLine("        html += '       </div>';");
        oJs.AppendLine("        html += '   </div>';");
        oJs.AppendLine("        html += '</div>';");
        oJs.AppendLine("        return html;");
        oJs.AppendLine("    }");
       
        oJs.AppendLine("    function loadPageNav() {");
        oJs.AppendLine("        $('#curPage').html(iPg+1);");
        oJs.AppendLine("        $('#totalPages').html(pageCount);");
        oJs.AppendLine("        if (iPg == 0) {");
        oJs.AppendLine("            $('#" + lnkPreviousTemplatePage.ClientID + "').closest('li').addClass('disabled');");
        oJs.AppendLine("        } else {");
        oJs.AppendLine("            $('#" + lnkPreviousTemplatePage.ClientID + "').closest('li').removeClass('disabled');");
        oJs.AppendLine("        }");
        oJs.AppendLine("        if (iPg == pageCount-1) {");
        oJs.AppendLine("            $('#" + lnkNextTemplatePage.ClientID + "').closest('li').addClass('disabled');");
        oJs.AppendLine("        } else {");
        oJs.AppendLine("            $('#" + lnkNextTemplatePage.ClientID + "').closest('li').removeClass('disabled');");
        oJs.AppendLine("        }");
        oJs.AppendLine("    }");
        oJs.AppendLine("    $('#" + lnkPreviousTemplatePage.ClientID + "').click(function(e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        if (!$(this).closest('li').hasClass('disabled')) {");
        oJs.AppendLine("            if (bSearch) {");
        oJs.AppendLine("                searchProductPage(iPg-1, false, true);");
        oJs.AppendLine("            } else {");
        oJs.AppendLine("                loadProductPage(iPg-1, false, true);");
        oJs.AppendLine("            }");
        oJs.AppendLine("        }");
        oJs.AppendLine("    });");
        oJs.AppendLine("    $('#" + lnkNextTemplatePage.ClientID + "').click(function(e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        if (!$(this).closest('li').hasClass('disabled')) {");
        oJs.AppendLine("            if (bSearch) {");
        oJs.AppendLine("                searchProductPage(iPg+1, false, true);");
        oJs.AppendLine("            } else {");
        oJs.AppendLine("                loadProductPage(iPg+1, true, true);");
        oJs.AppendLine("            }");
        oJs.AppendLine("        }");
        oJs.AppendLine("    });");

        //-- Show a default sample of templates
        if (oTemplateUtil.BusinessLineId > 0 || oTemplateUtil.IndustryId > 0)
        {
            oJs.AppendLine("    loadProductPage(0, false, true);");    
        }
        else
        {
            oJs.AppendLine("    searchProductPage(0, false, true);");            
        }
        


        // Preview template button
        oJs.AppendLine("    $('body').on('click', 'a[data-action=viewTemplate]', function(e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        //grab TemplateID, template name, and file name.");
        oJs.AppendLine("        var templateID = $(this).attr('data-templateid');");
        oJs.AppendLine("        var templateName = $(this).attr('data-templatename');");
        oJs.AppendLine("        var fileName = $(this).attr('data-frontimg');");
        oJs.AppendLine("        var businessLineName = $(this).attr('data-bl');");
        oJs.AppendLine("        var backside = $(this).attr('data-backimg');");
        oJs.AppendLine("        //Update title");
        oJs.AppendLine("        $('#lblTemplateName').html(templateName + ' #' + templateID);");
        oJs.AppendLine("        //set category label");
        oJs.AppendLine("        $('#lblPreviewCategory').html(businessLineName);");
        oJs.AppendLine("        //Update the description panel ");
        oJs.AppendLine("        $('#litTemplateName').html(templateName);");
        oJs.AppendLine("        //update large image ");
        oJs.AppendLine("        $('#imgTemplatePreview').attr('src', fileName.replace('/icon/', '/full/'));");
        oJs.AppendLine("        //disable View Back Side Button if no back image is detected.");
        oJs.AppendLine("        if (backside == '' || backside == null) {");
        oJs.AppendLine("            $('#btnViewFront').hide();");
        oJs.AppendLine("            $('#btnViewBack').hide();");
        oJs.AppendLine("        } else {");
        oJs.AppendLine("            $('#btnViewFront').show();");
        oJs.AppendLine("            $('#btnViewBack').show();");
        oJs.AppendLine("        }");
        oJs.AppendLine("        //update the Select And Continue button");
        oJs.AppendLine("        $('#btnPreviewSelected').attr('data-thumbnail', fileName);");
        oJs.AppendLine("        $('#btnPreviewSelected').attr('data-templateID', templateID);");
        oJs.AppendLine("        $('#btnPreviewSelected').attr('data-templateName', templateName);");
        oJs.AppendLine("        //Dismiss the Browser Modal");
        oJs.AppendLine("        $('#modalTemplates').modal('toggle');");
        oJs.AppendLine("    });");

        // Select template from Browser Window
        oJs.AppendLine("    $('body').on('click', 'a[data-action=selectTemplate]', function(e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        var templateID = $(this).attr('data-templateid');");
        oJs.AppendLine("        var fileName = $(this).attr('data-frontimg');");
        oJs.AppendLine("        var templateName = $(this).attr('data-templatename');");
        oJs.AppendLine("        ApplySelection(templateID, fileName, templateName);");
        oJs.AppendLine("    });");

        // Select template from preview window
        oJs.AppendLine("    $('a[data-action=previewSelected]').click(function (e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        //grab the TemplateID, file name, and template name");
        oJs.AppendLine("        var templateID = $(this).attr('data-templateid');");
        oJs.AppendLine("        var fileName = $(this).attr('data-thumbnail');");
        oJs.AppendLine("        var templateName = $(this).attr('data-templatename');");
        oJs.AppendLine("        ApplySelection(templateID, fileName, templateName);");
        oJs.AppendLine("    });");

        oJs.AppendLine("    function ApplySelection(templateID, fileName, templateName) {");
        oJs.AppendLine("        if (templateName == null) templateName = '';");
        oJs.AppendLine("        //hidden fields. Located on Container page. Update hidden textbox too for postbacks....");
        oJs.AppendLine("        $('#hidSelectedTemplateID').val(templateID);");
        oJs.AppendLine("        $('#txtSelectedTemplateID').val(templateID);");
        oJs.AppendLine("        $('#hidSelectedTemplateFileName').val(fileName);");
        oJs.AppendLine("        $('#txtSelectedTemplateFileName').val(fileName);");
        oJs.AppendLine("        $('#hidSelectedTemplateName').val(templateName);");
        oJs.AppendLine("        $('#txtSelectedTemplateName').val(templateName);");
        oJs.AppendLine("        //change the thumbnail");
        oJs.AppendLine("        $('#imgSelectedTemplate').attr('src', fileName);");
        oJs.AppendLine("        //Show the good choice message and complete it.");
        oJs.AppendLine("        $('#lblYouHaveSelected').html('You have selected <strong>' + templateName + ' (#' + templateID + ') </strong>.');");
        oJs.AppendLine("        $('#goodChoiceFreeDesignBlock').removeAttr('class');");
        oJs.AppendLine("        $('#goodChoiceFreeDesignBlock').attr('class', 'alert alert-success');");
        oJs.AppendLine("        $('#goodChoiceFreeDesignBlock').show();");
        oJs.AppendLine("        //make sure the correct design choice is selected.");
        oJs.AppendLine("        $('#freeTemplateBlock').show(1000);");
        oJs.AppendLine("        //hide other blocks");
        oJs.AppendLine("        $('#myDesignBlock').hide(1000);");
        oJs.AppendLine("        $('#professionalDesignBlock').hide(1000);");
        oJs.AppendLine("        //hide the Not Yet Selected block");
        oJs.AppendLine("        $('#notSelectedTemplateBlock').hide();");
        oJs.AppendLine("        //hide the template modal and this preview modal");
        oJs.AppendLine("        $('#modalTemplates').modal('hide');");
        oJs.AppendLine("        $('#modalPreview').modal('hide');");
        oJs.AppendLine("        //Unlock the button");
        oJs.AppendLine("        $('#btnContinue').removeAttr('class');");
        oJs.AppendLine("        $('#btnContinue').attr('class', 'btn btn-danger btn-lg pull-right');");
        oJs.AppendLine("        //Recalc the default/visible correct Launch Week");
        oJs.AppendLine("        DesignChanged();");
        oJs.AppendLine("    }");

        // View Front Button Click
        oJs.AppendLine("    $('button[data-action=viewFront]').click(function (e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        var fileName = $('#imgTemplatePreview').attr('src');");
        oJs.AppendLine("        var newFileName = fileName.replace('back', 'front');");
        oJs.AppendLine("        $('#imgTemplatePreview').attr('src', newFileName);");
        oJs.AppendLine("    });");

        // View Back Button Click
        oJs.AppendLine("    $('button[data-action=viewBack]').click(function (e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        var fileName = $('#imgTemplatePreview').attr('src');");
        oJs.AppendLine("        var newFileName = fileName.replace('front', 'back');");
        oJs.AppendLine("        $('#imgTemplatePreview').attr('src', newFileName);");
        oJs.AppendLine("    });");

        // Cancel from Preview and return to browse
        oJs.AppendLine("    $('a[data-action=cancelGoBack]').click(function (e) {");
        oJs.AppendLine("        e.preventDefault();");
        oJs.AppendLine("        $('#modalPreview').modal('hide');");
        oJs.AppendLine("        $('#modalTemplates').modal('show');");
        oJs.AppendLine("    });");

        oJs.AppendLine("});");

        jqueryHelper.RegisterStartupScript(Page, "TemplateBrowserAjaxInit", oJs.ToString());
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();

        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);

        if (siteDetails.HideTaradelContent)
        { phNoTaradelDesignerMessage.Visible = true; }
        else
        { phGeneralDesignerMessage.Visible = true; }

    }

}