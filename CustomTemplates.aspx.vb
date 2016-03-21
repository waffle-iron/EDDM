Imports System.Linq

Partial Class CustomTemplates

    Inherits appxCMS.PageBase

    Protected ReadOnly Property CurPage As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("pg")
        End Get
    End Property

    Private bHasCustom As Boolean = False
    Private SiteId As Integer = 0
    Private _IndustryId As Integer = 0
    Protected ReadOnly Property IndustryId As Integer
        Get
            If _IndustryId = 0 Then
                SiteId = appxCMS.Util.CMSSettings.GetSiteId
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oSiteReq As New TemplateCode.GetMyIndustryRequest(SiteId)
                    Dim oSiteResponse As TemplateCode.GetMyIndustryResponse = oTemplates.GetMyIndustry(oSiteReq)
                    If oSiteResponse.GetMyIndustryResult.Count > 0 Then
                        bHasCustom = True

                        _IndustryId = oSiteResponse.GetMyIndustryResult.FirstOrDefault.IndustryId
                    End If
                End Using
            End If
            Return _IndustryId
        End Get
    End Property

    'Protected ReadOnly Property IndustryId As Integer
    '    Get
    '        If _IndustryId = 0 Then
    '            SiteId = appxCMS.Util.CMSSettings.GetSiteId
    '            Using oTemplates As New TemplateCode.TemplateAPIClient
    '                Dim oSiteReq As New TemplateCode.GetMyIndustryRequest(SiteId)
    '                Dim oSiteResponse As TemplateCode.GetMyIndustryResponse = oTemplates.GetMyIndustry(oSiteReq)
    '                If oSiteResponse.GetMyIndustryResult.Count > 0 Then
    '                    bHasCustom = True

    '                    _IndustryId = oSiteResponse.GetMyIndustryResult.FirstOrDefault.IndustryId
    '                End If
    '            End Using
    '        End If
    '        Return _IndustryId
    '    End Get
    'End Property




    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        '-- Check to see if they have custom templates, otherwise, redirect to main templates page
        Dim iInd As Integer = IndustryId

        If Not bHasCustom Then
            Response.Redirect("~/Templates")
        End If

        Dim oTemplateUtil = New TemplateUtility(iInd)
        If oTemplateUtil.RequireAuth AndAlso Not Request.IsAuthenticated Then
            phAuth.Visible = True
            phTemplates.Visible = False
            Exit Sub
        End If

        phTemplates.Visible = True

        Search.IndustryId = Me.IndustryId


        '-- Now we can build the page
        PagedList.IndustryId = iInd
        PagedList.PageCount = 21
        If oTemplateUtil.BusinessLineId > 0 Then
            PagedList.BusinessLineId = oTemplateUtil.BusinessLineId
            Search.Visible = False
            PagedList.PageCount = 999
        End If
        PagedList.CurPage = Me.CurPage
        PagedList.PagerNavigateUrl = Page.AppRelativeVirtualPath
        PagedList.DoRender()

        'Set Page Header
        BuildPageHeader()


        Dim message As String = SiteUtility.GetStringResourceValue(SiteId, "CustomTemplatesMsg")
        If Not (String.IsNullOrEmpty(message)) Then
            If message.ToLower() <> "undefined" Then
                litCustomMessage.Text = message
            End If
        End If



    End Sub



    Private Sub BuildPageHeader()


        'Addtional Site data.  Build SiteDetails Obj
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteId)


        'Set Page Header control
        If Not (SiteDetails.UseRibbonBanners) Then
            PageHeader.headerType = "simple"
            PageHeader.simpleHeader = "Select a pre-made template"
        Else
            PageHeader.headerType = "partial"
            PageHeader.mainHeader = "Templates"
            PageHeader.subHeader = "Select a pre-made template"
        End If

    End Sub


End Class
