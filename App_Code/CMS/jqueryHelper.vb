Imports Microsoft.VisualBasic

Public Class jqueryHelper
    Public Shared Sub Include(ByVal oPg As Page)
        Dim sProtocol As String = "http://"
        If oPg.Request.IsSecureConnection Then
            sProtocol = "https://"
        End If
        If Not oPg.ClientScript.IsClientScriptIncludeRegistered("jQuery-Min") Then
            Dim sJQueryPath As String = sProtocol & "ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"
            Dim sLocalPath As String = appxCMS.Util.AppSettings.GetString("jQuery-Min")
            If Not String.IsNullOrEmpty(sLocalPath) Then
                sJQueryPath = sLocalPath
            End If
            If sJQueryPath.StartsWith("~/") Then
                sJQueryPath = VirtualPathUtility.ToAbsolute(sJQueryPath)
            End If
            oPg.ClientScript.RegisterClientScriptInclude("jQuery-Min", sJQueryPath)
        End If

        If Not oPg.ClientScript.IsClientScriptIncludeRegistered("jQueryUI-Min") Then
            Dim sUIPath As String = sProtocol & "ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/jquery-ui.min.js"
            Dim sLocalPath As String = appxCMS.Util.AppSettings.GetString("jQueryUI-Min")
            If Not String.IsNullOrEmpty(sLocalPath) Then
                sUIPath = sLocalPath
            End If
            If sUIPath.StartsWith("~/") Then
                sUIPath = VirtualPathUtility.ToAbsolute(sUIPath)
            End If
            oPg.ClientScript.RegisterClientScriptInclude("jQueryUI-Min", sUIPath)
        End If
    End Sub

    Public Shared Sub IncludePlugin(ByVal oPg As Page, ByVal PluginAlias As String, ByVal PluginPath As String)
        If Not oPg.ClientScript.IsClientScriptIncludeRegistered(PluginAlias) Then
            If PluginPath.StartsWith("~/") Then
                Dim sQuery As String = ""
                If PluginPath.Contains("?") Then
                    sQuery = PluginPath.Substring(PluginPath.IndexOf("?"))
                    PluginPath = PluginPath.Substring(0, PluginPath.IndexOf("?"))
                End If

                PluginPath = VirtualPathUtility.ToAbsolute(PluginPath)
                If Not String.IsNullOrEmpty(sQuery) Then
                    PluginPath = PluginPath & sQuery
                End If
            End If
            oPg.ClientScript.RegisterClientScriptInclude(PluginAlias, PluginPath)
        End If
    End Sub

    Public Shared Sub RegisterStylesheet(ByVal oPage As Page, ByVal sPath As String)
        If sPath.StartsWith("~/") Then
            Dim sQuery As String = ""
            If sPath.Contains("?") Then
                sQuery = sPath.Substring(sPath.IndexOf("?"))
                sPath = sPath.Substring(0, sPath.IndexOf("?"))
            End If
            sPath = VirtualPathUtility.ToAbsolute(sPath)
            If Not String.IsNullOrEmpty(sQuery) Then
                sPath = sPath & sQuery
            End If
        End If
        Dim oCss As New LiteralControl("<link rel=""stylesheet"" type=""text/css"" href=""" & sPath & """/>")
        If oPage.Header IsNot Nothing Then
            oPage.Header.Controls.Add(oCss)
            'Else
            '    '-- Not a dynamic page header, register this anywhere and hope for the best
            '    oPage.Controls.Add(oCss)
        End If
    End Sub

    Public Shared Sub RegisterClientScript(ByVal oPage As Page, ByVal ScriptAlias As String, ByVal Script As String)
        If Not oPage.ClientScript.IsClientScriptIncludeRegistered(ScriptAlias) Then
            If Not appxCMS.Util.CMSSettings.GetBoolean("CMSConfig", "EnableDebug") Then
                'cmshelp.DebugEnabled 
                '-- Minify script
                '-- Using jsMin
                'Script = JavaScriptSupport.JavaScriptMinifier.Minify(Script)
                '-- Using YUI Compressor
                'Script = Yahoo.Yui.Compressor.JavaScriptCompressor.Compress(Script)
            End If
            oPage.ClientScript.RegisterClientScriptBlock(GetType(String), ScriptAlias, Script, True)
        End If
    End Sub

    Public Shared Sub RegisterStartupScript(ByVal oPage As Page, ByVal ScriptAlias As String, ByVal Script As String)
        If Not oPage.ClientScript.IsClientScriptIncludeRegistered(ScriptAlias) Then
            If Not appxCMS.Util.CMSSettings.GetBoolean("CMSConfig", "EnableDebug") Then
                'cmshelp.DebugEnabled 
                '-- Minify script
                '-- Using jsMin
                'Script = JavaScriptSupport.JavaScriptMinifier.Minify(Script)
                '-- Using YUI Compressor
                'Script = Yahoo.Yui.Compressor.JavaScriptCompressor.Compress(Script)
            End If
            oPage.ClientScript.RegisterStartupScript(GetType(String), ScriptAlias, Script, True)
        End If
    End Sub
End Class
