<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BulkReceipt.aspx.vb" Inherits="FinanceBulkReceipt" %>

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
            cbp.PerformCallback("Sadad");
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
                                    <table>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="سداد مجموعة وثائق"></dx:ASPxLabel>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <table style="width: 100%;">
                            <tr>
                                <td class="dx-al" colspan="6">
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
                                    <asp:SqlDataSource ID="Systems" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select SUBSYSNO,rtrim(SUBSYSNAME) as SUBSYSNAME,MAINSYS from SUBSYSTEMS where SysType=1 and Branch=dbo.MainCenter() Order By MAINSYS"></asp:SqlDataSource>
                                    
                                    <asp:SqlDataSource ID="Pay" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Payment' order by TPNo"></asp:SqlDataSource>

                                    <asp:SqlDataSource ID="MoveDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                                        SelectCommand="SELECT PolicyFile.PolNo, PolicyFile.OrderNo, PolicyFile.TOTPRM,IssuDate, PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain,PolicyFile.EndNo,PolicyFile.LoadNo,ISNULL(PolicyFile.Inbox, 0) AS InBox,DBO.GetExtraCatName('Cur',PolicyFile.Currency) As Curr,CustName,AccountName FROM PolicyFile LEFT OUTER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER JOIN BranchInfo ON PolicyFile.Branch = BranchInfo.BranchNo LEFT OUTER JOIN SUBSYSTEMS ON PolicyFile.SubIns = SUBSYSTEMS.SUBSYSNO AND SUBSYSTEMS.Branch = PolicyFile.Branch LEFT OUTER JOIN AccountFile ON AccountFile.AccountNo=PolicyFile.IssueUser WHERE Inbox<>TOTPRM AND IssuDate between @IssuDate and @IssuDateTo AND PolicyFile.Branch=@Br AND PayType=1 AND PolicyFile.SubIns=@Sys and PolicyFile.Stop=0 and Commision=0">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="MoveDate" Name="IssuDate" PropertyName="Value" DefaultValue="0" />
                                            <asp:ControlParameter ControlID="MoveDateTo" Name="IssuDateTo" PropertyName="Value" DefaultValue="0" />
                                            <asp:SessionParameter Name="Br" SessionField="Branch" DefaultValue="0" />
                                            <asp:ControlParameter ControlID="Sys" Name="Sys" PropertyName="Value" DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="Accounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level&gt;=5 and (left(AccountNo,5)='1.1.3')"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="BankAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and (left(AccountNo,7)='1.1.1.2')"></asp:SqlDataSource>
                                   <asp:SqlDataSource ID="AccountsNotPayed" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=5 and (left(ltrim(AccountNo),8)='1.1.10.1') and AccountNo in (Select AccountNum from DailyJournal where SubIns=@Sys)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="Sys" PropertyName="Value" DefaultValue="0" Name="Sys"></asp:ControlParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">استلمت من :</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Customer" runat="server" Width="100%">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">مبلغ وقدره :</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Payed" runat="server" Width="170px" ClientInstanceName="SelectedSum" ReadOnly="true"
                                        DisplayFormatString="N3" Text="0">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxComboBox ID="Sys" runat="server" Caption=" وذلك عن إنتاج : " DataSourceID="Systems" EnableCallbackMode="True" OnValueChanged="PayTyp_ValueChanged" RightToLeft="True" TextField="SUBSYSNAME" ValueField="SUBSYSNO" Width="100%">
                                        <ClientSideEvents ValueChanged="HideOrShow" />
                                    </dx:ASPxComboBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxComboBox ID="PayTyp" runat="server" Caption="طريقة الدفع :" DataSourceID="Pay" EnableCallbackMode="True" IncrementalFilteringMode="StartsWith" OnValueChanged="PayTyp_ValueChanged" RightToLeft="True" TextField="TpName" ValueField="TpNo" Width="170px">
                                        <ClientSideEvents ValueChanged="HideOrShow" />
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">&nbsp;</td>
                                <td class="style1" style="text-align: left">&nbsp;</td>
                                <td class="dx-al">&nbsp;</td>
                                <td class="style1">
                                    <dx:ASPxDateEdit ID="MoveDate" runat="server" Caption=" للفترة من تاريخ ">
                                        <ClientSideEvents DateChanged="function(s, e) {
                                        griddata.PerformCallback();} " />
                                    </dx:ASPxDateEdit>
                                </td>
                                <td class="style1">
                                    <dx:ASPxDateEdit ID="MoveDateTo" runat="server" Caption=" إلى تاريخ  ">
                                        <ClientSideEvents DateChanged="function(s, e) {
                                        griddata.PerformCallback();} " />
                                    </dx:ASPxDateEdit>
                                </td>
                                <td class="style1">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dx-al">الوصف :</td>
                                <td class="style1" colspan="5">
                                    <dx:ASPxTextBox ID="Note" runat="server" AutoResizeWithContainer="True" ClientInstanceName="note" ReadOnly="True" Text="/" Width="100%">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">وبيانها كالتالي :</td>
                                <td class="style1" colspan="5" rowspan="2">

                                    <dx:ASPxGridView ID="GridData" ClientInstanceName="griddata" runat="server" Width="100%"
                                        SettingsText-SelectAllCheckBoxInAllPagesMode="All"
                                        DataSourceID="MoveDS" KeyFieldName="PolNo;EndNo;LoadNo;OrderNo;AccountName" AutoGenerateColumns="False">
                                        <Settings ShowFilterRow="True" ShowFooter="true" />
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False"></FilterControl>
                                        </SettingsPopup>
                                        <ClientSideEvents SelectionChanged="grid_SelectionChanged" />

                                        <SettingsSearchPanel Visible="True" />

                                        <SettingsText SelectAllCheckBoxInAllPagesMode="All"></SettingsText>
                                        <Columns>
                                            <dx:GridViewCommandColumn SelectAllCheckboxMode="AllPages" ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0" ShowClearFilterButton="True">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataColumn FieldName="PolNo" Caption="رقم الوثيقة" VisibleIndex="1"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="CustName" Caption="اسم الزبون" VisibleIndex="2"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="IssuDate" Caption="تاريخ الإصدار" VisibleIndex="3"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="OrderNo" Caption="رقم الطلب" VisibleIndex="4" Visible="false"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EndNo" Caption="رقم الملحق" VisibleIndex="5"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="LoadNo" Caption="رقم الإشعار" VisibleIndex="6"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TOTPRM" Caption="اجمالي القسط" VisibleIndex="7"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption=" صدرت بواسطة " FieldName="AccountName" ShowInCustomizationForm="True" VisibleIndex="13">
                                                <CellStyle Wrap="True">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <%--<dx:GridViewDataColumn FieldName="Curr" Caption="العملة" VisibleIndex="8"></dx:GridViewDataColumn>--%>
                                        </Columns>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="TOTPRM" ShowInColumn="TOTPRM" SummaryType="Sum" Tag="إجمالي الأقساط" ValueDisplayFormat="n3" />
                                        </TotalSummary>
                                        <SettingsPager PageSize="10" />
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al" style="vertical-align: bottom;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dx-al">&nbsp;</td>
                                <td class="style1" colspan="5">
                                    <dx:ASPxComboBox ID="AccName" runat="server"
                                        ClientInstanceName="AccName"
                                        ClientVisible="False"
                                        NullText="/" NullValueItemDisplayText="{1} || {0}" ValueType="System.String"
                                        RightToLeft="True" ValueField="AccountNo" TextField="AccountName"
                                        TextFormatString="{1} || {0}" Width="100%">
                                        <Columns>
                                            <dx:ListBoxColumn Caption="رقم الحساب" FieldName="AccountNo">
                                            </dx:ListBoxColumn>
                                            <dx:ListBoxColumn Caption="اسم الحساب" FieldName="AccountName">
                                            </dx:ListBoxColumn>
                                        </Columns>
                                        <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="مطلوب" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">&nbsp;</td>
                                <td class="style1" colspan="5">
                                    <dx:ASPxTextBox ID="AccNo" runat="server" Caption="رقم الصك/الإشعار :"
                                        ClientInstanceName="AccNo" ClientVisible="False" Width="170px">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">&nbsp;</td>
                                <td colspan="5">
                                    <dx:ASPxTextBox ID="Bank" ClientInstanceName="Bank" runat="server" Width="170px"
                                        Caption="على مصـرف :" ClientVisible="false">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al"></td>
                                <td colspan="2">
                                    <dx:ASPxButton ID="sdad" runat="server" AutoPostBack="False" UseSubmitBehavior="False"
                                        ClientInstanceName="sdad" Text="سداد">
                                        <ClientSideEvents Click="Sadad" />
                                    </dx:ASPxButton>
                                </td>
                                <td colspan="3">
                                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="خروج">
                                        <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();
                        }" />
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