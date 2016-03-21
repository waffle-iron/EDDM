<%@ WebHandler Language="VB" Class="ProductConfigHelp" %>

Imports System
Imports System.Web
Imports System.Collections.Generic
Imports System.Linq

Public Class ProductConfigHelp : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim ProductId As Integer = appxCMS.Util.Querystring.GetInteger("pid")
        Dim BaseProductId As Integer = appxCMS.Util.Querystring.GetInteger("bpid")
        Dim CategoryId As Integer = appxCMS.Util.Querystring.GetInteger("catid")
        Dim PageRef As String = appxCMS.Util.Querystring.GetString("pageref")
        
        Dim sResponse As String = "Unable to load requested help file."
        Dim oSb As New StringBuilder
        If ProductId > 0 And CategoryId > 0 Then
            Dim sProdName As String = ""
            Dim sCatName As String = ""
            Dim oWLProd As Taradel.WLProduct = Taradel.WLProductDataSource.GetProduct(ProductId)
            If oWLProd IsNot Nothing Then
                sProdName = oWLProd.Name
            End If
                
            Dim oBaseProd As Taradel.Product = Taradel.ProductDataSource.GetEffectiveProduct(BaseProductId)
            If oBaseProd IsNot Nothing Then
                Dim iProdId As Integer = oBaseProd.ProductID
                Dim oProdOptCats As List(Of Taradel.ProductOptionCategory) = Taradel.ProductDataSource.GetProductOptionCategories(iProdId)
                Dim oOptCat As Taradel.ProductOptionCategory = oProdOptCats.FirstOrDefault(Function(po As Taradel.ProductOptionCategory) po.OptCatID = CategoryId)
                If oOptCat IsNot Nothing Then
                    oSb.AppendLine("<p>About the " & oOptCat.Name & " options for " & sProdName & ":</p>")
                    Using oDb As New Taradel.taradelEntities
                        Dim oOpts As IQueryable(Of Taradel.ProductOption) = (From o In oDb.ProductPrintMethodOptions _
                                                                                         Where o.Product.ProductID = iProdId _
                                                                                         And o.ProductOption.ProductOptionCategory.OptCatID = oOptCat.OptCatID _
                                                                                         And o.Deleted = False _
                                                                                         And o.ProductOption.Deleted = False _
                                                                                         Order By o.ProductOption.SortOrder _
                                                                                         Select o.ProductOption).Distinct

                        If oOpts.Count > 0 Then
                            oSb.AppendLine("<dl>")
                        End If
                        For Each oOpt As Taradel.ProductOption In oOpts
                            oSb.AppendLine("<dt>" & oOpt.Name & "</dt>")
                            oSb.AppendLine("<dd>")
                            If Not String.IsNullOrEmpty(oOpt.ZoomImage) Then
                                oSb.AppendLine("<img src=""http://www.taradel.com/siteimages/" & oOpt.ZoomImage & """ align=""left"" style=""padding-right:10px;"">")
                            End If
                            If String.IsNullOrEmpty(oOpt.Description) Then
                                oSb.AppendLine("Description coming soon. If you are still unsure, give us a call or use the online chat.")
                            Else
                                oSb.AppendLine(oOpt.Description)                               
                            End If
                            oSb.AppendLine("<br /><br /></dd>")
                        Next
                        If oOpts.Count > 0 Then
                            oSb.AppendLine("</dl>")
                        End If
                    End Using
                    sResponse = oSb.ToString
                End If
            End If
        ElseIf Not String.IsNullOrEmpty(PageRef) Then
            sResponse = "We have a page to locate content for"
            Dim oContentAreas As List(Of appxCMS.AppliedPageContent) = appxCMS.ContentDataSource.GetAppliedPageContent(PageRef, 1)
            Dim oContent As appxCMS.AppliedPageContent = oContentAreas.FirstOrDefault(Function(apc As appxCMS.AppliedPageContent) apc.PlaceHolder = "phBody")
            If oContent IsNot Nothing Then
                sResponse = oContent.Content
            End If
        End If
        
        context.Response.ContentType = "text/html"
        context.Response.Write(sResponse)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class