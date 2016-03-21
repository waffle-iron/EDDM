<%@ WebHandler Language="VB" Class="CommunityServerResource" %>

Imports System
Imports System.Web
Imports System.Security.Cryptography

Public Class CommunityServerResource : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sPath As String = ""
        Dim sCacheKey As String = ""
        Dim sDisplayType As String = ""
        
        If context.Request.QueryString("path") IsNot Nothing Then
            sPath = context.Request.QueryString("path")
        End If
        
        If context.Request.QueryString("dt") IsNot Nothing Then
            sDisplayType = context.Request.QueryString("dt")
        End If
        
        If Not String.IsNullOrEmpty(sPath) Then
            sCacheKey = MD5Hash(sPath)
        End If
        
        If Not String.IsNullOrEmpty(sCacheKey) Then
            If context.Cache(sCacheKey) Is Nothing Then
                
            End If
        End If
        
        'context.Response.ContentType = "text/plain"
        'context.Response.Write("Hello World")
    End Sub
    
    Private Function MD5Hash(ByVal sInStr As String) As String
        Dim oHasher As New MD5CryptoServiceProvider()
    
        Dim hashedDataBytes As Byte()
        Dim encoder As New UTF8Encoding()

        hashedDataBytes = oHasher.ComputeHash(encoder.GetBytes(sInStr))
    
        Dim oSB As New StringBuilder
        Dim b As Byte
        For Each b In hashedDataBytes
            oSB.Append(b)
        Next b
        
        Return oSB.ToString
    End Function
        
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class