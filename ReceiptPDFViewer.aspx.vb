Imports System.Web
Imports System.IO
Imports WebSupergoo.ABCpdf8


Partial Class ReceiptPDFViewer
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim orderGUID As String = Trim(Request.QueryString("guid"))
        Dim orderID As Integer = Convert.ToInt32(Request.QueryString("id"))


        Dim oOrder As Taradel.OrderHeader = Taradel.OrderDataSource.GetOrder(orderID)
        'Dim xOrderCalc As TaradelReceiptUtility.OrderCalculator = TaradelReceiptUtility.RetrieveOrderCalculator(orderID)

        If oOrder IsNot Nothing Then

            If oOrder.OrderGUID.Equals(orderGUID) Then

                Dim dOrderDate As DateTime = oOrder.Created.Value
                Dim userEmail As String = oOrder.EmailAddress.Replace("@", "_")
                Dim sPDFFile As String = Context.Server.MapPath("~/app_data/receipts/" & dOrderDate.ToString("yyyy") & "/" & dOrderDate.ToString("MM") & "/" & dOrderDate.ToString("dd") & "/" & "Order-" & orderID & "-Receipt.pdf")
                Dim oFi As New FileInfo(sPDFFile)

                Context.Response.Clear()
                Context.Response.ContentType = "application/pdf"
                Context.Response.AddHeader("Content-Disposition", "inline; filename=" + oFi.Name)
                Context.Response.AddHeader("Content-Length", oFi.Length.ToString())
                Context.Response.WriteFile(sPDFFile)
                Context.Response.End()

            Else
                Context.Response.Write("Order Receipt not found.")
            End If

        Else
            Context.Response.Write("Invalid file request")
        End If

    End Sub


End Class




