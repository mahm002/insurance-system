<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PolicyControl.ascx.vb" Inherits="PolicyControl" %>

<!-- Bootstrap & jQuery -->

<%--<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />--%>
<link href="../../Content/bootstrap.min.css" rel="stylesheet" />
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

<!-- Toastr CSS -->
<%--<link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" rel="stylesheet" />--%>
<link href="../../Content/toastr.min.css" rel="stylesheet" />
<%--<link href="../Styles/toastr.min.css" rel="stylesheet" />--%>

<!-- Toastr JS -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
<%--<script src="../scripts/toastr.min.js"></script>--%>

<!-- Your Scripts -->
<%--<script src="../../scripts/Scripts.js"></script>--%>
<script src="../../scripts/jquery-latest.min.js"></script>
<%--<script src="../../scripts/jquery-3.7.1.min.js"></script>--%>
<script src="../../scripts/jquery-3.7.1.min.js"></script>

<%--<link href="../../Content/AdaptiveGridLayout.css" rel="stylesheet" />
<script src="../../Scripts/AdaptiveGridLayout.js" type="text/javascript"></script>--%>

<script type="text/javascript">
    // Toastr configuration
    $(document).ready(function () {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-left",
            "preventDuplicates": true,
            "onclick": null,
            "showDuration": "4000",
            "hideDuration": "4000",
            "timeOut": "5000",
            "extendedTimeOut": "4000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut",
            "rtl": true // Right-to-left for Arabic
        };
    });
    // Add CSS for results container
    $(document).ready(function () {
        $('head').append('<style>' +
            '#autocomplete-results { ' +
            'position: fixed !important; ' +  /* Changed from absolute to fixed */
            'z-index: 9999 !important; ' +    /* Increased z-index */
            'background: white; ' +
            'border: 1px solid #007bff; ' +
            'border-radius: 5px; ' +
            'box-shadow: 0 4px 12px rgba(0,0,0,0.15); ' +
            'width: auto; ' +                 /* Changed from 100% to auto */
            'min-width: 300px; ' +            /* Minimum width */
            'max-width: 500px; ' +            /* Maximum width */
            'display: none; ' +
            'max-height: none !important; ' + /* Remove height restrictions */
            'overflow: visible !important; ' +/* Remove overflow */
            '}' +
            '.customer-item { ' +
            'cursor: pointer; ' +
            'padding: 12px 15px; ' +
            'border-bottom: 1px solid #f8f9fa; ' +
            'transition: all 0.2s ease; ' +
            'margin: 0; ' +
            '}' +
            '.customer-item:hover { ' +
            'background-color: #d5e3e3; ' +
            'color: Grey; ' +
            'transform: translateX(5px); ' +
            '}' +
            '.customer-item:last-child { ' +
            'border-bottom: none; ' +
            'border-radius: 0 0 5px 5px; ' +
            '}' +
            '.customer-item:first-child { ' +
            'border-radius: 5px 5px 0 0; ' +
            '}' +
            '.customer-name { ' +
            'font-weight: bold; ' +
            'font-size: 18px; ' +
            'display: block; ' +
            '}' +
            '.customer-details { ' +
            'font-size: 16px; ' +
            'color: #6c757d; ' +
            'display: block; ' +
            'margin-top: 4px; ' +
            '}' +
            '.customer-item:hover .customer-details { ' +
            'color: red); ' +
            '}' +
            '.add-new-item { ' +
            'background: linear-gradient(to right, darkblue , blue); ' +
            'color: white; ' +
            'font-weight: bold; ' +
            'text-align: center; ' +
            '}' +
            '.add-new-item:hover { ' +
            'background: linear-gradient(135deg, #218838, #1e9e8a); ' +
            'color: white; ' +
            '}' +
            '</style>');
    });
    // Update results position on scroll and resize
    $(window).on('scroll resize', function () {
        var resultsContainer = $('#autocomplete-results');
        if (resultsContainer.is(':visible')) {
            var customerInput = clientTxtCustomerSearch;
            var inputElement = customerInput.GetInputElement();
            positionResultsContainer(resultsContainer, inputElement);
        }
    });
    // Also update the hide function to be more specific
    $(document).on('click', function (e) {
        if (!$(e.target).closest('.customer-item').length &&
            !$(e.target).closest('#<%= txtCustomerSearch.ClientID %>_I').length &&
            !$(e.target).is('#<%= txtCustomerSearch.ClientID %>_I')) {
            $('#autocomplete-results').hide();

        }
    });
    // Hide results when clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('.customer-item').length &&
            !$(e.target).is('#<%= txtCustomerSearch.ClientID %>_I')) {
            $('#autocomplete-results').hide();

        }
    });
    //================================================
    var searchTimeout = null;

    function onCustomerSearch(s, e) {
        clearTimeout(searchTimeout);
        var searchTerm = s.GetText().trim();
        var resultsContainer = $('#autocomplete-results');

        if (searchTerm.length > 2) {
            searchTimeout = setTimeout(function () {
                clientCallbackCustomer.PerformCallback("SEARCH:" + searchTerm);
            }, 500);
        } else {
            resultsContainer.hide().html('');
        }
    }

    function onCallbackComplete(s, e) {
        var resultsContainer = $('#autocomplete-results');
        var customerInput = clientTxtCustomerSearch;
        var inputElement = customerInput.GetInputElement();

        //console.log("Callback result:", e.result);

        // Check if result is a number (new customer ID)
        if (!isNaN(e.result) && e.result !== "") {
            var newId = parseInt(e.result);
            $('#<%= CustNo.ClientID %>').val(newId);
            resultsContainer.hide().html('');
            showSuccess('تم إضافة الزبون الجديد "' + customerInput.GetText() + '" برقم: ' + newId);
            CustNameE.SetText(customerInput.GetText());
            TelNo.SetText("+218000000000");
            Address.SetText("ليبيا");

            // ✅ SET INTERVAL TO 1 FOR NEW CUSTOMER
            //Intv.GetValue()
            if (Sy == 'MC' || Sy == 'MA' || Sy == 'MB' || Sy == 'CR' || Sy == 'ER') {
                //cbp.PerformCallback('ImportExcel');
                //Callbackflag = true;
            }
            else {
                Intv.SetValue(1);
            }

            // Optional: Trigger calculation

            // Optional: Trigger calculation after setting interval
            if (ASPxClientEdit.ValidateGroup('Data')) {
                if (cbp.InCallback()) return;
                cbp.PerformCallback('Calc');
            }
            //if (ASPxClientEdit.ValidateGroup('Data')) {
            //    setTimeout(function () {
            //        if (cbp.InCallback()) return;
            //        cbp.PerformCallback('Calc');
            //    }, 500);
            //}
            return;
        }

        // Handle search results
        try {
            if (!e.result) {
                resultsContainer.hide().html('');
                return;
            }

            var data = JSON.parse(e.result);
            resultsContainer.html('');

            if (data && data.length > 0) {

                // Show info toast with results count
                showInfo('تم العثور على ' + data.length + ' نتيجة للبحث، إذا كانت نتيجة البحث مطابقة للمطلوب قم باختيارها ');
                // Display ALL results without limiting to 5
                $.each(data, function (index, customer) {
                    var customerHtml = '<div class="customer-item">' +
                        '<span class="customer-name">' + customer.CustName + '</span>' +
                        '<span class="customer-details">';

                    // Add details if available
                    if (customer.TelNo && customer.TelNo !== '') {
                        customerHtml += '📞 ' + customer.TelNo + ' | ';
                    }
                    if (customer.CustNo && customer.CustNo !== '') {
                        customerHtml += '🆔 ' + customer.CustNo;
                    }

                    customerHtml += '</span></div>';

                    var resultItem = $(customerHtml)
                        .data('customer', customer)
                        .on('click', function (ev) {
                            ev.preventDefault();
                            ev.stopPropagation();
                            var cust = $(this).data('customer');
                            selectCustomer(cust);
                        });
                    resultsContainer.append(resultItem);
                });

                // Add "Add new" option at the end
                var searchTerm = customerInput.GetText();
                var addNewItem = $('<div class="customer-item add-new-item">')
                    .html('➕ إضافة زبون جديد: "' + searchTerm + '"')
                    .on('click', function (ev) {
                        ev.preventDefault();
                        ev.stopPropagation();
                        addNewCustomer(searchTerm);
                    });
                resultsContainer.append(addNewItem);

                // Position and show results
                positionResultsContainer(resultsContainer, inputElement);
                resultsContainer.show();

            } else {
                showWarning('لم يتم العثور على نتائج للبحث');
                // No results - show only add option
                var searchTerm = customerInput.GetText();
                var noResultsItem = $('<div class="customer-item text-center text-muted">')
                    .html('🔍 لم يتم العثور على نتائج')
                    .css({
                        'font-style': 'italic',
                        'background': '#fff3cd'
                    });
                resultsContainer.append(noResultsItem);

                var addNewItem = $('<div class="customer-item add-new-item">')
                    .html('➕ إضافة زبون جديد: "' + searchTerm + '"')
                    .on('click', function (ev) {
                        ev.preventDefault();
                        ev.stopPropagation();
                        addNewCustomer(searchTerm);
                    });
                resultsContainer.append(addNewItem);

                // Position and show results
                positionResultsContainer(resultsContainer, inputElement);
                resultsContainer.show();
            }
        } catch (err) {
            //console.error("Error parsing JSON: " + err);
            showError('حدث خطأ في معالجة البيانات');
            resultsContainer.hide().html('');
        }
    }

    // New function to position results container outside the round panel
    function positionResultsContainer(container, inputElement) {
        var inputRect = inputElement.getBoundingClientRect();
        var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        var scrollLeft = window.pageXOffset || document.documentElement.scrollLeft;

        // Position below the input field
        container.css({
            'position': 'fixed',
            'top': (inputRect.bottom + scrollTop) + 'px',
            'left': (inputRect.left + scrollLeft) + 'px',
            'width': Math.max(inputRect.width, 300) + 'px' // Minimum width of 300px
        });
    }

    // دوال محسنة للإشعارات مع Toastr
    function showSuccess(message) {
        toastr.success(message, 'نجاح', {
            timeOut: 5000,
            positionClass: 'toast-top-left',
            rtl: true
        });
    }

    function showError(message) {
        toastr.error(message, 'خطأ', {
            timeOut: 5000,
            positionClass: 'toast-top-left',
            rtl: true
        });
    }

    function showWarning(message) {
        toastr.warning(message, 'تنبيه', {
            timeOut: 5000,
            positionClass: 'toast-top-left',
            rtl: true
        });
    }

    function showInfo(message) {
        toastr.info(message, 'معلومة', {
            timeOut: 5000,
            positionClass: 'toast-top-left',
            rtl: true
        });
    }

    // دالة محسنة لاختيار الزبون
    function selectCustomer(customer) {
        // Set the customer name in search box
        clientTxtCustomerSearch.SetText(customer.CustName);

        // Set customer ID in hidden field
        $('#<%= CustNo.ClientID %>').val(customer.CustNo);
        CustNameE.SetText(customer.CustNameE);
        TelNo.SetText(customer.TelNo);
        Address.SetText(customer.Address);

        if (Sy == 'MC' || Sy == 'MA' || Sy == 'MB' || Sy == 'CR' || Sy == 'ER') {
            //cbp.PerformCallback('ImportExcel');
            //Callbackflag = true;
        }
        else {
            Intv.SetValue(1);
        }
        // Hide results
        $('#autocomplete-results').hide();

        // Show success message with toast
        var successMsg = 'تم اختيار الزبون: ' + customer.CustName;
        if (customer.CustNo) {
            successMsg += ' (رقم: ' + customer.CustNo + ')';
        }
        showSuccess(successMsg);

        // Load additional customer data if available
        if (customer.TelNo || customer.Address || customer.CustNameE) {
            loadCustomerDetails(customer);
        }

        // Optional: Trigger calculation after setting interval
        if (ASPxClientEdit.ValidateGroup('Data')) {
            if (cbp.InCallback()) return;
            cbp.PerformCallback('Calc');
        }
    }

    // دالة محسنة لتحميل تفاصيل الزبون
    function loadCustomerDetails(customer) {
        // console.log('تفاصيل الزبون:', customer);
        // هنا يمكنك تعبئة الحقول الأخرى في النموذج إذا كنت تريد
        // مثلاً:
        if (customer.CustNameE) $('#CustNameE').val(customer.CustNameE);
        if (customer.TelNo) $('#phoneField').val(customer.TelNo);
        if (customer.Address) $('#addressField').val(customer.Address);
    }
    function addNewCustomer(customerName) {
        if (customerName && customerName.length > 0) {
            showInfo('جاري إضافة الزبون الجديد: ' + customerName);

            // You can use a confirmation toast or keep the native confirm
            if (confirm('هل تريد إضافة الزبون "' + customerName + '" كزبون جديد؟')) {
                clientCallbackCustomer.PerformCallback("INSERT:" + customerName);
            }
        }
    }
    // Customer ID validation function
    function validateCustomerId() {
        var custNo = $('#<%= CustNo.ClientID %>').val();
        var custNoHidden = $('#<%= CustNo.ClientID %>');

        // Debug logging
        //console.log("Customer ID validation check:", custNo);
        //console.log("Hidden field element:", custNoHidden);

        if (!custNo || custNo === "" || custNo === "0" || custNo === 0) {
            showError('❗ الرجاء اختيار أو إضاقة زبون من القائمة قبل المتابعة');
            return false;
        }

        // Additional validation for numeric value
        if (isNaN(custNo) || parseInt(custNo) <= 0) {
            showError('❗ رقم الزبون غير صالح. الرجاء اختيار أو إضاقة زبون من القائمة');
            return false;
        }

        return true;
    }
    //================================================
    $('body').on('keydown', 'input, select', function (e) {
        //debugger;
        if (e.key === "Enter") {
            e.preventDefault();
            var self = $(this), form = self.parents('form:eq(0)'), focusable, next;
            focusable = form.find('input,a,select,button,textarea').filter(':visible:enabled');
            //focusable1 = form.find('input,a,select,button,textarea').filter(':ClientEnabled');
            next = focusable.eq(focusable.index(this) + 1);
            if (next.length) {
                // debugger;
                next.focus();
                next.select();
            } else {
                // debugger;
                btnShow.DoClick();
                $('input:text:first').focus();
            }
            return false;
        }
    });

    function OnCommChanged(e, s) {
        //alert(Broker.GetValue());

        if (Broker.GetValue() == 0 || Broker.GetValue() == null || NetPRM.GetValue() <= 34) {
            //    alert(Broker.GetValue());
            Commision.SetValue(0);
        }
        //alert(selectedValue);
    }
    //var Callbackflag;
    var previous;
    function DoProcessEnterKey(htmlEvent, editName) {
        if (htmlEvent.keyCode == 13) {
            ASPxClientUtils.PreventEventAndBubble(htmlEvent);
            if (editName) {
                ASPxClientControl.GetControlCollection().GetByName(editName).SetFocus();
            } else {
                btnShow.DoClick();
            }
        }
    }
    function ImportPopup() {

        var Order = OrderNo.GetValue().replace(',', '');
        var Pol = PolNo.GetValue().replace(',', '');
        var End = EndNo.GetValue().replace(',', '');
        var Load = LoadNo.GetValue().replace(',', '');
        var Syst = getParameterByName('Sys');
        var Br = OrderNo.substring(0, 4);
        //alert(Br)

    }
    function getParameterByName(na) {
        var match = RegExp('[?&]' + na + '=([^&]*)').exec(window.location.search);
        return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
    }
    function PolicyIssu(Path) {
        var viewportwidth;
        var viewportheight;
        if (typeof window.innerWidth != 'undefined') {
            viewportwidth = window.innerWidth;
            viewportheight = window.innerHeight
        }
        else if (typeof document.documentElement != 'undefined'
            && typeof document.documentElement.clientWidth != 'undefined' && document.documentElement.clientWidth != 0) {
            viewportwidth = document.documentElement.clientWidth;
            viewportheight = document.documentElement.clientHeight
        }
        popup.SetSize(viewportwidth, viewportheight);
        popup.SetContentUrl(Path);
        popup.Show();
    }

    function LocalExRateCall(s, e) {
        if (cbp.InCallback()) return;
        cbp.PerformCallback("LocalExRate");
        //Callbackflag = true;
    }

    function SelectAndClosePopup() {
        popup.Hide();
        // __doPostBack('', "RefreshPage");
        //parent.location.reload(true);

    }
    //function getParameterByName(name) {
    //    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
    //    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
    //}
    function OnCloseUp(s, e) {
        // __doPostBack('', "RefreshPage");
    }
    function Validate(s, e) {
        //debugger;
        if (!validateCustomerId()) {
            return false; // Stop execution if customer is invalid
        }
        var Syst = getParameterByName('Sys');
        var Order = document.getElementById('<%=OrderNo.ClientID%>').value;
        //alert(Order);
        if (ASPxClientEdit.ValidateGroup('Data') && (Syst !== '02' && Syst !== '03' && Syst !== 'FG' && Syst !== 'CM' && Syst !== 'PA' && Syst !== 'PI' && Syst !== 'EL' && Syst !== 'CS' && Syst !== 'FR' && Syst !== 'FB' && Syst !== 'HL' && Syst !== '09' && Syst !== '07')) {
            if (ASPxClientEdit.ValidateGroup('Data')) {
                if (cbp.InCallback()) return;
                cbp.PerformCallback('Calc');
                //Callbackflag = true;
            }
        }
        else {
            if ((Syst == '02' || Syst == '03' || Syst == 'FG' || Syst == 'CM' || Syst == 'PA' || Syst == 'EL' || Syst == 'CS' || Syst == 'FR' || Syst == 'FB' || Syst == 'HL' || Syst == '09' || Syst == '07')) {
                cbp.PerformCallback('ImportExcel');
                //Callbackflag = true;
            }
            else {
                if (ASPxClientEdit.ValidateGroup('Data')) {
                    if (cbp.InCallback()) return;
                    cbp.PerformCallback('Calc');
                    //Callbackflag = true;
                }
            }

        }
    }
    function ValidateS(s, e) {
        //debugger;
        if (!validateCustomerId()) {
            return false; // Stop execution if customer is invalid
        }
        if (ASPxClientEdit.ValidateGroup('Data')) {
            if (cbp.InCallback()) return;
            cbp.PerformCallback('Calc');
            // Callbackflag = true;
        }

    }
    function Endorsment(s, e) {
        var end = parseInt(EndNo.GetValue());
        OrderNo.SetValue(0);
        //alert(parseInt(EndNo.GetValue()));
        //EndNo.SetValue(end + 1);
        s.SetEnabled(false);
        //Endorsment.ClientVisible = false;
        if (cbp.InCallback()) return;
        cbp.PerformCallback('Endorsment');
        //Callbackflag = true;
        DataGrid.Refresh();
    }
    function ForLoss(s, e) {
        var end = parseInt(EndNo.GetValue());
        OrderNo.SetValue(0);
        s.SetEnabled(false);
        //Endorsment.ClientVisible = false;
        if (cbp.InCallback()) return;
        cbp.PerformCallback('ForLoss');
        //Callbackflag = true;
        DataGrid.Refresh();
    }
    function ChangeOwner(s, e) {
        var end = parseInt(EndNo.GetValue());
        OrderNo.SetValue(0);
        s.SetEnabled(false);
        //Endorsment.ClientVisible = false;
        cbp.PerformCallback('ChangeOwner');
        //Callbackflag = true;
        DataGrid.Refresh();
    }
    function Loads(s, e) {
        //debugger;
        var load = parseInt(LoadNo.GetValue());
        OrderNo.SetValue(0);
        //alert(parseInt(LoadNo.GetValue()));
        //LoadNo.SetValue(load + 1);
        //Shipment.ClientVisible = false;
        cbp.PerformCallback('Shipment');
        //Callbackflag = true;

    }
    //function OnSelectedIndexChanged(s, e) {
    //    cbpP.PerformCallback();
    //    DAdd();
    //}
    var Sy = getParameterByName('Sys');
    function DAdd(s, e) {
        //debugger;
        // cbpp.PerformCallback();
        //Interval.SetValue(Interval.GetValue());
        //alert(s);

        //var amnt = Interval.GetValue();
        var msr = Measure.GetValue();
        var end = parseInt(EndNo.GetValue());

        //if (Measure.GetValue() == 1 && Sy == "01") {

        //    Interval.SetMaxValue(1095);
        //    Interval.SetMinValue(15);
        //    }

        //else {
        //    switch (Sy) {
        //        case "01":
        //        //Interval.SetMinValue(1);
        //        //Interval.SetMaxValue(3);
        //    }

        //}

        //debugger;
        if (end == 0) {
            //debugger;
            //alert(getParameterByName('EndType'));

            CoverTo.SetValue(dateAdd(CoverFrom.GetDate(), msr));
        }
        else {
            if (getParameterByName('EndType') !== null) {
                CoverTo.SetValue(dateAdd(CoverFrom.GetDate(), msr));
            }
            else {

            }

        }
        //CoverTo.SetValue(dateAdd(CoverFrom.GetDate(), msr));
        if (ASPxClientEdit.ValidateGroup('Data')) {
            if (cbp.InCallback()) return;
            cbp.PerformCallback('Calc');
            //Callbackflag = true;
        }
        // alert(Interval.GetValue());
        //cbpp.PerformCallback();
        //break;
        //OriginalEndDate.setDate(dateAdd(OriginalBeginDate.GetDate(), OriginalTermYears.value()));
    }
    function dateAdd(dateold, ymd) {
        //debugger;
        //alert(Interval.GetValue());
        //alert(ymd);

        //alert(Sy);
        //alert(dateold);
        //alert(ymd);

        var td = new Date();
        switch (ymd) {
            //Years
            case 3:
                // debugger;
                var d = new Date(dateold);
                var year = d.getFullYear();
                var month = d.getMonth();
                var day = d.getDate() - 1;
                var result = new Date(year + Intv.GetValue(), month, day);
                return result;
            //Months
            case 2:
                var d = new Date(dateold);
                var year = d.getFullYear();
                var month = d.getMonth();
                var day = d.getDate() - 1;
                var result = new Date(year, month + Intv.GetValue(), day);
                return result;
            //Days
            case 1:
                //&& Sy!='OR'
                var d = new Date(dateold);
                var year = d.getFullYear();
                var month = d.getMonth();
                if (d.getMonth() == td.getMonth() && d.getFullYear() == td.getFullYear() && d.getDay() == td.getDay()) {
                    var day = d.getDate();
                }
                else {
                    var day = d.getDate() - 1;
                }

                var result = new Date(year, month, day + Intv.GetValue());
                //alert(Interval.GetValue());
                return result;
        }
        if (cbp.InCallback()) return;
        cbp.PerformCallback('Calc');
    }

    function StartFiltering(s, e) {
        //CustNo.SetText(s.GetValue());
        //CustNo.GetGridView.SetValue(s.GetValue());

    }
