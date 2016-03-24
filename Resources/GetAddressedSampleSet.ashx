<%@ WebHandler Language="C#" Class="GetAddressedSampleSet" %>

using System;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Text;
using System.IO;
using System.Collections.Generic;


    //=======================================================================================================================================
    //  Written by DSF. 3/24/2016. Improve as needed.
    //
    //  Will return sample records in the form of Bootstrap structured HTML table.
    //
    //  API Parameter Values:
    //  Radius: int
    //  Lat: decimal
    //  Lon: decimal
    //  RadiusType: String. Minutes or Miles
    //  CountOnly: String. True / False
    //  Gender: F, M, or U (unknown). 
    //  HomeOwnership: O or R
    //  Income:  A-J, comma delimted
    //  Ethnicity: char values, comma delimited.
    //  KidsPresent: Y,N
    //  MartialStatus: A, B, M, S.  Currently ONLY using M or S
    //  NetWorth: char values, comma delimited.
    //  Property Value (HomeMktVal): char values, comma delimited.
    //  OutputSample=True/False" Will return sample set of X or full complete set.
    //=======================================================================================================================================



public class GetAddressedSampleSet : IHttpHandler
{



    public void ProcessRequest (HttpContext context)
    {

        //STEPS:
        //1) Get and construct actual enpoint
        //2) Make web request
        //3) loop through lines and build html
        //4) return results as html structured string


        bool debug = false;                                                             //used for debugging.
        bool maskData = true;                                                           //used to mask some data to make it unusable but readable.
        int numOfSampleRecords = 15;                                                    //used to limit number of results. 
        string responseDescription = "";                                                //used for debugging.
        int numOfReturnedRecords = 0;                                                   //used for debugging.



        //STEP 1)
        //Endpoint URL and possible querystring parameters
        string baseEndPoint = "http://ktools.eddmsite.com/AddressedMailCounts.ashx?OutputSample=True&CountOnly=False";

        string radius = "";
        string lat = "";
        string lon = "";
        string radiusType = "";
        string gender = "";
        string homeOwnership = "";
        string income = "";
        string children = "";
        string maritalStatus = "";
        string ageRange = "";
        string ethnicity = "";
        string zipcode = "";
        string propertyValue = "";
        string netWorth = "";

        if (context.Request.QueryString["Radius"] != null)
        { radius = "&Radius=" + context.Request.QueryString["Radius"].ToString(); }

        if (context.Request.QueryString["Lat"] != null)
        { lat = "&Lat=" + context.Request.QueryString["Lat"].ToString(); }

        if (context.Request.QueryString["Lon"] != null)
        { lon = "&Lon=" + context.Request.QueryString["Lon"].ToString(); }

        if (context.Request.QueryString["RadiusType"] != null)
        { radiusType = "&RadiusType=" + context.Request.QueryString["RadiusType"].ToString(); }

        if (context.Request.QueryString["Gender"] != null)
        { gender = "&Gender=" + context.Request.QueryString["Gender"].ToString(); }

        if (context.Request.QueryString["HomeOwnership"] != null)
        { homeOwnership = "&HomeOwnership=" + context.Request.QueryString["HomeOwnership"].ToString(); }

        if (context.Request.QueryString["Income"] != null)
        { income = "&Income=" + context.Request.QueryString["Income"].ToString(); }

        if (context.Request.QueryString["KidsPresent"] != null)
        { children = "&KidsPresent=" + context.Request.QueryString["KidsPresent"].ToString(); }

        if (context.Request.QueryString["MaritalStatus"] != null)
        { maritalStatus = "&MaritalStatus=" + context.Request.QueryString["MaritalStatus"].ToString(); }

        if (context.Request.QueryString["AgeRange"] != null)
        { ageRange = "&AgeRange=" + context.Request.QueryString["AgeRange"].ToString(); }

        if (context.Request.QueryString["Ethnicity"] != null)
        { ethnicity = "&Ethnicity=" + context.Request.QueryString["Ethnicity"].ToString(); }

        if (context.Request.QueryString["ZipCode"] != null)
        { zipcode = "&ZipCode=" + context.Request.QueryString["ZipCode"].ToString(); }

        if (context.Request.QueryString["HomeMktVal"] != null)
        { propertyValue = "&HomeMktVal=" + context.Request.QueryString["HomeMktVal"].ToString(); }

        if (context.Request.QueryString["NetWorth"] != null)
        { netWorth = "&NetWorth=" + context.Request.QueryString["NetWorth"].ToString(); }

        string fullEndPoint = baseEndPoint + radius + lat + lon + radiusType + gender + homeOwnership + income + children + maritalStatus + ageRange + ethnicity + zipcode + propertyValue + netWorth;




        //STEP 2
        // Create a request for the URL. 
        StringBuilder results = new StringBuilder();
        WebRequest request = WebRequest.Create(fullEndPoint);


        // If required by the server, set the credentials.
        request.Credentials = CredentialCache.DefaultCredentials;


        // Get the response.
        WebResponse response = request.GetResponse();


        // Capture the status.
        responseDescription = ((HttpWebResponse)response).StatusDescription;


        // Get the stream containing content returned by the server.
        Stream dataStream = response.GetResponseStream();


        // Open the stream using a StreamReader for easy access.
        StreamReader reader = new StreamReader(dataStream);


        // Read the content.
        results.Append(reader.ReadToEnd());


        // Clean up the streams and the response.
        reader.Close();
        response.Close();





        //3) STEP 3 - Build HTML
        List<string> addressList = new List<string>(results.ToString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
        List<AddressRecord> listOfAddressObjs = new List<AddressRecord>();
        List<string> partialListOfAddresses = new List<string>();
        List<string> duplicateAddressList = new List<string>();                         //for debugging
        StringBuilder sampleHtml = new StringBuilder();
        int duplicates = 0;
        string compareString = "";
        int counter = 0;



        //Go through lines of response.  Only go through num of times required by defined sample set.
        foreach (string address in addressList)
        {

            List<string> splitAddress = new List<string>(address.Split('|'));
            AddressRecord indAddress = new AddressRecord();

            indAddress.firstName = splitAddress[0] != null ? splitAddress[0] : "n/a";
            indAddress.lastName = splitAddress[1] != null ? splitAddress[1] : "n/a";
            indAddress.address = splitAddress[2] != null ? splitAddress[2] : "n/a";
            indAddress.city = splitAddress[3] != null ? splitAddress[3] : "n/a";
            indAddress.state = splitAddress[4] != null ? splitAddress[4] : "n/a";
            indAddress.zipCode = splitAddress[5] != null ? splitAddress[5] : "n/a";


            //Build the partial compare string
            compareString = splitAddress[2].Trim() != null ? splitAddress[2] : "n/a" + "-";
            compareString += splitAddress[3].Trim() != null ? splitAddress[3] : "n/a" + "-";
            compareString += splitAddress[4].Trim() != null ? splitAddress[4] : "n/a" + "-";
            compareString += splitAddress[5].Trim() != null ? splitAddress[5] : "n/a";

            //Next - eliminate duplicates.  Skip it if it exists already.
            if (partialListOfAddresses.Contains(compareString))
            {
                duplicates++;
                duplicateAddressList.Add(compareString);
            }

            //Add it to the real list and the 'partial compare' list
            else
            {

                if(counter < numOfSampleRecords)
                {
                    partialListOfAddresses.Add(compareString);
                    listOfAddressObjs.Add(indAddress);
                    counter++;
                }

            }

        }



        //Now get an actual count of addresses captured and addd it to the output
        numOfReturnedRecords = listOfAddressObjs.Count;





        //loop through 'real' list (no duplicates) and convert to html
        sampleHtml.Append("<table class=\"table table-bordered table-condensed table-striped table-hover detailedData\">" + Environment.NewLine);
        sampleHtml.Append("<thead>" + Environment.NewLine);
        sampleHtml.Append("<tr>" + Environment.NewLine);
        sampleHtml.Append("<th class=\"col-sm-2\">First Name</th>" + Environment.NewLine);
        sampleHtml.Append("<th class=\"col-sm-2\">Last Name</th>" + Environment.NewLine);
        sampleHtml.Append("<th class=\"col-sm-3\">Address</th>" + Environment.NewLine);
        sampleHtml.Append("<th class=\"col-sm-3\">City</th>" + Environment.NewLine);
        sampleHtml.Append("<th class=\"col-sm-1\">State</th>" + Environment.NewLine);
        sampleHtml.Append("<th class=\"col-sm-1\">Zip Code</th>" + Environment.NewLine);
        sampleHtml.Append("</tr>" + Environment.NewLine);
        sampleHtml.Append("</thead>" + Environment.NewLine);
        sampleHtml.Append("<tbody>" + Environment.NewLine);


        if (maskData)
        {

            foreach (AddressRecord address in listOfAddressObjs)
            {
                sampleHtml.Append("<tr>" + Environment.NewLine);
                sampleHtml.Append("<td>" + address.firstName + "</td>");
                sampleHtml.Append("<td>**" + address.lastName.Substring(2,address.lastName.Length - 2) + "</td>");
                sampleHtml.Append("<td>**" + address.address.Substring(2,address.address.Length - 2) + "</td>");
                sampleHtml.Append("<td>**" + address.city.Substring(2,address.city.Length - 2) + "</td>");
                sampleHtml.Append("<td>" + address.state + "</td>");
                sampleHtml.Append("<td>" + address.zipCode + "</td>");
                sampleHtml.Append("</tr>" + Environment.NewLine);
            }

        }

        else
        {

            foreach (AddressRecord address in listOfAddressObjs)
            {
                sampleHtml.Append("<tr>" + Environment.NewLine);
                sampleHtml.Append("<td>" + address.firstName + "</td>");
                sampleHtml.Append("<td>" + address.lastName + "</td>");
                sampleHtml.Append("<td>" + address.address + "</td>");
                sampleHtml.Append("<td>" + address.city + "</td>");
                sampleHtml.Append("<td>" + address.state + "</td>");
                sampleHtml.Append("<td>" + address.zipCode + "</td>");
                sampleHtml.Append("</tr>" + Environment.NewLine);
            }

        }


        sampleHtml.Append("</tbody>" + Environment.NewLine);
        sampleHtml.Append("</table>" + Environment.NewLine);




        if (debug)
        {
            sampleHtml.Append("[status: " + responseDescription + "]" + Environment.NewLine);
            sampleHtml.Append("[addresses: " + numOfReturnedRecords + "]" + Environment.NewLine);
            sampleHtml.Append("[limit: " + numOfSampleRecords + "]" + Environment.NewLine);
            sampleHtml.Append("[counter: " + counter + "]" + Environment.NewLine);
            sampleHtml.Append("[duplicates: " + duplicates + "]" + Environment.NewLine);

            //show duplicates
            foreach (string address in duplicateAddressList)
            {
                sampleHtml.Append(address + Environment.NewLine);
            }

        }

        context.Response.ContentType = "text/plain";
        context.Response.Write(sampleHtml.ToString());

    }



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }



    public class AddressRecord
    {

        public string firstName
        { get; set; }

        public string lastName
        { get; set; }

        public string address
        { get; set; }

        public string city
        { get; set; }

        public string state
        { get; set; }

        public string zipCode
        { get; set; }

    }

}