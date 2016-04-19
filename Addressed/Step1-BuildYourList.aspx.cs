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
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Globalization;
using appxCMS;


public partial class BuildList : appxCMS.PageBase
{

    //============================================================================================================
    //============================================================================================================



    // Fields
    protected int uSelectTypeID = 6;
    protected int siteID = appxCMS.Util.CMSSettings.GetSiteId();





    //Methods
    protected void Page_Load(object sender, EventArgs e)
    {

        //get SiteDetails
        SiteUtility.SiteDetails siteDetails = new SiteUtility.SiteDetails();
        siteDetails = SiteUtility.RetrieveSiteSettings(siteID);


        //Look for cookie w/ Distribution which will help reset filters.
        int prevDistributionID = CheckForCookie();
        if (prevDistributionID > 0)
        { 
            //Dist created within current visit.  Reset filters to match.
            PreselectFilters(prevDistributionID);
        }



        if (!(Page.IsPostBack))
        {

            //wire up controls
            ddlRadiusValue.Attributes.Add("onchange", "RadiusValueChanged();");
            ddlRadiusType.Attributes.Add("onchange", "RadiusTypeChanged();");
            txtAddress.Attributes.Add("onchange", "AddressChanged();");
            txtZipCode.Attributes.Add("onchange", "ZipCodeChanged();");
            txtZipCodesList.Attributes.Add("onchange", "ZipCodesListChanged();");
            txtListName.Attributes.Add("placeholder","name this list");
            radAddress.Attributes.Add("onchange", "TargetAreaTypeChanged();");
            radZipCodes.Attributes.Add("onchange", "TargetAreaTypeChanged();");

     
            //Order Steps
            OrderSteps.numberOfSteps = 5;
            OrderSteps.step1Text = "1) Define Area";
            OrderSteps.step1Url = "/Addressed/Step1-BuildYourList.aspx";
            OrderSteps.step1State = "current";
            OrderSteps.step1Icon = "fa-map-marker";

            OrderSteps.step2Text = "2) Define Customers";
            OrderSteps.step2Url = "/Addressed/Step1-BuildYourList.aspx";
            OrderSteps.step2State = "";
            OrderSteps.step2Icon = "fa-user";

            OrderSteps.step3Text = "3) Choose Product";
            OrderSteps.step3Url = "";
            OrderSteps.step3State = "";
            OrderSteps.step3Icon = "fa-folder";

            OrderSteps.step4Text = "4) Define Delivery";
            OrderSteps.step4Url = "";
            OrderSteps.step4State = "";
            OrderSteps.step4Icon = "fa-envelope";

            OrderSteps.step5Text = "5) Check Out";
            OrderSteps.step5Url = "";
            OrderSteps.step5State = "";
            OrderSteps.step5Icon = "fa-credit-card";


            //SitePhoneNumber
            SitePhoneNumber.addCallUs = "true";
            SitePhoneNumber.useIcon = "true";
            SitePhoneNumber.makeHyperLink = "true";



            //Page Header
            if (!siteDetails.UseRibbonBanners)
            {
                PageHeader.headerType = "simple";
                PageHeader.simpleHeader = "Build Your List";
            }

            else
            {
                PageHeader.headerType = "partial";
                PageHeader.mainHeader = "Build Your List";
                PageHeader.subHeader = "Start Here";
            }



            //Preload "List Name".
            txtListName.Text = "Addressed List " + DateTime.Today.ToShortDateString();


            hypEmail.Text = "<span class=" + Convert.ToChar(34) + "fa fa-envelope" + Convert.ToChar(34) + "></span>&nbsp;" + siteDetails.SupportEmail;
            hypEmail.NavigateUrl = "mailto:" + siteDetails.SupportEmail;


        }


        if (siteDetails.TestMode)
        { 
            ShowDebug();
            txtAddress.Text = "4805 Lake Brooke Drive";
            txtZipCode.Text = "2306";
        }

        else 
        {
            if (!String.IsNullOrEmpty(Request.QueryString["debug"])) 
            {
                if (Request.QueryString["debug"] == "hodor")
                { ShowDebug(); }
            }
        }


    }



    private int CheckForCookie()
    {

        int results = 0;

        HttpCookie taradelDistIDCookie = Request.Cookies["TaradelDistributionID"];
        if (taradelDistIDCookie != null)
        {
            Int32.TryParse(taradelDistIDCookie.Value, out results);
            litDistIDCookie.Text = "<strong>Cookie Found</strong>.  Dist ID: " + taradelDistIDCookie.Value;
        }

        else
        { litDistIDCookie.Text = "No Cookie Found"; }

        return results;

    }



