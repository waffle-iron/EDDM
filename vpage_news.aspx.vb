
Partial Class vpage_news
    Inherits appxCMS.PageBase

    Private ReadOnly Property StoryID() As Integer
        Get
            Return QStringToInt("ID")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim oPost As appxCMS.NewsBlogPost = appxCMS.NewsBlogDataSource.GetPost(Me.StoryID)
            If oPost IsNot Nothing Then
                lHeadline.Text = oPost.Headline
                lPublishDate.Text = oPost.PublishDate.ToString("dd MMMM yyyy")
                lStory.Text = oPost.Story
                lAuthor.Text = oPost.Creator
            End If
        End If
    End Sub
End Class

