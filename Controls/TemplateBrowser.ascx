<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TemplateBrowser.ascx.cs" Inherits="TemplateBrowser" %>


<%--Browser Modal--%>
<div class="modal fade" id="modalTemplates" tabindex="-1" role="dialog" aria-labelledby="modalTemplatesLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="btn btn-danger btn-sm pull-right" data-dismiss="modal">
                    <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;Cancel
                </button>
                <div class="modal-title" id="myModalTitle">Select A Template</div>
            </div>
            <div class="modal-body">
                <p>
                    <strong>
                        <em><span class="text-info">Browsing the
                            <asp:Literal ID="litProductName" runat="server" />
                            Templates.</span></em>&nbsp;
                    <asp:Literal ID="litNumOfResults" runat="server"></asp:Literal>
                    </strong>
                </p>
                <%--Search Controls--%>
                <asp:Panel runat="server" ID="pFilter" class="well well-sm">
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:CustomValidator ID="rfvKeyword" runat="server" ClientValidationFunction="ValidateKeywordFilter" ErrorMessage="Keywords must be between 3 and 25 characters." ControlToValidate="txtSearchKey" ValidateEmptyText="False" EnableClientScript="true" Display="Dynamic">
                                <div class="alert alert-danger"><span class="fa fa-exclamation"></span>&nbsp;Keywords must be between 3 and 25 characters.</div>
                            </asp:CustomValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                    <small>
                                        <label for="txtSearchKey" class="col-sm-5 control-label">Find by Keyword</label></small>
                                    <div class="col-sm-7">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtSearchKey" runat="server" CssClass="form-control input-sm" ClientIDMode="Static" MaxLength="25" />
                                            <span class="input-group-btn">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm" />
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                    <small>
                                        <label for="ddlIndustry" class="col-sm-5 control-label">Filter By Category</label></small>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlIndustry" runat="server" CssClass="form-control input-sm" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                    <div class="col-sm-12">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <%--Error Panel--%>
                <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false" EnableViewState="False">
                    <span class="glyphicon glyphicon-remove pull-left"></span>&nbsp;
                    <asp:Literal ID="litError" runat="server" />
                </asp:Panel>


                <%--Info Panel--%>
                <asp:Panel ID="pnlInfo" runat="server" CssClass="alert alert-info" Visible="false" EnableViewState="False">
                    <span class="fa fa-info-circle pull-left"></span>&nbsp;
                    <asp:Literal ID="litInfo" runat="server" />
                </asp:Panel>


                <div class="row">
                    <div id="itemPlaceholder"></div>
                </div>

                <%--List View--%>
                <asp:HiddenField ID="hidReshowModal" runat="server" ClientIDMode="Static" />
            </div>
            <div class="modal-footer">
                <nav style="background: #FFF !important;">
                    <ul class="pager">
                        <li class="previous disabled">
                            <asp:LinkButton ID="lnkPreviousTemplatePage" runat="server"><span aria-hidden="true">&larr;</span> Previous</asp:LinkButton></li>
                        <li>Page <span id="curPage" style="font-weight: bold;"></span>&nbsp;of&nbsp;<span id="totalPages"
                            style="font-weight: bold;"></span></li>
                        <li class="next disabled">
                            <asp:LinkButton ID="lnkNextTemplatePage" runat="server">Next <span aria-hidden="true">&rarr;</span></asp:LinkButton></li>
                    </ul>
                </nav>
            </div>
        </div>
    </div>
</div>


<%--Preview Modal--%>
<div class="modal fade" id="modalPreview" tabindex="-1" role="dialog" aria-labelledby="modalPreviewLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="previewLabel">
                    <asp:Label ID="lblTemplateName" runat="server" ClientIDMode="Static" /></h5>
            </div>

            <div class="modal-body">

                <div class="row">
                    <div class="col-sm-9 text-center templatePreviewRow">
                        <img id="imgTemplatePreview" alt="" class="img-responsive templatePreviewImg" />
                    </div>
                    <div class="col-sm-3">


                        <h5><asp:Literal ID="litTemplateName" runat="server" /></h5>

                        <p>
                            <strong>Category:
                            <asp:Label ID="lblPreviewCategory" runat="server" ClientIDMode="Static" /></strong>
                        </p>

                        <p>This template qualifies for the Every Door Direct Mail&trade; program.</p>

                        <p>Select this template to continue with your order, or go back to view more template options.</p>

                        <asp:PlaceHolder ID="phGeneralDesignerMessage" runat="server" Visible="False">
                            <p>All template orders receive <mark>FREE</mark> text and logo customization by a Taradel graphic design professional - at no additional charge.</p>
                        </asp:PlaceHolder>

                        <asp:PlaceHolder ID="phNoTaradelDesignerMessage" runat="server" Visible="False">
                            <p>All template orders receive <mark>FREE</mark> text and logo customization by a graphic design professional - at no additional charge.</p>
                        </asp:PlaceHolder>

                    </div>
                </div>

            </div>

            <div class="modal-footer">

                <div class="row">

                    <div class="col-sm-3">
                        <asp:LinkButton ID="btnCancelGoBack" runat="server" CssClass="btn btn-danger btn-sm pull-left" ClientIDMode="Static" data-action="cancelGoBack">
                            <span class="glyphicon glyphicon-ban-circle"></span>
                            &nbsp;Cancel and Go Back
                        </asp:LinkButton>
                    </div>

                    <div class="col-sm-3 text-center">
                        <button type="button" class="btn btn-primary btn-sm" data-action="viewFront" id="btnViewFront">
                            <span class="fa fa-search-plus"></span>&nbsp;View Front
                        </button>
                    </div>

                    <div class="col-sm-3 text-center">
                        <button type="button" class="btn btn-primary btn-sm" data-action="viewBack" id="btnViewBack">
                            <span class="fa fa-search-plus"></span>&nbsp;View Back
                        </button>
                    </div>

                    <div class="col-sm-3">
                        <asp:LinkButton ID="btnPreviewSelected" runat="server" CssClass="btn btn-primary btn-lg pull-right" ClientIDMode="Static" data-action="previewSelected">
                            <span class="fa fa-check"></span>
                            &nbsp;Select This Template
                        </asp:LinkButton>
                    </div>

                </div>

            </div>

        </div>
    </div>
</div>


<%--<script src="/assets/javascripts/TemplateBrowser.js"></script>--%>




