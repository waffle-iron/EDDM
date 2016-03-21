
Partial Class CLibrary_EventList
    Inherits CLibraryBase

    Public Enum PageDisplayType
        UpcomingDays = 0
        UpcomingYear = 1
        UpcomingMonth = 2
        UpcomingWeek = 3
        NextNEvents = 4
    End Enum

    Public Property CssClass() As String
        Get
            Return pEventList.CssClass
        End Get
        Set(ByVal value As String)
            pEventList.CssClass = value
        End Set
    End Property

    Private _DisplayType As PageDisplayType = PageDisplayType.UpcomingDays
    Public Property DisplayAsType() As PageDisplayType
        Get
            Return Me._DisplayType
        End Get
        Set(ByVal value As PageDisplayType)
            Me._DisplayType = value
        End Set
    End Property

    Private _EventType As String = ""
    Public Property EventType() As String
        Get
            Return _EventType
        End Get
        Set(ByVal value As String)
            _EventType = value
        End Set
    End Property

    Private _EventSubType As String = ""
    Public Property EventSubType() As String
        Get
            Return _EventSubType
        End Get
        Set(ByVal value As String)
            _EventSubType = value
        End Set
    End Property

    Private _MaxEventsToShow As Integer = 100
    Public Property NumberOfEventsToShow() As Integer
        Get
            Return _MaxEventsToShow
        End Get
        Set(ByVal value As Integer)
            _MaxEventsToShow = value
        End Set
    End Property

    Private _NumberOfDaysToShow As Integer = 365
    Public Property NumberOfDaysToShow() As Integer
        Get
            Return _NumberOfDaysToShow
        End Get
        Set(ByVal value As Integer)
            _NumberOfDaysToShow = value
        End Set
    End Property

    Private _NoUpcomingEventsMessage As String = "There are no upcoming events to show."
    Public Property NoUpcomingEventsMessage() As String
        Get
            Return _NoUpcomingEventsMessage
        End Get
        Set(ByVal value As String)
            _NoUpcomingEventsMessage = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        jqueryHelper.RegisterStylesheet(Page, "~/app_styles/EventList.css")

        Using oEventA As New appxCMSEventTableAdapters.EventTableAdapter
            Dim oEvents As appxCMSEvent.EventDataTable = Nothing

            Select Case Me.DisplayAsType
                Case PageDisplayType.UpcomingMonth
                    oEvents = oEventA.GetUpcoming(System.DateTime.Now, 30, Me.EventType)
                Case PageDisplayType.UpcomingWeek
                    oEvents = oEventA.GetUpcoming(System.DateTime.Now, 7, Me.EventType)
                Case PageDisplayType.NextNEvents
                    oEvents = oEventA.GetNextEvents(Me.NumberOfEventsToShow, Me.EventType, Me.EventSubType)
                Case Else '-- Default to PageDisplayType.UpcomingDays
                    oEvents = oEventA.GetUpcoming(System.DateTime.Now, Me.NumberOfDaysToShow, Me.EventType)
            End Select

            lvEvents.DataSource = oEvents
            lvEvents.DataBind()

            oEvents.Dispose()
        End Using
    End Sub

    Protected Sub lvEvents_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvEvents.ItemCreated
        If e.Item.ItemType = ListViewItemType.EmptyItem Then
            Dim lNoUpcoming As Literal = DirectCast(e.Item.FindControl("lNoUpcoming"), Literal)
            If lNoUpcoming IsNot Nothing Then
                lNoUpcoming.Text = Me.NoUpcomingEventsMessage
            End If
        End If
    End Sub

    Protected Sub lvEvents_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvEvents.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim dCalDate As DateTime = DataBinder.Eval(oDItem.DataItem, "EventStartDate")
            Dim dCalEnd As DateTime = DataBinder.Eval(oDItem.DataItem, "EventEndDate")
            Dim iMonth As Integer = dCalDate.Month
            
            Dim lCalendarDate As Literal = DirectCast(e.Item.FindControl("lEventTime"), Literal)
            Dim oESb As New StringBuilder

            If dCalDate = dCalEnd Then
                oESb.Append(dCalDate.ToString("dddd, MM/dd/yyyy"))
                If dCalDate.Hour > 5 Then
                    oESb.Append(" at  ")
                    If dCalDate.Minute > 0 Then
                        oESb.Append(dCalDate.ToString("%h:mmtt"))
                    Else
                        oESb.Append(dCalDate.ToString("%htt"))
                    End If
                End If
            ElseIf dCalDate.Month = dCalEnd.Month And dCalDate.Year = dCalEnd.Year And dCalDate.Day = dCalEnd.Day Then
                '-- same date, show start and end times
                oESb.Append(dCalDate.ToString("dddd, MM/dd/yyyy"))
                oESb.Append(" from ")
                If dCalDate.Minute > 0 Then
                    oESb.Append(dCalDate.ToString("%h:mmtt"))
                Else
                    oESb.Append(dCalDate.ToString("%htt"))
                End If
                oESb.Append(" to ")
                If dCalEnd.Minute > 0 Then
                    oESb.Append(dCalEnd.ToString("%h:mmtt"))
                Else
                    oESb.Append(dCalEnd.ToString("%htt"))
                End If
            Else
                oESb.Append(dCalDate.ToString("dddd, MM/dd/yyyy") & " to " & dCalEnd.ToString("dddd, MM/dd/yyyy"))
            End If

            Dim sEventDisplayDate As String = oESb.ToString
            lCalendarDate.Text = sEventDisplayDate

            'Dim iEventId As Integer = DataBinder.Eval(oDItem.DataItem, "EventId")
            'Dim sEventTitle As String = DataBinder.Eval(oDItem.DataItem, "EventTitle")
            'Dim sEventSummary As String = DataBinder.Eval(oDItem.DataItem, "EventSummary")
            'Dim sEventDesc As String = DataBinder.Eval(oDItem.DataItem, "EventDesc")
            'phEventInfo.Controls.Add(New LiteralControl("<div id=""eventInfo" & iEventId & """ class=""appxEventInfoDialog"" title=""" & sEventDisplayDate & " " & sEventTitle & """><p>" & sEventSummary & "</p><div>" & sEventDesc & "</div><p><a href=""/resources/vEvent.ashx?id=" & iEventId & """>Save to your Calendar</a></p></div>"))
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        jqueryHelper.Include(Page)
        jqueryHelper.IncludePlugin(Page, "appxCalendar", "~/scripts/jquery.appxCalendar.js")
        jqueryHelper.RegisterStylesheet(Page, "~/app_styles/Calendar.css")

        Dim oJs As New StringBuilder()
        oJs.AppendLine("jQuery(document).ready(function() {")
        oJs.AppendLine("    jQuery('div.appxEventInfoDialog').dialog({")
        oJs.AppendLine("        bgiframe:true,")
        oJs.AppendLine("        width:500,")
        oJs.AppendLine("        height:300,")
        oJs.AppendLine("        autoOpen:false")
        oJs.AppendLine("    });")
        oJs.AppendLine("    jQuery('a.eventInfoLink').click(function() {")
        oJs.AppendLine("        jQuery('div#' + jQuery(this).attr('rel')).dialog('open');")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        Page.ClientScript.RegisterClientScriptBlock(GetType(String), "appxCalendarInit", oJs.ToString, True)
    End Sub
End Class
