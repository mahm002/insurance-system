<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PolicyUC.ascx.vb" Inherits="PolicyUC" %>

<!-- Add viewport meta tag for responsiveness -->
<meta name="viewport" content="width=device-width, initial-scale=1.0">

<!-- Bootstrap & jQuery -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

<!-- Toastr CSS -->
<link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" rel="stylesheet" />

<!-- Your existing scripts remain the same -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
<script src="../../scripts/jquery-latest.min.js"></script>
<script src="../../scripts/jquery-3.7.1.min.js"></script>

<!-- Responsive CSS -->
<style type="text/css">
    /* Responsive container */
    .responsive-policy-container {
        max-width: 100%;
        margin: 0 auto;
        padding: 10px;
    }

    /* Make DevExpress controls responsive */
    .dxeControl, .dxeTextBox, .dxeComboBox, .dxeDateEdit, .dxeSpinEdit {
        max-width: 100% !important;
    }

    /* Responsive grid layout */
    .policy-grid {
        display: grid;
        grid-template-columns: 1fr;
        gap: 15px;
        width: 100%;
    }

    @media (min-width: 768px) {
        .policy-grid {
            grid-template-columns: 2fr 1fr;
        }
    }

    @media (min-width: 1200px) {
        .policy-grid {
            grid-template-columns: 2fr 1fr;
        }
    }

    /* Responsive panels */
    .responsive-panel {
        min-height: 200px;
        margin-bottom: 15px;
    }

    /* Stack elements vertically on mobile */
    .mobile-stack {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }

    .mobile-stack .form-group {
        margin-bottom: 10px;
    }

    /* Auto-complete results responsive */
    #autocomplete-results { 
        position: fixed !important;
        z-index: 9999 !important;
        background: white;
        border: 1px solid #007bff;
        border-radius: 5px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        width: 90vw !important;
        max-width: 500px;
        min-width: 300px;
        display: none;
        max-height: 60vh;
        overflow-y: auto;
    }

    .customer-item { 
        cursor: pointer;
        padding: 12px 15px;
        border-bottom: 1px solid #f8f9fa;
        transition: all 0.2s ease;
        margin: 0;
    }

    .customer-item:hover { 
        background-color: #d5e3e3;
        color: Grey;
        transform: translateX(5px);
    }

    /* Responsive text */
    .responsive-text {
        font-size: clamp(12px, 2.5vw, 14px);
    }

    /* Button container for mobile */
    .button-container {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        margin: 10px 0;
    }

    .button-container .btn {
        flex: 1 1 auto;
        min-width: 120px;
        font-size: 12px;
        padding: 6px 12px;
    }

    /* Form labels */
    .form-label {
        font-weight: bold;
        margin-bottom: 5px;
        font-size: 14px;
    }

    /* DevExpress control wrappers */
    .dx-control-wrapper {
        width: 100% !important;
    }

    /* Hide less important elements on mobile */
    @media (max-width: 767px) {
        .hide-on-mobile {
            display: none !important;
        }
        
        .dxeCaptionHACSys {
            font-size: 12px;
        }
        
        /* Make round panels more compact */
        .dxrpCollapsed .dxrp-header, 
        .dxrp .dxrp-header {
            padding: 8px 12px;
        }
    }

    /* Print styles */
    @media print {
        .btn, .button-container {
            display: none !important;
        }
    }
</style>

<!-- Your existing JavaScript remains the same -->
<script type="text/javascript">
    // Your existing JavaScript code remains unchanged
    // Toastr configuration, customer search functions, etc.
</script>

