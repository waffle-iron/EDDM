
Partial Class CCustom_BingConversionCode
    Inherits System.Web.UI.UserControl


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()

        If (currentMode <> "dev") Then
            pnlBingCode.Visible = True
        Else
            pnlBingCode.Visible = False
        End If

    End Sub


End Class


