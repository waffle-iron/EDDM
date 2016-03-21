Imports Microsoft.VisualBasic
Imports System
Imports ICSharpCode.SharpZipLib.Zip
Imports System.IO
Imports System.Xml

Public Class apphelp
#Region "Audit"
    Public Shared Function GetAuditRow(ByVal sTable As String, ByVal sKeyField As String, ByVal sKey As Integer) As String
        Dim oSB As New StringBuilder
        Dim oA As New appxAuditTableAdapters.GetXMLDataTableAdapter
        Dim oT As appxAudit.GetXMLDataDataTable = oA.GetData(sTable, sKeyField, sKey)
        For Each oRow As appxAudit.GetXMLDataRow In oT.Rows
            oSB.Append(oRow.Item(0))
        Next
        oT.Dispose()
        oA.Dispose()
        Return oSB.ToString
    End Function

    Public Shared Sub AuditChange(ByVal sTable As String, ByVal sKeyField As String, ByVal sKey As Integer, ByVal sAction As String, ByVal sUser As String, ByVal sUserID As Integer)
        Dim sAuditData As String = apphelp.GetAuditRow(sTable, sKeyField, sKey)
        Using oA As New appxAuditTableAdapters.ChangeTableAdapter
            oA.GetData(sTable, sKeyField, sKey, sAction, sUser, sUserID, sAuditData)
        End Using
    End Sub

    Public Shared Sub AuditXMLChange(ByVal sDocPath As String, ByVal sAction As String, ByVal sUser As String, ByVal sUserID As Integer)
        Dim oXML As New XmlDocument
        oXML.Load(sDocPath)
        Dim sAuditData As String = oXML.OuterXml

        Dim sBase As String = HttpContext.Current.Server.MapPath("/")
        Dim sFile As String = sDocPath.ToLower.Replace(sBase.ToLower, "")
        Using oA As New appxAuditTableAdapters.ChangeTableAdapter
            oA.GetData(sFile, "XMLDoc", 0, sAction, sUser, sUserID, sAuditData)
        End Using
    End Sub

    Public Shared Sub AuditFileChange(ByVal sDocPath As String, ByVal sAction As String, ByVal sUser As String, ByVal sUserID As Integer)
        Dim sAuditData As String = ""
        Using oSR As StreamReader = File.OpenText(sDocPath)
            sAuditData = oSR.ReadToEnd()
        End Using

        Dim sBase As String = HttpContext.Current.Server.MapPath("/")
        Dim sFile As String = sDocPath.ToLower.Replace(sBase.ToLower, "")
        Using oA As New appxAuditTableAdapters.ChangeTableAdapter
            oA.GetData(sFile, "FileDoc", 0, sAction, sUser, sUserID, sAuditData)
        End Using
    End Sub
#End Region

#Region "Error Logging"
    Public Shared Function BuildErrString(ByVal ex As Exception, ByVal ctx As HttpContext) As String
        Dim oSErr As New StringBuilder
        Dim sReferrer As String = String.Empty
        If ctx.Request.ServerVariables("HTTP_REFERER") IsNot Nothing Then
            sReferrer = ctx.Request.ServerVariables("HTTP_REFERER")
        End If
        Dim sForm As String = String.Empty
        If ctx.Request.Form IsNot Nothing Then
            sForm = ctx.Request.Form.ToString
        End If
        Dim sQuery As String = ctx.Request.Url.PathAndQuery
        Dim sErrSource As String = ex.Source
        Dim sErrMsg As String = ex.Message
        Dim sErrTarget As String = ex.TargetSite.ToString
        Dim sErrStack As String = ex.StackTrace

        oSErr.AppendLine("<fieldset>")
        oSErr.AppendLine("<legend>Exception</legend>")
        oSErr.AppendLine("<b>Source:</b> " & sErrSource & "<br/>")
        oSErr.AppendLine("<b>Message:</b> " & sErrMsg & "<br/>")
        oSErr.AppendLine("<b>Target Site:</b> " & sErrTarget & "<br/>")
        oSErr.AppendLine("<b>Stacktrace:</b><br/>" & sErrStack)
        oSErr.AppendLine("</fieldset>")

        oSErr.AppendLine("<fieldset>")
        oSErr.AppendLine("<legend>Page</legend>")
        oSErr.AppendLine("<b>Request:</b> " & sQuery & "<br/>")
        oSErr.AppendLine("<b>Form:</b> " & sForm & "<br/>")
        oSErr.AppendLine("</fieldset>")

        oSErr.AppendLine("<fieldset>")
        oSErr.AppendLine("<legend>Visitor</legend>")
        oSErr.AppendLine("<b>Username: </b> " & ctx.User.Identity.Name & "<br/>")
        Dim sIP As String = ctx.Request.UserHostAddress
        oSErr.AppendLine("<b>IP Address: </b>" & sIP & "<br/>")
        oSErr.AppendFormat("<b>Geocode Information:</b> <a href=""http://api.hostip.info/get_html.php?ip={0}"" target=""_blank"">http://api.hostip.info/get_html.php?ip={0}</a><br/>", sIP)
        oSErr.AppendLine("<b>Host Name: </b>" & ctx.Request.UserHostName & "<br/>")
        oSErr.AppendLine("<b>Browser: </b>" & ctx.Request.Browser.Browser & "<br/>")
        oSErr.AppendLine("</fieldset>")

        Return oSErr.ToString
    End Function

    Public Shared Function LogErr(ByVal sUser As String, ByVal sURL As String, ByVal sErr As String) As Integer
        'TODO: Fix error logging
        'Dim iErr As Integer = 0
        'Using oA As New ErrorLoggerTableAdapters.TaradelErrorLogTableAdapter
        '    iErr = oA.LogErr(sUser, sURL, sErr)
        'End Using
        'oA.Dispose()
        'Return iErr
        Return 0
    End Function
