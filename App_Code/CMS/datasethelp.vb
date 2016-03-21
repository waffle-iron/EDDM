Imports System.Data


Public Class datasethelp
    Implements IDisposable

    Public ds As DataSet

    Private _CurTable As String = ""

    Private _NullCount As Integer = 0
    Public ReadOnly Property NullCount() As Integer
        Get
            Return _NullCount
        End Get
    End Property

    Private _DiscardNullOrEmpty As Boolean = True
    Public Property DiscardNullOrEmpty() As Boolean
        Get
            Return _DiscardNullOrEmpty
        End Get
        Set(ByVal value As Boolean)
            _DiscardNullOrEmpty = value
        End Set
    End Property

    Private _HasDuplicates As Boolean = False
    Public ReadOnly Property HasDuplicates() As Boolean
        Get
            Return _HasDuplicates
        End Get
    End Property

    Public ReadOnly Property MajorityDuplicates() As Boolean
        Get
            Return (aDups.Count > (ds.Tables(_CurTable).Rows.Count / 2))
        End Get
    End Property

    Private aDups As New ArrayList

    Private _CaseSensitive As Boolean = False
    Public Property CaseSensitive() As Boolean
        Get
            Return _CaseSensitive
        End Get
        Set(ByVal value As Boolean)
            _CaseSensitive = value
        End Set
    End Property

    Public Sub New(ByVal DataSet As DataSet)
        ds = DataSet
    End Sub

    Public Sub New()
        ds = Nothing
    End Sub

    Private Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean
        '
        ' Compares two values to determine if they are equal. Also compares DBNULL.Value.
        '
        ' NOTE: If your DataTable contains object fields, you must extend this
        ' function to handle the fields in a meaningful way if you intend to group on them.
        '
        If A Is DBNull.Value And B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value Or B Is DBNull.Value Then Return False ' Only one is DBNull.Value.
        If CaseSensitive Then
            Return A = B    ' Value type standard comparison
        Else
            Dim sA As String = A.ToString
            Dim sB As String = B.ToString
            Return sA.ToUpperInvariant = sB.ToUpperInvariant
        End If
    End Function

    Public Function SelectDistinct(ByVal TableName As String, _
                               ByVal SourceTable As DataTable, _
                               ByVal FieldName As String) As DataTable
        _CurTable = TableName
        Dim dt As New DataTable(TableName)
        dt.Columns.Add(FieldName, SourceTable.Columns(FieldName).DataType)
        Dim dr As DataRow
        Dim LastValue As Object = Nothing
        For Each dr In SourceTable.Select("", FieldName)
            If dr(FieldName) Is DBNull.Value Then
                _NullCount = _NullCount + 1
            ElseIf String.IsNullOrEmpty(dr(FieldName).ToString) Then
                _NullCount = _NullCount + 1
            End If
            If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr(FieldName)) Then
                LastValue = dr(FieldName)
                If DiscardNullOrEmpty Then
                    If LastValue IsNot Nothing And LastValue IsNot DBNull.Value Then
                        If Not String.IsNullOrEmpty(LastValue) Then
                            dt.Rows.Add(New Object() {LastValue})
                        End If
                    End If
                Else
                    dt.Rows.Add(New Object() {LastValue})
                End If
            Else
                _HasDuplicates = True
                If Not aDups.Contains(LastValue) Then
                    aDups.Add(LastValue)
                End If
            End If
        Next
        If Not ds Is Nothing Then ds.Tables.Add(dt)
        Return dt
    End Function

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            If ds IsNot Nothing Then
                ds.Dispose()
            End If
            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
