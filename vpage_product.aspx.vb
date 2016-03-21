Imports System.Collections.Generic
Imports System.Linq

Partial Class vpage_product
    Inherits appxCMS.PageBase

    Protected ReadOnly Property ProductId As Integer
        Get
            Return QStringToInt("productid")
        End Get
    End Property

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Me.ProductId > 0 Then
            Dim oProd As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(Me.ProductId)
            If oProd IsNot Nothing Then
                lProductName.Text = oProd.Name
                If Not String.IsNullOrEmpty(oProd.Image) Then
                    Dim sImg As String = ""
                    Dim sCMSBase As String = Taradel.WLUtil.GetRelativeSiteImagesPath
                    sImg = sCMSBase & oProd.Image
                    imgProduct.ImageUrl = sImg
                Else
                    pImage.Visible = False
                End If
                lProductDescription.Text = oProd.FullDescription
                pNoProd.Visible = False
                pProduct.Visible = True
                Page.Title = oProd.Name

                SamplePricing.ProductId = oProd.BaseProductID
            End If
        End If

        If pNoProd.Visible Then
            Dim SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId
            Dim CategoryId As Integer = 0
            Using oDb As New Taradel.taradelEntities
                Dim oCat As Taradel.WLCategory = (From c In oDb.WLCategories _
                                                  Where c.Name.ToLower.Contains("eddm") _
                                                  And c.appxSite.SiteId = SiteID).FirstOrDefault

                If oCat IsNot Nothing Then
                    CategoryId = oCat.CategoryID
                End If
            End Using

            Dim oCatProds As List(Of Taradel.WLProductCategory) = Taradel.WLCategoryDataSource.GetProductsInCategory(CategoryId)
            rProds.DataSource = oCatProds
            rProds.DataBind()
            Page.Title = "Choose a Product"
        End If
    End Sub
End Class
