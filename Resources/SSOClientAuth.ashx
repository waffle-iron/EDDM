<%@ WebHandler Language="VB" Class="SSOClientAuth" %>

Imports System
Imports System.Web
Imports log4net

Public Class SSOClientAuth : Implements IHttpHandler
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If Not context.Request.IsSecureConnection Then
            context.Response.StatusCode = 403
            context.Response.Write("This action requires a secure connection.")
            context.Response.End()
        End If
        
        Dim sAuthKey As String = appxCMS.Util.Querystring.GetString("authtoken")
        If String.IsNullOrEmpty(sAuthKey) Then
            context.Response.StatusCode = 500
            Log.Info("Missing SSO Client AuthKey URL: " & context.Request.RawUrl)
            context.Response.Write("Missing auth token")
            context.Response.End()
        End If
        
        Dim sEmail As String = appxCMS.Util.Querystring.GetString("email")
        Dim sUsername As String = appxCMS.Util.Querystring.GetString("username")
        If String.IsNullOrEmpty(sUsername) Then
            sUsername = sEmail
        End If
        
        If String.IsNullOrEmpty(sEmail) Then
            context.Response.StatusCode = 500
            Log.Info("Missing SSO Client Email URL: " & context.Request.RawUrl)
            context.Response.Write("Missing email")
            context.Response.End()
        End If
        
        Dim sRedirectUrl As String = appxCMS.Util.Querystring.GetString("RedirectUrl")
        
        Dim sToken As String = ""
        If context.Cache("SSOAuthToken:" & sUsername & ":" & sEmail) IsNot Nothing Then
            sToken = context.Cache("SSOAuthToken:" & sUsername & ":" & sEmail).ToString()
        End If
        If Not String.IsNullOrEmpty(sToken) AndAlso sToken.Equals(sAuthKey) Then
            '-- Good to authenticate
            Dim oUser As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(sUsername)
            If oUser IsNot Nothing Then
                Dim bAuth As Boolean = False
                Dim sAuthMsg As String = ""
                Dim oAuth As New Taradel.Auth
                oAuth.Authenticate(sUsername, oUser.Password, False, bAuth, sAuthMsg)
                
                If bAuth Then
                    If Not String.IsNullOrEmpty(sRedirectUrl) Then
                        context.Response.Redirect(sRedirectUrl)
                    Else
                        context.Response.Redirect("~/")
                    End If
                Else
                    context.Response.StatusCode = 500
                    Log.Warn("SSO Client Authentication Fail: " & context.Request.RawUrl)
                    context.Response.Write("Authentication Failure (002)")
                    context.Response.End()
                End If
            Else
                context.Response.StatusCode = 500
                Log.Warn("SSO Client Authentication Fail: " & context.Request.RawUrl)
                context.Response.Write("Authentication Failure (001)")
                context.Response.End()
            End If
        Else
            context.Response.StatusCode = 500
            Log.Warn("SSO Client Authentication Token Mismatch: " & context.Request.RawUrl)
            context.Response.Write("Authentication Failure (003)")
            context.Response.End()
        End If        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class