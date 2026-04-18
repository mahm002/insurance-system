<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Orange.aspx.vb" Inherits="Orange" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        .auto-style1 {
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <%--<ClientSideEvents BeginCallback="" />--%>
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="10" ShowImage="false" Enabled="false" />
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

                                    <table dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td class="auto-style2">رقم اللوحة :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="TableNo" runat="server" ClientInstanceName="TableNo" CssClass="1" Width="110px">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="حروف وأرقام" ValidationExpression="^[A-Za-z0-9ا-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">رقم الهيكل :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="BudyNo" runat="server" ClientInstanceName="BudyNo" CssClass="1" Width="200px">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9 \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">نوع السيارة :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="CarType" runat="server" ClientInstanceName="CarType" CssClass="1" Width="110px">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="true" Display="Dynamic">
                                                        <RegularExpression ErrorText="حروف فقط" ValidationExpression="^[A-Za-z0-9ا-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">سنة الصنع :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="MadeYear" runat="server" ClientInstanceName="MadeYear" CssClass="3" Width="110px">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>رقعة التغطية :</td>
                                            <td colspan="3">
                                                <dx:ASPxTokenBox ID="AreaCover" runat="server" AllowMouseWheel="True" CallbackPageSize="10"
                                                    AllowCustomTokens="false" TextField="TPName" ValueField="TPNo"
                                                    RightToLeft="True" NullText="(اختر الوجهة)"
                                                    Tokens="," Width="100%" DataSourceID="ArCountry">
                                                    <ClientSideEvents TokensChanged="Validate" KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'PermType'); }" />
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">نوع الترخيص :</td>
                                            <td colspan="3">
                                                <dx:ASPxComboBox ID="PermType" runat="server" ClientInstanceName="PermType" DataSourceID="PermType1" SelectedIndex="0"
                                                    DropDownStyle="DropDownList" RightToLeft="True" TextField="TPName" ValueField="TPNo" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="Validate" />
                                                    <ValidationSettings ValidationGroup="Data" Display="Dynamic">
                                                        <RequiredField ErrorText="نوع الترخيص مطلوب" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientEnabled="False" ClientInstanceName="Premium" CssClass="2" Text="0" Width="110px" DisplayFormatString="n3">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,3}" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <asp:SqlDataSource ID="ArCountry" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='ArCountry' AND type='OR'"></asp:SqlDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxButton ID="issue" runat="server" AutoPostBack="False" ClientInstanceName="issue" Enabled="False" Text="إصدار" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {
                                                                cbp.PerformCallback('Issue'); }" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style1">يمكنك الإصدار من هنا في حال التأكد من البيانات</td>
                                            <td>
                                                <asp:SqlDataSource ID="PermType1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='carperm'"></asp:SqlDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--                   </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>--%>
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