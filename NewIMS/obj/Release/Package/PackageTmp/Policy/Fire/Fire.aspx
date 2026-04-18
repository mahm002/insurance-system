<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Fire.aspx.vb" Inherits="Fire" %>

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
            var Brate = parseFloat(DataGrid.GetEditValue("BuildRate")/100);
            var CRate = parseFloat(DataGrid.GetEditValue("ContRate")/100);
            var BuildIns = parseFloat(DataGrid.GetEditValue("BuildIns")) + parseFloat(DataGrid.GetEditValue("ManBuildIns"));
            var ContIns = parseFloat(DataGrid.GetEditValue("BuildCont")) + parseFloat(DataGrid.GetEditValue("ManBuildCont"));
            var OIns = parseFloat(DataGrid.GetEditValue("Owner") + DataGrid.GetEditValue("Behind")) //+ parseFloat(DataGrid.GetEditValue("Behind"));
            //alert(BuildIns);
            //alert(ContIns);

            DataGrid.SetEditValue("Premium", ((Brate) * (BuildIns)) + ((CRate) * (ContIns)) + ((Brate) * (OIns)));
        }
    </script>
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
                                            <td class="dx-al">&nbsp;</td>
                                            <td class="auto-style1">
                                                <%--      <dx:ASPxCheckBox ID="RateAll" runat="server" AccessibilityLabelText="" CheckState="Unchecked" CssClass="N" RightToLeft="False" Text="احتساب السعر للكل" TextAlign="Left" Width="150px">
                                                   </dx:ASPxCheckBox>--%>
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style1">
                                                <%--   <dx:ASPxTextBox ID="Rate" runat="server" Text="0" Width="55px" CssClass="5">
                                                   </dx:ASPxTextBox>--%>
                                                   &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">الأخطار الإضافية</td>
                                            <td class="auto-style1" colspan="2">
                                                <dx:ASPxTokenBox ID="Conds" runat="server" AllowMouseWheel="True" CallbackPageSize="10" DataSourceID="FireConds" RightToLeft="True" TextField="Condition" AllowCustomTokens="false" Tokens="" ValueField="CondNo" Width="100%">
                                                    <ClientSideEvents TokensChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">بند التحمل</td>
                                            <td class="auto-style1" colspan="2">
                                                <dx:ASPxTextBox ID="RespVal" runat="server" ClientInstanceName="RespVal" CssClass="1" Text="/" Width="100%">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">ملاحظات</td>
                                            <td class="auto-style1" colspan="2">
                                                <dx:ASPxTokenBox ID="Notes" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">&nbsp;</td>
                                            <td class="auto-style1" colspan="2">
                                                <dx:ASPxCheckBox ID="RahnCond" runat="server" AccessibilityLabelText="" CheckState="Unchecked" RightToLeft="True" Text="تخضع هذه الوثيقة لشرط الرهن">
                                                    <ValidationSettings ErrorTextPosition="Left">
                                                    </ValidationSettings>
                                                </dx:ASPxCheckBox>
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
                                                    <SettingsEditing Mode="EditForm" NewItemRowPosition="Top"></SettingsEditing>
                                                    <Settings ShowFooter="True"></Settings>
                                                    <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
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
                                                    <SettingsPopup>
                                                        <EditForm Width="600">
                                                            <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="768" />
                                                        </EditForm>
                                                        <FilterControl AutoUpdatePosition="False"></FilterControl>
                                                    </SettingsPopup>
                                                    <SettingsSearchPanel Visible="True" />
                                                    <SettingsText ConfirmDelete="سوف يتم شطب هذا الببند، هل أنت متأكد؟" />
                                                    <EditFormLayoutProperties>
                                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
                                                    </EditFormLayoutProperties>
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
                                                        <dx:GridViewDataTextColumn FieldName="InsItem" ReadOnly="false" VisibleIndex="3" Caption="الأعيان المؤمن عليها">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Area" ReadOnly="false" VisibleIndex="4" Caption="الموقع">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn FieldName="BuildIns" ReadOnly="false" VisibleIndex="5" Caption="قيمة المبنى">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ManBuildIns" ReadOnly="false" VisibleIndex="6" Caption="قيمة المبنى الإداري"
                                                            PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="BuildCont" ReadOnly="false" VisibleIndex="7" Caption="قيمة المحتوى">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn FieldName="ManBuildCont" ReadOnly="false" VisibleIndex="8" Caption="قيمة المحتوى للمبنى الإداري">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Behind" ShowInCustomizationForm="True" Caption="قبل الجيران " VisibleIndex="9">
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                            <EditFormSettings Visible="True"></EditFormSettings>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Owner" ReadOnly="false" VisibleIndex="10" Caption="قبل المالك" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="BuildRate" ReadOnly="false" VisibleIndex="11" Caption="سعر المباني" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="سعر المحتويات" FieldName="ContRate" ShowInCustomizationForm="True" VisibleIndex="12">
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                            <EditFormSettings Visible="True" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Premium" ReadOnly="true" VisibleIndex="13" Caption="القسط" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                                <%--<ClientSideEvents ValueChanged="OnEditorValueChanged" />--%>
                                                            </PropertiesTextEdit>
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit DisplayFormatString="n3">
                                                            </PropertiesTextEdit>
                                                            <EditFormSettings Visible="True" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewCommandColumn Name="Commands" ShowEditButton="True" ShowDeleteButton="True" ShowNewButtonInHeader="True" ShowInCustomizationForm="True" VisibleIndex="0"></dx:GridViewCommandColumn>
                                                    </Columns>
                                                    <TotalSummary>
                                                        <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="Premium" Tag="Prm" FieldName="Premium" ShowInGroupFooterColumn="القيمة" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>
                                                        <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="CashIn" FieldName="BuildIns + BuildCont + ManBuildIns + ManBuildCont" Tag="مجموع مبالغ التأمين" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>
                                                    </TotalSummary>
                                                </dx:ASPxGridView>
                                                <asp:SqlDataSource ID="FireConds" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT CondNo, Condition FROM CondFile WHERE (SubSys = 'FR')"></asp:SqlDataSource>
                                                <asp:SqlDataSource ID="DataS" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                                    SelectCommand="SELECT OrderNo,EndNo,SerNo, InsItem, Area, BuildIns, BuildCont, ManBuildIns, ManBuildCont, Behind, Owner, BuildRate, ContRate, Premium FROM FireFile WHERE ([OrderNo] = @Order) AND ([EndNo]=@End)"
                                                    DeleteCommand="DELETE FROM [FireFile] WHERE [SerNo] = @SerNo"
                                                    InsertCommand="INSERT INTO [FireFile] (OrderNo, EndNo, LoadNo, SubIns, InsItem, Area, BuildIns, BuildCont, ManBuildIns, ManBuildCont, Behind, Owner, BuildRate, ContRate, Premium)
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@InsItem, @Area, @BuildIns, @BuildCont, @ManBuildIns, @ManBuildCont, @Behind, @Owner, @BuildRate, @ContRate, ((@BuildRate/100)*(@BuildIns+@ManBuildIns))+((@ContRate/100)*(@BuildCont+@ManBuildCont)) +((@BuildRate/100)*(@Owner+@Behind)))"
                                                    UpdateCommand="UPDATE [FireFile] SET [InsItem] = @InsItem, [Area] = @Area, [BuildIns] = @BuildIns , [BuildCont] = @BuildCont, [ManBuildIns]=@ManBuildIns, [Behind]=@Behind, [Owner]=@Owner,
                                                       [ManBuildCont] = @ManBuildCont, [BuildRate] = @BuildRate, [ContRate] = @ContRate, [Premium] = (@BuildRate/100)*(@BuildIns+@ManBuildIns)+(@BuildRate/100)*(@Owner+@Behind)+(@ContRate/100)*(@BuildCont+@ManBuildCont) WHERE [SerNo] = @SerNo">
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
                                                        <asp:Parameter Name="InsItem" Type="String" />
                                                        <asp:Parameter Name="Area" Type="String" />
                                                        <asp:Parameter Name="BuildIns" Type="Decimal" />
                                                        <asp:Parameter Name="BuildCont" Type="Decimal" />
                                                        <asp:Parameter Name="ManBuildIns" Type="Decimal" />
                                                        <asp:Parameter Name="ManBuildCont" Type="Decimal" />
                                                        <asp:Parameter Name="Behind" Type="Decimal" />
                                                        <asp:Parameter Name="Owner" Type="Decimal" />
                                                        <asp:Parameter Name="BuildRate" Type="Decimal" />
                                                        <asp:Parameter Name="ContRate" Type="Decimal" />
                                                    </InsertParameters>
                                                    <UpdateParameters>
                                                        <asp:Parameter Name="InsItem" Type="String" />
                                                        <asp:Parameter Name="Area" Type="String" />
                                                        <asp:Parameter Name="BuildIns" Type="Decimal" />
                                                        <asp:Parameter Name="BuildCont" Type="Decimal" />
                                                        <asp:Parameter Name="ManBuildIns" Type="Decimal" />
                                                        <asp:Parameter Name="ManBuildCont" Type="Decimal" />
                                                        <asp:Parameter Name="Behind" Type="Decimal" />
                                                        <asp:Parameter Name="Owner" Type="Decimal" />
                                                        <asp:Parameter Name="BuildRate" Type="Decimal" />
                                                        <asp:Parameter Name="ContRate" Type="Decimal" />
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