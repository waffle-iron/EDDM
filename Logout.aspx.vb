
Partial Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sCacheKey As String = "appxAuth:" & User.Identity.Name & ":Roles"
        Application(sCacheKey) = Nothing
        Cache.Remove(sCacheKey)
        FormsAuthentication.SignOut()
        FormsAuthentication.SignOut()
        Request.Cookies.Remove(FormsAuthentication.FormsCookieName)
        Dim oAuthTicket As FormsAuthenticationTicket
        Dim sEncTicket As String
        Dim oAuthCookie As HttpCookie

        '-- Create Authentication Ticket
        oAuthTicket = New FormsAuthenticationTicket(1, HttpContext.Current.User.Identity.Name.ToString, System.DateTime.Now.ToUniversalTime, System.DateTime.Now.AddHours(-12).ToUniversalTime, False, "")
        sEncTicket = FormsAuthentication.Encrypt(oAuthTicket)
        oAuthCookie = New HttpCookie(FormsAuthentication.FormsCookieName, sEncTicket)
        HttpContext.Current.Response.Cookies.Add(oAuthCookie)

        Session.Abandon()
        Response.Redirect("~/default.aspx")
    End Sub

End Class
