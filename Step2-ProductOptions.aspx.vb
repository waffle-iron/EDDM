Imports System.Collections.Generic
Imports System.Linq
Imports Taradel.EF
Imports System.IO
Imports System.Net
Imports System.Xml
Imports log4net
Imports System.Net.Mail
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports WebSupergoo.ABCpdf8




'=========================================================================================================================
'   NOTES:
'       The 'prodopt' CSS Class:
'       The javascript function UpdatePriceQuote(), located in the CampaignOverview.js and used by the CampaignOverview.ascx control, 
'       looks for a CSS class in the ddlProdOpts control called 'prodopt'. This is a creative workaround to get the value from the 
'       'optcatid' attribute contained in the drop down list and pass that value to the PrintRateQuote.ashx handler.  This dummy css
'       class of 'prodopt' acts as a tag and lets the javascript pick out the needed value (which is NOT the SelectedItem value of the
'       drop down.  In short, LEAVE this 'prodopt' css class (which has no actual css rules) applied to the ddlProdOpt control. Removing it
'       will effect the accuracy of the handler's return.
'
'       Hidden Fields:
'       Because many values can change on the client side (ie Launch Date) which are difficult to determine at the page 
'       build, many hidden fields and hidden textboxes are built in the page.  The hidden fields are mostly used for
'       client side manipulations - updating the Overview panel, showing and hidden form elements, picking Templates, etc.
'       Hidden fields do NOT hold state so the server can not see the new value(s) once they client side (jquery) has changed
'       them.  Hidden TEXTBOXES such as (txtLaunchWeek) DO hold state are essential for this page to work properly.
'
'       Job Names:
'       The pnlJobNames panel & section is recycled from the previous version and hidden from user.  This may be obsolete
'       logic but it does seem to be required for the SaveToCart to work.  It is put in place for the time being. 4/16/2015.
'
'
'       WISH LIST:
'       *Add Validation Summary control to Address fields.
'
'       UPDATES:
'       4/30/2015 - Implemented a javascript/jquery method to inform customer about changing products instead of using
'       a generic alert.  DropDownList no longer postsback automatically but instead, calls a javascript function to 
'       redirect the user to a new version of this page.  (new querystring params).
'
'       5/18/2015 - TMC/Blended and AddressedList Distributions Types needed to be incorporated into this process.
'       EDDM Distribution Types (USelectID 1) we already functioning as needed but this page needed to allow for 
'       USelectID types of 5, 6, and 7.
'       
'       USelectID Types 5 & 6 are stand alone AddressedList campaigns - however the USelectID of 7, the TMC "Blended"
'       contains both EDDM and AddressedLists.  Furthermore, the data is stored in a different table (SavedListSelection)
'       AND is in a different format than USelectID of 1.
'
'       Sample of USelectID 1:
'       {"Name":"23005R001","ZipCode":"23005","FriendlyName":"ASHLAND, VA","Residential":"590","Business":0,"POBoxes":0,"Total":"59000","Notes":""}
'
'       Sample of USelectID 5,6, and 7:
'       {"TargetPercent":8.51063829787234,"GeocodeRef":"23060R044","City":"GLEN ALLEN","State":"VA","EDDMTotal":687,"AddressedMatches":58,"RouteType":"Addressed","RouteCount":58,"Selected":true},
'
'       The original Taradel.CustomerDistributions.GetSelections Function is not designed to consume 'selections' of USelectIDs 5, 6, 7 
'       so this data needs to be processed manually.
'
'       The standard EDDM Multiple Impression/Drop Logic accounts for PO Boxes and Businesses. It assumed that TMC/Blended campaigns will not 
'       use POBoxes or Businesses so that logic is excluded.
'
'       5/19/2015 - Added functionality to extract selected demographics and adds to Profile.Cart (XML).
'
'       5/26/2015 - When the page is used to display and calculate a TMC/Blended Campaign, it is getting it's calculations and quotes
'       based on the AddressedList WL ProductID (ex: 736).  Since the TMC type of campaign is actually a blending of two products with 
'       TWO DIFFERENT Price Rates, we needed to also make a call and get price and data for the 'EDDM' counterpart product (ex: 216).  
'       Since there is no direct relationship established between two BaseProductIDs, one needed to be created and stored in a SQL Table.
'       The stored procedure 'usp_GetEddmBaseProductIDForListProduct' will look up and return the EDDM matching BaseProductID.  Having this
'       lets us call the PrintRateQuote two times for accurate PricePerPiece rates.  Once for the EDDM product and once for the AddressedList
'       product.
'
'       TMC Distribution:
'       ProductID 736 - WL Product ID for an AddressedList 6x11 Postcard.
'       BaseProductID 241
'       EDDM Counterpart BaseProductID - 216
'
'       7/27/2015 - Added a ProductCOnfiguration 'DisableTemplates' to CMS Admin.  Allows Taradel to disable the WL from showing/allowing
'       Templates as an option.  This only leave Upload Your Own and Professional Design as the available choices.
'
'=========================================================================================================================


