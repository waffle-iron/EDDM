Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports log4net

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://applicationx.net/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class MessageBlastWS
    Inherits System.Web.Services.WebService

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    <WebMethod()> _
    Public Function InitCampaignSend(ByVal CampaignID As Integer, ByVal TrackingNumber As String, ByVal UserName As String, ByVal UserID As Integer) As Integer
        '-- First, get a send id for this action
        Dim SendID As Integer = 0
        Using oSendA As New appxMessageBlastTableAdapters.CampaignSendTableAdapter
            SendID = oSendA.Add(CampaignID, TrackingNumber, UserName, UserID, System.DateTime.Now)
        End Using

        '-- Call the async web method to send the campaign
        'Dim oMsgBlast As New MessageBlastWS
        'oMsgBlast.SendCampaign(SendID, CampaignID, TrackingNumber, UserName, UserID)
        'oMsgBlast.Dispose()

        Return SendID
    End Function

    <SoapDocumentMethod(OneWay:=True), _
    WebMethod(Description:="Sends message blast campaign to distribution")> _
    Public Sub SendCampaign(ByVal SendID As Integer, ByVal CampaignID As Integer, ByVal TrackingNumber As String, ByVal UserName As String, ByVal UserID As Integer)

        '-- Now we are working on the send

        '-- Step 1. Get Campaign Info
        Dim sSubject As String = "" '-- Required
        Dim sRecipFld As String = "" '-- Required
        Dim sSenderName As String = ""
        Dim sSenderEmail As String = "" '-- Required
        Dim sReplyTo As String = ""
        Dim sHTMLBody As String = "" '-- At least one of the following is required
        Dim sTextBody As String = ""

        Using oCampaignA As New appxMessageBlastTableAdapters.CampaignTableAdapter
            Using oCampaignT As appxMessageBlast.CampaignDataTable = oCampaignA.GetCampaign(CampaignID)
                If oCampaignT.Rows.Count > 0 Then
                    Dim oCampaign As appxMessageBlast.CampaignRow = oCampaignT.Rows(0)

                    sSubject = oCampaign.Subject.Trim
                    sRecipFld = oCampaign.RecipientTemplateField.Trim
                    If Not oCampaign.IsSenderNameNull Then
                        sSenderName = oCampaign.SenderName.Trim
                    End If
                    sSenderEmail = oCampaign.SenderEmail.Trim
                    If Not oCampaign.IsReplyToNull Then
                        sReplyTo = oCampaign.ReplyTo.Trim
                    End If
                    If Not oCampaign.IsHTMLBodyNull Then
                        sHTMLBody = oCampaign.HTMLBody.Trim
                    End If
                    If Not oCampaign.IsTextBodyNull Then
                        sTextBody = oCampaign.TextBody.Trim
                    End If
                End If
            End Using
        End Using

        If String.IsNullOrEmpty(sSubject) Or String.IsNullOrEmpty(sRecipFld) _
            Or String.IsNullOrEmpty(sSenderEmail) Or _
            (String.IsNullOrEmpty(sHTMLBody) And String.IsNullOrEmpty(sTextBody)) Then

            '-- We cannot send this campaign. Missing required information.
            If String.IsNullOrEmpty(sSubject) Then
                AddSendStatus(SendID, "Unable to send campaign: A subject for the e-mail is required.", True)
            End If
            If String.IsNullOrEmpty(sRecipFld) Then
                AddSendStatus(SendID, "Unable to send campaign: A template recipient field must be specified.", True)
            End If
            If String.IsNullOrEmpty(sSenderEmail) Then
                AddSendStatus(SendID, "Unable to send campaign: The campaign sender's e-mail address must be specified.", True)
            End If
            If String.IsNullOrEmpty(sHTMLBody) Or String.IsNullOrEmpty(sTextBody) Then
                AddSendStatus(SendID, "Unable to send campaign: A message must exist in HTML or Text format.", True)
            End If

            AddSendStatus(SendID, "Aborting Campaign Send.", True)
            FinalizeSend(SendID)
            SetCampaignStatus(CampaignID, "Send Failed")

            Exit Sub
        End If

        '-- Step 2. Get Distribution Info and record recipients into messageblast_CampaignSendRecipient
        AddSendStatus(SendID, "Gathering Distribution Information")

        Dim aQuery As New Hashtable '-- This holds the queries we will use to get the data

        Using oSourceA As New appxMessageBlastTableAdapters.ListSourceTableAdapter
            Using oDistA As New appxMessageBlastTableAdapters.CampaignDistributionListsTableAdapter
                Using oDistT As appxMessageBlast.CampaignDistributionListsDataTable = oDistA.GetCampaignDistribution(CampaignID)
                    For Each oDist As appxMessageBlast.CampaignDistributionListsRow In oDistT.Rows
                        Dim sQuery As String = ""
                        Dim sFilter As String = ""

                        AddSendStatus(SendID, "Compiling distribution from list '" & oDist.Name & "'")

                        Using oSourceT As appxMessageBlast.ListSourceDataTable = oSourceA.GetListSource(oDist.ListSourceID)
                            If oSourceT.Rows.Count > 0 Then
                                Dim oSource As appxMessageBlast.ListSourceRow = oSourceT.Rows(0)
                                Dim sConn As String = oSource.ConnectionString
                                If sConn.ToLower.StartsWith("name=") Then
                                    sConn = ConfigurationManager.ConnectionStrings(sConn.Substring(5)).ConnectionString
                                End If
                                sQuery = oSource.ListQuery
                                sFilter = oDist.Filter
                                If Not String.IsNullOrEmpty(sFilter.Trim) Then
                                    If Not sFilter.ToLower.Trim.StartsWith("where ") Then
                                        sFilter = " WHERE " & sFilter
                                    End If
                                End If
                                sQuery = sQuery & sFilter & " FOR XML RAW"

                                'AddSendStatus(SendID, "Created select statement for distribution: " & sQuery)

                                Dim oDs As New DataSet
                                Using oConn As New SqlConnection(sConn)
                                    Using oCmd As New SqlCommand(sQuery, oConn)
                                        oCmd.CommandTimeout = 90
                                        Using oAdapter As New SqlDataAdapter(oCmd)
                                            Try
                                                oAdapter.Fill(oDs)
                                            Catch ex As Exception
                                                AddSendStatus(SendID, "Error filling adapter: " & ex.Message)
                                                FinalizeSend(SendID)
                                                AddSendStatus(SendID, "Send Failed")
                                                SetCampaignStatus(CampaignID, "Send Failed")

                                                oAdapter.Dispose()
                                                oCmd.Dispose()
                                                oDs.Dispose()
                                                oConn.Close()
                                                oConn.Dispose()

                                                Exit Sub
                                            End Try
                                        End Using
                                    End Using
                                End Using

                                AddSendStatus(SendID, "Retrieved distribution rows from database")

                                Dim oSB As New StringBuilder
                                For Each oRow As DataRow In oDs.Tables(0).Rows
                                    oSB.Append(oRow(0))
                                Next
                                oDs.Dispose()

                                Dim sXML As String = oSB.ToString
                                Dim oXML As New XmlDocument
                                oXML.LoadXml("<rows>" & sXML & "</rows>")

                                Using oRecipA As New appxMessageBlastTableAdapters.CampaignSendRecipientTableAdapter
                                    For Each oDataRow As XmlNode In oXML.SelectNodes("//row")
                                        Dim sEmail As String = ""
                                        Dim oEmail As XmlNode = oDataRow.Attributes.GetNamedItem(sRecipFld)
                                        If oEmail IsNot Nothing Then
                                            sEmail = oEmail.Value
                                            If Not String.IsNullOrEmpty(sEmail) Then
                                                Dim iExists As Integer = oRecipA.RecipientExists(SendID, sEmail)
                                                If iExists = 0 Then
                                                    '-- Record this recipient into the table
                                                    oRecipA.Insert(SendID, sEmail, 0, System.DateTime.Now, 0, False, "", 0, oDataRow.OuterXml)
                                                    AddSendStatus(SendID, "Compiling Campaign Distribution: Adding recipient " & sEmail)
                                                Else
                                                    AddSendStatus(SendID, "Compiling Campaign Distribution: Skipping duplicate recipient " & sEmail)
                                                End If
                                            End If
                                        End If
                                    Next
                                End Using
                            End If
                        End Using
                    Next
                End Using
            End Using
        End Using

        '-- Step 4. Send e-mail messages
        log.Info("Calling SendMessages(" & CampaignID & ", " & SendID & ", " & UserName & ", " & UserID & ")")
        SendMessages(CampaignID, SendID, UserName, UserID)

        FinalizeSend(SendID)
        AddSendStatus(SendID, "Send Completed")
        SetCampaignStatus(CampaignID, "Send Completed")

    End Sub

    Protected Sub SendMessages(ByVal CampaignID As Integer, ByVal SendID As Integer, ByVal UserName As String, ByVal UserID As Integer)
        Try
            SetCampaignStatus(CampaignID, "Sending Messages")

            Dim sFromEmail As String = ""
            Dim sFromName As String = ""
            Dim sReplyTo As String = ""
            Dim sSubject As String = ""
            Dim sHTMLBody As String = ""
            Dim sTextBody As String = ""
            Dim sSMTPServer As String = ""
            Dim sSMTPUser As String = ""
            Dim sSMTPPass As String = ""
            Dim iSMTPPort As Integer = 0

            Using oCampaignA As New appxMessageBlastTableAdapters.CampaignTableAdapter
                Using oCampaignT As appxMessageBlast.CampaignDataTable = oCampaignA.GetCampaign(CampaignID)
                    If oCampaignT.Rows.Count > 0 Then
                        Dim oMsg As appxMessageBlast.CampaignRow = oCampaignT.Rows(0)
                        sFromEmail = oMsg.SenderEmail
                        sFromName = oMsg.SenderName
                        sFromEmail = oMsg.SenderEmail
                        sSubject = oMsg.Subject
                        If Not oMsg.IsHTMLBodyNull Then
                            sHTMLBody = oMsg.HTMLBody
                        End If
                        If Not oMsg.IsTextBodyNull Then
                            sTextBody = oMsg.TextBody
                        End If
                        If Not oMsg.IsSMTPServerNull Then
                            sSMTPServer = oMsg.SMTPServer
                        End If
                        If Not oMsg.IsUsernameNull Then
                            sSMTPUser = oMsg.Username
                        End If
                        If Not oMsg.IsPasswordNull Then
                            sSMTPPass = oMsg.Password
                        End If
                        If Not oMsg.IsPortNull Then
                            iSMTPPort = oMsg.Port
                        End If
                    End If
                End Using
            End Using

            Dim aAttach As New ArrayList
            Using oAttachA As New appxMessageBlastTableAdapters.CampaignAttachmentTableAdapter
                Using oAttachT As appxMessageBlast.CampaignAttachmentDataTable = oAttachA.GetData(CampaignID)
                    For Each oAttach As appxMessageBlast.CampaignAttachmentRow In oAttachT.Rows
                        aAttach.Add(Server.MapPath("/cmsimages/mbattachments/" & CampaignID & "/" & oAttach.Name))
                    Next
                End Using
            End Using

            Using oRecipA As New appxMessageBlastTableAdapters.CampaignSendRecipientTableAdapter
                Dim oRecipT As appxMessageBlast.CampaignSendRecipientDataTable = oRecipA.GetUnsent(SendID)
                For Each oRecip As appxMessageBlast.CampaignSendRecipientRow In oRecipT.Rows
                    Dim oMsg As New MessageBlast.Message

                    Dim oRecipData As New XmlDocument
                    oRecipData.LoadXml("<recip>" & oRecip.RecipientData & "</recip>")
                    Dim oRecipNode As XmlNode = oRecipData.SelectSingleNode("/recip/row")
                    If oRecipNode IsNot Nothing Then
                        For Each oAttrib As XmlNode In oRecipNode.Attributes
                            oMsg.MessageArgs.Add(oAttrib.Name.ToLower, oAttrib.Value)
                        Next
                    End If
                    oMsg.MessageArgs.Add("recipientid", oRecip.RecipientID)
                    oMsg.MessageArgs.Add("sendid", oRecip.SendID)
                    If Not oMsg.MessageArgs.ContainsKey("emailaddress") Then
                        oMsg.MessageArgs.Add("emailaddress", oRecip.RecipientEmail)
                    End If
                    If Not oMsg.MessageArgs.ContainsKey("siteurl") Then
                        Try
                            oMsg.MessageArgs.Add("siteurl", "http://" & Context.Request.Url.Host.ToString)
                        Catch ex As Exception
                            log.Error(ex.Message, ex)
                            oMsg.MessageArgs.Add("siteurl", ConfigurationManager.AppSettings("NICSUrl"))
                        End Try
                    End If
                    'oMsg.MessageArgs.Add("unsubscribeurl", "http://" & HttpContext.Current.Request.Url.Host & "/Unsubscribe.aspx?sid=" & oRecip.SendID & "&rid=" & oRecip.RecipientID)

                    Dim sRecipEmail As String = oRecip.RecipientEmail

                    '-- For testing, set recipient e-mail to myself
                    'sRecipEmail = "misty-scs" & SendID & "@applicationx.net"

                    oMsg.SendFrom = sFromEmail
                    oMsg.SendFromName = sFromName
                    oMsg.ReplyTo = sFromEmail
                    oMsg.Recipient = sRecipEmail
                    oMsg.Subject = sSubject
                    If Not String.IsNullOrEmpty(sHTMLBody) Then
                        oMsg.HtmlBody = sHTMLBody
                    End If
                    If Not String.IsNullOrEmpty(sTextBody) Then
                        oMsg.TextBody = sTextBody
                    End If
                    If Not String.IsNullOrEmpty(sSMTPServer) Then
                        oMsg.ServerName = sSMTPServer
                    End If
                    If Not String.IsNullOrEmpty(sSMTPUser) Then
                        oMsg.UserName = sSMTPUser
                    End If
                    If Not String.IsNullOrEmpty(sSMTPPass) Then
                        oMsg.Password = sSMTPPass
                    End If
                    If iSMTPPort > 0 Then
                        oMsg.Port = iSMTPPort
                    End If

                    If aAttach.Count > 0 Then
                        For iAttach = 0 To aAttach.Count - 1
                            oMsg.Attachments.Add(aAttach(iAttach))
                        Next
                    End If

                    Dim bMsgSent As Boolean = False
                    bMsgSent = oMsg.SendMail()

                    If bMsgSent Then
                        oRecipA.MessageSent(oRecip.RecipientID)
                        AddSendStatus(SendID, "Message sent to " & sRecipEmail)
                    Else
                        AddSendStatus(SendID, "Unable to send message to " & sRecipEmail & ": " & oMsg.ErrorMessage, True)
                    End If

                    oMsg = Nothing
                Next
                oRecipT.Dispose()
            End Using
        Catch ex As Exception
            AddSendStatus(SendID, "Fatal Error Sending: " & ex.Message & "<br/>" & ex.StackTrace, True)

        End Try
    End Sub

    Protected Sub AddSendStatus(ByVal SendID As Integer, ByVal StatusMsg As String)
        AddSendStatus(SendID, StatusMsg, False)
    End Sub

    Protected Sub AddSendStatus(ByVal SendID As Integer, ByVal StatusMsg As String, ByVal ErrorStatus As Boolean)
        System.Threading.Thread.Sleep(50)
        Using oA As New appxMessageBlastTableAdapters.CampaignSendStatusTableAdapter
            oA.Insert(SendID, System.DateTime.Now, ErrorStatus, StatusMsg)
        End Using
    End Sub

    Protected Sub FinalizeSend(ByVal SendID As Integer)
        Using oA As New appxMessageBlastTableAdapters.CampaignSendTableAdapter
            oA.FinalizeSend(System.DateTime.Now, SendID)
        End Using
    End Sub

    Protected Sub SetCampaignStatus(ByVal CampaignID As Integer, ByVal Status As String)
        apphelp.AuditChange("appxmessageblast_Campaign", "CampaignID", CampaignID, "Update", "system", 0)
        Using oA As New appxMessageBlastTableAdapters.CampaignTableAdapter
            oA.SetStatus(Status, CampaignID)
        End Using
    End Sub
End Class
