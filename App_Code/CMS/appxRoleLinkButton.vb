Imports Microsoft.VisualBasic

Namespace appx.Web
    <ToolboxData("<{0}:RoleLinkButton runat=""server""></{0}:RoleLinkButton>")> _
    Public Class RoleLinkButton
        Inherits LinkButton

        Public Enum BehaviorType
            None = 0
            NoEdit = 1
            Hide = 2
            Message = 3
        End Enum

        Private _AllowedRoles As String = ""
        Public Property AllowedRoles() As String
            Get
                Return Me._AllowedRoles
            End Get
            Set(ByVal value As String)
                Me._AllowedRoles = value
            End Set
        End Property

        Private _UnauthorizedAction As BehaviorType = BehaviorType.None
        Public Property UnauthorizedAction() As BehaviorType
            Get
                Return Me._UnauthorizedAction
            End Get
            Set(ByVal value As BehaviorType)
                Me._UnauthorizedAction = value
            End Set
        End Property

        Private _UnauthorizedMessage As String = ""
        Public Property UnauthorizedMessage() As String
            Get
                Return Me._UnauthorizedMessage
            End Get
            Set(ByVal value As String)
                Me._UnauthorizedMessage = value
            End Set
        End Property

        Private _ConfirmMessage As String = ""
        Public Property ConfirmMessage() As String
            Get
                Return _ConfirmMessage
            End Get
            Set(ByVal value As String)
                _ConfirmMessage = value
            End Set
        End Property

        Public Overrides Sub RenderControl(ByVal writer As System.Web.UI.HtmlTextWriter)
            '-- Check that they are authorized
            Dim aSep() As Char = {",", ";", "|"}
            Dim bAuth As Boolean = False

            If Not String.IsNullOrEmpty(Me.AllowedRoles) Then
                Dim sAllowedRoles() As String = Me.AllowedRoles.Split(aSep)
                For i As Integer = 0 To sAllowedRoles.Length - 1
                    Dim sRole As String = sAllowedRoles(i).ToString.ToLower

                    If HttpContext.Current.User.IsInRole(sRole) Then
                        bAuth = True
                        Exit For
                    End If
                Next
            End If

            If Not bAuth Then
                '-- What to do
                Select Case Me.UnauthorizedAction
                    Case BehaviorType.Hide
                        Me.Visible = False
                    Case BehaviorType.NoEdit
                        Me.Enabled = False
                    Case BehaviorType.Message
                        Me.Enabled = False
                        Me.Text = Me.UnauthorizedMessage
                End Select
            End If

            If Not String.IsNullOrEmpty(Me.ConfirmMessage) Then
                Me.Attributes.Add("confirmmsg", Me.ConfirmMessage)
            End If

            MyBase.RenderControl(writer)
        End Sub
    End Class
End Namespace
