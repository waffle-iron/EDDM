<%@ Control Language="VB" AutoEventWireup="false" CodeFile="stateDDList.ascx.vb" Inherits="usercontrols_stateDDList" %>

<asp:Panel ID="pText" runat="server" Visible="false">
    <asp:RequiredFieldValidator ID="rfvStateTxtLeft" runat="server" Text="" ErrorMessage="Please enter the state." Display="Dynamic" EnableClientScript="true" ControlToValidate="StateText" Enabled="false" CssClass="text-danger">
        <span class="fa fa-2x fa-exclamation-circle"></span>
    </asp:RequiredFieldValidator>
    <asp:TextBox ID="StateText" runat="server" Columns="25" />
    <asp:RequiredFieldValidator ID="rfvStateTxtRight" runat="server" Text="" ErrorMessage="Please enter the state." Display="Dynamic" EnableClientScript="true" ControlToValidate="StateText" Enabled="false" CssClass="text-danger">
        <span class="fa fa-2x fa-exclamation-circle"></span>
    </asp:RequiredFieldValidator>
    <asp:Literal ID="WICustomVMessage" runat="server"></asp:Literal>
</asp:Panel>

<asp:Panel ID="pList" runat="server" Visible="false">
    <asp:RequiredFieldValidator ID="rfvStateLeft" runat="server" Text="" ErrorMessage="Please select the state." Display="Dynamic" EnableClientScript="true" ControlToValidate="StateList" Enabled="false" CssClass="text-danger">
        <span class="fa fa-2x fa-exclamation-circle"></span>
    </asp:RequiredFieldValidator>
    <asp:DropDownList ID="StateList" runat="server" AppendDataBoundItems="true" CssClass="form-control">
        <asp:ListItem Text="Select State" Value="" />
        <asp:ListItem Value="AL">Alabama</asp:ListItem>
        <asp:ListItem Value="AK">Alaska</asp:ListItem>
        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
        <asp:ListItem Value="CA">California</asp:ListItem>
        <asp:ListItem Value="CO">Colorado</asp:ListItem>
        <asp:ListItem Value="CT">Connecticut</asp:ListItem>
        <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
        <asp:ListItem Value="DE">Delaware</asp:ListItem>
        <asp:ListItem Value="FL">Florida</asp:ListItem>
        <asp:ListItem Value="GA">Georgia</asp:ListItem>
        <asp:ListItem Value="HI">Hawaii</asp:ListItem>
        <asp:ListItem Value="ID">Idaho</asp:ListItem>
        <asp:ListItem Value="IL">Illinois</asp:ListItem>
        <asp:ListItem Value="IN">Indiana</asp:ListItem>
        <asp:ListItem Value="IA">Iowa</asp:ListItem>
        <asp:ListItem Value="KS">Kansas</asp:ListItem>
        <asp:ListItem Value="KY">Kentucky</asp:ListItem>
        <asp:ListItem Value="LA">Louisiana</asp:ListItem>
        <asp:ListItem Value="ME">Maine</asp:ListItem>
        <asp:ListItem Value="MD">Maryland</asp:ListItem>
        <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
        <asp:ListItem Value="MI">Michigan</asp:ListItem>
        <asp:ListItem Value="MN">Minnesota</asp:ListItem>
        <asp:ListItem Value="MS">Mississippi</asp:ListItem>
        <asp:ListItem Value="MO">Missouri</asp:ListItem>
        <asp:ListItem Value="MT">Montana</asp:ListItem>
        <asp:ListItem Value="NE">Nebraska</asp:ListItem>
        <asp:ListItem Value="NV">Nevada</asp:ListItem>
        <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
        <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
        <asp:ListItem Value="NM">New Mexico</asp:ListItem>
        <asp:ListItem Value="NY">New York</asp:ListItem>
        <asp:ListItem Value="NC">North Carolina</asp:ListItem>
        <asp:ListItem Value="ND">North Dakota</asp:ListItem>
        <asp:ListItem Value="OH">Ohio</asp:ListItem>
        <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
        <asp:ListItem Value="OR">Oregon</asp:ListItem>
        <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
        <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
        <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
        <asp:ListItem Value="SC">South Carolina</asp:ListItem>
        <asp:ListItem Value="SD">South Dakota</asp:ListItem>
        <asp:ListItem Value="TN">Tennessee</asp:ListItem>
        <asp:ListItem Value="TX">Texas</asp:ListItem>
        <asp:ListItem Value="UT">Utah</asp:ListItem>
        <asp:ListItem Value="VI">US Virgin Islands</asp:ListItem>
        <asp:ListItem Value="VT">Vermont</asp:ListItem>
        <asp:ListItem Value="VA">Virginia</asp:ListItem>
        <asp:ListItem Value="WA">Washington</asp:ListItem>
        <asp:ListItem Value="WV">West Virginia</asp:ListItem>
        <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
        <asp:ListItem Value="WY">Wyoming</asp:ListItem>
        <asp:ListItem Value="AA">AA - APO/FPO</asp:ListItem>
        <asp:ListItem Value="AE">AE - APO/FPO</asp:ListItem>
        <asp:ListItem Value="AP">AP - APO/FPO</asp:ListItem>
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvStateList" runat="server" Text="" ErrorMessage="Please select the state." Display="Dynamic" EnableClientScript="true" ControlToValidate="StateList" Enabled="false" CssClass="text-danger">
        <span class="fa fa-2x fa-exclamation-circle"></span>
    </asp:RequiredFieldValidator>
    
    <asp:Literal ID="DDCustomVMessage" runat="server"></asp:Literal>

</asp:Panel>
<asp:Panel ID="pReadOnly" runat="server" Visible="false">
    <asp:Literal ID="lState" runat="server" />
</asp:Panel>
