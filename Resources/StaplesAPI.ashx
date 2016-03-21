<%@ WebHandler Language="C#" Class="StaplesAPI" %>

using System;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;




public class StaplesAPI : IHttpHandler
{

    public static string RetrieveAppSetting(string setting)
    {
        if (ConfigurationManager.AppSettings[setting] != null)
        {
            return ConfigurationManager.AppSettings[setting];
        }
        else
        {
            return "setting: " + setting + "NotFound";
        }
    }





    // TODO: Specify merchant account name
    //<add key="AuthNet.MerchantId" value="9Ph4yKN48"/>
    //<add key="AuthNet.TransactionId" value="6c56d4592ETh5aYZ"/>
    public static String MERCHANT_NAME = RetrieveAppSetting("AuthNet.MerchantId");          //"9Ph4yKN48";
    public static String TRANSACTION_KEY = RetrieveAppSetting("AuthNet.TransactionId");    //"6c56d4592ETh5aYZ";
    public static String API_URL = RetrieveAppSetting("AuthorizeAPIUrl");                   //"h ttps://apitest.authorize.net/soap/v1/Service.asmx";

    public class RootObject
    {
        public string WCToken { get; set; }
        public string WCTrustedToken { get; set; }
        public string personalizationID { get; set; }
        public string userId { get; set; }
    }

    /// <summary>
    public class Address
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string addressId { get; set; }
        public string city { get; set; }
        public string companyName { get; set; }
        public string extensionNumber { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone1 { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
    }

    public class Member
    {
        public List<Address> address { get; set; }
    }

    public class RootObject2
    {
        public List<Member> Member { get; set; }
    }
    /// </summary>
    /// <param name="context"></param>



    public void ProcessRequest(HttpContext context)
    {
        //context.Response.Write("this is a test of the context");
        //context.Response.Write("context.Request.Form.AllKeys.Length " + context.Request.Form.AllKeys.Length);

        string url = "https://api.staples.com/v1/10001/loginidentity";
        string logonID = string.Empty;
        string logonPassword = string.Empty;

        if (context.Request.Form.AllKeys.Length > 0)
        {
            foreach (string key in HttpContext.Current.Request.Form.AllKeys)
            {
                string value = HttpContext.Current.Request.Form[key];
                //context.Response.Write("key:" + key + " : " + value);
                if(key == "logonId")
                {
                    logonID = value;
                }

                if(key == "logonPassword")
                {
                    logonPassword = value;
                }
            }
        }

        //context.Response.End();
        string responseFromServer = string.Empty;
        string json = string.Empty;

        try
        {
            WebRequest request = WebRequest.Create(url); //"h ttp://api.openweathermap.org/data/2.5/weather?q=23060,us"
            request.Headers.Add("client_id", "8DYip4nmjuS7xn39atm0HjlG4CEORtaj");
            request.Method = "POST";
            request.ContentType = "application/json"; //"application/x-www-form-urlencoded";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                //string json = "{\"logonId\":\"gatortarheel@yahoo.com\"," +
                //              "\"logonPassword\":\"password1\"," +
                //              "\"storeId\":\"10001\"}";
                json = "{\"logonId\":\"" + logonID + "\"," +
                              "\"logonPassword\":\"" + logonPassword + "\"," +
                              "\"storeId\":\"10001\"}";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }


            //add this JSON to post
            //{ 
            //    "logonId" : "rzsmith@hotmail.com",
            //    "logonPassword" : "password", 
            //    "storeId" : "10001"
            //}
            Stream dataStream = request.GetRequestStream();
            WebResponse response = request.GetResponse();
            //
            //can't serialize--context.Response.Write("StatusDescription: " + ((HttpWebResponse)response).StatusDescription + System.Environment.NewLine);
            dataStream = response.GetResponseStream();

            using (StreamReader reader = new StreamReader(dataStream))
            {
                responseFromServer = reader.ReadToEnd();
                reader.Close();
            }
            // Clean up the streams.
            dataStream.Close();
            response.Close();
        }
        catch(Exception ex)
        {
            context.Response.Write("ERROR WITH json " + json);
            context.Response.Write(ex.ToString());
            return;
        }


        /////////////////////////////////////////////
        RootObject theRoot = new RootObject();
        try
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = responseFromServer;
            //context.Response.Clear();
            theRoot = jsonSerializer.Deserialize<RootObject>(jsonString);
            context.Response.ContentType = "application/json";
            //context.Response.Write("result" + System.Environment.NewLine);
            //context.Response.Write("theRoot.WCToken:" + theRoot.WCToken);
            //context.Response.Write(System.Environment.NewLine);
            //context.Response.Write("theRoot.WCTrustedToken:" + theRoot.WCTrustedToken);
            //context.Response.Write(System.Environment.NewLine);
            //context.Response.Write("theRoot.personalizationID:" + theRoot.personalizationID);
            //context.Response.Write(System.Environment.NewLine);
            //context.Response.Write("theRoot.userId:" + theRoot.userId);

        }
        catch (Exception ex)
        {
            context.Response.Write("Error with Staples LogIn");
            //transaction.Message = ex.ToString();
            context.Response.Write(ex.ToString());
            context.Response.End();
        }
        finally
        {
            //FOR DEBUG --> context.Response.Write("compiled ok");
        }


