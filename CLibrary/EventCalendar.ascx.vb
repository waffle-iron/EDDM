
Partial Class CLibrary_EventCalendar
    Inherits System.Web.UI.UserControl

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

    Private _CurMonth As Integer = 0

    Private ReadOnly Property StartDate() As DateTime
        Get
            Dim dNow As DateTime = System.DateTime.Now
            Dim dStart As DateTime = New DateTime(dNow.Year, dNow.Month, dNow.Day, 0, 0, 0)

            Dim sRDay As String = ""
            If Request.QueryString("d") IsNot Nothing Then
                sRDay = Request.QueryString("d")
            End If
            If Not DateTime.TryParse(sRDay, dStart) Then
                dStart = New DateTime(dNow.Year, dNow.Month, dNow.Day, 0, 0, 0)
            End If
            Return dStart
        End Get
    End Property

    Private _LookAheadDays As Integer = 365
    Public Property LookAheadDays() As String
        Get
            Return _LookAheadDays.ToString
        End Get
        Set(ByVal value As String)
            Dim iVal As Integer = _LookAheadDays
            If Integer.TryParse(value, iVal) Then
                _LookAheadDays = value
            End If
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Sub BuildControl()
        lTitle.Text = Me.CalendarTitle

        Using oEventA As New appxCMSEventTableAdapters.EventTableAdapter
            Using oEventT As appxCMSEvent.EventDataTable = oEventA.GetUpcoming(StartDate, Me._LookAheadDays, IIf(String.IsNullOrEmpty(Category), Nothing, Category))
                If oEventT.Rows.Count > 0 Then
                    lvCalendar.DataSource = oEventT
                    lvCalendar.DataBind()
                Else
                    pCalendar.Visible = False
                End If
            End Using
        End Using
    End Sub

    Protected Sub lvCalendar_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvCalendar.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim dCalDate As DateTime = DataBinder.Eval(oDItem.DataItem, "EventStartDate")
            Dim dCalEnd As DateTime = DataBinder.Eval(oDItem.DataItem, "EventEndDate")
            Dim iMonth As Integer = dCalDate.Month
            If iMonth <> _CurMonth Then
                Dim pMonthHead As Panel = DirectCast(e.Item.FindControl("pMonthHead"), Panel)
                pMonthHead.Visible = True
                Dim lCalendarMonth As Literal = DirectCast(e.Item.FindControl("lCalendarMonth"), Literal)
                lCalendarMonth.Text = dCalDate.ToString("MMMM")
                _CurMonth = iMonth
            End If

            Dim lCalendarDate As Literal = DirectCast(e.Item.FindControl("lCalendarDate"), Literal)
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
                'lCalendarDate.Text = dCalDate.ToString("dddd, MM/dd/yyyy") & " to " & dCalEnd.ToString("dddd, MM/dd/yyyy")
            End If

            lCalendarDate.Text = oESb.ToString
        End If
    End Sub
End Class
