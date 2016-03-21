using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CLibrary_VisualWebsiteOptimizer : System.Web.UI.UserControl
{

    protected int siteID = appxCMS.Util.CMSSettings.GetSiteId();
    protected string mode = appxCMS.Util.AppSettings.GetString("Environment").ToLower();


    protected void Page_Load(object sender, EventArgs e)
    {

        if(mode != "dev")
        { pnlVWOScript.Visible = true; }

    }


}