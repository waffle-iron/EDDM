
Partial Class CCustom_ROICalculator
    Inherits CLibraryBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        jqueryHelper.RegisterStylesheet(Page, "/cmsimages/ROICalculator/index.css")
        'jqueryHelper.IncludePlugin(Page, "jQuery-191", "~/cmsimages/ROICalculator/jquery-1.9.1.js")
        jqueryHelper.IncludePlugin(Page, "jQuery-Actual", "/cmsimages/ROICalculator/jquery.actual.min.js")
        jqueryHelper.IncludePlugin(Page, "CoreUtil", "/cmsimages/ROICalculator/Core.Utilities.js")
        jqueryHelper.IncludePlugin(Page, "iSlider", "/cmsimages/ROICalculator/iSlider.js")
        jqueryHelper.IncludePlugin(Page, "ROICalcInit", "/cmsimages/ROICalculator/roicalc.js")
    End Sub
End Class
