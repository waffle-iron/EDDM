using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SitePhoneNumber : System.Web.UI.UserControl
{

        // Pass this control any combination of three properties:
        //
        // useIcon:         true/false          True will show the font awesome icon.  False shows nothing.
        // addCallUs:       true/false          True will prefix the string with "Call Us".  False will not.
        // makeHyperLink    treu/false          True will convert the string into a hyperlink. False will display as text-only.
    


    private string _useIcon;
    public string useIcon
    {

        get
        {
            if (String.IsNullOrEmpty(_useIcon))
            { return "false"; }

            else
            { return _useIcon; }
        }

        set
        { _useIcon = value; }

    }


    private string _addCallUs;
    public string addCallUs
    {

        get
        {
            if (String.IsNullOrEmpty(_addCallUs))
            { return "false"; }

            else
            { return _addCallUs; }
        }

        set
        { _addCallUs = value; }

    }


    private string _makeHyperLink;
    public string makeHyperLink
    {

        get
        {
            if (String.IsNullOrEmpty(_makeHyperLink))
            { return "false"; }

            else
            { return _makeHyperLink; }
        }

        set
        { _makeHyperLink = value; }

    }





    protected void Page_Load(object sender, EventArgs e)
    {


        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        appxCMS.Site siteObj = appxCMS.SiteDataSource.GetSite(siteID);

        string output = "";
        string iconHtml = "";
        string phoneNumber = "";
        string callUs = "";

        if (siteObj != null)
        {


            if (!String.IsNullOrEmpty(siteObj.TollFreeNumber))
            { phoneNumber = FormatPhoneNumber(siteObj.TollFreeNumber); }

            else if (!String.IsNullOrEmpty(siteObj.PhoneNumber))
            { phoneNumber = FormatPhoneNumber(siteObj.PhoneNumber); }

            else
            { phoneNumber = "0000000000"; }


            if (useIcon.ToLower() == "true")
            { iconHtml = "<span class=\"fa fa-phone\"></span>&nbsp;"; }

            if (addCallUs.ToLower() == "true")
            { callUs = "Call Us "; }


            output = iconHtml + callUs + phoneNumber;

            // Make hyperlink if needed...
            if (makeHyperLink.ToLower() == "true")
            {
                output = "<a href=\"tel:" + phoneNumber + "\">" + output + "</a>";
            }

            litSitePhoneNumber.Text = output;

        } 


    }



    public string FormatPhoneNumber(string phoneNumber)
    {

        //===================================================================================
        // This function MUST receive a 10 digit phone number in order to work properly.	
        // It returns the 10-digit integer into a format like (804) 555-5555.				
        // ===================================================================================

        string areaCode;
        string prefixDigits;
        string suffixDigits;
        string formattedNumber;

        if (String.IsNullOrEmpty(phoneNumber))
        { formattedNumber = ""; }

        else
        {
            areaCode = phoneNumber.Substring(0, 3);
            prefixDigits = phoneNumber.Substring(3, 3);
            suffixDigits = phoneNumber.Substring(6, 4);

            formattedNumber = areaCode + "-" + prefixDigits + "-" + suffixDigits;
        }

        return formattedNumber;

    }

}