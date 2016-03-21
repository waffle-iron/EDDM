Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq


Partial Class MyAccount_account_orders
    Inherits MyAccountBase

    'Fields
    Protected SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim oOrders As List(Of Taradel.ExtendedOrderHeaderStatu) = Taradel.WLOrderDataSource.GetOrders(GetCustomerId)

        'Site Details Object
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


        'Set Page Header control
        If Not (SiteDetails.UseRibbonBanners) Then
            PageHeader.headerType = "simple"
            PageHeader.simpleHeader = "My Orders"
        Else
            PageHeader.headerType = "full"
            PageHeader.fullHeader = "My Orders"
        End If



        If oOrders.Count > 0 Then

            'Staples Act Mgr and Staples Store ONLY
            If ((SiteID = 78) Or (SiteID = 91)) Then
                lvOrders2.DataSource = oOrders
                lvOrders2.DataBind()
                lvOrders2.Visible = True
            ElseIf (SiteID = 98) 'change to Ramp when built
                lvOrdersLocked.DataSource = oOrders
                lvOrdersLocked.DataBind()
                lvOrdersLocked.Visible = True
            Else
                lvOrders.DataSource = oOrders
                lvOrders.DataBind()
                lvOrders.Visible = True
            End If

        Else
            pnlNoOrders.Visible = True
        End If


    End Sub



    Protected Sub lvOrders_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvOrders.ItemDataBound

        'For all WL sites expect Staples
        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim oDItem As ListViewDataItem = e.Item
            Dim oOrder As Taradel.ExtendedOrderHeaderStatu = DirectCast(oDItem.DataItem, Taradel.ExtendedOrderHeaderStatu)

            Dim stFileName As String = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/ReceiptPDFViewer.aspx?guid=" + oOrder.OrderGUID + "&id=" + oOrder.OrderID.ToString()
            Dim hypReceipt As HyperLink = DirectCast(e.Item.FindControl("hypReceipt"), HyperLink)
            hypReceipt.NavigateUrl = stFileName

            Dim DistID As String = TaradelReceiptUtility.RetrieveDistID(oOrder.OrderID).ToString()
            Dim hypMapName = DirectCast(e.Item.FindControl("hypMapName"), HyperLink)
            Dim mapName As String = TaradelReceiptUtility.RetrieveMapName(oOrder.OrderID)
            hypMapName.Text = mapName

            'OLB.
            If SiteID = 11 Then
                hypMapName.NavigateUrl = "~/olb/TargetDataMap1.aspx?distid=" & DistID
                'Everyone else
            Else
                hypMapName.NavigateUrl = "~/Step1-Target.aspx?distid=" & DistID
            End If


            Dim EDDMPrinted As Integer = TaradelReceiptUtility.RetrieveEDDMPrintedQuantity(oOrder.OrderID)
            Dim litEDDMPrinted As Literal = DirectCast(e.Item.FindControl("PrintQty"), Literal)
            If litEDDMPrinted IsNot Nothing Then
                litEDDMPrinted.Text = EDDMPrinted.ToString("N0")
            End If


            Dim RewardsNumber As Literal = DirectCast(e.Item.FindControl("RewardsNumber"), Literal)
            If RewardsNumber IsNot Nothing Then
                RewardsNumber.Text = GetPONumber(oOrder.OrderID)
            End If

            Dim dMarkup As Decimal = 0
            If oOrder.WLMarkup.HasValue Then
                dMarkup = oOrder.WLMarkup.Value
            End If


            Dim dOrderAmount As Double = oOrder.OrderAmt.Value + dMarkup
            Dim OrderAmount As Literal = DirectCast(e.Item.FindControl("OrderAmount"), Literal)
            If OrderAmount IsNot Nothing Then

                OrderAmount.Text = dOrderAmount.ToString("C")
            End If

            Dim dPaid As Double = 0
            If oOrder.PaidAmt.HasValue Then
                dPaid = oOrder.PaidAmt.Value
            End If

            Dim dBalance As Double = dOrderAmount - dPaid
            Dim OrderBalance As Literal = DirectCast(e.Item.FindControl("OrderBalance"), Literal)
            If OrderBalance IsNot Nothing Then
                OrderBalance.Text = dBalance.ToString("C")
            End If

        End If
    End Sub



    Protected Sub lvOrders2_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvOrders2.ItemDataBound

        'For Staples (only)

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim oDItem As ListViewDataItem = e.Item
            Dim oOrder As Taradel.ExtendedOrderHeaderStatu = DirectCast(oDItem.DataItem, Taradel.ExtendedOrderHeaderStatu)

            Dim stFileName As String = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/ReceiptPDFViewer.aspx?guid=" + oOrder.OrderGUID + "&id=" + oOrder.OrderID.ToString()
            Dim hypReceipt As HyperLink = DirectCast(e.Item.FindControl("hypReceipt"), HyperLink)
            hypReceipt.NavigateUrl = stFileName

            Dim DistID As String = TaradelReceiptUtility.RetrieveDistID(oOrder.OrderID).ToString()
            Dim hypMapName = DirectCast(e.Item.FindControl("hypMapName"), HyperLink)
            Dim mapName As String = TaradelReceiptUtility.RetrieveMapName(oOrder.OrderID)
            hypMapName.Text = mapName
            hypMapName.NavigateUrl = "~/Step1-Target.aspx?distid=" & DistID


            Dim EDDMPrinted As Integer = TaradelReceiptUtility.RetrieveEDDMPrintedQuantity(oOrder.OrderID)
            Dim litEDDMPrinted As Literal = DirectCast(e.Item.FindControl("PrintQty"), Literal)
            If litEDDMPrinted IsNot Nothing Then
                litEDDMPrinted.Text = EDDMPrinted.ToString("N0")
            End If



            Dim RewardsNumber As Literal = DirectCast(e.Item.FindControl("RewardsNumber"), Literal)
            If RewardsNumber IsNot Nothing Then
                RewardsNumber.Text = GetPONumber(oOrder.OrderID)
            End If

            Dim dMarkup As Decimal = 0
            If oOrder.WLMarkup.HasValue Then
                dMarkup = oOrder.WLMarkup.Value
            End If

            Dim dOrderAmount As Double = oOrder.OrderAmt.Value + dMarkup
            Dim OrderAmount As Literal = DirectCast(e.Item.FindControl("OrderAmount"), Literal)
            If OrderAmount IsNot Nothing Then

                OrderAmount.Text = dOrderAmount.ToString("C")
            End If

            Dim dPaid As Double = 0
            If oOrder.PaidAmt.HasValue Then
                dPaid = oOrder.PaidAmt.Value
            End If


            Dim dBalance As Double = dOrderAmount - dPaid
            Dim OrderBalance As Literal = DirectCast(e.Item.FindControl("OrderBalance"), Literal)
            If OrderBalance IsNot Nothing Then
                OrderBalance.Text = dBalance.ToString("C")
            End If

        End If
    End Sub


    Protected Sub lvOrdersLocked_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvOrdersLocked.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim oDItem As ListViewDataItem = e.Item
            Dim oOrder As Taradel.ExtendedOrderHeaderStatu = DirectCast(oDItem.DataItem, Taradel.ExtendedOrderHeaderStatu)

            Dim stFileName As String = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/ReceiptPDFViewer.aspx?guid=" + oOrder.OrderGUID + "&id=" + oOrder.OrderID.ToString()
            Dim hypReceipt As HyperLink = DirectCast(e.Item.FindControl("hypReceipt"), HyperLink)
            hypReceipt.NavigateUrl = stFileName

            Dim DistID As String = TaradelReceiptUtility.RetrieveDistID(oOrder.OrderID).ToString()
            Dim hypMapName = DirectCast(e.Item.FindControl("hypMapName"), HyperLink)
            Dim mapName As String = TaradelReceiptUtility.RetrieveMapName(oOrder.OrderID)
            hypMapName.Text = mapName
            hypMapName.NavigateUrl = "~/Step1-Target.aspx?distid=" & DistID


            Dim EDDMPrinted As Integer = TaradelReceiptUtility.RetrieveEDDMPrintedQuantity(oOrder.OrderID)
            Dim litEDDMPrinted As Literal = DirectCast(e.Item.FindControl("PrintQty"), Literal)
            If litEDDMPrinted IsNot Nothing Then
                litEDDMPrinted.Text = EDDMPrinted.ToString("N0")
            End If



            Dim litLockExpires As Literal = DirectCast(e.Item.FindControl("litLockExpires"), Literal)
            If litLockExpires IsNot Nothing Then
                litLockExpires.Text = ExclusiveUtility.RetrieveLockDate(oOrder.OrderID)
            End If

            Dim dMarkup As Decimal = 0
            If oOrder.WLMarkup.HasValue Then
                dMarkup = oOrder.WLMarkup.Value
            End If

            Dim dOrderAmount As Double = oOrder.OrderAmt.Value + dMarkup
            Dim OrderAmount As Literal = DirectCast(e.Item.FindControl("OrderAmount"), Literal)
            If OrderAmount IsNot Nothing Then

                OrderAmount.Text = dOrderAmount.ToString("C")
            End If

            Dim dPaid As Double = 0
            If oOrder.PaidAmt.HasValue Then
                dPaid = oOrder.PaidAmt.Value
            End If


            Dim dBalance As Double = dOrderAmount - dPaid
            Dim OrderBalance As Literal = DirectCast(e.Item.FindControl("OrderBalance"), Literal)
            If OrderBalance IsNot Nothing Then
                OrderBalance.Text = dBalance.ToString("C")
            End If

        End If
    End Sub



    Private Function GetPONumber(OrderID As Integer) As String

        Dim results As String = ""
        Dim selectSQL As String = "SELECT [PaymentRef] FROM [pnd_OrderPayment] WHERE OrderID = " & OrderID & " AND [PaymentType] LIKE 'Purchase Order'"
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim errorMsg As New StringBuilder()
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSQL, connObj)

        Try

            connObj.Open()
            results = CStr(sqlCommand.ExecuteScalar)

            If String.IsNullOrEmpty(results) Then
                results = "unknown"
            End If

        Catch objException As Exception

            errorMsg.Append("The following errors occurred:<br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " & objException.Message & "</li>")
            errorMsg.Append("<li>Source: " & objException.Source & "<li>")
            errorMsg.Append("<li>Stack Trace: " & objException.StackTrace & "<li>")
            errorMsg.Append("<li>Target Site: " & objException.TargetSite.Name & "<li>")
            errorMsg.Append("<li>SQL: " & selectSQL & "<li>")
            errorMsg.Append("</ul>")

            lvOrders.Visible = False
            pnlError.Visible = True
            litErrorMessage.Text = "Oops.  Something went wrong.  Our IT Staff has been notified. Sorry for any inconvenience. "
            EmailUtility.SendAdminEmail("Error in Accounts_Orders.aspx. GetPONumber() function. <br /><br />Details: " & errorMsg.ToString())

        Finally
            connObj.Close()
        End Try


        Return results


    End Function



End Class
