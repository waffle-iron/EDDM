<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderSteps.ascx.cs" Inherits="OrderSteps" %>

<div class="orderStepsWrapperNew">
    <div class="row">
        <div class="col-sm-12">

		    <ol class="cd-multi-steps text-center" id="orderSteps">
			    <li runat="server" id="liStep1" class="visited" ClientIDMode="static">
                    <asp:HyperLink ID="hypStep1" runat="server" ClientIDMode="static" Visible="False" />
                    <asp:Label ID="lblStep1" runat="server" ClientIDMode="static" Visible="False" />
			    </li>
			    <li runat="server" id="liStep2" class="visited" ClientIDMode="static">
                    <asp:HyperLink ID="hypStep2" runat="server" ClientIDMode="static" Visible="False" />
                    <asp:Label ID="lblStep2" runat="server" ClientIDMode="static" Visible="False" />
			    </li>
			    <li runat="server" id="liStep3" class="visited" ClientIDMode="static">
                    <asp:HyperLink ID="hypStep3" runat="server" ClientIDMode="static" Visible="False" />
                    <asp:Label ID="lblStep3" runat="server" ClientIDMode="static" Visible="False" />
			    </li>
			    <li runat="server" id="liStep4" class="visited" visible="false" ClientIDMode="static">
                    <asp:HyperLink ID="hypStep4" runat="server" ClientIDMode="static" Visible="False" />
                    <asp:Label ID="lblStep4" runat="server" ClientIDMode="static" Visible="False" />
			    </li>
			    <li runat="server" id="liStep5" class="visited" visible="false" ClientIDMode="static">
                    <asp:HyperLink ID="hypStep5" runat="server" ClientIDMode="static" Visible="False" />
                    <asp:Label ID="lblStep5" runat="server" ClientIDMode="static" Visible="False" />
			    </li>
		    </ol>

        </div>
    </div>
    </div>

