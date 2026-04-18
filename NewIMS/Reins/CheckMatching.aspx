<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="CheckMatching.aspx.vb" Inherits=".CheckMatching" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxGridView ID="MainGrid" runat="server" ClientInstanceName="MainGrid" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" RightToLeft="True" KeyFieldName="OrderNo"
        SettingsLoadingPanel-Text="تحميل..."
        Width="100%">
        <ClientSideEvents EndCallback="OnEndCallback" ToolbarItemClick="OnToolbarItemClick" />
        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />

        <SettingsSearchPanel CustomEditorID="tbToolbarSearch" />
        <SettingsBehavior AllowFocusedRow="true" />
        <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="DataAware" />

        <SettingsPager PageSize="25">
            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
        </SettingsPager>
        <Settings ShowGroupPanel="True" VerticalScrollBarStyle="VirtualSmooth" VerticalScrollableHeight="700" VerticalScrollBarMode="Visible" />
        <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" AllowEdit="False" AllowInsert="False" AllowDelete="true" />
        <SettingsPopup>
            <FilterControl AutoUpdatePosition="true">
            </FilterControl>
        </SettingsPopup>
        <SettingsSearchPanel Visible="True" ShowClearButton="true"></SettingsSearchPanel>
        <SettingsText SearchPanelEditorNullText="بحث..." />
<%--        <Columns>
<dx:GridViewDataTextColumn FieldName="OrderNo" ReadOnly="True" VisibleIndex="0"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="LoadNo" ReadOnly="True" VisibleIndex="1"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="EndNo" ReadOnly="True" VisibleIndex="2"></dx:GridViewDataTextColumn>
<dx:GridViewDataDateColumn FieldName="IssuDate" VisibleIndex="3"></dx:GridViewDataDateColumn>
<dx:GridViewDataTextColumn FieldName="Sys" ReadOnly="True" VisibleIndex="4"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="PolNo" VisibleIndex="5"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="Branch" ReadOnly="True" VisibleIndex="6"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="PolicyFile_NetPrm" ReadOnly="True" VisibleIndex="7"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="NetPrm_Sum" ReadOnly="True" VisibleIndex="8"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="Difference" ReadOnly="True" VisibleIndex="9"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="BranchName" VisibleIndex="10"></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="SUBSYSNAME" VisibleIndex="11"></dx:GridViewDataTextColumn>
</Columns>--%>
<SettingsBehavior AllowFocusedRow="true" />
        <Paddings Padding="0px" />
        <Border BorderWidth="0px" />
        <BorderBottom BorderWidth="1px" />
  <%--      <Toolbars>
            <dx:GridViewToolbar>
                <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                <Items>
                    <%--<dx:GridViewToolbarItem Command="Custom" Name="NewPolicy" Text="وثيقة جديدة" Image-IconID="tasks_newtask_16x16" AdaptivePriority="2" />
                    <dx:GridViewToolbarItem Command="Refresh" Name="ExtraSearch" Text="بحث مفصل" BeginGroup="true" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="2" />
                    <%-- <dx:GridViewToolbarItem Command="ShowCustomizationWindow" Name="ExtraSearch" Text="بحث موسع" Image-IconID="functionlibrary_lookupreference_16x16" AdaptivePriority="3" />
                    <dx:GridViewToolbarItem Command="Edit" />
                    <dx:GridViewToolbarItem Command="Delete" />
                    <dx:GridViewToolbarItem Text="تصدير إلى" Image-IconID="actions_download_16x16office2013" BeginGroup="true" AdaptivePriority="1">
                        <Items>
                            <dx:GridViewToolbarItem Command="ExportToPdf" Text="PDF" />
                            <dx:GridViewToolbarItem Command="ExportToDocx" Text="WORD" />
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="XLS" />
                            <dx:GridViewToolbarItem Command="ExportToXlsx" Text="XLSX" />
                            <dx:GridViewToolbarItem Command="ExportToRtf" Text="RTF" />
                            <dx:GridViewToolbarItem Command="ExportToCsv" Text="CSV" />
                            <%-- <dx:GridViewToolbarItem Name="CustomExportToXLS" Text="Export to XLS(WYSIWYG)" Image-IconID="export_exporttoxls_16x16office2013" />--%><%-- <dx:GridViewToolbarItem Name="CustomExportToXLSX" Text="Export to XLSX(WYSIWYG)" Image-IconID="export_exporttoxlsx_16x16office2013" />
                        </Items>
                    </dx:GridViewToolbarItem>  
                    <dx:GridViewToolbarItem Alignment="Right">
                        <Template>
                            <dx:ASPxButtonEdit ID="tbToolbarSearch" runat="server" NullText="بحث..." Height="100%" ClearButton-DisplayMode="Always">
                                <Buttons>
                                    <dx:SpinButtonExtended Image-IconID="find_find_16x16gray" />
                                </Buttons>
                            </dx:ASPxButtonEdit>
                        </Template>
                    </dx:GridViewToolbarItem>
                </Items>
            </dx:GridViewToolbar>
        </Toolbars>  --%>
        <Columns>
            <dx:GridViewDataTextColumn FieldName="CustName" VisibleIndex="2" Caption="المؤمن له" CellStyle-HorizontalAlign="Right" >
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PolNo" VisibleIndex="1" Caption="رقم الوثيقة" CellStyle-HorizontalAlign="Right" >
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EndNo" VisibleIndex="2" Caption="ملحق" CellStyle-HorizontalAlign="Right" Visible="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LoadNo" VisibleIndex="3" Caption="إشعار شحنة" CellStyle-HorizontalAlign="Right" Visible="True">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrderNo" VisibleIndex="4" Caption="رقم الطلب" CellStyle-HorizontalAlign="Right" Visible="false">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IssuDate" VisibleIndex="5" Caption="تاريخ الإصدار" AdaptivePriority="1" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            
            <dx:GridViewDataTextColumn FieldName="PolicyFile_NetPrm" VisibleIndex="7" Caption=" صافي قسط الإصدار" PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="N3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="NetPrm_Sum" VisibleIndex="8" Caption="  صافي قسط التوزيع " PropertiesTextEdit-DisplayFormatString="n3" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="n3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
                 <dx:GridViewDataTextColumn FieldName="Difference" VisibleIndex="9" Caption="  الفارق في التوزيع " PropertiesTextEdit-DisplayFormatString="n3" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="n3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <%--   <dx:GridViewDataTextColumn FieldName="PayAs" VisibleIndex="10" Caption="طريقة السداد" CellStyle-HorizontalAlign="Right">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="NetPrm_Existence" CellStyle-BackColor="OrangeRed" CellStyle-ForeColor="White" VisibleIndex="10" Caption="سبب عدم المطابقة" CellStyle-HorizontalAlign="Right" Visible="true">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="BranchName" VisibleIndex="11" Caption="الفرع" CellStyle-HorizontalAlign="Right" Visible="true">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="SUBSYSNAME" VisibleIndex="12" Caption="نوع التأمين" CellStyle-HorizontalAlign="Right" Visible="true">
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <%--<dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" Caption="عمليات" AllowTextTruncationInAdaptiveMode="true">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="Distribute" Text="توزيع">
                        <Image Url="~/Content/Images/DistPolicy.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة">
                        <Image Url="~/Content/Images/Print.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <%--      <dx:GridViewCommandColumnCustomButton ID="Renew" Text="تجديد">
                        <Image Url="~/Content/Images/renew_16px.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Issuance" Text="إصدار">
                        <Image Url="~/Content/Images/Accept.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Delete" Text="إلغاء الطلب">
                        <Image Url="~/Content/Images/Delete.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewCommandColumn>--%>
        </Columns>
    </dx:ASPxGridView>
<asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>' 
    SelectCommand="SELECT 
    PolicyFile.OrderNo, 
    PolicyFile.LoadNo, 
    PolicyFile.EndNo, 
    PolicyFile.IssuDate,
    PolicyFile.SubIns As Sys,
    PolicyFile.PolNo,
    PolicyFile.Branch,
    (PolicyFile.NETPRM) AS PolicyFile_NetPrm,
    NetPrm_Summary.NetPrm_Sum,
    ((PolicyFile.NETPRM) - COALESCE(NetPrm_Summary.NetPrm_Sum, 0)) AS Difference,
    CASE 
        WHEN NetPrm_Summary.NetPrm_Sum IS NULL THEN 'لا يوجد سجل في توزيعات الإعادة'
        ELSE 'موزع بقيمة مختلفة عن الإصدار'
    END AS NetPrm_Existence,
    BranchName, 
    SUBSYSTEMS.SUBSYSNAME, 
    CustName
FROM PolicyFile
LEFT JOIN (
    SELECT 
        PolNo, 
        EndNo, 
        LoadNo,
        SUM((Net+War)) AS NetPrm_Sum
    FROM NetPrm 
    GROUP BY PolNo, EndNo, LoadNo
) NetPrm_Summary ON NetPrm_Summary.PolNo = PolicyFile.PolNo 
    AND NetPrm_Summary.EndNo = PolicyFile.EndNo 
    AND NetPrm_Summary.LoadNo = PolicyFile.LoadNo
LEFT JOIN BranchInfo on BranchInfo.BranchNo = PolicyFile.Branch
LEFT JOIN SUBSYSTEMS on SUBSYSTEMS.SUBSYSNO = PolicyFile.SubIns 
    AND SUBSYSTEMS.Branch = dbo.MainCenter()
LEFT JOIN CustomerFile on CustomerFile.CustNo = PolicyFile.CustNo
WHERE ABS((PolicyFile.NETPRM) - COALESCE(NetPrm_Summary.NetPrm_Sum, 0)) > 0.1
    AND PolicyFile.SubIns NOT IN ('01','02','03','04','OR','27','08','PH','MN','MK','07','MD')  
    AND PolicyFile.IssuDate IS NOT NULL"></asp:SqlDataSource>
</asp:Content>
