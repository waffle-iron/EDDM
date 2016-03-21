
Partial Class CLibrary_ContentMessage
    Inherits CLibraryBase

    Private _ContentPage As String = ""
    Public Property ContentPage As String
        Get
            If String.IsNullOrEmpty(_ContentPage) Then
                _ContentPage = appxCMS.PageBase.GetRequestedURL(Page)
            End If
            If _ContentPage.StartsWith("~/") Then
                _ContentPage = _ContentPage.Substring(1)
            End If
            Return _ContentPage
        End Get
        Set(value As String)
            _ContentPage = value
        End Set
    End Property

    Private _MessageArea As String = ""
    Public Property MessageAreaName As String
        Get
            Return _MessageArea
        End Get
        Set(value As String)
            _MessageArea = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim oCMSMessage As appxCMS.ContentMessage = appxCMS.ContentDataSource.GetPageContentMessage(Me.ContentPage, Me.MessageAreaName)
        If oCMSMessage IsNot Nothing Then
            If GetType(Page).IsAssignableFrom(GetType(appxCMS.PageBase)) Then
                Dim oPage As appxCMS.PageBase = DirectCast(Page, appxCMS.PageBase)
                phContent.Controls.Add(oPage.FormatContent(oCMSMessage.Content))
            End If
        Else
            phContent.Controls.Add(New LiteralControl("Content block named '" & Me.MessageAreaName & "' not defined in page " & Me.ContentPage))
        End If
    End Sub
End Class