    protected void btnContinue_Click(object sender, EventArgs e)
    {

        //STEP 1
        //Insert into pnd_SavedAddressedSelections table (ValMap-DEV)- return ReferenceID via Stored Procedure
        
        //STEP 2
        //Insert into pnd_CustomerDistribution table (Redesign db) with ReferenceID and return DistributionID

        //STEP 3
        //Insert into pnd_SavedAddressedListFilters table

        //STEP 4
        //Set cookie to store DistributionID


        string referenceID = CreateDistribution();
        int distributionID = 0;
        
        //Something went wrong inserting the SavedAddressedSelection
        if(String.IsNullOrEmpty(referenceID))
        {
            
            pnlFilterData.Visible = false;
            pnlError.Visible = true;
            litErrorMessage.Text = "We're sorry but something went wrong while saving your selection.  Our IT Staff has been notified and you will be contacted very shortly about this error.";
            EmailUtility.SendAdminEmail("Error in CreateDistribution on BuildList page.");

            pnlSysError.Visible = true;
            litSysError.Text = "Error - No reference ID was returned."; 
        }

        //STEP 1 - insert into Map Server went OK
        else
        {
            pnlFilterData.Visible = true;
            pnlError.Visible = false;

            //STEP 2
            distributionID = SaveCustomerDistribution(referenceID);

            //STEP 3 
            SaveCustomerFilterSelections(distributionID);


            //Something went wrong with saving the customer selection.
            if (distributionID == 0)
            {

                pnlFilterData.Visible = false;
                pnlError.Visible = true;
                litErrorMessage.Text = "We're sorry but something went wrong while saving your selection.  Our IT Staff has been notified and you will be contacted very shortly about this error.";
                EmailUtility.SendAdminEmail("Error in SaveCustomerSelection on BuildList page.");

                pnlSysError.Visible = true;
                litSysError.Text = "Error - No Distribution ID was returned."; 

            }
    
            //All is good. Let's go!
            else
            {
                
                //Delete any existing cookies.
                if (Request.Cookies["TaradelDistributionID"] != null)
                {
                    HttpCookie taradelDistIDCookie = new HttpCookie("TaradelDistributionID");
                    taradelDistIDCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(taradelDistIDCookie);
                }

                //Set NEW cookie. Will expire in 20 minutes.
                HttpCookie newTaradelDistIDCookie = new HttpCookie("TaradelDistributionID");
                newTaradelDistIDCookie.Expires = DateTime.Now.AddMinutes(20);
                newTaradelDistIDCookie.Value = distributionID.ToString();
                Response.Cookies.Add(newTaradelDistIDCookie);
                //End =================


                Response.Redirect("~/Step1-TargetReview.aspx?distid=" + distributionID); 

            }


        }


    }



    protected string CreateDistribution()
    {

        //Method will insert data into SavedAddressedSelections table and return the ReferenceID of the successfully inserted record.
        StringBuilder insertSQL = new StringBuilder();

        string referenceID = Guid.NewGuid().ToString();
        string returnedRefID = "";
        string IPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string filterURL = txtAPIUrl.Text;
        string targetType = "";
        string startAddress = txtAddress.Text;
        string startZipCode = txtZipCode.Text;
        string startTargetedZipCodes = txtZipCodesList.Text;
        string radiusType = ddlRadiusType.SelectedValue;
        string jsonData = "";
        int radiusValue = Convert.ToInt32(ddlRadiusValue.SelectedValue);

        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["MapServerConn"].ToString();  //<--- DEV Database!


        if (radAddress.Checked)
        { targetType = "Address"; }
        else
        { targetType = "ZipCode"; }


        insertSQL.Append("EXEC sp_InsertSavedAddressedSelections ");
        insertSQL.Append("@paramReferenceID='" + referenceID + "', ");
        insertSQL.Append("@paramCreatedIP='" + IPAddress + "', ");
        insertSQL.Append("@paramFilterURL='" + filterURL + "',");
        insertSQL.Append("@paramTargetType='" + targetType + "',");
        insertSQL.Append("@paramStartAddress='" + startAddress + "',");
        insertSQL.Append("@paramZipCode='" + startZipCode + "',");
        insertSQL.Append("@paramStartTargetedZipCodes='" + startTargetedZipCodes + "',");
        insertSQL.Append("@paramRadiusType='" + radiusType + "',");
        insertSQL.Append("@paramRadiusValue=" + radiusValue + ",");
        insertSQL.Append("@paramJSONData='" + jsonData + "'");

            
        SqlConnection connectionObj = new SqlConnection(connectString);
        SqlCommand sqlCommand = new SqlCommand();

        sqlCommand.Connection = connectionObj;
        sqlCommand.CommandText = insertSQL.ToString();
        
        connectionObj.Open();
        returnedRefID = sqlCommand.ExecuteScalar().ToString();
        connectionObj.Close();

        return referenceID;

    }



