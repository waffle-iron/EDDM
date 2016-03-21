Imports System.Linq
Imports System.Collections.Generic

Partial Class vpage_newsblog
    Inherits appxCMS.PageBase

    'Private _NewsId As Integer = 0
    'Protected ReadOnly Property NewsId() As Integer
    '    Get
    '        If _NewsId = 0 Then
    '            _NewsId = QStringToInt("Id")
    '            If _NewsId = 0 And Me.NewsTypeId > 0 Then
    '                '-- Get the latest post
    '                Using oNewsA As New appxCMSNewsTableAdapters.NewsTableAdapter
    '                    Dim sID As String = oNewsA.GetLatestIdByNewsType(Me.NewsTypeId).ToString
    '                    Integer.TryParse(sID, _NewsId)
    '                End Using
    '            End If
    '        End If
    '        Return _NewsId
    '    End Get
    'End Property

    'Protected ReadOnly Property NewsTypeId() As Integer
    '    Get
    '        Return QStringToInt("TypeId")
    '    End Get
    'End Property

    'Private _PageId As Integer = 0
    'Protected ReadOnly Property PageId() As Integer
    '    Get
    '        Dim iPg As Integer = QStringToInt("PageId")
    '        If iPg > 0 Then
    '            _PageId = iPg
    '        End If
    '        Return _PageId
    '    End Get
    'End Property

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Me.NewsTypeId = 0 Then
    '        pChooseBlog.Visible = True
    '        pBlog.Visible = False

    '        Dim oBlogs As List(Of appxCMS.NewsBlogCategory) = appxCMS.NewsBlogCategoryDataSource.GetList()
    '        lvBlogList.DataSource = oBlogs
    '        lvBlogList.DataBind()
    '    Else
    '        pChooseBlog.Visible = False
    '        pBlog.Visible = True

    '        Dim oBlog As appxCMS.NewsBlogCategory = appxCMS.NewsBlogCategoryDataSource.GetCategory(Me.NewsTypeId)
    '        Dim oPosts As List(Of appxCMS.NewsBlogPost) = appxCMS.NewsBlogDataSource.GetLatestPosts(100, Me.NewsTypeId)
    '        Dim oLatest As appxCMS.NewsBlogPost = oPosts.FirstOrDefault
    '        If oLatest IsNot Nothing Then
    '            lHeadline.Text = oLatest.Headline
    '            lAuthor.Text = oLatest.Creator
    '            lPubDate.Text = oLatest.PublishDate.ToString("dd MMMM yyyy")
    '            lText.Text = oLatest.Story

    '            If oLatest.EnableComments Then
    '                Dim oComments As List(Of appxCMS.NewsBlogComment) = appxCMS.NewsBlogDataSource.GetComments(oLatest.NewsID)
    '                lvComments.DataSource = oComments
    '                lvComments.DataBind()
    '            Else
    '                pComments.Visible = False
    '            End If
    '        End If

    '        Dim sNewsType As String = ""
    '        If oBlog IsNot Nothing Then
    '            sNewsType = oBlog.Name
    '        End If

    '        lBlogName.Text = sNewsType
    '        hplRSS20.NavigateUrl = appxCMS.SEO.Rewrite.GetLink(sNewsType, Me.NewsTypeId, appxCMS.SEO.Rewrite.LinkType.NewsBlogRSS20)

    '        lvBlog.DataSource = oPosts
    '        lvBlog.DataBind()
    '    End If
    'End Sub

    ''Protected Sub lvBlog_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvBlog.ItemDataBound
    ''    If e.Item.ItemType = ListViewItemType.DataItem Then
    ''        Dim oDItem As ListViewDataItem = e.Item

    ''    End If
    ''End Sub

    'Protected Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    '    jqueryHelper.Include(Page)
    '    jqueryHelper.RegisterStylesheet(Page, "~/app_styles/forms.css")
    '    Dim oJs As New StringBuilder
    '    oJs.AppendLine("jQuery(document).ready(function() {")
    '    oJs.AppendLine("    jQuery('#jQTabs').tabs();")
    '    oJs.AppendLine("});")
    '    Page.ClientScript.RegisterClientScriptBlock(GetType(String), "newsblogInit", oJs.ToString, True)
    'End Sub
End Class
