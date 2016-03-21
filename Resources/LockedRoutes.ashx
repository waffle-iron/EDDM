<%@ WebHandler Language="VB" Class="LockedRoutes" %>

Imports System
Imports System.Web
Imports System.Collections.Generic
Imports System.Data.SqlClient

Public Class LockedRoutes : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        '        Select Case context.Request("HTTP_ORIGIN").ToLower()
        '            Case "http://"
        '        End Select
        '        switch ($_SERVER['HTTP_ORIGIN']) {
        '    case 'http://from.com': case 'https://from.com':
        '    header('Access-Control-Allow-Origin: '.$_SERVER['HTTP_ORIGIN']);
        '    header('Access-Control-Allow-Methods: GET, PUT, POST, DELETE, OPTIONS');
        '    header('Access-Control-Max-Age: 1000');
        '    header('Access-Control-Allow-Headers: Content-Type, Authorization, X-Requested-With');
        '    break;
        '}
        
        'TODO: Restrict the cross-origin request to the allowed domain for the USElect Method
        context.Response.AppendHeader("Access-Control-Allow-Origin", "*")
                
        context.Response.ContentType = "application/json"
        
        Dim oLocked As New List(Of String)
        Dim oExclusive As New List(Of String)
        Dim bAuth As Boolean = False
        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId()
        
        Dim bEnableLock As Boolean = False
                
        Dim bLockExclusive As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Route", "OffersExclusiveRoutes", siteId)
        Dim bLockTerritory As Boolean = appxCMS.Util.CMSSettings.GetBoolean("Route", "UseExclusiveTerritories", siteId)
        
        If bLockExclusive OrElse bLockTerritory Then
            bEnableLock = True
        End If

        Dim userId As Integer = 0

        If context.Request.IsAuthenticated Then
            Dim oCust = Taradel.CustomerDataSource.GetCustomer(context.User.Identity.Name)
            If oCust IsNot Nothing Then
                bAuth = True
                userId = oCust.CustomerID
            End If
        End If       

        If bEnableLock Then
            '-- Query to get locked routes NOT belonging to logged in user
            Dim oQ1 As New StringBuilder
            oQ1.AppendLine("SELECT DISTINCT GeocodeRef FROM USelectRouteLock rl")
            oQ1.AppendLine("LEFT JOIN USelectLockedRoute lr ON lr.LockId=rl.LockId")
            oQ1.AppendLine("WHERE rl.SiteId=" & siteId)
            oQ1.AppendLine("AND rl.EffectiveDate <= GetDate()")
            oQ1.AppendLine("AND rl.ExpirationDate >= GetDate()")
            oQ1.AppendLine("AND rl.CustomerId <> " & userId)
            oQ1.AppendLine("ORDER BY GeocodeRef ASC")

            '-- Query to get locked routes exclusive to the logged in user
            Dim oQ2 As New StringBuilder
            oQ2.AppendLine("SELECT DISTINCT GeocodeRef FROM USelectRouteLock rl")
            oQ2.AppendLine("LEFT JOIN USelectLockedRoute lr ON lr.LockId=rl.LockId")
            oQ2.AppendLine("WHERE rl.SiteId=" & siteId)
            oQ2.AppendLine("AND rl.EffectiveDate <= GetDate()")
            oQ2.AppendLine("AND rl.ExpirationDate >= GetDate()")
            oQ2.AppendLine("AND rl.CustomerId = " & userId)
            oQ2.AppendLine("ORDER BY GeocodeRef ASC")
        
            Using oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("appx").ConnectionString)
                oConn.Open()
                     
                '-- Load locked routes
                Using oCmd As New SqlCommand(oQ1.ToString(), oConn)
                    Using oRdr = oCmd.ExecuteReader()
                        While oRdr.Read
                            oLocked.Add(oRdr("GeocodeRef").ToString())
                        End While
                    End Using
                End Using
            
                '-- Load exclusive routes
                If userId > 0 Then
                    Using oCmd As New SqlCommand(oQ2.ToString(), oConn)
                        Using oRdr = oCmd.ExecuteReader()
                            While oRdr.Read
                                oExclusive.Add(oRdr("GeocodeRef").ToString())
                            End While
                        End Using
                    End Using
                End If
            End Using
        End If
        
        '-- Build our JSON object of the values
        Dim sLocked As String = String.Join(",", oLocked.ToArray())
        Dim sExclusive As String = String.Join(",", oExclusive.ToArray())
        Dim sResult As String = "{""Auth"":" & bAuth.ToString().ToLowerInvariant() & ", ""Locked"":""" & sLocked & """, ""Exclusive"":""" & sExclusive & """}"

        Dim sCb As String = appxCMS.Util.Querystring.GetString("callback")
        If Not String.IsNullOrEmpty(sCb) Then
            context.Response.Write(sCb & "(" & sResult & ");")
        Else
            context.Response.Write(sResult)
        End If
        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class