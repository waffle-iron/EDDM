<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="true" CodeFile="old-TargetDataMap3.aspx.cs" Inherits="TargetDataMap3" Trace="false" %>
<%@ Register Src="~/CCustom/OLBTargetMap.ascx" TagPrefix="eddm" TagName="OLBTargetMap" %>
<%@ Register Src="~/Controls/TemplateBrowser.ascx" TagPrefix="eddm" TagName="TemplateBrowser" %>


<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

<div class="container">

    <%--Developer Data--%>
    <asp:Panel ID="pnlDevData" runat="server" visible="false">
            
        <div class="row">
            <h3>Developer Data</h3>
        </div>

       
        <div>
            <asp:Panel ID="pnlDebug" runat="server" CssClass="alert alert-danger" ClientIDMode="Static">
                <p><strong>Debug Panel:</strong></p>
                <asp:Literal ID="litPageProps" runat="server" />
                <asp:Literal ID="litSessionVars" runat="server" />
                <span id="debugMsg" />
            </asp:Panel>
            
        </div>
    </asp:Panel>


    <div class="partialRibbon hidden-sm hidden-xs">
        <span class="arrowLeft"></span>
        <span class="subRibbonPop">OLB</span>
        <span class="subRibbon">Your Targeted Customers</span>
    </div>


    <div class="partialRibbonSmall visible-sm visible-xs">
        <span class="subRibbonPopSmall">OLB</span>
        <span class="subRibbonSmall">Your Targeted Customers</span>
    </div>
    

    <%--Normal display--%>
    <asp:Panel ID="pnlNormal" runat="server" Visible="True">

        <div class="contentWrapper">

        <p class="lead">Welcome to the OLB <em>Customer Targeter</em>. Attracting new customers is easier than
        ever! Just follow the three simple steps below to grow your franchise!</p>

        <p>&nbsp;</p>
        
        <div class="row">

            <%--Step 3--%>
            <div class="col-sm-6">

                <asp:Panel ID="pnlStep3" runat="server">

                    <%--Step 3 Form--%>
                    <section id="step3Section">
                        <div class="panel panel-primary" id="step2Panel">
                            <div class="panel-heading">
                                <h3 class="panel-title">Step 3 - Select Your Template</h3>
                            </div>
                    
                            <div class="panel-body">
                                <p>Now that you have defined your campaign, all you need to do is select a Template (which we will customize
                                for <strong>free)</strong>.</p>

                                <div class="text-center">
                                    <asp:LinkButton ID="btnShowTemplates" CssClass="btn btn-danger btn-lg" runat="server" ClientIDMode="Static">
                                    <span class="glyphicon glyphicon-picture"></span>&nbsp;Choose A Template
                                    </asp:LinkButton>
                                </div>

                                <eddm:TemplateBrowser runat="server" id="TemplateBrowser" />
                                
                                
                                <%--required hidden fields--%>
                                <asp:HiddenField ID="hidSelectedTemplateID" runat="server" Value="0" ClientIDMode="Static" />
                                <asp:TextBox ID="txtSelectedTemplateID" runat="server" ClientIDMode="Static" CssClass="hidden" Visible="true" />
                                <asp:HiddenField ID="hidSelectedTemplateFileName" runat="server" Value="0" ClientIDMode="Static" />
                                <asp:TextBox ID="txtSelectedTemplateFileName" runat="server" ClientIDMode="Static" CssClass="hidden" Visible="true" />
                                <asp:HiddenField ID="hidSelectedTemplateName" runat="server" Value="0" ClientIDMode="Static" />
                                <asp:TextBox ID="txtSelectedTemplateName" runat="server" ClientIDMode="Static" CssClass="hidden" Visible="true" />


                                <asp:Panel ID="pnlTemplateSelected" runat="server">

                                    <div>&nbsp;</div>

                                    <div class="row">
                                        <div class="col-md-7">
                                            
                                            <div class="alert alert-info" id="notSelectedTemplateBlock">
                                                <span class="glyphicon glyphicon-info-sign"></span>&nbsp;Select a Template to complete this step. Click the <em>Choose a Template</em> button.
                                            </div>

                                            <div class="hidden" id="goodChoiceFreeDesignBlock">
                                                <span class="glyphicon glyphicon-ok"></span>&nbsp;
                                                <strong>Good Choice!</strong><br />
                                                <asp:Label ID="lblYouHaveSelected" runat="server" ClientIDMode="Static" /><br />
                                                Our designers will work with you to customize the template you select
                                                at no additional charge. We'll be in touch within one to two business days
                                                of placing your order to begin customization of your template.
                                            </div>

                                        </div>
                                        <div class="col-md-5">
                                            <asp:Image ID="imgSelectedTemplate" runat="server" ImageUrl="~/assets/images/your-template.png" CssClass="img-thumbnail templateThumbnail" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </asp:Panel>

                                <div>&nbsp;</div>
                            </div>
                        </div>
                    </section>

                </asp:Panel>

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
                            <td class="col-sm-6"><asp:Label ID="lblFranchise" runat="server" Text="" ClientIDMode="Static" /></td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Location</td>
                            <td class="col-sm-6"><asp:Label ID="lblLocation" runat="server" Text="" ClientIDMode="Static" /></td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Selected</td>
                            <td class="col-sm-6"><asp:Label ID="lblSelected" runat="server" Text="" ClientIDMode="Static" /></td>
                        </tr>

                        <tr>
                            <td class="col-sm-6">Mail Drops</td>
                            <td class="col-sm-6"><asp:Label ID="lblImpressions" runat="server" Text="" ClientIDMode="Static" /></td>
                        </tr>

                        <tr runat="server" id="frequencyRow">
                            <td class="col-sm-6">Frequency</td>
                            <td class="col-sm-6"><asp:Label ID="lblFrequency" runat="server" Text="" ClientIDMode="Static" /></td>
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
                            <td class="col-sm-6">
                                <asp:Label ID="lblLaunchWeek" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>
                        </table>

                        <div class="row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnBackToStep2" runat="server" CssClass="btn btn-primary btn-lg pull-left" ClientIDMode="Static" OnClick="btnBackToStep2_Click">
                                    <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;Back To Step 2
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnContinue" runat="server" CssClass="btn btn-default btn-lg pull-right disabled" ClientIDMode="Static" OnClick="btnContinue_Click">
                                    Continue and Checkout&nbsp;<span class="glyphicon glyphicon-shopping-cart"></span>
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

            </div>

        </div>

        <div>&nbsp;</div>
        
    </div>

    </asp:Panel>
    
    
    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">

        <div class="contentWrapper">
            <i class="fa fa-2x fa-exclamation-circle pull-left"></i>&nbsp;
            <asp:Literal ID="litErrorMessage" runat="server" />
            <p>&nbsp;</p>
        </div>

    </asp:Panel>


