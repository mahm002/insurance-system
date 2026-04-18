<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreditPayments.aspx.vb" Inherits="CreditPayments" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function preventBack() {
            window.history.forward();
        }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };

        function ReturnToParentPage() {
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup(1);
        }

        function Sadad(s, e) {
            //if (ASPxClientEdit.ValidateGroup('Data'))

            if (ASPxClientEdit.ValidateGroup('ValidGroup')) {
                pcConfirmIssue.Show();
            }
        }

        //cbp.PerformCallback('Sadad');

        function YesIss_Click() {
            sdad.SetText('يرجى الانتظار حتى تتم عملية إصدار القيد');
            sdad.SetEnabled(false);
            cbp.PerformCallback("Approve");
            pcConfirmIssue.Hide();
        }

        function NoIss_Click() {
            pcConfirmIssue.Hide();
            s.SetText('سداد');
            s.SetEnabled(true);
        }

        function OnEndCallback(s, e) {

            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Issuance') {
                pcConfirmIssue.Show();
                pcConfirmIssue.SetHeaderText(s.cpCust);
            }
            s.cpMyAttribute = '';
            s.cpNewWindowUrl = null;

            //UpdateTimeoutTimer();/////////////////////////////

        }
        function onMetodChange(s, e) {
            griddata.PerformCallback();
        }
        function HideOrShow(s, e) {

            cbp.PerformCallback('PaymentChanged');
        }
        function grid_SelectionChanged(s, e) {
            //s.GetSelectedFieldValues("TOTPRM", GetSelectedFieldValuesCallback);
            //s.GetSelectedFieldValues("PolNo", GetSelectedFieldValuesCallback1);

            //clb.PerformCallback(selList.items());
            //
            cbp.PerformCallback('PaymentChanged');
        }
        function ListEndCallBack() {
            //cbp.PerformCallback('SelectionChanged');
        }
        //function OnChange(s, e) {
        //    if (s.GetValue() == null) {
        //        s.SetValue(0);
        //    } else {
        //        HiddenPayments.SetValue(0);
        //    }
        //   // scbp.PerformCallback('ShareChange');
        //}
        function GetSelectedFieldValuesCallback(values) {
            //alert(values);
            try {
                SelectedSum.SetValue(0);
                for (var i = 0; i < values.length; i++) {
                    SelectedSum.SetValue(parseFloat(SelectedSum.GetValue()) + parseFloat(values[i]));

                }
            } finally {

            }
            //document.getElementById("selCount").innerHTML = detailGrid.GetSelectedRowCount();
            /*alert(detailGrid.GetSelectedRowCount());*/
        }
        function GetSelectedFieldValuesCallback1(values) {
            if (values.length == 0) {
                selList.ClearItems();
            }
            else {
                //alert(values.length);
            }

            try {
                selList.ClearItems();
                for (var i = 0; i < values.length; i++) {
                    selList.AddItem(values[i]);

                    //alert(values[i]);
                }
            } finally {
                //selList.EndUpdate();
                //cbp.PerformCallback('SelectionChanged');
            }
            //document.getElementById("selCount").innerHTML = detailGrid.GetSelectedRowCount();
            //cbp.PerformCallback('SelectionChanged');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div dir="rtl">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" EndCallback="OnEndCallback" />
                <SettingsLoadingPanel Text="   تم الاصدار &amp;hellip; " Delay="0" Enabled="false" ShowImage="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" EnableViewState="False" ShowHeader="False" RightToLeft="True">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="سداد مجموعة وثائق على الحساب"></dx:ASPxLabel>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <table style="width: 100%;">
                            <tr>
                                <td class="auto-style5" colspan="6">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style5">
                                    <asp:SqlDataSource ID="Accounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level&gt;=5 and (left(AccountNo,7)='1.1.3.1' or left(AccountNo,7)='1.1.3.2')"></asp:SqlDataSource>
                                </td>
                                <td class="auto-style7" colspan="2">&nbsp;
                                    <asp:SqlDataSource ID="Systems" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select SUBSYSNO,rtrim(SUBSYSNAME) as SUBSYSNAME,MAINSYS from SUBSYSTEMS where SysType=1 and Branch=dbo.MainCenter() Order By MAINSYS"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="MoveDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>" SelectCommand="SELECT PolicyFile.PolNo, PolicyFile.OrderNo, PolicyFile.TOTPRM,IssuDate, PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain,PolicyFile.EndNo,PolicyFile.LoadNo,ISNULL(PolicyFile.Inbox, 0) AS InBox,DBO.GetExtraCatName('Cur',PolicyFile.Currency) As Curr,CustName FROM PolicyFile LEFT OUTER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER JOIN BranchInfo ON PolicyFile.Branch = BranchInfo.BranchNo LEFT OUTER JOIN SUBSYSTEMS ON PolicyFile.SubIns = SUBSYSTEMS.SUBSYSNO AND SUBSYSTEMS.Branch = PolicyFile.Branch WHERE Inbox&lt;&gt;TOTPRM AND PolicyFile.Branch=@Br AND PayType=2 and AccountNo=@AccntNo AND PolicyFile.SubIns=@Sys and PolicyFile.Stop=0 Order By IssuTime">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="AccName" DefaultValue="0" Name="AccntNo" PropertyName="Value" />
                                            <asp:SessionParameter DefaultValue="0" Name="Br" SessionField="Branch" />
                                            <asp:ControlParameter ControlID="Sys" DefaultValue="0" Name="Sys" PropertyName="Value" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td class="auto-style5">
                                    <asp:SqlDataSource ID="Pay" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Payment' and TPNo&lt;&gt;4 order by TPNo"></asp:SqlDataSource>
                                </td>
                                <td class="auto-style4">
                                    <asp:SqlDataSource ID="BankAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and (left(AccountNo,7)='1.1.1.2')"></asp:SqlDataSource>
                                </td>
                                <td class="auto-style3">
                                    <dx:ASPxPopupControl ID="pcConfirmIssue" runat="server" ClientInstanceName="pcConfirmIssue" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server" Font-Bold="True">
                                                <table dir="rtl">
                                                    <tr>
                                                        <td colspan="1" style="vertical-align: central;">تأكيد الإجراء</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="yesIssButton" runat="server" AutoPostBack="False" Text="موافق">
                                                                <ClientSideEvents Click="YesIss_Click" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="noIssButton" runat="server" AutoPostBack="False" Text="غير موافق">
                                                                <ClientSideEvents Click="NoIss_Click" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style5" colspan="4">
                                    <dx:ASPxComboBox ID="AccName" runat="server" Caption="حساب المدينون" ClientInstanceName="AccName"
                                        DataSourceID="Accounts" NullText="/"
                                        NullValueItemDisplayText="{1} || {0}"
                                        OnValueChanged="AccName_ValueChanged" RightToLeft="True"
                                        TextField="AccountName" TextFormatString="{1} || {0}" ValueField="AccountNo" Width="100%">
                                        <ClientSideEvents ValueChanged="function(s, e) { cbp.PerformCallback(); }" />
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
                                <td class="auto-style4">
                                    <dx:ASPxTextBox ID="Balance" runat="server" Caption="الرصيد" ClientInstanceName="Balance" DisplayFormatString="N3" ReadOnly="True" Text="0" Width="170px">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style3"></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <dx:ASPxTextBox ID="Customer" runat="server" Caption="استلمت من" Width="100%">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style4">
                                    <dx:ASPxComboBox ID="Sys" runat="server" Caption="وذلك عن وثائق"
                                        DataSourceID="Systems" EnableCallbackMode="True" OnValueChanged="Sys_ValueChanged"
                                        RightToLeft="True" TextField="SUBSYSNAME" ValueField="SUBSYSNO" Width="100%">
                                        <ClientSideEvents ValueChanged="HideOrShow" />
                                        <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td class="auto-style3">
                                    <dx:ASPxComboBox ID="PayTyp" runat="server" Caption="طريقة الدفع" DataSourceID="Pay" EnableCallbackMode="True" IncrementalFilteringMode="StartsWith" OnValueChanged="PayTyp_ValueChanged" RightToLeft="True" TextField="TpName" ValueField="TpNo" Width="170px">
                                        <ClientSideEvents ValueChanged="HideOrShow" />
                                        <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style5" colspan="2">
                                    <dx:ASPxTextBox ID="Payed" runat="server" Caption="مبلغ وقدره"
                                        ClientInstanceName="SelectedSum" DisplayFormatString="N3" Text="0" Width="100%">
                                        <ClientSideEvents LostFocus="HideOrShow" />
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RegularExpression ValidationExpression="^((0?0?\.([1-9]\d*|0[1-9]\d*))|(([1-9]|0[1-9])\d*(\.\d+)?))$" />
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>

                                <td class="auto-style5" colspan="2">
                                    <dx:ASPxComboBox ID="Method" runat="server" Caption="طريقة توزيع المبلغ" EnableCallbackMode="True" IncrementalFilteringMode="StartsWith" OnValueChanged="Method_ValueChanged" RightToLeft="True" Width="170px">
                                        <ClientSideEvents ValueChanged="HideOrShow" />
                                        <Items>
                                            <dx:ListEditItem Text="حسب أولوية إصدار الوثائق" Value="1" />
                                            <dx:ListEditItem Text="نسبية على جميع الوثائق" Value="2" />
                                        </Items>
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                                <td class="auto-style3"></td>
                            </tr>
                            <tr>
                                <td class="dx-al" colspan="6">
                                    <dx:ASPxGridView ID="GridData" runat="server" AutoGenerateColumns="False"
                                        ClientInstanceName="griddata" DataSourceID="MoveDS"
                                        OnDataBound="GridData_DataBound"
                                        KeyFieldName="PolNo;EndNo;LoadNo;OrderNo" Width="100%">
                                        <Settings ShowFilterRow="True" ShowFooter="True" AutoFilterCondition="Contains" />
                                        <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False">
                                            </FilterControl>
                                        </SettingsPopup>
                                        <SettingsSearchPanel Visible="True" />
                                        <SettingsText SelectAllCheckBoxInAllPagesMode="All" />
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowClearFilterButton="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم الوثيقة" FieldName="PolNo" ShowInCustomizationForm="True" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="الاسم" FieldName="CustName" ShowInCustomizationForm="True" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="تاريخ الإصدار" FieldName="IssuDate" ShowInCustomizationForm="True" VisibleIndex="3">
                                                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم الطلب" FieldName="OrderNo" ShowInCustomizationForm="True" Visible="False" VisibleIndex="4">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم الملحق" FieldName="EndNo" ShowInCustomizationForm="True" VisibleIndex="5">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم الإشعار" FieldName="LoadNo" ShowInCustomizationForm="True" VisibleIndex="6">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="الإجمالي" FieldName="TOTPRM" ShowInCustomizationForm="True" VisibleIndex="7">
                                                <PropertiesTextEdit DisplayFormatString="n3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="اجمالي المسدد" FieldName="InBox" ShowInCustomizationForm="True" VisibleIndex="8">
                                                <PropertiesTextEdit DisplayFormatString="n3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="المتبقي" FieldName="Remain" ShowInCustomizationForm="True" VisibleIndex="9">
                                                <PropertiesTextEdit DisplayFormatString="n3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MyTotal" Caption="Total" UnboundType="Decimal" UnboundExpression="TOTPRM-InBox" />
                                            <%--           <dx:GridViewDataTextColumn Caption="التوزيع" ShowInCustomizationForm="True" VisibleIndex="10">
                                                <DataItemTemplate>
                                                    <dx:ASPxLabel ID="Calculated" runat="server" OnDataBound="Calculated_DataBound"
                                                        Value='<%# Math.Abs(Eval("Remain")) %>'>
                                                    </dx:ASPxLabel>
                                                </DataItemTemplate>
                                                <FooterTemplate>
                                                    <dx:ASPxLabel ID="lblTotalPercent" runat="server" Font-Bold="true">
                                                    </dx:ASPxLabel>
                                                </FooterTemplate>
                                            </dx:GridViewDataTextColumn>--%>
                                        </Columns>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="TOTPRM" ShowInColumn="TOTPRM" ShowInGroupFooterColumn="إجمالي القسط" SummaryType="Sum" ValueDisplayFormat="n3" />
                                            <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="InBox" ShowInColumn="InBox" SummaryType="Sum" Tag=" المسدد" ValueDisplayFormat="n3" />
                                            <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="Remain" ShowInColumn="Remain" SummaryType="Sum" Tag=" المتبقي" ValueDisplayFormat="n3" />
                                        </TotalSummary>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style5">&nbsp;</td>
                                <td colspan="5" class="auto-style7">
                                    <dx:ASPxTextBox ID="AccNo" runat="server" Caption="رقم الصك/الإشعار :" ClientInstanceName="AccNo" ClientVisible="False" Width="170px">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style5">&nbsp;</td>
                                <td class="auto-style7" colspan="5">
                                    <dx:ASPxTextBox ID="Bank" runat="server" Caption="على مصـرف :" ClientInstanceName="Bank" ClientVisible="False" Width="170px">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style5">
                                    <dx:ASPxButton ID="sdad" runat="server" AutoPostBack="False" ClientInstanceName="sdad" Text="سداد" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="Sadad" />
                                    </dx:ASPxButton>
                                </td>
                                <td class="auto-style7" colspan="5">
                                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="خروج">
                                        <ClientSideEvents Click="function(s, e) {ReturnToParentPage();}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
            <dx:ASPxListBox ID="SelList" runat="server" ClientInstanceName="selList" ClientVisible="false"
                EnableCallbackMode="false" Width="100%">
            </dx:ASPxListBox>
        </div>
    </form>
</body>
</html>