Partial Class Step2_ProductOptions
    Inherits appxCMS.PageBase

    'FIELDS==================================================================================================================
    'Services/Servers
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected sTemplateServerHost As String = appxCMS.Util.AppSettings.GetString("TemplateServerHost")


    'Page/App
    Protected SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
    Protected maxDrops As Integer = appxCMS.Util.CMSSettings.GetInteger("Postal", "MaxDrops")
    Protected minAddressedListDropQTY As Integer = 1000


    'Map/Distribution
    Protected USelectMethod As Taradel.USelectMethod = Nothing
    Protected USelectProduct As Taradel.USelectProductConfiguration = Nothing


    'Product
    Private oProd As Taradel.WLProduct = Nothing
    Protected aOptValues As New List(Of KeyValuePair(Of String, Integer))
    Private aProps As New Hashtable
    Protected xPricePerPiece As Decimal = 0


    'Customer
    Protected oCust As Taradel.Customer = Nothing


    'Flags
    Protected bMarketingUpsell As Boolean = True
    Protected showJobsPanel As Boolean = False



    'Properties==================================================================================================================
    Protected ReadOnly Property UserDistributionId As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("distid")
        End Get
    End Property


    Protected ReadOnly Property Distribution As Taradel.CustomerDistribution
        Get
            Return Taradel.CustomerDistributions.GetDistribution(UserDistributionId)
        End Get
    End Property


    Private _ProductID As Integer
    Public Property ProductID() As Integer
        Get

            Dim prodIDOut As Integer = 0

            If HttpContext.Current.Request.QueryString("productid") Is Nothing Then
                Return prodIDOut
            Else
                Dim prodID As String = HttpContext.Current.Request.QueryString("productid")

                If Integer.TryParse(prodID, prodIDOut) Then
                    Return prodID
                Else
                    Return prodIDOut
                End If
            End If

        End Get

        Set(value As Integer)
            _ProductID = value
        End Set
    End Property


    Private _BaseProductID As Integer
    Private Property BaseProductID() As Integer
        Get
            Return _BaseProductID
        End Get
        Set(ByVal value As Integer)
            _BaseProductID = value
        End Set

    End Property


    Private _EDDMProductID As Integer = 0
    Public Property EDDMProductID() As Integer
        Get
            Return _EDDMProductID
        End Get
        Set(ByVal value As Integer)
            _EDDMProductID = value
        End Set
    End Property


    Private _EDDMBaseProductID As Integer = 0
    Private Property EDDMBaseProductID() As Integer
        Get
            Return _EDDMBaseProductID
        End Get
        Set(ByVal value As Integer)
            _EDDMBaseProductID = value
        End Set

    End Property


    Private _TemplateSizeID As Integer
    Private Property TemplateSizeID() As Integer
        Get
            Return _TemplateSizeID
        End Get
        Set(ByVal value As Integer)
            _TemplateSizeID = value
        End Set

    End Property


    Private _ProductName As String = ""
    Private Property ProductName() As String
        Get
            Return _ProductName
        End Get
        Set(ByVal value As String)
            _ProductName = value
        End Set
    End Property


    Private _SKU As String = ""
    Private Property SKU() As String
        Get
            Return _SKU
        End Get
        Set(ByVal value As String)
            _SKU = value
        End Set
    End Property


    Private _PaperWidth As Decimal = 0
    Private Property PaperWidth() As Decimal
        Get
            Return _PaperWidth
        End Get
        Set(ByVal value As Decimal)
            _PaperWidth = value
        End Set
    End Property


    Private _PaperHeight As Decimal = 0
    Private Property PaperHeight() As Decimal
        Get
            Return _PaperHeight
        End Get
        Set(ByVal value As Decimal)
            _PaperHeight = value
        End Set
    End Property


    Private _PageCount As Integer = 1
    Private Property PageCount() As Integer
        Get
            Return _PageCount
        End Get
        Set(ByVal value As Integer)
            _PageCount = value
        End Set
    End Property


    Private _MinimumQuantity As Integer = 1
    Private Property MinimumQuantity() As Integer
        Get
            Return _MinimumQuantity
        End Get
        Set(ByVal value As Integer)
            _MinimumQuantity = value
        End Set
    End Property


    Private _MaximumQuantity As Integer = Integer.MaxValue
    Protected Property MaximumQuantity() As Integer
        Get
            If _MaximumQuantity = 0 Then
                _MaximumQuantity = Me.MinimumQuantity
            End If
            Return _MaximumQuantity
        End Get
        Set(ByVal value As Integer)
            _MaximumQuantity = value
        End Set
    End Property


    Private _DesignFee As Double = 0
    Private Property DesignFee() As Double

        Get
            Return _DesignFee
        End Get
        Set(ByVal value As Double)
            _DesignFee = value
        End Set
    End Property


    Private _EnableExtraCopies As Boolean = True
    Protected Property EnableExtraCopies() As Boolean

        Get
            Return _EnableExtraCopies
        End Get

        Set(ByVal value As Boolean)
            _EnableExtraCopies = value
        End Set

    End Property


    Private _MarkUp As Decimal = 0
    Private Property MarkUp() As Decimal
        Get
            Return _MarkUp
        End Get
        Set(ByVal value As Decimal)
            _MarkUp = value
        End Set
    End Property


    Private _MarkUpType As String = ""
    Private Property MarkUpType() As String
        Get
            Return _MarkUpType
        End Get
        Set(ByVal value As String)
            _MarkUpType = value
        End Set
    End Property


    Private _TotalSelected As Integer = 0
    Private Property TotalSelected() As Integer

        Get
            Return _TotalSelected
        End Get
        Set(ByVal value As Integer)
            _TotalSelected = value
        End Set
    End Property


    Private _bMultipleImpressionsNoFee As Boolean = False
    Protected Property bMultipleImpressionsNoFee() As Boolean

        Get
            If Not (String.IsNullOrEmpty(appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressionsNoFee"))) Then
                Return appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressionsNoFee")
            End If
        End Get

        Set(ByVal value As Boolean)
            _bMultipleImpressionsNoFee = value
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


    Private _referenceID As String = ""
    Private Property referenceID() As String
        Get
            Return _referenceID
        End Get
        Set(ByVal value As String)
            _referenceID = value
        End Set
    End Property


    Private _CategoryID As Integer
    Private Property CategoryID() As Integer
        Get
            Return _CategoryID
        End Get
        Set(ByVal value As Integer)
            _CategoryID = value
        End Set

    End Property


    Private _EDDMSelected As Integer = 0
    Private Property EDDMSelected() As Integer

        Get
            Return _EDDMSelected
        End Get
        Set(ByVal value As Integer)
            _EDDMSelected = value
        End Set
    End Property


    Private _AddressedSelected As Integer = 0
    Private Property AddressedSelected() As Integer
        Get
            Return _AddressedSelected
        End Get
        Set(ByVal value As Integer)
            _AddressedSelected = value
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


    Private _DesignType As String = ""
    Private Property DesignType() As String
        Get
            Return _DesignType
        End Get
        Set(ByVal value As String)
            _DesignType = value
        End Set
    End Property


    Private _DefaultMultiImpressions As Integer = 0
    Private Property DefaultMultiImpressions() As Integer
        Get
            Return _DefaultMultiImpressions
        End Get
        Set(ByVal value As Integer)
            _DefaultMultiImpressions = value
        End Set
    End Property


    Private _DisableTemplates As Boolean = False
    Private Property DisableTemplates() As Boolean
        Get
            Return _DisableTemplates
        End Get
        Set(ByVal value As Boolean)
            _DisableTemplates = value
        End Set
    End Property


    Private _DisableUpload As Boolean = False
    Private Property DisableUpload As Boolean
        Get
            Return _DisableUpload
        End Get
        Set(value As Boolean)
            _DisableUpload = value
        End Set
    End Property


    Private _DisableProDesign As Boolean = False
    Private Property DisableProDesign() As Boolean
        Get
            Return _DisableProDesign
        End Get
        Set(ByVal value As Boolean)
            _DisableProDesign = value
        End Set
    End Property


    Private _AllowMultipleImpressions As Boolean = False
    Private Property AllowMultipleImpressions() As Boolean
        Get
            Return _AllowMultipleImpressions
        End Get
        Set(ByVal value As Boolean)
            _AllowMultipleImpressions = value
        End Set
    End Property


    Private _AllowSplitDrops As Boolean = False
    Private Property AllowSplitDrops() As Boolean
        Get
            Return _AllowSplitDrops
        End Get
        Set(ByVal value As Boolean)
            _AllowSplitDrops = value
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


    Private _CampaignOverviewDisplayDelay As Boolean = False
    Public Property CampaignOverviewDisplayDelay As Boolean
        Get
            Return _CampaignOverviewDisplayDelay
        End Get
        Set(value As Boolean)
            _CampaignOverviewDisplayDelay = value
        End Set

    End Property


    Private _MinimumQtyExclusive As Integer = 0
    Private Property MinimumQtyExclusive() As Integer
        Get
            Return _MinimumQtyExclusive
        End Get
        Set(ByVal value As Integer)
            _MinimumQtyExclusive = value
        End Set
    End Property


    Private _MinOrderQty As Integer = 0
    Private Property MinOrderQty() As Integer
        Get
            Return _MinOrderQty
        End Get
        Set(ByVal value As Integer)
            _MinOrderQty = value
        End Set
    End Property


    Private _MinEDDMPricingQty As Integer = 0
    Private Property MinEDDMPricingQty() As Integer
        Get
            Return _MinEDDMPricingQty
        End Get
        Set(ByVal value As Integer)
            _MinEDDMPricingQty = value
        End Set
    End Property


    Private _MinAddressedPricingQty As Integer = 0
    Private Property MinAddressedPricingQty() As Integer
        Get
            Return _MinAddressedPricingQty
        End Get
        Set(ByVal value As Integer)
            _MinAddressedPricingQty = value
        End Set
    End Property


    Private _OffersExclusiveRoutes As Boolean = False
    Private Property OffersExclusiveRoutes() As Boolean
        Get
            Return _OffersExclusiveRoutes
        End Get
        Set(ByVal value As Boolean)
            _OffersExclusiveRoutes = value
        End Set
    End Property


    Private _NumImpressionsForExclusive As Integer = 0
    Private Property NumImpressionsForExclusive() As Integer
        Get
            Return _NumImpressionsForExclusive
        End Get
        Set(ByVal value As Integer)
            _NumImpressionsForExclusive = value
        End Set
    End Property


    Private _HideSplitDrops As Boolean = False
    Private Property HideSplitDrops() As Boolean
        Get
            Return _HideSplitDrops
        End Get
        Set(ByVal value As Boolean)
            _HideSplitDrops = value
        End Set
    End Property


    Private _PostageRate As Decimal = 0
    Private Property PostageRate() As Decimal
        Get
            Return _PostageRate
        End Get
        Set(ByVal value As Decimal)
            _PostageRate = value
        End Set
    End Property


    Private _ValidateExtraCopiesAddress As Boolean = False
    Private Property ValidateExtraCopiesAddress() As Boolean
        Get
            Return _ValidateExtraCopiesAddress
        End Get
        Set(ByVal value As Boolean)
            _ValidateExtraCopiesAddress = value
        End Set
    End Property


    Private _DropFeeRate As Integer = 0
    Private Property DropFeeRate() As Integer
        Get
            Return _DropFeeRate
        End Get
        Set(ByVal value As Integer)
            _DropFeeRate = value
        End Set
    End Property






    'Methods ==================================================================================================================
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        oCust = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name)

        If (Request.QueryString("productid") IsNot Nothing) Then
            ProductID = Integer.Parse(Request.QueryString("productid"))
        End If

        If (Request.QueryString("baseid") IsNot Nothing) Then
            BaseProductID = Integer.Parse(Request.QueryString("baseid"))
        End If



        SetPageProperties(ProductID)
        LoadProducts()
        PopulatePageWithProduct()
        SetDropDateLabelAndModal()


        If (TestMode) Then
            ShowDebug()
        Else
            If Not (String.IsNullOrEmpty(Request.QueryString("debug"))) Then
                If Request.QueryString("debug") = "hodor" Then
                    ShowDebug()
                End If
            End If
        End If


    End Sub



    Private Function CalculateTotalSelected(oDist As Taradel.CustomerDistribution) As Integer
        USelectID = oDist.USelectMethodReference.ForeignKey()

        Select Case USelectID
            Case 1
                EDDMMap = True
            Case 5
                UploadedAddressedList = True
            Case 6
                GeneratedAddressedList = True
        End Select


        If (EDDMMap) Or (UploadedAddressedList) Or (GeneratedAddressedList) Then
            TotalSelected = oDist.TotalDeliveries
        End If


        If (EDDMMap) Then
            Dim sDistRef As String = oDist.ReferenceId
            Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(sDistRef)
            Dim oSelects As List(Of Taradel.MapServer.UserData.AreaSelection) = Taradel.CustomerDistributions.GetSelections(sDistRef)
            Dim residentialTotal As Integer = 0
            Dim businessTotal As Integer = 0
            Dim poboxTotal As Integer = 0
            Dim useBusinesses As Boolean = True
            Dim usePOBoxes As Boolean = True
            Dim areaCount As Integer = 0

            If oSummary IsNot Nothing Then
                useBusinesses = oSummary.UseBusiness
                usePOBoxes = oSummary.UsePOBox
                For Each oArea As Taradel.MapServer.UserData.AreaSelection In oSelects
                    residentialTotal = residentialTotal + oArea.Residential
                    If useBusinesses Then
                        businessTotal = businessTotal + oArea.Business
                    End If
                    If usePOBoxes Then
                        poboxTotal = poboxTotal + oArea.POBoxes
                    End If
                    areaCount = areaCount + 1
                Next
                TotalSelected = (residentialTotal + businessTotal + poboxTotal) ''total selected is variable that should be used going forward --- 
            End If
        End If





        Return TotalSelected
    End Function



    Protected Sub SetPageProperties(productID As Integer)

        'This method is to set page properties each time the page is loaded.
        Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistributions.GetDistribution(UserDistributionId)
        USelectID = oDist.USelectMethodReference.ForeignKey()

        Select Case USelectID
            Case 1
                EDDMMap = True
            Case 5
                UploadedAddressedList = True
            Case 6
                GeneratedAddressedList = True
        End Select

        Dim productObj As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(productID)
        Dim baseProdObj As Taradel.Product = Taradel.ProductDataSource.GetEffectiveProduct(productObj.BaseProductID.Value)

        ProductName = productObj.Name
        BaseProductID = baseProdObj.ProductID
        SKU = baseProdObj.SKU
        PaperWidth = baseProdObj.PaperWidth
        PaperHeight = baseProdObj.PaperHeight
        PageCount = baseProdObj.PageCount
        TemplateSizeID = Taradel.WLUtil.GetTemplateSize(productID)
        referenceID = oDist.ReferenceId
        bMarketingUpsell = appxCMS.Util.CMSSettings.GetBoolean("Product", "MarketingUpsell")
        DropFeeRate = 99


        'If is EDDM Distribution or an AddressedList Distribution (2 kinds), then only ONE product is needed.  Set Mark Up properties.
        If (EDDMMap) Or (UploadedAddressedList) Or (GeneratedAddressedList) Then
            TotalSelected = CalculateTotalSelected(oDist) ''new 3/22/2016
            MarkUp = productObj.Markup
            MarkUpType = productObj.MarkupType

            If MarkUp > 0 AndAlso MarkUpType.ToLower = "percent" Then
                If MarkUp > 1 Then
                    MarkUp = MarkUp / 100
                End If
            End If

        End If


        'Addtional Site data.  Build SiteDetails Obj
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

        DefaultMultiImpressions = SiteDetails.DefaultMultiImpressions
        AllowSplitDrops = SiteDetails.AllowSplitDrops
        MinimumQtyExclusive = SiteDetails.MinQtyForExclusive
        MinOrderQty = SiteDetails.MinOrderQty
        MinEDDMPricingQty = SiteDetails.MinEDDMPricingQty
        MinAddressedPricingQty = SiteDetails.MinAddressedPricingQty
        OffersExclusiveRoutes = SiteDetails.OffersExclusiveRoutes
        NumImpressionsForExclusive = SiteDetails.NumImpressionsForExclusive
        HideSplitDrops = SiteDetails.HideSplitDrops
        ValidateExtraCopiesAddress = SiteDetails.ValidateExtraCopiesAddress


        TestMode = SiteDetails.TestMode
        CampaignOverviewDisplayDelay = SiteDetails.CampaignOverviewDisplayDelay


        If SiteDetails.OffersExclusiveRoutes Then
            hidExclusiveQualify.Value = "Congratulations! This order qualifies for route exclusivity."
            hidExclusiveDoesNotQualify.Value = "Please note, this order does NOT qualify for route exclusivity. To qualify, you must order at least 3 drops of 10,000 pieces."
            hidExclusiveNeedsMore.Value = "You must order at least 10,000 pieces to proceed to checkout"
        End If


        'Design Fee
        If baseProdObj.DesignFee.HasValue Then
            DesignFee = baseProdObj.DesignFee.Value
        End If


        'EXTRA COPIES. Even though the setting is called Upsize, this **IS** Extra Copies.
        Dim sOrderUpsize As String = appxCMS.Util.CMSSettings.GetSetting("Product", "Upsize", SiteID)
        Boolean.TryParse(sOrderUpsize, EnableExtraCopies)


        'Disable Templates
        If Not appxCMS.Util.CMSSettings.GetSetting("Product", "DisableTemplates", SiteID) Is Nothing Then
            DisableTemplates = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableTemplates")
        End If

        'Disable ProDesign
        If Not appxCMS.Util.CMSSettings.GetSetting("Product", "DisableProDesign", SiteID) Is Nothing Then
            DisableProDesign = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableProDesign")
        End If

        If Not appxCMS.Util.CMSSettings.GetSetting("Product", "DisableUploadArtwork") Is Nothing Then
            DisableUpload = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableUploadArtwork")
        End If

        If Not DisableTemplates Then
            '-- Setup our template browser with some proper attributes
            TemplateBrowser.ProductId = productID
            TemplateBrowser.SelectedProductName = productObj.Name
            TemplateBrowser.SelectedTemplateField = hidSelectedTemplateID.ClientID
        End If


        'Set the Postage Rate (for javascript - CampaignOverview)
        PostageRate = GetPostageRate(BaseProductID, USelectID)



    End Sub



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        ValidateQueryString()

        'Build Page Header
        BuildPageHeader()


        If Not Page.IsPostBack Then

            'Set controls w/ javascript calls
            txtExtraCopies.Attributes.Add("onchange", "ExtraCopiesChanged();")
            ddlDeliveryAddressId.Attributes.Add("onchange", "DeliveryAddressChanged(); ValidateAddress(); ValidateEntireAddress();")
            ddlDesignOption.Attributes.Add("onchange", "DesignChanged(); OptionsUpdated();")
            ddlImpressions.Attributes.Add("onchange", "ImpressionsChanged(); UpdatePriceQuote();")
            ddlDrops.Attributes.Add("onchange", "DropsChanged(); UpdatePriceQuote();")
            ddlNumOfDrops.Attributes.Add("onchange", "UpdatePriceQuote();")
            ddlProduct.Attributes.Add("onchange", "ProductChanged();")
            txtExtraCopies.Attributes.Add("onchange", "ExtraCopiesChanged();")
            DeliveryAddress.Attributes.Add("onchange", "StreetChanged(); ValidateEntireAddress();")
            DeliveryCity.Attributes.Add("onchange", "CityChanged(); ValidateEntireAddress();")
            ZipCode.Attributes.Add("onchange", "ZipChanged(); ValidateEntireAddress();")

            'button to fire modal control
            lnkChooseTemplate.Attributes.Add("data-toggle", "modal")
            lnkChooseTemplate.Attributes.Add("data-target", "#modalTemplates")
            lnkChooseTemplate.Attributes.Add("data-dismiss", "modal")


            'Order Steps control
            BuildOrderSteps()

            'fill page
            LoadProducts()

            'Jobs
            If (showJobsPanel) Then
                pnlJobNames.Visible = True
            End If


            'Show the correct help modal for Multi Impressions and Order TYPE
            If (bMultipleImpressionsNoFee) Then

                If USelectID = 1 Then
                    lnkNumberOfTimesToMailNoFee.Visible = True
                Else
                    lnkNumberOfTimesToMailNoFeeList.Visible = True
                End If

            Else

                If USelectID = 1 Then
                    lnkNumberOfTimesToMail.Visible = True
                Else
                    lnkNumberOfTimesToMailList.Visible = True
                End If

            End If



            'Addtional Site data.  Build SiteDetails Obj
            Dim SiteDetails As SiteUtility.SiteDetails
            SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

            'PreSelect Require Proof is config'd to do so.
            chkRequestProof.Checked = SiteDetails.SelectRequireProof

            'new 11/5/2015 - hide Split Drops if SiteDetails requires
            If SiteDetails.HideSplitDrops Then
                pnlDropsBlock.Visible = False
            End If


        End If


        'Set the date ranges for the Template and Pro Design Launch Week drop downs.
        ddlTemplateDesignLaunchWeek.IsTemplateJob = True
        ddlProDesignLaunchWeek.IsProfessionalJob = True


    End Sub



    Protected Sub LoadProducts()

        Dim oProds As New List(Of Taradel.WLProduct)
        Dim TemplateSizeId As Integer = 0

        oProds = Taradel.WLProductDataSource.GetProducts()

        For Each product As Taradel.WLProduct In oProds
            Dim li As New ListItem()

            li.Text = product.Name
            li.Value = product.ProductID
            li.Attributes.Add("baseprodid", product.BaseProductID)

            TemplateSizeId = Taradel.WLUtil.GetTemplateSize(product.ProductID)
            li.Attributes.Add("templatesize", TemplateSizeId)

            Dim okToAdd As Boolean = True
            For Each li2 As ListItem In ddlProduct.Items
                If (li2.Value = product.ProductID) Then
                    okToAdd = False
                End If
            Next

            If (okToAdd) Then
                ddlProduct.Items.Add(li)
            End If
        Next

        ddlProduct.DataBind()
        ddlProduct.Enabled = False

    End Sub



    Private Function RetrieveTemplateIDFromCookie() As Integer
        Dim templateId As Integer = 0
        If Request.Cookies("SelectedTemplate") IsNot Nothing Then
            Integer.TryParse(Request.Cookies("SelectedTemplate").Value, templateId)
        End If

        Return templateId
    End Function



    Public Function RetrieveProductTemplateSizeID(ProductID As Integer) As Integer
        Dim returnThis As Integer = 0

        Dim connectString As String = ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString.ToString()
        Dim sql As String = "SELECT top 1 TemplateSizeID FROM pnd_Product where productid = " & ProductID.ToString()
        'Response.Write("connectString:" & connectString)

        Using cn As New SqlConnection(connectString)
            Using cmd As New SqlCommand(sql, cn)
                Try
                    cn.Open()
                    Using dr As IDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            Dim str As String = dr("TemplateSizeID").ToString()
                            returnThis = Integer.Parse(str)
                        End If
                    End Using

                Catch ex As Exception
                    Response.Write(ex.ToString())
                End Try
            End Using
        End Using


        Return returnThis
    End Function



    Private Sub PopulatePageWithProduct()


        EnableProductFeatures()
        CreateMultipleImpressionsDropdown()


        'Build Product obj
        If Me.ProductID > 0 Then

            Me.oProd = Taradel.WLProductDataSource.GetProduct(Me.ProductID)


            'Possible obsolete but in place for now....
            '-- Setup any site-specific job name customizations
            Dim sCustomField1 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription1")
            Dim sCustomField2 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription2")
            Dim sCustomField3 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription3")

            If Not String.IsNullOrEmpty(sCustomField1) Then

                phJobName.Visible = False
                phJobNameCustom.Visible = True

                lblJobNameCustom1.Text = sCustomField1
                phJobNameCustom1.Visible = True
                rfvJobNameCustom1.ErrorMessage = "Please enter a value for " & sCustomField1

                If Not String.IsNullOrEmpty(sCustomField2) Then
                    lblJobNameCustom2.Text = sCustomField2
                    phJobNameCustom2.Visible = True
                    rfvJobNameCustom2.ErrorMessage = "Please enter a value for " & sCustomField2
                End If

                If Not String.IsNullOrEmpty(sCustomField3) Then
                    lblJobNameCustom3.Text = sCustomField3
                    phJobNameCustom3.Visible = True
                    rfvJobNameCustom3.ErrorMessage = "Please enter a value for " & sCustomField3
                End If

            End If



            If oProd IsNot Nothing Then


                'Set Drop Down List
                ddlProduct.SelectedValue = Me.ProductID

                'Build BaseProd obj
                Dim oBaseProd As Taradel.Product = Taradel.ProductDataSource.GetEffectiveProduct(BaseProductID)

                If oBaseProd IsNot Nothing Then

                    'Insert the Design Fee in the DDL
                    Dim oProOpt As ListItem = ddlDesignOption.Items.FindByValue("Pro")
                    If oProOpt IsNot Nothing Then
                        oProOpt.Text = String.Format(oProOpt.Text, DesignFee.ToString("C"))
                    End If


                    '-- USelect Distribution
                    If UserDistributionId > 0 Then


                        If Me.Distribution IsNot Nothing Then

                            Me.USelectMethod = Taradel.Helper.USelect.GetById(Me.Distribution.USelectMethodReference.ForeignKey)

                            If Me.USelectMethod IsNot Nothing Then


                                'Set Hidden Fields
                                hidDistributionID.Value = Me.UserDistributionId
                                hidTotalSelected.Value = CalculateTotalSelected(Me.Distribution) 'Me.Distribution.TotalDeliveries.ToString() ''Misty found here
                                txtTotalSelected.Text = CalculateTotalSelected(Me.Distribution) 'Me.TotalSelected.ToString() 'Me.Distribution.TotalDeliveries.ToString()  3/22/2016
                                hidDesignFee.Value = DesignFee

                                hidProductID.Value = Me.ProductID
                                hidBaseProductID.Value = Me.BaseProductID
                                txtProductID.Text = Me.ProductID
                                txtBaseProductID.Text = Me.BaseProductID

                                txtEddmProdID.Text = EDDMProductID
                                txtEddmBaseProdID.Text = EDDMBaseProductID
                                hidEddmProdID.Value = EDDMProductID
                                hidEddmBaseProdID.Value = EDDMBaseProductID

                                hidProductName.Value = Me.ProductName
                                hidJobName.Value = Me.Distribution.Name
                                hidTemplateSizeID.Value = Me.TemplateSizeID
                                txtDistributionID.Text = Me.UserDistributionId
                                txtTemplateSizeID.Text = Me.TemplateSizeID
                                txtProductName.Text = Me.ProductName
                                hidNoMultipleFee.Value = bMultipleImpressionsNoFee.ToString()
                                hidUSelectID.Value = USelectID
                                txtUSelectID.Text = USelectID
                                hidEDDMSelected.Value = EDDMSelected
                                txtEDDMSelected.Text = EDDMSelected
                                hidAddressedSelected.Value = AddressedSelected
                                txtAddressedSelected.Text = AddressedSelected

                                hidMarkUp.Value = MarkUp.ToString()
                                hidMarkUpType.Value = MarkUpType.ToString()
                                txtMarkUp.Text = MarkUp.ToString()
                                txtMarkUpType.Text = MarkUpType.ToString()

                                hidMinimumQtyExclusive.Value = MinimumQtyExclusive
                                txtMinimumQtyExclusive.Text = MinimumQtyExclusive

                                hdnMinimumToOrder.Value = MinOrderQty
                                txtMinimumToOrder.Text = MinOrderQty

                                hidMinEDDMPricingQty.Value = MinEDDMPricingQty
                                txtMinEDDMPricingQty.Text = MinEDDMPricingQty

                                hidMinAddressedPricingQty.Value = MinAddressedPricingQty
                                txtMinAddressedPricingQty.Text = MinAddressedPricingQty

                                hidExclusiveSite.Value = OffersExclusiveRoutes
                                txtExclusiveSite.Text = OffersExclusiveRoutes

                                hidMinimumImpressionExclusive.Value = NumImpressionsForExclusive
                                txtMinimumImpressionExclusive.Text = NumImpressionsForExclusive

                                hidHideSplitDrops.Value = HideSplitDrops
                                txtHideSplitDrops.Text = HideSplitDrops

                                hidPostageRate.Value = PostageRate
                                txtPostageRate.Text = PostageRate


                                'Set addition "Help' button attributes for Products.
                                btnAboutProduct.Attributes.Add("data-title", Me.ProductName)
                                btnAboutProduct.Attributes.Add("data-helpfile", "/helpProduct" & Me.BaseProductID)


                                Dim oUSelectProd As Taradel.USelectProductConfiguration = Taradel.Helper.USelect.GetProduct(Me.USelectMethod.USelectId, oProd.BaseProductID.Value)


                                If oUSelectProd IsNot Nothing Then
                                    Me.USelectProduct = oUSelectProd
                                    CreateDropsDropdown()
                                Else
                                    pnlError.Visible = True
                                    pnlNormal.Visible = False
                                    litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
                                    EmailUtility.SendAdminEmail("oUSelectProd IS Null. Product is not configured in the Admin U-Select Configuration.  (Step2-ProductOptions.aspx)")
                                End If


                            Else
                                pnlError.Visible = True
                                pnlNormal.Visible = False
                                litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
                                EmailUtility.SendAdminEmail("USelectMethod IS Null. (Step2-ProductOptions.aspx)")
                            End If

                        Else
                            pnlError.Visible = True
                            pnlNormal.Visible = False
                            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
                            EmailUtility.SendAdminEmail("Distribution IS Null. (Step2-ProductOptions.aspx)")
                        End If

                    Else
                        pnlError.Visible = True
                        pnlNormal.Visible = False
                        litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
                        EmailUtility.SendAdminEmail("Distribution IS ZERO. (Step2-ProductOptions.aspx)")
                    End If



                    'Check for Cookie for saved Template from previous visit.
                    If Request.Cookies("DesignOption") IsNot Nothing Then

                        Dim sDesignOption As String = Request.Cookies("DesignOption").Value
                        Dim oSelDesignOpt As ListItem = ddlDesignOption.Items.FindByValue(sDesignOption)

                        If oSelDesignOpt IsNot Nothing Then
                            oSelDesignOpt.Selected = True
                        End If

                        If sDesignOption.ToLower = "template" Then

                            Dim templateId As Integer = 0

                            If Request.Cookies("SelectedTemplate") IsNot Nothing Then

                                Integer.TryParse(Request.Cookies("SelectedTemplate").Value, templateId)

                                templateId = RetrieveTemplateIDFromCookie()
                                'Response.Write("templateid from cookie:" & templateId.ToString())
                                If templateId > 0 Then

                                    Dim oTemplate As TemplateCode.Template1 = Nothing

                                    Using oAPI As New TemplateCode.TemplateAPIClient
                                        Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, templateId)
                                        Dim oResponse As TemplateCode.GetTemplateResponse = oAPI.GetTemplate(oRequest)
                                        oTemplate = oResponse.GetTemplateResult
                                    End Using

                                    If oTemplate IsNot Nothing Then
                                        'Response.Write("oTemplate is not nothing")

                                        '-- Check the size against what we have available
                                        'this is a change = 11/2/2015 - switch from ProductID to BaseProductID
                                        Dim TemplateSizeId As Integer = Taradel.WLUtil.GetTemplateSize(Me.BaseProductID)
                                        Dim TemplateSizeId2 As Integer = RetrieveProductTemplateSizeID(Me.BaseProductID)
                                        'Response.Write("Me.BaseProductID " & Me.BaseProductID.ToString() & "|")
                                        'Response.Write("Taradel.WLUtil.GetTemplateSize " & TemplateSizeId.ToString() & "|")
                                        'Response.Write("RetrieveProductTemplateSizeID " & TemplateSizeId2.ToString() & "|")
                                        'Response.Write("oTemplate.TemplateSizeId " & oTemplate.TemplateSizeId.ToString() & "|")

                                        If oTemplate.TemplateSizeId = TemplateSizeId2 Then

                                            hidPrevSelectedTemplateID.Value = oTemplate.TemplateId
                                            txtPrevSelectedTemplateID.Text = oTemplate.TemplateId

                                            imgSelectedTemplate.ImageUrl = sTemplateServerHost & "/templates/full/" & oTemplate.FrontImage
                                            lblYouHaveSelected.Text = "You previously selected Template # " & oTemplate.TemplateId & ". You can continue with this selection or pick another one."
                                        Else
                                            'Response.Write(oTemplate.TemplateSizeId = TemplateSizeId)
                                            'hidPrevSelectedTemplateID.Value = oTemplate.TemplateId
                                            'txtPrevSelectedTemplateID.Text = oTemplate.TemplateId

                                            'imgSelectedTemplate.ImageUrl = sTemplateServerHost & "/templates/full/" & oTemplate.FrontImage
                                            'lblYouHaveSelected.Text = "You previously selected Template # " & oTemplate.TemplateId & ". You can continue with this selection or pick another one."

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    End If


                    If Me.Distribution IsNot Nothing Then

                        'Load Product Options / Drop Down Lists
                        LoadProductConfig(Me.BaseProductID, Me.MinimumQuantity, DesignFee)

                        'Total Deliveries label
                        lblTotalDeliveries.Text = Me.Distribution.TotalDeliveries.ToString("N0")


                        If Me.USelectProduct Is Nothing Then
                            pnlError.Visible = True
                            pnlNormal.Visible = False
                            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
                            EmailUtility.SendAdminEmail("USelect Product is nothing (Step2-ProductOptions.aspx).")
                        End If


                    End If




                    'Show available Ship To Addresses.
                    Dim sDefZip As String = ""

                    'Get saved Customer ShipTo Addresses
                    Dim oShipAddresses As List(Of Taradel.CustomerShippingAddress) = Nothing
                    If oCust IsNot Nothing Then
                        Try
                            oShipAddresses = Taradel.CustomerAddressDataSource.GetList(oCust.CustomerID)

                        Catch ex As Exception

                            pnlError.Visible = True
                            pnlNormal.Visible = False
                            litErrorMessage.Text = "Uh Oh!  Don't worry but there is a problem with getting your address. Our IT Staff has been notified and you will be contacted very shortly about this error."
                            EmailUtility.SendAdminEmail("Step2-ProductOptions.aspx - Exception thrown in oShipAddresses. [PopulatePageWithProduct Method]. CUSTOMER ID is: " & oCust.CustomerID & ". <br /><br />Exception: " & ex.Message)
                        End Try

                    End If


                    If oShipAddresses IsNot Nothing Then
                        For Each oAdd As Taradel.CustomerShippingAddress In oShipAddresses

                            Dim oItem As New ListItem(oAdd.Address1 & " / " & oAdd.City & ", " & oAdd.State & " " & oAdd.ZipCode, oAdd.ShippingAddressID.ToString())
                            oItem.Attributes.Add("zipcode", oAdd.ZipCode)

                            If String.IsNullOrEmpty(sDefZip) Then
                                sDefZip = oAdd.ZipCode
                            End If

                            ddlDeliveryAddressId.Items.Add(oItem)

                        Next
                    End If

                    'Insert "New Address"
                    ddlDeliveryAddressId.Items.Add(New ListItem("(New Address)", "0"))


                    'If the site requires the customer to select 'something' as an address when the request Extra Copies.
                    If (ValidateExtraCopiesAddress) Then
                        ddlDeliveryAddressId.Items.Insert(0, "--Please Select--")
                    End If

                    '-- Default selected should be the -1 address
                    ZipCode.Text = sDefZip

                End If

                'oProd is missing for some reason.  Go back.
            Else

                If Me.UserDistributionId > 0 Then
                    HttpContext.Current.Response.Redirect("Step1-TargetReview.aspx?distid=" & Me.UserDistributionId)
                Else
                    HttpContext.Current.Response.Redirect("Step1-Target.aspx")
                End If

            End If


        End If


    End Sub



    Protected Sub LoadProductConfig(ByVal BaseProductId As Integer, ByVal MinimumQuantity As Integer, ByVal dDesignFee As Decimal)

        Dim oOptCats As List(Of Taradel.ProductOptionCategory)
        oOptCats = Taradel.ProductDataSource.GetProductOptionCategories(Me.BaseProductID)
        lvProdOpts.DataSource = oOptCats
        lvProdOpts.DataBind()

    End Sub






    'Helpers ============================================================================================
    Private Sub ValidateQueryString()

        Dim lstToValidate As New List(Of String)
        lstToValidate.Add("productid")
        lstToValidate.Add("distid")
        lstToValidate.Add("baseid")

        For Each validateMe As String In lstToValidate
            If String.IsNullOrEmpty(Request.QueryString(validateMe)) Then
                Response.Redirect("~/default.aspx")
            End If
        Next

    End Sub



    Private Function CalculateQuantity(numImpressions As Integer, totalSelected As Integer) As Integer

        Dim calculated As Integer = 0

        If (numImpressions > 1) Then
            calculated = (numImpressions * totalSelected)
        End If

        Return calculated

    End Function



    Private Function CalculatePrintQTY(totalSelected As Integer, holdQTY As Integer, extraCopies As Integer) As Integer

        Dim calculated As Integer = (totalSelected + holdQTY + extraCopies)

        Return calculated

    End Function



    Public Function CalculateMultipleImpressionDropFee(numDrops As Integer, dDropPrice As Double) As Integer

        Dim returnThis As Integer = 0

        If ((bMultipleImpressionsNoFee) Or (numDrops = 1)) Then
            returnThis = 0
        Else
            returnThis = (numDrops - 1) * dDropPrice
        End If

        Return returnThis

    End Function



    Public Function CalculateTaxablePrice(price As Double, postageRate As Double, qty As Integer) As Double

        Dim results As Decimal = 0

        results = (price - (postageRate * qty))

        Return results

    End Function



    Private Function GetPostageRate(BaseProductID As Integer, USelectID As Integer) As Decimal


        Dim results As Decimal = 0.16                                           'Default, fallback setting
        Dim connectString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim selectSql As String = "SELECT [PostageFee] FROM [USelectProductConfiguration] WHERE ProductID = " & BaseProductID & " AND [USelectID] = " & USelectID
        Dim connectObj As New System.Data.SqlClient.SqlConnection(connectString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connectObj)


        Try
            connectObj.Open()
            results = Convert.ToDecimal(sqlCommand.ExecuteScalar())

        Catch objException As Exception

            EmailUtility.SendAdminEmail("Error in <strong>GetPostageRate</strong> on Step2-ProductOptions. <br />SiteID: " & SiteID & "<br />Error Message:" & objException.Message.ToString())

        Finally
            connectObj.Close()
        End Try


        Return results


    End Function



    Protected Function AdjustedPrice(PricePerPiece As Decimal, ExtraCopies As Integer, NumImpressions As Integer) As Decimal

        'This function overrides the Price returned by the Taradel.ProductPriceQuote Class using new business logic. The purpose is NOT to 
        'overcharge and send a price to Cart if they user is under the selected Min Order Total.  Drop Fees, Impression Fees, Design Fees have
        'NO impact here.  They are stored in the cart elsewhere.
        'It uses the same business logic as the CampaignOverview.js file.
        'Function requires:
        '   PricePerPiece from Taradel.ProductPriceQuote.
        '   ExtraCopies from form.
        '   NumImpressions from form.


        Dim MinRequiredQty As Integer = 0                                               'Required Min Qty to base pricing from.  User can order under this qty but pricing will be based on this Min Qty value. 
        Dim MinRequiredOrderTotal As Decimal = 0                                        'This is calculated Required Min Order Total.
        Dim ExtraPcsPricePerPiece As Decimal = 0                                        'Price Per Piece for Extra Copies.  Does not include postage.
        Dim ExtraPcsCost As Decimal = 0                                                 'Total cost of Extra Copies (extra copies qty * extra copies price per piece).
        Dim EstOrderTotal As Decimal = 0                                                'Estimated Order Total. Used to check against. Does NOT include design fee.
        Dim AdjustedEstTotal As Decimal = 0                                             'Cost of (total mailed x ppp) + extraPcsCost.  NOT charging postage on extra pcs.




        '1) Determine if this is an EDDM or AddressedList quote and set MinRequiredQty as needed
        If (USelectID = 1) Then
            MinRequiredQty = MinEDDMPricingQty
        Else
            MinRequiredQty = MinAddressedPricingQty
        End If



        '2) Set the Min Required Order Total
        MinRequiredOrderTotal = (MinRequiredQty * PricePerPiece)




        '3) Make calculations for extra copies. 
        ExtraPcsPricePerPiece = (PricePerPiece - PostageRate)
        ExtraPcsCost = (ExtraCopies * ExtraPcsPricePerPiece)



        '4) Calculate the Est Order Total
        EstOrderTotal = (((TotalSelected * NumImpressions) * PricePerPiece) + ExtraPcsCost)



        '5) Check to see if the calculated order amount is < the minRequiredOrderTotal
        If (EstOrderTotal < MinRequiredOrderTotal) Then

            'Min Order Amt was not met.  Use MinRequiredOrderTotal as the PRICE plus any add-on fees.
            AdjustedEstTotal = MinRequiredOrderTotal

        Else

            'Min Order Amt WAS met.  Now, let's just charge for the print cost of the extra pcs - no postage on these pcs.
            AdjustedEstTotal = (((TotalSelected * NumImpressions) * (PricePerPiece)) + ExtraPcsCost)

        End If

        'For testting
        'Response.Write("MinRequiredOrderTotal:" & MinRequiredOrderTotal & "<br />")
        'Response.Write("ExtraPcsPricePerPiece: " & ExtraPcsPricePerPiece & "<br />")
        'Response.Write("ExtraPcsCost: " & ExtraPcsCost & "<br />")
        'Response.Write("EstOrderTotal: " & EstOrderTotal & "<br />")
        'Response.Write("AdjustedEstTotal (for Cart): " & AdjustedEstTotal & "<br />")
        'End testing

        Return AdjustedEstTotal


    End Function





    'Page Builders =======================================================================================
    Protected Sub EnableProductFeatures()

        If (EnableExtraCopies) Then
            pnlExtraCopies.Visible = True
        End If


        If (DisableUpload) Then
            Dim oRemItem As ListItem = ddlDesignOption.Items.FindByValue("My")
            If oRemItem IsNot Nothing Then
                ddlDesignOption.Items.Remove(oRemItem)
            End If
        End If


        If (DisableTemplates) Then
            Dim oRemItem As ListItem = ddlDesignOption.Items.FindByValue("Template")
            If oRemItem IsNot Nothing Then
                ddlDesignOption.Items.Remove(oRemItem)
            End If
        End If


        If (DisableProDesign) Then
            Dim oRemItem As ListItem = ddlDesignOption.Items.FindByValue("Pro")
            If oRemItem IsNot Nothing Then
                ddlDesignOption.Items.Remove(oRemItem)
            End If
        End If


        If Not (AllowSplitDrops) Then
            ddlDrops.Items.RemoveAt(1)
        End If


        'If enabled, hide with JQuery until next button is clicked.
        If (CampaignOverviewDisplayDelay) Then
            Dim campaignScript As String = "<script type=" & Convert.ToChar(34) & "text/javascript" & Convert.ToChar(34) & ">" & Environment.NewLine() & "" & "$('#pnlCampaignOverview').hide();" & Environment.NewLine() & "</script>"
            litCampaignOverviewHide.Text = campaignScript
            litCampaignOverviewHide.Visible = True
        End If

    End Sub



    Protected Sub CreateMultipleImpressionsDropdown()

        'added 8/17/2015
        If Not appxCMS.Util.CMSSettings.GetSetting("Product", "MultipleImpressions", SiteID) Is Nothing Then
            AllowMultipleImpressions = appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressions")
        End If
        'end added 8/17/2015

        'added 11/5/2015 to make multiple impressions configurable
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)
        Dim loops As Integer = 1 ' start at one - make it easy

        If AllowMultipleImpressions = False Then
            ddlImpressions.Items.Add(New ListItem("One Time", "1"))
        Else
            While loops <= SiteDetails.MaxImpressions
                If (bMultipleImpressionsNoFee) Then
                    If loops = 1 Then
                        ddlImpressions.Items.Add(New ListItem("1 Time", loops))
                    Else
                        ddlImpressions.Items.Add(New ListItem(loops & " Times", loops))
                    End If
                Else 'there is a fee
                    If loops = 1 Then
                        ddlImpressions.Items.Add(New ListItem("1 Time", loops))
                    Else
                        ddlImpressions.Items.Add(New ListItem(loops & " Times ($" & (loops - 1) * 99 & " fee plus cost of additional pieces)", loops))
                    End If

                End If

                loops = loops + 1
            End While


            'temp code. 9/9/2015
            'ddlImpressions.Items.Add(New ListItem("Four Times ($297 fee plus cost of additional pieces)", "4"))
            'ddlImpressions.Items.Add(New ListItem("Five Times ($396 fee plus cost of additional pieces)", "5"))
            'ddlImpressions.Items.Add(New ListItem("Six Times ($495 fee plus cost of additional pieces)", "6"))
            'ddlImpressions.Items.Add(New ListItem("Seven Times ($594 fee plus cost of additional pieces)", "7"))
            'ddlImpressions.Items.Add(New ListItem("Eight Times ($693 fee plus cost of additional pieces)", "8"))


        End If

        ddlImpressions.SelectedValue = DefaultMultiImpressions

    End Sub



    Private Sub CreateDropsDropdown()
        TotalSelected = CalculateTotalSelected(Me.Distribution)

        'Load number of MAX available drops.
        If maxDrops = 0 Then maxDrops = 4
        'Build for Single Impression sites only.
        If maxDrops = 1 Then
            For i As Integer = 1 To maxDrops
                ddlNumOfDrops.Items.Add(New ListItem("1", "One Drop"))
            Next
            'Multiple Impression Enabled sites..
        Else
            'EDDM Drop Logic
            If (EDDMMap) Then
                Dim sDistRef As String = Me.Distribution.ReferenceId
                Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(sDistRef)
                Dim oSelects As List(Of Taradel.MapServer.UserData.AreaSelection) = Taradel.CustomerDistributions.GetSelections(sDistRef)
                Dim residentialTotal As Integer = 0
                Dim businessTotal As Integer = 0
                Dim poboxTotal As Integer = 0
                Dim useBusinesses As Boolean = True
                Dim usePOBoxes As Boolean = True
                Dim areaCount As Integer = 0
                'Preload the defaults
                For i As Integer = 1 To maxDrops
                    ddlNumOfDrops.Items.Add(New ListItem(i.ToString(), i.ToString()))
                Next

                If oSummary IsNot Nothing Then
                    useBusinesses = oSummary.UseBusiness
                    usePOBoxes = oSummary.UsePOBox
                    For Each oArea As Taradel.MapServer.UserData.AreaSelection In oSelects
                        residentialTotal = residentialTotal + oArea.Residential
                        If useBusinesses Then
                            businessTotal = businessTotal + oArea.Business
                        End If
                        If usePOBoxes Then
                            poboxTotal = poboxTotal + oArea.POBoxes
                        End If
                        areaCount = areaCount + 1
                    Next
                    'commenting out for now -->TotalSelected = (residentialTotal + businessTotal + poboxTotal) ''total selected is variable that should be used going forward --- 
                Else
                    Dim oMinRange As Taradel.PriceMatrix = Taradel.ProductDataSource.GetMinRange(Me.BaseProductID)
                    If oMinRange IsNot Nothing Then
                        Me.MinimumQuantity = oMinRange.MinQty
                    End If
                End If

                '-- Leave all 4 drops intact
                If TotalSelected > 10000 Then
                    'nothing here
                Else
                    Dim iRemFrom As Integer = 3
                    If iRemFrom > areaCount Then
                        iRemFrom = areaCount + 1
                    End If
                    If TotalSelected <= 1000 Then
                        iRemFrom = 2
                    End If
                    '-- Remove drops that they don't qualify for
                    For i As Integer = iRemFrom To maxDrops
                        Dim oItem As ListItem = ddlNumOfDrops.Items.FindByValue(i.ToString)
                        If oItem IsNot Nothing Then
                            ddlNumOfDrops.Items.Remove(oItem)
                        End If
                    Next
                End If


                'Go through ddlNumOfDrops drop down, and modify selections
                Dim removeMe As ListItem = New ListItem()
                Dim ii As Integer = 1

                For Each oItem As ListItem In ddlNumOfDrops.Items

                    Dim splitAmount As Integer = 0
                    splitAmount = (TotalSelected / ii)

                    Dim iDrop As Integer = 0
                    Integer.TryParse(oItem.Value, iDrop)

                    Dim dropFee As Double = ((iDrop - 1) * Me.USelectProduct.ExtraDropPrice)

                    If iDrop > 1 Then
                        oItem.Text = iDrop & " Drops (add " & dropFee.ToString("C0") & ")"
                    Else
                        oItem.Text = iDrop & " Drop (add " & dropFee.ToString("C0") & ")"
                        removeMe = oItem
                    End If
                    ii = ii + 1


                    oItem.Attributes("pricemod") = dropFee.ToString()
                    oItem.Attributes("pricemodpercent") = "0"
                    oItem.Attributes("pricemodeflat") = "1"
                    oItem.Attributes("weightmod") = "0"

                Next

                'Removes the first item since they chose not to use a Single Drop.
                'ONLY remove if the number of impressions is more than one
                If (Convert.ToInt32(ddlImpressions.SelectedValue)) > 1 Then
                    ddlNumOfDrops.Items.Remove(removeMe)
                End If




                'AddressedList drop logic. Required because AddressedList does not use Areas/Routes or Area Counts.
            Else


                'Must have at least 2000 to have more than one drop.  1000 is the min.
                If (TotalSelected >= 2000) Then

                    Dim adjustedMaxDrops As Integer = 0
                    Dim removeFirstItem As ListItem = New ListItem()
                    Dim feesToApply As Integer = 1
                    Dim dropFee As Double = 0
                    Dim dropCounter As Integer = 1


                    'Limit to 4 or AKA the max defined # of drops
                    If (TotalSelected / minAddressedListDropQTY) > maxDrops Then
                        adjustedMaxDrops = maxDrops

                        'get the quotient
                    Else
                        adjustedMaxDrops = (TotalSelected \ minAddressedListDropQTY)
                    End If


                    'Add all possible drops
                    For i As Integer = 1 To adjustedMaxDrops
                        ddlNumOfDrops.Items.Add(New ListItem(i.ToString(), i.ToString()))
                    Next


                    'Go through ddlNumOfDrops drop down, and modify selections
                    For Each oItem As ListItem In ddlNumOfDrops.Items

                        'Capture the first item in the loop
                        If dropCounter = 1 Then
                            removeFirstItem = oItem
                        End If

                        dropFee = ((feesToApply - 1) * Me.USelectProduct.ExtraDropPrice)
                        oItem.Text = feesToApply & " Drops (add " & dropFee.ToString("C0") & ")"

                        feesToApply = feesToApply + 1
                        dropCounter = dropCounter + 1

                        oItem.Attributes("pricemod") = dropFee.ToString()
                        oItem.Attributes("pricemodpercent") = "0"
                        oItem.Attributes("pricemodeflat") = "1"
                        oItem.Attributes("weightmod") = "0"

                    Next


                    ddlNumOfDrops.Items.Remove(removeFirstItem)


                Else
                    ddlDrops.Items.Remove("No")
                End If


            End If


        End If


    End Sub



    Private Sub BuildPageHeader()

        'Site Details Object
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

        'Set Page Header control
        If Not (SiteDetails.UseRibbonBanners) Then
            PageHeader.headerType = "simple"
            PageHeader.simpleHeader = "Design and Delivery Options"
        Else
            PageHeader.headerType = "partial"
            PageHeader.mainHeader = "Print"
            PageHeader.subHeader = "Design and Delivery Options"
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
            OrderSteps.step3Url = "/Step1-TargetReview.aspx?distid=" & UserDistributionId
            OrderSteps.step3State = "visited"
            OrderSteps.step3Icon = "fa-folder"

            OrderSteps.step4Text = "4) Define Delivery"
            OrderSteps.step4Url = Request.Url.AbsoluteUri.ToString()
            OrderSteps.step4State = "current"
            OrderSteps.step4Icon = "fa-envelope"

            OrderSteps.step5Text = "5) Check Out"
            OrderSteps.step5Url = ""
            OrderSteps.step5State = ""
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
            OrderSteps.step3Url = "/Step1-TargetReview.aspx?distid=" & UserDistributionId
            OrderSteps.step3State = "visited"
            OrderSteps.step3Icon = "fa-folder"

            OrderSteps.step4Text = "4) Define Delivery"
            OrderSteps.step4Url = Request.Url.AbsoluteUri.ToString()
            OrderSteps.step4State = "current"
            OrderSteps.step4Icon = "fa-envelope"

            OrderSteps.step5Text = "5) Check Out"
            OrderSteps.step5Url = ""
            OrderSteps.step5State = ""
            OrderSteps.step5Icon = "fa-credit-card"

        End If



        If (UploadedAddressedList) Then

            OrderSteps.numberOfSteps = 4
            OrderSteps.step1Text = "1) Upload List"
            OrderSteps.step1Url = "/Addressed/#"
            OrderSteps.step1State = "visited"
            OrderSteps.step1Icon = "fa-upload"

            OrderSteps.step2Text = "2) Choose Product"
            OrderSteps.step2Url = "/Step1-TargetReview.aspx?distid=" & UserDistributionId
            OrderSteps.step2State = "visited"
            OrderSteps.step2Icon = "fa-folder"

            OrderSteps.step3Text = "3) Define Delivery"
            OrderSteps.step3Url = Request.Url.AbsoluteUri.ToString()
            OrderSteps.step3State = "current"
            OrderSteps.step3Icon = "fa-envelope"

            OrderSteps.step4Text = "4) Check Out"
            OrderSteps.step4Url = ""
            OrderSteps.step4State = ""
            OrderSteps.step4Icon = "fa-credit-card"

        End If

    End Sub



    Private Sub SetDropDateLabelAndModal()

        'USelectID 5 - UploadList    'USelectID 6 - Build List
        If (USelectID = 5 Or USelectID = 6) Then
            lblLaunchWeek2.Text = "Please indicate when your mailer should drop into the mail stream."
            ddlMyDesignLaunchWeek.IsMailingListJob = True
            ddlMyDesignLaunchWeek.Refresh()
            ddlTemplateDesignLaunchWeek.IsMailingListJob = True
            ddlTemplateDesignLaunchWeek.Refresh()
            ddlProDesignLaunchWeek.IsMailingListJob = True
            ddlProDesignLaunchWeek.Refresh()

            btnAddressedLaunchDate.Visible = True

            'EDDM label
        Else
            lblLaunchWeek2.Text = "When do you want your first mailing to reach each targeted address?"
            btnEDDMLaunchWeek.Visible = True
        End If


    End Sub





    'Events ===============================================================================================
    Protected Sub lvProdOpts_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvProdOpts.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim oDItem As ListViewDataItem = e.Item
            Dim oOptCat As Taradel.ProductOptionCategory = DirectCast(oDItem.DataItem, Taradel.ProductOptionCategory)
            Dim ddlProdOpt As DropDownList = DirectCast(e.Item.FindControl("ddlProdOpt"), DropDownList)
            ddlProdOpt.AppendDataBoundItems = False
            ddlProdOpt.DataValueField = "OptionId"
            ddlProdOpt.DataTextField = "Name"
            ddlProdOpt.Attributes.Add("optcatid", oOptCat.OptCatID)

            '-- Register all possible values for this so that we don't get errors later with post back
            ddlProdOpt.DataSource = oOptCat.Options
            ddlProdOpt.DataBind()

            Dim pOptRow As Panel = DirectCast(e.Item.FindControl("pOptRow"), Panel)
            pOptRow.Attributes.Add("data-optcatid", oOptCat.OptCatID)

            If oOptCat.Options.Count = 1 Then
                If pOptRow IsNot Nothing Then
                    pOptRow.Style.Add("display", "none")
                End If
            End If

        End If

    End Sub



    Protected Sub btnCheckout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckout.Click

        If Me.UserDistributionId = 0 Then

            '-- Check to see if they have a valid address selected
            Dim sAddrId As String = ddlDeliveryAddressId.SelectedValue

            If sAddrId = "0" Then

                '-- Need to validate the address fields
                rfvDeliveryAddress.ValidationGroup = "vgOrder"
                rfvDeliveryCity.ValidationGroup = "vgOrder"
                ddlDeliveryAddressId.ValidationGroup = "vgOrder"
                rfvZipCode.ValidationGroup = "vgOrder"

            Else

                rfvDeliveryAddress.ValidationGroup = "vgDeliveryAddress"
                rfvDeliveryCity.ValidationGroup = "vgDeliveryAddress"
                ddlDeliveryAddressId.ValidationGroup = "vgDeliveryAddress"
                rfvZipCode.ValidationGroup = "vgDeliveryAddress"

            End If


        End If

        Page.Validate("vgOrder")


        If Page.IsValid Then

            'Pick the correct method to build the cart.
            If (EDDMMap) Then
                BuildAndSaveEDDMCart()
            End If

            If (UploadedAddressedList) Or (GeneratedAddressedList) Then
                BuildAndSaveAddressedCart()
            End If


            If (bMarketingUpsell) Then

                Response.Redirect("~/MarketingServices.aspx")


                'Currently, only EDDM Campaigns offer Marketing Upsell Services.
                If (USelectID = 1) Then
                    Response.Redirect("~/MarketingServices.aspx")
                Else
                    Response.Redirect("~/Step3-Checkout.aspx")
                End If

            Else
                Response.Redirect("~/Step3-Checkout.aspx")
            End If



        Else

            pnlError.Visible = True
            pnlNormal.Visible = False
            litErrorMessage.Text = "Uh Oh!  Don't worry but there was problem loading this page. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("Page was NOT valid. [btnCheckout_Click Method].(Step2-ProductOptions.aspx)")

        End If


    End Sub



    Private Sub BuildAndSaveEDDMCart()
        '========================================================================================================================================
        '   USelectID type 1.
        '
        '   Cart Build Steps:
        '   1) EDDM Product
        '       a) Build the Xml Document.  Load <cart /> parent node.
        '       b) Begin building Product Node components
        '	        1) Build Options SortedList (Int, Int)
        '	        2) Build OptionCategories List (of ProductOptionCategory)
        '	        3) Build PriceMatrix
        '       c) Get PrintMethodID
        '       d) Loop through OptionCategories, add to Options as needed.
        '       e) Get the product options from the Selected Product.  Get the OptCatID.
        '       f) Build the Quote object
        '       g) Set variables returned from Quote Object.
        '       h) Finally, insert the Product Node into the Cart
        '   2) EDDM Attribute Nodes
        '   3) EDDM OrderCalc Node
        '   4) EDDM Drops Node
        '   5) EDDM Indiv Drop Nodes w/ nested Area Nodes
        '   6) EDDM Design Node
        '   7) EDDM SHIPMENTS Node and w/ nested Shipment node(s)
        '========================================================================================================================================




        'DEFINE ALL THE VARIABLES
        Dim eddmObjCalc As New TaradelReceiptUtility.OrderCalculator()
        Dim methodVars As New StringBuilder()
        Dim eddmGUID As String = System.Guid.NewGuid.ToString

        'Form values
        Dim sJobName As String = hidJobName.Value
        Dim sCustomField1 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription1") 'possibly obsolete.
        Dim sCustomField2 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription2") 'possibly obsolete.
        Dim sCustomField3 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription3") 'possibly obsolete.
        Dim designType As String = ddlDesignOption.SelectedValue.ToLower                            'possible choices: 'my', 'template', 'pro'
        Dim deliveryAddressID As String = ddlDeliveryAddressId.SelectedValue                        'Ship To address id in drop down list.
        Dim comments As String = JobComments.Text                                                   'User comments from order.
        Dim mailAllPiecesAtOnce As String = ddlDrops.SelectedValue                                  'Comes from form.  Yes/No, True/False.
        Dim dEarliest As DateTime = Date.Parse(txtLaunchWeek.Text)                                  'User selected start of 'Launch Week'
        Dim zip As String = ZipCode.Text                                                            'User Ship To Zip Code

        'Prices
        Dim eddmPrice As Double = 0                                                                 'Stores calculated total price from Quote Obj.  (total mailed x price per piece)
        Dim eddmTaxablePrice As Double = 0                                                          'Currently same as eddmPrice / eddmQuoteObj.Price
        Dim eddmPricePerPiece As Double = 0                                                         'Stores EDDM Price Per Piece from Quote Obj.

        'Fees / Rates
        Dim postageFee As Double = 0                                                                'Fee for postage. (totalMailQty * postageRate)
        Dim dropPrice As Double = 0                                                                 'Fee for additional drops. Ex $99 x number of drops.
        Dim postageRate As Double = 0                                                               'Postage Rate

        'Product attributes
        Dim weight As Double = 0                                                                    'Comes from Base Product Obj
        Dim WeightPerPage As Double = 0                                                             'Used for Shipments Node. (Weight * PageCount) / EDDMSelected

        'various quantities
        Dim totalInOrderForQuote As Integer = 0                                                     'totalSelected x numImpressions
        Dim holdQTY As Integer = 0                                                                  'Obsolete concept. In place just to be safe. AKA 'iHoldQty'
        Dim extraCopies As Integer = 0                                                              'AKA 'iShipQty'. Comes from user.
        Dim printQTY As Integer = 0                                                                 'AKA 'iPrintQty'. (totalSelected + holdQTY + extraCopies)
        Dim totalMailQty As Integer = 0                                                             'Total Mailed.  (Total Selected  * Impressions)

        'Campaign variables
        Dim numDrops As Integer = 0                                                                 'Num of drops. AKA iDrops
        Dim numImpressions As Integer = 0                                                           'Num of impresssions.
        Dim sendThisNumberOfDrops As Int16 = 0                                                      'Adjusted # of Drops to send to oPriceQuote. Work Around.
        Dim iFrequency As Integer = 0                                                               'Every X of weeks - selected in DropDownList.

        'Distribution variables
        Dim useBusinesses As Boolean = True                                                         'Flag to use Businesses in distribution.
        Dim usePOBoxes As Boolean = False                                                            'Flag to use PO Boxes in distribution.
        Dim useResidential As Boolean = True                                                      'Flag to use Residentials in distribution.
        Dim daysToAdd As Integer = 1                                                                'Used to logically find next drop date. AKA 'weekInterval'
        Dim totalDropSelections = 0                                                                 'Used as a running total to store total 'matches' in the EDDM DataTable

        'Quote params
        Dim bIsFlatRateShipping As Boolean = False                                                  'Set by Quote Obj.
        Dim flatRateFee As Decimal = 0                                                              'Comes from QuoteObj.ShipPrice.
        Dim flatRateShipQTY As Integer = 1000                                                       'Hard coded to 1000.



        'Get Design Type.  Set flags
        Dim bProDesign As Boolean = False
        If designType = "pro" Then
            bProDesign = True
        End If

        Dim bTemplateDesign As Boolean = False
        If designType = "template" Then
            bTemplateDesign = True
        End If



        'Postage Rate
        postageRate = appxCMS.Util.AppSettings.GetDecimal("USPSPostageRate")
        If postageRate = 0 Then
            postageRate = 0.154
        End If

        'Convert Stuff
        Integer.TryParse(hidHoldQuantity.Value, holdQTY)
        Integer.TryParse(txtExtraCopies.Text, extraCopies)
        Integer.TryParse(ddlImpressions.SelectedValue, numImpressions)
        Integer.TryParse(hidTotalSelected.Value, TotalSelected)
        Integer.TryParse(ddlNumOfDrops.SelectedValue, numDrops)
        Integer.TryParse(ddlFrequency.SelectedValue, iFrequency)





        'Calculate Stuff / Assign more stuff
        printQTY = CalculatePrintQTY(TotalSelected, holdQTY, extraCopies)
        totalMailQty = (TotalSelected * numImpressions)
        daysToAdd = (7 * iFrequency)

        eddmObjCalc.ExtraPieces = extraCopies



        'Multiple Impressions Logic
        'If is Multiple Impressions, change the number of drops to match the number of impressions
        If (numImpressions = 1) Then
            eddmObjCalc.IsThisAMultiple = False
            eddmObjCalc.NumOfDrops = numDrops
            eddmObjCalc.MailPieces = TotalSelected
            If mailAllPiecesAtOnce = "Yes" Then
                eddmObjCalc.NumOfDrops = 1
                numDrops = 1
            End If
        Else
            eddmObjCalc.IsThisAMultiple = True
            eddmObjCalc.NumOfDrops = numImpressions
            eddmObjCalc.MailPieces = (TotalSelected * numImpressions)
        End If



        'The PriceQuote Obj ADDS on the drop fees based on the number of drops since we are NOT charging for Multiple 
        'Impressions on SOME sites set the number of drops to 1 for these sites.
        sendThisNumberOfDrops = eddmObjCalc.NumOfDrops
        If (bMultipleImpressionsNoFee) Then
            sendThisNumberOfDrops = 1
        End If

        If (numDrops = 0) Then
            numDrops = 1
        End If



        'Custom Jobs.  Possibly obsolete concept.
        If Not String.IsNullOrEmpty(sCustomField1) Then
            Dim oJobName As New List(Of String)
            oJobName.Add(JobNameCustom1.Text.Trim)

            If Not String.IsNullOrEmpty(sCustomField2) Then
                oJobName.Add(JobNameCustom2.Text.Trim)
            End If

            If Not String.IsNullOrEmpty(sCustomField3) Then
                oJobName.Add(JobNameCustom3.Text.Trim)
            End If

            sJobName = String.Join("-", oJobName.ToArray)
        End If




        '1) PRODUCT NODE 


        'a) BUILD THE CART
        Dim oXML As New XmlDocument
        oXML.LoadXml("<cart />")

        Dim oCart As XmlNode = oXML.SelectSingleNode("/cart")


        'b) Begin building Product Node components

        'Get available Product Options
        'Build the selected options from what we have in the categories for this product and the underlying field values
        Dim options As New SortedList(Of Integer, Integer)
        Dim oOptCats As List(Of Taradel.ProductOptionCategory) = Taradel.ProductDataSource.GetProductOptionCategories(Me.BaseProductID)


        'New: 1/11/2016 - DSF
        'Reset the printQty if needed.  If the total num to be printed is less than the required Pricing Qty, reset it to the min!
        Dim minRequiredPricingQty As Integer = 0
        If (EDDMMap) Then
            minRequiredPricingQty = MinEDDMPricingQty
        Else
            minRequiredPricingQty = MinAddressedPricingQty
        End If


        If (printQTY < minRequiredPricingQty) Then
            'Reset the printQty to get price based on Min Required Amount
            printQTY = minRequiredPricingQty
        End If
        'End of 1/11/2016 changes


        Try

            Dim oPriceMatrix As Taradel.PriceMatrix = Taradel.ProductDataSource.GetPriceRange(Me.BaseProductID, printQTY)

            'Get Price Range
            If oPriceMatrix IsNot Nothing Then

                'c) Get PrintMethodID
                Dim printMethodId As Integer = oPriceMatrix.PrintMethod.PrintMethodId

                'd) Loop through OptionCategories, add to Options as needed.
                For Each oOptCat As Taradel.ProductOptionCategory In oOptCats
                    Dim oOpts As IEnumerable(Of Taradel.ProductOption) = oOptCat.Options.Where(Function(po As Taradel.ProductOption) po.ProductPrintMethodOptions.Any(Function(ppmo As Taradel.ProductPrintMethodOption) ppmo.PrintMethodReference.ForeignKey = printMethodId))
                    If oOpts.Count > 0 Then
                        options.Add(oOptCat.OptCatID, 0)
                    End If
                Next

            Else

                pnlNormal.Visible = False
                pnlError.Visible = True
                litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
                EmailUtility.SendAdminEmail("oPriceMatrix IS Null. (Step2-ProductOptions.aspx). SiteID is " & SiteID & ".<br />PrintQTY: " & printQTY & "<br />")
                Return

            End If

        Catch ex As Exception
            Response.Write(ex.ToString())
            Response.Write(ex.StackTrace.ToString())
        End Try


        'Get the product options from the selected product (drop down list)
        'e) Get the product options from the Selected Product.  Get the OptCatID.
        For Each oItem As ListViewItem In lvProdOpts.Items

            If oItem.ItemType = ListViewItemType.DataItem Then

                Dim oDropDown As DropDownList = DirectCast(oItem.FindControl("ddlProdOpt"), DropDownList)
                Dim sOptCatId As String = oDropDown.Attributes("optcatid")
                Dim sOptVal As String = oDropDown.SelectedValue
                Dim iOptCatId As Integer = 0
                Dim iOptVal As Integer = 0

                Integer.TryParse(sOptCatId, iOptCatId)

                If options.ContainsKey(iOptCatId) Then
                    Integer.TryParse(sOptVal, iOptVal)
                    If iOptCatId > 0 Then
                        options(iOptCatId) = iOptVal
                    End If
                End If

            End If
        Next



        'Get quote Obj
        'f) Build the Quote object. 13 args
        Dim eddmQuoteObj As New Taradel.ProductPriceQuote(SiteID, BaseProductID, eddmObjCalc.MailPieces, holdQTY, extraCopies, UserDistributionId, sendThisNumberOfDrops, options, ZipCode.Text, bProDesign, bTemplateDesign, MarkUp, MarkUpType)



        'g) Set variables returned from Quote Object.
        'Set Proper Weight, Price, Taxable Price, etc. These values and variables are needed/used later in the code.
        eddmPricePerPiece = eddmQuoteObj.PricePerPiece
        weight = eddmQuoteObj.Weight


        'New. 1/165/2016. DSF
        eddmPrice = AdjustedPrice(eddmPricePerPiece, extraCopies, numImpressions)


        eddmTaxablePrice = CalculateTaxablePrice(eddmPrice, postageRate, totalMailQty)
        bIsFlatRateShipping = eddmQuoteObj.IsFlatRateShipping
        flatRateFee = eddmQuoteObj.ShipPrice
        postageFee = (totalMailQty * postageRate)
        dropPrice = CalculateMultipleImpressionDropFee(eddmObjCalc.NumOfDrops, DropFeeRate)



        'Set Design Fee Attribute. Store 0 unless Pro was selected.
        If designType.ToLower() <> "pro" Then
            DesignFee = 0
        End If


        'h) Finally, insert the Product Node into the Cart
        'ADD THE EDDM PRODUCT NODE
        'oCart = (CartUtility.AddProduct(oCart, DateTime.Now.ToString(), BaseProductID, DesignFee, UserDistributionId, flatRateFee, flatRateShipQTY, eddmGUID,
        '                                bIsFlatRateShipping, comments, sJobName, ProductName, PaperHeight, PaperWidth, postageFee, eddmPrice, eddmPricePerPiece,
        '                                ProductID, totalMailQty, SiteID, SKU, eddmTaxablePrice.ToString(), "EDDM", weight))

        Dim productCartNode As New CartUtility.ProductCartNode()
        productCartNode.AddedDate = DateTime.Now.ToString()
        productCartNode.BaseProductID = BaseProductID
        productCartNode.DesignFee = DesignFee
        productCartNode.DistributionID = UserDistributionId
        productCartNode.FlatRateShipFee = flatRateFee
        productCartNode.FlatRateShipQty = flatRateShipQTY
        productCartNode.IndexGUID = eddmGUID
        productCartNode.IsFlatRateShipping = bIsFlatRateShipping
        productCartNode.JobComments = comments
        productCartNode.JobName = sJobName
        productCartNode.ProductName = ProductName
        productCartNode.PaperHeight = PaperHeight
        productCartNode.PaperWidth = PaperWidth
        productCartNode.PostageFees = postageFee
        productCartNode.Price = eddmPrice
        productCartNode.PricePerPiece = eddmPricePerPiece
        productCartNode.ProductID = ProductID
        productCartNode.Qty = totalMailQty
        productCartNode.SiteID = SiteID
        productCartNode.SKU = SKU
        productCartNode.TaxablePrice = eddmTaxablePrice
        productCartNode.ProductType = "EDDM"
        productCartNode.Weight = weight

        oCart = CartUtility.AddProduct(oCart, productCartNode)








        '2) ATTRIBUTE NODES
        For Each oOpt As Taradel.PMOptionInfo In eddmQuoteObj.SelectedOptions

            Dim dOptMarkup As Double = oOpt.BasePrice
            Dim bPercent As Boolean = oOpt.BaseMarkupPercent

            'This applies the override if one is defined for this option
            If oOpt.PriceMatrixOptionMarkup IsNot Nothing Then
                dOptMarkup = oOpt.PriceMatrixOptionMarkup.Markup
                bPercent = oOpt.PriceMatrixOptionMarkup.MarkupPercent
            End If

            oCart = (CartUtility.AddAttribute(oCart, "EDDM", oOpt.OptCatName, oOpt.OptCatId.ToString(), oOpt.OptionId.ToString(), oOpt.OptName, dOptMarkup.ToString(), bPercent.ToString(), oOpt.Weight.Value.ToString()))

        Next


        'Professional Service Attribute
        If designType = "pro" Then
            oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Professional Design Services", "0", "1", PaperWidth & "x" & PaperHeight & " (" & DesignFee.ToString("C") & ")", DesignFee.ToString(), "False", "0"))
        End If


        'Postage Fee Attribute
        oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Postage Fee", "0", postageFee.ToString(), eddmObjCalc.MailPieces.ToString("N0") & " pieces (" & postageFee.ToString("C") & ")", postageFee.ToString(), "False", "0"))


        'Number of Drops Attribute
        oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Number of Drops", "0", eddmObjCalc.NumOfDrops.ToString(), eddmObjCalc.NumOfDrops & " drop" & IIf(eddmObjCalc.NumOfDrops = 1, "", "s") & " (" & dropPrice.ToString("C") & ")", dropPrice.ToString(), "False", "0"))


        'Drop Schedule Attribute
        oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Drop Schedule", "0", "every " & iFrequency.ToString() & " weeks", iFrequency.ToString() & " weeks", "0", "False", "0"))




        '3) EDDM ORDER CALC NODE
        oCart = (CartUtility.AddOrderCalc(oCart, "EDDM", "0", "0", oCust.State, "", "0", "0", "0", "0", "0"))





        '4) EDDM DROPS NODE
        Dim oDist As Taradel.CustomerDistribution = Nothing
        Dim oUSelect As Taradel.USelectProductConfiguration = Nothing

        oDist = Taradel.CustomerDistributions.GetDistribution(UserDistributionId)

        If oDist IsNot Nothing Then
            oUSelect = Taradel.Helper.USelect.GetProduct(oDist.USelectMethodReference.ForeignKey, Me.BaseProductID)
        End If


        Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(Me.Distribution.ReferenceId)
        Dim oSelects As List(Of Taradel.MapServer.UserData.AreaSelection) = Taradel.CustomerDistributions.GetSelections(oDist.ReferenceId).OrderBy(Function(o As Taradel.MapServer.UserData.AreaSelection) o.Name).ToList

        If oSummary IsNot Nothing Then
            useBusinesses = oSummary.UseBusiness
            usePOBoxes = oSummary.UsePOBox
        End If

        oCart = (CartUtility.AddDrops(oCart, "EDDM", oCust.Company, oCust.Address1, oCust.Address2, oCust.City, oCust.State, oCust.ZipCode, "", "", useResidential, useBusinesses, usePOBoxes))




        '5) EDDM INDIV DROPS w/ Area Nodes
        Dim dDropBreak As Integer = eddmObjCalc.MailPieces / eddmObjCalc.NumOfDrops
        Dim bOnePer As Boolean = False



        'Multiple Impressions
        If eddmObjCalc.IsThisAMultiple Then
            'this is a muliple - so divide back out the tempObjCalc.MailPieces for each drop
            eddmObjCalc.MailPieces = eddmObjCalc.MailPieces / eddmObjCalc.NumOfDrops
            dDropBreak = eddmObjCalc.MailPieces
            bOnePer = False
            'postageFee = postageFee + (eddmObjCalc.MailPieces * oUSelect.MailPricePerPiece) '<--- may need to revisit this. 6/2/2015
            For dropNumber As Integer = 1 To eddmObjCalc.NumOfDrops
                Dim j As Integer = 0
                Dim dDropDate As DateTime = dEarliest.AddDays((dropNumber - 1) * daysToAdd)
                'Find Friday
                While (dDropDate.DayOfWeek <> DayOfWeek.Friday)
                    dDropDate = dDropDate.AddDays(-1)
                End While

                'Add the Drop Nodes. 0 is hard coded for dropNumber.  Will be updated later in code.
                oCart = CartUtility.AddIndividualDrop(oCart, "EDDM", dropNumber, "0", dDropDate.ToShortDateString(), True, eddmObjCalc.NumOfDrops)

                'Loop Thru Areas
                For j = j To oSelects.Count - 1
                    Dim oSelect As Taradel.MapServer.UserData.AreaSelection = oSelects(j)
                    Dim iSelectTotal As Integer = oSelect.Residential
                    'add businesses if needed
                    If useBusinesses Then
                        iSelectTotal = iSelectTotal + oSelect.Business
                    End If

                    'add PO Boxes if needed
                    If usePOBoxes Then
                        iSelectTotal = iSelectTotal + oSelect.POBoxes
                    End If

                    'Add each Area Node
                    oCart = CartUtility.AddIndividualArea(oCart, "EDDM", dropNumber, oSelect.Name, oSelect.FriendlyName, iSelectTotal.ToString())

                    totalDropSelections = totalDropSelections + iSelectTotal

                Next

                'Update total with new value
                oCart = CartUtility.UpdateDropCount(oCart, "EDDM", dropNumber, totalDropSelections)

                'reset
                totalDropSelections = 0


            Next


            'Single Impressions
        Else

            postageFee = postageFee + (TotalSelected * oUSelect.MailPricePerPiece) '<--- may need to revisit this. 6/2/2015
            dDropBreak = eddmObjCalc.MailPieces / eddmObjCalc.NumOfDrops


            'oSelects.Count = # of Routes
            'If number of drops is the same as number as the number of routes AND is more than one.
            If numDrops = oSelects.Count And eddmObjCalc.NumOfDrops > 1 Then
                bOnePer = True
            End If


            Dim j As Integer = 0

            For dropNumber As Integer = 1 To eddmObjCalc.NumOfDrops

                Dim dDropDate As DateTime = dEarliest.AddDays((dropNumber - 1) * daysToAdd)

                'Find Friday
                While (dDropDate.DayOfWeek <> DayOfWeek.Friday)
                    dDropDate = dDropDate.AddDays(-1)
                End While

                'Add the Drop Nodes
                oCart = CartUtility.AddIndividualDrop(oCart, "EDDM", dropNumber, "0", dDropDate.ToShortDateString(), False, eddmObjCalc.NumOfDrops)


                'Loop through the Areas
                For j = j To oSelects.Count - 1

                    Dim oSelect As Taradel.MapServer.UserData.AreaSelection = oSelects(j)
                    Dim iSelectTotal As Integer = oSelect.Residential

                    'Add businesses if needed
                    If useBusinesses Then
                        iSelectTotal = iSelectTotal + oSelect.Business
                    End If

                    'Add PO Boxes if needed
                    If usePOBoxes Then
                        iSelectTotal = iSelectTotal + oSelect.POBoxes
                    End If

                    'Add each Area Node
                    oCart = CartUtility.AddIndividualArea(oCart, "EDDM", dropNumber, oSelect.Name, oSelect.FriendlyName, iSelectTotal.ToString())


                    totalDropSelections = totalDropSelections + iSelectTotal

                    'First handle one area per drop
                    If bOnePer Then
                        j = j + 1
                        Exit For
                    End If

                    'now handle proper numeric allocation of areas to drops
                    'if the next area count will take this drop over the drop break, exit
                    ''attempt to sort from largest to smallest
                    Dim oSelectsSorted = From f In oSelects Order By f.Residential

                    If j + 1 < oSelectsSorted.Count - 1 Then

                        Dim oNextSel As Taradel.MapServer.UserData.AreaSelection = oSelectsSorted(j + 1)
                        Dim iNextTotal As Integer = oNextSel.Residential

                        If useBusinesses Then
                            iNextTotal = iNextTotal + oNextSel.Business
                        End If


                        If usePOBoxes Then
                            iNextTotal = iNextTotal + oNextSel.POBoxes
                        End If

                        If totalDropSelections + iNextTotal >= dDropBreak AndAlso dropNumber < numDrops Then
                            j = j + 1
                            Exit For
                        End If

                    End If

                Next

                'Update total with new value
                oCart = CartUtility.UpdateDropCount(oCart, "EDDM", dropNumber, totalDropSelections)
                totalDropSelections = 0

            Next

        End If




        '6) EDDM DESIGN NODE
        'Upload and Save the file first and then ...update the cart.
        Dim sClientBase As String = Taradel.WLUtil.GetRelativeSiteImagesPath & "/UserImages"
        Dim frontAction As String = ""
        Dim frontFileName As String = ""
        Dim frontFileExt As String = ""
        Dim frontFileSize As Long = 0
        Dim frontTmpName As String = ""
        Dim frontRealFileName As String = ""
        Dim backAction As String = ""
        Dim backFileName As String = ""
        Dim backFileExt As String = ""
        Dim backFileSize As Long = 0
        Dim backTmpName As String = ""
        Dim backRealFileName As String = ""
        Dim hasBackDesign As Boolean = False
        Dim artKey As String = ""
        Dim requiresProof As Boolean = False


        If Not Directory.Exists(Server.MapPath(sClientBase)) Then
            Directory.CreateDirectory(Server.MapPath(sClientBase))
        End If


        Dim sClientFolder As String = sClientBase & "/" & Profile.UserName.Replace("@", "_")
        Dim sClientPath As String = Server.MapPath(sClientFolder)


        Dim oDir As New DirectoryInfo(sClientPath)
        If Not oDir.Exists Then
            oDir.Create()
        End If


        Select Case designType.ToLower
            Case "my"

                requiresProof = chkRequestProof.Checked

                If File1.HasFile Then

                    frontAction = "Upload"

                    frontFileName = File1.FileName
                    Dim oUpload1 As FileInfo = New FileInfo(frontFileName)
                    frontFileExt = oUpload1.Extension
                    frontFileSize = File1.ContentLength
                    frontTmpName = System.Guid.NewGuid.ToString
                    frontRealFileName = frontTmpName & frontFileExt

                    File1.MoveTo(Path.Combine(sClientPath, frontTmpName & frontFileExt), Brettle.Web.NeatUpload.MoveToOptions.None)

                Else
                    frontAction = "Omitted"
                End If


                If File2.HasFile Then

                    hasBackDesign = True

                    '-- They have uploaded a file
                    backAction = "Upload"

                    backFileName = File2.FileName
                    Dim oUpload2 As FileInfo = New FileInfo(backFileName)
                    backFileExt = oUpload2.Extension
                    backTmpName = System.Guid.NewGuid.ToString
                    backRealFileName = backTmpName & backFileExt

                    File2.MoveTo(Path.Combine(sClientPath, backTmpName & backFileExt), Brettle.Web.NeatUpload.MoveToOptions.None)

                Else
                    backAction = "Omitted"
                End If



            Case "template"

                'ArtKey could be determined by either the hidPrevSelectedTemplateID (from a previous order OR coming from the Design page).
                'Otherwise, we will pull this value from the hidSelectedTemplateID.
                If hidSelectedTemplateID.Value = "0" Then
                    artKey = hidPrevSelectedTemplateID.Value
                Else
                    artKey = hidSelectedTemplateID.Value
                End If

            Case "pro"
                'Nothing yet...

        End Select



        'Add Design Node
        oCart = (CartUtility.AddDesign(oCart, "EDDM", designType, frontFileExt, frontFileName, frontRealFileName, frontAction, hasBackDesign, _
        backFileExt, backFileName, backRealFileName, backAction, artKey, requiresProof.ToString().ToUpper()))




        '7) EDDM SHIPMENTS NODE
        If UserDistributionId > 0 Then

            WeightPerPage = (weight * PageCount) / EDDMSelected

            If extraCopies > 0 Then

                If deliveryAddressID = "0" Then

                    '-- Create a new address and get the ID
                    Dim sMsg As String = ""
                    Dim iAddr As Integer = Taradel.CustomerAddressDataSource.NewAddress(oCust.CustomerID, "", "", "", "", DeliveryAddress.Text, DeliveryAddress2.Text, DeliveryCity.Text, USAStatesDropDown.SelectedValue, ZipCode.Text, sMsg)

                    If iAddr > 0 Then
                        deliveryAddressID = iAddr.ToString()

                        'Repair the record.  [pnd_CustomerShippingAddress].[Deleted] is being saved as Null.
                        FixShippingAddress(deliveryAddressID)

                    End If

                End If

            End If

        End If

        oCart = (CartUtility.AddShipments(oCart, "EDDM", deliveryAddressID, extraCopies, weight, PageCount, PaperWidth, PaperHeight, flatRateFee.ToString("N2"), zip, totalMailQty))



        Profile.Cart = oXML
        Profile.Save()


    End Sub



    Private Sub BuildAndSaveAddressedCart()


        '========================================================================================================================================
        ' For AddressedList Campaign types. USelect Types 5 & 6.
        '
        '   Cart Build Steps:
        '   1) ADDRESSED Product
        '   2) ADDRESSED Attribute Nodes
        '   3) ADDRESSED OrderCalc Node
        '   4) ADDRESSED Drops Node
        '   5) ADDRESSED Indiv Drop Nodes
        '   6) ADDRESSED Design Node
        '   7) ADDRESSED SHIPMENTS Node and w/ nested Shipment node(s)
        '   8) ADDRESSED Demographics Node
        '========================================================================================================================================

        'DEFINE THE VARIABLES
        Dim addressedObjCalc As New TaradelReceiptUtility.OrderCalculator()
        Dim methodVars As New StringBuilder()
        Dim addressedGUID As String = System.Guid.NewGuid.ToString

        'Form values
        Dim sJobName As String = hidJobName.Value
        Dim sCustomField1 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription1") 'possibly obsolete
        Dim sCustomField2 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription2") 'possibly obsolete
        Dim sCustomField3 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription3") 'possibly obsolete
        Dim designType As String = ddlDesignOption.SelectedValue.ToLower                            'possible choices: 'my', 'template', 'pro'
        Dim deliveryAddressID As String = ddlDeliveryAddressId.SelectedValue                        'Ship To address ID from User
        Dim mailAllPiecesAtOnce As String = ddlDrops.SelectedValue                                  'Comes from form.  Yes/No, True/False.
        Dim comments As String = JobComments.Text                                                   'User comments from order.
        Dim zip As String = ZipCode.Text                                                            'User Ship To Zip Code
        Dim earliestDropDate As DateTime = Date.Parse(txtLaunchWeek.Text)                           'User selected 'Launch Week'

        'Prices
        Dim addressedPrice As Double = 0                                                            'Stores calculated total price from Quote Obj.  (total mailed x price per piece) 
        Dim taxablePrice As Double = 0                                                              'Currently same as addressedPrice / addressedQuoteObj.Price
        Dim addressedPricePerPiece As Double = 0

        'Distribution values
        Dim numDrops As Integer = 0                                                                 'Num of drops. AKA iDrops
        Dim numImpressions As Integer = 0                                                           'Num of impresssions.
        Dim sendThisNumberOfDrops As Int16 = 0                                                      'Adjusted # of Drops to send to oPriceQuote. Work Around.
        Dim useBusinesses As Boolean = True                                                         'Flag to use Businesses in distribution.
        Dim usePOBoxes As Boolean = True                                                            'Flag to use PO Boxes in distribution.
        Const useResidential As Boolean = True                                                      'Flag to use Residentials in distribution.
        Dim iFrequency As Integer = 0                                                               'Every X of weeks - selected in DropDownList.
        Dim daysToAdd As Integer = 1                                                                'Used to logically find next drop date. AKA 'weekInterval'

        'Various Quantities
        Dim holdQTY As Integer = 0                                                                  'Obsolete concept. In place just to be safe. AKA 'iHoldQty'
        Dim extraCopies As Integer = 0                                                              'AKA 'iShipQty'. User Provided
        Dim printQTY As Integer = 0                                                                 'AKA 'iPrintQty'. (totalSelected + holdQTY + extraCopies)
        Dim totalInOrderForQuote As Integer = 0                                                     'totalSelected x numImpressions
        Dim totalMailQty As Integer = 0                                                             'Total Mailed.  (Total Selected  * Impressions)
        Dim addressedTotalDropSelections = 0                                                        'Used as a running total to store total 'matches' in the Addressed DataTable

        'Rates / Fees
        Dim postageFee As Double = 0                                                                'Fee for postage. (totalMailQty * postageRate)
        Dim dropPrice As Double = 0                                                                 'Fee for additional drops. Ex $99 x number of drops.
        Dim postageRate As Double = 0.3                                                             'Addressed Mail pcs are 0.30 each (postage).

        'Product attributes
        Dim weight As Double = 0                                                                    'Comes from Base Product Obj
        Dim WeightPerPage As Double = 0                                                             'Used for Shipments Node. (Weight * PageCount) / EDDMSelected

        'Quote params
        Dim bIsFlatRateShipping As Boolean = False                                                  'Set by Quote Obj.
        Dim flatRateFee As Decimal = 0                                                              'Comes from QuoteObj.ShipPrice.
        Dim flatRateShipQTY As Integer = 1000                                                       'Hard coded to 1000.


        'Get Design Type.  Set flags
        Dim bProDesign As Boolean = False
        If designType = "pro" Then
            bProDesign = True
        End If

        Dim bTemplateDesign As Boolean = False
        If designType = "template" Then
            bTemplateDesign = True
        End If



        'Convert Stuff
        Integer.TryParse(hidHoldQuantity.Value, holdQTY)
        Integer.TryParse(txtExtraCopies.Text, extraCopies)
        Integer.TryParse(ddlImpressions.SelectedValue, numImpressions)
        Integer.TryParse(hidTotalSelected.Value, TotalSelected)
        Integer.TryParse(ddlNumOfDrops.SelectedValue, numDrops)
        Integer.TryParse(ddlFrequency.SelectedValue, iFrequency)



        'Calculate Stuff / Assign more stuff
        printQTY = CalculatePrintQTY(TotalSelected, holdQTY, extraCopies)
        daysToAdd = (7 * iFrequency)
        totalMailQty = (TotalSelected * numImpressions)

        addressedObjCalc.ExtraPieces = extraCopies



        'Multiple Impressions Logic
        'If is Multiple Impressions, change the number of drops to match the number of impressions
        If (numImpressions = 1) Then
            addressedObjCalc.IsThisAMultiple = False
            addressedObjCalc.NumOfDrops = numDrops
            addressedObjCalc.MailPieces = TotalSelected
            If mailAllPiecesAtOnce = "Yes" Then
                addressedObjCalc.NumOfDrops = 1
                numDrops = 1
            End If
        Else
            addressedObjCalc.IsThisAMultiple = True
            addressedObjCalc.NumOfDrops = numImpressions
            addressedObjCalc.MailPieces = (TotalSelected * numImpressions)
        End If



        'The Quote Obj ADDS on the drop fees based on the number of drops since we are NOT charging for Multiple 
        'Impressions on SOME sites set the number of drops to 1 for these sites.
        sendThisNumberOfDrops = addressedObjCalc.NumOfDrops
        If (bMultipleImpressionsNoFee) Then
            sendThisNumberOfDrops = 1
        End If

        If (numDrops = 0) Then
            numDrops = 1
        End If


        'Custom Jobs.  Possibly obsolete concept.
        If Not String.IsNullOrEmpty(sCustomField1) Then
            Dim oJobName As New List(Of String)
            oJobName.Add(JobNameCustom1.Text.Trim)

            If Not String.IsNullOrEmpty(sCustomField2) Then
                oJobName.Add(JobNameCustom2.Text.Trim)
            End If

            If Not String.IsNullOrEmpty(sCustomField3) Then
                oJobName.Add(JobNameCustom3.Text.Trim)
            End If

            sJobName = String.Join("-", oJobName.ToArray)
        End If



        '-- Build cart
        Dim oXML As New XmlDocument
        oXML.LoadXml("<cart />")

        Dim oCart As XmlNode = oXML.SelectSingleNode("/cart")


        '1) PRODUCT NODE

        'Reset the printQty if needed
        If (printQTY < MinOrderQty) Then
            printQTY = MinOrderQty
        End If


        'Get available Product Options
        'Build the selected options from what we have in the categories for this product and the underlying field values
        Dim options As New SortedList(Of Integer, Integer)
        Dim oOptCats As List(Of Taradel.ProductOptionCategory) = Taradel.ProductDataSource.GetProductOptionCategories(Me.BaseProductID)
        Dim oPriceMatrix As Taradel.PriceMatrix = Taradel.ProductDataSource.GetPriceRange(Me.BaseProductID, printQTY)


        'Get Price Range
        If oPriceMatrix IsNot Nothing Then

            Dim printMethodId As Integer = oPriceMatrix.PrintMethod.PrintMethodId

            For Each oOptCat As Taradel.ProductOptionCategory In oOptCats
                Dim oOpts As IEnumerable(Of Taradel.ProductOption) = oOptCat.Options.Where(Function(po As Taradel.ProductOption) po.ProductPrintMethodOptions.Any(Function(ppmo As Taradel.ProductPrintMethodOption) ppmo.PrintMethodReference.ForeignKey = printMethodId))
                If oOpts.Count > 0 Then
                    options.Add(oOptCat.OptCatID, 0)
                End If
            Next

        Else

            pnlNormal.Visible = False
            pnlError.Visible = True
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("oPriceMatrix IS Null (Saved AddressedList Cart). <br />SiteID: " & SiteID & "<br />Sent At: " & DateTime.Now.ToString())
            Return

        End If


        'Get the product options from the selected product (drop down list)
        For Each oItem As ListViewItem In lvProdOpts.Items

            If oItem.ItemType = ListViewItemType.DataItem Then

                Dim oDropDown As DropDownList = DirectCast(oItem.FindControl("ddlProdOpt"), DropDownList)
                Dim sOptCatId As String = oDropDown.Attributes("optcatid")
                Dim sOptVal As String = oDropDown.SelectedValue
                Dim iOptCatId As Integer = 0
                Dim iOptVal As Integer = 0

                Integer.TryParse(sOptCatId, iOptCatId)

                If options.ContainsKey(iOptCatId) Then
                    Integer.TryParse(sOptVal, iOptVal)
                    If iOptCatId > 0 Then
                        options(iOptCatId) = iOptVal
                    End If
                End If

            End If
        Next


        Dim addressedQuoteObj As New Taradel.ProductPriceQuote(SiteID, BaseProductID, addressedObjCalc.MailPieces, holdQTY, extraCopies, UserDistributionId, sendThisNumberOfDrops, options, ZipCode.Text, bProDesign, bTemplateDesign, MarkUp, MarkUpType)

        'Set Proper Weight, Price, Taxable Price, etc. These values and variables are needed/used later in the code.
        addressedPricePerPiece = addressedQuoteObj.PricePerPiece
        weight = addressedQuoteObj.Weight
        addressedPrice = addressedQuoteObj.Price
        taxablePrice = addressedPrice
        flatRateFee = addressedQuoteObj.ShipPrice
        dropPrice = CalculateMultipleImpressionDropFee(addressedObjCalc.NumOfDrops, DropFeeRate)
        postageFee = (totalMailQty * postageRate)
        bIsFlatRateShipping = addressedQuoteObj.IsFlatRateShipping


        'Set Design Fee Attribute. Store 0 unless Pro was selected.
        If designType.ToLower() <> "pro" Then
            DesignFee = 0
        End If


        'ADD THE ADDRESSED PRODUCT NODE
        oCart = (CartUtility.AddProduct(oCart, DateTime.Now.ToString(), BaseProductID, DesignFee, UserDistributionId, flatRateFee, flatRateShipQTY, addressedGUID, bIsFlatRateShipping, comments, sJobName, ProductName, PaperHeight, PaperWidth, postageFee, addressedPrice, addressedPricePerPiece, ProductID, totalMailQty, SiteID, SKU, taxablePrice, "AddressedList", weight))




        '2) ATTRIBUTE NODES
        For Each oOpt As Taradel.PMOptionInfo In addressedQuoteObj.SelectedOptions

            Dim dOptMarkup As Double = oOpt.BasePrice
            Dim bPercent As Boolean = oOpt.BaseMarkupPercent

            'This applies the override if one is defined for this option
            If oOpt.PriceMatrixOptionMarkup IsNot Nothing Then
                dOptMarkup = oOpt.PriceMatrixOptionMarkup.Markup
                bPercent = oOpt.PriceMatrixOptionMarkup.MarkupPercent
            End If

            oCart = (CartUtility.AddAttribute(oCart, "AddressedList", oOpt.OptCatName, oOpt.OptCatId.ToString(), oOpt.OptionId.ToString(), oOpt.OptName, dOptMarkup.ToString(), bPercent.ToString(), oOpt.Weight.Value.ToString()))

        Next


        'Professional Service Attribute
        If designType = "pro" Then
            oCart = (CartUtility.AddAttribute(oCart, "AddressedList", "Professional Design Services", "0", "1", PaperWidth & "x" & PaperHeight & " (" & DesignFee.ToString("C") & ")", DesignFee.ToString(), "False", "0"))
        End If


        'Postage Fee Attribute
        oCart = (CartUtility.AddAttribute(oCart, "AddressedList", "Postage Fee", "0", postageFee.ToString(), addressedObjCalc.MailPieces.ToString("N0") & " pieces (" & postageFee.ToString("C") & ")", postageFee.ToString(), "False", "0"))


        'Number of Drops Attribute
        oCart = (CartUtility.AddAttribute(oCart, "AddressedList", "Number of Drops", "0", addressedObjCalc.NumOfDrops.ToString(), addressedObjCalc.NumOfDrops & " drop" & IIf(addressedObjCalc.NumOfDrops = 1, "", "s") & _
                                " (" & dropPrice.ToString("C") & ")", dropPrice.ToString(), "False", "0"))

        'Drop Schedule Attribute
        oCart = (CartUtility.AddAttribute(oCart, "AddressedList", "Drop Schedule", "0", "every " & iFrequency.ToString() & " weeks", iFrequency.ToString(), "0", "False", "0"))




        '3) ADDRESSED ORDER CALC NODE
        oCart = (CartUtility.AddOrderCalc(oCart, "AddressedList", "0", "0", oCust.State, "", "0", "0", "0", "0", "0"))




        '4) ADDRESSED DROPS NODE
        'Determine if Distribution has Businesses and PO Boxes selected...
        Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(Me.Distribution.ReferenceId)

        If oSummary IsNot Nothing Then
            useBusinesses = oSummary.UseBusiness
            usePOBoxes = oSummary.UsePOBox
        End If

        oCart = (CartUtility.AddDrops(oCart, "AddressedList", oCust.Company, oCust.Address1, oCust.Address2, oCust.City, oCust.State, oCust.ZipCode, "", "", useResidential, useBusinesses, usePOBoxes))

        '5) ADDRESSED INDIV DROPS
        Dim addressedDropBreak As Integer = addressedObjCalc.MailPieces / addressedObjCalc.NumOfDrops
        Dim addressedOnePer As Boolean = False

        Dim oDist As Taradel.CustomerDistribution = Nothing
        Dim addressedUSelect As Taradel.USelectProductConfiguration = Nothing

        oDist = Taradel.CustomerDistributions.GetDistribution(UserDistributionId)

        If oDist IsNot Nothing Then
            addressedUSelect = Taradel.Helper.USelect.GetProduct(oDist.USelectMethodReference.ForeignKey, Me.BaseProductID)
        End If





        'If this is a Multiple Impression distribution
        If addressedObjCalc.IsThisAMultiple Then

            'this is a muliple - so divide back out the tempObjCalc.MailPieces for each drop
            addressedObjCalc.MailPieces = (addressedObjCalc.MailPieces / addressedObjCalc.NumOfDrops)
            addressedDropBreak = addressedObjCalc.MailPieces
            addressedOnePer = False
            'addressedPostageFee = addressedPostageFee + (addressedObjCalc.MailPieces * addressedUSelect.MailPricePerPiece)    <--- may need to revisit this. 6/2/2015


            For dropNumber As Integer = 1 To addressedObjCalc.NumOfDrops

                Dim j As Integer = 0
                Dim dDropDate As DateTime = earliestDropDate.AddDays((dropNumber - 1) * daysToAdd)

                'Find Friday
                While (dDropDate.DayOfWeek <> DayOfWeek.Friday)
                    dDropDate = dDropDate.AddDays(-1)
                End While



                'Add the Drop Nodes
                oCart = CartUtility.AddIndividualDrop(oCart, "AddressedList", dropNumber, addressedObjCalc.MailPieces, dDropDate.ToShortDateString(), True, addressedObjCalc.NumOfDrops)


            Next


            'Single Impression
        Else


            Dim evenSplitInto As Integer = 0
            Dim remainderPcs As Integer = 0
            Dim dropCount As Integer = 0

            evenSplitInto = totalMailQty \ addressedObjCalc.NumOfDrops
            remainderPcs = totalMailQty Mod addressedObjCalc.NumOfDrops


            For dropNumber As Integer = 1 To addressedObjCalc.NumOfDrops

                Dim dDropDate As DateTime = earliestDropDate.AddDays((dropNumber - 1) * daysToAdd)

                'Find the Friday
                While (dDropDate.DayOfWeek <> DayOfWeek.Friday)
                    dDropDate = dDropDate.AddDays(-1)
                End While

                dropCount = evenSplitInto


                If dropNumber < remainderPcs + 1 Then
                    dropCount = dropCount + 1
                End If


                'Add the single Drop
                oCart = CartUtility.AddIndividualDrop(oCart, "AddressedList", dropNumber, dropCount, dDropDate.ToShortDateString(), False, "0")

            Next


        End If





        '6) ADDRESSED DESIGN NODE
        'Upload and Save the file first and then ...update the cart.
        Dim sClientBase As String = Taradel.WLUtil.GetRelativeSiteImagesPath & "/UserImages"
        Dim frontAction As String = ""
        Dim frontFileName As String = ""
        Dim frontFileExt As String = ""
        Dim frontFileSize As Long = 0
        Dim frontTmpName As String = ""
        Dim frontRealFileName As String = ""
        Dim backAction As String = ""
        Dim backFileName As String = ""
        Dim backFileExt As String = ""
        Dim backFileSize As Long = 0
        Dim backTmpName As String = ""
        Dim backRealFileName As String = ""
        Dim hasBackDesign As Boolean = False
        Dim artKey As String = ""
        Dim requiresProof As Boolean = False


        If Not Directory.Exists(Server.MapPath(sClientBase)) Then
            Directory.CreateDirectory(Server.MapPath(sClientBase))
        End If


        Dim sClientFolder As String = sClientBase & "/" & Profile.UserName.Replace("@", "_")
        Dim sClientPath As String = Server.MapPath(sClientFolder)


        Dim oDir As New DirectoryInfo(sClientPath)
        If Not oDir.Exists Then
            oDir.Create()
        End If


        Select Case designType.ToLower
            Case "my"

                requiresProof = chkRequestProof.Checked

                If File1.HasFile Then

                    frontAction = "Upload"

                    frontFileName = File1.FileName
                    Dim oUpload1 As FileInfo = New FileInfo(frontFileName)
                    frontFileExt = oUpload1.Extension
                    frontFileSize = File1.ContentLength
                    frontTmpName = System.Guid.NewGuid.ToString
                    frontRealFileName = frontTmpName & frontFileExt

                    File1.MoveTo(Path.Combine(sClientPath, frontTmpName & frontFileExt), Brettle.Web.NeatUpload.MoveToOptions.None)

                Else
                    frontAction = "Omitted"
                End If


                If File2.HasFile Then

                    hasBackDesign = True

                    '-- They have uploaded a file
                    backAction = "Upload"

                    backFileName = File2.FileName
                    Dim oUpload2 As FileInfo = New FileInfo(backFileName)
                    backFileExt = oUpload2.Extension
                    backTmpName = System.Guid.NewGuid.ToString
                    backRealFileName = backTmpName & backFileExt

                    File2.MoveTo(Path.Combine(sClientPath, backTmpName & backFileExt), Brettle.Web.NeatUpload.MoveToOptions.None)

                Else
                    backAction = "Omitted"
                End If



            Case "template"

                'ArtKey could be determined by either the hidPrevSelectedTemplateID (from a previous order OR coming from the Design page).
                'Otherwise, we will pull this value from the hidSelectedTemplateID.
                If hidSelectedTemplateID.Value = "0" Then
                    artKey = hidPrevSelectedTemplateID.Value
                Else
                    artKey = hidSelectedTemplateID.Value
                End If

            Case "pro"
                'Nothing yet...

        End Select



        'Add Design Node
        oCart = (CartUtility.AddDesign(oCart, "AddressedList", designType, frontFileExt, frontFileName, frontRealFileName, frontAction, hasBackDesign, _
        backFileExt, backFileName, backRealFileName, backAction, artKey, requiresProof.ToString().ToUpper()))




        '7) ADDRESSED SHIPMENTS NODE
        If UserDistributionId > 0 Then

            WeightPerPage = (weight * PageCount) / AddressedSelected

            If extraCopies > 0 Then

                If deliveryAddressID = "0" Then

                    '-- Create a new address and get the ID
                    Dim sMsg As String = ""
                    Dim iAddr As Integer = Taradel.CustomerAddressDataSource.NewAddress(oCust.CustomerID, "", "", "", "", DeliveryAddress.Text, DeliveryAddress2.Text, DeliveryCity.Text, USAStatesDropDown.SelectedValue, ZipCode.Text, sMsg)

                    If iAddr > 0 Then
                        deliveryAddressID = iAddr.ToString()
                    End If

                End If

            End If

            oCart = (CartUtility.AddShipments(oCart, "AddressedList", deliveryAddressID, extraCopies, weight, PageCount, PaperWidth, PaperHeight, flatRateFee.ToString("N2"), zip, totalMailQty))

        End If






        '8) DEMOGRAPHICS
        'Add Demographics to XML/Cart.  9 Filters so far. 
        'Currently, this is only created and inserted when AddressList types are selected.  Will insert and create a primary node like Shipments, Design, etc.

        'Dim filterList As New List(Of String)
        'filterList = DistributionUtility.RetrieveAddressedFilters(referenceID)

        'oCart = (CartUtility.AddDemographics(oCart, "AddressedList", UserDistributionId, filterList))



        Profile.Cart = oXML
        Profile.Save()


    End Sub



    Private Sub BuildAndSaveTMCCart()

        '========================================================================================================================================
        '   We will need two quote objects, two products and so on. One for EDDM Product and one for AddressedList Product. Any additional fees
        '   such as Professional Design Fee, drop fee, and so on will be associated with the AddressedList Product 'half' of this distribution.
        '   In otherwords, we do not want to charge twice for Professional Design (one for the EDDM and one for the Addressed product), multiple
        '   mulitple impressions, and so on.
        '
        '   The customer is only viewing the AddressedList Product on this page.  The EDDM 'matching' product is calculated and built behind the 
        '   scenes. Furthermore, ExtraCopies can be ordered but are only associated with and calculated by the AddressedList Product. 
        '
        '   Certain variables such as ExtraCopies, DesignFees, Impression/Drop Fees are set intentionally to 0 on the EDDM portion of this
        '   cart (product obj, quote obj) so the customer is not double charged.
        '
        '   USelect Types 7.
        '
        '   Cart Build Steps:
        '   1) EDDM Product
        '   2) EDDM Attribute Nodes
        '   3) EDDM OrderCalc Node
        '   4) EDDM Drops Node
        '   5) EDDM Indiv Drop Nodes w/ nested Area Nodes
        '   6) EDDM Design Node
        '   7) EDDM SHIPMENTS Node and w/ nested Shipment node(s)
        '   8) ADDRESSED Product
        '   9) ADDRESSED Attribute Nodes
        '   10) ADDRESSED OrderCacl Node
        '   11) ADDRESSED Drops Node
        '   12) ADDRESSED Indiv Drop Nodes w/ nested List
        '   13) ADDRESSED Design Node
        '   14) ADDRESSED SHIPMENTS Node and w/ nested Shipment node(s)
        '   15) ADDRESSED Demographics Node
        '   16) Add TMCData Node
        '
        '========================================================================================================================================


        '' ''Get 2 distinct order calculator objs.  One for EDDM product and other for 'visible' AddressedList prod.
        ' ''Dim eddmObjCalc As New TaradelReceiptUtility.OrderCalculator()
        ' ''Dim addressedObjCalc As New TaradelReceiptUtility.OrderCalculator()


        '' ''For debugging
        ' ''Dim methodVars As New StringBuilder()


        '' ''Form values
        ' ''Dim jobName As String = hidJobName.Value                                                            'Job name
        ' ''Dim comments As String = JobComments.Text                                                           'User comments for order.
        ' ''Dim sCustomField1 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription1")         'possibly obsolete.
        ' ''Dim sCustomField2 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription2")         'possibly obsolete.
        ' ''Dim sCustomField3 As String = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription3")         'possibly obsolete.
        ' ''Dim designType As String = ddlDesignOption.SelectedValue.ToLower                                    'User selected Design Type.  "my", "pro", etc..
        ' ''Dim deliveryAddressID As String = ddlDeliveryAddressId.SelectedValue                                'Ship To address for Extra Copies.
        ' ''Dim zip As String = ZipCode.Text                                                                    'Ship To zip code.
        ' ''Dim earliestDropDate As DateTime = Date.Parse(txtLaunchWeek.Text)                                   'User selected 'Launch Week'
        ' ''Dim mailAllPiecesAtOnce As String = ddlDrops.SelectedValue                                          'Comes from form.  Yes/No - True/False
        ' ''Dim requiresProof As Boolean = chkRequestProof.Checked                                              'If user required a Proof of artowrk or not.

        '' ''Various Quantities
        ' ''Dim holdQTY As Integer = 0                                                                          'Obsolete concept. Intentionally set to 0.

        '' ''Distribution values
        ' ''Dim numDrops As Integer = 0                                                                         'Num of drops.
        ' ''Dim numImpressions As Integer = 0                                                                   'Num of impressions.
        ' ''Dim sendThisNumberOfDrops As Int16 = 0                                                              'Adjusted # of Drops to send to oPriceQuote. Work Around.
        ' ''Dim iFrequency As Integer = 0                                                                       'Every X of weeks - selected in DropDownList.
        ' ''Dim daysToAdd As Integer = 1                                                                        'Used to logically find next drop date. AKA 'weekInterval'

        '' ''Rates / Fees
        ' ''Dim postageRate As Decimal = 0                                                                      'Postage rate for Product


        '' ''Obsolete
        '' ''Dim iNumberOfTimesToMail As Integer = 0                                                            'obsolete 6/3/2015.


        '' ''EDDM specific variables
        ' ''Dim eddmGUID As String = System.Guid.NewGuid.ToString
        ' ''Dim eddmProductID As Integer = 0
        ' ''Dim eddmBaseProductID As Integer = 0
        ' ''Dim eddmPrice As Double = 0                                                                         'Stores calculated total price from Quote Obj.  (total mailed x price per piece)
        ' ''Dim eddmPostageFee As Double = 0                                                                    'Total postage fee.  EDDM total mailed x postage rate from app config.
        ' ''Dim eddmDesignFee As Double = 0                                                                     'Intentionally set to 0.
        ' ''Dim eddmPricePerPiece As Double = 0                                                                 'Stores EDDM Price Per Piece from Quote Obj.
        ' ''Dim eddmWeight As Double = 0                                                                        'Comes from Base Product Obj
        ' ''Dim eddmTaxablePrice As Double = 0                                                                  'Currently same as eddmPrice / eddmQuoteObj.Price
        ' ''Dim eddmIsFlatRateShipping As Boolean = False                                                       'Comes from eddmQuoteObj.
        ' ''Dim eddmFlatRateFee As Decimal = 0                                                                  'Comes from eddmQuoteObj.ShipPrice.
        ' ''Dim eddmFlatRateShipQTY As Integer = 1000                                                           'Hard coded to 1000.
        ' ''Dim eddmExtraCopies As Integer = 0                                                                  'Intentionally set to 0 and not pulled from UI.
        ' ''Dim eddmUseBusinesses As Boolean = False                                                            'Intentionally set to False.
        ' ''Dim eddmUsePOBoxes As Boolean = False                                                               'Intentionally set to False.
        ' ''Dim eddmUseResidential As Boolean = True                                                            'Always True.
        ' ''Dim eddmProductName As String = ""                                                                  'Comes from eddmProdObj.Name.
        ' ''Dim eddmSKU As String = ""                                                                          'Comes from eddmBaseProdObj.SKU.
        ' ''Dim eddmPaperHeight As Decimal = 0                                                                  'Comes from eddmBaseProdObj.
        ' ''Dim eddmPaperWidth As Decimal = 0                                                                   'Comes from eddmBaseProdObj.
        ' ''Dim eddmPageCount As Integer = 0                                                                    'Comes from eddmBaseProdObj.
        ' ''Dim eddmMarkUp As Decimal = 0                                                                       'Comes from eddmProdObj.
        ' ''Dim eddmMarkUpType As String = ""                                                                   'Comes from eddmProdObj.
        ' ''Dim eddmExtraDropFee As Double = 0                                                                  'Intentionally set to 0. Normally the fee per extra Drop like $99.
        ' ''Dim eddmWeightPerPage As Double = 0                                                                 'Used for Shipments Node. (eddmWeight * eddmPageCount) / EDDMSelected
        ' ''Dim eddmTotalMailQty As Integer = 0                                                                 'Total Mailed.  (Total Selected  * Impressions)
        ' ''Dim eddmPrintQTY As Integer = 0                                                                     'PER IMPRESSION. (Total Selected + holdQTY + extraCopies)
        ' ''Dim eddmTotalDropSelections = 0                                                                     'Used as a running total to store total 'matches' in the EDDM DataTable
        ' ''Dim eddmDropPrice As Double = 0                                                                     'Intentionally set to 0. Fee for additional drops. Ex $99 x number of drops.


        '' ''AddressedList variables
        ' ''Dim addressedGUID As String = System.Guid.NewGuid.ToString
        ' ''Dim addressedPrintQTY As Integer = 0                                                               'PER IMPRESSION. (totalSelected + holdQTY + extraCopies)
        ' ''Dim addressedExtraCopies As Integer = 0                                                             'Provided by user.
        ' ''Dim addressedPerPiecePrice As Double = 0                                                            'Stores Addressed Price Per Piece from Quote Obj.
        ' ''Dim addressedFlatRateFee As Decimal = 0                                                             'Comes from addressedQuoteObj.ShipPrice.
        ' ''Dim addressedFlatRateShipQTY As Integer = 1000                                                      'Hard coded to 1000.
        ' ''Dim addressedIsFlatRateShipping As Boolean = False                                                  'Comes from addressedQuoteObj.
        ' ''Dim addressedPostageFee As Double = 0                                                               'Total postage fee.  Addressed total mailed x postage rate from app config
        ' ''Dim addressedTotalMailQty As Integer = 0                                                            'Total Mailed.  (Total Selected  * Impressions)
        ' ''Dim addressedPrice As Double = 0                                                                    'Stores calculated total price from Quote Obj.  (total mailed x price per piece)
        ' ''Dim addressedTaxablePrice As Double = 0                                                             'Currently same as addressedPrice / addressedQuoteObj.Price
        ' ''Dim addressedExtraDropFee As Double = 0                                                             'Intentionally set to 0.
        ' ''Dim addressedDropPrice As Double = 0                                                                'Intentionally set to 0.
        ' ''Dim addressedUseBusinesses As Boolean = False                                                       'Value comes from Distribution object
        ' ''Dim addressedUsePOBoxes As Boolean = False                                                          'Value comes from Distribution object
        ' ''Dim addressedUseResidential As Boolean = True                                                       'Always True.
        ' ''Dim addressedTotalDropSelections = 0                                                                'Used as a running total to store total 'matches' in the Addressed DataTable
        ' ''Dim addressedWeightPerPage As Double = 0                                                            'Used for Shipments Node. (addressedWeight * addressedPageCount) / AddressedSelected
        ' ''Dim addressedWeight As Double = 0                                                                   'Comes from Base Product Obj
        ' ''Dim addressedPageCount As Integer = 0                                                               'Comes from eddmBaseProdObj.



        '' ''Get Design Type - is it Pro or Template? Set flags
        ' ''Dim bProDesign As Boolean = False
        ' ''If designType = "pro" Then
        ' ''    bProDesign = True
        ' ''End If

        ' ''Dim bTemplateDesign As Boolean = False
        ' ''If designType = "template" Then
        ' ''    bTemplateDesign = True
        ' ''End If


        '' ''Convert Stuff
        ' ''Integer.TryParse(hidHoldQuantity.Value, holdQTY)
        ' ''Integer.TryParse(txtExtraCopies.Text, addressedExtraCopies)
        ' ''Integer.TryParse(ddlImpressions.SelectedValue, numImpressions)
        ' ''Integer.TryParse(hidTotalSelected.Value, TotalSelected)
        ' ''Integer.TryParse(ddlNumOfDrops.SelectedValue, numDrops)
        ' ''Integer.TryParse(ddlFrequency.SelectedValue, iFrequency)
        '' ''Integer.TryParse(ddlImpressions.SelectedValue, iNumberOfTimesToMail)                               'obsolete 6/3/2015



        '' ''Postage Rate
        ' ''postageRate = appxCMS.Util.AppSettings.GetDecimal("USPSPostageRate")
        ' ''If postageRate = 0 Then
        ' ''    postageRate = 0.16
        ' ''End If


        '' ''Set some of the properties of the eddmObJCalc and variables. Calculate as needed.
        ' ''eddmObjCalc.ExtraPieces = eddmExtraCopies                                                               'Will always be 0 for now.
        ' ''eddmPrintQTY = CalculatePrintQTY(EDDMSelected, holdQTY, eddmExtraCopies)
        ' ''eddmTotalMailQty = ((EDDMSelected) * numImpressions)
        ' ''eddmPostageFee = (eddmTotalMailQty * postageRate)

        ' ''addressedObjCalc.ExtraPieces = addressedExtraCopies
        ' ''addressedPrintQTY = CalculatePrintQTY(AddressedSelected, holdQTY, addressedExtraCopies)
        ' ''addressedTotalMailQty = ((AddressedSelected) * numImpressions)
        ' ''addressedPostageFee = (addressedTotalMailQty * postageRate)
        ' ''addressedExtraDropFee = CalculateMultipleImpressionDropFee(addressedObjCalc.NumOfDrops, addressedDropPrice)


        '' ''Multiple Impressions logic.
        '' ''If is Multiple Impressions, change the number of drops to match the number of impressions
        ' ''If (numImpressions = 1) Then

        ' ''    eddmObjCalc.IsThisAMultiple = False
        ' ''    eddmObjCalc.NumOfDrops = numDrops
        ' ''    eddmObjCalc.MailPieces = EDDMSelected

        ' ''    addressedObjCalc.IsThisAMultiple = False
        ' ''    addressedObjCalc.NumOfDrops = numDrops
        ' ''    addressedObjCalc.MailPieces = AddressedSelected

        ' ''    If mailAllPiecesAtOnce = "Yes" Then
        ' ''        eddmObjCalc.NumOfDrops = 1
        ' ''        addressedObjCalc.NumOfDrops = 1
        ' ''        numDrops = 1
        ' ''    End If

        ' ''Else

        ' ''    eddmObjCalc.IsThisAMultiple = True
        ' ''    eddmObjCalc.NumOfDrops = numImpressions
        ' ''    eddmObjCalc.MailPieces = (EDDMSelected * numImpressions)

        ' ''    addressedObjCalc.IsThisAMultiple = True
        ' ''    addressedObjCalc.NumOfDrops = numImpressions
        ' ''    addressedObjCalc.MailPieces = (AddressedSelected * numImpressions)

        ' ''End If



        '' ''Adjust the number of drops if needed.
        '' ''The Quote Obj ADDS on the drop fees based on the number of drops since we are NOT charging for Multiple Impressions on SOME sites set the number of drops to 1 for these sites.
        ' ''sendThisNumberOfDrops = eddmObjCalc.NumOfDrops
        ' ''If (bMultipleImpressionsNoFee) Then
        ' ''    sendThisNumberOfDrops = 1
        ' ''End If

        ' ''If (numDrops = 0) Then
        ' ''    numDrops = 1
        ' ''End If



        '' ''Custom Jobs.  Possibly obsolete concept.
        ' ''If Not String.IsNullOrEmpty(sCustomField1) Then
        ' ''    Dim oJobName As New List(Of String)
        ' ''    oJobName.Add(JobNameCustom1.Text.Trim)

        ' ''    If Not String.IsNullOrEmpty(sCustomField2) Then
        ' ''        oJobName.Add(JobNameCustom2.Text.Trim)
        ' ''    End If

        ' ''    If Not String.IsNullOrEmpty(sCustomField3) Then
        ' ''        oJobName.Add(JobNameCustom3.Text.Trim)
        ' ''    End If

        ' ''    jobName = String.Join("-", oJobName.ToArray)
        ' ''End If



        '' ''BUILD THE CART
        ' ''Dim oXML As New XmlDocument
        ' ''oXML.LoadXml("<cart />")

        ' ''Dim oCart As XmlNode = oXML.SelectSingleNode("/cart")





        '' ''1) EDDM PRODUCT NODE 
        '' ''We will need to extract these to accurately get a weighted quote.  
        '' ''The page properties will automatically be filled with values from the AddressedList Product so we need new vars to hold these two IDs.
        ' ''eddmProductID = ProductUtility.GetEddmProductID(ProductID, SiteID)
        ' ''eddmBaseProductID = ProductUtility.GetEddmBaseProductID(ProductID, SiteID)

        ' ''Dim eddmProdObj As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(eddmProductID)
        ' ''Dim eddmBaseProdObj As Taradel.Product = Taradel.ProductDataSource.GetEffectiveProduct(eddmBaseProductID)


        '' ''Set EDDM specific variables based on return of eddm prod objs.
        ' ''eddmProductName = eddmProdObj.Name
        ' ''eddmDesignFee = 0
        ' ''eddmSKU = eddmBaseProdObj.SKU
        ' ''eddmPaperWidth = eddmBaseProdObj.PaperWidth
        ' ''eddmPaperHeight = eddmBaseProdObj.PaperHeight
        ' ''eddmPageCount = eddmBaseProdObj.PageCount  '<-----find page count in EDDM Cart Build....
        ' ''eddmMarkUp = eddmProdObj.Markup
        ' ''eddmMarkUpType = eddmProdObj.MarkupType



        '' ''Get available Product Options
        '' ''Build the selected options from what we have in the categories for this product and the underlying field values
        ' ''Dim eddmProdOptions As New SortedList(Of Integer, Integer)
        ' ''Dim oEddmOptCats As List(Of Taradel.ProductOptionCategory) = Taradel.ProductDataSource.GetProductOptionCategories(eddmBaseProductID)
        ' ''Dim oEddmPriceMatrix As Taradel.PriceMatrix = Taradel.ProductDataSource.GetPriceRange(eddmBaseProductID, eddmPrintQTY)


        '' ''Get EDDM Price Range
        ' ''If oEddmPriceMatrix IsNot Nothing Then

        ' ''    Dim printMethodId As Integer = oEddmPriceMatrix.PrintMethod.PrintMethodId

        ' ''    For Each oOptCat As Taradel.ProductOptionCategory In oEddmOptCats
        ' ''        Dim oOpts As IEnumerable(Of Taradel.ProductOption) = oOptCat.Options.Where(Function(po As Taradel.ProductOption) po.ProductPrintMethodOptions.Any(Function(ppmo As Taradel.ProductPrintMethodOption) ppmo.PrintMethodReference.ForeignKey = printMethodId))
        ' ''        If oOpts.Count > 0 Then
        ' ''            eddmProdOptions.Add(oOptCat.OptCatID, 0)
        ' ''        End If
        ' ''    Next

        ' ''Else

        ' ''    pnlNormal.Visible = False
        ' ''    pnlError.Visible = True
        ' ''    litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
        ' ''    EmailUtility.SendAdminEmail("oEddmPriceMatrix IS Null. (Step2-ProductOptions.aspx)")
        ' ''    Return

        ' ''End If


        '' ''Now we have the price data, get the quote for the EDDM product
        ' ''Dim eddmQuoteObj As New Taradel.ProductPriceQuote(SiteID, eddmBaseProductID, eddmTotalMailQty, holdQTY, eddmExtraCopies, UserDistributionId, sendThisNumberOfDrops, eddmProdOptions, ZipCode.Text, bProDesign, bTemplateDesign, eddmMarkUp, eddmMarkUpType)


        '' ''Set remaining EDDM specific variables. Proper Weight, Price, Taxable Price, etc. 
        ' ''eddmPricePerPiece = eddmQuoteObj.PricePerPiece
        ' ''eddmWeight = eddmQuoteObj.Weight
        ' ''eddmPrice = eddmQuoteObj.Price
        ' ''eddmTaxablePrice = eddmPrice
        ' ''eddmIsFlatRateShipping = eddmQuoteObj.IsFlatRateShipping
        ' ''eddmFlatRateFee = eddmQuoteObj.ShipPrice



        '' ''ADD THE EDDM PRODUCT NODE
        ' ''oCart = (CartUtility.AddProduct(oCart, DateTime.Now.ToString(), eddmBaseProductID, eddmDesignFee, UserDistributionId, eddmFlatRateFee, eddmFlatRateShipQTY, eddmGUID, _
        ' ''                                eddmIsFlatRateShipping, comments, jobName, eddmProductName, eddmPaperHeight, eddmPaperWidth, eddmPostageFee, eddmPrice, eddmPricePerPiece, _
        ' ''                                eddmProductID, eddmTotalMailQty, SiteID, eddmSKU, eddmTaxablePrice, "EDDM", eddmWeight))







        '' ''2) EDDM PRODUCT ATTRIBUTE NODES 
        '' ''Loop through the attributes of the Product and insert a <Attribute> Node for each one.
        ' ''For Each oOpt As Taradel.PMOptionInfo In eddmQuoteObj.SelectedOptions

        ' ''    Dim dOptMarkup As Double = oOpt.BasePrice
        ' ''    Dim bPercent As Boolean = oOpt.BaseMarkupPercent

        ' ''    '-- This applies the override if one is defined for this option
        ' ''    If oOpt.PriceMatrixOptionMarkup IsNot Nothing Then
        ' ''        dOptMarkup = oOpt.PriceMatrixOptionMarkup.Markup
        ' ''        bPercent = oOpt.PriceMatrixOptionMarkup.MarkupPercent
        ' ''    End If

        ' ''    oCart = (CartUtility.AddAttribute(oCart, "EDDM", oOpt.OptCatName, oOpt.OptCatId.ToString(), oOpt.OptionId.ToString(), oOpt.OptName, dOptMarkup.ToString(), bPercent.ToString(), oOpt.Weight.Value.ToString()))

        ' ''Next


        '' ''Professional Service Attribute
        ' ''If designType = "pro" Then
        ' ''    oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Professional Design Services", "0", "1", eddmPaperWidth & "x" & eddmPaperHeight & " (" & eddmDesignFee.ToString("C") & ")", eddmDesignFee.ToString(), "False", "0"))
        ' ''End If


        '' ''Postage Fee Attribute
        ' ''oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Postage Fee", "0", eddmPostageFee.ToString(), eddmObjCalc.MailPieces.ToString("N0") & " pieces (" & eddmPostageFee.ToString("C") & ")", eddmPostageFee.ToString(), "False", "0"))


        '' ''Number of Drops Attribute
        ' ''oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Number of Drops", "0", eddmObjCalc.NumOfDrops.ToString(), eddmObjCalc.NumOfDrops & " drop" & IIf(eddmObjCalc.NumOfDrops = 1, "", "s") & _
        ' ''                        " (" & eddmExtraDropFee.ToString("C") & ")", eddmExtraDropFee.ToString(), "False", "0"))

        '' ''Drop Schedule Attribute
        ' ''oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Drop Schedule", "0", "every " & iFrequency.ToString() & " weeks", iFrequency.ToString(), "0", "False", "0"))




        '' ''3) EDDM ORDER CALC NODE
        ' ''oCart = (CartUtility.AddOrderCalc(oCart, "EDDM", "0", "0", oCust.State, "", "0", "0", "0", "0", "0"))




        '' ''4) EDDM DROPS NODE
        ' ''oCart = (CartUtility.AddDrops(oCart, "EDDM", oCust.Company, oCust.Address1, oCust.Address2, oCust.City, oCust.State, oCust.ZipCode, "", "", eddmUseResidential, eddmUseBusinesses, eddmUsePOBoxes))




        '' ''5) EDDM DROP (INDIV) NODES
        ' ''Dim dDropBreak As Integer = 0
        ' ''Dim bOnePer As Boolean = False
        ' ''Dim oDist As Taradel.CustomerDistribution = Nothing
        ' ''Dim oUSelect As Taradel.USelectProductConfiguration = Nothing

        ' ''oDist = Taradel.CustomerDistributions.GetDistribution(UserDistributionId)

        ' ''If oDist IsNot Nothing Then
        ' ''    oUSelect = Taradel.Helper.USelect.GetProduct(oDist.USelectMethodReference.ForeignKey, eddmBaseProductID)
        ' ''End If


        '' ''Table is used instead of the previous oSelects obj.  Data is stored in new table (AddressedListSelections)
        ' ''Dim eddmTable As DataTable
        ' ''eddmTable = DistributionUtility.RetrieveAddressedSelections(referenceID, "eddm")


        '' ''If this is a Multiple Impression distribution
        ' ''If eddmObjCalc.IsThisAMultiple Then

        ' ''    'this is a muliple - so divide back out the tempObjCalc.MailPieces for each drop
        ' ''    eddmObjCalc.MailPieces = (eddmObjCalc.MailPieces / eddmObjCalc.NumOfDrops)
        ' ''    dDropBreak = eddmObjCalc.MailPieces
        ' ''    bOnePer = False
        ' ''    'eddmPostageFee = eddmPostageFee + (eddmObjCalc.MailPieces * oUSelect.MailPricePerPiece)    <--- may need to revisit this. 6/2/2015


        ' ''    For dropNumber As Integer = 1 To eddmObjCalc.NumOfDrops

        ' ''        Dim j As Integer = 0
        ' ''        Dim dDropDate As DateTime = earliestDropDate.AddDays((dropNumber - 1) * daysToAdd)

        ' ''        While (dDropDate.DayOfWeek <> DayOfWeek.Friday)
        ' ''            dDropDate = dDropDate.AddDays(-1)
        ' ''        End While


        ' ''        'Add the Drop Nodes
        ' ''        oCart = CartUtility.AddIndividualDrop(oCart, "EDDM", dropNumber, dDropDate.ToShortDateString(), True, eddmObjCalc.NumOfDrops)


        ' ''        'Add Areas Nodes
        ' ''        For Each row As DataRow In eddmTable.Rows
        ' ''            oCart = CartUtility.AddIndividualArea(oCart, "EDDM", dropNumber, row("Name"), row("FriendlyName"), row("Matches"))
        ' ''            eddmTotalDropSelections = eddmTotalDropSelections + row("Matches")
        ' ''        Next row


        ' ''        'Update total with new value
        ' ''        oCart = CartUtility.UpdateDropCount(oCart, "EDDM", dropNumber, eddmTotalDropSelections)

        ' ''        'reset
        ' ''        eddmTotalDropSelections = 0

        ' ''    Next

        ' ''    'Clean up
        ' ''    eddmTable.Dispose()


        ' ''    'Single Impression
        ' ''Else

        ' ''    For dropNumber As Integer = 1 To eddmObjCalc.NumOfDrops

        ' ''        Dim dDropDate As DateTime = earliestDropDate.AddDays((dropNumber - 1) * daysToAdd)

        ' ''        'eddmPostageFee = eddmPostageFee + (EDDMSelected * oUSelect.MailPricePerPiece)    <--- may need to revisit this. 6/2/2015
        ' ''        dDropBreak = (eddmObjCalc.MailPieces / eddmObjCalc.NumOfDrops)

        ' ''        'Possible obsolete here.
        ' ''        If numDrops = eddmTable.Rows.Count And eddmObjCalc.NumOfDrops > 1 Then
        ' ''            bOnePer = True
        ' ''        End If

        ' ''        'Find the Friday
        ' ''        While (dDropDate.DayOfWeek <> DayOfWeek.Friday)
        ' ''            dDropDate = dDropDate.AddDays(-1)
        ' ''        End While


        ' ''        'Add the single Drop
        ' ''        oCart = CartUtility.AddIndividualDrop(oCart, "EDDM", dropNumber, dDropDate.ToShortDateString(), False, "0")


        ' ''        'Add Areas
        ' ''        For Each row As DataRow In eddmTable.Rows
        ' ''            oCart = CartUtility.AddIndividualArea(oCart, "EDDM", dropNumber, row("Name"), row("FriendlyName"), row("Matches"))
        ' ''            eddmTotalDropSelections = eddmTotalDropSelections + row("Matches")
        ' ''        Next row


        ' ''        'Update total with new value
        ' ''        oCart = CartUtility.UpdateDropCount(oCart, "EDDM", dropNumber, eddmTotalDropSelections)

        ' ''        'Clean up
        ' ''        eddmTable.Dispose()

        ' ''    Next


        ' ''End If





        '' ''6) EDDM DESIGN NODE
        '' ''Upload and Save the file first and then ...update the cart.
        ' ''Dim sClientBase As String = Taradel.WLUtil.GetRelativeSiteImagesPath & "/UserImages"
        ' ''Dim frontAction As String = ""
        ' ''Dim frontFileName As String = ""
        ' ''Dim frontFileExt As String = ""
        ' ''Dim frontFileSize As Long = 0
        ' ''Dim frontTmpName As String = ""
        ' ''Dim frontRealFileName As String = ""
        ' ''Dim backAction As String = ""
        ' ''Dim backFileName As String = ""
        ' ''Dim backFileExt As String = ""
        ' ''Dim backFileSize As Long = 0
        ' ''Dim backTmpName As String = ""
        ' ''Dim backRealFileName As String = ""
        ' ''Dim hasBackDesign As Boolean = False
        ' ''Dim artKey As String = ""


        ' ''If Not Directory.Exists(Server.MapPath(sClientBase)) Then
        ' ''    Directory.CreateDirectory(Server.MapPath(sClientBase))
        ' ''End If


        ' ''Dim sClientFolder As String = sClientBase & "/" & Profile.UserName.Replace("@", "_")
        ' ''Dim sClientPath As String = Server.MapPath(sClientFolder)


        ' ''Dim oDir As New DirectoryInfo(sClientPath)
        ' ''If Not oDir.Exists Then
        ' ''    oDir.Create()
        ' ''End If


        ' ''Select Case designType.ToLower
        ' ''    Case "my"

        ' ''        If File1.HasFile Then

        ' ''            frontAction = "Upload"

        ' ''            frontFileName = File1.FileName
        ' ''            Dim oUpload1 As FileInfo = New FileInfo(frontFileName)
        ' ''            frontFileExt = oUpload1.Extension
        ' ''            frontFileSize = File1.ContentLength
        ' ''            frontTmpName = System.Guid.NewGuid.ToString
        ' ''            frontRealFileName = frontTmpName & frontFileExt

        ' ''            File1.MoveTo(Path.Combine(sClientPath, frontTmpName & frontFileExt), Brettle.Web.NeatUpload.MoveToOptions.None)

        ' ''        Else
        ' ''            frontAction = "Omitted"
        ' ''        End If


        ' ''        If File2.HasFile Then

        ' ''            hasBackDesign = True

        ' ''            '-- They have uploaded a file
        ' ''            backAction = "Upload"

        ' ''            backFileName = File2.FileName
        ' ''            Dim oUpload2 As FileInfo = New FileInfo(backFileName)
        ' ''            backFileExt = oUpload2.Extension
        ' ''            backTmpName = System.Guid.NewGuid.ToString
        ' ''            backRealFileName = backTmpName & backFileExt

        ' ''            File2.MoveTo(Path.Combine(sClientPath, backTmpName & backFileExt), Brettle.Web.NeatUpload.MoveToOptions.None)

        ' ''        Else
        ' ''            backAction = "Omitted"
        ' ''        End If



        ' ''    Case "template"

        ' ''        'ArtKey could be determined by either the hidPrevSelectedTemplateID (from a previous order OR coming from the Design page).
        ' ''        'Otherwise, we will pull this value from the hidSelectedTemplateID.
        ' ''        If hidSelectedTemplateID.Value = "0" Then
        ' ''            artKey = hidPrevSelectedTemplateID.Value
        ' ''        Else
        ' ''            artKey = hidSelectedTemplateID.Value
        ' ''        End If

        ' ''    Case "pro"
        ' ''        'Nothing yet...

        ' ''End Select



        '' ''Add Design Node
        ' ''oCart = (CartUtility.AddDesign(oCart, "EDDM", designType, frontFileExt, frontFileName, frontRealFileName, frontAction, hasBackDesign, _
        ' ''backFileExt, backFileName, backRealFileName, backAction, artKey, requiresProof.ToString().ToUpper()))





        '' ''7) EDDM SHIPMENTS NODE
        ' ''If UserDistributionId > 0 Then

        ' ''    eddmWeightPerPage = (eddmWeight * eddmPageCount) / EDDMSelected


        ' ''    'Will always be 0 (for now) since the Extra Copies the user selects will be applied to the TMC/Blended Product and not the
        ' ''    'EDDM Product.
        ' ''    If eddmExtraCopies > 0 Then

        ' ''        If deliveryAddressID = "0" Then

        ' ''            '-- Create a new address and get the ID
        ' ''            Dim sMsg As String = ""
        ' ''            Dim iAddr As Integer = Taradel.CustomerAddressDataSource.NewAddress(oCust.CustomerID, "", "", "", "", DeliveryAddress.Text, DeliveryAddress2.Text, DeliveryCity.Text, USAStatesDropDown.SelectedValue, ZipCode.Text, sMsg)

        ' ''            If iAddr > 0 Then
        ' ''                deliveryAddressID = iAddr.ToString()
        ' ''                'Response.Write("<h1>" & deliveryAddressID & "</h1>")
        ' ''                'Response.End()
        ' ''            Else
        ' ''                'Response.Write("<h1>iAddr is 0</h1>")
        ' ''                'Response.End()

        ' ''            End If

        ' ''        Else
        ' ''            'Response.Write("<h1>deliveryAddressID is 0</h1>")
        ' ''            'Response.End()
        ' ''        End If

        ' ''    End If

        ' ''    oCart = (CartUtility.AddShipments(oCart, "EDDM", deliveryAddressID, eddmExtraCopies, eddmWeight, eddmPageCount, eddmPaperWidth, eddmPaperHeight, eddmFlatRateFee.ToString("N2"), zip, eddmTotalMailQty))

        ' ''End If




        '' ''8) ADDRESSED PRODUCT NODE
        '' '' The AddressedProduct will use standard page properties since this is the product the page build on like:
        '' ''   ProductName
        '' ''   PaperHeight
        '' ''   PaperWidth
        '' ''   SKU
        '' ''   MarkUp
        '' ''   MarkupType
        '' ''   DesignFee

        '' ''Get available Product Options
        '' ''Build the selected options from what we have in the categories for this product and the underlying field values
        ' ''Dim options As New SortedList(Of Integer, Integer)
        ' ''Dim oOptCats As List(Of Taradel.ProductOptionCategory) = Taradel.ProductDataSource.GetProductOptionCategories(Me.BaseProductID)
        ' ''Dim oPriceMatrix As Taradel.PriceMatrix = Taradel.ProductDataSource.GetPriceRange(Me.BaseProductID, addressedPrintQTY)



        '' ''Get Price Range
        ' ''If oPriceMatrix IsNot Nothing Then

        ' ''    Dim printMethodId As Integer = oPriceMatrix.PrintMethod.PrintMethodId

        ' ''    For Each oOptCat As Taradel.ProductOptionCategory In oOptCats
        ' ''        Dim oOpts As IEnumerable(Of Taradel.ProductOption) = oOptCat.Options.Where(Function(po As Taradel.ProductOption) po.ProductPrintMethodOptions.Any(Function(ppmo As Taradel.ProductPrintMethodOption) ppmo.PrintMethodReference.ForeignKey = printMethodId))
        ' ''        If oOpts.Count > 0 Then
        ' ''            options.Add(oOptCat.OptCatID, 0)
        ' ''        End If
        ' ''    Next

        ' ''Else

        ' ''    pnlNormal.Visible = False
        ' ''    pnlError.Visible = True
        ' ''    litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
        ' ''    EmailUtility.SendAdminEmail("oPriceMatrix in SaveTMCCart IS Null.")
        ' ''    Return

        ' ''End If


        '' ''Get the product options from the selected product (drop down list)
        ' ''For Each oItem As ListViewItem In lvProdOpts.Items

        ' ''    If oItem.ItemType = ListViewItemType.DataItem Then

        ' ''        Dim oDropDown As DropDownList = DirectCast(oItem.FindControl("ddlProdOpt"), DropDownList)
        ' ''        Dim sOptCatId As String = oDropDown.Attributes("optcatid")
        ' ''        Dim sOptVal As String = oDropDown.SelectedValue
        ' ''        Dim iOptCatId As Integer = 0
        ' ''        Dim iOptVal As Integer = 0

        ' ''        Integer.TryParse(sOptCatId, iOptCatId)

        ' ''        If options.ContainsKey(iOptCatId) Then
        ' ''            Integer.TryParse(sOptVal, iOptVal)
        ' ''            If iOptCatId > 0 Then
        ' ''                options(iOptCatId) = iOptVal

        ' ''            End If
        ' ''        End If

        ' ''    End If
        ' ''Next


        ' ''Dim addressedQuoteObj As New Taradel.ProductPriceQuote(SiteID, BaseProductID, addressedObjCalc.MailPieces, holdQTY, addressedExtraCopies, UserDistributionId, sendThisNumberOfDrops, options, ZipCode.Text, bProDesign, bTemplateDesign, MarkUp, MarkUpType)

        '' ''Set Proper Weight, Price, Taxable Price, etc. These values and variables are needed/used later in the code.
        ' ''addressedPerPiecePrice = addressedQuoteObj.PricePerPiece
        ' ''addressedWeight = addressedQuoteObj.Weight
        ' ''addressedPrice = addressedQuoteObj.Price
        ' ''addressedTaxablePrice = addressedPrice
        ' ''addressedIsFlatRateShipping = addressedQuoteObj.IsFlatRateShipping
        ' ''addressedFlatRateFee = addressedQuoteObj.ShipPrice



        '' ''Set Design Fee Attribute. Store 0 unless Pro was selected.
        ' ''If designType.ToLower() <> "pro" Then
        ' ''    DesignFee = 0
        ' ''End If


        '' ''ADD THE ADDRESSED PRODUCT NODE
        ' ''oCart = (CartUtility.AddProduct(oCart, DateTime.Now.ToString(), BaseProductID, DesignFee, UserDistributionId, addressedFlatRateFee, _
        ' ''                                addressedFlatRateShipQTY, addressedGUID, addressedIsFlatRateShipping, comments, jobName, ProductName, PaperHeight, _
        ' ''                                PaperWidth, addressedPostageFee, addressedPrice, addressedPerPiecePrice, ProductID, addressedTotalMailQty, SiteID, _
        ' ''                                SKU, addressedTaxablePrice, "AddressedList", addressedWeight))





        '' ''9) ADDRESSED PRODUCT ATTRIBUTE NODES 
        '' ''Loop through the attributes of the Product and insert a <Attribute> Node for each one.
        ' ''For Each oOpt As Taradel.PMOptionInfo In eddmQuoteObj.SelectedOptions

        ' ''    Dim dOptMarkup As Double = oOpt.BasePrice
        ' ''    Dim bPercent As Boolean = oOpt.BaseMarkupPercent

        ' ''    '-- This applies the override if one is defined for this option
        ' ''    If oOpt.PriceMatrixOptionMarkup IsNot Nothing Then
        ' ''        dOptMarkup = oOpt.PriceMatrixOptionMarkup.Markup
        ' ''        bPercent = oOpt.PriceMatrixOptionMarkup.MarkupPercent
        ' ''    End If

        ' ''    oCart = (CartUtility.AddAttribute(oCart, "AddressedList", oOpt.OptCatName, oOpt.OptCatId.ToString(), oOpt.OptionId.ToString(), oOpt.OptName, dOptMarkup.ToString(), bPercent.ToString(), oOpt.Weight.Value.ToString()))

        ' ''Next


        '' ''Professional Service Attribute
        ' ''If designType = "pro" Then
        ' ''    oCart = (CartUtility.AddAttribute(oCart, "AddressedList", "Professional Design Services", "0", "1", PaperWidth & "x" & PaperHeight & " (" & DesignFee.ToString("C") & ")", DesignFee.ToString(), "False", "0"))
        ' ''End If


        '' ''Postage Fee Attribute
        ' ''oCart = (CartUtility.AddAttribute(oCart, "AddressedList", "Postage Fee", "0", addressedPostageFee.ToString(), addressedObjCalc.MailPieces.ToString("N0") & " pieces (" & addressedPostageFee.ToString("C") & ")", addressedPostageFee.ToString(), "False", "0"))


        '' ''Number of Drops Attribute
        ' ''oCart = (CartUtility.AddAttribute(oCart, "AddressedList", "Number of Drops", "0", addressedObjCalc.NumOfDrops.ToString(), addressedObjCalc.NumOfDrops & " drop" & IIf(addressedObjCalc.NumOfDrops = 1, "", "s") & _
        ' ''                        " (" & addressedExtraDropFee.ToString("C") & ")", addressedExtraDropFee.ToString(), "False", "0"))

        '' ''Drop Schedule Attribute
        ' ''oCart = (CartUtility.AddAttribute(oCart, "AddressedList", "Drop Schedule", "0", "every " & iFrequency.ToString() & " weeks", iFrequency.ToString(), "0", "False", "0"))




        '' ''10) ADDRESSED ORDER CALC NODE
        ' ''oCart = (CartUtility.AddOrderCalc(oCart, "AddressedList", "0", "0", oCust.State, "", "0", "0", "0", "0", "0"))



        '' ''11) ADDRESSED DROPS NODE
        '' ''Determine if Distribution has Businesses and PO Boxes selected...
        ' ''Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(Me.Distribution.ReferenceId)

        ' ''If oSummary IsNot Nothing Then
        ' ''    addressedUseBusinesses = oSummary.UseBusiness
        ' ''    addressedUsePOBoxes = oSummary.UsePOBox
        ' ''End If

        ' ''oCart = (CartUtility.AddDrops(oCart, "AddressedList", oCust.Company, oCust.Address1, oCust.Address2, oCust.City, oCust.State, oCust.ZipCode, "", "", addressedUseResidential, addressedUseBusinesses, addressedUsePOBoxes))



        '' ''12) ADDRESSED DROP (INDIV) NODES
        ' ''Dim addressedDropBreak As Integer = 0
        ' ''Dim addressedOnePer As Boolean = False
        ' ''Dim addressedDistObj As Taradel.CustomerDistribution = Nothing
        ' ''Dim addressedUSelect As Taradel.USelectProductConfiguration = Nothing


        ' ''addressedDistObj = Taradel.CustomerDistributions.GetDistribution(UserDistributionId)

        ' ''If addressedDistObj IsNot Nothing Then
        ' ''    addressedUSelect = Taradel.Helper.USelect.GetProduct(oDist.USelectMethodReference.ForeignKey, BaseProductID)
        ' ''End If


        '' ''Table is used instead of the previous oSelects obj.  Data is stored in new table (AddressedListSelections).
        ' ''Dim addressedTable As DataTable
        ' ''addressedTable = DistributionUtility.RetrieveAddressedSelections(referenceID, "addressed")


        '' ''If this is a Multiple Impression distribution
        ' ''If addressedObjCalc.IsThisAMultiple Then

        ' ''    'this is a muliple - so divide back out the tempObjCalc.MailPieces for each drop
        ' ''    addressedObjCalc.MailPieces = (addressedObjCalc.MailPieces / addressedObjCalc.NumOfDrops)
        ' ''    addressedDropBreak = addressedObjCalc.MailPieces
        ' ''    addressedOnePer = False
        ' ''    'addressedPostageFee = addressedPostageFee + (addressedObjCalc.MailPieces * addressedUSelect.MailPricePerPiece)    <--- may need to revisit this. 6/2/2015


        ' ''    For dropNumber As Integer = 1 To addressedObjCalc.NumOfDrops

        ' ''        Dim j As Integer = 0
        ' ''        Dim dDropDate As DateTime = earliestDropDate.AddDays((dropNumber - 1) * daysToAdd)

        ' ''        'Find Friday
        ' ''        While (dDropDate.DayOfWeek <> DayOfWeek.Friday)
        ' ''            dDropDate = dDropDate.AddDays(-1)
        ' ''        End While


        ' ''        'Add the Drop Nodes
        ' ''        oCart = CartUtility.AddIndividualDrop(oCart, "AddressedList", dropNumber, dDropDate.ToShortDateString(), True, addressedObjCalc.NumOfDrops)


        ' ''        'Count the matches from each Area Node
        ' ''        For Each row As DataRow In addressedTable.Rows
        ' ''            addressedTotalDropSelections = addressedTotalDropSelections + row("Matches")
        ' ''        Next row


        ' ''        'Currently using place holders for ListKeyID and File Name.
        ' ''        oCart = CartUtility.AddListData(oCart, "AddressedList", dropNumber, "12345678-" & dropNumber, "x:\folder\folder\file" & dropNumber & ".xyz")


        ' ''        'Update total with new value
        ' ''        oCart = CartUtility.UpdateDropCount(oCart, "AddressedList", dropNumber, addressedTotalDropSelections)

        ' ''        'reset
        ' ''        addressedTotalDropSelections = 0

        ' ''    Next

        ' ''    'Clean up
        ' ''    addressedTable.Dispose()


        ' ''    'Single Impression
        ' ''Else

        ' ''    For dropNumber As Integer = 1 To addressedObjCalc.NumOfDrops

        ' ''        Dim dDropDate As DateTime = earliestDropDate.AddDays((dropNumber - 1) * daysToAdd)

        ' ''        'eddmPostageFee = eddmPostageFee + (EDDMSelected * oUSelect.MailPricePerPiece)    <--- may need to revisit this. 6/2/2015
        ' ''        dDropBreak = (addressedObjCalc.MailPieces / addressedObjCalc.NumOfDrops)

        ' ''        'Possible obsolete here.
        ' ''        If numDrops = addressedTable.Rows.Count And addressedObjCalc.NumOfDrops > 1 Then
        ' ''            bOnePer = True
        ' ''        End If

        ' ''        'Find the Friday
        ' ''        While (dDropDate.DayOfWeek <> DayOfWeek.Friday)
        ' ''            dDropDate = dDropDate.AddDays(-1)
        ' ''        End While


        ' ''        'Add the single Drop
        ' ''        oCart = CartUtility.AddIndividualDrop(oCart, "AddressedList", dropNumber, dDropDate.ToShortDateString(), False, "0")


        ' ''        'Add Areas
        ' ''        For Each row As DataRow In addressedTable.Rows
        ' ''            oCart = CartUtility.AddIndividualArea(oCart, "AddressedList", dropNumber, row("Name"), row("FriendlyName"), row("Matches"))
        ' ''            addressedTotalDropSelections = addressedTotalDropSelections + row("Matches")
        ' ''        Next row


        ' ''        'Update total with new value
        ' ''        oCart = CartUtility.UpdateDropCount(oCart, "AddressedList", dropNumber, addressedTotalDropSelections)

        ' ''    Next

        ' ''    'Clean up
        ' ''    addressedTable.Dispose()

        ' ''End If



        '' ''13) ADDRESSED DESIGN NODE
        '' ''NOTE - All of the variables and the process of file being saved is done in the EDDM portion (Step 6).  It didn't many any sense to 
        '' '' recreate the logic unless it needs to be separated.

        '' ''Add Design Node
        ' ''oCart = (CartUtility.AddDesign(oCart, "AddressedList", designType, frontFileExt, frontFileName, frontRealFileName, frontAction, hasBackDesign, _
        ' ''backFileExt, backFileName, backRealFileName, backAction, artKey, requiresProof.ToString().ToUpper()))




        '' ''14) ADDRESSED SHIPMENTS NODE
        ' ''If UserDistributionId > 0 Then

        ' ''    addressedWeightPerPage = ((addressedWeight * addressedPageCount) / AddressedSelected)


        ' ''    If addressedExtraCopies > 0 Then

        ' ''        If deliveryAddressID = "0" Then

        ' ''            '-- Create a new address and get the ID
        ' ''            Dim sMsg As String = ""
        ' ''            Dim iAddr As Integer = Taradel.CustomerAddressDataSource.NewAddress(oCust.CustomerID, "", "", "", "", DeliveryAddress.Text, DeliveryAddress2.Text, DeliveryCity.Text, USAStatesDropDown.SelectedValue, ZipCode.Text, sMsg)

        ' ''            If iAddr > 0 Then
        ' ''                deliveryAddressID = iAddr.ToString()
        ' ''            End If

        ' ''        End If


        ' ''    End If

        ' ''    oCart = (CartUtility.AddShipments(oCart, "AddressedList", deliveryAddressID, addressedExtraCopies, addressedWeight, addressedPageCount, PaperWidth, PaperHeight, addressedFlatRateFee.ToString("N2"), zip, addressedTotalMailQty))


        ' ''End If



        '' ''15) DEMOGRAPHICS
        '' ''Add Demographics to XML/Cart.  5 Filters so far. There should only be ONE row in this table.
        '' ''Currently, this is only created and inserted when AddressList types are selected.  Will insert
        '' ''and create a primary node like Shipments, Design, etc.

        ' ''Dim addressedFiltersTable As DataTable
        ' ''addressedFiltersTable = DistributionUtility.RetrieveDemographicFilters(referenceID)

        ' ''For Each row As DataRow In addressedFiltersTable.Rows
        ' ''    oCart = (CartUtility.AddDemographics(oCart, "AddressedList", UserDistributionId, row("Incomes"), row("Ages"), row("HomeOwner"), row("Gender"), row("Children")))
        ' ''Next

        '' ''Clean up
        ' ''addressedTable.Dispose()




        '' ''16) TMC Data Node
        ' ''oCart = (CartUtility.AddTmcData(oCart, EDDMSelected, eddmTotalMailQty, eddmBaseProductID, eddmPricePerPiece, AddressedSelected, addressedTotalMailQty, BaseProductID, addressedPerPiecePrice))



        ' ''Profile.Cart = oXML
        ' ''Profile.Save()


        ' ''If (debug) Then

        ' ''    methodVars.Append("jobName: " & jobName & "<br />")
        ' ''    methodVars.Append("comments: " & comments & "<br />")
        ' ''    methodVars.Append("sCustomField1: " & sCustomField1 & "<br />")
        ' ''    methodVars.Append("sCustomField2: " & sCustomField2 & "<br />")
        ' ''    methodVars.Append("sCustomField3: " & sCustomField3 & "<br />")
        ' ''    methodVars.Append("designType: " & designType & "<br />")
        ' ''    methodVars.Append("deliveryAddressID: " & deliveryAddressID & "<br />")
        ' ''    methodVars.Append("zip: " & zip & "<br />")
        ' ''    methodVars.Append("earliestDropDate: " & earliestDropDate & "<br />")
        ' ''    methodVars.Append("mailAllPiecesAtOnce: " & mailAllPiecesAtOnce & "<br />")
        ' ''    methodVars.Append("holdQTY: " & holdQTY & "<br />")
        ' ''    methodVars.Append("numDrops: " & numDrops & "<br />")
        ' ''    methodVars.Append("numImpressions: " & numImpressions & "<br />")
        ' ''    methodVars.Append("sendThisNumberOfDrops: " & sendThisNumberOfDrops & "<br />")
        ' ''    methodVars.Append("iFrequency: " & iFrequency & "<br />")
        ' ''    methodVars.Append("daysToAdd: " & daysToAdd & "<br />")
        ' ''    methodVars.Append("postageRate: " & postageRate & "<br />")
        ' ''    methodVars.Append("eddmGUID: " & eddmGUID & "<br />")
        ' ''    methodVars.Append("eddmProductID: " & eddmProductID & "<br />")
        ' ''    methodVars.Append("eddmBaseProductID: " & eddmBaseProductID & "<br />")
        ' ''    methodVars.Append("eddmPrice: " & eddmPrice & "<br />")
        ' ''    methodVars.Append("eddmPostageFee: " & eddmPostageFee & "<br />")
        ' ''    methodVars.Append("eddmDesignFee: " & eddmDesignFee & "<br />")
        ' ''    methodVars.Append("eddmPricePerPiece: " & eddmPricePerPiece & "<br />")
        ' ''    methodVars.Append("eddmWeight: " & eddmWeight & "<br />")
        ' ''    methodVars.Append("eddmTaxablePrice: " & eddmTaxablePrice & "<br />")
        ' ''    methodVars.Append("eddmIsFlatRateShipping: " & eddmIsFlatRateShipping & "<br />")
        ' ''    methodVars.Append("eddmFlatRateFee: " & eddmFlatRateFee & "<br />")
        ' ''    methodVars.Append("eddmFlatRateShipQTY: " & eddmFlatRateShipQTY & "<br />")
        ' ''    methodVars.Append("eddmExtraCopies: " & eddmExtraCopies & "<br />")
        ' ''    methodVars.Append("eddmUseBusinesses: " & eddmUseBusinesses & "<br />")
        ' ''    methodVars.Append("eddmUsePOBoxes: " & eddmUsePOBoxes & "<br />")
        ' ''    methodVars.Append("eddmUseResidential: " & eddmUseResidential & "<br />")
        ' ''    methodVars.Append("eddmProductName: " & eddmProductName & "<br />")
        ' ''    methodVars.Append("eddmSKU: " & eddmSKU & "<br />")
        ' ''    methodVars.Append("eddmPaperHeight: " & eddmPaperHeight & "<br />")
        ' ''    methodVars.Append("eddmPaperWidth: " & eddmPaperWidth & "<br />")
        ' ''    methodVars.Append("eddmPageCount: " & eddmPageCount & "<br />")
        ' ''    methodVars.Append("eddmMarkUp: " & eddmMarkUp & "<br />")
        ' ''    methodVars.Append("eddmMarkUpType: " & eddmMarkUpType & "<br />")
        ' ''    methodVars.Append("eddmExtraDropFee: " & eddmExtraDropFee & "<br />")
        ' ''    methodVars.Append("eddmWeightPerPage: " & eddmWeightPerPage & "<br />")
        ' ''    methodVars.Append("eddmTotalMailQty: " & eddmTotalMailQty & "<br />")
        ' ''    methodVars.Append("eddmPrintQTY: " & eddmPrintQTY & "<br />")
        ' ''    methodVars.Append("eddmTotalDropSelections: " & eddmTotalDropSelections & "<br />")
        ' ''    methodVars.Append("eddmDropPrice: " & eddmDropPrice & "<br />")
        ' ''    methodVars.Append("addressedGUID: " & addressedGUID & "<br />")
        ' ''    methodVars.Append("addressedPrintQTY: " & addressedPrintQTY & "<br />")
        ' ''    methodVars.Append("addressedExtraCopies: " & addressedExtraCopies & "<br />")
        ' ''    methodVars.Append("addressedPerPiecePrice: " & addressedPerPiecePrice & "<br />")
        ' ''    methodVars.Append("addressedFlatRateFee: " & addressedFlatRateFee & "<br />")
        ' ''    methodVars.Append("addressedFlatRateShipQTY: " & addressedFlatRateShipQTY & "<br />")
        ' ''    methodVars.Append("addressedIsFlatRateShipping: " & addressedIsFlatRateShipping & "<br />")
        ' ''    methodVars.Append("addressedPostageFee: " & addressedPostageFee & "<br />")
        ' ''    methodVars.Append("addressedTotalMailQty: " & addressedTotalMailQty & "<br />")
        ' ''    methodVars.Append("addressedPrice: " & addressedPrice & "<br />")
        ' ''    methodVars.Append("addressedTaxablePrice: " & addressedTaxablePrice & "<br />")
        ' ''    methodVars.Append("addressedExtraDropFee: " & addressedExtraDropFee & "<br />")
        ' ''    methodVars.Append("addressedDropPrice: " & addressedDropPrice & "<br />")
        ' ''    methodVars.Append("addressedUsePOBoxes: " & addressedUsePOBoxes & "<br />")
        ' ''    methodVars.Append("addressedUseBusinesses: " & addressedUseBusinesses & "<br />")
        ' ''    methodVars.Append("addressedUseResidential: " & addressedUseResidential & "<br />")
        ' ''    methodVars.Append("addressedTotalDropSelections: " & addressedTotalDropSelections & "<br />")
        ' ''    methodVars.Append("addressedWeightPerPage: " & addressedWeightPerPage & "<br />")
        ' ''    methodVars.Append("addressedWeight: " & addressedWeight & "<br />")
        ' ''    methodVars.Append("addressedPageCount: " & addressedPageCount & "<br />")

        ' ''    methodVars.Append("<hr />")

        ' ''    If options Is Nothing Then
        ' ''        methodVars.Append("<p>options SortedList is null</p>")
        ' ''    Else
        ' ''        If options.Count <= 0 Then
        ' ''            methodVars.Append("<p>options SortedList has no items</p>")
        ' ''        Else
        ' ''            'what exactly is in options?
        ' ''            For Each k As KeyValuePair(Of Integer, Integer) In options
        ' ''                methodVars.Append("<p>k.Key:" + k.Key.ToString() + " k.Value.ToString():" + k.Value.ToString() & "<p/>")
        ' ''            Next k

        ' ''        End If
        ' ''    End If

        ' ''    methodVars.Append("<hr />")

        ' ''    litDebugMethodVars.Text = methodVars.ToString()

        ' ''End If

    End Sub



    Private Sub FixShippingAddress(shippingAddressID As Integer)

        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim errorMsg As New StringBuilder()
        Dim updateSql As String = "UPDATE [pnd_CustomerShippingAddress] SET [Deleted] = 0 OUTPUT INSERTED.[ShippingAddressID] WHERE [shippingAddressID] = " & shippingAddressID
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(updateSql, connObj)

        Try

            connObj.Open()
            sqlCommand.ExecuteNonQuery()

        Catch objException As Exception

            errorMsg.Append("The following errors occurred:<br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " & objException.Message & "</li>")
            errorMsg.Append("<li>Source: " & objException.Source & "<li>")
            errorMsg.Append("<li>Stack Trace: " & objException.StackTrace & "<li>")
            errorMsg.Append("<li>Target Site: " & objException.TargetSite.Name & "<li>")
            errorMsg.Append("<li>SQL: " & updateSql & "<li>")
            errorMsg.Append("</ul>")

            EmailUtility.SendAdminEmail("Error in FixShippingAddress. (Step2-ProductOptions.aspx). Details:<br />" & errorMsg.ToString())


        Finally

            connObj.Close()

        End Try


    End Sub










    'Debug ===============================================================================================
    Private Sub ShowDebug()


        pnlDebug.CssClass = String.Empty
        pnlDebug.CssClass = "alert alert-danger"

        'Show Page Properties
        litDebugMinQty.Text = MinimumQuantity
        litDebugMaxQty.Text = MaximumQuantity
        litDebugEDDMSelected.Text = EDDMSelected
        litDebugAddressedSelected.Text = AddressedSelected
        litDebugTotalSelected.Text = TotalSelected
        litDebugProductID.Text = ProductID
        litDebugbaseProductID.Text = BaseProductID
        litDebugSKU.Text = SKU
        litDebugPaperWidth.Text = PaperWidth
        litDebugPaperHeight.Text = PaperHeight
        litDebugPageCount.Text = PageCount
        litDebugDesignFee.Text = DesignFee
        litDebugDistID.Text = UserDistributionId
        litDebugUSelectID.Text = USelectID
        litDebugReferenceID.Text = referenceID
        litDebugProductName.Text = ProductName
        litDebugMarkUp.Text = MarkUp
        litDebugMarkUpType.Text = MarkUpType
        litDebugMaxDrops.Text = maxDrops
        litDebugEDDM.Text = EDDMMap
        litDebugAddressed.Text = GeneratedAddressedList
        litDebugUploadedList.Text = UploadedAddressedList
        litDisableTemplates.Text = DisableTemplates
        litDisableProDesign.Text = DisableProDesign
        litAllowMultipleImpressions.Text = AllowMultipleImpressions
        litAllowSplitDrops.Text = AllowSplitDrops
        litAllowUpsell.Text = bMarketingUpsell.ToString()
        litTestMode.Text = TestMode
        litCampaignOverviewDisplayDelay.Text = CampaignOverviewDisplayDelay
        litDebugMinOrderQty.Text = MinOrderQty
        litDebugMinEDDMPricingQty.Text = MinEDDMPricingQty
        litDebugMinAddressedPricingQty.Text = MinAddressedPricingQty
        litDebugPostageRate.Text = PostageRate
        litValidateExtraCopiesAddress.Text = ValidateExtraCopiesAddress
        litDropFeeRate.Text = DropFeeRate.ToString()


        'EXPERIMENTAL CODE TO GET PRODUCTS - serves no purpose
        '1) Use Linq to filter our unwanted categories.
        'From Step1-TargetReview.aspx logic...
        Using oDb As New Taradel.taradelEntities

            Dim productTypes As String = ""

            Select Case USelectID
                Case 1
                    'EDDM
                    productTypes = "eddm"
                Case 5
                    productTypes = "uploaded"
                Case 6
                    'User Generated List
                    productTypes = "created"

                    'Case 7
                    'TMC/Blended List
                    'productTypes = "mailing list"
            End Select


            'From Step1-TargetReview.  GET CategoryID.
            Dim oCat As Taradel.WLCategory = (From c In oDb.WLCategories Where c.Name.ToLower.Contains(productTypes) And c.appxSite.SiteId = SiteID).FirstOrDefault

            If oCat IsNot Nothing Then
                CategoryID = oCat.CategoryID
                litDebugCategoryID.Text = CategoryID
            End If




            '2) Get the list of Products (categories) matching the CategoryID. This is a list of ProductCategoryIDs and a SortOrder.
            'From ProductListWithQuote logic....
            Dim oProductsWithinCategory As List(Of Taradel.WLProductCategory) = Taradel.WLCategoryDataSource.GetProductsInCategory(CategoryID)
            gvProductsInCategory.DataSource = oProductsWithinCategory
            gvProductsInCategory.DataBind()



            '3) Create a new empty Product List to be populated.
            Dim oNewProdList As New List(Of Taradel.WLProduct)

            'Create a new empty prod obj.  Will be defined w/in loop
            Dim tempProd As Taradel.WLProduct = Nothing

            'Loop through valid oProds list (categories) and generate a new list of Products to simulate what happens in 
            'in the control (ProductListWithQuote) on listview's databinding.
            For Each prod In oProductsWithinCategory
                tempProd = prod.WLProduct
                oNewProdList.Add(tempProd)
            Next


            '4) Bind new list to DDL.  From this page.  Create custom attributes.
            For Each product As Taradel.WLProduct In oNewProdList

                Dim li As New ListItem()

                li.Text = product.Name
                li.Value = product.ProductID
                li.Attributes.Add("baseprodid", product.BaseProductID)

                TemplateSizeID = Taradel.WLUtil.GetTemplateSize(product.ProductID)
                li.Attributes.Add("templatesize", TemplateSizeID)

                ddlTestProducts.Items.Add(li)
            Next

            ddlTestProducts.DataBind()



        End Using
        'END EXPERIMENTAL



        'Show original products assigned to site/category
        Dim oOrigProds As New List(Of Taradel.WLProduct)
        oOrigProds = Taradel.WLProductDataSource.GetProducts()
        gvOrigProducts.DataSource = oOrigProds
        gvOrigProducts.DataBind()




        'Show Product Options
        Dim oOptCats As List(Of Taradel.ProductOptionCategory) = Taradel.ProductDataSource.GetProductOptionCategories(BaseProductID)
        Dim prodOptCatsString As New StringBuilder()
        gvProdOptions.DataSource = oOptCats
        gvProdOptions.DataBind()

        If oOptCats.Count > 0 Then
            For Each opt In oOptCats
                prodOptCatsString.Append("opt.Description: " & opt.Description & "<br />")
                prodOptCatsString.Append("opt.Name: " & opt.Name & "<br />")
                prodOptCatsString.Append("opt.OptCatID: " & opt.OptCatID & "<br />")
            Next
        Else
            prodOptCatsString.Append("No Prod Options detected.  Error!")
        End If

        lblProdOptions.Text = prodOptCatsString.ToString()


    End Sub








End Class
