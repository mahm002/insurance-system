<%@ Page Language="VB" AutoEventWireup="false" Inherits="OfficeForm" CodeBehind="OfficeForm.aspx.vb" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>الوكلاء</title>
    <link href="../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/Scripts.js"></script>
    <script src="../scripts/jquery-latest.min.js"></script>
    <script src="../scripts/jquery.min.js"></script>
    <script type="text/javascript">

        $(document).on('keyup', 'input', function (e) {
            if (e.keyCode == 13 && e.target.type !== 'submit') {
                var inputs = $(e.target).parents("form").eq(0).find('input,a,select,button,textarea').filter(':visible:enabled');
                idx = inputs.index(e.target);
                if (idx == inputs.length - 1) {
                    inputs[0].select()
                } else {
                    inputs[idx + 1].focus();
                    inputs[idx + 1].select();
                }
            }
        });
        document.addEventListener("contextmenu", function (e) {
            e.preventDefault();
        }, false);
        function OnComboRe_ButtonClick(s, e) {
            popup.ShowAtElement(s.GetMainElement());
        }
        // clear the combo box selection
        function OnComboRe_EndCallback(s, e) {
            s.SetSelectedIndex(-1);
        }
        function OnEndCallback(s, e) {

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Edit') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = 'تعديل المستخدم - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'ResetPass') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = 'إعادة تعيين كلمة المرور - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'New') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult
                let mhdr = 'مستخدم جديد - '
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
            AgentUsers.DeleteRow(AgentUsers.cpRowIndex);
        }

        function No_Click() {
            pcConfirmDelete.Hide()
        }

        function YesIss_Click() {
            pcConfirmIssue.Hide();
            AgentUsers.PerformCallback("Activate");
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
            var height = Math.max(0, document.documentElement.clientHeight) * 0.40;
            PrintPop.SetWidth(width);
            PrintPop.SetSize(width, height);
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div >
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents EndCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="100" ShowImage="false" Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table border="0" dir="rtl" style="width: 100%;">
                            <tr>
                                <td dir="rtl" class="dxeICC">اسم الوكيل :</td>
                                <td>
                                    <dx:ASPxTextBox runat="server" Width="350px" ID="BranchName">
                                       <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="  "
                                                ValidationExpression="^[A-Za-z0-9اأإ-يءئ \\-]{5,40}$" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeCaptionHARSys">
                                    <dx:ASPxRadioButtonList ID="AgentOrOffice" runat="server" ValueType="System.Boolean" Width="100%" 
                                        RightToLeft="True" RepeatLayout="OrderedList">
                                        <RadioButtonStyle>
                                            <BackgroundImage HorizontalPosition="center" VerticalPosition="center" />
                                        </RadioButtonStyle>
                                        <Items>
                                            <dx:ListEditItem Value="False" Text="مكتب" Selected="false" />
                                            <dx:ListEditItem Value="True" Text="وكيل" Selected="true" />
                                        </Items>
                                    </dx:ASPxRadioButtonList>
                                </td>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dxeICC">رمز الوكيل :</td>
                                <td style="width: 131px">
                                    <dx:ASPxTextBox ID="BranchNo" runat="server" Width="32px">
                                        <MaskSettings Mask="000" ErrorText="يرجى إدخال ثلاثة أرقام " />
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeCaptionHARSys">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server"
                                        Text="هذا الوكيل موجود مسبقاً يرجى ادخال رمز مختلف"
                                        Visible="false" RightToLeft="True">
                                    </dx:ASPxLabel>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dxeICC">العنوان :</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox runat="server" Width="100%" ID="Address">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="width: 104px"></td>
                            </tr>
                            <tr>
                                <td class="dxeICC">رقم الهاتف :</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox runat="server" Width="100%" ID="Telephone">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="width: 104px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dxeICC">رقم&nbsp; الفاكس :</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox runat="server" Width="100%" ID="FaxNo">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="width: 104px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dxeICC">البريد الإلكتروني :</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox runat="server" Width="100%" ID="eMail">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="حروف فقط" ValidationExpression="^[A-Za-z0-9.-_ \\@-.]+"></RegularExpression>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="width: 104px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dxeICC" rowspan="2">نسب العمولة :</td>
                                <td colspan="3" rowspan="2">

                                    <dx:ASPxGridView ID="AgentCommGrid" runat="server" AutoGenerateColumns="False"
                                        DataSourceID="AgentsComm" SettingsBehavior-ConfirmDelete="true"
                                        OnRowDeleted="AgentCommGrid_RowDeleted"
                                        OnRowDeleting="AgentCommGrid_RowDeleting"
                                        KeyFieldName="Id; AgentNo" RightToLeft="True" Width="100%">
                                        <SettingsEditing EditFormColumnCount="6" Mode="Inline">
                                        </SettingsEditing>

                                        <SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False">
                                            </FilterControl>
                                        </SettingsPopup>
                                        <SettingsText CommandCancel="تراجع" CommandDelete="حذف" CommandEdit="تعديل" CommandNew="إضافة" CommandUpdate="حفظ" PopupEditFormCaption="بيانات الحساب والعمولة" />
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="ref" FieldName="Id" ShowInCustomizationForm="True" VisibleIndex="9" Visible="false">
                                                <EditFormSettings Visible="false" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewCommandColumn ShowInCustomizationForm="True" ShowNewButtonInHeader="True" VisibleIndex="0" ShowEditButton="True" ShowDeleteButton="True">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataComboBoxColumn Caption="رقم الحساب / جاري وكلاء ومكاتب التأمين" FieldName="AccountNo" ShowInCustomizationForm="True" VisibleIndex="6">
                                                <PropertiesComboBox DataSourceID="Accounts" TextField="AccountName" ValueField="AccountNo">
                                                </PropertiesComboBox>
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn Caption="نوع التأمين" FieldName="SubIns" ShowInCustomizationForm="True" VisibleIndex="6">
                                                <PropertiesComboBox DataSourceID="AgentSys" TextField="SUBSYSNAME" ValueField="SUBSYSNO">
                                                </PropertiesComboBox>
                                                <EditFormSettings Visible="True" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataComboBoxColumn>

                                            <dx:GridViewDataTextColumn Caption="رمز النظام" FieldName="SubIns" ShowInCustomizationForm="True" Visible="False" VisibleIndex="7">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="رمز الوكيل" FieldName="AgentNo" ShowInCustomizationForm="True" Visible="False" VisibleIndex="9">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="العمولة" FieldName="Comm" ShowInCustomizationForm="True" VisibleIndex="9">
                                                <EditFormSettings Visible="True" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                     <asp:SqlDataSource ID="AgentSys" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select SUBSYSNO ,rtrim(SUBSYSNAME) As SUBSYSNAME from SUBSYSTEMS where Branch=@AgentNo and SysType=1">
                                        <SelectParameters>
                                            <asp:SessionParameter SessionField="AgentNo" DefaultValue="0" Name="AgentNo" Type="String"></asp:SessionParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td style="width: 104px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 104px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dxeICC">&nbsp;</td>
                                <td>
                                    <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) { cbp.PerformCallback('Apply'); }" />
                                    </dx:ASPxButton>
                                </td>
                                <td >
                                    <dx:ASPxComboBox ID="OfficeManager" runat="server" DataSourceID="OfficeManagers"
                                        Caption="مدير المكتب" ValueType="System.Int32" Width="50%"
                                        TextField="Userfullname" ValueField="AccountNo" DropDownStyle="DropDown"
                                        IncrementalFilteringMode="Contains" RightToLeft="True" SelectedIndex="-1">
                                    </dx:ASPxComboBox>
                                    <asp:SqlDataSource ID="Branches" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>" SelectCommand="select * from BranchInfo where agent=1"></asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>

                                <td colspan="3">
                                    <asp:SqlDataSource ID="OfficeManagers" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select AccountNo,AccountName,BranchName,AccountName + '/' + BranchName As Userfullname
