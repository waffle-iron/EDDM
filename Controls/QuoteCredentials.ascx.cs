using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_EDDMCredentials : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
       
        //Staples Direct Mail and Ramp Express does not want to see this.
        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        if ((siteID == 93) || (siteID == 83) || (siteID == 91) || (siteID == 78) || (siteID == 98) || (siteID == 95))
        { pnlCredentials.Visible = false; }

    }


}