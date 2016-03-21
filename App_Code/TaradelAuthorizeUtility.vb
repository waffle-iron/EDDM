Imports AuthorizeNet.APICore
Imports AuthorizeNet
Imports System
Imports System.Web.Script.Serialization
Imports System.Net
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data




Public Class TaradelAuthorizeUtility
    'TODO: switch to production creditials.
    Public Const EXAMPLE_MERCHANT_NAME As String = "9Ph4yKN48"
    Public Const EXAMPLE_TRANSACTION_KEY As String = "6c56d4592ETh5aYZ"
    Public Const API_URL As String = "https://apitest.authorize.net/soap/v1/Service.asmx"

    Private Shared m_auth As AuthorizeNet.APICore.merchantAuthenticationType = Nothing
    Public customer As AuthorizeNet.Customer

    Public Class AuthorizeCustomerPaymentProfile
        Inherits AuthorizeCustomer

        Public Property ExpDate() As String
            Get
                Return m_ExpDate
            End Get
            Set(value As String)
                m_ExpDate = value
            End Set
        End Property
        Private m_ExpDate As String


        Public Property CCV() As String
            Get
                Return m_CCV
            End Get
            Set(value As String)
                m_CCV = value
            End Set
        End Property
        Private m_CCV As String

        Public Property EncryptedCCNumber() As String
            Get
                Return m_EncryptedCCNumber
            End Get
            Set(value As String)
                m_EncryptedCCNumber = value
            End Set
        End Property
        Private m_EncryptedCCNumber As String
    End Class

    Public Class AuthorizeCustomer
        Public Property CustomerID() As Integer
            Get
                Return m_CustomerID
            End Get
            Set(value As Integer)
                m_CustomerID = value
            End Set
        End Property
        Private m_CustomerID As Integer
        Public Property UserName() As String
            Get
                Return m_UserName
            End Get
            Set(value As String)
                m_UserName = value
            End Set
        End Property
        Private m_UserName As String
        Public Property ProfileID() As Long
            Get
                Return m_ProfileID
            End Get
            Set(value As Long)
                m_ProfileID = value
            End Set
        End Property
        Private m_ProfileID As Long
        Public Property PaymentProfileID() As Long
            Get
                Return m_PaymentProfileID
            End Get
            Set(value As Long)
                m_PaymentProfileID = value
            End Set
        End Property
        Private m_PaymentProfileID As Long
    End Class

    Public Class AuthorizeTransaction
        Public Property Approved() As String
            Get
                Return m_Approved
            End Get
            Set(value As String)
                m_Approved = value
            End Set
        End Property
        Private m_Approved As String
        Public Property AuthorizationCode() As String
            Get
                Return m_AuthorizationCode
            End Get
            Set(value As String)
                m_AuthorizationCode = value
            End Set
        End Property
        Private m_AuthorizationCode As String
        Public Property CardMask() As String
            Get
                Return m_CardMask
            End Get
            Set(value As String)
                m_CardMask = value
            End Set
        End Property
        Private m_CardMask As String
        Public Property CardType() As String
            Get
                Return m_CardType
            End Get
            Set(value As String)
                m_CardType = value
            End Set
        End Property
        Private m_CardType As String
        Public Property CustomerID() As Integer
            Get
                Return m_CustomerID
            End Get
            Set(value As Integer)
                m_CustomerID = value
            End Set
        End Property
        Private m_CustomerID As Integer
        Public Property EncryptedCCNumber() As String
            Get
                Return m_EncryptedCCNumber
            End Get
            Set(value As String)
                m_EncryptedCCNumber = value
            End Set
        End Property
        Private m_EncryptedCCNumber As String
        Public Property InvoiceID() As String
            Get
                Return m_InvoiceID
            End Get
            Set(value As String)
                m_InvoiceID = value
            End Set
        End Property
        Private m_InvoiceID As String
        Public Property Message() As String
            Get
                Return m_Message
            End Get
            Set(value As String)
                m_Message = value
            End Set
        End Property
        Private m_Message As String
        Public Property PaymentProfileID() As Long
            Get
                Return m_PaymentProfileID
            End Get
            Set(value As Long)
                m_PaymentProfileID = value
            End Set
        End Property
        Private m_PaymentProfileID As Long
        Public Property ProfileID() As Long
            Get
                Return m_ProfileID
            End Get
            Set(value As Long)
                m_ProfileID = value
            End Set
        End Property
        Private m_ProfileID As Long
        Public Property TxAmount() As Decimal
            Get
                Return m_TxAmount
            End Get
            Set(value As Decimal)
                m_TxAmount = value
            End Set
        End Property
        Private m_TxAmount As Decimal
        Public Property TxDate() As DateTime
            Get
                Return m_TxDate
            End Get
            Set(value As DateTime)
                m_TxDate = value
            End Set
        End Property
        Private m_TxDate As DateTime
        Public Property TransactionID() As String
            Get
                Return m_TransactionID
            End Get
            Set(value As String)
                m_TransactionID = value
            End Set
        End Property
        Private m_TransactionID As String


    End Class

    Public Shared Function RetrievePath(host As String) As String
        Dim returnThis As String = host + "/Resources/AuthorizeCreateCustomerPaymentProfile.ashx"
        Return returnThis
    End Function

    ''' <summary>
    ''' Authorize.Net 
    ''' </summary>
    ''' <param name="paymentProfile"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateCustomerPaymentProfile(paymentProfile As AuthorizeCustomerPaymentProfile, host As String) As AuthorizeCustomerPaymentProfile
        Dim jss As New JavaScriptSerializer()
        Dim json As String = jss.Serialize(paymentProfile)
        Dim result As String = String.Empty
        Dim url As String = String.Empty
        Try
            ''url = HttpContext.Server.MapPath("~/App_Data/somedata.xml")
            url = "http://" + host + "/Resources/AuthorizeCreateCustomerPaymentProfile.ashx"
            Dim wreq As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            wreq.Method = "POST"
            wreq.ContentType = "application/json;"
            'set the content type to JSON
            wreq.ContentLength = json.Length
            'returnThis += "json.Length: " + json.Length.ToString()
            Dim sw As Stream = wreq.GetRequestStream
            Dim wr As StreamWriter = New StreamWriter(sw)
            wr.Write(json)
            wr.Close()
            Dim httpResponse As HttpWebResponse = CType(wreq.GetResponse, HttpWebResponse)
            Dim sr As Stream = httpResponse.GetResponseStream
            Dim rd As StreamReader = New StreamReader(sr)
            result = rd.ReadToEnd
            LogThis("CreateCustomerPaymentProfile:" + result)
        Catch ex As Exception
            LogThis("CreateCustomerPayment Profile url:" + url)
            LogThis("CreateCustomerPayment Profile:" + ex.StackTrace.ToString())
            LogThis("CreateCustomerPayment Profile:" + ex.Message.ToString())
        End Try

        Dim returnProfile As AuthorizeCustomerPaymentProfile = New JavaScriptSerializer().Deserialize(Of AuthorizeCustomerPaymentProfile)(result)



        Return returnProfile
    End Function


    ''' <summary>
    ''' Go to Authorize to Create this customer
    ''' </summary>
    ''' <param name="email"></param>
    ''' <param name="customerID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateCustomerProfile2(email As String, customerID As String, host As String) As AuthorizeCustomer
        Dim result As String = String.Empty
        Dim customer As New AuthorizeCustomer
        customer.CustomerID = customerID
        customer.UserName = email
        Dim jss As New JavaScriptSerializer()
        Dim json As String = jss.Serialize(customer)
        Dim url As String = String.Empty

        Try
            url = "http://" + host + "/Resources/AuthorizeCreateCustomer.ashx"
            LogThis("URL:" & url)
            Dim wreq As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            wreq.Method = "POST"
            wreq.ContentType = "application/json;"
            'set the content type to JSON
            wreq.ContentLength = json.Length
            'returnThis += "json.Length: " + json.Length.ToString()
            Dim sw As Stream = wreq.GetRequestStream
            Dim wr As StreamWriter = New StreamWriter(sw)
            wr.Write(json)
            wr.Close()
            Dim httpResponse As HttpWebResponse = CType(wreq.GetResponse, HttpWebResponse)
            Dim sr As Stream = httpResponse.GetResponseStream
            Dim rd As StreamReader = New StreamReader(sr)
            result = rd.ReadToEnd
            LogThis("CreateCustomerProfile2 success " & result)
        Catch ex As Exception
            LogThis("CreateCustomerProfile2 url:" + url)
            LogThis("CreateCustomerProfile2:" + ex.StackTrace.ToString())
            LogThis("CreateCustomerProfile2:" + ex.Message.ToString())
        End Try

        Dim returnCustomer As AuthorizeCustomer = New JavaScriptSerializer().Deserialize(Of AuthorizeCustomer)(result)


        Return returnCustomer
    End Function

    Private Shared Sub LogThis(s As String)
        Dim lw As New LogWriter()
        lw.RecordInLog(s)
    End Sub


    Public Class Crypto
        Private Shared _salt As Byte() = Encoding.ASCII.GetBytes("o6806642kbM7c5")

        ''' <summary>
        ''' Encrypt the given string using AES.  The string can be decrypted using 
        ''' DecryptStringAES().  The sharedSecret parameters must match.
        ''' </summary>
        ''' <param name="plainText">The text to encrypt.</param>
        ''' <param name="sharedSecret">A password used to generate a key for encryption.</param>
        Public Shared Function EncryptStringAES(plainText As String, sharedSecret As String) As String
            If String.IsNullOrEmpty(plainText) Then
                Throw New ArgumentNullException("plainText")
            End If
            If String.IsNullOrEmpty(sharedSecret) Then
                Throw New ArgumentNullException("sharedSecret")
            End If

            Dim outStr As String = Nothing
            ' Encrypted string to return
            Dim aesAlg As RijndaelManaged = Nothing
            ' RijndaelManaged object used to encrypt the data.
            Try
                ' generate the key from the shared secret and the salt
                Dim key As New Rfc2898DeriveBytes(sharedSecret, _salt)

                ' Create a RijndaelManaged object
                aesAlg = New RijndaelManaged()
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8)

                ' Create a decryptor to perform the stream transform.
                Dim encryptor As ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)

                ' Create the streams used for encryption.
                Using msEncrypt As New MemoryStream()
                    ' prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, 4)
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length)
                    Using csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
                        Using swEncrypt As New StreamWriter(csEncrypt)
                            'Write all data to the stream.
                            swEncrypt.Write(plainText)
                        End Using
                    End Using
                    outStr = Convert.ToBase64String(msEncrypt.ToArray())
                End Using
            Finally
                ' Clear the RijndaelManaged object.
                If aesAlg IsNot Nothing Then
                    aesAlg.Clear()
                End If
            End Try

            ' Return the encrypted bytes from the memory stream.
            Return outStr
        End Function

        ''' <summary>
        ''' Decrypt the given string.  Assumes the string was encrypted using 
        ''' EncryptStringAES(), using an identical sharedSecret.
        ''' </summary>
        ''' <param name="cipherText">The text to decrypt.</param>
        ''' <param name="sharedSecret">A password used to generate a key for decryption.</param>
        Public Shared Function DecryptStringAES(cipherText As String, sharedSecret As String) As String
            If String.IsNullOrEmpty(cipherText) Then
                Throw New ArgumentNullException("cipherText")
            End If
            If String.IsNullOrEmpty(sharedSecret) Then
                Throw New ArgumentNullException("sharedSecret")
            End If

            ' Declare the RijndaelManaged object
            ' used to decrypt the data.
            Dim aesAlg As RijndaelManaged = Nothing

            ' Declare the string used to hold
            ' the decrypted text.
            Dim plaintext As String = Nothing

            Try
                ' generate the key from the shared secret and the salt
                Dim key As New Rfc2898DeriveBytes(sharedSecret, _salt)

                ' Create the streams used for decryption.                
                Dim bytes As Byte() = Convert.FromBase64String(cipherText)
                Using msDecrypt As New MemoryStream(bytes)
                    ' Create a RijndaelManaged object
                    ' with the specified key and IV.
                    aesAlg = New RijndaelManaged()
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8)
                    ' Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt)
                    ' Create a decrytor to perform the stream transform.
                    Dim decryptor As ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)
                    Using csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
                        Using srDecrypt As New StreamReader(csDecrypt)

                            ' Read the decrypted bytes from the decrypting stream
                            ' and place them in a string.
                            plaintext = srDecrypt.ReadToEnd()
                        End Using
                    End Using
                End Using
            Finally
                ' Clear the RijndaelManaged object.
                If aesAlg IsNot Nothing Then
                    aesAlg.Clear()
                End If
            End Try

            Return plaintext
        End Function

        Private Shared Function ReadByteArray(s As Stream) As Byte()
            Dim rawLength As Byte() = New Byte(4 - 1) {}
            If s.Read(rawLength, 0, rawLength.Length) <> rawLength.Length Then
                Throw New SystemException("Stream did not contain properly formatted byte array")
            End If

            Dim buffer As Byte() = New Byte(BitConverter.ToInt32(rawLength, 0) - 1) {}
            If s.Read(buffer, 0, buffer.Length) <> buffer.Length Then
                Throw New SystemException("Did not read byte array properly")
            End If

            Return buffer
        End Function
    End Class

#Region "CRUD Authorize Customer Payment Information"

    'AuthorizeCustomer
    Public Shared Function RetrieveAuthorizeCustomerProfile(customer As AuthorizeCustomer) As AuthorizeCustomer
        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                Dim sSql As String = "usp_RetrieveCustomerAuthorizeProfile"
                Dim siteId As Integer = 1
                Integer.TryParse(Taradel.WLUtil.GetSiteId, siteId)  'EDDM SiteID by default.
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@UserName", customer.UserName)
                    oConnA.Open()
                    Using RDR As SqlDataReader = oCmdA.ExecuteReader()
                        While (RDR.Read())
                            customer.CustomerID = TaradelReceiptUtility.OrderCalculator.ConvertToIntegerOrZero(RDR("CustomerID"))
                            customer.PaymentProfileID = TaradelReceiptUtility.OrderCalculator.ConvertToIntegerOrZero(RDR("PaymentProfileID"))
                            customer.ProfileID = TaradelReceiptUtility.OrderCalculator.ConvertToIntegerOrZero(RDR("ProfileID"))
                        End While
                    End Using
                End Using 'end command
            End Using 'end SqlConnection
            LogThis("RetrieveAuthorizeCustomerProfile success with customer.UserName: " + customer.UserName)
        Catch ex As Exception
            LogThis("RetrieveAuthorizeCustomerProfile :" + customer.UserName + " could not retrieved")
            LogThis("RetrieveAuthorizeCustomerProfile :" + ex.StackTrace.ToString())
            LogThis("RetrieveAuthorizeCustomerProfile :" + ex.Message.ToString())
        End Try


        Return customer

    End Function


    Public Shared Function UpdateAuthorizeCustomer(customer As AuthorizeCustomer) As AuthorizeCustomer
        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                Dim sSql As String = "usp_UpdateCustomerProfile"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    oCmdA.Parameters.AddWithValue("@CustomerID", customer.CustomerID)
                    oCmdA.Parameters.AddWithValue("@ProfileID", customer.ProfileID) 'CIMId
                    oConnA.Open()
                    oCmdA.ExecuteNonQuery()
                End Using 'end command
            End Using 'end SqlConnection
            LogThis("UpdateAuthorizeCustomer success")
        Catch ex As Exception
            LogThis("UpdateAuthorizeCustomer:" + ex.StackTrace.ToString())
            LogThis("UpdateAuthorizeCustomer :" + ex.Message.ToString())
        End Try

        Return customer
    End Function


    ''' <summary>
    ''' saves Authorize info in Taradel SQL
    ''' </summary>
    ''' <param name="customer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateAuthorizeCustomerPaymentProfile(customer As AuthorizeCustomer) As AuthorizeCustomer
        'Dim connectString As String = "Data Source=65.175.38.199;Initial Catalog=Taradel-Dev;Persist Security Info=True;User ID=pnd_admin;Password=t3$t"
        Try
            Using oConnA As New SqlConnection(ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString)
                Dim sSql As String = "usp_UpdateAuthorizeCustomerPaymentProfile"
                Using oCmdA As New SqlCommand(sSql, oConnA)
                    oCmdA.CommandType = CommandType.StoredProcedure
                    'oCmdA.Parameters.AddWithValue("@UserName", customer.UserName)
                    oCmdA.Parameters.AddWithValue("@PaymentProfileID", customer.PaymentProfileID)
                    oCmdA.Parameters.AddWithValue("@CustomerID", customer.CustomerID)
                    oCmdA.Parameters.AddWithValue("@ProfileID", customer.ProfileID)
                    oConnA.Open()
                    oCmdA.ExecuteNonQuery()
                End Using 'end command
            End Using 'end SqlConnection
            LogThis("UpdateAuthorizeCustomerPaymentProfile success")
        Catch ex As Exception
            LogThis("UpdateAuthorizeCustomerPaymentProfile:" + ex.StackTrace.ToString())
            LogThis("UpdateAuthorizeCustomerPaymentProfile :" + ex.Message.ToString())
        End Try
        Return customer
    End Function



#End Region




End Class
