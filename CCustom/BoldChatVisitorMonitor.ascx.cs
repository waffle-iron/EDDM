using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CCustom_BoldChatVisitorMonitor : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        string fileName = "";
        string fileContent = "";
        string appRoot = Server.MapPath("~/");

        //Create siteObj
        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);

        //If BoldChatVM Script is needed...
        if (siteDetails.UseBoldChatVMScript)
        { 


            switch (siteID)
            {

                //EDDM
                case 1:
                    fileName = "BoldChatVMScript-EDDM.txt";
                    break;


                //OLB
                case 11:
                    fileName = "BoldChatVMScript-OLB.txt";
                    break;


                //Rooterman
                case 18:
                    fileName = "BoldChatVMScript-Rooterman.txt";
                    break;


                //FedEx
                case 41:
                    fileName = "BoldChatVMScript-FedEx.txt";
                    break;


                //The Flyer
                case 57:
                    fileName = "BoldChatVMScript-TheFlyer.txt";
                    break;


                //CHHJ
                case 60:
                    fileName = "BoldChatVMScript-CHHJ.txt";
                    break;


                //CoBrand
                case 77:
                    fileName = "BoldChatVMScript-CoBrand.txt";
                    break;


                //Staples Act Mgr
                case 78:
                    fileName = "BoldChatVMScript-StaplesActMgr.txt";
                    break;


                ////FedEx LIST DEMO
                //case 84:
                //    fileName = "BoldChatVMScript-FedEx.txt";
                //    break;


                //Progressive
                case 90:
                    fileName = "BoldChatVMScript-Progressive.txt";
                    break;


                //Staples Store
                case 91:
                    fileName = "BoldChatVMScript-StaplesStore.txt";
                    break;


                //Staples Consumer
                case 93:
                    fileName = "BoldChatVMScript-StaplesConsumer.txt";
                    break;


                //Progressive Platinum
                case 94:
                    fileName = "BoldChatVMScript-ProgressivePlat.txt";
                    break;

                //Taradel DM
                case 100:
                    fileName = "BoldChatVMScript-EDDM.txt";
                    break;

                default:
                    break;

            }

            if (!String.IsNullOrEmpty(fileName))
            {

                if (File.Exists(appRoot + "\\assets\\scripts\\" + fileName))
                {
                    phBoldChatVMScript.Visible = true;
                    fileContent = File.ReadAllText(appRoot + "\\assets\\scripts\\" + fileName);
                    litBoldChatVMScript.Text = fileContent;
                }

            }

        }



    }

}