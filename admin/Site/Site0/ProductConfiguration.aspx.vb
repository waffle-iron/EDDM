Imports System.Collections.Generic

Partial Class admin_Site_Site0_ProductConfiguration
    Inherits adminBase

    Protected ReadOnly Property SiteId As Integer
        Get
            Return QStringToInt("siteid")
        End Get
    End Property


    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        Dim bUpsize As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "Upsize", Me.SiteId)
        Dim bMarketingUpSell As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "MarketingUpsell", Me.SiteId)
        Dim bNewMover As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "NewMover", Me.SiteId)
        Dim bAddressedListAddOns As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "AddressedListAddOns", Me.SiteId)
        Dim bEmail As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "Email", Me.SiteId)
        Dim bMultipleImpressions As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressions", Me.SiteId)
        Dim bMultipleImpressionsNoFee As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressionsNoFee", Me.SiteId)
        Dim bNewReceiptProcess As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "NewReceiptProcess", Me.SiteId)
        Dim bMailDropMonday As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "MailDropMonday", Me.SiteId)
        Dim bMailDropTuesday As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "MailDropTuesday", Me.SiteId)
        Dim bMailDropWednesday As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "MailDropWednesday", Me.SiteId)
        Dim bMailDropThursday As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "MailDropThursday", Me.SiteId)
        Dim bMailDropFriday As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "MailDropFriday", Me.SiteId)
        Dim bDisableSalesTax As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableSalesTax", Me.SiteId)
        Dim bDisableTemplates As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableTemplates", Me.SiteId)
        Dim bDisableProDesign As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableProDesign", Me.SiteId)
        Dim bDisableUploadArtwork As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Product", "DisableUploadArtwork", Me.SiteId)
        Dim bOffersExclusiveRoutes As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Route", "OffersExclusiveRoutes", Me.SiteId)
        Dim bUsesExclusiveTerritories As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Route", "UsesExclusiveTerritories", Me.SiteId)


        chkNewReceiptProcess.Checked = bNewReceiptProcess
        chkEnableUpsizing.Checked = bUpsize
        chkMarketingUpsell.Checked = bMarketingUpSell
        chkMultipleImpressions.Checked = bMultipleImpressions
        chkMultipleImpressionsNoFee.Checked = bMultipleImpressionsNoFee
        chkNewMover.Checked = bNewMover
        chkEmail.Checked = bEmail
        chkAddressedListAddOns.Checked = bAddressedListAddOns
        chkDisableSalesTax.Checked = bDisableSalesTax
        chkDisableTemplates.Checked = bDisableTemplates
        chkDisableProDesign.Checked = bDisableProDesign
        chkDisableArtwork.Checked = bDisableUploadArtwork

        chkExclusiveRoutes.Checked = bOffersExclusiveRoutes
        chkExclusiveTerritories.Checked = bUsesExclusiveTerritories

    End Sub

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click

        'Dim sMsg As String = ""
        'Dim bRet As Boolean = appxCMS.Util.CMSSettings.SaveSetting("Product", "Upsize", chkEnableUpsizing.Checked, Me.SiteId, sMsg)
        'If Not bRet Then
        '    lMsg.Text = UpdateStatusMsg("There was an error saving your setting: " & sMsg, True)
        'Else
        '    lMsg.Text = UpdateStatusMsg("Your changes have been saved")
        'End If

        Dim bErr As Boolean = False
        Dim oErr As New List(Of String)

        SaveSetting("Product", "Upsize", chkEnableUpsizing.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "MarketingUpsell", chkMarketingUpsell.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "NewMover", chkNewMover.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "Email", chkEmail.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "AddressedListAddOns", chkAddressedListAddOns.Checked.ToString(), oErr, bErr)

        SaveSetting("Product", "MultipleImpressions", chkMultipleImpressions.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "MultipleImpressionsNoFee", chkMultipleImpressionsNoFee.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "NewReceiptProcess", chkNewReceiptProcess.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "MailDropMonday", chkMondayDropDates.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "MailDropTuesday", chkTuesdayDropDates.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "MailDropWednesday", chkWednesdayDropDates.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "MailDropThursday", chkThursdayDropDates.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "MailDropFriday", chkFridayDropDates.Checked.ToString(), oErr, bErr)

        SaveSetting("Product", "DisableSalesTax", chkDisableSalesTax.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "DisableTemplates", chkDisableTemplates.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "DisableProDesign", chkDisableProDesign.Checked.ToString(), oErr, bErr)
        SaveSetting("Product", "DisableUploadArtwork", chkDisableArtwork.Checked.ToString(), oErr, bErr)

        SaveSetting("Route", "OffersExclusiveRoutes", chkExclusiveRoutes.Checked.ToString(), oErr, bErr)
        SaveSetting("Route", "UsesExclusiveTerritories", chkExclusiveTerritories.Checked.ToString(), oErr, bErr)


        If Not bErr Then
            lMsg.Text = UpdateStatusMsg("Your changes have been saved.")
        Else
            Dim oSb As New StringBuilder
            For Each s As String In oErr
                oSb.AppendLine("<p>" & s & "</p>")
            Next

            lMsg.Text = UpdateStatusMsg("There was a problem saving your changes:" & oSb.ToString, True)
        End If

    End Sub

    Protected Sub SaveSetting(sCat As String, sName As String, sVal As String, ByRef oErr As List(Of String), ByRef bErr As Boolean)
        Dim sMsg As String = ""
        Dim bRet As Boolean = appxCMS.Util.CMSSettings.SaveSetting(sCat, sName, sVal, Me.SiteId, sMsg)
        If Not bRet Then
            bErr = True
            oErr.Add(sMsg)
        End If
    End Sub
End Class
