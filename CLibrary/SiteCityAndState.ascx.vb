﻿
Partial Class CLibrary_SiteCityAndState
    Inherits CLibraryBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim oSite As appxCMS.Site = appxCMS.SiteDataSource.GetSite
        If oSite IsNot Nothing Then
            lName.Text = oSite.City & ", " & oSite.State
        End If
    End Sub
End Class