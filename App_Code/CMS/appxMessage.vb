Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.IO
Imports log4net

Public Class appxMessage
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Property Recipient() As String
        Get
            Return _Recipient
        End Get
        Set(ByVal value As String)
            _Recipient = value
        End Set
    End Property
    Private _Recipient As String
    Private _oRecipient As String

    Public Property SendFrom() As String
        Get
            Return _From
        End Get
        Set(ByVal value As String)
            _From = value
        End Set
    End Property
    Private _From As String
    Private _oFrom As String

    Public Property ReplyTo() As String
        Get
            Return _ReplyTo
        End Get
        Set(ByVal value As String)
            _ReplyTo = value
        End Set
    End Property
    Private _ReplyTo As String
    Private _oReplyTo As String

    Public Property CCList() As String
        Get
            Return _CCList
        End Get
        Set(ByVal value As String)
            _CCList = value
        End Set
    End Property
    Private _CCList As String
    Private _oCCList As String

    Public Property BCCList() As String
        Get
            Return _BCCList
        End Get
        Set(ByVal value As String)
            _BCCList = value
        End Set
    End Property
    Private _BCCList As String
    Private _oBCCList As String

    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property
    Private _Subject As String
    Private _oSubject As String

    Public Property SendHTML() As Boolean
        Get
            Return _SendHTML
        End Get
        Set(ByVal value As Boolean)
            _SendHTML = value
        End Set
    End Property
    Private _SendHTML As Boolean
    Private _oSendHTML As Boolean

    Public Property Body() As String
        Get
            Return _Body
        End Get
        Set(ByVal value As String)
            _Body = value
        End Set
    End Property
    Private _Body As String
    Private _oBody As String
    Private _oBodyPlain As String

    Public Property MessageArgs() As System.Collections.Hashtable
        Get
            Return _MessageArgs
        End Get
        Set(ByVal value As System.Collections.Hashtable)
            _MessageArgs = value
        End Set
    End Property
    Private _MessageArgs As New System.Collections.Hashtable

    Public Property Attachments() As ArrayList
        Get
            Return _Attachments
        End Get
        Set(ByVal value As ArrayList)
            _Attachments = value
        End Set
    End Property
    Private _Attachments As New ArrayList

    Private _BodyEncoding As Encoding = Nothing
    Public Property BodyEncoding() As Encoding
        Get
            Return _BodyEncoding
        End Get
        Set(ByVal value As Encoding)
            _BodyEncoding = value
        End Set
    End Property

    Private _ErrorMessage As String = ""
    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _ErrorMessage
        End Get
    End Property

    Private _ErrorStackTrace As String = ""
    Public ReadOnly Property ErrorStackTrace() As String
        Get
            Return _ErrorStackTrace
        End Get
    End Property

    Public Class MessageArgC
        Public UserID As Integer
        Public UserName As String
        Public OrderID As Integer
        Public SupplierID As Integer
    End Class

    Public Sub New()
        '-- Load the default values for the MessageArgs
        _MessageArgs.Add("siteurl", "http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME"))
        _MessageArgs.Add("datetimestamp", System.DateTime.Now.ToString)
        Dim oURL As New Uri(_MessageArgs("siteurl"))
        _MessageArgs.Add("sitename", oURL.Host)
        oURL = Nothing
    End Sub

    Public Function SendMail() As Boolean
        Dim bSuccess As Boolean = True
        Dim oMail As New System.Net.Mail.MailMessage

        Try
            _oRecipient = PrepStr(Recipient)
            _oFrom = PrepStr(SendFrom)
            _oReplyTo = PrepStr(ReplyTo)
            _oCCList = PrepStr(CCList)
            _oBCCList = PrepStr(BCCList)
            _oSubject = PrepStr(Subject)
            _oBody = PrepStr(Body)
            If SendHTML Then
                _oBodyPlain = ToPlainText(_oBody)
            Else
                _oBodyPlain = _oBody
            End If

            Dim aSep() As Char = {",", ";"}

            If Not String.IsNullOrEmpty(_oRecipient) Then
                Dim aTo() As String = _oRecipient.Split(aSep)
                For iTo As Integer = 0 To aTo.Length - 1
                    If Not String.IsNullOrEmpty(aTo(iTo).Trim) Then
                        oMail.To.Add(aTo(iTo))
                    End If
                Next
            End If

            oMail.From = New MailAddress(_oFrom)
            If String.IsNullOrEmpty(_oReplyTo) Then
                _oReplyTo = _oFrom
            End If
            oMail.ReplyTo = New MailAddress(_oReplyTo)
            'oMail.ReplyToList.Add(New MailAddress(_oReplyTo))

            If Not String.IsNullOrEmpty(_oCCList) Then
                Dim aCC() As String = _oCCList.Split(aSep)
                For iCC As Integer = 0 To aCC.Length - 1
                    If Not String.IsNullOrEmpty(aCC(iCC).Trim) Then
                        oMail.CC.Add(aCC(iCC))
                    End If
                Next
            End If

            If Not String.IsNullOrEmpty(_oBCCList) Then
                Dim aBCC() As String = _oBCCList.Split(aSep)
                For iBCC As Integer = 0 To aBCC.Length - 1
                    If Not String.IsNullOrEmpty(aBCC(iBCC).Trim) Then
                        oMail.Bcc.Add(aBCC(iBCC))
                    End If
                Next
            End If
            oMail.IsBodyHtml = SendHTML
            oMail.Subject = _oSubject
            oMail.DeliveryNotificationOptions = DeliveryNotificationOptions.None

            'Dim oBodyPlain As AlternateView = AlternateView.CreateAlternateViewFromString(_oBodyPlain)
            'Dim oBodyHTML As AlternateView = AlternateView.CreateAlternateViewFromString(_oBody)
            Try
                If Me.BodyEncoding IsNot Nothing Then
                    oMail.BodyEncoding = Me.BodyEncoding
                End If
            Catch ex As Exception

            End Try
            If SendHTML Then
                'oMail.AlternateViews.Add(oBodyHTML)
                oMail.Body = _oBody
            Else
                oMail.Body = _oBodyPlain
            End If
            'oMail.AlternateViews.Add(oBodyPlain)

            If Attachments.Count > 0 Then
                For iA As Integer = 0 To Attachments.Count - 1
                    oMail.Attachments.Add(New Attachment(Attachments.Item(iA)))
                Next
            End If

            Dim oSMTP As New SmtpClient()
            oSMTP.Send(oMail)
        Catch ex As Exception
            log.Error(ex.Message, ex)

            Me._ErrorMessage = ex.Message
            Me._ErrorStackTrace = ex.StackTrace

            'HttpContext.Current.Response.Write(ex.Message & " - " & ex.StackTrace)
            bSuccess = False
        Finally
            oMail.Dispose()
        End Try

        Return bSuccess
    End Function

    Private Function PrepStr(ByVal inStr As String) As String
        Dim sTmp As String = inStr
        Dim oRE As New Regex("#[a-zA-Z0-9]+#", RegexOptions.IgnoreCase & RegexOptions.Multiline)
        If Not String.IsNullOrEmpty(sTmp) Then
            For Each oMatch As Match In oRE.Matches(inStr)
                Dim sKey As String = oMatch.Value.Replace("#", "").ToLower
                If _MessageArgs.ContainsKey(sKey) Then
                    Try
                        sTmp = sTmp.Replace(oMatch.Value, _MessageArgs(sKey).ToString)
                    Catch ex As Exception
                        log.Error(ex.Message, ex)
                        log.Debug("Match Value: " & oMatch.Value)
                        log.Debug("Replacement Value: " & _MessageArgs(sKey).ToString)
                    End Try

                End If
            Next
        End If
        Return sTmp
    End Function

    Private Function ToPlainText(ByVal inHTMLStr As String) As String
        Dim sTmp As String = inHTMLStr
        Dim oRE As New Regex("<br>|<br/>|<p[^>]*>", RegexOptions.IgnoreCase & RegexOptions.Multiline)
        sTmp = oRE.Replace(sTmp, ControlChars.CrLf)
        Dim oREP As New Regex("</p>", RegexOptions.IgnoreCase & RegexOptions.Multiline)
        sTmp = oREP.Replace(sTmp, ControlChars.CrLf & ControlChars.CrLf)
        Dim oREHtml As New Regex("<(.|\n)+?>", RegexOptions.IgnoreCase & RegexOptions.Multiline)
        sTmp = oREHtml.Replace(sTmp, "")
        Return sTmp
    End Function

End Class
