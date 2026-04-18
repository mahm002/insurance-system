<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InwardIssue.aspx.vb" Inherits="InwardIssue" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<script src="http://code.jquery.com/jquery-latest.min.js"></script>--%>

    <script src="../../scripts/Scripts.js"></script>
    <script src="../../scripts/jquery-latest.min.js"></script>
    <script src="../../scripts/jquery-3.7.1.min.js"></script>
    <title></title>
    <script type="text/javascript">
        $('body').on('keydown', 'input, select', function (e) {
            //debugger;
            if (e.key === "Enter") {
                var self = $(this), form = self.parents('form:eq(0)'), focusable, next;
                focusable = form.find('input,a,select,button,textarea').filter(':visible:enabled');
                //focusable1 = form.find('input,a,select,button,textarea').filter(':ClientEnabled');
                next = focusable.eq(focusable.index(this) + 1);
                if (next.length) {
                    // debugger;
                    next.focus();
                    next.select();
                } else {
                    // debugger;
                    btnShow.DoClick();
                    $('input:text:first').focus();
                }
                return false;
            }
        });
        function ReturnToParentPage() {
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup(1);
        }
        function LocalAccSave(s, e) {
            ValidateEditors();
            if (ASPxClientEdit.ValidateEditorsInContainer(null)) {
                scbp.PerformCallback("SaveData");
            }

        }
        function OnChange(s, e) {
            ValidateEditors();
            if (s.GetValue() == null) {
                s.SetValue(0);
            }
            if (ASPxClientEdit.ValidateEditorsInContainer(null)) {
                scbp.PerformCallback('ShareChange');
            }
        }
        function ValidateEditors() {
            ASPxClientEdit.ValidateEditorsInContainer(null);
        }
        function OnEndCallback(s, e) {
            //alert(e.result.substring(0, 5));
            if (parseFloat(AcceptedShare.GetValue()) > 0) {
                if (e.result == null) {
                    //alert(e.result);
                    AcceptedSI.SetValue(parseFloat(AcceptedShare.GetValue()) * parseFloat(MainSI.GetValue()) / 100);
                    AcceptedNet.SetValue(parseFloat(AcceptedShare.GetValue()) * parseFloat(MainNet.GetValue()) / 100);
                    CommisionValue.SetValue(parseFloat(CommisionRatio.GetValue()) * parseFloat(AcceptedNet.GetValue()) / 100);
                    Balance.SetValue(parseFloat(AcceptedNet.GetValue()) - parseFloat(CommisionValue.GetValue()));
                }
                else {
                    if (e.result.substring(0, 5) == 'INWRD') {
                        SlipN.SetValue(e.result);
                        scbp.PerformCallback("InsertData");
                    }
                    else {
                        //alert(e.result);
                        AcceptedSI.SetValue(e.result);
                        AcceptedNet.SetValue(parseFloat(AcceptedShare.GetValue()) * parseFloat(MainNet.GetValue()) / 100);
                        CommisionValue.SetValue(parseFloat(CommisionRatio.GetValue()) * parseFloat(AcceptedNet.GetValue()) / 100);
                        Balance.SetValue(parseFloat(AcceptedNet.GetValue()) - parseFloat(CommisionValue.GetValue()));
                    }
                   
                }
            }
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <dx:ASPxRoundPanel ID="ASPxRoundPanel1" RightToLeft="True" runat="server" Width="100%" HeaderText="وثيقة وارد محلي" CssFilePath="~/App_Themes/DevEx/{0}/styles.css" CssPostfix="DevEx" GroupBoxCaptionOffsetX="6px" GroupBoxCaptionOffsetY="-19px" SpriteCssFilePath="~/App_Themes/DevEx/{0}/sprite.css">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="7px" PaddingRight="11px" PaddingTop="10px" />
                            <HeaderStyle>
                                <Paddings PaddingBottom="6px" PaddingLeft="7px" PaddingRight="11px" PaddingTop="1px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent>
                                    <br />
                                    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" CssClass="formLayout" RightToLeft="False" Theme="Office365">
                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600">
                                        </SettingsAdaptivity>
                                        <Items>
                                            <dx:LayoutGroup Caption="وثيقة وارد محلي" ColCount="4" ColSpan="1" ColumnCount="4" GroupBoxDecoration="HeadingLine" UseDefaultPaddings="true">
                                                <Paddings PaddingTop="10px" />
                                                <GroupBoxStyle>
                                                    <Caption Font-Bold="True" Font-Size="16pt" ForeColor="#006600">
                                                    </Caption>
                                                </GroupBoxStyle>
                                                <Items>
                                                    <dx:LayoutItem  ColSpan="1" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxTextBox ID="SlipN" runat="server" Caption="Reference. No" ClientInstanceName="SlipN" RightToLeft="true" Text="0" ClientEnabled="false">
                                                                    <ValidationSettings Display="Dynamic">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem ColSpan="3" HorizontalAlign="right" Caption="" ColumnSpan="3" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxTextBox ID="PolNo" runat="server" ClientEnabled="true" RightToLeft="true" Caption="رقم الوثيقة" SelectInputTextOnClick="True" Width="100%">
                                                                    <ValidationSettings Display="Dynamic">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="" ColSpan="1" HorizontalAlign="right" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxTextBox ID="EndNo" runat="server" Caption="رقم الملحق" ClientEnabled="true" ClientInstanceName="EndNo" RightToLeft="true">
                                                                    <ValidationSettings Display="Dynamic">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                                <dx:ASPxTextBox ID="LoadNo" runat="server" ClientInstanceName="LoadNo" Text="0" 
                                                                    ClientVisible="False" RightToLeft="true">
                                                                    <ValidationSettings Display="Dynamic">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                
                                                    <dx:LayoutItem Caption="" ColSpan="4" HorizontalAlign="right" ColumnSpan="4">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxDateEdit ID="EntryDate" runat="server" RightToLeft="True" Caption="تاريخ الإصدار" DisplayFormatString="yyyy/MM/dd" Width="200px">
                                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxDateEdit>
                                                                <dx:ASPxDateEdit ID="CoverFrom" runat="server" RightToLeft="True" Caption="التغطية من" DisplayFormatString="yyyy/MM/dd" Width="200px">
                                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxDateEdit>
                                                                <dx:ASPxDateEdit ID="CoverTo" runat="server" RightToLeft="True" Caption="التغطية إلى" DisplayFormatString="yyyy/MM/dd" Width="200px">
                                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxDateEdit>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="" ColSpan="4" HorizontalAlign="right" ColumnSpan="4">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                         <dx:ASPxTokenBox ID="RiskDiscription" runat="server" Caption="وصف الخطر" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                    <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                    </ValidationSettings>
                                                </dx:ASPxTokenBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="" ColSpan="4" ColumnSpan="4" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxGridLookup ID="CustNo" runat="server" AutoGenerateColumns="False" AutoResizeWithContainer="True" ClientInstanceName="CustNo" Caption="المؤمن له :" DataSourceID="Customers" DropDownStyle="DropDownList" GridViewProperties-SettingsCommandButton-ClearButton-Text="بحث جديد" IncrementalFilteringDelay="20" KeyFieldName="CustNo" PopupHorizontalAlign="Center" RenderIFrameForPopupElements="True" RightToLeft="True" TextFormatString="{1} | {0} | {2} | {3}" Width="100%">
                                                                    <GridViewProperties>
                                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                                                        <SettingsLoadingPanel Mode="Disabled" />
                                                                        <SettingsPopup>
                                                                            <FilterControl AutoUpdatePosition="False">
                                                                            </FilterControl>
                                                                        </SettingsPopup>
                                                                        <SettingsCommandButton>
                                                                            <NewButton ButtonType="Image" RenderMode="Image" Text="زبون جديد">
                                                                                <Image IconID="actions_newemployee_16x16devav">
                                                                                </Image>
                                                                            </NewButton>
                                                                            <UpdateButton ButtonType="Image" RenderMode="Image" Text="حفظ">
                                                                                <Image IconID="actions_save_16x16devav">
                                                                                </Image>
                                                                            </UpdateButton>
                                                                            <CancelButton ButtonType="Image" RenderMode="Image" Text="إلغاء">
                                                                                <Image IconID="actions_undo_16x16devav">
                                                                                </Image>
                                                                            </CancelButton>
                                                                            <EditButton ButtonType="Image" RenderMode="Image" Text="تحرير">
                                                                                <Image IconID="actions_edit_16x16devav">
                                                                                </Image>
                                                                            </EditButton>
                                                                            <SearchPanelApplyButton Text="بحث">
                                                                            </SearchPanelApplyButton>
                                                                        </SettingsCommandButton>
                                                                        <SettingsSearchPanel EditorNullTextDisplayMode="UnfocusedAndFocused" ShowApplyButton="True" ShowClearButton="True" Visible="True" />
                                                                    </GridViewProperties>
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn ShowEditButton="True" ShowInCustomizationForm="True" VisibleIndex="0" ShowNewButtonInHeader="True">
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataTextColumn Caption="رقم الزبون" FieldName="CustNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="اسم الزبون" FieldName="CustName" ShowInCustomizationForm="True" VisibleIndex="2">
                                                                            <EditFormSettings ColumnSpan="2" />
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="الاسم اللاتيني" FieldName="CustNameE" ShowInCustomizationForm="True" VisibleIndex="3">
                                                                            <PropertiesTextEdit HelpText="للاستفادة من خدمة الرسائل القصيرة">
                                                                                <MaskSettings Mask="\0\0\2\1\8\900000000" />
                                                                                <ValidationSettings Display="Dynamic">
                                                                                    <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                                                    <RequiredField IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle HorizontalAlign="Center">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="رقم النقال" FieldName="TelNo" ShowInCustomizationForm="True" VisibleIndex="4">
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="العنوان" FieldName="Address" ShowInCustomizationForm="True" VisibleIndex="5">
                                                                            <PropertiesTextEdit ClientInstanceName="CustInputName">
                                                                                <ValidationSettings Display="Dynamic">
                                                                                    <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءؤآإأئ ى \\-]+" />
                                                                                    <RequiredField IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                            <EditFormSettings ColumnSpan="2" />
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn FieldName="Prief" ShowInCustomizationForm="False" Visible="False" VisibleIndex="6">
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <ClientSideEvents DropDown="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" GotFocus="function(s,e){s.ShowDropDown();}" Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" ValueChanged="function(s, e) {cbp.PerformCallback('PayType'); }" />
                                                                    <ClearButton DisplayMode="Always">
                                                                    </ClearButton>
                                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxGridLookup>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="" ColSpan="4" ColumnSpan="4">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxGridLookup ID="OwnNo" runat="server" Caption="الشركة المسندة" AutoGenerateColumns="False" AutoResizeWithContainer="True" ClientInstanceName="OwnNo" DataSourceID="Customers" DropDownStyle="DropDownList" GridViewProperties-SettingsCommandButton-ClearButton-Text="بحث جديد" IncrementalFilteringDelay="20" IncrementalFilteringMode="StartsWith" KeyFieldName="CustNo" PopupHorizontalAlign="Center" RenderIFrameForPopupElements="True" RightToLeft="True" TextFormatString="{1} | {0} | {3}" Width="100%">
                                                                    <GridViewProperties>
                                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                                                        <SettingsText SearchPanelEditorNullText=" بإمكانك البحث عن الزبون برقم النقال أو الاسم أو الاسم اللاتيني" />
                                                                        <SettingsLoadingPanel Mode="Disabled" />
                                                                        <SettingsPopup>
                                                                            <FilterControl AutoUpdatePosition="False">
                                                                            </FilterControl>
                                                                        </SettingsPopup>
                                                                        <SettingsCommandButton>
                                                                            <NewButton ButtonType="Image" RenderMode="Image" Text="زبون جديد">
                                                                                <Image IconID="actions_newemployee_16x16devav">
                                                                                </Image>
                                                                            </NewButton>
                                                                            <UpdateButton ButtonType="Image" RenderMode="Image" Text="حفظ">
                                                                                <Image IconID="actions_save_16x16devav">
                                                                                </Image>
                                                                            </UpdateButton>
                                                                            <CancelButton ButtonType="Image" RenderMode="Image" Text="إلغاء">
                                                                                <Image IconID="actions_undo_16x16devav">
                                                                                </Image>
                                                                            </CancelButton>
                                                                            <EditButton ButtonType="Image" RenderMode="Image" Text="تحرير">
                                                                                <Image IconID="actions_edit_16x16devav">
                                                                                </Image>
                                                                            </EditButton>
                                                                            <SearchPanelApplyButton Text="بحث">
                                                                            </SearchPanelApplyButton>
                                                                        </SettingsCommandButton>
                                                                        <SettingsSearchPanel EditorNullTextDisplayMode="UnfocusedAndFocused" ShowApplyButton="True" ShowClearButton="True" Visible="True" />
                                                                    </GridViewProperties>
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn ShowEditButton="True" ShowInCustomizationForm="True" ShowNewButton="True" VisibleIndex="0">
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataTextColumn Caption="رقم الزبون" FieldName="CustNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="اسم الزبون" FieldName="CustName" ShowInCustomizationForm="True" VisibleIndex="2">
                                                                            <PropertiesTextEdit>
                                                                                <ValidationSettings Display="Dynamic">
                                                                                    <RegularExpression ValidationExpression="^[A-Za-zا-يءؤآإأئ ى \\-]+" />
                                                                                    <RequiredField IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                            <EditFormSettings ColumnSpan="2" />
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="الاسم اللاتيني" FieldName="CustNameE" ShowInCustomizationForm="True" VisibleIndex="3">
                                                                            <EditFormSettings ColumnSpan="2" />
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="رقم الهاتف" FieldName="TelNo" ShowInCustomizationForm="True" VisibleIndex="4">
                                                                            <PropertiesTextEdit>
                                                                                <MaskSettings Mask="\0\0\2\1\8000000000" />
                                                                                <ValidationSettings Display="Dynamic">
                                                                                    <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                                                    <RequiredField IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle HorizontalAlign="Center">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="العنوان" FieldName="Address" ShowInCustomizationForm="True" VisibleIndex="5">
                                                                            <CellStyle HorizontalAlign="Right">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <ClientSideEvents DropDown="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" GotFocus="function(s,e){s.ShowDropDown();}" Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" ValueChanged="function(s, e) { cbp.PerformCallback('PayType');}" />
                                                                    <ClearButton DisplayMode="Always">
                                                                    </ClearButton>
                                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxGridLookup>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem ColSpan="4" ColumnSpan="4" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxGridLookup ID="Sys" runat="server"  Caption="نوع التأمين" AutoGenerateColumns="False" AutoResizeWithContainer="True" ClientInstanceName="Sys" DataSourceID="Systems" KeyFieldName="SUBSYSNO" PopupHorizontalAlign="Center" RenderIFrameForPopupElements="True" RightToLeft="True" TextFormatString="{0} | {1}" Width="100%">
                                                                    <GridViewProperties>
                                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                                                        <SettingsPopup>
                                                                            <FilterControl AutoUpdatePosition="False">
                                                                            </FilterControl>
                                                                        </SettingsPopup>
                                                                    </GridViewProperties>
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn FieldName="SUBSYSNO" ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True">
                                                                            <PropertiesTextEdit Width="250px">
                                                                            </PropertiesTextEdit>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn FieldName="SUBSYSNAME" ShowInCustomizationForm="True" VisibleIndex="2">
                                                                            <EditFormSettings Visible="False" />
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <ClientSideEvents DropDown="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" />
                                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxGridLookup>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                               
                                                    <dx:LayoutItem  ColSpan="1" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxTextBox ID="MainSi" runat="server" Caption="%مبلغ التأمين 100" 
                                                                    ClientEnabled="true" ClientInstanceName="MainSi" DisplayFormatString="n3" 
                                                                    RightToLeft="true" SelectInputTextOnClick="True" Text="0">
                                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                    <ValidationSettings Display="Dynamic">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem  ColSpan="1" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxTextBox ID="MainNet" runat="server" Caption="%القسط 100" ClientEnabled="true" ClientInstanceName="MainNet" DisplayFormatString="n3" RightToLeft="true" SelectInputTextOnClick="True" Text="0">
                                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                    <ValidationSettings Display="Dynamic">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem  ColSpan="1" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxTextBox ID="AcceptedShare" runat="server" Caption="الحصة المقبولة %" ClientInstanceName="AcceptedShare" DisplayFormatString="n3" RightToLeft="true" SelectInputTextOnClick="True" Text="0">
                                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem  ColSpan="1" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxTextBox ID="CommisionRatio" runat="server" Caption=" العمولة % " ClientInstanceName="CommisionRatio" DisplayFormatString="n3" RightToLeft="true" SelectInputTextOnClick="True" Text="0">
                                                                    <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                    <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                        <CaptionStyle Font-Bold="True">
                                                        </CaptionStyle>
                                                    </dx:LayoutItem>
                                                </Items>
                                            </dx:LayoutGroup>
                                
                                            <dx:LayoutItem  ColSpan="1" ShowCaption="False" >
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxTextBox ID="AcceptedSI" runat="server" RightToLeft="true" Caption="مبلغ التأمين المقبول" 
                                                            ClientEnabled="False" ClientInstanceName="AcceptedSI" 
                                                            DisplayFormatString="n3" SelectInputTextOnClick="True" >
                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                                <RequiredField IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem  ColSpan="1" ShowCaption="False">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                       
                                                        <dx:ASPxTextBox ID="AcceptedNet" runat="server" RightToLeft="true" Caption="القســـط المقبـــول" ClientEnabled="False" 
                                                            ClientInstanceName="AcceptedNet" DisplayFormatString="n3" SelectInputTextOnClick="True" >
                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                                <RequiredField IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem ColSpan="1" ShowCaption="False">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxTextBox ID="CommisionValue" runat="server" RightToLeft="true" Caption="يخصم منه قيمـــة العمـــولة" ClientEnabled="False" 
                                                            ClientInstanceName="CommisionValue" DisplayFormatString="n3" >
                                                        </dx:ASPxTextBox>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem  ColSpan="1" ShowCaption="False">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxTextBox ID="Balance" runat="server" Caption="المستحـــــــــق" ClientEnabled="False" ClientInstanceName="Balance" DisplayFormatString="n3" RightToLeft="true">
                                                        </dx:ASPxTextBox>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                                <CaptionStyle Font-Bold="True">
                                                </CaptionStyle>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem Caption="" ColSpan="1">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxButton ID="Save" runat="server" AutoPostBack="False" Text="SAVE">
                                                            <ClientSideEvents Click="function OnChanged(s, e) {
                                                                                scbp.PerformCallback('SaveData');
                                                                                //ReturnToParentPage();
                                                                            }" />
                                                        </dx:ASPxButton>
                                                        <dx:ASPxLabel ID="CommLbl" runat="server">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                                <CaptionStyle Font-Bold="True">
                                                </CaptionStyle>
                                            </dx:LayoutItem>
                                        </Items>
                                    </dx:ASPxFormLayout>
                                    <br />
                                    <dx:ASPxCallback ID="scbp" runat="server" ClientInstanceName="scbp" OnCallback="Scbp_Callback">
                                        <ClientSideEvents CallbackComplete="OnEndCallback" />
                                    </dx:ASPxCallback>
                                    <dx:ASPxComboBox ID="PayType" runat="server" Height="18px" IncrementalFilteringMode="StartsWith" RightToLeft="True" SelectedIndex="0" Width="150px" ClientVisible="false">
                                        <Items>
                                            <dx:ListEditItem Text="نقداً" Value="1" Selected="True" />
                                            <dx:ListEditItem  Text="على الحساب" Value="2" />
                                        </Items>
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                    <asp:SqlDataSource ID="Companies" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT CustNo, rtrim(CustName) As CustName , CustNameE, TelNo FROM CustomerFile"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="Customers" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
                                        InsertCommand="NewCustomer" 
                                        InsertCommandType="StoredProcedure" 
                                        SelectCommand="SELECT CustNo, rtrim([CustName]) As CustName, rtrim([CustNameE]) As CustNameE, rtrim(TelNo) as TelNo, rtrim([CustName]) +'/'+ rtrim([CustNameE]) +'/'+ rtrim(TelNo) As Prief, [Email], [Address] ,[SpecialCase], [AccNo] FROM [CustomerFile] Order BY CustNo desc,REVERSE(CustName)" 
                                        UpdateCommand="UpdateCustomer" 
                                        UpdateCommandType="StoredProcedure">
                                        <InsertParameters>
                                            <asp:Parameter Name="CustName" Type="String" />
                                            <asp:Parameter DefaultValue="/" Name="CustNameE" Type="String" />
                                            <asp:Parameter DefaultValue="00218" Name="TelNo" Type="String" />
                                            <asp:Parameter DefaultValue="ليبيا" Name="Address" Type="String" />
                                            <asp:SessionParameter DefaultValue="/" SessionField="User" Name="User" Type="String" />
                                        </InsertParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="CustNo" Type="Int32" />
                                            <asp:Parameter Name="CustName" Type="String" />
                                            <asp:Parameter DefaultValue="/" Name="CustNameE" Type="String" />
                                            <asp:Parameter DefaultValue="+218" Name="TelNo" Type="String" />
                                            <asp:Parameter DefaultValue="ليبيا" Name="Address" Type="String" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="Customers1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" InsertCommand="NewCustomer" InsertCommandType="StoredProcedure" SelectCommand="SELECT CustNo, rtrim([CustName]) As CustName, [CustNameE], [TelNo],[Email], [Address] ,[SpecialCase], [AccNo] FROM [CustomerFile]
                                                        Order By CustNo desc">
                                         <InsertParameters>
                                            <asp:Parameter Name="CustName" Type="String" />
                                            <asp:Parameter DefaultValue="/" Name="CustNameE" Type="String" />
                                            <asp:Parameter DefaultValue="00218" Name="TelNo" Type="String" />
                                            <asp:Parameter DefaultValue="ليبيا" Name="Address" Type="String" />
                                            <asp:SessionParameter DefaultValue="/" SessionField="User" Name="User" Type="String" />
                                        </InsertParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="Systems" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT rtrim(SUBSYSNO) As SUBSYSNO, rtrim(SUBSYSNAME) As SUBSYSNAME , MAINSYS FROM SUBSYSTEMS WHERE (MAINSYS IN (1,3,6,9)) AND Branch=DBO.MainCenter() ORDER BY SUBSYSNO"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="Reinsurers" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" InsertCommand="NewReinsurer" InsertCommandType="StoredProcedure" ProviderName="System.Data.SqlClient" SelectCommand="SELECT rtrim(TPName) As ReinsurerName,TPNo FROM ExtraInfo WHERE tp='ReCom' ORDER BY TpNo">
                                        <InsertParameters>
                                            <asp:Parameter Name="ReinsurerName" Type="String" />
                                        </InsertParameters>
                                    </asp:SqlDataSource>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
        </div>
    </form>
</body>
</html>