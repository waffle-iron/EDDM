Imports Microsoft.VisualBasic

Public Interface IEventListener
    Class appxEventArgs
        Inherits EventArgs

        Private _ArgList As New System.Collections.Generic.List(Of appxEventArg)
        Public Property ArgList() As System.Collections.Generic.List(Of appxEventArg)
            Get
                Return _ArgList
            End Get
            Set(ByVal value As System.Collections.Generic.List(Of appxEventArg))
                _ArgList = value
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(ByVal oArgList As System.Collections.Generic.List(Of appxEventArg))
            Me.ArgList = oArgList
        End Sub
    End Class

    Structure appxEventArg
        Public Name As String
        Public Value As String
    End Structure

    Sub HandleEvent(ByVal sender As Object, ByVal e As appxEventArgs)
End Interface
