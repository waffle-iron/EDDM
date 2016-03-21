Imports System.Data
Imports System.Xml
Imports System.IO
Imports System.Linq
Imports System.Collections.Generic
Imports WebSupergoo.ABCpdf7
Imports Taradel.EF
Imports Taradel
Imports System.Xml.Linq
Imports System.Net.Mail
Imports System.Text
Imports System.Data.Sql
Imports System.Data.SqlClient


'=========================================================================================================================
'   NOTES:
'   Will require additional development to include Marketing Services add-ons.
'
'=========================================================================================================================



Partial Class QuotePDF
    Inherits appxCMS.PageBase




#Region "Fields"

    Protected oXML As New XmlDocument
    Protected oCart As XmlNode = Nothing
    Protected oQuote As Taradel.ProductPriceQuote = Nothing
    Protected oBaseQuote As Taradel.ProductPriceQuote = Nothing
    Protected SavedCartObj As New SavedCart
    Protected numEmailBlasts As Integer = 3
    Protected dropFeeRate As Decimal = 99

#End Region


#Region "Properties"""


    Private _AddressedMap As Boolean = False
    Public Property AddressedMap As Boolean
        Get
            Return _AddressedMap
        End Get
        Set(value As Boolean)
            _AddressedMap = value
        End Set

    End Property


    Private _AddressedShipPrice As Decimal = 0
    Public Property AddressedShipPrice As Decimal

        Get
            Return _AddressedShipPrice
        End Get

        Set(value As Decimal)
            _AddressedShipPrice = value
        End Set

    End Property


    Private _BaseProductID As Integer = ProductID
    Private Property BaseProductID() As Integer
        Get
            Return _BaseProductID
        End Get
        Set(ByVal value As Integer)
            _BaseProductID = value
        End Set
    End Property


    Private _DesignFee As Decimal = 0
    Public Property DesignFee As Decimal
        Get
            Return _DesignFee
        End Get
        Set(value As Decimal)
            _DesignFee = value
        End Set

    End Property


    Private _DistributionId As Integer = 0
    Public Property DistributionId As Integer
        Get
            Return _DistributionId
        End Get
        Set(value As Integer)
            _DistributionId = value
        End Set

    End Property


    Private _DropFee As Decimal = 0
    Public Property DropFee As Decimal

        Get
            Return _DropFee
        End Get

        Set(value As Decimal)
            _DropFee = value
        End Set

    End Property


    Private _EDDMMap As Boolean = False
    Public Property EDDMMap As Boolean
        Get
            Return _EDDMMap
        End Get
        Set(value As Boolean)
            _EDDMMap = value
        End Set

    End Property


    Private _EDDMShipPrice As Decimal = 0
    Public Property EDDMShipPrice As Decimal

        Get
            Return _EDDMShipPrice
        End Get

        Set(value As Decimal)
            _EDDMShipPrice = value
        End Set

    End Property


    Private _EnvironmentMode As String
    Private ReadOnly Property EnvironmentMode() As String

        Get

            'SHOULD return as Dev or Prod
            If ConfigurationManager.AppSettings("Environment") IsNot Nothing Then
                _EnvironmentMode = ConfigurationManager.AppSettings("Environment").ToLower()
            Else
                'Fall back value
                _EnvironmentMode = "dev"
            End If

            Return _EnvironmentMode

        End Get

    End Property


    Private _ExtraCopies As Integer = 0
    Public Property ExtraCopies As Integer
        Get
            Return _ExtraCopies
        End Get
        Set(value As Integer)
            _ExtraCopies = value
        End Set

    End Property


    Private _ExtraPcsPricePerPiece As Decimal = 0
    Public Property ExtraPcsPricePerPiece As Decimal

        Get
            Return _ExtraPcsPricePerPiece
        End Get

        Set(value As Decimal)
            _ExtraPcsPricePerPiece = value
        End Set

    End Property


    Private _HasDropFee As Boolean = False
    Public Property HasDropFee As Boolean

        Get
            Return _HasDropFee
        End Get

        Set(value As Boolean)
            _HasDropFee = value
        End Set

    End Property


    Private _IsProfessionalDesign As Boolean = False
    Public Property IsProfessionalDesign As Boolean

        Get
            Return _IsProfessionalDesign
        End Get

        Set(value As Boolean)
            _IsProfessionalDesign = value
        End Set

    End Property


    Private _MultipleImpressionsNoFee As Boolean = False
    Protected Property MultipleImpressionsNoFee() As Boolean

        Get
            If Not (String.IsNullOrEmpty(appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressionsNoFee"))) Then
                Return appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressionsNoFee")
            End If
        End Get

        Set(ByVal value As Boolean)
            _MultipleImpressionsNoFee = value
        End Set

    End Property


    Private _NumOfDrops As Integer = 0
    Public Property NumOfDrops As Integer

        Get
            Return _NumOfDrops
        End Get

        Set(value As Integer)
            _NumOfDrops = value
        End Set

    End Property


    Private _NumOfImpressions As Integer = 0
    Public Property NumOfImpressions As Integer

        Get
            Return _NumOfImpressions
        End Get

        Set(value As Integer)
            _NumOfImpressions = value
        End Set

    End Property


    Private _PostageRate As Decimal = 0
    Public Property PostageRate As Decimal
        Get
            Return _PostageRate
        End Get
        Set(value As Decimal)
            _PostageRate = value
        End Set
    End Property


    Private _PricePerPiece As Decimal = 0
    Public Property PricePerPiece As Decimal

        Get
            Return _PricePerPiece
        End Get

        Set(value As Decimal)
            _PricePerPiece = value
        End Set

    End Property


    Private _ProductID As Integer = ProductID
    Private Property ProductID() As Integer
        Get
            Return _ProductID
        End Get
        Set(ByVal value As Integer)
            _ProductID = value
        End Set
    End Property


    Private _QuoteKey As String
    Private Property QuoteKey() As String
        Get
            Return _QuoteKey
        End Get
        Set(ByVal value As String)
            _QuoteKey = value
        End Set
    End Property


    Private _SalesTax As Decimal = 0
    Public Property SalesTax As Decimal
        Get
            Return _SalesTax
        End Get
        Set(value As Decimal)
            _SalesTax = value
        End Set

    End Property


    Private _ShipToAddress As String = ""
    Public Property ShipToAddress As String

        Get
            Return _ShipToAddress
        End Get

        Set(value As String)
            _ShipToAddress = value
        End Set

    End Property


    Private _SiteID As Integer = 0
    Public Property SiteID As Integer
        Get
            Return _SiteID
        End Get
        Set(value As Integer)
            _SiteID = value
        End Set
    End Property


    Private _TotalSelected As Integer = 0
    Public Property TotalSelected As Integer

        Get
            Return _TotalSelected
        End Get

        Set(value As Integer)
            _TotalSelected = value
        End Set

    End Property


    Private _TMCMap As Boolean = False
    Public Property TMCMap As Boolean
        Get
            Return _TMCMap
        End Get
        Set(value As Boolean)
            _TMCMap = value
        End Set

    End Property


    Private _USelectID As Integer = 0
    Public Property USelectID As Integer
        Get
            Return _USelectID
        End Get
        Set(value As Integer)
            _USelectID = value
        End Set

    End Property


    Private _UserName As String
    Private Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property


    Private _PONumber As String = ""
    Private Property PONumber() As String
        Get
            Return _PONumber
        End Get
        Set(ByVal value As String)
            _PONumber = value
        End Set
    End Property


    Private _StoreNumber As String = ""
    Private Property StoreNumber() As String
        Get
            Return _StoreNumber
        End Get
        Set(ByVal value As String)
            _StoreNumber = value
        End Set
    End Property


    Private _GeneratedAddressedList As Boolean = False
    Public Property GeneratedAddressedList As Boolean
        Get
            Return _GeneratedAddressedList
        End Get
        Set(value As Boolean)
            _GeneratedAddressedList = value
        End Set

    End Property


    Private _UploadedAddressedList As Boolean = False
    Public Property UploadedAddressedList As Boolean
        Get
            Return _UploadedAddressedList
        End Get
        Set(value As Boolean)
            _UploadedAddressedList = value
        End Set

    End Property


    Private _IsMultiple As Boolean = False
    Public Property IsMultiple As Boolean
        Get
            Return _IsMultiple
        End Get
        Set(value As Boolean)
            _IsMultiple = value
        End Set

    End Property


    Private _TestMode As Boolean = False
    Public Property TestMode As Boolean

        Get
            Return _TestMode
        End Get

        Set(value As Boolean)
            _TestMode = value
        End Set

    End Property


#End Region




    'Init Methods
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        'Missing 
        If (Request.QueryString("key") Is Nothing) Then
            phQuote.Visible = False
            pnlError.Visible = True
            litErrorMessage.Text = "Sorry but we were unable to locate this Quote."
            Exit Sub
        End If


        'Helpers
        SetPageProperties()
        ShowHidePageElements()


        If (TestMode) Then
            ShowDebug()
        Else
            If Not (String.IsNullOrEmpty(Request.QueryString("debug"))) Then
                If Request.QueryString("debug") = "hodor" Then
                    ShowDebug()
                End If
            End If
        End If



        If EDDMMap Then
            BindEDDMCart()
        End If

        If (UploadedAddressedList) Or (GeneratedAddressedList) Then
            BindAddressedCart()
        End If


    End Sub



    Protected Sub SetPageProperties()


        'SiteID
        siteID = appxCMS.Util.CMSSettings.GetSiteId

        'Quote/Saved Cart key
        QuoteKey = Request.QueryString("key")


        'If a PO Number was provided.... 
        If Not (Request.QueryString("po") Is Nothing) Then
            PONumber = Request.QueryString("po").ToString()
        Else
            PONumber = "(not provided)"
        End If


        'If a Store Number was provided.... 
        If Not (Request.QueryString("store") Is Nothing) Then
            StoreNumber = Request.QueryString("store").ToString()
        Else
            StoreNumber = "(not provided)"
        End If



        'Created SavedCartObj to use throughout.
        BuildSavedCartObj()



        'SIMULATING Profile.Cart.
        'Reading from database. The WebsuperGoo ABCPdf cannot access the Profile.Cart data.
        Dim cartData As String = SavedCartObj.Cart

        'Fill the XmlDoc
        oXML.LoadXml(cartData)

        'Define the traditional 'cart'. 'Build the XmlCart Doc
        oCart = oXML.SelectSingleNode("/cart")
        'END of cart simulation



        'UerName
        UserName = SavedCartObj.CreatedBy


        'DistributionID
        DistributionId = CartUtility.GetDistributionID(oCart)


        'Figure out what TYPE of product we have.
        Dim firstProd As XmlNode = oCart.SelectSingleNode("//Product")
        Dim productType As String = xmlhelp.ReadAttribute(firstProd, "Type")


        'Determine the USelectID. This will help tell the whole page how to behave.
        Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistributions.GetDistribution(DistributionId)
        USelectID = oDist.USelectMethodReference.ForeignKey()


        'Determine the DistributionType and set flags
        Dim productNodeFilter As String = ""
        Select Case USelectID
            Case 1
                EDDMMap = True
                productNodeFilter = "EDDM"
            Case 5
                UploadedAddressedList = True
                productNodeFilter = "AddressedList"
            Case 6
                GeneratedAddressedList = True
                productNodeFilter = "AddressedList"
        End Select


        'Now get the PROPER ProductID and BaseProductID
        Dim oProd As XmlNode
        oProd = oCart.SelectSingleNode("//Product[@Type='" & productNodeFilter & "']")


        'Set ProductID and BaseProductID
        ProductID = Integer.Parse(xmlhelp.ReadAttribute(oProd, "ProductID"))
        BaseProductID = Integer.Parse(xmlhelp.ReadAttribute(oProd, "BaseProductID"))


        'Number of Drops
        Dim oNumDrops As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Number of Drops']")
        If oNumDrops IsNot Nothing Then
            NumOfDrops = Integer.Parse(xmlhelp.ReadAttribute(oNumDrops, "Value"))
        Else
            'Count the number of drops and add this in
            Dim oTotalDrops As XmlNodeList = oProd.SelectNodes("//Drop")
            NumOfDrops = oTotalDrops.Count
        End If



        'Set Postage Rate. This needs to be improved. 12/22/2015. DSF
        If (EDDMMap) Then
            PostageRate = appxCMS.Util.AppSettings.GetDecimal("USPSPostageRate")

            If PostageRate = 0 Then
                PostageRate = 0.16
            End If
        Else
            PostageRate = 0.3
        End If


        'Price Per Piece
        PricePerPiece = CartUtility.GetPricePerPiece(oCart, productType)


        'Extra Copies
        ExtraCopies = CartUtility.GetExtraPiecesQty(oCart, productType)


        'Extra Copies Price Per Piece
        ExtraPcsPricePerPiece = (PricePerPiece - PostageRate)


        'Professional Design Fee
        If (CartUtility.GetDesignFee(oCart, productType) > 0) Then
            IsProfessionalDesign = True
            DesignFee = CartUtility.GetDesignFee(oCart, productType)
        End If


        'Ship To Address
        ShipToAddress = CartUtility.GetShipToAddress(oCart, productType)



        'Drop Fees. Normally Step3-Checkout uses the OrderCalculator to calculate and set this property. Here, we have to emulate it and recalculate it.
        'It is difficult to determine the difference between multiple DROPS and multiple IMPRESSIONS - the order calculator simply stores the calculated fee in the 
        '"DropFee" attribute. 
        If (EDDMMap) Then
            NumOfImpressions = CartUtility.GetNumberOfImpressions(oCart, "EDDM")
            NumOfDrops = CartUtility.GetNumberOfDrops(oCart, "EDDM")
        Else
            NumOfImpressions = CartUtility.GetNumberOfImpressions(oCart, "AddressedList")
            NumOfDrops = CartUtility.GetNumberOfDrops(oCart, "AddressedList")
        End If



        If NumOfImpressions > 1 Then

            IsMultiple = True

            If Not MultipleImpressionsNoFee Then
                HasDropFee = True
                DropFee = ((NumOfImpressions - 1) * dropFeeRate)
            End If

        'Single Impression
        Else

            IsMultiple = False

            If NumOfDrops > 1 Then
                HasDropFee = True
                DropFee = ((NumOfDrops - 1) * dropFeeRate)
            End If

        End If




        'Total Selected
        TotalSelected = CartUtility.GetTotalSelected(oCart, productNodeFilter, NumOfImpressions)


        'Sales Tax
        'Set to 0 for now...
        SalesTax = 0


        'Build SiteDetails Obj
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)
        TestMode = SiteDetails.TestMode



    End Sub





    'Events
    Protected Sub rptEddmDrops_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptEddmDrops.ItemDataBound

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            Dim rptEddmRoutes As Repeater = DirectCast(e.Item.FindControl("rptEddmRoutes"), Repeater)
            Dim DropNumber As Integer = Integer.Parse(DataBinder.Eval(e.Item.DataItem, "Number"))
            Dim oDropOrg As XmlNode = oCart.SelectSingleNode("//Drop[@Number=" & DropNumber & "]")

            'Routes only exist in EDDM Distributions
            If EDDMMap Then
                If oDropOrg IsNot Nothing Then
                    Dim oDrop As XmlNode = oDropOrg.CloneNode(True)
                    oDrop.Attributes.RemoveAll()
                    Using oSr As New StringReader(oDrop.OuterXml)
                        Using oDs As New DataSet
                            oDs.ReadXml(oSr)
                            rptEddmRoutes.DataSource = oDs
                            rptEddmRoutes.DataBind()
                        End Using
                    End Using
                End If
            End If


            Dim ddlDropDate As DropDownList = DirectCast(e.Item.FindControl("ddlDropDate"), DropDownList)
            Dim drpDate As Date = Date.Parse(DataBinder.Eval(e.Item.DataItem, "Date"))

            If ddlDropDate IsNot Nothing Then
                Dim i As Int16 = 1
                While i < 6
                    Dim listItem As New ListItem()
                    listItem.Text = drpDate.AddDays(i * 7)
                    listItem.Value = drpDate.AddDays(i * 7)
                    ddlDropDate.Items.Add(listItem)
                    i = i + 1
                End While
                ddlDropDate.DataBind()
            Else
                pnlError.Visible = True
                litErrorMessage.Text = "Drop Date is nothing. (rDrops_ItemDataBound)"
            End If



        End If
    End Sub



    Protected Sub rptAddressedDrops_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptAddressedDrops.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            Dim rAddressedRoutes As Repeater = DirectCast(e.Item.FindControl("rAddressedRoutes"), Repeater)
            Dim DropNumber As Integer = Integer.Parse(DataBinder.Eval(e.Item.DataItem, "Number"))

            'Dim oAddressedDropsOrg As XmlNode = Profile.Cart.SelectSingleNode("//AddressedDrops[@Type='Addressed']/Drop[@Number=" & DropNumber & "]")
            Dim oAddressedDropsOrg As XmlNode = oCart.SelectSingleNode("//AddressedDrops[@Type='Addressed']/Drop[@Number=" & DropNumber & "]")

            If oAddressedDropsOrg IsNot Nothing Then
                Dim oDrop As XmlNode = oAddressedDropsOrg.CloneNode(True)
                oDrop.Attributes.RemoveAll()
                Using oSr As New StringReader(oDrop.OuterXml)
                    Using oDs As New DataSet
                        oDs.ReadXml(oSr)
                        rAddressedRoutes.DataSource = oDs
                        rAddressedRoutes.DataBind()
                    End Using
                End Using
            Else
                Response.Write("<h1>..nothing..</h1>")
            End If

            Dim ddlDropDate As DropDownList = DirectCast(e.Item.FindControl("ddlDropDate"), DropDownList)
            Dim drpDate As Date = Date.Parse(DataBinder.Eval(e.Item.DataItem, "Date"))
            If ddlDropDate IsNot Nothing Then
                Dim i As Int16 = 1
                While i < 6
                    Dim listItem As New ListItem()
                    listItem.Text = drpDate.AddDays(i * 7)
                    listItem.Value = drpDate.AddDays(i * 7)
                    ddlDropDate.Items.Add(listItem)
                    i = i + 1
                End While
                ddlDropDate.DataBind()
            Else
                pnlError.Visible = True
                litErrorMessage.Text = "Drop Date is nothing. (rDrops_ItemDataBound)"
            End If



        End If
    End Sub



    Protected Sub BindEDDMCart()

        Dim eddmProdNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='EDDM']")
        Dim numberOfDrops As Integer = 0
        Dim dFirstDropDate As DateTime
        Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Nothing
        Dim oDist As Taradel.CustomerDistribution = Nothing


        If DistributionId > 0 Then


            oDist = Taradel.CustomerDistributions.GetDistribution(DistributionId)

            If oDist IsNot Nothing Then

                'Show Campaign name
                If Not String.IsNullOrEmpty(oDist.Name) Then
                    lSelectName.Text = oDist.Name
                End If


                'Get the number of drops
                Dim oNumDrops As XmlNode = eddmProdNode.SelectSingleNode("Attribute[@Name='Number of Drops']")
                If oNumDrops IsNot Nothing Then
                    numberOfDrops = Integer.Parse(xmlhelp.ReadAttribute(oNumDrops, "Value"))
                Else
                    'Count the number of drops and add this in
                    Dim oTotalDrops As XmlNodeList = eddmProdNode.SelectNodes("//Drop")
                    numberOfDrops = oTotalDrops.Count
                End If


                'get and set the first Drop Date
                Dim oFirstDrop As XmlNode = eddmProdNode.SelectSingleNode("Drops/Drop[1]")
                Dim sFirstDropDate As String = xmlhelp.ReadAttribute(oFirstDrop, "Date")
                DateTime.TryParse(sFirstDropDate, dFirstDropDate)
                dFirstDropDate = New DateTime(dFirstDropDate.Year, dFirstDropDate.Month, dFirstDropDate.Day, 23, 59, 59)

                oSummary = Taradel.CustomerDistributions.GetSelectionSummary(oDist.ReferenceId)

            End If


        End If


        'get and build the Product Obj
        Dim oProduct As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(ProductID)

        'Get the options for this product.
        Dim oOpts As XmlNodeList = eddmProdNode.SelectNodes("Attribute[@OptCatId != 0]")
        Dim oOptList As New SortedList(Of Integer, Integer)

        If (oOpts.Count > 0) Then
            For Each oOpt As XmlNode In oOpts
                Dim optionId As Integer = 0
                Dim optCatId As Integer = 0
                Integer.TryParse(xmlhelp.ReadAttribute(oOpt, "Value"), optionId)
                Integer.TryParse(xmlhelp.ReadAttribute(oOpt, "OptCatId"), optCatId)
                If (optCatId > 0) Then
                    oOptList.Add(optCatId, optionId)
                End If
            Next
        End If


        'Determine if it is a professional design
        Dim bDesign As Boolean = False
        Dim oDesign As XmlNode = eddmProdNode.SelectSingleNode("Attribute[@Name='Professional Design Services']")
        If oDesign IsNot Nothing Then
            bDesign = True
        End If


        'Determine if this is a Template design
        Dim bTemplate As Boolean = False
        Dim oFront As XmlNode = eddmProdNode.SelectSingleNode("Design/Front")
        If oFront IsNot Nothing Then
            Dim sDesignType As String = xmlhelp.ReadAttribute(oFront, "DesignSelectionType")
            If sDesignType.ToLower = "template" Then
                bTemplate = True
            End If
        End If


        'All is used for displaying user selection description.  
        'Ex: Your selection of 4 carrier routes, across 2 zip codes targeting Residential deliveries will reach 2,243 postal customers.
        Dim iResTotal As Integer = 0
        Dim iBizTotal As Integer = 0
        Dim iBoxTotal As Integer = 0
        Dim iAreaCount As Integer = 0
        Dim aZips As New ArrayList
        Dim bBiz As Boolean = True
        Dim bBox As Boolean = True

        If oSummary IsNot Nothing Then

            bBiz = oSummary.UseBusiness
            bBox = oSummary.UsePOBox

            Dim oSelects As List(Of Taradel.MapServer.UserData.AreaSelection) = Taradel.CustomerDistributions.GetSelections(oDist.ReferenceId)
            For Each oArea As Taradel.MapServer.UserData.AreaSelection In oSelects
                iResTotal = iResTotal + oArea.Residential

                If bBiz Then
                    iBizTotal = iBizTotal + oArea.Business
                End If

                If bBox Then
                    iBoxTotal = iBoxTotal + oArea.POBoxes
                End If
                iAreaCount = iAreaCount + 1

                If Not aZips.Contains(oArea.Name.Substring(0, 5)) Then
                    aZips.Add(oArea.Name.Substring(0, 5))
                End If
            Next

            Dim iZipCount As Integer = aZips.Count
            Dim sZipPlural As String = "s"
            If iZipCount = 1 Then sZipPlural = ""

            Dim sTargetDesc As String = ""
            If bBox And bBiz Then
                sTargetDesc = ", Business and Post Office Box"
            ElseIf bBox Then
                sTargetDesc = " and Post Office Box"
            ElseIf bBiz Then
                sTargetDesc = " and Business"
            End If

            Dim iTotal As Integer = iResTotal + iBizTotal + iBoxTotal

            lSelectDescription.Text = "Your selection of " & iAreaCount.ToString("N0") & " carrier routes, across " & iZipCount.ToString("N0") & " zip code" & sZipPlural & " targeting Residential" & sTargetDesc & " deliveries will reach " & iTotal.ToString("N0") & " postal customers."

        End If


        'QTY.  AKA TotalMailed. Selected x Num of Impressions
        Dim iQty As Integer = 0
        Integer.TryParse(xmlhelp.ReadAttribute(eddmProdNode, "Quantity"), iQty)


        Dim sZip As String = ""
        If DistributionId = 0 Then

            'Direct ship product, need zip code
            Dim oShip As XmlNode = eddmProdNode.SelectSingleNode("shipments/shipment[1]")

            If oShip IsNot Nothing Then
                Try
                    sZip = xmlhelp.ReadAttribute(oShip, "pricehash").Split(New Char() {"-"})(1)
                Catch ex As Exception
                End Try
            End If

        End If


        'Mark Up
        Dim markup As Double = 0
        Dim markupType As String = "percent"
        If markup > 0 AndAlso markupType.ToLower = "percent" Then
            If markup > 1 Then
                markup = markup / 100
            End If
        End If


        'Build the Quote Obj
        Try
            oQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProduct.BaseProductID.Value, iQty, DistributionId, numberOfDrops, oOptList, sZip, bDesign, bTemplate, markup, markupType)

            'Now that we have a Qoute Obj, set the ShipTotal Property. Used later.
            EDDMShipPrice = oQuote.ShipPrice

        Catch ex As Exception
            pnlError.Visible = True
            phQuoteInfoDisplay.Visible = False
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("Error in building oQuote on QuotePDF. <br /><br />Message: <br />" & ex.StackTrace.ToString())
        End Try



        If markup > 0 Then
            oBaseQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oQuote.ProductId, oQuote.Quantity, oQuote.UserDistributionId, oQuote.NumberOfDrops, oQuote.Options, oQuote.ZipCode, oQuote.WithDesign, oQuote.WithTemplate, 0, "")
        Else
            oBaseQuote = oQuote
        End If




        'Get Number of weeks
        Dim sFreq As String = xmlhelp.ReadAttribute(eddmProdNode.SelectSingleNode("Attribute[@Name='Drop Schedule']"), "Value")
        Dim iFreq As Integer = 0

        If Integer.TryParse(sFreq, 0) Then
            iFreq = XmlConvert.ToInt32(sFreq)
        End If



        'Professional Design
        If oQuote.WithDesign Then
            phProDesign.Visible = True

            'My Design, Template Design
        Else
            phMyDesign.Visible = True

            Dim sIndex As String = xmlhelp.ReadAttribute(eddmProdNode, "Index")
            Dim bHasFile As Boolean = False
            Dim oGNode As XmlNode = eddmProdNode.SelectSingleNode("Design")

            If oGNode IsNot Nothing Then

                Dim oFNode As XmlNode = oGNode.SelectSingleNode("Front")
                If oFNode IsNot Nothing Then

                    Dim sDesignSelectionType As String = xmlhelp.ReadAttribute(oFNode, "DesignSelectionType").ToString.ToLowerInvariant

                    Select Case (sDesignSelectionType)
                        Case "multiad"

                        Case "upload"
                            '-- This was an uploaded file, the data is contained in the xml
                            imgFile1.ImageUrl = "/resources/imgthumb.ashx?ID=" & sIndex & "&r=0&fb=Front"
                            bHasFile = True
                            imgFile1.Visible = True
                            lblDesignType.Text = "My Custom Design: "

                        Case "template"

                            Dim sTemplateServerHost As String = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
                            Dim sTemplateId As String = appxCMS.Util.Xml.ReadAttribute(oFNode, "ArtKey")
                            Dim templateId As Integer = 0

                            If Integer.TryParse(sTemplateId, templateId) Then

                                lLaterMsg.Text = "Template #" & templateId
                                Dim oTemplate As TemplateCode.Template1 = Nothing

                                Using oAPI As New TemplateCode.TemplateAPIClient
                                    Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, templateId)
                                    Dim oResponse As TemplateCode.GetTemplateResponse = oAPI.GetTemplate(oRequest)
                                    oTemplate = oResponse.GetTemplateResult
                                End Using

                                If oTemplate IsNot Nothing Then
                                    imgFile1.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.FrontImage
                                    imgFile1.Visible = True

                                    If Not String.IsNullOrEmpty(oTemplate.BackImage) Then
                                        imgFile2.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.BackImage
                                        imgFile2.Visible = True
                                    Else
                                        imgFile2.Visible = False
                                    End If

                                    lblDesignType.Text = "Selected Template: "

                                End If
                                bHasFile = True

                            End If

                        Case "designdiy"
                            'Nothing at this time
                        Case Else
                            imgFile1.Visible = False
                    End Select
                End If

                Dim oBNode As XmlNode = oGNode.SelectSingleNode("Back")
                If oBNode IsNot Nothing Then
                    Dim sDesignSelectionType As String = xmlhelp.ReadAttribute(oBNode, "DesignSelectionType").ToString.ToLowerInvariant
                    If sDesignSelectionType = "multiad" Then
                    ElseIf sDesignSelectionType = "upload" Then
                        '-- This was an uploaded file, the data is contained in the xml
                        imgFile2.ImageUrl = "/resources/imgthumb.ashx?ID=" & sIndex & "&r=0&fb=Back"
                        bHasFile = True
                        imgFile2.Visible = True
                    ElseIf sDesignSelectionType = "template" Then
                        Dim sTemplateServerHost As String = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
                        Dim sTemplateId As String = appxCMS.Util.Xml.ReadAttribute(oBNode, "ArtKey")
                        Dim templateId As Integer = 0
                        If Integer.TryParse(sTemplateId, templateId) Then
                            lLaterMsg.Text = lLaterMsg.Text & " and #" & templateId
                            Dim oTemplate As TemplateCode.Template1 = Nothing
                            Using oAPI As New TemplateCode.TemplateAPIClient
                                Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, templateId)
                                Dim oResponse As TemplateCode.GetTemplateResponse = oAPI.GetTemplate(oRequest)
                                oTemplate = oResponse.GetTemplateResult
                            End Using
                            If oTemplate IsNot Nothing Then
                                imgFile2.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.FrontImage
                                imgFile2.Visible = True
                            End If
                        End If
                    ElseIf sDesignSelectionType = "designdiy" Then
                        'Nothing at this time...
                    Else
                        '-- This page design was omitted
                        imgFile2.Visible = False
                    End If
                Else
                    If String.IsNullOrEmpty(imgFile2.ImageUrl) Then
                        imgFile2.Visible = False
                    End If
                End If

            End If

            If Not bHasFile Then
                imgFile1.Visible = False
                imgFile2.Visible = False
                lLaterMsg.Text = "I will upload my files later."
                lblDesignType.Text = "Custom Design: "
            End If

        End If


        If oDist IsNot Nothing Then
            Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(oDist.USelectMethodReference.ForeignKey)
            Dim sMapImage As String = ""
            Dim sMapReview As String = oUSelect.ReviewUrl
            sMapImage = sMapReview & "?referenceid=" & oDist.ReferenceId
            lMapReview.Text = "<img src=""" & sMapImage & """ class=""img-thumbnail img-responsive receiptMap"" />"
        Else
            lMapReview.Text = "oDist is null"
        End If



        'Databind the options repeater
        If (oOpts.Count > 0) Then

            Dim oOptSb As New StringBuilder
            oOptSb.AppendLine("<attributes>")

            For Each oOpt As XmlNode In oOpts
                oOptSb.AppendLine(oOpt.OuterXml)
            Next

            oOptSb.AppendLine("</attributes>")

            Try
                Using oSr As New StringReader(oOptSb.ToString)
                    Using oDs As New DataSet()
                        oDs.ReadXml(oSr)
                        rEddmOpts.DataSource = oDs
                        rEddmOpts.DataBind()
                    End Using
                End Using
            Catch ex As Exception

            End Try
        End If



        'EDDM Drops
        Dim oDropsOrg As XmlNode = eddmProdNode.SelectSingleNode("Drops[@Type='EDDM']")
        If oDropsOrg IsNot Nothing Then

            Dim oDrops As XmlNode = oDropsOrg.CloneNode(True)

            oDrops.Attributes.RemoveAll()

            Using oSr As New StringReader(oDrops.OuterXml)
                Using oDs As New DataSet
                    oDs.ReadXml(oSr)
                    rptEddmDrops.DataSource = oDs
                    rptEddmDrops.DataBind()
                End Using
            End Using

        End If



        'Build Product Name label
        Dim sProdName As String = xmlhelp.ReadAttribute(eddmProdNode, "Name")
        litEddmProductName.Text = sProdName

        ''adding logic here to account for 1000 minimum quantity 11/4/2015  1 of 2 'parameterize this if necessary
        Dim metTheMinimum As Integer = 0
        metTheMinimum = (TotalSelected * NumOfImpressions) + ExtraCopies
        'end adding logic here to account for 1000 minimum quantity 11/4/2015  1 of 2


        'Update the EDDM Mailing Row
        lblPrintingEstimate.Text = ((TotalSelected * NumOfImpressions) * PricePerPiece).ToString("C")
        litNumOfPcs.Text = (TotalSelected * NumOfImpressions).ToString("N0") & " @ " & PricePerPiece.ToString("C")


        litExtraPcs.Text = ExtraCopies.ToString("N0") & " @ " & ExtraPcsPricePerPiece.ToString("C")
        litShipTo.Text = ShipToAddress
        lblShippingEstimate.Text = (ExtraPcsPricePerPiece * ExtraCopies).ToString("C")


        'SubTotal & Total
        Dim subTotal As Decimal = ((TotalSelected * NumOfImpressions) * PricePerPiece) + (DesignFee) + (DropFee) + (ExtraCopies * ExtraPcsPricePerPiece)
        lblSubTotal.Text = subTotal.ToString("C")

        Dim total As Decimal = (subTotal + SalesTax)
        TotalEstimate.Text = total.ToString("C")

        ''adding logic here to account for 1000 minimum quantity 11/4/2015  2 of 2 'parameterize this if necessary
        If metTheMinimum < 1000 Then
            lblPrintingEstimate.Text = (1000 * PricePerPiece).ToString("C")
            litNumOfPcs.Text = (TotalSelected * NumOfImpressions).ToString("N0") & " (1000 minimum) @ " & PricePerPiece.ToString("C")
            subTotal = (1000 * PricePerPiece) + (DesignFee) + (DropFee) + (ExtraCopies * ExtraPcsPricePerPiece)
            lblSubTotal.Text = subTotal.ToString("C")
            total = subTotal
            TotalEstimate.Text = total.ToString("C")
        End If
        ''end adding logic here to account for 1000 minimum quantity 11/4/2015  2 of 2 'parameterize this if necessary

    End Sub



    Protected Sub BindAddressedCart()

        Dim listProdNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='AddressedList']")
        Dim numberOfDrops As Integer = 0
        Dim dFirstDropDate As DateTime
        Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Nothing
        Dim oDist As Taradel.CustomerDistribution = Nothing


        If DistributionId > 0 Then


            oDist = Taradel.CustomerDistributions.GetDistribution(DistributionId)

            If oDist IsNot Nothing Then

                'Show Campaign name
                If Not String.IsNullOrEmpty(oDist.Name) Then
                    lSelectName.Text = oDist.Name
                End If


                'Get the number of drops
                Dim oNumDrops As XmlNode = listProdNode.SelectSingleNode("Attribute[@Name='Number of Drops']")
                If oNumDrops IsNot Nothing Then
                    numberOfDrops = Integer.Parse(xmlhelp.ReadAttribute(oNumDrops, "Value"))
                Else
                    'Count the number of drops and add this in
                    Dim oTotalDrops As XmlNodeList = listProdNode.SelectNodes("//Drop")
                    numberOfDrops = oTotalDrops.Count
                End If


                'get and set the first Drop Date
                Dim oFirstDrop As XmlNode = listProdNode.SelectSingleNode("Drops/Drop[1]")
                Dim sFirstDropDate As String = xmlhelp.ReadAttribute(oFirstDrop, "Date")
                DateTime.TryParse(sFirstDropDate, dFirstDropDate)
                dFirstDropDate = New DateTime(dFirstDropDate.Year, dFirstDropDate.Month, dFirstDropDate.Day, 23, 59, 59)

                oSummary = Taradel.CustomerDistributions.GetSelectionSummary(oDist.ReferenceId)

            End If


        End If


        'get and build the Product Obj
        Dim oProduct As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(ProductID)

        'Get the options for this product.
        Dim oOpts As XmlNodeList = listProdNode.SelectNodes("Attribute[@OptCatId != 0]")
        Dim oOptList As New SortedList(Of Integer, Integer)

        If (oOpts.Count > 0) Then
            For Each oOpt As XmlNode In oOpts
                Dim optionId As Integer = 0
                Dim optCatId As Integer = 0
                Integer.TryParse(xmlhelp.ReadAttribute(oOpt, "Value"), optionId)
                Integer.TryParse(xmlhelp.ReadAttribute(oOpt, "OptCatId"), optCatId)
                If (optCatId > 0) Then
                    oOptList.Add(optCatId, optionId)
                End If
            Next
        End If


        'Determine if it is a professional design
        Dim bDesign As Boolean = False
        Dim oDesign As XmlNode = listProdNode.SelectSingleNode("Attribute[@Name='Professional Design Services']")
        If oDesign IsNot Nothing Then
            bDesign = True
        End If


        'Determine if this is a Template design
        Dim bTemplate As Boolean = False
        Dim oFront As XmlNode = listProdNode.SelectSingleNode("Design/Front")
        If oFront IsNot Nothing Then
            Dim sDesignType As String = xmlhelp.ReadAttribute(oFront, "DesignSelectionType")
            If sDesignType.ToLower = "template" Then
                bTemplate = True
            End If
        End If



        'Map Description & List Drop Details
        Dim numPcs = CartUtility.GetAddressedDropTotal(oCart, IsMultiple, numberOfDrops, TotalSelected, "AddressedList")
        lSelectDescription.Text = "Your specialized criteria will allow you to reach and deliver to " & numPcs.ToString("N0") & " targeted addresses."


        'Show Drop and Filter Details
        BuildAddressedListDetails(numPcs)



        'QTY.  AKA TotalMailed. Selected x Num of Impressions
        Dim iQty As Integer = 0
        Integer.TryParse(xmlhelp.ReadAttribute(listProdNode, "Quantity"), iQty)


        Dim sZip As String = ""
        If DistributionId = 0 Then

            'Direct ship product, need zip code
            Dim oShip As XmlNode = listProdNode.SelectSingleNode("shipments/shipment[1]")

            If oShip IsNot Nothing Then
                Try
                    sZip = xmlhelp.ReadAttribute(oShip, "pricehash").Split(New Char() {"-"})(1)
                Catch ex As Exception
                    pnlError.Visible = True
                    litErrorMessage.Text = ex.ToString()
                End Try
            End If

        End If



        'Mark Up
        Dim markup As Double = 0
        Dim markupType As String = "percent"
        If markup > 0 AndAlso markupType.ToLower = "percent" Then
            If markup > 1 Then
                markup = markup / 100
            End If
        End If


        'Build the Quote Obj
        Try
            oQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProduct.BaseProductID.Value, iQty, DistributionId, numberOfDrops, oOptList, sZip, bDesign, bTemplate, markup, markupType)

            'Now that we have a Qoute Obj, set the ShipTotal Property. Used later.
            EDDMShipPrice = oQuote.ShipPrice

        Catch ex As Exception
            pnlError.Visible = True
            phQuoteInfoDisplay.Visible = False
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("Error in building oQuote on QuotePDF. <br /><br />Message: <br />" & ex.StackTrace.ToString())
        End Try



        If markup > 0 Then
            oBaseQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oQuote.ProductId, oQuote.Quantity, oQuote.UserDistributionId, oQuote.NumberOfDrops, oQuote.Options, oQuote.ZipCode, oQuote.WithDesign, oQuote.WithTemplate, 0, "")
        Else
            oBaseQuote = oQuote
        End If




        'Get Number of weeks
        Dim sFreq As String = xmlhelp.ReadAttribute(listProdNode.SelectSingleNode("Attribute[@Name='Drop Schedule']"), "Value")
        Dim iFreq As Integer = 0

        If Integer.TryParse(sFreq, 0) Then
            iFreq = XmlConvert.ToInt32(sFreq)
        End If



        'Professional Design
        If oQuote.WithDesign Then
            phProDesign.Visible = True

            'My Design, Template Design
        Else
            phMyDesign.Visible = True

            Dim sIndex As String = xmlhelp.ReadAttribute(listProdNode, "Index")
            Dim bHasFile As Boolean = False
            Dim oGNode As XmlNode = listProdNode.SelectSingleNode("Design")

            If oGNode IsNot Nothing Then

                Dim oFNode As XmlNode = oGNode.SelectSingleNode("Front")
                If oFNode IsNot Nothing Then

                    Dim sDesignSelectionType As String = xmlhelp.ReadAttribute(oFNode, "DesignSelectionType").ToString.ToLowerInvariant

                    Select Case (sDesignSelectionType)
                        Case "multiad"

                        Case "upload"
                            '-- This was an uploaded file, the data is contained in the xml
                            imgFile1.ImageUrl = "/resources/imgthumb.ashx?ID=" & sIndex & "&r=0&fb=Front"
                            bHasFile = True
                            imgFile1.Visible = True
                            lblDesignType.Text = "My Custom Design: "

                        Case "template"

                            Dim sTemplateServerHost As String = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
                            Dim sTemplateId As String = appxCMS.Util.Xml.ReadAttribute(oFNode, "ArtKey")
                            Dim templateId As Integer = 0

                            If Integer.TryParse(sTemplateId, templateId) Then

                                lLaterMsg.Text = "Template #" & templateId
                                Dim oTemplate As TemplateCode.Template1 = Nothing

                                Using oAPI As New TemplateCode.TemplateAPIClient
                                    Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, templateId)
                                    Dim oResponse As TemplateCode.GetTemplateResponse = oAPI.GetTemplate(oRequest)
                                    oTemplate = oResponse.GetTemplateResult
                                End Using

                                If oTemplate IsNot Nothing Then
                                    imgFile1.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.FrontImage
                                    imgFile1.Visible = True

                                    If Not String.IsNullOrEmpty(oTemplate.BackImage) Then
                                        imgFile2.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.BackImage
                                        imgFile2.Visible = True
                                    Else
                                        imgFile2.Visible = False
                                    End If

                                    lblDesignType.Text = "Selected Template: "

                                End If
                                bHasFile = True

                            End If

                        Case "designdiy"
                            'Nothing at this time
                        Case Else
                            imgFile1.Visible = False
                    End Select
                End If

                Dim oBNode As XmlNode = oGNode.SelectSingleNode("Back")
                If oBNode IsNot Nothing Then
                    Dim sDesignSelectionType As String = xmlhelp.ReadAttribute(oBNode, "DesignSelectionType").ToString.ToLowerInvariant
                    If sDesignSelectionType = "multiad" Then
                    ElseIf sDesignSelectionType = "upload" Then
                        '-- This was an uploaded file, the data is contained in the xml
                        imgFile2.ImageUrl = "/resources/imgthumb.ashx?ID=" & sIndex & "&r=0&fb=Back"
                        bHasFile = True
                        imgFile2.Visible = True
                    ElseIf sDesignSelectionType = "template" Then
                        Dim sTemplateServerHost As String = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
                        Dim sTemplateId As String = appxCMS.Util.Xml.ReadAttribute(oBNode, "ArtKey")
                        Dim templateId As Integer = 0
                        If Integer.TryParse(sTemplateId, templateId) Then
                            lLaterMsg.Text = lLaterMsg.Text & " and #" & templateId
                            Dim oTemplate As TemplateCode.Template1 = Nothing
                            Using oAPI As New TemplateCode.TemplateAPIClient
                                Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, templateId)
                                Dim oResponse As TemplateCode.GetTemplateResponse = oAPI.GetTemplate(oRequest)
                                oTemplate = oResponse.GetTemplateResult
                            End Using
                            If oTemplate IsNot Nothing Then
                                imgFile2.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.FrontImage
                                imgFile2.Visible = True
                            End If
                        End If
                    ElseIf sDesignSelectionType = "designdiy" Then
                        'Nothing at this time...
                    Else
                        '-- This page design was omitted
                        imgFile2.Visible = False
                    End If
                Else
                    If String.IsNullOrEmpty(imgFile2.ImageUrl) Then
                        imgFile2.Visible = False
                    End If
                End If

            End If

            If Not bHasFile Then
                imgFile1.Visible = False
                imgFile2.Visible = False
                lLaterMsg.Text = "I will upload my files later."
                lblDesignType.Text = "Custom Design: "
            End If

        End If


        If oDist IsNot Nothing Then
            Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(oDist.USelectMethodReference.ForeignKey)
            Dim sMapImage As String = ""
            Dim sMapReview As String = oUSelect.ReviewUrl
            sMapImage = sMapReview & "?referenceid=" & oDist.ReferenceId
            lMapReview.Text = "<img src=""" & sMapImage & """ class=""img-thumbnail img-responsive receiptMap"" />"
        Else
            lMapReview.Text = "oDist is null"
        End If



        'Databind the options repeater
        If (oOpts.Count > 0) Then

            Dim oOptSb As New StringBuilder
            oOptSb.AppendLine("<attributes>")

            For Each oOpt As XmlNode In oOpts
                oOptSb.AppendLine(oOpt.OuterXml)
            Next

            oOptSb.AppendLine("</attributes>")

            Try
                Using oSr As New StringReader(oOptSb.ToString)
                    Using oDs As New DataSet()
                        oDs.ReadXml(oSr)
                        rptAddressedOpts.DataSource = oDs
                        rptAddressedOpts.DataBind()
                    End Using
                End Using
            Catch ex As Exception

            End Try
        End If



        'Addressed Drops
        Dim oDropsOrg As XmlNode = listProdNode.SelectSingleNode("Drops[@Type='AddressedList']")
        If oDropsOrg IsNot Nothing Then

            Dim oDrops As XmlNode = oDropsOrg.CloneNode(True)

            oDrops.Attributes.RemoveAll()

            Using oSr As New StringReader(oDrops.OuterXml)
                Using oDs As New DataSet
                    oDs.ReadXml(oSr)
                    rptEddmDrops.DataSource = oDs
                    rptEddmDrops.DataBind()
                End Using
            End Using

        End If



        'Build Product Name label
        Dim sProdName As String = xmlhelp.ReadAttribute(listProdNode, "Name")
        litAddressedProductName.Text = sProdName

        ''adding logic here to account for 1000 minimum quantity 11/4/2015  1 of 2 'parameterize this if necessary
        Dim metTheMinimum As Integer = 0
        metTheMinimum = (TotalSelected * NumOfImpressions) + ExtraCopies
        'end adding logic here to account for 1000 minimum quantity 11/4/2015  1 of 2


        'Update the EDDM Mailing Row
        lblPrintingEstimate.Text = ((TotalSelected * NumOfImpressions) * PricePerPiece).ToString("C")
        litNumOfPcs.Text = (TotalSelected * NumOfImpressions).ToString("N0") & " @ " & PricePerPiece.ToString("C")


        litExtraPcs.Text = ExtraCopies.ToString("N0") & " @ " & ExtraPcsPricePerPiece.ToString("C")
        litShipTo.Text = ShipToAddress
        lblShippingEstimate.Text = (ExtraPcsPricePerPiece * ExtraCopies).ToString("C")


        'SubTotal & Total
        Dim subTotal As Decimal = ((TotalSelected * NumOfImpressions) * PricePerPiece) + (DesignFee) + (DropFee) + (ExtraCopies * ExtraPcsPricePerPiece)
        lblSubTotal.Text = subTotal.ToString("C")

        Dim total As Decimal = (subTotal + SalesTax)
        TotalEstimate.Text = total.ToString("C")

        ''adding logic here to account for 1000 minimum quantity 11/4/2015  2 of 2 'parameterize this if necessary
        If metTheMinimum < 1000 Then
            lblPrintingEstimate.Text = (1000 * PricePerPiece).ToString("C")
            litNumOfPcs.Text = (TotalSelected * NumOfImpressions).ToString("N0") & " (1000 minimum) @ " & PricePerPiece.ToString("C")
            subTotal = (1000 * PricePerPiece) + (DesignFee) + (DropFee) + (ExtraCopies * ExtraPcsPricePerPiece)
            lblSubTotal.Text = subTotal.ToString("C")
            total = subTotal
            TotalEstimate.Text = total.ToString("C")
        End If
        ''end adding logic here to account for 1000 minimum quantity 11/4/2015  2 of 2 'parameterize this if necessary




    End Sub



    Private Sub BuildSavedCartObj()


        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim errorMsg As New StringBuilder()
        Dim selectSql As String = "EXEC usp_GetSavedCart @paramSiteID = " & SiteID & ", @paramGUID = '" & QuoteKey & "'"
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)
        Dim sqlReader As SqlDataReader


        Try

            connObj.Open()
            sqlReader = sqlCommand.ExecuteReader()

            If sqlReader.HasRows Then
                sqlReader.Read()

                SavedCartObj.QuoteID = Convert.ToInt32(sqlReader("pnd_SavedCartID"))
                SavedCartObj.SiteID = sqlReader("SiteID")
                SavedCartObj.QuoteGUID = sqlReader("GUID")
                SavedCartObj.Cart = sqlReader("Cart")
                SavedCartObj.ExpirationDate = sqlReader("ExpirationDate")
                SavedCartObj.CreatedDate = sqlReader("CreatedDate")
                SavedCartObj.CreatedBy = sqlReader("CreatedBy")
            Else
                phQuote.Visible = False
                pnlError.Visible = True
                litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
                EmailUtility.SendAdminEmail("Error in BuildSavedCartObj(). Huh? No Rows were found.  (QuotePDF.aspx). Details:<br />" & errorMsg.ToString())
            End If

        Catch objException As Exception

            errorMsg.Append("The following errors occurred:<br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " & objException.Message & "</li>")
            errorMsg.Append("<li>Source: " & objException.Source & "<li>")
            errorMsg.Append("<li>Stack Trace: " & objException.StackTrace & "<li>")
            errorMsg.Append("<li>Target Site: " & objException.TargetSite.Name & "<li>")
            errorMsg.Append("<li>SQL: " & selectSql & "<li>")
            errorMsg.Append("</ul>")

            phQuote.Visible = False
            pnlError.Visible = True
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("Error in BuildSavedCartObj(). (QuotePDF.aspx). Details:<br />" & errorMsg.ToString())


        Finally

            connObj.Close()

        End Try



    End Sub



    Protected Sub BuildAddressedListDetails(numPcs As Integer)

        litAddressedPcs.Text = CartUtility.GetDropQtyList(oCart, "AddressedList")
        litAddressedDropDates.Text = CartUtility.GetDropDatesList(oCart, "AddressedList")

        If (GeneratedAddressedList) Then
            litDemographicFilters.Text = AddressedListUtility.GetSavedFilters(DistributionId)
        Else
            litDemographicFilters.Text = "(not applicable to uploaded lists)"
        End If


    End Sub






    'Page Builders
    Protected Sub ShowHidePageElements()

        BuildTaradelHeader()
        BuildQuoteHeader()


        'Show the correct Product panels
        If (EDDMMap) Then
            phEddmProduct.Visible = True
            phEDDMDrops.Visible = True
            phAddressedProduct.Visible = False
            phAddressedDrops.Visible = False
        End If

        If (GeneratedAddressedList) Or (UploadedAddressedList) Then
            phEddmProduct.Visible = False
            phEDDMDrops.Visible = False
            phAddressedProduct.Visible = True
            phAddressedDrops.Visible = True
        End If




        'Extra Copies
        If ExtraCopies > 0 Then
            phExtraPcs.Visible = True
            BuildExtraCopiesDisplay()
        End If


        'Professional Design
        If (IsProfessionalDesign) Then
            phDesignFee.Visible = True
            lblDesignFee.Text = DesignFee.ToString("C")
        End If


        'Drop Fee
        If (HasDropFee) Then
            phNumOfDrops.Visible = True
            BuildDropFeeDisplay()
        End If


        'Sales Tax 
        If (SalesTax) > 0 Then
            phSalesTax.Visible = True
            BuildSalexTaxDisplay()
        End If



        'Addtional Site data.  Build SiteDetails Obj
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

        litPONumberMsg.Text = SiteUtility.GetStringResourceValue(SiteID, "POLabel") & "#: " & PONumber
        litCustNumLabel.Text = SiteUtility.GetStringResourceValue(SiteID, "CustNumLabel")


    End Sub



    Protected Sub BuildExtraCopiesDisplay()

        'This needs to emulate what the usp_RetrieveOrderPaymentDetails and OrderCalculator do.  The OrderCalc is dependent on the Profile.Cart and 
        'the Authorized User.  This page cannot use those so we need to emulate how the ExtraCopies PPP is calculated.

        litExtraPcs.Text = ExtraCopies.ToString("N0") & " @ " & ExtraPcsPricePerPiece.ToString("C")
        litShipTo.Text = ShipToAddress
        lblShippingEstimate.Text = (ExtraPcsPricePerPiece * ExtraCopies).ToString("C")


    End Sub



    Protected Sub BuildDropFeeDisplay()

        Dim drops As Integer = 0

        If NumOfDrops > 1 Then
            drops = NumOfDrops
        End If

        If NumOfImpressions > 1 Then
            drops = NumOfImpressions
        End If

        litNumOfDrops.Text = (drops - 1).ToString() & " @ $99"
        lblNumOfDrops.Text = DropFee.ToString("C")


    End Sub



    Protected Sub BuildSalexTaxDisplay()

        lblSalesTax.Text = SalesTax.ToString("C")
        lblSalesTaxMessage.Text = "TBD"

    End Sub



    Protected Sub BuildTaradelHeader()

        Dim sPhoneNumber As String = ""
        Dim addressText As String = ""

        Try

            Dim oSite As appxCMS.Site = appxCMS.SiteDataSource.GetSite()

            If oSite IsNot Nothing Then

                Dim bHasPhone As Boolean = False
                If oSite.TollFreeNumber IsNot Nothing Then
                    If Not String.IsNullOrEmpty(oSite.TollFreeNumber) Then
                        sPhoneNumber = oSite.TollFreeNumber
                        bHasPhone = True
                    End If
                End If

                If Not bHasPhone Then
                    If oSite.PhoneNumber IsNot Nothing Then
                        If Not String.IsNullOrEmpty(oSite.PhoneNumber) Then
                            sPhoneNumber = oSite.PhoneNumber
                            bHasPhone = True
                        End If
                    End If
                End If

                litQuoteExpiration.Text = SavedCartObj.ExpirationDate.ToShortDateString()
                lReceiptPartnerPhone.Text = sPhoneNumber
                lReceiptPartnerName.Text = oSite.Name

                'Address string
                addressText = oSite.Address1 & "<br />"

                If oSite.Address2.Length > 0 Then
                    addressText = addressText & ", " & oSite.Address2 & "<br />"
                End If

                addressText = addressText & " " & oSite.City & ", " & oSite.State & " " & oSite.ZipCode & "<br />"

                lReceiptPartnerAddress.Text = addressText

                'Hide certains things for Staples
                If siteID = 78 Then
                    lReceiptPartnerName.Visible = False
                    lReceiptPartnerAddress.Visible = False
                    lReceiptPartnerPhone.Visible = False
                End If


                'Default values
            Else
                lReceiptPartnerPhone.Text = "(804) 364-8444"
                lReceiptPartnerName.Text = "<strong>Taradel</strong>"
                lReceiptPartnerAddress.Text = "4805 Lake Brooke Drive - Suite 140<br/>Glen Allen, VA 23060<br />"
            End If

        Catch ex As Exception

        End Try


    End Sub



    Protected Sub BuildQuoteHeader()

        Dim customerObj As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(UserName)

        litQuoteDate.Text = DateTime.Today.ToShortDateString()

        If customerObj IsNot Nothing Then

            litName.Text = customerObj.FirstName & " " & customerObj.LastName
            litCustCompany.Text = customerObj.Company
            litCustAddress.Text = customerObj.Address1
            litCityStateZip.Text = customerObj.City + " " + customerObj.State + ", " + customerObj.ZipCode
            litPhone.Text = customerObj.PhoneNumber
            litCustomerID.Text = customerObj.CustomerID.ToString()

            'For Staples Act Mgr Site 95.  If other sites start to use this feature then make it configurable!
            If SiteID = 95 Then
                pnlStoreNumber.Visible = True
                litStoreNumber.Text = StoreNumber
            End If

        Else

            phQuoteInfoDisplay.Visible = False
            pnlError.Visible = True
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("customerObj was NULL (QuotePDF - BuildQuoteHeader).")

        End If

        customerObj = Nothing

    End Sub





    'Helpers / Calculators
    Public Function ReturnTheNextMonday(oldDate As Object) As String

        Dim theNewDate As DateTime
        DateTime.TryParse(oldDate.ToString(), theNewDate)

        While (theNewDate.DayOfWeek <> DayOfWeek.Monday)
            theNewDate = theNewDate.AddDays(1)
        End While
        Return theNewDate.ToString("dddd, MMM, dd yyyy")

    End Function



    Public Function CalculateStateSalesTax(amount As Decimal, state As String, numItems As Integer) As Decimal

        Dim returnThis As Decimal = 0
        Dim postageFee As Double = 0

        If (Session("PostageFee") IsNot Nothing) Then
            postageFee = Convert.ToDouble(Session("PostageFee"))
        End If

        'subtract postage fee from amount prior to tax calculation
        amount = amount - postageFee

        Dim salestaxcalc As New SalesTaxCalculator(amount, state)
        salestaxcalc.Calculate()
        Return salestaxcalc.Amount

    End Function







    'Debug
    Protected Sub ShowDebug()

        pnlDebug.CssClass = String.Empty
        pnlDebug.CssClass = "alert alert-danger"


        litEnvironmentMode.Text = environmentMode
        litSiteID.Text = SiteID.ToString
        litTestMode.Text = TestMode.ToString().ToUpper()
        litDistributionID.Text = DistributionId.ToString()
        litUSelectID.Text = USelectID.ToString()
        litProductID.Text = ProductID.ToString()
        litBaseProductID.Text = BaseProductID.ToString()
        litEDDMMap.Text = EDDMMap.ToString().ToUpper()
        litGeneratedAddressedList.Text = GeneratedAddressedList.ToString().ToUpper()
        litUploadedAddressedList.Text = UploadedAddressedList.ToString().ToUpper()
        litTMCMap.Text = TMCMap.ToString().ToUpper()
        litPostageRate.Text = PostageRate.ToString()
        litExtraCopies.Text = ExtraCopies.ToString().ToUpper()
        litIsProfessionalDesign.Text = IsProfessionalDesign.ToString().ToUpper()
        litDesignFee.Text = DesignFee.ToString()
        litHasDropFee.Text = HasDropFee.ToString().ToUpper()
        litDropFee.Text = HasDropFee.ToString()
        litSalesTax.Text = SalesTax.ToString()
        litEDDMShipPrice.Text = EDDMShipPrice.ToString()
        litAddressedShipPrice.Text = AddressedShipPrice.ToString()
        litNumOfDrops2.Text = NumOfDrops.ToString()
        litAddressedMap.Text = AddressedMap.ToString().ToUpper()
        litExtraPcsPricePerPiece.Text = ExtraPcsPricePerPiece.ToString()
        litMultipleImpressionsNoFee.Text = MultipleImpressionsNoFee.ToString().ToUpper()
        litNumOfImpressions.Text = NumOfImpressions.ToString()
        litPricePerPiece.Text = PricePerPiece.ToString()
        litQuoteKey.Text = QuoteKey.ToString()
        litShipToAddress.Text = ShipToAddress.ToString()
        litTotalSelected2.Text = TotalSelected.ToString()
        litUserName.Text = UserName.ToString()
        litPONumber.Text = PONumber.ToString()
        litIsMultiple.Text = IsMultiple.ToString().ToUpper()


    End Sub








    'Container Class
    Public Class SavedCart


        Private _QuoteID As Integer
        Public Property QuoteID() As Integer
            Get
                Return _QuoteID
            End Get
            Set(ByVal value As Integer)
                _QuoteID = value
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


        Private _QuoteGUID As String
        Public Property QuoteGUID() As String
            Get
                Return _QuoteGUID
            End Get
            Set(ByVal value As String)
                _QuoteGUID = value
            End Set
        End Property


        Private _Cart As String
        Public Property Cart() As String
            Get
                Return _Cart
            End Get
            Set(ByVal value As String)
                _Cart = value
            End Set
        End Property


        Private _ExpirationDate As Date
        Public Property ExpirationDate() As Date
            Get
                Return _ExpirationDate
            End Get
            Set(ByVal value As Date)
                _ExpirationDate = value
            End Set
        End Property


        Private _CreatedDate As Date
        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(ByVal value As Date)
                _CreatedDate = value
            End Set
        End Property


        Private _CreatedBy As String
        Public Property CreatedBy() As String
            Get
                Return _CreatedBy
            End Get
            Set(ByVal value As String)
                _CreatedBy = value
            End Set
        End Property


    End Class





End Class
