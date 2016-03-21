Imports System.Collections.Generic
Imports System.Xml
Imports System.Linq
Imports Taradel.EF
Imports System.IO
Imports System.Data

Partial Class Receipt
    Inherits appxCMS.PageBase

    Protected ReadOnly Property OrderGuid As String
        Get
            Return appxCMS.Util.Querystring.GetString("refid")
        End Get
    End Property

    Protected oCart As XmlDocument = Nothing

    Protected sGoogleCommerce As String = ""

    'the process should be storing the Friday of the drop
    Public Function ReturnTheNextMonday(oldDate As Object) As String

        Dim theNewDate As DateTime
        DateTime.TryParse(oldDate.ToString(), theNewDate)

        While (theNewDate.DayOfWeek <> DayOfWeek.Monday)
            theNewDate = theNewDate.AddDays(1)
        End While
        Return theNewDate.ToString("dddd, dd MMM yyyy")
    End Function

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not String.IsNullOrEmpty(Me.OrderGuid) Then
            Dim sPhoneNumber As String = "800-481-1656"
            Dim oSite As appxCMS.Site = Nothing
            Dim sSiteName As String = "EDDM"
            Try
                oSite = appxCMS.SiteDataSource.GetSite()
                If oSite IsNot Nothing Then
                    sSiteName = oSite.Name
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
                End If
            Catch ex As Exception

            End Try
            lPhoneNumber.Text = sPhoneNumber

            Dim oGoogle As New StringBuilder

            Dim oOrder As Taradel.OrderHeader = Taradel.OrderDataSource.GetOrder(Me.OrderGuid)
            If oOrder IsNot Nothing Then
                lOrderNumber.Text = oOrder.OrderID

                oGoogle.AppendLine("<script type=""text/javascript"">")
                oGoogle.AppendLine("ga('require', 'ecommerce');")
                oGoogle.AppendLine("ga('ecommerce:addTransaction', {")
                oGoogle.AppendLine("  'id': '" & oOrder.OrderID & "',                     // Transaction ID. Required.")
                oGoogle.AppendLine("  'affiliation': '" & appxCMS.Util.jQuery.JSBless(sSiteName) & "',   // Affiliation or store name.")
                oGoogle.AppendLine("  'revenue': '" & oOrder.OrderAmt & "',               // Grand Total.")
                oGoogle.AppendLine("  'shipping': '" & oOrder.Shipping & "',                  // Shipping.")
                oGoogle.AppendLine("  'tax': '" & oOrder.SalesTax & "'                     // Tax.")
                oGoogle.AppendLine("});")

                If oOrder.OrderItems.Count > 0 Then
                    Dim oOrderItem As Taradel.OrderItem = oOrder.OrderItems.FirstOrDefault
                    oCart = New XmlDocument
                    oCart.LoadXml(oOrderItem.OrderItemXml)

                    Dim oProd As XmlNode = oCart.SelectSingleNode("//Product")
                    Dim DistributionId As Integer = Integer.Parse(xmlhelp.ReadAttribute(oProd, "DistributionId"))
                    Dim oDist As Taradel.CustomerDistribution = Nothing
                    Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Nothing
                    Dim oSelects As List(Of Taradel.MapServer.UserData.AreaSelection) = Nothing

                    Dim numberOfDrops As Integer = 0
                    If DistributionId > 0 Then
                        oDist = Taradel.CustomerDistributions.GetDistribution(DistributionId)
                        If oDist IsNot Nothing Then
                            If Not String.IsNullOrEmpty(oDist.Name) Then
                                lSelectName.Text = oDist.Name
                            End If

                            Dim oNumDrops As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Number of Drops']")
                            If oNumDrops IsNot Nothing Then
                                numberOfDrops = Integer.Parse(xmlhelp.ReadAttribute(oNumDrops, "Value"))
                            Else
                                '-- Count the number of drops and add this in
                                Dim oTotalDrops As XmlNodeList = oProd.SelectNodes("//Drop")
                                numberOfDrops = oTotalDrops.Count
                            End If

                            oSummary = Taradel.CustomerDistributions.GetSelectionSummary(oDist.ReferenceId)
                            oSelects = Taradel.CustomerDistributions.GetSelections(oDist.ReferenceId)

                        End If
                    End If

                    Dim oProduct As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(Integer.Parse(xmlhelp.ReadAttribute(oProd, "ProductID")))
                    Dim oOpts As XmlNodeList = oProd.SelectNodes("//Attribute[@OptCatId != 0]")

                    Dim oOptList As New SortedList(Of Integer, Integer)
                    For Each oOpt As XmlNode In oOpts
                        oOptList.Add(Integer.Parse(xmlhelp.ReadAttribute(oOpt, "OptCatId")), Integer.Parse(xmlhelp.ReadAttribute(oOpt, "Value")))
                    Next

                    Dim bDesign As Boolean = False
                    Dim oDesign As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Professional Design Services']")
                    If oDesign IsNot Nothing Then
                        bDesign = True
                    End If

                    lJobName.Text = xmlhelp.ReadAttribute(oProd, "JobName")
                    Dim sComments As String = xmlhelp.ReadAttribute(oProd, "JobComments").Trim
                    If Not String.IsNullOrEmpty(sComments) Then
                        lComments.Text = sComments
                    Else
                        pComments.Visible = False
                    End If

                    '-- Regenerate a quote
                    Dim iQty As Integer = 0
                    Integer.TryParse(xmlhelp.ReadAttribute(oProd, "Quantity"), iQty)

                    Dim sZip As String = ""
                    If DistributionId = 0 Then
                        '-- Direct ship product, need zip code
                        Dim oShip As XmlNode = oProd.SelectSingleNode("shipments/shipment[1]")
                        If oShip IsNot Nothing Then
                            Try
                                sZip = xmlhelp.ReadAttribute(oShip, "pricehash").Split(New Char() {"-"})(1)
                            Catch ex As Exception
                                'log.Error(ex.Message, ex)
                                'log.Warn("Price has was invalid")
                            End Try
                        End If
                    End If

                    Dim bTemplate As Boolean = False
                    Dim oFront As XmlNode = oProd.SelectSingleNode("Design/Front")
                    If oFront IsNot Nothing Then
                        Dim sDesignType As String = xmlhelp.ReadAttribute(oFront, "DesignSelectionType")
                        If sDesignType.ToLower = "template" Then
                            bTemplate = True
                        End If
                    End If

                    '-- Add our markup in here
                    Dim markup As Double = oProduct.Markup 'appxCMS.Util.CMSSettings.GetDecimal("Catalog", "Markup")
                    Dim markupType As String = oProduct.MarkupType 'appxCMS.Util.CMSSettings.GetSetting("Catalog", "MarkupType")
                    If markup > 0 AndAlso markupType.ToLower = "percent" Then
                        If markup > 1 Then
                            markup = markup / 100
                        End If
                    End If
                    'Dim oQuote As Taradel.ProductPriceQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProduct.BaseProductID.Value, iQty, DistributionId, numberOfDrops, oOptList, sZip, bDesign, bTemplate, markup, markupType)
                    Dim oQuote = New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProduct.BaseProductID.Value, iQty, DistributionId, numberOfDrops, oOptList, sZip, bDesign, bTemplate, markup, markupType)

                    If oQuote.WithDesign Then
                        phEarliest.Visible = True
                        EarliestDelivery.Text = oQuote.EarliestDeliveryDate.ToString("dddd, dd MMMM yyyy")
                    Else
                        phMyDesign.Visible = True

                        Dim sIndex As String = xmlhelp.ReadAttribute(oProd, "Index")
                        '-- Did they upload files?
                        Dim bHasFile As Boolean = False
                        Dim oGNode As XmlNode = oProd.SelectSingleNode("Design")
                        If oGNode IsNot Nothing Then
                            Dim oFNode As XmlNode = oGNode.SelectSingleNode("Front")
                            If oFNode IsNot Nothing Then
                                Dim sDesign As String = oFNode.InnerText
                                Dim sDesignSelectionType As String = xmlhelp.ReadAttribute(oFNode, "DesignSelectionType").ToString.ToLowerInvariant
                                Select Case (sDesignSelectionType)
                                    Case "multiad"

                                    Case "upload"
                                        '-- This was an uploaded file, the data is contained in the xml
                                        imgFile1.ImageUrl = "/resources/imgthumb.ashx?ID=" & sIndex & "&r=1&fb=Front"
                                        bHasFile = True
                                        imgFile1.Visible = True
                                    Case "template"
                                        Dim sTemplateServerHost As String = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
                                        Dim sTemplateId As String = appxCMS.Util.Xml.ReadAttribute(oFNode, "ArtKey")
                                        lLaterMsg.Text = "Template #" & sTemplateId
                                        Dim TemplateId As Integer = 0
                                        If Integer.TryParse(sTemplateId, TemplateId) Then
                                            Dim oTemplate As TemplateCode.Template1 = Nothing
                                            Using oAPI As New TemplateCode.TemplateAPIClient
                                                Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, TemplateId)
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
                                            End If
                                            bHasFile = True
                                        End If
                                    Case "designdiy"
                                    Case Else
                                        imgFile1.Visible = False
                                End Select
                            End If

                            Dim oBNode As XmlNode = oGNode.SelectSingleNode("Back")
                            If oBNode IsNot Nothing Then
                                Dim sDesign As String = oBNode.InnerText
                                Dim sDesignSelectionType As String = xmlhelp.ReadAttribute(oBNode, "DesignSelectionType").ToString.ToLowerInvariant
                                If sDesignSelectionType = "multiad" Then
                                ElseIf sDesignSelectionType = "upload" Then
                                    '-- This was an uploaded file, the data is contained in the xml
                                    imgFile2.ImageUrl = "/resources/imgthumb.ashx?ID=" & sIndex & "&r=1&fb=Back"
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
                        End If
                    End If

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
                    End If

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

                    If DistributionId > 0 Then
                        lSelectDescription.Text = "Your selection of " & iAreaCount.ToString("N0") & " carrier routes, across " & iZipCount.ToString("N0") & " zip code" & sZipPlural & " targeting Residental" & sTargetDesc & " deliveries will reach " & iTotal.ToString("N0") & " postal customers."
                    End If

                    'multiple ? ? ? ? 
                    Dim multiple As Integer = 0
                    If DistributionId > 0 Then
                        Dim oDropsOrg As XmlNode = oProd.SelectSingleNode("Drops")
                        Dim oDrops As XmlNode = oDropsOrg.SelectSingleNode("Drop")
                        Dim testMultiple As String = xmlhelp.ReadAttribute(oDrops, "Multiple")
                        If (testMultiple <> "") Then
                            Integer.TryParse(testMultiple, multiple)
                        End If
                    End If

                    If (multiple > 0) Then
                        lSelectDescription.Text = "Your selection of " & iAreaCount.ToString("N0") & " carrier routes, across " & iZipCount.ToString("N0") & " zip code" & sZipPlural & " targeting Residental" & sTargetDesc & " deliveries will reach " & iTotal.ToString("N0") & " postal customers " & multiple & " times."
                    End If

                    'TotalDistribution = oDist.TotalDeliveries

                    If DistributionId > 0 Then
                        Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(oDist.USelectMethodReference.ForeignKey)
                        'lMapReview.Text = "<iframe id=""MapReview"" margin=""0"" frameborder=""0"" border=""0"" src=""" & oUSelect.ReviewUrl & "?referenceid=" & oDist.ReferenceId & """ height=""250"" width=""100%""></iframe>"
                        Dim sMapImage As String = ""
                        Dim sMapReview As String = oUSelect.ReviewUrl
                        sMapImage = sMapReview & "?referenceid=" & oDist.ReferenceId
                        lMapReview.Text = "<img src=""" & sMapImage & """ style=""max-height:250px;max-width:250px;"" />"
                    End If

                    Dim sQty As String = xmlhelp.ReadAttribute(oProd, "Quantity")
                    Dim dQty As Decimal = 0
                    Decimal.TryParse(sQty, dQty)
                    lQty.Text = dQty.ToString("N0")

                    Dim oOptSb As New StringBuilder
                    oOptSb.AppendLine("<attributes>")
                    For Each oOpt As XmlNode In oOpts
                        oOptSb.AppendLine(oOpt.OuterXml)
                    Next
                    oOptSb.AppendLine("</attributes>")
                    Using oSr As New StringReader(oOptSb.ToString)
                        Using oDs As New DataSet()
                            oDs.ReadXml(oSr)
                            rOpts.DataSource = oDs
                            rOpts.DataBind()
                        End Using
                    End Using

                    If DistributionId > 0 Then
                        Dim oDropsOrg As XmlNode = oProd.SelectSingleNode("Drops")
                        Dim oDrops As XmlNode = oDropsOrg.CloneNode(True)
                        oDrops.Attributes.RemoveAll()
                        Using oSr As New StringReader(oDrops.OuterXml)
                            Using oDs As New DataSet
                                oDs.ReadXml(oSr)
                                rDrops.DataSource = oDs
                                rDrops.DataBind()
                            End Using
                        End Using
                    End If

                    Dim sProdName As String = xmlhelp.ReadAttribute(oProd, "Name")
                    lProductName.Text = sProdName

                    '-- Our paid in full flag is based on Taradel having received payment, from the customer's perspective,
                    '-- if this is a pass-thru gateway, they may have paid in full

                    Dim dTotal As Decimal = 0
                    If oOrder.OrderAmt.HasValue Then
                        dTotal = dTotal + oOrder.OrderAmt.Value
                    End If
                    If oOrder.WLMarkup.HasValue Then
                        dTotal = dTotal + oOrder.WLMarkup.Value
                    End If
                    Dim dPaidAmt As Decimal = 0
                    If oOrder.PaidAmt.HasValue Then
                        dPaidAmt = oOrder.PaidAmt.Value
                    End If
                    If oOrder.PaidInFull Then
                        lPaymentMessage.Text = "Thank you for your payment of " & dTotal.ToString("C") & ". Your order is paid in full."
                    Else
                        If dPaidAmt = 0 Then
                            lPaymentMessage.Text = "Thank you for your order."
                        Else
                            lPaymentMessage.Text = "Thank you for your payment of " & dPaidAmt.ToString("C") & "."

                            '-- Need to adjust deposit amount so that finance amount can be equally split among all future payments
                            Dim deposit As Double = dPaidAmt
                            Dim dBalance As Double = dTotal - deposit - (numberOfDrops * 25)

                            Dim financePayment As Double = (dBalance / numberOfDrops) + 25
                            Dim sSched As String = "Weekly"
                            Dim oSched As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Drop Schedule']")
                            If oSched IsNot Nothing Then
                                sSched = xmlhelp.ReadAttribute(oSched, "Value")
                            End If

                            Dim iAddDays As Integer = 7
                            Select Case sSched.ToLower
                                Case "every 0 weeks"
                                    iAddDays = 0
                                Case "every 1 weeks"
                                    iAddDays = 7
                                Case "every 2 weeks"
                                    iAddDays = 14
                                Case "every 3 weeks"
                                    iAddDays = 21
                                Case "every 4 weeks"
                                    iAddDays = 28
                            End Select

                            '-- This puts their first financed payment at the earliest drop date - 4 days (for the Monday before)
                            'new idea - go to the friday before the drop date 9/5/2014
                            Dim xDrops As XmlNode = oProd.SelectSingleNode("Drops")
                            Dim xDrop As XmlNode = xDrops.SelectSingleNode("Drop")

                            'new way 
                            Dim xDropDate As String = xmlhelp.ReadAttributeValue(xDrop, "Date")
                            Dim dEarliest As DateTime = Date.Parse(xDropDate).AddDays(-7)


                            'old way --> Dim dEarliest As DateTime = oQuote.EarliestDeliveryDate.AddDays(-4)

                            If dEarliest.DayOfWeek <> DayOfWeek.Friday Then
                                dEarliest = dEarliest.AddDays(-1)
                            End If

                            Dim oAdditional As New SortedList(Of DateTime, Double)
                            For i As Integer = 0 To numberOfDrops - 1
                                oAdditional.Add(dEarliest.AddDays(iAddDays * i), financePayment)
                            Next
                            rPayments.DataSource = oAdditional
                            rPayments.DataBind()
                        End If
                    End If

                    oGoogle.AppendLine("ga('ecommerce:addItem', {")
                    oGoogle.AppendLine("  'id': '" & oOrder.OrderID & "',                     // Transaction ID. Required.")
                    oGoogle.AppendLine("  'name': '" & oProduct.Name.Replace("'", "\'") & "',    // Product name. Required.")
                    oGoogle.AppendLine("  'sku': '',                 // SKU/code.")
                    oGoogle.AppendLine("  'category': '',         // Category or variation.")
                    oGoogle.AppendLine("  'price': '" & oQuote.PricePerPiece & "',                 // Unit price.")
                    oGoogle.AppendLine("  'quantity': '" & oQuote.Quantity & "'                   // Quantity.")
                    oGoogle.AppendLine("});")

                End If
                oGoogle.AppendLine("ga('ecommerce:send');")
                oGoogle.AppendLine("</script>")
                sGoogleCommerce = oGoogle.ToString()

                'Page.ClientScript.RegisterStartupScript(GetType(String), "GoogleEcommerce", oGoogle.ToString, True)
            End If
        End If
    End Sub

    Protected Sub rDrops_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rDrops.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim rRoutes As Repeater = DirectCast(e.Item.FindControl("rRoutes"), Repeater)

            Dim dropNumber As Integer = Integer.Parse(DataBinder.Eval(e.Item.DataItem, "Number").ToString())
            Dim oDropOrg As XmlNode = oCart.SelectSingleNode("//Drop[@Number=" & dropNumber & "]")
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
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        If Not String.IsNullOrEmpty(sGoogleCommerce) Then
            Page.ClientScript.RegisterStartupScript(GetType(String), "GoogleCommerce", sGoogleCommerce, False)
            'Page.Form.Controls.Add(New LiteralControl(sGoogleCommerce))
        End If
    End Sub
End Class