    protected int SaveCustomerDistribution(string referenceID)
    {
        //This method inserts the data into pnd_CustomerDistribution and returns the new ID.


        Taradel.Customer custObj = Taradel.CustomerDataSource.GetCustomer(User.Identity.Name);

        int distributionID = 0;
        int customerID = custObj.CustomerID;
        int totalMatches = Convert.ToInt32(txtCount.Text);
        string listName = txtListName.Text;

        using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ConnectionString))
        {

            oConn.Open();

            using (SqlCommand oCmd = new SqlCommand("usp_InsertCustomerDistribution", oConn))
            {

                oCmd.Parameters.AddWithValue("@CustomerId", customerID);
                oCmd.Parameters.AddWithValue("@DistName", listName);
                oCmd.Parameters.AddWithValue("@DistRefId", referenceID);
                oCmd.Parameters.AddWithValue("@TotalDeliveries", totalMatches);
                oCmd.Parameters.AddWithValue("@USelectId", uSelectTypeID);
                oCmd.Parameters.Add("@OutputID", SqlDbType.Int).Direction = ParameterDirection.Output;
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.ExecuteScalar();
                distributionID = Convert.ToInt32(oCmd.Parameters["@OutputID"].Value);
            }

            oConn.Close();

        }

        return distributionID;

    }



    protected void SaveCustomerFilterSelections(int distributionID)
    {

        //Determine if filter was actually selected or not. Insert it only if was selected.
        
        //NOTE - None of the filters are actually required.  It is possible for a SavedAddressedList Distribution List to exist with NO saved filters.  User simply
        //did not select anything.


        //Read the API URL.  Split it up.  Determine if anything was actually selected.
        string filterURL = txtAPIUrl.Text;
        string[] filterParams = filterURL.Split('&');

        foreach (string param in filterParams)
        { 

            //Insert as needed
            //Home Ownership
            if (param.StartsWith("HomeOwnership"))
            {
                if (param.Length > 14)
                {
                    string filterValue = param.Substring((param.IndexOf("=")+1),1);
                    InsertCustomerFilterSelection(1, distributionID, filterValue, 0, 0); 
                }
            }


            //Marital Status
            if (param.StartsWith("MaritalStatus"))
            {
                if (param.Length > 14)
                {
                    string filterValue = param.Substring((param.IndexOf("=") + 1), 1);
                    InsertCustomerFilterSelection(2, distributionID, filterValue, 0, 0);
                }
            }


            //Children
            if (param.StartsWith("KidsPresent"))
            {
                if (param.Length > 12)
                {
                    string filterValue = param.Substring((param.IndexOf("=") + 1), 1);
                    InsertCustomerFilterSelection(3, distributionID, filterValue, 0, 0);
                }
            }


            //Gender
            if (param.StartsWith("Gender"))
            {
                if (param.Length > 7)
                {
                    string filterValue = param.Substring((param.IndexOf("=") + 1), 1);
                    InsertCustomerFilterSelection(4, distributionID, filterValue, 0, 0);
                }
            }


            //Income
            if (param.StartsWith("Income"))
            {
                if (param.Length > 7)
                {
                    int minVal = Convert.ToInt32(txtMinIncome.Text);
                    int maxVal = Convert.ToInt32(txtMaxIncome.Text);
                    string filterValue = param.Substring((param.IndexOf("=") + 1), (param.Length - (param.IndexOf("=") + 1)));

                    InsertCustomerFilterSelection(5, distributionID, filterValue, minVal, maxVal);
                }
            }


            //Age
            if (param.StartsWith("AgeRange"))
            {
                if (param.Length > 9)
                {
                    int minVal = Convert.ToInt32(txtMinAge.Text);
                    int maxVal = Convert.ToInt32(txtMaxAge.Text);
                    string filterValue = param.Substring((param.IndexOf("=") + 1), (param.Length - (param.IndexOf("=") + 1)));

                    InsertCustomerFilterSelection(7, distributionID, filterValue, minVal, maxVal);
                }
            }


            //Property Value "HomeMktVal"
            if (param.StartsWith("HomeMktVal"))
            {
                if (param.Length > 11)
                {
                    string filterValue = param.Substring((param.IndexOf("=") + 1), (param.Length - (param.IndexOf("=") + 1)));
                    InsertCustomerFilterSelection(8, distributionID, filterValue, 0, 0);
                }
            }


            //NetWorth
            if (param.StartsWith("NetWorth"))
            {
                if (param.Length > 9)
                {
                    string filterValue = param.Substring((param.IndexOf("=") + 1), (param.Length - (param.IndexOf("=") + 1)));
                    InsertCustomerFilterSelection(9, distributionID, filterValue, 0, 0);
                }
            }


            //Ethnicity
            if (param.StartsWith("Ethnicity"))
            {
                if (param.Length > 10)
                {
                    string filterValue = param.Substring((param.IndexOf("=") + 1), (param.Length - (param.IndexOf("=") + 1)));
                    InsertCustomerFilterSelection(10, distributionID, filterValue, 0, 0);
                }
            }

        }


    }



    private void InsertCustomerFilterSelection(int addressedListFilterCategoryID, int distributionID, string filterValue, int minVal, int maxVal)
    {


        StringBuilder insertSQL = new StringBuilder();
        string connectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();  


        insertSQL.Append("EXEC usp_InsertSavedAddressedListFilter ");
        insertSQL.Append("@paramAddressedListFilterCategoryID=" + addressedListFilterCategoryID + ", ");
        insertSQL.Append("@paramDistributionID=" + distributionID + ", ");
        insertSQL.Append("@paramFilterValue='" + filterValue + "',");
        insertSQL.Append("@paramMinVal=" + minVal + ",");
        insertSQL.Append("@paramMaxVal=" + maxVal);


        SqlConnection connectionObj = new SqlConnection(connectString);
        SqlCommand sqlCommand = new SqlCommand();

        sqlCommand.Connection = connectionObj;
        sqlCommand.CommandText = insertSQL.ToString();

        connectionObj.Open();
        sqlCommand.ExecuteNonQuery();
        connectionObj.Close();

    
    }



    protected void PreselectFilters(int prevDistributionID)
    {
        //Fills hidden textboxes with stored filter values.

        //Take DistID and create SqlDataReader
        StringBuilder getSQL = new StringBuilder();
        getSQL.Append("SELECT [pnd_AddressedListFilterCategories].[CategoryName], [pnd_AddressedListFilterCategories].[FilterParam], ");
        getSQL.Append("[pnd_AddressedListFilterCategories].[DataType], [pnd_SavedAddressedListFilters].[SavedAddressedListFilterID], "); 
        getSQL.Append("[pnd_SavedAddressedListFilters].[AddressedListFilterCategoryID], [pnd_SavedAddressedListFilters].[DistributionID],  ");
        getSQL.Append("[pnd_SavedAddressedListFilters].[MinVal], [pnd_SavedAddressedListFilters].[MaxVal], [pnd_SavedAddressedListFilters].[FilterValue],  ");
        getSQL.Append("[pnd_SavedAddressedListFilters].[CreatedOn] ");
        getSQL.Append("FROM [pnd_SavedAddressedListFilters] ");
        getSQL.Append("INNER JOIN [pnd_AddressedListFilterCategories] ON [pnd_SavedAddressedListFilters].[AddressedListFilterCategoryID] = [pnd_AddressedListFilterCategories].[AddressedListFilterCategoryID] ");
        getSQL.Append("WHERE ([pnd_SavedAddressedListFilters].[DistributionID] = " + prevDistributionID  + ") ");
        getSQL.Append("ORDER BY [pnd_AddressedListFilterCategories].[FilterParam] ");


        string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["TaradelWLConnectionString"].ToString();
        SqlConnection connectionObj = new SqlConnection(sqlConnectString);
        SqlCommand getFilters = new SqlCommand();
        getFilters.CommandText = getSQL.ToString();
        getFilters.Connection = connectionObj;


        try
        {
        
            connectionObj.Open();
            SqlDataReader filterData = getFilters.ExecuteReader();
            StringBuilder debugFilterData = new StringBuilder();

            if (!filterData.HasRows)
            {
                //Nothing to do here.
            }

            else
            {
                while (filterData.Read())
                {
                    debugFilterData.Append(filterData["FilterParam"].ToString() + ": " + filterData["FilterValue"].ToString() + ", Min: " + filterData["MinVal"].ToString() + ", Max: "  + filterData["MaxVal"].ToString() + "<br />");

                    //homeownership
                    if (filterData["FilterParam"].ToString().ToLower() == "homeownership")
                    { txtHomeOwnership.Text = filterData["FilterValue"].ToString(); }


                    //marital status
                    if (filterData["FilterParam"].ToString().ToLower() == "maritalstatus")
                    { txtMartialStatus.Text = filterData["FilterValue"].ToString(); }


                    //childred
                    if (filterData["FilterParam"].ToString().ToLower() == "kidspresent")
                    { txtChildren.Text = filterData["FilterValue"].ToString(); }


                    //gender
                    if (filterData["FilterParam"].ToString().ToLower() == "gender")
                    { txtGender.Text = filterData["FilterValue"].ToString(); }


                    //income
                    if (filterData["FilterParam"].ToString().ToLower() == "income")
                    {
                        txtCombinedIncome.Text = filterData["FilterValue"].ToString(); 
                        txtMinIncome.Text = filterData["MinVal"].ToString();
                        txtMaxIncome.Text = filterData["MaxVal"].ToString();
                    }


                    //age range
                    if (filterData["FilterParam"].ToString().ToLower() == "agerange")
                    {
                        txtAgeRanges.Text = filterData["FilterValue"].ToString();
                        txtMinAge.Text = filterData["MinVal"].ToString();
                        txtMaxAge.Text = filterData["MaxVal"].ToString();
                    }


                    //property value / home market value
                    if (filterData["FilterParam"].ToString().ToLower() == "homemktval")
                    {
                        txtPropertyValue.Text = filterData["FilterValue"].ToString();

                        int count = Convert.ToInt16(filterData["FilterValue"].ToString().Replace(",","").Length);
                        txtPropValueTotalSelected.Text = count.ToString();
                        hidPropValueTotalSelected.Value = count.ToString();
                    }

                    
                    //net worth
                    if (filterData["FilterParam"].ToString().ToLower() == "networth")
                    {
                        txtNetWorth.Text = filterData["FilterValue"].ToString();

                        int count = Convert.ToInt16(filterData["FilterValue"].ToString().Replace(",", "").Length);
                        txtNetWorthTotalSelected.Text = count.ToString();
                        hidNetWorthTotalSelected.Value = count.ToString();
                    }


                    //ethnicity
                    if (filterData["FilterParam"].ToString().ToLower() == "ethnicity")
                    {
                        txtEthnicity.Text = filterData["FilterValue"].ToString();

                        int count = Convert.ToInt16(filterData["FilterValue"].ToString().Replace(",", "").Length);
                        txtTotalEthnicitySelected.Text = count.ToString();
                        hidTotalEthnicitySelected.Value = count.ToString();
                    }



                }

                litFilterData.Text = debugFilterData.ToString() + "<br />SQL:<br />" + getSQL.ToString(); ;

            }
            
            filterData.Close();

        }

        catch (Exception objException)
        {

            StringBuilder errorMsg = new StringBuilder();

            errorMsg.AppendLine("The following errors occurred:<br />");
            errorMsg.AppendLine("Message: " + objException.Message + "<br />");
            errorMsg.AppendLine("Source: " + objException.Source + "<br />");
            errorMsg.AppendLine("Stack Trace: " + objException.StackTrace + "<br />");
            errorMsg.AppendLine("Target Site: " + objException.TargetSite.Name + "<br />");
            errorMsg.AppendLine("SQL: " + getSQL.ToString() + "<br />");

            pnlSysError.Visible = true;
            litSysError.Text = errorMsg.ToString();

        }


        finally
        { connectionObj.Close(); }



    }



    protected void ShowDebug()
    { 
        pnlDebug.CssClass = String.Empty;
    }



    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        Session.Clear();

        HttpCookie taradelDistIDCookie = new HttpCookie("TaradelDistributionID");
        taradelDistIDCookie.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(taradelDistIDCookie);

        Response.Redirect("Step1-BuildYourList.aspx");

    }

}
