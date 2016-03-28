<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="Step1-UploadYourList2.aspx.vb" Inherits="UploadYourList2" %>
<%@ Register Src="~/Controls/OrderSteps.ascx" TagPrefix="appx" TagName="OrderSteps" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="appx" TagName="PageHeader" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phHead" runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="phNav" runat="Server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="phBody" runat="Server">

    <div class="container">
        
        <appx:PageHeader runat="server" id="PageHeader" />
        
        <div class="contentWrapper">

            <appx:OrderSteps runat="server" id="OrderSteps" />

            <div class="row">

                <%--Step 1--%>
                <asp:Panel runat="server" ID="uploadStep1" ClientIDMode="Static">

                    <div class="col-sm-6 col-sm-offset-2">

                        <div class="well well-sm">

                            <h4>Step 1 - Upload Your File&nbsp;<span class="fa fa-cloud-upload"></span></h4>

                            <p>It's easy! Choose any Excel (xls, xlsx), csv, or delimited text file containing your addressed list and add it to the box below. 
                            You can drag and drop the file into the box below or just click it to browse the files on your computer.</p>

                            <div id="mydropzone" class="dropzone"></div>

                            <p class="text-center">Once you upload your custom list, we will validate it.</p>

                            <div>&nbsp;</div>

                        </div>

                    </div>

                    <div class="col-sm-4">

                        <div class="info-board info-board-theme-primary" id="fileRequirements">

                            <h4>File Requirements</h4>

                            <ul>
                                <li>Excel, csv, or txt files are accepted</li>
                                <li>Text and CSV files must be comma delimited</li>
                                <li>File size cannot exceed 4MB</li>
                                <li><a href="/downloads/Addressed-List-Upload-Sample.xlsx" target="_blank">Download a sample Excel (.xlsx) file.</a></li>
                            </ul>
                
                        </div>

                    </div>

                </asp:Panel>

                <%--Step 2--%>
                <asp:Panel runat="server" ID="identifyStep2" ClientIDMode="Static">

                    <div class="col-sm-12">
                            
                        <div class="well well-sm">

                            <h4>Step 2 - Identify Your Fields&nbsp;<span class="fa fa-pencil"></span></h4>

                            <p><span class="leadDropWord">Great!</span> Now identify each column for the file you uploaded. Your file must contain these required columns: First Name, 
                            Last Name, Address, City, State, and Zip Code.  Additional columns such as Address 2 and Company Name are optional.</p> 
                            
                            <p>Shown below is a <mark>preview</mark> of your data file which contains the <mark>first <strong>10</strong> rows</mark> of data.</p>
                            
                            <div id="myIdentify">
 
                                <div class="checkbox">
                                    <label class="control-label">
                                        <asp:CheckBox runat="server" ID="chkFirstContainsFieldNames" ClientIDMode="Static" /><strong>First Row Contains Field Names</strong>
                                    </label>
                                </div>

                                <%--Output table--%>
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped table-condensed" id="uploadedListTable">
                                        <thead>
                                            <tr></tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>

                            </div>

                            <p>&nbsp;</p>

                            <div class="row">

                                <div class="col-sm-6 text-left">
                                    <a id="btnStep1" class="btn btn-danger btn-lg"><span class="fa fa-fw fa-chevron-left"></span>&nbsp;Back to Step 1</a>
                                </div>


                                <div class="col-sm-6 text-right">
                                    <a id="btnStep3" class="btn btn-danger btn-lg">Go to Step 3&nbsp;<span class="fa fa-fw fa-chevron-right"></span></a>
                                </div>

                            </div>

                            <p>&nbsp;</p>

                        </div>

                    </div>

                </asp:Panel>

                <%--Step 3--%>
                <asp:Panel runat="server" ID="continueStep3" ClientIDMode="Static">
                        
                    <div class="col-sm-12">

                        <%--Loading panel--%>
                        <div id="processingFile">

                            <div class="row">

                                <div class="col-sm-6 col-sm-offset-3">

                                    <div class="well well-sm">
                                        <div class="text-center">
                                            
                                            <p>&nbsp;</p>

                                            <h5><span class="fa fa-2x fa-cog fa-spin"></span>&nbsp;Processing your list...</h5>

                                            <p class="text-danger"><small>(This could take several minutes.)</small></p>

                                            <p>&nbsp;</p>

                                            <p>&nbsp;</p>
                                            
                                        </div>
                                    </div>

                                </div>

                            </div>

                        </div>

                        <%--Results panel--%>
                        <div id="uploadResponse">

                            <div class="row">

                                <div class="col-sm-4">

                                    <div class="well well-sm">

                                        <h5 class="text-center"><span class="fa fa-map-marker"></span>&nbsp;Original Addresses</h5>
                                        
                                        <p class="lead text-center" id="oAddressCount"></p>

                                        <p><small>This is how many addresses we detected in your file.</small></p>

                                    </div>

                                </div>

                                <div class="col-sm-4">

                                    <div class="well well-sm">

                                        <h5 class="text-center"><span class="fa fa-close"></span>&nbsp;Invalid Addresses</h5>
                                        
                                        <p class="lead text-center" id="oInvalidCount"></p>

                                        <p><small>This is how many addresses that were incomplete or could not be used.</small></p>

                                    </div>

                                </div>

                                <div class="col-sm-4">

                                    <div class="well well-sm">

                                        <h5 class="text-center"><span class="fa fa-check"></span>&nbsp;Valid Addresses</h5>

                                        <p class="lead text-center" id="oValidCount"></p>

                                        <p><small>This is your final count of addresses to mail to.</small></p>

                                    </div>

                                </div>

                            </div>

                            <%--Name your list--%>

                            <p>&nbsp;</p>

                            <div class="row">
                                <div class="col-sm-6 col-sm-offset-3">

                                    <p>When you are finished, please provide a <strong>name</strong> for your custom uploaded list and click Continue.</p>

                                    <asp:HiddenField runat="server" ID="hfListCount" ClientIDMode="Static" />

                                    <div class="form-group">
                                        
                                        <label for="txtListName" class="label formLabelRequired">List Name</label>
                                        <asp:TextBox ID="txtListName" runat="server" CssClass="form-control" MaxLength="100" />
                                        <asp:RequiredFieldValidator ID="rfvListName" ControlToValidate="txtListName" runat="server" ErrorMessage="List Name is a required field." Display="dynamic" CssClass="label label-danger" SetFocusOnError="True">
                                            List Name is  required.
                                        </asp:RequiredFieldValidator>

                                    </div>

                                    <p>&nbsp;</p>

                                    <asp:LinkButton ID="btnContinue" runat="server" CssClass="btn btn-danger lrgActionButton btn-lg btn-block" OnClick="btnContinue_Click">
                                        Continue&nbsp;<span class="fa fa-chevron-right"></span>
                                    </asp:LinkButton>

                                    <p class="text-center"><a href="Step1-UploadYourList.aspx" title="Start over">Start over?</a></p>

                                </div>
                            </div>


                        </div>


                    </div>

                </asp:Panel>

                <%--Error Panel--%>
                <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
                    <p><span class="fa fa-2x fa-exclamation-circle"></span>&nbsp;
                    <asp:Literal ID="litErrorMessage" runat="server" /></p>
                    <p>&nbsp;</p>
                </asp:Panel>


                <%--Error Message--%>
                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div id="warningMessageAlert" class="alert alert-danger">
                            <p><span class="fa fa-warning"></span>&nbsp;<span id="warningMessage"></span></p>
                        </div>
                    </div>
                </div>


            </div>


            <%--Debug Data--%>
            <asp:Panel ID="pnlDebug" ClientIDMode="static" runat="server" CssClass="hidden">

                <div class="row">
                    <div class="col-sm-12">
                        <div class="alert alert-info">
        
                            <h5>Debug Data</h5>

                            <div class="alert alert-warning">
                                <asp:Literal ID="litSysMessage" runat="server" />
                            </div>

                        </div>
                    </div>
                </div>

            </asp:Panel>



        </div>

    </div>

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="phForm" runat="Server">
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="phFoot" runat="Server">
</asp:Content>


<asp:Content ID="Content7" ContentPlaceHolderID="cpScripts" runat="Server">
    
   
    <script type="text/javascript" src="../assets/javascripts/dropzone.js"></script>
    <script type="text/javascript" src="../assets/javascripts/Step1-UploadYourList2.js"></script>

    <script type="text/javascript">

        $("#identifyStep2").hide();
        $("#continueStep3").hide();
        $("#uploadResponse").hide();
        $("#warningMessageAlert").hide();
        $("#dialog").hide();
    </script>


</asp:Content>

