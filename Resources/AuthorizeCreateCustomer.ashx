<%@ WebHandler Language="C#" Class="Authorize1" %>

using System;
using System.Web;
using CustomerProfileWS;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;




public class Authorize1 : IHttpHandler
{

    public class AuthorizeCustomer
    {
        public int CustomerID { get; set; }
        public string UserName { get; set; }
        public long ProfileID { get; set; }
        public long PaymentProfileID { get; set; }
    }

    public class AuthorizeTransaction
    {
        public string Approved { get; set; }
        public string AuthorizationCode { get; set; }
        public string CardMask { get; set; }
        public string CardType { get; set; }
        public int CustomerID { get; set; }
        public string EncryptedCCNumber { get; set; }
        public string InvoiceID { get; set; }
        public string Message { get; set; }
        public long PaymentProfileID { get; set; }
        public long ProfileID { get; set; }
        public decimal TxAmount { get; set; }
        public DateTime TxDate { get; set; }
        public string TransactionID { get; set; }
    }

    //public static CustomerProfileWS.ServiceSoapClient Service
    //{
    //    get
    //    {
    //        if (service == null)
    //        {
    //            service = new CustomerProfileWS.ServiceSoapClient();
    //            service.
    //            service.Url = API_URL;
    //        }
    //        return service;
    //    }
    //}

    public static string RetrieveAppSetting(string setting)
    {
        if(ConfigurationManager.AppSettings[setting] != null)
        {
            return ConfigurationManager.AppSettings[setting];
        }
        else
        {
            return "setting: " + setting + "NotFound";
        }
    }

    public static CustomerProfileWS.MerchantAuthenticationType MerchantAuthentication
    {
        get
        {
            if (m_auth == null)
            {
                m_auth = new CustomerProfileWS.MerchantAuthenticationType();
                m_auth.name = RetrieveMerchantName();
                m_auth.transactionKey = RetrieveTransactionKey();
            }
            return m_auth;
        }
    }

    public static string RetrieveMerchantName()
    {
        AuthorizeSettings authSettings = new AuthorizeSettings();
        authSettings = RetrieveAuthorizeSettings(Taradel.WLUtil.GetSiteId());
        return authSettings.MerchantId;
    }

    public static string RetrieveTransactionKey()
    {
        AuthorizeSettings authSettings = new AuthorizeSettings();
        authSettings = RetrieveAuthorizeSettings(Taradel.WLUtil.GetSiteId());
        return authSettings.TransactionId;
    }

    public static string RetrieveTestMode()
    {
        AuthorizeSettings authSettings = new AuthorizeSettings();
        authSettings = RetrieveAuthorizeSettings(Taradel.WLUtil.GetSiteId());
        return authSettings.TestMode;
    }


    private static CustomerProfileWS.MerchantAuthenticationType m_auth = null;
    private static CustomerProfileWS.ServiceSoapClient service = null;
    public AuthorizeCustomer customer;


    public void ProcessRequest(HttpContext context)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        //if (HttpContext.Current.Request.Form.AllKeys.Length == 0)
        //{
        //    context.Response.ContentType = "text/plain";
        //    context.Response.Write("HttpContext empty but handler compiled.");
        //    context.Response.End();
        //}
        AuthorizeSettings authSettings = new AuthorizeSettings();
        authSettings = RetrieveAuthorizeSettings(Taradel.WLUtil.GetSiteId());   //TODO: try it here but pass in if not reliable source of SiteID
        string MERCHANT_NAME = authSettings.MerchantId;  //RetrieveAppSetting("AuthNet.MerchantId");          //"9Ph4yKN48";
        string TRANSACTION_KEY = authSettings.TransactionId; //RetrieveAppSetting("AuthNet.TransactionId");    //"6c56d4592ETh5aYZ";
        string API_URL = authSettings.AuthorizeAPIUrl; //RetrieveAppSetting("AuthorizeAPIUrl");                   //"h ttps://apitest.authorize.net/soap/v1/Service.asmx";

