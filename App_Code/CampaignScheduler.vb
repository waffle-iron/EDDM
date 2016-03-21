Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports System
Imports System.Data.SqlClient
Imports System.Configuration


Public Class CampaignScheduler


    Private _newMoverSelected As Boolean = False
    Public Property newMoverSelected As Boolean
        Get
            Return _newMoverSelected
        End Get
        Set(ByVal value As Boolean)
            _newMoverSelected = value
        End Set
    End Property


    Private _targetedEmailSelected As Boolean = False
    Public Property targetedEmailSelected As Boolean
        Get
            Return _targetedEmailSelected
        End Get
        Set(ByVal value As Boolean)
            _targetedEmailSelected = value
        End Set
    End Property


    Private _genericRoutes As String = ""
    Public Property genericRoutes As String

        'Generic Routes is simply a placeholder for now.  As it is, EDDM Campaigns can be "split" whereas New Mover and Targeted Email campaigns
        'go the entire route list each time. Since it's possible that the Campaign Overview repeater AND XML file could show different route
        ' strings for each DROP, this is a place to store the entire string of routes "unsplit".  This will probably be refined later....

        Get
            Return _genericRoutes
        End Get
        Set(ByVal value As String)
            _genericRoutes = value
        End Set

    End Property

    Public Function GetScheduleData(orderID As Integer) As DataView
        Dim scheduleTable As New DataTable
        scheduleTable.Columns.Add("StartDate", GetType(DateTime))
        scheduleTable.Columns.Add("Quantity", GetType(String))
        scheduleTable.Columns.Add("Type", GetType(String))
        scheduleTable.Columns.Add("Routes", GetType(String))
        scheduleTable.AcceptChanges()

        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                oConnA.Open()
                'TODO: turn into a stored procedure 
                Dim sSql As String = "usp_RetrieveAddOnSchedule"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@orderID", orderID)
                    Using RDR = oCmdA.ExecuteReader()
                        If RDR.HasRows Then
                            Do While RDR.Read()
                                Dim newRow As DataRow = scheduleTable.NewRow()
                                newRow("StartDate") = DateTime.Parse((RDR.Item("DropDate2").ToString()))
                                newRow("Quantity") = (RDR.Item("Quantity").ToString())
                                newRow("Type") = (RDR.Item("Type").ToString())
                                newRow("Routes") = (RDR.Item("Routes").ToString())
                                scheduleTable.Rows.Add(newRow)
                            Loop
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Dim newRow As DataRow = scheduleTable.NewRow()
            newRow("StartDate") = DateTime.Now
            newRow("Quantity") = "0"
            newRow("Type") = "0"
            newRow("Routes") = ex.Message + System.Environment.NewLine + ex.StackTrace
            scheduleTable.Rows.Add(newRow)
        End Try

        scheduleTable.AcceptChanges()

        Dim scheduleDataView As New DataView(scheduleTable)
        scheduleDataView.Sort = "StartDate"
        'END OF PART 4 ==========================================================================================



        Return scheduleDataView


    End Function


    Public Function GetScheduleData2(orderXml As String) As DataView
        Dim scheduleTable As New DataTable
        scheduleTable.Columns.Add("StartDate", GetType(DateTime))
        scheduleTable.Columns.Add("Quantity", GetType(String))
        scheduleTable.Columns.Add("Type", GetType(String))
        scheduleTable.Columns.Add("Routes", GetType(String))
        scheduleTable.AcceptChanges()

        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                oConnA.Open()
                'TODO: turn into a stored procedure 
                Dim sSql As String = "usp_RetrieveAddOnScheduleFromXml"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@orderstring", orderXml)
                    Using RDR = oCmdA.ExecuteReader()
                        If RDR.HasRows Then
                            Do While RDR.Read()
                                Dim newRow As DataRow = scheduleTable.NewRow()
                                newRow("StartDate") = DateTime.Parse((RDR.Item("DropDate2").ToString()))
                                newRow("Quantity") = (RDR.Item("Quantity").ToString())
                                newRow("Type") = (RDR.Item("Type").ToString())
                                newRow("Routes") = (RDR.Item("Routes").ToString())
                                scheduleTable.Rows.Add(newRow)
                            Loop
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Dim newRow As DataRow = scheduleTable.NewRow()
            newRow("StartDate") = DateTime.Now
            newRow("Quantity") = "0"
            newRow("Type") = "0"
            newRow("Routes") = ex.Message + System.Environment.NewLine + ex.StackTrace
            scheduleTable.Rows.Add(newRow)
        End Try

        scheduleTable.AcceptChanges()

        Dim scheduleDataView As New DataView(scheduleTable)
        scheduleDataView.Sort = "StartDate"
        'END OF PART 4 ==========================================================================================



        Return scheduleDataView


    End Function

    Public Function GetScheduleData(oCart As XmlDocument) As DataView
        targetedEmailSelected = CheckForEmails(oCart)
        newMoverSelected = CheckForNewMovers(oCart)
        '===================================================================================================================
        ' This function will extract the data from the XML Cart, build and populate a DataTable which is then sorted and 
        ' bound to the rptSchedule control on the page.
        '
        ' BUSINESS LOGIC:
        '   EDDM Campaigns - always DROP on the Friday before the START date (which is a Monday). Start Date is the 
        '       earliest possible date the mail pieces can get in the mail stream. 
        '   Email Campaigns - always drop/broadcast on the following Tuesday AFTER the EDDM Drop Dates. Are sent
        '       out in weekly intervals if there are no drops remaining --Updated 2/26/15
        '   New Mover Campaigns - always drop one week AFTER the first EDDM START and every 4 WEEKS until canceled.
        '
        '
        ' Example Schedule:
        '
        ' EDDM Drop Date    |   Start Date  |   Drop Number     |   Pieces  |   Description     |       Routes
        '------------------------------------------------------------------------------------------------------------------
        '   3/6/2015 (fri)      3/9/2015 (mon)      1               10000       EDDM Postcard               .....
        '   3/6/2015            3/10/2015           1               2000        Emails (1)                  .....
        '   3/6/2015            3/16/2015           1               300         New Mover                   .....
        '   4/3/2015            4/6/2015            2               1000        EDDM Postcard               .....
        '   4/3/2015            4/7/2015            2               2000        Emails (2)                  .....
        '   3/6/2015            4/13/2015           2               300         New Mover (4 weeks later)   .....
        '   5/1/2015            5/4/2015            3               10000       EDDM Postcard               .....
        '   5/1/2015            5/5/2015            3               2000        Emails (3)                  .....
        '   3/6/2015            5/11/2015           3               300         New Mover (4 weeks later...until canceled)
        '===================================================================================================================

        Dim scheduleTable As New DataTable
        scheduleTable.Columns.Add("StartDate", GetType(DateTime))
        scheduleTable.Columns.Add("Quantity", GetType(Integer))
        scheduleTable.Columns.Add("Type", GetType(String))
        scheduleTable.Columns.Add("Routes", GetType(String))
        scheduleTable.AcceptChanges()

        'Create DataTable to be populated throughout this process
        Try


            'Document to read from
            'Dim oCart As XmlDocument = Profile.Cart


            'STEP 1 - Get EDDM Product Data ====================================================================================
            Dim eddmProductNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='EDDM']")
            Dim eddmProductID As Integer = XmlConvert.ToInt32(eddmProductNode.Attributes("ProductID").Value())
            Dim eddmProductName As String = eddmProductNode.Attributes("Name").Value()

            'Get the Number of drops for this EDDM Product.
            Dim eddmDropsNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='EDDM']/Attribute[@Name='Number of Drops']")
            Dim eddmNumOfDrops As Integer = XmlConvert.ToInt32(eddmDropsNode.Attributes("Value").Value())

            'Get ALL nodes named 'Drop' under 'Drops'.
            Dim eddmDropList As XmlNodeList = oCart.SelectNodes("//Product[@Type='EDDM']/Drops/Drop")


            'Create a Min Date place holder. This placeholder, 'eddmFirstStartDate' will hold the earliest 'StartDate' in the loop.
            'By default, it is set to 1 year aheead so it can be easily tested against and overwritten.
            Dim eddmFirstStartDate As DateTime = DateTime.Now.AddYears(1)
            Dim duration As TimeSpan

            'Data pulled from each loop
            Dim eddmStartDate As DateTime
            Dim routes As String
            Dim dropNumber As Integer


            For Each drop As XmlNode In eddmDropList

                'Read the 'Date' which is actually Drop data and add 3 days to it to make it a Monday.
                eddmStartDate = Convert.ToDateTime(drop.Attributes("Date").Value())
                eddmStartDate = eddmStartDate.AddDays(3)
                routes = ""
                dropNumber = drop.Attributes("Number").Value()

                'This will store test with negative number the first pass through.  Afterward, it should test with a postive number
                'which will NOT reset the eddmFirstStartDate value.  eddmFirstStartDate will be used in testing/setting with Targeted Email
                'and New Mover schedule entries.
                duration = eddmStartDate - eddmFirstStartDate

                If duration.TotalDays <= 0 Then
                    eddmFirstStartDate = eddmStartDate
                End If


                'Get ALL nodes named 'Area' under 'Drop'.
                Dim eddmAreaList As XmlNodeList = oCart.SelectNodes("//Product[@Type='EDDM']/Drops/Drop[@Number='" & dropNumber & "']/Area")

                'Loop through all the areas and get the ROUTES
                For Each area As XmlNode In eddmAreaList
                    routes += area.Attributes("Name").Value() & ", "
                Next

                routes = routes.Substring(0, (routes.Length - 2))

                'Set to use later for emails and new movers.  New Mover and Targeted Emails do not split drops.  They send to all addresses within the routes each time.
                genericRoutes = routes

                'Add to row(s) to table: | Date | QTY | Type | Routes |
                scheduleTable.Rows.Add(eddmStartDate, drop.Attributes("Total").Value(), "EDDM " & eddmProductName & "(Drop " & dropNumber & ")", routes)

            Next
            'END OF EDDM Product Data ====================================================================================




            If targetedEmailSelected = True Then

                'STEP 2 - Get EMAIL Product Data =========================================================================

                Dim emailProductNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='Targeted Email']")
                Dim emailProductName As String = emailProductNode.Attributes("Name").Value()
                Dim emailProductID As Integer = XmlConvert.ToInt32(emailProductNode.Attributes("ProductID").Value())
                Dim emailQTY As Integer = XmlConvert.ToInt32(emailProductNode.Attributes("Quantity").Value())

                'In case I do something stupid, this is a fail-safe counter.
                Dim dummyCounter As Integer = 0

                'Three Drop dates for EMAIL Campaign
                Dim emailFirstDropDate As DateTime = eddmFirstStartDate
                Dim emailSecondDropDate As DateTime
                Dim emailThirdDropDate As DateTime

                'If the current day happens to be a Tuesday, then add ONE week
                If emailFirstDropDate.DayOfWeek = 2 Then
                    emailFirstDropDate.AddDays(7)

                    'Otherwise, find the next Tuesday
                Else
                    Do While ((emailFirstDropDate.DayOfWeek <> emailFirstDropDate.DayOfWeek.Tuesday) And (dummyCounter < 10))
                        emailFirstDropDate = emailFirstDropDate.AddDays(1)
                        dummyCounter = dummyCounter + 1
                    Loop
                End If

                'Set the 2nd and 3rd drop dates
                emailSecondDropDate = emailFirstDropDate.AddDays(7)
                emailThirdDropDate = emailFirstDropDate.AddDays(14)


                'Add to table: | Date | QTY | Type | Routes |
                scheduleTable.Rows.Add(emailFirstDropDate, emailQTY, emailProductName & " (Broadcast 1)", genericRoutes)
                scheduleTable.Rows.Add(emailSecondDropDate, emailQTY, emailProductName & " (Broadcast 2)", genericRoutes)
                scheduleTable.Rows.Add(emailThirdDropDate, emailQTY, emailProductName & " (Broadcast 3)", genericRoutes)
                'END OF EMAIL Product Data ===========================================================================

            End If



            If newMoverSelected = True Then

                'PART 3 - Get New Movers Data ============================================================================
                Dim moverProductNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='New Mover Postcard']")
                Dim moverProductName As String = moverProductNode.Attributes("Name").Value()
                Dim moverProductID As Integer = XmlConvert.ToInt32(moverProductNode.Attributes("ProductID").Value())
                Dim moverQTY As Integer = XmlConvert.ToInt32(moverProductNode.Attributes("Quantity").Value())
                Dim moverFirstDropDate As DateTime = eddmFirstStartDate.AddDays(7)

                'add to table : | Date | QTY | Type | Routes |
                scheduleTable.Rows.Add(moverFirstDropDate, moverQTY, moverProductName & " (Drop 1)", genericRoutes)
                scheduleTable.Rows.Add(moverFirstDropDate.AddDays(28), 0, moverProductName & " (every 4 weeks)", genericRoutes)
                'END OF Get New Movers Data ============================================================================

            End If
        Catch ex As Exception

            scheduleTable.Rows.Add(DateTime.Now, 0, ex.Message, genericRoutes)

        End Try



        scheduleTable.AcceptChanges()
        'PART 4 - Resort table into a DataView and send it back ===================================================
        Dim scheduleDataView As New DataView(scheduleTable)
        scheduleDataView.Sort = "StartDate"
        'END OF PART 4 ==========================================================================================



        Return scheduleDataView


    End Function


    Public Function CheckForNewMovers(oCart As XmlDocument) As Boolean

        Dim results As Boolean = False

        'Dim oCart As XmlDocument = Profile.Cart
        Dim newMoverNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='New Mover Postcard']")

        If (newMoverNode IsNot Nothing) Then
            results = True
        End If

        Return results

    End Function


    Public Function CheckForAddressedAddOns(oCart As XmlDocument) As Boolean

        Dim results As Boolean = False
        Dim xNode As XmlNode = oCart.SelectSingleNode("//Product[@Type='AddressedMail AddOn']")

        If (xNode IsNot Nothing) Then
            results = True
        End If

        Return results


    End Function



    Public Function CheckForEmails(oCart As XmlDocument) As Boolean

        Dim results As Boolean = False

        'Dim oCart As XmlDocument = Profile.Cart
        Dim emailNode As XmlNode = oCart.SelectSingleNode("//Product[@Name='Targeted Email']")

        'bug fix -- sometimes node added as Targeted Emails so check for that  -- 2/4/2016 rs
        If (emailNode Is Nothing) Then
            emailNode = oCart.SelectSingleNode("//Product[@Name='Targeted Emails']")
        End If


        If (emailNode IsNot Nothing) Then
            results = True
        End If

        Return results


    End Function

    Public Function CheckForEmails(orderID As Integer) As Integer
        Dim result As Integer = 0
        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                oConnA.Open()

                'TODO: turn into a stored procedure 
                Dim sSql As String = "usp_RetrieveAddOnEmails"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@orderID", orderID)
                    Dim oResult As Object = oCmdA.ExecuteScalar()
                    If oResult IsNot Nothing Then
                        Integer.TryParse(oResult.ToString(), result)
                    End If
                End Using
            End Using

        Catch ex As Exception
           
        End Try

        Return result
    End Function



End Class
