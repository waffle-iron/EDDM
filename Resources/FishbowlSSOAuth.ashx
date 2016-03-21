<%@ WebHandler Language="VB" Class="FishbowlSSOAuth" %>

Imports System.Web
Imports log4net
Imports appxCMS.EF

Public Class FishbowlSSOAuth : Implements IHttpHandler
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If Not context.Request.IsSecureConnection Then
            context.Response.StatusCode = 403
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
            context.Response.StatusCode = 500
            Log.Info("Missing SSO AuthKey URL: " & context.Request.RawUrl)
            context.Response.Write("Missing authkey")
            context.Response.End()
        End If
        
        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId
        
        Dim oSiteSso As appxCMS.SiteSSO = appxCMS.SiteDataSource.AuthSSOProvider(sAuthKey)
        If oSiteSso IsNot Nothing Then
            If Not oSiteSso.Enabled Then
                context.Response.StatusCode = 402
                context.Response.Write("Disabled.")
                context.Response.End()
            End If
            
            If Not oSiteSso.SiteReference.ForeignKey = siteId Then
                context.Response.StatusCode = 410
                context.Response.Write("Wrong site.")
                context.Response.End()
            End If
        Else
            context.Response.StatusCode = 404
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
        Dim sUsername As String = ""
        Dim sPhone As String = ""
        Dim sAddr As String = ""
        Dim sAddr2 As String = ""
        Dim sCity As String = ""
        Dim sState As String = ""
        Dim sZip As String = ""
        Dim sCompany As String = ""
        Dim sRedirectUrl As String = ""
        Dim preAuth As Boolean = False
        
        Dim oUserProps As New SortedList
        
        If context.Request.HttpMethod.ToUpper() = "GET" Then
            sEmail = appxCMS.Util.Querystring.GetString("email")
            sFName = appxCMS.Util.Querystring.GetString("fname")
            sLName = appxCMS.Util.Querystring.GetString("lname")
            sUsername = appxCMS.Util.Querystring.GetString("username")
            sPhone = appxCMS.Util.Querystring.GetString("phone")
            sAddr = appxCMS.Util.Querystring.GetString("addr")
            sAddr2 = appxCMS.Util.Querystring.GetString("addr2")
            If String.IsNullOrEmpty(sAddr2) Then
                sAddr2 = appxCMS.Util.Querystring.GetString("add2")
            End If
            sCity = appxCMS.Util.Querystring.GetString("city")
            sState = appxCMS.Util.Querystring.GetString("state")
            sZip = appxCMS.Util.Querystring.GetString("zip")
            sCompany = appxCMS.Util.Querystring.GetString("company")
            sRedirectUrl = appxCMS.Util.Querystring.GetString("RedirectUrl")
            preAuth = appxCMS.Util.Querystring.GetBoolean("pre", False)
            
            '-- Log the SSO Auth attempt
            For Each sKey As String In context.Request.QueryString.Keys
                Log.Info("SSO Auth Field '" & sKey & "' = " & context.Request.QueryString(sKey))
                
                If sKey.ToLower.StartsWith("uprop-") Then
                    Dim sUpropVal As String = context.Request.QueryString(sKey)
                    If Not oUserProps.ContainsKey(sKey) Then
                        oUserProps.Add(sKey.ToUpper, sUpropVal)
                    End If
                End If
            Next
        Else
            sEmail = appxCMS.Util.Form.GetString("email")
            sFName = appxCMS.Util.Form.GetString("fname")
            sLName = appxCMS.Util.Form.GetString("lname")
            sUsername = appxCMS.Util.Form.GetString("username")
            sPhone = appxCMS.Util.Form.GetString("phone")
            sAddr = appxCMS.Util.Form.GetString("addr")
            sAddr2 = appxCMS.Util.Form.GetString("addr2")
            If String.IsNullOrEmpty(sAddr2) Then
                sAddr2 = appxCMS.Util.Form.GetString("add2")
            End If
            sCity = appxCMS.Util.Form.GetString("city")
            sState = appxCMS.Util.Form.GetString("state")
            sZip = appxCMS.Util.Form.GetString("zip")
            sCompany = appxCMS.Util.Form.GetString("company")
            sRedirectUrl = appxCMS.Util.Form.GetString("RedirectUrl")
            preAuth = appxCMS.Util.Form.GetBoolean("pre", False)
            
            '-- Log the SSO Auth attempt
            For Each sKey As String In context.Request.Form.Keys
                Log.Info("SSO Auth Field '" & sKey & "' = " & context.Request.Form(sKey))
                
                If sKey.ToLower.StartsWith("uprop-") Then
                    Dim sUpropVal As String = context.Request.Form(sKey)
                    If Not oUserProps.ContainsKey(sKey) Then
                        oUserProps.Add(sKey.ToUpper, sUpropVal)
                    End If
                End If
            Next
        End If
        
        If String.IsNullOrEmpty(sUsername) Then
            sUsername = sEmail
        End If
        
        Dim oUser As Taradel.Customer = Nothing
        If Not String.IsNullOrEmpty(sUsername) Then
            '-- Check for the user
            oUser = Taradel.CustomerDataSource.GetCustomer(sUsername)
            If oUser Is Nothing Then
                Dim sPass As String = System.Web.Security.Membership.GeneratePassword(10, 5)
            
                '-- We are going to create this user and attach to the site
                Dim customerId As Integer = Taradel.CustomerDataSource.Add(siteId, sUsername, sPass, sFName, sLName, _
                                                            sEmail, sPhone, "", sAddr, sAddr2, sCity, _
                                                            sState, sZip, False, False, 0, sCompany)
            
                If customerId > 0 Then
                    oUser = Taradel.CustomerDataSource.GetCustomer(customerId)
                    Log.Info("Added new account with SSO for " & sEmail & ", CustomerId = " & customerId)
                End If
            Else
                '-- Update the user record
                Taradel.CustomerDataSource.Update(oUser.CustomerID, sUsername, oUser.Password, sFName, "", sLName, sEmail, sPhone, "", "", "", "", "", sAddr, sAddr2, sCity, sState, sZip, False, False, sCompany)
            End If
            
        End If
        
        If oUser IsNot Nothing Then
            '-- Save the user properties that were sent with this login to their properties
            If oUserProps.Count > 0 Then
                For Each sUpropKey As String In oUserProps.Keys
                    Dim sPropVal As String = oUserProps(sUpropKey).ToString
                    If sPropVal.Length > 255 Then
                        sPropVal = sPropVal.Substring(0, 255)
                    End If
                    Taradel.CustomerDataSource.SaveCustomerProperty(oUser.CustomerID, sUpropKey, sPropVal, "")
                Next
            End If
            
            '-- Authenticate the user
            Dim bAuth As Boolean = False
            Dim sAuthMsg As String = ""
            Dim oAuth As New Taradel.Auth
            oAuth.Authenticate(sUsername, oUser.Password, False, bAuth, sAuthMsg)
                        
            If bAuth Then
                If preAuth Then
                    context.Response.StatusCode = 200
                    
                    '-- Create an auth token for this client and store it in the cache
                    Dim sAuthToken As String = System.Web.Security.Membership.GeneratePassword(20, 0)
                    context.Cache.Insert("SSOAuthToken:" & sUsername & ":" & sEmail, sAuthToken)
                    context.Response.Headers.Add("ClientAuthKey", sAuthToken)
                    
                    context.Response.Write("Auth success.")
                    context.Response.End()
                Else
                    If Not String.IsNullOrEmpty(sRedirectUrl) Then
                        context.Response.Redirect(sRedirectUrl)
                    Else
                        context.Response.Redirect("~/")
                    End If
                End If
            Else
                context.Response.StatusCode = 401
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