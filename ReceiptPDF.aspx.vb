Imports System.Collections.Generic
Imports System.Xml
Imports System.Linq
Imports Taradel.EF
Imports System.IO
Imports System.Data
Imports System.Globalization
Imports System.Data.SqlClient
Imports TaradelReceiptUtility



Partial Class ReceiptPDF
    Inherits appxCMS.PageBase


    '====================================================================================================================================================
    'NOTES:
    '
    '   This page expects the WHITE LABEL ProductID (wl_Products table), not the Taradel ProductID.
    '
    '   One of the main differences between this page and the Receipt.aspx page is that the Receipt.aspx page CAN read the Profile.Cart content.  
    '   This page, since it Is accessed anonymously by WebSupergoo, can Not pull a user's profile. Instead, the 'oCart' object is created and set globally.
    '
    'UPDATES:
    '
    '   3/15/2016   Added Store Number panel. Will show the required store number captured on Step3-Checkout for Site #95 (and other sites if needed).
    '====================================================================================================================================================



    'Fields
    Protected oCart As XmlDocument = Nothing
    Protected debug As Boolean = False




    'Properties
    Protected ReadOnly Property OrderGuid As String
        Get
            Return appxCMS.Util.Querystring.GetString("refid")
        End Get
    End Property



    Protected ReadOnly Property OrderID As String
        Get
            Return appxCMS.Util.Querystring.GetString("OrderID")
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



    Private _DropFee As Decimal = 0
    Public Property DropFee As Decimal

        Get
            Return _DropFee
        End Get

        Set(value As Decimal)
            _DropFee = value
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



    Private _ExtraCopies As Integer = 0
    Public Property ExtraCopies As Integer
        Get
            Return _ExtraCopies
        End Get
        Set(value As Integer)
            _ExtraCopies = value
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



    Private _TargetedEmailsSelected As Boolean = False
    Public Property TargetedEmailsSelected As Boolean
        Get
            Return _TargetedEmailsSelected
        End Get
        Set(value As Boolean)
            _TargetedEmailsSelected = value
        End Set

    End Property



    Private _AddressedAddOnsSelected As Boolean = False
    Public Property AddressedAddOnsSelected() As Boolean
        Get
            Return _AddressedAddOnsSelected
        End Get
        Set(ByVal value As Boolean)
            _AddressedAddOnsSelected = value
        End Set
    End Property



    Private _siteID As Integer = 0
    Public Property siteID As Integer
        Get
            Return _siteID
        End Get
        Set(ByVal value As Integer)
            _siteID = value
        End Set
    End Property



    Private _IsMultipleImpression As Boolean = False
    Public Property IsMultipleImpression As Boolean

        Get
            Return _IsMultipleImpression
        End Get

        Set(value As Boolean)
            _IsMultipleImpression = value
        End Set

    End Property



    Private _MailPieces As Integer = 0
    Public Property MailPieces As Integer
        Get
            Return _MailPieces
        End Get
        Set(value As Integer)
            _MailPieces = value
        End Set

    End Property



    Private _NumberOfDrops As Integer = 0
    Public Property NumberOfDrops As Integer
        Get
            Return _NumberOfDrops
        End Get
        Set(value As Integer)
            _NumberOfDrops = value
        End Set

    End Property


    Private _ProductType As String
    Private Property ProductType() As String

        Get
            Return _ProductType
        End Get
        Set(value As String)
            _ProductType = value
        End Set

    End Property






    'Methods
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        If Not String.IsNullOrEmpty(Me.OrderGuid) Then

            SetPageProperties()
            ShowHidePageElements()
            LoadTerms()
            ShowDebug(debug)

        Else

            pnlReceiptSuccess.Visible = False
            pnlError.Visible = True
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."

        End If

    End Sub



    Protected Sub SetPageProperties()


        'This page cannot read the Profile.Cart so the XmlDocument needs to be recreated.
        'Recreate the cart from the order.
        'Extract from database.
        Dim oOrder As Taradel.OrderHeader = Taradel.OrderDataSource.GetOrder(Me.OrderGuid)
        Dim oOrderItem As Taradel.OrderItem = oOrder.OrderItems.FirstOrDefault


        'Recreate the cart from the order
        Dim oXml As New XmlDocument()
        oXml.LoadXml("<cart/>")

        'Create Product Node and add attributes
        oCart = oXml

        Dim sb As New StringBuilder
        sb.Append("<cart>")

        For Each yOrderItem As Taradel.OrderItem In oOrder.OrderItems
            sb.Append(yOrderItem.OrderItemXml)
        Next

        sb.Append("</cart>")
        oCart.LoadXml(sb.ToString())


        'DistributionID
        DistributionId = CartUtility.GetDistributionID(oCart)


        'SiteID
        siteID = appxCMS.Util.CMSSettings.GetSiteId


        'Determine the USelectID. This will help tell the whole page how to behave.
        Dim oDist As Taradel.CustomerDistribution = Taradel.CustomerDistributions.GetDistribution(DistributionId)
        USelectID = oDist.USelectMethodReference.ForeignKey()



        'Determine the DistributionType and set flags
        Select Case USelectID
            Case 1
                EDDMMap = True
                ProductType = "eddm"
            Case 5
                UploadedAddressedList = True
                ProductType = "addressed"
            Case 6
                GeneratedAddressedList = True
                ProductType = "addressed"
        End Select




        'Build OrderCalc
        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)



        'Is Multiple Impression?
        'IsMultipleImpression = orderCalc.IsThisAMultiple  <-- this does not work.  orderCalc (usp_RetrieveOrderPaymentDetails4) does not properly set this value. 3/9/2016. DSF.
        IsMultipleImpression = TaradelReceiptUtility.RetrieveIsThisAMultipleImpression(oCart)



        'Mail Pieces (total pcs.  total selected x num impressions from OrderCalc
        MailPieces = orderCalc.MailPieces


        'Drops / Impressions
        NumberOfDrops = orderCalc.NumOfDrops




        'Dertmine NewMoverSelected
        Dim campaignScheduleObj As New CampaignScheduler()
        If (campaignScheduleObj.CheckForNewMovers(oCart)) = True Then
            NewMoverSelected = True
        End If


        'Determine EmailCampaign Selected
        If (campaignScheduleObj.CheckForEmails(oCart)) = True Then
            TargetedEmailsSelected = True
        End If

        'Determine Addressed Add-Ons
        If (campaignScheduleObj.CheckForAddressedAddOns(oCart)) Then
            AddressedAddOnsSelected = True
        End If

        'Drop Fee
        DropFee = orderCalc.DropFee



        'Professional Design.  Pick the correct type
        'If Design Fees are ever set to $0 as a promo, we may need to use this:
        'Dim oDesign As XmlNode = oProd.SelectSingleNode("Attribute[@Name='Professional Design Services']")
        If (EDDMMap) Then
            If (RetrieveDesignFeeFromOrder("EDDM") > 0) Then
                IsProfessionalDesign = True
                DesignFee = RetrieveDesignFeeFromOrder("EDDM")
            End If
        End If

        If (GeneratedAddressedList) Or (UploadedAddressedList) Then
            If (RetrieveDesignFeeFromOrder("AddressedList") > 0) Then
                IsProfessionalDesign = True
                DesignFee = RetrieveDesignFeeFromOrder("AddressedList")
            End If
        End If


        'Extra Copies
        ExtraCopies = orderCalc.ExtraPieces


        orderCalc = Nothing

    End Sub



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If debug = True Then

            pnlDebug.Visible = True
            pnlProductDisplay.Visible = True
            pnlCampaignOverview.Visible = True
            pnlExtraPcs.Visible = True
            pnlDesignFee.Visible = True
            pnlNumOfdrops.Visible = True
            pnlNewMovers.Visible = True
            pnlEmails.Visible = True
            pnlFinanceFees.Visible = True
            pnlShippingFee.Visible = True
            pnlCouponDiscount.Visible = True
            pnlSalesTax.Visible = True
            pnlTerms.Visible = False

        End If

    End Sub



    Protected Sub rptEddmDrops_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptEddmDrops.ItemDataBound

        'Dim oCart As XmlDocument = Profile.Cart

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            Dim rptEddmRoutes As Repeater = DirectCast(e.Item.FindControl("rptEddmRoutes"), Repeater)
            Dim dropNumber As Integer = Integer.Parse(DataBinder.Eval(e.Item.DataItem, "Number").ToString())
            Dim dropNode As XmlNode = oCart.SelectSingleNode("//Drop[@Number=" & dropNumber & "][@Type='EDDM']")

            If dropNode IsNot Nothing Then

                Dim oDrop As XmlNode = dropNode.CloneNode(True)
                oDrop.Attributes.RemoveAll()

                Using oSr As New StringReader(oDrop.OuterXml)
                    Using oDs As New DataSet
                        oDs.ReadXml(oSr)

                        rptEddmRoutes.DataSource = oDs
                        rptEddmRoutes.DataBind()

                    End Using
                End Using

            Else

            End If

        End If

    End Sub



    Private Function RetrieveDesignFeeFromOrder(productType As String) As Integer

        'Dim oCart As XmlDocument = Profile.Cart
        Return CartUtility.GetDesignFee(oCart, productType)

    End Function



    Protected Function GetPONumber() As String

        Dim results As String = ""
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim errorMsg As New StringBuilder()
        Dim selectSql As String = "SELECT [PaymentRef] FROM [pnd_OrderPayment] WHERE OrderID = " & OrderID & " AND [PaymentType] LIKE 'Purchase Order'"
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)

        Try

            connObj.Open()
            results = sqlCommand.ExecuteScalar()


        Catch objException As Exception

            errorMsg.Append("The following errors occurred:<br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " & objException.Message & "</li>")
            errorMsg.Append("<li>Source: " & objException.Source & "<li>")
            errorMsg.Append("<li>Stack Trace: " & objException.StackTrace & "<li>")
            errorMsg.Append("<li>Target Site: " & objException.TargetSite.Name & "<li>")
            errorMsg.Append("<li>SQL: " & selectSql & "<li>")
            errorMsg.Append("</ul>")

            results = errorMsg.ToString()
            pnlError.Visible = True
            pnlReceiptSuccess.Visible = False
            litErrorMessage.Text = "Sorry but there was an error loading this page.  The IT Staff has been notified."
            EmailUtility.SendAdminEmail("Error extracting PO Number in Receipt.aspx.<br /><br />Details:" & results)


        Finally

            connObj.Close()

        End Try

        Return results

    End Function





    'Page Builders
    Protected Sub ShowHidePageElements()

        BuildTaradelHeader()
        BuildOrderHeader()


        'Show correct product panel
        If (EDDMMap) Then
            BuildProductDisplay("EDDM")
            BuildMapDisplay("EDDM")
            BuildEddmDropsDisplay()
            BuildDesignDisplay("EDDM")
        End If

        If (GeneratedAddressedList) Or (UploadedAddressedList) Then
            BuildProductDisplay("AddressedList")
            BuildMapDisplay("AddressedList")
            BuildAddressedDropsDisplay()
            BuildDesignDisplay("AddressedList")
        End If


        'New Mover
        If (NewMoverSelected) Then
            BuildNewMoverDisplay()
        End If


        'Emails
        If (TargetedEmailsSelected) Then
            pnlEmails.Visible = True
            BuildTargetedEmailsDisplay()
        End If


        'Addressed AddOns
        If (AddressedAddOnsSelected) Then
            pnlAddressedAddOns.Visible = True
            BuildAddressedAddOnsDisplay()
        End If


        'If one of the Add Ons is selected then show the schedule
        If ((TargetedEmailsSelected) Or (NewMoverSelected)) Then

            pnlCampaignOverview.Visible = True

            If (TargetedEmailsSelected) Then
                BuildCampaignSchedule("Targeted Emails")
            End If

            If (NewMoverSelected) Then
                BuildCampaignSchedule("New Mover Postcard")
            End If

        End If




        'Drop Fee
        If (DropFee) > 0 Then
            ShowDropFeeDisplay()
        End If

        'Extra Copies
        If ExtraCopies > 0 Then
            pnlExtraPcs.Visible = True
            BuildExtraCopiesDisplay()
        End If


        'Professional Design Fee
        If (IsProfessionalDesign) Then
            pnlDesignFee.Visible = True
            lblDesignFee.Text = DesignFee().ToString("C")
            pnlNextStepsDisplay.Visible = True
        End If


        'Coupon display.  No test - just execute sub.
        BuildCouponDisplay()


        'Sales Tax display.  No test - just execute.
        BuildSalesTaxDisplay()


        'Show or hide Payment Information section
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(siteID)

        If Not (SiteDetails.ShowReceiptPaymentInfo) Then
            pnlPaymentPanel.Visible = False
        End If

        If Not (SiteDetails.ShowReceiptTermsAndConditions) Then
            pnlTerms.Visible = False
        End If


        BuildFinanceDisplay()

        BuildMailingDisplay()

        BuildTotalsPaymentsDisplay()

        If (SiteDetails.UseReceiptPDFBarCodes) Then
            ShowBarCodes()
        End If

        'Show Locked Routes Message if needed
        If (SiteDetails.OffersExclusiveRoutes) Then
            BuildLockedRoutesDisplay()
        End If


    End Sub



    Protected Sub BuildAddressedAddOnsDisplay()
        Dim targetedEmailsSchedObj As New CampaignScheduler()

        'Get Price
        Dim AddressedAddOnNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='AddressedMail AddOn']")
        Dim AddressedAddOnPrice As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(AddressedAddOnNode, "Price"))
        litAddressedAddOnsPrice.Text = AddressedAddOnPrice.ToString("C")

        'Get Description
        Dim AddressedAddOnQTY As Integer = Convert.ToInt32(xmlhelp.ReadAttribute(AddressedAddOnNode, "Quantity").ToString().Replace(",", ""))
        Dim AddressedAddOnPPP As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(AddressedAddOnNode, "PricePerPiece"))

        Dim addressedAddOnsNode2 As XmlNode = AddressedAddOnNode.SelectSingleNode("Drops/Drop")
        Dim numAddressedAddOns As Integer = Convert.ToInt32(xmlhelp.ReadAttribute(addressedAddOnsNode2, "Multiple").ToString())


        litAddressedAddOnsDescription.Text = AddressedAddOnQTY.ToString("N0") & " @ " & AddressedAddOnPPP.ToString("C") '+ " Mailed directly to high value prospects in your market area but outside of your EDDM mail routes " + numAddressedAddOns.ToString() + " time(s)."
    End Sub



    Protected Sub BuildTargetedEmailsDisplay()


        Dim targetedEmailsSchedObj As New CampaignScheduler()

        'Get Price
        Dim emailNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='Targeted Emails']")
        Dim emailPrice As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(emailNode, "Price"))
        litEmailPrice.Text = emailPrice.ToString("C")

        'Get Description
        Dim emailQTY As Integer = Convert.ToInt32(xmlhelp.ReadAttribute(emailNode, "Quantity").ToString().Replace(",", ""))
        Dim emailPPP As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(emailNode, "PricePerPiece"))

        'new new new 1/13/2016 rs 
        emailQTY = emailQTY / 3
        'end new new new 1/13/2016 rs

        'refactor refactor refactor 1/13/2016 rs
        litEmailDescription.Text = emailQTY.ToString("N0") & " Targeted Emails emailed " & 3 & " times"


    End Sub



    Protected Sub BuildTaradelHeader()

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

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

                lPhoneNumber.Text = sPhoneNumber
                lReceiptPartnerPhone.Text = sPhoneNumber
                lReceiptPartnerName.Text = oSite.Name

                'Address string
                addressText = oSite.Address1 & "<br />"

                If oSite.Address2.Length > 0 Then
                    addressText = addressText & oSite.Address2 & "<br />"
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
                lPhoneNumber.Text = "(804) 364-8444"
                lReceiptPartnerPhone.Text = "(804) 364-8444"
                lReceiptPartnerName.Text = "<strong>Taradel</strong>"
                lReceiptPartnerAddress.Text = "4805 Lake Brooke Drive - Suite 140<br/>Glen Allen, VA 23060<br />"
            End If

        Catch ex As Exception

        End Try


        'Look for PO info
        If orderCalc.CustomerHasPaid = 0 Then
            pnlPONumber.Visible = True
            litPOText.Text = SiteUtility.GetStringResourceValue(siteID, "POLabel") & ": " & GetPONumber()
        End If


        'For Staples Act Mgr Site 95.  If other sites start to use this feature then make it configurable!
        If siteID = 95 Then
            ShowStoreNumber(OrderID)
        End If


        'Job Name - Currently hidden
        litJobName.Text = CartUtility.GetJobName(oCart, "EDDM")

        'Job Comments
        Dim sComments As String = CartUtility.GetJobComments(oCart, "EDDM")

        If Not String.IsNullOrEmpty(sComments) Then
            phJobComments.Visible = True
            litJobComments.Text = "<small>Job Comments:<br />" & sComments & "</small>"
        End If


    End Sub



    Protected Sub BuildOrderHeader()

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        Dim oOrder As Taradel.OrderHeader = Taradel.OrderDataSource.GetOrder(Me.OrderGuid)

        'Note - Job Comments are in Xml ONLY (not orderCalc) so they are loaded later on.
        lReceiptDate.Text = orderCalc.ReceiptDate.ToShortDateString()
        lReceiptNumber.Text = "Receipt #: " & orderCalc.ReceiptNumber
        lOrderNum.Text = "Order #: " & Int32.Parse(OrderID)

        If oOrder IsNot Nothing Then

            lSoldTo.Text = oOrder.FullName
            lSoldToCompanyName.Text = oOrder.CompanyName
            lSoldToAddress.Text = oOrder.Address
            lSoldToCityStateZip.Text = oOrder.City + " " + oOrder.State + ", " + oOrder.ZipCode
            lSoldToPhone.Text = oOrder.PhoneNumber
            lCustomerID.Text = orderCalc.CustomerID.ToString()
            litCustNumLabel.Text = SiteUtility.GetStringResourceValue(siteID, "CustNumLabel")

        Else

            pnlReceiptSuccess.Visible = False
            pnlError.Visible = True
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."
            EmailUtility.SendAdminEmail("oOrder was NULL (ReceiptPDF - BuildOrderHeader).")

        End If


        oOrder = Nothing
        orderCalc = Nothing

    End Sub



    Protected Sub BuildProductDisplay(productType As String)


        If (productType = "EDDM") Then
            litProductType.Text = "EDDM"
            litProductName.Text = CartUtility.GetProductName(oCart, "EDDM")

        ElseIf (productType = "AddressedList") Then
            litProductType.Text = "ADDRESSED LIST"
            litProductName.Text = CartUtility.GetProductName(oCart, "AddressedList")

        End If




        'Product Options
        '1) Get NodeList
        '2) Add to SortedList
        '3) Make String
        '4) Give it to repeater as datasource
        Dim oOpts As XmlNodeList = CartUtility.GetProdOptionsList(oCart, productType)
        Dim oOptList As New SortedList(Of Integer, Integer)

        For Each oOpt As XmlNode In oOpts
            oOptList.Add(Integer.Parse(xmlhelp.ReadAttribute(oOpt, "OptCatId")), Integer.Parse(xmlhelp.ReadAttribute(oOpt, "Value")))
        Next

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



    End Sub



    Protected Sub BuildTotalsPaymentsDisplay()

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        Dim oOrder As Taradel.OrderHeader = Taradel.OrderDataSource.GetOrder(Me.OrderGuid)

        lSubtotal.Text = OrderCalculator.FormatAsCurrency(orderCalc.Subtotal)
        lOrderTotal.Text = OrderCalculator.FormatAsCurrency(orderCalc.OrderTotal)

        lPaymentsCredits.Text = OrderCalculator.FormatAsCredit(orderCalc.CustomerHasPaid)
        lRemainingBalance.Text = OrderCalculator.FormatAsCurrency(orderCalc.FinanceCustomerOwes)


        'Payments / Financing logic
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

        If orderCalc.FinanceCustomerOwes = 0 Then
            lPaymentMessage.Text = "Thank you for your payment of " & OrderCalculator.FormatAsCurrency(orderCalc.CustomerHasPaid) & ". Your order is paid in full."
        Else
            If orderCalc.CustomerHasPaid = 0 Then
                lPaymentMessage.Text = "Thank you for your order."
            Else

                Dim lstPayments As New List(Of Payment)
                Dim amountOfPayment As String

                Dim i As Integer = 1
                While i <= orderCalc.NumOfDrops

                    Dim xPayment As New Payment()

                    xPayment.Payment = orderCalc.FinancePaymentAmount
                    amountOfPayment = orderCalc.FinancePaymentAmount
                    xPayment.Balance = (orderCalc.FinancePaymentAmount * orderCalc.NumOfDrops) - (orderCalc.FinancePaymentAmount * i)

                    If (i = 1) Then
                        xPayment.DropDate = orderCalc.FirstDropDate
                    Else
                        xPayment.DropDate = orderCalc.FirstDropDate.AddDays(orderCalc.DropIntervalAsDays * (i - 1))
                    End If

                    xPayment.BillDate = xPayment.DropDate.AddDays(-7).ToShortDateString() 'the previous Friday

                    If (xPayment.BillDate < DateTime.Today) Then
                        xPayment.BillDate = DateTime.Today
                    End If

                    lstPayments.Add(xPayment)

                    i = i + 1

                End While

                rPayments2.DataSource = lstPayments
                amountOfPayment = OrderCalculator.FormatAsCurrency(lstPayments(0).Payment)
                rPayments2.DataBind()

                lPaymentMessage.Text = "Thank you for your payment of " & OrderCalculator.FormatAsCurrency(orderCalc.CustomerHasPaid) & ". "

                If lstPayments.Count = 1 Then
                    lPaymentMessage.Text += "<br/>You have " & lstPayments.Count & " payment of " & amountOfPayment & " remaining."
                Else
                    lPaymentMessage.Text += "<br/>You have " & lstPayments.Count & " payments of " & amountOfPayment & " remaining."
                End If

            End If

        End If


        orderCalc = Nothing
        oOrder = Nothing

    End Sub



    Protected Sub BuildMailingDisplay()

        pnlMailingPcs.Visible = True

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        'Mailing
        litNumOfPcs.Text = orderCalc.MailPieces.ToString("N0") & " pieces @ " & OrderCalculator.FormatAsCurrency3(orderCalc.PricePerPiece)
        lPriceOfPieces.Text = OrderCalculator.FormatAsCurrency(orderCalc.MailPiecesPrice)

        orderCalc = Nothing

    End Sub



    Protected Sub ShowDropFeeDisplay()

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        If (DropFee > 0) Then
            pnlNumOfdrops.Visible = True
            litNumOfDrops.Text = (orderCalc.NumOfDrops - 1).ToString() & " @ $99"
            lDropFee.Text = OrderCalculator.FormatAsCurrency(orderCalc.DropFee)
        End If

        orderCalc = Nothing

    End Sub



    Protected Sub BuildFinanceDisplay()

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        'Finance Fees
        If (orderCalc.FinanceFee = 0) Then
            pnlFinanceFees.Visible = False
        Else
            pnlFinanceFees.Visible = True
            lFinanceFee.Text = OrderCalculator.FormatAsCurrency(orderCalc.FinanceFee)
            lFinanceFeeDetail.Text = (orderCalc.FinanceFee / 25).ToString("N0") & " @ $25"
        End If

        orderCalc = Nothing

    End Sub



    Protected Sub BuildSalesTaxDisplay()

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        If (orderCalc.SalesTax = 0) Then
            pnlSalesTax.Visible = False
        Else
            pnlSalesTax.Visible = True
            lblSalesTax.Text = OrderCalculator.FormatAsCurrency(orderCalc.SalesTax)
            lblSalesTaxMessage.Text = orderCalc.SalesTaxMessage
        End If


        'Sales Tax Disclaimer
        If (appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableSalesTax")) Then
            litSalesTaxDisclaimer.Text = "(Does not include Sales Tax.)"
        End If


        orderCalc = Nothing

    End Sub



    Protected Sub BuildCouponDisplay()

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        If (orderCalc.CouponDiscount = 0) Then
            pnlCouponDiscount.Visible = False
        Else
            pnlCouponDiscount.Visible = True
            lCouponDiscount.Text = OrderCalculator.FormatAsCredit(orderCalc.CouponDiscount)
            litCouponName.Text = orderCalc.CouponCode
        End If

        orderCalc = Nothing


    End Sub



    Protected Sub BuildExtraCopiesDisplay()

        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        litExtraQuantity.Text = orderCalc.ExtraPieces.ToString("N0")
        litExtraPricePerPiece.Text = TaradelReceiptUtility.OrderCalculator.FormatAsCurrency3(orderCalc.ExtraPiecesPricePerPiece)

        Dim shipTo As String = orderCalc.ExtraPiecesAddress1

        If (orderCalc.ExtraPiecesAddress2 IsNot Nothing) Then
            If Not String.IsNullOrEmpty(orderCalc.ExtraPiecesAddress2) Then
                shipTo += "<br/>" + orderCalc.ExtraPiecesAddress2
            End If
        End If

        shipTo += "<br/>" + orderCalc.ExtraPiecesAddressCityStateZip

        litExtraQuantityAddress.Text = shipTo

        lblExtraPrice.Text = orderCalc.ExtraPiecesPrice.ToString("C")

        orderCalc = Nothing

    End Sub



    Protected Sub BuildNewMoverDisplay()

        pnlNewMovers.Visible = True

        'get Price
        Dim newMoverNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='New Mover Postcard']")
        Dim nmPrice As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(newMoverNode, "Price"))
        litNewMoverPrice.Text = nmPrice.ToString("C")

        'get Description
        Dim nmQTY As Integer = Convert.ToInt32(xmlhelp.ReadAttribute(newMoverNode, "Quantity"))
        Dim nmPPP As Decimal = Convert.ToDecimal(xmlhelp.ReadAttribute(newMoverNode, "PricePerPiece"))
        litNewMoverDescription.Text = "1st Month: " & nmQTY & " postcards @ " & nmPPP.ToString("C")

    End Sub



    Protected Sub BuildCampaignSchedule(productType As String)

        'Get the schedule DataView and bind it to the repeater.
        Dim scheduleDataView As DataView
        Dim scheduleDataUtility As New CampaignScheduler()

        scheduleDataView = scheduleDataUtility.GetScheduleData2(oCart.OuterXml)
        rptSchedule.DataSource = scheduleDataView
        rptSchedule.DataBind()

        scheduleDataView.Dispose()

    End Sub



    Protected Sub BuildMapDisplay(productNodeType As String)

        'Dim oCart As XmlDocument = Profile.Cart
        Dim oDist As Taradel.CustomerDistribution = Nothing
        Dim oSummary As Taradel.MapServer.UserData.SelectionSummary = Nothing
        Dim oSelects As List(Of Taradel.MapServer.UserData.AreaSelection) = Nothing
        Dim numberOfDrops As Integer = CartUtility.GetNumberOfImpressions(oCart, productNodeType)

        If DistributionId > 0 Then

            oDist = Taradel.CustomerDistributions.GetDistribution(DistributionId)

            If oDist IsNot Nothing Then

                If Not String.IsNullOrEmpty(oDist.Name) Then
                    litSelectName.Text = oDist.Name
                End If

                oSummary = Taradel.CustomerDistributions.GetSelectionSummary(oDist.ReferenceId)
                oSelects = Taradel.CustomerDistributions.GetSelections(oDist.ReferenceId)

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
            Dim numPcs = CartUtility.GetAddressedDropTotal(oCart, IsMultipleImpression, numberOfDrops, MailPieces, "AddressedList")

            If (GeneratedAddressedList) Then
                litSelectDescription.Text = "Your custom Generated List will allow you to reach and deliver to " & numPcs.ToString("N0") & " targeted addresses."
            ElseIf (UploadedAddressedList) Then
                litSelectDescription.Text = "Your Uploaded List will allow you to reach and deliver to " & numPcs.ToString("N0") & " targeted addresses."
            Else
                litSelectDescription.Text = "Your selection of " & iAreaCount.ToString("N0") & " carrier routes, across " & iZipCount.ToString("N0") & " zip code" & sZipPlural & " targeting Residental" & sTargetDesc & " deliveries will reach " & iTotal.ToString("N0") & " postal customers."
            End If
        End If


        If DistributionId > 0 Then
            Dim oUSelect As Taradel.USelectMethod = Taradel.Helper.USelect.GetById(oDist.USelectMethodReference.ForeignKey)
            Dim sMapImage As String = ""
            Dim sMapReview As String = oUSelect.ReviewUrl

            sMapImage = sMapReview & "?referenceid=" & oDist.ReferenceId
            imgMap.ImageUrl = sMapImage
        End If

    End Sub



    Protected Sub BuildEddmDropsDisplay()

        pnlEddmDrops.Visible = True

        Dim productNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='EDDM']")
        Dim oDropsOrg2 As XmlNode = productNode.SelectSingleNode("Drops[@Type='EDDM']")
        Dim oDrops2 As XmlNode = oDropsOrg2.CloneNode(True)

        oDrops2.Attributes.RemoveAll()
        Using oSr As New StringReader(oDrops2.OuterXml)
            Using oDs As New DataSet
                oDs.ReadXml(oSr)
                rptEddmDrops.DataSource = oDs
                rptEddmDrops.DataBind()
            End Using
        End Using

    End Sub



    Protected Sub BuildAddressedDropsDisplay()

        pnlAddressedDrops.Visible = True

        litAddressedPcs.Text = CartUtility.GetDropQtyList(oCart, "AddressedList")
        litAddressedDropDates.Text = CartUtility.GetDropDatesList(oCart, "AddressedList")

        If (GeneratedAddressedList) Then
            litDemographicFilters.Text = AddressedListUtility.GetSavedFilters(DistributionId)
        Else
            litDemographicFilters.Text = "(not applicable to uploaded lists)"
        End If


    End Sub



    Protected Sub BuildDesignDisplay(productNodeType As String)

        Dim oProd As XmlNode = oCart.SelectSingleNode("//Product[@Type='" & productNodeType & "']")

        If IsProfessionalDesign Then

            phProDesign.Visible = True

        Else

            phMyDesign.Visible = True

            Dim sIndex As String = CartUtility.GetProductIndex(oCart, productNodeType)

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
                            phMyDesign.Visible = True
                            '-- This was an uploaded file, the data is contained in the xml
                            imgFile1.ImageUrl = "/resources/imgthumb.ashx?ID=" & sIndex & "&r=1&fb=Front"
                            bHasFile = True
                            imgFile1.Visible = True
                            lblDesignType.Text = "My Custom Design: "

                        Case "template"
                            phMyDesign.Visible = True
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
                                    imgFile1.ImageUrl = "http:" & sTemplateServerHost & "/templates/icon/" & oTemplate.FrontImage
                                    imgFile1.Visible = True
                                    If Not String.IsNullOrEmpty(oTemplate.BackImage) Then
                                        imgFile2.ImageUrl = "http:" & sTemplateServerHost & "/templates/icon/" & oTemplate.BackImage
                                        imgFile2.Visible = True
                                    Else
                                        imgFile2.Visible = False
                                    End If

                                    lblDesignType.Text = "Selected Template: "

                                End If

                                bHasFile = True

                            End If
                        Case "designdiy"
                            'nothing here
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
                        'nothing here
                    Else
                        '-- This page design was omitted
                        imgFile2.Visible = False
                    End If
                Else
                    If String.IsNullOrEmpty(imgFile2.ImageUrl) Then
                        imgFile2.Visible = False
                    End If
                End If

            Else
                Response.Write("<h1>oGNode IS nothing</h1>")
            End If

            If Not bHasFile Then
                imgFile1.Visible = False
                imgFile2.Visible = False
                lLaterMsg.Text = "I will upload my files later."
                lblDesignType.Text = "Custom Design: "
            End If

        End If



    End Sub



    Protected Sub ShowBarCodes()


        Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
        orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)

        pnlStaplesFooter.Visible = True
        lblStaplesPostage.Text = orderCalc.Postage.ToString("C")
        lblStaplesProductCost.Text = (orderCalc.Subtotal - orderCalc.Postage - orderCalc.CouponDiscount).ToString("C")

        imgStaplesPostage.ImageUrl = SiteUtility.GetStringResourceValue(siteID, "ReceiptPostageBarCode")
        imgStaplesProduct.ImageUrl = SiteUtility.GetStringResourceValue(siteID, "ReceiptProductBarCode")


    End Sub



    Private Sub LoadTerms()

        Dim oContentAreas As List(Of appxCMS.AppliedPageContent) = appxCMS.ContentDataSource.GetAppliedPageContent("/Legal-Sales-Terms", siteID)
        Dim oContent As appxCMS.AppliedPageContent = oContentAreas.FirstOrDefault(Function(apc As appxCMS.AppliedPageContent) apc.PlaceHolder = "phBody")
        Dim terms As String = ""

        If oContent IsNot Nothing Then
            terms = oContent.Content
            litTermsAndAgreement.Text = terms
        End If

    End Sub



    Private Sub BuildLockedRoutesDisplay()

        phLockedRoutesMsg.Visible = True
        litLockedRoutes.Text = ExclusiveUtility.RetrieveLockMessage(OrderID)

    End Sub



    Protected Sub ShowStoreNumber(OrderID As String)


        'Get the Store Number TagGroupID
        Dim logger As New LogWriter()
        Dim tagGroupID As Integer = 0
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim getTagGroupIDSql As String = "SELECT TOP 1 [OrderTagGroupID] FROM [OrderTagGroup] WHERE [Name] LIKE 'Store Number'"
        Dim connectionObj As SqlConnection = New SqlConnection(connectionString)
        Dim tagGroupIDSqlCommand As SqlCommand = New SqlCommand()
        Dim errorMsg As StringBuilder = New StringBuilder()

        tagGroupIDSqlCommand.CommandType = System.Data.CommandType.Text
        tagGroupIDSqlCommand.CommandText = getTagGroupIDSql
        tagGroupIDSqlCommand.Connection = connectionObj

        Try

            connectionObj.Open()
            tagGroupID = Convert.ToInt32(tagGroupIDSqlCommand.ExecuteScalar())



            If tagGroupID > 0 Then


                Dim lookupStoreNumberSql As String = "SELECT TOP 1 ISNULL([Tag], 0) As Tag FROM [OrderTag] WHERE [OrderTagGroupID] = " & tagGroupID & " AND [OrderID] = " & OrderID
                Dim storeNumberSqlCommand As New SqlCommand(lookupStoreNumberSql, connectionObj)
                Dim storeNumber As String = ""

                storeNumberSqlCommand.CommandType = System.Data.CommandType.Text
                storeNumberSqlCommand.CommandText = lookupStoreNumberSql
                storeNumberSqlCommand.Connection = connectionObj


                Try

                    storeNumber = storeNumberSqlCommand.ExecuteScalar().ToString()

                    If storeNumber.Length > 3 Then
                        pnlStoreNumber.Visible = True
                        litStoreNumber.Text = storeNumber.ToString()
                    End If

                Catch objException As Exception
                    errorMsg.Append("The following errors occurred" & Environment.NewLine)
                    errorMsg.Append("Message: " & objException.Message & Environment.NewLine)
                    errorMsg.Append("Source: " & objException.Source & Environment.NewLine)
                    errorMsg.Append("Stack Trace: " & objException.StackTrace & Environment.NewLine)
                    errorMsg.Append("Target Site: " & objException.TargetSite.Name & Environment.NewLine)
                    logger.RecordInLog("[ReceiptPDF.aspx ERROR] Error in ShowStoreNumber: " & Environment.NewLine & errorMsg.ToString())
                    EmailUtility.SendAdminEmail("There was an error on ReceiptPDF.aspx (ShowStoreNumber). Check the EDDM-Log.")
                End Try

            End If


        Catch objException As Exception

            errorMsg.Append("The following errors occurred" & Environment.NewLine)
            errorMsg.Append("Message: " & objException.Message & Environment.NewLine)
            errorMsg.Append("Source: " & objException.Source & Environment.NewLine)
            errorMsg.Append("Stack Trace: " & objException.StackTrace & Environment.NewLine)
            errorMsg.Append("Target Site: " & objException.TargetSite.Name & Environment.NewLine)
            logger.RecordInLog("[ReceiptPDF.aspx ERROR] Error Retrieving TagGroupID: " & Environment.NewLine & errorMsg.ToString())
            EmailUtility.SendAdminEmail("There was an error retriveing OrderTagID in ReceiptPDF.aspx. Check the EDDM-Log.")

        Finally
            connectionObj.Close()
        End Try



    End Sub




    'Debug
    Protected Sub ShowDebug(debug As Boolean)

        If (debug) Then


            Dim orderCalc As TaradelReceiptUtility.OrderCalculator = New TaradelReceiptUtility.OrderCalculator()
            orderCalc = TaradelReceiptUtility.RetrieveOrderCalculator(Int32.Parse(OrderID), ProductType)


            Dim dt As DataTable = TaradelReceiptUtility.LogOrderCalculator(orderCalc)

            Dim dtView As New DataView(dt)
            dtView.Sort = "Name"
            gvTest.DataSource = dtView
            gvTest.DataBind()
            gvTest.Visible = True

            Dim debugData As New StringBuilder()
            pnlDebug.Visible = True
            debugData.Append("<br />***PAGE PROPERTIES***<br />")
            debugData.Append("OrderGuid: " & OrderGuid & "<br />")
            debugData.Append("OrderID: " & OrderID & "<br />")
            debugData.Append("DistributionId: " & DistributionId & "<br />")
            debugData.Append("USelectID: " & USelectID & "<br />")
            debugData.Append("EDDMMap: " & EDDMMap & "<br />")
            debugData.Append("GeneratedAddressedList: " & GeneratedAddressedList & "<br />")
            debugData.Append("UploadedAddressedList: " & UploadedAddressedList & "<br />")
            debugData.Append("TMCMap: " & TMCMap & "<br />")
            debugData.Append("NewMoverSelected: " & NewMoverSelected & "<br />")
            debugData.Append("TargetedEmailsSelected: " & TargetedEmailsSelected & "<br />")
            debugData.Append("DropFee: " & DropFee & "<br />")
            debugData.Append("IsProfessionalDesign: " & IsProfessionalDesign & "<br />")
            debugData.Append("DesignFee: " & DesignFee & "<br />")
            debugData.Append("IsMultipleImpression: " & IsMultipleImpression & "<br />")
            debugData.Append("MailPieces: " & MailPieces & "<br />")
            debugData.Append("NumberofDrops: " & NumberOfDrops & "<br />")
            debugData.Append("ExtraCopies: " & ExtraCopies & "<br />")

            lblDebug.Text = debugData.ToString()

            litCart.Text = oCart.OuterXml

        End If

    End Sub



End Class
