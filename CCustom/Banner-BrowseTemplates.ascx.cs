using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CCustom_Banner_BrowseTemplates : System.Web.UI.UserControl
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



    protected void Page_Load(object sender, EventArgs e)
    {
        hypViewTemplates.NavigateUrl = navigateUrl;
        hypViewTemplates.Text = "VIEW TEMPLATES&nbsp;<span class=" + Convert.ToChar(34) + "fa fa-chevron-right" + Convert.ToChar(34) + "></span>";
    }


}