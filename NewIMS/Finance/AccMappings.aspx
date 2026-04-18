<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="AccMappings.aspx.vb" Inherits=".AccMappings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div dir="rtl">
        <%--        <dx:BootstrapComboBox ID="Systems" runat="server" Width="50%" OnValueChanged="Systems_ValueChanged"
             DataSourceID="SqlDataSource" TextField="SUBSYSNAME" ValueField="SUBSYSNO"
            IncrementalFilteringMode="Contains">
        </dx:BootstrapComboBox>--%>

        <dx:ASPxComboBox ID="SystemTp" runat="server" ClientInstanceName="SystemTp" DataSourceID="SqlDataSource"
            SelectedIndex="-1" ValueType="System.String"
            DropDownStyle="DropDownList" RightToLeft="True" TextField="SUBSYSNAME" ValueField="SUBSYSNO" Width="450px">
            <ClientSideEvents SelectedIndexChanged="function(s, e) {JournalMapping.PerformCallback();}" />
            <ValidationSettings ValidationGroup="Data" Display="Dynamic">
                <RequiredField ErrorText="نوع الترخيص مطلوب" IsRequired="True" />
            </ValidationSettings>
        </dx:ASPxComboBox>

        <dx:ASPxCallback ID="ASPxCallback1" runat="server">
        </dx:ASPxCallback>

        <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>
            "
            SelectCommand="SELECT rtrim(SUBSYSNO) As SUBSYSNO, rtrim(SUBSYSNAME) As SUBSYSNAME FROM SUBSYSTEMS
                        WHERE (Branch = dbo.MainCenter()) AND (SysType = '01')"></asp:SqlDataSource>

        <dx:ASPxGridView ID="JournalMapping" runat="server" ClientInstanceName="JournalMapping" AutoGenerateColumns="False" KeyFieldName="SubIns;AccTp"
            DataSourceID="SqlDataSource1" Width="100%">
            <SettingsPopup>
                <FilterControl AutoUpdatePosition="False"></FilterControl>
            </SettingsPopup>
            <SettingsBehavior AllowFocusedRow="true" />
            <Columns>
                <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0"></dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="SubIns" VisibleIndex="1" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SUBSYSNAME" VisibleIndex="2" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="AccTp" VisibleIndex="3" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="IDENTIFIER" Caption="اسم الحساب" ReadOnly="True" VisibleIndex="4">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataComboBoxColumn Caption="رقم الحساب في الدليل" CellStyle-Wrap="False" EditFormSettings-Caption="رقم الحساب في الدليل"
                    FieldName="AccountNum" VisibleIndex="5">
                    <PropertiesComboBox TextField="AccountName" ValueField="AccountNo" DataSourceID="Accds" DropDownStyle="DropDownList"
                        IncrementalFilteringMode="Contains" ValueType="System.String"
                        TextFormatString="{1}{2}" EnableCallbackMode="true" CallbackPageSize="10">
                    </PropertiesComboBox>

                    <EditFormSettings Caption="رقم الحساب في الدليل"></EditFormSettings>

                    <CellStyle Wrap="False"></CellStyle>
                </dx:GridViewDataComboBoxColumn>
            </Columns>
        </dx:ASPxGridView>
        <asp:SqlDataSource ID="Accds" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4"></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
            SelectCommand="select SubIns,SUBSYSNAME,AccTp,RTRIM(Accessor)+ ' / ' + RTRIM(SUBSYSNAME) AS IDENTIFIER, AccountNum
                                from SUBSYSTEMS
                                left join DailyJournal on DailyJournal.SubIns=SUBSYSNO
                                left JOIN EXTRAINFO ON TP='JournalType' AND DailyJournal.AccTp=TPName
                                where (branch=dbo.MainCenter()) and (SysType='01') AND (DailyJournal.SubIns = @SubS)"
            UpdateCommand="Update DailyJournal set SubIns=@SubIns, AccTp=@AccTp, AccountNum=@AccountNum where SubIns=@SubIns and AccTp=@AccTp">
            <SelectParameters>
                <asp:ControlParameter ControlID="SystemTp" DefaultValue="0" Name="SubS" PropertyName="Value" />
            </SelectParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="SystemTp" DbType="String" Name="SubIns" PropertyName="Value" />
                <asp:Parameter Name="AccTp" Type="String" />
                <asp:Parameter Name="AccountNum" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>