
Partial Class CCustom_TemplateBusinessLines
    Inherits CLibraryBase

    Protected bBuilt As Boolean = False

    Private _IndustryId As Integer = 0
    Public Property IndustryId As Integer
        Get
            Return _IndustryId
        End Get
        Set(value As Integer)
            Dim bChange As Boolean = False
            If _IndustryId <> value Then
                bChange = True
            End If
            _IndustryId = value

            If bBuilt Then
                '-- Control has already been rendered

                If bChange Then
                    '-- Reset our industry
                    _Industry = Nothing
                    bLoaded = False
                End If
                BuildControl()
            End If
        End Set
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

    Public Property IntroText As String
        Get
            Return lIntroText.Text
        End Get
        Set(value As String)
            lIntroText.Text = value
        End Set
    End Property

    Public Property FullText As String
        Get
            Return lFullText.Text
        End Get
        Set(value As String)
            lFullText.Text = value
        End Set
    End Property

    Protected Overrides Sub BuildControl()
        If Me.IndustryId > 0 Then
            Using oTemplates As New TemplateCode.TemplateAPIClient
                Dim oRequest As New TemplateCode.GetBusinessLinesRequest(appxCMS.Util.CMSSettings.GetSiteId, Me.IndustryId)
                Dim oResponse As TemplateCode.GetBusinessLinesResponse = oTemplates.GetBusinessLines(oRequest)
                lvBusinessLines.DataSource = oResponse.GetBusinessLinesResult
                lvBusinessLines.DataBind()
            End Using
        End If
        bBuilt = True
    End Sub

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Protected Sub lvBusinessLines_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvBusinessLines.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oItem As ListViewDataItem = e.Item
            Dim oBusinessLine As TemplateCode.BusinessLine = DirectCast(oItem.DataItem, TemplateCode.BusinessLine)
            Dim hplBusinessLine As HyperLink = DirectCast(e.Item.FindControl("hplBusinessLine"), HyperLink)
            hplBusinessLine.NavigateUrl = appxCMS.SEO.Rewrite.BuildLink(Me.Industry.Name & "-" & oBusinessLine.Name, Me.IndustryId & "-" & oBusinessLine.BusinessLineId, "templatebusinessline")
        End If
    End Sub
End Class