</script>

<style type="text/css">
    .auto-style1 {
        text-align: left;
    }

    .auto-style2 {
        text-align: center;
        width: 79px;
    }

    .auto-style3 {
        text-align: left;
        width: 90px;
    }
</style>
<table style="width: 100%;">
    <tr>
        <td class="auto-style1">
            <dx:ASPxRoundPanel ID="PolicyPanel" ClientInstanceName="PolicyPanel" runat="server" RightToLeft="True"
                LoadContentViaCallback="true" HeaderText="بيانات الزبون ونوع التغطية" Width="100%" Height="100%">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td class="dxeICC">رقم الوثيقة</td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="PolNo" runat="server" ClientEnabled="False" CssClass="1"
                                        ClientInstanceName="PolNo" Width="100%" RightToLeft="True">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="OldPolicy" runat="server" ClientVisible="False" CssClass="1"
                                        ClientInstanceName="OldPolicy" Width="100%" RightToLeft="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1" style="text-align: left">
                                    <%--        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>">
                                      <%--  <InsertParameters>
                                            <asp:Parameter Name="CustName" Type="String" />
                                            <asp:Parameter DefaultValue="/" Name="CustNameE" Type="String" />
                                            <asp:Parameter DefaultValue="+218" Name="TelNo" Type="String" />
                                            <asp:Parameter DefaultValue="ليبيا" Name="Address" Type="String" />
                                        </InsertParameters>
                                    </asp:SqlDataSource>--%>
                                    <asp:SqlDataSource ID="Customers" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        InsertCommand="NewCustomer" InsertCommandType="StoredProcedure"
                                        SelectCommand="SELECT CustNo, rtrim([CustName]) As CustName, rtrim([CustNameE]) As CustNameE, rtrim(TelNo) as TelNo, rtrim([CustName]) +'/'+ rtrim([CustNameE]) +'/'+ rtrim(TelNo) As Prief, [Email], [Address] ,[SpecialCase], [AccNo] FROM [CustomerFile] Order BY CustNo desc,REVERSE(CustName)"
                                        UpdateCommand="UpdateCustomer" UpdateCommandType="StoredProcedure">

                                        <InsertParameters>
                                            <asp:Parameter Name="CustName" Type="String" />
                                            <asp:Parameter DefaultValue="/" Name="CustNameE" Type="String" />
                                            <asp:Parameter DefaultValue="00218" Name="TelNo" Type="String" />
                                            <asp:Parameter DefaultValue="ليبيا" Name="Address" Type="String" />
                                            <asp:SessionParameter DefaultValue="/" SessionField="User" Name="User" Type="String" />
                                        </InsertParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="CustNo" Type="Int64" />
                                            <asp:Parameter Name="CustName" Type="String" />
                                            <asp:Parameter DefaultValue="/" Name="CustNameE" Type="String" />
                                            <asp:Parameter DefaultValue="+218" Name="TelNo" Type="String" />
                                            <asp:Parameter DefaultValue="ليبيا" Name="Address" Type="String" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                    <dx:ASPxTextBox ID="OrderNo" runat="server" ClientEnabled="False"
                                        ClientInstanceName="OrderNo" ClientVisible="false" CssClass="1"
                                        RightToLeft="True" Text="0"
                                        Width="150px">
                                        <ValidationSettings SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                    <asp:SqlDataSource ID="Covers" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="SELECT [CoverNo], rtrim([CoverName]) As CoverName FROM [Covers] WHERE ([SubSystem] = @SubSystem)">
                                        <SelectParameters>
                                            <asp:QueryStringParameter DefaultValue="01" Name="SubSystem" QueryStringField="Sys" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td class="dxeCaptionHACSys">
                                    <%-- OnClick="Endorsment_Click"--%>
                                    <dx:ASPxButton ID="ForLoss" runat="server" AutoPostBack="False"
                                        ClientInstanceName="ForLoss" Text="ملـحق بدل فاقد" ClientVisible="false">
                                        <ClientSideEvents Click="ForLoss" />
                                    </dx:ASPxButton>
                                    <asp:HiddenField ID="CustNo" runat="server" />
                                </td>
                                <td class="auto-style2">
                                    <dx:ASPxButton ID="Endorsment" runat="server" AutoPostBack="False" ClientInstanceName="Endorsment" ClientVisible="False" Text="ملحق" Width="22px">
                                        <ClientSideEvents Click="Endorsment" />
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="ChangeOwner" runat="server" AutoPostBack="False" ClientInstanceName="ChangeOwner"
                                        ClientVisible="False" Text="ملحق تغيير ملكية">
                                        <ClientSideEvents Click="ChangeOwner" />
                                    </dx:ASPxButton>
                                </td>
                                <td class="auto-style3">رقم الملحق</td>
                                <td class="auto-style1">
                                    <dx:ASPxCallback ID="CallbackCustomer" runat="server" ClientInstanceName="clientCallbackCustomer" OnCallback="CallbackCustomer_Callback">
                                        <ClientSideEvents CallbackComplete="onCallbackComplete" />
                                    </dx:ASPxCallback>
                                    <dx:ASPxTextBox ID="EndNo" runat="server" ClientEnabled="False" ClientInstanceName="EndNo" CssClass="3" Text="0" Width="30px">
                                        <ValidationSettings SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">&nbsp;</td>
                                <td class="dxeICC">
                                    <dx:ASPxButton ID="Shipment" runat="server" AutoPostBack="False" ClientInstanceName="Shipment" ClientVisible="False" Text="شهادة تأمين">
                                        <ClientSideEvents Click="Loads" />
                                    </dx:ASPxButton>
                                    رقم الشهادة</td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="LoadNo" runat="server" ClientEnabled="False" ClientInstanceName="LoadNo" CssClass="3" Text="0" Width="30px">
                                        <ValidationSettings SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">الزبــــــون</td>
                                <td colspan="9" class="auto-style1">
                                    <div style="position: relative; margin-bottom: 10px;">
                                        <%--<RegularExpression ValidationExpression="^[A-Za-z0-9ا.,&-يءإؤةأئ /-\\-]+" />--%>

                                        <dx:ASPxTextBox ID="txtCustomerSearch" runat="server"
                                            ClientInstanceName="clientTxtCustomerSearch"
                                            Width="100%"
                                            NullText="🔍 اكتب اسم الزبون أو رقم الهاتف للبحث (3 أحرف على الأقل)...">
                                            <ClientSideEvents KeyUp="onCustomerSearch" />
                                            <ValidationSettings Display="Dynamic">

                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>

                                        <!-- Customer number hidden field -->

                                        <!-- Help message -->
                                        <%--<div style="font-size: 15px; color: #6c757d; margin-top: 5px;">
                                            💡 اكتب 3 أحرف على الأقل للبحث - اضغط على النتيجة لاختيارها
                                        </div>--%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-start" colspan="10">
                                    <dx:ASPxTextBox ID="CustNameE" runat="server" Caption="الاسم الإنجليزي" ClientInstanceName="CustNameE" CssClass="10" Width="100%">
                                        <ValidationSettings Display="Dynamic">
                                            <RegularExpression ErrorText="مطلوب" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-start" colspan="2">الهاتف</td>
                                <td class="text-start" colspan="2">
                                    <dx:ASPxTextBox ID="TelNo" runat="server" ClientInstanceName="TelNo" CssClass="10" NullText="للاستفادة من خدمة الرسائل القصيرة" RightToLeft="True" Width="170px">
                                        <MaskSettings AllowEscapingInEnums="True" Mask="+\2\1\8000000000" />
                                        <ValidationSettings Display="Dynamic">
                                            <RegularExpression ErrorText="مطلوب" ValidationExpression="^\+[1-9]{1}[0-9]{3,14}$" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1" colspan="6">
                                    <dx:ASPxTextBox ID="Address" runat="server" Caption="العنوان" ClientInstanceName="Address" CssClass="10" Width="100%">
                                        <ValidationSettings Display="Dynamic">
                                            <RegularExpression ErrorText="مطلوب" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al" colspan="10">
                                    <dx:ASPxGridLookup ID="OwnNo" runat="server" AutoGenerateColumns="False" AutoResizeWithContainer="True" Caption="إصدار لصالح" ClientInstanceName="OwnNo" DataSourceID="Customers" KeyFieldName="CustNo" NullText="/" PopupHorizontalAlign="Center" RenderIFrameForPopupElements="True" RightToLeft="True" TextFormatString="{0} | {1} | {3}" Width="100%">
                                        <GridViewProperties>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                            <SettingsPopup>
                                                <FilterControl AutoUpdatePosition="False">
                                                </FilterControl>
                                            </SettingsPopup>
                                            <SettingsCommandButton>
                                                <NewButton Text="زبون جديد">
                                                </NewButton>
                                                <UpdateButton Text="حفظ">
                                                </UpdateButton>
                                                <CancelButton Text="إلغاء">
                                                </CancelButton>
                                            </SettingsCommandButton>
                                        </GridViewProperties>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="رقم الزبون" FieldName="CustNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="اسم الزبون" FieldName="CustName" ShowInCustomizationForm="True" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="الاسم اللاتيني" FieldName="CustNameE" ShowInCustomizationForm="True" VisibleIndex="3">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم الهاتف" FieldName="TelNo" ShowInCustomizationForm="True" VisibleIndex="4">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="العنوان" FieldName="Address" ShowInCustomizationForm="True" VisibleIndex="5">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents DropDown="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                        </ValidationSettings>
                                    </dx:ASPxGridLookup>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al" colspan="10">
                                    <dx:ASPxComboBox ID="CoverType" runat="server" Caption="نوع التغطية" ClientInstanceName="CoverType" DataSourceID="Covers" RightToLeft="True" SelectedIndex="0" TextField="CoverName" ValueField="CoverNo" ValueType="System.Int32" Width="100%">
                                        <ClientSideEvents GotFocus="function(s, e) {  }" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <!-- Place the results container OUTSIDE the round panel -->
            <div id="autocomplete-results" style="width: 300px;"></div>
        </td>
        <td class="auto-style1">
            <dx:ASPxRoundPanel ID="PremiumPanel" ClientInstanceName="PremiumPanel" runat="server" AllowCollapsingByHeaderClick="True"
                RightToLeft="True" HeaderText="/ العملة /طريقة الدفع /بيانات القسط" Width="100%" Collapsed="false">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table style="width: 100%; padding: 5px;" dir="rtl">
                            <tr>
                                <td class="auto-style1">
                                    <strong>صافـــي القسط</strong></td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="NETPRM" runat="server" ClientEnabled="False" ClientInstanceName="NetPRM" CssClass="2" Font-Bold="True" oreColor="#0033cc" Text="0" Width="108px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="text-align: left" class="auto-style1">
                                    <dx:ASPxComboBox ID="Currency" runat="server" ClientInstanceName="Currency" DataSourceID="Cur" RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" ValueType="System.Int32" Width="110px">
                                        <ClientSideEvents GotFocus="function(s, e) {  }" SelectedIndexChanged="function(s, e) {cbp.PerformCallback('ExRate');}" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td aria-live="off">
                                    <dx:ASPxTextBox ID="ExcRate" runat="server" Caption="سعر الصرف" ClientInstanceName="ExcRate" CaptionSettings-Position="Bottom" CssClass="5" Text="1" Width="110px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RegularExpression ErrorText="zero value not allowed" ValidationExpression="^([1-9]\d*(\.\d+)?|0\.\d+)$" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>دمغة التصرف</td>
                                <td>
                                    <strong><span class="auto-style17">
                                        <dx:ASPxTextBox ID="TAXPRM" runat="server" ClientEnabled="False" ClientInstanceName="TAXPRM" CssClass="2" ForeColor="#0033CC" Text="0" Width="108px">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </span></strong>
                                </td>
                                <td style="text-align: left">
                                    <dx:ASPxComboBox ID="PayType" runat="server" ClientInstanceName="PayType" DataSourceID="Pay" RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" ValueType="System.Int32" Width="110px">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                                     cbp.PerformCallback('PayType');  }" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td><span class="auto-style17">
                                    <dx:ASPxTextBox ID="AccountNo" runat="server" ClientEnabled="False" ClientInstanceName="AccountNo" CssClass="1" Text="0" Width="110px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </span></td>
                            </tr>
                            <tr>
                                <td>رسوم الإشراف والرقابة</td>
                                <td><strong>
                                    <dx:ASPxTextBox ID="CONPRM" runat="server" ClientEnabled="False" ClientInstanceName="CONPRM" CssClass="2" ForeColor="#0033CC" Text="0" Width="108px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </strong></td>
                                <td style="text-align: left">
                                    <asp:SqlDataSource ID="Pay" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Pay' order by TPNo"></asp:SqlDataSource>
                                </td>
                                <td>
                                    <asp:SqlDataSource ID="Cur" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Cur' order by TpNo"></asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>دمغة المحررات</td>
                                <td>
                                    <strong>
                                        <dx:ASPxTextBox ID="STMPRM" runat="server" ClientInstanceName="STMPRM" CssClass="2" ForeColor="#0033CC" Text="0" Width="108px">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </strong>
                                </td>
                                <td style="text-align: left"><strong>
                                    <dx:ASPxTextBox ID="LASTNET" runat="server" ClientEnabled="False" ClientInstanceName="LASTNET" ClientVisible="False" CssClass="2" Text="0" Width="108px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </strong></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>مصاريف الإصدار</td>
                                <td><strong>
                                    <dx:ASPxTextBox ID="ISSPRM" runat="server" ClientEnabled="False" ClientInstanceName="ISSPRM" CssClass="2" ForeColor="#0033CC" SelectInputTextOnClick="True" Text="0" Width="108px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </strong></td>
                                <td style="text-align: left">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td rowspan="4">
                                    <strong>الإجمـــــالي</strong></td>
                                <td rowspan="4"><strong>
                                    <dx:ASPxTextBox ID="TOTPRM" runat="server" ClientEnabled="False" ClientInstanceName="TOTPRM" CssClass="2" Font-Bold="True" ForeColor="Red" Text="0" Width="108px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </strong></td>
                                <td style="text-align: left" colspan="2"></td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
        </td>
    </tr>
    <tr>
        <td class="auto-style1" colspan="2">
            <dx:ASPxRoundPanel ID="CoverDate" ClientInstanceName="CoverDate" runat="server" AllowCollapsingByHeaderClick="True" RightToLeft="True" HeaderText="فترة التغطية" Width="100%" HeaderStyle-Height="20px">
                <HeaderStyle Height="20px"></HeaderStyle>
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td class="dx-al">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP='meser'"></asp:SqlDataSource>
                                    <dx:ASPxSpinEdit ID="Interval" runat="server" ClientInstanceName="Intv" RightToLeft="True"
                                        Width="100px" AllowNull="false">
                                        <SpinButtons Position="Left" ClientVisible="false">
                                        </SpinButtons>
                                        <ClientSideEvents ValueChanged="DAdd" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxSpinEdit>
                                </td>
                                <td>&nbsp;</td>
                                <td class="auto-style7">
                                    <asp:SqlDataSource ID="Measures" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Measure' order by TPNO"></asp:SqlDataSource>

                                    <dx:ASPxComboBox ID="Measure" runat="server" ClientInstanceName="Measure" DataSourceID="Measures"
                                        ValueType="System.Int32" DropDownStyle="DropDownList"
                                        SelectedIndex="2"
                                        TextField="TpName" ValueField="TpNo" Width="100%" AutoPostBack="false">
                                        <ClientSideEvents SelectedIndexChanged="DAdd" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">التغطية من</td>
                                <td>
                                    <dx:ASPxDateEdit ID="CoverFrom" runat="server" ClientInstanceName="CoverFrom" DisplayFormatString="yyyy/MM/dd"
                                        EditFormatString="yyyy/MM/dd" RightToLeft="True" Width="100%">
                                        <ClientSideEvents DateChanged="Validate" ValueChanged="DAdd" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </td>
                                <td class="dx-al">إلى</td>
                                <td class="auto-style7">
                                    <dx:ASPxDateEdit ID="CoverTo" runat="server" ClientInstanceName="CoverTo" ReadOnlyStyle-ForeColor="Black"
                                        ClientEnabled="false" ClientReadOnly="True" DisplayFormatString="yyyy/MM/dd" RightToLeft="True" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <dx:ASPxRoundPanel ID="Commissions" runat="server" AllowCollapsingByHeaderClick="True" ClientInstanceName="Commissions" Collapsed="True" HeaderText="العمولات والمسوقين" RightToLeft="true" Width="100%">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dx-al">العميل أو المسوق </td>
                                <td colspan="2">
                                    <dx:ASPxGridLookup ID="Broker" runat="server" AutoGenerateColumns="False" AutoResizeWithContainer="True" ClientInstanceName="Broker" DataSourceID="Brokers" KeyFieldName="TpNo" PopupHorizontalAlign="Center" RenderIFrameForPopupElements="True" RightToLeft="True" TextFormatString="{0} | {1} " Width="100%">
                                        <GridViewProperties>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                            <SettingsPopup>
                                                <FilterControl AutoUpdatePosition="False">
                                                </FilterControl>
                                            </SettingsPopup>
                                            <SettingsCommandButton>
                                                <NewButton Text="عميل/مسوق جديد">
                                                </NewButton>
                                                <UpdateButton Text="حفظ">
                                                </UpdateButton>
                                                <CancelButton Text="إلغاء">
                                                </CancelButton>
                                            </SettingsCommandButton>
                                        </GridViewProperties>
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowInCustomizationForm="True" ShowNewButton="True" VisibleIndex="0">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم العميل أو المسوق" FieldName="TpNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="اسم العميل أو المسوق" FieldName="BrokerName" ShowInCustomizationForm="True" VisibleIndex="2">
                                                <PropertiesTextEdit Width="250px">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents DropDown="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" 
                                            Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" 
                                            ValueChanged="function(s, e) {
                                            //alert(CommisionTp.GetValue());
                                            if (s.GetValue() == 0) {
                                                          Commision.SetEnabled(false);
                                                          CommisionTp.SetEnabled(false);
                                                          Commision.SetValue(0);
                                                          CommisionTp.SetValue(0);
                                                        } else {
                                                            Commision.SetEnabled(true);
                                                            CommisionTp.SetEnabled(true);
                                                        }

                                            }" />
                                    </dx:ASPxGridLookup>

                                    <%--ValueChanged="function(e,s) {OnBrokerChanged();}"--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">
                                    <asp:SqlDataSource ID="Brokers" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" InsertCommand="NewBroker" InsertCommandType="StoredProcedure" SelectCommand="SELECT TpNo, rtrim([TpName]) As BrokerName FROM [ExtraInfo] where TP='Broker' Order By TpNo desc">
                                        <InsertParameters>
                                            <asp:Parameter Name="BrokerName" Type="String" />
                                        </InsertParameters>
                                    </asp:SqlDataSource>
                                    العمولة </td>
                                <td>
                                    <dx:ASPxTextBox ID="Commision" runat="server" ClientEnabled="false" ClientInstanceName="Commision" CssClass="2" DisplayFormatString="n2" SelectInputTextOnClick="True" Width="110px">
                                        <%--<MaskSettings Mask="&lt;0..99&gt;.&lt;00..99&gt;" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>--%>
                                        <%--<ClientSideEvents  LostFocus="function(s, e) {}" />--%>
                                        <%--<ClientSideEvents LostFocus="function(e,s) {OnCommChanged();}" />--%>
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxRadioButtonList ID="CommisionType" runat="server" Border-BorderStyle="None" Caption="نوع العمولة" ClientEnabled="false" ClientInstanceName="CommisionTp" RepeatLayout="OrderedList" ValueType="System.Int32" Width="100%">
                                        <Items>
                                            <dx:ListEditItem Text="العمولة قيمة" Value="1" />
                                            <dx:ListEditItem Text="العمولة نسبة%" Value="2" />
                                        </Items>
                                        <%--<ValidationSettings>
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>--%>
                                        <Border BorderStyle="None" />
                                    </dx:ASPxRadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <dx:ASPxButton ID="ExcelImp" runat="server" AutoPostBack="false" Text=" استيراد من ملف Excel " Width="100%">
                <Image Url="../Content/Images/IMPORTEXCEL.png">
                </Image>
                <ClientSideEvents Click="function(s,e){ cbp.PerformCallback('Import'); }" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="DistPolicy" runat="server" AutoPostBack="false" Text=" معاينة توزيع الوثيقة  " Width="100%">
                <Image Url="../Content/Images/DistPolicy.png">
                </Image>
                <ClientSideEvents Click="function(s,e){ cbp.PerformCallback('Dist'); }" />
            </dx:ASPxButton>
        </td>
    </tr>
</table>
<hr style="height: 4px; border-width: 0; color: darkgreen; background-color: darkgray">