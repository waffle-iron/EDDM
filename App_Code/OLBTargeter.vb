Imports Microsoft.VisualBasic

Public Class OLBTargeter


    Public Shared Function GetProductName(productID As Integer) As String

        '==========================================================================================
        'TO DO:
        ' Do NOT hard code values here.  Use legitmate database or Class calls to get Product info.
        '==========================================================================================

        Dim results As String = "unknown"

        'These are ProductIDs, not BaseProductIDs
        If (productID = 734) Then
            results = "4.25&quot; x 11&quot; EDDM™ Postcard"
        ElseIf (productID = 66) Then
            results = "6.25&quot; x 9&quot; EDDM™ Postcard"
        ElseIf (productID = 65) Then
            results = "8.5&quot; x 11&quot; EDDM™ Postcard"
        ElseIf ((productID = 953) Or (productID = 891)) Then
            results = "11&quot; x 15&quot; EDDM™ Flyer"
        End If

        Return results

    End Function



    Public Shared Function GetFrequencyString(frequency As Integer) As String

        Dim results As String = "unknown"

        If (frequency = 1) Then
            results = "Every Week"
        Else
            results = "Every " & frequency & " weeks"
        End If

        Return results

    End Function



    Public Shared Function StripBadChars(input As String) As String

        input = input.Replace("&quot;", "")
        input = input.Replace("&amp;quot;", "")
        input = input.Replace(",", "")

        Return input

    End Function


End Class
