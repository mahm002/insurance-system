<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="FAC.aspx.vb" Inherits=".FAC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../scripts/Scripts.js"></script>
    
    <script type="text/javascript">
        var rowVisibleIndex;

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
      
        function OnEndCallback(s, e) {

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'NewSlip') {
                PrintPop.SetHeaderText(s.cpResult);
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Edit') {
                PrintPop.SetHeaderText(s.cpResult);
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                PrintPop.SetHeaderText(s.cpResult);
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'Cancel') {
                PrintPop.SetHeaderText(s.cpResult);
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
            }

            s.cpMyAttribute=''
            s.cpNewWindowUrl = null
            //grid.PerformCallback();
        }
        function SelectAndClosePopup() {
            popup.Hide();
            grid.PerformCallback();
        }
        function onCallbackMultiValues(values) {
           //alert(values);
            SlipGrid.PerformCallback('Delete,' + values);
        }
        function Yes_Click() {
            pcConfirmCancel.Hide()
            SlipGrid.GetRowValues(SlipGrid.GetFocusedRowIndex(), 'SlipNo', onCallbackMultiValues);
           }

        function No_Click() {
            pcConfirmCancel.Hide()
        }
        function YesIss_Click() {
            pcConfirmCancel.Hide();
            FacGrid.PerformCallback("Issue");
           }

        function NoIss_Click() {
            pcConfirmIssue.Hide()
        }

        function GetPickUpPoints(values) {
            PickUpPoints = values;

            if (rowVisibleIndex == 'إضافة قسيمة اختياري') {
                values = rowVisibleIndex + ',' + values
                FacGrid.PerformCallback(values);
                rowVisibleIndex = '';
            }
            rowVisibleIndex = '';
            values = '';

        }
       
    </script>
     <dx:ASPxGridView ID="Grid" runat="server" ClientInstanceName="grid" DataSourceID="SqlDataSource" 
        
        KeyFieldName="PolNo;EndNo;LoadNo;FacPolRef"
        SettingsDetail-AllowOnlyOneMasterRowExpanded="True" 
        SettingsBehavior-AllowFocusedRow="true"
        Width="100%" SettingsPager-PageSize="25">
         <ClientSideEvents EndCallback="OnEndCallback" />
         <Columns>
             <dx:GridViewDataColumn FieldName="CustName" Caption="المؤمن له" />
             <dx:GridViewDataColumn FieldName="PolNo" Caption=" رقم الوثيقة " />
             <dx:GridViewDataColumn FieldName="EndNo" Caption="Endor. No" MaxWidth="1" Width="1%" />
             <dx:GridViewDataColumn FieldName="LoadNo" Caption="Load. No" MaxWidth="1" Width="1%" />
             <dx:GridViewDataTextColumn FieldName="IssuDate" VisibleIndex="6" Caption="تاريخ الإصدار" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                 <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                 </PropertiesTextEdit>
                 <CellStyle HorizontalAlign="Right">
                 </CellStyle>
             </dx:GridViewDataTextColumn>
             <dx:GridViewDataColumn FieldName="BranchName" Caption="الفرع" />
             <dx:GridViewDataColumn FieldName="Sys" Caption="نوع التأمين" />
             <dx:GridViewDataTextColumn FieldName="SumIns" Caption="إجمالي مبلغ التأمين المكتتب">
                 <PropertiesTextEdit DisplayFormatString="n3" />
             </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="TotreinsPRM" Caption="صافي القسط المكتتب">
                 <PropertiesTextEdit DisplayFormatString="n3" />
             </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="FacPolRef" Visible="false">
             </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="ElectiveRatio" Caption="نسبة الفائض على الاتفاقية % ">
                 <DataItemTemplate>
                     <dx:ASPxProgressBar ID="pb" runat="server" Theme="Office2003Blue"
                         OnDataBound="Pb_DataBound"
                         Value='<%# Math.Abs(Eval("ElectiveRatio")) %>' Width="100px" DisplayFormatString="0.00"
                         DisplayMode="Percentage">
                     </dx:ASPxProgressBar>
                 </DataItemTemplate>
             </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="Accepted" Caption="النسبة المسندة % ">
                 <DataItemTemplate>
                     <dx:ASPxProgressBar ID="Apb" runat="server" Height="21px" Theme="Office2003Blue"
                         OnDataBound="APb_DataBound"
                         Value='<%# Math.Abs(Eval("Accepted")) %>' Width="100px" DisplayFormatString="0.00"
                         DisplayMode="Percentage">
                     </dx:ASPxProgressBar>
                 </DataItemTemplate>
             </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="Currency" Caption="نوع العملة" Visible="false">
             </dx:GridViewDataTextColumn>
             <%--<dx:GridViewDataTextColumn FieldName="ExcRate" Caption="سعر الصرف ">
                 <PropertiesTextEdit DisplayFormatString="N3" />
             </dx:GridViewDataTextColumn>--%>
         </Columns>
         <Templates>
             <DetailRow>
                 <div style="padding: 3px 3px 2px 3px">
                     <dx:ASPxPageControl runat="server" ID="pageControl" Width="100%" EnableCallBacks="true">
                         <TabPages>
                             <dx:TabPage Text="الاتفاقي" Visible="true">
                                 <ContentCollection>
                                     <dx:ContentControl runat="server">
                                         <dx:ASPxGridView ID="TraetyGrid" runat="server" DataSourceID="SqlDataSourcePol"
                                            KeyFieldName="PolNo;EndNo;LoadNo" Width="100%" 
                                            OnBeforePerformDataSelect="TraetyGrid_BeforePerformDataSelect">
                                             <Columns>
                                                 <dx:GridViewDataTextColumn FieldName="TreatyDisSI" Caption=" مبلغ التأمين الصادر اتفاقي">
                                                     <PropertiesTextEdit DisplayFormatString="n3" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="TreatyDistPrm" Caption="القسط الصادر اتفاقي">
                                                     <PropertiesTextEdit DisplayFormatString="n3" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="TreatyRatio" Caption="% نسبة الاتفاقي" PropertiesTextEdit-DisplayFormatString="{0} %">
                                                 </dx:GridViewDataTextColumn>
                                             </Columns>
                                             <SettingsPager EnableAdaptivity="true" />
                                             <%--  <SettingsDetail ShowDetailRow="true" />
                                            <Settings ShowFooter="true" />
                                            <SettingsPager EnableAdaptivity="true" />
                                            <Styles Header-Wrap="True"/>--%>
                                         </dx:ASPxGridView>
                                     </dx:ContentControl>
                                 </ContentCollection>
                             </dx:TabPage>
                             <dx:TabPage Text="الاختياري" Visible="true">
                                 <ContentCollection>
                                     <dx:ContentControl runat="server">
                                         <dx:ASPxGridView ID="FacGrid" runat="server" ClientInstanceName="FacGrid" 
                                            DataSourceID="SqlDataSourcePol" 
                                            EnableRowsCache="False" OnHtmlDataCellPrepared="FacGrid_HtmlDataCellPrepared" 
                                            OnCustomCallback="FG_CustomCallback" EnableCallBacks="true"
                                            KeyFieldName="PolNo;EndNo;LoadNo;FacPolRef" Width="100%" EnablePagingGestures="False" 
                                            OnBeforePerformDataSelect="TraetyGrid_BeforePerformDataSelect">
                                             <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" />
                                             <ClientSideEvents  
                                                 EndCallback="OnEndCallback"
                                                 RowClick="function(s, e) {
                                                    if (rowVisibleIndex=='إضافة قسيمة اختياري'){
                                                        s.GetRowValues(e.visibleIndex, 'PolNo;EndNo;LoadNo;FacPolRef', GetPickUpPoints);
                                                    }
                                           }" />
                                             <SettingsBehavior AllowFocusedRow="true" />
                                             <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" AllowEdit="False" AllowInsert="False" AllowDelete="true" />
                                             <Columns>
                                                 <dx:GridViewDataTextColumn FieldName="PolNo" Caption="Pol" Visible="false">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="EndNo" Caption="end" Visible="false">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="LoadNo" Caption="load" Visible="false">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="FacPolRef" Caption="sn" Visible="false">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="ElectiveSI" Caption=" مبلغ التأمين الاختياري">
                                                     <PropertiesTextEdit DisplayFormatString="n3" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="ElectivePRM" Caption=" القسط الاختياري">
                                                     <PropertiesTextEdit DisplayFormatString="n3" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="ElectiveRatio" Caption="% نسبة الاختياري"
                                                    PropertiesTextEdit-DisplayFormatString="{0} %">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataColumn FieldName=" " ToolTip="menu" VisibleIndex="5" CellStyle-HorizontalAlign="Left">
                                                     <DataItemTemplate>
                                                         <dx:ASPxMenu ID="FacMenu" ClientInstanceName="FacMenu" runat="server"
                                                            AutoSeparators="RootOnly" Width="100%">
                                                             <ClientSideEvents ItemMouseOver="function(s, e) {
                                                                              rowVisibleIndex=e.item.GetText();
                                                                            }" />
                                                             <Items>
                                                                 <dx:MenuItem Text="إجراءات">
                                                                 </dx:MenuItem>
                                                             </Items>
                                                             <SubMenuStyle GutterWidth="0px"></SubMenuStyle>
                                                         </dx:ASPxMenu>
                                                     </DataItemTemplate>
                                                 </dx:GridViewDataColumn>
                                             </Columns>
                                             <SettingsPager EnableAdaptivity="true" />
                                         </dx:ASPxGridView>
                                     </dx:ContentControl>
                                 </ContentCollection>
                             </dx:TabPage>
                         </TabPages>
                         <TabPages>
                             <dx:TabPage Text=" قسائم الاسناد " Visible="true">
                                 <ContentCollection>
                                     <dx:ContentControl runat="server">
                                         <dx:ASPxGridView ID="SlipGrid" ClientInstanceName="SlipGrid" runat="server" DataSourceID="SlipsSource"
                                             OnCustomButtonCallback="SlipGrid_CustomButtonCallback" EnableCallBacks="true"
                                             OnCustomCallback="SlipGrid_CustomCallback"
                                             KeyFieldName="SlipNo" Width="100%"
                                             OnBeforePerformDataSelect="TraetyGrid_BeforePerformDataSelect">
                                             <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" />
                                             <ClientSideEvents EndCallback="function(s,e){grid.PerformCallback();
                                                 OnEndCallback(s, e);}"  
                                                 CustomButtonClick="function(s, e) {
                                                                                                 
                                                   if (e.buttonID=='Edit'){
                                                        e.processOnServer = true;
                                                    }
                                                   if (e.buttonID=='Print'){
                                                         e.processOnServer = true;
                                                    }
                                                if (e.buttonID=='Cancel'){
                                                         //OnCustomButtonClick;
                                                         //pcConfirmCancel.Show();
                                                        e.processOnServer = true;
                                                    }
                                                 if (e.buttonID=='Delete'){
                                                          pcConfirmCancel.Show();
                                                    }
                                           }" />
                                             <SettingsBehavior AllowFocusedRow="true" />
                                             <Columns>
                                                 <dx:GridViewDataTextColumn FieldName="SlipNo" Caption="SlipNo">
                                                     <PropertiesTextEdit DisplayFormatString="n3" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="Messr" Caption="ReInsurer">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="SlipDate" Caption="Slip Date">
                                                      <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="PolNo" Caption="PolNo" Visible="false">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="EndNo" Caption="EndNo" Visible="false">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="EndNo" Caption="LoadNo" Visible="false">
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="AcceptedShare" Caption="Accepted %" PropertiesTextEdit-DisplayFormatString="{0:n2}%">
                                                     <%--<PropertiesTextEdit DisplayFormatString="P" />--%>
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="CommisionRatio" Caption="Commision %" PropertiesTextEdit-DisplayFormatString="{0:n2}%">
                                                     <%--<PropertiesTextEdit DisplayFormatString="P" />--%>
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn FieldName="Balance" Caption="Balance ">
                                                     <PropertiesTextEdit DisplayFormatString="n3" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewCommandColumn AllowDragDrop="True" ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" Caption="عمليات" AllowTextTruncationInAdaptiveMode="true" MinWidth="90" MaxWidth="90" Width="7%">
                                                     <CustomButtons>
                                                         <dx:GridViewCommandColumnCustomButton ID="Edit" Text="تحرير">
                                                             <Image Url="~/Content/Images/Edit.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>
                                                         <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة">
                                                             <Image Url="~/Content/Images/Print.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>
                                                         <dx:GridViewCommandColumnCustomButton ID="Cancel" Text="إلغاء">
                                                             <Image Url="~/Content/Images/Delete.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>
                                                         <dx:GridViewCommandColumnCustomButton ID="Delete" Text="مسح القسيمة">
                                                             <Image Url="~/Content/Images/DeleteRed.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>
                                                     </CustomButtons>
                                                     <CellStyle HorizontalAlign="Center">
                                                     </CellStyle>
                                                 </dx:GridViewCommandColumn>
                                             </Columns>
                                             <SettingsPager EnableAdaptivity="true" />
                                             <Styles Header-Wrap="True" />
                                         </dx:ASPxGridView>
                                     </dx:ContentControl>
                                 </ContentCollection>
                             </dx:TabPage>
                         </TabPages>
                     </dx:ASPxPageControl>
                 </div>
             </DetailRow>
         </Templates>
         <Settings ShowGroupPanel="true" />
         <SettingsBehavior EnableCustomizationWindow="true" />
         <SettingsDetail ShowDetailRow="true" />
         <SettingsSearchPanel Visible="True">
         </SettingsSearchPanel>
    </dx:ASPxGridView>
    <dx:ASPxPopupControl ID="Popup" runat="server" AllowDragging="true" AllowResize="true" ClientInstanceName="PrintPop" 
         Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled">
        <ClientSideEvents    
             CloseUp="function(s,e){grid.PerformCallback();}" 
             Init="puOnInit" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pcConfirmCancel" runat="server" ClientInstanceName="pcConfirmCancel" ShowCloseButton="false"
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            HeaderText="إلغاء القسيمة" Theme="iOS">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table dir="rtl">
                    <tr>
                        <td style="align-items: center;" colspan="2">تأكيد إلغاء القسيمة؟
                            </td>
                    </tr>
                    <tr>
                        <td><a href="javascript:Yes_Click()">نعم</a> </td>
                        <td><a href="javascript:No_Click()">لا</a> </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <asp:SqlDataSource  ID="SqlDataSource" runat="server" ViewStateMode="Enabled" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
        SelectCommand="FacPolicies"
        SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="SqlDataSourcePol" 
        ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
        SelectCommand="FacPoliciesPol"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="Ref" SessionField="FacPolRef" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SlipsSource" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>' 
            SelectCommand="SELECT  SlipNo, CSlipNo, FacClosingSlips.PolNo, FacClosingSlips.EndNo, FacClosingSlips.LoadNo, SlipDate, PolRef, SubIns, FacClosingSlips.InsuranceType, InsuredName,
                            Period, OriginalSI, OriginalNet, Currency, ExRate, RefNo, Recom, AcceptedShare, CommisionRatio, FacSI, FacNet, Commision, Balance,
                            rtrim([dbo].[GetExtraCatName]('Cur',Currency)) As Curr,rtrim([dbo].[GetExtraCatName]('Recom',Recom)) As Messr
                            FROM FacClosingSlips JOIN NetPrm on FacClosingSlips.PolRef=NetPrm.FacPolRef where PolRef=@Ref">
        <SelectParameters>
            <asp:SessionParameter SessionField="FacPolRef" DefaultValue="0" Name="Ref"></asp:SessionParameter>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
