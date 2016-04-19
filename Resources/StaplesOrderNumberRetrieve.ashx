<%@ WebHandler Language="C#" Class="StaplesOrderNumberRetrieve" %>
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

public class StaplesOrderNumberRetrieve : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Clear();

        if (!String.IsNullOrEmpty(context.Request.QueryString["OrderID"]))
        {
            string OrderID = context.Request.QueryString["OrderID"];
            context.Response.ContentType = "text/html";
            string StaplesOrderID = RetrieveStaplesOrderNumber(OrderID);
            context.Response.Write(StaplesOrderID);
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("<p>Need a valid Order ID</p>");
        }
    }

    public string RetrieveStaplesOrderNumber(string OrderID)
    {
        string connectString = ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString;
        string sql ="usp_RetrieveStaplesOrderNumber";
        string returnThis = "0";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("TaradelOrder_ID", OrderID));
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        returnThis = rdr[0].ToString();
                    }
                }
                conn.Close();
            }
        }
        catch(Exception ex)
        {
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Write(ex.ToString());
        }
        return returnThis;
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }



}
