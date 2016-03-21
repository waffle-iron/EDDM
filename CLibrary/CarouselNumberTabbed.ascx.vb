Imports System.IO
Imports System.Collections.Generic
Imports System.Xml

Partial Class CLibrary_CarouselNumberTabbed
    Inherits CLibraryBase

    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property CssClass() As String
        Get
            Return pCarousel.CssClass
        End Get
        Set(ByVal value As String)
            pCarousel.CssClass = value
        End Set
    End Property

    Private _SidebarImage As String = ""
    <appx.cms(appx.cmsAttribute.DataValueType.CMSImage)> _
    Public Property SidebarImage() As String
        Get
            Return _SidebarImage
        End Get
        Set(ByVal value As String)
            _SidebarImage = value
        End Set
    End Property

    Private _SidebarTitle As String = ""
    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property SidebarTitle() As String
        Get
            Return _SidebarTitle
        End Get
        Set(ByVal value As String)
            _SidebarTitle = value
        End Set
    End Property

    Private _SidebarUrl As String = ""
    <appx.cms(appx.cmsAttribute.DataValueType.URLList)> _
    Public Property SidebarUrl() As String
        Get
            Return _SidebarUrl
        End Get
        Set(ByVal value As String)
            _SidebarUrl = value
        End Set
    End Property

    Private _AutoScrollInterval As Integer = 4
    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property AutoScrollInterval() As Integer
        Get
            Return _AutoScrollInterval
        End Get
        Set(ByVal value As Integer)
            _AutoScrollInterval = value
        End Set
    End Property

    Private _SourceDirectory As String = ""
    <appx.cms(appx.cmsAttribute.DataValueType.CMSDirectory)> _
    Public Property SourceDirectory() As String
        Get
            Return _SourceDirectory
        End Get
        Set(ByVal value As String)
            _SourceDirectory = value
        End Set
    End Property

    Protected ReadOnly Property CarouselId() As String
        Get
            Return Me.ClientID.Replace(Me.ClientIDSeparator, "")
        End Get
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        If Not String.IsNullOrEmpty(Me.SourceDirectory) Then
            Dim sStylesheet As String = ""
            Dim sDirName As String = Me.SourceDirectory
            Dim sDirPath As String = sDirName
            If sDirPath.StartsWith("/") Then
                sDirPath = Server.MapPath(sDirPath)
            End If
            Dim sCacheKey As String = "Carousel:" & sDirName.Replace("/", ":").Trim
            Dim dQuery As Hashtable = apphelp.RebuildQuerystring(apphelp.GetRequestedURL, False)
            Dim sNoCache As String = "0"
            If dQuery.ContainsKey("nocache") Then
                sNoCache = dQuery("nocache")
            End If
            If sNoCache = "1" Then
                Cache.Remove(sCacheKey)
            End If

            Dim oCarousel As appxCMSLib.Carousel = Nothing
            If Cache(sCacheKey) Is Nothing Then
                If Directory.Exists(sDirPath) Then
                    Dim sBase As String = Server.MapPath("/")
                    Dim oDir As New DirectoryInfo(sDirPath)
                    Dim sConfig As String = Path.Combine(oDir.FullName, "carousel.xml")
                    If File.Exists(sConfig) Then
                        oCarousel = New appxCMSLib.Carousel(sConfig)
                    Else
                        Dim aFiles() As FileInfo = oDir.GetFiles
                        If aFiles.Length > 0 Then
                            For iFile As Integer = 0 To aFiles.Length - 1
                                Dim oFile As FileInfo = aFiles(iFile)
                                Dim sExt As String = oFile.Extension.ToLower
                                If sExt = ".css" Then
                                    sStylesheet = oFile.FullName.Replace(sBase, "").Replace("\", "/")
                                Else
                                    If sExt = ".jpg" Or sExt = ".gif" Or sExt = ".png" Then
                                        If oCarousel Is Nothing Then
                                            oCarousel = New appxCMSLib.Carousel
                                        End If
                                        Dim sImgUrl As String = oFile.FullName.Replace(sBase, "").Replace("\", "/")
                                        If Not sImgUrl.StartsWith("/") Then
                                            sImgUrl = "/" & sImgUrl
                                        End If
                                        Dim oCItem As New appxCMSLib.CarouselItem(sImgUrl, sImgUrl, oFile.Name)
                                        oCarousel.Add(oCItem)
                                    End If
                                End If
                            Next
                        End If
                    End If
                    Using oDepends As New CacheDependency(sDirPath)
                        Cache.Insert(sCacheKey, oCarousel, oDepends)
                    End Using
                End If
            Else
                oCarousel = DirectCast(Cache(sCacheKey), appxCMSLib.Carousel)
            End If

            If oCarousel IsNot Nothing Then
                lvCarousel.DataSource = oCarousel
                lvCarousel.DataBind()

                jqueryHelper.Include(Page)
                jqueryHelper.IncludePlugin(Page, "jCarousel", "~/scripts/jquery.jcarousel.pack.js")
                jqueryHelper.IncludePlugin(Page, "jCarousel" & Me.ClientID, "~/scripts/CarouselNumberTabbed.ashx?carouselid=" & Server.UrlEncode(Me.CarouselId) & "&auto=" & Me.AutoScrollInterval)
                jqueryHelper.RegisterStylesheet(Page, "~/scripts/jquery.jcarousel.css")
                If String.IsNullOrEmpty(sStylesheet) Then
                    sStylesheet = "~/scripts/CarouselNumberTabbed.css.ashx?class=" & Server.UrlEncode(Me.CssClass) & "&w=" & oCarousel.Width & "&h=" & oCarousel.Height
                End If
                jqueryHelper.RegisterStylesheet(Page, sStylesheet)
            End If
        Else
            Me.Visible = False
        End If
    End Sub

    Protected Sub lvCarousel_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvCarousel.DataBound
        Dim lCarouselStart As Literal = DirectCast(lvCarousel.FindControl("lCarouselStart"), Literal)
        If lCarouselStart IsNot Nothing Then
            lCarouselStart.Text = "<ul id=""" & Me.CarouselId & """ class=""" & Me.CssClass & """>"
        End If

        Dim lCarouselControl As Literal = DirectCast(lvCarousel.FindControl("lCarouselControl"), Literal)
        If lCarouselControl IsNot Nothing Then
            lCarouselControl.Text = "<div id=""" & Me.CarouselId & "Control"" class=""carouselControlTabs""></div>"
        End If

        Dim pCarouselSideBar As Panel = DirectCast(lvCarousel.FindControl("pCarouselSideBar"), Panel)
        Dim imgSidebar As Image = DirectCast(lvCarousel.FindControl("imgSidebar"), Image)
        Dim hplSidebar As HyperLink = DirectCast(lvCarousel.FindControl("hplSidebar"), HyperLink)
        If Not String.IsNullOrEmpty(Me.SidebarImage) Then
            imgSidebar.ImageUrl = Me.SidebarImage
            imgSidebar.ToolTip = Me.SidebarTitle
            imgSidebar.AlternateText = Me.SidebarTitle
            If Not String.IsNullOrEmpty(Me.SidebarUrl) Then
                hplSidebar.NavigateUrl = Me.SidebarUrl
            End If
        Else
            pCarouselSideBar.Visible = False
        End If
    End Sub
End Class
