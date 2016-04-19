<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenericProDesignPage.ascx.cs" Inherits="GenericProDesignPage" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="uc1" TagName="PageHeader" %>


<div class="container">
       <uc1:PageHeader runat="server" id="PageHeader" />
    <div class="contentWrapper">
        <div class="row">
            <div class="col-sm-12">
                <h2 class="text-center"><strong>Order Professional Design Service | 100% Satisfaction Guaranteed.</strong></h2>
                <h5>About &quot;Professional Design Service&quot; Option:</h5>
                <p>
                    If you have print-ready artwork in  high-resolution (.PDF) format, simply upload your files directly to your order.
                    This option is perfect for those who have design experience, or, prefer to use a third party agency to handle the creative process.
                </p>
                <h5>How To Choose &quot;My Design&quot; Option:</h5>
                <ul>
                    <li>Complete the Targeting Options (Steps 1 &amp; 2) to proceed to <strong>Choose Product (Step 3)</strong></li>
                    <li>Under the Choose Your Options menu, select <strong>Professional Design Service</strong></li>
                    <li>Note that a one-time affordable design fee applies to your order</li>
                </ul>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3"></div>
            <div class="col-sm-6">
                <div class="panel panel-primary" id="designOptionsBlock">
                    <div class="panel-heading">Design Options</div>
                    <div class="panel-body">
                        <p class="text-center"><img alt="" src="/assets/images/generic-design-options.png" class="img-responsive img-thumbnail" /></p>
                        <div>&nbsp;</div>
                        
                            <p class="text-right">
                                <div class="btn btn-cta btn-lg pull-right btn-shadow" href="#"> Continue to Delivery Options&nbsp;<span class="glyphicon glyphicon-chevron-right"></span> </div>
                            </p>
                    </div>
                </div>
            </div>
            <div class="col-sm-3"></div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <p class="text-center">
                    <small>(screen shot of <strong>Professional Design Service</strong> artwork menu)</small>
                </p>
                <p>&nbsp;</p>
                <p class="text-center"> <a href="/Design" class="btn btn-danger btn-lg btn-shadow"> <span class="fa fa-check"></span>&nbsp;GET STARTED </a> </p>
                <p>&nbsp;</p>
            </div>
        </div>
    </div>
</div>
