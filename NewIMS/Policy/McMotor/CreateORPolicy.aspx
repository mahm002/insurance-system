<%@ Page Language="VB" AutoEventWireup="true"
    CodeBehind="CreateORPolicy.aspx.vb"
    Inherits="CreateORPolicy"
    Async="true"
    Culture="ar-SA" UICulture="ar" %>

<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>إصدار وثيقة تأمين</title>

    <link href="../../Content/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">
    <!-- Flatpickr CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/themes/material_blue.css">

    <style>
        /* ---------- المتغيرات العامة ---------- */
        :root {
            --primary: #4361ee;
            --primary-dark: #3a56d4;
            --secondary: #7209b7;
            --success: #4cc9f0;
            --light: #f8f9fa;
            --dark: #212529;
            --gradient-primary: linear-gradient(135deg, #4361ee 0%, #3a56d4 100%);
            --gradient-success: linear-gradient(135deg, #4cc9f0 0%, #4895ef 100%);
            --shadow-sm: 0 2px 8px rgba(0,0,0,0.05);
            --shadow-md: 0 4px 16px rgba(0,0,0,0.08);
            --shadow-lg: 0 8px 30px rgba(0,0,0,0.12);
            --border-radius-sm: 10px;
            --border-radius-md: 12px;
            --border-radius-lg: 16px;
        }

        body {
            font-family: 'Sakkal Majalla', serif;
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            min-height: 100vh;
            text-align: right;
            direction: rtl;
            color: #333;
            font-size: 1.1rem;
        }

        h1, h2, h3, h4, h5, h6 {
            font-family: 'Sakkal Majalla', serif;
            font-weight: bold;
        }

        .container {
            max-width: 100% !important;
            width: 100% !important;
            padding: 0 15px;
            margin: 1rem auto;
        }

        /* ---------- الحاوية الرئيسية للقسم ---------- */
        .section-container {
            background: white;
            border-radius: var(--border-radius-lg);
            padding: 1.5rem;
            margin-bottom: 1.5rem;
            box-shadow: var(--shadow-md);
            border: none;
            position: relative;
            overflow: hidden;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }

        .section-container:hover {
            transform: translateY(-4px);
            box-shadow: var(--shadow-lg);
        }

        /* الحد الأيمن باللون الأسود */
        .section-container::before {
            content: '';
            position: absolute;
            top: 0;
            right: 0;
            width: 6px;
            height: 100%;
            background: #000000; /* أسود */
        }

        /* عنوان القسم باللون الأخضر */
        .section-title {
            font-weight: 700;
            color: #28a745; /* أخضر */
            margin-bottom: 1.5rem;
            padding-bottom: 0.75rem;
            border-bottom: 2px solid #f0f3ff;
            font-size: 1.4rem;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .section-title i {
            background: none;
            color: #28a745; /* أخضر */
        }

        /* الشبكات والنماذج - التعديل الأساسي لجعل الحقول تمتد */
        .grid-container {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 1.5rem;
            margin-bottom: 1rem;
            width: 100%;
        }

        /* لشبكة بيانات المركبة (5 أعمدة على الشاشات الكبيرة) */
        @media (min-width: 992px) {
            .car-details-grid {
                grid-template-columns: repeat(5, 1fr) !important;
                gap: 20px;
            }
        }

        .form-group {
            margin-bottom: 1.25rem;
        }

        .form-label {
            font-weight: 600;
            color: #495057;
            margin-bottom: 0.5rem;
            font-size: 0.95rem;
            display: flex;
            align-items: center;
            gap: 5px;
        }

        .form-control, .form-select {
            border: 2px solid #e2e8f0;
            border-radius: var(--border-radius-sm);
            padding: 0.75rem 1rem;
            font-size: 1rem;
            transition: all 0.3s ease;
            box-shadow: var(--shadow-sm);
            text-align: right;
            font-family: 'Sakkal Majalla', serif;
        }

        .form-control:focus, .form-select:focus {
            border-color: var(--primary);
            box-shadow: 0 0 0 3px rgba(67, 97, 238, 0.15);
            outline: none;
        }

        .required-field::after {
            content: " *";
            color: #e63946;
            font-weight: bold;
        }

        /* بطاقات الأقساط */
        .premium-section {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(140px, 1fr));
            gap: 1.25rem;
            margin-bottom: 1.5rem;
        }

        .premium-item {
            background: white;
            padding: 1.25rem 0.75rem;
            border-radius: var(--border-radius-md);
            border: 2px solid #f1f5f9;
            text-align: center;
            transition: all 0.3s ease;
            box-shadow: var(--shadow-sm);
        }

        .premium-item:hover {
            transform: translateY(-3px);
            box-shadow: var(--shadow-md);
            border-color: #e2e8f0;
        }

        .premium-label {
            font-size: 0.85rem;
            color: #64748b;
            margin-bottom: 0.5rem;
            font-weight: 600;
        }

        .premium-value {
            font-weight: 800;
            font-size: 1.3rem;
            color: var(--primary);
        }

        /* الأزرار */
        .btn-submit {
            background: var(--gradient-success);
            border: none;
            border-radius: var(--border-radius-sm);
            padding: 0.875rem 2rem;
            font-weight: 700;
            font-size: 1rem;
            transition: all 0.3s ease;
            box-shadow: var(--shadow-md);
            min-width: 140px;
            font-family: 'Sakkal Majalla', serif;
        }

        .btn-submit:hover {
            transform: translateY(-3px);
            box-shadow: var(--shadow-lg);
        }

        .button-container {
            display: flex;
            justify-content: flex-start;
            gap: 0rem;
            margin-top: 1rem;
            padding-top: 1rem;
        }

        /* رسائل التغذية الراجعة */
        .feedback {
            margin-top: 1rem;
            padding: 1rem 1.5rem;
            border-radius: var(--border-radius-md);
            font-weight: 500;
            border-right: 4px solid;
            animation: slideIn 0.3s ease;
        }

        .feedback.error {
            background-color: #f8d7da;
            border-color: #dc3545;
            color: #721c24;
        }

        .feedback.success {
            background-color: #d4edda;
            border-color: #28a745;
            color: #155724;
        }

        .invalid-feedback {
            display: block;
            color: #e63946;
            font-size: 0.85rem;
            margin-top: 0.4rem;
            font-weight: 500;
        }

        .form-control.is-invalid, .form-select.is-invalid {
            border-color: #e63946;
        }

        .form-control.is-valid, .form-select.is-valid {
            border-color: #28a745;
        }

        @keyframes slideIn {
            from {
                opacity: 0;
                transform: translateY(10px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        /* استجابات الشاشات */
        @media (max-width: 768px) {
            .container {
                padding: 1rem;
            }
            .section-container {
                padding: 1rem;
            }
            .grid-container {
                grid-template-columns: 1fr;
            }
            .premium-section {
                grid-template-columns: repeat(2, 1fr);
            }
            .btn-submit {
                width: 100%;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" novalidate>
        <div class="container mt-4">
            <!-- 1. المؤمن له -->
            <div class="section-container">
                <h4 class="section-title"><i class="fas fa-user-tie"></i>المؤمن له</h4>
                <div class="grid-container">
                    <div class="form-group">
                        <label for="txtInsuranceName" class="form-label required-field">اسم المؤمن</label>
                        <asp:TextBox ID="txtInsuranceName" runat="server" CssClass="form-control" required="true" placeholder="أدخل اسم المؤمن بالكامل" />
                        <div class="invalid-feedback">يرجى إدخال اسم المؤمن</div>
                    </div>
                    <div class="form-group">
                        <label for="txtInsuranceLocation" class="form-label required-field">العنوان</label>
                        <asp:TextBox ID="txtInsuranceLocation" runat="server" CssClass="form-control" required="true" placeholder="أدخل العنوان التفصيلي" />
                        <div class="invalid-feedback">يرجى إدخال العنوان</div>
                    </div>
                    <div class="form-group">
                        <label for="txtInsurancePhone" class="form-label required-field">الهاتف</label>
                        <asp:TextBox ID="txtInsurancePhone" runat="server" CssClass="form-control" required="true" placeholder="09xxxxxxxx" pattern="^[\+]?[0-9\s\-\(\)]{10,}$" />
                        <div class="invalid-feedback">يرجى إدخال رقم هاتف صحيح</div>
                    </div>
                </div>
            </div>

            <!-- 2. بيانات المركبة -->
            <div class="section-container">
                <h4 class="section-title"><i class="fas fa-car"></i>بيانات المركبة</h4>
                <div class="grid-container car-details-grid">
                    <div class="form-group">
                        <label for="txtPlateNumber" class="form-label required-field">اللوحة المعدنية</label>
                        <asp:TextBox ID="txtPlateNumber" runat="server" CssClass="form-control" required="true" placeholder="أدخل اللوحة المعدنية" />
                        <div class="invalid-feedback">يرجى إدخال اللوحة المعدنية</div>
                    </div>
                    <div class="form-group">
                        <label for="txtChassisNumber" class="form-label required-field">رقم الهيكل</label>
                        <asp:TextBox ID="txtChassisNumber" runat="server" CssClass="form-control" required="true" MaxLength="17" pattern="[A-Za-z0-9]{1,17}" placeholder="أدخل رقم الهيكل (حروف وأرقام فقط)" />
                        <div class="invalid-feedback">يرجى إدخال رقم هيكل صحيح من 1-17 خانة</div>
                    </div>
                    <div class="form-group">
                        <label for="ddlCarMadeYear" class="form-label required-field">سنة الصنع</label>
                        <asp:DropDownList ID="ddlCarMadeYear" runat="server" CssClass="form-select" required="true" />
                        <div class="invalid-feedback">يرجى اختيار سنة الصنع</div>
                    </div>
                    <div class="form-group">
                        <label for="ddlCars" class="form-label required-field">النوع</label>
                        <asp:DropDownList ID="ddlCars" runat="server" CssClass="form-select" required="true">
                            <asp:ListItem Text="اختر نوع السيارة" Value="" Selected="true" />
                        </asp:DropDownList>
                        <div class="invalid-feedback">يرجى اختيار نوع السيارة</div>
                    </div>
                    <div class="form-group">
                        <label for="ddlNationalities" class="form-label required-field">جنسية المركبة</label>
                        <asp:DropDownList ID="ddlNationalities" runat="server" CssClass="form-select" required="true" Enabled="false">
                            <asp:ListItem Text="اختر جنسية المركبة" Value="" Selected="true" />
                        </asp:DropDownList>
                        <div class="invalid-feedback">يرجى اختيار جنسية المركبة</div>
                    </div>
                </div>
            </div>

            <!-- 3. مدة التأمين -->
            <div class="section-container">
                <h4 class="section-title"><i class="fas fa-calendar-check"></i>مدة التأمين وتفاصيله</h4>
                <div class="grid-container">
                    <div class="form-group">
                        <label for="txtInsuranceFrom" class="form-label required-field">من يوم</label>
                        <asp:TextBox ID="txtInsuranceFrom" runat="server" CssClass="form-control" required="true" placeholder="dd-mm-yyyy" />
                        <div class="invalid-feedback">يرجى تحديد تاريخ البدء (التنسيق: يوم-شهر-سنة)</div>
                    </div>
                    <div class="form-group">
                        <label for="txtInsuranceDays" class="form-label required-field">عدد الأيام</label>
                        <asp:TextBox ID="txtInsuranceDays" runat="server" TextMode="Number" CssClass="form-control" required="true" min="7" max="90" value="7" placeholder="7-90 يوم" />
                        <div class="invalid-feedback">يرجى إدخال عدد الأيام (7-90)</div>
                    </div>
                    <div class="form-group">
                        <label for="txtInsuranceTo" class="form-label">إلى يوم</label>
                        <asp:TextBox ID="txtInsuranceTo" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <label for="ddlClauses" class="form-label required-field">البنود</label>
                        <asp:DropDownList ID="ddlClauses" runat="server" CssClass="form-select" required="true">
                            <asp:ListItem Text="اختر البنود التأمينية" Value="" Selected="true" />
                        </asp:DropDownList>
                        <div class="invalid-feedback">يرجى اختيار البند</div>
                    </div>
                    <div class="form-group">
                        <label for="ddlCountries" class="form-label required-field">البلد المزار</label>
                        <asp:DropDownList ID="ddlCountries" runat="server" CssClass="form-select" required="true">
                            <asp:ListItem Text="اختر البلد المزار" Value="" Selected="true" />
                        </asp:DropDownList>
                        <div class="invalid-feedback">يرجى اختيار البلد المزار</div>
                    </div>
                </div>
            </div>

            <!-- 4. تفاصيل القسط -->
            <div class="section-container">
                <h4 class="section-title"><i class="fas fa-money-check-alt"></i>تفاصيل القسط</h4>
                <div class="premium-section">
                    <div class="premium-item">
                        <div class="premium-label">صافي القسط</div>
                        <div class="premium-value" id="lblNetPremium">49.000</div>
                    </div>
                    <div class="premium-item">
                        <div class="premium-label">الضريبة</div>
                        <div class="premium-value" id="lblTax">1.000</div>
                    </div>
                    <div class="premium-item">
                        <div class="premium-label">رسوم الإشراف</div>
                        <div class="premium-value" id="lblControlFees">0.500</div>
                    </div>
                    <div class="premium-item">
                        <div class="premium-label">رسوم الدمغة</div>
                        <div class="premium-value" id="lblStampFees">0.250</div>
                    </div>
                    <div class="premium-item">
                        <div class="premium-label">رسوم الإصدار</div>
                        <div class="premium-value" id="lblIssueFees">10.000</div>
                    </div>
                    <div class="premium-item">
                        <div class="premium-label">إجمالي القسط</div>
                        <div class="premium-value" id="lblTotalPremium">59.995</div>
                    </div>
                </div>
            </div>

            <!-- زر الإصدار -->
            <div class="button-container">
                <div class="w-100">
                    <asp:HiddenField ID="hdnConfirmed" runat="server" Value="false" />
                    <asp:Button ID="btnSubmitReal" runat="server" Text="إصدار الوثيقة" CssClass="btn btn-success btn-submit w-100" OnClick="btnSubmit_Click" CausesValidation="true" Style="display: none;" />
                    <button type="button" id="btnShowConfirmation" class="btn btn-success btn-submit w-100">إصدار الوثيقة</button>
                </div>
            </div>

            <asp:Label ID="lblResult" runat="server" CssClass="feedback d-none mt-3" />
        </div>
    </form>

    <!-- السكربتات -->
    <script src="../../Scripts/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/js/all.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <script>
        // دالة مساعدة لتنسيق التاريخ إلى dd-mm-yyyy
        function formatDDMMYYYY(date) {
            let d = new Date(date);
            let day = String(d.getDate()).padStart(2, '0');
            let month = String(d.getMonth() + 1).padStart(2, '0');
            let year = d.getFullYear();
            return `${day}-${month}-${year}`;
        }

        // الحصول على حقل تاريخ البدء
        const fromDateInput = document.getElementById('<%= txtInsuranceFrom.ClientID %>');

        // تهيئة Flatpickr - استخدام اللغة الإنجليزية لضمان التقويم الميلادي
        flatpickr(fromDateInput, {
            dateFormat: "d-m-Y",
            locale: "en",          // القوة على الميلادي
            allowInput: true,
            minDate: new Date(),   // اليوم بالميلادي
            defaultDate: new Date(),
            onChange: function (selectedDates, dateStr, instance) {
                calculateToDate();
                if (selectedDates.length > 0) {
                    instance.input.classList.remove('is-invalid');
                    instance.input.classList.add('is-valid');
                } else {
                    instance.input.classList.add('is-invalid');
                    instance.input.classList.remove('is-valid');
                }
            }
        });

        // ضبط القيمة الظاهرة يدوياً بالميلادي
        fromDateInput.value = formatDDMMYYYY(new Date());

        // --- دوال التحقق وحساب الأقساط ---
        function validateForm() {
            const form = document.getElementById('form1');
            let isValid = true;

            // فحص جميع الحقول المطلوبة
            const inputs = form.querySelectorAll('input[required], select[required]');
            inputs.forEach(input => { if (!validateField(input)) isValid = false; });

            // فحص تاريخ البدء
            const fromVal = fromDateInput.value;
            if (fromVal) {
                const parts = fromVal.split('-');
                if (parts.length === 3) {
                    const fromDate = new Date(parts[2], parts[1] - 1, parts[0]);
                    const today = new Date(); today.setHours(0, 0, 0, 0);
                    fromDate.setHours(0, 0, 0, 0);
                    if (isNaN(fromDate.getTime()) || fromDate < today) {
                        isValid = false;
                        fromDateInput.classList.add('is-invalid');
                    } else {
                        fromDateInput.classList.remove('is-invalid');
                        fromDateInput.classList.add('is-valid');
                    }
                } else {
                    isValid = false;
                    fromDateInput.classList.add('is-invalid');
                }
            } else {
                isValid = false;
                fromDateInput.classList.add('is-invalid');
            }

            // فحص عدد الأيام
            const daysInput = document.getElementById('<%= txtInsuranceDays.ClientID %>');
            const days = parseInt(daysInput.value);
            if (isNaN(days) || days < 7 || days > 90) {
                isValid = false;
                daysInput.classList.add('is-invalid');
            } else {
                daysInput.classList.remove('is-invalid');
                daysInput.classList.add('is-valid');
            }

            return isValid;
        }

        function validateField(field) {
            const feedback = field.nextElementSibling;
            if (field.checkValidity()) {
                field.classList.remove('is-invalid');
                field.classList.add('is-valid');
                if (feedback && feedback.classList.contains('invalid-feedback')) feedback.style.display = 'none';
                return true;
            } else {
                field.classList.remove('is-valid');
                field.classList.add('is-invalid');
                if (feedback && feedback.classList.contains('invalid-feedback')) feedback.style.display = 'block';
                return false;
            }
        }

        // ربط الأحداث للتحقق الفوري
        document.querySelectorAll('input[required], select[required]').forEach(field => {
            field.addEventListener('input', () => validateField(field));
            field.addEventListener('change', () => validateField(field));
        });

        function showFeedback(message, type) {
            const feedback = document.getElementById('<%= lblResult.ClientID %>');
            if (feedback) {
                feedback.innerHTML = message;
                feedback.className = `feedback d-block ${type === 'error' ? 'error' : 'success'}`;
            }
        }

        // حساب تاريخ الانتهاء
        function calculateToDate() {
            const fromVal = fromDateInput.value;
            const daysInput = document.getElementById('<%= txtInsuranceDays.ClientID %>');
            const toField = document.getElementById('<%= txtInsuranceTo.ClientID %>');
            if (!fromVal || !daysInput.value) return;
            const parts = fromVal.split('-');
            if (parts.length !== 3) return;
            const fromDate = new Date(parts[2], parts[1] - 1, parts[0]);
            const days = parseInt(daysInput.value) || 7;
            const today = new Date(); today.setHours(0, 0, 0, 0);
            fromDate.setHours(0, 0, 0, 0);
            if (isNaN(fromDate.getTime()) || fromDate < today) {
                toField.value = '';
                return;
            }
            if (days >= 7 && days <= 90) {
                const toDate = new Date(fromDate);
                toDate.setDate(toDate.getDate() + days - 1);
                toField.value = formatDDMMYYYY(toDate);
                toField.classList.add('is-valid');
                setTimeout(() => toField.classList.remove('is-valid'), 1000);
                calculatePremium();
            } else {
                toField.value = '';
            }
        }

        // حساب الأقساط
        function calculatePremium() {
            const days = parseInt(document.getElementById('<%= txtInsuranceDays.ClientID %>').value) || 7;
            const clauseId = parseInt(document.getElementById('<%= ddlClauses.ClientID %>').value);
            const countryId = document.getElementById('<%= ddlCountries.ClientID %>').value;
            if (!days || !clauseId || !countryId) return;

            const isPV = clauseId <= 5;
            const isCV = clauseId >= 6;
            let baseDailyRate = 7.000;

            if ((countryId == '1' || countryId == '2') && isPV) baseDailyRate = 7.000;
            else if (countryId == '3' && isPV) baseDailyRate = 10.000;
            else if ((countryId == '1' || countryId == '2') && isCV) baseDailyRate = 8.000;
            else if (countryId == '3' && isCV) baseDailyRate = 11.000;
            else if (countryId == '4' && (isPV || isCV)) {
                baseDailyRate = 3.000;
                if (days < 15) {
                    document.getElementById('<%= txtInsuranceDays.ClientID %>').value = '15';
                    const newNetPremium = (baseDailyRate * 15).toFixed(3);
                    const newTax = calculateCeiledTax(parseFloat(newNetPremium));
                    const newSupervisionFee = calculateSupervisionFee(parseFloat(newNetPremium));
                    const newTotalPremium = (parseFloat(newNetPremium) + parseFloat(newTax) + parseFloat(newSupervisionFee) + 0.250 + 10.000).toFixed(3);
                    document.getElementById('lblNetPremium').textContent = newNetPremium;
                    document.getElementById('lblTax').textContent = newTax;
                    document.getElementById('lblControlFees').textContent = newSupervisionFee;
                    document.getElementById('lblTotalPremium').textContent = newTotalPremium;
                    return;
                }
            }

            const netPremium = (baseDailyRate * days).toFixed(3);
            const supervisionFee = calculateSupervisionFee(parseFloat(netPremium));
            const stampFee = 0.250;
            const issueFee = 10.000;
            const tax = calculateCeiledTax(parseFloat(netPremium));
            const totalPremium = (parseFloat(netPremium) + parseFloat(tax) + parseFloat(supervisionFee) + stampFee + issueFee).toFixed(3);

            document.getElementById('lblNetPremium').textContent = netPremium;
            document.getElementById('lblTax').textContent = tax;
            document.getElementById('lblControlFees').textContent = supervisionFee;
            document.getElementById('lblStampFees').textContent = stampFee.toFixed(3);
            document.getElementById('lblIssueFees').textContent = issueFee.toFixed(3);
            document.getElementById('lblTotalPremium').textContent = totalPremium;
        }

        function calculateSupervisionFee(amount) {
            return (amount * 0.005).toFixed(3);
        }
        function calculateCeiledTax(amount) {
            const taxBeforeCeiling = amount * 0.01;
            const ceiledTax = Math.ceil(taxBeforeCeiling * 2) / 2;
            return ceiledTax.toFixed(3);
        }

        // أحداث تغيير القيمة لإعادة حساب القسط
        document.getElementById('<%= txtInsuranceDays.ClientID %>').addEventListener('input', calculatePremium);
        document.getElementById('<%= ddlClauses.ClientID %>').addEventListener('change', calculatePremium);
        document.getElementById('<%= ddlCountries.ClientID %>').addEventListener('change', calculatePremium);
        document.getElementById('<%= txtInsuranceFrom.ClientID %>').addEventListener('change', calculateToDate);
        document.getElementById('<%= txtInsuranceDays.ClientID %>').addEventListener('input', calculateToDate);

        // عند تحميل الصفحة
        window.addEventListener('load', function() {
            if (!document.getElementById('<%= txtInsuranceDays.ClientID %>').value) {
                document.getElementById('<%= txtInsuranceDays.ClientID %>').value = '7';
            }
            calculateToDate();
            setTimeout(calculatePremium, 100);

            // زر التأكيد
            const confirmBtn = document.getElementById('btnShowConfirmation');
            confirmBtn.addEventListener('click', function() {
                if (validateForm()) {
                    if (confirm("هل أنت متأكد من إصدار وثيقة التأمين؟")) {
                        this.disabled = true;
                        this.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> جاري الإصدار...';
                        document.getElementById('<%= hdnConfirmed.ClientID %>').value = 'true';
                        document.getElementById('<%= btnSubmitReal.ClientID %>').click();
                    }
                } else {
                    showFeedback('❌ يرجى ملء جميع الحقول المطلوبة بشكل صحيح', 'error');
                }
            });
        });

        // التنقل بالضغط على Enter
        document.addEventListener('keydown', function (e) {
            if (e.key !== 'Enter') return;
            const target = e.target;
            if (!target.matches('input, select') || target.matches('textarea')) return;
            e.preventDefault();
            const allInputs = Array.from(document.querySelectorAll('input, select')).filter(el => !el.readOnly && !el.disabled);
            const idx = allInputs.indexOf(target);
            if (idx >= 0 && idx < allInputs.length - 1) allInputs[idx + 1].focus();
        });
    </script>
</body>
</html>