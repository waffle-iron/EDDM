﻿
Partial Class vpage_templatebusinessline
    Inherits appxCMS.PageBase

    Protected ReadOnly Property IndustryId As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("id")
        End Get
    End Property

    Protected ReadOnly Property BusinessLineId As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("blid")
        End Get
    End Property

    Private _Industry As TemplateCode.Industry = Nothing
    Private bLoaded As Boolean = False
    Protected ReadOnly Property Industry As TemplateCode.Industry
        Get
            If _Industry Is Nothing AndAlso Not bLoaded Then
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetIndustryRequest(Me.IndustryId, appxCMS.Util.CMSSettings.GetSiteId)
                    Dim oResponse As TemplateCode.GetIndustryResponse = oTemplates.GetIndustry(oRequest)
                    _Industry = oResponse.GetIndustryResult
                End Using
                bLoaded = True
            End If
            Return _Industry
        End Get
    End Property

    Private _BusinessLine As TemplateCode.BusinessLine = Nothing
    Private bBLLoaded As Boolean = False
    Protected ReadOnly Property BusinessLine As TemplateCode.BusinessLine
        Get
            If _BusinessLine Is Nothing AndAlso Not bBLLoaded Then
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetBusinessLineRequest(appxCMS.Util.CMSSettings.GetSiteId, Me.BusinessLineId)
                    Dim oResponse As TemplateCode.GetBusinessLineResponse = oTemplates.GetBusinessLine(oRequest)
                    _BusinessLine = oResponse.GetBusinessLineResult
                End Using
                bBLLoaded = True
            End If
            Return _BusinessLine
        End Get
    End Property

    Protected ReadOnly Property CurPage As Integer
        Get
            Dim iPg As Integer = appxCMS.Util.Querystring.GetInteger("pg")
            If iPg = 0 Then iPg = 1
            Return iPg
        End Get
    End Property

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim oMeta As New HtmlMeta
        oMeta.Name = "robots"
        oMeta.Content = "NOINDEX, NOFOLLOW"
        Page.Header.Controls.Add(oMeta)

        If Me.Industry IsNot Nothing AndAlso Me.BusinessLine IsNot Nothing Then

            PagedTemplateList.Industry = Me.Industry
            PagedTemplateList.BusinessLine = Me.BusinessLine
            PagedTemplateList.PageCount = 999
            PagedTemplateList.CurPage = Me.CurPage
            PagedTemplateList.DoRender()

            hplIndustry.NavigateUrl = appxCMS.SEO.Rewrite.BuildLink(Me.Industry.Name, Me.Industry.IndustryId, "templateindustry")
            lIndustryName.Text = Me.Industry.Name

            TemplateBusinessLines.IndustryId = Me.IndustryId

            litBusinessLine.text = Me.BusinessLine.Name
            litBusinessLineSmall.text = Me.BusinessLine.Name

            'BannerFull.FullText = Me.Industry.Name & " &raquo; " & Me.BusinessLine.Name

            Page.Title = "Customizable Every Door Direct Mail&trade; Templates for " & Me.BusinessLine.Name & " in " & Me.Industry.Name

        End If
    End Sub
End Class
