Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.IO
Imports System.Drawing

Namespace appxCMSLib
    <Serializable()> _
    Public Class Carousel
        Inherits System.Collections.Generic.List(Of CarouselItem)

        Private _Height As Integer = 75
        Public Property Height() As Integer
            Get
                Return _Height
            End Get
            Set(ByVal value As Integer)
                _Height = value
            End Set
        End Property

        Private _Width As Integer = 75
        Public Property Width() As Integer
            Get
                Return _Width
            End Get
            Set(ByVal value As Integer)
                _Width = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal sCarouselXml As String)
            If File.Exists(sCarouselXml) Then
                Try
                    Dim oXml As New XmlDocument
                    oXml.Load(sCarouselXml)

                    Dim oCarousel As XmlNode = oXml.SelectSingleNode("//carousel")
                    For Each oItem As XmlNode In oCarousel.SelectNodes("item")
                        Dim bEnabled As Boolean = True
                        Dim sUrl As String = xmlhelp.ReadAttribute(oItem, "imageurl")
                        Dim sNav As String = xmlhelp.ReadAttribute(oItem, "navigateurl")
                        Dim sTitle As String = xmlhelp.ReadAttribute(oItem, "title")
                        Dim sEnabled As String = xmlhelp.ReadAttribute(oItem, "enabled")
                        If sEnabled = "0" Then
                            bEnabled = False
                        End If
                        If bEnabled Then
                            Dim oCItem As New CarouselItem(sUrl, sNav, sTitle)
                            Me.Add(oCItem)

                            If oCItem.Width > Me.Width Then
                                Me.Width = oCItem.Width
                            End If

                            If oCItem.Height > Me.Height Then
                                Me.Height = oCItem.Height
                            End If
                        End If
                    Next
                Catch ex As Exception
                    HttpContext.Current.Response.Write(ex.Message & "<br/>" & ex.StackTrace)
                End Try
            End If
        End Sub

        Public Overloads Sub Add(ByVal oItem As CarouselItem)
            If oItem.Width > Me.Width Then
                Me.Width = oItem.Width
            End If
            If oItem.Height > Me.Height Then
                Me.Height = oItem.Height
            End If
            MyBase.Add(oItem)
        End Sub
    End Class

    <Serializable()> _
    Public Class CarouselItem
        Private _ImageUrl As String = ""
        Public Property ImageUrl() As String
            Get
                Return _ImageUrl
            End Get
            Set(ByVal value As String)
                _ImageUrl = value
            End Set
        End Property

        Private _NavigateUrl As String = ""
        Public Property NavigateUrl() As String
            Get
                Return _NavigateUrl
            End Get
            Set(ByVal value As String)
                _NavigateUrl = value
            End Set
        End Property

        Private _Title As String = ""
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set
        End Property

        Private _Height As Integer = 75
        Public Property Height() As Integer
            Get
                Return _Height
            End Get
            Set(ByVal value As Integer)
                _Height = value
            End Set
        End Property

        Private _Width As Integer = 75
        Public Property Width() As Integer
            Get
                Return _Width
            End Get
            Set(ByVal value As Integer)
                _Width = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal sImage As String, ByVal sUrl As String, ByVal sTitle As String)
            Me.ImageUrl = sImage
            Me.NavigateUrl = sUrl
            Me.Title = sTitle

            If Me.ImageUrl.StartsWith("/") Then
                Dim sImagePath As String = HttpContext.Current.Server.MapPath(Me.ImageUrl)
                Try
                    Using oBmp As New Bitmap(sImagePath)
                        Me.Width = oBmp.Width
                        Me.Height = oBmp.Height
                    End Using
                Catch ex As Exception

                End Try
            End If
        End Sub
    End Class
End Namespace
