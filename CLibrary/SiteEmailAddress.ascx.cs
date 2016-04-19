using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteEmailAddress : System.Web.UI.UserControl
{

    //Pass this control the property useIcon (string) of "true" to display the icon.
    //Pass a property showEmailUs (string)  of "true" to display the "Email Us" instea of the actual email address.

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


    private string _showEmailUs;
    public string showEmailUs
    {

        get
        {
            if (String.IsNullOrEmpty(_showEmailUs))
            { return "false"; }

            else
            { return _showEmailUs; }
        }

        set
        { _showEmailUs = value; }

    }





    protected void Page_Load(object sender, EventArgs e)
    {


        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        appxCMS.Site siteObj = appxCMS.SiteDataSource.GetSite(siteID);

        string iconHtml = "";
        string displayHtml = "";


        if (siteObj != null)
        {

            if (!String.IsNullOrEmpty(siteObj.EmailAddress))
            {

                hypEmail.NavigateUrl = "mailto:" + siteObj.EmailAddress;
                hypEmail.Visible = true;



                //else
                //{ hypEmail.Text = siteObj.EmailAddress; }



                if (useIcon.ToLower() == "true")
                { iconHtml = "<span class=\"fa fa-envelope\"></span>&nbsp;"; }

                if (showEmailUs.ToLower() == "true")
                { displayHtml = "Email Us"; }
                else
                { displayHtml = siteObj.EmailAddress; }



                hypEmail.Text = iconHtml + displayHtml;

            }

        } 


    }


}