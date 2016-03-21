Imports System.IO
Imports Obout.Ajax.UI.HTMLEditor
Imports Obout.Ajax.UI.HTMLEditor.ToolbarButton
Imports Obout.Ajax.UI.HTMLEditor.Popups
Imports Obout.Ajax.UI.HTMLEditor.ContextMenu
Imports System.Text.RegularExpressions
Imports System.Xml
Imports Sgml.SgmlReader

Partial Class usercontrols_EmailEditor
    Inherits System.Web.UI.UserControl

    Public Property TableRef() As String
        Get
            Return _TableRef
        End Get
        Set(ByVal value As String)
            _TableRef = value
        End Set
    End Property
    Private _TableRef As String = ""

    Public Property RefID() As String
        Get
            Return _RefID
        End Get
        Set(ByVal value As String)
            _RefID = value
        End Set
    End Property
    Private _RefID As String = ""

    Public Property Content() As String
        Get
            Dim sContent As String = oEd.EditPanel.Content

            Dim sUrlBase As String = "http://" & Request.Url.Host
            Try
                Dim sXHTML As String = sContent
                Using oMs As New MemoryStream(UTF8Encoding.Default.GetBytes("<cmsEmailTemplate>" & sContent & "</cmsEmailTemplate>"))
                    Using oTR As TextReader = New StreamReader(oMs)
                        Dim oSGMLReader As New Sgml.SgmlReader
                        oSGMLReader.DocType = "HTML"
                        oSGMLReader.InputStream = oTR
                        sXHTML = oSGMLReader.ReadOuterXml
                    End Using
                End Using

                Dim oDoc As New XmlDocument
                oDoc.LoadXml(sXHTML)

                Dim oNodes As XmlNodeList = oDoc.SelectNodes("//*[@src or @href]")
                For Each oNode As XmlNode In oNodes
                    If oNode.Attributes.GetNamedItem("src") IsNot Nothing Then
                        Dim sSrc As String = xmlhelp.ReadAttribute(oNode, "src")
                        If sSrc.StartsWith("/") Then
                            '-- Convert to fully qualified
                            Dim sNewSrc As String = sUrlBase & sSrc
                            xmlhelp.AddOrUpdateXMLAttribute(oNode, "src", sNewSrc)
                        End If
                    ElseIf oNode.Attributes.GetNamedItem("href") IsNot Nothing Then
                        Dim sHref As String = xmlhelp.ReadAttribute(oNode, "href")
                        If sHref.StartsWith("/") Then
                            '-- Convert to fully qualified
                            xmlhelp.AddOrUpdateXMLAttribute(oNode, "href", sUrlBase & sHref)
                        End If
                    End If
                Next
                Dim oTemplate As XmlNode = oDoc.SelectSingleNode("//cmsEmailTemplate")
                sContent = oTemplate.InnerXml
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try

            sContent = sContent.Replace(sUrlBase, "#siteurl#")

            Return sContent
        End Get
        Set(ByVal value As String)
            Dim sContent As String = value
            If Not String.IsNullOrEmpty(sContent) Then
                sContent = sContent.Replace("#siteurl#", "http://" & Request.Url.Host)
            End If
            oEd.EditPanel.Content = sContent
        End Set
    End Property

    Public ReadOnly Property PlainText() As String
        Get
            Return oEd.PlainText
        End Get
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
            oEd.EditPanel.FullHtml = True
            oEd.BottomToolbar.AddButtons.Add(New HorizontalSeparator)
            oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.MessageBlastTemplate)
            '    oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.MessageBlastTemplateFields)
        End If

        MyBase.OnInit(e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.bValidLoad Then Exit Sub

        Dim oMeta As New StringBuilder
        oMeta.AppendLine("")
        oMeta.AppendLine("<!--[if IE 9]>")
        oMeta.AppendLine("<meta http-equiv=""X-UA-Compatible"" content=""IE=EmulateIE8"">")
        oMeta.AppendLine("<![endif]-->")
        oMeta.AppendLine("")
        Page.Header.Controls.AddAt(0, New LiteralControl(oMeta.ToString))

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
End Class
