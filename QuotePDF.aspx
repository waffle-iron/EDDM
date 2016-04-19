<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/PDF.master" AutoEventWireup="false" CodeFile="QuotePDF.aspx.vb" Inherits="QuotePDF" Trace="false" %>
<%@ Register Src="~/Controls/ReceiptHeaderImage.ascx" TagPrefix="appx" TagName="ReceiptHeaderImage" %>



<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">


    <%--Error--%>
    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
        <p>&nbsp;</p>
        <span class="fa fa-2x fa-exclamation-circle text-danger"></span>&nbsp;
        <asp:Literal ID="litErrorMessage" runat="server" />
        <p>&nbsp;</p>
        <p>&nbsp;</p>
    </asp:Panel>

        
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
                    <td class="width50">AddressedMap</td>
                    <td class="width50"><asp:Literal ID="litAddressedMap" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">AddressedShipPrice</td>
                    <td class="width50"><asp:Literal ID="litAddressedShipPrice" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">BaseProductID</td>
                    <td class="width50"><asp:Literal ID="litBaseProductID" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">DesignFee</td>
                    <td class="width50"><asp:Literal ID="litDesignFee" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">DistributionID</td>
                    <td class="width50"><asp:Literal ID="litDistributionID" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">DropFee</td>
                    <td class="width50"><asp:Literal ID="litDropFee" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">EDDMMap</td>
                    <td class="width50"><asp:Literal ID="litEDDMMap" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">EDDMShipPrice</td>
                    <td class="width50"><asp:Literal ID="litEDDMShipPrice" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">EnvironmentMode</td>
                    <td class="width50"><asp:Literal ID="litEnvironmentMode" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">ExtraCopies</td>
                    <td class="width50"><asp:Literal ID="litExtraCopies" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">ExtraPcsPricePerPiece</td>
                    <td class="width50"><asp:Literal ID="litExtraPcsPricePerPiece" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">GeneratedAddressedList</td>
                    <td class="width50"><asp:Literal ID="litGeneratedAddressedList" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">HasDropFee</td>
                    <td class="width50"><asp:Literal ID="litHasDropFee" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">IsMultiple</td>
                    <td class="width50"><asp:Literal ID="litIsMultiple" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">IsProfessionalDesign</td>
                    <td class="width50"><asp:Literal ID="litIsProfessionalDesign" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">MultipleImpressionsNoFee</td>
                    <td class="width50"><asp:Literal ID="litMultipleImpressionsNoFee" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">MailPiecesPrice</td>
                    <td class="width50"><asp:Literal ID="litMailPiecesPrice" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">NumOfDrops</td>
                    <td class="width50"><asp:Literal ID="litNumOfDrops2" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">NumOfImpressions</td>
                    <td class="width50"><asp:Literal ID="litNumOfImpressions" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">QuoteKey</td>
                    <td class="width50"><asp:Literal ID="litQuoteKey" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">PONumber</td>
                    <td class="width50"><asp:Literal ID="litPONumber" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">PostageRate</td>
                    <td class="width50"><asp:Literal ID="litPostageRate" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">PricePerPiece</td>
                    <td class="width50"><asp:Literal ID="litPricePerPiece" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">ProductID</td>
                    <td class="width50"><asp:Literal ID="litProductID" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">SalesTax</td>
                    <td class="width50"><asp:Literal ID="litSalesTax" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">ShipToAddress</td>
                    <td class="width50"><asp:Literal ID="litShipToAddress" runat="server" /></td>
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
                    <td class="width50">TotalSelected</td>
                    <td class="width50"><asp:Literal ID="litTotalSelected2" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">TotalMailed</td>
                    <td class="width50"><asp:Literal ID="litTotalMailed" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">UploadedAddressedList</td>
                    <td class="width50"><asp:Literal ID="litUploadedAddressedList" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">USelectID</td>
                    <td class="width50"><asp:Literal ID="litUSelectID" runat="server" /></td>
                </tr>

                <tr>
                    <td class="width50">UserName</td>
                    <td class="width50"><asp:Literal ID="litUserName" runat="server" /></td>
                </tr>

            </tbody>

        </table>

        <div>
            <strong>Cart</strong><br />
            <asp:TextBox ID="txtDebugCart" runat="server" TextMode="MultiLine" Rows="15" Columns="120" Font-Size="12px"></asp:TextBox>
        </div>

        <div>
            <asp:Literal ID="litGeneralError" runat="server" />
        </div>

    </asp:Panel>


    <%--Empty Cart--%>
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false">

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


    <%--Quote--%>
    <asp:PlaceHolder ID="phQuote" runat="server" Visible="True">

        <table id="quoteTable" class="width100">

            <%--Your Quote--%>
            <tr class="pdfHeaderRow">
                <td class="width100 text-center">YOUR QUOTE</td>
            </tr>

            <tr>
                <td><p>Below are the details of your EDDM Quote. This quote is valid until <asp:Literal ID="litQuoteExpiration" runat="server" />.</p></td>
            </tr>


            <%--Taradel Header info--%>
            <asp:PlaceHolder ID="phTaradeDisplay" runat="server" Visible="True">
                <tr>
                    <td>
                        <p>&nbsp;</p>
                        <p><appx:ReceiptHeaderImage runat="server" id="ReceiptHeaderImage" /></p>
                        <p><strong><asp:Literal ID="lReceiptPartnerName" runat="server" Visible="false" />
                        <asp:Literal runat="server" ID="lReceiptPartnerAddress" />
                        <asp:Literal ID="lReceiptPartnerPhone" runat="server" /></strong></p>
                    </td>
                </tr>
            </asp:PlaceHolder>


            <%--Quote Information--%>
            <asp:PlaceHolder ID="phQuoteInfoDisplay" runat="server" Visible="True">
                <tr class="pdfHeaderRow">
                    <td><strong>QUOTE FOR</strong></td>
                </tr>

                <tr>
                    <td>
                        <table id="orderInfoTable" class="pdfDataTable">

                            <tr>
                                <td class="width50"><asp:Literal ID="litName" runat="server" /></td>
                                <td class="width50 alignRight">Quote created on <asp:Literal ID="litQuoteDate" runat="server" /></td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="litCustCompany" runat="server" /></td>
                                <td class="width50 alignRight">&nbsp;</td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="litCustAddress" runat="server" /></td>
                                <td class="width50 alignRight">&nbsp;</td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="litCityStateZip" runat="server" /></td>
                                <td class="width50 alignRight">&nbsp;</td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="litPhone" runat="server" /></td>
                                <td class="width50 alignRight">&nbsp;</td>
                            </tr>

                            <tr>
                                <td class="width50"><asp:Literal ID="litCustNumLabel" runat="server" />:&nbsp;<asp:Literal ID="litCustomerID" runat="server" /></td>
                                <td class="width50 alignRight"><asp:Literal ID="litPONumberMsg" runat="server" />&nbsp;</td>
                            </tr>

                            <asp:Panel ID="pnlStoreNumber" runat="server" Visible="False">
                            <tr>
                                <td class="width50">Store Number: <asp:Literal ID="litStoreNumber" runat="server" /></td>
                                <td class="width50 alignRight">&nbsp;</td>
                            </tr>
                            </asp:Panel>

                        </table>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:PlaceHolder>


            <%--EDDM Product--%>
            <asp:PlaceHolder ID="phEddmProduct" runat="server" Visible="False">
                        
                <tr class="pdfHeaderRow">
                    <td>EDDM Product</td>
                </tr>

                <tr>

                    <td>

                        <table id="pdfEDDMProductTable" class="pdfDataTable">
                            <tr>
                                <td class="width33"><asp:Literal ID="litEddmProductName" runat="server" Visible="true" /></td>
                                <td class="width33">&nbsp;</td>
                                <td class="width33">

                                    <asp:Repeater ID="rEddmOpts" runat="server">

                                        <HeaderTemplate>
                                            <table id="pdfOptionsTable" class="pdfDataTable">
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <tr>
                                                <td class="width33"><small><asp:Literal ID="lOptCat" runat="server" Text='<%#Eval("Name") & ": "%>' /></small></td>
                                                <td class="width67"><small><asp:Literal ID="lOptVal" runat="server" Text='<%#Eval("ValueName") %>' /></small></td>
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

            </asp:PlaceHolder>

                    
            <%--AddressedList Product--%>
            <asp:PlaceHolder ID="phAddressedProduct" runat="server" Visible="False">
                        
                <tr class="pdfHeaderRow">
                    <td>AddressedList Product</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfAddressedProductTable" class="pdfDataTable">
                            <tr>
                                <td class="width33"><asp:Literal ID="litAddressedProductName" runat="server" Visible="true" /></td>
                                <td class="width33">&nbsp;</td>
                                <td class="width33">

                                    <asp:Repeater ID="rptAddressedOpts" runat="server">

                                        <HeaderTemplate>
                                            <table id="pdfOptionsTable" class="pdfDataTable">
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <tr>
                                                <td class="width33"><small><asp:Literal ID="lOptCat" runat="server" Text='<%#Eval("Name") & ": "%>' /></small></td>
                                                <td class="width67"><small><asp:Literal ID="lOptVal" runat="server" Text='<%#Eval("ValueName") %>' /></small></td>
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

            </asp:PlaceHolder>


            <%--Design Type--%>
            <asp:PlaceHolder ID="phDesigns" runat="server">
                <tr class="pdfHeaderRow">
                    <td>DESIGN</td>
                </tr>

                <tr>
                    <td>

                        <asp:PlaceHolder ID="phProDesign" runat="server" Visible="false">
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
                        </asp:PlaceHolder>

                        <asp:PlaceHolder ID="phMyDesign" runat="server" Visible="false">
                        
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
                        
                        </asp:PlaceHolder>

                    </td>

                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:PlaceHolder>


            <%--Map--%>
            <asp:PlaceHolder ID="phMap" runat="server">
                <tr class="pdfHeaderRow">
                    <td>MAP</td>
                </tr>
                
                <tr>
                    <td>

                        <table id="pdfMapTable" class="pdfDataTable">
                            <tbody>
                            <tr>
                                <td class="width33">
                                    <asp:Literal ID="lMapReview" runat="server" />
                                </td>
                                <td class="width33">
                                    <strong><asp:Literal ID="lSelectName" runat="server" Visible="true" /></strong>
                                </td>
                                <td class="width33">
                                    <asp:Literal ID="lSelectDescription" runat="server" />
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


            <%--EDDM Drops--%>
            <asp:PlaceHolder ID="phEDDMDrops" runat="server" Visible="False">

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

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:PlaceHolder>


            <%--Addressed Drops--%>
            <asp:PlaceHolder ID="phAddressedDrops" runat="server" Visible="False">

                <tr class="pdfHeaderRow pageBreak">
                    <td>ADDRESSED LIST DROPS</td>
                </tr>

                <tr>
                    <td>
                        <p><small>Here are the details regarding your AddressedList campaign:</small></p>
                    </td>
                </tr>


                <tr>
                    <td>

                        <table class="width100">
                            <tr>

                                <td class="width33">

                                    <p><small><strong>Pieces:</strong></small></p>

                                    <asp:Literal ID="litAddressedPcs" runat="server" />

                                </td>

                                <td class="width33">

                                    <p><small><strong>Drop Dates:</strong></small></p>

                                    <asp:Literal ID="litAddressedDropDates" runat="server" />

                                </td>

                                <td class="width33">

                                    <p><small><strong>Filters Applied:</strong></small></p>

                                    <asp:Literal ID="litDemographicFilters" runat="server" />

                                </td>

                            </tr>
                        </table>

                    </td>


                </tr>

                <tr class="hidden">
                    <td>

                        <asp:Repeater ID="rptAddressedDrops" runat="server">
                            <HeaderTemplate>
                                <table class="table table-striped table-hover table-bordered table-condensed" id="tblDrops">
                                    <thead>
                                        <tr>
                                            <th class="col-sm-1"><asp:Literal ID="lDropDate" runat="server" Text="Drop Date" /></th>
                                            <th class="col-sm-1"><asp:Literal ID="lDropNum" runat="server" Text="Drop Number" /></th>
                                            <th class="col-sm-1"><asp:Literal ID="lPiecesInDrop" Text="Pieces" runat="server" /></th>
                                            <th class="col-sm-9"><asp:Literal ID="lRoutes" runat="server" Text="Routes" /></th>
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
                                        <asp:Repeater ID="rAddressedRoutes" runat="server">
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

                    </td>
                </tr>


                <tr>
                    <td>&nbsp;</td>
                </tr>


            </asp:PlaceHolder>


            <%--Mailing Price--%>
            <asp:PlaceHolder ID="phMailingPrice" runat="server" Visible="True">
                     
                <tr class="pdfHeaderRow">
                    <td><asp:Literal ID="lMailingLabel" runat="server" Text="MAILING" /></td>
                </tr>
                   
                <tr>
                    <td>

                        <table id="pdfMailingTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><p class="text-center"><asp:Literal ID="litNumOfPcs" runat="server" /></p></td>
                                <td class="width33 alignRight"><strong><asp:Literal ID="lblPrintingEstimate" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:PlaceHolder>


            <%--Extra Pcs. Optional. --%>
            <asp:PlaceHolder ID="phExtraPcs" runat="server" Visible="false">
                        
                <tr class="pdfHeaderRow">
                    <td>ADDITIONAL PIECES</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfExtraPcsTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33">
                                    <strong><asp:Literal ID="litExtraPcs" runat="server" /></strong><br />
                                    Shipping To:<br />
                                    <asp:Literal ID="litShipTo" runat="server" />
                                </td>
                                <td class="width33 alignRight"><strong><asp:Label ID="lblShippingEstimate" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:PlaceHolder>
    
                            
            <%--Design Fee. Optional --%>
            <asp:PlaceHolder ID="phDesignFee" runat="server" Visible="false">

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

            </asp:PlaceHolder>
                    

            <%--Additional Drop Fee. Optional --%>
            <asp:PlaceHolder ID="phNumOfDrops" runat="server" Visible="false">

                <tr class="pdfHeaderRow">
                    <td>ADDITIONAL DROP FEES</td>
                </tr>

                <tr>
                    <td>

                        <table id="pdfDropFeeTable" class="pdfDataTable">
                            <tr>
                                <td class="width33">&nbsp;</td>
                                <td class="width33"><p class="text-center"><asp:Literal ID="litNumOfDrops" runat="server" /></p></td>
                                <td class="width33"><p class="text-right"><strong><asp:Label ID="lblNumOfDrops" runat="server" /></strong></p></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

            </asp:PlaceHolder>
      
                          
            <%--Sub Total--%>
            <asp:PlaceHolder ID="phSubTotalDisplay" runat="server" Visible="True">
                <tr class="pdfHeaderRow">
                    <td>
                        <table id="pdfSubTotalTable" class="width100">
                            <tr>
                                <td class="width50">SUBTOTAL</td>
                                <td class="width50 alignRight"><strong><asp:Literal ID="lblSubTotal" runat="server" /></strong></td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td><hr /></td>
                </tr>

            </asp:PlaceHolder>

            
            <%--Sales Tax. Optional --%>
            <asp:PlaceHolder ID="phSalesTax" runat="server" Visible="false">
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
            </asp:PlaceHolder>


            <%--Total--%>
            <asp:PlaceHolder ID="phTotalDisplay" runat="server" Visible="True">
                <tr class="pdfHeaderRow">
                    <td>
                        <table id="pdfTotalTable" class="width100">
                            <tr>
                                <td class="width25">TOTAL</td>
                                <td class="width50 pdfHeaderRowSmallText text-center">(This quote does not include Sales Tax.)</td>
                                <td class="width25 alignRight"><strong><asp:Label ID="TotalEstimate" runat="server" /></strong></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                </tr>
            </asp:PlaceHolder>
            

        </table>

        <p>&nbsp;</p>

        <p>&nbsp;</p>

   </asp:PlaceHolder>



</asp:Content>


