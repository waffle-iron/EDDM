
Partial Class neatUpload_UploadProgressWindow
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        neatProgressBar.Attributes.Add("name", neatProgressBar.ID)
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        jqueryHelper.Include(Page)
        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function() {")
        oJs.AppendLine("    jQuery('#" & ProgressDialog.ClientId & "').dialog({")
        oJs.AppendLine("        bgiframe: true,")
        oJs.AppendLine("        autoOpen: false,")
        oJs.AppendLine("        height: 150,")
        oJs.AppendLine("        width:600,")
        oJs.AppendLine("        modal: true")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        oJs.AppendLine("function ShowFileProgressWindow(validationGroup) {")
        oJs.AppendLine("    if (typeof(Page_ClientValidate) == 'function') {")
        oJs.AppendLine("        if (Page_ClientValidate()) {")
        oJs.AppendLine("            jQuery('#" & ProgressDialog.ClientId & "').dialog('open');")
        oJs.AppendLine("        }")
        oJs.AppendLine("    } else {")
        oJs.AppendLine("        jQuery('#" & ProgressDialog.ClientId & "').dialog('open');")
        oJs.AppendLine("    }")
        oJs.AppendLine("}")
        Page.ClientScript.RegisterClientScriptBlock(GetType(String), "UploadProgress" & Me.ClientID, oJs.ToString, True)
    End Sub
End Class
