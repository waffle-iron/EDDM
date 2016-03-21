Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Text

Namespace EZCustomWebParts
    Public Class ezWebPartsChrome
        Inherits WebPartChrome

        Public Sub New(ByVal zone As System.Web.UI.WebControls.WebParts.WebPartZoneBase, ByVal manager As System.Web.UI.WebControls.WebParts.WebPartManager)
            MyBase.New(zone, manager)
        End Sub

        Protected Shadows ReadOnly Property Zone() As ezWebPartsZone
            Get
                Return MyBase.Zone
            End Get
        End Property

        Private Sub RenderButton(ByVal writer As System.Web.UI.HtmlTextWriter, ByVal clientID As String, ByVal buttonName As String, ByVal webPart As WebPart, Optional ByVal MethodName As String = "", Optional ByVal Display As String = "block")
            writer.AddStyleAttribute("width", "16px")
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "hand")
            writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("{0}{1}", clientID, buttonName))
            writer.RenderBeginTag(HtmlTextWriterTag.Span)
            Dim img As New HtmlImage
            img.Src = String.Format("images/{0}.gif", buttonName)
            img.Attributes.Add("onmouseover", String.Format("this.src='images/{0}.gif';", buttonName))
            img.Attributes.Add("onmouseout", String.Format("this.src='images/{0}.gif';", buttonName))
            img.Style.Add("display", Display)
            Dim sAlt As String = MethodName
            If String.IsNullOrEmpty(sAlt) Then
                sAlt = buttonName.ToUpper
            End If
            img.Attributes.Add("alt", sAlt)
            If buttonName = "verbs" Then
                img.Attributes.Add("id", String.Format("{0}VerbsPopup", clientID))
            Else
                img.Attributes.Add("onclick", String.Format("__wpm.SubmitPage('{0}', '{2}:{1}');", Zone.ClientID.Replace("_"c, "$"c), webPart.ID, buttonName))
            End If
            img.RenderControl(writer)
            writer.RenderEndTag() 'Span
            writer.RenderEndTag() 'TD
        End Sub

        Private Sub RenderTitleBar(ByVal writer As System.Web.UI.HtmlTextWriter, ByVal webPart As System.Web.UI.WebControls.WebParts.WebPart)
            Dim clientID As String = Me.GetWebPartChromeClientID(webPart)
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "16px")
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%")
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#CFCFCF")
            writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold")
            writer.RenderBeginTag(HtmlTextWriterTag.Table)
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)

            'Title Icon
            If Me.Zone.ShowTitleIcons Then
                If Not String.IsNullOrEmpty(webPart.TitleIconImageUrl) Then
                    writer.AddStyleAttribute("width", "1px")
                    writer.RenderBeginTag(HtmlTextWriterTag.Td)
                    Dim imgIcon As New HtmlImage()
                    imgIcon.Src = webPart.TitleIconImageUrl
                    imgIcon.RenderControl(writer)
                    writer.RenderEndTag() 'TD
                End If
            End If

            'Title Text
            writer.AddAttribute(HtmlTextWriterAttribute.Id, Me.GetWebPartTitleClientID(webPart))
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            If Not String.IsNullOrEmpty(webPart.TitleUrl) Then
                Dim anchorTitle As New HtmlAnchor()
                anchorTitle.HRef = webPart.TitleUrl
                anchorTitle.InnerHtml = webPart.Title & " - " & webPart.Subtitle
                anchorTitle.RenderControl(writer)
            Else
                writer.Write(webPart.Title)
            End If
            writer.RenderEndTag() 'TD

            'Verbs Button
            RenderButton(writer, clientID, "verbs", webPart, "Options", "none")

            'Minimize/Restore Button
            If webPart.ChromeState = PartChromeState.Minimized Then
                RenderButton(writer, clientID, "restore", webPart, "Restore")
            Else
                RenderButton(writer, clientID, "minimize", webPart, "Minimize")
            End If

            'Close Button
            RenderButton(writer, clientID, "close", webPart, "Close")
            writer.RenderEndTag() 'TR
            writer.RenderEndTag() 'Table

            'Chrome Verbs Pop-up Menu
            writer.AddAttribute("id", String.Format("{0}verbsMenu", clientID))
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none")
            writer.RenderBeginTag(HtmlTextWriterTag.Table)
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            For Each verb As WebPartVerb In webPart.Verbs
                RenderVerb(writer, webPart, verb)
            Next
            writer.RenderEndTag() 'TD
            writer.RenderEndTag() 'TR
            writer.RenderEndTag() 'Table

            'Write Client Script to Handle Pop-up Menu
            Dim scriptBuilder As New StringBuilder()
            scriptBuilder.AppendLine("<script type='text/javascript'>")
            scriptBuilder.AppendLine(String.Format("var menu{0}Verbs = new WebPartMenu(document.getElementById('{0}verbs'), document.getElementById('{0}verbsPopup'), document.getElementById('{0}verbsMenu'));", clientID))
            scriptBuilder.AppendLine("</script>")
            webPart.Page.ClientScript.RegisterStartupScript(Me.GetType(), clientID, scriptBuilder.ToString())
        End Sub

        Private Sub RenderVerb(ByVal writer As System.Web.UI.HtmlTextWriter, ByVal webpart As WebPart, ByVal verb As System.Web.UI.WebControls.WebParts.WebPartVerb)
            writer.RenderBeginTag(HtmlTextWriterTag.Div)
            writer.AddAttribute("onclick", String.Format("document.body.__wpm.SubmitPage('{0}', 'partverb:{1}:{2}');", Zone.ID, verb.ID, webpart.ID))
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#nogo")
            writer.AddStyleAttribute("white-space", "nowrap")
            writer.RenderBeginTag(HtmlTextWriterTag.A)
            If Not String.IsNullOrEmpty(verb.ImageUrl) Then
                Dim img As New Image()
                img.ImageUrl = verb.ImageUrl
                img.Width = New Unit(16)
                img.Height = New Unit(16)
                img.RenderControl(writer)
            End If
            writer.Write("&nbsp;" & verb.Text & "&nbsp;")
            writer.RenderEndTag() 'A
            writer.RenderEndTag() 'DIV
        End Sub

        Public Overrides Sub RenderWebPart(ByVal writer As System.Web.UI.HtmlTextWriter, ByVal webPart As System.Web.UI.WebControls.WebParts.WebPart)
            If (webPart Is Nothing) Then
                Throw New ArgumentNullException("webPart")
            End If
            Dim _webPart As WebPart = webPart
            Dim flag As Boolean = (Me.Zone.LayoutOrientation = Orientation.Vertical)
            Dim chromeType As PartChromeType = Me.Zone.GetEffectiveChromeType(webPart)
            Dim style As Style = Me.CreateWebPartChromeStyle(webPart, chromeType)
            If Not style.IsEmpty Then
                style.AddAttributesToRender(writer, Me.Zone)
            End If
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2")
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0")
            If flag Then
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%")
            ElseIf (webPart.ChromeState <> PartChromeState.Minimized) Then
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%")
            End If
            If Me.Zone.RenderClientScript Then
                'This ID is important for the draggability.
                writer.AddAttribute(HtmlTextWriterAttribute.Id, Me.GetWebPartChromeClientID(webPart))
            End If

            writer.RenderBeginTag(HtmlTextWriterTag.Table)

            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            RenderTitleBar(writer, webPart)
            writer.RenderEndTag() 'TD
            writer.RenderEndTag() 'TR

            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            If webPart.ChromeState = PartChromeState.Minimized Then
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none")
            End If
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            RenderPartContents(writer, webPart)
            writer.RenderEndTag() 'TD
            writer.RenderEndTag() 'TR

            writer.RenderEndTag() 'Table
        End Sub

    End Class
End Namespace
