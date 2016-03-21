<%@ WebHandler Language="VB" Class="MyListIdentify" %>

Imports System
Imports System.Web
Imports System.IO
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq

Public Class MyListIdentify : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim projectId As String = appxCMS.Util.Querystring.GetString("p")
        Dim oRe As New Regex("[^A-Z0-9a-z\-]")
        projectId = oRe.Replace(projectId, "")
        If projectId.Length > 36 Then
            projectId = projectId.Substring(0, 36)
        End If
        Dim sExt As String = appxCMS.Util.Querystring.GetString("ext")

        'Dim sBasePath As String = context.Server.MapPath("~/app_data/AddressedInbound/" & projectId)
        Dim sBasePath As String = context.Server.MapPath("~/app_data/AddressedListInbound/" & projectId)
        Dim sFile As String = projectId & "." & sExt
        Dim sFileName As String = Path.Combine(sBasePath, sFile)

        Dim oInfo As New MyFileInfo()
        
        Select Case sExt.ToLower
            Case "xls", "xlsx"
                Dim oExcel As New ExcelReader()

                oExcel.ExcelFilePath = sFileName
                oExcel.HeaderRow = False
                oExcel.LimitScanRows = True
                
                Dim oSheets As List(Of String) = oExcel.WorkSheetNames()
                Dim oWorksheet As DataTable = Nothing
                '-- Try to identify the first non-empty sheet
                For Each sWorkSheet As String In oSheets
                    Using oDt As DataTable = oExcel.ExcelToDataTable(sWorkSheet)
                        If oDt IsNot Nothing Then
                            If oDt.Columns.Count > 0 Then
                                If oDt.Rows.Count > 0 Then
                                    oWorksheet = oDt
                                    Exit For
                                End If
                            End If
                        End If
                    End Using
                Next
                
                If oWorksheet IsNot Nothing Then
                    oInfo.ColumnCount = oWorksheet.Columns.Count
                    
                    Dim oSb As New StringBuilder()
                    Dim iRow As Integer = 0
                    For Each oRow As DataRow In oWorksheet.Rows
                        oSb.AppendLine("<tr id=" & Convert.ToChar(34) & "row" & iRow + 1 & Convert.ToChar(34) & ">")
                        For iCol As Integer = 0 To oWorksheet.Columns.Count - 1
                            oSb.AppendLine("<td data-label=""" & oWorksheet.Columns(iCol).ColumnName & """>" & oRow(iCol).ToString() & "</td>")
                        Next
                        oSb.AppendLine("</tr>")
                        iRow += 1
                        If iRow > 10 Then
                            Exit For
                        End If
                    Next
                    oInfo.PreviewRows = oSb.ToString()
                End If

            Case "csv", "txt"
                'added by rs 10/8/2015
                Dim oSb As New StringBuilder()

                Dim delimiter As String = DetermineDelimiter(sFileName)
                Dim TextFileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(sFileName)
                TextFileReader.TextFieldType = FileIO.FieldType.Delimited
                TextFileReader.SetDelimiters(delimiter) ' default

                Dim TextFileTable As DataTable = Nothing

                Dim Column As DataColumn
                Dim Row As DataRow
                Dim UpperBound As Int32
                Dim ColumnCount As Int32
                Dim CurrentRow As String()
                Dim iRow As Integer = 0

                While Not TextFileReader.EndOfData
                    Try
                        CurrentRow = TextFileReader.ReadFields()
                        If Not CurrentRow Is Nothing Then
                            ''# Check if DataTable has been created
                            If TextFileTable Is Nothing Then
                                TextFileTable = New DataTable("TextFileTable")
                                ''# Get number of columns
                                UpperBound = CurrentRow.GetUpperBound(0)
                                ''# Create new DataTable
                                For ColumnCount = 0 To UpperBound
                                    Column = New DataColumn()
                                    Column.DataType = System.Type.GetType("System.String")
                                    Column.ColumnName = "Column" & ColumnCount
                                    Column.Caption = "Column" & ColumnCount
                                    Column.ReadOnly = True
                                    Column.Unique = False
                                    TextFileTable.Columns.Add(Column)
                                Next
                            End If
                            Row = TextFileTable.NewRow
                            For ColumnCount = 0 To UpperBound
                                Row("Column" & ColumnCount) = CurrentRow(ColumnCount).ToString
                            Next
                            TextFileTable.Rows.Add(Row)
                        End If
                    Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                        LogThis("Line " & ex.Message & "is not valid and will be skipped.")
                    End Try
                End While
                TextFileReader.Dispose()

                oInfo.ColumnCount = TextFileTable.Columns.Count


                For Each oRow As DataRow In TextFileTable.Rows
                    oSb.AppendLine("<tr>")
                    For iCol As Integer = 0 To TextFileTable.Columns.Count - 1
                        oSb.AppendLine("<td data-label=""" & TextFileTable.Columns(iCol).ColumnName & """>" & oRow(iCol).ToString() & "</td>")
                    Next
                    oSb.AppendLine("</tr>")
                    iRow += 1
                    If iRow > 10 Then
                        Exit For
                    End If
                Next
                oInfo.PreviewRows = oSb.ToString()

                'end added rs 10/8/2015
                'Case "txt"


        End Select
        
        Dim sResponse As String = appxCMS.Util.JavaScriptSerializer.Serialize(Of MyFileInfo)(oInfo)
        context.Response.ContentType = "application/json"
        context.Response.Write(sResponse)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property


    Public Shared Function CountString(ByVal inputString As String, ByVal stringToBeSearchedInsideTheInputString As String) As Integer
        Return inputString.Split(stringToBeSearchedInsideTheInputString).Length - 1
    End Function

    Public Shared Function DetermineDelimiter(ByVal sFileName As String) As String

        Dim returnDelimiter As String = "," ' default value
        Dim entireFile As String = File.ReadAllText(sFileName)
        Dim numberOfCommas As Integer = CountString(entireFile, ",")
        Dim numberOfBars As Integer = CountString(entireFile, "|")
        Dim numberOfTabs As Integer = CountString(entireFile, "\t")

        If (numberOfBars > numberOfCommas AndAlso numberOfBars > numberOfTabs) Then
            returnDelimiter = "|"
        End If

        If (numberOfTabs > numberOfCommas AndAlso numberOfTabs > numberOfBars) Then
            returnDelimiter = "\t"
        End If

        Return returnDelimiter
    End Function



    Public Class MyFileInfo
        Public Property ColumnCount As Integer
        Public Property PreviewRows As String
    End Class

    Public Shared Sub LogThis(logMessage As String)
        Dim logFileName As String = "~\Logs\MyListIdentify.txt"
        Dim fullPath As String = HttpContext.Current.Server.MapPath(logFileName)

        Try
            Using sw As System.IO.StreamWriter = System.IO.File.AppendText(fullPath)
                sw.WriteLine(logMessage)
            End Using
        Catch ex As Exception
            Dim errorFileName As String = "~\Logs\EDDM-LogError.txt"
            Dim errorFullPath As String = HttpContext.Current.Server.MapPath(errorFileName)

            Try
                Using sw As System.IO.StreamWriter = System.IO.File.AppendText(errorFullPath)
                    sw.WriteLine(ex.ToString())
                    sw.WriteLine(logMessage)
                End Using
                'eat it
            Catch ex2 As Exception
            End Try
        End Try
    End Sub
End Class