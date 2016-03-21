<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaradelDMCarousel.ascx.cs" Inherits="TaradelCarousel" %>

<div id="carouselWrapper" class="hidden-xs">

    <div id="imgCarousel" class="carousel slide" data-interval="5000" data-ride="carousel">

        <%--Controls--%>
        <ol class="carousel-indicators">
            <li data-target="#imgCarousel" data-slide-to="0" class="active"></li>
            <li data-target="#imgCarousel" data-slide-to="1"></li>
            <li data-target="#imgCarousel" data-slide-to="2"></li>
        </ol>

        <%--Slides--%>
        <div class="carousel-inner" role="listbox">

            <%--Slide 1--%>
            <div class="item active">

                <div class="lrgLandingWrapper" id="slider1">

                    <div class="transparentBarWrapper">
            
                        <div class="container">

                            <div class="row">

                                <div class="col-sm-10 col-sm-offset-1">
            
                                    <div id="ctaContent">

                                        <h1>Direct Mail Services from Taradel</h1>
            
                                        <p class="lead text-center">Build your direct mail campaign online. Let our expert team do the rest.</p>
                       
                                        <div class="row">

                                            <div class="col-md-6 text-center">
                                                <asp:Hyperlink ID="hypAddressed" runat="server" NavigateUrl="~/Addressed-Overview" CssClass="btn btn-primary btn-lg btn-shadow lrgActionButton">
                                                    ADDRESSED DIRECT MAIL<br /><small>The best way to reach a specific audience</small>
                                                </asp:Hyperlink>
                                            </div>

                                            <div class="col-md-6 text-center">
                                                <asp:Hyperlink ID="hypEDDM" runat="server" NavigateUrl="~/EDDM-Overview" CssClass="btn btn-info btn-lg btn-shadow lrgActionButton">
                                                    EVERY DOOR DIRECT MAIL&reg;<br /><small>The best way to reach every local mailbox</small>
                                                </asp:Hyperlink>
                                            </div>

                                        </div>

                                    </div>

                                </div>

                            </div>

                        </div>


                    </div>

                </div>

            </div>

            <%--Slide 2--%>
            <div class="item">

                <div class="lrgLandingWrapper" id="slider2">

                    <div class="transparentBarWrapper">
            
                        <div class="container">

                            <div class="row">

                                <div class="col-sm-10 col-sm-offset-1">
            
                                    <div id="ctaContent2">

                                        <h1>Every Door Direct Mail<small>&reg;</small></h1>
            
                                        <p class="lead text-center">Reach every mailbox in 3 easy steps.</p>
                       
                                        <div class="row">

                                            <div class="col-md-5 col-md-offset-1 text-center">
                                                <asp:Hyperlink ID="hypQuote" runat="server" NavigateUrl="~/EDDM-Quote-Request" CssClass="btn btn-primary btn-block btn-lg btn-shadow lrgActionButton">
                                                    <span class="fa fa-clipboard"></span>&nbsp;FREE Price Quote
                                                </asp:Hyperlink>
                                            </div>

                                            <div class="col-md-5 text-center">
                                                <asp:Hyperlink ID="hypGetStarted" runat="server" NavigateUrl="~/Step1-Target.aspx" CssClass="btn btn-info btn-block btn-lg btn-shadow lrgActionButton" ToolTip="Get Started">
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

            </div>

            <%--Slide 3--%>
            <div class="item">

                <div class="lrgLandingWrapper" id="slider3">

                    <div class="container">

                        <div class="row">

                            <div class="col-md-9 col-sm-12">
            
                                <div class="ctaLandingWrapper" id="ctaContent3">

                                    <h1>Targeted Mailing Lists</h1>
            
                                    <p class="lead text-center">Purchase a mailing list or upload your own.<br />
                                    Try our easy direct mail campaign builder.</p>
                       
                                    <p>&nbsp;</p>

                                    <div class="row">

                                        <div class="col-md-5 col-md-offset-1 col-sm-6 text-center">
                                            <asp:Hyperlink ID="hypUpload" runat="server" NavigateUrl="~/Addressed/Step1-UploadYourList.aspx" CssClass="btn btn-shadow btn-primary btn-lg">
                                                <span class="fa fa-upload"></span>&nbsp;UPLOAD YOUR LIST
                                            </asp:Hyperlink>
                                        </div>

                                        <div class="col-md-5 col-sm-6 text-center">
                                            <asp:Hyperlink ID="hypGenerate" runat="server" NavigateUrl="~/Addressed/Step1-BuildYourList.aspx" CssClass="btn btn-shadow btn-info btn-lg" ToolTip="Get Started">
                                                <span class="fa fa-cog"></span>&nbsp;BUILD NEW LIST
                                            </asp:Hyperlink>
                                        </div>


                                    </div>

                                </div>

                            </div>

                        </div>

                    </div>


                    

                </div>

            </div>

        </div>


        <%--Prev Next Controls--%>
        <a class="left carousel-control" href="#imgCarousel" role="button" data-slide="prev">
            <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
            <span class="sr-only">Previous</span>
        </a>

        <a class="right carousel-control" href="#imgCarousel" role="button" data-slide="next">
            <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
            <span class="sr-only">Next</span>
        </a>

    </div>

</div>



<%--Visible in xs devices only.--%>
<div class="row visible-xs mobileSplashWrapper">

    <div class="col-sm-12 visible-xs" id="mobileSplash">

        <h1 class="text-center">Introducing direct mail from Taradel<small><span style="color:#ffffff;">&reg;</span></small></h1>

        <p class="text-center">Build your direct mail campaign online.</p>
        
        <p class="text-center">Let our expert team do the rest.</p>

        <asp:HyperLink ID="hypCallUs" runat="server" CssClass="btn btn-lg btn-danger btn-block center-block lrgActionButton" NavigateUrl="tel:8004811656">
            <span class="fa fa-mobile-phone"></span>&nbsp;Call Us Now
        </asp:HyperLink>

    </div>

</div>





<script type="text/javascript">

    //override single background image with unique imgs
    $("#slider1").attr("style", "background-image: url('/cmsimages/100/slide1.jpg');");
    $("#slider2").attr("style", "background-image: url('/cmsimages/100/slide2.jpg');");
    $("#slider3").attr("style", "background-image: url('/cmsimages/100/slide4.jpg');");

</script>

