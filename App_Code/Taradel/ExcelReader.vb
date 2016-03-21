Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.Web
Imports log4net
Imports System.Text

Public Class ExcelReader

    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Enum ForceCase
        Preserve = 1
        Lower = 2
        Upper = 3
    End Enum

    Public Sub New()
    End Sub

    Private _limitScanRows As Boolean = False
    Public Property LimitScanRows As Boolean
        Get
            Return _limitScanRows
        End Get
        Set(value As Boolean)
            _limitScanRows = value
        End Set
    End Property

    Public Sub New(ByVal sFilePath As String, ByVal bHeaderRow As Boolean)
        Me.ExcelFilePath = sFilePath
        Me.HeaderRow = bHeaderRow
    End Sub

    'Public ReadOnly Property oledbProviderString() As String
    '    Get
    '        Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR={1};IMEX=1"""
    '    End Get
    'End Property

    Public ReadOnly Property OledbProviderString() As String
        Get
            If Me.ExcelFilePath.ToLowerInvariant().EndsWith(".xlsx") Then
                Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0;HDR={1};IMEX=1;MAXSCANROWS={2}"""
            Else
                Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR={1};IMEX=1;MAXSCANROWS={2}"""
            End If
        End Get
    End Property

    Public ReadOnly Property EditableOledbProviderString() As String
        Get
            If Me.ExcelFilePath.ToLower().EndsWith(".xlsx") Then
                Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Mode=ReadWrite;Extended Properties=""Excel 12.0;HDR={1};"""
            Else
                Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=ReadWrite;Extended Properties=""Excel 8.0;HDR={1};"""
            End If
        End Get
    End Property

    Public Property HeaderRow() As Boolean
        Get
            Return _headerRow
        End Get
        Set(ByVal value As Boolean)
            _headerRow = value
        End Set
    End Property
    Private _headerRow As Boolean = True

    Public Property ExcelFilePath() As String
        Get
            Return _excelFilePath
        End Get
        Set(ByVal value As String)
            _excelFilePath = value
        End Set
    End Property
    Private _excelFilePath As String = ""

    Private ReadOnly Property Hdr() As String
        Get
            If HeaderRow Then
                Return "Yes"
            Else
                Return "No"
            End If
        End Get
    End Property

    Public ReadOnly Property ProviderString() As String
        Get
            Return String.Format(OledbProviderString, ExcelFilePath, Hdr, IIf(Me.LimitScanRows, 16, 0))
        End Get
    End Property

    Public ReadOnly Property EditableProviderString() As String
        Get
            Return String.Format(EditableOledbProviderString, ExcelFilePath, Hdr, IIf(Me.LimitScanRows, 16, 0))
        End Get
    End Property

    Public Function WorkSheetNames() As List(Of String)
        Dim worksheets As New List(Of String)
        Dim oledbConn As OleDbConnection = New OleDbConnection(ProviderString)
        Try
            oledbConn.Open()
            Dim dataTable As DataTable = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
            For Each oRow As DataRow In dataTable.Rows
                worksheets.Add(oRow("TABLE_NAME").ToString)
            Next
            oledbConn.Close()
        Catch ex As Exception
            Log.Error(ex.Message, ex)
        Finally
            CType(oledbConn, IDisposable).Dispose()
        End Try
        Return worksheets
    End Function

    Public Function ColumnNames(ByVal sWorksheet As String, ByVal requireCase As ForceCase) As List(Of String)
        Dim columCollection As DataColumnCollection = ColumnNamesCollection(sWorksheet)
        Dim columNames As New List(Of String)
        For Each column As DataColumn In columCollection
            Select Case requireCase
                Case ForceCase.Lower
                    columNames.Add(column.ColumnName.ToLowerInvariant)
                Case ForceCase.Upper
                    columNames.Add(column.ColumnName.ToUpperInvariant)
                Case Else
                    '-- Preserve case
                    columNames.Add(column.ColumnName)
            End Select
        Next
        Return columNames
    End Function

    Public Function ColumnNamesArray(ByVal sWorksheet As String) As String()
        Return ColumnNamesArray(sWorksheet, ForceCase.Preserve)
    End Function

    Public Function ColumnNamesArray(ByVal sWorksheet As String, ByVal requireCase As ForceCase) As String()
        Dim columCollection As DataColumnCollection = ColumnNamesCollection(sWorksheet)
        Dim columNames(columCollection.Count) As String
        For Each column As DataColumn In columCollection
            Select Case requireCase
                Case ForceCase.Lower
                    columNames(column.Ordinal) = column.ColumnName.ToLowerInvariant
                Case ForceCase.Upper
                    columNames(column.Ordinal) = column.ColumnName.ToUpperInvariant
                Case Else
                    '-- Preserve case
                    columNames(column.Ordinal) = column.ColumnName
            End Select
        Next
        Return columNames
    End Function

    Public Function ColumnNamesCollection(ByVal sheetName As String) As DataColumnCollection
        Dim columNames As DataColumnCollection
        ' Using 
        Dim oledbConn As OleDbConnection = New OleDbConnection(ProviderString)
        Try
            Dim sqlStatement As String = "Select top 1 * from [" + sheetName + "]"
            oledbConn.Open()
            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(sqlStatement, oledbConn)
            Dim table As DataTable = New DataTable
            adapter.Fill(table)
            columNames = table.Columns
            oledbConn.Close()
        Finally
            CType(oledbConn, IDisposable).Dispose()
        End Try
        Return columNames
    End Function

    Public Function ExcelToDataSet() As DataSet
        Dim workSheets As List(Of String) = WorkSheetNames()
        Dim dataSet As DataSet = New DataSet
        For i As Integer = 0 To workSheets.Count - 1 ' In workSheets
            Dim dataTable As DataTable = New DataTable(workSheets.Item(i))
            dataTable = ExcelToDataTable(workSheets.Item(i))
            dataSet.Tables.Add(dataTable)
        Next
        Return dataSet
    End Function

    Public Function ExcelToDataTable(ByVal index As Integer) As DataTable
        Return ExcelToDataSet().Tables(index)
    End Function

    Public Function ExcelToDataTable(ByVal workSheet As String) As DataTable
        Dim dataTable As DataTable = New DataTable(workSheet)
        ' Using 
        Dim oledbConn As OleDbConnection = New OleDbConnection(ProviderString)
        Try
            Dim sqlStatement As String = "SELECT * FROM [" + workSheet + "]"
            oledbConn.Open()
            Try
                Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(sqlStatement, oledbConn)
                adapter.Fill(dataTable)
            Catch
            End Try
            oledbConn.Close()
        Finally
            CType(oledbConn, IDisposable).Dispose()
        End Try
        Return dataTable
    End Function

    Public Function ExcelToDataTableCustomRange(sQuery As String) As DataTable
        Dim dataTable As DataTable = New DataTable("Query")
        ' Using 
        Dim oledbConn As OleDbConnection = New OleDbConnection(ProviderString)
        Try
            Dim sqlStatement As String = sQuery
            oledbConn.Open()
            Try
                Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(sqlStatement, oledbConn)
                adapter.Fill(dataTable)
            Catch
            End Try
            oledbConn.Close()
        Finally
            CType(oledbConn, IDisposable).Dispose()
        End Try
        Return dataTable
    End Function

    Public Function WriteWorksheet(newName As String, oData As DataTable, ByRef sMsg As String) As Boolean
        Dim bRet As Boolean = False
        Try
            Using oledbConn As OleDbConnection = New OleDbConnection(EditableProviderString)
                oledbConn.Open()

                Dim aCols As New List(Of String)
                Dim oCreateSb As New StringBuilder
                oCreateSb.Append("CREATE TABLE [" & newName & "] (")
                Dim sJoin As String = ""
                For Each oCol As DataColumn In oData.Columns
                    aCols.Add(oCol.ColumnName)
                    oCreateSb.Append(sJoin & "[" & oCol.ColumnName & "] char(255)")
                    sJoin = ", "
                Next
                oCreateSb.Append(")")

                Dim oCmd As New OleDbCommand(oCreateSb.ToString(), oledbConn)
                oCmd.ExecuteNonQuery()

                For Each oRow In oData.Rows
                    Dim oInsertSb As New StringBuilder
                    oInsertSb.Append("INSERT INTO [" & newName & "] VALUES (")
                    Dim sInsJoin As String = ""
                    For Each sCol As String In aCols
                        Dim sVal As String = oRow(sCol)
                        If sVal.Length > 255 Then sVal = sVal.Substring(0, 255)
                        oInsertSb.Append(sInsJoin & "'" & sVal & "'")
                        sInsJoin = ", "
                    Next
                    oInsertSb.Append(")")
                    Dim oIns As New OleDbCommand(oInsertSb.ToString(), oledbConn)
                    oIns.ExecuteNonQuery()
                Next
                oledbConn.Close()
            End Using

            bRet = True
        Catch ex As Exception
            Log.Error(ex.Message, ex)
            sMsg = ex.Message
        End Try
        Return bRet
    End Function

    Public Function DeleteSheet(sheetName As String, ByRef sMsg As String) As Boolean
        Dim bRet As Boolean = False
        Try
            Using oledbConn As OleDbConnection = New OleDbConnection(EditableProviderString)
                oledbConn.Open()

                Using oCmd As New OleDbCommand("DROP TABLE [" & sheetName & "]", oledbConn)
                    oCmd.ExecuteNonQuery()
                End Using
                oledbConn.Close()
                bRet = True
            End Using

        Catch ex As Exception
            Log.Error(ex.Message, ex)
            sMsg = ex.Message
        End Try
        Return bRet

    End Function
End Class
