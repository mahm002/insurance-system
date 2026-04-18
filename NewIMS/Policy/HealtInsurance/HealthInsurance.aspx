<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HealthInsurance.aspx.vb" Inherits=".HealthInsurance" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>

<uc1:PolicyControl runat="server" ID="PolicyControl" />
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
		    DataGrid.SetEditValue("Premium", (rate / 100) * sumins);
		}
		function Dg_OnCustomButtonClick(s, e) {
		    //let result = "";
		    switch (e.buttonID) {
		        //case "EditBnt":
		        //    //let edr = confirm("سوف يتم تعديل هذه المركبة، هل أنت متأكد؟", "Confirm delete");
		        //    //edr.then((dialogResult) => {
		        //        //if (dialogResult) {
		        //            s.StartEditRow(e.visibleIndex);
		        //        //}
		        //    //});
		           
		        case "NewBtn":
		            s.AddNewRow()
		        //case "ExpandBtn":
		        //  cbp.PerformCallback('ExpandCover');
	
		    }
		}

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                   <ClientSideEvents BeginCallback="" EndCallback="OnEndCallback"  />
                   <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="100" />
                   <PanelCollection>
                       <dx:PanelContent runat="server">

                           <table id="TableForm" style="width: 100%;" dir="rtl">
                               <tr>
                                   <td>
                                       <uc1:PolicyControl runat="server" ID="PolicyControl1" />
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
                                                   <dx:ASPxCheckBox ID="RateAll" runat="server" AccessibilityLabelText="" CheckState="Unchecked" CssClass="N" RightToLeft="False" Text="احتساب السعر للكل" TextAlign="Left" Width="150px">
                                                   </dx:ASPxCheckBox>
                                               </td>
                                               <td class="auto-style1">
                                                   <dx:ASPxTextBox ID="Rate" runat="server" CssClass="4" Text="0" Width="100px">
                                                   </dx:ASPxTextBox>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td class="auto-style2">التحمل</td>
                                               <td class="auto-style1">
                                                   <dx:ASPxTextBox ID="RespVal" runat="server" ClientInstanceName="RespVal" CssClass="1" Text="/" Width="100%">
                                                       <ClientSideEvents ValueChanged="Validate" />
                                                       <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                           <RequiredField IsRequired="True" />
                                                       </ValidationSettings>
                                                   </dx:ASPxTextBox>
                                               </td>
                                               <td class="auto-style1">&nbsp;</td>
                                           </tr>
                                           <tr>
                                               <td class="auto-style2">حدود مسؤولية للطرف الثالث</td>
                                               <td class="auto-style1">
                                                   <dx:ASPxTextBox ID="ThirdPart" runat="server" ClientInstanceName="ThirdPart" CssClass="2" Text="0" Width="109px">
                                                       <ClientSideEvents LostFocus="LocalExRateCall" TextChanged="Validate" />
                                                       <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                           <RegularExpression ErrorText="أرقام فقط" />
                                                           <RequiredField IsRequired="True" />
                                                       </ValidationSettings>
                                                   </dx:ASPxTextBox>
                                               </td>
                                               <td class="auto-style1">&nbsp;</td>
                                           </tr>
                                           <tr>
                                               <td class="auto-style2">حدود مسؤولية الحوادث الشخصية</td>
                                               <td class="auto-style1">
                                                   <dx:ASPxTextBox ID="PerAcc" runat="server" ClientInstanceName="PerAcc" CssClass="2" Text="0" Width="109px">
                                                       <ClientSideEvents LostFocus="LocalExRateCall" TextChanged="Validate" />
                                                       <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                           <RegularExpression ErrorText="أرقام فقط" />
                                                           <RequiredField IsRequired="True" />
                                                       </ValidationSettings>
                                                   </dx:ASPxTextBox>
                                               </td>
                                               <td class="auto-style1">&nbsp;</td>
                                           </tr>
                                           <tr>
                                               <td class="auto-style2">شروط خاصة</td>
                                               <td class="auto-style1" colspan="2">
                                                   <dx:ASPxTokenBox ID="Notes" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                       <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                       <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                       </ValidationSettings>
                                                   </dx:ASPxTokenBox>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td>
                                                   <dx:ASPxCheckBox ID="Herican" runat="server" AccessibilityLabelText="" CheckState="Unchecked" RightToLeft="True" Text="أخطار الزلازل والبراكين">
                                                       <ValidationSettings ErrorTextPosition="Left">
                                                       </ValidationSettings>
                                                   </dx:ASPxCheckBox>
                                               </td>
                                               <td class="auto-style1">
                                                   <dx:ASPxCheckBox ID="Shageb" runat="server" AccessibilityLabelText="" CheckState="Unchecked" RightToLeft="True" Text="الشغب والإضطرابات الأهلية">
                                                       <ValidationSettings ErrorTextPosition="Left">
                                                       </ValidationSettings>
                                                   </dx:ASPxCheckBox>
                                               </td>
                                               <td class="auto-style1">&nbsp;</td>
                                           </tr>
                                           <tr>
                                               <td colspan="3">
                                                   <dx:ASPxGridView ID="DataGrid" ClientInstanceName="DataGrid" runat="server" DataSourceID="DataS" RightToLeft="True" CellStyle-HorizontalAlign="Right"
                                                       OnDataBinding="DataGrid_DataBinding"
                                                       OnDataBound="DataGrid_DataBound"
                                                       OnHtmlDataCellPrepared="DataGrid_HtmlDataCellPrepared"
                                                       KeyFieldName="SerNo" Width="100%"
                                                       EnableRowsCache="False" AutoGenerateColumns="False">
                                                       <%-- <ClientSideEvents CustomButtonClick="Dg_OnCustomButtonClick"/>--%>
                                                       <Columns>
                                                           <%-- <dx:GridViewCommandColumn id ShowEditButton="true" />--%>
                                                           <dx:GridViewDataTextColumn FieldName="SerNo" ReadOnly="True" VisibleIndex="0" Visible="false">
                                                               <EditFormSettings Visible="False" />
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="OrderNo" ReadOnly="True" VisibleIndex="1" Visible="false">
                                                               <EditFormSettings Visible="False" />
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="EndNo" ReadOnly="True" VisibleIndex="2" Visible="true" Caption="رقم الملحق">
                                                               <EditFormSettings Visible="False" />
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="CarType" ReadOnly="false" VisibleIndex="3" Caption="نوع السيارة">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="TableNo" ReadOnly="false" VisibleIndex="4" Caption="رقم اللوحة">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="BudyNo" ReadOnly="false" VisibleIndex="5" Caption="رقم الهيكل">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="PermType" ReadOnly="false" VisibleIndex="6" Caption="غرض الاستعمال">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="PassNo" ReadOnly="false" VisibleIndex="7" Caption="عدد الركاب">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="Power" ReadOnly="false" VisibleIndex="8" Caption="القوة">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="CarColor" ReadOnly="false" VisibleIndex="9" Caption="اللون">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="MadeYear" ReadOnly="false" VisibleIndex="10" Caption="سنة الصنع">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="SumIns" ReadOnly="false" VisibleIndex="11" Caption="القيمة"
                                                               PropertiesTextEdit-DisplayFormatString="n3">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                               <PropertiesTextEdit>
                                                                   <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                               </PropertiesTextEdit>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="Rate" ReadOnly="false" VisibleIndex="12" Caption="السعر" PropertiesTextEdit-DisplayFormatString="n3">
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                               <PropertiesTextEdit>
                                                                   <ClientSideEvents ValueChanged="OnEditorValueChanged" />
                                                               </PropertiesTextEdit>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="Premium" ReadOnly="true" VisibleIndex="13" Caption="القسط"
                                                               PropertiesTextEdit-DisplayFormatString="n3">
                                                               <PropertiesTextEdit DisplayFormatString="n3"></PropertiesTextEdit>
                                                               <EditFormSettings Visible="true" />
                                                               <CellStyle HorizontalAlign="Right">
                                                               </CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewCommandColumn Name="Commands"
                                                               ShowDeleteButton="true"
                                                               ShowEditButton="true"
                                                               ShowNewButton="true"
                                                               ShowNewButtonInHeader="True"
                                                               ShowInCustomizationForm="True"
                                                               VisibleIndex="0">
                                                               <CustomButtons>
                                                                   <dx:GridViewCommandColumnCustomButton ID="DelButton" Text=" م شطب سيارة " Styles-FocusRectStyle-Font-Underline="false">
                                                                       <Styles>
                                                                           <FocusRectStyle Font-Underline="False"></FocusRectStyle>
                                                                       </Styles>
                                                                   </dx:GridViewCommandColumnCustomButton>

                                                                   <dx:GridViewCommandColumnCustomButton ID="ExpandBtn" Text=" ملحق توسيع رقعة " Styles-FocusRectStyle-Font-Underline="false">
                                                                       <Styles>
                                                                           <FocusRectStyle Font-Underline="False"></FocusRectStyle>
                                                                       </Styles>
                                                                   </dx:GridViewCommandColumnCustomButton>
                                                               </CustomButtons>
                                                               <%-- <CustomButtons>
                                                                  <dx:GridViewCommandColumnCustomButton ID="NewBtn" Text="إضافة">
                                                                   </dx:GridViewCommandColumnCustomButton>
                                                               </CustomButtons>--%>
                                                           </dx:GridViewCommandColumn>

                                                       </Columns>

                                                       <SettingsCommandButton>
                                                           <NewButton Text="إضافة">
                                                           </NewButton>
                                                           <UpdateButton Text="حفظ">
                                                           </UpdateButton>
                                                           <CancelButton Text="تراجع">
                                                           </CancelButton>
                                                           <EditButton Text="تعديل">
                                                           </EditButton>
                                                           <DeleteButton Text="شطب">
                                                           </DeleteButton>
                                                       </SettingsCommandButton>
                                                       <SettingsEditing Mode="EditForm" NewItemRowPosition="Top"></SettingsEditing>
                                                       <Settings ShowFooter="True"></Settings>
                                                       <SettingsSearchPanel Visible="True" />
                                                       <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                                       <SettingsText ConfirmDelete="سوف يتم شطب هذه المركبة، هل أنت متأكد؟" />
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
                                                       SelectCommand="SELECT OrderNo,EndNo,SerNo, UPPER(CarType) As CarType, TableNo, BudyNo,PermType, PassNo, Power, CarColor, MadeYear, SumIns, Premium,
                                                       Notes, Rate FROM MOMOTORFILE WHERE ([OrderNo] = @Order) AND ([EndNo]=@End)"
                                                       DeleteCommand="DELETE FROM [MOMOTORFILE] WHERE [SerNo] = @SerNo"
                                                       InsertCommand="INSERT INTO [MOMOTORFILE] (OrderNo, EndNo, LoadNo,SubIns, CarType,TableNo, BudyNo, PermType, PassNo, Power, CarColor,MadeYear,SumIns,Premium,Rate) 
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@CarType ,@TableNo, @BudyNo, @PermType,  @PassNo, @Power, @CarColor, @MadeYear,@SumIns,(@Rate/100)*@SumIns,@Rate)"
                                                       UpdateCommand="UPDATE [MOMOTORFILE] SET [CarType] = @CarType, [TableNo] = @TableNo, [BudyNo] = @BudyNo , [PermType] = @PermType , [PassNo] = @PassNo, [Power] = @Power, [CarColor] = @CarColor, [MadeYear] = @MadeYear, 
                                                       [SumIns] = @SumIns, [Premium] = (@Rate/100)*@SumIns, [Rate] = @Rate WHERE [SerNo] = @SerNo">
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
                                                           <asp:Parameter Name="CarType" Type="String" />
                                                           <asp:Parameter Name="TableNo" Type="String" />
                                                           <asp:Parameter Name="BudyNo" Type="String" />
                                                           <asp:Parameter Name="PermType" Type="String" />
                                                           <asp:Parameter Name="PassNo" Type="Int16" />
                                                           <asp:Parameter Name="Power" Type="Int16" />
                                                           <asp:Parameter Name="CarColor" Type="String" />
                                                           <asp:Parameter Name="MadeYear" Type="Int16" />
                                                           <asp:Parameter Name="SumIns" Type="Decimal" />
                                                           <asp:Parameter Name="Rate" Type="Decimal" />
                                                       </InsertParameters>
                                                       <UpdateParameters>
                                                           <asp:Parameter Name="CarType" Type="String" />
                                                           <asp:Parameter Name="TableNo" Type="String" />
                                                           <asp:Parameter Name="BudyNo" Type="String" />
                                                           <asp:Parameter Name="PermType" Type="String" />
                                                           <asp:Parameter Name="PassNo" Type="Int16" />
                                                           <asp:Parameter Name="Power" Type="Int16" />
                                                           <asp:Parameter Name="CarColor" Type="String" />
                                                           <asp:Parameter Name="MadeYear" Type="Int16" />
                                                           <asp:Parameter Name="SumIns" Type="Decimal" />
                                                           <asp:Parameter Name="Premium" Type="Decimal" />
                                                           <asp:Parameter Name="Rate" Type="Decimal" />
                                                           <asp:Parameter Name="SerNo" Type="Int32" />
                                                       </UpdateParameters>
                                                   </asp:SqlDataSource>
                                                   <asp:SqlDataSource ID="DataS1" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                                       SelectCommand="SELECT MOMOTORFILE.OrderNo,MOMOTORFILE.EndNo,SerNo, UPPER(CarType) As CarType, TableNo, BudyNo,PermType, PassNo, Power, CarColor, MadeYear, SumIns, Premium, Notes, Rate
                                                                    FROM PolicyFile left join MOMOTORFILE on PolicyFile.OrderNo=MOMOTORFILE.OrderNo WHERE PolNo=@Pol and Zone='ليبيا'"
                                                       InsertCommand="INSERT INTO [MOMOTORFILE] (OrderNo, EndNo, LoadNo,SubIns, CarType,TableNo, BudyNo, PermType, PassNo, Power, CarColor,MadeYear,SumIns,Premium,Rate) 
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@CarType ,@TableNo, @BudyNo, @PermType,  @PassNo, @Power, @CarColor, @MadeYear,@SumIns,(@Rate/100)*@SumIns,@Rate)"
                                                       DeleteCommand="INSERT INTO [MOMOTORFILE] (OrderNo, EndNo, LoadNo,SubIns, CarType,TableNo, BudyNo, PermType, PassNo, Power, CarColor,MadeYear,SumIns,Premium,Rate) 
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@CarType ,@TableNo, @BudyNo, @PermType,  @PassNo, @Power, @CarColor, @MadeYear,@SumIns,(@Rate/100)*@SumIns,@Rate)*-1"
                                                       UpdateCommand="UPDATE [MOMOTORFILE] SET [CarType] = @CarType, [TableNo] = @TableNo, [BudyNo] = @BudyNo , [PermType] = @PermType , [PassNo] = @PassNo, [Power] = @Power, [CarColor] = @CarColor, [MadeYear] = @MadeYear, 
                                                       [SumIns] = @SumIns, [Premium] = (@Rate/100)*@SumIns, [Rate] = @Rate WHERE [SerNo] = @SerNo And OrderNo=@Order">
                                                       <SelectParameters>
                                                           <asp:SessionParameter SessionField="Pol" DefaultValue="0" Name="Pol" Type="String"></asp:SessionParameter>
                                                       </SelectParameters>
                                                       <DeleteParameters>
                                                           <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                           <asp:QueryStringParameter Name="System" DbType="String" Direction="Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                                                           <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                           <asp:Parameter Name="CarType" Type="String" />
                                                           <asp:Parameter Name="TableNo" Type="String" />
                                                           <asp:Parameter Name="BudyNo" Type="String" />
                                                           <asp:Parameter Name="PermType" Type="String" />
                                                           <asp:Parameter Name="PassNo" Type="Int16" />
                                                           <asp:Parameter Name="Power" Type="Int16" />
                                                           <asp:Parameter Name="CarColor" Type="String" />
                                                           <asp:Parameter Name="MadeYear" Type="Int16" />
                                                           <asp:Parameter Name="SumIns" Type="Decimal" />
                                                           <asp:Parameter Name="Rate" Type="Decimal" />
                                                       </DeleteParameters>
                                                       <InsertParameters>
                                                           <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                           <asp:QueryStringParameter Name="System" DbType="String" Direction="Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                                                           <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                           <asp:Parameter Name="CarType" Type="String" />
                                                           <asp:Parameter Name="TableNo" Type="String" />
                                                           <asp:Parameter Name="BudyNo" Type="String" />
                                                           <asp:Parameter Name="PermType" Type="String" />
                                                           <asp:Parameter Name="PassNo" Type="Int16" />
                                                           <asp:Parameter Name="Power" Type="Int16" />
                                                           <asp:Parameter Name="CarColor" Type="String" />
                                                           <asp:Parameter Name="MadeYear" Type="Int16" />
                                                           <asp:Parameter Name="SumIns" Type="Decimal" />
                                                           <asp:Parameter Name="Rate" Type="Decimal" />
                                                       </InsertParameters>
                                                       <UpdateParameters>
                                                           <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                           <asp:Parameter Name="CarType" Type="String" />
                                                           <asp:Parameter Name="TableNo" Type="String" />
                                                           <asp:Parameter Name="BudyNo" Type="String" />
                                                           <asp:Parameter Name="PermType" Type="String" />
                                                           <asp:Parameter Name="PassNo" Type="Int16" />
                                                           <asp:Parameter Name="Power" Type="Int16" />
                                                           <asp:Parameter Name="CarColor" Type="String" />
                                                           <asp:Parameter Name="MadeYear" Type="Int16" />
                                                           <asp:Parameter Name="SumIns" Type="Decimal" />
                                                           <asp:Parameter Name="Premium" Type="Decimal" />
                                                           <asp:Parameter Name="Rate" Type="Decimal" />
                                                           <asp:Parameter Name="SerNo" Type="Int32" />
                                                       </UpdateParameters>
                                                   </asp:SqlDataSource>
                                                   <asp:SqlDataSource ID="DataS2" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                                       SelectCommand="SELECT MOMOTORFILE.OrderNo,MOMOTORFILE.EndNo,SerNo, UPPER(CarType) As CarType, TableNo, BudyNo,PermType, PassNo, Power, CarColor, MadeYear, SumIns, Premium, Notes, Rate
                                                                    FROM PolicyFile left join MOMOTORFILE on PolicyFile.OrderNo=MOMOTORFILE.OrderNo WHERE PolNo=@Pol and Zone='ليبيا'"
                                                       InsertCommand="INSERT INTO [MOMOTORFILE] (OrderNo, EndNo, LoadNo,SubIns, CarType,TableNo, BudyNo, PermType, PassNo, Power, CarColor,MadeYear,SumIns,Premium,Rate) 
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@CarType ,@TableNo, @BudyNo, @PermType,  @PassNo, @Power, @CarColor, @MadeYear,@SumIns,(@Rate/100)*@SumIns,@Rate)"
                                                       DeleteCommand="INSERT INTO [MOMOTORFILE] (OrderNo, EndNo, LoadNo,SubIns, CarType,TableNo, BudyNo, PermType, PassNo, Power, CarColor,MadeYear,SumIns,Premium,Rate) 
                                                       VALUES (@Order,@End,[dbo].[GetLoadNo](@Order),@System ,@CarType ,@TableNo, @BudyNo, @PermType,  @PassNo, @Power, @CarColor, @MadeYear,@SumIns,(@Rate/100)*@SumIns,@Rate)*-1"
                                                       UpdateCommand="UPDATE [MOMOTORFILE] SET [CarType] = @CarType, [TableNo] = @TableNo, [BudyNo] = @BudyNo , [PermType] = @PermType , [PassNo] = @PassNo, [Power] = @Power, [CarColor] = @CarColor, [MadeYear] = @MadeYear, 
                                                       [SumIns] = @SumIns, [Premium] = (@Rate/100)*@SumIns, [Rate] = @Rate WHERE [SerNo] = @SerNo And OrderNo=@Order">
                                                       <SelectParameters>
                                                           <asp:SessionParameter SessionField="Pol" DefaultValue="0" Name="Pol" Type="String"></asp:SessionParameter>
                                                       </SelectParameters>
                                                       <DeleteParameters>
                                                           <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                           <asp:QueryStringParameter Name="System" DbType="String" Direction="Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                                                           <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                           <asp:Parameter Name="CarType" Type="String" />
                                                           <asp:Parameter Name="TableNo" Type="String" />
                                                           <asp:Parameter Name="BudyNo" Type="String" />
                                                           <asp:Parameter Name="PermType" Type="String" />
                                                           <asp:Parameter Name="PassNo" Type="Int16" />
                                                           <asp:Parameter Name="Power" Type="Int16" />
                                                           <asp:Parameter Name="CarColor" Type="String" />
                                                           <asp:Parameter Name="MadeYear" Type="Int16" />
                                                           <asp:Parameter Name="SumIns" Type="Decimal" />
                                                           <asp:Parameter Name="Rate" Type="Decimal" />
                                                       </DeleteParameters>
                                                       <InsertParameters>
                                                           <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                           <asp:QueryStringParameter Name="System" DbType="String" Direction="Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                                                           <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                           <asp:Parameter Name="CarType" Type="String" />
                                                           <asp:Parameter Name="TableNo" Type="String" />
                                                           <asp:Parameter Name="BudyNo" Type="String" />
                                                           <asp:Parameter Name="PermType" Type="String" />
                                                           <asp:Parameter Name="PassNo" Type="Int16" />
                                                           <asp:Parameter Name="Power" Type="Int16" />
                                                           <asp:Parameter Name="CarColor" Type="String" />
                                                           <asp:Parameter Name="MadeYear" Type="Int16" />
                                                           <asp:Parameter Name="SumIns" Type="Decimal" />
                                                           <asp:Parameter Name="Rate" Type="Decimal" />
                                                       </InsertParameters>
                                                       <UpdateParameters>
                                                           <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                           <asp:Parameter Name="CarType" Type="String" />
                                                           <asp:Parameter Name="TableNo" Type="String" />
                                                           <asp:Parameter Name="BudyNo" Type="String" />
                                                           <asp:Parameter Name="PermType" Type="String" />
                                                           <asp:Parameter Name="PassNo" Type="Int16" />
                                                           <asp:Parameter Name="Power" Type="Int16" />
                                                           <asp:Parameter Name="CarColor" Type="String" />
                                                           <asp:Parameter Name="MadeYear" Type="Int16" />
                                                           <asp:Parameter Name="SumIns" Type="Decimal" />
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
