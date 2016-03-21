<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuoteFooter.ascx.cs" Inherits="Controls_EDDMCredentials" %>


<footer>

    <div>&nbsp;</div>

    <asp:PlaceHolder ID="phFooterContent" runat="server" Visible="False">

        <div class="container"> 
           
            <div class="row">
                                    
                <div class="col-sm-5">                
                    <asp:Image ID="imgLogo" runat="server" AlternateText="" BorderWidth="0" CssClass="img-responsive" />
                    <address><small>Copyright 2015 Taradel, LLC. <br />
                    All rights reserved.</small></address>
                </div>
                      
                <div class="col-sm-7">                
                    <address><small>EVERY DOOR DIRECT MAIL®, EDDM®, EDDM RETAIL®, EDDM BMEU®, UNITED STATES POSTAL SERVICE®,  
                    U.S. POSTAL SERVICE®, USPS®, U.S.  POST OFFICE®, POST OFFICE®, and ZIP CODE™ are trademarks of the United States Postal Service®  
                    and are used with permission under license.</small></address> 
                </div>


            </div>

        </div>

    </asp:PlaceHolder>

    <div>&nbsp;</div>

</footer>