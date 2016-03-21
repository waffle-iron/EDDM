Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

Partial Class vpage_template
    Inherits appxCMS.PageBase


    'Properties
    Protected ReadOnly Property TemplateId As Integer
        Get
            Return appxCMS.Util.Querystring.GetInteger("tid")
        End Get
    End Property

    Private _BusinessLineId As Integer = 0
    Protected Property BusinessLineId As Integer
        Get
            If _BusinessLineId = 0 Then
                _BusinessLineId = appxCMS.Util.Querystring.GetInteger("bid")
            End If
            Return _BusinessLineId
        End Get
        Set(value As Integer)
            _BusinessLineId = value
        End Set
    End Property

    Private _IndustryId As Integer = 0
    Protected Property IndustryId As Integer
        Get
            If _IndustryId = 0 Then
                _IndustryId = appxCMS.Util.Querystring.GetInteger("iid")
            End If
            Return _IndustryId
        End Get
        Set(value As Integer)
            _IndustryId = value
        End Set
    End Property


    Private _Template As TemplateCode.Template1 = Nothing
    Private bLoaded As Boolean = False
    Private TemplateSizeId As Integer = 0

    Protected ReadOnly Property Template As TemplateCode.Template1
        Get
            If _Template Is Nothing AndAlso Not bLoaded Then
                Using oAPI As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetTemplateRequest(appxCMS.Util.CMSSettings.GetSiteId, Me.TemplateId)
                    Dim oResponse As TemplateCode.GetTemplateResponse = oAPI.GetTemplate(oRequest)
                    _Template = oResponse.GetTemplateResult
                    bLoaded = True
                End Using
            End If
            Return _Template
        End Get
    End Property

    Private _BusinessLine As TemplateCode.BusinessLine = Nothing
    Private bBLLoaded As Boolean = False
    Protected ReadOnly Property BusinessLine As TemplateCode.BusinessLine
        Get
            If _BusinessLine Is Nothing AndAlso Not bBLLoaded And Me.BusinessLineId > 0 Then
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetBusinessLineRequest(appxCMS.Util.CMSSettings.GetSiteId, Me.BusinessLineId)
                    Dim oResponse As TemplateCode.GetBusinessLineResponse = oTemplates.GetBusinessLine(oRequest)
                    _BusinessLine = oResponse.GetBusinessLineResult
                    If _BusinessLine IsNot Nothing Then
                        IndustryId = _BusinessLine.IndustryId
                    End If
                End Using
                bBLLoaded = True
            End If
            Return _BusinessLine
        End Get
    End Property

    Private _Industry As TemplateCode.Industry = Nothing
    Private bIndLoaded As Boolean = False
    Protected ReadOnly Property Industry As TemplateCode.Industry
        Get
            If _Industry Is Nothing AndAlso Not bIndLoaded AndAlso Me.IndustryId > 0 Then
                Using oTemplates As New TemplateCode.TemplateAPIClient
                    Dim oRequest As New TemplateCode.GetIndustryAndBusinessLinesRequest(Me.IndustryId, appxCMS.Util.CMSSettings.GetSiteId)
                    Dim oResponse As TemplateCode.GetIndustryAndBusinessLinesResponse = oTemplates.GetIndustryAndBusinessLines(oRequest)
                    _Industry = oResponse.GetIndustryAndBusinessLinesResult
                End Using
                bLoaded = True
            End If
            Return _Industry
        End Get
    End Property

    'Protected BusinessLineId As Integer = 0
    'Protected IndustryId As Integer = 0
    Protected sTemplateServerHost As String = ""



    'Methods
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        'Hide Taradel content if needed.
        Dim SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

        If (SiteDetails.HideTaradelContent) Then
            pnlTaradelContent.Visible = False
            pnlGenericContent.Visible = True
        Else
            pnlTaradelContent.Visible = True
            pnlGenericContent.Visible = False
        End If


        sTemplateServerHost = appxCMS.Util.AppSettings.GetString("TemplateServerHost")
        Dim oMeta As New HtmlMeta
        oMeta.Name = "robots"
        oMeta.Content = "NOINDEX, NOFOLLOW"
        Page.Header.Controls.Add(oMeta)

        If Me.Template IsNot Nothing Then
            Me.TemplateSizeId = Me.Template.TemplateSizeId
            lTemplateIdTop.Text = Me.Template.TemplateId
            lTemplateId.Text = Me.Template.TemplateId
            lSize.Text = Me.Template.PageSize
            Me.BusinessLineId = Me.Template.BusinessLineId
            ShowImage(hplFront, imgFront, Me.Template.FrontImage)
            ShowImage(hplInside, imgInside, Me.Template.InsideImage)
            ShowImage(hplBack, imgBack, Me.Template.BackImage)

            'BannerFull.FullText = "Template #" & Me.Template.TemplateId

            If Me.BusinessLine IsNot Nothing And Me.Industry IsNot Nothing Then
                lIndustryName.Text = Me.Industry.Name
                lBusinessLines.Text = Me.BusinessLine.Name

                'Commented out 11/2/15.  Possibly obsolete.
                'hplReturn.NavigateUrl = appxCMS.SEO.Rewrite.BuildLink(Me.Industry.Name & "-" & Me.BusinessLine.Name, Me.BusinessLine.IndustryId & "-" & Me.BusinessLineId, "templatebusinessline")

                Page.Title = "Customizable Every Door Direct Mail&tm; Template #" & Me.Template.TemplateId & " for " & Me.Industry.Name & ", " & Me.BusinessLine.Name


            Else
                'Commented out 11/2/15.  Possibly obsolete.
                'hplReturn.Visible = False

                Page.Title = "Customizable Every Door Direct Mail&tm; Template #" & Me.TemplateId
            End If
        End If
    End Sub


    Protected Sub ShowImage(oHpl As HyperLink, oImg As Image, sImg As String)
        If Not String.IsNullOrEmpty(sImg) Then
            oHpl.NavigateUrl = sTemplateServerHost & "/templates/full/" & sImg
            oHpl.Attributes.Add("rel", "prettyphoto[template]")
            oImg.ImageUrl = sTemplateServerHost & "/templates/thumb/" & sImg
        Else
            oHpl.Visible = False
            oImg.Visible = False
        End If
    End Sub


    Protected Sub lnkSelect_Click(sender As Object, e As System.EventArgs) Handles lnkSelect.Click
        Dim bUSelect As Boolean = False
        Dim iQty As Integer = 500
        Dim uSelectID As Integer = 0

        '-- Check the underlying product for USelect definition
        Dim oTemplateProd As Taradel.WLProduct = Taradel.WLUtil.GetProductBySize(Me.TemplateSizeId)
        Dim newUSelectID As Integer = RetrieveUSelectID(Me.TemplateSizeId)

        If oTemplateProd IsNot Nothing Then
            Dim productId As Integer = oTemplateProd.BaseProductID.Value
            '-- Check for USelect config
            Dim oUSelect As List(Of Taradel.USelectProductConfiguration) = Taradel.ProductDataSource.GetUSelectConfigurations(productId)

            For Each uselect As Taradel.USelectProductConfiguration In oUSelect
                uSelectID = uselect.USelectMethod.USelectId
            Next


            If oUSelect.Count() > 0 Then
                bUSelect = True
            Else
                '-- Get the minimum order quantity for this product
                Dim oMinRange As Taradel.PriceMatrix = Taradel.ProductDataSource.GetMinRange(productId)
                If oMinRange IsNot Nothing Then
                    iQty = oMinRange.MinQty
                End If
            End If
        End If

        If newUSelectID = 5 Or newUSelectID = 6 Then
            Response.Cookies.Add(New HttpCookie("DesignOption", "Template"))
            Response.Cookies.Add(New HttpCookie("SelectedTemplate", Me.TemplateId))
            Response.Cookies.Add(New HttpCookie("SelectedTemplateNew", Me.TemplateId))
            Response.Redirect("~/Addressed-Overview")
        End If


        If bUSelect Then
            Response.Cookies.Add(New HttpCookie("DesignOption", "Template"))
            Response.Cookies.Add(New HttpCookie("SelectedTemplate", Me.TemplateId))
            Response.Redirect("~/Step1-Target.aspx")
            'Response.Write("~/Step1-Target.aspx")
        Else
            Response.Cookies.Add(New HttpCookie("DesignOption", "Template"))
            Response.Cookies.Add(New HttpCookie("SelectedTemplate", Me.TemplateId))
            'Response.Redirect(appxCMS.SEO.Rewrite.BuildLink(oTemplateProd.Name, oTemplateProd.ProductID.ToString(), "p"))
            '-->Response.Redirect("~/Step2-Product.aspx?productid=" & oTemplateProd.ProductID & "&qty=" & iQty)
            Response.Redirect(("~/Step2-Product.aspx?productid=" & oTemplateProd.ProductID & "&qty=" & iQty))
        End If
    End Sub


    Public Function RetrieveUSelectID(TemplateSizeId As Integer) As String
        Dim returnThis As String = String.Empty

        Dim connectString As String = ConfigurationManager.ConnectionStrings("TaradelWLConnectionString").ConnectionString.ToString()
        Dim connectionObj As New SqlConnection(connectString)

        Dim sql As String = "SELECT TOP 1 USelectID FROM USelectProductConfiguration WHERE ProductID IN (SELECT ProductID FROM pnd_Product where productid IN (select TOP 1 ProductID from pnd_product where TemplateSizeId = " & TemplateSizeId.ToString() & "))"

        Dim command As New SqlCommand()
        command.CommandType = CommandType.Text
        command.CommandText = sql
        command.Connection = connectionObj
        Try
            connectionObj.Open()
            returnThis = command.ExecuteScalar().ToString()
        Catch objException As Exception
            Response.Write(objException.Message.ToString())
            Response.Write("__________________________________________")
            Response.Write(objException.StackTrace.ToString())
        End Try

        Return returnThis
    End Function

End Class
