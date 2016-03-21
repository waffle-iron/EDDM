<%@ WebHandler Language="VB" Class="GetProductTemplates" %>

Imports System
Imports System.Web
Imports System.Collections.Generic

Public Class GetProductTemplates : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sResult As String = ""
        
        Dim WLProductId As Integer = appxCMS.Util.Querystring.GetInteger("id")
        Dim Filter As String = appxCMS.Util.Querystring.GetString("industryid")
        
        Dim IndustryId As Integer = 0
        Dim BusinessLineId As Integer = 0
        
        If Filter.Contains("-") Then
            Dim aFilter() As String = Filter.Split("-")
            Integer.TryParse(aFilter(0), IndustryId)
            Integer.TryParse(aFilter(1), BusinessLineId)
        Else
            Integer.TryParse(Filter, IndustryId)
        End If
        
        Dim PageNumber As Integer = appxCMS.Util.Querystring.GetInteger("pagenumber")
        PageNumber = PageNumber + 1
        Dim TemplateSizeId As Integer = Taradel.WLUtil.GetTemplateSize(WLProductId)
        If TemplateSizeId > 0 Then
            Dim oSizes As New List(Of Integer)
            oSizes.Add(TemplateSizeId)
            
            Using oAPI As New TemplateCode.TemplateAPIClient
                If BusinessLineId > 0 Then
                    Dim oRequest As New TemplateCode.GetTemplatesRequest(appxCMS.Util.CMSSettings.GetSiteId, BusinessLineId, oSizes, PageNumber, 12)
                    Dim oResponse As TemplateCode.GetTemplatesResponse = oAPI.GetTemplates(oRequest)
                    Dim oResult As TemplateCode.TemplateList = oResponse.GetTemplatesResult
                    sResult = appxCMS.Util.JavaScriptSerializer.Serialize(Of TemplateCode.TemplateList)(oResult)
                ElseIf IndustryId > 0 Then
                    Dim oRequest As New TemplateCode.GetIndustryTemplatesRequest(appxCMS.Util.CMSSettings.GetSiteId, IndustryId, oSizes, PageNumber, 12)
                    Dim oResponse As TemplateCode.GetIndustryTemplatesResponse = oAPI.GetIndustryTemplates(oRequest)
                    Dim oResult As TemplateCode.TemplateList = oResponse.GetIndustryTemplatesResult
                    sResult = appxCMS.Util.JavaScriptSerializer.Serialize(Of TemplateCode.TemplateList)(oResult)
                Else
                    Dim oRequest As New TemplateCode.GetRandomTemplatesBySizeRequest(appxCMS.Util.CMSSettings.GetSiteId, TemplateSizeId, 12)
                    Dim oResponse As TemplateCode.GetRandomTemplatesBySizeResponse = oAPI.GetRandomTemplatesBySize(oRequest)
                    Dim oTemplates As List(Of TemplateCode.Template1) = oResponse.GetRandomTemplatesBySizeResult
                    Dim oResult As New TemplateCode.TemplateList
                    oResult.CurrentPage = 1
                    oResult.PageSize = TemplateSizeId
                    oResult.Templates = oTemplates
                    oResult.TotalPages = 1
                    oResult.TotalRecords = 12
                    sResult = appxCMS.Util.JavaScriptSerializer.Serialize(Of TemplateCode.TemplateList)(oResult)
                End If
            End Using
        End If

        context.Response.Clear()
        context.Response.ContentType = "application/json"
        context.Response.Write(sResult)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class