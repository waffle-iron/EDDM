
Partial Class admin_Site
    Inherits adminBase

    Protected ReadOnly Property SiteId As Integer
        Get
            Return QStringToInt("siteid")
        End Get
    End Property

    Protected SelectedTab As Integer = 0

    Private _Site As appxCMS.Site = Nothing
    Dim bSet As Boolean = False
    Protected ReadOnly Property CurSite As appxCMS.Site
        Get
            If _Site Is Nothing And Not bSet Then
                _Site = appxCMS.SiteDataSource.GetSite(Me.SiteId)
                bSet = True
            End If
            Return _Site
        End Get
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Me.CurSite IsNot Nothing Then
            Name.Text = Me.CurSite.Name
            Address1.Text = Me.CurSite.Address1
            Address2.Text = Me.CurSite.Address2
            City.Text = Me.CurSite.City
            State.SelectedValue = Me.CurSite.State
            ZipCode.Text = Me.CurSite.ZipCode
            Phone.Text = Me.CurSite.PhoneNumber
            TollFreeNumber.Text = Me.CurSite.TollFreeNumber
            Fax.Text = Me.CurSite.FaxNumber
            Email.Text = Me.CurSite.EmailAddress
            Active.Checked = Me.CurSite.Active
            If Me.CurSite.EnableSignin.HasValue Then
                EnableSignin.Checked = Me.CurSite.EnableSignin.Value
            End If
            If Me.CurSite.EnableSignup.HasValue Then
                EnableSignup.Checked = Me.CurSite.EnableSignup.Value
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim sMsg As String = ""
        Dim bRet As Boolean = appxCMS.SiteDataSource.Update(Me.SiteId, Name.Text, Address1.Text, Address2.Text, City.Text, State.SelectedValue, ZipCode.Text, Phone.Text, TollFreeNumber.Text, Fax.Text, Email.Text, Active.Checked, EnableSignin.Checked, EnableSignup.Checked, sMsg)
        If bRet Then
            lMsg.Text = UpdateStatusMsg("Your changes have been saved.")
        Else
            lMsg.Text = UpdateStatusMsg("There was an error saving your changes: " & sMsg, True)
        End If
    End Sub
End Class
