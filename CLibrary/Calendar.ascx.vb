Imports System.Data

Partial Class CLibrary_Calendar
    Inherits CLibraryBase

    Public Property CssClass() As String
        Get
            Return pCalendar.CssClass
        End Get
        Set(ByVal value As String)
            pCalendar.CssClass = value
        End Set
    End Property

    Private _Title As String = "Calendar of Events"
    <appX.cms(appX.cmsAttribute.DataValueType.Free)> _
    Public Property CalendarTitle() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = Server.UrlDecode(value)
        End Set
    End Property

    Private _CSSFile As String = "~/app_styles/Calendar.css"
    <appX.cms(appX.cmsAttribute.DataValueType.Free)> _
    Public Property CSSFile() As String
        Get
            Return _CSSFile
        End Get
        Set(ByVal value As String)
            _CSSFile = Server.UrlDecode(value)
        End Set
    End Property

    Private _Category As String = ""
    <appX.cms(appX.cmsAttribute.DataValueType.Free)> _
    Public Property Category() As String
        Get
            Return _Category
        End Get
        Set(ByVal value As String)
            _Category = value
        End Set
    End Property

    Private _DisplayDate As DateTime = System.DateTime.Now
    <appX.cms(appX.cmsAttribute.DataValueType.Free)> _
    Public Property DisplayDate() As String
        Get
            Return Me._DisplayDate.ToString("MM/dd/yyyy")
        End Get
        Set(ByVal value As String)
            Dim dDate As DateTime = _DisplayDate
            If DateTime.TryParse(value, dDate) Then
                _DisplayDate = dDate
            End If
        End Set
    End Property

    Private ReadOnly Property FirstDayOfMonth() As DateTime
        Get
            Return New DateTime(Me._DisplayDate.Year, Me._DisplayDate.Month, 1)
        End Get
    End Property

    Private ReadOnly Property LastDayOfMonth() As DateTime
        Get
            Dim dLastDayOfMonth As DateTime = Me.FirstDayOfMonth.AddMonths(1).AddDays(-1)
            Return New DateTime(dLastDayOfMonth.Year, dLastDayOfMonth.Month, dLastDayOfMonth.Day)
        End Get
    End Property

    Private dDateCells As New System.Collections.Generic.Dictionary(Of DateTime, String)
    Private sPage As String = ""
    Private dQuery As Hashtable = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        sPage = pageBase.GetRequestedURL(Page)
        dQuery = apphelp.RebuildQuerystring(sPage, True)
        If dQuery.ContainsKey("viewdate") Then
            Dim dViewDate As DateTime
            If DateTime.TryParse(dQuery("viewdate"), dViewDate) Then
                Me._DisplayDate = dViewDate
            End If
            dQuery.Remove("viewdate")
        End If

        jqueryHelper.Include(Page)
        jqueryHelper.IncludePlugin(Page, "appxCalendar", "~/scripts/jquery.appxCalendar.js")
        jqueryHelper.RegisterStylesheet(Page, Me.CSSFile)

        Dim oJs As New StringBuilder()
        oJs.AppendLine("jQuery(document).ready(function() {")
        oJs.AppendLine("    jQuery('#eventCal').appxCalendar();")
        oJs.AppendLine("    jQuery('div.appxEventInfoDialog').dialog({")
        oJs.AppendLine("        bgiframe:true,")
        oJs.AppendLine("        width:650,")
        oJs.AppendLine("        height:400,")
        oJs.AppendLine("        autoOpen:false")
        oJs.AppendLine("    });")

        Dim iRows As Integer = 0
        Using oDT As New DataTable("Calendar")
            oDT.Columns.Add(New DataColumn("Sunday", GetType(Integer)))
            oDT.Columns.Add(New DataColumn("Monday", GetType(Integer)))
            oDT.Columns.Add(New DataColumn("Tuesday", GetType(Integer)))
            oDT.Columns.Add(New DataColumn("Wednesday", GetType(Integer)))
            oDT.Columns.Add(New DataColumn("Thursday", GetType(Integer)))
            oDT.Columns.Add(New DataColumn("Friday", GetType(Integer)))
            oDT.Columns.Add(New DataColumn("Saturday", GetType(Integer)))

            '-- Get first day of Display Month
            Dim dLastMonth As DateTime = New DateTime(Me._DisplayDate.AddMonths(-1).Year, Me._DisplayDate.AddMonths(-1).Month, 1)
            Dim dNextMonth As DateTime = New DateTime(Me._DisplayDate.AddMonths(1).Year, Me._DisplayDate.AddMonths(1).Month, 1)
            Dim dFirstDay As DateTime = New DateTime(dLastMonth.AddMonths(1).Year, dLastMonth.AddMonths(1).Month, 1)
            Dim dLastDay As DateTime = dNextMonth.AddDays(-1)
            Dim oDR As DataRow = oDT.NewRow
            For i = 1 To dLastDay.Day
                Dim dCurDay As DateTime = New DateTime(Me._DisplayDate.Year, Me._DisplayDate.Month, i)
                oDR(dCurDay.DayOfWeek.ToString) = dCurDay.Day
                If dCurDay.DayOfWeek = DayOfWeek.Saturday Then
                    oDT.Rows.Add(oDR)
                    oDR = oDT.NewRow
                End If
            Next
            oDT.Rows.Add(oDR)
            iRows = oDT.Rows.Count

            lvCalendar.DataSource = oDT
            lvCalendar.DataBind()

            Dim lTitle As Literal = DirectCast(lvCalendar.FindControl("lTitle"), Literal)
            If lTitle IsNot Nothing Then
                lTitle.Text = Me.CalendarTitle
            End If

            '-- Get our events for the month and append them to the document
            Using oEventA As New appxCMSEventTableAdapters.EventTableAdapter
                Using oEventT As appxCMSEvent.EventDataTable = oEventA.GetCalendarEventsAllTypes(Me.FirstDayOfMonth, Me.LastDayOfMonth)
                    For Each oEvent As appxCMSEvent.EventRow In oEventT.Rows
                        Dim dEventStart As DateTime = oEvent.EventStartDate
                        Dim dEventEnd As DateTime = oEvent.EventEndDate
                        Dim sEventStartTime As String = ""
                        If dEventStart.Hour <> 0 Then
                            sEventStartTime = FormatEventTimeString(dEventStart) & ": "
                        End If
                        Dim sEventDisplayDate As String = FormatEventDisplayDate(dEventStart, dEventEnd)
                        For iAddDay As Integer = 0 To apphelp.DateDiff(dEventEnd, dEventStart, apphelp.TimeSpanType.Days)
                            Dim dEventDate As DateTime = dEventStart.AddDays(iAddDay)
                            If dEventDate > dEventEnd Or dEventDate > Me.LastDayOfMonth Then
                                Exit For
                            End If
                            Dim iEventID As Integer = oEvent.EventId
                            oJs.AppendLine("    jQuery('td[rel=" & dEventDate.Day & "]').append('<div class=""calendarEventTitle""><a href=""#eventlink" & oEvent.EventId & """ rel=""eventInfo" & oEvent.EventId & """ class=""eventInfoLink"" title=""" & apphelp.JSBless(oEvent.EventSummary) & """>" & apphelp.JSBless(sEventStartTime & oEvent.EventTitle) & "</a></div>');")
                        Next
                        phEventInfo.Controls.Add(New LiteralControl("<div id=""eventInfo" & oEvent.EventId & """ class=""appxEventInfoDialog"" title=""" & sEventDisplayDate & " " & oEvent.EventTitle & """><p>" & oEvent.EventSummary & "</p><div>" & oEvent.EventDesc & "</div><p><a href=""/resources/vEvent.ashx?id=" & oEvent.EventId & """>Save to your Calendar</a></p></div>"))
                    Next
                End Using
            End Using
        End Using

        Dim sMonth As String = Me._DisplayDate.ToString("MMMM")
        Dim lMonthName As Literal = DirectCast(lvCalendar.FindControl("lMonthName"), Literal)
        If lMonthName IsNot Nothing Then
            lMonthName.Text = sMonth & " " & Me._DisplayDate.Year
        End If

        Dim oLVI As ListViewItem = lvCalendar.Items(iRows - 1)
        If oLVI IsNot Nothing Then
            Dim weekRow As HtmlTableRow = DirectCast(oLVI.FindControl("weekRow"), HtmlTableRow)
            If weekRow IsNot Nothing Then
                Dim sClass As String = weekRow.Attributes("class")
                weekRow.Attributes.Remove("class")
                weekRow.Attributes.Add("class", (sClass & " lastweekofmonth").Trim)
            End If
        End If

        oJs.AppendLine("    jQuery('a.eventInfoLink').click(function() {")
        oJs.AppendLine("        jQuery('div#' + jQuery(this).attr('rel')).dialog('open');")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        Page.ClientScript.RegisterClientScriptBlock(GetType(String), "appxCalendarInit", oJs.ToString, True)
    End Sub

    Protected Sub lvCalendar_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvCalendar.DataBound
        Dim sJoin As String = "/?"
        Dim sQuery As String = ""
        Dim oEnum As IDictionaryEnumerator = dQuery.GetEnumerator
        Dim aQuery As New ArrayList
        While oEnum.MoveNext
            aQuery.Add(oEnum.Key & "=" & Server.UrlEncode(oEnum.Value))
        End While
        If aQuery.Count > 0 Then
            sQuery = String.Join("&", aQuery.ToArray(GetType(String)))
            sPage = sPage & "?" & sQuery
            sJoin = "&"
        End If

        Dim hplNextMonth As HyperLink = DirectCast(lvCalendar.FindControl("hplNextMonth"), HyperLink)
        If hplNextMonth IsNot Nothing Then
            hplNextMonth.Text = Me._DisplayDate.AddMonths(1).ToString("MMMM")
            hplNextMonth.NavigateUrl = sPage & sJoin & "viewdate=" & Server.UrlEncode(Me._DisplayDate.AddMonths(1).ToString("MM/dd/yyyy"))
        End If

        Dim hplPrevMonth As HyperLink = DirectCast(lvCalendar.FindControl("hplPrevMonth"), HyperLink)
        If hplPrevMonth IsNot Nothing Then
            hplPrevMonth.Text = Me._DisplayDate.AddMonths(-1).ToString("MMMM")
            hplPrevMonth.NavigateUrl = sPage & sJoin & "viewdate=" & Server.UrlEncode(Me._DisplayDate.AddMonths(-1).ToString("MM/dd/yyyy"))
        End If
    End Sub

    Protected Sub lvCalendar_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvCalendar.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            If Me._DisplayDate.Year = Now.Year And Me._DisplayDate.Month = Now.Month Then
                Dim oDItem As ListViewDataItem = e.Item

                Dim sSunday As String = DataBinder.Eval(oDItem.DataItem, "Sunday").ToString
                If Not String.IsNullOrEmpty(sSunday) Then
                    Dim iSunday As Integer = Integer.Parse(sSunday)

                    If iSunday = Me._DisplayDate.Day Then
                        Dim sCellName As String = "SundayCell"
                    ElseIf Me._DisplayDate.Day > iSunday And Me._DisplayDate.Day <= iSunday + 6 Then

                    End If

                End If

            End If
        End If
    End Sub

    Protected Function FormatEventDisplayDate(ByVal dStartDate As DateTime, ByVal dEndDate As DateTime) As String
        Dim sRet As String = ""
        If apphelp.DateDiff(dStartDate, dEndDate, apphelp.TimeSpanType.Days) = 0 Then
            If apphelp.DateDiff(dStartDate, dEndDate, apphelp.TimeSpanType.Hours) = 0 Then
                sRet = dStartDate.ToString("dddd, MMM dd yyyy") & " " & FormatEventTimeString(dStartDate)
            Else
                sRet = dStartDate.ToString("dddd, MMM dd yyyy") & " " & FormatEventTimeString(dStartDate) & " to " & FormatEventTimeString(dEndDate)
            End If
        Else
            If dStartDate.Hour <> 0 Then
                sRet = dStartDate.ToString("MM/dd/yyyy") & " " & FormatEventTimeString(dStartDate) & " to " & dEndDate.ToString("MM/dd/yyyy") & " " & FormatEventTimeString(dEndDate)
            Else
                sRet = dStartDate.ToString("MM/dd/yyyy") & " to " & dEndDate.ToString("MM/dd/yyyy")
            End If
        End If
        Return sRet
    End Function

    Protected Function FormatEventTimeString(ByVal dDate As DateTime) As String
        Dim iHour As Integer = dDate.Hour
        Dim iMin As Integer = dDate.Minute
        Dim sOff As String = IIf(iHour >= 12, "pm", "am")
        If iMin <> 0 Then
            Return dDate.ToString("%h:mm") & sOff
        Else
            Return dDate.ToString("%h") & sOff
        End If
    End Function
End Class
