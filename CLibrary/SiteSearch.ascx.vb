
Partial Class CLibrary_SiteSearch
    Inherits System.Web.UI.UserControl

    Public Property CssClass() As String
        Get
            Return pSiteSearch.CssClass
        End Get
        Set(ByVal value As String)
            pSiteSearch.CssClass = value
        End Set
    End Property

    Private _WatermarkText As String = ""
    Public Property WatermarkText() As String
        Get
            Return _WatermarkText
        End Get
        Set(ByVal value As String)
            _WatermarkText = value
        End Set
    End Property

    Public Property ButtonText() As String
        Get
            Return lnkSearch.Text
        End Get
        Set(ByVal value As String)
            lnkSearch.Text = value
        End Set
    End Property

    Protected Sub lnkSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSearch.Click
        Response.Redirect("~/Search.aspx?q=" & Server.UrlEncode(SearchText.Text))
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not String.IsNullOrEmpty(Me.WatermarkText) Then
            SearchText.Attributes.Add("placeholder", WatermarkText)
            'jqueryHelper.IncludePlugin(Page, "Watermark", "~/scripts/jquery.watermark.min.js")
            'Dim oJs As New StringBuilder()
            'oJs.AppendLine("jQuery(document).ready(function($) {")
            'oJs.AppendLine("    $('#" & SearchText.ClientID & "').watermark('" & apphelp.JSBless(Me.WatermarkText) & "');")
            'oJs.AppendLine("});")
            'jqueryHelper.RegisterClientScript(Page, "SiteSearchInit", oJs.ToString)
        Else
            SearchText.Attributes.Add("placeholder", "Search")
        End If
    End Sub
End Class
