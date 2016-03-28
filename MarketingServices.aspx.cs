using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Taradel;
using log4net;
using System.Data;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using System.Xml.Linq;

public partial class MarketingServices : appxCMS.PageBase
{

    //=======================================================================================================================================================
    // NOTES:
    //
    //  6/10/2015 - This page is cannot be dependent on Sessions or QueryString parameters. All data is pulled from Xml Cart (Profile).
    //
    //  For the time being, these upsell services (Targeted Emails, New Movers, Addressed AddOns) are ONLY available under with a EDDM
    //  Campaign (USelectID 1).  They are not currently offered with AddressedList campaigns.
    //
    //  Furthermore, ONLY OLB can offer AddressedAdOns at this time. 3/16/2016. Will need to revisit.
    //
    //=======================================================================================================================================================


    //Global Properties
    private int _siteID = 0;
    public int siteID
    {
        get
        { return _siteID; }

        set
        { _siteID = value; }

    }


    private int _distributionID = 0;
    public int distributionID
    {
        get
        { return _distributionID; }

        set
        { _distributionID = value; }

    }


    private string _referenceID = "";
    public string referenceID
    {
        get
        { return _referenceID; }

        set
        { _referenceID = value; }

    }


    private string _currentMode = "dev";
    public string currentMode
    {
        get
        { return _currentMode; }

        set
        { _currentMode = value; }

    }


    private bool _testMode = false;
    public bool testMode
    {
        get
        { return _testMode; }

        set
        { _testMode = value; }

    }


    private int _uSelectID = 0;
    public int uSelectID
    {
        get
        { return _uSelectID; }

        set
        { _uSelectID = value; }

    }


    private bool _eddmMap = false;
    public bool eddmMap
    {
        get
        { return _eddmMap; }

        set
        { _eddmMap = value; }

    }


    private bool _addressedMap = false;
    public bool addressedMap
    {
        get
        { return _addressedMap; }

        set
        { _addressedMap = value; }

    }


    private int _productID = 0;
    public int productID
    {
        get
        { return _productID; }

        set
        { _productID = value; }

    }


    private int _baseProductID = 0;
    public int baseProductID
    {
        get
        { return _baseProductID; }

        set
        { _baseProductID = value; }

    }


    private string _productName = "";
    public string productName
    {
        get
        { return _productName; }

        set
        { _productName = value; }

    }


    private double _totalSelected = 0;
    public double totalSelected
    {
        //Total Selected in all routes per drop/impression.
        get
        { return _totalSelected; }

        set
        { _totalSelected = value; }

    }


    private int _numImpressions = 10;
    public int numImpressions
    {
        get
        { return _numImpressions; }

        set
        { _numImpressions = value; }

    }


    private int _extraCopies = 0;
    public int extraCopies
    {
        get
        { return _extraCopies; }

        set
        { _extraCopies = value; }

    }

    //EDDM Postage rate - needed??
    public decimal postageRate
    {

        get
        {
            decimal defaultRate = 0.16M;
            decimal configRate = 0;

            if (!Decimal.TryParse(appxCMS.Util.AppSettings.GetDecimal("USPSPostageRate").ToString(), out defaultRate))
            { return defaultRate; }

            else
            {
                configRate = Convert.ToDecimal(appxCMS.Util.AppSettings.GetDecimal("USPSPostageRate").ToString());
                return configRate;
            }

        }

    }


    private decimal _productPrice = 0;
    public decimal productPrice
    {
        //includes the drop fees (stored in Xml Cart).
        get
        { return _productPrice; }

        set
        { _productPrice = value; }

    }


    private decimal _mailingCost = 0;
    public decimal mailingCost
    {
        get
        { return _mailingCost; }

        set
        { _mailingCost = value; }

    }


    private decimal _designFee = 0;
    public decimal designFee
    {
        get
        { return _designFee; }

        set
        { _designFee = value; }

    }


    private string _zipCode = "";
    public string zipCode
    {
        get
        { return _zipCode; }

        set
        { _zipCode = value; }

    }





    //Addressed AddOn props
    private bool _showAddressedListAddOns = false;
    public bool showAddressedListAddOns
    {

        get
        { return _showAddressedListAddOns; }

        set
        { _showAddressedListAddOns = value; }

    }


    private int _addOnAddressedProspects = 0;
    public int addOnAddressedProspects
    {
        get
        { return _addOnAddressedProspects; }

        set
        { _addOnAddressedProspects = value; }

    }


    private double _addressedMailMarkUp = 0;
    public double addressedMailMarkUp
    {
        get
        { return _addressedMailMarkUp; }

        set
        { _addressedMailMarkUp = value; }

    }


    private string _addressedMailMarkUpType = "";
    public string addressedMailMarkUpType
    {
        get
        { return _addressedMailMarkUpType; }

        set
        { _addressedMailMarkUpType = value; }

    }


    private int _addressedAddOnBaseProductID = 0;
    public int addressedAddOnBaseProductID
    {
        get
        { return _addressedAddOnBaseProductID; }

        set
        { _addressedAddOnBaseProductID = value; }

    }


    private int _addressedAddOnProductID = 0;
    public int addressedAddOnProductID
    {
        get
        { return _addressedAddOnProductID; }

        set
        { _addressedAddOnProductID = value; }

    }


    private decimal _addressedAddOnPricePerPiece = 0;
    public decimal addressedAddOnPricePerPiece
    {
        get
        { return _addressedAddOnPricePerPiece; }

        set
        { _addressedAddOnPricePerPiece = value; }

    }


    private decimal _addressedAddOnPostageRate = 0.3m;
    public decimal addressedAddOnPostageRate
    {
        get
        { return _addressedAddOnPostageRate; }

        set
        { _addressedAddOnPostageRate = value; }

    }


    private int _suggestedAddressedAddOnStartQty = 0;
    public int suggestedAddressedAddOnStartQty
    {
        get
        { return _suggestedAddressedAddOnStartQty; }

        set
        { _suggestedAddressedAddOnStartQty = value; }

    }






    //New Mover Properties
    private int _newMoverCount = -1;
    public int newMoverCount
    {
        //Set to -1 incase there are 0 New Movers.
        get
        { return _newMoverCount; }

        set
        { _newMoverCount = value; }

    }


    public decimal newMoverPostageRate
    {

        get
        {
            decimal defaultRate = 0.34M;
            decimal configRate = 0;

            if (!Decimal.TryParse(appxCMS.Util.AppSettings.GetDecimal("NewMoverPostageRate").ToString(), out defaultRate))
            { return defaultRate; }

            else
            {
                configRate = Convert.ToDecimal(appxCMS.Util.AppSettings.GetDecimal("NewMoverPostageRate").ToString());
                return configRate;
            }

        }

    }


