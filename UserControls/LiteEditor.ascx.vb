Imports System.IO
Imports System.Collections.Generic
Imports Obout.Ajax.UI.HTMLEditor
Imports Obout.Ajax.UI.HTMLEditor.ToolbarButton
Imports Obout.Ajax.UI.HTMLEditor.Popups
Imports Obout.Ajax.UI.HTMLEditor.ContextMenu
Imports System.Text.RegularExpressions

Partial Class usercontrols_LiteEditor
    Inherits System.Web.UI.UserControl

    Public Property Content() As String
        Get
            Return oEd.EditPanel.Content
        End Get
        Set(ByVal value As String)
            oEd.EditPanel.Content = value
        End Set
    End Property

    Private _quickListItems As String = ""
    Public Property QuickListItems As String
        Get
            Return _quickListItems
        End Get
        Set(value As String)
            _quickListItems = value
        End Set
    End Property

    Private _quickListLabel As String = ""
    Public Property QuickListLabel As String
        Get
            Return _quickListLabel
        End Get
        Set(value As String)
            _quickListLabel = value
        End Set
    End Property

    Private oSCM As ScriptManager = Nothing
    Private bValidLoad As Boolean = True

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        oSCM = ScriptManager.GetCurrent(Page)
        If oSCM Is Nothing Then
            Me.Controls.Clear()
            Me.Controls.Add(New LiteralControl("<p><b style=""color:red;"">" & Me.GetType.ToString & " requires a ScriptManager on the page.</b></p>"))
            bValidLoad = False
        Else
            If Not String.IsNullOrEmpty(QuickListItems) Then
                Dim oItemList As New List(Of String)
                oItemList.AddRange(QuickListItems.Split(New Char() {",", ";"}))
                oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.CustomFields(oItemList, QuickListLabel))
            End If
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
            ScriptManager.RegisterClientScriptBlock(oEd, oEd.GetType, "appxEditorSetup", oEdSetup.ToString, True)
            Dim sEditorInit As String = File.ReadAllText(Server.MapPath("~/admin/editor/appxEditor.js"))
            ScriptManager.RegisterStartupScript(oEd, oEd.GetType, "appxEditorInit", sEditorInit, True)
            oSCM.Scripts.Add(New ScriptReference("Obout.Ajax.UI.HTMLEditor.Popups.PopupsCommon.js", "Obout.Ajax.UI"))
        End If
    End Sub
End Class
