using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_QuoteHeaderLogo : System.Web.UI.UserControl
{

    //===============================================================================================================
    //  NOTES:
    //  Logo image must exist in the white label cmsimages folder and be named 'quote-header-logo.png'
    //
    //  If site has a CoBrand logo (ex: FedEx) then the file name must be in the pnd_SiteStringResourceMgr tbl and the file must be 
    //  located in the cmsimages folder structure.
    //===============================================================================================================
    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        imgLogo.ImageUrl = "/cmsimages/" + siteID + "/quote-header-logo.png";


        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);


        if (siteDetails.UseCoBrandLogo)
        {
            if (!String.IsNullOrEmpty(SiteUtility.GetStringResourceValue(siteID, "CoBrandHeaderLogo")))
            { imgCoBrandLogo.ImageUrl = "/cmsimages/" + siteID + "/" + SiteUtility.GetStringResourceValue(siteID, "CoBrandHeaderLogo"); }
        }

        else
        { imgCoBrandLogo.Visible = false; }



    }
}