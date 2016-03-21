using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OLB_default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Profile.Cart = null;
        Profile.Save();
        Response.Redirect("TargetDataMap1.aspx");
    }
}