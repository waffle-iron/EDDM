<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaplesStoreHomeCover.ascx.cs" Inherits="StaplesStoreHomeCover" %>

<section id="landingSection">
  <%--Visible in small, med, large devices.--%>
  <div class="lrgLandingWrapper hidden-xs" id="coverImgBlock">
    <div class="transparentBarWrapper">
      <div class="container">
        <div class="row">
          <div class="col-sm-8 col-sm-offset-2">
            <div id="ctaContent">
              <h1>Grow your business with<br />
                Every Door Direct Mail<sup><small>&reg;</small></sup></h1>
              <p class="lead text-center">Build an effective direct mail campaign in 3 easy steps.</p>
              <div class="row">
                <div class="col-md-10 text-right">
                  <asp:Hyperlink ID="hypGetStarted" runat="server" NavigateUrl="~/Step1-Target.aspx" CssClass="btn btn-danger btn-lg lrgActionButton" ToolTip="Get Started"> <span class="fa fa-check"></span>&nbsp;FIND CUSTOMERS NOW </asp:Hyperlink>
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
      <p>Build your direct mail campaign online.<br />
        Let our expert team do the rest.</p>
      <asp:HyperLink ID="hypCallUs" runat="server" CssClass="btn btn-lg btn-danger btn-block center-block lrgActionButton" NavigateUrl="tel:8884011463"> <span class="glyphicon glyphicon-phone"></span>&nbsp;Call Us Now </asp:HyperLink>
    </div>
  </div>
</section>
