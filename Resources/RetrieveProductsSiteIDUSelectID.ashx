<%@ WebHandler Language="C#" Class="RetrieveProductsSiteIDUSelectID" %>

using System;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Taradel.Util;




public class RetrieveProductsSiteIDUSelectID : IHttpHandler
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


    //try not to change this code 3/16/2016
    public class SlimProduct
    {
        public int ProductID { get; set; }
        public int BaseProductID { get; set; }
        public int USelectID { get; set;}
        public string Name { get; set; }
        public int SiteID { get; set; }
    }



    public void ProcessRequest(HttpContext context)
    {
        List<SlimProduct> lstSlimProducts = new List<SlimProduct>();
        string siteID = string.Empty;
        string uSelectID = string.Empty;
        if (context.Request.Params.AllKeys.Length > 0)
        {
            foreach (string key in HttpContext.Current.Request.Params.AllKeys)
            {
                string value = HttpContext.Current.Request.Params[key];
                //context.Response.Write("key:" + key + " : " + value);
                if(key == "USelectID")
                {
                    uSelectID = value;
                }

                if(key == "SiteID")
                {
                    siteID = value;
                }
            }
        }

        if(siteID == string.Empty)
        {
            siteID = "1";
        }

        if(uSelectID == string.Empty)
        {
            uSelectID = "1";
        }




        try
        {
            DataTable dt = RetrieveProducts(uSelectID, siteID);
            foreach(DataRow dr in dt.Rows)
            {
                //do this on the other end if needed --> 
                //Taradel.WLProduct WLproduct = Taradel.WLProductDataSource.GetProduct(Int32.Parse(dr["ProductID"].ToString()));
                SlimProduct slim = new SlimProduct();
                slim.SiteID = Int32.Parse(siteID);
                slim.USelectID = Int32.Parse(uSelectID);
                slim.ProductID = Int32.Parse(dr["ProductID"].ToString());
                slim.BaseProductID =  Int32.Parse(dr["BaseProductID"].ToString());
                slim.Name = dr["Name"].ToString();
                lstSlimProducts.Add(slim);
            }
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.Write(jsonSerializer.Serialize(lstSlimProducts));



        }
        catch (Exception ex)
        {
            string errorFileName = "~\\Logs\\RetrieveProductSiteIDUSelectID_Error.txt";
            string errorFullPath = HttpContext.Current.Server.MapPath(errorFileName);

            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(errorFullPath))
                {
                    sw.WriteLine("-------------------------START ERROR-------------------------------------------");
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("-------------------------END ERROR-------------------------------------------");

                }
            }
            catch(Exception ex2)
            {
                //eat it
            }
        }

    } //end method



    protected DataTable RetrieveProducts(string USelectID, string SiteID)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT        dbo.wl_Product.ProductID, dbo.wl_Product.BaseProductID, dbo.wl_Product.Name, dbo.wl_Product.SiteId, dbo.wl_Product.Deleted, ");
        sb.Append("                 dbo.USelectProductConfiguration.USelectId, dbo.USelectMethod.Name AS USelectName, dbo.USelectMethod.ShortDescription, dbo.USelectMethod.Description ");
        sb.Append("FROM            dbo.wl_Product INNER JOIN ");
        sb.Append("                 dbo.pnd_Product ON dbo.wl_Product.BaseProductID = dbo.pnd_Product.ProductID INNER JOIN ");
        sb.Append("                 dbo.USelectProductConfiguration ON dbo.pnd_Product.ProductID = dbo.USelectProductConfiguration.ProductId INNER JOIN ");
        sb.Append("                 dbo.USelectMethod ON dbo.USelectProductConfiguration.USelectId = dbo.USelectMethod.USelectId ");
        sb.Append("WHERE(dbo.wl_Product.SiteId = " + SiteID + ") AND(dbo.wl_Product.Deleted = 0) AND(USelectProductConfiguration.USelectId = " + USelectID + ")");
        string sql = sb.ToString();
        string connectString = "Data Source=204.186.24.24;Initial Catalog=Taradel-EDDMRedesignMerge;Persist Security Info=True;User ID=pnd_admin;Password=t3$t";
        DataTable dt = new DataTable();

        try
        {
            SqlConnection conn = new SqlConnection(connectString);
            SqlDataAdapter fillMe = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();

            fillMe.Fill(ds);
            dt = ds.Tables[0];
        }

        catch (Exception ex)
        {
            string errorFileName = "~\\Logs\\RetrieveProductSiteIDUSelectID_Error.txt";
            string errorFullPath = HttpContext.Current.Server.MapPath(errorFileName);

            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(errorFullPath))
                {
                    sw.WriteLine("-------------------------START ERROR-------------------------------------------");
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("-------------------------END ERROR-------------------------------------------");

                }
            }
            catch(Exception ex2)
            {
                //eat it
            }
        }
        return dt;


    }



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}


