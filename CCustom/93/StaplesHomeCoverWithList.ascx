<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaplesHomeCoverWithList.ascx.cs" Inherits="StaplesHomeCover" %>
<%@ Register Src="StaplesTestimonials.ascx" TagPrefix="appx" TagName="StaplesTestimonials" %>
<%@ Register Src="~/CCustom/93/StaplesCarousel.ascx" TagPrefix="appx" TagName="StaplesCarousel" %>
<%@ Register Src="~/CCustom/93/StaplesEasySteps.ascx" TagPrefix="appx" TagName="StaplesEasySteps" %>
<%@ Register Src="~/CCustom/93/PrivyWidget.ascx" TagPrefix="appx" TagName="PrivyWidget" %>






<section id="homePageContent">

    <appx:StaplesCarousel runat="server" id="StaplesCarousel" />

    <appx:StaplesEasySteps runat="server" id="StaplesEasySteps" />

    <appx:StaplesTestimonials runat="server" id="StaplesTestimonials" />

    <%--<appx:PrivyWidget runat="server" id="PrivyWidget" />--%>

</section>