        ///now to retrieve the address
        try
        {
            RootObject2 theRoot2 = new RootObject2();
            //context.Response.Write(System.Environment.NewLine);
            //context.Response.Write("now to retrieve the address");
            //context.Response.Write(System.Environment.NewLine);
            string urlGet = "https://api.staples.com/v1/10001/member/profile/address?locale=en_US&client_id=8DYip4nmjuS7xn39atm0HjlG4CEORtaj";
            HttpWebRequest requestGet = (HttpWebRequest)WebRequest.Create(urlGet); //"h ttp://api.openweathermap.org/data/2.5/weather?q=23060,us"
            requestGet.Headers.Add("client_id","8DYip4nmjuS7xn39atm0HjlG4CEORtaj");
            requestGet.Headers.Add("WCToken", theRoot.WCToken);
            requestGet.Headers.Add("WCTrustedToken", theRoot.WCTrustedToken);
            requestGet.Method = "GET";
            HttpWebResponse res = (HttpWebResponse)requestGet.GetResponse();
            Stream sr = res.GetResponseStream();
            StreamReader sre = new StreamReader(sr);
            string s = sre.ReadToEnd();
            context.Response.Write(s);
        }
        catch(Exception ex)
        {
            context.Response.Write("STAPLES USER WITH NO STAPLES ADDRESS"); //keep this constant
            //context.Response.Write(ex.ToString());
            return;
        }
        //System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer2 = new System.Web.Script.Serialization.JavaScriptSerializer();
        //theRoot2 = jsonSerializer2.Deserialize<RootObject2>(s);
        //List<Member> lstMembers = theRoot2.Member;
        //foreach(Member m in lstMembers)
        //{
        //    foreach(Address address in m.address)
        //    {
        //        //context.Response.Write(System.Environment.NewLine);
        //        //context.Response.Write(address.address1);
        //    }
        //}




    } //end method





    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}


//    #region safeCode
//public void ProcessRequest_safe(HttpContext context)
//{
//    //HttpRequest request = context.Request;
//    //HttpResponse response = context.Response;
//    object o = new object();
//    ////////////////////////////////////////////
//    //9/8/2015 --  Create a request using a URL that can receive a post. 
//    string url = "https://sapi.staples.com/v1/10001/guestidentity";
//    WebRequest request = WebRequest.Create(url); //"h ttp://api.openweathermap.org/data/2.5/weather?q=23060,us"
//    NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
//    request.Headers.Add("client_id","k8aM4itF5n8JrSnLrl5iSKOQyTtWe5f8");
//    outgoingQueryString.Add("","");
//    //string postData = outgoingQueryString.ToString();
//    // Set the Method property of the request to POST.
//    request.Method = "POST";
//    // Create POST data and convert it to a byte array.
//    //string postData = "This is a test that posts this string to a Web server.";
//    //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
//    // Set the ContentType property of the WebRequest.
//    request.ContentType = "application/json"; //"application/x-www-form-urlencoded";
//    // Set the ContentLength property of the WebRequest.
//    //request.ContentLength = byteArray.Length;
//    // Get the request stream.
//    Stream dataStream = request.GetRequestStream();
//    // Write the data to the request stream.
//    //dataStream.Write(byteArray, 0, byteArray.Length);
//    // Close the Stream object.
//    dataStream.Close();
//    // Get the response.
//    WebResponse response = request.GetResponse();
//    // Display the status.
//    context.Response.Write("StatusDescription: " + ((HttpWebResponse)response).StatusDescription + System.Environment.NewLine);
//    // Get the stream containing content returned by the server.
//    dataStream = response.GetResponseStream();
//    // Open the stream using a StreamReader for easy access.
//    StreamReader reader = new StreamReader(dataStream);
//    // Read the content.
//    string responseFromServer = reader.ReadToEnd();
//    // Display the content.
//    //context.Response.Write("responseFromServer");
//    //context.Response.Write(System.Environment.NewLine);

