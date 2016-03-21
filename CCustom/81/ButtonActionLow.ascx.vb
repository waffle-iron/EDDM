
Partial Class CCustom_ButtonActionLow
    Inherits CLibraryBase

    Public Property LinkText As String
        Get
            Return hplButton.Text
        End Get
        Set(value As String)
            hplButton.Text = value
        End Set
    End Property

    Public Property NavigateUrl As String
        Get
            Return hplButton.NavigateUrl
        End Get
        Set(value As String)
            hplButton.NavigateUrl = value
        End Set
    End Property

    Protected Overrides Sub BuildControl()

    End Sub

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        BuildControl()
    End Sub
End Class
