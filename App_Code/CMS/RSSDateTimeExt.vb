Imports System
Imports System.Collections
Imports System.Text.RegularExpressions
Imports System.Diagnostics
Imports System.Xml

Namespace RssComponents

    Public Class DateTimeExt

        Private Shared regExpression As String = "\s*(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun)" & _
            "\s*,\s*)?(\d{1,2})\s+(Jan|Feb|Mar|Apr|May|Jun" & _
            "|Jul|Aug|Sep|Oct|Nov|Dec)\s+(\d{2,})\s+(\d{2})\s*:\s*(\d{2})\s*(?::\s*(\d{2}))?" & _
            "\s+([+\-]\d{4}|UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT|[A-IK-Z])"
        Private Shared rfc2822 As Regex = New Regex(regExpression, RegexOptions.Compiled)
        Private Shared months As New ArrayList(New String() _
            {"ZeroIndex", "Jan", "Feb", "Mar", "Apr", "May", "Jun", _
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"})

        'private static TimeSpan dayLightDelta = TimeZone.CurrentTimeZone.GetDaylightChanges(DateTime.Now.Year).Delta

        ''' <summary>
        ''' Converts an ISO 8601 date to a DateTime object. Helper method needed to 
        ''' deal with timezone offset since they are unsupported by the 
        ''' .NET Framework. 
        ''' </summary>
        ''' <param name="datetime">DateTime string</param>
        ''' <returns>DateTime instance</returns>
        ''' <exception cref="FormatException">On format errors parsing the datetime</exception>
        Public Shared Function ToDateTime(ByVal datetime As String) As DateTime

            Dim timeIndex As Integer = datetime.IndexOf(":")

            If timeIndex <> -1 Then

                Dim tzoneIndex As Integer = datetime.IndexOf("-", timeIndex)

                If tzoneIndex = -1 Then
                    tzoneIndex = datetime.IndexOf("+", timeIndex)
                    If tzoneIndex <> -1 Then 'timezone is ahead of UTC
                        Return AddOffset("+", datetime, tzoneIndex)

                    End If

                Else 'timezone is behind UTC

                    Return AddOffset("-", datetime, tzoneIndex)
                End If


            End If

            Return XmlConvert.ToDateTime(datetime, XmlDateTimeSerializationMode.Local)

        End Function

        ''' <summary>
        ''' Parse is able to parse RFC2822/RFC822 formatted dates.
        ''' It has a fallback mechanism: if the string does not match,
        ''' the normal DateTime.Parse() function is called.
        ''' </summary>
        ''' <param name="dateTimeString">DateTime String to parse</param>
        ''' <returns>DateTime instance</returns>
        ''' <exception cref="FormatException">On format errors parsing the datetime</exception>
        Public Shared Function Parse(ByVal dateTimeString As String) As DateTime

            If dateTimeString = Nothing Then
                Return DateTime.Now.ToUniversalTime()
            End If

            If dateTimeString.Trim().Length = 0 Then
                Return DateTime.Now.ToUniversalTime()
            End If

            Dim m As Match = rfc2822.Match(dateTimeString)
            If m.Success Then

                Try
                    Dim dd As Integer = Int32.Parse(m.Groups(1).Value)
                    Dim mth As Integer = months.IndexOf(m.Groups(2).Value)
                    Dim yy As Integer = Int32.Parse(m.Groups(3).Value)
                    ' following year completion is compliant with RFC 2822.
                    'yy = (yy < 50 ? 2000 + yy: (yy < 1000 ? 1900 + yy: yy))
                    Dim hh As Integer = Int32.Parse(m.Groups(4).Value)
                    Dim mm As Integer = Int32.Parse(m.Groups(5).Value)
                    Dim ss As Integer = Int32.Parse(m.Groups(6).Value)
                    Dim zone As String = m.Groups(7).Value

                    Dim xd As New DateTime(yy, mth, dd, hh, mm, ss)

                    Return xd.AddHours(RFCTimeZoneToGMTBias(zone) * -1)
                Catch ex As Exception

                    Throw New FormatException("RES_ExceptionRFC2822ParseGroupsMessage")
                End Try
            Else
                ' fallback, if regex does not match:
                Return DateTime.Parse(dateTimeString)

            End If
        End Function

        Private Sub New()
            'constructor is new to prevent creation, we just need the statics
        End Sub


        Private Structure TZB
            Public Sub New(ByVal z As String, ByVal b As Integer)
                Zone = z
                Bias = b
            End Sub

            Public Zone As String
            Public Bias As Integer
        End Structure

        Private Const timeZones As Integer = 34
        Private Shared ZoneBias As TZB() = New TZB(timeZones) { _
            New TZB("GMT", 0), New TZB("UT", 0), _
            New TZB("EST", -5 * 60), New TZB("EDT", -4 * 60), _
            New TZB("CST", -6 * 60), New TZB("CDT", -5 * 60), _
            New TZB("MST", -7 * 60), New TZB("MDT", -6 * 60), _
            New TZB("PST", -8 * 60), New TZB("PDT", -7 * 60), _
            New TZB("Z", 0), New TZB("A", -1 * 60), _
            New TZB("B", -2 * 60), New TZB("C", -3 * 60), _
            New TZB("D", -4 * 60), New TZB("E", -5 * 60), _
            New TZB("F", -6 * 60), New TZB("G", -7 * 60), _
            New TZB("H", -8 * 60), New TZB("I", -9 * 60), _
            New TZB("K", -10 * 60), New TZB("L", -11 * 60), _
            New TZB("M", -12 * 60), New TZB("N", 1 * 60), _
            New TZB("O", 2 * 60), New TZB("P", 3 * 60), _
            New TZB("Q", 4 * 60), New TZB("R", 3 * 60), _
            New TZB("S", 6 * 60), New TZB("T", 3 * 60), _
            New TZB("U", 8 * 60), New TZB("V", 3 * 60), _
            New TZB("W", 10 * 60), New TZB("X", 3 * 60), _
            New TZB("Y", 12 * 60)}

        Private Shared Function RFCTimeZoneToGMTBias(ByVal zone As String) As Double

            Dim s As String

            If zone.IndexOfAny(New Char() {"+"c, "-"c}) = 0 Then  ' +hhmm format

                Dim fact As Integer
                If zone.Substring(0, 1) = "-" Then
                    fact = -1
                Else
                    fact = 1
                End If

                s = zone.Substring(1).TrimEnd()
                Dim hh As Double = Math.Min(23, Int32.Parse(s.Substring(0, 2)))
                Dim mm As Double = Math.Min(59, Int32.Parse(s.Substring(2, 2))) / 60
                Return fact * (hh + mm)
            Else
                ' named format
                s = zone.ToUpper().Trim()
                For i As Integer = 0 To timeZones
                    If (ZoneBias(i).Zone.Equals(s)) Then
                        Return ZoneBias(i).Bias / 60
                    End If
                Next

            End If

            Return 0.0
        End Function

        Private Shared Function AddOffset(ByVal offsetOp As String, ByVal datetimeString As String, ByVal tzoneIndex As Integer) As DateTime

            Dim offset As String() = datetimeString.Substring(tzoneIndex + 1).Split(New Char() {":"c})
            Dim original As String = datetimeString
            datetimeString = datetimeString.Substring(0, tzoneIndex)

            If (datetimeString.IndexOf(":") = datetimeString.LastIndexOf(":")) Then 'check if seconds part is missing
                datetimeString = datetimeString + ":00"
            End If

            Dim toReturn As DateTime = XmlConvert.ToDateTime(datetimeString, XmlDateTimeSerializationMode.Local)

            Try
                Select Case offsetOp
                    Case "+"
                        toReturn = toReturn.Subtract(New TimeSpan(Int32.Parse(offset(0)), Int32.Parse(offset(1)), 0))
                    Case "-"
                        toReturn = toReturn.Add(New TimeSpan(Int32.Parse(offset(0)), Int32.Parse(offset(1)), 0))
                End Select

                Return toReturn '.ToLocalTime() 'we treat all dates in feeds as if they are local time (later)

            Catch iex As IndexOutOfRangeException

                Throw New FormatException("RES_ExceptionRFC2822InvalidTimezoneFormatMessage")
            End Try

        End Function

    End Class ' DateTimeExt
End Namespace