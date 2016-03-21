<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProductListWithQuote.ascx.vb" Inherits="CLibrary_ProductListWithQuote" %>
<%@ Register TagPrefix="taradel" TagName="SamplePricingTable" Src="~/CCustom/SamplePricingTable.ascx" %>


<%--Panel shows when coming from Design page....--%>
<asp:Panel ID="pTemplate" runat="server" Visible="false">
   
    <div class="row">
        <div class="col-sm-6">

            <div class="panel panel-default">
                <div class="panel-body">
    
                    <div class="text-center">
                        <asp:HyperLink ID="hplImageOrder" runat="server">
                            <asp:Image ID="prodImage" runat="server" CssClass="img-responsive" />
                        </asp:HyperLink>
                    </div>
                                            
                    <div class="productTitle text-center"><asp:HyperLink ID="hplTitleOrder" runat="server"><%#Eval("WLProduct.Name")%></asp:HyperLink></div>

                    <p class="lead"><mark><asp:Literal ID="litQuote" runat="server" /></mark></p>

                    <div class="text-center">
                        <asp:HyperLink ID="hplOrder" runat="server" Text="Order Now" CssClass="btn btn-danger btn-lg">
                            <span class="glyphicon glyphicon-ok"></span>&nbsp;Order Now
                        </asp:HyperLink>
                    </div>

                </div>
            </div>

        </div>

        <div class="col-sm-6">

            <div class="well">

                <p class="lead">Based on your template selection, you should order the <strong><asp:Literal ID="lTemplateProdName" runat="server" /></strong>.</p>
                <p>Your selected template:</p>
                <asp:Image ID="imgTemplateFront" runat="server" />
                <asp:Image ID="imgTemplateInside" runat="server" />
                <asp:Image ID="imgTemplateBack" runat="server" />

                <br /><br />

                <p><em>More choices are below....</em></p>

            </div>

        </div>
    </div>

</asp:Panel>


<%--Products ListView--%>
<asp:ListView id="lvProducts" runat="server" ItemPlaceHolderID="phItemTemplate">

    <LayoutTemplate>
        <div class="row">
            <asp:PlaceHolder id="phItemTemplate" runat="server" />
        </div>        
    </LayoutTemplate>

    <ItemTemplate>

        <div class="col-sm-12">

            <div class="panel panel-default" runat="server" id="pnlQuote">

                <div class="panel-body">

                    <div class="row">
                        <div class="col-md-12">
                            <div class="productTitle extraBottomPadding"><asp:HyperLink ID="hplTitleOrder" runat="server"><%#Eval("WLProduct.Name")%></asp:HyperLink></div>
                        </div>
                    </div>

                    <div class="row">

                        <div class="col-sm-2 text-center">
                            <asp:HyperLink ID="hplImageOrder" runat="server">
                                <asp:Image ID="prodImage" runat="server" CssClass="img-responsive" />
                            </asp:HyperLink>
                        </div>

                        <div class="col-sm-6">
                            <asp:Literal ID="litQuote" runat="server" /><br />

                            <p class="text-center">
                                <small>
                                    <asp:LinkButton ID="btnAboutProduct" runat="server" CausesValidation="false" data-target="#moreInfoModal" data-toggle="modal" data-action="infowindow" ClientIDMode="Static">
                                        <span class="fa fa-info-circle"></span>&nbsp;Tell me about this product
                                    </asp:LinkButton>
                                </small>
                            </p>



                        </div>
                        
                        <div class="col-md-4">
                       
                            <p class="text-center">
                                <asp:HyperLink ID="hplOrder" runat="server" Text="Order Now" CssClass="btn btn-block btn-cta btn-lg btn-shadow">
                                    Next Step&nbsp;<span class="fa fa-chevron-right"></span>
                                </asp:HyperLink>
                            </p>
     
                            <p class="text-center">
                                <button type="button" class="btn-block btn btn-primary btn-sm btn-shadow" data-toggle="modal" data-target="#priceBreaksModal-<%#Eval("WLProduct.BaseProductId") %>">
                                    <span class="fa fa-search"></span>&nbsp;View Price Breaks
                                </button>
                            </p>

                        </div>

                    </div>


                    <%--Price Breaks Modal--%>
                    <div class="modal fade" id="priceBreaksModal-<%#Eval("WLProduct.BaseProductId") %>" tabindex="-1" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
        
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title"><%#Eval("WLProduct.Name")%>&nbsp;Price Breaks</h4>
                                </div>

                                <div class="modal-body">
                                    <asp:Panel ID="pPricingGrid" runat="server" ToolTip='<%#Eval("WLProduct.Name") & " Price Breaks" %>'>
                    
                                    <Taradel:SamplePricingTable ID="SamplePricingTable" runat="server" ProductId='<%#Eval("WLProduct.BaseProductId") %>' />

                                    </asp:Panel>
                                </div>

                                <div class="modal-footer">
                                    <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">Close</button>
                                </div>
                            </div>
                       
                        </div>

                    </div>


                </div>

            </div>

        </div>


    </ItemTemplate>

</asp:ListView>




<%--Savings Upsell Panel--%>
<asp:Panel ID="pSavings" runat="server" Visible="false">
    <div class="well">

        <p class="lead"><strong>
        <span class="fa fa-dollar fa-2x text-success"></span>
        &nbsp;Wait! You can order more and save BIG</strong></p>

        <p><strong>Take advantage of our tiered pricing!</strong> For example, if you order the <asp:Literal ID="lSavingsProductName" runat="server" />&nbsp; 
        and increase your quantity to <asp:Literal ID="lSavingsNextRange" runat="server" />, you'll reach&nbsp;
        <asp:Literal ID="lSavingsAddressExtra" runat="server" /> more addresses for a total of&nbsp;
        <asp:Literal ID="lSavingsNewQuoteTotal" runat="server" />. That's a savings of <asp:Literal ID="lSavingsAmount" runat="server" />&nbsp;
        and your price per piece drops from <asp:Literal ID="lSavingsOldPricePerPiece" runat="server" />
        &nbsp;to&nbsp;
        <asp:Literal ID="lSavingsNewPricePerPiece" runat="server" />.</p>

        <p class="text-center">
            <asp:LinkButton ID="btnAddMore" runat="server" CssClass="btn btn-danger btn-lg" OnClick="AddMore">
                <span class="glyphicon glyphicon-plus"></span>&nbsp;Add More Addresses & Save
            </asp:LinkButton>
        </p>

    </div>
</asp:Panel>



<%--Debugging--%>
<asp:Panel ID="pnlDebug" runat="server" CssClass="alert alert-danger" Visible="False">
    <asp:Label ID="lblDebug" runat="server" Text="" CssClass="" />
    <div>&nbsp;</div>
</asp:Panel>


