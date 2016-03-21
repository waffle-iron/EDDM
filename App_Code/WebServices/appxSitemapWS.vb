Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.Net.Mail
Imports log4net

<ServiceContract(Namespace:="")> _
<SilverLightFaultBehavior()> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class appxSitemapWS
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    <OperationContract()> _
    Public Sub GenerateSitemap(sKey As String, SiteId As Integer, sNotify As String)
        If sKey = ConfigurationManager.AppSettings("AutogenKey") Then
            Dim oStart As Uri = New Uri(appxCMS.SiteDataSource.GetBaseUrl(SiteId))
            Dim sStart As String = oStart.ToString.ToLower

            Dim sMsg As String = ""
            Dim bGen As Boolean = appxCMS.SEO.SitemapGenerator.Generate(SiteId, oStart, sStart, sMsg)

            Try
                Using oMail As New MailMessage("no-reply@" & oStart.Host, sNotify, "Sitemap Generation Complete", "")
                    If bGen Then
                        oMail.Body = "The sitemap was generated successfully for " & sStart
                        '-- Attach the sitemap file
                        oMail.Attachments.Add(New Attachment(HttpContext.Current.Server.MapPath("~/app_data/Sitemap/sitemap" & SiteId & ".xml")))
                    Else
                        oMail.Body = "There was a problem generating the sitemap for " & sStart & ": " & sMsg & ControlChars.CrLf & ControlChars.CrLf & "You can check the site error log for more detail."
                    End If

                    Dim oSmtp As New SmtpClient
                    oSmtp.Send(oMail)
                End Using
            Catch ex As Exception
                log.Error(ex.Message, ex)
            End Try
        Else
            log.Warn("GenerateSitemap accessed from " & HttpContext.Current.Request.UserHostAddress & " with invalid request key.")
        End If
    End Sub

    ' Add more operations here and mark them with <OperationContract()>

End Class
