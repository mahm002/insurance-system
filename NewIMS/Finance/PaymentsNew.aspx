<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="PaymentsNew.aspx.vb" Inherits=".PaymentsNew" %>

<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>نظام توزيع المدفوعات</title>
    
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    
    <!-- Fix DevExpress Theme Conflicts -->
    <style>
        /* Reset conflicting DevExpress styles */
        .dxpc-content, .dxpc-content * {
            box-sizing: border-box !important;
        }
        
        /* Ensure our styles override DevExpress */
        body.dxBody {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%) !important;
            font-family: 'Segoe UI', 'Noto Sans Arabic', Tahoma, Geneva, Verdana, sans-serif !important;
        }
        
        /* Main Container */
        .main-container {
            max-width: 1600px;
            margin: 20px auto;
            background: white;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
            overflow: hidden;
        }
        
        /* Header */
        .main-header {
            background: linear-gradient(135deg, #2c3e50 0%, #1a2530 100%);
            color: white;
            padding: 20px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
        }
        
        .main-header h1 {
            font-size: 1.8rem;
            font-weight: 600;
            margin: 0;
            color: white;
        }
        
        .header-badge {
            background: rgba(255, 255, 255, 0.2);
            padding: 8px 16px;
            border-radius: 20px;
            font-size: 0.9rem;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            margin-top: 10px;
        }
        
        .balance-display {
            background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
            color: white;
            padding: 15px 20px;
            border-radius: 10px;
            text-align: center;
            min-width: 250px;
        }
        
        .balance-amount {
            font-size: 1.8rem;
            font-weight: 700;
            margin: 5px 0;
        }
        
        .balance-label {
            font-size: 0.9rem;
            opacity: 0.9;
        }
        
        /* Main Layout */
        .main-content {
            display: grid;
            grid-template-columns: 350px 1fr;
            gap: 20px;
            padding: 20px;
        }
        
        @media (max-width: 1200px) {
            .main-content {
                grid-template-columns: 1fr;
            }
        }
        
        /* Control Panel */
        .control-panel {
            background: #f8f9fa;
            border-radius: 10px;
            padding: 20px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        
        .control-group {
            margin-bottom: 20px;
        }
        
        .control-label {
            display: block;
            font-weight: 600;
            margin-bottom: 8px;
            color: #2c3e50;
            font-size: 0.95rem;
        }
        
        /* Form Controls */
        .form-control-wrapper {
            position: relative;
            margin-bottom: 15px;
        }
        
        .form-control-wrapper i {
            position: absolute;
            left: 12px;
            top: 50%;
            transform: translateY(-50%);
            color: #3498db;
            z-index: 1;
        }
        
        /* Make DevExpress controls look better */
        .dxeEditArea.dxeEditAreaSys,
        .dxeEditArea.dxeEditAreaSys input,
        .dxeEditArea.dxeEditAreaSys textarea {
            font-family: 'Segoe UI', 'Noto Sans Arabic', sans-serif !important;
            font-size: 14px !important;
            padding-right: 40px !important;
            padding-left: 10px !important;
        }
        
        .dxeButtonEditButton,
        .dxeCalendarButton {
            background: #3498db !important;
            border-color: #3498db !important;
        }
        
        .dxeButtonEditButton:hover,
        .dxeCalendarButton:hover {
            background: #2980b9 !important;
        }
        
        /* Status Badges */
        .status-badge {
            display: inline-block;
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 0.85rem;
            font-weight: 600;
        }
        
        .status-paid { background: #d4edda; color: #155724; }
        .status-partial { background: #fff3cd; color: #856404; }
        .status-pending { background: #f8d7da; color: #721c24; }
        .status-unpaid { background: #d1ecf1; color: #0c5460; }
        
        /* Action Buttons */
        .action-buttons {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
            gap: 10px;
            margin-top: 20px;
        }
        
        .action-btn {
            padding: 12px;
            border: none;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            font-size: 14px;
        }
        
        .btn-primary { background: linear-gradient(135deg, #3498db, #2980b9); color: white; }
        .btn-success { background: linear-gradient(135deg, #27ae60, #219653); color: white; }
        .btn-danger { background: linear-gradient(135deg, #e74c3c, #c0392b); color: white; }
        
        .action-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        }
        
        .action-btn:disabled {
            opacity: 0.5;
            cursor: not-allowed;
            transform: none !important;
            box-shadow: none !important;
        }
        
        /* Grid Container */
        .grid-container {
            background: white;
            border-radius: 10px;
            padding: 15px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        
        .grid-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 15px;
            padding-bottom: 15px;
            border-bottom: 2px solid #dee2e6;
        }
        
        .grid-stats {
            display: flex;
            gap: 20px;
        }
        
        .stat-item {
            text-align: center;
        }
        
        .stat-value {
            font-size: 1.5rem;
            font-weight: 700;
            color: #2c3e50;
        }
        
        .stat-label {
            font-size: 0.85rem;
            color: #6c757d;
        }
        
        /* Payment Summary */
        .payment-summary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 20px;
            border-radius: 10px;
            margin-top: 20px;
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 15px;
        }
        
        .summary-item {
            text-align: center;
        }
        
        .summary-value {
            font-size: 1.5rem;
            font-weight: 700;
            margin: 5px 0;
        }
        
        .summary-label {
            font-size: 0.9rem;
            opacity: 0.9;
        }
        
        /* Messages */
        .message-container {
            margin-top: 15px;
            padding: 0 20px 20px;
        }
        
        .alert {
            padding: 15px;
            border-radius: 8px;
            margin: 10px 0;
            display: flex;
            align-items: center;
            gap: 10px;
            animation: fadeIn 0.3s ease-out;
        }
        
        .alert-success { background: #d4edda; color: #155724; border: 1px solid #c3e6cb; }
        .alert-error { background: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }
        .alert-info { background: #d1ecf1; color: #0c5460; border: 1px solid #bee5eb; }
        
        /* Grid Styling Overrides */
        .dxgvHeader.dxgvHeader_DevEx,
        .dxgvHeader.dxgvHeader_DevEx td {
            background: #2c3e50 !important;
            color: white !important;
            font-weight: bold !important;
            border-color: #2c3e50 !important;
        }
        
        .dxgvDataRow.dxgvDataRow_DevEx:hover td {
            background-color: #f8f9fa !important;
        }
        
        .dxgvSelectedRow.dxgvSelectedRow_DevEx td {
            background-color: #e3f2fd !important;
            border-color: #bbdefb !important;
        }
        
        /* Footer summary */
        .dxgvFooter.dxgvFooter_DevEx {
            background: #f8f9fa !important;
            font-weight: bold !important;
        }
        
        /* Animations */
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(-10px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        /* Loading overlay */
        .loading-overlay {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.5);
            display: flex;
            justify-content: center;
            align-items: center;
            z-index: 9999;
            display: none;
        }
        
        .loading-spinner {
            width: 50px;
            height: 50px;
            border: 5px solid #f3f3f3;
            border-top: 5px solid #3498db;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
        
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        
        /* Responsive */
        @media (max-width: 768px) {
            .main-content {
                padding: 10px;
                gap: 15px;
            }
            
            .control-panel {
                position: static;
            }
            
            .main-header {
                flex-direction: column;
                text-align: center;
                gap: 15px;
            }
            
            .balance-display {
                width: 100%;
            }
            
            .action-buttons {
                grid-template-columns: 1fr;
            }
            
            .payment-summary {
                grid-template-columns: 1fr;
            }
        }
    </style>
    
    <!-- JavaScript -->
    <script type="text/javascript">
        // Debug function
        function debug(msg) {
            console.log('[DEBUG] ' + msg);
        }

        // Grid selection changed handler
        function grid_SelectionChanged(s, e) {
            debug('Grid selection changed');

            // Update grid stats
            updateGridStats();

            // If payment type is 4 (debt transfer), calculate total
            if (typeof PayTyp !== 'undefined' && PayTyp.GetValue() == 4) {
                calculateSelectedTotal();
            }
        }

        // Update grid statistics
        function updateGridStats() {
            try {
                if (typeof ASPxGridView1 !== 'undefined') {
                    var totalRows = ASPxGridView1.GetVisibleRowsOnPage() || 0;
                    var selectedRows = ASPxGridView1.GetSelectedRowCount() || 0;

                    var totalEl = document.getElementById('totalInvoices');
                    var selectedEl = document.getElementById('selectedInvoices');

                    if (totalEl) totalEl.textContent = totalRows;
                    if (selectedEl) selectedEl.textContent = selectedRows;
                }
            } catch (e) {
                console.error('Error updating grid stats:', e);
            }
        }

        // Calculate total of selected invoices for debt transfer
        function calculateSelectedTotal() {
            try {
                if (typeof ASPxGridView1 !== 'undefined' && PayTyp.GetValue() == 4) {
                    var selectedItems = ASPxGridView1.GetSelectedFieldValues('TOTPRM');
                    var total = 0;

                    if (selectedItems && selectedItems.length > 0) {
                        for (var i = 0; i < selectedItems.length; i++) {
                            total += parseFloat(selectedItems[i]) || 0;
                        }
                    }

                    if (txtPaymentAmount) {
                        txtPaymentAmount.SetValue(total.toFixed(3));
                        updateBalanceDisplay();
                    }
                }
            } catch (e) {
                console.error('Error calculating selected total:', e);
            }
        }

        // Update balance display
        function updateBalanceDisplay() {
            try {
                var balance = Balance ? Balance.GetValue() || 0 : 0;
                var paymentAmount = txtPaymentAmount ? txtPaymentAmount.GetValue() || 0 : 0;
                var remaining = balance - paymentAmount;

                var balanceEl = document.getElementById('currentBalance');
                var remainingEl = document.getElementById('remainingBalance');
                var paymentEl = document.getElementById('paymentAmountDisplay');

                if (balanceEl) balanceEl.textContent = formatCurrency(balance);
                if (remainingEl) remainingEl.textContent = formatCurrency(remaining);
                if (paymentEl) paymentEl.textContent = formatCurrency(paymentAmount);
            } catch (e) {
                console.error('Error updating balance display:', e);
            }
        }

        // Format currency
        function formatCurrency(value) {
            if (!value && value !== 0) return '0.000';

            var num = parseFloat(value);
            if (isNaN(num)) return '0.000';

            return num.toFixed(3).replace(/\d(?=(\d{3})+\.)/g, '$&,') + ' LYD';
        }

        // Check selections and enable/disable buttons
        function checkSelections() {
            try {
                var customer = cmbCustomer ? cmbCustomer.GetValue() : null;
                var productType = cmbProductType ? cmbProductType.GetValue() : null;

                if (btnLoadInvoices) {
                    btnLoadInvoices.SetEnabled(customer != null && productType != null);
                }
            } catch (e) {
                console.error('Error checking selections:', e);
            }
        }

        // Load invoices button click handler
        function handleLoadInvoices() {
            debug('Load invoices clicked');

            // Validate
            var customer = cmbCustomer ? cmbCustomer.GetValue() : null;
            var productType = cmbProductType ? cmbProductType.GetValue() : null;

            if (!customer) {
                alert('يرجى اختيار الحساب المدين');
                return;
            }

            if (!productType) {
                alert('يرجى اختيار نوع التأمين');
                return;
            }

            // Show loading
            if (typeof LoadingPanel !== 'undefined') {
                LoadingPanel.Show();
            }

            // Perform callback
            if (typeof cbp !== 'undefined') {
                cbp.PerformCallback('LOAD_INVOICES');
            }
        }

        // Calculate payments button click handler
        function handleCalculate() {
            debug('Calculate clicked');

            // Validate
            var paymentAmount = txtPaymentAmount ? txtPaymentAmount.GetValue() : 0;
            if (!paymentAmount || paymentAmount <= 0) {
                alert('يرجى إدخال قيمة المبلغ المسدد');
                return;
            }

            // Show loading
            if (typeof LoadingPanel !== 'undefined') {
                LoadingPanel.Show();
            }

            // Perform callback
            if (typeof cbp !== 'undefined') {
                cbp.PerformCallback('CALCULATE:' + paymentAmount);
            }
        }

        // Save payments button click handler
        function handleSave() {
            debug('Save clicked');

            // Validate
            var paymentAmount = txtPaymentAmount ? txtPaymentAmount.GetValue() : 0;
            if (!paymentAmount || paymentAmount <= 0) {
                alert('يرجى إدخال قيمة المبلغ المسدد');
                return;
            }

            // Confirm
            if (!confirm('هل أنت متأكد من حفظ توزيع المدفوعات؟')) {
                return;
            }

            // Show loading
            if (typeof LoadingPanel !== 'undefined') {
                LoadingPanel.Show();
            }

            // Perform callback
            if (typeof cbp !== 'undefined') {
                cbp.PerformCallback('SAVE_PAYMENTS:' + paymentAmount);
            }
        }

        // On account changed
        function OnAccountChanged(s, e) {
            debug('Account changed: ' + s.GetValue());

            // Update balance
            if (typeof cbp !== 'undefined') {
                cbp.PerformCallback('UPDATE_BALANCE');
            }

            // Check selections
            checkSelections();
        }

        // On system changed
        function OnSystemChanged(s, e) {
            debug('System changed: ' + s.GetValue());
            checkSelections();
        }

        // Hide or show based on payment type
        function HideOrShow(s, e) {
            debug('Payment type changed: ' + s.GetValue());

            if (typeof cbp !== 'undefined') {
                cbp.PerformCallback('PaymentChanged');
            }
        }

        // Callback end handler
        function OnEndCallback(s, e) {
            debug('Callback completed');

            // Hide loading
            if (typeof LoadingPanel !== 'undefined') {
                LoadingPanel.Hide();
            }

            // Handle messages
            if (s.cpErrorMessage) {
                alert('خطأ: ' + s.cpErrorMessage);
                s.cpErrorMessage = null;
            }

            if (s.cpSuccessMessage) {
                alert('نجاح: ' + s.cpSuccessMessage);
                s.cpSuccessMessage = null;
            }

            if (s.cpMessage && typeof lblMessage !== 'undefined') {
                lblMessage.SetText(s.cpMessage);
                s.cpMessage = null;
            }

            // Refresh grid if needed
            if (s.cpRefreshGrid) {
                if (typeof ASPxGridView1 !== 'undefined') {
                    ASPxGridView1.PerformCallback('refresh');
                }

                // Update stats and balance
                updateGridStats();
                updateBalanceDisplay();

                s.cpRefreshGrid = null;
            }

            // Update balance display
            if (s.cpUpdateBalance) {
                updateBalanceDisplay();
                s.cpUpdateBalance = null;
            }
        }

        // Initialize page
        function initPage() {
            debug('Page initialized');

            // Initial updates
            updateBalanceDisplay();
            updateGridStats();
            checkSelections();

            debug('Page initialization complete');
        }

        // Initialize when DevExpress controls are ready
        if (typeof ASPxClientControl !== 'undefined') {
            ASPxClientControl.ControlCollection.prototype.ControlsInitialized.addHandler(function () {
                debug('DevExpress controls initialized');
                setTimeout(initPage, 300);
            });
        }

        // Also initialize on page load
        document.addEventListener('DOMContentLoaded', function () {
            debug('DOM loaded');

            // If DevExpress not available, try direct initialization
            setTimeout(function () {
                if (typeof ASPxClientControl === 'undefined') {
                    debug('DevExpress not found, initializing directly');
                    initPage();
                }
            }, 2000);
        });
    </script>
</head>
<body class="dxBody">
    <form id="form1" runat="server">
        <!-- Loading Overlay -->
        <div id="loadingOverlay" class="loading-overlay">
            <div class="loading-spinner"></div>
        </div>
        
        <!-- Data Sources (MUST be outside callback panel) -->
        <asp:SqlDataSource ID="Systems" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
            SelectCommand="SELECT SUBSYSNO, RTRIM(SUBSYSNAME) as SUBSYSNAME, MAINSYS FROM SUBSYSTEMS WHERE SysType=1 AND Branch=dbo.MainCenter() ORDER BY MAINSYS" />
        
        <asp:SqlDataSource ID="Accounts" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
            SelectCommand="SELECT RTRIM(AccountNo) As AccountNo, REPLACE([AccountNo],'.','')+' - '+RTRIM(AccountName) As AccountName FROM Accounts WHERE AccountNo NOT IN (SELECT ISNULL(ParentAcc,'') FROM Accounts) AND Level>=5 AND (LEFT(AccountNo,7)='1.1.3.1' OR LEFT(AccountNo,7)='1.1.3.2' OR LEFT(AccountNo,8)='1.1.10.1')" />
        
        <asp:SqlDataSource ID="Pay" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
            SelectCommand="SELECT TPName, TPNo FROM EXTRAINFO WHERE TP='Payment' AND TPNo<>6 ORDER BY TPNo" />
        
        <asp:SqlDataSource ID="BankAccounts" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
            SelectCommand="SELECT RTRIM(AccountNo) As AccountNo, REPLACE([AccountNo],'.','')+' - '+RTRIM(AccountName) As AccountName FROM Accounts WHERE AccountNo NOT IN (SELECT ISNULL(ParentAcc,'') FROM Accounts) AND (LEFT(AccountNo,7)='1.1.1.2')" />
        
        <asp:SqlDataSource ID="AccountPayable" runat="server" 
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
            SelectCommand="SELECT RTRIM(AccountNo) As AccountNo, REPLACE([AccountNo],'.','')+' - '+RTRIM(AccountName) As AccountName FROM Accounts WHERE AccountNo NOT IN (SELECT ISNULL(ParentAcc,'') FROM Accounts) AND Level>=5 AND (LEFT(AccountNo,5)='1.1.3')" />
        
        <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" SettingsLoadingPanel-ShowImage="false" SettingsLoadingPanel-Enabled="false" 
            OnCallback="Callback_Callback" Width="100%">
            <ClientSideEvents EndCallback="OnEndCallback" />
            <PanelCollection>
                <dx:PanelContent>
                    <div class="main-container">
                        <!-- Header -->
                        <div class="main-header">
                            <div>
                                <h1><i class="fas fa-money-bill-wave"></i> نظام توزيع المدفوعات على الوثائق</h1>
                                <div class="header-badge">
                                    <i class="fas fa-building"></i>
                                    <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="balance-display">
                                <div class="balance-label">الرصيد الحالي</div>
                                <div class="balance-amount" id="currentBalance">0.000 LYD</div>
                                <div class="balance-label">المبلغ المسدد: <span id="paymentAmountDisplay">0.000 LYD</span></div>
                            </div>
                        </div>
                        
                        <!-- Main Content -->
                        <div class="main-content">
                            <!-- Left Panel - Controls -->
                            <div class="control-panel">
                                <!-- Customer Account -->
                                <div class="control-group">
                                    <label class="control-label">
                                        <i class="fas fa-user-tie"></i> الحساب المدين / تحت التحصيل
                                    </label>
                                    <div class="form-control-wrapper">
                                        <i class="fas fa-user"></i>
                                        <dx:ASPxComboBox ID="cmbCustomer" runat="server" ClientInstanceName="cmbCustomer"
                                            DataSourceID="Accounts" ValueField="AccountNo" TextField="AccountName"
                                            NullText="اختر الحساب..." Width="100%" DropDownStyle="DropDown"
                                            EnableSynchronization="False" EnableCallbackMode="True">
                                              <ClientSideEvents ValueChanged="function(s, e) { OnAccountChanged(s, e); }" />
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                
                                <!-- Insurance Type -->
                                <div class="control-group">
                                    <label class="control-label">
                                        <i class="fas fa-shield-alt"></i> نوع التأمين
                                    </label>
                                    <div class="form-control-wrapper">
                                        <i class="fas fa-shield"></i>
                                        <dx:ASPxComboBox ID="cmbProductType" runat="server" ClientInstanceName="cmbProductType"
                                            DataSourceID="Systems" ValueField="SUBSYSNO" TextField="SUBSYSNAME"
                                            NullText="اختر نوع التأمين..." Width="100%" DropDownStyle="DropDown"
                                            EnableSynchronization="False" EnableCallbackMode="True">
                                            <ClientSideEvents ValueChanged="function(s, e) { OnSystemChanged(s, e); }" />
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                
                                <!-- Payment Amount -->
                                <div class="control-group">
                                    <label class="control-label">
                                        <i class="fas fa-money-bill"></i> المبلغ المسدد
                                    </label>
                                    <div class="form-control-wrapper">
                                        <i class="fas fa-money-bill-wave"></i>
                                        <dx:ASPxTextBox ID="txtPaymentAmount" runat="server" ClientInstanceName="txtPaymentAmount"
                                            DisplayFormatString="N3" Width="100%" NullText="أدخل المبلغ..." Text="0">
                                            <ValidationSettings>
                                                <RequiredField IsRequired="true" ErrorText="مطلوب" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                    <div class="balance-display" style="margin-top: 10px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);">
                                        <div class="balance-label">المتبقي بعد السداد</div>
                                        <div class="balance-amount" id="remainingBalance">0.000 LYD</div>
                                    </div>
                                </div>
                                
                                <!-- Payment Method -->
                                <div class="control-group">
                                    <label class="control-label">
                                        <i class="fas fa-credit-card"></i> طريقة الدفع
                                    </label>
                                    <div class="form-control-wrapper">
                                        <i class="fas fa-credit-card"></i>
                                        <dx:ASPxComboBox ID="PayTyp" runat="server" ClientInstanceName="PayTyp"
                                            DataSourceID="Pay" ValueField="TPNo" TextField="TPName" SelectedIndex="0"
                                            Width="100%" DropDownStyle="DropDown" EnableSynchronization="False">
                                            <ClientSideEvents ValueChanged="function(s, e) { HideOrShow(s, e); }" />
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                
                                <!-- Received From -->
                                <div class="control-group">
                                    <label class="control-label">
                                        <i class="fas fa-user-check"></i> استلمت من
                                    </label>
                                    <dx:ASPxTextBox ID="Customer" runat="server" Width="100%" NullText="اسم العميل...">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="true" ErrorText="مطلوب" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </div>
                                
                                <!-- Check/Transfer Details -->
                                <div class="control-group">
                                    <label class="control-label">
                                        <i class="fas fa-receipt"></i> رقم الصك / الإشعار
                                    </label>
                                    <dx:ASPxTextBox ID="AccNo" runat="server" ClientInstanceName="AccNo" 
                                        ClientVisible="false" Width="100%">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="true" ErrorText="مطلوب" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </div>
                                
                                <!-- Bank Account -->
                                <div class="control-group">
                                    <label class="control-label">
                                        <i class="fas fa-university"></i> الحساب المصرفي
                                    </label>
                                    <dx:ASPxComboBox ID="AccName" runat="server" ClientInstanceName="AccName"
                                        DataSourceID="BankAccounts" ValueField="AccountNo" TextField="AccountName"
                                        Width="100%" NullText="اختر الحساب..." DropDownStyle="DropDown"
                                        EnableSynchronization="False" EnableCallbackMode="True">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="AccountNo" Caption="رقم الحساب" Width="120" />
                                            <dx:ListBoxColumn FieldName="AccountName" Caption="اسم الحساب" Width="250" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </div>
<!-- Action Buttons -->
<div class="action-buttons">
    <dx:ASPxButton ID="btnLoadInvoices" runat="server" 
        ClientInstanceName="btnLoadInvoices" 
        Text="تحميل الوثائق"
        CssClass="action-btn btn-primary">
        <ClientSideEvents Click="function(s, e) { handleLoadInvoices(); }" />
    </dx:ASPxButton>
    
    <dx:ASPxButton ID="btnCalculate" runat="server" 
        ClientInstanceName="btnCalculate" 
        Text="توزيع المبلغ"
        CssClass="action-btn btn-success">
        <ClientSideEvents Click="function(s, e) { handleCalculate(); }" />
    </dx:ASPxButton>
    
    <dx:ASPxButton ID="btnSave" runat="server" 
        ClientInstanceName="btnSave" 
        Text="حفظ التوزيع"
        CssClass="action-btn btn-danger">
        <ClientSideEvents Click="function(s, e) { handleSave(); }" />
    </dx:ASPxButton>
</div>
                            
                            <!-- Right Panel - Grid -->
                            <div class="grid-container">
                                <div class="grid-header">
                                    <h2 style="margin: 0; color: #2c3e50;">
                                        <i class="fas fa-file-invoice"></i> الوثائق المختارة
                                    </h2>
                                    <div class="grid-stats">
                                        <div class="stat-item">
                                            <div class="stat-value" id="totalInvoices">0</div>
                                            <div class="stat-label">إجمالي الوثائق</div>
                                        </div>
                                        <div class="stat-item">
                                            <div class="stat-value" id="selectedInvoices">0</div>
                                            <div class="stat-label">الوثائق المختارة</div>
                                        </div>
                                    </div>
                                </div>
                                
                                <!-- Grid -->
                                <dx:ASPxGridView ID="ASPxGridView1" runat="server" ClientInstanceName="ASPxGridView1"
                                    AutoGenerateColumns="False" KeyFieldName="PolNo;OrderNo" Width="100%"
                                    OnCustomCallback="ASPxGridView1_CustomCallback"
                                    OnDataBinding="ASPxGridView1_DataBinding"
                                    Settings-ShowFooter="true" SettingsBehavior-AllowSort="false"
                                    EnableCallBacks="true" EnableRowsCache="false">
                                    <ClientSideEvents SelectionChanged="function(s, e) { grid_SelectionChanged(s, e); }" />

                                    <Columns>
                                        <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="True" VisibleIndex="0" Width="50">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="PolNo" Caption="رقم الوثيقة" VisibleIndex="1" Width="100">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="OrderNo" Caption="رقم الطلب" Visible="false" VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn FieldName="IssuDate" Caption="تاريخ الإصدار" VisibleIndex="3" Width="100">
                                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd">
                                            </PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn FieldName="CustName" Caption="اسم المؤمن له" VisibleIndex="4" Width="200">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="TOTPRM" Caption="إجمالي القسط" VisibleIndex="5" Width="120">
                                            <PropertiesTextEdit DisplayFormatString="N3">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="InBox" Caption="المسدد منه" VisibleIndex="6" Width="120">
                                            <PropertiesTextEdit DisplayFormatString="N3">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Remain" Caption="المستحق" VisibleIndex="7" Width="120">
                                            <PropertiesTextEdit DisplayFormatString="N3">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Payment" Caption="المدفوع" VisibleIndex="8" Width="120">
                                            <PropertiesTextEdit DisplayFormatString="N3">
                                            </PropertiesTextEdit>
                                            <CellStyle BackColor="#E8F5E9" Font-Bold="true">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="NewRemaining" Caption="المتبقي" VisibleIndex="9" Width="120">
                                            <PropertiesTextEdit DisplayFormatString="N3">
                                            </PropertiesTextEdit>
                                            <CellStyle BackColor="#FFF3CD" Font-Bold="true">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Status" Caption="الحالة" VisibleIndex="10" Width="100">
                                            <DataItemTemplate>
                                                <%# GetStatusBadge(Eval("Status").ToString()) %>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>

                                    <TotalSummary>
                                        <dx:ASPxSummaryItem FieldName="TOTPRM" SummaryType="Sum" DisplayFormat="N3" />
                                        <dx:ASPxSummaryItem FieldName="InBox" SummaryType="Sum" DisplayFormat="N3" />
                                        <dx:ASPxSummaryItem FieldName="Remain" SummaryType="Sum" DisplayFormat="N3" />
                                        <dx:ASPxSummaryItem FieldName="Payment" SummaryType="Sum" DisplayFormat="N3" />
                                        <dx:ASPxSummaryItem FieldName="NewRemaining" SummaryType="Sum" DisplayFormat="N3" />
                                    </TotalSummary>

                                    <SettingsPager PageSize="15" Position="Bottom">
                                        <PageSizeItemSettings Visible="true" Items="10,15,20,50" />
                                    </SettingsPager>

                                    <Settings ShowGroupPanel="false" ShowFilterRow="false" />
                                    <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="false" />
                                    <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />

                                    <Styles>
                                        <Header Font-Bold="true" HorizontalAlign="Center" />
                                        <Cell HorizontalAlign="Right" />
                                        <Footer Font-Bold="true" />
                                        <SelectedRow BackColor="#E8F4F8" />
                                    </Styles>
                                </dx:ASPxGridView>
                                
                                <!-- Summary -->
                                <div class="payment-summary">
                                    <div class="summary-item">
                                        <div class="summary-label">إجمالي المستحق</div>
                                        <div class="summary-value" id="totalRemaining">0.000 LYD</div>
                                    </div>
                                    <div class="summary-item">
                                        <div class="summary-label">إجمالي المدفوع</div>
                                        <div class="summary-value" id="totalPayment">0.000 LYD</div>
                                    </div>
                                    <div class="summary-item">
                                        <div class="summary-label">إجمالي المتبقي</div>
                                        <div class="summary-value" id="totalNewRemaining">0.000 LYD</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Messages -->
                        <div class="message-container">
                            <dx:ASPxLabel ID="lblMessage" runat="server" ClientInstanceName="lblMessage" 
                                EnableViewState="False" />
                        </div>
                    </div>
                    
                    <!-- Hidden Controls -->
                    <dx:ASPxTextBox ID="Balance" runat="server" ClientInstanceName="Balance" 
                        DisplayFormatString="N3" ReadOnly="True" Text="0" ClientVisible="false" />
                    
                    <dx:ASPxDateEdit ID="MoveDate" runat="server" OnInit="MoveDate_Init" 
                        ClientVisible="false" />
                    
                    <dx:ASPxTextBox ID="Bank" runat="server" ClientInstanceName="Bank" 
                        ClientVisible="false" />
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
    </form>
<!-- Debug button (remove after testing) -->
<div style="position: fixed; bottom: 10px; right: 10px; z-index: 10000;">
    <button onclick="testCallback()" style="padding: 10px; background: red; color: white; border: none; border-radius: 5px;">
        TEST CALLBACK
    </button>
</div>

<script>
    // Debug test
    setTimeout(function () {
        console.log('=== DEBUG CHECK ===');
        console.log('grid_SelectionChanged defined:', typeof grid_SelectionChanged);
        console.log('handleLoadInvoices defined:', typeof handleLoadInvoices);
        console.log('handleCalculate defined:', typeof handleCalculate);
        console.log('handleSave defined:', typeof handleSave);
        console.log('OnAccountChanged defined:', typeof OnAccountChanged);
        console.log('OnSystemChanged defined:', typeof OnSystemChanged);
        console.log('HideOrShow defined:', typeof HideOrShow);
        console.log('OnEndCallback defined:', typeof OnEndCallback);

        // Test button clicks
        if (typeof btnLoadInvoices !== 'undefined') {
            console.log('btnLoadInvoices found');
        }

        if (typeof cmbCustomer !== 'undefined') {
            console.log('cmbCustomer value:', cmbCustomer.GetValue());
        }
    }, 3000);
</script>
</body>
</html>