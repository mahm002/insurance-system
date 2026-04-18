<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="CustomersForm.aspx.vb" Inherits="CustomersForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>الزبائن والوكلاء </title>
    <script src="../scripts/Scripts.js"></script>
    <script src="../scripts/jquery-latest.min.js"></script>
    <script src="../scripts/jquery.min.js"></script>
    <script lang="javascript" type="text/javascript">
        $(document).on('keyup', 'input', function (e) {
            if (e.keyCode == 13 && e.target.type !== 'submit') {
                var inputs = $(e.target).parents("form").eq(0).find('input,a,select,button,textarea').filter(':visible:enabled');
                idx = inputs.index(e.target);
                if (idx == inputs.length - 1) {
                    inputs[0].select()
                } else {
                    inputs[idx + 1].focus();
                    inputs[idx + 1].select();
                }
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="ArabicForm" style="z-index: 101">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents EndCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="100" ShowImage="false" Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dx-al">رقـم الزبون</td>
                                <td>
                                    <dx:ASPxTextBox runat="server" Width="100px" ID="CustNo">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="  أرقام فقط " ValidationExpression="^[0-9]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeCaptionHACSys" colspan="2">
                                    <strong>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ForeColor="Green" RightToLeft="True" Visible="False">
                                        </dx:ASPxLabel>
                                    </strong>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">إسـم الزبون</td>
                                <td>
                                    <dx:ASPxTextBox runat="server" Width="350px" ID="CustName">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءؤإآأئ /-\\-]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dx-al">الإسم اللاتيني</td>
                                <td>
                                    <dx:ASPxTextBox ID="CustNameE" runat="server" Width="350px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءؤإآأئ /.-\\-]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dx-al">رقم التعريف</td>
                                <td>
                                    <dx:ASPxTextBox ID="IDNo" runat="server" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">رقم رخصة القيادة</td>
                                <td style="width: 125px; height: 21px">
                                    <dx:ASPxTextBox ID="DrCardNo" runat="server" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">رقم الهاتف</td>
                                <td>
                                    <dx:ASPxTextBox ID="TelNo" runat="server" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9 + -]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">رقم الفاكس</td>
                                <td style="width: 125px; height: 21px">
                                    <dx:ASPxTextBox ID="FaxNo" runat="server" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">العنوان</td>
                                <td>
                                    <dx:ASPxTextBox ID="Address" runat="server" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا,-يءإأئ /-\\-]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">&nbsp;</td>
                                <td style="width: 125px; height: 21px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dx-al">البريد الإلكتروني</td>
                                <td>
                                    <dx:ASPxTextBox runat="server" Width="100%" ID="eMail">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="حروف فقط" ValidationExpression="^[A-Za-z0-9.-_ \\@-.]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">&nbsp;</td>
                                <td style="width: 125px; height: 21px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dx-al">حالات خاصة</td>
                                <td colspan="3">
                                    <dx:ASPxComboBox ID="SpCase" runat="server" ClientInstanceName="SpCase" DataSourceID="Spc" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" SelectedIndex="0"
                                        RightToLeft="True" TextField="TpName" ValueField="TpNo" Width="100%">
                                        <ValidationSettings SetFocusOnError="True">
                                            <RequiredField ErrorText="مطلوب اسم الزبون" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">رقم الحساب</td>
                                <td colspan="3">
                                    <dx:ASPxComboBox ID="AccNo" runat="server" ClientInstanceName="AccNo" DataSourceID="Accounts" DropDownStyle="DropDownList" NullText="/"
                                        IncrementalFilteringMode="Contains" RightToLeft="True" TextField="AccountName" ValueField="AccountNo" Width="100%" SelectedIndex="-1">
                                        <%-- <ValidationSettings SetFocusOnError="True">
                            <RequiredField ErrorText="مطلوب اسم الزبون" IsRequired="True" />
                        </ValidationSettings>--%>
                                        <Columns>
                                            <dx:ListBoxColumn Caption="رقم الحساب" FieldName="AccountNo">
                                            </dx:ListBoxColumn>
                                            <dx:ListBoxColumn Caption="اسم الحساب" FieldName="AccountName">
                                            </dx:ListBoxColumn>
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">
                                    <asp:SqlDataSource ID="Spc" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select TPName,TPNo from EXTRAINFO where TP='special'"></asp:SqlDataSource>
                                </td>
                                <td colspan="3">
                                    <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {
                                                                cbp.PerformCallback('Apply'); }" />
                                    </dx:ASPxButton>
                                    <asp:SqlDataSource ID="Accounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSAccConnectionString %>"
                                        SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4 and (left(AccountNo,7)='1.1.3.1')">
                                        <SelectParameters>
                                            <asp:SessionParameter DefaultValue="1" Name="Br" SessionField="BranchAcc" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
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