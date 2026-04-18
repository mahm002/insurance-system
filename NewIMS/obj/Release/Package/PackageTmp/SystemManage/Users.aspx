 <%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" Inherits="Users" Codebehind="users.aspx.vb" %> 

<asp:Content  ID="HeaderContent" ContentPlaceHolderID="MainContent" Runat="Server">
<script src="../Scripts/jquery-3.7.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery.quicksearch.js" type="text/javascript"></script>
    <script type="text/javascript">
       
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
                let mhdr = ' ⁄œÌ· «·„” Œœ„ - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'ResetPass') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = '≈⁄«œ…  ⁄ÌÌ‰ ﬂ·„… «·„—Ê— - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'New') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = '„” Œœ„ ÃœÌœ - '
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
            return command == "NewUser" || command == "ExtraSearch";
            //command == "CustomExportToXLS" || command == "CustomExportToXLSX" ||
        }
        function SelectAndClosePopup() {
            PrintPop.Hide();
        }
    </script>
        <table style="width: 100%;">
            <tr>
                <td>
                    <dx:ASPxComboBox ID="BranchNo" runat="server" RightToLeft="True"
                        ValueField="BranchNo" TextField="BranchName" DataSourceID="SqlDataSource1">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {MainGrid.PerformCallback();}" />
                    </dx:ASPxComboBox>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>">
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="UsersDS" runat="server" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>' 
                        SelectCommand="SELECT DISTINCT * FROM [AccountFile] WHERE ([Branch] = @Branch)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="BranchNo" PropertyName="Value" DefaultValue="0" Name="Branch" Type="String"></asp:ControlParameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="BranchUsersDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString%>"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="AgentUsersDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString%>"></asp:SqlDataSource>
                    
                    <dx:ASPxGridView ID="MainGrid" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource2" RightToLeft="True" 
                        KeyFieldName="AccountNo"
                        OnRowDeleting="MainGrid_RowDeleting" OnCustomCallback="MainGrid_CustomCallback" 
                        OnHtmlDataCellPrepared="MainGrid_HtmlDataCellPrepared" ClientVisible="true"
                        SettingsLoadingPanel-Text=" Õ„Ì·..." SettingsLoadingPanel-Delay="1" SettingsSearchPanel-Delay="1"
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

                        <SettingsLoadingPanel Delay="1" Text=" Õ„Ì·..."></SettingsLoadingPanel>

                        <SettingsText SearchPanelEditorNullText="»ÕÀ..." />
                        <SettingsBehavior AllowFocusedRow="true" />
                        <Paddings Padding="0px" />
                        <Border BorderWidth="0px" />
                        <BorderBottom BorderWidth="1px" />
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="AccountNo" Visible="false" Caption="ID" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" Width="20%">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="AccountName" VisibleIndex="1" Caption="«”„ «·„” Œœ„" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" Width="20%">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="AccountLogIn" VisibleIndex="2" Caption="—„“ «·œŒÊ·" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" MaxWidth="250" Width="20%">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="BrName" VisibleIndex="3" Caption="«”„ «·›—⁄" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" MinWidth="150" MaxWidth="250" Width="20%">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Stop" VisibleIndex="4" Caption="Õ«·… «·„” Œœ„" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" Visible="false" MinWidth="150" MaxWidth="250" Width="20%">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Branch" VisibleIndex="5" Caption="—„“ «·›—⁄" CellStyle-HorizontalAlign="Right" AllowTextTruncationInAdaptiveMode="true" Visible="false" MinWidth="150" MaxWidth="250" Width="20%">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" MinWidth="100" MaxWidth="250" Width="15%" Caption="⁄„·Ì« " AllowTextTruncationInAdaptiveMode="true">
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="Edit" Text=" ⁄œÌ·">
                                        <Image Url="~/Content/Images/Edit.png">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                    <dx:GridViewCommandColumnCustomButton ID="Activate" Text=" ›⁄Ì· «·„” Œœ„">
                                        <Image Url="~/Content/Images/Accept.png">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                    <dx:GridViewCommandColumnCustomButton ID="Delete" Text="≈Ìﬁ«› «·„” Œœ„">
                                        <Image Url="~/Content/Images/Delete.png">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                    <dx:GridViewCommandColumnCustomButton ID="ResetPass" Text="≈⁄«œ…  ⁄ÌÌ‰ ﬂ·„… «·„—Ê—">
                                        <Image Url="~/Content/Images/ResetPassword1.png">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewCommandColumn>
                        </Columns>
                        <Toolbars>
                            <dx:GridViewToolbar>
                                <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                                <Items>
                                    <dx:GridViewToolbarItem Command="Custom" Name="NewUser" Text="„” Œœ„ ÃœÌœ" Image-IconID="tasks_newtask_16x16" AdaptivePriority="2">
                                        <Image IconID="tasks_newtask_16x16"></Image>
                                    </dx:GridViewToolbarItem>

                                    <dx:GridViewToolbarItem Text=" ’œÌ— ≈·Ï" Image-IconID="actions_download_16x16office2013" BeginGroup="true" AdaptivePriority="1">
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
                                            <dx:ASPxButtonEdit ID="tbToolbarSearch" runat="server" NullText="»ÕÀ..." Height="100%" ClearButton-DisplayMode="Always">
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
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString%>"
                        DeleteCommand="Update AccountFile Set Stop=1 where AccountNo=@AccountNo" >
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="true" AllowDragging="false" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
        PopupHorizontalAlign="WindowCenter" Modal="true">
        <ClientSideEvents CloseUp="function(s,e){ MainGrid.PerformCallback();}"
            Init="puOnInit" />
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="pcConfirmDelete" runat="server" ClientInstanceName="pcConfirmDelete" ShowCloseButton="false"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="≈Ìﬁ«› «·„” Œœ„">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" Font-Bold="true">
                <table dir="rtl" style="width :100%">
                    <tr>
                        <td style="vertical-align: central;" colspan="3">”Ì „ ≈Ìﬁ«› «·„” Œœ„ ø
                        </td>
                    </tr>
                    <tr>
                        <td><%--<a href="javascript:Yes_Click()">‰⁄„</a>--%>
                            <dx:ASPxButton ID="yesButton" runat="server" Text="‰⁄„" AutoPostBack="false">
                                <ClientSideEvents Click="Yes_Click" />
                            </dx:ASPxButton>
                        </td>
                        <td><%-- <a href="javascript:No_Click()">·«</a>--%>
                            <dx:ASPxButton ID="noButton" runat="server" Text="·«" AutoPostBack="false">
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
            HeaderText="≈⁄«œ…  ›⁄Ì· «·„” Œœ„">
        <ContentCollection>
            
            <dx:PopupControlContentControl runat="server" Font-Bold="true">
                <table dir="rtl" style="width :100%">
                    <tr>
                        <td colspan="2"><%--<a href="javascript:YesIss_Click()">„Ê«›ﬁ</a>--%>
                             √ﬂÌœ  ›⁄Ì· «·„” Œœ„ø
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="yesIssButton" runat="server" AutoPostBack="False" Text="„Ê«›ﬁ">
                                <ClientSideEvents Click="YesIss_Click" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="noIssButton" runat="server" AutoPostBack="False" Text="€Ì— „Ê«›ﬁ">
                                <ClientSideEvents Click="NoIss_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

 </asp:Content>
