using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DropDate : System.Web.UI.UserControl
{

    //=============================================================================================================
    //  NOTES:
    //      The containing page can determine what drop date calculates to be by setting ONE of these control 
    //      properties to true like so:
    //      ddlDropDate.IsProfessionalJob = false;  
    //=============================================================================================================


    //default values are false unless otherwise set by containing page
    public bool IsTemplateJob { get; set; }
    public bool IsProfessionalJob { get; set; }
    
    //meet the new paramter
    public bool IsMailingListJob { get; set; }



    public void Refresh()
    {
        
        ddlDropDate.Items.Clear();
        
        //Starting point.  Now.
        DateTime dropDate = DateTime.Today;

        //business logic - Thursday midnight is the cut off
        //ex: Friday, the 27th is an option until midnight on Thursday, the 19th
        //which is 8 days difference
        //start with the Friday that is at least 8 days away from today
        //  Today	    Fri after	Drops	  Week Displayed
        //  2/16/2015	2/20/2015	2/27/2015	3/2 - 3/6
        //  2/20/2015	2/27/2015	3/6/2015	3/9 - 3/13

        int AddDaysToGoOut = 7;

        if ((IsTemplateJob) && (!IsProfessionalJob))
        { AddDaysToGoOut = 14; }

        else if ((IsProfessionalJob) && (!IsTemplateJob))
        { AddDaysToGoOut = 21; }    


        dropDate = DateTime.Now.AddDays(AddDaysToGoOut);  

        if (dropDate.DayOfWeek == DayOfWeek.Friday)
        { dropDate = dropDate.AddDays(1); }

        while (dropDate.DayOfWeek != DayOfWeek.Friday)
        {
            dropDate = dropDate.AddDays(1);
        }



        DateTime fridayAfterDropDate = dropDate.AddDays(7);
        DateTime mondayAfterDropDate = dropDate.AddDays(3);
        DateTime loopDate = mondayAfterDropDate;

        int counter = 0;

        //loop 12x for 12 weeks
        do
        {
            ListItem weekItem = new ListItem();
            weekItem.Value = loopDate.ToShortDateString();
            if (IsMailingListJob)
            {
                weekItem.Text = loopDate.AddDays(-3).ToLongDateString();  //Monday --> Friday
            }
            else
            {
                weekItem.Text = loopDate.ToLongDateString() + " - " + loopDate.AddDays(4).ToLongDateString();
            }
            ddlDropDate.Items.Insert(counter, weekItem);
            loopDate = loopDate.AddDays(7);
            counter++;
        }

        while (counter < 6);//was 12

        ddlDropDate.DataBind();

        ddlDropDate.Items.Insert(0, "--Please Select--");

    }


    public void Page_Load(object sender, EventArgs e)
    {
        ddlDropDate.Attributes.Add("onchange", "LaunchWeekChanged(this.id); ValidateLaunchWeek();");
        Refresh(); 
    }


}