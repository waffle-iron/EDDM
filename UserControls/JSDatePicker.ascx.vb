Imports System.Web.Script.Serialization

Partial Class UserControls_JSDatePicker
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property IsValid() As Boolean
        Get
            cvSelectedDate.Validate()
            Return cvSelectedDate.IsValid
        End Get
    End Property

    Public Property SelectedDateString() As String
        Get
            Return hfSelectDate.Value
        End Get
        Set(ByVal value As String)
            hfSelectDate.Value = value
        End Set
    End Property

    Public Property SelectedDate() As DateTime
        Get
            Dim dt As New DateTime
            If DateTime.TryParse(hfSelectDate.Value, dt) Then
                Return dt
            End If
            Return Nothing
        End Get
        Set(ByVal value As DateTime)
            If value > DateTime.MinValue Then
                Dim dt As DateTime = value
                hfSelectDate.Value = dt.ToString("MM/dd/yyyy")
            End If
        End Set
    End Property

    Private _DisplayInline As Boolean = True
    Public Property DisplayInline() As Boolean
        Get
            Return _DisplayInline
        End Get
        Set(ByVal value As Boolean)
            _DisplayInline = value
        End Set
    End Property

    Private _ListClass As String = ""
    Public Property ListClass() As String
        Get
            Return _ListClass
        End Get
        Set(ByVal value As String)
            _ListClass = value
        End Set
    End Property

    Private _MaxYear As Integer = DateTime.Now.AddYears(100).Year
    Public Property MaxYear() As Integer
        Get
            Return _MaxYear
        End Get
        Set(ByVal value As Integer)
            _MaxYear = value
        End Set
    End Property

    Private _MinYear As Integer = 1900
    Public Property MinYear() As Integer
        Get
            Return _MinYear
        End Get
        Set(ByVal value As Integer)
            _MinYear = value
        End Set
    End Property

    Private _YearValidationError As String = "Please enter a year between " & MinYear & " and " & MaxYear & "."
    Public Property YearValidationError() As String
        Get
            Return _YearValidationError
        End Get
        Set(ByVal value As String)
            _YearValidationError = value
        End Set
    End Property

    Private _InvalidDateError As String = "Please enter a valid date."
    Public Property InvalidDateError() As String
        Get
            Return _InvalidDateError
        End Get
        Set(ByVal value As String)
            _InvalidDateError = value
        End Set
    End Property

    Private _ContainerClass As String = ""
    Public Property ContainerClass() As String
        Get
            Return _ContainerClass
        End Get
        Set(ByVal value As String)
            _ContainerClass = value
        End Set
    End Property

    Public Property YearInputWidth() As System.Web.UI.WebControls.Unit
        Get
            Return txtYear.Width
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            txtYear.Width = value
        End Set
    End Property

    Private _HighlightError As Boolean
    Public Property HighlightError() As Boolean
        Get
            Return _HighlightError
        End Get
        Set(ByVal value As Boolean)
            _HighlightError = value
        End Set
    End Property

    Private _Required As Boolean = False
    Public Property Required() As Boolean
        Get
            Return _Required
        End Get
        Set(ByVal value As Boolean)
            _Required = value
            cvSelectedDate.Visible = value
        End Set
    End Property

    Private _RequiredErrorMessage As String = "A date is required."
    Public Property RequiredErrorMessage() As String
        Get
            Return _RequiredErrorMessage
        End Get
        Set(ByVal value As String)
            _RequiredErrorMessage = value
        End Set
    End Property

    Public Property ValidationGroup() As String
        Get
            Return cvSelectedDate.ValidationGroup
        End Get
        Set(ByVal value As String)
            cvSelectedDate.ValidationGroup = value
        End Set
    End Property

    Public Property ValidationText() As String
        Get
            Return cvSelectedDate.Text
        End Get
        Set(ByVal value As String)
            cvSelectedDate.Text = value
        End Set
    End Property

    Public Property ShowDay() As Boolean
        Get
            Return phDay.Visible
        End Get
        Set(ByVal value As Boolean)
            phDay.Visible = False
        End Set
    End Property

    Private _RequiredValidatorClass As String = ""
    Public Property RequiredValidatorClass() As String
        Get
            Return _RequiredValidatorClass
        End Get
        Set(ByVal value As String)
            _RequiredValidatorClass = value
        End Set
    End Property

    Private _ValidatorClass As String = ""
    Public Property ValidatorClass() As String
        Get
            Return _ValidatorClass
        End Get
        Set(ByVal value As String)
            _ValidatorClass = value
        End Set
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Me.Required Then
            cvSelectedDate.ClientValidationFunction = Me.ClientID & "_checkSelectedDate"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtYear.Attributes.Add("placeholder", "Year")

        If Required Then
            cvSelectedDate.ErrorMessage = RequiredErrorMessage
            cvSelectedDate.CssClass = ValidatorClass & IIf(Not String.IsNullOrEmpty(RequiredValidatorClass), " " & RequiredValidatorClass, "")
        Else
            cvSelectedDate.ErrorMessage = ""
            cvSelectedDate.CssClass = ValidatorClass
        End If
        pContainer.CssClass = ContainerClass
        If DisplayInline Then
            pContainer.Attributes.CssStyle.Add("display", "inline;")
        End If
        'Response.Write(" I am " & cvSelectedDate.ClientID & " required = " & Required.ToString() & "!<br />")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim aMonth As New ArrayList
        For i As Integer = 1 To 12
            aMonth.Add(MonthName(i, False))
        Next
        Dim aDays As New ArrayList

        For i As Integer = 1 To Date.DaysInMonth(SelectedDate.Year, SelectedDate.Month)
            aDays.Add(i)
        Next
        Dim sMonths As String = New JavaScriptSerializer().Serialize(aMonth)
        Dim sDays As String = New JavaScriptSerializer().Serialize(aDays)
        Dim oJs As New StringBuilder
        oJs.AppendLine("function " & Me.ClientID & "_checkSelectedDate(sender, args) {")
        oJs.AppendLine("    var bValid = true;")
        oJs.AppendLine("    if (jQuery('#" & ddlMonth.ClientID & "').val() == '') {")
        'oJs.AppendLine("        console.log('invalid month');")
        oJs.AppendLine("        bValid = false;")
        oJs.AppendLine("    }")
        If Me.ShowDay Then
            oJs.AppendLine("    if (jQuery('#" & ddlDay.ClientID & "').val() == '') {")
            'oJs.AppendLine("        console.log('invalid day');")
            oJs.AppendLine("        bValid = false;")
            oJs.AppendLine("    }")
        End If
        If Me.MinYear > 0 Or Me.MaxYear > 0 Then
            If Me.MinYear > 0 Then
                oJs.AppendLine("    if (jQuery('#" & txtYear.ClientID & "').val()*1 < " & Me.MinYear & ") {")
                'oJs.AppendLine("        console.log('invalid year (less than allowed)');")
                oJs.AppendLine("        bValid = false;")
                oJs.AppendLine("    }")
            End If
            If Me.MaxYear > 0 Then
                oJs.AppendLine("    if (jQuery('#" & txtYear.ClientID & "').val()*1 > " & Me.MaxYear & ") {")
                'oJs.AppendLine("        console.log('invalid year (greater than allowed)');")
                oJs.AppendLine("        bValid = false;")
                oJs.AppendLine("    }")
            End If
        Else
            '-- Verify that the provided year is a number
            oJs.AppendLine("    if (jQuery('#" & txtYear.ClientID & "').val() == '') {")
            'oJs.AppendLine("        console.log('invalid year');")
            oJs.AppendLine("        bValid = false;")
            oJs.AppendLine("    }")
        End If
        oJs.AppendLine("    args.IsValid = bValid;")
        oJs.AppendLine("}")
        oJs.AppendLine("jQuery(document).ready(function($) {")
        oJs.AppendLine("    var aMonths = " & sMonths & ";")
        oJs.AppendLine("    var mULList = $('#" & ddlMonth.ClientID & "');")
        oJs.AppendLine("    for (var iM = 0; iM < aMonths.length; iM++) {")
        oJs.AppendLine("        mULList.append('<option value=""' + (iM) + '"">' + aMonths[iM] + '</option>');")
        oJs.AppendLine("    }")
        If ShowDay Then
            oJs.AppendLine("    var aDays =  " & sDays & ";")
            oJs.AppendLine("    var dULList = $('#" & ddlDay.ClientID & "');")
            oJs.AppendLine("    for (var iD = 0; iD < aDays.length; iD++) {")
            oJs.AppendLine("        dULList.append('<option value=""' + aDays[iD].toString() + '"">' + aDays[iD] + '</option>');")
            oJs.AppendLine("    }")
        End If
        oJs.AppendLine("    var sDate = $('#" & hfSelectDate.ClientID & "').val();")
        oJs.AppendLine("    if (sDate != '') {")
        oJs.AppendLine("        var selDate = new Date(Date.parse(sDate));")
        If ShowDay Then
            oJs.AppendLine("        $('#" & ddlDay.ClientID & " option[value=""' + selDate.getUTCDate() + '""]').attr('selected','selected');")
        End If
        oJs.AppendLine("        $('#" & ddlMonth.ClientID & " option[value=""' + (selDate.getUTCMonth()) + '""]').attr('selected', 'selected');")
        oJs.AppendLine("        $('#" & txtYear.ClientID & "').val(selDate.getFullYear());")
        oJs.AppendLine("    }")
        oJs.AppendLine("    $('#" & ddlMonth.ClientID & "').change(function(e) {")
        If ShowDay Then
            oJs.AppendLine("        setDaysOfMonth();")
        End If
        oJs.AppendLine("    });")
        oJs.AppendLine("    $('#" & ddlMonth.ClientID & ", #" & IIf(ShowDay, ddlDay.ClientID & ", #" & txtYear.ClientID, txtYear.ClientID) & "').blur(function() {")
        oJs.AppendLine("        var iYear = parseInt($('#" & txtYear.ClientID & "').val());")
        If ShowDay Then
            oJs.AppendLine("        setDaysOfMonth();")
        End If
        oJs.AppendLine("        bSetDate = true;")
        oJs.AppendLine("        if (!isNaN(iYear)) {")
        oJs.AppendLine("            if (iYear >= " & MinYear & " && iYear <= " & MaxYear & ") {")
        oJs.AppendLine("                $('#" & txtYear.ClientID & "').removeClass('ui-state-error');")
        oJs.AppendLine("            } else {")
        oJs.AppendLine("                $('#" & txtYear.ClientID & "').addClass('ui-state-error');")
        oJs.AppendLine("                bSetDate = false;")
        oJs.AppendLine("            }")
        oJs.AppendLine("        } else {")
        oJs.AppendLine("            $('#" & txtYear.ClientID & "').val('').addClass('ui-state-error');")
        oJs.AppendLine("            bSetDate = false;")
        oJs.AppendLine("        }")
        oJs.AppendLine("        var iMonth = parseInt($('#" & ddlMonth.ClientID & "').val());")
        oJs.AppendLine("        if (!isNaN(iMonth)) {")
        If ShowDay Then
            oJs.AppendLine("                setDaysOfMonth();")
        End If
        oJs.AppendLine("        } else {")
        oJs.AppendLine("            $('#" & ddlMonth.ClientID & " option:selected').attr('selected','');")
        oJs.AppendLine("            bSetDate = false;")
        oJs.AppendLine("        }")
        oJs.AppendLine("        var iDay = 1;")
        If ShowDay Then
            oJs.AppendLine("        iDay = parseInt($('#" & ddlDay.ClientID & "').val());")
            oJs.AppendLine("        if (isNaN(iDay)) {")
            oJs.AppendLine("            $('#" & ddlDay.ClientID & " option:selected').attr('selected','');")
            oJs.AppendLine("            bSetDate = false;")
            oJs.AppendLine("        }")
        End If
        oJs.AppendLine("        if (bSetDate) {")
        oJs.AppendLine("            var sMonth = (iMonth < 9 ? 0 + (iMonth + 1).toString() : (iMonth + 1).toString());")
        oJs.AppendLine("            var sDay = (iDay < 10 ? 0 + iDay.toString() : iDay.toString());")
        oJs.AppendLine("            var sDateSet = sMonth + '/' + sDay + '/' + iYear;")
        oJs.AppendLine("            $('#" & hfSelectDate.ClientID & "').val(sDateSet);")
        oJs.AppendLine("            $('#" & cvSelectedDate.ClientID & "').hide();")
        oJs.AppendLine("        } else {")
        oJs.AppendLine("            $('#" & hfSelectDate.ClientID & "').val('');")
        oJs.AppendLine("            $('#" & cvSelectedDate.ClientID & "').show();")
        oJs.AppendLine("        }")
        oJs.AppendLine("    });")
        If ShowDay Then
            oJs.AppendLine("    function setDaysOfMonth() {")
            oJs.AppendLine("        var iM = parseInt($('#" & ddlMonth.ClientID & "').val());")
            oJs.AppendLine("        var iMcnt = ($('#" & ddlDay.ClientID & " > option').size() - 1);")
            oJs.AppendLine("        if (!isNaN(iM)) {")
            oJs.AppendLine("            if (iM == 1 || iM == 3 || iM == 5 || iM == 7 || iM == 8 || iM == 10 || iM == 12) {")
            oJs.AppendLine("                if (iMcnt < 31) {")
            oJs.AppendLine("                    var iAdd = (31 - iMcnt);")
            oJs.AppendLine("                    for (var i = iAdd; i > 0; i--) {")
            oJs.AppendLine("                        var iVal = (31 - i) + 1;")
            oJs.AppendLine("                        dULList.append('<option value=""' + iVal.toString() + '"">' + iVal + '</option>');")
            oJs.AppendLine("                    }")
            oJs.AppendLine("                }")
            oJs.AppendLine("            } else if (iM == 4 || iM == 6 || iM == 9 || iM == 11) {")
            oJs.AppendLine("                if (iMcnt > 30) {")
            oJs.AppendLine("                    var iRem = (iMcnt - 30);")
            oJs.AppendLine("                    for (var i = iRem; i > 0; i--) {")
            oJs.AppendLine("                        $('#" & ddlDay.ClientID & " > option:last').remove();")
            oJs.AppendLine("                    }")
            oJs.AppendLine("                } else if (iMcnt < 30) {")
            oJs.AppendLine("                    var iAdd = (30 - iMcnt);")
            oJs.AppendLine("                    for (var i = iAdd; i > 0; i--) {")
            oJs.AppendLine("                        var iVal = (30 - i) + 1;")
            oJs.AppendLine("                        dULList.append('<option value=""' + iVal.toString() + '"">' + iVal + '</option>');")
            oJs.AppendLine("                    }")
            oJs.AppendLine("                }")
            oJs.AppendLine("            } else {")
            oJs.AppendLine("                var iYear = parseInt($('#" & txtYear.ClientID & "').val());")
            oJs.AppendLine("                if (!isNaN(iYear)) {")
            oJs.AppendLine("                    if ((iYear % 4) == 0) {")
            oJs.AppendLine("                        if (iMcnt > 29) {")
            oJs.AppendLine("                            var iRem = (iMcnt - 29);")
            oJs.AppendLine("                            for (var i = iRem; i > 0; i--) {")
            oJs.AppendLine("                                $('#" & ddlDay.ClientID & " > option:last').remove();")
            oJs.AppendLine("                            }")
            oJs.AppendLine("                        } else if (iMcnt < 29) {")
            oJs.AppendLine("                            var iAdd = (iMcnt - 29);")
            oJs.AppendLine("                            for (var i = iAdd; i > 0; i--) {")
            oJs.AppendLine("                                var iVal = (30 - i) + 1;")
            oJs.AppendLine("                                dULList.append('<option value=""' + iVal.toString() + '"">' + iVal + '</option>');")
            oJs.AppendLine("                            }")
            oJs.AppendLine("                        }")
            oJs.AppendLine("                    } else {")
            oJs.AppendLine("                        if (iMcnt > 28) {")
            oJs.AppendLine("                            var iRem = (iMcnt - 28);")
            oJs.AppendLine("                            for (var i = iRem; i > 0; i--) {")
            oJs.AppendLine("                                $('#" & ddlDay.ClientID & " > option:last').remove();")
            oJs.AppendLine("                            }")
            oJs.AppendLine("                        }")
            oJs.AppendLine("                    }")
            oJs.AppendLine("                } else {")
            oJs.AppendLine("                    if (iMcnt > 29) {")
            oJs.AppendLine("                        var iRem = (iMcnt - 29);")
            oJs.AppendLine("                        for (var i = iRem; i > 0; i--) {")
            oJs.AppendLine("                            $('#" & ddlDay.ClientID & " > option:last').remove();")
            oJs.AppendLine("                        }")
            oJs.AppendLine("                    } else if (iMcnt < 29) {")
            oJs.AppendLine("                        var iAdd = (iMcnt - 29);")
            oJs.AppendLine("                        for (var i = iAdd; i > 0; i--) {")
            oJs.AppendLine("                            var iVal = (30 - i) + 1;")
            oJs.AppendLine("                            dULList.append('<option value=""' + iVal.toString() + '"">' + iVal + '</option>');")
            oJs.AppendLine("                        }")
            oJs.AppendLine("                    }")
            oJs.AppendLine("                }")
            oJs.AppendLine("            }")
            oJs.AppendLine("        }")
            oJs.AppendLine("    }")
        End If
        oJs.AppendLine("});")
        'oJs.AppendLine("function " & pContainer.ClientID & "_checkSelectedDate(sender, args) {")
        'oJs.AppendLine("    var sDate = jQuery('#" & hfSelectDate.ClientID & "').val();")
        'oJs.AppendLine("    if (sDate == '') {")
        'If Required Then
        '    oJs.AppendLine("        args.IsValid = false;")
        '    oJs.AppendLine("        sender.errormessage = '" & InvalidDateError & "';")
        '    oJs.AppendLine("        return;")
        'Else
        '    oJs.AppendLine("        args.IsValid = true;")
        '    oJs.AppendLine("        return;")
        'End If
        'oJs.AppendLine("    } else {")
        'oJs.AppendLine("        var iMonth = parseInt(jQuery('#" & ddlMonth.ClientID & "').val());")
        'oJs.AppendLine("        var iDay = 1;")
        'If ShowDay Then
        '    oJs.AppendLine("        iDay = parseInt(jQuery('#" & ddlDay.ClientID & "').val());")
        'End If
        'oJs.AppendLine("        var iYear = parseInt(jQuery('#" & txtYear.ClientID & "').val());")
        'oJs.AppendLine("        if (isNaN(iDay) || isNaN(iMonth) || isNaN(iYear)) {")
        'oJs.AppendLine("            args.IsValid = false;")
        'oJs.AppendLine("            sender.errormessage = '" & InvalidDateError & "';")
        'oJs.AppendLine("            return;")
        'oJs.AppendLine("        } else {")
        'oJs.AppendLine("            if (iYear <= " & MinYear & " || iYear >= " & MaxYear & ") {")
        'oJs.AppendLine("                args.IsValid = false;")
        'oJs.AppendLine("                sender.errormessage = '" & YearValidationError & "';")
        'oJs.AppendLine("                return;")
        'oJs.AppendLine("            } else {")
        'oJs.AppendLine("                args.IsValid = true;")
        'oJs.AppendLine("                return;")
        'oJs.AppendLine("            }")
        'oJs.AppendLine("        }")
        'oJs.AppendLine("    }")
        'oJs.AppendLine("}")
        'appxCMS.Util.jQuery.RegisterClientScript(Page, Me.ClientID & "_dateSelectINIT", oJs.ToString())
        Page.ClientScript.RegisterStartupScript(GetType(String), pContainer.ClientID & "_dateSelectINIT", oJs.ToString(), True)
    End Sub

    Protected Sub cvSelectedDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvSelectedDate.ServerValidate
        If Required And String.IsNullOrEmpty(hfSelectDate.Value) Then
            args.IsValid = False
            cvSelectedDate.ErrorMessage = RequiredErrorMessage
        Else
            If Required Then
                Dim vDt As New DateTime
                If DateTime.TryParse(hfSelectDate.Value, vDt) Then
                    If vDt.Year >= MinYear And vDt.Year <= MaxYear Then
                        args.IsValid = True
                    Else
                        args.IsValid = False
                        cvSelectedDate.ErrorMessage = YearValidationError
                    End If
                Else
                    cvSelectedDate.ErrorMessage = InvalidDateError
                    args.IsValid = False
                End If
            Else
                args.IsValid = True
            End If
        End If
    End Sub
End Class
