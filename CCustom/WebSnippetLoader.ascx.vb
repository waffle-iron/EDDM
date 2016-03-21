
Imports System.IO
Imports System.Xml
Imports log4net

Partial Class CCustom_WebSnippetLoader
    Inherits CLibraryBase

    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _cacheId As String = ""
    Public Property CacheId As String
        Get
            Return _cacheId
        End Get
        Set(value As String)
            _cacheId = value
        End Set
    End Property

    Private _loadUrl As String = ""
    Public Property LoadUrl As String
        Get
            Return _loadUrl
        End Get
        Set(value As String)
            _loadUrl = value
        End Set
    End Property

    Private _snippetSelectorTag As String = ""
    Public Property SnippetSelectorTag As String
        Get
            Return _snippetSelectorTag
        End Get
        Set(value As String)
            _snippetSelectorTag = value
        End Set
    End Property

    Private _snippetSelectorAttribute As String = ""
    Public Property SnippetSelectorAttribute As String
        Get
            Return _snippetSelectorAttribute
        End Get
        Set(value As String)
            _snippetSelectorAttribute = value
        End Set
    End Property

    Private _snippetSelectorAttributeValue As String = ""
    Public Property SnippetSelectorAttributeValue As String
        Get
            Return _snippetSelectorAttributeValue
        End Get
        Set(value As String)
            _snippetSelectorAttributeValue = value
        End Set
    End Property

    Private _stripScriptTags As Boolean = False
    Public Property StripScriptTags As Boolean
        Get
            Return _stripScriptTags
        End Get
        Set(value As Boolean)
            _stripScriptTags = value
        End Set
    End Property

    Protected Overrides Sub BuildControl()
        Dim bBurst As Boolean = appxCMS.Util.Querystring.GetBoolean("burst")

        Dim sCk As String = "WebSnippet:SiteId: " & appxCMS.Util.CMSSettings.GetSiteId() & ":" & CacheId
        Dim sSnippet As String = appxCMS.Util.Cache.GetString(sCk)
        If String.IsNullOrEmpty(sSnippet) Or bBurst Then
            Try
                '-- Retrieve and parse out the selected text
                Dim sPage As String = appxCMS.Util.httpHelp.GetXMLURLPage(LoadUrl)
                Dim sXHTML As String = sPage
                Using oMs As New MemoryStream(UTF8Encoding.Default.GetBytes(sPage))
                    Using oTR As TextReader = New StreamReader(oMs)
                        Dim oSGMLReader As New Sgml.SgmlReader
                        oSGMLReader.DocType = "HTML"
                        oSGMLReader.InputStream = oTR
                        sXHTML = oSGMLReader.ReadOuterXml
                    End Using
                End Using

                Dim oXml As New XmlDocument
                oXml.LoadXml(sXHTML)

                If StripScriptTags Then
                    Dim oScriptTags As XmlNodeList = oXml.SelectNodes("//script")
                    For iTag As Integer = oScriptTags.Count - 1 To 0 Step -1
                        Dim oScriptTag As XmlNode = oScriptTags(iTag)
                        oScriptTag.ParentNode.RemoveChild(oScriptTag)
                    Next
                End If

                Dim oNode As XmlNode = oXml.SelectSingleNode("//" & SnippetSelectorTag & "[@" & SnippetSelectorAttribute & "='" & SnippetSelectorAttributeValue & "']")

                If oNode IsNot Nothing Then
                    sSnippet = oNode.OuterXml
                End If

                appxCMS.Util.Cache.Add(sCk, sSnippet)
            Catch ex As Exception
                Log.Error(ex.Message, ex)
                sSnippet = ""
                lSnippet.Text = ex.Message
            End Try
        End If

        If Not String.IsNullOrEmpty(sSnippet) Then
            lSnippet.Text = sSnippet
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
