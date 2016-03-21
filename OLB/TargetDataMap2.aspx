<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="true" CodeFile="TargetDataMap2.aspx.cs" Inherits="TargetDataMap2" Trace="False" %>
<%@ Register Src="~/CCustom/OLBTargetMap.ascx" TagPrefix="eddm" TagName="OLBTargetMap" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="appx" TagName="PageHeader" %>


<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

<div class="container">

    <appx:PageHeader runat="server" id="PageHeader" />

    <div class="contentWrapper">

        <p class="lead">Welcome to the OLB <em>Customer Targeter</em>. Attracting new customers is easier than
        ever! Just follow the three simple steps below to grow your franchise!</p>

        <p>&nbsp;</p>
        
        <div class="row">

            <div class="col-sm-6">

                <%--Step 2--%>
                <asp:Panel ID="pnlStep2" runat="server">

                    <%--Step 2 Form--%>
                    <section id="step2Section">
                        <div class="panel panel-primary" id="step2Panel">
                            <div class="panel-heading">
                                <h3 class="panel-title">Step 2 - View Your Routes</h3>
                            </div>
                    
                            <div class="panel-body">

                                <p>Below is your very own custom map targeting your ideal customers based on the criteria you defined. Click Modify Suggested Routes
                                if you wish to make changes or simply go on to <strong>Step 3</strong>.</p>

                                <p class="text-center"><asp:Image ID="imgmap" runat="server" CssClass="img-responsive img-thumbnail" ImageUrl="~/assets/images/map-placeholder.jpg" /></p>

                                <br />

                                <div class="text-center">
                                    <asp:LinkButton ID="btnShowMeMap" CssClass="btn btn-lg btn-primary" runat="server" ClientIDMode="Static" data-toggle="modal" data-target="#modalMap">
                                        <span class="glyphicon glyphicon-map-marker"></span>&nbsp;Modify Suggested Routes
                                    </asp:LinkButton>
                                </div>

                                <div>&nbsp;</div>

                                <p>Your Map has been given a systematic name which is shown below.  <mark>Please feel free to rename it to your liking.</mark></p>

                                <div class="form-group">
                                    <label for="txtNameOfMap" class="label formLabelRequired">Map Name</label>
                                    <asp:TextBox CssClass="form-control input-sm" runat="server" ID="txtNameOfMap" />
                                    <asp:RequiredFieldValidator ID="rfvMapName" ControlToValidate="txtNameOfMap" runat="server" ErrorMessage="A Map Name is required." Display="dynamic" CssClass="label label-danger" SetFocusOnError="True">
                                        A Map Name is required.
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>
                        </div>
                    </section>

                    <%--Loading Section--%>
                    <asp:Panel ID="pnlLoadingSection" runat="server">
                        <section id="step2LoadingSection">
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

                <br />
              
            </div>
            
            <div class="col-sm-6">

                <%--Campaign Progress--%>
                <div class="panel panel-primary" id="campaignProgress">
                    <div class="panel-heading">
                        <h3 class="panel-title">Campaign Progress for <asp:Label ID="lblUserName" runat="server" /></h3>
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
                            <td class="col-sm-6">
                                <asp:Label ID="lblFranchise" runat="server" Text="" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Location</td>
                            <td class="col-sm-6">
                                <asp:Label ID="lblLocation" runat="server" Text="" ClientIDMode="Static" />
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
                            <td class="col-sm-6">
                                <asp:Label ID="lblImpressions" runat="server" Text="" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <tr runat="server" id="frequencyRow">
                            <td class="col-sm-6">Frequency</td>
                            <td class="col-sm-6">
                                <asp:Label ID="lblFrequency" runat="server" Text="" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Ad Template</td>
                            <td class="col-sm-6">
                                <asp:Label ID="lblTemplate" runat="server" Text="" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <%--This is hidden if user modifies the preloaded routes.--%>
                        <tr runat="server" id="rowAvgMatch">
                            <td class="col-sm-6">Average Match to OLB Profile</td>
                            <td class="col-sm-6">
                                <asp:Label ID="lblPctMatchAvg" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Estimated Cost</td>
                            <td class="col-sm-6">
                                $<asp:Label ID="lblAmount" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Total Mailed</td>
                            <td class="col-sm-6">
                                <asp:Label ID="lblTotalMailed" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Price Per Piece</td>
                            <td class="col-sm-6">
                                $<asp:Label ID="lblPricePerPiece" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Launch Week</td>
                            <td class="col-sm-6">
                                <asp:Label ID="lblLaunchWeek" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>
                        </table>

                        <p>Once you have selected your routes, you will select a Ad Template for your Campaign on the next step.</p>

                        <div class="row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnbackToStep1" runat="server" CssClass="btn btn-lg btn-primary pull-left" OnClick="btnbackToStep1_Click">
                                    <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;Back To Step 1
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnGoToStep3" runat="server" CssClass="btn btn-lg btn-danger pull-right" OnClick="btnGoToStep3_Click">
                                    Go To Step 3&nbsp;<span class="glyphicon glyphicon-chevron-right"></span>
                                </asp:LinkButton>
                                 
                            </div>
                        </div>

                        <div>&nbsp;</div>

                        <div class="row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnStartOver" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnStartOver_Click">
                                    <span class="glyphicon glyphicon-step-backward"></span>&nbsp;Start Over
                                </asp:LinkButton>
                            </div>
                        </div>

                    </div>

                </div>


                <%--Hidden fields--%>
                <asp:Panel ID="pnlHiddenFields" runat="server">
                    <asp:HiddenField ID="hidSelected" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hidPricePerPiece" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hidTotalMailed" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hidAmount" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hidPctMatchAvg" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hidTemplate" runat="server" ClientIDMode="Static" />
                </asp:Panel>

            </div>

        </div>

        <div>&nbsp;</div>

        <%--Developer Data--%>
        <asp:Panel ID="pnlDevData" runat="server" CssClass="hidden">
            
            <div class="row">
            
                <h3>Developer Data</h3>
                <asp:TextBox ID="txtTotalResidencesFromMap" runat="server" />
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
        
            <div>
                <asp:Panel ID="pnlDebug" runat="server" CssClass="alert alert-danger" ClientIDMode="Static" Visible="True">
                    <p><strong>Debug Panel:</strong></p>
                    <p><asp:Literal ID="litSessionVariables" runat="server" /></p>
                    <span id="debugMsg" />
                </asp:Panel>
            
                <asp:Panel ID="pnlError" runat="server" Visible="true">
                    <div class="alert alert-danger" role="alert">
                        <asp:Label ID="lblError" runat="server" Text="" />
                    </div>
                </asp:Panel>
            </div>
        </asp:Panel>

        
    <%--End of ContentWrapper--%>
    </div>

    <%--Map Modal--%>
    <div class="modal fade" id="modalMap" tabindex="-1" role="dialog" aria-labelledby="modalMapLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="btn btn-danger btn-sm pull-right" data-dismiss="modal" aria-label="Close">
                        <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;Cancel</button>
                    <h4 class="modal-title" id="modalMapLabel">Your Targeted Routes</h4>
                </div>
        
                <div class="modal-body">

                    <eddm:OLBTargetMap runat="server" id="OLBTargetMap" />

                </div>

            </div>
        </div>
    </div>


<%--End of Container--%>
</div>

</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">

    <script src="../assets/javascripts/olb.min.js"></script>
    <script src="../assets/javascripts/TargetDataMap2.min.js"></script>

</asp:Content>

