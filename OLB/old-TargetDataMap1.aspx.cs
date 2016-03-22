using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



//===============================================================================================================
//NOTES:
// 11 querystring parameters: 
// d = distID
// l = location
// i = impressions
// c = campaign 
// q = qty
// b = budget
// f = frequency (weeks)
// s = start date
// p = product ID 
// bp = base product ID
// t = type, aka Franchise Type
//
//
// 19 Session Variables: TODO: put in constants
// Session["sesDistID"]
// Session["sesFranchise"]
// Session["sesLocation"]
// Session["sesTemplate"]
// Session["sesImpressions"]
// Session["sesCampaign"]
// Session["sesQTY"]
// Session["sesBudget"]
// Session["sesRevisedMap"]
// Session["sesTotSelected"]
// Session["sesAvgMatch"]
// Session["sesEstCost"]
// Session["sesTotalMailed"]
// Session["sesPricePerPiece"]
// Session["sesFrequency"]
// Session["sesStartDate"]
// Session["sesProductID"] 
// Session["sesBaseProductID"] 
// Session["sesMapName"]
//===============================================================================================================
//test of commit - deployment 3/22/2016


public partial class TargetDataMap1 : appxCMS.PageBase
{

    private string _environmentMode;
    private string environmentMode
    {
        get
        {
            //SHOULD return as Dev or Prod
            if (ConfigurationManager.AppSettings["Environment"] != null)
            {
                _environmentMode = ConfigurationManager.AppSettings["Environment"].ToLower();
            }
            else {
                //Fall back value
                _environmentMode = "dev";
            }
            return _environmentMode;
        }
    }


    //keep this struct in sync with any page that uses the session objects: TargetDataMap1.aspx, TargetDataMap2.aspx, TargetDataMap3.aspx
    struct TaradelSession
    {
        public const string AvgMatch = "AvgMatch";
        public const string Budget = "Budget";
        public const string Campaign = "Campaign";
        public const string DistID = "DistID";
        public const string EstCost = "EstCost";
        public const string Franchise = "Franchise";
        public const string Frequency = "Frequency";
        public const string Impressions = "Impressions";
        public const string Location = "Location";
        public const string MapName = "MapName";
        public const string PricePerPiece = "PricePerPiece";
        public const string ProductID = "ProductID";
        public const string BaseProductID = "BaseProductID";
        public const string QTY = "QTY";
        public const string RevisedMap = "RevisedMap";
        public const string StartDate = "StartDate";
        public const string Template = "Template";
        public const string TemplateID = "TemplateID";
        public const string TotalMailed = "TotalMailed";
        public const string TotSelected = "TotSelected";
        public const string ScoobySnack = "Yummy";

    }



    //Fields
    protected bool debug = false;






    //Properties
    private int _siteID = 0;
    public int siteID
    {
        get
        { return _siteID; }

        set
        { _siteID = value; }

    }



    private string _currentMode = "dev";
    public string currentMode
    {
        get
        { return _currentMode; }

        set
        { _currentMode = value; }

    }



    protected decimal DistributionAvgPctMatch = 0;


    //Methods
    protected void Page_Init(object sender, EventArgs e)
    {  SetPageProperties();  }


    protected void Page_Load(object sender, EventArgs e)
    {
        //experiment
        Page.ClientScript.RegisterStartupScript(this.GetType(), "TemplateChanged", "TemplateChanged();", true);
        //end experiment

        ShowDebug(debug);


        if (!Page.IsPostBack)
        {

            UpdateProgressBar(25);
            FillBrands();
            FillTerritories();
            FillLaunchWeek();

            if (FromPrevStep())
            { SetDropDownsAndLabels(); }

            SetOverview();
            LoadSuggestedRoutes();
            LoadSavedMaps();
            FillProducts(); //put back in ! postback

            ddlTargetCampaign.Attributes.Add("onchange", "CampaignChanged(); Calculate();");
            ddlFrequency.Attributes.Add("onchange", "FrequencyChanged()");
            ddlLaunchWeek.Attributes.Add("onchange", "LaunchWeekChanged()");            
            ddlOLBTemplates.Attributes.Add("onchange", "TemplateChanged();");
            ddlImpressions.Attributes.Add("onchange", "ImpressionsChanged(); Calculate();");
            ddlBudget.Attributes.Add("onchange", "Calculate();");
            ddlNumPieces.Attributes.Add("onchange", "Calculate();");
            ddlSavedMaps.Attributes.Add("onchange", "SavedMapCalculate();");

            if(Request.QueryString["debug"] != null)
            {
                debug = true;
            }
            else
            {
                debug = false;
            }

            //This is a return visit from TargetDataMap2.
            if (Request.QueryString["d"] != null)
            {
                int distID = Int32.Parse(Request.QueryString["d"]);

                ddlTargetCampaign.SelectedValue = "savedmap";

                foreach (ListItem li in ddlSavedMaps.Items)
                {
                    if (Int32.Parse(li.Value.ToString()) == distID)
                    {
                        li.Selected = true;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "SavedMapCalculate", "SavedMapCalculateDelay()", true);
                        lblPctMatchAvg2.Text = DistributionAvgPctMatch.ToString("p2");
                        //decimal percent = RetrieveMatchPercentageForRoutes(distID);
                        //lblPctMatchAvg2.Text = percent.ToString("p2");
                    }

                    else
                    { li.Selected = false; }

                }
            
            }

        }

        //Control should rebuild on every page load.
        lblUserName.Text = ddlOLBTerritories.SelectedValue.ToString();
        lblLocation.Text = RetrieveLocationLogic();
        lblFranchise.Text = ddlOLBBrands.SelectedValue.ToString();

