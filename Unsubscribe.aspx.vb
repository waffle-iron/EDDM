
Partial Class Unsubscribe
    Inherits pageBase

    Public ReadOnly Property SendId() As Integer
        Get
            Return QStringToInt("sid")
        End Get
    End Property

    Public ReadOnly Property RecipId() As Integer
        Get
            Return QStringToInt("rid")
        End Get
    End Property

    Public ReadOnly Property EmailAddress() As String
        Get
            Return QStringToVal("email")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jqueryHelper.RegisterStylesheet(Page, "~/app_styles/forms.css")
        If Not Page.IsPostBack Then
            '-- validate recipient information
            Dim iValid As Integer = 0
            Using oRecipA As New appxMessageBlastTableAdapters.CampaignSendRecipientTableAdapter
                iValid = oRecipA.ValidateUnsubscribe(Me.RecipId, Me.SendId, Me.EmailAddress)
            End Using

            If iValid > 0 Then
                Dim oSubs As System.Collections.Generic.List(Of appxCMS.MailingListSubscription) = appxCMS.MailingListDataSource.GetListSubscriptions(Me.EmailAddress)
                If oSubs IsNot Nothing Then
                    For Each oSub As appxCMS.MailingListSubscription In oSubs
                        If oSub.MailingList.PublicSubscription Then
                            Dim oItem As ListItem = New ListItem(oSub.MailingList.Name, oSub.MailingList.MailingListId)
                            'oItem.Selected = True
                            chkUnsubscribe.Items.Add(oItem)
                        End If
                    Next
                End If
                'Using oSubA As New appxMembershipTableAdapters.MailingListSubscriptionStatusTableAdapter
                '    Using oSubT As appxMembership.MailingListSubscriptionStatusDataTable = oSubA.GetData(iLMemberId)
                '        For Each oSub As appxMembership.MailingListSubscriptionStatusRow In oSubT.Rows
                '            If oSub.Subscribed = 1 And oSub.PublicSubscription Then
                '                Dim oItem As ListItem = New ListItem(oSub.Name, oSub.MailingListId)
                '                oItem.Selected = True
                '                chkUnsubscribe.Items.Add(oItem)
                '            End If
                '        Next
                '    End Using
                'End Using

                'Dim iLType As Integer = -1
                'Dim iMemberId As Integer = 0
                'Dim iLMemberId As Integer = 0

                'Using oDb As New appx.CMS.appxCMSEntities

                'End Using

                'Using oMemberA As New appxAuthTableAdapters.AdminTableAdapter
                '    Using oMemberT As appxAuth.AdminDataTable = oMemberA.GetUserByEmailAddress(Me.EmailAddress)
                '        If oMemberT.Rows.Count > 0 Then
                '            Dim oMember As appxAuth.AdminRow = oMemberT.Rows(0)

                '            iMemberId = oMember.AdminID
                '            'If oMember.IsAdmin Then
                '            '    iLMemberId = iMemberId * -1
                '            'Else
                '            '    iLMemberId = iMemberId
                '            'End If
                '        End If
                '    End Using
                'End Using

                'If iMemberId > 0 Then
                '    iLType = 0
                'Else

                'End If



                'If iLMemberId <> 0 Then
                '    Using oSubA As New appxMembershipTableAdapters.MailingListSubscriptionStatusTableAdapter
                '        Using oSubT As appxMembership.MailingListSubscriptionStatusDataTable = oSubA.GetData(iLMemberId)
                '            For Each oSub As appxMembership.MailingListSubscriptionStatusRow In oSubT.Rows
                '                If oSub.Subscribed = 1 And oSub.PublicSubscription Then
                '                    Dim oItem As ListItem = New ListItem(oSub.Name, oSub.MailingListId)
                '                    oItem.Selected = True
                '                    chkUnsubscribe.Items.Add(oItem)
                '                End If
                '            Next
                '        End Using
                '    End Using
                'End If
            Else
                pSubInfo.Visible = False
                pInvalid.Visible = True
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Dim iMemberId As Integer = 0
        'Dim iLMemberId As Integer = 0
        'Using oMemberA As New appxAuthTableAdapters.AdminTableAdapter
        '    Using oMemberT As appxAuth.AdminDataTable = oMemberA.GetUserByEmailAddress(Me.EmailAddress)
        '        If oMemberT.Rows.Count > 0 Then
        '            Dim oMember As appxAuth.AdminRow = oMemberT.Rows(0)

        '            iMemberId = oMember.AdminID
        '            If oMember.IsAdmin Then
        '                iLMemberId = iMemberId * -1
        '            Else
        '                iLMemberId = iMemberId
        '            End If
        '        End If
        '    End Using
        'End Using

        Dim oSubs As New System.Collections.Generic.List(Of Integer)
        For Each oItem As ListItem In chkUnsubscribe.Items
            If oItem.Selected Then
                Dim iListId As Integer = oItem.Value
                oSubs.Add(iListId)
            End If
        Next

        'If iLMemberId <> 0 Then
        '    Using oSubA As New appxMembershipTableAdapters.appxMailingList_SubscriptionTableAdapter

        '        For Each oItem As ListItem In chkUnsubscribe.Items
        '            If oItem.Selected Then
        '                Dim iListId As Integer = oItem.Value
        '                oSubA.DeleteMemberSubscription(iListId, iLMemberId)
        '            End If
        '        Next
        '    End Using
        'End If

        If oSubs.Count > 0 Then
            Dim bRet As Boolean = appxCMS.MailingListDataSource.ManageSubscriptions(Me.EmailAddress, oSubs)
        End If

        pSubInfo.Visible = False
        pInvalid.Visible = False
        pRemoved.Visible = True
    End Sub
End Class
