<%@ WebHandler Language="C#" Class="StaplesAPI_SendPOS" %>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Xml.XPath;
using System.Configuration;
using System.Net.Sockets;




public class StaplesAPI_SendPOS : IHttpHandler
{
    public bool debug;
    public bool submit;
    public string url;
    public string environment;





    public void ProcessRequest(HttpContext context)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        url = "https://scgtw.test2.staplesb2b.com/ws/SplsB2BTransactionSalesOrder.EFI.inbound.wsd:SO_inbound_request";         //TODO: make configurable prior to production
        environment = "DEV";  //TODO: make configurable
        int siteID = Taradel.WLUtil.GetSiteId();
        if (siteID != 91)
        {
            LogThis("Attempt to access STAPLES POS FROM non-Staples store site.  All Stop!");
            context.Response.Write("siteID :" + siteID.ToString() + System.Environment.NewLine);
            context.Response.Write("Attempt to access STAPLES POS FROM non-Staples store site.  All Stop!");
            context.Response.End();
        }





        LogThis("-------------------------------------------------------------------------------------------");
        LogThis("Process hit at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

        try
        {
            Uri theRealURL = new Uri(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.RawUrl);
            //ontext.Response.Write(System.Environment.NewLine + "TheRealURL:" + theRealURL + System.Environment.NewLine);
            string OrderID = HttpUtility.ParseQueryString(theRealURL.Query).Get("OrderID");
            string Debug = HttpUtility.ParseQueryString(theRealURL.Query).Get("Debug");
            string Submit = HttpUtility.ParseQueryString(theRealURL.Query).Get("Submit");
            string StaplesOrderID = RetrieveStaplesOrderNumber(OrderID);//"6860000974"; //TODO: make sequential


            //context.Response.Write("Debug:" + Debug + System.Environment.NewLine);
            debug = false;
            submit = false;

            bool.TryParse(Debug, out debug);
            bool.TryParse(Submit, out submit);
            context.Response.Write("debug (bool):" + debug.ToString() + System.Environment.NewLine);
            context.Response.Write("submit (bool):" + submit.ToString() + System.Environment.NewLine);
            context.Response.Write("siteID :" + siteID.ToString() + System.Environment.NewLine);

            context.Response.Write("OrderID:" + OrderID + System.Environment.NewLine);
            LogThis("Processing Taradel OrderID: " + OrderID);
            StaplesOrderInfo staplesInfo = RetrieveTheOrderDetails(OrderID, StaplesOrderID);
            //InsertStaplesPOS(staplesInfo);
            string transformedPath = string.Empty;

            if(debug)
            {
                System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                context.Response.ContentType = "application/json";
                context.Response.Write(jsonSerializer.Serialize(staplesInfo));
                string logFileName = "~\\Logs\\StaplesXml\\SendPOSLog.txt";
                string fullPath = HttpContext.Current.Server.MapPath(logFileName);
                context.Response.Write(System.Environment.NewLine);
                context.Response.Write(fullPath);
                context.Response.Write(System.Environment.NewLine);
                transformedPath = TransformTheXML(staplesInfo);
                context.Response.Write(System.Environment.NewLine);
                context.Response.Write("File saved at: " + transformedPath);
                context.Response.Write(System.Environment.NewLine);
                UpdateStaplesPOS(staplesInfo);
            }

            if (submit)
            {
                bool success = SendTheXMLToStaples(transformedPath);
                if(success)
                {
                    LogThis(transformedPath + " transferred to Staples POS ");
                    //for next phase --> UpdateStaplesPOS(staplesInfo);
                }
                else
                {
                    LogThis(transformedPath + " failed to transfer to Staples POS ");
                }
            }

            //if (HttpContext.Current.Request.Form.AllKeys.Length == 0)
            //{
            //    context.Response.ContentType = "text/plain";
            //    context.Response.Write("HttpContext.Current.Request.Form empty but handler compiled.");
            //}
        }
        catch(Exception ex)
        {
            context.Response.Write(ex.ToString());
            context.Response.Write(ex.StackTrace.ToString());
        }

    } //end ProcessRequest method



    public string RetrieveStaplesOrderNumber(string OrderID)
    {
        // string connectString = ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString;
        //use handler
        StringBuilder results = new StringBuilder();
        WebRequest request = WebRequest.Create("http://staplesstore.redesign.eddmsite.com/Resources/StaplesOrderNumberRetrieve.ashx?OrderID=" + OrderID);
        request.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = request.GetResponse();
        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        results.Append(reader.ReadToEnd());
        reader.Close();
        response.Close();
        string returnThis = results.ToString();
        LogThis("RetrieveStaplesOrderNumber:" + returnThis);

        //string sql ="usp_RetrieveStaplesOrderNumber";
        //string returnThis = "0";
        //try
        //{
        //    using (SqlConnection conn = new SqlConnection(connectString))
        //    {
        //        SqlCommand command = new SqlCommand(sql, conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = sql;
        //        command.Parameters.Add(new SqlParameter("TaradelOrder_ID", OrderID));
        //        conn.Open();
        //        using (SqlDataReader rdr = command.ExecuteReader())
        //        {
        //            while (rdr.Read())
        //            {
        //                returnThis = rdr[0].ToString();
        //            }
        //        }
        //        conn.Close();
        //    }
        //}
        //catch(Exception ex)
        //{
        //    HttpContext.Current.Response.ContentType = "text/html";
        //    HttpContext.Current.Response.Write(ex.ToString());
        //}
        return returnThis;
    }


