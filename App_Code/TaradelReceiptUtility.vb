Imports log4net
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml
Imports System.Globalization
Imports System.Reflection
Imports System.IO
Imports WebSupergoo.ABCpdf8
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Configuration
Imports System
Imports System.Xml.Linq

Public Class TaradelReceiptUtility

    Private Shared Log As ILog = LogManager.GetLogger(Reflection.MethodBase.GetCurrentMethod().DeclaringType())

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="orderid"></param>
    ''' <returns>1. ProductID
    '''          2. Quantity 
    ''' </returns>
    ''' <remarks></remarks>
    ''' 

    Public Shared Function RetrieveOrderParameters(orderid As Int32) As List(Of String)
        Dim returnThis As New List(Of String)

        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("appx").ConnectionString)
                oConnA.Open()

                'TODO: turn into a stored procedure 
                Dim sSql As String = "SELECT pnd.ProductID, Quantity, Markup, MarkupType from pnd_OrderItem pnd inner join wl_product wl on pnd.ProductID = wl.BaseProductID  where OrderID = " & orderid & " AND SiteID = 1 and Deleted = 0 "
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.Text
                    Using RDR = oCmdA.ExecuteReader()
                        If RDR.HasRows Then
                            Do While RDR.Read()
                                returnThis.Add(RDR.Item("ProductID").ToString())
                                returnThis.Add(RDR.Item("Quantity").ToString())
                                returnThis.Add(RDR.Item("Markup").ToString())
                                returnThis.Add(RDR.Item("MarkupType").ToString())
                            Loop
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception

        End Try

        Return returnThis
    End Function


    ''' <summary>
    ''' can't find in the log
    ''' </summary>
    ''' <param name="orderid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function LogInfo(test1 As String) As String

        Return ""
    End Function



    Public Shared Function DebugThis(orderid As Int32) As String

        Dim returnThis As String
        returnThis = "test"

        Try
            Dim orderCalc As OrderCalculator = New OrderCalculator()
            orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(orderid), "eddm")

        Catch ex As Exception
            returnThis += ex.ToString()
            returnThis += ex.StackTrace.ToString()
        End Try

        Return returnThis
    End Function



    Public Shared Function CustomMathRound(valueToRound As Decimal) As Decimal
        valueToRound = Math.Round(valueToRound, 2, MidpointRounding.AwayFromZero)
        Return valueToRound
    End Function



    Public Shared Function GetPropertyValue(ByVal obj As Object, ByVal PropName As String) As Object
        Dim objType As Type = obj.GetType()
        Dim pInfo As System.Reflection.PropertyInfo = objType.GetProperty(PropName)
        Dim PropValue As Object = pInfo.GetValue(obj, Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
        Return PropValue
    End Function



    ''' <summary>
    ''' http://stackoverflow.com/questions/1410807/reflection-iterate-objects-properties-recursively-within-my-own-assemblies-v
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>


    Public Shared Function LogOrderCalculatorProducts(lstProducts As List(Of Product)) As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Name")
        dt.Columns.Add("Value")
        dt.AcceptChanges()
        Dim property_value As Object
        For Each p As Product In lstProducts
            Try
                Dim properties_info As PropertyInfo() = GetType(Product).GetProperties()
                For i As Integer = 0 To properties_info.Length - 1
                    With properties_info(i)
                        If .GetIndexParameters().Length = 0 Then
                            property_value = .GetValue(p, Nothing)
                            If property_value Is Nothing Then

                            Else
                                Dim dr As DataRow = dt.NewRow()
                                dr("Name") = .Name.ToString()
                                dr("Value") = property_value.ToString()
                                dt.Rows.Add(dr)
                            End If
                        End If
                    End With
                Next i
            Catch ex As Exception
                Dim dr As DataRow = dt.NewRow()
                dr("Name") = ex.Message
                dr("Value") = ex.StackTrace
                dt.Rows.Add(dr)
            End Try
        Next

        Return dt
    End Function



    Public Shared Function LogOrderCalculator(orderCalc As OrderCalculator) As DataTable

        Dim dt As New DataTable()
        dt.Columns.Add("Name")
        dt.Columns.Add("Value")
        dt.AcceptChanges()

        ' List the properties.
        '' Use the class you want to study instead of Form1.
        Dim property_value As Object

        Try
            Dim properties_info As PropertyInfo() = GetType(OrderCalculator).GetProperties()
            For i As Integer = 0 To properties_info.Length - 1
                With properties_info(i)
                    If .GetIndexParameters().Length = 0 Then
                        property_value = .GetValue(orderCalc, Nothing)
                        If property_value Is Nothing Then

                        Else
                            Dim dr As DataRow = dt.NewRow()
                            dr("Name") = .Name.ToString()
                            dr("Value") = property_value.ToString()
                            dt.Rows.Add(dr)
                        End If
                    End If
                End With
            Next i
        Catch ex As Exception
            Dim dr As DataRow = dt.NewRow()
            dr("Name") = ex.Message
            dr("Value") = ex.StackTrace

        End Try

        dt.AcceptChanges()

        Return dt

    End Function



    Public Shared Function LogObject(obj As Object) As DataTable

        Dim dt As New DataTable()
        dt.Columns.Add("Name")
        dt.Columns.Add("Value")
        dt.AcceptChanges()

        ' List the properties.
        '' Use the class you want to study instead of Form1.
        Dim property_value As Object

        Try
            Dim properties_info As PropertyInfo() = obj.GetType().GetProperties()
            For i As Integer = 0 To properties_info.Length - 1
                With properties_info(i)
                    If .GetIndexParameters().Length = 0 Then
                        property_value = .GetValue(obj, Nothing)
                        If property_value Is Nothing Then

                        Else
                            Dim dr As DataRow = dt.NewRow()
                            dr("Name") = .Name.ToString()
                            dr("Value") = property_value.ToString()
                            dt.Rows.Add(dr)
                        End If
                    End If
                End With
            Next i
        Catch ex As Exception
            Dim dr As DataRow = dt.NewRow()
            dr("Name") = ex.Message
            dr("Value") = ex.StackTrace

        End Try

        dt.AcceptChanges()

        Return dt

    End Function



    Public Shared Function RetrievePostageRate() As Decimal
        'Step 2 - Postage for deduction from taxes
        Dim postageRate As Decimal = 0
        postageRate = appxCMS.Util.AppSettings.GetDecimal("USPSPostageRate") 'Moved to appSettings.config
        If postageRate = 0 Then
            postageRate = 0.154
        End If
        Return postageRate
    End Function



    Public Shared Function HydrateOrderCalcWithDataSet(orderCalc As OrderCalculator, ds As DataSet) As OrderCalculator

        Dim dtProducts As DataTable = ds.Tables(0)
        Dim dtOrderCalc As DataTable = ds.Tables(1)

        'clear the products
        If orderCalc IsNot Nothing Then
            orderCalc.LstProducts.Clear()
        End If


        For Each dr As DataRow In dtProducts.Rows
            Dim p As New Product()
            p.PricePerPiece = OrderCalculator.ConvertToDecimalOrZero(dr("PricePerPiece"))
            p.ProductID = OrderCalculator.ConvertToIntegerOrZero(dr("ProductID"))
            p.ProductName = (dr("ProductName").ToString())
            p.Quantity = OrderCalculator.ConvertToIntegerOrZero(dr("Quantity"))
            p.TotalProductPrice = OrderCalculator.ConvertToDecimalOrZero(dr("TotalProductPrice"))
            orderCalc.LstProducts.Add(p)
        Next


        'Do While RDR.Read()
        'generic order data - should the same where financed or not
        Dim datarow As DataRow = dtOrderCalc.Rows(0)
        Try
            orderCalc.PreTaxRevenue = OrderCalculator.ConvertToDecimalOrZero(datarow("PreTaxRevenue"))
            orderCalc.Nontaxable = OrderCalculator.ConvertToDecimalOrZero(datarow("Nontaxable"))
            orderCalc.PricePerPiece = OrderCalculator.ConvertToDecimalOrZero(datarow("PricePerPiece"))
            orderCalc.DesignFee = OrderCalculator.ConvertToDecimalOrZero(datarow("DesignFee"))
            orderCalc.DropFee = OrderCalculator.ConvertToDecimalOrZero(datarow("DropFeeAmt"))
            orderCalc.NumOfDrops = OrderCalculator.ConvertToIntegerOrZero(datarow("NumOfDrops"))
            orderCalc.MailPieces = OrderCalculator.ConvertToIntegerOrZero(datarow("PiecesToMail"))
            orderCalc.MailPiecesPrice = OrderCalculator.ConvertToDecimalOrZero(datarow("MailingPrice"))
            orderCalc.SalesTaxRate = OrderCalculator.ConvertToDecimalOrZero(datarow("SalesTaxRate"))
            orderCalc.SalesTaxMessage = (datarow("TaxMessage").ToString())
            orderCalc.Postage = OrderCalculator.ConvertToDecimalOrZero(datarow("PostageFeeAmt"))
            orderCalc.DropIntervalAsDays = OrderCalculator.ConvertToIntegerOrZero(datarow("DropDaysInterval"))
            orderCalc.FirstDropDate = DateTime.Parse(datarow("FirstDropDate"))


            'single payment order data
            orderCalc.Subtotal = OrderCalculator.ConvertToDecimalOrZero(datarow("Subtotal"))
            orderCalc.SalesTax = OrderCalculator.ConvertToDecimalOrZero(datarow("SalesTaxAmt"))
            orderCalc.TaxableAmount = OrderCalculator.ConvertToDecimalOrZero(datarow("TaxableAmount"))



            'Finance Data
            orderCalc.FinanceFirstPayment = OrderCalculator.ConvertToDecimalOrZero(datarow("FinanceFirstPayment"))
            orderCalc.FinancePaymentAmount = OrderCalculator.ConvertToDecimalOrZero(datarow("FinancePayAmt"))
            orderCalc.FinanceTotal = OrderCalculator.ConvertToDecimalOrZero(datarow("FinancePayTotal"))
            orderCalc.FinancedSubTotWithFinanceAmt = OrderCalculator.ConvertToDecimalOrZero(datarow("FinancedSubTotWithFinanceAmt"))
            orderCalc.FinancedNonTaxAmtWithFinanceAmt = OrderCalculator.ConvertToDecimalOrZero(datarow("FinancedNonTaxAmtWithFinanceAmt"))
            orderCalc.FinancedTaxableAmt = OrderCalculator.ConvertToDecimalOrZero(datarow("FinancedTaxableAmt"))
            orderCalc.FinanceFee = OrderCalculator.ConvertToDecimalOrZero(datarow("FinanceFeeAmt"))
            orderCalc.FinanceCustomerOwes = OrderCalculator.ConvertToDecimalOrZero(datarow("FinanceCustomerOwes"))


            'data blank if calculated, data appears if order
            orderCalc.WLMarkup = OrderCalculator.ConvertToDecimalOrZero(datarow("WLMarkup"))
            orderCalc.TaradelOrderAmt = OrderCalculator.ConvertToDecimalOrZero(datarow("TaradelOrderAmt"))
            orderCalc.CustomerOrderAmt = OrderCalculator.ConvertToDecimalOrZero(datarow("CustomerOrderAmt"))
            orderCalc.CustomerHasPaid = OrderCalculator.ConvertToDecimalOrZero(datarow("CustomerHasPaid"))
            orderCalc.ShippingFee = OrderCalculator.ConvertToDecimalOrZero(datarow("ShippingFee"))
            'orderCalc.ProductID()
            orderCalc.ReceiptNumber = OrderCalculator.ConvertToIntegerOrZero(datarow("ReceiptID"))
            orderCalc.ReceiptDate = OrderCalculator.ConvertToStringOrDate(datarow("ReceiptDate"))
            'orderCalc.PurchaseOrderDate = OrderCalculator.ConvertToStringOrDate(RDR.Item("PurchaseOrderDate"))
            'orderCalc.PurchaseOrderNumber = OrderCalculator.ConvertToIntegerOrZero(RDR.Item("PurchaseOrderID"))


            orderCalc.CouponDiscount = OrderCalculator.ConvertToDecimalOrZero(datarow("CouponDiscount"))
            orderCalc.CouponCode = datarow("CouponCode").ToString()

            '---------------------Extra Pieces Data------------------------------
            orderCalc.ExtraPieces = OrderCalculator.ConvertToIntegerOrZero(datarow("ExtraPieces"))
            orderCalc.ExtraPiecesAddress1 = (datarow("ExtraPcsAddress1")).ToString()
            orderCalc.ExtraPiecesAddress2 = (datarow("ExtraPcsAddress2")).ToString()
            orderCalc.ExtraPiecesContact = (datarow("ExtraPcsContact")).ToString()
            orderCalc.ExtraPiecesCompany = (datarow("ExtraPcsCompany")).ToString()
            orderCalc.ExtraPiecesAddressCityStateZip = (datarow("ExtraPcsCityStateZip")).ToString()
            orderCalc.ExtraPiecesPrice = OrderCalculator.ConvertToDecimalOrZero(datarow("ExtraPcsPrice"))
            orderCalc.ExtraPiecesPricePerPiece = OrderCalculator.ConvertToDecimalOrZero(datarow("ExtraPcsPPP"))

            orderCalc.BasePrice = OrderCalculator.ConvertToDecimalOrZero(datarow("BasePrice"))
            orderCalc.OrderTotal = OrderCalculator.ConvertToDecimalOrZero(datarow("OrderTotal"))
            orderCalc.CustomerID = OrderCalculator.ConvertToIntegerOrZero(datarow("CustomerID"))



        Catch ex As Exception
            orderCalc.zErrorMessage = ex.Message.ToString() + vbCrLf + ex.StackTrace.ToString()
        End Try
        '


        'Loop

        Return orderCalc

    End Function



    Public Shared Function HydrateOrderCalcWithReader(orderCalc As OrderCalculator, RDR As SqlDataReader) As OrderCalculator
        Do While RDR.Read()
            'generic order data - should the same where financed or not
            Try
                orderCalc.PreTaxRevenue = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("PreTaxRevenue"))
                orderCalc.Nontaxable = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("Nontaxable"))
                orderCalc.PricePerPiece = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("PricePerPiece"))
                orderCalc.DesignFee = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("DesignFee"))
                orderCalc.DropFee = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("DropFeeAmt"))
                orderCalc.NumOfDrops = OrderCalculator.ConvertToIntegerOrZero(RDR.Item("NumOfDrops"))
                orderCalc.MailPieces = OrderCalculator.ConvertToIntegerOrZero(RDR.Item("PiecesToMail"))
                orderCalc.MailPiecesPrice = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("MailingPrice"))
                orderCalc.SalesTaxRate = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("SalesTaxRate"))
                orderCalc.SalesTaxMessage = (RDR.Item("TaxMessage").ToString())
                orderCalc.Postage = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("PostageFeeAmt"))
                orderCalc.DropIntervalAsDays = OrderCalculator.ConvertToIntegerOrZero(RDR.Item("DropDaysInterval"))
                orderCalc.FirstDropDate = DateTime.Parse(RDR.Item("FirstDropDate"))


                'single payment order data
                orderCalc.Subtotal = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("Subtotal"))
                orderCalc.SalesTax = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("SalesTaxAmt"))
                orderCalc.TaxableAmount = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("TaxableAmount"))



                'Finance Data
                orderCalc.FinanceFirstPayment = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("FinanceFirstPayment"))
                orderCalc.FinancePaymentAmount = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("FinancePayAmt"))
                orderCalc.FinanceTotal = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("FinancePayTotal"))
                orderCalc.FinancedSubTotWithFinanceAmt = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("FinancedSubTotWithFinanceAmt"))
                orderCalc.FinancedNonTaxAmtWithFinanceAmt = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("FinancedNonTaxAmtWithFinanceAmt"))
                orderCalc.FinancedTaxableAmt = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("FinancedTaxableAmt"))
                orderCalc.FinanceFee = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("FinanceFeeAmt"))
                orderCalc.FinanceCustomerOwes = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("FinanceCustomerOwes"))


                'data blank if calculated, data appears if order
                orderCalc.WLMarkup = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("WLMarkup"))
                orderCalc.TaradelOrderAmt = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("TaradelOrderAmt"))
                orderCalc.CustomerOrderAmt = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("CustomerOrderAmt"))
                orderCalc.CustomerHasPaid = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("CustomerHasPaid"))
                orderCalc.ShippingFee = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("ShippingFee"))
                'orderCalc.ProductID()
                orderCalc.ReceiptNumber = OrderCalculator.ConvertToIntegerOrZero(RDR.Item("ReceiptID"))
                orderCalc.ReceiptDate = OrderCalculator.ConvertToStringOrDate(RDR.Item("ReceiptDate"))
                'orderCalc.PurchaseOrderDate = OrderCalculator.ConvertToStringOrDate(RDR.Item("PurchaseOrderDate"))
                'orderCalc.PurchaseOrderNumber = OrderCalculator.ConvertToIntegerOrZero(RDR.Item("PurchaseOrderID"))


                orderCalc.CouponDiscount = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("CouponDiscount"))
                orderCalc.CouponCode = RDR.Item("CouponCode").ToString()

                '---------------------Extra Pieces Data------------------------------
                orderCalc.ExtraPieces = OrderCalculator.ConvertToIntegerOrZero(RDR.Item("ExtraPieces"))
                orderCalc.ExtraPiecesAddress1 = (RDR.Item("ExtraPcsAddress1")).ToString()
                orderCalc.ExtraPiecesAddress2 = (RDR.Item("ExtraPcsAddress2")).ToString()
                orderCalc.ExtraPiecesContact = (RDR.Item("ExtraPcsContact")).ToString()
                orderCalc.ExtraPiecesCompany = (RDR.Item("ExtraPcsCompany")).ToString()
                orderCalc.ExtraPiecesAddressCityStateZip = (RDR.Item("ExtraPcsCityStateZip")).ToString()
                orderCalc.ExtraPiecesPrice = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("ExtraPcsPrice"))
                orderCalc.ExtraPiecesPricePerPiece = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("ExtraPcsPPP"))

                orderCalc.BasePrice = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("BasePrice"))
                orderCalc.OrderTotal = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("OrderTotal"))
                orderCalc.CustomerID = OrderCalculator.ConvertToIntegerOrZero(RDR.Item("CustomerID"))

            Catch ex As Exception
                orderCalc.zErrorMessage = ex.Message.ToString() + System.Environment.NewLine + ex.StackTrace.ToString()
            End Try
            '


        Loop

        orderCalc.zErrorMessage = "HydrateOrderCalcWithReader"

        Return orderCalc

    End Function



    Public Shared Function GenerateDataTableFromProductListForSqlParam(lstProduct As List(Of Product)) As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("ProductID", GetType(Integer))
        dt.Columns.Add("Quantity", GetType(Integer))
        dt.Columns.Add("PricePerPiece", GetType(Decimal))
        dt.Columns.Add("TotalProductPrice", GetType(Decimal))
        dt.Columns.Add("ProductName", GetType(String))
        dt.AcceptChanges()

        For Each p As Product In lstProduct
            Dim dr As DataRow = dt.NewRow()
            dr("ProductID") = p.ProductID
            dr("Quantity") = p.Quantity
            dr("PricePerPiece") = p.PricePerPiece
            dr("ProductName") = p.ProductName
            dr("TotalProductPrice") = p.TotalProductPrice
            dt.Rows.Add(dr)
        Next

        dt.AcceptChanges()

        Return dt

    End Function



    Public Shared Function CalculateOrder2(orderCalc As OrderCalculator) As OrderCalculator

        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)

                oConnA.Open()

                Dim sSql As String = "usp_CalculateOrderDetailsv6"

                Using oCmdA As New SqlCommand(sSql, oConnA)

                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@paramPiecesToMail", orderCalc.MailPieces)
                    oCmdA.Parameters.AddWithValue("@paramPiecesToShip", orderCalc.ExtraPieces)
                    oCmdA.Parameters.AddWithValue("@paramPricePerPiece", orderCalc.PricePerPiece)
                    oCmdA.Parameters.AddWithValue("@paramCouponDiscount", orderCalc.CouponDiscount)
                    oCmdA.Parameters.AddWithValue("@paramCouponCode", orderCalc.CouponCode)
                    oCmdA.Parameters.AddWithValue("@paramDesignFee", orderCalc.DesignFee)
                    oCmdA.Parameters.AddWithValue("@paramNumberOfDrops", orderCalc.NumOfDrops)
                    oCmdA.Parameters.AddWithValue("@paramCustomerID", orderCalc.CustomerID)
                    oCmdA.Parameters.AddWithValue("@paramCustomerState", orderCalc.SalesTaxState)
                    oCmdA.Parameters.AddWithValue("@paramShipAddressID", orderCalc.ShipAddressID)
                    oCmdA.Parameters.AddWithValue("@paramSiteID", orderCalc.SiteID)
                    oCmdA.Parameters.AddWithValue("@paramBasePricePerPiece", orderCalc.BasePricePerPiece)
                    oCmdA.Parameters.AddWithValue("@paramIsThisAMultiple", orderCalc.IsThisAMultiple)

                    Dim tvpParam As SqlParameter = oCmdA.Parameters.AddWithValue("@paramProductTable", GenerateDataTableFromProductListForSqlParam(orderCalc.LstProducts))
                    tvpParam.SqlDbType = SqlDbType.Structured
                    tvpParam.TypeName = "dbo.OrderCalcProductTableType"

                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(oCmdA)
                    Dim ds As DataSet = New DataSet()
                    adapter.Fill(ds)

                    orderCalc = HydrateOrderCalcWithDataSet(orderCalc, ds)
                    orderCalc.zErrorMessage = "CalculateOrder2 " & orderCalc.CouponDiscount.ToString()


                End Using
            End Using

        Catch ex As Exception
            orderCalc.zErrorMessage = ex.StackTrace.ToString() & vbCrLf & ex.Message.ToString()
            Log.Error("RetrieveOrderCalculator:" & ex.Message.ToString(), ex)
        End Try

        Return orderCalc


    End Function


    'deprecated 2/1/2016
    'Public Shared Function CalculateOrder(orderCalc As OrderCalculator) As OrderCalculator
    '    Try
    '        Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("appx").ConnectionString)
    '            oConnA.Open()

    '            Dim sSql As String = "usp_CalculateOrderDetailsv21"
    '            Dim siteId As Integer = Integer.TryParse(Taradel.WLUtil.GetSiteId, 1)  'EDDM SiteID by default.



    '            Using oCmdA As New SqlCommand(sSql, oConnA)
    '                oCmdA.CommandType = CommandType.StoredProcedure
    '                oCmdA.Parameters.AddWithValue("@paramPiecesToMail", orderCalc.MailPieces)
    '                oCmdA.Parameters.AddWithValue("@paramPiecesToShip", orderCalc.ExtraPieces)
    '                oCmdA.Parameters.AddWithValue("@paramPricePerPiece", orderCalc.PricePerPiece)
    '                oCmdA.Parameters.AddWithValue("@paramCouponDiscount", orderCalc.CouponDiscount)
    '                oCmdA.Parameters.AddWithValue("@paramCouponCode", orderCalc.CouponCode)
    '                oCmdA.Parameters.AddWithValue("@paramDesignFee", orderCalc.DesignFee)
    '                oCmdA.Parameters.AddWithValue("@paramNumberOfDrops", orderCalc.NumOfDrops)
    '                oCmdA.Parameters.AddWithValue("@paramCustomerID", orderCalc.CustomerID)
    '                oCmdA.Parameters.AddWithValue("@paramCustomerState", orderCalc.SalesTaxState)
    '                oCmdA.Parameters.AddWithValue("@paramShipAddressID", orderCalc.ShipAddressID)
    '                oCmdA.Parameters.AddWithValue("@paramSiteID", siteId)

    '                Using RDR = oCmdA.ExecuteReader()
    '                    If RDR.HasRows Then
    '                        'Do While RDR.Read()
    '                        orderCalc = HydrateOrderCalcWithReader(orderCalc, RDR)
    '                        'Loop
    '                    End If
    '                End Using
    '            End Using
    '        End Using
    '        'txtDebug.Text = "SUCCESS:" + vbCrLf + orderCalc.SubtotalWithMarkup.ToString()


    '    Catch ex As Exception
    '        orderCalc.zErrorMessage = ex.StackTrace.ToString() & vbCrLf & ex.Message.ToString()
    '        Log.Error("RetrieveOrderCalculator:" & ex.Message.ToString(), ex)
    '        'txtDebug.Text = ex.ToString()
    '    End Try


    '    Return orderCalc

    'End Function



    ''' <summary>
    ''' ONLY PASS IN VALID COUPONS
    ''' TODO: add code to validate coupons
    ''' </summary>
    ''' <param name="mailPieces"></param>
    ''' <param name="extraPieces"></param>
    ''' <param name="pricePerPiece"></param>
    ''' <param name="CouponCode"></param>
    ''' <param name="FinanceFee"></param>
    ''' <param name="DesignFee"></param>
    ''' <param name="numberOfDrops"></param>
    ''' <param name="CustomerID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 


    Public Shared Function RetrieveOrderCalculator(mailPieces As Int32, extraPieces As Int32, pricePerPiece As Decimal, CouponCode As String, FinanceFee As Decimal, DesignFee As Decimal, numberOfDrops As Int32, CustomerID As Int32, ShippingFee As Decimal, bMultipleNoFee As Boolean, ShipContact As String, ShipAddress As String, ShipCity As String, ShipState As String, ShipZip As String, IsMultipleImp As Boolean) As OrderCalculator
        Dim orderCalc As New OrderCalculator()
        orderCalc.Coupon = New Taradel.Coupon()
        orderCalc.CustomerID = CustomerID
        orderCalc.SalesTaxState = ShipState
        orderCalc.IsThisAMultiple = IsMultipleImp

        If (CouponCode <> "") Then
            orderCalc.Coupon = Taradel.WLCouponDataSource.GetCoupon(CouponCode)
            orderCalc.CouponCode = orderCalc.Coupon.CouponCode
            orderCalc.CouponDiscount = orderCalc.Coupon.DiscountAmount 'forcing display of this for debugging
        Else
            orderCalc.Coupon.CouponCode = "n/a (testing)"
            orderCalc.Coupon.DiscountAmount = 0
            orderCalc.CouponDiscount = 0
            orderCalc.Coupon.DiscountPercent = 0
            orderCalc.Coupon.ApplyDiscount = 0
        End If

        orderCalc.PostageRate = RetrievePostageRate()

        'Trim down to 3 decimal places.  No rounding wanted.
        'Update.  Nah.  No rounding.  No truncating.


        Try
            orderCalc.ExtraPiecesPricePerPiece = pricePerPiece - orderCalc.PostageRate
            orderCalc.PricePerPiece = pricePerPiece
            orderCalc.DesignFee = OrderCalculator.ConvertToDecimalOrZero(DesignFee)
            orderCalc.FinanceFee = OrderCalculator.ConvertToDecimalOrZero(FinanceFee)
            orderCalc.ShippingFee = OrderCalculator.ConvertToDecimalOrZero(ShippingFee)
            orderCalc.DiscountToPostage = 0
            orderCalc.DiscountToShipping = 0
            orderCalc.DiscountToSubTotal = 0
            orderCalc.DropFee = 0
            orderCalc.Postage = OrderCalculator.CalculatePostage(orderCalc)
            orderCalc.NumOfDrops = numberOfDrops


            If numberOfDrops > 1 Then
                orderCalc.DropFee = (numberOfDrops - 1) * 99
            End If

            If (IsMultipleImp) Then
                If (bMultipleNoFee) Then
                    orderCalc.DropFee = 0
                End If
            End If


            orderCalc.ExtraPieces = extraPieces
            orderCalc.MailPieces = mailPieces
            orderCalc.ExtraPiecesPrice = CustomMathRound(orderCalc.ExtraPiecesPricePerPiece * orderCalc.ExtraPieces)
            orderCalc.BasePrice = CustomMathRound(orderCalc.MailPieces * orderCalc.PricePerPiece) + CustomMathRound(orderCalc.ExtraPieces * orderCalc.ExtraPiecesPricePerPiece)
            orderCalc.MailPiecesPrice = CustomMathRound(orderCalc.MailPieces * orderCalc.PricePerPiece)
            'Step 3b - Subtotal to be displayed with DesignFee + DropFee + ShippingFee
            orderCalc.Subtotal = OrderCalculator.CalculateBasePriceWithFees(orderCalc)

            'Step 1B Extra Pieces - the addresss
            orderCalc.ExtraPiecesAddress1 = ShipAddress
            orderCalc.ExtraPiecesAddressCityStateZip = ShipCity + ", " + ShipState + " " + ShipZip

            'APPLY DESIGN FEE COUPONS
            If orderCalc.Coupon.ApplyDiscount = 3 Then
                If orderCalc.Coupon.DiscountPercent Then
                    orderCalc.DesignFee = orderCalc.DesignFee - (orderCalc.DesignFee * orderCalc.Coupon.DiscountAmount)
                    orderCalc.CouponMessage = orderCalc.Coupon.CouponCode & " coupon applied " & OrderCalculator.FormatAsCurrency(orderCalc.DesignFee * orderCalc.Coupon.DiscountAmount) & " to design."
                Else
                    If orderCalc.DesignFee > 0 Then
                        If orderCalc.Coupon.DiscountAmount >= orderCalc.DesignFee Then
                            orderCalc.DesignFee = 0
                        Else
                            orderCalc.DesignFee = orderCalc.DesignFee - orderCalc.Coupon.DiscountAmount
                        End If

                        orderCalc.CouponMessage = orderCalc.Coupon.CouponCode & " coupon applied " & OrderCalculator.FormatAsCurrency(orderCalc.Coupon.DiscountAmount) & " to design."
                    End If
                End If

                'recalculate subtotal
                orderCalc.Subtotal = OrderCalculator.CalculateBasePriceWithFees(orderCalc)
            End If
            'end DESIGN Coupons

            'APPLY SHIPPING COUPONS-----
            'Deduct USPS coupon from postage USPS50-2014 USPS100-2014
            'this will also manage shipping coupons
            'Dennis said that percentage WOULD NEVER apply to postage -- NEVER
            If orderCalc.Coupon.ApplyDiscount = 2 Then
                If orderCalc.Coupon.DiscountPercent Then
                    orderCalc.ShippingFee = orderCalc.ShippingFee - CustomMathRound(orderCalc.ShippingFee * orderCalc.Coupon.DiscountAmount)
                    orderCalc.CouponMessage = orderCalc.Coupon.CouponCode & " coupon applied " & OrderCalculator.FormatAsCurrency(orderCalc.ShippingFee * orderCalc.Coupon.DiscountAmount) & " to shipping."
                    orderCalc.DiscountToShipping = CustomMathRound(orderCalc.ShippingFee * orderCalc.Coupon.DiscountAmount)
                    orderCalc.DiscountToPostage = 0
                Else
                    'subtract from shipping THEN postage
                    Dim totalDiscount As Decimal = orderCalc.Coupon.DiscountAmount

                    If orderCalc.ShippingFee > 0 Then
                        If totalDiscount >= orderCalc.ShippingFee Then
                            orderCalc.ShippingFee = 0
                            orderCalc.DiscountToShipping = totalDiscount - orderCalc.ShippingFee
                            totalDiscount = totalDiscount - orderCalc.ShippingFee
                            orderCalc.DiscountToPostage = 0
                        Else
                            orderCalc.ShippingFee = orderCalc.ShippingFee - totalDiscount
                            totalDiscount = 0 'all used up
                        End If

                        orderCalc.CouponMessage = orderCalc.Coupon.CouponCode & " coupon applied " & OrderCalculator.FormatAsCurrency(orderCalc.Coupon.DiscountAmount) & " to shipping."
                    End If

                    If totalDiscount > 0 Then
                        orderCalc.DiscountToPostage = totalDiscount
                        orderCalc.CouponMessage += orderCalc.Coupon.CouponCode & " coupon applied " & OrderCalculator.FormatAsCurrency(orderCalc.DiscountToPostage) & " to postage."
                    End If
                End If

            End If
            'end of applying shipping discounts

            orderCalc.Postage = OrderCalculator.CalculatePostage(orderCalc) ' need refresh since discount to postage should be present now
            orderCalc.Subtotal = OrderCalculator.CalculateBasePriceWithFees(orderCalc)


            'APPLY Order Total (pre-tax) coupon
            If orderCalc.Coupon.ApplyDiscount = 1 Then
                If orderCalc.Coupon.DiscountPercent Then
                    orderCalc.DiscountToSubTotal = CustomMathRound(orderCalc.Subtotal * orderCalc.Coupon.DiscountAmount)
                Else
                    If orderCalc.Coupon.DiscountAmount >= orderCalc.Subtotal Then
                        orderCalc.DiscountToSubTotal = orderCalc.Subtotal
                    Else
                        orderCalc.DiscountToSubTotal = orderCalc.Coupon.DiscountAmount
                    End If
                    orderCalc.CouponMessage = orderCalc.Coupon.CouponCode & " coupon applied " & OrderCalculator.FormatAsCurrency(orderCalc.Coupon.DiscountAmount) & " to order subtotal."
                End If
            End If
            'END APPLY Order Total (pre-tax) coupon

            'Step 4


            orderCalc.Subtotal = OrderCalculator.CalculateSubTotalWithFeesAndDiscounts(orderCalc)
            orderCalc.TaxableAmount = OrderCalculator.CalculateTaxableAmount(orderCalc)

            'calculate taxes
            Dim salestaxcalc As New Taradel.SalesTaxCalculator(orderCalc.TaxableAmount, orderCalc.SalesTaxState)
            salestaxcalc.Calculate()
            orderCalc.SalesTax = CustomMathRound(salestaxcalc.Amount)
            orderCalc.TaxMessage = orderCalc.SalesTaxState & "|" & salestaxcalc.Rate.ToString()
            orderCalc.SalesTaxState = orderCalc.SalesTaxState
            orderCalc.OrderTotal = orderCalc.SalesTax + orderCalc.Subtotal

            'APPLY ENTIRE ORDER COUPON
            If orderCalc.Coupon.ApplyDiscount = 4 Then
                If orderCalc.Coupon.DiscountPercent Then
                    orderCalc.OrderTotal = orderCalc.OrderTotal - (orderCalc.OrderTotal * orderCalc.CouponDiscount)
                Else
                    If orderCalc.Coupon.DiscountAmount >= orderCalc.OrderTotal Then
                        orderCalc.OrderTotal = 0
                    Else
                        orderCalc.OrderTotal = orderCalc.OrderTotal - orderCalc.Coupon.DiscountAmount
                    End If
                End If
                orderCalc.CouponMessage = orderCalc.Coupon.CouponCode & " coupon applied " & OrderCalculator.FormatAsCurrency(orderCalc.Coupon.DiscountAmount) & " to order total."
            End If
            'END APPLY ENTIRE ORDER COUPON





            'Step 5 - the finance fees First Payment & the finance fees Payment
            'Dim orderTotalNoFinanceFee As Decimal = orderCalc.OrderTotal - orderCalc.FinanceFee 'removing the finance fee for the calculation
            orderCalc.FinanceFirstPayment = CustomMathRound(orderCalc.OrderTotal * 0.5)
            'Log.Info("1 financeFirstPayment:" & orderCalc.FinanceFirstPayment.ToString())
            Dim balance As Decimal = orderCalc.OrderTotal - orderCalc.FinanceFirstPayment
            'Log.Info("1 balance:" & balance.ToString())

            orderCalc.FinancePaymentAmount = CustomMathRound(balance / numberOfDrops) + 25
            'Log.Info("1 FinancePaymentAmount:" & orderCalc.FinancePaymentAmount.ToString())


            Dim totalOfPayments As Decimal = CustomMathRound(orderCalc.FinancePaymentAmount * numberOfDrops) + orderCalc.FinanceFirstPayment


            'If totalOfPayments <> orderCalc.OrderTotal Then
            '    Dim difference As Decimal = orderCalc.OrderTotal - totalOfPayments

            '    Dim newFinanceFirstPayment As Decimal = CustomMathRound(orderCalc.FinanceFirstPayment + difference)
            '    Dim newBalance As Decimal = CustomMathRound(orderCalc.OrderTotal - newFinanceFirstPayment)
            '    Dim newFinancePayment As Decimal = CustomMathRound(newBalance / numberOfDrops)

            '    orderCalc.FinancePaymentAmount = newFinancePayment
            '    orderCalc.FinanceFirstPayment = newFinanceFirstPayment
            'End If

            orderCalc.FinanceTotal = (orderCalc.FinancePaymentAmount * orderCalc.NumOfDrops) + orderCalc.FinanceFirstPayment

        Catch ex As Exception
            Log.Error(ex.Message, ex)
            orderCalc.zErrorMessage = ex.ToString()
        End Try



        Return orderCalc
    End Function



    Public Shared Function RetrieveIsThisAMultipleImpression(oCart As XmlDocument) As Boolean
        Dim thisIsAMultiple As Boolean = False
        Dim MailedItems As Int32 = 0
        Dim Multiple As Int32 = 0

        Dim oDrops As XmlNodeList = oCart.SelectNodes("/cart/Product/Drops/Drop")

        For Each XNode As XmlNode In oDrops
            Int32.TryParse(xmlhelp.ReadAttributeValue(XNode, "Multiple"), Multiple)
            If (Multiple > 0) Then
                thisIsAMultiple = True
                Exit For
            End If
        Next

        Return thisIsAMultiple
    End Function



    Public Shared Function RetrieveMailedItemsInOrder(oCart As XmlDocument) As Integer
        Dim MailedItems As Int32 = 0
        Dim AddressID As Int32 = 0

        Dim oShipments2 As XmlNode = oCart.SelectSingleNode("//Product/shipments")
        Dim oShips As XmlNodeList = oShipments2.SelectNodes("shipment")

        For Each XNode As XmlNode In oShips
            Int32.TryParse(xmlhelp.ReadAttributeValue(XNode, "AddressID"), AddressID)
            If (AddressID = 0) Then
                Int32.TryParse(xmlhelp.ReadAttributeValue(XNode, "Qty"), MailedItems)
                Exit For 'exit since we have Qty
            End If
        Next

        Return MailedItems
    End Function



    Public Shared Function RetrievePricePerPiece(oCart As XmlDocument) As Decimal
        Dim PricePerPiece As Decimal = 0
        Dim XNode As XmlNode = oCart.SelectSingleNode("/cart/Product")
        Decimal.TryParse(xmlhelp.ReadAttributeValue(XNode, "PricePerPiece"), PricePerPiece)

        Return PricePerPiece
    End Function



    Public Shared Function RetrieveShippedItemsInOrder(oCart As XmlDocument) As Integer
        Dim ShippedItems As Int32 = 0
        Dim AddressID As Int32 = 0

        Dim oShipments2 As XmlNode = oCart.SelectSingleNode("/cart/Product/shipments")
        Dim oShips As XmlNodeList = oShipments2.SelectNodes("shipment")

        For Each XNode As XmlNode In oShips
            Int32.TryParse(xmlhelp.ReadAttributeValue(XNode, "AddressID"), AddressID)
            If (AddressID <> 0) Then
                Int32.TryParse(xmlhelp.ReadAttributeValue(XNode, "Qty"), ShippedItems)
                Exit For 'exit since we have Qty
            End If
        Next

        Return ShippedItems
    End Function



    Public Shared Function RetrieveShippedIDInOrder(oCart As XmlDocument) As Integer
        Dim AddressID As Int32 = 0

        Dim oShipments2 As XmlNode = oCart.SelectSingleNode("/cart/Product/shipments")
        Dim oShips As XmlNodeList = oShipments2.SelectNodes("shipment")

        For Each XNode As XmlNode In oShips
            Int32.TryParse(xmlhelp.ReadAttributeValue(XNode, "AddressID"), AddressID)
            If (AddressID <> 0) Then
                Exit For 'exit since we have Qty
            End If
        Next

        Return AddressID
    End Function



    Public Shared Function RetrieveOrderCalculator(orderid As Integer, productType As String) As OrderCalculator

        Dim orderCalc As OrderCalculator = New OrderCalculator()
        orderCalc.LstProducts = New List(Of Product)

        'Dim lstParameters As List(Of String) = RetrieveOrderParameters(orderid)
        Try

            'EDDM-Redesign Connection String - TaradelWLConnectionString
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                oConnA.Open()

                Dim sSql As String = "usp_RetrieveOrderPaymentDetails4"
                Using oCmdA As New SqlCommand(sSql, oConnA)

                    Dim paramOrderID As New SqlParameter()
                    paramOrderID.ParameterName = "orderID"
                    paramOrderID.Value = orderid

                    Dim paramProductType As New SqlParameter()
                    paramProductType.ParameterName = "productType"
                    paramProductType.Value = productType

                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.Add(paramOrderID)
                    oCmdA.Parameters.Add(paramProductType)

                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(oCmdA)
                    Dim ds As DataSet = New DataSet()
                    adapter.Fill(ds)

                    orderCalc = HydrateOrderCalcWithDataSet(orderCalc, ds)

                End Using
            End Using
            'txtDebug.Text = "SUCCESS:" + vbCrLf + orderCalc.SubtotalWithMarkup.ToString()
            Log.Info("RetrieveOrderCalculator success")

        Catch ex As Exception
            orderCalc.zErrorMessage = ex.Message & System.Environment.NewLine & ex.StackTrace.ToString()
            Log.Error("RetrieveOrderCalculator:" & ex.StackTrace.ToString(), ex)
            'txtDebug.Text = ex.ToString()
        End Try

        Return orderCalc

    End Function



    Public Shared Function Generate(orderId As Integer, ccOrderId As String, ByRef sFile As String, productType As String) As String
        '=========================================================================================================================
        ' Notes from original process.  Now obsolete. 3/10/2015
        '   The "receipts" folder in App_Data is a **Virtual Folder** and does not actually exist on the DEV Server.
        '   It only exists on the staging server (IIS mapping) so when you go to save and pull from the 'receipts' folder
        '   in code on the DEV, it works fine.  But, when you go to view it via FTP, it's not there.  You have to actually look
        '   on the Staging Server via FTP.
        '
        ' Orginal
        '   Dim sTargetPath As String = HttpContext.Current.Server.MapPath("~/App_Data/receipts/" & dOrderDate.ToString("yyyy") & "/" & dOrderDate.ToString("MM") & "/" & dOrderDate.ToString("dd") & "/")
        '   ORIG Dim sClientBase As String = HttpContext.Current.Server.MapPath("~/App_Data/receipts")
        '   ORIG If Not Directory.Exists(sClientBase) Then
        '   ORIG     Directory.CreateDirectory(sClientBase)
        '   ORIG End If
        '=========================================================================================================================

        Dim sPDFFile As String = ""

        Try
            Dim siteId As Integer = Taradel.WLUtil.GetSiteId
            Dim sAutoGenKey As String = ConfigurationManager.AppSettings("AutoGenKey")
            Dim oOrder As Taradel.OrderHeader = Taradel.OrderDataSource.GetOrder(ccOrderId)
            Dim xOrderCalc As OrderCalculator = RetrieveOrderCalculator(orderId, productType)

            If oOrder IsNot Nothing Then

                Dim docID As Int16 = 0
                Dim orderGUID As String = oOrder.OrderGUID
                Dim xorderID As String = oOrder.OrderID
                Dim theDoc As New Doc()
                Dim dOrderDate As DateTime = oOrder.Created.Value
                Dim stFileName As String = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/ReceiptPDF.aspx?refid=" + orderGUID + "&orderid=" + xorderID

                theDoc.Rect.Inset(25, 25)
                theDoc.Color.String = "255 255 255"

                'Log.Info("TRU:" & stFileName)
                docID = theDoc.AddImageUrl(stFileName)

                System.Net.ServicePointManager.ServerCertificateValidationCallback = Function() True


                While (True)
                    theDoc.FrameRect()
                    If Not theDoc.Chainable(docID) Then
                        Exit While
                    Else
                        theDoc.Page = theDoc.AddPage()
                        docID = theDoc.AddImageToChain(docID)
                    End If
                End While

                Dim i As Int16 = 1
                While i <= theDoc.PageCount
                    theDoc.PageNumber = i
                    theDoc.Flatten()
                    i = i + 1
                End While

                Dim sTargetPath As String = HttpContext.Current.Server.MapPath("~/App_Data/receipts/" & dOrderDate.ToString("yyyy") & "/" & dOrderDate.ToString("MM") & "/" & dOrderDate.ToString("dd") & "/")
                'Dim sTargetPath As String = HttpContext.Current.Server.MapPath("~/App_Data/OrderReceipts/" & dOrderDate.ToString("yyyy") & "/" & dOrderDate.ToString("MM") & "/" & dOrderDate.ToString("dd") & "/" & xOrderCalc.CustomerID.ToString() & "/")

                Dim myLogWriter As New LogWriter()
                myLogWriter.RecordInLog("Receipt PDF and Path: " & sTargetPath)


                If Not Directory.Exists(sTargetPath) Then
                    Directory.CreateDirectory(sTargetPath)
                End If


                Dim oCust As Taradel.Customer = oOrder.Customer
                If oCust Is Nothing Then
                    oCust = Taradel.CustomerDataSource.GetCustomer(xOrderCalc.CustomerID)  'oOrder.CustomerReference.ForeignKey)
                End If


                sPDFFile = Path.Combine(sTargetPath, "Order-" & orderId & "-Receipt.pdf")
                theDoc.Save(sPDFFile)
                theDoc.Clear()

            End If

        Catch ex As Exception
            Log.Error("Receipt error:" & ex.StackTrace.ToString() & "|" & ex.Message.ToString())
        End Try

        Return sPDFFile

    End Function



    Public Class OrderCalculatorV2
        Inherits OrderCalculator

        Private _lstProduct As List(Of Productv2)
        Public Property LstProducts2 As List(Of Productv2)
            Get
                Return _lstProduct
            End Get
            Set(value As List(Of Productv2))
                _lstProduct = value
            End Set
        End Property

    End Class



    Public Class OrderCalculator


        Public Shared Function FormatAsCurrency(value As Decimal) As String
            ' Dim value As Double = 12345.6789
            Dim s As String = String.Empty
            s = value.ToString("C", CultureInfo.CurrentCulture)
            Return s
        End Function


        ''' <summary>
        ''' orderCalc.BasePrice + orderCalc.DesignFee + orderCalc.DropFee + orderCalc.ShippingFee
        ''' </summary>
        ''' <param name="orderCalc"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>


        Public Shared Function CalculateBasePriceWithFees(orderCalc As OrderCalculator) As Decimal
            Return CustomMathRound((orderCalc.MailPieces * orderCalc.PricePerPiece) + (orderCalc.ExtraPieces * orderCalc.ExtraPiecesPricePerPiece) + orderCalc.DesignFee + orderCalc.DropFee + orderCalc.ShippingFee)
        End Function


        Public Shared Function CalculatePostage(orderCalc As OrderCalculator) As Decimal
            Dim dPostage As Decimal = CustomMathRound((orderCalc.MailPieces * orderCalc.PostageRate) - orderCalc.DiscountToPostage)
            If (dPostage < 0) Then
                dPostage = 0
            End If
            Return dPostage
        End Function


        Public Shared Function CalculateSubTotalWithFeesAndDiscounts(orderCalc As OrderCalculator) As Decimal
            Return CustomMathRound((orderCalc.MailPieces * orderCalc.PricePerPiece) + (orderCalc.ExtraPieces * orderCalc.ExtraPiecesPricePerPiece) + orderCalc.DesignFee + orderCalc.DropFee + orderCalc.ShippingFee - (orderCalc.DiscountToShipping + orderCalc.DiscountToPostage + orderCalc.DiscountToSubTotal))
        End Function


        Public Shared Function CalculateTaxableAmount(orderCalc As OrderCalculator) As Decimal
            Return CustomMathRound(orderCalc.Subtotal - orderCalc.Postage)
        End Function


        Public Shared Function FormatAsCurrency3(value As Decimal) As String
            ' Dim value As Double = 12345.6789
            Dim s As String = String.Empty
            s = value.ToString("C3", CultureInfo.CurrentCulture)
            Return s
        End Function


        Public Shared Function FormatAsCurrency4(value As Decimal) As String
            'Will format a decimal value to be displayed as Cents.

            Dim dPPP As Decimal = 0
            Dim returnString As String = ""

            If value < 1 Then

                dPPP = (Math.Truncate(value * 1000) / 10)
                returnString = dPPP.ToString()

                If returnString.EndsWith("00") AndAlso returnString.Contains(".") Then
                    returnString = returnString.Substring(0, returnString.Length - 1)
                ElseIf returnString.EndsWith("0") AndAlso returnString.Contains(".") Then
                    returnString = returnString.Substring(0, returnString.Length - 2)
                End If

                Return returnString & "&cent;"

            Else

                returnString = value.ToString("C")
                Return returnString

            End If


        End Function


        Public Shared Function ConvertToDecimalOrZero(o As Object) As Decimal
            Dim returnThis As Decimal = 0
            If o IsNot Nothing Then
                Decimal.TryParse(o.ToString(), returnThis)
            End If

            Return returnThis
        End Function


        Public Shared Function ConvertToIntegerOrZero(o As Object) As Decimal
            Dim returnThis As Integer = 0
            If o IsNot Nothing Then
                Integer.TryParse(o.ToString(), returnThis)
            End If

            Return returnThis
        End Function


        Public Shared Function ConvertToStringOrDate(o As Object) As String
            Dim returnThis As String = String.Empty

            Try
                If o IsNot Nothing Then
                    Dim receiptDate As New Date()
                    Date.TryParse(o.ToString(), receiptDate)
                    returnThis = receiptDate.ToShortDateString + " " + receiptDate.ToShortTimeString()
                End If
            Catch ex As Exception

            End Try

            Return returnThis
        End Function


        Public Shared Function ConvertToDate(o As Object) As String
            Dim returnThis As Date

            Try
                If o IsNot Nothing Then
                    Dim receiptDate As New Date()
                    Date.TryParse(o.ToString(), returnThis)
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return returnThis
        End Function


        Public Shared Function FormatAsCredit(value As Decimal) As String
            ' Dim value As Double = 12345.6789


            Dim s As String = String.Empty
            s = "(" + value.ToString("C", CultureInfo.CurrentCulture) + ")"
            Return s
        End Function



#Region "Properties"
        Private _lstProduct As List(Of Product)
        Public Property LstProducts As List(Of Product)
            Get
                Return _lstProduct
            End Get
            Set(value As List(Of Product))
                _lstProduct = value
            End Set
        End Property




        ''' <summary>
        ''' a WhiteLabel site charges Markup which IS NOT included in TaradelOrderAmt
        ''' </summary>
        ''' <remarks></remarks>
        Private _taradelOrderAmt As Decimal
        Public Property TaradelOrderAmt() As Decimal
            Get
                Return _taradelOrderAmt
            End Get
            Set(ByVal value As Decimal)
                _taradelOrderAmt = value
            End Set
        End Property

        ''' <summary>
        ''' used for Postage Tax Calculation
        ''' </summary>
        ''' <remarks></remarks>

        Public Property PreTaxRevenue() As Decimal
            Get
                Return _preTaxRevenue
            End Get
            Set(ByVal value As Decimal)
                _preTaxRevenue = value
            End Set
        End Property
        Private _preTaxRevenue As Decimal

        ''' <summary>
        ''' Amount that can NOT be taxed
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Nontaxable() As Decimal
            Get
                Return _nontaxable
            End Get
            Set(ByVal value As Decimal)
                _nontaxable = value
            End Set
        End Property
        Private _nontaxable As Decimal

        ''' <summary>
        ''' INCLUDES markup
        ''' </summary>
        ''' <remarks></remarks>
        Public Property PricePerPiece() As Decimal
            Get
                Return _pricePerPiece
            End Get
            Set(ByVal value As Decimal)
                _pricePerPiece = value
            End Set
        End Property
        Private _pricePerPiece As Decimal

        ''' <summary>
        ''' DOES NOT include MARKUP
        ''' </summary>
        ''' <remarks></remarks>
        Public Property BasePricePerPiece() As Decimal
            Get
                Return _basePricePerPiece
            End Get
            Set(ByVal value As Decimal)
                _basePricePerPiece = value
            End Set
        End Property
        Private _basePricePerPiece As Decimal

        Private _coupon As Taradel.Coupon
        Public Property Coupon() As Taradel.Coupon
            Get
                Return _coupon
            End Get
            Set(ByVal value As Taradel.Coupon)
                _coupon = value
            End Set
        End Property



        Private _basePrice As Decimal
        ''' <summary>
        ''' (Price of Pieces) * (Price Per Piece)--comment
        ''' </summary>
        ''' <value>(Price of Pieces) * (Price Per Piece)</value>
        ''' <returns>returns (Price of Pieces) * (Price Per Piece)</returns>
        ''' <remarks>from usp_RetrieveOrderPaymentDetails</remarks>
        Public Property BasePrice() As Decimal
            Get
                Return _basePrice
            End Get
            Set(ByVal value As Decimal)
                _basePrice = value
            End Set
        End Property


        ''' <summary>
        ''' by order of Tom NcNally 6/17/14
        ''' OrderTotal ALWAYS includes markup 
        ''' </summary>
        ''' <remarks></remarks>
        Private _customerOrderAmt As Decimal
        Public Property CustomerOrderAmt() As Decimal
            Get
                Return _customerOrderAmt
            End Get
            Set(ByVal value As Decimal)
                _customerOrderAmt = value
            End Set
        End Property


        Private _customerHasPaid As Decimal
        Public Property CustomerHasPaid() As Decimal
            Get
                Return _customerHasPaid
            End Get
            Set(ByVal value As Decimal)
                _customerHasPaid = value
            End Set
        End Property


        Private _financedTaxableAmt As Decimal
        Public Property FinancedTaxableAmt() As Decimal
            Get
                Return _financedTaxableAmt
            End Get
            Set(ByVal value As Decimal)
                _financedTaxableAmt = value
            End Set
        End Property

        Private _financedNonTaxAmtWithFinanceAmt As Decimal
        Public Property FinancedNonTaxAmtWithFinanceAmt() As Decimal
            Get
                Return _financedNonTaxAmtWithFinanceAmt
            End Get
            Set(ByVal value As Decimal)
                _financedNonTaxAmtWithFinanceAmt = value
            End Set
        End Property

        Private _financedSubTotWithFinanceAmt As Decimal
        Public Property FinancedSubTotWithFinanceAmt() As Decimal
            Get
                Return _financedSubTotWithFinanceAmt
            End Get
            Set(ByVal value As Decimal)
                _financedSubTotWithFinanceAmt = value
            End Set
        End Property


        Private _customerOwes As Decimal
        Public Property FinanceCustomerOwes() As Decimal
            Get
                Return _customerOwes
            End Get
            Set(ByVal value As Decimal)
                _customerOwes = value
            End Set
        End Property

        Private _orderTotal As Decimal
        ''' <summary>
        ''' THE ORDER TOTAL ON THE RECEIPT
        ''' </summary>
        ''' <value>THE ORDER TOTAL ON THE RECEIPT</value>
        ''' <returns>THE ORDER TOTAL ON THE RECEIPT</returns>
        ''' <remarks>THE ORDER TOTAL ON THE RECEIPT</remarks>
        Public Property OrderTotal() As Decimal
            Get
                Return _orderTotal
            End Get
            Set(ByVal value As Decimal)
                _orderTotal = value
            End Set
        End Property


        Private _financeFirstPayment As Decimal
        ''' <summary>
        ''' THE INITIAL PAYMENT IN A PAYMENT PLAN
        ''' </summary>
        ''' <value>THE INITIAL PAYMENT IN A PAYMENT PLAN</value>
        ''' <returns>THE INITIAL PAYMENT IN A PAYMENT PLAN</returns>
        ''' <remarks>THE INITIAL PAYMENT IN A PAYMENT PLAN</remarks>
        Public Property FinanceFirstPayment() As Decimal
            Get
                Return _financeFirstPayment
            End Get
            Set(ByVal value As Decimal)
                _financeFirstPayment = value
            End Set
        End Property


        Private _financePaymentAmount As Decimal
        ''' <summary>
        ''' THE PAYMENT IN A PAYMENT PLAN
        ''' </summary>
        ''' <value>THE PAYMENT IN A PAYMENT PLAN</value>
        ''' <returns>THE PAYMENT IN A PAYMENT PLAN</returns>
        ''' <remarks>THE PAYMENT IN A PAYMENT PLAN</remarks>
        Public Property FinancePaymentAmount() As Decimal
            Get
                Return _financePaymentAmount
            End Get
            Set(ByVal value As Decimal)
                _financePaymentAmount = value
            End Set
        End Property

        Private _financeTotal As Decimal
        ''' <summary>
        ''' THE PAYMENT IN A PAYMENT PLAN
        ''' </summary>
        ''' <value>THE PAYMENT IN A PAYMENT PLAN</value>
        ''' <returns>THE PAYMENT IN A PAYMENT PLAN</returns>
        ''' <remarks>THE PAYMENT IN A PAYMENT PLAN</remarks>
        Public Property FinanceTotal() As Decimal
            Get
                Return _financeTotal
            End Get
            Set(ByVal value As Decimal)
                _financeTotal = value
            End Set
        End Property







        Private _subtotal As Decimal
        ''' <summary>
        ''' Subtotal includes mailed and shipped pieces AND All the fees except finance fees
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Subtotal() As Decimal
            Get
                Return _subtotal
            End Get
            Set(ByVal value As Decimal)
                _subtotal = value
            End Set
        End Property


        Private _postage As Decimal
        Public Property Postage() As Decimal
            Get
                Return _postage
            End Get
            Set(ByVal value As Decimal)
                _postage = value
            End Set
        End Property


        Private _taxableAmount As Decimal
        Public Property TaxableAmount() As Decimal
            Get
                Return _taxableAmount
            End Get
            Set(ByVal value As Decimal)
                _taxableAmount = value
            End Set
        End Property


        Private _couponDiscount As Decimal
        Public Property CouponDiscount() As Decimal
            Get
                Return _couponDiscount
            End Get
            Set(ByVal value As Decimal)
                _couponDiscount = value
            End Set
        End Property


        Private _couponCode As String
        Public Property CouponCode() As String
            Get
                Return _couponCode
            End Get
            Set(ByVal value As String)
                _couponCode = value
            End Set
        End Property


        Private _WLMarkup As Decimal
        Public Property WLMarkup() As Decimal
            Get
                Return _WLMarkup
            End Get
            Set(ByVal value As Decimal)
                _WLMarkup = value
            End Set
        End Property


        Private _paidInFull As Integer
        Public Property PaidInFull() As Integer
            Get
                Return _paidInFull
            End Get
            Set(ByVal value As Integer)
                _paidInFull = value
            End Set
        End Property


        Private _dropIntervalAsDays As Integer
        Public Property DropIntervalAsDays() As Integer
            Get
                Return _dropIntervalAsDays
            End Get
            Set(ByVal value As Integer)
                _dropIntervalAsDays = value
            End Set
        End Property

        ''' <summary>
        ''' /Product/Attribute[@Name="OrderCalc"]/@SalesTax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SalesTax() As Decimal
            Get
                Return _salesTax
            End Get
            Set(ByVal value As Decimal)
                _salesTax = value
            End Set
        End Property
        Private _salesTax As Decimal

        Private _salesTaxRate As Decimal
        Public Property SalesTaxRate() As Decimal
            Get
                Return _salesTaxRate
            End Get
            Set(ByVal value As Decimal)
                _salesTaxRate = value
            End Set
        End Property

        Private _salesTaxMessage As String
        Public Property SalesTaxMessage() As String
            Get
                Return _salesTaxMessage
            End Get
            Set(ByVal value As String)
                _salesTaxMessage = value
            End Set
        End Property

        Private _designFee As Decimal
        Public Property DesignFee() As Decimal
            Get
                Return _designFee
            End Get
            Set(ByVal value As Decimal)
                _designFee = value
            End Set
        End Property


        Private _financeFee As Decimal
        Public Property FinanceFee() As Decimal
            Get
                Return _financeFee
            End Get
            Set(ByVal value As Decimal)
                _financeFee = value
            End Set
        End Property


        Private _dropFee As Decimal
        Public Property DropFee() As Decimal
            Get
                Return _dropFee
            End Get
            Set(ByVal value As Decimal)
                _dropFee = value
            End Set
        End Property


        Private _shippingFee As Decimal
        Public Property ShippingFee() As Decimal
            Get
                Return _shippingFee
            End Get
            Set(ByVal value As Decimal)
                _shippingFee = value
            End Set
        End Property


        Private _receiptNumber As Integer
        Public Property ReceiptNumber() As Integer
            Get
                Return _receiptNumber
            End Get
            Set(ByVal value As Integer)
                _receiptNumber = value
            End Set
        End Property

        Private _SiteID As Integer
        Public Property SiteID() As Integer
            Get
                Return _SiteID
            End Get
            Set(ByVal value As Integer)
                _SiteID = value
            End Set
        End Property

        Private _receiptDate As Date
        Public Property ReceiptDate() As Date
            Get
                Return _receiptDate
            End Get
            Set(ByVal value As Date)
                _receiptDate = value
            End Set
        End Property

        Private _firstDropDate As Date
        Public Property FirstDropDate() As Date
            Get
                Return _firstDropDate
            End Get
            Set(ByVal value As Date)
                _firstDropDate = value
            End Set
        End Property


        Private _purchaseOrderNumber As Integer
        Public Property PurchaseOrderNumber() As Integer
            Get
                Return _purchaseOrderNumber
            End Get
            Set(ByVal value As Integer)
                _purchaseOrderNumber = value
            End Set
        End Property


        Private _purchaseOrderDate As Date
        Public Property PurchaseOrderDate() As Date
            Get
                Return _purchaseOrderDate
            End Get
            Set(ByVal value As Date)
                _purchaseOrderDate = value
            End Set
        End Property


        Private _mailPieces As Int32
        Public Property MailPieces() As Int32
            Get
                Return _mailPieces
            End Get
            Set(ByVal value As Int32)
                _mailPieces = value
            End Set
        End Property


        ''' <summary>
        ''' MailPieces * PricePerPiece
        ''' /Product/Attribute[@Name="OrderCalc"]/@MailPiecesPrice
        ''' </summary>
        ''' <remarks></remarks>
        Public Property MailPiecesPrice() As Decimal
            Get
                Return _mailPiecesPrice
            End Get
            Set(ByVal value As Decimal)
                _mailPiecesPrice = value
            End Set
        End Property
        Private _mailPiecesPrice As Decimal

        ''' <summary>
        ''' Mail PricePerPiece - PostageRate
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ExtraPiecesPricePerPiece() As Decimal
            Get
                Return _extraPiecesPricePerPiece
            End Get
            Set(ByVal value As Decimal)
                _extraPiecesPricePerPiece = value
            End Set
        End Property
        Private _extraPiecesPricePerPiece As Decimal

        ''' <summary>
        '''  Pieces Shipped to Customer * ExtraPiecesPricePerPiece
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ExtraPiecesPrice() As Decimal
            Get
                Return _extraPiecesPrice
            End Get
            Set(ByVal value As Decimal)
                _extraPiecesPrice = value
            End Set
        End Property
        Private _extraPiecesPrice As Decimal


        ''' <summary>
        '''  Pieces Shipped to Customer
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ExtraPieces() As Int32
            Get
                Return _extraPieces
            End Get
            Set(ByVal value As Int32)
                _extraPieces = value
            End Set
        End Property
        Private _extraPieces As Int32


        Public Property ExtraPiecesAddress1() As String
            Get
                Return _extraPiecesAddress
            End Get
            Set(ByVal value As String)
                _extraPiecesAddress = value
            End Set
        End Property
        Private _extraPiecesAddress As String


        Private _extraPiecesAddress2 As String
        Public Property ExtraPiecesAddress2() As String
            Get
                Return _extraPiecesAddress2
            End Get
            Set(ByVal value As String)
                _extraPiecesAddress2 = value
            End Set
        End Property


        Private _extraPiecesAddressCityStateZip As String
        Public Property ExtraPiecesAddressCityStateZip() As String
            Get
                Return _extraPiecesAddressCityStateZip
            End Get
            Set(ByVal value As String)
                _extraPiecesAddressCityStateZip = value
            End Set
        End Property


        Private _extraPiecesContact As String
        Public Property ExtraPiecesContact() As String
            Get
                Return _extraPiecesContact
            End Get
            Set(ByVal value As String)
                _extraPiecesContact = value
            End Set
        End Property


        Private _extraPiecesCompany As String
        Public Property ExtraPiecesCompany() As String
            Get
                Return _extraPiecesCompany
            End Get
            Set(ByVal value As String)
                _extraPiecesCompany = value
            End Set
        End Property

        ''' <summary>
        ''' Does not include tax on $####.## postage.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TaxMessage() As String
            Get
                Return _taxMessage
            End Get
            Set(ByVal value As String)
                _taxMessage = value
            End Set
        End Property
        Private _taxMessage As String

        Public Property SalesTaxState() As String
            Get
                Return _taxState
            End Get
            Set(ByVal value As String)
                _taxState = value
            End Set
        End Property
        Private _taxState As String

        Private _couponMessage As String
        Public Property CouponMessage() As String
            Get
                Return _couponMessage
            End Get
            Set(ByVal value As String)
                _couponMessage = value
            End Set
        End Property


        Private _discountToShipping As Decimal
        Public Property DiscountToShipping() As Decimal
            Get
                Return _discountToShipping
            End Get
            Set(ByVal value As Decimal)
                _discountToShipping = value
            End Set
        End Property


        Private _discountToPostage As Decimal
        Public Property DiscountToPostage() As Decimal
            Get
                Return _discountToPostage
            End Get
            Set(ByVal value As Decimal)
                _discountToPostage = value
            End Set
        End Property


        Private _discountToSubtotal As Decimal
        Public Property DiscountToSubTotal() As Decimal
            Get
                Return _discountToSubtotal
            End Get
            Set(ByVal value As Decimal)
                _discountToSubtotal = value
            End Set
        End Property


        Private _postageRate As Decimal
        Public Property PostageRate() As Decimal
            Get
                Return _postageRate
            End Get
            Set(ByVal value As Decimal)
                _postageRate = value
            End Set
        End Property


        Private _errorMessage As String
        Public Property zErrorMessage() As String
            Get
                Return _errorMessage
            End Get
            Set(ByVal value As String)
                _errorMessage = value
            End Set
        End Property

        Private _isThisAMultiple As Boolean
        Public Property IsThisAMultiple() As Boolean
            Get
                Return _isThisAMultiple
            End Get

            Set(ByVal value As Boolean)
                _isThisAMultiple = value
            End Set

        End Property


        Private _numOfDrops As Integer
        Public Property NumOfDrops() As Integer
            Get
                Return _numOfDrops
            End Get

            Set(ByVal value As Integer)
                _numOfDrops = value
            End Set

        End Property

        Private _customerID As Integer
        Public Property CustomerID() As Integer
            Get
                Return _customerID
            End Get

            Set(ByVal value As Integer)
                _customerID = value
            End Set

        End Property

        Private _shipAddressID As Integer
        Public Property ShipAddressID() As Integer
            Get
                Return _shipAddressID
            End Get

            Set(ByVal value As Integer)
                _shipAddressID = value
            End Set

        End Property
#End Region
    End Class



    Public Shared Function AddAllProductsToOrderCalculator(orderCalc As OrderCalculator, oCart As XmlDocument) As OrderCalculator
        Dim oProductList As XmlNodeList = oCart.SelectNodes("//Product")
        Dim numProductsInCart As Integer = 0
        orderCalc.LstProducts = New List(Of TaradelReceiptUtility.Product)

        For Each x As XmlNode In oProductList
            Dim product As New TaradelReceiptUtility.Product
            Decimal.TryParse(x.Attributes("PricePerPiece").Value.ToString(), product.PricePerPiece)
            Decimal.TryParse(x.Attributes("Quantity").Value.ToString(), product.Quantity)
            Decimal.TryParse(x.Attributes("Price").Value.ToString(), product.TotalProductPrice)
            Integer.TryParse(x.Attributes("ProductID").Value.ToString(), product.ProductID)
            product.ProductName = x.Attributes("Name").Value
            orderCalc.LstProducts.Add(product)
        Next

        Return orderCalc
    End Function



    Class Productv2
        Inherits Product

        ''pass as 0 for regular and 1 for ship to Customer
        Private _ShipToCustomer As Int16
        Public Property ShipToCustomer() As String
            Get
                Return _ShipToCustomer
            End Get

            Set(ByVal value As String)
                _ShipToCustomer = value
            End Set

        End Property
    End Class



    Public Class Product
        Private _productName As String
        Public Property ProductName() As String
            Get
                Return _productName
            End Get

            Set(ByVal value As String)
                _productName = value
            End Set

        End Property

        Private _quantity As Integer
        Public Property Quantity() As Integer
            Get
                Return _quantity
            End Get

            Set(ByVal value As Integer)
                _quantity = value
            End Set

        End Property


        Private _pricePerPiece As Decimal
        Public Property PricePerPiece() As Decimal
            Get
                Return _pricePerPiece
            End Get

            Set(ByVal value As Decimal)
                _pricePerPiece = value
            End Set

        End Property


        Private _totalProductPrice As Decimal
        Public Property TotalProductPrice() As Decimal
            Get
                Return _totalProductPrice
            End Get

            Set(ByVal value As Decimal)
                _totalProductPrice = value
            End Set

        End Property

        Private _productID As Integer
        Public Property ProductID() As Integer
            Get
                Return _productID
            End Get

            Set(ByVal value As Integer)
                _productID = value
            End Set

        End Property


    End Class



    Private Shared Function RetrieveMapNameBasedOnDistID(distID As Integer) As String
        'This method accepts the newly created DistributionID from the MapServer (diff server) and looks up the map name
        Dim results As String = ""
        Dim connectString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim selectSQL As String = "SELECT TOP 1 Name FROM pnd_CustomerDistribution where DistributionID = " + distID.ToString() + " AND [Deleted] = 0"
        Dim myConnection As New SqlConnection(connectString)
        Dim mySQLCommand As New SqlCommand()
        mySQLCommand.Connection = myConnection
        mySQLCommand.CommandText = selectSQL

        myConnection.Open()
        results = DirectCast(mySQLCommand.ExecuteScalar(), [String])
        myConnection.Close()
        Return results

    End Function

    ''' <summary>
    ''' make a property of Order Calculator ? ? ! ! 
    ''' </summary>
    ''' <param name="orderid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Shared Function RetrieveMapName(orderid As Int32) As String
        Dim MapName As String
        Dim DistID As Integer = RetrieveDistID(orderid)
        MapName = RetrieveMapNameBasedOnDistID(DistID)
        Return MapName
    End Function



    Public Shared Function RetrieveDistID(orderid As Int32) As Integer
        Dim DistID As Integer = 0
        Dim xmlDoc As New XmlDocument
        xmlDoc = RetrieveCartXml(orderid)
        Dim productNode As XmlNode = xmlDoc.SelectSingleNode("//Product")
        Integer.TryParse(productNode.Attributes("DistributionId").Value.ToString(), DistID)
        Return DistID
    End Function



    Public Shared Function RetrieveEDDMPrintedQuantity(orderid As Int32) As Integer
        Dim EDDMPrinted As Integer = 0
        Dim xmlDoc As New XmlDocument
        xmlDoc = RetrieveCartXml(orderid)
        EDDMPrinted = RetrieveMailedItemsInOrder(xmlDoc)
        Return EDDMPrinted
    End Function



    Public Shared Function RetrieveCartXml(orderid As Int32) As XmlDocument
        Dim DistID As Integer = 0
        Dim xmlDoc As New XmlDocument

        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("appx").ConnectionString)
                oConnA.Open()

                Dim sSql As String = "SELECT TOP 1 OrderItemXml FROM pnd_OrderItem WHERE OrderID = " + orderid.ToString()
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    'Dim paramz As New SqlParameter()
                    'paramz.ParameterName = "orderID"
                    'paramz.Value = orderid
                    oCmdA.CommandType = CommandType.Text
                    'oCmdA.Parameters.Add(paramz)
                    Using RDR As SqlDataReader = oCmdA.ExecuteReader()
                        If RDR.HasRows Then
                            Do While RDR.Read()
                                Dim xmlValue As String = RDR.GetValue(0)
                                xmlDoc.LoadXml(xmlValue)
                            Loop
                        End If
                    End Using
                End Using
            End Using
            'txtDebug.Text = "SUCCESS:" + vbCrLf + orderCalc.SubtotalWithMarkup.ToString()


        Catch ex As Exception
            Log.Error("RetrieveMapName:" & ex.ToString(), ex)
        End Try

        Dim sOrderXML As String = "<cart>" & xmlDoc.OuterXml & "</cart>"
        Dim oOrderXML As New XmlDocument
        oOrderXML.LoadXml(sOrderXML)

        Return oOrderXML


    End Function



    Public Shared Function RetrievePaymentSchedule(orderid As Int32) As List(Of Payment)
        Dim lstPayments As New List(Of Payment)


        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("appx").ConnectionString)
                oConnA.Open()

                Dim sSql As String = "usp_RetrievePaymentSchedule"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    Dim paramz As New SqlParameter()
                    paramz.ParameterName = "orderID"
                    paramz.Value = orderid

                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.Add(paramz)
                    Using RDR = oCmdA.ExecuteReader()
                        If RDR.HasRows Then
                            Do While RDR.Read()
                                Dim p As New Payment
                                p.Payment = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("Payment"))
                                p.Balance = OrderCalculator.ConvertToDecimalOrZero(RDR.Item("Balance"))
                                p.DropDate = RDR.Item("DropDate").ToString()
                                p.BillDate = RDR.Item("BillDate").ToString()
                                lstPayments.Add(p)
                            Loop
                        End If
                    End Using
                End Using
            End Using
            'txtDebug.Text = "SUCCESS:" + vbCrLf + orderCalc.SubtotalWithMarkup.ToString()


        Catch ex As Exception
            Log.Error("RetrievePaymentSchedule:" & ex.ToString(), ex)
        End Try


        Return lstPayments


    End Function



    Public Class PaymentSchedule
        Private _ListPayments As New List(Of Payment)
        Public Property ListPayments() As List(Of Payment)
            Get
                Return _ListPayments
            End Get
            Set(ByVal value As List(Of Payment))
                _ListPayments = value
            End Set
        End Property
    End Class



    Public Class Payment
        Private _billDate As String
        Public Property BillDate() As String
            Get
                Return _billDate
            End Get
            Set(ByVal value As String)
                _billDate = value
            End Set
        End Property

        Private _dropDate As Date
        Public Property DropDate() As Date
            Get
                Return _dropDate
            End Get
            Set(ByVal value As Date)
                _dropDate = value
            End Set
        End Property

        Private _payment As Decimal
        Public Property Payment() As Decimal
            Get
                Return _payment
            End Get
            Set(ByVal value As Decimal)
                _payment = value
            End Set
        End Property


        Private _balance As Decimal
        Public Property Balance() As Decimal
            Get
                Return _balance
            End Get
            Set(ByVal value As Decimal)
                _balance = value
            End Set
        End Property

    End Class


End Class