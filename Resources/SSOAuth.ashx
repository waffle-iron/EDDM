<%@ WebHandler Language="VB" Class="SSOAuth" %>

Imports System
Imports System.Web
Imports log4net
Imports appxCMS.EF

Public Class SSOAuth : Implements IHttpHandler
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If Not context.Request.IsSecureConnection Then
            context.Response.Write("This action requires a secure connection.")
            context.Response.End()
        End If
        
        Dim sAuthKey As String = ""
        If context.Request.HttpMethod.ToUpper() = "GET" Then
            sAuthKey = appxCMS.Util.Querystring.GetString("authkey")
        Else
            sAuthKey = appxCMS.Util.Form.GetString("authkey")
        End If
         
        If String.IsNullOrEmpty(sAuthKey) Then
            Log.Info("Missing SSO AuthKey URL: " & context.Request.RawUrl)
            context.Response.Write("Missing authkey")
            context.Response.End()
        End If
        
        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId
        
        Dim oSiteSSO As appxCMS.SiteSSO = appxCMS.SiteDataSource.AuthSSOProvider(sAuthKey)
        If oSiteSSO IsNot Nothing Then
            If Not oSiteSSO.Enabled Then
                context.Response.Write("Disabled.")
                context.Response.End()
            End If
            
            If Not oSiteSSO.SiteReference.ForeignKey = siteId Then
                context.Response.Write("Wrong site.")
                context.Response.End()
            End If
        Else
            context.Response.Write("Not authorized.")
            context.Response.End()
        End If
        
        'EmailAddress.Text, Password.Text, FirstName.Text, LastName.Text, _
        'EmailAddress.Text, DirectLine.Text, DirectLineExt.Text, Address1.Text, Address2.Text, City.Text, _
        'State.SelectedValue, PostalCode.Text, False, Eletter.Checked, iBizClass, Company.Text
        
        '-- If we are here, then the provider is correct for the site and authorized
        Dim sEmail As String = ""
        Dim sFName As String = ""
        Dim sLName As String = ""
        Dim sPhone As String = ""
        Dim sAddr As String = ""
        Dim sAddr2 As String = ""
        Dim sCity As String = ""
        Dim sState As String = ""
        Dim sZip As String = ""
        Dim sCompany As String = ""
        Dim sRedirectUrl As String = ""
        
        If context.Request.HttpMethod.ToUpper() = "GET" Then
            sEmail = appxCMS.Util.Querystring.GetString("email")
            sFName = appxCMS.Util.Querystring.GetString("fname")
            sLName = appxCMS.Util.Querystring.GetString("lname")
            sPhone = appxCMS.Util.Querystring.GetString("phone")
            sAddr = appxCMS.Util.Querystring.GetString("addr")
            sAddr2 = appxCMS.Util.Querystring.GetString("addr2")
            sCity = appxCMS.Util.Querystring.GetString("city")
            sState = appxCMS.Util.Querystring.GetString("state")
            sZip = appxCMS.Util.Querystring.GetString("zip")
            sCompany = appxCMS.Util.Querystring.GetString("company")
            sRedirectUrl = appxCMS.Util.Querystring.GetString("RedirectUrl")
            
            '-- Log the SSO Auth attempt
            For Each sKey As String In context.Request.QueryString.Keys
                Log.Info("SSO Auth Field '" & sKey & "' = " & context.Request.QueryString(sKey))
            Next
        Else
            sEmail = appxCMS.Util.Form.GetString("email")
            sFName = appxCMS.Util.Form.GetString("fname")
            sLName = appxCMS.Util.Form.GetString("lname")
            sPhone = appxCMS.Util.Form.GetString("phone")
            sAddr = appxCMS.Util.Form.GetString("addr")
            sAddr2 = appxCMS.Util.Form.GetString("addr2")
            sCity = appxCMS.Util.Form.GetString("city")
            sState = appxCMS.Util.Form.GetString("state")
            sZip = appxCMS.Util.Form.GetString("zip")
            sCompany = appxCMS.Util.Form.GetString("company")
            sRedirectUrl = appxCMS.Util.Form.GetString("RedirectUrl")
            
            '-- Log the SSO Auth attempt
            For Each sKey As String In context.Request.Form.Keys
                Log.Info("SSO Auth Field '" & sKey & "' = " & context.Request.Form(sKey))
            Next
        End If
        
        '-- Check for the user
        Dim oUser As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(sEmail)
        If oUser Is Nothing Then
            Dim sPass As String = System.Web.Security.Membership.GeneratePassword(10, 5)
            
            '-- We are going to create this user and attach to the site
            Dim customerId As Integer = Taradel.CustomerDataSource.Add(siteId, sEmail, sPass, sFName, sLName, _
                                                        sEmail, sPhone, "", sAddr, sAddr2, sCity, _
                                                        sState, sZip, False, False, 0, sCompany)
            
            If customerId > 0 Then
                oUser = Taradel.CustomerDataSource.GetCustomer(customerId)
                Log.Info("Added new account with SSO for " & sEmail & ", CustomerId = " & customerId)
            End If
        End If
        
        If oUser IsNot Nothing Then
            '-- Authenticate the user
            Dim bAuth As Boolean = False
            Dim sAuthMsg As String = ""
            Dim oAuth As New Taradel.Auth
            oAuth.Authenticate(sEmail, oUser.Password, False, bAuth, sAuthMsg)
            
            If bAuth Then
                'If Not String.IsNullOrEmpty(sRedirectUrl) Then
                '    context.Response.Redirect(sRedirectUrl)
                'Else
                context.Response.Redirect("~/")
                'End If
            Else
                context.Response.Write("Auth failed.")
                context.Response.End()
            End If
        End If
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class