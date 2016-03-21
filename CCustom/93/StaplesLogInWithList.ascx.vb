Imports System.Collections.Generic
Imports System.Web.Script.Serialization

Partial Class StaplesLogIn
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



    Public Function RetrieveStaplesAPIPath() As String

        Dim host As String = Request.ServerVariables("server_name")
        Dim returnThis As String = "http://" + host + "/Resources/StaplesAPI.ashx"
        Return returnThis

    End Function



    Public Function GeneratePassword() As String
        Dim eddmGUID As String = System.Guid.NewGuid.ToString().Substring(0, 10)
        Return eddmGUID
    End Function


    Protected Sub btnSignIn_Click(sender As Object, e As System.EventArgs) Handles btnSignIn.Click

        Dim sUser As String = Username.Text
        Dim sPass As String = Password.Text
        Dim isStaplesUser As Boolean = False

        Dim sLogonFailMsg As String = "<div class=""alert alert-danger"" role=""alert"">Sorry - The username or password is invalid. Please try again.</div>"
        Dim bLogon As Boolean = False
        Dim data As New RootObject2()

        '-- Set the Destination (if bt querystring exists)
        Dim sRedirect As String = ""
        If HttpContext.Current.Request.QueryString("bt") IsNot Nothing Then
            sRedirect = HttpContext.Current.Request.QueryString("bt")
        ElseIf HttpContext.Current.Request.QueryString("ReturnUrl") IsNot Nothing Then
            sRedirect = HttpContext.Current.Request.QueryString("ReturnUrl")
        End If


        '1. Check the Staples API   --- Staples API -- 9/15/2015 '
        Dim iContactID As Integer = 0
        Dim sCRMAction As String = RetrieveStaplesAPIPath() '"http://eddm.redesign.eddmsite.com/Resources/StaplesAPI.ashx"
        Dim sCRMMethod As String = "POST"
        Dim aSubmitData As Hashtable = New Hashtable()

        aSubmitData.Add("logonId", sUser)
        aSubmitData.Add("logonPassword", sPass)

        Dim sResponse As String = appxCMS.Util.httpHelp.XMLURLPageRequest(sCRMAction, aSubmitData, sCRMMethod)
        Dim jss = New JavaScriptSerializer()

        If (sResponse.IndexOf("STAPLES USER WITH NO STAPLES ADDRESS") > -1) Then
            isStaplesUser = True
        Else
            Try
                data = jss.Deserialize(Of RootObject2)(sResponse)
                isStaplesUser = True
            Catch ex As Exception

                'Not in Staples
                lLogonMsg.Text = "<div class=""alert alert-danger"" role=""alert""Sorry - Unable to authenticate your Staples account. Please try again.<br/></div>" '& sResponse &
                isStaplesUser = False

            End Try

        End If

        '2. If Staples API says "YES" then check Taradel for username and password
        If isStaplesUser Then
            Dim tempCustomer As New Taradel.Customer()
            tempCustomer.CustomerID = 0
            tempCustomer = Taradel.CustomerDataSource.GetCustomer(sUser)

            If (tempCustomer IsNot Nothing) Then
                sUser = tempCustomer.Username
                sPass = tempCustomer.Password

            Else
                '3. If Taradel does not have user name and password, create the user
                Dim staplesAddr As New Address()
                If data.Member IsNot Nothing Then
                    For Each m As Member In data.Member

                        For Each adx As Address In m.address

                            staplesAddr = adx

                            'change password so that we never store Staples password
                            sPass = GeneratePassword()

                        iContactID = Taradel.CustomerDataSource.Add(appxCMS.Util.CMSSettings.GetSiteId, sUser, sPass, staplesAddr.firstname, staplesAddr.lastname,
                                                    sUser, staplesAddr.phone1, staplesAddr.extensionNumber, staplesAddr.address1, staplesAddr.address2, staplesAddr.city,
                                                    staplesAddr.state, staplesAddr.zipcode, False, False, 0, staplesAddr.companyName)

                            Exit For
                        Next

                    Next
                Else
                    'no address information
                    iContactID = Taradel.CustomerDataSource.Add(appxCMS.Util.CMSSettings.GetSiteId, sUser, sPass, String.Empty, String.Empty, sUser, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, True, False, 0, String.Empty)
                End If

            End If

        End If


        Dim oAuth As New Taradel.Auth
        oAuth.Authenticate(sUser, sPass, False, bLogon)
        If bLogon Then
            Response.Redirect("~" & Request.RawUrl)
        Else
            lLogonMsg.Text = "<div class=""extraTopPadding alert alert-danger"" role=""alert"">Sorry - Unable to authenticate your account. Please try again.</div>"
        End If


    End Sub

    ''' <summary>
    ''' RootObject2 is from the StaplesAPI
    ''' </summary>

    Public Class RootObject2
        Public Property Member() As List(Of Member)
            Get
                Return m_Member
            End Get
            Set
                m_Member = Value
            End Set
        End Property
        Private m_Member As List(Of Member)
    End Class


    Public Class Address
        Public Property address1() As String
            Get
                Return m_address1
            End Get
            Set
                m_address1 = Value
            End Set
        End Property
        Private m_address1 As String
        Public Property address2() As String
            Get
                Return m_address2
            End Get
            Set
                m_address2 = Value
            End Set
        End Property
        Private m_address2 As String
        Public Property addressId() As String
            Get
                Return m_addressId
            End Get
            Set
                m_addressId = Value
            End Set
        End Property
        Private m_addressId As String
        Public Property city() As String
            Get
                Return m_city
            End Get
            Set
                m_city = Value
            End Set
        End Property
        Private m_city As String
        Public Property companyName() As String
            Get
                Return m_companyName
            End Get
            Set
                m_companyName = Value
            End Set
        End Property
        Private m_companyName As String
        Public Property extensionNumber() As String
            Get
                Return m_extensionNumber
            End Get
            Set
                m_extensionNumber = Value
            End Set
        End Property
        Private m_extensionNumber As String
        Public Property firstname() As String
            Get
                Return m_firstname
            End Get
            Set
                m_firstname = Value
            End Set
        End Property
        Private m_firstname As String
        Public Property lastname() As String
            Get
                Return m_lastname
            End Get
            Set
                m_lastname = Value
            End Set
        End Property
        Private m_lastname As String
        Public Property phone1() As String
            Get
                Return m_phone1
            End Get
            Set
                m_phone1 = Value
            End Set
        End Property
        Private m_phone1 As String
        Public Property state() As String
            Get
                Return m_state
            End Get
            Set
                m_state = Value
            End Set
        End Property
        Private m_state As String
        Public Property zipcode() As String
            Get
                Return m_zipcode
            End Get
            Set
                m_zipcode = Value
            End Set
        End Property
        Private m_zipcode As String
    End Class


    Public Class Member
        Public Property address() As List(Of Address)
            Get
                Return m_address
            End Get
            Set
                m_address = Value
            End Set
        End Property
        Private m_address As List(Of Address)
    End Class


End Class
