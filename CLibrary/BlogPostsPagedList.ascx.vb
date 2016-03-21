Imports System.Collections.Generic
Imports System.Linq
Imports System.IO

Partial Class CLibrary_BlogPostsPagedList
    Inherits CLibraryBase

    Protected ReadOnly Property PageNumber As Integer
        Get
            Dim iPg As Integer = 1
            Dim oQ = appxCMS.PageBase.RebuildQuerystring(appxCMS.PageBase.GetRequestedUrl(Page))
            If oQ.ContainsKey("pg") Then
                Integer.TryParse(oQ("pg").ToString(), iPg)
            End If
            'Dim iPg As Integer = appxCMS.Util.Querystring.GetInteger("pg")
            If iPg < 1 Then
                iPg = 1
            End If
            Return iPg
        End Get
    End Property

    Private _pageSize As Integer = 10
    Public Property NumberOfStoriesToShow As Integer
        Get
            Return _pageSize
        End Get
        Set(value As Integer)
            _pageSize = value
        End Set
    End Property

    Private _defaultPostImage As String = ""
    Private _defaultPostImageLoaded As Boolean = False
    Private ReadOnly Property DefaultPostImage As String
        Get
            If Not _defaultPostImageLoaded Then
                _defaultPostImage = appxCMS.Util.CMSSettings.GetSetting("Blog", "DefaultPostImage")
                _defaultPostImageLoaded = True
            End If
            Return _defaultPostImage
        End Get
    End Property

    Private _postPathPrefix As String = ""
    Private _postPathPrefixLoaded As Boolean = False
    Private ReadOnly Property PostPathPrefix As String
        Get
            If Not _postPathPrefixLoaded Then
                Dim sPrefix As String = appxCMS.Util.CMSSettings.GetSetting("Blog", "BlogPostPathPrefix")
                If Not String.IsNullOrEmpty(sPrefix) Then
                    If sPrefix.StartsWith("/") Then
                        sPrefix = sPrefix.Substring(1)
                    End If
                    If Not sPrefix.EndsWith("/") Then
                        sPrefix = sPrefix & "/"
                    End If
                End If
                _postPathPrefix = sPrefix
                _postPathPrefixLoaded = True
            End If
            Return _postPathPrefix
        End Get
    End Property
    
    Protected PostItemFormat As String = ""
    Private aExt() As String = {".jpg", ".png", ".gif"}
    Private oImagesDir As DirectoryInfo

    Protected Overrides Sub BuildControl()
        oImagesDir = New DirectoryInfo(Server.MapPath("~/cmsimages/PostImages"))

        Dim siteId As Integer = appxCMS.Util.CMSSettings.GetSiteId

        Dim sPageFile As String = Server.MapPath("~/cmsimages/_bloglayout/" & siteId & "/BlogItemLayout.htm")
        Dim sCk As String = "SiteId" & siteId & "BlogItemLayout"

        PostItemFormat = appxCMS.Util.Cache.GetString(sCk)
        If String.IsNullOrEmpty(PostItemFormat) Then
            PostItemFormat = File.ReadAllText(sPageFile)
            appxCMS.Util.Cache.Add(sCk, PostItemFormat, sPageFile)
        End If

        Dim iTotalPosts As Integer = 0

        Dim categoryId As Integer = appxCMS.Util.Querystring.GetInteger("categoryid")
        Dim authorId As Integer = appxCMS.Util.Querystring.GetInteger("authorid")
        Dim month As Integer = appxCMS.Util.Querystring.GetInteger("month")
        Dim year As Integer = appxCMS.Util.Querystring.GetInteger("year")
        Dim tag As String = appxCMS.Util.Querystring.GetString("tag")

        Dim oPosts As List(Of appxCMS.NewsBlogPost) = Nothing
        Using oDb As New appxCMS.appxCMSEntities
            Dim oData = (From p In oDb.NewsBlogPosts.Include("Category") _
                         Where p.Published = True _
                         AndAlso p.PublishDate <= DateTime.Now _
                         AndAlso p.SiteId = siteId)

            If categoryId > 0 Then
                oData = oData.Where(Function(p) p.Category.CategoryId = categoryId)
                '-- Load the title, meta and keywords for a category
                Dim oCat As appxCMS.NewsBlogCategory = appxCMS.NewsBlogCategoryDataSource.GetCategory(categoryId)
                If oCat IsNot Nothing Then
                    Page.Title = oCat.Name
                    If oCat.Description IsNot Nothing AndAlso Not String.IsNullOrEmpty(oCat.Description.Trim()) Then
                        Page.Header.Controls.Add(New LiteralControl("<meta name=""description"" content=""" & oCat.Description & """ />"))
                        Page.Header.Controls.Add(New LiteralControl("<meta property=""og:description"" content=""" & oCat.Description & """ />"))
                    End If
                End If

            ElseIf authorId > 0 Then
                oData = oData.Where(Function(p) p.CreatedBy = authorId)
            ElseIf month > 0 AndAlso year > 0 Then
                oData = oData.Where(Function(p) p.PublishDate.Month = month AndAlso p.PublishDate.Year = year)
                Dim dDate As New DateTime(year, month, 1)
                Page.Title = "Posts for " & dDate.ToString("MMMM") & " " & dDate.Year
                Page.Header.Controls.Add(New LiteralControl("<meta name=""description"" content=""Blog posts for " & dDate.ToString("MMMM yyyy") & """ />"))
                Page.Header.Controls.Add(New LiteralControl("<meta property=""og:description"" content=""Blog posts for " & dDate.ToString("MMMM yyyy") & """ />"))
            ElseIf Not String.IsNullOrEmpty(tag) Then
                oData = oData.Where(Function(p) p.Tags.Any(Function(t) t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase)))
                Page.Title = "Posts tagged '" & tag & "'"
                Page.Header.Controls.Add(New LiteralControl("<meta name=""description"" content=""Blog posts tagged '" & tag & "'"" />"))
                Page.Header.Controls.Add(New LiteralControl("<meta property=""og:description"" content=""Blog posts tagged '" & tag & "'"" />"))
            Else
                '-- Default list of all posts
            End If

            iTotalPosts = oData.Count()

            Dim iPg As Integer = PageNumber - 1
            If iPg < 0 Then
                iPg = 0
            End If
            Dim iSkip As Integer = iPg * NumberOfStoriesToShow

            oPosts = (From p In oData _
                      Order By p.PublishDate Descending).Skip(iSkip).Take(NumberOfStoriesToShow).ToList


        End Using

        lvPosts.DataSource = oPosts
        lvPosts.DataBind()

        If iTotalPosts <= NumberOfStoriesToShow Then
            blogPager.Visible = False
            blogPagerTop.Visible = False
        Else
            Dim oPg As appxCMS.PageBase = DirectCast(Page, appxCMS.PageBase)
            Dim sPg As String = oPg.GetRequestedUrl()
            Dim oQ As Hashtable = appxCMS.PageBase.RebuildQuerystring(sPg, True)
            If oQ.ContainsKey("pg") Then
                oQ.Remove("pg")
            End If
            If oQ.Count > 0 Then
                Dim oQs As New List(Of String)
                For Each sKey As String In oQ.Keys
                    oQs.Add(sKey & "=" & Server.UrlEncode(oQ(sKey).ToString()))
                Next
                sPg = sPg & "?" & String.Join("&", oQs.ToArray())
            End If
            Dim iMaxPages As Double = Math.Ceiling(iTotalPosts / NumberOfStoriesToShow)
            If PageNumber = 1 Then
                '-- Add a canonical for this, so it doesn't get confused with ?pg=1
                Page.Header.Controls.Add(New LiteralControl("<link rel=""canonical"" href=""https://" & Request.Url.Host & sPg & """/>"))
                Page.Header.Controls.Add(New LiteralControl("<link rel=""next"" href=""https://" & Request.Url.Host & sPg & "?pg=" & PageNumber + 1 & """/>"))
            ElseIf PageNumber = CType(iMaxPages, Integer) Then
                Page.Header.Controls.Add(New LiteralControl("<link rel=""prev"" href=""https://" & Request.Url.Host & sPg & "?pg=" & PageNumber - 1 & """/>"))
            Else
                Page.Header.Controls.Add(New LiteralControl("<link rel=""next"" href=""https://" & Request.Url.Host & sPg & "?pg=" & PageNumber + 1 & """/>"))
                Page.Header.Controls.Add(New LiteralControl("<link rel=""prev"" href=""https://" & Request.Url.Host & sPg & "?pg=" & PageNumber - 1 & """/>"))
            End If

            blogPager.ResultCount = iTotalPosts
            blogPager.CurPage = PageNumber
            blogPager.PageSize = NumberOfStoriesToShow
            blogPager.NavigatePage = sPg
            blogPager.QuerystringField = "pg"
            blogPager.HideMaxMove = True
            blogPager.ShowPageNumbers = True
            blogPager.HideUnusableItems = True
            'blogPager.MovePreviousText = "Newer Posts <i class=""fa fa-chevron-right fa-fw""></i>"
            'blogPager.MoveNextText = "<i class=""fa fa-chevron-right fa-fw""></i> More Posts"

            blogPagerTop.ResultCount = iTotalPosts
            blogPagerTop.CurPage = PageNumber
            blogPagerTop.PageSize = NumberOfStoriesToShow
            blogPagerTop.NavigatePage = sPg
            blogPagerTop.QuerystringField = "pg"
            blogPagerTop.HideMaxMove = True
            blogPagerTop.ShowPageNumbers = True
            blogPagerTop.HideUnusableItems = True
            'blogPagerTop.MovePreviousText = "Newer Posts"
            'blogPagerTop.MoveNextText = "Older Posts"
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Sub lvPosts_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvPosts.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = CType(e.Item, ListViewDataItem)

            Dim oPost As appxCMS.NewsBlogPost = DirectCast(oDItem.DataItem, appxCMS.NewsBlogPost)

            Dim phPost As PlaceHolder = DirectCast(e.Item.FindControl("phPost"), PlaceHolder)

            '-- Replace all of our template variables for this post and put it into the placeholder
            Dim sPost As String = PostItemFormat

            '-- Get the post image
            Dim sPostImageCk As String = "BlogPostImage" & oPost.NewsID
            Dim sPostImage As String = appxCMS.Util.Cache.GetString(sPostImageCk)
            If String.IsNullOrEmpty(sPostImage) Then
                Dim oPostImages() As FileInfo = oImagesDir.GetFiles(oPost.NewsID & ".*")
                If oPostImages.Length > 0 Then
                    For Each oPostImage As FileInfo In oPostImages
                        If Not oPostImage.Extension.Equals(".caption", System.StringComparison.OrdinalIgnoreCase) Then
                            sPostImage = "/cmsimages/PostImages/" & oPost.NewsID & oPostImage.Extension
                            Exit For
                        End If
                    Next
                End If
                If Not String.IsNullOrEmpty(sPostImage) Then
                    appxCMS.Util.Cache.Add(sPostImageCk, sPostImage)
                End If
            End If
            If String.IsNullOrEmpty(sPostImage) Then
                If Not String.IsNullOrEmpty(DefaultPostImage) Then
                    sPostImage = DefaultPostImage
                Else
                    sPostImage = "/cmsimages/spacer.gif"
                End If
            End If
            'PostId,Headline,Category,CategoryLink,Summary,Post,PostLink,PostImage,PostImageCaption,Author,AuthorLink,PublishDate
            sPost = FixVar(sPost, "PostId", oPost.NewsID.ToString())
            sPost = FixVar(sPost, "Headline", oPost.Headline)
            sPost = FixVar(sPost, "Category", oPost.Category.Name)
            sPost = FixVar(sPost, "CategoryLink", appxCMS.SEO.Rewrite.BuildLink(oPost.Category.Name, oPost.Category.CategoryId.ToString(), "blogcategory"))
            sPost = FixVar(sPost, "Summary", oPost.Summary)
            sPost = FixVar(sPost, "Post", oPost.Story)
            sPost = FixVar(sPost, "PostLink", appxCMS.SEO.Rewrite.BuildLink(PostPathPrefix & oPost.Headline, oPost.NewsID.ToString, "blogpost"))
            sPost = FixVar(sPost, "PostImage", sPostImage)
            sPost = FixVar(sPost, "PostImageCaption", "")
            sPost = FixVar(sPost, "Author", oPost.Creator)
            sPost = FixVar(sPost, "AuthorLink", "")
            sPost = FixVar(sPost, "PublishDate", oPost.PublishDate.ToString("MMM %d, yyyy"))

            phPost.Controls.Add(New LiteralControl(sPost))
        End If
    End Sub

    Protected Function FixVar(ByVal str As String, ByRef sKey As String, ByRef sVal As String) As String
        str = Regex.Replace(str, "#" & sKey & "#", sVal, RegexOptions.IgnoreCase And RegexOptions.Multiline)
        Return str
    End Function
End Class
