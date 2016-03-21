Imports System.IO
Imports Obout.Ajax.UI.HTMLEditor
Imports Obout.Ajax.UI.HTMLEditor.ToolbarButton
Imports Obout.Ajax.UI.HTMLEditor.Popups
Imports Obout.Ajax.UI.HTMLEditor.ContextMenu
Imports System.Text.RegularExpressions

Partial Class usercontrols_NewsWidgetEditor
    Inherits System.Web.UI.UserControl

    Private oSCM As ScriptManager = Nothing
    Private bValidLoad As Boolean = True

    Public Property Content() As String
        Get
            Return oEd.EditPanel.Content
        End Get
        Set(ByVal value As String)
            '-- Clean up any legacy CLib refs without the <clibrary> wrapper
            If Not String.IsNullOrEmpty(value) Then
                Dim oRe As New Regex("(?:(?<!<clibrary[^>]{0,}>))(\[CLibrary/[^\]]+\])")
                value = oRe.Replace(value, "<clibrary contentEditable=""false"">$1</clibrary>")
            End If
            If oEd IsNot Nothing Then
                oEd.EditPanel.Content = value
            End If
            oEd.EditPanel.Content = value
        End Set
    End Property

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        oSCM = ScriptManager.GetCurrent(Page)
        If oSCM Is Nothing Then
            Me.Controls.Clear()
            Me.Controls.Add(New LiteralControl("<p><b style=""color:red;"">" & Me.GetType.ToString & " requires a ScriptManager on the page.</b></p>"))
            bValidLoad = False
        Else
            oEd.BottomToolbar.AddButtons.Add(New HorizontalSeparator)
            oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.LayoutTemplate)

            oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.ControlLibraryButton)
            oEd.EditPanel.ContextMenu.AddItems.Add(New appxCMS.Web.Editor.ControlLibraryEditItem)
            oEd.EditPanel.ContextMenu.AddItems.Add(New appxCMS.Web.Editor.ControlLibraryRemoveItem)
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
        oEdSetup.AppendLine("var LibInfoPath = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/LibInfo.ashx") & "';")
        If Me.Visible Then
            ScriptManager.RegisterClientScriptBlock(oEd, oEd.GetType, "appxEditorSetup", oEdSetup.ToString, True)
            Dim sEditorInit As String = File.ReadAllText(Server.MapPath("~/admin/editor/appxEditor.js"))
            ScriptManager.RegisterStartupScript(oEd, oEd.GetType, "appxEditorInit", sEditorInit, True)
            oSCM.Scripts.Add(New ScriptReference("Obout.Ajax.UI.HTMLEditor.Popups.PopupsCommon.js", "Obout.Ajax.UI"))
        End If
    End Sub
End Class
