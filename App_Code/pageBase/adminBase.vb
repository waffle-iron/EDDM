Imports Microsoft.VisualBasic

Public Class adminBase
    Inherits System.Web.UI.Page
    Implements appxCMS.ISiteMapResolver

    Public Shared Function QStringToInt(ByVal sQFld As String) As Integer
        Dim sid As String = QStringToVal(sQFld)
        Dim id As Integer = 0
        Integer.TryParse(sid, id)
        Return id
    End Function

    Public Shared Function QStringToVal(ByVal sQFld As String) As String
        Dim sid As String = ""
        If HttpContext.Current.Request.QueryString(sQFld) IsNot Nothing Then
            sid = HttpContext.Current.Request.QueryString(sQFld)
        End If
        Return sid
    End Function

    Public Shared Function QStringToDate(ByVal queryString As String, ByVal defaultDate As DateTime) As DateTime
        Dim sid As String = HttpContext.Current.Server.UrlDecode(QStringToVal(queryString))
        Dim mdate As DateTime = defaultDate
        If Not DateTime.TryParse(sid, mdate) Then
            mdate = defaultDate
        End If
        Return mdate
    End Function

    Public Shared Function QStringToDate(ByVal queryString As String) As DateTime
        Return QStringToDate(queryString, DateTime.Now)
    End Function

    Protected ReadOnly Property CurPage() As String
        Get
            Dim sPage As String = Page.AppRelativeVirtualPath.ToLower
            Return sPage
        End Get
    End Property

    Public ReadOnly Property LoggedOnUserID() As Integer
        Get
            Dim sUser As String = HttpContext.Current.User.Identity.Name
            Dim AdminID As Integer = 0
            Using oA As New appxAuthTableAdapters.AdminTableAdapter
                AdminID = oA.GetAccountIdForUsername(sUser)
            End Using
            Return AdminID
        End Get
    End Property

    Protected ReadOnly Property SurveyID() As Integer
        Get
            Dim iSurvey As Integer = 0
            Dim sSurvey As String = Request.QueryString("surveyid")
            Integer.TryParse(sSurvey, iSurvey)
            Return iSurvey
        End Get
    End Property

    Protected ReadOnly Property SurveyName() As String
        Get
            Return Request.QueryString("surveyname")
        End Get
    End Property

    Protected ReadOnly Property CampaignID() As Integer
        Get
            Return QStringToInt("campaignid")
        End Get
    End Property

    Protected ReadOnly Property CampaignName() As String
        Get
            Return QStringToVal("campaignname")
        End Get
    End Property

    Public Function SiteMapResolve1(sender As Object, e As System.Web.SiteMapResolveEventArgs) As System.Web.SiteMapNode Implements appxCMS.ISiteMapResolver.SiteMapResolve
        Return ExpandPaths(sender, e)
    End Function

    Protected Overridable Function ExpandPaths(ByVal sender As Object, ByVal e As System.Web.SiteMapResolveEventArgs) As SiteMapNode
        If SiteMap.CurrentNode IsNot Nothing Then
            Dim currentNode As SiteMapNode = SiteMap.CurrentNode.Clone(True)

            Dim tempNode As SiteMapNode = currentNode

            Do Until tempNode.ParentNode Is Nothing
                Select Case tempNode.Url.ToLower
                    '-- Message Blast
                    Case VirtualPathUtility.ToAbsolute("~/admin/messageblast_campaign_edit.aspx")
                        If currentNode.Url.ToLower <> VirtualPathUtility.ToAbsolute("~/admin/messageblast_campaign_edit.aspx") Then
                            tempNode.Title = tempNode.Title & " (" & CampaignName & ")"
                            tempNode.Url = tempNode.Url & "?campaignid=" & CampaignID & "&campaignname=" & Server.UrlEncode(CampaignName)
                        End If

                        '-- Surveys
                    Case VirtualPathUtility.ToAbsolute("~/admin/survey_edit.aspx")
                        If currentNode.Url.ToLower <> VirtualPathUtility.ToAbsolute("~/admin/survey_edit.aspx") Then
                            tempNode.Title = tempNode.Title & " (" & SurveyName & ")"
                            tempNode.Url = tempNode.Url & "?surveyid=" & QStringToInt("surveyid") & "&surveyname=" & Server.UrlEncode(QStringToVal("surveyname"))
                        End If

                        '-- Referrers
                    Case VirtualPathUtility.ToAbsolute("~/admin/referrer_keyword_list.aspx")
                        If currentNode.Url.ToLower <> VirtualPathUtility.ToAbsolute("~/admin/referrer_keyword_list.aspx") Then
                            tempNode.Url = tempNode.Url & "?id=" & QStringToInt("ruid")
                        End If
                End Select

                If tempNode("defqs") IsNot Nothing Then
                    Dim sDefQs As String = tempNode("defqs")
                    Dim sQ As String = appxCMS.Util.Querystring.GetString(sDefQs)
                    If Not String.IsNullOrEmpty(sQ) Then
                        Dim bHasQ As Boolean = tempNode.Url.Contains("?")
                        Dim sJoin As String = "?"
                        If bHasQ Then
                            sJoin = "&"
                        End If
                        tempNode.Url = tempNode.Url & sJoin & sDefQs & "=" & Server.UrlEncode(sQ)
                    End If
                End If

                tempNode = tempNode.ParentNode
            Loop
            Page.Title = currentNode.Title
            Return currentNode
        Else
            Return Nothing
        End If
    End Function

    Protected Overridable Function IsSamePage(ByVal context1 As HttpContext, ByVal context2 As HttpContext) As Boolean
        'by default, the contexts are considered the same if they map to the same file and have the same query string
        Return ((Server.MapPath(context1.Request.AppRelativeCurrentExecutionFilePath) = Server.MapPath(context2.Request.AppRelativeCurrentExecutionFilePath)) _
        AndAlso (context1.Request.QueryString.Equals(context2.Request.QueryString)))
    End Function

    Protected Sub SetPgAction(ByVal value As String)
        Page.Title = value

        Dim oTitle As ContentPlaceHolder = DirectCast(Page.Master.FindControl("Title"), ContentPlaceHolder)
        If oTitle IsNot Nothing Then
            oTitle.Controls.Add(New LiteralControl("<div class=""pgHead"">" & value & "</div>"))
        End If
    End Sub

    Public Shared Function UpdateStatusMsg(ByVal msg As String, Optional ByVal ErrIndicator As Boolean = False) As String
        Return StatusMsg(msg, ErrIndicator)
        'If ErrIndicator Then
        '    Return "<p class=""msg-warning"">" & msg & "</p>"
        'Else
        '    Return "<p class=""msg-success"">" & msg & "</p>"
        'End If
    End Function

    Public Shared Function StatusMsg(ByVal sMsg As String, Optional ByVal bErr As Boolean = False) As String
        Dim sClass As String = "ui-state-highlight"
        Dim sIcon As String = "ui-icon-info"
        If bErr Then
            sClass = "ui-state-error"
            sIcon = "ui-icon-alert"
        End If
        Return "<div class=""ui-widget"" style=""margin:10px 0;font-size:0.9em;""><div class=""" & sClass & " ui-corner-all"" style=""padding:0.7em;""><p><span class=""ui-icon " & sIcon & """ style=""float: left; margin-right: .3em;""></span><div style=""margin-bottom:1em;"">" & sMsg & "</div></p></div></div>"
    End Function

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If String.IsNullOrEmpty(Page.Title) Or Page.Title.ToLower = "untitled page" Then
            Dim sTitle As String = ConfigurationManager.AppSettings("SiteTitle")
            Page.Title = sTitle & " | administration"
        End If
    End Sub

    Public Shared Sub JGrowlErrorMsg(ByVal oPage As Page, ByVal msg As String, ByVal sTitle As String)
        oPage.ClientScript.RegisterStartupScript(GetType(String), "jGrowlStatusMsg", "$.jGrowl('" & apphelp.JSBless(msg) & "', { sticky: true, header: '" & apphelp.JSBless(sTitle) & "' });", True)
    End Sub

    Public Shared Sub JGrowlStatusMsg(ByVal oPage As Page, ByVal msg As String, ByVal sTitle As String)
        oPage.ClientScript.RegisterStartupScript(GetType(String), "jGrowlStatusMsg", "$.jGrowl('" & apphelp.JSBless(msg) & "', { header: '" & apphelp.JSBless(sTitle) & "' });", True)
    End Sub
End Class
