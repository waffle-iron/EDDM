
Partial Class account_quotes
    Inherits pageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim oA As New pndQuoteTableAdapters.QuoteTableAdapter
        'Dim oT As pndQuote.QuoteDataTable = oA.GetCustomerQuotes(appHelp.GetCustomerID)
        'If oT.Rows.Count > 0 Then
        '    rQuotes.DataSource = oT
        '    rQuotes.DataBind()
        'Else
        '    rQuotes.Visible = False
        '    pnlNoQuotes.Visible = True
        'End If
        'oT.Dispose()
        'oA.Dispose()
    End Sub

End Class
