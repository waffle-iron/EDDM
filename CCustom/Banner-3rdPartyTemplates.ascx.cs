using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ThirdPartyTemplates : System.Web.UI.UserControl
{

    private string _navigateUrl;
    public string navigateUrl
    {
        get
        {

            if (String.IsNullOrEmpty(_navigateUrl))
            { return "/Templates"; }

            else
            { return _navigateUrl; }

          }

        set
        { _navigateUrl = value; }
    }

    private string _brand;
    public string brand
    {
        get
        {

            if (String.IsNullOrEmpty(_brand))
            { return "Our"; }

            else
            { return _brand; }

          }

        set
        { _brand = value; }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        hypViewTemplates.NavigateUrl = "http://" + navigateUrl;
        hypViewTemplates.Text = "Get Design Help&nbsp;<span class=" + Convert.ToChar(34) + "fa fa-chevron-right" + Convert.ToChar(34) + "></span>";
        litBrand.Text = brand.Replace("_", " ");

    }


}
