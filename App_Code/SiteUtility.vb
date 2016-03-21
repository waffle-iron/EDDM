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

Public Class SiteUtility


    '=================================================================================================================================================
    ' DATA DEFINITIONS:
    '   HideTaradelContent:             Flag used to show/hide certain page elements throughout site containing references to Taradel. This is used 
    '                                   on pages such as account_manage.aspx (hides Taradel Newsletter checkbox), vpage_template.aspx (determines
    '                                   if panel referencing Taradel graphic designer is shown), and the TemplateBrowser control.
    '
    '   TestMode:                       When set to True in database, it will cause core pages to set debug related data test panels to become visible
    '                                   to the user.
    '
    '   UseBoldChatVMScript             Boolean flag to tell [BoldChatVisitorMonitor.ascx] control whether to look for and inject a BoldChat script
    '                                   or not.
    '
    '   EnableHubSpot                   Boolean used on Step3-Checkout, Step1-TargetReview, and Account_SignIn.  Determines whether we should send 
    '                                   "lead" data to Hubspot or not for the given site.
    '
    '   CampaignOverviewDisplayDelay    When this is set to True, the Campaign Overview will be hidden **until** the user clicks Continue to Delivery
    '                                   options.
    '
    '   EnableHubSpot                   When enabled, this will send lead data to Hubspot from the account_signin.aspx, Step1-TargetReview.aspx, and
    '                                   Step3-Checkout.aspx pages. This is disabled in DEV for these three pages.  
    '
    '   UseReceiptPDFBarCodes           When enabled, allows the ReceiptPDF page to display bar code images.  Currently used on some Staples sites.
    '
    '   MinOrderQty                     Value which must be met to allow user to proceed with order. 
    '
    '   MinEDDMPricingQty               Value of the Min Qty an EDDM order/quote will be priced at.  Usually 1000.
    '
    '   MinAddressedPricingQty          Value of the Min Qty an Addressed List order/quote will be priced at.  Usually 1000.
    '
    '   ValidateExtraCopiesAddress      When this is set to TRUE, it will require the user to select a Delivery Address on Step2-ProductOptions
    '                                   instead of defaulting to an address.  
    '=================================================================================================================================================




    Private Shared Log As ILog = LogManager.GetLogger(Reflection.MethodBase.GetCurrentMethod().DeclaringType())



    Public Shared Function RetrieveSiteSettings(SiteID As Integer) As SiteDetails

        Dim siteDetails As SiteDetails = New SiteDetails()

        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)

                oConnA.Open()

                Dim sSql As String = "EXEC usp_GetSiteDetails @paramSiteID = " & SiteID.ToString()

                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.Text
                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(oCmdA)
                    Dim ds As DataSet = New DataSet()
                    adapter.Fill(ds)
                    Dim dtSite As DataTable = ds.Tables(0)

                    For Each dr As DataRow In dtSite.Rows
                        siteDetails.SiteDescription = dr("SiteDescription").ToString()
                        siteDetails.UseGoogleAnalytics = dr("UseGoogleAnalytics").ToString()
                        siteDetails.UseBoldChatVMScript = dr("UseBoldChatVMScript").ToString()
                        siteDetails.RequirePONumber = dr("RequirePONumber").ToString()
                        siteDetails.SupportEmail = dr("SupportEmail").ToString()
                        siteDetails.SalesEmail = dr("SalesEmail").ToString()
                        siteDetails.AdminEmail = dr("AdminEmail").ToString()
                        siteDetails.SysFromEmailAcct = dr("SysFromEmailAcct").ToString()
                        siteDetails.BCCOrderEmails = dr("BCCOrderEmails").ToString()
                        siteDetails.SupportPhone = dr("SupportPhone").ToString()
                        siteDetails.AllowGetQuote = Convert.ToBoolean(dr("AllowGetQuote"))
                        siteDetails.ShowReceiptEmail = Convert.ToBoolean(dr("ShowReceiptEmail"))
                        siteDetails.ShowChat = Convert.ToBoolean(dr("ShowChat"))
                        siteDetails.SelectRequireProof = Convert.ToBoolean(dr("SelectRequireProof"))
                        siteDetails.ShowFaq = Convert.ToBoolean(dr("ShowFaq"))
                        siteDetails.DefaultMultiImpressions = Convert.ToInt32(dr("DefaultMultiImpressions"))
                        siteDetails.SendCustShipEmail = Convert.ToBoolean(dr("SendCustShipEmail"))
                        siteDetails.SendCustPOEmail = Convert.ToBoolean(dr("SendCustPOEmail"))
                        siteDetails.HideTaradelContent = Convert.ToBoolean(dr("HideTaradelContent"))
                        siteDetails.ShowBillingInfo = Convert.ToBoolean(dr("ShowBillingInfo"))
                        siteDetails.PrefillBillingInfo = Convert.ToBoolean(dr("PrefillBillingInfo"))
                        siteDetails.ShowForgotPWDLink = Convert.ToBoolean(dr("ShowForgotPWDLink"))
                        siteDetails.ShowRegisterModal = Convert.ToBoolean(dr("ShowRegisterModal"))
                        siteDetails.ShowBillingEmail = Convert.ToBoolean(dr("ShowBillingEmail"))
                        siteDetails.CCAccountHolderOnReceipt = Convert.ToBoolean(dr("CCAccountHolderOnReceipt"))
                        siteDetails.ShowReceiptPaymentInfo = Convert.ToBoolean(dr("ShowReceiptPaymentInfo"))
                        siteDetails.EnableHubSpot = Convert.ToBoolean(dr("EnableHubSpot"))
                        siteDetails.UseCoBrandLogo = Convert.ToBoolean(dr("UseCoBrandLogo"))
                        siteDetails.AllowSplitDrops = Convert.ToBoolean(dr("AllowSplitDrops"))
                        siteDetails.ShowReceiptTermsAndConditions = Convert.ToBoolean(dr("ShowReceiptTermsAndConditions"))
                        siteDetails.AllowEDDMProducts = Convert.ToBoolean(dr("AllowEDDMProducts"))
                        siteDetails.AllowListProducts = Convert.ToBoolean(dr("AllowListProducts"))
                        siteDetails.Auth_AuthorizeAPIUrl = dr("AuthNet_AuthorizeAPIUrl").ToString()
                        siteDetails.Auth_CCMerchantId = dr("AuthNet_CCMerchantID").ToString()
                        siteDetails.Auth_MerchantId = dr("AuthNet_MerchantID").ToString()
                        siteDetails.Auth_SettlementTime = dr("AuthNet_SettlementTime").ToString()
                        siteDetails.Auth_TestMode = dr("AuthNet_TestMode").ToString()
                        siteDetails.Auth_TransactionId = dr("AuthNet_TransactionID").ToString()
                        siteDetails.Auth_CCTransactionId = dr("AuthNet_CCTransactionID").ToString()
                        siteDetails.Auth_TransactionLog = dr("AuthNet_TransactionLog").ToString()
                        siteDetails.HideSplitDrops = Convert.ToBoolean(dr("HideSplitDrops"))
                        siteDetails.OffersExclusiveRoutes = Convert.ToBoolean(dr("OffersExclusiveRoutes"))
                        siteDetails.NumImpressionsForExclusive = Convert.ToInt32(dr("NumImpressionsForExclusive"))
                        siteDetails.MinQtyForExclusive = Convert.ToInt32(dr("MinQtyForExclusive"))
                        siteDetails.NumDaysExclusiveExpires = Convert.ToInt32(dr("NumDaysExclusiveExpires"))
                        siteDetails.MaxImpressions = Convert.ToInt32(dr("MaxImpressions"))
                        siteDetails.MinOrderQty = Convert.ToInt32(dr("MinOrderQty"))
                        siteDetails.MinEDDMPricingQty = Convert.ToInt32(dr("MinEDDMPricingQty"))
                        siteDetails.MinAddressedPricingQty = Convert.ToInt32(dr("MinAddressedPricingQty"))
                        siteDetails.RequiredDownPymtPercentage = Convert.ToInt32(dr("RequiredDownPymtPercentage"))
                        siteDetails.NumDaysPymntDuePriorToDrop = Convert.ToInt32(dr("NumDaysPymntDuePriorToDrop"))
                        siteDetails.UseRibbonBanners = Convert.ToBoolean(dr("UseRibbonBanners"))
                        siteDetails.Step1TargetReviewRedirectIfSingleProduct = Convert.ToInt32(dr("Step1TargetReviewRedirectIfSingleProduct"))
                        siteDetails.TestMode = Convert.ToBoolean(dr("TestMode"))
                        siteDetails.CampaignOverviewDisplayDelay = Convert.ToBoolean(dr("CampaignOverviewDisplayDelay"))
                        siteDetails.UseReceiptPDFBarCodes = Convert.ToBoolean(dr("UseReceiptPDFBarCodes"))
                        siteDetails.ValidateExtraCopiesAddress = Convert.ToBoolean(dr("ValidateExtraCopiesAddress"))
                        siteDetails.StatusMsg = "OK"

                    Next
                End Using
            End Using

        Catch ex As Exception

            Dim myLogWriter As New LogWriter()
            myLogWriter.RecordInLog("Error in RetrieveSiteSettings: " & ex.StackTrace.ToString())

            'PreExisting Log Tool
            Log.Error("RetrieveSiteSettings: " & ex.StackTrace.ToString(), ex)

            siteDetails.StatusMsg = "Error: " & ex.StackTrace.ToString()

        End Try


        Return siteDetails


    End Function



    Public Shared Function UpdateSiteSettings(SiteID As Integer, TestMode As Boolean, ShowChat As Boolean, ShowFAQ As Boolean, HideTaradel As Boolean, ShowPWDLink As Boolean, _
                                            ShowRegisterModal As Boolean, UseCoBrandLogo As Boolean, UseRibbponBanners As Boolean, salesEmail As String, supportEmail As String, _
                                            adminEmail As String, systemEmail As String, bccOrders As String, sendCustShipEmail As Boolean, sendCustPOEmail As Boolean, _
                                            ccAccountHolderOnReceipt As Boolean, supportPhone As String, useBoldChatVM As Boolean, useGoogleAnalytics As Boolean, _
                                            allowEDDMProducts As Boolean, allowListProducts As Boolean, offersExclusiveRoutes As Boolean, numImpressionsForExclusive As Integer, _
                                            minQtyForExclusive As Integer, numDaysExclusiveExpires As Integer, step1TargetReviewRedirectIfSingleProduct As Boolean, allowSplitDrops As Boolean, _
                                            campaignOverviewDisplayDelay As Boolean, hideSplitDrops As Boolean, preselectRequireProof As Boolean, maxImpressions As Integer, _
                                            minOrderQty As Integer, minEDDMPricingQty As Integer, minAddressedPricingQty As Integer, defaultNumImpressions As Integer, 
                                            validateExtraCopiesAddress As Boolean, allowGetQuote As Boolean, prefillBillingInfo As Boolean, requirePONumber As Boolean, _ 
                                            showBillingEmail As Boolean, showBillingInfo As Boolean, showReceiptEmail As Boolean, numDaysPymntDuePriorToDrop As Integer, _ 
                                            requiredDownPymtPercentage As Integer, showReceiptPaymentInfo As Boolean, showReceiptTerms As Boolean, useReceiptBarCodes As Boolean, _ 
                                            authAuthorizeAPIUrl As String, authCCMerchantId As String, authMerchantId As String, authSettlementTime As String, authTestMode As Boolean, _ 
                                            authTransactionId As String, authCCTransactionId As String, authTransactionLog As String, enableHubSpot As Boolean) As Integer
        '57

        Dim results As Integer = 0
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim updateSql As StringBuilder = New StringBuilder()
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)

        '57
        updateSql.Append("EXEC usp_UpdateSiteDetails ")
        updateSql.Append("@paramSiteID = " & SiteID & ", ")
        updateSql.Append("@paramTestMode = " & Convert.ToInt16(TestMode) & ", ")
        updateSql.Append("@paramShowChat = " & Convert.ToInt16(ShowChat) & ", ")
        updateSql.Append("@paramShowFAQ = " & Convert.ToInt16(ShowFAQ) & ", ")
        updateSql.Append("@paramHideTaradel = " & Convert.ToInt16(HideTaradel) & ", ")
        updateSql.Append("@paramShowPWDLink = " & Convert.ToInt16(ShowPWDLink) & ", ")
        updateSql.Append("@paramShowRegisterModal = " & Convert.ToInt16(ShowRegisterModal) & ", ")
        updateSql.Append("@paramUseCoBrandLogo = " & Convert.ToInt16(UseCoBrandLogo) & ", ")
        updateSql.Append("@paramUseRibbonBanners = " & Convert.ToInt16(UseRibbponBanners) & ", ")
        updateSql.Append("@paramSalesEmail = '" & salesEmail & "', ")
        updateSql.Append("@paramSupportEmail = '" & supportEmail & "', ")
        updateSql.Append("@paramAdminEmail = '" & adminEmail & "', ")
        updateSql.Append("@paramSystemEmail = '" & systemEmail & "', ")
        updateSql.Append("@paramBCCOrders = '" & bccOrders & "', ")
        updateSql.Append("@paramSendCustShipEmail = " & sendCustShipEmail & ", ")
        updateSql.Append("@paramSendCustPOEmail = " & sendCustPOEmail & ", ")
        updateSql.Append("@paramCCAccountHolderOnReceipt = " & ccAccountHolderOnReceipt & ", ")
        updateSql.Append("@paramSupportPhone = '" & supportPhone & "', ")
        updateSql.Append("@paramUseBoldChatVM = " & useBoldChatVM & ", ")
        updateSql.Append("@paramUseGoogleAnalytics = " & useGoogleAnalytics & ", ")
        updateSql.Append("@paramAllowEDDMProducts = " & allowEDDMProducts & ", ")
        updateSql.Append("@paramAllowListProducts = " & allowListProducts & ", ")
        updateSql.Append("@paramOffersExclusiveRoutes = " & offersExclusiveRoutes & ", ")
        updateSql.Append("@paramNumImpressionsForExclusive = " & numImpressionsForExclusive & ", ")
        updateSql.Append("@paramMinQtyForExclusive = " & minQtyForExclusive & ", ")
        updateSql.Append("@paramNumDaysExclusiveExpires = " & numDaysExclusiveExpires & ", ")
        updateSql.Append("@paramStep1TargetReviewRedirectIfSingleProduct = " & step1TargetReviewRedirectIfSingleProduct & ", ")
        updateSql.Append("@paramAllowSplitDrops = " & allowSplitDrops & ", ")
        updateSql.Append("@paramCampaignOverviewDisplayDelay = " & campaignOverviewDisplayDelay & ", ")
        updateSql.Append("@paramHideSplitDrops = " & hideSplitDrops & ", ")
        updateSql.Append("@paramPreselectRequireProof = " & preselectRequireProof & ", ")
        updateSql.Append("@paramMaxImpressions = " & maxImpressions & ", ")
        updateSql.Append("@paramMinOrderQty = " & minOrderQty & ", ")
        updateSql.Append("@paramMinEDDMPricingQty = " & minEDDMPricingQty & ", ")
        updateSql.Append("@paramMinAddressedPricingQty = " & minAddressedPricingQty & ", ")
        updateSql.Append("@paramDefaultNumImpressions = " & defaultNumImpressions & ", ")
        updateSql.Append("@paramValidateExtraCopiesAddress = " & validateExtraCopiesAddress & ", ")
        updateSql.Append("@paramAllowGetQuote = " & allowGetQuote & ", ")
        updateSql.Append("@paramPrefillBillingInfo = " & prefillBillingInfo & ", ")
        updateSql.Append("@paramRequirePONumber = " & requirePONumber & ", ")
        updateSql.Append("@paramShowBillingEmail = " & showBillingEmail & ", ")
        updateSql.Append("@paramShowBillingInfo = " & showBillingInfo & ", ")
        updateSql.Append("@paramShowReceiptEmail = " & showReceiptEmail & ", ")
        updateSql.Append("@paramNumDaysPymntDuePriorToDrop = " & numDaysPymntDuePriorToDrop & ", ")
        updateSql.Append("@paramRequiredDownPymtPercentage = " & requiredDownPymtPercentage & ", ")
        updateSql.Append("@paramShowReceiptPaymentInfo = " & showReceiptPaymentInfo & ", ")
        updateSql.Append("@paramShowReceiptTerms = " & showReceiptTerms & ", ")
        updateSql.Append("@paramUseReceiptBarCodes = " & useReceiptBarCodes & ", ")
        updateSql.Append("@paramAuthAuthorizeAPIUrl = '" & authAuthorizeAPIUrl & "', ")
        updateSql.Append("@paramAuthCCMerchantId = '" & authCCMerchantId & "', ")
        updateSql.Append("@paramAuthMerchantId = '" & authMerchantId & "', ")
        updateSql.Append("@paramAuthSettlementTime = '" & authSettlementTime & "', ")
        updateSql.Append("@paramAuthTestMode = " & authTestMode & ", ")
        updateSql.Append("@paramAuthTransactionId = '" & authTransactionId & "', ")
        updateSql.Append("@paramAuthCCTransactionId = '" & authCCTransactionId & "', ")
        updateSql.Append("@paramAuthTransactionLog = '" & authTransactionLog & "', ")
        updateSql.Append("@paramEnableHubspot = " & enableHubSpot & "")



        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(updateSql.ToString(), connObj)

        Try

            connObj.Open()
            results = sqlCommand.ExecuteScalar()

        Catch objException As Exception

            Dim logWriter As New LogWriter()
            logWriter.RecordInLog("Error in UpdateSiteSettings: " & objException.StackTrace.ToString())
            logWriter.RecordInLog("Error in UpdateSiteSettings SQL: " & updateSql.ToString())

        Finally

            connObj.Close()

        End Try


        Return results

    End Function



    'To do
    Public Shared Function InsertSiteSettings(SiteID As Integer) As Integer

        Dim results As Integer = 0


        Return results

    End Function




    Public Shared Function GetStringResourceValue(siteID As Integer, stringName As String) As String

        Dim results As String = "undefined"
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim errorMsg As New StringBuilder()
        Dim selectSql As String = "EXEC usp_GetStringResourceValue @paramSiteID = " & siteID & ", @paramStringName = '" & stringName & "'"
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)
        Dim sqlReader As SqlDataReader

        Try

            connObj.Open()
            sqlReader = sqlCommand.ExecuteReader()

            If sqlReader.HasRows Then
                Do While sqlReader.Read()
                    results = sqlReader("Value")
                Loop
            End If

        Catch objException As Exception
            'Nothing yet

        Finally
            connObj.Close()
        End Try


        'Uh oh.  Looking for something that doesn't exist.
        If results = "undefined" Then
            EmailUtility.SendAdminEmail("Oops. A Site was looking for something in the StringResourceMgr that doesn't exist.  Site # <strong>" & siteID & "</strong> is looking for <strong>" & stringName & "</strong>.")
        End If


        Return results



    End Function




    Public Class SiteDetails


        Private _SiteDescription As String
        Public Property SiteDescription() As String
            Get
                Return _SiteDescription
            End Get
            Set(ByVal value As String)
                _SiteDescription = value
            End Set
        End Property


        Private _UseBoldChatVMScript As Boolean
        Public Property UseBoldChatVMScript() As Boolean
            Get
                Return _UseBoldChatVMScript
            End Get
            Set(ByVal value As Boolean)
                _UseBoldChatVMScript = value
            End Set
        End Property


        Private _UseGoogleAnalytics As Boolean
        Public Property UseGoogleAnalytics() As Boolean
            Get
                Return _UseGoogleAnalytics
            End Get
            Set(ByVal value As Boolean)
                _UseGoogleAnalytics = value
            End Set
        End Property


        Private _RequirePONumber As Boolean
        Public Property RequirePONumber() As Boolean
            Get
                Return _RequirePONumber
            End Get
            Set(ByVal value As Boolean)
                _RequirePONumber = value
            End Set
        End Property


        Private _SupportEmail As String
        Public Property SupportEmail() As String
            Get
                Return _SupportEmail
            End Get
            Set(ByVal value As String)
                _SupportEmail = value
            End Set
        End Property


        Private _SalesEmail As String
        Public Property SalesEmail() As String
            Get
                Return _SalesEmail
            End Get
            Set(ByVal value As String)
                _SalesEmail = value
            End Set
        End Property


        Private _AdminEmail As String
        Public Property AdminEmail() As String
            Get
                Return _AdminEmail
            End Get
            Set(ByVal value As String)
                _AdminEmail = value
            End Set
        End Property


        Private _SysFromEmailAcct As String
        Public Property SysFromEmailAcct() As String
            Get
                Return _SysFromEmailAcct
            End Get
            Set(ByVal value As String)
                _SysFromEmailAcct = value
            End Set
        End Property


        Private _BCCOrderEmails As String
        Public Property BCCOrderEmails() As String
            Get
                Return _BCCOrderEmails
            End Get
            Set(ByVal value As String)
                _BCCOrderEmails = value
            End Set
        End Property


        Private _SupportPhone As String
        Public Property SupportPhone() As String
            Get
                Return _SupportPhone
            End Get
            Set(ByVal value As String)
                _SupportPhone = value
            End Set
        End Property


        Private _AllowGetQuote As Boolean
        Public Property AllowGetQuote() As Boolean
            Get
                Return _AllowGetQuote
            End Get
            Set(ByVal value As Boolean)
                _AllowGetQuote = value
            End Set
        End Property


        Private _ShowReceiptEmail As Boolean
        Public Property ShowReceiptEmail() As Boolean
            Get
                Return _ShowReceiptEmail
            End Get
            Set(ByVal value As Boolean)
                _ShowReceiptEmail = value
            End Set
        End Property


        Private _ShowChat As Boolean
        Public Property ShowChat() As Boolean
            Get
                Return _ShowChat
            End Get
            Set(ByVal value As Boolean)
                _ShowChat = value
            End Set
        End Property


        Private _SelectRequireProof As Boolean
        Public Property SelectRequireProof() As Boolean
            Get
                Return _SelectRequireProof
            End Get
            Set(ByVal value As Boolean)
                _SelectRequireProof = value
            End Set
        End Property


        Private _ShowFaq As Boolean
        Public Property ShowFaq() As Boolean
            Get
                Return _ShowFaq
            End Get
            Set(ByVal value As Boolean)
                _ShowFaq = value
            End Set
        End Property


        Private _DefaultMultiImpressions As Integer
        Public Property DefaultMultiImpressions As Integer
            Get
                Return _DefaultMultiImpressions
            End Get
            Set(value As Integer)
                _DefaultMultiImpressions = value
            End Set
        End Property


        Private _SendCustShipEmail As Boolean
        Public Property SendCustShipEmail() As Boolean
            Get
                Return _SendCustShipEmail
            End Get
            Set(ByVal value As Boolean)
                _SendCustShipEmail = value
            End Set
        End Property


        Private _SendCustPOEmail As Boolean
        Public Property SendCustPOEmail() As Boolean
            Get
                Return _SendCustPOEmail
            End Get
            Set(ByVal value As Boolean)
                _SendCustPOEmail = value
            End Set
        End Property


        Private _HideTaradelContent As Boolean
        Public Property HideTaradelContent() As Boolean
            Get
                Return _HideTaradelContent
            End Get
            Set(ByVal value As Boolean)
                _HideTaradelContent = value
            End Set
        End Property


        Private _ShowBillingInfo As Boolean
        Public Property ShowBillingInfo() As Boolean
            Get
                Return _ShowBillingInfo
            End Get
            Set(ByVal value As Boolean)
                _ShowBillingInfo = value
            End Set
        End Property


        Private _PrefillBillingInfo As Boolean
        Public Property PrefillBillingInfo() As Boolean
            Get
                Return _PrefillBillingInfo
            End Get
            Set(ByVal value As Boolean)
                _PrefillBillingInfo = value
            End Set
        End Property


        Private _ShowForgotPWDLink As Boolean
        Public Property ShowForgotPWDLink() As Boolean
            Get
                Return _ShowForgotPWDLink
            End Get
            Set(ByVal value As Boolean)
                _ShowForgotPWDLink = value
            End Set
        End Property


        Private _ShowRegisterModal As Boolean
        Public Property ShowRegisterModal() As Boolean
            Get
                Return _ShowRegisterModal
            End Get
            Set(ByVal value As Boolean)
                _ShowRegisterModal = value
            End Set
        End Property


        Private _ShowBillingEmail As Boolean
        Public Property ShowBillingEmail() As Boolean
            Get
                Return _ShowBillingEmail
            End Get
            Set(ByVal value As Boolean)
                _ShowBillingEmail = value
            End Set
        End Property


        Private _CCAccountHolderOnReceipt As Boolean
        Public Property CCAccountHolderOnReceipt() As Boolean
            Get
                Return _CCAccountHolderOnReceipt
            End Get
            Set(ByVal value As Boolean)
                _CCAccountHolderOnReceipt = value
            End Set
        End Property


        Private _ShowReceiptPaymentInfo As Boolean
        Public Property ShowReceiptPaymentInfo() As Boolean
            Get
                Return _ShowReceiptPaymentInfo
            End Get
            Set(ByVal value As Boolean)
                _ShowReceiptPaymentInfo = value
            End Set
        End Property


        Private _ShowReceiptTermsAndConditions As Boolean
        Public Property ShowReceiptTermsAndConditions() As Boolean
            Get
                Return _ShowReceiptTermsAndConditions
            End Get
            Set(ByVal value As Boolean)
                _ShowReceiptTermsAndConditions = value
            End Set
        End Property


        Private _EnableHubSpot As Boolean
        Public Property EnableHubSpot() As Boolean
            Get
                Return _EnableHubSpot
            End Get
            Set(ByVal value As Boolean)
                _EnableHubSpot = value
            End Set
        End Property


        Private _UseCoBrandLogo As Boolean
        Public Property UseCoBrandLogo() As Boolean
            Get
                Return _UseCoBrandLogo
            End Get
            Set(ByVal value As Boolean)
                _UseCoBrandLogo = value
            End Set
        End Property


        Private _AllowSplitDrops As Boolean
        Public Property AllowSplitDrops() As Boolean
            Get
                Return _AllowSplitDrops
            End Get
            Set(ByVal value As Boolean)
                _AllowSplitDrops = value
            End Set
        End Property


        Private _AllowEDDMProducts As Boolean
        Public Property AllowEDDMProducts() As Boolean
            Get
                Return _AllowEDDMProducts
            End Get
            Set(ByVal value As Boolean)
                _AllowEDDMProducts = value
            End Set
        End Property


        Private _AllowListProducts As Boolean
        Public Property AllowListProducts() As Boolean
            Get
                Return _AllowListProducts
            End Get
            Set(ByVal value As Boolean)
                _AllowListProducts = value
            End Set
        End Property


        'TO DO: Convert to Boolean
        Private _AuthTestMode As String
        Public Property Auth_TestMode() As String
            Get
                Return _AuthTestMode
            End Get
            Set(ByVal value As String)
                _AuthTestMode = value
            End Set
        End Property


        Private _MerchantId As String
        Public Property Auth_MerchantId() As String
            Get
                Return _MerchantId
            End Get
            Set(ByVal value As String)
                _MerchantId = value
            End Set
        End Property


        Private _TransactionId As String
        Public Property Auth_TransactionId() As String
            Get
                Return _TransactionId
            End Get
            Set(ByVal value As String)
                _TransactionId = value
            End Set
        End Property


        Private _CCMerchantId As String
        Public Property Auth_CCMerchantId() As String
            Get
                Return _CCMerchantId
            End Get
            Set(ByVal value As String)
                _CCMerchantId = value
            End Set
        End Property


        'Intentionally not set.???
        Private _CCTransactionId As String
        Public Property Auth_CCTransactionId() As String
            Get
                Return _CCTransactionId
            End Get
            Set(ByVal value As String)
                _CCTransactionId = value
            End Set
        End Property


        Private _TransactionLog As String
        Public Property Auth_TransactionLog() As String
            Get
                Return _TransactionLog
            End Get
            Set(ByVal value As String)
                _TransactionLog = value
            End Set
        End Property


        Private _SettlementTime As String
        Public Property Auth_SettlementTime() As String
            Get
                Return _SettlementTime
            End Get
            Set(ByVal value As String)
                _SettlementTime = value
            End Set
        End Property


        Private _AuthorizeAPIUrl As String
        Public Property Auth_AuthorizeAPIUrl() As String
            Get
                Return _AuthorizeAPIUrl
            End Get
            Set(ByVal value As String)
                _AuthorizeAPIUrl = value
            End Set
        End Property


        ''' <summary>
        ''' Order Process  
        ''' Hide/Show Ability to Divide Order into Drops
        ''' </summary>
        Private _HideSplitDrops As Boolean
        Public Property HideSplitDrops() As Boolean
            Get
                Return _HideSplitDrops
            End Get
            Set(ByVal value As Boolean)
                _HideSplitDrops = value
            End Set
        End Property


        Private _OffersExclusiveRoutes As Boolean
        ''' <summary>
        ''' Order Process
        ''' Lock Routes for Site Determined Time
        ''' </summary>
        Public Property OffersExclusiveRoutes() As Boolean
            Get
                Return _OffersExclusiveRoutes
            End Get
            Set(ByVal value As Boolean)
                _OffersExclusiveRoutes = value
            End Set
        End Property


        Private _NumImpressionsForExclusive As Integer
        Public Property NumImpressionsForExclusive() As Integer
            Get
                Return _NumImpressionsForExclusive
            End Get
            Set(value As Integer)
                _NumImpressionsForExclusive = value
            End Set
        End Property


        Private _MinQtyForExclusive As Integer
        Public Property MinQtyForExclusive() As Integer
            Get
                Return _MinQtyForExclusive
            End Get
            Set(value As Integer)
                _MinQtyForExclusive = value
            End Set
        End Property


        Private _NumDaysExclusiveExpires As Integer
        Public Property NumDaysExclusiveExpires() As Integer
            Get
                Return _NumDaysExclusiveExpires
            End Get
            Set(value As Integer)
                _NumDaysExclusiveExpires = value
            End Set
        End Property


        Private _MaxImpressions As Integer
        Public Property MaxImpressions() As Integer
            Get
                Return _MaxImpressions
            End Get
            Set(value As Integer)
                _MaxImpressions = value
            End Set
        End Property


        Private _MinOrderQty As Integer
        Public Property MinOrderQty() As Integer
            Get
                Return _MinOrderQty
            End Get
            Set(value As Integer)
                _MinOrderQty = value
            End Set
        End Property


        Private _MinEDDMPricingQty As Integer
        Public Property MinEDDMPricingQty() As Integer
            Get
                Return _MinEDDMPricingQty
            End Get
            Set(value As Integer)
                _MinEDDMPricingQty = value
            End Set
        End Property


        Private _MinAddressedPricingQty As Integer
        Public Property MinAddressedPricingQty() As Integer
            Get
                Return _MinAddressedPricingQty
            End Get
            Set(value As Integer)
                _MinAddressedPricingQty = value
            End Set
        End Property


        Private _RequiredDownPymtPercentage As Integer
        Public Property RequiredDownPymtPercentage() As Integer
            Get
                Return _RequiredDownPymtPercentage
            End Get
            Set(value As Integer)
                _RequiredDownPymtPercentage = value
            End Set
        End Property


        Private _NumDaysPymntDuePriorToDrop As Integer
        Public Property NumDaysPymntDuePriorToDrop() As Integer
            Get
                Return _NumDaysPymntDuePriorToDrop
            End Get
            Set(value As Integer)
                _NumDaysPymntDuePriorToDrop = value
            End Set
        End Property


        Private _UseRibbonBanners As Boolean
        Public Property UseRibbonBanners() As Boolean
            Get
                Return _UseRibbonBanners
            End Get
            Set(ByVal value As Boolean)
                _UseRibbonBanners = value
            End Set
        End Property


        Private _Step1TargetReviewRedirectIfSingleProduct As Boolean
        Public Property Step1TargetReviewRedirectIfSingleProduct() As Boolean
            Get
                Return _Step1TargetReviewRedirectIfSingleProduct
            End Get
            Set(ByVal value As Boolean)
                _Step1TargetReviewRedirectIfSingleProduct = value
            End Set
        End Property


        Private _TestMode As Boolean
        Public Property TestMode() As Boolean
            Get
                Return _TestMode
            End Get
            Set(ByVal value As Boolean)
                _TestMode = value
            End Set
        End Property


        Private _CampaignOverviewDisplayDelay As Boolean
        Public Property CampaignOverviewDisplayDelay() As Boolean
            Get
                Return _CampaignOverviewDisplayDelay
            End Get
            Set(ByVal value As Boolean)
                _CampaignOverviewDisplayDelay = value
            End Set
        End Property


        Private _UseReceiptPDFBarCodes As Boolean
        Public Property UseReceiptPDFBarCodes() As Boolean
            Get
                Return _UseReceiptPDFBarCodes
            End Get
            Set(ByVal value As Boolean)
                _UseReceiptPDFBarCodes = value
            End Set
        End Property


        Private _ValidateExtraCopiesAddress As Boolean
        Public Property ValidateExtraCopiesAddress() As Boolean
            Get
                Return _ValidateExtraCopiesAddress
            End Get
            Set(ByVal value As Boolean)
                _ValidateExtraCopiesAddress = value
            End Set
        End Property


        Private _StatusMsg As String
        Public Property StatusMsg() As String
            Get
                Return _StatusMsg
            End Get
            Set(ByVal value As String)
                _StatusMsg = value
            End Set
        End Property



    End Class





End Class