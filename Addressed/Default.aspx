<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Addressed_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">

    <div class="container">

        <div class="partialRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            <span class="subRibbonPop">Addressed</span>
            <span class="subRibbon">Addressed Mail Made Simple</span>
        </div>

        <div class="partialRibbonSmall visible-sm visible-xs">
            <span class="subRibbonPopSmall">Addressed</span>
            <span class="subRibbonSmall">Addressed Mail Made Simple</span>
        </div>

        <div class="contentWrapper">
            
            <h3 class="text-danger">Under Construction</h3>

            <p class="lead hidden"><span class="leadDropWord">Now</span> it's easier than ever to reach new customers.  You can upload your own distribution list or you can 
            demographically build your own customized list.  Please choose from one of the options below. </p>

            <p>&nbsp;</p>

            <div class="row equal hidden">

                <div class="col-sm-4">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            
                            <h3 class="text-center">I Have My Own List</h3>
                            
                            <p  class="text-center"><asp:HyperLink runat="server" ID="hplOwnList" NavigateUrl="MyList.aspx" CssClass="btn btn-danger btn-lg">
                            <span class="fa fa-align-justify"></span>&nbsp;Get Started With My List</asp:HyperLink>
                            </p>

                            <p>&nbsp;</p>

                        </div>
                    </div>
                </div>

                <div class="col-sm-4">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            
                            <h3 class="text-center">Build A New List</h3>
                            
                            <p class="text-center"><asp:HyperLink runat="server" ID="hplBuildList" NavigateUrl="BuildList.aspx" CssClass="btn btn-danger btn-lg">
                            <span class="fa fa-plus"></span>&nbsp;Get Started Building a List</asp:HyperLink></p>

                        </div>
                    </div>
                </div>

                <div class="col-sm-4">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            
                            <h3 class="text-center">Build A New List (Beta)</h3>
                            
                            <p class="text-center"><asp:HyperLink runat="server" ID="hypBuildList" NavigateUrl="BuildListBeta.aspx" CssClass="btn btn-danger btn-lg">
                            <span class="fa fa-plus"></span>&nbsp;Get Started Building a List</asp:HyperLink></p>

                        </div>
                    </div>
                </div>

            </div>

            <p>&nbsp;</p>

            <div class="row hidden">

                <div class="col-sm-4">
                    <img alt="STAPLES® direct mail" src="/cmsimages/93/staples-home-image.png" title="STAPLES® direct mail" class="img-responsive" />
                </div>

                <div class="col-sm-8">

                    <h3>Save big on Every Door Direct Mail®</h3>

                    <p>
                        No more paperwork, no more trips to the Post Office®. Every Door Direct Mail® makes it easy for STAPLES&reg; customers to design, print,
                        and mail powerful offers <span class="productTitle">as low as 25¢</span> per piece.
                    </p>

                    <p><strong>Why Every Door Direct Mail®?</strong></p>

                    <ul class="list-unstyled">
                        <li><span class="fa fa-check text-danger"></span>&nbsp;No permit, mailing list, or experience required</li>
                        <li><span class="fa fa-check text-danger"></span>&nbsp;Mail postcards or flyers at super-low prices</li>
                        <li><span class="fa fa-check text-danger"></span>&nbsp;Target local neighborhoods or entire ZIP Codes</li>
                        <li><span class="fa fa-check text-danger"></span>&nbsp;Drive awareness, promote products, and increase sales</li>
                        <li><span class="fa fa-check text-danger"></span>&nbsp;Get all-inclusive service with one simple online order</li>
                    </ul>

                    <p>&nbsp;</p>

                    <div class="row">
                        <div class="col-sm-12 text-right">
                            <a id="hypEddm" class="btn btn-danger btn-lg" title="Staples EDDM Solutions" href="Step1-Target.aspx">
                                <span class="fa fa-check"></span>&nbsp;START YOUR EDDM CAMPAIGN NOW
                            </a>
                        </div>
                    </div>

                </div>

            </div>

            <p>&nbsp;</p>

            <div class="row hidden">
                <div class="col-sm-4">
                    <img alt="STAPLES® direct mail" src="/cmsimages/93/staples-postcard.jpg" title="STAPLES® direct mail" class="img-responsive" />
                </div>
                <div class="col-sm-8">
                    <h3>Looking to do a list-based mailing campaign?</h3>

                    <p>Utilizie our extensive database and try our simple, new &quot;Customer List Builder&quot; tool!</p>

                    <p><strong>Why use Customer List Builder?</strong></p>

                    <ul class="list-unstyled">
                        <li><span class="fa fa-check text-success"></span>&nbsp;Target specific customer types</li>
                        <li><span class="fa fa-check text-success"></span>&nbsp;Not restgricted to entire mailing routes</li>
                        <li><span class="fa fa-check text-success"></span>&nbsp;Potentially greater returns</li>
                        <li><span class="fa fa-check text-success"></span>&nbsp;Get all-inclusive service with one simple online order</li>
                    </ul>

                    <p class="text-right">
                        <a id="hypAddressed" class="btn btn-success btn-lg" title="Staples Addressed List Solutions" href="/Addressed/Step1-BuildYourList.aspx">
                            <span class="badge">New</span>&nbsp;USE CUSTOMER LIST BUILDER NOW
                        </a>
                    </p>
                </div>
            </div>


        </div>

    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>


<asp:Content ID="Content7" ContentPlaceHolderID="cpScripts" runat="Server">
</asp:Content>

