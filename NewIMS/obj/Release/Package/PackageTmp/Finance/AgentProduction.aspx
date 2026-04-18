<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="AgentProduction.aspx.vb" Inherits=".AgentProduction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function OnToolbarItemClick(s, e) {
            if (IsCustomExportToolbarCommand(e.item.name)) {
               //e.processOnServer = true;
                pcConfirmIssue.Show();
                //e.usePostBack = true;
            }
        }

        function IsCustomExportToolbarCommand(command) {
            return command == "IssueAgentJournal";
            //command == "CustomExportToXLS" || command == "CustomExportToXLSX" ||
        }

        function YesIss_Click() {
            pcConfirmIssue.Hide();
            ProductionGrid.PerformCallback("IssueJournal");
        }

        function NoIss_Click() {
            pcConfirmIssue.Hide();
        }
    </script>
    <script type="text/javascript">

                 function OnToolbarItemClick(s, e) {
                     if (e.item.name == "IssueAgentJournal") {
                         // Disable the button
                         e.item.SetEnabled(false);
                         pcConfirmIssue.Show();
                         // Optionally show a loading indicator

                         // Process the action (this will call your server-side code)
                         // After completion, re-enable in the callback
                     }
                     if (e.item.name == "IssuesAgent") {
                         ProductionGrid.PerformCallback("IssuesAgent");
                     }
                 }
                 function OnEndCallback(s, e) {
                     if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                         let hdr = s.cpResult
                         let mhdr = 'طباعة - '
                        // alert(s.cpResult);
                         PrintPop.SetHeaderText(mhdr.concat(hdr));
                         PrintPop.SetContentHtml(null);
                         PrintPop.SetContentUrl(s.cpNewWindowUrl);
                         PrintPop.Show();
                     }
                     else {
                         isProcessing = false;
                         ProductionGrid.GetToolbar(0).GetItemByName("IssueAgentJournal").SetEnabled(true);
                         LoadingPanel.Hide();
                         ProductionGrid.SetEnabled(true);
                     }
                     s.cpMyAttribute = ''
                     s.cpNewWindowUrl = null
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
                     var width = Math.max(0, document.documentElement.clientWidth) * 0.97;
                     var height = Math.max(0, document.documentElement.clientHeight) * 0.97;
                     PrintPop.SetWidth(width);
                     PrintPop.SetSize(width, height);
                 }
                 function IsCustomExportToolbarCommand(command) {
                     return command == "IssueAgentJournal";
                     //command == "CustomExportToXLS" || command == "CustomExportToXLSX" ||
                 }

                 function YesIss_Click() {
                     pcConfirmIssue.Hide();
                     ProductionGrid.PerformCallback("IssueJournal");
                     ProductionGrid.SetEnabled(false);
                     LoadingPanel.Show();
                 }

                 function NoIss_Click() {
                     pcConfirmIssue.Hide();
                     ProductionGrid.GetToolbar(0).GetItemByName("IssueAgentJournal").SetEnabled(true);
                     LoadingPanel.Hide();
                     ProductionGrid.SetEnabled(true);
                 }
    </script>
    <div dir="rtl">
        <table runat="server" style="width: 100%;">
            <tr>
                <td style="text-align: left">للوكيل :</td>
                <td>
                    <dx:ASPxComboBox ID="Agent" runat="server" ClientInstanceName="Agent" DataSourceID="SqlDataSource"
                        SelectedIndex="-1" ValueType="System.String"
                        DropDownStyle="DropDownList" RightToLeft="True"
                        TextField="BranchName"
                        ValueField="BranchNo"
                        Width="450px">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) { ProductionGrid.PerformCallback(); }" />
                    </dx:ASPxComboBox>
                </td>
                <td style="text-align: left">لشهر :</td>
                <td>
                    <dx:ASPxDateEdit ID="ProdDate" runat="server" PickerType="Months" OnDateChanged="Page_Load">
                        <ClientSideEvents DateChanged="function(s, e) { ProductionGrid.PerformCallback();}" />
                    </dx:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: center" colspan="4"></td>
            </tr>
        </table>

        <dx:ASPxGridView ID="ProductionGrid"
            ClientInstanceName="ProductionGrid" runat="server"
            EnableCallBacks="true"
            ClientSideEvents-ColumnGrouping="Comment"
            Settings-ShowTitlePanel="true"
            AutoGenerateColumns="False"
            OnCustomCallback="ProductionGrid_CustomCallback"
            OnToolbarItemClick="ProductionGrid_ToolbarItemClick"
            OnHtmlRowPrepared="ProductionGrid_HtmlRowPrepared"
            DataSourceID="SqlDataSource1" Width="100%"
            SettingsLoadingPanel-Text="تحميل..."
            SettingsLoadingPanel-Delay="1"
            SettingsSearchPanel-Delay="1"
            SettingsLoadingPanel-Mode="Disabled">
            <SettingsPager PageSize="40" NumericButtonCount="40"></SettingsPager>
            <Settings ShowFooter="True"></Settings>
            <SettingsText Title="" />
            <ClientSideEvents ToolbarItemClick="OnToolbarItemClick" EndCallback="OnEndCallback" />
            <Toolbars>
                <dx:GridViewToolbar Name="Commands">
                    <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                    <Items>
                        <dx:GridViewToolbarItem Command="Custom" Name="IssueAgentJournal" Text="ترحيل قيد الإنتاج"
                            Image-IconID="tasks_newtask_16x16" AdaptivePriority="2" />
                        <dx:GridViewToolbarItem Command="Custom" Name="IssuesAgent" Text="تقرير المكاتب/ والوكلاء الغير مرحلة"
                            Image-IconID="tasks_newtask_16x16" AdaptivePriority="3" />
                    </Items>
                </dx:GridViewToolbar>
            </Toolbars>
            <SettingsPopup>
                <FilterControl AutoUpdatePosition="False">
                </FilterControl>
            </SettingsPopup>
            <SettingsBehavior AllowFocusedRow="true" />

            <Columns>
                <dx:GridViewDataTextColumn FieldName="Cnt" ReadOnly="True" VisibleIndex="0" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SubIns" ReadOnly="True" VisibleIndex="1" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AccountNumber" Caption="اسم الحساب" VisibleIndex="2">
                    <DataItemTemplate>
                        <dx:ASPxLabel ID="Accnam" runat="server" OnDataBound="Accnam_DataBound"
                            Value='<%# Eval("AccountNumber") %>'>
                        </dx:ASPxLabel>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AccountNumber" Caption="رقم الحساب" ReadOnly="True" VisibleIndex="3" Visible="true">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AccTp" ReadOnly="True" VisibleIndex="4" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="DR" ReadOnly="True" VisibleIndex="5" Caption="مدين" PropertiesTextEdit-DisplayFormatString="n3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CR" ReadOnly="True" VisibleIndex="6" Caption="دائن" PropertiesTextEdit-DisplayFormatString="n3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Acc" ReadOnly="True" VisibleIndex="7" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Paytype" ReadOnly="True" VisibleIndex="8" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Branch" ReadOnly="True" VisibleIndex="9" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Comment" Caption="وصف القيد" VisibleIndex="10" ReadOnly="True">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Currency" VisibleIndex="11" ReadOnly="True" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ExcRate" ReadOnly="True" VisibleIndex="12" Visible="false">
                </dx:GridViewDataTextColumn>
            </Columns>
            <TotalSummary>
                <dx:ASPxSummaryItem ShowInColumn="مدين" SummaryType="Sum" FieldName="DR"></dx:ASPxSummaryItem>
                <dx:ASPxSummaryItem ShowInColumn="دائن" SummaryType="Sum" FieldName="CR"></dx:ASPxSummaryItem>
            </TotalSummary>
        </dx:ASPxGridView>
        <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="true" AllowDragging="true" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
            PopupHorizontalAlign="WindowCenter" Modal="True">
            <ClientSideEvents Init="puOnInit" />
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pcConfirmIssue" runat="server" ClientInstanceName="pcConfirmIssue" ShowCloseButton="false"
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            HeaderText="تأكيد ترحيل القيد">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server" Font-Bold="true">
                    <table dir="rtl" style="width: 100%">
                        <tr>
                            <td colspan="2"><%--<a href="javascript:YesIss_Click()">موافق</a>--%>هل انت متأكد من ترحيل القيد ؟
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
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
            SelectCommand="JournalEntryMonthlyAgentBulk" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="Agent" PropertyName="Value" Name="Br" Type="String"></asp:ControlParameter>
                <asp:SessionParameter SessionField="Mnth" Name="Month" Type="Int32"></asp:SessionParameter>
                <asp:SessionParameter SessionField="Yer" Name="Year" Type="Int32"></asp:SessionParameter>
            </SelectParameters>
        </asp:SqlDataSource>
        <%--agent=1 and--%>
        <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
            SelectCommand="SELECT rtrim(BranchNo) as BranchNo,rtrim(BranchName) as BranchName from BranchInfo where Left(BranchNo, 2)= left(@Br,2) and right(BranchInfo.BranchNo,3)<>'000' ">
            <SelectParameters>
                <asp:SessionParameter SessionField="Branch" Name="Br" Type="String"></asp:SessionParameter>
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>