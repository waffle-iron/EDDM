
Partial Class CLibrary_LatestNews
    Inherits CLibraryBase

    <appx.cms(appx.cmsAttribute.DataValueType.Free)> _
    Public Property CssClass() As String
        Get
            Return pNews.CssClass
        End Get
        Set(ByVal value As String)
            pNews.CssClass = value
        End Set
    End Property

    Private _NumberOfStories As Integer = 5
    Public Property NumberOfStoriesToShow() As String
        Get
            Return _NumberOfStories.ToString
        End Get
        Set(ByVal value As String)
            Dim iVal As Integer = _NumberOfStories
            If Integer.TryParse(value, iVal) Then
                _NumberOfStories = iVal
            End If
        End Set
    End Property

    Private _NewsTypeId As Integer = 0
    <appx.cms(appx.cmsAttribute.DataValueType.NewsBlogCategory)> _
    Public Property NewsTypeId() As Integer
        Get
            Return _NewsTypeId
        End Get
        Set(value As Integer)
            _NewsTypeId = value
        End Set
    End Property

    Private _NoNewsMessage As String = "There are no news stories currently active."
    <appX.cms(appX.cmsAttribute.DataValueType.Free)> _
    Public Property NoNewsMessage() As String
        Get
            Return _NoNewsMessage
        End Get
        Set(ByVal value As String)
            _NoNewsMessage = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Using oNewsA As New appxCMSNewsTableAdapters.NewsTableAdapter
            Dim oNewsT As appxCMSNews.NewsDataTable = Nothing
            If Me.NewsTypeId > 0 Then
                oNewsT = oNewsA.GetLatestByNewsType(Me._NumberOfStories, Me.NewsTypeId)
            Else
                oNewsT = oNewsA.GetLatest(Me._NumberOfStories)
            End If
            lvNews.DataSource = oNewsT
            lvNews.DataBind()
            oNewsT.Dispose()
        End Using
    End Sub

    Protected Sub lvNews_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvNews.ItemCreated
        If e.Item.ItemType = ListViewItemType.EmptyItem Then
            Dim lEmptyMessage As Literal = DirectCast(e.Item.FindControl("lEmptyMessage"), Literal)
            lEmptyMessage.Text = Me.NoNewsMessage
        End If
    End Sub
End Class
