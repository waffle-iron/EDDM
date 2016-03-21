<%@ WebHandler Language="VB" Class="vEvent" %>

Imports System
Imports System.Web

Public Class vEvent : Implements IHttpHandler
    Private UTCFormatString As String = "yyyyMMddThhmmssZ"
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim oSb As New StringBuilder
        oSb.AppendLine("BEGIN:VCALENDAR")
        oSb.AppendLine("PRODID:-//Application X//appxCMS 2.0//EN")
        oSb.AppendLine("VERSION:2.0")
        oSb.AppendLine("METHOD:PUBLISH")
        oSb.AppendLine("BEGIN:VEVENT")
        oSb.AppendLine("CLASS:PUBLIC")
        oSb.AppendLine("CREATED:" & System.DateTime.Now.ToUniversalTime.ToString(UTCFormatString))
        oSb.AppendLine("SUMMARY:{1}")
        oSb.AppendLine("LOCATION:")
        oSb.AppendLine("UID:eventid{0}@" & context.Request.Url.Host)
        oSb.AppendLine("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:")
        oSb.AppendLine("{2}")
        oSb.AppendLine("DTEND:{4}")
        oSb.AppendLine("DTSTART:{3}")
        oSb.AppendLine("PRIORITY:3")
        oSb.AppendLine("END:VEVENT")
        oSb.AppendLine("END:VCALENDAR")
        Dim sCal As String = oSb.ToString
        
        Dim iEventID As Integer = pageBase.QStringToInt("id")
        Dim sEventTitle As String = ""
        If iEventID > 0 Then
            Using oEventA As New appxCMSEventTableAdapters.EventTableAdapter
                Using oEventT As appxCMSEvent.EventDataTable = oEventA.GetEvent(iEventID)
                    If oEventT.Rows.Count > 0 Then
                        Dim oEvent As appxCMSEvent.EventRow = oEventT.Rows(0)
                        Dim dStart As DateTime = oEvent.EventStartDate.ToUniversalTime
                        Dim dEnd As DateTime = oEvent.EventEndDate.ToUniversalTime
                        sEventTitle = oEvent.EventTitle
                        sCal = String.Format(sCal, iEventID, oEvent.EventTitle, oEvent.EventDesc, dStart.ToString(UTCFormatString), dEnd.ToString(UTCFormatString))
                    End If
                End Using
            End Using
            context.Response.Clear()
            
            context.Response.ContentType = "text/calendar"
            context.Response.AddHeader("content-disposition", "inline;filename=" & sEventTitle & ".ics")
            context.Response.BinaryWrite(New System.Text.ASCIIEncoding().GetBytes(sCal))
            context.Response.Write(sCal)
        End If
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class