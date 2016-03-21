Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.SqlClient
Imports System.Data

Partial Class CLibrary_ProductListWithQuote


    '=======================================================================================================================================
    'Notes:
    '=======================================================================================================================================

    Inherits CLibraryBase


    'Fields
    Protected Markup As Decimal = 0
    Protected MarkupType As String = ""
    Protected RefProd As Taradel.WLProduct
    Protected RefQuote As Taradel.ProductPriceQuote
    Protected RefProdId As Integer
    Protected debug As Boolean = False




    'Properties
    Private _CategoryId As Integer = 0
    Public Property CategoryId As Integer
        Get
            Return _CategoryId
        End Get
        Set(value As Integer)
            _CategoryId = value
        End Set
    End Property


    Private _Quantity As Integer = 0
    Public Property Quantity As Integer
        Get
            Return _Quantity
        End Get
        Set(value As Integer)
            _Quantity = value
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


    Private _USelectID As Integer = 0
    Public Property USelectID As Integer
        Get
            Return _USelectID
        End Get
        Set(value As Integer)
            _USelectID = value
        End Set
    End Property


    Private _IncludeDesign As Boolean = False
    Public Property IncludeDesign As Boolean
        Get
            Return _IncludeDesign
        End Get
        Set(value As Boolean)
            _IncludeDesign = value
        End Set
    End Property


    Private _NavigateUrl As String = ""
    Public Property NavigateUrl As String
        Get
            Return _NavigateUrl
        End Get
        Set(value As String)
            _NavigateUrl = value
        End Set
    End Property


    Protected ReadOnly Property SiteID As Integer

        Get
            Dim _siteID As Integer = appxCMS.Util.CMSSettings.GetSiteId

            If _siteID = 0 Then
                _siteID = 1
            End If

            Return _siteID

        End Get

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


    Private _AddressedMap As Boolean = False
    Public Property AddressedMap As Boolean
        Get
            Return _AddressedMap
        End Get
        Set(value As Boolean)
            _AddressedMap = value
        End Set

    End Property


    Private _TemplateID As Integer
    Public Property TemplateID() As Integer
        Get
            Return _TemplateID
        End Get
        Set(ByVal value As Integer)
            _TemplateID = value
        End Set
    End Property


    Private _TemplateSizeID As Integer
    Public Property TemplateSizeID() As Integer
        Get
            Return _TemplateSizeID
        End Get
        Set(ByVal value As Integer)
            _TemplateSizeID = value
        End Set
    End Property


    Public Sub SetUpTemplateFromCookie()
        Me.TemplateID = 0
        Me.TemplateSizeID = 0

        If Request.Cookies("SelectedTemplate") IsNot Nothing Then
            Integer.TryParse(Request.Cookies("SelectedTemplate").Value, Me.TemplateID)
        End If

        Dim oTemplate As TemplateCode.Template1 = Nothing

        Using oAPI As New TemplateCode.TemplateAPIClient
            Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, Me.TemplateID)
            Dim oResponse As TemplateCode.GetTemplateResponse = oAPI.GetTemplate(oRequest)
            oTemplate = oResponse.GetTemplateResult
        End Using

        If oTemplate IsNot Nothing Then
            'TemplateID = oTemplate.TemplateId
            Me.TemplateSizeID = oTemplate.TemplateSizeId
        End If
        'If Request.Cookies("SelectedTemplateNew") IsNot Nothing Then
        '    Integer.TryParse(Request.Cookies("SelectedTemplateNew").Value, templateId)
        'End If

        'Response.Write("SetUpTemplateFromCookie  Me.TemplateSizeID:" & Me.TemplateSizeID.ToString() & " Me.TemplateID:" & Me.TemplateID.ToString())

        'Return iTemplateId
    End Sub






    'Methods
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        BuildControl()
        SetUpTemplateFromCookie()


    End Sub




    Protected Overrides Sub BuildControl()

        Markup = appxCMS.Util.CMSSettings.GetDecimal("Catalog", "Markup")
        MarkupType = appxCMS.Util.CMSSettings.GetSetting("Catalog", "MarkupType")

        If MarkupType.ToLower = "percent" Then
            If Markup > 1 Then
                Markup = Markup / 100
            End If
        End If

        If Me.DistributionId > 0 Then

            Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistributions.GetDistribution(Me.DistributionId)

            If oDist IsNot Nothing Then

                Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Taradel.CustomerDistributions.GetSelectionSummary(oDist.ReferenceId)
                Dim oSelects As List(Of Taradel.MapServer.UserData.AreaSelection) = Taradel.CustomerDistributions.GetSelections(oDist.ReferenceId)
                Dim iResTotal As Integer = 0
                Dim iBizTotal As Integer = 0
                Dim iBoxTotal As Integer = 0
                Dim iAreaCount As Integer = 0
                Dim bBiz As Boolean = True
                Dim bBox As Boolean = True

                If oSummary IsNot Nothing Then

                    bBiz = oSummary.UseBusiness
                    bBox = oSummary.UsePOBox

                    For Each oArea As Taradel.MapServer.UserData.AreaSelection In oSelects

                        iResTotal = iResTotal + oArea.Residential

                        If bBiz Then
                            iBizTotal = iBizTotal + oArea.Business
                        End If

                        If bBox Then
                            iBoxTotal = iBoxTotal + oArea.POBoxes
                        End If

                        iAreaCount = iAreaCount + 1

                    Next

                    Me.Quantity = iResTotal + iBizTotal + iBoxTotal

                End If

            End If

        End If

        Dim oProds As List(Of Taradel.WLProductCategory) = Taradel.WLCategoryDataSource.GetProductsInCategory(Me.CategoryId)

        '-- 20151005 MRM: This logic will filter the master list of available products down to only sizes supported
        ' by their template configuration, if they do NOT have File Upload and Pro Design Enabled (and templates are enabled)
        Dim oTemplateUtil As New TemplateUtility
        If oTemplateUtil.RestrictToTemplates Then
            '-- Which template sizes do they have available
            Dim oTemplateProds = New List(Of Taradel.WLProductCategory)
            Dim oSizes = oTemplateUtil.AvailableSizes()
            For Each oSize In oSizes
                Dim oTempProd = Taradel.WLUtil.GetProductBySize(oSize)
                If oTempProd IsNot Nothing Then
                    Dim oFound = oProds.FirstOrDefault(Function(pc) pc.WLProduct.ProductID = oTempProd.ProductID)
                    If oFound IsNot Nothing Then
                        oTemplateProds.Add(oFound)
                    End If
                End If
            Next
            oProds = oTemplateProds
        End If


        Dim bHasTemplate As Boolean = False
        Dim oTemplate As TemplateCode.Template1 = Nothing
        Dim oTemplateProd As Taradel.WLProduct = Nothing

        If Request.Cookies("SelectedTemplate") IsNot Nothing Then
            Dim TemplateId As Integer = 0
            Integer.TryParse(Request.Cookies("SelectedTemplate").Value, TemplateId)
            'Response.Write("TemplateID: " & TemplateId.ToString())
            Using oAPI As New TemplateCode.TemplateAPIClient
                Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, TemplateId)
                Dim oResponse As TemplateCode.GetTemplateResponse = oAPI.GetTemplate(oRequest)
                oTemplate = oResponse.GetTemplateResult
                If oTemplate IsNot Nothing Then
                    bHasTemplate = True
                End If
            End Using

            If bHasTemplate Then
                oTemplateProd = Taradel.WLUtil.GetProductBySize(oTemplate.TemplateSizeId)
                If oTemplateProd Is Nothing Then
                    bHasTemplate = False
                End If
            End If

        End If


        If bHasTemplate Then
            RefProd = oTemplateProd
            RefProdId = oTemplateProd.BaseProductID

            '-- Remove the template matched product from the list
            Dim oWLProdCat As Taradel.WLProductCategory = oProds.FirstOrDefault(Function(wp As Taradel.WLProductCategory) wp.WLProduct.ProductID = oTemplateProd.ProductID)

            If oWLProdCat IsNot Nothing Then

                oProds.Remove(oWLProdCat)

                SetupProduct(litQuote, hplOrder, hplImageOrder, hplTitleOrder, prodImage, oWLProdCat.WLProduct, Nothing)

                pTemplate.Visible = True

                lTemplateProdName.Text = oWLProdCat.WLProduct.Name

                Dim sTemplateServerHost As String = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
                If Not String.IsNullOrEmpty(oTemplate.FrontImage) Then
                    imgTemplateFront.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.FrontImage
                Else
                    imgTemplateFront.Visible = False
                End If

                If Not String.IsNullOrEmpty(oTemplate.InsideImage) Then
                    imgTemplateInside.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.InsideImage
                Else
                    imgTemplateInside.Visible = False
                End If

                If Not String.IsNullOrEmpty(oTemplate.BackImage) Then
                    imgTemplateBack.ImageUrl = sTemplateServerHost & "/templates/icon/" & oTemplate.BackImage
                Else
                    imgTemplateBack.Visible = False
                End If

            Else

                'let's not dump the cookie just yet --> rs 10/29/2015 -- Response.Cookies("SelectedTemplate").Value = ""

                Dim oFirstProd As Taradel.WLProduct = oProds.FirstOrDefault.WLProduct
                If oFirstProd IsNot Nothing Then
                    RefProd = oFirstProd
                    RefProdId = oFirstProd.BaseProductID
                End If

            End If

        Else

            Dim oFirstProd As Taradel.WLProduct = oProds.FirstOrDefault.WLProduct

            If oFirstProd IsNot Nothing Then
                RefProd = oFirstProd
                RefProdId = oFirstProd.BaseProductID
            End If

        End If

        If RefProdId > 0 Then

            '-- Get the next range for this quantity
            Dim oProdRange As Taradel.PriceMatrix = Taradel.ProductDataSource.GetNextPriceRange(RefProdId, Me.Quantity)
            If oProdRange IsNot Nothing Then
                '-- Build a quote 
                Dim refMarkup As Decimal = RefProd.Markup
                Dim refMarkupType As String = RefProd.MarkupType
                If refMarkup > 0 AndAlso refMarkupType.ToLower = "percent" Then
                    If refMarkup > 1 Then
                        refMarkup = refMarkup / 100
                    End If
                End If

                'RefQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, RefProdId, oProdRange.MinQty, 0, IIf(Me.DistributionId > 0, 1, 0), Nothing, "", Me.IncludeDesign, False, refMarkup, refMarkupType)

            End If
        End If

        'probably a better way to do this - 10/30/2015
        'Dim lstNew As New List(Of NewProductCategory)
        'For Each prod As Taradel.WLProductCategory In oProds
        '    Dim newProd As New NewProductCategory()
        '    'newProd = DirectCast(prod, NewProductCategory)
        '    newProd.ProductCategoryID = prod.ProductCategoryID
        '    newProd.SelectedSize = prod.s

        '    lstNew.Add(newProd)
        'Next
        SetUpTemplateFromCookie()

        For Each xProdCategory As Taradel.WLProductCategory In oProds
            Dim iTemplateSizeID As Integer = RetrieveProductTemplateSizeID(xProdCategory.WLProduct.BaseProductID)
            If Me.TemplateSizeID > 0 Then
                If (iTemplateSizeID = Me.TemplateSizeID) Then
                    xProdCategory.SortOrder = 0
                End If
            End If
            'Response.Write(xProdCategory.SortOrder.ToString() & "|")
        Next
        'Response.Write("iTemplateSizeID:" & iTemplateSizeID.ToString())
        'Response.Write("oProd.BaseProductID:" & oProd.BaseProductID.ToString())


        Dim oProdSort = From oProd In oProds Order By oProd.SortOrder


        lvProducts.DataSource = oProdSort
        lvProducts.DataBind()

        ShowDebug(debug)


    End Sub




    Public Class NewProductCategory
        Inherits Taradel.WLProductCategory

        Private _SelectedSize As Integer
        Public Property SelectedSize() As Integer
            Get
                Return _SelectedSize
            End Get
            Set(ByVal value As Integer)
                _SelectedSize = value
            End Set
        End Property


    End Class



    Protected Sub lvProducts_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvProducts.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim oDItem As ListViewDataItem = e.Item

            Dim oProdCat As Taradel.WLProductCategory = DirectCast(oDItem.DataItem, Taradel.WLProductCategory)
            Dim oProd As Taradel.WLProduct = oProdCat.WLProduct


            If oProd IsNot Nothing Then

                Dim litQuote As Literal = DirectCast(e.Item.FindControl("litQuote"), Literal)
                Dim hplOrder As HyperLink = DirectCast(e.Item.FindControl("hplOrder"), HyperLink)
                Dim hplImageOrder As HyperLink = DirectCast(e.Item.FindControl("hplImageOrder"), HyperLink)
                Dim hplTitleOrder As HyperLink = DirectCast(e.Item.FindControl("hplTitleOrder"), HyperLink)
                Dim prodImage As Image = DirectCast(e.Item.FindControl("prodImage"), Image)
                Dim pnlPriceBreaksButton As Panel = DirectCast(e.Item.FindControl("pnlPriceBreaksButton"), Panel)
                Dim btnAboutProduct As LinkButton = DirectCast(e.Item.FindControl("btnAboutProduct"), LinkButton)


                'Set addition "Help' button attributes for Products.
                btnAboutProduct.Attributes.Add("data-title", oProd.Name)
                btnAboutProduct.Attributes.Add("data-helpfile", "/helpProduct" & oProd.BaseProductID)

                SetupProduct(litQuote, hplOrder, hplImageOrder, hplTitleOrder, prodImage, oProd, e.Item)

            Else
                e.Item.Visible = False
            End If

        End If

    End Sub


    Protected Sub SetupProduct(litQuote As Literal, hplOrder As HyperLink, hplImageOrder As HyperLink, hplTitleOrder As HyperLink, prodImage As Image, oProd As Taradel.WLProduct, oItem As ListViewItem)
        If (Me.TemplateID = 0) Then
            SetUpTemplateFromCookie()
        End If
        Dim quoteText As New StringBuilder


        'Display custom quotes/text/controls
        If EDDMMap = True Then

            Dim oQuote As New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProd.BaseProductID, Me.Quantity, Me.DistributionId, IIf(Me.DistributionId > 0, 1, 0), Nothing, "", Me.IncludeDesign, False, oProd.Markup, oProd.MarkupType)
            Dim dPrice As Decimal = oQuote.Price
            Dim sWidthDesign As String = ""

            If Me.IncludeDesign Then
                sWidthDesign = " (includes professional design)"
            End If

            quoteText.Append("You selected " & Me.Quantity.ToString("N0") & " addresses. Reach them all in a single drop for " & dPrice.ToString("C") & sWidthDesign & ". That's only " & oQuote.FormattedPricePerPiece & " per address.")

            'The upsell panel and data is only visible for EDDM distributions.
            If oProd.BaseProductID = RefProdId Then

                If RefQuote IsNot Nothing Then

                    '-- Test this to see if they will save money at the next level
                    If oQuote.Price > RefQuote.Price Then
                        pSavings.Visible = True
                        lSavingsProductName.Text = oProd.Name
                        lSavingsNextRange.Text = RefQuote.Quantity.ToString("N0")
                        lSavingsAddressExtra.Text = (RefQuote.Quantity - oQuote.Quantity).ToString("N0")
                        lSavingsNewQuoteTotal.Text = RefQuote.Price.ToString("C")
                        lSavingsAmount.Text = (oQuote.Price - RefQuote.Price).ToString("C")
                        lSavingsOldPricePerPiece.Text = oQuote.FormattedPricePerPiece
                        lSavingsNewPricePerPiece.Text = RefQuote.FormattedPricePerPiece
                    End If

                End If

            End If


        End If

        If AddressedMap = True Then
            Dim oQuote As New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProd.BaseProductID, Me.Quantity, Me.DistributionId, IIf(Me.DistributionId > 0, 1, 0), Nothing, "", Me.IncludeDesign, False, oProd.Markup, oProd.MarkupType)
            Dim dPrice As Decimal = oQuote.Price
            Dim sWidthDesign As String = ""

            If Me.IncludeDesign Then
                sWidthDesign = " (includes professional design)"
            End If

            quoteText.Append(sWidthDesign)


            quoteText.Append("You selected " & Me.Quantity.ToString("N0") & " addresses. Reach them all in a single drop for " & dPrice.ToString("C") & sWidthDesign & ". That's only " & oQuote.FormattedPricePerPiece & " per address.") '& " Cookie Value: " & Me.TemplateID.ToString())

            If (debug) Then
                quoteText.Append("<div class=""alert alert-danger"">")
                quoteText.Append("<strong>**DEBUG DATA**</strong><br />")
                quoteText.Append("Addressed Price: " & oQuote.Price & "<br />")
                quoteText.Append("Addressed Base Price: " & oQuote.BasePrice & "<br />")
                quoteText.Append("Addressed PPP: " & oQuote.PricePerPiece & "<br />")
                quoteText.Append("Addressed QTY: " & oQuote.Quantity & "<br />")
                quoteText.Append("BaseProductID: " & oProd.BaseProductID & "<br />")
                quoteText.Append("ProductID: " & oProd.ProductID & "<br /><br />")
                quoteText.Append("</div>")
            End If

        End If

        'does the template size id match the product size id?
        'if so, change the format to be something slightly different
        'RetrieveTemplateSizeID for product id
        Dim useCookie As Boolean = False

        Dim iTemplateSizeID As Integer = RetrieveProductTemplateSizeID(oProd.BaseProductID)
        'Response.Write("iTemplateSizeID:" & iTemplateSizeID.ToString())
        'Response.Write("oProd.BaseProductID:" & oProd.BaseProductID.ToString())

        If Me.TemplateSizeID > 0 Then
            If (iTemplateSizeID = Me.TemplateSizeID) Then
                useCookie = True
                quoteText.AppendLine("<br /><strong>(Previously selected template size)</strong>")
                litQuote.Text = quoteText.ToString() 'COOKIE!!!" & "|" ' & litQuote.Parent.ID.ToString() & "|"
                Dim pnlQuote As HtmlGenericControl = Nothing

                If oItem IsNot Nothing Then
                    pnlQuote = DirectCast(oItem.FindControl("pnlQuote"), HtmlGenericControl)
                End If

                If pnlQuote IsNot Nothing Then
                    pnlQuote.Attributes.Remove("class")
                    pnlQuote.Attributes.Add("class", "panel panel-success")
                    'pnlQuote.Attributes.Add("class", "panel panel-warning")
                Else
                    'Response.Write("oItem pnlQuote is nothing")
                End If

                'HtmlGenericControl pnlQuote = (HtmlGenericControl);
            End If
            End If

        If Not useCookie Then
            litQuote.Text = quoteText.ToString()
        End If


        'Button's URL
        Dim sOrderUrl As String = String.Format(Me.NavigateUrl, oProd.ProductID, Me.DistributionId, oProd.BaseProductID)

        'If OLB. Slight hack. Need to revisit.
        If SiteID = 11 Then

            hplOrder.NavigateUrl = "~/olb/default.aspx"
            hplImageOrder.NavigateUrl = "~/olb/default.aspx"
            hplTitleOrder.NavigateUrl = "~/olb/default.aspx"

            'All others...
        Else
            hplOrder.NavigateUrl = sOrderUrl
            hplImageOrder.NavigateUrl = sOrderUrl
            hplTitleOrder.NavigateUrl = sOrderUrl
        End If


        'Hyperlink Title
        hplTitleOrder.Text = oProd.Name

        'Image
        Dim sCMSBase As String = Taradel.WLUtil.GetRelativeSiteImagesPath
        prodImage.ImageUrl = sCMSBase & oProd.Thumbnail


    End Sub


    Protected Sub AddMore(sender As Object, e As System.EventArgs)

        Response.Redirect("~/Step1-Target.aspx?distid=" & DistributionId)

    End Sub


    Protected Sub ShowDebug(debug As Boolean)

        If String.IsNullOrEmpty(Request.QueryString("debug")) Then
        Else
            Me.debug = True
            debug = True
            Response.Write("debug true")
        End If

        If (debug) Then

            Dim debugStuff As New StringBuilder
            debugStuff.Append("PAGE PROPERTIES:<br />")
            debugStuff.Append("DistributionID: " & DistributionId & "<br />")
            debugStuff.Append("USelectId: " & USelectID & "<br />")
            debugStuff.Append("EDDMMap: " & CStr(EDDMMap) & "<br />")
            'debugStuff.Append("AddressedMap: " & CStr(AddressedMap) & "<br />")
            'debugStuff.Append("TMCMap: " & CStr(TMCMap) & "<br />")
            debugStuff.Append("CategoryID: " & CategoryId & "<br />")
            debugStuff.Append("Quantity: " & Quantity & "<br />")
            'debugStuff.Append("EDDMTotal: " & EDDMTotal & "<br />")
            'debugStuff.Append("AddressedTotal: " & AddressedTotal & "<br />")
            lblDebug.Text = debugStuff.ToString()
            pnlDebug.Visible = True

        End If

    End Sub


    ''added 10/29/2015
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


End Class

