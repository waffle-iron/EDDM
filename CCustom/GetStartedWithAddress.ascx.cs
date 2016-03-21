using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CCustom_GetStartedWithAddress : System.Web.UI.UserControl
{




    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void lnkGetStarted_Click(object sender, EventArgs e)
    {

        if (Page.IsValid)
        {
            Response.Redirect("/Step1-Target.aspx?addr=" + Server.UrlEncode(StreetAddress.Text) + "&zip=" + ZipCode.Text);
        }
    }


}