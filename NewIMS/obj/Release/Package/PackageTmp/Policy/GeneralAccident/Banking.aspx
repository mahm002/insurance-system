<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Banking.aspx.vb" Inherits="Banking" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        p.MsoNormal {
            margin: 0in;
            text-align: right;
            direction: rtl;
            unicode-bidi: embed;
            font-size: 12.0pt;
            font-family: "Times New Roman",serif;
        }

        .auto-style1 {
            height: 18px;
        }

        .auto-style2 {
            text-align: center;
            height: 18px;
        }

        .auto-style3 {
            height: 26px;
        }

        .auto-style5 {
            height: 18px;
            text-align: left;
        }

        .auto-style6 {
            text-align: right;
            height: 26px;
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

                        <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
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
                                            <td class="auto-style3" colspan="4">
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style6">حدود المسئولية للحادث</td>
                                            <td class="auto-style6">حد المسئولية خلال مدة التأمين</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="4"><span>الشرط التأميني الأول ( عدم أمانة الموظفين )&nbsp;:</span></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="SecretAcc" runat="server" ClientInstanceName="SecretAcc" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespSecret" runat="server" ClientInstanceName="RespSecret" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="4">الشرط التأميني الثاني ( في المياني ) :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="BuildingAcc" runat="server" ClientInstanceName="BuildingAcc" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespBuilding" runat="server" ClientInstanceName="RespBuilding" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="4">الشرط التأميني الثالث ( أثناء النقل ) :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="TransAcc" runat="server" ClientInstanceName="TransAcc" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespTrans" runat="server" ClientInstanceName="RespTrans" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="4">الشرط التأميني الرابع ( الصكوك المزورة) :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="CheckAcc" runat="server" ClientInstanceName="CheckAcc" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespCheck" runat="server" ClientInstanceName="RespCheck" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="4">الشرط التأميني الخامس ( العملة المزورة) :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="MonyAcc" runat="server" ClientInstanceName="MonyAcc" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespMony" runat="server" ClientInstanceName="RespMony" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="4">الشرط التأميني السادس (&nbsp; أضرار المكاتب والمحتويات ) :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="OfficeAcc" runat="server" ClientInstanceName="OfficeAcc" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespOffice" runat="server" ClientInstanceName="RespOffice" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="4">الشرط التأميني السابع (&nbsp; آلات السحب الذاتي ) :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="ATMAcc" runat="server" ClientInstanceName="ATMAcc" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespATM" runat="server" ClientInstanceName="RespATM" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="4">الشرط التأميني الثامن (&nbsp; الحروب والإرهاب ) :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="WarAcc" runat="server" ClientInstanceName="WarAcc" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespWar" runat="server" ClientInstanceName="RespWar" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="5">الحد الاقصى لمسئولية الشركة بالنسبة للاضرار عن حادث واحد أو أكثر ناشئ عن سبب واحد :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="SumInsPerAcc" runat="server" ClientInstanceName="SumInsPerAcc" CssClass="2" Text="0" Width="170px">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data" Display="Dynamic">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="3">بند التحمل :</td>
                                            <td class="auto-style1" colspan="3">
                                                <dx:ASPxTextBox ID="RespLimit" runat="server" ClientInstanceName="RespLimit" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="5">التاريخ الرجعي :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxDateEdit ID="DateBack" runat="server" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="110px">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5" colspan="2">السعر :</td>
                                            <td class="auto-style1" colspan="3">
                                                <dx:ASPxTextBox ID="Rate" runat="server" ClientInstanceName="Rate" CssClass="5" Height="17px" Text="0" Width="110px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc');}"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,5}" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientEnabled="False" ClientInstanceName="Premium" CssClass="2" Height="17px" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5">شروط خاصة :</td>
                                            <td class="auto-style1" colspan="5">
                                                <dx:ASPxTokenBox ID="Notes" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
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