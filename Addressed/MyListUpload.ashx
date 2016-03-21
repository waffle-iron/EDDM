<%@ WebHandler Language="VB" Class="MyListUpload" %>

Imports System
Imports System.Web
Imports System.IO
Imports log4net
Imports appxCMS.Web

Public Class MyListUpload : Implements IHttpHandler
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        Dim oResult As New AsyncUploadResult
        LogThis("ProcessRequest - process hit")
        Dim projectId As String = appxCMS.Util.Querystring.GetString("p")
        Dim oRe As New Regex("[^A-Z0-9a-z\-]")
        projectId = oRe.Replace(projectId, "")
        
        If projectId.Length > 36 Then
            projectId = projectId.Substring(0, 36)
        End If
        
        'Dim sBasePath As String = context.Server.MapPath("~/app_data/AddressedInbound/" & projectId)
        Dim sBasePath As String = context.Server.MapPath("~/app_data/AddressedListInbound/" & projectId)

        If Not Directory.Exists(sBasePath) Then
            Try
                Directory.CreateDirectory(sBasePath)
            Catch ex As Exception
                oResult.SetResponse(False, "Unable to access the directory", "")
                LogThis(ex.Message)
                LogThis(ex.StackTrace)
                SendResponse(context, oResult)
            End Try
        End If

        LogThis("made it to file name " & sBasePath)


        Dim oFile As HttpPostedFile = context.Request.Files(0)
        Dim fileName As String
        If HttpContext.Current.Request.Browser.Browser.ToUpper() = "IE" Then
            Dim files As String() = oFile.FileName.Split(New Char() {"\"})
            fileName = files(files.Length - 1)
        Else
            fileName = oFile.FileName
        End If
        Dim sExt As String = Path.GetExtension(fileName)
        
        Dim sSaveFileName As String = projectId & sExt
        Dim sSaveFile As String = Path.Combine(sBasePath, sSaveFileName)
        If File.Exists(sSaveFile) Then
            Try
                File.Delete(sSaveFile)
            Catch ex As Exception
                LogThis(ex.Message)
                LogThis(ex.StackTrace)
                oResult.SetResponse(False, "Error removing existing file with same name.", "")
                SendResponse(context, oResult)
            End Try
        End If
       
        Try
            oFile.SaveAs(sSaveFile)
            
        Catch ex As Exception
            LogThis(ex.Message)
            LogThis(ex.StackTrace)
            oResult.SetResponse(False, "Error saving file.", "")
            SendResponse(context, oResult)
        End Try
        
        Dim sRelPath As String = sSaveFile.Replace(context.Server.MapPath("~/"), "").Replace("\", "/")
        If Not sRelPath.StartsWith("/") Then
            sRelPath = "/" & sRelPath
        End If
        
        oResult.SetResponse(True, "", sSaveFileName)
        SendResponse(context, oResult)
    End Sub
 
    Protected Sub SendResponse(context As HttpContext, oResponse As AsyncUploadResult)
        Dim sMsg As String = appxCMS.Util.JavaScriptSerializer.Serialize(oResponse)
        context.Response.Clear()
        If Not oResponse.Success Then
            context.Response.StatusCode = 500
            context.Response.Write(oResponse.Message)
        Else
            context.Response.ContentType = "application/json"
            context.Response.Write(sMsg)
        End If
        context.Response.End()
    End Sub
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Shared Sub LogThis(logMessage As String)
        Dim logFileName As String = "~\Logs\MyListUpload.txt"
        Dim fullPath As String = HttpContext.Current.Server.MapPath(logFileName)

        Try
            Using sw As System.IO.StreamWriter = System.IO.File.AppendText(fullPath)
                sw.WriteLine(logMessage)
            End Using
        Catch ex As Exception
            Dim errorFileName As String = "~\Logs\EDDM-LogError.txt"
            Dim errorFullPath As String = HttpContext.Current.Server.MapPath(errorFileName)

            Try
                Using sw As System.IO.StreamWriter = System.IO.File.AppendText(errorFullPath)
                    sw.WriteLine(ex.ToString())
                    sw.WriteLine(logMessage)
                End Using
                'eat it
            Catch ex2 As Exception
            End Try
        End Try
    End Sub

End Class