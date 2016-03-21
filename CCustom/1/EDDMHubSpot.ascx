<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EDDMHubSpot.ascx.cs" Inherits="EDDMHubSpot" %>


<asp:PlaceHolder ID="phHubspotScript" runat="server" Visible="False">

    <!-- HUBSPOT injection -->
    <script type="text/javascript">
    
        (function(d,s,i,r) 
        {
            if (d.getElementById(i))
            {return;}
    
            var n=d.createElement(s),e=d.getElementsByTagName(s)[0];

                n.id=i;n.src='//js.hubspot.com/analytics/'+(Math.ceil(new Date()/r)*r)+'/212947.js'
                e.parentNode.insertBefore(n, e);
        })

        (document, "script", "hs-analytics", 300000);

        (function (d, s, i, r)
        {
            if (d.getElementById(i)){return;}
            var n=d.createElement(s),e=d.getElementsByTagName(s)[0];
                n.id=i;n.src='//fast.wistia.com/static/integrations-hubspot-v1.js'
            e.parentNode.insertBefore(n, e);
        })

        (document, "script", "wistia-analytics", 300000);
            
    </script>
    <!-- END HUBSPOT injection -->
        
</asp:PlaceHolder>
