using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class KenshooTrackingPixels : System.Web.UI.UserControl
{

    // This control is currently embedded on the Receipt.aspx page.  It is
    // passed the two properties: orderID and orderTotal. So far, this is only used on the Staples Direct Mail site (#93).

    public int orderID 
    { get; set; }
   
    public int orderTotal 
    { get; set; }




    protected void Page_Load(object sender, EventArgs e)
    {

        int siteID = appxCMS.Util.CMSSettings.GetSiteId();
        string referralUrl = Request.RawUrl.ToLower();
        string trackingPixel = "";


        //Staples Direct Mail Site ID. 
        if (siteID == 93)
        {

            if (referralUrl.ToLower().Contains("receipt.aspx"))
            {

                pnlKenshoo.Visible = true;

                StringBuilder kenshooScript = new StringBuilder();

                kenshooScript.Append("<!-- Kenshoo Tracking Script -->" + Environment.NewLine);
                kenshooScript.Append("<script type=\"text/javascript\">" + Environment.NewLine);
                kenshooScript.Append("   var hostProtocol = ((\"https:\" == document.location.protocol) ? \"https\": \"http\");" + Environment.NewLine);
                kenshooScript.Append("   document.write('<scr'+'ipt src=\"', hostProtocol + '://534.xg4ken.com/media/getpx.php?cid=d423eabc-b7ed-4be4-9805-1b9eac5d17c5', '\" type=\"text/JavaScript\"></scr'+'ipt>');" + Environment.NewLine);
                kenshooScript.Append("</script>" + Environment.NewLine);
                kenshooScript.Append("<script type=\"text/javascript\">" + Environment.NewLine);
                kenshooScript.Append("   var params = new Array();" + Environment.NewLine);
                kenshooScript.Append("   params[0] = 'id=d423eabc-b7ed-4be4-9805-1b9eac5d17c5';" + Environment.NewLine);
                kenshooScript.Append("   params[1] = 'type=conv';" + Environment.NewLine);
                kenshooScript.Append("   params[2] = 'val=" + orderTotal + "';" + Environment.NewLine);              //<-- fill
                kenshooScript.Append("   params[3] = 'orderId=" + orderID + "';" + Environment.NewLine);             //<--- fill
                kenshooScript.Append("   params[4] = 'promoCode=';" + Environment.NewLine);
                kenshooScript.Append("   params[5] = 'valueCurrency=USD';" + Environment.NewLine);
                kenshooScript.Append("   k_trackevent(params,'534');" + Environment.NewLine);
                kenshooScript.Append("</script>" + Environment.NewLine);
                kenshooScript.Append("<!-- END Kenshoo Tracking Script -->" + Environment.NewLine);

                //Set the script
                litKenshooScript.Text = kenshooScript.ToString();
                
                trackingPixel = "<img src=\"https://534.xg4ken.com/media/redir.php?track=1&token=d423eabc-b7ed-4be4-9805-1b9eac5d17c5&type=convEDDM&val=" + orderTotal + "&orderId=" + orderID + "&promoCode=&valueCurrency=USD&GCID=&kw=&product=\" width=\"1\" height=\"1\"/>";
                litKenshooPixels.Text = trackingPixel;

            }


        }

    }


}