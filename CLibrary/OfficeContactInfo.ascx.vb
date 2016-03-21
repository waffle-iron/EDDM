
Partial Class CCustom_OfficeContactInfo
    Inherits System.Web.UI.UserControl

    Private _SiteId As Integer = -1
    Public Property SiteId As Integer
        Get
            Return _SiteId
        End Get
        Set(value As Integer)
            _SiteId = value
        End Set
    End Property

    Public Property ShowOfficeName As Boolean
        Get
            Return phOfficeName.Visible
        End Get
        Set(value As Boolean)
            phOfficeName.Visible = value
        End Set
    End Property

    Private bControlBuilt As Boolean = False

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not bControlBuilt Then
            BuildControl()
        End If
    End Sub

    Public Sub BuildControl()
        bControlBuilt = True

        Dim iSiteId As Integer = Me.SiteId
        'If iSiteId < 0 Then

        'End If
        iSiteId = appxCMS.Util.CMSSettings.GetSiteId

        Dim oSite As appxCMS.Site = appxCMS.SiteDataSource.GetSite(iSiteId)
        If oSite IsNot Nothing Then
            lOfficeName.Text = oSite.Name
            If String.IsNullOrEmpty(oSite.Address1) Then
                phAddress.Visible = False
            Else
                lAddress1.Text = oSite.Address1
                If Not String.IsNullOrEmpty(oSite.Address2) Then
                    lAddress2.Text = oSite.Address2
                    pAddress2.Visible = True
                End If
                lCity.Text = oSite.City
                lState.Text = oSite.State
                lZip.Text = oSite.ZipCode
            End If
            If oSite.EmailAddress IsNot Nothing Then
                If Not String.IsNullOrEmpty(oSite.EmailAddress) Then
                    hplEmail.Text = oSite.EmailAddress
                    hplEmail.NavigateUrl = "mailto:" & oSite.EmailAddress
                    phEmail.Visible = True
                End If
            End If
            If oSite.PhoneNumber IsNot Nothing Then
                If Not String.IsNullOrEmpty(oSite.PhoneNumber) Then
                    lPhone.Text = oSite.PhoneNumber
                    phPhone.Visible = True
                End If
            End If
        Else
            Me.Visible = False
        End If
    End Sub
End Class