<!-- Responsive Container -->
<div class="responsive-policy-container">
    <div class="policy-grid">
        <!-- Left Column - Main Policy Data -->
        <div class="left-column">
            <!-- Customer and Coverage Data -->
            <dx:ASPxRoundPanel ID="PolicyPanel" ClientInstanceName="PolicyPanel" runat="server" 
                RightToLeft="True" LoadContentViaCallback="true" 
                HeaderText="بيانات الزبون ونوع التغطية" 
                CssClass="responsive-panel" Width="100%">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <div class="container-fluid">
                            <div class="row mobile-stack mb-3">
                                <div class="col-12 col-md-6 col-lg-3">
                                    <div class="form-group">
                                        <label class="form-label">رقم الوثيقة</label>
                                        <dx:ASPxTextBox ID="PolNo" runat="server" ClientEnabled="False" 
                                            ClientInstanceName="PolNo" Width="100%" RightToLeft="True">
                                        </dx:ASPxTextBox>
                                        <dx:ASPxTextBox ID="OrderNo" runat="server" ClientEnabled="False"
                                        ClientInstanceName="OrderNo" ClientVisible="false" CssClass="1"
                                        RightToLeft="True" Text="0"
                                        Width="150px">
                                        <ValidationSettings SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-12 col-md-6 col-lg-3">
                                    <div class="form-group">
                                        <label class="form-label">رقم الملحق</label>
                                        <div class="d-flex gap-2">
                                            <dx:ASPxTextBox ID="EndNo" runat="server" ClientEnabled="False" 
                                                ClientInstanceName="EndNo" CssClass="3" Text="0" Width="100%">
                                                <ValidationSettings SetFocusOnError="True">
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                            <dx:ASPxButton ID="Endorsment" runat="server" AutoPostBack="False" 
                                                ClientInstanceName="Endorsment" ClientVisible="False" Text="ملحق" 
                                                CssClass="btn-sm">
                                                <ClientSideEvents Click="Endorsment" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 col-md-6 col-lg-3">
                                    <div class="form-group">
                                        <label class="form-label">رقم الشهادة</label>
                                        <div class="d-flex gap-2">
                                            <dx:ASPxTextBox ID="LoadNo" runat="server" ClientEnabled="False" 
                                                ClientInstanceName="LoadNo" CssClass="3" Text="0" Width="100%">
                                                <ValidationSettings SetFocusOnError="True">
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                            <dx:ASPxButton ID="Shipment" runat="server" AutoPostBack="False" 
                                                ClientInstanceName="Shipment" ClientVisible="False" Text="شهادة تأمين"
                                                CssClass="btn-sm">
                                                <ClientSideEvents Click="Loads" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 col-md-6 col-lg-3">
                                    <div class="button-container">
                                        <dx:ASPxButton ID="ForLoss" runat="server" AutoPostBack="False"
                                            ClientInstanceName="ForLoss" Text="ملـحق بدل فاقد" ClientVisible="false"
                                            CssClass="btn-sm">
                                            <ClientSideEvents Click="ForLoss" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="ChangeOwner" runat="server" AutoPostBack="False" 
                                            ClientInstanceName="ChangeOwner" ClientVisible="False" Text="ملحق تغيير ملكية"
                                            CssClass="btn-sm">
                                            <ClientSideEvents Click="ChangeOwner" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-12">
                                    <div class="form-group">
                                        <label class="form-label">الزبــــــون</label>
                                        <div style="position: relative;">
                                            <dx:ASPxTextBox ID="txtCustomerSearch" runat="server"
                                                ClientInstanceName="clientTxtCustomerSearch"
                                                Width="100%"
                                                NullText="🔍 اكتب اسم الزبون أو رقم الهاتف للبحث (3 أحرف على الأقل)...">
                                                <ClientSideEvents KeyUp="onCustomerSearch" />
                                                <ValidationSettings Display="Dynamic">
                                                    <RequiredField IsRequired="true" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                            <asp:HiddenField ID="CustNo" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row mobile-stack mb-3">
                                <div class="col-12 col-md-6">
                                    <div class="form-group">
                                        <label class="form-label">الاسم الإنجليزي</label>
                                        <dx:ASPxTextBox ID="CustNameE" runat="server" ClientInstanceName="CustNameE" 
                                            Width="100%" CssClass="10">
                                            <ValidationSettings Display="Dynamic">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-12 col-md-6">
                                    <div class="form-group">
                                        <label class="form-label">الهاتف</label>
                                        <dx:ASPxTextBox ID="TelNo" runat="server" ClientInstanceName="TelNo" 
                                            CssClass="10" NullText="للاستفادة من خدمة الرسائل القصيرة" 
                                            RightToLeft="True" Width="100%">
                                            <MaskSettings AllowEscapingInEnums="True" Mask="+\2\1\8000000000" />
                                            <ValidationSettings Display="Dynamic">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-12">
                                    <div class="form-group">
                                        <label class="form-label">العنوان</label>
                                        <dx:ASPxTextBox ID="Address" runat="server" ClientInstanceName="Address" 
                                            CssClass="10" Width="100%">
                                            <ValidationSettings Display="Dynamic">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-12">
                                    <div class="form-group">
                                        <label class="form-label">إصدار لصالح</label>
                                        <dx:ASPxGridLookup ID="OwnNo" runat="server" AutoGenerateColumns="False" 
                                            AutoResizeWithContainer="True" Caption="إصدار لصالح" ClientInstanceName="OwnNo" 
                                            DataSourceID="Customers" KeyFieldName="CustNo" NullText="/" 
                                            PopupHorizontalAlign="Center" RenderIFrameForPopupElements="True" 
                                            RightToLeft="True" TextFormatString="{0} | {1} | {3}" Width="100%">
    
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <div class="form-group">
                                        <label class="form-label">نوع التغطية</label>
                                        <dx:ASPxComboBox ID="CoverType" runat="server" ClientInstanceName="CoverType" 
                                            DataSourceID="Covers" RightToLeft="True" SelectedIndex="0" 
                                            TextField="CoverName" ValueField="CoverNo" ValueType="System.Int32" 
                                            Width="100%">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>

            <!-- Coverage Dates -->
            <dx:ASPxRoundPanel ID="CoverDate" ClientInstanceName="CoverDate" runat="server" 
                AllowCollapsingByHeaderClick="True" RightToLeft="True" 
                HeaderText="فترة التغطية" Width="100%" CssClass="responsive-panel">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <div class="container-fluid">
                            <div class="row mobile-stack">
                                <div class="col-12 col-md-4">
                                    <div class="form-group">
                                        <label class="form-label">المقيـــــــــاس</label>
                                        <dx:ASPxComboBox ID="Measure" runat="server" ClientInstanceName="Measure" 
                                            DataSourceID="Measures" ValueType="System.Int32" DropDownStyle="DropDownList"
                                            SelectedIndex="2" TextField="TpName" ValueField="TpNo" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="DAdd" />
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-12 col-md-2">
                                    <div class="form-group">
                                        <label class="form-label">الفترة</label>
                                        <dx:ASPxSpinEdit ID="Interval" runat="server" ClientInstanceName="Intv" 
                                            RightToLeft="True" Width="100%" AllowNull="false">
                                            <ClientSideEvents ValueChanged="DAdd" />
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxSpinEdit>
                                    </div>
                                </div>
                                <div class="col-12 col-md-3">
                                    <div class="form-group">
                                        <label class="form-label">التغطية من</label>
                                        <dx:ASPxDateEdit ID="CoverFrom" runat="server" ClientInstanceName="CoverFrom" 
                                            DisplayFormatString="yyyy/MM/dd" EditFormatString="yyyy/MM/dd" 
                                            RightToLeft="True" Width="100%">
                                            <ClientSideEvents DateChanged="Validate" ValueChanged="DAdd" />
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="col-12 col-md-3">
                                    <div class="form-group">
                                        <label class="form-label">إلى</label>
                                        <dx:ASPxDateEdit ID="CoverTo" runat="server" ClientInstanceName="CoverTo" 
                                            ClientEnabled="false" ClientReadOnly="True" DisplayFormatString="yyyy/MM/dd" 
                                            RightToLeft="True" Width="100%">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
        </div>

        <!-- Right Column - Premium and Commissions -->
        <div class="right-column">
            <!-- Premium Panel -->
            <dx:ASPxRoundPanel ID="PremiumPanel" ClientInstanceName="PremiumPanel" runat="server" 
                AllowCollapsingByHeaderClick="True" RightToLeft="True" 
                HeaderText="/ العملة /طريقة الدفع /بيانات القسط" Width="100%" 
                CssClass="responsive-panel" Collapsed="false">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <div class="container-fluid">
                            <div class="row mobile-stack">
                                <div class="col-12 col-sm-6">
                                    <div class="form-group">
                                        <label class="form-label">العملة</label>
                                        <dx:ASPxComboBox ID="Currency" runat="server" ClientInstanceName="Currency" 
                                            DataSourceID="Cur" DropDownStyle="DropDownList" ValueType="System.Int32"
                                            RightToLeft="True" SelectedIndex="0" AutoPostBack="false"
                                            TextField="TpName" ValueField="TpNo" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {cbp.PerformCallback('ExRate');}" />
                                            <ValidationSettings SetFocusOnError="True" Display="Dynamic">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-6">
                                    <div class="form-group">
                                        <label class="form-label">سعر الصرف</label>
                                        <dx:ASPxTextBox ID="ExcRate" runat="server" ClientEnabled="True" 
                                            ClientInstanceName="ExcRate" CssClass="5" Text="1" Width="100%" 
                                            Caption="سعر الصرف">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                        <dx:ASPxComboBox ID="PayType" runat="server" ClientInstanceName="PayType" DataSourceID="Pay" RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" ValueType="System.Int32" Width="110px">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                                     cbp.PerformCallback('PayType');  }" />
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                        <dx:ASPxTextBox ID="AccountNo" runat="server" ClientEnabled="False" ClientInstanceName="AccountNo" CssClass="1" Text="0" Width="110px">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <!-- Premium amounts in a more compact layout -->
                            <div class="premium-amounts">
                                <div class="row">
                                    <div class="col-12 col-sm-6">
                                        <label class="form-label">صافـــي القسط</label>
                                    </div>
                                    <div class="col-12 col-sm-6">
                                        <dx:ASPxTextBox ID="NETPRM" runat="server" ClientEnabled="False" 
                                            ClientInstanceName="NetPRM" CssClass="2" Text="0" Width="100%" 
                                            ForeColor="#0033cc" Font-Bold="true">
                                        </dx:ASPxTextBox>
                                        <strong>
                                        <dx:ASPxTextBox ID="ISSPRM" runat="server" ClientEnabled="False" ClientInstanceName="ISSPRM" CssClass="2" ForeColor="#0033CC" SelectInputTextOnClick="True" Text="0" Width="108px">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                        <dx:ASPxTextBox ID="STMPRM" runat="server" ClientInstanceName="STMPRM" CssClass="2" ForeColor="#0033CC" Text="0" Width="108px">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                        </strong>
                                    </div>
                                </div>
                                <!-- Repeat similar structure for TAXPRM, CONPRM, STMPRM, ISSPRM, TOTPRM -->
                            </div>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>

            <!-- Commissions Panel -->
            <dx:ASPxRoundPanel ID="Commissions" runat="server" AllowCollapsingByHeaderClick="True" 
                ClientInstanceName="Commissions" Collapsed="false" HeaderText="العمولات والمسوقين" 
                RightToLeft="true" Width="100%" CssClass="responsive-panel">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <!-- Your existing commissions content with responsive layout -->
                        <dx:ASPxGridLookup ID="Broker" runat="server" AutoGenerateColumns="False" AutoResizeWithContainer="True" ClientInstanceName="Broker" DataSourceID="Brokers" KeyFieldName="TpNo" PopupHorizontalAlign="Center" RenderIFrameForPopupElements="True" RightToLeft="True" TextFormatString="{0} | {1} " Width="100%">
                            <GridViewProperties>
                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                <SettingsPopup>
                                    <FilterControl AutoUpdatePosition="False">
                                    </FilterControl>
                                </SettingsPopup>
                                <SettingsCommandButton>
                                    <NewButton Text="عميل/مسوق جديد">
                                    </NewButton>
                                    <UpdateButton Text="حفظ">
                                    </UpdateButton>
                                    <CancelButton Text="إلغاء">
                                    </CancelButton>
                                </SettingsCommandButton>
                            </GridViewProperties>
                            <Columns>
                                <dx:GridViewCommandColumn ShowInCustomizationForm="True" ShowNewButton="True" VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn Caption="رقم العميل أو المسوق" FieldName="TpNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="اسم العميل أو المسوق" FieldName="BrokerName" ShowInCustomizationForm="True" VisibleIndex="2">
                                    <PropertiesTextEdit Width="250px">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents DropDown="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());}" ValueChanged="function(s, e) {
                                            //alert(CommisionTp.GetValue());
                                            if (s.GetValue() == 0) {
                                                          Commision.SetEnabled(false);
                                                          CommisionTp.SetEnabled(false);
                                                          Commision.SetValue(0);
                                                          CommisionTp.SetValue(0);
                                                        } else {
                                                            Commision.SetEnabled(true);
                                                            CommisionTp.SetEnabled(true);
                                                        }

                                            }" />
                        </dx:ASPxGridLookup>
                        <dx:ASPxTextBox ID="Commision" runat="server" ClientEnabled="False" ClientInstanceName="Commision" CssClass="2" DisplayFormatString="n2" SelectInputTextOnClick="True" Width="110px">
                        </dx:ASPxTextBox>
                        <dx:ASPxRadioButtonList ID="CommisionType" runat="server" Caption="نوع العمولة" ClientEnabled="False" ClientInstanceName="CommisionTp" RepeatLayout="OrderedList" ValueType="System.Int32" Width="100%">
                            <Items>
                                <dx:ListEditItem Text="العمولة قيمة" Value="1" />
                                <dx:ListEditItem Text="العمولة نسبة%" Value="2" />
                            </Items>
                            <Border BorderStyle="None" />
                        </dx:ASPxRadioButtonList>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>

            <!-- Action Buttons -->
            <div class="action-buttons mt-3">
                <div class="d-grid gap-2">
                    <dx:ASPxButton ID="ExcelImp" runat="server" AutoPostBack="false" 
                        Text="استيراد من ملف Excel" Width="100%" CssClass="btn-secondary">
                        <Image Url="../Content/Images/IMPORTEXCEL.png"></Image>
                        <ClientSideEvents Click="function(s,e){ cbp.PerformCallback('Import'); }" />
                    </dx:ASPxButton>
                    
                    <dx:ASPxButton ID="DistPolicy" runat="server" AutoPostBack="false" 
                        Text="معاينة توزيع الوثيقة" Width="100%" CssClass="btn-info">
                        <Image Url="../Content/Images/DistPolicy.png"></Image>
                        <ClientSideEvents Click="function(s,e){ cbp.PerformCallback('Dist'); }" />
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Auto-complete results container -->
<div id="autocomplete-results"></div>

