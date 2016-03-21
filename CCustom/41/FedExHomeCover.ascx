<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FedExHomeCover.ascx.cs" Inherits="CCustom_41_FedExHomeCover" %>
<%@ Register Src="~/Controls/ThreeEasySteps.ascx" TagPrefix="appx" TagName="ThreeEasySteps" %>
<%@ Register Src="~/Controls/Testimonials.ascx" TagPrefix="appx" TagName="Testimonials" %>
<%@ Register Src="~/Controls/EDDMCredentials.ascx" TagPrefix="appx" TagName="EDDMCredentials" %>



<section id="LandingRowBlock">

    <%--Visible in small, med, large devices.--%>
    <div class="lrgLandingWrapper hidden-xs" id="coverImgBlock">
        <div class="container-fluid">

            <div class="row">

                <div class="col-md-8 col-md-offset-2">
            
                    <div class="ctaLandingWrapper" id="ctaContent">

                        <h1>Reach Thousands of Prospects<br /> with EDDM<small>&reg;</small> from the USPS<small class="text-primary">&reg;</small></h1>
            
                        <p class="lead text-center">With just a few clicks, you can launch a direct mail campaign for your business in 10 
                        minutes or less. Even better, we handle all of the printing, paperwork, postage, and delivery for you. Get started today!</p>
                       
                        <div class="row">

                            <div class="col-md-5 col-md-offset-1 text-center">
                                <asp:Hyperlink ID="hypQuote" runat="server" NavigateUrl="~/EDDM-Quote-Request" CssClass="btn btn-primary btn-lg lrgActionButton">
                                    <span class="fa fa-clipboard"></span>&nbsp;FREE Price Quote
                                </asp:Hyperlink>
                            </div>

                            <div class="col-md-5 text-center">
                                <asp:Hyperlink ID="hypGetStarted" runat="server" NavigateUrl="~/Step1-Target.aspx" CssClass="btn btn-cta btn-lg lrgActionButton" ToolTip="Get Started">
                                    <span class="fa fa-check"></span>&nbsp;Find Customers Now
                                </asp:Hyperlink>
                            </div>


                        </div>

                    </div>

                </div>

            </div>

        </div>

    </div>


    <%--Visible in xs devices only.--%>
    <div class="row visible-xs mobileSplashWrapper">

        <div class="col-sm-12 visible-xs" id="mobileSplash">

            <h1 class="text-center">Get Started</h1>

            <p class="lead">Let us help you grow your business.  Give us a call and we will show you how easy it really is.</p>

            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-lg btn-cta btn-block center-block lrgActionButton" NavigateUrl="tel:8889008101">
                <span class="glyphicon glyphicon-phone"></span>&nbsp;Call Us Now
            </asp:HyperLink>

        </div>

    </div>

    
    <appx:EDDMCredentials runat="server" id="EDDMCredentials1" />

    <div class="videoWrapper">
        <div class="container-fluid">
            <div class="hidden-xs" id="videoBlock">

                <h1 class="text-uppercase text-center">EVERY DOOR DIRECT MAIL<small>&reg;</small>DRIVES RESULTS</h1>

                <div class="col-sm-6 col-sm-offset-3">
                    <div class="embed-responsive embed-responsive-16by9">
                        <iframe src="//fast.wistia.net/embed/iframe/yjq2ie9i73?videoFoam=true" allowtransparency="true" frameborder="0" scrolling="no" class="wistia_embed" name="wistia_embed" allowfullscreen mozallowfullscreen webkitallowfullscreen oallowfullscreen msallowfullscreen></iframe>
                        <script src="//fast.wistia.net/assets/external/E-v1.js"></script>
                    </div>

                <br />

                <p class="text-center text-info"><strong>It's Easy! No mailing permit, mailing list, or paperwork is required.</strong></p>

                </div>

                <p>&nbsp;</p>



            </div>

        </div>
    
    </div>

    <appx:ThreeEasySteps runat="server" id="ThreeEasySteps" />

    <appx:Testimonials runat="server" id="Testimonials" />


</section>


