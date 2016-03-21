Imports System.IO
Imports System.Collections.ObjectModel
Imports System.Collections.Generic
Imports HtmlAgilityPack
Imports Obout.Ajax.UI.HTMLEditor
Imports Obout.Ajax.UI.HTMLEditor.ToolbarButton
Imports Obout.Ajax.UI.HTMLEditor.Popups
Imports Obout.Ajax.UI.HTMLEditor.ContextMenu
Imports System.Text.RegularExpressions
Imports System.Xml
Imports log4net

Partial Class usercontrols_FullEditor
    Inherits System.Web.UI.UserControl

    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Enum CustomLookupModeType
        None = 0
        Survey = 1
    End Enum

#Region "Properties"
    Public ReadOnly Property EditorId As String
        Get
            Return oEd.ID
        End Get
    End Property

    Public ReadOnly Property EditorClientId As String
        Get
            Return oEd.ClientID
        End Get
    End Property

    Private _LookupMode As CustomLookupModeType = CustomLookupModeType.None
    Public Property LookupMode() As CustomLookupModeType
        Get
            Return _LookupMode
        End Get
        Set(ByVal value As CustomLookupModeType)
            _LookupMode = value
        End Set
    End Property

    Private _LookupRefID As Integer = 0
    Public Property LookupRefID() As Integer
        Get
            Return _LookupRefID
        End Get
        Set(ByVal value As Integer)
            _LookupRefID = value
        End Set
    End Property

    Public Property Content() As String
        Get
            Dim sContent As String = oEd.EditPanel.Content
            sContent = MakeRelativePaths(sContent)
            Return sContent
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

    'Public Property PopupHolderId As String
    '    Get
    '        Return oEd.PopupHolderID
    '    End Get
    '    Set(ByVal value As String)
    '        'oEd.PopupHolderID = value
    '    End Set
    'End Property

    Private _LoadCss As Boolean = False
    Public Property LoadCss As Boolean
        Get
            Return _LoadCss
        End Get
        Set(value As Boolean)
            _LoadCss = value
        End Set
    End Property


    Private oSCM As ScriptManager = Nothing
    Private bValidLoad As Boolean = True
#End Region

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        oSCM = ScriptManager.GetCurrent(Page)
        If oSCM Is Nothing Then
            Me.Controls.Clear()
            Me.Controls.Add(New LiteralControl("<p><b style=""color:red;"">" & Me.GetType.ToString & " requires a ScriptManager on the page.</b></p>"))
            bValidLoad = False
        Else
            '-- Find a popupholder control on the page
            Dim oPopupHolder As PopupHolder = DirectCast(appxCMS.Util.ControlHelper.DeepFindControl(Page, "appxCMSEditorOboutPopupHolder"), PopupHolder)
            If oPopupHolder Is Nothing Then
                oPopupHolder = New PopupHolder()
                oPopupHolder.ID = "appxCMSEditorOboutPopupHolder"
                oPopupHolder.DefaultAddPolicy = PopupHolderAddPolicy.Demand
                oPopupHolder.OnClientPopupStateChanged = "popupStateChanged"

                phDynControls.Controls.Add(oPopupHolder)
            End If
            oEd.PopupHolderID = oPopupHolder.ID

            oEd.BottomToolbar.AddButtons.Add(New HorizontalSeparator)
            oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.LayoutTemplate)

            Select Case Me.LookupMode
                Case CustomLookupModeType.Survey
                    If Me.LookupRefID = 0 Then
                        '-- Try to get it from the querystring
                        Me.LookupRefID = pageBase.QStringToInt("surveyid")
                    End If
                    oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.SurveyFields(Me.LookupRefID))
            End Select

            'cInsertImage.RelatedPopupType = 
            oEd.BottomToolbar.AddButtons.Add(New appxCMS.Web.Editor.ControlLibraryButton)
            oEd.EditPanel.ContextMenu.AddItems.Add(New appxCMS.Web.Editor.ControlLibraryEditItem)
            oEd.EditPanel.ContextMenu.AddItems.Add(New appxCMS.Web.Editor.ControlLibraryRemoveItem)
            'oEd.EditPanel.ContextMenu.AddItems.Add(New appxCMS.Web.Editor.ImageMapperEditItem)
            oEd.EditPanel.ContextMenu.AddItems.Add(New appxCMS.Web.Editor.ImageMapperRemoveItem)
        End If

        MyBase.OnInit(e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.bValidLoad Then Exit Sub

        If Me.LoadCss Then
            oEd.EditPanel.DesignPanelCssPath = VirtualPathUtility.ToAbsolute("~/admin/editor/ThemeHandler.ashx")
        End If

        Dim sCustomImageProperties As String = GetType(appxCMS.Web.Editor.ImageProperties).AssemblyQualifiedName
        Dim oInsButtons As Collection(Of CommonButton) = oEd.TopToolbar.GetButtonsByType(GetType(InsertImage))
        If oInsButtons.Count > 0 Then
            Dim oButton As InsertImage = DirectCast(oInsButtons(0), InsertImage)
            oButton.RelatedPopupType = sCustomImageProperties
        End If

        Dim oEdImgButtons As Collection(Of CommonButton) = oEd.EditPanel.ContextMenu.GetButtonsByType(GetType(EditImageItem))
        If oEdImgButtons.Count > 0 Then
            Dim oItem As EditImageItem = DirectCast(oEdImgButtons(0), EditImageItem)
            oItem.RelatedPopupType = sCustomImageProperties
        End If

        Dim oEdSetup As New StringBuilder
        oEdSetup.AppendLine("var appxEditorURLBrowserUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxEditorURLBrowser.aspx") & "';")
        oEdSetup.AppendLine("var appxEditorImageBrowserUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxEditorImageBrowser.aspx") & "';")
        oEdSetup.AppendLine("var appxEditorFlashBrowserUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxEditorFlashBrowser.aspx") & "';")
        oEdSetup.AppendLine("var appxEditorMediaBrowserUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxEditorMediaBrowser.aspx") & "';")
        oEdSetup.AppendLine("var appxEditorImageMapperUrl = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/appxImageMapper.aspx") & "';")
        oEdSetup.AppendLine("var LibInfoPath = '" & VirtualPathUtility.ToAbsolute("~/admin/editor/LibInfo.ashx") & "';")
        If Me.Visible Then
            ScriptManager.RegisterClientScriptBlock(pEd, pEd.GetType, "appxEditorSetup", oEdSetup.ToString, True)
            Dim sEditorInit As String = File.ReadAllText(Server.MapPath("~/admin/editor/appxEditor.js"))
            ScriptManager.RegisterStartupScript(pEd, pEd.GetType, "appxEditorInit", sEditorInit, True)

            '-- Temporary, for imagemapper
            Dim sImgMapperInit As String = File.ReadAllText(Server.MapPath("~/admin/editor/appxEditor-ImageMapper.js"))
            ScriptManager.RegisterStartupScript(pEd, pEd.GetType, "appxEditorInit-ImageMapper", sImgMapperInit, True)

            oSCM.Scripts.Add(New ScriptReference("Obout.Ajax.UI.HTMLEditor.Popups.PopupsCommon.js", "Obout.Ajax.UI"))
            'oSCM.Scripts.Add(New ScriptReference("/admin/editor/appxEditor.js"))
        End If
    End Sub

