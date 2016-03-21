
Partial Class CLibrary_BlogFacebookComments
    Inherits CLibraryBase

    Private _fbAppId As String = ""
    Public Property FacebookAppId As String
        Get
            Return _fbAppId
        End Get
        Set(value As String)
            _fbAppId = value
        End Set
    End Property

    Private _built As Boolean = False
    Protected Overrides Sub BuildControl()
        If _built Then Exit Sub
        If String.IsNullOrEmpty(FacebookAppId) Then Exit Sub

        _built = True

        Dim appId As String = FacebookAppId

        '-- Fix our post Url 
        Dim PostUrl As String = "https://" & Request.Url.Host & Request.RawUrl
        
        If PostUrl.Contains("?") Then
            PostUrl = PostUrl.Substring(0, PostUrl.IndexOf("?"))
        End If

        Dim oFbSDK As New StringBuilder
        oFbSDK.AppendLine("<div id=""fb-root""></div>")
        oFbSDK.AppendLine("<script>(function(d, s, id) {")
        oFbSDK.AppendLine("  var js, fjs = d.getElementsByTagName(s)[0];")
        oFbSDK.AppendLine("  if (d.getElementById(id)) return;")
        oFbSDK.AppendLine("  js = d.createElement(s); js.id = id;")
        oFbSDK.AppendLine("  js.src = '//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.5&appId=" & appId & "';")
        oFbSDK.AppendLine("  fjs.parentNode.insertBefore(js, fjs);")
        oFbSDK.AppendLine("}(document, 'script', 'facebook-jssdk'));</script>")

        Page.Form.Controls.AddAt(0, New LiteralControl(oFbSDK.ToString()))

        lFb.Text = "<div class=""fb-comments"" data-href=""" & PostUrl & """ data-width=""100%"" data-numposts=""5""></div>"

        Page.Header.Controls.Add(New LiteralControl("<meta property=""fb:app_id"" content=""" & appId & """ />"))
    End Sub

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        BuildControl()
    End Sub
End Class