    //deprecated
    public string InsertStaplesPOS(StaplesOrderInfo orderInfo)
    {
        string connectString = ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString;
        string sql = " INSERT INTO StaplesPOS (StaplesOrder_ID, TaradelOrder_ID) VALUES (" + orderInfo.StaplesOrderID.ToString() + "," + orderInfo.OrderID + ")";
        string returnThis = "0";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                conn.Open();
                returnThis = command.ExecuteNonQuery().ToString();
                conn.Close();
            }
        }
        catch(Exception ex)
        {
            LogThis(sql);
            LogThis(ex.ToString());
        }
        return returnThis;
    }


    public string UpdateStaplesPOS(StaplesOrderInfo orderInfo)
    {
        string connectString = ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString;
        string sql = " UPDATE StaplesPOS SET TaradelOrder_ID = '" + orderInfo.OrderID + "', DateSent = GetDate() WHERE StaplesOrder_ID = " + orderInfo.StaplesOrderID;
        string returnThis = "0";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                conn.Open();
                returnThis = command.ExecuteNonQuery().ToString();
                conn.Close();
            }
        }
        catch(Exception ex)
        {
            LogThis(sql);
            LogThis(ex.ToString());
        }
        return returnThis;
    }

    private static IPEndPoint BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
    {
        var localIpAddress = IPAddress.Parse("204.186.24.30"); //TODO: configurable
        var hasLocalAddress =
            Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.Any(
                    oIp => oIp.AddressFamily == AddressFamily.InterNetwork && oIp.Equals(localIpAddress));
        //HttpContext.Current.Response.Write("Has Local Address: " + hasLocalAddress + "<br/>");

        return new IPEndPoint(hasLocalAddress ? localIpAddress : IPAddress.Any, 0);
    }

    public bool SendTheXMLToStaples(string fileName)
    {
        //string fileName = @"C:\Users\rsmith\Documents\Visual Studio 2015\Projects\DataIris\DataIris\SampleStaplesOrderEDDM2_submitTest.xml";
        //string fileName = Server.MapPath("SampleStaplesOrderEDDM2_submitTest2.xml");
        //Response.Write("fileName:" + fileName + "<br/>");
        //string ip = getExternalIp(); // Request.UserHostAddress.ToString();
        //Response.Write("IP:" + ip + "<br/>");
        bool returnThis = false;

        try
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XDocument xdoc = XDocument.Load(fileName);
            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            xdoc.Save(fileName);
            string xmlString = xdoc.ToString();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);  //the original file here 
            using (var stringWriter = new StringWriter())
            {
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xmlDoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    stringWriter.GetStringBuilder().ToString();
                }
                Uri uri = new Uri(url);
                ServicePoint sp = ServicePointManager.FindServicePoint(uri);
                sp.BindIPEndPointDelegate -= BindIPEndPointCallback; // avoid duplicate calls to Bind
                sp.BindIPEndPointDelegate += BindIPEndPointCallback;

                HttpWebRequest req = (HttpWebRequest) WebRequest.Create(uri);
                req.Method = "POST";
                req.Proxy = null;
                req.KeepAlive = false;
                req.ProtocolVersion = HttpVersion.Version10;
                req.UserAgent = "TaradelAPICaller1.0";
                LogThis("ServicePoint Address: " + req.ServicePoint.Address.ToString() + "<br/>");

                NetworkCredential creds = new NetworkCredential("taradeltest", "t@r@de1T$t");
                //TODO: make configurable
                req.Credentials = creds;
                req.PreAuthenticate = true;

                byte[] postData = System.Text.Encoding.ASCII.GetBytes(stringWriter.ToString());
                req.ContentType = "text/xml; encoding='utf-8'";
                req.ContentLength = postData.Length;

                Stream requestStream = req.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                requestStream.Close();

                var oResp = (HttpWebResponse)req.GetResponse();
                LogThis("Response Status Code: " + oResp.StatusCode + "<br/>");

                StreamReader streamIn = new StreamReader(oResp.GetResponseStream());
                string strResponse = streamIn.ReadToEnd();
                LogThis(strResponse);
                streamIn.Close();
                returnThis = true;
            }
        }
        catch (WebException webex)
        {
            LogThis("<p>" + webex.ToString() + "</p>");
            var resp = new StreamReader(webex.Response.GetResponseStream()).ReadToEnd();
            LogThis("<p>" + resp + "</p>");

        }
        catch (Exception ex)
        {
            LogThis(ex.ToString());
        }
        return returnThis;
    }


    private void LogThis(string logMessage)
    {
        string logFileName = "~\\Logs\\StaplesXml\\SendPOSLog.txt";
        string fullPath = HttpContext.Current.Server.MapPath(logFileName);

        if(File.Exists(fullPath))
        {
            var FileSize = Convert.ToDecimal((new System.IO.FileInfo(fullPath)).Length);
            if (FileSize > 50000)
            {
                File.Delete(fullPath);
            }
        }


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
    //using the approved Staples XML as a staring point, create a new XML document for this order
    //there is probably a more efficient way to manage this, like through XLST but Staples wants
    //this done yesterday
    //returns the fileName which is then submitted to Staples
    public string TransformTheXML(StaplesOrderInfo orderInfo)
    {
        //string fileName = @"C:\Users\rsmith\Documents\Visual Studio 2015\Projects\DataIris\DataIris\SampleStaplesOrderEDDM2_submitTest2.xml";

        string staplesXML = "~\\Logs\\StaplesXml\\SampleStaplesOrderEDDM2_submitTest2.xml";
        string StaplesTemplateXMLPath = HttpContext.Current.Server.MapPath(staplesXML);

        string StaplesTemplateXMLAltered =  "~\\Logs\\StaplesXml\\SampleStaplesOrderEDDM_Altered" + orderInfo.OrderID + ".xml";
        string StaplesTemplateXMLAlteredPath = HttpContext.Current.Server.MapPath(StaplesTemplateXMLAltered);

        XmlDocument doc = new XmlDocument();
        doc.Load(StaplesTemplateXMLPath);
        XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
        XmlNode rootNode = doc.DocumentElement;
        manager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
        manager.AddNamespace("v2", "http://schemas.staples.com/esb/salesorder/create/V2_0");
        XmlNodeList aNodes = doc.SelectNodes("/soapenv:Envelope/soapenv:Body/SalesOrderType/v2:CreateSalesOrder", manager);
        XDocument xdoc = XDocument.Load(StaplesTemplateXMLPath);

        //	<SOAP-ENV:Header><ns:SplsTID xmlns:ns="schema.staples.com/eai/canonicals/SOTID2"><ns:Custparm>6860000974</ns:Custparm>
        XNamespace nsSoap2 = "http://schemas.xmlsoap.org/soap/envelope/";
        XNamespace v22 = "http://schemas.staples.com/esb/salesorder/create/V2_0";

        LogThis("Attempting new code");
        try
        {
            XElement xEnvelope2 = xdoc.Element(nsSoap2 + "Envelope");  //worked
            XElement xHeader2 = xEnvelope2.Element(nsSoap2 + "Header");    //worked
            foreach(var element in xHeader2.Elements())
            {
                foreach (var element2 in element.Elements())
                {
                    //LogThis("element2 LocalName: " + element2.Name.LocalName + ":" + element2.Value);
                    //if(element2.Name.)
                    if(element2.Name.LocalName == "Custparm")
                    {
                        element2.Value = orderInfo.NMB;
                    }
                    //LogThis("--------------------------------------------------");
                }
                //if(element.Name == "ns:Custparm")
                //{
                //    element.Value = orderInfo.NMB;
                //}
            }
            //            XElement xCustParm = xHeader2.Element(nsSoap2 + "SplsTID");

            //XElement xCustParm2 = xCustParm.Element("Custparm");
            //xCustParm2.Value = orderInfo.NMB;

        }
        catch(Exception ex)
        {
            LogThis("ex from new Staples NMB code");
            LogThis(ex.StackTrace.ToString());
            LogThis(ex.ToString());
        }




        XNamespace nsSoap = "http://schemas.xmlsoap.org/soap/envelope/";
        XNamespace v2 = "http://schemas.staples.com/esb/salesorder/create/V2_0";
        XElement xEnvelope = xdoc.Element(nsSoap + "Envelope");  //worked
        XElement xBody = xEnvelope.Element(nsSoap + "Body");    //worked

        XElement xSalesOrderType = xBody.Element("SalesOrderType");
        XElement xCreateSalesOrder = xSalesOrderType.Element(v2 + "CreateSalesOrder");
        XElement xHeader = xCreateSalesOrder.Element("Header");





        XElement xNmb = xHeader.Element("Nmb");

        //show full query then shortened query
        //XElement xNmb1 = xdoc.Element(nsSoap + "Envelope").Element(nsSoap + "Body").Element("SalesOrderType").Element(v2 + "CreateSalesOrder").Element("Header").Element("Nmb");
        XElement xNmb1 = xHeader.Element("Nmb");
        xNmb1.Value = orderInfo.NMB;

        //XElement xNmb2 = xdoc.Element(nsSoap + "Envelope").Element(nsSoap + "Body").Element("SalesOrderType").Element(v2 + "CreateSalesOrder").Element("Header").Element("PurchaseOrderList").Element("Customer").Element("Nmb");
        XElement xNmb2 = xHeader.Element("PurchaseOrderList").Element("Customer").Element("Nmb");
        xNmb2.Value = orderInfo.NMB;

        //to change attribute, get the element first
        XElement xNmb3 = xHeader.Element("PurchaseOrderList").Element("Customer").Element("Id");
        xNmb3.Attribute("Original").Value = orderInfo.NMB;

        XElement xNmb4 = xHeader.Element("PurchaseOrderList").Element("PurchaseOrder").Element("Id");
        xNmb4.Attribute("Original").Value = orderInfo.NMB;

        XElement xNmb5 = xHeader.Element("PurchaseOrderList").Element("PurchaseOrder").Element("Nmb");
        xNmb5.Value = orderInfo.NMB;


        //Store Number
        XElement xStoreNumber = xHeader.Element("Retail").Element("Store").Element("Nmb");
        xStoreNumber.Value = orderInfo.StaplesStoreNumber;

        //Bill To Company Info
        XElement xBillToCompanyName = xHeader.Element("PartyList").Element("BillTo").Element("OrganizationList").Element("Company").Element("Name");
        xBillToCompanyName.Value = orderInfo.billingCustomer.CompanyName;

        XElement xBillToFirstName = xHeader.Element("PartyList").Element("BillTo").Element("NameList").Element("Name").Element("FirstName");
        xBillToFirstName.Value = orderInfo.billingCustomer.FirstName;

        XElement xBillToLastName = xHeader.Element("PartyList").Element("BillTo").Element("NameList").Element("Name").Element("LastName");
        xBillToLastName.Value = orderInfo.billingCustomer.LastName;

        XElement xBillToPhoneNumber = xHeader.Element("PartyList").Element("BillTo").Element("PhoneList").Element("Day").Element("Number");
        xBillToPhoneNumber.Value = orderInfo.billingCustomer.Phone;

        XElement xAddressLine1 = xHeader.Element("PartyList").Element("BillTo").Element("PostalAddress").Element("AddressLine1");
        xAddressLine1.Value = orderInfo.billingCustomer.Address1;

        XElement xAddressCity = xHeader.Element("PartyList").Element("BillTo").Element("PostalAddress").Element("City");
        xAddressCity.Value = orderInfo.billingCustomer.City;

        XElement xAddressPostalCode = xHeader.Element("PartyList").Element("BillTo").Element("PostalAddress").Element("PostalCode");
        xAddressPostalCode.Value = orderInfo.billingCustomer.Zip;

        XElement xAddressState = xHeader.Element("PartyList").Element("BillTo").Element("PostalAddress").Element("State").Element("Code");
        xAddressState.Value = orderInfo.billingCustomer.State;

        XElement xCustomerEmail = xHeader.Element("PartyList").Element("Primary").Element("EmailList").Element("Primary").Element("Address");
        xCustomerEmail.Value = orderInfo.billingCustomer.Email;

        /////ship to ///////

        XElement xShipToCompanyName = xHeader.Element("PartyList").Element("ShipTo").Element("OrganizationList").Element("Company").Element("Name");
        xShipToCompanyName.Value = orderInfo.shippingCustomer.CompanyName;

        XElement xShipToFirstName = xHeader.Element("PartyList").Element("ShipTo").Element("NameList").Element("Name").Element("FirstName");
        xShipToFirstName.Value = orderInfo.shippingCustomer.FirstName;// + " [Shipto]"; // Clark";

        XElement xShipToLastName = xHeader.Element("PartyList").Element("ShipTo").Element("NameList").Element("Name").Element("LastName");
        xShipToLastName.Value = orderInfo.shippingCustomer.LastName; // + " [Shipto]"; // Clark";"Kent";

        XElement xShipToPhoneNumber = xHeader.Element("PartyList").Element("ShipTo").Element("PhoneList").Element("Day").Element("Number");
        xShipToPhoneNumber.Value = orderInfo.shippingCustomer.Phone; // "8881234567";

        XElement xShipAddressLine1 = xHeader.Element("PartyList").Element("ShipTo").Element("PostalAddress").Element("AddressLine1");
        xShipAddressLine1.Value = orderInfo.shippingCustomer.Address1; // + " [Shipto]"; // Clark";"Kent"; "1 Justice Way";

        XElement xShipAddressLine2 = xHeader.Element("PartyList").Element("ShipTo").Element("PostalAddress").Element("AddressLine2");
        xShipAddressLine2.Value = string.Empty; // orderInfo.billingCustomer.Address2  //"Suite K";

        XElement xShipAddressCity = xHeader.Element("PartyList").Element("ShipTo").Element("PostalAddress").Element("City");
        xShipAddressCity.Value = orderInfo.shippingCustomer.City;// + " [Shipto]"; //"Metropolis";

        XElement xShipAddressZip = xHeader.Element("PartyList").Element("ShipTo").Element("PostalAddress").Element("PostalCode");
        xShipAddressZip.Value = orderInfo.shippingCustomer.Zip; // orderInfo.billingCustomer.Address2  //"Suite K";

        XElement xShipAddressState = xHeader.Element("PartyList").Element("ShipTo").Element("PostalAddress").Element("State").Element("Code");
        xShipAddressState.Value = orderInfo.shippingCustomer.State; //"NY";

        XElement xShipCustomerEmail = xHeader.Element("PartyList").Element("Primary").Element("EmailList").Element("Primary").Element("Address");
        xShipCustomerEmail.Value = orderInfo.shippingCustomer.Email; //"clark.kent@justice.org";


        //End Ship to

        //Standard amount for a sub total after any coupons or discounts are applied
        //Tax is not included
        XElement xSubtotal = xHeader.Element("TotalList").Element("Cart").Element("AmountList").Element("SubTotal");
        xSubtotal.Attribute("Amount").Value = orderInfo.Amount;

        XElement xOverallTotal = xHeader.Element("TotalList").Element("Overall").Element("Amount");
        xOverallTotal.Attribute("Amount").Value = orderInfo.Amount;


        //SKU
        XElement xSKU = xCreateSalesOrder.Element("Lines").Element("Line").Element("ItemList").Element("Ordered").Element("Nmb");
        xSKU.Value = orderInfo.StaplesSKU;

        XElement xSKUQuantity = xCreateSalesOrder.Element("Lines").Element("Line").Element("ItemList").Element("Ordered").Element("QuantityList").Element("Additional").Element("Quantity").Element("Quantity");
        xSKUQuantity.Value = orderInfo.Quantity;

        XElement xSKUQuantity2 = xCreateSalesOrder.Element("Lines").Element("Line").Element("ItemList").Element("Ordered").Element("PriceList").Element("Unit").Element("Quantity");
        xSKUQuantity2.Value = orderInfo.CalculatedQuantity;

        //Price
        //Net price after any coupons or discounts are applied.
        //Tax is not included
        XElement xPrice = xCreateSalesOrder.Element("Lines").Element("Line").Element("ItemList").Element("Ordered").Element("PriceList").Element("Unit").Element("Amount");
        xPrice.Attribute("Amount").Value = orderInfo.CalculatedAmount;//Price.ToString();

        //Description
        XElement xDescription = xCreateSalesOrder.Element("Lines").Element("Line").Element("ItemList").Element("Ordered").Element("Description");
        xDescription.Value = orderInfo.ItemDescription;

        //Another Price
        XElement xPrice2 = xCreateSalesOrder.Element("Lines").Element("Line").Element("ItemList").Element("Ordered").Element("PriceList").Element("List").Element("Amount");
        xPrice2.Attribute("Amount").Value = orderInfo.CalculatedAmount;

        XElement xExtendedSellPrice = xCreateSalesOrder.Element("Lines").Element("Line").Element("ItemList").Element("Ordered").Element("PriceList").Element("Extended").Element("Amount");
        xExtendedSellPrice.Attribute("Amount").Value = orderInfo.CalculatedAmount;

        //Order Creation Date
        XElement xOrderCreationDate = xCreateSalesOrder.Element("Header").Element("DateList").Element("Create").Element("Date");
        XElement xOrderCreationDate2 = xCreateSalesOrder.Element("Header").Element("DatetimeList").Element("Create").Element("DateTime");
        xOrderCreationDate.Value = orderInfo.CreationDate;
        xOrderCreationDate2.Value = orderInfo.CreationDate;
        //Order Due Date - Date requested by the customer
        XElement xOrderDueDate = xCreateSalesOrder.Element("Header").Element("DateList").Element("RequestedDelivery").Element("Date");
        xOrderDueDate.Value = orderInfo.DueDate;

        //Expected Delivery Date 
        XElement xOrderExpectedDeliveryDate = xCreateSalesOrder.Element("Header").Element("DateList").Element("ExpectedDelivery").Element("Date");
        xOrderExpectedDeliveryDate.Value = orderInfo.DueDate;

        //DatetimeList/Create/DateTime
        XElement xOrderDatetimeListCreate = xCreateSalesOrder.Element("Header").Element("DatetimeList").Element("Create").Element("DateTime");
        xOrderDatetimeListCreate.Value = orderInfo.DueDate;



        XElement xOrderStatusDate = xCreateSalesOrder.Element("Header").Element("Status").Element("Current").Element("DateList").Element("Date").Element("Date") ;
        xOrderStatusDate.Value = orderInfo.OrderStatusDate;

        XElement xOrderStatus = xCreateSalesOrder.Element("Header").Element("Status").Element("Current").Element("Description");
        xOrderStatus.Value = orderInfo.OrderStatus;

        //Job Details
        XElement xJobID = xCreateSalesOrder.Element("Lines").Element("Line").Element("InstructionList").Element("Instruction").Element("SequenceNmb");
        xJobID.Value = "1"; //TODO: business logic orderInfo.JobId;

        XElement xJobDescription = xCreateSalesOrder.Element("Lines").Element("Line").Element("InstructionList").Element("Instruction").Element("Name");
        xJobDescription.Value = orderInfo.ItemDescription;

        //Payment Method
        XElement xPaymentMethod = xCreateSalesOrder.Element("Header").Element("PaymentMethodList").Element("PaymentMethod").Element("Description");
        xPaymentMethod.Value = orderInfo.PaymentMethod;

        //Production Location
        XElement xProductionLocation = xCreateSalesOrder.Element("Lines").Element("Line").Element("LocationList").Element("PrimaryFulfillmentCenter").Element("Name");
        xProductionLocation.Value = orderInfo.ProductionLocation;

        //loop through the items
        var lstElements = xCreateSalesOrder.XPathSelectElements("Lines");
        int i = 1; //line 1
        foreach(XElement xel in lstElements)
        {
            foreach (XElement xel2 in xel.Elements())
            {
                //Response.Write("Line:" + i.ToString() + "<br/>");
                foreach (XElement xel3 in xel2.Elements())
                {
                    //Response.Write(xel3.Name + "<br/>");
                    if(xel3.Name == "ItemList")
                    {
                        foreach(XElement xItemList in xel3.Elements())
                        {
                            //Response.Write("-----" + xItemList.Name + "<br/>");
                            if((xItemList.Name == "Ordered") && (i == 1))
                            {
                                //XElement extendedItemPostageAmount = xItemList.Element("PriceList").Element("Extended").Element("Amount");
                                //extendedItemPostageAmount.Attribute("Amount").Value = orderInfo.PostageAmount;

                                //XElement extendedItemListPostageAmount = xItemList.Element("PriceList").Element("List").Element("Amount");
                                //extendedItemListPostageAmount.Attribute("Amount").Value = orderInfo.PostageAmount;

                                //XElement extendedItemUnitPostageAmount = xItemList.Element("PriceList").Element("Unit").Element("Amount");
                                //extendedItemUnitPostageAmount.Attribute("Amount").Value = orderInfo.PostageAmount;

                                //XElement extendedItemUnitPostageQuantity = xItemList.Element("PriceList").Element("Unit").Element("Quantity");
                                //extendedItemUnitPostageQuantity.Value = orderInfo.CalculatedQuantity;

                                XElement xPriceListQuantityListAdditional = xItemList.Element("QuantityList").Element("Additional").Element("Quantity").Element("Quantity");
                                xPriceListQuantityListAdditional.Value = orderInfo.CalculatedQuantity;
                            }


                            if ((xItemList.Name == "Ordered") && (i == 2))
                            {
                                XElement extendedItemPostageAmount = xItemList.Element("PriceList").Element("Extended").Element("Amount");
                                extendedItemPostageAmount.Attribute("Amount").Value = orderInfo.PostageAmount;

                                XElement extendedItemListPostageAmount = xItemList.Element("PriceList").Element("List").Element("Amount");
                                extendedItemListPostageAmount.Attribute("Amount").Value = orderInfo.PostageAmount;

                                XElement extendedItemUnitPostageAmount = xItemList.Element("PriceList").Element("Unit").Element("Amount");
                                extendedItemUnitPostageAmount.Attribute("Amount").Value = orderInfo.PostageAmount;

                                XElement extendedItemUnitPostageQuantity = xItemList.Element("PriceList").Element("Unit").Element("Quantity");
                                extendedItemUnitPostageQuantity.Value = orderInfo.CalculatedQuantity;

                                XElement xPriceListQuantityListAdditional = xItemList.Element("QuantityList").Element("Additional").Element("Quantity").Element("Quantity");
                                xPriceListQuantityListAdditional.Value = orderInfo.CalculatedQuantity;
                            }
                        }

                    }
                }
                i++;
            }
        }


        xdoc.Save(StaplesTemplateXMLAlteredPath);
        //Response.Write(fileName2 + " saved.");
        //string xmlString = xdoc.ToString();
        //XmlDocument xmlDoc = new XmlDocument();
        //xmlDoc.Load(fileName);  //the original file here 
        //using (var stringWriter = new StringWriter())
        //{
        //    using (var xmlTextWriter = XmlWriter.Create(stringWriter))
        //    {
        //        xmlDoc.WriteTo(xmlTextWriter);
        //        xmlTextWriter.Flush();
        //        stringWriter.GetStringBuilder().ToString();
        //    }
        //    com.staplesb2b.test.scgtw.SO_inbound_request soRequest = new com.staplesb2b.test.scgtw.SO_inbound_request();
        //    soRequest.Url = "https://scgtw.test.staplesb2b.com/ws/SplsB2BTransactionSalesOrder.EFI.inbound.wsd:SO_inbound_request";
        //    //http authenication - 
        //    NetworkCredential creds = new NetworkCredential("taradeltest", "t@r@de1T$t");
        //    soRequest.Credentials = creds;
        //    soRequest.PreAuthenticate = true;



        //    //com.staplesb2b.test.scgtw.handleRequest hRequest = new com.staplesb2b.test.scgtw.handleRequest();
        //    //object xmlFile = new object();
        //    string sReturn = string.Empty;
        //    string faultMessage = soRequest.handleRequest(stringWriter.ToString(), out sReturn);
        //    Response.Write(stringWriter.ToString());
        //    Response.Write("faultMessage:" + faultMessage);
        //    Response.Write("<br/>sReturn:" + sReturn);
        return StaplesTemplateXMLAlteredPath;
    }




    //retrieve the order details to place in the StaplesPOSXML
    //TODO: combine queries to one stored procedure
    private StaplesOrderInfo RetrieveTheOrderDetails(string orderID, string StaplesOrderID)
    {
        StaplesOrderInfo staplesOrderInfo = new StaplesOrderInfo();
        staplesOrderInfo.OrderID = orderID;
        staplesOrderInfo.StaplesOrderID = StaplesOrderID;
        staplesOrderInfo.billingCustomer = new StaplesOrderInfo.BillingCustomer(); //move into default constructor
        staplesOrderInfo.shippingCustomer = new StaplesOrderInfo.ShippingCustomer();
        staplesOrderInfo.CreationDate = DateTime.UtcNow.ToString("s");    //change for StaplesFormat YYYY-MM-DDTHH24:MI:SS
        staplesOrderInfo.DueDate = DateTime.UtcNow.ToString("s");                //change for StaplesFormat
        staplesOrderInfo.OrderStatus = "Order Placed";
        staplesOrderInfo.OrderStatusDate = DateTime.UtcNow.ToString("s");      //change for StaplesFormat
        staplesOrderInfo.PaymentMethod = "Pay At Store";
        staplesOrderInfo.ProductionLocation = "EDDM";
        staplesOrderInfo.StaplesStoreNumber = "0126";

        string connectString = ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString;
        string errorMsg = "";
        string sql = "SELECT * FROM pnd_OrderHeader where OrderID = " + orderID;
        //SendProcessEmail("DEBUG RetrieveTheURLToProcess", sql, jobName);
        //LogThis("Processing " + ReferenceID);
        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        staplesOrderInfo.NMB = StaplesOrderID;//rdr["OrderID"].ToString();
                        staplesOrderInfo.OrderID = orderID;
                        staplesOrderInfo.JobId = rdr["OrderGuid"].ToString();
                        staplesOrderInfo.Amount = FormatAmount(rdr["OrderAmt"].ToString());
                        staplesOrderInfo.SubTotal = FormatAmount(rdr["Subtotal"].ToString());
                        staplesOrderInfo.ItemDescription = "DIRECT MAIL"; //TODO
                                                                          //TODO: Yes item and postage would require a separate CreateSalesOrder / Lines / Line entry.
                                                                          //The 2 SKUs provided by Sam are 1690302, 1798651 
                        staplesOrderInfo.StaplesSKU = "1690302";


                        staplesOrderInfo.billingCustomer.FirstName = ExtractFirstName(rdr["FullName"].ToString()); //TODO: Split
                        staplesOrderInfo.billingCustomer.LastName = ExtractLastName(rdr["FullName"].ToString()); //TODO otherPartOfName;
                        staplesOrderInfo.billingCustomer.CompanyName = rdr["CompanyName"].ToString();
                        staplesOrderInfo.billingCustomer.Address1 = rdr["Address"].ToString();
                        staplesOrderInfo.billingCustomer.City = rdr["City"].ToString();
                        staplesOrderInfo.billingCustomer.State = rdr["State"].ToString();
                        staplesOrderInfo.billingCustomer.Zip = rdr["ZipCode"].ToString();
                        staplesOrderInfo.billingCustomer.Phone = FormatPhoneNumber(rdr["PhoneNumber"].ToString());
                        staplesOrderInfo.billingCustomer.Email = rdr["EmailAddress"].ToString();


                        //default values replaced below
                        staplesOrderInfo.shippingCustomer.FirstName = rdr["FullName"].ToString(); //TODO: Split
                        staplesOrderInfo.shippingCustomer.LastName = "Last Name"; //TODO otherPartOfName;
                        staplesOrderInfo.shippingCustomer.CompanyName = rdr["CompanyName"].ToString();
                        staplesOrderInfo.shippingCustomer.Address1 = rdr["Address"].ToString();
                        staplesOrderInfo.shippingCustomer.City = rdr["City"].ToString();
                        staplesOrderInfo.shippingCustomer.State = rdr["State"].ToString();
                        staplesOrderInfo.shippingCustomer.Zip = rdr["ZipCode"].ToString();
                        staplesOrderInfo.shippingCustomer.Phone = FormatPhoneNumber(rdr["PhoneNumber"].ToString());
                        staplesOrderInfo.shippingCustomer.Email = rdr["EmailAddress"].ToString();


                    }
                }


                //other parts
                string sql2 = "SELECT * FROM pnd_OrderItem where OrderID = " + orderID;
                command.CommandText = sql2;
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        staplesOrderInfo.Quantity = rdr["Quantity"].ToString();
                    }
                }


                sql = "usp_RetrieveOrderPaymentDetails4";
                //use a dataset here - 
                DataSet dataset = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(sql, conn);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("orderID", orderID));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("ProductType", "EDDM"));  //TODO - what if addressed product?
                adapter.Fill(dataset);

                int i = 0;
                foreach(DataTable dt in dataset.Tables)
                {
                    i++;
                    if(i == 2)
                    {
                        staplesOrderInfo.shippingCustomer.FirstName = ExtractFirstName(dt.Rows[0]["ExtraPcsContact"].ToString());
                        staplesOrderInfo.shippingCustomer.LastName = ExtractLastName(dt.Rows[0]["ExtraPcsContact"].ToString());
                        staplesOrderInfo.shippingCustomer.CompanyName = dt.Rows[0]["ExtraPcsCompany"].ToString();
                        staplesOrderInfo.shippingCustomer.Address1 = dt.Rows[0]["ExtraPcsAddress1"].ToString();
                        staplesOrderInfo.shippingCustomer.City = ExtractCity(dt.Rows[0]["ExtraPcsCityStateZip"].ToString());
                        staplesOrderInfo.shippingCustomer.State = ExtractState(dt.Rows[0]["ExtraPcsCityStateZip"].ToString());
                        staplesOrderInfo.shippingCustomer.Zip = ExtractZip(dt.Rows[0]["ExtraPcsCityStateZip"].ToString());
                        staplesOrderInfo.PostageAmount = FormatAmount(dt.Rows[0]["PostageFeeAmt"].ToString());
                        staplesOrderInfo.CalculatedAmount = CalculateAndFormatOrderAmount(staplesOrderInfo);
                        staplesOrderInfo.CalculatedSubTotal = CalculateAndFormatOrderAmount(staplesOrderInfo);
                        staplesOrderInfo.ExtraPieces = (dt.Rows[0]["ExtraPieces"].ToString());
                        staplesOrderInfo.CalculatedQuantity = CalculateAndFormatOrderQuantity(staplesOrderInfo);

                    }
                }

                //Staples Store Number
                string sql4 = "SELECT ISNull(Tag, 0) as StaplesStoreNumber FROM  dbo.OrderTag INNER JOIN dbo.OrderTagGroup ON dbo.OrderTag.OrderTagGroupID = dbo.OrderTagGroup.OrderTagGroupID INNER JOIN dbo.pnd_OrderHeader ON dbo.OrderTag.OrderID = dbo.pnd_OrderHeader.OrderID INNER JOIN dbo.pnd_OrderItem ON dbo.pnd_OrderHeader.OrderID = dbo.pnd_OrderItem.OrderID WHERE dbo.OrderTag.OrderTagGroupID = 3 AND dbo.pnd_OrderItem.OrderID = " + orderID;
                command.CommandText = sql4;
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        staplesOrderInfo.StaplesStoreNumber = rdr["StaplesStoreNumber"].ToString();
                    }
                }

                if(environment == "DEV")
                {
                    staplesOrderInfo.StaplesStoreNumber = "8508";
                }


                conn.Close();
            }

        }


        catch (Exception objException)
        {
            errorMsg = "The following errors occurred:<br /><br />";
            errorMsg += "<ul>";
            errorMsg += "<li>Message: " + objException.Message + "</li>";
            errorMsg += "<li>Source: " + objException.Source + "</li>";
            errorMsg += "<li>Stack Trace: " + objException.StackTrace + "</li>";
            errorMsg += "<li>Target Site: " + objException.TargetSite.Name + "</li>";
            errorMsg += "<li>SQL: " + sql + "</li>";
            errorMsg += "</ul>";
            if(debug)
            {
                HttpContext.Current.Response.Write(errorMsg + System.Environment.NewLine);
            }
            //Response.Write(errorMsg);
            //pnlError.Visible = true;
            //litError.Text = errorMsg;
            //SendProcessEmail(errorMsg, "HangFire Exception", "ERROR " + jobName);
            //LogThis(jobName + " ERROR: " + errorMsg + ":" + sql);
        }

        return staplesOrderInfo;

    }


    public string CalculateAndFormatOrderAmount(StaplesOrderInfo staplesOrderInfo)
    {
        string returnThis = string.Empty;
        decimal postage = 0;
        decimal orderAmount = 0;
        decimal newOrderAmount = 0;
        decimal.TryParse(staplesOrderInfo.Amount, out orderAmount);
        decimal.TryParse(staplesOrderInfo.PostageAmount, out postage);
        newOrderAmount = orderAmount - postage;
        return FormatAmount(newOrderAmount.ToString());
    }


    public string CalculateAndFormatOrderQuantity(StaplesOrderInfo staplesOrderInfo)
    {
        string returnThis = string.Empty;
        decimal pieces = 0;
        decimal extrapieces = 0;
        decimal newQuantity = 0;
        decimal.TryParse(staplesOrderInfo.Quantity, out pieces);
        decimal.TryParse(staplesOrderInfo.ExtraPieces, out extrapieces);
        newQuantity = pieces + extrapieces;
        return String.Format("{0:0}", newQuantity);
    }


    public string ExtractFirstName(string fullName)
    {
        string returnThis = string.Empty;
        var names = fullName.Split(' ');

        string firstName = names[0];
        returnThis = firstName;
        //string lastName = names[1];

        return returnThis;
    }

    public string ExtractLastName(string fullName)
    {
        string returnThis = string.Empty;
        var names = fullName.Split(' ');

        string firstName = names[0];
        if(names.Length > 1)
        {
            string lastName = names[1];
            returnThis = lastName;
        }

        return returnThis;
    }

    public string ExtractCity(string cityStateZip)
    {
        string returnThis = string.Empty;
        var names = cityStateZip.Split(',');
        returnThis = names[0];
        return returnThis;
    }

    public string ExtractState(string cityStateZip)
    {
        string returnThis = string.Empty;
        var names = cityStateZip.Split(' ');
        foreach(string s in names)
        {
            if(s.Length == 2)
            {
                returnThis = s;
            }
        }
        return returnThis;
    }


    public string ExtractZip(string cityStateZip)
    {
        string returnThis = string.Empty;
        var names = cityStateZip.Split(' ');
        foreach (string s in names)
        {
            returnThis = s;
        }
        return returnThis;
    }

    public string FormatPhoneNumber(string oldPhoneNumber)
    {
        return new string(oldPhoneNumber.Where(c => char.IsDigit(c)).ToArray());
    }

    public string FormatAmount(string oldAmount)
    {
        string returnThis = string.Empty;
        decimal d = 0;
        decimal.TryParse(oldAmount, out d);
        returnThis = String.Format("{0:0.00}",d);
        return returnThis;
    }





    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public class StaplesOrderInfo
    {
        public string NMB;
        public string JobId;

        /// <summary>
        /// Standard amount for a subtotal after any coupons or discounts are applied
        /// no reason to store as number, it comes in as a string and leaves as a string
        /// and there is no logic done to it in between
        /// </summary>
        public string Amount;
        public string CalculatedAmount;
        public string CalculatedSubTotal;


        /// <summary>
        /// Standard amount for a subtotal after any coupons or discounts are applied
        /// </summary>
        public string SubTotal;

        public string AmountAfterDiscountsNoTax;
        public string AmountBeforeDiscountsNoTax;

        public string CreationDate;
        public string DueDate;

        public string OrderID;
        public string StaplesOrderID;
        public string OrderStatus;
        public string OrderStatusDate;

        public string ItemDescription;

        public string PaymentMethod;
        public string PostageAmount;
        public string ProductionLocation;

        /// <summary>
        /// Printed Pieces + Extra Pieces
        /// </summary>
        public string CalculatedQuantity;
        public string Quantity;
        public string ExtraPieces;
        public string StaplesSKU;
        public string StaplesStoreNumber;

        public BillingCustomer billingCustomer { get; set; }
        public ShippingCustomer shippingCustomer { get; set; }

        //protected StaplesOrderInfo(StaplesOrderInfo staplesInfo)
        //{
        //    this.shippingCustomer = staplesInfo.shippingCustomer;
        //    this.billingCustomer = staplesInfo.billingCustomer;
        //}


        public class BillingCustomer
        {
            public string CompanyName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address1 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }

        public class ShippingCustomer : BillingCustomer
        {

        }



    }
}


