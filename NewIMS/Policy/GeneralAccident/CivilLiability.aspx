<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CivilLiability.aspx.vb" Inherits="CivilLiability" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function END_OnEndCallback(s,e) {
            SumInsured.SetValue(e.result);
            //xtraBroker.OnEndCallback(s);
        }
        function OnChange(s, e) {
            //console.log("qtyprice");
            if (s.GetValue() == null) {
                s.SetValue(0);
            }
            scbp.PerformCallback('SuminsCHANGE');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="70" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dx-al">
                                    <uc1:PolicyControl runat="server" ID="PolicyControl" />
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">
                                    <dx:ASPxCallback ID="ASPxCallbackPanel1" ClientInstanceName="scbp" OnCallback="ASPxCallbackPanel1_Callback" runat="server">
                                        <ClientSideEvents CallbackComplete="END_OnEndCallback" />
                                    </dx:ASPxCallback>
                                    <table border="0" dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td><span>للشخص الواحد</span><span lang="AR-SA"> </span><span>:</span></td>
                                            <td>
                                                <dx:ASPxTextBox ID="SumInsForOne" runat="server" ClientInstanceName="SumInsForOne" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>للكارثة مهما بلغ عدد الأشخاص :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="SumInsForAll" runat="server" ClientInstanceName="SumInsForAll" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>للأضرار المادية عن حادث أو أكثر ناشئ عن سبب واحد :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="SumInsPeracc" runat="server" ClientInstanceName="SumInsPeracc" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>خلال مدة التأمين بأكملها فيما يخص البنوذ أعلاه :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="SumInsPerPol" runat="server" ClientInstanceName="SumInsPerPol" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" LostFocus="function(s, e) { OnChange(s, e); }" ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>قيمة العقد :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="ContractSumIns" runat="server" ClientInstanceName="ContractSumIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" LostFocus="function(s, e) { OnChange(s, e); }" ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>صاحب العمل :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="JobOwner" runat="server" ClientInstanceName="Owner" CssClass="1" Text="/" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>اسم المشروع :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="ProjectName" runat="server" ClientInstanceName="ProjectName" CssClass="1" Text="/" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>التلوث :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="PollutionIns" runat="server" ClientInstanceName="PollutionIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" LostFocus="function(s, e) { OnChange(s, e); }" ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>تغطي المسئولية القانونية الناشئة عن قيام المؤمن له بأعماله :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="SevRespSumIns" runat="server" ClientInstanceName="SevRespSumIns" CssClass="1" Width="320px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">

                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>التحمل :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="RespVal" runat="server" ClientInstanceName="RespVal" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="ValidateS" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>شروط خاصة :</td>
                                            <td>
                                                <dx:ASPxTokenBox ID="Conds" runat="server" Tokens="" ValueSeparator="-" TextSeparator="-" AllowMouseWheel="True" CallbackPageSize="10" Width="100%" RightToLeft="True">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }"></ClientSideEvents>

                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data"></ValidationSettings>
                                                </dx:ASPxTokenBox>

                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>ملاحظات :</td>
                                            <td>
                                                <dx:ASPxTokenBox ID="Notes" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>إجمالي مبلغ التأمين :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="SumInsured" runat="server" ClientInstanceName="SumInsured" ClientEnabled="false" CssClass="2" DisplayFormatString="n3" Text="0" Width="130px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>السعر :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="Rate" runat="server" ClientInstanceName="Rate" CssClass="5" Text="0" Width="110px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc');}"
                                                        TextChanged="ValidateS" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,8}" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="ValidateS" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientEnabled="False" ClientInstanceName="Premium" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>
                                                    <br />
                                                </div>
                                                &nbsp;
                                          <asp:TextBox ID="INT" runat="server" Text="1" Visible="False" Width="101px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
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