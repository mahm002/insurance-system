<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NewAccount.aspx.vb" Inherits=".NewAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../scripts/Scripts.js"></script>
    <script src="../scripts/jquery-latest.min.js"></script>
    <script src="../scripts/jquery.min.js"></script>
    <script type="text/javascript">

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

               function ValidateS(s, e) {
                   //if (ASPxClientEdit.ValidateGroup('Data'))
                       //AccNo.SetValue(accno.GetText());
                       cbp.PerformCallback('Save');

               }
               function ReturnToParentPage() {
                   var parentWindow = window.parent;

                   parentWindow.SelectAndClosePopup(1);
               }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="dx-ar">
                <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback" Width="100%">
                    <ClientSideEvents EndCallback="" />
                    <SettingsLoadingPanel Text="حفظ&amp;hellip;" Delay="100" ShowImage="false" Enabled="false" />
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table id="TableForm" dir="rtl" style="width: 100%;">
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ClientInstanceName="accnoh" ClientVisible="False" HorizontalAlign="Center" Width="170px">
                                        </dx:ASPxTextBox>
                                        <dx:ASPxTextBox ID="ParentAcc" runat="server" Caption=" الحساب الأب" ClientEnabled="False" RightToLeft="True" Width="100%">
                                            <Border BorderStyle="None" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" ForeColor="Green" RightToLeft="True" Font-Bold="true" Font-Size="Larger">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <%-- ClientVisible="false"--%>
                                        <dx:ASPxTextBox ID="AccountNo" runat="server" Caption="رقم الحساب التحليلي" ClientInstanceName="AccNo" ClientVisible="false"
                                            HorizontalAlign="Center" Width="170px">
                                            <MaskSettings ErrorText="تأكد من إدخال رقم الحساب كاملاً" />
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="AccountName" runat="server" Caption=" اسم الحساب الجديد :" Width="100%">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإآؤأئ /)(-\\-]+" />
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <dx:ASPxRadioButtonList ID="TransOrParent" runat="server" RepeatDirection="Horizontal" SelectedIndex="0" ClientVisible="false"
                                            ValueType="System.Int32" Width="100%">
                                            <Items>
                                                <dx:ListEditItem Selected="True" Text=" حساب رئيسي (يقبل إضافة أبناء) " Value="0" />
                                                <dx:ListEditItem Text="حساب حركة" Value="1" />
                                            </Items>
                                        </dx:ASPxRadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ForeColor="Green" RightToLeft="True" Visible="False">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False"
                                            ClientInstanceName="btnShow" Text="حفظ" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="ValidateS" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="خروج" Visible="False">
                                            <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();
                        coa.PerformCallback();
                        }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </div>
        </div>
    </form>
</body>
</html>