</div>

</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">

    <script src="../assets/javascripts/OLB-Targeter.min.js?ver=1.0"></script>
    
    <script type="text/javascript">

        $('a[data-action=selectTemplate]').click(function (e) {
            //No postback
            e.preventDefault();

            //grab the TemplateID, file name, and template name
            var templateID = $(this).attr('data-templateID');
            var fileName = $(this).attr('data-thumbnail');
            var templateName = $(this).attr('data-templateName');

            //hidden field
            $('#hidSelectedTemplate').val(templateID);

            //change the thumbnail
            $('#imgSelectedTemplate').attr('src', fileName);

            //change the description
            $('#litTemplateMsg').html('Good choice!  You have selected <strong>' + templateName + ' (#' + templateID + ') </strong>. Now you are ready for the last step.');

            //hide the template modal
            $('#modalTemplates').modal('hide');

            //remove all the css classes (mainly disabled) from the button and readd add the correct ones.
            $('#btnCheckOut').removeAttr('class');
            $('#btnCheckOut').attr('class', 'btn btn-danger pull-right');

            //turn the Show Templates from Red to Blue
            $('#btnShowTemplates').removeAttr('class');
            $('#btnShowTemplates').attr('class', 'btn btn-primary btn-lg');


            //$('#progressBar').removeAttr('style');
            $('#progressBar').attr('aria-valuenow', '100');
            $('#progressBar').attr('style', 'width: 100%;');
            $('#progressVal').html('100%');

        });


    </script>
    
</asp:Content>






