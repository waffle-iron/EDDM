Imports System.IO
Imports Obout.Ajax.UI.HTMLEditor
Imports Obout.Ajax.UI.HTMLEditor.ToolbarButton
Imports Obout.Ajax.UI.HTMLEditor.Popups
Imports Obout.Ajax.UI.HTMLEditor.ContextMenu
Imports System.Text.RegularExpressions

Partial Class usercontrols_MessageBlastHTMLEditor
    Inherits System.Web.UI.UserControl

    Public Property Content() As String
        Get
            Dim sContent As String = oEd.EditPanel.Content
            sContent = sContent.Replace("http://" & Request.Url.Host, "#siteurl#")
            Return sContent
        End Get
        Set(ByVal value As String)
            Dim sContent As String = value
            sContent = sContent.Replace("#siteurl#", "http://" & Request.Url.Host)
            oEd.EditPanel.Content = sContent
        End Set
    End Property

    Public ReadOnly Property PlainText() As String
        Get
            Return oEd.PlainText
        End Get
    End Property

    Public Property CampaignID() As Integer
        Get
            Return _CampaignID
        End Get
        Set(ByVal value As Integer)
            _CampaignID = value
        End Set
    End Property
    Private _CampaignID As Integer = 0

    Private oSCM As ScriptManager = Nothing
    Private bValidLoad As Boolean = True

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        oSCM = ScriptManager.GetCurrent(Page)
        If oSCM Is Nothing Then
            Me.Controls.Clear()
            Me.Controls.Add(New LiteralControl("<p><b style=""color:red;"">" & Me.GetType.ToString & " requires a ScriptManager on the page.</b></p>"))
            bValidLoad = False
        Else
            oEd.BottomToolbar.AddButtons.Add(New HorizontalSeparator)
            oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.MessageBlastTemplate)
            oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.MessageBlastTemplateFields)
        End If

        MyBase.OnInit(e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.bValidLoad Then Exit Sub

        Dim oEdSetup As New StringBuilder
        oEdSetup.AppendLine("var appxEditorURLBrowserUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxEditorURLBrowser.aspx") & "';")
        oEdSetup.AppendLine("var appxEditorImageBrowserUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxEditorImageBrowser.aspx") & "';")
        oEdSetup.AppendLine("var appxEditorFlashBrowserUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxEditorFlashBrowser.aspx") & "';")
        oEdSetup.AppendLine("var appxEditorMediaBrowserUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxEditorMediaBrowser.aspx") & "';")
        If Me.Visible Then
            ScriptManager.RegisterClientScriptBlock(oEd, oEd.GetType, "appxMessageBlastEditorSetup", oEdSetup.ToString, True)
            Dim sEditorInit As String = File.ReadAllText(Server.MapPath("~/admin/editor/appxMessageBlastEditor.js"))
            ScriptManager.RegisterStartupScript(oEd, oEd.GetType, "appxMessageBlastEditorInit", sEditorInit, True)
            oSCM.Scripts.Add(New ScriptReference("Obout.Ajax.UI.HTMLEditor.Popups.PopupsCommon.js", "Obout.Ajax.UI"))
        End If
    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    '    '-- Add in custom template selector
    '    Try
    '        Dim oDir As New DirectoryInfo(Server.MapPath("/cmsimages/MessageBlastTemplates"))
    '        Dim oTemplates() As FileInfo = oDir.GetFiles("*.htm")
    '        If oTemplates.Length > 0 Then
    '            Dim oSel As New OboutInc.Editor.CustomSelect
    '            oSel.Handler = "processMessageTemplate"
    '            oSel.Options.Add(New OboutInc.Editor.Option("", "Message Template"))
    '            For Each oTemplate As FileInfo In oTemplates
    '                oSel.Options.Add(New OboutInc.Editor.Option(oTemplate.Name, oTemplate.Name))
    '            Next
    '            oEd.Buttons.Add(oSel)
    '            Dim oTempFldSB As New StringBuilder
    '            oTempFldSB.AppendLine("function processMessageTemplate(editor, command, selectObject) {")
    '            oTempFldSB.AppendLine(" // changed  value in select")
    '            oTempFldSB.AppendLine(" if(command == 'change') {")
    '            oTempFldSB.AppendLine("     var value = selectObject.options.item(selectObject.selectedIndex).value;")
    '            oTempFldSB.AppendLine("     var sHTML = '';")
    '            oTempFldSB.AppendLine("     if (value != '') {")
    '            oTempFldSB.AppendLine("         jQuery.ajax({")
    '            oTempFldSB.AppendLine("             async:true,")
    '            oTempFldSB.AppendLine("             url:'/cmsimages/MessageBlastTemplates/' + value,")
    '            oTempFldSB.AppendLine("             success:function(data) {")
    '            oTempFldSB.AppendLine("                 sHTML = data;")
    '            oTempFldSB.AppendLine("                 sHTML = sHTML.replace('#siteurl#', 'http://" & Request.Url.Host & "');")
    '            oTempFldSB.AppendLine("                 editor.setContent(sHTML);")
    '            oTempFldSB.AppendLine("             },")
    '            oTempFldSB.AppendLine("             error:function() {")
    '            oTempFldSB.AppendLine("                 alert('There was a problem loading the requested template.');")
    '            oTempFldSB.AppendLine("             }")
    '            oTempFldSB.AppendLine("         });")
    '            oTempFldSB.AppendLine("         selectObject.selectedIndex = 0;")
    '            oTempFldSB.AppendLine("     }")
    '            oTempFldSB.AppendLine(" }")
    '            oTempFldSB.AppendLine("}")
    '            Page.ClientScript.RegisterClientScriptBlock(GetType(String), "processMessageTemplate", oTempFldSB.ToString, True)

    '        End If
    '    Catch ex As Exception

    '    End Try

    '    '-- Add in custom lookup for Template Fields based on CampaignID
    '    Try
    '        If CampaignID > 0 Then
    '            Dim aDist As New ArrayList
    '            Dim oSel As New OboutInc.Editor.CustomSelect
    '            oSel.Handler = "processTemplateField"
    '            oSel.Options.Add(New OboutInc.Editor.Option("", "Template Merge Fields"))

    '            Dim oSourceA As New appxMessageBlastTableAdapters.ListSourceTableAdapter
    '            Dim oSourceT As appxMessageBlast.ListSourceDataTable = oSourceA.GetCampaignListSources(CampaignID)
    '            If oSourceT.Rows.Count > 0 Then
    '                For Each oSource As appxMessageBlast.ListSourceRow In oSourceT.Rows
    '                    If Not aDist.Contains(oSource.ListSourceID) Then
    '                        aDist.Add(oSource.ListSourceID)

    '                        Dim sQuery As String = oSource.ListQuery
    '                        sQuery = sQuery.Substring(sQuery.IndexOf(" "))
    '                        sQuery = "SELECT TOP 1 " & sQuery.Trim

    '                        Dim oConn As New SqlConnection(oSource.ConnectionString)
    '                        Dim oDs As New DataSet
    '                        Dim oCmd As New SqlCommand(sQuery, oConn)
    '                        Dim oAdapter As New SqlDataAdapter(oCmd)
    '                        oAdapter.Fill(oDs)

    '                        For Each oCol As DataColumn In oDs.Tables(0).Columns
    '                            oSel.Options.Add(New OboutInc.Editor.Option("#" & oCol.ColumnName.ToLower & "#", oCol.ColumnName))
    '                        Next
    '                        oAdapter.Dispose()
    '                        oCmd.Dispose()
    '                        oDs.Dispose()
    '                        oConn.Close()
    '                        oConn.Dispose()
    '                    End If
    '                Next
    '            End If
    '            oSourceT.Dispose()
    '            oSourceA.Dispose()

    '            If aDist.Count > 0 Then
    '                oEd.Buttons.Add(oSel)

    '                Dim oTempFldSB As New StringBuilder
    '                oTempFldSB.AppendLine("function processTemplateField(editor, command, selectObject) {")
    '                oTempFldSB.AppendLine("// changed  value in select")
    '                oTempFldSB.AppendLine("if(command == 'change') {")
    '                oTempFldSB.AppendLine("var value = selectObject.options.item(selectObject.selectedIndex).value;")
    '                oTempFldSB.AppendLine("if (value != '') {")
    '                oTempFldSB.AppendLine("editor.InsertHTML(value);")
    '                oTempFldSB.AppendLine("selectObject.selectedIndex = 0;")
    '                oTempFldSB.AppendLine("}")
    '                oTempFldSB.AppendLine("}")
    '                oTempFldSB.AppendLine("}")
    '                Page.ClientScript.RegisterClientScriptBlock(GetType(String), "processTemplateField", oTempFldSB.ToString, True)
    '            End If
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class
