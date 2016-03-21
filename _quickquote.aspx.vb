Imports System.Collections.Generic

Partial Class _quickquote
    Inherits appxCMS.PageBase

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim oProds As List(Of Taradel.WLProduct) = Taradel.WLProductDataSource.GetProducts()
        radProduct.DataValueField = "ProductId"
        radProduct.DataTextField = "Name"
        radProduct.DataSource = oProds
        radProduct.DataBind()
    End Sub

    Protected Sub lnkGetEstimate_Click(sender As Object, e As System.EventArgs) Handles lnkGetEstimate.Click

        Dim oSb As New StringBuilder
        oSb.AppendLine("<div class=""well well-lg"">")

        oSb.AppendLine("<table class=""table table-bordered table-striped table-hover""><thead>")
        oSb.AppendLine("<tr><th class=""priceHeader"">Quantity</th><th class=""priceHeader"">Price/Piece</th></tr></thead>")
        oSb.AppendLine("<tbody>")

        Dim ProductId As Integer = 0
        Integer.TryParse(radProduct.SelectedValue, ProductId)
        If ProductId > 0 Then
            Dim aQty As New ArrayList
            aQty.Add(1000)
            aQty.Add(2500)
            aQty.Add(5000)
            aQty.Add(10000)

            For i As Integer = 0 To aQty.Count - 1
                Dim qty As Integer = aQty(i)

                If qty > 0 Then
                    Dim oProd As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(ProductId)
                    Dim oQuote As New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProd.BaseProductID, qty, 0, 1, Nothing, "", False, False, oProd.Markup, oProd.MarkupType)
                    oSb.AppendLine("<tr>")
                    oSb.AppendLine("<td class=""text-center"">" & qty.ToString("N0") & "</td>")
                    oSb.AppendLine("<td class=""text-center"">" & oQuote.FormattedPricePerPiece & "</td>")
                    oSb.AppendLine("</tr>")
                End If
            Next

        End If
        oSb.AppendLine("</tbody></table>")
        oSb.AppendLine("</div>")
        lEstimate.Text = oSb.ToString
    End Sub
End Class
