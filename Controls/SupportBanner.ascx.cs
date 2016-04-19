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
        string phoneNumber = "";

        //Addtional Site data.  This will be used to determine if we show chat and/or if we show FAQ/Support link.
        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);


        //Create appX oSite obj to get phone number for site.
        appxCMS.Site oSite = appxCMS.SiteDataSource.GetSite(siteID);


        if (oSite != null)
        {

            if (!String.IsNullOrEmpty(oSite.TollFreeNumber))
            { phoneNumber = ReformatPhoneNumber(oSite.TollFreeNumber); }
            else if (!String.IsNullOrEmpty(oSite.PhoneNumber))
            { phoneNumber = ReformatPhoneNumber(oSite.PhoneNumber); }
            else
            { phoneNumber = "unknown"; }
            
        }


        //Set phone number labels
        litPhone.Text = phoneNumber;
        litPhone2.Text = phoneNumber;
        litPhone3.Text = phoneNumber;


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


    public string ReformatPhoneNumber(string PhoneNumber)
    {
        //===================================================================================
        // This function MUST receive a 10 digit phone number in order to work properly.	
        // It returns the 10-digit integer into a format like (804) 555-5555.				
        // ===================================================================================

        string areaCode;
        string prefixDigits;
        string suffixDigits;
        string formattedNumber;

        if (String.IsNullOrEmpty(PhoneNumber))
        { formattedNumber = ""; }

        else
        {
            areaCode = PhoneNumber.Substring(0, 3);
            prefixDigits = PhoneNumber.Substring(3, 3);
            suffixDigits = PhoneNumber.Substring(6, 4);

            formattedNumber = areaCode + "-" + prefixDigits + "-" + suffixDigits;
        }

        return formattedNumber;
    }


}