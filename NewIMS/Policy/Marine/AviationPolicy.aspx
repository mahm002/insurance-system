<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AviationPolicy.aspx.vb" Inherits="AviationPolicy" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }
        function OnEndCallback(s, e) {
            //debugger;
            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'ImportExcel') {

                popup.SetSize(700, 300);
                popup.SetHeaderText('استيراد من ملف Excel');
                popup.SetContentHtml(null);
                popup.SetContentUrl(s.cpNewWindowUrl);
                popup.Show();
            }

            s.cpMyAttribute = ''
            s.cpNewWindowUrl = null

           //UpdateTimeoutTimer();/////////////////////////////

        }
		function OnEditorValueChanged(s, e) {
		    var rate = DataGrid.GetEditValue("Rate");
		    var sumins = parseFloat(DataGrid.GetEditValue("SumIns"));
		    DataGrid.SetEditValue("Premium", (rate / 1000) * sumins);
		}
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dx-ac">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" EndCallback="OnEndCallback" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="100" />
                <PanelCollection>
                    <dx:PanelContent runat="server">

                        <table id="TableForm" style="width: 100%;" dir="rtl">
                            <tr>
                                <td>
                                    <uc1:PolicyControl runat="server" id="PolicyControl" />
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">
                                    <%--                              <dx:ASPxRoundPanel runat="server" AllowCollapsingByHeaderClick="True" ID="PolicyData" RightToLeft="True" Width="100%" HorizontalAlign="Center">
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">--%>
                                    <table dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td class="auto-style1">
                                                <%-- <dx:ASPxHyperLink ImageUrl="../../Content/Images/upload_file_16px.png" ToolTip="تحميل ملف EXCEL"
                                                        NavigateUrl="javascript:ImportPopup();" ID="Import" runat="server" Visible="true"></dx:ASPxHyperLink>--%>

                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxCheckBox ID="RateAll" runat="server" AccessibilityLabelText="" CheckState="Unchecked" CssClass="N" RightToLeft="False" Text="احتساب السعر للكل" TextAlign="Left" Width="150px">
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="Rate" runat="server" CssClass="4" Text="0" Width="55px">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">التغطيات : </td>
                                            <td class="auto-style1" colspan="2">
                                                <dx:ASPxTokenBox ID="Covers" runat="server" AllowMouseWheel="True" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td class="auto-style1">&nbsp;</td>
                                            <td class="auto-style1">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <dx:ASPxGridView ID="DataGrid" ClientInstanceName="DataGrid" runat="server" DataSourceID="DataS" RightToLeft="True"
                                                    OnDataBinding="DataGrid_DataBinding"
                                                    OnDataBound="DataGrid_DataBound"
                                                    OnHtmlDataCellPrepared="DataGrid_HtmlDataCellPrepared"
                                                    KeyFieldName="SerNo" Width="100%"
                                                    EnableRowsCache="False" AutoGenerateColumns="False">
                                                    <%-- <ClientSideEvents EndCallback="function(s, e) {Premium.SetText(s.cpSummary);}" />--%>
                                                    <Columns>
                                                        <%-- <dx:GridViewCommandColumn id ShowEditButton="true" />--%>
                                                        <dx:GridViewDataTextColumn FieldName="SerNo" ReadOnly="True" VisibleIndex="0" Visible="false">
                                                            <EditFormSettings Visible="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="OrderNo" ReadOnly="True" VisibleIndex="1" Visible="false">
                                                            <EditFormSettings Visible="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="EndNo" ReadOnly="True" VisibleIndex="2" Visible="false">
                                                            <EditFormSettings Visible="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="PlaneType" ReadOnly="false" VisibleIndex="3" Caption="نوع الطائرة">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="PlaneMark" ReadOnly="false" VisibleIndex="4" Caption="علامة النداء">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="PlaneUse" ReadOnly="false" VisibleIndex="5" Caption="نوع الاستعمال">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="CrewSeatNo" ReadOnly="false" VisibleIndex="6" Caption="عدد مقاعد الطاقم">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="PassengerSeatNo" ReadOnly="false" VisibleIndex="7" Caption="عدد مقاعد الركاب">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ThirdPartyRespLimit" ReadOnly="false" VisibleIndex="8" Caption="حدود المسؤولية للطرف الثالث" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <PropertiesTextEdit DisplayFormatString="n3"></PropertiesTextEdit>
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="RespVal" ReadOnly="false" VisibleIndex="8" Caption="التحمل باستثناء الخسارة الكلية" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <PropertiesTextEdit DisplayFormatString="n3"></PropertiesTextEdit>
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SumIns" ReadOnly="false" VisibleIndex="11" Caption="مبلغ التأمين للجسم" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Rate" ReadOnly="false" VisibleIndex="12" Caption="السعر" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Premium" ReadOnly="true" VisibleIndex="13" Caption="القسط"
                                                            PropertiesTextEdit-DisplayFormatString="n3">
                                                            <PropertiesTextEdit DisplayFormatString="n3"></PropertiesTextEdit>
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewCommandColumn Name="Commands" ShowEditButton="True" ShowDeleteButton="True" ShowNewButtonInHeader="True" ShowInCustomizationForm="True" VisibleIndex="0"></dx:GridViewCommandColumn>
                                                    </Columns>
                                                    <SettingsCommandButton>
                                                        <DeleteButton Text="مسح">
                                                        </DeleteButton>
                                                        <UpdateButton Text="حفظ">
                                                        </UpdateButton>
                                                        <NewButton Text="إضافة">
                                                        </NewButton>
                                                        <CancelButton Text="تراجع">
                                                        </CancelButton>
                                                        <EditButton Text="تعديل">
                                                        </EditButton>
                                                    </SettingsCommandButton>
                                                    <SettingsEditing Mode="EditForm" NewItemRowPosition="Top"></SettingsEditing>
                                                    <Settings ShowFooter="True"></Settings>
                                                    <SettingsSearchPanel Visible="True" />
                                                    <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                                    <SettingsText ConfirmDelete="سوف يتم شطب هذه الطائرة، هل أنت متأكد؟" />
                                                    <EditFormLayoutProperties>
                                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
                                                    </EditFormLayoutProperties>
                                                    <TotalSummary>
                                                        <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="Premium" Tag="Prm" FieldName="Premium" ShowInGroupFooterColumn="القيمة" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>
                                                        <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="SumIns" FieldName="SumIns" Tag="مجموع مبالغ التأمين" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>
                                                    </TotalSummary>
                                                    <SettingsPopup>
                                                        <EditForm Width="600">
                                                            <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="768" />
                                                        </EditForm>
                                                        <FilterControl AutoUpdatePosition="False"></FilterControl>
                                                    </SettingsPopup>
                                                </dx:ASPxGridView>
                                                <asp:SqlDataSource ID="DataS" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                                    SelectCommand="SELECT  OrderNo, Endno, LoadNo, SubIns, GroupNo, PlaneType, PlaneMark, PlaneUse, CrewSeatNo, PassengerSeatNo, Covers, ThirdPartyRespLimit, RespVal, SumIns, Rate, Premium, SerNo
                                                            FROM  AviationFile WHERE ([OrderNo] = @Order) AND ([EndNo]=@End)"
                                                    DeleteCommand="DELETE FROM [AviationFile] WHERE [SerNo] = @SerNo"
                                                    InsertCommand="INSERT INTO [AviationFile] (OrderNo,Endno,LoadNo,SubIns,PlaneType,PlaneMark,PlaneUse,CrewSeatNo,PassengerSeatNo,ThirdPartyRespLimit,RespVal,SumIns,Rate,Premium)
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@PlaneType ,@PlaneMark, @PlaneUse, @CrewSeatNo,  @PassengerSeatNo, @ThirdPartyRespLimit, @RespVal, @SumIns,@Rate,(@Rate/1000)*@SumIns)"
                                                    UpdateCommand="UPDATE [AviationFile] SET [PlaneType] = @PlaneType, [PlaneMark] = @PlaneMark, [PlaneUse] = @PlaneUse , [CrewSeatNo] = @CrewSeatNo , [PassengerSeatNo] = @PassengerSeatNo, [ThirdPartyRespLimit] = @ThirdPartyRespLimit, [RespVal] = @RespVal,
                                                       [SumIns] = @SumIns, [Premium] = (@Rate/1000)*@SumIns, [Rate] = @Rate WHERE [SerNo] = @SerNo">
                                                    <SelectParameters>
                                                        <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                        <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                    </SelectParameters>
                                                    <DeleteParameters>
                                                        <asp:Parameter Name="SerNo" Type="Int32" />
                                                    </DeleteParameters>

                                                    <InsertParameters>
                                                        <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                        <asp:QueryStringParameter Name="System" DbType="String" Direction="Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                                                        <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                        <asp:Parameter Name="PlaneType" Type="String" />
                                                        <asp:Parameter Name="PlaneMark" Type="String" />
                                                        <asp:Parameter Name="PlaneUse" Type="String" />
                                                        <asp:Parameter Name="CrewSeatNo" Type="Int16" />
                                                        <asp:Parameter Name="PassengerSeatNo" Type="Int16" />
                                                        <asp:Parameter Name="ThirdPartyRespLimit" Type="Decimal" />
                                                        <asp:Parameter Name="RespVal" Type="Decimal" />
                                                        <asp:Parameter Name="SumIns" Type="Decimal" />
                                                        <asp:Parameter Name="Rate" Type="Decimal" />
                                                    </InsertParameters>

                                                    <UpdateParameters>
                                                        <asp:Parameter Name="PlaneType" Type="String" />
                                                        <asp:Parameter Name="PlaneMark" Type="String" />
                                                        <asp:Parameter Name="PlaneUse" Type="String" />
                                                        <asp:Parameter Name="CrewSeatNo" Type="Int16" />
                                                        <asp:Parameter Name="PassengerSeatNo" Type="Int16" />
                                                        <asp:Parameter Name="ThirdPartyRespLimit" Type="Decimal" />
                                                        <asp:Parameter Name="RespVal" Type="Decimal" />
                                                        <asp:Parameter Name="SumIns" Type="Decimal" />
                                                        <asp:Parameter Name="Rate" Type="Decimal" />
                                                        <asp:Parameter Name="SerNo" Type="Int32" />
                                                    </UpdateParameters>
                                                </asp:SqlDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <dx:ASPxPopupControl ID="popup" runat="server" AllowDragging="True" AllowResize="True" Modal="true"
                                                    ClientInstanceName="popup" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled">
                                                    <ClientSideEvents CloseUp="function(s,e){
                                                                                   DataGrid.PerformCallback();}" />
                                                    <ContentCollection>
                                                        <dx:PopupControlContentControl runat="server">
                                                        </dx:PopupControlContentControl>
                                                    </ContentCollection>
                                                </dx:ASPxPopupControl>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>