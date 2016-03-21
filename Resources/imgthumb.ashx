<%@ WebHandler Language="VB" Class="imgthumb" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.Drawing
Imports System.IO
Imports System.Web.Profile
Imports WebSupergoo.ImageGlue7
Imports log4net

Public Class imgthumb : Implements IHttpHandler
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Const NoPreview As String = "/cmsimages/nopreview.jpg"
    Private Const MaxFileSize As Integer = 41943040 '-- 40mb
    
    Private ReadOnly Property PUID() As String
        Get
            Dim sID As String = ""
            If HttpContext.Current.Request.QueryString("ID") IsNot Nothing Then
                sID = HttpContext.Current.Request.QueryString("ID")
            End If
            Return sID
        End Get
    End Property
    
    Private ReadOnly Property sSize() As Integer
        Get
            Try
                Return Integer.Parse(HttpContext.Current.Request.QueryString("s"))
            Catch ex As Exception
                Return -1
            End Try
        End Get
    End Property
    
    Private ReadOnly Property ForReceipt() As Boolean
        Get
            If HttpContext.Current.Request.QueryString("r") = "1" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    
    Private ReadOnly Property ForAutoGen() As Boolean
        Get
            If HttpContext.Current.Request.QueryString("ag") = "1" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    
    Private ReadOnly Property fb() As String
        Get
            Return HttpContext.Current.Request.QueryString("fb")
        End Get
    End Property
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim oThumb As New Taradel.Util.CreateThumbNail
        
        If String.IsNullOrEmpty(PUID) Then
            oThumb.ErrorResult(NoPreview)
            Return
        End If

        Dim Size As Integer = 200
        If sSize >= 0 Then
            Size = Int32.Parse(sSize)
        End If
        If Size < 200 Then
            Size = 200
        End If
        
        Dim oP As New ProfileCommon
        Dim oProfile As ProfileCommon
        Dim oXML As New XmlDocument
        Dim sUserId As String = ""
        
        If ForReceipt Then
            oProfile = oP.GetProfile(context.User.Identity.Name)
            sUserId = oProfile.UserName.Replace("@", "_")
            If oProfile.LastOrder.OuterXml.StartsWith("<cart") Then
                oXML.LoadXml(oProfile.LastOrder.OuterXml)
            Else
                oXML.LoadXml("<cart>" & oProfile.LastOrder.OuterXml & "</cart>")
            End If
        ElseIf ForAutoGen Then
            oProfile = oP.GetProfile(context.Request.QueryString("user"))
            sUserId = oProfile.UserName.Replace("@", "_")
            oXML.LoadXml(oProfile.LastOrder.OuterXml)
        Else
            If context.Request.IsAuthenticated Then
                sUserId = context.User.Identity.Name
            Else
                sUserId = context.Request.AnonymousID
            End If
                        
            oProfile = oP.GetProfile(sUserId)
          
            If oProfile IsNot Nothing Then
                If context.Request.IsAuthenticated Then
                    sUserId = oProfile.UserName.Replace("@", "_")
                End If

                Try
                    oXML.LoadXml(oProfile.Cart.OuterXml)
                Catch ex As Exception
                    log.Error(ex.Message, ex)
                    appHelp.LogErr(context.User.Identity.Name, context.Request.Url.PathAndQuery, appHelp.BuildErrString(ex, context))
                End Try
            Else
                oThumb.ErrorResult(NoPreview)
                Exit Sub
            End If
        End If
        
        Dim oFile As XmlNode = oXML.SelectSingleNode("//Product[@Index='" & PUID & "']/Design/" & fb & "/thumb")
        
        If oFile IsNot Nothing Then
            '-- We have a thumbnail already, just stream it back out
            Dim sThumb As String = xmlHelp.ReadNode(oFile)
            If Not String.IsNullOrEmpty(sThumb) Then
                Dim oFileB() As Byte = Convert.FromBase64String(sThumb)
                context.Response.ContentType = "image/jpeg"
                context.Response.BinaryWrite(oFileB)
                Exit Sub
            End If
        End If
        
        '-- Try to create the thumbnail from the uploaded file
        oFile = oXML.SelectSingleNode("//Product[@Index='" & PUID & "']/Design/" & fb)
        If oFile Is Nothing Then
            oThumb.ErrorResult(NoPreview)
            Exit Sub
        End If
        
        Dim sExt As String = xmlHelp.ReadAttribute(oFile, "filetype").ToLower
        Dim sFile As String = xmlHelp.ReadAttribute(oFile, "realfilename")
        Dim sOFile As String = xmlHelp.ReadAttribute(oFile, "filename")
        Dim sFilePath As String = context.Server.MapPath("/cmsimages/" & Taradel.WLUtil.GetSiteId & "/UserImages/" & sUserId & "/" & sFile)
        
        Dim oFI As New FileInfo(sFilePath)
        If Not oFI.Exists Then
            sFilePath = context.Server.MapPath("/cmsimages/" & Taradel.WLUtil.GetSiteId & "/UserImages/" & sUserId & "/" & xmlhelp.ReadAttribute(oFile, "filename"))
            oFI = New FileInfo(sFilePath)
            If Not oFI.Exists Then
                oThumb.ErrorResult(NoPreview)
                Exit Sub
            End If
        End If
        
        If oFI.Length > MaxFileSize Then
            oThumb.CustomErrorResult("File Size exceeds max allowed for preview", sOFile)
            'oThumb.ErrorResult(NoPreview)
            Exit Sub
        End If
        
        Dim oThumbBytes(0) As Byte
        Dim iPageCount As Integer = 1
        If sExt.EndsWith(".pdf") Then
            Dim drawOpts As New WebSupergoo.ImageGlue7.DrawOptions
            drawOpts.ImageFit = WebSupergoo.ImageGlue7.DrawOptions.ImageFitType.None
            drawOpts.Interpolation = WebSupergoo.ImageGlue7.DrawOptions.InterpolationType.Super
            drawOpts.RenderingIntent = WebSupergoo.ImageGlue7.DrawOptions.RenderingIntentType.Saturation
            drawOpts.Limit = New Size(200, 200)
            Using oImg As WebSupergoo.ImageGlue7.XImage = XImage.FromFile(sFilePath)
                If oImg.FrameCount > 1 Then
                    Dim oPg1 As Bitmap = Nothing
                    Dim oPg2 As Bitmap = Nothing
                    
                    '-- Get Page 1
                    oImg.Frame = 1
                    Using oCanvas As New WebSupergoo.ImageGlue7.Canvas()
                        Dim oExport As New WebSupergoo.ImageGlue7.XExport()
                        oExport.AntiAlias = True
                        oExport.Quality = 60
                        
                        oCanvas.DrawImage(oImg, drawOpts)
                        oPg1 = oCanvas.ToBitmap(".jpg")
                        'oPg1Bytes = oCanvas.GetAs("tmp.jpg", oExport)
                    End Using
                    
                    oImg.Frame = 2
                    Using oCanvas As New WebSupergoo.ImageGlue7.Canvas()
                        Dim oExport As New WebSupergoo.ImageGlue7.XExport()
                        oExport.AntiAlias = True
                        oExport.Quality = 60
                        
                        oCanvas.DrawImage(oImg, drawOpts)
                        oPg2 = oCanvas.ToBitmap(".jpg")
                        'oPg2Bytes = oCanvas.GetAs("tmp.jpg", oExport)
                    End Using
                    
                    Dim iOutW As Integer = Size
                    Dim iOutH As Integer = Size
                    Dim bLandscape As Boolean = False
                    If oPg1.Width > oPg1.Height Then
                        iOutW = oPg1.Width
                        iOutH = oPg1.Height * 2
                        bLandscape = True
                    Else
                        iOutW = oPg1.Width * 2
                        iOutH = oPg1.Height
                    End If
                    Using oBmp As New Bitmap(iOutW, iOutH)
                        Using oG As Graphics = Graphics.FromImage(oBmp)
                            oG.DrawImage(oPg1, New Rectangle(0, 0, oPg1.Width, oPg1.Height))
                            If bLandscape Then
                                '-- Draw below first image
                                oG.DrawImage(oPg2, New Rectangle(0, oPg1.Height + 1, oPg2.Width, oPg2.Height))
                            Else
                                '-- draw to right of first image
                                oG.DrawImage(oPg2, New Rectangle(oPg1.Width + 1, 0, oPg2.Width, oPg2.Height))
                            End If                            
                        End Using
                                            
                        Using oMOut As New MemoryStream
                            oBmp.Save(oMOut, Imaging.ImageFormat.Jpeg)
                            oThumbBytes = oMOut.GetBuffer()
                        End Using
                    End Using
                Else
                    oImg.Frame = 1
                    Using oCanvas As New WebSupergoo.ImageGlue7.Canvas()
                        Dim oExport As New WebSupergoo.ImageGlue7.XExport()
                        oExport.AntiAlias = True
                        oExport.Quality = 100
                        
                        oCanvas.DrawImage(oImg, drawOpts)
                        oThumbBytes = oCanvas.GetAs("tmp.jpg", oExport)
                    End Using
                End If
            End Using
        ElseIf sExt.EndsWith(".jpg") Then
            '-- Create a thumbnail of the graphic
            Dim oBmp As Bitmap = Taradel.Util.CreateThumbNail.CreateThumbnail(sFilePath, Size, Size)
            Using oMs As New MemoryStream
                oBmp.Save(oMs, System.Drawing.Imaging.ImageFormat.Jpeg)
                oThumbBytes = oMs.GetBuffer
            End Using
            oBmp.Dispose()
        Else
            '-- Composite a graphic that explains this extension cannot be previewed online
            'oThumb.CustomErrorResult("Extension not supported for preview", sOFile)
            '-- Try to get a thumbnail using shell extensions
            'Try
            '    Dim oBmp As Bitmap = PNDGraphics.CreateThumbNail.GetShellThumbnail(sFilePath, Size, 32)
            '    Using oMs As New MemoryStream
            '        oBmp.Save(oMs, System.Drawing.Imaging.ImageFormat.Jpeg)
            '        oThumbBytes = oMs.GetBuffer
            '    End Using
            'Catch ex As Exception
            '    context.Response.Write(ex.Message)
            '    context.Response.Write("<p>" & ex.StackTrace & "</p>")
            '    context.Response.End()
            '    'log.Error(ex.Message, ex)
            oThumb.CustomErrorResult("Extension not supported for preview", sOFile)
            'End Try            
        End If
        
        If oThumbBytes.Length < 2 Then
            oThumb.ErrorResult(NoPreview)
            Exit Sub
        End If
        
        Using oM As New MemoryStream(oThumbBytes)
            Dim oProduct As XmlNode = oXML.SelectSingleNode("/cart/Product[@Index='" & PUID & "']/Design/" & fb)
            If oProduct IsNot Nothing Then
                xmlHelp.AddOrUpdateXMLNode(oProduct, "thumb", oThumb.MemoryStreamToBase64(oM))
                If ForReceipt Then
                    oProfile.LastOrder = oXML
                Else
                    oProfile.Cart = oXML
                End If
                oProfile.Save()
            End If
            
            Using oBmp As Bitmap = Taradel.Util.CreateThumbNail.CreateThumbnail(oM, Size, Size)
                context.Response.Clear()
                context.Response.ContentType = "image/jpeg"
                oBmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            End Using
        End Using
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class