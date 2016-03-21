<%@ Page Title="Success! Your order has been received" Language="VB" MasterPageFile="~/App_MasterPages/Site.master"
    AutoEventWireup="false" CodeFile="Receipt-Orig.aspx.vb" Inherits="Receipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">
    <asp:Panel ID="pError" runat="server" Visible="false">
        <asp:Literal ID="lErrorMsg" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pCheckout" runat="server">
        <div class="ui-helper-clearfix padtop">
            <div class="contentBlock">
                <div class="banner-flag ui-helper-clearfix">
                    <ul>
                        <li class="first">
                            <h1>
                                My Order</h1>
                        </li>
                        <li class="last">
                            <h2>
                                Your Order Is Complete</h2>
                        </li>
                    </ul>
                    <div class="shd-left">
                        &nbsp;</div>
                    <div class="shd-right">
                        &nbsp;</div>
                </div>
                <div class="ui-helper-clearfix">
                    <div class="pricerow">
                        <h1>
                            Your Order Number is
                            <asp:Literal runat="server" ID="lOrderNumber" /></h1>
                        <p>
                            <b>Next Steps: </b>Our expert team will review your order and contact you shortly.
                            No further action is required from you at this time.</p>
                        <p>
                            If you have any questions, contact our help line at <asp:Literal id="lPhoneNumber" runat="server"/>.</p>
                    </div>
                </div>
                <div class="ui-helper-clearfix">
                    <div class="thirty fleft">
                        <div class="padright2 ui-helper-clearfix">
                            <asp:Literal ID="lMapReview" runat="server" />
                            <h2>
                                <asp:Literal ID="lSelectName" runat="server" /></h2>
                            <p>
                                <asp:Literal ID="lSelectDescription" runat="server" /></p>
                            <div class="row">
                                <div class="label">
                                    Job Name:</div>
                                <div class="aright">
                                    <asp:Literal ID="lJobName" runat="server" /></div>
                            </div>
                            <asp:Panel ID="pComments" runat="server" CssClass="row">
                                <div class="label">
                                    Comments:</div>
                                <div class="aright">
                                    <asp:Literal ID="lComments" runat="server" /></div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="seventy fleft">
                        <h1>
                            <asp:Literal ID="lProductName" runat="server" /></h1>
                        <div class="row">
                            <div class="label">
                                Quantity:</div>
                            <div class="aright">
                                <asp:Literal ID="lQty" runat="server" />
                                pieces to direct mail</div>
                        </div>
                        <asp:Repeater ID="rOpts" runat="server">
                            <ItemTemplate>
                                <div class="row">
                                    <div class="label">
                                        <asp:Literal ID="lOptCat" runat="server" Text='<%#Eval("Name") & ":" %>' /></div>
                                    <div class="aright">
                                        <asp:Literal ID="lOptVal" runat="server" Text='<%#Eval("ValueName") %>' /></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <hr />
                        <asp:PlaceHolder ID="phEarliest" runat="server" Visible="false">
                            <div class="row">
                                <div class="label">
                                    Design Option:</div>
                                <div class="aright">
                                    Professional Design Service</div>
                            </div>
                            <div class="row" style="display:none;">
                                <div class="label">
                                    Earliest Delivery:</div>
                                <div class="aright">
                                    Based on your selections, your pieces will be presented at the post office as early
                                    as
                                    <asp:Label ID="EarliestDelivery" runat="server" />. <span id="Span1" />
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phMyDesign" runat="server" Visible="false">
                            <div class="row">
                                <div class="label">
                                    My Design:</div>
                                <div class="aright">
                                    <asp:Literal ID="lLaterMsg" runat="server" />
                                    <div class="ui-helper-clearfix">
                                        <div class="fifty fleft">
                                            <asp:Image ID="imgFile1" runat="server" Visible="false" /></div>
                                        <div class="fifty fleft">
                                            <asp:Image ID="imgFile2" runat="server" Visible="false" /></div>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <asp:Repeater ID="rDrops" runat="server">
                            <ItemTemplate>
                                <div class="row">
                                    <div class="label">
                                        Drop #
                                        <asp:Literal ID="lDropNumber" runat="server" Text='<%#Eval("Number") %>' />:</div>
                                    <div class="aright">
                                        <asp:Literal ID="lDropCount" runat="server" Text='<%#Integer.Parse(Eval("Total")).ToString("N0") %>' />
                                        pieces should reach your target audience the week of 
                                        <asp:Literal ID="lDropDate" runat="server" Text='<%# ReturnTheNextMonday(Eval("Date"))%>' />
                                        <div>
                                            Delivery To:
                                            <asp:Repeater ID="rRoutes" runat="server">
                                                <ItemTemplate>
                                                    <asp:Literal ID="lRoute" runat="server" Text='<%#Eval("Name") %>' /></ItemTemplate>
                                                <SeparatorTemplate>
                                                    ,
                                                </SeparatorTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <hr />
                        <div class="center">
                            <h2>
                                <asp:Literal ID="lPaymentMessage" runat="server" /></h2>
                            <asp:Repeater ID="rPayments" runat="server">
                                <ItemTemplate>
                                    <div>
                                        <%#Taradel.Util.NumberHelp.FormatTextCount(Container.ItemIndex + 2)%>
                                        Payment
                                        <asp:Literal ID="lPayDate" runat="server" Text='<%#DateTime.Parse(Eval("Key")).ToString("dddd, dd MMM yyyy") %>' />:
                                        <b>
                                            <asp:Literal ID="lPayAmount" runat="server" Text='<%#Decimal.Parse(Eval("Value").ToString()).ToString("C") %>' /></b></div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--Bing Conversion Tracking Code--%>
        <script type="text/javascript">            if (!window.mstag) mstag = { loadTag: function () { }, time: (new Date()).getTime() };</script>
        <script id="mstag_tops" type="text/javascript" src="//flex.msn.com/mstag/site/70284dd6-17b4-4493-b514-94072ce299d0/mstag.js"></script>
        <script type="text/javascript">            mstag.loadTag("analytics", { dedup: "1", domainId: "289704", type: "1", actionid: "153847" })</script>
        <noscript>
            <iframe src="//flex.msn.com/mstag/tag/70284dd6-17b4-4493-b514-94072ce299d0/analytics.html?dedup=1&domainId=289704&type=1&actionid=153847"
                frameborder="0" scrolling="no" width="1" height="1" style="visibility: hidden;
                display: none"></iframe>
        </noscript>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>
