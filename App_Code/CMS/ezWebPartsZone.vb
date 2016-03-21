Imports Microsoft.VisualBasic

Namespace EZCustomWebParts
    Public Class ezWebPartsZone
        Inherits WebPartZone

        Protected Overrides Function CreateWebPartChrome() As System.Web.UI.WebControls.WebParts.WebPartChrome
            Return New ezWebPartsChrome(Me, WebPartManager.GetCurrentWebPartManager(Me.Page))
        End Function

        Protected Overrides Sub RenderContents(ByVal writer As System.Web.UI.HtmlTextWriter)
            MyBase.RenderContents(writer)
        End Sub

        Protected Friend Shadows ReadOnly Property RenderClientScript() As Boolean
            Get
                Return MyBase.RenderClientScript
            End Get
        End Property
    End Class
End Namespace