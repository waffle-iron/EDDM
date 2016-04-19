using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GenericMailPage : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // page header
        PageHeader.headerType = "partial";
        PageHeader.mainHeader = "MAIL";
        PageHeader.subHeader = "HOW EDDM<sup>&reg;</sup> WORKS";

    }
}