<%@ WebHandler Language="VB" Class="ProDownload" %>

Imports System
Imports System.Web
Imports System.IO

Public Class ProDownload : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sDir As String = pageBase.QStringToVal("path")
        Dim sFile As String = pageBase.QStringToVal("file")
        If sDir.StartsWith("\") Then
            sDir = sDir.Substring(1)
        End If
        Dim sBasePath As String = context.Server.MapPath("/app_data/ProtectedFiles")
        Dim sRequestDir As String = Path.Combine(sBasePath, sDir)
        Dim sFilePath As String = Path.Combine(sRequestDir, sFile)
        If File.Exists(sFilePath) And context.Request.IsAuthenticated Then
            context.Response.Clear()
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" & context.Server.UrlEncode(sFile))
            context.Response.ContentType = "application/octet-stream"
            context.Response.WriteFile(sFilePath)
            context.Response.End()
        Else
            context.Response.Write("There was an error downloading the requested file.")
        End If
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class