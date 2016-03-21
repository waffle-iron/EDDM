<%@ WebHandler Language="VB" Class="MyListCertify" %>

Imports System
Imports System.Web
Imports System.IO
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq

Public Class MyListCertify : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim projectId As String = appxCMS.Util.Form.GetString("p")
        Dim oRe As New Regex("[^A-Z0-9a-z\-]")
        projectId = oRe.Replace(projectId, "")
        If projectId.Length > 36 Then
            projectId = projectId.Substring(0, 36)
        End If
        Dim sExt As String = appxCMS.Util.Form.GetString("ext")
        Dim colMap As String = appxCMS.Util.Form.GetString("fldMap")
        Dim bHeaderRow As Boolean = appxCMS.Util.Form.GetBoolean("hr")

        'Dim sBasePath As String = context.Server.MapPath("~/app_data/AddressedInbound/" & projectId)
        Dim sBasePath As String = context.Server.MapPath("~/app_data/AddressedListInbound/" & projectId)
        Dim sFile As String = projectId & "." & sExt
        Dim sFileName As String = Path.Combine(sBasePath, sFile)

        Dim oColMap As List(Of ColumnMap) = appxCMS.Util.JavaScriptSerializer.Deserialize(Of List(Of ColumnMap))(colMap)
        Dim oColMap2 As New List(Of ColumnMap)

        'For Each cmap As ColumnMap In oColMap
        '    LogThis("cmap.col:" & cmap.col & "    cmap.colname:" & cmap.colname & "  cmap.fld:" & cmap.fld)
        'Next
        Dim oSb As New StringBuilder
        Dim oWorksheet As DataTable = Nothing

        Select Case sExt.ToLower
            Case "xls", "xlsx"
                'Dim oExcel As New appxCMS.Reporting.ExcelDataHelper()
                Dim oExcel As New ExcelReader()

                oExcel.ExcelFilePath = sFileName
                oExcel.HeaderRow = bHeaderRow
                oExcel.LimitScanRows = True

                Dim oSheets As List(Of String) = oExcel.WorkSheetNames()
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
            Case "csv", "txt"     'added 10/8/2015
                Try
                    oWorksheet = ConvertCSVToDataTable(sFileName, bHeaderRow)
                Catch ex As Exception
                    LogThis(ex.StackTrace)
                    LogThis(ex.Message)
                End Try


                'end case csv 'added 10/8/2015
        End Select

        If oWorksheet IsNot Nothing Then
            Dim oColData As New List(Of String)
            If bHeaderRow Then
                '-- Need to map our col indexes to column names
                For Each oMap In oColMap
                    Dim sCol As String = oWorksheet.Columns(oMap.col).ColumnName
                    oColData.Add(sCol)
                    oMap.colname = sCol
                Next
            Else
                '-- We are going to add "Known" column names and use them in the map
                For iCol As Integer = 0 To oWorksheet.Columns.Count - 1
                    Dim iCurCol As Integer = iCol
                    Dim sCol As String = "Col" & iCurCol
                    oColData.Add(sCol)
                    Dim oMap As ColumnMap = oColMap.FirstOrDefault(Function(c) c.col = iCurCol)
                    If oMap IsNot Nothing Then
                        oMap.colname = sCol
                    End If
                Next
            End If
            oSb.AppendLine(String.Join("|", oColData.ToArray()))

            Dim iFirstRow As Integer = 0
            '-- 20160309 MRM
            '-- Since the Excel reader and CSV to Datatable routines above already take the header row into consideration,
            '-- this next line has been removed to prevent removing an EXTRA line from the output
            'If bHeaderRow Then iFirstRow = 1
            For iRow As Integer = iFirstRow To oWorksheet.Rows.Count - 1
                Dim oRow As DataRow = oWorksheet.Rows(iRow)
                Dim oRowData As New List(Of String)
                For iCol As Integer = 0 To oWorksheet.Columns.Count - 1
                    oRowData.Add(oRow(iCol).ToString())
                Next
                oSb.AppendLine(String.Join("|", oRowData.ToArray()))
            Next
        End If


        '-- Now we have everything ready for a post
        Dim oPostData As New Hashtable
        oPostData.Add("p", projectId)
        LogThis("-------------the columns (" & projectId & ")-----------")
        LogThis(appxCMS.Util.JavaScriptSerializer.Serialize(Of List(Of ColumnMap))(oColMap))
        LogThis("---------the data (" & projectId & ")------------------")
        LogThis(oSb.ToString())
        LogThis("-----------------------------------")

        oPostData.Add("colmap", appxCMS.Util.JavaScriptSerializer.Serialize(Of List(Of ColumnMap))(oColMap))
        oPostData.Add("fileData", oSb.ToString())

        Dim sResponse As String = appxCMS.Util.httpHelp.PostXMLURLPage("https://demographics2.eddmsite.com/AddressedMyList/ReceiveFile.ashx", oPostData, Nothing, Nothing)

        Dim oResponse As FileSummary = appxCMS.Util.JavaScriptSerializer.Deserialize(Of FileSummary)(sResponse)
        If oResponse IsNot Nothing Then
            '-- Save the Certified File contents back out
            Dim sCertFile As String = oResponse.CertifiedFile
            LogThis("---------the response (" & projectId & ")------------------")
            LogThis(sCertFile)
            LogThis("-----------------------------------")
            File.WriteAllText(Path.Combine(sBasePath, "cassed-result.csv"), sCertFile)
            oResponse.CertifiedFile = ""

            Dim oSels As New List(Of TMCRecommends)
            '-- Now, back-fill a summary into the certified file, based on the data in the response
            Dim oCert As List(Of String) = sCertFile.Split(New Char() {ControlChars.NewLine}).ToList

            Dim iHeaderZip As Integer = -1
            Dim iHeaderCr As Integer = -1
            Dim iHeaderCity As Integer = -1
            Dim iHeaderState As Integer = -1

            Dim sHeader As String = ""
            If oCert.Count > 0 Then
                sHeader = oCert(0)
                Dim oHeader As List(Of String) = sHeader.Split(New Char() {"|"}).ToList()

                For i As Integer = 0 To oHeader.Count - 1
                    Dim sHeaderCol As String = oHeader(i)
                    Select Case sHeaderCol.ToLower
                        Case "cass_zip"
                            iHeaderZip = i
                        Case "cass_carrierroute"
                            iHeaderCr = i
                        Case "cass_city"
                            iHeaderCity = i
                        Case "cass_state"
                            iHeaderState = i
                    End Select
                Next
            End If

            For iLine As Integer = 1 To oCert.Count - 1
                Dim sLine As String = oCert(iLine).Trim

                If Not String.IsNullOrEmpty(sLine) Then
                    Dim oLine As List(Of String) = sLine.Split(New Char() {"|"}).ToList()
                    If oLine.Count >= 11 Then
                        Dim sZip As String = oLine(iHeaderZip)
                        Dim sRoute As String = oLine(iHeaderCr)
                        Dim sGeocodeRef As String = sZip & sRoute

                        Dim oArea = oSels.FirstOrDefault(Function(a) a.GeocodeRef = sGeocodeRef)
                        If oArea Is Nothing Then
                            oArea = New TMCRecommends
                            oArea.GeocodeRef = sGeocodeRef
                            oArea.TargetPercent = 100
                            oArea.City = oLine(iHeaderCity)
                            oArea.State = oLine(iHeaderState)
                            oArea.EDDMTotal = 0
                            oArea.AddressedMatches = 0
                            oArea.RouteType = "Addressed"
                            oArea.RouteCount = 0
                            oArea.Selected = True
                            oSels.Add(oArea)
                        End If

                        oArea.AddressedMatches += 1
                        oArea.RouteCount += 1
                    End If
                End If
            Next

            Dim sSummary As String = appxCMS.Util.JavaScriptSerializer.Serialize(Of List(Of TMCRecommends))(oSels)
            File.WriteAllText(Path.Combine(sBasePath, "list-summary.json"), sSummary)

            context.Response.ContentType = "application/json"
            context.Response.Write(appxCMS.Util.JavaScriptSerializer.Serialize(Of FileSummary)(oResponse))
        End If
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Class ColumnMap
        Public Property col As Integer
        Public Property colname As String
        Public Property fld As String
    End Class

    Class FileSummary
        Public Property OriginalCount As Integer
        Public Property ValidCount As Integer
        Public Property ChangedCount As Integer
        Public Property InvalidCount As Integer
        Public Property CertifiedFile As String
    End Class

    Public Function ConvertCSVToDataTable(sFileName As String, bHeaderRow As Boolean) As DataTable
        Dim delimiter As String = DetermineDelimiter(sFileName)
        LogThis("--------------------------------------------------------------------")
        LogThis("Made it to ConvertCSVToDataTable " & sFileName & " using delimited:" & delimiter)

        Dim TextFileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(sFileName)
        TextFileReader.TextFieldType = FileIO.FieldType.Delimited
        TextFileReader.SetDelimiters(delimiter) ' default

        Dim Column As DataColumn
        Dim Row As DataRow
        Dim UpperBound As Int32
        Dim ColumnCount As Int32
        Dim CurrentRow As String()
        Dim iRow As Integer = 0
        Dim oWorkSheet As DataTable = Nothing
        Dim lstColumns As New List(Of String)
        Dim htMap As New Hashtable

        While Not TextFileReader.EndOfData
            Try
                CurrentRow = TextFileReader.ReadFields()
                If (CurrentRow.GetUpperBound(0) = 0) Then 'didn't work right -
                    'LogThis("Reattempting split of " & CurrentRow(0))
                    CurrentRow = CurrentRow(0).ToString().Split(delimiter)
                    'LogThis("After resplit " & CurrentRow(0))

                End If
                UpperBound = CurrentRow.GetUpperBound(0)

                If Not CurrentRow Is Nothing Then
                    'Check if the datatable has been created
                    If oWorkSheet Is Nothing Then
                        oWorkSheet = New DataTable("oWorkSheet")
                        'Get number of columns
                        'Create new columns in the datatable
                        For ColumnCount = 0 To UpperBound
                            Column = New DataColumn()
                            Column.DataType = System.Type.GetType("System.String")
                            Column.ColumnName = "Column" & ColumnCount
                            If bHeaderRow Then
                                htMap.Add(CurrentRow(ColumnCount).ToString(), ColumnCount)
                                'LogThis("added " & CurrentRow(ColumnCount).ToString() & " to the htMap")
                            End If
                            Column.Caption = "Column" & ColumnCount
                            Column.ReadOnly = False
                            Column.Unique = False
                            oWorkSheet.Columns.Add(Column)
                        Next
                    End If

                    Row = oWorkSheet.NewRow
                    For ColumnCount = 0 To UpperBound
                        Row("Column" & ColumnCount) = CurrentRow(ColumnCount).ToString
                    Next

                    oWorkSheet.Rows.Add(Row)


                End If
            Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                LogThis("Line" & ex.Message & "is not valid and will be skipped")
            Catch ex As Exception
                LogThis(ex.ToString)
                LogThis(ex.StackTrace)

            End Try
        End While

        TextFileReader.Dispose()


        Return oWorkSheet
    End Function





    Public Shared Sub LogThis(logMessage As String)
        Dim logFileName As String = "~\Logs\MyListCertify.txt"
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

    Public Shared Function CountString(ByVal inputString As String, ByVal stringToBeSearchedInsideTheInputString As String) As Integer
        Return inputString.Split(stringToBeSearchedInsideTheInputString).Length - 1
    End Function

    Public Shared Function DetermineDelimiter(ByVal sFileName As String) As String
        Dim returnDelimiter As String = ", " ' default value
        Dim entireFile As String = File.ReadAllText(sFileName)
        Dim numberOfCommas As Integer = CountString(entireFile, ", ")
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




End Class