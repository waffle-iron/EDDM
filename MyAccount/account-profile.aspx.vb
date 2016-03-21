Imports Taradel

Partial Class account_profile
    Inherits appxCMS.PageBase

    Private sSelIndustry As String = ""


    Private _environmentMode As String
    Private ReadOnly Property environmentMode() As String

        Get

            'SHOULD return as Dev or Prod
            If ConfigurationManager.AppSettings("Environment") IsNot Nothing Then
                _environmentMode = ConfigurationManager.AppSettings("Environment").ToLower()
            Else
                'Fall back value
                _environmentMode = "dev"
            End If

            Return _environmentMode

        End Get

    End Property





    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        Dim oCustomer As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name)
        Dim ddlStates As DropDownList = CType(USAStatesDropDown.FindControl("ddlStates"), DropDownList)
        Dim lstPleaseSelect As New ListItem
        lstPleaseSelect.Text = "Please Select"
        lstPleaseSelect.Value = ""
        ddlStates.Items.Insert(0, lstPleaseSelect)


        If environmentMode = "dev" Then
            pnlDebug.Visible = True
        End If


        If oCustomer IsNot Nothing Then

            Company.Text = oCustomer.Company
            FirstName.Text = oCustomer.FirstName
            LastName.Text = oCustomer.LastName

            If oCustomer.Address1 IsNot Nothing Then
                Address1.Text = oCustomer.Address1
            End If

            If oCustomer.Address2 IsNot Nothing Then
                Address2.Text = oCustomer.Address2
            End If

            If oCustomer.City IsNot Nothing Then
                City.Text = oCustomer.City
            End If

            If oCustomer.State IsNot Nothing Then
                USAStatesDropDown.SelectedValue = oCustomer.State
            End If

            If oCustomer.ZipCode IsNot Nothing Then
                PostalCode.Text = oCustomer.ZipCode
            End If

            If oCustomer.PhoneNumber IsNot Nothing Then
                DirectLine.Text = oCustomer.PhoneNumber
            End If

            If oCustomer.Extension IsNot Nothing Then
                DirectLineExt.Text = oCustomer.Extension
            End If

            If oCustomer.BusinessClass IsNot Nothing Then
                sSelIndustry = oCustomer.BusinessClass.BusinessClassID.ToString()
            Else
                sSelIndustry = oCustomer.BusinessClassReference.ForeignKey().ToString()
            End If

        End If

    End Sub



    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

        Dim oCustomer As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name)

        If oCustomer IsNot Nothing Then

            Dim custID As Integer = oCustomer.CustomerID
            Dim custUID As String = oCustomer.Username
            Dim custPWD As String = oCustomer.Password
            Dim custFirstName As String = FirstName.Text
            Dim midInt As String = MI.Text
            Dim custLastName As String = LastName.Text
            Dim emailAddress As String = oCustomer.EmailAddress
            Dim phone As String = DirectLine.Text
            Dim phoneExt As String = DirectLineExt.Text
            Dim mobile As String = ""
            Dim fax As String = ""
            Dim companyPhone As String = CompanyLine.Text
            Dim companyPhoneExt As String = CompanyLineExt.Text
            Dim custAddress1 As String = Address1.Text
            Dim custAddress2 As String = Address2.Text
            Dim custCity As String = City.Text
            Dim custState As String = USAStatesDropDown.SelectedValue
            Dim custZip As String = PostalCode.Text
            Dim custCompanyName As String = Company.Text
            Dim businessClassID As Integer = Industry.SelectedValue

            Dim bUpdated As Boolean = Taradel.CustomerDataSource.Update(custID, custUID, custPWD, custFirstName, midInt, custLastName, emailAddress, phone, phoneExt, _
                                            mobile, fax, companyPhone, companyPhoneExt, custAddress1, custAddress2, custCity, custState, custZip, False, True, custCompanyName, businessClassID)

            If bUpdated Then

                Profile.FirstName = FirstName.Text
                Profile.LastName = LastName.Text

                DisabledFields()
                pnlSuccess.Visible = True
                lblSuccess.Text = "Thank you!  Your account is now complete.  Please click Continue to Order at the bottom of the page."
                pnlError.Visible = False
                pnlGreeting.Visible = False
                btnContinue.Visible = True

            Else
                pnlSuccess.Visible = False
                pnlError.Visible = True
                lblError.Text = "There was a problem updating your account information."
            End If

        Else

            pnlSuccess.Visible = False
            pnlError.Visible = True
            lblError.Text = "There was a problem loading your account record."

        End If

    End Sub



    Protected Sub btnContinue_Click(sender As Object, e As System.EventArgs) Handles btnContinue.Click
        Response.Redirect("~/Step3-Checkout.aspx")
    End Sub



    Protected Sub Industry_DataBound(sender As Object, e As System.EventArgs) Handles Industry.DataBound
        Dim oItem As ListItem = Industry.Items.FindByValue(sSelIndustry)
        If oItem IsNot Nothing Then
            Industry.ClearSelection()
            oItem.Selected = True
        End If
    End Sub



    Protected Sub DisabledFields()

        FirstName.Enabled = False
        LastName.Enabled = False
        Address1.Enabled = False
        Address2.Enabled = False
        City.Enabled = False
        PostalCode.Enabled = False
        DirectLine.Enabled = False
        DirectLineExt.Enabled = False
        CompanyLine.Enabled = False
        CompanyLineExt.Enabled = False
        Industry.Enabled = False
        btnSave.Visible = False
        btnContinue.Visible = True
        Company.Enabled = False
        MI.Enabled = False
        USAStatesDropDown.enabledState = False

        'Strip away styles
        FirstName.CssClass = String.Empty
        LastName.CssClass = String.Empty
        Address1.CssClass = String.Empty
        Address2.CssClass = String.Empty
        City.CssClass = String.Empty
        PostalCode.CssClass = String.Empty
        DirectLine.CssClass = String.Empty
        DirectLineExt.CssClass = String.Empty
        CompanyLine.CssClass = String.Empty
        CompanyLineExt.CssClass = String.Empty
        Industry.CssClass = String.Empty
        btnSave.CssClass = String.Empty
        Company.CssClass = String.Empty
        MI.CssClass = String.Empty
        USAStatesDropDown.cssClass = String.Empty

        'make smaller
        FirstName.CssClass = "form-control input-sm"
        LastName.CssClass = "form-control input-sm"
        Address1.CssClass = "form-control input-sm"
        Address2.CssClass = "form-control input-sm"
        City.CssClass = "form-control input-sm"
        PostalCode.CssClass = "form-control input-sm"
        DirectLine.CssClass = "form-control input-sm"
        DirectLineExt.CssClass = "form-control input-sm"
        CompanyLine.CssClass = "form-control input-sm"
        CompanyLineExt.CssClass = "form-control input-sm"
        Industry.CssClass = "form-control input-sm"
        btnSave.CssClass = "form-control input-sm"
        Company.CssClass = "form-control input-sm"
        MI.CssClass = "form-control input-sm"
        USAStatesDropDown.cssClass = "form-control input-sm"


    End Sub



End Class