#Region "Methods"
    Public Shared Function MakeRelativePaths(sContent As String) As String
        Try
            Dim sXHTML As String = sContent
            Dim oHtml As New HtmlAgilityPack.HtmlDocument
            oHtml.OptionFixNestedTags = True
            oHtml.OptionAutoCloseOnEnd = True
            oHtml.OptionWriteEmptyNodes = True
            oHtml.LoadHtml(sXHTML)

            If oHtml.DocumentNode IsNot Nothing Then
                Dim oBaseUrls As List(Of appxCMS.SiteUrl) = appxCMS.SiteDataSource.GetUrlList(appxCMS.Util.CMSSettings.GetSiteId())
                Dim oNodes As HtmlNodeCollection = oHtml.DocumentNode.SelectNodes("//*[@src or @href]")
                If oNodes IsNot Nothing Then
                    For Each oNode As HtmlNode In oNodes
                        If oNode.Attributes.Contains("src") Then
                            Dim sSrc As String = oNode.GetAttributeValue("src", "")
                            For Each oUrl As appxCMS.SiteUrl In oBaseUrls
                                Dim sBaseUrl As String = "http://" & oUrl.Url.ToLower()
                                Dim sBaseSecure As String = "https://" & oUrl.Url.ToLower()
                                If sSrc.ToLower.StartsWith(sBaseUrl) Or sSrc.ToLower.StartsWith(sBaseSecure) Then
                                    sSrc = Regex.Replace(sSrc, "http(s?)://" & oUrl.Url, "", RegexOptions.IgnoreCase)
                                    If Not sSrc.StartsWith("/") Then
                                        sSrc = "/" & sSrc
                                    End If
                                    oNode.SetAttributeValue("src", sSrc)
                                End If
                            Next
                        ElseIf oNode.Attributes.Contains("href") Then
                            Dim sHref As String = oNode.GetAttributeValue("href", "")
                            For Each oUrl As appxCMS.SiteUrl In oBaseUrls
                                Dim sBaseUrl As String = "http://" & oUrl.Url.ToLower()
                                Dim sBaseSecure As String = "https://" & oUrl.Url.ToLower()
                                If sHref.ToLower.StartsWith(sBaseUrl) Or sHref.ToLower.StartsWith(sBaseSecure) Then
                                    sHref = Regex.Replace(sHref, "http(s?)://" & oUrl.Url, "", RegexOptions.IgnoreCase)
                                    If Not sHref.StartsWith("/") Then
                                        sHref = "/" & sHref
                                    End If
                                    oNode.SetAttributeValue("href", sHref)
                                End If
                            Next
                        End If
                    Next
                End If

                Using oSr As New StringWriter
                    oHtml.Save(oSr)
                    sContent = oSr.ToString()
                End Using
            End If

        Catch ex As Exception
            Log.Error(ex.Message, ex)
        End Try

        Return sContent
    End Function

#End Region
End Class
