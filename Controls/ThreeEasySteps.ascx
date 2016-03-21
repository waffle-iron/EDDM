<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ThreeEasySteps.ascx.cs" Inherits="CLibrary_ThreeEasySteps" %>

<div class="easyStepsWrapper" id="easyStepsWrapper">
    <div class="container-fluid">

        <div class="easyStepsH1Wrapper">
            <h1 class="text-uppercase text-center">Launch your EDDM<small>&reg;</small> Campaign in 3 Easy Steps</h1>
        </div>

        <div class="row">

            <div class="col-sm-4">

                <div class="text-center iconspin">
                    <span class="fa fa-3x fa-bullseye fa-spin"></span>
                </div>

                <h5 class="text-center">TARGET</h5>

                <p class="text-center">Search by ZIP Code&trade; or postal carrier route with our &quot;point and click&quot; 
                mapping tool. Get instant mailbox counts, apply demographic filters, and more.</p>

            </div>

            <div class="col-sm-4">

                <div class="text-center iconspin">
                    <span class="fa fa-3x fa-paint-brush text-primary"></span>
                </div>

                <h5 class="text-center">DESIGN</h5>

                <p class="text-center">Choose from multiple postcard and flyer size 
                options, then browse over one thousand FREE design templates. You can also upload your 
                own artwork or get design help.</p>

            </div>

            <div class="col-sm-4">

                <div class="text-center iconspin">
                    <span class="fa fa-3x fa fa-rocket text-primary"></span>
                </div>

                <h5 class="text-center">LAUNCH</h5>

                <p class="text-center">All orders include printing, mail preparation, 
                postage, and delivery. No paperwork or Post Office&reg; drop-offs required. Place your order online, and you're done.</p>

            </div>

        </div>


        <%--All Dev Buttons--%>
        <asp:Panel ID="pnlDevButtons" runat="server" Visible="False">

            <asp:Panel runat="server" ID="pnlEddmDevButtons" Visible="False">

                <div class="row extraTopPadding">

                    <div class="col-sm-4 col-sm-offset-2 text-center">
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

                    <div class="col-sm-4 text-center">
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

            </asp:Panel>

            <asp:Panel runat="server" ID="pnlFedExDevButtons" Visible="False">

                <div class="row">

                    <div class="col-sm-4 col-sm-offset-2 text-center">
                        <asp:Hyperlink ID="hypQuote" runat="server" NavigateUrl="~/EDDM-Quote-Request" CssClass="btn btn-default btn-lg lrgActionButton">
                            <span class="fa fa-clipboard"></span>&nbsp;FREE Price Quote
                        </asp:Hyperlink>
                    </div>

                    <div class="col-sm-4 text-center">
                        <asp:Hyperlink ID="hypGetStarted" runat="server" NavigateUrl="~/Step1-Target.aspx" CssClass="btn btn-cta btn-lg lrgActionButton" ToolTip="Get Started">
                            <span class="fa fa-check"></span>&nbsp;Find Customers Now
                        </asp:Hyperlink>
                    </div>

                </div>

            </asp:Panel>

        </asp:Panel>

        <%--Production Buttons--%>
        <asp:Panel ID="pnlProdButtons" runat="server" Visible="False">

            <asp:Panel runat="server" id="pnlEddmProdButtons" Visible="False">

                <div class="row extraTopPadding">

                    <div class="col-sm-4 col-sm-offset-2 text-center">

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

                    <div class="col-sm-4 text-center">

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

            </asp:Panel>

            <asp:Panel runat="server" id="pnlFedExProdButtons" Visible="False">

                <div class="row">

                    <div class="col-sm-4 col-sm-offset-2 text-center">
                        <asp:Hyperlink ID="hypFedExQuoteProd" runat="server" NavigateUrl="~/EDDM-Quote-Request" CssClass="btn btn-default btn-lg lrgActionButton">
                            <span class="fa fa-clipboard"></span>&nbsp;FREE Price Quote
                        </asp:Hyperlink>
                    </div>

                    <div class="col-sm-4 text-center">
                        <asp:Hyperlink ID="hypFedExStartProd" runat="server" NavigateUrl="~/Step1-Target.aspx" CssClass="btn btn-cta btn-lg lrgActionButton" ToolTip="Get Started">
                            <span class="fa fa-check"></span>&nbsp;Find Customers Now
                        </asp:Hyperlink>
                    </div>

                </div>

            </asp:Panel>

        </asp:Panel>

        <div>&nbsp;</div>

    </div>

</div>


<script type="text/javascript">

    $('.iconspin').hover(function ()
    {
        $(this).children("span").toggleClass("fa-spin");
        return false;
    });

</script>


