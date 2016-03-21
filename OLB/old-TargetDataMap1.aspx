<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="true" CodeFile="old-TargetDataMap1.aspx.cs" Inherits="TargetDataMap1" Trace="False" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="appx" TagName="PageHeader" %>




<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

    <div class="container">

        <appx:PageHeader runat="server" id="PageHeader" />
        
        <div class="contentWrapper">

            <%--Debug--%>
            <asp:Panel ID="pnlDebug" runat="server" CssClass="alert alert-danger" ClientIDMode="Static" Visible="false">
                <p><strong>Debug Panel:</strong></p>
                <asp:Literal ID="litDebug" runat="server" Text="" /><br />
                <p><strong>Page Properties:</strong></p>
                <asp:Literal ID="litPageProps" runat="server" Text="" /><br />
                <span id="debugMsg" />
            </asp:Panel>


            <%--Developer Data--%>
            <asp:Panel ID="pnlDevData" runat="server" CssClass="hidden" ClientIDMode="Static">
            
                <div class="row">
            
                    <h3>Developer Data</h3>
                    
                    <input type="button" value="Get PPP Test" onclick="PPPTester();" /><br />
                    <input type="button" value="TEST" onclick="Calculate();" /><br />
                    <table class="table table-bordered table-striped table-condensed table-hover" id="tblPricePerPiece">
                    <caption>Price Per Piece</caption>
                    <thead>
                    <tr>
                        <th>Quantity</th>
                        <th>Price Per Piece</th>
                    </tr>
                    </thead>

                    <tbody>
                    <tr>
                        <td class="width50">1,000</td>
                        <td class="width50">$<asp:Label ID="lblPriceAt1000" runat="server" Text="" ClientIDMode="Static" /></td>
                    </tr>

                    <tr>
                        <td class="width50">2,500</td>
                        <td class="width50">$<asp:Label ID="lblPriceAt2500" runat="server" Text="" ClientIDMode="Static" /></td>
                    </tr>

                    <tr>
                        <td class="width50">5,000</td>
                        <td class="width50">$<asp:Label ID="lblPriceAt5000" runat="server" Text="" ClientIDMode="Static" /></td>
                    </tr>

                    <tr>
                        <td class="width50">10,000</td>
                        <td class="width50">$<asp:Label ID="lblPriceAt10000" runat="server" Text="" ClientIDMode="Static" /></td>
                    </tr>

                    <tr>
                        <td class="width50">25,000</td>
                        <td class="width50">$<asp:Label ID="lblPriceAt25000" runat="server" Text="" ClientIDMode="Static" /></td>
                    </tr>

                    <tr>
                        <td class="width50">50,000</td>
                        <td class="width50">$<asp:Label ID="lblPriceAt50000" runat="server" Text="" ClientIDMode="Static" /></td>
                    </tr>
                    </tbody>
                    </table>       
            
                    <p>Suggested Routes</p>
                    <div class="form-group">
                        <label for="cblRoutes" class="label label-primary">Suggested Routes</label>
                        <asp:Repeater ID="rptRoutes" runat="server">

                            <HeaderTemplate>
                                <table class="table table-bordered table-condensed table-striped table-hover" id="tblRoutes">
                                <thead>
                                    <tr>
                                    <th>Included</th>
                                    <th>Rank</th>
                                    <th>Zip Code</th>
                                    <th>Route</th>
								    <th>Zip Route</th>
                                    <th>% Match</th>
                                    <th>Residences</th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>

                            <ItemTemplate>

                                <tr>
                                <td>
                                    <div class="checkbox text-center CheckboxTight">
                                        <label>
                                            <asp:CheckBox ID="chkInclude" runat="server" Checked="false" />
                                        </label>
                                    </div>
                                </td>
                                <td><%# DataBinder.Eval(Container.DataItem, "Rank") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "ZipCode") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "CarrierRoute") %></td>
                                <td><asp:Label ID="lblZipRoute" runat="server" Text='<%# String.Concat(DataBinder.Eval(Container.DataItem, "ZipCode"), DataBinder.Eval(Container.DataItem, "CarrierRoute")) %>' ClientIDMode="Static" /></td>
                                <td><asp:Label ID="lblPctMatch" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TargetPct2","{0:P2}") %>' ClientIDMode="Static" /></td>
                                <td><asp:Label ID="lblResTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ResidentialTotal") %>' ClientIDMode="Static" /></td>
                                </tr>
                            </ItemTemplate>

                            <FooterTemplate>
                                </table>
                            </FooterTemplate>

                        </asp:Repeater>

                    </div>
         
                </div>

                <div>&nbsp;</div>
        
            </asp:Panel>


            <%--Error Panel--%>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="alert alert-danger" role="alert">
                    <asp:Label ID="lblError" runat="server" Text="" />
                </div>
                <asp:GridView ID="gvDebug" runat="server"></asp:GridView>
            </asp:Panel>


            <p class="lead">Welcome to the OLB <em>Customer Targeter</em>. Attracting new customers is easier than
            ever! Just follow the three simple steps below to grow your franchise!</p>

            <div class="row">

                <%--Define your campaign--%>
                <div class="col-sm-6">

                    <%--Step 1--%>
                    <asp:Panel ID="pnlStep1" runat="server">

                        <%--Step 1 Form--%>
                        <section id="step1Section">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Step 1 - Define Your Campaign</h3>
                                </div>
                    
                                <div class="panel-body">
                                    <p>First, define the criteria of your Campaign. Pick a Template, Number of Mailings,
                                    and your Campaign Target.</p>

                                    <%--Franchise--%>
                                    <div class="form-group" id="brandsGroup">
                                        <label for="ddlOLBBrands" class="label formLabelRequired">Franchise Brand</label>
                                        <asp:DropDownList ID="ddlOLBBrands" CssClass="form-control input-sm" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlOLBBrands_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>

                                    <%--Location--%>
                                    <div class="form-group">
                                        <label for="ddlOLBTerritories" class="label formLabelRequired">Location (Load the data for this franchise)</label>
                                        <asp:DropDownList ID="ddlOLBTerritories" CssClass="form-control input-sm" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlOLBTerritories_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>

                                    <%--Templates--%>
                                    <div class="form-group">
                                        <label for="ddlOLBTemplates" class="label formLabelRequired">Ad Template</label>
                                        <asp:DropDownList ID="ddlOLBTemplates" CssClass="form-control input-sm" runat="server" ClientIDMode="Static" EnableViewState="true">
                                        </asp:DropDownList>
                                    </div>

                                    <%--First Week...--%>
                                    <div class="form-group">
                                        <label class="label formLabelRequired">Week of first mailing</label>
                                        <asp:DropDownList ID="ddlLaunchWeek" runat="server" CssClass="form-control input-sm" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>

                                    <%--Drops--%>
                                    <div class="form-group">
                                        <label for="ddlImpressions" class="label formLabelRequired">Number of Mail Drops</label>
                                        <asp:DropDownList ID="ddlImpressions" CssClass="form-control input-sm" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="1">1</asp:ListItem>
                                            <asp:ListItem Value="2">2</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="3">3 (OLB Recommends a minimum of 3 drops)</asp:ListItem>
                                            <asp:ListItem Value="4">4</asp:ListItem>
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="6">6</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <%--Frequency--%>
                                    <div class="form-group" id="frequencyGroup">
                                        <label for="ddlFrequency" class="label formLabelRequired">Frequency of mailings</label>
                                        <asp:DropDownList ID="ddlFrequency" CssClass="form-control input-sm" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="1">Every Week</asp:ListItem>
                                            <asp:ListItem Value="2">Every 2 Weeks</asp:ListItem>
                                            <asp:ListItem Value="3">Every 3 Weeks</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="4">Every 4 Weeks</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <%--Campaign Target--%>
                                    <div class="form-group">
                                        <label for="ddlTargetCampaign" class="label formLabelRequired">Campaign Target</label>
                                        <asp:DropDownList ID="ddlTargetCampaign" CssClass="form-control input-sm" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Selected="True" Value="OLB">OLB Recommends a minimum of 10,000 Residential Addresses</asp:ListItem>
                                            <asp:ListItem Value="budget">Calculate Based On Budget</asp:ListItem>
                                            <asp:ListItem  Value="numpieces">Calculate Based On Residential Addresses</asp:ListItem>
                                            <asp:ListItem  Value="savedmap">Use a Saved Map</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <%--Budget--%>
                                    <div class="form-group" id="budgetGroup">
                                        <label for="ddlBudget" class="label formLabelRequired">Budget</label>
                                        <asp:DropDownList ID="ddlBudget" CssClass="form-control input-sm" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="50000">$50,000</asp:ListItem>
                                            <asp:ListItem Value="40000">$40,000</asp:ListItem>
                                            <asp:ListItem Value="30000">$30,000</asp:ListItem>
                                            <asp:ListItem Value="20000">$20,000</asp:ListItem>
                                            <asp:ListItem Value="15000">$15,000</asp:ListItem>
                                            <asp:ListItem Value="10000">$10,000</asp:ListItem>
                                            <asp:ListItem Value="7500">$7,500</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="5000">$5,000</asp:ListItem>
                                            <asp:ListItem Value="4000">$4,000</asp:ListItem>
                                            <asp:ListItem Value="3000">$3,000</asp:ListItem>
                                            <asp:ListItem Value="2000">$2,000</asp:ListItem>
                                            <asp:ListItem Value="1000">$1,000</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <%--Num of pcs--%>
                                    <div class="form-group" id="numPiecesGroup">
                                        <label for="ddlNumPieces" class="label formLabelRequired">Residential Addresses</label>
                                        <asp:DropDownList ID="ddlNumPieces" CssClass="form-control input-sm" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="50000">50,000</asp:ListItem>                                                                                
                                            <asp:ListItem Value="25000">25,000</asp:ListItem>                                                                                                                        
                                            <asp:ListItem Value="20000">20,000</asp:ListItem>                                                                                
                                            <asp:ListItem Value="15000">15,000</asp:ListItem>                                        
                                            <asp:ListItem Value="10000">10,000</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="9000">9,000</asp:ListItem>
                                            <asp:ListItem Value="8000">8,000</asp:ListItem>
                                            <asp:ListItem Value="7000">7,000</asp:ListItem>
                                            <asp:ListItem Value="6000">6,000</asp:ListItem>
                                            <asp:ListItem Value="5000">5,000</asp:ListItem>
                                            <asp:ListItem Value="4000">4,000</asp:ListItem>
                                            <asp:ListItem Value="3000">3,000</asp:ListItem>
                                            <asp:ListItem Value="2000">2,000</asp:ListItem>
                                            <asp:ListItem Value="1000">1,000</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <%--Button--%>
                                    <div class="form-group" id="savedMapsGroup">
                                        <label for="ddlSavedMaps" class="label formLabelRequired">Saved Maps</label>
                                        <asp:DropDownList ID="ddlSavedMaps" CssClass="form-control input-sm" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlSavedMaps_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                            </div>
                        </section>
                    
                        <%--Loading Section--%>
                        <asp:Panel ID="pnlLoadingSection" runat="server">
                            <section id="step1LoadingSection">
                                <div class="panel panel-primary">
                                    <div class="panel-body">
                                        <p class="lead">Calculating data.....please wait....</p>
                                        <p class="text-center"><img src="../cmsimages/11/olb-loading.gif" /></p>
                                    </div>
                                </div>
                            </section>
                        </asp:Panel>

                        <div>&nbsp;</div>

                    </asp:Panel>

                    <%--Hidden fields--%>
                    <asp:Panel ID="pnlHiddenFields" runat="server" CssClass="hidden">
                        <asp:HiddenField ID="hidSelected" runat="server" ClientIDMode="Static" />
                        <asp:TextBox ID="txtSelected" runat="server" Visible="true" ClientIDMode="Static" />
                        <asp:HiddenField ID="hidBaseProductID" runat="server" ClientIDMode="Static" />
                        <asp:TextBox ID="txtBaseProductID" runat="server" Visible="true" ClientIDMode="Static" />
                    </asp:Panel>

                </div>
            
                <div class="col-sm-6">

                    <%--Campaign Progress--%>
                    <div class="panel panel-primary" id="campaignProgress">

                        <div class="panel-heading">
                            <h3 class="panel-title">Campaign Progress for <asp:Label ID="lblUserName" runat="server" Text="" /></h3>
                        </div>
                    
                        <div class="panel-body">
                            <div class="progress">
                                <asp:Literal ID="litProgressBar" runat="server" />
                            </div>
                        </div>

                    </div>

                    <div>&nbsp;</div>

                    <%--Campaign Overview--%>
                    <div class="panel panel-primary" id="campaignOverview">
                        <div class="panel-heading">
                            <h3 class="panel-title">Campaign Overview</h3>
                        </div>
                    
                        <div class="panel-body">
                            <p>This is your campaign so far. It will update as you go through the steps.</p>

                            <table class="table table-striped table-bordered table-hover table-condensed detailedData" id="overviewTable">

                            <thead>
                            <tr>
                                <th colspan="2">Campaign Statistics</th>
                            </tr>
                            </thead>    

                            <tr>
                                <td class="col-sm-6">Franchise</td>
                                <td class="col-sm-6"><asp:Label ID="lblFranchise" runat="server" Text="" ClientIDMode="Static" />
                                </td>
                            </tr>

                            <tr>
                                <td class="col-sm-6">Location</td>
                                <td class="col-sm-6"><asp:Label ID="lblLocation" runat="server" Text="" ClientIDMode="Static" />
                                </td>
                            </tr>

                            <tr>
                                <td class="col-sm-6">Selected</td>
                                <td class="col-sm-6">
                                    <asp:Label ID="lblSelected" runat="server" Text="" ClientIDMode="Static" />
                                </td>
                            </tr>

                            <tr>
                                <td class="col-sm-6">Mail Drops</td>
                                <td class="col-sm-6"><asp:Label ID="lblImpressions" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr id="frequencyRow">
                                <td class="col-sm-6">Frequency</td>
                                <td class="col-sm-6"><asp:Label ID="lblFrequency" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="col-sm-6">Ad Template</td>
                                <td class="col-sm-6"><asp:Label ID="lblTemplate" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr id="AverageMatchRow">
                                <td class="col-sm-6">Average Match to OLB Profile</td>
                                <td class="col-sm-6"><asp:Label ID="lblPctMatchAvg" runat="server" ClientIDMode="Static" />
                                    <asp:Label ID="lblPctMatchAvg2" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="col-sm-6">Estimated Cost</td>
                                <td class="col-sm-6">$<asp:Label ID="lblAmount" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="col-sm-6">Total Mailed</td>
                                <td class="col-sm-6"><asp:Label ID="lblTotalMailed" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="col-sm-6">Price Per Piece</td>
                                <td class="col-sm-6">$<asp:Label ID="lblPricePerPiece" runat="server" ClientIDMode="Static" /></td>
                            </tr>

                            <tr>
                                <td class="col-sm-6">Launch Week</td>
                                <td class="col-sm-6"><asp:Label ID="lblLaunchWeek" runat="server" ClientIDMode="Static" /></td>
                            </tr>
                            </table>

                            <p>Next, we will <strong>show you</strong> the recommended routes you should focus your campaign on based on your criteria. You will also be able to add and
                            and remove routes to your campaign.</p>

                            <p class="text-right">
                                <asp:LinkButton ID="btnGoToStep2" runat="server" CssClass="btn btn-lg btn-danger pull-right" OnClick="btnGoToStep2_Click">
                                    Go To Step 2&nbsp;<span class="glyphicon glyphicon-chevron-right"></span>
                                </asp:LinkButton>
                            </p>

                        </div>

                    </div>


                    <%--Minimum Warning--%>
                    <div class="alert alert-danger" id="underMinimumBlock">
                    
                        <p class="lead text-danger"><i class="fa fa-2x fa-exclamation-circle pull-left"></i>&nbsp;
                        Under The Minimum</p>

                        <p class="text-danger">We're sorry but this Map is under the 1,000 minimum quantity.  Please create a new 
                        map which meets the minimum criteria or pick a different Saved Map.</p>

                        <p>&nbsp;</p>

                    </div>
                                      
                </div>

            </div>

            <div>&nbsp;</div>


        <%--End of ContentWrapper--%>
        </div>

    </div>

