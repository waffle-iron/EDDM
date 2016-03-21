<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaplesActMgrListHomeCover.ascx.cs" Inherits="StaplesHomeCover" %>
<%@ Register Src="~/CCustom/95/StaplesActMgrListCarousel.ascx" TagPrefix="appx" TagName="StaplesCarousel" %>
<%@ Register Src="~/CCustom/95/StaplesActMgrListEasySteps.ascx" TagPrefix="appx" TagName="StaplesEasySteps" %>






<section id="homePageContent">

    <appx:StaplesCarousel runat="server" id="StaplesCarousel" />

    <appx:StaplesEasySteps runat="server" id="StaplesEasySteps" />

</section>



