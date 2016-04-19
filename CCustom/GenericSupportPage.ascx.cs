using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GenericSupportPage : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

        // phone 
        SitePhoneNumber.useIcon = "true";
        SitePhoneNumber.addCallUs = "true";
        SitePhoneNumber.makeHyperLink = "true";

        // email
        SiteEmailAddress.showEmailUs = "true";
        SiteEmailAddress.useIcon = "true";
        
        // page header
        PageHeader.headerType = "partial";
        PageHeader.mainHeader = "SUPPORT";
        PageHeader.subHeader = "By Phone, Live Chat or Email";

    }

}