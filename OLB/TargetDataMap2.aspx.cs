using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using appxCMS;
using System.Collections.Specialized;

//===============================================================================================================
//NOTES:
//  The exitURL property is a string which is passed to the MapControl (BindMapButton method). This control is designed to 
//  return the exact same querystring parameters to this page once the map is saved/modified so these pages will continue
//  to function property.  The exitURL and MapHandler must have matching querystring parameters.
//
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


public partial class TargetDataMap2 : appxCMS.PageBase
{

    //keep this struct in sync with any page that uses the session objects
    //TargetDataMap1.aspx
    //TargetDataMap2.aspx
    //TargetDataMap3.aspx

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

    }



    // Fields
    protected int mapUSelectID = 1;
    protected bool debug = false;






    //Properties
    protected int distID
    {
        get { return appxCMS.Util.Querystring.GetInteger("d"); }
    }
    public string mapDistributionID;
    

    protected string mapAddress
    {
        get { return appxCMS.Util.Querystring.GetString("addr"); }
    }


    protected string zipCode
    {
        get { return appxCMS.Util.Querystring.GetString("zip"); }
    }



    private int _productID = 0;
    protected int productID
    {

        get
        {
            int outNumber = 0;

            bool convertProductID = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("p"), out outNumber);

            if (convertProductID)
            { _productID = outNumber; }

            return _productID;
        }

        set
        { _productID = value; }

    }



    private int _baseProductID = 0;
    protected int baseProductID
    {

        get
        {
            int outNumber = 0;

            bool convertProductID = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("bp"), out outNumber);

            if (convertProductID)
            { _baseProductID = outNumber; }

            return _baseProductID;
        }

        set
        { _baseProductID = value; }

    }



    private int _frequency = 0;
    protected int frequency
    {

        get
        {
            int outNumber = 0;

            bool convertFrequency = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("f"), out outNumber);

            if (convertFrequency)
            { _frequency = outNumber; }

            return _frequency;
        }

        set
        { _frequency = value; }

    }



    private string _franchiseBrand = "";
    protected string franchiseBrand
    {
        get
        { return _franchiseBrand; }

        set
        { _franchiseBrand = value; }
    }



    private string _location = "";
    protected string location
    {
        get
        { return _location; }

        set
        { _location = value; }

    }



    private string _exitUrl = "";
    protected string exitUrl
    {
        get
        {
            if (String.IsNullOrEmpty(_exitUrl))
            {
                string sLoc = appxCMS.Util.Querystring.GetString("l");
                int imp = appxCMS.Util.Querystring.GetInteger("i");
                string campaign = appxCMS.Util.Querystring.GetString("c");

                //no reason to use INT here just to put into the querystring  rs - 3/1/2016
                string qty = string.Empty;
                qty = RetrieveSafelyFromQueryString("q");
                qty = qty.Replace(",", "");
                //appxCMS.Util.Querystring.GetInteger("q");

                int budget = appxCMS.Util.Querystring.GetInteger("b");
                int frequency = appxCMS.Util.Querystring.GetInteger("f");
                string startDate = appxCMS.Util.Querystring.GetString("s");
                int productID = appxCMS.Util.Querystring.GetInteger("p");
                int baseProductID = appxCMS.Util.Querystring.GetInteger("bp");
                string franchiseBrand = appxCMS.Util.Querystring.GetString("t");

                _exitUrl = appxCMS.Util.urlHelp.AppRelativeToFullyQualified("~/resources/OLBUSelectEDDMExitHandler.ashx?referenceid={0}&uselectid=" + mapUSelectID + "&l=" + Server.UrlEncode(sLoc) + "&bp=" + baseProductID + "&p=" + productID + "&i=" + imp + "&c=" + Server.UrlEncode(campaign) + "&q=" + qty + "&b=" + budget + "&f=" + frequency + "&s=" + Server.UrlEncode(startDate) + "&t=" + Server.UrlEncode(franchiseBrand));
            }

            return _exitUrl;
        }
    }








    //============================================================================================================
    //Methods
    protected void Page_Load(object sender, EventArgs e)
    {

        ToggleDebugMode(debug);


        //Page Header
        PageHeader.headerType = "partial";
        PageHeader.mainHeader = "OLB";
        PageHeader.subHeader = "Your Targeted Customers";


        if (!Page.IsPostBack)
        {

            if (CheckQueryString())
            {
                //Querystring seems legit.  Do super cool stuff....
                SetOverview();
                UpdateProgressBar(50);
                franchiseBrand = Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("t"));
                location = Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("l"));
                lblUserName.Text = Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("l"));
                txtNameOfMap.Text = Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("mapName"));
                
                //distid
                int distID = Int32.Parse(HttpContext.Current.Request.QueryString.Get("d"));
                if(txtNameOfMap.Text.Length == 0)
                {
                    //retrive map name based on dist
                    txtNameOfMap.Text = RetrieveMapNameBasedOnDistID(distID);
                }


                //if is only single impression, hide frequency row.
                if (HttpContext.Current.Request.QueryString.Get("i").ToString() == "1")
                { frequencyRow.Visible = false; }


                string mapRefID = "";

                //Distribution was saved and returned from prev page.
                if (distID > 0)
                {
                    mapDistributionID = GetReferenceID(Convert.ToInt32(distID));

                    //get the saved Distribution list.
                    var oDist = Taradel.CustomerDistributions.GetDistribution(distID);

                    if (oDist != null)
                    {
                        mapRefID = oDist.ReferenceId;
                        int methodId = oDist.USelectMethodReference.ForeignKey();
                        Taradel.USelectMethod oUSelect = Taradel.Helper.USelect.GetById(methodId);

                        if (oUSelect != null)
                        {
                            imgmap.ImageUrl = oUSelect.ReviewUrl + "?referenceid=" + Server.UrlEncode(oDist.ReferenceId);

                            string baseUrl = oUSelect.ConfigurationUrl;
                            string redirectURL = exitUrl;
                            string mapUrl = baseUrl + "?saveredirect=" + Server.UrlEncode(redirectURL) + "&distid=" + distID + "&addr=" + mapAddress + "&zip=" + zipCode + "&refid=" + Server.UrlEncode(mapRefID) + "&nc=" + DateTime.Now.Ticks.ToString();

                            BindMapButton(distID, mapUrl);
                        }

                    }

                }

                else
                {
                    //DistID in querystring missing or not legit.
                    Response.Redirect("TargetDataMap1.aspx");
                }


                //r=true is only added to the querystring with the distribution was modified, saved, and returned back to this page via
                //OLBUSelectEDDMExitHandler with a new distribution ID num.
                if (String.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("r")))
                {
                    LoadSuggestedRoutes(Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("l")));
                }


                else
                {
                    //Get the ID of the new Distribution and load the repeater with it.
                    LoadCustomizedRoutes(distID.ToString());
                }

            }

            else
            { 
                //QueryString values don't check out.  Get outta here..
                Response.Redirect("TargetDataMap1.aspx"); 
            }

        }

    }



    private void ToggleDebugMode(bool debug)
    {

        if (debug)
        {
            pnlDevData.CssClass = string.Empty;
        }

    }



    private bool CheckQueryString()
    {
        //Check to make sure all required querystring params exist.
        bool results = false;
        int defaultNumber = 0;

        

        // (1) Make sure distID is present and numeric.
        if(HttpContext.Current.Request.QueryString.Get("d") != null)
        {
            bool convertDistID = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("d"), out defaultNumber);

            if (convertDistID)
            { results = true; }
        }

        else
        { 
            results = false;
            return results;
        }


        // (2) Make sure location exists.
        if (HttpContext.Current.Request.QueryString.Get("l") != null)
        { results = true; }

        else
        { 
            results = false;
            return results;
        }


        // (3) Make sure drops ("impressions") exists and is numeric.
        if (HttpContext.Current.Request.QueryString.Get("i") != null)
        {
            bool convertDrops = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("i"), out defaultNumber);

            if (convertDrops)
            { results = true; }
        }

        else
        { 
            results = false;
            return results;
        }


        // (4) Make sure Campaign type exists.
        if (HttpContext.Current.Request.QueryString.Get("c") != null)
        { results = true;  }

        else
        { 
            results = false;
            return results;
        }


        // (5) Make sure QTY ("numPieces") exists and is numeric.
        if (HttpContext.Current.Request.QueryString.Get("q") != null)
        {
            
            bool convertQTY = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("q"), out defaultNumber);

            if (convertQTY)
            { results = true; }
        }

        else
        { 
            results = false;
            return results;
        }


        // (6) Make sure budget exists and is numeric.
        if (HttpContext.Current.Request.QueryString.Get("b") != null)
        {
            bool convertBudget = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("b"), out defaultNumber);

            if (convertBudget)
            { results = true; }
        }



        // (7) Make sure frequency exists and is numeric.
        if (HttpContext.Current.Request.QueryString.Get("f") != null)
        {
            bool convertFrequency = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("f"), out defaultNumber);

            if (convertFrequency)
            { results = true; }
        }

        else
        {
            results = false;
            return results;
        }



        // (8) Make sure start date exists.
        if (HttpContext.Current.Request.QueryString.Get("s") != null)
        { results = true;  }

        else
        { 
            results = false;
            return results;
        }


        // (9) Make sure productID is present and numeric.
        if (HttpContext.Current.Request.QueryString.Get("p") != null)
        {
            bool convertProductID = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("p"), out defaultNumber);

            if (convertProductID)
            { results = true; }
        }

        else
        {
            results = false;
            return results;
        }


        // (10) Make sure baseProductID is present and numeric.
        if (HttpContext.Current.Request.QueryString.Get("bp") != null)
        {
            bool convertProductID = Int32.TryParse(HttpContext.Current.Request.QueryString.Get("p"), out defaultNumber);

            if (convertProductID)
            { results = true; }
        }

        else
        {
            results = false;
            return results;
        }


        // (11) Make sure type exists.
        if (HttpContext.Current.Request.QueryString.Get("t") != null)
        { results = true; }

        else
        {
            results = false;
            return results;
        }


        return results;
    }



    private void BindMapButton(int distID, string mapUrl)
    {
        btnShowMeMap.Attributes.Add("data-distID", distID.ToString());
        btnShowMeMap.Attributes.Add("data-src", mapUrl);
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



    private void LoadSuggestedRoutes(string ownerName)
    {

        string errorMsg = "";
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        //string selectedBrand = ddlOLBBrands.SelectedValue.ToString();
        //string selectedLocation = ddlOLBTerritories.SelectedValue.ToString();
        //string selectSQL = "EXEC pnd_admin.sp_GetOLBTargeterData @paramFranchiseName = '" + selectedBrand + "', @paramLocationName = '" + selectedLocation + "'";
        string selectSQL = "EXEC pnd_admin.sp_GetOLBTargeterData2 @paramFranchiseName = '" + franchiseBrand + "', @paramLocationName = '" + location + "'";

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

            foreach (System.Data.DataColumn col in dataTable.Columns)
            {
                col.ReadOnly = false;
            }


            foreach (DataRow dr2 in dataTable.Rows)
            {
                dr2["ResidentialTotal"] = RetrieveMapServerCountForRoute(dr2["ZipCarrierRoute"].ToString());
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



    private void LoadCustomizedRoutes(string distID)
    {

        //distID is the PK of the newly created, 'revised' distribution list.  We need to use a helper function to call over to the MapServer
        //to give us back the ReferenceID.
       
        List<RouteData> xData = new List<RouteData>();
        string selectSQL = "SELECT Selection FROM SavedSelection WHERE ReferenceID = '" + GetReferenceID(Convert.ToInt32(distID)) + "'";
        mapDistributionID = GetReferenceID(Convert.ToInt32(distID));
        
        using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MapServerConn"].ConnectionString))
        {
            oConn.Open();

            //Getting existing Distribution.  
            using (SqlCommand oCmd = new SqlCommand(selectSQL, oConn))
            {
                oCmd.ExecuteScalar();
                SqlDataReader oRdr = oCmd.ExecuteReader();

                if (oRdr.HasRows)
                {
                    while (oRdr.Read())
                    {
                        xData = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<RouteData>>(oRdr["Selection"].ToString());
                        break;
                    }
                }

                else
                {
                    rptRoutes.Visible = false;
                    pnlError.Visible = true;
                    lblError.Text = "Oops.  No routes were found." + selectSQL;
                }
                                 
            }

            oConn.Close();
        }
    
        //code to change xData to DataTable so that it will be happy...Demo in less than an hour!!!
        DataTable dt = new DataTable();
        dt.Columns.Add("Rank");
        dt.Columns.Add("ZipCode");
        dt.Columns.Add("CarrierRoute");
        dt.Columns.Add("TargetPct2");
        dt.Columns.Add("ResidentialTotal");
        dt.AcceptChanges();
        int i = 1;
        int residentialTotal = 0;

        foreach (RouteData rd in xData)
        {
            DataRow dr = dt.NewRow();
            dr["Rank"] = i;
            dr["ZipCode"] = rd.Name.Substring(0,5);
            dr["CarrierRoute"] = rd.Name.Substring(5,4);
            dr["TargetPct2"] = "n/a";
            dr["ResidentialTotal"] = rd.Residential;
            residentialTotal = residentialTotal + Int32.Parse(rd.Residential);
            dt.Rows.Add(dr);
            i++;
        }
        dt.AcceptChanges();


        hidSelected.Value = residentialTotal.ToString();
        lblSelected.Text = residentialTotal.ToString();

        //end code

        rptRoutes.DataSource = dt; // xData.ToList();
        rptRoutes.DataBind();

        //Hide this row in th Overview.  No longer valid.
        rowAvgMatch.Visible = false;
    }



    private int RetrieveDistributionResidentialTotal(int distID)
    {
        //distID is the PK of the newly created, 'revised' distribution list.  We need to use a helper function to call over to the MapServer
        //to give us back the ReferenceID.

        List<RouteData> xData = new List<RouteData>();
        string selectSQL = "SELECT Selection FROM SavedSelection WHERE ReferenceID = '" + GetReferenceID(Convert.ToInt32(distID)) + "'";

        using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MapServerConn"].ConnectionString))
        {
            oConn.Open();

            //Getting existing Distribution.  
            //To Do - dynamically build SELECT Statement.
            using (SqlCommand oCmd = new SqlCommand(selectSQL, oConn))
            {
                oCmd.ExecuteScalar();
                SqlDataReader oRdr = oCmd.ExecuteReader();

                if (oRdr.HasRows)
                {
                    while (oRdr.Read())
                    {
                        xData = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<RouteData>>(oRdr["Selection"].ToString());
                        break;
                    }
                }

                else
                {
                    rptRoutes.Visible = false;
                    pnlError.Visible = true;
                    lblError.Text = "Oops.  No routes were found." + selectSQL;
                }

            }

            oConn.Close();
        }

        //code to change xData to DataTable so that it will be happy...Demo in less than an hour!!!
        //TODO: query xData for Sum of ResidentialTotal
        DataTable dt = new DataTable();
        dt.Columns.Add("Rank");
        dt.Columns.Add("ZipCode");
        dt.Columns.Add("CarrierRoute");
        dt.Columns.Add("TargetPct2");
        dt.Columns.Add("ResidentialTotal");
        dt.AcceptChanges();
        int i = 1;
        int residentialTotal = 0;

        foreach (RouteData rd in xData)
        {
            DataRow dr = dt.NewRow();
            dr["Rank"] = i;
            dr["ZipCode"] = rd.Name.Substring(0, 5);
            dr["CarrierRoute"] = rd.Name.Substring(5, 4);
            dr["TargetPct2"] = "n/a";
            dr["ResidentialTotal"] = rd.Residential;
            residentialTotal = residentialTotal + Int32.Parse(rd.Residential);
            dt.Rows.Add(dr);
            i++;
        }
        dt.AcceptChanges();
        txtTotalResidencesFromMap.Text = residentialTotal.ToString();
        lblSelected.Text = residentialTotal.ToString();
        return residentialTotal;
    }



    private string RetrieveMapNameBasedOnDistID(int distID)
    {
        //This method accepts the newly created DistributionID from the MapServer (diff server) and looks up the map name
        string results = "";
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        string selectSQL = "SELECT TOP 1 Name FROM pnd_CustomerDistribution where DistributionID = " + distID + " AND [Deleted] = 0";
        SqlConnection myConnection = new SqlConnection(connectString);
        SqlCommand mySQLCommand = new SqlCommand();
        mySQLCommand.Connection = myConnection;
        mySQLCommand.CommandText = selectSQL;

        myConnection.Open();
        results = (String)mySQLCommand.ExecuteScalar();
        myConnection.Close();
        return results;

    }



    private string GetReferenceID(int distID)
    {
        //This method accepts the newly created DistributionID from the MapServer (diff server) and looks up the ReferenceID that will be needed
        //in the Dev Server to find the Saved Selection.

        string results = "";
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        string selectSQL = "SELECT TOP 1 [ReferenceId] FROM [pnd_CustomerDistribution] WHERE [DistributionId] = " + distID + " AND [Deleted] = 0";

        SqlConnection myConnection = new SqlConnection(connectString);
        SqlCommand mySQLCommand = new SqlCommand();

        mySQLCommand.Connection = myConnection;
        mySQLCommand.CommandText = selectSQL;

        myConnection.Open();
        results = (String)mySQLCommand.ExecuteScalar();
        myConnection.Close();

        LogWriter myLog = new LogWriter();
        myLog.RecordInLog("GetReferenceID sql: " + selectSQL);
        
        return results;
    
    }



    protected void btnbackToStep1_Click(object sender, EventArgs e)
    {

        int distID = Convert.ToInt32(HttpContext.Current.Request.QueryString.Get("d"));
        string locationName = HttpContext.Current.Request.QueryString.Get("l");
        int numDrops = Convert.ToInt16(HttpContext.Current.Request.QueryString.Get("i"));
        string campaignTarget = HttpContext.Current.Request.QueryString.Get("c");

        //q sometimes has commas - so let's account for those 3/1/2016
        string pieces = RetrieveSafelyFromQueryString("q").Replace(",", "");
        int numPieces = Convert.ToInt32(pieces);

        int budget = Convert.ToInt16(HttpContext.Current.Request.QueryString.Get("b"));
        int frequency = Convert.ToInt32(HttpContext.Current.Request.QueryString.Get("f"));
        string launchDate = HttpContext.Current.Request.QueryString.Get("s");
        string franchiseType = HttpContext.Current.Request.QueryString.Get("t");


        StringBuilder myURL = new StringBuilder();
        myURL.Append("?d=" + distID);
        myURL.Append("&l=" + Server.UrlEncode(locationName));
        myURL.Append("&i=" + numDrops);
        myURL.Append("&c=" + Server.UrlEncode(campaignTarget));
        myURL.Append("&q=" + numPieces);
        myURL.Append("&b=" + budget);
        myURL.Append("&f=" + frequency);
        myURL.Append("&s=" + Server.UrlEncode(launchDate));
        myURL.Append("&bp=" + baseProductID);
        myURL.Append("&p=" + productID);
        myURL.Append("&t=" + Server.UrlEncode(franchiseType));

        Response.Redirect("TargetDataMap1.aspx" + myURL.ToString());
    }

    
    
    protected void btnGoToStep3_Click(object sender, EventArgs e)
    {

        //Convert querystring variables AND label values to session variables and go to Step3.
        //labels (<span> tags) do not hold state.  We use hiddenfields to pass the data
        //to the session variables before going to the next page.
        string sesDistID = HttpContext.Current.Request.QueryString.Get("d");
        string sesFranchise = HttpContext.Current.Request.QueryString.Get("t");
        string sesLocation = HttpContext.Current.Request.QueryString.Get("l");
        string sesImpressions = HttpContext.Current.Request.QueryString.Get("i");
        string sesCampaign = HttpContext.Current.Request.QueryString.Get("c");
        //commenting out here because it is set below here 2/26/2016 -- >string sesQTY = HttpContext.Current.Request.QueryString.Get("q");
        string sesBudget = HttpContext.Current.Request.QueryString.Get("b");
        string sesFrequency = HttpContext.Current.Request.QueryString.Get("f");
        string sesStartDate = HttpContext.Current.Request.QueryString.Get("s");
        string sesProductID = HttpContext.Current.Request.QueryString.Get("p");
        string sesBaseProductID = HttpContext.Current.Request.QueryString.Get("bp");
        string sesMapName = txtNameOfMap.Text;

        bool sesRevisedMap = false;

        //r=revised.
        //If the revised parameter in querystring is true...
        if (HttpContext.Current.Request.QueryString.Get("r") == "true")
        { sesRevisedMap = true; }

        //new 2/24/2015 - use the distributions counts
        int mapCount = RetrieveDistributionResidentialTotal(Int32.Parse(sesDistID));
        string sesQTY = mapCount.ToString();

        CreateSessions(sesDistID, sesFranchise, sesLocation, sesImpressions, sesCampaign, sesQTY, sesBudget, sesRevisedMap, sesFrequency, sesStartDate, sesBaseProductID, sesProductID, sesMapName);
        
        
        Taradel.Customer oCust = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name);
        string CustomerId = oCust.CustomerID.ToString(); //"12345"; //TODO: get real CustomerID

       
        if(mapDistributionID == null)
        {
            mapDistributionID = GetReferenceID(Int32.Parse(sesDistID));
        }


        //new 2/27/2015 rename the map
        using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString))
        {
            oConn.Open();
            using (SqlCommand oCmd = new SqlCommand("usp_UpdateCustomerDistribution", oConn))
            {
                oCmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                oCmd.Parameters.AddWithValue("@DistName", txtNameOfMap.Text);
                oCmd.Parameters.AddWithValue("@DistRefId", mapDistributionID);
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.ExecuteScalar();
            }
            oConn.Close();
        }

        //string q = RebuildQuerystring()

        Response.Redirect("TargetDataMap3.aspx");

    }



    protected void CreateSessions(string sesDistID, string sesFranchise, string sesLocation, string sesImpressions, string sesCampaign, string sesQTY, string sesBudget, bool sesRevisedMap, string sesFrequency, string sesStartDate, string sesBaseProductID, string sesProductID, string sesMapName)
    {
        //19 possible session variables. 

        //Removed clear method b/case session "mapName" is now created on TargetDataMap1.
        //Session.Clear();        
        Session.Add(TaradelSession.AvgMatch, hidPctMatchAvg.Value);
        Session.Add(TaradelSession.Budget, sesBudget);
        Session.Add(TaradelSession.Campaign, sesCampaign);
        Session.Add(TaradelSession.DistID, sesDistID);
        Session.Add(TaradelSession.EstCost, hidAmount.Value);
        Session.Add(TaradelSession.Franchise, sesFranchise);
        Session.Add(TaradelSession.Frequency, sesFrequency);
        Session.Add(TaradelSession.Impressions, sesImpressions);
        Session.Add(TaradelSession.Location, sesLocation);
        Session.Add(TaradelSession.MapName, sesMapName);
        Session.Add(TaradelSession.PricePerPiece, hidPricePerPiece.Value);
        Session.Add(TaradelSession.ProductID, sesProductID);
        Session.Add(TaradelSession.BaseProductID, sesBaseProductID);
        Session.Add(TaradelSession.QTY, hidSelected.Value); //overriding --> 2/26/2016  //sesQTY);
        Session.Add(TaradelSession.RevisedMap, sesRevisedMap.ToString());
        Session.Add(TaradelSession.StartDate, sesStartDate);
        Session.Add(TaradelSession.Template, OLBTargeter.GetProductName(productID));
        Session.Add(TaradelSession.TotalMailed, hidTotalMailed.Value);
        Session.Add(TaradelSession.TotSelected, hidSelected.Value);
    }



    protected void btnStartOver_Click(object sender, EventArgs e)
    { Response.Redirect("TargetDataMap1.aspx"); }



    protected void SetOverview()
    {
        //11 Controls in all to match up
        lblFranchise.Text = Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("t"));
        lblLocation.Text = Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("l"));
        lblImpressions.Text = HttpContext.Current.Request.QueryString.Get("i");
        lblFrequency.Text = OLBTargeter.GetFrequencyString(frequency);
        lblTemplate.Text = OLBTargeter.GetProductName(productID);
        lblLaunchWeek.Text = "Week of " + Server.UrlDecode(HttpContext.Current.Request.QueryString.Get("s"));
        int distID2 = Int32.Parse(HttpContext.Current.Request.QueryString.Get("d"));
        hidSelected.Value = RetrieveDistributionResidentialTotal(distID2).ToString();

    }



    public class Routes
    {
        //Used to extract and use Saved routes.

        public List<RouteData> data { get; set; }
    }



    public class RouteData
    {
        //Properties
        public string Business { get; set; }
        public string FriendlyName { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string POBoxes { get; set; }
        public string Residential { get; set; }
        public string Total { get; set; }

        public RouteData() { }
    }



    //============================================================================================================
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
        catch(Exception ex)
        {
            Response.Write(ex.ToString() + "<br/>");
        }

        return returnThis;
    }


    public static string RetrieveFromSession(string name)
    {
        string returnThis = string.Empty;

       if(HttpContext.Current.Session[name] == null)
       {
            HttpContext.Current.Response.Redirect("TargetDataMap1.aspx?sesTimeout=y");
        }

       else
       {
           returnThis = HttpContext.Current.Session[name].ToString();
       }

       return returnThis;
    }



}