<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="SettlmentsToPay.aspx.vb" Inherits=".SettlmentsPay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        var LastReport = null;
        var start;

        function grid_Init(s, e) {
            MainGrid.Refresh();
        }

        function grid_BeginCallback(s, e) {
            start = new Date();
            //ClientCommandLabel.SetText(e.command);
            //ClientTimeLabel.SetText("تحميل...");
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

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'طباعة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'AgentNote') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'طباعة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Cashier') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'طباعة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'DebitNote') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'طباعة - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'IssuSerial') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult;
                let mhdr = 'إصدار - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Distribute') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1400, 800);
                let hdr = s.cpResult;
                let mhdr = 'توزيع الوثيقة - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Edit') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult;
                let mhdr = 'تحرير - ';
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'New') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(s.cpSize, 800);
                let hdr = s.cpResult;
                let mhdr='وثيقة جديدة - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Renew') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(800, 800);
                let hdr = s.cpResult;
                let mhdr = 'تجديد - ';
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
            if(s.cpNewWindowUrl != null &&  s.cpMyAttribute == 'AllIssues') {

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
            if (s.cpShowDeleteConfirmBox && s.cpMyAttribute == 'Delete'){
                pcConfirmDelete.Show();
                pcConfirmDelete.SetHeaderText(s.cpCust)
            }
            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Issuance') {
                pcConfirmIssue.Show();
                pcConfirmIssue.SetHeaderText(s.cpCust);
            }
            s.cpMyAttribute = '';
            s.cpNewWindowUrl = null;

            //UpdateTimeoutTimer();/////////////////////////////

        }

        function Yes_Click() {
            pcConfirmDelete.Hide();
            MainGrid.DeleteRow(MainGrid.cpRowIndex);
           }

        function No_Click() {
            pcConfirmDelete.Hide();
        }

        function YesIss_Click() {
            pcConfirmIssue.Hide();
            MainGrid.PerformCallback("Issue");
      }

        function NoIss_Click() {
            pcConfirmIssue.Hide();
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

        function SelectAndClosePopup() {
            PrintPop.Hide();

        }
    </script>
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
        KeyFieldName="ClmNo;TPID;No" RightToLeft="True" Width="100%" ClientInstanceName="MainGrid">
        <ClientSideEvents Init="grid_Init" BeginCallback="grid_BeginCallback" EndCallback="OnEndCallback" ToolbarItemClick="OnToolbarItemClick" />
        <SettingsSearchPanel Visible="True" ShowClearButton="true" Delay="5000"></SettingsSearchPanel>
        <SettingsBehavior AllowFocusedRow="true" />
        <Settings ShowGroupPanel="True" VerticalScrollBarStyle="VirtualSmooth" VerticalScrollableHeight="700" VerticalScrollBarMode="Visible" ShowFooter="true" />
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ClmNo" Caption="رقم الحادث" ReadOnly="True" VisibleIndex="1"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CustName" Caption="اسم المشترك" ReadOnly="True" VisibleIndex="2"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TPID" ReadOnly="True" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="No" Caption="رقم التسوية" ReadOnly="True" VisibleIndex="4"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PayTo" Caption="المستفيد" VisibleIndex="5"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SettelementDesc" Caption="وصف التسوية" VisibleIndex="6"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Date" VisibleIndex="7" Caption="تاريخ الإداخال"
                PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UserName" Caption="اسم المستخدم" VisibleIndex="8"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Net" Caption="صافي التعويض" ReadOnly="True" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="n3" CellStyle-HorizontalAlign="Right"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DAILYNUM" Caption="رقم إذن الصرف" ReadOnly="True" VisibleIndex="10"></dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="DAILYDTE" Caption="تاريخ إذن الصرف" VisibleIndex="11"></dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="SubIns" VisibleIndex="12" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SysName" Caption="نوع التأمين" ReadOnly="True" VisibleIndex="13"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AccountNo" ReadOnly="True" VisibleIndex="14" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" Caption="عمليات" AllowTextTruncationInAdaptiveMode="true">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة">
                        <Image Url="~/Content/Images/Print.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="ApprovePay" Text="اعتماد الصرف">
                        <Image Url="~/Content/Images/Approved.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewCommandColumn>
        </Columns>
    </dx:ASPxGridView>

    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>'
        SelectCommand="SELECT MainSattelement.ClmNo, MainSattelement.TPID, MainSattelement.No, MainSattelement.PayTo, MainSattelement.SettelementDesc, MainSattelement.Date, MainSattelement.UserName,  Customerfile.CustName,isnull(SUM(DetailSettelement.Value),0) AS Net,rtrim(MainSattelement.DAILYNUM) as DAILYNUM, MainSattelement.DAILYDTE,MainClaimFile.SubIns,dbo.SysName(MainClaimFile.SubIns) As SysName, dbo.GetAccountNo('Clm',MainClaimFile.SubIns,0,MainClaimFile.Branch,0) As AccountNo FROM  MainSattelement left outer JOIN DetailSettelement ON DetailSettelement.ClmNo = MainSattelement.ClmNo AND DetailSettelement.TPID = MainSattelement.TPID AND DetailSettelement.No = MainSattelement.No inner join MainClaimFile on MainSattelement.ClmNo=MainClaimFile.ClmNo inner join PolicyFile on PolicyFile.PolNo=MainClaimFile.PolNo and PolicyFile.EndNo=MainClaimFile.EndNo and PolicyFile.LoadNo=MainClaimFile.LoadNo inner join CustomerFile on PolicyFile.custno=CustomerFile.CustNo where MainClaimFile.ClmCloseDate is null GROUP BY MainSattelement.ClmNo, MainSattelement.TPID, MainSattelement.No, MainSattelement.PayTo, MainSattelement.SettelementDesc, MainSattelement.Date, MainSattelement.DAILYNUM,MainSattelement.DAILYDTE,MainClaimFile.SubIns,MainClaimFile.Branch,MainSattelement.UserName,Customerfile.CustName having SUM(DetailSettelement.Value)<>0 and DAILYNUM='0' AND MainClaimFile.Branch=@Branch order by date desc">

        <SelectParameters>
            <asp:SessionParameter SessionField="Branch" Name="Branch"></asp:SessionParameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="false" AllowDragging="false" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
        PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="true" Modal="false">
        <ClientSideEvents CloseUp="function(s,e){ MainGrid.PerformCallback();}" Init="puOnInit" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pcConfirmIssue" runat="server" ClientInstanceName="pcConfirmIssue" ShowCloseButton="false"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="إصدار الوثيقة">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" Font-Bold="true">
                <table dir="rtl">
                    <tr>
                        <td style="vertical-align: central;" colspan="1">عند الموافقة سيتم إصدار إذن صرف آلي وسيتم احتساب التسوية من ضمن التسويات المسددة، هل أنت متأكد؟؟
                        </td>
                    </tr>
                    <tr>
                        <td><%--<a href="javascript:YesIss_Click()">موافق</a>--%>
                            <dx:ASPxButton ID="yesIssButton" runat="server" Text="موافق" AutoPostBack="false">
                                <ClientSideEvents Click="YesIss_Click" />
                            </dx:ASPxButton>
                        </td>
                        <td><%--<a href="javascript:NoIss_Click()">غير موافق</a>--%>
                            <dx:ASPxButton ID="noIssButton" runat="server" Text="غير موافق" AutoPostBack="false">
                                <ClientSideEvents Click="NoIss_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>