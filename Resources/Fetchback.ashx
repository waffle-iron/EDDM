<%@ WebHandler Language="VB" Class="Fetchback" %>

Imports System
Imports System.Web

Public Class Fetchback : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sEmail As String = pageBase.QStringToVal("e")
        If String.IsNullOrEmpty(sEmail) Then
            sEmail = pageBase.FStringToVal("e")
        End If
        
        Dim bSaved As Boolean = False
        If Not String.IsNullOrEmpty(sEmail) Then
            If sEmail.Contains("@") Then
                '-- Send data to Driven
                Dim oDrivenPost As New Hashtable
                oDrivenPost.Add("source", "Fetchback")
                oDrivenPost.Add("Type", "PC")
                oDrivenPost.Add("Category", "GL")
                oDrivenPost.Add("Sample Requested", "NO")
                oDrivenPost.Add("Marketing Consult Requested", "NO")
                oDrivenPost.Add("email", sEmail)
                Dim sDrivenResult As String = appxCMS.Util.httpHelp.PostXMLURLPage("http://driven.taradel.com/TResources/Lead.ashx", oDrivenPost, Nothing, Nothing)
                'context.Response.Write(sDrivenResult & ControlChars.CrLf)
                
                '-- Send data to Hubspot
                Dim oHubSpotPost As New Hashtable
                'oHubSpotPost.Add("FirstName", sEmail)
                'oHubSpotPost.Add("LastName", sEmail)
                oHubSpotPost.Add("Email", sEmail)
                Dim sHubspotResult As String = appxCMS.Util.httpHelp.PostXMLURLPage("https://forms.hubspot.com/uploads/form/v2/212947/2cfb1a7a-7131-46eb-8cc5-99566b9db909", oHubSpotPost, Nothing, Nothing)
                'context.Response.Write(sHubspotResult & ControlChars.CrLf)
                bSaved = True
            End If
        End If
        
        'context.Response.ContentType = "text/plain"
        'If bSaved Then
        '    context.Response.Write("OK")
        'Else
        '    context.Response.Write("Error")
        'End If
        
        context.Response.Redirect("/")
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class