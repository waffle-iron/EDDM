Imports System.Xml

Partial Class _Logon
    Inherits appxCMS.PageBase

    Protected Sub Login1_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate
        Page.Validate("vgLogon")
        If Page.IsValid Then
            Dim sUser As String = Login1.UserName.Trim
            Dim sPassword As String = Login1.Password
            Login1.DestinationPageUrl = QStringToVal("ReturnUrl")
            Dim iAuth As Integer = 0
            Dim bSuspended As Boolean = True
            Dim bConfirmed As Boolean = False
            Dim MemberId As Integer = 0
            Dim bFound As Boolean = False

            Dim oUser As appxCMS.User = appxCMS.UserDataSource.GetUser(sUser)
            If oUser IsNot Nothing Then
                bFound = True
                If oUser.CheckPassword(sPassword) Then
                    MemberId = oUser.AdminID
                    Dim sConfDate As String = ""
                    If oUser.ConfirmationDate.HasValue Then
                        sConfDate = oUser.ConfirmationDate.ToString
                    End If
                    Dim sConfIP As String = ""
                    If oUser.ConfirmationIP IsNot Nothing Then
                        sConfIP = oUser.ConfirmationIP
                    End If
                    If String.IsNullOrEmpty(sConfDate) Or String.IsNullOrEmpty(sConfIP) Then
                        bConfirmed = False
                    Else
                        bConfirmed = True

                        If Not oUser.Suspended Then
                            iAuth = 1
                            bSuspended = False
                        End If
                    End If
                End If
            End If

            If iAuth > 0 Then
                '-- Create Authentication Ticket
                Dim oAuthTicket As FormsAuthenticationTicket
                Dim sEncTicket As String
                Dim oAuthCookie As HttpCookie
                Dim sTimeout As String = ""
                Dim iTimeout As Integer = 0
                If Cache("FormsAuthExpiration") IsNot Nothing Then
                    sTimeout = Cache("FormsAuthExpiration")
                    Integer.TryParse(sTimeout, iTimeout)
                End If
                If iTimeout = 0 Then
                    Try
                        Dim sWebConfig As String = Server.MapPath(VirtualPathUtility.ToAbsolute("~/web.config"))
                        Dim oConfigXml As New XmlDocument
                        oConfigXml.Load(sWebConfig)
                        Dim oForms As XmlNode = oConfigXml.SelectSingleNode("//*[local-name()='forms']")
                        iTimeout = xmlhelp.ReadAttribute(oForms, "timeout")
                        Using oDepends As New CacheDependency(sWebConfig)
                            Cache.Insert("FormsAuthExpiration", iTimeout, oDepends)
                        End Using
                    Catch ex As Exception
                        iTimeout = 480
                    End Try
                End If
                Dim dExp As DateTime = System.DateTime.Now.AddMinutes(iTimeout)
                oAuthTicket = New FormsAuthenticationTicket(1, sUser.ToString, System.DateTime.Now.ToUniversalTime, dExp.ToUniversalTime, False, "")
                sEncTicket = FormsAuthentication.Encrypt(oAuthTicket)
                oAuthCookie = New HttpCookie(FormsAuthentication.FormsCookieName, sEncTicket)
                HttpContext.Current.Response.Cookies.Add(oAuthCookie)
                e.Authenticated = True
            Else
                If Not bFound Then
                    'Login1.FailureText = UpdateStatusMsg("Check your username and try again.", True)
                    Login1.FailureText = "<div class=""alert alert-danger"">Check your username and try again.</div>"
                ElseIf bFound Then
                    'Login1.FailureText = UpdateStatusMsg("Check your password and try again.", True)
                    Login1.FailureText = "<div class=""alert alert-danger"">Check your password and try again.</div>"
                ElseIf Not bConfirmed Then
                    'Login1.FailureText = UpdateStatusMsg("Your account has not been confirmed.", True)
                    Login1.FailureText = "<div class=""alert alert-danger"">Your account has not been confirmed.</div>"
                ElseIf bSuspended Then
                    'Login1.FailureText = UpdateStatusMsg("Your account has been suspended.", True)
                    Login1.FailureText = "<div class=""alert alert-danger"">Your account has been suspended.</div>"
                Else
                    'Login1.FailureText = UpdateStatusMsg("Check your username and password and try again.", True)
                    Login1.FailureText = "<div class=""alert alert-danger"">Check your username and try again.</div>"
                End If
                e.Authenticated = False
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pSignup.Visible = cmshelp.ShowSignUp
    End Sub

    Protected Sub LoadLogin(ByVal sender As Object, ByVal e As System.EventArgs)
        '-- Get login button and make it the default submit behavior

        Dim oLogon As Login = DirectCast(sender, Login)

        If oLogon.DisplayRememberMe Then
            If Not Page.IsPostBack Then
                Dim oRememberMe As HttpCookie = Request.Cookies("LoginRememberMe")
                If oRememberMe IsNot Nothing Then
                    If oRememberMe.Value.ToString = "1" Then
                        Dim oUserCookie As HttpCookie = Request.Cookies("LoginUser")
                        If oUserCookie IsNot Nothing Then
                            Dim sUser As String = oUserCookie.Value.ToString
                            If Not String.IsNullOrEmpty(sUser) Then
                                oLogon.UserName = sUser
                                oLogon.RememberMeSet = True
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Dim oLoginButton As Control = oLogon.FindControl("LoginLinkButton")
        If oLoginButton Is Nothing Then
            oLoginButton = oLogon.FindControl("Login")
        End If
        If oLoginButton IsNot Nothing Then
            Dim sUID As String = oLoginButton.UniqueID.Replace(oLogon.NamingContainer.UniqueID, "")
            If sUID.StartsWith("$") Then
                sUID = sUID.Substring(1)
            End If
            Dim oNC As Control = oLogon.Parent
            Dim pLogin As Panel = DirectCast(oNC, Panel)
            pLogin.DefaultButton = sUID
        End If
    End Sub

    Protected Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim oUsername As TextBox = DirectCast(Login1.FindControl("UserName"), TextBox)
        If oUsername IsNot Nothing Then
            jqueryHelper.Include(Page)
            Dim oJs As New StringBuilder
            oJs.AppendLine("jQuery(document).ready(function($) {")
            oJs.AppendLine("    $('#" & oUsername.ClientID & "').focus();")
            oJs.AppendLine("});")
            jqueryHelper.RegisterClientScript(Page, "LogonInit", oJs.ToString)
        End If
    End Sub
End Class
