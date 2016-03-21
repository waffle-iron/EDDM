
Partial Class CCustom_JavascriptInclude
    Inherits CLibraryBase

    Private _scriptId As String = ""
    Public Property ScriptId As String
        Get
            Return _scriptId
        End Get
        Set(value As String)
            _scriptId = value
        End Set
    End Property


    Private _scriptPath As String = ""
    Public Property ScriptPath As String
        Get
            Return _scriptPath
        End Get
        Set(value As String)
            _scriptPath = value
        End Set
    End Property

    Protected Overrides Sub BuildControl()
        If Not String.IsNullOrEmpty(ScriptPath) Then
            Page.ClientScript.RegisterClientScriptInclude(ScriptId, ScriptPath)
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
