
Partial Class CLibrary_MenuLinkList
    Inherits CLibraryBase

    Private _Separator As String = "::"
    Public Property Separator() As String
        Get
            Return Server.UrlDecode(_Separator)
        End Get
        Set(ByVal value As String)
            _Separator = value
        End Set
    End Property

    Public Property CssClass() As String
        Get
            If pMenuLinks.CssClass IsNot Nothing Then
                Return pMenuLinks.CssClass
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            pMenuLinks.CssClass = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Using oMenuA As New appxCMSDataTableAdapters.EasyMenuTableAdapter
            Using oMenuT As appxCMSData.EasyMenuDataTable = oMenuA.GetByParent(0)
                If oMenuT.Rows.Count > 0 Then
                    lvMenu.DataSource = oMenuT
                    lvMenu.DataBind()
                Else
                    pMenuLinks.Visible = False
                End If
            End Using
        End Using
    End Sub
End Class
