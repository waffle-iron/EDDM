using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using appxCMS;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml;




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
//
//===============================================================================================================


public partial class TargetDataMap3 : appxCMS.PageBase
{


    struct TaradelSession
    {
        //keep this struct in sync with any page that uses the session objects
        //TargetDataMap1.aspx
        //TargetDataMap2.aspx
        //TargetDataMap3.aspx

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



    //Fields
    protected bool debug = false;






    // Properties
    private int _productID = 0;
    protected int productID
    {

        get
        { return _productID; }

        set
        { _productID = value; }

    }



    private int _baseProductID = 0;
    protected int baseProductID
    {

        get
        {
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

            bool convertFrequency = Int32.TryParse(TaradelSession.Frequency.ToString(), out outNumber);

            if (convertFrequency)
            { _frequency = outNumber; }

            return _frequency;
        }

        set
        { _frequency = value; }

    }



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



    private string _referenceID;
    public string referenceID
    {
        get
        { return _referenceID; }

        set
        { _referenceID = value; }

    }



    private int _uSelectID = 0;
    public int uSelectID
    {
        get
        { return _uSelectID; }

        set
        { _uSelectID = value; }

    }



    private int _totalSelected = 0;
    public int totalSelected
    {
        get
        { return _totalSelected; }

        set
        { _totalSelected = value; }

    }



    private int _totalMailed = 0;
    public int totalMailed
    {
        get
        { return _totalMailed; }

        set
        { _totalMailed = value; }

    }



