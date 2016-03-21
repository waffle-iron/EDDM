
Imports System.Data
Imports System.Data.SqlClient

Partial Class CCustom_TemplatePagedList
    Inherits CLibraryBase

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

    Private _pageSizeId As Integer = 20
    Public Property PageSizeId As Integer
        Get
            Return _pageSizeId
        End Get
        Set(value As Integer)
            _pageSizeId = value
        End Set
    End Property

    Private _pagerNavigateUrl As String = ""
    Public Property PagerNavigateUrl As String
        Get
            Return _pagerNavigateUrl
        End Get
        Set(value As String)
            _pagerNavigateUrl = value
        End Set
    End Property

    Private _showName As Boolean = False
    Public Property ShowName As Boolean
        Get
            Return _showName
        End Get
        Set(value As Boolean)
            _showName = value
        End Set
    End Property

    Private _Industry As TemplateCode.Industry = Nothing
    Public Property Industry As TemplateCode.Industry
        Get
            If _Industry Is Nothing And Me.IndustryId > 0 Then
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetIndustryAndBusinessLinesRequest(Me.IndustryId, appxCMS.Util.CMSSettings.GetSiteId)
                    Dim oResponse As TemplateCode.GetIndustryAndBusinessLinesResponse = oTemplates.GetIndustryAndBusinessLines(oRequest)
                    _Industry = oResponse.GetIndustryAndBusinessLinesResult
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

    Private _PageSize As TemplateCode.TemplateSize = Nothing
    Public Property PageSize As TemplateCode.TemplateSize
        Get
            If _pageSizeId > 0 And _PageSize Is Nothing Then
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetTemplateSizeRequest(Me.PageSizeId)
                    Dim oResponse As TemplateCode.GetTemplateSizeResponse = oTemplates.GetTemplateSize(oRequest)
                    _PageSize = oResponse.GetTemplateSizeResult
                    If _PageSize IsNot Nothing Then
                        PageSizeId = _PageSize.TemplateSizeId
                    End If
                End Using
            End If
            Return _PageSize
        End Get
        Set(value As TemplateCode.TemplateSize)
            _PageSize = value
            If _PageSize IsNot Nothing Then
                PageSizeId = value.TemplateSizeId
            End If
        End Set
    End Property

    Private _CurPage As Integer = 1
    Public Property CurPage As Integer
        Get
            Return _CurPage
        End Get
        Set(value As Integer)
            _CurPage = value
        End Set
    End Property

    Private _PageCount As Integer = 20
    Public Property PageCount As Integer
        Get
            Return _PageCount
        End Get
        Set(value As Integer)
            _PageCount = value
        End Set
    End Property

    Private _TotalPages As Integer = 0
    Public ReadOnly Property TotalPages As Integer
        Get
            Return _TotalPages
        End Get
    End Property

    Private _TotalRecords As Integer = 0
    Public ReadOnly Property TotalRecords As Integer
        Get
            Return _TotalRecords
        End Get
    End Property

    Protected sTemplateServerHost As String = ""
    Protected sTemplateSubHost As String = ""
    Protected sTemplateTLD As String = ""

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Public Sub DoRender()
        BuildControl()
    End Sub

    Protected oTemplates As TemplateCode.TemplateList = Nothing

    Protected Overrides Sub BuildControl()
        Dim type As String = "EDDM"
        Dim loc As String = "loc"
        If Not String.IsNullOrEmpty(Request.QueryString("loc")) Then
            loc = Request.QueryString("loc").ToString()
            'Response.Write("loc=" & loc & " index " & loc.IndexOf("Addressed").ToString())
            If loc.IndexOf("Addressed") >= 0 Then
                type = "Addressed"
            End If
        End If

        Dim lstNewTemplates As New List(Of NewTemplate1)

        sTemplateServerHost = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
        If Not String.IsNullOrEmpty(sTemplateServerHost) Then
            Dim aHost As String() = sTemplateServerHost.Split(".")
            If aHost.Length = 3 Then
                sTemplateSubHost = aHost(0)
                sTemplateTLD = aHost(1) & "." & aHost(2)
            End If
        End If

        Dim SiteId As Integer = appxCMS.Util.CMSSettings.GetSiteId
        Using oAPI As New TemplateCode.TemplateAPIClient
            If Me.BusinessLineId > 0 Then
                Dim oRequest As New TemplateCode.GetTemplatesRequest(SiteId, Me.BusinessLineId, Taradel.WLUtil.GetTemplateSizes, Me.CurPage, Me.PageCount)
                Dim oResponse As TemplateCode.GetTemplatesResponse = oAPI.GetTemplates(oRequest)
                oTemplates = oResponse.GetTemplatesResult
            ElseIf Me.IndustryId > 0 Then
                Dim oRequest As New TemplateCode.GetIndustryTemplatesRequest(SiteId, Me.IndustryId, Taradel.WLUtil.GetTemplateSizes, Me.CurPage, Me.PageCount)
                Dim oResponse As TemplateCode.GetIndustryTemplatesResponse = oAPI.GetIndustryTemplates(oRequest)
                oTemplates = oResponse.GetIndustryTemplatesResult
            Else
                Exit Sub
            End If
        End Using

        If oTemplates IsNot Nothing Then
            For Each oTemp As TemplateCode.Template1 In oTemplates.Templates
                Dim newTemplate As New NewTemplate1()
                newTemplate.BackImage = oTemp.BackImage
                newTemplate.BusinessLine = oTemp.BusinessLine
                newTemplate.BusinessLineId = oTemp.BusinessLineId
                newTemplate.Description = oTemp.Description
                newTemplate.FoldType = oTemp.FoldType
                newTemplate.FrontImage = oTemp.FrontImage
                newTemplate.InsideImage = oTemp.InsideImage
                newTemplate.PageSize = oTemp.PageSize
                newTemplate.ProductType = oTemp.ProductType
                newTemplate.Summary = oTemp.Summary
                newTemplate.TemplateId = oTemp.TemplateId
                newTemplate.TemplateSizeId = oTemp.TemplateSizeId
                newTemplate.USelectID = RetrieveUSelectID(newTemplate.TemplateSizeId)
                newTemplate.Name = oTemp.Name '& "| PageType:" & type & "|" & loc & "|" & newTemplate.USelectID.ToString()

                'oTemp.Name = oTemp.Name & "| USelectID:" & newTemplate.USelectID
                If type = "Addressed" And newTemplate.USelectID = "5" Or newTemplate.USelectID = "6" Then
                    lstNewTemplates.Add(newTemplate)
                End If

                If type = "EDDM" And newTemplate.USelectID = 1 Then
                    lstNewTemplates.Add(newTemplate)
                End If


                'Dim oTags As List(Of TemplateCode.Tag) = TemplateCode.'TagDataSource.GetTagsForTemplate(Me.TemplateId)
                'Dim oTags As List(Of TemplateServerCore.Tag) = TemplateServerCore.TagDataSource.GetTagsForTemplate(Me.TemplateId)
                'lvTags.DataSource = oTags
                'lvTags.DataBind()
                ''not sure where else oTemp.ProductType is used 
            Next

            Me._TotalPages = oTemplates.TotalPages
            Me._TotalRecords = lstNewTemplates.Count 'oTemplates.TotalRecords
            'LINQ could go here
            lvTemplates.DataSource = lstNewTemplates 'oTemplates.Templates
            lvTemplates.DataBind()
        End If

        'Me._TotalPages = oTemplates.TotalPages
        'Me._TotalRecords = oTemplates.TotalRecords
        'lvTemplates.DataSource = oTemplates.Templates
        'lvTemplates.DataBind()
    End Sub

    Dim iHost As Integer = 1

    Protected Sub lvTemplates_DataBound(sender As Object, e As System.EventArgs) Handles lvTemplates.DataBound
        Dim sName As String = "Templates"
        If Me.TotalRecords = 1 Then
            sName = "Template"
        End If
        Dim sDesc As String = "<b>" & Me.TotalRecords.ToString("N0") & " " & sName & ":</b> "
        Dim oPager As UserControls_DataPager = DirectCast(lvTemplates.FindControl("dpTop"), UserControls_DataPager)
        If oPager IsNot Nothing Then
            oPager.CurPage = Me.CurPage
            If String.IsNullOrEmpty(Me.PagerNavigateUrl) Then
                Me.PagerNavigateUrl = appxCMS.SEO.Rewrite.BuildLink(Me.Industry.Name, Me.IndustryId, "templateindustry")
            End If
            oPager.NavigatePage = Me.PagerNavigateUrl
            oPager.ResultCount = Me.TotalRecords
            oPager.PageSize = Me.PageCount
            oPager.ResultDescription = sDesc
        End If
    End Sub

    Protected Sub lvTemplates_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvTemplates.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item
            Dim oTemplate As TemplateCode.Template1 = DirectCast(oDItem.DataItem, TemplateCode.Template1)

            Dim imgTemplate As Image = DirectCast(e.Item.FindControl("imgTemplate"), Image)
            imgTemplate.ImageUrl = sTemplateSubHost & iHost & "." & sTemplateTLD & "/templates/icon/" & oTemplate.FrontImage
            'imgTemplate.Style.Add("max-height", "125px;")


            Dim hplTemplate As HyperLink = DirectCast(e.Item.FindControl("hplTemplate"), HyperLink)
            hplTemplate.NavigateUrl = appxCMS.SEO.Rewrite.BuildLink("Template" & oTemplate.TemplateId, oTemplate.TemplateId, "template")

            iHost = iHost + 1
            If iHost > 4 Then
                iHost = 1
            End If
        End If
    End Sub

    Public Class NewTemplate1
        Inherits TemplateCode.Template1

        Private _USelectID As String
        Public Property USelectID() As String
            Get
                Return _USelectID
            End Get
            Set(ByVal value As String)
                _USelectID = value
            End Set
        End Property

    End Class

    Public Function RetrieveUSelectID(TemplateSizeId As Integer) As String
        Dim returnThis As String = String.Empty

        Dim connectString As String = ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString.ToString()
        Dim connectionObj As New SqlConnection(connectString)

        Dim sql As String = "SELECT TOP 1 USelectID FROM USelectProductConfiguration WHERE ProductID IN (SELECT ProductID FROM pnd_Product where productid IN (select TOP 1 ProductID from pnd_product where TemplateSizeId = " & TemplateSizeId.ToString() & "))"

        Dim command As New SqlCommand()
        command.CommandType = CommandType.Text
        command.CommandText = sql
        command.Connection = connectionObj
        Try
            connectionObj.Open()
            returnThis = command.ExecuteScalar().ToString()
        Catch objException As Exception
            Response.Write(objException.Message.ToString())
            Response.Write("__________________________________________")
            Response.Write(objException.StackTrace.ToString())
        End Try

        Return returnThis
    End Function
End Class
