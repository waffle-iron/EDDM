<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenericFooter.ascx.cs" Inherits="CCustom_GenericFooter" %>
<%@ Register Src="~/CLibrary/SitePhoneNumber.ascx" TagPrefix="uc1" TagName="SitePhoneNumber" %>
<%@ Register Src="~/CCustom/BoldChatTextLink.ascx" TagPrefix="uc1" TagName="BoldChatTextLink" %>
<%@ Register Src="~/CLibrary/SiteEmailAddress.ascx" TagPrefix="uc1" TagName="SiteEmailAddress" %>



<!-- Build the Basic Footer-->



<footer>
    <div class="container">
        <div class="row">
            <div class="col-sm-4">
                <h3>About the Site</h3>
                <ul class="list-unstyled">
                    <li><a href="/" target="_self" title="Home">Home</a></li>
                    <li><a href="/Step1-Target.aspx" Target" target="_self" title="Target">Target</a></li>
                    <li><a href="/Design" target="_self" title="Design">Design</a></li>
                    <li><a href="/Print" target="_self" title="Print">Print</a></li>
                    <li><a href="/Mail" target="_self" title="Mail">Mail</a></li>
                    <li><a href="/terms" target="_self" title="Mail">Terms and Conditions</a></li>
                    <li><a href="/privacy-policy" target="_self" title="Mail">Privacy</a></li>
                </ul>
            </div>
            

            <div class="col-sm-4">
                <h3>Customer Service</h3>
                <ul class="list-unstyled">
                    <li><a href="/myaccount" target="_self" title="My Account"><span class="fa fa-user"></span>&nbsp;My Account</a></li>
                    <li><span class="fa fa-phone"></span>&nbsp;Call <uc1:SitePhoneNumber runat="server" id="SitePhoneNumber" /></li>
                    <li><uc1:SiteEmailAddress runat="server" id="SiteEmailAddress" /></li>
                   <!-- Check to hide--> <li><a href="/Support" title="Support"><span class="fa fa-support"></span>&nbsp;Support</a></li>
                   <!-- Check to hide--><li><a target="_self" href="#"><uc1:BoldChatTextLink runat="server" id="BoldChatTextLink" /></a></li>
                </ul>
               
                
            </div>
            <div class="col-sm-4">
                <h3>Payment Options</h3>

                <p>
                    <span class="fa fa-cc-mastercard fa-3x"></span>
                    <span class="fa fa-cc-visa fa-3x"></span>
                    <span class="fa fa-cc-discover fa-3x"></span>
                    <span class="fa fa-cc-amex fa-3x"></span>
                </p>
                <!-- Check to hide--><p><img id="footerlogo" alt="" src="/assets/images/footer-logo.png" class="img-responsive" /></p>
                <address>
                    <small>
                        Copyright © 2016 | Powered by Taradel, LLC.  <br />
                        All rights reserved.
                    </small>
                </address>

            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="col-sm-12">
                <address><small>EVERY DOOR DIRECT MAIL®, EDDM®, EDDM RETAIL®, EDDM BMEU®, UNITED STATES POSTAL SERVICE®,  U.S. POSTAL SERVICE®, USPS®, U.S. POST OFFICE®, POST OFFICE®, and ZIP CODE™ are trademarks of the United States Postal Service®  and are used with permission under license.</small></address>
            </div>
        </div>
    </div>
</footer>