Imports System.Xml
Imports System.Xml.Xsl

Partial Class CLibrary_RSSCombinedFeedReader
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

    Private _FeedUrls As String = ""
    <appx.cms(appx.cmsAttribute.DataValueType.URLList)> _
    Public Property FeedUrls() As String
        Get
            Return _FeedUrls
        End Get
        Set(ByVal value As String)
            _FeedUrls = value
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

    Private _FeedTransformFile As String = "/transformers/rssCombined.xsl"
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
            Dim aSplit() As Char = {ControlChars.CrLf, ControlChars.Cr, ControlChars.Lf}
            Dim aFeedUrl() As String = Me.FeedUrls.Split(aSplit)

            Dim aFeedItems As New SortedList
            Dim oFeedXML As New XmlDocument
            For iFeed As Integer = 0 To aFeedUrl.Length - 1
                Dim sFeedUrl As String = aFeedUrl(iFeed)
                If Not String.IsNullOrEmpty(sFeedUrl) Then
                    Dim sFeedXml As String = httpHelp.GetXMLURLPage(sFeedUrl, Nothing, Nothing)
                    If Not String.IsNullOrEmpty(sFeedXml) Then
                        Dim oRssXml As New XmlDocument
                        oRssXml.LoadXml(sFeedXml)
                        Dim oChannelTitle As XmlNode = oRssXml.SelectSingleNode("rss/channel/title")
                        Dim sChannelTitle As String = ""
                        If oChannelTitle IsNot Nothing Then
                            sChannelTitle = oChannelTitle.InnerText
                        End If
                        Dim oItems As XmlNodeList = oRssXml.SelectNodes("rss/channel/item")
                        For Each oItem As XmlNode In oItems
                            oItem = NormalizePublishDate(oRssXml, oItem)
                            xmlhelp.AddOrUpdateXMLNode(oItem, "channeltitle", sChannelTitle)
                            Dim sItemDate As DateTime = DateTime.Parse(oItem.SelectSingleNode("pubDate").InnerText)
                            Dim sKey As String = String.Format("{0}_{1}", sItemDate.ToString("u"), oItem.ChildNodes(0).InnerText)
                            aFeedItems.Add(sKey, oItem)
                        Next
                    End If
                End If
            Next

            Dim oFeedSb As New StringBuilder
            oFeedSb.AppendLine("<rss><channel>")
            Dim iAdded As Integer = 0
            For iFeedItem As Integer = aFeedItems.Count - 1 To 0 Step -1
                If iAdded > Me.NumberOfItemsToShow Then
                    Exit For
                End If
                Dim oItem As XmlNode = aFeedItems.GetByIndex(iFeedItem)
                oFeedSb.AppendLine(oItem.OuterXml)
                iAdded = iAdded + 1
            Next
            oFeedSb.AppendLine("</channel></rss>")

            Dim sFeed As String = xmlhelp.InMemoryTransform(oFeedSb.ToString, Me.FeedTransformFile)

            Using oCDepends As New CacheDependency(Server.MapPath(Me.FeedTransformFile))
                Cache.Insert(sCacheKey, sFeed, oCDepends, System.DateTime.UtcNow.AddMinutes(Me.CacheExpiration), System.Web.Caching.Cache.NoSlidingExpiration)
            End Using
        End If

        lFeedData.Text = Cache(sCacheKey)
    End Sub

    Private Function NormalizePublishDate(ByVal doc As XmlDocument, ByRef itemNode As XmlElement) As XmlElement
        Dim workDate As DateTime
        Dim work As String
        Dim workNode As XmlElement
        'date may be in one of three main formats (that I've seen so far)
        '  this routine attempts to convert them all into a single
        '   pubDate based on RFC1123
        '   see the article for more complaints, 
        '   see RssBandit for a more full featured implementation

        If itemNode.GetElementsByTagName("pubDate").Count > 0 Then
            workNode = itemNode.GetElementsByTagName("pubDate").Item(0)
            work = workNode.InnerText
        Else
            'we may have Dublin Core date
            workNode = itemNode.GetElementsByTagName("date", "http://purl.org/dc/elements/1.1/").Item(0)
            work = workNode.InnerText()
            'we should also create a pubDate for this element for use later
            Dim newNode As XmlElement = doc.CreateElement("pubDate")
            workNode = itemNode.AppendChild(newNode)
        End If
        'use the RssComponent.DateTimeExt from RssBandit to parse
        workDate = RssComponents.DateTimeExt.Parse(work)
        workNode.InnerText = workDate.ToString("R")

        Return itemNode
    End Function

End Class
