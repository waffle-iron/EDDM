Imports Microsoft.VisualBasic
Imports System.Text

Public Class textHelp
    Public Shared Function stripHTML(ByVal strHTML As String, ByVal allProperties As Boolean) As String
        If allProperties Then
            Dim reg As New System.Text.RegularExpressions.Regex("</?\w+((\s+\w+(\s*=\s*(?:""(.|\n)*?""|'(.|\n)*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegularExpressions.RegexOptions.Multiline)
            Dim oMatches As MatchCollection = reg.Matches(strHTML)
            Dim sStrOut As String = strHTML

            For Each oMatch As Match In oMatches
                sStrOut = sStrOut.Replace(oMatch.ToString(), " ")
            Next

            Dim regSpace As New Regex("\s+")
            Dim oSpaceMatches As MatchCollection = regSpace.Matches(sStrOut)
            For Each oSpaceMatch In oSpaceMatches
                sStrOut = sStrOut.Replace(oSpaceMatch.ToString(), " ")
            Next

            Return sStrOut.Trim()
        Else
            Return stripHTML(strHTML)
        End If
    End Function

    Public Shared Function StripHTML(ByVal strHTML As String) As String
        Dim reg As New System.Text.RegularExpressions.Regex("</?\w+((\s+\w+(\s*=\s*(?:""(.|\n)*?""|'(.|\n)*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegularExpressions.RegexOptions.Singleline)
        Return reg.Replace(strHTML, "").Replace("<", "").Trim
    End Function

    Public Shared Function StripHTML(ByVal strIn As String, ByVal aTags As ArrayList) As String
        Dim strOutput As String = strIn
        If Not String.IsNullOrEmpty(strOutput) Then
            If aTags Is Nothing Then
                strOutput = StripHTML(strIn)
                'strOutput = Regex.Replace(strOutput, "<[^>]*>", " ")
            Else
                For i As Integer = 0 To aTags.Count - 1
                    strOutput = Regex.Replace(strOutput, "</?" & aTags(i) & "[^>]*>", "")
                Next
            End If
        End If
        Return strOutput
    End Function

    Public Shared Function GetFirstParagraph(ByVal strIn As String) As String
        If Not String.IsNullOrEmpty(strIn) Then
            Dim aRemTags As New ArrayList
            aRemTags.Add("object")
            aRemTags.Add("embed")

            Dim strOutput As String = StripHTML(strIn, aRemTags)
            Dim iPreviewLength As Integer = 250
            If strOutput.Length < iPreviewLength Then
                iPreviewLength = strOutput.Length - 1
            End If
            strOutput = strOutput.Replace("<div>", "")
            strOutput = strOutput.Replace("<p>", "")
            strOutput = strOutput.Replace("</p>", "<br/><br/>")
            strOutput = strOutput.Replace("</div>", "<br/><br/>")
            strOutput = strOutput.Replace("<br />", "<br/>")
            Dim iFirstParaEnd As Integer = strOutput.IndexOf("<br/><br/>") + 10
            If iFirstParaEnd > 0 And iFirstParaEnd <= strOutput.Length Then
                strOutput = strOutput.Substring(0, iFirstParaEnd)
            End If
            Return strOutput
        Else
            Return ""
        End If
    End Function

End Class
