using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GenericDesignPage : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PageHeader.headerType = "partial";
        PageHeader.mainHeader = "DESIGN";
        PageHeader.subHeader = "3 Easy Ways to Start";

    }
}