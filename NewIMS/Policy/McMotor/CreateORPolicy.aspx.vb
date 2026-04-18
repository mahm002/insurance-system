Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports System.Web.UI
Imports System.Globalization

Partial Class CreateORPolicy
    Inherits Page

    Private ReadOnly Property ConnectionString() As String
        Get
            Return ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' لا نقوم بتعيين txtInsuranceFrom.Text هنا، لأن الجافاسكريبت ستضبطه.
            ' ولكن يمكن تعيين قيمة احتياطية (لن تظهر لأن العميل سيغيرها فوراً)
            ' لتجنب أي فراغ في حال تعطيل الجافاسكريبت:
            txtInsuranceFrom.Text = DateTime.Now.ToString("dd-MM-yyyy")

            ' Populate car made year dropdown
            PopulateCarMadeYearDropdown()

            ' Load API dropdowns asynchronously
            Dim task = New PageAsyncTask(AddressOf LoadDropdownsAsync)
            RegisterAsyncTask(task)
            ExecuteRegisteredAsyncTasks()

            ' Show any error from previous submission
            If Not String.IsNullOrEmpty(Session("PolicyError")) Then
                lblResult.Text = Session("PolicyError").ToString()
                lblResult.CssClass = "feedback error d-block"
                Session.Remove("PolicyError")
            End If
        End If
    End Sub

    Private Sub PopulateCarMadeYearDropdown()
        Dim currentYear As Integer = DateTime.Now.Year
        For year As Integer = currentYear To 1950 Step -1
            ddlCarMadeYear.Items.Add(New ListItem(year.ToString(), year.ToString()))
        Next
        ddlCarMadeYear.Items.Insert(0, New ListItem("اختر سنة الصنع", ""))
    End Sub

    Private Async Function LoadDropdownsAsync() As Task
        Try
            Dim api As New ApiHelper()
            Dim carsTask = api.GetCarTypesAsync()
            Dim nationsTask = api.GetVehicleNationalitiesAsync()
            Dim countriesTask = api.GetInsuranceCountriesAsync()
            Dim clausesTask = api.GetClausesAsync()

            Await Task.WhenAll(carsTask, nationsTask, countriesTask, clausesTask)

            BindDropdownWithDefault(ddlCars, Await carsTask, "name", "id", "اختر نوع السيارة")
            BindDropdownWithDefault(ddlNationalities, Await nationsTask, "name", "id", "اختر جنسية المركبة")
            BindDropdownWithDefault(ddlCountries, Await countriesTask, "name", "id", "اختر البلد المزار")

            Dim clauses = Await clausesTask
            ddlClauses.DataSource = clauses
            ddlClauses.DataTextField = "slug"
            ddlClauses.DataValueField = "id"
            ddlClauses.DataBind()
            ddlClauses.Items.Insert(0, New ListItem("اختر البنود التأمينية", ""))

            If ddlClauses.Items.Count > 1 Then
                ddlClauses.SelectedIndex = 1
            End If
        Catch ex As Exception
            lblResult.Text = "❌ خطأ في تحميل البيانات: " & ex.Message
            lblResult.CssClass = "feedback error d-block"
        End Try
    End Function

    Private Sub BindDropdownWithDefault(ddl As DropDownList, data As List(Of LookupItem), textField As String, valueField As String, selectText As String)
        ddl.DataSource = data
        ddl.DataTextField = textField
        ddl.DataValueField = valueField
        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem(selectText, ""))
        If ddl.Items.Count > 1 Then
            ddl.SelectedIndex = 1
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        If hdnConfirmed.Value <> "true" Then Return
        hdnConfirmed.Value = "false"

        If Not ValidateForm() Then Return

        Dim loadingScript As String = "if (document.getElementById('btnShowConfirmation')) {" &
                                     "document.getElementById('btnShowConfirmation').disabled = true; " &
                                     "document.getElementById('btnShowConfirmation').innerHTML = '<span class=""spinner-border spinner-border-sm me-1""></span> جاري الإصدار...'; }"
        ScriptManager.RegisterStartupScript(Me, [GetType](), "ShowLoading", loadingScript, True)

        Dim task = New PageAsyncTask(AddressOf SubmitPolicyAsync)
        RegisterAsyncTask(task)
        ExecuteRegisteredAsyncTasks()
    End Sub

    Private Async Function SubmitPolicyAsync() As Task
        Try
            '' تحويل التاريخ من dd-MM-yyyy إلى DateTime
            'Dim fromDate As DateTime = ParseDateFromString(txtInsuranceFrom.Text.Trim())
            'Dim toDate As DateTime = ParseDateFromString(txtInsuranceTo.Text.Trim())
            ' تحويل التاريخ من dd-MM-yyyy إلى DateTime
            Dim fromDate As DateTime = ParseDateFromString(txtInsuranceFrom.Text.Trim())
            Dim days As Integer = CInt(txtInsuranceDays.Text)
            ' حساب تاريخ الانتهاء مباشرةً من تاريخ البداية وعدد الأيام
            Dim toDate As DateTime = fromDate.AddDays(days - 1)
            ' بناء الكائن المرسل للـ API
            Dim policy As New With {
                .user_name = ConfigurationManager.AppSettings("ApiUsername").ToString(),
                .pass_word = ConfigurationManager.AppSettings("ApiPassword").ToString(),
                .insurance_name = txtInsuranceName.Text.Trim(),
                .insurance_location = txtInsuranceLocation.Text.Trim(),
                .insurance_phone = txtInsurancePhone.Text.Trim(),
                .chassis_number = txtChassisNumber.Text.Trim(),
                .motor_number = "--",
                .plate_number = txtPlateNumber.Text.Trim(),
                .car_made_date = ddlCarMadeYear.SelectedValue,
                .cars_id = CInt(ddlCars.SelectedValue),
                .vehicle_nationalities_id = CInt(ddlNationalities.SelectedValue),
                .insurance_day_from = fromDate.ToString("yyyy-MM-dd"),
                .insurance_days_number = CInt(txtInsuranceDays.Text),
                .insurance_clauses_id = CInt(ddlClauses.SelectedValue),
                .insurance_country_number = CInt(ddlCountries.SelectedValue),
                .countries_id = CInt(ddlCountries.SelectedValue)
            }

            Dim api As New ApiHelper()
            Dim response As ApiResponse = Await api.CreateInsurancePolicyAsync(policy)

            If response Is Nothing OrElse String.IsNullOrEmpty(response.policyNumber) Then
                Throw New Exception("لم يتم إرجاع رقم وثيقة من API")
            End If

            Dim policyCard As String = response.policyNumber.TrimEnd()

            ' حفظ البيانات في قاعدة البيانات
            Await SaveToDatabaseAsync(response, policyCard, fromDate, toDate)

            ' التوجيه إلى صفحة الطباعة
            Dim redirectScript As String = "setTimeout(function() {window.location.href = 'PrintPolicy.aspx?policy=" & HttpUtility.JavaScriptStringEncode(policyCard) & "';" &
                                           "}, 100);"
            ScriptManager.RegisterStartupScript(Me, [GetType](), "RedirectToPrint", redirectScript, True)

        Catch ex As Exception
            lblResult.Text = "❌ فشل إصدار الوثيقة: " & ex.Message
            lblResult.CssClass = "feedback error d-block"

            Dim errorScript As String = "if (document.getElementById('btnShowConfirmation')) {" &
                                       "document.getElementById('btnShowConfirmation').disabled = false; " &
                                       "document.getElementById('btnShowConfirmation').innerHTML = 'إصدار الوثيقة'; }"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EnableButtonOnError", errorScript, True)
        End Try
    End Function

    Private Async Function SaveToDatabaseAsync(response As ApiResponse, policyCard As String, coverFrom As DateTime, coverTo As DateTime) As Task
        Await Task.Run(Sub()
                           Using connection As New SqlConnection(ConnectionString)
                               connection.Open()
                               Dim newId As Integer = 0

                               ' الحصول على رقم عميل جديد
                               Dim getMaxSql As String = "SELECT ISNULL(MAX(CAST(CustNo AS INT)), 0) + 1 FROM CustomerFile"
                               Using getMaxCmd As New SqlCommand(getMaxSql, connection)
                                   newId = Convert.ToInt32(getMaxCmd.ExecuteScalar())
                               End Using

                               ' إدراج عميل جديد باستخدام Stored Procedure
                               Dim insertSql As String = "NewCustomer"
                               Using command As New SqlCommand(insertSql, connection)
                                   command.CommandType = CommandType.StoredProcedure
                                   command.Parameters.AddWithValue("@CustName", response.data.insurance_name.TrimEnd())
                                   command.Parameters.AddWithValue("@CustNameE", response.data.insurance_name.TrimEnd())
                                   command.Parameters.AddWithValue("@TelNo", response.data.insurance_phone.TrimEnd())
                                   command.Parameters.AddWithValue("@Address", response.data.insurance_location.TrimEnd())
                                   command.Parameters.AddWithValue("@User", If(Session("User"), "System"))
                                   command.ExecuteNonQuery()
                               End Using

                               ' إدراج بيانات الوثيقة (PolicyFile) باستخدام معاملات آمنة
                               Dim policySql As String = "INSERT INTO PolicyFile " &
                                   "(PolNo, OrderNo, EndNo, LoadNo, IssuDate, IssuTime, EntryDate, CustNo, AgentNo, OwnNo, Broker, SubIns, Currency, ExcRate, PayType, AccountNo, CoverType, Measure, Interval, CoverFrom, CoverTo, LASTNET, NETPRM, TAXPRM, CONPRM, STMPRM, ISSPRM, EXTPRM, TOTPRM, ExtraRate, Stat, Inbox, ForLoss, Stop, Printed, financed, Discount, Branch, UserName, IssueUser, Commision) " &
                                   "VALUES (@PolNo, @OrderNo, @EndNo, @LoadNo, @IssuDate, @IssuTime, @EntryDate, @CustNo, @AgentNo, @OwnNo, @Broker, @SubIns, @Currency, @ExcRate, @PayType, @AccountNo, @CoverType, @Measure, @Interval, @CoverFrom, @CoverTo, @LASTNET, @NETPRM, @TAXPRM, @CONPRM, @STMPRM, @ISSPRM, @EXTPRM, @TOTPRM, @ExtraRate, @Stat, @Inbox, @ForLoss, @Stop, @Printed, @financed, @Discount, @Branch, @UserName, @IssueUser, @Commision)"

                               Using cmd As New SqlCommand(policySql, connection)
                                   cmd.Parameters.AddWithValue("@PolNo", policyCard)
                                   cmd.Parameters.AddWithValue("@OrderNo", policyCard)
                                   cmd.Parameters.AddWithValue("@EndNo", 0)
                                   cmd.Parameters.AddWithValue("@LoadNo", 0)
                                   cmd.Parameters.AddWithValue("@IssuDate", DateTime.Now)
                                   cmd.Parameters.AddWithValue("@IssuTime", DateTime.Now)
                                   cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now)
                                   cmd.Parameters.AddWithValue("@CustNo", newId)
                                   cmd.Parameters.AddWithValue("@AgentNo", 0)
                                   cmd.Parameters.AddWithValue("@OwnNo", 0)
                                   cmd.Parameters.AddWithValue("@Broker", 0)
                                   cmd.Parameters.AddWithValue("@SubIns", "OR")
                                   cmd.Parameters.AddWithValue("@Currency", 1)
                                   cmd.Parameters.AddWithValue("@ExcRate", 1)
                                   cmd.Parameters.AddWithValue("@PayType", 1)
                                   cmd.Parameters.AddWithValue("@AccountNo", 0)
                                   cmd.Parameters.AddWithValue("@CoverType", 1)
                                   cmd.Parameters.AddWithValue("@Measure", 1)
                                   cmd.Parameters.AddWithValue("@Interval", response.data.insurance_days_number)
                                   cmd.Parameters.AddWithValue("@CoverFrom", coverFrom)
                                   cmd.Parameters.AddWithValue("@CoverTo", coverTo)
                                   cmd.Parameters.AddWithValue("@LASTNET", CDec(response.data.insurance_installment))
                                   cmd.Parameters.AddWithValue("@NETPRM", CDec(response.data.insurance_installment))
                                   cmd.Parameters.AddWithValue("@TAXPRM", CDec(response.data.insurance_tax))
                                   cmd.Parameters.AddWithValue("@CONPRM", CDec(response.data.insurance_supervision))
                                   cmd.Parameters.AddWithValue("@STMPRM", CDec(response.data.insurance_stamp))
                                   cmd.Parameters.AddWithValue("@ISSPRM", CDec(response.data.insurance_version))
                                   cmd.Parameters.AddWithValue("@EXTPRM", 0)
                                   cmd.Parameters.AddWithValue("@TOTPRM", CDec(response.data.insurance_total))
                                   cmd.Parameters.AddWithValue("@ExtraRate", 0)
                                   cmd.Parameters.AddWithValue("@Stat", 1)
                                   cmd.Parameters.AddWithValue("@Inbox", 0)
                                   cmd.Parameters.AddWithValue("@ForLoss", 0)
                                   cmd.Parameters.AddWithValue("@Stop", 0)
                                   cmd.Parameters.AddWithValue("@Printed", 0)
                                   cmd.Parameters.AddWithValue("@financed", 0)
                                   cmd.Parameters.AddWithValue("@Discount", 0)
                                   cmd.Parameters.AddWithValue("@Branch", If(Session("Branch"), ""))
                                   cmd.Parameters.AddWithValue("@UserName", If(Session("UserId"), ""))
                                   cmd.Parameters.AddWithValue("@IssueUser", If(Session("UserId"), ""))
                                   cmd.Parameters.AddWithValue("@Commision", 0)
                                   cmd.ExecuteNonQuery()
                               End Using

                               ' إدراج بيانات المركبة (MOTORFILE)
                               Dim motorSql As String = "INSERT INTO [dbo].[MOTORFILE] ([OrderNo], [EndNo], [LoadNo], [SubIns], [BudyNo], [TableNo], [CarType], [CarColor], [MadeYear], [AreaCover], [PassNo], [PermZone], [Power], [Premium], [PermType]) " &
                                   "VALUES (@OrderNo, @EndNo, @LoadNo, @SubIns, @BudyNo, @TableNo, @CarType, @CarColor, @MadeYear, @AreaCover, @PassNo, @PermZone, @Power, @Premium, @PermType)"
                               Using cmd As New SqlCommand(motorSql, connection)
                                   cmd.Parameters.AddWithValue("@OrderNo", policyCard)
                                   cmd.Parameters.AddWithValue("@EndNo", 0)
                                   cmd.Parameters.AddWithValue("@LoadNo", 0)
                                   cmd.Parameters.AddWithValue("@SubIns", "OR")
                                   cmd.Parameters.AddWithValue("@BudyNo", response.data.chassis_number)
                                   cmd.Parameters.AddWithValue("@TableNo", txtPlateNumber.Text.Trim())
                                   cmd.Parameters.AddWithValue("@CarType", ddlCars.SelectedItem.Text)
                                   cmd.Parameters.AddWithValue("@CarColor", "/")
                                   cmd.Parameters.AddWithValue("@MadeYear", ddlCarMadeYear.SelectedValue)
                                   cmd.Parameters.AddWithValue("@AreaCover", ddlCountries.SelectedItem.Text)
                                   cmd.Parameters.AddWithValue("@PassNo", 4)
                                   cmd.Parameters.AddWithValue("@PermZone", ddlCountries.SelectedItem.Text)
                                   cmd.Parameters.AddWithValue("@Power", 0)
                                   cmd.Parameters.AddWithValue("@Premium", CDec(response.data.insurance_installment))
                                   cmd.Parameters.AddWithValue("@PermType", 0)
                                   cmd.ExecuteNonQuery()
                               End Using
                           End Using
                       End Sub)
    End Function

    Private Function ValidateForm() As Boolean
        ' التحقق من الحقول المطلوبة
        If String.IsNullOrWhiteSpace(txtInsuranceName.Text) OrElse
           String.IsNullOrWhiteSpace(txtInsuranceLocation.Text) OrElse
           String.IsNullOrWhiteSpace(txtInsurancePhone.Text) OrElse
           String.IsNullOrWhiteSpace(txtChassisNumber.Text) OrElse
           String.IsNullOrWhiteSpace(txtPlateNumber.Text) OrElse
           String.IsNullOrWhiteSpace(ddlCarMadeYear.SelectedValue) OrElse
           String.IsNullOrWhiteSpace(ddlCars.SelectedValue) OrElse
           String.IsNullOrWhiteSpace(ddlNationalities.SelectedValue) OrElse
           String.IsNullOrWhiteSpace(txtInsuranceFrom.Text) OrElse
           String.IsNullOrWhiteSpace(txtInsuranceDays.Text) OrElse
           String.IsNullOrWhiteSpace(ddlClauses.SelectedValue) OrElse
           String.IsNullOrWhiteSpace(ddlCountries.SelectedValue) Then

            lblResult.Text = "❌ يرجى ملء جميع الحقول المطلوبة"
            lblResult.CssClass = "feedback error d-block"
            Return False
        End If

        ' التحقق من عدد الأيام
        Dim days As Integer
        If Not Integer.TryParse(txtInsuranceDays.Text, days) OrElse days < 7 OrElse days > 90 Then
            lblResult.Text = "❌ عدد الأيام يجب أن يكون بين 7 و 90 يوم"
            lblResult.CssClass = "feedback error d-block"
            Return False
        End If

        ' التحقق من تاريخ البدء بصيغة dd-MM-yyyy
        Dim startDate As DateTime
        If Not DateTime.TryParseExact(txtInsuranceFrom.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, startDate) Then
            lblResult.Text = "❌ تاريخ البدء غير صحيح. استخدم الصيغة: يوم-شهر-سنة (مثال: 04-03-2026)"
            lblResult.CssClass = "feedback error d-block"
            Return False
        End If

        If startDate.Date < DateTime.Now.Date Then
            lblResult.Text = "❌ تاريخ البدء يجب أن يكون اليوم أو لاحقاً"
            lblResult.CssClass = "feedback error d-block"
            Return False
        End If

        Return True
    End Function

    Private Function ParseDateFromString(dateStr As String) As DateTime
        Return DateTime.ParseExact(dateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture)
    End Function

    Private Sub ClearForm()
        txtInsuranceName.Text = ""
        txtInsuranceLocation.Text = ""
        txtInsurancePhone.Text = ""
        txtChassisNumber.Text = ""
        txtPlateNumber.Text = ""
        ddlCarMadeYear.SelectedIndex = 0
        ddlCars.SelectedIndex = 0
        ddlNationalities.SelectedIndex = 0
        txtInsuranceFrom.Text = DateTime.Now.ToString("dd-MM-yyyy")
        txtInsuranceDays.Text = "7"
        txtInsuranceTo.Text = ""
        ddlClauses.SelectedIndex = 0
        ddlCountries.SelectedIndex = 0
    End Sub

    Protected Async Sub btnTestApi_Click(sender As Object, e As EventArgs)
        Try
            Dim api As New ApiHelper()
            Dim cars = Await api.GetCarTypesAsync()
            lblResult.Text = $"✅ تم تحميل {cars.Count} أنواع سيارات"
            lblResult.CssClass = "feedback success d-block"
        Catch ex As Exception
            lblResult.Text = "❌ خطأ: " & ex.Message
            lblResult.CssClass = "feedback error d-block"
        End Try
    End Sub
End Class