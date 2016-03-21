Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Net
Imports System.IO

Public Class ProductUtility

    '==============================================================================================================================
    '   Written 5/26/2015.  Improve, expand as needed.
    '   DSF
    '==============================================================================================================================


    Public Shared Function RetrieveSlimProductsForUSelectID_SiteID(siteID As String, USelectID As String, host As String) As List(Of SlimProduct)
        'Dim siteID As String = "100" 'ddlSiteID.SelectedItem.Value
        'Dim USelectID As String = "1" 'ddlUSelectID.SelectedItem.Value
        Dim responseFromServer As String = String.Empty
        Dim lstSlimProduct As New List(Of SlimProduct)()
        Dim lstSlimProduct2 As New List(Of SlimProduct)()

        Dim test As New SlimProduct()

        Dim json As String = String.Empty
        Dim url As String = "http://" & host & "/Resources/RetrieveProductsSiteIDUSelectID.ashx?USelectID=" & USelectID + "&SiteID=" & siteID
        test.Name = url
        test.BaseProductID = 0
        test.ProductID = 0
        test.SiteID = 0
        test.USelectID = 0
        Try
            Dim request As WebRequest = WebRequest.Create(url)
            request.Method = "POST"
            request.ContentType = "application/json"
            Using streamWriter = New StreamWriter(request.GetRequestStream())
                streamWriter.Write(json)
                streamWriter.Flush()
                streamWriter.Close()
            End Using


            Dim dataStream As Stream = request.GetRequestStream()
            Dim response__1 As WebResponse = request.GetResponse()
            dataStream = response__1.GetResponseStream()

            Using reader As New StreamReader(dataStream)
                responseFromServer = reader.ReadToEnd()
                reader.Close()
            End Using
            ' Clean up the streams.
            dataStream.Close()
            response__1.Close()
        Catch ex As Exception
            test.Name = test.Name & "|" & ex.StackTrace
            'TODO: Add logging
            'Response.Write(ex.ToString())
            'Return
        End Try


        Try
            Dim jsonSerializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonString As String = responseFromServer
            lstSlimProduct2 = jsonSerializer.Deserialize(Of List(Of SlimProduct))(jsonString)
            For Each smallProduct As SlimProduct In lstSlimProduct2
                lstSlimProduct.Add(smallProduct)
            Next

        Catch ex As Exception
            test.Name = test.Name & "|" & ex.StackTrace
            'TODO: Add logging
            'Response.Write(ex.ToString())
            'FOR DEBUG --> context.Response.Write("compiled ok");
        Finally
        End Try

        lstSlimProduct.Add(test)

        Return lstSlimProduct
    End Function

    'try not to change this code 3/16/2016
    Public Class SlimProduct
        Public Property ProductID() As Integer
            Get
                Return m_ProductID
            End Get
            Set
                m_ProductID = Value
            End Set
        End Property
        Private m_ProductID As Integer
        Public Property BaseProductID() As Integer
            Get
                Return m_BaseProductID
            End Get
            Set
                m_BaseProductID = Value
            End Set
        End Property
        Private m_BaseProductID As Integer
        Public Property USelectID() As Integer
            Get
                Return m_USelectID
            End Get
            Set
                m_USelectID = Value
            End Set
        End Property
        Private m_USelectID As Integer
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set
                m_Name = Value
            End Set
        End Property
        Private m_Name As String
        Public Property SiteID() As Integer
            Get
                Return m_SiteID
            End Get
            Set
                m_SiteID = Value
            End Set
        End Property
        Private m_SiteID As Integer
    End Class





    'Deprecated 3/16/2016

    'Public Shared Function GetEddmBaseProductID(addressedWLProductID As Integer, siteID As Integer) As Integer

    '    '======================================================================================================
    '    '   This function will look up and return a EDDM WhiteLabel BaseProductID for a 'matching' AddressedList
    '    '   ProductID.  Requires the AddressedListProductId and the SiteID.
    '    '======================================================================================================

    '    Dim results As Integer = 0
    '    Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
    '    Dim errorMsg As New StringBuilder()
    '    Dim selectSql As String = "EXEC usp_GetEddmBaseProductIDForListProduct @paramSiteID = " & siteID & ", @paramListWLProductID = " & addressedWLProductID
    '    Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
    '    Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)
    '    Dim sqlReader As SqlDataReader

    '    Try

    '        connObj.Open()
    '        sqlReader = sqlCommand.ExecuteReader()

    '        If sqlReader.HasRows Then
    '            Do While sqlReader.Read()
    '                results = sqlReader("EddmBaseProductID")
    '            Loop
    '        End If

    '    Catch objException As Exception
    '        'Nothing yet

    '    Finally
    '        connObj.Close()
    '    End Try

    '    Return results

    'End Function



    'Public Shared Function GetEddmProductID(addressedWLProductID As Integer, siteID As Integer) As Integer

    '    '======================================================================================================
    '    '   This function will look up and return a EDDM WhiteLabel BaseProductID for a 'matching' AddressedList
    '    '   ProductID.  Requires the AddressedListProductId and the SiteID.
    '    '======================================================================================================

    '    Dim results As Integer = 0
    '    Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
    '    Dim errorMsg As New StringBuilder()
    '    Dim selectSql As String = "EXEC usp_GetEddmProductIDForListProduct @paramSiteID = " & siteID & ", @paramListWLProductID = " & addressedWLProductID
    '    Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
    '    Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)
    '    Dim sqlReader As SqlDataReader

    '    Try

    '        connObj.Open()
    '        sqlReader = sqlCommand.ExecuteReader()

    '        If sqlReader.HasRows Then
    '            Do While sqlReader.Read()
    '                results = sqlReader("EddmProductID")
    '            Loop
    '        End If

    '    Catch objException As Exception
    '        'Nothing yet

    '    Finally
    '        connObj.Close()
    '    End Try

    '    Return results

    'End Function



    'Public Shared Function CalculateWeightedPrice(QTY As Integer, eddmPrice As Decimal, addressedPrice As Decimal) As Decimal


    '    '======================================================================================================
    '    '   This function will calculate and weight the price per piece based on the QTYs of each time.  
    '    '   Typically only called and used in TMC/ Blended scenarios.
    '    '======================================================================================================

    '    Dim results As Decimal = 0

    '    results = Math.Round(((addressedPrice + eddmPrice) / QTY), 2, MidpointRounding.AwayFromZero)
    '    'results = ((addressedPrice + eddmPrice) / QTY)

    '    Return results

    'End Function



End Class
