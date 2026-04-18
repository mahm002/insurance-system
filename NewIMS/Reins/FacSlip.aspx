<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FacSlip.aspx.vb" Inherits=".FacSlip" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function ReturnToParentPage() {
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup(1);
        }

        function END_OnEndCallback(s, e) {
            if (parseFloat(AcceptedShare.GetValue()) > 0) {
                if (e.result == null) {
                    FacSi.SetValue(parseFloat(AcceptedShare.GetValue()) * parseFloat(OriginalSI.GetValue()) / 100);
                    FacNet.SetValue(parseFloat(AcceptedShare.GetValue()) * parseFloat(OriginalNet.GetValue()) / 100);
                    Commision.SetValue(parseFloat(CommisionRatio.GetValue()) * parseFloat(FacNet.GetValue()) / 100);
                    Balance.SetValue(parseFloat(FacNet.GetValue()) - parseFloat(Commision.GetValue()));
                }
                else {
                    FacSi.SetValue(e.result);
                    FacNet.SetValue(parseFloat(AcceptedShare.GetValue()) * parseFloat(OriginalNet.GetValue()) / 100);
                    Commision.SetValue(parseFloat(CommisionRatio.GetValue()) * parseFloat(FacNet.GetValue()) / 100);
                    Balance.SetValue(parseFloat(FacNet.GetValue()) - parseFloat(Commision.GetValue()));
                }
            }
        }

        function OnComboRe_ButtonClick(s, e) {
            Repop.ShowAtElement(s.GetMainElement());
        }
        var command = "";
        function OngridReins_BeginCallback(s, e) {
            command = e.command;
        }

        function OngridReins_EndCallback(s, e) {
            if (command == "UPDATEEDIT") {
                ReCom.PerformCallback();
            }
        }

        function OnChange(s, e) {
            if (s.GetValue() == null) {
                s.SetValue(0);
            }
            scbp.PerformCallback('ShareChange');
        }
    </script>
    <style type="text/css">
        .dxgvControl_Glass,
        .dxgvDisabled_Glass {
            border: 1px solid #7EACB1;
            font: 12px Tahoma, Geneva, sans-serif;
            background-color: #f4fafb;
            color: Black;
            cursor: default;
        }

        .dxgvTable_Glass {
            -webkit-tap-highlight-color: transparent;
        }

        .dxgvTable_Glass {
            background-color: #f4fafb;
            border-width: 0;
            border-collapse: separate !important;
            overflow: hidden;
        }

        .dxgvInlineEditRow_Glass,
        .dxgvDataRow_Glass {
            background-color: White;
        }

        .dxgvCommandColumn_Glass {
            padding: 2px;
            white-space: nowrap;
        }

        .dxgvPagerTopPanel_Glass,
        .dxgvPagerBottomPanel_Glass {
            padding: 3px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width: 100%;">
        <dx:ASPxLabel ID="CheckRemainlbl" runat="server" Text="" Style="font-size: x-large; font-weight: 700; text-align: center">
        </dx:ASPxLabel>
        <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout1" CssClass="formLayout" RightToLeft="False" Theme="Office365">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
            <Items>
                <dx:LayoutGroup Caption="Facultative Closing" ColCount="4" GroupBoxDecoration="HeadingLine" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                    <Paddings PaddingTop="10px"></Paddings>

                    <GroupBoxStyle>
                        <Caption Font-Bold="true" Font-Size="16" ForeColor="#006600" />
                    </GroupBoxStyle>
                    <Items>
                        <dx:LayoutItem Caption="Policy Number" HorizontalAlign="Left">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="PolNo" runat="server" RightToLeft="False" SelectInputTextOnClick="True" ClientEnabled="false" Width="100%">
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="End. No." HorizontalAlign="Left">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="EndNo" runat="server" ClientEnabled="false" ClientInstanceName="EndNo" RightToLeft="False">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="LoadNo" runat="server" ClientVisible="False" ClientInstanceName="LoadNo" RightToLeft="False">
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Period" HorizontalAlign="Left" ColSpan="1">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="Period" runat="server" RightToLeft="False" Text="/">
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="SlipDate" ColSpan="1" HorizontalAlign="Left">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxDateEdit ID="SlipDate" runat="server" RightToLeft="True">
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Insured Name" ColSpan="2" ColumnSpan="2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="InsuredName" runat="server" RightToLeft="False" Text="/" Width="100%">
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Type Of Insurance">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="InsuranceType" runat="server" RightToLeft="False" Text="/">
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Your Ref.">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="RefNo" runat="server" RightToLeft="False" Text="/">
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem ColSpan="4" Caption="Messrs">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxComboBox ID="ReCom" runat="server" ClientInstanceName="ReCom" DataSourceID="Reinsurers" RightToLeft="false" ValueType="System.Int32"
                                        TextField="ReinsurerName" ValueField="TPNo" Width="100%" DropDownStyle="DropDownList">
                                        <ClientSideEvents ButtonClick="OnComboRe_ButtonClick" />
                                        <Buttons>
                                            <dx:EditButton>
                                                <Image Url="../Content/Images/new_copy_16px.png" AlternateText="New" />
                                            </dx:EditButton>
                                        </Buttons>
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Sum Insured">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="OriginalSI" ClientInstanceName="OriginalSI" runat="server" DisplayFormatString="##,#00.000" RightToLeft="False" SelectInputTextOnClick="True" Text="0"
                                        ClientEnabled="false">
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Premium">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="OriginalNet" ClientInstanceName="OriginalNet" runat="server" DisplayFormatString="##,#00.000" RightToLeft="False" SelectInputTextOnClick="True" Text="0" ClientEnabled="false">
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Share %">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="AcceptedShare" runat="server" ClientInstanceName="AcceptedShare"
                                        Text="0" RightToLeft="False" DisplayFormatString="n3" SelectInputTextOnClick="true">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ValidationGroup="Data">
                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                        <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Commission %">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="CommisionRatio" runat="server" ClientInstanceName="CommisionRatio"
                                        DisplayFormatString="n3" RightToLeft="False" SelectInputTextOnClick="True" Text="0">
                                        <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                        <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                            <CaptionStyle Font-Bold="True">
                            </CaptionStyle>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutItem Caption="Slip No." ColSpan="1">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxLabel ID="CancelLbl" runat="server" Text="" ClientVisible="false" DisabledStyle-ForeColor="Red">
                                <DisabledStyle ForeColor="Red">
                                </DisabledStyle>
                            </dx:ASPxLabel>
                            <dx:ASPxTextBox ID="SlipNo" ClientInstanceName="SlipNo" runat="server" RightToLeft="False">
                                <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                    <RequiredField IsRequired="True"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                    <CaptionStyle Font-Bold="True">
                    </CaptionStyle>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Facultative Sum Insured" ColSpan="1">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="FacSI" ClientInstanceName="FacSi" DisplayFormatString="##,#00.000" runat="server"
                                ClientEnabled="false" RightToLeft="False">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Facultative Premium" ColSpan="1">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="FacNet" ClientInstanceName="FacNet" runat="server" DisplayFormatString="##,#00.000"
                                ClientEnabled="false" RightToLeft="False">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Commision" ColSpan="1">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="Commision" ClientInstanceName="Commision" runat="server" DisplayFormatString="##,#00.000"
                                ClientEnabled="false" RightToLeft="False">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Balance" ColSpan="1">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="Balance" ClientInstanceName="Balance" DisplayFormatString="##,#00.000" runat="server"
                                ClientEnabled="false" RightToLeft="False">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                    <CaptionStyle Font-Bold="True">
                    </CaptionStyle>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="" ColSpan="1">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxButton ID="Save" runat="server" AutoPostBack="False" Text="SAVE" CausesValidation="true">
                                <ClientSideEvents Click="function OnChanged(s, e) {
                                                                                scbp.PerformCallback('SaveData');
                                                                                ReturnToParentPage();
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

        <dx:ASPxCallback ID="scbp" runat="server" ClientInstanceName="scbp" OnCallback="Scbp_Callback">
            <ClientSideEvents CallbackComplete="END_OnEndCallback" />
        </dx:ASPxCallback>

        <dx:ASPxPopupControl ID="Repop" runat="server" ClientInstanceName="Repop" PopupHorizontalAlign="WindowCenter" Width="300px" AutoUpdatePosition="true"
            PopupHorizontalOffset="10" AllowDragging="True" EnableClientSideAPI="True" HeaderText="Ad New Participants Reinsurance" EncodeHtml="False" EnableViewState="False">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxGridView runat="server" AutoGenerateColumns="False" KeyFieldName="TPNo" ClientInstanceName="gridReins" RightToLeft="True"
                        DataSourceID="Reinsurers" Width="300px" ID="gridReins">
                        <ClientSideEvents BeginCallback="OngridReins_BeginCallback" EndCallback="OngridReins_EndCallback"></ClientSideEvents>

                        <SettingsEditing Mode="Inline"></SettingsEditing>

                        <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>

                        <SettingsPopup>
                            <FilterControl AutoUpdatePosition="False"></FilterControl>
                        </SettingsPopup>
                        <Columns>
                            <dx:GridViewCommandColumn ShowNewButton="True" ShowInCustomizationForm="True" VisibleIndex="0"></dx:GridViewCommandColumn>
                            <dx:GridViewDataColumn FieldName="ReinsurerName" ShowInCustomizationForm="True" VisibleIndex="0"></dx:GridViewDataColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <br />

        <dx:ASPxLabel ID="SubIns" runat="server" Text="" ClientVisible="false">
        </dx:ASPxLabel>
        <dx:ASPxLabel ID="Cur" runat="server" Text="" ClientVisible="false">
        </dx:ASPxLabel>
        <dx:ASPxLabel ID="ExcRate" runat="server" Text="" ClientVisible="false">
        </dx:ASPxLabel>
        <dx:ASPxLabel ID="PolRef" runat="server" Text="" ClientVisible="false">
        </dx:ASPxLabel>

        <asp:SqlDataSource ID="Reinsurers" runat="server" ProviderName="System.Data.SqlClient"
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            SelectCommand="SELECT rtrim(TPName) As ReinsurerName,TPNo FROM ExtraInfo WHERE tp='ReCom' ORDER BY TpNo"
            InsertCommand="NewReinsurer"
            InsertCommandType="StoredProcedure">
            <InsertParameters>
                <asp:Parameter Name="ReinsurerName" Type="String"></asp:Parameter>
            </InsertParameters>
        </asp:SqlDataSource>
        <dx:ASPxPopupControl runat="server" PopupHorizontalAlign="OutsideRight" PopupHorizontalOffset="10" AllowDragging="True" ClientInstanceName="popup" EnableClientSideAPI="True" HeaderText="Reinsurance Participants" EncodeHtml="False" ID="popup" EnableViewState="False">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <asp:SqlDataSource ID="Participants" runat="server"
            CancelSelectOnNullParameter="False"
            ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
            DeleteCommand="DELETE FROM TRSECFLE WHERE (Id = @Id)"
            InsertCommand="NewParticipant"
            InsertCommandType="StoredProcedure"
            SelectCommand="SELECT Id, dbo.GetExtraCatName(N&#39;ReCom&#39;, TRREINSCO) AS TRREINSCO, TRSHACOM, IsLeader,War FROM TRSECFLE WHERE (RTRIM(TREATYNO) = @Treaty)">
            <DeleteParameters>
                <asp:Parameter Name="Id"></asp:Parameter>
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="TRREINSCO" Type="Int16"></asp:Parameter>
                <asp:QueryStringParameter QueryStringField="Year" Name="TREATYNO"></asp:QueryStringParameter>
                <asp:Parameter Name="TRSHACOM" Type="Double"></asp:Parameter>
                <asp:Parameter DefaultValue="False" Name="IsLeader" Type="Boolean"></asp:Parameter>
                <asp:Parameter DefaultValue="False" Name="War" Type="Boolean"></asp:Parameter>
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="Year" DefaultValue="0" Name="Treaty"></asp:QueryStringParameter>
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>