Imports System.IO

Partial Class vpage_blogpost
    Inherits appxCMS.PageBase

    Protected ReadOnly Property NewsId() As Integer
        Get
            Return QStringToInt("Id")
        End Get
    End Property

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim oPost = appxCMS.NewsBlogDataSource.GetDisplayPost(Me.NewsId)
        If oPost IsNot Nothing Then
            Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId

            Dim sPageFile As String = Server.MapPath("~/cmsimages/_bloglayout/" & siteId & "/PostPageLayout.htm")

            Dim sHeadline As String = oPost.Post.Headline

            Dim sCK As String = "SiteId" & siteId & "PostPageLayout"
            Dim sPost As String = appxCMS.Util.Cache.GetString(sCK)
            If String.IsNullOrEmpty(sPost) AndAlso File.Exists(sPageFile) Then
                sPost = File.ReadAllText(sPageFile)
                appxCMS.Util.Cache.Add(sCK, sPost, sPageFile)
            End If

            '-- Clean-up spagecontent variables
            sPost = FixVar(sPost, "PostId", oPost.Post.NewsID.ToString())
            sPost = FixVar(sPost, "Headline", sHeadline)
            sPost = FixVar(sPost, "Category", oPost.Category.Name)
            sPost = FixVar(sPost, "CategoryLink", appxCMS.SEO.Rewrite.BuildLink(oPost.Category.Name, oPost.Category.CategoryId.ToString(), "blogcategory"))
            sPost = FixVar(sPost, "Summary", oPost.Post.Summary)
            sPost = FixVar(sPost, "Post", oPost.Post.Story)
            sPost = FixVar(sPost, "PostLink", appxCMS.SEO.Rewrite.BuildLink(oPost.Post.Headline, oPost.Post.NewsID.ToString, "blogpost"))
            sPost = FixVar(sPost, "PostImage", "")
            sPost = FixVar(sPost, "PostImageCaption", "")
            sPost = FixVar(sPost, "Author", oPost.Post.Creator)
            sPost = FixVar(sPost, "AuthorLink", "")
            sPost = FixVar(sPost, "PublishDate", oPost.Post.PublishDate.ToString("MMM %d, yyyy"))
            sPost = FixVar(sPost, "RssUrl", appxCMS.SEO.Rewrite.BuildLink(oPost.Category.Name, oPost.Category.CategoryId.ToString, "newsblogrss"))
            'sPost = FixVar(sPost, "PreviousPostUrl", appxCMS.SEO.Rewrite.BuildLink(oPost.PreviousPost.Headline, oPost.PreviousPost.NewsID.ToString(), "blogpost"))
            'sPost = FixVar(sPost, "NextPostUrl", appxCMS.SEO.Rewrite.BuildLink(oPost.NextPost.Headline, oPost.NextPost.NewsID.ToString, "blogpost"))

            If Not String.IsNullOrEmpty(sPost) Then
                phPostPage.Controls.Add(DirectCast(Page, appxCMS.PageBase).FormatContent(sPost))
            End If

            appxCMS.Util.jQuery.RegisterStylesheet(Page, "~/cmsimages/_bloglayout/" & siteId & "/blog.css")

            '-- Our pingback header
            Response.AddHeader("X-Pingback", appxCMS.Util.urlHelp.AppRelativeToFullyQualified("~/pingback.ashx"))

            Dim oRdf As New StringBuilder
            oRdf.AppendLine("<!--")

            Dim sPostUrl As String = appxCMS.SEO.Rewrite.BuildLink(sHeadline, Me.NewsId.ToString(), "blogpost")

            '-- {0} = Post Url (full, include protocol and host)
            '-- {1} = Post Title
            '-- {2} = Trackback url
            Dim s0 As String = appxCMS.Util.urlHelp.AppRelativeToFullyQualified(sPostUrl)
            Dim s1 As String = sHeadline
            Dim s2 As String = appxCMS.Util.urlHelp.AppRelativeToFullyQualified("~/trackback.ashx?id=" & Me.NewsId)

            oRdf.AppendLine("<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"">")
            oRdf.AppendLine(String.Format("<rdf:Description rdf:about=""{0}"" dc:identifier=""{0}"" dc:title=""{1}"" trackback:ping=""{2}"" />", s0, s1, s2))
            oRdf.AppendLine("</rdf:RDF>")
            oRdf.AppendLine("-->")
            lRDF.Text = oRdf.ToString

            'If oPost.Post.EnableComments Or oPost.Comments.Count > 0 Then
            '    lvComments.DataSource = oPost.Comments
            '    lvComments.DataBind()
            'Else
            '    pComments.Visible = False
            'End If

            Page.Title = oPost.Post.Headline
        End If
    End Sub

    Protected Overrides Sub Render(writer As HtmlTextWriter)
        For Each control As Control In Page.Header.Controls
            Dim link As HtmlLink

            link = TryCast(control, HtmlLink)
            If (link IsNot Nothing) AndAlso link.Href.StartsWith("~/") Then
                Dim sLink As String = VirtualPathUtility.ToAbsolute(link.Href)
                link.Href = sLink
                'If Request.ApplicationPath = "/" Then
                '    link.Href = link.Href.Substring(1)
                'Else
                '    link.Href = Request.ApplicationPath + "/" & link.Href.Substring("~/".Length)
                'End If
            End If
        Next
        MyBase.Render(writer)
    End Sub

    Protected Function FixVar(ByVal str As String, ByRef sKey As String, ByRef sVal As String) As String
        str = Regex.Replace(str, "#" & sKey & "#", sVal, RegexOptions.IgnoreCase And RegexOptions.Multiline)
        Return str
    End Function
End Class
