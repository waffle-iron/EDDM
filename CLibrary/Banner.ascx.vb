Imports System.Collections.Generic

Partial Class CLibrary_Banner
    Inherits CLibraryBase

    Private _NumberOfBannersToShow As Integer = 0
    Public Property NumberOfBannersToShow As Integer
        Get
            Return _NumberOfBannersToShow
        End Get
        Set(value As Integer)
            _NumberOfBannersToShow = value
        End Set
    End Property

    Private _Keyword As String = ""
    Public Property Keyword As String
        Get
            Return _Keyword
        End Get
        Set(value As String)
            _Keyword = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        Dim oAds As List(Of appxCMS.Banner) = appxCMS.BannerDataSource.GetDisplayBanners(Me.NumberOfBannersToShow, Me.Keyword)
        For Each oAd As appxCMS.Banner In oAds
            Dim BannerId As Integer = oAd.BannerId
            Dim ImageUrl As String = oAd.ImageUrl
            Dim NavigateUrl As String = oAd.NavigateUrl
            NavigateUrl = VirtualPathUtility.ToAbsolute("~/Resources/AdTracker.ashx") & "?url=" & NavigateUrl & "&imp=" & oAd.ImpressionId
            Dim iW As Integer = oAd.Width
            Dim iH As Integer = oAd.Height
            Dim AltText As String = oAd.AlternateText

            If ImageUrl.ToLower.EndsWith(".swf") Then
                '-- Process flash
                phBanners.Controls.Add(New LiteralControl("<div style=""position:relative;width:" & iW & "px;height:" & iH & "px;"">"))

                phBanners.Controls.Add(New LiteralControl("		<object classid=""clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"" width=""" & iW & """ height=""" & iH & """ id=""banner" & BannerId & """ align=""middle"" style=""z-index:1;"">"))
                phBanners.Controls.Add(New LiteralControl("		    <param name=""movie"" value=""" & ImageUrl & """ />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""quality"" value=""high"" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""bgcolor"" value=""#ffffff"" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""play"" value=""true"" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""loop"" value=""true"" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""wmode"" value=""transparent"" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""scale"" value=""showall"" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""menu"" value=""true"" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""devicefont"" value=""false"" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""salign"" value="""" />"))
                phBanners.Controls.Add(New LiteralControl("			<param name=""allowScriptAccess"" value=""sameDomain"" />"))
                phBanners.Controls.Add(New LiteralControl("			<!--[if !IE]>-->"))
                phBanners.Controls.Add(New LiteralControl("			<object type=""application/x-shockwave-flash"" data=""" & ImageUrl & """ width=""" & iW & """ height=""" & iH & """>"))
                phBanners.Controls.Add(New LiteralControl("			    <param name=""movie"" value=""" & ImageUrl & """ />"))
                phBanners.Controls.Add(New LiteralControl("			    <param name=""quality"" value=""high"" />"))
                phBanners.Controls.Add(New LiteralControl("			    <param name=""bgcolor"" value=""#ffffff"" />"))
                phBanners.Controls.Add(New LiteralControl("				<param name=""play"" value=""true"" />"))
                phBanners.Controls.Add(New LiteralControl("				<param name=""loop"" value=""true"" />"))
                phBanners.Controls.Add(New LiteralControl("				<param name=""wmode"" value=""transparent"" />"))
                phBanners.Controls.Add(New LiteralControl("				<param name=""scale"" value=""showall"" />"))
                phBanners.Controls.Add(New LiteralControl("				<param name=""menu"" value=""true"" />"))
                phBanners.Controls.Add(New LiteralControl("				<param name=""devicefont"" value=""false"" />"))
                phBanners.Controls.Add(New LiteralControl("				<param name=""salign"" value="""" />"))
                phBanners.Controls.Add(New LiteralControl("				<param name=""allowScriptAccess"" value=""sameDomain"" />"))
                phBanners.Controls.Add(New LiteralControl("				<!--<![endif]-->"))
                phBanners.Controls.Add(New LiteralControl("				<a href=""http://www.adobe.com/go/getflash"">"))
                phBanners.Controls.Add(New LiteralControl("				    <img src=""http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif"" alt=""Get Adobe Flash player"" />"))
                phBanners.Controls.Add(New LiteralControl("				</a>"))
                phBanners.Controls.Add(New LiteralControl("				<!--[if !IE]>-->"))
                phBanners.Controls.Add(New LiteralControl("			</object>"))
                phBanners.Controls.Add(New LiteralControl("			<!--<![endif]-->"))
                phBanners.Controls.Add(New LiteralControl("		</object>"))

                phBanners.Controls.Add(New LiteralControl("<a href=""" & NavigateUrl & """ target=""_blank"" style=""display:block;position:absolute;top:0;left:0;width:" & iW & "px;height:" & iH & "px;z-index:10;"">"))
                Dim oImg As New Image
                oImg.ID = "banner" & BannerId & "Cover"
                oImg.ImageUrl = "/cmsimages/spacer.gif"
                oImg.Width = iW
                oImg.Height = iH
                phBanners.Controls.Add(oImg)
                phBanners.Controls.Add(New LiteralControl("</a>"))

                phBanners.Controls.Add(New LiteralControl("</div>"))
            Else
                '-- Process Image
                phBanners.Controls.Add(New LiteralControl("<div>"))
                phBanners.Controls.Add(New LiteralControl("<a href=""" & NavigateUrl & """ target=""_blank"">"))
                Dim oImg As New Image
                oImg.ID = "bannerAd" & BannerId
                oImg.ImageUrl = ImageUrl
                oImg.AlternateText = AltText
                oImg.ToolTip = AltText
                phBanners.Controls.Add(oImg)
                phBanners.Controls.Add(New LiteralControl("</a>"))
                phBanners.Controls.Add(New LiteralControl("</div>"))
            End If
        Next
    End Sub
End Class
