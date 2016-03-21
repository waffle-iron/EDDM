using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DevWarningBanner : System.Web.UI.UserControl
{


    protected void Page_Load(object sender, EventArgs e)
    {

        string currentMode = appxCMS.Util.AppSettings.GetString("Environment").ToLower();
        int siteID = appxCMS.Util.CMSSettings.GetSiteId();


        if (currentMode == "dev")
        { pnlDevBanner.Visible = true; }


    }


}