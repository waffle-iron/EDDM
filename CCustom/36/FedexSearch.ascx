<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FedexSearch.ascx.vb"
    Inherits="CCustom_36_FedexSearch" %>
<div id="header-search">
    <asp:Panel runat="server" ID="pSearch" DefaultButton="headersearchsubmit">
        <asp:TextBox runat="server" ID="headersearchbox" CssClass="headersearchbox" placeholder="What are you looking for?"
            alt="What are you looking for?" /><asp:Button runat="server" ID="headersearchsubmit" CssClass="headersearchsubmit" />
    </asp:Panel>
</div>
<%--<form id="header-search" name="searchform" method="post" action="http://www.fedex.com/cgi-bin/search_redirect">
<input type="text" name="q" value="What are you looking for?" alt="What are you looking for?"
    id="header-input-string"><input type="submit" value="" id="header-search-submit"
        onclick="document.header-search.submit();"><input type="hidden" name="output" value="xml_no_dtd"><input
            type="hidden" name="sort" value="date:D:L:d1"><input type="hidden" name="client"
                value="fedex_us_fxo_support"><input type="hidden" name="ud" value="1"><input type="hidden"
                    name="oe" value="UTF-8"><input type="hidden" name="ie" value="UTF-8"><input type="hidden"
                        name="proxystylesheet" value="fedex_us_fxo_support"><input type="hidden" name="hl"
                            value="en"><input type="hidden" name="site" value="fxo_support"><input type="hidden"
                                name="headerFooterDir" value="us">
</form>
--%>