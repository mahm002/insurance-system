<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MultiPaymentReceipt.aspx.vb"
    Inherits="MultiPaymentReceipt"
    EnableEventValidation="false"
    EnableSessionState="True"
    Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>سداد متعدد - نظام الدفع المتعدد</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

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

        .modern-container {
            max-width: 1400px;
            margin: 0 auto;
            background: white;
            border-radius: 12px;
            box-shadow: 0 8px 30px rgba(0,0,0,0.08);
            overflow: hidden;
        }

        .modern-header {
            background: linear-gradient(135deg, #4361ee, #3a0ca3);
            color: white;
            padding: 25px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .header-title {
            font-size: 24px;
            margin-bottom: 8px;
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .header-subtitle {
            font-size: 14px;
            opacity: 0.9;
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .content-wrapper {
            padding: 30px;
        }

        .amount-summary {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 20px;
            margin-bottom: 25px;
            background: #f8f9fa;
            padding: 25px;
            border-radius: 10px;
            border: 1px solid #e0e0e0;
        }

        .summary-item {
            text-align: center;
            padding: 15px;
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.05);
        }

        .summary-label {
            font-size: 14px;
            color: #666;
            margin-bottom: 8px;
            font-weight: 600;
        }

        .summary-value {
            font-size: 28px;
            font-weight: 700;
            color: #333;
        }

        .balance-remaining {
            color: #f72585 !important;
        }

        .payment-status {
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 20px;
            font-weight: 600;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .status-exact {
            background: rgba(76, 175, 80, 0.1);
            border: 1px solid #4CAF50;
            color: #2e7d32;
        }

        .status-under {
            background: rgba(255, 152, 0, 0.1);
            border: 1px solid #FF9800;
            color: #ef6c00;
        }

        .status-over {
            background: rgba(244, 67, 54, 0.1);
            border: 1px solid #f44336;
            color: #d32f2f;
        }

        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1.5fr;
            gap: 30px;
            margin-bottom: 30px;
        }

        .form-section {
            background: #f8f9fa;
            padding: 25px;
            border-radius: 10px;
            border: 1px solid #e0e0e0;
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
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .validation-message {
            color: #f72585;
            font-size: 12px;
            margin-top: 5px;
            display: none;
        }

            .validation-message.visible {
                display: block;
            }

        .payment-entries-container {
            margin-bottom: 25px;
        }

        .payment-entry-card {
            background: white;
            border: 2px solid #e0e0e0;
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 15px;
            transition: all 0.3s ease;
        }

            .payment-entry-card:hover {
                border-color: #4361ee;
                box-shadow: 0 4px 12px rgba(67, 97, 238, 0.1);
            }

        .entry-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 15px;
            padding-bottom: 15px;
            border-bottom: 1px solid #eee;
        }

        .entry-number {
            background: #4361ee;
            color: white;
            width: 30px;
            height: 30px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 14px;
            font-weight: 600;
        }

        .remove-entry {
            background: #f72585;
            color: white;
            border: none;
            width: 30px;
            height: 30px;
            border-radius: 50%;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: all 0.3s ease;
        }

            .remove-entry:hover {
                background: #d11d6d;
                transform: scale(1.1);
            }

        .entry-form {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 15px;
        }

            .entry-form .form-group {
                margin-bottom: 0;
            }

        .account-selection {
            grid-column: 1 / -1;
            margin-top: 15px;
            padding-top: 15px;
            border-top: 1px dashed #ddd;
            display: none;
        }

            .account-selection.visible {
                display: block;
            }

        .add-payment-btn {
            background: #4cc9f0;
            color: white;
            border: none;
            padding: 12px 24px;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
            font-size: 14px;
            display: flex;
            align-items: center;
            gap: 8px;
            transition: all 0.3s ease;
        }

            .add-payment-btn:hover {
                background: #3ab7dd;
                transform: translateY(-2px);
            }

        .payment-totals {
            background: white;
            padding: 20px;
            border-radius: 10px;
            border: 1px solid #e0e0e0;
            margin-top: 20px;
        }

        .totals-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 15px;
            margin-top: 15px;
        }

        .total-item {
            background: #f8f9fa;
            padding: 15px;
            border-radius: 8px;
            text-align: center;
        }

        .total-type {
            font-size: 12px;
            color: #666;
            margin-bottom: 5px;
        }

        .total-amount {
            font-size: 18px;
            font-weight: 700;
            color: #333;
        }

        .validation-error {
            border-color: #f72585 !important;
            background: rgba(247, 37, 133, 0.05) !important;
        }

        .action-buttons {
            display: flex;
            gap: 15px;
            justify-content: center;
            margin-top: 40px;
            padding-top: 30px;
            border-top: 1px solid #e0e0e0;
        }

        .modern-btn {
            padding: 14px 28px;
            border: none;
            border-radius: 8px;
            font-weight: 600;
            font-size: 16px;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 10px;
            transition: all 0.3s ease;
        }

        .btn-success {
            background: linear-gradient(135deg, #4cc9f0, #4361ee);
            color: white;
        }

            .btn-success:hover:not(:disabled) {
                background: linear-gradient(135deg, #3ab7dd, #3a0ca3);
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(76, 201, 240, 0.3);
            }

        .btn-danger {
            background: linear-gradient(135deg, #f72585, #b5179e);
            color: white;
        }

            .btn-danger:hover {
                background: linear-gradient(135deg, #d11d6d, #9c158a);
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(247, 37, 133, 0.3);
            }

        .button-disabled {
            opacity: 0.6;
            cursor: not-allowed !important;
        }

            .button-disabled:hover {
                transform: none !important;
                box-shadow: none !important;
            }

        .payment-type-1 {
            border-left: 4px solid #4CAF50;
        }

        .payment-type-2 {
            border-left: 4px solid #2196F3;
        }

        .payment-type-3 {
            border-left: 4px solid #FF9800;
        }

        .payment-type-4 {
            border-left: 4px solid #9C27B0;
        }

        .payment-type-5 {
            border-left: 4px solid #E91E63;
        }

        .payment-type-6 {
            border-left: 4px solid #607D8B;
        }

        input:focus, select:focus {
            outline: none;
            border-color: #4361ee !important;
            box-shadow: 0 0 0 2px rgba(67, 97, 238, 0.1) !important;
        }

        .loading-accounts {
            position: relative;
            color: transparent !important;
        }

            .loading-accounts:after {
                content: '';
                position: absolute;
                right: 10px;
                top: 50%;
                transform: translateY(-50%);
                width: 16px;
                height: 16px;
                border: 2px solid #f3f3f3;
                border-top: 2px solid #4361ee;
                border-radius: 50%;
                animation: spin 1s linear infinite;
            }

        @keyframes spin {
            0% {
                transform: translateY(-50%) rotate(0deg);
            }

            100% {
                transform: translateY(-50%) rotate(360deg);
            }
        }

        .loading-overlay {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(255, 255, 255, 0.8);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 9999;
            display: none;
        }

        .spinner {
            width: 50px;
            height: 50px;
            border: 5px solid #f3f3f3;
            border-top: 5px solid #4361ee;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        .error-message {
            background: rgba(244, 67, 54, 0.1);
            border: 1px solid #f44336;
            color: #d32f2f;
            padding: 15px;
            border-radius: 8px;
            margin: 15px 0;
            font-weight: 600;
        }

        .success-message {
            background: rgba(76, 175, 80, 0.1);
            border: 1px solid #4CAF50;
            color: #2e7d32;
            padding: 15px;
            border-radius: 8px;
            margin: 15px 0;
            font-weight: 600;
        }

        .modal-backdrop {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0,0,0,0.5);
            z-index: 9998;
        }

        .popup-container {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 9999;
            background: white;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.3);
            width: 500px;
            max-height: 90vh;
            overflow-y: auto;
        }

        /* === REQUIRED FOR OTHER RECEIPTS === */
        .required-star::after {
            content: " *";
            color: #f72585;
            font-weight: bold;
        }

        .field-error {
            border: 2px solid #f72585 !important;
            background-color: #fff5f5 !important;
        }

            .field-error:focus {
                border-color: #f72585 !important;
                box-shadow: 0 0 0 3px rgba(247, 37, 133, 0.1) !important;
            }

        .error-message-box {
            background: rgba(247, 37, 133, 0.1);
            border: 1px solid #f72585;
            color: #f72585;
            padding: 12px;
            border-radius: 8px;
            margin: 10px 0;
            font-size: 14px;
        }

            .error-message-box ul {
                margin: 8px 0 0 20px;
                padding: 0;
            }

            .error-message-box li {
                margin-bottom: 4px;
            }
    </style>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</head>
<body>
    <div class="loading-overlay" id="loadingOverlay">
        <div class="spinner"></div>
    </div>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
        </asp:ScriptManager>
        <!-- Hidden Fields -->
        <asp:HiddenField ID="hdnCurrentDate" runat="server" />
        <asp:HiddenField ID="hdnCurrentUser" runat="server" />
        <asp:HiddenField ID="hdnReceiptType" runat="server" />
        <asp:HiddenField ID="hdnPayments" runat="server" />
        <asp:HiddenField ID="hdnPaymentEntries" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTotalDue" runat="server" />
        <asp:HiddenField ID="hdnAccountsData" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnMessage" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnIsFinalSubmit" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnGridData" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnBranchCode" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnReceiptData" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPolicyData" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnIsOtherReceipt" runat="server" ClientIDMode="Static" Value="0" />
        <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" Value="0" />
        <div class="modern-container">
            <div class="modern-header">
                <div>
                    <h1 class="header-title">
                        <i class="fas fa-money-check-alt"></i>
                        <asp:Label ID="lblTitle" runat="server" Text="سداد متعدد - نظام الدفع المتعدد"></asp:Label>
                    </h1>
                    <p class="header-subtitle" id="headerSubtitle">
                        <span id="dateDisplay"></span>
                        <span id="userDisplay"></span>
                        <span id="typeDisplay"></span>
                    </p>
                </div>
                <div style="color: rgba(255,255,255,0.8);">
                    <i class="fas fa-building"></i>فرع:
                    <asp:Label ID="lblBranch" runat="server"></asp:Label>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <contenttemplate>
                    <div class="content-wrapper">
                        <!-- Error/Success Messages -->
                        <div id="messageContainer"></div>
                        <div class="amount-summary">
                            <div class="summary-item">
                                <div class="summary-label">المبلغ المستحق</div>
                                <div class="summary-value" id="totalDueDisplay">0.000</div>
                            </div>
                            <div class="summary-item">
                                <div class="summary-label">إجمالي المدفوع</div>
                                <div class="summary-value" id="totalPaid">0.000</div>
                            </div>
                            <div class="summary-item">
                                <div class="summary-label">المتبقي</div>
                                <div class="summary-value balance-remaining" id="balance">0.000</div>
                            </div>
                        </div>
                        <div id="paymentStatus"></div>
                        <div id="validationErrors" class="validation-container"></div>
                        <div class="form-grid">
                            <div class="form-section">
                                <div class="form-group">
                                    <label class="form-label"><i class="fas fa-user"></i>العميل</label>
                                    <dx:aspxtextbox id="Customer" runat="server" width="100%" height="40px" clientinstancename="Customer">
                                        <validationsettings validationgroup="ValidGroup">
                                            <requiredfield isrequired="True" errortext="اسم العميل مطلوب" />
                                        </validationsettings>
                                        <border bordercolor="#e0e0e0" borderstyle="Solid" borderwidth="1px" />
                                    </dx:aspxtextbox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label"><i class="fas fa-calendar-day"></i>تاريخ الحركة</label>
                                    <dx:aspxdateedit id="MoveDate" runat="server" width="100%" height="40px" clientinstancename="MoveDate">
                                        <border bordercolor="#e0e0e0" borderstyle="Solid" borderwidth="1px" />
                                    </dx:aspxdateedit>
                                </div>
                                <div class="form-group">
                                    <label class="form-label"><i class="fas fa-sticky-note"></i>ملاحظات عامة</label>
                                    <dx:aspxtextbox id="Note" runat="server" width="100%" text="/" height="80px" clientinstancename="Note">
                                        <validationsettings validationgroup="ValidGroup">
                                            <requiredfield isrequired="True" errortext="الملاحظات مطلوبة" />
                                        </validationsettings>
                                        <border bordercolor="#e0e0e0" borderstyle="Solid" borderwidth="1px" />
                                    </dx:aspxtextbox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label"><i class="fas fa-file-invoice-dollar"></i>إجمالي المبلغ المستحق</label>
                                    <dx:aspxtextbox id="TOTPRM" runat="server" width="100%" clientenabled="false"
                                        font-size="24px" horizontalalign="Center" clientinstancename="TOTPRM">
                                        <border borderstyle="None" />
                                    </dx:aspxtextbox>
                                </div>
                            </div>
                            <div class="form-section">
                                <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px;">
                                    <h3 style="margin: 0; color: #333;">
                                        <i class="fas fa-money-bill-wave"></i>طرق الدفع المتعددة
                                    </h3>
                                    <button type="button" class="add-payment-btn" id="addPaymentButton">
                                        <i class="fas fa-plus"></i>إضافة طريقة دفع
                                    </button>
                                </div>
                                <div class="payment-entries-container" id="paymentEntriesContainer">
                                    <!-- Payment entries will be added here by JavaScript -->
                                </div>
                                <div class="payment-totals">
                                    <h4 style="margin: 0 0 15px 0; color: #333; text-align: center;">
                                        <i class="fas fa-chart-pie"></i>تفصيل المدفوعات حسب النوع
                                    </h4>
                                    <div class="totals-grid" id="paymentBreakdown"></div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section" style="margin-top: 25px;">
                            <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
                                <label class="form-label"><i class="fas fa-file-contract"></i>الوثائق المراد سدادها</label>
                                <%--<dx:ASPxCheckBox ID="CheckCust" runat="server" Text="عرض جميع الوثائق"
                                OnCheckedChanged="CheckCust_CheckedChanged" AutoPostBack="True">
                            </dx:ASPxCheckBox>--%>
                            </div>
                            <asp:Panel ID="pnlGridContainer" runat="server">
                                <dx:aspxgridview id="GridData" runat="server" width="100%" clientinstancename="GridData" enablerowscache="false">
                                    <settingspopup>
                                        <filtercontrol autoupdateposition="False"></filtercontrol>
                                    </settingspopup>
                                    <settingspager pagesize="10" />
                                    <settingsloadingpanel mode="ShowAsPopup" />
                                    <styles>
                                        <header backcolor="#f8f9fa" forecolor="#333" font-bold="true" />
                                        <cell horizontalalign="Center" />
                                        <alternatingrow enabled="True" backcolor="#fafafa" />
                                    </styles>
                                    <columns>
                                        <dx:gridviewdatatextcolumn fieldname="PolNo" caption="رقم الوثيقة" width="150">
                                            <cellstyle font-bold="true" />
                                        </dx:gridviewdatatextcolumn>
                                        <dx:gridviewdatatextcolumn fieldname="EndNo" caption="رقم الملحق" width="100" />
                                        <dx:gridviewdatatextcolumn fieldname="LoadNo" caption="رقم الإشعار" width="100" />
                                        <dx:gridviewdatatextcolumn fieldname="TOTPRM" caption="اجمالي القسط" width="120">
                                            <propertiestextedit displayformatstring="n3" />
                                            <cellstyle horizontalalign="Left" font-bold="true" forecolor="#2e7d32" />
                                        </dx:gridviewdatatextcolumn>
                                        <dx:gridviewdatatextcolumn fieldname="TpName" caption="العملة" width="100" />
                                        <dx:gridviewdatatextcolumn fieldname="Commissioned" caption="العمولة - المسوق" width="150" />
                                    </columns>
                                </dx:aspxgridview>
                            </asp:Panel>
                            <div class="action-buttons">
                                <dx:aspxbutton id="sdad" runat="server" autopostback="True" usesubmitbehavior="False"
                                    clientinstancename="sdad" text="تأكيد السداد المتعدد"
                                    cssclass="modern-btn btn-success" width="250px" height="45px">
                                    <clientsideevents
                                        click="function(s, e) {
            // Prevent default and run our validation
            e.processOnServer = false;
            validateAndSubmit();
        }" />
                                </dx:aspxbutton>
                                <dx:aspxbutton id="btnExit" runat="server" text="خروج"
                                    cssclass="modern-btn btn-danger" width="150px" height="45px">
                                    <clientsideevents click="function(s, e) { ReturnToParentPage(); }" />
                                </dx:aspxbutton>
                            </div>
                        </div>
                    </div>
                </contenttemplate>
                <triggers>
                    <asp:AsyncPostBackTrigger ControlID="sdad" EventName="Click" />
                    <%--<asp:AsyncPostBackTrigger ControlID="CheckCust" EventName="CheckedChanged" />--%>
                </triggers>
            </asp:UpdatePanel>
        </div>

        <!-- Confirmation Popup -->
        <div id="confirmationPopup" class="modal-backdrop" style="display: none;">
            <div class="popup-container">
                <div style="background: linear-gradient(135deg, #4361ee, #3a0ca3); color: white; padding: 20px; border-radius: 12px 12px 0 0;">
                    <h3 style="margin: 0; display: flex; justify-content: space-between; align-items: center;">
                        <span>تأكيد السداد المتعدد</span>
                        <button type="button" style="background: none; border: none; color: white; font-size: 24px; cursor: pointer;" onclick="hideConfirmationPopup()">×</button>
                    </h3>
                </div>
                <div style="padding: 20px;">
                    <div style="text-align: center; margin-bottom: 20px;">
                        <i class="fas fa-clipboard-check" style="font-size: 48px; color: #4cc9f0;"></i>
                    </div>
                    <div id="confirmationContent"></div>
                    <div style="display: flex; gap: 15px; justify-content: center; margin-top: 30px;">
                        <button class="modern-btn btn-success" id="yesIssButton" style="width: 200px;" onclick="confirmPayment()">
                            <i class="fas fa-check"></i>نعم، تأكيد السداد
                        </button>
                        <button class="modern-btn btn-danger" id="noIssButton" style="width: 150px;" onclick="hideConfirmationPopup()">
                            <i class="fas fa-times"></i>إلغاء
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <asp:SqlDataSource ID="Accounts" runat="server"
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,5)='1.1.3')"></asp:SqlDataSource>
        <asp:SqlDataSource ID="BankAccounts" runat="server"
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,7)='1.1.1.2')"></asp:SqlDataSource>
        <asp:SqlDataSource ID="AccountsNotPayed" runat="server"
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=5 and (left(ltrim(AccountNo),8)='1.1.10.1') ">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="Sys" DefaultValue="0" Name="Sys" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="Pay" runat="server"
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="SELECT TPName, TPNo FROM EXTRAINFO WHERE TP='Payment' ORDER BY TPNo"></asp:SqlDataSource>
    </form>
    <script type="text/javascript">
        var paymentTypes = [
            { value: '1', name: 'نقداً', icon: 'fas fa-money-bill-wave', color: '#4CAF50', requiresAccount: false },
            { value: '2', name: 'بصك', icon: 'fas fa-file-invoice', color: '#2196F3', requiresAccount: false },
            { value: '3', name: 'بإشعار', icon: 'fas fa-receipt', color: '#FF9800', requiresAccount: true, dataSource: 'BankAccounts' },
            { value: '4', name: 'على الحساب', icon: 'fas fa-user-clock', color: '#9C27B0', requiresAccount: true, dataSource: 'Accounts' },
            { value: '5', name: 'بطاقة مصرفية', icon: 'fas fa-credit-card', color: '#E91E63', requiresAccount: true, dataSource: 'BankAccounts' },
            { value: '6', name: 'تحت التحصيل', icon: 'fas fa-clock', color: '#607D8B', requiresAccount: true, dataSource: 'AccountsNotPayed' }
        ];
        var paymentEntries = [];
        var entryCounter = 0;
        var isSubmitting = false;
        var isOtherReceipt = false;

        // Global error handler
        $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
            console.error('AJAX Error:', thrownError);
            console.error('URL:', settings.url);
            console.error('Status:', jqxhr.status);
            console.error('Response:', jqxhr.responseText);

            // Only show generic error for specific AJAX calls
            if (settings.url && settings.url.indexOf('/Handlers/') === -1) {
                showMessage('حدث خطأ في الاتصال. يرجى المحاولة مرة أخرى.', 'error');
            }
        });

        window.onerror = function (msg, url, lineNo, columnNo, error) {
            console.error('Global Error:', msg, url, lineNo, error);
            showMessage('حدث خطأ غير متوقع. يرجى تحديث الصفحة والمحاولة مرة أخرى.', 'error');
            return false;
        };

        function showMessage(message, type) {
            var container = $('#messageContainer');
            var className = type === 'error' ? 'error-message' : 'success-message';
            var icon = type === 'error' ? 'fas fa-exclamation-triangle' : 'fas fa-check-circle';
            container.html('<div class="' + className + '"><i class="' + icon + '"></i> ' + message + '</div>');
            container.show();
            // Auto-hide success messages after 5 seconds
            if (type === 'success') {
                setTimeout(function () {
                    container.fadeOut();
                }, 5000);
            }
        }

        function showLoading(show) {
            document.getElementById('loadingOverlay').style.display = show ? 'flex' : 'none';
        }

        function showConfirmationPopup() {
            document.getElementById('confirmationPopup').style.display = 'block';
        }

        function hideConfirmationPopup() {
            document.getElementById('confirmationPopup').style.display = 'none';
        }

        $(document).ready(function () {
            console.log('Page loaded successfully');
            try {
                // Check if this is "Other" receipt
                isOtherReceipt = $('#<%= hdnIsOtherReceipt.ClientID %>').val() === '1';
                console.log('Is Other Receipt:', isOtherReceipt);

                // Initialize header display
                updateHeaderDisplay();

                // ===== Setup Other Receipt UI if needed =====
                if (isOtherReceipt) {
                    console.log('Setting up Other Receipt UI');

                    // Add required asterisks to labels
                    $('.form-label').each(function () {
                        var labelText = $(this).text();
                        if (labelText.includes('العميل') || labelText.includes('الحركة') || labelText.includes('ملاحظات')) {
                            $(this).addClass('required-star');
                        }
                    });

                    // Make sure fields are clear and ready for input
                    setTimeout(function () {
                        // Clear Customer if it has default value
                        var customerElement = ASPxClientControl.GetControlCollection().GetByName('Customer');
                        if (customerElement) {
                            var customerValue = customerElement.GetValue() || '';
                            if (!customerValue || customerValue === 'undefined') {
                                customerElement.SetText('');
                            }
                        }

                        // Clear Note if it has default "/"
                        var noteElement = ASPxClientControl.GetControlCollection().GetByName('Note');
                        if (noteElement) {
                            var noteValue = noteElement.GetValue() || '';
                            if (noteValue === '/' || noteValue === 'undefined') {
                                noteElement.SetText('');
                            }
                        }

                        // Enable MoveDate
                        var moveDateElement = ASPxClientControl.GetControlCollection().GetByName('MoveDate');
                        if (moveDateElement) {
                            moveDateElement.SetEnabled(true);
                            if (!moveDateElement.GetValue()) {
                                moveDateElement.SetValue(new Date());
                            }
                        }
                    }, 500);

                    // Setup Real-Time Validation
                    setupRealTimeValidation();
                }

                // Handle UpdatePanel postbacks
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                if (prm) {
                    prm.add_beginRequest(function (sender, args) {
                        console.log('Postback starting...');
                        if (!isSubmitting) {
                            // Save current state before postback
                            var paymentData = savePaymentEntries();
                            $('#<%= hdnPaymentEntries.ClientID %>').val(paymentData);
                        console.log('Payment entries saved before postback:', paymentEntries.length);
                    }
                });

                prm.add_endRequest(function (sender, args) {
                    console.log('Postback completed');

                    // Check for errors in the response
                    if (args.get_error() != null) {
                        console.error('Postback error:', args.get_error());
                        showMessage('حدث خطأ أثناء معالجة الطلب: ' + args.get_error().message, 'error');
                        args.set_errorHandled(true);
                        showLoading(false);
                        return;
                    }

                    // Restore state after postback
                    restorePaymentEntriesFromServer();
                    restoreGridData();
                    updateHeaderDisplay();

                    // Re-setup Other Receipt UI after postback
                    if (isOtherReceipt) {
                        // Re-add required asterisks
                        $('.form-label').each(function () {
                            var labelText = $(this).text();
                            if (labelText.includes('العميل') || labelText.includes('الحركة') || labelText.includes('ملاحظات')) {
                                $(this).addClass('required-star');
                            }
                        });

                        // Re-setup real-time validation
                        setupRealTimeValidation();
                    }

                    // Check for server messages
                    var message = $('#<%= hdnMessage.ClientID %>').val();
                    if (message) {
                        showMessage(message, 'success');
                        $('#<%= hdnMessage.ClientID %>').val('');
                    }

                    // Check if we need to show confirmation
                    if ($('#<%= hdnIsFinalSubmit.ClientID %>').val() === '1') {
                        // This is a final submit, don't restore payment entries as they'll be processed
                        console.log('Final submit detected');
                    } else {
                        // Normal postback, restore everything
                        restorePaymentEntries();
                        updateTotals();
                        validatePayments();
                        updatePaymentBreakdown();
                    }
                });
                }

                // Initial setup
                restorePaymentEntries();
                restoreGridData();

                // Load document data - Only for policy payments
                if (!isOtherReceipt) {
                    loadDocumentData();
                } else {
                    // For Other receipts, create first payment entry immediately
                    if (paymentEntries.length === 0) {
                        createFirstPaymentEntry();
                    }
                }

                // Setup event handlers
                setupEventHandlers();

                // Initial setup after page loads
                setTimeout(function () {
                    // Update totals
                    updateTotals();
                    updatePaymentBreakdown();

                    // For "Other" receipts, setup manual total input
                    if (isOtherReceipt) {
                        setupManualTotalForOtherReceipt();
                    }

                    // Clear any validation errors on initial load
                    $('#validationErrors').html('');

                    // Enable the submit button only if we have valid data
                    var totalDue = parseNumber($('#<%= hdnTotalDue.ClientID %>').val()) || 0;
                var submitButton = $('#<%= sdad.ClientID %>');

                if (submitButton.length) {
                    // For Other receipts, button is only enabled if manual total is set
                    if (isOtherReceipt) {
                        submitButton.prop('disabled', totalDue <= 0);
                    } else {
                        submitButton.prop('disabled', true);
                    }
                    submitButton.parent().find('.modern-btn').addClass('button-disabled');
                }
            }, 500);

                // Prevent back navigation
                preventBack();
            } catch (error) {
                console.error('Error in initialization:', error);
                showMessage('حدث خطأ في تهيئة الصفحة: ' + error.message, 'error');
            }
        });

        // Real-time validation setup
        function setupRealTimeValidation() {
            if (!isOtherReceipt) return;

            console.log('Setting up real-time validation for Other Receipt');

            // Customer field validation
            $('#<%= Customer.ClientID %>_I').off('blur').on('blur', function () {
                var value = $(this).val();
                if (!value || value.trim() === '') {
                    $(this).addClass('field-error').css('border-color', '#f72585');
                } else {
                    $(this).removeClass('field-error').css('border-color', '');
                }
            });

            // Note field validation
            $('#<%= Note.ClientID %>_I').off('blur').on('blur', function () {
                var value = $(this).val();
                if (!value || value.trim() === '' || value.trim() === '/') {
                    $(this).addClass('field-error').css('border-color', '#f72585');
                } else {
                    $(this).removeClass('field-error').css('border-color', '');
                }
            });

            // Manual total input validation
            $(document).on('blur', '#manualTotalInput', function () {
                var value = $(this).val();
                var num = parseNumber(value);
                if (isNaN(num) || num <= 0) {
                    $(this).addClass('field-error').css('border-color', '#f72585');
                } else {
                    $(this).removeClass('field-error').css('border-color', '');
                }
            });

            // MoveDate validation
            try {
                var moveDateElement = ASPxClientControl.GetControlCollection().GetByName('MoveDate');
                if (moveDateElement && moveDateElement.AddValueChanged) {
                    moveDateElement.AddValueChanged(function (s, e) {
                        var value = s.GetValue();
                        var inputElement = $('#<%= MoveDate.ClientID %>_I');
                    if (!value) {
                        inputElement.addClass('field-error').css('border-color', '#f72585');
                    } else {
                        inputElement.removeClass('field-error').css('border-color', '');
                    }
                });
                }
            } catch (e) {
                console.log('Could not attach MoveDate validation:', e);
            }
        }

        function updateHeaderDisplay() {
            var dateElement = $('#dateDisplay');
            var userElement = $('#userDisplay');
            var typeElement = $('#typeDisplay');

            if (dateElement.length) {
                var currentDate = $('#<%= hdnCurrentDate.ClientID %>').val();
                dateElement.html('<i class="fas fa-calendar"></i> ' + (currentDate || ''));
            }

            if (userElement.length) {
                var currentUser = $('#<%= hdnCurrentUser.ClientID %>').val();
                userElement.html('<i class="fas fa-user"></i> ' + (currentUser || ''));
            }

            if (typeElement.length) {
                var receiptType = $('#<%= hdnReceiptType.ClientID %>').val();
                typeElement.html('<i class="fas fa-receipt"></i> ' + (receiptType || ''));
            }
        }

        function setupEventHandlers() {
            $('#addPaymentButton').off('click').on('click', function (e) {
                e.preventDefault();
                addPaymentEntry();
                return false;
            });

            $(document)
                .on('change', '.payment-type-select', function () {
                    var entryId = $(this).closest('.payment-entry-card').attr('id');
                    updatePaymentType(this, entryId);
                })
                .on('blur', '.payment-amount', function () {
                    $(this).data('touched', true);
                    var entryId = $(this).closest('.payment-entry-card').attr('id');
                    formatEntryAmount(this, entryId);
                })
                .on('input', '.payment-amount', function () {
                    var entryId = $(this).closest('.payment-entry-card').attr('id');
                    updateEntryAmount(this, entryId);
                })
                .on('change', '.payment-details', function () {
                    var entryId = $(this).closest('.payment-entry-card').attr('id');
                    updateEntryDetails(this, entryId);
                })
                .on('change', '.account-select', function () {
                    $(this).data('touched', true);
                    var entryId = $(this).closest('.payment-entry-card').attr('id');
                    validateAccountSelection(this, entryId);
                })
                .on('focus', '.payment-amount, .account-select', function () {
                    $(this).removeClass('field-error').css('border-color', '');
                });
        }

        function loadDocumentData() {
            showLoading(true);

            var polNo = '<%= Request.QueryString("PolNo") %>';
            var endNo = '<%= Request.QueryString("EndNo") %>';
            var loadNo = '<%= Request.QueryString("LoadNo") %>';
            var branch = '<%= Session("Branch") %>';

            console.log('Loading document data:', { polNo: polNo, endNo: endNo, loadNo: loadNo, branch: branch });

            if (polNo === 'أخرى' || polNo === 'أخـرى') {
                // Handle "other payments" case
                isOtherReceipt = true;
                console.log('Processing as Other Receipt');

                // Update hidden field
                $('#<%= hdnIsOtherReceipt.ClientID %>').val('1');

            // Update receipt type display
            $('#<%= hdnReceiptType.ClientID %>').val('إيصال آخر');
                updateHeaderDisplay();

                showLoading(false);

                // Create first payment entry
                if (paymentEntries.length === 0) {
                    createFirstPaymentEntry();
                }

                // Setup Other Receipt UI
                setTimeout(function () {
                    setupOtherReceiptUI();
                    setupManualTotalForOtherReceipt();
                }, 300);

                return;
            }

            // For policy payments, load data normally
            /*url: "/Handlers/DocumentHandler.ashx",*/
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("~/Handlers/DocumentHandler.ashx") %>',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                action: "getDocument",
                polNo: polNo,
                endNo: endNo,
                loadNo: loadNo,
                branch: branch
            }),
            success: function (response) {
                console.log('Document data response:', response);
                if (response.success) {
                    // Update page with document data
                    var document = response.document;

                    // Set customer name
                    var customerElement = ASPxClientControl.GetControlCollection().GetByName('Customer');
                    if (customerElement) {
                        customerElement.SetValue(document.custName || '');
                    }

                    // Set total amount
                    var totprmElement = ASPxClientControl.GetControlCollection().GetByName('TOTPRM');
                    if (totprmElement) {
                        totprmElement.SetValue(formatNumber(document.totprm));
                    }

                    $('#<%= hdnTotalDue.ClientID %>').val(document.totprm);

                    // Update totals display
                    $('#totalDueDisplay').text(formatNumber(document.totprm));

                    // Update grid data
                    updateGridData(document);

                    // If no payment entries exist, create first one
                    if (paymentEntries.length === 0) {
                        createFirstPaymentEntry();
                    }
                } else {
                    showMessage(response.message || "فشل في تحميل بيانات الوثيقة", "error");
                }
            },
            error: function (xhr, status, error) {
                console.error('Error loading document data:', error, xhr.responseText);
                showMessage('حدث خطأ أثناء تحميل بيانات الوثيقة. الرجاء المحاولة مرة أخرى.', "error");
            },
            complete: function () {
                showLoading(false);
            }
        });
        }

        function setupOtherReceiptUI() {
            console.log('Setting up Other Receipt UI');

            // Add required asterisks to labels
            $('.form-label').each(function () {
                var labelText = $(this).text();
                if (labelText.includes('العميل') || labelText.includes('الحركة') || labelText.includes('ملاحظات')) {
                    $(this).addClass('required-star');
                }
            });

            // Clear fields
            setTimeout(function () {
                // Clear Customer
                var customerElement = ASPxClientControl.GetControlCollection().GetByName('Customer');
                if (customerElement) {
                    customerElement.SetValue('');
                }

                // Clear Note (don't set to "/")
                var noteElement = ASPxClientControl.GetControlCollection().GetByName('Note');
                if (noteElement) {
                    noteElement.SetValue('');
                }

                // Set MoveDate to today
                var moveDateElement = ASPxClientControl.GetControlCollection().GetByName('MoveDate');
                if (moveDateElement) {
                    moveDateElement.SetValue(new Date());
                    moveDateElement.SetEnabled(true);
                }
            }, 300);
        }

        function setupManualTotalForOtherReceipt() {
            // Remove existing if any
            $('#manualTotalContainer').remove();

            // Create manual total input for Other receipts
            var manualHtml = `
            <div class="form-group" id="manualTotalContainer" style="margin-top: 20px;">
                <label class="form-label required-star"><i class="fas fa-file-invoice-dollar"></i> إجمالي المبلغ المطلوب</label>
                <div style="display: flex; gap: 10px; align-items: center;">
                    <input type="text" id="manualTotalInput" class="payment-amount"
                           placeholder="0.000" style="flex: 1; padding: 10px; border: 1px solid #e0e0e0; border-radius: 8px; text-align: left; direction: ltr;">
                    <button type="button" class="add-payment-btn" onclick="setManualTotal()" style="width: auto; padding: 10px 20px;">
                        <i class="fas fa-check"></i> تعيين
                    </button>
                </div>
                <div class="validation-message" id="manualTotalError" style="display: none;">الرجاء إدخال مبلغ صحيح أكبر من الصفر</div>
            </div>
        `;

            // Insert in the first form section, after the existing total field
            $('.form-section').first().append(manualHtml);

            // Focus on the input
            setTimeout(function () {
                $('#manualTotalInput').focus();
            }, 100);
        }

        function setManualTotal() {
            var manualInput = $('#manualTotalInput');
            var value = manualInput.val();
            var num = parseNumber(value);

            if (!isNaN(num) && num > 0) {
                // Format and set the value
                manualInput.val(formatNumber(num));

                // Update the hidden total due
                $('#<%= hdnTotalDue.ClientID %>').val(num);

                // Update the total due display
                $('#totalDueDisplay').text(formatNumber(num));

                // Hide error and remove error styling
                $('#manualTotalError').hide();
                manualInput.removeClass('field-error').css('border-color', '');

                // Update totals and validation
                updateTotals();
                validatePayments();
                updatePaymentBreakdown();

                // Enable submit button for Other receipts
                var submitButton = $('#<%= sdad.ClientID %>');
                if (submitButton.length) {
                    // Check if all required fields are filled
                    var customerValue = '';
                    var customerElement = ASPxClientControl.GetControlCollection().GetByName('Customer');
                    if (customerElement) {
                        customerValue = customerElement.GetValue() || '';
                    }

                    var moveDateValue = '';
                    var moveDateElement = ASPxClientControl.GetControlCollection().GetByName('MoveDate');
                    if (moveDateElement) {
                        moveDateValue = moveDateElement.GetValue();
                    }

                    var noteValue = '';
                    var noteElement = ASPxClientControl.GetControlCollection().GetByName('Note');
                    if (noteElement) {
                        noteValue = noteElement.GetValue() || '';
                    }

                    if (customerValue.trim() !== '' && moveDateValue !== '' && noteValue.trim() !== '' && noteValue.trim() !== '/') {
                        submitButton.prop('disabled', false);
                        submitButton.parent().find('.button-disabled').removeClass('button-disabled');
                    }
                }

                showMessage('تم تعيين المبلغ الإجمالي بنجاح', 'success');
            } else {
                // Show error
                $('#manualTotalError').show();
                manualInput.addClass('field-error').css('border-color', '#f72585');
                showMessage('الرجاء إدخال مبلغ صحيح أكبر من الصفر', 'error');

                // Disable submit button
                if (isOtherReceipt) {
                    var submitButton = $('#<%= sdad.ClientID %>');
                    if (submitButton.length) {
                        submitButton.prop('disabled', true);
                        submitButton.parent().find('.modern-btn').addClass('button-disabled');
                    }
                }
            }
        }

        function updateGridData(document) {
            var gridHtml = `
            <table style="width:100%; border-collapse:collapse;">
                <thead>
                    <tr style="background:#f8f9fa; color:#333;">
                        <th style="padding:10px; text-align:center; border:1px solid #ddd;">رقم الوثيقة</th>
                        <th style="padding:10px; text-align:center; border:1px solid #ddd;">رقم الملحق</th>
                        <th style="padding:10px; text-align:center; border:1px solid #ddd;">رقم الإشعار</th>
                        <th style="padding:10px; text-align:center; border:1px solid #ddd;">اجمالي القسط</th>
                        <th style="padding:10px; text-align:center; border:1px solid #ddd;">العملة</th>
                        <th style="padding:10px; text-align:center; border:1px solid #ddd;">العمولة - المسوق</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="padding:10px; text-align:center; border:1px solid #ddd;">${document.polNo}</td>
                        <td style="padding:10px; text-align:center; border:1px solid #ddd;">${document.endNo}</td>
                        <td style="padding:10px; text-align:center; border:1px solid #ddd;">${document.loadNo}</td>
                        <td style="padding:10px; text-align:right; border:1px solid #ddd; color:#2e7d32; font-weight:bold;">${formatNumber(document.totprm)}</td>
                        <td style="padding:10px; text-align:center; border:1px solid #ddd;">${document.tpName}</td>
                        <td style="padding:10px; text-align:center; border:1px solid #ddd;">${document.commissioned || '0'}</td>
                    </tr>
                </tbody>
            </table>
        `;

            $('#<%= pnlGridContainer.ClientID %>').html(gridHtml);
            $('#<%= hdnGridData.ClientID %>').val(JSON.stringify(document));
        }

        function createFirstPaymentEntry() {
            try {
                entryCounter = 1;
                var entryId = 'paymentEntry_' + entryCounter;
                var newEntry = {
                    id: entryId,
                    type: '1',
                    amount: 0,
                    details: '',
                    account: '',
                    isValid: false
                };
                paymentEntries.push(newEntry);
                var html = createPaymentEntryHTML(newEntry, 1);
                $('#paymentEntriesContainer').append(html);
            } catch (error) {
                console.error('Error creating first payment entry:', error);
                showMessage('خطأ في إنشاء طريقة الدفع الأولى', 'error');
            }
        }

        function createPaymentEntryHTML(entry, number) {
            var paymentType = paymentTypes.find(t => t.value === entry.type);
            var amountValue = entry.amount > 0 ? formatNumber(entry.amount) : '';
            return `
            <div class="payment-entry-card payment-type-${entry.type}" id="${entry.id}" data-entry='${JSON.stringify(entry)}'>
                <div class="entry-header">
                    <div class="entry-number">${number}</div>
                    ${number > 1 ? '<button type="button" class="remove-entry" onclick="removePaymentEntry(\'' + entry.id + '\')"><i class="fas fa-times"></i></button>' : ''}
                </div>
                <div class="entry-form">
                    <div class="form-group">
                        <label class="form-label">طريقة الدفع</label>
                        <select class="payment-type-select" style="width: 100%; padding: 10px; border: 1px solid #e0e0e0; border-radius: 8px;">
                            ${paymentTypes.map(type => `<option value="${type.value}" ${type.value === entry.type ? 'selected' : ''}>${type.name}</option>`).join('')}
                        </select>
                    </div>
                    <div class="form-group">
                        <label class="form-label">المبلغ</label>
                        <input type="text" class="payment-amount" value="${amountValue}" placeholder="0.000" data-touched="false"
                            style="width: 100%; padding: 10px; border: 1px solid #e0e0e0; border-radius: 8px; text-align: left; direction: ltr;">
                        <div class="validation-message" id="amountError_${entry.id}">المبلغ مطلوب</div>
                    </div>
                    <div class="form-group">
                        <label class="form-label">تفاصيل إضافية</label>
                        <input type="text" class="payment-details" value="${entry.details || ''}" placeholder="رقم الصك / البنك / etc"
                            style="width: 100%; padding: 10px; border: 1px solid #e0e0e0; border-radius: 8px;">
                    </div>
                </div>
                <div class="account-selection" id="accountSelection_${entry.id}" style="display: none;">
                    <div class="form-group">
                        <label class="form-label">اختر الحساب</label>
                        <select class="account-select" style="width: 100%; padding: 10px; border: 1px solid #e0e0e0; border-radius: 8px;" data-touched="false">
                            <option value="">-- اختر حساب --</option>
                        </select>
                        <div class="validation-message" id="accountError_${entry.id}">اختيار الحساب مطلوب</div>
                    </div>
                </div>
            </div>
        `;
        }

        function addPaymentEntry() {
            try {
                entryCounter++;
                var entryId = 'paymentEntry_' + entryCounter;
                var newEntry = {
                    id: entryId,
                    type: '1',
                    amount: 0,
                    details: '',
                    account: '',
                    isValid: false
                };
                paymentEntries.push(newEntry);
                // Store immediately
                savePaymentEntries();
                var html = createPaymentEntryHTML(newEntry, paymentEntries.length);
                $('#paymentEntriesContainer').append(html);
                updateTotals();
                validatePayments();
                updatePaymentBreakdown();
            } catch (error) {
                console.error('Error adding payment entry:', error);
                showMessage('خطأ في إضافة طريقة دفع جديدة', 'error');
            }
        }

        function removePaymentEntry(entryId) {
            try {
                if (paymentEntries.length <= 1) {
                    showMessage('يجب أن يكون هناك على الأقل طريقة دفع واحدة', 'error');
                    return;
                }
                $('#' + entryId).remove();
                paymentEntries = paymentEntries.filter(entry => entry.id !== entryId);
                // Store immediately
                savePaymentEntries();
                renumberEntries();
                updateTotals();
                validatePayments();
                updatePaymentBreakdown();
            } catch (error) {
                console.error('Error removing payment entry:', error);
                showMessage('خطأ في إزالة طريقة الدفع', 'error');
            }
        }

        function renumberEntries() {
            $('.payment-entry-card').each(function (index) {
                var newNumber = index + 1;
                var entryId = $(this).attr('id');
                $(this).find('.entry-number').text(newNumber);
                var removeButton = $(this).find('.remove-entry');
                if (newNumber === 1 && removeButton.length > 0) {
                    removeButton.remove();
                } else if (newNumber > 1 && removeButton.length === 0) {
                    $(this).find('.entry-header').append('<button type="button" class="remove-entry" onclick="removePaymentEntry(\'' + entryId + '\')"><i class="fas fa-times"></i></button>');
                }
            });
        }

        function updatePaymentType(select, entryId) {
            try {
                var type = $(select).val();
                var entryCard = $('#' + entryId);
                var accountSelection = $('#accountSelection_' + entryId);
                var paymentType = paymentTypes.find(t => t.value === type);

                // Update styling
                entryCard.removeClass(function (index, className) {
                    return (className.match(/payment-type-\d+/g) || []).join(' ');
                });
                entryCard.addClass('payment-type-' + type);

                // Update in array
                var entry = paymentEntries.find(e => e.id === entryId);
                if (entry) {
                    entry.type = type;
                    savePaymentEntries();
                }

                // Show/hide account selection
                if (paymentType.requiresAccount) {
                    accountSelection.show();
                    loadAccountsForEntry(entryId, paymentType.dataSource);
                } else {
                    accountSelection.hide();
                    if (entry) entry.account = '';
                    $('#accountError_' + entryId).hide();
                }

                validatePayments();
                updatePaymentBreakdown();
            } catch (error) {
                console.error('Error updating payment type:', error);
            }
        }

        function loadAccountsForEntry(entryId, dataSource) {
            try {
                console.log('Loading accounts for:', dataSource);
                var accountSelect = $('#' + entryId + ' .account-select');
                accountSelect.html('<option value="">-- جاري تحميل الحسابات --</option>');
                accountSelect.addClass('loading-accounts');

                // Get branch from session or hidden field
                var branch = '<%= Session("Branch") %>' || '001';

                // Call Web Method with error handling
                //*url: "../Handlers/AccountHandler.ashx",**/
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("~/Handlers/AccountHandler.ashx") %>',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    action: "getAccounts",
                    dataSource: dataSource,
                    branch: branch
                }),
                success: function (response) {
                    console.log('Accounts response:', response);
                    accountSelect.removeClass('loading-accounts');
                    accountSelect.empty();
                    accountSelect.append('<option value="">-- اختر حساب --</option>');

                    if (response && response.success && response.accounts && response.accounts.length > 0) {
                        $.each(response.accounts, function (index, account) {
                            accountSelect.append(
                                '<option value="' + account.Value + '">' +
                                account.Text + '</option>'
                            );
                        });
                        // Restore previously selected value if exists
                        var entry = paymentEntries.find(e => e.id === entryId);
                        if (entry && entry.account) {
                            accountSelect.val(entry.account);
                        }
                    } else {
                        accountSelect.append('<option value="">-- لا توجد حسابات متاحة --</option>');
                        showMessage('لا توجد حسابات متاحة للنوع المختار', 'error');
                    }

                    // Trigger change event to validate
                    accountSelect.trigger('change');
                },
                error: function (xhr, status, error) {
                    console.error('Error loading accounts:', error, xhr.responseText);
                    accountSelect.removeClass('loading-accounts');
                    accountSelect.html('<option value="">-- خطأ في التحميل --</option>');
                    showMessage('خطأ في تحميل قائمة الحسابات. تأكد من اتصال الخادم.', 'error');
                }
            });
            } catch (error) {
                console.error('Error loading accounts:', error);
                $('#' + entryId + ' .account-select').html('<option value="">-- خطأ في التحميل --</option>');
                showMessage('خطأ في تحميل الحسابات: ' + error.message, 'error');
            }
        }

        function updateEntryAmount(input, entryId) {
            try {
                var value = $(input).val();
                var num = parseNumber(value);
                var entry = paymentEntries.find(e => e.id === entryId);
                if (entry) {
                    entry.amount = num;
                    entry.isValid = num > 0 && (!entry.account || entry.account !== '' || !paymentTypes.find(t => t.value === entry.type).requiresAccount);
                    savePaymentEntries();
                }
                updateTotals();
                validatePayments();
                updatePaymentBreakdown();
            } catch (error) {
                console.error('Error updating entry amount:', error);
            }
        }

        function formatEntryAmount(input, entryId) {
            try {
                var value = $(input).val();
                if (value && value.trim() !== '') {
                    var num = parseNumber(value);
                    if (!isNaN(num) && num > 0) {
                        $(input).val(formatNumber(num));
                        $('#amountError_' + entryId).hide();
                        $(input).removeClass('validation-error');
                        $(input).removeClass('field-error').css('border-color', '');
                    } else {
                        $(input).addClass('validation-error');
                        $('#amountError_' + entryId).show();
                    }
                }
                updateTotals();
                validatePayments();
            } catch (error) {
                console.error('Error formatting entry amount:', error);
            }
        }

        function updateEntryDetails(input, entryId) {
            try {
                var value = $(input).val();
                var entry = paymentEntries.find(e => e.id === entryId);
                if (entry) {
                    entry.details = value;
                    savePaymentEntries();
                }
            } catch (error) {
                console.error('Error updating entry details:', error);
            }
        }

        function validateAccountSelection(select, entryId) {
            try {
                var value = $(select).val();
                var errorElement = $('#accountError_' + entryId);
                var entry = paymentEntries.find(e => e.id === entryId);
                if (!entry) return false;

                if (!value) {
                    $(select).addClass('validation-error');
                    $(select).addClass('field-error').css('border-color', '#f72585');
                    errorElement.show();
                    entry.account = '';
                    entry.isValid = false;
                    savePaymentEntries();
                    return false;
                } else {
                    $(select).removeClass('validation-error');
                    $(select).removeClass('field-error').css('border-color', '');
                    errorElement.hide();
                    entry.account = value;
                    entry.isValid = entry.amount > 0;
                    savePaymentEntries();
                    return true;
                }
            } catch (error) {
                console.error('Error validating account selection:', error);
                return false;
            }
        }

        function updateTotals() {
            try {
                var totalDue = parseNumber($('#<%= hdnTotalDue.ClientID %>').val()) || 0;
                var totalPaid = 0;
                paymentEntries.forEach(function (entry) {
                    if (entry.amount > 0) {
                        totalPaid += entry.amount;
                    }
                });
                var balance = totalDue - totalPaid;

                $('#totalDueDisplay').text(formatNumber(totalDue));
                $('#totalPaid').text(formatNumber(totalPaid));
                $('#balance').text(formatNumber(Math.abs(balance)));

                updatePaymentStatus(totalDue, totalPaid, balance);
            } catch (error) {
                console.error('Error updating totals:', error);
            }
        }

        function updatePaymentStatus(totalDue, totalPaid, balance) {
            try {
                var statusElement = $('#paymentStatus');
                var statusHtml = '';

                if (isOtherReceipt) {
                    // Special handling for Other receipts
                    if (totalPaid === 0) {
                        statusHtml = `<div class="payment-status status-under">
                        <i class="fas fa-info-circle"></i> لم يتم إضافة أي مدفوعات بعد
                    </div>`;
                    } else if (Math.abs(balance) < 0.001) {
                        statusHtml = `<div class="payment-status status-exact">
                        <i class="fas fa-check-circle"></i> التسوية كاملة (${formatNumber(totalPaid)} = ${formatNumber(totalDue)})
                    </div>`;
                    } else if (balance > 0) {
                        statusHtml = `<div class="payment-status status-under">
                        <i class="fas fa-exclamation-triangle"></i> مبلغ ناقص (${formatNumber(balance)}) - إجمالي المدفوع ${formatNumber(totalPaid)}
                    </div>`;
                    } else {
                        statusHtml = `<div class="payment-status status-over">
                        <i class="fas fa-exclamation-triangle"></i> مبلغ زائد (${formatNumber(Math.abs(balance))}) - إجمالي المدفوع ${formatNumber(totalPaid)}
                    </div>`;
                    }
                } else {
                    // Original logic for policy payments
                    if (Math.abs(balance) < 0.001) {
                        statusHtml = `<div class="payment-status status-exact">
                        <i class="fas fa-check-circle"></i> التسوية كاملة (${formatNumber(totalPaid)} = ${formatNumber(totalDue)})
                    </div>`;
                    } else if (balance > 0) {
                        statusHtml = `<div class="payment-status status-under">
                        <i class="fas fa-exclamation-triangle"></i> مبلغ ناقص (${formatNumber(balance)}) - إجمالي المدفوع ${formatNumber(totalPaid)}
                    </div>`;
                    } else {
                        statusHtml = `<div class="payment-status status-over">
                        <i class="fas fa-exclamation-triangle"></i> مبلغ زائد (${formatNumber(Math.abs(balance))}) - إجمالي المدفوع ${formatNumber(totalPaid)}
                    </div>`;
                    }
                }

                statusElement.html(statusHtml);
            } catch (error) {
                console.error('Error updating payment status:', error);
            }
        }

        function updatePaymentBreakdown() {
            try {
                var breakdown = {};
                paymentEntries.forEach(function (entry) {
                    if (entry.amount > 0) {
                        var typeName = paymentTypes.find(t => t.value === entry.type)?.name || 'غير معروف';
                        if (!breakdown[typeName]) {
                            breakdown[typeName] = 0;
                        }
                        breakdown[typeName] += entry.amount;
                    }
                });

                var breakdownHtml = '';
                for (var type in breakdown) {
                    if (breakdown[type] > 0) {
                        breakdownHtml += `
                        <div class="total-item">
                            <div class="total-type">${type}</div>
                            <div class="total-amount">${formatNumber(breakdown[type])}</div>
                        </div>
                    `;
                    }
                }

                if (breakdownHtml === '') {
                    breakdownHtml = '<div style="text-align: center; color: #999; padding: 20px;">لم يتم إضافة أي مدفوعات</div>';
                }

                $('#paymentBreakdown').html(breakdownHtml);
            } catch (error) {
                console.error('Error updating payment breakdown:', error);
            }
        }

        function validatePayments() {
            try {
                var totalDue = parseNumber($('#<%= hdnTotalDue.ClientID %>').val()) || 0;
                var totalPaid = 0;
                var isValid = true;
                var errorMessages = [];

                // Check if we're in initial load state (no payments yet)
                var hasValidPayments = false;
                paymentEntries.forEach(function (entry) {
                    if (entry.amount > 0) {
                        hasValidPayments = true;
                    }
                });

                // Only validate payments if we have at least some payment data
                // OR if we're trying to submit
                var isSubmittingFlag = $('#<%= hdnIsFinalSubmit.ClientID %>').val() === '1';

                if (!isSubmittingFlag && !hasValidPayments && paymentEntries.length === 1) {
                    // Initial state - don't show validation errors yet
                    $('#validationErrors').html('');
                    return true;
                }

                // For Other receipts, check if manual total is set
                if (isOtherReceipt && totalDue <= 0) {
                    errorMessages.push('الرجاء إدخال المبلغ الإجمالي المطلوب أولاً في حقل "إجمالي المبلغ المطلوب"');
                    isValid = false;
                }

                if (paymentEntries.length === 0) {
                    errorMessages.push('يرجى إضافة طريقة دفع واحدة على الأقل');
                    isValid = false;
                }

                paymentEntries.forEach(function (entry) {
                    var paymentTypeObj = paymentTypes.find(t => t.value === entry.type);

                    // Show amount error only if the field has been touched or if it's required for submission
                    var amountInput = $('#' + entry.id + ' .payment-amount');
                    var hasBeenTouched = amountInput.data('touched') || amountInput.val() !== '';

                    if (entry.amount <= 0) {
                        if (hasBeenTouched || isSubmittingFlag) {
                            $('#amountError_' + entry.id).show();
                            amountInput.addClass('validation-error');
                            if (isSubmittingFlag) {
                                errorMessages.push('المبلغ في إحدى المدخلات غير صالح');
                                isValid = false;
                            }
                        }
                    } else {
                        $('#amountError_' + entry.id).hide();
                        amountInput.removeClass('validation-error');
                        totalPaid += entry.amount;
                    }

                    if (paymentTypeObj && paymentTypeObj.requiresAccount) {
                        var accountSelect = $('#' + entry.id + ' .account-select');
                        var accountTouched = accountSelect.data('touched') || accountSelect.val() !== '';

                        if (!entry.account || entry.account === '') {
                            if (accountTouched || isSubmittingFlag) {
                                $('#accountError_' + entry.id).show();
                                accountSelect.addClass('validation-error');
                                if (isSubmittingFlag && entry.amount > 0) {
                                    errorMessages.push('اختيار الحساب مطلوب لطريقة الدفع ' + paymentTypeObj.name);
                                    isValid = false;
                                }
                            }
                        } else {
                            $('#accountError_' + entry.id).hide();
                            accountSelect.removeClass('validation-error');
                        }
                    }
                });

                // Different validation for Other receipts vs Policy payments
                if (isOtherReceipt) {
                    // For Other receipts: Payments must not exceed total, but can be less
                    if (totalPaid > totalDue) {
                        errorMessages.push('إجمالي المدفوعات (' + formatNumber(totalPaid) + ') يجب ألا يتجاوز المبلغ المطلوب (' + formatNumber(totalDue) + ')');
                        isValid = false;
                    }
                } else {
                    // For Policy payments: Payments must exactly equal total
                    if (Math.abs(totalPaid - totalDue) > 0.001) {
                        errorMessages.push('إجمالي المدفوعات (' + formatNumber(totalPaid) + ') يجب أن يساوي المبلغ المستحق (' + formatNumber(totalDue) + ')');
                        isValid = false;
                    }
                }

                var validationContainer = $('#validationErrors');
                if (errorMessages.length > 0) {
                    var errorsHtml = '<div style="background: rgba(247, 37, 133, 0.1); padding: 15px; border-radius: 8px; margin: 15px 0; border: 1px solid #f72585;">';
                    errorsHtml += '<strong style="color: #f72585;"><i class="fas fa-exclamation-triangle"></i> أخطاء التحقق:</strong><ul style="margin: 10px 0 0 0; padding-right: 20px;">';
                    errorMessages.forEach(msg => {
                        errorsHtml += '<li style="margin-bottom: 5px;">' + msg + '</li>';
                    });
                    errorsHtml += '</ul></div>';
                    validationContainer.html(errorsHtml);
                } else {
                    validationContainer.html('');
                }

                var submitButton = $('#<%= sdad.ClientID %>');
                if (submitButton.length) {
                    // Only enable button if we have valid payments
                    if (isOtherReceipt) {
                        // For Other receipts: need manual total set and at least one payment
                        var hasManualTotal = totalDue > 0;
                        var hasAnyPayment = paymentEntries.some(e => e.amount > 0);
                        var isValidForOther = hasManualTotal && hasAnyPayment && totalPaid <= totalDue && isValid;

                        if (isValidForOther) {
                            submitButton.prop('disabled', false);
                            submitButton.parent().find('.button-disabled').removeClass('button-disabled');
                        } else {
                            submitButton.prop('disabled', true);
                            submitButton.parent().find('.modern-btn').addClass('button-disabled');
                        }
                    } else {
                        // Original logic for policy payments
                        var hasValidPayments = paymentEntries.some(e => e.amount > 0);
                        if (hasValidPayments && Math.abs(totalPaid - totalDue) < 0.001 && isValid) {
                            submitButton.prop('disabled', false);
                            submitButton.parent().find('.button-disabled').removeClass('button-disabled');
                        } else {
                            submitButton.prop('disabled', true);
                            submitButton.parent().find('.modern-btn').addClass('button-disabled');
                        }
                    }
                }

                return isValid;
            } catch (error) {
                console.error('Error validating payments:', error);
                return false;
            }
        }

        function collectPaymentData() {
            var payments = [];
            console.log('collectPaymentData called. Payment entries:', paymentEntries);

            paymentEntries.forEach(function (entry) {
                console.log(`Checking entry: id=${entry.id}, amount=${entry.amount}, type=${entry.type}`);

                if (entry.amount > 0) {
                    var paymentTypeObj = paymentTypes.find(t => t.value === entry.type);
                    console.log(`Adding payment: ${entry.amount} of type ${paymentTypeObj ? paymentTypeObj.name : 'unknown'}`);

                    payments.push({
                        Type: entry.type,
                        Amount: entry.amount,
                        Details: entry.details,
                        Account: entry.account,
                        TypeName: paymentTypeObj ? paymentTypeObj.name : 'غير معروف',
                        RequiresAccount: paymentTypeObj ? paymentTypeObj.requiresAccount : false
                    });
                }
            });

            console.log('Total payments collected:', payments.length);
            return payments;
        }

        function validateAndSubmit() {
            try {
                if (isSubmitting) return false;
                isSubmitting = true;
                showLoading(true);

                // Clear previous errors
                $('#messageContainer').empty();
                $('.field-error').removeClass('field-error');

                // Get current receipt type
                var isOtherReceipt = $('#<%= hdnIsOtherReceipt.ClientID %>').val() === '1';

                // ===== STRICT VALIDATION =====
                if (isOtherReceipt) {
                    var validationErrors = [];

                    // 1. Validate Customer (REQUIRED)
                    var customerElement = ASPxClientControl.GetControlCollection().GetByName('Customer');
                    var customerValue = '';
                    if (customerElement) {
                        customerValue = customerElement.GetValue() || '';
                    } else {
                        customerValue = $('#<%= Customer.ClientID %>_I').val() || '';
                }

                if (!customerValue || customerValue.trim() === '') {
                    validationErrors.push('اسم العميل مطلوب للمقبوضات الأخرى');
                    $('#<%= Customer.ClientID %>_I').addClass('field-error').css('border-color', '#f72585');
                }

                // 2. Validate MoveDate (REQUIRED)
                var moveDateElement = ASPxClientControl.GetControlCollection().GetByName('MoveDate');
                var moveDateValue = '';
                if (moveDateElement) {
                    moveDateValue = moveDateElement.GetValue();
                } else {
                    moveDateValue = $('#<%= MoveDate.ClientID %>_I').val() || '';
                }

                if (!moveDateValue) {
                    validationErrors.push('تاريخ الحركة مطلوب للمقبوضات الأخرى');
                    $('#<%= MoveDate.ClientID %>_I').addClass('field-error').css('border-color', '#f72585');
                }

                // 3. Validate Note (REQUIRED)
                var noteElement = ASPxClientControl.GetControlCollection().GetByName('Note');
                var noteValue = '';
                if (noteElement) {
                    noteValue = noteElement.GetValue() || '';
                } else {
                    noteValue = $('#<%= Note.ClientID %>_I').val() || '';
                }

                if (!noteValue || noteValue.trim() === '' || noteValue.trim() === '/') {
                    validationErrors.push('الملاحظات مطلوبة للمقبوضات الأخرى');
                    $('#<%= Note.ClientID %>_I').addClass('field-error').css('border-color', '#f72585');
                }

                // 4. Validate Manual Total (REQUIRED)
                var totalDue = parseNumber($('#<%= hdnTotalDue.ClientID %>').val()) || 0;
                    if (totalDue <= 0) {
                        validationErrors.push('يرجى إدخال المبلغ الإجمالي المطلوب أولاً في حقل "إجمالي المبلغ المطلوب"');
                        $('#manualTotalInput').addClass('field-error').css('border-color', '#f72585');
                    }

                    // 5. Check payments
                    var tempPayments = [];
                    paymentEntries.forEach(function (entry) {
                        var amountInput = $('#' + entry.id + ' .payment-amount');
                        var amountValue = amountInput.val();
                        var amount = parseNumber(amountValue);

                        if (amount > 0) {
                            tempPayments.push({
                                amount: amount,
                                type: entry.type
                            });
                        }
                    });

                    // 6. Validate at least one payment entry with amount > 0
                    if (tempPayments.length === 0) {
                        validationErrors.push('يرجى إدخال مبلغ في طريقة الدفع على الأقل');

                        // Highlight all payment amount fields
                        $('.payment-amount').each(function () {
                            $(this).addClass('field-error').css('border-color', '#f72585');
                        });
                    }

                    // ===== BLOCK SUBMISSION IF ERRORS =====
                    if (validationErrors.length > 0) {
                        var errorHtml = '<div class="error-message-box">';
                        errorHtml += '<strong><i class="fas fa-exclamation-triangle"></i> أخطاء في البيانات:</strong><br>';
                        validationErrors.forEach(function (error, index) {
                            errorHtml += '<div style="margin: 5px 0; padding-right: 10px;">' + (index + 1) + '. ' + error + '</div>';
                        });
                        errorHtml += '</div>';

                        $('#messageContainer').html(errorHtml).show();
                        showLoading(false);
                        isSubmitting = false;

                        // Scroll to errors
                        $('html, body').animate({
                            scrollTop: $('#messageContainer').offset().top - 100
                        }, 500);

                        return false;
                    }
                }

                // Save entries before validation
                savePaymentEntries();

                // Validate payments
                if (!validatePayments()) {
                    showLoading(false);
                    isSubmitting = false;
                    $('html, body').animate({
                        scrollTop: $('#validationErrors').offset().top - 100
                    }, 500);
                    return false;
                }

                var payments = collectPaymentData();
                var totalDue = parseNumber($('#<%= hdnTotalDue.ClientID %>').val()) || 0;
                var totalPaid = payments.reduce((sum, p) => sum + p.Amount, 0);

                // Additional check
                if (payments.length === 0) {
                    showMessage('يرجى إدخال مبلغ في طريقة الدفع على الأقل', 'error');
                    showLoading(false);
                    isSubmitting = false;
                    return false;
                }

                // Save payments data
                $('#<%= hdnPayments.ClientID %>').val(JSON.stringify(payments));
                $('#<%= hdnIsFinalSubmit.ClientID %>').val('0');

                // Show confirmation
                showConfirmation(payments, totalDue, totalPaid);
                showLoading(false);
                isSubmitting = false;
                return false;

            } catch (error) {
                console.error('Error in validateAndSubmit:', error);
                showMessage('حدث خطأ أثناء التحقق من البيانات: ' + error.message, 'error');
                showLoading(false);
                isSubmitting = false;
                return false;
            }
        }

        function confirmPayment() {
            try {
                // Double-check validation before final submission
                var isOtherReceipt = $('#<%= hdnIsOtherReceipt.ClientID %>').val() === '1';

                if (isOtherReceipt) {
                    var validationErrors = [];

                    // Validate Customer
                    var customerElement = ASPxClientControl.GetControlCollection().GetByName('Customer');
                    var customerValue = customerElement ? customerElement.GetValue() || '' : $('#<%= Customer.ClientID %>_I').val() || '';
                if (!customerValue || customerValue.trim() === '') {
                    validationErrors.push('اسم العميل مطلوب');
                }

                // Validate MoveDate
                var moveDateElement = ASPxClientControl.GetControlCollection().GetByName('MoveDate');
                var moveDateValue = moveDateElement ? moveDateElement.GetValue() : $('#<%= MoveDate.ClientID %>_I').val() || '';
                if (!moveDateValue) {
                    validationErrors.push('تاريخ الحركة مطلوب');
                }

                // Validate Note
                var noteElement = ASPxClientControl.GetControlCollection().GetByName('Note');
                var noteValue = noteElement ? noteElement.GetValue() || '' : $('#<%= Note.ClientID %>_I').val() || '';
                if (!noteValue || noteValue.trim() === '' || noteValue.trim() === '/') {
                    validationErrors.push('الملاحظات مطلوبة');
                }

                // Validate Manual Total
                var totalDue = parseNumber($('#<%= hdnTotalDue.ClientID %>').val()) || 0;
                    if (totalDue <= 0) {
                        validationErrors.push('المبلغ الإجمالي غير محدد');
                    }

                    if (validationErrors.length > 0) {
                        hideConfirmationPopup();
                        showMessage('أخطاء في البيانات:<br>' + validationErrors.join('<br>'), 'error');
                        return;
                    }
                }

                if (window.multiPaymentSubmitted) {
                    console.log('Payment already submitted, ignoring');
                    return;
                }

                if (isSubmitting) {
                    console.log('Already submitting, ignoring');
                    return;
                }

                window.multiPaymentSubmitted = true;
                isSubmitting = true;
                showLoading(true);

                // Save payments to hidden field
                var payments = collectPaymentData();
                $('#<%= hdnPayments.ClientID %>').val(JSON.stringify(payments));
                $('#<%= hdnIsFinalSubmit.ClientID %>').val('1');

                // Disable ALL buttons to prevent double click
                $('.modern-btn').prop('disabled', true).addClass('button-disabled');

                // Hide confirmation popup
                hideConfirmationPopup();

                // Force full postback
                __doPostBack('', '');

            } catch (error) {
                console.error('Error in confirmPayment:', error);
                showMessage('حدث خطأ أثناء تأكيد الدفع: ' + error.message, 'error');
                showLoading(false);
                isSubmitting = false;
                window.multiPaymentSubmitted = false;
                $('.modern-btn').prop('disabled', false).removeClass('button-disabled');
            }
        }

        function showConfirmation(payments, totalDue, totalPaid) {
            try {
                var summaryHtml = `
                <div style="text-align: right; margin-bottom: 20px;">
                    <div style="font-size: 16px; margin-bottom: 10px;">
                        <strong>إجمالي المستحق:</strong> ${formatNumber(totalDue)}
                    </div>
                    <div style="font-size: 16px; margin-bottom: 10px;">
                        <strong>إجمالي المدفوع:</strong> ${formatNumber(totalPaid)}
                    </div>
                    <div style="font-size: 16px; margin-bottom: 20px; color: ${Math.abs(totalDue - totalPaid) < 0.001 ? '#4CAF50' : (totalPaid < totalDue ? '#FF9800' : '#f44336')};">
                        <strong>الفرق:</strong> ${formatNumber(Math.abs(totalDue - totalPaid))}
                        ${Math.abs(totalDue - totalPaid) < 0.001 ? '(تسوية كاملة)' : (totalPaid < totalDue ? '(مبلغ ناقص)' : '(مبلغ زائد)')}
                    </div>
                    <div style="background: #f8f9fa; padding: 15px; border-radius: 8px; margin-bottom: 20px;">
                        <strong>تفاصيل المدفوعات:</strong>
                        <div style="margin-top: 10px;">
            `;

                payments.forEach((payment, index) => {
                    var paymentType = paymentTypes.find(t => t.value === payment.Type);
                    summaryHtml += `
                    <div style="padding: 12px; margin-bottom: 8px; background: white; border-radius: 6px; border: 1px solid #e0e0e0; border-left: 4px solid ${paymentType ? paymentType.color : '#999'};">
                        <div style="display: flex; justify-content: space-between; align-items: center;">
                            <span><strong>${index + 1}. ${payment.TypeName}</strong></span>
                            <span style="font-weight: 700; color: #333; font-size: 16px;">${formatNumber(payment.Amount)}</span>
                        </div>
                        ${payment.Details ? `<div style="font-size: 13px; color: #666; margin-top: 5px;"><i class="fas fa-info-circle"></i> ${payment.Details}</div>` : ''}
                        ${payment.Account ? `<div style="font-size: 13px; color: #666; margin-top: 5px;"><i class="fas fa-wallet"></i> الحساب: ${payment.Account}</div>` : ''}
                    </div>
                `;
                });

                summaryHtml += `
                        </div>
                    </div>
                </div>
            `;

                $('#confirmationContent').html(summaryHtml);
                showConfirmationPopup();
            } catch (error) {
                console.error('Error showing confirmation:', error);
                showMessage('حدث خطأ أثناء عرض التأكيد: ' + error.message, 'error');
            }
        }

        function savePaymentEntries() {
            try {
                // Update all entries with current values from UI
                paymentEntries.forEach(function (entry) {
                    var $entry = $('#' + entry.id);
                    if ($entry.length) {
                        entry.type = $entry.find('.payment-type-select').val();

                        // Get amount from input field
                        var amountInput = $entry.find('.payment-amount').val();
                        entry.amount = parseNumber(amountInput);

                        entry.details = $entry.find('.payment-details').val();
                        var accountSelect = $entry.find('.account-select');
                        if (accountSelect.length && accountSelect.val()) {
                            entry.account = accountSelect.val();
                        }
                    }
                });

                var jsonData = JSON.stringify(paymentEntries);
                $('#<%= hdnPaymentEntries.ClientID %>').val(jsonData);
                console.log('Payment entries saved:', paymentEntries.length);

                // Debug log
                paymentEntries.forEach(function (entry, index) {
                    console.log(`Entry ${index}: amount = ${entry.amount}, type = ${entry.type}`);
                });

                return jsonData;
            } catch (error) {
                console.error('Error saving payment entries:', error);
                return '[]';
            }
        }

        function restorePaymentEntries() {
            try {
                var storedEntries = $('#<%= hdnPaymentEntries.ClientID %>').val();
                if (storedEntries && storedEntries !== '[]') {
                    try {
                        paymentEntries = JSON.parse(storedEntries);
                        console.log('Restored payment entries:', paymentEntries.length);
                    } catch (e) {
                        console.error('Error parsing payment entries:', e);
                        paymentEntries = [];
                    }
                }

                if (paymentEntries && paymentEntries.length > 0) {
                    entryCounter = paymentEntries.length;
                    $('#paymentEntriesContainer').empty();
                    paymentEntries.forEach(function (entry, index) {
                        var html = createPaymentEntryHTML(entry, index + 1);
                        $('#paymentEntriesContainer').append(html);

                        // Set the values after a small delay
                        setTimeout(function () {
                            var $entry = $('#' + entry.id);
                            if ($entry.length) {
                                $entry.find('.payment-type-select').val(entry.type || '1');
                                $entry.find('.payment-amount').val(entry.amount > 0 ? formatNumber(entry.amount) : '');
                                $entry.find('.payment-details').val(entry.details || '');
                                if (entry.account && entry.account !== '') {
                                    $entry.find('.account-select').val(entry.account);
                                }
                            }
                        }, 50);
                    });

                    // Update totals after all entries are restored
                    setTimeout(function () {
                        updateTotals();
                        validatePayments();
                        updatePaymentBreakdown();
                    }, 300);
                } else {
                    createFirstPaymentEntry();
                }
            } catch (error) {
                console.error('Error restoring payment entries:', error);
                createFirstPaymentEntry();
            }
        }

        function restorePaymentEntriesFromServer() {
            try {
                var storedEntries = $('#<%= hdnPaymentEntries.ClientID %>').val();
                if (storedEntries && storedEntries !== '[]') {
                    try {
                        var newEntries = JSON.parse(storedEntries);
                        if (newEntries.length > 0) {
                            paymentEntries = newEntries;
                            entryCounter = paymentEntries.length;
                            console.log('Payment entries restored from server:', paymentEntries.length);
                        }
                    } catch (e) {
                        console.error('Error parsing server payment entries:', e);
                    }
                }
            } catch (error) {
                console.error('Error restoring payment entries from server:', error);
            }
        }

        function restoreGridData() {
            try {
                var gridData = $('#<%= hdnGridData.ClientID %>').val();
                if (gridData && gridData !== '') {
                    try {
                        console.log('Grid data state restored');
                    } catch (e) {
                        console.error('Error restoring grid data: ', e);
                    }
                }
            } catch (error) {
                console.error('Error restoring grid data: ', error);
            }
        }

        function formatNumber(num) {
            if (num === null || num === undefined || num === '' || num === '0') return '0.000';
            var n = parseFloat(num.toString().replace(/,/g, '').replace(/[^\d.-]/g, ''));
            if (isNaN(n)) return '0.000';
            return n.toLocaleString('en-US', {
                minimumFractionDigits: 3,
                maximumFractionDigits: 3
            });
        }

        function parseNumber(formattedNum) {
            if (!formattedNum || formattedNum.trim() === '') return 0;
            return parseFloat(formattedNum.replace(/,/g, '').replace(/[^\d.-]/g, '')) || 0;
        }

        function ReturnToParentPage() {
            try {
                if (window.parent && typeof window.parent.SelectAndClosePopup === 'function') {
                    window.parent.SelectAndClosePopup(1);
                } else {
                    window.close();
                }
            } catch (error) {
                window.close();
            }
        }

        function preventBack() {
            window.history.pushState(null, null, window.location.href);
            window.onpopstate = function () {
                window.history.pushState(null, null, window.location.href);
            };
        }
    </script>
</body>
</html>