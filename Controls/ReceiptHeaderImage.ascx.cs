using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_ReceiptHeaderImage : System.Web.UI.UserControl
{

    //Requirements:
    //logo (jpg) file must be named 'receipt-logo.jpg' and be approximately 150x50 in size.  Can be wider but taller is not recommended. 
    //logo file must be a jpg.  WebSupergoo doesn't do well with PNGs.
    //logo file  MUST exists in the user's /cmsimages/[SiteID] folder.
    //EDDM: /cmsimages/1/receipt-logo.jpg.


    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        imgHeaderLogo.ImageUrl = "/cmsimages/" + siteID + "/receipt-logo.jpg";

    }


}