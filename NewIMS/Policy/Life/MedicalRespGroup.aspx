<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MedicalRespGroup.aspx.vb" Inherits="MedicalRespGroup" %>
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
            DataGrid.SetEditValue("Premium", e.result);
		}

        function OnChange(s, e) {
            //console.log("qtyprice");
            if (s.GetValue() == null) {
                s.SetValue(0);
            }
            //alert(s.GetValue());
            scbp.PerformCallback('SpCHANGE' + "|" + s.GetValue());
        }
        function END_OnEndCallback(s, e) {
            //SumIns.SetValue(e.result);
            DataGrid.SetEditValue("Premium", e.result);
            //xtraBroker.OnEndCallback(s);
            //e.result
        }
    </script>

    <style type="text/css">
        .auto-style3 {
            text-align: right;
            height: 17px;
        }
        .auto-style4 {
            text-align: right;
            height: 49px;
        }
        .auto-style5 {
            text-align: left;
            height: 49px;
        }
    </style>

</head>
<body>
       <form id="form1" runat="server">
           <div class="dx-ac">
               <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                   <ClientSideEvents BeginCallback="" EndCallback="OnEndCallback"  />
                   <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="0" />
                   <PanelCollection>
                       <dx:PanelContent runat="server">
                           <table id="TableForm" border="0" dir="rtl" style="width:100%;">
                               <tr>
                                   <td >
                                       <uc1:PolicyControl runat="server" id="PolicyControl" />
                                   </td>
                               </tr>
                               <tr>
                                   <td class="dx-al">
                                
                                       <table dir="rtl" style="width: 100%;">
                                           <tr>
                                               <td class="auto-style5">
                                                  
                                                   حدود المسئولية :</td>
                                               <td class="auto-style4">
                                                   <dx:ASPxTextBox ID="RespLimit" runat="server" ClientInstanceName="RespLinit" CssClass="1" Text="/" Width="100%">
                                                       <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                           <RegularExpression ErrorText="*" ValidationExpression="^[A-Za-z0-9ا/%,-يءئ \\-]+" />
                                                           <RequiredField IsRequired="True" />
                                                       </ValidationSettings>
                                                   </dx:ASPxTextBox>
                                               </td>
                                           </tr>
                                               <tr>
                                                   <td class="auto-style4">
                                                       <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ">
                                                           <ClientSideEvents Click="Validate" />
                                                       </dx:ASPxButton>
                                                   </td>
                                                   <td class="auto-style4">
                                                       <dx:ASPxTextBox ID="Rate" runat="server" Text="0" Width="55px">
                                                       </dx:ASPxTextBox>
                                                       <dx:ASPxCheckBox ID="RateAll" runat="server" CheckState="Unchecked" CssClass="N" RightToLeft="False" Text="احتساب السعر للكل" TextAlign="Left" Width="150px">
                                                       </dx:ASPxCheckBox>
                                                   </td>
                                           </tr>
                                               <dx:ASPxCallback ID="ASPxCallbackPanel1" ClientInstanceName="scbp" OnCallback="ASPxCallbackPanel1_Callback" runat="server">
                                                <ClientSideEvents CallbackComplete="END_OnEndCallback" />
                                            </dx:ASPxCallback>
                                           <tr>
                                               <td colspan="2">
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
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="EndNo" ReadOnly="True" VisibleIndex="2" Visible="false">
                                                               <EditFormSettings Visible="False" />
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="Name" ReadOnly="false" VisibleIndex="3" CellStyle-HorizontalAlign="Right" Caption="الاسم">
                                                               <EditFormSettings Visible="true" />

                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="WorkPlace" ReadOnly="false" VisibleIndex="4" CellStyle-HorizontalAlign="Right" Caption="مكان العمل">
                                                               <EditFormSettings Visible="true" />

                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="Job" ReadOnly="false" VisibleIndex="5" CellStyle-HorizontalAlign="Right" Caption="التخصص">
                                                               <EditFormSettings Visible="true" />

                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <%-- <dx:GridViewDataTextColumn FieldName="Specialize" ReadOnly="false" VisibleIndex="6" Caption="التصنيف">
                                                               <EditFormSettings Visible="true" />
                                                              TextChanged="onTextChanged"
                                                              EndCallback="onEndCallback"
                                                           </dx:GridViewDataTextColumn>--%>
                                                           <dx:GridViewDataComboBoxColumn FieldName="Specialize" VisibleIndex="6" CellStyle-HorizontalAlign="Right" Caption="التصنيف">
                                                               <PropertiesComboBox DataSourceID="SqlDataSource3" ValueType="System.Int32" DropDownStyle="DropDownList" TextField="TpName" ValueField="TpNo">
                                                                   <ClientSideEvents EndCallback="function(s, e) { OnChange(s, e); }" LostFocus="function(s, e) { OnChange(s, e); }" SelectedIndexChanged="function(s, e) { OnChange(s, e); }" BeginCallback="function(s, e) { OnChange(s, e); }" />
                                                               </PropertiesComboBox>
                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
                                                           </dx:GridViewDataComboBoxColumn>
                                                           <dx:GridViewDataTextColumn FieldName="IdNo" ReadOnly="false" VisibleIndex="7" CellStyle-HorizontalAlign="Right" Caption="رقمه">
                                                               <EditFormSettings Visible="true" />

                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="LicNo" ReadOnly="false" VisibleIndex="8" CellStyle-HorizontalAlign="Right" Caption="رقم الترخيص">
                                                               <EditFormSettings Visible="true" />

                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="PermNo" ReadOnly="false" VisibleIndex="9" CellStyle-HorizontalAlign="Right" Caption="إذن المزاولة">
                                                               <EditFormSettings Visible="true" />

                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="CommerceNo" ReadOnly="false" VisibleIndex="10" CellStyle-HorizontalAlign="Right" Caption="السجل التجاري">
                                                               <EditFormSettings Visible="true" />

                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataTextColumn FieldName="Premium" ReadOnly="true" VisibleIndex="11" CellStyle-HorizontalAlign="Right" Caption="القسط"
                                                               PropertiesTextEdit-DisplayFormatString="n3">
                                                               <PropertiesTextEdit DisplayFormatString="n3"></PropertiesTextEdit>
                                                               <EditFormSettings Visible="true" />

                                                               <CellStyle HorizontalAlign="Right"></CellStyle>
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
                                                       <SettingsText ConfirmDelete="سوف يتم شطب هذا العنصر، هل أنت متأكد؟" />
                                                       <EditFormLayoutProperties>
                                                           <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
                                                       </EditFormLayoutProperties>
                                                       <TotalSummary>
                                                           <dx:ASPxSummaryItem SummaryType="Sum" ShowInColumn="Premium" Tag="Prm" FieldName="Premium" ShowInGroupFooterColumn="الاشتراك" DisplayFormat="n3" ValueDisplayFormat="n3"></dx:ASPxSummaryItem>

                                                       </TotalSummary>
                                                       <SettingsPopup>
                                                           <EditForm Width="600">
                                                               <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="768" />
                                                           </EditForm>
                                                           <FilterControl AutoUpdatePosition="False"></FilterControl>
                                                       </SettingsPopup>
                                                   </dx:ASPxGridView>
                                                   <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>' 
                                                       SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO WHERE (TP = 'Specialize') AND TPNo<>44"></asp:SqlDataSource>

                                                   <asp:SqlDataSource  ID="DataS" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                                       SelectCommand="SELECT  SerNo, OrderNo, EndNo, LoadNo, GroupNo, SubIns, Name, WorkPlace, Job, Specialize,IdNo,LicNo,PermNo,CommerceNo, Premium FROM Medical_ResponspilityGroup WHERE ([OrderNo] = @Order) AND ([EndNo]=@End)"
                                                       DeleteCommand="DELETE FROM Medical_ResponspilityGroup WHERE [SerNo] = @SerNo"
                                                       InsertCommand="INSERT INTO [Medical_ResponspilityGroup] ([OrderNo],[EndNo],[LoadNo],[SubIns],[Name],[Job],[WorkPlace],[Specialize] ,[IdNo],[LicNo],[PermNo],[CommerceNo],[Premium])
                                                       VALUES (@Order ,@End ,[dbo].[GetLoadNo](@Order) , @System ,@Name ,@Job ,@WorkPlace,@Specialize, @IdNo,@LicNo,@PermNo,@CommerceNo,dbo.GetSpPremium(@Specialize))"
                                                      UpdateCommand="UPDATE [Medical_ResponspilityGroup] SET [Name] =@Name ,[Job] =@Job,[WorkPlace]=@WorkPlace ,[Specialize] = @Specialize,[IdNo]=@IdNo,[LicNo]=@LicNo,[PermNo]=@PermNo,[CommerceNo]=@CommerceNo,[Premium]=dbo.GetSpPremium(@Specialize) 
                                                       WHERE [SerNo] = @SerNo"
                                                       >
                                                       <SelectParameters>
                                                           <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                           <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                       </SelectParameters>
                                                    
                                                       <InsertParameters>
                                                           <asp:SessionParameter SessionField="Order" DefaultValue="0" Name="Order" Type="String"></asp:SessionParameter>
                                                           <asp:QueryStringParameter Name="System" DbType = "String" Direction = "Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                                                           <asp:SessionParameter SessionField="End" DefaultValue="0" Name="End" Type="String"></asp:SessionParameter>
                                                           <asp:Parameter Name="Name" Type="String" />
                                                           <asp:Parameter Name="Job" Type="String" />
                                                           <asp:Parameter Name="WorkPlace" Type="String" />
                                                           <asp:Parameter Name="Specialize" Type="Int16" />
                                                           <asp:Parameter Name="IdNo" Type="String" />
                                                           <asp:Parameter Name="LicNo" Type="String" />
                                                           <asp:Parameter Name="PermNo" Type="String" />
                                                           <asp:Parameter Name="CommerceNo" Type="String" />
                                                           <asp:Parameter Name="Premium" Type="Decimal" />
                                                       </InsertParameters>
                                                       <DeleteParameters>
                                                           <asp:Parameter Name="SerNo" Type="Int32" />
                                                       </DeleteParameters>
                                                       <UpdateParameters>
                                                           <asp:QueryStringParameter Name="System" DbType = "String" Direction = "Input" QueryStringField="Sys" DefaultValue="" ConvertEmptyStringToNull="True" />
                                                           <asp:Parameter Name="Name" Type="String" />
                                                           <asp:Parameter Name="Job" Type="String" />
                                                           <asp:Parameter Name="WorkPlace" Type="String" />
                                                           <asp:Parameter Name="Specialize" Type="String" />
                                                           <asp:Parameter Name="IdNo" Type="String" />
                                                           <asp:Parameter Name="LicNo" Type="String" />
                                                           <asp:Parameter Name="PermNo" Type="String" />
                                                           <asp:Parameter Name="CommerceNo" Type="String" />
                                                       </UpdateParameters>
                                                   </asp:SqlDataSource>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td colspan="2">
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
