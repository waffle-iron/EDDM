using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CLibrary_ThreeEasySteps : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

        string currentMode = appxCMS.Util.AppSettings.GetString("Environment").ToLower();
        int siteID = appxCMS.Util.CMSSettings.GetSiteId();

        if (currentMode == "dev")
        {
            pnlDevButtons.Visible = true;
            pnlProdButtons.Visible = false;

            switch (siteID)
            {
                //EDDM
                case 1:
                    pnlEddmDevButtons.Visible = true;
                    break;

                //FedEx
                case 41:
                    pnlFedExDevButtons.Visible = true;
                    break;

                //fall back
                default:
                    pnlEddmDevButtons.Visible = true;
                    break;

            }

        }

        //fall back to Prod
        else
        {
            pnlDevButtons.Visible = false;
            pnlProdButtons.Visible = true;

            switch (siteID)
            {
                //EDDM
                case 1:
                    pnlEddmProdButtons.Visible = true;
                    break;

                //FedEx
                case 41:
                    pnlFedExProdButtons.Visible = true;
                    break;

                //fall back
                default:
                    pnlEddmProdButtons.Visible = true;
                    break;

            }

        }


    }

}