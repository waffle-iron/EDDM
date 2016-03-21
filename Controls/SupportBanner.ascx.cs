using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CLibrary_SupportBanner : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();

        //Addtional Site data.  Build SiteDetails Obj
        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);

        litPhone.Text = siteDetails.SupportPhone;
        litPhone2.Text = siteDetails.SupportPhone;
        litPhone3.Text = siteDetails.SupportPhone;


        //if Chat not allowed, remove it
        if (siteDetails.ShowChat)
        {

            //show everything
            if (siteDetails.ShowFaq)
            { pnlWithChat.Visible = true;}

        }

        //hide chat option
        else 
        {

            //don't show chat or FAQ links
            if (!siteDetails.ShowFaq)
            { pnlWithoutChatFaq.Visible = true; }

            //show FAQs but not chat
            else
            { pnlWithoutChat.Visible = true; }
            
            
        }


        //slight hack. If other sites require this then make it configurable.
        //Staples Act Mgr
        if (siteID == 95)
        {

            litSupport.Text = "Support/FAQs";
            hypSupport.Text = "See FAQs Here";
            hypSupport.NavigateUrl = "/FAQ";
        }
        
        else
        {
            litSupport.Text = "Support";
            hypSupport.Text = "Get Answers Here";
            hypSupport.NavigateUrl = "/Support";
        }

    }

}