
Partial Class usercontrols_stateDDList
    Inherits System.Web.UI.UserControl

    Public Property IsReadOnly() As Boolean
        Get
            Return _IsReadOnly
        End Get
        Set(ByVal value As Boolean)
            _IsReadOnly = value
        End Set
    End Property
    Private _IsReadOnly As Boolean = False

    Public Property RequireField() As Boolean
        Get
            Return _RequireField
        End Get
        Set(ByVal value As Boolean)
            _RequireField = value
        End Set
    End Property
    Private _RequireField As Boolean = False

    Public Property ValidationGroup() As String
        Get
            Return _ValidationGroup
        End Get
        Set(ByVal value As String)
            _ValidationGroup = value
        End Set
    End Property
    Private _ValidationGroup As String

    Public Property ValidationErrorMessage() As String
        Get
            Return _ValidationErrorMessage
        End Get
        Set(ByVal value As String)
            _ValidationErrorMessage = value
        End Set
    End Property
    Private _ValidationErrorMessage As String = ""

    Public Property ValidationText() As String
        Get
            Return _ValidationText
        End Get
        Set(ByVal value As String)
            _ValidationText = value
        End Set
    End Property
    Private _ValidationText As String = "State is required."

    Public Property ValidationSide() As ValidatorSide
        Get
            Return _ValidationSide
        End Get
        Set(ByVal value As ValidatorSide)
            _ValidationSide = value
        End Set
    End Property
    Private _ValidationSide As ValidatorSide = ValidatorSide.Right

    Public Property SelectedIndex() As Integer
        Get
            If AllowInternational Then
                If String.IsNullOrEmpty(StateText.Text) Then
                    Return 0
                Else
                    Return 1
                End If
            Else
                Return StateList.SelectedIndex
            End If
        End Get
        Set(ByVal value As Integer)
            If AllowInternational Then
                If value = 0 Then
                    StateText.Text = ""
                End If
            Else
                StateList.SelectedIndex = value
            End If
        End Set
    End Property

    Private _DelayedSelectedValue As String = ""
    Public Property DelayedSelectedValue As String
        Get
            Return _DelayedSelectedValue
        End Get
        Set(value As String)
            _DelayedSelectedValue = value
        End Set
    End Property

    Public Property SelectedValue() As String
        Get
            If AllowInternational Then
                Return StateText.Text
            Else
                Return StateList.SelectedValue
            End If
        End Get
        Set(ByVal value As String)
            If AllowInternational Then
                StateText.Text = value
            Else
                StateList.ClearSelection()
                Dim oItem As ListItem = StateList.Items.FindByValue(value)
                If oItem IsNot Nothing Then
                    oItem.Selected = True
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property SelectedText As String
        Get
            Dim sRet As String = ""
            If AllowInternational Then
                sRet = StateText.Text
            Else

                Dim oItem As ListItem = StateList.SelectedItem
                If oItem IsNot Nothing Then
                    If Not String.IsNullOrEmpty(oItem.Value) Then
                        sRet = oItem.Text
                    End If
                End If
            End If
            Return sRet
        End Get
    End Property

    Public Property CustomVMessage() As String
        Get
            Return WICustomVMessage.Text
        End Get
        Set(ByVal value As String)
            WICustomVMessage.Text = value
            DDCustomVMessage.Text = value
        End Set
    End Property

    Private _AllowInternational As Boolean = False
    Public Property AllowInternational() As Boolean
        Get
            Return _AllowInternational
        End Get
        Set(ByVal value As Boolean)
            _AllowInternational = value
            InitControl()
        End Set
    End Property

    Public Property MaxLength() As Integer
        Get
            Return StateText.MaxLength
        End Get
        Set(ByVal value As Integer)
            StateText.MaxLength = value
        End Set
    End Property

    Public ReadOnly Property MyClientID() As String
        Get
            If Me.AllowInternational Then
                Return StateText.ClientID
            Else
                Return StateList.ClientID
            End If
        End Get
    End Property

    Public Property ValidationDisplay() As ValidatorDisplay
        Get
            Return rfvStateTxtLeft.Display
        End Get
        Set(ByVal value As ValidatorDisplay)
            rfvStateTxtLeft.Display = value
            rfvStateTxtRight.Display = value
            rfvStateLeft.Display = value
            rfvStateList.Display = value
        End Set
    End Property

    Public Enum ValidatorSide
        Left = 1
        Right = 2
    End Enum



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        InitControl()
    End Sub

    Protected Sub InitControl()
        pReadOnly.Visible = False
        If AllowInternational Then
            pText.Visible = True
            pList.Visible = False
            '-- Populate textbox with drop-down selection
            Dim sVal As String = StateList.SelectedValue
            If Not String.IsNullOrEmpty(Me.DelayedSelectedValue) Then
                sVal = Me.DelayedSelectedValue
            End If
            StateText.Text = sVal
        Else
            pText.Visible = False
            pList.Visible = True
            '-- Populate drop down with selection (if exists)
            Dim sVal As String = StateList.Text
            If Not String.IsNullOrEmpty(Me.DelayedSelectedValue) Then
                sVal = Me.DelayedSelectedValue
            End If
            Dim oItem As ListItem = StateList.Items.FindByValue(sVal)
            If oItem IsNot Nothing Then
                StateList.ClearSelection()
                oItem.Selected = True
            End If
        End If

        If RequireField Then
            If String.IsNullOrEmpty(ValidationErrorMessage) Then
                ValidationErrorMessage = "Please select a state"
            End If
            If String.IsNullOrEmpty(ValidationText) Then
                ValidationText = "State is required."
            End If

            Dim oV As RequiredFieldValidator = Nothing
            If ValidationSide = ValidatorSide.Left Then
                rfvStateTxtRight.Visible = False
                rfvStateList.Visible = False

                If AllowInternational Then
                    oV = rfvStateTxtLeft
                Else
                    oV = rfvStateLeft
                End If
            Else
                rfvStateTxtLeft.Visible = False
                rfvStateLeft.Visible = False
                If AllowInternational Then
                    oV = rfvStateTxtRight
                Else
                    oV = rfvStateList
                End If
            End If

            If Not String.IsNullOrEmpty(ValidationGroup) Then
                oV.ValidationGroup = ValidationGroup
            End If
            oV.Text = ValidationText
            oV.ErrorMessage = ValidationErrorMessage
            oV.Enabled = True
            oV.Visible = True
        Else
            rfvStateLeft.Visible = False
            rfvStateList.Visible = False
            rfvStateTxtLeft.Visible = False
            rfvStateTxtRight.Visible = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsReadOnly And Not String.IsNullOrEmpty(SelectedValue) Then
            lState.Text = "<input class=""form-control"" type=""text"" text=""" & StateList.Items.FindByValue(SelectedValue).Text & """ placeholder=""" & StateList.Items.FindByValue(SelectedValue).Text & """ disabled />"
            pList.Visible = False
            pText.Visible = False
            pReadOnly.Visible = True
        End If
    End Sub
End Class
