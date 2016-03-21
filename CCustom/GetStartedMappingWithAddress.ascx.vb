
Partial Class CCustom_GetStartedMappingWithAddress
    Inherits CLibraryBase

    Protected Overrides Sub BuildControl()
        
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('#" & lnkGetStarted.ClientID & "').click(function(e) {")
        oJs.AppendLine("        e.preventDefault();")
        oJs.AppendLine("        var sAddr = $('#" & StreetAddress.ClientID & "').val();")
        oJs.AppendLine("        var sZip = $('#" & ZipCode.ClientID & "').val();")
        oJs.AppendLine("        window.location = '/Step1-Target.aspx?addr=' + escape(sAddr) + '&zip=' + escape(sZip);")
        oJs.AppendLine("    });")
        oJs.AppendLine("    $('#" & StreetAddress.ClientID & "').keyup(function(e) {")
        oJs.AppendLine("        if (e.which == 13) {")
        oJs.AppendLine("            e.preventDefault();")
        oJs.AppendLine("            $('#" & ZipCode.ClientID & "').focus();")
        oJs.AppendLine("        }")
        oJs.AppendLine("    });")
        oJs.AppendLine("    $('#" & ZipCode.ClientID & "').keyup(function(e) {")
        oJs.AppendLine("        if (e.which == 13) {")
        oJs.AppendLine("            e.preventDefault();")
        oJs.AppendLine("            $('#" & lnkGetStarted.ClientID & "').click();")
        oJs.AppendLine("        }")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        Page.ClientScript.RegisterClientScriptBlock(GetType(String), Me.ClientID & "Init", oJs.ToString, True)
    End Sub
End Class
