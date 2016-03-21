
Partial Class vpage_templateindustry
    Inherits appxCMS.PageBase

    Protected ReadOnly Property IndustryId As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("id")
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

    Protected dRequest As Hashtable = RebuildQuerystring(GetRequestedURL(), False)

    Protected ReadOnly Property CurPage As Integer
        Get
            Dim iPg As Integer = 1
            If dRequest.ContainsKey("pg") Then
                Integer.TryParse(dRequest("pg"), iPg)
            End If
            'Dim iPg As Integer = appxCMS.Util.Querystring.GetInteger("pg")
            'If iPg = 0 Then iPg = 1
            Return iPg
        End Get
    End Property

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        Dim oMeta As New HtmlMeta
        oMeta.Name = "robots"
        oMeta.Content = "NOINDEX, NOFOLLOW"
        Page.Header.Controls.Add(oMeta)

        If Me.Industry IsNot Nothing Then

            'BannerFull.FullText = Me.Industry.Name

            litBusinessLine.Text = Me.Industry.Name
            TemplateBusinessLines.IndustryId = Me.IndustryId

            PagedTemplateList.IndustryId = Me.IndustryId
            PagedTemplateList.PageCount = 21
            PagedTemplateList.CurPage = Me.CurPage
            PagedTemplateList.DoRender()

            Dim sTitle As String = "Customizable Every Door Direct Mail&trade; Templates for " & Me.Industry.Name
            If Me.CurPage > 1 Then
                sTitle = sTitle & ", Page " & Me.CurPage
            End If
            Page.Title = sTitle

        End If

        Dim type As String = "EDDM"
        Dim loc As String = "loc"
        If Not String.IsNullOrEmpty(Request.QueryString("loc")) Then
            loc = Request.QueryString("loc").ToString()
            'Response.Write("loc=" & loc & " index " & loc.IndexOf("Addressed").ToString())
            If loc.IndexOf("Addressed") >= 0 Then
                type = "Addressed"
            End If
        End If
        If type.Equals("addressed", StringComparison.OrdinalIgnoreCase) Then
            TemplateBusinessLines.Visible = False
        End If
    End Sub
End Class