        //Page Header
        PageHeader.headerType = "partial";
        PageHeader.mainHeader = "OLB";
        PageHeader.subHeader = "Your Targeted Customers";
        
    }


    private void SetPageProperties()
    {

        //SiteID
        siteID = appxCMS.Util.CMSSettings.GetSiteId();

        //environment
        currentMode = appxCMS.Util.AppSettings.GetString("Environment").ToLower();

    }


    private string RetrieveLocationLogic()
    {
        string locationLogic = ddlOLBTerritories.SelectedValue.ToString(); //default
        //if (ddlTargetCampaign.SelectedItem.Text == "Use a Saved Map")
        //{
        //    if (ddlSavedMaps.SelectedItem != null)
        //    {
        //        if (ddlSavedMaps.SelectedItem.Value != "0")
        //        {
        //            locationLogic = ddlSavedMaps.SelectedItem.Text;
        //        }    
        //    }            
        //}

        return locationLogic;
    }


    private void FillBrands()
    {
        //rs code 1/4/2016 
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        string sql = "SELECT Distinct(Brand) as Franchise FROM pnd_OLBRouteData Order by Brand";
        List<ListItem> lstItems = new List<ListItem>();
        using (SqlConnection conn = new SqlConnection(connectString))
        {
            SqlCommand command = new SqlCommand(sql, conn);
            command.CommandText = sql;
            conn.Open();
            using (SqlDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ListItem li = new ListItem(rdr[0].ToString(), rdr[0].ToString());
                    lstItems.Add(li);
                }
            }
            conn.Close();

            //  1/4/2016 - wu - If the data doesn't include Archedeck and Outdoor Lighting Perspectives
            //                  use their data from last year   
            ListItem liArchaDeck = new ListItem("Archadeck", "Archadeck");
            ListItem liOLP = new ListItem("Outdoor Lighting Perspectives", "OLP");

            if(!(lstItems.Contains(liArchaDeck)))
            {
                lstItems.Add(liArchaDeck);
            }

            if(!(lstItems.Contains(liOLP)))
            {
                lstItems.Add(liOLP);
            }
            ddlOLBBrands.DataSource = lstItems;
            //ddlOLBBrands.DataValueField = "Franchise";
            //ddlOLBBrands.DataTextField = "Franchise";
            ddlOLBBrands.SelectedValue = "Mosquito Squad";
            ddlOLBBrands.DataBind();
        }

        //end rs code 1/4/2016

        //start suspect code
        //string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        //string getSQL = "SELECT Distinct(Brand) as Franchise FROM pnd_OLBRouteData Order by Brand";

        //SqlConnection myConnection = new SqlConnection(connectString);
        //SqlCommand getData = new SqlCommand(getSQL, myConnection);

        //myConnection.Open();
        //SqlDataReader drBrands = getData.ExecuteReader();
        //ddlOLBBrands.DataSource = drBrands;
        //ddlOLBBrands.DataValueField = "Franchise";
        //ddlOLBBrands.DataTextField = "Franchise";
        //ddlOLBBrands.SelectedValue = "Mosquito Squad";
        //ddlOLBBrands.DataBind();

        //drBrands.Close();
        //myConnection.Close();
        //end suspect code

    }


    private void LoadSavedMaps()
    {
        Taradel.Customer oCust = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name);

        List<Taradel.CustomerDistribution> oSavedList = Taradel.CustomerDistributions.GetList(oCust.CustomerID, false);
        foreach(Taradel.CustomerDistribution oDist in oSavedList)
        {
            string listItemText = oDist.Name + " (" + oDist.TotalDeliveries + ")";
            string listItemValue = oDist.DistributionId.ToString();
            ListItem li = new ListItem(listItemText, listItemValue);
            ddlSavedMaps.Items.Add(li);
        }

        ddlSavedMaps.Items.Insert(0, new ListItem("Select One", "0"));

    }


    private void FillProducts()
    {

        //New and Improved.  Cleaner.  DSF. 1/13/2016

        //Clear it
        ddlOLBTemplates.Items.Clear();


        List<Taradel.WLProduct> wlProducts = Taradel.WLProductDataSource.GetProducts();
        
        //For OLB site, we need to append the 8.5x11 with 'OLB Recommends'
        if (siteID == 11)
        {
            foreach (Taradel.WLProduct item in wlProducts)
            {
                if (item.BaseProductID == 216)
                { item.Name += " (OLB Recommends)"; }
            }
        }


        //Add the products to the DDL
        foreach (Taradel.WLProduct product in wlProducts)
        {

            ListItem prodItem = new ListItem();
            prodItem.Text = product.Name;
            prodItem.Value = product.ProductID.ToString();
            prodItem.Attributes.Add("baseprodid", product.BaseProductID.ToString());

            ddlOLBTemplates.Items.Add(prodItem);

        }

        //Bind DDL
        ddlOLBTemplates.DataBind();


        //Set the default product to 216 (8x11) in OLB
        if (siteID == 11)
        { ddlOLBTemplates.SelectedValue = "65"; }


    }


    private void FillLaunchWeek()
    {
        //Starting point.  Now.
        DateTime dropDate = DateTime.Today;

        //business logic - Thursday midnight is the cut off
        //ex: Friday, the 27th is an option until midnight on Thursday, the 19th
        //which is 8 days difference
        //start with the Friday that is at least 8 days away from today
        //  Today	    Fri after	Drops	  Week Displayed
        //  2/16/2015	2/20/2015	2/27/2015	3/2 - 3/6
        //  2/20/2015	2/27/2015	3/6/2015	3/9 - 3/13

        dropDate = DateTime.Now.AddDays(14);                                      //         2/16/15
        
        if(dropDate.DayOfWeek == DayOfWeek.Friday)
        {
            dropDate = dropDate.AddDays(1);
        }

        while (dropDate.DayOfWeek != DayOfWeek.Friday)                 
        {
            dropDate = dropDate.AddDays(1);                           
        }

        DateTime fridayAfterDropDate = dropDate.AddDays(7);           
        DateTime mondayAfterDropDate = dropDate.AddDays(3);           
        DateTime loopDate = mondayAfterDropDate;

        //Use for testing/debugging.
        //string loopTestString = "";
        //end

        int counter = 0;

        //loop 12x for 12 weeks
        do
        {
            //loopTestString += "[ddlValue: " + loopDate.ToShortDateString() + "]" + loopDate.ToLongDateString() + "-" + loopDate.AddDays(4).ToLongDateString() + "<br />";

            ListItem weekItem = new ListItem();
            weekItem.Value = loopDate.ToShortDateString();
            weekItem.Text = loopDate.ToLongDateString() + " - " + loopDate.AddDays(4).ToLongDateString();
            ddlLaunchWeek.Items.Insert(counter, weekItem);
            loopDate = loopDate.AddDays(7);
            counter++;
        }

        while (counter < 12);

        ddlLaunchWeek.DataBind();


    }

    
    private void FillTerritories()
    {
        //To Do - Pull from web config
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();


        //string selectSQL = "SELECT DISTINCT(Owner) FROM OLB_CarrierRouteHeat ORDER BY OWNER";
        string selectSQL = "SELECT Distinct(TradeAreaName) as Owner FROM pnd_OLBRouteData WHERE Brand = '" + ddlOLBBrands.SelectedItem.Text + "' Order by TradeAreaName ";
        string errorMsg = "";

        //hard code Archadeck and Outdoor Lighting Perspectives rs - 1/4/2016
        if (ddlOLBBrands.SelectedItem.Text == "Archadeck")
        {
            selectSQL = "SELECT DISTINCT(Owner) as Owner from OLB_FranchiseAssigns where Franchise = 'Archadeck' and Owned = 'Owned' order by Owner";
        }

        if (ddlOLBBrands.SelectedItem.Text == "Outdoor Lighting Perspectives")
        {
            selectSQL = "SELECT  DISTINCT(Owner) as Owner from OLB_FranchiseAssigns where Franchise = 'Outdoor Lighting Perspectives' and Owned = 'Owned' order by Owner";
        }
        



        try
        {
            SqlConnection myConnection = new SqlConnection(connectString);
            SqlDataAdapter myDataAdapter = new SqlDataAdapter(selectSQL, myConnection);
            DataSet statesDataSet = new DataSet();

            myDataAdapter.Fill(statesDataSet);
            ddlOLBTerritories.DataSource = statesDataSet;
            ddlOLBTerritories.DataTextField = "Owner";
            ddlOLBTerritories.DataValueField = "Owner";
            ddlOLBTerritories.DataBind();
            ddlOLBTerritories.Items.Insert(0, new ListItem("Select One", "n/a"));
            
            ddlOLBTerritories.SelectedIndex = 0;
            //ddlOLBTerritories.DataBind();
        }


        catch (Exception objException)
        {
            errorMsg = "The following errors occurred:<br /><br />";
            errorMsg += "<ul>";
            errorMsg += "<li>Message: " + objException.Message + "</li>";
            errorMsg += "<li>Source: " + objException.Source + "</li>";
            errorMsg += "<li>Stack Trace: " + objException.StackTrace + "</li>";
            errorMsg += "<li>Target Site: " + objException.TargetSite.Name + "</li>";
            errorMsg += "<li>SQL: " + selectSQL + "</li>";
            errorMsg += "</ul>";

            pnlError.Visible = true;
            lblError.Text = errorMsg;
        }

    }


    private bool FromPrevStep()
    {
        //Check to see if a DistID is in the QueryString. If so, then they are returning from a previous step which is 
        //most likely from the back button.

        bool results = false;
        string distID = HttpContext.Current.Request.QueryString.Get("d");

        if (!String.IsNullOrEmpty(distID))
        { results = true; }

        return results;
    
    }


    private void SetDropDownsAndLabels()
    {
        //If the user comes from Step 2 back to this page, we need to detect for querystring params and set the drop downs to 
        //what the user selected.

        int distID = Convert.ToInt32(HttpContext.Current.Request.QueryString.Get("d"));  //Saved distID at this point.
        string locationName = HttpContext.Current.Request.QueryString.Get("l");
        int numDrops = Convert.ToInt16(HttpContext.Current.Request.QueryString.Get("i"));
        string campaignTarget = HttpContext.Current.Request.QueryString.Get("c");

        //q sometimes has commas - so let's account for those 3/1/2016
        string pieces = RetrieveSafelyFromQueryString("q").Replace(",", "");
        int numPieces = Convert.ToInt32(pieces);
        int budget = Convert.ToInt32(HttpContext.Current.Request.QueryString.Get("b"));
        int frequency = Convert.ToInt32(HttpContext.Current.Request.QueryString.Get("f"));
        string startDate = Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("s"));
        string franchiseType = Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("t"));
        int productID = Convert.ToInt16(HttpContext.Current.Request.QueryString.Get("p"));
        

        if (!String.IsNullOrEmpty(locationName))
        { ddlOLBTerritories.SelectedValue = locationName; }

        if (productID > 0)
        { ddlOLBTemplates.SelectedValue = productID.ToString(); }

        if (numDrops != null)
        { ddlImpressions.SelectedValue = numDrops.ToString(); }

        if (!String.IsNullOrEmpty(campaignTarget))
        {

            ddlTargetCampaign.SelectedValue = campaignTarget;

            switch (campaignTarget)
            {
                case "OLB":
                    ddlTargetCampaign.SelectedValue = "OLB";
                    break;
                case "budget":
                    ddlTargetCampaign.SelectedValue = "budget";
                    ddlBudget.SelectedValue = budget.ToString();
                    break;
                case "numpieces":
                    ddlTargetCampaign.SelectedValue = "numpieces";
                    ddlNumPieces.SelectedValue = numPieces.ToString();
                    break;
            }


        }

        if (numPieces != null)
        { ddlNumPieces.SelectedValue = numPieces.ToString(); }

        if (budget != null)
        { ddlBudget.SelectedValue = budget.ToString(); }

        if (frequency != null)
        { ddlFrequency.SelectedValue = frequency.ToString(); }

        if (startDate != null)
        { ddlLaunchWeek.SelectedValue = startDate.ToString(); }

        if (franchiseType != null)
        { ddlOLBBrands.SelectedValue = franchiseType; }


        lblTemplate.Text = OLBTargeter.GetProductName(productID);
        lblFrequency.Text = OLBTargeter.GetFrequencyString(frequency);
        lblImpressions.Text = numDrops.ToString();
        lblLaunchWeek.Text = "Week of " + startDate;

    }


    private int RetrieveMapServerCountForRoute(string CarrierRoute)
    {
        int mapCount = 0;

        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["MapServerConn"].ToString();
        string errorMsg = "";
        string mapSql = "SELECT ResidentialTotal FROM CarrierRouteDistribution WHERE GeoCodeRef = '" + CarrierRoute + "'";

        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand comm = new SqlCommand(mapSql, conn);
                conn.Open();
                using (SqlDataReader rdr = comm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        mapCount = Int32.Parse(rdr[0].ToString());
                    }
                }
                conn.Close();
            }           
        }
        catch (Exception objException)
        {
            errorMsg = "The following errors occurred:<br /><br />";
            errorMsg += "<ul>";
            errorMsg += "<li>Message: " + objException.Message + "</li>";
            errorMsg += "<li>Source: " + objException.Source + "</li>";
            errorMsg += "<li>Stack Trace: " + objException.StackTrace + "</li>";
            errorMsg += "<li>Target Site: " + objException.TargetSite.Name + "</li>";
            errorMsg += "<li>SQL: " + mapSql + "</li>";
            errorMsg += "</ul>";

            //txtDebug.Text = errorMsg;
            pnlError.Visible = true;
            lblError.Text = errorMsg;
        }


        //Response.Write("MapCount: " + mapCount.ToString() + "<br/>");
        
        return mapCount;
    }


    private void LoadSuggestedRoutes()
    {

        string errorMsg = "";
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        string selectedBrand = ddlOLBBrands.SelectedValue.ToString();
        string selectedLocation = ddlOLBTerritories.SelectedValue.ToString();
        if(selectedLocation == "n/a")
        {
            return;
        }
        //string selectSQL = "EXEC pnd_admin.sp_GetOLBTargeterData @paramFranchiseName = '" + selectedBrand + "', @paramLocationName = '" + selectedLocation + "'";
        string selectSQL = "EXEC pnd_admin.sp_GetOLBTargeterData2 @paramFranchiseName = '" + selectedBrand + "', @paramLocationName = '" + selectedLocation + "'";
        
        SqlConnection myConnection = new SqlConnection(connectString);
        SqlCommand getData = new SqlCommand(selectSQL, myConnection);

        try
        {
            myConnection.Open();
            DataTable dataTable = new DataTable();

            SqlDataReader dr = getData.ExecuteReader(CommandBehavior.CloseConnection);
            dataTable.Load(dr);
            dataTable.AcceptChanges();
            //Response.Write("rows:" + dataTable.Rows.Count.ToString());


            if (environmentMode == "dev")  //retrieve the most accurate counts from MapServer for testing Dev purposes
            {
                foreach (System.Data.DataColumn col in dataTable.Columns)
                {
                    col.ReadOnly = false;
                }

                foreach (DataRow dr2 in dataTable.Rows)
                {
                    dr2["ResidentialTotal"] = RetrieveMapServerCountForRoute(dr2["ZipCarrierRoute"].ToString());
                }
            }

            dataTable.AcceptChanges();
            //For debugging
            //LogWriter myLog = new LogWriter();
            //myLog.RecordInLog("Connect String: " + connectString);
            //myLog.RecordInLog("SQL: " + selectSQL);

            if (dataTable.Rows.Count == 0)
            {
                rptRoutes.Visible = false;
                pnlError.Visible = true;
                lblError.Text = "Oops.  No routes were found." + selectSQL.ToString();
            }

            else
            {

                int distId = appxCMS.Util.Querystring.GetInteger("distid");
                if (distId == 0)
                {
                    distId = appxCMS.Util.Querystring.GetInteger("d");
                }
                if (distId > 0)
                {
                    int iTotal = 0;
                    decimal dRunPct = 0;

                    var oDist = Taradel.CustomerDistributions.GetDistribution(distId);

                    if (oDist != null)
                    {
                        var oSelectAreas = Taradel.CustomerDistributions.GetSelections(oDist.ReferenceId);
                        if (oSelectAreas != null)
                        {
                            foreach (DataRow oRow in dataTable.Rows)
                            {
                                string sCr = oRow["ZipCarrierRoute"].ToString();
                                var oSel = oSelectAreas.FirstOrDefault(a => a.Name.Equals(sCr, StringComparison.OrdinalIgnoreCase));

                                if (oSel != null)
                                {
                                    decimal dCurPct = 0;
                                    decimal.TryParse(oRow["TargetPct2"].ToString(), out dCurPct);
                                    iTotal += 1;
                                    dRunPct += dCurPct;
                                }
                            }

                            if (iTotal > 0 && dRunPct > 0)
                            {
                                DistributionAvgPctMatch = dRunPct/iTotal;
                            }
                        }
                    }
                }

                rptRoutes.DataSource = dataTable;
                rptRoutes.DataBind();

            }

        }

        catch (Exception objException)
        {
            errorMsg = "The following errors occurred:<br /><br />";
            errorMsg += "<ul>";
            errorMsg += "<li>Message: " + objException.Message + "</li>";
            errorMsg += "<li>Source: " + objException.Source + "</li>";
            errorMsg += "<li>Stack Trace: " + objException.StackTrace + "</li>";
            errorMsg += "<li>Target Site: " + objException.TargetSite.Name + "</li>";
            errorMsg += "<li>SQL: " + selectSQL.ToString() + "</li>";
            errorMsg += "</ul>";

            pnlError.Visible = true;
            lblError.Text = errorMsg;
        }

        finally
        { myConnection.Close(); };

    }


    private void UpdateProgressBar(int progressVal)
    {
        StringBuilder progressHTML = new StringBuilder();

        progressHTML.Append("<div aria-valuemax=" + Convert.ToChar(34) + "100" + Convert.ToChar(34) + " ");
        progressHTML.Append("aria-valuemin=" + Convert.ToChar(34) + "0" + Convert.ToChar(34) + " aria-valuenow=" + Convert.ToChar(34) + progressVal + Convert.ToChar(34) + " ");
        progressHTML.Append("class=" + Convert.ToChar(34) + "progress-bar progress-bar-success" + Convert.ToChar(34) + " role=" + Convert.ToChar(34) + "progressbar" + Convert.ToChar(34) + " ");
        progressHTML.Append("style=" + Convert.ToChar(34) + "width: " + progressVal + "%;" + Convert.ToChar(34) + ">" + progressVal + "%" + "</div>");

        litProgressBar.Text = progressHTML.ToString();
    }


    private string RetrieveTheReferenceID(int distributionID)
    {
        string returnThis = string.Empty;
        //To Do - Pull from web config
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        string errorMsg = "";
        string sql = "select ReferenceId From pnd_CustomerDistribution WHERE DistributionID = " + distributionID.ToString();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);
                conn.Open();
                object result = comm.ExecuteScalar();
                if (result != null)
                {
                    returnThis = result.ToString();
                }
            }
        }

        catch (Exception objException)
        {
            errorMsg = "The following errors occurred:<br /><br />";
            errorMsg += "<ul>";
            errorMsg += "<li>Message: " + objException.Message + "</li>";
            errorMsg += "<li>Source: " + objException.Source + "</li>";
            errorMsg += "<li>Stack Trace: " + objException.StackTrace + "</li>";
            errorMsg += "<li>Target Site: " + objException.TargetSite.Name + "</li>";
            errorMsg += "<li>SQL: " + sql + "</li>";
            errorMsg += "</ul>";
            pnlError.Visible = true;
            lblError.Text = errorMsg;
        }

        return returnThis;

    }


    protected decimal RetrieveMatchPercentageForRoutes(int distributionID)
    {
        decimal returnThis = 0;

        //Step 1. Retrieve the Distribution ID
        //select ReferenceId From pnd_CustomerDistribution WHERE DistributionID = 86360-- has referenceid
        //This is stored in the pnd_CustomerDistribution table of the site's regular database
        string referenceID = string.Empty;
        //string sql = "select ReferenceId From pnd_CustomerDistribution WHERE DistributionID = " + distributionID.ToString();
        referenceID = RetrieveTheReferenceID(distributionID);


        //Step 2. Retrieve the ZipCode/Route date from the Map Server
        //        Pull apart the JSON in the SELECTION and put it into a table as a TVP for usp_RetrieveEmailCountForRoutes
        DataTable dt = RetrieveTheZipRouteTable(referenceID);
        string franchiseName = ddlOLBBrands.SelectedItem.Text;
        string testValue = string.Empty;

        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        string selectSQL = "usp_RetrievePercentMatchForRoutes2"; // "select cust.CustomerID as CustomerID, UserName, isnull(ProfileID,0) as ProfileID, isnull(PaymentProfileID,0) as PaymentProfileID from pnd_Customer cust INNER JOIN pnd_AuthorizePaymentProfile auth ON cust.CustomerID = auth.CustomerID WHERE UserName = '" + email + "'";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(selectSQL, conn);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter paramFranchiseName = command.Parameters.AddWithValue("@paramFranchiseName", franchiseName);

                SqlParameter tvpParam = command.Parameters.AddWithValue("@tvpRoutes", dt);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "dbo.RouteTableType";
                command.CommandText = selectSQL;
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        testValue = rdr[0].ToString();
                        returnThis = decimal.Parse(rdr[0].ToString());
                    }
                }
                conn.Close();
            }
        }
        catch (Exception objException)
        {
            gvDebug.DataSource = dt;
            gvDebug.DataBind();
            string errorMsg = "The following errors occurred:<br /><br />";
            errorMsg += "<ul>";
            errorMsg += "<li>Message: " + objException.Message + "</li>";
            errorMsg += "<li>Source: " + objException.Source + "</li>";
            errorMsg += "<li>Stack Trace: " + objException.StackTrace + "</li>";
            errorMsg += "<li>Target Site: " + objException.TargetSite.Name + "</li>";
            errorMsg += "<li>testValue: " + testValue + "</li>";

            errorMsg += "<li>SQL: " + selectSQL + "</li>";
            errorMsg += "</ul>";
            lblError.Text = errorMsg;
            pnlError.Visible = true;
        }
        returnThis = returnThis / 100;
        Response.Write("RetrieveMatchPct: " + returnThis.ToString());
        return returnThis;
    }


    private DataTable RetrieveTheZipRouteTable(string referenceID)
    {
        DataTable dt = new DataTable();
        string jsonString = string.Empty;
        dt.Columns.Add("ZipRoute");
        dt.AcceptChanges();

        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["MapServerConn"].ToString(); 
        string errorMsg = "";
        string mapSql = "select SELECTION FROM savedSelection where ReferenceID = '" + referenceID + "'";

        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand comm = new SqlCommand(mapSql, conn);
                conn.Open();
                using (SqlDataReader rdr = comm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        jsonString = (rdr[0].ToString());
                    }
                }
                conn.Close();
            }

            //        Pull apart the JSON in the SELECTION and put it into a table as a TVP for usp_RetrieveEmailCountForRoutes
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Route> lRoutes = new List<Route>();
            var arrayOfRoutes = js.Deserialize<Route[]>(jsonString);
            foreach (Route r in arrayOfRoutes)
            {
                DataRow dr = dt.NewRow();
                dr["ZipRoute"] = r.Name;
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
        }
        catch (Exception objException)
        {
            errorMsg = "The following errors occurred:<br /><br />";
            errorMsg += "<ul>";
            errorMsg += "<li>Message: " + objException.Message + "</li>";
            errorMsg += "<li>Source: " + objException.Source + "</li>";
            errorMsg += "<li>Stack Trace: " + objException.StackTrace + "</li>";
            errorMsg += "<li>Target Site: " + objException.TargetSite.Name + "</li>";
            errorMsg += "<li>SQL: " + mapSql + "</li>";
            errorMsg += "</ul>";
            
            //txtDebug.Text = errorMsg;
            pnlError.Visible = true;
            lblError.Text = errorMsg;
        }


        return dt;
    }


    public class Routes
    {
        public List<Route> lstRoutes { get; set; }
    }


    public class Route
    {
        public string Business { get; set; }
        public string FriendlyName { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string POBoxes { get; set; }
        public string Residential { get; set; }
        public string Total { get; set; }

    }


    protected void btnGoToStep2_Click(object sender, EventArgs e)
    {


        //Product ID
        //foreach(ListItem li in ddlOLBTemplates.Items)
        //{
        //    Response.Write(li.Text + ":" + li.Selected.ToString() + "<br/>");
        //}
        int productID = Convert.ToInt32(ddlOLBTemplates.SelectedValue);
        //Response.Write("productID:" + productID.ToString());
        //Response.End();


        //BASE ProductID. We also need the BaseProductID which will be passed in the querystring (for the QuoteHandler)
        Taradel.WLProduct productObj = Taradel.WLProductDataSource.GetProduct(productID);
        int baseProductID = Convert.ToInt32(productObj.BaseProductID);


        //Grab values from drop down lists and send values in querystring.
        //Look for a previous Saved Map.
        int savedDistID = 0;

        if (!String.IsNullOrEmpty(ddlSavedMaps.SelectedValue))
        { savedDistID = Int32.Parse(ddlSavedMaps.SelectedItem.Value); }



        //If user did not selected Saved Map
        if(ddlSavedMaps.SelectedItem.Value == "0")
        { savedDistID = SaveDistribution(); }

        else
        {
            string mapName = ddlSavedMaps.SelectedItem.Text.Substring(0, ddlSavedMaps.SelectedItem.Text.IndexOf("("));
            Session["sesMapName"] = mapName;
            Session["sesDistRefId"] = savedDistID;
        }

        string locationName = RetrieveLocationLogic();
        int numDrops = Convert.ToInt16(ddlImpressions.SelectedValue);
        string campaignTarget = ddlTargetCampaign.SelectedValue;
        int numPieces = Convert.ToInt32(ddlNumPieces.SelectedValue);
        int budget = Convert.ToInt32(ddlBudget.SelectedValue);
        int frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
        string launchDate = ddlLaunchWeek.SelectedValue.ToString();
        string franchiseType = ddlOLBBrands.SelectedValue.ToString();

        //Response.Write(Session[xSelected].ToString());
        StringBuilder myURL = new StringBuilder();
        myURL.Append("?d=" + savedDistID);
        myURL.Append("&l=" + Server.UrlEncode(locationName));
        myURL.Append("&i=" + numDrops );
        myURL.Append("&c=" + Server.UrlEncode(campaignTarget));
        myURL.Append("&q=" + numPieces);
        myURL.Append("&b=" + budget);
        myURL.Append("&f=" + frequency);
        myURL.Append("&s=" + Server.UrlEncode(launchDate));
        myURL.Append("&p=" + productID);
        myURL.Append("&bp=" + baseProductID);
        myURL.Append("&t=" + Server.UrlEncode(franchiseType));
        myURL.Append("&mapName=" + Session["sesMapName"].ToString()); //overkill? change to page-level variable?

        if ((ddlSavedMaps.SelectedItem.Value != "0") && (ddlTargetCampaign.SelectedItem.Value == "savedmap"))
        {
            myURL.Append("&r=true"); //treating a saved map as a revised map since we are not using the regular logic on the next page.
        }

        Response.Redirect("TargetDataMap2.aspx" + myURL.ToString());

    }

    
    private int SaveDistribution()
    {
        //Needs to Save Distribution data and return the ID
        //txtDebug.Text += "SaveDistribution start " + Environment.NewLine;
        int distID = ProcessTheRepeater();//84424;
        //txtDebug.Text += "SaveDistribution end " + distID.ToString() + Environment.NewLine;
        return distID;

    }

    
    public int ProcessTheRepeater()
    {
        //txtDebug.Text += "rptRoutes.count:" + rptRoutes.Items.Count.ToString() + Environment.NewLine;
        
        //the value to be returned...
        int distID = 0;
        StringBuilder txtPaste = new StringBuilder();
        int ii = 0;
        List<string> theRoutes = new List<string>(); //for preventing duplicates

        foreach (RepeaterItem item in rptRoutes.Items)
        {
            ii++;
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                var chkInclude = (CheckBox)item.FindControl("chkInclude");
                var lblZipRoute = (Label)item.FindControl("lblZipRoute");
                var theRouteToAdd = lblZipRoute.Text;

                if (chkInclude.Checked)
                {
                    if (theRoutes.Contains(theRouteToAdd))
                    {
                        //do not add duplicates
                    }
                    else
                    {
                        txtPaste.AppendLine(theRouteToAdd);
                    }
                }

                else 
                { }

            }

            //lblError.Text += "processing repeater:" + txtPaste.ToString() + Environment.NewLine;
        }
       
        ////////////////////////////////////////////////////////////////
        string[] aPasteData = txtPaste.ToString().Split('\n');
        List<Taradel.MapServer.UserData.AreaSelection> oSelects = new List<Taradel.MapServer.UserData.AreaSelection>();
        string sDistRefId = System.Guid.NewGuid().ToString();
        string PasteDataName = "OLB_" + DateTime.Now.Ticks.ToString();
        Session["sesMapName"] = PasteDataName;
        Session["sesDistRefId"] = sDistRefId;
        Taradel.Customer oCust = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name);
        string CustomerId = oCust.CustomerID.ToString(); //"12345"; //TODO: get real CustomerID
        //if(oCust)

        int iBizTotal = 0;
        int iResTotal = 0;
        int iBoxTotal = 0;

        using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MapServerConn"].ConnectionString))
        {
            oConn.Open();
            for (int i = 0; i <= aPasteData.Length - 1; i++)
            {
                string sZip = aPasteData[i].Trim();

                if(sZip.IndexOf("B") > 0)
                {
                    continue; //go to the next one in the loop is this a box route
                }
                if (!string.IsNullOrEmpty(sZip))
                {
                    //-- Load all of the carrier routes in this zip code
                    try
                    {
                        
                        //Modified 3/19/15 DSF
                        //Added (BoxCount = 0) clause to select statement to filter out POBoxes
                        //-- Account for zip code or zip + carrier route name
                        string sSql = "select TOP 1 GeocodeRef, City + ', ' + State As Name, ResidentialTotal, BusinessActive, BoxCount from CarrierRouteDistribution WHERE ZipCode='" + sZip + "' AND [BoxCount] = 0";

                        if (sZip.Length > 5)
                        {
                            sZip = sZip.Replace(" ", "");
                            sSql = "select TOP 1 GeocodeRef, City + ', ' + State As Name, ResidentialTotal, BusinessActive, BoxCount from CarrierRouteDistribution WHERE GeocodeRef='" + sZip + "' AND [BoxCount] = 0";
                        }

                        using (SqlCommand oCmd = new SqlCommand(sSql.ToString(), oConn))
                        {
                            SqlDataReader oRdr = oCmd.ExecuteReader();

                            if (oRdr.HasRows)
                            {
                                while (oRdr.Read())
                                {
                                    Taradel.MapServer.UserData.AreaSelection oArea = new Taradel.MapServer.UserData.AreaSelection();
                                    oArea.Name = oRdr.GetString(0);
                                    oArea.FriendlyName = oRdr.GetString(1);
                                    oArea.Residential = oRdr.GetInt32(2);
                                    oArea.Business = 0;
                                    oArea.POBoxes = 0;

                                    //Don't need --> OLB is all residential
                                    //if (chkPasteBusiness.Checked)
                                    //{
                                    //    oArea.Business = oRdr.GetInt32(3);
                                    //}
                                    //else
                                    //{
                                    //    oArea.Business = 0;
                                    //}
                                    //if (chkPastePOBoxes.Checked)
                                    //{
                                    //    oArea.POBoxes = oRdr.GetInt32(4);
                                    //}
                                    //else
                                    //{
                                    //    oArea.POBoxes = 0;
                                    //}
                                    oArea.Total = oArea.Business + oArea.Residential + oArea.POBoxes;

                                    if (oArea.Total > 0)
                                    {
                                        oSelects.Add(oArea);
                                        iResTotal = iResTotal + oArea.Residential;
                                        iBizTotal = iBizTotal + oArea.Business;
                                        iBoxTotal = iBoxTotal + oArea.POBoxes;
                                    }
                                }
                            }
                            oRdr.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        //not sure about this---
                        pnlError.Visible = true;
                        lblError.Text = ex.StackTrace.ToString();

                    }
                }
            }
        } //end  using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MapServerConn"].ConnectionString))

        string sSelects = Taradel.JavascriptSerializer.Serialize<List<Taradel.MapServer.UserData.AreaSelection>>(oSelects);

        using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MapServerConn"].ConnectionString))
        {
            oConn.Open();

            using (SqlCommand oCmd = new SqlCommand("INSERT INTO SavedSelection (ReferenceId, CreatedDate, CreatedIPAddress, Name, StartAddress, StartZipCode, UseBusiness, UsePOBoxes, Selection) VALUES (@DistRefId, GetDate(), @IPAddress, @DistName, '', '', @UseBusiness, @UsePOBox, @Dist)", oConn))
            {
                oCmd.Parameters.AddWithValue("@DistRefId", sDistRefId);
                oCmd.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress);
                oCmd.Parameters.AddWithValue("@DistName", PasteDataName);

                oCmd.Parameters.AddWithValue("@UseBusiness", false);
                oCmd.Parameters.AddWithValue("@UsePOBox", false);
                oCmd.Parameters.AddWithValue("@Dist", sSelects);

                //Temp
                //LogWriter myLog = new LogWriter();

                //myLog.RecordInLog("TargetDataMap1");
                //myLog.RecordInLog("sDistRefId: " + sDistRefId);
                //myLog.RecordInLog("IPAddress: " + HttpContext.Current.Request.UserHostAddress);
                //myLog.RecordInLog("DistName: " + PasteDataName);
                //myLog.RecordInLog("Dist: " + sSelects);
                //End Temp


                oCmd.ExecuteScalar();
            }

            oConn.Close();

        } 

        using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString))
        {
            oConn.Open();
            using (SqlCommand oCmd = new SqlCommand("usp_InsertCustomerDistribution", oConn))
            {
                oCmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                oCmd.Parameters.AddWithValue("@DistName", PasteDataName);
                oCmd.Parameters.AddWithValue("@DistRefId", sDistRefId);
                oCmd.Parameters.AddWithValue("@TotalDeliveries", iResTotal + iBizTotal + iBoxTotal);
                oCmd.Parameters.AddWithValue("@USelectId", 1);
                oCmd.Parameters.Add("@OutputID", SqlDbType.Int).Direction = ParameterDirection.Output;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.ExecuteScalar();
                distID = Convert.ToInt32(oCmd.Parameters["@OutputID"].Value);
            }
            oConn.Close();

        }

        return distID;

    }

    
    private void SetOverview()
    {
        //Response.Write("SetOverview");
        //Initialize the labels when page first loads.  11 controls in all to match up.
        lblFranchise.Text = ddlOLBBrands.SelectedValue;
        lblLocation.Text = RetrieveLocationLogic();
        lblSelected.Text = "0";
        //lblPctMatchAvg.Text = "0.0";
        lblAmount.Text = "0.00";
        lblTotalMailed.Text = "0";
        lblPricePerPiece.Text = "0.00";
        lblLaunchWeek.Text = "Week of " + ddlLaunchWeek.SelectedValue.ToString();
        int temp = 0;
        if(Int32.TryParse(ddlOLBTemplates.SelectedValue.ToString(), out temp))
        {
            lblTemplate.Text = OLBTargeter.GetProductName(Convert.ToInt32(ddlOLBTemplates.SelectedValue.ToString()));
        }
        lblImpressions.Text = "3";
        lblFrequency.Text = "Every 4 Weeks";

    }
       

    protected void ddlOLBTerritories_SelectedIndexChanged(object sender, EventArgs e)
    { LoadSuggestedRoutes(); }
    

    protected void ddlOLBBrands_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillTerritories();
        LoadSuggestedRoutes(); 
    }


    protected void ddlSavedMaps_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        int distID = Int32.Parse(ddlSavedMaps.SelectedItem.Value);
        string url = Request.Url.ToString();
        string path = string.Empty;

        if (url.IndexOf("?") > 0)
        {
            path = url.Substring(0, url.IndexOf("?"));
        }
        else
        {
            path = url;
        }


        //Experimental
        // Here, we're going to save all the current selections and pass them in the querystring
        // so when the page reloads, the selections will be saved.
        string locationName = ddlOLBTerritories.SelectedValue;
        string productID = ddlOLBTemplates.SelectedValue;
        string numDrops = ddlImpressions.SelectedValue;
        string campaignTarget = ddlTargetCampaign.SelectedValue;
        string budget = ddlBudget.SelectedValue;
        string numPieces = ddlNumPieces.SelectedValue;
        
        //Set these even though the may be hidden
        switch (campaignTarget)
        {
            case "OLB":
                ddlTargetCampaign.SelectedValue = "OLB";
                break;
            case "budget":
                ddlTargetCampaign.SelectedValue = "budget";
                ddlBudget.SelectedValue = budget.ToString();
                break;
            case "numpieces":
                ddlTargetCampaign.SelectedValue = "numpieces";
                ddlNumPieces.SelectedValue = numPieces.ToString();
                break;
        }

        string frequency = ddlFrequency.SelectedValue;
        string startDate = ddlLaunchWeek.SelectedValue;
        string franchiseType = ddlOLBBrands.SelectedValue;

        StringBuilder urlRedirect = new StringBuilder();
        urlRedirect.Append(path);
        urlRedirect.Append("?distid=" + distID.ToString());
        urlRedirect.Append("&d=" + distID.ToString());
        urlRedirect.Append("&l=" + Server.UrlEncode(locationName));
        urlRedirect.Append("&i=" + numDrops.ToString());
        urlRedirect.Append("&c=" + Server.UrlEncode(campaignTarget));
        urlRedirect.Append("&q=" + numPieces);
        urlRedirect.Append("&b=" + budget);
        urlRedirect.Append("&f=" + frequency);
        urlRedirect.Append("&s=" + Server.UrlEncode(startDate));
        urlRedirect.Append("&p=" + productID);
        urlRedirect.Append("&t=" + franchiseType);

        Response.Redirect(urlRedirect.ToString());
        // End of experimental


        //Response.Redirect(path + "?distid=" + distID.ToString());
        
        
        //decimal percent = RetrieveMatchPercentageForRoutes(distID);
        //lblPctMatchAvg2.Text = RetrieveMatchPercentageForRoutes(distID).ToString("p2");

    }


    public string RetrieveSafelyFromQueryString(string name)
    {
        string returnThis = string.Empty;

        try
        {
            if (Request.QueryString[name] != null)
            {
                returnThis = Request.QueryString[name].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString() + "<br/>");
        }

        return returnThis;
    }



    //Debug
    private void ShowDebug(bool debug)
    {

        if (debug)
        {
            StringBuilder pageProps = new StringBuilder();

            pnlDebug.Visible = true;
            litDebug.Text = "";

            pageProps.Append("currentMode: " + currentMode + "</br />");
            pageProps.Append("siteID: " + siteID + "</br />");

            litPageProps.Text = pageProps.ToString();

            pnlDevData.CssClass = string.Empty;

        }

    }

    

}