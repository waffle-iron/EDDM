
Partial Class CCustom_BoldChatConversion
    Inherits CLibraryBase

    'These scripts should only write to the page on the EDDM Site and only in Production.
    'They should only write to the four pages listed below.


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




    'Methods
    Protected Overrides Sub BuildControl()

        If environmentMode <> "dev" Then

            If SiteID = 1 Then

                Dim bShow As Boolean = False
                Dim sPg As String = Page.AppRelativeVirtualPath.ToLower()

                Select Case sPg
                    Case "~/receipt.aspx"
                        phReceipt.Visible = True
                        bShow = True
                    Case "~/eddmquote.aspx"
                        phQuote.Visible = True
                        bShow = True
                    Case "~/step1-targetreview.aspx"
                        phMap.Visible = True
                        bShow = True
                    Case Else
                        Dim sUrl As String = appxCMS.PageBase.GetRequestedURL(Page)
                        If sUrl.ToLower().StartsWith("/account-welcome") Then
                            phAccount.Visible = True
                            bShow = True
                        End If
                End Select

                If Not bShow Then
                    Me.Visible = False
                End If

            End If

        End If


    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        BuildControl()
    End Sub


End Class
