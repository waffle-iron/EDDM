Imports System.Collections.Generic
Imports System.Linq
Imports appxCMS

Partial Class EDDMQuote
    Inherits appxCMS.PageBase

    Protected oResponse As List(Of appxCMS.SurveyResponseItem) = Nothing
    Protected sProduct As String = ""
    Protected sAddress As String = ""
    Protected sZip As String = ""
    Protected sQty As String = ""
    Protected iQty As Integer = 0
    Protected sEmail As String = ""
    Protected oProd As Taradel.WLProduct = Nothing
    Protected sProdImg As String = ""
    Protected sQuote As String = ""



    '=================================================================================================================================================
    'NOTES:
    ' This QuoteConfirm page is designed to use with EDDM Quotes and AddressedList quotes.  If will detect the referring URL and change the page header
    ' accordingly (in Page_Init).
    '=================================================================================================================================================



    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init


        Dim SurveyResponseId As Integer = 0
        Dim siteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()


        If Request.Cookies("LastSurveyResponseId") IsNot Nothing Then
            Integer.TryParse(Request.Cookies("LastSurveyResponseId").Value, SurveyResponseId)
        End If


        If SurveyResponseId > 0 Then


            Using oDb As New appxCMS.appxCMSEntities
                oResponse = (From sr In oDb.SurveyResponses _
                            Where sr.SurveyResponseHeaderID = SurveyResponseId _
                            Select New SurveyResponseItem With { _
                                .Question = (From q In oDb.SurveyQuestions Where q.SurveyQuestionID = sr.SurveyQuestionID).FirstOrDefault.Question, _
                                .Response = sr.Response _
                            }).ToList
            End Using


            'Driven logic
            Dim DrivenCRMLeadId As Integer = Profile.DrivenCRMLeadId
            'hidden until further notice....5/8/2015
            If DrivenCRMLeadId > 0 Then
                pSamples.Visible = False
                pConsult.Visible = False
            End If




            sAddress = GetResponseItem(oResponse, "Street Address")
            sZip = GetResponseItem(oResponse, "Zip Code")
            sProduct = GetResponseItem(oResponse, "Products")
            sQty = GetResponseItem(oResponse, "Quantity")
            sEmail = GetResponseItem(oResponse, "Email Address")
            Integer.TryParse(sQty, iQty)

            'Uncomment out to see what was sent via form
            'For Each oRItem As appxCMS.SurveyResponseItem In oResponse
            'Response.Write(oRItem.Question & " = " & oRItem.Response & ControlChars.CrLf & "<br />")
            'Next

            'Determine what kind of Quote Request this is.
            Dim referralURL As String = Request.UrlReferrer.AbsoluteUri.ToString().ToLower()

            If (referralURL.Contains("addressed-list-quote-request")) Then
                litConfirmHeaderLg.Text = "Here is your <u>free</u> Instant Addressed List Mail® Quote!"
                litConfirmHeaderSm.Text = "Here is your <u>free</u> Instant Addressed List Mail® Quote!"
                hidQuoteType.Value = "AddressedList"
            Else
                litConfirmHeaderLg.Text = "Here is your <u>free</u> Instant Every Door Direct Mail® Quote!"
                litConfirmHeaderSm.Text = "Here is your <u>free</u> Instant Every Door Direct Mail® Quote!"
                hidQuoteType.Value = "EDDM"
            End If


            If Not String.IsNullOrEmpty(sProduct) Then

                oProd = Taradel.WLProductDataSource.GetProduct(sProduct)

                If oProd IsNot Nothing Then

                    If Not String.IsNullOrEmpty(oProd.Image) Then
                        Dim sCMSBase As String = Taradel.WLUtil.GetRelativeSiteImagesPath
                        sProdImg = sCMSBase & oProd.Image
                        imgProduct.ImageUrl = sProdImg
                    Else
                        pImage.Visible = False
                    End If


                    lProductDescription.Text = oProd.FullDescription
                    lQty.Text = iQty.ToString("N0")
                    lProduct.Text = oProd.Name
                    lEmail.Text = sEmail


                    Dim oSb As New StringBuilder
                    oSb.AppendLine("<table class=""table table-bordered table-hover""><thead>")
                    oSb.AppendLine("<tr><th class=""priceHeader"">Quantity</th><th class=""priceHeader"">Price/Piece</th><th class=""priceHeader"">Total</th></tr></thead>")
                    oSb.AppendLine("<tbody>")

                    Dim aQty As New SortedList(Of Integer, Integer)
                    aQty.Add(1000, 1000)
                    aQty.Add(2500, 2500)
                    aQty.Add(5000, 5000)
                    aQty.Add(10000, 10000)
                    aQty.Add(25000, 25000)
                    aQty.Add(50000, 50000)

                    If iQty > 0 Then
                        If Not aQty.ContainsKey(iQty) Then
                            aQty.Add(iQty, iQty)
                        End If
                    End If

                    Dim bHasOpts As Boolean = False

                    Dim oEnum = aQty.GetEnumerator
                    While oEnum.MoveNext
                        Dim qty As Integer = oEnum.Current.Value

                        If qty > 0 Then

                            Dim oQuote As New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, oProd.BaseProductID, qty, 0, 1, Nothing, "", False, False, oProd.Markup, oProd.MarkupType)
                            If qty = iQty Then
                                oSb.AppendLine("<tr class=""highlightRow"">")
                            Else
                                oSb.AppendLine("<tr>")
                            End If

                            oSb.AppendLine("<td>" & qty.ToString("N0") & "</td>")
                            oSb.AppendLine("<td class=""text-center"">" & oQuote.FormattedPricePerPiece & "</td>")
                            oSb.AppendLine("<td class=""text-center"">" & oQuote.FormattedPrice & "</td>")
                            oSb.AppendLine("</tr>")

                        End If
                    End While

                    oSb.AppendLine("</tbody></table>")
                    sQuote = oSb.ToString
                    lQuote.Text = sQuote

                    pnlSuccess.Visible = True


                Else

                    pnlError.Visible = True
                    pnlSuccess.Visible = False
                    litErrorMessage.Text = "Uh Oh!  Don't worry but something went wrong. Our IT Staff has been notified and you will be contacted very shortly about this error."

                    EmailUtility.SendAdminEmail("QuoteConfirm.aspx could not find the product. (oProd was null). SiteID is <strong>" & siteID & "</strong>. Attempt for Quote was made by " & sEmail & ".")

                End If

            End If

        End If


    End Sub



    Protected Function GetResponseItem(oResponse As List(Of appxCMS.SurveyResponseItem), sQuestion As String) As String
        Dim sRet As String = ""
        Dim oRet As appxCMS.SurveyResponseItem = oResponse.FirstOrDefault(Function(sri As appxCMS.SurveyResponseItem) sri.Question.Equals(sQuestion, System.StringComparison.OrdinalIgnoreCase))
        If oRet IsNot Nothing Then
            sRet = oRet.Response
        End If
        Return sRet
    End Function



    Protected Sub lnkGoToMap_Click(sender As Object, e As System.EventArgs) Handles lnkGoToMap.Click

        Dim bSampleKit As Boolean = chkGetSamples.Checked
        Dim bConsult As Boolean = chkConsult.Checked
        Dim bEmail As Boolean = chkEmail.Checked

        Dim DrivenLeadId As Integer = Profile.DrivenCRMLeadId

        If (bSampleKit Or bConsult) And DrivenLeadId > 0 Then

            '-- Post the data to the driven handler
            Dim sBaseUrl As String = appxCMS.Util.AppSettings.GetString("DrivenCRMUrl")

            If Not String.IsNullOrEmpty(sBaseUrl) Then

                If Not sBaseUrl.EndsWith("/") Then
                    sBaseUrl = sBaseUrl & "/"
                End If

                Dim sUrl As String = sBaseUrl & "TResources/LeadUpdater.ashx"
                Dim aPostData As New Hashtable
                aPostData.Add("LeadId", DrivenLeadId)
                If bSampleKit Then
                    aPostData.Add("Sample Requested", "YES")
                End If
                If bConsult Then
                    aPostData.Add("Marketing Consult Requested", "YES")
                End If
                Dim sResponse As String = appxCMS.Util.httpHelp.XMLURLPageRequest(sUrl, aPostData, "POST")

                If sResponse.Trim = "1" Then
                    '-- Good, the data was sent and saved
                Else
                    '-- Hmmm... we got a bad response while updating the lead
                    Response.Write(sResponse)
                    Response.End()
                End If

            End If

        End If

        Dim sAddr As String = GetResponseItem(oResponse, "Street Address")
        Dim sZip As String = GetResponseItem(oResponse, "Zip Code")

        If bEmail Then

            '-- Send this quote to them via e-mail
            If oResponse IsNot Nothing Then

                Dim sEmail As String = GetResponseItem(oResponse, "Email Address")
                Dim sFullName As String = GetResponseItem(oResponse, "First Name")

                If String.IsNullOrEmpty(sFullName) Then
                    sFullName = "Visitor"
                End If

                sQuote = sQuote.Replace("class=""highlight""", "style=""font-weight:bold; background-color:yellow; font-size:1.2em;""")
                sQuote = sQuote.Replace("<table", "<table style=""font-family:calibri,candara,segoe,'segoe ui',optima,arial,sans-serif;font-size:14px;""")
                sQuote = sQuote.Replace("<th class=""left"">", "<th style=""background-color:red; color:#FFFFFF; text-transform: uppercase; padding: 5px;text-align:left;"">")
                sQuote = sQuote.Replace("<th>", "<th style=""background-color:red; color:#FFFFFF; text-transform: uppercase; padding: 5px;"">")
                sQuote = sQuote.Replace("class=""center""", "style=""text-align:center;""")

                If Not String.IsNullOrEmpty(sEmail) Then

                    '-- Send it
                    Dim oMsg As New appxCMS.appxMessage
                    oMsg.MessageArgs.Add("emailaddress", sEmail)
                    oMsg.MessageArgs.Add("fullname", sFullName)
                    If Not String.IsNullOrEmpty(sProdImg) Then
                        oMsg.MessageArgs.Add("prodimage", "<img src=""http://" & Request.Url.Host & sProdImg & """ alt=""" & sProduct & """/>")
                    Else
                        oMsg.MessageArgs.Add("prodimage", "")
                    End If
                    oMsg.MessageArgs.Add("mailqty", iQty)
                    oMsg.MessageArgs.Add("productname", sProduct)
                    oMsg.MessageArgs.Add("proddescription", oProd.FullDescription)
                    oMsg.MessageArgs.Add("quotetable", sQuote)
                    oMsg.MessageArgs.Add("targetlink", "http://" & Request.Url.Host & "/Step1-Target.aspx?addr=" & sAddr & "&zip=" & sZip)

                    If (hidQuoteType.Value = "AddressedList") Then
                        appxCMS.Messaging.SendEmail(oMsg, "Addressed List Quote")
                    Else
                        appxCMS.Messaging.SendEmail(oMsg, "EDDM Quote")
                    End If

                End If

            End If

        End If


        If (hidQuoteType.Value = "AddressedList") Then
            Response.Redirect("~/Addressed-Overview")
        Else
            If Not String.IsNullOrEmpty(sAddr) And Not String.IsNullOrEmpty(sZip) Then
                Response.Redirect("~/Step1-Target.aspx?addr=" & Server.UrlEncode(sAddr) & "&zip=" & Server.UrlEncode(sZip) & "&lid=" & DrivenLeadId)
            Else
                Response.Redirect("~/Step1-Target.aspx")
            End If
        End If



    End Sub


End Class