</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">

    <!--<script src="../assets/javascripts/OLB-Targeter.min.js?ver=1.0"></script>-->
    <script src="../assets/javascripts/OLB-Targeter.js"></script>
        
    <script type="text/javascript">

        //Hide these initially
        $('#step1Section').hide();
        $('#campaignOverview').hide();
        $('#campaignProgress').hide();
        $('#underMinimumBlock').hide();


        //execute these
        SetBaseProductID();
        CheckForSavedMap();
        ShowHideOptions();


        //see what do to based on selected Territory (aka Locations)
        var territory = $('#ddlOLBTerritories :selected').val();

        if (territory != "n/a")
        {
            setTimeout(function ()
            {
                //Gives pricing grid time to finish loading (x seconds).
                $("#step1LoadingSection").hide();
                $('#step1Section').fadeIn(500);
                $("#step1Section").show();
                $('#campaignOverview').fadeIn(500);
                $("#campaignOverview").show();
                $('#campaignProgress').fadeIn(500);
                $("#campaignProgress").show();
                Calculate();
            },
            4000);
        }

        else
        {
            $("#step1LoadingSection").hide();
            $('#step1Section').fadeIn(500);
            $("#step1Section").show();
            $('#campaignProgress').fadeIn(500);
            $("#campaignProgress").show();
        }



        //look for selected Saved Map.
        var savedMap = $('#ddlSavedMaps :selected').val();


        if (savedMap != 0)
        {
            $('#AverageMatchRow').hide(750);
        }

        $("input[type=checkbox]").change(function ()
        { UpdateRouteTotals(); });

    </script>

</asp:Content>

