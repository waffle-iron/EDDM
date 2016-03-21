Imports log4net
Partial Class CCustom_WistiaVideoEmbed
    Inherits CLibraryBase

    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _mediaHashedId As String = ""
    Public Property MediaHashedId As String
        Get
            Return _mediaHashedId
        End Get
        Set(value As String)
            _mediaHashedId = value
        End Set
    End Property

    Private _width As Integer = 0
    Public Property Width As Integer
        Get
            Return _width
        End Get
        Set(value As Integer)
            _width = value
        End Set
    End Property

    Private _height As Integer = 0
    Public Property Height As Integer
        Get
            Return _height
        End Get
        Set(value As Integer)
            _height = value
        End Set
    End Property




    Protected Overrides Sub BuildControl()

        Dim sProtocol As String = IIf(Request.IsSecureConnection, "HTTPS", "HTTP").ToString
        Dim sCk As String = "Wistia-" & MediaHashedId & "-" & sProtocol & "-" & Width & "-" & Height
        Dim sEmbedCode As String = appxCMS.Util.Cache.GetString(sCk)

        If String.IsNullOrEmpty(sEmbedCode) Then
            Dim sEmbedUrl As String = "http://home.wistia.com/medias/" & MediaHashedId & "?embedType=seo"
            If sProtocol = "HTTPS" Then
                sEmbedUrl = sEmbedUrl & "&ssl=true"
            End If
            If Width > 0 Then
                sEmbedUrl = sEmbedUrl & "&width=" & Width
            End If
            If Height > 0 Then
                sEmbedUrl = sEmbedUrl & "&height=" & Height
            End If

            Dim sRequestUrl As String = "http://fast.wistia.com/oembed.json"

            Dim aRequest As New Hashtable
            aRequest.Add("url", sEmbedUrl)

            Dim sResponse As String = appxCMS.Util.httpHelp.GetXMLURLPage(sRequestUrl, aRequest, Nothing)
            Dim oResponse As WistiaResponse = Nothing
            Try
                oResponse = appxCMS.Util.JavaScriptSerializer.Deserialize(Of WistiaResponse)(sResponse)
            Catch ex As Exception
                Log.Error(ex.Message, ex)
                Me.Visible = False
                Exit Sub
            End Try
            If oResponse IsNot Nothing Then
                sEmbedCode = oResponse.html

                appxCMS.Util.Cache.Add(sCk, sEmbedCode)
            End If
        End If
        lWistiaEmbed.Text = sEmbedCode
    End Sub


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub




    Protected Class WistiaResponse

        Private _duration As Double = 0
        Public Property duration As Double
            Get
                Return _duration
            End Get
            Set(value As Double)
                _duration = value
            End Set
        End Property

        Private _height As Integer = 0
        Public Property height As Integer
            Get
                Return _height
            End Get
            Set(value As Integer)
                _height = value
            End Set
        End Property

        Private _html As String = ""
        Public Property html As String
            Get
                Return _html
            End Get
            Set(value As String)
                _html = value
            End Set
        End Property

        Private _provider_name As String = ""
        Public Property provider_name As String
            Get
                Return _provider_name
            End Get
            Set(value As String)
                _provider_name = value
            End Set
        End Property

        Private _provider_url As String = ""
        Public Property provider_url As String
            Get
                Return _provider_url
            End Get
            Set(value As String)
                _provider_url = value
            End Set
        End Property

        Private _thumbnail_height As Integer = 0
        Public Property thumbnail_height As Integer
            Get
                Return _thumbnail_height
            End Get
            Set(value As Integer)
                _thumbnail_height = value
            End Set
        End Property

        Private _thumbnail_url As String = ""
        Public Property thumbnail_url As String
            Get
                Return _thumbnail_url
            End Get
            Set(value As String)
                _thumbnail_url = value
            End Set
        End Property

        Private _thumbnail_width As Integer = 0
        Public Property thumbnail_width As Integer
            Get
                Return _thumbnail_width
            End Get
            Set(value As Integer)
                _thumbnail_width = value
            End Set
        End Property

        Private _title As String = ""
        Public Property title As String
            Get
                Return _title
            End Get
            Set(value As String)
                _title = value
            End Set
        End Property

        Private _type As String = ""
        Public Property type As String
            Get
                Return _type
            End Get
            Set(value As String)
                _type = value
            End Set
        End Property

        Private _version As String = ""
        Public Property version As String
            Get
                Return _version
            End Get
            Set(value As String)
                _version = value
            End Set
        End Property

        Private _width As Integer = 0
        Public Property width As Integer
            Get
                Return _width
            End Get
            Set(value As Integer)
                _width = value
            End Set
        End Property

        Public Sub New()

        End Sub

    End Class



End Class
