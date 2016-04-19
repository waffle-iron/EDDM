<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="account_selects.aspx.vb" Inherits="MyAccount_account_selects" %>
<%@ Register Src="~/CLibrary/YourAccountMenu.ascx" TagPrefix="appx" TagName="MemberSideBarRD" %>
<%@ Register Src="~/CCustom/PageHeader.ascx" TagPrefix="appx" TagName="PageHeader" %>



<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="phForm" Runat="Server">

    <section id="PageContent">
        <div class="container">
            
            <appx:PageHeader runat="server" id="PageHeader" />
            
            <div class="contentWrapper">
            
                <div class="row">

                    <%--Side bar--%>
                    <div class="col-sm-3 hidden-xs">
                        <appx:membersidebarrd runat="server" id="MemberSideBarRD" />
                    </div>

                    <%--Main form--%>
                    <div class="col-sm-9 col-xs-12">

                        <p>Here on the Saved U-SELECT Maps page, you can reorder from an existing map, start a new map, or create a 
                        custom map by uploading zip codes.</p>

                        <%--Tabs--%>
                        <div role="tabpanel">

                            <%--Nav tabs--%>
                            <div id="selectTabs">
                                <ul class="nav nav-tabs" role="tablist">
                                    <li role="presentation" class="active"><a href="#myprojects" aria-controls="myprojects" role="tab" data-toggle="tab">My Projects</a></li>
                                    <li role="presentation"><a href="#newproject" aria-controls="profile" role="tab" data-toggle="tab">Start A New Project</a></li>
                                    <li role="presentation"><a href="#pastezips" aria-controls="messages" role="tab" data-toggle="tab">Paste Zip(s)</a></li>
                                    <asp:PlaceHolder ID="phCustomUploadTab" runat="server" Visible="false">
                                        <li role="presentation"><a href="#customfiles" aria-controls="settings" role="tab" data-toggle="tab">Upload Custom File</a></li>
                                    </asp:PlaceHolder>
                                </ul>
                            </div>


                            <%--Tab panes--%>
                            <div class="tab-content">

                                <%--My Projects--%>
                                <div role="tabpanel" class="tab-pane active" id="myprojects">

                                    <div class="well well-sm">

                                        <h4>My Projects</h4>

                                        <div class="row">
                                            <div class="col-sm-8">
                                                <p>Want to place a single order using multiple USelect projects? Place a check mark next to the projects to include 
                                                and build a new project that contains all of the selections.</p>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:LinkButton ID="lnkCombine" runat="server" CssClass="btn btn-primary btn-shadow" data-toggle="modal" data-target="#combinedModal">
                                                    <span class="glyphicon glyphicon-resize-small"></span>&nbsp;Combine Selected
                                                </asp:LinkButton>
                                            </div>
                                        </div>

                                        <p><asp:PlaceHolder ID="phUSelectIntro" runat="server" />&nbsp;</p>

                                        <asp:Panel ID="pnlProjError" runat="server" CssClass="alert alert-danger" Visible="false" ClientIDMode="Static">
                                            <span class="glyphicon glyphicon-remove pull-left"></span>&nbsp;
                                            <asp:Label ID="lblProjError" runat="server" />
                                        </asp:Panel>

                                        <asp:Panel ID="pnlProjSuccess" runat="server" CssClass="alert alert-success" Visible="false" ClientIDMode="Static">
                                            <span class="glyphicon glyphicon-ok pull-left"></span>&nbsp;
                                            <asp:Label ID="lblProjSuccess" runat="server" />
                                        </asp:Panel>


                                        <%--Normal viewing--%>
                                        <asp:ListView ID="lvSelects" runat="server" ItemPlaceholderID="phItemTemplate" DataKeyNames="DistributionId">
                                        
                                            <LayoutTemplate>
                                

                                                <%--Normal view--%>
                                                <table class="table table-condensed table-striped table-bordered table-hover hidden-xs">
                                                    <thead>
                                                        <tr class="tableHeaderRow">
                                                        <th>Select</th>
                                                        <th>Name</th>
                                                        <th>Created Date</th>
                                                        <th>Total Deliveries</th>
                                                        <th>&nbsp;</th>
                                                        </tr>
                                                    </thead>
                                
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phItemTemplate" runat="server" />
                                                    </tbody>

                                                </table>

                                                <%--XS Small Device View--%>
                                                <table class="table table-condensed table-bordered table-hover visible-xs">
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phItemTemplateSmall" runat="server" />
                                                    </tbody>
                                                </table>

                                                <div class="text-center">
                                                    <appx:DataPager ID="pgBottom" runat="server"/>
                                                </div>

                                            </LayoutTemplate>

                                            <ItemTemplate>

                                                <%--Normal View--%>
                                                <tr class="hidden-xs">
                                                    <td class="text-center">
                                                    <asp:CheckBox ID="chkCombine" runat="server" />
                                                    <asp:HiddenField ID="hfDistributionId" runat="server" Value='<%#Eval("DistributionId") %>' />
                                                    </td>

                                                    <td><small><%#Eval("Name")%></small></td>
                                                    <td><small><%#DateTime.Parse(Eval("CreatedDate")).ToString("MM/dd/yyyy")%></small></td>
                                                    <td><small>
                                                        <asp:Literal runat="server" ID="lTotalDeliveries"></asp:Literal>
                                                        <%--<%#Integer.Parse(Eval("TotalDeliveries")).ToString("N0")%>--%>
                                                        </small>
                                                    </td>
                                                    <td>

                                                        <asp:HyperLink ID="hplOpenSelect" runat="server" cssclass="btn btn-cta btn-xs btn-shadow" ToolTip="Start Order">
                                                            <span class="fa fa-check"></span>&nbsp;Start Order
                                                        </asp:HyperLink>

                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%#Eval("DistributionId") & "|" & Eval("ReferenceId") %>' OnClick="OpenMapForEdit" CssClass="btn btn-edit btn-shadow btn-xs" ToolTip="Edit Map">
                                                            <span class="fa fa-pencil"></span>&nbsp;Edit
                                                        </asp:LinkButton>
                                                       
                                                        
                                                        <asp:LinkButton ID="lnkDelete" runat="server" confirmmsg="Are you sure you want to delete this project?" OnClick="DeleteUSelect" CssClass="btn btn-delete btn-shadow btn-xs" CommandArgument='<%#Eval("DistributionId") %>' ToolTip="Delete Map">
                                                            <span class="fa fa-close"></span>&nbsp;Delete
                                                        </asp:LinkButton>

                                                    </td>
                                                </tr>

                                                <%--XS Device View--%>
                                                <div id="xsWrapperBlock" class="visible-xs">

                                                    <div class="panel panel-primary visible-xs">

                                                        <div class="panel-heading">
                                                            <div class="panel-title">Map <%#Eval("Name")%></div>
                                                        </div>

                                                        <div class="panel-body">

                                                            <div class="row visible-xs">

                                                                <div class="col-xs-3">
                                                                    <div class="checkbox">
                                                                        <label>
                                                                        <asp:CheckBox ID="chkCombineXS" runat="server" />
                                                                            Select (<%#Eval("Name")%>)
                                                                        </label>
                                                                    </div>
                                                                    <asp:HiddenField ID="hfDistributionIdXS" runat="server" Value='<%#Eval("DistributionId") %>' />
                                                                </div>

                                                                <div class="col-xs-3">
                                                                    <%#DateTime.Parse(Eval("CreatedDate")).ToString("MM/dd/yyyy")%>
                                                                </div>

                                                                <div class="col-xs-6">
                                                                    <asp:Literal runat="server" ID="lTotalDeliveriesXS"></asp:Literal>
                                                                </div>

                                                            </div>

                                                            <div class="row visible-xs">

                                                                <div class="col-xs-3">
                                                                    <asp:HyperLink ID="hypOpenSelectXS" runat="server" cssclass="btn btn-cta" ToolTip="Start Order">
                                                                        <span class="fa fa-check"></span>&nbsp;Start Order
                                                                    </asp:HyperLink>
                                                                </div>

                                                                <div class="col-xs-3 col-xs-offset-3">
                                                                    <asp:LinkButton ID="lnkEditXS" runat="server" CommandArgument='<%#Eval("DistributionId") & "|" & Eval("ReferenceId") %>' OnClick="OpenMapForEdit" CssClass="btn btn-edit btn-shadow" ToolTip="Edit Map">
                                                                        <span class="fa fa-pencil"></span>&nbsp;Edit
                                                                    </asp:LinkButton>
                                                                </div>

                                                                <div class="col-xs-3">
                                                                    <asp:LinkButton ID="lnkDeleteXS" runat="server" confirmmsg="Are you sure you want to delete this project?" OnClick="DeleteUSelect" CssClass="btn btn-delete btn-shadow" CommandArgument='<%#Eval("DistributionId") %>' ToolTip="Delete Map">
                                                                        <span class="fa fa-close"></span>&nbsp;Delete
                                                                    </asp:LinkButton>
                                                                </div>

                                                            </div>

                                                        </div>
                                                    </div>

                                                </div>

                                            </ItemTemplate>
                        
                                            <EmptyDataTemplate>
                                                <p>You do not have any saved USelect projects.</p>
                                            </EmptyDataTemplate>

                                        </asp:ListView>


                                        <hr />

                                        <%--Combined Selected Modal--%>
                                        <div class="modal fade" id="combinedModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                            <span aria-hidden="true">&times;</span>
                                                        </button>
                                                        <h4 class="modal-title" id="myModalLabel">Combined Project Settings</h4>
                                                    </div>
                                                
                                                    <div class="modal-body">
                                            
                                                        <asp:ValidationSummary ID="vsCombined" runat="server" ValidationGroup="vgCombined" CssClass="alert alert-danger" />

                                                        <div class="form-group">
                                                            <label class="label label-primary" for="CombinedName">Name</label>
                                                            <asp:TextBox ID="CombinedName" runat="server" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator ID="rfvCombinedName" runat="server" ControlToValidate="CombinedName" ErrorMessage="Please enter a name for your new combined project." ValidationGroup="vgCombined" CssClass="label label-danger">
                                                                Please enter a name for your new combined project.
                                                            </asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkCombinedBusiness" runat="server" Checked="False" Text="Include <strong>Business</strong> addresses in this project." />
                                                            </label>
                                                        </div>

                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkCombinedPOBoxes" runat="server" Checked="False" Text="Include <strong>PO Box</strong> addresses in this project" />
                                                            </label>
                                                        </div>

                                                    </div>

                                                    <div class="modal-footer">
                                                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-danger" data-dismiss="modal" ClientIDMode="Static">
                                                            <span class="glyphicon glyphicon-ban-circle"></span>
                                                            &nbsp;Cancel
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCreateCombined" runat="server" ValidationGroup="vgCombined" CssClass="btn btn-primary">
                                                            <span class="glyphicon glyphicon-ok"></span>
                                                            &nbsp;Save Combined Project
                                                        </asp:LinkButton>                                            

                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <%--Original modal--%>
                                        <%--<div id="combinedSettings" title="Combined Project Settings" style="display: none;">
                            
                                            <asp:ValidationSummary ID="vsCombined" runat="server" ValidationGroup="vgCombined" CssClass="alert alert-danger" />
                            
                                            <div class="form-group">
                                                <label class="label label-primary" for="CombinedName">Name</label>
                                                <asp:TextBox ID="CombinedName" runat="server" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="rfvCombinedName" runat="server" ControlToValidate="CombinedName" ErrorMessage="Please enter a name for your new combined project." ValidationGroup="vgCombined" CssClass="label label-danger">
                                                    Please enter a name for your new combined project.
                                                </asp:RequiredFieldValidator>
                                            </div>

                                            <div class="checkbox">
                                                <label>
                                                    <asp:CheckBox ID="chkCombinedBusiness" runat="server" Checked="False" Text="Include <strong>Business</strong> addresses in this project." />
                                                </label>
                                            </div>

                                            <div class="checkbox">
                                                <label>
                                                    <asp:CheckBox ID="chkCombinedPOBoxes" runat="server" Checked="False" Text="Include <strong>PO Box</strong> addresses in this project" />
                                                </label>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:LinkButton ID="lnkCreateCombined" runat="server" Text="Save Combined Project" ValidationGroup="vgCombined" />
                                                </div>
                                            </div>
                            
                                        </div>--%>

                                        <div>&nbsp;</div>

                                    </div>

                                </div>

                                <%--New Project--%>
                                <div role="tabpanel" class="tab-pane" id="newproject">

                                    <div class="well well-sm">

                                        <h4>Start A New Project</h4>

                                        <p>Enter your starting address and zip code.</p>

                                        <asp:ValidationSummary ID="vsGetStarted" runat="server" ValidationGroup="vgGetStarted" CssClass="alert alert-danger" />
                            
                                        <div class="row">
                                            <div class="col-md-7">
                                                <div class="form-group">

                                                    <div class="row">
                                                        <div class="col-sm-10">
                                                            <label class="label label-primary" for="StreetAddress">Street Address</label>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-sm-11">
                                                            <asp:TextBox ID="StreetAddress" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:RequiredFieldValidator ID="rfvStreetAddress" runat="server" ControlToValidate="StreetAddress" ErrorMessage="Please enter the street address." ValidationGroup="vgGetStarted" CssClass="text-danger" Display="Dynamic">
                                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="col-md-5">
                                                <div class="form-group">

                                                    <div class="row">
                                                        <div class="col-sm-9">
                                                            <label class="label label-primary" for="ZipCode">Zip Code</label>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="ZipCode" runat="server" cssclass="form-control" />
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:RequiredFieldValidator ID="rfvZipCode" runat="server" ControlToValidate="ZipCode" ErrorMessage="Please enter the zip code." ValidationGroup="vgGetStarted" CssClass="text-danger" Display="Dynamic">
                                                                <span class="fa fa-2x fa-exclamation-circle"></span>
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div>&nbsp;</div>

                                        <p class="text-center">
                                            <asp:LinkButton ID="lnkGetStarted" runat="server" ValidationGroup="vgGetStarted" CssClass="btn btn-primary">
                                                <span class="fa fa-plus"></span>&nbsp;Get Started
                                            </asp:LinkButton>
                                        </p>

                                        <div>&nbsp;</div>
                                        <div>&nbsp;</div>
                                        <div>&nbsp;</div>

                                    </div>
                                </div>
                    
                                <%--Paste Zip Codes--%>
                                <div role="tabpanel" class="tab-pane" id="pastezips">
                            
                                    <div class="well well-sm">

                                        <h4>Paste Zip Code(s)</h4>

                                        <p>Enter or paste a list of zip codes or zip code and carrier route combinations to
                                        build a new U-Select project.</p>
                            
                                        <p>Entries should be delimited by a comma, semi-colon or line-break. Zip code and carrier
                                        route combinations can appear together or be separated by a space.</p>

                                        <asp:Panel ID="pnlZipError" runat="server" CssClass="alert alert-danger" Visible="false" ClientIDMode="Static">
                                            <span class="glyphicon glyphicon-remove pull-left"></span>&nbsp;
                                            <asp:Label ID="lblZipError" runat="server" />
                                        </asp:Panel>

                                        <asp:Panel ID="pnlZipSuccess" runat="server" CssClass="alert alert-success" Visible="false" ClientIDMode="Static">
                                            <span class="glyphicon glyphicon-ok pull-left"></span>&nbsp;
                                            <asp:Label ID="lblZipSuccess" runat="server" />
                                        </asp:Panel>

                                        <%--<asp:Literal runat="server" ID="lPasteMsg" />--%>

                                        <asp:ValidationSummary runat="server" ID="vsPasteData" ValidationGroup="vgPasteData" CssClass="alert alert-danger"/>

                                        <div class="form-group">

                                            <div class="row">
                                                <div class="col-sm-10">
                                                    <label class="label label-primary" for="PasteDataName">Selection Name</label>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="PasteDataName" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPasteDataName" ControlToValidate="PasteDataName" ErrorMessage="Please enter a name for your U-Select project." CssClass="text-danger" ValidationGroup="vgPasteData" Display="Dynamic">
                                                        <span class="fa fa-2x fa-exclamation-circle"></span>
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        
                                        </div>


                                        <div class="label label-primary">Delivery Types</div>

                                        <div>
                                            <label class="checkbox-inline">
                                                <asp:CheckBox runat="server" ID="chkPasteResidential" Checked="True" Enabled="False" Text="Residential" />
                                            </label>

                                            <label class="checkbox-inline">
                                                <asp:CheckBox runat="server" ID="chkPasteBusiness" Checked="True" Text="Business" />
                                            </label>
                            
                                            <label class="checkbox-inline">
                                                <asp:CheckBox runat="server" ID="chkPastePOBoxes" Checked="True" Text="PO Boxes" />
                                            </label>
                                        </div>

                                        <div>&nbsp;</div>
                            
                                        <div class="form-group">
                                            <label class="label label-primary" for="PasteData">Paste Zip Codes below</label>
                                            <asp:TextBox runat="server" ID="PasteData" TextMode="MultiLine" CssClass="form-control" Rows="5" />
                                        </div>

                                        <p class="text-center">
                                            <asp:LinkButton runat="server" ID="lnkProcessPasteData" ValidationGroup="vgPasteData" CssClass="btn btn-primary">
                                                <span class="glyphicon glyphicon-plus"></span>&nbsp;Create U-Select Map
                                            </asp:LinkButton>
                                        </p>

                                        <div>&nbsp;</div>

                                    </div>

                                </div>
                    
                                <%--Upload File--%>
                                <div role="tabpanel" class="tab-pane" id="customfiles">

                                    <div class="well well-sm">

                                        <asp:PlaceHolder ID="phCustomUpload" runat="server" Visible="false">
                                
                                        <h4>Upload Custom File</h4>

                                        <asp:Panel ID="pnlUploadError" runat="server" CssClass="alert alert-danger" Visible="false" ClientIDMode="Static">
                                            <span class="glyphicon glyphicon-remove pull-left"></span>&nbsp;
                                            <asp:Label ID="lblUploadError" runat="server" />
                                        </asp:Panel>

                                        <asp:Panel ID="pnlUploadSuccess" runat="server" CssClass="alert alert-success" Visible="false" ClientIDMode="Static">
                                            <span class="glyphicon glyphicon-ok pull-left"></span>&nbsp;
                                            <asp:Label ID="lblUploadSuccess" runat="server" />
                                        </asp:Panel>

                                        <p>You can provide us with your very own custom distribution list.  Simply follow the guidelines below and upload
                                        your own file. If you need assistance, please feel free to email us at <a href="mailto:info@taradel.com">info@taradel.com</a>.</p>

                                        <p><strong>Explanation of valid file format</strong></p>

                                        <asp:Literal ID="lCustomExplanation" runat="server" />
    
                                        <p><strong>Example of valid file format</strong></p>

                                        <div>
                                            <asp:Literal ID="lCustomSample" runat="server" />
                                        </div>

                                        <p><strong>Upload your custom selection file</strong></p>

                                        <%--<div>
                                            <asp:Literal ID="lCustomMsg" runat="server" />
                                        </div>--%>

                                        <div class="form-group">
                                            <label class="label label-primary" for="CustomName">Selection Name</label>
                                            <asp:TextBox ID="CustomName" runat="server" CssClass="form-control" />
                                        </div>
                                
                                        <div class="form-group">
                                            <label for="CustomFile">Choose File</label>
                                            <neatUpload:InputFile ID="CustomFile" runat="server" />
                                        </div>

                                        <p class="text-center">
                                            <asp:LinkButton ID="lnkUpload" runat="server" cssclass="btn btn-primary">
                                                <span class="glyphicon glyphicon-cloud-upload"></span>&nbsp;Upload File
                                            </asp:LinkButton>
                                        </p>
                    
                                        </asp:PlaceHolder>
                                
                                    </div>

                                </div>

                            </div>

                        <%--End of Tabs--%>
                        </div>

                    </div>

                </div>

            </div>

        </div>
    </section>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">
</asp:Content>

