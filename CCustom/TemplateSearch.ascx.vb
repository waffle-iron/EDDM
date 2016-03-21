
Partial Class CCustom_TemplateSearch
    Inherits CLibraryBase

    Private _SearchUrl As String = ""
    Public Property SearchUrl As String
        Get
            Return _SearchUrl
        End Get
        Set(value As String)
            _SearchUrl = value
        End Set
    End Property

    Private _IndustryId As Integer = 0
    Public Property IndustryId As Integer
        Get
            Return _IndustryId
        End Get
        Set(value As Integer)
            _IndustryId = value
        End Set
    End Property

    Public Sub DoRender()
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        
        'appxCMS.Util.jQuery.RegisterClientScript(Page, Me.ClientID & "Init", oJs.ToString)
    End Sub

    'Protected Sub DoSearch()
    '    Response.Redirect("~/TemplateSearchResults.aspx?q=" & Server.UrlEncode(Keyword.Text))
    'End Sub

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Dim sSearchUrl As String = Me.SearchUrl
        If String.IsNullOrEmpty(sSearchUrl) Then
            sSearchUrl = "~/TemplateSearchResults.aspx"
        End If

        If sSearchUrl.StartsWith("~/") Then
            sSearchUrl = VirtualPathUtility.ToAbsolute(sSearchUrl)
        End If

        Dim pathForSearch As String = "EDDM"

        If Request.Url.ToString().IndexOf("Addressed") > 0 Then
            pathForSearch = "Addressed"
        End If

        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('#" & btnSearch.ClientID & "').click(function(e) {")
        oJs.AppendLine("        e.preventDefault();")
        oJs.AppendLine("        window.location = '" & sSearchUrl & "?loc=" & pathForSearch & "&q=' + escape($('#" & Keyword.ClientID & "').val()) + '&i=" & Me.IndustryId & "';")
        oJs.AppendLine("    });")
        oJs.AppendLine("    $('input[type=text]').keyup(function(e) {")
        oJs.AppendLine("        if(e.keyCode == 13) {")
        oJs.AppendLine("            $('#" & btnSearch.ClientID & "').click();")
        oJs.AppendLine("        }")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        Page.ClientScript.RegisterClientScriptBlock(GetType(String), Me.ClientID & "Init", oJs.ToString, True)
    End Sub
End Class
