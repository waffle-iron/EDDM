<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EDDMCredentials.ascx.cs" Inherits="Controls_EDDMCredentials" %>
<%@ Register Src="~/Controls/FacebookLikes.ascx" TagPrefix="appx" TagName="FacebookLikes" %>

<%--Extends full width of page. No padding.--%>
<div class="whitePaddedWrapper" id="credentialsWrapper">
    <div class="container-fluid">
    
        <div class="row">

            <div class="col-xs-6 col-md-2 text-center">
                <asp:Image ID="imgUSPS" runat="server" ImageUrl="~/assets/images/usps_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="USPS" ToolTip="USPS" />
            </div>

            <div class="col-xs-6 col-md-2 text-center">
                <a href="http://www.bbb.org/richmond/business-reviews/image-and-graphics-printing/taradel-llc-in-glen-allen-va-63391727" target="_blank">
                <asp:Image ID="imgBBB" runat="server" ImageUrl="~/assets/images/bbb_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="BBB" ToolTip="BBB" />
                </a>
            </div>
            
            <div class="col-xs-6 col-md-2 text-center">
                <asp:Image ID="imgNPF" runat="server" ImageUrl="~/assets/images/npf_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="NPF" ToolTip="NPF" />
            </div>

            <div class="col-xs-6 col-md-2 text-center">
                <asp:Image ID="imgINC" runat="server" ImageUrl="~/assets/images/inc5000_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="INC5000" ToolTip="INC5000" />
            </div>

            <div class="col-xs-6 col-md-2 text-center">
                <asp:Image ID="imgAuthNet" runat="server" ImageUrl="~/assets/images/authnet_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="Authorize.Net" ToolTip="Authorize.Net" />
            </div>

            <div class="col-xs-6 col-md-2 text-center">
                <br />
                <appx:FacebookLikes runat="server" id="FacebookLikes" />
            </div>

        </div>

    </div>
</div>


<script type="text/javascript">

    $(function ()
    {
        $("#imgNPF").mouseover(function ()
        { this.src = "/assets/images/npf_color.png" }).mouseout(function () { this.src = "/assets/images/npf_bw.png" });

        $("#imgINC").mouseover(function ()
        { this.src = "/assets/images/inc5000_color.png" }).mouseout(function () { this.src = "/assets/images/inc5000_bw.png" });

        $("#imgAuthNet").mouseover(function ()
        { this.src = "/assets/images/authnet_color.png" }).mouseout(function () { this.src = "/assets/images/authnet_bw.png" });

        $("#imgUSPS").mouseover(function ()
        { this.src = "/assets/images/usps_color.png" }).mouseout(function () { this.src = "/assets/images/usps_bw.png" });

        $("#imgBBB").mouseover(function ()
        { this.src = "/assets/images/bbb_color.png" }).mouseout(function () { this.src = "/assets/images/bbb_bw.png" });

    });

</script>

