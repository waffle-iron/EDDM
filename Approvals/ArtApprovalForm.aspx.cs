using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Approvals_ArtApprovalForm : appxCMS.PageBase
{



    //Fields
    protected bool debug = true;






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



    
    //Methods
    protected void Page_Init(object sender, EventArgs e)
    { SetPageProperties(); }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

            //For testing and debugging
            if (currentMode.ToLower() == "dev")
            {

                if (debug)
                {
                    txtFullName.Text = "John Q Doe";
                    txtCompanyName.Text = "ACME Inc";
                    txtOrderNumber.Text = "12345";
                    txtDesigner.Text = "Jane Designer";
                    ddlExperience.SelectedValue = "5";
                    ddlRecommend.SelectedValue = "6";
                    chkConfirm.Checked = true;
                    txtComments.Text = "I love my custom design.";
                }

            }


            //Build page elements
            switch (siteID)
            {
                //EDDM
                case 1:
                    imgHeaderImage.ImageUrl = "/cmsimages/1/quote-header-logo.png";
                    imgHeaderImage.AlternateText = "EveryDoorDirectMail.com";
                    litCompanyNotResponsible.Text = "Taradel";
                    litSiteNameDisclaimer.Text = "Taradel";
                    litCompanyDisclaimer.Text = "Taradel LLC";
                    litCompanyDisclaimer2.Text = "Taradel LLC";
                    litCompanyDisclaimer3.Text = "Taradel LLC";
                    litCompanyDisclaimer4.Text = "Taradel";
                    litCompanyDisclaimer5.Text = "Taradel";
                    litCompanyDisclaimer6.Text = "Taradel";
                    litCompanyDisclaimer7.Text = "Taradel";
                    imgFooterLogo.ImageUrl = "/cmsimages/1/quote-footer-logo.png";
                    litFooterCompany.Text = "<address><small>Copyright 2015 Taradel, LLC. <br />All rights reserved.</small></address>";
                    litFooterTrademarks.Text = "<address><small>EVERY DOOR DIRECT MAIL&reg;, EDDM®, EDDM RETAIL&reg;, EDDM BMEU&reg;, UNITED STATES POSTAL SERVICE&reg;, U.S. POSTAL SERVICE&reg;, USPS&reg;, U.S.  POST OFFICE&reg;, POST OFFICE, and ZIP CODE&trade; are trademarks of the United States Postal Service&reg;and are used with permission under license.</small></address>";
                    break;


                //Staples Act Mgr
                case 78:
                    pnlDesigner.Visible = false;
                    pnlExperience.Visible = false;
                    pnlRecommend.Visible = false;

                    imgHeaderImage.ImageUrl = "/cmsimages/78/quote-header-logo.png";
                    imgHeaderImage.AlternateText = "Staples Every Door Direct Mail";
                    litCompanyNotResponsible.Text = "Staples";
                    litSiteNameDisclaimer.Text = "Staples Inc.";
                    litCompanyDisclaimer.Text = "Staples";
                    litCompanyDisclaimer2.Text = "Staples";
                    litCompanyDisclaimer3.Text = "Staples";
                    litCompanyDisclaimer4.Text = "Staples";
                    litCompanyDisclaimer5.Text = "Staples";
                    litCompanyDisclaimer6.Text = "Staples";
                    litCompanyDisclaimer7.Text = "Staples";
                    imgFooterLogo.ImageUrl = "/cmsimages/78/quote-footer-logo.png";
                    litFooterCompany.Text = "<address><small>Copyright 2015 Staples Inc. <br />All rights reserved.</small></address>";
                    litFooterTrademarks.Text = "<address><small>EVERY DOOR DIRECT MAIL&reg;, EDDM®, EDDM RETAIL&reg;, EDDM BMEU&reg;, UNITED STATES POSTAL SERVICE&reg;, U.S. POSTAL SERVICE&reg;, USPS&reg;, U.S.  POST OFFICE&reg;, POST OFFICE, and ZIP CODE&trade; are trademarks of the United States Postal Service&reg;and are used with permission under license.</small></address>";
                    break;


                //Staples Store
                case 91:
                    pnlDesigner.Visible = false;
                    pnlExperience.Visible = false;
                    pnlRecommend.Visible = false;

                    imgHeaderImage.ImageUrl = "/cmsimages/91/quote-header-logo.png";
                    imgHeaderImage.AlternateText = "Staples Every Door Direct Mail";
                    litCompanyNotResponsible.Text = "Staples";
                    litSiteNameDisclaimer.Text = "Staples Inc.";
                    litCompanyDisclaimer.Text = "Staples";
                    litCompanyDisclaimer2.Text = "Staples";
                    litCompanyDisclaimer3.Text = "Staples";
                    litCompanyDisclaimer4.Text = "Staples";
                    litCompanyDisclaimer5.Text = "Staples";
                    litCompanyDisclaimer6.Text = "Staples";
                    litCompanyDisclaimer7.Text = "Staples";
                    imgFooterLogo.ImageUrl = "/cmsimages/91/quote-footer-logo.png";
                    litFooterCompany.Text = "<address><small>Copyright 2015 Staples Inc. <br />All rights reserved.</small></address>";
                    litFooterTrademarks.Text = "<address><small>EVERY DOOR DIRECT MAIL&reg;, EDDM®, EDDM RETAIL&reg;, EDDM BMEU&reg;, UNITED STATES POSTAL SERVICE&reg;, U.S. POSTAL SERVICE&reg;, USPS&reg;, U.S.  POST OFFICE&reg;, POST OFFICE, and ZIP CODE&trade; are trademarks of the United States Postal Service&reg;and are used with permission under license.</small></address>";
                    break;

                //Staples Direct Mail - TEMP SITE - LIST DEVELOPMENT
                case 83:
                    pnlDesigner.Visible = false;
                    pnlExperience.Visible = false;
                    pnlRecommend.Visible = false;

                    imgHeaderImage.ImageUrl = "/cmsimages/93/quote-header-logo.png";
                    imgHeaderImage.AlternateText = "Staples Every Door Direct Mail";
                    litCompanyNotResponsible.Text = "Staples";
                    litSiteNameDisclaimer.Text = "Staples Inc.";
                    litCompanyDisclaimer.Text = "Staples";
                    litCompanyDisclaimer2.Text = "Staples";
                    litCompanyDisclaimer3.Text = "Staples";
                    litCompanyDisclaimer4.Text = "Staples";
                    litCompanyDisclaimer5.Text = "Staples";
                    litCompanyDisclaimer6.Text = "Staples";
                    litCompanyDisclaimer7.Text = "Staples";
                    imgFooterLogo.ImageUrl = "/cmsimages/93/quote-footer-logo.png";
                    litFooterCompany.Text = "<address><small>Copyright 2015 Staples Inc. <br />All rights reserved.</small></address>";
                    litFooterTrademarks.Text = "<address><small>EVERY DOOR DIRECT MAIL&reg;, EDDM®, EDDM RETAIL&reg;, EDDM BMEU&reg;, UNITED STATES POSTAL SERVICE&reg;, U.S. POSTAL SERVICE&reg;, USPS&reg;, U.S.  POST OFFICE&reg;, POST OFFICE, and ZIP CODE&trade; are trademarks of the United States Postal Service&reg;and are used with permission under license.</small></address>";
                    break;

                //Staples Direct Mail
                case 93:
                    pnlDesigner.Visible = false;
                    pnlExperience.Visible = false;
                    pnlRecommend.Visible = false;

                    imgHeaderImage.ImageUrl = "/cmsimages/93/quote-header-logo.png";
                    imgHeaderImage.AlternateText = "Staples Every Door Direct Mail";
                    litCompanyNotResponsible.Text = "Staples";
                    litSiteNameDisclaimer.Text = "Staples Inc.";
                    litCompanyDisclaimer.Text = "Staples";
                    litCompanyDisclaimer2.Text = "Staples";
                    litCompanyDisclaimer3.Text = "Staples";
                    litCompanyDisclaimer4.Text = "Staples";
                    litCompanyDisclaimer5.Text = "Staples";
                    litCompanyDisclaimer6.Text = "Staples";
                    litCompanyDisclaimer7.Text = "Staples";
                    imgFooterLogo.ImageUrl = "/cmsimages/93/quote-footer-logo.png";
                    litFooterCompany.Text = "<address><small>Copyright 2015 Staples Inc. <br />All rights reserved.</small></address>";
                    litFooterTrademarks.Text = "<address><small>EVERY DOOR DIRECT MAIL&reg;, EDDM®, EDDM RETAIL&reg;, EDDM BMEU&reg;, UNITED STATES POSTAL SERVICE&reg;, U.S. POSTAL SERVICE&reg;, USPS&reg;, U.S.  POST OFFICE&reg;, POST OFFICE, and ZIP CODE&trade; are trademarks of the United States Postal Service&reg;and are used with permission under license.</small></address>";
                    break;


                //Staples Act Mgr List - TEMP SITE
                case 95:
                    pnlDesigner.Visible = false;
                    pnlExperience.Visible = false;
                    pnlRecommend.Visible = false;

                    imgHeaderImage.ImageUrl = "/cmsimages/93/quote-header-logo.png";
                    imgHeaderImage.AlternateText = "Staples Direct Mail";
                    litCompanyNotResponsible.Text = "Staples";
                    litSiteNameDisclaimer.Text = "Staples Inc.";
                    litCompanyDisclaimer.Text = "Staples";
                    litCompanyDisclaimer2.Text = "Staples";
                    litCompanyDisclaimer3.Text = "Staples";
                    litCompanyDisclaimer4.Text = "Staples";
                    litCompanyDisclaimer5.Text = "Staples";
                    litCompanyDisclaimer6.Text = "Staples";
                    litCompanyDisclaimer7.Text = "Staples";
                    imgFooterLogo.ImageUrl = "/cmsimages/93/quote-footer-logo.png";
                    litFooterCompany.Text = "<address><small>Copyright 2015 Staples Inc. <br />All rights reserved.</small></address>";
                    litFooterTrademarks.Text = "<address><small>EVERY DOOR DIRECT MAIL&reg;, EDDM®, EDDM RETAIL&reg;, EDDM BMEU&reg;, UNITED STATES POSTAL SERVICE&reg;, U.S. POSTAL SERVICE&reg;, USPS&reg;, U.S.  POST OFFICE&reg;, POST OFFICE, and ZIP CODE&trade; are trademarks of the United States Postal Service&reg;and are used with permission under license.</small></address>";
                    break;

                //Ramp Express
                case 98:

                    imgHeaderImage.ImageUrl = "/cmsimages/98/quote-header-logo.png";
                    imgHeaderImage.AlternateText = "RAMP Express Direct Mail";
                    litCompanyNotResponsible.Text = "RAMP Express Direct";
                    litSiteNameDisclaimer.Text = "RAMP Express Direct";
                    litCompanyDisclaimer.Text = "RAMP Express Direct";
                    litCompanyDisclaimer2.Text = "RAMP Express Direct";
                    litCompanyDisclaimer3.Text = "RAMP Express Direct";
                    litCompanyDisclaimer4.Text = "RAMP Express Direct";
                    litCompanyDisclaimer5.Text = "RAMP Express Direct";
                    litCompanyDisclaimer6.Text = "RAMP Express Direct";
                    litCompanyDisclaimer7.Text = "RAMP Express Direct";
                    imgFooterLogo.ImageUrl = "/cmsimages/98/quote-footer-logo.png";
                    litFooterCompany.Text = "<address><small>Copyright 2015 RAMP Express Direct Inc. <br />All rights reserved.</small></address>";
                    litFooterTrademarks.Text = "<address><small>EVERY DOOR DIRECT MAIL&reg;, EDDM®, EDDM RETAIL&reg;, EDDM BMEU&reg;, UNITED STATES POSTAL SERVICE&reg;, U.S. POSTAL SERVICE&reg;, USPS&reg;, U.S.  POST OFFICE&reg;, POST OFFICE, and ZIP CODE&trade; are trademarks of the United States Postal Service&reg;and are used with permission under license.</small></address>";
                    break;


                //Fallback
                default:
                    imgHeaderImage.ImageUrl = "/cmsimages/1/quote-header-logo.png";
                    imgHeaderImage.AlternateText = "EveryDoorDirectMail.com";
                    litCompanyNotResponsible.Text = "Taradel";
                    litSiteNameDisclaimer.Text = "Taradel";
                    litCompanyDisclaimer.Text = "Taradel LLC";
                    litCompanyDisclaimer2.Text = "Taradel LLC";
                    litCompanyDisclaimer3.Text = "Taradel LLC";
                    litCompanyDisclaimer4.Text = "Taradel";
                    litCompanyDisclaimer5.Text = "Taradel";
                    litCompanyDisclaimer6.Text = "Taradel";
                    litCompanyDisclaimer7.Text = "Taradel";
                    imgFooterLogo.ImageUrl = "/cmsimages/1/quote-footer-logo.png";
                    litFooterCompany.Text = "<address><small>Copyright 2015 Taradel, LLC. <br />All rights reserved.</small></address>";
                    litFooterTrademarks.Text = "<address><small>EVERY DOOR DIRECT MAIL&reg;, EDDM®, EDDM RETAIL&reg;, EDDM BMEU&reg;, UNITED STATES POSTAL SERVICE&reg;, U.S. POSTAL SERVICE&reg;, USPS&reg;, U.S.  POST OFFICE&reg;, POST OFFICE, and ZIP CODE&trade; are trademarks of the United States Postal Service&reg;and are used with permission under license.</small></address>";
                    break;

            }
        }

    }


    private void SetPageProperties()
    {

        //SiteID
        siteID = appxCMS.Util.CMSSettings.GetSiteId();

        //environment
        currentMode = appxCMS.Util.AppSettings.GetString("Environment").ToLower();

    }


    protected void SubmitApproval(object sender, EventArgs e)
    {

        if (Page.IsValid)
        {
            //set
            string siteName = "";
            string fullName = txtFullName.Text;
            string companyName = txtCompanyName.Text;
            string orderNumber = txtOrderNumber.Text;
            string designerName = txtDesigner.Text;
            string experience = ddlExperience.SelectedValue;
            string recommendation = ddlRecommend.SelectedValue;
            string approved = chkConfirm.Checked.ToString().ToUpper();
            string comments = txtComments.Text;
            string shareFeedback = radShareFeedback.SelectedValue.ToUpper();

            //Determine Site Name
            switch (siteID)
            {
                case 1:
                    siteName = "EDDM";
                    break;

                case 78:
                    siteName = "Staples";
                    designerName = "not provided";
                    experience = "not provided";
                    recommendation = "not provided";
                    break;

                case 91:
                    siteName = "Staples";
                    designerName = "not provided";
                    experience = "not provided";
                    recommendation = "not provided";
                    break;

                case 93:
                    siteName = "Staples";
                    designerName = "not provided";
                    experience = "not provided";
                    recommendation = "not provided";
                    break;

                case 95:
                    siteName = "Staples";
                    designerName = "not provided";
                    experience = "not provided";
                    recommendation = "not provided";
                    break;

                case 98:
                    siteName = "RAMP Express";
                    designerName = "not provided";
                    experience = "not provided";
                    recommendation = "not provided";
                    break;

                default:
                    siteName = "unknown";
                    break;

            }

            //scrub
            fullName = Server.HtmlEncode(fullName.Trim());
            companyName = Server.HtmlEncode(companyName.Trim());
            designerName = Server.HtmlEncode(designerName.Trim());
            comments = Server.HtmlEncode(comments.Trim());


            //site As String, fullName As String
            EmailUtility.SendArtApprovalEmail(siteName, fullName, companyName, orderNumber, designerName, experience, recommendation, approved, comments, shareFeedback);

            pnlApprovalForm.Visible = false;
            pnlSuccess.Visible = true;
            litSuccessMessage.Text = "Thank you!  Your artwork has now been approved and we will begin the printing process.";
        }

        else
        {
            pnlError.Visible = true;
            pnlApprovalForm.Visible = false;
            litErrorMessage.Text = "Sorry but this form submission is not valid.";
        }


    }



}