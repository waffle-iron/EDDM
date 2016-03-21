
Partial Class CLibrary_BreadCrumbs
    Inherits CLibraryBase

    Private _Separator As String = " &raquo; "

    Public Property Separator() As String
        Get
            Return _Separator
        End Get
        Set(ByVal value As String)
            _Separator = value
        End Set
    End Property

    Public Property CssClass() As String
        Get
            If pBreadCrumbs.CssClass IsNot Nothing Then
                Return pBreadCrumbs.CssClass
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            pBreadCrumbs.CssClass = value
        End Set
    End Property

    Private Function GetRequestedURL() As String
        Dim sURL As String = Request.ServerVariables("QUERY_STRING").Replace("404;", "")
        Try
            Dim oURI As New Uri(sURL)
            sURL = oURI.PathAndQuery
            Return sURL
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim sPg As String = GetRequestedURL()
        If String.IsNullOrEmpty(sPg) Then
            sPg = VirtualPathUtility.ToAbsolute(Page.AppRelativeVirtualPath)
        End If
        Using oMenuA As New appxCMSDataTableAdapters.BreadCrumbsTableAdapter
            Using oBCT As appxCMSData.BreadCrumbsDataTable = oMenuA.GetData(sPg)
                If oBCT.Rows.Count > 0 Then
                    lvCrumbs.DataSource = oBCT
                    lvCrumbs.DataBind()
                Else
                    pBreadCrumbs.Visible = False
                End If
            End Using
        End Using
    End Sub
End Class
