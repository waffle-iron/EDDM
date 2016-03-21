Imports System.IO
Imports appxEncrypt
Imports System.Reflection
'Imports SgmlReaderDll
Imports System.Xml

Public Class pageBase
    Inherits System.Web.UI.Page

    Protected myObfuscator As New MatchEvaluator(AddressOf EmailAddressObfuscator)

    Private sPageTitle As String = ""
    Private sPageRef As String = ""
    Private sPage As String = ""
    Private bVirtual As Boolean = False
    Private sRequestPage As String = ""

    Public Shared ReadOnly Property LoggedOnUserID() As Integer
        Get
            Dim sUserType As String = ""
            Dim iUserId As Integer = 0
            Dim sUser As String = HttpContext.Current.User.Identity.Name
            If Not String.IsNullOrEmpty(sUser) Then
                Using oA As New appxAuthTableAdapters.AdminTableAdapter
                    iUserId = oA.GetAccountIdForUsername(sUser)
                End Using
            End If

            Return iUserId
        End Get
    End Property

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        '-- Set the page theme based on the stored configuration value
        'Dim sTheme As String = appxCMS.Util.CMSSettings.GetSetting("Site", "Theme")
        'If Not String.IsNullOrEmpty(sTheme) Then
        '    Page.Theme = sTheme
        'End If
    End Sub
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        sPage = GetRequestedURL()
        If String.IsNullOrEmpty(sPage) Then
            sPage = VirtualPathUtility.ToAbsolute(Page.AppRelativeVirtualPath)
        End If
        sRequestPage = sPage
        If sRequestPage.Contains("?") Then
            sRequestPage = sPage.Substring(0, sPage.IndexOf("?"))
        End If

        Dim sPagePath As String = Server.MapPath(sRequestPage)
        If Not File.Exists(sPagePath) Or sPage.ToLowerInvariant.StartsWith("/vpage_") Then
            bVirtual = True
        End If

        sPage = Server.UrlDecode(sPage)

        sPageRef = Server.UrlDecode(sRequestPage)

        '-- Virtual Content Areas
        Using oCMSA As New appxCMSDataTableAdapters.ContentTableAdapter
            Using oCMST As appxCMSData.ContentDataTable = oCMSA.GetUnifiedPageContent(sPageRef)
                If oCMST.Rows.Count > 0 Then
                    For Each oContentRow As appxCMSData.ContentRow In oCMST.Rows
                        Dim sCArea As String = oContentRow.ContentArea
                        Dim sContent As String = ""
                        If Not oContentRow.IsContentDataNull Then
                            sContent = oContentRow.ContentData
                        End If

                        Dim oMC As Control = Nothing
                        Dim oBase As MasterPage = Page.Master
                        Do While oBase IsNot Nothing
                            oMC = oBase.FindControl(sCArea)
                            If oMC IsNot Nothing Then
                                '-- Does this contain a control that matches our search?
                                oMC = cmshelp.RecursiveControlSearch(oMC, sCArea)
                                Exit Do
                            End If
                            If oBase.Master Is Nothing Then
                                Exit Do
                            End If
                            oBase = oBase.Master
                        Loop

                        If oMC Is Nothing Then
                            oMC = cmshelp.DeepFindContentPlaceHolder(oBase, sCArea)
                        End If

                        If oMC IsNot Nothing Then
                            oMC.Controls.Add(FormatContent(sContent))
                        End If
                    Next
                End If
            End Using
        End Using

        Using oCMSA As New appxCMSDataTableAdapters.ContentMessageTableAdapter
            Using oCMST As appxCMSData.ContentMessageDataTable = oCMSA.GetPageContent(sPageRef)
                For Each oCMSRow As appxCMSData.ContentMessageRow In oCMST.Rows
                    If Not oCMSRow.IsContentNull Then
                        Dim sContent As String = oCMSRow.Content.Trim

                        If Not String.IsNullOrEmpty(sContent) Then
                            '-- Clean content, using any content filters we are applying
                            sContent = TextFormatContent(sContent)

                            Dim sClass As String = ""
                            If Not oCMSRow.IsContentClassNull Then
                                sClass = oCMSRow.ContentClass
                            End If

                            Dim sPH As String = oCMSRow.Placeholder
                            Dim oPH As Control = apphelp.DeepFindControl(Page.Master, sPH)
                            If oPH IsNot Nothing Then
                                If Not String.IsNullOrEmpty(sClass) Then
                                    oPH.Controls.Add( _
                                                      New LiteralControl( _
                                                                          "<div class=""" & sClass & """>" & sContent & _
                                                                          "</div>"))
                                Else
                                    oPH.Controls.Add(FormatContent(sContent))
                                End If
                            End If
                        End If
                    End If
                Next
            End Using
        End Using

        If bVirtual Then
            Dim bExpired As Boolean = False
            Dim sExpAction As String = ""
            Dim sContent As String = ""
            Dim sTargetId As String = "phBody"
            Using oCMSA As New appxCMSDataTableAdapters.VirtualTableAdapter
                Using oCMST As appxCMSData.VirtualDataTable = oCMSA.GetPage(sPageRef)
                    If oCMST.Rows.Count > 0 Then
                        Dim oVRow As appxCMSData.VirtualRow = oCMST.Rows(0)
                        If Not oVRow.IsContentDataNull Then
                            sContent = oVRow.ContentData
                        End If
                        If Not oVRow.IsExpirationDateNull Then
                            Dim dExp As DateTime = oVRow.ExpirationDate
                            Dim dExpiration As DateTime = New DateTime(dExp.Year, dExp.Month, dExp.Day, 23, 59, 59)
                            If dExpiration < System.DateTime.Now Then
                                bExpired = True
                                If Not oVRow.IsExpirationActionNull Then
                                    sExpAction = oVRow.ExpirationAction
                                Else
                                    sExpAction = "Redirect"
                                End If

                                If Not oVRow.IsExpirationContentNull Then
                                    sContent = oVRow.ExpirationContent
                                Else
                                    sExpAction = "Redirect"
                                    sContent = "/"
                                End If
                            End If
                        End If
                    End If
                End Using

                If bExpired And sExpAction.ToLowerInvariant = "redirect" Then
                    linkHelp.Redirect301(sContent, True)
                End If

                If Not String.IsNullOrEmpty(sContent) Then
                    Dim oMC As Control = Nothing

                    Dim oBase As MasterPage = Page.Master
                    Do While oBase IsNot Nothing
                        oMC = oBase.FindControl(sTargetId)
                        If oMC IsNot Nothing Then
                            '-- Does this contain a control that matches our search?
                            oMC = cmshelp.RecursiveControlSearch(oMC, sTargetId)
                            Exit Do
                        End If
                        If oBase.Master Is Nothing Then
                            Exit Do
                        End If
                        oBase = oBase.Master
                    Loop
                    If oMC Is Nothing Then
                        oMC = cmshelp.DeepFindContentPlaceHolder(oBase, sTargetId)
                    End If
                    If oMC IsNot Nothing Then
                        oMC.Controls.Add(FormatContent(sContent))
                    End If
                End If
            End Using

        End If
        jqueryHelper.Include(Page)
        jqueryHelper.IncludePlugin(Page, "common", "/scripts/common.js")
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim aLinks As New ArrayList
        Dim oHead As HtmlHead = Page.Header
        Dim oCEnum As IEnumerator = oHead.Controls.GetEnumerator
        While oCEnum.MoveNext
            If TypeOf oCEnum.Current Is HtmlLink Then
                Dim oLnk As HtmlLink = DirectCast(oCEnum.Current, HtmlLink)
                Dim sOHref As String = oLnk.Href
                If sOHref.ToLower.IndexOf("app_themes") >= 0 Then
                    Dim sNewHref As String = VirtualPathUtility.ToAbsolute(sOHref)
                    oLnk.Href = sNewHref
                End If
            End If
        End While
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Using oPgA As New appxCMSDataTableAdapters.ContentPageTableAdapter
            Using oPgT As appxCMSData.ContentPageDataTable = oPgA.GetByPageRef(sPageRef)
                If oPgT.Rows.Count > 0 Then
                    Dim oPg As appxCMSData.ContentPageRow = oPgT.Rows(0)

                    sPageTitle = oPg.PageTitle
                    Page.Title = sPageTitle

                    Dim oMeta As New StringBuilder
                    If Not oPg.IsMeta_AbstractNull Then
                        oMeta.AppendLine("<meta name=""abstract"" content=""" & oPg.Meta_Abstract & """/>")
                    End If
                    If Not oPg.IsMeta_AuthorNull Then
                        oMeta.AppendLine("<meta name=""author"" content=""" & oPg.Meta_Author & """/>")
                    End If
                    If Not oPg.IsMeta_CopyrightNull Then
                        oMeta.AppendLine("<meta name=""copyright"" content=""" & oPg.Meta_Copyright & """/>")
                    End If
                    If Not oPg.IsMeta_DescriptionNull Then
                        oMeta.AppendLine("<meta name=""description"" content=""" & oPg.Meta_Description & """/>")
                    End If
                    If Not oPg.IsMeta_KeywordNull Then
                        oMeta.AppendLine("<meta name=""keywords"" content=""" & oPg.Meta_Keyword & """/>")
                    End If

                    If Page.Header IsNot Nothing Then
                        Page.Header.Controls.Add(New LiteralControl(oMeta.ToString))
                    End If

                    If Not oPg.IsScriptResourceNull Then
                        Page.Header.Controls.Add(New LiteralControl(oPg.ScriptResource & ControlChars.CrLf))
                    End If
                End If
            End Using
        End Using

        Dim sTitle As String = Page.Title
        If String.IsNullOrEmpty(sTitle) Or sTitle.ToLower = "untitled page" Then
            If String.IsNullOrEmpty(sPageTitle) Or sPageTitle.ToLower = "untitled page" Then
                If ConfigurationManager.AppSettings("SiteTitle") IsNot Nothing Then
                    sPageTitle = ConfigurationManager.AppSettings("SiteTitle")
                End If
                If String.IsNullOrEmpty(sPageTitle) Then
                    sPageTitle = "AppX CMS"
                End If
            End If
            Page.Title = sPageTitle
        End If

        Dim sAbsPage As String = sPage
        If sAbsPage.StartsWith("/") Then
            sAbsPage = sAbsPage.Substring(1)
        End If
        If sAbsPage.Contains("?") Then
            sAbsPage = sAbsPage.Substring(0, sAbsPage.IndexOf("?"))
        End If

        If File.Exists(Server.MapPath("~/scripts/" & sAbsPage.Replace(".aspx", ".js"))) Then
            jqueryHelper.IncludePlugin(Page, sAbsPage.Replace(".aspx", ".js"), "~/scripts/" & sAbsPage.Replace(".aspx", ".js"))
        End If

        If File.Exists(Server.MapPath("~/app_styles/" & sAbsPage.Replace(".aspx", ".css"))) Then
            jqueryHelper.RegisterStylesheet(Page, "~/app_styles/" & sAbsPage.Replace(".aspx", ".css"))
        End If
    End Sub

    Public Shared Function QStringToInt(ByVal sQFld As String) As Integer
        Dim sid As String = ""
        If HttpContext.Current.Request.QueryString(sQFld) IsNot Nothing Then
            sid = HttpContext.Current.Request.QueryString(sQFld)
        End If
        Dim id As Integer = 0
        Integer.TryParse(sid, id)
        Return id
    End Function

    Public Shared Function FStringToInt(ByVal sFFld As String) As Integer
        Dim sId As String = FStringToVal(sFFld)
        Dim iRet As Integer = 0
        Integer.TryParse(sId, iRet)
        Return iRet
    End Function
    Public Shared Function QStringToDecimal(ByVal sQFld As String) As Decimal
        Dim sid As String = QStringToVal(sQFld)
        Dim dRet As Decimal = 0
        Decimal.TryParse(sid, dRet)
        Return dRet
    End Function
    Public Shared Function FStringToDecimal(ByVal sFFld As String) As Decimal
        Dim sRet As String = FStringToVal(sFFld)
        Dim dRet As Decimal = 0
        Decimal.TryParse(sRet, dRet)
        Return dRet
    End Function
    Public Shared Function QStringToVal(ByVal sQFld As String) As String
        Dim sQVal As String = ""
        If HttpContext.Current.Request.QueryString(sQFld) IsNot Nothing Then
            sQVal = HttpContext.Current.Request.QueryString(sQFld)
        End If
        Return sQVal
    End Function
    Public Shared Function FStringToVal(ByVal sFFld As String) As String
        Dim sFVal As String = ""
        If HttpContext.Current.Request.Form(sFFld) IsNot Nothing Then
            sFVal = HttpContext.Current.Request.Form(sFFld)
        End If
        Return sFVal
    End Function

    Public Shared Function QStringToDate(ByVal queryString As String, ByVal defaultDate As DateTime) As DateTime
        Dim mdate As DateTime = defaultDate
        If HttpContext.Current.Request.QueryString(queryString) IsNot Nothing Then
            Dim sid As String = QStringToVal(queryString)
            If Not DateTime.TryParse(sid, mdate) Then
                mdate = defaultDate
            End If
        End If
        Return mdate
    End Function

    Public Shared Function QStringToDate(ByVal queryString As String) As DateTime
        Return QStringToDate(queryString, DateTime.Now)
    End Function


    Public Function TextFormatContent(ByVal sContent As String) As String
        Dim sC As String = sContent
        sC = EmbedMediaContent(sC)
        sC = ObfuscateEmail(sC)
        Return sC
    End Function

    Public Function FormatContent(ByVal sContent As String) As PlaceHolder
        Dim sC As String = sContent
        sC = EmbedMediaContent(sC)
        sC = ObfuscateEmail(sC)
        Dim oPH As PlaceHolder = EmbedControls(sC)
        Return oPH
    End Function

    Private Function EmbedMediaContent(ByVal sContent As String) As String
        Dim sProtocol As String = IIf(Request.IsSecureConnection, "https://", "http://")
        Dim _
            sRunContentBase As String = _
                "AC_FL_RunContent( 'codebase','" & sProtocol & "download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0','width','{0}','height','{1}','src','{2}','quality','high','wmode','transparent','pluginspage','http://www.macromedia.com/go/getflashplayer','movie','{2}' );"

        Try
            Dim oEmbedRE As New Regex("(<embed[^>]+(</embed>|/>))")
            Dim oEmbedMatches As MatchCollection = oEmbedRE.Matches(sContent)
            If oEmbedMatches.Count > 0 Then
                Page.ClientScript.RegisterClientScriptInclude("AC_RunActiveContent", _
                                                               VirtualPathUtility.ToAbsolute( _
                                                                                              "~/scripts/AC_RunActiveContent.js"))

                For Each oEmbedMatch As Match In oEmbedMatches

                    Dim sEmbed As String = oEmbedMatch.Value
                    Dim sXHTML As String = sEmbed
                    Using oMs As New MemoryStream(UTF8Encoding.Default.GetBytes(sEmbed))
                        Using oTR As TextReader = New StreamReader(oMs)
                            Dim oSGMLReader As New Sgml.SgmlReader
                            oSGMLReader.DocType = "HTML"
                            oSGMLReader.InputStream = oTR
                            sXHTML = oSGMLReader.ReadOuterXml
                        End Using
                    End Using

                    Dim oXML As New XmlDocument
                    oXML.LoadXml(sXHTML)

                    Dim oEmbed As XmlNode = oXML.SelectSingleNode("embed")
                    Dim sWidth As String = xmlhelp.ReadAttribute(oEmbed, "width")
                    Dim sHeight As String = xmlhelp.ReadAttribute(oEmbed, "height")
                    Dim sID As String = "embedVideo" & oEmbedMatch.Index
                    Dim sSrc As String = xmlhelp.ReadAttribute(oEmbed, "src")
                    sSrc = sSrc.ToLower.Replace(".swf", "")

                    Dim sEmbedStr As String = String.Format(sRunContentBase, sWidth, sHeight, sSrc)
                    'width = 0
                    'height = 1
                    'id = 2
                    'src = 3

                    '-- Flash Object
                    Dim oSb As New StringBuilder
                    oSb.AppendLine("<script language=""javascript"">")
                    oSb.AppendLine("if (AC_FL_RunContent == 0) {")
                    oSb.AppendLine("alert('This page requires AC_RunActiveContent.js.');")
                    oSb.AppendLine("} else {")
                    oSb.AppendLine(sEmbedStr & " //end AC code")
                    oSb.AppendLine("}")
                    oSb.AppendLine("</script>")
                    oSb.AppendLine("<noscript>")
                    oSb.AppendLine(sEmbed)
                    oSb.AppendLine("</noscript>")

                    sContent = sContent.Replace(sEmbed, oSb.ToString)
                Next
            End If

        Catch ex As Exception
            'Response.Write(ex.Message & "<br/>" & ex.StackTrace)
        End Try

        Return sContent
    End Function

    Private Function EmbedControls(ByVal sContent As String) As PlaceHolder
        '-- Replace all control placeholders with rendered controls
        'Dim sDyn As String = "\[((CLibrary|CCustom)[^\]]*)\]"
        Dim sDyn As String = "<clibrary>\[((CLibrary|CCustom)[^\]]*)\]<\/clibrary>|\[((CLibrary|CCustom)[^\]]*)\]"
        Dim oDyn As New Regex(sDyn, RegexOptions.Multiline Or RegexOptions.IgnoreCase)

        Dim aControls As New ArrayList
        Dim oMatches As MatchCollection = oDyn.Matches(sContent)
        Dim sTmpContent As String = sContent

        Try
            For Each oMatch As Match In oMatches
                Dim sLiteralContent As String = sTmpContent.Substring(0, sTmpContent.IndexOf(oMatch.Value))
                'Response.Write("<p>Adding control with: " & Server.HtmlEncode(sLiteralContent) & "</p>")
                Dim oLtl As New LiteralControl(sLiteralContent)
                aControls.Add(oLtl)
                aControls.Add(DynamicControlLoader(oMatch))
                sTmpContent = sTmpContent.Substring(sLiteralContent.Length + oMatch.Value.Length)
            Next
            If Not String.IsNullOrEmpty(sTmpContent) Then
                aControls.Add(New LiteralControl(sTmpContent))
            End If
        Catch ex As Exception
            aControls.Add(New LiteralControl(ex.Message))
        End Try

        Dim oPH As New PlaceHolder
        For iControl As Integer = 0 To aControls.Count - 1
            oPH.Controls.Add(aControls(iControl))
        Next
        Return oPH
    End Function

    Protected Function ObfuscateEmail(ByVal sContent As String) As String
        '-- Replace all mailto hyperlinks with encrypted data/javascript decoding
        Dim pattern As String = "<a[^>]+href=""mailto:[^""]+""[^>]*>(.*?)</a>"
        Dim re As New Regex(pattern, RegexOptions.Multiline And RegexOptions.IgnoreCase)
        Dim sRet As String = re.Replace(sContent, myObfuscator)

        Return sRet
    End Function

    Protected Function EmailAddressObfuscator(ByVal m As Match) As String
        Dim oRan As New Random
        Dim sRan As String = oRan.Next().ToString
        Dim oXOrCrypto As New XOrCrypto
        oXOrCrypto.Key = sRan
        Dim sEnc As String = oXOrCrypto.Encode(m.Value)

        Dim oSb As New StringBuilder
        oSb.AppendLine("<script type=""text/javascript""><!--")
        oSb.AppendLine("document.write(xordecode('" & sRan & "', '" & sEnc & "'));")
        oSb.AppendLine("//-->")
        oSb.AppendLine("</script>")
        oSb.AppendLine("<noscript><a href=""/ContactUs"">Contact Form</a></noscript>")
        jqueryHelper.IncludePlugin(Page, "xordecode", "/scripts/xordecode.js")
        Return oSb.ToString
    End Function

    Private Function DynamicControlLoader(ByVal m As Match) As Control
        Dim sLibCall As String = m.Groups(1).Captures(0).Value

        Dim aLibArgs As New Hashtable
        Dim sLib As String = ""
        Dim sLibArgs As String = ""
        If sLibCall.IndexOf(" ") > 0 Then
            sLib = sLibCall.Substring(0, sLibCall.IndexOf(" ", 0))
            sLibArgs = sLibCall.Replace(sLib, "").Trim
            Dim sLibArgPairs() As String = sLibArgs.Split(" ")
            For iArg As Integer = 0 To sLibArgPairs.Length - 1
                Dim sPair() As String = sLibArgPairs(iArg).Split("=")
                aLibArgs.Add(sPair(0), sPair(1))
            Next
        Else
            sLib = sLibCall
        End If

        Dim oControl As Control = Nothing
        If File.Exists(Server.MapPath("/" & sLib & ".ascx")) Then
            Try
                Dim oType As Type
                oControl = LoadControl("~/" & sLib & ".ascx")
                oType = oControl.GetType()
                Dim oEnum As IDictionaryEnumerator = aLibArgs.GetEnumerator
                While oEnum.MoveNext
                    ' Get access to the property 
                    Dim oArg As PropertyInfo = oType.GetProperty(oEnum.Key)
                    If oArg Is Nothing Then
                        Dim aProps() As PropertyInfo = oType.GetProperties
                        For iProp As Integer = 0 To aProps.Length - 1
                            If aProps(iProp).ToString.ToLower = oEnum.Key.ToString.ToLower Then
                                oArg = oType.GetProperty(aProps(iProp).ToString)
                                Exit For
                            End If
                        Next
                    End If

                    If oArg IsNot Nothing Then
                        ' Set the property 
                        '-- Need to set the type-specific value
                        If oArg.PropertyType.IsEnum Then
                            oArg.SetValue(oControl, [Enum].Parse(oArg.PropertyType, Server.UrlDecode(oEnum.Value)), Nothing)
                        Else
                            Dim oArgType As System.Type = oArg.PropertyType
                            If oArgType Is GetType(Boolean) Then
                                oArg.SetValue(oControl, Boolean.Parse(Server.UrlDecode(oEnum.Value)), Nothing)
                            ElseIf oArgType Is GetType(Integer) Then
                                oArg.SetValue(oControl, Integer.Parse(Server.UrlDecode(oEnum.Value)), Nothing)
                            ElseIf oArgType Is GetType(Decimal) Then
                                oArg.SetValue(oControl, Decimal.Parse(Server.UrlDecode(oEnum.Value)), Nothing)
                            ElseIf oArgType Is GetType(DateTime) Then
                                oArg.SetValue(oControl, DateTime.Parse(Server.UrlDecode(oEnum.Value)), Nothing)
                            Else
                                '-- Allow it to automagically convert to the proper type
                                oArg.SetValue(oControl, Server.UrlDecode(oEnum.Value), Nothing)
                            End If
                        End If
                    End If
                End While
                Dim oExec As MethodInfo = oType.GetMethod("BuildControl")
                If oExec IsNot Nothing Then
                    oExec.Invoke(oControl, Nothing)
                End If
            Catch ex As Exception
                oControl = New LiteralControl("<b>[" & sLib & "] Load Error:</b><br/>" & ex.Message)
            End Try
        Else
            oControl = New LiteralControl("Control not found: " & sLib)
        End If

        Return oControl
    End Function

    Public Function GetRequestedURL() As String
        Dim sURL As String = HttpContext.Current.Request.ServerVariables("QUERY_STRING")
        If Page.IsPostBack Then
            sURL = Server.UrlDecode(sURL)
        End If
        sURL = sURL.Replace("404;", "")

        Try
            Dim oURI As New Uri(sURL)
            sURL = oURI.PathAndQuery
            Return sURL
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function GetRequestedURL(ByVal oPg As Page) As String
        Dim sURL As String = HttpContext.Current.Request.ServerVariables("QUERY_STRING")
        If oPg.IsPostBack Then
            sURL = HttpContext.Current.Server.UrlDecode(sURL)
        End If
        sURL = sURL.Replace("404;", "")

        Try
            Dim oURI As New Uri(sURL)
            sURL = oURI.PathAndQuery
            Return sURL
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Sub RegisterStylesheet(ByVal sPath As String, ByVal oPage As Page)
        Dim oCss As New LiteralControl("<link rel=""stylesheet"" type=""text/css"" href=""" & sPath & """/>")
        If oPage.Header IsNot Nothing Then
            oPage.Header.Controls.Add(oCss)
        Else
            '-- Not a dynamic page header, register this anywhere and hope for the best
            oPage.Controls.Add(oCss)
        End If
    End Sub

    Public Shared Function UpdateStatusMsg(ByVal sMsg As String, ByVal bErr As Boolean) As String
        Dim sClass As String = "ui-state-highlight"
        Dim sIcon As String = "ui-icon-info"
        If bErr Then
            sClass = "ui-state-error"
            sIcon = "ui-icon-alert"
        End If
        Return "<div class=""ui-widget"" style=""margin:10px 0;font-size:0.9em;""><div class=""" & sClass & " ui-corner-all"" style=""padding: 0 .7em;""><p><span class=""ui-icon " & sIcon & """ style=""float: left; margin-right: .3em;""></span><div>" & sMsg & "</div></p></div></div>"
    End Function

    Public Shared Sub DebugPrint(ByVal sMsg As String)
        HttpContext.Current.Response.Write("<p style=""background-color:#FFF;color:#000;padding:5px;"">" & sMsg & "</p>")
    End Sub

    Public Shared Function UpdateStatusMsg(ByVal sMsg As String) As String
        Return UpdateStatusMsg(sMsg, False)
    End Function
End Class
