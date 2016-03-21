Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Data.SqlClient




Public Class AddressedListUtility


    Public Shared Function GetSavedFilters(distributionID As Integer) As String

        'This function builds a presentable HTML string to be used on web forms/pages.  String contains all AddressedList filters user selected.

        Dim results As String = String.Empty

        '1) Get all possible records from pnd_SavedAddressedListFilters from this distribution
        Dim getSQL As String = "EXEC usp_GetSavedAddressedListFilters @paramDistributionID = " & distributionID
        Dim connectString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim conn As New System.Data.SqlClient.SqlConnection(connectString)
        Dim getData As New System.Data.SqlClient.SqlCommand(getSQL, conn)

        conn.Open()

        Dim reader As SqlDataReader = getData.ExecuteReader()
        If reader.HasRows Then
            Do While reader.Read()

                If reader("DataType").ToString() = "Char" Then
                    results = results & "<strong>" & reader("CategoryName").ToString() & "</strong>: " & LookUpFilterLabel(reader("FilterValue").ToString(), Convert.ToInt32(reader("AddressedListFilterCategoryID").ToString())) & "<br />"
                End If


                If (reader("DataType").ToString() = "Range") Then
                    Dim minMaxValues As String() = GetMinAndMaxValues(reader("AddressedListFilterCategoryID"), distributionID).Split(",")
                    results = results & "<strong>" & reader("CategoryName").ToString() & "</strong>: From " & Convert.ToInt32(minMaxValues(0).ToString()).ToString("N0") & " to " & Convert.ToInt32(minMaxValues(1).ToString()).ToString("N0") & "<br />"
                End If


                If (reader("DataType").ToString() = "CommaDelimited") Then

                    'Look at Filter Value, split it up into string Array, loop through values and replace them with Display Value
                    Dim preText As String = "<strong>" & reader("CategoryName").ToString() & "</strong>:<br /><ul>"
                    Dim midText As String = ""
                    Dim splitVals As String() = (reader("FilterValue").ToString()).Split(",")

                    For Each val As String In splitVals
                        midText = midText & "<li>" & LookUpFilterLabel(val, Convert.ToInt32(reader("AddressedListFilterCategoryID").ToString())) & "</li>"
                    Next

                    results = results & preText & midText & "</ul>"

                End If


            Loop
        Else
            results = "(none selected)"
        End If

        reader.Close()
        conn.Close()

        Return results

    End Function



    Protected Shared Function LookUpFilterLabel(lookUpVal As String, categoryID As Integer) As String

        Dim results As String = String.Empty
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim selectSql As String = "SELECT [DisplayValue] FROM [pnd_AddressedListFilterValues] WHERE FilterValue = '" & lookUpVal & "' AND AddressedListFilterCategoryID = " & categoryID
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)


        Try
            connObj.Open()
            results = sqlCommand.ExecuteScalar()

        Catch objException As Exception
            results = "unknown"
        Finally
            connObj.Close()
        End Try

        Return results

    End Function



    Protected Shared Function GetMinAndMaxValues(categoryID As Integer, distributionID As Integer) As String

        Dim results As String = String.Empty

        Dim getSQL As String = "EXEC usp_GetMinMaxRangeValuesAddressedList @paramCategoryID = " & categoryID & ", @paramDistributionID = " & distributionID
        Dim connectString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim conn As New System.Data.SqlClient.SqlConnection(connectString)
        Dim getData As New System.Data.SqlClient.SqlCommand(getSQL, conn)

        conn.Open()

        Dim reader As SqlDataReader = getData.ExecuteReader()
        If reader.HasRows Then
            Do While reader.Read()
                results = reader("MinMax").ToString()
            Loop
        End If

        reader.Close()
        conn.Close()

        Return results

    End Function


End Class
