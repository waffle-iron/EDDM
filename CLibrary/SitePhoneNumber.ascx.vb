
Partial Class CLibrary_SitePhoneNumber
    Inherits CLibraryBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim oSite As appxCMS.Site = appxCMS.SiteDataSource.GetSite
        If oSite IsNot Nothing Then
            Dim sPhone As String = "not configured"

            If Not String.IsNullOrEmpty(oSite.TollFreeNumber) Then
                sPhone = oSite.TollFreeNumber
            ElseIf Not String.IsNullOrEmpty(oSite.PhoneNumber) Then
                sPhone = oSite.PhoneNumber
            End If

            lPhone.Text = sPhone
        End If
    End Sub
End Class