    public decimal newMoverRate
    {

        get
        {
            decimal defaultRate = 0.89M;
            decimal configRate = 0;

            if (!Decimal.TryParse(appxCMS.Util.AppSettings.GetDecimal("NewMoverRate").ToString(), out defaultRate))
            { return defaultRate; }

            else
            {
                configRate = Convert.ToDecimal(appxCMS.Util.AppSettings.GetDecimal("NewMoverRate").ToString());
                return configRate;
            }

        }

    }


    private bool _showNewMover = false;
    public bool showNewMover
    {

        get
        { return _showNewMover; }

        set
        { _showNewMover = value; }

    }


    private int _newMoverBaseProductID = 0;
    public int newMoverBaseProductID
    {
        get
        { return _newMoverBaseProductID; }

        set
        { _newMoverBaseProductID = value; }

    }


    private int _newMoverProductID = 0;
    public int newMoverProductID
    {
        get
        { return _newMoverProductID; }

        set
        { _newMoverProductID = value; }

    }





    //Targeted Email Properties
    private int _targetedEmailCount = -1;
    public int targetedEmailCount
    {
        //Set to -1 incase there are 0 matching emails.
        get
        { return _targetedEmailCount; }

        set
        { _targetedEmailCount = value; }

    }


    public int numEmailCampaigns
    {

        get
        {
            int defaultVal = 3;
            int configVal = 0;

            if (!Int32.TryParse(appxCMS.Util.AppSettings.GetString("NumOfEmailBlasts"), out defaultVal))
            { return defaultVal; }

            else
            {
                configVal = Convert.ToInt32(appxCMS.Util.AppSettings.GetString("NumOfEmailBlasts"));
                return configVal;
            }

        }

    }


    public int emailServiceFee
    {

        get
        {
            int defaultRate = 475;
            int configRate = 0;

            if (!Int32.TryParse(appxCMS.Util.AppSettings.GetString("EmailServiceFee").ToString(), out defaultRate))
            { return defaultRate; }

            else
            {
                configRate = Convert.ToInt32(appxCMS.Util.AppSettings.GetDecimal("EmailServiceFee").ToString());
                return configRate;
            }

        }

    }


    public decimal emailPerThousandRate
    {

        get
        {
            decimal defaultRate = 0.04M;
            decimal configRate = 0;

            if (!Decimal.TryParse(appxCMS.Util.AppSettings.GetDecimal("EmailPerThousandRate").ToString(), out defaultRate))
            { return defaultRate; }

            else
            {
                configRate = Convert.ToDecimal(appxCMS.Util.AppSettings.GetDecimal("EmailPerThousandRate").ToString());
                return configRate;
            }

        }

    }


    private bool _showEmails = false;
    public bool showEmails
    {

        get
        { return _showEmails; }

        set
        { _showEmails = value; }

    }


    private int _targetedEmailBaseProductID = 0;
    public int targetedEmailBaseProductID
    {
        get
        { return _targetedEmailBaseProductID; }

        set
        { _targetedEmailBaseProductID = value; }

    }


    private int _targetedEmailProductID = 0;
    public int targetedEmailProductID
    {
        get
        { return _targetedEmailProductID; }

        set
        { _targetedEmailProductID = value; }

    }











    //Methods
    protected void Page_Init(object sender, EventArgs e)
    {
        SetPageProperties();
    }



