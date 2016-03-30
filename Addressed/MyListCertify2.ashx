<%@ WebHandler Language="VB" Class="MyListCertify2" %>

Imports System
Imports System.Web
Imports System.IO
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq

Public Class MyListCertify2 : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        DeleteLog() ''comment 3:22 pm - this works 
        RetrieveListOfCASSCodes()

        Dim projectId As String = appxCMS.Util.Form.GetString("p")
        Dim oRe As New Regex("[^A-Z0-9a-z\-]")
        projectId = oRe.Replace(projectId, "")
        If projectId.Length > 36 Then
            projectId = projectId.Substring(0, 36)
        End If
        Dim sExt As String = appxCMS.Util.Form.GetString("ext")
        Dim colMap As String = appxCMS.Util.Form.GetString("fldMap")
        Dim bHeaderRow As Boolean = appxCMS.Util.Form.GetBoolean("hr")
        'LogThis("line 24 colMap:" & colMap)

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
        'LogThis("-------------the columns (" & projectId & ")-----------")
        'LogThis("line 118 colMap:  " + appxCMS.Util.JavaScriptSerializer.Serialize(Of List(Of ColumnMap))(oColMap))
        'LogThis("---------the data (" & projectId & ")------------------")
        'LogThis(oSb.ToString())
        'LogThis("-----------------------------------")

        oPostData.Add("colmap", appxCMS.Util.JavaScriptSerializer.Serialize(Of List(Of ColumnMap))(oColMap))
        oPostData.Add("fileData", oSb.ToString())


        Dim sResponse As String = appxCMS.Util.httpHelp.PostXMLURLPage("https://demographics2.eddmsite.com/AddressedMyList/ReceiveFile.ashx", oPostData, Nothing, Nothing)

        Dim oResponse As FileSummary = appxCMS.Util.JavaScriptSerializer.Deserialize(Of FileSummary)(sResponse)
        If oResponse IsNot Nothing Then
            '-- Save the Certified File contents back out
            Dim sCertFile As String = oResponse.CertifiedFile
            'LogThis("---------the response (" & projectId & ")------------------")
            'LogThis("line 134 --> the good results are saved in this directory under the name cassed-result.csv: " & sCertFile)
            ''was working 3/29/2016
            'LogThis("-----------------------------------")
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
                        'LogThis("Check record for CASS:" & sLine)
                        CheckForCASSPass(oLine)

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
                        Else    'should be the records that failed CASS 
                            'nope this is not the right logic LogBadRecord("CASS Failed:" & sLine)
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

            Dim sRejects As String = appxCMS.Util.JavaScriptSerializer.Serialize(Of List(Of RejectedAddress))(lstRejectAddresses)
            File.WriteAllText(Path.Combine(sBasePath, "list-rejects.json"), sRejects)


            ''all the bad records are in the text file - write them to json



        End If
    End Sub


    Public Function CheckForCASSPass(oLine As List(Of String)) As Boolean
        Dim logThisRecord As String = String.Empty
        Dim CASSFailCode As String = String.Empty
        Dim lstOfPass As New List(Of String)
        lstOfPass.Add("AS01")
        lstOfPass.Add("AS02")
        Dim returnThis As Boolean = False
        Dim i As Integer = 0

        For Each s As String In oLine
            'LogThis("CheckForCASSPass " & i.ToString & " " & s)
            If i < 8 And s.Length > 0 Then
                logThisRecord = logThisRecord + s + ","
            End If

            If s.Length = 4 Then 'check all columns with a length of 4 to find CASS column
                For Each cssLookup As CassLookup In lstCassLookup
                    If cssLookup.Code = s Then  'this is the cass code
                        If (lstOfPass.Contains(s)) Then
                            returnThis = True
                            Exit For
                        Else
                            CASSFailCode = s
                        End If
                    End If
                Next
            End If
            i = i + 1

        Next

        If CASSFailCode.Length > 0 Then
            If (returnThis = False) Then
                If (logThisRecord.IndexOf("First Name") = -1) Then
                    LogCassFailure(logThisRecord, RetrieveCASSCodeMeaningAndDetails(CASSFailCode))
                End If
            End If
        End If


        Return returnThis
    End Function


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

    Public lstRejectAddresses As New List(Of RejectedAddress)
    Public lstCassLookup As New List(Of CassLookup)


    Public Function ConvertCSVToDataTable(sFileName As String, bHeaderRow As Boolean) As DataTable
        Dim delimiter As String = DetermineDelimiter(sFileName)
        'LogThis("--------------------------------------------------------------------")
        'LogThis("Made it to ConvertCSVToDataTable " & sFileName & " using delimited:" & delimiter)

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
        Dim tempHold As String = String.Empty

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
                    tempHold = String.Join(",", CurrentRow)
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
                LogThis(TextFileReader.ErrorLine) '"@VB.Net Line 347 " & ex.Message & "is not valid and will be skipped")
                LogBadRecord(TextFileReader.ErrorLine) ' & "|" & ex.Message & "|" & ex.StackTrace)
            Catch ex As Exception
                LogThis(ex.Message)
                LogBadRecordGeneric(tempHold, ex.Message)
            End Try
        End While

        TextFileReader.Dispose()


        Return oWorkSheet
    End Function


    Public Shared Sub DeleteLog()
        Dim logFileName As String = "~\Logs\MyListCertify2.txt"
        Dim fullPath As String = HttpContext.Current.Server.MapPath(logFileName)
        If (File.Exists(fullPath)) Then
            File.Delete(fullPath)
        End If

    End Sub

    Public Sub LogBadRecordGeneric(sBadRecord As String, rejectReason As String)
        Dim reject As New RejectedAddress
        reject.Address = sBadRecord.Trim().Substring(0, sBadRecord.Length - 1)
        reject.State = "N/A"
        reject.Zip = "N/A"
        reject.City = "N/A"
        If rejectReason.IndexOf("Column") > -1 Then
            rejectReason = "Too many columns."
        End If
        reject.RejectReason = rejectReason

        lstRejectAddresses.Add(reject)
    End Sub

    Public Sub LogCassFailure(sBadRecord As String, sCassReason As String)
        Dim reject As New RejectedAddress
        reject.Address = sBadRecord.Trim().Substring(0, sBadRecord.Length - 1)
        reject.State = "N/A"
        reject.Zip = "N/A"
        reject.City = "N/A"
        reject.RejectReason = sCassReason

        lstRejectAddresses.Add(reject)
    End Sub

    Public Sub LogBadRecord(sBadRecord As String)
        Dim reject As New RejectedAddress
        reject.Address = sBadRecord.Trim().Substring(0, sBadRecord.Length - 1)
        reject.State = "N/A"
        reject.Zip = "N/A"
        reject.City = "N/A"
        reject.RejectReason = "Record may contain illegal characters."

        lstRejectAddresses.Add(reject)

    End Sub


    Public Shared Sub LogThis(logMessage As String)
        Dim logFileName As String = "~\Logs\MyListCertify2.txt"
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


    Public Class CassLookup
        Public Property Code As String
        Public Property Meaning As String
        Public Property Details As String
        Public Sub New(ByVal inCode As String, ByVal inMeaning As String, ByVal inDetails As String)
            Me.Code = inCode
            Me.Meaning = inMeaning
            Me.Details = inDetails
        End Sub
    End Class

    Public Class RejectedAddress

        Public Property RejectReason() As String
            Get
                Return m_Details
            End Get
            Set
                m_Details = Value
            End Set
        End Property
        Private m_Details As String

        Public Property Address() As String
            Get
                Return m_RejectAddress
            End Get
            Set
                m_RejectAddress = Value
            End Set
        End Property
        Private m_RejectAddress As String

        Public Property City() As String
            Get
                Return m_City
            End Get
            Set
                m_City = Value
            End Set
        End Property
        Private m_City As String

        Public Property State() As String
            Get
                Return m_State
            End Get
            Set
                m_State = Value
            End Set
        End Property
        Private m_State As String
        Public Property Zip() As String
            Get
                Return m_RejectZip
            End Get
            Set
                m_RejectZip = Value
            End Set
        End Property
        Private m_RejectZip As String





    End Class


    Public Sub RetrieveListOfCASSCodes()
        'Dim lstCassLookup As New List(Of CassLookup)
        'Me.lstCassReasons
        lstCassLookup.Add(New CassLookup("AS01", "Address Matched to Postal Database", "Street Address Is valid And deliverable. Check AE08 And AE09 for full deliverability."))
        lstCassLookup.Add(New CassLookup("AS02", "Street Address Match", "Address street matched to USPS database but a suite was missing Or invalid."))
        lstCassLookup.Add(New CassLookup("AS03", "Non-USPS Address", "The input represents a real physical address but it Is not in the USPS database. It may be deliverable by other carriers (UPS, Fedex…)"))
        lstCassLookup.Add(New CassLookup("AS09", "Foreign Postal Code Detected", "Postal Code from a non-supported foreign country detected. A US ZIP Code Or Canadian Postal Code can also return this error if the US Or Canadian data files are not initialized."))
        lstCassLookup.Add(New CassLookup("AS10", "Address Matched to CMRA", "Address belongs to a Commercial Mail Receiving Agency (CMRA) Like The UPS Store®."))
        lstCassLookup.Add(New CassLookup("AS13", "Address has been Updated by LACSLink", "Address has been converted by LACSLink® from a rural-style address to a city-style address."))
        lstCassLookup.Add(New CassLookup("AS14", "Suite Appended by SuiteLink", "A suite was appended by Suite ™ using the address And company name."))
        lstCassLookup.Add(New CassLookup("AS15", "Suite Appended by AddressPlus", "A suite was appended by AddressPlus using the address And last name."))
        lstCassLookup.Add(New CassLookup("AS16", "Address Is vacant", "Address has been unoccupied for 90 days Or more."))
        lstCassLookup.Add(New CassLookup("AS17", "Alternate delivery", "Address does not receive mail at this time."))
        lstCassLookup.Add(New CassLookup("AS18", "DPV Error", "Call 1-800-Melissa Tech Support for assistance."))
        lstCassLookup.Add(New CassLookup("AS20", "This address Is deliverable by USPS only.", "Alternate carriers such as UPS And Fed Ex do not deliver to this address."))
        lstCassLookup.Add(New CassLookup("AS22", "No suggestions.", "No suggested alternatives were found."))
        lstCassLookup.Add(New CassLookup("AS23", "Extraneous Information Found", "Information found in input street address that was not used for verification."))
        lstCassLookup.Add(New CassLookup("AE01", "Zip Code Error", "The Postal Code does not exist And could not be determined by the city/municipality And state/province."))
        lstCassLookup.Add(New CassLookup("AE02", "Unknown Street Error", "An exact street name match could not be found And phonetically matching the street name resulted in either no matches Or matches to more than one street name."))
        lstCassLookup.Add(New CassLookup("AE03", "Component Mismatch Error", "Either the directionals Or the suffix field did not match the post office database, Or there was more than one choice for correcting the address."))
        lstCassLookup.Add(New CassLookup("AE04", "Non-Deliverable Address Error", "The physical location exists but there are no homes on this street. One reason might be railroad tracks Or rivers running alongside this street, as they would prevent construction of homes in this location."))
        lstCassLookup.Add(New CassLookup("AE05", "Multiple Match Error", "Address matched to multiple records. More than one record matches the address And there Is not enough information available in the input address to break the tie between multiple records."))
        lstCassLookup.Add(New CassLookup("AE06", "Early Warning System Error", "This address has been identified in the Early Warning System (EWS) data file And should be included in the next postal database update."))
        lstCassLookup.Add(New CassLookup("AE07", "Missing Minimum Address Input Error", "Minimum required input of address/city/state Or address/zip not found."))
        lstCassLookup.Add(New CassLookup("AE08", "Suite Range Invalid Error", "The input street address was found but the input suite number was not valid."))
        lstCassLookup.Add(New CassLookup("AE09", "Suite Range Missing Error", "The input street address was found but a required suite number Is missing."))
        lstCassLookup.Add(New CassLookup("AE10", "Primary Range Invalid Error", "The street number in the input address was not valid."))
        lstCassLookup.Add(New CassLookup("AE11", "Primary Range Missing Error", "The street number in the input address was missing."))
        lstCassLookup.Add(New CassLookup("AE12", "PO, HC, Or RR Box Number Invalid Error", "The input address PO, RR Or HC number was invalid."))
        lstCassLookup.Add(New CassLookup("AE13", "PO, HC, Or RR Box Number Missing Error", "The input address Is missing a PO, RR, Or HC Box number."))
        lstCassLookup.Add(New CassLookup("AE14", "CMRA Secondary Missing Error", "Address Matched to a CMRA Address but the secondary (Private mailbox number) Is missing."))
        lstCassLookup.Add(New CassLookup("AE15", "Demo Mode", "Demo mode only validates Nevada addresses."))
        lstCassLookup.Add(New CassLookup("AE16", "Expired Database", "The database has expired. Please update with a fresh database."))
        lstCassLookup.Add(New CassLookup("AE17", "Suite Range Extraneous Error", "A suite number was entered but no suite information found for primary address."))
        lstCassLookup.Add(New CassLookup("AE19", "Find Suggestion time-out", "Time allotted to the Find Suggestion function was exceeded."))
        lstCassLookup.Add(New CassLookup("AE20", "Suggestions disabled.", "The SetCASSEnable function must be set to false And the DPV data path must be set in order to use FindSuggestion."))
        lstCassLookup.Add(New CassLookup("AC01", "ZIP Code Change", "The five-digit ZIP Code™ was added Or corrected based on the city And state names."))
        lstCassLookup.Add(New CassLookup("AC02", "State Change", "The state name was corrected based on the combination of city name And ZIP Code."))
        lstCassLookup.Add(New CassLookup("AC03", "City Change", "The city name was added Or corrected based on the ZIP Code."))
        lstCassLookup.Add(New CassLookup("AC04", "Base/Alternate Change", "Some addresses have alternate names, often chosen by the owner Or resident for clarity Or prestige. This change code indicates that the address from the official, Or base, record has been substituted for the alternate."))
        lstCassLookup.Add(New CassLookup("AC05", "Alias Change", "An alias Is a common abbreviation for a long street name, such as 'MLK Blvd' for 'Martin Luther King Blvd.' This change code indicates that the full street name has been substituted for the alias."))
        lstCassLookup.Add(New CassLookup("AC06", "Address1/Address2 Swap", "The value passed to SetAddress could not be verified, but SetAddress2 was used for verification. The value passed to the SetAddress function will be returned by the GetAddress2 function."))
        lstCassLookup.Add(New CassLookup("AC07", "Address1/Company Swap", "A company name was detected in address line 1 and moved to the GetCompany function."))
        lstCassLookup.Add(New CassLookup("AC08", "Plus4 Change", "A non-empty Plus4 was changed."))
        lstCassLookup.Add(New CassLookup("AC09", "Urbanization Change", "The Urbanization was changed."))
        lstCassLookup.Add(New CassLookup("AC10", "Street Name Change", "The street name was changed due to a spelling correction."))
        lstCassLookup.Add(New CassLookup("AC11", "Suffix Change", "The street name suffix was corrected, such as from St to Rd."))
        lstCassLookup.Add(New CassLookup("AC12", "Street Directional Change", "The street pre-directional or post-directional was corrected, such as from 'N' to 'NW.'"))
        lstCassLookup.Add(New CassLookup("AC13", "Suite Name Change", "The unit type designator for the secondary address, such as from 'STE' to 'APT, ' was changed or appended."))
        lstCassLookup.Add(New CassLookup("AC14", "Suite Range Change", "The secondary unit number was changed or appended."))
        'Return lstCassLookup
    End Sub

    ''' <summary>
    ''' refactor to data lookup
    ''' </summary>
    ''' <param name="CassCode"></param>
    ''' <returns></returns>
    Public Function RetrieveCASSCodeMeaningAndDetails(CassCode As String) As String
        Dim returnThis As String = String.Empty
        'LogThis("RetrieveCASSCodeMeaningAndDetails Lookup - " & CassCode)

        'Dim lstCassDetails = RetrieveListOfCASSCodes()
        For Each cassDetail As CassLookup In lstCassLookup
            'LogThis("comparing cassDetail " & cassDetail.Code & " to CassCode:" + CassCode)
            If CassCode = cassDetail.Code Then
                returnThis = cassDetail.Details ' & ")" 'cassDetail.Meaning & " (" & 
            End If
        Next
        'LogThis("RetrieveCASSCodeMeaningAndDetails - " & returnThis)



        Return returnThis
    End Function




End Class