        try
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = String.Empty;
            using (System.IO.StreamReader inputStream = new System.IO.StreamReader(HttpContext.Current.Request.InputStream))
            {
                jsonString = inputStream.ReadToEnd();
            }

            AuthorizeCustomer customer = jsonSerializer.Deserialize<AuthorizeCustomer>(jsonString);
            customer = CreateCustomerProfile(customer);
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.Write(jsonSerializer.Serialize(customer));

        }
        catch(Exception ex)
        {
            LogThis(ex.ToString());
            LogThis(ex.StackTrace.ToString());
        }

    } //end method



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


    /// <summary>
    /// Create a transaction for the customer and payment profile specified
    /// </summary>
    /// <param name="profile_id">The ID of the customer doing the transaction</param>
    /// <param name="payment_profile_id">The ID for the customers payment profile they are going to use</param>
    public AuthorizeTransaction CreateTransaction(AuthorizeTransaction tx)//long profile_id, long payment_profile_id, decimal amountOfTx
    {
        string guid = Guid.NewGuid().ToString();    //for use as invoice number
        CustomerProfileWS.ProfileTransAuthCaptureType auth_capture = new CustomerProfileWS.ProfileTransAuthCaptureType();
        auth_capture.customerProfileId = tx.ProfileID; // customer.ProfileID; // profile_id;
        auth_capture.customerPaymentProfileId = tx.PaymentProfileID;  //payment_profile_id;
        auth_capture.amount = tx.TxAmount; //amountOfTx;
        auth_capture.order = new CustomerProfileWS.OrderExType();
        auth_capture.order.invoiceNumber = guid; //????
        CustomerProfileWS.ProfileTransactionType trans = new CustomerProfileWS.ProfileTransactionType();
        trans.Item = auth_capture;
        tx.TxDate = DateTime.Now;
        using(CustomerProfileWS.ServiceSoapClient client = new ServiceSoapClient("ServiceSoap1"))
        {
            CustomerProfileWS.CreateCustomerProfileTransactionResponseType response = client.CreateCustomerProfileTransaction(MerchantAuthentication, trans, null);
            string[] responses = response.directResponse.Split(',');
            int ii = 0;

            //could also map by array ?!?!?!?!?
            foreach (string s in responses)
            {
                switch (ii)
                {
                    case 3:
                        tx.Message = s;
                        break;
                    case 4:
                        tx.AuthorizationCode = s;
                        break;
                    case 5:
                        tx.Approved = s;
                        break;
                    case 6:
                        tx.TransactionID = s;
                        break;
                    case 7:
                        tx.InvoiceID = s;
                        break;
                    case 9:
                        tx.TxAmount = Decimal.Parse(s);
                        break;
                    case 50:
                        tx.CardMask = s;
                        break;
                    case 51:
                        tx.CardType = s;
                        break;
                }
                ii++;
            }

            if (tx.Approved == "Y")
            {
                //txtResponse.Text += "Transaction Approved." + System.Environment.NewLine;
                //int result = SaveApprovedTransaction(tx);
            }

        }
        tx.Message = "Authorize";
        return tx;
    }



    public AuthorizeCustomer CreateCustomerProfile(AuthorizeCustomer customer)
    {
        long out_id = 0;
        CustomerProfileWS.CustomerProfileType m_new_cust = new CustomerProfileWS.CustomerProfileType();
        m_new_cust.email = customer.UserName;
        m_new_cust.description = customer.CustomerID.ToString();
        ValidationModeEnum testMode = new ValidationModeEnum();
        string stTestMode = RetrieveTestMode();
        //LogThis("stTestMode():" + stTestMode);
        if(stTestMode == "True")
        {
            testMode = ValidationModeEnum.none; //none will let you create a payment profile ID with no payment info
        }
        else
        {
            testMode = ValidationModeEnum.liveMode;
        }

        using (CustomerProfileWS.ServiceSoapClient client = new ServiceSoapClient("ServiceSoap1"))
        {
            LogThis("AuthorizeCreateCustomer MerchantAuthentication.name: " + MerchantAuthentication.name);
            LogThis("AuthorizeCreateCustomer MerchantAuthentication.transactionKey: " + MerchantAuthentication.transactionKey);
            LogThis("AuthorizeCreateCustomer testMode: " + testMode.ToString());
            LogThis("AuthorizeCreateCustomer m_new_cust.email: " + m_new_cust.email);
            LogThis("AuthorizeCreateCustomer m_new_cust.description: " + m_new_cust.description);

            CustomerProfileWS.CreateCustomerProfileResponseType response = client.CreateCustomerProfile(MerchantAuthentication, m_new_cust, testMode);

            if (response.resultCode.ToString() == "Error" && (response.messages[0].text.IndexOf("duplicate") > 0))
            {
                LogThis("AuthorizeCreateCustomer Message: " + response.resultCode.ToString());

                //customer = RetrieveCustomerPaymentProfile(customer);
                //response doesn't provide ProfileID, we have to scrub it our of the error response so that we can save it for next time
                //customer.ProfileID = ScrubOutProfileID(response.messages[0].text);
                //UpdateTaradelCustomer(customer);
                //txtResponse.Text += "Updating Taradel SQL: " + response.resultCode + System.Environment.NewLine;
            }

            for (int i = 0; i < response.messages.Length; i++)
            {
                LogThis("AuthorizeCreateCustomer Message: " + response.messages[i].text);
            }
            out_id = response.customerProfileId;
            customer.ProfileID = response.customerProfileId;
            if (out_id > 0)
            {
                LogThis("AuthorizeCreateCustomer - Customer profile created.  Saving to Taradel.");
                //UpdateTaradelCustomer(customer);
            }
            else
            {
                LogThis("AuthorizeCreateCustomer - Customer profile NOT created.");
            }

        }
        //txtResponse.Text += "Response Code: " + response.resultCode + System.Environment.NewLine;

        //Authorize has a record marked as a duplicate - so save that ProfileID to Taradel

        //customer = RetrieveCustomer(customer);

        return customer;
    }


    public static void LogThis(string logMessage)
    {
        string logFileName = "~\\Logs\\Auth-CreateCustomer.txt";
        string fullPath = HttpContext.Current.Server.MapPath(logFileName);

        try
        {
            using (System.IO.StreamWriter sw = System.IO.File.AppendText(fullPath))
            {
                sw.WriteLine(logMessage);
            }
        }
        catch (Exception ex)
        {
            string errorFileName = "~\\Logs\\EDDM-LogError.txt";
            string errorFullPath = HttpContext.Current.Server.MapPath(errorFileName);

            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(errorFullPath))
                {
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine(logMessage);
                }
            }
            catch(Exception ex2)
            {
                //eat it
            }
        }
    }




    public class Crypto
    {
        private static byte[] _salt = System.Text.Encoding.ASCII.GetBytes("o6806642kbM7c5");

        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        public static string EncryptStringAES(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return
            System.Security.Cryptography.RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                System.Security.Cryptography.Rfc2898DeriveBytes key = new System.Security.Cryptography.Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object
                aesAlg = new System.Security.Cryptography.RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                System.Security.Cryptography.ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (System.IO.MemoryStream msEncrypt = new System.IO.MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (System.Security.Cryptography.CryptoStream csEncrypt = new System.Security.Cryptography.CryptoStream(msEncrypt, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        using (System.IO.StreamWriter swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public static string DecryptStringAES(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            System.Security.Cryptography.RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                System.Security.Cryptography.Rfc2898DeriveBytes key = new System.Security.Cryptography.Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (System.IO.MemoryStream msDecrypt = new System.IO.MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new System.Security.Cryptography.RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    System.Security.Cryptography.ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (System.Security.Cryptography.CryptoStream csDecrypt = new System.Security.Cryptography.CryptoStream(msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        using (System.IO.StreamReader srDecrypt = new System.IO.StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        private static byte[] ReadByteArray(System.IO.Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }

    /////////////////////////////////
    //AuthorizeSettings

    public class AuthorizeSettings
    {
        private string _TestMode;
        public string TestMode {
            get { return _TestMode; }
            set { _TestMode = value; }
        }

        private string _MerchantId;
        public string MerchantId {
            get { return _MerchantId; }
            set { _MerchantId = value; }
        }

        private string _TransactionId;
        public string TransactionId {
            get { return _TransactionId; }
            set { _TransactionId = value; }
        }

        private string _CCMerchantId;
        public string CCMerchantId {
            get { return _CCMerchantId; }
            set { _CCMerchantId = value; }
        }

        private string _CCTransactionId;
        public string CCTransactionId {
            get { return _CCTransactionId; }
            set { _CCTransactionId = value; }
        }

        private string _TransactionLog;
        public string TransactionLog {
            get { return _TransactionLog; }
            set { _TransactionLog = value; }
        }

        private string _SettlementTime;
        public string SettlementTime {
            get { return _SettlementTime; }
            set { _SettlementTime = value; }
        }

        private string _AuthorizeAPIUrl;
        public string AuthorizeAPIUrl {
            get { return _AuthorizeAPIUrl; }
            set { _AuthorizeAPIUrl = value; }
        }


    }



    public static AuthorizeSettings RetrieveAuthorizeSettings(int siteID)
    {
        AuthorizeSettings authSettings = new AuthorizeSettings();

        try {
            using (SqlConnection oConnA = new SqlConnection(ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString)) {
                string sSql = "SELECT [AuthNet_TestMode],[AuthNet_MerchantID],[AuthNet_TransactionID],[AuthNet_CCMerchantID],[AuthNet_CCTransactionID],[AuthNet_TransactionLog],[AuthNet_SettlementTime],[AuthNet_AuthorizeAPIUrl] FROM [dbo].[pnd_SiteDetails] WHERE SiteID = @SiteID";
                //Integer.TryParse(Taradel.WLUtil.GetSiteId, siteID)  'EDDM SiteID by default.
                using (SqlCommand oCmdA = new SqlCommand(sSql, oConnA)) {
                    //oCmdA.CommandType = CommandType.StoredProcedure;
                    oCmdA.Parameters.AddWithValue("@SiteID", siteID);
                    oConnA.Open();
                    using (SqlDataReader RDR = oCmdA.ExecuteReader()) {
                        while ((RDR.Read())) {
                            authSettings.TestMode = RDR["AuthNet_TestMode"].ToString();
                            authSettings.MerchantId = RDR["AuthNet_MerchantID"].ToString();
                            authSettings.TransactionId = RDR["AuthNet_TransactionID"].ToString();
                            authSettings.CCMerchantId = RDR["AuthNet_CCMerchantID"].ToString();
                            authSettings.CCTransactionId = RDR["AuthNet_CCTransactionID"].ToString();
                            authSettings.TransactionLog = RDR["AuthNet_TransactionLog"].ToString();
                            authSettings.SettlementTime = RDR["AuthNet_SettlementTime"].ToString();
                            authSettings.AuthorizeAPIUrl = RDR["AuthNet_AuthorizeAPIUrl"].ToString();
                        }
                    }
                }
                //end command
            }
            //end SqlConnection
            //LogThis("RetrieveAuthorizeSettings success with SiteID: " + siteID.ToString());
        } catch (Exception ex) {
            //LogThis("RetrieveAuthorizeSettings :" + ex.StackTrace.ToString());
            //LogThis("RetrieveAuthorizeSettings :" + ex.Message.ToString());
        }


        return authSettings;

    }

}