using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CCustom_BoldChatTextLink : System.Web.UI.UserControl
{
    
    //NOTE:  If this is to be placed into the phHeader section as a clickable, text link, use this in CMS:
    //<clibrary>[CCustom/BoldChatTextLink]</clibrary>



    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        string fileName = "";
        string fileContent = "";
        string appRoot = Server.MapPath("~/");


        switch (siteID)
        {
            


            //EDDM
            case 1:
            fileName = "BoldChatTextScript-EDDM.txt";
            break;


            //OLB
            case 11:
            fileName = "BoldChatTextScript-OLB.txt";
            break;


            //Rooterman
            case 18:
            fileName = "BoldChatTextScript-Rooterman.txt";
            break;


            //Primrose
            case 27:
            fileName = "BoldChatTextScript-Primrose.txt";
            break;


            //Fishbowl
            case 31:
            fileName = "BoldChatTextScript-Fishbowl.txt";
            break;


            //Tax Pro Marketer
            case 38:
            fileName = "BoldChatTextScript-TaxProMarketer.txt";
            break;


            //FedEx
            case 41:
            fileName = "BoldChatTextScript-FedEx.txt";
            break;


            //Massage Heights
            case 56:
            fileName = "BoldChatTextScript-MassageHeights.txt";
            break;


            //CHHJ
            case 60:
            fileName = "BoldChatTextScript-CHHJ.txt";
            break;


            //Staples Act Mgr
            case 78:
            fileName = "BoldChatTextScript-StaplesActMgr.txt";
            break;


            //Wingstop
            case 79:
            fileName = "BoldChatTextScript-Wingstop.txt";
            break;


            //Miami RE
            case 83:
            fileName = "BoldChatTextScript-EDDM.txt";
            break;


            //FedEx LIST DEMO
            case 84:
            fileName = "BoldChatTextScript-FedEx.txt";
            break;


            //Progressive
            case 90:
            fileName = "BoldChatTextScript-Progressive.txt";
            break;


            //Staples Store
            case 91:
            fileName = "BoldChatTextScript-StaplesStore.txt";
            break;


            //Staples Consumer
            case 93:
            fileName = "BoldChatTextScript-StaplesConsumer.txt";
            break;


            //Progressive Platinum
            case 94:
            fileName = "BoldChatTextScript-ProgressivePlat.txt";
            break;


            //Staples Act Mgr
            case 95:
            fileName = "BoldChatTextScript-StaplesActMgr.txt";
            break;


            //FedEx Act Mgr
            case 99:
            fileName = "BoldChatTextScript-FedExActMgr.txt";
            break;


            //Taradel DM
            case 100:
            fileName = "BoldChatTextScript-EDDM.txt";
            break;


            //Bee Safe
            case 101:
            fileName = "BoldChatTextScript-EDDM.txt";
            break;


            //Monkees
            case 102:
            fileName = "BoldChatTextScript-EDDM.txt";
            break;


            default:
            break;


            
        }

        if (!String.IsNullOrEmpty(fileName))
        {

            if (File.Exists(appRoot + "\\assets\\scripts\\" + fileName))
            {
                phBoldChatScript.Visible = true;
                fileContent = File.ReadAllText(appRoot + "\\assets\\scripts\\" + fileName);
                litBoldChatScript.Text = fileContent;
            }
        
        }




    }


}