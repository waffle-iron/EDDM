Imports System.Xml
Imports System.IO
Imports System.Linq
Imports System.Collections.Generic
Imports Taradel.EF
Imports log4net
Imports Taradel
Imports System.Xml.Linq
Imports System.Net.Mail
Imports System.Text
Imports WebSupergoo.ABCpdf8
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Net

Partial Class Step3_Checkout
    Inherits appxCMS.PageBase


    '=================================================================================================================================
    'NOTES:
    '   There is loose, business logic being defined as-needed regarding email addresses: who gets the receipt, should we show the receipt
    '   email address field, should we show the billing email address field, who should get the actual reeipt, should we copy the account
    '   holder and so on.  There is no strongly defined logic yet however, when setting up a site only ONE boolean should be set to true
    '   in the pnd_SiteDetails table: ShowBillingEmail or ShowReceiptEmail or ShowBillingEmail.  Having them both set to true or false
    '   could lead to unpredicatble results.
    '================================================================================================================================



#Region "Fields"


    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected oCust As Taradel.Customer = Nothing

    '-- These will be used for Finance Checkout
    Protected NumberOfPayments As Integer = 0
    Protected FirstPaymentDate As DateTime
    Protected oAmounts As Taradel.OrderAmounts = Nothing
    Protected oFinanceAmounts As Taradel.OrderAmounts = Nothing
    Protected oQuote As Taradel.ProductPriceQuote = Nothing
    Protected oBaseQuote As Taradel.ProductPriceQuote = Nothing
    Protected oCoupon As Taradel.WLCouponValidator = Nothing
    Protected CouponCode As String = ""


    Protected xTaxAmount As Decimal = 0
    Protected xTaxRate As Decimal = 0
    Protected xTaxJurisdiction As String = ""
    Protected xCoupon As Taradel.Coupon = Nothing
    Protected xCouponDiscount As Taradel.Marketing.CouponDiscount = Nothing

    Protected xOrderCalc As New TaradelReceiptUtility.OrderCalculator()
    Protected SiteDetails As SiteUtility.SiteDetails
    Protected currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()


    'Add on Services
    Protected numEmailBlasts As Integer = 3



#End Region



#Region "Properties"""


    Private _ProductID As Integer = ProductID
    Private Property ProductID() As Integer
        Get
            Return _ProductID
        End Get
        Set(ByVal value As Integer)
            _ProductID = value
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



    Private _environmentMode As String
    Private ReadOnly Property environmentMode() As String

        Get

            'SHOULD return as Dev or Prod
            If ConfigurationManager.AppSettings("Environment") IsNot Nothing Then
                _environmentMode = ConfigurationManager.AppSettings("Environment").ToLower()
            Else
                'Fall back value
                _environmentMode = "dev"
            End If

            Return _environmentMode

        End Get

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



    Private _USelectID As Integer = 0
    Public Property USelectID As Integer
        Get
            Return _USelectID
        End Get
        Set(value As Integer)
            _USelectID = value
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



    Private _EDDMMap As Boolean = False
    Public Property EDDMMap As Boolean
        Get
            Return _EDDMMap
        End Get
        Set(value As Boolean)
            _EDDMMap = value
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



    Private _TMCMap As Boolean = False
    Public Property TMCMap As Boolean
        Get
            Return _TMCMap
        End Get
        Set(value As Boolean)
            _TMCMap = value
        End Set

    End Property



    Private _NewMoverSelected As Boolean = False
    Public Property NewMoverSelected As Boolean
        Get
            Return _NewMoverSelected
        End Get
        Set(value As Boolean)
            _NewMoverSelected = value
        End Set

    End Property



    Private _EmailCampaignSelected As Boolean = False
    Public Property EmailCampaignSelected As Boolean
        Get
            Return _EmailCampaignSelected
        End Get
        Set(value As Boolean)
            _EmailCampaignSelected = value
        End Set

    End Property



    Private _AddressedAddOnCampaignSelected As Boolean = False
    Public Property AddressedAddOnCampaignSelected As Boolean
        Get
            Return _AddressedAddOnCampaignSelected
        End Get
        Set(value As Boolean)
            _AddressedAddOnCampaignSelected = value
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



    Private _UseOwnGateWay As Boolean = False
    Public Property UseOwnGateWay As Boolean

        Get
            Return _UseOwnGateWay
        End Get

        Set(value As Boolean)
            _UseOwnGateWay = value
        End Set

    End Property



    Private _MerchantID As String = ""
    Public Property MerchantID As String

        Get
            Return _MerchantID
        End Get

        Set(value As String)
            _MerchantID = value
        End Set

    End Property



    Private _TransactionID As String = ""
    Public Property TransactionID As String

        Get
            Return _TransactionID
        End Get

        Set(value As String)
            _TransactionID = value
        End Set

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



    Private _IsProfessionalDesign As Boolean = False
    Public Property IsProfessionalDesign As Boolean

        Get
            Return _IsProfessionalDesign
        End Get

        Set(value As Boolean)
            _IsProfessionalDesign = value
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



    Private _HasDropFee As Boolean = False
    Public Property HasDropFee As Boolean

        Get
            Return _HasDropFee
        End Get

        Set(value As Boolean)
            _HasDropFee = value
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



    Private _SalesTax As Decimal = 0
    Public Property SalesTax As Decimal
        Get
            Return _SalesTax
        End Get
        Set(value As Decimal)
            _SalesTax = value
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



    Private _AddressedShipPrice As Decimal = 0
    Public Property AddressedShipPrice As Decimal

        Get
            Return _AddressedShipPrice
        End Get

        Set(value As Decimal)
            _AddressedShipPrice = value
        End Set

    End Property



    Private _CouponDiscount As Decimal = 0
    Public Property CouponDiscount As Decimal

        Get
            Return _CouponDiscount
        End Get

        Set(value As Decimal)
            _CouponDiscount = value
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



    Private _HasActiveCoupons As Boolean = False
    Public Property HasActiveCoupons As Boolean

        Get
            Return _HasActiveCoupons
        End Get

        Set(value As Boolean)
            _HasActiveCoupons = value
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


    Private _ProductType2 As String
    Private Property ProductType2() As String

        Get
            Return _ProductType2
        End Get
        Set(value As String)
            _ProductType2 = value
        End Set

    End Property


