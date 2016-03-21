Imports log4net

Partial Class _404
    Inherits appxCMS.PageBase

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected sAbsPath As String = ""
    Protected sOURL As String = ""
    Protected sErr As String = ""

    Protected Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If User.IsInRole("Manage.Content") Then
            pCMSCreate.Visible = True
            CreatePage.SetPageRef = sOURL
            If Not String.IsNullOrEmpty(Me.sErr) Then
                pErrorInfo.Visible = True
                lErr.Text = Me.sErr
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sRebase As String = Request.Url.ToString.Replace(Request.Url.Scheme & "://" & Request.Url.Host, "")
        Page.Form.Action = sRebase
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        sOURL = apphelp.GetRequestedURL()
        If sOURL.EndsWith("/") Then
            sOURL = sOURL.Substring(0, sOURL.Length - 1)
            linkHelp.Redirect301(sOURL, True)
        End If

        If sOURL.Contains("/?") Then
            Dim sQuery As String = sOURL.Substring(sOURL.LastIndexOf("/?") + 2)
            Dim sURL As String = sOURL.Substring(0, sOURL.LastIndexOf("/?"))
            If sURL.StartsWith("/") Then
                sURL = "~" & sURL
            End If
            linkHelp.Redirect301(sURL & "?" & sQuery, True)
        End If

        Dim sTransfer As String = ""
        Try
            appxCMS.SEO.Rewrite.ApplyRules(sOURL, sTransfer)

            Dim sUrl As String = Request.Url.Scheme & "://" & Request.Url.Host
            Dim oURI As System.Uri = New System.Uri(sUrl & sOURL)
            sAbsPath = Server.UrlDecode(oURI.AbsolutePath)
            If sAbsPath.EndsWith("/") Then
                sAbsPath = sAbsPath.Substring(0, sAbsPath.Length - 1)
            End If

            Using oTemplateA As New appxCMSDataTableAdapters.VirtualTableAdapter
                sTransfer = oTemplateA.GetTemplate(sAbsPath)
            End Using

            If Not String.IsNullOrEmpty(sTransfer) Then
                Response.Clear()
                Server.Transfer(sTransfer, True)
            End If

        Catch ex As Exception
            'log.Info("404 or load error on " & sAbsPath)
            'Log.Error(sTransfer, ex)
            Dim oErr As New StringBuilder
            oErr.AppendLine("<p>Absolute Path: " & sAbsPath & "</p>")
            oErr.AppendLine("<p>Transfer Path: " & sTransfer & "</p>")
            oErr.AppendLine("<p>Error: " & ex.Message & "</p>")
            oErr.AppendLine("<p>Stacktrace: " & ex.StackTrace & "</p>")
            Me.sErr = oErr.ToString
        End Try

        '-- If we are still here, send the 404 response type
        Response.StatusCode = 404
        Response.Status = "404 Not Found"
    End Sub

    'Private Function GetRequestedURL() As String
    '    Dim sURL As String = Request.ServerVariables("QUERY_STRING").Replace("404;", "")
    '    Try
    '        Dim oURI As New Uri(sURL)
    '        sURL = oURI.PathAndQuery
    '        Return sURL
    '    Catch ex As Exception
    '        Return ""
    '    End Try
    'End Function
End Class
