Imports System.Collections.Generic

Partial Class CCustom_SamplePricingTable
    Inherits CLibraryBase


    Protected debug As Boolean = False

    Private _ProductId As Integer = 0
    <appx.cms(appx.cmsAttribute.DataValueType.WLProduct)> _
    Public Property ProductId As Integer
        Get
            Return _ProductId
        End Get
        Set(value As Integer)
            _ProductId = value
        End Set
    End Property

    Private _QtyOverride As String = "1000,2500,5000,10000,25000,50000"
    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property QtyOverride As String
        Get
            Return _QtyOverride
        End Get
        Set(value As String)
            _QtyOverride = value
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        BuildControl()

    End Sub



    Protected Overrides Sub BuildControl()

        Dim bBust As Boolean = False
        Dim oBase As New appxCMS.PageBase
        Dim oQs As Hashtable = appxCMS.PageBase.RebuildQuerystring(oBase.GetRequestedURL, False)
        If oQs.ContainsKey("burst") Then
            bBust = True
        End If
        Dim sCK As String = "SamplePricingTable:" & appxCMS.Util.CMSSettings.GetSiteId & ":" & Me.ProductId
        Dim oQtyList As List(Of QtyListItem) = Nothing
        oQtyList = DirectCast(appxCMS.Util.Cache.GetObject(sCK), List(Of QtyListItem))

        Dim bLoad As Boolean = False
        If bBust Then
            bLoad = True
        ElseIf oQtyList Is Nothing Then
            bLoad = True
        ElseIf oQtyList.Count = 0 Then
            bLoad = True
        End If

        If bLoad Then
            oQtyList = New List(Of QtyListItem)

            Dim aSplit As Char() = {","}
            Dim aQty As String() = Me.QtyOverride.Split(aSplit)

            Dim oProd As Taradel.WLProduct = Taradel.WLProductDataSource.GetProductByBaseId(Me.ProductId)
            If oProd IsNot Nothing Then

                For iQtyIdx As Integer = 0 To aQty.Length - 1
                    Dim iQty As Integer = 0
                    If Integer.TryParse(aQty(iQtyIdx), iQty) Then
                        If iQty > 0 Then
                            oQtyList.Add(GetQtyQuote(oProd, iQty))
                        End If
                    End If
                Next


                If (debug) Then

                    pnlDebug.Visible = True
                    litDebug.Text = "Mark Up Amt: " & oProd.Markup & "<br />" & "Mark Up Type: " & oProd.MarkupType

                End If


                'oQtyList.Add(GetQtyQuote(oProd, 2500))
                'oQtyList.Add(GetQtyQuote(oProd, 5000))
                'oQtyList.Add(GetQtyQuote(oProd, 10000))
                'oQtyList.Add(GetQtyQuote(oProd, 25000))




            End If

            appxCMS.Util.Cache.Add(sCK, oQtyList)


        End If

        lvPricing.DataSource = oQtyList
        lvPricing.DataBind()


    End Sub



    Protected Function GetQtyQuote(oProd As Taradel.WLProduct, Qty As Integer) As QtyListItem
        Dim oRet As New QtyListItem

        Dim oQuote As New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProd.BaseProductID, Qty, 0, 1, Nothing, "", False, False, oProd.Markup, oProd.MarkupType)
        If oQuote IsNot Nothing Then
            oRet.Quantity = Qty
            oRet.PricePerPiece = oQuote.FormattedPricePerPiece
        End If
        Return oRet
    End Function

    
End Class
