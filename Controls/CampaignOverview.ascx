<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CampaignOverview.ascx.cs" Inherits="Controls_CampaignOverview" %>


<%--Campaign Overview--%>
<div class="panel panel-primary" id="campaignOverview">
    <div class="panel-heading">
        Campaign Overview
    </div>
                    
    <div class="panel-body">

        <div class="alert alert-warning hidden" id="warningBlock">
            <span class="fa fa-warning text-warning"></span>&nbsp;<asp:Label ID="lblWarningMessage" runat="server" Text="Label" ClientIDMode="Static" />
        </div>

        <p>This is your campaign so far. It will update as you update your options.</p>

        <table class="table table-bordered table-hover table-condensed detailedData">

            <%--Product--%>
            <tr>
                <td class="col-sm-5">Product</td>
                <td class="col-sm-7"><asp:Label ID="lblProductName" runat="server" ClientIDMode="Static" /></td>
            </tr>

            <%--Pieces Mailed--%>
            <tr>
                <td class="col-sm-5">Pieces Mailed</td>
                <td class="col-sm-7"><asp:Label ID="lblPiecesMailed" runat="server" ClientIDMode="Static" /></td>
            </tr>

            <%--Extra copies--%>
            <tr>
                <td class="col-sm-5">Extra Copies</td>
                <td class="col-sm-7"><asp:Label ID="lblExtraCopies" runat="server" ClientIDMode="Static" /></td>
            </tr>

            <%--Price Per Piece--%>
            <tr class="overviewTotalsRow">
                <td class="col-sm-5"><strong>Price Per Piece</strong></td>
                <td class="col-sm-7"><strong><asp:Label ID="lblPricePerPiece" runat="server" ClientIDMode="Static" /></strong></td>
            </tr>

            <%--Total--%>
            <tr class="overviewTotalsRow">
                <td class="col-sm-5"><strong>Est. Total</strong></td>
                <td class="col-sm-7"><strong><asp:Label ID="lblEstTotal" runat="server" ClientIDMode="Static" /></strong></td>
            </tr>

        </table>

        <p><small>(Other options and Sales Tax on the final step may affect the Total Cost.)</small></p>

        <p><small>(Postage is not charged on Extra Copies.)</small></p>

        <div>&nbsp;</div>

    </div>

</div>



<script src="/assets/javascripts/CampaignOverview.min.js?ver=1.0.2"></script>
