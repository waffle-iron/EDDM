<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EDDMHomeCover.ascx.cs" Inherits="Controls_EDDMHomeCover" %>
<%@ Register Src="~/Controls/ThreeEasySteps.ascx" TagPrefix="appx" TagName="ThreeEasySteps" %>
<%@ Register Src="~/Controls/Testimonials.ascx" TagPrefix="appx" TagName="Testimonials" %>
<%@ Register Src="~/Controls/EDDMCredentials.ascx" TagPrefix="appx" TagName="EDDMCredentials" %>
<%@ Register Src="~/Controls/HomePageVideo.ascx" TagPrefix="appx" TagName="HomePageVideo" %>



<section id="LandingRowBlock">

    <%--Visible in small, med, large devices.--%>
    <div class="lrgLandingWrapper hidden-xs" id="coverImgBlock">
        <div class="container-fluid">

            <div class="row">

                <div class="col-md-8 col-md-offset-2">
            
                    <div class="ctaLandingWrapper">

                        <h1>Grow your business with<br />Every door direct mail<small class="text-primary">&reg;</small></h1>
            
                        <p class="lead text-center">With EDDM<small>&reg;</small>, you can easily reach the customers who matter most by 
                        targeting specific local neighborhoods or ZIP codes.  No mailing permits, mailing lists, or 
                        experience is required.</p>
                       

                        

                            <div class="row">

                                <div class="col-md-5 col-md-offset-1 text-center">
                                    
                                    <!--HubSpot Call-to-Action Code -->
                                    <span class="hs-cta-wrapper" id="hs-cta-wrapper-5de80cb5-e99c-4945-981d-bf3ed6b6b6f9">
                                        <span class="hs-cta-node hs-cta-5de80cb5-e99c-4945-981d-bf3ed6b6b6f9" id="hs-cta-5de80cb5-e99c-4945-981d-bf3ed6b6b6f9">
                                            <!--[if lte IE 8]><div id="hs-cta-ie-element"></div><![endif]-->
                                            <a href="http://cta-redirect.hubspot.com/cta/redirect/212947/5de80cb5-e99c-4945-981d-bf3ed6b6b6f9" ><img class="hs-cta-img" id="hs-cta-img-5de80cb5-e99c-4945-981d-bf3ed6b6b6f9" style="border-width:0px;" src="https://no-cache.hubspot.com/cta/default/212947/5de80cb5-e99c-4945-981d-bf3ed6b6b6f9.png"  alt="✔ Find Prospects"/></a>
                                        </span>
                                        <script charset="utf-8" src="https://js.hscta.net/cta/current.js"></script>
                                        <script type="text/javascript">
                                            hbspt.cta.load(212947, '5de80cb5-e99c-4945-981d-bf3ed6b6b6f9', {});
                                        </script>
                                    </span>
                                    <!-- end HubSpot Call-to-Action Code -->

                                </div>

                                <div class="col-md-5 text-center">

                                    <!--HubSpot Call-to-Action Code -->
                                    <span class="hs-cta-wrapper" id="hs-cta-wrapper-926e252a-1b89-457b-a427-59aa85a5e1c8">
                                        <span class="hs-cta-node hs-cta-926e252a-1b89-457b-a427-59aa85a5e1c8" id="hs-cta-926e252a-1b89-457b-a427-59aa85a5e1c8">
                                            <!--[if lte IE 8]><div id="hs-cta-ie-element"></div><![endif]-->
                                            <a href="http://cta-redirect.hubspot.com/cta/redirect/212947/926e252a-1b89-457b-a427-59aa85a5e1c8" ><img class="hs-cta-img" id="hs-cta-img-926e252a-1b89-457b-a427-59aa85a5e1c8" style="border-width:0px;" src="https://no-cache.hubspot.com/cta/default/212947/926e252a-1b89-457b-a427-59aa85a5e1c8.png"  alt="Get FREE Quote"/></a>
                                        </span>
                                        <script charset="utf-8" src="https://js.hscta.net/cta/current.js"></script>
                                        <script type="text/javascript">
                                            hbspt.cta.load(212947, '926e252a-1b89-457b-a427-59aa85a5e1c8', {});
                                        </script>
                                    </span>
                                    <!-- end HubSpot Call-to-Action Code -->
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

            <asp:HyperLink ID="hypCallUs" runat="server" CssClass="btn btn-lg btn-danger btn-block center-block lrgActionButton" NavigateUrl="tel:8004811656">
                <span class="glyphicon glyphicon-phone"></span>&nbsp;Call Us Now
            </asp:HyperLink>

        </div>

    </div>

    
    <appx:EDDMCredentials runat="server" id="EDDMCredentials" />

    <appx:HomePageVideo runat="server" id="HomePageVideo" />

    <appx:ThreeEasySteps runat="server" id="ThreeEasySteps" />

    <appx:Testimonials runat="server" id="Testimonials" />


</section>

