<%@ control language="VB" autoeventwireup="false" codefile="PaymentOptions.ascx.vb"
    inherits="CCustom_PaymentOptions" %>
<asp:Panel ID="pPaymentOptions" runat="server">
    <h3>Payment Options</h3>
    <ul class="list-inline">
        <asp:PlaceHolder ID="pCreditCard" runat="server">
            <li class="<%=GetLiClass() %>">
                <img class="img-responsive" alt="Visa" src="/cmsimages/visa-curved.png" title="Visa" /></li>
            <li class="<%=GetLiClass() %>">
                <img class="img-responsive" alt="MasterCard" src="/cmsimages/mastercard-curved.png"
                    title="MasterCard" /></li>
            <li class="<%=GetLiClass() %>">
                <img class="img-responsive" alt="American Express" src="/cmsimages/american-express-curved.png"
                    title="American Express" /></li>
            <li class="<%=GetLiClass() %>">
                <img class="img-responsive" alt="Discover" src="/cmsimages/discover-curved.png"
                    title="Discover" /></li>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="pECheck" runat="server">
            <li class="<%=GetLiClass() %>">
                <img class="img-responsive" alt="eCheck" src="/cmsimages/echeck-curved.png" title="eCheck" /></li>
        </asp:PlaceHolder>
    </ul>
</asp:Panel>
