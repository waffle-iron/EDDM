
Partial Class CLibrary_LoginStatus
    Inherits CLibraryBase

    Public Property SignInLinkText() As String
        Get
            Return hplSignIn.Text
        End Get
        Set(ByVal value As String)
            hplSignIn.Text = value
        End Set
    End Property

    Private _ShowSignupLink As TriState = TriState.UseDefault
    Public Property ShowSignUpLink As Boolean
        Get
            Dim bRet As Boolean = False
            If _ShowSignupLink = TriState.UseDefault Then
                bRet = cmshelp.ShowSignUp
            Else
                If _ShowSignupLink = TriState.True Then
                    bRet = True
                End If
            End If

            Return bRet
        End Get
        Set(ByVal value As Boolean)
            _ShowSignupLink = value
            phSignup.Visible = value
        End Set
    End Property

    Public Property SignUpLinkText() As String
        Get
            Return hplSignUp.Text
        End Get
        Set(ByVal value As String)
            hplSignUp.Text = value
        End Set
    End Property

    Public Property MyAccountLinkText() As String
        Get
            Return hplMyAccount.Text
        End Get
        Set(ByVal value As String)
            hplMyAccount.Text = value
        End Set
    End Property

    Public Property MyAccountLinkNavigateUrl As String
        Get
            Return hplMyAccount.NavigateUrl
        End Get
        Set(ByVal value As String)
            hplMyAccount.NavigateUrl = value
        End Set
    End Property

    Public Property SignOutLinkText() As String
        Get
            Return hplSignOut.Text
        End Get
        Set(ByVal value As String)
            hplSignOut.Text = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        phSignup.Visible = Me.ShowSignUpLink
        Dim sNextUrl As String = apphelp.GetRequestedURL()
        If String.IsNullOrEmpty(sNextUrl) Then
            sNextUrl = VirtualPathUtility.ToAbsolute(Page.AppRelativeVirtualPath)
        End If

        If Request.IsAuthenticated Then
            pAuth.Visible = True
            hplMyAccount.Text = Me.MyAccountLinkText

            If HttpContext.Current.User.IsInRole("Admin") Then
                hplAdmin.NavigateUrl = "~/admin/"
                hplAdmin.Visible = True
                lAdminSep.Text = " | "
            End If

            hplSignOut.NavigateUrl = "~/Logout.aspx"
        Else
            pAnon.Visible = True
            hplSignIn.NavigateUrl = "~/Logon.aspx?ReturnUrl=" & Server.UrlEncode(sNextUrl)
            hplSignUp.NavigateUrl = "~/Signup.aspx?ReturnUrl=" & Server.UrlEncode(sNextUrl)
        End If
    End Sub
End Class
