
Partial Class CCustom_36_FedexSearch
    Inherits CLibraryBase
    
    Protected Overrides Sub BuildControl()

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Sub headersearchsubmit_Click(sender As Object, e As System.EventArgs) Handles headersearchsubmit.Click
        Response.Redirect("http://www.fedex.com/Search/search?q=" & Server.UrlEncode(headersearchbox.Text) & "&output=xml_no_dtd&sort=date%3AD%3AL%3Ad1&client=fedex_us_fxo_support&ud=1&oe=UTF-8&ie=UTF-8&proxystylesheet=fedex_us_fxo_support&hl=en&site=fxo_support&headerFooterDir=us")
    End Sub
End Class
