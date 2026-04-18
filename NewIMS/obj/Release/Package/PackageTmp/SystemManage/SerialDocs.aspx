<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="SerialDocs.aspx.vb" Inherits="SerialDocs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../scripts/Scripts.js"></script>
    <script type="text/javascript">

        function onSelectedIndexChanged(s, e) {
            var selectedItem = s.GetSelectedItem();
            Branch.SetText(selectedItem.GetColumnText("BranchNo"));
            Sys.SetText(selectedItem.GetColumnText("SUBSYSNO"));
            //lblCountry.SetText(selectedItem.GetColumnText("Country"));
            //lblPhone.SetText(selectedItem.GetColumnText("Phone"));
            MainGrid.SetVisible(true);
            //cmbReports.SetVisible(true);

            MainGrid.PerformCallback();
        }

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
            var width = Math.max(0, document.documentElement.clientWidth) * 0.7;
            var height = Math.max(0, document.documentElement.clientHeight) * 0.8;
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
            s.cpMyAttribute=''
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
            <td style="height: 50px">
                <dx:ASPxTimer ID="notificationTimer" runat="server" Interval="5000">
                    <ClientSideEvents Tick="OnTick" />
                </dx:ASPxTimer>
                <%--    <dx:ASPxTextBox ID="SSS1" runat="server" Width="100%">
                    </dx:ASPxTextBox>--%>
            </td>
            <td style="height: 50px">
                <%--<dx:ASPxComboBox ID="Branches" ClientInstanceName="Branches" runat="server" Caption="الفرع / الوكيل" DataSourceID="BranchesDS" RightToLeft="True" TextField="BranchName" ValueField="BranchNo">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {//MainGrid.PerformCallback();
                            SetSystem();}" />
                    </dx:ASPxComboBox>
                    <asp:SqlDataSource ID="BranchesDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                        SelectCommand="SELECT BranchNo, BranchName FROM BranchInfo WHERE (RIGHT (BranchNo, 2) = '00')"></asp:SqlDataSource>--%>
            </td>
            <td class="dxeICC" style="height: 50px">
                <%--       <dx:ASPxComboBox ID="Systems" ClientInstanceName="Systems" runat="server" Caption="لنوع النظام" DataSourceID="SystemsDS"
                        RightToLeft="True" TextField="SerialBranch" ValueField="SUBSYSNO">
                        <Columns>
                            <dx:ListBoxColumn FieldName="BranchNo" />
                            <dx:ListBoxColumn FieldName="SUBSYSNO" />
                            <dx:ListBoxColumn FieldName="SerialBranch" />
                        </Columns>
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {//MainGrid.PerformCallback();
                            SetSystem();}" />
                    </dx:ASPxComboBox>--%>
                <dx:ASPxComboBox ID="Systems" runat="server" Width="285px" DropDownWidth="550"
                    Caption="اختر نوع التأمين والفرع/ أو الوكيل"
                    DropDownStyle="DropDownList" ValueField="SerialBranch" ValueType="System.String"
                    TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                    CallbackPageSize="30">
                    <ClientSideEvents SelectedIndexChanged="onSelectedIndexChanged" />
                    <Columns>
                        <dx:ListBoxColumn FieldName="SerialBranch" Width="100px" Caption="فرع/وكيل - نوع التأمين" />
                        <dx:ListBoxColumn FieldName="BranchNo" Width="30px" ClientVisible="false" />
                        <dx:ListBoxColumn FieldName="SUBSYSNO" Width="10%" ClientVisible="false" />
                        <%-- <dx:ListBoxColumn FieldName="Phone" Width="100px" />--%>
                    </Columns>
                </dx:ASPxComboBox>
                <asp:SqlDataSource ID="SystemsDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                    SelectCommand="SELECT BranchNo, SUBSYSNO, Rtrim(BranchName)+' - '+ Rtrim(SUBSYSNAME) As SerialBranch
                            FROM BranchInfo LEFT OUTER JOIN SUBSYSTEMS ON SUBSYSTEMS.Branch=BranchInfo.BranchNo AND SUBSYSNO IN ('01','27','OR','04')"></asp:SqlDataSource>

                <asp:SqlDataSource ID="SystemsDSA" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                    SelectCommand="SELECT BranchNo, SUBSYSNO, Rtrim(BranchName)+' - '+ Rtrim(SUBSYSNAME) As SerialBranch
                            FROM BranchInfo LEFT OUTER JOIN SUBSYSTEMS ON SUBSYSTEMS.Branch=BranchInfo.BranchNo AND SUBSYSNO IN ('01','27','OR','04') where SUBSYSTEMS.Branch=@Br">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="0" Name="Br" SessionField="Branch" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="SystemsDSB" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                    SelectCommand="SELECT BranchNo, SUBSYSNO, Rtrim(BranchName)+' - '+ Rtrim(SUBSYSNAME) As SerialBranch
                            FROM BranchInfo LEFT OUTER JOIN SUBSYSTEMS ON SUBSYSTEMS.Branch=BranchInfo.BranchNo AND SUBSYSNO IN ('01','27','OR','04')
                         where left(SUBSYSTEMS.Branch,2)=left(@Br,2)">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="0" Name="Br" SessionField="Branch" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="height: 50px">
                <dx:ASPxTextBox ID="Branch" ClientInstanceName="Branch" Text="0" runat="server"
                    Height="10px"
                    Width="40px" Enabled="true" ClientVisible="false">
                    <Border BorderStyle="None" />
                    <ClientSideEvents TextChanged="function(s, e) {MainGrid.PerformCallback();
                             //MainGrid.Refresh()
                            ;}" />
                </dx:ASPxTextBox>
            </td>
            <td style="height: 50px">
                <dx:ASPxTextBox ID="Sys" ClientInstanceName="Sys" Text="0" runat="server"
                    Height="10px"
                    Width="40px" Enabled="true" ClientVisible="false">
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
    <dx:ASPxGridView ID="MainGrid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource" RightToLeft="True" KeyFieldName="Sr"
        OnCustomCallback="MainGrid_CustomCallback" OnHtmlDataCellPrepared="MainGrid_HtmlDataCellPrepared"
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
        <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" AllowInsert="False" AllowDelete="true" />
        <SettingsPopup>
            <FilterControl AutoUpdatePosition="true">
            </FilterControl>
        </SettingsPopup>
        <SettingsSearchPanel Visible="True" ShowClearButton="true"></SettingsSearchPanel>
        <SettingsLoadingPanel Text="تحميل..."></SettingsLoadingPanel>
        <SettingsText SearchPanelEditorNullText="بحث..." />
        <Columns>
            <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="SerialF" ShowInCustomizationForm="True" AllowTextTruncationInAdaptiveMode="True" Caption="بداية التسلسل" VisibleIndex="1">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SerialT" ShowInCustomizationForm="True" AllowTextTruncationInAdaptiveMode="True" Caption="نهاية التسلسل" VisibleIndex="2">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DocsCount" ShowInCustomizationForm="True" AllowTextTruncationInAdaptiveMode="True" Caption="عدد النماذج المستلمة" VisibleIndex="3">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UsedSer" ShowInCustomizationForm="True" AllowTextTruncationInAdaptiveMode="True" Caption="المستعمل" VisibleIndex="4">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="RemSer" ShowInCustomizationForm="True" AllowTextTruncationInAdaptiveMode="True" Caption="المتبقي" VisibleIndex="5">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsBehavior AllowFocusedRow="true" />
        <Paddings Padding="0px" />
        <Border BorderWidth="0px" />
        <BorderBottom BorderWidth="1px" />
        <Toolbars>
            <dx:GridViewToolbar>
                <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                <Items>
                    <%--<dx:GridViewToolbarItem Command="Custom" Name="NewPolicy" Text="وثيقة جديدة" Image-IconID="tasks_newtask_16x16" AdaptivePriority="2" />--%><%--<dx:GridViewToolbarItem Command="Refresh" Name="ExtraSearch" Text="بحث مفصل" BeginGroup="true" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="2" />--%><%-- <dx:GridViewToolbarItem Command="ShowCustomizationWindow" Name="ExtraSearch" Text="بحث موسع" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="3" />
                    <dx:GridViewToolbarItem Command="Edit" />
                    <dx:GridViewToolbarItem Command="Delete" />

                      <dx:GridViewToolbarItem Alignment="Right">
                          <Template>
                              <dx:ASPxButtonEdit ID="tbToolbarSearch" runat="server" NullText="بحث..." Height="100%" ClearButton-DisplayMode="Always">
                                  <Buttons>
                                      <dx:SpinButtonExtended Image-IconID="find_find_16x16gray" />
                                  </Buttons>
                              </dx:ASPxButtonEdit>
                          </Template>
                      </dx:GridViewToolbarItem>--%>
                </Items>
            </dx:GridViewToolbar>
        </Toolbars>
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
        SelectCommand="select SerialF,SerialT,Sr,(CASE WHEN SERIALF=0 OR SERIALT=0 THEN 0 WHEN SERIALT>SERIALF THEN SerialT-SerialF+1 WHEN SERIALF=SERIALT AND SERIALT>0 THEN 1 END ) AS DocsCount,dbo.[RemainSerials](@Branch,@Sys) As RemSer,dbo.[UsedSerials](@Branch,@Sys) As UsedSer from SUBSYSTEMS where Branch=@Branch And SUBSYSNO=@Sys"
        UpdateCommand="UPDATE [SUBSYSTEMS] SET [SerialF] = @SerialF, [SerialT] = @SerialT WHERE [Sr] = @Sr AND @SerialT>=@SerialF"
        OnUpdating="SqlDataSource_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="Sys" PropertyName="Text" DefaultValue="0" Name="Sys"></asp:ControlParameter>
            <asp:ControlParameter ControlID="Branch" PropertyName="Text" DefaultValue="0" Name="Branch"></asp:ControlParameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="SerialF" Type="Int32" />
            <asp:Parameter Name="SerialT" Type="Int32" />
            <asp:Parameter Name="Sr" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>