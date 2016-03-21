using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_YourAccountUserMenu : System.Web.UI.UserControl
{
    
    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();

        //Addtional Site data.  Build SiteDetails Obj
        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);


        if (siteDetails.AllowListProducts)
        { phSavedLists.Visible = true;}


    }

}