<!-- Data Sources -->
<asp:SqlDataSource ID="Customers" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
    InsertCommand="NewCustomer" InsertCommandType="StoredProcedure"
    SelectCommand="SELECT CustNo, rtrim([CustName]) As CustName, rtrim([CustNameE]) As CustNameE, rtrim(TelNo) as TelNo, rtrim([CustName]) +'/'+ rtrim([CustNameE]) +'/'+ rtrim(TelNo) As Prief, [Email], [Address] ,[SpecialCase], [AccNo] FROM [CustomerFile] Order BY CustNo desc,REVERSE(CustName)"
    UpdateCommand="UpdateCustomer" UpdateCommandType="StoredProcedure">
    <InsertParameters>
        <asp:Parameter Name="CustName" Type="String" />
        <asp:Parameter DefaultValue="/" Name="CustNameE" Type="String" />
        <asp:Parameter DefaultValue="00218" Name="TelNo" Type="String" />
        <asp:Parameter DefaultValue="ليبيا" Name="Address" Type="String" />
        <asp:SessionParameter DefaultValue="/" SessionField="User" Name="User" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="Covers" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
    SelectCommand="SELECT [CoverNo], rtrim([CoverName]) As CoverName FROM [Covers] WHERE ([SubSystem] = @SubSystem)">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="01" Name="SubSystem" QueryStringField="Sys" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

                                    <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP=&#39;Measure&#39; order by TPNO" ID="Measures"></asp:SqlDataSource>


                                    <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP=&#39;meser&#39;" ID="SqlDataSource1"></asp:SqlDataSource>

                                    <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP=&#39;Cur&#39; order by TpNo" ID="Cur"></asp:SqlDataSource>

                                    <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP=&#39;Pay&#39; order by TPNo" ID="Pay"></asp:SqlDataSource>

                                

<!-- Other data sources (Pay, Cur, Measures, Brokers) remain the same -->