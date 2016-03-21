
Partial Class vpage_displayevent
    Inherits appxCMS.PageBase

    Protected ReadOnly Property EventID() As Integer
        Get
            Return QStringToInt("ID")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oEA As New appxCMSEventTableAdapters.EventTableAdapter
        Dim oET As appxCMSEvent.EventDataTable = oEA.GetEvent(EventID)
        'Dim oER As appxCMSEvent.EventRow = oET.Rows(0)

        dlEvent.DataSource = oET
        dlEvent.DataBind()

        oET.Dispose()
        oEA.Dispose()
    End Sub


    Protected Sub dlEvent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlEvent.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Then
            Dim oDItem As DataListItem = e.Item

            Dim dCalDate As DateTime = DataBinder.Eval(oDItem.DataItem, "EventStartDate")
            Dim dCalEnd As DateTime = DataBinder.Eval(oDItem.DataItem, "EventEndDate")
            'Dim iMonth As Integer = dCalDate.Month
            'If iMonth <> _CurMonth Then
            '    Dim pMonthHead As Panel = DirectCast(e.Item.FindControl("pMonthHead"), Panel)
            '    pMonthHead.Visible = True
            '    Dim lCalendarMonth As Literal = DirectCast(e.Item.FindControl("lCalendarMonth"), Literal)
            '    lCalendarMonth.Text = dCalDate.ToString("MMMM")
            '    _CurMonth = iMonth
            'End If

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
                lCalendarDate.Text = dCalDate.ToString("dddd, MM/dd/yyyy") & " to " & dCalEnd.ToString("dddd, MM/dd/yyyy")
            End If

            lCalendarDate.Text = oESb.ToString
        End If
    End Sub
End Class
