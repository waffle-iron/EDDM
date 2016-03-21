using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;



public partial class OrderSteps : System.Web.UI.UserControl
{

    //Can hold up to 5 possible steps
    //3 possibles 'states' (css classes):
    //  Visited
    //  Current
    //  None "" (no class) - means Step "To Do"

    //http://fontawesome.io/cheatsheet/
    //Possible Font Awesome Icons:
    //fa-map-marker - Map Pin Icon
    //fa-user - User/Person
    //fa-credit-card - Credit Card
    //fa-folder - Folder
    //fa-check - Check


    //USING THIS CONTROL:
    //This control will display a minimum of 3 steps and a maximum of 5 steps.  To use this control, you must pass it the Number of Steps and the 
    //properties of at least the first three steps.  If you need use steps 4 and/or 5, pass those properties as well from the container page.

    //OrderSteps.numberOfSteps = 3;
    //OrderSteps.step1Text = "Define Your Area";
    //OrderSteps.step1Url = "/Addressed/Step1-BuildYourList.aspx";
    //OrderSteps.step1State = "visited";
    //OrderSteps.step1Icon = "fa-map-marker";

    //OrderSteps.step2Text = "Define Your Customers";
    //OrderSteps.step2Url = "/Addressed/Step1-BuildYourList.aspx";
    //OrderSteps.step2State = "current";
    //OrderSteps.step2Icon = "fa-user";

    //OrderSteps.step3Text = "Choose Your Product";
    //OrderSteps.step3Url = "";
    //OrderSteps.step3State = "";
    //OrderSteps.step3Icon = "fa-folder";

    //OrderSteps.step4Text = "Define your Delivery";
    //OrderSteps.step4Url = "";
    //OrderSteps.step4State = "";
    //OrderSteps.step4Icon = "fa-envelope";

    //OrderSteps.step5Text = "Review and Check Out";
    //OrderSteps.step5Url = "";
    //OrderSteps.step5State = "";
    //OrderSteps.step5Icon = "fa-credit-card";



    public int _numberOfSteps;

    public int numberOfSteps
    {
        get
        {
            if (_numberOfSteps == null)
            { return 3; }

            else
            { return _numberOfSteps; }
        }

        set
        { _numberOfSteps = value; }
    }





    //Step 1
    public string step1Text 
    { get; set; }

    public string step1Url 
    { get; set; }

    public string step1State 
    { get; set; }

    public string step1Icon 
    { get; set; }



    //Step 2
    public string step2Text
    { get; set; }

    public string step2Url
    { get; set; }

    public string step2State
    { get; set; }

    public string step2Icon 
    { get; set; }



    //Step 3
    public string step3Text
    { get; set; }

    public string step3Url
    { get; set; }

    public string step3State
    { get; set; }

    public string step3Icon
    { get; set; }



    //Step 4
    public string step4Text
    { get; set; }

    public string step4Url
    { get; set; }

    public string step4State
    { get; set; }

    public string step4Icon
    { get; set; }



    //Step 5
    public string step5Text
    { get; set; }

    public string step5Url
    { get; set; }

    public string step5State
    { get; set; }

    public string step5Icon
    { get; set; }




    protected void Page_Load(object sender, EventArgs e)
    {

        HtmlGenericControl step1 = (HtmlGenericControl)FindControl("liStep1");
        HtmlGenericControl step2 = (HtmlGenericControl)FindControl("liStep2");
        HtmlGenericControl step3 = (HtmlGenericControl)FindControl("liStep3");
        HtmlGenericControl step4 = (HtmlGenericControl)FindControl("liStep4");
        HtmlGenericControl step5 = (HtmlGenericControl)FindControl("liStep5");


        //STEP 1
        hypStep1.Text = "<span class=" + Convert.ToChar(34) + "fa " + step1Icon + Convert.ToChar(34) + "></span>&nbsp;" + step1Text;
        lblStep1.Text = "<span class=" + Convert.ToChar(34) + "fa " + step1Icon + Convert.ToChar(34) + "></span>&nbsp;" + step1Text;
        hypStep1.NavigateUrl = step1Url;
        liStep1.Attributes.Add("class", step1State);

        if (String.IsNullOrEmpty(step1State))
        {
            hypStep1.Visible = false;
            lblStep1.Visible = true;
        }
        else
        {
            hypStep1.Visible = true;
            lblStep1.Visible = false;
        }


        //STEP 2
        hypStep2.Text = "<span class=" + Convert.ToChar(34) + "fa " + step2Icon + Convert.ToChar(34) + "></span>&nbsp;" + step2Text;
        lblStep2.Text = "<span class=" + Convert.ToChar(34) + "fa " + step2Icon + Convert.ToChar(34) + "></span>&nbsp;" + step2Text;
        hypStep2.NavigateUrl = step2Url;
        liStep2.Attributes.Add("class", step2State);

        if (String.IsNullOrEmpty(step2State))
        {
            hypStep2.Visible = false;
            lblStep2.Visible = true;
        }
        else
        {
            hypStep2.Visible = true;
            lblStep2.Visible = false;
        }


        //STEP 3
        hypStep3.Text = "<span class=" + Convert.ToChar(34) + "fa " + step3Icon + Convert.ToChar(34) + "></span>&nbsp;" + step3Text;
        lblStep3.Text = "<span class=" + Convert.ToChar(34) + "fa " + step3Icon + Convert.ToChar(34) + "></span>&nbsp;" + step3Text;
        hypStep3.NavigateUrl = step3Url;
        liStep3.Attributes.Add("class", step3State);


        if (String.IsNullOrEmpty(step3State))
        {
            hypStep3.Visible = false;
            lblStep3.Visible = true;
            liStep3.Attributes.Remove("class");
        }
        else
        {
            hypStep3.Visible = true;
            lblStep3.Visible = false;
        }


        if (numberOfSteps > 3)
        {
            hypStep4.Text = "<span class=" + Convert.ToChar(34) + "fa " + step4Icon + Convert.ToChar(34) + "></span>&nbsp;" + step4Text;
            lblStep4.Text = "<span class=" + Convert.ToChar(34) + "fa " + step4Icon + Convert.ToChar(34) + "></span>&nbsp;" + step4Text;
            hypStep4.NavigateUrl = step4Url;
            liStep4.Visible = true;
            liStep4.Attributes.Add("class", step4State);

            if (String.IsNullOrEmpty(step4State))
            {
                hypStep4.Visible = false;
                lblStep4.Visible = true;
                liStep4.Attributes.Remove("class");
            }
            else
            {
                hypStep4.Visible = true;
                lblStep4.Visible = false;
            }


            if (numberOfSteps > 4)
            {
                hypStep5.Text = "<span class=" + Convert.ToChar(34) + "fa " + step5Icon + Convert.ToChar(34) + "></span>&nbsp;" + step5Text;
                lblStep5.Text = "<span class=" + Convert.ToChar(34) + "fa " + step5Icon + Convert.ToChar(34) + "></span>&nbsp;" + step5Text;
                hypStep5.NavigateUrl = step5Url;
                liStep5.Visible = true;
                liStep5.Attributes.Add("class", step5State);

                if (String.IsNullOrEmpty(step5State))
                {
                    hypStep5.Visible = false;
                    lblStep5.Visible = true;
                    liStep5.Attributes.Remove("class");
                }
                else
                {
                    hypStep5.Visible = true;
                    lblStep5.Visible = false;
                }

            }

        }



    }



}