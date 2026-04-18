<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Main.master" Inherits="Branches" CodeBehind="Branches.aspx.vb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script lang="javascript" type="text/javascript">
        function DownFont(sender) {
            // Retrieve a reference to the control.
            var control = sender.getHost();

            // Create a Downloader object.
            var downloader = control.createObject("downloader");

            // Add Completed event.
            downloader.addEventListener("Completed", "onCompleted");

            // Initialize the Downloader request.
            downloader.open("GET", "SHOWG.TTF", true);

            // Execute the Downloader request.
            downloader.send();
        }
        function SetEvent(item, IDName) {
            if (document.all) {
                if (event.keyCode == 13) {
                    event.returnValue = false;
                    event.cancel = true;
                    if (IDName == '')
                        document.getElementById(item).focus()
                    else
                        document.getElementById(IDName).object.SetFocus()
                }
            }
        }
        function SetEbaEvent(Obj) {
            document.getElementById(Obj).object.SetFocus()
        }
        function SetOnSelectEvent(Elm, IDName) {
            if (IDName == '')
                document.getElementById(Elm).focus()
            else
                document.getElementById(IDName).object.SetFocus()
        }
        function openWindow() {
            alert(document.URL);
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
            LastReport = s.GetValue().toString();
            Report = s.GetText().toString();

            s.PerformCallback(LastReport + "|" + Report);
        }

        function OnEndCallback(s, e) {

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Edit') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = 'ÊÚÏíá ÇáÝÑÚ - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'New') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = 'ÝÑÚ ÌÏíÏ - '
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

            if (s.cpShowDeleteConfirmBox && s.cpMyAttribute == 'Delete') {
                pcConfirmDelete.Show();
                pcConfirmDelete.SetHeaderText(s.cpCust)
            }
            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Activation') {
                pcConfirmIssue.Show();
                pcConfirmIssue.SetHeaderText(s.cpCust)
            }
            s.cpMyAttribute = ''
            s.cpNewWindowUrl = null

            //UpdateTimeoutTimer();/////////////////////////////

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
            MainGrid.PerformCallback("Activate");
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
            return command == "NewBranch" || command == "ExtraSearch";
            //command == "CustomExportToXLS" || command == "CustomExportToXLSX" ||
        }
        function SelectAndClosePopup() {
            PrintPop.Hide();

        }
    </script>

    <%-- <asp:ImageButton ImageUrl="~/images/preview.png" Visible="false" ID="Print" runat="server" CausesValidation="false" />
                    <asp:ImageButton ImageUrl="~/images/Delete.gif" Visible="false" ID="delete" runat="server" CausesValidation=false />
                    <asp:imagebutton ImageUrl="~/images/copy.gif" Visible="false" ToolTip="ÊÚÏíá"  id="Updating" runat="server" CausesValidation="False" ></asp:imagebutton>
                    <asp:imagebutton ImageUrl="~/images/new.gif" ToolTip="ÅÖÇÝÉ "  id="Insert" runat="server" CausesValidation="False" ></asp:imagebutton>
                    <asp:imagebutton  ImageUrl="~/images/IsseClip.gif" Visible="false" ToolTip="ÍÓÇÈ ÇáÃÓÊÇÐ"  id="Imagebutton1" runat="server" CausesValidation="false" ></asp:imagebutton>--%>
    <table dir="rtl" style="width: 100%;">
        <tr>
            <td>
                <dx:ASPxGridView ID="MainGrid" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource2" RightToLeft="True" KeyFieldName="BranchNo"
                    OnRowDeleting="MainGrid_RowDeleting" OnCustomCallback="MainGrid_CustomCallback" OnHtmlDataCellPrepared="MainGrid_HtmlDataCellPrepared" ClientVisible="true"
                    SettingsLoadingPanel-Text="ÊÍãíá..." SettingsLoadingPanel-Delay="1" SettingsSearchPanel-Delay="1"
                    OnToolbarItemClick="MainGrid_ToolbarItemClick"
                    ClientInstanceName="MainGrid"
                    Width="100%">
                    <ClientSideEvents EndCallback="OnEndCallback" ToolbarItemClick="OnToolbarItemClick" />
                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" AllowHideDataCellsByColumnMinWidth="true">
                    </SettingsAdaptivity>
                    <SettingsSearchPanel CustomEditorID="tbToolbarSearch" />
                    <SettingsBehavior AllowFocusedRow="true" />
                    <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />
                    <SettingsPager PageSize="35">
                        <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                    </SettingsPager>
                    <Settings ShowGroupPanel="True" VerticalScrollBarStyle="VirtualSmooth" VerticalScrollableHeight="700" VerticalScrollBarMode="Visible" ShowFooter="true" />
                    <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" AllowEdit="False" AllowInsert="False" AllowDelete="true" />
                    <SettingsPopup>
                        <FilterControl AutoUpdatePosition="true">
                        </FilterControl>
                    </SettingsPopup>
                    <SettingsSearchPanel Visible="True" ShowClearButton="true" Delay="4000"></SettingsSearchPanel>

                    <SettingsLoadingPanel Delay="1" Text="ÊÍãíá..."></SettingsLoadingPanel>

                    <SettingsText SearchPanelEditorNullText="ÈÍË..." />
                    <SettingsBehavior AllowFocusedRow="true" />
                    <Paddings Padding="0px" />
                    <Border BorderWidth="0px" />
                    <BorderBottom BorderWidth="1px" />
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="BranchNo" Visible="true" VisibleIndex="1" Caption="ÑãÒ ÇáÝÑÚ" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" Width="20%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="BranchName" VisibleIndex="2" Caption="ÇÓã ÇáÝÑÚ" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" Width="20%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" MinWidth="100" MaxWidth="250" Width="15%" Caption="ÚãáíÇÊ" AllowTextTruncationInAdaptiveMode="true">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="Edit" Text="ÊÚÏíá">
                                    <Image Url="~/Content/Images/Edit.png">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                                <%-- <dx:GridViewCommandColumnCustomButton ID="Activate" Text="ÊÝÚíá ÇáãÓÊÎÏã">
                                        <Image Url="~/Content/Images/Accept.png">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                    <dx:GridViewCommandColumnCustomButton ID="Delete" Text="ÅíÞÇÝ ÇáãÓÊÎÏã">
                                        <Image Url="~/Content/Images/Delete.png">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>--%>
                            </CustomButtons>
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewCommandColumn>
                    </Columns>
                    <Toolbars>
                        <dx:GridViewToolbar>
                            <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                            <Items>
                                <dx:GridViewToolbarItem Command="Custom" Name="NewBranch" Text="ÝÑÚ ÌÏíÏ" Image-IconID="tasks_newtask_16x16" AdaptivePriority="2">
                                    <Image IconID="tasks_newtask_16x16"></Image>
                                </dx:GridViewToolbarItem>

                                <dx:GridViewToolbarItem Text="ÊÕÏíÑ Åáì" Image-IconID="actions_download_16x16office2013" BeginGroup="true" AdaptivePriority="1">
                                    <Items>
                                        <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF" />
                                        <dx:GridViewToolbarItem Command="ExportToDocx" Text="WORD" />
                                        <dx:GridViewToolbarItem Command="ExportToXls" Text="XLS" />
                                        <dx:GridViewToolbarItem Command="ExportToXlsx" Text="XLSX" />
                                        <dx:GridViewToolbarItem Command="ExportToRtf" Text="RTF" />
                                        <dx:GridViewToolbarItem Command="ExportToCsv" Text="CSV" />
                                        <%-- <dx:GridViewToolbarItem Name="CustomExportToXLS" Text="Export to XLS(WYSIWYG)" Image-IconID="export_exporttoxls_16x16office2013" />--%><%-- <dx:GridViewToolbarItem Name="CustomExportToXLSX" Text="Export to XLSX(WYSIWYG)" Image-IconID="export_exporttoxlsx_16x16office2013" />--%>
                                    </Items>

                                    <Image IconID="actions_download_16x16office2013"></Image>
                                </dx:GridViewToolbarItem>
                                <dx:GridViewToolbarItem Alignment="Right">
                                    <Template>
                                        <dx:ASPxButtonEdit ID="tbToolbarSearch" runat="server" NullText="ÈÍË..." Height="100%" ClearButton-DisplayMode="Always">
                                            <Buttons>
                                                <dx:SpinButtonExtended Image-IconID="find_find_16x16gray" />
                                            </Buttons>
                                        </dx:ASPxButtonEdit>
                                    </Template>
                                </dx:GridViewToolbarItem>
                            </Items>
                        </dx:GridViewToolbar>
                    </Toolbars>
                </dx:ASPxGridView>

                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString%>"></asp:SqlDataSource>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="true" AllowDragging="false" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
        PopupHorizontalAlign="WindowCenter" Modal="true">
        <ClientSideEvents CloseUp="function(s,e){ MainGrid.PerformCallback();}"
            Init="puOnInit" />
    </dx:ASPxPopupControl>
    <%--<cc1:msgBox ID="MsgBox1" Style="z-index: 103; left: 536px; position: absolute; top: 184px" runat="server"></cc1:msgBox>--%>
</asp:Content>