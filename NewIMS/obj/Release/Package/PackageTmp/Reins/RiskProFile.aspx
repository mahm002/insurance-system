<%@ Page Title="Risk Profile" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="RiskProFile.aspx.vb" Inherits=".RiskProFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function puOnInit(s, e) {
            AdjustSize();
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
                if (s.IsVisible())
                    s.UpdatePosition();
            });
        }

        function AdjustSize() {
            var width = Math.max(0, document.documentElement.clientWidth) * 0.98;
            var height = Math.max(0, document.documentElement.clientHeight) * 0.98;
            PrintPop.SetWidth(width);
            PrintPop.SetSize(width, height);
        }

        function OnEndCallback(s, e) {
            //alert(s.cpNewWindowUrl);

            if (s.cpNewWindowUrl != null && s.cpMyAttribute == 'PRINT') {
                //alert(s.cpNewWindowUrl);
                let hdr = s.cpResult
                let mhdr = s.cpMyAttribute + ' - '
                PrintPop.SetHeaderText(mhdr.concat(hdr));
                PrintPop.SetContentHtml(null);
                PrintPop.SetContentUrl(s.cpNewWindowUrl);
                PrintPop.Show();
                
            }

            //s.cpMyAttribute = ''
            //s.cpNewWindowUrl = null
        }


    </script>

    <dx:ASPxCallback ID="ASPxCallback1" ClientInstanceName="cbp" runat="server" OnCallback="ASPxCallback1_Callback">
        <ClientSideEvents EndCallback="OnEndCallback" />
    </dx:ASPxCallback>
    <dx:ASPxFormLayout runat="server" RightToLeft="false" Theme="Office365" ID="ASPxFormLayout1" Width="100%" 
        AlignItemCaptionsInAllGroups="true">
        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit">
        </SettingsAdaptivity>
        <Items>
            <dx:LayoutGroup Caption="RISK PROFILE" ColCount="4" ColumnCount="4" GroupBoxDecoration="HeadingLine"  Width="100%"
                UseDefaultPaddings="False"
                ColSpan="1">
                <Paddings PaddingTop="10px">
                </Paddings>
                <GroupBoxStyle>
                    <Caption Font-Bold="True" Font-Size="16pt" ForeColor="#006600">
                    </Caption>
                </GroupBoxStyle>
                <Items>
                    <dx:LayoutItem Caption="Main Ins.Dept" ColSpan="4" ColumnSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="Sys" runat="server" AutoPostBack="True" DataSourceID="MainSys" RightToLeft="False" SelectedIndex="0" TextField="SYSNAMEL" ValueField="SYSNO" Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {SubSys.PerformCallback();}" />
                                </dx:ASPxComboBox>
                                <asp:SqlDataSource ID="MainSys" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                    SelectCommand="SELECT SYSNO, RTRIM(SYSNAMEL) AS SYSNAMEL FROM SYSTEMS WHERE (SYSNO IN (1, 3, 6, 9))"></asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionStyle Font-Bold="True">
                        </CaptionStyle>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="SubSystem. Dept" ColSpan="4" ColumnSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTokenBox ID="SubSys" runat="server" AllowCustomTokens="False" AllowMouseWheel="True" ClientInstanceName="SubSys" DataSourceID="SubSystems" RightToLeft="False" TextField="SUBSYSNAME" Tokens="" ValueField="SUBSYSNO" Width="100%">
                                </dx:ASPxTokenBox>
                                <asp:SqlDataSource ID="SubSystems" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                    SelectCommand="SELECT SUBSYSNO, MAINSYS, rtrim(SUBSYSNAME) as SUBSYSNAME, SUBSYSNAMEL
                                    FROM SUBSYSTEMS WHERE (MAINSYS = @MAINSYS) AND (Branch = dbo.MainCenter())">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="Sys" Name="MAINSYS" Type="Int16"></asp:ControlParameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionStyle Font-Bold="True">
                        </CaptionStyle>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="SUM INSURED" ColCount="4" ColumnCount="4" GroupBoxDecoration="HeadingLine"
                UseDefaultPaddings="False" ColSpan="1">
                <Paddings PaddingTop="10px">
                </Paddings>
                <GroupBoxStyle>
                    <Caption Font-Bold="True" Font-Size="16pt" ForeColor="#006600">
                    </Caption>
                </GroupBoxStyle>
                <Items>
                    <dx:LayoutItem Caption="From Sum Ins." ColSpan="3" ColumnSpan="3">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="SumInsF" runat="server" ClientInstanceName="SumInsF" DisplayFormatString="n3"
                                    RightToLeft="False" SelectInputTextOnClick="True" Text="0">
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionStyle Font-Bold="True">
                        </CaptionStyle>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="To Sum Ins" ColSpan="3" ColumnSpan="3">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="SumInsT" runat="server" ClientInstanceName="SumInsT" DisplayFormatString="n3"
                                    RightToLeft="False" SelectInputTextOnClick="True" Text="0">
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionStyle Font-Bold="True">
                        </CaptionStyle>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Increase By" ColSpan="3" ColumnSpan="3">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="SumInsBy" runat="server" ClientInstanceName="SumInsBy" DisplayFormatString="n3"
                                    RightToLeft="False" SelectInputTextOnClick="True" Text="0">
                                    <ValidationSettings Display="Dynamic">
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
            <dx:LayoutGroup Caption="FOR Period" ColCount="4" ColumnCount="4" GroupBoxDecoration="HeadingLine"
                UseDefaultPaddings="False" ColSpan="1">
                <Paddings PaddingTop="10px">
                </Paddings>
                <GroupBoxStyle>
                    <Caption Font-Bold="True" Font-Size="16pt" ForeColor="#006600">
                    </Caption>
                </GroupBoxStyle>
                <Items>
                    <dx:LayoutItem Caption="To" ColSpan="2" ColumnSpan="2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxDateEdit runat="server" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="yyyy/MM/dd" DisplayFormatString="yyyy/MM/dd"
                                    ID="DTo" RightToLeft="False">
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField IsRequired="True">
                                        </RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionStyle Font-Bold="True">
                        </CaptionStyle>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="From" ColSpan="2" ColumnSpan="2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxDateEdit runat="server" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="yyyy/MM/dd" DisplayFormatString="yyyy/MM/dd"
                                    ID="DFrom" RightToLeft="False">
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField IsRequired="True">
                                        </RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionStyle Font-Bold="True">
                        </CaptionStyle>
                    </dx:LayoutItem>
                    <dx:LayoutItem ColSpan="4" Caption="">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxButton ID="Calculate" ClientInstanceName="Calculate" runat="server" AutoPostBack="False" Text="Calculate Risk Profile">
                                    <ClientSideEvents Click="function OnChanged(s, e) {
                                        s.SetText('CALCULATING RISK PROFILE ONPROCESS... Please Wait the report'); 
                                        s.SetEnabled(false); 
                                        cbp.PerformCallback('CollectData');}" />
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                        <CaptionStyle Font-Bold="True">
                        </CaptionStyle>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
        <SettingsItemHelpTexts HorizontalAlign="Right" Position="Top" />
    </dx:ASPxFormLayout>
    <dx:ASPxPopupControl ID="Popup" ClientInstanceName="PrintPop" runat="server" AllowResize="true" AllowDragging="true" 
        PopupVerticalAlign="WindowCenter" ViewStateMode="Enabled"
            PopupHorizontalAlign="WindowCenter" Modal="True">
        <ClientSideEvents Init="puOnInit" />
    </dx:ASPxPopupControl>

</asp:Content>