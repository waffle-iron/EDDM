Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.IO
Imports log4net

'-- Messageblast
Namespace MessageBlast
    Public Class Message
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

        Public Property SendFromName() As String
            Get
                Return _FromName
            End Get
            Set(ByVal value As String)
                _FromName = value
            End Set
        End Property
        Private _FromName As String
        Private _oFromName As String

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

        Public Property ServerName() As String
            Get
                Return _ServerName
            End Get
            Set(ByVal value As String)
                _ServerName = value
            End Set
        End Property
        Private _ServerName As String = "localhost"

        Public Property Port() As Integer
            Get
                Return _Port
            End Get
            Set(ByVal value As Integer)
                _Port = value
            End Set
        End Property
        Private _Port As Integer = 25

        Public Property UserName() As String
            Get
                Return _Username
            End Get
            Set(ByVal value As String)
                _Username = value
            End Set
        End Property
        Private _Username As String = ""

        Public Property Password() As String
            Get
                Return _Password
            End Get
            Set(ByVal value As String)
                _Password = value
            End Set
        End Property
        Private _Password As String = ""

        Public Property TextBody() As String
            Get
                Return _TextBody
            End Get
            Set(ByVal value As String)
                _TextBody = value
            End Set
        End Property
        Private _TextBody As String = ""

        Public Property HtmlBody() As String
            Get
                Return _HtmlBody
            End Get
            Set(ByVal value As String)
                _HtmlBody = value
            End Set
        End Property
        Private _HtmlBody As String = ""

        Private _oTextBody As String = ""
        Private _oHtmlBody As String = ""

        Public ReadOnly Property ErrorMessage() As String
            Get
                Return _ErrorMessage
            End Get
        End Property
        Private _ErrorMessage As String = ""

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

        Public Class MessageArgC
            Public UserID As Integer
            Public UserName As String
            Public OrderID As Integer
            Public SupplierID As Integer
        End Class

        Public Sub New()
            '-- Load the default values for the MessageArgs
            _MessageArgs.Add("datetimestamp", System.DateTime.Now.ToString)
            'Try
            '    _MessageArgs.Add("siteurl", "http://" & HttpContext.Current.Request.Url.Host)
            'Catch ex As Exception
            '    log.Error(ex.Message, ex)
            'End Try
        End Sub

        Public Overloads Function PopMessageArgs(ByVal msgtype As MessageType, ByVal oArgs As MessageArgC) As Boolean
            'Select Case msgtype
            '    Case MessageType.Order
            'If Not String.IsNullOrEmpty(oArgs.UserID.ToString) Then
            ' PopUserData(oArgs.UserID)
            ' Else
            'PopUserData(oArgs.UserName)
            'End If
            '    Case MessageType.User
            'If oArgs.UserID.ToString <> 0 Then
            ' PopUserData(oArgs.UserID)
            ' Else
            ' PopUserData(oArgs.UserName)
            ' End If

            '    Case Else
            'Return False
            'End Select
            Return True
        End Function

        '    Private Overloads Sub PopUserData(ByVal UserID As Integer)
        ' Dim oA As New efCustomerTableAdapters.CustomerTableAdapter
        'Dim oCustT As efCustomer.CustomerDataTable = oA.GetCustomer(UserID)
        '    PopUserData(oCustT)
        '    oCustT.Dispose()
        '    oA.Dispose()
        'End Sub

        '    Private Overloads Sub PopUserData(ByVal Username As String)
        ' Dim oA As New efCustomerTableAdapters.CustomerTableAdapter
        ' Dim oCustT As efCustomer.CustomerDataTable = oA.GetCustomerByEmail(Username)
        '     PopUserData(oCustT)
        '     oCustT.Dispose()
        '     oA.Dispose()
        'End Sub

        'Private Overloads Sub PopUserData(ByRef UserData As efCustomer.CustomerDataTable)
        '    If UserData.Rows.Count > 0 Then
        '        For i As Integer = 0 To UserData.Columns.Count - 1
        '            _MessageArgs.Add(UserData.Columns(i).ColumnName.ToLower, UserData.Item(0)(i))
        '        Next
        '    End If
        'End Sub

        Public Enum MessageType
            User
            Order
            Supplier
        End Enum

        Public Function SendMail() As Boolean
            Dim bSuccess As Boolean = True
            Dim oMail As New System.Net.Mail.MailMessage

            Try
                _oRecipient = PrepStr(Recipient)
                _oFrom = PrepStr(SendFrom)
                _oFromName = PrepStr(SendFromName)
                _oReplyTo = PrepStr(ReplyTo)
                _oCCList = PrepStr(CCList)
                _oBCCList = PrepStr(BCCList)
                _oSubject = PrepStr(Subject)
                _oTextBody = PrepStr(TextBody)
                _oHtmlBody = PrepStr(HtmlBody)

                Dim aSep() As Char = {",", ";"}

                If Not String.IsNullOrEmpty(_oRecipient) Then
                    Dim aTo() As String = _oRecipient.Split(aSep)
                    For iTo As Integer = 0 To aTo.Length - 1
                        If Not String.IsNullOrEmpty(aTo(iTo).Trim) Then
                            oMail.To.Add(aTo(iTo))
                        End If
                    Next
                End If

                oMail.From = New MailAddress(_oFrom, _oFromName)
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
                oMail.Subject = _oSubject
                oMail.DeliveryNotificationOptions = DeliveryNotificationOptions.None

                If Not String.IsNullOrEmpty(_oHtmlBody) Then
                    'Dim oBodyHtml As AlternateView = AlternateView.CreateAlternateViewFromString(_oHtmlBody, System.Text.Encoding.ASCII, "text/html")
                    'Dim oBodyHtml As AlternateView = AlternateView.CreateAlternateViewFromString(_oHtmlBody)
                    'oMail.AlternateViews.Add(oBodyHtml)
                    AddConvertHtmlImgToCID(oMail, _oHtmlBody)
                End If

                If Not String.IsNullOrEmpty(_oTextBody) Then
                    Dim oBodyPlain As AlternateView = AlternateView.CreateAlternateViewFromString(_oTextBody)
                    oMail.AlternateViews.Add(oBodyPlain)
                End If

                If Attachments.Count > 0 Then
                    For iA As Integer = 0 To Attachments.Count - 1
                        oMail.Attachments.Add(New Attachment(Attachments.Item(iA)))
                    Next
                End If

                If String.IsNullOrEmpty(ServerName) Then
                    ServerName = "localhost"
                End If
                If Port = 0 Then
                    Port = 25
                End If

                Dim oSMTP As New SmtpClient(ServerName, Port)
                If Not String.IsNullOrEmpty(UserName) Then
                    oSMTP.Credentials = New System.Net.NetworkCredential(UserName, Password)
                End If
                oSMTP.Send(oMail)
            Catch ex As Exception
                _ErrorMessage = "<b>" & ex.Message & "</b><br/>" & ex.StackTrace
                bSuccess = False
                log.Error(ex.Message, ex)
            Finally
                oMail.Dispose()
            End Try

            Return bSuccess

        End Function

        Private Function PrepStr(ByVal inStr As String) As String
            Dim sTmp As String = inStr

            If Not String.IsNullOrEmpty(sTmp) Then
                Dim oRE As New Regex("#[a-zA-Z0-9]+#", RegexOptions.IgnoreCase & RegexOptions.Multiline)
                For Each oMatch As Match In oRE.Matches(inStr)
                    Dim sMatchItem As String = oMatch.Value.Replace("#", "").ToLower
                    If _MessageArgs.ContainsKey(sMatchItem) Then
                        sTmp = sTmp.Replace(oMatch.Value, _MessageArgs(sMatchItem).ToString)
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

        Private Sub AddConvertHtmlImgToCID(ByVal oMail As System.Net.Mail.MailMessage, ByVal sHTML As String)
            '<img[^>]+src="([^"]+)"[^>]+>
            Dim dicMatches As New Hashtable

            Dim iMatch As Integer = 1
            Dim oRE As New Regex("<img[^>]+src=""(?<ImgSrc>[^""]+)""[^>]+>", RegexOptions.IgnoreCase & RegexOptions.Multiline)
            For Each oMatch As Match In oRE.Matches(sHTML)
                sHTML = sHTML.Replace(oMatch.Groups("ImgSrc").Value, "cid:img" & iMatch)
                If Not dicMatches.ContainsKey(iMatch) Then
                    dicMatches.Add(iMatch, oMatch.Groups("ImgSrc").Value)
                End If
                iMatch = iMatch + 1
            Next

            Dim oAltView As AlternateView = AlternateView.CreateAlternateViewFromString(sHTML, System.Text.Encoding.ASCII, "text/html")
            Dim oEnum As IDictionaryEnumerator = dicMatches.GetEnumerator
            While oEnum.MoveNext
                Dim sFile As String = oEnum.Value
                Dim sCID As String = "img" & oEnum.Key

                If sFile.ToLower.StartsWith("/cmsimages") Then
                    sFile = HttpContext.Current.Server.MapPath(sFile)
                    Dim oLnk As New LinkedResource(sFile)
                    oLnk.ContentId = sCID
                    oLnk.TransferEncoding = Net.Mime.TransferEncoding.Base64
                    Dim oFi As New FileInfo(sFile)
                    Select Case oFi.Extension.ToLower
                        Case ".jpg"
                            oLnk.ContentType.MediaType = System.Net.Mime.MediaTypeNames.Image.Jpeg
                        Case ".png"

                        Case ".gif"
                            oLnk.ContentType.MediaType = System.Net.Mime.MediaTypeNames.Image.Gif
                    End Select
                    oAltView.LinkedResources.Add(oLnk)

                ElseIf sFile.ToLower.StartsWith("http://") Or sFile.ToLower.StartsWith("https://") Then
                    Dim oClienta As New System.Net.WebClient
                    Dim oFile() As Byte = oClienta.DownloadData(sFile)
                    Dim oMs As New MemoryStream(oFile)
                    Dim oLnk As New LinkedResource(oMs)
                    oLnk.ContentId = sCID
                    oLnk.TransferEncoding = Net.Mime.TransferEncoding.Base64
                    oAltView.LinkedResources.Add(oLnk)
                End If
            End While

            oMail.AlternateViews.Add(oAltView)
        End Sub

        'Public Sub ConvertImgSrcToCID(ByVal html As String, ByVal oMail As System.Net.Mail.MailMessage)

        '    Dim document As HtmlDocument = HtmlDocument.Create(html, False)
        '    Dim imageNodes As New HtmlNodeCollection()
        '    ' Get All the img nodes
        '    GetImageNodes(document.Nodes, imageNodes)
        '    Dim element As HtmlElement
        '    For Each element In imageNodes
        '        Dim path As String = HttpContext.Current.Server.MapPath(element.Attributes("src").Value)
        '        Dim imageFileInfo As New FileInfo(HttpContext.Current.Server.MapPath(element.Attributes("src").Value))
        '        Dim contentId As String = imageFileInfo.Name.Replace(imageFileInfo.Extension, String.Empty)
        '        Dim bodyPart As CDO.IBodyPart = emailMessage.AddRelatedBodyPart(path, contentId, CDO.CdoReferenceType.cdoRefTypeLocation, String.Empty, String.Empty)
        '        bodyPart.Fields.Append("urn:schemas:mailheader:Content-ID", DataTypeEnum.adVariant, 255, FieldAttributeEnum.adFldMayBeNull, String.Format("<{0}>", contentId))
        '        bodyPart.Fields.Update()
        '        'Change the src to "cid:<contentId>"
        '        element.Attributes("src").Value = String.Format("cid:{0}", contentId)
        '    Next element
        '    ' set the email text to the modified html
        '    emailMessage.HTMLBody = document.HTML
        'End Sub 'ConvertImagesToEmbeddedMailImages


        'Private Sub GetImageNodes(ByVal nodes As HtmlNodeCollection, ByRef imageNodes As HtmlNodeCollection)
        '    Dim node As HtmlNode
        '    For Each node In nodes
        '        Dim element As HtmlElement = node '
        '        'ToDo: Error processing original source shown below
        '        '
        '        '
        '        '----------------------------^--- Syntax error: ';' expected
        '        If Not (element Is Nothing) Then
        '            If element.Name.ToLower() = "img" Then
        '                imageNodes.Add(element)
        '            End If
        '            If element.Nodes.Count > 0 Then
        '                GetImageNodes(element.Nodes, imageNodes)
        '            End If
        '        End If
        '    Next node
        'End Sub 'GetImageNodes
    End Class
End Namespace

