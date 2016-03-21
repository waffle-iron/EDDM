<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/QuoteRequest.master" AutoEventWireup="false" CodeFile="EDDMQuoteAlt.aspx.vb" Inherits="EDDMQuoteAlt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHeaderLg" Runat="Server">
    Here is your <u>free</u> Instant Every Door Direct Mail&reg; Quote!
</asp:Content>


<asp:Content ID="Content7" ContentPlaceHolderID="phHeaderSm" Runat="Server">
    Here is your <u>free</u> Instant Every Door Direct Mail&reg; Quote!
</asp:Content>


<asp:content id="Content4" contentplaceholderid="phBody" runat="Server">

    <h3>You are interested in mailing <asp:literal id="lQty" runat="server" /> pieces using the <asp:literal id="lProduct" runat="server" />.</h3>

    <div class="row">
        <div class="col-sm-9">
            <asp:literal id="lProductDescription" runat="server" />
        </div>
        <div class="col-sm-3">
            <asp:panel id="pImage" runat="server">
                <asp:image id="imgProduct" runat="server" CssClass="img-responsive" />
            </asp:panel>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-6 col-sm-offset-3">
                    
            <h3>Order More, Save More!</h3>

            <div class="well well-lg">
                <asp:literal id="lQuote" runat="server" />
            </div>

            <div>&nbsp;</div>

        </div>
    </div>

    <div class="row">
        <div class="col-sm-12">


            <asp:Literal ID="lSubmitted" runat="server" />

            <%--Possibly needs styling--%>
            <asp:Panel id="pSamples" runat="server" Visible="false">
                <div><asp:checkbox id="chkGetSamples" runat="server" /></div>
                <div>Please send me a <strong>FREE</strong> sample kit</div>
            </asp:Panel>

            <%--Possible needs styling--%>
            <asp:Panel id="pConsult" runat="server" Visible="false">
                <div><asp:checkbox id="chkConsult" runat="server" /></div>
                <div>Please contact me to schedule a <strong>FREE</strong> 15-minute, no obligation marketing consultation.</div>
            </asp:Panel>

            <div>&nbsp;</div>

            <div class="jumbotron">
                <h3>Are you ready to get started?</h3>

                <div class="checkbox">
                    <label>
                        <asp:checkbox id="chkEmail" runat="server" Checked="true" /> Please send me a copy of this quote by e-mail to <asp:literal id="lEmail" runat="server" />.
                    </label>
                </div>

                <p class="text-right">
                    <asp:linkbutton id="lnkSave" runat="server" CssClass="btn btn-danger btn-lg">
                        <span class="fa fa-envelope"></span>&nbsp;Send Me A Copy
                    </asp:linkbutton>
                </p>

            </div>

        </div>
    </div>

    <div>&nbsp;</div>
    
</asp:content>


