<%@ Application Language="VB" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="log4net" %>
<%@ Import Namespace="System.Threading" %>


<script RunAt="server">
    
    
    
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        log4net.Config.XmlConfigurator.Configure(New System.IO.FileInfo(HttpContext.Current.Server.MapPath("~/app_data/Configuration/log4net.config")))
        log.Info("Application is starting up")
        
        '-- Wire in the global sitemap resolver
        AddHandler SiteMap.SiteMapResolve, AddressOf Provider_SiteMapResolve

        'System.Net.WebRequest.DefaultWebProxy = New System.Net.WebProxy("127.0.0.1", 8888)
	
    End Sub
    
    Private Function Provider_SiteMapResolve(sender As Object, e As SiteMapResolveEventArgs) As SiteMapNode
        If TypeOf e.Context.CurrentHandler Is appxCMS.ISiteMapResolver Then
            Return DirectCast(e.Context.CurrentHandler, appxCMS.ISiteMapResolver).SiteMapResolve(sender, e)
        Else
            Return Nothing
        End If
    End Function
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
        log.Info("Application is shutting down...")
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
        Dim ctx As HttpContext = HttpContext.Current
        
        ' Code that runs when an unhandled error occurs
        Dim oEx As Exception = Server.GetLastError()
                
        '-- Ignore certain types of HTTP errors
        If TypeOf oEx Is HttpException Then
            Dim oHttpEx As HttpException = DirectCast(oEx, HttpException)
            If oHttpEx.GetHttpCode = 404 Then
                '-- Ignore this
                Exit Sub
            ElseIf oHttpEx.Message.ToLower.StartsWith("a potentially dangerous") Then
                '-- Built-in form protection
                Exit Sub
            End If
        End If

        '-- Do not log viewstate exceptions
        If IsVSEx(oEx) Then Exit Sub
        
        If ctx.Request.Path.ToLowerInvariant.EndsWith(".axd") Then Exit Sub
        
        '-- Do not log Thread was being aborted exceptions
        If TypeOf oEx Is ThreadAbortException Then
            Exit Sub
        End If
        
        Dim oLog As log4net.ILog = log4net.LogManager.GetLogger(oEx.TargetSite.DeclaringType)
        log4net.ThreadContext.Stacks("RawUrl").Push(ctx.Request.RawUrl)
        oLog.Error("Unhandled Exception: " & oEx.Message, oEx)
    End Sub
    
    Private Function IsVSEx(oEx As Exception) As Boolean
        If oEx Is Nothing Then Return False
        If TypeOf oEx Is ViewStateException Then Return True
        Return IsVSEx(oEx.InnerException)
    End Function

    Protected Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        If Not (HttpContext.Current.User Is Nothing) Then
            If HttpContext.Current.User.Identity.AuthenticationType <> "Forms" Then
                Throw New Exception("Only forms authentication is supported, not " + HttpContext.Current.User.Identity.AuthenticationType)
            End If
      
            Dim userId As System.Security.Principal.IIdentity = HttpContext.Current.User.Identity
                
            Dim sCacheKey As String = "appxAuth:" & userId.Name & ":Roles"
            
            'Do we have some roles to retrieve?  If so, replace the user object
            'If Application(userId.Name & "Roles") IsNot Nothing Then
            Dim aRoles() As String
            Dim alRoles As New ArrayList
                
            Dim bLoadRoles As Boolean = True
            If appxCMS.Util.Cache.Exists(sCacheKey) Then
                alRoles = appxCMS.Util.Cache.GetObject(sCacheKey)

                If alRoles.Count > 0 Then
                    bLoadRoles = False
                End If
            End If

            If bLoadRoles Then
                Dim AdminID As Integer = 0
                
                '-- Try to get an admin user
                
                Dim oUser As appxCMS.User = Nothing
                Try
                    oUser = appxCMS.UserDataSource.GetSiteInvariantUser(userId.Name)
                Catch ex As ReflectionTypeLoadException
                    Dim sb As New StringBuilder()
                    For Each exSub As Exception In ex.LoaderExceptions
                        sb.AppendLine(exSub.Message)
                        If TypeOf exSub Is FileNotFoundException Then
                            Dim exFileNotFound As FileNotFoundException = TryCast(exSub, FileNotFoundException)
                            If Not String.IsNullOrEmpty(exFileNotFound.FusionLog) Then
                                sb.AppendLine("Fusion Log:")
                                sb.AppendLine(exFileNotFound.FusionLog)
                            End If
                        End If
                        sb.AppendLine()
                    Next
                    Dim errorMessage As String = sb.ToString()
                    'Display or log the error based on your application.
                    Response.Write(errorMessage)
                    Response.End()
                End Try
                'appxCMS.UserDataSource.GetSiteInvariantUser(userId.Name)
                If oUser IsNot Nothing Then
                    alRoles.Add("Admin")
                    AdminID = oUser.AdminID
                    appxCMS.Util.Cache.Add("appxAuth:" & userId.Name & ":Id", AdminID)
                Else
                    Dim oCust As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(userId.Name)
                    If oCust IsNot Nothing Then
                        alRoles.Add("Member")
                        appxCMS.Util.Cache.Add("appxAuth:" & userId.Name & ":Id", oCust.CustomerID)
                    End If
                End If
               
                If AdminID > 0 Then                   
                    If oUser IsNot Nothing Then
                        alRoles.Add("Site" & oUser.SiteId)
                    End If
                    Using oRolesA As New appxAuthTableAdapters.AdminRolesTableAdapter
                        Using oRolesT As appxAuth.AdminRolesDataTable = oRolesA.GetData(AdminID)
                            If oRolesT.Rows.Count > 0 Then
                                For Each oRole As appxAuth.AdminRolesRow In oRolesT
                                    alRoles.Add(oRole.RoleCat & "." & oRole.RoleName)
                                Next
                            End If
                        End Using
                    End Using
                End If
            End If
                
            If alRoles.Count > 0 Then
                aRoles = alRoles.ToArray(GetType(String))
                HttpContext.Current.Cache.Insert(sCacheKey, alRoles, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), CacheItemPriority.Normal, Nothing)
                HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(userId, aRoles)
            Else
                Exit Sub
            End If
        End If
    End Sub 'Application_AuthenticateRequest

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
       
    Sub Profile_MigrateAnonymous(ByVal sender As Object, ByVal e As ProfileMigrateEventArgs)
        Dim aProfile As ProfileCommon = Profile.GetProfile(e.AnonymousID)
        
        Dim sUAuth As String = aProfile.UserName
        Dim sAuth As String = Profile.UserName
           
        If aProfile.Cart iSNot NOthing Then
    If Not String.IsNullOrEmpty(aProfile.Cart.OuterXml) Then
            '-- Load their current cart over any stored cart in their profile          
            Profile.Cart = aProfile.Cart
            
            '-- Migrate their existing temp image folder to their client folder
            Dim sClientBase As String = Taradel.WLUtil.GetRelativeSiteImagesPath & "/UserImages"
            Dim sUFolder As String = sClientBase & "/" & sUAuth
            Dim sFolder As String = sClientBase & "/" & sAuth.Replace("@", "_")
            Dim sUPath As String = Server.MapPath(sUFolder)
            Dim sPath As String = Server.MapPath(sFolder)
            
            Dim oDir As New DirectoryInfo(sPath)
            If Not oDir.Exists() Then
                oDir.Create()
            End If
            
            Dim oUFolder As New DirectoryInfo(sUPath)
            If oUFolder.Exists Then
                For Each oUFile As FileInfo In oUFolder.GetFiles
                    oUFile.MoveTo(Path.Combine(sPath, oUFile.Name))
                Next
                Try
                    oUFolder.Delete()
                Catch ex As Exception
                    log.Error(ex.Message, ex)
                End Try
            End If
        End If
        
        End If

        
        '-- Migrate the user's distributions stored as anonymous
        Dim oOldCust As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(sUAuth)
        Dim oNewCust As Taradel.Customer = Taradel.CustomerDataSource.GetCustomer(sAuth)
        If oOldCust IsNot Nothing AndAlso oNewCust IsNot Nothing Then
            Taradel.Helper.USelect.MigrateOwnership(oOldCust.CustomerID, oNewCust.CustomerID)
        End If      
               
        AnonymousIdentificationModule.ClearAnonymousIdentifier()
    End Sub
    
    
</script>
