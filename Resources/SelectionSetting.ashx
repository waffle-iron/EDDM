<%@ WebHandler Language="VB" Class="SelectionSetting" %>

Imports System
Imports System.Web

Public Class SelectionSetting : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sCookieName As String = appxCMS.Util.Querystring.GetString("name")
        Dim sCookieValue As String = appxCMS.Util.Querystring.GetString("value")
        Dim sRedirect As String = appxCMS.Util.Querystring.GetString("ReturnUrl")
        
        context.Response.Cookies.Add(New HttpCookie(sCookieName, sCookieValue))
        context.Response.Redirect(sRedirect)        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class