#End Region

    Public Shared Function SplitCamelCase(ByVal source As String) As String()
        Return Regex.Split(source, "(?<!^)(?=[A-Z])")
    End Function

    Public Shared Function CamelCaseToTitle(ByVal source As String) As String
        Dim aSource() As String = SplitCamelCase(source)
        Return String.Join(" ", aSource)
    End Function

    Public Shared Function ExtractArchive(ByVal zipFilename As String, ByVal ExtractDir As String, Optional ByVal Password As String = "") As Boolean
        Try
            Dim Redo As Integer = 1
            Dim MyZipInputStream As ZipInputStream
            Dim MyFileStream As FileStream = Nothing
            MyZipInputStream = New ZipInputStream(New FileStream(zipFilename, _
              FileMode.Open, FileAccess.Read))
            If Not String.IsNullOrEmpty(Password) Then
                MyZipInputStream.Password = Password
            End If
            Dim MyZipEntry As ZipEntry = MyZipInputStream.GetNextEntry
            If Not Directory.Exists(ExtractDir) Then
                Directory.CreateDirectory(ExtractDir)
            End If
            While Not MyZipEntry Is Nothing
                If (MyZipEntry.IsDirectory) Then
                    Directory.CreateDirectory(ExtractDir & "\" & MyZipEntry.Name)
                Else
                    If Not Directory.Exists(ExtractDir & "\" & _
                    Path.GetDirectoryName(MyZipEntry.Name)) Then
                        Directory.CreateDirectory(ExtractDir & "\" & _
                        Path.GetDirectoryName(MyZipEntry.Name))
                    End If
                    MyFileStream = New FileStream(ExtractDir & "\" & _
                      MyZipEntry.Name, FileMode.OpenOrCreate, FileAccess.Write)
                    Dim count As Integer
                    Dim buffer(4096) As Byte
                    count = MyZipInputStream.Read(buffer, 0, 4096)
                    While count > 0
                        MyFileStream.Write(buffer, 0, count)
                        count = MyZipInputStream.Read(buffer, 0, 4096)
                    End While
                    MyFileStream.Close()
                End If
                Try
                    MyZipEntry = MyZipInputStream.GetNextEntry
                Catch ex As Exception
                    MyZipEntry = Nothing
                End Try
            End While
            If Not (MyZipInputStream Is Nothing) Then MyZipInputStream.Close()
            If Not (MyFileStream Is Nothing) Then MyFileStream.Close()
            MyZipInputStream.Dispose()
            MyFileStream.Dispose()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function RoundMoney(ByVal iDec As Decimal) As Decimal
        iDec = iDec * 100
        iDec = Math.Round(iDec)
        iDec = iDec / 100
        Return iDec
    End Function

    Public Shared Function AbbrString(ByVal inStr As String, ByVal iLen As Integer, Optional ByVal RemoveHtml As Boolean = False) As String
        Dim sStr As String = inStr
        If RemoveHtml Then
            sStr = stripHTML(sStr)
        End If
        If inStr.Length > iLen Then
            Return inStr.Substring(0, iLen)
        Else
            Return inStr
        End If
    End Function

    Public Shared Function stripHTML(ByVal strHTML As String) As String
        Dim reg As New System.Text.RegularExpressions.Regex("</?\w+((\s+\w+(\s*=\s*(?:""(.|\n)*?""|'(.|\n)*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegularExpressions.RegexOptions.Singleline)
        Return reg.Replace(strHTML, "").Replace("<", "").Trim
    End Function

    Public Shared Function AlphaList() As ArrayList
        Dim oAlpha As New ArrayList
        oAlpha.Add("A")
        oAlpha.Add("B")
        oAlpha.Add("C")
        oAlpha.Add("D")
        oAlpha.Add("E")
        oAlpha.Add("F")
        oAlpha.Add("G")
        oAlpha.Add("H")
        oAlpha.Add("I")
        oAlpha.Add("J")
        oAlpha.Add("K")
        oAlpha.Add("L")
        oAlpha.Add("M")
        oAlpha.Add("N")
        oAlpha.Add("O")
        oAlpha.Add("P")
        oAlpha.Add("Q")
        oAlpha.Add("R")
        oAlpha.Add("S")
        oAlpha.Add("T")
        oAlpha.Add("U")
        oAlpha.Add("V")
        oAlpha.Add("W")
        oAlpha.Add("X")
        oAlpha.Add("Y")
        oAlpha.Add("Z")
        Return oAlpha
    End Function

    Public Shared Function Iif(ByVal Exp As Boolean, ByVal TrueValue As String, ByVal FalseValue As String) As String
        If Exp Then
            Return TrueValue
        Else
            Return FalseValue
        End If
    End Function

    Public Shared Function DeepFindControl(ByVal Root As Control, ByVal Id As String) As Control
        If Root Is Nothing Then
            Return Nothing
        End If
        If Root.ID = Id Then
            Return Root
        End If

        Dim Ctl As Control
        For Each Ctl In Root.Controls
            Dim FoundCtl As Control = DeepFindControl(Ctl, Id)
            If Not (FoundCtl Is Nothing) Then
                Return FoundCtl
            End If
        Next Ctl
        Return Nothing
    End Function 'FindControlRecursive 

    Public Shared Function JSBless(ByVal str As String) As String
        str = str.Replace("'", "\'")
        str = str.Replace(ControlChars.Quote, "&quot;")
        str = str.Replace(ControlChars.CrLf, " ")
        Return str
    End Function

    Public Shared Function LastDayOfMonth(ByVal iMonth As Integer, Optional ByVal iYear As Integer = 0) As Integer
        If iYear = 0 Then iYear = DateTime.Now.Year

        Dim oTDate As New DateTime(iYear, iMonth, 1)
        oTDate = oTDate.AddMonths(1).AddDays(-1)
        Return oTDate.Day
    End Function

    Public Shared Function FirstDayOfWeek(ByVal dReference As DateTime) As DateTime
        Return dReference.AddDays(dReference.DayOfWeek * -1)
    End Function

    Public Shared Function FirstDayOfQuarter(ByVal dReference As DateTime) As DateTime
        Dim iMonth As Integer = dReference.Month
        Select Case iMonth
            Case 1, 2, 3
                iMonth = 1
            Case 4, 5, 6
                iMonth = 4
            Case 7, 8, 9
                iMonth = 7
            Case 10, 11, 12
                iMonth = 10
        End Select

        '-- This is the last day of the quarter
        Return New DateTime(dReference.Year, iMonth, 1)
    End Function

    Public Shared Function LastDayOfQuarter(ByVal dReference As DateTime) As DateTime
        Dim iMonth As Integer = dReference.Month
        Select Case iMonth
            Case 1, 2, 3
                iMonth = 3
            Case 4, 5, 6
                iMonth = 6
            Case 7, 8, 9
                iMonth = 9
            Case 10, 11, 12
                iMonth = 12
        End Select

        '-- This is the last day of the quarter
        Return New DateTime(dReference.Year, iMonth, LastDayOfMonth(iMonth, dReference.Year))
    End Function

    Public Shared Function DateDiff(ByVal d1 As DateTime, ByVal d2 As DateTime, ByVal Span As TimeSpanType) As Decimal
        Dim oTS As TimeSpan = d1.Subtract(d2)

        Select Case Span
            Case TimeSpanType.Seconds
                Return oTS.Seconds
            Case TimeSpanType.Minutes
                Return oTS.Minutes
            Case TimeSpanType.Hours
                Return oTS.Hours
            Case TimeSpanType.Days
                Return oTS.Days
            Case TimeSpanType.Weeks
                Return oTS.Days / 7
            Case TimeSpanType.Years
                Return d1.Year - d2.Year
            Case Else
                Return oTS.Seconds
        End Select
    End Function

    Public Enum TimeSpanType
        Seconds = 1
        Minutes = 2
        Hours = 3
        Days = 4
        Weeks = 5
        Months = 6
        Years = 7
    End Enum

    Public Shared Sub RegisterReferral(ByVal sQuery As String)
        If sQuery.StartsWith("?") Then
            sQuery = sQuery.Substring(1)
        End If
        Dim oQuery() As String = sQuery.Split("&")
        Dim sLnk As String = ""
        For i As Integer = 0 To oQuery.Length - 1
            Dim aQuery() As String = oQuery(i).Split("=")
            If aQuery(0).ToLower = "ref" Then
                sLnk = aQuery(1)
                Exit For
            End If
        Next

        If Not String.IsNullOrEmpty(sLnk) Then
            '-- Register this visit as a referral and set the cookie
            Dim sIPAddress As String = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If String.IsNullOrEmpty(sIPAddress) Then
                sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
            End If
            Dim sBrowser As String = HttpContext.Current.Request.Browser.Browser & " " & HttpContext.Current.Request.Browser.Version

            Dim iRef As Integer = 0

            sLnk = Regex.Replace(sLnk, "[^0-9]", "")

            'Using oRefA As New appxReferralTableAdapters.ReferralTableAdapter
            '    oRefA.
            'End Using
            'iRef = oRefA.RegisterReferral(sLnk, sIPAddress, sBrowser)

            If iRef <> 0 Then
                '-- Create the cookie
                Dim oRefCookie As HttpCookie
                oRefCookie = New HttpCookie("Referral")
                oRefCookie.Domain = HttpContext.Current.Request.Url.Host
                oRefCookie.Value = iRef
                oRefCookie.Expires = System.DateTime.Now.AddDays(14)
                HttpContext.Current.Response.Cookies.Add(oRefCookie)
            End If
        End If
    End Sub

    Public Shared Function GetRequestedURL() As String
        Dim sQuery As String = HttpContext.Current.Request.Url.Query
        If sQuery.StartsWith("?") Then
            sQuery = sQuery.Substring(1)
        End If

        Dim sURL As String = ""
        '-- What type of page are we dealing with here?
        If sQuery.StartsWith("404;") Then
            sURL = sQuery.Replace("404;", "")
        ElseIf sQuery.Contains("aspxerrorpath") Then
            sURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Scheme) & HttpContext.Current.Request.Url.Host & HttpContext.Current.Request.QueryString("aspxerrorpath")
        Else
            sURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Scheme) & HttpContext.Current.Request.Url.Host & HttpContext.Current.Request.Path
        End If

        Try
            Dim oURI As New Uri(sURL)
            sURL = oURI.PathAndQuery
            Return sURL
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function RebuildQuerystring(ByRef sURL As String, Optional ByVal bRemoveQuerystring As Boolean = False) As Hashtable
        Dim sQuery As String = ""
        Dim aQuery() As String = Nothing
        Dim dQuery As New Hashtable
        If sURL.Contains("?") Then
            sQuery = sURL.Substring(sURL.IndexOf("?") + 1)
        End If
        If Not String.IsNullOrEmpty(sQuery) Then
            sQuery = HttpContext.Current.Server.UrlDecode(sQuery)
            aQuery = sQuery.Split("&")

            For iQ As Integer = 0 To aQuery.Length - 1
                Dim sPair As String = aQuery(iQ)
                Dim aPair() As String = sPair.Split("=")
                Dim sKey As String = aPair(0)
                Dim sVal As String = ""
                If aPair.Length > 1 Then
                    sVal = aPair(1)
                End If

                If Not dQuery.ContainsKey(sKey) Then
                    dQuery.Add(sKey, sVal)
                Else
                    dQuery(sKey) = dQuery(sKey) & ", " & sVal
                End If
            Next
            If bRemoveQuerystring Then
                sURL = sURL.Substring(0, sURL.IndexOf("?"))
            End If
        End If
        
        Return dQuery
    End Function
End Class
