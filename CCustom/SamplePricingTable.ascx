<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SamplePricingTable.ascx.vb" Inherits="CCustom_SamplePricingTable" %>

<asp:ListView id="lvPricing" runat="server" ItemPlaceholderId="phItemTemplate">

    <LayoutTemplate>
        <table class="table table-striped table-bordered table-hover">
            <thead>
                <tr>
                    <th class="priceHeader">Quantity</th>
                    <th class="priceHeader">Price/Piece</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="phItemTemplate" runat="server" />
            </tbody>
        </table>    
    </LayoutTemplate>

    <ItemTemplate>
        <tr>
            <td><%#Eval("Quantity", "{0:N0}")%></td>
            <td><%#Eval("PricePerPiece") %></td>
        </tr>
    </ItemTemplate>

    <EmptyDataTemplate>
        <p>Please contact a sales representative for pricing.</p>
    </EmptyDataTemplate>

</asp:ListView>

<asp:Panel ID="pnlDebug" runat="server" Visible="False" CssClass="alert alert-info">
    <asp:Literal ID="litDebug" runat="server"></asp:Literal>
</asp:Panel>
