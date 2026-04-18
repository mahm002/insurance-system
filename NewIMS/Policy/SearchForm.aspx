<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchForm.aspx.vb" Inherits="SearchForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function OnToolbarItemClick(s, e) {
            if (IsCustomExportToolbarCommand(e.item.name)) {
                e.processOnServer = true;
                //e.usePostBack = true;
            }

        }

        function OnToolbarItemClick1(s, e) {
            if (IsCustomExportToolbarCommand(e.item.name)) {
                e.processOnServer = true;
                //e.usePostBack = true;
            }

        }

        function IsCustomExportToolbarCommand(command) {
            return command == "NewPolicy" || command == "ExtraSearch";
            //command == "CustomExportToXLS" || command == "CustomExportToXLSX" ||
        }
        function OnEndCallback(s, e) {
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                //window.open(s.cpNewWindowUrl);
                PrintPop.SetSize(1000, 800);
                PrintPop.SetHeaderText('طباعة');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Distribute') {
                //window.open(s.cpNewWindowUrl);
                PrintPop.SetSize(1400, 800);
                PrintPop.SetHeaderText('توزيع الوثيقة');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Edit') {
                //window.open(s.cpNewWindowUrl);
                debugger;
                PrintPop.SetSize(s.cpSize, 900);
                PrintPop.SetHeaderText('تحرير');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'New') {
                //window.open(s.cpNewWindowUrl);
                PrintPop.SetSize(s.cpSize, 900);
                PrintPop.SetHeaderText('وثيقة جديدة');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Renew') {
                //window.open(s.cpNewWindowUrl);
                PrintPop.SetSize(800, 900);
                PrintPop.SetHeaderText('تجديد');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == '') {
                //window.open(s.cpNewWindowUrl);
                PrintPop.SetSize(1400, 900);
                PrintPop.SetHeaderText('REPORT');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'IssuClip') {
                //window.open(s.cpNewWindowUrl);
                PrintPop.SetSize(1400, 900);
                PrintPop.SetHeaderText('حافظة الإنتاج');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'IssuClipOverall') {
                //window.open(s.cpNewWindowUrl);
                PrintPop.SetSize(1400, 900);
                PrintPop.SetHeaderText('سجل الإنتاج على مستوى الشركة');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Search') {
                //window.open(s.cpNewWindowUrl);
                PrintPop.SetSize(1200, 800);
                PrintPop.SetHeaderText('بحث متقدم');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            s.cpMyAttribute = ''
            s.cpNewWindowUrl = null

            UpdateTimeoutTimer();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table dir="rtl" style="width: 100%;">
                <tr>
                    <td style="width: 5%;">
                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="بحث" OnClick="ASPxButton1_Click" />
                    </td>
                    <td class="dx-al" style="width: 95%;">
                        <%--  <dx:ASPxButton ID="search" runat="server" Text="بحث" OnClick="ASPxButton1_Click">
                    </dx:ASPxButton>--%>
                        <dx:ASPxTextBox ID="searchtxt" runat="server" Width="100%" NullText="يمكنك البحث عن أي وثيقة على مستوى الشركة بالكامل بأي بيان له علاقة (المؤمن له ،رقم اللوحة ،رقم الهيكل ، رقم الجواز ....">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <dx:ASPxGridView ID="SearchGrid" ClientInstanceName="SearchGrid" runat="server" AutoGenerateColumns="true" RightToLeft="True" Settings-ShowTitlePanel="true" Width="100%">
                            <ClientSideEvents ToolbarItemClick="OnToolbarItemClick" />
                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                            <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
                            <SettingsSearchPanel Visible="True" />
                            <SettingsBehavior AllowFocusedRow="true" />
                            <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="Default" />
                            <SettingsPager PageSize="20" />
                            <SettingsText Title="الإصدارات" />
                            <Toolbars>
                                <dx:GridViewToolbar>
                                    <SettingsAdaptivity EnableCollapseRootItemsToIcons="true" Enabled="true" />
                                    <Items>
                                        <dx:GridViewToolbarItem AdaptivePriority="1" BeginGroup="true" Image-IconID="actions_download_16x16office2013" Text="تصدير إلى">
                                            <Items>
                                                <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF" />
                                                <dx:GridViewToolbarItem Command="ExportToDocx" Text="WORD" />
                                                <dx:GridViewToolbarItem Command="ExportToXls" Text="XLS" />
                                                <dx:GridViewToolbarItem Command="ExportToXlsx" Text="XLSX" />
                                                <dx:GridViewToolbarItem Command="ExportToRtf" Text="RTF" />
                                                <dx:GridViewToolbarItem Command="ExportToCsv" Text="CSV" />
                                            </Items>
                                        </dx:GridViewToolbarItem>
                                    </Items>
                                </dx:GridViewToolbar>
                            </Toolbars>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="المؤمن له" CellStyle-HorizontalAlign="Right" FieldName="CustName" VisibleIndex="2">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="رقم الوثيقة" CellStyle-HorizontalAlign="Right" FieldName="PolNo" VisibleIndex="1">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="رقم الطلب" CellStyle-HorizontalAlign="Right" FieldName="OrderNo" Visible="false" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="الملحق" CellStyle-HorizontalAlign="Right" FieldName="EndNo" Visible="true" VisibleIndex="4">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="الملحق" CellStyle-HorizontalAlign="Right" FieldName="LoadNo" Visible="false" VisibleIndex="5">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Report" CellStyle-HorizontalAlign="Right" FieldName="Report" Visible="false" VisibleIndex="6">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="تاريخ الإصدار" CellStyle-HorizontalAlign="Right" FieldName="IssuDate" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="7">
                                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="وقت الإصدار" CellStyle-HorizontalAlign="Right" FieldName="IssuTime" VisibleIndex="7">
                                    <PropertiesTextEdit DisplayFormatString="dddd HH:mm:ss">
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="التغطية من" CellStyle-HorizontalAlign="Right" FieldName="CoverFrom" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="التغطية إلى" CellStyle-HorizontalAlign="Right" FieldName="CoverTo" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="9">
                                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="الفرع/ الوكيل" CellStyle-HorizontalAlign="Right" FieldName="BranchName" VisibleIndex="10">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="EditForm" CellStyle-HorizontalAlign="Right" FieldName="EditForm" Visible="false" VisibleIndex="11">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="رمز النظام" CellStyle-HorizontalAlign="Right" FieldName="SubIns" Visible="false" VisibleIndex="12">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption=" الفرع" CellStyle-HorizontalAlign="Right" FieldName="Branch" Visible="false" VisibleIndex="12">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" Caption="عمليات" CellStyle-HorizontalAlign="Center" VisibleIndex="0">
                                    <CustomButtons>
                                        <dx:GridViewCommandColumnCustomButton ID="Edit" Text="تحرير">
                                            <Image Url="~/Content/Images/Edit.png">
                                            </Image>
                                        </dx:GridViewCommandColumnCustomButton>
                                        <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة">
                                            <Image Url="~/Content/Images/Print.png">
                                            </Image>
                                        </dx:GridViewCommandColumnCustomButton>
                                        <dx:GridViewCommandColumnCustomButton ID="Renew" Text="تجديد">
                                            <Image Url="~/Content/Images/renew_16px.png">
                                            </Image>
                                        </dx:GridViewCommandColumnCustomButton>
                                        <dx:GridViewCommandColumnCustomButton ID="SpecialProcedure" Text="عمليات خاصة">
                                            <Image Url="~/Content/Images/SpcOp.png">
                                            </Image>
                                        </dx:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dx:GridViewCommandColumn>
                            </Columns>
                        </dx:ASPxGridView>

                        <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="true" AllowDragging="true"
                            PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
                            PopupHorizontalAlign="WindowCenter" Modal="True">
                            <ClientSideEvents CloseUp="function(s,e){}" />
                        </dx:ASPxPopupControl>
                        <dx:ASPxGridView ID="SearchClm" ClientInstanceName="SearchClm" runat="server" AutoGenerateColumns="true" RightToLeft="True" Settings-ShowTitlePanel="true" Width="100%">
                            <ClientSideEvents ToolbarItemClick="OnToolbarItemClick1" />
                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                            <SettingsSearchPanel CustomEditorID="tbToolbarSearchClm" Visible="True" />
                            <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
                            <SettingsBehavior AllowFocusedRow="true" />
                            <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />
                            <SettingsPager PageSize="20" />
                            <SettingsText Title="المطالبات" />
                            <Toolbars>
                                <dx:GridViewToolbar>
                                    <SettingsAdaptivity EnableCollapseRootItemsToIcons="true" Enabled="true" />
                                    <Items>
                                        <dx:GridViewToolbarItem AdaptivePriority="1" BeginGroup="true" Image-IconID="actions_download_16x16office2013" Text="تصدير إلى">
                                            <Items>
                                                <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF" />
                                                <dx:GridViewToolbarItem Command="ExportToDocx" Text="WORD" />
                                                <dx:GridViewToolbarItem Command="ExportToXls" Text="XLS" />
                                                <dx:GridViewToolbarItem Command="ExportToXlsx" Text="XLSX" />
                                                <dx:GridViewToolbarItem Command="ExportToRtf" Text="RTF" />
                                                <dx:GridViewToolbarItem Command="ExportToCsv" Text="CSV" />
                                            </Items>
                                        </dx:GridViewToolbarItem>
                                    </Items>
                                </dx:GridViewToolbar>
                            </Toolbars>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="المؤمن له" CellStyle-HorizontalAlign="Right" FieldName="CustName" VisibleIndex="0">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="رقم الوثيقة" CellStyle-HorizontalAlign="Right" FieldName="PolNo" VisibleIndex="1">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="رقم الحادث" CellStyle-HorizontalAlign="Right" FieldName="ClmNo" VisibleIndex="2">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="رقم الطلب" CellStyle-HorizontalAlign="Right" FieldName="OrderNo" Visible="false" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="الملحق" CellStyle-HorizontalAlign="Right" FieldName="EndNo" Visible="false" VisibleIndex="4">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="الملحق" CellStyle-HorizontalAlign="Right" FieldName="LoadNo" Visible="false" VisibleIndex="5">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Report" CellStyle-HorizontalAlign="Right" FieldName="Report" Visible="false" VisibleIndex="6">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="تاريخ الحادث" CellStyle-HorizontalAlign="Right" FieldName="ClmDate" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="7">
                                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="التغطية من" CellStyle-HorizontalAlign="Right" FieldName="CoverFrom" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="التغطية إلى" CellStyle-HorizontalAlign="Right" FieldName="CoverTo" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="9">
                                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="الفرع/ الوكيل" CellStyle-HorizontalAlign="Right" FieldName="BranchName" VisibleIndex="10">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="EditForm" CellStyle-HorizontalAlign="Right" FieldName="EditForm" Visible="false" VisibleIndex="11">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="رمز النظام" CellStyle-HorizontalAlign="Right" FieldName="SubIns" Visible="false" VisibleIndex="12">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridView>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>