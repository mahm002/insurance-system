<%@ Page Title="" Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="ClaimsManage_Default" CodeBehind="Default.aspx.vb" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        var rowVisibleIndex;
        var Vals;

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
            popup.SetWidth(width);
            popup.SetSize(width, height);
        }
        function GetPickUpPoints(values) {
            PickUpPoints = values;
            //debugger;

            if (rowVisibleIndex == 'طباعة') {
                values = rowVisibleIndex + ',' + values
                GV2.PerformCallback(values);
                rowVisibleIndex = '';
            }
            if (rowVisibleIndex == 'إقرار سداد ومخالصة') {
                values = rowVisibleIndex + ',' + values
                GV2.PerformCallback(values);

                rowVisibleIndex = '';
            }
            if (rowVisibleIndex == 'توزيع') {

                //popup.SetSize(1300, 950);
                popup.SetHeaderText(' توزيع الحادث ');
                popup.SetContentHtml(null);
                popup.SetContentUrl('../ReIns/DestributeClaim.aspx?ClmNo=' + values);
                popup.Show()

                rowVisibleIndex = '';
            }
            if (rowVisibleIndex == 'معاينة') {
                //debugger;
                //popup.SetSize(1300, 950);
                popup.SetHeaderText('معاينة الملف');
                popup.SetContentHtml(null);
                popup.SetContentUrl('../Claims/ClmOpen/OpenClmFile.aspx?ClmNo=' + values[0] + '&Sys=' + values[1]);
                popup.Show();
            }
            if (rowVisibleIndex == 'تقرير بيانات الحادث') {
                ///debugger;
                //alert(values);
                values = rowVisibleIndex + ',' + values
                MainGrid.PerformCallback(values);
            }
            if (rowVisibleIndex == 'إقفال') {
                var Path;
                var system = SysTxt.GetValue();
                //Path = '../ClaimsManage/close/CloseClmFile.aspx?ClmNo' + values;
                //popup.SetSize(400, 300);
                popup.SetHeaderText('إقفال الملف');
                popup.SetContentHtml(null);
                popup.SetContentUrl('../Claims/ClmClose/CloseClmFile.aspx?ClmNo=' + values);
                popup.Show();

                rowVisibleIndex = '';
            }
            if (rowVisibleIndex == 'تقدير احتياطي') {
                //popup.SetSize(1700, 800);
                popup.SetHeaderText('تقدير احتياطي');
                popup.SetContentHtml(null);
                popup.SetContentUrl('../Claims/Estimation/Estimate.aspx?ClmNo=' + values[0]);
                popup.Show()
                //PolicyIssu('../ReInsurance/DestributeClaim.aspx?OrderNo=' + values);
                rowVisibleIndex = '';
            }
            if (rowVisibleIndex == 'تسوية جديدة') {
                //popup.SetSize(800, 800);
                popup.SetHeaderText('تسوية جديدة');
                popup.SetContentHtml(null);
                popup.SetContentUrl('../Claims/settlement/Settle.aspx?ClmNo=' + values[0] + '&TPID=' + values[1] + '&Mode=New&Sys=' + SysTxt.GetValue());
                popup.Show()
                //PolicyIssu('../ReInsurance/DestributeClaim.aspx?OrderNo=' + values);
                rowVisibleIndex = '';
            }
            if (rowVisibleIndex == 'تحرير') {

                values = rowVisibleIndex + ',' + values
                GV2.PerformCallback(values);

            }
            if (rowVisibleIndex == 'سداد') {
                //popup.SetSize(800, 800);
                popup.SetHeaderText('سداد تسوية');
                popup.SetContentHtml(null);
                popup.SetContentUrl('../Claims/settlement/Sadad.aspx?ClmNo=' + values[0] + '&No=' + values[1]);
                popup.Show()
                rowVisibleIndex = '';
            }
            if (rowVisibleIndex == 'إلغاء التسوية') {
                pcConfirmDelete.SetHeaderText(rowVisibleIndex + '/' + values[0] + '/' + values[1]);
                pcConfirmDelete.Show();
                values = rowVisibleIndex + ',' + values
                Vals = values;
                //pcConfirmDelete.SetHeaderText();

            }
            rowVisibleIndex = '';
            values = '';
        }
        function SetSystem(sys, br) {
            SysTxt.SetValue(sys);
            Branch.SetValue(br);
            MainGrid.SetVisible(true);
            NewClaim.SetVisible(true);
            ReportsMenu.SetVisible(true);
            MainGrid.PerformCallback(sys);
        }
        function OnEndCallback(s, e) {

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'SettlePrint') {

                //popup.SetSize(1300, 950);
                let hdr = s.cpResult

                popup.SetHeaderText(s.cpResult);
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Eqrar') {

                //popup.SetSize(1300, 950);
                let hdr = s.cpResult
                //let mhdr = 'طباعة - '
                popup.SetHeaderText(s.cpResult);
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'ClmReport') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                //popup.SetSize(1300, 950);
                let hdr = s.cpResult
                //let mhdr = 'طباعة - '
                popup.SetHeaderText(s.cpResult);
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'EditSatl') {

                //popup.SetSize(1300, 950);
                let hdr = s.cpResult

                popup.SetHeaderText(s.cpResult);
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }

            //if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'DelSatl') {

            //    //popup.SetSize(1300, 950);
            //    let hdr = s.cpResult

            //    popup.SetHeaderText(s.cpResult);
            //    popup.SetContentHtml(null);
            //    popup.SetContentUrl(s.cpNewWindowUrl);
            //    popup.Show();
            //}

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                var width = Math.max(0, document.documentElement.clientWidth) * 0.8;
                var height = Math.max(0, document.documentElement.clientHeight) * 0.8;
                //popup.SetWidth(width);
                //popup.SetSize(width, height);
                let hdr = s.cpResult
                let mhdr = 'طباعة '
                popup.SetHeaderText(mhdr.concat(hdr));
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINTD') {
                var width = Math.max(0, document.documentElement.clientWidth) * 0.8;
                var height = Math.max(0, document.documentElement.clientHeight) * 0.8;
                //popup.SetWidth(width);
                //popup.SetSize(width, height);
                let hdr = s.cpResult
                let mhdr = 'طباعة '
                popup.SetHeaderText(mhdr.concat(hdr));
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINTDU') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult
                let mhdr = 'طباعة '
                popup.SetHeaderText(mhdr.concat(hdr));
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }
            s.cpMyAttribute = ''
            s.cpNewWindowUrl = null

            //UpdateTimeoutTimer();/////////////////////////////

        }
        function OnContextMenuItemClick(sender, e) {
            if (e.item.name == "CustomExportToXLS") {
                e.processOnServer = true;
                e.usePostBack = true;
            }
        }
        function ShowScanPopup(customerID, flag) {
            var viewportwidth;
            var viewportheight;
            if (typeof window.innerWidth != 'undefined') {
                viewportwidth = window.innerWidth;
                viewportheight = window.innerHeight
            }
            else if (typeof document.documentElement != 'undefined'
                && typeof document.documentElement.clientWidth != 'undefined' && document.documentElement.clientWidth != 0) {
                viewportwidth = document.documentElement.clientWidth;
                viewportheight = document.documentElement.clientHeight
            }
            popup.SetSize(viewportwidth, viewportheight);
            if (flag == 1) {
                popup.SetContentUrl('../Reporting/PreViewer.aspx?OrderNo=' + customerID);
            }
            else {
                popup.SetContentUrl('../Reporting/PreViewer.aspx?Report=/IMSReports/Chasher&OrderNo=' + customerID);
            }
            popup.Show();
        }
        function ShowReport(Report, SubIns, Branch, TDate) {
            var viewportwidth;
            var viewportheight;
            if (typeof window.innerWidth != 'undefined') {
                viewportwidth = window.innerWidth;
                viewportheight = window.innerHeight
            }
            else if (typeof document.documentElement != 'undefined'
                && typeof document.documentElement.clientWidth != 'undefined' && document.documentElement.clientWidth != 0) {
                viewportwidth = document.documentElement.clientWidth;
                viewportheight = document.documentElement.clientHeight
            }
            popup.SetSize(viewportwidth, viewportheight);
            popup.SetContentHtml(null);
            //popup.SetContentUrl('../OutPutManagement/PreView.aspx?Report=' + Report + '&Sys=' + SubIns + '&Today=' + TDate);

        }
        function ShowReports(Report, SubIns, Branch, FDate, TDate) {
            var viewportwidth;
            var viewportheight;
            if (typeof window.innerWidth != 'undefined') {
                viewportwidth = window.innerWidth;
                viewportheight = window.innerHeight
            }
            else if (typeof document.documentElement != 'undefined'
                && typeof document.documentElement.clientWidth != 'undefined' && document.documentElement.clientWidth != 0) {
                viewportwidth = document.documentElement.clientWidth;
                viewportheight = document.documentElement.clientHeight
            }
            //popup.SetSize(viewportwidth, viewportheight);
            popup.SetContentHtml(null);

            window.open(unescape(location.pathname).substring(0, unescape(location.pathname).lastIndexOf("/Claims")) + '/Reporting/PreViewer.aspx?Report=' + Report + '&Sys=' + SubIns + '&D1=' + FDate + '&D2=' + TDate);

        }
        function SelectAndClosePopup() {
            popup.Hide();
            MainGrid.Refresh();

        }
        function ShowPopup(Parameter) {

            if (Parameter == 'New') {
                //popup.SetSize(1300, 950);
                popup.SetHeaderText('فتح ملف حادث جديد');
                popup.SetContentHtml(null);
                popup.SetContentUrl('../Claims/ClmOpen/OpenClmFile.aspx?Sys=' + SysTxt.GetValue());
                popup.Show();
            }
            else {

                HeaderText = "Print"

                window.open(unescape(location.pathname).substring(0, unescape(location.pathname).lastIndexOf("/Reins")) + '/Reporting/PreViewer.aspx?Report=/IMSReports/Soa');
            }

        }
        function Yes_Click() {
            pcConfirmDelete.Hide();
            GV2.PerformCallback(Vals);
            Vals = '';
        }
        function No_Click(Vals) {
            pcConfirmDelete.Hide();
            Vals = '';
        }
        function CloseFile(Path) {
            var viewportwidth;
            var viewportheight;
            if (typeof window.innerWidth != 'undefined') {
                viewportwidth = window.innerWidth;
                viewportheight = window.innerHeight
            }
            else if (typeof document.documentElement != 'undefined'
                && typeof document.documentElement.clientWidth != 'undefined' && document.documentElement.clientWidth != 0) {
                viewportwidth = document.documentElement.clientWidth;
                viewportheight = document.documentElement.clientHeight
            }
            popup.SetSize(500, 500);
            popup.HeaderText = 'Close Claim File'
            popup.SetContentHtml(null);
            popup.SetContentUrl(Path);
            popup.Show();
        }
    </script>
    <div>
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <dx:ASPxHyperLink ID="ASPxHyperLink1" ToolTip="فتح ملف حادث" NavigateUrl="javascript:ShowPopup('New')" ImageUrl="~/images/add.png" runat="server" Text="ASPxHyperLink" ClientVisible="False" ClientInstanceName="NewClaim">
                    </dx:ASPxHyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <dx:ASPxMenu ID="ASPxMenu2" runat="server" RightToLeft="True" ShowPopOutImages="True" ClientVisible="False" ClientInstanceName="ReportsMenu"
                        AutoSeparators="RootOnly">
                        <ClientSideEvents ItemClick="function(s, e) {
                                                if (e.item.GetText()=='نعويضات تحت التسوية'){
                                                    //var d = new Date();
                                                    //var n = d.getDate();
                                                    //var mm=d.getMonth()+1;
                                                    //var yy=d.getFullYear();
                                                    //ShowReport('/TakafulyReports/Outstanding',SysTxt.GetValue(),Branch.GetValue(),yy + '/' + mm + '/' + n);
                                                    clb.PerformCallback('OutStnding');
                                                }
                                                if (e.item.GetText()=='تعويضات مسددة'){
                                                    //var d = new Date();
                                                    //var n = d.getDate();
                                                    //var mm=d.getMonth()+1;
                                                    //var yy=d.getFullYear();
                                                    //ShowReports('/TakafulyReports/PaidClaims',SysTxt.GetValue(),Branch.GetValue(),yy + '/' + mm + '/' + n,yy + '/' + mm + '/' + n);
                                                    clb.PerformCallback('Payed');
                                                }

                                            }" />
                        <Items>
                            <dx:MenuItem Text="التقارير" Image-Url="~/Images/Rep.png">
                                <Items>
                                    <dx:MenuItem Text="نعويضات تحت التسوية">
                                    </dx:MenuItem>
                                    <dx:MenuItem Text="تعويضات مسددة">
                                    </dx:MenuItem>
                                    <dx:MenuItem Text="تعويضات مبلغة">
                                    </dx:MenuItem>
                                </Items>

                                <Image Url="~/Images/Rep.png"></Image>
                            </dx:MenuItem>
                        </Items>

                        <RootItemSubMenuOffset FirstItemX="-1" LastItemX="-1" X="-1"></RootItemSubMenuOffset>
                        <ItemStyle DropDownButtonSpacing="11px" ToolbarDropDownButtonSpacing="8px" ToolbarPopOutImageSpacing="8px" />
                        <SubMenuStyle GutterWidth="0px" />
                    </dx:ASPxMenu>

                </asp:TableCell>
                <asp:TableCell>
                    <dx:ASPxTextBox ID="Branch" ClientInstanceName="Branch" Text="0" runat="server" ClientVisible="false"
                        Height="10px"
                        Width="30px" Enabled="true">
                        <Border BorderColor="White" BorderStyle="None" />
                    </dx:ASPxTextBox>
                </asp:TableCell>
                <asp:TableCell>
                    <dx:ASPxTextBox ID="Sys" ClientInstanceName="SysTxt" Text="0" runat="server" ClientVisible="false"
                        Height="10px"
                        Width="30px" Enabled="true">
                        <Border BorderColor="White" BorderStyle="None" />
                    </dx:ASPxTextBox>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <dx:ASPxTextBox ID="txtSearch" NullText="بحث" runat="server" Width="400px"></dx:ASPxTextBox>
        <dx:ASPxGridView ID="MainGrid" ClientInstanceName="MainGrid" runat="server" AutoGenerateColumns="False" 
            DataSourceID="SqlDataSource" KeyFieldName="ClmNo"
            SettingsText-GroupPanel="اسحب رأس العمود لتكوين مجموعات حسب الطلب" SettingsPager-PageSize="20" ClientVisible="False"
            SettingsText-EmptyDataRow="لايوجد مطالبات لهذا النوع من التأمين" SettingsBehavior-AllowFocusedRow="true" 
            SettingsSearchPanel-Delay="1"
            OnHtmlDataCellPrepared="MainGrid_HtmlRowPrepared" Width="98%" RightToLeft="True" 
            SettingsDetail-AllowOnlyOneMasterRowExpanded="True">
            <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" />
            <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
            <StylesEditors>
                <CalendarHeader Spacing="1px">
                </CalendarHeader>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
            <Templates>
                <DetailRow>
                    الأطراف المتضررة/
                    <dx:ASPxGridView ID="GV1" ClientInstanceName="GV1" runat="server"
                        AutoGenerateColumns="False" DataSourceID="SqlDetails"  
                        SettingsBehavior-AllowFocusedRow="true"
                        KeyFieldName="TPID" OnHtmlDataCellPrepared="GV1_HtmlRowPrepared" 
                        SettingsText-EmptyDataRow="لم يتم إضافة متضررين للحادث"
                        OnBeforePerformDataSelect="GV1_BeforePerformDataSelect" 
                        OnDataBound="GV1_DataBound" Width="100%" RightToLeft="True">
                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
                        <Templates>
                            <DetailRow>
                                التسويات /
                                <dx:ASPxGridView ID="GV2" ClientInstanceName="GV2" runat="server" AutoGenerateColumns="False" 
                                    DataSourceID="SqlDataMainSettlement" 
                                    EnableRowsCache="false" SettingsBehavior-AllowFocusedRow="true"
                                    SettingsDetail-AllowOnlyOneMasterRowExpanded="True" OnCustomCallback="GV2_CustomCallback"
                                    OnContextMenuItemClick="GV2_ContextMenuItemClick"
                                    KeyFieldName="No" OnHtmlDataCellPrepared="GV2_HtmlRowPrepared" 
                                    SettingsText-EmptyDataRow="لا يوجد تسويات لهذا المتضرر"
                                    OnBeforePerformDataSelect="GV2_BeforePerformDataSelect" Width="100%" RightToLeft="True">
                                    <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                    <ClientSideEvents EndCallback="OnEndCallback"
                                        RowClick="function(s, e) {
                                                    //debugger;
                                                    if (rowVisibleIndex=='طباعة'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;No;TPID', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='تحرير'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;No;TPID', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='سداد'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;No;TPID', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='إقرار سداد ومخالصة'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;No;TPID', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='إقرار سداد ومخالصة'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;No;TPID', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='إلغاء التسوية'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;No;TPID;DAILYNUM', GetPickUpPoints);
                                                    }

                                                }" />
                                    <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" />
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="" ToolTip="menu" VisibleIndex="6" CellStyle-HorizontalAlign="Left">
                                            <DataItemTemplate>
                                                <dx:ASPxMenu ID="SetlmMenu" ClientInstanceName="SetlmMenu" runat="server" ShowPopOutImages="true" AutoSeparators="RootOnly" 
                                                    BorderBetweenItemAndSubMenu="HideRootOnly" GutterWidth="0px" HorizontalAlign="Right" VerticalAlign="Bottom" RightToLeft="True"
                                                    SubMenuStyle-Wrap="True" ItemStyle-HorizontalAlign="Right">
                                                    <ClientSideEvents ItemMouseOver="function(s, e) {
                                                                //debugger;
                                                                rowVisibleIndex=e.item.GetText();
                                                                }" />
                                                    <Items>
                                                        <dx:MenuItem Text="إجراءات التسوية">
                                                        </dx:MenuItem>
                                                    </Items>
                                                    <LoadingPanelImage Url="~/App_Themes/SoftOrange/Web/Loading.gif">
                                                    </LoadingPanelImage>
                                                    <ItemStyle DropDownButtonSpacing="8px" ToolbarDropDownButtonSpacing="5px" ToolbarPopOutImageSpacing="5px" PopOutImageSpacing="10px"></ItemStyle>
                                                    <SubMenuStyle GutterWidth="0px"></SubMenuStyle>
                                                </dx:ASPxMenu>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataTextColumn FieldName="ClmNo" ReadOnly="True" VisibleIndex="1" Visible="false">
                                            <EditFormSettings Visible="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="No" VisibleIndex="2" Caption="رقم التسوية">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn FieldName="SettelementDesc" VisibleIndex="3" Caption="وصف التسوية">
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataDateColumn FieldName="Date" VisibleIndex="4" Caption="تاريخ ادخال التسوية الصرف" PropertiesDateEdit-DisplayFormatString="yyyy/MM/dd">
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn FieldName="Net" VisibleIndex="5" Caption="إجمالي التسوية">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="DAILYNUM" VisibleIndex="6" Caption="إذن الصرف">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn FieldName="DAILYDTE" VisibleIndex="7" Caption="تاريخ إذن الصرف" PropertiesDateEdit-DisplayFormatString="yyyy/MM/dd">
                                        </dx:GridViewDataDateColumn>
                                    </Columns>
                                    <SettingsPager>
                                        <AllButton Text="All">
                                        </AllButton>
                                        <NextPageButton Text="Next &gt;">
                                        </NextPageButton>
                                        <PrevPageButton Text="&lt; Prev">
                                        </PrevPageButton>
                                    </SettingsPager>
                                    <SettingsLoadingPanel ImagePosition="Top"></SettingsLoadingPanel>
                                    <Paddings Padding="1px"></Paddings>
                                    <Images SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css">
                                        <LoadingPanelOnStatusBar Url="~/App_Themes/SoftOrange/GridView/gvLoadingOnStatusBar.gif">
                                        </LoadingPanelOnStatusBar>
                                        <LoadingPanel Url="~/App_Themes/SoftOrange/GridView/Loading.gif">
                                        </LoadingPanel>
                                    </Images>
                                    <ImagesFilterControl>
                                        <LoadingPanel Url="~/App_Themes/SoftOrange/Editors/Loading.gif">
                                        </LoadingPanel>
                                    </ImagesFilterControl>
                                    <Styles GroupButtonWidth="28">
                                        <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="8px">
                                        </LoadingPanel>
                                    </Styles>
                                    <StylesEditors>
                                        <CalendarHeader Spacing="1px">
                                        </CalendarHeader>
                                        <ProgressBar Height="29px">
                                        </ProgressBar>
                                    </StylesEditors>
                                    <SettingsDetail IsDetailGrid="True" />
                                </dx:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" />
                        <ClientSideEvents RowClick="function(s, e) {
                                                    //debugger;
                                                    if (rowVisibleIndex=='تقدير احتياطي'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;TPID', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='تسوية جديدة'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;TPID', GetPickUpPoints);
                                                    }

                                                }" />
                        <Columns>
                            <dx:GridViewCommandColumn VisibleIndex="0" Visible="False">
                                <%--<ClearFilterButton Visible="True"></ClearFilterButton>--%>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataColumn FieldName="TPID" VisibleIndex="0" Visible="false" />
                            <dx:GridViewDataColumn FieldName="ThirdParty" VisibleIndex="1" Caption="المتضرر" />
                            <dx:GridViewDataColumn FieldName="TPType" VisibleIndex="2" Caption="نوعه" />
                            <dx:GridViewDataColumn FieldName="Asset" VisibleIndex="3" Caption="البيان المتضرر" />
                            <dx:GridViewDataColumn FieldName="Damage" VisibleIndex="4" Caption="بيان الأضرار" />
                            <dx:GridViewDataColumn FieldName="ClmNo" VisibleIndex="5" Visible="false" />
                            <dx:GridViewDataColumn FieldName="" ToolTip="menu" VisibleIndex="5" CellStyle-HorizontalAlign="Left">
                                <DataItemTemplate>
                                    <dx:ASPxMenu ID="ClmMenu" ClientInstanceName="ASPxMenu" runat="server" AutoSeparators="RootOnly" >
                                        <ClientSideEvents ItemMouseOver="function(s, e) {
                                                                //debugger;
                                                                rowVisibleIndex=e.item.GetText();
                                                }" />
                                        <Items>
                                            <dx:MenuItem Text="إجراءات المتضرر">
                                            </dx:MenuItem>
                                        </Items>
                                       <%-- <LoadingPanelImage Url="~/App_Themes/PlasticBlue/Web/Loading.gif">
                                        </LoadingPanelImage>
                                        <ItemSubMenuOffset FirstItemY="-1" LastItemY="-1" Y="-1"></ItemSubMenuOffset>
                                        <RootItemSubMenuOffset FirstItemX="1" LastItemX="1" X="1"></RootItemSubMenuOffset>
                                        <ItemStyle DropDownButtonSpacing="13px" ToolbarDropDownButtonSpacing="5px"
                                            ToolbarPopOutImageSpacing="5px"></ItemStyle>--%>
                                        <SubMenuStyle GutterWidth="0px"></SubMenuStyle>
                                    </dx:ASPxMenu>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                        </Columns>
                        <SettingsPager ShowDefaultImages="False">
                            <AllButton Text="All">
                            </AllButton>
                            <NextPageButton Text="Next &gt;">
                            </NextPageButton>
                            <PrevPageButton Text="&lt; Prev">
                            </PrevPageButton>
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" Delay="2500"></SettingsSearchPanel>
                        <SettingsDetail IsDetailGrid="True" ShowDetailRow="True" />
            
     
                    </dx:ASPxGridView>
                </DetailRow>
            </Templates>
            <ClientSideEvents EndCallback="OnEndCallback" RowClick="function(s, e) {
                                                   //debugger;
                                                    if (rowVisibleIndex=='معاينة'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;Sys', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='تقرير بيانات الحادث'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo;Sys', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='توزيع'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmPath', GetPickUpPoints);
                                                    }
                                                    if (rowVisibleIndex=='إقفال'){
                                                        s.GetRowValues(e.visibleIndex, 'ClmNo', GetPickUpPoints);
                                                    }
                                                 }" />
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" Visible="False">
                    <%--<ClearFilterButton Visible="True"></ClearFilterButton>--%>
                </dx:GridViewCommandColumn>
                <dx:GridViewDataColumn FieldName="ClmPath" VisibleIndex="1" Visible="false" />
                <dx:GridViewDataColumn FieldName="ClmNo" VisibleIndex="2" Caption="رقم الحادث" />
                <dx:GridViewDataColumn FieldName="PolNo" VisibleIndex="3" Caption="رقم الوثيقة" />
                <dx:GridViewDataColumn FieldName="CustName" VisibleIndex="4" Caption="اسم المؤمن له">
                    <Settings AutoFilterCondition="Contains"></Settings>
                </dx:GridViewDataColumn>
                <%--                <dx:GridViewDataColumn FieldName="ClmDate" VisibleIndex="5" Caption="تاريخ الحادث" />--%>
                <dx:GridViewDataTextColumn FieldName="ClmDate" VisibleIndex="5" Caption="تاريخ الحادث" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                    </PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ClmSysDate" VisibleIndex="5" Caption="تاريخ فتح الحادث" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                    <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                    </PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <%-- <dx:GridViewDataColumn FieldName="Status" VisibleIndex="5"  />--%>
                <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="6" Caption="الحالة " Width="40px">
                    <DataItemTemplate>
                        <dx:ASPxImage runat="server" ID="ClaimStatus" OnInit="ClaimStatus_Init">
                        </dx:ASPxImage>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataColumn FieldName="Sys" VisibleIndex="7" Visible="false" />
                <dx:GridViewDataColumn FieldName="" ToolTip="menu" VisibleIndex="8" CellStyle-HorizontalAlign="Left">
                    <DataItemTemplate>
                        <dx:ASPxMenu ID="MClmMenu" ClientInstanceName="MASPxMenu" runat="server" ShowPopOutImages="true"
                            AutoSeparators="RootOnly">
                            <ClientSideEvents ItemMouseOver="function(s, e) {
                                                                 //debugger;
                                                                rowVisibleIndex=e.item.GetText();
                                                                    }" />
                            <Items>
                                <dx:MenuItem Text="إجراءات الحادث">
                                </dx:MenuItem>
                            </Items>
                            <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                            </LoadingPanelImage>
                            <RootItemSubMenuOffset FirstItemX="-1" LastItemX="-1" X="-1"></RootItemSubMenuOffset>
                            <ItemStyle DropDownButtonSpacing="11px" ToolbarDropDownButtonSpacing="8px" ToolbarPopOutImageSpacing="8px"></ItemStyle>
                            <SubMenuStyle GutterWidth="0px"></SubMenuStyle>
                        </dx:ASPxMenu>
                    </DataItemTemplate>
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dx:GridViewDataColumn>
            </Columns>
            <SettingsDetail ShowDetailRow="True" />
            <Settings ShowGroupPanel="True"></Settings>
            <SettingsSearchPanel Visible="True" CustomEditorID="txtSearch"></SettingsSearchPanel>
        </dx:ASPxGridView>
        <br />
        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="clb" OnCallback="ASPxCallback1_Callback">
            <ClientSideEvents CallbackComplete="OnEndCallback" />
        </dx:ASPxCallback>
        <asp:SqlDataSource ID="SqlDataSource" runat="server"
            ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
            SelectCommand="select distinct rtrim(MainClaimFile.ClmNo) As ClmNo, mainclaimfile.PolNo,mainclaimfile.ClmDate,mainclaimfile.ClmSysDate, mainclaimfile.Status,CustomerFile.CustName,isnull(mainclaimfile.GroupNo,0) As GroupNo,
  RTRIM(MainClaimFile.ClmNo) + '&EndNo=' + LTRIM(MainClaimFile.EndNo) + '&LoadNo=' + LTRIM(MainClaimFile.LoadNo) + '&OrderNo=' + rtrim(PolicyFile.OrderNo) + '&Sys=' + PolicyFile.SubIns + '&Branch=' + PolicyFile.Branch AS Clm,
  RTRIM(MainClaimFile.ClmNo) + '&EndNo=' + LTRIM(MainClaimFile.EndNo) + '&LoadNo=' + LTRIM(MainClaimFile.LoadNo) + '&OrderNo=' + rtrim(PolicyFile.OrderNo) + '&Sys=' + PolicyFile.SubIns + '&Branch=' + PolicyFile.Branch + '&PolNo=' + PolicyFile.PolNo + '&GroupNo=' + LTRIM(mainclaimfile.GroupNo)  AS ClmPath,PolicyFile.SubIns As Sys
  from MainClaimFile left outer join Estimation on MainClaimFile.ClmNo=Estimation.clmno and MainClaimFile.PolNo=Estimation.polno
  left outer join NetPrm on NetPrm.PolNo = rtrim(MainClaimFile.PolNo) + '-' + rtrim(MainClaimFile.ClmNo) and round(Estimation.value,3)=round(netprm.Net,3) left outer join policyfile
  on MainClaimFile.PolNo=PolicyFile.PolNo and MainClaimFile.EndNo=PolicyFile.EndNo and MainClaimFile.LoadNo=PolicyFile.LoadNo
  left outer join CustomerFile on CustomerFile.CustNo=policyfile.CustNo left outer join BranchInfo on mainclaimfile.Branch=BranchInfo.BranchNo
  WHERE MainClaimFile.SubIns=@Sys and mainclaimfile.Branch=@Branch order by ClmSysDate desc,ClmNo Desc">
            <SelectParameters>
                <asp:ControlParameter ControlID="Sys" Name="Sys" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="Branch" Name="Branch" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource runat="server" ID="SqlDetails" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
            SelectCommand="SELECT TPID, rtrim(ThirdParty.ClmNo) As ClmNo,ThirdParty.ThirdParty, Asset, Damage, Value, ThirdParty.UserName,dbo.GetExtraCatName('ThirdParty',ThirdParty.TPID) As TPType
