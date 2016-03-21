
Partial Class UserControls_cmsImageBrowser
    Inherits System.Web.UI.UserControl

    Public Property Tooltip() As String
        Get
            Return pCMSImageBrowser.ToolTip
        End Get
        Set(ByVal value As String)
            pCMSImageBrowser.ToolTip = value
        End Set
    End Property

    Public Property Text() As String
        Get
            Return SelectedImage.Text
        End Get
        Set(ByVal value As String)
            SelectedImage.Text = value
        End Set
    End Property

    Public Property Width() As String
        Get
            Return SelectedImage.Width.ToString
        End Get
        Set(ByVal value As String)
            SelectedImage.Width = New WebControls.Unit(value)
        End Set
    End Property

    Private _SchoolUpload As Boolean = False
    Public Property SchoolUpload() As Boolean
        Get
            Return _SchoolUpload
        End Get
        Set(ByVal value As Boolean)
            _SchoolUpload = value
        End Set
    End Property

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        lCMSImageBrowser.Text = "<iframe src=""/admin/Editor/cmsImageBrowser.aspx?targetsrcfld=" & hfSelImg.ClientID & "&school=" & SchoolUpload.ToString().ToLower() & """ frameborder=""0"" width=""100%"" height=""475""></iframe>"

        jqueryHelper.Include(Page)
        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('#" & pCMSImageBrowser.ClientID & "').dialog({autoOpen:false,modal:true,width:800,height:545,")
        oJs.AppendLine("        buttons:{")
        oJs.AppendLine("            'Select Image': function() {")
        oJs.AppendLine("                var sImg = jQuery('#" & hfSelImg.ClientID & "').val();")
        oJs.AppendLine("                if (sImg != null && sImg != '') {")
        oJs.AppendLine("                    jQuery('#" & SelectedImage.ClientID & "').val(sImg);")
        oJs.AppendLine("                }")
        oJs.AppendLine("                jQuery('#" & pCMSImageBrowser.ClientID & "').dialog('close');")
        oJs.AppendLine("            },")
        oJs.AppendLine("            'Cancel': function() {")
        oJs.AppendLine("                jQuery('#" & pCMSImageBrowser.ClientID & "').dialog('close');")
        oJs.AppendLine("            }")
        oJs.AppendLine("        }")
        oJs.AppendLine("     });")
        oJs.AppendLine("    $('#" & lnkCMSImageBrowser.ClientID & "').click(function(e) {")
        oJs.AppendLine("        e.preventDefault();")
        oJs.AppendLine("        jQuery('#" & pCMSImageBrowser.ClientID & "').dialog('open');")
        oJs.AppendLine("    });")
        oJs.AppendLine("    $('#" & pCMSImageBrowser.ClientID & "').css('padding', '0').css('overflow', 'hidden');")
        oJs.AppendLine("});")
        jqueryHelper.RegisterClientScript(Page, Me.ClientID & "Init", oJs.ToString)
    End Sub
End Class
