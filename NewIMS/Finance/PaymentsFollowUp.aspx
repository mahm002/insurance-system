<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="PaymentsFollowUp.aspx.vb" Inherits=".PaymentsFollowUp" %>

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
    <dx:ASPxGridView ID="Grid" runat="server" 
        ClientInstanceName="grid"
        DataSourceID="SqlDataSource"
        KeyFieldName="PolNo;EndNo;LoadNo;InvoiceID"
        SettingsDetail-AllowOnlyOneMasterRowExpanded="True"
        SettingsBehavior-AllowFocusedRow="true"
        Width="100%" SettingsPager-PageSize="25">
        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
        <ClientSideEvents EndCallback="OnEndCallback" />
        <Columns>
            <dx:GridViewDataColumn FieldName="CustName" Caption="المؤمن له" />
            <dx:GridViewDataColumn FieldName="PolNo" Caption=" رقم الوثيقة " />
            <dx:GridViewDataColumn FieldName="EndNo" Caption="رقم الملحق" MaxWidth="1" Width="1%" Visible="false" />
            <dx:GridViewDataColumn FieldName="LoadNo" Caption="رقم الشحنة" MaxWidth="1" Width="1%" Visible="false" />
            <dx:GridViewDataTextColumn FieldName="InvoiceDate" Caption="تاريخ الإصدار" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="Right">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TotalAmount" Caption="إجمالي القسط">
                <PropertiesTextEdit DisplayFormatString="n3" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Payed" Caption="المسدد">
                <PropertiesTextEdit DisplayFormatString="n3" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Remain" Caption="المتبقي">
                <PropertiesTextEdit DisplayFormatString="n3" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PercentageP" Caption="نسبة المسدد % ">
                <DataItemTemplate>
                    <dx:ASPxProgressBar ID="Apb" runat="server" Theme="Office2003Blue"
                        OnDataBound="APb_DataBound"
                        Value='<%# Math.Abs(Eval("PercentageP")) %>' Width="100px" DisplayFormatString="0.00"
                        DisplayMode="Percentage">
                    </dx:ASPxProgressBar>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PercentageR" Caption="نسبة المتبقي % ">
                <DataItemTemplate>
                    <dx:ASPxProgressBar ID="pb" runat="server" Theme="Office2003Blue"
                        OnDataBound="Pb_DataBound"
                        Value='<%# Math.Abs(Eval("PercentageR")) %>' Width="100px" DisplayFormatString="0.00"
                        DisplayMode="Percentage">
                    </dx:ASPxProgressBar>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataColumn FieldName="IssuingUser" Caption="صادرة بواسطة" />
            <dx:GridViewDataColumn FieldName="BranchName" Caption="الفرع" />
            <dx:GridViewDataColumn FieldName="SystemName" Caption="نوع التأمين" />

            <dx:GridViewDataColumn FieldName="Branch" Caption="الفرع" Visible="false" />
            <dx:GridViewDataColumn FieldName="SubIns" Caption="نوع التأمين" Visible="false" />
            <dx:GridViewDataTextColumn FieldName="InvoiceID" Visible="false">
            </dx:GridViewDataTextColumn>
        </Columns>
        <Templates>
            <DetailRow>
                <div style="padding: 3px 3px 2px 3px">
                    <dx:ASPxPageControl runat="server" ID="pageControl" Width="100%" EnableCallBacks="true">
                        <TabPages>
                            <dx:TabPage Text=" إيصالات القبض " Visible="true">
                                <ContentCollection>
                                    <dx:ContentControl runat="server">
                                        <dx:ASPxGridView ID="SlipGrid" ClientInstanceName="SlipGrid" runat="server"
                                            DataSourceID="Reciepts"
                                            OnCustomButtonCallback="SlipGrid_CustomButtonCallback" EnableCallBacks="true"
                                            OnCustomCallback="SlipGrid_CustomCallback"
                                            OnCustomButtonInitialize="SlipGrid_CustomButtonInitialize"
                                            KeyFieldName="RecNo" Width="100%"
                                            OnBeforePerformDataSelect="TraetyGrid_BeforePerformDataSelect">
                                            <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="768" AllowOnlyOneAdaptiveDetailExpanded="true" />
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
                                                if (e.buttonID=='DPrint'){
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
                                                <dx:GridViewDataTextColumn FieldName="RecNo" Caption="رقم ايصال القبض">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="DocDat" Caption=" تاريخ ايصال القبض">
                                                    <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="DAILYNUM" Caption="رقم اليومية">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="DAILYDTE" Caption=" تاريخ اليومية">
                                                    <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="RecPayment" Caption=" المبلغ المستلم">
                                                    <PropertiesTextEdit DisplayFormatString="n3" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="CurUser" Caption=" مُصدر القيد " Visible="true">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="TPName" Caption="'طريقة الدفع">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="UserName" Caption=" المستلم " Visible="true">
                                                </dx:GridViewDataTextColumn>
                                                 <dx:GridViewCommandColumn AllowDragDrop="True" ButtonRenderMode="Image" ButtonType="Image" VisibleIndex="0" CellStyle-HorizontalAlign="Center" Caption="" AllowTextTruncationInAdaptiveMode="true" MinWidth="90" MaxWidth="90" Width="7%">
                                                     <CustomButtons>
                                                       <%--  <dx:GridViewCommandColumnCustomButton ID="Edit" Text="تحرير">
                                                             <Image Url="~/Content/Images/Edit.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>--%>
                                                         <dx:GridViewCommandColumnCustomButton ID="Print" Text="طباعة إيصال القبض">
                                                             <Image Url="~/Content/Images/Print.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>
                                                           <dx:GridViewCommandColumnCustomButton ID="DPrint" Text="طباعة قيد اليومية">
                                                             <Image Url="~/Content/Images/Print.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>
                                                         <%--<dx:GridViewCommandColumnCustomButton ID="Cancel" Text="إلغاء">
                                                             <Image Url="~/Content/Images/Delete.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>--%>
                                                      <%--   <dx:GridViewCommandColumnCustomButton ID="Delete" Text="مسح القسيمة">
                                                             <Image Url="~/Content/Images/DeleteRed.png">
                                                             </Image>
                                                         </dx:GridViewCommandColumnCustomButton>--%>
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
        <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
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
    <asp:SqlDataSource ID="SqlDataSource" runat="server" ViewStateMode="Enabled"
        ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
        SelectCommand="Payments"
        SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    <asp:SqlDataSource ID="Reciepts" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
        SelectCommand="PaymentsHistory"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter SessionField="InvoiceID" DefaultValue="0" Name="Order"></asp:SessionParameter>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>