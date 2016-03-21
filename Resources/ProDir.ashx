<%@ WebHandler Language="VB" Class="ProDir" %>

Imports System
Imports System.Web
Imports System.IO
Imports log4net

Public Class ProDir : Implements IHttpHandler
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sAction As String = pageBase.FStringToVal("maction")
        Dim sType As String = pageBase.FStringToVal("mtype")
        Dim sData As String = pageBase.FStringToVal("mdata")
        Dim sPath As String = pageBase.FStringToVal("mpath")
        Dim sKey As String = pageBase.FStringToVal("mkey")
        Dim bRet As Boolean = False
        Dim sRet As String = ""
        
        Dim sBasePath As String = context.Server.MapPath("/app_data/ProtectedFiles")
        
        If sKey = ConfigurationManager.AppSettings("ProDirEditKey") Then
            '-- Make sure that the path exists in the Pro Dir setup
            If sPath.ToLower.StartsWith(sBasePath.ToLower) And sPath.ToLower <> sBasePath.ToLower Then
                Select Case sAction.ToLower
                    Case "edit"
                        Dim sMetaExt As String = ""
                        Select Case sType.ToLower
                            Case "file"
                                sMetaExt = ".cmsdesc"
                    
                            Case "link"
                                sMetaExt = ".cmsurl"
                    
                            Case "dir"
                                sMetaExt = ".cmsdir"
                        End Select
                
                        If String.IsNullOrEmpty(sMetaExt) Then
                            sRet = "Unhandled type"
                        Else
                            If sPath.EndsWith(sMetaExt) Then
                                File.WriteAllText(sPath, sData)
                                bRet = True
                            Else
                                sRet = "Incorrect extension"
                            End If
                        End If
                    Case "delete"
                        Select Case sType.ToLower
                            Case "file"
                                If File.Exists(sPath & ".cmsdesc") Then
                                    File.Delete(sPath & ".cmsdesc")
                                End If
                                If File.Exists(sPath) Then
                                    File.Delete(sPath)
                                    bRet = True
                                Else
                                    sRet = "Path does not exist"
                                End If
                            Case "link"
                                If File.Exists(sPath) Then
                                    File.Delete(sPath)
                                    bRet = True
                                Else
                                    sRet = "Path does not exist"
                                End If
                            Case "dir"
                                If Directory.Exists(sPath) Then
                                    Try
                                        Directory.Delete(sPath)
                                        bRet = True
                                    Catch ex As Exception
                                        sRet = "Unable to delete directory: " & ex.Message
                                    End Try
                                Else
                                    sRet = "Path does not exist"
                                End If
                        End Select
                End Select
            Else
                sRet = "Invalid path requested."
            End If
        Else
            sRet = "Invalid edit key"
        End If

        context.Response.Clear()
        context.Response.ContentType = "text/html"
        context.Response.Write(bRet.ToString.ToLower & "|" & sRet)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class