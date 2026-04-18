<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PersonalAccident.aspx.vb" Inherits="PersonalAccident" %>

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
		    //var sumins = parseFloat(DataGrid.GetEditValue("SumIns"));
		    var sumins = parseFloat(DataGrid.GetEditValue("MaxForPerson")) * parseFloat(DataGrid.GetEditValue("EmpCnt"));
		    //var cnt = DataGrid.GetEditValue("EmpCnt");
		    DataGrid.SetEditValue("SumIns", sumins);
		    DataGrid.SetEditValue("Premium", (rate / 1000) * sumins);
		}
    </script>

    <style type="text/css">
        .auto-style2 {
            text-align: left;
        }
        .auto-style3 {
            text-align: right;
            height: 18px;
        }
        .auto-style4 {
            text-align: left;
            height: 18px;
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
                                            <td class="auto-style3">&nbsp;</td>
                                            <td class="auto-style4">المستفيد :</td>
                                            <td class="auto-style3" colspan="2">
                                                <dx:ASPxTextBox ID="beneficiary" runat="server" ClientInstanceName="beneficiary" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3"></td>
                                            <td class="auto-style4">نوع النشاط :</td>
                                            <td class="auto-style3" colspan="2">
                                                <dx:ASPxTextBox ID="BusinessType" runat="server" ClientInstanceName="BusinessType" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">&nbsp;</td>
                                            <td class="auto-style2">شروط خاصة :</td>
                                            <td class="auto-style1" colspan="2">
                                                <dx:ASPxTokenBox ID="Conds" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">&nbsp;</td>
                                            <td class="auto-style2">التحمل</td>
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
                                            <td class="dxeCaptionHARSys">
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxCheckBox ID="RateAll" runat="server" AccessibilityLabelText="" CheckState="Unchecked" CssClass="N" RightToLeft="False" Text="احتساب السعر للكل" TextAlign="Left" Width="150px">
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxTextBox ID="Rate" runat="server" Text="0" Width="55px">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <dx:ASPxGridView ID="DataGrid" ClientInstanceName="DataGrid" runat="server" 
                                                    DataSourceID="DataS"
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
                                                        <dx:GridViewDataTextColumn FieldName="Name" ReadOnly="false" VisibleIndex="3" Caption="الاسم">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataDateColumn FieldName="Dob" Caption="تاريخ الميلاد" EditFormSettings-Visible="True" PropertiesDateEdit-DisplayFormatString="yyyy/MM/dd" VisibleIndex="4">
                                                            <EditFormSettings Visible="True" />
                                                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd" />
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataTextColumn FieldName="EmpCnt" ReadOnly="false" VisibleIndex="5" Caption="عدد الأشخاص">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="IdentityID" ReadOnly="false" VisibleIndex="6" Caption="الرقم الوطني">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Occupation" ReadOnly="false" VisibleIndex="7" Caption="المهنة">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Nationality" ReadOnly="false" VisibleIndex="8" Caption="الجنسية">
                                                            <EditFormSettings Visible="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="MaxForPerson" ReadOnly="false" VisibleIndex="9" Caption="الحد الأقصى لمسؤولية الشركة للحادث الواحد"
                                                            PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SumIns" ReadOnly="true" VisibleIndex="10" Caption="حدود مسؤولية الشركة"
                                                            PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Rate" ReadOnly="false" VisibleIndex="11" Caption="السعر" PropertiesTextEdit-DisplayFormatString="n3">
                                                            <EditFormSettings Visible="true" />
                                                            <PropertiesTextEdit>
                                                                <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Premium" ReadOnly="true" VisibleIndex="12" Caption="القسط"
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
                                                    <SettingsText ConfirmDelete="سوف يتم شطب هذه الشخص، هل أنت متأكد؟" />
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
                                                    SelectCommand="SELECT OrderNo,EndNo,SerNo, Name,Dob, IdentityID, Occupation,Nationality, EmpCnt, MaxForPerson, SumIns, Premium, Rate FROM PersonalAccident WHERE ([OrderNo] = @Order) AND ([EndNo]=@End)"
                                                    DeleteCommand="DELETE FROM [PersonalAccident] WHERE [SerNo] = @SerNo"
                                                    InsertCommand="INSERT INTO [PersonalAccident] (OrderNo, EndNo, LoadNo, SubIns, Name,Dob, IdentityID, Occupation, Nationality, EmpCnt, MaxForPerson, SumIns,Premium,Rate)
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@Name ,@Dob, @IdentityID, @Occupation, @Nationality, @EmpCnt, @MaxForPerson, @MaxForPerson*@EmpCnt,(@Rate/1000)*(@MaxForPerson*@EmpCnt),@Rate)"
                                                    UpdateCommand="UPDATE [PersonalAccident] SET [Name] = @Name,[Dob] = @Dob, [IdentityID] = @IdentityID , [Occupation] = @Occupation, [Nationality]=@Nationality,
                                                       [EmpCnt] = @EmpCnt, [MaxForPerson] = @MaxForPerson, [SumIns] = @MaxForPerson*@EmpCnt, [Premium] = (@Rate/1000)*(@MaxForPerson*@EmpCnt), [Rate] = @Rate WHERE [SerNo] = @SerNo">
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
                                                        <asp:Parameter Name="Name" Type="String" />
                                                        <asp:Parameter Name="Dob" Type="DateTime" />
                                                        <asp:Parameter Name="EmpCnt" Type="Int32" />
                                                        <asp:Parameter Name="IdentityID" Type="String" />
                                                        <asp:Parameter Name="Occupation" Type="String" />
                                                        <asp:Parameter Name="Nationality" Type="String" />
                                                        <asp:Parameter Name="MaxForPerson" Type="Decimal" />
                                                        <asp:Parameter Name="SumIns" Type="Decimal" />
                                                        <asp:Parameter Name="Rate" Type="Decimal" />
                                                    </InsertParameters>
                                                    <UpdateParameters>
                                                        <asp:Parameter Name="Name" Type="String" />
                                                        <asp:Parameter Name="Dob" Type="DateTime" />
                                                        <asp:Parameter Name="EmpCnt" Type="Int32" />
                                                        <asp:Parameter Name="IdentityID" Type="String" />
                                                        <asp:Parameter Name="Occupation" Type="String" />
                                                        <asp:Parameter Name="Nationality" Type="String" />
                                                        <asp:Parameter Name="MaxForPerson" Type="Decimal" />
                                                        <asp:Parameter Name="SumIns" Type="Decimal" />
                                                        <asp:Parameter Name="Premium" Type="Decimal" />
                                                        <asp:Parameter Name="Rate" Type="Decimal" />
                                                        <asp:Parameter Name="SerNo" Type="Int32" />
                                                    </UpdateParameters>
                                                </asp:SqlDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
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