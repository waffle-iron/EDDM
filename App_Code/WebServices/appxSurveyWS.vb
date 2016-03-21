Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.IO
Imports System.Xml
Imports System.Net
Imports System.Net.Mail
Imports log4net

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://applicationx.net/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class appxSurveyWS
    Inherits System.Web.Services.WebService

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    <WebMethod()> _
    Public Function GetSurveyHeader(ByVal SurveyID As Integer) As String
        Dim sXML As String = ""
        Using oSurveyA As New appxSurveyTableAdapters.SurveyHeaderTableAdapter
            Dim oSurveyT As appxSurvey.SurveyHeaderDataTable = oSurveyA.GetSurvey(SurveyID)
            If oSurveyT.Rows.Count > 0 Then
                sXML = xmlhelp.DataToXMLString(oSurveyT)
            End If
            oSurveyT.Dispose()
        End Using

        Return sXML
    End Function

    <WebMethod()> _
    Public Function GetSurveyQuestions(ByVal SurveyID As Integer) As String
        Dim sXML As String = ""
        Using oSurveyA As New appxSurveyTableAdapters.SurveyQuestionTableAdapter
            Dim oQT As appxSurvey.SurveyQuestionDataTable = oSurveyA.GetData(SurveyID)
            If oQT.Rows.Count > 0 Then
                sXML = xmlhelp.DataToXMLString(oQT)
            End If
            oQT.Dispose()
        End Using
        Return sXML
    End Function

    <WebMethod()> _
    Public Function RecordSurveyResponse(ByVal SurveyID As Integer, ByVal sResponse As String) As Boolean
        Dim bSuccess As Boolean = True
        Try
            Dim oSb As New StringBuilder

            Dim oXML As New XmlDocument
            oXML.LoadXml(sResponse)

            Dim oRespondant As XmlNode = oXML.SelectSingleNode("/response/respondant")
            If oRespondant IsNot Nothing Then
                Dim iResponseID As Integer = 0
                Dim sIP As String = xmlhelp.ReadAttribute(oRespondant, "IPAddress")
                Dim sBrowser As String = xmlhelp.ReadAttribute(oRespondant, "Browser")
                Dim sResponseURL As String = xmlhelp.ReadAttribute(oRespondant, "ResponseURL")
                Using oSurveyA As New appxSurveyTableAdapters.SurveyResponseHeaderTableAdapter
                    iResponseID = oSurveyA.Add(SurveyID, 0, System.DateTime.Now, sIP, "", sBrowser, sResponseURL)
                End Using

                oSb.Append("Response URL: " & sResponseURL & ControlChars.CrLf & ControlChars.CrLf)
                oSb.Append("Response From IP Address: " & sIP & ControlChars.CrLf & ControlChars.CrLf)
                oSb.Append("Response Browser Type: " & sBrowser & ControlChars.CrLf & ControlChars.CrLf)

                If iResponseID > 0 Then
                    Using oQA As New appxSurveyTableAdapters.SurveyResponseTableAdapter
                        '-- Add the question responses
                        Dim oQuestions As XmlNodeList = oXML.SelectNodes("/response/questions/question")
                        For Each oQuestion As XmlNode In oQuestions
                            Dim sQuestionID As String = xmlhelp.ReadAttribute(oQuestion, "QuestionID")
                            Dim sQuestion As String = xmlhelp.ReadAttribute(oQuestion, "Question")
                            Dim sQuestionResponse As String = oQuestion.InnerText
                            oQA.AddResponse(iResponseID, sQuestionID, sQuestionResponse)

                            oSb.Append(sQuestion & ": " & sQuestionResponse.Replace("|", ", ") & ControlChars.CrLf & ControlChars.CrLf)
                        Next
                    End Using
                End If
            End If

            Dim sSurveyName As String = ""
            Dim sAction As String = ""
            Dim sResource As String = ""

            Using oSurveyA As New appxSurveyTableAdapters.SurveyHeaderTableAdapter
                Dim oSurveyT As appxSurvey.SurveyHeaderDataTable = oSurveyA.GetSurvey(SurveyID)
                If oSurveyT.Rows.Count > 0 Then
                    Dim oSurvey As appxSurvey.SurveyHeaderRow = oSurveyT.Rows(0)
                    sSurveyName = oSurvey.SurveyName
                    If Not oSurvey.IsResponseActionNull Then
                        sAction = oSurvey.ResponseAction
                    End If
                    If Not oSurvey.IsResponseActionResourceNull Then
                        sResource = oSurvey.ResponseActionResource
                    End If
                End If
                oSurveyT.Dispose()
            End Using

            If sAction.ToLower = "email" Then
                '-- Send the response data in an e-mail, specified in sResource
                Dim oContactMsg As New appxMessage
                oContactMsg.MessageArgs.Add("surveyname", sSurveyName)
                oContactMsg.MessageArgs.Add("responsedata", oSb.ToString)

                Dim oMessageA As New appxSetupTableAdapters.EmailTemplateTableAdapter
                Dim oMessageT As appxSetup.EmailTemplateDataTable = oMessageA.GetEmailTemplateByName(sResource)
                If oMessageT.Rows.Count > 0 Then
                    Dim oMsg As appxSetup.EmailTemplateRow = oMessageT.Rows(0)
                    oContactMsg.SendFrom = oMsg.FromAddress
                    If Not oMsg.IsReplyToAddressNull Then
                        oContactMsg.ReplyTo = oMsg.ReplyToAddress
                    Else
                        oContactMsg.ReplyTo = oMsg.FromAddress
                    End If
                    oContactMsg.Recipient = oMsg.ToAddress
                    oContactMsg.CCList = oMsg.CCList
                    oContactMsg.BCCList = oMsg.BCCList
                    oContactMsg.Subject = oMsg.Subject
                    oContactMsg.SendHTML = oMsg.IsHTML
                    oContactMsg.Body = oMsg.Body
                End If
                oMessageT.Dispose()
                oMessageA.Dispose()

                Dim bMsgSent As Boolean = False
                bMsgSent = oContactMsg.SendMail()
                'If Not bMsgSent Then
                '    apphelp.LogErr(User.Identity.Name, Page.AppRelativeVirtualPath, oContactMsg.SendErr)
                'End If
            End If

        Catch ex As Exception
            bSuccess = False
        End Try
        Return bSuccess
    End Function

    <WebMethod()> _
    Public Function InitPrepareAndSendReport(ByVal SurveyID As Integer, ByVal Recipient As String, ByVal ReportMode As String, sHost As String) As Boolean
        Dim bInit As Boolean = False
        Dim bSurveyValid As Boolean = True
        Dim bRecipValid As Boolean = False
        Dim bModeValid As Boolean = False

        '-- Ensure that survey exists
        Using oSurveyA As New appxSurveyTableAdapters.SurveyHeaderTableAdapter
            Using oSurveyT As appxSurvey.SurveyHeaderDataTable = oSurveyA.GetSurvey(SurveyID)
                If oSurveyT.Rows.Count = 0 Then
                    bSurveyValid = False
                End If
            End Using
        End Using

        If bSurveyValid Then
            '-- Validate e-mail recipient
            Try
                Dim oRecip As New MailAddress(Recipient)
                bRecipValid = True
            Catch ex As Exception
                log.Error(ex.Message, ex)
                bRecipValid = False
            End Try
        End If

        If bRecipValid Then
            '-- Verify report mode is all or unacknowledged or acknowledged
            Dim sMode As String = ReportMode.ToLower
            If sMode = "all" Or sMode = "unacknowledged" Or sMode = "acknowledged" Then
                bModeValid = True
            End If
        End If

        If bSurveyValid And bRecipValid And bModeValid Then
            bInit = True
            Try
                PrepareAndSendReport(SurveyID, Recipient, ReportMode, sHost)
            Catch ex As Exception
                log.Error(ex.Message, ex)
                bInit = False
            End Try
        Else
            bInit = False
        End If

        Return bInit
    End Function

    <SoapDocumentMethod(OneWay:=True), _
    WebMethod(Description:="Generate CSV report of survey responses and e-mail results.")> _
    Public Sub PrepareAndSendReport(ByVal SurveyID As Integer, ByVal Recipient As String, ByVal ReportMode As String, sHost As String)
        Try
            Dim sDate As String = System.DateTime.Now.ToString("yyyyMMdd")
            Dim oRptData As New StringBuilder

            Dim SurveyName As String = ""

            Using oSurveyA As New appxSurveyTableAdapters.SurveyHeaderTableAdapter
                Using oSurveyT As appxSurvey.SurveyHeaderDataTable = oSurveyA.GetSurvey(SurveyID)
                    If oSurveyT.Rows.Count > 0 Then
                        Dim oSurvey As appxSurvey.SurveyHeaderRow = oSurveyT.Rows(0)
                        SurveyName = oSurvey.SurveyName
                    End If
                End Using
            End Using

            Dim sFileName As String = Server.UrlEncode(SurveyName) & "-" & sDate & ".txt"

            Using oHeaderA As New appxSurveyTableAdapters.SurveyResponseHeaderTableAdapter
                Using oResponseA As New appxSurveyTableAdapters.SurveyResponseTableAdapter

                    Dim oHeaderT As appxSurvey.SurveyResponseHeaderDataTable = Nothing
                    Select Case ReportMode.ToLower
                        Case "all"
                            oHeaderT = oHeaderA.GetData(SurveyID)
                        Case "unacknowledged"
                            oHeaderT = oHeaderA.GetUnacknowledged(SurveyID)
                        Case "acknowledged"
                            oHeaderT = oHeaderA.GetAcknowledged(SurveyID)
                    End Select

                    '-- Build Array Lists of Headers for Response Header, Questions (also get Question IDs to get individual responses)
                    Dim alResponseHeader As New ArrayList
                    alResponseHeader.Add(ControlChars.Quote & "SurveyResponseHeaderID" & ControlChars.Quote)
                    alResponseHeader.Add(ControlChars.Quote & "ResponseURL" & ControlChars.Quote)
                    alResponseHeader.Add(ControlChars.Quote & "ResponseDate" & ControlChars.Quote)
                    alResponseHeader.Add(ControlChars.Quote & "IPAddress" & ControlChars.Quote)
                    alResponseHeader.Add(ControlChars.Quote & "Browser" & ControlChars.Quote)

                    Dim alQuestionHeader As New ArrayList
                    Dim alQuestionID As New ArrayList
                    Using oQuestionA As New appxSurveyTableAdapters.SurveyQuestionTableAdapter
                        Dim oQuestionT As appxSurvey.SurveyQuestionDataTable = oQuestionA.GetData(SurveyID)
                        For Each oQ As appxSurvey.SurveyQuestionRow In oQuestionT.Rows
                            alQuestionHeader.Add(ControlChars.Quote & oQ.Question.ToString & ControlChars.Quote)
                            alQuestionID.Add(oQ.SurveyQuestionID.ToString)
                        Next
                        oQuestionT.Dispose()
                    End Using

                    '-- Print out our data headers
                    Dim aResponseHeader() As String = alResponseHeader.ToArray(GetType(String))
                    Dim sResponseHeader As String = String.Join("|", aResponseHeader)

                    Dim aQuestionHeader() As String = alQuestionHeader.ToArray(GetType(String))
                    Dim sQuestionHeader As String = String.Join("|", aQuestionHeader)

                    oRptData.Append(sResponseHeader & "|" & sQuestionHeader & ControlChars.CrLf)
                    'Response.Write(sResponseHeader & "|" & sQuestionHeader & ControlChars.CrLf)

                    '-- Now, print out our responses
                    For Each oHeader As appxSurvey.SurveyResponseHeaderRow In oHeaderT.Rows
                        Dim iResponse As Integer = oHeader.SurveyResponseHeaderID

                        Dim alUserResponseHeader As New ArrayList
                        For iHeader As Integer = 0 To alResponseHeader.Count - 1
                            alUserResponseHeader.Add(ControlChars.Quote & oHeader.Item(alResponseHeader(iHeader).ToString.Replace(ControlChars.Quote, "")).ToString & ControlChars.Quote)
                        Next
                        Dim aUserResponseHeader() As String = alUserResponseHeader.ToArray(GetType(String))
                        Dim sUserResponseHeader As String = String.Join("|", aUserResponseHeader)

                        '-- Get individual question responses
                        Dim alUserResponseData As New ArrayList
                        For iQIndex = 0 To alQuestionID.Count - 1
                            Dim sResponse As String = ""
                            Try
                                sResponse = oResponseA.GetResponse(iResponse, alQuestionID(iQIndex)).ToString()
                            Catch ex As Exception

                            End Try
                            alUserResponseData.Add(ControlChars.Quote & sResponse.ToString.Replace("|", ",").Replace(ControlChars.CrLf, " ") & ControlChars.Quote)
                        Next
                        Dim aUserResponseData() As String = alUserResponseData.ToArray(GetType(String))
                        Dim sUserResponseData As String = String.Join("|", aUserResponseData)

                        oRptData.Append(sUserResponseHeader & "|" & sUserResponseData & ControlChars.CrLf)
                    Next
                    oHeaderT.Dispose()
                End Using
            End Using

            '-- We have all of the data, create the e-mail message and send the file to the recipient
            File.WriteAllText(Path.Combine(Server.MapPath("~/app_data/tmp"), sFileName), oRptData.ToString)
            Using oMS As New MemoryStream
                Dim oBytes() As Byte = Encoding.UTF8.GetBytes(oRptData.ToString)
                oMS.Write(oBytes, 0, oBytes.Length)
                oMS.Seek(0, SeekOrigin.Begin)
                Using oMail As New System.Net.Mail.MailMessage
                    oMail.To.Add(New MailAddress(Recipient))
                    oMail.From = New MailAddress("no-reply@" & sHost)
                    oMail.Subject = "Survey Results: " & sFileName
                    oMail.Attachments.Add(New Attachment(oMS, sFileName))

                    Dim oSMTP As New SmtpClient()
                    oSMTP.Send(oMail)
                End Using
            End Using
        Catch ex As Exception
            '-- Log the error
            log.Error(ex.Message, ex)
            'Using oLogA As New appxLogTableAdapters.ErrorLogTableAdapter
            '    oLogA.Insert(System.DateTime.Now, False, "", "PrepareAndSendReport: " & ex.Message & "<p>" & ex.StackTrace & "</p>")
            'End Using
        End Try
    End Sub
End Class
