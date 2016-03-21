
Partial Class vpage_surveyreceived
    Inherits appxCMS.PageBase

    Protected ReadOnly Property SurveyID() As Integer
        Get
            Return QStringToInt("id")
        End Get
    End Property

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sConfirmation As String = "Thank you for your response."

        Dim oSurvey As appxCMS.SurveyHeader = appxCMS.SurveyDataSource.GetSurvey(Me.SurveyID)
        If oSurvey IsNot Nothing Then
            If oSurvey.ConfirmationText IsNot Nothing Then
                sConfirmation = oSurvey.ConfirmationText
            End If
        End If

        phText.Controls.Add(FormatContent(sConfirmation))
    End Sub
End Class
