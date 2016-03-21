using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShopperApproved : System.Web.UI.UserControl
{

    //This script is used on EveryDoorDirectMail.com and is fires on the Receipt page:




    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();

        if (siteID == 1)
        {

            string fileName = "ShopperApproved-EDDM.txt";
            string appRoot = Server.MapPath("~/");
            string fileContent = File.ReadAllText(appRoot + "\\assets\\scripts\\" + fileName);
            string referralUrl = Request.RawUrl.ToLower();




            if (!String.IsNullOrEmpty(fileContent))
            {

                phtShopperApproved.Visible = true;
                litShopperApproved.Text = fileContent;

            }

        }

    }


}