FROM  ThirdParty INNER JOIN MainClaimfile ON MainClaimfile.ClmNo = ThirdParty.ClmNo
where ThirdParty.ClmNo=@ClmNo
GROUP BY TPID, ThirdParty.ClmNo, ThirdParty, Asset, Damage, Value,ThirdParty.UserName">
            <SelectParameters>
                <asp:SessionParameter Name="ClmNo" SessionField="ClmNo" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource runat="server" ID="SqlDataMainSettlement" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
            SelectCommand="SELECT  MainSattelement.ClmNo, MainSattelement.TPID, MainSattelement.No, MainSattelement.PayTo, MainSattelement.SettelementDesc, MainSattelement.Date, MainSattelement.DAILYDTE,
isnull(SUM(DetailSettelement.Value),0) AS Net,
                            rtrim(MainSattelement.DAILYNUM) as DAILYNUM
FROM  MainSattelement left outer JOIN
                            DetailSettelement ON DetailSettelement.ClmNo = MainSattelement.ClmNo AND DetailSettelement.TPID = MainSattelement.TPID AND DetailSettelement.No = MainSattelement.No
                            WHERE (MainSattelement.ClmNo = @ClmNo) AND (MainSattelement.TPID = @TPID)
GROUP BY MainSattelement.ClmNo, MainSattelement.TPID, MainSattelement.No, MainSattelement.PayTo, MainSattelement.SettelementDesc, MainSattelement.Date, MainSattelement.DAILYNUM, MainSattelement.DAILYDTE">
            <SelectParameters>
                <asp:SessionParameter Name="TPID" SessionField="TPID" Type="Int32" />
                <asp:SessionParameter Name="ClmNo" SessionField="ClmNo" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <dx:ASPxPopupControl ID="pcConfirmDelete" runat="server" ClientInstanceName="pcConfirmDelete" HeaderText="إلغاء التسوية" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="false">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server" Font-Bold="true">
                    <table dir="rtl">
                        <tr>
                            <td colspan="3" style="vertical-align: central;">تأكيد إلغاء التسوية؟ </td>
                        </tr>
                        <tr>
                            <td><%--<a href="javascript:Yes_Click()">نعم</a>--%>
                                <dx:ASPxButton ID="yesButton" runat="server" AutoPostBack="false" Text="نعم">
                                    <ClientSideEvents Click="Yes_Click" />
                                </dx:ASPxButton>
                            </td>
                            <td><%-- <a href="javascript:No_Click()">لا</a>--%>
                                <dx:ASPxButton ID="noButton" runat="server" AutoPostBack="false" Text="لا">
                                    <ClientSideEvents Click="No_Click" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <br />
        <dx:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server" AllowDragging="True" AllowResize="True"
            CloseAction="CloseButton"
            EnableViewState="False" PopupElementID="Image1" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowFooter="True" Width="576px" Modal="True"
            Height="196px" FooterText=""
            EnableHierarchyRecreation="True"
            CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css" ResizingMode="Postponed" ScrollBars="Both" HeaderText="">
            <LoadingPanelImage Url="~/App_Themes/PlasticBlue/Web/dvLoading.gif" ToolTip="loading...">
            </LoadingPanelImage>
            <ClientSideEvents CloseUp="function(s, e) {MainGrid.PerformCallback();}"
                CloseButtonClick="function(s, e) {MainGrid.PerformCallback();}" Init="puOnInit" />
            <CloseButtonStyle>
                <Paddings Padding="0px" />
            </CloseButtonStyle>
            <ContentStyle>
                <BorderBottom BorderColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px" />
            </ContentStyle>
            <HeaderStyle>
                <Paddings PaddingBottom="4px" PaddingTop="4px" PaddingLeft="10px" PaddingRight="4px" />
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
</asp:Content>