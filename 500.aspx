<%@ Page Language="VB" MasterPageFile="~/app_masterpages/Site.master" AutoEventWireup="false" CodeFile="500.aspx.vb" Inherits="_500" Title="An Error Has Occured" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phForm" runat="Server">

<div class="container">

    <div class="partialRibbon hidden-sm hidden-xs">
        <span class="arrowLeft"></span>
        <span class="subRibbonPop">Sorry!</span>
        <span class="subRibbon">An Error Occurred</span>
    </div>

    <div class="partialRibbonSmall visible-sm visible-xs">
        <span class="subRibbonPopSmall">Sorry!</span>
        <span class="subRibbonSmall">An Error Occurred</span>
    </div>
    

    <div class="contentWrapper">
        <div class="row">

            <div class="col-sm-12">
                <div class="jumbotron">

                    <p class="lead">An error has been encountered in the application. This error has been recorded and reported to the support team.</p>

                    <p>&nbsp;</p>

                </div>
            </div>

        </div>
    </div>


</div>





</asp:Content>
