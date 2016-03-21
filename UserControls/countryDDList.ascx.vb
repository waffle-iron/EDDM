
Partial Class usercontrols_countryDDList
    Inherits System.Web.UI.UserControl

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
    Private _ValidationText As String = "!"

    Public Property ValidationSide() As ValidatorSide
        Get
            Return _ValidationSide
        End Get
        Set(ByVal value As ValidatorSide)
            _ValidationSide = value
        End Set
    End Property
    Private _ValidationSide As ValidatorSide = ValidatorSide.Left

    Public Property SelectedValue() As String
        Get
            Return Country.SelectedValue
        End Get
        Set(ByVal value As String)
            Country.ClearSelection()
            Dim oItem As ListItem = Country.Items.FindByValue(value)
            If oItem IsNot Nothing Then
                oItem.Selected = True
            End If
        End Set
    End Property

    Public ReadOnly Property SelectedText() As String
        Get
            Dim sRet As String = ""
            If Country.SelectedItem IsNot Nothing Then
                sRet = Country.SelectedItem.Text
            End If
            Return sRet
        End Get
    End Property

    Public Property SelectedIndex() As Integer
        Get
            Return Country.SelectedIndex
        End Get
        Set(ByVal value As Integer)
            Country.SelectedIndex = value
        End Set
    End Property

    Public Property AutoPostBack() As Boolean
        Get
            Return Country.AutoPostBack
        End Get
        Set(ByVal value As Boolean)
            Country.AutoPostBack = value
        End Set
    End Property

    Public Property IsReadOnly() As Boolean
        Get
            Return _IsReadOnly
        End Get
        Set(ByVal value As Boolean)
            _IsReadOnly = value
        End Set
    End Property
    Private _IsReadOnly As Boolean = False

    Public Event CountryChanged()

    Public ReadOnly Property MyClientID() As String
        Get
            Return Country.ClientID
        End Get
    End Property

    Public Enum ValidatorSide
        Left = 1
        Right = 2
    End Enum

    Public Property ValidationDisplay() As System.Web.UI.WebControls.ValidatorDisplay
        Get
            Return rfvLeft.Display
        End Get
        Set(ByVal value As System.Web.UI.WebControls.ValidatorDisplay)
            rfvLeft.Display = value
            rfvRight.Display = value
        End Set
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If RequireField Then
            If String.IsNullOrEmpty(ValidationErrorMessage) Then
                ValidationErrorMessage = "Please select a country"
            End If
            If String.IsNullOrEmpty(ValidationText) Then
                ValidationText = "!"
            End If

            Dim oV As RequiredFieldValidator = Nothing
            If ValidationSide = ValidatorSide.Left Then
                oV = rfvLeft
            Else
                oV = rfvRight
            End If

            If Not String.IsNullOrEmpty(ValidationGroup) Then
                oV.ValidationGroup = ValidationGroup
            End If
            oV.Text = ValidationText
            oV.ErrorMessage = ValidationErrorMessage
            oV.Enabled = True
            oV.Visible = True
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsReadOnly And Not String.IsNullOrEmpty(SelectedValue) Then
            lCountry.Text = Country.Items.FindByValue(SelectedValue).Text
            Country.Visible = False
        End If
    End Sub

    Protected Sub Country_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Country.SelectedIndexChanged
        RaiseEvent CountryChanged()
    End Sub
End Class
