
Partial Class CCustom_BoldChatVisitorMonitor
    Inherits CLibraryBase


    'Fields
    Protected SiteID As Integer = appxCMS.Util.CMSSettings.GetSiteId()




    'Properties
    Private _environmentMode As String
    Private ReadOnly Property environmentMode() As String

        Get

            'SHOULD return as Dev or Prod
            If ConfigurationManager.AppSettings("Environment") IsNot Nothing Then
                _environmentMode = ConfigurationManager.AppSettings("Environment").ToLower()
            Else
                'Fall back value
                _environmentMode = "dev"
            End If

            Return _environmentMode

        End Get

    End Property



    Protected Overrides Sub BuildControl()

        'Addtional Site data.  Build SiteDetails Obj
        Dim SiteDetails As SiteUtility.SiteDetails
        SiteDetails = SiteUtility.RetrieveSiteSettings(SiteID)

        If (SiteDetails.UseBoldChatVMScript) Then
            litBoldChatMonitorCode.Text = SiteDetails.BoldChatVMScript
        End If


    End Sub



    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        Dim currentMode As String = appxCMS.Util.AppSettings.GetString("Environment").ToLower()

        If (currentMode <> "dev") Then
            BuildControl()
        End If

    End Sub


End Class
