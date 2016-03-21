
Partial Class UserControls_cmsRolePicker
    Inherits System.Web.UI.UserControl

    Public Property Text() As String
        Get
            Return SelectedRoles.Text
        End Get
        Set(ByVal value As String)
            SelectedRoles.Text = value
        End Set
    End Property

    Public Property Width() As String
        Get
            Return SelectedRoles.Width.ToString
        End Get
        Set(ByVal value As String)
            SelectedRoles.Width = New WebControls.Unit(value)
        End Set
    End Property

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        lCMSRoleBrowser.Text = "<iframe src=""/admin/Editor/cmsRoleBrowser.aspx?targetsrcfld=" & hfSelRoles.ClientID & "&selects=" & Server.UrlEncode(Me.Text) & """ frameborder=""0"" width=""100%"" height=""475""></iframe>"

        jqueryHelper.Include(Page)
        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('#" & pCMSRoleBrowser.ClientID & "').dialog({autoOpen:false,modal:true,width:800,height:545,")
        oJs.AppendLine("        buttons:{")
        oJs.AppendLine("            'Set Roles': function() {")
        oJs.AppendLine("                var sRoles = jQuery('#" & hfSelRoles.ClientID & "').val();")
        oJs.AppendLine("                if (sRoles != null && sRoles != '') {")
        oJs.AppendLine("                    $('#" & SelectedRoles.ClientID & "').val(sRoles);")
        oJs.AppendLine("                }")
        oJs.AppendLine("                $('#" & pCMSRoleBrowser.ClientID & "').dialog('close');")
        oJs.AppendLine("            },")
        oJs.AppendLine("            'Cancel': function() {")
        oJs.AppendLine("                $('#" & pCMSRoleBrowser.ClientID & "').dialog('close');")
        oJs.AppendLine("            }")
        oJs.AppendLine("        }")
        oJs.AppendLine("     });")
        oJs.AppendLine("    $('#" & lnkRoleBrowser.ClientID & "').click(function(e) {")
        oJs.AppendLine("        e.preventDefault();")
        oJs.AppendLine("        $('#" & pCMSRoleBrowser.ClientID & "').dialog('open');")
        oJs.AppendLine("    });")
        oJs.AppendLine("    $('#" & pCMSRoleBrowser.ClientID & "').css('padding', '0').css('overflow', 'hidden');")
        oJs.AppendLine("});")
        jqueryHelper.RegisterClientScript(Page, Me.ClientID & "Init", oJs.ToString)
    End Sub
End Class
