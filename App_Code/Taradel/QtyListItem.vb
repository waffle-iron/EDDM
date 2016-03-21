Imports Microsoft.VisualBasic

Public Class QtyListItem
    Private _Quantity As Integer = 0
    Public Property Quantity As Integer
        Get
            Return _Quantity
        End Get
        Set(value As Integer)
            _Quantity = value
        End Set
    End Property

    Private _PricePerPiece As String = ""
    Public Property PricePerPiece As String
        Get
            Return _PricePerPiece
        End Get
        Set(value As String)
            _PricePerPiece = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(Qty As Integer, Price As String)
        Me.Quantity = Qty
        Me.PricePerPiece = Price
    End Sub
End Class
