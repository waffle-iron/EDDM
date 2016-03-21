Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Net
Imports System.IO





Public Class HubSpot

    '=====================================================================================================================================
    '   Written to allow any white label web site to send data to HubSpot without using hard coded portalID and form GUID values.
    '   Improve as needed.  DSF. 2/25/2016.
    '=====================================================================================================================================

    'Properties
    Private _firstName As String = ""
    Public Property firstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property


    Private _lastName As String = ""
    Public Property lastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property


    Private _companyName As String = ""
    Public Property companyName() As String
        Get
            Return _companyName
        End Get
        Set(ByVal value As String)
            _companyName = value
        End Set
    End Property


    Private _emailAddress As String = ""
    Public Property emailAddress() As String
        Get
            Return _emailAddress
        End Get
        Set(ByVal value As String)
            _emailAddress = value
        End Set
    End Property


    Private _phoneNumber As String = ""
    Public Property phoneNumber() As String
        Get
            Return _phoneNumber
        End Get
        Set(ByVal value As String)
            _phoneNumber = value
        End Set
    End Property


    Private _address As String = ""
    Public Property address() As String
        Get
            Return _address
        End Get
        Set(ByVal value As String)
            _address = value
        End Set
    End Property


    Private _address2 As String = ""
    Public Property address2() As String
        Get
            Return _address2
        End Get
        Set(ByVal value As String)
            _address2 = value
        End Set
    End Property


    Private _city As String = ""
    Public Property city() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property


    Private _state As String = ""
    Public Property state() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property


    Private _zipCode As String = ""
    Public Property zipCode() As String
        Get
            Return _zipCode
        End Get
        Set(ByVal value As String)
            _zipCode = value
        End Set
    End Property


    Private _industry As String = ""
    Public Property industry() As String
        Get
            Return _industry
        End Get
        Set(ByVal value As String)
            _industry = value
        End Set
    End Property


    Private _hsPortalID As String = ""
    Public Property hsPortalID() As String
        Get
            Return _hsPortalID
        End Get
        Set(ByVal value As String)
            _hsPortalID = value
        End Set
    End Property


    Private _hsFormGUID As String = ""
    Public Property hsFormGUID() As String
        Get
            Return _hsFormGUID
        End Get
        Set(ByVal value As String)
            _hsFormGUID = value
        End Set
    End Property


    Private _cookieValue As String = ""
    Public Property cookieValue() As String
        Get
            Return _cookieValue
        End Get
        Set(ByVal value As String)
            _cookieValue = value
        End Set
    End Property


    Private _pageTitle As String = ""
    Public Property pageTitle() As String
        Get
            Return _pageTitle
        End Get
        Set(ByVal value As String)
            _pageTitle = value
        End Set
    End Property


    Private _pageURL As String = ""
    Public Property pageURL() As String
        Get
            Return _pageURL
        End Get
        Set(ByVal value As String)
            _pageURL = value
        End Set
    End Property


    Private _ipAddress As String = ""
    Public Property ipAddress() As String
        Get
            Return _ipAddress
        End Get
        Set(ByVal value As String)
            _ipAddress = value
        End Set
    End Property


    Private _orderID As Integer = 0
    Public Property orderID() As Integer
        Get
            Return _orderID
        End Get
        Set(ByVal value As Integer)
            _orderID = value
        End Set
    End Property


    Private _orderDate As String = ""
    Public Property orderDate() As String
        Get
            Return _orderDate
        End Get
        Set(ByVal value As String)
            _orderDate = value
        End Set
    End Property


    Private _orderAmount As Decimal = 0
    Public Property orderAmount() As Decimal
        Get
            Return _orderAmount
        End Get
        Set(ByVal value As Decimal)
            _orderAmount = value
        End Set
    End Property


    Private _paidAmount As Decimal = 0
    Public Property paidAmount() As Decimal
        Get
            Return _paidAmount
        End Get
        Set(ByVal value As Decimal)
            _paidAmount = value
        End Set
    End Property


    Private _productName As String = ""
    Public Property productName() As String
        Get
            Return _productName
        End Get
        Set(ByVal value As String)
            _productName = value
        End Set
    End Property


    Private _quantity As Integer = 0
    Public Property quantity() As Integer
        Get
            Return _quantity
        End Get
        Set(ByVal value As Integer)
            _quantity = value
        End Set
    End Property


    Private _paper As String = ""
    Public Property paper() As String
        Get
            Return _paper
        End Get
        Set(ByVal value As String)
            _paper = value
        End Set
    End Property


    Private _jobName As String = ""
    Public Property jobName() As String
        Get
            Return _jobName
        End Get
        Set(ByVal value As String)
            _jobName = value
        End Set
    End Property


    Private _jobComments As String = ""
    Public Property jobComments() As String
        Get
            Return _jobComments
        End Get
        Set(ByVal value As String)
            _jobComments = value
        End Set
    End Property






    Public Function SendLeadData(hsObj As HubSpot) As String

        '===================================================================================================================
        '   Note: Populate the HubSpot object and then send it back to this function.  This function will return OK or the 
        '   the HubSpot error message.
        '===================================================================================================================


        'Code sample originated from http://developers.hubspot.com/docs/methods/forms/submit_form

        'HubSpot API Repsonses:
        '204 when the form submissions is sucessful - comes as "No Content".
        '302 when the form submissions is sucessful and a redirectUrl is included or set in the form settings.
        '404 when the Form GUID is not found for the provided Portal ID.
        '500 when an internal server error occurs.

        Dim results As String = ""
        Dim logger As New LogWriter()


        'Build Endpoint URL
        Dim endPoint As String = String.Format("https://forms.hubspot.com/uploads/form/v2/{0}/{1}", hsObj.hsPortalID, hsObj.hsFormGUID)


        'Only SOME items need to be Serialized as JSON data - mainly the Form data.
        'Create a HashTable for those items.
        Dim hsContextDataTbl As New Hashtable
        hsContextDataTbl.Add("hutk", hsObj.cookieValue)
        hsContextDataTbl.Add("ipAddress", hsObj.ipAddress)
        hsContextDataTbl.Add("pageUrl", hsObj.pageURL)
        hsContextDataTbl.Add("pageName", hsObj.pageTitle)


        'Serialize these values into JSON
        Dim json As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim strHubSpotContextJSON As String = json.Serialize(hsContextDataTbl)



        'Create second HashTable top contain values to passed as querystring parameters
        Dim hsUrlValues As New Hashtable
        hsUrlValues.Add("firstname", hsObj.firstName)
        hsUrlValues.Add("lastname", hsObj.lastName)
        hsUrlValues.Add("email", hsObj.emailAddress)
        hsUrlValues.Add("company", hsObj.companyName)
        hsUrlValues.Add("phone", hsObj.phoneNumber)
        hsUrlValues.Add("address", hsObj.address)
        hsUrlValues.Add("address2", hsObj.address2)
        hsUrlValues.Add("city", hsObj.city)
        hsUrlValues.Add("state", hsObj.state)
        hsUrlValues.Add("zip", hsObj.zipCode)
        hsUrlValues.Add("industry", hsObj.industry)



        'If this came from a completed order...
        If (hsObj.pageTitle.Contains("Account Transaction")) Then
            hsUrlValues.Add("order_number", hsObj.orderID)
            hsUrlValues.Add("order_date", DateTimeToUnixTimestamp(hsObj.orderDate))
            hsUrlValues.Add("order_amount", hsObj.orderAmount.ToString())
            hsUrlValues.Add("total_payments", hsObj.paidAmount.ToString())
            hsUrlValues.Add("product", hsObj.productName)
            hsUrlValues.Add("quantity", hsObj.quantity.ToString())
            hsUrlValues.Add("paper", hsObj.paper)
            hsUrlValues.Add("job_name", hsObj.jobName)
            hsUrlValues.Add("message", hsObj.jobComments)
        End If


        'Create string with post data
        Dim strPostData As StringBuilder = New StringBuilder()


        'Add HastTable values into string
        For Each item As DictionaryEntry In hsUrlValues
            strPostData.Append(item.Key & "=" + HttpContext.Current.Server.UrlEncode(item.Value) & "&")
        Next


        'Append strHubSpotContextJSON
        strPostData.Append("hs_context=" & HttpContext.Current.Server.UrlEncode(strHubSpotContextJSON))

        'logger.RecordInLog("[HubSpot Util] Here is the endPoint: " & endPoint)
        'logger.RecordInLog("[HubSpot Util] Here is the Post Data: " & strPostData.ToString())
        'logger.RecordInLog("[HubSpot Util] Sent From: " & hsObj.pageURL)



        'Create web request object
        'Dim webRequest As WebRequest = webRequest.Create(endPoint)
        Dim hsRequest As HttpWebRequest = CType(WebRequest.Create(endPoint), HttpWebRequest)

        'Set headers for POST
        hsRequest.Method = "POST"
        hsRequest.ContentType = "application/x-www-form-urlencoded"
        hsRequest.ContentLength = strPostData.Length


        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPostData.ToString())
        Dim dataStream As Stream = hsRequest.GetRequestStream()

        ' Write the data to the request stream.
        dataStream.Write(byteArray, 0, byteArray.Length)

        ' Close the Stream object.
        dataStream.Close()


        Try

            ' Get the response.
            Dim hsResponse As WebResponse = hsRequest.GetResponse()
            Dim hsStatusDescription = (CType(hsResponse, HttpWebResponse).StatusDescription).ToString()
            Dim hsStatusCode = (CType(hsResponse, HttpWebResponse).StatusCode).ToString()

            ' Log the Response
            logger.RecordInLog("HubSpot Contact Submission Status Description: " & hsStatusDescription)
            logger.RecordInLog("HubSpot Contact Submission Status Code: " & hsStatusCode)

            'HubSpot sends "No Content" as a response for a successful form posting.  We will look for this response.
            If (hsStatusDescription.ToLower() = "no content") Then
                results = "OK"
            Else
                results = hsStatusDescription
            End If

            hsResponse.Close()

        Catch ex As Exception

            results = ex.ToString()
            'logger.RecordInLog("HubSpot Contact Submission ERROR: " & ex.ToString())

        End Try


        Return results


    End Function





    Public Function DateTimeToUnixTimestamp(orderDate As DateTime) As Double

        'Hubspot requires a Unix Timestamp format in MILLISECONDS.
        'http://developers.hubspot.com/docs/faq/how-should-timestamps-be-formatted-for-hubspots-apis
        'Seconds since Jan 1, 1970 = Unix Time

        Dim logger As New LogWriter()
        Dim results As Double = 0
        Dim unixStartDate As New DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)
        Dim interval As TimeSpan = (orderDate - unixStartDate.AddHours(-5)) '<-- Adjust for UTC

        'logger.RecordInLog("[HubSpot Util] Original orderDate: " & orderDate.ToString())
        'logger.RecordInLog("[HubSpot Util] Unix Start Date: " & unixStartDate.ToString())
        'logger.RecordInLog("[Hubspot Util] Milliseconds between two dates: " & interval.TotalMilliseconds.ToString())

        Return interval.TotalMilliseconds


    End Function




End Class
