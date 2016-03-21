
Partial Class CCustom_TemplateDirector
    Inherits CLibraryBase

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim iSiteId As Integer = appxCMS.Util.CMSSettings.GetSiteId

        Dim bHasCustom As Boolean = False
        Using oTemplates As New TemplateCode.TemplateAPIClient
            Dim oSiteReq As New TemplateCode.GetMyIndustryRequest(iSiteId)
            Dim oSiteResponse As TemplateCode.GetMyIndustryResponse = oTemplates.GetMyIndustry(oSiteReq)
            If oSiteResponse.GetMyIndustryResult.Count > 0 Then
                bHasCustom = True
            End If
        End Using

        If bHasCustom Then
            Response.Redirect("~/CustomTemplates.aspx")
        End If
    End Sub
End Class
