<%@ WebHandler Language="VB" Class="NewsBlogRSS" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.Linq
Imports System.Collections.Generic

Public Class NewsBlogRSS : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim iNewsTypeID As Integer = pageBase.QStringToInt("Id")
        
        Dim oXml As New XmlDocument
        oXml.LoadXml("<rss/>")
        Dim oRss As XmlNode = oXml.SelectSingleNode("/rss")
        xmlhelp.AddOrUpdateXMLAttribute(oRss, "version", "2.0")
        
        If iNewsTypeID > 0 Then
            Dim oBlog As appxCMS.NewsBlogCategory = appxCMS.NewsBlogCategoryDataSource.GetCategory(iNewsTypeID)
            If oBlog IsNot Nothing Then
                Dim oPosts As List(Of appxCMS.NewsBlogPost) = appxCMS.NewsBlogDataSource.GetLatestPosts(100, iNewsTypeID)
                
                Dim oLatest As appxCMS.NewsBlogPost = oPosts.FirstOrDefault
                Dim oChannel As XmlNode = xmlhelp.AddOrUpdateXMLNode(oRss, "channel", "")
                Dim oTitle As XmlNode = xmlhelp.AddOrUpdateXMLNode(oChannel, "title", oBlog.Name)
                Dim oLink As XmlNode = xmlhelp.AddOrUpdateXMLNode(oChannel, "link", appxCMS.SEO.Rewrite.GetLink(oBlog.Name, iNewsTypeID, appxCMS.SEO.Rewrite.LinkType.NewsBlog))
                If Not String.IsNullOrEmpty(oBlog.Description) Then
                    xmlhelp.AddOrUpdateXMLNode(oChannel, "description", oBlog.Description)
                End If
                If Not String.IsNullOrEmpty(oBlog.Language) Then
                    xmlhelp.AddOrUpdateXMLNode(oChannel, "language", oBlog.Language)
                End If
                Dim dPubDate As DateTime = System.DateTime.Now.ToUniversalTime
                If oLatest IsNot Nothing Then
                    dPubDate = oLatest.PublishDate.ToUniversalTime
                End If
                xmlhelp.AddOrUpdateXMLNode(oChannel, "pubDate", RssDateFormat(dPubDate))
                xmlhelp.AddOrUpdateXMLNode(oChannel, "lastBuildDate", RssDateFormat(System.DateTime.Now.ToUniversalTime))
                xmlhelp.AddOrUpdateXMLNode(oChannel, "generator", "appxCMS Rss 2.0 Generator")
                If Not String.IsNullOrEmpty(oBlog.ManagingEditor) Then
                    xmlhelp.AddOrUpdateXMLNode(oChannel, "managingEditor", oBlog.ManagingEditor)
                End If
                If Not String.IsNullOrEmpty(oBlog.Webmaster) Then
                    xmlhelp.AddOrUpdateXMLNode(oChannel, "webMaster", oBlog.Webmaster)
                End If
                    
                For Each oPost As appxCMS.NewsBlogPost In oPosts
                    Dim oItem As XmlNode = xmlhelp.AddOrUpdateXMLNode(oChannel, "item", "")
                    Dim oItemTitle As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "title", oPost.Headline)
                    Dim oItemLink As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "link", appxCMS.SEO.Rewrite.GetLink(oPost.Headline, oPost.NewsID, appxCMS.SEO.Rewrite.LinkType.NewsPost))
                    Dim sSummary As String = oPost.Summary
                    If String.IsNullOrEmpty(sSummary) Then
                        sSummary = textHelp.GetFirstParagraph(oPost.Story)
                    End If
                    sSummary = textHelp.stripHTML(sSummary)
                    Dim oItemDesc As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "description", sSummary)
                    Dim oPubDate As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "pubDate", RssDateFormat(oPost.PublishDate.ToUniversalTime))
                    Dim oGuid As XmlNode = xmlhelp.AddOrUpdateXMLNode(oItem, "guid", "http://" & context.Request.Url.Host & "/vpage_blogpost.aspx?ID=" & oPost.NewsID)
                    xmlhelp.AddOrUpdateXMLAttribute(oGuid, "isPermaLink", "true")
                Next
            End If
        End If
        
        context.Response.Clear()
        context.Response.ContentType = "text/xml"
        context.Response.Write(oXml.OuterXml)
        context.Response.End()
    End Sub
    
    Private Function RssDateFormat(ByVal dDate As DateTime) As String
        'Sat, 13 Dec 2003 18:30:02 GMT
        Return dDate.ToString("ddd, dd MMM yyyy hh:mm:ss GMT")
    End Function
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class