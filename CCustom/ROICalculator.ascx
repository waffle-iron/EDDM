<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ROICalculator.ascx.vb"
    Inherits="CCustom_ROICalculator" %>
<div id="ROICalc">
    <div id="innerContent">
        <div id="customerLeverage" class="innerContentWrapper" style="width: 490px;">
            <div class="innerTopBar">
                <span id="titleTextMain">EDDM Campaign ROI Calculator v.0.11</span>
            </div>
            <h3>
                Choose Product</h3>
            <table style="width: 100%; border-spacing: 5px;">
                <tr>
                    <td>
                        <img src="/cmsimages/ROICalculator/images/425x11postcard-eddm.jpg" class="unselected" id="425x11" />
                    </td>
                    <td>
                        <img src="/cmsimages/ROICalculator/images/6x9postcard.jpg" class="unselected" id="6x9" />
                    </td>
                    <td>
                        <img src="/cmsimages/ROICalculator/images/8x11postcard.jpg" class="unselected" id="8x11" />
                    </td>
                    <td>
                        <img src="/cmsimages/ROICalculator/images/thumb_3_11x17flyer.jpg" class="unselected" id="11x17" />
                    </td>
                </tr>
                <tr>
                    <td class="blueWords">
                        4.25"x11"<br />
                        Postcard
                    </td>
                    <td class="blueWords">
                        6"x9"<br />
                        Postcard
                    </td>
                    <td class="blueWords">
                        8.5"x11"<br />
                        Postcard
                    </td>
                    <td class="blueWords">
                        11"x17"<br />
                        Postcard
                    </td>
                </tr>
            </table>
            <br />
            <div id="Div4" style="background-color: #FFFFFF; width: 490px;">
                <div style="margin-top: 10px;">
                    <h3>
                        Campaign Information</h3>
                </div>
                <div class="largeSliderWrapper" style="width: 480px; margin-top: -5px;">
                    <div class="largeSliderArea budget" style="width: 480px">
                        <div>
                            <span class="titleText panel_text">Direct Mail Budget</span>
                            <div id="Div5" class="iSlider" style="width: 280px;" data-slider-increment="500"
                                data-slider-max="1000000" data-slider-value-element="directMailBudget" data-slider-format="currency"
                                data-slider-initial-value="100000" onchange="CalculateROI2();">
                            </div>
                        </div>
                        <div id="directMailBudget" class="value">
                            $0.00</div>
                        <br />
                        <div id="qty" style="position: absolute; right: 100px;">
                        </div>
                    </div>
                    <!--end largeSliderAreaBudget-->
                    <div class="largeSliderArea ExpectedResponseRate" style="width: 480px">
                        <div>
                            <span class="titleText panel_text">Expected Response Rate</span>
                            <div id="Div1" class="iSlider" style="width: 280px" data-slider-increment="0.5" data-slider-max="15"
                                data-slider-value-element="ResponseRate" data-slider-format="percent" data-slider-initial-value="5.5"
                                onchange="CalculateROI2();">
                            </div>
                        </div>
                        <div id="ResponseRate" class="value">
                            0%</div>
                    </div>
                    <div class="largeSliderArea SalesConversionRate" style="width: 480px">
                        <div>
                            <span class="titleText panel_text">Expected Sales Conversion Rate</span>
                            <div id="Div2" class="iSlider" style="width: 280px" data-slider-increment="5" data-slider-max="100"
                                data-slider-value-element="SalesConversionRate" data-slider-format="percent"
                                data-slider-initial-value="25" onchange="CalculateROI2();">
                            </div>
                        </div>
                        <div id="SalesConversionRate" class="value">
                            0</div>
                    </div>
                    <div class="largeSliderArea SpendPerBuyer" style="width: 480px">
                        <div>
                            <span class="titleText panel_text">Expected Spend Per Buyer (Avg. Sale Worth)</span>
                            <div id="Div3" class="iSlider" style="width: 280px" data-slider-increment="500" data-slider-max="1000000"
                                data-slider-value-element="ExpectedSpend" data-slider-format="currency" data-slider-initial-value="10000"
                                onchange="CalculateROI2();">
                            </div>
                        </div>
                        <div id="ExpectedSpend" class="value">
                            $100.00</div>
                    </div>
                    <!--works at 11:00 am-->
                    <div>
                        <div>
                            <h3>
                                Get Your Results</h3>
                        </div>
                        <div class="row">
                            <div class="zleft">
                                Number of responses&nbsp<div id="numResponses" class="zvalue">
                                    100</div>
                            </div>
                            <!--<div class="zright">
                    Cost per<br />response<div id="costPerResponse" class="zvalue">$100</div>
                </div>-->
                        </div>
                        <br />
                        <div class="row">
                            <div class="zleft">
                                Number of buyers from campaign&nbsp<div id="numBuyers" class="zvalue">
                                    100</div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="zleft" style="background-image: url('/cmsimages/ROICalculator/images/Revenue-img.png'); background-repeat: no-repeat;">
                                <div style="padding-left: 70px;">
                                    Total campaign revenue generated</div>
                                <div id="totalCampaignRevenue" class="zvalue">
                                    $100</div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="zTotalROI">
                                Return on Investment<div id="ROI" class="zvalue">
                                    10%</div>
                            </div>
                        </div>
                    </div>
                    <!--not displayed but required for slider to work figure out when time -->
                    <div class="largeSliderArea people" style="display: none;">
                        <div>
                            <span class="titleText panel_text">Quantity Sent</span>
                            <div id="leverageNumCustomersSlider" class="iSlider" data-slider-increment="1000"
                                data-slider-max="100000" data-slider-value-element="leverageNumCustomersVal"
                                data-slider-format="comma" data-slider-initial-value="1000" onchange="CalculateROI();">
                            </div>
                        </div>
                        <div id="leverageNumCustomersVal" class="value">
                            0</div>
                    </div>
                    <div class="largeSliderArea email" style="display: none;">
                        <div>
                            <span class="titleText panel_text">Response Rate</span>
                            <div id="leverageTakeRateSlider" class="iSlider" data-slider-increment="1" data-slider-max="5"
                                data-slider-value-element="leverageTakeRateVal" data-slider-format="percent"
                                data-slider-initial-value="1" onchange="CalculateROI();">
                            </div>
                        </div>
                        <div id="leverageTakeRateVal" class="value">
                            0%</div>
                    </div>
                    <div class="largeSliderArea check" style="display: none;">
                        <div>
                            <span class="titleText panel_text">Average Order</span>
                            <div id="leverageCheckSlider" class="iSlider" data-slider-increment="500" data-slider-max="100000"
                                data-slider-value-element="leverageCheckVal" data-slider-format="currency" data-slider-initial-value="1000"
                                onchange="CalculateROI();">
                            </div>
                        </div>
                        <div id="leverageCheckVal" class="value">
                            $10.00</div>
                    </div>
                    <div class="largeSliderArea tag" style="display: none;">
                        <div>
                            <span class="titleText panel_text">Cost of Goods Sold</span>
                            <div id="leverageCOGSlider" class="iSlider" data-slider-increment="1" data-slider-max="100"
                                data-slider-value-element="leverageCOGVal" data-slider-format="percent" data-slider-initial-value="1">
                            </div>
                        </div>
                        <div id="leverageCOGVal" class="value">
                            1%</div>
                    </div>
                </div>
            </div>
        </div>
        <!--<div class="row">Get Your Results</div>-->
    </div>
    <!--largeSliderWrapper-->
    <div class="innerSubBox flippable" style="display: none;">
        <div class="card front">
            <div class="innerSubBoxTitle">
                <span class="titleText">TOTALS</span></div>
            <div class="totalGroup shaded topgroup">
                <span class="totalLabel">Revenue From Campaign</span>
                <div class="totalWrapper">
                    <span id="leverageTotalRevenue">$0.00</span></div>
                <!--<span class="totalLabel">Gross Margin From Campaign</span>
                            <div class="totalWrapper"><span id="leverageGrossMargin">$0.00</span></div>-->
            </div>
            <div class="totalGroup shaded hidden">
                <span class="totalLabel">Annual Revenue From Campaign</span>
                <div class="totalWrapper">
                    <span id="leverageAnnualRevenue">$0.00</span></div>
                <span class="totalLabel">Annual Gross Margin From Campaign</span>
                <div class="totalWrapper">
                    <span id="leverageAnnualGrossMargin">$0.00</span></div>
            </div>
            <div class="totalGroup roi">
                <span class="totalLabel">Campaign ROI</span>
                <div class="totalWrapper">
                    <span id="leverageDealROI">0%</span></div>
            </div>
            <div class="card back">
                <div class="innerSubBoxTitle">
                    <span class="titleText">TOTALS</span></div>
                <div class="innerSubBoxTitleOverlay">
                </div>
                <div id="leverageCostVal">
                    $5.00</div>
                <div id="leverageCostPrompt">
                    SELECT THE COST OF THE CAMPAIGN</div>
                <div id="leverageCostSlider" class="iSlider" data-slider-increment="500" data-slider-min="0"
                    data-slider-max="20000" data-slider-value-element="leverageCostVal" data-slider-format="currency"
                    data-slider-initial-value="6000">
                </div>
            </div>
            <!-- Wait message for offline loading -->
            <div id="loaderOverlay">
                <div class="loaderCenterpoint">
                    <div class="loader">
                        Updating App
                        <br />
                        Please wait...
                        <div class="spinner">
                        </div>
                    </div>
                </div>
            </div>
            <!-- This null JS helps overcome problems on some devices -->
            <script type="text/javascript">                null;</script>
            <div style="display: none;">
                <select class="titleText panel_text" multiple size="4" id="PrintOptions" style="font-family: Calibri;
                    font-size: 12px;" onchange="CalculateROI();">
                    <option>11x17</option>
                    <option>8.5x11</option>
                    <option>6.25x9</option>
                    <option>4.25x11</option>
                </select>
                <div class="clear">
                </div>
                <div id="website_footer">
                    <img src="/cmsimages/ROICalculator/images/TaradelFooter.gif" /></div>
                <div>
                    <div class="columns">
                        <div class="red">
                            Size</div>
                        <div class="grey">
                            Quantity</div>
                        <div class="red">
                            Potential</div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="columns" title="11x17">
                        <div class="red">
                            11x17</div>
                        <div class="grey">
                            <div id="div11Qty">
                            </div>
                        </div>
                        <div class="red">
                            <div id="div11Cash">
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="columns" title="8.5x11">
                        <div class="red">
                            8.5x11</div>
                        <div class="grey">
                            <div id="div85Qty">
                            </div>
                        </div>
                        <div class="red">
                            <div id="div85Cash">
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="columns" title="6.25x9">
                        <div class="red">
                            6.25x9</div>
                        <div class="grey">
                            <div id="div625Qty">
                            </div>
                        </div>
                        <div class="red">
                            <div id="div625Cash">
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="columns" title="4.25x11">
                        <div class="red">
                            4.25x11</div>
                        <div class="grey">
                            <div id="div425Qty">
                            </div>
                        </div>
                        <div class="red">
                            <div id="div425Cash">
                            </div>
                        </div>
                    </div>
                </div>
                <label for="DesignHelp">
                    Design Help?</label>
                <input type="checkbox" title="DesignHelp" name="DesignHelp" id="DesignHelp" onchange="CalculateROI();">
            </div>
        </div>
    </div>
</div>
