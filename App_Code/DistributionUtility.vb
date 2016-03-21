Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.Collections.Generic




Public Class DistributionUtility


    '==============================================================================================================================
    '   Written 5/27/2015.  Improve, expand as needed. DSF.
    '==============================================================================================================================


    Public Shared Function RetrieveReferenceID(DistributionID As Integer) As String

        '==============================================================================================================================
        ' Method simply returns the ReferenceID (GUID) for the referred DistributionID.
        '==============================================================================================================================

        Dim results As String = ""
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim errorMsg As New StringBuilder()
        Dim selectSql As String = "SELECT ReferenceId FROM pnd_CustomerDistribution WHERE DistributionID = " & DistributionID
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)


        Try

            connObj.Open()
            results = sqlCommand.ExecuteScalar()

        Catch objException As Exception

            errorMsg.Append("The following errors occurred:<br /><br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " & objException.Message & "</li>")
            errorMsg.Append("<li>Source: " & objException.Source & "</li>")
            errorMsg.Append("<li>Stack Trace: " & objException.StackTrace & "</li>")
            errorMsg.Append("<li>Target Site: " & objException.TargetSite.Name & "</li>")
            errorMsg.Append("<li>SQL: " & selectSql & "</li>")
            errorMsg.Append("</ul>")

            results = errorMsg.ToString()

        Finally

            connObj.Close()

        End Try

        Return results

    End Function



    Public Shared Function RetrieveAddressedListCount(distributionID As Integer) As Integer

        Dim errorMsg As New StringBuilder()
        Dim addressedCount As Integer = 0
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ToString()
        Dim selectSql As String = "SELECT [TotalDeliveries] FROM [pnd_CustomerDistribution] WHERE DistributionID = " & distributionID
        Dim connObj As New System.Data.SqlClient.SqlConnection(connectionString)
        Dim sqlCommand As New System.Data.SqlClient.SqlCommand(selectSql, connObj)


        Try

            connObj.Open()
            addressedCount = sqlCommand.ExecuteScalar()

        Catch objException As Exception

            errorMsg.Append("The following errors occurred:<br /><br />")
            errorMsg.Append("<ul>")
            errorMsg.Append("<li>Message: " & objException.Message & "</li>")
            errorMsg.Append("<li>Source: " & objException.Source & "</li>")
            errorMsg.Append("<li>Stack Trace: " & objException.StackTrace & "</li>")
            errorMsg.Append("<li>Target Site: " & objException.TargetSite.Name & "</li>")
            errorMsg.Append("<li>SQL: " & selectSql & "</li>")
            errorMsg.Append("</ul>")

            EmailUtility.SendAdminEmail("There was an error retrieving the RetrieveAddressedList count in DistributionUtility. Message:<br /><br />" & errorMsg.ToString() & "<br /><br />")

        Finally

            connObj.Close()

        End Try

        Return addressedCount

    End Function



    Public Class TMCRecommends

        Private m_TargetPercent As Decimal
        Public Property TargetPercent() As Decimal
            Get
                Return m_TargetPercent
            End Get
            Set(value As Decimal)
                m_TargetPercent = value
            End Set
        End Property


        Private m_GeocodeRef As String
        Public Property GeocodeRef() As String
            Get
                Return m_GeocodeRef
            End Get
            Set(value As String)
                m_GeocodeRef = value
            End Set
        End Property


        Private m_City As String
        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(value As String)
                m_City = value
            End Set
        End Property


        Private m_State As String
        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(value As String)
                m_State = value
            End Set
        End Property


        Private m_EDDMTotal As Integer
        Public Property EDDMTotal() As Integer
            Get
                Return m_EDDMTotal
            End Get
            Set(value As Integer)
                m_EDDMTotal = value
            End Set
        End Property


        Private m_AddressedMatches As Integer
        Public Property AddressedMatches() As Integer
            Get
                Return m_AddressedMatches
            End Get
            Set(value As Integer)
                m_AddressedMatches = value
            End Set
        End Property


        Private m_RouteType As String
        Public Property RouteType() As String
            Get
                Return m_RouteType
            End Get
            Set(value As String)
                m_RouteType = value
            End Set
        End Property


        Private m_RouteCount As Integer
        Public Property RouteCount() As Integer
            Get
                Return m_RouteCount
            End Get
            Set(value As Integer)
                m_RouteCount = value
            End Set
        End Property


        Private m_Selected As Boolean
        Public Property Selected() As Boolean
            Get
                Return m_Selected
            End Get
            Set(value As Boolean)
                m_Selected = value
            End Set
        End Property

    End Class



    Public Class SavedListFilters

        'Container Class
        Private _hhincome As String
        Public Property hhincome() As String
            Get
                Return _hhincome
            End Get
            Set(value As String)
                _hhincome = value
            End Set
        End Property


        Private _children As String
        Public Property children() As String
            Get
                Return _children
            End Get
            Set(value As String)
                _children = value
            End Set
        End Property


        Private _homeownership As String
        Public Property homeownership() As String
            Get
                Return _homeownership
            End Get
            Set(value As String)
                _homeownership = value
            End Set
        End Property


        Private _ages As String
        Public Property ages() As String
            Get
                Return _ages
            End Get
            Set(value As String)
                _ages = value
            End Set
        End Property


        Private _gender As String
        Public Property gender() As String
            Get
                Return _gender
            End Get
            Set(value As String)
                _gender = value
            End Set
        End Property

    End Class



    Public Class SavedListSelection

        Private m_TargetPercent As Decimal
        Public Property TargetPercent() As Decimal
            Get
                Return m_TargetPercent
            End Get
            Set(value As Decimal)
                m_TargetPercent = value
            End Set
        End Property


        Private m_GeocodeRef As String
        Public Property GeocodeRef() As String
            Get
                Return m_GeocodeRef
            End Get
            Set(value As String)
                m_GeocodeRef = value
            End Set
        End Property


        Private m_City As String
        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(value As String)
                m_City = value
            End Set
        End Property


        Private m_State As String
        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(value As String)
                m_State = value
            End Set
        End Property


        Private m_EDDMTotal As Integer
        Public Property EDDMTotal() As Integer
            Get
                Return m_EDDMTotal
            End Get
            Set(value As Integer)
                m_EDDMTotal = value
            End Set
        End Property


        Private m_AddressedMatches As Integer
        Public Property AddressedMatches() As Integer
            Get
                Return m_AddressedMatches
            End Get
            Set(value As Integer)
                m_AddressedMatches = value
            End Set
        End Property


        Private m_RouteType As String
        Public Property RouteType() As String
            Get
                Return m_RouteType
            End Get
            Set(value As String)
                m_RouteType = value
            End Set
        End Property


        Private m_RouteCount As Integer
        Public Property RouteCount() As Integer
            Get
                Return m_RouteCount
            End Get
            Set(value As Integer)
                m_RouteCount = value
            End Set
        End Property


        Private m_Selected As Boolean
        Public Property Selected() As Boolean
            Get
                Return m_Selected
            End Get
            Set(value As Boolean)
                m_Selected = value
            End Set
        End Property

    End Class


End Class
