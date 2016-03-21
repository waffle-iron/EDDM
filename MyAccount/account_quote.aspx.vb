Imports System.IO
Imports System.Xml

Partial Class account_quote
    Inherits pageBase

    Protected ReadOnly Property QuoteId() As Integer
        Get
            Return QStringToInt("ID")
        End Get
    End Property

#Region "Page Load Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Using oQuoteA As New pndQuoteTableAdapters.QuoteTableAdapter
            '    Using oQuoteT As pndQuote.QuoteDataTable = oQuoteA.GetQuote(QuoteId, appHelp.GetCustomerID)
            '        If oQuoteT.Rows.Count > 0 Then
            '            Dim oQuote As pndQuote.QuoteRow = oQuoteT.Rows(0)

            '            Name.Text = oQuote.Name
            '            QuoteExpiration.Text = oQuote.QuoteExpiration
            '            lQuoteDate.Text = oQuote.QuoteDate.ToString("MM/dd/yyyy")
            '            pError.Visible = False
            '        Else
            '            pQuote.Visible = False
            '            pError.Visible = True
            '        End If
            '    End Using
            'End Using

            'Page.ClientScript.RegisterClientScriptInclude("account_quote.js", VirtualPathUtility.ToAbsolute("~/resources/account_quote.js"))
            'btnAddToCart.Attributes.Add("onclick", "return validatePage(); ")
        End If
    End Sub
#End Region

#Region "QuoteItems databound events"
    Protected Sub lvQuoteItems_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs)
        'To bind the sublist view [Quote Item Attributes]
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim JobName As TextBox = e.Item.FindControl("JobName")
            Dim rowValidate As HtmlTableRow = e.Item.FindControl("rowValidate")

            Dim oDItem As ListViewDataItem = e.Item
            Dim QuoteItemID As Integer = DataBinder.Eval(oDItem.DataItem, "QuoteItemID")
            Dim lvQuoteItemsAttr As ListView = e.Item.FindControl("lvQuoteItemsAttr")
            oQuoteItemAttributes.SelectParameters("QuoteItemID").DefaultValue = QuoteItemID
            lvQuoteItemsAttr.DataSource = oQuoteItemAttributes
            lvQuoteItemsAttr.DataBind()
        End If
    End Sub
#End Region

