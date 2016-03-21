
Partial Class CCustom_RadioOffer
    Inherits CLibraryBase

    Public Property ImageUrl As String
        Get
            Return imgRadio.ImageUrl
        End Get
        Set(value As String)
            imgRadio.ImageUrl = value
        End Set
    End Property

    Public Property Tooltip As String
        Get
            Return lnkRadioOffer.ToolTip
        End Get
        Set(value As String)
            lnkRadioOffer.ToolTip = value
            pRadioOffer.ToolTip = value
        End Set
    End Property

    <appx.cms(appx.cmsAttribute.DataValueType.CMSSurvey)> _
    Public Property SurveyId As Integer
        Get
            Return RadioSurvey.SurveyId
        End Get
        Set(value As Integer)
            RadioSurvey.SurveyId = value
        End Set
    End Property

    Protected AutoOpen As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim oJs As New StringBuilder
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    $('#" & pRadioOffer.ClientID & "').dialog({")
        oJs.AppendLine("        width:500,")
        oJs.AppendLine("        autoOpen:" & Me.AutoOpen.ToString.ToLower & ",")
        oJs.AppendLine("        modal:true,")
        oJs.AppendLine("        open:function() { jQuery(this).parent().appendTo('form:first'); }")
        oJs.AppendLine("    });")
        oJs.AppendLine("    $('#" & lnkRadioOffer.ClientID & "').click(function(e) {")
        oJs.AppendLine("        e.preventDefault();")
        oJs.AppendLine("        $('#" & pRadioOffer.ClientID & "').dialog('open');")
        oJs.AppendLine("    });")
        oJs.AppendLine("});")
        jqueryHelper.RegisterClientScript(Page, Me.ClientID & "RadioOfferInit", oJs.ToString)
    End Sub
End Class
