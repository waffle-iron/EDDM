<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="Step3-Checkout.aspx.vb" Inherits="Step3_Checkout" Trace="false" %>
<%@ Register Src="~/Controls/OrderSteps.ascx" TagPrefix="appx" TagName="OrderSteps" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="eddm" TagName="PageHeader" %>




<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">
    <section id="PageContent">
        
        <div class="container">

            <eddm:PageHeader runat="server" id="PageHeader" />

            <div class="contentWrapper">

                <%--Debugging--%>
                <asp:Panel ID="pnlDebug" runat="server" CssClass="hidden">

                    <h3>TEST MODE</h3>

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
                                <td class="width50">AddressedShipPrice</td>
                                <td class="width50"><asp:Literal ID="litAddressedShipPrice" runat="server" /></td>
                            </tr>

                            <tr class="lightGray">
                                <td class="width50">BaseProductID</td>
                                <td class="width50"><asp:Literal ID="litBaseProductID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">CouponDiscount</td>
                                <td class="width50"><asp:Literal ID="litCouponDiscount" runat="server" /></td>
                            </tr>

                            <tr class="lightGray">
                                <td class="width50">DesignFee</td>
                                <td class="width50"><asp:Literal ID="litDesignFee" runat="server" /></td>
                            </tr>


                            <tr>
                                <td class="width50">DistributionID</td>
                                <td class="width50"><asp:Literal ID="litDistributionID" runat="server" /></td>
                            </tr>

                            <tr class="lightGray">
                                <td class="width50">DropFee</td>
                                <td class="width50"><asp:Literal ID="litDropFee" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">EDDMMap</td>
                                <td class="width50"><asp:Literal ID="litEDDMMap" runat="server" /></td>
                            </tr>


                            <tr class="lightGray">
                                <td class="width50">EDDMShipPrice</td>
                                <td class="width50"><asp:Literal ID="litEDDMShipPrice" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">EmailCampaignSelected</td>
                                <td class="width50"><asp:Literal ID="litEmailCampaignSelected" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">AddressedAddOnSelected</td>
                                <td class="width50"><asp:Literal ID="litAddressedAddOnSelected" runat="server" /></td>
                            </tr>

                            <tr class="lightGray">
                                <td class="width50">EnvironmentMode</td>
                                <td class="width50"><asp:Literal ID="litEnvironmentMode" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">ExtraCopies</td>
                                <td class="width50"><asp:Literal ID="litExtraCopies" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">GeneratedAddressedList</td>
                                <td class="width50"><asp:Literal ID="litGeneratedAddressedList" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">HasActiveCoupons</td>
                                <td class="width50"><asp:Literal ID="litHasActiveCoupons" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">HasDropFee</td>
                                <td class="width50"><asp:Literal ID="litHasDropFee" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">IsProfessionalDesign</td>
                                <td class="width50"><asp:Literal ID="litIsProfessionalDesign" runat="server" /></td>
                            </tr>

                            <tr class="lightGray">
                                <td class="width50">MerchantID</td>
                                <td class="width50"><asp:Literal ID="litMerchantId" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">NewMoverSelected</td>
                                <td class="width50"><asp:Literal ID="litNewMoverSelected" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">NumOfDrops</td>
                                <td class="width50"><asp:Literal ID="litNumOfDrops2" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">PostageRate</td>
                                <td class="width50"><asp:Literal ID="litPostageRate" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">ProductID</td>
                                <td class="width50"><asp:Literal ID="litProductID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">ProductType</td>
                                <td class="width50"><asp:Literal ID="litProductType" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">SalesTax</td>
                                <td class="width50"><asp:Literal ID="litSalesTax" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">SiteID</td>
                                <td class="width50"><asp:Literal ID="litSiteID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">TestMode</td>
                                <td class="width50"><asp:Literal ID="litTestMode" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">TMCMap</td>
                                <td class="width50"><asp:Literal ID="litTMCMap" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">TransactionID</td>
                                <td class="width50"><asp:Literal ID="litTransactionID" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">UploadedAddressedList</td>
                                <td class="width50"><asp:Literal ID="litUploadedAddressedList" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">UseOwnGateway</td>
                                <td class="width50"><asp:Literal ID="litUseOwnGateway" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50">USelectID</td>
                                <td class="width50"><asp:Literal ID="litUSelectID" runat="server" /></td>
                            </tr>

                        </tbody>

                    </table>

                    <p><strong>OrderCalculator Obj</strong></p>
                    <asp:GridView ID="gvOrderCalc" runat="server" BackColor="White" Visible="false"></asp:GridView>

                    <p>&nbsp;</p>

                    <div class="row">
                        <div class="col-sm-12">
                            Lock the Routes: <asp:CheckBox Text="Lock the Routes" runat="server" ID="chkLocktheRoutes" />
                        </div>
                    </div>

                    <p><strong>Products:</strong></p>
                    <asp:GridView ID="gvProducts" runat="server" BackColor="WhiteSmoke" Visible="False"></asp:GridView>

                    <p>&nbsp;</p>

                    <div><asp:Literal ID="litCampaignSchedule" runat="server" /></div>

                    <div>&nbsp;</div>

                    <div><asp:Literal ID="litGeneralError" runat="server" /></div>

                </asp:Panel>

                <%--Error--%>
                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                    <p>&nbsp;</p>
                    <span class="fa fa-2x fa-exclamation-circle text-danger"></span>&nbsp;
                    <asp:Literal ID="litErrorMessage" runat="server" />
                    <p>&nbsp;</p>
                    <p>&nbsp;</p>
                </asp:Panel>

                <%--Empty Cart--%>
                <asp:Panel ID="pEmpty" runat="server" Visible="false">

                    <h4 class="text-center text-primary">Your cart is empty.</h4>

                    <div>&nbsp;</div>

                    <div class="alert alert-info" role="alert">
                        <p><strong>Your ordering options are:</strong></p>

                        <ul>
                            <li><asp:HyperLink ID="hplNewUSelect" runat="server" NavigateUrl="~/Step1-Target.aspx">Start a new U-Select Project</asp:HyperLink></li>
                            <li><asp:HyperLink ID="hplExistingProject" runat="server" NavigateUrl="~/myaccount/account_selects.aspx">Open an existing U-Select Project</asp:HyperLink></li>
                            <li><asp:HyperLink ID="hplTemplates" runat="server" NavigateUrl="~/Templates">Explore our Template Library</asp:HyperLink></li>
                        </ul>
                    </div>
                
                    <div>&nbsp;</div>

                </asp:Panel>

                <%--Checkout--%>
                <asp:Panel ID="pCheckout" runat="server" Visible="True">

                    <asp:Literal ID="lCheckoutMsg" runat="server" />

                    <asp:ValidationSummary ID="vsCheckout" CssClass="alert alert-danger" runat="server" ValidationGroup="vgCheckout" HeaderText="Please correct the following to complete your order:" />
                    <asp:ValidationSummary ID="vsCCPayment" CssClass="alert alert-danger" runat="server" ValidationGroup="vgCCPay" HeaderText="Your payment option is missing some required information:" />
                    <asp:ValidationSummary ID="vsECPayment" CssClass="alert alert-danger" runat="server" ValidationGroup="vgECPay" HeaderText="Your payment option is missing some required information:" />
                    <asp:ValidationSummary ID="vsPOPayment" CssClass="alert alert-danger" runat="server" ValidationGroup="vgPOPay" HeaderText="Your payment option is missing some required information:" />
  

                    <%--IS this whole section obsolete?--%>
                    <div>
                        <div>


                            <%--Obsolete?--%>
                            <div runat="server" visible="false">
                                <div>
                                    <img src="/cmsimages/order-review.png" height="125" width="125" alt="Order Review Guarantee" />
                                </div>

                                <div>
                                    <h2>Buy with Confidence</h2>

                                    <p>Every order is reviewed by an Every Door Direct Mail expert. We will never print
                                    or mail your order without contacting you first. All artwork is reviewed by a graphic
                                    artist prior to printing.</p>

                                    <div>
                                        <ul>
                                            <li>Instant receipt by email</li>
                                            <li>Order confirmation by phone</li>
                                            <li>Artwork review prior to production</li>
                                            <li>Access to 24/7 phone support</li>
                                        </ul>
                                    </div>

                                    <div>
                                        <p><strong>If you selected a Free Template or Professional Design Service, you will be contacted within one to 
                                        two business days of placing your order.</strong></p>
                                    </div>
                                </div>
                            </div>


                            <%--Obsolete?--%>
                            <div>
                                <div>

                                    <%--Obsolete?--%>
                                    <div runat="server" visible="false" id="legacyQty">
                                        <div>Quantity:</div>
                                        <div>
                                            <asp:Literal ID="lQty" runat="server" />
                                            <asp:Literal runat="server" ID="lQtyDM" Visible="False"> pieces to direct mail</asp:Literal>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>

                    <appx:OrderSteps runat="server" id="OrderSteps" />

                    <p>Below are the details of your entire order.  This information will also be included in your receipt.  <strong>Be sure to 
                    scroll all the way to the bottom of the page</strong> where you provide your credit card information to finalize your campaign.</p>

                    <a id="paymentArea"></a>

                    <%--EDDM Product--%>
                    <asp:Panel ID="pnlEddmProduct" runat="server" Visible="False">
                        <section id="eddmProductBlock">
                        
                            <div class="catRow">
                                <div class="catRowHeading">EDDM Product</div>
                            </div>

                            <div>&nbsp;</div>

                            <div class="row">

                                <div class="col-sm-8">
                                    <strong><asp:Literal ID="litEddmProductName" runat="server" Visible="true" /></strong>
                                </div>

                                <div class="col-sm-4">
                                    <asp:Repeater ID="rEddmOpts" runat="server">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-xs-3"><small><asp:Literal ID="lOptCat" runat="server" Text='<%#Eval("Name") & ": "%>' /></small></div>
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
                    <asp:Panel ID="pnlAddressedProduct" runat="server" Visible="False">
                    <section id="addressedProductBlock">
                        
                        <div class="catRow">
                            <div class="catRowHeading">AddressedList Product</div>
                        </div>

                        <div>&nbsp;</div>

                        <div class="row">

                            <div class="col-sm-8">
                                <strong><asp:Literal ID="litAddressedProductName" runat="server" Visible="true" /></strong>
                            </div>

                            <div class="col-sm-4">
                                <asp:Repeater ID="rptAddressedOpts" runat="server">
                                    <ItemTemplate>
                                        <div class="row">
                                            <div class="col-xs-3"><small><asp:Literal ID="lOptCat" runat="server" Text='<%#Eval("Name") & ": "%>' /></small></div>
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
                    <section id="designBlock">
                        <div class="catRow">
                            <div class="catRowHeading">Design</div>
                        </div>

                        <div>&nbsp;</div>

                        <asp:PlaceHolder ID="phProDesign" runat="server" Visible="false">
                            <div class="row">
                                <div class="col-sm-8">
                                    <strong>Design Option:</strong>
                                </div>
                                <div class="col-sm-4">
                                    Professional Design Service
                                </div>
                            </div>
                        </asp:PlaceHolder>

                        <asp:PlaceHolder ID="phMyDesign" runat="server" Visible="false">
                            <div class="row">
                                <div class="col-sm-4">
                                    <strong><asp:Label ID="lblDesignType" runat="server" Text="" /></strong>
                                </div>

                                <div class="col-sm-4">
                                    <asp:Image ID="imgFile1" runat="server" ImageUrl="http://templateserver.taradel.com/templates/icon/12672-front.jpg" CssClass="img-responsive"/> 
                                    <asp:Literal ID="lLaterMsg" runat="server" />
                                </div>

                                <div class="col-sm-4">
                                    <asp:Image ID="imgFile2" runat="server" Visible="false" CssClass="img-responsive" />
                                </div>
                            </div>
                        </asp:PlaceHolder>

                        <div>&nbsp;</div>
                    </section>


                    <%--Map--%>
                    <section id="mapBlock">
                        <div class="catRow">
                            <div class="catRowHeading">Map</div>
                        </div>
                
                        <div>&nbsp;</div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Literal ID="lMapReview" runat="server" />
                            </div>

                            <div class="col-sm-4">
                                <strong><asp:Literal ID="lSelectName" runat="server" Visible="true" /></strong>
                            </div>

                            <div class="col-sm-4">
                                <asp:Literal ID="lSelectDescription" runat="server" />
                            </div>
                        </div>

                        <div>&nbsp;</div>
                    </section>


                    <%--EDDM Drops--%>
                    <asp:Panel ID="pnlEDDMDrops" runat="server" Visible="false">
                        <section id="eddmDropsBlock">

                            <div class="catRow">
                                <div class="catRowHeading">EDDM Drops</div>
                            </div>

                            <div>&nbsp;</div>

                            <div class="row">

                                <div class="col-sm-12">

                                    <asp:Repeater ID="rDrops2" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-striped table-hover table-bordered table-condensed" id="tblDrops">
                                                <thead>
                                                    <tr>
                                                        <th class="col-sm-1"><asp:Literal ID="lDropDateLabel2" runat="server" Text="Drop Date" /></th>
                                                        <th class="col-sm-1"><asp:Literal ID="lDropNumLabel" runat="server" Text="Drop Number" /></th>
                                                        <th class="col-sm-1"><asp:Literal ID="lPiecesInDropLabel" Text="Pieces" runat="server" /></th>
                                                        <th class="col-sm-9"><asp:Literal ID="lRoutesLabel" runat="server" Text="Routes" /></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                
                                        <ItemTemplate>
                                            <tr>
                                                <td class="col-sm-1">
                                                    <asp:Literal ID="lDropDate" runat="server" Text='<%#(Eval("Date"))%>' Visible="true" />
                                                    <asp:DropDownList ID="ddlDropDate" runat="server" Visible="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="col-sm-1"><asp:Literal ID="lDropNumber" runat="server" Text='<%#Eval("Number") %>' /></td>
                                                <td class="col-sm-1"><asp:Literal ID="lDropCount" runat="server" Text='<%#Integer.Parse(Eval("Total")).ToString("N0") %>' /></td>
                                                <td class="col-sm-9">
                                                    <asp:Repeater ID="rRoutes" runat="server">
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
                                            </tbody>
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
                    <asp:Panel ID="pnlAddressedDrops" runat="server" Visible="false">
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
                   

                    <%--Overview. Optional.  Only visible if any add-ons are selected.--%>
                    <asp:Panel ID="pnlCampaignOverview" runat="server" Visible="false">

                        <section id="overviewBlock">
                        
                            <div class="catRow">
                                <div class="catRowHeading">Campaign Overview and Schedule</div>
                            </div>

                            <div>&nbsp;</div>

                            <asp:Repeater ID="rptSchedule" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-striped table-hover table-bordered table-condensed" id="scheduleTable">
                                        <thead>
                                            <tr>
                                                <th class="col-sm-1">Start Date</th>
                                                <th class="col-sm-1">Quantity</th>
                                                <th class="col-sm-1">Type</th>
                                                <th class="col-sm-9">Routes</th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                            
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("StartDate").ToShortDateString() %></td>
                                        <td><%#Eval("Quantity") %> </td>
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


                    <%--Mailing--%>
                    <asp:Panel ID="pnlMailingPrice" runat="server" Visible="True">
                        <section id="mailingBlock">
                        
                            <div class="catRow">
                                <div class="row">
                                    <div class="col-sm-4 catRowHeading"><asp:Literal ID="litMailingTitle" runat="server" /></div>
                                    <div class="col-sm-4 text-center catRowDetails"><asp:Literal ID="litNumOfPcs" runat="server" /></div>
                                    <div class="col-sm-4 text-right catRowHeading"><strong><asp:Label ID="lblPrintingEstimate" runat="server" /></strong></div>
                                </div>
                            </div>

                            <div>&nbsp;</div>
                        
                        </section>
                    </asp:Panel>


                    <%--Extra Pcs. Optional. --%>
                    <asp:Panel ID="pnlExtraPcs" runat="server" Visible="false">
                        <section id="extraPcsBlock">
                        
                            <div class="catRow">
                                <div class="row">

                                    <div class="col-sm-4 catRowHeading">
                                        <asp:Literal ID="lShipping" runat="server" Text="Shipping" />
                                    </div>

                                    <div class="col-sm-4 text-center catRowDetails">
                                        <strong><asp:Literal ID="litExtraPcs" runat="server" /></strong><br />
                                        Shipping To:<br />
                                        <asp:Literal ID="litShipTo" runat="server" />
                                    </div>

                                    <div class="col-sm-4 text-right catRowHeading">
                                        <asp:Label ID="lblShippingEstimate" runat="server" />
                                    </div>

                                </div>
                            </div>

                            <div>&nbsp;</div>

                        </section>
                    </asp:Panel>
                    

                    <%--Design Fee. Optional --%>
                    <asp:Panel ID="pnlDesignFee" runat="server" Visible="false">
                        <section id="designFeeBlock">

                            <div class="catRow">

                                <div class="row">
                                    <div class="col-sm-6 catRowHeading">
                                        Professional Design Fee
                                    </div>
                                    <div class="col-sm-6 text-right catRowHeading">
                                        <asp:Label ID="lblDesignFee" runat="server" />
                                    </div>
                                </div>

                            </div>

                            <div>&nbsp;</div>

                        </section>
                    </asp:Panel>
                    

                    <%--Additional Drop Fee. Optional --%>
                    <asp:Panel ID="pnlNumOfDrops" runat="server" Visible="false">
                        <section id="dropFeeBlock">

                            <div class="catRow">

                                <div class="row">

                                    <div class="col-sm-4 catRowHeading">
                                        Additional Drops Fee
                                    </div>

                                    <div class="col-sm-4 text-center catRowDetails">
                                        <asp:Literal ID="litNumOfDrops" runat="server" />
                                    </div>

                                    <div class="col-sm-4 text-right catRowHeading">
                                        <strong><asp:Label ID="lblNumOfDrops" runat="server" /></strong>
                                    </div>

                                </div>

                            </div> 
                            
                            <div>&nbsp;</div>

                        </section>
                    </asp:Panel>
                    

                    <%--New Movers --%>
                    <asp:Panel ID="pnlNewMovers" runat="server" Visible="false">
                        <section id="newmoversBlock">   
                             
                            <div class="catRow">
                        
                                <div class="row">

                                    <div class="col-sm-4 catRowHeading">
                                        New Mover Campaign
                                    </div>

                                    <div class="col-sm-4 text-center catRowDetails">
                                        <asp:Literal ID="litNewMoverDescription" runat="server" />
                                    </div>

                                    <div class="col-sm-4 text-right catRowHeading">
                                        <strong><asp:Literal ID="litNewMoverPrice" runat="server" /></strong>
                                    </div>

                                </div>

                            </div>

                            <div>&nbsp;</div>
                        
                        </section>
                    </asp:Panel>
                    

                    <%--Emails --%>
                    <asp:Panel ID="pnlEmails" runat="server" Visible="false">
                        <section id="emailsBlock">

                            <div class="catRow">
                        
                                <div class="row">

                                    <div class="col-sm-4 catRowHeading">
                                        Targeted Email Campaign
                                    </div>

                                    <div class="col-sm-4 text-center catRowDetails">
                                        <asp:Literal ID="litEmailDescription" runat="server" />
                                    </div>

                                    <div class="col-sm-4 text-right catRowHeading">
                                        <strong><asp:Literal ID="litEmailPrice" runat="server" /></strong>
                                    </div>

                                </div>

                            </div>
                            
                            <div>&nbsp;</div>

                        </section>
                    </asp:Panel>

                     <%--Addressed Add-Ons --%>
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


                    <%--Sub Total--%>
                    <section id="subtotalBlock">
                        <asp:Panel ID="pnlSubTotal" runat="server">
                        <div class="catRow">
                            <div class="row">
                                <div class="col-sm-6 catRowHeading">Subtotal</div>
                                <div class="col-sm-6 text-right catRowHeading"><asp:Label ID="lblSubTotal" runat="server" /></div>
                            </div>
                        </div>
                    </asp:Panel>
                    </section>

                    <hr />

                    <%--Coupon. Optional --%>
                    <asp:Panel ID="pnlCouponDiscount" runat="server" Visible="false" ClientIDMode="Static">
                        
                        <section id="discountBlock">

                            <div class="catRow" id="couponBlock">
                                <div class="row">
                                    <div class="col-sm-4 catRowHeading">
                                        Discounts<a id="coupon"></a>
                                    </div>
                                    <div class="col-sm-4 text-center">
                                        <asp:Label ID="lblCouponName" runat="server" CssClass="text-info" />&nbsp;<asp:Label ID="lblCouponMsg" runat="server" CssClass="text-info" />
                                    </div>
                                    <div class="col-sm-4 text-right catRowHeading">
                                        <asp:Label ID="lblCouponDiscount" runat="server"/>
                                    </div>
                                </div>
                            </div>

                            <%--Coupon Form--%>
                            <asp:Panel ID="pnlCouponForm" runat="server">
                                <div class="row" id="couponFormBlock">

                                    <div class="col-sm-8 col-sm-offset-4">

                                        <asp:Literal ID="litCouponError" runat="server" Visible="False" />

                                        <div class="form-inline">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <label class="sr-only" for="txtCouponCode">Coupon Code</label>
                                                    <asp:TextBox ID="txtCouponCode" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>

                                            <asp:LinkButton ID="lnkApplyCoupon" runat="server" CssClass="btn btn-primary btn-sm" OnClick="lnkApplyCoupon_Click" CausesValidation="False">
                                                <span class="fa fa-plus"></span>&nbsp;Apply Coupon
                                            </asp:LinkButton>

                                        </div>
                                    </div>

                                </div>

                                <div>&nbsp;</div>  
                            </asp:Panel>

                         </section>

                    </asp:Panel>
                   

                    <%--Sales Tax. Optional --%>
                    <asp:Panel ID="pnlSalesTax" runat="server" Visible="false">
                        <section id="taxesBlock">

                            <div class="catRow">
                                <div class="row">
                                    <div class="col-sm-4 catRowHeading"><asp:Literal ID="Literal1" runat="server" Text="Sales Tax" /></div>
                                    <div class="col-sm-4 text-center catRowDetails"><asp:Label ID="lblSalesTaxMessage" runat="server" /></div>
                                    <div class="col-sm-4 text-right catRowHeading"><strong><asp:Label ID="lblSalesTax" runat="server" /></strong></div>
                                </div>
                            </div>
                   
                            <div>&nbsp;</div>  

                        </section>   
                    </asp:Panel>
                 
                    
                    <%-- Total--%>
                    <section id="totalBlock">
                        <div id="EstTotal">
                            <div class="catRow">

                                <div class="row">
                                    <div class="col-sm-4 catRowHeading">Total</div>
                                    <div class="col-sm-4 text-center"><asp:Literal ID="litSalesTaxDisclaimer" runat="server" /></div>
                                    <div class="col-sm-4 text-right grandTotal"><asp:Label ID="TotalEstimate" runat="server" /></div>
                                </div>

                            </div>
                        </div>
                    </section>

                    <hr />

                    <%--Payment / Billing Section--%>
                    <section id="paymentBlock">
                        <div class="row">

                            <%--Billing Info / Opt Out--%>
                            <div class="col-sm-6">
                                
                                 <%--Billing Info--%>
                                <asp:Panel ID="pnlBillingInfo" runat="server">
                                    <div class="panel panel-primary">

                                    <div class="panel-heading">
                                        <asp:Literal ID="litBillingPanelHeader" runat="server" />
                                    </div>

                                    <div class="panel-body">
                                    
                                        <p><small><asp:Literal ID="litBillingPanelText" runat="server" /></small></p>
                                    
                                        <div role="form">

                                            <div class="row">

                                                <%--First Name--%>
                                                <div class="col-xs-6">

                                                    <div class="form-group">
                                                        <label for="BillInfo_FirstName" class="label formLabelRequired">First Name</label>
                                                        <div class="row">
                                                            <div class="col-xs-10">
                                                                <asp:TextBox ID="BillInfo_FirstName" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="50" />
                                                            </div>
                                                            <div class="col-xs-2">
                                                                <asp:RequiredFieldValidator ID="rfvBillInfoFirstName" ControlToValidate="BillInfo_FirstName" runat="server" Display="dynamic" CssClass="text-danger" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                </asp:RequiredFieldValidator>
                                                            </div>

                                                        </div>
                                                    </div>

                                                </div>

                                                <%--Last Name--%>
                                                <div class="col-xs-6">

                                                    <div class="form-group">
                                                        <label for="BillInfo_LastName" class="label formLabelRequired">Last Name</label>
                                                        <div class="row">
                                                            <div class="col-xs-10">
                                                                <asp:TextBox ID="BillInfo_LastName" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="50" />
                                                            </div>
                                                            <div class="col-xs-2">
                                                                <asp:RequiredFieldValidator ID="rfvLastName" ControlToValidate="BillInfo_LastName" runat="server" Display="dynamic" CssClass="text-danger" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                </asp:RequiredFieldValidator>
                                                                <asp:CompareValidator ID="cvLastName" runat="server" CssClass="text-danger" ErrorMessage="Last name cannot be the same as first name." ControlToCompare="BillInfo_FirstName" ControlToValidate="BillInfo_LastName" Display="Dynamic" Operator="NotEqual" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                </asp:CompareValidator>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>


                                            <%--Company--%>
                                            <div class="form-group">
                                                <label for="BillInfo_Company" class="label formLabelRequired">Company</label>
                                                <div class="row">
                                                    <div class="col-xs-10">
                                                        <asp:TextBox ID="BillInfo_Company" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="50" />
                                                    </div>
                                                    <div class="col-xs-2">
                                                        <asp:RequiredFieldValidator ID="rfvCompany" ControlToValidate="BillInfo_Company" runat="server" Display="dynamic" CssClass="text-danger" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                            <span class="fa fa-2x fa-exclamation-circle"></span>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>


                                            <%--Address1--%>
                                            <div class="form-group">
                                                <label for="BillInfo_Address1" class="label formLabelRequired">Address</label>
                                                <div class="row">
                                                    <div class="col-xs-10">
                                                        <asp:TextBox ID="BillInfo_Address1" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="60" />
                                                    </div>
                                                    <div class="col-xs-2">
                                                        <asp:RequiredFieldValidator ID="rfvBillInfoAddress1" ControlToValidate="BillInfo_Address1" runat="server" Display="dynamic" CssClass="text-danger" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                            <span class="fa fa-2x fa-exclamation-circle"></span>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>


                                            <%--Address2--%>
                                            <div class="form-group">
                                                <label for="BillInfo_Address2" class="label formLabel">Address 2</label>
                                                <div class="row">
                                                    <div class="col-xs-10">
                                                        <asp:TextBox ID="BillInfo_Address2" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="100" />
                                                    </div>
                                                    <div class="col-xs-2">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </div>


                                            <%--City--%>
                                            <div class="form-group">
                                                <label for="BillInfo_City" class="label formLabelRequired">City</label>
                                                <div class="row">
                                                    <div class="col-xs-10">
                                                        <asp:TextBox ID="BillInfo_City" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="40" />
                                                    </div>
                                                    <div class="col-xs-2">
                                                        <asp:RequiredFieldValidator ID="rfvBillInfoCity" ControlToValidate="BillInfo_City" runat="server" Display="dynamic" CssClass="text-danger" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                            <span class="fa fa-2x fa-exclamation-circle"></span>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>


                                            <%--State & Postal Code--%>
                                            <div class="row">

                                                <div class="col-xs-7">

                                                    <div class="form-group">
                                                        <label class="label formLabelRequired" for="BillInfo_State">State</label>

                                                        <div class="row">
                                                            <div class="col-sm-10">

                                                                <asp:DropDownList ID="BillInfo_State" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Select State" Value=""></asp:ListItem>
                                                                    <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                    <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                    <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                    <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                    <asp:ListItem Value="CA">California</asp:ListItem>
                                                                    <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                    <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                    <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                    <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                    <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                    <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                    <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                    <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                    <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                    <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                    <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                    <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                    <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                    <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                    <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                    <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                    <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                    <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                    <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                    <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                    <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                    <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                    <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                    <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                    <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                    <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                    <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                    <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                    <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                    <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                    <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                    <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                    <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                    <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                    <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                    <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                    <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                    <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                    <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                    <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                    <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                    <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                    <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                    <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                    <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                    <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>
                                                            <div class="col-sm-2">
                                                                <asp:RequiredFieldValidator ID="rfvBillingState" ControlToValidate="BillInfo_State" runat="server" Text="" CssClass="text-danger" Display="Dynamic" ValidationGroup="vgCheckout">
                                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                </asp:RequiredFieldValidator>
                                                            </div>

                                                        </div>

                                                    </div>

                                                </div>

                                                <div class="col-xs-5">

                                                    <div class="form-group">
                                                        <label for="BillInfo_Zip" class="label formLabelRequired">Zip Code</label>
                                                        <div class="row">
                                                            <div class="col-xs-10">
                                                                <asp:TextBox ID="BillInfo_Zip" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="10" />
                                                            </div>
                                                            <div class="col-xs-2">
                                                                <asp:RequiredFieldValidator ID="rfvPostalCode" ControlToValidate="BillInfo_Zip" runat="server" Text="" CssClass="text-danger" Display="Dynamic" ValidationGroup="vgCheckout">
                                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                </asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="revPostalCode" ControlToValidate="BillInfo_Zip" runat="server" CssClass="text-danger" ErrorMessage="Please provide a valid postal code (5-10 characters)." ValidationExpression="[A-Za-z0-9\-]{5,10}" Display="Dynamic" ValidationGroup="vgCheckout">
                                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>


                                            <div class="row">

                                                <%--Email--%>
                                                <%--This is configurable per site --%>
                                                <div class="col-xs-7">

                                                    <asp:Panel ID="pnlBillingEmail" runat="server" ClientIDMode="Static">
                                                        <div class="form-group">
                                                            <label class="label formLabelRequired" for="BillInfo_Email">Email</label>
                                                            <div class="row">
                                                                <div class="col-xs-10">
                                                                    <asp:TextBox ID="BillInfo_Email" runat="server" CssClass="form-control input-sm" MaxLength="100" Enabled="False" />
                                                                </div>
                                                                <div class="col-xs-2">
                                                                    <asp:RequiredFieldValidator ID="rfvBillingEmail" Display="dynamic" ControlToValidate="BillInfo_Email" runat="server" EnableClientScript="true" CssClass="text-danger" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="revBillingEmail" runat="server" ControlToValidate="BillInfo_Email" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger" ValidationGroup="vgCheckout">
                                                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                    </asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>

                                                </div>

                                                
                                                <%--Phone--%>
                                                <div class="col-xs-5">

                                                    <div class="form-group">
                                                        <label for="BillInfo_Phone" class="label formLabelRequired">Phone Number</label>
                                                        <div class="row">
                                                            <div class="col-xs-10">
                                                                <asp:TextBox ID="BillInfo_Phone" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="25" ClientIDMode="Static" />
                                                            </div>
                                                            <div class="col-xs-2">
                                                                <asp:RequiredFieldValidator ID="rfvBillPhone" CssClass="text-danger" ControlToValidate="BillInfo_Phone" runat="server" Display="Dynamic" ValidationGroup="vgCheckout">
                                                                    <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>

                                            
                                            <%--Comments--%>
                                            <%--Currently hidden--%>
                                            <div class="form-group hidden">
                                                <label for="BillInfo_Comments" class="col-sm-4 control-label">Comments</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="BillInfo_Comments" runat="server" CssClass="form-control" Enabled="false" />
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                </div>
                                </asp:Panel>

                                <%--Opt Out--%>
                                <asp:PlaceHolder runat="server" ID="phOptOut" Visible="false">

                                    <div class="panel panel-primary">
                                    
                                        <div class="panel-heading">
                                            Opt Out Verification
                                        </div>

                                        <div class="panel-body">

                                            <div class="well well-sm">
                                                <div class="radio">
                                                    <asp:RadioButtonList ID="radMailListOptOut" runat="server" ClientIDMode="Static">
                                                        <asp:ListItem Value="NoOptOut" Selected="True">
                                                            To the best of my knowledge, no residents/businesses have opted-out from receiving my marketing collateral.
                                                        </asp:ListItem>
                                                        <asp:ListItem Value="OptOutList">
                                                            Some residents/businesses have opted-out from receiving my marketing collateral. (I will provide a list of these opt-outs)
                                                        </asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </asp:PlaceHolder>


                                <%--Terms--%>
                                <section id="termsBlock">

                                    <%--Terms and Agreement--%>
                                    <div class="panel panel-primary">

                                        <div class="panel-heading">
                                            Terms of Use
                                        </div>

                                        <div class="panel-body">
                                            <div class="checkbox">
                                                <label>
                                                    <asp:CheckBox ID="chkTandCAgree" runat="server" CausesValidation="true" />
                                                    <span class="text-danger"><strong>By checking this box I am confirming that I have read and agree to the Terms and Conditions below.</strong></span>
                                                </label>
                                            </div>

                                            <br />
                                
                                            <%--Validator--%>
                                            <asp:CustomValidator ID="rfvTandCAgree" runat="server" CssClass="" ClientValidationFunction="TandCAgree" ErrorMessage="You must agree to the Terms and Conditions to place your order (see below)."  ControlToValidate="CCNumber" ValidateEmptyText="True" EnableClientScript="true" ValidationGroup="vgCheckout" Display="Dynamic">
                                                <div class="alert alert-danger"><span class="fa fa-exclamation"></span>&nbsp;You must agree to the Terms and Conditions to place your order.</div>
                                            </asp:CustomValidator>


                                            <iframe id="termsandconditions" src="/Legal-Sales-Terms" runat="server" height="225" width="100%" border="1" frameborder="0" />

                                        </div>

                                    </div>

                                </section>


                            </div>

                            <%--Payment--%>                        
                            <div class="col-sm-6">

                                <%--Job Requests--%>
                                <asp:PlaceHolder runat="server" ID="phJobRequests" Visible="true">

                                    <div class="panel panel-primary">
                                    
                                        <div class="panel-heading">
                                            <asp:Literal ID="litJobCommentsHeader" runat="server" />
                                        </div>

                                        <div class="panel-body">

                                            <p><asp:Literal ID="litJobComments" runat="server" /></p>

                                            <%--Store Number--%>
                                            <asp:Panel ID="pnlStoreNumber" runat="server" Visible="false">
                                                <div class="row">
                                                    <div class="col-xs-12">

                                                        <div class="form-group">
                                                            <label for="txtStoreNumber" class="label formLabelRequired">Store Number</label>
                                                            <div class="row">
                                                            
                                                                <div class="col-xs-6">
                                                                    <asp:TextBox ID="txtStoreNumber" runat="server" Enabled="true" CssClass="form-control input-sm" MaxLength="4" />
                                                                </div>

                                                                <div class="col-xs-6">
                                                                    <asp:RequiredFieldValidator ID="rfvStoreNumber" Display="dynamic" Text="*" ControlToValidate="txtStoreNumber" runat="server" ErrorMessage="Store Number is required." EnableClientScript="true" CssClass="text-danger" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                    </asp:RequiredFieldValidator>

                                                                    <asp:RegularExpressionValidator ID="revStoreNumber" runat="server" ErrorMessage="Store Number is a 4-Digit Number."  ValidationExpression="\d{4}" ControlToValidate="txtStoreNumber" CssClass="text-danger" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgCheckout">
                                                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                    </asp:RegularExpressionValidator>
                                                                </div>

                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </asp:Panel>

                                            <%--Email Address--%>
                                            <asp:Panel ID="pnlReceiptEmail" runat="server"> 
                                                <div class="row">
                                                    <div class="col-xs-12">

                                                        <div class="form-group">
                                                            <label for="txtEmailReceipt" class="label formLabelRequired">Email To</label>
                                                            
                                                            <div class="row">
                                                                <div class="col-xs-10">
                                                                    <asp:TextBox ID="txtEmailReceipt" runat="server" CssClass="form-control input-sm" />
                                                                </div>

                                                                <div class="col-xs-2">
                                                                    <asp:RequiredFieldValidator ID="rfvEmailAddress" Display="dynamic" Text="*" ControlToValidate="txtEmailReceipt" runat="server" ErrorMessage="Email address is required." EnableClientScript="true" CssClass="text-danger" SetFocusOnError="True" ValidationGroup="vgCheckout">
                                                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="revEmailAddress" runat="server" ErrorMessage="A valid e-email address is required." ControlToValidate="txtEmailReceipt" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger" ValidationGroup="vgCheckout">
                                                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                                                    </asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </asp:Panel>

                                            <%--Comments--%>
                                            <div class="row">

                                                <div class="col-xs-12">

                                                <label for="txtJobComments" class="label formLabel">Comments</label>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <asp:TextBox ID="txtJobComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </asp:PlaceHolder>

                                <%--Payment Method--%>
                                <asp:Panel ID="pPayMeth" runat="server">
                                    <div class="panel panel-primary">
                                        <div class="panel-heading">
                                            Payment Method
                                        </div>
                            
                                        <div class="panel-body">
                                            <asp:RadioButtonList ID="radPaymentType" runat="server" RepeatDirection="Vertical" ClientIDMode="Static">
                                                <asp:ListItem Text="Credit Card" Value="CC" />
                                                <asp:ListItem Text="eCheck" Value="EC" />
                                                <asp:ListItem Text="Purchase Order" Value="PO" />
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <%--Credit Card--%>
                                <asp:Panel ID="pPayCC" runat="server" data-paytype="CC">
                                    <div class="panel panel-primary">
                                    
                                        <div class="panel-heading">
                                            Credit Card Details
                                        </div>
                                    
                                        <div class="panel-body">
                                            <p><strong>Choose your payment option:</strong></p>

                                            <%--Pay in Full--%>
                                            <div class="well well-sm">
                                                <div class="row">
                                                    <div class="col-sm-6">

                                                        <div class="noTopMargin">
                                                            <label>
                                                                <asp:RadioButton ID="radCCPayNow" runat="server" GroupName="PayType" Checked="true" ClientIDMode="Static" />
                                                                <strong>Pay in full</strong>
                                                            </label>
                                                        </div>

                                                    </div>
                                                    <div class="col-sm-6">
                                                        <p class="dueAmount text-right"><strong>Due Today: <asp:Literal ID="litCCFullPayBalanceDue" runat="server" /></strong></p>
                                                    </div>
                                                </div>
                                            </div>
                                        
                                            <%--Finance--%>
                                            <asp:PlaceHolder ID="phOriginalCCFullPayBalanceDue" runat="server" Visible="true">
                                                <div class="well well-sm">
                                                
                                                    <%--Obsolete 6/11/215
                                                    <div class="text-muted">
                                                        <strong><s><asp:Literal ID="litOriginalCCFullPayBalanceDue" runat="server" Visible="False" /></s></strong>
                                                    </div>--%>
                                        
                                                    <asp:Panel runat="server" ID="pnlFinance" Visible="True">

                                                        <div class="row">

                                                            <div class="col-sm-6">
                                                                <div class="noTopMargin">
                                                                    <label>
                                                                        <asp:RadioButton ID="radCCFinance" runat="server" GroupName="PayType" ClientIDMode="Static" />
                                                                        <strong>Finance Payments</strong>
                                                                    </label>
                                                                </div>

                                                            </div>

                                                            <div class="col-sm-6">
                                                                <p class="dueAmount text-right"><strong>Due Today: <asp:Literal ID="litCCFinancePayBalanceDue" runat="server" /></strong></p>

                                                                <asp:PlaceHolder ID="phOriginalFinancePayBalanceDue" runat="server" Visible="false">
                                                                    <p><asp:Literal ID="lOriginalFinancePayBalanceDue" runat="server" /></p>
                                                                </asp:PlaceHolder>

                                                            </div>
                                                        </div>

                                                        <p><small>50% deposit due with order. $25.00 service fee for each subsequent payment applies. 
                                                        Subsequent payments will be automatically charged to your credit card on file on
                                                        the dates indicated below:</small></p>

                                                        <asp:Repeater ID="rCCPayments" runat="server">
                                                            <ItemTemplate>
                                                                <div class="row">
                                                                    <div class="col-sm-7 col-sm-offset-1">
                                                                        <small><%#Taradel.Util.NumberHelp.FormatTextCount(Container.ItemIndex + 2)%> Payment - 
                                                                        <asp:Literal ID="lPayDate" runat="server" Text='<%#DateTime.Parse(Eval("Key")).ToString("dddd, dd MMM yyyy") %>' />:</small>
                                                                    </div>
                                                                    <div class="col-sm-4">
                                                                        <small><strong><asp:Literal ID="lPayAmount" runat="server" Text='<%#Decimal.Parse(Eval("Value")).ToString("C") %>' /></strong></small>
                                                                    </div>
                                                                </div> 
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                        <hr />

                                                        <p class="dueAmount text-right"><strong>Total Due: <asp:Literal ID="litCCFinanceTotal" runat="server" /></strong></p>

                                                    </asp:Panel>

                                                </div>
                                            </asp:PlaceHolder>
                                    
                                            <p><strong>Credit Card Information</strong></p>

                                            <%--Save Credit Card Info--%>
                                            <asp:PlaceHolder runat="server" ID="phSaveMyCCInfo" Visible="false">

                                                <div class="well well-sm">
                                    
                                                    <p><strong>Save My Credit Card Information</strong></p>

                                                        <div class="radio">
                                                            <asp:RadioButtonList ID="rdlSaveMyCreditCardIndo" runat="server" ClientIDMode="Static">
                                                            <asp:ListItem Value="SaveMyCC" >
                                                                Securely store my credit card information for future transactions.
                                                            </asp:ListItem>
                                                            <asp:ListItem Value="NoSaveMyCC" Selected="True">
                                                                Do not store my credit card information for future transactions.
                                                            </asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>

                                                    <asp:Button ID="btnSaveCreditCardInfo" Text="testSave" runat="server" OnClick="btnSaveCreditCardInfo_Click" Visible="false" />
                                                </div>                                     

                                            </asp:PlaceHolder>

                                            <%--Credit Card info--%>
                                            <div class="well well-sm">
                                       
                                                <div class="form-horizontal" role="form">
                                                    <div class="form-group">
                                                        <label for="CCNumber" class="col-sm-5 control-label">Credit Card Number</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="CCNumber" runat="server" MaxLength="20" CssClass="form-control input-sm" />
                                                            <asp:RequiredFieldValidator ID="rfvCCNumber" runat="server" ControlToValidate="CCNumber" CssClass="label label-danger" ErrorMessage="The credit card number is required." ValidationGroup="vgCCPay" Display="Dynamic">
                                                                The credit card number is required.
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="CCExp" class="col-sm-5 control-label">Expiration Date</label>
                                                        <div class="col-sm-7">
                                                            <picker:JSDate ID="CCExp" runat="server" Required="true" RequiredErrorMessage="The credit card expiration date is required." ValidationGroup="vgCCPay" ValidationText="(*)" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="CVV2" class="col-sm-5 control-label">Security Number</label>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="CVV2" runat="server" MaxLength="4" CssClass="form-control input-sm"/>
                                                            <asp:RequiredFieldValidator ID="rfvCVV2" runat="server" CssClass="label label-danger" ControlToValidate="CVV2" ErrorMessage="The security code is required. This is a 3-digit code located on the back of you card (Visa and Mastercard) or a 4-digit code located on the front of your card (American Express)." ValidationGroup="vgCCPay" Display="Dynamic">
                                                                The security code is required.
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <asp:Panel ID="pnlNewMoverInfo" runat="server" Visible="False">
                                                        <p><small>Your credit card information will be saved with your New Mover order and billed approximately every four weeks for each subsequent new mover order.</small></p>
                                                    </asp:Panel>

                                                </div>    

                                            </div>   

                                        </div>

                                    </div>
                                </asp:Panel>

                                <%--Electronic check--%>
                                <%--Needs styling--%>
                                <asp:Panel ID="pPayEC" runat="server" Style="display: none;" data-paytype="EC">
                                    <div class="panel panel-primary">
                                    
                                        <div class="panel-heading">
                                            Electronic Check
                                        </div>
                                    
                                        <div class="panel-body">

                                            <asp:RadioButton ID="radECPayNow" runat="server" Text="Pay in full" GroupName="PayType" Checked="true" Visible="false" />&nbsp;
                    
                                            <h4>Due Today: <asp:Literal ID="litECFullPayBalanceDue" runat="server" /></h4>

                                            <h4>Checking Account Information</h4>

                                            Routing Number:
                                            <asp:TextBox ID="BankRoutingNumber" runat="server" MaxLength="20" />
                                            <asp:RequiredFieldValidator ID="rfvBankRoutingNumber" runat="server" ControlToValidate="BankRoutingNumber" ErrorMessage="The bank routing number is required." Text="(*)" ValidationGroup="vgECPay" />
                    
                                            Bank Account Number
                                            <asp:TextBox ID="BankAccountNumber" runat="server" MaxLength="20" />
                                            <asp:RequiredFieldValidator ID="rfvBankAccountNumber" runat="server" ControlToValidate="BankAccountNumber" ErrorMessage="The bank account number is required." Text="(*)" ValidationGroup="vgECPay" />
                    
                                            Bank Name:
                                            <asp:TextBox ID="BankName" runat="server" MaxLength="100" />
                                            <asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="BankName" ErrorMessage="The bank name is required." Text="(*)" ValidationGroup="vgECPay" />
                    
                                            Name on Account:
                                            <asp:TextBox ID="BankNameOnAccount" runat="server" MaxLength="100" />
                                            <asp:RequiredFieldValidator ID="rfvBankNameOnAccount" runat="server" ControlToValidate="BankNameOnAccount" ErrorMessage="The name on the checking account is required." Text="(*)" ValidationGroup="vgECPay" />


                                        </div>
                                    </div>
                                </asp:Panel>

                                <%--Purchase Order--%>
                                <asp:Panel ID="pPayPO" runat="server" Style="display: none;" data-paytype="PO">
    
                                    <div class="panel panel-primary">

                                        <div class="panel-heading">
                                            <asp:Literal ID="litPOPanelText" runat="server" />
                                        </div>

                                        <div class="panel-body">

                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <strong>Purchase Order Amount</strong>
                                                </div>
                                                <div class="col-sm-6">
                                                    <strong><asp:Literal ID="litPOFullPayBalanceDue" runat="server" /></strong>
                                                </div>
                                            </div>

                                            <asp:PlaceHolder ID="phOriginalPOFullPayBalanceDue" runat="server" Visible="false">
                                                <h4><asp:Literal ID="litOriginalPOFullPayBalanceDue" runat="server" /></h4>
                                            </asp:PlaceHolder>

                                            <div>&nbsp;</div>

                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <strong><asp:Literal ID="litPOLabel" runat="server" /></strong>
                                                </div>
                                                <div class="col-sm-6">

                                                    <div class="row">
                                                        <div class="col-xs-7">
                                                            <asp:TextBox ID="PONumber" runat="server" MaxLength="20" CssClass="form-control input-sm" ClientIDMode="Static" />
                                                        </div>
                                                        <div class="col-xs-5">
                                                            <asp:RequiredFieldValidator ID="rfvPONumber" runat="server" CssClass="text-danger" ControlToValidate="PONumber" ValidationGroup="vgPOPay">
                                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="revPONumber" runat="server" CssClass="text-danger" ValidationExpression="^\d{10}$" ControlToValidate="PONumber" ErrorMessage="Customer Rewards Number must be 10 numeric characters." Display="Dynamic" ValidationGroup="vgPOPay" Enabled="False">
                                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                </asp:Panel>

                                <%--Order Button--%>
                                <div class="text-center">
                                    <appx:SingleClickButton ID="lnkCheckout" runat="server" Text="COMPLETE YOUR ORDER" 
                                    ShowProcessingModal="true" 
                                    ProcessingModalHtml='<div class="processingModal"><div class="sectionHeader">Processing</div><br /><br /><br /><br /><h4>Processing your order....</h4><br /><br /><br /><br /><p><img src="/cmsimages/loadingbar.gif" height="22" width="126" title="Processing..." alt="Processing..." /></p></div>' 
                                    ProcessingModalTitle="" ClickedText="Processing..." ValidationGroup="vgCheckout" class="btn btn-danger btn-lg" />
                                </div>

                                <div>&nbsp;</div>

                                <div class="text-center"> 
                                    <asp:LinkButton ID="btnGetQuote" runat="server" ClientIDMode="Static" CssClass="btn btn-info btn-lg" Visible="False" CausesValidation="False">
                                        <span class="fa fa-download"></span>&nbsp;Download Quote (PDF)
                                    </asp:LinkButton>
                                </div>


                            </div>

                        </div>
                    </section>

                    <div>&nbsp;</div>


                </asp:Panel>

                <%--Whats This--%>
                <div id="whatIsThis" title="What's This" style="display: none;">
                    <div id="wtLoader" style="display: none;">
                        <div>
                            <h3>Loading</h3>
                            <p><img src="/cmsimages/loadingbar.gif" alt="Loading" /></p>
                        </div>
                    </div>
                    <div id="wtContent"></div>
                </div>

            </div>


        </div>

    </section>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">

    <script src="/assets/javascripts/PhoneMask.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(window).load(function ()
        {
        var phones = [{ "mask": "(###) ###-####" }, { "mask": "(###) ###-##############" }];
        $('#BillInfo_Phone').inputmask(
            {
                mask: phones,
                greedy: false,
                definitions: {'#': { validator: "[0-9]", cardinality: 1 } }
            });
        });
    </script>


</asp:Content>

