using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_QuoteOver60MillionMailed : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

        //Ramp Express does not want to see this.
        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        
        if (siteID == 98)
        { pnlMailed.Visible = false; }

    }

}