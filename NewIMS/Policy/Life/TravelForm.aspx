<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="TravelForm.aspx.vb" Inherits=".TravelForm" %>

<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>نموذج بيانات الزبون - التأمين الصحي للمسافرين</title>
    
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    
    <style>
        :root {
            --primary-color: #2c3e50;
            --secondary-color: #3498db;
            --accent-color: #1abc9c;
        }
        
        body {
            background-color: #f8f9fa;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
        }
        
        .header-card {
            background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
            color: white;
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 30px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        
        .form-card {
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.08);
            padding: 25px;
            margin-bottom: 20px;
            border: none;
        }
        
        .form-section-title {
            color: var(--primary-color);
            border-bottom: 2px solid var(--accent-color);
            padding-bottom: 8px;
            margin-bottom: 20px;
            font-weight: 600;
        }
        
        .form-label {
            font-weight: 600;
            color: #444;
            margin-bottom: 5px;
        }
        
        .required-field::after {
            content: " *";
            color: #e74c3c;
        }
        
        .btn-primary-custom {
            background-color: var(--accent-color);
            border: none;
            padding: 10px 25px;
            font-weight: 600;
            transition: all 0.3s ease;
        }
        
        .btn-primary-custom:hover {
            background-color: #16a085;
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        
        .icon-input {
            position: relative;
        }
        
        .icon-input i {
            position: absolute;
            left: 15px;
            top: 50%;
            transform: translateY(-50%);
            color: #7f8c8d;
        }
        
        .icon-input input, .icon-input select {
            padding-left: 40px;
        }
        
        .phone-input-group .input-group-text {
            background-color: #f8f9fa;
            font-weight: 500;
        }
        
        @media (max-width: 768px) {
            .form-card {
                padding: 15px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4 mb-5">
            <!-- Header -->
            <div class="header-card text-center">
                <h1><i class="fas fa-user-shield me-2"></i>بيانات الزبون - نوع التغطية</h1>
                <p class="mb-0">نظام التأمين الصحي للمسافرين</p>
            </div>
            
            <!-- Main Form -->
            <div class="row">
                <div class="col-lg-12">
                    <div class="form-card">
                        <!-- Certificate and File Info -->
                        <div class="row mb-4">
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">رقم الشهادة</label>
                                <asp:TextBox ID="txtCertificateNo" runat="server" CssClass="form-control" 
                                    placeholder="أدخل رقم الشهادة"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label">رقم الملف</label>
                                <asp:TextBox ID="txtFileNo" runat="server" CssClass="form-control" 
                                    placeholder="أدخل رقم الملف"></asp:TextBox>
                            </div>
                        </div>
                        
                        <!-- Search Section -->
                        <h5 class="form-section-title">
                            <i class="fas fa-search me-2"></i>البحث عن الزبون
                        </h5>
                        <div class="row mb-4">
                            <div class="col-md-12">
                                <div class="input-group mb-3">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                                        placeholder="أدخل اسم الزبون أو رقم الهاتف (3 أحرف على الأقل)"></asp:TextBox>
                                    <asp:Button ID="btnSearch" runat="server" Text="بحث" 
                                        CssClass="btn btn-outline-primary" />
                                </div>
                            </div>
                        </div>
                        
                        <!-- Customer Information -->
                        <h5 class="form-section-title">
                            <i class="fas fa-user-circle me-2"></i>معلومات الزبون
                        </h5>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">اسم الزبون</label>
                                <div class="icon-input">
                                    <i class="fas fa-user"></i>
                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" 
                                        placeholder="الاسم الكامل"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label">الاسم الإنجليزي</label>
                                <asp:TextBox ID="txtEnglishName" runat="server" CssClass="form-control" 
                                    placeholder="English Name"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">رقم الهاتف</label>
                                <div class="phone-input-group">
                                    <div class="input-group">
                                        <span class="input-group-text">+218</span>
                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" 
                                            placeholder="××××××××" TextMode="Phone"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">تاريخ الميلاد</label>
                                <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control" 
                                    TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">الجنس</label>
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="" Text="-- اختر الجنس --"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="ذكر"></asp:ListItem>
                                    <asp:ListItem Value="F" Text="أنثى"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">الجنسية</label>
                                <asp:TextBox ID="txtNationality" runat="server" CssClass="form-control" 
                                    placeholder="الجنسية"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <label class="form-label">العنوان</label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" 
                                    placeholder="العنوان الكامل" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </div>
                        </div>
                        
                        <!-- Coverage Information -->
                        <h5 class="form-section-title">
                            <i class="fas fa-shield-alt me-2"></i>معلومات التغطية التأمينية
                        </h5>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">نوع التغطية</label>
                                <asp:DropDownList ID="ddlCoverageType" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="" Text="-- اختر نوع التغطية --"></asp:ListItem>
                                    <asp:ListItem Value="health_traveler" Text="التأمين الصحي للمسافرين" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="other" Text="أنواع أخرى"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">منطقة التغطية</label>
                                <asp:DropDownList ID="ddlCoverageArea" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="" Text="-- اختر منطقة التغطية --"></asp:ListItem>
                                    <asp:ListItem Value="worldwide" Text="جميع أنحاء العالم"></asp:ListItem>
                                    <asp:ListItem Value="arab" Text="الدول العربية"></asp:ListItem>
                                    <asp:ListItem Value="europe" Text="أوروبا"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                        <!-- Coverage Period -->
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <label class="form-label required-field">فترة التغطية - من</label>
                                <asp:TextBox ID="txtCoverageFrom" runat="server" CssClass="form-control" 
                                    TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label required-field">إلى</label>
                                <asp:TextBox ID="txtCoverageTo" runat="server" CssClass="form-control" 
                                    TextMode="Date" Text="2026/02/08"></asp:TextBox>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label required-field">مدة التغطية (أيام)</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtCoverageDays" runat="server" CssClass="form-control" 
                                        Text="10" TextMode="Number"></asp:TextBox>
                                    <span class="input-group-text">يوم</span>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Additional Information -->
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">الجزء/القسم</label>
                                <asp:TextBox ID="txtSection" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label">إصدار الصالح</label>
                                <asp:TextBox ID="txtIssueValid" runat="server" CssClass="form-control" 
                                    placeholder="تاريخ الإصدار"></asp:TextBox>
                            </div>
                        </div>
                        
                        <!-- Submit Buttons -->
                        <div class="row mt-4">
                            <div class="col-md-12 text-center">
                                <asp:Button ID="btnSave" runat="server" Text="حفظ البيانات" 
                                    CssClass="btn btn-primary-custom me-2" />
                                <asp:Button ID="btnClear" runat="server" Text="مسح النموذج" 
                                    CssClass="btn btn-outline-secondary me-2" />
                                <asp:Button ID="btnPrint" runat="server" Text="طباعة" 
                                    CssClass="btn btn-outline-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Status Message -->
            <div class="row" id="divMessage" runat="server" visible="false">
                <div class="col-md-12">
                    <div class="alert alert-success alert-dismissible fade show" role="alert">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
    
    <script>
        // Client-side validation and functionality
        document.addEventListener('DOMContentLoaded', function() {
            // Calculate days between dates
            const fromDate = document.getElementById('<%= txtCoverageFrom.ClientID %>');
            const toDate = document.getElementById('<%= txtCoverageTo.ClientID %>');
            const daysField = document.getElementById('<%= txtCoverageDays.ClientID %>');
            
            function calculateDays() {
                if (fromDate.value && toDate.value) {
                    const start = new Date(fromDate.value);
                    const end = new Date(toDate.value);
                    const diffTime = Math.abs(end - start);
                    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
                    daysField.value = diffDays;
                }
            }
            
            if (fromDate && toDate && daysField) {
                fromDate.addEventListener('change', calculateDays);
                toDate.addEventListener('change', calculateDays);
            }
            
            // Phone number formatting
            const phoneInput = document.getElementById('<%= txtPhone.ClientID %>');
            if (phoneInput) {
                phoneInput.addEventListener('input', function(e) {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            }
        });
    </script>
</body>
</html>