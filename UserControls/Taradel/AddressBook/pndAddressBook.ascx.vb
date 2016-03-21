
Partial Class usercontrols_pndAddressBook
    Inherits System.Web.UI.UserControl

    Public Enum AddressBookDisplayMode
        ManagementList = 1
        SelectList = 2
        AddForm = 3
    End Enum

    Private _DisplayMode As AddressBookDisplayMode = AddressBookDisplayMode.ManagementList
    Public Property DisplayMode() As AddressBookDisplayMode
        Get
            Return Me._DisplayMode
        End Get
        Set(ByVal value As AddressBookDisplayMode)
            Me._DisplayMode = value
        End Set
    End Property

    Public Property CssClass() As String
        Get
            Return pAddressBook.CssClass
        End Get
        Set(ByVal value As String)
            pAddressBook.CssClass = (pAddressBook.CssClass & " " & value).Trim
        End Set
    End Property

    Public Event ContactSelected(ByVal sender As Object, ByVal e As System.EventArgs)

    Private _CustomerID As Integer = 0
    Public ReadOnly Property CustomerID() As Integer
        Get
            If Me._CustomerID = 0 Then
                Dim sCustID As String = pageBase.LoggedOnUserID
                Dim iCustID As Integer = 0
                Integer.TryParse(sCustID, iCustID)
                Me._CustomerID = iCustID
            End If
            Return Me._CustomerID
        End Get
    End Property

    Private _SelectedContactID As Integer = 0
    Public ReadOnly Property SelectedContactID() As Integer
        Get
            Return Me._SelectedContactID
        End Get
    End Property

    Private _IsResidential As Boolean = False
    Public ReadOnly Property IsResidential() As Integer
        Get
            If Me._IsResidential Then
                Return 1
            Else
                Return 0
            End If
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Me.DisplayMode = AddressBookDisplayMode.SelectList Then
                pSelectForm.Visible = True
                lvContactIndex.Visible = False
                pAddressBook.Style.Add("height", "250px")
                pAddressBook.Style.Add("overflow", "auto")
            ElseIf Me.DisplayMode = AddressBookDisplayMode.AddForm Then
                pAddForm.Visible = True
                '-- Update the validation group for these controls so that they are absolutely unique in the page
                Dim sUnique As String = vSummNewAddr.ValidationGroup & System.DateTime.Now.Ticks.ToString
                vSummNewAddr.ValidationGroup = sUnique
                rfvNewAddrCompany.ValidationGroup = sUnique
                rfvNewAddrAddress1.ValidationGroup = sUnique
                rfvNewAddrCity.ValidationGroup = sUnique
                rfvNewAddrState.ValidationGroup = sUnique
                rfvNewAddrZip.ValidationGroup = sUnique
                NewAddrCreate.ValidationGroup = sUnique
            ElseIf Me.DisplayMode = AddressBookDisplayMode.ManagementList Then
                pSelectForm.Visible = True
            End If

            BindAddressBook()
        End If
    End Sub

    Public Sub BindAddressBook()
        Dim iCount As Integer = 0
        'TODO: Replace these with updated Taradel-managed calls
        'Using oABA As New taradelCustomerTableAdapters.pnd_CustomerShippingAddressTableAdapter
        '    iCount = oABA.Count(Me.CustomerID)
        'End Using
        If iCount = 0 Then
            lvContactIndex.Visible = False
            lvContacts.Visible = False
        Else
            Dim oAlpha As New SortedList

            'Using oAList As New taradelCustomerTableAdapters.AddressBookAlphaListTableAdapter
            '    Dim oAListT As taradelCustomer.AddressBookAlphaListDataTable = oAList.GetData(Me.CustomerID)
            '    Dim oMatch() As taradelCustomer.AddressBookAlphaListRow
            '    Dim iEmpty As Integer = 0
            '    oMatch = oAListT.Select("Letter='?'")
            '    If oMatch.Length > 0 Then
            '        iEmpty = oMatch(0).LetterCount
            '    End If
            '    oAlpha.Add("?", iEmpty)

            '    Dim iNum As Integer = 0
            '    oMatch = oAListT.Select("Letter='0-9'")
            '    If oMatch.Length > 0 Then
            '        iNum = oMatch(0).LetterCount
            '    End If
            '    oAlpha.Add("0-9", iNum)

            '    For iChar As Integer = 65 To 90
            '        Dim iCharCount As Integer = 0
            '        oMatch = oAListT.Select("Letter='" & Chr(iChar) & "'")
            '        If oMatch.Length > 0 Then
            '            iCharCount = oMatch(0).LetterCount
            '        End If
            '        oAlpha.Add(Chr(iChar).ToString, iCharCount)
            '    Next
            'End Using
            If Me.DisplayMode = AddressBookDisplayMode.ManagementList Then
                lvContactIndex.Visible = True
                lvContactIndex.DataSource = oAlpha
                lvContactIndex.DataBind()
            End If
            lvContacts.Visible = True
            lvContacts.DataSource = oAlpha
            lvContacts.DataBind()
            If Me.DisplayMode = AddressBookDisplayMode.SelectList Then
                Dim pContactList As Panel = DirectCast(lvContacts.FindControl("pContactList"), Panel)
                pContactList.CssClass = "contact-list-select"
            End If
        End If
    End Sub

    Protected Sub lvContacts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvContacts.ItemDataBound
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            Dim sLetter As String = DataBinder.Eval(oDItem.DataItem, "Key")
            Dim iCount As Integer = DataBinder.Eval(oDItem.DataItem, "Value")

            If iCount > 0 Then
                Dim lvItems As ListView = DirectCast(e.Item.FindControl("lvItems"), ListView)
                If lvItems IsNot Nothing Then
                    'Using oA As New taradelCustomerTableAdapters.pnd_CustomerShippingAddressTableAdapter
                    '    Dim oT As taradelCustomer.pnd_CustomerShippingAddressDataTable = oA.GetAlphaPage(Me.CustomerID, sLetter)
                    '    lvItems.DataSource = oT
                    '    lvItems.DataBind()
                    '    oT.Dispose()
                    'End Using
                End If
            End If
        End If
    End Sub

    Protected Sub lvItems_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs)
        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim oDItem As ListViewDataItem = e.Item

            If Me.DisplayMode = AddressBookDisplayMode.ManagementList Then
                Dim lnkSelectContact As LinkButton = DirectCast(e.Item.FindControl("lnkSelectContact"), LinkButton)
                lnkSelectContact.Visible = False
            Else
                '-- Hide the hyperlink for mailto 
                Dim hplCareOf As HyperLink = DirectCast(e.Item.FindControl("hplCareOf"), HyperLink)
                hplCareOf.NavigateUrl = ""
                'hplCareOf.Enabled = False
                hplCareOf.CssClass = "nolink"

                Dim pManageContact As Panel = DirectCast(e.Item.FindControl("pManageContact"), Panel)
                pManageContact.Visible = False
            End If
        End If
    End Sub

    Protected Sub DeleteShippingAddress(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oLnk As LinkButton = DirectCast(sender, LinkButton)
        Dim iAddrID As Integer = oLnk.CommandArgument

        'Using oA As New taradelCustomerTableAdapters.pnd_CustomerShippingAddressTableAdapter
        '    oA.DeleteCustomerShippingAddress(iAddrID, Me.CustomerID)
        'End Using

        Response.Redirect(Page.AppRelativeVirtualPath)
    End Sub

    Protected Sub SelectContact(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oLnk As LinkButton = DirectCast(sender, LinkButton)
        Dim sAddrID As String = oLnk.CommandArgument
        Dim iAddrID As Integer = 0
        Integer.TryParse(sAddrID, iAddrID)
        If iAddrID > 0 Then
            'Using oAddrA As New taradelCustomerTableAdapters.pnd_CustomerShippingAddressTableAdapter
            '    Me._IsResidential = oAddrA.IsResidential(iAddrID)
            'End Using
            Me._SelectedContactID = iAddrID
            RaiseEvent ContactSelected(Me, New System.EventArgs)
            'OnContactSelected(Me, New System.EventArgs)
        End If
    End Sub

    Protected Sub CreateNewAddress(ByVal sender As Object, ByVal e As System.EventArgs)
        Page.Validate(vSummNewAddr.ValidationGroup)
        If Page.IsValid Then
            Dim iAddressID As Integer = 0
            'Using oAddrA As New taradelCustomerTableAdapters.pnd_CustomerShippingAddressTableAdapter
            '    iAddressID = oAddrA.NewAddress(Me.CustomerID, NewAddrCompany.Text, NewAddrCareOf.Text, NewAddrAddress1.Text, NewAddrAddress2.Text, _
            '                                   NewAddrCity.Text, NewAddrState.Text, NewAddrZip.Text, NewAddrCountry.Text, NewAddrPhone.Text, "", NewAddrNonCommercial.Checked)
            'End Using
            If iAddressID > 0 Then
                Me._SelectedContactID = iAddressID
                Me._IsResidential = NewAddrNonCommercial.Checked
                RaiseEvent ContactSelected(Me, New System.EventArgs)
            End If
        End If
    End Sub
End Class
