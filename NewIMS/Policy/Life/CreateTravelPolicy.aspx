<%@ Page Language="VB" AutoEventWireup="true"
    CodeBehind="CreateTravelPolicy.aspx.vb"
    Inherits="CreateTravelPolicy"
    Async="true"
    Culture="ar-SA" UICulture="ar" %>

<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Security-Policy: script-src 'self' https: http://localhost:57413 'unsafe-inline' 'unsafe-eval';">
    <title>إصدار وثيقة تأمين سفر</title>

    <%--<link rel="stylesheet" href="Content/bootstrap.min.css" />--%>
    <%--<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="../../Content/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="../../Content/bootstrap.min.css" rel="stylesheet" />--%>
    <%--<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">--%>
    <link href="../../Content/all.min.css" rel="stylesheet" />
    <style>
        /* Base RTL Styles with Sakkal Majalla */
        body {
            font-family: 'Sakkal Majalla', serif;
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            min-height: 100vh;
            text-align: right;
            direction: rtl;
            color: #333;
            font-size: 1.1rem;
        }

        input, select, textarea, button {
            font-family: 'Sakkal Majalla', 'Traditional Arabic', 'Times New Roman', serif;
        }

        /* باقي الأكواد الخاصة بالتصميم تبقى كما هي... */
        .container {
            max-width: 1300px;
        }

        .section-container {
            background: white;
            border-radius: 16px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.03);
            border: 1px solid rgba(0,0,0,0.02);
        }

        .section-title {
            margin-bottom: 20px;
            color: #1e2b4f;
            font-weight: 600;
            display: flex;
            align-items: center;
            gap: 10px;
            border-bottom: 1px solid #e9ecef;
            padding-bottom: 12px;
            font-size: 1.3rem;
        }

            .section-title i {
                color: #2a5c8a;
            }

        .grid-container {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 20px;
        }

        .form-group {
            margin-bottom: 0; /* handled by grid gap */
        }

        .form-label {
            font-weight: 500;
            margin-bottom: 8px;
            display: flex;
            align-items: center;
            gap: 5px;
            color: #2c3e50;
        }

        .required-field:after {
            content: " *";
            color: #dc3545;
        }

        .form-control, .form-select {
            border-radius: 10px;
            border: 1px solid #dee2e6;
            padding: 10px 15px;
            transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
        }

            .form-control:focus, .form-select:focus {
                border-color: #86b7fe;
                box-shadow: 0 0 0 0.25rem rgba(13,110,253,0.1);
            }

        /* Premium section redesign */
        .premium-section {
            background: linear-gradient(145deg, #f8fafc 0%, #f1f4f8 100%);
            border-radius: 20px;
            padding: 25px 20px;
            margin-top: 10px;
        }

        .premium-grid {
            display: grid;
            grid-template-columns: repeat(6, 1fr);
            gap: 15px;
        }

        .premium-item {
            background: white;
            border-radius: 16px;
            padding: 18px 8px;
            text-align: center;
            box-shadow: 0 5px 15px rgba(0,0,0,0.02);
            border: 1px solid rgba(255,255,255,0.5);
            backdrop-filter: blur(2px);
            transition: all 0.2s ease;
        }

            .premium-item:hover {
                transform: translateY(-4px);
                box-shadow: 0 15px 25px rgba(0,40,80,0.1);
                border-color: #cfe2ff;
            }

        .premium-label {
            font-size: 0.85rem;
            color: #4b6584;
            margin-bottom: 8px;
            font-weight: 500;
            letter-spacing: 0.3px;
        }

        .premium-value {
            font-size: 1.3rem;
            font-weight: 700;
            color: #1e3a8a;
            font-family: 'Courier New', monospace;
            direction: ltr;
        }

        /* Highlight total premium */
        .premium-item:last-child .premium-value {
            color: #0f6b3a;
            font-size: 1.6rem;
        }

        /* Responsive grid */
        @media (max-width: 1100px) {
            .premium-grid {
                grid-template-columns: repeat(3, 1fr);
            }
        }

        @media (max-width: 700px) {
            .premium-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 450px) {
            .premium-grid {
                grid-template-columns: 1fr;
            }
        }

        .button-container {
            display: flex;
            align-items: center;
            gap: 20px;
            margin-top: 30px;
            justify-content: center;
        }

        .btn-submit {
            padding: 14px 40px;
            font-size: 1.2rem;
            border-radius: 50px;
            background: linear-gradient(145deg, #1b4a2e, #0f3b22);
            border: none;
            box-shadow: 0 10px 20px rgba(0,80,20,0.2);
            transition: all 0.2s;
            color: white;
            font-weight: 600;
        }

            .btn-submit:hover {
                transform: scale(1.02);
                background: linear-gradient(145deg, #1f5e36, #124e2a);
                box-shadow: 0 15px 25px rgba(0,100,30,0.3);
            }

        .feedback {
            margin-top: 20px;
            padding: 15px 25px;
            border-radius: 50px;
            font-weight: 500;
            text-align: center;
        }

        .submitting {
            opacity: 0.7;
            pointer-events: none;
        }

        /* Optional debug alert styling */
        .debug-alert {
            background: white;
            border-right: 5px solid #2a5c8a;
            border-radius: 12px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.03);
            font-size: 0.9rem;
        }
    </style>
</head>
<body>
    <div class="container mt-4">
        <div class="alert debug-alert alert-dismissible fade show" role="alert">
            <i class="fas fa-info-circle me-2"></i>
            <strong>حالة الصفحة:</strong>
            <%= If(IsPostBack, "✅ تم الإرسال", "🔄 تحميل أولي") %> ·
            <strong>الطريقة:</strong> <%= Request.HttpMethod %> ·
            <strong>الوقت:</strong> <%= DateTime.Now.ToString("HH:mm:ss") %>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    </div>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <div class="container mt-4">
            <!-- 1. Traveler Details -->
            <div class="section-container">
                <h4 class="section-title"><i class="fas fa-user-tie"></i>بيانات المسافر</h4>
                <div class="grid-container">
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-user-circle"></i>اسم المسافر
                        </label>
                        <asp:TextBox ID="txtTravelerName" runat="server" CssClass="form-control" placeholder="الاسم الكامل" />
                    </div>
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-map-marker-alt"></i>العنوان
                        </label>
                        <asp:TextBox ID="txtTravelerLocation" runat="server" CssClass="form-control" placeholder="العنوان التفصيلي" />
                    </div>
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-phone"></i>الهاتف
                        </label>
                        <asp:TextBox ID="txtTravelerPhone" runat="server" CssClass="form-control" placeholder="09xxxxxxxx" />
                    </div>
                </div>
            </div>

            <!-- 2. Personal Details -->
            <div class="section-container">
                <h4 class="section-title"><i class="fas fa-id-card"></i>بيانات المسافر الشخصية</h4>
                <div class="grid-container">
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-passport"></i>رقم جواز السفر
                        </label>
                        <asp:TextBox ID="txtPassportNumber" runat="server" CssClass="form-control" placeholder="رقم جواز السفر" />
                    </div>
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-calendar-alt"></i>تاريخ الميلاد
                        </label>
                        <asp:TextBox ID="txtDateOfBirth" runat="server" TextMode="Date" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-venus-mars"></i>الجنس
                        </label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                            <asp:ListItem Text="اختر الجنس" Value="" Selected="True" />
                            <asp:ListItem Text="ذكر" Value="1" />
                            <asp:ListItem Text="أنثى" Value="2" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-flag"></i>الجنسية
                        </label>
                        <asp:DropDownList ID="ddlPersonNationality" runat="server" CssClass="form-select" />
                    </div>
                </div>
            </div>

            <!-- 3. Coverage & Trip Details -->
            <div class="section-container">
                <h4 class="section-title"><i class="fas fa-calendar-check"></i>مدة الرحلة وتفاصيلها</h4>
                <div class="grid-container">
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-play-circle"></i>من تاريخ
                        </label>
                        <asp:TextBox ID="txtInsuranceFrom" runat="server" TextMode="Date" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-clock"></i>عدد الأيام
                        </label>
                        <asp:TextBox ID="txtInsuranceDays" runat="server" TextMode="Number" CssClass="form-control"
                            min="1" max="1095" Text="7" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">
                            <i class="fas fa-stop-circle"></i>إلى تاريخ
                        </label>
                        <asp:TextBox ID="txtInsuranceTo" runat="server" TextMode="Date" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <label class="form-label required-field">
                            <i class="fas fa-file-signature"></i>نوع التغطية
                        </label>
                        <asp:DropDownList ID="ddlClauses" runat="server" CssClass="form-select" />
                    </div>
                    <div class="form-group full-width" style="grid-column: span 2;">
                        <label class="form-label required-field">
                            <i class="fas fa-globe-asia"></i>وجهة السفر
                        </label>
                        <asp:DropDownList ID="ddlDestination" runat="server" CssClass="form-select" />
                    </div>
                </div>
            </div>

            <!-- 4. Premium Details Section - NEW MODERN DESIGN -->
            <div class="section-container">
                <h4 class="section-title"><i class="fas fa-money-check-alt"></i>تفاصيل القسط</h4>
                <div class="premium-section">
                    <div class="premium-grid">
                        <div class="premium-item">
                            <div class="premium-label">صافي القسط</div>
                            <div class="premium-value">
                                <asp:Label ID="lblNetPremium" runat="server" Text="0.000" />
                            </div>
                        </div>
                        <div class="premium-item">
                            <div class="premium-label">الضريبة</div>
                            <div class="premium-value">
                                <asp:Label ID="lblTax" runat="server" Text="0.000" />
                            </div>
                        </div>
                        <div class="premium-item">
                            <div class="premium-label">رسوم الإشراف</div>
                            <div class="premium-value">
                                <asp:Label ID="lblControlFees" runat="server" Text="0.000" />
                            </div>
                        </div>
                        <div class="premium-item">
                            <div class="premium-label">رسوم الدمغة</div>
                            <div class="premium-value">
                                <asp:Label ID="lblStampFees" runat="server" Text="0.250" />
                            </div>
                        </div>
                        <div class="premium-item">
                            <div class="premium-label">رسوم الإصدار</div>
                            <div class="premium-value">
                                <asp:Label ID="lblIssueFees" runat="server" Text="10.000" />
                            </div>
                        </div>
                        <div class="premium-item">
                            <div class="premium-label">إجمالي القسط</div>
                            <div class="premium-value">
                                <asp:Label ID="lblTotalPremium" runat="server" Text="0.000" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Submit Button -->
            <div class="button-container">
                <asp:Button ID="btnSubmit" runat="server" Text="إصدار الوثيقة"
                    CssClass="btn btn-submit"
                    OnClick="BtnSubmit_Click"
                    OnClientClick="return confirmSubmit();" />
                <span id="spinner" class="spinner-border spinner-border-lg text-success" role="status" aria-hidden="true" style="display: none;"></span>
            </div>

            <!-- Feedback -->
            <asp:Label ID="lblResult" runat="server" CssClass="feedback d-none" />
        </div>
    </form>
    <%--<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>--%>
    <script src="../../scripts/bootstrap.bundle.js"></script>
    <%--<script src="../../Scripts/bootstrap.bundle.min.js"></script>--%>
    <%--<script src="../../scripts/bootstrap.bundle.min.js"></script>--%>

    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/js/all.min.js"></script>--%>
    <script src="../../scripts/all.min.js"></script>
    <script>
        function confirmSubmit() {
            document.getElementById('spinner').style.display = 'inline-block';
            document.getElementById('<%= btnSubmit.ClientID %>').classList.add('submitting');
            return confirm('هل أنت متأكد من إصدار وثيقة تأمين السفر؟\nلن يمكنك تعديل البيانات بعد الإصدار.');
        }

        function calculateToDate() {
            const from = document.getElementById('<%= txtInsuranceFrom.ClientID %>').value;
            const days = parseInt(document.getElementById('<%= txtInsuranceDays.ClientID %>').value) || 0;
            const toField = document.getElementById('<%= txtInsuranceTo.ClientID %>');

            if (!from || days < 1) {
                toField.value = '';
                return;
            }

            const toDate = new Date(from);
            toDate.setDate(toDate.getDate() + days - 1);
            toField.value = toDate.toISOString().split('T')[0];
        }

        function fetchPremium() {
            const days = document.getElementById('<%= txtInsuranceDays.ClientID %>').value;
            const clause = document.getElementById('<%= ddlClauses.ClientID %>').value;
            const dest = document.getElementById('<%= ddlDestination.ClientID %>').value;
            const from = document.getElementById('<%= txtInsuranceFrom.ClientID %>').value;
            const dob = document.getElementById('<%= txtDateOfBirth.ClientID %>').value;

            if (!days || !clause || !dest || !from || !dob) return;

            // Show a CSS spinner (no inline SVG, so no CSP issue)
            //document.querySelectorAll('.premium-value').forEach(el => {
            //    el.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';
            //});

            console.log('Calling PageMethods with:', { days, clause, dest, from, dob });

            PageMethods.CalculatePremium(
                parseInt(days), parseInt(clause), parseInt(dest), from, dob,
                result => {
                    console.log('Success – raw result:', result);
                    const data = result.d ? result.d : result;
                    console.log('Data to use:', data);

                    // Get all label elements
                    const netPremiumEl = document.getElementById('<%= lblNetPremium.ClientID %>');
            const taxEl = document.getElementById('<%= lblTax.ClientID %>');
            const controlFeesEl = document.getElementById('<%= lblControlFees.ClientID %>');
            const stampFeesEl = document.getElementById('<%= lblStampFees.ClientID %>');
            const issueFeesEl = document.getElementById('<%= lblIssueFees.ClientID %>');
            const totalPremiumEl = document.getElementById('<%= lblTotalPremium.ClientID %>');

            // Update only if element exists
            if (netPremiumEl) netPremiumEl.textContent = data.NetPremium || '0.000';
            if (taxEl) taxEl.textContent = data.Tax || '0.000';
            if (controlFeesEl) controlFeesEl.textContent = data.ControlFees || '0.000';
            if (stampFeesEl) stampFeesEl.textContent = data.StampFees || '0.250';
            if (issueFeesEl) issueFeesEl.textContent = data.IssueFees || '10.000';
            if (totalPremiumEl) totalPremiumEl.textContent = data.TotalPremium || '0.000';
        },
        err => {
            console.error('PageMethods error:', err);
            // Reset to defaults
            const netPremiumEl = document.getElementById('<%= lblNetPremium.ClientID %>');
            const taxEl = document.getElementById('<%= lblTax.ClientID %>');
            const controlFeesEl = document.getElementById('<%= lblControlFees.ClientID %>');
            const stampFeesEl = document.getElementById('<%= lblStampFees.ClientID %>');
            const issueFeesEl = document.getElementById('<%= lblIssueFees.ClientID %>');
            const totalPremiumEl = document.getElementById('<%= lblTotalPremium.ClientID %>');

            if (netPremiumEl) netPremiumEl.textContent = '0.000';
            if (taxEl) taxEl.textContent = '0.000';
            if (controlFeesEl) controlFeesEl.textContent = '0.000';
            if (stampFeesEl) stampFeesEl.textContent = '0.250';
            if (issueFeesEl) issueFeesEl.textContent = '10.000';
            if (totalPremiumEl) totalPremiumEl.textContent = '0.000';
        }
    );
        }

        document.addEventListener('DOMContentLoaded', () => {
            const fromEl = document.getElementById('<%= txtInsuranceFrom.ClientID %>');
            const daysEl = document.getElementById('<%= txtInsuranceDays.ClientID %>');

            if (fromEl) fromEl.addEventListener('change', calculateToDate);
            if (daysEl) daysEl.addEventListener('input', calculateToDate);

            [daysEl, document.getElementById('<%= ddlClauses.ClientID %>'),
                document.getElementById('<%= ddlDestination.ClientID %>'),
                fromEl, document.getElementById('<%= txtDateOfBirth.ClientID %>')]
                .forEach(el => {
                    if (el) el.addEventListener('change', () => setTimeout(fetchPremium, 100));
                });

            calculateToDate();
            setTimeout(fetchPremium, 300);
        });
    </script>
</body>
</html>