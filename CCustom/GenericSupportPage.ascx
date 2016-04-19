<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenericSupportPage.ascx.cs" Inherits="GenericSupportPage" %>
<%@ Register Src="~/CLibrary/SitePhoneNumber.ascx" TagPrefix="uc1" TagName="SitePhoneNumber" %>
<%@ Register Src="~/CCustom/BoldChatTextLink.ascx" TagPrefix="uc1" TagName="BoldChatTextLink" %>
<%@ Register Src="~/CLibrary/SiteEmailAddress.ascx" TagPrefix="uc1" TagName="SiteEmailAddress" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="uc1" TagName="PageHeader" %>


<div class="container">
   
    <uc1:PageHeader runat="server" id="PageHeader" />
    
    <div class="contentWrapper">
        <div class="row">
            <div class="col-sm-8">
                <h2 class="headerUnderlined">Contact Us</h2>
                <p class="lead">
                    <strong>Fast, easy, and accurate customer support is our priority.</strong><br />
                    Please do not hesitate to contact one of our friendly customer support representatives with your direct mail service-related                  
                    questions, concerns, or feedback. We offer multiple ways to connect for your conveinence.
                </p>
                <p class="lead">Choose an option below to connect now.</p>
            </div>
            <div class="col-sm-4"><img src="/assets/images/phone-support-2.jpg" class="img-responsive" />            </div>
        </div>
        <div class="row grayPaddedRow">
            <div class="col-sm-3">
                <p class="text-center"><strong>Mailing Address:</strong></p>
                <p class="text-center">
                    Taradel LLC <br />
                    4805 Lake Brook Drive STE #140<br />
                    Glen Allen, VA 23060
                </p>
            </div>
            <div class="col-sm-3">
                <p class="text-center"><strong>Phone:</strong></p>
                <p class="text-center"><uc1:SitePhoneNumber runat="server" id="SitePhoneNumber" /></p>
            </div>
            <div class="col-sm-3">
                <p class="text-center"><strong>Live Chat (online):</strong></p>
                <p class="text-center"><uc1:BoldChatTextLink runat="server" id="BoldChatTextLink" /></p>
            </div>
            <div class="col-sm-3">
                <p class="text-center"><strong>Email:</strong></p>
                <p class="text-center"><uc1:SiteEmailAddress runat="server" id="SiteEmailAddress" /></p>
            </div>
        </div>
        <p>&nbsp;</p>



        <h2>Frequently Asked Questions</h2>

        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: What Is Every Door Direct Mail®?</h4>
                    <p>
                        EDDM® is a program created by the United States Postal Service®. The program enables advertisers to reach every
                        address within targeted carrier routes, at reduced rates, without the need for additional mailing services, lists, or permits.
                    </p>
                    <p>To date, advertisers have mailed over 60,000,000 postcards and flyers using our Every Door Direct Mail service.</p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: What Is a Carrier Route?</h4>
                    <p>
                        A carrier route is a group of addresses used by the USPS® to deliver mail in a specific area.
                        ZIP Codes™ may contain anywhere from several, to over a dozen carrier routes, depending on the
                        rural or urban nature of the region. Carrier routes are similar to neighborhoods.
                    </p>
                    <p>You can target carrier routes with U-Select, our top-rated online mapping tool. <a href="/Step1-Target.aspx" target="_self">CLICK HERE</a>.</p>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: How Much Does It Cost?</h4>
                    <p>
                        There are several factors that play into your total cost per piece. Production costs (design, printing, folding, bundling, etc),
                        delivery options, and postage costs will determine your rates.
                        By mailing larger quantities, you can obtain &quot;all inclusive&quot; rates as low as $0.25 per home, delivered.
                    </p>
                    <p>For an instant online price quote, <a href="/Pricing" target="_self">CLICK HERE</a>.</p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: How Long Does Delivery Take?</h4>
                    <p>
                        Turnaround time varies based on the required creative and production processes associated with each order.
                        Generally, the entire process usually takes from two to four weeks to deliver into mailboxes.
                        The creative process (design) usually plays the largest role in determining total turnaround time.
                        Customers who upload their own print-ready artwork usually experience the fastest delivery times.
                    </p>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: Does My Mail Deliver to Every Address? Can I Choose?</h4>
                    <p>
                        With EDDM®, your mailers deliver to every address/mailbox within your selected carrier routes.
                        You MUST deliver to all residential addresses, however, you may choose to include or exclude business
                        and PO Box addresses at no additional cost.
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: Can I Use Demographic Targeting with EDDM®?</h4>
                    <p>
                        With our new Demographics Filter, you can identify which carrier routes contain the highest density
                        of ideal prospects. Available demographic filters include: Household Income, Age, Home Ownership, Gender, and Presence of Children.
                    </p>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: How Do I Get Counts of Local Mailboxes for My Campaign?</h4>
                    <p>
                        U-Select, the online mapping tool, enables you to instantly gather mailbox counts in your
                        local market. Just &quot;point and click&quot; on routes to obtain instant counts and view a
                        breakdown by mailbox type (residential, business, &amp; PO Box) for each route.
                        <a href="/Step1-Target.aspx" target="_self">CLICK HERE to see</a>.
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: What Size Pieces Can I Mail?</h4>
                    <p>
                        The EDDM® program requires that you mail commercial flats. A commercial flat must be
                        more than 6.125&quot; tall OR longer than 11.5&quot; wide. The largest size the mail piece
                        can be is 12&quot;x15&quot; to qualify. The mailers must also be equal to or greater than
                        0.007 thick. We have made choosing a mail piece simple by offering the four most popular
                        sizes online. <a href="/Print" target="_self">CLICK HERE</a>.
                    </p>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: Does My Mail Deliver Inside of an Envelope of Publication?</h4>
                    <p>
                        EDDM® offers are mailed solo -- which means that only your offer will be delivered.
                        Your offer will never be inserted into an envelope, combined with other offers, or mailed as part of a publication.
                        EDDM® delivers your offers directly to your prospects, resulting in increased visibility and higher value.
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: Can I Print My Own Pieces and Use Every Door Direct Mail?</h4>
                    <p>
                        No. Our process is all-inclusive and designed to help you succeed, save time, and save money.
                        Our automated process includes design options, full-color printing, all USPS® paperwork, postage, and delivery to mailboxes.
                        In short, everything you need is included with our Every Door Direct Mail service.
                    </p>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: What Is the Average Response Rate for Every Door Direct Mail®?</h4>
                    <p>
                        Like all advertising, your response rates will be affected by many factors
                        including, but not limited to: your offers, your reputation, local competition,
                        local economic factors, local events, supply and demand, and even the weather.
                        With this in mind, direct mail is an extremely effective way of reaching new prospects.
                        There is no way to guarantee response rates or results.
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: What Is Your Refund Policy?</h4>
                    <p>
                        1) If your order does not include design, we will provide a full refund if you cancel the order
                        before the piece goes to print (roughly one week before your first mail drop date). 2) If you order design services,
                        we will provide a full refund of the design fee if the designer has not started working on your order, and we will provide
                        a full refund of the rest of the order if you cancel before the order is sent into production.
                        For more details, <a href="/terms" target="_self">CLICK HERE</a>.
                    </p>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>Q: Is Every Door Direct Mail Cheaper Than Mailing Myself?</h4>
                    <p>
                        In many cases, it will cost you more to design, print, and mail your own pieces when compared to
                        ordering our &quot;all inclusive&quot; packages for as little as $0.25 per home. We offer commercial-grade
                        results similar to what you would find from national advertisers, at discounted rates.
                        When you order with us, you leverage the power and efficiency of high-volume printing and mailing.
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well well-sm">
                    <h4>About Taradel LLC</h4>
                    <p>
                        Taradel LLC operates EveryDoorDirectMail.com. Taradel is a perennial Inc. 5000 company with an A+ Better Business Bureau rating.
                        All Every Door Direct Mail orders are managed and processed by Taradel LLC.
                    </p>
                    <p>To learn more about Taradel,&nbsp; <a href="http://www.Taradel.com" target="_self">CLICK HERE</a>.</p>
                </div>
            </div>
        </div>


    </div>
</div>