from AccountFile left join BranchInfo on BranchInfo.BranchNo=AccountFile.Branch
where len(rtrim(AccountSysManag))&lt;&gt;0 and BranchInfo.Agent=0 and BranchInfo.CompanyOffice=1
AND AccountName&lt;&gt;'branchAdmin'"></asp:SqlDataSource>
                                    
                                </td>
                                <td style="width: 104px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dxeICC">&nbsp;</td>
                                <td colspan="3">
                                    <dx:ASPxGridView ID="AgentUsers" ClientInstanceName="AgentUsers" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2"
                                        KeyFieldName="AccountNo"
                                        OnCustomCallback="AgentUsers_CustomCallback" OnHtmlDataCellPrepared="AgentUsers_HtmlDataCellPrepared"
                                        OnRowDeleting="AgentUsers_RowDeleting" OnToolbarItemClick="AgentUsers_ToolbarItemClick" RightToLeft="True" Width="100%">
                                        <ClientSideEvents EndCallback="OnEndCallback" ToolbarItemClick="OnToolbarItemClick" />
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowHideDataCellsByColumnMinWidth="True" AllowOnlyOneAdaptiveDetailExpanded="True">
                                        </SettingsAdaptivity>
                                        <SettingsPager PageSize="10">
                                            <PageSizeItemSettings ShowAllItem="True" Visible="True">
                                            </PageSizeItemSettings>
                                        </SettingsPager>
                                        <Settings ShowFooter="True" ShowGroupPanel="True" VerticalScrollBarMode="Visible" VerticalScrollBarStyle="VirtualSmooth" />
                                        <SettingsBehavior AllowFocusedRow="True" />
                                        <SettingsDataSecurity AllowEdit="False" AllowInsert="False" AllowReadUnlistedFieldsFromClientApi="True" />
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False">
                                            </FilterControl>
                                        </SettingsPopup>
                                        <SettingsSearchPanel CustomEditorID="tbToolbarSearch" Delay="4000" ShowClearButton="True" Visible="True" />
                                        <SettingsExport EnableClientSideExportAPI="True" ExcelExportMode="DataAware">
                                        </SettingsExport>
                                        <SettingsLoadingPanel Delay="1" Text="تحميل..." />
                                        <SettingsText SearchPanelEditorNullText="بحث..." />
                                        <Columns>
                                            <dx:GridViewDataTextColumn AllowTextTruncationInAdaptiveMode="True" Caption="ID" FieldName="AccountNo" MinWidth="150" ShowInCustomizationForm="True" Visible="False" Width="20%">
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn AllowTextTruncationInAdaptiveMode="True" Caption="اسم المستخدم" FieldName="AccountName" MinWidth="150" ShowInCustomizationForm="True" VisibleIndex="1" Width="20%">
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn AllowTextTruncationInAdaptiveMode="True" Caption="رمز الدخول" FieldName="AccountLogIn" MaxWidth="250" MinWidth="150" ShowInCustomizationForm="True" VisibleIndex="2" Width="20%">
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn AllowTextTruncationInAdaptiveMode="True" Caption="الفرع أو الوكيل التايع له" FieldName="BrName" MaxWidth="250" MinWidth="150" ShowInCustomizationForm="True" VisibleIndex="3" Width="20%">
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn AllowTextTruncationInAdaptiveMode="True" Caption="حالة المستخدم" FieldName="Stop" MaxWidth="250" MinWidth="150" ShowInCustomizationForm="True" Visible="False" VisibleIndex="4" Width="20%">
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn AllowTextTruncationInAdaptiveMode="True" Caption="رمز الوكيل" FieldName="Branch" MaxWidth="250" MinWidth="150" ShowInCustomizationForm="True" Visible="False" VisibleIndex="5" Width="20%">
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewCommandColumn AllowTextTruncationInAdaptiveMode="True" ButtonRenderMode="Image" ButtonType="Image" Caption="عمليات" MaxWidth="250" MinWidth="100" ShowInCustomizationForm="True" VisibleIndex="0" Width="15%">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="Edit" Text="تعديل">
                                                        <Image Url="~/Content/Images/Edit.png">
                                                        </Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                    <dx:GridViewCommandColumnCustomButton ID="Activate" Text="تفعيل المستخدم">
                                                        <Image Url="~/Content/Images/Accept.png">
                                                        </Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                    <dx:GridViewCommandColumnCustomButton ID="Delete" Text="إيقاف المستخدم">
                                                        <Image Url="~/Content/Images/Delete.png">
                                                        </Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                    <dx:GridViewCommandColumnCustomButton ID="ResetPass" Text="إعادة تعيين كلمة المرور">
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
                                                <Items>
                                                    <dx:GridViewToolbarItem AdaptivePriority="2" Name="NewUser" Text="مستخدم جديد">
                                                        <Image IconID="tasks_newtask_16x16">
                                                        </Image>
                                                    </dx:GridViewToolbarItem>
                                                    <dx:GridViewToolbarItem AdaptivePriority="1" BeginGroup="True" Text="تصدير إلى">
                                                        <Items>
                                                            <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF">
                                                            </dx:GridViewToolbarItem>
                                                            <dx:GridViewToolbarItem Command="ExportToDocx" Text="WORD">
                                                            </dx:GridViewToolbarItem>
                                                            <dx:GridViewToolbarItem Command="ExportToXls" Text="XLS">
                                                            </dx:GridViewToolbarItem>
                                                            <dx:GridViewToolbarItem Command="ExportToXlsx" Text="XLSX">
                                                            </dx:GridViewToolbarItem>
                                                            <dx:GridViewToolbarItem Command="ExportToRtf" Text="RTF">
                                                            </dx:GridViewToolbarItem>
                                                            <dx:GridViewToolbarItem Command="ExportToCsv" Text="CSV">
                                                            </dx:GridViewToolbarItem>
                                                        </Items>
                                                        <Image IconID="actions_download_16x16office2013">
                                                        </Image>
                                                    </dx:GridViewToolbarItem>
                                                    <dx:GridViewToolbarItem Alignment="Right">
                                                        <Template>
                                                            <dx:ASPxButtonEdit ID="tbToolbarSearch" runat="server" ClearButton-DisplayMode="Always" Height="100%" NullText="بحث...">
                                                                <Buttons>
                                                                    <dx:SpinButtonExtended Image-IconID="find_find_16x16gray" />
                                                                </Buttons>
                                                            </dx:ASPxButtonEdit>
                                                        </Template>
                                                    </dx:GridViewToolbarItem>
                                                </Items>
                                                <SettingsAdaptivity EnableCollapseRootItemsToIcons="True" Enabled="True" />
                                            </dx:GridViewToolbar>
                                        </Toolbars>
                                        <Paddings Padding="0px" />
                                        <Border BorderWidth="0px" />
                                        <BorderBottom BorderWidth="1px" />
                                    </dx:ASPxGridView>
                                </td>
                                <td style="width: 104px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="BranchesDD" runat="server" DataSourceID="Branches" DataTextField="BranchName" DataValueField="BranchNo" Style="margin-right: 0px" Visible="False" Width="176px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label3" runat="server" CssClass="Caption" Style="text-align: right" Text="الفرع أو الوكيل" Visible="False"></asp:Label>
                                </td>

                                <td colspan="4" rowspan="2">
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" DeleteCommand="Update AccountFile Set Stop=1 where AccountNo=@AccountNo"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"></asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="auto-style3" dir="rtl" valign="top"></td>
                            </tr>
                            <tr>
                                <td align="left" class="style4" dir="rtl">
                                    <asp:Button ID="Button1" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="CaptionM" Font-Bold="True" TabIndex="8" Text="حفظ" UseSubmitBehavior="False" Visible="False" Width="120px" />
                                </td>
                                <td colspan="3" style="height: 24px">
                                    <asp:TextBox ID="syss" runat="server" ForeColor="White" Visible="False"></asp:TextBox>

                                    <asp:SqlDataSource ID="AgentsComm" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                                        SelectCommand="SELECT Id,SUBSYSNAME,SubIns,Comm,AgentNo, AgentsCommisions.AccountNo,AccountName+'/'+ AgentsCommisions.AccountNo as AccountName
                            FROM AgentsCommisions left join
                            SUBSYSTEMS on SUBSYSTEMS.Branch=AgentsCommisions.AgentNo AND SUBSYSTEMS.SUBSYSNO=AgentsCommisions.SubIns AND SUBSYSTEMS.SysType=1 left join
                            Accounts on accounts.accountno=AgentsCommisions.AccountNo
                            WHERE AgentNo = @AgentNo"
                                        InsertCommand="INSERT INTO AgentsCommisions (AgentNo,SubIns,Comm,AccountNo) VALUES (@AgentNo, @SubIns, @Comm, @AccountNo)"
                                        UpdateCommand="UPDATE AgentsCommisions SET SubIns = @SubIns, Comm = @Comm , AccountNo = @AccountNo WHERE Id=@Id"
                                        DeleteCommand="DELETE FROM AgentsCommisions WHERE Id=@Id">
                                        <InsertParameters>
                                            <asp:SessionParameter SessionField="AgentNo" Name="AgentNo" Type="String"></asp:SessionParameter>
                                            <asp:Parameter Name="SubIns" Type="String"></asp:Parameter>
                                            <asp:Parameter Name="Comm" Type="Double"></asp:Parameter>
                                            <asp:Parameter Name="AccountNo" Type="String"></asp:Parameter>
                                        </InsertParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="SubIns" Type="String"></asp:Parameter>
                                            <asp:Parameter Name="Comm" Type="Double"></asp:Parameter>
                                            <asp:Parameter Name="AccountNo" Type="String"></asp:Parameter>
                                            <asp:Parameter Name="Id" Type="Int64"></asp:Parameter>
                                        </UpdateParameters>
                                        <DeleteParameters>
                                            <asp:Parameter Name="Id" Type="Int64"></asp:Parameter>
                                        </DeleteParameters>
                                        <SelectParameters>
                                            <asp:SessionParameter SessionField="AgentNo" Name="AgentNo" Type="String"></asp:SessionParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td colspan="1" style="height: 24px">
                                    <asp:SqlDataSource ID="Accounts" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName
                            From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=5 and (left(AccountNo,7)='1.1.3.3')"></asp:SqlDataSource>

                                    <dx:ASPxPopupControl ID="popup" runat="server" AllowDragging="True" ClientInstanceName="popup" EnableClientSideAPI="True"
                                        EnableViewState="False" EncodeHtml="False" HeaderText="نوع" PopupHorizontalAlign="OutsideRight" PopupHorizontalOffset="20">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server">
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                    <dx:ASPxPopupControl ID="pcConfirmDelete" runat="server" ClientInstanceName="pcConfirmDelete" ShowCloseButton="false"
                                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                        HeaderText="إيقاف المستخدم">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server" Font-Bold="true">
                                                <table dir="rtl" style="width: 100%">
                                                    <tr>
                                                        <td style="vertical-align: central;" colspan="2">سيتم إيقاف المستخدم ؟
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
            HeaderText="إعادة تفعيل المستخدم">
        <ContentCollection>
            
            <dx:PopupControlContentControl runat="server" Font-Bold="true">
                <table dir="rtl" style="width :100%">
                    <tr>
                        <td colspan="2"><%--<a href="javascript:YesIss_Click()">موافق</a>--%>
                            تأكيد تفعيل المستخدم؟
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
                                   
                                </td>
                            </tr>
                            <%--<cc1:msgBox ID="MsgBox1" Style="z-index: 103; left: 536px; position: absolute; top: 184px" runat="server"></cc1:msgBox>--%>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="PrintPop" runat="server" AllowResize="true" AllowDragging="false" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
                PopupHorizontalAlign="WindowCenter" Modal="true">
                <ClientSideEvents CloseUp="function(s,e){AgentUsers.PerformCallback();}"
                    Init="puOnInit" />
            </dx:ASPxPopupControl>

   
        </div>
    </form>
</body>
</html>