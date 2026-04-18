<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HealthInsurance.aspx.vb" Inherits="Hinsure" %>

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
		    //var rate = DataGrid.GetEditValue("Rate");
		    //var sumins = parseFloat(DataGrid.GetEditValue("SumIns"));
		    //var sumins = parseFloat(DataGrid.GetEditValue("MaxForPerson")) * parseFloat(DataGrid.GetEditValue("EmpCnt"));
		    //var cnt = DataGrid.GetEditValue("EmpCnt");
		    //DataGrid.SetEditValue("SumIns", sumins);
		    //DataGrid.SetEditValue("Premium", (rate / 1000) * sumins);
		}
    </script>

    <style type="text/css">
        .auto-style2 {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dx-ac">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" EndCallback="OnEndCallback" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="70" />
                <PanelCollection>
                    <dx:PanelContent runat="server">

                        <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
                            <tr>
                                <td>
                                    <uc1:PolicyControl runat="server" ID="PolicyControl" />
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">

                                    <table dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td class="auto-style1">&nbsp;</td>
                                            <td class="auto-style2">التحمل :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="RespVal" runat="server" ClientInstanceName="RespVal" CssClass="1" Text="/" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">&nbsp;</td>
                                            <td class="auto-style2">شروط خاصة :</td>
                                            <td class="auto-style1">
                                                <dx:ASPxTokenBox ID="Notes" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dxeCaptionHARSys">
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxCheckBox ID="RateAll" runat="server" AccessibilityLabelText="" CheckState="Unchecked" CssClass="N"
                                                    RightToLeft="False" Text="احتساب القسط (السنوي) للكل - flat rate" TextAlign="Left" Width="260px">
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="Rate" runat="server" Text="0" Width="55px">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <dx:ASPxGridView ID="DataGrid" ClientInstanceName="DataGrid" runat="server" DataSourceID="DataS"
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

                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="EndNo" ReadOnly="True" VisibleIndex="2" CellStyle-HorizontalAlign="Right" Visible="true" Caption="رقم الملحق">
                                                            <EditFormSettings Visible="False" />

                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Name" ReadOnly="false" VisibleIndex="3" CellStyle-HorizontalAlign="Right" Caption="الاسم">
                                                            <EditFormSettings Visible="true" />
                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="EmpID" ReadOnly="false" VisibleIndex="4" CellStyle-HorizontalAlign="Right" Caption="الرقم الوظيفي">
                                                            <EditFormSettings Visible="true" />
                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="CardNo" ReadOnly="false" VisibleIndex="5" CellStyle-HorizontalAlign="Right" Caption="رقم البطاقة">
                                                            <EditFormSettings Visible="true" />
                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Relation" ReadOnly="false" VisibleIndex="6" CellStyle-HorizontalAlign="Right" Caption="الصفة / صلة القرابة">
                                                            <EditFormSettings Visible="true" />
                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataDateColumn FieldName="Dob" Caption="تاريخ الميلاد" EditFormSettings-Visible="True" CellStyle-HorizontalAlign="Right" VisibleIndex="7"
                                                            PropertiesDateEdit-DisplayFormatString="yyyy/MM/dd">
                                                            <EditFormSettings Visible="True" />
                                                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd" />
                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SumIns" ReadOnly="false" VisibleIndex="8" Caption="حدود التغطية للشخص " CellStyle-HorizontalAlign="Right"
                                                            PropertiesTextEdit-DisplayFormatString="n3">
                                                            <PropertiesTextEdit DisplayFormatString="n3"></PropertiesTextEdit>
                                                            <EditFormSettings Visible="true" />
                                                            <%--<PropertiesTextEdit>
                                                                   <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                               </PropertiesTextEdit>--%>
                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Premium" ReadOnly="false" VisibleIndex="9" CellStyle-HorizontalAlign="Right" Caption="القسط السنوي"
                                                            PropertiesTextEdit-DisplayFormatString="n3">
                                                            <PropertiesTextEdit DisplayFormatString="n3"></PropertiesTextEdit>
                                                            <EditFormSettings Visible="true" />

                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewCommandColumn Name="Commands" ShowEditButton="True" ShowDeleteButton="True"
                                                            ShowNewButtonInHeader="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                                        </dx:GridViewCommandColumn>
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
                                                    <SettingsText ConfirmDelete="سوف يتم شطب هذا الشخص من القائمة، هل أنت متأكد؟" />
                                                    <EditFormLayoutProperties>
                                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
                                                    </EditFormLayoutProperties>
                                                    <TotalSummary>
                                                        <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="Premium" Tag="Prm" FieldName="Premium" ShowInGroupFooterColumn="القيمة" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>
                                                        <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="SumIns" FieldName="SumIns" Tag="مجموع المسؤولية التأمين" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>
                                                    </TotalSummary>
                                                    <SettingsPopup>
                                                        <EditForm Width="600">
                                                            <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="768" />
                                                        </EditForm>
                                                        <FilterControl AutoUpdatePosition="False"></FilterControl>
                                                    </SettingsPopup>
                                                </dx:ASPxGridView>
                                                <asp:SqlDataSource ID="DataS" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                                    SelectCommand="SELECT OrderNo,EndNo,SerNo, GroupNo, Name, Dob, EmpID, CardNo, Relation, Rate, SumIns, Premium FROM HealthInsurance WHERE ([OrderNo] = @Order) AND ([EndNo]=@End)"
                                                    DeleteCommand="DELETE FROM [HealthInsurance] WHERE [SerNo] = @SerNo"
                                                    InsertCommand="INSERT INTO [HealthInsurance] (OrderNo, EndNo, LoadNo, SubIns, Name, Dob, EmpID, CardNo, Relation, SumIns, Premium)
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@Name ,@Dob, @EmpID, @CardNo, @Relation, @SumIns, @Premium)"
                                                    UpdateCommand="UPDATE [HealthInsurance] SET [Name] = @Name, [Dob] = @Dob , [EmpID] = @EmpID, [CardNo]=@CardNo,[Relation]=@Relation,
                                                       [SumIns] = @SumIns, [Premium] = @Premium WHERE [SerNo] = @SerNo">
                                                    <SelectParameters>
                                                        <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                        <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                    </SelectParameters>
                                                    <DeleteParameters>
                                                        <asp:Parameter Name="SerNo" Type="Int32" />
                                                    </DeleteParameters>
                                                    <InsertParameters>
                                                        <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                        <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                        <asp:QueryStringParameter Name="System" DbType="String" Direction="Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                                                        <asp:Parameter Name="Name" Type="String" />
                                                        <asp:Parameter Name="Dob" Type="DateTime" />
                                                        <asp:Parameter Name="EmpID" Type="String" />
                                                        <asp:Parameter Name="CardNo" Type="String" />
                                                        <asp:Parameter Name="Relation" Type="String" />
                                                        <asp:Parameter Name="SumIns" Type="Decimal" />
                                                        <asp:Parameter Name="Premium" Type="Decimal" />
                                                    </InsertParameters>
                                                    <UpdateParameters>
                                                        <asp:Parameter Name="Name" Type="String" />
                                                        <asp:Parameter Name="Dob" Type="DateTime" />
                                                        <asp:Parameter Name="EmpID" Type="String" />
                                                        <asp:Parameter Name="CardNo" Type="String" />
                                                        <asp:Parameter Name="Relation" Type="String" />
                                                        <asp:Parameter Name="SumIns" Type="Decimal" />
                                                        <asp:Parameter Name="Premium" Type="Decimal" />
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