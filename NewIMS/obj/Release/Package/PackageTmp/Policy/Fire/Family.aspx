<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Family.aspx.vb" Inherits="Family" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function END_OnEndCallback(s,e) {
            SumIns.SetValue(e.result);
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

    <style type="text/css">
        .auto-style1 {
            height: 27px;
        }

        .auto-style2 {
            height: 27px;
            text-align: center;
        }

        .auto-style3 {
            height: 27px;
            text-align: left;
        }

        .auto-style4 {
            height: 27px;
            direction: ltr;
        }

        .auto-style5 {
            height: 27px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="70" />
                <PanelCollection>
                    <dx:PanelContent runat="server">

                        <table id="TableForm" dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dx-al">
                                    <uc1:PolicyControl runat="server" ID="PolicyControl" />
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">
                                    <%--                              <dx:ASPxRoundPanel runat="server" AllowCollapsingByHeaderClick="True" ID="PolicyData" RightToLeft="True" Width="100%" HorizontalAlign="Center">
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">--%>
                                    <table dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td class="auto-style3" colspan="2">&nbsp;</td>
                                            <td class="auto-style1">&nbsp;</td>
                                            <td class="auto-style2" colspan="4">بيانات المبنى</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">نوع المبنى :<br class="Apple-interchange-newline" />
                                            </td>
                                            <td class="auto-style1" colspan="4">
                                                <dx:ASPxTextBox ID="FaBldTyp" runat="server" ClientInstanceName="FaBldTyp" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">عدد الأدوار :</td>
                                            <td class="auto-style1" colspan="4">
                                                <dx:ASPxTextBox ID="FaFloors" runat="server" ClientInstanceName="FaFloors" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">نوع السقف :</td>
                                            <td class="auto-style1" colspan="4">
                                                <dx:ASPxTextBox ID="FaCieling" runat="server" ClientInstanceName="FaCieling" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">الغرض من الاستعمال :</td>
                                            <td class="auto-style1" colspan="4">
                                                <dx:ASPxTextBox ID="FaPurpos" runat="server" ClientInstanceName="FaPurpos" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">موقع المبنى :</td>
                                            <td class="auto-style1" colspan="4">
                                                <dx:ASPxTextBox ID="FaPlace" runat="server" ClientInstanceName="FaPlace" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style2">حدود المبنى&nbsp; </td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="North" runat="server" ClientInstanceName="North" CssClass="1" Width="95%" Caption="شمالاً">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="South" runat="server" ClientInstanceName="South" CssClass="1" Width="95%" Caption="جنوباً">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="East" runat="server" ClientInstanceName="East" CssClass="1" Width="95%" Caption="شؤقاً">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="West" runat="server" ClientInstanceName="West" CssClass="1" Width="95%" Caption="غرباً">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">مبالغ التأمين</td>
                                            <td class="auto-style3">المبنى :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="BuildIns" runat="server" ClientInstanceName="BuildIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">المحتوى :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="BuildContIns" runat="server" ClientInstanceName="BuildContIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">&nbsp;</td>
                                            <td class="auto-style3">المجوهرات :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="GoldIns" runat="server" ClientInstanceName="GoldIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">الوفاة أو العجز الكلي الدائم :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="DeathIns" runat="server" ClientInstanceName="DeathIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">المسؤوليات</td>
                                            <td class="auto-style5">المسؤولية من قبل المالك / المؤجر :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="OwnerIns" runat="server" ClientInstanceName="OwnerIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">المسؤولية قبل الجيران :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="Behind" runat="server" ClientInstanceName="Behind" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">المستفيدوت في حالة الوفاة :</td>
                                            <td class="auto-style1" colspan="3">
                                                <dx:ASPxTextBox ID="FaBenef" runat="server" ClientInstanceName="FaBenef" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9/اأإآ-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style1">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">&nbsp;</td>
                                            <td class="auto-style1">
                                                <dx:ASPxCheckBox ID="RahnCond" runat="server" AccessibilityLabelText="" CheckState="Unchecked" RightToLeft="True" Text="شرط الرهن">
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td class="auto-style1"></td>
                                            <td class="auto-style3">إجمالي مبالغ التأمين :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="SumIns" runat="server" ClientEnabled="False" ClientInstanceName="SumIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">بند التحمل :</td>
                                            <td class="auto-style1" colspan="4">
                                                <dx:ASPxTextBox ID="RespVal" runat="server" ClientInstanceName="RespVal" CssClass="1" RightToLeft="True" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءأئ ى \\%/()-.]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                            <td class="auto-style3">ملاحظات :</td>
                                            <td class="auto-style1" colspan="4">
                                                <dx:ASPxTokenBox ID="Notes" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3" colspan="3">السعر :</td>
                                            <td class="auto-style1" colspan="4">
                                                <dx:ASPxTextBox ID="Rate" runat="server" ClientInstanceName="Rate" CssClass="5" Text="0" Width="110px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc');}"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,9}" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <dx:ASPxCallback ID="ASPxCallbackPanel1" ClientInstanceName="scbp" OnCallback="ASPxCallbackPanel1_Callback" runat="server">
                                                <ClientSideEvents CallbackComplete="END_OnEndCallback" />
                                            </dx:ASPxCallback>
                                            <td class="auto-style3" colspan="3">&nbsp;&nbsp;القسط :</td>
                                            <td class="auto-style1" colspan="3">
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientEnabled="False" ClientInstanceName="Premium" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"></td>
                                            <td class="auto-style4" colspan="2">
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="ValidateS" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style1" colspan="4">&nbsp;</td>
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