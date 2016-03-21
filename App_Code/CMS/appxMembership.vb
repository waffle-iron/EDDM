Imports Microsoft.VisualBasic

Namespace appx
    Public Class Membership
        Public Shared Function SendAccountConfirmation(ByVal MemberId As Integer) As Boolean
            Dim bSent As Boolean = False

            Dim sRegKey As String = ""
            Dim sConfKey As String = ""
            Dim sFirstName As String = ""
            Dim sLastName As String = ""
            Dim sEmailAddress As String = ""
            Dim sUsername As String = ""
            Dim sPassword As String = ""

            Dim oMember As appxCMS.User = appxCMS.UserDataSource.GetUser(MemberId)
            If oMember IsNot Nothing Then
                sRegKey = oMember.RegistrationKey
                sConfKey = oMember.ConfirmationKey
                sFirstName = oMember.FirstName
                sLastName = oMember.LastName
                sEmailAddress = oMember.EmailAddress
                sUsername = oMember.UserID
                sPassword = oMember.Token
            End If

            If Not String.IsNullOrEmpty(sRegKey) Then
                Dim sConfUrl As String = ""
                Dim sConfFullUrl As String = ""
                Dim oBuilder As New UriBuilder(HttpContext.Current.Request.Url)
                oBuilder.Path = "Signup_Confirm.aspx"
                sConfUrl = oBuilder.ToString
                oBuilder.Query = "regkey=" & HttpContext.Current.Server.UrlEncode(sRegKey) & "&confkey=" & HttpContext.Current.Server.UrlEncode(sConfKey)
                sConfFullUrl = oBuilder.ToString

                '-- Send e-mail with confirmation in it
                Dim oMsg As New appxMessage
                oMsg.MessageArgs.Add("firstname", sFirstName)
                oMsg.MessageArgs.Add("lastname", sFirstName)
                oMsg.MessageArgs.Add("emailaddress", sEmailAddress)
                oMsg.MessageArgs.Add("username", sUsername)
                oMsg.MessageArgs.Add("password", sPassword)
                oMsg.MessageArgs.Add("fullactivationlink", sConfFullUrl)
                oMsg.MessageArgs.Add("activationlink", sConfUrl)
                oMsg.MessageArgs.Add("regkey", sRegKey)
                oMsg.MessageArgs.Add("confkey", sConfKey)

                Using oMessageA As New appxSetupTableAdapters.EmailTemplateTableAdapter
                    Using oMessageT As appxSetup.EmailTemplateDataTable = oMessageA.GetEmailTemplateByName("Member Signup Confirmation")
                        If oMessageT.Rows.Count > 0 Then
                            Dim oMessage As appxSetup.EmailTemplateRow = oMessageT.Rows(0)

                            oMsg.Recipient = oMessage.ToAddress
                            oMsg.SendFrom = oMessage.FromAddress
                            If oMessage.IsReplyToAddressNull Then
                                oMsg.ReplyTo = oMessage.FromAddress
                            Else
                                If String.IsNullOrEmpty(oMessage.ReplyToAddress) Then
                                    oMsg.ReplyTo = oMessage.FromAddress
                                Else
                                    oMsg.ReplyTo = oMessage.ReplyToAddress
                                End If
                            End If
                            oMsg.CCList = oMessage.CCList
                            oMsg.BCCList = oMessage.BCCList
                            oMsg.Subject = oMessage.Subject
                            oMsg.SendHTML = oMessage.IsHTML
                            oMsg.Body = oMessage.Body

                            Try
                                bSent = oMsg.SendMail()
                            Catch ex As Exception
                            End Try
                        End If
                    End Using
                End Using
            End If

            Return bSent
        End Function
    End Class
End Namespace
