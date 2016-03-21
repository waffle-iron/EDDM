
Partial Class CCustom_OfficeGoogleMap
    Inherits System.Web.UI.UserControl

    Public Enum GoogleMapSize
        Small = 0
        Medium = 1
        Large = 2
        Custom = 3
    End Enum

    Private _MapSize As GoogleMapSize = GoogleMapSize.Medium
    Public Property MapSize As GoogleMapSize
        Get
            Return _MapSize
        End Get
        Set(value As GoogleMapSize)
            _MapSize = value
        End Set
    End Property

    Private _MapWidth As Integer = 425
    Public Property MapWidth As Integer
        Get
            Return _MapWidth
        End Get
        Set(value As Integer)
            _MapWidth = value
        End Set
    End Property

    Private _MapHeight As Integer = 350
    Public Property MapHeight As Integer
        Get
            Return _MapHeight
        End Get
        Set(value As Integer)
            _MapHeight = value
        End Set
    End Property

    Private bControlBuilt As Boolean = False

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not bControlBuilt Then
            BuildControl()
        End If
    End Sub

    Public Sub BuildControl()
        bControlBuilt = True
        Dim SiteId As Integer = appxCMS.Util.CMSSettings.GetSiteId

        Dim oSite As appxCMS.Site = appxCMS.SiteDataSource.GetSite()

        If oSite IsNot Nothing Then
            If Not String.IsNullOrEmpty(oSite.Address1) Then
                Dim sAddr As String = Server.UrlEncode(oSite.Address1 & " " & oSite.ZipCode)
                Dim sNear As String = Server.UrlEncode(oSite.Address1 & " " & oSite.City & ", " & oSite.State & " " & oSite.ZipCode)
                Dim sMapUrl As String = "http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q={0}&aq=0&z=14&iwloc=A&output=embed"
                Dim sLinkUrl As String = "http://maps.google.com/maps?f=q&source=embed&hl=en&geocode=&q={0}&aq=0&ie=UTF8&hq=&hnear={1}&z=14&iwloc=A"

                gMap.Attributes.Add("src", String.Format(sMapUrl, sAddr))
                'width="640" height="480" 
                Dim iW As Integer = 640
                Dim iH As Integer = 480

                Select Case Me.MapSize
                    Case GoogleMapSize.Small
                        iW = 300
                        iH = 300
                    Case GoogleMapSize.Medium
                        iW = 425
                        iH = 350
                    Case GoogleMapSize.Large
                        iW = 640
                        iH = 480
                    Case GoogleMapSize.Custom
                        iW = Me.MapWidth
                        iH = Me.MapHeight

                        If iW = 0 Then
                            iW = 425
                            iH = 350
                        End If
                End Select
                gMap.Attributes.Add("width", iW)
                gMap.Attributes.Add("height", iH)

                hplLarger.NavigateUrl = String.Format(sLinkUrl, sAddr, sNear)
            Else
                Me.Visible = False
            End If
        Else
            Me.Visible = False
        End If
    End Sub
End Class
