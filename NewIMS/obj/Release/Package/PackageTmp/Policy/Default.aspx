<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="Default.aspx.vb" Inherits="Policy_Default" EnableSessionState="ReadOnly" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var LastReport = null;
        var start;

        function grid_Init(s, e) {
            MainGrid.Refresh();
        }

        function grid_BeginCallback(s, e) {
            start = new Date();
        }

        function SetSystem(sys, br) {
            Sys.SetValue(sys);
            Branch.SetValue(br);
            MainGrid.SetVisible(true);
            cmbReports.SetVisible(true);

            MainGrid.PerformCallback();
            //MainGrid.PerformCallback(sys);

            cmbReports.PerformCallback();
            //cmbReports.PerformCallback(sys);
            //var cp = ASPxClientControl.GetControlCollection().GetByName('MainGrid');
            //cp.PerformCallback();
        }

        function puOnInit(s, e) {
            AdjustSize();
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
                if (s.IsVisible())
                    s.UpdatePosition();
            });

        }
        function PopClose(s, e) {
            MainGrid.PerformCallback();
        }

        function AdjustSize() {
            var width = Math.max(0, document.documentElement.clientWidth) * 0.98;
            var height = Math.max(0, document.documentElement.clientHeight) * 0.98;
            PrintPop.SetWidth(width);
            PrintPop.SetHeight(height);
            PrintPop.SetSize(width, height);
        }

        function OnReportChange(s) {
            LastReport = s.GetValue().toString();
            Report = s.GetText().toString();

            s.PerformCallback(LastReport + "|" + Report);
        }

        function OnEndCallback(s, e) {

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'طباعة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'CreditNote') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'طباعة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Cashier') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'طباعة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'DebitNote') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'طباعة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'IssuSerial') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'إصدار - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Distribute') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1400, 800);
                let hdr = s.cpResult;
                let mhdr = 'توزيع الوثيقة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Edit') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult;
                let mhdr = 'تحرير - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'New') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                //alert(s.cpNewWindowUrl);
                let hdr = s.cpResult;
                let mhdr = 'وثيقة جديدة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Renew') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(800, 800);
                let hdr = s.cpResult;
                let mhdr = 'تجديد - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'RequestCancel') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(800, 800);
                let hdr = s.cpResult;
                let mhdr = 'X';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == '') {

                PrintPop.SetHeaderText(s.cpResult);
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'AllIssues') {

                PrintPop.SetHeaderText(s.cpResult);
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'IssuClipOverall') {

                PrintPop.SetHeaderText(s.cpResult);
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Search') {
                let hdr = s.cpResult;
                let mhdr = 'بحث متقدم - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpShowDeleteConfirmBox && s.cpMyAttribute == 'Delete') {
                pcConfirmDelete.Show();
                pcConfirmDelete.SetHeaderText(s.cpCust)
            }
            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Issuance') {
                pcConfirmIssue.Show();
                pcConfirmIssue.SetHeaderText(s.cpCust)
            }
            s.cpMyAttribute = '';
            s.cpNewWindowUrl = null;

        }

        function Yes_Click() {
            pcConfirmDelete.Hide();
            //alert(MainGrid.cpRowIndex);
            MainGrid.DeleteRow(MainGrid.cpRowIndex);
        }

        function No_Click() {
            pcConfirmDelete.Hide();
        }

        function YesIss_Click() {
            pcConfirmIssue.Hide();
            MainGrid.PerformCallback("Issue");
        }

        function NoIss_Click() {
            pcConfirmIssue.Hide();
        }

        function OnToolbarItemClick(s, e) {
            if (IsCustomExportToolbarCommand(e.item.name)) {
                e.processOnServer = true;
                //e.usePostBack = true;
            }

        }

        function IsCustomExportToolbarCommand(command) {
            return command == "NewPolicy" || command == "ExtraSearch";
            //command == "CustomExportToXLS" || command == "CustomExportToXLSX" ||
        }

        function SelectAndClosePopup() {

            PrintPop.Hide();
            //detailGrid.Refresh();
            //AccMoveGrid.Refresh();
        }
    </script>
    <table id="Table" runat="server" style="width: 100%;">
        <tr>
            <td>
                <dx:ASPxTimer ID="notificationTimer" runat="server" Interval="5000">
                    <ClientSideEvents Tick="OnTick" />
                </dx:ASPxTimer>
                <%--    <dx:ASPxTextBox ID="SSS1" runat="server" Width="100%">
                    </dx:ASPxTextBox>--%>
            </td>
            <td>
                <div class="dxeICC">
                </div>
                <asp:SqlDataSource ID="Reports" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                    SelectCommand="SELECT [Rep], [Desc] FROM [ReportMap] WHERE ([SubIns] = @SubIns OR [SubIns]='0') ORDER BY [SubIns]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Sys" PropertyName="Text" Name="SubIns" Type="String"></asp:ControlParameter>
                    </SelectParameters>
                </asp:SqlDataSource>
                <dx:ASPxComboBox ID="cmbReports" ClientInstanceName="cmbReports" runat="server" NullText="قائمة التقارير" DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith" SettingsLoadingPanel-Text="إعداد التقرير"
                    TextField="Desc" ValueField="Rep" Width="100%" DataSourceID="Reports" ClientVisible="False"
                    EnableSynchronization="false" RightToLeft="True" AllowEllipsisInText="True">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnReportChange(s); }" EndCallback="OnEndCallback" />
                    <CaptionSettings Position="Left" VerticalAlign="Middle" />
                    <CaptionStyle Font-Bold="True" Font-Names="Sakkal Majalla" Font-Size="Large">
                    </CaptionStyle>
                </dx:ASPxComboBox>
            </td>
            <td class="dxeICC">&nbsp;</td>
            <td>
                <dx:ASPxTextBox ID="Branch" ClientInstanceName="Branch" Text="0" runat="server" ClientVisible="false"
                    Height="10px"
                    Width="40px" Enabled="true">
                    <Border BorderStyle="None" />
                    <ClientSideEvents TextChanged="function(s, e) {MainGrid.PerformCallback();}" />
                </dx:ASPxTextBox>
                <dx:ASPxTextBox ID="Sys" ClientInstanceName="Sys" Text="0" runat="server" ClientVisible="false"
                    Height="10px"
                    Width="40px" Enabled="true">
                    <Border BorderStyle="None" />
                    <ClientSideEvents TextChanged="function(s, e) {MainGrid.PerformCallback();}" />
                </dx:ASPxTextBox>
            </td>
            <td></td>
        </tr>
    </table>
    <br />
    <br />
    <%--       <div class="BottomLargeMargin" dir="ltr">
            Command: <b><dx:ASPxLabel runat="server" ClientInstanceName="ClientCommandLabel" Text="..." /></b><br />
            Time taken (ms): <b><dx:ASPxLabel runat="server" ClientInstanceName="ClientTimeLabel" Text="..." /></b>
        </div>--%>
    <dx:ASPxGridView ID="MainGrid" runat="server" AutoGenerateColumns="False" RightToLeft="True" KeyFieldName="OrderNo" DataSourceID="SqlDataSource"
        OnRowDeleting="MainGrid_RowDeleting" OnCustomCallback="MainGrid_CustomCallback" OnHtmlDataCellPrepared="MainGrid_HtmlDataCellPrepared"
        ClientVisible="False"
        SettingsLoadingPanel-Text="تحميل..." SettingsLoadingPanel-Delay="1" SettingsSearchPanel-Delay="1" SettingsLoadingPanel-Mode="Disabled"
        OnToolbarItemClick="MainGrid_ToolbarItemClick"
        ClientInstanceName="MainGrid"
        Width="100%">
        <ClientSideEvents Init="grid_Init" BeginCallback="grid_BeginCallback" EndCallback="OnEndCallback" ToolbarItemClick="OnToolbarItemClick" />
        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" AllowHideDataCellsByColumnMinWidth="true">
        </SettingsAdaptivity>
        <SettingsSearchPanel CustomEditorID="tbToolbarSearch" />
        <SettingsBehavior AllowFocusedRow="true" />
        <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />
        <SettingsPager PageSize="25">
            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
        </SettingsPager>
        <Settings ShowGroupPanel="True" VerticalScrollBarStyle="VirtualSmooth" VerticalScrollableHeight="700" VerticalScrollBarMode="Visible" ShowFooter="true" />
        <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" AllowEdit="False" AllowInsert="False" AllowDelete="true" />
        <SettingsPopup>
            <FilterControl AutoUpdatePosition="true">
            </FilterControl>
        </SettingsPopup>
        <SettingsSearchPanel Visible="True" ShowClearButton="true" Delay="5000"></SettingsSearchPanel>
        <SettingsText SearchPanelEditorNullText="بحث..." />
        <SettingsBehavior AllowFocusedRow="true" />
        <Paddings Padding="0px" />
        <Border BorderWidth="0px" />
        <BorderBottom BorderWidth="1px" />
        <Toolbars>
            <dx:GridViewToolbar>
                <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                <Items>
                    <dx:GridViewToolbarItem Command="Custom" Name="NewPolicy" Text="وثيقة جديدة" Image-IconID="tasks_newtask_16x16" AdaptivePriority="2" />
                    <dx:GridViewToolbarItem Command="Refresh" Name="ExtraSearch" Text="بحث متقدم" BeginGroup="true" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="2" />
                    <%-- <dx:GridViewToolbarItem Command="ShowCustomizationWindow" Name="ExtraSearch" Text="بحث موسع" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="3" />
                    <dx:GridViewToolbarItem Command="Edit" />
                    <dx:GridViewToolbarItem Command="Delete" />--%>
                    <dx:GridViewToolbarItem Text="تصدير إلى" Image-IconID="actions_download_16x16office2013" BeginGroup="true" AdaptivePriority="1">
                        <Items>
                            <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF" />
                            <dx:GridViewToolbarItem Command="ExportToDocx" Text="WORD" />
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="XLS" />
                            <dx:GridViewToolbarItem Command="ExportToXlsx" Text="XLSX" />
                            <dx:GridViewToolbarItem Command="ExportToRtf" Text="RTF" />
                            <dx:GridViewToolbarItem Command="ExportToCsv" Text="CSV" />
                            <%-- <dx:GridViewToolbarItem Name="CustomExportToXLS" Text="Export to XLS(WYSIWYG)" Image-IconID="export_exporttoxls_16x16office2013" />--%><%-- <dx:GridViewToolbarItem Name="CustomExportToXLSX" Text="Export to XLSX(WYSIWYG)" Image-IconID="export_exporttoxlsx_16x16office2013" />--%>
                        </Items>
                    </dx:GridViewToolbarItem>
                    <dx:GridViewToolbarItem Alignment="Right">
                        <Template>
                            <dx:ASPxButtonEdit ID="tbToolbarSearch" runat="server" NullText="بحث..." Height="100%" ClearButton-DisplayMode="Always">
                                <Buttons>
                                    <dx:SpinButtonExtended Image-IconID="find_find_16x16gray" />
                                </Buttons>
                            </dx:ASPxButtonEdit>
                        </Template>
                    </dx:GridViewToolbarItem>
                </Items>
            </dx:GridViewToolbar>
        </Toolbars>
        <Columns>
            <dx:GridViewDataTextColumn FieldName="CustName" VisibleIndex="2" Caption="المؤمن له" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" MaxWidth="250" Width="18%">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PolNo" VisibleIndex="1" Caption="رقم الوثيقة" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" MaxWidth="250" Width="20%" PropertiesTextEdit-EnableClientSideAPI="False" PropertiesTextEdit-DisplayFormatInEditMode="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EndNo" VisibleIndex="2" Caption="ملحق" CellStyle-HorizontalAlign="Right" Visible="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LoadNo" VisibleIndex="3" Caption="شهادة تأمين" CellStyle-HorizontalAlign="Right" Visible="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrderNo" VisibleIndex="4" Caption="رقم الطلب" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IssuDate" VisibleIndex="5" Caption="تاريخ الإصدار" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EntryDate" VisibleIndex="6" Caption="تاريخ الإدخال" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right" Visible="true">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="NETPRM" VisibleIndex="7" Caption="صافي القسط" PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="N3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TOTPRM" VisibleIndex="8" Caption="إجمالي القسط" PropertiesTextEdit-DisplayFormatString="n3" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="n3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TpName" VisibleIndex="9" Caption="نوع العملة" AdaptivePriority="1" CellStyle-HorizontalAlign="Right">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="Payment" VisibleIndex="10" Caption="حالة السداد" AdaptivePriority="2" CellStyle-HorizontalAlign="Center">
                <DataItemTemplate>
                    <dx:ASPxImage ID="imgStatus" runat="server" OnInit="imgStatus_Init" AlternateText='<%# Eval("Payment") %>' />
                    <%--  <dx:ASPxLabel ID="DataItemLabel" runat="server"
                        Text='<%# Eval("Payment") %>' CssClass="image-text-label">
                    </dx:ASPxLabel>--%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Report" VisibleIndex="11" Caption="التقرير" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EditForm" VisibleIndex="12" Caption="مسار" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SubIns" VisibleIndex="13" Caption="رمز النظام" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Expired" VisibleIndex="14" Caption="Expiration" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Stoped" VisibleIndex="15" Caption="Stoped" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Br" VisibleIndex="16" Caption="Br" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IDs" VisibleIndex="17" Caption="IDs" CellStyle-HorizontalAlign="Right" Visible="false" SortOrder="None">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Financed" VisibleIndex="18" Caption="Financed" CellStyle-HorizontalAlign="Right" Visible="false" SortOrder="None">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="commision" VisibleIndex="19" Caption="Commision" CellStyle-HorizontalAlign="Right" Visible="false" SortOrder="None">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Broker" VisibleIndex="20" Caption="Broker" CellStyle-HorizontalAlign="Right" Visible="false" SortOrder="None">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IssMinutes" VisibleIndex="21" Caption="issMinute" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
              <dx:gridviewdatatextcolumn fieldname="UserName" visibleindex="22" caption="UserName" visible="false">
                <cellstyle horizontalalign="Right">
                </cellstyle>
            </dx:gridviewdatatextcolumn>
            <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center"
                MinWidth="100" MaxWidth="250" Width="15%" Caption="إجراءات" AllowTextTruncationInAdaptiveMode="true">
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
                    <dx:GridViewCommandColumnCustomButton ID="Issuance" Text="إصدار">
                        <Image Url="~/Content/Images/Accept.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Delete" Text="إلغاء الطلب">
                        <Image Url="~/Content/Images/Delete.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="CreditNote" Text="إشعار دائن">
                        <Image Url="~/Content/Images/receive_cash.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Payment" Text="أمر توريد/إيصال قبض">
                        <Image Url="~/Content/Images/pay_16px.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="DebitNote" Text="إشعار مدين">
                        <Image Url="~/Content/Images/DebitNote.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="RequestCancel" Text="طلب إلغاء">
                        <Image Url="~/Content/Images/Sandglass.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewCommandColumn>
        </Columns>
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="NETPRM" ShowInColumn="NETPRM" SummaryType="Sum" ShowInGroupFooterColumn="صافي القسط" ValueDisplayFormat="n3" />
            <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="TOTPRM" ShowInColumn="TOTPRM" SummaryType="Sum" Tag="إجمالي القسط" ValueDisplayFormat="n3" />
        </TotalSummary>
    </dx:ASPxGridView>
    <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="true" Modal="true"
        ShowCollapseButton="true" ShowMaximizeButton="true"
        AllowDragging="false" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
        PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="true">
        <ClientSideEvents CloseUp="PopClose" Init="puOnInit" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pcConfirmDelete" runat="server" ClientInstanceName="pcConfirmDelete" ShowCloseButton="false"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="إلغاء الطلب">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" Font-Bold="true">
                <table dir="rtl" style="width: 100%">
                    <tr>
                        <td style="vertical-align: central;" colspan="3">تأكيد إلغاء الطلب؟
                        </td>
                    </tr>
                    <tr>
                        <td><%--<a href="javascript:Yes_Click()">نعم</a>--%>
                            <dx:ASPxButton ID="yesButton" runat="server" Text="نعم" AutoPostBack="false">
                                <ClientSideEvents Click="Yes_Click" />
                            </dx:ASPxButton>
                        </td>
                        <td><%-- <a href="javascript:No_Click()">لا</a>--%>
                            <dx:ASPxButton ID="noButton" runat="server" Text="لا" AutoPostBack="false">
                                <ClientSideEvents Click="No_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pcConfirmIssue" runat="server" ClientInstanceName="pcConfirmIssue" ShowCloseButton="false"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="إصدار الوثيقة">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" Font-Bold="true">
                <table dir="rtl" style="width: 100%">
                    <tr>
                        <td colspan="2"><%--<a href="javascript:YesIss_Click()">موافق</a>--%>تأكيد إصدار الوثيقة، في حال الموافقة لن يمكنك التعديل في البيانات؟
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="yesIssButton" runat="server" AutoPostBack="False" Text="موافق">
                                <ClientSideEvents Click="YesIss_Click" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="noIssButton" runat="server" AutoPostBack="False" Text="غير موافق">
                                <ClientSideEvents Click="NoIss_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <br />
    <%--    <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
            SelectCommand=";WITH DDD AS (SELECT PolNo,EndNo,LoadNo,OrderNo,DBO.GetExtraCatName('Cur',Currency) As TpName, IssuDate,NETPRM,TOTPRM,CustName,SubIns,isnull([dbo].[policyreport](SubIns),0) As Report, isnull([dbo].[policyEditForm](SubIns),0) As EditForm, DBO.GetExtraCatName('Pay',PayType) As PayAs, EntryDate, CASE WHEN coverto <= getdate() THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END As Expired, CASE WHEN Stop = 1 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END As Stoped,Branch As Br,PolicyFile.IDs from PolicyFile LEFT JOIN CustomerFile on PolicyFile.CustNo=CustomerFile.CustNo) SELECT * FROM DDD where Br=@Branch AND LEFT(OrderNo,2)<>'IW' and (SubIns=@Sys) ORDER BY IDs DESC">
        <SelectParameters>
            <asp:ControlParameter ControlID="Sys" PropertyName="Text" DefaultValue="0" Name="Sys"></asp:ControlParameter>
            <asp:ControlParameter ControlID="Branch" PropertyName="Text" DefaultValue="0" Name="Branch"></asp:ControlParameter>
        </SelectParameters>
    </asp:SqlDataSource>--%>
    <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
        SelectCommand="MainPageSource" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="Branch" PropertyName="Text" DefaultValue="0" Name="Branch"></asp:ControlParameter>
            <asp:ControlParameter ControlID="Sys" PropertyName="Text" DefaultValue="0" Name="Sys"></asp:ControlParameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceLog" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
        SelectCommand="MainPageSourceLIMITED" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="Branch" PropertyName="Text" DefaultValue="0" Name="Branch"></asp:ControlParameter>
            <asp:ControlParameter ControlID="Sys" PropertyName="Text" DefaultValue="0" Name="Sys"></asp:ControlParameter>
            <asp:SessionParameter SessionField="UserID" DefaultValue="0" Name="User" Type="String"></asp:SessionParameter>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>