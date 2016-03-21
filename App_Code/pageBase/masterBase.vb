Public Class masterBase
    Inherits MasterPage

    Private Function QStringToInt (ByVal sQFld As String) As Integer
        Dim sid As String = Request.QueryString (sQFld)
        Dim id As Integer = 0
        Integer.TryParse (sid, id)
        Return id
    End Function

    Private Sub Page_Load (ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'Page.ClientScript.RegisterClientScriptInclude(GetType(String), "common", _
        '                                               VirtualPathUtility.ToAbsolute("~/scripts/common.js"))

        If Request.QueryString ("ref") IsNot Nothing Then
            Dim sLnk As String = Request.QueryString ("ref")
            Dim iLnk As Integer = 0

            If Integer.TryParse (sLnk, iLnk) Then
                '-- Register this visit as a referral and set the cookie
                Dim sIPAddress As String = Request.ServerVariables ("HTTP_X_FORWARDED_FOR")
                If String.IsNullOrEmpty (sIPAddress) Then
                    sIPAddress = Request.ServerVariables ("REMOTE_ADDR")
                End If
                Dim sBrowser As String = Request.Browser.Browser & " " & Request.Browser.Version
            End If
        End If

        Dim sPage As String = GetRequestedURL()
        If String.IsNullOrEmpty (sPage) Then
            sPage = VirtualPathUtility.ToAbsolute (Page.AppRelativeVirtualPath)
        End If
        cmshelp.SetupContentPage (sPage, Page)

    End Sub

    Public Function GetRequestedURL() As String
        Dim sURL As String = Request.ServerVariables ("QUERY_STRING").Replace ("404;", "")
        Try
            Dim oURI As New Uri (sURL)
            sURL = oURI.PathAndQuery
            Return sURL
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class
