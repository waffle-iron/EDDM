
Partial Class CCustom_HubspotCallToActionButton
    Inherits CLibraryBase

    Private _ctaId As String = ""
    Public Property CTAId As String
        Get
            Return _ctaId
        End Get
        Set(value As String)
            _ctaId = value
        End Set
    End Property

    Private _hubspotId As String = ""
    Public Property HubspotId As String
        Get
            Return _hubspotId
        End Get
        Set(value As String)
            _hubspotId = value
        End Set
    End Property
    
    Protected Overrides Sub BuildControl()
        Dim oSb As New StringBuilder
        oSb.AppendLine("<!--HubSpot Call-to-Action Code -->")
        oSb.AppendLine("<span class=""hs-cta-wrapper"" id=""hs-cta-wrapper-" & CTAId & """>")
        oSb.AppendLine("    <span class=""hs-cta-node hs-cta-" & CTAId & """ id=""hs-cta-" & CTAId & """>")
        oSb.AppendLine("        <!--[if lte IE 8]><div id=""hs-cta-ie-element""></div><![endif]-->")
        oSb.AppendLine("        <a href=""http://cta-redirect.hubspot.com/cta/redirect/" & HubspotId & "/" & CTAId & """><img class=""hs-cta-img"" id=""hs-cta-img-" & CTAId & """ style=""border-width:0px;"" src=""https://no-cache.hubspot.com/cta/default/" & HubspotId & "/" & CTAId & ".png"" /></a>")
        oSb.AppendLine("    </span>")
        oSb.AppendLine("    <script charset=""utf-8"" src=""https://js.hscta.net/cta/current.js""></script>")
        oSb.AppendLine("        <script type=""text/javascript"">")
        oSb.AppendLine("            hbspt.cta.load(" & HubspotId & ", '" & CTAId & "');")
        oSb.AppendLine("        </script>")
        oSb.AppendLine("</span>")
        oSb.AppendLine("<!-- end HubSpot Call-to-Action Code -->")
        lCTA.Text = oSb.ToString()
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub
End Class