#End Region




    'Methods
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init


        If (Profile.Cart Is Nothing) Then

            ShowEmpty()

        Else


            'Helpers
            SetPageProperties()
            ShowHidePageElements()
            BuildBillingSection()
            BuildPaymentSelections()
            BuildOrderSteps()
            BuildPageHeader()



            If (TestMode) Then
                ShowDebug()
            Else
                If Not (String.IsNullOrEmpty(Request.QueryString("debug"))) Then
                    If Request.QueryString("debug") = "hodor" Then
                        ShowDebug()
                    End If
                End If
            End If


            'Build SiteDetails Obj
            SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


            'Fall back on Gateway setting
            If String.IsNullOrEmpty(MerchantID) OrElse String.IsNullOrEmpty(TransactionID) Then
                UseOwnGateWay = False
            End If


            'Set the site gateway information
            If Not UseOwnGateWay Then

                If ConfigurationManager.AppSettings("AuthNet.MerchantId") IsNot Nothing Then
                    MerchantID = ConfigurationManager.AppSettings("AuthNet.MerchantId")
                End If

                If ConfigurationManager.AppSettings("AuthNet.TransactionId") IsNot Nothing Then
                    TransactionID = ConfigurationManager.AppSettings("AuthNet.TransactionId")
                End If

            End If

            'Always pull from SiteDetails
            MerchantID = SiteDetails.Auth_MerchantId
            TransactionID = SiteDetails.Auth_TransactionId

            'Send Drop Down Control some properties
            CCExp.MinYear = Now.Year
            CCExp.MaxYear = Now.Year + 15
            CCExp.ShowDay = False



            'If in Dev Mode, we're going to strip out the customer's actual email address to prevent the 
            'tester from accidentally sending the customer a test receipt.
            If environmentMode.ToLower() = "dev" Then
                txtEmailReceipt.Text = "your_name@taradel.com"
                CCNumber.Text = "4111111111111111"
                CVV2.Text = "111"
                chkTandCAgree.Checked = True
            End If



            If (SiteDetails.RequireStoreNumber) Then
                pnlStoreNumber.Visible = True
            End If


            If EDDMMap Then
                BindEDDMCart()
            End If


            If (UploadedAddressedList) Or (GeneratedAddressedList) Then
                BindAddressedCart()
            End If


            'Load this last
            BuildPaymentPanel()

            ''encryption
            '-- What payment options are enabled for this site/user
            Dim bEnableCC As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Payment", "EnableCreditCard")

            If bEnableCC And Not Request.IsSecureConnection Then
                Response.Redirect("https://" & Request.Url.Host & VirtualPathUtility.ToAbsolute(Page.AppRelativeVirtualPath))
            End If
            ''end encryption


        End If

    End Sub



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            PONumber.Attributes.Add("onchange", "PONumberChanged();")


            'If one of the Add Ons is selected then show the schedule
            If ((EmailCampaignSelected) Or (NewMoverSelected) Or (AddressedAddOnCampaignSelected)) Then

                AddScheduleDataToCart()

                pnlCampaignOverview.Visible = True

                If (EmailCampaignSelected) Then
                    BuildCampaignSchedule("Targeted Emails")        'TODO: switch to productid based
                End If

                If (NewMoverSelected) Then
                    BuildCampaignSchedule("New Mover Postcard")     'TODO: switch to productid based
                End If

                If (AddressedAddOnCampaignSelected) Then
                    BuildCampaignSchedule("Addressed Add Ons")      'TODO: switch to productid based
                End If

            End If


        End If

    End Sub



    Protected Sub SetPageProperties()

        'Read the Cart and get a DistributionID. Since we don't know if the Cart will have a EDDM Product and/or an AddressedList Product, 
        'just grab the first one it finds.  There should always be atleast one product.  Both will have the same DistributionID.  Get the Type value too.
        Dim productNodeFilter As String = ""
        Dim oCart As XmlDocument = Profile.Cart
        Dim firstProd As XmlNode = oCart.SelectSingleNode("//Product")
        Dim productType As String


        'Load the customer data (for entire page)
        oCust = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name)



        'Read DistributionID and set property.
        Integer.TryParse(xmlhelp.ReadAttribute(firstProd, "DistributionId"), DistributionId)


        'Figure out what TYPE of product we have.
        productType = xmlhelp.ReadAttribute(firstProd, "Type")


        'SiteID
        SiteID = appxCMS.Util.CMSSettings.GetSiteId


        'Build SiteDetails Obj
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)
        TestMode = SiteDetails.TestMode



        'Determine the USelectID. This will help tell the whole page how to behave.
        Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistributions.GetDistribution(DistributionId)
        USelectID = oDist.USelectMethodReference.ForeignKey()


        'Determine the DistributionType and set flags
        Select Case USelectID
            Case 1
                EDDMMap = True
                productNodeFilter = "EDDM"
                ProductType2 = "eddm"
            Case 5
                UploadedAddressedList = True
                productNodeFilter = "AddressedList"
                ProductType2 = "addressed"
            Case 6
                GeneratedAddressedList = True
                productNodeFilter = "AddressedList"
                ProductType2 = "addressed"
            Case 7
                TMCMap = True
                productNodeFilter = "AddressedList"
                ProductType2 = ""
        End Select


        'Now get the PROPER ProductID and BaseProductID
        Dim oProd As XmlNode
        oProd = oCart.SelectSingleNode("//Product[@Type='" & productNodeFilter & "']")


        'Set ProductID and BaseProductID
        ProductID = Integer.Parse(xmlhelp.ReadAttribute(oProd, "ProductID"))
        BaseProductID = Integer.Parse(xmlhelp.ReadAttribute(oProd, "BaseProductID"))


        'Has any coupons?
        HasActiveCoupons = Taradel.WLCouponDataSource.HasActiveCoupons()



        'Set Postage Rate. This needs to be improved. 12/22/2015. DSF
        If (EDDMMap) Then
            PostageRate = appxCMS.Util.AppSettings.GetDecimal("USPSPostageRate")

            If PostageRate = 0 Then
                PostageRate = 0.16
            End If
        Else
            PostageRate = 0.3
        End If



        'Set other properties
        UseOwnGateWay = appxCMS.Util.CMSSettings.GetBoolean("Payment", "UseOwnGatewayAccount")
        MerchantID = appxCMS.Util.CMSSettings.GetSetting("Payment", "MerchantId")
        TransactionID = appxCMS.Util.CMSSettings.GetSetting("Payment", "TransactionId")


        'Number of Drops
        Dim oNumDrops As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Number of Drops']")
        If oNumDrops IsNot Nothing Then
            NumOfDrops = Integer.Parse(xmlhelp.ReadAttribute(oNumDrops, "Value"))
        Else
            'Count the number of drops and add this in
            Dim oTotalDrops As XmlNodeList = oProd.SelectNodes("//Drop")
            NumOfDrops = oTotalDrops.Count
        End If


        'Dertmine NewMoverSelected
        Dim campaignScheduleObj As New CampaignScheduler()
        If (campaignScheduleObj.CheckForNewMovers(oCart)) = True Then
            NewMoverSelected = True
        End If


        'Determine EmailCampaign Selected
        If (campaignScheduleObj.CheckForEmails(oCart)) = True Then
            EmailCampaignSelected = True
        End If


        'Determine Addressed AddOns Selected
        If (campaignScheduleObj.CheckForAddressedAddOns(oCart)) = True Then
            AddressedAddOnCampaignSelected = True
        End If


        'Extra Copies
        Me.xOrderCalc = GetNewOrderCalcObj()
        ExtraCopies = xOrderCalc.ExtraPieces


        'Professional Design Fee
        If (CartUtility.GetDesignFee(oCart, productType) > 0) Then
            IsProfessionalDesign = True
            DesignFee = CartUtility.GetDesignFee(oCart, productType)
        End If


        'Drop Fees
        If Me.xOrderCalc.DropFee > 1 Then
            HasDropFee = True
            DropFee = xOrderCalc.DropFee
        End If


        'Sales Tax
        If Me.xOrderCalc.SalesTax > 0 Then
            SalesTax = xOrderCalc.SalesTax
        End If


        'Discount
        If Me.xOrderCalc.CouponDiscount > 0 Then
            CouponDiscount = xOrderCalc.CouponDiscount
        End If



        'Clean Up
        xOrderCalc = Nothing


    End Sub



    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        'jqueryHelper.IncludePlugin(Page, "jQuery-BlockUI", "~/scripts/jquery.blockUI.js")

        Dim oJs As New StringBuilder

        oJs.AppendLine("function TandCAgree(sender, args) {")
        oJs.AppendLine("    var tc = jQuery('#" & chkTandCAgree.ClientID & "');")
        oJs.AppendLine("    if (tc.is(':checked')) {")
        oJs.AppendLine("        args.IsValid = true;")
        oJs.AppendLine("    } else {")
        oJs.AppendLine("        args.IsValid = false;")
        oJs.AppendLine("    }")
        oJs.AppendLine("}")

        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    function selPayment(oSel) {")
        oJs.AppendLine("        var sVal = oSel.val();")
        oJs.AppendLine("        $('div[data-paytype]').hide();")
        oJs.AppendLine("        $('div[data-paytype=' + sVal + ']').show();")
        oJs.AppendLine("    }")

        oJs.AppendLine("    $('#" & radPaymentType.ClientID & " input[type=radio]').change(function() {")
        oJs.AppendLine("        selPayment($(this));")
        oJs.AppendLine("    });")

        oJs.AppendLine("    $('#whatIsThis').dialog({")
        oJs.AppendLine("        autoOpen:false,")
        oJs.AppendLine("        width:750,")
        oJs.AppendLine("        height:500")
        oJs.AppendLine("    });")

        oJs.AppendLine("    $('a[data-action=infowindow]').click(function(e) {")
        oJs.AppendLine("        e.preventDefault();")
        oJs.AppendLine("        $('#wtContent').html($('#wtLoader').html());")
        oJs.AppendLine("        $('#whatIsThis').dialog('open');")

        oJs.AppendLine("        var optcatid = $(this).attr('data-optcatid');")
        oJs.AppendLine("        var pageref = $(this).attr('data-helpfile');")
        oJs.AppendLine("        $('#wtContent').load('/Resources/ProductConfigHelp.ashx?pid=" & ProductID & "&bpid=" & BaseProductID & "&catid=' + optcatid + '&pageref=' + escape(pageref));")
        oJs.AppendLine("    });")

        oJs.AppendLine("    var oSelItem = $('#" & radPaymentType.ClientID & " input[type=radio][checked=checked]');")
        oJs.AppendLine("    selPayment(oSelItem);")

        oJs.AppendLine("});")

        appxCMS.Util.jQuery.RegisterClientScript(Page, "Step3CheckoutInit", oJs.ToString)

    End Sub



    Protected Sub rfvTandCAgree_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles rfvTandCAgree.ServerValidate
        If chkTandCAgree.Checked Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub



    'Needs clean up and revising.
    Protected Sub lnkCheckout_Click(sender As Object, e As System.EventArgs) Handles lnkCheckout.Click

        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)
        Dim logger As New LogWriter()

        Page.Validate("vgCheckout")
        Page.Validate("vg" & radPaymentType.SelectedValue & "Pay")


        If (Me.xOrderCalc Is Nothing) Then
            Me.xOrderCalc = GetNewOrderCalcObj()
        End If


        If Page.IsValid Then

            'Warning - the cart could have multiple Product Nodes.
            Dim oProd As XmlNode = Profile.Cart.SelectSingleNode("//Product[@SiteId=" & appxCMS.Util.CMSSettings.GetSiteId & "]")

            'add the comments 3/24/2015
            xmlhelp.AddOrUpdateXMLAttribute(oProd, "JobComments", txtJobComments.Text)

            'add the secondary email if it existstxtEmailReceipt
            If txtEmailReceipt.Enabled Then
                If txtEmailReceipt.Text.Length > 0 Then
                    xmlhelp.AddOrUpdateXMLAttribute(oProd, "ExtraEmail", txtEmailReceipt.Text)
                End If
            End If


            Dim sCCOrderRef As String = System.Guid.NewGuid.ToString()

            '-- The white label markup amount is any difference between the Taradel price and the instance price
            Dim dWLMarkup As Decimal = Me.xOrderCalc.WLMarkup 'oQuote.BasePrice - oBaseQuote.BasePrice

            '-- OrderAmount includes shipping, postage, taxes, design fees, etc. If these are collected, they also need to go back to Taradel
            Dim dOrderAmt As Decimal = Me.xOrderCalc.OrderTotal 'oBaseQuote.Price + oBaseQuote.DesignPrice + oBaseQuote.ShipPrice + oAmounts.SalesTax '-- This is the Taradel price

            If (Me.xOrderCalc.CouponDiscount > 0) Then
                dOrderAmt = dOrderAmt - Me.xOrderCalc.CouponDiscount ' new 3/11/2015 - violates standards
            End If


            If radPaymentType.SelectedValue = "CC" Then

                Dim camSch As New CampaignScheduler()
                'Response.Write(camSch.CheckForNewMovers(Profile.Cart).ToString())
                If (camSch.CheckForNewMovers(Profile.Cart)) Then
                    SavePaymentInformation()
                End If



                If radCCFinance.Checked Then

                    Dim dFinanceFee As Decimal = Me.xOrderCalc.FinanceFee '(oFinanceAmounts.TotalPayments * 25)
                    '-- This is the problem, if they are financing, we need that finance fee added back in
                    dOrderAmt = dOrderAmt + dFinanceFee

                    ''testing this here - ---- - -3/13/2015
                    dOrderAmt = dOrderAmt + Me.xOrderCalc.CouponDiscount

                    Me.xOrderCalc.OrderTotal = dOrderAmt 'xOrderCalc.OrderTotal + xOrderCalc.FinanceFee

                    '-- Add the finance fee onto the subtotal
                    oAmounts.SubTotal = oAmounts.SubTotal + dFinanceFee

                    Dim oFAtt As XmlNode = xmlhelp.AddOrUpdateXMLNode(oProd, "Attribute", "")
                    '<Attribute Name="Number of Drops" OptCatId="0" Value="1" ValueName="1 drop ($0.00)" PriceMod="0.00" PriceModePercent="False" WeightMod="0" />
                    xmlhelp.AddOrUpdateXMLAttribute(oFAtt, "Name", "Finance Fees")
                    xmlhelp.AddOrUpdateXMLAttribute(oFAtt, "OptCatId", "0")
                    xmlhelp.AddOrUpdateXMLAttribute(oFAtt, "Value", oFinanceAmounts.TotalPayments.ToString)
                    xmlhelp.AddOrUpdateXMLAttribute(oFAtt, "ValueName", oFinanceAmounts.TotalPayments & " Payments" & " (" & dFinanceFee.ToString("C") & ")")
                    xmlhelp.AddOrUpdateXMLAttribute(oFAtt, "PriceMod", dFinanceFee.ToString)
                    xmlhelp.AddOrUpdateXMLAttribute(oFAtt, "PriceModePercent", "False")
                    xmlhelp.AddOrUpdateXMLAttribute(oFAtt, "WeightMod", "0")
                End If
            End If


            'If this is a PO, store it in the Cart.
            If PONumber.Text.Length > 0 Then
                'nothing yet
            End If



            If oAmounts.SalesTax > 0 Then
                '-- Lets update our Postage Fee attribute with the fee
                Dim oPAtt As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Postage Fee']")
                If oPAtt IsNot Nothing Then
                    Dim iDropQty As Integer = 0
                    Dim oPDrops As XmlNodeList = oProd.SelectNodes("Drops/Drop")
                    For Each oDrop As XmlNode In oPDrops
                        Dim iDrop As Integer = 0
                        Integer.TryParse(xmlhelp.ReadAttribute(oDrop, "Total"), iDrop)
                        iDropQty = iDropQty + iDrop
                    Next



                    Dim dPostageFee As Decimal = Me.xOrderCalc.Postage 'postageRate * iDropQty
                    If Me.xOrderCalc.CouponCode = "USPS50-2014" Or Me.xOrderCalc.CouponCode = "USPS100-2014" Then
                        'dPostageFee = dPostageFee - xOrderCalc.CouponDiscount
                    End If
                    xmlhelp.AddOrUpdateXMLAttribute(oPAtt, "Value", iDropQty.ToString)
                    xmlhelp.AddOrUpdateXMLAttribute(oPAtt, "ValueName", iDropQty.ToString("N0") & " pieces (" & dPostageFee.ToString("C") & ")")
                    xmlhelp.AddOrUpdateXMLAttribute(oPAtt, "PriceMod", Math.Round(dPostageFee, 2).ToString)
                End If
            End If




            '-- Create our order object
            Dim oOrder As Taradel.OrderHeader = Taradel.OrderHeader.CreateOrderHeader(0, False, False)
            oOrder.OrderGUID = sCCOrderRef
            oOrder.AffiliateID = Taradel.WLUtil.GetSiteId
            oOrder.CustomerReference.EntityKey = oCust.EntityKey
            oOrder.Created = System.DateTime.Now
            oOrder.FullName = BillInfo_FirstName.Text & " " & BillInfo_LastName.Text
            oOrder.CompanyName = BillInfo_Company.Text
            oOrder.Address = (BillInfo_Address1.Text & " " & BillInfo_Address2.Text).Trim
            oOrder.City = BillInfo_City.Text
            oOrder.State = BillInfo_State.SelectedValue
            oOrder.ZipCode = BillInfo_Zip.Text
            oOrder.Country = "US" 'BillInfo_Country.SelectedValue
            oOrder.PhoneNumber = BillInfo_Phone.Text


            'Save the email address where the receipt is sent in OrderHeader
            'One or the other (ShowReceiptEmail or ShowBillingEmail) must be set to true.
            If (SiteDetails.ShowReceiptEmail) Then
                oOrder.EmailAddress = txtEmailReceipt.Text
            Else
                oOrder.EmailAddress = BillInfo_Email.Text
            End If


            oOrder.PaymentType = Taradel.Util.Commerce.GetCardTypeFromNumber(CCNumber.Text).ToString


            oOrder.WLMarkup = dWLMarkup
            oOrder.WLCommission = 0
            oOrder.WLPaymentDue = 0

            If UseOwnGateWay Then
                oOrder.WLPaymentDue = dOrderAmt '-- Customer pays dOrderAmt - Bottom line of receipt
            Else
                If dWLMarkup > 0 Then
                    oOrder.WLCommission = dWLMarkup
                Else
                    oOrder.WLPaymentDue = dWLMarkup * -1
                End If
            End If

            oOrder.Subtotal = oAmounts.SubTotal


            'new code for coupons being applied on this page 3/6/2015
            If (Request.QueryString("c") IsNot Nothing) Then
                Dim newCoupon As New Taradel.Marketing.CouponDiscount
                newCoupon = RetrieveCoupon(Request.QueryString("c").ToString())
                AddCouponCodeToCartXml(newCoupon)
                oOrder.Discount = newCoupon.DiscountAmount
            End If


            oOrder.AssociationDiscount = 0
            oOrder.Shipping = 0
            oOrder.ShippingDiscount = 0
            oOrder.AssociationShippingDiscount = 0


            oOrder.SalesTax = Me.xOrderCalc.SalesTax '.xTaxAmount
            oOrder.SalesTaxRate = Me.xOrderCalc.SalesTaxRate 'Me.xTaxRate
            oOrder.SalesTaxJurisdiction = Me.xOrderCalc.SalesTaxState 'Me.xTaxJurisdiction
            'end sales tax functionality - state tax

            oOrder.CouponCode = Me.xOrderCalc.CouponCode 'oAmounts.CouponCode
            oOrder.CouponDiscount = Me.xOrderCalc.CouponDiscount 'oAmounts.CouponDiscount

            '-- This needs to be the full amount that Taradel is due for this order
            oOrder.OrderAmt = Me.xOrderCalc.OrderTotal 'dOrderAmt 'oAmounts.OrderAmount




            'This process inserts the item into the pnd_OrderItems table.  BaseProductID is required!
            '5/28/2015
            Dim xNodeList As XmlNodeList = Profile.Cart.SelectNodes("//Product")
            For Each XNode As XmlNode In xNodeList
                Dim testID As Integer = 1
                Integer.TryParse((XNode.Attributes("Quantity").Value).ToString().Replace(",", String.Empty), testID) 'could also prevent comma entry in the 1st place
                Dim xOrderItem As Taradel.OrderItem = Taradel.OrderItem.CreateOrderItem(0, testID)
                xOrderItem.DesignFee = Decimal.Parse(XNode.Attributes("DesignFee").Value)
                xOrderItem.HasResidentialShipping = False
                xOrderItem.Price = Decimal.Parse(XNode.Attributes("Price").Value)


                xOrderItem.ProductReference.EntityKey = Taradel.ProductDataSource.GetEntityKey(Integer.Parse(XNode.Attributes("BaseProductID").Value))


                xOrderItem.XMLData = XNode.OuterXml

                Dim xAttribs As XmlNodeList = XNode.SelectNodes("Attribute")
                For Each oAttrib As XmlNode In xAttribs
                    Dim optionId As Integer = 0
                    Dim sOptCatId As String = xmlhelp.ReadAttribute(oAttrib, "OptCatId")
                    Dim sOptVal As String = ""
                    If sOptCatId <> "0" Then
                        sOptVal = xmlhelp.ReadAttribute(oAttrib, "Value")
                    End If
                    Integer.TryParse(sOptVal, optionId)
                    Dim sName As String = xmlhelp.ReadAttribute(oAttrib, "Name")
                    Dim sValue As String = xmlhelp.ReadAttribute(oAttrib, "ValueName")
                    Dim oOrderItemAttribute As Taradel.OrderItemAttribute = Taradel.OrderItemAttribute.CreateOrderItemAttribute(0, optionId, sValue)
                    oOrderItemAttribute.AttributeName = sName
                    xOrderItem.Attributes.Add(oOrderItemAttribute)
                Next

                oOrder.OrderItems.Add(xOrderItem)

            Next


            Dim oOrderItem As Taradel.OrderItem = Taradel.OrderItem.CreateOrderItem(0, oQuote.Quantity)
            oOrderItem.DesignFee = oQuote.DesignPrice
            oOrderItem.HasResidentialShipping = False
            oOrderItem.Price = oQuote.Price
            oOrderItem.ProductReference.EntityKey = Taradel.ProductDataSource.GetEntityKey(oQuote.OriginalProductId)
            oOrderItem.XMLData = oProd.OuterXml


            Dim oAttribs As XmlNodeList = oProd.SelectNodes("Attribute")
            For Each oAttrib As XmlNode In oAttribs
                Dim optionId As Integer = 0
                Dim sOptCatId As String = xmlhelp.ReadAttribute(oAttrib, "OptCatId")
                Dim sOptVal As String = ""
                If sOptCatId <> "0" Then
                    sOptVal = xmlhelp.ReadAttribute(oAttrib, "Value")
                End If
                Integer.TryParse(sOptVal, optionId)
                Dim sName As String = xmlhelp.ReadAttribute(oAttrib, "Name")
                Dim sValue As String = xmlhelp.ReadAttribute(oAttrib, "ValueName")
                Dim oOrderItemAttribute As Taradel.OrderItemAttribute = Taradel.OrderItemAttribute.CreateOrderItemAttribute(0, optionId, sValue)
                oOrderItemAttribute.AttributeName = sName
                oOrderItem.Attributes.Add(oOrderItemAttribute)
            Next




            Dim oDrops As XmlNodeList = oProd.SelectNodes("Drop")
            For Each oDrop As XmlNode In oDrops
                Dim dropNumber As Integer = 0
                Integer.TryParse(xmlhelp.ReadAttribute(oDrop, "Number"), dropNumber)
                Dim deliveryDate As DateTime = DateTime.Parse(xmlhelp.ReadAttribute(oDrop, "Date"))
                Dim oOrderItemDelivery As Taradel.OrderItemDelivery = Taradel.OrderItemDelivery.CreateOrderItemDelivery(0, dropNumber)
                oOrderItemDelivery.DeliveryDate = deliveryDate
                oOrderItem.Drops.Add(oOrderItemDelivery)
            Next



            Dim oBillName As New Taradel.PersonName
            oBillName.FirstName = BillInfo_FirstName.Text
            oBillName.LastName = BillInfo_LastName.Text
            oBillName.EmailAddress = BillInfo_Email.Text

            Dim oBillPlace As New Taradel.Address
            oBillPlace.CompanyName = BillInfo_Company.Text
            oBillPlace.Address1 = BillInfo_Address1.Text
            oBillPlace.Address2 = BillInfo_Address2.Text
            oBillPlace.City = BillInfo_City.Text
            oBillPlace.State = BillInfo_State.SelectedValue
            oBillPlace.ZipCode = BillInfo_Zip.Text
            oBillPlace.Country = "US"
            oBillPlace.PhoneNumber = BillInfo_Phone.Text

            Dim oBillInfo As New Taradel.Contact(oBillName, oBillPlace)
            Dim oResponse As Taradel.OrderResponse = Nothing


            'PAYMENT TYPE LOGIC
            Select Case radPaymentType.SelectedValue

                Case "CC"
                    If radCCPayNow.Checked Then

                        '-- Do a full payment now
                        oOrder.PaidAmt = xOrderCalc.OrderTotal 'oAmounts.OrderAmount
                        oOrder.PaidInFull = True

                        oResponse = Taradel.WLOrderDataSource.SaveFullPayment(oOrder, oAmounts, oBillInfo, CCNumber.Text, CVV2.Text, CCExp.SelectedDate, MerchantID, TransactionID, UseOwnGateWay)
                    Else

                        '-- Do a partial payment now, and schedule ARB payments for the balance
                        oOrder.PaidAmt = oFinanceAmounts.Deposit 'which is xOrderCalc.FinanceFirstPayment
                        oOrder.PaidInFull = False
                        oOrder.Recurring = True

                        Dim oSub As Taradel.Subscription = Taradel.Subscription.CreateSubscription(0, "Order Financing", oFinanceAmounts.FirstPaymentDate, "Days", oFinanceAmounts.PaymentInterval, _
                                                                                                    oFinanceAmounts.TotalPayments, oFinanceAmounts.MonthlyPaymentAmount, oBillName.FirstName, _
                                                                                                    oBillName.LastName, oBillPlace.StreetAddress, oBillPlace.City, oBillPlace.State, _
                                                                                                    oBillPlace.ZipCode, oBillPlace.Country, oBillPlace.PhoneNumber, oBillName.EmailAddress, _
                                                                                                    Taradel.Util.Commerce.GetCardTypeFromNumber(CCNumber.Text).ToString, "", "", "")
                        oSub.Active = True
                        oSub.Company = BillInfo_Company.Text
                        oSub.CustomerReference.EntityKey = oOrder.CustomerReference.EntityKey
                        oSub.ProductReference.EntityKey = oOrderItem.ProductReference.EntityKey
                        oSub.TrialAmount = oFinanceAmounts.IntroPaymentAmount
                        oSub.TrialOccurs = oFinanceAmounts.IntroPayments
                        oOrder.Subscription = oSub

                        oResponse = Taradel.WLOrderDataSource.SaveFinancePayment(oOrder, oFinanceAmounts, oBillInfo, CCNumber.Text, CVV2.Text, CCExp.SelectedDate, Me.MerchantID, Me.TransactionID, Me.UseOwnGateWay)

                    End If

                Case "EC"
                    If radECPayNow.Checked Then

                        oOrder.PaidAmt = oAmounts.PaidAmount
                        oOrder.PaidInFull = True
                        oResponse = Taradel.WLOrderDataSource.SaveFullPayment(oOrder, oAmounts, oBillInfo, BankRoutingNumber.Text, BankAccountNumber.Text, BankName.Text, BankNameOnAccount.Text, Me.MerchantID, Me.TransactionID, Me.UseOwnGateWay)

                    End If

                Case "PO"

                    oOrder.PaidAmt = 0
                    oOrder.PaidInFull = False
                    oResponse = Taradel.WLOrderDataSource.SavePOPayment(oOrder, oBillInfo, PONumber.Text)

            End Select



            'CHECKING PAYMENT STATUS
            If oResponse.StatusCode = Taradel.OrderResponse.ResponseStatusCode.Success Then
                Try
                    Dim oSavedOrderItem As Taradel.OrderItem = oResponse.OrderData.OrderItems.FirstOrDefault
                    If oSavedOrderItem IsNot Nothing Then
                        If DistributionId > 0 Then
                            '-- Update the order with the direct mail flag
                            Taradel.OrderItemDataSource.SetDirectMailFlag(oSavedOrderItem.OrderItemID)
                            '-- Update the order with the has map flag
                            Taradel.OrderItemDataSource.SetHasMapFlag(oSavedOrderItem.OrderItemID)
                        End If
                    End If

                    'new exclusive logic 
                    If chkLocktheRoutes.Checked Then
                        ExclusiveUtility.SaveExclusive(oResponse)
                    End If

                    If SiteDetails.OffersExclusiveRoutes Then
                        Dim result As String = ExclusiveUtility.SaveExclusive(oResponse)
                        'Response.Write("ExclusiveUtility.SaveExclusive:" & result)
                        'Response.End()
                    End If
                    'end new exclusive logic


                Catch ex As Exception
                    log.Error(ex.Message, ex)
                End Try





                'RECORD THR STORE NUMBER in the [OrderTag] table if needed.
                If SiteDetails.RequireStoreNumber Then

                    'Get the Store Number TagGroupID
                    Dim storeNumber As String = Trim(txtStoreNumber.Text)                                   'Requires to provide as integers from user but really is a string in database.
                    Dim tagGroupID As Integer = 0
                    Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
                    Dim getSQL As String = "SELECT TOP 1 [OrderTagGroupID] FROM [OrderTagGroup] WHERE [Name] LIKE 'Store Number'"
                    Dim connectionObj As SqlConnection = New SqlConnection(connectionString)
                    Dim sqlCommand As SqlCommand = New SqlCommand()
                    Dim errorMsg As StringBuilder = New StringBuilder()

                    sqlCommand.CommandType = System.Data.CommandType.Text
                    sqlCommand.CommandText = getSQL
                    sqlCommand.Connection = connectionObj

                    Try

                        connectionObj.Open()
                        tagGroupID = sqlCommand.ExecuteScalar()

                    Catch objException As Exception

                        errorMsg.Append("The following errors occurred" & Environment.NewLine)
                        errorMsg.Append("Message: " & objException.Message & Environment.NewLine)
                        errorMsg.Append("Source: " & objException.Source & Environment.NewLine)
                        errorMsg.Append("Stack Trace: " & objException.StackTrace & Environment.NewLine)
                        errorMsg.Append("Target Site: " & objException.TargetSite.Name & Environment.NewLine)
                        logger.RecordInLog("[Step3-Checkout ERROR] Error Retrieving TagGroupID: " & Environment.NewLine & errorMsg.ToString())
                        EmailUtility.SendAdminEmail("There was an error retriveing OrderTagID in Step3-Checkout. Check the EDDM-Log.")

                    Finally
                        connectionObj.Close()
                    End Try


                    'Insert the record and record the store number.
                    If (tagGroupID > 0) Then
                        RecordStoreNumber(tagGroupID, oResponse.OrderId, storeNumber)
                    End If


                    'SEND ORDER DATA and 'Staples Store Number'.  At the moment, this is only used for Site#91 - Staples Store Site.
                    If (SiteDetails.TransmitOrderData) Then

                        Try

                            Dim apiURL As String = "http://" & Request.ServerVariables("http_host") & String.Format(SiteDetails.TransmitOrderAPIEndPoint, oResponse.OrderId.ToString())
                            Dim s As HttpWebRequest
                            Dim enc As UTF8Encoding
                            Dim postdata As String
                            Dim postdatabytes As Byte()

                            s = HttpWebRequest.Create(apiURL)
                            enc = New System.Text.UTF8Encoding()
                            postdata = String.Empty '"username=*****&password=*****&message=test+message&orig=test&number=447712345678"
                            postdatabytes = enc.GetBytes(postdata)
                            s.Method = "POST"
                            s.ContentType = "application/x-www-form-urlencoded"
                            s.ContentLength = postdatabytes.Length

                            Using stream = s.GetRequestStream()
                                stream.Write(postdatabytes, 0, postdatabytes.Length)
                            End Using
                            Dim result = s.GetResponse()

                        Catch ex As Exception

                            Response.Write(ex.StackTrace)
                            Response.Write(ex.ToString)

                        Finally

                        End Try

                    End If


                End If




                'TRY CREATING PDF AND SENDING EMAIL.....
                Try

                    'CREATE RECEIPT PDF
                    Dim receiptFilePath As String = ""
                    receiptFilePath = TaradelReceiptUtility.Generate(oResponse.OrderId, oResponse.OrderData.OrderGUID, receiptFilePath, ProductType2)
                    logger.RecordInLog("Attempting to create PDF.  Receipt PDF Path: " & receiptFilePath)


                    'Send the Email
                    If receiptFilePath.Length > 0 Then
                        SendReceiptEmail(receiptFilePath, oOrder)
                    Else
                        log.Error("Receipt Generated FALSE")
                    End If


                Catch ex As Exception
                    log.Error("from email:" & ex.Message, ex)
                End Try







                'HUBSPOT
                '======================================================================================================================
                Dim sPaper As String = ""
                Try
                    Dim oPaper As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Paper']")
                    If oPaper IsNot Nothing Then
                        sPaper = xmlhelp.ReadAttribute(oPaper, "ValueName")
                    End If
                Catch ex As Exception

                End Try



                If currentMode <> "dev" Then

                    If (SiteDetails.EnableHubSpot) Then

                        If oCust IsNot Nothing Then

                            Dim hsPortalID As String = ""
                            Dim hsFormID As String = ""
                            Dim hsCookieValue As String = ""
                            Dim pageTitle As String = ""

                            'Only EDDM and TaradelDM should be using this code
                            Try
                                Select Case SiteID

                                    Case 1
                                        hsPortalID = "212947"
                                        hsFormID = "5c2b91a7-a5a6-4411-830b-0bb78529557f"
                                        pageTitle = "EveryDoorDirectMail.com Account Transaction"

                                    Case 100
                                        hsPortalID = "101857"
                                        hsFormID = "e85638b6-e1d7-41cd-aaf8-3edd6764d1ff"
                                        pageTitle = "Taradel Direct Mail Account Transaction"

                                    Case Else
                                        hsPortalID = "212947"
                                        hsFormID = "5c2b91a7-a5a6-4411-830b-0bb78529557f"
                                        pageTitle = "Unknown page Account Transaction"

                                End Select



                                'get and populate the HubSpot object.
                                Dim hubSpotObj As New HubSpot()
                                hubSpotObj.firstName = Trim(oCust.FirstName)
                                hubSpotObj.lastName = Trim(oCust.LastName)
                                hubSpotObj.companyName = Trim(oCust.Company)
                                hubSpotObj.emailAddress = Trim(oCust.EmailAddress)
                                hubSpotObj.phoneNumber = Trim(oCust.PhoneNumber)

                                'This can be null.  Test for it.
                                Dim oBizClass As Taradel.BusinessClass = oCust.BusinessClass
                                If oBizClass IsNot Nothing Then
                                    hubSpotObj.industry = oCust.BusinessClass.Name
                                End If

                                hubSpotObj.hsPortalID = hsPortalID
                                hubSpotObj.hsFormGUID = hsFormID
                                hubSpotObj.pageTitle = pageTitle
                                hubSpotObj.pageURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri
                                hubSpotObj.ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress
                                hubSpotObj.orderID = oResponse.OrderId
                                hubSpotObj.orderDate = (System.DateTime.Now).ToString()
                                hubSpotObj.orderAmount = oResponse.OrderData.OrderAmt
                                hubSpotObj.paidAmount = oResponse.OrderData.PaidAmt
                                hubSpotObj.productName = xmlhelp.ReadAttribute(oProd, "Name")
                                hubSpotObj.quantity = xmlhelp.ReadAttribute(oProd, "Quantity")
                                hubSpotObj.paper = sPaper
                                hubSpotObj.jobName = xmlhelp.ReadAttribute(oProd, "JobName")
                                hubSpotObj.jobComments = xmlhelp.ReadAttribute(oProd, "JobComments")


                                'Look for previous cookie and retrieve that.
                                If HttpContext.Current.Request.Cookies("hubspotutk") IsNot Nothing Then
                                    hsCookieValue = HttpContext.Current.Request.Cookies("hubspotutk").Value
                                End If


                                'Post to HubSpot.  Read results.
                                Dim postResults As String = hubSpotObj.SendLeadData(hubSpotObj)


                                If (postResults = "OK") Then
                                    'Post to HubSpot was successful.  Carry on.
                                Else
                                    'Send email
                                    EmailUtility.SendAdminEmail("There was an error sending Contact Data to HubSpot.  Check the EDDM-Log.  HubSpot response was: " & postResults & ".")
                                End If


                            Catch ex As Exception
                                EmailUtility.SendAdminEmail("There was an error attempting to send Contact Data to HubSpot.  Error was: " & ex.ToString() & ".")
                            End Try

                        End If

                    End If

                End If


                'DELETE AFTER 7/1/2016
                '======================================================================================================================
                'Original code
                'Dim oHubPost As New Hashtable
                'If oCust Is Nothing Then
                '    oCust = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name)
                'End If

                'If oCust IsNot Nothing Then
                '    oHubPost.Add("firstname", oCust.FirstName)
                '    oHubPost.Add("lastname", oCust.LastName)
                '    oHubPost.Add("email", oCust.EmailAddress)
                '    oHubPost.Add("phone", oCust.PhoneNumber)
                '    oHubPost.Add("address", oCust.Address1)
                '    oHubPost.Add("city", oCust.City)
                '    oHubPost.Add("state", oCust.State)
                '    oHubPost.Add("zip", oCust.ZipCode)

                '    Dim oBizClass As Taradel.BusinessClass = oCust.BusinessClass
                '    If oBizClass IsNot Nothing Then
                '        oHubPost.Add("industry", oBizClass.Name)
                '    End If
                'End If

                'oHubPost.Add("order_number", oResponse.OrderId)
                'oHubPost.Add("order_date", System.DateTime.Now.ToString("MM/dd/yyyy"))
                'oHubPost.Add("order_amount", oResponse.OrderData.OrderAmt)
                'oHubPost.Add("total_payments", oResponse.OrderData.PaidAmt)
                'oHubPost.Add("product", xmlhelp.ReadAttribute(oProd, "Name"))
                'oHubPost.Add("quantity", xmlhelp.ReadAttribute(oProd, "Quantity"))
                'oHubPost.Add("paper", sPaper)
                'oHubPost.Add("job_name", xmlhelp.ReadAttribute(oProd, "JobName"))
                'oHubPost.Add("message", xmlhelp.ReadAttribute(oProd, "JobComments"))


                ''Send to Hubspot if indicated in settings.....
                'If ConfigurationManager.AppSettings("SendToHubSpot") IsNot Nothing Then

                '    If ConfigurationManager.AppSettings("SendToHubSpot") <> False Then

                '        If (SiteDetails.EnableHubSpot) Then

                '            Dim currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()

                '            If currentMode <> "dev" Then
                '                Try
                '                    Taradel.Hubspot.SendLeadData("5c2b91a7-a5a6-4411-830b-0bb78529557f", oHubPost)
                '                Catch ex As Exception
                '                    log.Error(ex.Message, ex)
                '                End Try
                '            End If

                '        End If

                '    End If

                '    'if it is nothing - go ahead and send it, we don't want to lose this...
                'Else
                '    Try
                '        Taradel.Hubspot.SendLeadData("5c2b91a7-a5a6-4411-830b-0bb78529557f", oHubPost)
                '    Catch ex As Exception
                '        log.Error(ex.Message, ex)
                '    End Try
                'End If
                '======================================================================================================================




                'If this is an AddressedList order
                If (USelectID = 5) Or (USelectID = 6) Then

                    'Populate pnd_CustomerListFulfillments if this is a AddressedList order.
                    InsertListFulfillmentRecord(oOrder.OrderID)

                    'Delete existing cookie which saves user-selected Filter Data.
                    If (Not Request.Cookies("TaradelDistributionID") Is Nothing) Then
                        Dim taradelDistIDCookie As HttpCookie
                        taradelDistIDCookie = New HttpCookie("TaradelDistributionID")
                        taradelDistIDCookie.Expires = DateTime.Now.AddDays(-1D)
                        Response.Cookies.Add(taradelDistIDCookie)
                    End If


                Else
                End If




                'Clean out current 'cart' and store order in Profile.LastOrder.  Receipt.aspx will use this.
                Profile.LastOrder = Profile.Cart
                Profile.Cart = Nothing
                Session.Abandon()



                Dim bNewReceiptProcess As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "NewReceiptProcess")
                bNewReceiptProcess = True

                If bNewReceiptProcess Then
                    Response.Redirect("~/Receipt.aspx?refid=" & sCCOrderRef & "&OrderID=" & oOrder.OrderID)
                Else
                    Response.Redirect("~/Receipt-Orig.aspx?refid=" & sCCOrderRef)
                End If


            Else

                Dim errorPrefix As String
                Dim errorSuffix As String

                errorPrefix = "<div class=""alert alert-danger""><span class=""fa fa-exclamation-circle""></span>&nbsp;"
                errorSuffix = "</div>"

                '-- Oops. We have a problem!
                Select Case oResponse.StatusCode
                    Case Taradel.OrderResponse.ResponseStatusCode.PaymentError
                        lCheckoutMsg.Text = (errorPrefix & "Your payment could not be processed: " & oResponse.Message & errorSuffix)

                    Case Taradel.OrderResponse.ResponseStatusCode.SaveFailed
                        lCheckoutMsg.Text = (errorPrefix & "<strong>Your card has not been charged.</strong> There was an error saving your order:" & oResponse.Message & errorSuffix)

                    Case Taradel.OrderResponse.ResponseStatusCode.Unknown
                        lCheckoutMsg.Text = (errorPrefix & "<strong>Your card has not been charged.</strong> There was an error saving your order:" & oResponse.Message & errorSuffix)
                End Select

            End If



        Else
            '-- Re-enable the correct payment panel
            Dim sSelPayType As String = radPaymentType.SelectedValue
            Dim oSelPayPnl As Panel = Nothing
            Select Case sSelPayType.ToLower
                Case "cc"
                    oSelPayPnl = pPayCC
                Case "ec"
                    oSelPayPnl = pPayEC
                Case "po"
                    oSelPayPnl = pPayPO
            End Select
            If oSelPayPnl IsNot Nothing Then
                oSelPayPnl.Style.Remove("display")
            End If
        End If




    End Sub



    Protected Sub rDrops_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rDrops2.ItemDataBound

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            Dim rRoutes As Repeater = DirectCast(e.Item.FindControl("rRoutes"), Repeater)
            Dim DropNumber As Integer = Integer.Parse(DataBinder.Eval(e.Item.DataItem, "Number"))
            Dim oDropOrg As XmlNode = Profile.Cart.SelectSingleNode("//Drop[@Number=" & DropNumber & "]")

            'Routes only exist in EDDM Distributions
            If EDDMMap Then

                If oDropOrg IsNot Nothing Then

                    Dim oDrop As XmlNode = oDropOrg.CloneNode(True)
                    oDrop.Attributes.RemoveAll()

                    Using oSr As New StringReader(oDrop.OuterXml)
                        Using oDs As New DataSet
                            oDs.ReadXml(oSr)
                            rRoutes.DataSource = oDs
                            rRoutes.DataBind()
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



    Protected Sub lnkApplyCoupon_Click(sender As Object, e As EventArgs)
        ApplyCoupon(txtCouponCode.Text.ToUpper())
    End Sub



    Protected Sub btnSaveCreditCardInfo_Click(sender As Object, e As EventArgs)

        SavePaymentInformation()

    End Sub



    Protected Function RetrieveCoupon(sCouponCode As String) As Taradel.Marketing.CouponDiscount
        Me.xCouponDiscount = Nothing
        'Response.Write("RetrieveCoupon  xOrderCalc.CustomerID :" + xOrderCalc.CustomerID.ToString())
        Dim oWLCoupon As Taradel.Coupon = Taradel.WLCouponDataSource.GetCoupon(sCouponCode)
        '    Protected oCoupon As Taradel.WLCouponValidator = Nothing
        Dim couponDiscount As New Taradel.Marketing.CouponDiscount
        Try
            If oWLCoupon IsNot Nothing Then
                couponDiscount = oWLCoupon.Validate(Profile.Cart, Me.oCust.CustomerID)
                If couponDiscount.IsValid Then
                    couponDiscount = CalculateSuperCoupon(couponDiscount)
                    Me.xCouponDiscount = couponDiscount
                    Me.xCoupon = oWLCoupon
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try
        Return couponDiscount
    End Function



    Protected Sub ApplyCoupon(sCouponCode As String)

        Me.xCouponDiscount = Nothing

        Dim oWLCoupon As Taradel.Coupon = Taradel.WLCouponDataSource.GetCoupon(sCouponCode)
        Dim couponDiscount As New Taradel.Marketing.CouponDiscount


        Try
            If oWLCoupon IsNot Nothing Then

                couponDiscount = oWLCoupon.Validate(Profile.Cart, Me.oCust.CustomerID)

                If couponDiscount.IsValid Then
                    Me.xCouponDiscount = couponDiscount
                    'don't calculate here
                    'calculate after the response.redirect
                    'Dim tempCalc As New TaradelReceiptUtility.OrderCalculator()
                    'tempCalc = GetNewOrderCalcObj()
                    'Me.xCouponDiscount = CalculateSuperCoupon(tempCalc, couponDiscount)
                    'Me.xCoupon = oWLCoupon
                    Response.Redirect(Request.Url.ToString() & "?c=" & Me.xCouponDiscount.CouponCode & "#coupon")
                    litCouponError.Visible = False
                Else
                    Session("couponDiscount") = Nothing
                    litCouponError.Visible = True
                    litCouponError.Text = "<span class=""fa fa-exclamation-circle text-danger""></span>&nbsp;<small><span class=""text-danger"">" & "Sorry but the discount amount is not valid.  Please contact us for assistance." & "</span></small>"

                End If

            Else
                litCouponError.Visible = True
                litCouponError.Text = "<span class=""fa fa-exclamation-circle text-danger""></span>&nbsp;<small><span class=""text-danger"">" & "Sorry but this Coupon Code is not valid.  Please try again." & "</span></small>"
            End If

        Catch ex As Exception
            litCouponError.Visible = True
            litCouponError.Text = "<span class=""fa fa-exclamation-circle text-danger""></span>&nbsp;<small><span class=""text-danger"">" & "Ooops. There was an error applying this coupon.  Please contact us for assistance." & "</span></small>"

            Response.Write(ex.ToString())
        End Try


    End Sub



    Private Sub AddCouponCodeToCartXml(xCoupon As Taradel.Marketing.CouponDiscount)

        Dim oXML As XmlDocument = Profile.Cart
        Dim oCart As XmlNode = oXML.SelectSingleNode("/cart")

        'Original
        Dim oCartProd As XmlNode = oCart.SelectSingleNode("Product")
        Dim oCoupon As XmlNode = oCartProd.SelectSingleNode("Attribute[@Name='Coupon']")

        If oCoupon IsNot Nothing Then
            oCoupon.Attributes("CouponCode").Value = xCoupon.CouponCode
            oCoupon.Attributes("DiscountAmount").Value = xCoupon.DiscountAmount
        Else
            Dim oCouponNew As XmlNode = xmlhelp.AddOrUpdateXMLNode(oCartProd, "Attribute", "")
            xmlhelp.AddOrUpdateXMLAttribute(oCouponNew, "Name", "Coupon")
            xmlhelp.AddOrUpdateXMLAttribute(oCouponNew, "CouponCode", xCoupon.CouponCode)
            xmlhelp.AddOrUpdateXMLAttribute(oCouponNew, "DiscountAmount", xCoupon.DiscountAmount)
        End If
        Profile.Cart = oXML
        Profile.Save()


        'ADD THE EDDM PRODUCT NODE
        'oCart = (CartUtility.AddCoupon(oCart, xCoupon.CouponCode, xCoupon.DiscountAmount.ToString()))




    End Sub



    Protected Sub SavePaymentInformation()
        Try
            Dim expDate As DateTime = DateTime.Parse(CCExp.SelectedDate)
            Dim saveDate As String = expDate.ToString("MM/yyyy")
            Dim aCustomer As New TaradelAuthorizeUtility.AuthorizeCustomer()
            aCustomer.UserName = oCust.Username 'txtUserName.Text
            Dim host As String = Request.ServerVariables("server_name")
            aCustomer = TaradelAuthorizeUtility.RetrieveAuthorizeCustomerProfile(aCustomer)
            'txtResult.Text = "Customer ID = " + aCustomer.CustomerID.ToString() + System.Environment.NewLine


        If aCustomer.ProfileID = 0 Then

            aCustomer = TaradelAuthorizeUtility.CreateCustomerProfile2(aCustomer.UserName, aCustomer.CustomerID, host)
            'txtResult.Text = txtResult.Text + "New profile id = " + aCustomer.ProfileID.ToString() + System.Environment.NewLine
            aCustomer = TaradelAuthorizeUtility.UpdateAuthorizeCustomer(aCustomer)
            'txtResult.Text = txtResult.Text + " Updated " + System.Environment.NewLine
            'txtResult.Text = txtResult.Text + " Customer User Name " + aCustomer.UserName + System.Environment.NewLine
        Else
            'txtResult.Text = "Profile id = " + aCustomer.ProfileID.ToString() + System.Environment.NewLine
        End If

        If aCustomer.PaymentProfileID = 0 Then
            Dim paymentProfile As New TaradelAuthorizeUtility.AuthorizeCustomerPaymentProfile()
            paymentProfile.CCV = CVV2.Text 'txtCVV.Text
            paymentProfile.CustomerID = aCustomer.CustomerID
            Dim sharedSecret As String = appxCMS.Util.AppSettings.GetString("SharedSecret")
            paymentProfile.EncryptedCCNumber = TaradelAuthorizeUtility.Crypto.EncryptStringAES(CCNumber.Text, sharedSecret)
            paymentProfile.ExpDate = saveDate '"10/2016" 'CCExp.SelectedDate 'needs to be 10/2016 
            paymentProfile.ProfileID = aCustomer.ProfileID
            paymentProfile.UserName = aCustomer.UserName
            paymentProfile = TaradelAuthorizeUtility.CreateCustomerPaymentProfile(paymentProfile, host)
            'txtResult.Text = txtResult.Text + "paymentProfileID:" + paymentProfile.PaymentProfileID.ToString()
            aCustomer.PaymentProfileID = paymentProfile.PaymentProfileID
            TaradelAuthorizeUtility.UpdateAuthorizeCustomerPaymentProfile(aCustomer)

            End If
        Catch ex As Exception
            Response.Write(ex.ToString())
            Response.End()
        End Try


    End Sub



    Protected Sub BindEDDMCart()

        Me.xOrderCalc = GetNewOrderCalcObj()

        Dim logObj As New LogWriter()
        Dim oCart As XmlDocument = Profile.Cart
        Dim sXML As String = Profile.Cart.OuterXml
        Dim eddmProdNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='EDDM']")
        Dim numberOfDrops As Integer = 0
        Dim dFirstDropDate As DateTime
        Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Nothing
        Dim oDist As Taradel.CustomerDistribution = Nothing
        Dim jobComments As String = BillInfo_Comments.Text


        If DistributionId > 0 Then

            phOptOut.Visible = True

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


        'All is used for displaying user selection discription.  
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
                    log.Error(ex.Message, ex)
                    log.Warn("Price has was invalid")
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
            pCheckout.Visible = False
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("Error in building oQuote on Step3-Checkout. <br /><br />Message: <br />" & ex.StackTrace.ToString())
        End Try



        If markup > 0 Then
            'Response.Write("markup:" & markup)
            Response.Write("<br/>")

            oBaseQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oQuote.ProductId, oQuote.Quantity, oQuote.UserDistributionId, oQuote.NumberOfDrops, oQuote.Options, oQuote.ZipCode, oQuote.WithDesign, oQuote.WithTemplate, 0, "")
        Else
            'Response.Write("markup:" & markup)
            'Response.Write("<br/>")

            oBaseQuote = oQuote
        End If

        If oQuote Is Nothing Then

            Response.Write("numberOfDrops:" & numberOfDrops)
            Response.Write("<br/>")
            Response.Write("DistributionId:" & DistributionId)
            Response.Write("<br/>")
            Response.Write("appxCMS.Util.CMSSettings.GetSiteId():" & appxCMS.Util.CMSSettings.GetSiteId())
            Response.Write("<br/>")
            Response.Write(" oProduct.BaseProductID.Value:" & oProduct.BaseProductID.Value)
            Response.Write("<br/>")
            Response.Write(" iQty:" & iQty)


            Response.Write("<br/>")


            Response.Write("oQuote is null")
            Response.End()
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


        'QTY
        Dim sQty As String = xmlhelp.ReadAttribute(eddmProdNode, "Quantity")
        Dim dQty As Decimal = 0
        Decimal.TryParse(sQty, dQty)
        lQty.Text = dQty.ToString("N0")



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
                    rDrops2.DataSource = oDs
                    rDrops2.DataBind()
                End Using
            End Using

        End If



        'Build Product Name label
        Dim sProdName As String = xmlhelp.ReadAttribute(eddmProdNode, "Name")
        litEddmProductName.Text = sProdName




        'Show / Hide Panels
        'RetrieveMaps(Me.xOrderCalc.CustomerID)




        'Update the EDDM Mailing Row
        litMailingTitle.Text = "Every Door Direct Mail Campaign"
        lblPrintingEstimate.Text = Me.xOrderCalc.MailPiecesPrice.ToString("C")
        litNumOfPcs.Text = Me.xOrderCalc.MailPieces.ToString("N0") & " @ " & TaradelReceiptUtility.OrderCalculator.FormatAsCurrency3(xOrderCalc.PricePerPiece)


        'SubTotal & Total
        lblSubTotal.Text = Me.xOrderCalc.Subtotal.ToString("C")


        'Sales Tax Disclaimer
        If (appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableSalesTax")) Then
            litSalesTaxDisclaimer.Text = " <span class=""catRowDetails""> (Does not include Sales Tax.)</span>"
        End If


        TotalEstimate.Text = Me.xOrderCalc.OrderTotal.ToString("C")



        UpdateCouponDisplayLogic(Me.xOrderCalc)

        ''Exclusive logic - 11/25/2015
        If SiteDetails.OffersExclusiveRoutes Then
            If Me.xOrderCalc.NumOfDrops >= SiteDetails.NumImpressionsForExclusive Then
                If Me.xOrderCalc.MailPieces + Me.xOrderCalc.ExtraPieces >= SiteDetails.MinQtyForExclusive Then
                    phLockedRoutesMsg.Visible = True
                    litLockedRoutes.Text = ExclusiveUtility.RetrieveStep3Message(oDropsOrg)
                End If
            End If
        End If
        ''

    End Sub



    Protected Sub BindAddressedCart()

        Me.xOrderCalc = GetNewOrderCalcObj()

        Dim logObj As New LogWriter()
        Dim oCart As XmlDocument = Profile.Cart
        Dim sXML As String = Profile.Cart.OuterXml
        Dim productNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='AddressedList']")
        Dim numberOfDrops As Integer = 0
        Dim dFirstDropDate As DateTime
        Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Nothing
        Dim oDist As Taradel.CustomerDistribution = Nothing
        Dim jobComments As String = BillInfo_Comments.Text


        If DistributionId > 0 Then

            phOptOut.Visible = True

            oDist = Taradel.CustomerDistributions.GetDistribution(DistributionId)

            If oDist IsNot Nothing Then

                'Show Campaign name
                If Not String.IsNullOrEmpty(oDist.Name) Then
                    lSelectName.Text = oDist.Name
                End If


                'Get the number of drops
                Dim oNumDrops As XmlNode = productNode.SelectSingleNode("Attribute[@Name='Number of Drops']")
                If oNumDrops IsNot Nothing Then
                    numberOfDrops = Integer.Parse(xmlhelp.ReadAttribute(oNumDrops, "Value"))
                Else
                    'Count the number of drops and add this in
                    Dim oTotalDrops As XmlNodeList = productNode.SelectNodes("//Drop")
                    numberOfDrops = oTotalDrops.Count
                End If


                'get and set the first Drop Date
                Dim oFirstDrop As XmlNode = productNode.SelectSingleNode("Drops/Drop[1]")
                Dim sFirstDropDate As String = xmlhelp.ReadAttribute(oFirstDrop, "Date")
                DateTime.TryParse(sFirstDropDate, dFirstDropDate)
                dFirstDropDate = New DateTime(dFirstDropDate.Year, dFirstDropDate.Month, dFirstDropDate.Day, 23, 59, 59)

                ''oSummary = Taradel.CustomerDistributions.GetSelectionSummary(oDist.ReferenceId)

            End If


        End If


        'get and build the Product Obj
        Dim oProduct As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(ProductID)

        'Get the options for this product.
        Dim oOpts As XmlNodeList = productNode.SelectNodes("Attribute[@OptCatId != 0]")
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
        Dim oDesign As XmlNode = productNode.SelectSingleNode("Attribute[@Name='Professional Design Services']")
        If oDesign IsNot Nothing Then
            bDesign = True
        End If


        'Determine if this is a Template design
        Dim bTemplate As Boolean = False
        Dim oFront As XmlNode = productNode.SelectSingleNode("Design/Front")
        If oFront IsNot Nothing Then
            Dim sDesignType As String = xmlhelp.ReadAttribute(oFront, "DesignSelectionType")
            If sDesignType.ToLower = "template" Then
                bTemplate = True
            End If
        End If


        'Map Description & List Drop Details
        Dim numPcs = CartUtility.GetAddressedDropTotal(oCart, xOrderCalc.IsThisAMultiple, numberOfDrops, xOrderCalc.MailPieces, "AddressedList")
        lSelectDescription.Text = "Your specialized criteria will allow you to reach and deliver to " & numPcs.ToString("N0") & " targeted addresses."


        'Show Drop and Filter Details
        BuildAddressedListDetails(numPcs)


        'QTY.  AKA TotalMailed. Selected x Num of Impressions
        Dim iQty As Integer = 0
        Integer.TryParse(xmlhelp.ReadAttribute(productNode, "Quantity"), iQty)


        Dim sZip As String = ""
        If DistributionId = 0 Then

            'Direct ship product, need zip code
            Dim oShip As XmlNode = productNode.SelectSingleNode("shipments/shipment[1]")

            If oShip IsNot Nothing Then
                Try
                    sZip = xmlhelp.ReadAttribute(oShip, "pricehash").Split(New Char() {"-"})(1)
                Catch ex As Exception
                    log.Error(ex.Message, ex)
                    log.Warn("Price has was invalid")
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
            pCheckout.Visible = False
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("Error in building oQuote on Step3-Checkout. <br /><br />Message: <br />" & ex.StackTrace.ToString())
        End Try



        If markup > 0 Then
            oBaseQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oQuote.ProductId, oQuote.Quantity, oQuote.UserDistributionId, oQuote.NumberOfDrops, oQuote.Options, oQuote.ZipCode, oQuote.WithDesign, oQuote.WithTemplate, 0, "")
        Else
            oBaseQuote = oQuote
        End If




        'Get Number of weeks
        Dim sFreq As String = xmlhelp.ReadAttribute(productNode.SelectSingleNode("Attribute[@Name='Drop Schedule']"), "Value")
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

            Dim sIndex As String = xmlhelp.ReadAttribute(productNode, "Index")
            Dim bHasFile As Boolean = False
            Dim oGNode As XmlNode = productNode.SelectSingleNode("Design")

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


        'QTY
        Dim sQty As String = xmlhelp.ReadAttribute(productNode, "Quantity")
        Dim dQty As Decimal = 0
        Decimal.TryParse(sQty, dQty)
        lQty.Text = dQty.ToString("N0")



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




        'Build Product Name label
        Dim sProdName As String = xmlhelp.ReadAttribute(productNode, "Name")
        litAddressedProductName.Text = sProdName



        'Update the Mailing Row
        litMailingTitle.Text = "Addressed List Mail Campaign"
        lblPrintingEstimate.Text = Me.xOrderCalc.MailPiecesPrice.ToString("C")
        litNumOfPcs.Text = Me.xOrderCalc.MailPieces.ToString("N0") & " @ " & TaradelReceiptUtility.OrderCalculator.FormatAsCurrency3(xOrderCalc.PricePerPiece)


        'SubTotal & Total
        lblSubTotal.Text = Me.xOrderCalc.Subtotal.ToString("C")


        'Sales Tax Disclaimer
        If (appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableSalesTax")) Then
            litSalesTaxDisclaimer.Text = " <span class=""catRowDetails""> (Does not include Sales Tax.)</span>"
        End If


        TotalEstimate.Text = Me.xOrderCalc.OrderTotal.ToString("C")



        UpdateCouponDisplayLogic(Me.xOrderCalc)


    End Sub



    Private Function UpdateOrderCalcInCart(orderCalc2 As TaradelReceiptUtility.OrderCalculator, productNodeType As String) As TaradelReceiptUtility.OrderCalculator

        'On the previous page (ProductOptions), the OrderCalc node is created but most attributes are set to default values. Ex:
        '<OrderCalc ExtraPiecesPrice="0" FinanceFirstPayment="0" FinancePaymentAmount="0" FinanceTotal="0" MailPiecesPrice="0" SalesTax="0" SalesTaxRate="0" SalesTaxState="0" SalesTaxMessage="VA" Type="EDDM" />

        'On this page, we have new calculated values which come from the OrderCalculator we can update this node with such as Sales Tax.

        'Here, we're going to add (comes from Taradel Receipt Utility / Order Calculator):
        'Sales Tax
        'Sales Tax Rate
        'Sales Tax State
        'Sales Tax Message
        'Mail PPP
        'Extra Pieces Price
        'Finance First Payment
        'Finance Payment Amt
        'Finance Total
        'Drop Fee

        Dim oXML As XmlDocument = Profile.Cart
        Dim oCart As XmlNode = oXML.SelectSingleNode("/cart")
        Dim oCartProd As XmlNode = oCart.SelectSingleNode("Product[@Type='" & productNodeType & "']")
        Dim oOrderCalc As XmlNode = oCartProd.SelectSingleNode("Attribute[@Name='OrderCalc']")


        oCart = CartUtility.UpdateOrderCalc(oCart, productNodeType, orderCalc2.SalesTax.ToString(), orderCalc2.SalesTaxRate.ToString(), orderCalc2.SalesTaxState, _
                                            orderCalc2.SalesTaxMessage, orderCalc2.MailPiecesPrice.ToString(), orderCalc2.ExtraPiecesPrice.ToString(), orderCalc2.FinanceFirstPayment.ToString(), _
                                            orderCalc2.FinancePaymentAmount.ToString(), orderCalc2.FinanceTotal.ToString(), orderCalc2.DropFee.ToString())

        Return orderCalc2

    End Function



    Private Function RemoveFinanceAttributes(oProd As XmlNode) As XmlNode
        Dim xml As XDocument = XDocument.Load(New XmlNodeReader(oProd))
        For Each node In xml.Descendants().Where(Function(e) e.Attribute("FinanceFirstPayment") IsNot Nothing)
            node.Attribute("FinanceFirstPayment").Remove()
            node.Attribute("FinanceTotal").Remove()
            node.Attribute("FinancePaymentAmount").Remove()
        Next

        Using xReader2 As XmlReader = xml.CreateReader()
            Dim xmlDoc As New XmlDocument()
            xmlDoc.Load(xReader2)
            Return xmlDoc
        End Using

    End Function



    Private Function GetNewOrderCalcObj() As TaradelReceiptUtility.OrderCalculator

        '=========================================================================================
        '=========================================================================================

        'This is what is to be returned...
        Dim orderCalc2 As New TaradelReceiptUtility.OrderCalculator()

        'Cart / XML 
        Dim oCart As XmlDocument = Profile.Cart
        Dim oProd As XmlNode = oCart.SelectSingleNode("//Product[@SiteId=" & appxCMS.Util.CMSSettings.GetSiteId & "]")
        Dim oNumDrops As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Number of Drops']")
        Dim oCust As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name)
        Dim oOpts As XmlNodeList = oProd.SelectNodes("Attribute[@OptCatId != 0]")
        Dim productNodeType As String = xmlhelp.ReadAttribute(oProd, "Type")


        'Prod and properties
        '*************** 4/17/2015************************
        'Dim oProduct As Taradel.Product = Taradel.ProductDataSource.GetProduct(Integer.Parse(xmlhelp.ReadAttribute(oProd, "ProductID")))
        Dim oProduct As Taradel.Product = Taradel.ProductDataSource.GetProduct(Integer.Parse(xmlhelp.ReadAttribute(oProd, "BaseProductID")))
        Dim iQty As Integer = (xmlhelp.ReadAttribute(oProd, "Quantity"))
        Dim DistributionId As Integer = (xmlhelp.ReadAttribute(oProd, "DistributionId"))
        Dim bMultipleImpressionsNoFee = appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressionsNoFee")
        Dim numberOfDrops As Integer = (xmlhelp.ReadAttribute(oNumDrops, "Value"))
        Dim dDesignFee As Decimal = (xmlhelp.ReadAttribute(oProd, "DesignFee"))

        Dim sendThisNumberOfDrops As Int16 = numberOfDrops
        If (bMultipleImpressionsNoFee) Then
            sendThisNumberOfDrops = 1
        End If

        Dim oOptList As New SortedList(Of Integer, Integer)
        For Each oOpt As XmlNode In oOpts
            Dim optionId As Integer = Integer.Parse(xmlhelp.ReadAttribute(oOpt, "Value"))
            oOptList.Add(Integer.Parse(xmlhelp.ReadAttribute(oOpt, "OptCatId")), optionId)
        Next

        Dim sZip As String = ""
        If DistributionId = 0 Then
            '-- Direct ship product, need zip code
            Dim oShip As XmlNode = oProd.SelectSingleNode("shipments/shipment[1]")
            If oShip IsNot Nothing Then
                Try
                    sZip = xmlhelp.ReadAttribute(oShip, "pricehash").Split(New Char() {"-"})(1)
                Catch ex As Exception
                    'log.Error(ex.Message, ex)
                    log.Warn("Price hash was invalid")
                End Try
            End If
        End If

        Dim bDesign As Boolean = False
        Dim oDesign As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Professional Design Services']")
        If oDesign IsNot Nothing Then
            bDesign = True
        End If

        Dim bTemplate As Boolean = False
        Dim oFront As XmlNode = oProd.SelectSingleNode("Design/Front")
        If oFront IsNot Nothing Then
            Dim sDesignType As String = xmlhelp.ReadAttribute(oFront, "DesignSelectionType")
            If sDesignType.ToLower = "template" Then
                bTemplate = True
            End If
        End If


        Dim iQtyForQuote As Integer = iQty '* numberOfDrops
        Dim oQuote As Taradel.ProductPriceQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProduct.ProductID, iQtyForQuote, DistributionId, sendThisNumberOfDrops, oOptList, sZip, bDesign, bTemplate)
        Dim dPricePerPiece As Decimal = TaradelReceiptUtility.RetrievePricePerPiece(oCart)

        Dim oCustBillInfo As List(Of Taradel.CustomerBillingInfo) = Taradel.Customers.GetBillingAddresses(oCust.CustomerID)
        Dim oBill As Taradel.CustomerBillingInfo = oCustBillInfo.FirstOrDefault


        orderCalc2.MailPieces = TaradelReceiptUtility.RetrieveMailedItemsInOrder(oCart)
        orderCalc2.IsThisAMultiple = TaradelReceiptUtility.RetrieveIsThisAMultipleImpression(oCart)
        orderCalc2.NumOfDrops = numberOfDrops

        'this is wonky - killing the wonky 2/18/2016
        ''If orderCalc2.IsThisAMultiple Then
        ''orderCalc2.MailPieces = (orderCalc2.MailPieces / orderCalc2.NumOfDrops)
        ''End If
        'end of the wonky

        orderCalc2.ExtraPieces = TaradelReceiptUtility.RetrieveShippedItemsInOrder(oCart)
        orderCalc2.PricePerPiece = (dPricePerPiece * 100)
        orderCalc2.BasePricePerPiece = orderCalc2.PricePerPiece '(dPricePerPiece * 100)
        orderCalc2.CouponCode = ""
        orderCalc2.DesignFee = dDesignFee
        orderCalc2.CustomerID = oCust.CustomerID
        orderCalc2.CouponDiscount = 0



        orderCalc2.SalesTaxState = oCust.State
        orderCalc2.ShipAddressID = TaradelReceiptUtility.RetrieveShippedIDInOrder(oCart)
        orderCalc2.zErrorMessage = "GetNewOrderCalcObj"
        orderCalc2.SiteID = GetSiteId()
        ''''''new for multiple products
        orderCalc2 = TaradelReceiptUtility.AddAllProductsToOrderCalculator(orderCalc2, oCart)
        ''''''end new for multiple products


        'CheckForDebug(orderCalc2)

        'new code for coupons being applied on this page 3/6/2015
        If (Request.QueryString("c") IsNot Nothing) Then
            Dim newCoupon As New Taradel.Marketing.CouponDiscount
            Me.xOrderCalc = orderCalc2 'hack hack hack'
            'Response.Write("Me.xOrderCalc.DropFee:" + Me.xOrderCalc.DropFee.ToString())
            newCoupon = RetrieveCoupon(Request.QueryString("c").ToString())
            orderCalc2.CouponCode = newCoupon.CouponCode
            orderCalc2.CouponDiscount = newCoupon.DiscountAmount
            orderCalc2.CouponMessage = newCoupon.CouponMessage
            If (Me.xCouponDiscount Is Nothing) Then
                Me.xCouponDiscount = New Taradel.Marketing.CouponDiscount
            End If
            Me.xCouponDiscount = newCoupon
        End If
        'end new code



        orderCalc2 = TaradelReceiptUtility.CalculateOrder2(orderCalc2)

        CheckForDebug(orderCalc2)


        'Response.Write("<h1>" & orderCalc2.ExtraPieces & "</h1>")
        'Response.End()


        Dim oNewMover As XmlNode = oProd.SelectSingleNode("//Product[@Type='New Mover Postcard']")
        Dim oEmail As XmlNode = oProd.SelectSingleNode("//Product[@Type='Targeted Email']")


        'update the Cart's xml with the correct pricing attributes...
        For Each p As TaradelReceiptUtility.Product In orderCalc2.LstProducts
            If (p.ProductID = 235) Then
                xmlhelp.AddOrUpdateXMLAttribute(oNewMover, "PricePerPiece", p.PricePerPiece.ToString())
                xmlhelp.AddOrUpdateXMLAttribute(oNewMover, "Price", p.TotalProductPrice.ToString())
            ElseIf (p.ProductID = 236) Then
                xmlhelp.AddOrUpdateXMLAttribute(oEmail, "PricePerPiece", p.PricePerPiece.ToString())
                xmlhelp.AddOrUpdateXMLAttribute(oEmail, "Price", p.TotalProductPrice.ToString())
            End If
        Next


        'update the cart's xml with the OrderCalc attributes
        orderCalc2 = UpdateOrderCalcInCart(orderCalc2, productNodeType)


        Profile.Cart = oCart
        Profile.Save()
        'end update the cart
        'UpdateCouponDisplayLogic(orderCalc2)
        Me.xOrderCalc = orderCalc2
        Return orderCalc2

    End Function



    Private Function RetrieveNumberOfItemsInOrder() As Integer
        Dim oCart As XmlDocument = Profile.Cart
        Dim oProd As XmlNode = oCart.SelectSingleNode("//Product[@SiteId=" & appxCMS.Util.CMSSettings.GetSiteId & "]")
        Dim iQty As Integer = 0
        Integer.TryParse(xmlhelp.ReadAttribute(oProd, "Quantity"), iQty)
        Return iQty
    End Function



    Public Function ReturnAddressedZipCodes(oCart As XmlDocument) As String

        'Sample return:
        '<ul class="list-unstyled">
        '<li><small><span class="fa fa-check text-primary"></span>&nbsp;11111 (XX)</small></li>
        '<li><small><span class="fa fa-check text-primary"></span>&nbsp;11111 (XX)</small></li>
        '...etc..
        '</ul>

        '1) Find AddressedDrops in Cart object to get each area.
        '2) No need to (hopefully) loop thru the multiple drops since they will all be identical.  Drop #1 will be fine.
        '3) Strip it of route suffix and add to list IF it does not exist already.
        '4) Get the count of pre-stripped Area and add to total.
        '5) Go to next area.
        '6) Build html and return it.

        Dim results As New StringBuilder()
        Dim addressedDropsXml As XmlNode = oCart.SelectSingleNode("//AddressedDrops[@Type='Addressed']/Drop[@Number='1']")
        Dim counter As Integer = 0
        Dim zipCode As String = ""
        Dim routeName As String = ""
        'Dim zipCodeList As New List(Of String)
        Dim areaCount As Integer = 0



        If addressedDropsXml Is Nothing Then
            results.Append("<small>(No zip codes were detected.)</small>")
        Else

            'Make a data table to hold all records.
            Dim zipCodeTable As New DataTable
            zipCodeTable.Columns.Add("ZipCode", GetType(String))
            zipCodeTable.Columns.Add("Count", GetType(Integer))


            'start HTML
            results.Append("<ul class=""list-unstyled"">")


            For Each area As XmlNode In addressedDropsXml

                routeName = xmlhelp.ReadAttribute(area, "Name")
                areaCount = xmlhelp.ReadAttribute(area, "Total")

                'Make sure it's complete
                If routeName.Length = 9 Then
                    zipCode = routeName.Substring(0, 5)
                    zipCodeTable.Rows.Add(zipCode, Convert.ToInt32(areaCount))
                End If

            Next


            'Create a new table to store Grouped results.
            Dim sortedZipTable As New DataTable
            sortedZipTable.Columns.Add("ZipCode")
            sortedZipTable.Columns.Add("Count")

            'Use linq to painfully inserted the grouped results.
            Dim linqQuery = _
                    From c As DataRow In zipCodeTable.Rows.Cast(Of DataRow)() _
                    Order By c.Field(Of String)("ZipCode") Ascending _
                    Group c By zip = c.Field(Of String)("ZipCode") Into Group _
                          Select New With {.ZipCode = zip, .Count = Group.Sum(Function(r As DataRow) r.Field(Of Integer)("Count"))}


            For Each row In linqQuery
                sortedZipTable.Rows.Add(row.ZipCode, row.Count)
            Next

            'Debugging
            'For Each row As DataRow In zipCodeTable.Rows
            '    lblDebug.Text = lblDebug.Text & row("ZipCode") & " (" & row("Count") & ")<br /><br />"
            'Next

            For Each row As DataRow In sortedZipTable.Rows
                results.Append("<li><small><span class=""fa fa-check text-primary""></span>&nbsp;" & row("ZipCode") & " (" & row("Count") & ")</small></li>")
            Next

            zipCodeTable.Dispose()
            sortedZipTable.Dispose()

            'Complete HTML
            results.Append("</ul>")


        End If

        Return results.ToString()


    End Function



    Public Function ReturnAddressedDropDates(oCart As XmlDocument) As String

        'Sample return:
        '<ul class="list-unstyled">
        '<li><small><span class="fa fa-check text-primary"></span>&nbsp;1/1/2015</small></li>
        '<li><small><span class="fa fa-check text-primary"></span>&nbsp;2/1/2015</small></li>
        '...etc..
        '</ul>


        Dim results As New StringBuilder()
        Dim addressedDrops As XmlNodeList = oCart.SelectNodes("//AddressedDrops/Drop")
        Dim dropDate As String = ""

        If addressedDrops.Count <= 0 Then

            results.Append("<small>(No Drop Dates were detected.)</small>")

        Else

            'start HTML
            results.Append("<ul class=""list-unstyled"">")


            'Make a list of distinct zip codes
            For Each drop As XmlNode In addressedDrops
                dropDate = xmlhelp.ReadAttribute(drop, "Date")
                results.Append("<li><small><span class=""fa fa-check text-primary""></span>&nbsp;" & dropDate & "</small></li>")
            Next


            'Complete HTML
            results.Append("</ul>")


        End If

        Return results.ToString()


    End Function



    Private Function CalculateSuperCoupon(calcCoupon As Taradel.Marketing.CouponDiscount) As Taradel.Marketing.CouponDiscount

        If (calcCoupon.CouponCode.ToUpper() = "3FREE") Then

            'Removes any DropFees, Design Fees, and give free New Movers (one month). Display purposes only.
            'Does not update cart.

            Dim oCart As XmlDocument = Profile.Cart
            Dim superCouponMessage As String = "Custom Discount:"
            Dim discountAmount As Decimal = 0
            Dim dropFee As Decimal = CartUtility.GetDropFee(oCart)
            Dim designFee As Decimal = CartUtility.GetDesignFee(oCart, "EDDM")


            Dim calcNode As XmlNode = oCart.SelectSingleNode("//OrderCalc")

            If dropFee > 0 Then
                superCouponMessage += System.Environment.NewLine + "<br/>FREE DROP FEE!"
                discountAmount = discountAmount + dropFee
            End If


            If (designFee > 0) Then
                discountAmount = (discountAmount + designFee)
                superCouponMessage += System.Environment.NewLine + "<br/>FREE DESIGN FEE!"
            End If


            Dim newMoverNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='New Mover Postcard']")

            If (xmlhelp.ReadAttribute(newMoverNode, "Price") IsNot Nothing) Then
                Dim nmPrice As Decimal = 0
                Decimal.TryParse(xmlhelp.ReadAttribute(newMoverNode, "Price"), nmPrice)
                If (nmPrice > 0) Then
                    discountAmount = discountAmount + nmPrice
                    superCouponMessage += System.Environment.NewLine + "<br/>FREE NEW MOVER (first month!)"
                End If
            End If

            calcCoupon.CouponMessage = superCouponMessage
            calcCoupon.DiscountAmount = discountAmount

        End If

        Return calcCoupon

    End Function



    Public Sub UpdateCouponDisplayLogic(orderCalc2 As TaradelReceiptUtility.OrderCalculator)

        'ONLY valid Coupon will exist in the Session - not sure this is necessary, but thinking about legacy code
        If (Me.xCouponDiscount IsNot Nothing) Then

            'If (Me.xCouponDiscount.CouponCode = "3FREE4ME") Then
            '    Me.xCouponDiscount = CalculateSuperCoupon(Me.xCouponDiscount)
            'End If 'end super coupon

            lblCouponMsg.Text = Me.xCouponDiscount.CouponMessage
            lblCouponName.Text = Me.xCouponDiscount.CouponCode

            lblCouponDiscount.Text = TaradelReceiptUtility.OrderCalculator.FormatAsCredit(Me.xCouponDiscount.DiscountAmount)

            Me.xOrderCalc.CouponDiscount = Me.xCouponDiscount.DiscountAmount
            pnlCouponForm.Visible = False
            txtCouponCode.Visible = False
            lnkApplyCoupon.Visible = False
            lnkApplyCoupon.Enabled = False
        Else
            'no session, so probs no coupon
            If (txtCouponCode.Text.Length > 0) Then
                lblCouponMsg.Text = "Invalid Coupon - coupon not found"
            End If
        End If
    End Sub



    Protected Sub AddScheduleDataToCart()

        Dim oXML As XmlDocument = Profile.Cart
        Dim oCart As XmlNode = oXML.SelectSingleNode("/cart")
        Dim scheduleDataView As DataView
        Dim scheduleDataUtility As New CampaignScheduler()

        scheduleDataView = scheduleDataUtility.GetScheduleData2(oCart.OuterXml)

        CartUtility.AddScheduleToCart(scheduleDataView, oCart, DistributionId)

        Profile.Cart = oXML
        Profile.Save()


        'Clean up
        scheduleDataView.Dispose()

    End Sub



    Protected Sub btnGetQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetQuote.Click

        Dim savedQuoteID As Integer = 0                                                             'PK from table
        Dim guidKey As String = System.Guid.NewGuid.ToString                                        'Guid to use to retrieve
        Dim sAutoGenKey As String = ConfigurationManager.AppSettings("AutoGenKey")                  'Licence Key
        Dim docID As Int16 = 0
        Dim theDoc As New Doc()
        Dim purchaseOrderNumber As String = PONumber.Text
        Dim quotePDFPage As String = ""

        'A purchase order was provided so we will show that in the PDF
        If purchaseOrderNumber.Length > 0 Then
            quotePDFPage = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/QuotePDF.aspx?key=" & guidKey & "&po=" & purchaseOrderNumber
        Else
            quotePDFPage = "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & "/QuotePDF.aspx?key=" & guidKey
        End If


        'Staples Act Mgr Site Only
        If SiteID = 95 Then
            If txtStoreNumber.Text.Length = 4 Then
                quotePDFPage += "&store=" & txtStoreNumber.Text
            End If
        End If


        'Save the Cart in Profile into table
        savedQuoteID = SaveQuoteData(guidKey)


        If (TestMode) Then

            Response.Redirect(quotePDFPage)

        'Stream it instead of redirect
        Else

            Dim fileName As String = "EDDM-Quote-" & oCust.CustomerID.ToString() & "-" & DateTime.Now.Ticks & ".pdf"


            'SavedQuoteData returned an ID
            If (savedQuoteID > 0) Then

                'Shape the doc
                theDoc.Rect.Inset(25, 25)
                theDoc.Color.String = "255 255 255"

                docID = theDoc.AddImageUrl(quotePDFPage)

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


                'Stream it
                Dim theData() As Byte = theDoc.GetData()

                Response.Clear()
                Response.ContentType = "application/force-download"
                Response.AddHeader("content-disposition", "attachment;filename=" & fileName)

                Response.AddHeader("content-length", theData.Length.ToString())
                Response.BinaryWrite(theData)

                theDoc.Clear()
                Response.End()

            End If


        End If


    End Sub



    Protected Function SaveQuoteData(guidKey As String) As Integer

        'Will save QuoteData in pnd_SavedCarts

        Dim results As Integer = 0
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim errorMsg As New StringBuilder()
        Dim cart As String = Replace(Profile.Cart.OuterXml, "'", "")
        Dim selectSql As String = "EXEC usp_InsertSavedCart @paramSiteID = " & SiteID & ", @paramUID = '" & Profile.UserName & "', @paramCartData = '" & cart & "', @paramGUID = '" & guidKey & "'"
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)
        Dim sqlReader As SqlDataReader

        Try

            connObj.Open()
            sqlReader = sqlCommand.ExecuteReader()

            If sqlReader.HasRows Then
                Do While sqlReader.Read()
                    results = sqlReader("pnd_SavedCartID")
                Loop
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

            pCheckout.Visible = False
            pnlError.Visible = True
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("Error in SaveQuoteData(). (Step3-Checkout.aspx). Details:<br />" & errorMsg.ToString())


        Finally

            connObj.Close()

        End Try

        Return results



    End Function



    Private Sub SendReceiptEmail(receiptFilePath As String, oOrder As Taradel.OrderHeader)

        '==========================================================================================
        'Staples Sites #78 (Act Mgr) and #95 should only send the receipt email to the account holder (pnd_Customer.EmailAddress) can completely ignore the 
        'Billing Email address provided. This is so the Receipt is sent to the Staples Act Mgr and NOT the customer.

        'Stales Site #91 (Store) should send the receipt email to the Billing Email Address who is actually the customer.

        'All other sites will send the receipt to Billing Email Address OR the Receipt email address - which ever is visible and enabled.
        'If for some reason, the Billing Email address is different than the account holder email address (and not Site 78, 91, 95) then the account holder will get a 
        'CC of the email.

        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

        Dim myLogWriter As New LogWriter()


        Try

            'Needs clean up, rewriting...
            Dim smtpMailServer As New SmtpClient
            Dim emailMsg As New MailMessage()
            Dim msgBody As New StringBuilder()
            Dim receiptFilePDF As New System.Net.Mail.Attachment(receiptFilePath)
            Dim dOrderDate As DateTime = oOrder.Created.Value
            Dim appEnvironment As String = ""



            'Server stuff
            smtpMailServer.UseDefaultCredentials = False
            smtpMailServer.Credentials = New Net.NetworkCredential("dotnetemail@taradel.com", "Hodor!Hodor!")
            smtpMailServer.Port = 587
            smtpMailServer.EnableSsl = True
            smtpMailServer.Host = "smtp.gmail.com"

            'Message properties
            emailMsg = New MailMessage()
            emailMsg.From = New MailAddress(SiteDetails.SysFromEmailAcct)
            msgBody.Append("<html><body>")

            'Send to actual cust or Dev Team based on environment.
            If environmentMode.ToLower() = "dev" Then

                If (SiteDetails.ShowReceiptEmail) Then
                    emailMsg.To.Add(txtEmailReceipt.Text)
                Else
                    emailMsg.To.Add(BillInfo_Email.Text)
                End If

                emailMsg.CC.Add("ITGroup@taradel.com")
                emailMsg.Subject = "DEV ORDER: Your order has been received."
                msgBody.Append("<h2 style='color: red;'>***TEST ORDER***</h2>")

                'EmailUtility.SendOrderConfirmation(SiteID)

            ElseIf environmentMode.ToLower() = "prod" Then

                'At the moment, Sites #78 & #95 (Staples Act Mgr) should never send receipt via email to 'billing info' and should
                'always send to the account holder email address. 
                If (SiteID = 78) Or (SiteID = 95) Then

                    emailMsg.To.Add(oCust.EmailAddress)

                    'Staples Store #91 should send the receipt to the Billing Email Address.
                ElseIf SiteID = 91 Then

                    emailMsg.To.Add(BillInfo_Email.Text)

                Else

                    'If ShowReceiptEmail is enabled, sent to that email address
                    If (SiteDetails.ShowReceiptEmail) Then

                        emailMsg.To.Add(txtEmailReceipt.Text)

                        'Send to BillingEmailAddress by default
                    Else

                        emailMsg.To.Add(BillInfo_Email.Text)

                        'If CCAccountHolderOnReceipt is enabled AND the Billing/Customer email address does NOT match the Account Holder email address, 
                        'then send a copy (CC) to the account holder.
                        If (SiteDetails.CCAccountHolderOnReceipt) Then

                            If BillInfo_Email.Text.ToString.ToLower() <> oCust.EmailAddress.ToLower() Then
                                emailMsg.CC.Add(oCust.EmailAddress)
                            End If

                        End If

                    End If


                End If

                'Go through list of BCCs and add them.
                Dim bccEmails As String = SiteDetails.BCCOrderEmails
                Dim bccEmailList As String() = bccEmails.Split(New Char() {";"c})
                Dim bccEmail As String
                For Each bccEmail In bccEmailList
                    emailMsg.Bcc.Add(bccEmail)
                Next


                emailMsg.Subject = "Your order has been received."

            Else
                emailMsg.To.Add("ITGroup@taradel.com")
                emailMsg.Subject = "DEV ORDER: Your order has been received."
            End If


            emailMsg.IsBodyHtml = True
            emailMsg.Attachments.Add(receiptFilePDF)

            msgBody.Append("<p><strong>Thank for your order!</strong></p>")
            msgBody.Append("<p>Your order has been received and will be sent to our printing department as soon as it is verified.</p>")
            msgBody.Append("<p>Attached is a PDF version of your order for your records.</p>")
            msgBody.Append("<p>&nbsp;</p>")
            msgBody.Append("<p>Thank you!</p>")
            msgBody.Append("</body></html>")

            emailMsg.Body = msgBody.ToString()
            smtpMailServer.Send(emailMsg)
            emailMsg.Dispose()
            smtpMailServer.Dispose()

        Catch ex As Exception
            myLogWriter.RecordInLog("**Something went wrong with sending Receipt Email**")
            myLogWriter.RecordInLog(ex.Message)
        End Try

    End Sub



    Protected Sub InsertListFulfillmentRecord(orderID As Integer)


        Dim insertSQL As New StringBuilder()
        Dim errorMsg As New StringBuilder()
        Dim listKey As String = DistributionUtility.RetrieveReferenceID(DistributionId)
        Dim listMapName As String = ""
        Dim oDist As Taradel.CustomerDistribution = Nothing
        Dim connectString As String = ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString.ToString()
        Dim connectionObj As New SqlConnection(connectString)

        'get Map Name
        oDist = Taradel.CustomerDistributions.GetDistribution(DistributionId)

        If oDist IsNot Nothing Then

            'Map Name
            If Not String.IsNullOrEmpty(oDist.Name) Then
                listMapName = oDist.Name
            End If

        End If

        insertSQL.Append("EXEC usp_InsertCustomerListFulfillments @paramListKey = '" & listKey & "', ")
        insertSQL.Append("@paramUSelectTypeID = " & USelectID & ", ")
        insertSQL.Append("@paramCustomerListName = '" & listMapName & "', ")
        insertSQL.Append("@paramFulfilled = 0, ")
        insertSQL.Append("@paramOrderID = " & orderID & ", ")
        insertSQL.Append("@paramCustomerID = " & oCust.CustomerID)


        Dim insertCommand As New SqlCommand()
        insertCommand.CommandType = CommandType.Text
        insertCommand.CommandText = insertSQL.ToString()
        insertCommand.Connection = connectionObj


        Try

            connectionObj.Open()
            insertCommand.ExecuteNonQuery()

        Catch objException As Exception

            errorMsg.Append("<strong>The following errors occurred:</strong><br /><br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " + objException.Message + "</li>")
            errorMsg.Append("<li>Source: " + objException.Source + "</li>")
            errorMsg.Append("<li>Stack Trace: " + objException.StackTrace + "</li>")
            errorMsg.Append("<li>Target Site: " + objException.TargetSite.Name + "</li>")
            errorMsg.Append("<li>SQL: " + insertSQL.ToString() + "</li>")
            errorMsg.Append("</ul>")

            pnlError.Visible = True
            litErrorMessage.Text = errorMsg.ToString()

        Finally
            connectionObj.Close()
        End Try

    End Sub



    Private Sub RecordStoreNumber(tagGroupID As Integer, orderID As Integer, storeNumber As String)


        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim errorMsg As New StringBuilder()
        Dim insertSql As String = "INSERT INTO OrderTag (OrderTagGroupID, OrderID, Tag) VALUES (" & tagGroupID & ", " & orderID & ", '" & storeNumber & "')"
        Dim connectionObj As New SqlConnection(connectionString)
        Dim sqlCommand As New SqlCommand(insertSql, connectionObj)
        Dim logger As New LogWriter()


        sqlCommand.CommandType = System.Data.CommandType.Text
        sqlCommand.CommandText = insertSql
        sqlCommand.Connection = connectionObj


        Try

            connectionObj.Open()
            sqlCommand.ExecuteNonQuery()

            logger.RecordInLog("[Step3-Checkout.aspx RecordStoreNumber] lookupStoreNumberSql: " & insertSql)


        Catch objException As Exception

            errorMsg.Append("The following errors occurred" & Environment.NewLine)
            errorMsg.Append("Message: " & objException.Message & Environment.NewLine)
            errorMsg.Append("Source: " & objException.Source & Environment.NewLine)
            errorMsg.Append("Stack Trace: " & objException.StackTrace & Environment.NewLine)
            errorMsg.Append("Target Site: " & objException.TargetSite.Name & Environment.NewLine)
            logger.RecordInLog("[Step3-Checkout ERROR] Error in RecordStoreNumber: " & Environment.NewLine & errorMsg.ToString())
            EmailUtility.SendAdminEmail("There was an error on Step3-Checkout (RecordStoreNumber). Check the EDDM-Log.")

        Finally
            connectionObj.Close()
        End Try


    End Sub






    'Page Builders
    Protected Sub ShowHidePageElements()


        'Show the correct Product panels
        If (EDDMMap) Then
            pnlEddmProduct.Visible = True
            pnlEDDMDrops.Visible = True
            pnlAddressedProduct.Visible = False
            pnlAddressedDrops.Visible = False
        End If

        If (GeneratedAddressedList) Or (UploadedAddressedList) Then
            pnlEddmProduct.Visible = False
            pnlEDDMDrops.Visible = False
            pnlAddressedProduct.Visible = True
            pnlAddressedDrops.Visible = True
        End If

        If (TMCMap) Then
            pnlEddmProduct.Visible = True
            pnlEDDMDrops.Visible = True
            pnlAddressedProduct.Visible = True
            pnlAddressedDrops.Visible = True
        End If


        'New Mover
        If (NewMoverSelected) Then
            pnlNewMovers.Visible = True
            BuildNewMoverDisplay()
        End If


        'Email Campaign
        If (EmailCampaignSelected) Then
            pnlEmails.Visible = True
            BuildTargetedEmailsDisplay()
        End If

        'Addressed Add-Ons
        If (AddressedAddOnCampaignSelected) Then
            pnlAddressedAddOns.Visible = True
            BuildAddressedAddOnsDisplay()
        End If



        'Extra Copies
        If ExtraCopies > 0 Then
            pnlExtraPcs.Visible = True
            BuildExtraCopiesDisplay()
        End If


        'Professional Design
        If (IsProfessionalDesign) Then
            pnlDesignFee.Visible = True
            lblDesignFee.Text = DesignFee.ToString("C")
        End If


        'Drop Fee
        If (HasDropFee) Then
            pnlNumOfDrops.Visible = True
            BuildDropFeeDisplay()
        End If


        'Show Coupon form if needed
        If (HasActiveCoupons) Then
            pnlCouponDiscount.Visible = True
        End If


        'Sales Tax 
        If (SalesTax) > 0 Then
            pnlSalesTax.Visible = True
            BuildSalesTaxDisplay()
        End If



        'Hide or Show items defined in the SiteDetails table. 
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

        If Not (SiteDetails.ShowReceiptEmail) Then
            pnlReceiptEmail.Attributes.Add("class", "hidden")
            txtEmailReceipt.Enabled = False
            rfvEmailAddress.Enabled = False
            revEmailAddress.Enabled = False
        End If

        If (SiteDetails.AllowGetQuote) Then
            btnGetQuote.Visible = True
        End If

        If Not (SiteDetails.ShowBillingInfo) Then
            pnlBillingInfo.Attributes.Add("class", "hidden")
        End If


    End Sub



    Protected Sub BuildNewMoverDisplay()

        Dim oCart As XmlDocument = Profile.Cart

        'get Price
        Dim newMoverNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='New Mover Postcard']")
        Dim nmPrice As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(newMoverNode, "Price"))
        litNewMoverPrice.Text = nmPrice.ToString("C")

        'get Description
        Dim nmQTY As Integer = Convert.ToInt32(xmlhelp.ReadAttribute(newMoverNode, "Quantity"))
        Dim nmPPP As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(newMoverNode, "PricePerPiece"))
        litNewMoverDescription.Text = "1st Month: " & nmQTY & " postcards @ " & nmPPP.ToString("C")

        'Turn on message in credit card panel
        pnlNewMoverInfo.Visible = True


    End Sub



    Protected Sub BuildTargetedEmailsDisplay()

        Dim oCart As XmlDocument = Profile.Cart
        Dim targetedEmailsSchedObj As New CampaignScheduler()

        'Get Price
        Dim emailNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='Targeted Emails']")
        Dim emailPrice As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(emailNode, "Price"))
        litEmailPrice.Text = emailPrice.ToString("C")

        'Get Description
        Dim emailQTY As Integer = Convert.ToInt32(xmlhelp.ReadAttribute(emailNode, "Quantity").ToString().Replace(",", ""))
        '1/13/2016 new
        '1/13/2016 rs change - divide out the email impressions which is always 3
        '1/13/2016 refactor to use actual number of email impresstions
        emailQTY = emailQTY / 3
        '1/13/2016 end new


        Dim emailPPP As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(emailNode, "PricePerPiece"))
        litEmailDescription.Text = emailQTY.ToString("N0") & " Targeted Emails emailed " & numEmailBlasts.ToString() & " times"


    End Sub



    Protected Sub BuildAddressedAddOnsDisplay()
        Dim oCart As XmlDocument = Profile.Cart
        ''Dim targetedEmailsSchedObj As New CampaignScheduler()

        'Get Price
        Dim addressedAddOnsNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='AddressedMail AddOn']")
        Dim addressedAddOnPrice As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(addressedAddOnsNode, "Price"))
        litAddressedAddOnsPrice.Text = addressedAddOnPrice.ToString("C")

        'Get Price Per Piece
        Dim addressedAddOnPricePerPiece As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(addressedAddOnsNode, "PricePerPiece"))


        Dim addressedAddOnQTY As Integer = Convert.ToInt32(xmlhelp.ReadAttribute(addressedAddOnsNode, "Quantity").ToString().Replace(",", ""))


        Dim addressedAddOnsNode2 As XmlNode = addressedAddOnsNode.SelectSingleNode("Drops/Drop")
        Dim numAddressedAddOns As Integer = Convert.ToInt32(xmlhelp.ReadAttribute(addressedAddOnsNode2, "Multiple").ToString())

        Dim addressedAddOnPPP As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(addressedAddOnsNode, "PricePerPiece"))
        litAddressedAddOnsDescription.Text = addressedAddOnQTY.ToString("N0") & " @ " & addressedAddOnPricePerPiece.ToString("C")




    End Sub



    Protected Sub BuildExtraCopiesDisplay()

        Me.xOrderCalc = GetNewOrderCalcObj()
        litExtraPcs.Text = Me.xOrderCalc.ExtraPieces.ToString("N0") & " @ " & TaradelReceiptUtility.OrderCalculator.FormatAsCurrency3(Me.xOrderCalc.ExtraPiecesPricePerPiece)

        Dim shipTo As String = Me.xOrderCalc.ExtraPiecesAddress1

        If (Me.xOrderCalc.ExtraPiecesAddress2 IsNot Nothing) Then
            If Not String.IsNullOrEmpty(Me.xOrderCalc.ExtraPiecesAddress2) Then
                shipTo += "<br/>" + Me.xOrderCalc.ExtraPiecesAddress2
            End If
        End If

        shipTo += "<br/>" + Me.xOrderCalc.ExtraPiecesAddressCityStateZip

        litShipTo.Text = shipTo

        lblShippingEstimate.Text = Me.xOrderCalc.ExtraPiecesPrice.ToString("C")

        xOrderCalc = Nothing

    End Sub



    Protected Sub BuildDropFeeDisplay()

        Me.xOrderCalc = GetNewOrderCalcObj()

        litNumOfDrops.Text = (Me.xOrderCalc.NumOfDrops - 1).ToString() & " @ $99"
        lblNumOfDrops.Text = Me.xOrderCalc.DropFee.ToString("C")

        xOrderCalc = Nothing

    End Sub



    Protected Sub BuildSalesTaxDisplay()

        Me.xOrderCalc = GetNewOrderCalcObj()

        lblSalesTax.Text = Me.xOrderCalc.SalesTax.ToString("C")
        lblSalesTaxMessage.Text = Me.xOrderCalc.SalesTaxMessage


        xOrderCalc = Nothing

    End Sub



    Protected Sub BuildPaymentPanel()

        'Response.Write(USelectID & "<br />")
        'Response.Write(AddressedMap.ToString & "<br />")
        'Response.End()

        Me.xOrderCalc = GetNewOrderCalcObj()
        Dim oCart As XmlDocument = Profile.Cart
        Dim productNode As XmlNode = Nothing
        Dim shippingTotal As Decimal = ((EDDMShipPrice + AddressedShipPrice) + (RetrieveNumberOfItemsInOrder()) * PostageRate)
        Dim combinedSubTotal As Decimal = 0
        Dim eddmTotal As Decimal = 0
        Dim addressedTotal As Decimal = 0
        Dim combinedTotal As Decimal = 0


        If EDDMMap Then
            productNode = oCart.SelectSingleNode("//Product[@Type='EDDM']")
            eddmTotal = xmlhelp.ReadAttribute(productNode, "Price")
        End If


        If (GeneratedAddressedList) Or (UploadedAddressedList) Then
            productNode = oCart.SelectSingleNode("//Product[@Type='AddressedList']")
            addressedTotal = xmlhelp.ReadAttribute(productNode, "Price")
        End If

        combinedSubTotal = ((eddmTotal + addressedTotal + DesignFee + DropFee) - CouponDiscount)
        combinedTotal = (combinedSubTotal + SalesTax)


        litCCFullPayBalanceDue.Text = (Me.xOrderCalc.OrderTotal).ToString("C")
        litECFullPayBalanceDue.Text = (Me.xOrderCalc.OrderTotal).ToString("C")
        litPOFullPayBalanceDue.Text = (Me.xOrderCalc.OrderTotal).ToString("C")
        litCCFinancePayBalanceDue.Text = Me.xOrderCalc.FinanceFirstPayment.ToString("C")


        Dim financeFee As Decimal = 0
        Dim deposit As Decimal = 0
        Dim balance As Decimal = 0
        Dim financePayment As Decimal = 0
        Dim financePymntFee As Decimal = 0


        If DistributionId > 0 Then

            'Need to adjust deposit amount so that finance amount can be equally split among all future payments
            deposit = Me.xOrderCalc.FinanceFirstPayment
            balance = combinedTotal - deposit

            financePayment = Me.xOrderCalc.FinancePaymentAmount.ToString("C")

            Dim sSched As String = "Weekly"
            Dim oSched As XmlNode = productNode.SelectSingleNode("Attribute[@Name='Drop Schedule']")
            Dim weekSchedule As String = appxCMS.Util.Xml.ReadAttributeValue(oSched, "ValueName").ToLower()
            Dim iAddDays As Integer = 7
            Dim oFirstDrop As XmlNode = Nothing

            If (weekSchedule = "0 weeks") Then
                iAddDays = 0 'switch for payment scheduling...was 0 
            ElseIf (weekSchedule = "1 weeks") Then
                iAddDays = 7
            ElseIf (weekSchedule = "2 weeks") Then
                iAddDays = 14
            ElseIf (weekSchedule = "3 weeks") Then
                iAddDays = 21
            Else
                iAddDays = 28
            End If

            oFirstDrop = productNode.SelectSingleNode("Drops/Drop[1]")


            Dim testDate As DateTime = DateTime.Parse(appxCMS.Util.Xml.ReadAttribute(oFirstDrop, "Date"))

            If oSched IsNot Nothing Then
                sSched = xmlhelp.ReadAttribute(oSched, "Value")
            End If

            Dim dEarliest As DateTime = testDate.AddDays(-7)


            'new code for single drop payment plan 9/5/2014
            If iAddDays = 0 Then
                Dim timeSpan As TimeSpan = dEarliest.Subtract(DateTime.Now())
                iAddDays = timeSpan.Days
                If (iAddDays < 7) Then
                    iAddDays = 7
                End If
            End If


            'Get the finance payment charge fee
            financePymntFee = appxCMS.Util.AppSettings.GetDecimal("FinancePymntFee")
            If financePymntFee = 0 Then
                financePymntFee = 25
            End If


            Dim oAdditional As New SortedList(Of DateTime, Decimal)
            For i As Integer = 0 To NumOfDrops - 1
                oAdditional.Add(dEarliest.AddDays(iAddDays * i), Me.xOrderCalc.FinancePaymentAmount.ToString("C"))
            Next

            rCCPayments.DataSource = oAdditional

            rCCPayments.DataBind()

            litCCFinanceTotal.Text = Me.xOrderCalc.FinanceTotal.ToString("C")
            financeFee = (financePymntFee * NumOfDrops)

            oFinanceAmounts = New Taradel.OrderAmounts
            oFinanceAmounts.SubTotal = Me.xOrderCalc.FinanceTotal
            oFinanceAmounts.OrderAmount = Me.xOrderCalc.FinanceTotal
            oFinanceAmounts.Deposit = Me.xOrderCalc.FinanceFirstPayment
            oFinanceAmounts.TotalPayments = Me.xOrderCalc.NumOfDrops
            oFinanceAmounts.FirstPaymentDate = dEarliest
            oFinanceAmounts.IntroPayments = 0
            oFinanceAmounts.PaymentInterval = iAddDays
            oFinanceAmounts.MonthlyPaymentAmount = Me.xOrderCalc.FinancePaymentAmount
        End If

        'Page level property.
        oAmounts = New Taradel.OrderAmounts
        oAmounts.SubTotal = Me.xOrderCalc.Subtotal


        If Me.xOrderCalc.CouponCode IsNot Nothing Then
            oAmounts.CouponCode = Me.xOrderCalc.CouponCode
            oAmounts.CouponDiscount = Me.xOrderCalc.CouponDiscount
        Else
            oAmounts.CouponCode = ""
            oAmounts.CouponDiscount = 0
        End If


        If DistributionId > 0 Then
            oAmounts.Shipping = oQuote.ShipPrice
        End If

        oAmounts.OrderAmount = Me.xOrderCalc.OrderTotal
        oAmounts.SalesTax = Me.xOrderCalc.SalesTax
        oAmounts.SalesTaxRate = Me.xOrderCalc.SalesTaxRate
        oAmounts.SalesTaxJurisdiction = Me.xOrderCalc.SalesTaxState



        xOrderCalc = Nothing


    End Sub



    Protected Sub BuildCampaignSchedule(productType As String)


        'Get the schedule DataView and bind it to the repeater.
        Dim oCart As XmlDocument = Profile.Cart
        Dim scheduleDataView As DataView
        Dim scheduleDataUtility As New CampaignScheduler()

        scheduleDataView = scheduleDataUtility.GetScheduleData2(oCart.OuterXml)
        rptSchedule.DataSource = scheduleDataView
        rptSchedule.DataBind()

        scheduleDataView.Dispose()


    End Sub



    Protected Sub ShowEmpty()
        pEmpty.Visible = True
        pCheckout.Visible = False
    End Sub



    Protected Sub BuildBillingSection()

        'Addtional Site data.  Build SiteDetails Obj
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


        If (SiteDetails.PrefillBillingInfo) Then

            If oCust IsNot Nothing Then

                '-- Check that the customer record is complete. It not, force them to complete it.
                If String.IsNullOrEmpty(oCust.Address1) Then
                    Response.Redirect("~/MyAccount/account-profile.aspx")
                End If


                Dim oCustBillInfo As List(Of Taradel.CustomerBillingInfo) = Taradel.Customers.GetBillingAddresses(oCust.CustomerID)
                If oCustBillInfo.Count > 0 Then

                    Dim oBill As Taradel.CustomerBillingInfo = oCustBillInfo.FirstOrDefault
                    BillInfo_FirstName.Text = oBill.FirstName
                    BillInfo_LastName.Text = oBill.LastName
                    BillInfo_Company.Text = oBill.Company
                    BillInfo_Address1.Text = oBill.Address1
                    BillInfo_Address2.Text = oBill.Address2
                    BillInfo_City.Text = oBill.City
                    BillInfo_State.SelectedValue = oBill.State
                    BillInfo_Zip.Text = oBill.PostalCode
                    BillInfo_Phone.Text = oBill.Phone
                    BillInfo_Email.Text = oBill.Email
                    BillInfo_Comments.Text = oBill.Comments
                    txtEmailReceipt.Text = oBill.Email


                Else
                    BillInfo_FirstName.Text = oCust.FirstName
                    BillInfo_LastName.Text = oCust.LastName
                    BillInfo_Company.Text = oCust.Company
                    BillInfo_Address1.Text = oCust.Address1
                    BillInfo_Address2.Text = oCust.Address2
                    BillInfo_City.Text = oCust.City
                    BillInfo_State.SelectedValue = oCust.State
                    BillInfo_Zip.Text = oCust.ZipCode
                    BillInfo_Phone.Text = oCust.PhoneNumber
                    BillInfo_Email.Text = oCust.EmailAddress
                    txtEmailReceipt.Text = oCust.EmailAddress

                End If


            Else

                pCheckout.Visible = False
                pnlError.Visible = True
                litErrorMessage.Text = "Sorry but we were unable to locate your customer record. Please check that you are logged in and try again."
                Exit Sub

            End If


            'Set fields to be empty.
        Else

            BillInfo_FirstName.Text = String.Empty
            BillInfo_LastName.Text = String.Empty
            BillInfo_Company.Text = String.Empty
            BillInfo_Address1.Text = String.Empty
            BillInfo_Address2.Text = String.Empty
            BillInfo_City.Text = String.Empty
            BillInfo_State.SelectedIndex = 0
            BillInfo_Zip.Text = String.Empty
            BillInfo_Phone.Text = String.Empty
            txtEmailReceipt.Text = String.Empty
            BillInfo_Email.Text = String.Empty

        End If



        'Show and enable Billing Email or not?
        If SiteDetails.ShowBillingEmail Then
            pnlBillingEmail.Attributes.Remove("class")
            BillInfo_Email.Enabled = True
        Else
            pnlBillingEmail.Attributes.Add("class", "hidden")
            BillInfo_Email.Enabled = False
            rfvBillingEmail.Enabled = False
            revBillingEmail.Enabled = False
        End If


        'Billing Info Section
        litBillingPanelHeader.Text = SiteUtility.GetStringResourceValue(SiteID, "BillingPanelHeader")
        litBillingPanelText.Text = SiteUtility.GetStringResourceValue(SiteID, "BillingPanelText")

        'Job Comments
        litJobComments.Text = SiteUtility.GetStringResourceValue(SiteID, "JobCommentsMessage")

        'Job Comments Header
        litJobCommentsHeader.Text = SiteUtility.GetStringResourceValue(SiteID, "JobCommentsHeader")


        'Populate Field Validators
        rfvBillInfoFirstName.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingFirstNameMsg")
        rfvLastName.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingLastNameMsg")
        rfvCompany.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingCompanyMsg")
        rfvBillInfoAddress1.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingAddress1Msg")
        rfvBillInfoCity.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingCityMsg")
        rfvPostalCode.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingZipCodeMsg")
        rfvBillPhone.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingPhoneMsg")
        rfvBillingEmail.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingEmailMsg")
        revBillingEmail.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingValidEmailMsg")
        rfvBillingState.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "BillingStateMsg")

    End Sub



    Protected Sub BuildPaymentSelections()


        'PO Business Logic-------------------------------
        Dim enableCreditCard As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Payment", "EnableCreditCard")
        Dim enableCheck As Boolean = False
        Dim enablePOSetting As String = Taradel.CustomerDataSource.GetCustomerProperty(oCust.CustomerID, "EnablePO").ToLower
        Dim enablePO As Boolean = False


        'Set the PO Flag
        If enablePOSetting = "1" Or enablePOSetting = "yes" Or enablePOSetting = "true" Then
            enablePO = True
        End If


        'Check their settings for core-Taradel PO Enabled flag
        If oCust.AffiliateID = 0 Then
            Dim oSettings As Taradel.CustomerSetting = Taradel.CustomerDataSource.GetCustomerSettings(oCust.CustomerID)

            If oSettings IsNot Nothing Then
                If oSettings.EnablePO.Value Then
                    enablePO = True
                End If
            End If

        End If


        'We have to leave at least one payment method enabled
        If Not enableCreditCard And Not enableCheck Then
            enablePO = True
        End If


        If Not enableCreditCard Then
            Dim oCC As ListItem = radPaymentType.Items.FindByValue("CC")
            If oCC IsNot Nothing Then
                radPaymentType.Items.Remove(oCC)
            End If
        End If


        If Not enableCheck Then
            Dim oCheck As ListItem = radPaymentType.Items.FindByValue("EC")
            If oCheck IsNot Nothing Then
                radPaymentType.Items.Remove(oCheck)
            End If
        End If


        If Not enablePO Then
            Dim oPO As ListItem = radPaymentType.Items.FindByValue("PO")
            If oPO IsNot Nothing Then
                radPaymentType.Items.Remove(oPO)
            End If
        End If
        'End of PO Business Logic-----------------------------




        'Payment type selections business logic---------------
        radPaymentType.ClearSelection()


        Dim sSelPayType As String = radPaymentType.Items(0).Value
        radPaymentType.Items(0).Selected = True

        If Not Page.IsPostBack Then
            Dim oSelPayPnl As Panel = Nothing
            Select Case sSelPayType.ToLower
                Case "cc"
                    oSelPayPnl = pPayCC
                Case "ec"
                    oSelPayPnl = pPayEC
                Case "po"
                    oSelPayPnl = pPayPO
            End Select

            If oSelPayPnl IsNot Nothing Then
                oSelPayPnl.Style.Remove("display")
            End If

        End If

        If radPaymentType.Items.Count = 1 Then
            pPayMeth.Style.Add("display", "none")
        End If
        'End of Payment type selections business logic---------------




        'PO Label related controls
        litPOLabel.Text = SiteUtility.GetStringResourceValue(SiteID, "POLabel")
        litPOPanelText.Text = SiteUtility.GetStringResourceValue(SiteID, "POPanelHeader")
        rfvPONumber.ErrorMessage = SiteUtility.GetStringResourceValue(SiteID, "PONumErrorMessage")




        'Addtional Site data.  Build SiteDetails Obj
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


        'Staples is the only using this right now
        If SiteID = 78 Then
            revPONumber.Enabled = True
        End If


        If SiteDetails.RequirePONumber = False Then
            revPONumber.Enabled = False
            rfvPONumber.Enabled = False
        End If




    End Sub



    Protected Sub BuildAddressedListDetails(numPcs As Integer)

        'To get Drop Dates
        Dim oCart As XmlDocument = Profile.Cart

        litAddressedPcs.Text = CartUtility.GetDropQtyList(oCart, "AddressedList")
        litAddressedDropDates.Text = CartUtility.GetDropDatesList(oCart, "AddressedList")

        If (GeneratedAddressedList) Then
            litDemographicFilters.Text = AddressedListUtility.GetSavedFilters(DistributionId)
        Else
            litDemographicFilters.Text = "(not applicable to uploaded lists)"
        End If


    End Sub



    Private Sub BuildOrderSteps()

        'if this is an EDDM Map
        If (EDDMMap) Then

            OrderSteps.numberOfSteps = 5
            OrderSteps.step1Text = "1) Target Area"
            OrderSteps.step1Url = "/Step1-Target.aspx"
            OrderSteps.step1State = "visited"
            OrderSteps.step1Icon = "fa-map-marker"

            OrderSteps.step2Text = "2) Select Routes"
            OrderSteps.step2Url = "/Step1-Target.aspx"
            OrderSteps.step2State = "visited"
            OrderSteps.step2Icon = "fa-truck"

            OrderSteps.step3Text = "3) Choose Product"
            OrderSteps.step3Url = "/Step1-TargetReview.aspx?distid=" & DistributionId
            OrderSteps.step3State = "visited"
            OrderSteps.step3Icon = "fa-folder"

            OrderSteps.step4Text = "4) Define Delivery"
            OrderSteps.step4Url = "/Step2-ProductOptions.aspx?productid=" & ProductID & "&distid=" & DistributionId & "&baseid=" & BaseProductID
            OrderSteps.step4State = "visited"
            OrderSteps.step4Icon = "fa-envelope"

            OrderSteps.step5Text = "5) Check Out"
            OrderSteps.step5Url = "/Step3-Checkout.aspx"
            OrderSteps.step5State = "current"
            OrderSteps.step5Icon = "fa-credit-card"

        End If



        'If this is an AddressedList (user generated)
        If (GeneratedAddressedList) Then

            OrderSteps.numberOfSteps = 5
            OrderSteps.step1Text = "1) Define Area"
            OrderSteps.step1Url = "/Addressed/Step1-BuildYourList.aspx"
            OrderSteps.step1State = "visited"
            OrderSteps.step1Icon = "fa-map-marker"

            OrderSteps.step2Text = "2) Define Customers"
            OrderSteps.step2Url = "/Addressed/Step1-BuildYourList.aspx"
            OrderSteps.step2State = "visited"
            OrderSteps.step2Icon = "fa-user"

            OrderSteps.step3Text = "3) Choose Product"
            OrderSteps.step3Url = "/Step1-TargetReview.aspx?distid=" & DistributionId
            OrderSteps.step3State = "visited"
            OrderSteps.step3Icon = "fa-folder"

            OrderSteps.step4Text = "4) Define Delivery"
            OrderSteps.step4Url = "/Step2-ProductOptions.aspx?productid=" & ProductID & "&distid=" & DistributionId & "&baseid=" & BaseProductID
            OrderSteps.step4State = "visited"
            OrderSteps.step4Icon = "fa-envelope"

            OrderSteps.step5Text = "5) Check Out"
            OrderSteps.step5Url = "/Step3-Checkout.aspx"
            OrderSteps.step5State = "current"
            OrderSteps.step5Icon = "fa-credit-card"

        End If



        If (UploadedAddressedList) Then

            OrderSteps.numberOfSteps = 4
            OrderSteps.step1Text = "1) Upload List"
            OrderSteps.step1Url = "/Addressed/#"
            OrderSteps.step1State = "visited"
            OrderSteps.step1Icon = "fa-upload"

            OrderSteps.step2Text = "2) Choose Product"
            OrderSteps.step2Url = "/Step1-TargetReview.aspx?distid=" & DistributionId
            OrderSteps.step2State = "visited"
            OrderSteps.step2Icon = "fa-folder"

            OrderSteps.step3Text = "3) Define Delivery"
            OrderSteps.step3Url = "/Step2-ProductOptions.aspx?productid=" & ProductID & "&distid=" & DistributionId & "&baseid=" & BaseProductID
            OrderSteps.step3State = "visited"
            OrderSteps.step3Icon = "fa-envelope"

            OrderSteps.step4Text = "4) Check Out"
            OrderSteps.step4Url = "/Step3-Checkout.aspx"
            OrderSteps.step4State = "current"
            OrderSteps.step4Icon = "fa-credit-card"

        End If

    End Sub



    Private Sub BuildPageHeader()

        'Set Page Header control
        If Not (SiteDetails.UseRibbonBanners) Then
            PageHeader.headerType = "simple"
            PageHeader.simpleHeader = "Review Your Order"
        Else
            PageHeader.headerType = "partial"
            PageHeader.mainHeader = "My Order"
            PageHeader.subHeader = "Review Your Order"
        End If

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
        litHasActiveCoupons.Text = HasActiveCoupons.ToString().ToUpper()
        litEDDMMap.Text = EDDMMap.ToString().ToUpper()
        litGeneratedAddressedList.Text = GeneratedAddressedList.ToString().ToUpper()
        litUploadedAddressedList.Text = UploadedAddressedList.ToString().ToUpper()
        litTMCMap.Text = TMCMap.ToString().ToUpper()
        litPostageRate.Text = PostageRate.ToString()
        litUseOwnGateway.Text = UseOwnGateWay.ToString().ToUpper()
        litMerchantId.Text = MerchantID.ToString()
        litTransactionID.Text = TransactionID.ToString()
        litNewMoverSelected.Text = NewMoverSelected.ToString().ToUpper()
        litEmailCampaignSelected.Text = EmailCampaignSelected.ToString().ToUpper()
        litAddressedAddOnSelected.Text = AddressedAddOnCampaignSelected.ToString().ToUpper()
        litExtraCopies.Text = ExtraCopies.ToString().ToUpper()
        litIsProfessionalDesign.Text = IsProfessionalDesign.ToString().ToUpper()
        litDesignFee.Text = DesignFee.ToString()
        litHasDropFee.Text = HasDropFee.ToString().ToUpper()
        litDropFee.Text = HasDropFee.ToString()
        litSalesTax.Text = SalesTax.ToString()
        litEDDMShipPrice.Text = EDDMShipPrice.ToString()
        litAddressedShipPrice.Text = AddressedShipPrice.ToString()
        litCouponDiscount.Text = CouponDiscount.ToString()
        litNumOfDrops2.Text = NumOfDrops.ToString()
        litProductType.Text = ProductType2.ToString()

    End Sub



    Protected Sub DebugProperties(info() As System.Reflection.PropertyInfo, o As Object, objectName As String)
        'Response.Write(objectName)
        'Response.Write("<br/>")
        'For Each i In info
        '    Try
        '        If i.CanRead Then
        '            If Not IsNothing(i) Then
        '                Response.Write(i.Name.ToString() & ":" & i.GetValue(o, Nothing).ToString())
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Response.Write(i.Name.ToString() & ": Exception")
        '    End Try
        '    Response.Write("<br/>")
        'Next
    End Sub



    Protected Sub CheckForDebug(orderCalc As TaradelReceiptUtility.OrderCalculator)


        Dim dt As DataTable = TaradelReceiptUtility.LogOrderCalculator(orderCalc)
        Dim dtView As New DataView(dt)
        dtView.Sort = "Name"
        gvOrderCalc.DataSource = dtView
        gvOrderCalc.DataBind()
        gvOrderCalc.Visible = True

        Dim dtProducts As DataTable = TaradelReceiptUtility.LogOrderCalculatorProducts(orderCalc.LstProducts)
        gvProducts.DataSource = orderCalc.LstProducts.ToList()
        gvProducts.DataBind()
        gvProducts.Visible = True

        If Session("sesCampaignSchedule") Is Nothing Then
            litCampaignSchedule.Text = "No schedule in session"
        Else
            litCampaignSchedule.Text = "Schedule added to session"
        End If



        'To show or not to show....
        If (TestMode) Then
            gvOrderCalc.Visible = True
        Else
            If Not (String.IsNullOrEmpty(Request.QueryString("debug"))) Then
                If Request.QueryString("debug") = "hodor" Then
                    gvOrderCalc.Visible = True
                End If
            End If
        End If


    End Sub





End Class
