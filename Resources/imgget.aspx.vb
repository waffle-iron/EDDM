Imports PNDGraphics.CreateThumbNail
Imports System.Drawing
Imports System.Xml
Imports System.IO
Imports WebSupergoo.ImageGlue6
Imports log4net

Partial Class imgget
    Inherits System.Web.UI.Page

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private ReadOnly Property PUID() As Integer
        Get
            Try
                Return Integer.Parse(Request.QueryString("ID"))
            Catch ex As Exception
                Return -1
            End Try

        End Get
    End Property

    Private ReadOnly Property sSize() As Integer
        Get
            Try
                Return Integer.Parse(Request.QueryString("s"))
            Catch ex As Exception
                Return -1
            End Try
        End Get
    End Property

    Private ReadOnly Property ForReceipt() As Boolean
        Get
            If Request.QueryString("r") = "1" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Private ReadOnly Property fb() As String
        Get
            Return Request.QueryString("fb")
        End Get
    End Property

    Private Property ProfileUser() As String
        Get
            Return _ProfileUser.Trim()
        End Get
        Set(ByVal value As String)
            _ProfileUser = value
        End Set
    End Property
    Private _ProfileUser As String = ""

    Private bOldSkool As Boolean = False

    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Page.Theme = ""
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If PUID < 0 Then
            Return
        End If

        Dim sXML As String = ""
        'Using oA As New pndOrderTableAdapters.pnd_OrderItemTableAdapter
        '    Using oT As pndOrder.pnd_OrderItemDataTable = oA.GetOrderItem(PUID)
        '        If oT.Rows.Count > 0 Then
        '            Dim oTRow As pndOrder.pnd_OrderItemRow = oT.Rows(0)
        '            sXML = oTRow.XMLData
        '        End If
        '    End Using
        '    ProfileUser = oA.GetCustomerName(PUID)
        'End Using

        Dim oXML As New XmlDocument
        If sXML.StartsWith("<cart") Then
            oXML.LoadXml(sXML)
        Else
            oXML.LoadXml("<cart>" & sXML & "</cart>")
        End If

        Dim sClientBase As String = Taradel.WLUtil.GetRelativeSiteImagesPath & "/UserImages"
        If Not Directory.Exists(Server.MapPath(sClientBase)) Then
            Directory.CreateDirectory(Server.MapPath(sClientBase))
        End If
        Dim sClientFolder As String = sClientBase & "/" & Profile.UserName.Replace("@", "_")
        Dim sClientPath As String = Server.MapPath(sClientFolder)

        Dim oFileB() As Byte = Nothing
        Dim oFile As XmlNode
        oFile = oXML.SelectSingleNode("/cart/Product/Design/" & fb)
        Dim sFileRefType As String = xmlhelp.ReadAttribute(oFile, "DesignSelectionType")
        If oFile IsNot Nothing Then
            Select Case sFileRefType.ToLowerInvariant
                Case "multiad"

                Case "upload", "designdiy"
                    Dim sExt As String = xmlhelp.ReadAttribute(oFile, "filetype")
                    Dim sFileName As String = xmlhelp.ReadAttribute(oFile, "realfilename")
                    Dim sUser As String = ProfileUser.Replace("@", "_").Trim
                    Dim sFile As String = xmlhelp.ReadAttribute(oFile, "filename")
                    Dim sUserBase As String = sClientPath
                    Dim sFilePath As String = Path.Combine(sUserBase, sFileName) 'Server.MapPath("/siteimages/" & sUser & "/" & sFileName)
                    log.Info("Download File Path is: " & sFilePath)
                    If Not File.Exists(sFilePath) Then
                        log.Info("File not found for download at: " & sFilePath)
                        bOldSkool = True
                    End If

                    If bOldSkool Then
                        Dim oThumb As XmlNode = oFile.SelectSingleNode("thumb")
                        If oThumb IsNot Nothing Then
                            oFile.RemoveChild(oThumb)
                        End If
                        Dim sFileStuff As String = oFile.InnerText
                        Dim sFileExt As String = xmlhelp.ReadAttribute(oFile, "filetype") 'oFile.Attributes.GetNamedItem("filetype").Value

                        oFileB = System.Convert.FromBase64String(sFileStuff)

                    Else
                        Dim oFStream As FileStream = Nothing
                        Dim br As BinaryReader = Nothing
                        Try
                            oFStream = New FileStream(sFilePath, FileMode.Open, FileAccess.Read)
                            br = New BinaryReader(oFStream)
                            oFileB = br.ReadBytes(oFStream.Length)
                        Catch ex As Exception
                            Response.Write(ex.ToString)
                            Response.End()
                        Finally
                            br.Close()
                            oFStream.Close()
                            oFStream.Dispose()
                        End Try
                    End If
                    Response.Clear()
                    If (oFileB.Length > 0) Then
                        Try
                            Response.ContentType = "application/x-msdownload"
                            Response.AddHeader("content-disposition", "attachment; filename=" & Server.UrlEncode(sFile))
                            Response.BinaryWrite(oFileB)
                            Response.End()
                        Catch ex As Exception
                            Response.Write("<p>There was an error streaming the file.</p>")
                            Response.Write("<p>User File Name: " & sFile & "</p>")
                            Response.Write("<p>Storage Name: " & sFileName & "</p>")
                            Response.Write("<p><a href=""/siteimages/" & Server.UrlEncode(sUser) & "/" & Server.UrlEncode(sFileName) & """>Try a direct download</a></p>")
                        End Try
                    Else
                        Response.Write("<p>There was an error streaming the file.</p>")
                        Response.Write("<p>User File Name: " & sFile & "</p>")
                        Response.Write("<p>Storage Name: " & sFileName & "</p>")
                        Response.Write("<p><a href=""/siteimages/" & Server.UrlEncode(sUser) & "/" & Server.UrlEncode(sFileName) & """>Try a direct download</a></p>")
                    End If
            End Select
        Else
            Response.Write("Your must indicate the front/back to download.")
        End If

        ' Put user code to initialize the page here
    End Sub 'Page_Load

End Class