    private void SetPageProperties()
    {

        XmlDocument oCart = Profile.Cart;
        Taradel.WLProduct oProd = null;
        Taradel.WLProduct oAddressedMailProd = null;
        Taradel.WLProduct oNewMoverProd = null;
        Taradel.WLProduct oTargetedEmailProd = null;


        //environment
        currentMode = appxCMS.Util.AppSettings.GetString("Environment").ToLower();




        if (oCart != null)
        {

            //SiteID
            siteID = appxCMS.Util.CMSSettings.GetSiteId();


            //DistributionID
            distributionID = CartUtility.GetDistributionID(oCart);



            //Get the USelectID
            Taradel.CustomerDistribution oDist = Taradel.CustomerDistributions.GetDistribution(distributionID);
            uSelectID = oDist.USelectMethodReference.ForeignKey();

            //Just to be safe, let's get outta here in case this is anything but an EDDM Order
            if (uSelectID != 1)
            {
                Response.Redirect("~/Step3-Checkout.aspx");
                return;
            }


            //Set referenceID
            referenceID = DistributionUtility.RetrieveReferenceID(distributionID);



            //SiteDetails
            SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
            siteDetails = SiteUtility.RetrieveSiteSettings(siteID);

            testMode = siteDetails.TestMode;


            //Set the USelect Flags, USelectID specific values: totalCost, productName, etc
            eddmMap = true;
            productID = CartUtility.GetProductID(oCart, "EDDM");
            baseProductID = CartUtility.GetBaseProductID(oCart, "EDDM");
            oProd = Taradel.WLProductDataSource.GetProductByBaseId(baseProductID);
            productName = oProd.Name;
            numImpressions = CartUtility.GetNumberOfImpressions(oCart, "EDDM");
            totalSelected = CartUtility.GetTotalSelected(oCart, "EDDM", numImpressions);
            designFee = CartUtility.GetDesignFee(oCart, "EDDM");
            productPrice = CartUtility.GetPrice(oCart, "EDDM");
            mailingCost = CalculateTotalCost(productPrice, designFee);
            extraCopies = CartUtility.GetExtraPiecesQty(oCart, "EDDM");
            zipCode = CartUtility.GetZipCode(oCart, "EDDM");





            //New Mover, Targeted Email ProductID and BaseProductIDs
            //PROD
            if (currentMode != "dev")
            {

                //Production IDs
                addressedAddOnBaseProductID = 244;
                newMoverBaseProductID = 235;
                targetedEmailBaseProductID = 236;


                if (showAddressedListAddOns)
                {
                    oAddressedMailProd = Taradel.WLProductDataSource.GetProductByBaseId(addressedAddOnBaseProductID);
                    addressedAddOnProductID = oAddressedMailProd.ProductID;
                }

                if (showNewMover)
                {
                    oNewMoverProd = Taradel.WLProductDataSource.GetProductByBaseId(newMoverBaseProductID);
                    newMoverProductID = oNewMoverProd.ProductID;
                }

                if (showEmails)
                {
                    oTargetedEmailProd = Taradel.WLProductDataSource.GetProductByBaseId(targetedEmailBaseProductID);
                    targetedEmailProductID = oTargetedEmailProd.ProductID;
                }

            }

            //DEV
            else
            {

                //DEV IDs
                addressedAddOnBaseProductID = 246;
                newMoverBaseProductID = 237;
                targetedEmailBaseProductID = 238;

                if (showAddressedListAddOns)
                {
                    oAddressedMailProd = Taradel.WLProductDataSource.GetProductByBaseId(addressedAddOnBaseProductID);
                    addressedAddOnProductID = oAddressedMailProd.ProductID;
                }


                if (showNewMover)
                {
                    oNewMoverProd = Taradel.WLProductDataSource.GetProductByBaseId(newMoverBaseProductID);
                    newMoverProductID = oNewMoverProd.ProductID;
                }

                if (showEmails)
                {
                    oTargetedEmailProd = Taradel.WLProductDataSource.GetProductByBaseId(targetedEmailBaseProductID);
                    targetedEmailProductID = oTargetedEmailProd.ProductID;
                }

            }



            //New Mover properties
            if (appxCMS.Util.CMSSettings.GetBoolean("Product", "NewMover", siteID))
            {
                showNewMover = true;
                newMoverCount = GetNewMoverCount();
            }


            //Targeted Email properties
            if (appxCMS.Util.CMSSettings.GetBoolean("Product", "Email", siteID))
            {
                showEmails = true;
                targetedEmailCount = GetEmailCount();
            }



            //Addressed AddOn 
            //For the moment, this is only offered and used by OLB.  Will need additional development to extend to other sites.
            if (siteID == 11)
            {


                //ONLY applies to OLB!
                //Addressed AddOn properties
                if (appxCMS.Util.CMSSettings.GetBoolean("Product", "AddressedListAddOns", siteID))
                {

                    showAddressedListAddOns = true;


                    //get addressedmail mark up and mark up type from database!
                    oAddressedMailProd = Taradel.WLProductDataSource.GetProductByBaseId(addressedAddOnBaseProductID);
                    addressedMailMarkUp = oAddressedMailProd.Markup;
                    addressedMailMarkUpType = oAddressedMailProd.MarkupType;

                    addOnAddressedProspects = RetrieveHyperTargetRouteCount(Session["Franchise"].ToString(), Session["Location"].ToString());


                    //AddressedAddOn Price Per Piece from initial page load
                    if (addOnAddressedProspects > 0)  //<-- How and when is this set?
                    {
                        Taradel.ProductPriceQuote oPriceQuoteAddressed = new Taradel.ProductPriceQuote(siteID, addressedAddOnBaseProductID, addOnAddressedProspects, 0, 0, 0, 1, null, zipCode, false, false, 0, "");
                        addressedAddOnPricePerPiece = oPriceQuoteAddressed.PricePerPiece;
                    }

                }


            }

            //Set values of hidden fields.
            //Used to send BaseProductID and ProductID to hyperlinks to fire proper help modals
            hidBaseProductID.Value = baseProductID.ToString();
            txtBaseProductID.Text = baseProductID.ToString();
            hidProductID.Value = productID.ToString();
            txtProductID.Text = productID.ToString();


            //User selected number of AddressedMail AddOns.  Stores value from UI.
            txtTotalSelected.Text = totalSelected.ToString();
            hidTotalSelected.Value = totalSelected.ToString();
            txtNumImpressions.Text = numImpressions.ToString();
            hidNumImpressions.Value = numImpressions.ToString();
            txtExtraCopies.Text = extraCopies.ToString();
            hidExtraCopies.Value = extraCopies.ToString();
            txtDistributionID.Text = distributionID.ToString();
            hidDistributionID.Value = distributionID.ToString();
            txtZipCode.Text = zipCode.ToString();
            hidZipCode.Value = zipCode.ToString();
            txtUSelectID.Text = uSelectID.ToString();
            hidUSelectID.Value = uSelectID.ToString();
            txtAddressedMailMarkUp.Text = addressedMailMarkUp.ToString();
            hidAddressedMailMarkUp.Value = addressedMailMarkUp.ToString();
            txtAddressedMailMarkUpType.Text = addressedMailMarkUpType.ToString();
            hidAddressedMailMarkUpType.Value = addressedMailMarkUpType.ToString();
            txtAddressedAddOnBaseProductID.Text = addressedAddOnBaseProductID.ToString();
            hidAddressedAddOnBaseProductID.Value = addressedAddOnBaseProductID.ToString();
            txtAddressedAddOnPricePerPiece.Text = addressedAddOnPricePerPiece.ToString();
            hidAddressedAddOnPricePerPiece.Value = addressedAddOnPricePerPiece.ToString();


        }

        else
        {
            Response.Redirect("~/default.aspx");
        }


    }



