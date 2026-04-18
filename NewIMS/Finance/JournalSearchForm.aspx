<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="JournalSearchForm.aspx.vb" Inherits="JournalSearchForm" %>

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
                PrintPop.SetHeaderText('بحث مفصل');
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
                        <dx:ASPxTextBox ID="searchtxt" runat="server" Width="100%">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <dx:ASPxGridView ID="SearchGrid" ClientInstanceName="SearchGrid" runat="server" AutoGenerateColumns="true" RightToLeft="True"
                            Settings-ShowTitlePanel="true" Width="100%">
                            <ClientSideEvents ToolbarItemClick="OnToolbarItemClick" />
                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                            <SettingsSearchPanel CustomEditorID="tbToolbarSearch" Visible="True" />
                            <SettingsBehavior AllowFocusedRow="true" />
                            <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="Default" />
                            <SettingsPager PageSize="20" />
                            <SettingsText Title="نتائج البحث" />
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
                                <dx:GridViewDataTextColumn Caption="نوع القيد" CellStyle-HorizontalAlign="Right" FieldName="Type" VisibleIndex="1">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="الرقم" CellStyle-HorizontalAlign="Right" FieldName="DAILYNUM" VisibleIndex="2">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="التاريخ" CellStyle-HorizontalAlign="Right" FieldName="DAILYDTE" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="3">
                                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="وصف القيد" CellStyle-HorizontalAlign="Right" FieldName="Comment" VisibleIndex="4">
                                    <CellStyle HorizontalAlign="Right" Wrap="True">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="نوع القيد" CellStyle-HorizontalAlign="Right" FieldName="DailyTyp" VisibleIndex="5" Visible="false">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="الفرع" CellStyle-HorizontalAlign="Right" FieldName="BranchName" VisibleIndex="6" Visible="true">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <%--                            <dx:GridViewDataTextColumn Caption="EditForm" CellStyle-HorizontalAlign="Right" FieldName="EditForm" Visible="false" VisibleIndex="11">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="رمز النظام" CellStyle-HorizontalAlign="Right" FieldName="SubIns" Visible="false" VisibleIndex="12">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>--%>
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

                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>