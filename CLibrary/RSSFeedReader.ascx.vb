
Partial Class CLibrary_RSSFeedReader
    Inherits CLibraryBase

    Private _CssClass As String = ""
    Public Property CssClass() As String
        Get
            Return pRSSFeed.CssClass
        End Get
        Set(ByVal value As String)
            pRSSFeed.CssClass = value
        End Set
    End Property

    Public Property FeedTitle() As String
        Get
            Return lFeedTitle.Text
        End Get
        Set(ByVal value As String)
            lFeedTitle.Text = value
        End Set
    End Property

    Private _FeedUrl As String = ""
    Public Property FeedUrl() As String
        Get
            Return _FeedUrl
        End Get
        Set(ByVal value As String)
            _FeedUrl = value
        End Set
    End Property

    Private _NumberOfItemsToShow As Integer = 5
    Public Property NumberOfItemsToShow() As Integer
        Get
            Return _NumberOfItemsToShow
        End Get
        Set(ByVal value As Integer)
            _NumberOfItemsToShow = value
        End Set
    End Property

    Private _FeedTransformFile As String = "/transformers/rss.xsl"
    Public Property FeedTransformFile() As String
        Get
            Return _FeedTransformFile
        End Get
        Set(ByVal value As String)
            _FeedTransformFile = value
        End Set
    End Property

    Private _CacheExpiration As Integer = 15
    Public Property CacheExpiration() As Integer
        Get
            Return _CacheExpiration
        End Get
        Set(ByVal value As Integer)
            _CacheExpiration = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim sCacheKey As String = "RSSFeed:"
        If Not String.IsNullOrEmpty(Me.FeedTitle) Then
            sCacheKey = sCacheKey & Me.FeedTitle
        Else
            sCacheKey = sCacheKey & Me.UniqueID
        End If

        If Cache(sCacheKey) Is Nothing Then
            '-- Get the feed XML
            Dim sFeedXml As String = httpHelp.GetXMLURLPage(Me.FeedUrl, Nothing, Nothing)

            Dim sFeedData As String = xmlhelp.InMemoryTransform(sFeedXml, Server.MapPath(Me.FeedTransformFile))

            Using oCDepends As New CacheDependency(Server.MapPath(Me.FeedTransformFile))
                Cache.Insert(sCacheKey, sFeedData, oCDepends, System.DateTime.UtcNow.AddMinutes(Me.CacheExpiration), System.Web.Caching.Cache.NoSlidingExpiration)
            End Using
        End If

        lFeedData.Text = Cache(sCacheKey)
    End Sub

End Class
