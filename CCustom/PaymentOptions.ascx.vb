
Partial Class CCustom_PaymentOptions
    Inherits CLibraryBase

    Private liClass As String = ""
    Protected Overrides Sub BuildControl()
        '-- What payment options are enabled for this site/user
        Dim bEnableCC As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Payment", "EnableCreditCard")
        '-- Wendy doesn't want this right now
        Dim bEnableCheck As Boolean = False 'appxCMS.Util.CMSSettings.GetBoolean("Payment", "EnableElectronicCheck")

        If Not bEnableCC And Not bEnableCheck Then
            pPaymentOptions.Visible = False
        Else
            pCreditCard.Visible = bEnableCC
            pECheck.Visible = bEnableCheck

            If bEnableCC AndAlso bEnableCheck Then
                liClass = "col-xs-2"
            ElseIf bEnableCC Then
                liClass = "col-xs-3"
            ElseIf bEnableCheck Then
                liClass = "col-xs-12"
            End If
        End If
    End Sub

    Protected Function GetLiClass() As String
        Return liClass
    End Function

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
