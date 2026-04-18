<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintCheques.aspx.vb" Inherits="PrintCheques" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        var LastReport = null;
        var start;

        function grid_Init(s, e) {
            shMainGrid.Refresh();
        }

        function grid_BeginCallback(s, e) {
            start = new Date();
            //ClientCommandLabel.SetText(e.command);
            //ClientTimeLabel.SetText("تحميل...");
        }
        function SetSystem(sys, br) {
            Sys.SetValue(sys);
            Branch.SetValue(br);
            shMainGrid.SetVisible(true);
            cmbReports.SetVisible(true);

            shMainGrid.PerformCallback();
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
            var width = Math.max(0, document.documentElement.clientWidth) * 0.75;
            var height = Math.max(0, document.documentElement.clientHeight) * 0.75;
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
                let mhdr = 'وثيقة جديدة - '
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
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Search') {
                let hdr = s.cpResult
                let mhdr = 'بحث مفصل - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpShowDeleteConfirmBox && s.cpMyAttribute == 'Delete') {
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
            shMainGrid.DeleteRow(MainGrid.cpRowIndex);
        }

        function No_Click() {
            pcConfirmDelete.Hide();
        }

        function YesIss_Click() {
            pcConfirmIssue.Hide();
            shMainGrid.PerformCallback("Issue");
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
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxTextBox ID="txtSearch" runat="server" Width="400px"></dx:ASPxTextBox>
            <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                ClientInstanceName="shMainGrid"
                RightToLeft="True">
                <ClientSideEvents EndCallback="OnEndCallback" />
                <Settings ShowFilterRow="True" AutoFilterCondition="Contains"></Settings>
                <SettingsBehavior AllowFocusedRow="true" />

                <SettingsSearchPanel Visible="true" AllowTextInputTimer="true" CustomEditorID="txtSearch" />
                <Columns>
                    <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" Visible="false"></dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="DAILYNUM" Caption="رقم اذن الصرف" VisibleIndex="1"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Type" Caption="نوع القيد" ReadOnly="True" VisibleIndex="2"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="DAILYDTE" Caption="تاريخ القيد" PropertiesDateEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="3"></dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="DailyTyp" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ANALSNUM" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Comment" Caption="وصف القيد" VisibleIndex="4"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CustName" Caption="المستفيد" VisibleIndex="5"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Value" Caption="القيمة" PropertiesTextEdit-DisplayFormatString="n3" VisibleIndex="6"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DocNum" Caption="رقم الصك" VisibleIndex="7"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AccountName" Caption="على المصرف" VisibleIndex="8"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AccountNo" VisibleIndex="9" Visible="false"></dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" MinWidth="100" MaxWidth="250" Width="15%" Caption="طباعة" AllowTextTruncationInAdaptiveMode="true">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة">
                                <Image Url="~/Content/Images/Print.png">
                                </Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewCommandColumn>
                </Columns>
            </dx:ASPxGridView>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>'
                SelectCommand="SELECT MainJournal.DAILYNUM, MainJournal.DAILYDTE, MainJournal.DailyTyp ,MainJournal.ANALSNUM ,MainJournal.Comment, Journal.CustName, Journal.DocNum,iif(MainJournal.DailyTyp=1 ,iif(MainJournal.ANALSNUM='A','قيد يومية آلي','قيد يومية'),iif(MainJournal.ANALSNUM='SF','إ.صرف صناديق ذاتية','إذن صرف ')) As Type ,Journal.AccountNo,Accounts.AccountName,abs(Cr) As Value FROM MainJournal INNER JOIN Journal ON MainJournal.DAILYNUM = Journal.DAILYNUM AND MainJournal.DailyTyp = Journal.TP  INNER JOIN Accounts ON Journal.AccountNo=Accounts.AccountNo WHERE CustName is not null and DocNum is not null and CustName <>'' and DocNum <>'' and DailyTyp in (3,4) and journal.Dr=0 GROUP By MainJournal.DAILYNUM , MainJournal.Comment,Journal.CustName, Journal.DocNum , MainJournal.DAILYDTE, DailyTyp ,MainJournal.ANALSNUM ,Journal.AccountNo,Accounts.AccountName,cr  ORDER By MainJournal.DAILYDTE DESC"></asp:SqlDataSource>
            <dx:ASPxPopupControl ID="Popup" runat="server" AllowDragging="false" AllowResize="false" AutoUpdatePosition="true" ClientInstanceName="PrintPop" Modal="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled">
                <ClientSideEvents CloseUp="function(s,e){ MainGrid.PerformCallback();}" Init="puOnInit" />
            </dx:ASPxPopupControl>
        </div>
    </form>
</body>
</html>