<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenericMailPage.ascx.cs" Inherits="GenericMailPage" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="uc1" TagName="PageHeader" %>
<%@ Register Src="~/CCustom/Banner-TargetLocal.ascx" TagPrefix="uc1" TagName="BannerTargetLocal" %>




<div class="container">
    <uc1:PageHeader runat="server" id="PageHeader" />
    <div class="contentWrapper">
        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <span class="fa fa-fw fa-lightbulb-o fa-5x pull-left">&nbsp;</span>
                    <h3>A SIMPLIFIED DIRECT MAIL PROCESS</h3>
                    <p>Every Door Direct Mail<sup>&reg;</sup> has changed the way small businesses send direct mail. By targeting every address in a specific zip code or carrier route, you bypass having to purchase a mailing list as well as costs associated with ink jetting those addresses on the mailer.</p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <span class="fa fa-fw fa-envelope-o fa-5x pull-left">&nbsp;</span>
                    <h3>FROM DESKTOP TO MAILBOX</h3>
                    <p>We've made ordering Every Door Direct Mail<sup>&reg;</sup> as easy as possible.Easily point and click to target your advertising area.Upload your own print-ready design or let us create one for you. After confirming your details and placing your order, we start production on your campaign.</p>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <span class="fa fa-fw fa-building-o fa-5x pull-left">&nbsp;</span>
                    <h3>FOR ALL TYPES OF BUSINESSES</h3>
                    <p>Businesses of all types and sizes are seeing huge success with Every Door Direct Mail<sup>&reg;</sup>. Retail, restaurants, professional services, home improvement, medical, and even websites are generating substantial results by sending their offers via Every Door Direct Mail<sup>&reg;</sup>.</p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <span class="fa fa-fw fa-refresh fa-5x pull-left">&nbsp;</span>
                    <h3>A COMPLETE DIRECT MAIL SOLUTION</h3>
                    <p>Our team takes care of all the heavy lifting involved with the Every Door Direct Mail<sup>&reg;</sup> process for a true turnkey solution. From start to finish, we handle all the paperwork, design, and production of your mailers - and then arrange your drops and deliveries with the USPS.</p>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <uc1:BannerTargetLocal runat="server" id="BannerTargetLocal" />
            </div>
        </div>
    </div>
</div>