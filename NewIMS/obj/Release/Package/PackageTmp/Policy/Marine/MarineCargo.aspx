<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="MarineCargo.aspx.vb" Inherits="MarineCargo" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%-- <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" ClientInstanceName="loadingPanel" runat="server" Text="احتساب&amp;hellip;" Modal="true"  HorizontalAlign="Center" VerticalAlign="Middle"></dx:ASPxLoadingPanel> --%>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="0" />
                <ClientSideEvents />
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
                                    <table border="0" dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td class="auto-style2" colspan="6">
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" Width="100%" 
                                                    ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="function(s, e) { if (cbp.InCallback()) return;
                                                        cbp.PerformCallback('Calc'); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">اسم المورد :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="Importer" runat="server" ClientInstanceName="Importer" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا.,&-يءإؤةأئ /-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">رقم العقد :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="ContractNo" runat="server" ClientInstanceName="ContractNo" CssClass="1" RightToLeft="False" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا.,&-يءإؤةأئ /-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">عدد الشحنات :</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="Loads" runat="server" ClientInstanceName="Loads" MaxValue="1000" MinValue="1" Number="1" RightToLeft="True" Width="100px">
                                                    <SpinButtons Position="Left" ShowLargeIncrementButtons="True">
                                                    </SpinButtons>
                                                    <ClientSideEvents NumberChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>رقم الفاتورة :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="InvNo" runat="server" ClientInstanceName="InvNo" CssClass="1" Width="100%" RightToLeft="False">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا.,&-يءإؤةأئ /-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>الكمية :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="Quantity" runat="server" ClientInstanceName="Quantity" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا.,&-يءإؤةأئ /-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>رقم الاعتماد :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="SurNo" runat="server" ClientInstanceName="SurNo" CssClass="1" Width="180px">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا.,&-يءإؤةأئ /-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>المصرف :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="Bank" runat="server" ClientInstanceName="Bank" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإؤةأئ /.-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">العمولة %:</td>
                                            <td>
                                                <dx:ASPxTextBox ID="BankCommision" runat="server" ClientInstanceName="BankCommision" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>القيمة المستندية :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="DocVal" runat="server" ClientInstanceName="DocVal" CssClass="2" 
                                                    Text="0" Width="180px" DisplayFormatString="n3">
                                                    <ClientSideEvents LostFocus="LocalExRateCall"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>نوع العملة :</td>
                                            <td>
                                                <dx:ASPxComboBox ID="Currency" runat="server" ClientInstanceName="Currency"
                                                    DataSourceID="Cur1" DropDownStyle="DropDown" ValueType="System.Int32"
                                                    RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" Width="110px">
                                                    <ClientSideEvents SelectedIndexChanged="LocalExRateCall" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td class="auto-style3">سعر الصرف :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="ExcRate" runat="server" ClientInstanceName="ExcRate" CssClass="5" Text="1" Width="109px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('SumInsCalc'); }" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,8}" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>مبلغ التأمين :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="SumIns" runat="server" ClientInstanceName="SumIns" CssClass="2" Text="0" Width="180px" DisplayFormatString="n3">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxTextBox ID="NewSumIns" runat="server" ClientInstanceName="NewSumIns" 
                                                    CssClass="2" Text="0" Width="100%" ClientEnabled="false" DisplayFormatString="n3">
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">هامش إضافي :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxSpinEdit ID="Margin" runat="server" ClientInstanceName="Margin" MaxValue="1000" Number="0" RightToLeft="True" Width="100px">
                                                    <SpinButtons Position="Left" ShowLargeIncrementButtons="True">
                                                    </SpinButtons>
                                                    <ClientSideEvents NumberChanged="Validate" ValueChanged="function(s, e) {
                                                                cbp.PerformCallback('SumInsCalc'); }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>التحمل :</td>
                                            <td colspan="5">
                                                <dx:ASPxTextBox ID="RespVal" runat="server" ClientInstanceName="RespVal" CssClass="1" Width="100%" Text="0.5 %من قيمة الشحنة بحد أدنى 5000 لكل شحنة">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>وسيلة النقل :</td>
                                            <td>
                                                <dx:ASPxTokenBox runat="server" Tokens="" AllowMouseWheel="True" AllowCustomTokens="false" CallbackPageSize="10" DataSourceID="MarB" TextField="TPName" ValueField="TPNo" Width="100%" RightToLeft="True" ID="MarBy">
                                                    <ClientSideEvents TokensChanged="Validate"></ClientSideEvents>
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                            <td>السفـــــــينة :</td>
                                            <td colspan="3">
                                                <dx:ASPxGridLookup ID="ShipNo" runat="server" AutoGenerateColumns="False" AutoResizeWithContainer="True" ClientInstanceName="ShipNo" DataSourceID="Ships" KeyFieldName="ShipNo" PopupHorizontalAlign="Center" RenderIFrameForPopupElements="True" RightToLeft="True" TextFormatString="{0} | {1} | {3}" Width="100%">
                                                    <GridViewProperties>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                                        <SettingsPopup>
                                                            <FilterControl AutoUpdatePosition="False">
                                                            </FilterControl>
                                                        </SettingsPopup>
                                                        <SettingsCommandButton>
                                                            <NewButton Text="سفينة جديدة">
                                                            </NewButton>
                                                            <UpdateButton Text="حفظ">
                                                            </UpdateButton>
                                                            <CancelButton Text="إلغاء">
                                                            </CancelButton>
                                                        </SettingsCommandButton>
                                                    </GridViewProperties>
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" ShowNewButton="True" VisibleIndex="0">
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn Caption="رقم السفينة" FieldName="ShipNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                                            <EditFormSettings Visible="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="اسم السفينة" FieldName="ShipName" ShowInCustomizationForm="True" VisibleIndex="2">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="سنة الصتع" FieldName="MadeYear" ShowInCustomizationForm="True" VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="جنسيتها" FieldName="Nation" ShowInCustomizationForm="True" VisibleIndex="4">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="نوعها" FieldName="Type" ShowInCustomizationForm="True" VisibleIndex="5">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="مادة الصنع" FieldName="Material" ShowInCustomizationForm="True" VisibleIndex="6">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ClientSideEvents DropDown="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" />
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxGridLookup>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">اتجاه الرحلة من :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="TripFrom" runat="server" ClientInstanceName="TripFrom" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءإؤةأئ /.-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">إلى :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="TripTo" runat="server" ClientInstanceName="TripTo" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءإؤةأئ /.-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style4">ومنها إلى :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="TripFromTo" runat="server" ClientInstanceName="TripFromTo" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءئ. \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">تصنيف البضاعة :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="GodCat" runat="server" ClientInstanceName="GodCat" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءإؤةأئ /.-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">نوع البضاعة :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="GodType" runat="server" ClientInstanceName="GodType" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءإؤةأئ /.-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style4">نوع التعبئة :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="FillType" runat="server" ClientInstanceName="FillType" CssClass="1" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءإؤةأئ /.-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">شروط التغطية :</td>
                                            <td class="auto-style1" colspan="5">
                                                <dx:ASPxComboBox ID="InsCond" runat="server" ClientInstanceName="InsCond"
                                                    DataSourceID="InsConds" DropDownStyle="DropDown" ValueType="System.Int32"
                                                    RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" Width="100%">
                                                    <ClientSideEvents GotFocus="function(s, e) {  }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>الشروط الخاصة :</td>
                                            <td colspan="5">
                                                <dx:ASPxTokenBox ID="SpcCond" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="- "
                                                    ItemValueType="System.String" ValueSeparator="- "
                                                    Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="false" />
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td class="dxeCaptionHARSys" colspan="5">للفصل بين الشروط أو التغطيات الإضافية استخدم الرمز -</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>المسافة :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="Distance" runat="server" ClientInstanceName="Distance" CssClass="3" Text="0" Width="110px">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">تاريخ بداية التغطية :</td>
                                            <td>
                                                <dx:ASPxDateEdit ID="GodArrival" runat="server" DisplayFormatString="yyyy/MM/dd" Width="110px" NullText="1900/01/01">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxCheckBox ID="WarCover" runat="server" AccessibilityLabelText="" CheckState="Unchecked" RightToLeft="True" Text="تغطية الحروب">
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td>سعر النقل :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="MarRate" runat="server" ClientInstanceName="MarRate" CssClass="5" Text="0" Width="110px">
                                                    <ClientSideEvents LostFocus="function(s, e) {cbp.PerformCallback('Calc'); }" TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,8}" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">قسط النقل :</td>
                                            <td>
                                                <%--ClientEnabled="False" --%>
                                                <dx:ASPxTextBox ID="MarPrm" runat="server" ClientInstanceName="MarPrm" ClientEnabled="False" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc'); }"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxCheckBox ID="ReLoad" runat="server" AccessibilityLabelText="" CheckState="Unchecked" RightToLeft="True" Text="إعادة شحن">
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td>سعر ك. العمر :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="ShipRate" runat="server" ClientInstanceName="ShipRate" CssClass="5" Text="0" Width="110px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc'); }"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,8}" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">قسط ك. العمر :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="ShipPrm" runat="server" ClientInstanceName="ShipPrm" ClientEnabled="False" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc'); }"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxCheckBox ID="OutLoad" runat="server" AccessibilityLabelText="" CheckState="Unchecked" RightToLeft="True" Text="ش. خارج العنابر">
                                                    <ValidationSettings ErrorTextPosition="Left">
                                                    </ValidationSettings>
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td>سعر إضافي :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="ExtraRate" runat="server" ClientInstanceName="ExtraRate" CssClass="5" Text="0" Width="110px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc'); }"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,8}" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">قسط إضافي :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="ExtraPrm" runat="server" ClientInstanceName="ExtraPrm" ClientEnabled="False" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc'); }"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>سعر الحروب :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="WarRate" runat="server" ClientInstanceName="WarRate" CssClass="5" Text="0" Width="110px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc'); }"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,8}" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style3">قسط الحروب :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="WarPrm" runat="server" ClientInstanceName="WarPrm" ClientEnabled="False" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents LostFocus="function(s, e) {
                                                                cbp.PerformCallback('Calc'); }"
                                                        TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True">
                                                        <RegularExpression ErrorText="أرقام فقط" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:SqlDataSource ID="Ships" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    InsertCommand="NewShip"
                                                    InsertCommandType="StoredProcedure"
                                                    SelectCommand="SELECT ShipNo, ShipName, MadeYear, Nation, Type, Material FROM [ShipFile]">
                                                    <InsertParameters>
                                                        <asp:Parameter Name="ShipName" Type="String" />
                                                        <asp:Parameter Name="MadeYear" Type="Int16" />
                                                        <asp:Parameter Name="Nation" Type="String" />
                                                        <asp:Parameter Name="Type" Type="String" />
                                                        <asp:Parameter Name="Material" Type="String" />
                                                    </InsertParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="Condss" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT [CondNo], [Condition] FROM [CondFile] WHERE ([SubSys] = @SubSys)">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter DefaultValue="04" Name="SubSys" QueryStringField="Sys" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="InsConds" runat="server" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>'
                                                    SelectCommand="SELECT TPNo, TPName, Accessor FROM EXTRAINFO WHERE ((TP = 'MarInsCond') AND (Accessor = @Accessor)) ORDER BY TPNo">
                                                    <SelectParameters>

                                                        <asp:QueryStringParameter QueryStringField="Sys" DefaultValue="MC" Name="Accessor" Type="String"></asp:QueryStringParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="sCondss" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='MarSpcCond' Order By TPNo">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter DefaultValue="MC" Name="SubSys" QueryStringField="Sys" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="MarB" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='MarBy' order by TPNo"></asp:SqlDataSource>
                                                <asp:SqlDataSource ID="Banks" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='Banks' order by TPNo"></asp:SqlDataSource>
                                                <asp:SqlDataSource ID="Cur1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='Cur' order by TPNo"></asp:SqlDataSource>
                                                &nbsp;
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