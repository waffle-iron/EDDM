
Partial Class CCustom_BannerFull
    Inherits CLibraryBase

    Private _introText As String = ""
    Public Property IntroText As String
        Get
            Return _IntroText
        End Get
        Set(value As String)
            _IntroText = value
        End Set
    End Property

    Private _fullText As String = ""
    Public Property FullText As String
        Get
            Return _FullText
        End Get
        Set(value As String)
            _FullText = value
        End Set
    End Property

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        lIntro.Text = Me.IntroText
        lFull.Text = Me.FullText
        'Dim sTag1 As String = "h1"
        'Dim sTag2 As String = "h2"
        'lIntro.Text = String.Format("<{0}>{1}</{0}>", sTag1, Me.IntroText)
        'lFull.Text = String.Format("<{0}>{1}</{0}>", sTag2, Me.FullText)
    End Sub

End Class
