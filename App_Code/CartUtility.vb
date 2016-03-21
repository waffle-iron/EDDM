Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.HttpServerUtility
Imports System.Web.Profile
Imports System.Web.Profile.ProfileBase
Imports System.Net
Imports System.Xml
Imports System.Data
Imports System.Collections.Generic



Public Class CartUtility


    '==============================================================================================================================
    '   Written 5/29/2015.  Improve, expand as needed. DSF
    '==============================================================================================================================

    'ADD DATA

    Public Class ProductCartNode
        Private _AddedDate As String
        Public Property AddedDate() As String
            Get
                Return _AddedDate
            End Get
            Set(ByVal value As String)
                _AddedDate = value
            End Set
        End Property

        Private _baseProductID As String
        Public Property BaseProductID() As String
            Get
                Return _baseProductID
            End Get
            Set(ByVal value As String)
                _baseProductID = value
            End Set
        End Property

        Private _DesignFee As String
        Public Property DesignFee() As String
            Get
                Return _DesignFee
            End Get
            Set(ByVal value As String)
                _DesignFee = value
            End Set
        End Property

        Private _DistributionID As String
        Public Property DistributionID() As String
            Get
                Return _DistributionID
            End Get
            Set(ByVal value As String)
                _DistributionID = value
            End Set
        End Property

        Private _FlatRateShipFee As String
        Public Property FlatRateShipFee() As String
            Get
                Return _FlatRateShipFee
            End Get
            Set(ByVal value As String)
                _FlatRateShipFee = value
            End Set
        End Property

        Private _flatRateShipQty As String
        Public Property FlatRateShipQty() As String
            Get
                Return _flatRateShipQty
            End Get
            Set(ByVal value As String)
                _flatRateShipQty = value
            End Set
        End Property

        Private _IndexGUID As String
        Public Property IndexGUID() As String
            Get
                Return _IndexGUID
            End Get
            Set(ByVal value As String)
                _IndexGUID = value
            End Set
        End Property

        Private _IsFlatRateShipping As String
        Public Property IsFlatRateShipping() As String
            Get
                Return _IsFlatRateShipping
            End Get
            Set(ByVal value As String)
                _IsFlatRateShipping = value
            End Set
        End Property

        Private _JobComments As String
        Public Property JobComments() As String
            Get
                Return _JobComments
            End Get
            Set(ByVal value As String)
                _JobComments = value
            End Set
        End Property

        Private _jobName As String
        Public Property JobName() As String
            Get
                Return _jobName
            End Get
            Set(ByVal value As String)
                _jobName = value
            End Set
        End Property

        Private _ProductName As String
        Public Property ProductName() As String
            Get
                Return _ProductName
            End Get
            Set(ByVal value As String)
                _ProductName = value
            End Set
        End Property

        Private _paperHeight As String
        Public Property PaperHeight() As String
            Get
                Return _paperHeight
            End Get
            Set(ByVal value As String)
                _paperHeight = value
            End Set
        End Property


        Private _paperWidth As String
        Public Property PaperWidth() As String
            Get
                Return _paperWidth
            End Get
            Set(ByVal value As String)
                _paperWidth = value
            End Set
        End Property


        Private _postageFees As String
        Public Property PostageFees() As String
            Get
                Return _postageFees
            End Get
            Set(ByVal value As String)
                _postageFees = value
            End Set
        End Property

        Private _Price As String
        Public Property Price() As String
            Get
                Return _Price
            End Get
            Set(ByVal value As String)
                _Price = value
            End Set
        End Property

        Private _pricePerPiece As String
        Public Property PricePerPiece() As String
            Get
                Return _pricePerPiece
            End Get
            Set(ByVal value As String)
                _pricePerPiece = value
            End Set
        End Property

        Private _productID As String
        Public Property ProductID() As String
            Get
                Return _productID
            End Get
            Set(ByVal value As String)
                _productID = value
            End Set
        End Property

        Private _Qty As String
        Public Property Qty() As String
            Get
                Return _Qty
            End Get
            Set(ByVal value As String)
                _Qty = value
            End Set
        End Property

        Private _SiteID As String
        Public Property SiteID() As String
            Get
                Return _SiteID
            End Get
            Set(ByVal value As String)
                _SiteID = value
            End Set
        End Property

        Private _Sku As String
        Public Property SKU() As String
            Get
                Return _Sku
            End Get
            Set(ByVal value As String)
                _Sku = value
            End Set
        End Property

        Private _taxablePrice As String
        Public Property TaxablePrice() As String
            Get
                Return _taxablePrice
            End Get
            Set(ByVal value As String)
                _taxablePrice = value
            End Set
        End Property

        Private _productType As String
        Public Property ProductType() As String
            Get
                Return _productType
            End Get
            Set(ByVal value As String)
                _productType = value
            End Set
        End Property

        Private _weight As String
        Public Property Weight() As String
            Get
                Return _weight
            End Get
            Set(ByVal value As String)
                _weight = value
            End Set
        End Property

    End Class



    Public Shared Function AddProduct(cartNode As XmlNode, productCartNode As ProductCartNode) As XmlNode

        'Valid Types:
        'EDDM
        'AddressedList
        'Targeted Emails
        'New Mover Postcard
        'AddressedMail AddOn

        Dim productNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(cartNode, "Product", "")
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "AddedDate", productCartNode.AddedDate)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "BaseProductID", productCartNode.BaseProductID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "DesignFee", productCartNode.DesignFee)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "DistributionId", productCartNode.DistributionID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "FlatRateShipFee", productCartNode.FlatRateShipFee)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "FlatRateShipQty", productCartNode.FlatRateShipQty)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Index", productCartNode.IndexGUID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "IsFlatRateShipping", productCartNode.IsFlatRateShipping)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "JobComments", productCartNode.JobComments)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "JobName", productCartNode.JobName)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Name", productCartNode.ProductName)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "PaperHeight", productCartNode.PaperHeight)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "PaperWidth", productCartNode.PaperWidth)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "PostageFees", productCartNode.PostageFees)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Price", productCartNode.Price)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "PricePerPiece", productCartNode.PricePerPiece)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "ProductID", productCartNode.ProductID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Quantity", productCartNode.Qty)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "SiteId", productCartNode.SiteID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "SKU", productCartNode.SKU)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "TaxablePrice", productCartNode.TaxablePrice)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Type", productCartNode.ProductType)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Weight", productCartNode.Weight)

        cartNode.AppendChild(productNode)

        Return cartNode

    End Function



    Public Shared Function AddProduct(cartNode As XmlNode, addedDate As String, baseProductID As String, designFee As String, distributionID As String, _
                                      flatRateShipFee As String, flatRateShipQTY As String, indexGUID As String, isFlatRateShipping As String, jobComments As String, _
                                      jobName As String, productName As String, paperHeight As String, paperWidth As String, postageFees As String, price As String, _
                                      pricePerPiece As String, productID As String, qty As String, siteID As String, sku As String, taxablePrice As String, productType As String, _
                                      weight As String) As XmlNode

        Dim productNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(cartNode, "Product", "")
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "AddedDate", addedDate)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "BaseProductID", baseProductID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "DesignFee", designFee)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "DistributionId", distributionID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "FlatRateShipFee", flatRateShipFee)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "FlatRateShipQty", flatRateShipQTY)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Index", indexGUID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "IsFlatRateShipping", isFlatRateShipping)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "JobComments", jobComments)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "JobName", jobName)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Name", productName)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "PaperHeight", paperHeight)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "PaperWidth", paperWidth)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "PostageFees", postageFees)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Price", price)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "PricePerPiece", pricePerPiece)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "ProductID", productID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Quantity", qty)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "SiteId", siteID)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "SKU", sku)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "TaxablePrice", taxablePrice)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Type", productType)
        xmlhelp.AddOrUpdateXMLAttribute(productNode, "Weight", weight)

        cartNode.AppendChild(productNode)

        Return cartNode

    End Function



    Public Shared Function AddAttribute(cartNode As XmlNode, productNodeType As String, catName As String, optCatId As String, optionID As String, valueName As String, _
                                        priceMod As String, priceModPercent As String, weightMod As String) As XmlNode

        'Will add attribute under the specified PRODUCT Node.  

        Dim productNode As XmlNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Dim attributeNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(productNode, "Attribute", "")

        xmlhelp.AddOrUpdateXMLAttribute(attributeNode, "Name", catName)
        xmlhelp.AddOrUpdateXMLAttribute(attributeNode, "OptCatId", optCatId)
        xmlhelp.AddOrUpdateXMLAttribute(attributeNode, "Value", optionID)
        xmlhelp.AddOrUpdateXMLAttribute(attributeNode, "ValueName", valueName)
        xmlhelp.AddOrUpdateXMLAttribute(attributeNode, "PriceMod", priceMod)
        xmlhelp.AddOrUpdateXMLAttribute(attributeNode, "PriceModPercent", priceModPercent)
        xmlhelp.AddOrUpdateXMLAttribute(attributeNode, "WeightMod", weightMod)

        productNode.AppendChild(attributeNode)

        Return cartNode


    End Function



    Public Shared Function AddOrderCalc(cartNode As XmlNode, productNodeType As String, salesTax As String, salesTaxRate As String, salesTaxMessage As String, salesTaxState As String, mailPiecesPrice As String, _
                                        extraPiecesPrice As String, financeFirstPayment As String, financePaymentAmount As String, financeTotal As String) As XmlNode

        'Will add an OrderCalc Node under the specified PRODUCT Node.  

        Dim productNode As XmlNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Dim orderCalNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(productNode, "OrderCalc", "")

        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "ExtraPiecesPrice", extraPiecesPrice)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "FinanceFirstPayment", financeFirstPayment)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "FinancePaymentAmount", financePaymentAmount)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "FinanceTotal", financeTotal)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "MailPiecesPrice", mailPiecesPrice)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "SalesTax", salesTax)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "SalesTaxRate", salesTaxRate)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "SalesTaxState", salesTaxRate)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "SalesTaxMessage", salesTaxMessage)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalNode, "Type", productNodeType)

        productNode.AppendChild(orderCalNode)

        Return cartNode


    End Function



    Public Shared Function AddDrops(cartNode As XmlNode, productNodeType As String, returnCompany As String, returnAddress1 As String, _
                                    returnAddress2 As String, returnCity As String, returnState As String, returnZipCode As String, mailingPrinter As String, _
                                    additionalDistribution As String, useResidentialDelivery As String, useBusinessDelivery As String, usePOBoxDelivery As String) As XmlNode


        Dim productNode As XmlNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Dim dropsNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(productNode, "Drops", "")

        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "Type", productNodeType)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "returnCompany", returnCompany)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "returnAddress1", returnAddress1)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "returnAddress2", returnAddress2)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "returnCity", returnCity)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "returnState", returnState)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "returnZip", returnZipCode)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "mailingPrinter", mailingPrinter)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "AdditionalDistribution", additionalDistribution)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "UseResidentialDelivery", useResidentialDelivery)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "UseBusinessDelivery", useBusinessDelivery)
        xmlhelp.AddOrUpdateXMLAttribute(dropsNode, "UsePOBoxDelivery", usePOBoxDelivery)

        productNode.AppendChild(dropsNode)

        Return cartNode


    End Function



    Public Shared Function AddIndividualDrop(cartNode As XmlNode, productNodeType As String, dropNumber As String, dropTotal As String, dropDate As String, isMultiple As Boolean, numOfDrops As String) As XmlNode


        Dim dropsNode As XmlNode = cartNode.SelectSingleNode("//Drops[@Type='" & productNodeType & "']")
        Dim indDropNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(dropsNode, "Drop", "")

        xmlhelp.AddOrUpdateXMLAttribute(indDropNode, "Number", dropNumber)
        xmlhelp.AddOrUpdateXMLAttribute(indDropNode, "Total", dropTotal)
        xmlhelp.AddOrUpdateXMLAttribute(indDropNode, "Date", dropDate)
        xmlhelp.AddOrUpdateXMLAttribute(indDropNode, "Type", productNodeType)

        If (isMultiple) Then

            'Temp disabled to see if these attributes are really needed. Delete if not needed. 6/5/2015
            'xmlhelp.AddOrUpdateXMLAttribute(indDropNode, "Paid", "False")
            'xmlhelp.AddOrUpdateXMLAttribute(indDropNode, "Status", "Pending")

            xmlhelp.AddOrUpdateXMLAttribute(indDropNode, "Multiple", numOfDrops)
        End If

        dropsNode.AppendChild(indDropNode)

        Return cartNode


    End Function



    Public Shared Function AddIndividualArea(cartNode As XmlNode, productNodeType As String, dropNumber As String, areaName As String, friendlyName As String, total As String) As XmlNode

        'To be used with EDDM distributions only.  USelectID 1.

        Dim dropNode As XmlNode = cartNode.SelectSingleNode("//Drop[@Number='" & dropNumber & "'][@Type='" & productNodeType & "']")
        Dim areaNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(dropNode, "Area", "")

        If productNodeType.ToLower() = "eddm" Then
            xmlhelp.AddOrUpdateXMLAttribute(areaNode, "Name", areaName)
            xmlhelp.AddOrUpdateXMLAttribute(areaNode, "FriendlyName", friendlyName)
            xmlhelp.AddOrUpdateXMLAttribute(areaNode, "Total", total)

            dropNode.AppendChild(areaNode)
        End If

        Return cartNode


    End Function



    Public Shared Function AddListData(cartNode As XmlNode, productNodeType As String, dropNumber As String, listKeyID As String, file As String) As XmlNode

        'To be used with AddressedList distributions only.  USelectID 5,6,7.

        Dim dropNode As XmlNode = cartNode.SelectSingleNode("//Drop[@Number='" & dropNumber & "'][@Type='" & productNodeType & "']")
        Dim areaNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(dropNode, "List", "")

        If productNodeType.ToLower() = "addressedlist" Then

            xmlhelp.AddOrUpdateXMLAttribute(areaNode, "ListKeyID", listKeyID)
            xmlhelp.AddOrUpdateXMLAttribute(areaNode, "File", file)

            dropNode.AppendChild(areaNode)

        End If


        Return cartNode


    End Function



    Public Shared Function AddDesign(cartNode As XmlNode, productNodeType As String, designType As String, frontFileExt As String, frontFileName As String, frontRealFileName As String, frontAction As String, _
                                     hasBackDesign As Boolean, backFileExt As String, backFileName As String, backRealFileName As String, backAction As String, _
                                     artKey As String, requiresProof As String) As XmlNode


        Dim productNode As XmlNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Dim designNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(productNode, "Design", "")
        Dim frontNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(designNode, "Front", "")
        Dim backNode As XmlNode = Nothing

        xmlhelp.AddOrUpdateXMLAttribute(designNode, "Type", productNodeType)
        xmlhelp.AddOrUpdateXMLAttribute(designNode, "RequiredProof", requiresProof)

        Select Case designType.ToLower

            Case "my"

                If Not String.IsNullOrEmpty(frontFileExt) Then
                    xmlhelp.AddOrUpdateXMLAttribute(frontNode, "filetype", frontFileExt)
                End If

                If Not String.IsNullOrEmpty(frontFileName) Then
                    xmlhelp.AddOrUpdateXMLAttribute(frontNode, "filename", frontFileName)
                End If

                If Not String.IsNullOrEmpty(frontRealFileName) Then
                    xmlhelp.AddOrUpdateXMLAttribute(frontNode, "realfilename", frontRealFileName)
                End If

                xmlhelp.AddOrUpdateXMLAttribute(frontNode, "DesignSelectionType", frontAction)


                If (hasBackDesign) Then

                    backNode = xmlhelp.AddOrUpdateXMLNode(designNode, "Back", "")

                    If Not String.IsNullOrEmpty(backFileExt) Then
                        xmlhelp.AddOrUpdateXMLAttribute(backNode, "filetype", backFileExt)
                    End If

                    If Not String.IsNullOrEmpty(backFileName) Then
                        xmlhelp.AddOrUpdateXMLAttribute(backNode, "filename", backFileName)
                    End If

                    If Not String.IsNullOrEmpty(backRealFileName) Then
                        xmlhelp.AddOrUpdateXMLAttribute(backNode, "realfilename", backRealFileName)
                    End If

                    xmlhelp.AddOrUpdateXMLAttribute(backNode, "DesignSelectionType", backAction)

                End If

            Case "template"
                xmlhelp.AddOrUpdateXMLAttribute(frontNode, "DesignSelectionType", "Template")
                xmlhelp.AddOrUpdateXMLAttribute(frontNode, "ArtKey", artKey)

            Case "pro"
                xmlhelp.AddOrUpdateXMLAttribute(frontNode, "DesignSelectionType", "Professional Design")

            Case "professional design"
                xmlhelp.AddOrUpdateXMLAttribute(frontNode, "DesignSelectionType", "Professional Design")

            Case "omitted"
                xmlhelp.AddOrUpdateXMLAttribute(frontNode, "DesignSelectionType", "Omitted")

            Case "upload"
                xmlhelp.AddOrUpdateXMLAttribute(frontNode, "DesignSelectionType", "Upload")

        End Select


        productNode.AppendChild(designNode)

        Return cartNode


    End Function



    Public Shared Function AddShipments(cartNode As XmlNode, productNodeType As String, deliveryAddressID As String, extraCopies As String, weight As String, pageCount As String, _
                                        paperWidth As String, paperHeight As String, shipPrice As String, zipCode As String, mailQTY As Integer) As XmlNode

        'By default, ONE shipment node will be added - for the standard campaign.
        'IF Extra copies is detected, it will added a second shipment node.
        '<shipments>
        '   <shipment....etc />
        '<shipments>

        Dim productNode As XmlNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Dim shipmentsNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(productNode, "shipments", "")
        Dim shipmentNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(shipmentsNode, "shipment", "")
        Dim extraCopiesShipmentNode As XmlNode = Nothing

        xmlhelp.AddOrUpdateXMLAttribute(shipmentsNode, "Type", productNodeType)

        'Optionally added
        If (extraCopies > 0) Then

            extraCopiesShipmentNode = xmlhelp.AddOrUpdateXMLNode(shipmentsNode, "shipment", "")

            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "Index", System.Guid.NewGuid.ToString)
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "AddressID", deliveryAddressID)
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "NonCommercialAddress", "0")
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "Qty", extraCopies.ToString())
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "Weight", (weight * pageCount).ToString())
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "Width", paperWidth)
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "Height", paperHeight)
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "Length", ((Integer.Parse(extraCopies) / 1000) * 3).ToString())
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "Price", shipPrice)
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "pricehash", extraCopies & "-" & zipCode)
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "SalesTax", "0")
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "SalesTaxJurisdiction", "")
            xmlhelp.AddOrUpdateXMLAttribute(extraCopiesShipmentNode, "Type", "ExtraCopies")

        End If


        'Will ALWAYS be added
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "Index", System.Guid.NewGuid.ToString)
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "AddressID", "0")
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "NonCommercialAddress", "0")
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "Qty", mailQTY)
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "Weight", (weight * pageCount).ToString())
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "Width", paperWidth)
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "Height", paperHeight)
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "Length", ((mailQTY / 1000) * 3).ToString())
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "QuoteItem", "1")
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "Price", "0")
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "State", "")
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "SalesTax", "0")
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "SalesTaxRate", "0")
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "SalesTaxJurisdiction", "")
        xmlhelp.AddOrUpdateXMLAttribute(shipmentNode, "Type", productNodeType & " Campaign")


        productNode.AppendChild(shipmentsNode)

        Return cartNode


    End Function



    Public Shared Function AddTmcData(cartNode As XmlNode, EddmTotalSelected As String, EddmTotalMailed As String, EddmBaseProductID As String, EddmPPP As String, AddressedTotalSelected As String, _
                            AddressedTotalMailed As String, AddressedBaseProductID As String, AddressedPPP As String) As XmlNode

        Dim tmcDataNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(cartNode, "TMCData", "")

        xmlhelp.AddOrUpdateXMLAttribute(tmcDataNode, "EddmTotalSelected", EddmTotalSelected)
        xmlhelp.AddOrUpdateXMLAttribute(tmcDataNode, "EddmTotalMailed", EddmTotalMailed)
        xmlhelp.AddOrUpdateXMLAttribute(tmcDataNode, "EddmBaseProductID", EddmBaseProductID)
        xmlhelp.AddOrUpdateXMLAttribute(tmcDataNode, "EddmPPP", EddmPPP)
        xmlhelp.AddOrUpdateXMLAttribute(tmcDataNode, "AddressedTotalSelected", AddressedTotalSelected)
        xmlhelp.AddOrUpdateXMLAttribute(tmcDataNode, "AddressedTotalMailed", AddressedTotalMailed)
        xmlhelp.AddOrUpdateXMLAttribute(tmcDataNode, "AddressedBaseProductID", AddressedBaseProductID)
        xmlhelp.AddOrUpdateXMLAttribute(tmcDataNode, "AddressedPPP", AddressedPPP)

        cartNode.AppendChild(tmcDataNode)

        Return cartNode

    End Function



    Public Shared Function AddCoupon(cartNode As XmlNode, couponCode As String, discountAmt As String) As XmlNode

        Dim couponNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(cartNode, "Coupon", "")

        xmlhelp.AddOrUpdateXMLAttribute(couponNode, "Name", "Coupon")
        xmlhelp.AddOrUpdateXMLAttribute(couponNode, "CouponCode", couponCode)
        xmlhelp.AddOrUpdateXMLAttribute(couponNode, "DiscountAmount", discountAmt)

        cartNode.AppendChild(couponNode)

        Return cartNode

    End Function



    'Obsolete - DELETE after 5/1/2016
    'Public Shared Function AddScheduleToCart(scheduleDataView As DataView, cartNode As XmlNode, productNodeType As String, distributionID As Integer) As XmlNode

    '<cart>
    '   <product ..>....etc....
    '   </product>
    '   <schedule>
    '       <task>
    '           <date>2/15/2015</date>
    '           <quantity>1000</quantity>
    '           <type>EDDM 11x17...</type>
    '           <routes>...long honkin' string...</routes>
    '       <task>
    '       <task>
    '           <date>2/20/15</date>
    '....etc


    'Dim productNode As XmlNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
    'Dim scheduleNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(productNode, "Schedule", "")

    ''Temp atttributes
    'xmlhelp.AddOrUpdateXMLAttribute(scheduleNode, "DistributionId", distributionID.ToString())
    'xmlhelp.AddOrUpdateXMLAttribute(scheduleNode, "Time", DateTime.Now.Ticks)

    'For Each rowView As DataRowView In scheduleDataView
    '    Dim taskNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(scheduleNode, "Task", "")
    '    Dim dateNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(taskNode, "Date", rowView("StartDate").ToShortDateString())
    '    Dim qtyNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(taskNode, "Quantity", rowView("Quantity").ToString())
    '    Dim prodNameNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(taskNode, "Type", rowView("Type").ToString())
    '    Dim routesNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(taskNode, "Routes", rowView("Routes").ToString())
    '    scheduleNode.AppendChild(taskNode)
    'Next

    'productNode.AppendChild(scheduleNode)

    'Return cartNode

    'End Function


    'Experimental



    Public Shared Function AddScheduleToCart(scheduleDataView As DataView, cartNode As XmlNode, distributionID As Integer) As XmlNode

        '<cart>
        '   <product ..>....etc....
        '   </product>
        '   <schedule>
        '       <task>
        '           <date>2/15/2015</date>
        '           <quantity>1000</quantity>
        '           <type>EDDM 11x17...</type>
        '           <routes>...long honkin' string...</routes>
        '       <task>
        '       <task>
        '           <date>2/20/15</date>
        '....etc


        Dim scheduleNodeList As XmlNodeList = cartNode.SelectNodes("//Schedule")

        If (scheduleNodeList.Count <= 0) Then

            Dim scheduleNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(cartNode, "Schedule", "")
            xmlhelp.AddOrUpdateXMLAttribute(scheduleNode, "DistributionId", distributionID.ToString())
            xmlhelp.AddOrUpdateXMLAttribute(scheduleNode, "Time", DateTime.Now.Ticks)

            For Each rowView As DataRowView In scheduleDataView
                Dim taskNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(scheduleNode, "Task", "")
                Dim dateNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(taskNode, "Date", rowView("StartDate").ToShortDateString())
                Dim qtyNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(taskNode, "Quantity", rowView("Quantity").ToString())
                Dim prodNameNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(taskNode, "Type", rowView("Type").ToString())
                Dim routesNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(taskNode, "Routes", rowView("Routes").ToString())
                scheduleNode.AppendChild(taskNode)
            Next

            cartNode.AppendChild(scheduleNode)

        End If

        Return cartNode

    End Function



    Public Shared Function AddScheduleToCart2(scheduleDataView As DataView, oXML As XmlDocument, productNodeType As String) As XmlDocument

        'This function is exerimental and not in use.
        'Look into deleting after 3/1/2016.

        '<cart>
        '   <product ..>....etc....
        '   </product>
        '   <schedule>
        '       <task>
        '           <date>2/15/2015</date>
        '           <quantity>1000</quantity>
        '           <type>EDDM 11x17...</type>
        '           <routes>...long honkin' string...</routes>
        '       <task>
        '       <task>
        '           <date>2/20/15</date>
        '....etc

        Dim revisedXmlDoc As XmlDocument = New XmlDocument()
        Dim currentXML As String = oXML.OuterXml


        revisedXmlDoc.LoadXml(currentXML)

        'Pick the correct ProductNode
        Dim productNode As XmlNode = revisedXmlDoc.SelectSingleNode("//Product[@Type='" & productNodeType & "']")


        ' Create a new element node.
        Dim scheduleNode As XmlNode = revisedXmlDoc.CreateNode("element", "Schedule", "")

        For Each rowView As DataRowView In scheduleDataView
            Dim row As DataRow = rowView.Row
            Dim taskNode As XmlNode = revisedXmlDoc.CreateNode("element", "Task", "")
            Dim dateNode As XmlNode = revisedXmlDoc.CreateNode("element", "Date", "")
            Dim typeNode As XmlNode = revisedXmlDoc.CreateNode("element", "Type", "")
            Dim qtyNode As XmlNode = revisedXmlDoc.CreateNode("element", "Quantity", "")
            Dim routesNode As XmlNode = revisedXmlDoc.CreateNode("element", "Routes", "")

            dateNode.InnerText = row("StartDate").ToShortDateString()
            qtyNode.InnerText = row("Quantity").ToString()
            typeNode.InnerText = row("Type").ToString()
            routesNode.InnerText = row("Routes").ToString()

            'Add to Task node
            taskNode.AppendChild(dateNode)
            taskNode.AppendChild(typeNode)
            taskNode.AppendChild(qtyNode)
            taskNode.AppendChild(routesNode)

            'Add Tasks to Schedule
            scheduleNode.AppendChild(taskNode)
        Next


        Dim root As XmlElement = revisedXmlDoc.DocumentElement
        root.AppendChild(scheduleNode)

        Return revisedXmlDoc


    End Function




    'UPDATE DATA
    Public Shared Function UpdateDropCount(cartNode As XmlNode, productNodeType As String, dropNumber As String, total As String) As XmlNode

        'Drill down to the correct node
        Dim dropNode As XmlNode = cartNode.SelectSingleNode("//Drop[@Number='" & dropNumber & "'][@Type='" & productNodeType & "']")
        xmlhelp.AddOrUpdateXMLAttribute(dropNode, "Total", total)

        Return cartNode

    End Function



    Public Shared Function UpdatedSalesTax(cartNode As XmlNode) As XmlNode

        Return cartNode

    End Function



    Public Shared Function UpdateOrderCalc(cartNode As XmlNode, productNodeType As String, salesTax As String, salesTaxRate As String, salesTaxState As String, salesTaxMessage As String, _
                                           mailPiecePerPrice As String, extraPiecesPrice As String, financeFirstPayment As String, financePaymentAmt As String, _
                                           financeTotal As String, dropFee As String) As XmlNode

        Dim orderCalcNode As XmlNode = cartNode.SelectSingleNode("//OrderCalc[@Type='" & productNodeType & "']")

        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "SalesTax", salesTax)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "SalesTaxRate", salesTaxRate)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "SalesTaxState", salesTaxRate)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "SalesTaxMessage", salesTaxMessage)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "MailPiecesPrice", mailPiecePerPrice)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "ExtraPiecesPrice", extraPiecesPrice)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "FinanceFirstPayment", financeFirstPayment)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "FinancePaymentAmount", financePaymentAmt)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "FinanceTotal", financeTotal)
        xmlhelp.AddOrUpdateXMLAttribute(orderCalcNode, "DropFee", dropFee)


        Return cartNode

    End Function





    'GET DATA
    Public Shared Function GetDistributionID(cartNode As XmlNode) As Integer

        'This assumes there will be atleast one Product node.  There should be anyways...
        Dim results As Integer = 0
        Dim firstProd As XmlNode

        firstProd = cartNode.SelectSingleNode("//Product")
        Integer.TryParse(xmlhelp.ReadAttribute(firstProd, "DistributionId"), results)

        Return results

    End Function



    Public Shared Function GetNumberOfImpressions(cartNode As XmlNode, campaignType As String) As Integer

        'The individual drop nodes **can** contain an attribute called Multiple which contains the number of impressions.
        'IF this exists then it will return that number.  Otherwise, it will return 1 indicating a single impression campaign.

        'Two campaign types: EDDM and AddressedList.  TMC/Blended will contain drop nodes of BOTH types. These params ARE case sensitive.

        Dim results As Integer = 1
        Dim dropNode As XmlNode

        dropNode = cartNode.SelectSingleNode("//Drop[@Number='1'][@Type='" & campaignType & "']")
        Integer.TryParse(xmlhelp.ReadAttribute(dropNode, "Multiple"), results)

        If results < 1 Then
            Return 1
        Else
            Return results
        End If

    End Function



    Public Shared Function GetProductID(cartNode As XmlNode, productNodeType As String) As Integer

        Dim results As Integer = 1
        Dim productNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Integer.TryParse(xmlhelp.ReadAttribute(productNode, "ProductID"), results)

        Return results

    End Function



    Public Shared Function GetBaseProductID(cartNode As XmlNode, productNodeType As String) As Integer

        Dim results As Integer = 1
        Dim productNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Integer.TryParse(xmlhelp.ReadAttribute(productNode, "BaseProductID"), results)

        Return results

    End Function



    Public Shared Function GetTotalSelected(cartNode As XmlNode, productNodeType As String, numImpressions As Integer) As Integer

        'MarketingServices is the only page to use this Function so far.
        'If Number of Impressions is more than one then look at first drop. 
        'By default, it will simply grab the first Drop Node regardless of how many impressions there are. There should always be atleast one drop.

        'If it a Single Impression, then we will look in a different location in the cart.

        Dim results As Integer = 1

        If numImpressions > 1 Then
            Dim dropNode As XmlNode
            dropNode = cartNode.SelectSingleNode("//Drop[@Number='1'][@Type='" & productNodeType & "']")
            Integer.TryParse(xmlhelp.ReadAttribute(dropNode, "Total"), results)
        Else
            Dim productNode As XmlNode
            productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
            Integer.TryParse(xmlhelp.ReadAttribute(productNode, "Quantity"), results)
        End If



        Return results

    End Function



    Public Shared Function GetPrice(cartNode As XmlNode, productNodeType As String) As Decimal

        'Will return the "Price" which was previously calculated.  (TotalSelected x Num Impressions) x Price Per Piece.
        ' Drop Fees are included in this price as per original design.

        Dim results As Decimal = 1
        Dim dropNode As XmlNode

        dropNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Decimal.TryParse(xmlhelp.ReadAttribute(dropNode, "Price"), results)

        Return results

    End Function



    Public Shared Function GetDesignFee(cartNode As XmlNode, productNodeType As String) As Decimal

        Dim productNode As XmlNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        Dim designFee As Integer = 0

        Integer.TryParse(xmlhelp.ReadAttribute(productNode, "DesignFee"), designFee)

        Return designFee

    End Function



    Public Shared Function GetDropFee(cartNode As XmlNode) As Decimal

        'Gets DropFee value from OrderCalc Node.
        Dim calcNode As XmlNode = cartNode.SelectSingleNode("//OrderCalc")
        Dim dropFee As Integer = 0

        Decimal.TryParse(xmlhelp.ReadAttribute(calcNode, "DropFee"), dropFee)

        Return dropFee

    End Function



    Public Shared Function GetNewMoverPrice(cartNode As XmlNode) As Decimal

        Dim newMoverPrice As Decimal = 0
        Dim newMoverNode As XmlNode = cartNode.SelectSingleNode("//Product[@Type='New Mover Postcard']")

        If (xmlhelp.ReadAttribute(newMoverNode, "Price") IsNot Nothing) Then
            Decimal.TryParse(xmlhelp.ReadAttribute(newMoverNode, "Price"), newMoverPrice)
        End If

        Return newMoverPrice

    End Function



    Public Shared Function GetProductName(cartNode As XmlNode, productNodeType As String) As String

        Dim results As String = "unknown"
        Dim productNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        results = xmlhelp.ReadAttribute(productNode, "Name")

        Return results

    End Function



    Public Shared Function GetProdOptionsList(cartNode As XmlNode, productNodeType As String) As XmlNodeList

        Dim productNode As XmlNode
        Dim attributesList As XmlNodeList

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        attributesList = productNode.SelectNodes("//Attribute[@OptCatId != 0]")

        Return attributesList

    End Function



    Public Shared Function GetProductIndex(cartNode As XmlNode, productNodeType As String) As String

        Dim results As String = "unknown"
        Dim productNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        results = xmlhelp.ReadAttribute(productNode, "Index")

        Return results

    End Function



    Public Shared Function GetJobName(cartNode As XmlNode, productNodeType As String) As String

        Dim results As String = ""
        Dim productNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        results = xmlhelp.ReadAttribute(productNode, "JobName")

        Return results

    End Function



    Public Shared Function GetJobComments(cartNode As XmlNode, productNodeType As String) As String

        Dim results As String = ""
        Dim productNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        results = xmlhelp.ReadAttribute(productNode, "JobComments")

        Return results

    End Function



    Public Shared Function GetExtraPiecesQty(cartNode As XmlNode, productNodeType As String) As Integer

        Dim results As Integer = 0
        Dim shipmentsNode As XmlNode
        Dim shipmentNode As XmlNode                                                         'Single shipment node

        shipmentsNode = cartNode.SelectSingleNode("//shipments[@Type='" & productNodeType & "']")
        shipmentNode = shipmentsNode.SelectSingleNode("//shipment[@AddressID!='0']")        'Get anything that does NOT shave an ID of 0.

        Integer.TryParse(xmlhelp.ReadAttribute(shipmentNode, "Qty"), results)

        Return results

    End Function



    Public Shared Function GetNumberOfDrops(cartNode As XmlNode, campaignType As String) As Integer

        'For SINGLE Impression campaigns.
        'Two campaign types: EDDM and AddressedList.  TMC/Blended will contain drop nodes of BOTH types. These params ARE case sensitive.

        'Create an XmlNodeList which meet single impression criteria - does NOT contain 'multiple'
        'Count list items

        Dim results As Integer = 0
        Dim dropsList As XmlNodeList
        Dim dropNode As XmlNode


        dropNode = cartNode.SelectSingleNode("//Drops[@Type='" & campaignType & "']")
        dropsList = dropNode.SelectNodes("//Drop")

        For Each drop As XmlNode In dropsList

            If drop.Attributes.ItemOf("Multiple") Is Nothing Then
                results = results + 1
            End If

        Next

        Return results

    End Function



    Public Shared Function GetPricePerPiece(cartNode As XmlNode, productNodeType As String) As Decimal

        Dim results As Decimal = 0
        Dim productNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")

        Decimal.TryParse(xmlhelp.ReadAttribute(productNode, "PricePerPiece"), results)

        Return results

    End Function



    Public Shared Function GetShipToAddress(cartNode As XmlNode, productNodeType As String) As String

        Dim results As String = ""
        Dim dropsNode As XmlNode

        dropsNode = cartNode.SelectSingleNode("//Drops[@Type='" & productNodeType & "']")

        results = xmlhelp.ReadAttribute(dropsNode, "returnAddress1") & "<br />"

        If (xmlhelp.ReadAttribute(dropsNode, "returnAddress2")).Length > 0 Then
            results = results + xmlhelp.ReadAttribute(dropsNode, "returnAddress2") & "<br />"
        End If

        results = results + xmlhelp.ReadAttribute(dropsNode, "returnCity") & ", " & xmlhelp.ReadAttribute(dropsNode, "returnState") & " " & xmlhelp.ReadAttribute(dropsNode, "returnZip")

        Return results

    End Function



    Public Shared Function GetDesignType(cartNode As XmlNode, productNodeType As String) As String

        'This function will look in the cart and get the Design Type from the indicated product type (EDDM or AddressedList) and return it.
        'For the moment, this is only used on MarketingService with the New Mover add-on service.  The cart
        'will indicate which type was selected instead of hard coding 'template' which is what it previously did.

        Dim results As String = ""
        Dim productNode As XmlNode
        Dim designNode As XmlNode
        Dim frontNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        designNode = productNode.SelectSingleNode("//Design")
        frontNode = designNode.SelectSingleNode("//Front")

        results = xmlhelp.ReadAttribute(frontNode, "DesignSelectionType")

        Return results

    End Function



    Public Shared Function GetZipCode(cartNode As XmlNode, productNodeType As String) As String

        'This function will look in the cart and get the Zip Code from the indicated product type (EDDM or AddressedList) and return it.

        Dim results As String = ""
        Dim productNode As XmlNode
        Dim dropsNode As XmlNode

        productNode = cartNode.SelectSingleNode("//Product[@Type='" & productNodeType & "']")
        dropsNode = productNode.SelectSingleNode("//Drops")

        results = xmlhelp.ReadAttribute(dropsNode, "returnZip")

        Return results

    End Function



    Public Shared Function GetAddressedDropTotal(cartNode As XmlNode, isMultiple As Boolean, numDrops As Integer, mailPieces As Integer, productNodeType As String) As Integer

        'This function needs to determine whether this is a single impression with a single drop, 
        'a singe impression with multple drops, or multiple impressions.

        Dim dropTotal As Integer = 0

        'Single Impression. Number of drops not relevant. Send mail pcs right back. Brilliant.
        If Not (isMultiple) Then
            dropTotal = mailPieces

            'Look for total in first drop of all the drops.  Should be the same value for each drop
        Else

            Dim dropNode As XmlNode = cartNode.SelectSingleNode("//Drop[@Number='1'][@Type='" & productNodeType & "']")
            dropNode = cartNode.SelectSingleNode("//Drop[@Number='1'][@Type='" & productNodeType & "']")

            Integer.TryParse(xmlhelp.ReadAttribute(dropNode, "Total"), dropTotal)

        End If


        Return dropTotal


    End Function



    Public Shared Function GetDropDatesList(cartNode As XmlNode, productNodeType As String) As String

        Dim dropDatesString As String = ""
        Dim dropNode As XmlNode = cartNode.SelectSingleNode("//Drops[@Type='" & productNodeType & "']")
        Dim dropsList As XmlNodeList = dropNode.SelectNodes("//Drop")

        For Each drop As XmlNode In dropsList
            dropDatesString = dropDatesString & xmlhelp.ReadAttribute(drop, "Date").ToString() & "<br />"
        Next

        Return dropDatesString

    End Function



    Public Shared Function GetDropQtyList(cartNode As XmlNode, productNodeType As String) As String

        'Used for AddressedList displays

        Dim dropQTYString As String = ""
        Dim dropNode As XmlNode = cartNode.SelectSingleNode("//Drops[@Type='" & productNodeType & "']")
        Dim dropsList As XmlNodeList = dropNode.SelectNodes("//Drop")

        For Each drop As XmlNode In dropsList
            dropQTYString = dropQTYString & Convert.ToInt32(xmlhelp.ReadAttribute(drop, "Total").ToString()).ToString("N0") & "<br />"
        Next

        Return dropQTYString

    End Function



End Class
