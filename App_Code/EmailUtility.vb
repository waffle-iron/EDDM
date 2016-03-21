Imports System
Imports System.Web
Imports System.Web.HttpServerUtility
Imports System.Web.HttpRequest
Imports Microsoft.VisualBasic
Imports System.Net.Mail


Public Class EmailUtility


    '==============================================================================================================================
    '   Written 6/5/2015.  Improve, expand as needed.
    '==============================================================================================================================


    Public Shared ReadOnly Property environmentMode As String
        Get

            'SHOULD return as Dev or Prod
            If ConfigurationManager.AppSettings("Environment") IsNot Nothing Then
                Return ConfigurationManager.AppSettings("Environment").ToLower()
            Else
                'Fall back value
                Return "dev"
            End If


        End Get
    End Property



    Public Shared Sub SendAdminEmail(errorMsg As String)

        Dim smtpMailServer As New SmtpClient
        Dim emailMsg As New MailMessage()
        Dim msgBody As New StringBuilder()
        Dim appEnvironment As String = ""

        'Server stuff
        smtpMailServer.UseDefaultCredentials = False
        smtpMailServer.Credentials = New Net.NetworkCredential("dotnetemail@taradel.com", "Hodor!Hodor!")
        smtpMailServer.Port = 587
        smtpMailServer.EnableSsl = True
        smtpMailServer.Host = "smtp.gmail.com"

        'Message properties
        emailMsg = New MailMessage()
        emailMsg.From = New MailAddress("sales@taradel.com", "EDDM App Message")
        msgBody.Append("<html><body>")

        If environmentMode.ToLower() = "dev" Then
            emailMsg.To.Add("itgroup@taradel.com")
            'emailMsg.CC.Add("russell@taradel.com")
            emailMsg.Subject = "EDDM App Error DEV Mode"
            msgBody.Append("<h3 style='color: red;'>***DEV MODE***</h3>")
        ElseIf environmentMode.ToLower() = "prod" Then
            emailMsg.To.Add("itgroup@taradel.com")
            'emailMsg.Bcc.Add("russell@taradel.com")
            emailMsg.Subject = "EDDM App Error PROD Mode"
            msgBody.Append("<h3 style='color: red;'>***PROD MODE***</h3>")
        Else
            emailMsg.To.Add("itgroup@taradel.com")
            emailMsg.Subject = "EDDM App Error - Mode Unknown"
        End If


        emailMsg.IsBodyHtml = True

        msgBody.Append("<p><strong>OOPS!!</strong></p>")
        msgBody.Append("<p>There was an error encountered in EDDM.</p>")
        msgBody.Append("<p>Please take a look.</p>")
        msgBody.Append("<p><strong>The error was:</strong><br />")
        msgBody.Append(errorMsg & "</p>")
        msgBody.Append("<p><strong>URL:</strong><br />")
        msgBody.Append(HttpContext.Current.Request.Url.AbsolutePath & "</p>")
        msgBody.Append("</body></html>")

        emailMsg.Body = msgBody.ToString()
        smtpMailServer.Send(emailMsg)
        emailMsg.Dispose()
        smtpMailServer.Dispose()


    End Sub



    Public Shared Sub SendQuoteRequest(quoteRequestEmail As String, quoteRequestTo As String)

        Dim smtpMailServer As New SmtpClient
        Dim emailMsg As New MailMessage()
        Dim msgBody As New StringBuilder()
        Dim appEnvironment As String = ""

        'Server stuff
        smtpMailServer.UseDefaultCredentials = False
        smtpMailServer.Credentials = New Net.NetworkCredential("dotnetemail@taradel.com", "Hodor!Hodor!")
        smtpMailServer.Port = 587
        smtpMailServer.EnableSsl = True
        smtpMailServer.Host = "smtp.gmail.com"

        'Message properties
        emailMsg = New MailMessage()
        emailMsg.From = New MailAddress("sales@taradel.com", "EDDM App Message")
        msgBody.Append("<html><body>")
        emailMsg.To.Add(quoteRequestTo)
        If environmentMode.ToLower() = "dev" Then
            emailMsg.To.Add("itgroup@taradel.com")
            'emailMsg.CC.Add("russell@taradel.com")
            emailMsg.Subject = "Your EDDM Quote from www.everydoordirectmail.com"
            msgBody.Append("<h3 style='color: red;'>***DEV MODE***</h3>")
        ElseIf environmentMode.ToLower() = "prod" Then
            emailMsg.To.Add("itgroup@taradel.com")
            'emailMsg.Bcc.Add("russell@taradel.com")
            emailMsg.Subject = "Your Quote"
            msgBody.Append("<h3 style='color: red;'>***PROD MODE***</h3>")
            msgBody.Append("<h3 style='color: red;'>***PROD MODE***</h3>")
        Else
            emailMsg.To.Add("itgroup@taradel.com")
            emailMsg.Subject = "EDDM App Error - Mode Unknown"
        End If


        emailMsg.IsBodyHtml = True

        msgBody.Append(quoteRequestEmail & "</p>")
        msgBody.Append("</body></html>")

        emailMsg.Body = msgBody.ToString()
        smtpMailServer.Send(emailMsg)
        emailMsg.Dispose()
        smtpMailServer.Dispose()


    End Sub



    Public Shared Sub SendArtApprovalEmail(site As String, fullName As String, companyName As String, orderNumber As String, designerName As String, experience As String, recommendation As String, approved As String, comments As String, shareFeedback As String)

        Dim smtpMailServer As New SmtpClient
        Dim emailMsg As New MailMessage()
        Dim msgBody As New StringBuilder()
        Dim appEnvironment As String = ""

        'Server stuff
        smtpMailServer.UseDefaultCredentials = False
        smtpMailServer.Credentials = New Net.NetworkCredential("dotnetemail@taradel.com", "Hodor!Hodor!")
        smtpMailServer.Port = 587
        smtpMailServer.EnableSsl = True
        smtpMailServer.Host = "smtp.gmail.com"

        'Message properties
        emailMsg = New MailMessage()
        emailMsg.From = New MailAddress("sales@taradel.com", site.ToUpper() & " Art Approval")
        msgBody.Append("<html><body>")

        If environmentMode.ToLower() = "dev" Then
            emailMsg.To.Add("shane@taradel.com")
            emailMsg.CC.Add("tom@taradel.com")
            emailMsg.Subject = site.ToUpper() & " Art Approval Submission [DEV MODE]"
        ElseIf environmentMode.ToLower() = "prod" Then
            emailMsg.To.Add("art@taradel.com")
            emailMsg.Bcc.Add("shane@taradel.com")
            emailMsg.Subject = site.ToUpper() & " Art Approval Submission"
        Else
            emailMsg.To.Add("shane@taradel.com")
            emailMsg.Subject = "EDDM App Error - Mode Unknown"
        End If


        emailMsg.IsBodyHtml = True

        msgBody.Append("<h3>" & site.ToUpper() & " Art Approval Submission</h3>")
        msgBody.Append("<p>A Customer approved the provided art file. Please see the details below.</p>")
        msgBody.Append("<p><strong>Full Name: </strong>" & fullName & "<br />")
        msgBody.Append("<strong>Order Number: </strong>" & orderNumber & "<br />")
        msgBody.Append("<strong>Designer: </strong>" & orderNumber & "<br />")
        msgBody.Append("<strong>Company Name: </strong>" & companyName & "<br />")
        msgBody.Append("<strong>Experience Score: </strong>" & experience & "<br />")
        msgBody.Append("<strong>Recommedation Score: </strong>" & recommendation & "<br />")
        msgBody.Append("<strong>Approved: </strong>" & approved & "<br />")
        msgBody.Append("<strong>Comments: </strong>" & comments & "<br />")
        msgBody.Append("<strong>Share Feedback: </strong>" & shareFeedback & "</p>")
        msgBody.Append("</body></html>")

        emailMsg.Body = msgBody.ToString()
        smtpMailServer.Send(emailMsg)
        emailMsg.Dispose()
        smtpMailServer.Dispose()


    End Sub



    Public Shared Sub SendRegisterConfirmEmail(SiteID As Integer, site As String, customerEmail As String, firstName As String, lastName As String, baseURL As String)

        Dim smtpMailServer As New SmtpClient
        Dim emailMsg As New MailMessage()
        Dim msgBody As New StringBuilder()
        Dim appEnvironment As String = ""

        'Site Obj
        Dim siteObj As appxCMS.Site = appxCMS.SiteDataSource.GetSite(SiteID)


        'Addtional Site data.  Build SiteDetails Obj
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


        'Server stuff
        smtpMailServer.UseDefaultCredentials = False
        smtpMailServer.Credentials = New Net.NetworkCredential("dotnetemail@taradel.com", "Hodor!Hodor!")
        smtpMailServer.Port = 587
        smtpMailServer.EnableSsl = True
        smtpMailServer.Host = "smtp.gmail.com"

        'Message properties
        emailMsg = New MailMessage()
        emailMsg.From = New MailAddress(SiteDetails.SysFromEmailAcct, site.ToUpper() & " Registration Confirmation")
        msgBody.Append("<html><body>")

        If environmentMode.ToLower() = "dev" Then
            emailMsg.To.Add("shane@taradel.com")
            emailMsg.CC.Add("wendy@taradel.com")
            emailMsg.Bcc.Add("russell@taradel.com")
            emailMsg.Subject = site.ToUpper() & " Registration Confirmation [DEV MODE]"
        ElseIf environmentMode.ToLower() = "prod" Then
            emailMsg.To.Add(customerEmail)
            emailMsg.Bcc.Add("shane@taradel.com")
            emailMsg.Subject = site.ToUpper() & " Registration Confirmation"
        Else
            emailMsg.To.Add("shane@taradel.com")
            emailMsg.Subject = "EDDM App Error - Mode Unknown (Customer Registration)"
        End If

        emailMsg.IsBodyHtml = True

        msgBody.Append("<h3>" & site.ToUpper() & " Registration Confirmation</h3>")
        msgBody.Append("<p>" & firstName & " " & lastName & ",<br /><br />")
        msgBody.Append("Your registration is now complete!  You can now plan and launch Every Door Direct Mail&reg; campaigns for your organization with just a few clicks.</p>")
        msgBody.Append("<p>You can do any of the following:</p>")
        msgBody.Append("<ul>")
        msgBody.Append("<li>Get an instant <a href=""" & baseURL & "/EDDM-Quote-Request"" target=""_blank"">FREE instant price quote</a></li>")
        msgBody.Append("<li>Browse thousands of <a href=""" & baseURL & "/Templates"" target=""_blank"">FREE design templates</a></li>")
        msgBody.Append("<li>Contact <a href=""" & baseURL & "/help"" target=""_blank"">customer service</a> for assistance</li>")
        msgBody.Append("<li>Start <a href=""" & baseURL & "/Step1-Target.aspx"" target=""_blank"">targeting your area</a> by radius, drive time or demographics</li>")
        msgBody.Append("</ul>")
        msgBody.Append("<p>&nbsp;</p>")
        msgBody.Append("<p>Thanks again for registering!</p>")
        msgBody.Append("<p>&nbsp;</p>")
        msgBody.Append("</body></html>")

        emailMsg.Body = msgBody.ToString()
        smtpMailServer.Send(emailMsg)
        emailMsg.Dispose()
        smtpMailServer.Dispose()


    End Sub



    Public Shared Sub SendOrderConfirmation(SiteID As Integer)

        '' IN PROGRESS. REFACTOR. 7/22/2015.


        ' ''Dim smtpMailServer As New SmtpClient
        ' ''Dim emailMsg As New MailMessage()
        ' ''Dim msgBody As New StringBuilder()
        ' ''Dim appEnvironment As String = ""
        ' ''Dim siteName As String = ""


        '' ''Site Obj
        ' ''Dim siteObj As appxCMS.Site = appxCMS.SiteDataSource.GetSite(SiteID)


        '' ''Addtional Site data.  Build SiteDetails Obj
        ' ''Dim SiteDetails As SiteUtility.SiteDetails
        ' ''SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)


        '' ''Server stuff
        ' ''smtpMailServer.UseDefaultCredentials = False
        ' ''smtpMailServer.Credentials = New Net.NetworkCredential("dotnetemail@taradel.com", "Hodor!Hodor!")
        ' ''smtpMailServer.Port = 587                   '<-- Add to SiteDetails
        ' ''smtpMailServer.EnableSsl = True             '<-- Add to SiteDetails
        ' ''smtpMailServer.Host = "smtp.gmail.com"      '<-- Add to SiteDetails


        '' ''Message properties
        ' ''emailMsg = New MailMessage()
        ' ''emailMsg.From = New MailAddress("sales@taradel.com", siteObj.Name & " Order Confirmation")


        'msgBody.Append("<html><body>")

        'If environmentMode.ToLower() = "dev" Then
        '    emailMsg.To.Add("shane@taradel.com")
        '    emailMsg.CC.Add("wendy@taradel.com")
        '    emailMsg.Bcc.Add("russell@taradel.com")
        '    emailMsg.Subject = site.ToUpper() & " Registration Confirmation [DEV MODE]"
        'ElseIf environmentMode.ToLower() = "prod" Then
        '    emailMsg.To.Add(customerEmail)
        '    emailMsg.Bcc.Add("shane@taradel.com")
        '    emailMsg.Subject = site.ToUpper() & " Registration Confirmation"
        'Else
        '    emailMsg.To.Add("shane@taradel.com")
        '    emailMsg.Subject = "EDDM App Error - Mode Unknown (Customer Registration)"
        'End If

        'emailMsg.IsBodyHtml = True

        'msgBody.Append("<h3>" & site.ToUpper() & " Registration Confirmation</h3>")
        'msgBody.Append("<p>" & firstName & " " & lastName & ",<br /><br />")
        'msgBody.Append("Your registration is now complete!  You can now plan and launch Every Door Direct Mail&reg; campaigns for your organization with just a few clicks.</p>")
        'msgBody.Append("<p>You can do any of the following:</p>")
        'msgBody.Append("<ul>")
        'msgBody.Append("<li>Get an instant <a href=""" & baseURL & "/EDDM-Quote-Request"" target=""_blank"">FREE instant price quote</a></li>")
        'msgBody.Append("<li>Browse thousands of <a href=""" & baseURL & "/Templates"" target=""_blank"">FREE design templates</a></li>")
        'msgBody.Append("<li>Contact <a href=""" & baseURL & "/help"" target=""_blank"">customer service</a> for assistance</li>")
        'msgBody.Append("<li>Start <a href=""" & baseURL & "/Step1-Target.aspx"" target=""_blank"">targeting your area</a> by radius, drive time or demographics</li>")
        'msgBody.Append("</ul>")
        'msgBody.Append("<p>&nbsp;</p>")
        'msgBody.Append("<p>Thanks again for registering!</p>")
        'msgBody.Append("<p>&nbsp;</p>")
        'msgBody.Append("</body></html>")

        'emailMsg.Body = msgBody.ToString()
        'smtpMailServer.Send(emailMsg)
        'emailMsg.Dispose()
        'smtpMailServer.Dispose()


    End Sub


End Class
