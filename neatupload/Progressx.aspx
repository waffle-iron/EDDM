<%@ Page language="c#" AutoEventWireup="false" MasterPageFile="~/app_masterpages/popup.master" Inherits="Brettle.Web.NeatUpload.ProgressPage" %>
<%--
NeatUpload - an HttpModule and User Controls for uploading large files
Copyright (C) 2005  Dean Brettle

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
--%>
<asp:Content ID="cPage" runat="server" ContentPlaceHolderID="phBody">
<table id="progressDisplayCenterer">
<tr>
<td>
<table id="progressDisplay" class="ProgressDisplay">
<tr>
<td>
	<span id="label" runat="server" class="Label">Upload&#160;Status:</span>
</td>
<td id="barTd" >
	<div id="statusDiv" runat="server" class="StatusMessage">&#160;
		<neatUpload:DetailsSpan id="normalInProgress" runat="server" WhenStatus="NormalInProgress" style="font-weight: normal; white-space: nowrap;">
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
			Rejected: <%# Rejection != null ? Rejection.Message : "" %>
		</neatUpload:DetailsSpan>
		<neatUpload:DetailsSpan id="Error" runat="server" WhenStatus="Failed">
			Error: <%# Failure != null ? Failure.Message : "" %>
		</neatUpload:DetailsSpan>
		<neatUpload:DetailsDiv id="barDetailsDiv" runat="server" UseHtml4="true"
			 Width='<%# Unit.Percentage(Math.Floor(100*FractionComplete)) %>' class="ProgressBar"></neatUpload:DetailsDiv>	
	</div>
</td>
</tr>
<tr>
<td>
    <asp:Button ID="cancel" runat="server" Text="Cancel Upload" Visible='<%#CancelVisible %>' OnClientClick='<%# CancelUrl %>' ></asp:Button>
    <asp:Button ID="Refresh" runat="server" Text="Refresh" Visible='<%# StartRefreshVisible %>' OnClientClick='<%# StartRefreshUrl %>' ></asp:Button>
    <asp:Button ID="stopRefresh" runat="server" Text="Stop Refresh" Visible='<%# StopRefreshVisible %>' OnClientClick='<%# StopRefreshUrl %>' ></asp:Button>                      
</td>
</tr>
</table>
</td>
</tr>
</table>
</asp:Content>
