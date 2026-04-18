<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BulkReceipts.aspx.vb" Inherits=".BULK" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function HideOrShow(s, e) {

            //if (s.GetValue() == "0") {
            //    AccNo.SetVisible(false);
            //    Bank.SetVisible(false);
            //    AccName.SetVisible(false);
            //}
            //else {
            //    if (s.GetValue() == "1" || s.GetValue() == "2") {
            //        AccNo.SetVisible(true);
            //        Bank.SetVisible(true);
            //        AccName.SetVisible(false);
            //    }
            //    else {
            //        AccNo.SetVisible(false);
            //        Bank.SetVisible(false);
            //        AccName.SetVisible(true);
            //    }

            //}

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server">
            <asp:SqlDataSource ID="MoveDS" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                SelectCommand="SELECT PolicyFile.PolNo, PolicyFile.TOTPRM, PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain,PolicyFile.EndNo,PolicyFile.LoadNo,ISNULL(PolicyFile.Inbox, 0) AS InBox,DBO.GetExtraCatName('Cur',PolicyFile.Currency) As Curr FROM PolicyFile LEFT OUTER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo LEFT OUTER JOIN BranchInfo ON PolicyFile.Branch = BranchInfo.BranchNo LEFT OUTER JOIN SUBSYSTEMS ON PolicyFile.SubIns = SUBSYSTEMS.SUBSYSNO AND SUBSYSTEMS.Branch = PolicyFile.Branch  WHERE Inbox<>TOTPRM AND IssuDate=@IssuDate AND PolicyFile.Branch=@Br AND PayType=1 AND PolicyFile.SubIns=@Sys and PolicyFile.Stop=0">
                <SelectParameters>
                    <asp:ControlParameter ControlID="MoveDate" Name="IssuDate" PropertyName="Value" />
                    <asp:SessionParameter Name="Br" SessionField="Branch" DefaultValue="0" />
                    <asp:ControlParameter ControlID="Sys" Name="Sys" PropertyName="Value" DefaultValue="0" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="Systems" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                SelectCommand="SELECT SUBSYSNO, RTRIM(SUBSYSNAME) AS SUBSYSNAME FROM SUBSYSTEMS WHERE (SysType = 1) AND (Branch = dbo.MainCenter())"></asp:SqlDataSource>

            <dx:ASPxDateEdit ID="MoveDate" runat="server" PickerDisplayMode="ScrollPicker"
                Caption="ليوم :">
            </dx:ASPxDateEdit>

            <dx:ASPxComboBox ID="Sys" runat="server" EnableCallbackMode="True"
                IncrementalFilteringMode="StartsWith"
                DataSourceID="Systems" TextField="SUBSYSNAME" ValueField="SUBSYSNO"
                Width="100%" RightToLeft="True" Caption=":">
                <%--<ClientSideEvents ValueChanged="HideOrShow"></ClientSideEvents>--%>
            </dx:ASPxComboBox>

            <dx:ASPxGridView ID="GridData" ClientInstanceName="griddata" runat="server" Width="100%"
                DataSourceID="MoveDS" KeyFieldName="PolNo;EndNo;LoadNo" AutoGenerateColumns="False">
                <SettingsPopup>
                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                </SettingsPopup>
                <ClientSideEvents SelectionChanged="grid_SelectionChanged" />
                <Columns>
                    <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataColumn FieldName="PolNo" Caption="رقم الوثيقة" VisibleIndex="1"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="EndNo" Caption="رقم الملحق" VisibleIndex="2"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="LoadNo" Caption="رقم الإشعار" VisibleIndex="3"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="TOTPRM" Caption="اجمالي القسط" VisibleIndex="4"></dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="Curr" Caption="العملة" VisibleIndex="5"></dx:GridViewDataColumn>
                </Columns>
                <SettingsPager PageSize="10" />
            </dx:ASPxGridView>

            <br />
            <br />
        </div>
    </form>
</body>
</html>