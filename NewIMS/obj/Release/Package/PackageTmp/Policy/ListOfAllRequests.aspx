<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ListOfAllRequests.aspx.vb" Inherits=".ListOfAllRequests" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxGridView ID="CancelRequestsGrid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                KeyFieldName="Id" Width="100%" RightToLeft="True" OnCustomButtonCallback="CancelRequestsGrid_CustomButtonCallback"
                SettingsBehavior-AllowFocusedRow="true">
                <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
                <Settings ShowFilterRow="True" AutoFilterCondition="Contains"></Settings>
                <SettingsPager PageSize="25">
                    <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                </SettingsPager>
                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />
                <Toolbars>
                    <dx:GridViewToolbar>
                        <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                        <Items>
                            <dx:GridViewToolbarItem Text="تصدير إلى" Image-IconID="actions_download_16x16office2013" BeginGroup="true" AdaptivePriority="1">
                                <Items>
                                    <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF" />
                                    <dx:GridViewToolbarItem Command="ExportToDocx" Text="WORD" />
                                    <dx:GridViewToolbarItem Command="ExportToXls" Text="XLS" />
                                    <dx:GridViewToolbarItem Command="ExportToXlsx" Text="XLSX" />
                                    <dx:GridViewToolbarItem Command="ExportToRtf" Text="RTF" />
                                    <dx:GridViewToolbarItem Command="ExportToCsv" Text="CSV" />
                                </Items>
                            </dx:GridViewToolbarItem>
                        </Items>
                    </dx:GridViewToolbar>
                </Toolbars>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="Type" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="Action" Visible="false" />
                    <dx:GridViewDataCheckColumn FieldName="IsRead" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="UserId" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="OrderNo" ReadOnly="True" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="PolNo" ReadOnly="True" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="EndNo" ReadOnly="True" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="LoadNo" ReadOnly="True" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="Sys" ReadOnly="True" Visible="false" />

                    <dx:GridViewDataTextColumn FieldName="Message" Caption="سبب الإلغاء" AdaptivePriority="3" AllowTextTruncationInAdaptiveMode="true" VisibleIndex="4" />
                    <dx:GridViewDataDateColumn FieldName="Timestamp" Caption="وقت وتاريخ الطلب" AdaptivePriority="2" VisibleIndex="2" PropertiesDateEdit-DisplayFormatString="HH:mm:ss yyyy/MM/dd dddd" />
                    <dx:GridViewDataCheckColumn FieldName="IsTreated" Caption="حالة الطلب" AdaptivePriority="1" VisibleIndex="5" />
                    <dx:GridViewDataTextColumn FieldName="TreatedBy" Caption="تمت معالجة الطلب بواسطة" AdaptivePriority="4" VisibleIndex="6" />
                    <dx:GridViewDataTextColumn FieldName="GeneratedBy" Caption="مقدم الطلب" AdaptivePriority="1" VisibleIndex="3" />
                    <dx:GridViewDataDateColumn FieldName="TreatingDate" Caption="وقت وتاريخ المعالجة" AdaptivePriority="4" VisibleIndex="7" PropertiesDateEdit-DisplayFormatString="HH:mm:ss yyyy/MM/dd dddd" />
                    <dx:GridViewDataTextColumn FieldName="OrderNo" Caption="وقت وتاريخ الإصدار " VisibleIndex="1">
                        <DataItemTemplate>
                            <dx:ASPxLabel ID="IssuDate" runat="server" OnDataBound="IssuDate_DataBound"
                                Value='<%# Eval("OrderNo") %>'>
                            </dx:ASPxLabel>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" CellStyle-HorizontalAlign="Center" VisibleIndex="0"
                        MinWidth="100" MaxWidth="250" Width="10%" Caption="إجراءات" AllowTextTruncationInAdaptiveMode="true" ShowClearFilterButton="True">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="Cancel" Text="اعتماد الطلب">
                                <Image Url="~/Content/Images/DeleteRed.png"></Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                        <CellStyle HorizontalAlign="Center" />
                    </dx:GridViewCommandColumn>
                </Columns>
            </dx:ASPxGridView>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                SelectCommand="AllNotificationsByUserId" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="UserId" DefaultValue="0" Name="UserId" Type="Int32"></asp:QueryStringParameter>
                    <asp:QueryStringParameter QueryStringField="Type" DefaultValue="0" Name="Type" Type="Int32"></asp:QueryStringParameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>