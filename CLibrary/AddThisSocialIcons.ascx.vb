
Partial Class CLibrary_AddThisSocialIcons
    Inherits CLibraryBase

    Private _AddThisUsername As String
    Public Property AddThisUsername As String
        Get
            Return _AddThisUsername
        End Get
        Set(value As String)
            _AddThisUsername = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildControl()
    End Sub

    Protected Overrides Sub BuildControl()
        hplGooglePlusOne.Attributes.Add("g:plusone:size", "medium")
        hplFacebookLike.Attributes.Add("fb:like:layout", "button_count")
        appxCMS.Util.jQuery.IncludePlugin(Page, "AddThisWidget", "//s7.addthis.com/js/250/addthis_widget.js#username=" & Me.AddThisUsername)
    End Sub
End Class
