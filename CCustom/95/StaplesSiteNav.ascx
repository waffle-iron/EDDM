<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaplesSiteNav.ascx.cs" Inherits="StaplesSiteNav" %>


<div id="userMenu">

    <ul class="nav navbar-nav">
        <li class="active"><a href="/default.aspx">Home <span class="sr-only">(current)</span></a></li>
        <li class="dropdown">
            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">
            Every Door Direct Mail<span class="caret"></span></a>
            <ul class="dropdown-menu">
                <li><a href="/Step1-Target.aspx">Get Started</a></li>
                <li role="separator" class="divider"></li>
                <li><a href="/EDDM-Overview">Overview</a></li>
                <li><a href="/EDDM-Target">Target Customers</a></li>
                <li><a href="/EDDM-Pricing">Pricing</a></li>
                <li><a href="/EDDM-Design">Design</a></li>
                <%--<li><a href="/EDDM-Quote-Request">Get Price Quote</a></li>--%>
            </ul>
        </li> 

        <li class="dropdown">
            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">
                Personalized Mail<span class="caret"></span></a>

            <ul class="dropdown-menu">
                <li><a href="/Personalized-Mail-Overview">Get Started</a></li>
                <li role="separator" class="divider"></li>
                <li><a href="/Personalized-Mail-Overview">Overview</a></li>
                <li><a href="/Addressed/Step1-BuildYourList.aspx">Build Your List</a></li>
                <li><a href="/Addressed/Step1-UploadYourList.aspx">Upload Your List</a></li>
                <li><a href="/Personalized-Mail-Pricing">Pricing</a></li>
                <li><a href="/Personalized-Mail-Design">Design</a></li>
                <%--<li><a href="/Addressed-List-Quote-Request">Get Price Quote</a></li>--%>
            </ul>
        </li>
        <%--<li><a href="/Quote-Guidance">Get A Quote</a></li>--%>
        <li><a href="/FAQ">FAQ</a></li>
        <li><a href="/Contact-Us">Contact Us</a></li>
    </ul>

</div>



