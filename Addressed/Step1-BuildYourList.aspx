<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="true" CodeFile="Step1-BuildYourList.aspx.cs" Inherits="BuildList" Trace="False" %>
<%@ Register Src="~/CCustom/BoldChatTextLink.ascx" TagPrefix="appx" TagName="BoldChatTextLink" %>
<%@ Register Src="~/Controls/OrderSteps.ascx" TagPrefix="appx" TagName="OrderSteps" %>
<%@ Register Src="~/CCustom/Banner-ExpertTeams.ascx" TagPrefix="appx" TagName="BannerExpertTeams" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="appx" TagName="PageHeader" %>







<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
    <link href="/assets/css/jquery-ui.css" rel="stylesheet" />
</asp:Content>



<asp:content id="Content2" contentplaceholderid="phForm" runat="Server">

    <div class="container">

        <appx:PageHeader runat="server" id="PageHeader" />

        <div class="contentWrapper">

            <%--Steps--%>
            <appx:OrderSteps runat="server" id="OrderSteps" />

            <%--Error Panel--%>
            <asp:Panel ID="pnlSysError" runat="server" Visible="False" CssClass="alert alert-danger">
                <div>
                    <span class="fa fa-exclamation-circle"></span>&nbsp;
                    <asp:Literal ID="litSysError" runat="server" />
                </div>
            </asp:Panel>

            <%--Debug Data--%>
            <asp:Panel ID="pnlDebug" runat="server" CssClass="hidden">

                <div class="row">
                    <div class="col-sm-12">
                        <div class="alert alert-danger">
        
                            <h3>Test Mode</h3>

                            <p><strong>Selected Form Data</strong></p>
                            <table class="table table-condensed">

                            <tr>
                                <td>
                                    <p>Count:<br />
                                    <asp:TextBox ID="txtCount" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control"></asp:TextBox></p>
                                </td>

                                <td>
                                    <p>Latitude:<br />
                                    <asp:TextBox ID="txtLatitude" runat="server" ClientIDMode="Static" Text="" CssClass="form-control"></asp:TextBox></p>
                                </td>

                                <td>
                                    <p>Longitude:<br />
                                    <asp:TextBox ID="txtLongitude" runat="server" ClientIDMode="Static" Text="" CssClass="form-control"></asp:TextBox></p>
                                </td>

                                <td>
                                    &nbsp;
                                </td>

                            </tr>

                            <tr>
                                <td>
                                    <p>Home Ownership:<br />
                                    <asp:TextBox ID="txtHomeOwnership" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>
                                </td>

                                <td>
                                    <p>Married:<br />
                                    <asp:TextBox ID="txtMartialStatus" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>

                                <td>
                                    <p>Children:<br />
                                    <asp:TextBox ID="txtChildren" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>

                                <td>&nbsp;
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <p>Income:<br />
                                    <asp:TextBox ID="txtCombinedIncome" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>
                                <td>
                                    <p>Min Income:<br />
                                    <asp:TextBox ID="txtMinIncome" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>
                                <td>
                                    <p>Max Income:<br />
                                    <asp:TextBox ID="txtMaxIncome" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>
                                <td>&nbsp;</td>
                            </tr>

                            <tr>
                                <td>
                                    <p>Ages:<br />
                                    <asp:TextBox ID="txtAgeRanges" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>

                                <td>
                                    <p>Min Age:<br />
                                    <asp:TextBox ID="txtMinAge" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>

                                <td>
                                    <p>Max Age:<br />
                                    <asp:TextBox ID="txtMaxAge" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>

                                <td>
                                    <p>Gender:<br />
                                    <asp:TextBox ID="txtGender" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <p>Property Values:<br />
                                    <asp:TextBox ID="txtPropertyValue" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>

                                <td>
                                    <p>Property Value Total Selected:<br />
                                    <asp:HiddenField ID="hidPropValueTotalSelected" runat="server" ClientIDMode="Static" Value="0" />
                                    <asp:TextBox ID="txtPropValueTotalSelected" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control"></asp:TextBox></p>                 
                                </td>

                                <td>
                                    <p>Net Worth Values:<br />
                                    <asp:TextBox ID="txtNetWorth" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                  
                                </td>

                                <td>
                                    <p>Net Worth Total Selected:<br />
                                    <asp:HiddenField ID="hidNetWorthTotalSelected" runat="server" ClientIDMode="Static" Value="0" />
                                    <asp:TextBox ID="txtNetWorthTotalSelected" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control"></asp:TextBox></p>                 
                                </td>
                            </tr>

                            <tr>

                                <td>
                                    <p>Ethnicity:<br />
                                    <asp:TextBox ID="txtEthnicity" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                 
                                </td>

                                <td>
                                    <p>Ethnicity Total Selected Selected:<br />
                                    <asp:HiddenField ID="hidTotalEthnicitySelected" runat="server" ClientIDMode="Static" Value="0" />
                                    <asp:TextBox ID="txtTotalEthnicitySelected" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control"></asp:TextBox></p>                 
                                </td>

                                <td>
                                    <p>Zip Codes:<br />
                                    <asp:TextBox ID="txtZipCodes" runat="server" ClientIDMode="Static" Text="(not defined)" CssClass="form-control"></asp:TextBox></p>                 
                                </td>

                                <td>
                                    &nbsp;                 
                                </td>

                            </tr>

                            <tr>
                                <td colspan="4">

                                    <p>API call:<br />
                                    <textarea id="txtUrlText" cols="170" rows="6" style="font-size:12px;" class="form-control"></textarea></p>
                                    <asp:TextBox ID="txtAPIUrl" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>

                                    <asp:HiddenField ID="hidRawMinIncome" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hidRawMaxIncome" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hidRawMinAge" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hidRawMaxAge" runat="server" ClientIDMode="Static" />

                                </td>
                            </tr>

                            </table>

                            <p>&nbsp;</p>

                            <p><strong>Cookie Detection</strong></p>

                            <p>DistributionID Cookie:<br />
                            <asp:Literal ID="litDistIDCookie" runat="server" /></p>

                            <hr />

                            <p><strong>Saved Filter Data (from visit with past 20 minutes)</strong></p>

                            <p>Selected Filters:<br />
                            <asp:Literal ID="litFilterData" runat="server" /></p>

                            <hr />

                            <p><strong>Clear Cookies / Session</strong></p>

                            <p><asp:Button ID="btnClearAll" CssClass="btn btn-sm btn-danger" runat="server" Text="Clear Cookies and Session" OnClick="btnClearAll_Click" /></p>

                        </div>
                    </div>
                </div>

            </asp:Panel>


            <div class="row">

                <%--Data--%>
                <div class="col-sm-8">

                    <%--Error Panel--%>
                    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                        <p>&nbsp;</p>
                        <span class="fa fa-2x fa-exclamation-circle text-danger"></span>&nbsp;
                        <asp:Literal ID="litErrorMessage" runat="server" />
                        <p>&nbsp;</p>
                        <p>&nbsp;</p>
                    </asp:Panel>


                    <%--Data / Results / Intro Panel--%>
                    <asp:Panel ID="pnlFilterData" runat="server" Visible="true">

                        <%--Intro Block--%>
                        <div class="well well-sm" id="introBlock">

                            <p>I am adicted to the Hokey Pokey but I'll turn myself around...</p>

                            <p class="lead"><span class="leadDropWord">How This Works</span><br />
                            Generating your Personalized Mail list is easy with our online campaign builder. 
                            Step by step, you can create and launch effective direct mail campaigns in minutes.</p>

                            <div class="row">
                                <div class="col-sm-10">
                                    <h5>Step 1 - Define Target Area</h5>
                                    <p>Get started by entering your business address or ZIP Code(s) to define a geographic target area. 
                                    You can also apply a radius or drive-time around your target area to further enhance results.</p>
                                </div>

                                <div class="col-sm-2 hidden-xs">
                                    <h5 class="text-center"><span class="fa fa-fw fa-4x fa-crosshairs"></span></h5>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-10">
                                    <h5>Step 2 - Target Customers</h5>
                                    <p>Next, refine your list based on the traits and characteristics which best represent your ideal prospect. 
                                    Choose from demographic and behavioral selects such as home ownership, gender, net worth, income level, ethnicity, 
                                    and more. As you refine your search, the total number of prospects will increase or decrease accordingly.</p>
                                </div>

                                <div class="col-sm-2 hidden-xs">
                                    <h5 class="text-center"><span class="fa fa-fw fa-4x fa-user-plus"></span></h5>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-10">
                                    <h5>Step 3 - Choose Design & Print Options</h5>
                                    <p>With your ideal prospects targeted, you can now choose your preferred design and print options. 
                                    From FREE design templates, to large format postcards, we make getting great results easy.</p>
                                </div>

                                <div class="col-sm-2 hidden-xs">
                                    <h5 class="text-center"><span class="fa fa-fw fa-4x fa-list"></span></h5>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-10">
                                    <h5>Step 4 - Schedule Mail Delivery</h5>
                                    <p>Based on your campaign requirements, our online campaign builder presents you with a list of available 
                                    delivery schedules. Simply choose the preferred delivery date range to ensure that your offers deliver to 
                                    the right prospects, at the right time.</p>
                                </div>

                                <div class="col-sm-2 hidden-xs">
                                    <h5 class="text-center"><span class="fa fa-fw fa-4x fa-truck"></span></h5>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-10">
                                    <h5>Step 5 - Checkout</h5>
                                    <p>Review your campaign details and confirm that all information appears correctly. 
                                    Next, complete all required billing and payment information and submit your order. </p>
                                </div>

                                <div class="col-sm-2 hidden-xs">
                                    <h5 class="text-center"><span class="fa fa-fw fa-4x fa-cart-plus"></span></h5>
                                </div>

                            </div>

                            <p>&nbsp;</p>

                            <p>&nbsp;</p>

                        </div>

                        <%--Results Block--%>
                        <div id="resultsBlock">

                            <%--Results found--%>
                            <div id="returnedResults" class="well well-sm">

                                <p class="lead"><span class="leadDropWord">Here Are Your Results!</span></p>
                        
                                <p>&nbsp;</p>

                                <p class="lead">There are <span id="addressedMailCount" class="leadDropWord"></span> households matching your criteria. Listed below
                                are recommended products for your marketing campaign.</p>

                                <p>When you are finished, please provide a <strong>name</strong> for your custom list and click Continue.</p>

                                <p>&nbsp;</p>

                                <%--List Name--%>
                                <div class="row">
                                    <div class="col-sm-8 col-sm-offset-2">

                                        <div class="form-group">
                            
                                            <label for="txtListName" class="label formLabelRequired">List Name</label>
                                            <asp:TextBox ID="txtListName" runat="server" CssClass="form-control" MaxLength="100" />
                                            <asp:RequiredFieldValidator ID="rfvListName" ControlToValidate="txtListName" runat="server" ErrorMessage="List Name is a required field." Display="dynamic" CssClass="label label-danger" SetFocusOnError="True">
                                                List Name is  required.
                                            </asp:RequiredFieldValidator>

                                        </div>

                                    </div>
                                </div>

                                <p>&nbsp;</p>

                                <p>&nbsp;</p>

                                <%--Continue Button--%>
                                <div class="row">
                                    <div class="col-sm-10 col-sm-offset-1">
                                        <asp:LinkButton ID="btnContinue" runat="server" CssClass="btn btn-cta lrgActionButton btn-lg btn-block" OnClick="btnContinue_Click">
                                            Continue&nbsp;<span class="fa fa-chevron-right"></span>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <p>&nbsp;</p>

                                <div class="hidden alert alert-warning" id="belowMinimumQty">
                                    <span class="fa fa-info-circle"></span>&nbsp;Addressed List Campaigns require a minimum quantity of 1,000. Please increase your target radius or
                                    apply less filters.
                                </div>

                            </div>

                            <div>&nbsp;</div>

                            <%--No results--%>
                            <div id="noResults" class="well">

                                <h5 class="text-center">No Results Found</h5>

                                <p>&nbsp;</p>

                                <div class="alert alert-warning">
                                    <p><span class="fa fa-warning fa-2x text-warning"></span>&nbsp;Sorry - no results found.  This is most likely due to your criteria being too restrictive
                                    or you have entered an invalid Zip Code.  Please check these values and try again.</p>

                                    <p>If you continue to experience problems, please <a href="/help">Contact Us</a> for assistance.</p>

                                    <p>&nbsp;</p>

                                    <p class="text-center">
                                        <a href="Step1-BuildYourList.aspx" title="Try Again?" class="btn btn-primary">
                                            Try Again?
                                        </a>
                                    </p>

                                </div>

                                <p>&nbsp;</p>

                            </div>

                        </div>

                    </asp:Panel>

                </div>

                <%--Form & Filters--%>
                <div class="col-sm-4">

                    <div>

                        <%--Target Area Form--%>
                        <div id="targetAreaBlock" class="well well-sm">
                            
                            <asp:ValidationSummary ID="vsAddress" runat="server" HeaderText="Please check for the following errors:" CssClass="alert alert-danger" />

                            <div id="validationSummary" class="hidden">
                                <span class="fa fa-exclamation-triangle"></span>&nbsp;<span id="validationMessage"></span>
                            </div>

                            <h4>Step 1 - Define Target Area&nbsp;<span class="fa fa-crosshairs"></span></h4>

                            <div>&nbsp;</div>

                            <div id="defineYourAreaToBegin" class="alert alert-inverse text-center">
                                Define your Target Area to begin
                            </div>

                            <%--Radio Buttons--%>
                            <div id="radioButtonsBlock">
                                <div class="radio">
                                    <label>
                                        <asp:RadioButton ID="radAddress" runat="server" ClientIDMode="Static" GroupName="TargetType" /> By Address
                                    </label>
                                </div>

                                <div class="radio">
                                    <label>
                                        <asp:RadioButton ID="radZipCodes" runat="server" ClientIDMode="Static" GroupName="TargetType" /> By Zip Code(s)
                                    </label>
                                </div>
                            </div>

                            <p>&nbsp;</p>

                            <%--Address Radius Block--%>
                            <div id="addressBlock">

                                <div class="alert alert-warning" id="addressInstructions">
                                    <span class="fa fa-info-circle"></span>&nbsp;Please provide a street address and a Zip Code.
                                </div>


                                <%--Address--%>
                                <div class="form-group" id="addressControlBlock">
                                    <label id="addressLabel" class="label formLabel formLabelRequired" for="txtAddress">Address</label>

                                    <div class="row">
                                        <div class="col-xs-12">
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                           
                                </div>

                                <%--Zip Code--%>
                                <div class="form-group" id="zipCodeControlBlock">
                                    <label id="zipCodeLabel" class="label formLabel formLabelRequired" for="txtZipCode">Zip Code</label>
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" ClientIDMode="Static" MaxLength="5"></asp:TextBox>
                                        </div>
                                    </div>
                            
                                </div>

                                <%--Radius Type--%>
                                <div class="form-group">
                                    <label class="label formLabel formLabelRequired" for="ddlRadiusType">Radius Type</label>
                                    <div class="row">
                                        <div class="col-xs-10">
                                            <asp:DropDownList ID="ddlRadiusType" runat="server" CssClass="form-control" ClientIDMode="Static">
                                                <asp:ListItem Value="Miles" Selected="True">Miles</asp:ListItem>
                                                <asp:ListItem Value="Minutes">Drive Time (Minutes)</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <%--Radius values--%>
                                <div class="form-group">
                                    <label class="label formLabel formLabelRequired" for="ddlRadiusValue">Miles / Minutes</label>
                                    <div class="row">
                                        <div class="col-xs-10">
                                            <asp:DropDownList ID="ddlRadiusValue" runat="server" CssClass="form-control" ClientIDMode="Static">
                                                <asp:ListItem Value="1" Selected="True">1</asp:ListItem>
                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="15">15</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="25">25</asp:ListItem>
                                                <asp:ListItem Value="50">50</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <%--Button--%>
                                <div id="targetAreaButton">
                                    <p class="text-right">
                                        <a id="hypNext" class="btn btn-default disabled" onclick="GoToStep2();">
                                            Use This Address  - Go to Step 2&nbsp;<span class="fa fa-chevron-right"></span>
                                        </a>
                                    </p>
                                </div>


                            </div>


                            <%--Zip Code Block--%>
                            <div id="zipCodesBlock">

                                <div class="alert alert-warning" id="zipCodeInstructions">
                                    <span class="fa fa-info-circle"></span>&nbsp;Please provide Zip Codes (5 digits each) separated by commas.
                                </div>
                                
                                <p><small>11111,22222,33333, etc...</small></p>

                                <%--Zip Codes--%>
                                <div class="form-group" id="zipCodesListControlBlock">
                                    <label id="zipCodesListLabel" class="label formLabel" for="txtZipCodes">Zip Codes</label>

                                    <div class="row">
                                        <div class="col-xs-12">
                                            <asp:TextBox ID="txtZipCodesList" runat="server" CssClass="form-control" ClientIDMode="Static" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </div>
                                    </div>
                           
                                </div>

                                <%--Button--%>
                                <div id="zipCodesButton">
                                    <p class="text-right">
                                        <a href="#" id="hypNextZipList" class="btn btn-default disabled" onclick="GoToStep2();">
                                            Use These Zip Codes  - Go to Step 2
                                        </a>
                                    </p>
                                </div>


                            </div>


                        </div>

                        <%--Filters--%>
                        <div id="filtersBlock" class="well well-sm">

                            <h4>Step 2 - Target Customers&nbsp;<span class="fa fa-crosshairs"></span></h4>

                            <p><small>Expand and apply the filters below to target your ideal customers.</small></p>

                            <div class="panel-group" id="filterControlsBlock" role="tablist" aria-multiselectable="true">
                                
                                <%--Home Ownership--%>
                                <div class="panel panel-info" id="homeOwnershipWrapper">

                                    <div class="panel-heading" role="tab" id="homeOwnershipHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="homeOwnershipSelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#homeOwnershipControlBlock" aria-expanded="true" aria-controls="homeOwnershipControlBlock">
                                                <span id="homeOwnershipIcon" class="fa fa-plus fa-border"></span>&nbsp;Home Ownership&nbsp;<span class="categoryLabel pull-right" id="homeOwnershipLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>

                                    <div id="homeOwnershipControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="homeOwnershipHeaderBlock">

                                        <div class="panel-body ui-controls">
                                            <button onclick="HomeOwnershipSelected('btnHomeowner');" type="button" class="btn btn-sm btn-block" id="btnHomeowner">Homeowner</button>
                                            <button onclick="HomeOwnershipSelected('btnRenter');" type="button" class="btn btn-sm btn-block" id="btnRenter">Renter</button>
                                        </div>

                                    </div>

                                </div>

                                <%--Marital Status--%>
                                <div class="panel panel-info" id="maritalStatusWrapper">
                                    <div class="panel-heading" role="tab" id="maritalStatusHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="maritalStatusSelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#maritalStatusControlBlock" aria-expanded="true" aria-controls="maritalStatusControlBlock">
                                                <span id="maritalStatusIcon" class="fa fa-plus fa-border"></span>&nbsp;Marital Status&nbsp;<span class="categoryLabel pull-right" id="maritalStatusLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>
                            
                                    <div id="maritalStatusControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="maritalStatusHeaderBlock">

                                        <div class="panel-body ui-controls">
                                            <button onclick="MaritalSelected('btnMarried');" type="button" class="btn btn-sm btn-block" id="btnMarried">Married</button>
                                            <button onclick="MaritalSelected('btnSingle');" type="button" class="btn btn-sm btn-block" id="btnSingle">Single</button>
                                        </div>

                                    </div>

                                </div>

                                <%--Children--%>
                                <div class="panel panel-info" id="childrenWrapper">
                                    <div class="panel-heading" role="tab" id="childrenHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="childrenSelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#childrenControlBlock" aria-expanded="true" aria-controls="childrenControlBlock">
                                                <span id="childrenIcon" class="fa fa-plus fa-border"></span>&nbsp;Children&nbsp;<span class="categoryLabel pull-right" id="childrenLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>
                            
                                    <div id="childrenControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="childrenHeaderBlock">

                                        <div class="panel-body ui-controls">
                                            <button onclick="ChildrenSelected('btnChildren');" type="button" class="btn btn-sm btn-block" id="btnChildren">Children</button>
                                            <button onclick="ChildrenSelected('btnNoChildren');" type="button" class="btn btn-sm btn-block" id="btnNoChildren">No Children</button>
                                        </div>

                                    </div>

                                </div>

                                <%--Gender--%>
                                <div class="panel panel-info" id="genderWrapper">
                                    <div class="panel-heading" role="tab" id="genderHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="genderSelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#genderControlBlock" aria-expanded="true" aria-controls="genderControlBlock">
                                                <span id="genderIcon" class="fa fa-plus fa-border"></span>&nbsp;Gender&nbsp;<span class="categoryLabel pull-right" id="genderLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>
                            
                                    <div id="genderControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="genderHeaderBlock">

                                        <div class="panel-body ui-controls">
                                            <button onclick="GenderSelected('btnMale');" type="button" class="btn btn-sm btn-block" id="btnMale">Male</button>
                                            <button onclick="GenderSelected('btnFemale');" type="button" class="btn btn-sm btn-block" id="btnFemale">Female</button>
                                        </div>

                                    </div>

                                </div>

                                <%--Income--%>
                                <div class="panel panel-info" id="incomeWrapper">
                                    <div class="panel-heading" role="tab" id="incomeHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="incomeSelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#incomeControlBlock" aria-expanded="true" aria-controls="incomeControlBlock">
                                                <span id="incomeIcon" class="fa fa-plus fa-border"></span>&nbsp;Income&nbsp;<span class="categoryLabel pull-right" id="incomeLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>
                            
                                    <div id="incomeControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="incomeHeaderBlock">

                                        <div class="panel-body">

                                            <div>
                                                <small>Household Income: <span id="minIncome">$0</span> - <span id="maxIncome">$250,000+</span></small>
                                            </div>

                                            <div>&nbsp;</div>

                                            <div id="incomeSlider"></div>

                                        </div>

                                    </div>

                                </div>

                                <%--Age--%>
                                <div class="panel panel-info" id="ageWrapper">
                                    <div class="panel-heading" role="tab" id="ageHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="ageSelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#ageControlBlock" aria-expanded="true" aria-controls="ageControlBlock">
                                                <span id="ageIcon" class="fa fa-plus fa-border"></span>&nbsp;Age Range&nbsp;<span class="categoryLabel pull-right" id="ageLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>
                            
                                    <div id="ageControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="ageHeaderBlock">

                                        <div class="panel-body">

                                            <div>
                                                <small>Age Range: <span id="minAge">18</span> - <span id="maxAge">54</span></small>
                                            </div>

                                            <div>&nbsp;</div>

                                            <div id="ageSlider"></div>

                                        </div>

                                    </div>

                                </div>

                                <%--Property Value--%>
                                <div class="panel panel-info" id="propertyWrapper">
                                    <div class="panel-heading" role="tab" id="propertyHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="propertySelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#propertyControlBlock" aria-expanded="true" aria-controls="propertyControlBlock">
                                                <span id="propertyIcon" class="fa fa-plus fa-border"></span>&nbsp;Property Value&nbsp;<span class="categoryLabel pull-right" id="propertyLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>
                            
                                    <div id="propertyControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="propertyHeaderBlock">

                                        <div class="panel-body">


                                            <div>
                                                <small>Check the values you wish to include.</small>
                                            </div>

                                            <div>

                                                <div class="checkbox">
                                                    <div class="text-right">
                                                        <label>
                                                            <input id="chkSelectAllPropValue" class="chkbox" type="checkbox" /><strong><span id="selectAllPropValueLabel">Select All</span></strong>
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue1" class="propval" onclick="PropValueSelected();" value="A" type="checkbox" /> $1,000 - $24,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue2" class="propval" onclick="PropValueSelected();" value="B" type="checkbox" /> $25,000 - $49,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue3" class="propval" onclick="PropValueSelected();" value="C" type="checkbox" /> $50,000 - $74,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue4" class="propval" onclick="PropValueSelected();" value="D" type="checkbox" /> $75,000 - $99,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue5" class="propval" onclick="PropValueSelected();" value="E" type="checkbox" /> $100,000 - $124,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue6" class="propval" onclick="PropValueSelected();" value="F" type="checkbox" /> $125,000 - $149,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue7" class="propval" onclick="PropValueSelected();" value="G" type="checkbox" /> $150,000 - $174,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue8" class="propval" onclick="PropValueSelected();" value="H" type="checkbox" /> $175,000 - $199,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue9" class="propval" onclick="PropValueSelected();" value="I" type="checkbox" /> $200,000 - $224,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue10" class="propval" onclick="PropValueSelected();" value="J" type="checkbox" /> $225,000 - $249,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue11" class="propval" onclick="PropValueSelected();" value="K" type="checkbox" /> $250,000 - $274,999
                                                        </label>
                                                    </div>


                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue12" class="propval" onclick="PropValueSelected();" value="L" type="checkbox" /> $275,000 - $299,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue13" class="propval" onclick="PropValueSelected();" value="M" type="checkbox" /> $300,000 - $349,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue14" class="propval" onclick="PropValueSelected();" value="N" type="checkbox" /> $350,000 - $399,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue15" class="propval" onclick="PropValueSelected();" value="O" type="checkbox" /> $400,000 - $449,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue16" class="propval" onclick="PropValueSelected();" value="P" type="checkbox" /> $450,000 - $499,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue17" class="propval" onclick="PropValueSelected();" value="Q" type="checkbox" /> $500,000 - $749,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue18" class="propval" onclick="PropValueSelected();" value="R" type="checkbox" /> $750,000 - $999,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkPropValue19" class="propval" onclick="PropValueSelected();" value="S" type="checkbox" /> $1,000,000+
                                                        </label>
                                                    </div>


                                                </div>

                                            </div>



                                        </div>

                                    </div>

                                </div>

                                <%--Net Worth--%>
                                <div class="panel panel-info" id="netWorthWrapper">
                                    <div class="panel-heading" role="tab" id="netWorthHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="netWorthSelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#netWorthControlBlock" aria-expanded="true" aria-controls="netWorthControlBlock">
                                                <span id="netWorthIcon" class="fa fa-plus fa-border"></span>&nbsp;Net Worth&nbsp;<span class="categoryLabel pull-right" id="netWorthLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>
                            
                                    <div id="netWorthControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="netWorthHeaderBlock">

                                        <div class="panel-body">

                                            <div>
                                                <small>Check the values you wish to include.</small>
                                            </div>

                                            <div>

                                                <div class="checkbox">
                                                    <div class="text-right">
                                                        <label>
                                                            <input id="chkSelectAllNetWorth" class="networth" type="checkbox" /><strong><span id="selectAllNetWorthLabel">Select All</span></strong>
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth1" class="networth" onclick="NetWorthSelected();" value="1" type="checkbox" /> Less than or equal to $0
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth2" class="networth" onclick="NetWorthSelected();" value="2" type="checkbox" /> $1 - $4,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth3" class="networth" onclick="NetWorthSelected();" value="3" type="checkbox" /> $5,000 - $9,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth4" class="networth" onclick="NetWorthSelected();" value="4" type="checkbox" /> $10,000 - $24,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth5" class="networth" onclick="NetWorthSelected();" value="5" type="checkbox" /> $25,000 - $49,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth6" class="networth" onclick="NetWorthSelected();" value="6" type="checkbox" /> $50,000 - $99,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth7" class="networth" onclick="NetWorthSelected();" value="7" type="checkbox" /> $100,000 - $249,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth8" class="networth" onclick="NetWorthSelected();" value="8" type="checkbox" /> $250,000 - $499,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth9" class="networth" onclick="NetWorthSelected();" value="9" type="checkbox" /> $500,000 - $999,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth10" class="networth" onclick="NetWorthSelected();" value="A" type="checkbox" /> $1,000,000 - $1,999,999
                                                        </label>
                                                    </div>

                                                    <div>
                                                        <label>
                                                            <input id="chkNetWorth11" class="networth" onclick="NetWorthSelected();" value="B" type="checkbox" /> $2,000,000 +
                                                        </label>
                                                    </div>

                                                </div>

                                            </div>

                                        </div>

                                    </div>

                                </div>

                                <%--Ethnicity--%>
                                <div class="panel panel-info" id="ethnicityWrapper">
                                    <div class="panel-heading" role="tab" id="ethnicityHeaderBlock">
                                        
                                        <p class="panel-title">
                                        
                                            <a id="ethnicitySelect" role="button" data-toggle="collapse" data-action="rotateIcon" data-parent="#filterControlsBlock" href="#ethnicityControlBlock" aria-expanded="true" aria-controls="ethnicityControlBlock">
                                                <span id="ethnicityIcon" class="fa fa-plus fa-border"></span>&nbsp;Ethnicity&nbsp;<span class="categoryLabel pull-right" id="ethnicityLabel">No Filter</span>
                                            </a>

                                        </p>

                                    </div>
                            
                                    <div id="ethnicityControlBlock" class="panel-collapse collapse" role="tabpanel" aria-labelledby="ethnicityHeaderBlock">

                                        <div class="panel-body ui-controls">

                                            <button onclick="EthnicitySelected('btnAfricanAmerican');" type="button" class="btn btn-sm btn-block" id="btnAfricanAmerican">African American</button>
                                            <button onclick="EthnicitySelected('btnArab');" type="button" class="btn btn-sm btn-block" id="btnArab">Arab</button>
                                            <button onclick="EthnicitySelected('btnAsian');" type="button" class="btn btn-sm btn-block" id="btnAsian">Asian</button>
                                            <button onclick="EthnicitySelected('btnAsianNonOriental');" type="button" class="btn btn-sm btn-block" id="btnAsianNonOriental">Asian (Non Oriental)</button>
                                            <button onclick="EthnicitySelected('btnFrench');" type="button" class="btn btn-sm btn-block" id="btnFrench">French</button>
                                            <button onclick="EthnicitySelected('btnGerman');" type="button" class="btn btn-sm btn-block" id="btnGerman">German</button>
                                            <button onclick="EthnicitySelected('btnHispanic');" type="button" class="btn btn-sm btn-block" id="btnHispanic">Hispanic</button>
                                            <button onclick="EthnicitySelected('btnItalian');" type="button" class="btn btn-sm btn-block" id="btnItalian">Italian</button>
                                            <button onclick="EthnicitySelected('btnJewish');" type="button" class="btn btn-sm btn-block" id="btnJewish">Jewish</button>
                                            <button onclick="EthnicitySelected('btnMiscellaneous');" type="button" class="btn btn-sm btn-block" id="btnMiscellaneous">Miscellaneous</button>
                                            <button onclick="EthnicitySelected('btnNorthernEuropean');" type="button" class="btn btn-sm btn-block" id="btnNorthernEuropean">Northern European</button>
                                            <button onclick="EthnicitySelected('btnPolynesian');" type="button" class="btn btn-sm btn-block" id="btnPolynesian">Polynesian</button>
                                            <button onclick="EthnicitySelected('btnScottishIrish');" type="button" class="btn btn-sm btn-block" id="btnScottishIrish">Scottish/Irish</button>
                                            <button onclick="EthnicitySelected('btnSouthernEuropean');" type="button" class="btn btn-sm btn-block" id="btnSouthernEuropean">Southern European</button>
                                            <button onclick="EthnicitySelected('btnUnclassified');" type="button" class="btn btn-sm btn-block" id="btnUnclassified">Unclassified</button>

                                        </div>

                                    </div>

                                </div>

                            </div>


                            <%--Change / Reset--%>
                            <div>
                                <button id="changeTarget" class="btn btn-cta btn-xs pull-left">
                                    <span class="fa fa-filter"></span>&nbsp;Change Target Area
                                </button>

                                <button id="resetFilters" class="btn btn-cta btn-xs pull-right">
                                    <span class="fa fa-refresh"></span>&nbsp;Reset filters
                                </button>
                            </div>

                            <p>&nbsp;</p>
                            <p>&nbsp;</p>

                            <%--Help--%>
                            <div class="panel panel-info">
                                <div class="panel-heading">Need Help?</div>
                                <div class="panel-body">

                                    <p>Have questions about how to use this?  Questions about the 
                                    results?  No worries - please feel free to ask for help!</p>

                                    <p><span class="fa fa-phone"></span>&nbsp;<asp:Literal ID="litPhone" runat="server" /></p>
                                    <p><appx:BoldChatTextLink runat="server" id="BoldChatTextLink" /></p>
                                    <p><asp:HyperLink ID="hypEmail" runat="server"></asp:HyperLink></p>

                                </div>
                            </div>

                        </div>

                        <div>&nbsp;</div>

                    </div>

                </div>

            </div>

            <div class="row">
                <div class="col-sm-12">
                    <appx:BannerExpertTeams runat="server" id="BannerExpertTeams" />
                </div>
            </div>

            <p>&nbsp;</p>

        </div>

    </div>

</asp:content>



<asp:content id="Content3" contentplaceholderid="cpScripts" runat="Server">


    <script src="/assets/javascripts/Step1-BuildYourList.min.js?ver=1.1.0" type="text/javascript"></script>
    <%--<script src="/assets/javascripts/Step1-BuildYourList.js" type="text/javascript"></script>--%>

    
    <script type="text/javascript">

        $("#filtersBlock").hide();
        $("#resultsBlock").hide();
        $("#zipCodesBlock").hide();
        $("#addressBlock").hide();
        $("#defineYourAreaToBegin").hide();

    </script>

</asp:content>

