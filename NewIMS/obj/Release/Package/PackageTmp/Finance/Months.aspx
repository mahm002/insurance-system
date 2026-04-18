<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="Months.aspx.vb" Inherits=".Months" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function ShowScanPopup() {
            popup.SetSize(750, 550);
            popup.SetContentUrl('Cheek.aspx');
            popup.Show();
        }
        function SelectAndClosePopup() {
            popup.Hide();
        }
    </script>
    <div>
        <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            DeleteCommand="Delete From MonthCheck where Month=@Month"
            InsertCommand="INSERT INTO MonthCheck (Month) VALUES (@Month)"
            SelectCommand="SELECT Month from MonthCheck order by Month">
            <DeleteParameters>
                <asp:SessionParameter Name="Month" SessionField="Month" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Month" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>

        <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" Width="100%"
            DataSourceID="SqlDataSource" KeyFieldName="Month"
            OnRowDeleting="DelRow"
            OnRowInserting="InsRow"
            AutoGenerateColumns="False"
            SettingsBehavior-ConfirmDelete="true"
            SettingsText-ConfirmDelete="سوف يتم اقفال هذا الشهر، هل أنت متأكد؟"
            SettingsCommandButton-NewButton-Text="فتح شهر جديد"
            SettingsCommandButton-DeleteButton-Text="أقفال الشهر">

            <SettingsCommandButton>
                <UpdateButton Text="فتح الشهر" RenderMode="Image" Image-IconID="actions_save_16x16devav">
                    <Image IconID="actions_save_16x16devav"></Image>
                </UpdateButton>
                <CancelButton Text="تراجع" RenderMode="Image" Image-IconID="actions_undo_16x16devav">
                    <Image IconID="actions_undo_16x16devav"></Image>
                </CancelButton>
            </SettingsCommandButton>
            <Columns>
                <dx:GridViewDataTextColumn Caption="الشهر/السنة" FieldName="Month" VisibleIndex="2">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsPager Mode="ShowAllRecords" />
            <SettingsEditing EditFormColumnCount="3" />
            <Settings ShowTitlePanel="true" ShowGroupPanel="True" />
            <SettingsText Title="تحديد حالة الأشهر ( المفتوحة والمقفلة )" />
            <Columns>
                <dx:GridViewCommandColumn ShowNewButtonInHeader="True" VisibleIndex="0" ShowDeleteButton="True">
                </dx:GridViewCommandColumn>
            </Columns>
        </dx:ASPxGridView>
    </div>
</asp:Content>