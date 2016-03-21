
Partial Class CCustom_TemplateSizes
    Inherits CLibraryBase

    Protected bBuilt As Boolean = False

    Private _BusinessLineId As Integer = 0
    Public Property BusinessLineId As Integer
        Get
            Return _BusinessLineId
        End Get
        Set(value As Integer)
            _BusinessLineId = value
        End Set
    End Property

    Private _IndustryId As Integer = 0
    Public Property IndustryId As Integer
        Get
            Return _IndustryId
        End Get
        Set(value As Integer)
            _IndustryId = value
        End Set
    End Property

    Private _Industry As TemplateCode.Industry = Nothing
    Public Property Industry As TemplateCode.Industry
        Get
            If _Industry Is Nothing And _IndustryId > 0 Then
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetIndustryRequest(Me.IndustryId, appxCMS.Util.CMSSettings.GetSiteId)
                    Dim oResponse As TemplateCode.GetIndustryResponse = oTemplates.GetIndustry(oRequest)
                    _Industry = oResponse.GetIndustryResult
                    If _Industry IsNot Nothing Then
                        IndustryId = _Industry.IndustryId
                    End If
                End Using
            End If
            Return _Industry
        End Get
        Set(value As TemplateCode.Industry)
            _Industry = value
            If value IsNot Nothing Then
                IndustryId = value.IndustryId
            End If
        End Set
    End Property

    Private _BusinessLine As TemplateCode.BusinessLine = Nothing
    Public Property BusinessLine As TemplateCode.BusinessLine
        Get
            If _BusinessLine Is Nothing And _BusinessLineId > 0 Then
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetBusinessLineRequest(appxCMS.Util.CMSSettings.GetSiteId, Me.BusinessLineId)
                    Dim oResponse As TemplateCode.GetBusinessLineResponse = oTemplates.GetBusinessLine(oRequest)
                    _BusinessLine = oResponse.GetBusinessLineResult
                    If _BusinessLine IsNot Nothing Then
                        BusinessLineId = _BusinessLine.BusinessLineId
                    End If
                End Using
            End If
            Return _BusinessLine
        End Get
        Set(value As TemplateCode.BusinessLine)
            _BusinessLine = value
            If _BusinessLine IsNot Nothing Then
                BusinessLineId = value.BusinessLineId
            End If
        End Set
    End Property

    Protected Overrides Sub BuildControl()
        If Me.BusinessLineId > 0 Then
            Using oTemplates As New TemplateCode.TemplateAPIClient
                Dim oRequest As New TemplateCode.GetTemplateSizesRequest(appxCMS.Util.CMSSettings.GetSiteId, Me.BusinessLineId)
                Dim oResponse As TemplateCode.GetTemplateSizesResponse = oTemplates.GetTemplateSizes(oRequest)
                lvSizes.DataSource = oResponse.GetTemplateSizesResult
                lvSizes.DataBind()
            End Using
        End If
        bBuilt = True
    End Sub

    Public Sub DoRender()
        BuildControl()
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Protected Sub lvSizes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvSizes.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oItem As ListViewDataItem = e.Item
            Dim oTemplateSize As TemplateCode.TemplateSize = DirectCast(oItem.DataItem, TemplateCode.TemplateSize)
            Dim hplSize As HyperLink = DirectCast(e.Item.FindControl("hplSize"), HyperLink)
            hplSize.NavigateUrl = appxCMS.SEO.Rewrite.BuildLink(Me.Industry.Name & "-" & Me.BusinessLine.Name & "-" & oTemplateSize.Name, Me.IndustryId & "-" & Me.BusinessLineId & "-" & oTemplateSize.TemplateSizeId, "templatepagesize")
        End If
    End Sub
End Class
