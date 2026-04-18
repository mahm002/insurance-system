<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="Default.aspx.vb" Inherits="Default1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
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
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'AgentNote') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult
                let mhdr = 'طباعة - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'IssuSerial') {
                //window.open(s.cpNewWindowUrl);
                //PrintPop.SetSize(1000, 800);
                let hdr = s.cpResult
                let mhdr = 'إصدار - '
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
                let mhdr = 'وثيقة جديدة - '
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
                pcConfirmIssue.SetHeaderText(s.cpCust)
            }
            s.cpMyAttribute = ''
            s.cpNewWindowUrl = null

            UpdateTimeoutTimer();/////////////////////////////

        }
    </script>

    <div>
        <asp:SqlDataSource ID="Reports" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="SELECT [Rep], [Desc] FROM [ReportMap] WHERE ([SubIns]='Re') ORDER BY [SubIns]"></asp:SqlDataSource>
        <dx:ASPxComboBox ID="cmbReports" ClientInstanceName="cmbReports" runat="server" NullText="Reinsurance Reports"
            DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith" SettingsLoadingPanel-Text="Please Wait... "
            TextField="Desc" ValueField="Rep" Width="100%" DataSourceID="Reports"
            EnableSynchronization="false" RightToLeft="false" AllowEllipsisInText="True">
            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnReportChange(s); }" EndCallback="OnEndCallback" />
            <CaptionSettings Position="Left" VerticalAlign="Middle" />
            <CaptionStyle Font-Bold="True" Font-Names="Sakkal Majalla" Font-Size="Large">
            </CaptionStyle>
        </dx:ASPxComboBox>
        <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="true" AllowDragging="false" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
            PopupHorizontalAlign="WindowCenter" Modal="true">
            <ClientSideEvents CloseUp="function(s,e){
                //MainGrid.Refresh();
                MainGrid.PerformCallback();}"
                Init="puOnInit" />
        </dx:ASPxPopupControl>
    </div>
</asp:Content>