//    //context.Response.Write(responseFromServer);
//    // Clean up the streams.
//    reader.Close();
//    dataStream.Close();
//    response.Close();

//    /////////////////////////////////////////////
//    try
//    {
//        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
//        string jsonString = responseFromServer;
//        //context.Response.Clear();
//        RootObject theRoot = jsonSerializer.Deserialize<RootObject>(jsonString);

//        context.Response.ContentType = "application/json";
//        context.Response.Write("result" + System.Environment.NewLine);
//        context.Response.Write("theRoot.WCToken:" + theRoot.WCToken);
//        context.Response.Write(System.Environment.NewLine);
//        context.Response.Write("theRoot.WCTrustedToken:" + theRoot.WCTrustedToken);
//        context.Response.Write(System.Environment.NewLine);
//        context.Response.Write("theRoot.personalizationID:" + theRoot.personalizationID);
//        context.Response.Write(System.Environment.NewLine);
//        context.Response.Write("theRoot.userId:" + theRoot.userId);

//    }
//    catch (Exception ex)
//    {
//        //transaction.Message = ex.ToString();
//        context.Response.Write(ex.ToString());
//    }
//    finally
//    {
//        //FOR DEBUG --> context.Response.Write("compiled ok");
//    }

//} //end method

//public void ProcessRequest_safePROD(HttpContext context)
//{
//    string url = "https://api.staples.com/v1/10001/guestidentity";
//    WebRequest request = WebRequest.Create(url); //"h ttp://api.openweathermap.org/data/2.5/weather?q=23060,us"
//    NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
//    request.Headers.Add("client_id","8DYip4nmjuS7xn39atm0HjlG4CEORtaj");
//    outgoingQueryString.Add("","");
//    //string postData = outgoingQueryString.ToString();
//    // Set the Method property of the request to POST.
//    request.Method = "POST";
//    // Create POST data and convert it to a byte array.
//    //string postData = "This is a test that posts this string to a Web server.";
//    //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
//    // Set the ContentType property of the WebRequest.
//    request.ContentType = "application/json"; //"application/x-www-form-urlencoded";
//    // Set the ContentLength property of the WebRequest.
//    //request.ContentLength = byteArray.Length;
//    // Get the request stream.
//    Stream dataStream = request.GetRequestStream();
//    // Write the data to the request stream.
//    //dataStream.Write(byteArray, 0, byteArray.Length);
//    // Close the Stream object.
//    dataStream.Close();
//    // Get the response.
//    WebResponse response = request.GetResponse();
//    // Display the status.
//    context.Response.Write("StatusDescription: " + ((HttpWebResponse)response).StatusDescription + System.Environment.NewLine);
//    // Get the stream containing content returned by the server.
//    dataStream = response.GetResponseStream();
//    // Open the stream using a StreamReader for easy access.
//    StreamReader reader = new StreamReader(dataStream);
//    // Read the content.
//    string responseFromServer = reader.ReadToEnd();
//    // Display the content.
//    //context.Response.Write("responseFromServer");
//    //context.Response.Write(System.Environment.NewLine);

//    //context.Response.Write(responseFromServer);
//    // Clean up the streams.
//    reader.Close();
//    dataStream.Close();
//    response.Close();

//    /////////////////////////////////////////////
//    try
//    {
//        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
//        string jsonString = responseFromServer;
//        //context.Response.Clear();
//        RootObject theRoot = jsonSerializer.Deserialize<RootObject>(jsonString);

//        context.Response.ContentType = "application/json";
//        context.Response.Write("result" + System.Environment.NewLine);
//        context.Response.Write("theRoot.WCToken:" + theRoot.WCToken);
//        context.Response.Write(System.Environment.NewLine);
//        context.Response.Write("theRoot.WCTrustedToken:" + theRoot.WCTrustedToken);
//        context.Response.Write(System.Environment.NewLine);
//        context.Response.Write("theRoot.personalizationID:" + theRoot.personalizationID);
//        context.Response.Write(System.Environment.NewLine);
//        context.Response.Write("theRoot.userId:" + theRoot.userId);

//    }
//    catch (Exception ex)
//    {
//        //transaction.Message = ex.ToString();
//        context.Response.Write(ex.ToString());
//    }
//    finally
//    {
//        //FOR DEBUG --> context.Response.Write("compiled ok");
//    }

//} //end method
//#endregion
