<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaradelDMHomeCover.ascx.cs" Inherits="TaradelDMHomeCover" %>
<%@ Register Src="~/CCustom/100/TaradelDMCarousel.ascx" TagPrefix="appx" TagName="TaradelDMCarousel" %>
<%@ Register Src="~/CCustom/100/TaradelDMListEasySteps.ascx" TagPrefix="appx" TagName="TaradelDMListEasySteps" %>




<section id="homePageContent">

    <appx:TaradelDMCarousel runat="server" id="TaradelDMCarousel" />

    <appx:TaradelDMListEasySteps runat="server" id="TaradelDMListEasySteps" />

</section>



