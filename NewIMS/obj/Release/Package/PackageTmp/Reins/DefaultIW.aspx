<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master"
    CodeBehind="DefaultIW.aspx.vb" Inherits="DefaultIW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../scripts/Scripts.js"></script>
    <script type="text/javascript">
        function SetSystem(sys, br) {
            Sys.SetValue(sys);
            //Branch.SetValue(br);
            //MainGrid.SetVisible(true);
            //cmbReports.SetVisible(true);

            MainGrid.PerformCallback();
            //MainGrid.PerformCallback(sys);

            cmbReports.PerformCallback();
            //cmbReports.PerformCallback(sys);
            var cp = ASPxClientControl.GetControlCollection().GetByName('MainGrid');
            //cp.PerformCallback();
        }
        var LastReport = null;
        function puOnInit(s, e) {
            AdjustSize();
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
                if (s.IsVisible())
                    s.UpdatePosition();
            });
        }

        function AdjustSize() {
            var width = Math.max(0, document.documentElement.clientWidth) * 0.95;
            var height = Math.max(0, document.documentElement.clientHeight) * 0.95;
            PrintPop.SetWidth(width);
            PrintPop.SetSize(width, height);
        }
        function OnReportChange(s) {
            //if (cmbCity.InCallback())
            //debugger;
            LastReport = s.GetValue().toString();
            Report = s.GetText().toString();
            //else
            s.PerformCallback(LastReport + "|" + Report);
        }

        function OnEndCallback(s, e) {
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult
                let mhdr = 'طباعة - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'AgentNote') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult
                let mhdr = 'طباعة - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Distribute') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1400, 800);
                let hdr = s.cpResult
                let mhdr = 'توزيع الوثيقة - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Edit') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = 'تحرير - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'New') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = 'وثيقة وارد جديدة - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Renew') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(800, 800);
                let hdr = s.cpResult
                let mhdr = 'تجديد - '
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
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'IssuClip') {

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
                let hdr = s.cpResult
                let mhdr = 'بحث مفصل - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpShowDeleteConfirmBox && s.cpMyAttribute == 'Delete') {
                pcConfirmDelete.Show();
            }
            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Issuance') {
                pcConfirmIssue.Show();
            }
            s.cpMyAttribute = ''
            s.cpNewWindowUrl = null

            UpdateTimeoutTimer();/////////////////////////////

        }
        function Yes_Click() {
            pcConfirmDelete.Hide();
            MainGrid.DeleteRow(MainGrid.cpRowIndex);

        }

        function No_Click() {
            pcConfirmDelete.Hide()
        }
        function YesIss_Click() {
            pcConfirmIssue.Hide();
            MainGrid.PerformCallback("Issue");
        }

        function NoIss_Click() {
            pcConfirmIssue.Hide()
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
                    SelectCommand="SELECT [Rep], [Desc] FROM [ReportMap] WHERE ([SubIns] = @SubIns) ORDER BY [SubIns]">
                    <SelectParameters>
                        <%--<asp:ControlParameter ControlID="Sys" PropertyName="Text" Name="SubIns" Type="String"></asp:ControlParameter>--%>
                        <asp:QueryStringParameter Name="SubIns" DbType="String" Direction="Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <dx:ASPxComboBox ID="cmbReports" ClientInstanceName="cmbReports" runat="server" NullText="قائمة التقارير"
                    DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith" SettingsLoadingPanel-Text="إعداد التقرير"
                    TextField="Desc" ValueField="Rep" Width="100%" DataSourceID="Reports"
                    EnableSynchronization="false" RightToLeft="True" AllowEllipsisInText="True" Theme="MetropolisBlue">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnReportChange(s); }" EndCallback=" OnEndCallback" />
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
                    <ClientSideEvents TextChanged="function(s, e) {MainGrid.PerformCallback();
                             //MainGrid.Refresh()
                            ;}" />
                </dx:ASPxTextBox>
                <dx:ASPxTextBox ID="Sys" ClientInstanceName="Sys" Text="0" runat="server" ClientVisible="false"
                    Height="10px"
                    Width="40px" Enabled="true">
                    <Border BorderStyle="None" />
                    <ClientSideEvents TextChanged="function(s, e) {MainGrid.PerformCallback();
                            //MainGrid.Refresh()
                            ;}" />
                </dx:ASPxTextBox>
            </td>
            <td></td>
        </tr>
    </table>
    <br />
    <br />
    <dx:ASPxGridView ID="MainGrid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource" RightToLeft="True" KeyFieldName="OrderNo"
        OnRowDeleting="MainGrid_RowDeleting" OnCustomCallback="MainGrid_CustomCallback" OnHtmlDataCellPrepared="MainGrid_HtmlDataCellPrepared"
        SettingsLoadingPanel-Text="تحميل..."
        OnToolbarItemClick="MainGrid_ToolbarItemClick"
        ClientInstanceName="MainGrid"
        Width="100%">
        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
            AllowHideDataCellsByColumnMinWidth="true">
        </SettingsAdaptivity>
        <SettingsSearchPanel CustomEditorID="tbToolbarSearch" />
        <SettingsBehavior AllowFocusedRow="true" />
        <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />
        <ClientSideEvents EndCallback="OnEndCallback" ToolbarItemClick="OnToolbarItemClick" />
        <SettingsPager PageSize="25">
            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
        </SettingsPager>
        <Settings ShowGroupPanel="True" VerticalScrollBarStyle="VirtualSmooth" VerticalScrollableHeight="700" VerticalScrollBarMode="Visible" />
        <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" AllowEdit="False" AllowInsert="False" AllowDelete="true" />
        <SettingsPopup>
            <FilterControl AutoUpdatePosition="true">
            </FilterControl>
        </SettingsPopup>
        <SettingsSearchPanel Visible="True" ShowClearButton="true"></SettingsSearchPanel>
        <SettingsText SearchPanelEditorNullText="بحث..." />
        <SettingsBehavior AllowFocusedRow="true" />
        <Paddings Padding="0px" />
        <Border BorderWidth="0px" />
        <BorderBottom BorderWidth="1px" />
        <Toolbars>
            <dx:GridViewToolbar>
                <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                <Items>
                    <dx:GridViewToolbarItem Command="Custom" Name="NewPolicy" Text="وثيقة وارد جديدة" Image-IconID="tasks_newtask_16x16" AdaptivePriority="2" />
                    <dx:GridViewToolbarItem Command="Refresh" Name="ExtraSearch" Text="بحث مفصل" BeginGroup="true" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="2" />
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
            <dx:GridViewDataTextColumn FieldName="PolNo" VisibleIndex="1" Caption="رقم الوثيقة" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="80" MaxWidth="160" Width="15%">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Company" VisibleIndex="2" Caption="الشركة المسندة" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="120" MaxWidth="100" Width="20%">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CustName" VisibleIndex="3" Caption="المؤمن له" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="120" MaxWidth="100" Width="20%">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="EndNo" VisibleIndex="4" Caption="ملحق" CellStyle-HorizontalAlign="Right" Visible="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <%--            <dx:GridViewDataTextColumn FieldName="LoadNo" VisibleIndex="5" Caption="إشعار شحنة" CellStyle-HorizontalAlign="Right" Visible="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="OrderNo" VisibleIndex="5" Caption="رقم الطلب" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IssuDate" VisibleIndex="6" Caption="تاريخ الإصدار" AdaptivePriority="1" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EntryDate" VisibleIndex="7" Caption="تاريخ الإدخال" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <%--            <dx:GridViewDataTextColumn FieldName="NETPRM" VisibleIndex="7" Caption="صافي الاشتراك" PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="N3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="TOTPRM" VisibleIndex="8" Caption="إجمالي القسط" PropertiesTextEdit-DisplayFormatString="n3" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="n3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TpName" VisibleIndex="9" Caption="العملة" CellStyle-HorizontalAlign="Right">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SystemName" VisibleIndex="10" Caption="نوع الوثيقة" CellStyle-HorizontalAlign="Right">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Sys" VisibleIndex="11" Caption="نوع الوثيقة" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PayAs" VisibleIndex="12" Caption="طريقة السداد" CellStyle-HorizontalAlign="Right">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Report" VisibleIndex="13" Caption="التقرير" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EditForm" VisibleIndex="13" Caption="مسار" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <%--           <dx:GridViewDataTextColumn FieldName="SubIns" VisibleIndex="13" Caption="رمز النظام" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" Caption="عمليات" AllowTextTruncationInAdaptiveMode="true" MinWidth="90" MaxWidth="90" Width="7%">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="Edit" Text="تحرير">
                        <Image Url="~/Content/Images/Edit.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Distribute" Text="توزيع">
                        <Image Url="~/Content/Images/Treaty.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة">
                        <Image Url="~/Content/Images/Print.png">
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
                    <%--                    <dx:GridViewCommandColumnCustomButton ID="AgentNote" Text="إشعار عميل">
                        <Image Url="~/Content/Images/receive_cash.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Renew" Text="تجديد">
                        <Image Url="~/Content/Images/renew_16px.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>--%>
                </CustomButtons>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewCommandColumn>
        </Columns>
    </dx:ASPxGridView>
    <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="true" AllowDragging="true" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
        PopupHorizontalAlign="WindowCenter" Modal="True">
        <ClientSideEvents CloseUp="function(s,e){
                //MainGrid.Refresh();
            MainGrid.PerformCallback();}"
            Init="puOnInit" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pcConfirmDelete" runat="server" ClientInstanceName="pcConfirmDelete" ShowCloseButton="false"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="إلغاء الطلب" Theme="iOS">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table dir="rtl">
                    <tr>
                        <td style="align-items: center;" colspan="2">تأكيد إلغاء الطلب؟
                        </td>
                    </tr>
                    <tr>
                        <td><a href="javascript:Yes_Click()">نعم</a> </td>
                        <td><a href="javascript:No_Click()">لا</a> </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pcConfirmIssue" runat="server" ClientInstanceName="pcConfirmIssue" ShowCloseButton="false" Width="250px"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="إصدار الوثيقة" Theme="iOS">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table dir="rtl">
                    <tr>
                        <td style="align-items: center;" colspan="2">تأكيد إصدار الوثيقة، في حال الموافقة لن يمكنك التعديل في البيانات؟
                        </td>
                    </tr>
                    <tr>
                        <td><a href="javascript:YesIss_Click()">موافق</a> </td>
                        <td><a href="javascript:NoIss_Click()">غير موافق</a> </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <br />
    <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
        SelectCommand="select PolNo,EndNo,LoadNo,OrderNo,TpName,IssuDate,TOTPRM,customerfile.CustName As CustName,dbo.[SysName](SubIns) as SystemName, SubIns As Sys, case when PayType=1 then 'نقداً' else 'على الحساب' end as PayAs, Custf.CustName as Company, EntryDate from PolicyFile inner join CustomerFile on customerfile.custno=policyfile.custno inner join  ExtraInfo on policyfile.Currency=extrainfo.TPNo  and extrainfo.tp='Cur' inner join CustomerFile as Custf on Custf.custno=policyfile.OwnNo
                where Branch=@Sys Order by OrderNo Desc, PolNo Desc">
        <SelectParameters>
            <%-- <asp:ControlParameter ControlID="Sys" PropertyName="Text" DefaultValue="0" Name="Sys"></asp:ControlParameter>--%>
            <asp:QueryStringParameter Name="Sys" DbType="String" Direction="Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>