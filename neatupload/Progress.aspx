<%@ Page Language="VB" AutoEventWireup="false" Inherits="neatUpload_Progress" CodeFile="~/neatUpload/Progress.aspx.vb" title="Upload Progress" %>
<html>
<head>
    <title>Progress</title>        <style type="text/css">        /*body {
	        font-family: Verdana, Arial, Helvetica, sans-serif;
	        font-size: 11px;
	        color: #000000;
	        margin: 0;
	        padding: 0;
	        /*padding-bottom:75px;*/
        /*}

        .ProgressDisplay {
	        font-weight: bold;
        }

        .ProgressDisplay .StatusMessage {
	        margin: 1px;
	        height: 1.5em;
        }

        .ProgressDisplay .ProgressBar {
	        background-color: #009933;
        }

        .ProgressDisplay .ImageButton {
	        border-width: 0px; 
	        margin: 0px; 
	        padding: 0px;
	        background-color: inherit;
        }

        .ProgressDisplay .ImageButton img {
	        border-width: 2px; 
	        border-style: outset;
	        border-color: white;
	        margin: 0px; 
	        padding: 0px;
	        height: 1.5em;
	        vertical-align: top;
        }

        #progressDisplayCenterer {
	        vertical-align: middle;
	        width: 100%;
	        height: 100%;
        }

        #progressDisplay {
	        vertical-align: middle;
	        width: 100%;
        }

        #progressDisplay TD 
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
	        font-size: 11px;
	        color: #000000;
        }

        #barTd {
	        width: 100%;
        }

        #statusDiv {
	        border-width: 1px;
	        border-style: solid;
	        padding: 0px;
	        position: relative;
	        width: 100%;
	        text-align: center;
	        z-index: 1; 
	        font-family: Arial, Helvetica, sans-serif;
        }

        #barDiv,#barDetailsDiv {
	        border: 0px none ; 
	        margin: 0px; 
	        padding: 0px; 
	        position: absolute; 
	        top: 0pt; 
	        left: 0pt; 
	        z-index: -1; 
	        height: 100%;
	        width: 75%;
        }*/        </style>
</head>
<body>
<form id="frm" runat="server">
<table id="progressDisplayCenterer">
    <tr>
        <td>
        <table id="progressDisplay" class="ProgressDisplay">
            <tr>
                <td id="barTd">
                <div id="statusDiv" runat="server" class="StatusMessage">
		            <neatUpload:DetailsSpan id="normalInProgress" runat="server" WhenStatus="NormalInProgress">
			        <%# FormatCount(BytesRead) %>/<%# FormatCount(BytesTotal) %> <%# CountUnits %>
    			    (<%# String.Format("{0:0%}", FractionComplete) %>) at <%# FormatRate(BytesPerSec) %>
    			    - <%# FormatTimeSpan(TimeRemaining) %> left
        		    </neatUpload:DetailsSpan>
		            <neatUpload:DetailsSpan id="chunkedInProgress" runat="server" WhenStatus="ChunkedInProgress" style="font-weight: normal; white-space: nowrap;">
			            <%# FormatCount(BytesRead) %> <%# CountUnits %>
			            at <%# FormatRate(BytesPerSec) %>
			            - <%# FormatTimeSpan(TimeElapsed) %> elapsed
		            </neatUpload:DetailsSpan>
		            <neatUpload:DetailsSpan id="completed" runat="server" WhenStatus="Completed">
			            Complete: <%# FormatCount(BytesRead) %> <%# CountUnits %>
			            at <%# FormatRate(BytesPerSec) %>
			            took <%# FormatTimeSpan(TimeElapsed) %>
		            </neatUpload:DetailsSpan>
		            <neatUpload:DetailsSpan id="cancelled" runat="server" WhenStatus="Cancelled">
			            Cancelled!
		            </neatUpload:DetailsSpan>
		            <neatUpload:DetailsSpan id="rejected" runat="server" WhenStatus="Rejected">
			            Rejected: <%If Not String.IsNullOrEmpty(Rejection.ToString) Then Response.Write(Rejection.Message) Else Response.Write("")%>
		            </neatUpload:DetailsSpan>
		            <neatUpload:DetailsSpan id="error" runat="server" WhenStatus="Failed">
			            Error: <%If Not String.IsNullOrEmpty(Failure.ToString) Then Response.Write(Failure.Message) Else Response.Write("")%>
		            </neatUpload:DetailsSpan>
		            <neatUpload:DetailsDiv id="barDetailsDiv" runat="server" UseHtml4="true" Width='<%# Unit.Percentage(Math.Floor(100*FractionComplete)) %>' CssClass="ProgressBar"></neatUpload:DetailsDiv>
	            </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="cancel" runat="server" Text="Cancel Upload" Visible='<%# CancelVisible %>' OnClientClick='<%# CancelUrl %>' ></asp:Button>
                    <asp:Button ID="refresh" runat="server" Text="Refresh" Visible='<%# StartRefreshVisible %>' OnClientClick='<%# StartRefreshUrl %>' ></asp:Button>
                    <asp:Button ID="stopRefresh" runat="server" Text="Stop Refresh" Visible='<%# StopRefreshVisible %>' OnClientClick='<%# StopRefreshUrl %>' ></asp:Button>                      
                </td>
            </tr>
        </table>
        </td>
    </tr>
</table>
</form>
</body>
</html>
