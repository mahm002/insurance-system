<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Mc.aspx.vb" Inherits="Mc" %>

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
        <div class="dx-ac">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents EndCallback="" />
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
                                            <td>رقم اللوحة :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="TableNo" runat="server" ClientInstanceName="TableNo" CssClass="1" Width="200px">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءئ \\-]+" ErrorText=" ( - حروف وأرقام (يحوي علامة " />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="2">رقم الهيكل :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="BudyNo" runat="server" ClientInstanceName="BudyNo" CssClass="1" Width="200px">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="حروف وأرقام" ValidationExpression="^[A-Za-z0-9 \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>نوع المركبة :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="CarType" runat="server" ClientInstanceName="CarType" CssClass="1" Width="110px">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="true" Display="Dynamic">
                                                        <RegularExpression ErrorText="حروف وأرقام" ValidationExpression="^[A-Za-z0-9ا-يءأئ ى \\%/()-.]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="2">اللون :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="CarColor" runat="server" ClientInstanceName="CarColor" CssClass="1" Width="110px">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="true" Display="Dynamic">
                                                        <RegularExpression ErrorText="حروف فقط" ValidationExpression="^[A-Za-z0-9ا-يءأئ ى \\%/()-.]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">سنة الصنع :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="MadeYear" runat="server" ClientInstanceName="MadeYear" CssClass="3" Width="110px">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="true" Display="Dynamic">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="dx-al" colspan="2">عدد الركاب :</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="PassNo" runat="server" ClientInstanceName="PassNo" MaxValue="100" Number="4" Width="110px" AutoPostBack="false"
                                                    RightToLeft="True">
                                                    <SpinButtons Position="left" ShowLargeIncrementButtons="True" />
                                                    <ClientSideEvents NumberChanged="ValidateS" />
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">القوة بالحصان :</td>
                                            <td class="dx-al">
                                                <dx:ASPxSpinEdit ID="Power" runat="server" ClientInstanceName="Power" MaxValue="1500" Number="0" Width="110px" RightToLeft="True" AutoPostBack="false">
                                                    <SpinButtons Position="left" ShowLargeIncrementButtons="True" />
                                                    <ClientSideEvents NumberChanged="ValidateS" />
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td class="dx-al" colspan="2">الحمولة بالطن :</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="Carry" runat="server" ClientInstanceName="Carry" MaxValue="100" Number="0" Width="110px" RightToLeft="True" DecimalPlaces="1" NumberType="Float">
                                                    <SpinButtons Position="left" ShowLargeIncrementButtons="True" />
                                                    <ClientSideEvents NumberChanged="ValidateS" />
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">جهة الترخيص :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="PermZone" runat="server" ClientInstanceName="PermZone" CssClass="1" Width="200px" Text="طرابلس">
                                                    <ValidationSettings ValidationGroup="Data" Display="Dynamic">
                                                        <RegularExpression ErrorText="حروف وأرقام" ValidationExpression="^[A-Za-zا-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="2">ملاحظات</td>
                                            <td>
                                                <dx:ASPxTextBox ID="Notes" runat="server" ClientInstanceName="Notes" CssClass="1" Width="200px" Text="/">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="*" ValidationExpression="^[A-Za-z0-9ا/-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">نوع الترخيص :</td>
                                            <td colspan="2">
                                                <dx:ASPxComboBox ID="PermType" runat="server" ClientInstanceName="PermType" DataSourceID="PermType1" SelectedIndex="0"
                                                    DropDownStyle="DropDownList" RightToLeft="True" TextField="TPName" ValueField="TPNo" Width="450px">
                                                    <ClientSideEvents SelectedIndexChanged="ValidateS" />
                                                    <ValidationSettings ValidationGroup="Data" Display="Dynamic">
                                                        <RequiredField ErrorText="نوع الترخيص مطلوب" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>جنسية المركبة :</td>
                                            <td>
                                                <dx:ASPxComboBox ID="Nation" runat="server" ClientInstanceName="Nation" DataSourceID="Nate1" SelectedIndex="0"
                                                    DropDownStyle="DropDownList" RightToLeft="True" TextField="Accessor" ValueField="TPNo" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="ValidateS" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField ErrorText="جنسية المركبة مطلوبة" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:SqlDataSource ID="PermType1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='carperm' Order By TpNo"></asp:SqlDataSource>
                                            </td>
                                            <td colspan="2">
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="ValidateS" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientVisible="false" ClientEnabled="False" ClientInstanceName="Premium" CssClass="2" Text="0" Width="110px" DisplayFormatString="n3">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,3}" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:SqlDataSource ID="Nate1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT TPNo, RTRIM(Accessor)+'ة' As Accessor FROM EXTRAINFO where tp='NatE' ORDER BY TPNo"></asp:SqlDataSource>
                                            </td>
                                            <td colspan="2">
                                                <dx:ASPxButton ID="issue" runat="server" AutoPostBack="False" ClientInstanceName="issue" Enabled="False" Text="إصدار" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {
                                                                cbp.PerformCallback('Issue'); }" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style1">&lt;== يمكنك الإصدار من هنا في حال التأكد من صحة البيانات</td>
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