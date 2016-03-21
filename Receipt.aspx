<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="Receipt.aspx.vb" Trace="false" Inherits="Receipt" %>
<%@ Register Src="~/CLibrary/VisualWebsiteOptimizer.ascx" TagPrefix="eddm" TagName="VisualWebsiteOptimizer" %>
<%@ Register Src="~/Controls/ReceiptHeaderImage.ascx" TagPrefix="eddm" TagName="ReceiptHeaderImage" %>
<%@ Register Src="~/CCustom/BingConversionCode.ascx" TagPrefix="eddm" TagName="BingConversionCode" %>
<%@ Register Src="~/CCustom/BoldChatConversion.ascx" TagPrefix="eddm" TagName="BoldChatConversion" %>
<%@ Register Src="~/CCustom/93/KenshooTrackingPixels.ascx" TagPrefix="eddm" TagName="KenshooTrackingPixels" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="eddm" TagName="PageHeader" %>
<%@ Register Src="~/CCustom/1/FacebookTrackingPixel.ascx" TagPrefix="eddm" TagName="FacebookTrackingPixel" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <eddm:BoldChatConversion runat="server" id="BoldChatConversion" />
    <eddm:VisualWebsiteOptimizer runat="server" id="VisualWebsiteOptimizer" />
    <eddm:BingConversionCode runat="server" id="BingConversionCode" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

    <div class="container">

        <eddm:PageHeader runat="server" id="PageHeader" />

        <div class="contentWrapper">
           
            <%--User Error panel--%>
            <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="true">
                <i class="fa fa-2x fa-exclamation-circle pull-left"></i>&nbsp;
                <asp:Literal ID="litErrorMessage" runat="server" />
                <p>&nbsp;</p>
            </asp:Panel>


            <%--Debugging--%>
            <asp:Panel ID="pnlDebug" runat="server" CssClass="alert alert-danger" Visible="False">
                
                <h3>TEST MODE</h3>
                
                <p><strong>Cart Content</strong></p>
                <asp:TextBox ID="txtCart" runat="server" TextMode="MultiLine" cssclass="form-control" Rows="12"></asp:TextBox>

                <p>&nbsp;</p>

                <p><strong>Order Calculator Properties</strong></p>
                <asp:GridView ID="gvOrderCalcProperties" runat="server" BackColor="White" Width="100%"></asp:GridView>

                <p>&nbsp;</p>

                <p><strong>Product</strong></p>
                <asp:GridView ID="gvProducts" runat="server" BackColor="WhiteSmoke"></asp:GridView>

                <p>&nbsp;</p>

                <p><strong>Page Properties</strong></p>

                <table class="table table-bordered table-striped table-condensed">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Value</th>
                        </tr>
                    </thead>

                    <tbody>

                        <tr>
                            <td>DesignFee</td>
                            <td><asp:Literal ID="litDesignFee" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>Distribution ID</td>
                            <td><asp:Literal ID="litDistributionID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>DropFee</td>
                            <td><asp:Literal ID="litDropFee" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>EDDMMap</td>
                            <td><asp:Literal ID="litEDDMMap" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>Environment Mode</td>
                            <td><asp:Literal ID="litEnvironmentMode" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>ExtraCopies</td>
                            <td><asp:Literal ID="litExtraCopies" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>GeneratedAddressedList</td>
                            <td><asp:Literal ID="litGeneratedAddressedList" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>HasDropFee</td>
                            <td><asp:Literal ID="litHasDropFee" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>IsMultipleImpression</td>
                            <td><asp:Literal ID="litIsMultipleImpression" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>IsProfessionalDesign</td>
                            <td><asp:Literal ID="litIsProfessionalDesign" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>MailPieces</td>
                            <td><asp:Literal ID="litMailPieces" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>New Mover Product ID</td>
                            <td><asp:Literal ID="litNewMoverProductID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>NewMoverSelected</td>
                            <td><asp:Literal ID="litNewMoverSelected" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>NumEmailBlasts</td>
                            <td><asp:Literal ID="litNumEmailBlasts" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>NumberOfDrops</td>
                            <td><asp:Literal ID="litNumberOfDrops" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>OrderGuid</td>
                            <td><asp:Literal ID="litOrderGuid" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>Order ID</td>
                            <td><asp:Literal ID="litOrderID" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>OrderTotal</td>
                            <td><asp:Literal ID="litTestModeOrderTotal" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>ProductType</td>
                            <td><asp:Literal ID="litProductType" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>Site ID</td>
                            <td><asp:Literal ID="litSiteID" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>Targeted Email Product ID</td>
                            <td><asp:Literal ID="litTargetedEmailProductID" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>TargetedEmailsSelected</td>
                            <td><asp:Literal ID="litTargetedEmailsSelected" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>TestMode</td>
                            <td><asp:Literal ID="litTestMode" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>TMCMap</td>
                            <td><asp:Literal ID="litTMCMap" runat="server" /></td>
                        </tr>

                        <tr>
                            <td>UploadedAddressedList</td>
                            <td><asp:Literal ID="litUploadedAddressedList" runat="server" /></td>
                        </tr>

                        <tr class="lightGray">
                            <td>USelect ID</td>
                            <td><asp:Literal ID="litUSelectID" runat="server" /></td>
                        </tr>

                    </tbody>

                </table>

            </asp:Panel>


            <%--This is for Staples. Site #93--%>
            <eddm:KenshooTrackingPixels runat="server" id="KenshooTrackingPixels" />

            <%--These are for EDDM. Site #1--%>
            <eddm:FacebookTrackingPixel runat="server" id="FacebookTrackingPixel" />


            <%--Normal Display--%>
            <asp:Panel ID="pnlReceiptSuccess" runat="server" Visible="True">

                <%--Header information--%>
                <section id="HeaderSection">
                    
                    <%--Taradel Info--%>
                    <div class="row">
                        <div class="col-xs-12">
                            <div>
                                <eddm:ReceiptHeaderImage runat="server" id="ReceiptHeaderImage" />
                            </div>
                            <p><strong><asp:Literal ID="lReceiptPartnerName" runat="server" Visible="false" />
                            <asp:Literal runat="server" ID="lReceiptPartnerAddress" />
                            <asp:Literal ID="lReceiptPartnerPhone" runat="server" /></strong></p>
                        </div>
                    </div>
                    
                    <div class="catRow">
                        <div class="catRowHeading">Order Information</div>
                    </div>
                    
                    <div class="row">
                        <div class="col-xs-6"><strong><asp:Literal ID="litSoldTo" runat="server" /></strong></div>
                        <div class="col-xs-6"><div class="pull-right"><strong><asp:Literal ID="lReceiptDate" runat="server" /></strong></div></div>
                    </div>   
                
                    <div class="row">
                        <div class="col-xs-6"><strong><asp:Literal ID="litSoldToCompanyName" runat="server" /></strong></div>
                        <div class="col-xs-6"><div class="pull-right"><strong><asp:Literal ID="lReceiptNumber" runat="server" /></strong></div></div>
                    </div>   
                
                    <div class="row">
                        <div class="col-xs-6"><strong><asp:Literal ID="litSoldToAddress" runat="server" /></strong></div>
                        <div class="col-xs-6"><div class="pull-right"><strong><asp:Literal ID="lOrderNum" runat="server" /></strong></div></div>
                    </div>   
                
                    <div class="row">
                        <div class="col-xs-6"><strong><asp:Literal ID="litSoldToCityStateZip" runat="server" /></strong></div>
                        <div class="col-xs-6"><div class="pull-right"><strong><asp:HyperLink ID="hyperlinkGetPDF" runat="server" Target="_blank" Text="View as PDF/Print"></asp:HyperLink></strong></div></div>
                    </div>   
                        
                    <div class="row">
                        <div class="col-xs-6"><strong><asp:Literal ID="litSoldToPhone" runat="server" /></strong></div>
                        <div class="col-xs-6">&nbsp;</div>
                    </div>   
                           
                    <div class="row">
                        <div class="col-xs-6"><strong><asp:Literal ID="litCustNumLabel" runat="server" />:&nbsp;<asp:Literal ID="litCustomerID" runat="server" /></strong></div>
                        <div class="col-xs-6"><div class="pull-right"><strong><asp:Literal ID="litJobName" runat="server" Visible="false" /></strong></div></div>
                    </div>
                  
                    <div>&nbsp;</div>

                    <asp:Panel ID="pnlPONumber" runat="server" Visible="True">
                        <div class="row">
                            <div class="col-xs-6"><strong><asp:Literal ID="litPOText" runat="server" /></strong></div>
                            <div class="col-xs-6">&nbsp;</div>
                        </div>
                    </asp:Panel>
                    
                    <asp:Panel ID="pnlStoreNumber" runat="server" Visible="False">
                        <div class="row">
                            <div class="col-xs-6"><strong>Store Number: <asp:Literal ID="litStoreNumber" runat="server" /></strong></div>
                            <div class="col-xs-6">&nbsp;</div>
                        </div>
                    </asp:Panel>

                         
                    <asp:PlaceHolder ID="phJobComments" runat="server" Visible="False">
                        <div>&nbsp;</div>
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Literal ID="litJobComments" runat="server" />
                            </div>
                        </div>
                    </asp:PlaceHolder>
                                                
                    <div>&nbsp;</div>

                </section>


                <%--EDDM Product--%>
                <asp:Panel ID="pnlEDDMProduct" runat="server" Visible="false">

                    <section id="eddmProductSection">
                        <div class="catRow">
                            <div class="catRowHeading"><asp:Literal runat="server" ID="litEDDMProductType" /> Product</div>
                        </div>

                        <div class="row">

                            <div class="col-xs-8">
                                <strong><asp:Literal ID="litEDDMProductName" runat="server" Visible="true" /></strong>
                            </div>

                            <div class="col-xs-4">
                                <asp:Repeater ID="rOpts" runat="server">
                                    <ItemTemplate>
                                        <div class="row">
                                            <div class="col-xs-3"><small><asp:Literal ID="lOptCat" runat="server" Text='<%#Eval("Name") & ":" %>' /></small></div>
                                            <div class="col-xs-9"><small><asp:Literal ID="lOptVal" runat="server" Text='<%#Eval("ValueName") %>' /></small></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                        </div>

                        <div>&nbsp;</div>
                    </section>

                </asp:Panel>


                <%--AddressedList Product--%>
                <asp:Panel ID="pnlAddressedProduct" runat="server" Visible="false">

                    <section id="ProductSection">
                        <div class="catRow">
                            <div class="catRowHeading"><asp:Literal runat="server" ID="litAddressedProductType" /> Product</div>
                        </div>

                        <div class="row">

                            <div class="col-xs-8">
                                <strong><asp:Literal ID="litAddressedProductName" runat="server" Visible="true" /></strong>
                            </div>

                            <div class="col-xs-4">
                                <asp:Repeater ID="rAddressedOpts" runat="server">
                                    <ItemTemplate>
                                        <div class="row">
                                            <div class="col-xs-3"><small><asp:Literal ID="lOptCat" runat="server" Text='<%#Eval("Name") & ":" %>' /></small></div>
                                            <div class="col-xs-9"><small><asp:Literal ID="lOptVal" runat="server" Text='<%#Eval("ValueName") %>' /></small></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                        </div>

                        <div>&nbsp;</div>
                    </section>

                </asp:Panel>


                <%--Design Type--%>
                <section id="DesignSection">
                    <div class="catRow">
                        <div class="catRowHeading">Design</div>
                    </div>

                    <asp:PlaceHolder ID="phProDesign" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-xs-8">
                                <strong>Design Option:</strong>
                            </div>
                            <div class="col-xs-4">
                                Professional Design Service
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phMyDesign" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-xs-4">
                                <strong><asp:Label ID="lblDesignType" runat="server" Text="" /></strong>
                            </div>

                            <div class="col-xs-4">
                                <asp:Image ID="imgFile1" runat="server" Visible="false" CssClass="img-responsive" />
                                <asp:Literal ID="lLaterMsg" runat="server" /> 
                            </div>

                            <div class="col-xs-4">
                                <asp:Image ID="imgFile2" runat="server" Visible="false" CssClass="img-responsive" />
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <div>&nbsp;</div>
                </section>


                <%--Map--%>
                <section id="MapSection">
                    <div class="catRow">
                        <div class="catRowHeading">Map</div>
                    </div>
                
                    <div class="row">
                        <div class="col-xs-4">
                            <asp:Image ID="imgMap" runat="server" CssClass="img-thumbnail img-responsive receiptMap" />
                        </div>
                        <div class="col-xs-4">
                            <strong>Map Name</strong><br />
                            <asp:Literal ID="litSelectName" runat="server" />
                        </div>
                        <div class="col-xs-4">
                            <strong>Route Description</strong><br />
                            <asp:Literal ID="litSelectDescription" runat="server" />
                        </div>
                    </div>

                    <div>&nbsp;</div>
                </section>


                <%--EDDM Drops--%>
                <asp:Panel ID="pnlEDDMDrops" runat="server" Visible="False">

                    <section id="DropsSection">
                        <div class="catRow">
                            <div class="catRowHeading">EDDM Drops</div>
                        </div>

                        <div>&nbsp;</div>

                        <div class="row">
                            <div class="col-xs-12">

                                <asp:Repeater ID="rptEddmDrops" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-hover table-bordered table-condensed" id="tblDrops">
                                            <thead>
                                                <tr>
                                                    <th class="col-xs-1"><asp:Literal ID="litDropDate" runat="server" Text="Drop Date" /></th>
                                                    <th class="col-xs-1"><asp:Literal ID="litDropNum" runat="server" Text="Drop Number" /></th>
                                                    <th class="col-xs-1"><asp:Literal ID="litPiecesInDrop" Text="Pieces" runat="server" /></th>
                                                    <th class="col-xs-9"><asp:Literal ID="litRoutes" runat="server" Text="Routes" /></th>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="col-xs-1">
                                                <asp:Literal ID="lDropDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date", "{0:MM/dd/yyyy}")%>' />
                                            </td>
                                            <td class="col-xs-1">
                                                <asp:Literal ID="lDropNumber" runat="server" Text='<%#(Eval("Number"))%>' />
                                            </td>
                                            <td class="col-xs-1">
                                                <asp:Literal ID="lDropCount" runat="server" Text='<%#Integer.Parse(Eval("Total")).ToString("N0")%>' />
                                            </td>
                                            <td class="col-xs-9">
                                                <asp:Repeater ID="rptEddmRoutes" runat="server">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="lRoute" runat="server" Text='<%#Eval("Name") %>' />
                                                    </ItemTemplate>
                                                    <SeparatorTemplate>
                                                        ,&nbsp;
                                                    </SeparatorTemplate>
                                                 </asp:Repeater>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>

                            </div>
                        </div>

                        <asp:PlaceHolder ID="phLockedRoutesMsg" runat="server" Visible="False">
                            <div class="row">
                                <div class="col-sm-12">
                                    <h5 class="text-center"><asp:Literal ID="litLockedRoutes" runat="server" /></h5>
                                </div>
                            </div>
                        </asp:PlaceHolder>

                        <div>&nbsp;</div>

                    </section>

                </asp:Panel>


                <%--Addressed Drops--%>
                <asp:Panel ID="pnlAddressedDrops" runat="server" Visible="False">
                    <section id="addressedDropsBlock">

                        <div class="catRow">
                            <div class="catRowHeading">Addressed List Drops</div>
                        </div>

                        <div><p><small>Here are the details regarding your AddressedList campaign:</small></p></div>

                        <div class="row">

                            <div class="col-sm-10 col-sm-offset-1">
                                <table class="table table-striped table-bordered table-condensed" id="tblDrops">
                                    <thead>
                                        <tr>
                                            <td class="col-sm-3">Pieces</td>
                                            <td class="col-sm-3">Drop Dates</td>
                                            <td class="col-sm-6">Filters</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="col-sm-3"><asp:Literal ID="litAddressedPcs" runat="server" /></td>
                                            <td class="col-sm-3"><asp:Literal ID="litAddressedDropDates" runat="server" /></td>
                                            <td class="col-sm-6"><asp:Literal ID="litDemographicFilters" runat="server" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>


                        </div>

                        <div>&nbsp;</div>

                    </section>
                </asp:Panel>


                <%--Campaign Overview. Optional.  Only visible if any add-ons are selected.--%>
                <asp:Panel ID="pnlCampaignOverview" runat="server" Visible="false">
                    <section id="OverviewSection">

                        <div class="catRow">
                            <div class="catRowHeading">Campaign Overview and Schedule</div>
                        </div>

                        <div>&nbsp;</div>

                        <asp:Repeater ID="rptSchedule" runat="server">
                            <HeaderTemplate>
                                <table class="table table-striped table-hover table-bordered table-condensed" id="scheduleTable">
                                    <thead>
                                        <tr>
                                            <th class="col-xs-1">Start Date</th>
                                            <th class="col-xs-1">Quantity</th>
                                            <th class="col-xs-1">Type</th>
                                            <th class="col-xs-9">Routes</th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("StartDate").ToShortDateString() %></td>
                                    <td><%#(Eval("Quantity"))%> </td>
                                    <td><%#Eval("Type") %></td>
                                    <td><%#Eval("Routes") %></td>
                                </tr>
                            </ItemTemplate>

                            <FooterTemplate>
                                </table>
                            </FooterTemplate>

                        </asp:Repeater>

                        <div>&nbsp;</div>

                    </section>

                </asp:Panel>
              
                  
                <div>&nbsp;</div>
                
                <%--Mailing--%>
                <section id="MailingSection">
                    <div class="catRow">
                        <div class="row">
                            <div class="col-xs-4 catRowHeading"><asp:Literal ID="lMailing" runat="server" Text="Mailing" /></div>
                            <div class="col-xs-4 text-center catRowDetails"><asp:Literal ID="litNumOfPcs" runat="server" /></div>
                            <div class="col-xs-4 text-right catRowHeading"><strong><asp:Literal ID="lPriceOfPieces" runat="server" /></strong></div>
                        </div>
                    </div>

                    <div>&nbsp;</div>
                </section>


                <%--Extra Pcs. Optional --%>
                <asp:Panel ID="pnlExtraPcs" runat="server" Visible="false">
                    <section id="ExtraPcsSection">
                        
                        <div class="catRow">
                        <div class="row">

                            <div class="col-xs-4 catRowHeading">
                                <strong>Additional Pieces</strong>
                            </div>

                            <div class="col-xs-4 text-center catRowDetails">
                                <strong><asp:Literal ID="litExtraQuantity" runat="server" /> @  <asp:Literal ID="litExtraPricePerPiece" runat="server" /></strong><br />
                                Shipping To:<br />
                                <asp:Literal ID="litExtraQuantityAddress" runat="server" />
                            </div>

                            <div class="col-xs-4 text-right catRowHeading">
                                <asp:Label ID="lblExtraPrice" runat="server" />
                            </div>

                        </div>
                    </div>

                        <div>&nbsp;</div>

                    </section>
                </asp:Panel>
                

                 <%--Design Fee. Optional --%>
                <asp:Panel ID="pnlDesignFee" runat="server" Visible="false">
                    <section id="DesignFeeSection">
                    
                        <div class="catRow">

                            <div class="row">
                                <div class="col-xs-6 catRowHeading">
                                    Professional Design Fee
                                </div>
                                <div class="col-xs-6 text-right catRowHeading">
                                    <asp:Label ID="lblDesignFee" runat="server" />
                                </div>
                            </div>

                        </div>

                        <div>&nbsp;</div>

                    </section>
                </asp:Panel>


                <%--Additional Drop Fee. Optional --%>
                <asp:Panel ID="pnlNumOfDrops" runat="server" Visible="false">
                    <section id="AddtionalDropFeeSection">
                    
                        <div class="catRow">

                            <div class="row">

                                <div class="col-xs-4 catRowHeading">
                                    Additional Drops Fee
                                </div>

                                <div class="col-xs-4 text-center catRowDetails">
                                    <asp:Literal ID="litNumOfDrops" runat="server" />
                                </div>

                                <div class="col-xs-4 text-right catRowHeading">
                                    <strong><asp:Label ID="lblDropFee" runat="server" /></strong>
                                </div>

                            </div>

                        </div> 
                            
                        <div>&nbsp;</div>
                    </section>
                </asp:Panel>
                

                <%--New Movers. Optional --%>
                <asp:Panel ID="pnlNewMovers" runat="server" Visible="false">
                    <section id="NewMoversSection">

                        <div class="catRow">
                        
                            <div class="row">

                                <div class="col-xs-4 catRowHeading">
                                    New Mover Campaign
                                </div>

                                <div class="col-xs-4 text-center catRowDetails">
                                    <asp:Literal ID="litNewMoverDescription" runat="server" />
                                </div>

                                <div class="col-xs-4 text-right catRowHeading">
                                    <strong><asp:Literal ID="litNewMoverPrice" runat="server" /></strong>
                                </div>

                            </div>

                        </div>

                        <div>&nbsp;</div>

                    </section>
                </asp:Panel>
                

                <%--Emails. Optional --%>
                <asp:Panel ID="pnlEmails" runat="server" Visible="false">
                    <section id="EmailsSection">
                    
                        <div class="catRow">
                        
                            <div class="row">

                                <div class="col-xs-4 catRowHeading">
                                    Targeted Email Campaign
                                </div>

                                <div class="col-xs-4 text-center catRowDetails">
                                    <asp:Literal ID="litEmailDescription" runat="server" />
                                </div>

                                <div class="col-xs-4 text-right catRowHeading">
                                    <strong><asp:Literal ID="litEmailPrice" runat="server" /></strong>
                                </div>

                            </div>

                        </div>
                            
                        <div>&nbsp;</div>

                    </section>
                </asp:Panel>


                <%--Addressed Add-ons. Optional --%>
                <asp:Panel ID="pnlAddressedAddOns" runat="server" Visible="false">
                        <section id="addressedAddOnsBlock">

                            <div class="catRow">
                        
                                <div class="row">

                                    <div class="col-sm-4 catRowHeading">
                                        Addressed Add-On Campaign
                                    </div>

                                    <div class="col-sm-4 text-center catRowDetails">
                                        <asp:Literal ID="litAddressedAddOnsDescription" runat="server" />
                                    </div>

                                    <div class="col-sm-4 text-right catRowHeading">
                                        <strong><asp:Literal ID="litAddressedAddOnsPrice" runat="server" /></strong>
                                    </div>

                                </div>

                            </div>
                            
                            <div>&nbsp;</div>

                        </section>
                    </asp:Panel>
                

                <%--Finance Fees. Optional --%>
                <asp:Panel ID="pnlFinanceFees" runat="server" Visible="false">
                    <section id="FinanceSection">

                        <div class="catRow">
                        
                            <div class="row">

                                <div class="col-xs-4 catRowHeading">
                                    Finance Fee
                                </div>

                                <div class="col-xs-4 text-center catRowDetails">
                                    <asp:Literal ID="lFinanceFeeDetail" runat="server" />
                                </div>

                                <div class="col-xs-4 text-right catRowHeading">
                                    <strong><asp:Literal ID="lFinanceFee" runat="server" /></strong>
                                </div>

                            </div>

                        </div>
                            
                        <div>&nbsp;</div>

                    </section>
                </asp:Panel>
                

                <%--Sub Total--%>
                <section id="SubTotalSection">
                    <div class="catRow">
                        <div class="row">
                            <div class="col-xs-6 catRowHeading">Subtotal</div>
                            <div class="col-xs-6 text-right catRowHeading"><asp:Literal ID="litSubTotal" runat="server" /></div>
                        </div>
                    </div>

                    <div>&nbsp;</div>
                </section>


                <%--Coupon. Optional --%>
                <asp:Panel ID="pnlCouponDiscount" runat="server" Visible="false">

                    <section id="CouponSection">

                        <div class="catRow">
                        
                            <div class="row">

                                <div class="col-xs-4 catRowHeading">
                                    Discounts
                                </div>

                                <div class="col-xs-4 text-center catRowDetails">
                                    <asp:Literal ID="litCouponName" runat="server" />
                                </div>

                                <div class="col-xs-4 text-right catRowHeading">
                                    <strong><asp:Literal ID="lCouponDiscount" runat="server" /></strong>
                                </div>

                            </div>

                        </div>
                            
                        <div>&nbsp;</div>

                    </section>

                </asp:Panel>
                

                <%--Sales Tax. Optional --%>
                <asp:Panel ID="pnlSalesTax" runat="server" Visible="false">
                    <section id="SalesTaxSection">
                        
                        <div class="catRow">
                            <div class="row">
                                <div class="col-xs-4 catRowHeading"><asp:Literal ID="Literal1" runat="server" Text="Sales Tax" /></div>
                                <div class="col-xs-4 text-center catRowDetails"><asp:Label ID="lblSalesTaxMessage" runat="server" /></div>
                                <div class="col-xs-4 text-right catRowHeading"><strong><asp:Label ID="lblSalesTax" runat="server" /></strong></div>
                            </div>
                        </div>

                        <div>&nbsp;</div>  
                           
                    </section>                       
                </asp:Panel>
                

                <%--Total--%>
                <section id="TotalSection">
                    <div class="catRow">
                        <div class="row">
                            <div class="col-xs-4 catRowHeading">Total</div>
                            <div class="col-xs-4 text-center"><asp:Literal ID="litSalesTaxDisclaimer" runat="server" /></div>
                            <div class="col-xs-4 text-right grandTotal"><asp:Label ID="litOrderTotal" runat="server" /></div>
                        </div>
                    </div>

                    <div>&nbsp;</div>
                </section>


                <%--Payment--%>
                <asp:Panel ID="pnlPaymentInfo" runat="server">
                    <section id="PaymentSection">

                        <div class="catRow">
                            <div class="catRowHeading">Payment Information</div>
                        </div>

                        <div class="row">
                            <div class="col-xs-4">
                                <strong>Payments/Credits</strong>
                            </div>
                            <div class="col-xs-4">
                                &nbsp;
                            </div>
                            <div class="col-xs-4">
                                <div class="text-right">
                                    <strong><asp:Literal runat="server" ID="lPaymentsCredits" /></strong>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-4">
                                <strong>Balance</strong>
                            </div>
                            <div class="col-xs-4">
                                &nbsp;
                            </div>
                            <div class="col-xs-4">
                                <div class="text-right">
                                    <strong><asp:Literal runat="server" ID="lRemainingBalance" /></strong>
                                </div>
                            </div>
                        </div>

                        <div>&nbsp;</div>

                    </section>
                </asp:Panel>


                <%--Payment Message--%>
                <div class="well well-xs">
                    <p class="lead text-center"><asp:Literal ID="lPaymentMessage" runat="server" /></p>
                </div>
                

                <%--Payments--%>
                <section id="PaymentsSection">

                    <div class="row">
                        <div class="col-xs-10 col-sm-offset-1">
                            <asp:Repeater ID="rPayments2" runat="server">
                                <HeaderTemplate>

                                    <h5>Scheduled Payments</h5>

                                    <table class="table table-striped table-hover table-bordered table-condensed" id="tblPayments">
                                        <thead>
                                            <tr>
                                                <th class="width25">Bill Date</th>
                                                <th class="width25">Drop Date</th>
                                                <th class="width25">Payment</th>
                                                <th class="width25">Balance<br />(after payment)</th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <tr>
                                        <td class="width25"><asp:Literal ID="lBillDate" runat="server" Text='<%#DateTime.Parse(Eval("BillDate")).ToString("MM/dd/yyyy")%>'  /></td>
                                        <td class="width25"><asp:Literal ID="lDropDate" runat="server" Text='<%#DateTime.Parse(Eval("DropDate")).ToString("MM/dd/yyyy")%>' /></td>
                                        <td class="width25"><asp:Literal ID="lDropCount" runat="server" Text='<%#Decimal.Parse(Eval("Payment").ToString()).ToString("C")%>' /></td>
                                        <td class="width25"><asp:Literal ID="lBalance" runat="server" Text='<%#Decimal.Parse(Eval("Balance").ToString()).ToString("C")%>' /></td>
                                    </tr>
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>

                            <%--Obsolete Repeater--%>
                            <asp:Repeater ID="rPayments" runat="server" Visible="false">
                                <ItemTemplate>
                                    <div>
                                        <%#Taradel.Util.NumberHelp.FormatTextCount(Container.ItemIndex + 2)%>
                                         Payment <asp:Literal ID="lPayDate" runat="server" Text='<%#DateTime.Parse(Eval("Key")).ToString("dddd, dd MMM yyyy") %>' />:&nbsp;
                                        <strong> <asp:Literal ID="lPayAmount" runat="server" Text='<%#Decimal.Parse(Eval("Value").ToString()).ToString("C") %>' /></strong>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                        </div>
                    </div>

                    <div>&nbsp;</div>

                </section>


                <%--Disclaimers--%>
                <section id="disclaimerBlock">

                    <asp:PlaceHolder ID="phNextSteps" runat="server" Visible="false">
                        <div>
                            <p><strong>Next Steps: </strong>Our expert team will review your order and contact you shortly. No further action is required from you at this time.
                            If you have any questions, contact our help line at <asp:Literal ID="lPhoneNumber" runat="server" />.</p>
                            <p>&nbsp;</p>
                        </div>
                    </asp:PlaceHolder>
                    
                    <%--Needs styling?--%>
                    <asp:PlaceHolder ID="phEarliest" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-xs-4">
                                <strong>Design Option:</strong>
                            </div>
                            <div class="col-xs-8">
                                <strong>Professional Design Service</strong>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-4">
                                <strong>Drop Date:</strong>
                            </div>
                            <div class="col-xs-8">
                                Based on your selections, your pieces will be presented at the post office as early as <asp:Label ID="EarliestDelivery" runat="server" />.
                            </div>
                        </div>
                    </asp:PlaceHolder>

                </section>


                <%--Custom Footer Imgs--%>
                <asp:Panel ID="pnlStaplesFooter" runat="server" Visible="False">

                    <table class="centered95Width" id="staplesReceiptFooterTbl">
                        <tr>
                            <td colspan="2"><h4 class="text-center"><strong><u>Product</u></strong></h4></td>
                            <td></td>
                            <td colspan="2"><h4 class="text-center"><strong><u>Postage</u></strong></h4></td>
                        </tr>

                        <tr>
                            <td class="width20"><h4><strong>1) Scan</strong></h4></td>
                            <td class="width30"><asp:Image ID="imgStaplesProduct" runat="server" /><br /><br /></td>
                            <td class="width1px"></td>
                            <td class="width20"><h4><strong>3) Scan</strong></h4></td>
                            <td class="width30"><asp:Image ID="imgStaplesPostage" runat="server" /><br /><br /></td>
                        </tr>

                        <tr>
                            <td class="width20"><h4><strong>2) Type at POS:</strong></h4></td>
                            <td class="width30"><span class="checkoutPrice"><strong><asp:Label ID="lblStaplesProductCost" runat="server" /></strong></span></td>
                            <td class="width1px"></td>
                            <td class="width20"><h4><strong>4) Type at POS:</strong></h4></td>
                            <td class="width30"><span class="checkoutPrice"><strong><asp:Label ID="lblStaplesPostage" runat="server" /></strong></span></td>
                        </tr>

                    </table>

                </asp:Panel>

                <div>&nbsp;</div>
                <div>&nbsp;</div>

            </asp:Panel>


        </div>

    </div>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">

    <asp:Literal ID="litGoogleRevenueScript" runat="server" />
    <asp:Literal ID="litShopperApprovedScript" runat="server" />

</asp:Content>

