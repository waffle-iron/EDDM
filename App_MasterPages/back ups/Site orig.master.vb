
Partial Class App_MasterPages_Site
    Inherits System.Web.UI.MasterPage

    Implements appxCMS.IMasterPage

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        appxCMS.Util.jQuery.Include(Page)
        appxCMS.Util.jQuery.IncludePlugin(Page, "common", "~/scripts/common.js")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function(){")
        oJs.AppendLine("    jQuery('ul.sf-menu').supersubs({")
        oJs.AppendLine("        minWidth: 12,")
        oJs.AppendLine("        maxWidth: 27,")
        oJs.AppendLine("        extraWidth: 1")
        oJs.AppendLine("    }).superfish();")
        oJs.AppendLine("});")
        appxCMS.Util.jQuery.RegisterClientScript(Page, "superfish", oJs.ToString)
    End Sub

    Public Property BodyClass As String Implements appxCMS.IMasterPage.BodyClass
        Get
            Dim sClass As String = ""
            If masterBody.Attributes("class") IsNot Nothing Then
                sClass = masterBody.Attributes("class")
            End If
            Return sClass
        End Get
        Set(value As String)
            Dim sClass As String = (Me.BodyClass & " " & value).Trim
            If masterBody.Attributes("class") Is Nothing Then
                masterBody.Attributes.Add("class", sClass)
            Else
                masterBody.Attributes("class") = sClass
            End If
        End Set
    End Property
End Class

