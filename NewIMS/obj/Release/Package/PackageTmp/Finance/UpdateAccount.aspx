<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateAccount.aspx.vb" Inherits=".UpdateAccount" %>

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
                   //debugger;

                   //alert(accno.GetText());
                   //alert(accnoh.GetText());
                   if (ASPxClientEdit.ValidateGroup('Data'))
                       accnoh.SetValue(accno.GetText());
                       cbp.PerformCallback('Save');

               }
               function ReturnToParentPage() {
                   var parentWindow = window.parent;
                   parentWindow.SelectAndClosePopup(1);
               }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 30px;
        }

        .auto-style2 {
            height: 29px;
        }

        .auto-style3 {
            width: 488px;
        }

        .auto-style4 {
            height: 30px;
            width: 488px;
        }

        .auto-style5 {
            height: 29px;
            width: 488px;
        }
    </style>
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
                                    <td class="auto-style3">
                                        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="370px" ClientEnabled="False" Border-BorderStyle="None" Caption="الحساب الأب">
                                            <Border BorderStyle="None"></Border>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Caption="رقم الحساب الفرعي" ClientEnabled="False" ClientInstanceName="accno" HorizontalAlign="Center" Width="170px" ClientVisible="false">
                                            <MaskSettings ErrorText="تأكد من إدخال رقم الحساب كاملاً" />
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style4">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Green" RightToLeft="True">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="auto-style1">
                                        <dx:ASPxTextBox ID="AccountName" runat="server" Caption="اسم الحساب الفرعي" Width="100%">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإآؤأئ /)(-\\-]+" />
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style5">
                                        <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ClientInstanceName="accnoh" ClientVisible="False" HorizontalAlign="Center" Width="170px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="auto-style2">
                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="خروج" Visible="False" Width="30px">
                                            <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();
                        }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ" UseSubmitBehavior="False" Width="30px">
                                            <ClientSideEvents Click="ValidateS" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ForeColor="Green" RightToLeft="True" Visible="False">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>&nbsp;</td>
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