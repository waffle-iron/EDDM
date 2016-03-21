<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="true" CodeFile="MarketingServices.aspx.cs" Inherits="MarketingServices" Debug="true" Trace="false" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="eddm" TagName="PageHeader" %>


<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
    <link href="/assets/css/jquery-ui.css" rel="stylesheet" />
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

    <div class="container">

        <eddm:PageHeader runat="server" id="PageHeader" />

        <div class="contentWrapper">
            

            <%--Debug panel--%>
            <asp:Panel ID="pnlDebug" runat="server" CssClass="hidden">

                <h3>TEST MODE</h3>

                <p><strong>Hidden TextBox Controls and Hidden Fields</strong></p>

                <table class="table table-bordered table-striped table-condensed table-test-mode">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>TextBox</th>
                            <th>Hidden Field</th>
                            <th>Description</th>
                        </tr>
                    </thead>

                    <tbody>

                        <tr>
                            <td class="width30">BaseProductID</td>
                            <td class="width15"><asp:TextBox ID="txtBaseProductID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidBaseProductID" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Used to wire up help modals for prods on this page</td>
                        </tr>

                        <tr>
                            <td class="width30">ProductID</td>
                            <td class="width15"><asp:TextBox ID="txtProductID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidProductID" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Used to wire up help modals for prods on this page</td>
                        </tr>

                        <tr>
                            <td class="width30">NumTimesSelected</td>
                            <td class="width15"><asp:TextBox ID="txtNumTimesSelected" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidNumTimesSelected" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">For AddOnAddressedList impressions ONLY</td>
                        </tr>

                        <tr>
                            <td class="width30">Total Selected</td>
                            <td class="width15"><asp:TextBox ID="txtTotalSelected" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidTotalSelected" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Total selected from original EDDM map</td>
                        </tr>

                        <tr>
                            <td class="width30">Num Impressions</td>
                            <td class="width15"><asp:TextBox ID="txtNumImpressions" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidNumImpressions" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Num of EDDM Impressions selected</td>
                        </tr>

                        <tr>
                            <td class="width30">Extra Copies</td>
                            <td class="width15"><asp:TextBox ID="txtExtraCopies" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidExtraCopies" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Extra Copies from EDDM order</td>
                        </tr>

                        <tr>
                            <td class="width30">Distribution ID</td>
                            <td class="width15"><asp:TextBox ID="txtDistributionID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidDistributionID" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Distribution / Map ID</td>
                        </tr>

                        <tr>
                            <td class="width30">Zip Code</td>
                            <td class="width15"><asp:TextBox ID="txtZipCode" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidZipCode" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Zip Code of customer</td>
                        </tr>

                        <tr>
                            <td class="width30">Addressed Mail AddOn Mark Up</td>
                            <td class="width15"><asp:TextBox ID="txtAddressedMailMarkUp" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidAddressedMailMarkUp" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Addressed Mail AddOn Product Mark Up Val</td>
                        </tr>

                        <tr>
                            <td class="width30">AddressedMail Mark Up Type</td>
                            <td class="width15"><asp:TextBox ID="txtAddressedMailMarkUpType" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidAddressedMailMarkUpType" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">AddressedMail Product Mark Up Type</td>
                        </tr>

                        <tr>
                            <td class="width30">USelectID</td>
                            <td class="width15"><asp:TextBox ID="txtUSelectID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidUSelectID" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">USelectID of order method used to reach this page.  EDDM (1) versus Addressed (5 or 6)</td>
                        </tr>

                        <tr>
                            <td class="width30">Addressed AddOn BaseProductID</td>
                            <td class="width15"><asp:TextBox ID="txtAddressedAddOnBaseProductID" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidAddressedAddOnBaseProductID" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">Currently hard coded</td>
                        </tr>

                        <tr>
                            <td class="width30">Addressed AddOn PricePerPiece</td>
                            <td class="width15"><asp:TextBox ID="txtAddressedAddOnPricePerPiece" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidAddressedAddOnPricePerPiece" runat="server" ClientIDMode="Static" /></td>
                            <td class="width40">From initial load - can CHANGE from UI interaction</td>
                        </tr>

                        <tr>
                            <td class="width30">SelectedAddOnAddressedProspects</td>
                            <td class="width15"><asp:TextBox ID="txtSelectedAddOnAddressedList" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidSelectedAddOnAddressedList" runat="server" ClientIDMode="Static" EnableViewState="true" /></td>
                            <td class="width40">User selected number of AddressedMail AddOns</td>
                        </tr>

                        <tr>
                            <td class="width30">SuggestedAddressedAddOnStartQty</td>
                            <td class="width15"><asp:TextBox ID="txtSuggestedAddressedAddOnStartQty" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidSuggestedAddressedAddOnStartQty" runat="server" ClientIDMode="Static" EnableViewState="true" /></td>
                            <td class="width40">Initial calculated suggested start amount for AddressedList AddOns</td>
                        </tr>

                        <tr>
                            <td class="width30">AddOnAddressed Prospects (max)</td>
                            <td class="width15"><asp:TextBox ID="txtAddOnAddressedProspects" runat="server" ClientIDMode="Static" Visible="true" CssClass="form-control input-sm" /></td>
                            <td class="width15"><asp:HiddenField ID="hidAddOnAddressedProspects" runat="server" ClientIDMode="Static" EnableViewState="true" />
                                <asp:HiddenField ID="hidOverrideAddOnAddressedProspects" runat="server" ClientIDMode="Static" EnableViewState="true" />
                                <asp:HiddenField ID="hidOverrideAddOnAddressedTimes" runat="server" ClientIDMode="Static" EnableViewState="true" />
                            </td>
                            <td class="width40">(max value - initially calculated)</td>
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
                            <td class="width50">SiteID</td>
                            <td class="width50"><asp:Literal ID="litSiteID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">TestMode</td>
                            <td class="width50"><asp:Literal ID="litTestMode" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Current Mode</td>
                            <td class="width50"><asp:Literal ID="litCurrentMode" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">DistributionID</td>
                            <td class="width50"><asp:Literal ID="litDistributionID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">ReferenceID</td>
                            <td class="width50"><asp:Literal ID="litReferenceID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">USelectID</td>
                            <td class="width50"><asp:Literal ID="litUSelectID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">TotalSelected</td>
                            <td class="width50"><asp:Literal ID="litDebugTotalSelected" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">NumImpressions</td>
                            <td class="width50"><asp:Literal ID="litNumImpressions" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">MailingCost</td>
                            <td class="width50"><asp:Literal ID="litMailingCost" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">ProductID</td>
                            <td class="width50"><asp:Literal ID="litProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">BaseProductID</td>
                            <td class="width50"><asp:Literal ID="litBaseProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">ProductName</td>
                            <td class="width50"><asp:Literal ID="litProductName" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">DesignFee</td>
                            <td class="width50"><asp:Literal ID="litDesignFee" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">ProductPrice</td>
                            <td class="width50"><asp:Literal ID="litProductPrice" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">EddmMap</td>
                            <td class="width50"><asp:Literal ID="litEddmMap" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">AddressedMap</td>
                            <td class="width50"><asp:Literal ID="litAddressedMap" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Zip Code</td>
                            <td class="width50"><asp:Literal ID="litZipCode" runat="server" /></td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <strong>Addressed AddOn Properties</strong>
                            </td>
                        </tr>

                        <tr>
                            <td class="width50">AddressedAddOn PostageRate</td>
                            <td class="width50"><asp:Literal ID="litAddressedAddOnPostageRate" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">addOnAddressedProspects (max value - initially calculated)</td>
                            <td class="width50"><asp:Literal ID="litAddOnAddressedProspects" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">AddressedMail Mark Up</td>
                            <td class="width50"><asp:Literal ID="litAddressedMailMarkUp" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">AddressedMail Mark Up Type</td>
                            <td class="width50"><asp:Literal ID="litAddressedMailMarkUpType" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Addressed AddOn BaseProductID</td>
                            <td class="width50"><asp:Literal ID="litAddressedAddOnBaseProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">AddressedAddOnProductID</td>
                            <td class="width50"><asp:Literal ID="litAddressedAddOnProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Addressed AddOn PricePerPiece (static page init ONLY)</td>
                            <td class="width50"><asp:Literal ID="litAddressedAddOnPricePerPiece" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Show Addressed AddOn</td>
                            <td class="width50"><asp:Literal ID="litShowAddressedAddOn" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">suggestedAddressedAddOnStartQty</td>
                            <td class="width50"><asp:Literal ID="litSuggestedAddressedAddOnStartQty" runat="server" /></td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <strong>New Mover Properties</strong>
                            </td>
                        </tr>

                        <tr>
                            <td class="width50">NewMoverPostageRate</td>
                            <td class="width50"><asp:Literal ID="litNewMoverPostageRate" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">NewMoverBaseProductID</td>
                            <td class="width50"><asp:Literal ID="litNewMoverBaseProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">NewMoverProductID</td>
                            <td class="width50"><asp:Literal ID="litNewMoverProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">NewMoverRate</td>
                            <td class="width50"><asp:Literal ID="litNewMoverRate" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">NewMoverCount</td>
                            <td class="width50"><asp:Literal ID="litNewMoverCount" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">ShowNewMover</td>
                            <td class="width50"><asp:Literal ID="litShowNewMover" runat="server" /></td>
                        </tr>


                        <tr>
                            <td colspan="2">
                                <strong>Targeted Email Properties</strong>
                            </td>
                        </tr>

                        <tr>
                            <td class="width50">ShowEmails</td>
                            <td class="width50"><asp:Literal ID="litShowEmails" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">NumEmailCampaigns</td>
                            <td class="width50"><asp:Literal ID="litDebugNumEmailCampaigns" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">EmailServiceFee</td>
                            <td class="width50"><asp:Literal ID="litEmailServiceFee" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">EmailPerThousandRate</td>
                            <td class="width50"><asp:Literal ID="litEmailPerThousandRate" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">TargetedEmailCount</td>
                            <td class="width50"><asp:Literal ID="litTargetedEmailCount" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">TargetedEmailBaseProductID</td>
                            <td class="width50"><asp:Literal ID="litTargetedEmailBaseProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">TargetedEmailProductID</td>
                            <td class="width50"><asp:Literal ID="litTargetedEmailProductID" runat="server" /></td>
                        </tr>
                        
                    </tbody>

                </table>

                <p>&nbsp;</p>
                
                <p><strong>OLB Session Variables</strong></p>

                <table class="table table-bordered table-striped table-condensed table-test-mode">

                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Value</th>
                        </tr>
                    </thead>

                    <tbody>

                        <tr>
                            <td class="width50">DistID</td>
                            <td class="width50"><asp:Literal ID="litSesDistID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Franchise</td>
                            <td class="width50"><asp:Literal ID="litSesFranchise" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Location</td>
                            <td class="width50"><asp:Literal ID="litSesLocation" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Template</td>
                            <td class="width50"><asp:Literal ID="litSesTemplate" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">TemplateID</td>
                            <td class="width50"><asp:Literal ID="litSesTemplateID" runat="server" /></td>
                        </tr>


                        <tr>
                            <td class="width50">Impressions</td>
                            <td class="width50"><asp:Literal ID="litSesImpressions" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Campaign</td>
                            <td class="width50"><asp:Literal ID="litSesCampaign" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">QTY</td>
                            <td class="width50"><asp:Literal ID="litSesQTY" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Budget</td>
                            <td class="width50"><asp:Literal ID="litSesBudget" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">RevisedMap</td>
                            <td class="width50"><asp:Literal ID="litSesRevisedMap" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">TotSelected</td>
                            <td class="width50"><asp:Literal ID="litSesTotSelected" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">AvgMatch</td>
                            <td class="width50"><asp:Literal ID="litSesAvgMatch" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">EstCost</td>
                            <td class="width50"><asp:Literal ID="litSesEstCost" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">TotalMailed</td>
                            <td class="width50"><asp:Literal ID="litSesTotalMailed" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">PricePerPiece</td>
                            <td class="width50"><asp:Literal ID="litSesPricePerPiece" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">Frequency</td>
                            <td class="width50"><asp:Literal ID="litSesFrequency" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">StartDate</td>
                            <td class="width50"><asp:Literal ID="litSesStartDate" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">ProductID</td>
                            <td class="width50"><asp:Literal ID="litSesProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">BaseProductID</td>
                            <td class="width50"><asp:Literal ID="litSesBaseProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">sesMapName</td>
                            <td class="width50"><asp:Literal ID="litSesMapName" runat="server" /></td>
                        </tr>

                        <tr>
                            <td class="width50">sesDistRefId</td>
                            <td class="width50"><asp:Literal ID="litSesDistRefId" runat="server" /></td>
                        </tr>

                    </tbody>

                </table>

                <p>&nbsp;</p>

            </asp:Panel>


            <%--Error Display--%>
            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                <i class="fa fa-2x fa-exclamation-circle pull-left"></i>&nbsp;
                <asp:Literal ID="litErrorMessage" runat="server" />
                <p>&nbsp;</p>
            </asp:Panel>


            <%--Normal display--%>
            <asp:Panel ID="pnlNormalDisplay" runat="server">

                <h2>This is a great start!</h2>

                <%--Mailed Products--%>
                <asp:Panel ID="pnlMailedProducts" runat="server">

                    <div class="panel panel-default">
                        <div class="panel-body">
                    
                            <div class="row">
                                <div class="col-sm-2">
                                    <asp:Image ID="imgMailbox" runat="server" ImageUrl="~/assets/images/mailbox3.png" CssClass="img-responsive" />
                                </div>
                                <div class="col-sm-7">
                                    <p class="productTitle"><asp:Literal ID="litProdName" runat="server" /> Mailing Campaign</p>
                                    <p>Mail high-powered direct mail offers to&nbsp;<asp:Literal ID="litTotalSelected" runat="server" />&nbsp;
                                    targeted residential addresses&nbsp;<asp:Literal id="litImpressions" runat="server" />.</p>
                                    <p><em>Includes design, print, mail prep, all required paperwork, USPS&reg; postage and delivery.</em></p>
                                </div>
                                <div class="col-sm-3 text-right">
                                    <span class="checkoutReviewSubTitle">Only</span><br />
                                    <asp:Label runat="server" ID="lblMailedProductPrice" CssClass="checkoutPrice" /><br />
                                    <small>(100% All-Inclusive!)</small><br />
                                    <div class="addOnWrapper checkoutReviewSubTitle">
                                        <asp:Image ID="imgCheck" runat="server" CssClass="img-responsive icon30" ImageUrl="~/assets/images/step-check.png" />In Your Order
                                    </div>
                                </div>
                            </div>
                    
                        </div>
                    </div>

                </asp:Panel>
            
                <%--Add ons--%>
                <asp:Panel ID="pnlUpsellServices" runat="server">

                    <div class="jumbotron">

                        <p>Now add <strong>one or more</strong> of our <strong>top-rated marketing services</strong> to reinforce 
                        your message and expand your reach!</p>

                    </div>

                    <h2>Additional Marketing Services</h2>

                    <%--AddressedList AddOns--%>
                    <asp:Panel ID="pnlAddressedListAddOns" runat="server" Visible="false">

                        <div class="panel panel-default">

                            <div class="panel-body">

                                <div class="col-sm-2">

                                    <%--This img will need to be dynamically set.--%>
                                    <asp:Image ID="imgAddOn" runat="server" CssClass="img-responsive" ImageUrl="~/cmsimages/11/6.25x9-template.png" />

                                </div>

                                <div class="col-sm-6">

                                    <p class="productTitle">Addressed Mail Add Ons</p>
        
                                    <p>There are <asp:Label runat="server" ID="lblAddressedHyperMatchCount" Text="" CssClass="checkoutPrice" /> 
                                    High Value Prospects within your franchise territory of <mark><asp:Label ID="lblFranchise" runat="server" /></mark> 
                                    but outside your High Value EDDM Routes. We will send every one of them a 6 X 11 Postcard. 
                                    This is a perfect way to reach every qualified prospect in your trade area.</p>
               
                                    <p>Use the slider below to set how many prospects you wish to mail. <br />
                                    Tip: you can use your arrow keys to fine tune the quantity.</p>

                                    <%--Slider container--%>
                                    <div class="alert alert-success">
                                        
                                        <div>
                                            <p><strong>How many High Value Prospects would you like to mail?</strong></p>
                                        </div>

                                        <div class="row extraBottomPadding">
                                            <div class="col-sm-3 text-right">
                                                <span id="minProspects"></span>
                                            </div>
                                            <div class="col-sm-6">
                                                <div id="sliderControl"></div>
                                            </div>
                                            <div class="col-sm-3 text-left">
                                                <span id="maxProspects"></span>
                                            </div>
                                        </div>

                                        <p class="text-center">I want to send to <asp:Label ID="lblSendToAddOns" runat="server" ClientIDMode="static" CssClass="checkoutPrice" /> top-ranking prospects
                                        <strong><asp:Label ID="lblSendNumTimes" runat="server" ClientIDMode="static" /></strong>!</p>

                                        <div id="addressedPricePerPieceBlock">
                                            <p class="text-center">(only <span id="addressedPricePerPiece"></span> each)</p>
                                        </div>

                                    </div>

                                    <p>Select below to indicate how many times you wish to send out your campaign. <mark>These will be presented to the USPS at the same time as your EDDM campaigns.</mark></p>

                                    <%--Num Times Buttons--%>
                                    <div class="row ui-controls">

                                        <div class="col-sm-4">
                                            <button onclick="NumTimesAddressedAddOnSelected('btn1Time');" type="button" class="btn btn-sm btn-block btn-shadow" id="btn1Time">One Time</button>
                                        </div>
                                        <div class="col-sm-4">
                                            <button onclick="NumTimesAddressedAddOnSelected('btn2Times');" type="button" class="btn btn-sm btn-shadow btn-block" id="btn2Times">Two Times</button>
                                        </div>
                                        <div class="col-sm-4">
                                            <button onclick="NumTimesAddressedAddOnSelected('btn3Times');" type="button" class="btn btn-sm btn-block btn-shadow selected" id="btn3Times">Three Times</button>
                                        </div>

                                    </div>

                                    <p>&nbsp;</p>

                                    <div class="row">
                                        <div class="col-xs-8">
                                            <p><em>Includes design, print, mail prep, all required paperwork, USPS&reg; postage and delivery.</em></p>
                                        </div>
                                        <div class="col-xs-4">
                                            <small>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="Addressed List Add-Ons" data-action="infowindow" data-helpfile="/helpAddressedListAddOns">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                </asp:LinkButton>
                                            </small>
                                        </div>
                                    </div>

                                </div>

                                <%--addressedMailPriceBlock--%>
                                <div class="col-sm-4 text-right">

                                    <div id="addressedMailPriceBlock">
                                        <span class="checkoutReviewSubTitle">Only</span><br />
        
                                        <asp:Label runat="server" ID="lblAddressedAddOnPrice" ClientIDMode="static" CssClass="checkoutPrice" /><br />
        
                                        <small>100% All-Inclusive</small><br />
        
                                        <div class="addOnWrapper checkoutReviewSubTitle">
                                            <label>
                                                <asp:CheckBox ID="chkAddressedAddOns" runat="server" />&nbsp;Select and Add To Order
                                            </label>
                                        </div>
                                    </div>
                                    
                                    <div id="addressedMailPriceError" class="hidden">
                                        <p><span class="fa fa-warning"></span>&nbsp;Sorry - but there was an error retrieving the data.  Please try this option later.</p>
                                    </div>

                                </div>

                            </div>

                        </div>

                    </asp:Panel>

                    <%--New Mover--%>
                    <asp:Panel ID="pnlNewMover" runat="server" Visible="False">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <div class="col-sm-2">
                                    <asp:Image ID="imgNewMover" runat="server" CssClass="img-responsive" ImageUrl="~/assets/images/newmover.png" />
                                </div>

                                <asp:Panel ID="pnlNewMoversSellPane" runat="server" Visible="False">

                                    <div class="col-sm-6">
                                        <p class="productTitle">New Mover&trade; Postcards</p>
                                        <p>Reach <strong><asp:Literal ID="litRecentMoverCount" runat="server" />
                                        recent new movers</strong> in the same target area with personalized postcards. This is a <mark>monthly
                                        service</mark> which you can cancel at any time. </p> 
                                        <div class="row">
                                            <div class="col-xs-8">
                                                <p><em>Includes design, print, mail prep, all required paperwork, USPS&reg; postage and delivery.</em></p>
                                            </div>
                                            <div class="col-xs-4">
                                                <small>
                                                    <asp:LinkButton ID="btnNewMoverInfo" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="New Mover Postcards" data-action="infowindow" data-helpfile="/helpNewMover">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>
                                                </small>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="col-sm-4 text-right">
                                        <span class="checkoutReviewSubTitle">Only</span><br />
                                        <asp:Label runat="server" ID="lblNewMoverPrice" CssClass="checkoutPrice" /><br />
                                        <small>100% All-Inclusive</small><br />
                                        <div class="addOnWrapper checkoutReviewSubTitle">
                                            <label>
                                                <asp:CheckBox ID="chkAddNewMover" runat="server" />&nbsp;Select and Add To Order
                                            </label>
                                        </div>
                                    </div>

                                </asp:Panel>

                                <asp:Panel ID="pnlNoNewMoversFound" runat="server" Visible="False">

                                    <div class="col-sm-10">
                                        <p class="productTitle">New Mover&trade; Postcards</p>
                                        <p><em>Sorry but no New Movers were found in the areas you selected.  Please continue to Check out.</em></p>
                                    </div>

                                </asp:Panel>

                            </div>
                        </div>
                    </asp:Panel>
            
                    <%--Targeted Emails--%>
                    <asp:Panel ID="pnlTargetedEmails" runat="server" Visible="False">
                        <div class="panel panel-default">
                            <div class="panel-body">

                                <div class="col-sm-2">
                                    <asp:Image ID="imgEmails" runat="server" CssClass="img-responsive" ImageUrl="~/assets/images/inbox.png" />
                                </div>

                                <asp:Panel ID="pnlTargetedEmailsSellPane" runat="server" Visible="False">

                                    <div class="col-sm-6">
                                        <p class="productTitle">Targeted Emails</p>
                                        <p>Communicate directly with <strong><asp:Literal ID="litEmailCount" runat="server" /> prospects</strong> 
                                        with <mark><asp:Literal ID="litNumEmailCampaigns" runat="server" /> targeted email campaigns</mark> for a total of <asp:Literal ID="litEmailImpressions" runat="server" /> impressions.</p>
                        
                        
                                        <div class="row">
                                            <div class="col-xs-8">
                                                <p><em>Includes text-only or HTML email templates, list usage and delivery.</em></p>
                                            </div>
                                            <div class="col-xs-4">
                                                <small>
                                                    <asp:LinkButton ID="btnEmailInfo" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-title="Email Campaigns" data-action="infowindow" data-helpfile="/helpEmailCampaigns">
                                                    <span class="fa fa-info-circle"></span>&nbsp;More Information
                                                    </asp:LinkButton>
                                                </small>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="col-sm-4 text-right">
                                        <span class="checkoutReviewSubTitle">Only</span><br />
                                        <asp:Label runat="server" ID="lblEmailPrice" Text="$###.##" CssClass="checkoutPrice" /><br />
                                        <small>100% All-Inclusive</small><br />
                                        <div class="addOnWrapper checkoutReviewSubTitle">
                                            <label>
                                                <asp:CheckBox ID="chkAddEmails" runat="server" />&nbsp;Select and Add To Order
                                            </label>
                                        </div>
                                    </div>

                                </asp:Panel>


                                <asp:Panel ID="pnlNoTargetedEmails" runat="server" Visible="False">

                                     <div class="col-sm-10">
                                         <p class="productTitle">Targeted Emails</p>
                                         <p><em>Sorry but no matching customers with email addresses were found in the areas you selected.  Please continue to Check out.</em></p>
                                     </div>

                                </asp:Panel>



                            </div>
                        </div>
                    </asp:Panel>
           

                </asp:Panel>

                <div>&nbsp;</div>

                <%--Checkout--%>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:LinkButton ID="btnCheckOut" CssClass="btn btn-danger btn-lg pull-right" runat="server" OnClick="btnCheckOut_Click">
                            <span class="fa fa-credit-card" onclick="btnCheckout_Click"></span>&nbsp;Continue to Check Out
                        </asp:LinkButton>
                    </div>
                </div>

                <div>&nbsp;</div>

                <div>&nbsp;</div>

            </asp:Panel>


        </div>

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



    </div>


</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">

    <script type="text/javascript" src="/assets/javascripts/MarketingServices.min.js?ver=1.0.0"></script>
    <%--<script type="text/javascript" src="/assets/javascripts/MarketingServices.js"></script>--%>

</asp:Content>

