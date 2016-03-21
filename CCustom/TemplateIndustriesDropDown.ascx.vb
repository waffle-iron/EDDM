Imports System.Collections.Generic

Partial Class CCustom_TemplateIndustriesDropDown
    Inherits CLibraryBase

    Protected Overrides Sub BuildControl()
        Using oTemplates As New TemplateCode.TemplateAPIClient
            Dim oRequest As New TemplateCode.GetIndustriesRequest()
            Dim oResponse As TemplateCode.GetIndustriesResponse = oTemplates.GetIndustries(oRequest)
            Dim oIndustries As List(Of TemplateCode.Industry) = oResponse.GetIndustriesResult
            Industry.DataValueField = "IndustryId"
            Industry.DataTextField = "Name"
            Industry.DataSource = oIndustries
            Industry.DataBind()
        End Using
    End Sub

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Public Sub DoRender()
        BuildControl()
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Dim pathForSearch As String = "EDDM"

        If Request.Url.ToString().IndexOf("Addressed") > 0 Then
            pathForSearch = "Addressed"
        End If

        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('#" & lnkChooseIndustry.ClientID & "').click(function(e) {")
        oJs.AppendLine("        e.preventDefault();")
        oJs.AppendLine("        var oSel = $('#" & Industry.ClientID & " option:selected');")
        oJs.AppendLine("        window.location = '/' + oSel.text().replace(' ', '-').replace('&', 'and') + '-templateindustry' + oSel.val() + '?loc=" & pathForSearch & "';")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        Page.ClientScript.RegisterClientScriptBlock(GetType(String), Me.ClientID & "Init", oJs.ToString, True)
    End Sub

    'Protected Sub lnkChooseIndustry_Click(sender As Object, e As System.EventArgs) Handles lnkChooseIndustry.Click
    '    Dim IndustryId As String = Industry.SelectedValue

    '    If Not String.IsNullOrEmpty(IndustryId) Then
    '        Dim sIndustry As String = Industry.SelectedItem.Text
    '        Response.Redirect(appxCMS.SEO.Rewrite.BuildLink(sIndustry, IndustryId, "templateindustry"))
    '    End If
    'End Sub

    ''Protected Sub lvIndustries_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvIndustries.ItemDataBound
    '    If e.Item.ItemType = ListViewItemType.DataItem Then
    '        Dim oItem As ListViewDataItem = e.Item
    '        Dim oIndustry As TemplateCode.Industry = DirectCast(oItem.DataItem, TemplateCode.Industry)
    '        Dim hplIndustry As HyperLink = DirectCast(e.Item.FindControl("hplIndustry"), HyperLink)
    '        hplIndustry.NavigateUrl = appxCMS.SEO.Rewrite.BuildLink(oIndustry.Name, oIndustry.IndustryId, "templateindustry")
    '    End If
    'End Sub
End Class
