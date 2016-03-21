
Partial Class account_manage
    Inherits MyAccountBase



    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        'Site Details Object
        Dim SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(siteID)



        'Dim customerId As Integer = GetCustomerId
        Dim oCustomer As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(HttpContext.Current.User.Identity.Name)
        If oCustomer Is Nothing Then
            Exit Sub
        End If


        'Set Page Header control
        If Not (SiteDetails.UseRibbonBanners) Then
            PageHeader.headerType = "simple"
            PageHeader.simpleHeader = "Account Settings"
        Else
            PageHeader.headerType = "full"
            PageHeader.fullHeader = "Account Settings"
        End If


        Company.Text = oCustomer.Company
        FirstName.Text = oCustomer.FirstName
        MI.Text = oCustomer.MiddleInitial
        LastName.Text = oCustomer.LastName
        Address1.Text = oCustomer.Address1
        Address2.Text = oCustomer.Address2
        City.Text = oCustomer.City
        State.SelectedValue = oCustomer.State
        PostalCode.Text = oCustomer.ZipCode
        DirectLine.Text = oCustomer.PhoneNumber
        DirectLineExt.Text = oCustomer.Extension
        CompanyLine.Text = oCustomer.CompanyPhoneNumbere
        CompanyLineExt.Text = oCustomer.CompanyExtension
        MobilePhone.Text = oCustomer.MobilePhoneNumber
        Fax.Text = oCustomer.FaxNumber
        EmailAddress.Text = oCustomer.EmailAddress
        CurrentPassword.Text = oCustomer.Password

        'Set the password textbox to be password type
        CurrentPassword.Attributes.Add("type", "password")

        If oCustomer.NewsletterSignup.HasValue Then
            Eletter.Checked = oCustomer.NewsletterSignup.Value
        End If


        'Hide Newsletter if needed.
        If (SiteDetails.HideTaradelContent) Then
            pnlNewsletter.Visible = False
        End If


    End Sub



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub



    Protected Sub SaveChanges(ByVal sender As Object, ByVal e As System.EventArgs)

        Page.Validate("vgAccount")

        If Not Page.IsValid Then
            pnlSuccess.Visible = False
            lblSuccess.Visible = False
            Exit Sub
        End If

        'Dim oEr As Label = lErr
        Dim oAuth As New Taradel.Auth
        Dim sUser As String = HttpContext.Current.User.Identity.Name
        Dim sPass As String = CurrentPassword.Text

        Dim sNewPass As String = ""
        Dim sNPass As String = Password.Text
        Dim sNPassC As String = ConfirmPassword.Text


        If sNPass = sNPassC Then
            If Not String.IsNullOrEmpty(sNPass) Then
                sNewPass = sNPass
            End If
        Else
            If Not String.IsNullOrEmpty(sNPass) Then
                lblError.Text = "Passwords do not match."
                pnlError.Visible = True
                pnlSuccess.Visible = False
                Exit Sub
            End If
        End If

        Dim sEmail As String = EmailAddress.Text
        Dim sCo As String = Company.Text
        Dim sFname As String = FirstName.Text
        Dim sMI As String = MI.Text
        Dim sLName As String = LastName.Text
        Dim sPhone As String = DirectLine.Text
        Dim sPhoneExt As String = DirectLineExt.Text
        Dim sCPhone As String = CompanyLine.Text
        Dim sCPhoneExt As String = CompanyLineExt.Text
        Dim sMobile As String = MobilePhone.Text
        Dim sFax As String = Fax.Text
        Dim sAdd1 As String = Address1.Text
        Dim sAdd2 As String = Address2.Text
        Dim sCity As String = City.Text
        Dim sState As String = State.SelectedValue
        Dim sZip As String = PostalCode.Text
        Dim bEletter As Boolean = Eletter.Checked

        Dim customerId As Integer = GetCustomerId
        Dim oCust As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(customerId)
        Dim bAuth As Boolean = False
        bAuth = oAuth.TestAuth(sUser, sPass)
        Dim bUpdated As Boolean = False

        If bAuth Then

            '-- They are validated and allowed to access this account
            'newPass needs to be set to OLD pass if newPass is empty!!!
            If sNewPass.Length = 0 Then
                sNewPass = sPass
            End If


            If bAuth Then
                bUpdated = Taradel.CustomerDataSource.Update(customerId, sEmail, sNewPass, sFname, sMI, sLName, _
                                                                            sEmail, sPhone, sPhoneExt, sMobile, sFax, sCPhone, sCPhoneExt, _
                                                                            sAdd1, sAdd2, sCity, sState, sZip, False, bEletter, sCo)
            Else
                pnlSuccess.Visible = False
                pnlError.Visible = True
                lblError.Text = "Authorization failed.  Please check the password."
            End If

            'Success.
            If bUpdated Then

                Profile.FirstName = sFname
                Profile.LastName = sLName
                lblSuccess.Text = "Success! Your changes have been saved."
                pnlSuccess.Visible = True
                pnlError.Visible = False

                If sNewPass <> sPass Then
                    pnlError.Visible = True
                    lblError.Text = "Please enter the correct password."
                    pnlSuccess.Visible = False
                End If

                'Did not save
            Else
                pnlError.Visible = True
                lblError.Text = "There was a problem saving your account changes."
                pnlSuccess.Visible = False
            End If

        Else
            pnlError.Visible = True
            lblError.Text = "Your Password was invalid. Please check the password."
            pnlSuccess.Visible = False
            Exit Sub
        End If

    End Sub



End Class
