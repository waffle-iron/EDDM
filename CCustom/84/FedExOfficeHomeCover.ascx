<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FedExOfficeHomeCover.ascx.cs" Inherits="FedExOfficeHomeCover" %>
<%@ Register Src="FedExOfficeTestimonials.ascx" TagPrefix="appx" TagName="FedExOfficeTestimonials" %>
<%@ Register Src="FedExOfficeCarousel.ascx" TagPrefix="appx" TagName="FedExOfficeCarousel" %>
<%@ Register Src="FedExOfficeAtAGlance.ascx" TagPrefix="appx" TagName="FedExOfficeAtAGlance" %>






<section id="homePageContent">

    <appx:FedExOfficeCarousel runat="server" id="FedExOfficeCarousel" />

    <appx:FedExOfficeAtAGlance runat="server" id="FedExOfficeAtAGlance" />

    <appx:FedExOfficeTestimonials runat="server" id="FedExOfficeTestimonials" />

</section>



