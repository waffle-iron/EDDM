Imports System.Linq

Partial Class UserControls_BlogComment
    Inherits System.Web.UI.UserControl

    Private _NewsId As Integer = 0
    Protected ReadOnly Property NewsId() As Integer
        Get
            If _NewsId = 0 Then
                _NewsId = pageBase.QStringToInt("Id")
            End If
            Return _NewsId
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            hplSignin.NavigateUrl = "~/Logon.aspx?ReturnUrl=" & Server.UrlEncode(apphelp.GetRequestedURL())
        End If
        If Me.NewsId > 0 Then
            Dim oDisplayMode As appxCMS.Lookup.EnableComment = appxCMS.NewsBlogDataSource.EnablePostComments(NewsId)
            If oDisplayMode = appxCMS.Lookup.EnableComment.GloballyDisabled Then
                Me.Visible = False
            Else
                If oDisplayMode = appxCMS.Lookup.EnableComment.DisableComments Then
                    pNoComments.Visible = True
                Else
                    If oDisplayMode = appxCMS.Lookup.EnableComment.AuthenticatedUsersOnly And Not Request.IsAuthenticated Then
                        pNoAnonymous.Visible = True
                    Else
                        If Request.IsAuthenticated Then
                            pAnonymousInformation.Visible = False
                        End If
                        pAddComment.Visible = True
                    End If
                End If
            End If
        Else
            Me.Visible = False
        End If
    End Sub

    Protected Sub btnSaveComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveComment.Click
        If Me.NewsId > 0 Then
            Dim sFirstName As String = FirstName.Text
            Dim sLastName As String = LastName.Text
            Dim sEmail As String = Email.Text
            Dim sUrl As String = Url.Text
            Dim sComment As String = Comment.Text

            Dim iUser As Integer = pageBase.LoggedOnUserID
            If iUser > 0 Then
                Using oAdminA As New appxAuthTableAdapters.AdminTableAdapter
                    Using oAdminT As appxAuth.AdminDataTable = oAdminA.GetUser(iUser)
                        If oAdminT.Rows.Count > 0 Then
                            Dim oAdmin As appxAuth.AdminRow = oAdminT.Rows(0)
                            sFirstName = oAdmin.FirstName
                            sLastName = oAdmin.LastName
                            sEmail = oAdmin.UserID
                        End If
                    End Using
                End Using
            End If

            Dim oPost As appxCMS.NewsBlogPost = appxCMS.NewsBlogDataSource.AddComment(Me.NewsId, iUser, sFirstName, sLastName, sEmail, sUrl, Request.UserHostAddress, sComment, False)
            If oPost IsNot Nothing Then
                Response.Redirect(appxCMS.SEO.Rewrite.GetLink(oPost.Headline, Me.NewsId, appxCMS.SEO.Rewrite.LinkType.NewsPost))
            End If
            'Using oCommentA As New appxCMSNewsTableAdapters.BlogCommentTableAdapter
            '    oCommentA.Insert(Me.NewsId, iUser, sFirstName, sLastName, sEmail, sUrl, Request.UserHostAddress, System.DateTime.Now, sComment, False)
            'End Using

            'Dim sHeadline As String = ""
            'Using oDb As New Data.appxCMSLINQDataContext()
            '    Dim oPost As Data.BlogPost = oDb.BlogPosts.Single(Function(oP As Data.BlogPost) oP.NewsID = Me.NewsId And oP.Published)
            '    If oPost IsNot Nothing Then
            '        sHeadline = oPost.Headline
            '    End If
            'End Using

            'Response.Redirect(linkHelp.SEOLink(sHeadline, Me.NewsId, linkHelp.LinkType.NewsPost))
        End If
    End Sub
End Class
