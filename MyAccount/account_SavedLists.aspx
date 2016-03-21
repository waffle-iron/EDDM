<%@ Page Title="" Language="VB" MasterPageFile="~/App_MasterPages/Site.master" AutoEventWireup="false" CodeFile="account_SavedLists.aspx.vb" Inherits="SavedLists" %>
<%@ Register Src="~/Controls/YourAccountUserMenu.ascx" TagPrefix="appx" TagName="YourAccountUserMenu" %>
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
                    <appx:YourAccountUserMenu runat="server" id="YourAccountUserMenu" />
                </div>

                <%--Main form--%>
                <div class="col-sm-9 col-xs-12">

                        <div class="well well-sm">

                            <h5>My Lists</h5>

                            <div class="row">

                                <div class="col-sm-8">
                                    <p>Listed below are your saved Addressed Lists. This page will display both types of lists, <a href="../Addressed/Step1-UploadYourList.aspx">Uploaded Lists</a> 
                                    and <a href="../Addressed/Step1-BuildYourList.aspx">Generated Lists</a>.
                                    You can reorder from an existing list, start a brand new list.</p>
                                </div>

                                <div class="col-sm-4">

                                    <p class="text-right"><asp:LinkButton ID="btnGenerateNewList" runat="server" CssClass="btn btn-primary">
                                        <span class="fa fa-file-text-o"></span>&nbsp;Create a New List
                                    </asp:LinkButton></p>

                                    <p class="text-right"><asp:LinkButton ID="btnUploadNewList" runat="server" CssClass="btn btn-primary">
                                        <span class="fa fa-upload"></span>&nbsp;Upload a New List
                                    </asp:LinkButton></p>

                                </div>

                            </div>

                            <div>&nbsp;</div>

                            <asp:Panel ID="pnlProjError" runat="server" CssClass="alert alert-danger" Visible="false" ClientIDMode="Static">
                                <span class="glyphicon glyphicon-remove pull-left"></span>&nbsp;
                                <asp:Label ID="lblProjError" runat="server" />
                            </asp:Panel>

                            <asp:Panel ID="pnlProjSuccess" runat="server" CssClass="alert alert-success" Visible="false" ClientIDMode="Static">
                                <span class="glyphicon glyphicon-ok pull-left"></span>&nbsp;
                                <asp:Label ID="lblProjSuccess" runat="server" />
                            </asp:Panel>

                            <asp:ListView ID="lvSelects" runat="server" ItemPlaceholderID="phItemTemplate" DataKeyNames="DistributionId">
                                        
                                <LayoutTemplate>

                                    <%--Normal view--%>
                                    <table class="table table-condensed table-striped table-bordered table-hover hidden-xs">
                                        <thead>
                                            <tr class="tableHeaderRow">
                                            <th class="width25">Name</th>
                                            <th class="width10 text-center">Created Date</th>
                                            <th class="width25 text-center">Type</th>
                                            <th class="width10 text-center">Total Deliveries</th>
                                            <th class="width30">&nbsp;</th>
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
                                        <td class="width25"><small><%#Eval("Name")%></small></td>
                                        <td class="width10"><small><%#DateTime.Parse(Eval("CreatedDate")).ToString("MM/dd/yyyy")%></small></td>
                                        <td class="width25">
                                            <small>
                                                <asp:Literal runat="server" ClientIDMode="Static" ID="lDescription" Text='<%#RetrieveDescription(Eval("DistributionID"), Eval("USelectID")) %>'></asp:Literal>
                                                &nbsp;<asp:HyperLink ID="hypViewFilters" ClientIDMode="Static" data-toggle="modal" data-action="ViewFilters" data-target="#filterDataModal" data-filter-title='<%#Eval("Name")%>' data-filter-data='<%#GetDataFilters(Eval("DistributionID"))%>' NavigateUrl="#" runat="server" Visible="False"><span class="fa fa-search"></span>&nbsp;View Filters</asp:HyperLink>
                                            </small>
                                            <asp:HiddenField ID="hfDistributionId" runat="server" Value='<%#Eval("DistributionId") %>' />
                                        </td>
                                        <td class="width10">
                                            <small><asp:Literal runat="server" ID="lTotalDeliveries" Text='<%#Integer.Parse(Eval("TotalDeliveries")).ToString("N0") %>'></asp:Literal></small>
                                        </td>
                                        <td class="width30">
                                            <p class="text-right">
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%#Eval("DistributionId") & "|" & Eval("ReferenceId") %>' OnClick="OpenListForEdit" CssClass="btn btn-primary btn-xs">
                                                <span class="glyphicon glyphicon-pencil"></span>&nbsp;Edit
                                            </asp:LinkButton>
                                            <asp:HyperLink ID="hplOpenSelect" runat="server" cssclass="btn btn-primary btn-xs">
                                                <span class="glyphicon glyphicon-plus"></span>&nbsp;Start Order
                                            </asp:HyperLink>
                                            <asp:LinkButton ID="lnkDelete" runat="server" confirmmsg="Are you sure you want to delete this list?" OnClick="DeleteList" CssClass="btn btn-danger btn-xs" CommandArgument='<%#Eval("DistributionId") %>'>
                                                <span class="glyphicon glyphicon-trash"></span>&nbsp;Delete
                                            </asp:LinkButton>
                                            </p>
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
                                                        <asp:HiddenField ID="hfDistributionIdXS" runat="server" Value='<%#Eval("DistributionId") %>' />
                                                        <%#DateTime.Parse(Eval("CreatedDate")).ToString("MM/dd/yyyy")%>
                                                    </div>

                                                    <div class="col-xs-6">
                                                        <asp:Literal runat="server" ID="lTotalDeliveriesXS"></asp:Literal>
                                                    </div>

                                                </div>

                                                <div class="row visible-xs">
                                                    <div class="col-xs-3 col-xs-offset-3">
                                                        <asp:LinkButton ID="lnkEditXS" runat="server" CommandArgument='<%#Eval("DistributionId") & "|" & Eval("ReferenceId") %>' OnClick="OpenListForEdit" CssClass="btn btn-primary">
                                                            <span class="glyphicon glyphicon-pencil"></span>&nbsp;Edit
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="col-xs-3">
                                                        <asp:HyperLink ID="hypOpenSelectXS" runat="server" cssclass="btn btn-primary">
                                                            <span class="glyphicon glyphicon-plus"></span>&nbsp;Start Order
                                                        </asp:HyperLink>
                                                    </div>
                                                    <div class="col-xs-3">
                                                        <asp:LinkButton ID="lnkDeleteXS" runat="server" confirmmsg="Are you sure you want to delete this list?" OnClick="DeleteList" CssClass="btn btn-danger" CommandArgument='<%#Eval("DistributionId") %>'>
                                                            <span class="glyphicon glyphicon-trash"></span>&nbsp;Delete
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                    </div>

                                </ItemTemplate>
                        
                                <EmptyDataTemplate>
                                    <p>You do not have any saved lists.</p>
                                </EmptyDataTemplate>

                            </asp:ListView>

                            <div>&nbsp;</div>

                            <div class="modal fade" id="filterDataModal" tabindex="-1" role="dialog" aria-labelledby="filterDataModal" aria-hidden="true">
                                <div class="modal-dialog">
                                    <div class="modal-content">
            
                                        <div class="modal-header">
                                            <h4 class="modal-title">Filters used for <span id="modalTitle"></span></h4>
                                        </div>

                                        <div class="modal-body">
                                            <span id="modalContent"></span>
                                        </div>

                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                                        </div>

                                    </div>
                                </div>
                            </div>

                        </div>

                </div>

            </div>

        </div>

    </div>

</section>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpScripts" Runat="Server">

    <script type="text/javascript">

        $('a[data-action=ViewFilters]').click(function (e)
        {

            //No postback
            e.preventDefault();

            //grab the properties
            var title = $(this).attr('data-filter-title');
            var filters = $(this).attr('data-filter-data');

            //hidden fields. Located on Container page. Update hidden textbox too for postbacks....
            $('#modalTitle').html(title);
            $('#modalContent').html(filters);


        });

    </script>

</asp:Content>

