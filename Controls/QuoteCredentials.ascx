<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuoteCredentials.ascx.cs" Inherits="Controls_EDDMCredentials" %>

<asp:Panel ID="pnlCredentials" runat="server">

    <div id="quoteCredentials">

        <div class="quoteSideBarHeader">A Trusted Provider</div>

        <div class="row">
            <div class="col-sm-6">
                <div class="text-center">
                    <asp:Image ID="imgUSPS" runat="server" ImageUrl="~/assets/images/usps_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="USPS" ToolTip="USPS" />
                </div>
            </div>

            <div class="col-sm-6">
                <div class="text-center">
                    <asp:Image ID="imgBBB" runat="server" ImageUrl="~/assets/images/bbb_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="BBB" ToolTip="BBB" />
                </div>
            </div>
        </div>

        <div>&nbsp;</div>

        <div class="row">
            <div class="col-sm-6">
                <div class="text-center">
                    <asp:Image ID="imgNPF" runat="server" ImageUrl="~/assets/images/npf_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="NPF" ToolTip="NPF" />
                </div>
            </div>

            <div class="col-sm-6">
                <div class="text-center">
                    <asp:Image ID="imgINC" runat="server" ImageUrl="~/assets/images/inc5000_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="INC5000" ToolTip="INC5000" />
                </div>
            </div>
        </div>

        <div>&nbsp;</div>

        <div class="row">
            <div class="col-sm-12">
                <div class="text-center">
                <asp:Image ID="imgAuthNet" runat="server" ImageUrl="~/assets/images/authnet_bw.png" CssClass="img-responsive center-block" ClientIDMode="Static" AlternateText="Authorize.Net" ToolTip="Authorize.Net" />
                </div>
            </div>
        </div>


        <script type="text/javascript">

            $(function () {
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

    </div>

</asp:Panel>

