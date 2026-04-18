<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReceiptMultiPayments.aspx.vb" Inherits="ReceiptMultiPayments" %>

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

            //if (s.GetValue() == "0") {
            //    AccNo.SetVisible(false);
            //    Bank.SetVisible(false);
            //    AccName.SetVisible(false);
            //}
            //else {
            //    if (s.GetValue() == "1" || s.GetValue() == "2") {
            //        AccNo.SetVisible(true);
            //        Bank.SetVisible(true);
            //        AccName.SetVisible(false);
            //    }
            //    else {
            //        AccNo.SetVisible(false);
            //        Bank.SetVisible(false);
            //        AccName.SetVisible(true);
            //    }

            //}
            cbp.PerformCallback('PaymentChanged');
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
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="تسوية أو سداد وثيقة"></dx:ASPxLabel>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <table style="width: 100%;">
                            <tr>
                                <td class="dx-al">&nbsp;القيمة :</td>
                                <td class="style1" colspan="3">
                                    <dx:ASPxDateEdit ID="MoveDate" runat="server" Caption="تاريخ الحركة " OnInit="MoveDate_Init">
                                    </dx:ASPxDateEdit>
                                    <dx:ASPxTextBox ID="TOTPRM" runat="server" Width="170px" ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1" colspan="2" style="text-align: left">الإجمالي المدفوع :</td>
                                <td class="style1" colspan="2">
                                    <asp:SqlDataSource ID="Pay" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"></asp:SqlDataSource>
                                    <dx:ASPxTextBox ID="Payed" runat="server" Width="170px">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">القيمة الجزئية</td>
                                <td class="style1" colspan="2">
                                    <dx:ASPxTextBox ID="SubPay" runat="server" Width="170px">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField ErrorText="مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1" colspan="2">
                                    <asp:Label ID="Label1" runat="server" Text="طريقة الدفع: "></asp:Label>
                                </td>
                                <td class="style1" colspan="2">
                                    <dx:ASPxComboBox ID="PayTyp" runat="server" DataSourceID="Pay" EnableCallbackMode="True" Height="18px" IncrementalFilteringMode="StartsWith" OnValueChanged="PayTyp_ValueChanged" RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" Width="150px">
                                        <ClientSideEvents ValueChanged="HideOrShow" />
                                    </dx:ASPxComboBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="إضافة" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="AddNewPayment" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">من :</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Customer" runat="server" Width="100%">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="مطلوب" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">
                                    &nbsp;</td>
                                <td class="style1" colspan="2">
                                    &nbsp;</td>
                                <td class="style1" colspan="3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dx-al">&nbsp;</td>
                                <td class="style1">&nbsp;</td>
                                <td class="dx-al">

                                    <asp:SqlDataSource ID="Accounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,5)='1.1.3')"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="BankAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,7)='1.1.1.2')"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="AccountsNotPayed" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=5 and (left(ltrim(AccountNo),8)='1.1.10.1') and AccountNo in (Select AccountNum from DailyJournal where SubIns=@Sys)">
                                        <SelectParameters>
                                            <asp:QueryStringParameter QueryStringField="Sys" DefaultValue="0" Name="Sys"></asp:QueryStringParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td class="style1" colspan="5">
                                    <%--SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Payment' order by TPNo"--%>

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
                                <td class="dx-al">مقابل سداد :</td>
                                <td class="style1" colspan="7" rowspan="2">
                                    <dx:ASPxCheckBox ID="CheckCust" Text="عرض جميع الوثائق" runat="server" ClientVisible="false"
                                        OnCheckedChanged="Unnamed1_CheckedChanged" AutoPostBack="True">
                                    </dx:ASPxCheckBox>
                                    <dx:ASPxGridView ID="GridData" runat="server" Width="100%">
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False"></FilterControl>
                                        </SettingsPopup>
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="PolNo" Caption="رقم الوثيقة"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EndNo" Caption="رقم الملحق"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="LoadNo" Caption="رقم الإشعار"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TOTPRM" Caption="اجمالي القسط"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TpName" Caption="العملة"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Commissioned" Caption="العمولة - المسوق"></dx:GridViewDataColumn>
                                        </Columns>
                                        <SettingsPager PageSize="10" />
                                    </dx:ASPxGridView>
                                    <dx:ASPxTextBox ID="Note" runat="server" Width="100%" Text="/" AutoResizeWithContainer="true">
                                        <ValidationSettings ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="مطلوب" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al" style="vertical-align: bottom;">ملاحظات :</td>
                            </tr>
                            <tr>
                                <td class="dx-al">&nbsp;</td>
                                <td class="style1" colspan="7">
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
                                <td class="style1" colspan="7">
                                    <dx:ASPxTextBox ID="AccNo" runat="server" Caption="رقم الصك/الإشعار :"
                                        ClientInstanceName="AccNo" ClientVisible="False" Text="/" Width="170px">
                                        <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="مطلوب" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">&nbsp;</td>
                                <td colspan="7">
                                    <dx:ASPxTextBox ID="Bank" ClientInstanceName="Bank" runat="server" Width="170px"
                                        Text="/" Caption="على مصـرف :" ClientVisible="false">
                                        <ValidationSettings Display="Dynamic" ValidationGroup="ValidGroup">
                                            <RequiredField IsRequired="True" ErrorText="مطلوب" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">&nbsp;</td>
                                <td colspan="2">
                                    <dx:ASPxButton ID="sdad" runat="server" AutoPostBack="False" UseSubmitBehavior="False"
                                        ClientInstanceName="sdad" Text="موافق">
                                        <ClientSideEvents Click="Sadad" />
                                    </dx:ASPxButton>
                                </td>
                                <td colspan="5">
                                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="خروج">
                                        <ClientSideEvents Click="function(s, e) { ReturnToParentPage(); }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>