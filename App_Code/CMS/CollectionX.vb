Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Collections.Generic
Imports System.Reflection
''' <summary> 
''' Summary description for Collection 
''' </summary> 
<Serializable()> _
Public Class CollectionX(Of T)
    Inherits CollectionBase
    Public Sub New()
        ' 
        ' TODO: Add constructor logic here 
        ' 
    End Sub
    Default Public Property Item(ByVal index As Integer) As T
        Get
            Return DirectCast(Me.List(index), T)
        End Get
        Set(ByVal value As T)
            Me.List(index) = value
        End Set
    End Property

    Public Function IndexOf(ByVal item As T) As Integer
        Return Me.List.IndexOf(item)
    End Function

    Public Function Add(ByVal item As T) As Integer
        Return Me.List.Add(item)
    End Function

    Public Sub Remove(ByVal item As T)
        Me.List.Remove(item)
    End Sub

    Public Sub CopyTo(ByVal array As Array, ByVal index As Integer)
        Me.List.CopyTo(array, index)
    End Sub

    Public Sub AddRange(ByVal collection As CollectionX(Of T))
        For i As Integer = 0 To collection.Count - 1
            Me.List.Add(collection(i))
        Next
    End Sub
    Public Sub AddRange(ByVal collection As T())
        Me.AddRange(collection)
    End Sub
    Public Function Contains(ByVal item As T) As Boolean
        Return Me.List.Contains(item)
    End Function
    Public Sub Insert(ByVal index As Integer, ByVal item As T)
        Me.List.Insert(index, item)
    End Sub

    ' Sort 
    Public Sub Sort(ByVal SortExpression As String, ByVal Order As SortOrder)
        Dim comp As New GenericComparer(SortExpression, Order)
        Me.InnerList.Sort(comp)
    End Sub

    Public Function GetPage(ByVal iPgSize As Integer, ByVal iPg As Integer) As CollectionX(Of T)
        Dim oRet As New CollectionX(Of T)
        For i As Integer = (iPg * iPgSize) - 1 To iPgSize - 1
            If i > Me.List.Count Then
                Exit For
            End If
            oRet.Add(Me.List(i))
        Next
        Return oRet
    End Function
End Class
Public Enum SortOrder
    ASC
    DSC
End Enum
Class GenericComparer
    Implements IComparer

    'Implements IComparer
    Private _property As String
    Private _order As SortOrder
    Public Sub New(ByVal [Property] As String, ByVal Order As SortOrder)
        Me._property = [Property]
        Me._order = Order
    End Sub

    'Sort 
    Public Function Compare(ByVal obj1 As Object, ByVal obj2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim returnValue As Integer
        Dim type As Type = obj1.[GetType]()
        Dim propertie1 As PropertyInfo = type.GetProperty(_property)

        Dim type2 As Type = obj2.[GetType]()
        Dim propertie2 As PropertyInfo = type2.GetProperty(_property)

        Dim finalObj1 As Object = propertie1.GetValue(obj1, Nothing)
        Dim finalObj2 As Object = propertie2.GetValue(obj2, Nothing)

        Dim Ic1 As IComparable = TryCast(finalObj1, IComparable)
        Dim Ic2 As IComparable = TryCast(finalObj2, IComparable)

        If _order = SortOrder.ASC Then
            returnValue = Ic1.CompareTo(Ic2)
        Else
            returnValue = Ic2.CompareTo(Ic1)
        End If
        Return returnValue
    End Function
End Class