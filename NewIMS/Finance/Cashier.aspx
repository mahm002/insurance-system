<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="Cashier.aspx.vb" Inherits="Cashier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function OnEndCallback(s, e) {
            //debugger;
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult
                let mhdr = 'طباعة - '
                popup.SetHeaderText(mhdr.concat(hdr));
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINTD') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult
                let mhdr = 'طباعة - '
                popup.SetHeaderText(mhdr.concat(hdr));
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINTU') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult
                let mhdr = 'طباعة - '
                popup.SetHeaderText(mhdr.concat(hdr));
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }
            s.cpMyAttribute = ''
            s.cpNewWindowUrl = null

            //UpdateTimeoutTimer();/////////////////////////////

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
            popup.SetWidth(width);
            popup.SetSize(width, height);
        }
        function SelectAndClosePopup() {
            popup.Hide();
            detailGrid.PerformCallback();
            AccMoveGrid.PerformCallback();
        }

        function ShowPayPopup(customerID) {
            //alert(customerID);
            //switch customerID {
            //    case 'أخـرى':

            //        let hdr = customerID
            //        let mhdr = 'مقبوضات أخرى - '
            //        popup.SetHeaderText(mhdr.concat(hdr));
            //        popup.SetContentUrl('Receipt.aspx?PolNo=' + customerID + '&BranchName=' + SelectedSum.GetValue());
            //    case BulkPayment:

            //        //alert(customerID);
            //    default:

            //        let hdr = 'وثيقة '
            //        let mhdr = 'سداد - '
            //        popup.SetHeaderText(mhdr.concat(hdr));
            //        popup.SetContentUrl('Receipt.aspx?PolNo=' + customerID);

            //}
            if (customerID == 'أخـرى') {
                let hdr = customerID
                let mhdr = 'مقبوضات أخرى - '
                popup.SetHeaderText(mhdr.concat(hdr));
                //popup.SetContentUrl('Receipt.aspx?PolNo=' + customerID + '&BranchName=' + SelectedSum.GetValue());
                popup.SetContentUrl('MultiPaymentReceipt.aspx?PolNo=' + customerID + '&BranchName=' + SelectedSum.GetValue());
            }
            else if (customerID == 'BulkPayment') {
                let hdr = customerID
                let mhdr = 'تسوية مجموعة وثائق - '
                popup.SetHeaderText(mhdr.concat(hdr));
                popup.SetContentUrl('BulkReceipt.aspx');
            }
            else if (customerID == 'Credit') {
                let hdr = customerID
                let mhdr = 'تسوية وثائق (علي الحساب أو تحت التحصيل) - '
                popup.SetHeaderText(mhdr.concat(hdr));
                popup.SetContentUrl('Payments.aspx');
            }
            else {
                let hdr = 'وثيقة '
                let mhdr = 'سداد - '
                popup.SetHeaderText(mhdr.concat(hdr));
                //popup.SetContentUrl('Receipt.aspx?PolNo=' + customerID);
                popup.SetContentUrl('MultiPaymentReceipt.aspx?PolNo=' + customerID);
            }
            popup.Show();
        }
        function ShowChasherList(customerID) {

            clb.PerformCallback('PrintCashierD');

        }
        function ShowChasherInOrOut(customerID) {

            clb.PerformCallback('ShowChasherInOrOut');

        }
        function ShowUnpaid(customerID) {
            clb.PerformCallback('PrintUnPaidD');
        }

        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }

        function grid_SelectionChanged(s, e) {
            s.GetSelectedFieldValues("TOTPRM", GetSelectedFieldValuesCallback);
            s.GetSelectedFieldValues("Pol", GetSelectedFieldValuesCallback1);
            //clb.PerformCallback(selList.items());
        }

        function GetSelectedFieldValuesCallback(values) {
            try {
                SelectedSum.SetValue(0);
                for (var i = 0; i < values.length; i++) {
                    SelectedSum.SetValue(parseFloat(SelectedSum.GetValue()) + values[i]);
                }
            } finally {

            }
            //document.getElementById("selCount").innerHTML = detailGrid.GetSelectedRowCount();
            /*alert(detailGrid.GetSelectedRowCount());*/
        }
        function GetSelectedFieldValuesCallback1(values) {
            if (values.length == 0) {

            }
            else {
                alert(values.length);
            }

            try {
                selList.ClearItems();
                for (var i = 0; i < values.length; i++) {
                    selList.AddItem(values[i]);
                    //alert(values[i]);
                }
            } finally {
                selList.EndUpdate();
            }
            //document.getElementById("selCount").innerHTML = detailGrid.GetSelectedRowCount();
        }
    </script>
    <div>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" EnableViewState="False" ShowHeader="False">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <table>
                        <tr>
                            <td>الخزينة
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server"
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="detailDataSource" runat="server"
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="SELECT PolicyFile.PolNo, PolicyFile.TOTPRM,PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain,ISNULL(PolicyFile.Inbox, 0) AS InBox, EXTRAINFO.TPName, CustomerFile.CustName, PolicyFile.IssuDate, PolicyFile.NETPRM, PolicyFile.Branch, ISNULL(PolicyFile.Stop, 0) AS Stop, CASE WHEN SubSysName IS NULL THEN BranchName +'/'+ 'أخرى' ELSE BranchName +'/'+ SubSysName END AS BR,CASE WHEN Commision<>0 and Broker<>0 then CAST(Commision AS NVARCHAR(100)) + iif(CommisionType=1,' '+EXTRAINFO.TPName,' %') +'-' + BrokersInfo.TPName else '0' END As Commissioned, CASE WHEN PolicyFile.PayType=1 THEN '0' ELSE PolicyFile.AccountNo END AS AccountN,AccountFile.AccountName, PolicyFile.EndNo,PolicyFile.LoadNo, RTRIM(PolicyFile.PolNo) + '&EndNo=' + RTRIM(PolicyFile.EndNo) + '&LoadNo=' + RTRIM(PolicyFile.LoadNo) + '&Sys=' + RTRIM(PolicyFile.SubIns) + '&AccNo=' + CASE WHEN PolicyFile.PayType=1 THEN '0' ELSE PolicyFile.AccountNo END  AS Pol FROM PolicyFile LEFT OUTER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER JOIN BranchInfo ON PolicyFile.Branch = BranchInfo.BranchNo LEFT OUTER JOIN SUBSYSTEMS ON PolicyFile.SubIns = SUBSYSTEMS.SUBSYSNO AND SUBSYSTEMS.Branch = PolicyFile.Branch INNER JOIN EXTRAINFO ON EXTRAINFO.TP = 'Cur' AND EXTRAINFO.TPNo = PolicyFile.Currency LEFT OUTER JOIN EXTRAINFO As BrokersInfo on BrokersInfo.TP='Broker' and PolicyFile.Broker=BrokersInfo.TPNo left outer join AccountFile on PolicyFile.IssueUser=AccountFile.AccountNo WHERE Inbox<>TOTPRM AND month(IssuDate) = month(@IssuDate) AND year(IssuDate) = year(@IssuDate) AND PolicyFile.Branch = @Br AND PayType=@Ptyp and  PolicyFile.Stop=0">
            <SelectParameters>
                <asp:ControlParameter ControlID="dateEdit" PropertyName="Value" Name="IssuDate" Type="DateTime"></asp:ControlParameter>
                <asp:SessionParameter SessionField="Branch" Name="Br" Type="String"></asp:SessionParameter>
                <asp:ControlParameter ControlID="PayType" PropertyName="Value" Name="Ptyp" Type="Int16"></asp:ControlParameter>
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="CurrentPays" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="SELECT cast(ltrim(ACCMOVE.DocNo) as nchar) AS Poll, ACCMOVE.DocDat As IssuDate, ACCMOVE.CustNAme,iif(ACCMOVE.PayMent=ACCMOVE.Amount, ACCMOVE.PayMent,ACCMOVE.Amount) AS TOTPRM,
                            CASE WHEN ACCMOVE.ForW='أخـرى' then
                            ACCMOVE.Note
                            else ' وثيقة رقم ' + right(rtrim(ACCMOVE.ForW),100) end AS PolNo,
                            ACCMOVE.EndNo, ACCMOVE.LoadNo, ACCMOVE.Tp, ACCMOVE.Cur AS TPName,
                    dbo.GetExtraCatName('Payment',PayTp) As PayTp, ACCMOVE.Branch,
					CASE WHEN SubSysName IS NULL
					THEN BranchName +'/'+'أخرى' ELSE
					BranchName +'/'+ SubSysName END AS BR,0 as Stop
                    FROM  ACCMOVE LEFT OUTER JOIN
                         PolicyFile ON ACCMOVE.ForW = PolicyFile.PolNo AND ACCMOVE.EndNo = PolicyFile.EndNo AND ACCMOVE.LoadNo = PolicyFile.LoadNo LEFT OUTER JOIN
                         CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER JOIN
                         BranchInfo ON ACCMOVE.Branch = BranchInfo.BranchName LEFT OUTER JOIN
                         SUBSYSTEMS ON PolicyFile.SubIns = SUBSYSTEMS.SUBSYSNO AND SUBSYSTEMS.Branch = PolicyFile.Branch INNER JOIN
                         EXTRAINFO ON EXTRAINFO.TP = 'Cur' AND EXTRAINFO.TPName = ACCMOVE.Cur
                         WHERE month(DocDat) = month(@IssuDate) and year(DocDat) = year(@IssuDate) AND ACCMOVE.Branch = @Br Order By DocDat desc">
            <SelectParameters>
                <asp:ControlParameter ControlID="dateEdit" PropertyName="Value" Name="IssuDate" Type="DateTime"></asp:ControlParameter>
                <asp:SessionParameter SessionField="Branch" DefaultValue="0" Name="Br" Type="String"></asp:SessionParameter>
            </SelectParameters>
        </asp:SqlDataSource>
        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="clb" OnCallback="ASPxCallback1_Callback">
            <ClientSideEvents CallbackComplete="OnEndCallback" />
        </dx:ASPxCallback>
        <div class="style5">
            <table dir="rtl" style="width: 100%;">
                <tr>
                    <td style="text-align: left">التاريخ</td>
                    <td>
                        <dx:ASPxDateEdit ID="dateEdit" runat="server" PickerType="Months"
                            OnDateChanged="casherDay" EditFormat="Custom" Width="100%" AllowMouseWheel="true"
                            DisplayFormatString="yyyy/MM" EditFormatString="yyyy/MM" RightToLeft="True">
                            <CalendarProperties FirstDayOfWeek="Sunday">
                            </CalendarProperties>
                            <ClientSideEvents DateChanged="function(s, e) {
                                        detailGrid.PerformCallback();
                                        AccMoveGrid.PerformCallback();}" />
                        </dx:ASPxDateEdit>
                    </td>
                    <td>

                        <dx:ASPxComboBox ID="PayType" runat="server" SelectedIndex="0" ClientVisible="false" IncrementalFilteringMode="StartsWith" Width="150px" Height="18px" RightToLeft="True">
                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                        detailGrid.PerformCallback();
                                        AccMoveGrid.PerformCallback();}" />
                            <Items>
                                <dx:ListEditItem Selected="True" Text="نقداً" Value="1"></dx:ListEditItem>
                                <dx:ListEditItem Text="على الحساب" Value="2"></dx:ListEditItem>
                            </Items>
                        </dx:ASPxComboBox>
                    </td>
                    <td>

                        <%-- <dx:ASPxHyperLink ID="ASPxHyperLink3"  NavigateUrl="javascript:ShowUnpaid('/IMSReports/Unpaid')" ImageUrl="~/images/Last.png" runat="server"
                            Text="ASPxHyperLink" AllowEllipsisInText="True">
                        </dx:ASPxHyperLink>
                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="مقبوضات أخرى" RenderMode="Secondary" AutoPostBack="False" Image-Url="~/images/copy.gif" Width="100%">
                            <ClientSideEvents Click="function(s, e) {ShowPayPopup('أخـرى');}" />
                        </dx:ASPxButton>--%>

                        <dx:ASPxMenu ID="Payments" runat="server" RightToLeft="True" ShowPopOutImages="True" ClientInstanceName="Payments"
                            Width="100%">
                            <ClientSideEvents ItemClick="function(s, e) {
                                                if (e.item.GetText()=='تسوية مجموعة وثائق'){
                                                   ShowPayPopup('BulkPayment');
                                                }
                                                if (e.item.GetText()=='مقبوضات أخرى'){
                                                    ShowPayPopup('أخـرى');
                                                }
                                                if (e.item.GetText()=='تسوية وثائق (علي الحساب أو تحت التحصيل)'){
                                                    ShowPayPopup('Credit');
                                                }

                                            }" />
                            <Items>
                                <dx:MenuItem Text="مقيوضات" Image-Url="~/Images/Rep.png">
                                    <Items>
                                        <%--                                        <dx:MenuItem Text="مقبوضات على الحساب">
                                        </dx:MenuItem>--%>
                                        <dx:MenuItem Text="مقبوضات أخرى">
                                        </dx:MenuItem>
                                        <dx:MenuItem Text=" تسوية مجموعة وثائق ">
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="تسوية وثائق (علي الحساب أو تحت التحصيل)">
                                        </dx:MenuItem>
                                    </Items>
                                    <Image Url="~/Images/Rep.png"></Image>
                                </dx:MenuItem>
                            </Items>

                            <RootItemSubMenuOffset FirstItemX="-1" LastItemX="-1" X="-1"></RootItemSubMenuOffset>
                            <ItemStyle DropDownButtonSpacing="11px" ToolbarDropDownButtonSpacing="8px" ToolbarPopOutImageSpacing="8px" />
                            <SubMenuStyle GutterWidth="0px" />
                        </dx:ASPxMenu>
                    </td>
                    <td>

                        <%-- <dx:ASPxHyperLink ID="ASPxHyperLink1"  NavigateUrl="javascript:ShowPayPopup('أخـرى');" ImageUrl="~/images/copy.gif" runat="server" Text="ASPxHyperLink" AllowEllipsisInText="True">
                        </dx:ASPxHyperLink>--%>
                        <dx:ASPxButton ID="MyButton" runat="server" Text="كشف حركة المقيوضات" RenderMode="Secondary"
                            AutoPostBack="False" Image-Url="~/images/IsseClip.gif" Width="100%">
                            <ClientSideEvents Click="ShowChasherList" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text=" كشف حركة الخزينة (الصادر والوارد) " RenderMode="Secondary"
                            AutoPostBack="False" Image-Url="~/images/IsseClip.gif" Width="100%">
                            <ClientSideEvents Click="ShowChasherInOrOut" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="كشف أقساط تحت التحصيل" RenderMode="Secondary" AutoPostBack="False" Image-Url="~/images/Last.png" Width="100%">
                            <ClientSideEvents Click="ShowUnpaid" />
                        </dx:ASPxButton>
                    </td>
                    <td style="text-align: left">
                        <dx:ASPxTextBox ID="Search" runat="server" Width="170px" OnTextChanged="Search_TextChanged" AutoPostBack="true" ClientVisible="false">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="ASPxTextBox1" ClientInstanceName="SelectedSum" runat="server" Width="16px" ClientVisible="FALSE">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="100%" EnableViewState="False" ShowHeader="true"
                AllowCollapsingByHeaderClick="true" HeaderText="وثائق معلقة أو غير مسددة (تحت التحصيل )" HeaderStyle-HorizontalAlign="Right" ShowCollapseButton="True" Theme="BlackGlass">
                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td style="direction: rtl">
                                    <dx:ASPxGridView ID="detailGrid" runat="server" AccessibilityCompliant="True" AutoGenerateColumns="False"
                                        ClientInstanceName="detailGrid" DataSourceID="detailDataSource" KeyFieldName="PolNo"
                                        OnHeaderFilterFillItems="Filter" OnHtmlDataCellPrepared="detailGrid_HtmlDataCellPrepared"
                                        OnHtmlRowPrepared="grid_HtmlRowPrepared" RightToLeft="True" Width="100%">
                                        <ClientSideEvents SelectionChanged="grid_SelectionChanged" />
                                        <SettingsPager PageSize="20000">
                                        </SettingsPager>
                                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                        <Settings ShowGroupPanel="True" />
                                        <SettingsBehavior AllowFocusedRow="True" />
                                        <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" />
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False">
                                            </FilterControl>
                                        </SettingsPopup>
                                        <SettingsSearchPanel Delay="1" Visible="True" />
                                        <SettingsLoadingPanel Delay="1" ImagePosition="Top" Mode="Disabled" Text="تحميل..." />
                                        <SettingsText EmptyDataRow="لا يوجد بيانات" />
                                        <Columns>
                                            <dx:GridViewDataHyperLinkColumn Caption="تسوية" FieldName="Pol" ReadOnly="True" ShowInCustomizationForm="True" ToolTip="سداد" VisibleIndex="1" Width="40px">
                                                <PropertiesHyperLinkEdit ImageUrl="~/Content/Images/pay_16px.png" NavigateUrlFormatString="javascript:ShowPayPopup('{0}',1);" Text="سداد">
                                                </PropertiesHyperLinkEdit>
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataHyperLinkColumn>
                                            <dx:GridViewDataColumn Caption="رقم الوثيقة" FieldName="PolNo" ShowInCustomizationForm="True" VisibleIndex="2">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="رقم الملحق" FieldName="EndNo" ShowInCustomizationForm="True" VisibleIndex="3">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="رقم الإشعار" FieldName="LoadNo" ShowInCustomizationForm="True" VisibleIndex="4">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="رقم العقد" FieldName="Pol" ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="المؤمن له" FieldName="CustName" ShowInCustomizationForm="True" VisibleIndex="6">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle Wrap="true">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="الفرع" FieldName="BR" ShowInCustomizationForm="True" VisibleIndex="7">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataTextColumn Caption="إجمالي القسط" FieldName="TOTPRM" ShowInCustomizationForm="True" VisibleIndex="8">
                                                <PropertiesTextEdit DisplayFormatString="n3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="المسدد" FieldName="InBox" ShowInCustomizationForm="True" VisibleIndex="9">
                                                <PropertiesTextEdit DisplayFormatString="n3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="المستحق" FieldName="Remain" ShowInCustomizationForm="True" VisibleIndex="10">
                                                <PropertiesTextEdit DisplayFormatString="n3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataDateColumn Caption="تاريخ الإصدار" FieldName="IssuDate" ShowInCustomizationForm="True" VisibleIndex="11">
                                                <PropertiesDateEdit DisplayFormatInEditMode="True" DisplayFormatString="yyyy/MM/dd" EditFormatString="yyyy/MM/dd">
                                                </PropertiesDateEdit>
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataColumn Caption="العملة" FieldName="TPName" ShowInCustomizationForm="True" VisibleIndex="12">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption=" العمولة - المسوق " FieldName="Commissioned" ShowInCustomizationForm="True" VisibleIndex="13">
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption=" صدرت بواسطة " FieldName="AccountName" ShowInCustomizationForm="True" VisibleIndex="14">
                                                <CellStyle Wrap="True">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                        </Columns>
                                        <GroupSummary>
                                            <dx:ASPxSummaryItem FieldName="TOTPRM" SummaryType="Sum" ValueDisplayFormat="n3" />
                                            <dx:ASPxSummaryItem FieldName="PolNo" SummaryType="Count" ValueDisplayFormat="n0" />
                                        </GroupSummary>
                                        <Styles GroupButtonWidth="28">
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                            </Header>
                                            <LoadingPanel ImageSpacing="8px">
                                            </LoadingPanel>
                                        </Styles>
                                        <Paddings Padding="1px" />
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <br />
            <dx:ASPxListBox ID="ASPxListBox1" ClientInstanceName="selList" runat="server" ClientVisible="false" Width="100%" />
            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" EnableViewState="False" ShowHeader="true"
                AllowCollapsingByHeaderClick="true" HeaderText="حركة المقبوضات" HeaderStyle-HorizontalAlign="Right"
                ShowCollapseButton="True" Theme="BlackGlass">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent3" runat="server">
                        <dx:ASPxGridView ID="AccMoveGrid" runat="server" AccessibilityCompliant="True"
                            AutoGenerateColumns="False" ClientInstanceName="AccMoveGrid" DataSourceID="CurrentPays"
                            KeyFieldName="Poll" OnHeaderFilterFillItems="Filter" RightToLeft="True" Width="100%">
                            <ClientSideEvents EndCallback="OnEndCallback" SelectionChanged="grid_SelectionChanged" />
                            <SettingsPager PageSize="20000">
                            </SettingsPager>
                            <SettingsAdaptivity
                                AdaptivityMode="HideDataCellsWindowLimit"
                                HideDataCellsAtWindowInnerWidth="768"
                                AllowOnlyOneAdaptiveDetailExpanded="false"
                                AdaptiveDetailColumnCount="1">
                            </SettingsAdaptivity>

                            <Settings ShowGroupPanel="True" />
                            <SettingsBehavior AllowFocusedRow="True" />
                            <SettingsPopup>
                                <FilterControl AutoUpdatePosition="False">
                                </FilterControl>
                            </SettingsPopup>
                            <SettingsSearchPanel Visible="True" ShowClearButton="true" Delay="5000"></SettingsSearchPanel>

                            <SettingsLoadingPanel Delay="1" ImagePosition="Top" Mode="Disabled" Text="تحميل..." />
                            <SettingsText EmptyDataRow="لا يوجد بيانات" />
                            <StylesEditors>
                                <CalendarHeader Spacing="1px">
                                </CalendarHeader>
                                <ProgressBar Height="29px">
                                </ProgressBar>
                            </StylesEditors>
                            <Columns>
                                <dx:GridViewCommandColumn AllowTextTruncationInAdaptiveMode="True" ButtonRenderMode="Image" ButtonType="Image" MaxWidth="250" MinWidth="100" ShowInCustomizationForm="True" ToolTip="طباعة إيصال القيض" VisibleIndex="0" Width="15%">
                                    <CustomButtons>
                                        <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة إيصال">
                                            <Image Url="~/Content/Images/Print.png">
                                            </Image>
                                        </dx:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataColumn Caption="رقم الإيصال" FieldName="Poll" ShowInCustomizationForm="True" VisibleIndex="1">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="مقابل سداد" FieldName="PolNo" ShowInCustomizationForm="True" VisibleIndex="2">
                                    <EditFormSettings Visible="False" />
                                    <CellStyle Wrap="true">
                                    </CellStyle>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="رقم الملحق" FieldName="EndNo" ShowInCustomizationForm="True" VisibleIndex="3">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="رقم الإشعار" FieldName="LoadNo" ShowInCustomizationForm="True" VisibleIndex="4">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="رقم العقد" FieldName="Pol" ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
                                    <EditFormSettings Visible="False" />
                                    <CellStyle Wrap="true">
                                    </CellStyle>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="من طرف " FieldName="CustNAme"
                                    ShowInCustomizationForm="True" VisibleIndex="6">
                                    <EditFormSettings Visible="False" />
                                    <CellStyle Wrap="true">
                                    </CellStyle>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="الفرع" FieldName="BR" ShowInCustomizationForm="True" VisibleIndex="7">
                                    <EditFormSettings Visible="False" />
                                    <CellStyle Wrap="true">
                                    </CellStyle>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataTextColumn Caption="قيمة المحصل" FieldName="TOTPRM" ShowInCustomizationForm="True" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="n3">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataColumn Caption="طريقة الدفع" FieldName="PayTp" ShowInCustomizationForm="True" VisibleIndex="9">
                                    <CellStyle Wrap="true">
                                    </CellStyle>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataDateColumn Caption="تاريخ الإيصال / السداد" FieldName="IssuDate" ShowInCustomizationForm="True" VisibleIndex="10">
                                    <PropertiesDateEdit DisplayFormatInEditMode="True" DisplayFormatString="yyyy/MM/dd" EditFormatString="yyyy/MM/dd">
                                    </PropertiesDateEdit>
                                    <CellStyle Wrap="true">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataColumn Caption="العملة" FieldName="TPName" ShowInCustomizationForm="True" VisibleIndex="11">
                                    <CellStyle Wrap="False">
                                    </CellStyle>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="RecNo" FieldName="Poll" ShowInCustomizationForm="True" Visible="False" VisibleIndex="12">
                                    <CellStyle Wrap="true">
                                    </CellStyle>
                                </dx:GridViewDataColumn>
                            </Columns>
                            <GroupSummary>
                                <dx:ASPxSummaryItem FieldName="TOTPRM" SummaryType="Sum" ValueDisplayFormat="n3" />
                                <dx:ASPxSummaryItem FieldName="PolNo" SummaryType="Count" ValueDisplayFormat="n0" />
                            </GroupSummary>
                            <Paddings Padding="1px" />
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <br />
            <%-- OnHtmlRowPrepared="grid_HtmlRowPrepared"--%>
            <br />
        </div>
        <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" AllowResize="True"
            CloseAction="CloseButton"
            EnableViewState="False" PopupElementID="Image1" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowFooter="True" Width="700px" Modal="true"
            Height="600px" FooterText=""
            ClientInstanceName="popup" EnableHierarchyRecreation="True"
            EnableHotTrack="False">
            <ClientSideEvents CloseUp="SelectAndClosePopup" Init="puOnInit" Closing="function(s, e) {
	                                                                s.PerformCallback();
                                }" />
            <HeaderStyle>
                <Paddings PaddingLeft="7px" />
            </HeaderStyle>
            <LoadingPanelStyle ImageSpacing="5px">
            </LoadingPanelStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <br />
    </div>
</asp:Content>