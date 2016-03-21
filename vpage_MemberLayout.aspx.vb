
Partial Class vpage_MemberLayout
    Inherits appxCMS.PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim bAuth As Boolean = False
        If User.IsInRole("Admin") Or User.IsInRole("UserFunction.Teacher") Or User.IsInRole("UserFunction.Director") Or User.IsInRole("UserFunction.BoardMember") Then
            bAuth = True
        End If
        If Not bAuth Then
            Response.Redirect("~/Logon.aspx?ReturnUrl=" & pageBase.GetRequestedURL(Page))
            'FormsAuthentication.RedirectToLoginPage()
        End If

        'Dim sRebase As String = Request.Url.ToString.Replace(Request.Url.Scheme & "://" & Request.Url.Host, "")
        'Page.Form.Action = sRebase
    End Sub

    Protected Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Context.RewritePath(apphelp.GetRequestedURL)
        'Page.Form.Action = "/vpage_survey.aspx?id=" & Me.SurveyID
    End Sub
End Class
