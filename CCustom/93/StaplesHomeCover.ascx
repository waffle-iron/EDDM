<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaplesHomeCover.ascx.cs" Inherits="StaplesHomeCover" %>
<%@ Register Src="StaplesTestimonials.ascx" TagPrefix="appx" TagName="StaplesTestimonials" %>



<section id="landingSection">

    <%--Visible in small, med, large devices.--%>
    <div class="lrgLandingWrapper hidden-xs" id="coverImgBlock">

        <div class="transparentBarWrapper">
            
            <div class="container">

                <div class="row">

                    <div class="col-sm-8 col-sm-offset-2">
            
                        <div id="ctaContent">

                            <h1>Grow your business with<br />Every Door Direct Mail<small>&reg;</small></h1>
            
                            <p class="lead text-center">Build an effective direct mail campaign in 3 easy steps.</p>
                       
                            <div class="row">

                                <div class="col-md-5 col-md-offset-1 text-center">
                                    <asp:Hyperlink ID="hypQuote" runat="server" NavigateUrl="~/EDDM-Quote-Request" CssClass="btn btn-primary btn-lg lrgActionButton">
                                        <span class="fa fa-clipboard"></span>&nbsp;FREE Price Quote
                                    </asp:Hyperlink>
                                </div>

                                <div class="col-md-5 text-center">
                                    <asp:Hyperlink ID="hypGetStarted" runat="server" NavigateUrl="~/Step1-Target.aspx" CssClass="btn btn-danger btn-lg lrgActionButton" ToolTip="Get Started">
                                        <span class="fa fa-check"></span>&nbsp;Try It Now
                                    </asp:Hyperlink>
                                </div>


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

            <h1 class="text-center">Introducing direct mail from Staples&reg;</h1>

            <p>Build your direct mail campaign online.<br />Let our expert team do the rest.</p>

            <asp:HyperLink ID="hypCallUs" runat="server" CssClass="btn btn-lg btn-danger btn-block center-block lrgActionButton" NavigateUrl="tel:8884011463">
                <span class="glyphicon glyphicon-phone"></span>&nbsp;Call Us Now
            </asp:HyperLink>

        </div>

    </div>

    <%--Video--%>
    <div class="videoWrapper">
        <div class="container-fluid">
            <div class="hidden-xs" id="videoBlock">

                <div>
                    <h1 class="text-center">What is Every Door Direct Mail?</h1>
                </div>
                
                <div class="col-sm-6">
                    <div class="embed-responsive embed-responsive-16by9">
                        <iframe src="//fast.wistia.net/embed/iframe/wty9i1b5wq?videoFoam=true" allowtransparency="true" frameborder="0" scrolling="no" class="wistia_embed" name="wistia_embed" allowfullscreen mozallowfullscreen webkitallowfullscreen oallowfullscreen msallowfullscreen height="100%" width="100%"></iframe>
                        <script src="//fast.wistia.net/assets/external/E-v1.js"></script>
                    </div>
                </div>

                <div class="col-sm-6">

                    <p class="lead text-left"><strong>Online Campaign Builder Makes Direct Mail Easy</strong><br /> 
                    With EDDM&reg; from Staples&reg;, you can mail offers to prospective customers by Zip Code or postal carrier route (neighborhood). 
                    No mailing permit, mailing list, or experience is required. Place your order online and you’re done -- Staples handles the rest!</p>

                    <p class="lead text-left">Order in 3 Easy Steps</p>

                    <ol class="lead text-left">
                        <li>Target mail delivery areas</li>
                        <li>Choose design and print options</li>
                        <li>Schedule mail delivery dates and submit order</li>
                    </ol>

                    <p class="text-center"><a href="/Step1-Target.aspx" class="btn btn-danger btn-lg lrgActionButton" Title="Get Started">
                        <span class="fa fa-check"></span>&nbsp;Try It Now
                    </a></p>

                </div>

                <p>&nbsp;</p>



            </div>

        </div>
    
    </div>

    <appx:StaplesTestimonials runat="server" id="StaplesTestimonials" />


</section>

