
Partial Class InboundRef
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Request.Cookies("Staff") Is Nothing Then
            Dim oSrcCookie As New HttpCookie("RefSource")
            oSrcCookie.Domain = Request.Url.Host
            oSrcCookie.Value = "USPS EDDM Page"
            oSrcCookie.Expires = System.DateTime.Now.AddDays(365)
            Response.Cookies.Add(oSrcCookie)
        End If

        Response.Redirect("~/")
    End Sub
End Class
