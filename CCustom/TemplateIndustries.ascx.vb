Imports System.Collections.Generic

Partial Class CCustom_TemplateIndustries
    Inherits CLibraryBase

    Protected Overrides Sub BuildControl()
        Using oTemplates As New TemplateCode.TemplateAPIClient
            Dim oRequest As New TemplateCode.GetIndustriesRequest()
            Dim oResponse As TemplateCode.GetIndustriesResponse = oTemplates.GetIndustries(oRequest)
            Dim oIndustries As List(Of TemplateCode.Industry) = oResponse.GetIndustriesResult
            lvIndustries.DataSource = oIndustries
            lvIndustries.DataBind()
        End Using
    End Sub

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Public Sub DoRender()
        BuildControl()
    End Sub

    Protected Sub lvIndustries_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvIndustries.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oItem As ListViewDataItem = e.Item
            Dim oIndustry As TemplateCode.Industry = DirectCast(oItem.DataItem, TemplateCode.Industry)
            Dim hplIndustry As HyperLink = DirectCast(e.Item.FindControl("hplIndustry"), HyperLink)
            hplIndustry.NavigateUrl = appxCMS.SEO.Rewrite.BuildLink(oIndustry.Name, oIndustry.IndustryId, "templateindustry")
        End If
    End Sub
End Class