    private bool _multipleImpressionsNoFee = false;
    public bool multipleImpressionsNoFee
    {
        get
        {
            if (!String.IsNullOrEmpty(appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressionsNoFee").ToString()))
            { return appxCMS.Util.CMSSettings.GetBoolean("Product", "MultipleImpressionsNoFee"); }

            else
            { return false; }

        }

        set
        { _multipleImpressionsNoFee = value; }

    }


    private int _printMethodID = 0;
    public int printMethodID
    {
        get
        { return _printMethodID; }

        set
        { _printMethodID = value; }

    }



    private double _designFee = 0;
    public double designFee
    {
        get
        { return _designFee; }

        set
        { _designFee = value; }

    }



    private string _productSKU;
    public string productSKU
    {
        get
        { return _productSKU; }

        set
        { _productSKU = value; }

    }


    private string _productName;
    public string productName
    {
        get
        { return _productName; }

        set
        { _productName = value; }

    }



    private double _pricePerPiece = 0;
    public double pricePerPiece
    {
        get
        { return _pricePerPiece; }

        set
        { _pricePerPiece = value; }

    }



    private double _paperWidth = 0;
    public double paperWidth
    {
        get
        { return _paperWidth; }

        set
        { _paperWidth = value; }

    }



    private double _paperHeight = 0;
    public double paperHeight
    {
        get
        { return _paperHeight; }

        set
        { _paperHeight = value; }


    }



    private int _numImpressions = 0;
    public int numImpressions
    {
        get
        { return _numImpressions; }

        set
        { _numImpressions = value; }

    }



    private double _extraDropFee = 0;
    public double extraDropFee
    {
        get
        { return _extraDropFee; }

        set
        { _extraDropFee = value; }

    }



    private double _dropPrice = 0;
    public double dropPrice
    {
        get
        { return _dropPrice; }

        set
        { _dropPrice = value; }

    }



    private string _custState;
    public string custState
    {
        get
        { return _custState; }

        set
        { _custState = value; }

    }



    private int _pageCount = 0;
    public int pageCount
    {
        get
        { return _pageCount; }

        set
        { _pageCount = value; }

    }





    
    //Methods
    protected void Page_Init(object sender, EventArgs e)
    {  SetPageProperties();  }



    private void SetPageProperties()
    {


        //ProductID - this is what is actually stored in the Session
        productID = Convert.ToInt32(RetrieveFromSession(TaradelSession.ProductID).ToString());


        //BaseProductID - this is what is actually stored in the Session
        baseProductID = Convert.ToInt32(RetrieveFromSession(TaradelSession.BaseProductID).ToString());


        //base Prod Obj.  Needed below.
        Taradel.Product baseProdObj = Taradel.ProductDataSource.GetEffectiveProduct(baseProductID);



        //DistributionID
        distributionID = Convert.ToInt32(Session["DistID"]);



        //Impressions
        numImpressions = Int32.Parse(RetrieveFromSession(TaradelSession.Impressions));


        //Price Per Piece
        pricePerPiece = Double.Parse(RetrieveFromSession(TaradelSession.PricePerPiece));



        //SiteID
        siteID = appxCMS.Util.CMSSettings.GetSiteId();


        //Total Mailed
        totalMailed = Convert.ToInt32(RetrieveFromSession(TaradelSession.TotalMailed).ToString().Replace(",",""));


        
        //Total Selected
        totalSelected = Convert.ToInt32(RetrieveFromSession(TaradelSession.TotSelected).ToString().Replace(",", ""));



        //Design Fee
        if (baseProdObj.DesignFee.HasValue)
        {designFee = baseProdObj.DesignFee.Value;}
        //By default, OLB does not have Design Fees....so reset
        designFee = 0;


        //Drop Price
        dropPrice = 99;



        //Extra drop fee  
        if (numImpressions > 1)
        { extraDropFee = ((numImpressions - 1) * dropPrice); }

        if (siteID == 11)
        { extraDropFee = 0; }

        

        //ProductName
        productName = baseProdObj.Name;


        //Sku
        productSKU = baseProdObj.SKU;


        //Width
        paperWidth = Convert.ToDouble(baseProdObj.PaperWidth);
        

        //Height
        paperHeight = Convert.ToDouble(baseProdObj.PaperHeight);


        //Page Count
        pageCount = Convert.ToInt32(baseProdObj.PageCount);


        //Frequency
        frequency = Convert.ToInt32(RetrieveFromSession(TaradelSession.Frequency).ToString());


    }



    protected void Page_Load(object sender, EventArgs e)
    {
        string sTemplateServerHost = appxCMS.Util.AppSettings.GetString("TemplateServerHost");

        //Give the control the correct Product ID
        TemplateBrowser.ProductId = productID;

        ShowDebug(debug);

        if(!Page.IsPostBack)
        {

            if (RetrieveFromSession(TaradelSession.RevisedMap) == "True")
            {
                //Hide the Avg Match Row in Campaign Overview if the suggested Map/Routes have been modified.  No longer valid.
                //sesAvgMatch will be set to 0 since the average is no longer valid if they customize the suggested map.
                rowAvgMatch.Visible = false;
                Session.Remove(TaradelSession.AvgMatch);
                Session.Add(TaradelSession.AvgMatch, "0");
            }


            UpdateProgressBar(75);
            SetOverview();

            //if is only single impression, hide frequency row.
            if (RetrieveFromSession(TaradelSession.Impressions) == "1")
            { frequencyRow.Visible = false; }


            btnShowTemplates.Attributes.Add("data-toggle", "modal");
            btnShowTemplates.Attributes.Add("data-target", "#modalTemplates");
            btnShowTemplates.Attributes.Add("data-dismiss", "modal");


        }

    }



    private void UpdateProgressBar(int progressVal)
    {

        StringBuilder progressHTML = new StringBuilder();

        progressHTML.Append("<div id=" + Convert.ToChar(34) + "progressBar" + Convert.ToChar(34) + " aria-valuemax=" + Convert.ToChar(34) + "100" + Convert.ToChar(34) + " ");
        progressHTML.Append("aria-valuemin=" + Convert.ToChar(34) + "0" + Convert.ToChar(34) + " aria-valuenow=" + Convert.ToChar(34) + progressVal + Convert.ToChar(34) + " ");
        progressHTML.Append("class=" + Convert.ToChar(34) + "progress-bar progress-bar-success" + Convert.ToChar(34) + " role=" + Convert.ToChar(34) + "progressbar" + Convert.ToChar(34) + " ");
        progressHTML.Append("style=" + Convert.ToChar(34) + "width: " + progressVal + "%;" + Convert.ToChar(34) + "><span id=" + Convert.ToChar(34) + "progressVal" + Convert.ToChar(34) + ">");
        progressHTML.Append(progressVal + "%" + "</span></div>");

        litProgressBar.Text = progressHTML.ToString();
    }



    protected void SetOverview()
    {

        //Set labels in Campaign Overview.
        lblFranchise.Text = RetrieveFromSession(TaradelSession.Franchise);
        lblLocation.Text = RetrieveFromSession(TaradelSession.Location);
        lblUserName.Text = RetrieveFromSession(TaradelSession.Location);
        //lblSelected.Text = Convert.ToInt32(RetrieveFromSession(TaradelSession.TotSelected)).ToString("N0");
        lblSelected.Text = RetrieveFromSession(TaradelSession.TotSelected);
        lblPctMatchAvg.Text = RetrieveFromSession(TaradelSession.AvgMatch);
        lblAmount.Text = RetrieveFromSession(TaradelSession.EstCost);
        lblTotalMailed.Text = RetrieveFromSession(TaradelSession.TotalMailed);
        lblPricePerPiece.Text = RetrieveFromSession(TaradelSession.PricePerPiece);
        lblImpressions.Text = RetrieveFromSession(TaradelSession.Impressions);
        lblFrequency.Text = OLBTargeter.GetFrequencyString(Convert.ToInt32(RetrieveFromSession(TaradelSession.Frequency)));
        lblTemplate.Text = OLBTargeter.GetProductName(productID);
        lblLaunchWeek.Text =  "Week of: " + RetrieveFromSession(TaradelSession.StartDate);


        //set the cookie for the template
        HttpCookie cookie = new HttpCookie("DesignOption");
        // Set value of cookie to current date time.
        cookie.Value = "Template";
        // Set cookie to expire in 10 minutes.
        cookie.Expires = DateTime.Now.AddMinutes(10d);
        // Insert the cookie in the current HttpResponse.
        Response.Cookies.Add(cookie);
    }



    public string RetrieveFromSession(string name)
    {
        string returnThis = string.Empty;

        if (HttpContext.Current.Session[name] == null)
        {
            HttpContext.Current.Response.Redirect("TargetDataMap1.aspx?sesTimeout=y");
        }
        else
        {
            returnThis = HttpContext.Current.Session[name].ToString();
        }

        return returnThis;
    }



    protected int CalculatePrintQTY(int totalSelected, int holdQTY, int extraCopies)
    {
        int results = (totalSelected + holdQTY + extraCopies);
        return results;
    }



    protected decimal CalculateTaxPrice(decimal price, decimal postageRate, int qty)
    {
        return (price - (postageRate * qty));
    }



    protected double CalculateMultipleImpressionDropFee(int numDrops, double dropPrice)
    {
    
        double returnThis = 0;

        if((multipleImpressionsNoFee) || (numDrops == 1))
        {returnThis = 0;}
        
        else
        {returnThis = (numDrops - 1) * dropPrice;}

        return returnThis;
    
    }



    protected void btnStartOver_Click(object sender, EventArgs e)
    { Response.Redirect("TargetDataMap1.aspx"); }



    protected void btnBackToStep2_Click(object sender, EventArgs e)
    {
        StringBuilder myURL = new StringBuilder();
        myURL.Append("?d=" + RetrieveFromSession(TaradelSession.DistID));  
        myURL.Append("&l=" + RetrieveFromSession(TaradelSession.Location)); 
        myURL.Append("&i=" + RetrieveFromSession(TaradelSession.Impressions));
        myURL.Append("&c=" + RetrieveFromSession(TaradelSession.Campaign));  
        myURL.Append("&q=" + RetrieveFromSession(TaradelSession.QTY));  
        myURL.Append("&b=" + RetrieveFromSession(TaradelSession.Budget));  
        myURL.Append("&f=" + RetrieveFromSession(TaradelSession.Frequency)); 
        myURL.Append("&s=" + RetrieveFromSession(TaradelSession.StartDate));  
        myURL.Append("&bp=" + RetrieveFromSession(TaradelSession.BaseProductID)); 
        myURL.Append("&p=" + RetrieveFromSession(TaradelSession.ProductID));  
        myURL.Append("&t=" + RetrieveFromSession(TaradelSession.Franchise));  
        myURL.Append("&mapName=" + RetrieveFromSession(TaradelSession.MapName));

        Response.Redirect("TargetDataMap2.aspx" + myURL.ToString());
    }



    protected void btnContinue_Click(object sender, EventArgs e)
    {

        string UserDistributionId = "85888";

        if (RetrieveFromSession(TaradelSession.DistID) != null)
        { UserDistributionId = RetrieveFromSession(TaradelSession.DistID); }

        Session[TaradelSession.TemplateID] = hidSelectedTemplateID.Value;

        CreateCart();

        Response.Redirect("~/MarketingServices.aspx");

    }



    private void CreateCart()
    {


        //========================================================================================================================================
        //   USelectID type 1.

        //   Cart Build Steps:
        //   1) EDDM Product
        //       a) Build the Xml Document.  Load <cart /> parent node.
        //       b) Begin building Product Node components
        //	        1) Build Options SortedList (Int, Int)
        //	        2) Build OptionCategories List (of ProductOptionCategory)
        //	        3) Build PriceMatrix
        //       c) Get PrintMethodID
        //       d) Loop through OptionCategories, add to Options as needed.
        //       e) Get the product options from the Selected Product.  Get the OptCatID.
        //       f) Build the Quote object
        //       g) Set variables returned from Quote Object.
        //       h) Finally, insert the Product Node into the Cart
        //   2) EDDM Attribute Nodes
        //   3) EDDM OrderCalc Node
        //   4) EDDM Drops Node
        //   5) EDDM Indiv Drop Nodes w/ nested Area Nodes
        //   6) EDDM Design Node
        //   7) EDDM SHIPMENTS Node and w/ nested Shipment node(s)
        //========================================================================================================================================




        //DEFINE ALL THE VARIABLES
        TaradelReceiptUtility.OrderCalculator eddmObjCalc = new TaradelReceiptUtility.OrderCalculator();
        StringBuilder methodVars = new StringBuilder();


        //Customer data.  Needed below in multiple nodes.
        Taradel.Customer oCust = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name);



        string eddmGUID = System.Guid.NewGuid().ToString();
        string jobName = "OLB Campaign for " + RetrieveFromSession(TaradelSession.Location) + "-" + DateTime.Today.ToShortDateString();
        string sCustomField1 = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription1");
        string sCustomField2 = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription2");
        string sCustomField3 = appxCMS.Util.CMSSettings.GetSetting("Job", "JobDescription3");
        string designType = "template";                                                                 //'possible choices: 'my', 'template', 'pro'
        string deliveryAddressID = "0";                                                                 //Ship To Address ID
        string comments = "";                                                                           //also comes from JobComments.Text on normal pages
        string mailAllPiecesAtOnce = "yes";                                                            //Normally comes from ddlDrops.SelectedValue. Yes/No, True/False
        DateTime startingDate = DateTime.Parse(RetrieveFromSession(TaradelSession.StartDate));          //Normally comes from (txtLaunchWeek.Text). User selected start of 'Launch Week'
        string zip = "23060";                                                                          //Normally ZipCode.Text. User Ship To Zip Code.

        //Prices
        decimal price = 0;                                                                              //Stores calculated total price from Quote Obj.  (total mailed x price per piece)
        decimal taxablePrice = 0;                                                                       //Currently same as eddmPrice / eddmQuoteObj.Price
        decimal pricePerPiece = 0;                                                                      //Stores EDDM Price Per Piece from Quote Obj.

        //Fees / Rates
        decimal postageFee = 0;                                                                         //Fee for postage. (totalMailQty * postageRate)
        double dropFee = 0;                                                                             //Fee for extra drop. Ex $99
        double dropPrice = 0;                                                                           //Fee for additional drops. Ex $99 x number of drops.
        decimal postageRate = 0;                                                                        //Postage Rate

        //Product attributes
        decimal weight = 0;                                                                              //Comes from Base Product Obj
        decimal WeightPerPage = 0;                                                                       //Used for Shipments Node. (Weight * PageCount) / EDDMSelected

        //various quantities
        int holdQTY = 0;                                                                                //Obsolete concept. In place just to be safe. AKA 'iHoldQty' Does not apply to OLB.
        int extraCopies = 00;                                                                            //AKA 'iShipQty'. Comes from user. Does not apply to OLB.
        int printQTY = 0;                                                                               //AKA 'iPrintQty'. (totalSelected + holdQTY + extraCopies)

        //Campaign variables
        int numDrops = 1;                                                                               //Num of drops. AKA iDrops. Default to 1 for OLB.
        int sendThisNumberOfDrops = 0;                                                                  //Adjusted # of Drops to send to oPriceQuote. Work Around.
        //int iFrequency = 0;                                                                           //Every X of weeks - selected in DropDownList. Now stored as Page Property

        //Distribution variables
        bool useBusinesses = true;                                                                      //Flag to use Businesses in distribution.
        bool usePOBoxes = true;                                                                         //Flag to use PO Boxes in distribution.
        bool useResidential = true;                                                                     //Flag to use Residentials in distribution.
        int daysToAdd = 1;                                                                              //Used to logically find next drop date. AKA 'weekInterval'
        int totalDropSelections = 0;                                                                    //Used as a running total to store total 'matches' in the EDDM DataTable
        int totalAreaSelections = 0;

        //Quote params
        bool isFlatRateShipping = false;                                                                //Set by Quote Obj.
        decimal flatRateFee = 0;                                                                        //Comes from QuoteObj.ShipPrice.
        int flatRateShipQTY = 1000;                                                                     //Hard coded to 1000.



        //Get Design Type.  Set flags
        bool bProDesign = false;
        if (designType == "pro")
        { bProDesign = true; }

        //In OLB Environment, Template will ALWAYS be the design type.  Normally, designType is set by DropDownList on page.
        bool bTemplateDesign = false;
        if (designType == "template")
        { bTemplateDesign = true; }



        //Postage Rate
        postageRate = appxCMS.Util.AppSettings.GetDecimal("USPSPostageRate");
        if (postageRate <= 0)
        { postageRate = 0.16M; }



        //Calculate Stuff / Assign more stuff
        printQTY = CalculatePrintQTY(totalSelected, holdQTY, extraCopies);
        //daysToAdd = (7 * frequency);




        //Multiple Impressions Logic
        //If is Multiple Impressions, change the number of drops to match the number of impressions
        if (numImpressions == 1)
        {
            eddmObjCalc.IsThisAMultiple = false;
            eddmObjCalc.NumOfDrops = numDrops;
            eddmObjCalc.MailPieces = totalSelected;

            //Should always be Yes for OLB
            if (mailAllPiecesAtOnce.ToLower() == "yes")
            {
                eddmObjCalc.NumOfDrops = 1;
                numDrops = 1;
            }

        }

        else
        {
            eddmObjCalc.IsThisAMultiple = true;
            eddmObjCalc.NumOfDrops = numImpressions;
            eddmObjCalc.MailPieces = (totalSelected * numImpressions);
        }




        //The PriceQuote Obj ADDS on the drop fees based on the number of drops since we are NOT charging for Multiple 
        //Impressions on SOME sites set the number of drops to 1 for these sites.
        sendThisNumberOfDrops = eddmObjCalc.NumOfDrops;

        if (multipleImpressionsNoFee)
        { sendThisNumberOfDrops = 1; }

        if (numDrops == 0)
        { numDrops = 1; }



        //1) PRODUCT NODE


        //a) Build the Xml Document.  Load <cart /> parent node.
        XmlDocument oXML = new XmlDocument();
        oXML.LoadXml("<cart />");



        //Find and define 'the cart'. Doesn't yet exist in Profile.
        XmlNode oCart = oXML.SelectSingleNode("/cart");



        //b) Begin building Product Node components
        //1) Build Options SortedList (Int, Int)
        SortedList<int, int> productOptions;
        productOptions = new SortedList<int, int>();

        //2) Build OptionCategories List (of ProductOptionCategory)
        List<Taradel.ProductOptionCategory> oOptCats = Taradel.ProductDataSource.GetProductOptionCategories(baseProductID);


        //oPriceMatrix
        //3) Build PriceMatrix
        Taradel.PriceMatrix oPriceMatrix = Taradel.ProductDataSource.GetPriceRange(baseProductID, totalSelected);

        if (oPriceMatrix != null)
        {
            //c) Get PrintMethodID
            printMethodID = oPriceMatrix.PrintMethod.PrintMethodId;

            //d) Loop through OptionCategories, add to Options as needed.
            foreach (Taradel.ProductOptionCategory oOptCat in oOptCats)
            {
                IEnumerable<Taradel.ProductOption> oOpts = default(IEnumerable<Taradel.ProductOption>);
                oOpts = oOptCat.Options.Where((Taradel.ProductOption po) => po.ProductPrintMethodOptions.Any((ppmo => ppmo.PrintMethodReference.ForeignKey() == printMethodID)));
                if (oOpts.Count() > 0)
                {
                    productOptions.Add(oOptCat.OptCatID, 0);
                }
            }
        }

        else
        {
            pnlNormal.Visible = false;
            pnlError.Visible = true;
            litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error.";
            EmailUtility.SendAdminEmail("oPriceMatrix IS Null. (TargetDataMap3.aspx)");
        }



        //e) Get the product options from the Selected Product.  Get the OptCatID. ???? Already done??
        Taradel.ProductPriceQuote oPriceQuote = new Taradel.ProductPriceQuote(siteID, baseProductID, totalMailed, holdQTY, extraCopies, distributionID, sendThisNumberOfDrops, productOptions, oCust.ZipCode, bProDesign, bTemplateDesign, 0, "percent");




        //f) Build the Quote object
        //For Each oOpt As Taradel.PMOptionInfo In oPriceQuote.SelectedOptions
        foreach (Taradel.PMOptionInfo oOpt in oPriceQuote.SelectedOptions)
        {

            double dOptMarkup = 0;
            Boolean bPercent = false;

            if (oOpt.PriceMatrixOptionMarkup != null)
            {
                dOptMarkup = oOpt.BasePrice;
                bPercent = oOpt.BaseMarkupPercent;
            }
        }



        //g) Set variables returned from Quote Object.
        //Set Proper Weight, Price, Taxable Price, etc. These values and variables are needed/used later in the code.
        pricePerPiece = oPriceQuote.PricePerPiece;
        weight = oPriceQuote.Weight;
        price = oPriceQuote.Price;
        taxablePrice = CalculateTaxPrice(price, postageRate, totalMailed);
        isFlatRateShipping = oPriceQuote.IsFlatRateShipping;
        flatRateFee = oPriceQuote.ShipPrice;
        postageFee = (totalMailed * postageRate);
        dropFee = CalculateMultipleImpressionDropFee(eddmObjCalc.NumOfDrops, dropPrice);


        //Set Design Fee Attribute. Store 0 unless Pro was selected.
        if (designType.ToLower() != "pro")
        { designFee = 0; }




        //h) Finally, insert the Product Node into the Cart
        //ADD THE EDDM PRODUCT NODE

        oCart = (CartUtility.AddProduct(oCart, DateTime.Now.ToString(), baseProductID.ToString(), designFee.ToString(), distributionID.ToString(), flatRateFee.ToString(),
            flatRateShipQTY.ToString(), eddmGUID, isFlatRateShipping.ToString(), comments, jobName, productName, paperHeight.ToString(), paperWidth.ToString(),
            postageFee.ToString(), price.ToString(), pricePerPiece.ToString(), productID.ToString(), totalMailed.ToString(), siteID.ToString(),
            productSKU.ToString(), taxablePrice.ToString(), "EDDM", weight.ToString()));





        //2) EDDM Attribute Nodes
        foreach (Taradel.PMOptionInfo oOpt in oPriceQuote.SelectedOptions)
        {
            double dOptMarkup = 0;
            Boolean bPercent = false;

            if (oOpt.PriceMatrixOptionMarkup != null)
            {
                dOptMarkup = oOpt.BasePrice;
                bPercent = oOpt.BaseMarkupPercent;
            }

            oCart = (CartUtility.AddAttribute(oCart, "EDDM", oOpt.OptCatName, oOpt.OptCatId.ToString(), oOpt.OptionId.ToString(), oOpt.OptName, dOptMarkup.ToString(), bPercent.ToString(), oOpt.Weight.Value.ToString()));

        }




        //Professional Service Attribute
        if (designType == "pro")
        { oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Professional Design Services", "0", "1", paperWidth + "x" + paperHeight + " (" + designFee.ToString("C") + ")", designFee.ToString(), "False", "0")); }


        //Postage Fee Attribute
        oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Postage Fee", "0", postageFee.ToString(), eddmObjCalc.MailPieces.ToString("N0") + " pieces (" + postageFee.ToString("C") + ")", postageFee.ToString(), "False", "0"));



        //Number of Drops Attribute
        string dropLabel = "";

        if (numImpressions == 1)
        { dropLabel = " drop"; }

        else
        { dropLabel = " drops"; }

        oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Number of Drops", "0", eddmObjCalc.NumOfDrops.ToString(), (numImpressions.ToString() + dropLabel + " (" + extraDropFee.ToString("C") + ")"), dropFee.ToString(), "False", "0"));



        //Drop Schedule Attribute
        oCart = (CartUtility.AddAttribute(oCart, "EDDM", "Drop Schedule", "0", "every " + frequency.ToString() + " weeks", frequency.ToString() + " weeks", "0", "False", "0"));






        //3) EDDM ORDER CALC NODE
        oCart = (CartUtility.AddOrderCalc(oCart, "EDDM", "0", "0", oCust.State, "", "0", "0", "0", "0", "0"));




        //4) EDDM DROPS NODE

        //oDist
        Taradel.CustomerDistribution oDist = Taradel.CustomerDistributions.GetDistribution(distributionID);

        //oUselect
        Taradel.USelectProductConfiguration oUSelect = Taradel.Helper.USelect.GetProduct(oDist.USelectMethodReference.ForeignKey(), baseProductID);

        //oSummary obj
        Taradel.MapServer.UserData.SelectionSummary oSummary = Taradel.CustomerDistributions.GetSelectionSummary(oDist.ReferenceId);


        //oSelects
        List<Taradel.MapServer.UserData.AreaSelection> oSelects = Taradel.CustomerDistributions.GetSelections(oDist.ReferenceId);


        if (oSummary != null)
        {
            useBusinesses = oSummary.UseBusiness;
            usePOBoxes = oSummary.UsePOBox;
        }

        oCart = (CartUtility.AddDrops(oCart, "EDDM", oCust.Company, oCust.Address1, oCust.Address2, oCust.City, oCust.State, oCust.ZipCode, "", "", useResidential.ToString(), useBusinesses.ToString(), usePOBoxes.ToString()));




        //5) EDDM INDIV DROPS w/ Area Nodes
        int dropBreak = totalSelected / numImpressions;
        bool bOnePer = false;
        DateTime dropDate = startingDate;


        //Multiple impressions
        if (numImpressions > 1)
        {

            //this is a muliple - so divide back out the tempObjCalc.MailPieces for each drop
            eddmObjCalc.MailPieces = eddmObjCalc.MailPieces / eddmObjCalc.NumOfDrops;
            dropBreak = eddmObjCalc.MailPieces;
            bOnePer = false;


            //Loop through the drops
            for (int dropNumber = 1; dropNumber <= numImpressions; dropNumber++)
            {

                daysToAdd = ((dropNumber - 1) * (frequency * 7));
                dropDate = startingDate.AddDays(daysToAdd);

                //Find Prev Friday
                DateTime prevFridayDropDate = dropDate;
                while (prevFridayDropDate.DayOfWeek != DayOfWeek.Friday)
                { prevFridayDropDate = prevFridayDropDate.AddDays(-1); }

                //added 2/19/2016 
                dropDate = prevFridayDropDate;
                //end added 2/19/2016 

                //Add the Drop Nodes
                
                oCart = CartUtility.AddIndividualDrop(oCart, "EDDM", dropNumber.ToString(), totalMailed.ToString(), dropDate.ToShortDateString(), true, eddmObjCalc.NumOfDrops.ToString());

                Response.Write("totalMailed" + totalMailed.ToString() + "<br />");
                Response.Write("totalSelected" + totalSelected.ToString() + "<br />");
                

                //new --> rs 1/14/2016
                //CartUtility.AddIndividualDrop(oCart, "EDDM", dropNumber.ToString(), eddmObjCalc.MailPieces.ToString(), dropDate.ToShortDateString(), true, eddmObjCalc.NumOfDrops.ToString());
                //end the new

                //Loop Thru Areas
                foreach (Taradel.MapServer.UserData.AreaSelection oSelect in oSelects)
                {

                    try
                    {
                        totalAreaSelections = oSelect.Residential;

                        if (useBusinesses)
                        { totalAreaSelections = totalAreaSelections + oSelect.Business; }

                        if (usePOBoxes)
                        { totalAreaSelections = totalAreaSelections + oSelect.POBoxes; }


                        //Add each Area Node
                        oCart = CartUtility.AddIndividualArea(oCart, "EDDM", dropNumber.ToString(), oSelect.Name, oSelect.FriendlyName, totalAreaSelections.ToString());

                        totalDropSelections = totalDropSelections + totalAreaSelections;
                    }

                    catch (Exception ex)
                    { }

                }


                //Update total attribute with new value
                oCart = CartUtility.UpdateDropCount(oCart, "EDDM", dropNumber.ToString(), totalDropSelections.ToString());

                //reset
                totalDropSelections = 0;

            }

        }

            
        //single impression
        else
        {

            dropBreak = totalSelected / numImpressions;

            //oSelects.Count = # of Routes
            //If number of drops is the same as number as the number of routes AND is more than one.
            if (numImpressions == oSelects.Count && numImpressions > 1)
            { bOnePer = true; }


            for (int dropNumber = 1; dropNumber == numImpressions; dropNumber++)
            {

                DateTime prevFridayDropDate = startingDate;

                //Find the previous Friday
                while (prevFridayDropDate.DayOfWeek != DayOfWeek.Friday)
                { prevFridayDropDate = prevFridayDropDate.AddDays(-1); }



                //Add the Drop Nodes
                oCart = CartUtility.AddIndividualDrop(oCart, "EDDM", dropNumber.ToString(), totalMailed.ToString(), dropDate.ToShortDateString(), false, eddmObjCalc.NumOfDrops.ToString());


                //Loop Thru Areas
                foreach (Taradel.MapServer.UserData.AreaSelection oSelect in oSelects)
                {

                    try
                    {

                        totalAreaSelections = oSelect.Residential;

                        if (useBusinesses)
                        { totalAreaSelections = totalAreaSelections + oSelect.Business; }

                        if (usePOBoxes)
                        { totalAreaSelections = totalAreaSelections + oSelect.POBoxes; }


                        //Add each Area Node
                        oCart = CartUtility.AddIndividualArea(oCart, "EDDM", dropNumber.ToString(), oSelect.Name, oSelect.FriendlyName, totalAreaSelections.ToString());

                        totalDropSelections = totalDropSelections + totalAreaSelections;

                    }
                    catch (Exception ex)
                    { }

                }


                //Update total attribute with new value
                oCart = CartUtility.UpdateDropCount(oCart, "EDDM", dropNumber.ToString(), totalDropSelections.ToString());

                //reset
                totalDropSelections = 0;

            }

        }




        //6) EDDM DESIGN NODE
        //Upload and Save the file first and then ...update the cart.
        string sClientBase = Taradel.WLUtil.GetRelativeSiteImagesPath() + "/UserImages";
        string frontAction = "";
        string frontFileName = "";
        string frontFileExt = "";
        long frontFileSize = 0;
        string frontTmpName = "";
        string frontRealFileName = "";
        string backAction = "";
        string backFileName = "";
        string backFileExt = "";
        long backFileSize = 0;
        string backTmpName = "";
        string backRealFileName = "";
        bool hasBackDesign = false;
        string artKey = "";
        string requiresProof = "";



        if (!Directory.Exists(Server.MapPath(sClientBase)))
        { Directory.CreateDirectory(Server.MapPath(sClientBase)); }


        string sClientFolder = sClientBase + "/" + Profile.UserName.Replace("@", "_");
        string sClientPath = Server.MapPath(sClientFolder);


        DirectoryInfo oDir = new DirectoryInfo(sClientPath);
        if (!oDir.Exists)
        { oDir.Create(); }


        switch (designType.ToLower())
        {
            case "my":
                //logic ommitted because OLB always uses a Template
                break;

            case "template":
                artKey = hidSelectedTemplateID.Value;
                break;

        }


        //Add Design Node
        oCart = (CartUtility.AddDesign(oCart, "EDDM", designType, frontFileExt, frontFileName, frontRealFileName, frontAction, hasBackDesign, backFileExt, backFileName, backRealFileName, backAction, artKey, requiresProof));






        //7) EDDM SHIPMENTS NODE
        WeightPerPage = ((weight * pageCount) / totalSelected);

        if (extraCopies > 0)
        {

            if (deliveryAddressID == "0")
            {
                //Create a new address and get the ID
                string sMsg = "";
                int iAddr = Taradel.CustomerAddressDataSource.NewAddress(oCust.CustomerID, "", "", "", "", oCust.Address1.ToString(), oCust.Address2.ToString(), oCust.City.ToString(), oCust.State.ToString(), oCust.ZipCode.ToString(), ref sMsg);

                if (iAddr > 0)
                { deliveryAddressID = iAddr.ToString(); }

            }

        }

        oCart = (CartUtility.AddShipments(oCart, "EDDM", deliveryAddressID.ToString(), extraCopies.ToString(), weight.ToString(), pageCount.ToString(), paperWidth.ToString(), paperHeight.ToString(), flatRateFee.ToString("N2"), oCust.ZipCode, totalMailed));



        Profile.Cart = oXML;
        Profile.Save();

    }



    //Debug
    private void ShowDebug(bool debug)
    {

        if (debug)
        {
            pnlDevData.Visible = true;

            StringBuilder pageProps = new StringBuilder();
            pageProps.Append("distributionID: " + distributionID.ToString() + "<br />");
            pageProps.Append("totalSelected: " + totalSelected.ToString() + "<br />");
            pageProps.Append("multipleImpressionsNoFee: " + multipleImpressionsNoFee.ToString() + "<br />");
            pageProps.Append("productID: " + productID.ToString() + "<br />");
            pageProps.Append("baseProductID: " + baseProductID.ToString() + "<br />");
            pageProps.Append("printMethodID: " + printMethodID.ToString() + "<br />");
            pageProps.Append("siteID: " + siteID.ToString() + "<br />");
            pageProps.Append("totalMailed: " + totalMailed.ToString() + "<br />");
            pageProps.Append("productSKU: " + productSKU.ToString() + "<br />");
            pageProps.Append("designFee: " + designFee.ToString() + "<br />");
            pageProps.Append("productName: " + productName.ToString() + "<br />");
            pageProps.Append("paperHeight: " + paperHeight.ToString() + "<br />");
            pageProps.Append("paperWidth: " + paperWidth.ToString() + "<br />");
            pageProps.Append("pricePerPiece: " + pricePerPiece.ToString() + "<br />");
            pageProps.Append("numImpressions: " + numImpressions.ToString() + "<br />");
            pageProps.Append("extraDropFee: " + extraDropFee.ToString() + "<br />");
            pageProps.Append("dropPrice: " + dropPrice.ToString() + "<br />");
            pageProps.Append("pageCount: " + pageCount.ToString() + "<br />");
            pageProps.Append("frequency: " + frequency.ToString() + "<br />");

            litPageProps.Text = "Page Properties:<br />" + pageProps.ToString() + "<br /><br />";


            StringBuilder sessionStuff = new StringBuilder();
            sessionStuff.Append("AvgMatch: " + RetrieveFromSession(TaradelSession.AvgMatch) + "<br />");
            sessionStuff.Append("Budget: " + RetrieveFromSession(TaradelSession.Budget) + "<br />");
            sessionStuff.Append("Campaign: " + RetrieveFromSession(TaradelSession.Campaign) + "<br />");
            sessionStuff.Append("DistID: " + RetrieveFromSession(TaradelSession.DistID) + "<br />");
            sessionStuff.Append("EstCost: " + RetrieveFromSession(TaradelSession.EstCost) + "<br />");
            sessionStuff.Append("Frequency: " + RetrieveFromSession(TaradelSession.Frequency) + "<br />");
            sessionStuff.Append("Impressions: " + RetrieveFromSession(TaradelSession.Impressions) + "<br />");
            sessionStuff.Append("Location: " + RetrieveFromSession(TaradelSession.Location) + "<br />");
            sessionStuff.Append("MapName: " + RetrieveFromSession(TaradelSession.MapName) + "<br />");
            sessionStuff.Append("PricePerPiece: " + RetrieveFromSession(TaradelSession.PricePerPiece) + "<br />");
            sessionStuff.Append("ProductID: " + RetrieveFromSession(TaradelSession.ProductID) + "<br />");
            sessionStuff.Append("BaseProductID: " + RetrieveFromSession(TaradelSession.BaseProductID) + "<br />");
            sessionStuff.Append("QTY: " + RetrieveFromSession(TaradelSession.QTY) + "<br />");
            sessionStuff.Append("RevisedMap: " + RetrieveFromSession(TaradelSession.RevisedMap) + "<br />");
            sessionStuff.Append("StartDate: " + RetrieveFromSession(TaradelSession.StartDate) + "<br />");
            sessionStuff.Append("Template: " + RetrieveFromSession(TaradelSession.Template) + "<br />");
            sessionStuff.Append("TotalMailed: " + RetrieveFromSession(TaradelSession.TotalMailed) + "<br />");
            sessionStuff.Append("TotSelected: " + RetrieveFromSession(TaradelSession.TotSelected) + "<br />");

            litSessionVars.Text = "Session Variables:<br />" + sessionStuff.ToString();

        }

    }





    //Classes
    #region templateclasses
    public class EntityKeyValue
    {
        public string Key { get; set; }
        public int Value { get; set; }
    }

    public class EntityKey
    {
        public string id { get; set; }
        public string EntityContainerName { get; set; }
        public List<EntityKeyValue> EntityKeyValues { get; set; }
        public string EntitySetName { get; set; }
    }

    public class IndustryReference
    {
        public EntityKey EntityKey { get; set; }
    }

    public class EntityKeyValue2
    {
        public string Key { get; set; }
        public int Value { get; set; }
    }

    public class EntityKey2
    {
        public string id { get; set; }
        public string EntityContainerName { get; set; }
        public List<EntityKeyValue2> EntityKeyValues { get; set; }
        public string EntitySetName { get; set; }
    }

    public class BusinessLine
    {
        public string id { get; set; }
        public int BusinessLineId { get; set; }
        public object Industry { get; set; }
        public int IndustryId { get; set; }
        public IndustryReference IndustryReference { get; set; }
        public string Name { get; set; }
        public List<object> Templates { get; set; }
        public EntityKey2 EntityKey { get; set; }
        public string refid { get; set; }
    }

    public class Template
    {
        public string BackImage { get; set; }
        public BusinessLine BusinessLine { get; set; }
        public int BusinessLineId { get; set; }
        public object Description { get; set; }
        public string FoldType { get; set; }
        public string FrontImage { get; set; }
        public string InsideImage { get; set; }
        public string Name { get; set; }
        public string PageSize { get; set; }
        public string ProductType { get; set; }
        public object Summary { get; set; }
        public int TemplateId { get; set; }
        public int TemplateSizeId { get; set; }
    }

    public class RootObject
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<Template> Templates { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }

    #endregion


}