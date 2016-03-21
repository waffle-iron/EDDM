
Partial Class App_MasterPages_PDF
    Inherits System.Web.UI.MasterPage

    Implements appxCMS.IMasterPage


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

