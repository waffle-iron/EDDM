Imports System.Collections.Generic

Partial Class CCustom_AccountHeaderMenu
    Inherits CLibraryBase


    Public Property LoginMessage As String
        Get
            'Return lLoginMessage.Text
        End Get
        Set(value As String)
            'lLoginMessage.Text = value
        End Set
    End Property


    Public Property LoginMessageCssClass As String
        Get
            'Return lLoginMessage.CssClass
        End Get
        Set(value As String)
            'lLoginMessage.CssClass = value
        End Set
    End Property


    Public Property SignupSeparator As String
        Get
            'Return lSignupSeparator.Text
        End Get
        Set(value As String)
            'lSignupSeparator.Text = value
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



        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId()
        Dim SiteDetails As SiteUtility.SiteDetails

        SiteDetails = SiteUtility.RetrieveSiteSettings(siteId)



        If Request.IsAuthenticated Then

            Dim sName As String = ""

            If HttpContext.Current.User.IsInRole("Admin") Then

                phMyAccount.Visible = False

                Dim oAdmin As appxCMS.User = appxCMS.UserDataSource.GetUserByEmail(HttpContext.Current.User.Identity.Name)
                If oAdmin IsNot Nothing Then
                    sName = oAdmin.FirstName & " " & oAdmin.LastName
                End If
            Else
                Dim oCust As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(HttpContext.Current.User.Identity.Name)
                If oCust IsNot Nothing Then
                    sName = oCust.FirstName & " " & oCust.LastName
                End If
            End If


            If Profile.Cart Is Nothing OrElse String.IsNullOrEmpty(Profile.Cart.OuterXml) Then
                phEmptyCart.Visible = True

                'This is a hack. Needs improving
                If siteId = 11 Then
                    hypEmptyCart.NavigateUrl = "/OLB/default.aspx"
                Else
                    hypEmptyCart.NavigateUrl = "~/default.aspx"
                End If
            Else
                phFullCart.Visible = True
            End If

            lWelcomeMessage.Text = "Hello, " & sName
            pAuth.Visible = True

        Else

            Dim bEnableSignup As Boolean = True
            Dim bEnableSignIn As Boolean = True
            Dim bSSOMulti As Boolean = False
            Dim bSSO As Boolean = False
            Dim oSite As appxCMS.Site = appxCMS.SiteDataSource.GetSite(siteId)

            If oSite IsNot Nothing Then

                If oSite.EnableSignin.HasValue Then
                    bEnableSignIn = oSite.EnableSignin.Value
                End If

                If oSite.EnableSignup.HasValue Then
                    bEnableSignup = oSite.EnableSignup.Value
                End If

                If bEnableSignup = False Then
                    phRegister.Visible = False
                End If

                If Not (SiteDetails.ShowForgotPWDLink) Then
                    hplForgot.Visible = False
                End If

                litWelcome.Text = SiteUtility.GetStringResourceValue(siteId, "LoginMenuGreeting")



                Dim oSSOProviders As List(Of appxCMS.SiteSSO) = appxCMS.SiteDataSource.GetSSOProviders(siteId)
                Dim iEnabledSSO As Integer = oSSOProviders.Where(Function(s) s.Enabled = True).Count()
                If iEnabledSSO > 0 Then
                    bSSO = True
                End If

                If Not bEnableSignIn AndAlso bSSO Then
                    '-- Check for one or more sso login pages
                    Dim oSSOLogonPages As List(Of appxCMS.SiteSSOLogonPage) = appxCMS.SiteDataSource.GetSSOLogonPages(siteId)
                    Dim oEnabled

                    If oSSOLogonPages.Count = 0 Then
                        '-- Logon is not allowed
                    ElseIf oSSOLogonPages.Count() = 1 Then
                        '-- There is exactly one sso logon page, direct link to it
                        hplSSOLogonPage.NavigateUrl = oSSOLogonPages(0).URL
                        hplSSOLogonPage.Visible = True
                    Else
                        '-- There is a list of SSO logon pages
                        bSSOMulti = True
                        ddSSOLogonPages.Items.Clear()
                        ddSSOLogonPages.Items.Add(New ListItem("Choose Log-In Gateway", ""))
                        For Each oSSOLogonPage As appxCMS.SiteSSOLogonPage In oSSOLogonPages
                            ddSSOLogonPages.Items.Add(New ListItem(oSSOLogonPage.Name, oSSOLogonPage.URL))
                        Next
                        phSSOMulti.Visible = True
                    End If
                Else
                    pAnon.Visible = True
                End If
            Else
                pAnon.Visible = True
            End If
            'jqueryHelper.IncludePlugin(Page, "watermark", "~/scripts/jquery.watermark.min.js")

            Dim oJs As New StringBuilder
            oJs.AppendLine("jQuery(document).ready(function($) {")
            'If bEnableSignIn Then
            '    oJs.AppendLine("    $('#" & Username.ClientID & "').watermark('" & Me.UsernameWatermark & "', { className: 'watermark' });")
            '    oJs.AppendLine("    $('#" & Password.ClientID & "').watermark('" & Me.PasswordWatermark & "', { className: 'watermark' });")
            'Else
            If bSSOMulti Then
                oJs.AppendLine("    $('#" & btnSSOSignIn.ClientID & "').click(function(e) {")
                oJs.AppendLine("        e.preventDefault();")
                oJs.AppendLine("        var sVal = $('#" & ddSSOLogonPages.ClientID & " option:selected').val();")
                oJs.AppendLine("        if (sVal != '') {")
                oJs.AppendLine("            window.location = sVal;")
                oJs.AppendLine("        }")
                oJs.AppendLine("    });")
            End If
            oJs.AppendLine("});")

            jqueryHelper.RegisterClientScript(Page, Me.ClientID & "Init", oJs.ToString)

            End If
    End Sub


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub


    Protected Sub btnSignIn_Click(sender As Object, e As System.EventArgs) Handles btnSignIn.Click

        Dim sUser As String = Username.Text
        Dim sPass As String = Password.Text

        Dim sLogonFailMsg As String = "<div class=""alert alert-danger"" role=""alert""><i class=""glyphicon glyphicon-remove pull-left""></i>&nbsp;The username or password is invalid. Please try again.</div>"
        Dim bLogon As Boolean = False

        '-- Set the Destination (if bt querystring exists)
        Dim sRedirect As String = ""
        If HttpContext.Current.Request.QueryString("bt") IsNot Nothing Then
            sRedirect = HttpContext.Current.Request.QueryString("bt")
        ElseIf HttpContext.Current.Request.QueryString("ReturnUrl") IsNot Nothing Then
            sRedirect = HttpContext.Current.Request.QueryString("ReturnUrl")
        End If

        Dim oAuth As New Taradel.Auth
        oAuth.Authenticate(sUser, sPass, False, bLogon)
        If bLogon Then
            Response.Redirect("~" & Request.RawUrl)
        Else
            lLogonMsg.Text = "<div class=""alert alert-danger"" role=""alert""><i class=""glyphicon glyphicon-remove pull-left""></i>&nbsp;Sorry - Unable to authenticate your account.</div>"
        End If

    End Sub


End Class
