Imports log4net
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml
Imports System.Globalization
Imports System.Reflection
Imports System.IO
Imports WebSupergoo.ABCpdf8
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Configuration
Imports System
Imports Taradel.EF

Public Class ExclusiveUtility

    Private Shared Log As ILog = LogManager.GetLogger(Reflection.MethodBase.GetCurrentMethod().DeclaringType())



    Public Shared Function SaveExclusive(oResponse As Taradel.OrderResponse) As String

        'Dim siteDetails As SiteDetails = New SiteDetails()
        'Dim siteId As Integer = 1
        'Integer.TryParse(Taradel.WLUtil.GetSiteId, siteId)  'EDDM SiteID by default.


        Dim resultString As String = String.Empty
        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                Dim sSql As String = "usp_LockRoutesForOrderID"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@OrderID", oResponse.OrderId)
                    oConnA.Open()
                    Using RDR As SqlDataReader = oCmdA.ExecuteReader()
                        While (RDR.Read())
                            resultString += (RDR("ReturnMessage").ToString())
                            LogThis("SaveExclusive:" & resultString)
                        End While
                    End Using
                End Using 'end command
            End Using 'end SqlConnection
        Catch ex As Exception
            resultString += ("SaveExclusive:" & ex.ToString())
        End Try

        Return resultString

    End Function

    Public Shared Function RetrieveLockDate(orderID As Integer) As String

        'Dim siteDetails As SiteDetails = New SiteDetails()
        Dim siteId As Integer = 1
        Integer.TryParse(Taradel.WLUtil.GetSiteId, siteId)  'EDDM SiteID by default.


        Dim resultString As String = String.Empty
        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                Dim sSql As String = "usp_RetrieveLockDate"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@OrderID", orderID)
                    oConnA.Open()
                    Using RDR As SqlDataReader = oCmdA.ExecuteReader()
                        While (RDR.Read())
                            resultString = (RDR("result"))
                            LogThis("RetrieveLockMessage:" & resultString)
                        End While
                    End Using
                End Using 'end command
            End Using 'end SqlConnection
        Catch ex As Exception
            LogThis("SaveExclusive:" & ex.ToString())
        End Try


        Return resultString
    End Function


    Public Shared Function RetreiveStep2Message(qty As Integer, impressions As Integer) As String
        Dim message As String = "Congratulations, this order qualifies for mail route exclusivity"
        If qty > 10000 Then
            If impressions < 3 Then
                message = "This order does not qualify for mail route exclusivity"
            End If
        Else
            message = "You need at least 10,000 pieces mailed to proceed to checkout"
        End If

        Return message
    End Function

    Public Shared Function RetrieveStep3Message(drops As XmlNode) As String
        'drops/ Drop /@Date
        Dim lstDates As New List(Of Date)
        Dim lstDates2 As New List(Of Date)
        Dim lastDate As String = String.Empty
        For Each node As XmlNode In drops.ChildNodes
            For Each attr As XmlAttribute In node.Attributes
                If attr.Name = "Date" Then
                    lastDate = attr.Value
                End If
            Next
        Next

        Dim lastDate2 As DateTime = DateTime.Parse(lastDate)
        Dim expireDate As DateTime = lastDate2.AddDays(30)



        Return "All routes for this order will be locked until " + expireDate.ToString("M/d/yyyy") + " upon completing checkout"
    End Function









    Public Shared Function RetrieveLockMessage(orderID As Integer) As String

        'Dim siteDetails As SiteDetails = New SiteDetails()
        Dim siteId As Integer = 1
        Integer.TryParse(Taradel.WLUtil.GetSiteId, siteId)  'EDDM SiteID by default.


        Dim resultString As String = String.Empty
        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                Dim sSql As String = "usp_RetrieveLockMessage"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@OrderID", orderID)
                    oConnA.Open()
                    Using RDR As SqlDataReader = oCmdA.ExecuteReader()
                        While (RDR.Read())
                            resultString = (RDR("result"))
                            LogThis("RetrieveLockMessage:" & resultString)
                        End While
                    End Using
                End Using 'end command
            End Using 'end SqlConnection
        Catch ex As Exception
            LogThis("SaveExclusive:" & ex.ToString())
        End Try


        Return resultString
    End Function


    Private Shared Sub LogThis(s As String)
        Dim lw As New LogWriter()
        lw.RecordInLog("ExclusiveUtility:")
    End Sub









End Class