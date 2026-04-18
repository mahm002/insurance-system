<%@ Page Language="VB" AutoEventWireup="false" Inherits="settlement_Sadad" CodeBehind="Sadad.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
            function ReturnToParentPage() {
                //debugger;
                var parentWindow = window.parent;
                parentWindow.SelectAndClosePopup();
                //var t = ExtPrm.GetValue();
                //window.parent.document.getElementById("EXTPRM").value = t;
            }
            function onStartAdd(s, e) {
                doOperation('Sadad');
            }

            function doOperation(name) {
                cbps.PerformCallback(name);
            }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxCallback ID="Cbps" ClientInstanceName="cbps" runat="server" OnCallback="Cbps_Callback"></dx:ASPxCallback>

        <div dir="rtl">
            <asp:SqlDataSource runat="server" ID="DataDetails" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                SelectCommand="select rtrim(MainJournal.DAILYNUM) As DAILYNUM, CONVERT(varchar(10), MainJournal.DAILYDTE, 111) AS DAILYDTE, Abs(Cr) As Credit,DocNum,CustName, rtrim(MainJournal.Comment) As Comment from dbo.MainJournal left join Journal on MainJournal.DAILYNUM=Journal.DAILYNUM and MainJournal.DailyTyp=tp where MainJournal.DailyPrv=1 and MainJournal.DailyTyp=3 and cr<>0 AND CustName IS NOT NULL AND RTRIM(CUSTNAME)<>'' "></asp:SqlDataSource>
            <dx:ASPxComboBox ID="DailyNo" ClientInstanceName="DailyNo" runat="server" DropDownStyle="DropDownList"
                IncrementalFilteringMode="Contains" ItemStyle-Wrap="True"
                DataSourceID="DataDetails" TextFormatString="{0}||{1}||{4}||{5}" ValueField="DAILYNUM" TextField="Comment" Width="100%" RightToLeft="True" AutoResizeWithContainer="True">
                <%--                <ClientSideEvents SelectedIndexChanged="function(s, e) {
	alert(DailyNo.GetValue());
}"></ClientSideEvents>--%>
                <Columns>
                    <dx:ListBoxColumn FieldName="DAILYNUM" Caption="رقم إذن الصرف" Width="5%"></dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="Comment" Caption="الوصف" Width="40%"></dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="DAILYDTE" Caption="تاريخ إذن الصرف" Width="10%"></dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="CustName" Caption="المستفيد" Width="25%"></dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="Credit" Caption="القيمة" Width="10%">
                    </dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="DocNum" Caption="رقم الصك" Width="10%"></dx:ListBoxColumn>
                </Columns>
                <ValidationSettings>
                    <RequiredField ErrorText="يرجى اختيار اذن الصرف من القائمة" IsRequired="True" />
                </ValidationSettings>
            </dx:ASPxComboBox>
            <hr />
            <dx:ASPxButton ID="ASPxButton" runat="server" Text="سداد" AutoPostBack="true">
                <ClientSideEvents Click="onStartAdd" />
                <%--<ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();
                                    }" />--%>
            </dx:ASPxButton>
        </div>
    </form>
</body>
</html>