using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GoogleAnalytics : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        string fileName = "";
        string fileContent = "";
        string appRoot = Server.MapPath("~/");

        //Create siteObj
        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);

        //If Google Analytics script is needed...
        if (siteDetails.UseGoogleAnalytics)
        { 


            switch (siteID)
            {

                //EDDM
                case 1:
                    fileName = "GoogleAnalytics-EDDM.txt";
                    break;


                //FedEx
                case 41:
                    fileName = "GoogleAnalytics-FedEx.txt";
                    break;


                //Staples Act Mgr
                case 78:
                    fileName = "GoogleAnalytics-StaplesActMgr.txt";
                    break;


                //RAMP
                case 80:
                    fileName = "GoogleAnalytics-RAMP.txt";
                    break;


                //FedEx LIST DEMO
                case 84:
                    fileName = "GoogleAnalytics-FedEx.txt";
                    break;


                //Staples Store
                case 91:
                    fileName = "GoogleAnalytics-StaplesStore.txt";
                    break;


                //Staples Consumer
                case 93:
                    fileName = "GoogleAnalytics-StaplesConsumer.txt";
                    break;


                //RAMP Express
                case 98:
                    fileName = "GoogleAnalytics-RAMPExpress.txt";
                    break;


                //Taradel DM
                case 100:
                    fileName = "GoogleAnalytics-TaradelDM.txt";
                    break;

                default:
                    break;

            }

            if (!String.IsNullOrEmpty(fileName))
            {

                if (File.Exists(appRoot + "\\assets\\scripts\\" + fileName))
                {
                    phGoogleAnalyticsScript.Visible = true;
                    fileContent = File.ReadAllText(appRoot + "\\assets\\scripts\\" + fileName);
                    litGoogleAnalyticsScript.Text = fileContent;
                }

            }

        }



    }

}