    protected void Page_Load(object sender, EventArgs e)
    {

        //This site is only designed to be displayed for EDDM campaigns.
        if (uSelectID != 1)
        {
            Response.Redirect("~/Step3-Checkout.aspx");
            return;
        }


        else
        {

            //Redirect if no upsell services are enabled for some reason
            if ((!showEmails) && (!showNewMover) && (!showAddressedListAddOns))
            {
                Response.Redirect("~/Step3-Checkout.aspx");
            }

        }

        if (!Page.IsPostBack)
        {
            BuildAddressedAddOnsDisplay();
            BuildMailedProductsDisplay();
            BuildNewMoverDisplay();
            BuildEmailDisplay();
            BuildPageHeader();
        }



        //Show Debug Panel
        if (testMode)
        { ShowDebug(); }

        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["debug"]))
            {
                if (Request.QueryString["debug"] == "hodor")
                { ShowDebug(); }
            }
        }


    }



    private void BuildPageHeader()
    {
        //Site Details Object
        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);


        //Set Page Header control
        if (!siteDetails.UseRibbonBanners)
        {
            PageHeader.headerType = "simple";
            PageHeader.simpleHeader = "Design and Delivery Options";
        }
        
        else
        {            
            PageHeader.headerType = "partial";
            PageHeader.mainHeader = "Print";
            PageHeader.subHeader = "Design and Delivery Options";
        }

    }



    protected string RetrieveUSelectCountURL(string type)
    {

        StringBuilder errorMsg = new StringBuilder();
        string returnThis = string.Empty;
        string sql = "SELECT CountUrl FROM USelectMethod WHERE Name = '" + type + "'";
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();


        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader rdr = comm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        returnThis = rdr[0].ToString();
                    }
                }
            }

        }


        catch (Exception objException)
        {
            pnlError.Visible = true;
            errorMsg.Append("The following errors occurred:<br /><br />");
            errorMsg.Append("<ul>");
            errorMsg.Append("<li>Message: " + objException.Message + "</li>");
            errorMsg.Append("<li>Source: " + objException.Source + "</li>");
            errorMsg.Append("<li>Stack Trace: " + objException.StackTrace + "</li>");
            errorMsg.Append("<li>Target Site: " + objException.TargetSite.Name + "</li>");
            errorMsg.Append("<li>SQL: " + sql + "</li>");
            errorMsg.Append("</ul>");
            litErrorMessage.Text = errorMsg.ToString();
        }


        return returnThis;
    }




    private DataTable RetrieveTheZipRouteTable(string referenceID)
    {

        DataTable dt = new DataTable();
        string jsonString = string.Empty;
        dt.Columns.Add("ZipRoute");
        dt.AcceptChanges();

        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["MapServerConn"].ToString();
        StringBuilder errorMsg = new StringBuilder();
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
                    { jsonString = (rdr[0].ToString()); }
                }

                conn.Close();
            }

            // Pull apart the JSON in the SELECTION and put it into a table as a TVP for usp_RetrieveEmailCountForRoutes
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
            errorMsg.Append("The following errors occurred:<br /><br />");
            errorMsg.Append("<ul>");
            errorMsg.Append("<li>Message: " + objException.Message + "</li>");
            errorMsg.Append("<li>Source: " + objException.Source + "</li>");
            errorMsg.Append("<li>Stack Trace: " + objException.StackTrace + "</li>");
            errorMsg.Append("<li>Target Site: " + objException.TargetSite.Name + "</li>");
            errorMsg.Append("<li>SQL: " + mapSql + "</li>");
            errorMsg.Append("</ul>");

            pnlError.Visible = true;
            litErrorMessage.Text = errorMsg.ToString();

        }


        return dt;
    }



    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        UpdateCart();
        Response.Redirect("~/Step3-Checkout.aspx");
    }



    protected void UpdateCart()
    {

        bool newMoverSelected = false;
        bool emailCampaignSelected = false;
        bool addressedMailSelected = false;
        string newMoverGUID = System.Guid.NewGuid().ToString();
        string targetedEmailGUID = System.Guid.NewGuid().ToString();
        string addressedAddOnGUID = System.Guid.NewGuid().ToString();
        decimal newMoverPrice = CalculateNewMoverPrice(newMoverCount);
        decimal newMoverTaxablePrice = CalculateNewMoverTaxablePrice(newMoverPrice, newMoverPostageRate, newMoverCount);
        decimal newMoverPostage = CalculateNewMoverPostage(newMoverCount, newMoverPostageRate);

        if (chkAddNewMover.Checked)
        { newMoverSelected = true; }

        if (chkAddEmails.Checked)
        { emailCampaignSelected = true; }

        if (chkAddressedAddOns.Checked)
        { addressedMailSelected = true; }


        XmlDocument oXML = new XmlDocument();
        oXML.LoadXml(Profile.Cart.OuterXml);

        XmlNode oCart = oXML.SelectSingleNode("/cart");

        //architecture needs work here - BUT given the list of addons, let's remove them all from the cart by default
        List<string> productAddOns = new List<string>();
        productAddOns.Add(newMoverProductID.ToString()); //new mover
        productAddOns.Add(targetedEmailProductID.ToString()); //email
        productAddOns.Add(addressedAddOnProductID.ToString()); //addressed add ons



        //Removes from cart 
        foreach (string removeFromCart in productAddOns)
        {
            oCart = RemoveProductFromCart(oCart, removeFromCart);
            Profile.Cart = oXML;
            Profile.Save();
        }


        //Build New Mover Product if needed.
        if (newMoverSelected)
        {

            //Get BASE Product data for NewMover prod.  
            Taradel.Product oProdNewMover = Taradel.ProductDataSource.GetProduct(newMoverBaseProductID);


            
            oCart = (CartUtility.AddProduct(oCart, System.DateTime.Now.ToString(), newMoverBaseProductID.ToString(), oProdNewMover.DesignFee.ToString(), distributionID.ToString(),
                oProdNewMover.FlatRateShipFee.ToString(), oProdNewMover.FlatRateShipQty.ToString(), newMoverGUID, oProdNewMover.IsFlatRateShipping.ToString(), "", "", oProdNewMover.Name, 
                oProdNewMover.PaperHeight.ToString(), oProdNewMover.PaperWidth.ToString(), newMoverPostage.ToString("C") + " (est. per drop)", newMoverPrice.ToString(), newMoverRate.ToString(),
                newMoverProductID.ToString(), newMoverCount.ToString(), siteID.ToString(), oProdNewMover.SKU, newMoverTaxablePrice.ToString(), "New Mover Postcard", ""));
            
            string deliveryAddressID = "0";
            string extraCopies = "0";
            string weight = "0";
            string PageCount = "1";
            string zip = "23060";
            string designType = CartUtility.GetDesignType(oCart, "New Mover Postcard").ToString();       //<-- will need to check for type instead of hard code EDDM once AddressedList goes live...
            string frontFileExt = "";
            string frontFileName = "";
            string frontRealFileName = "";
            string frontAction = "omitted";
            Boolean hasBackDesign = true;
            string backFileExt = "";
            string backFileName = "";
            string backRealFileName = "";
            string backAction = "";
            string artKey = "";
            string requiresProof = "";


            oCart = CartUtility.AddShipments(oCart, "New Mover Postcard", deliveryAddressID, extraCopies, weight, PageCount, oProdNewMover.PaperWidth.ToString(), oProdNewMover.PaperHeight.ToString(), newMoverRate.ToString("N2"), zip, newMoverCount);


            
            oCart = CartUtility.AddDesign(oCart, "New Mover Postcard", designType, frontFileExt, frontFileName, frontRealFileName, frontAction, hasBackDesign, backFileExt, backFileName, backRealFileName, backAction, artKey, requiresProof);
            //oCart = CartUtility.AddDesign(oCart, "New Mover Postcard", designType, frontFileExt, frontFileName, frontRealFileName, frontAction, hasBackDesign, backFileExt, backFileName, backRealFileName, backAction, artKey);


            oCart = CartUtility.AddAttribute(oCart, "New Mover Postcard", "catName", "0", "0", "0", "0", "0", "0");

            Profile.Cart = oXML;
            Profile.Save();

            //Clean up.
            oProdNewMover = null;

        }


        //Build Email Campaign Product if needed.
        if (emailCampaignSelected)
        {

            //Get BASE Product data for TargetedEmail prod.  
            Taradel.Product oProdEmail = Taradel.ProductDataSource.GetProduct(targetedEmailBaseProductID);

            //Params:
            //cartNode, addedDate, baseProductID, designFee, distributionID, flatRateShipFee, flatRateShipQTY, indexGUID, isFlatRateShipping, jobComments, 
            //jobName, productName, paperHeight, paperWidth, postageFees, price, pricePerPiece, productID, qty, siteID, sku, taxablePrice, productType, weight
            oCart = (CartUtility.AddProduct(oCart, System.DateTime.Now.ToString(), targetedEmailBaseProductID.ToString(), oProdEmail.DesignFee.ToString(), distributionID.ToString(),
                oProdEmail.FlatRateShipFee.ToString(), oProdEmail.FlatRateShipQty.ToString(), targetedEmailGUID, oProdEmail.IsFlatRateShipping.ToString(), "", "", oProdEmail.Name, "n/a",
                "n/a", "n/a", CalculateEmailPrice(targetedEmailCount, numEmailCampaigns, emailServiceFee, emailPerThousandRate).ToString(),
                CalculateEmailPricePerPiece(targetedEmailCount, numEmailCampaigns, emailServiceFee, emailPerThousandRate).ToString(),
                targetedEmailProductID.ToString(), (targetedEmailCount * numEmailCampaigns).ToString(), siteID.ToString(), oProdEmail.SKU,
                CalculateEmailPrice(targetedEmailCount, numEmailCampaigns, emailServiceFee, emailPerThousandRate).ToString("C"), "Targeted Emails", "n/a"));



            Profile.Cart = oXML;
            Profile.Save();

            //Clean up.
            oProdEmail = null;


        }


        //Build AddressedList AddOn Product if needed.
        if (addressedMailSelected)
        {

            int qty = (Convert.ToInt32(hidSelectedAddOnAddressedList.Value) * Convert.ToInt32(hidNumTimesSelected.Value));
            decimal price = CalculateAddressedAddOn();



            //Get BASE Product data for AddressedList 6x11 prod.  
            Taradel.Product oAddressedListProd = Taradel.ProductDataSource.GetProduct(addressedAddOnBaseProductID);



            //Get and build a quick quoteObj to get Weight
            Taradel.ProductPriceQuote quoteObj = new Taradel.ProductPriceQuote(siteID, addressedAddOnBaseProductID, qty, 0, extraCopies, distributionID, 1, null, 
                zipCode, false, false, Convert.ToDecimal(addressedMailMarkUp), addressedMailMarkUpType);





            //24 Params:
            //cartNode, addedDate, baseProductID, designFee, distributionID, flatRateShipFee, flatRateShipQTY, indexGUID, isFlatRateShipping, jobComments, 
            //jobName, productName, paperHeight, paperWidth, postageFees, price, pricePerPiece, productID, qty, siteID, sku, taxablePrice, productType, weight
            oCart = (CartUtility.AddProduct(oCart, 
                System.DateTime.Now.ToString(),                                                                                             //AddDate
                addressedAddOnBaseProductID.ToString(),                                                                                     //BaseProductID
                oAddressedListProd.DesignFee.ToString(),                                                                                    //Design Fee
                distributionID.ToString(),                                                                                                  //DistributionID
                oAddressedListProd.FlatRateShipFee.ToString(),                                                                              //Flat Rate Ship Fee
                oAddressedListProd.FlatRateShipQty.ToString(),                                                                              //Flat Rate Ship Qty
                addressedAddOnGUID,                                                                                                         //GUID
                oAddressedListProd.IsFlatRateShipping.ToString(),                                                                           //Is FlatRate Shipping?
                "",                                                                                                                         //Job Comments
                oAddressedListProd.Name + " AddressedMail AddOn",                                                                           //Job Name
                oAddressedListProd.Name,                                                                                                    //Product Name                                                         
                oAddressedListProd.PaperHeight.ToString(),                                                                                  //Paper Height                                                    
                oAddressedListProd.PaperWidth.ToString(),                                                                                   //Paper Width
                (qty * addressedAddOnPostageRate).ToString(),                                                                               //Postage Fees         
                price.ToString(),                                                                                                           //Price
                hidAddressedAddOnPricePerPiece.Value.ToString(),                                                                            //PricePerPc
                addressedAddOnProductID.ToString(),                                                                                         //ProductID   
                qty.ToString(),                                                                                                             //QTY
                siteID.ToString(),                                                                                                          //SiteID
                oAddressedListProd.SKU,                                                                                                     //SKU
                CalculateAddressedAddOnTaxablePrice(price, addressedAddOnPostageRate, qty).ToString(),                                      //Taxable price        
                "AddressedMail AddOn",                                                                                                      //Product Type          
                quoteObj.Weight.ToString()));                                                                                               //Weight                


            //Need to add Drops Node and child Drop nodes
            //Clean up schedule nodes in Cart


            //Add Drops Node
            //Create the CustomerObj
            Taradel.Customer customerObj = null;
            customerObj = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name);
            

            //Create the Summary Obj
            Taradel.MapServer.UserData.SelectionSummary summaryObj = null;
            summaryObj = Taradel.CustomerDistributions.GetSelectionSummary(referenceID);


            oCart = (CartUtility.AddDrops(oCart, "AddressedMail AddOn", customerObj.Company, customerObj.Address1, 
            customerObj.Address2, customerObj.City, customerObj.State, customerObj.ZipCode, "", "", 
            "True", summaryObj.UseBusiness.ToString(), summaryObj.UsePOBox.ToString()));


            //Add Individual Drops
            int numTimesSelected = Convert.ToInt16(hidNumTimesSelected.Value);
            bool isMulitple = false;

            if (numTimesSelected > 1)
            { isMulitple = true; }


            int daysToAdd = 1;
            int frequency = Convert.ToInt16(Session["Frequency"].ToString());
            DateTime startingDate = Convert.ToDateTime(Session["StartDate"].ToString());
            DateTime dropDate;


            //loop thru number of times
            for (int dropNumber = 1; dropNumber <= numTimesSelected; dropNumber++)
            {


                //OLB Logic only. Will need to improve outside of OLB orders
                if (siteID == 11)
                {


                    daysToAdd = ((dropNumber - 1) * (frequency * 7));
                    dropDate = startingDate.AddDays(daysToAdd);

                    //Find Prev Friday
                    DateTime prevFridayDropDate = dropDate;
                    while (prevFridayDropDate.DayOfWeek != DayOfWeek.Friday)
                    { prevFridayDropDate = prevFridayDropDate.AddDays(-1); }

                    //added 2/25/2016 - rs
                    dropDate = prevFridayDropDate;
                    //end added 2/25/2016 - rs

                    oCart = CartUtility.AddIndividualDrop(oCart, "AddressedMail AddOn", dropNumber.ToString(), hidSelectedAddOnAddressedList.Value.ToString(), dropDate.ToShortDateString(), true, numTimesSelected.ToString());

                }


            }




            Profile.Cart = oXML;
            Profile.Save();

            //Clean up.
            oAddressedListProd = null;
            summaryObj = null;
            customerObj = null;


        }

    }



    private XmlNode RemoveProductFromCart(XmlNode xmlCart, string productID)
    {

        XmlNodeList nodes = xmlCart.ChildNodes;//.GetElementsByTagName("Product");
        XmlNodeList nodeList = xmlCart.SelectNodes("//Product[@ProductID='" + productID + "']");
        foreach(XmlNode xListNode in nodeList)
        {
            xListNode.ParentNode.RemoveChild(xListNode);
        }

       

        return xmlCart;



        ////.Net 4.0
        //var xDoc = XDocument.Parse(xmlCart.OuterXml.ToString()); //XDocument.Load(filename)

        ////.Net 2.0
        //XmlDocument xmlDoc = new XmlDocument();

        //Response.Write("xDoc.Length before:" + xDoc.ToString().Length.ToString() + "<br/>");

        //try
        //{
        //    xDoc.Descendants("Product")
        //    .FirstOrDefault(a => (string)a.Attribute("ProductID") == "237")

        //    .Remove();
        //}
        //catch(Exception ex)
        //{
        //    Response.Write("Product not found" + "<br/>");
        //}

        ////
        ////xml.Descendants("Photos")
        ////.Where(e => (string)e.Attribute("File") == ID)
        ////.Remove();
        ////

        //using (XmlReader xmlReader = xDoc.CreateReader())
        //{
        //    xmlDoc.Load(xmlReader);
        //}
        ////Response.Write("xDoc.Length after:" + xDoc.ToString().Length.ToString() + "<br/>");

        //XmlNode xNode = xmlDoc.SelectSingleNode("cart");

        //return xNode;
    }



    protected decimal CalculateTotalCost(decimal productPrice, decimal designFee)
    {
        decimal results = (productPrice + Convert.ToDecimal(designFee));
        return results;
    }






    //Page Builders
    protected void BuildMailedProductsDisplay()
    {

        litProdName.Text = productName;
        litTotalSelected.Text = totalSelected.ToString("N0");
        lblMailedProductPrice.Text = mailingCost.ToString("C");

        //Impressions display string
        if (numImpressions > 1)
        { litImpressions.Text = numImpressions.ToString() + " times"; }

        else
        { litImpressions.Text = "1 time"; }

        lblMailedProductPrice.Text = mailingCost.ToString("C");

    }



    protected void BuildNewMoverDisplay()
    {

        if (showNewMover)
        { 

            pnlNewMover.Visible = true;
            litRecentMoverCount.Text = newMoverCount.ToString();
            lblNewMoverPrice.Text = CalculateNewMoverPrice(newMoverCount).ToString("C");

            if (newMoverCount > 0)
            { 
                pnlNewMoversSellPane.Visible = true;
                pnlNoNewMoversFound.Visible = false;
            }

            else 
            {
                pnlNewMoversSellPane.Visible = false;
                pnlNoNewMoversFound.Visible = true;
            }

        }

        else
        { pnlNewMover.Visible = false; }

    }



    protected void BuildEmailDisplay()
    {

        if (showEmails)
        {
            pnlTargetedEmails.Visible = true;
            litEmailCount.Text = targetedEmailCount.ToString("N0");
            litNumEmailCampaigns.Text = numEmailCampaigns.ToString();
            litEmailImpressions.Text = CalculateEmailImpressions(targetedEmailCount, numEmailCampaigns).ToString("N0");
            lblEmailPrice.Text = CalculateEmailPrice(targetedEmailCount, numEmailCampaigns, emailServiceFee, emailPerThousandRate).ToString("C");

            if (targetedEmailCount > 0)
            {
                pnlTargetedEmailsSellPane.Visible = true;
                pnlNoTargetedEmails.Visible = false;
            }

            else
            {
                pnlTargetedEmailsSellPane.Visible = false;
                pnlNoTargetedEmails.Visible = true;
            }

        }

        else
        { pnlTargetedEmails.Visible = false; }
    
    
    }



    private void BuildAddressedAddOnsDisplay()
    {


        //For the moment, this is only offered and used by OLB.  Will need additional development to extend to other sites.
        if (siteID == 11)
        {


            //3/2/2016 - - - - - - - - - - rs update
            //         IF there is a cart object - and there is an addressed add on product in that cart object
            //         use those values instead of the default values    
            XmlDocument oCart = Profile.Cart;
            int addressedAddOns_selectorStart = 0;
            int addressedAddOns_Times = 0;

            if(oCart != null)
                {
                XmlNodeList nodeList = oCart.SelectNodes("//Product[@ProductID='" + addressedAddOnProductID + "']");

                foreach (XmlNode xNode in nodeList)
                {
                    //Response.Write(xNode.Attributes["Quantity"].Value + "<br/>");
                    addressedAddOns_selectorStart = Int32.Parse(xNode.Attributes["Quantity"].Value);
                }
                XmlNodeList nodeDropList = oCart.SelectNodes("//Product[@ProductID='" + addressedAddOnProductID + "']/Drops/Drop");
                foreach(XmlNode nNode in nodeDropList)
                {
                    //Response.Write(nNode.Attributes["Multiple"].Value + "<br/>");
                    addressedAddOns_Times = Int32.Parse(nNode.Attributes["Multiple"].Value);
                    break;
                }

            }




            //If the required sessions exisit....
            if ((Session["Franchise"] != null) && (Session["Location"] != null))
            {

                //If the sessions have content...
                if ((Session["Franchise"].ToString().Length > 0) && (Session["Location"].ToString().Length > 0))
                {
                    lblFranchise.Text = Session["Location"].ToString();

                    //seems to be the primary variable here - 
                    suggestedAddressedAddOnStartQty = ((addOnAddressedProspects - 1000) / 2) + 1000;
                    //so change
                    if(addressedAddOns_selectorStart > 0)
                    {
                        suggestedAddressedAddOnStartQty = addressedAddOns_selectorStart;
                    }

                    hidOverrideAddOnAddressedProspects.Value = addressedAddOns_selectorStart.ToString();
                    hidOverrideAddOnAddressedTimes.Value = addressedAddOns_Times.ToString();


                    if (appxCMS.Util.CMSSettings.GetBoolean("Product", "AddressedListAddOns", siteID))
                    {
                        //QTY must be 1000 or greater to offer...
                        if (addOnAddressedProspects >= 1000)
                        {

                            showAddressedListAddOns = true;
                            lblSendToAddOns.Text = addOnAddressedProspects.ToString("N0");
                            lblAddressedHyperMatchCount.Text = addOnAddressedProspects.ToString("N0");


                            //Set these hidden fields for the js and UI.
                            txtSelectedAddOnAddressedList.Text = suggestedAddressedAddOnStartQty.ToString();
                            hidSelectedAddOnAddressedList.Value = suggestedAddressedAddOnStartQty.ToString();

                            txtSuggestedAddressedAddOnStartQty.Text = suggestedAddressedAddOnStartQty.ToString();
                            hidSuggestedAddressedAddOnStartQty.Value = suggestedAddressedAddOnStartQty.ToString();

                            txtAddOnAddressedProspects.Text = addOnAddressedProspects.ToString();
                            hidAddOnAddressedProspects.Value = addOnAddressedProspects.ToString();

                        }

                        else
                        {
                            txtSelectedAddOnAddressedList.Text = "n/a";
                            hidSelectedAddOnAddressedList.Value = "n/a";

                            txtSuggestedAddressedAddOnStartQty.Text = "n/a";
                            hidSuggestedAddressedAddOnStartQty.Value = "n/a";

                            txtAddOnAddressedProspects.Text = "n/a";
                            hidAddOnAddressedProspects.Value = "n/a";

                        }

                    }

                }

                else
                {
                    txtSelectedAddOnAddressedList.Text = "n/a";
                    hidSelectedAddOnAddressedList.Value = "n/a";

                    txtSuggestedAddressedAddOnStartQty.Text = "n/a";
                    hidSuggestedAddressedAddOnStartQty.Value = "n/a";

                    txtAddOnAddressedProspects.Text = "n/a";
                    hidAddOnAddressedProspects.Value = "n/a";

                }


            }

            //required sessions are missing.  Do not show AddressedAddOns
            else
            { 
                showAddressedListAddOns = false;
                txtSelectedAddOnAddressedList.Text = "n/a";
                hidSelectedAddOnAddressedList.Value = "n/a";
            }



        }

        else
        {
            showAddressedListAddOns = false;
        }



        //Show/hide the panel
        if (showAddressedListAddOns)
        { pnlAddressedListAddOns.Visible = true; }
        else
        { pnlAddressedListAddOns.Visible = false; }


    }






    //New Mover Helpers
    protected int GetNewMoverCount()
    {

        //http://demographics3.eddmsite.com/NewMovers/Resources/NewMoverCountsFor

        //Set to -1 incase there are actually 0 new movers.
        int results = -1;

        string NewMoverURL = RetrieveUSelectCountURL("New Movers");
        string sNewMoverUrl = NewMoverURL + uSelectID.ToString() + ".ashx?refid=" + referenceID;
        string sNewMover = httpHelp.GetXMLURLPage(sNewMoverUrl);


        //Log the call:
        LogWriter logObj = new LogWriter();
        logObj.RecordInLog("Calling New Mover API: " + sNewMoverUrl);

        if (Int32.TryParse(sNewMover, out results))
        { results = Convert.ToInt32(sNewMover); }

        return results;



    }



    protected decimal CalculateNewMoverPrice(int newMoverCount)
    {

        decimal results = (newMoverCount * newMoverRate);
        return results;

    }



    protected decimal CalculateNewMoverPostage(int newMoverCount, decimal postageRate)
    {
        //strictly an estimate / guess.
        decimal results = (newMoverCount * postageRate);
        return results;

    }

    

    protected decimal CalculateNewMoverTaxablePrice(decimal price, decimal postageRate, int qty)
    {

        decimal results = 0;

        results = (price - (postageRate * qty));

        return results;

    }







    //Email Helpers
    protected int GetEmailCount()
    {
        //Initialize to -1 incase there are actually 0 matching emails.

        int returnThis = -1;
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        string selectSQL = "usp_RetrieveEmailCountForRoutes";
        StringBuilder errorMsg = new StringBuilder();

        //Retrieve the ZipCode/Route date from the Map Server
        //Pull apart the JSON in the SELECTION and put it into a table as a TVP for usp_RetrieveEmailCountForRoutes
        DataTable dt = RetrieveTheZipRouteTable(referenceID);

        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(selectSQL, conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter tvpParam = command.Parameters.AddWithValue("@tvpRoutes", dt);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "dbo.RouteTableType";
                
                command.CommandText = selectSQL;
                conn.Open();
                
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    { returnThis = Int32.Parse(rdr[0].ToString()); }
                }

                conn.Close();

            }

        }


        catch (Exception objException)
        {
            errorMsg.Append("The following errors occurred:<br /><br />");
            errorMsg.Append("<ul>");
            errorMsg.Append("<li>Message: " + objException.Message + "</li>");
            errorMsg.Append("<li>Source: " + objException.Source + "</li>");
            errorMsg.Append("<li>Stack Trace: " + objException.StackTrace + "</li>");
            errorMsg.Append("<li>Target Site: " + objException.TargetSite.Name + "</li>");
            errorMsg.Append("<li>SQL: " + selectSQL + "</li>");
            errorMsg.Append("</ul>");

            pnlError.Visible = true;
            litErrorMessage.Text = errorMsg.ToString();

        }


        return returnThis;

    }






    //Add On AddressedList  - OLB only for right now....
    protected int RetrieveHyperTargetRouteCount(string franchiseName, string locationName)
    {

        int returnThis = 0;
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        string selectSQL = "usp_RetrieveAddressedHyperCountForRoutes";
        StringBuilder errorMsg = new StringBuilder();

        //Retrieve the ZipCode/Route date from the Map Server
        //Pull apart the JSON in the SELECTION and put it into a table as a TVP for usp_RetrieveEmailCountForRoutes
        DataTable dt = RetrieveTheZipRouteTable(referenceID);


        try
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(selectSQL, conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@paramFranchiseName", franchiseName);
                command.Parameters.AddWithValue("@paramLocationName", locationName);

                SqlParameter tvpParam = command.Parameters.AddWithValue("@paramRouteTableType", dt);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "dbo.RouteTableType";

                command.CommandText = selectSQL;
                conn.Open();

                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    { returnThis = Int32.Parse(rdr[0].ToString()); }
                }

                conn.Close();

            }

        }


        catch (Exception objException)
        {
            errorMsg.Append("The following errors occurred:<br /><br />");
            errorMsg.Append("<ul>");
            errorMsg.Append("<li>Message: " + objException.Message + "</li>");
            errorMsg.Append("<li>Source: " + objException.Source + "</li>");
            errorMsg.Append("<li>Stack Trace: " + objException.StackTrace + "</li>");
            errorMsg.Append("<li>Target Site: " + objException.TargetSite.Name + "</li>");
            errorMsg.Append("<li>SQL: " + selectSQL + "</li>");
            errorMsg.Append("</ul>");

            pnlError.Visible = true;
            litErrorMessage.Text = errorMsg.ToString();

        }


        return returnThis;

    }



    protected decimal CalculateEmailImpressions(int emailCount, int emailCampaigns)
    {

        decimal results = (emailCount * emailCampaigns);
        return results;

    }



    protected decimal CalculateEmailPrice(int targetedEmailCount, int emailCampaigns, int emailServiceFee, decimal emailPerThousandRate)
    {
        //quick fix here 2/9/2016
        decimal emailBasePrice = 300;
        decimal emailPricePerPiece = .024m;
        decimal emailTotalPrice = emailPricePerPiece * targetedEmailCount * emailCampaigns;
        if(emailTotalPrice < emailBasePrice)
        {
            emailTotalPrice = emailBasePrice;
        }

        return emailTotalPrice;

        //old
        //decimal results = (((targetedEmailCount * emailCampaigns) * emailPerThousandRate) + emailServiceFee);
        //return results;

    }



    protected decimal CalculateEmailPricePerPiece(int targetedEmailCount, int emailCampaigns, int emailServiceFee, decimal emailPerThousandRate)
    {

        //Best guess for now.
        decimal results = (((targetedEmailCount * emailCampaigns) * emailPerThousandRate) + emailServiceFee) / (targetedEmailCount * emailCampaigns);
        results = Math.Round(results, 2);
        return results;

    }



    protected decimal CalculateAddressedAddOn()
    {
        decimal results = (Convert.ToDecimal(hidSelectedAddOnAddressedList.Value) * Convert.ToInt32(txtNumTimesSelected.Text)  * Convert.ToDecimal(hidAddressedAddOnPricePerPiece.Value));
        return results;

    }



    protected decimal CalculateAddressedAddOnTaxablePrice(decimal price, decimal postageRate, int qty)
    {

        decimal results = 0;

        results = (price - (postageRate * qty));

        return results;

    }








    //Debug
    protected void ShowDebug() 
    {

        pnlDebug.CssClass = String.Empty;
        pnlDebug.CssClass = "alert alert-danger";

        //Properties
        litSiteID.Text = siteID.ToString();
        litTestMode.Text = testMode.ToString().ToUpper();
        litCurrentMode.Text = currentMode.ToString();
        litDistributionID.Text = distributionID.ToString();
        litReferenceID.Text = referenceID.ToString();
        litUSelectID.Text = uSelectID.ToString();
        litDebugTotalSelected.Text = totalSelected.ToString();
        litNumImpressions.Text = numImpressions.ToString();
        litMailingCost.Text = mailingCost.ToString();
        litProductID.Text = productID.ToString();
        litBaseProductID.Text = baseProductID.ToString();
        litProductName.Text = productName;
        litDesignFee.Text = designFee.ToString();
        litProductPrice.Text = productPrice.ToString();
        litEddmMap.Text = eddmMap.ToString().ToUpper();
        litAddressedMap.Text = addressedMap.ToString().ToUpper();
        litZipCode.Text = zipCode;


        //AddressedAddOn Properties
        litShowAddressedAddOn.Text = showAddressedListAddOns.ToString();
        litAddOnAddressedProspects.Text = addOnAddressedProspects.ToString();
        litAddressedMailMarkUp.Text = addressedMailMarkUp.ToString();
        litAddressedMailMarkUpType.Text = addressedMailMarkUpType;
        litAddressedAddOnBaseProductID.Text = addressedAddOnBaseProductID.ToString();
        litAddressedAddOnProductID.Text = addressedAddOnProductID.ToString();
        litAddressedAddOnPricePerPiece.Text = addressedAddOnPricePerPiece.ToString();
        litSuggestedAddressedAddOnStartQty.Text = suggestedAddressedAddOnStartQty.ToString();
        litAddressedAddOnPostageRate.Text = addressedAddOnPostageRate.ToString();


        //New Mover Properties
        litNewMoverRate.Text = newMoverRate.ToString();
        litNewMoverCount.Text = newMoverCount.ToString();
        litShowNewMover.Text = showNewMover.ToString().ToUpper();
        litNewMoverProductID.Text = newMoverProductID.ToString();
        litNewMoverBaseProductID.Text = newMoverBaseProductID.ToString();
        litNewMoverPostageRate.Text = newMoverPostageRate.ToString();


        //Targeted Email Properties
        litTargetedEmailCount.Text = targetedEmailCount.ToString();
        litTargetedEmailBaseProductID.Text = targetedEmailBaseProductID.ToString();
        litTargetedEmailProductID.Text = targetedEmailProductID.ToString();
        litNumEmailCampaigns.Text = numEmailCampaigns.ToString();
        litEmailServiceFee.Text = emailServiceFee.ToString();
        litEmailPerThousandRate.Text = emailPerThousandRate.ToString();
        litShowEmails.Text = showEmails.ToString().ToUpper();


        //Session Variables. Sessions will not ALWAYS exists in non-OLB orders.

        if ((siteID == 11) && (Session["DistID"] != null))
        { 
            litSesDistID.Text = Session["DistID"].ToString();
            litSesFranchise.Text = Session["Franchise"].ToString();
            litSesLocation.Text = Session["Location"].ToString();
            litSesTemplate.Text = Session["Template"].ToString();
            litSesTemplateID.Text = Session["TemplateID"].ToString();
            litSesImpressions.Text = Session["Impressions"].ToString();
            litSesCampaign.Text = Session["Campaign"].ToString();
            litSesQTY.Text = Session["QTY"].ToString();
            litSesBudget.Text= Session["Budget"].ToString();
            litSesRevisedMap.Text = Session["RevisedMap"].ToString();
            litSesTotSelected.Text = Session["TotSelected"].ToString();
            litSesAvgMatch.Text = Session["AvgMatch"].ToString();
            litSesEstCost.Text = Session["EstCost"].ToString();
            litSesTotalMailed.Text = Session["TotalMailed"].ToString();
            litSesPricePerPiece.Text= Session["PricePerPiece"].ToString();
            litSesFrequency.Text = Session["Frequency"].ToString();
            litSesStartDate.Text = Session["StartDate"].ToString();
            litSesProductID.Text = Session["ProductID"].ToString();
            litSesBaseProductID.Text = Session["BaseProductID"].ToString();
        }

    }









    //Container Classes
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



    
}