#Region "Add to cart button"
    Protected Sub btnAddToCart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToCart.Click
        Page.Validate()
        If Page.IsValid Then
            SaveProductToCart()
            Response.Redirect("~/Cart.aspx")
        End If
    End Sub

    Private Sub SaveProductToCart()
        '-- Add this product to the cart
        Dim oXML As New XmlDocument
        Dim sXML As String = ""
        If Profile.Cart.OuterXml.Trim = "" Or Profile.Cart.OuterXml.ToLower = "<cart />" Or Profile.Cart.OuterXml.ToLower = "<cart/>" Then
            sXML = "<cart />"
        Else
            sXML = Profile.Cart.OuterXml
        End If
        oXML.LoadXml(sXML)

        Dim oCart As XmlNode = oXML.SelectSingleNode("/cart")

        For Each lvItem As ListViewDataItem In lsvQuoteItemEmail.Items
            Dim QuoteItemID As Integer = 0
            Dim oQuoteItemID As HiddenField = DirectCast(lvItem.FindControl("hfQuoteItemID"), HiddenField)

            If oQuoteItemID IsNot Nothing Then
                Dim sQuoteItemID As String = oQuoteItemID.Value
                Integer.TryParse(sQuoteItemID, QuoteItemID)
            End If

            If QuoteItemID > 0 Then
                Dim JobName As TextBox = DirectCast(lvItem.FindControl("JobName"), TextBox)
                Dim sJobName As String = JobName.Text

                Dim JobComments As TextBox = DirectCast(lvItem.FindControl("JobComments"), TextBox)
                Dim sComments As String = JobComments.Text

                Dim sIndex As String = System.Guid.NewGuid.ToString

                Dim iProductID As Integer = 0
                Dim sProductName As String = ""
                Dim sProductSku As String = ""
                Dim iPaperWidth As Integer = 0
                Dim iPaperHeight As Integer = 0
                Dim dProductPrice As Decimal = 0
                Dim dShippingPrice As Decimal = 0
                Dim iShipAddress As Integer = 0
                Dim iQty As Integer = 0
                'Using oQuoteItemA As New pndQuoteTableAdapters.QuoteItemTableAdapter
                '    Using oQuoteItemT As pndQuote.QuoteItemDataTable = oQuoteItemA.GetQuoteItem(QuoteItemID)
                '        If oQuoteItemT.Rows.Count > 0 Then
                '            Dim oQuoteItemRow As pndQuote.QuoteItemRow = oQuoteItemT.Rows(0)
                '            sProductName = oQuoteItemRow.Name
                '            dProductPrice = oQuoteItemRow.PrintingPrice
                '            iQty = oQuoteItemRow.Quantity
                '            dShippingPrice = oQuoteItemRow.ShippingPrice
                '            iShipAddress = oQuoteItemRow.ShipToAddressID
                '            If iShipAddress = 0 Then
                '                iShipAddress = -1
                '            End If
                '        End If
                '    End Using
                'End Using

                Dim oProd As XmlNode = xmlHelp.AddOrUpdateXMLNode(oCart, "Product", "")
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "Index", sIndex)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "ProductID", iProductID)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "Name", sProductName)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "SKU", sProductSku)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "PaperWidth", iPaperWidth)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "PaperHeight", iPaperHeight)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "Price", dProductPrice)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "Weight", 0)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "JobName", JobName.Text)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "JobComments", JobComments.Text)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "QuoteItem", 1)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "QuoteID", QuoteId)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "Quantity", iQty)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "IsFlatRateShipping", False)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "FlatRateShipFee", 0)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "FlatRateShipQty", 1000)
                xmlHelp.AddOrUpdateXMLAttribute(oProd, "DesignFee", 0)

                'Using oAttrA As New pndQuoteTableAdapters.QuoteItemAttributeTableAdapter
                '    Using oAttrT As pndQuote.QuoteItemAttributeDataTable = oAttrA.GetQuoteAttributes(QuoteItemID)
                '        For Each oAttr As pndQuote.QuoteItemAttributeRow In oAttrT.Rows
                '            Dim oAtt As XmlNode = xmlHelp.AddOrUpdateXMLNode(oProd, "Attribute", "")
                '            xmlHelp.AddOrUpdateXMLAttribute(oAtt, "Name", oAttr.Name)
                '            xmlHelp.AddOrUpdateXMLAttribute(oAtt, "Value", oAttr.ItemValue)
                '            xmlHelp.AddOrUpdateXMLAttribute(oAtt, "PriceMod", 0)
                '            xmlHelp.AddOrUpdateXMLAttribute(oAtt, "PriceModPercent", 0)
                '            xmlHelp.AddOrUpdateXMLAttribute(oAtt, "WeightMod", 0)
                '        Next
                '    End Using
                'End Using

                '-- Add the selected designs for this product
                Dim oProdDesign As XmlNode = xmlHelp.AddOrUpdateXMLNode(oProd, "Design", "")

                Dim oFrontDesign As XmlNode = xmlHelp.AddOrUpdateXMLNode(oProdDesign, "Front", "")
                Dim sDesignSelectionType As String = ""
                Dim iDesign As Integer = 0
                Dim sClientFolder As String = "/siteimages/" & Profile.UserName.Replace("@", "_")
                Dim sClientPath As String = Server.MapPath(sClientFolder)
                Dim oDir As New DirectoryInfo(sClientPath)
                If Not oDir.Exists Then
                    oDir.Create()
                End If
                Dim FrontDesignFile As Brettle.Web.NeatUpload.InputFile = DirectCast(lvItem.FindControl("FrontDesignFile"), Brettle.Web.NeatUpload.InputFile)
                If FrontDesignFile.HasFile Then
                    '-- They have uploaded a file
                    sDesignSelectionType = "Upload"
                    Dim sFileName As String = FrontDesignFile.FileName
                    Dim oFI As FileInfo = New FileInfo(sFileName)
                    Dim sExt As String = oFI.Extension
                    Dim iFileSize As Integer = FrontDesignFile.ContentLength

                    Dim sFrontTmp As String = System.Guid.NewGuid.ToString
                    FrontDesignFile.MoveTo(Path.Combine(sClientPath, sFrontTmp & sExt), Brettle.Web.NeatUpload.MoveToOptions.None)

                    xmlHelp.AddOrUpdateXMLAttribute(oFrontDesign, "filetype", sExt)
                    xmlHelp.AddOrUpdateXMLAttribute(oFrontDesign, "filename", oFI.Name)
                    xmlHelp.AddOrUpdateXMLAttribute(oFrontDesign, "realfilename", sFrontTmp & sExt)
                Else
                    sDesignSelectionType = "Omitted"
                End If
                xmlHelp.AddOrUpdateXMLAttribute(oFrontDesign, "DesignSelectionType", sDesignSelectionType)

                Dim oBackDesign As XmlNode = xmlHelp.AddOrUpdateXMLNode(oProdDesign, "Back", "")
                Dim BackDesignFile As Brettle.Web.NeatUpload.InputFile = DirectCast(lvItem.FindControl("BackDesignFile"), Brettle.Web.NeatUpload.InputFile)
                If BackDesignFile.HasFile Then
                    '-- They have uploaded a file
                    sDesignSelectionType = "Upload"
                    Dim sFileName As String = BackDesignFile.FileName
                    Dim oFI As FileInfo = New FileInfo(sFileName)
                    Dim sExt As String = oFI.Extension

                    Dim sBackTmp As String = System.Guid.NewGuid.ToString
                    BackDesignFile.MoveTo(Path.Combine(sClientPath, sBackTmp & sExt), Brettle.Web.NeatUpload.MoveToOptions.None)
                    Dim oFile As String = Path.Combine(sClientFolder, sBackTmp & sExt)

                    xmlHelp.AddOrUpdateXMLAttribute(oBackDesign, "filetype", sExt)
                    xmlHelp.AddOrUpdateXMLAttribute(oBackDesign, "filename", oFI.Name)
                    xmlHelp.AddOrUpdateXMLAttribute(oBackDesign, "realfilename", sBackTmp & sExt)
                Else
                    sDesignSelectionType = "Omitted"
                End If
                xmlHelp.AddOrUpdateXMLAttribute(oBackDesign, "DesignSelectionType", sDesignSelectionType)

                Dim oShipments As XmlNode = xmlHelp.AddOrUpdateXMLNode(oProd, "shipments", "")
                Dim oShipment As XmlNode = xmlHelp.AddOrUpdateXMLNode(oShipments, "shipment", "")
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "Index", System.Guid.NewGuid.ToString)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "AddressID", iShipAddress)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "NonCommercialAddress", 0)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "Qty", iQty)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "Weight", 0)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "Width", 0)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "Height", 0)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "Length", (Integer.Parse(iQty) / 1000) * 3)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "QuoteItem", 1)
                xmlHelp.AddOrUpdateXMLAttribute(oShipment, "Price", dShippingPrice)

            End If
        Next
        Profile.Cart = oXML
        Profile.Save()
    End Sub
#End Region
End Class
