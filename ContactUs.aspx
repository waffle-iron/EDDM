<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="ContactUs.aspx.vb" Inherits="ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="phNav" Runat="Server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="phBody" Runat="Server">
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="phForm" Runat="Server">

    <div class="container">

        <div class="partialRibbon hidden-sm hidden-xs">
            <span class="arrowLeft"></span>
            <span class="subRibbonPop">Contact Us</span>
            <span class="subRibbon">By Phone, Live Chat, or Email</span>
        </div>

        <div class="partialRibbonSmall visible-sm visible-xs">
            <span class="subRibbonPopSmall">Contact Us</span>
            <span class="subRibbonSmall">By Phone, Live Chat, or Email</span>
        </div>

        <div class="contentWrapper">

            <div class="sectionHeader">Contact Us</div>

            <div class="row"> 
                       
                <div class="col-sm-6"> 
                           
                    <p class="lead">We're here to help by phone, live chat, or email. <em>That's 3 easy ways</em> to contact us. 
                    Our goal is to put you in touch with a live representative as quickly as possible. </p>

                    <h5>Mailing Address:</h5>

                    <p>Taradel LLC<br />
                    4805 Lake Brook Drive STE 140<br />
                    Glen Allen, VA 23060</p>

                    <h5>Phone:</h5>

                    <p><asp:Literal ID="litPhone" runat="server" /></p>

                    <p>&nbsp;</p>

                    <h5>Live Chat (online):</h5>

                    <p><a href="https://livechat.boldchat.com/aid/4508312422156215194/bc.chat?resize=true&amp;cbdid=2448837140957533226&amp;wdid=3571380572082624780" target="_blank" onclick="window.open((window.pageViewer &amp;&amp; pageViewer.link || function(link){return link;})(this.href + (this.href.indexOf('?')>=0 ? '&amp;' : '?') + 'url=' + escape(document.location.href)), 'Chat1081050786201172630', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=640,height=480');return false;">
                        <img width="181" height="55" alt="" src="/assets/images/live-chat.jpg" />
                    </a></p>

                    <p>&nbsp;</p>

                    <h5>Email:</h5>

                    <p><a href="mailto:info@everydoordirectmail.com">info@everydoordirectmail.com</a></p>

                    <p>&nbsp;</p>


                </div>                 

                <div class="col-sm-6"> 
                    <div class="embed-responsive embed-responsive-4by3">
                    <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3158.1940866245104!2d-77.57257500000006!3d37.668147!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x89b1400ff7c40cab%3A0xc6204c7dceac00bd!2s4805+Lake+Brook+Dr%2C+Glen+Allen%2C+VA+23060!5e0!3m2!1sen!2sus!4v1427748700669" width="600" height="450" frameborder="0" style="border:0"></iframe>
                    </div>
                </div>       
                      
            </div>

            <h5>Have a question?</h5>

            <p>Please complete this form and one of our friendly representatives  will contact you within one business day.</p>

            <p>Can't wait?   We understand!  Call us at <strong>(800) 481-1656</strong>.</p>

            <p>&nbsp;</p>

        </div>


    </div>

</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" Runat="Server">
</asp:Content>


<asp:Content ID="Content7" ContentPlaceHolderID="cpScripts" Runat="Server">
</asp:Content>

