using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GenericTemplatesPage : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PageHeader.headerType = "partial";
        PageHeader.mainHeader = "TEMPLATES";
        PageHeader.subHeader = "Select a pre-made template";
        TemplateCoverflow.DisplayMode = "TopNRandom";
        TemplateCoverflowNumberToShow = "10";
        TemplateCoverflowPageSize = "8.5x11";

    }
}