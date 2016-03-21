using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_USAStatesDropDown : System.Web.UI.UserControl
{
    
    //NOTES:
    // If you set the property 'showAll50States to TRUE, it will offer this as a list item selection.

    //Properties
    public string selectedValue
    {
        get { return ddlStates.SelectedValue; }
        set { ddlStates.SelectedValue = value; }
    }


    private bool _enabledState;
    public bool enabledState
    {

        get
        { return _enabledState; }

        set
        {
            value = _enabledState;

            if (!value)
            { ddlStates.Enabled = false; }

        }

    }


    private string _cssClass;
    public string cssClass
    {

        get
        { return _cssClass; }

        set
        {
            value = _cssClass;

            if (value != null)
            { ddlStates.CssClass = value; }

        }

    }


    public bool showAll50States 
    { get; set; }


    public bool showPleaseSelect 
    { get; set; }





    //Methods
    protected void Page_Load(object sender, EventArgs e)
    {

        //To show or not to show all 50 States selection
        if (!showAll50States) 
        {
            //Remove the USA selection
            ListItem all50States = new ListItem();
            all50States = ddlStates.Items.FindByValue("USA");
            ddlStates.Items.Remove(all50States);

        }


        //To show or not to show all 50 States selection
        if (!showPleaseSelect)
        {
            //Remove the USA selection
            ListItem pleaseSelect = new ListItem();
            pleaseSelect = ddlStates.Items.FindByValue("**");
            ddlStates.Items.Remove(pleaseSelect);

        }
    
    }



}