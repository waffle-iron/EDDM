using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CCustom_PageHeader : System.Web.UI.UserControl
{
    //Place this control between the top-most <div class="container"> and the <div class="contentWrapper">.  For example:
    //<div class="container">
    //   <clibrary contenteditable="false">[CCustom/PageHeader headerType=simple simpleHeader=Addressed_Direct_Mail_Overview]</clibrary>
    //      <div class="contentWrapper">
    //
    // OR 
    //
    //<div class="container">
    //   <eddm:PageHeader runat="server" id="PageHeader" />
    //      <div class="contentWrapper">


    //Pass this Control a string property of headerType in CMS or aspx page.  
    //headerType must be one of the following:
    // *simple
    // *partial
    // *full 

    //If you need to use a space in the header strings, pass a underscore "_".  It will be replaced with a space. 

    //If you do end us using the Simple Header style, you will probably want to change the CSS variables of the contentWrapper "@content-wrapper-margin-top".

    //Samples:
    // <clibrary contenteditable="false">[CCustom/PageHeader headerType=full fullHeader=Flat-rate_Pricing]</clibrary>
    // <clibrary contenteditable="false">[CCustom/PageHeader headerType=partial mainHeader=EDDM_Pricing subHeader=Flat-rate_Pricing]</clibrary>
    // <clibrary contenteditable="false">[CCustom/PageHeader headerType=simple simpleHeader=Flat-rate_Pricing]</clibrary>


    private string _headerType;
    public string headerType
    {

        get
        {
            if (String.IsNullOrEmpty(_headerType))
            { return "unknown"; }

            else
            { return _headerType; }
        }

        set
        { _headerType = value; }

    }


    private string _mainHeader;
    public string mainHeader
    {

        get
        {
            if (String.IsNullOrEmpty(_mainHeader))
            { return "unknown"; }

            else
            { return _mainHeader; }
        }

        set
        { _mainHeader = value; }

    }


    private string _subHeader;
    public string subHeader
    {

        get
        {
            if (String.IsNullOrEmpty(_subHeader))
            { return "unknown"; }

            else
            { return _subHeader; }
        }

        set
        { _subHeader = value; }

    }


    private string _fullHeader;
    public string fullHeader
    {

        get
        {
            if (String.IsNullOrEmpty(_fullHeader))
            { return "unknown"; }

            else
            { return _fullHeader; }
        }

        set
        { _fullHeader = value; } 

    }


    private string _simpleHeader;
    public string simpleHeader
    {

        get
        {
            if (String.IsNullOrEmpty(_simpleHeader))
            { return "unknown"; }

            else
            { return _simpleHeader; }
        }

        set
        { _simpleHeader = value; }

    }



    protected void Page_Load(object sender, EventArgs e)
    {

        switch (headerType.ToLower())
        {
         
            case "simple":
                pnlSimple.Visible = true;
                litSimpleHeader.Text = simpleHeader.Replace("_", " ");
                break;

            case "partial":
                pnlPartial.Visible = true;
                litMainHeader.Text = mainHeader.Replace("_"," ");
                litMainHeaderSmall.Text = mainHeader.Replace("_", " ");
                litSubHeader.Text = subHeader.Replace("_"," ");
                litSubHeaderSmall.Text = subHeader.Replace("_", " ");
                break;

            case "full":
                pnlFullPage.Visible = true;
                litFullHeader.Text = fullHeader.Replace("_"," ");
                litFullHeaderSmall.Text = fullHeader.Replace("_", " ");
                break;

            default:
                pnlSimple.Visible = true;
                break;
        }

    }


}