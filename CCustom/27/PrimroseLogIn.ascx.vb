Imports System.Collections.Generic
Imports System.Web.Script.Serialization

Partial Class PrimroseLogIn
    Inherits CLibraryBase





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
                hypEmptyCart.NavigateUrl = "~/default.aspx"
            Else
                phFullCart.Visible = True
            End If

            lWelcomeMessage.Text = "Hello, " & sName
            pAuth.Visible = True

        Else

            Dim bEnableSignup As Boolean = True
            Dim bEnableSignIn As Boolean = True
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
                    phRegisterButton.Visible = False
                End If

                If Not (SiteDetails.ShowForgotPWDLink) Then
                    hplForgot.Visible = False
                End If

                litWelcome.Text = SiteUtility.GetStringResourceValue(siteId, "LoginMenuGreeting")

                pAnon.Visible = True

            End If


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
