<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CashInTransit.aspx.vb" Inherits="CashInTransit" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <style type="text/css">
        .auto-style2 {
            height: 18px;
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
                                    <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td class="auto-style2"><span lang="AR-SA"><strong>المبلغ التقديري للأموال المتوقع نقلها خلال مدة التأمين </strong> </span><span><strong>:</strong></span></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="CashYear" runat="server" ClientInstanceName="CashYear" CssClass="2" Text="0" Width="200px">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"><strong>حدود المسؤولية للنقلة الواحدة :</strong></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="CashOne" runat="server" ClientInstanceName="CashOne" CssClass="2" Text="0" Width="200px">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"><strong>حد المسئولية خلال فترة التأمين :</strong></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="SumIns" runat="server" ClientInstanceName="SumIns" CssClass="2" Text="0" Width="200px">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"><strong>بند التحمل :</strong></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="RespVal" runat="server" ClientInstanceName="RespVal" CssClass="1" Text="/" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"><strong>الشروط والاستثناءات :</strong></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTokenBox ID="Conds" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"><strong>ملاحظات :</strong></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTokenBox ID="Notes" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"><strong>المجال الجغرافي :</strong></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="Zone" runat="server" ClientInstanceName="Zone" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"><strong>السعر :</strong></td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="Rate" runat="server" ClientInstanceName="Rate" CssClass="5" Text="0" Width="200px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc');}"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,8}" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientEnabled="False" ClientInstanceName="Premium" CssClass="2" Text="0" Width="200px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
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