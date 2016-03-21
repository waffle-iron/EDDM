<%@ WebHandler Language="VB" Class="SearchProductTemplates" %>

Imports System
Imports System.Web
Imports System.Collections.Generic

Public Class SearchProductTemplates : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sResult As String = ""
        
        Dim WLProductId As Integer = appxCMS.Util.Querystring.GetInteger("id")
        Dim Filter As String = appxCMS.Util.Querystring.GetString("keyword")
        Dim PageNumber As Integer = appxCMS.Util.Querystring.GetInteger("pagenumber")
        PageNumber = PageNumber + 1
        Dim TemplateSizeId As Integer = Taradel.WLUtil.GetTemplateSize(WLProductId)
        
        If TemplateSizeId > 0 Then
            Dim oSizes As New List(Of Integer)
            oSizes.Add(TemplateSizeId)
            
            Using oAPI As New TemplateCode.TemplateAPIClient               
                Dim oRequest As New TemplateCode.FindTemplatesRequest(appxCMS.Util.CMSSettings.GetSiteId, Filter, oSizes, PageNumber, 12)
                Dim oResponse As TemplateCode.FindTemplatesResponse = oAPI.FindTemplates(oRequest)
                Dim oResult As TemplateCode.TemplateList = oResponse.FindTemplatesResult
                sResult = appxCMS.Util.JavaScriptSerializer.Serialize(Of TemplateCode.TemplateList)(oResult)
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