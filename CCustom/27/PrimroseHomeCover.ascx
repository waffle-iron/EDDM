<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimroseHomeCover.ascx.cs" Inherits="PrimroseHomeCover" %>



<section id="LandingRowBlock">

    <%--Visible in small, med, large devices.--%>
    <div class="lrgLandingWrapper hidden-xs" id="coverImgBlock">

        <div class="transparentBarWrapper">
            
            <div class="container">

                <div class="row">

                    <div class="col-sm-8 col-sm-offset-2">
            
                        <div id="ctaContent">

                            <h1>Grow your business with<br />Every Door Direct Mail<sup>&reg;</sup></h1>
            
                            <p class="lead text-center">With EDDM<sup>&reg;</sup>, you can easily reach the customers who matter most by targeting specific local neighborhoods or ZIP codes. No mailing permits, mailing lists, or experience is required.</p>
                       
                            <div class="row">

                                <div class="col-md-6 col-md-offset-6 text-center">
                                    <asp:Hyperlink ID="hypGetStarted" runat="server" NavigateUrl="~/Step1-Target.aspx" CssClass="btn btn-cta btn-lg lrgActionButton" ToolTip="Get Started">
                                        <span class="fa fa-check"></span>&nbsp;Find New Customers
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

            <h1 class="text-center">GET STARTED</h1>

            <p>Let us help you grow your business.  Give us a call and we will show you how easy it really is.</p>

            <asp:HyperLink ID="hypPhone" runat="server" CssClass="btn btn-lg btn-danger btn-block center-block lrgActionButton" NavigateUrl="tel:8004811656">
                <span class="glyphicon glyphicon-phone"></span>&nbsp;Call Us Now
            </asp:HyperLink>

        </div>

    </div>


</section>









