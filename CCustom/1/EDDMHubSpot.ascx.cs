using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EDDMHubSpot : System.Web.UI.UserControl
{


    //This script is used on EveryDoorDirectMail.com (only) and is injected in all pages (not in DEV).



    //Fields
    string currentMode = appxCMS.Util.AppSettings.GetString("Environment").ToLower();
    int siteID = appxCMS.Util.CMSSettings.GetSiteId();




    protected void Page_Load(object sender, EventArgs e)
    {


        if (siteID == 1) 
        {

            if (currentMode.ToLower() != "dev")
            { phHubspotScript.Visible = true; }

        }
        

    }
}