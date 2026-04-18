<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="Dist.aspx.vb" Inherits="Dist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../scripts/Scripts.js"></script>
    <script type="text/javascript">
        function SetSystem() {
            Sys.SetValue(Systems.GetValue());
            Branch.SetValue(Branches.GetValue());
            MainGrid.SetVisible(true);
            //cmbReports.SetVisible(true);

            MainGrid.PerformCallback();
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
                let mhdr='وثيقة جديدة - '
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

                PrintPop.SetHeaderText('REPORT');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if(s.cpNewWindowUrl != null &&  s.cpMyAttribute == 'AllIssues') {

                PrintPop.SetHeaderText('سجل تجميعي');
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'IssuClipOverall') {

                PrintPop.SetHeaderText('سجل الإنتاج');
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
            if (s.cpShowDeleteConfirmBox && s.cpMyAttribute == 'Delete'){
                pcConfirmDelete.Show();
            }
            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Issuance') {
                pcConfirmIssue.Show();
            }
            s.cpMyAttribute = '';
            s.cpNewWindowUrl = null;

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
            <td style="height: 50px">
                <dx:ASPxTimer ID="notificationTimer" runat="server" Interval="5000">
                    <ClientSideEvents Tick="OnTick" />
                </dx:ASPxTimer>
                <%--    <dx:ASPxTextBox ID="SSS1" runat="server" Width="100%">
                    </dx:ASPxTextBox>--%>
            </td>
            <td style="height: 50px">
                <dx:ASPxComboBox ID="Branches" ClientInstanceName="Branches" runat="server" Caption="الفروع" DataSourceID="BranchesDS" RightToLeft="True" TextField="BranchName" ValueField="BranchNo" Width="400px">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {//MainGrid.PerformCallback();
                            SetSystem();}" />
                </dx:ASPxComboBox>
                <asp:SqlDataSource ID="BranchesDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                    SelectCommand="SELECT BranchNo, rtrim(BranchName) As BranchName FROM BranchInfo WHERE (RIGHT(BranchNo, 3) = '000')"></asp:SqlDataSource>
            </td>
            <td class="dxeICC" style="height: 50px">
                <dx:ASPxComboBox ID="Systems" ClientInstanceName="Systems" runat="server" Caption="الانظمة :" DataSourceID="SystemsDS" RightToLeft="True" TextField="SUBSYSNAME" ValueField="SUBSYSNO" Width="400px">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {//MainGrid.PerformCallback();
                            SetSystem();}" />
                </dx:ASPxComboBox>
                <asp:SqlDataSource ID="SystemsDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                    SelectCommand="SELECT SUBSYSNO,rtrim(SUBSYSNAME) As SUBSYSNAME,MAINSYS
                                        FROM SUBSYSTEMS
                                        WHERE (MAINSYS in (1,3,6,9) OR SUBSYSNO='PA') and Branch=[dbo].[MainCenter]()
                                        ORDER BY MAINSYS"></asp:SqlDataSource>
            </td>
            <td style="height: 50px">
                <dx:ASPxTextBox ID="Branch" ClientInstanceName="Branch" Text="0" runat="server"
                    Height="10px"
                    Width="40px" Enabled="true" ClientVisible="False">
                    <Border BorderStyle="None" />
                    <ClientSideEvents TextChanged="function(s, e) {MainGrid.PerformCallback();
                             //MainGrid.Refresh()
                            ;}" />
                </dx:ASPxTextBox>
            </td>
            <td style="height: 50px">
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

    <dx:ASPxGridView ID="MainGrid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource" RightToLeft="True" KeyFieldName="OrderNo"
        OnCustomCallback="MainGrid_CustomCallback"
        OnHtmlDataCellPrepared="MainGrid_HtmlDataCellPrepared"
        SettingsLoadingPanel-Text="تحميل..."
        OnToolbarItemClick="MainGrid_ToolbarItemClick"
        ClientInstanceName="MainGrid"
        Width="100%">
        <ClientSideEvents EndCallback="OnEndCallback" ToolbarItemClick="OnToolbarItemClick" />
        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"
            AllowHideDataCellsByColumnMinWidth="true">
        </SettingsAdaptivity>
        <SettingsSearchPanel CustomEditorID="tbToolbarSearch" />
        <SettingsBehavior AllowFocusedRow="true" />
        <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />

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
                    <%--<dx:GridViewToolbarItem Command="Custom" Name="NewPolicy" Text="وثيقة جديدة" Image-IconID="tasks_newtask_16x16" AdaptivePriority="2" />--%>
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
            <dx:GridViewDataTextColumn FieldName="CustName" VisibleIndex="2" Caption="المؤمن له" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" MaxWidth="250" Width="18%">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PolNo" VisibleIndex="1" Caption="رقم الوثيقة" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" MaxWidth="250" Width="27%">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EndNo" VisibleIndex="2" Caption="ملحق" CellStyle-HorizontalAlign="Right" Visible="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LoadNo" VisibleIndex="3" Caption="إشعار شحنة" CellStyle-HorizontalAlign="Right" Visible="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrderNo" VisibleIndex="4" Caption="رقم الطلب" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IssuDate" VisibleIndex="5" Caption="تاريخ الإصدار" AdaptivePriority="1" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EntryDate" VisibleIndex="6" Caption="تاريخ الإدخال" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
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
            <dx:GridViewDataTextColumn FieldName="TpName" VisibleIndex="9" Caption="العملة" CellStyle-HorizontalAlign="Right">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <%--   <dx:GridViewDataTextColumn FieldName="PayAs" VisibleIndex="10" Caption="طريقة السداد" CellStyle-HorizontalAlign="Right">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="Report" VisibleIndex="11" Caption="التقرير" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Br" VisibleIndex="12" Caption="الفرع" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EditForm" VisibleIndex="13" Caption="مسار" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SubIns" VisibleIndex="13" Caption="رمز النظام" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" Caption="عمليات" AllowTextTruncationInAdaptiveMode="true">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="Distribute" Text="توزيع">
                        <Image Url="~/Content/Images/DistPolicy.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة">
                        <Image Url="~/Content/Images/Print.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <%--      <dx:GridViewCommandColumnCustomButton ID="Renew" Text="تجديد">
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
                        <td>
                            <a href="javascript:Yes_Click()">نعم</a>
                        </td>
                        <td>
                            <a href="javascript:No_Click()">لا</a>
                        </td>
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
                        <td>
                            <a href="javascript:YesIss_Click()">موافق</a>
                        </td>
                        <td>
                            <a href="javascript:NoIss_Click()">غير موافق</a>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <br />
    <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
        SelectCommand="SELECT PolNo,EndNo,LoadNo,OrderNo,Rtrim(TpName) As TpName,IssuDate,NETPRM,TOTPRM,CustName,SubIns,Branch as Br,isnull([dbo].[policyreport](SubIns),0) As Report,isnull([dbo].[policyEditForm](SubIns),0) As EditForm,EntryDate from PolicyFile LEFT OUTER JOIN CustomerFile on PolicyFile.CustNo=CustomerFile.CustNo LEFT OUTER JOIN ExtraInfo on  Currency=TPNo and TP='Cur' where branch=@Branch  AND LEFT(OrderNo,2)<>'IW' and (SubIns=@Sys) AND IssuDate is not null ORDER BY EntryDate DESC, OrderNo DESC">
        <SelectParameters>
            <asp:ControlParameter ControlID="Sys" PropertyName="Text" DefaultValue="0" Name="Sys"></asp:ControlParameter>
            <asp:ControlParameter ControlID="Branch" PropertyName="Text" DefaultValue="0" Name="Branch"></asp:ControlParameter>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>