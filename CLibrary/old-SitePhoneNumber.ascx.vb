
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
                sPhone = FormatPhoneNumber(oSite.TollFreeNumber)
            ElseIf Not String.IsNullOrEmpty(oSite.PhoneNumber) Then
                sPhone = FormatPhoneNumber(oSite.PhoneNumber)
            End If

            lPhone.Text = sPhone
        End If

    End Sub



    Protected Function FormatPhoneNumber(PhoneNumber As String) As String

        Dim areaCode As String = ""
        Dim prefixDigits As String = ""
        Dim suffixDigits As String = ""
        Dim formattedNumber As String = ""

        If (String.IsNullOrEmpty(PhoneNumber)) Then
            formattedNumber = ""
        Else
            areaCode = PhoneNumber.Substring(0, 3)
            prefixDigits = PhoneNumber.Substring(3, 3)
            suffixDigits = PhoneNumber.Substring(6, 4)

            formattedNumber = areaCode & "-" & prefixDigits & "-" + suffixDigits
        End If


        Return formattedNumber

    End Function


End Class
