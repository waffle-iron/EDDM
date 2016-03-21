
Partial Class UserControls_EnabledToggler
    Inherits System.Web.UI.UserControl

    Private _ObjectEnabled As Boolean = False
    Public Property ObjectEnabled() As Boolean
        Get
            Return _ObjectEnabled
        End Get
        Set(ByVal value As Boolean)
            _ObjectEnabled = value
        End Set
    End Property

    Private _ObjectName As String = ""
    Public Property ObjectName() As String
        Get
            Return _ObjectName
        End Get
        Set(ByVal value As String)
            _ObjectName = value
        End Set
    End Property

    Public Event Enabled()
    Public Event Disabled()

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim sStatus As String = "This " & Me.ObjectName & " is " & IIf(Me.ObjectEnabled, "enabled", "disabled")
        lMsg.Text = sStatus
        lIcon.CssClass = "ui-icon ui-icon-lightbulb"
        lnkSetStatus.Text = "Click to " & IIf(Me.ObjectEnabled, "disable", "enable")
        pStatus.CssClass = IIf(Me.ObjectEnabled, "ui-state-highlight", "ui-state-disabled") & " ui-corner-all"
    End Sub

    Protected Sub lnkSetStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSetStatus.Click
        If Me.ObjectEnabled Then
            RaiseEvent Disabled()
        Else
            RaiseEvent Enabled()
        End If
    End Sub
End Class
