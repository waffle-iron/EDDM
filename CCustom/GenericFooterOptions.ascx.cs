using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FooterCustomOptions : System.Web.UI.UserControl
{

    // Pass this control any combination properties:
    //
    // hideSupportPage:         true/false            "True" will hide the support link from customer serice column. /  "False" does nothing.
    // hideFooterLogo:          true/false            "True" will hide the EDDM footer logo. / False does nothing.
    // hideBoldChat             true/false            "True" will hide the EDDM footer logo. / False does nothing.




    private string _hideSupportPage;
    public string hideSupportPage
    {

        get
        {
            if (String.IsNullOrEmpty(_hideSupportPage))
            { return "false"; }

            else
            { return _hideSupportPage; }
        }

        set
        { _hideSupportPage = value; }

    }
}