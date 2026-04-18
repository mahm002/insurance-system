<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResetPassword.aspx.vb" Inherits=".ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        document.addEventListener("contextmenu", function (e) {
            e.preventDefault();
        }, false);
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
              <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback1">
                <ClientSideEvents EndCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="100" ShowImage="false" Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table dir="rtl" style="width: 100%">
                            <tr>
                                <td class="auto-style1" dir="rtl" style="text-align: left">
                                    <dx:ASPxTextBox ID="AccountName" runat="server" Caption="اسم المستخدم" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td dir="ltr">
                                    <asp:SqlDataSource ID="Branches" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"></asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1" dir="rtl">
                                    <dx:ASPxTextBox ID="PassWord" runat="server" Caption="كلمة المرور" Password="True" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="حروف وأرقام وأن لا تحتوي على مسافات" ValidationExpression="^[0-9a-zA-Z#@ا-ي\s]+" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style2" dir="ltr" style="text-align: right">
                                    <dx:ASPxLabel ID="lblMessage" runat="server">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeCaptionHARSys" dir="rtl">
                                    <dx:ASPxTextBox ID="ConfirmPass" runat="server" Caption="تأكيد كلمة المرور" Password="True" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="حروف وأرقام وأن لا تحتوي على مسافات" ValidationExpression="^[0-9a-zA-Z#@ا-ي\s]+" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeCaptionHARSys" dir="ltr">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2" class="dx-ac">
                                    <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ" UseSubmitBehavior="False" Width="100%">
                                        <ClientSideEvents Click="function(s, e) {
                                cbp.PerformCallback('Apply'); }" />
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
