Imports System.Data.SqlClient
Imports System.Web.Services

Public Class CreateTravelPolicy
    Inherits Page

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        ' Disable unobtrusive validation to avoid jQuery requirement
        ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Response.Headers.Add("Content-Security-Policy", "default-src *; img-src *;")
        txtInsuranceFrom.Attributes("min") = Date.Today.ToString("yyyy-MM-dd")

        If Not IsPostBack Then
            LoadNationalities()
            LoadClauses()
            LoadDestinations()
            txtInsuranceFrom.Text = Date.Today.ToString("yyyy-MM-dd")
            txtInsuranceDays.Text = "7"
        End If

        CalculateToDate()
        RecalculatePremiumIfPossible()
    End Sub

    Private Sub RecalculatePremiumIfPossible()
        If Not String.IsNullOrWhiteSpace(txtDateOfBirth.Text) AndAlso
           Not String.IsNullOrWhiteSpace(txtInsuranceFrom.Text) AndAlso
           Not String.IsNullOrWhiteSpace(txtInsuranceDays.Text) AndAlso
           ddlClauses.SelectedIndex > 0 AndAlso
           ddlDestination.SelectedIndex > 0 Then

            Dim days As Integer
            Dim clauseValue As Integer
            Dim destValue As Integer

            If Integer.TryParse(txtInsuranceDays.Text, days) AndAlso
               Integer.TryParse(ddlClauses.SelectedValue, clauseValue) AndAlso
               Integer.TryParse(ddlDestination.SelectedValue, destValue) AndAlso
               days > 0 AndAlso clauseValue > 0 AndAlso destValue > 0 Then

                Dim premium As PremiumResult = CalculatePremiumValues(days, clauseValue, destValue, txtInsuranceFrom.Text, txtDateOfBirth.Text)
                UpdatePremiumLabels(premium)
            End If
        End If
    End Sub

    Protected Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ' Simple server-side validation
        If String.IsNullOrWhiteSpace(txtTravelerName.Text) OrElse
           String.IsNullOrWhiteSpace(txtTravelerLocation.Text) OrElse
           String.IsNullOrWhiteSpace(txtTravelerPhone.Text) OrElse
           String.IsNullOrWhiteSpace(txtPassportNumber.Text) OrElse
           String.IsNullOrWhiteSpace(txtDateOfBirth.Text) OrElse
           ddlGender.SelectedIndex <= 0 OrElse
           ddlPersonNationality.SelectedIndex <= 0 OrElse
           String.IsNullOrWhiteSpace(txtInsuranceFrom.Text) OrElse
           String.IsNullOrWhiteSpace(txtInsuranceDays.Text) OrElse
           ddlClauses.SelectedIndex <= 0 OrElse
           ddlDestination.SelectedIndex <= 0 Then

            lblResult.CssClass = "feedback d-block alert alert-danger"
            lblResult.Text = "❌ البيانات غير مكتملة. يرجى ملء جميع الحقول المطلوبة."
            Return
        End If

        ' Validate date logic
        Dim fromDate As Date
        Dim dob As Date
        If Not Date.TryParse(txtInsuranceFrom.Text, fromDate) OrElse fromDate < Date.Today Then
            lblResult.CssClass = "feedback d-block alert alert-danger"
            lblResult.Text = "❌ تاريخ البدء يجب أن يكون اليوم أو لاحقاً."
            Return
        End If
        If Not Date.TryParse(txtDateOfBirth.Text, dob) OrElse dob >= Date.Today Then
            lblResult.CssClass = "feedback d-block alert alert-danger"
            lblResult.Text = "❌ تاريخ الميلاد يجب أن يكون في الماضي."
            Return
        End If

        ' If all good, show success
        lblResult.CssClass = "feedback d-block alert alert-success"
        lblResult.Text = "✅ تم إصدار وثيقة تأمين السفر بنجاح! رقم الوثيقة: TRV-" & DateTime.Now.ToString("yyyyMMddHHmmss")
    End Sub

    Private Sub LoadNationalities()
        Dim dt As New DataTable()
        Dim connString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
        Dim query As String = "SELECT TPNo, TPName, Accessor, rtrim(Accessor) + ' / ' + Rtrim(TPName) As NationalityName FROM ExtraInfo Where TP='NatE' ORDER BY TPNo"

        Using conn As New SqlConnection(connString)
            Using da As New SqlDataAdapter(query, conn)
                da.Fill(dt)
            End Using
        End Using

        ddlPersonNationality.DataSource = dt
        ddlPersonNationality.DataTextField = "NationalityName"
        ddlPersonNationality.DataValueField = "TPNo"
        ddlPersonNationality.DataBind()
        ddlPersonNationality.Items.Insert(0, New ListItem("اختر الجنسية", ""))
    End Sub

    Private Sub LoadClauses()
        Dim dt As New DataTable()
        Dim connString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
        Dim query As String = "SELECT [CoverNo], rtrim([CoverName]) As CoverName FROM [Covers] WHERE ([SubSystem] = '27')"

        Using conn As New SqlConnection(connString)
            Using da As New SqlDataAdapter(query, conn)
                da.Fill(dt)
            End Using
        End Using

        ddlClauses.DataSource = dt
        ddlClauses.DataTextField = "CoverName"
        ddlClauses.DataValueField = "CoverNo"
        ddlClauses.DataBind()
        ddlClauses.Items.Insert(0, New ListItem("اختر نوع التغطية", ""))
    End Sub

    Private Sub LoadDestinations()
        Dim dt As New DataTable()
        Dim connString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
        Dim query As String = "SELECT TPNo, rtrim(TPName) +' || '+ rtrim(Accessor) as TPName FROM EXTRAINFO where tp='AreaT' ORDER BY TPNo"

        Using conn As New SqlConnection(connString)
            Using da As New SqlDataAdapter(query, conn)
                da.Fill(dt)
            End Using
        End Using

        ddlDestination.DataSource = dt
        ddlDestination.DataTextField = "TPName"
        ddlDestination.DataValueField = "TPNo"
        ddlDestination.DataBind()
        ddlDestination.Items.Insert(0, New ListItem("اختر الوجهة", ""))
    End Sub

    Private Sub CalculateToDate()
        If Not String.IsNullOrEmpty(txtInsuranceFrom.Text) AndAlso Not String.IsNullOrEmpty(txtInsuranceDays.Text) Then
            Dim fromDate As Date
            Dim days As Integer
            If Date.TryParse(txtInsuranceFrom.Text, fromDate) AndAlso Integer.TryParse(txtInsuranceDays.Text, days) Then
                Dim toDate As Date = fromDate.AddDays(days - 1)
                txtInsuranceTo.Text = toDate.ToString("yyyy-MM-dd")
            End If
        End If
    End Sub

    Public Shared Function CalculatePremiumValues(days As Integer, clause As Integer, destination As Integer, fromDate As String, dob As String) As PremiumResult
        Dim birthDate As Date
        Dim startDate As Date
        Dim age As Integer = 0

        If Date.TryParse(dob, birthDate) AndAlso Date.TryParse(fromDate, startDate) Then
            Dim endDate As Date = startDate.AddDays(days - 1)
            age = endDate.Year - birthDate.Year
            If birthDate > endDate.AddYears(-age) Then age -= 1
        End If

        ' --- Your premium logic (example) ---
        Dim baseRate As Decimal = 5D
        Select Case destination
            Case 1 : baseRate = 8D
            Case 2 : baseRate = 6D
            Case 3 : baseRate = 7D
            Case 4 : baseRate = 12D
        End Select

        If age >= 60 Then baseRate *= 1.5D
        If age <= 17 Then baseRate *= 0.8D

        Select Case clause
            Case 1 : baseRate *= 1D
            Case 2 : baseRate *= 1.3D
            Case 3 : baseRate *= 1.8D
        End Select

        Dim netPremium As Decimal = baseRate * days
        Dim stampFee As Decimal = 0.25D
        Dim issueFee As Decimal = 10D
        Dim tax As Decimal = Math.Ceiling(netPremium * 0.01D * 2) / 2
        Dim supervisionFee As Decimal = netPremium * 0.005D
        Dim totalPremium As Decimal = netPremium + tax + supervisionFee + stampFee + issueFee

        Return New PremiumResult With {
            .NetPremium = netPremium.ToString("F3"),
            .Tax = tax.ToString("F3"),
            .ControlFees = supervisionFee.ToString("F3"),
            .StampFees = stampFee.ToString("F3"),
            .IssueFees = issueFee.ToString("F3"),
            .TotalPremium = totalPremium.ToString("F3")
        }
    End Function

    Private Sub UpdatePremiumLabels(result As PremiumResult)
        lblNetPremium.Text = result.NetPremium
        lblTax.Text = result.Tax
        lblControlFees.Text = result.ControlFees
        lblStampFees.Text = result.StampFees
        lblIssueFees.Text = result.IssueFees
        lblTotalPremium.Text = result.TotalPremium
    End Sub

    <WebMethod()>
    Public Shared Function CalculatePremium(days As Integer, clause As Integer, destination As Integer, fromDate As String, dob As String) As PremiumResult
        Return CalculatePremiumValues(days, clause, destination, fromDate, dob)

    End Function

End Class

Public Class PremiumResult
    Public Property NetPremium As String
    Public Property Tax As String
    Public Property ControlFees As String
    Public Property StampFees As String
    Public Property IssueFees As String
    Public Property TotalPremium As String
End Class