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

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        imgLogo.ImageUrl = "/cmsimages/" + siteID + "/quote-footer-logo.png";

        //Temp logic.  Will improve later.
        if (siteID != 98)
        { phFooterContent.Visible = true;}

    }
}