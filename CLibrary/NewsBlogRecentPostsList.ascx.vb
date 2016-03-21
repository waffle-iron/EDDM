
Partial Class CLibrary_NewsBlogRecentPostsList
    Inherits CLibraryBase

    Private _sectionHeader As String = "Recent Posts"
    Public Property SectionHeader As String
        Get
            Return _sectionHeader
        End Get
        Set(value As String)
            _sectionHeader = value
        End Set
    End Property

    Private _numberToShow As Integer = 5
    Public Property NumberToShow As Integer
        Get
            Return _numberToShow
        End Get
        Set(value As Integer)
            _numberToShow = value
        End Set
    End Property

    Private _postPathPrefix As String = ""
    Private _postPathPrefixLoaded As Boolean = False
    Protected ReadOnly Property PostPathPrefix As String
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

    Protected Overrides Sub BuildControl()
        Dim aHeader As String() = SectionHeader.Trim.Split(New Char() {" "})
        If aHeader.Length > 1 Then
            Dim sFirst As String = aHeader(0)
            aHeader(0) = ""
            Dim sRest As String = String.Join(" ", aHeader).Trim
            lHeaderText.Text = "<b>" & sFirst & "</b> " & sRest
        End If

        Dim oNews = appxCMS.NewsBlogDataSource.GetLatestPosts(NumberToShow)
        lvPosts.DataSource = oNews
        lvPosts.DataBind()
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
