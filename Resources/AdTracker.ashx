<%@ WebHandler Language="VB" Class="AdTracker" %>

Imports System
Imports System.Web
Imports System.IO

Public Class AdTracker : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sNavUrl As String = appxCMS.Util.Querystring.GetString("url")
        Dim ImpressionId As Long
        Dim sImpId As String = appxCMS.Util.Querystring.GetString("imp")
        Long.TryParse(sImpId, ImpressionId)

        '-- Do some exit logging that the ad was clicked here
        appxCMS.BannerDataSource.RecordClick(ImpressionId)
        
        '-- Redirect to the ad target
        context.Response.Redirect(sNavUrl)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class