Imports Microsoft.VisualBasic

Public Class dateHelp
    Public Const cMinDate As DateTime = #1/1/1753#
    Public Const cMaxDate As DateTime = #12/31/9999#

    Public Enum TimeSpanType
        Seconds = 1
        Minutes = 2
        Hours = 3
        Days = 4
        Weeks = 5
        Months = 6
        Years = 7
    End Enum

    Public Shared Function LastDayOfMonth(ByVal iMonth As Integer, Optional ByVal iYear As Integer = 0) As Integer
        If iYear = 0 Then iYear = DateTime.Now.Year

        Dim oTDate As New DateTime(iYear, iMonth, 1)
        oTDate = oTDate.AddMonths(1).AddDays(-1)
        Return oTDate.Day
    End Function

    Public Shared Function LastDateOfMonth(ByVal dRef As DateTime) As DateTime
        Dim oTDate As New DateTime(dRef.Year, dRef.Month, 1)
        oTDate = oTDate.AddMonths(1).AddDays(-1)
        Return oTDate
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

    Public Shared Function ArticulateDate(ByVal dDate As DateTime) As String
        Dim oTs As TimeSpan = System.DateTime.Now.Subtract(dDate)
        Return ArticulateDate(oTs)
    End Function

    Public Shared Function ArticulateDate(ByVal oTs As TimeSpan) As String
        Return Cynosura.Articulate(oTs, Cynosura.TemporalGroupType.second)
    End Function

    Public Shared ReadOnly Property SqlMinDate() As DateTime
        Get
            Return New DateTime(1753, 1, 1, 0, 0, 0)
        End Get
    End Property

    Public Shared ReadOnly Property SqlMaxDate() As DateTime
        Get
            Return New DateTime(9999, 12, 31, 23, 59, 59)
        End Get
    End Property
End Class
