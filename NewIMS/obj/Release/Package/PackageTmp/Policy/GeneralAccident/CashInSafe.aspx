<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CashInSafe.aspx.vb" Inherits="CashInSafe" %>

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
		    var sumins = parseFloat(DataGrid.GetEditValue("CashIn")) + parseFloat(DataGrid.GetEditValue("Body"));
		    DataGrid.SetEditValue("Premium", (rate / 100) * (sumins));
		}
    </script>
    <style type="text/css">
        .auto-style2 {
            text-align: left;
        }
        .auto-style3 {
            text-align: left;
            height: 18px;
        }
        .auto-style4 {
            text-align: right;
            height: 18px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dx-ac">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" EndCallback="OnEndCallback" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="0" />
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
                                            <td class="auto-style1">

                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style1">
                                                <strong>
                                                <dx:ASPxCheckBox ID="RateAll" runat="server" CheckState="Unchecked" CssClass="N" RightToLeft="False" Text="احتساب السعر للكل" TextAlign="Left" Width="150px">
                                                </dx:ASPxCheckBox>
                                                </strong>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="Rate" runat="server" Text="0" Width="55px">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"><strong>بند التحمل :</strong></td>
                                            <td class="auto-style4" colspan="2">
                                                <dx:ASPxTextBox ID="RespVal" runat="server" ClientInstanceName="RespVal" CssClass="1" Text="/" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">
                                                <strong>الشروط والاستثناءات :</strong></td>
                                            <td class="auto-style1" colspan="2">
                                                <dx:ASPxTokenBox ID="Conds" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2"><strong>ملاحظات</strong> :</td>
                                            <td class="auto-style1" colspan="2">
                                                <dx:ASPxTokenBox ID="Notes" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
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
                                                        <dx:GridViewDataTextColumn FieldName="SafeType" ReadOnly="false" VisibleIndex="3" Caption="نوع الخزينة">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SafePlaces" ReadOnly="false" VisibleIndex="4" Caption="مكان وجودها">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Size" ReadOnly="false" VisibleIndex="5" Caption="الحجم">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SafeFix" ReadOnly="false" VisibleIndex="6" Caption="التثبيت">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SafeSecurity" ReadOnly="false" VisibleIndex="7" Caption="طريقة الفتح">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="CashIn" ReadOnly="false" VisibleIndex="8" Caption="المبلغ داخل الخزينة"
                                                            PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>

                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Body" ReadOnly="false" VisibleIndex="9" Caption="قيمة جسم الخزينة"
                                                            PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>

                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Rate" ReadOnly="false" VisibleIndex="10" Caption="السعر" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>

                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Premium" ReadOnly="true" VisibleIndex="11" Caption="القسط"
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
                                                    <SettingsText ConfirmDelete="سوف يتم شطب هذه الخزينة، هل أنت متأكد؟" />
                                                    <EditFormLayoutProperties>
                                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
                                                    </EditFormLayoutProperties>
                                                    <TotalSummary>
                                                        <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="Premium" Tag="Prm" FieldName="Premium" ShowInGroupFooterColumn="القيمة" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>
                                                        <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="CashIn" FieldName="CashIn + Body" Tag="مجموع مبالغ التأمين" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>
                                                    </TotalSummary>
                                                    <SettingsPopup>
                                                        <EditForm Width="600">
                                                            <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="768" />
                                                        </EditForm>
                                                        <FilterControl AutoUpdatePosition="False"></FilterControl>
                                                    </SettingsPopup>
                                                </dx:ASPxGridView>
                                                <asp:SqlDataSource ID="DataS" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                                    SelectCommand="SELECT OrderNo,EndNo,SerNo, SafeType, SafePlaces, Size, SafeFix, SafeSecurity, CashIn, Body, Premium, Rate FROM CashInSafe WHERE ([OrderNo] = @Order) AND ([EndNo]=@End)"
                                                    DeleteCommand="DELETE FROM [CashInSafe] WHERE [SerNo] = @SerNo"
                                                    InsertCommand="INSERT INTO [CashInSafe] (OrderNo, EndNo, LoadNo, SubIns, SafeType, SafePlaces, Size, SafeFix, SafeSecurity, CashIn, Body, Premium,Rate)
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@SafeType , @SafePlaces, @Size, @SafeFix, @SafeSecurity, @CashIn, @Body, (@Rate/100)*(@CashIn+@Body),@Rate)"
                                                    UpdateCommand="UPDATE [CashInSafe] SET [SafeType] = @SafeType, [SafePlaces] = @SafePlaces, [Size] = @Size , [SafeFix] = @SafeFix, [SafeSecurity]=@SafeSecurity,
                                                       [CashIn] = @CashIn, [Body] = @Body, [Premium] = (@Rate/100)*(@CashIn+@Body), [Rate] = @Rate WHERE [SerNo] = @SerNo">
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
                                                        <asp:Parameter Name="SafeType" Type="String" />
                                                        <asp:Parameter Name="SafePlaces" Type="String" />
                                                        <asp:Parameter Name="Size" Type="String" />
                                                        <asp:Parameter Name="SafeFix" Type="String" />
                                                        <asp:Parameter Name="SafeSecurity" Type="String" />
                                                        <asp:Parameter Name="CashIn" Type="Decimal" />
                                                        <asp:Parameter Name="Body" Type="Decimal" />
                                                        <asp:Parameter Name="Rate" Type="Decimal" />
                                                    </InsertParameters>
                                                    <UpdateParameters>
                                                        <asp:Parameter Name="SafeType" Type="String" />
                                                        <asp:Parameter Name="SafePlaces" Type="String" />
                                                        <asp:Parameter Name="Size" Type="String" />
                                                        <asp:Parameter Name="SafeFix" Type="String" />
                                                        <asp:Parameter Name="SafeSecurity" Type="String" />
                                                        <asp:Parameter Name="CashIn" Type="Decimal" />
                                                        <asp:Parameter Name="Body" Type="Decimal" />
                                                        <asp:Parameter Name="Premium" Type="Decimal" />
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