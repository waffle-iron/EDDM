
Partial Class CLibrary_GoogleTranslateThisPage
    Inherits System.Web.UI.UserControl

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        jqueryHelper.Include(Page)
        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function() {")
        oJs.AppendLine("    if (top.location != location) {")
        oJs.AppendLine("        jQuery('#" & pGoogleTranslate.ClientID & "').hide();")
        oJs.AppendLine("    }")
        oJs.AppendLine("    jQuery('#" & ddTargetLanguage.ClientID & "').change(function() {")
        'window.location='http://www.google.com/translate_c?hl=en&langpair=en%7Cfr&u=' + window.location.href
        oJs.AppendLine("        location.href = 'http://translate.google.com/translate?hl=en&u=' + escape(window.location.href) + '&sl=en&tl=' + jQuery(this).val();")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        Page.ClientScript.RegisterClientScriptBlock(GetType(String), "GoogleTranslateThisPage:" & Me.ClientID, oJs.ToString, True)
    End Sub
End Class
