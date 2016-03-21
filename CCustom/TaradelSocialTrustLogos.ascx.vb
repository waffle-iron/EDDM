
Partial Class CCustom_TaradelSocialTrustLogos
    Inherits CLibraryBase


    Dim _bBuilt As Boolean = False
    Protected Overrides Sub BuildControl()
        If _bBuilt Then Exit Sub
        _bBuilt = True

        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    function preloadImages(srcs, imgs, callback) {")
        oJs.AppendLine("        var img;")
        oJs.AppendLine("        var remaining = srcs.length;")
        oJs.AppendLine("        for (var i = 0; i < srcs.length; i++) {")
        oJs.AppendLine("            img = new Image();")
        oJs.AppendLine("            img.onload = function () {")
        oJs.AppendLine("                --remaining;")
        oJs.AppendLine("                if (remaining <= 0) {")
        oJs.AppendLine("                    callback();")
        oJs.AppendLine("                }")
        oJs.AppendLine("            };")
        oJs.AppendLine("            img.src = srcs[i];")
        oJs.AppendLine("            imgs.push(img);")
        oJs.AppendLine("        }")
        oJs.AppendLine("    }")

        oJs.AppendLine("    var oImages = [];")
        oJs.AppendLine("    var oLoaded = [];")
        oJs.AppendLine("    $('img', $('#" & trustList.ClientID & "')).each(function() {")
        oJs.AppendLine("        oImages.push($(this).attr('data-onsrc'));")
        oJs.AppendLine("        $(this).attr('data-offsrc', $(this).attr('src'));")
        oJs.AppendLine("    });")
        oJs.AppendLine("    preloadImages(oImages, oLoaded, hoverOn);")

        oJs.AppendLine("    function hoverOn() {")
        oJs.AppendLine("        $('img', $('#" & trustList.ClientID & "')).mouseover(function() {")
        oJs.AppendLine("            $(this).attr('src', $(this).attr('data-onsrc'));")
        oJs.AppendLine("        }).mouseout(function() {")
        oJs.AppendLine("            $(this).attr('src', $(this).attr('data-offsrc'));")
        oJs.AppendLine("        });")
        oJs.AppendLine("    }")
        oJs.AppendLine("});")

        jqueryHelper.RegisterClientScript(Page, Me.ClientID & "Init", oJs.ToString())
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
