<%@ WebHandler Language="C#" Class="ShowInvalidUploadedAddresses" %>

using System;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;






public class ShowInvalidUploadedAddresses : IHttpHandler
{


    //=======================================================================================================================================
    //  Written by DSF. 3/25/2016. Improve as needed.
    //=======================================================================================================================================

    //Steps.  
    //Step 1 - create a Container Class
    //Step 2 - create a list of container classes (empty)
    //Step 3 - read the source json file (string)
    //Step 4 - create a new serializer object
    //Step 5 - fill the list of container classes by using Deserialize which will automatically read it as a list (assuming the source file is an array).
    //Step 6 - Loop through results and build wrapping HTML


    public void ProcessRequest (HttpContext context)
    {


        //Step 2 - create a list of container classes (empty)
        List<RejectedAddress> oAddresses = new List<RejectedAddress>();


        //Step 3 - read the source json file (string)
        string projectID = "";
        if (!string.IsNullOrEmpty(context.Request.QueryString["p"]))
        { projectID = context.Request.QueryString["p"].ToString(); }

        string filePath = "c:\\inetpub\\webroot\\TARADELUS\\everydoordirectmail.com WL\\App_Data\\AddressedListInbound\\" + projectID + "\\list-rejects.json";
        string fileContent = File.ReadAllText(filePath);

        //Step 4 - create a new serializer object
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();



        StringBuilder sampleHtml = new StringBuilder();

        if (fileContent.Length > 2)
        {

            try
            {

                //Step 5 - fill the list of container classes by using Deserialize which will automatically read it as a list (assuming the source file is an array).
                oAddresses = jsonSerializer.Deserialize<List<RejectedAddress>>(fileContent);



                //Step 6 - Loop through results and build wrapping html
                sampleHtml.Append("<table class=\"table table-bordered table-condensed table-striped table-hover detailedData\">" + Environment.NewLine);
                sampleHtml.Append("<thead>" + Environment.NewLine);
                sampleHtml.Append("<tr>" + Environment.NewLine);
                sampleHtml.Append("<th class=\"col-sm-7\">Address</th>" + Environment.NewLine);
                sampleHtml.Append("<th class=\"col-sm-5\">Issue</th>" + Environment.NewLine);
                sampleHtml.Append("</tr>" + Environment.NewLine);
                sampleHtml.Append("</thead>" + Environment.NewLine);
                sampleHtml.Append("<tbody>" + Environment.NewLine);

                foreach (var address in oAddresses)
                {
                    sampleHtml.Append("<tr>");
                    sampleHtml.Append("<td>" + address.Address + "</td>");
                    sampleHtml.Append("<td>" + address.RejectReason + "</td>");
                    sampleHtml.Append("</tr>");

                }

                sampleHtml.Append("</tbody>" + Environment.NewLine);
                sampleHtml.Append("</table>" + Environment.NewLine);

            }


            catch (Exception ex)
            {

                LogWriter logger = new LogWriter();
                logger.RecordInLog("Error in ShowInvalidUploadedAddresses Handler: " + ex.Message.ToString());

            }


        }

        //File will always exist but may contain just "[]" indicating no errors.
        else
        {sampleHtml.Append("no errors");}




        context.Response.ContentType = "text/plain";
        context.Response.Write(sampleHtml.ToString());

    }





    public bool IsReusable
    {
        get {
            return false;
        }
    }



    //Step 1  - create a Container Class
    public class RejectedAddress
    {

        public string RejectReason { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

    }


}

