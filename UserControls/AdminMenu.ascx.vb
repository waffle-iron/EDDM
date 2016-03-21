Imports System
Imports System.Xml

Partial Class usercontrols_AdminMenu
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim bEnableCache As Boolean = False
        If ConfigurationManager.AppSettings("EnableCache") IsNot Nothing Then
            Dim sEnableCache As String = ConfigurationManager.AppSettings("EnableCache").ToString
            If sEnableCache = "1" Or sEnableCache.ToUpperInvariant = "true" Then
                bEnableCache = True
            End If
        End If

        Dim sMenu As String = BuildMenu(bEnableCache)

        phMenu.Controls.Add(New LiteralControl(sMenu))
    End Sub

    Protected Function BuildMenu(ByVal bEnableCache As Boolean) As String
        Dim sUser As String = HttpContext.Current.User.Identity.Name
        Dim sCacheKey As String = "appxAuth:" & sUser & ":Menu"

        If bEnableCache Then
            '-- Check for the existence in the cache
            If Cache(sCacheKey) IsNot Nothing Then
                Return Cache(sCacheKey)
            End If
        End If

        Dim sSiteMapPath As String = Server.MapPath(VirtualPathUtility.ToAbsolute("~/admin/web.sitemap"))
        Dim oSM As New XmlDocument
        oSM.Load(sSiteMapPath)
        Dim oNSMgr As New XmlNamespaceManager(oSM.NameTable)
        oNSMgr.AddNamespace("sm", "http://schemas.microsoft.com/AspNet/SiteMap-File-1.0")

        Dim oSMRoot As XmlNode = oSM.SelectSingleNode("//sm:siteMap/sm:siteMapNode", oNSMgr)
        If oSMRoot Is Nothing Then
            Response.Write("We don't have it")
            Response.End()
        End If

        Dim oMenu As New XmlDocument
        oMenu.LoadXml("<ul />")
        Dim oMenuRoot As XmlNode = oMenu.SelectSingleNode("ul")
        xmlhelp.AddOrUpdateXMLAttribute(oMenuRoot, "class", "sf-menu")

        '-- We add the root node at the same level as the top-level children to create a home link
        Dim oHome As XmlNode = xmlhelp.AddOrUpdateXMLNode(oMenuRoot, "li", "")
        Dim sDesc As String = xmlhelp.ReadAttribute(oSMRoot, "description")
        Dim sTitle As String = xmlhelp.ReadAttribute(oSMRoot, "title")
        Dim oHomeLink As XmlNode = xmlhelp.AddOrUpdateXMLNode(oHome, "a", "")
        xmlhelp.AddOrUpdateXMLAttribute(oHomeLink, "href", Pathalyzer(xmlhelp.ReadAttribute(oSMRoot, "url")))
        xmlhelp.AddOrUpdateXMLAttribute(oHomeLink, "title", sDesc)
        Dim sIcon As String = xmlhelp.ReadAttribute(oSMRoot, "icon")
        If String.IsNullOrEmpty(sIcon) Then
            sIcon = "~/admin/images/spacer.gif"
        End If
        If sIcon.StartsWith("~/") Then
            sIcon = VirtualPathUtility.ToAbsolute(sIcon)
        End If
        If Not String.IsNullOrEmpty(sIcon) Then
            Dim oIcon As XmlNode = xmlhelp.AddOrUpdateXMLNode(oHomeLink, "img", "")
            xmlhelp.AddOrUpdateXMLAttribute(oIcon, "src", sIcon)
            xmlhelp.AddOrUpdateXMLAttribute(oIcon, "width", 16)
            xmlhelp.AddOrUpdateXMLAttribute(oIcon, "height", 16)
            xmlhelp.AddOrUpdateXMLAttribute(oIcon, "title", sDesc)
        End If
        Dim oText As XmlNode = xmlhelp.AddOrUpdateXMLNode(oHomeLink, "span", sTitle)

        For Each oTL As XmlNode In oSMRoot.SelectNodes("sm:siteMapNode[not(@requireArgs=1)]", oNSMgr)
            ProcessMenuTree(oMenuRoot, oTL, oNSMgr, 1)
        Next

        Dim sMenu As String = oMenuRoot.OuterXml

        If bEnableCache Then
            Using oCDepends As New CacheDependency(sSiteMapPath)
                Cache.Insert(sCacheKey, sMenu, oCDepends, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 20, 0))
            End Using
        End If

        Return sMenu
    End Function

    Protected Sub ProcessMenuTree(ByRef oParentMenu As XmlNode, ByVal oSMSource As XmlNode, ByVal oNSMgr As XmlNamespaceManager, ByVal iLevel As Integer)
        Dim bAuth As Boolean = False
        Dim bProcRoles As Boolean = True
        Dim sSiteId As String = xmlhelp.ReadAttribute(oSMSource, "siteid").Trim
        If Not String.IsNullOrEmpty(sSiteId) Then
            If Not HttpContext.Current.User.IsInRole("Site" & sSiteId) Then
                bProcRoles = False
            End If
        End If
        If bProcRoles Then
            Dim sRoles As String = xmlhelp.ReadAttribute(oSMSource, "roles").Trim
            If Not String.IsNullOrEmpty(sRoles) Then
                Dim aRoles() As String = sRoles.Split(",")
                For iRole As Integer = 0 To aRoles.Length - 1
                    Dim sRole As String = aRoles(iRole).Trim
                    If HttpContext.Current.User.IsInRole(sRole) Then
                        bAuth = True
                        Exit For
                    End If
                Next
            Else
                '-- Default authorize, there is no role requirement
                bAuth = True
            End If
        End If

        If bAuth And iLevel < 3 Then
            Dim oMenuItem As XmlNode = xmlhelp.AddOrUpdateXMLNode(oParentMenu, "li", "")
            Dim sDesc As String = xmlhelp.ReadAttribute(oSMSource, "description")
            Dim sTitle As String = xmlhelp.ReadAttribute(oSMSource, "title")
            Dim oMenuLink As XmlNode = xmlhelp.AddOrUpdateXMLNode(oMenuItem, "a", "")
            xmlhelp.AddOrUpdateXMLAttribute(oMenuLink, "href", Pathalyzer(xmlhelp.ReadAttribute(oSMSource, "url")))
            xmlhelp.AddOrUpdateXMLAttribute(oMenuLink, "title", sDesc)

            Dim sIcon As String = xmlhelp.ReadAttribute(oSMSource, "icon")
            If String.IsNullOrEmpty(sIcon) Then
                sIcon = "~/admin/images/spacer.gif"
            End If
            If sIcon.StartsWith("~/") Then
                sIcon = VirtualPathUtility.ToAbsolute(sIcon)
            End If
            Dim oIcon As XmlNode = xmlhelp.AddOrUpdateXMLNode(oMenuLink, "img", "")
            xmlhelp.AddOrUpdateXMLAttribute(oIcon, "src", sIcon)
            xmlhelp.AddOrUpdateXMLAttribute(oIcon, "width", 16)
            xmlhelp.AddOrUpdateXMLAttribute(oIcon, "height", 16)
            xmlhelp.AddOrUpdateXMLAttribute(oIcon, "title", sDesc)
            Dim oText As XmlNode = xmlhelp.AddOrUpdateXMLNode(oMenuLink, "span", sTitle)

            If iLevel + 1 < 3 And oSMSource.SelectNodes("sm:siteMapNode[not(@requireArgs=1)]", oNSMgr).Count > 0 Then
                Dim oUL As XmlNode = xmlhelp.AddOrUpdateXMLNode(oMenuItem, "ul", "")
                For Each oSMChild As XmlNode In oSMSource.SelectNodes("sm:siteMapNode[not(@requireArgs=1)]", oNSMgr)
                    ProcessMenuTree(oUL, oSMChild, oNSMgr, iLevel + 1)
                Next
            End If
        End If
    End Sub

    Protected Function Pathalyzer(ByVal sPath As String) As String
        If sPath.StartsWith("~/") Then
            sPath = VirtualPathUtility.ToAbsolute(sPath)
        End If
        Return sPath
    End Function

    'Protected aMenu As EasyMenu
    'Protected aConfig As EasyMenu
    'Protected aUsers As EasyMenu
    'Protected aTools As EasyMenu

    'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    ' create the Parent EasyMenu
    '    aMenu = New EasyMenu()
    '    ' set the ID (must be unique)
    '    aMenu.ID = "aMenu"

    '    ' Create the submenus
    '    aConfig = New EasyMenu()
    '    aConfig.ID = "aMenuConfig"

    '    aTools = New EasyMenu()
    '    aTools.ID = "aMenuTools"

    '    aUsers = New EasyMenu
    '    aUsers.ID = "aMenuUsers"

    '    ' add the submenus to the page
    '    Me.Controls.Add(aConfig)
    '    Me.Controls.Add(aTools)

    '    ' add the menu to the placeholder on the page
    '    phMenu.Controls.Add(aMenu)
    'End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Not Page.IsPostBack Then
    '        Dim oUser As System.Security.Principal.IPrincipal = HttpContext.Current.User
    '        '-- Build menu
    '        aMenu.ShowEvent = MenuShowEvent.Always
    '        aMenu.UseIcons = True
    '        aMenu.IconsPosition = MenuIconsPosition.Left
    '        aMenu.IconsFolder = "~/images/"
    '        aMenu.StyleFolder = "~/ob/easymenustyles/ezp"
    '        aMenu.Position = MenuPosition.Horizontal
    '        aMenu.CSSMenu = "ParentMenu"
    '        aMenu.CSSMenuItemContainer = "ParentItemContainer"
    '        aMenu.Width = "100%"
    '        aMenu.AddItem(New MenuItem("Home", "Home", "house.png", "~/"))

    '        If oUser.IsInRole("Manage..") Or oUser.IsInRole("UserFunction.Admin") Then
    '            aMenu.AddItem(New MenuItem("Config", "Manage", "wrench.png", "~/configure.aspx"))
    '        End If

    '        If oUser.IsInRole("Security..") Or oUser.IsInRole("UserFunction.Admin") Then
    '            aMenu.AddItem(New MenuItem("Security", "Security", "administrator2_16x16.gif", "~/security.aspx"))
    '        End If

    '        If oUser.IsInRole("Tools..") Or oUser.IsInRole("UserFunction.Admin") Then
    '            aMenu.AddItem(New MenuItem("Tools", "Tools", "settings2_16x16.gif", "~/tools.aspx"))
    '        End If

    '        '-- Config Menu
    '        If oUser.IsInRole("Manage..") Then
    '            aConfig.AttachTo = "Config"
    '            aConfig.Align = MenuAlign.Under
    '            aConfig.ShowEvent = MenuShowEvent.MouseOver
    '            aConfig.StyleFolder = "~/ob/easymenustyles/ezp"
    '            aConfig.Width = 140
    '            aConfig.IconsFolder = "~/images/"
    '            aConfig.IconsPosition = MenuIconsPosition.Left
    '            aConfig.UseIcons = True

    '            If oUser.IsInRole("Manage.Content") Then
    '                aConfig.AddItem(New MenuItem("Content", "Content", "page_world.png", "~/cms.aspx"))
    '            End If
    '            If oUser.IsInRole("Manage.EmailTemplate.Edit") Then
    '                aConfig.AddItem(New MenuItem("MessageTemplate", "E-mail Templates", "email.png", "~/emailtemplate.aspx"))
    '            End If
    '            If oUser.IsInRole("Manage.Surveys") Then
    '                aConfig.AddItem(New MenuItem("Survey", "Surveys", "comments.png", "~/survey.aspx"))
    '            End If
    '        End If

    '        '-- Tools Menu
    '        If oUser.IsInRole("Tools..") Then
    '            aTools.AttachTo = "Tools"
    '            aTools.Align = MenuAlign.Under
    '            aTools.ShowEvent = MenuShowEvent.MouseOver
    '            aTools.StyleFolder = "~/ob/easymenustyles/ezp"
    '            aTools.Width = 160
    '            aTools.IconsFolder = "~/images/"
    '            aTools.IconsPosition = MenuIconsPosition.Left
    '            aTools.UseIcons = True

    '            aTools.AddItem(New MenuItem("About", "About", "information.png", "~/about.aspx"))
    '            If oUser.IsInRole("UserFunction.Admin") Then
    '                aTools.AddItem(New MenuItem("ErrorLog", "Error Log", "exclamation.png", "~/errorlog.aspx"))
    '            End If
    '            If oUser.IsInRole("UserFunction.Admin") Then
    '                aTools.AddItem(New MenuItem("ViewStateDecoder", "Viewstate Decoder", "script_code.png", "~/viewstatedecoder.aspx"))
    '            End If
    '        End If
    '    End If
    'End Sub
End Class
