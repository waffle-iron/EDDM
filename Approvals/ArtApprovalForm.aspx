<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ArtApproval.master" AutoEventWireup="true" CodeFile="ArtApprovalForm.aspx.cs" Inherits="Approvals_ArtApprovalForm" %>


<%--Header Logo--%>
<asp:Content ID="Content1" ContentPlaceHolderID="phHeaderLogo" Runat="Server">
    <asp:Image ID="imgHeaderImage" runat="server" CssClass="img-responsive" />
</asp:Content>


<%--Header - Large, Med, and Small sizes--%>
<asp:Content ID="Content2" ContentPlaceHolderID="phHeaderLg" Runat="Server">
    Artwork Proof Approval Form
</asp:Content>


<%--Header - XS sizes--%>
<asp:Content ID="Content3" ContentPlaceHolderID="phHeaderSm" Runat="Server">
    Artwork Proof Approval Form
</asp:Content>


<%--Form contents--%>
<asp:Content ID="Content4" ContentPlaceHolderID="phBody" Runat="Server">

    <div>

        <%--Error Display--%>
        <asp:Panel ID="pnlError" runat="server" Visible="false">
            <span class="fa fa-2x fa-exclamation-circle pull-left"></span>&nbsp;
            <asp:Literal ID="litErrorMessage" runat="server" />
            <p>&nbsp;</p>
        </asp:Panel>

        <%--Success display--%>
        <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success" Visible="false">
            <span class="fa fa-3x fa-check-circle pull-left"></span>
            <p class="lead"><asp:Literal ID="litSuccessMessage" runat="server" /></p>
        </asp:Panel>

        <%--Form--%>
        <asp:Panel ID="pnlApprovalForm" runat="server">

            <p>Please carefully review the artwork proofs sent to you via email before submitting this approval form. <asp:Literal ID="litCompanyNotResponsible" runat="server" /> 
            is not responsible for any graphic, text, pricing, color variation, or format issues/errors after you submit your approval to print. Once your artwork is 
            approved you are responsible for all errors not brought to our attention.</p>

            <p>The most recent proof you received via email is the proof we will print.</p>

            <div>&nbsp;</div>

            <div>
                <asp:ValidationSummary ID="vsApproval" runat="server" CssClass="alert alert-danger" HeaderText="Please check for the following errors:" />
            </div>

            <div role="form">

                <%--Name--%>
                <div class="row">
                    <div class="col-xs-10">

                        <div class="form-group">
                            <label for="txtFullName">Please provide your full name:</label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" ClientIDMode="Static" />
                        </div>

                    </div>
                    <div class="col-xs-2">

                        <br />
                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ErrorMessage="Your name is required." SetFocusOnError="True" Display="Dynamic" ControlToValidate="txtFullName">
                            <span class="fa fa-exclamation-circle fa-2x text-danger"></span>
                        </asp:RequiredFieldValidator>

                    </div>
                </div>
            
                <%--Company--%>
                <div class="row">
                    <div class="col-xs-10">

                        <div class="form-group">
                            <label for="txtCompanyName">Please type your company/organization's name:</label>
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" ClientIDMode="Static" />
                        </div>

                    </div>
                    <div class="col-xs-2">

                        <br />
                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Company name is required." SetFocusOnError="True" Display="Dynamic" ControlToValidate="txtCompanyName">
                            <span class="fa fa-exclamation-circle fa-2x text-danger"></span>
                        </asp:RequiredFieldValidator>

                    </div>
                </div>

                <%--Order #--%>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-10">
                            <label for="txtOrderNumber">Please type your Order # (if known):</label>
                            <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="form-control" ClientIDMode="Static" />
                        </div>
                    </div>
                </div>

                <%--Designer--%>
                <asp:Panel ID="pnlDesigner" runat="server">
                    <div class="row">
                        <div class="col-xs-10">

                            <div class="form-group">
                                <label for="txtDesigner">Please enter your designer's name:</label>
                                <asp:TextBox ID="txtDesigner" runat="server" CssClass="form-control" ClientIDMode="Static" />
                            </div>

                        </div>
                        <div class="col-xs-2">

                            <br />
                            <asp:RequiredFieldValidator ID="rfvDesigner" runat="server" ErrorMessage="The Designer's name is required." SetFocusOnError="True" Display="Dynamic" ControlToValidate="txtDesigner">
                                <span class="fa fa-exclamation-circle fa-2x text-danger"></span>
                            </asp:RequiredFieldValidator>

                        </div>
                    </div>
                </asp:Panel>

                <%--Experience--%>
                <asp:Panel ID="pnlExperience" runat="server">
                    <div class="row">
                        <div class="col-xs-10">

                            <div class="form-group">
                                <label for="ddlExperience">On a scale of 0-10 (10 being best), how would you rate your design experience:</label>
                                <asp:DropDownList ID="ddlExperience" runat="server" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Text="Select One" Value="" Selected="True">Select One</asp:ListItem>
                                    <asp:ListItem Text="0 - Poor" Value="0" />
                                    <asp:ListItem Text="1" Value="1" />
                                    <asp:ListItem Text="2" Value="2" />
                                    <asp:ListItem Text="3" Value="3" />
                                    <asp:ListItem Text="4" Value="4" />
                                    <asp:ListItem Text="5" Value="5" />
                                    <asp:ListItem Text="6" Value="6" />
                                    <asp:ListItem Text="7" Value="7" />
                                    <asp:ListItem Text="8" Value="8" />
                                    <asp:ListItem Text="9" Value="9" />
                                    <asp:ListItem Text="10 - Perfect" Value="10" />
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div class="col-xs-2">

                            <br />
                            <asp:RequiredFieldValidator ID="rfvExperience" runat="server" ErrorMessage="Please rate your experience." SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlExperience">
                                <span class="fa fa-exclamation-circle fa-2x text-danger"></span>
                            </asp:RequiredFieldValidator>

                        </div>
                    </div>
                </asp:Panel>

                <%--Recommend--%>
                <asp:Panel ID="pnlRecommend" runat="server">
                    <div class="row">
                    <div class="col-xs-10">

                        <div class="form-group">
                            <label for="ddlRecommend">On a scale of 0-10 (10 being extremely likely), how likely is it that you would recommend us to a friend or colleague?</label>
                            <asp:DropDownList ID="ddlRecommend" runat="server" ClientIDMode="Static" CssClass="form-control">
                                <asp:ListItem Text="Select One" Value="" Selected="True">Select One</asp:ListItem>
                                <asp:ListItem Text="0 - Not likely at all" Value="0" />
                                <asp:ListItem Text="1" Value="1" />
                                <asp:ListItem Text="2" Value="2" />
                                <asp:ListItem Text="3" Value="3" />
                                <asp:ListItem Text="4" Value="4" />
                                <asp:ListItem Text="5 - Neutral" Value="5" />
                                <asp:ListItem Text="6" Value="6" />
                                <asp:ListItem Text="7" Value="7" />
                                <asp:ListItem Text="8" Value="8" />
                                <asp:ListItem Text="9" Value="9" />
                                <asp:ListItem Text="10 - Extremely likely" Value="10" />
                            </asp:DropDownList>
                        </div>

                    </div>
                    <div class="col-xs-2">

                        <br />
                        <asp:RequiredFieldValidator ID="rfvRecommend" runat="server" ErrorMessage="Please rate your recommendation." SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlRecommend">
                            <span class="fa fa-exclamation-circle fa-2x text-danger"></span>
                        </asp:RequiredFieldValidator>

                    </div>
                </div>
                </asp:Panel>

                <div>&nbsp;</div>

                <%--Confirm--%>
                <div class="row">
                    <div class="col-xs-10">

                        <strong>Please confirm that you understand that proofreading is your responsibility</strong>
                        <div class="checkbox">
                            <label>
                                <asp:CheckBox ID="chkConfirm" runat="server" ClientIDMode="Static" />
                                I have proofread my artwork and am aware that <asp:Literal ID="litSiteNameDisclaimer" runat="server" /> is not responsible for any graphic, text, pricing, 
                                color variation or format issues after I submit the approval for print.
                            </label>
                        </div>

                    </div>
                    <div class="col-xs-2">

                        <br />

                        <asp:CustomValidator ID="cvConfirm" runat="server" CssClass="" ClientValidationFunction="ConfirmProof" ErrorMessage="Please confirm that you understand that proofreading is your responsibility." Display="Dynamic">
                            <span class="fa fa-exclamation-circle fa-2x text-danger"></span>
                        </asp:CustomValidator>

                    </div>
                </div>

                <div>&nbsp;</div>

                <%--Comments--%>
                <div class="form-group">
                    <label for="txtComments">Please include comments about your experience</label>
                    <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" ClientIDMode="Static" TextMode="MultiLine" Rows="4" />
                </div>

                <%--Feedback--%>
                <div>
                    <strong>May we share your feedback?</strong>
                    <asp:RadioButtonList ID="radShareFeedback" runat="server" RepeatDirection="Vertical" ClientIDMode="Static" CssClass="paddedRadioButtonList">
                        <asp:ListItem Text="Yes, I'm happy to share my feedback" Value="Yes" Selected="True" />
                        <asp:ListItem Text="No, do not share my feedback with others" Value="No" />
                    </asp:RadioButtonList>
                </div>

                <div>&nbsp;</div>

                <%--Disclaimer--%>
                <div class="well well-sm">
                    <small>
                        <strong>Graphic and Image Disclaimer</strong><br />
                        You understand that <asp:Literal ID="litCompanyDisclaimer" runat="server" /> and/or its representatives are 
                        not responsible for likenesses or graphics provided to us or deriving from pictures or images captured on your behalf. 
                        The graphics and/or likenesses are provided with the understanding that <asp:Literal ID="litCompanyDisclaimer2" runat="server" /> and/or its 
                        affiliates or representatives are not engaged in rendering legal counseling or other professional 
                        services or advice regarding copyright or trademark infringement. <asp:Literal ID="litCompanyDisclaimer3" runat="server" /> and/or its affiliates or 
                        representatives encourage you to seek appropriate professional advice for any questions. 
                        <asp:Literal ID="litCompanyDisclaimer4" runat="server" /> and/or its affiliates or representatives shall not be responsible for any loss or 
                        damage caused, or alleged to have been caused, directly or indirectly, by the graphics and/or 
                        likenesses or ideas contained. No advice or information, whether oral or written, obtained by 
                        you from <asp:Literal ID="litCompanyDisclaimer5" runat="server" /> and/or its affiliates or representatives shall create any warranty not 
                        expressly made herein. <asp:Literal ID="litCompanyDisclaimer6" runat="server" /> and/or its affiliates or representatives are not responsible for 
                        copyrighted or trademarked data or information. As a convenience to our customers, 
                        we may suggest resources, which are beyond our control. We make no representations as to the quality, 
                        suitability, functionality or legality of any suggestion to which we may provide and you 
                        hereby waive any claim you might have against <asp:Literal ID="litCompanyDisclaimer7" runat="server" /> and /or its affiliates or representatives.
                    </small>
                </div>

                <div>&nbsp;</div>

                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-success btn-lg pull-right" OnClick="SubmitApproval">
                    <span class="fa fa-check"></span>&nbsp;Approve to Print
                </asp:LinkButton>

                <div>&nbsp;</div>

            </div>

        </asp:Panel>

    </div>

</asp:Content>



<%--Footer Logo--%>
<asp:Content ID="Content6" ContentPlaceHolderID="phFooterLogo" Runat="Server">
    <asp:Image ID="imgFooterLogo" runat="server" CssClass="img-responsive" />
</asp:Content>

<%--Footer Company--%>
<asp:Content ID="Content7" ContentPlaceHolderID="phFooterCompany" Runat="Server">
    <asp:Literal ID="litFooterCompany" runat="server" />
</asp:Content>


<%--Footer Trademarks--%>
<asp:Content ID="Content8" ContentPlaceHolderID="phFooterTrademarks" Runat="Server">
    <asp:Literal ID="litFooterTrademarks" runat="server" />
</asp:Content>


<%--javascripts--%>
<asp:Content ID="Content9" ContentPlaceHolderID="phScripts" Runat="Server">

    <script type="text/javascript">

        function ConfirmProof(sender, args) {
            var tc = jQuery('#chkConfirm');

            if (tc.is(':checked'))
            { args.IsValid = true; }

            else
            { args.IsValid = false; }

        };

    </script>

</asp:Content>






