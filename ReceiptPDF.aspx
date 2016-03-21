<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReceiptPDF.aspx.vb" Inherits="ReceiptPDF" MasterPageFile="~/App_MasterPages/PDF.master" %>
<%@ Register Src="~/Controls/ReceiptHeaderImage.ascx" TagPrefix="appx" TagName="ReceiptHeaderImage" %>




<asp:Content ID="Content3" ContentPlaceHolderID="phForm" Runat="Server">


    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
        <i class="fa fa-2x fa-exclamation-circle pull-left"></i>&nbsp;
        <asp:Literal ID="litErrorMessage" runat="server" />
        <p>&nbsp;</p>
    </asp:Panel>


    <asp:Panel ID="pnlReceiptSuccess" runat="server" Visible="True">

        <asp:Panel ID="pnlDebug" runat="server" Visible="False" CssClass="ui-icon-alert alert-info">
            <div class="well">

                <p><strong>OrderCalc:</strong></p>
                <asp:GridView ID="gvTest" runat="server">
                </asp:GridView>

                <p><asp:Label ID="lblDebug" runat="server" /></p>

                <p><strong>oCart:</strong><br />
                <asp:Literal ID="litCart" runat="server" /></p>

            </div>
        </asp:Panel>


        <table id="receiptTable">

            <%--Your Receipt--%>
            <tr class="pdfHeaderRow">
                <td><div class="text-center">YOUR RECEIPT</div></td>
            </tr>


            <%--Taradel Header info--%>
            <asp:Panel ID="pnlTaradeDisplay" runat="server" Visible="True">
                <tr>
                    <td>
                        <p>&nbsp;</p>
                        <p><appx:ReceiptHeaderImage runat="server" id="ReceiptHeaderImage" /></p>
                        <p><strong><asp:Literal ID="lReceiptPartnerName" runat="server" Visible="false" />
                        <asp:Literal runat="server" ID="lReceiptPartnerAddress" />
                        <asp:Literal ID="lReceiptPartnerPhone" runat="server" /></strong></p>
                    </td>
                </tr>
            </asp:Panel>


            <%--Order Information--%>
            <asp:Panel ID="pnlOrderInfoDisplay" runat="server" Visible="True">
                <tr class="pdfHeaderRow">
                    <td><strong>ORDER INFORMATION</strong></td>
                </tr>

                <tr>
                    <td>
                        <table id="orderInfoTable" class="pdfDataTable">

                            <tr>
                                <td class="width50"><asp:Literal ID="lSoldTo" runat="server" /></td>
                                <td class="width50 alignRight"><asp:Literal ID="lReceiptDate" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="lSoldToCompanyName" runat="server" /></td>
                                <td class="width50 alignRight"><asp:Literal ID="lReceiptNumber" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="lSoldToAddress" runat="server" /></td>
                                <td class="width50 alignRight"><asp:Literal ID="lOrderNum" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="lSoldToCityStateZip" runat="server" /></td>
                                <td class="width50 alignRight">&nbsp;</td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="lSoldToPhone" runat="server" /></td>
                                <td class="width50 alignRight"><asp:Literal ID="litJobName" runat="server" Visible="false" /></td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="litCustNumLabel" runat="server" />:&nbsp;<asp:Literal ID="lCustomerID" runat="server" /></td>
                                <td class="width50">&nbsp;</td>
                            </tr>

                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>

                            <asp:Panel ID="pnlPONumber" runat="server" Visible="True">
                                <tr>
                                    <td class="width50"><asp:Literal ID="litPOText" runat="server" /></td>
                                    <td class="width50">&nbsp;</td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlStoreNumber" runat="server" Visible="False">
                                <tr>
                                    <td class="width50">Store Number: <asp:Literal ID="litStoreNumber" runat="server" /></td>
                                    <td class="width50">&nbsp;</td>
                                </tr>
                            </asp:Panel>

                            <asp:PlaceHolder ID="phJobComments" runat="server" Visible="False">
                        
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="litJobComments" runat="server" />
                                    </td>
                                </tr>
                            </asp:PlaceHolder>

                        </table>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:Panel>


            <%-- Product Display--%>
            <asp:Panel ID="pnlProductDisplay" runat="server" Visible="true">
            
                <tr class="pdfHeaderRow">
                    <td><asp:Literal runat="server" ID="litProductType" /> PRODUCT</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfProductTable" class="pdfDataTable">
                            <tr>
                                <td class="width33"><asp:Literal ID="litProductName" runat="server" Visible="true" /></td>
                                <td class="width33">&nbsp;</td>
                                <td class="width33">

                                    <asp:Repeater ID="rOpts" runat="server">

                                        <HeaderTemplate>
                                            <table id="pdfOptionsTable" class="pdfDataTable">
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <tr>
                                                <td class="width33"><asp:Literal ID="lOptCat" runat="server" Text='<%#Eval("Name") & ":" %>' /></td>
                                                <td class="width67"><asp:Literal ID="lOptVal" runat="server" Text='<%#Eval("ValueName") %>' /></td>
                                            </tr>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>

                                    </asp:Repeater>

                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>


                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Pro Design--%>
            <asp:PlaceHolder ID="phProDesign" runat="server" Visible="false">
                <tr class="pdfHeaderRow">
                    <td>DESIGN</td>
                </tr>

                <tr>
                    <td>
                
                        <table id="pdfProDesignTbl" class="pdfDataTable">
                            <tbody>
                            <tr>
                                <td class="width67">
                                    <strong>Design Option:</strong>
                                </td>
                                <td class="width33">
                                    Professional Design Service
                                </td>
                            </tr>
                            </tbody>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:PlaceHolder>


            <%--My Design--%>
            <asp:PlaceHolder ID="phMyDesign" runat="server" Visible="false">
                <tr class="pdfHeaderRow">
                    <td>DESIGN</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfMyDesignTbl" class="pdfDataTable">
                            <tbody>
                            <tr>

                                <td class="width33">
                                    <strong><asp:Label ID="lblDesignType" runat="server" Text="" /></strong>
                                </td>

                                <td class="width33">
                                    <asp:Image ID="imgFile1" runat="server" Visible="false" CssClass="img-responsive" />                         
                                    <asp:Literal ID="lLaterMsg" runat="server" />
                                </td>

                                <td class="width33">
                                    <asp:Image ID="imgFile2" runat="server" Visible="false" CssClass="img-responsive" />
                                </td>

                            </tr>
                            </tbody>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:PlaceHolder>


            <%--Map--%>
            <asp:Panel ID="pnlMapDisplay" runat="server" Visible="True">

                <tr class="pdfHeaderRow">
                    <td>MAP</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfMapTable" class="pdfDataTable">
                            <tbody>
                            <tr>
                                <td class="width33">
                                    <asp:Image ID="imgMap" runat="server" Visible="true" ClientIDMode="Static" />
                                </td>
                                <td class="width33">
                                    <strong>Map Name</strong><br />
                                    <asp:Literal ID="litSelectName" runat="server" />
                                </td>
                                <td class="width33">
                                    <strong>Route Description</strong><br />
                                    <asp:Literal ID="litSelectDescription" runat="server" />
                                </td>
                            </tr>
                            </tbody>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--EDDM Drops--%>
            <asp:Panel ID="pnlEddmDrops" runat="server" Visible="False">

                <tr class="pdfHeaderRow pageBreak">
                    <td>EDDM DROPS</td>
                </tr>

                <tr>
                    <td>

                        <br />

                        <asp:Repeater ID="rptEddmDrops" runat="server">

                            <HeaderTemplate>
                                <table id="pdfDropsTable" class="pdfDataGridTable">
                                    <thead>
                                        <tr>
                                            <th class="width10 pdfColumnHeader"><asp:Literal ID="lDropDateLabel2" runat="server" Text="Drop Date" /></th>
                                            <th class="width15 pdfColumnHeader"><asp:Literal ID="lDropNumLabel" runat="server" Text="Drop Number" /></th>
                                            <th class="width10 pdfColumnHeader"><asp:Literal ID="lPiecesInDropLabel" Text="Pieces" runat="server" /></th>
                                            <th class="width65 pdfColumnHeader"><asp:Literal ID="lRoutesLabel" runat="server" Text="Routes" /></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr>
                                    <td class="width10 text-center">
                                        <asp:Literal ID="lDropDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date", "{0:MM/dd/yyyy}")%>' Visible="true" />
                                        <asp:DropDownList ID="ddlDropDate" runat="server" Visible="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="width15 text-center">
                                        <asp:Literal ID="lDropNumber" runat="server" Text='<%#Eval("Number") %>' />
                                    </td>
                                    <td class="width10 text-center">
                                        <asp:Literal ID="lDropCount" runat="server" Text='<%#Integer.Parse(Eval("Total")).ToString("N0") %>' />
                                    </td>
                                    <td class="width65">
                                       <asp:Repeater ID="rptEddmRoutes" runat="server">

                                            <ItemTemplate>
                                                <asp:Literal ID="lRoute" runat="server" Text='<%#Eval(Trim("Name")) %>' />
                                            </ItemTemplate>

                                            <SeparatorTemplate>
                                                ,&nbsp;
                                            </SeparatorTemplate>

                                        </asp:Repeater>

                                    </td>
                                </tr>
                            </ItemTemplate>

                            <AlternatingItemTemplate>
                                <tr class="pdfAltRowColor">
                                    <td class="width10 text-center">
                                        <asp:Literal ID="lDropDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date", "{0:MM/dd/yyyy}")%>' Visible="true" />
                                        <asp:DropDownList ID="ddlDropDate" runat="server" Visible="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="width15 text-center">
                                        <asp:Literal ID="lDropNumber" runat="server" Text='<%#Eval("Number") %>' />
                                    </td>
                                    <td class="width10 text-center">
                                        <asp:Literal ID="lDropCount" runat="server" Text='<%#Integer.Parse(Eval("Total")).ToString("N0") %>' />
                                    </td>
                                    <td class="width65">
                                    <asp:Repeater ID="rptEddmRoutes" runat="server">

                                        <ItemTemplate>
                                            <asp:Literal ID="lRoute" runat="server" Text='<%#Eval(Trim("Name")) %>' />
                                        </ItemTemplate>

                                        <SeparatorTemplate>
                                            ,&nbsp;
                                        </SeparatorTemplate>
                                        </asp:Repeater>

                                    </td>
                                </tr>
                            </AlternatingItemTemplate>

                            <FooterTemplate>
                                </tbody>
                                </table>
                            </FooterTemplate>

                        </asp:Repeater>

                    </td>
                </tr>

                <asp:PlaceHolder ID="phLockedRoutesMsg" runat="server" Visible="False">
                    <tr>
                        <td>
                            <h5 class="text-center"><asp:Literal ID="litLockedRoutes" runat="server" /></h5>
                        </td>
                    </tr>
                </asp:PlaceHolder>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Addressed Drops--%>
            <asp:Panel ID="pnlAddressedDrops" runat="server" Visible="False">
                
                <tr class="pdfHeaderRow pageBreak">
                    <td>Addressed List Drops</td>
                </tr>

                <tr>
                    <td>
                        Here are the details regarding your AddressedList campaign:
                    </td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfDropsTable" class="pdfDataGridTable">
                            <thead>
                                <tr>
                                    <th class="width25 pdfColumnHeader">Pieces</th>
                                    <th class="width25 pdfColumnHeader">Drop Dates</th>
                                    <th class="width50 pdfColumnHeader">Filters</th>
                                </tr>
                            </thead>

                            <tbody>
                                <tr>
                                    <td class="width25"><asp:Literal ID="litAddressedPcs" runat="server" /></td>
                                    <td class="width25"><asp:Literal ID="litAddressedDropDates" runat="server" /></td>
                                    <td class="width50"><asp:Literal ID="litDemographicFilters" runat="server" /></td>
                                </tr>
                            </tbody>
                            </table>

                    </td>
                </tr>        


                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Campaign Schedule. Optional --%>
            <asp:Panel ID="pnlCampaignOverview" runat="server" Visible="false">

                <tr class="pdfHeaderRow">
                    <td>CAMPAIGN OVERVIEW AND SCHEDULE</td>
                </tr>

                <tr>
                    <td>

                        <br />

                        <asp:Repeater ID="rptSchedule" runat="server">
                            <HeaderTemplate>
                                <table id="pdfScheduleTable" class="pdfDataGridTable">
                                    <thead>
                                        <tr>
                                            <th class="width10 pdfColumnHeader">Start Date</th>
                                            <th class="width10 pdfColumnHeader">Quantity</th>
                                            <th class="width15 pdfColumnHeader">Type</th>
                                            <th class="width65 pdfColumnHeader">Routes</th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            
                            <ItemTemplate>
                                <tr>
                                    <td class="width10 text-center"><%#Eval("StartDate").ToShortDateString() %></td>
                                    <td class="width10 text-center"><%#(Eval("Quantity"))%> </td>
                                    <td class="width15"><%#Eval("Type") %></td>
                                    <td class="width65"><%#Eval("Routes") %></td>
                                </tr>
                            </ItemTemplate>

                            <AlternatingItemTemplate>
                                <tr class="pdfAltRowColor">
                                    <td class="width10 text-center"><%#Eval("StartDate").ToShortDateString() %></td>
                                    <td class="width10 text-center"><%#(Eval("Quantity"))%> </td>
                                    <td class="width15"><%#Eval("Type") %></td>
                                    <td class="width65"><%#Eval("Routes") %></td>
                                </tr>
                            </AlternatingItemTemplate>

                            <FooterTemplate>
                                </table>
                            </FooterTemplate>

                        </asp:Repeater>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Mailing Pcs--%>
            <asp:Panel ID="pnlMailingPcs" runat="server" Visible="False">

                <tr class="pdfHeaderRow">
                    <td><asp:Literal ID="lMailingLabel" runat="server" Text="MAILING" /></td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfMailingTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><asp:Literal ID="litNumOfPcs" runat="server" /></td>
                                <td class="width33 alignRight"><strong><asp:Literal ID="lPriceOfPieces" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Extra Pcs. Optional --%>
            <asp:Panel ID="pnlExtraPcs" runat="server" Visible="false">
           
                <tr class="pdfHeaderRow">
                    <td>ADDITIONAL PIECES</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfExtraPcsTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33">
                                    <asp:Literal ID="litExtraQuantity" runat="server" /> @  <asp:Literal ID="litExtraPricePerPiece" runat="server" /><br />
                                    <strong>Shipping To:</strong><br />
                                    <asp:Literal ID="litExtraQuantityAddress" runat="server" />
                                </td>
                                <td class="width33 alignRight"><strong><asp:Label ID="lblExtraPrice" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Design Fee. Optional --%>
            <asp:Panel ID="pnlDesignFee" runat="server" Visible="false">

                <tr class="pdfHeaderRow">
                    <td>PROFESSIONAL DESIGN FEE</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfDesignFeeTable" class="pdfDataTable">
                            <tr>
                                <td class="text-right"><strong><asp:Label ID="lblDesignFee" runat="server" /></strong>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>


                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Additional Drop Fee. Optional --%>
            <asp:Panel ID="pnlNumOfdrops" runat="server" Visible="false">

                <tr class="pdfHeaderRow">
                    <td>ADDITIONAL DROP FEES</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfDropFeeTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><p class="text-center"><asp:Literal ID="litNumOfDrops" runat="server" /></p></td>
                                <td class="width33"><p class="text-right"><strong><asp:Label ID="lDropFee" runat="server" /></strong></p></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>


            </asp:Panel>


            <%--New Movers. Optional --%>
            <asp:Panel ID="pnlNewMovers" runat="server" Visible="false">

                <tr class="pdfHeaderRow">
                    <td>NEW MOVER CAMPAIGN</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfNewMoverTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><asp:Literal ID="litNewMoverDescription" runat="server" /></td>
                                <td class="width33 alignRight"><strong><asp:Literal ID="litNewMoverPrice" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Emails. Optional --%>
            <asp:Panel ID="pnlEmails" runat="server" Visible="false">
                        
                <tr class="pdfHeaderRow">
                    <td>TARGETED EMAIL CAMPAIGN</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfEmailTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><asp:Literal ID="litEmailDescription" runat="server" /></td>
                                <td class="width33 alignRight"><strong><asp:Literal ID="litEmailPrice" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
                       
            </asp:Panel>


      <%--Addressed AddOns. Optional --%>
            <asp:Panel ID="pnlAddressedAddOns" runat="server" Visible="false">
                        
                <tr class="pdfHeaderRow">
                    <td>Addressed Add-On Campaign</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfAddressedAddOn" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><asp:Literal ID="litAddressedAddOnsDescription" runat="server" /></td>
                                <td class="width33 alignRight"><strong><asp:Literal ID="litAddressedAddOnsPrice" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
                       
            </asp:Panel>



            <%--Finance Fees. Optional --%>
            <asp:Panel ID="pnlFinanceFees" runat="server" Visible="false">

                <tr class="pdfHeaderRow">
                    <td>FINANCE FEES</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfFinanceFeeTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><asp:Literal ID="lFinanceFeeDetail" runat="server" /></td>
                                <td class="width33 alignRight"><strong><asp:Literal ID="lFinanceFee" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Shipping Fee. Optional. Possibly obsolete--%>
            <asp:Panel ID="pnlShippingFee" runat="server" Visible="False">

                <tr class="pdfHeaderRow">
                    <td>SHIPPING FEE</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfShippingFeeTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33">&nbsp;</td>
                                <td class="width33 alignRight"><strong><asp:Literal runat="server" ID="lShippingFee" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Sub Total--%>
            <asp:Panel ID="pnlSubTotalDisplay" runat="server" Visible="True">
                <tr class="pdfHeaderRow">
                    <td>
                        <table id="pdfSubTotalTable" class="width100">
                            <tr>
                                <td class="width50">SUBTOTAL</td>
                                <td class="width50 alignRight"><strong><asp:Literal ID="lSubtotal" runat="server" /></strong></td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:Panel>


            <%--Coupons. Optional --%>
            <asp:Panel ID="pnlCouponDiscount" runat="server" Visible="false">

                <tr class="pdfHeaderRow">
                    <td>DISCOUNTS</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfCouponTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><asp:Literal ID="litCouponName" runat="server" /></td>
                                <td class="width33 alignRight"><strong><asp:Literal ID="lCouponDiscount" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>


            <%--Sales Tax. Optional --%>
            <asp:Panel ID="pnlSalesTax" runat="server" Visible="false">
                <tr class="pdfHeaderRow">
                    <td>SALES TAX</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfTaxTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><asp:Label ID="lblSalesTaxMessage" runat="server" /></td>
                                <td class="width33 alignRight"><strong><asp:Label ID="lblSalesTax" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:Panel>


            <%--Total--%>
            <asp:Panel ID="pnlTotalDisplay" runat="server" Visible="True">
                <tr class="pdfHeaderRow">
                    <td>
                        <table id="pdfTotalTable" class="width100">
                            <tr>
                                <td class="width25">TOTAL</td>
                                <td class="width50 pdfHeaderRowSmallText text-center"><asp:Literal ID="litSalesTaxDisclaimer" runat="server" />&nbsp;</td>
                                <td class="width25 alignRight"><strong><asp:Label ID="lOrderTotal" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:Panel>


            <%--Payments--%>
            <asp:Panel ID="pnlPaymentPanel" runat="server" Visible="True">
                <tr class="pdfHeaderRow">
                    <td>PAYMENT INFORMATION</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfPaymentTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">Payments/Credits</td>
                                <td class="width33">&nbsp;</td>
                                <td class="width33 alignRight"><strong><asp:Literal runat="server" ID="lPaymentsCredits" /></strong></td>
                            </tr>

                            <tr>
                                <td class="width33">Balance</td>
                                <td class="width33">&nbsp;</td>
                                <td class="width33 alignRight"><strong><asp:Literal runat="server" ID="lRemainingBalance" /></strong></td>
                            </tr>
                        </table>

                        <p>&nbsp;</p>

                    </td>
                </tr>
            </asp:Panel>


            <%--Thank you/Payment message--%>
            <asp:Panel ID="pnlPaymentMsgDisplay" runat="server" Visible="True">
                <tr>
                    <td>

                        <div class="well well-sm">
                            <h4 class="text-center"><asp:Literal ID="lPaymentMessage" runat="server" /></h4>
                        </div>

                    </td>
                </tr>
            </asp:Panel>


            <%--Financed Payments--%>
            <asp:Panel ID="pnlFinancePaymentsDisplay" runat="server">
                <tr>
                    <td>
                        <asp:Repeater ID="rPayments2" runat="server">
                            <HeaderTemplate>
                        
                                <h4><strong>Scheduled Payments</strong></h4>

                                <table id="pdfPaymentsTable" class="pdfDataGridTable">
                                    <thead>
                                        <tr>
                                            <th class="width25 pdfColumnHeader">Bill Date</th>
                                            <th class="width25 pdfColumnHeader">Drop Date</th>
                                            <th class="width25 pdfColumnHeader">Payment</th>
                                            <th class="width25 pdfColumnHeader">Balance (after payment)</th>
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

                            <AlternatingItemTemplate>
                                <tr class="pdfAltRowColor">
                                    <td class="width25"><asp:Literal ID="lBillDate" runat="server" Text='<%#DateTime.Parse(Eval("BillDate")).ToString("MM/dd/yyyy")%>'  /></td>
                                    <td class="width25"><asp:Literal ID="lDropDate" runat="server" Text='<%#DateTime.Parse(Eval("DropDate")).ToString("MM/dd/yyyy")%>' /></td>
                                    <td class="width25"><asp:Literal ID="lDropCount" runat="server" Text='<%#Decimal.Parse(Eval("Payment").ToString()).ToString("C")%>' /></td>
                                    <td class="width25"><asp:Literal ID="lBalance" runat="server" Text='<%#Decimal.Parse(Eval("Balance").ToString()).ToString("C")%>' /></td>
                                </tr>
                            </AlternatingItemTemplate>

                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>


                        <%--OBSOLETE Repeater --%>
                        <asp:Repeater ID="rPayments" runat="server" Visible="false">

                            <ItemTemplate><p>
                                <%#Taradel.Util.NumberHelp.FormatTextCount(Container.ItemIndex + 2)%>
                                    Payment
                                    <asp:Literal ID="lPayDate" runat="server" Text='<%#DateTime.Parse(Eval("Key")).ToString("dddd, dd MMM yyyy") %>' />:&nbsp;
                                    <strong><asp:Literal ID="lPayAmount" runat="server" Text='<%#Decimal.Parse(Eval("Value").ToString()).ToString("C") %>' /></strong></p>
                            </ItemTemplate>

                        </asp:Repeater>

                    </td>

                </tr>


                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:Panel>


            <%--Next Steps--%>
            <asp:Panel ID="pnlNextStepsDisplay" runat="server" Visible="False">
                <tr>
                    <td>

                    <p><strong>Next Steps:</strong> Our expert team will review your order and contact you shortly. No further action is required from you at this time.
                    If you have any questions, contact our help line at <asp:Literal ID="lPhoneNumber" runat="server" />.</p>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:Panel>


            <%--Custom Footer Imgs--%>
            <asp:Panel ID="pnlStaplesFooter" runat="server" Visible="False">

                <tr>
                    <td>

                        <table class="centered95Width" id="staplesReceiptFooterTbl">
                            <tr>
                                <td>&nbsp;</td>
                                <td><h2 class="text-center"><strong><u>Product</u></strong></h2></td>
                                <td></td>
                                <td>&nbsp;</td>
                                <td><h2 class="text-center"><strong><u>Postage</u></strong></h2></td>
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

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:Panel>

            <tr>
                <td>&nbsp;</td>
            </tr>

            <tr>
                <td>&nbsp;</td>
            </tr>

            <%--Terms and Agreement--%>
            <asp:Panel ID="pnlTerms" runat="server" Visible="true">
                <tr class="pdfHeaderRow pageBreak">
                    <td>Terms of Use</td>
                </tr>
            
                <tr>
                    <td>

                        <div id="termsBlock">

                            <br />

                            <asp:Literal ID="litTermsAndAgreement" runat="server" />

                            <br />

                        </div>


                    </td>
                </tr>
            </asp:Panel>


        </table>

    </asp:Panel>


    <p>&nbsp;</p>
    <p>&nbsp;</p>



</asp:Content>

