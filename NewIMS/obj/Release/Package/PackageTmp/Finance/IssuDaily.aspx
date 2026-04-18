<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="IssuDaily.aspx.vb" Inherits="IssuDaily" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../scripts/Scripts.js"></script>
    <script type="text/javascript">
        function SetSystem() {
            Sys.SetValue(Systems.GetValue());
            //Branch.SetValue(Branches.GetValue());

            MainGrid.SetVisible(true);
            //cmbReports.SetVisible(true);

            //MainGrid.PerformCallback();
            //MainGrid.PerformCallback(sys);

            //cmbReports.PerformCallback();
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

        function puOnInit(s, e) {
            AdjustSize();
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
                if (s.IsVisible())
                    s.UpdatePosition();
            });
        }

        function AdjustSize() {
            var width = Math.max(0, document.documentElement.clientWidth) * 0.98;
            var height = Math.max(0, document.documentElement.clientHeight) * 0.98;
            PrintPop.SetWidth(width);
            PrintPop.SetSize(width, height);
        }
        function OnReportChange(s) {
            //if (cmbCity.InCallback())
            LastReport = s.GetValue().toString();
            //else
            s.PerformCallback(LastReport);
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
                let mhdr='إضافة - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Search') {
                let hdr = s.cpResult
                let mhdr = 'استعلام - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpShowDeleteConfirmBox && s.cpMyAttribute == 'Delete'){
                pcConfirmDelete.Show();
            }
            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Issuance') {
                pcConfirmIssue.Show();
            }
            s.cpMyAttribute=''
            s.cpNewWindowUrl = null

           // UpdateTimeoutTimer();/////////////////////////////

        }
        function Yes_Click() {
            pcConfirmDelete.Hide();
            MainGrid.DeleteRow(MainGrid.cpRowIndex);

           }

        function No_Click() {
           // pcConfirmDelete.Hide()
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
            return command == "NewDaily" || command == "ExtraSearch" || command == "BalanceSheet" || command == "MJournal" || command == "DJournal";
            //command == "CustomExportToXLS" || command == "CustomExportToXLSX" ||
        }
    </script>
    <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
        <ClientSideEvents BeginCallback="" />
        <SettingsLoadingPanel Text="&amp;hellip;" Delay="0" />
        <PanelCollection>
            <dx:PanelContent runat="server">
                <table id="Table" runat="server" style="width: 100%;">
                    <tr>
                        <td style="height: 50px">
                            <dx:ASPxTimer ID="notificationTimer" runat="server" Interval="5000">
                                <ClientSideEvents Tick="OnTick" />
                            </dx:ASPxTimer>
                            <%--    <dx:ASPxTextBox ID="SSS1" runat="server" Width="100%">
                    </dx:ASPxTextBox>--%>
                            <asp:SqlDataSource ID="MonthsSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand=";with Months as (select month(GETDATE()) as Mnumber, convert(nvarchar(15),FORMAT(GETDATE(),'MMMM','ar-ly'),111) as name, 1 as number union all select month(dateadd(month,number,(GETDATE()))) Mnumber,convert(nvarchar(15),FORMAT(dateadd(month,number,(GETDATE())),'MMMM','ar-ly'),111) as name,number+1 from Months  where number&lt;12 ) select Mnumber, name from Months order by Mnumber"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="YearsSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand=";WITH yearlist AS (SELECT min(year(dailydte)) AS year from mainjournal UNION ALL SELECT yl.year + 1 AS year FROM yearlist yl  WHERE yl.year + 1 &lt;= YEAR(GETDATE())) SELECT  Year FROM yearlist ORDER BY year DESC"></asp:SqlDataSource>
                        </td>
                        <td style="height: 50px">&nbsp;</td>
                        <td style="height: 50px">
                            <dx:ASPxComboBox ID="Months" runat="server" Caption="لشهر" ClientInstanceName="Months"
                                DataSourceID="MonthsSource"
                                RightToLeft="True" TextField="name" ValueField="Mnumber">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) {MainGrid.PerformCallback();}" />
                            </dx:ASPxComboBox>
                        </td>
                        <td style="height: 50px">
                            <dx:ASPxComboBox ID="Years" runat="server" Caption="لسنة" ClientInstanceName="Years" DataSourceID="YearsSource"
                                IncrementalFilteringMode="StartsWith" MaxLength="3" TextField="Year" ValueField="Year" ValueType="System.Int32" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) {MainGrid.PerformCallback();}" />
                                <LoadingPanelStyle ImageSpacing="5px">
                                </LoadingPanelStyle>
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dx:ASPxComboBox>
                            <dx:ASPxTextBox ID="Sys" ClientInstanceName="Sys" Text="0" runat="server"
                                Height="10px"
                                Width="40px" Enabled="true" ClientVisible="False">
                                <Border BorderStyle="None" />
                                <ClientSideEvents TextChanged="function(s, e) {MainGrid.PerformCallback();
                            //MainGrid.Refresh()
                            ;}" />
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <dx:ASPxGridView ID="MainGrid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource" RightToLeft="True"
                    KeyFieldName="DAILYNUM; DailyTyp"
                    OnCustomCallback="MainGrid_CustomCallback"
                    OnHtmlDataCellPrepared="MainGrid_HtmlDataCellPrepared"
                    SettingsLoadingPanel-Text="تحميل..." Delay="5"
                    OnToolbarItemClick="MainGrid_ToolbarItemClick"
                    ClientInstanceName="MainGrid"
                    Width="100%">
                    <ClientSideEvents EndCallback="OnEndCallback" ToolbarItemClick="OnToolbarItemClick" />
                    <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
                    <SettingsSearchPanel CustomEditorID="tbToolbarSearch" />
                    <SettingsBehavior AllowFocusedRow="true" />
                    <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />

                    <SettingsPager PageSize="25">
                        <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                    </SettingsPager>
                    <Settings ShowGroupPanel="True" VerticalScrollBarStyle="VirtualSmooth" VerticalScrollableHeight="700" VerticalScrollBarMode="Visible" />
                    <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" AllowEdit="False" AllowInsert="False" AllowDelete="true" />
                    <SettingsPopup>
                        <FilterControl AutoUpdatePosition="False">
                        </FilterControl>
                    </SettingsPopup>
                    <SettingsSearchPanel Visible="True" ShowClearButton="true"></SettingsSearchPanel>
                    <SettingsLoadingPanel Text="تحميل..."></SettingsLoadingPanel>
                    <SettingsText SearchPanelEditorNullText="بحث..." />
                    <SettingsBehavior AllowFocusedRow="true" />
                    <Paddings Padding="0px" />
                    <Border BorderWidth="0px" />
                    <BorderBottom BorderWidth="1px" />
                    <Toolbars>
                        <dx:GridViewToolbar>
                            <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                            <Items>
                                <dx:GridViewToolbarItem Command="Custom" Name="NewDaily" Text=" إضافة" Image-IconID="tasks_newtask_16x16" AdaptivePriority="1" />
                                <dx:GridViewToolbarItem Command="Custom" Name="BalanceSheet" Text=" ميزان المراجعة" Image-IconID="reports_sparklinewinloss_svg_16x16" AdaptivePriority="2" />
                                <dx:GridViewToolbarItem Command="Custom" Name="MJournal" Text=" القيد الشهري" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="3" />
                                <dx:GridViewToolbarItem Command="Custom" Name="DJournal" Text=" القيد اليومي" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="4" />
                                <dx:GridViewToolbarItem Command="Custom" Name="ExtraSearch" Text=" بحث متقدم " Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="5" />
                                <dx:GridViewToolbarItem Command="Custom" Name="PrintCheques" Text="طباعة صكوك " Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="6" />

                                <%--<dx:GridViewToolbarItem Command="Custom" Name="NewPolicy" Text="وثيقة جديدة" Image-IconID="tasks_newtask_16x16" AdaptivePriority="2" />
                                    <dx:GridViewToolbarItem Command="Refresh" Name="ExtraSearch" Text="بحث " BeginGroup="true" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="4">
                                        <Image IconID="functionlibrary_lookupreference_16x16"></Image>
                                    </dx:GridViewToolbarItem>
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
                                    <Image IconID="actions_download_16x16office2013">
                                    </Image>
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
                        <dx:GridViewDataTextColumn FieldName="DAILYNUM" VisibleIndex="1" Caption="رقم اليومية" CellStyle-HorizontalAlign="Right"
                            AllowTextTruncationInAdaptiveMode="true">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="DAILYSRL" VisibleIndex="2" Caption="رقم القيد" AdaptivePriority="2" CellStyle-HorizontalAlign="Right" Visible="True">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="DAILYDTE" VisibleIndex="3" Caption="تاريخ اليومية"
                            PropertiesTextEdit-DisplayFormatString="dd MMMM yyyy" CellStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit DisplayFormatString="dd MMMM yyyy">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="DailyTyp" VisibleIndex="4" Caption="type" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" Visible="false">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Comment" VisibleIndex="5" Caption="الشرح" CellStyle-HorizontalAlign="Right" Visible="True" Width="42%">
                            <CellStyle HorizontalAlign="Right" Wrap="true">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="DailyPrv" VisibleIndex="6" Caption="approved" CellStyle-HorizontalAlign="Right" Visible="false">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="DailyChk" VisibleIndex="7" Caption="Checked" CellStyle-HorizontalAlign="Right" Visible="false">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Dr" VisibleIndex="8" Caption="مجموع المدين" AdaptivePriority="1" PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit DisplayFormatString="N3">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Cr" VisibleIndex="9" Caption="مجموع الدائن" AdaptivePriority="1" PropertiesTextEdit-DisplayFormatString="n3" CellStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit DisplayFormatString="n3">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <%--   <dx:GridViewDataTextColumn FieldName="PayAs" VisibleIndex="10" Caption="طريقة السداد" CellStyle-HorizontalAlign="Right">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
                        <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" Caption="عمليات" AllowTextTruncationInAdaptiveMode="true">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="Edit" Text="تحرير">
                                    <Image Url="~/Content/Images/Edit.png">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                                <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة">
                                    <Image Url="~/Content/Images/Print.png">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                                <dx:GridViewCommandColumnCustomButton ID="Approved" Text="معتمد">
                                    <Image Url="~/Content/Images/Approved.png">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                                <dx:GridViewCommandColumnCustomButton ID="Approvable" Text="جاهز للمراجعة والاعتماد">
                                    <Image Url="~/Content/Images/ApproveReady.png">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                                <dx:GridViewCommandColumnCustomButton ID="Errorlbl" Text="خطأ في الادخال">
                                    <Image Url="~/Content/Images/DeleteRed.png">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
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
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                        </dx:PopupControlContentControl>
                    </ContentCollection>
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
                <asp:SqlDataSource ID="SqlDataSource" runat="server"
                    SelectCommand="SELECT RTRIM(MainJournal.DAILYNUM) As DAILYNUM, MainJournal.DailyTyp, MainJournal.DAILYDTE, MainJournal.DAILYSRL, MainJournal.DailyPrv, MainJournal.DailyChk, MainJournal.Comment, SUM(Journal.Dr) AS Dr, SUM(ABS(Journal.Cr)) AS Cr FROM MainJournal  LEFT OUTER JOIN Journal ON MainJournal.DAILYNUM = Journal.DAILYNUM AND MainJournal.DailyTyp = Journal.TP WHERE (YEAR(MainJournal.DAILYDTE) = @Year) AND (MONTH(MainJournal.DAILYDTE) = @Month) AND (MainJournal.DailyTyp = @type) AND RTRIM(LTRIM(MainJournal.ANALSNUM))='A' AND MainJournal.Branch=@Br GROUP BY Sn,MainJournal.DAILYNUM, MainJournal.DailyTyp, MainJournal.DAILYDTE, MainJournal.DAILYSRL,MainJournal.DailyPrv, MainJournal.DailyChk, MainJournal.Comment ORDER BY DailyChk asc,MainJournal.Sn Desc">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Years" PropertyName="Value" DefaultValue="0" Name="Year"></asp:ControlParameter>
                        <asp:ControlParameter ControlID="Months" PropertyName="Value" DefaultValue="0" Name="Month"></asp:ControlParameter>
                        <asp:QueryStringParameter QueryStringField="Sys" DefaultValue="0" Name="type"></asp:QueryStringParameter>
                        <asp:SessionParameter SessionField="Branch" DefaultValue="0" Name="Br"></asp:SessionParameter>
                    </SelectParameters>
                </asp:SqlDataSource>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>