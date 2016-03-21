<%@ Control Language="VB" AutoEventWireup="false" CodeFile="JSDatePicker.ascx.vb" Inherits="UserControls_JSDatePicker" %>
    
<asp:HiddenField ID="hfSelectDate" runat="server" />    

<asp:Panel ID="pContainer" runat="server">

    <div class="row">

        <div class="col-sm-6">

            <select id="ddlMonth" runat="server" class="form-control input-sm"> 
                <option value="">Month</option>
            </select>

            <asp:PlaceHolder ID="phDay" runat="server">
                <select id="ddlDay" runat="server" class="form-control input-sm">
                    <option value="">Day</option>
                </select>
                &nbsp;
            </asp:PlaceHolder>

        </div>

        <div class="col-sm-6">
            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control input-sm" />
        </div>

    </div>

    <asp:CustomValidator ID="cvSelectedDate" runat="server" CssClass="label label-danger" Visible="false">
    </asp:CustomValidator>
    
</asp:Panel>
