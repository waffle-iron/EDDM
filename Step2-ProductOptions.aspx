<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" Trace="false" AutoEventWireup="false" CodeFile="Step2-ProductOptions.aspx.vb" Inherits="Step2_ProductOptions" EnableEventValidation="False" Debug="True" %>
<%@ Register Src="~/Controls/TemplateBrowser.ascx" TagPrefix="appx" TagName="TemplateBrowser" %>
<%@ Register Src="~/Controls/CampaignOverview.ascx" TagPrefix="appx" TagName="CampaignOverview" %>
<%@ Register Src="~/Controls/DropDate.ascx" TagPrefix="appx" TagName="DropDate" %>
<%@ Register Src="~/Controls/USAStatesDropDown.ascx" TagPrefix="appx" TagName="USAStatesDropDown" %>
<%@ Register Src="~/Controls/OrderSteps.ascx" TagPrefix="appx" TagName="OrderSteps" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="eddm" TagName="PageHeader" %>




<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

    <div class="container">

        <eddm:PageHeader runat="server" id="PageHeader" />

        <div class="contentWrapper">

            <%--Order Steps--%>
            <appx:OrderSteps runat="server" id="OrderSteps" />

            <div>&nbsp;</div>

            <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
                <i class="fa fa-2x fa-exclamation-circle pull-left"></i>&nbsp;
                <asp:Literal ID="litErrorMessage" runat="server" />
                <p>&nbsp;</p>
            </asp:Panel>


            <asp:Panel ID="pnlNormal" runat="server" Visible="True">


                <%--Debug--%>
                <asp:Panel ID="pnlDebug" runat="server" CssClass="hidden">

                    <h3>TEST MODE</h3>

                    <p><strong>Hidden TextBox Controls and Hidden Fields</strong></p>

                    <table class="table table-bordered table-striped table-condensed table-test-mode">
                        <thead>
                            <tr>
                                <th>TextBox Controls</th>
                                <th>&nbsp;</th>
                                <th>Hidden Fields</th>
                            </tr>
                        </thead>

                        <tbody>

                            <tr>
                                <td class="width33">AddressedSelected</td>
                                <td class="width33"><asp:TextBox ID="txtAddressedSelected" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidAddressedSelected" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">BaseProductID</td>
                                <td class="width33"><asp:TextBox ID="txtBaseProductID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidBaseProductID" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">DesignFee</td>
                                <td class="width33"><small>(no visible textbox)</small></td>
                                <td class="width33"><asp:HiddenField ID="hidDesignFee" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">DistributionID</td>
                                <td class="width33"><asp:TextBox ID="txtDistributionID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidDistributionID" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">EDDM BaseProductID</td>
                                <td class="width33"><asp:TextBox ID="txtEddmBaseProdID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidEddmBaseProdID" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">EDDM ProductID</td>
                                <td class="width33"><asp:TextBox ID="txtEddmProdID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidEddmProdID" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">EDDMSelected</td>
                                <td class="width33"><asp:TextBox ID="txtEDDMSelected" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidEDDMSelected" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">ExclusiveQualify</td>
                                <td class="width33"><small>(no visible textbox)</small></td>
                                <td class="width33"><asp:HiddenField ID="hidExclusiveQualify" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">ExclusiveDoesNotQualify</td>
                                <td class="width33"><small>(no visible textbox)</small></td>
                                <td class="width33"><asp:HiddenField ID="hidExclusiveDoesNotQualify" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">ExclusiveNeedsMore</td>
                                <td class="width33"><small>(no visible textbox)</small></td>
                                <td class="width33"><asp:HiddenField ID="hidExclusiveNeedsMore" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">HideSplitDrops</td>
                                <td class="width33"><asp:TextBox ID="txtHideSplitDrops" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidHideSplitDrops" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">HoldQuantity</td>
                                <td class="width33"><small>(no visible textbox)</small></td>
                                <td class="width33"><asp:HiddenField ID="hidHoldQuantity" runat="server" ClientIDMode="Static" Value="0" /></td>
                            </tr>

                            <tr>
                                <td class="width33">JobName</td>
                                <td class="width33"><small>(no visible textbox)</small></td>
                                <td class="width33"><asp:HiddenField ID="hidJobName" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">Launch Week</td>
                                <td class="width33"><asp:TextBox ID="txtLaunchWeek" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidLaunchWeek" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">MarkUp</td>
                                <td class="width33"><asp:TextBox ID="txtMarkUp" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidMarkUp" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">MarkUpType</td>
                                <td class="width33"><asp:TextBox ID="txtMarkUpType" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidMarkUpType" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">MinOrderQty</td>
                                <td class="width33"><asp:TextBox ID="txtMinimumToOrder" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hdnMinimumToOrder" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">MinEDDMPricingQty</td>
                                <td class="width33"><asp:TextBox ID="txtMinEDDMPricingQty" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidMinEDDMPricingQty" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">MinAddressedPricingQty</td>
                                <td class="width33"><asp:TextBox ID="txtMinAddressedPricingQty" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidMinAddressedPricingQty" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">MinimumQtyExclusive</td>
                                <td class="width33"><asp:TextBox ID="txtMinimumQtyExclusive" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidMinimumQtyExclusive" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">NoMultipleFee</td>
                                <td class="width33"><small>(no visible textbox)</small></td>
                                <td class="width33"><asp:HiddenField ID="hidNoMultipleFee" runat="server" ClientIDMode="Static" Value="" /></td>
                            </tr>

                            <tr>
                                <td class="width33">NumImpressionsForExclusive</td>
                                <td class="width33"><asp:TextBox ID="txtMinimumImpressionExclusive" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidMinimumImpressionExclusive" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">OffersExclusiveRoutes</td>
                                <td class="width33"><asp:TextBox ID="txtExclusiveSite" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidExclusiveSite" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">Postage Rate</td>
                                <td class="width33"><asp:TextBox ID="txtPostageRate" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidPostageRate" runat="server" Value="0" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">PrevSelectedTemplateID (from cookie)</td>
                                <td class="width33"><asp:TextBox ID="txtPrevSelectedTemplateID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidPrevSelectedTemplateID" runat="server" Value="0" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">ProductID</td>
                                <td class="width33"><asp:TextBox ID="txtProductID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidProductID" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">Product Name</td>
                                <td class="width33"><asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidProductName" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">SelectedTemplateFileName</td>
                                <td class="width33"><asp:TextBox ID="txtSelectedTemplateFileName" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidSelectedTemplateFileName" runat="server" Value="0" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">SelectedTemplateID</td>
                                <td class="width33"><asp:TextBox ID="txtSelectedTemplateID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidSelectedTemplateID" runat="server" Value="0" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">SelectedTemplateName</td>
                                <td class="width33"><asp:TextBox ID="txtSelectedTemplateName" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidSelectedTemplateName" runat="server" Value="0" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">TemplateSizeID</td>
                                <td class="width33"><asp:TextBox ID="txtTemplateSizeID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidTemplateSizeID" runat="server" Value="0" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">TotalSelected</td>
                                <td class="width33"><asp:TextBox ID="txtTotalSelected" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidTotalSelected" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="width33">USelectID</td>
                                <td class="width33"><asp:TextBox ID="txtUSelectID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                                <td class="width33"><asp:HiddenField ID="hidUSelectID" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                        </tbody>

                    </table>

                    <p>&nbsp;</p>

                    <p><strong>Page Properties</strong></p>

                    <table class="table table-bordered table-striped table-condensed table-test-mode">
                        <thead>
                            <tr>
                                <th>Property</th>
                                <th>Value</th>
                            </tr>
                        </thead>

                        <tbody>

                            <tr>
                                <td>Addressed</td>
                                <td><asp:Literal ID="litDebugAddressed" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>AddressedSelected</td>
                                <td><asp:Literal ID="litDebugAddressedSelected" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>AllowMultipleImpressions</td>
                                <td><asp:Literal ID="litAllowMultipleImpressions" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>AllowSplitDrops</td>
                                <td><asp:Literal ID="litAllowSplitDrops" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>AllowUpsell</td>
                                <td><asp:Literal ID="litAllowUpsell" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>BaseProductID</td>
                                <td><asp:Literal ID="litDebugbaseProductID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>CampaignOverviewDisplayDelay</td>
                                <td><asp:Literal ID="litCampaignOverviewDisplayDelay" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>CategoryID</td>
                                <td><asp:Literal ID="litDebugCategoryID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>DesignFee</td>
                                <td><asp:Literal ID="litDebugDesignFee" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>DistributionID</td>
                                <td><asp:Literal ID="litDebugDistID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>DisableTemplates</td>
                                <td><asp:Literal ID="litDisableTemplates" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>DisableProDesign</td>
                                <td><asp:Literal ID="litDisableProDesign" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>DropFeeRate</td>
                                <td><asp:Literal ID="litDropFeeRate" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>EDDM</td>
                                <td><asp:Literal ID="litDebugEDDM" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>EDDMSelected</td>
                                <td><asp:Literal ID="litDebugEDDMSelected" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MarkUp</td>
                                <td><asp:Literal ID="litDebugMarkUp" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MarkUpType</td>
                                <td><asp:Literal ID="litDebugMarkUpType" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MaxDrops</td>
                                <td><asp:Literal ID="litDebugMaxDrops" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MaxQty</td>
                                <td><asp:Literal ID="litDebugMaxQty" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MinQty</td>
                                <td><asp:Literal ID="litDebugMinQty" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MinOrderQty</td>
                                <td><asp:Literal ID="litDebugMinOrderQty" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MinEDDMPricingQty</td>
                                <td><asp:Literal ID="litDebugMinEDDMPricingQty" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MinAddressedPricingQty</td>
                                <td><asp:Literal ID="litDebugMinAddressedPricingQty" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>MinPrintQty</td>
                                <td><asp:Literal ID="litDebugMinPrintQty" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>PageCount</td>
                                <td><asp:Literal ID="litDebugPageCount" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>PaperHeight</td>
                                <td><asp:Literal ID="litDebugPaperHeight" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>PaperWidth</td>
                                <td><asp:Literal ID="litDebugPaperWidth" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>PostageRate</td>
                                <td><asp:Literal ID="litDebugPostageRate" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>ProductID</td>
                                <td><asp:Literal ID="litDebugProductID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>ProductName</td>
                                <td><asp:Literal ID="litDebugProductName" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>ReferenceID</td>
                                <td><asp:Literal ID="litDebugReferenceID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>SKU</td>
                                <td><asp:Literal ID="litDebugSKU" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>TestMode</td>
                                <td><asp:Literal ID="litTestMode" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>TotalSelected</td>
                                <td><asp:Literal ID="litDebugTotalSelected" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>UploadedList</td>
                                <td><asp:Literal ID="litDebugUploadedList" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>USelectID</td>
                                <td><asp:Literal ID="litDebugUSelectID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td>ValidateExtraCopiesAddress</td>
                                <td><asp:Literal ID="litValidateExtraCopiesAddress" runat="server" /></td>
                            </tr>

                        </tbody>

                    </table>


                    <p><strong>Products in CategoryID (USelect configured):</strong></p>
                    <asp:GridView ID="gvProductsInCategory" runat="server" BackColor="White">
                    </asp:GridView>                    

                    <p>&nbsp;</p>

                    <p><strong>Original Products:</strong></p>
                    <asp:GridView ID="gvOrigProducts" runat="server" Width="90%" Font-Size="8px" BackColor="White">
                    </asp:GridView>

                    <p>&nbsp;</p>

                    <p><strong>Test Products:</strong></p>
                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlTestProducts" ClientIDMode="Static" AutoPostBack="false">
                    </asp:DropDownList>

                    <p>&nbsp;</p>

                    <p><strong>Product Options:</strong></p>
                    <asp:GridView ID="gvProdOptions" runat="server" BackColor="White">
                    </asp:GridView>
                    <asp:Label ID="lblProdOptions" runat="server"></asp:Label>

                    <p>&nbsp;</p>

                </asp:Panel>


                <%--Entire Selection Process--%>
                <div class="row">
               
                    <%--Design and Delivery Options--%>
                    <div class="col-sm-8">

                        <%--Design Options--%>
                        <div class="panel panel-primary" id="designOptionsBlock">

                            <div class="panel-heading">Design Options</div>

                            <div class="panel-body">

                                <p class="lead"><span class="leadDropWord">First,</span> let's define your Design Options.</p>
                              
                                <asp:ValidationSummary ID="vsOrder" CssClass="text-danger" runat="server" HeaderText="Please correct the following to proceed:" ValidationGroup="vgOrder" />

                                <div class="form-horizontal" role="form">

                                    <%--Product--%>
                                    <div class="form-group">
                                        <label for="ddlProduct" class="col-sm-4 control-label">Selected Product</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlProduct" ClientIDMode="Static" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3 text-center">
                                            <small>
                                                <asp:LinkButton ID="btnAboutProduct" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-action="infowindow" ClientIDMode="Static">
                                                <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                </asp:LinkButton>
                                            </small>
                                        </div>
                                    </div>
                                
                                    <%--Total Deliveries--%>
                                    <div class="form-group">
                                        <label for="lblTotalDeliveries" class="col-sm-4 control-label">Total Deliveries</label>
                                        <div class="col-sm-5">
                                            <label class="control-label"><asp:Label ID="lblTotalDeliveries" runat="server" Font-Bold="True" ClientIDMode="Static" /></label>
                                        </div>
                                        <div class="col-sm-3 text-center">
                                            <small>
                                                <asp:LinkButton ID="btnTotalDeliveries" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="About Total Deliveries" data-action="infowindow" data-helpfile="/helpTotalDeliveries">
                                                <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                </asp:LinkButton>
                                            </small>
                                        </div>
                                    </div>

                                    <%--Extra Copies--%>
                                    <asp:Panel ID="pnlExtraCopies" runat="server" Visible="false">
                                        <div class="form-group">
                                            <label for="txtExtraCopies" class="col-sm-4 control-label">Extra Copies</label>
                                            <div class="col-sm-5">
                                                <asp:TextBox ID="txtExtraCopies" runat="server" CssClass="form-control" ClientIDMode="Static" Text="0" />
                                            </div>
                                            <div class="col-sm-3 text-center">
                                                <small>
                                                    <asp:LinkButton ID="btnExtraCopies" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="About Extra Copies" data-action="infowindow" data-helpfile="/helpExtraCopies">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>
                                                </small>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%--Ship To Address--%>
                                    <div id="shipToAddressBlock">

                                        <%--Shipping Address DDL--%>
                                        <div class="form-group" id="ShippingAddressInnerBlock">
                                            <label for="ddlDeliveryAddressId" class="col-sm-4 control-label">Delivery Address</label>
                                            <div class="col-sm-5">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDeliveryAddressId" ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3 text-center">
                                                <small>
                                                    <asp:LinkButton ID="btnDeliveryAddress" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="About Delivery Address" data-action="infowindow" data-helpfile="/helpDeliveryAddress">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>
                                                </small>
                                            </div>
                                        </div>

                                        <%--New Address--%>
                                        <div id="newAddressBlock">

                                            <%--Street Address--%>
                                            <div class="form-group has-error" id="address1Block">
                                                <label for="DeliveryAddress" class="col-sm-4 control-label">Street Address</label>
                                                <div class="col-sm-5">
                                                    <asp:TextBox runat="server" ID="DeliveryAddress" CssClass="form-control" ClientIDMode="Static" />
                                                </div>
                                                <div class="col-sm-3 text-center">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDeliveryAddress" ControlToValidate="DeliveryAddress" ErrorMessage="A street address is required for the delivery address." ValidationGroup="vgDeliveryAddress" Display="Dynamic">
                                                        <span class="fa fa-2x text-danger fa-exclamation-circle"></span>
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <%--Street Address 2--%>
                                            <div class="form-group">
                                                <label for="DeliveryAddress2" class="col-sm-4 control-label">Street Address 2</label>
                                                <div class="col-sm-5">
                                                    <asp:TextBox runat="server" ID="DeliveryAddress2" CssClass="form-control" ClientIDMode="Static" />
                                                </div>
                                                <div class="col-sm-3 text-center">
                                                    &nbsp;
                                                </div>
                                            </div>

                                            <%--City--%>
                                            <div class="form-group has-error" id="cityBlock">
                                                <label for="DeliveryCity" class="col-sm-4 control-label">City</label>
                                                <div class="col-sm-5">
                                                    <asp:TextBox runat="server" ID="DeliveryCity" CssClass="form-control" ClientIDMode="Static" />
                                                </div>
                                                <div class="col-sm-3 text-center">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDeliveryCity" ControlToValidate="DeliveryCity" ErrorMessage="A city is required for the delivery address." ValidationGroup="vgDeliveryAddress" Display="Dynamic">
                                                        <span class="fa fa-2x text-danger fa-exclamation-circle"></span>
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <%--State--%>
                                            <div class="form-group" id="stateBlock">
                                                <label for="DeliveryState" class="col-sm-4 control-label">State</label>
                                                <div class="col-sm-5">
                                                    <%--<geoselect:State runat="server" ID="DeliveryState" RequireField="True" ValidationGroup="vgDeliveryAddress" ValidationErrorMessage="A state is required for the delivery address." ValidationText="" ValidationSide="Right" />--%>
                                                    <appx:usastatesdropdown runat="server" id="USAStatesDropDown" />
                                                </div>
                                                <div class="col-sm-3 text-center">
                                                    &nbsp;
                                                </div>
                                            </div>

                                            <%--Zip Cope--%>
                                            <div class="form-group" id="zipBlock">
                                                <label for="ZipCode" class="col-sm-4 control-label">Zip Code</label>
                                                <div class="col-sm-5">
                                                    <asp:TextBox runat="server" ID="ZipCode" CssClass="form-control" MaxLength="10" ClientIDMode="Static" />
                                                </div>
                                                <div class="col-sm-3 text-center">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvZipCode" ControlToValidate="ZipCode" ErrorMessage="A zip code is required for the delivery address." ValidationGroup="vgDeliveryAddress" Display="Dynamic">
                                                        <span class="fa fa-2x text-danger fa-exclamation-circle"></span>
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <%--Product Options--%>
                                    <asp:ListView ID="lvProdOpts" runat="server" ItemPlaceholderID="phItemTemplate" Visible="true" EnableViewState="true">

                                        <LayoutTemplate>
                                            <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                                        </LayoutTemplate>

                                        <ItemTemplate>
                                            <asp:Panel ID="pOptRow" runat="server">

                                                <div class="form-group">
                                                    <label for="<%#Eval("Name")%>" class="col-sm-4 control-label"><%#Eval("Name")%></label>
                                                    <div class="col-sm-5">
                                                        <asp:DropDownList ID="ddlProdOpt" runat="server" CssClass="form-control prodopt" ViewStateMode="Enabled" ClientIDMode="Static" onchange="UpdatePriceQuote();" />
                                                    </div>
                                                    <div class="col-sm-3 text-center">
                                                        <small>
                                                            <asp:LinkButton ID="lnkInfo" runat="server" data-target="#moreInfoModal" data-toggle="modal" data-title="Product Options" CausesValidation="false" data-action="infowindow" data-optcatid='<%#Eval("OptCatId") %>'>
                                                                <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                            </asp:LinkButton>
                                                        </small>
                                                    </div>
                                                </div>

                                            </asp:Panel>
                                        </ItemTemplate>

                                    </asp:ListView>

                                    <%--Design Option--%>
                                    <div class="form-group">
                                        <label for="ddlDesignOption" class="col-sm-4 control-label">Design Option</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlDesignOption" CssClass="form-control" runat="server" ClientIDMode="Static">
                                                <asp:ListItem Text="My Design ($0.00)" Value="My" />
                                                <asp:ListItem Text="Free Template ($0.00)" Value="Template" />
                                                <asp:ListItem Text="Professional Design Service ({0})" Value="Pro" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3 text-center">
                                            <small>
                                                <asp:LinkButton ID="btnMoreInfo" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="About Design Options" data-action="infowindow" data-helpfile="/helpDesignOption">
                                                <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                </asp:LinkButton>
                                            </small>
                                        </div>
                                    </div>

                                    <%--My Designs and Upload--%>
                                    <div id="myDesignBlock" class="well well-sm">

                                        <div class="form-group">
                                            <label class="col-sm-4 control-label" for="exampleInputFile">Upload Front</label>
                                            <neatUpload:InputFile ID="File1" runat="server" class="col-sm-5" /><br />
                                            <p class="help-block"><small>Upload your first file here.</small></p>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-4 control-label" for="exampleInputFile">Upload Back</label>
                                            <neatUpload:InputFile ID="File2" runat="server" class="col-sm-5" /><br />
                                            <p class="help-block"><small>Upload your second file here.</small></p>
                                        </div>

                                        <p class="text-center"><small><span class="fa fa-info-circle text-info"></span>&nbsp;You can provide your files later.</small></p>

                                        <div class="row">
                                            <div class="col-sm-8 col-sm-offset-4">

                                                <div class="checkbox">
                                                    <label class="control-label">
                                                        <asp:CheckBox ID="chkRequestProof" runat="server" ClientIDMode="Static" />
                                                        <strong>Request a PDF proof before printing</strong>
                                                    </label>
                                                </div>

                                                <br />

                                                <small>
                                                    We review all customer submitted art files and will contact you if we find any issues (whether you request a proof or not). 
                                                    If you request a proof, we will not send your file to print until you have submitted your approval. 
                                                    <mark>Requesting a proof may delay your mailing up to one week.</mark>  
                                                </small>

                                            </div>
                                        </div>


                                    </div>

                                    <%--Free Template--%>
                                    <div id="freeTemplateBlock" class="well well-sm">

                                        <asp:Panel ID="pnlTemplateSelected" runat="server">

                                            <div>&nbsp;</div>

                                            <div class="row">
                                                <div class="col-md-7">

                                                    <div class="alert alert-info" id="notSelectedTemplateBlock">
                                                        <span class="glyphicon glyphicon-info-sign"></span>&nbsp;Select a Template to complete this step. Click the <em>Choose a Template</em> button.
                                                    </div>

                                                    <div class="hidden" id="goodChoiceFreeDesignBlock">
                                                        <span class="glyphicon glyphicon-ok"></span>&nbsp;
                                                        <strong>Great choice!</strong><br />
                                                        <asp:Label ID="lblYouHaveSelected" runat="server" ClientIDMode="Static" /><br />
                                                        Great choice! A designer will be in touch within 1-2 business days to begin customization of your template. 
                                                        We'll replace text, images and logos with your own selections for free!
                                                    </div>

                                                </div>

                                                <div class="col-md-5">
                                                    <p class="text-center">
                                                        <asp:Image ID="imgSelectedTemplate" runat="server" ImageUrl="~/assets/images/your-template.png" CssClass="img-thumbnail templateThumbnail" ClientIDMode="Static" />
                                                    </p>
                                                </div>

                                            </div>

                                        </asp:Panel>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <p class="text-center">
                                                    <asp:LinkButton ID="lnkChooseTemplate" runat="server" CssClass="btn btn-primary btn-lg" ClientIDMode="Static">
                                                        <span class="glyphicon glyphicon-search"></span>&nbsp;Choose a Template
                                                    </asp:LinkButton>
                                                </p>
                                            </div>
                                        </div>

                                    </div>

                                    <%--Professional Design--%>
                                    <div id="professionalDesignBlock" class="well well-sm">
                                        <div class="alert alert-info">
                                            <p><span class="glyphicon glyphicon-earphone"></span>&nbsp;
                                            <strong>Good Choice!</strong> One of our designers will contact you within one to two business days of placing your order.</p>
                                        </div>
                                    </div>

                                    <div>&nbsp;</div>

                                    <%--Button--%>
                                    <div id="continueBlock">
                                        <p class="text-right">
                                            <asp:LinkButton ID="btnContinue" runat="server" CssClass="btn btn-cta btn-lg pull-right btn-shadow" ClientIDMode="Static" OnClientClick="ShowDeliveryOptions(); ContinueToDeliveryOptions(); return false;" CausesValidation="False">
                                                Continue to Delivery Options&nbsp;<span class="glyphicon glyphicon-chevron-right"></span>
                                            </asp:LinkButton>
                                        </p>
                                    </div>

                                </div>

                            </div>  

                        </div>

                        <%--Delivery Options--%>
                        <div class="panel panel-primary" id="deliveryOptionsBlock">

                            <div class="panel-heading">Delivery Options</div>

                            <div class="panel-body">

                                <p class="lead"><span class="leadDropWord">Great!</span> Now let's define your Delivery Options.</p>

                                <div class="form-horizontal" role="form">

                                    <%--Impressions--%>
                                    <asp:Panel ID="pnlMultipleImpressions" runat="server" Visible="True">
                                        <div class="form-group">
                                            <label for="ddlImpressions" class="col-sm-4 control-label">How many times do you want to mail this offer to each address?</label>
                                            <div class="col-sm-5">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlImpressions" ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3 text-center">
                                                <small>

                                                    <%--EDDM Orders--%>
                                                    <asp:LinkButton ID="lnkNumberOfTimesToMail" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="How many times do you want to mail each address?" data-action="infowindow" data-helpfile="/helpnumbertimesaddress" Visible="False">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>

                                                    <asp:LinkButton ID="lnkNumberOfTimesToMailNoFee" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="How many times do you want to mail each address?" data-action="infowindow" data-helpfile="/helpNumImpressionsNoFee" Visible="False">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>

                                                    <%--AddressedList Orders--%>
                                                    <asp:LinkButton ID="lnkNumberOfTimesToMailList" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="How many times do you want to mail each address?" data-action="infowindow" data-helpfile="/helpnumbertimesaddressList" Visible="False">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>

                                                    <asp:LinkButton ID="lnkNumberOfTimesToMailNoFeeList" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="How many times do you want to mail each address?" data-action="infowindow" data-helpfile="/helpNumImpressionsNoFeeList" Visible="False">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>




                                                </small>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%--Drops - All At Once?--%>
                                    <asp:Panel ID="pnlDropsBlock" runat="server" ClientIDMode="Static">
                                        <div class="form-group">
                                            <label for="ddlDrops" class="col-sm-4 control-label">Do you want to mail all of your pieces at once?</label>
                                            <div class="col-sm-5">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDrops" ClientIDMode="Static">
                                                    <asp:ListItem Value="Yes" Selected="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="No">No</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3 text-center">
                                                <small>
                                                    <asp:LinkButton ID="btnDrops" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="About the Number of Drops" data-action="infowindow" data-helpfile="/helpNumberOfDrops">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>
                                                </small>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%--Number of Drops--%>
                                    <asp:Panel ID="pnlNumOfDropsBlock" runat="server" ClientIdMode="Static">
                                        <div class="form-group">
                                            <label for="ddlNumOfDrops" class="col-sm-4 control-label">How many mailings would you like to distribute?</label>
                                            <div class="col-sm-5">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNumOfDrops" ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3 text-center">
                                                <small>
                                                    <asp:LinkButton ID="btnNumOfMailing" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="About the Number of Drops" data-action="infowindow" data-helpfile="/helpNumberOfDrops">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>
                                                </small>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%--Launch Week--%>
                                    <div class="form-group has-error" id="launchWeekBlock">

                                        <strong><asp:label for="ddlLaunchWeek" runat="server" id="lblLaunchWeek2" CssClass="col-sm-4 control-label" /></strong>
                                        
                                        <div class="col-sm-5">

                                            <div id="myDesignLaunchWeekBlock">
                                                <appx:DropDate runat="server" id="ddlMyDesignLaunchWeek" />
                                            </div>

                                            <div id="templateDesignLaunchWeekBlock">
                                                <appx:DropDate runat="server" id="ddlTemplateDesignLaunchWeek" />
                                            </div>

                                            <div id="proDesignLaunchWeekBlock">
                                                <appx:DropDate runat="server" id="ddlProDesignLaunchWeek" />
                                            </div>
                                        </div>

                                        <div class="col-sm-3 text-center">
                                            <small>
                                                <%--EDDM help modal--%>
                                                <asp:LinkButton ID="btnEDDMLaunchWeek" Visible="false" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-action="infowindow" data-title="When do you want your first mailing to reach each targeted address?" data-helpfile="/helpfirstmaildate">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                </asp:LinkButton>
                                                <%--AddressedList help modal--%>
                                                <asp:LinkButton ID="btnAddressedLaunchDate" Visible="false" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-action="infowindow" data-title="Please indicate when your mailer should drop into the mail stream" data-helpfile="/helpAddressedLaunchDate">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                </asp:LinkButton>
                                            </small>
                                        </div>
                                    </div>

                                    <%--Frequency--%>
                                    <asp:Panel ID="pnlFrequencyBlock" runat="server" ClientIDMode="Static">
                                        <div class="form-group">
                                            <label for="ddlFrequency" class="col-sm-4 control-label">What frequency should additional mailings distribute?</label>
                                            <div class="col-sm-5">
                                                <asp:DropDownList ID="ddlFrequency" CssClass="form-control" runat="server" ClientIDMode="Static">
                                                    <asp:ListItem Value="1">Every Week</asp:ListItem>
                                                    <asp:ListItem Value="2">Every 2 Weeks</asp:ListItem>
                                                    <asp:ListItem Value="3">Every 3 Weeks</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="4">Every 4 Weeks</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3 text-center">
                                                <small>
                                                    <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-action="infowindow" data-title="With what frequency should additional mailings distribute?" data-helpfile="/helpmailfrequency">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>
                                                </small>
                                            </div>
                                        </div>
                                    </asp:Panel>


                                    <%--Exclusivity--%>
                                    <div id="ExclusivityBlock">

                                        <div class="col-sm-12 text-center">
                                            <strong><asp:Label ID="lExclusive" runat="server" ClientIDMode="Static" CssClass="text-info" /></strong>
                                        </div>

                                        <div>&nbsp;</div>

                                    </div>  



                                    <%--Job Names--%>
                                    <%--Needs to be styled if ever shown--%>
                                    <asp:Panel ID="pnlJobNames" runat="server" Visible="false" CssClass="well well-sm">

                                        <p>(Needs to be styled)</p>

                                        <asp:PlaceHolder runat="server" ID="phJobName">
                                            <div>Job Name<br />
                                                <asp:TextBox ID="JobName" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvJobName" runat="server" ErrorMessage="Please enter a job name." Text="(*)" ValidationGroup="vgOrder" ControlToValidate="JobName" />
                                            </div>
                                        </asp:PlaceHolder>
                                    
                                        <asp:PlaceHolder runat="server" ID="phJobNameCustom">
                                    
                                            <asp:PlaceHolder runat="server" ID="phJobNameCustom1" Visible="False">
                                                <asp:Literal runat="server" ID="lblJobNameCustom1" />:<br />
                                                <asp:TextBox runat="server" ID="JobNameCustom1" Columns="10" />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvJobNameCustom1" ControlToValidate="JobNameCustom1" ErrorMessage="Please enter a value" Text="(*)" ValidationGroup="vgOrder" />
                                            </asp:PlaceHolder>
                                    
                                            <asp:PlaceHolder runat="server" ID="phJobNameCustom2" Visible="False">
                                                <asp:Literal runat="server" ID="lblJobNameCustom2" />:<br />
                                                <asp:TextBox runat="server" ID="JobNameCustom2" Columns="10" />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvJobNameCustom2" ControlToValidate="JobNameCustom2" ErrorMessage="Please enter a value" Text="(*)" ValidationGroup="vgOrder" />
                                            </asp:PlaceHolder>
                                    
                                            <asp:PlaceHolder runat="server" ID="phJobNameCustom3" Visible="False">
                                                <asp:Literal runat="server" ID="lblJobNameCustom3" />:<br />
                                                <asp:TextBox runat="server" ID="JobNameCustom3" Columns="10" />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvJobNameCustom3" ControlToValidate="JobNameCustom3" ErrorMessage="Please enter a value" Text="(*)" ValidationGroup="vgOrder" />
                                            </asp:PlaceHolder>
                                    
                                        </asp:PlaceHolder>

                                        <div>Comments</div>
                                        <div><asp:TextBox ID="JobComments" runat="server" TextMode="MultiLine" Rows="5" /></div>

                                    </asp:Panel>

                                    <div>&nbsp;</div>

                                </div>

                                <%--Buttons--%>
                                <div class="row">

                                    <div class="col-sm-6">
                                        <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-primary pull-left btn-shadow" ClientIDMode="Static" OnClientClick="BackToDesignOptions(); return false" CausesValidation="False">
                                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;Back to Design Options
                                        </asp:LinkButton>
                                    </div>

                                    <div class="col-sm-6">
                                        <asp:LinkButton ID="btnCheckout" runat="server" CssClass="btn btn-default pull-right disabled btn-shadow" ClientIDMode="Static" CausesValidation="False">
                                            Proceed to Check Out&nbsp;<span class="glyphicon glyphicon-shopping-cart"></span>
                                        </asp:LinkButton>
                                    </div>

                                </div>


                            </div>  

                        </div>

                    </div>

                    <%--Campaign Overview--%>
                    <div class="col-sm-4">
                        <asp:Panel ID="pnlCampaignOverview" runat="server" ClientIDMode="Static">
                            <appx:CampaignOverview runat="server" id="CampaignOverview" />
                        </asp:Panel>
                    </div>

                </div>


                <%--Template Browser Control--%>
                <appx:TemplateBrowser runat="server" id="TemplateBrowser" />


                <%--More Info Modal--%>
                <div class="modal fade" id="moreInfoModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">

                            <div class="modal-header">
                                <div class="modal-title"><span id="infoTitle"></span></div>
                            </div>

                            <div class="modal-body">
                                <div id="infoContent"></div>
                            </div>

                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary pull-right" data-dismiss="modal">
                                    <span class="fa fa-check"></span>&nbsp;OK
                                </button>
                            </div>

                        </div>
                    </div>
                </div>


                <%--Warning Modal--%>
                <div class="modal fade" id="warningModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">

                            <div class="modal-header">
                                <div class="modal-title">Attention</div>
                            </div>

                            <div class="modal-body">
                                <p><span class="fa fa-info-circle text-warning"></span>&nbsp;Everything is OK but we will need to reload this page and recalculate when you 
                                change products.</p>

                                <p>&nbsp;</p>

                                <div class="text-center">
                                    <button type="button" class="btn btn-warning text-center" data-dismiss="modal" onclick="LoadNewProduct();">
                                        <span class="fa fa-check"></span>&nbsp;OK
                                    </button>
                                </div>

                                <p>&nbsp;</p>

                            </div>

                        </div>
                    </div>
                </div>

            </asp:Panel>


        </div>
    
    </div>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">

    <script src="/assets/javascripts/Step2-ProductOptions.min.js?v=1.0.4"></script>

    <script type="text/javascript">

        //Hide initially
        $('#freeTemplateBlock').hide();
        $('#professionalDesignBlock').hide();
        $('#shipToAddressBlock').hide();
        $('#newAddressBlock').hide();
        $('#deliveryOptionsBlock').hide();
        $('#templateDesignLaunchWeekBlock').hide();
        $('#proDesignLaunchWeekBlock').hide();
        $('#pnlNumOfDropsBlock').hide();
        $('#pnlFrequencyBlock').hide();

    </script>

    <asp:Literal ID="litCampaignOverviewHide" runat="server" Visible="False" />

</asp:Content>

