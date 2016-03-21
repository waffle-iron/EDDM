<%@ WebHandler Language="VB" Class="PrintRateQuote" %>

Imports System
Imports System.Web
Imports System.Collections.Generic
Public Class PrintRateQuote : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim ProductId As Integer = appxCMS.Util.Form.GetInteger("pid")
        Dim Quantity As Integer = appxCMS.Util.Form.GetInteger("qty")
        Dim HoldQuantity As Integer = appxCMS.Util.Form.GetInteger("hqty")
        Dim ShipQuantity As Integer = appxCMS.Util.Form.GetInteger("sqty")
        Dim DistributionId As Integer = appxCMS.Util.Form.GetInteger("distid")
        Dim DropCount As Integer = appxCMS.Util.Form.GetInteger("drops")
        Dim ZipCode As String = appxCMS.Util.Form.GetString("zip")
        Dim WithDesign As Boolean = appxCMS.Util.Form.GetBoolean("wd")
        Dim WithTemplate As Boolean = appxCMS.Util.Form.GetBoolean("wt")
        'Dim Markup As Decimal = appxCMS.Util.Form.GetDecimal("m")
        'Dim MarkupType As String = appxCMS.Util.Form.GetString("mt")
        Dim Markup As Decimal = 0
        Dim MarkupType As String = ""
        Markup = appxCMS.Util.CMSSettings.GetDecimal("Catalog", "Markup")
        MarkupType = appxCMS.Util.CMSSettings.GetSetting("Catalog", "MarkupType")
        '-- Check for product-level markup
        Dim oProd As Taradel.WLProduct = Taradel.WLProductDataSource.GetProductByBaseId(ProductId)
        If oProd IsNot Nothing Then
            If oProd.Markup > 0 Then
                Markup = oProd.Markup
                MarkupType = oProd.MarkupType
            End If
        End If
        
        If Markup > 0 AndAlso MarkupType.ToLower = "percent" Then
            If Markup > 1 Then
                Markup = Markup / 100
            End If
        End If

        Dim Options As New SortedList(Of Integer, Integer)
        Dim oFieldsEnum As IEnumerator = context.Request.Form.GetEnumerator()
        While oFieldsEnum.MoveNext
            Dim sFld As String = oFieldsEnum.Current.ToString
            If sFld.StartsWith("opt") Then
                Dim OptCatId As Integer = 0
                Integer.TryParse(sFld.Substring(3), OptCatId)
                Dim OptionId As Integer = 0
                Integer.TryParse(context.Request.Form(sFld), OptionId)
                Options.Add(OptCatId, OptionId)
            End If
        End While
        
        Dim oResult As New Taradel.ProductPriceQuote(appxCMS.Util.CMSSettings.GetSiteId, ProductId, Quantity, HoldQuantity, ShipQuantity, DistributionId, DropCount, Options, ZipCode, WithDesign, WithTemplate, Markup, MarkupType)
        oResult.SelectedOptions.Clear()
        
        
        
        context.Response.Clear()
        context.Response.ContentType = "application/json"
        context.Response.Write(Taradel.JavascriptSerializer.Serialize(Of Taradel.ProductPriceQuote)(oResult))
        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class