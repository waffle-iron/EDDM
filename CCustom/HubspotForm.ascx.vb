
Partial Class CCustom_HubspotForm
    Inherits CLibraryBase

    Private _portalId As String = ""
    Public Property PortalId As String
        Get
            Return _portalId
        End Get
        Set(value As String)
            _portalId = value
        End Set
    End Property

    Private _formId As String = ""
    Public Property FormId As String
        Get
            Return _formId
        End Get
        Set(value As String)
            _formId = value
        End Set
    End Property

    Protected Overrides Sub BuildControl()
        Dim oJs As New StringBuilder
        oJs.AppendLine("<script charset=""utf-8"" src=""//js.hsforms.net/forms/current.js""></script>")
        oJs.AppendLine("<script>")
        oJs.AppendLine("    hbspt.forms.create({")
        oJs.AppendLine("        portalId:  '" & PortalId & "',")
        oJs.AppendLine("        formId:  '" & FormId & "'")
        oJs.AppendLine("    });")
        oJs.AppendLine("</script>")

        lFormControl.Text = oJs.ToString()
    End Sub


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
