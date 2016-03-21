<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/QuoteRequest.master" AutoEventWireup="false" CodeFile="QuoteConfirm.aspx.vb" Inherits="EDDMQuote" %>
<%@ Register Src="~/CCustom/BoldChatConversion.ascx" TagPrefix="appx" TagName="BoldChatConversion" %>
<%@ Register Src="~/CCustom/BingConversionCode.ascx" TagPrefix="appx" TagName="BingConversionCode" %>




<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="phHeaderLg" Runat="Server">
    <asp:Literal ID="litConfirmHeaderLg" runat="server" />
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="phHeaderSm" Runat="Server">
    <asp:Literal ID="litConfirmHeaderSm" runat="server" />
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">

    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
        <i class="fa fa-2x fa-exclamation-circle pull-left"></i>&nbsp;
        <asp:Literal ID="litErrorMessage" runat="server" />
        <p>&nbsp;</p>
    </asp:Panel>



    <%--Required Hidden fields--%>
    <asp:Panel ID="pnlHiddenFields" runat="server">
        <div class="row">
            <div class="col-sm-12">
                <asp:HiddenField ID="hidQuoteType" runat="server" ClientIDMode="Static" />
            </div>
        </div>
    </asp:Panel>
    


    <asp:Panel ID="pnlSuccess" runat="server" Visible="False">

        <h3>You are interested in mailing <asp:literal id="lQty" runat="server" /> pieces using the <asp:literal id="lProduct" runat="server" />.</h3>

        <div class="row">
            <div class="col-sm-9">
                <asp:Literal ID="lProductDescription" runat="server" />
            </div>
            <div class="col-sm-3">

                <asp:Panel ID="pImage" runat="server">
                    <asp:Image ID="imgProduct" runat="server" CssClass="img-responsive" />
                </asp:Panel>

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
                <asp:Panel ID="pSamples" runat="server" Visible="false">
                    <div><asp:CheckBox ID="chkGetSamples" runat="server" /></div>
                    <div>Please send me a <b>FREE</b> sample kit</div>
                </asp:Panel>

                <%--Possible needs styling--%>
                <asp:Panel ID="pConsult" runat="server" Visible="false">
                    <div><asp:CheckBox ID="chkConsult" runat="server" /></div>
                    <div>Please contact me to schedule a <b>FREE</b> 15-minute, no obligation marketing consultation.</div>
                </asp:Panel>

                <div>&nbsp;</div>

                <div class="jumbotron">
                    <h3>Are you ready to get started?</h3>

                    <div class="checkbox">
                        <label>
                            <asp:checkbox id="chkEmail" runat="server" Checked="true" /> Please send me a copy of this quote by e-mail to <asp:literal id="lEmail" runat="server" />.
                        </label>
                    </div>

                </div>

            </div>
        </div>

        <div class="row">
            <div class="col-sm-6 col-sm-offset-6">
                <asp:LinkButton ID="lnkGoToMap" runat="server" CssClass="btn btn-danger btn-lg">
                    <span class="fa fa-envelope"></span>&nbsp;Save and continue to my map...
                </asp:LinkButton>
            </div>
        </div>

        <div>&nbsp;</div>

    </asp:Panel>

    <%--These must stay within the body. phHead and phFooter are marked to be invisible in the master page.--%>
    <appx:BoldChatConversion runat="server" id="BoldChatConversion" />
    <appx:BingConversionCode runat="server" id="BingConversionCode" />

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>


