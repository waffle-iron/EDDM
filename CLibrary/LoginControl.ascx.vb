
Partial Class CLibrary_LoginControl
    Inherits CLibraryBase

    Public Property LoginMessage As String
        Get
            Return lLoginMessage.Text
        End Get
        Set(value As String)
            lLoginMessage.Text = value
        End Set
    End Property

    Public Property LoginMessageCssClass As String
        Get
            Return lLoginMessage.CssClass
        End Get
        Set(value As String)
            lLoginMessage.CssClass = value
        End Set
    End Property

    Public Property SignupSeparator As String
        Get
            Return lSignupSeparator.Text
        End Get
        Set(value As String)
            lSignupSeparator.Text = value
        End Set
    End Property

    Public Property ShowSignupLink As Boolean
        Get
            Return phSignUp.Visible
        End Get
        Set(value As Boolean)
            phSignUp.Visible = value
        End Set
    End Property

    Public Property SignupText As String
        Get
            Return hplSignup.Text
        End Get
        Set(value As String)
            hplSignup.Text = value
        End Set
    End Property

    Public Property SignupCssClass As String
        Get
            Return hplSignup.CssClass
        End Get
        Set(value As String)
            hplSignup.CssClass = value
        End Set
    End Property

    Private _UsernameWatermark As String = "Username"
    Public Property UsernameWatermark As String
        Get
            Return _UsernameWatermark
        End Get
        Set(value As String)
            _UsernameWatermark = value
        End Set
    End Property

    Private _PasswordWatermark As String = "Password"
    Public Property PasswordWatermark As String
        Get
            Return _PasswordWatermark
        End Get
        Set(value As String)
            _PasswordWatermark = value
        End Set
    End Property

    Protected Overrides Sub BuildControl()
        If Request.IsAuthenticated Then
            If HttpContext.Current.User.IsInRole("Admin") Then
                phMyAccount.Visible = False
            End If

            hplLogonStatus.NavigateUrl = "~/logout.aspx"
            lLogonStatus.Text = "Logoff"
            pAuth.Visible = True
        Else
            'jqueryHelper.IncludePlugin(Page, "watermark", "~/scripts/jquery.watermark.min.js")

            'Dim oJs As New StringBuilder
            'oJs.AppendLine("jQuery(document).ready(function($) {")
            'oJs.AppendLine("    $('#" & Username.ClientID & "').watermark('" & Me.UsernameWatermark & "', { className: 'watermark' });")
            'oJs.AppendLine("    $('#" & Password.ClientID & "').watermark('" & Me.PasswordWatermark & "', { className: 'watermark' });")
            'oJs.AppendLine("});")
            'jqueryHelper.RegisterClientScript(Page, Me.ClientID & "Init", oJs.ToString)

            pAnon.Visible = True
        End If
    End Sub
End Class
