<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="Default_Modern.aspx.vb" Inherits="Policy_Default_Modern" EnableSessionState="ReadOnly" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Simple CSS -->
    <style>
        /* Basic table styles */
        .simple-table {
            width: 100%;
            border-collapse: collapse;
            font-family: Arial, sans-serif;
        }

            .simple-table th {
                background-color: #2c3e50;
                color: white;
                padding: 10px;
                text-align: right;
                border: 1px solid #34495e;
            }

            .simple-table td {
                padding: 8px;
                border: 1px solid #ddd;
                text-align: right;
            }

            .simple-table tr:nth-child(even) {
                background-color: #f8f9fa;
            }

            .simple-table tr:hover {
                background-color: #e9ecef;
            }

        .loading {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: white;
            padding: 20px;
            border: 1px solid #ccc;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
            z-index: 1000;
        }

        .btn {
            padding: 6px 12px;
            background: #3498db;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            margin: 2px;
        }

            .btn:hover {
                background: #2980b9;
            }

        .search-box {
            padding: 6px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 300px;
        }

        .toolbar {
            margin-bottom: 15px;
            padding: 10px;
            background: #ecf0f1;
            border-radius: 4px;
        }
    </style>

    <!-- Hidden Fields -->
    <asp:HiddenField ID="hdnSys" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="hdnBranch" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="hdnUserID" runat="server" ClientIDMode="Static" />

    <div>
        <!-- Loading Indicator -->
        <div id="loadingDiv" class="loading">
            <div style="text-align: center;">
                <div style="margin-bottom: 10px;">تحميل البيانات...</div>
                <div style="width: 40px; height: 40px; border: 4px solid #f3f3f3; border-top: 4px solid #3498db; border-radius: 50%; margin: 0 auto; animation: spin 1s linear infinite;"></div>
            </div>
        </div>

        <style>
            @keyframes spin {
                0% {
                    transform: rotate(0deg);
                }

                100% {
                    transform: rotate(360deg);
                }
            }
        </style>

        <!-- Toolbar -->
        <div class="toolbar">
            <button class="btn" onclick="loadData()">تحميل البيانات</button>
            <button class="btn" onclick="testWebMethod()">اختبار الاتصال</button>
            <input type="text" id="searchInput" class="search-box" placeholder="بحث..." />
            <button class="btn" onclick="searchData()">بحث</button>

            <div style="margin-top: 10px;">
                <label>النظام: </label>
                <input type="text" id="systemInput" value="01" style="width: 50px;" />
                <label>الفرع: </label>
                <input type="text" id="branchInput" value="01000" style="width: 100px;" />
            </div>
        </div>

        <!-- Data Display Area -->
        <div id="dataContainer">
            <h3>بيانات الوثائق</h3>
            <div id="resultInfo"></div>

            <table id="dataTable" class="simple-table">
                <thead>
                    <tr>
                        <th>رقم الطلب</th>
                        <th>المؤمن له</th>
                        <th>رقم الوثيقة</th>
                        <th>صافي القسط</th>
                        <th>إجمالي القسط</th>
                        <th>تاريخ الإصدار</th>
                        <th>حالة السداد</th>
                    </tr>
                </thead>
                <tbody id="tableBody">
                    <!-- Data will be loaded here -->
                </tbody>
            </table>

            <div style="margin-top: 20px;">
                <button class="btn" onclick="prevPage()" id="prevBtn">السابق</button>
                <span id="pageInfo">الصفحة 1</span>
                <button class="btn" onclick="nextPage()" id="nextBtn">التالي</button>
                <select id="pageSizeSelect" onchange="changePageSize()">
                    <option value="5">5 صفوف</option>
                    <option value="10" selected>10 صفوف</option>
                    <option value="20">20 صفوف</option>
                    <option value="50">50 صفوف</option>
                </select>
            </div>
        </div>

        <!-- Debug Area -->
        <div style="margin-top: 20px; padding: 10px; background: #f8f9fa; border: 1px solid #ddd; display: none;" id="debugArea">
            <h4>معلومات التصحيح:</h4>
            <pre id="debugOutput"></pre>
        </div>
    </div>

    <!-- Simple JavaScript -->
    <script>
        // Global variables
        let currentPage = 1;
        let pageSize = 10;
        let totalRecords = 0;
        let totalPages = 1;
        let currentData = [];

        // Show/hide loading
        function showLoading(show) {
            document.getElementById('loadingDiv').style.display = show ? 'block' : 'none';
        }

        // Format number
        function formatNumber(num) {
            return parseFloat(num || 0).toFixed(3);
        }

        // Format date
        function formatDate(dateString) {
            if (!dateString) return '-';
            return dateString.split('T')[0];
        }

        // Display data in table
        function displayData(data) {
            const tbody = document.getElementById('tableBody');
            tbody.innerHTML = '';

            if (!data || data.length === 0) {
                tbody.innerHTML = '<tr><td colspan="7" style="text-align: center;">لا توجد بيانات</td></tr>';
                return;
            }

            data.forEach(function (item) {
                const row = document.createElement('tr');

                row.innerHTML = `
                    <td>${item.OrderNo || '-'}</td>
                    <td>${item.CustName || '-'}</td>
                    <td>${item.PolNo || '-'}</td>
                    <td style="text-align: left;">${formatNumber(item.NETPRM)}</td>
                    <td style="text-align: left;">${formatNumber(item.TOTPRM)}</td>
                    <td>${formatDate(item.IssuDate)}</td>
                    <td>${getPaymentStatus(item.Payment)}</td>
                `;

                tbody.appendChild(row);
            });

            // Update page info
            document.getElementById('pageInfo').textContent =
                `الصفحة ${currentPage} من ${totalPages} (إجمالي السجلات: ${totalRecords})`;

            // Update button states
            document.getElementById('prevBtn').disabled = currentPage <= 1;
            document.getElementById('nextBtn').disabled = currentPage >= totalPages;
        }

        // Get payment status text
        function getPaymentStatus(status) {
            switch (status) {
                case 'Paid': return '✅ مدفوع';
                case 'Pending': return '⏳ قيد الانتظار';
                case 'Credit': return '💰 على الحساب';
                default: return '❓ غير محدد';
            }
        }

        // Load data from server
        function loadData() {
            showLoading(true);

            // Get system and branch values
            const system = document.getElementById('systemInput').value || $('#hdnSys').val();
            const branch = document.getElementById('branchInput').value || $('#hdnBranch').val();
            const searchText = document.getElementById('searchInput').value;

            console.log('Loading data with:', { system, branch, searchText, currentPage, pageSize });

            // Make AJAX call
            $.ajax({
                url: 'Default_Modern.aspx/GetSimpleData',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    system: system,
                    branch: branch,
                    searchText: searchText,
                    pageNumber: currentPage,
                    pageSize: pageSize
                }),
                success: function (response) {
                    console.log('Server response:', response);

                    if (response && response.d) {
                        if (response.d.success) {
                            // Display the data
                            displayData(response.d.data);
                            totalRecords = response.d.totalRecords || 0;
                            totalPages = Math.ceil(totalRecords / pageSize);

                            // Show success message
                            document.getElementById('resultInfo').innerHTML =
                                `<div style="color: green; padding: 5px;">✅ تم تحميل ${response.d.data.length} سجل</div>`;
                        } else {
                            // Show error
                            document.getElementById('resultInfo').innerHTML =
                                `<div style="color: red; padding: 5px;">❌ خطأ: ${response.d.message || 'حدث خطأ'}</div>`;
                        }
                    }
                },
                error: function (xhr, status, error) {
                    console.error('AJAX error:', error);
                    console.error('Response text:', xhr.responseText);

                    document.getElementById('resultInfo').innerHTML =
                        `<div style="color: red; padding: 5px;">❌ خطأ في الاتصال: ${error}</div>`;

                    // Show debug info
                    document.getElementById('debugArea').style.display = 'block';
                    document.getElementById('debugOutput').textContent =
                        'Status: ' + status + '\n' +
                        'Error: ' + error + '\n' +
                        'Response: ' + xhr.responseText;
                },
                complete: function () {
                    showLoading(false);
                }
            });
        }

        // Test WebMethod
        function testWebMethod() {
            showLoading(true);

            $.ajax({
                url: 'Default_Modern.aspx/TestConnection',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{}',
                success: function (response) {
                    console.log('Test response:', response);

                    if (response && response.d) {
                        alert('✅ الاتصال ناجح: ' + (response.d.message || 'تم الاتصال بالخادم'));
                    } else {
                        alert('⚠️ استجابة غير متوقعة');
                    }
                },
                error: function (xhr) {
                    alert('❌ فشل الاتصال: ' + xhr.statusText);
                    console.log('Response:', xhr.responseText);
                },
                complete: function () {
                    showLoading(false);
                }
            });
        }

        // Search function
        function searchData() {
            currentPage = 1;
            loadData();
        }

        // Pagination functions
        function prevPage() {
            if (currentPage > 1) {
                currentPage--;
                loadData();
            }
        }

        function nextPage() {
            if (currentPage < totalPages) {
                currentPage++;
                loadData();
            }
        }

        function changePageSize() {
            pageSize = parseInt(document.getElementById('pageSizeSelect').value);
            currentPage = 1;
            loadData();
        }

        // Initialize page
        $(document).ready(function () {
            console.log('Page loaded');

            // Check if jQuery is loaded
            if (typeof jQuery == 'undefined') {
                alert('⚠️ jQuery غير محمل. سيتم تحميله الآن.');
                loadJQuery();
            } else {
                // Load data after 1 second
                setTimeout(loadData, 1000);
            }

            // Handle Enter key in search
            document.getElementById('searchInput').addEventListener('keypress', function (e) {
                if (e.key === 'Enter') {
                    searchData();
                }
            });
        });

        // Load jQuery if not present
        function loadJQuery() {
            var script = document.createElement('script');
            script.src = 'https://code.jquery.com/jquery-3.6.0.min.js';
            script.onload = function () {
                console.log('jQuery loaded successfully');
                loadData();
            };
            script.onerror = function () {
                alert('❌ فشل تحميل jQuery. الرجاء تحميل الصفحة مرة أخرى.');
            };
            document.head.appendChild(script);
        }

        function SetSystem(sys, br) {
            // Check if modern page exists and has the function
            if (typeof window.SetSystem === 'function') {
                window.SetSystem(sys, br);
            } else {
                alert(sys);
                // Fallback to old method
                if (typeof sys !== 'undefined') {
                    Sys.SetValue(sys);
                    Branch.SetValue(br);
                    //mainGridTable.sh(true);
                    //cmbReports.SetVisible(true);

                    //MainGrid.PerformCallback();
                    ////MainGrid.PerformCallback(sys);

                    //cmbReports.PerformCallback();
                }
            }
        }
    </script>
</asp:Content>