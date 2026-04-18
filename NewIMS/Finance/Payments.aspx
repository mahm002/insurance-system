<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Payments.aspx.vb" Inherits=".Payments" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Payment Distribution System</title>
    <style>
        .container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 20px;
            direction: rtl;
        }

        .header {
            margin-bottom: 20px;
            text-align: center;
        }

        .selection-panel {
            margin-bottom: 20px;
            padding: 15px;
            background-color: #f8f9fa;
            border-radius: 5px;
        }

        .payment-controls {
            margin: 20px 0;
            padding: 15px;
            background-color: #e8f4f8;
            border-radius: 5px;
        }

        .control-group {
            margin-bottom: 15px;
        }

        .status-مسدد {
            color: green;
            font-weight: bold;
        }

        .status-سداد_جزئي {
            color: orange;
            font-weight: bold;
        }

        .status-معلق {
            color: red;
            font-weight: bold;
        }

        .status-غير_مسدد {
            color: blue;
            font-weight: bold;
        }

        .grid-container {
            margin-top: 20px;
        }
        .auto-style2 {
            height: 30px;
        }
        .auto-style3 {
            height: 68px;
        }
    </style>
    <script type="text/javascript">
        function preventBack() {
            window.history.forward();
        }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };

        function OnAccountChanged(s, e) {
            CheckSelections();
        }

        function OnSystemChanged(s, e) {
            CheckSelections();
        }

        function CheckSelections() {
            var customer = cmbCustomer.GetValue();
            var productType = cmbProductType.GetValue();

            cbp.PerformCallback("LOAD_INVOICES");
            btnLoadInvoices.SetEnabled(customer != null && productType != null);
        }
        function HideOrShow(s, e) {
            cbp.PerformCallback('PaymentChanged');
        }

        function LoadInvoices() {
            if (cmbCustomer.GetValue() != null && cmbProductType.GetValue() != null) {
                //LoadingPanel.Show();
                cbp.PerformCallback("LOAD_INVOICES");
            } else {
                alert("يرجى تحديد حساب المدينون ونوع التأمين.");
            }
        }

        function CalculatePayments() {
            var paymentAmount = parseFloat(txtPaymentAmount.GetText());
            if (isNaN(paymentAmount) || paymentAmount <= 0) {
                alert("يرجى أدخال قيمة المبلغ المسدد .");
                return;
            }

            //LoadingPanel.Show();
            cbp.PerformCallback("CALCULATE:" + paymentAmount);
        }
        function grid_SelectionChanged(s, e) {
            //s.GetSelectedFieldValues("TOTPRM", GetSelectedFieldValuesCallback);
            //s.GetSelectedFieldValues("PolNo", GetSelectedFieldValuesCallback1);

            //clb.PerformCallback(selList.items());
            //
            cbp.PerformCallback('PaymentChanged');
        }
        function SavePayments() {
            var paymentAmount = parseFloat(txtPaymentAmount.GetText());
            if (isNaN(paymentAmount) || paymentAmount <= 0) {
                alert("يرجى أدخال قيمة المبلغ المسدد .");
                return;
            }

            cbp.PerformCallback("SAVE_PAYMENTS:" + paymentAmount);
        }

        function OnEndCallback(s, e) {
            //LoadingPanel.Hide();

            if (s.cpErrorMessage) {
                alert(s.cpErrorMessage);
                s.cpErrorMessage = null;
            }

            if (s.cpSuccessMessage) {
                alert(s.cpSuccessMessage);
                s.cpSuccessMessage = null;

                // Clear payment amount on successful save
                if (s.cpRefreshGrid) {
                    txtPaymentAmount.SetText("");
                }
            }

            if (s.cpMessage) {
                lblMessage.SetText(s.cpMessage);
                s.cpMessage = null;
            }

            // Always refresh the grid if requested
            if (s.cpRefreshGrid) {
                // Use the grid's custom callback to refresh
                griddata.PerformCallback('refresh');
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" />

        <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
            <ClientSideEvents EndCallback="OnEndCallback" />
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" EnableViewState="true" ShowHeader="False" RightToLeft="True">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <div class="container">
                                    <div class="header">
                                        <h1 class="dx-ar">تسوية وثائق (على الحساب أو تحت التحصيل)</h1>
                                        <dx:ASPxLabel ID="lblBranch" runat="server" Font-Bold="true" ForeColor="Maroon" Visible="false"></dx:ASPxLabel>
                                    </div>
                                    <!-- Selection Panel -->
                                    <div class="selection-panel">
                                        <div class="control-group">
                                            <%--<dx:ASPxLabel ID="lblCustomer" runat="server" Text="اختر الحساب المدين :"></dx:ASPxLabel>--%>
                                            <table style="width:100%;">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:SqlDataSource ID="Pay" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Payment' AND TPNo&lt;&gt;6 order by TPNo"></asp:SqlDataSource>
                                                        <asp:SqlDataSource ID="BankAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and (left(AccountNo,7)='1.1.1.2')"></asp:SqlDataSource>
                                                        <asp:SqlDataSource ID="Accounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level&gt;=5 and (left(AccountNo,7)='1.1.3.1' or left(AccountNo,7)='1.1.3.2' or left(AccountNo,8)='1.1.10.1')"></asp:SqlDataSource>
                                                        <asp:SqlDataSource ID="AccountPayable" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level&gt;=5 and (left(AccountNo,5)='1.1.3')"></asp:SqlDataSource>
                                                        <asp:SqlDataSource ID="Systems" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select SUBSYSNO,rtrim(SUBSYSNAME) as SUBSYSNAME,MAINSYS from SUBSYSTEMS where SysType=1 and Branch=dbo.MainCenter() Order By MAINSYS"></asp:SqlDataSource>
                                                        <dx:ASPxComboBox ID="cmbCustomer" runat="server" Caption="اختر الحساب المدين/ أو تحت التحصيل" ClientInstanceName="cmbCustomer" DataSourceID="Accounts" NullText="/" TextField="AccountName" ValueField="AccountNo" Width="400px">
                                                            <ClientSideEvents ValueChanged="OnAccountChanged" />
                                                            <ClearButton DisplayMode="OnHover">
                                                            </ClearButton>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="Balance" runat="server" Caption="الرصيد" ForeColor="Red" ClientInstanceName="Balance"
                                                            DisplayFormatString="N3" ReadOnly="True" Text="0" Width="170px">
                                                            <ValidationSettings ValidationGroup="ValidGroup">
                                                                <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="auto-style2" colspan="2">
                                                        <dx:ASPxComboBox ID="cmbProductType" runat="server" Caption="اختر نوع التأمين" ClientInstanceName="cmbProductType" DataSourceID="Systems" TextField="SUBSYSNAME" ValueField="SUBSYSNO" Width="300px">
                                                            <ClientSideEvents ValueChanged="OnSystemChanged" />
                                                            <ClearButton DisplayMode="OnHover">
                                                            </ClearButton>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td class="auto-style2">
                                                        <dx:ASPxDateEdit ID="MoveDate" runat="server" Caption="تاريخ الحركة " OnInit="MoveDate_Init">
                                                        </dx:ASPxDateEdit>
                                                    </td>
                                                    <td class="auto-style2"></td>
                                                </tr>
                                              
                                                <tr>
                                                    <td class="auto-style3">
                                                        
                                                        <dx:ASPxTextBox ID="txtPaymentAmount" runat="server" Caption="المبلغ المسدد" ClientInstanceName="txtPaymentAmount" DisplayFormatString="N3" Style="margin: 0 10px;" Width="200px">
                                                            <ValidationSettings ValidationGroup="ValidGroup">
                                                                <RegularExpression ValidationExpression="^((0?0?\.([1-9]\d*|0[1-9]\d*))|(([1-9]|0[1-9])\d*(\.\d+)?))$" />
                                                                <RequiredField ErrorText="مبلغ السداد مطلوب" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td class="auto-style3">
                                                        <dx:ASPxTextBox ID="Customer" runat="server" Width="100%" Caption="استلمت من :">
                                                            <ValidationSettings ValidationGroup="ValidGroup">
                                                                <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td class="auto-style3">
                                                       
                                                        <dx:ASPxComboBox ID="PayTyp" runat="server" Caption="طريقة الدفع :" DataSourceID="Pay" EnableCallbackMode="True" IncrementalFilteringMode="StartsWith" RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" Width="170px">
                                                            <ClientSideEvents ValueChanged="HideOrShow" />
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td class="auto-style3">
                                                        <dx:ASPxButton ID="btnCalculate" runat="server" AutoPostBack="False" Style="margin-right: 10px; width:auto;" Text="توزيع المبلغ على الوثائق" >
                                                            <ClientSideEvents Click="CalculatePayments" />
                                                        </dx:ASPxButton> 
                                                        <dx:ASPxButton ID="btnSave" runat="server" AutoPostBack="False" Style="margin-right: 10px; width:auto;" Text="حفط" >
                                                            <ClientSideEvents Click="SavePayments" />
                                                        </dx:ASPxButton> 
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <dx:ASPxTextBox ID="Bank" runat="server" Caption="على مصـرف :" ClientInstanceName="Bank" ClientVisible="False" Width="170px">
                                                            <ValidationSettings ValidationGroup="ValidGroup">
                                                                <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                        <dx:ASPxTextBox ID="AccNo" runat="server" Caption="رقم الصك/الإشعار :" ClientInstanceName="AccNo" ClientVisible="False" Width="170px">
                                                            <ValidationSettings ValidationGroup="ValidGroup">
                                                                <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <dx:ASPxComboBox ID="AccName" runat="server" ClientInstanceName="AccName" NullText="/" NullValueItemDisplayText="{1} || {0}" RightToLeft="True" TextField="AccountName" TextFormatString="{1} || {0}" ValueField="AccountNo" Width="100%">
                                                            <Columns>
                                                                <dx:ListBoxColumn Caption="رقم الحساب" FieldName="AccountNo">
                                                                </dx:ListBoxColumn>
                                                                <dx:ListBoxColumn Caption="اسم الحساب" FieldName="AccountName">
                                                                </dx:ListBoxColumn>
                                                            </Columns>
                                                            <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                                                <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
                                                            ClientInstanceName="griddata" KeyFieldName="PolNo;OrderNo" 
                                                            OnCustomCallback="ASPxGridView1_CustomCallback" OnDataBinding="ASPxGridView1_DataBinding" RightToLeft="True" Width="100%">
                                                            <ClientSideEvents SelectionChanged="grid_SelectionChanged" />
                                                            <Settings ShowFooter="True" />
                                                            <SettingsBehavior AllowSort="False" />
                                                            <SettingsPopup>
                                                                <FilterControl AutoUpdatePosition="False">
                                                                </FilterControl>
                                                            </SettingsPopup>
                                                            <Columns>
                                                                <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0">
                                                                </dx:GridViewCommandColumn>
                                                                <dx:GridViewDataTextColumn Caption="رقم الوثيقة/الفاتورة" FieldName="PolNo" ShowInCustomizationForm="True" VisibleIndex="1" Width="100px">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="رقم الطلب" FieldName="OrderNo" ShowInCustomizationForm="True" Visible="False" VisibleIndex="2" Width="100px">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataDateColumn Caption="تاريخ الإصدار" FieldName="IssuDate" ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
                                                                    <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd">
                                                                    </PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataTextColumn Caption="اسم المؤمن له" FieldName="CustName" ShowInCustomizationForm="True" VisibleIndex="4" Width="200px">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="إجمالي القسط" FieldName="TOTPRM" ShowInCustomizationForm="True" VisibleIndex="5" Width="120px">
                                                                    <PropertiesTextEdit DisplayFormatString="n3">
                                                                    </PropertiesTextEdit>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="المسدد منه" FieldName="InBox" ShowInCustomizationForm="True" VisibleIndex="6" Width="120px">
                                                                    <PropertiesTextEdit DisplayFormatString="n3">
                                                                    </PropertiesTextEdit>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="المستحق" FieldName="Remain" ShowInCustomizationForm="True" VisibleIndex="7" Width="120px">
                                                                    <PropertiesTextEdit DisplayFormatString="n3">
                                                                    </PropertiesTextEdit>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="نوع العملة" FieldName="Curr" ShowInCustomizationForm="True" VisibleIndex="8" Width="80px">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="رقم الملحق" FieldName="EndNo" ShowInCustomizationForm="True" VisibleIndex="9" Width="80px">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="رقم الإشعار" FieldName="LoadNo" ShowInCustomizationForm="True" VisibleIndex="10" Width="80px">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="المدفوع بعد التوزيع" FieldName="Payment" ShowInCustomizationForm="True" VisibleIndex="11" Width="120px">
                                                                    <PropertiesTextEdit DisplayFormatString="n3">
                                                                    </PropertiesTextEdit>
                                                                    <CellStyle BackColor="#E8F5E9" Font-Bold="True">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="المتبقي بعد التوزيع" FieldName="NewRemaining" ShowInCustomizationForm="True" VisibleIndex="12" Width="120px">
                                                                    <PropertiesTextEdit DisplayFormatString="n3">
                                                                    </PropertiesTextEdit>
                                                                     <CellStyle BackColor="#ff9900" Font-Bold="True">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="الحالة بعد التوزيع" FieldName="Status" ShowInCustomizationForm="True" VisibleIndex="13" Width="100px">
                                                                    <DataItemTemplate>
                                                                        <span class='status-<%# Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                                                                    </DataItemTemplate>
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <TotalSummary>
                                                                <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="TOTPRM" SummaryType="Sum" />
                                                                <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="InBox" SummaryType="Sum" />
                                                                <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="Remain" SummaryType="Sum" />
                                                                <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="Payment" SummaryType="Sum" />
                                                                <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="NewRemaining" SummaryType="Sum" />
                                                            </TotalSummary>
                                                            <Styles>
                                                                <Header Font-Bold="True" HorizontalAlign="Center">
                                                                </Header>
                                                                <Cell HorizontalAlign="Right">
                                                                </Cell>
                                                                <Footer Font-Bold="True">
                                                                </Footer>
                                                            </Styles>
                                                        </dx:ASPxGridView>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="dx-al">
                                                <dx:ASPxLabel ID="lblMessage" runat="server" ClientInstanceName="lblMessage" EnableViewState="False" Style="margin-top: 10px; font-weight: bold;">
                                                </dx:ASPxLabel>
                                                <br />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <%--<dx:ASPxLabel ID="lblProductType" runat="server" Text="اختر نوع التأمين:"></dx:ASPxLabel>--%>
                                        </div>

                                        <dx:ASPxButton ID="btnLoadInvoices" runat="server" Text="Load Invoices" ClientVisible="false"
                                            ClientInstanceName="btnLoadInvoices" AutoPostBack="false" Enabled="false">
                                            <ClientSideEvents Click="function(s, e) { LoadInvoices(); }" />
                                        </dx:ASPxButton>
                                    </div>
                                    <!-- Payment Controls -->
                                    <div class="payment-controls" id="divPaymentControls" runat="server" visible="true">
                                        <%--<dx:ASPxLabel ID="lblPaymentAmount" runat="server" Text="المبلغ المسدد:"></dx:ASPxLabel>--%>
                                        <br />
                                    </div>
                                    <!-- Grid View -->
                                    <div class="grid-container">
                                    </div>
                                </div>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
    </form>
</body>
</html>