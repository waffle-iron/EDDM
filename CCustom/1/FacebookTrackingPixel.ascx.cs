using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FacebookTrackingPixel : System.Web.UI.UserControl
{

    //This script is used on EveryDoorDirectMail.com and is fires on these four pages:
    //Quote Request (EDDM-Quote-Request)
    //Receipt.aspx
    //Account-Welcome
    //Map Creation (Step1-TargetReview.aspx)



    //Set from Receipt.aspx page
    public int orderTotal
    { get; set; }



    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();

        if (siteID == 1)
        {

            string fileName = "FacebookTrackingPixel-EDDM.txt";
            string appRoot = Server.MapPath("~/");
            string fileContent = File.ReadAllText(appRoot + "\\assets\\scripts\\" + fileName);
            string referralUrl = Request.RawUrl.ToLower();




            if (!String.IsNullOrEmpty(fileContent))
            {

                //Upon Sign Up....
                if (referralUrl.ToLower().Contains("account-welcome"))
                {
                    fileContent = fileContent.Replace("##event-tags##", "fbq('track', \"CompleteRegistration\")");
                }


                //Upon Quote Request Confirmation...
                else if (referralUrl.ToLower().Contains("eddmquote.aspx"))
                {
                    fileContent = fileContent.Replace("##event-tags##", "fbq('track', \"Lead\")");
                }


                //Upon Map Completion...
                else if (referralUrl.ToLower().Contains("step1-targetreview.aspx"))
                {
                    fileContent = fileContent.Replace("##event-tags##", "fbq('track', \"InitiateCheckout\")");
                }


                //Upon Order Completion...
                else if (referralUrl.ToLower().Contains("receipt.aspx"))
                {
                    fileContent = fileContent.Replace("##event-tags##", "fbq('track', 'Purchase', {value: '" + orderTotal + "', currency: 'USD'})");
                }

                else 
                {
                    //Set default action
                    fileContent = fileContent.Replace("##event-tags##", "fbq('track', \"PageView\")");
                }


                phFacebookTrackingPixel.Visible = true;
                litFacebookTrackingPixel.Text = fileContent;

            }

        }

    }


}