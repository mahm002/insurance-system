<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ModernReceipt.aspx.vb" Inherits="ModernReceipt" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>سداد مدفوعات - النظام الحديث</title>
    
    <!-- Modern Styling -->
    <style>
        /* Modern Design System */
        :root {
            --primary-color: #4361ee;
            --secondary-color: #3a0ca3;
            --success-color: #4cc9f0;
            --danger-color: #f72585;
            --warning-color: #f8961e;
            --info-color: #7209b7;
            --light-bg: #f8f9fa;
            --dark-bg: #212529;
            --card-shadow: 0 8px 30px rgba(0, 0, 0, 0.08);
            --hover-shadow: 0 12px 40px rgba(0, 0, 0, 0.12);
            --border-radius: 12px;
            --transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            margin: 0;
            padding: 20px;
            min-height: 100vh;
        }

        /* Modern Container */
        .modern-container {
            max-width: 1200px;
            margin: 0 auto;
            background: white;
            border-radius: var(--border-radius);
            box-shadow: var(--card-shadow);
            overflow: hidden;
        }

        /* Modern Header */
        .modern-header {
            background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
            color: white;
            padding: 25px 30px;
            position: relative;
            overflow: hidden;
        }

        .modern-header::before {
            content: '';
            position: absolute;
            top: -50%;
            right: -50%;
            width: 200%;
            height: 200%;
            background: radial-gradient(circle, rgba(255,255,255,0.1) 1px, transparent 1px);
            background-size: 20px 20px;
            opacity: 0.1;
        }

        .header-title {
            font-size: 24px;
            font-weight: 700;
            margin: 0;
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .header-subtitle {
            font-size: 14px;
            opacity: 0.9;
            margin: 5px 0 0 0;
        }

        /* Modern Form Grid */
        .form-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 20px;
            margin: 20px;
        }

        .form-section {
            background: white;
            border-radius: var(--border-radius);
            padding: 25px;
            border: 1px solid rgba(0, 0, 0, 0.05);
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-label {
            display: block;
            margin-bottom: 8px;
            font-weight: 600;
            color: #333;
            font-size: 14px;
        }

        /* Amount Display */
        .amount-display {
            background: linear-gradient(135deg, #4cc9f0, #4895ef);
            color: white;
            padding: 20px;
            border-radius: var(--border-radius);
            text-align: center;
            margin: 20px;
        }

        .amount-label {
            font-size: 14px;
            opacity: 0.9;
            margin-bottom: 5px;
        }

        .amount-value {
            font-size: 32px;
            font-weight: 700;
            direction: ltr;
        }

        /* Modern Buttons */
        .dxbButton_DevEx.modern-btn {
            border-radius: 8px !important;
            font-weight: 600 !important;
            padding: 12px 30px !important;
            border: none !important;
            transition: var(--transition) !important;
            display: inline-flex !important;
            align-items: center !important;
            justify-content: center !important;
            gap: 8px !important;
        }

        .modern-btn-primary {
            background: linear-gradient(135deg, var(--primary-color), var(--secondary-color)) !important;
            color: white !important;
        }

        .modern-btn-success {
            background: linear-gradient(135deg, #4cc9f0, #4895ef) !important;
            color: white !important;
        }

        .modern-btn-danger {
            background: linear-gradient(135deg, var(--danger-color), #b5179e) !important;
            color: white !important;
        }

        .modern-btn:hover {
            transform: translateY(-2px) !important;
            box-shadow: 0 6px 20px rgba(0, 0, 0, 0.15) !important;
        }

        /* Payment Cards */
        .payment-options {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(140px, 1fr));
            gap: 10px;
            margin: 15px 0;
        }

        .payment-option {
            background: #f8f9fa;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            padding: 12px;
            text-align: center;
            cursor: pointer;
            transition: var(--transition);
        }

        .payment-option:hover {
            border-color: var(--primary-color);
            background: rgba(67, 97, 238, 0.05);
        }

        .payment-option.selected {
            border-color: var(--primary-color);
            background: rgba(67, 97, 238, 0.1);
        }

        .payment-icon {
            font-size: 20px;
            color: var(--primary-color);
            margin-bottom: 5px;
        }

        .payment-name {
            font-size: 13px;
            font-weight: 600;
            color: #333;
        }

        /* Grid Styling */
        .modern-grid {
            border-radius: var(--border-radius) !important;
            overflow: hidden !important;
            border: none !important;
        }

        /* Loading Animation */
        .modern-loading {
            display: inline-block;
            width: 20px;
            height: 20px;
            border: 3px solid rgba(67, 97, 238, 0.1);
            border-top: 3px solid var(--primary-color);
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }

        /* Responsive */
        @media (max-width: 768px) {
            .form-grid {
                grid-template-columns: 1fr;
                margin: 10px;
            }
            
            .form-section {
                padding: 15px;
            }
        }
    </style>

    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

    <script type="text/javascript">
        // Original functions preserved
        function preventBack() {
            window.history.forward();
        }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };

        function ReturnToParentPage() {
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup(1);
        }

        // Modified Sadad function with modern validation
        function Sadad(s, e) {
            if (ASPxClientEdit.ValidateGroup('ValidGroup')) {
                // Show modern confirmation
                var paymentType = PayTyp.GetText();
                var amount = Payed.GetText();
                var customer = Customer.GetText();

                var message = "تأكيد سداد مبلغ " + amount + " للعميل " + customer +
                    " باستخدام طريقة " + paymentType;

                // Use the existing confirmation popup
                pcConfirmIssue.SetHeaderText("تأكيد عملية السداد");
                pcConfirmIssue.Show();
            }
        }

        function YesIss_Click() {
            sdad.SetText('<span class="modern-loading"></span> جاري المعالجة...');
            sdad.SetEnabled(false);
            cbp.PerformCallback("Sadad");
            pcConfirmIssue.Hide();
        }

        function NoIss_Click() {
            pcConfirmIssue.Hide();
            sdad.SetText('<i class="fas fa-money-check"></i> سداد');
            sdad.SetEnabled(true);
        }

        function OnEndCallback(s, e) {
            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Issuance') {
                pcConfirmIssue.Show();
                pcConfirmIssue.SetHeaderText(s.cpCust);
            }
            s.cpMyAttribute = '';
            s.cpNewWindowUrl = null;
        }

        // Original HideOrShow function with modern updates
        function HideOrShow(s, e) {
            var value = s.GetValue();

            // Update UI based on payment type
            updatePaymentFieldsUI(value);

            // Perform callback for server-side updates
            cbp.PerformCallback('PaymentChanged');
        }

        function updatePaymentFieldsUI(paymentType) {
            // Reset all fields visibility
            AccNo.SetVisible(false);
            Bank.SetVisible(false);
            AccName.SetVisible(false);

            // Set labels and visibility based on payment type
            switch (paymentType) {
                case '1': // نقداً
                    // Nothing to show
                    break;
                case '2': // بصك
                    AccNo.SetVisible(true);
                    Bank.SetVisible(true);
                    AccNo.SetCaption('رقم الصك :');
                    Bank.SetCaption('على مصرف :');
                    break;
                case '3': // بإشعار
                    AccNo.SetVisible(true);
                    Bank.SetVisible(true);
                    AccName.SetVisible(true);
                    AccNo.SetCaption('رقم الإشعار :');
                    Bank.SetCaption('على مصرف :');
                    AccName.SetCaption('الحساب المصرفي رقم :');
                    break;
                case '4': // على الحساب
                    AccName.SetVisible(true);
                    AccName.SetCaption('حساب المدينون رقم :');
                    break;
                case '5': // بطاقة مصرفية
                    AccName.SetVisible(true);
                    AccName.SetCaption('الحساب المصرفي رقم :');
                    break;
                case '6': // تحت التحصيل
                    AccName.SetVisible(true);
                    AccName.SetCaption('حساب تحت التحصيل رقم :');
                    break;
            }

            // Update selected payment option
            $('.payment-option').removeClass('selected');
            $('.payment-option[data-value="' + paymentType + '"]').addClass('selected');
        }

        // Format amount on blur
        function formatAmount(s, e) {
            var value = s.GetText();
            if (value && value.trim() !== '') {
                var num = parseFloat(value.replace(/,/g, ''));
                if (!isNaN(num)) {
                    s.SetText(num.toLocaleString('en-US', {
                        minimumFractionDigits: 3,
                        maximumFractionDigits: 3
                    }));
                }
            }
        }

        // Initialize payment options
        function initPaymentOptions() {
            // Create payment options HTML
            var options = [
                { value: '1', icon: 'fas fa-money-bill-wave', name: 'نقداً' },
                { value: '2', icon: 'fas fa-file-invoice', name: 'بصك' },
                { value: '3', icon: 'fas fa-receipt', name: 'بإشعار' },
                { value: '4', icon: 'fas fa-user-clock', name: 'على الحساب' },
                { value: '5', icon: 'fas fa-credit-card', name: 'بطاقة مصرفية' },
                { value: '6', icon: 'fas fa-clock', name: 'تحت التحصيل' }
            ];

            var html = '';
            options.forEach(function (opt) {
                html += '<div class="payment-option" data-value="' + opt.value + '" onclick="selectPaymentOption(\'' + opt.value + '\')">' +
                    '<div class="payment-icon"><i class="' + opt.icon + '"></i></div>' +
                    '<div class="payment-name">' + opt.name + '</div>' +
                    '</div>';
            });

            $('.payment-options').html(html);

            // Set initial selection
            if (PayTyp && PayTyp.GetValue()) {
                var initialValue = PayTyp.GetValue().toString();
                updatePaymentFieldsUI(initialValue);
                $('.payment-option[data-value="' + initialValue + '"]').addClass('selected');
            }
        }

        function selectPaymentOption(value) {
            PayTyp.SetValue(value);
            HideOrShow(PayTyp, null);
        }

        // Initialize on page load
        $(document).ready(function () {
            initPaymentOptions();

            // Format initial amounts
            if (Payed && Payed.GetText()) {
                formatAmount(Payed, null);
            }
            if (TOTPRM && TOTPRM.GetText()) {
                formatAmount(TOTPRM, null);
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="modern-container">
            <!-- Header -->
            <div class="modern-header">
                <div>
                    <h1 class="header-title">
                        <i class="fas fa-file-invoice-dollar"></i>
                        <asp:Label ID="lblTitle" runat="server" Text="تسوية أو سداد وثيقة"></asp:Label>
                    </h1>
                    <p class="header-subtitle">
                        <i class="fas fa-calendar-alt"></i> <%= DateTime.Now.ToString("yyyy/MM/dd") %> | 
                        <i class="fas fa-user"></i> <%= Session("User") %>
                    </p>
                </div>
            </div>

            <!-- Amount Display -->
            <div class="amount-display">
                <div class="amount-label">القيمة المستحقة</div>
                <div class="amount-value">
                    <dx:ASPxTextBox ID="TOTPRM" runat="server" Width="100%" 
                        ClientEnabled="false" Border-BorderStyle="None" 
                        Font-Size="32px" HorizontalAlign="Center">
                        <Border BorderStyle="None" />
                        <ClientSideEvents Init="function(s,e){ formatAmount(s,e); }" />
                    </dx:ASPxTextBox>
                </div>
            </div>

            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" 
                OnCallback="Callback_Callback" Width="100%">
                <ClientSideEvents EndCallback="OnEndCallback" />
                <SettingsLoadingPanel Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <div class="form-grid">
                            <!-- Left Column: Customer & Payment Info -->
                            <div class="form-section">
                                <!-- Customer -->
                                <div class="form-group">
                                    <label class="form-label">
                                        <i class="fas fa-user"></i> العميل / من
                                    </label>
                                    <dx:ASPxTextBox ID="Customer" runat="server" Width="100%" 
                                        Height="40px">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="اسم العميل مطلوب" />
                                        </ValidationSettings>
                                        <Border BorderColor="#e0e0e0" BorderStyle="Solid" BorderWidth="1px" />
                                    </dx:ASPxTextBox>
                                </div>

                                <!-- Amount Paid -->
                                <div class="form-group">
                                    <label class="form-label">
                                        <i class="fas fa-money-bill-wave"></i> المبلغ المدفوع
                                    </label>
                                    <dx:ASPxTextBox ID="Payed" runat="server" Width="100%" 
                                        Height="40px">
                                        <ClientSideEvents LostFocus="formatAmount" />
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="المبلغ المدفوع مطلوب" />
                                        </ValidationSettings>
                                        <Border BorderColor="#e0e0e0" BorderStyle="Solid" BorderWidth="1px" />
                                    </dx:ASPxTextBox>
                                </div>

                                <!-- Payment Type -->
                                <div class="form-group">
                                    <label class="form-label">
                                        <i class="fas fa-credit-card"></i> نوع التسوية
                                    </label>
                                    
                                    <!-- Visual Payment Options -->
                                    <div class="payment-options"></div>
                                    
                                    <!-- Original ComboBox (hidden but functional) -->
                                    <dx:ASPxComboBox ID="PayTyp" runat="server" Width="100%"
                                        EnableCallbackMode="true" Height="40px"
                                        TextField="TPName" ValueField="TPNo"
                                        IncrementalFilteringMode="StartsWith"
                                        OnValueChanged="PayTyp_ValueChanged"
                                        RightToLeft="True" SelectedIndex="0" 
                                        DataSourceID="Pay" ClientInstanceName="PayTyp">
                                        <ClientSideEvents ValueChanged="HideOrShow" />
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="نوع التسوية مطلوب" />
                                        </ValidationSettings>
                                        <Border BorderColor="#e0e0e0" BorderStyle="Solid" BorderWidth="1px" />
                                    </dx:ASPxComboBox>
                                </div>

                                <!-- Transaction Date -->
                                <div class="form-group">
                                    <label class="form-label">
                                        <i class="fas fa-calendar-day"></i> تاريخ الحركة
                                    </label>
                                    <dx:ASPxDateEdit ID="MoveDate" runat="server" Width="100%" 
                                        Height="40px" OnInit="MoveDate_Init"
                                        ClientInstanceName="MoveDate">
                                        <Border BorderColor="#e0e0e0" BorderStyle="Solid" BorderWidth="1px" />
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>

                            <!-- Right Column: Payment Details & Notes -->
                            <div class="form-section">
                                <!-- Dynamic Payment Fields -->
                                <div id="dynamicFields">
                                    <!-- Account Name -->
                                    <div class="form-group">
                                        <dx:ASPxComboBox ID="AccName" runat="server" Width="100%"
                                            ClientInstanceName="AccName" ClientVisible="False"
                                            ValueType="System.String" RightToLeft="True" 
                                            ValueField="AccountNo" TextField="AccountName"
                                            TextFormatString="{1} || {0}" Height="40px">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="رقم الحساب" FieldName="AccountNo" />
                                                <dx:ListBoxColumn Caption="اسم الحساب" FieldName="AccountName" />
                                            </Columns>
                                            <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                                <RequiredField IsRequired="True" ErrorText="الحساب مطلوب" />
                                            </ValidationSettings>
                                            <Border BorderColor="#e0e0e0" BorderStyle="Solid" BorderWidth="1px" />
                                        </dx:ASPxComboBox>
                                    </div>

                                    <!-- Account Number -->
                                    <div class="form-group">
                                        <dx:ASPxTextBox ID="AccNo" runat="server" Width="100%"
                                            ClientInstanceName="AccNo" ClientVisible="False" 
                                            Text="/" Height="40px">
                                            <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                                <RequiredField IsRequired="True" ErrorText="رقم الصك/الإشعار مطلوب" />
                                            </ValidationSettings>
                                            <Border BorderColor="#e0e0e0" BorderStyle="Solid" BorderWidth="1px" />
                                        </dx:ASPxTextBox>
                                    </div>

                                    <!-- Bank -->
                                    <div class="form-group">
                                        <dx:ASPxTextBox ID="Bank" ClientInstanceName="Bank" 
                                            runat="server" Width="100%" Text="/" 
                                            ClientVisible="false" Height="40px">
                                            <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                                <RequiredField IsRequired="True" ErrorText="اسم البنك مطلوب" />
                                            </ValidationSettings>
                                            <Border BorderColor="#e0e0e0" BorderStyle="Solid" BorderWidth="1px" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>

                                <!-- Notes -->
                                <div class="form-group">
                                    <label class="form-label">
                                        <i class="fas fa-sticky-note"></i> ملاحظات
                                    </label>
                                    <dx:ASPxTextBox ID="Note" runat="server" Width="100%" 
                                        Text="/" Height="60px">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="الملاحظات مطلوبة" />
                                        </ValidationSettings>
                                        <Border BorderColor="#e0e0e0" BorderStyle="Solid" BorderWidth="1px" />
                                    </dx:ASPxTextBox>
                                </div>

                                <!-- Action Buttons -->
                                <div style="display: flex; gap: 15px; margin-top: 30px;">
                                    <dx:ASPxButton ID="sdad" runat="server" AutoPostBack="False" 
                                        UseSubmitBehavior="False" ClientInstanceName="sdad" 
                                        Text="<i class='fas fa-check'></i> تأكيد السداد"
                                        CssClass="modern-btn modern-btn-success" Width="200px" Height="45px">
                                        <ClientSideEvents Click="Sadad" />
                                    </dx:ASPxButton>
                                    
                                    <dx:ASPxButton ID="btnExit" runat="server" Text="<i class='fas fa-times'></i> خروج"
                                        CssClass="modern-btn modern-btn-danger" Width="150px" Height="45px">
                                        <ClientSideEvents Click="function(s, e) { ReturnToParentPage(); }" />
                                    </dx:ASPxButton>
                                </div>
                            </div>
                        </div>

                        <!-- Policies Grid -->
                        <div class="form-section" style="margin: 20px;">
                            <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
                                <label class="form-label">
                                    <i class="fas fa-file-contract"></i> مقابل سداد
                                </label>
                                <dx:ASPxCheckBox ID="CheckCust" runat="server" 
                                    Text="عرض جميع الوثائق" ClientVisible="false"
                                    OnCheckedChanged="CheckCust_CheckedChanged" 
                                    AutoPostBack="True">
                                </dx:ASPxCheckBox>
                            </div>
                            
                            <dx:ASPxGridView ID="GridData" runat="server" Width="100%"
                                CssClass="modern-grid">
                                <SettingsPopup>
                                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                                </SettingsPopup>
                                <SettingsPager PageSize="10" />
                                <Styles>
                                    <Header BackColor="#f8f9fa" ForeColor="#333" Font-Bold="true">
                                    </Header>
                                    <Cell HorizontalAlign="Center" />
                                    <AlternatingRow Enabled="True" BackColor="#fafafa" />
                                </Styles>
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="PolNo" Caption="رقم الوثيقة" Width="150">
                                        <CellStyle Font-Bold="true" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="EndNo" Caption="رقم الملحق" Width="100" />
                                    <dx:GridViewDataTextColumn FieldName="LoadNo" Caption="رقم الإشعار" Width="100" />
                                    <dx:GridViewDataTextColumn FieldName="TOTPRM" Caption="اجمالي القسط" Width="120">
                                        <PropertiesTextEdit DisplayFormatString="n3" />
                                        <CellStyle HorizontalAlign="Left" Font-Bold="true" ForeColor="#2e7d32" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="TpName" Caption="العملة" Width="100" />
                                    <dx:GridViewDataTextColumn FieldName="Commissioned" Caption="العمولة - المسوق" Width="150" />
                                </Columns>
                            </dx:ASPxGridView>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>

        <!-- Confirmation Popup (Original but styled) -->
        <dx:ASPxPopupControl ID="pcConfirmIssue" runat="server" 
            ClientInstanceName="pcConfirmIssue" HeaderText="تأكيد عملية السداد"
            Modal="True" PopupHorizontalAlign="WindowCenter" 
            PopupVerticalAlign="WindowCenter" ShowCloseButton="False"
            Width="400" Height="250">
            <HeaderStyle BackColor="#4361ee" ForeColor="White" Font-Bold="true" />
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <div style="padding: 30px; text-align: center;">
                        <div style="font-size: 48px; color: #f8961e; margin-bottom: 20px;">
                            <i class="fas fa-question-circle"></i>
                        </div>
                        <div style="font-size: 18px; font-weight: 600; color: #333; margin-bottom: 20px;">
                            هل أنت متأكد من تنفيذ عملية السداد؟
                        </div>
                        <div style="color: #666; margin-bottom: 30px; line-height: 1.6;">
                            سيتم إصدار قيد محاسبي وإرسال الإشعار للجهات المعنية
                        </div>
                        <div style="display: flex; gap: 15px; justify-content: center;">
                            <dx:ASPxButton ID="yesIssButton" runat="server" AutoPostBack="False" 
                                Text="<i class='fas fa-check'></i> نعم، تأكيد"
                                CssClass="modern-btn modern-btn-success">
                                <ClientSideEvents Click="YesIss_Click" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="noIssButton" runat="server" AutoPostBack="False" 
                                Text="<i class='fas fa-times'></i> لا، إلغاء"
                                CssClass="modern-btn modern-btn-danger">
                                <ClientSideEvents Click="NoIss_Click" />
                            </dx:ASPxButton>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <!-- Data Sources -->
        <asp:SqlDataSource ID="Accounts" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,5)='1.1.3')">
        </asp:SqlDataSource>
        
        <asp:SqlDataSource ID="BankAccounts" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,7)='1.1.1.2')">
        </asp:SqlDataSource>
        
        <asp:SqlDataSource ID="AccountsNotPayed" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=5 and (left(ltrim(AccountNo),8)='1.1.10.1') and AccountNo in (Select AccountNum from DailyJournal where SubIns=@Sys)">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="Sys" DefaultValue="0" Name="Sys" />
            </SelectParameters>
        </asp:SqlDataSource>
        
        <asp:SqlDataSource ID="Pay" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>">
        </asp:SqlDataSource>
    </form>

    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</body>
</html>