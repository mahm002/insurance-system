<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TreatyEntry.aspx.vb" Inherits="TreatyEntry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0" />
    <script type="text/javascript">
        $('body').on('keydown', 'input, select', function (e) {
            //debugger;
            if (e.key === "Enter") {
                e.preventDefault();
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
        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }
        function OnInit(s, e) {
            s.InsertItem(0, 'Add New Item', -1);
        }
        // show a popup to edit a combo box datasource
        function OnComboRe_ButtonClick(s, e) {
            popup.ShowAtElement(s.GetMainElement());
        }
        // clear the combo box selection
        function OnComboRe_EndCallback(s, e) {
            s.SetSelectedIndex(-1);
        }
        var command = "";
        function OngridReins_BeginCallback(s, e) {
            command = e.command;
        }
        // update the combo box datasource
        function OngridReins_EndCallback(s, e) {
            if (command == "UPDATEEDIT") {
                ComboRe.PerformCallback();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback" RightToLeft="False">
                <ClientSideEvents BeginCallback="" />
                <SettingsLoadingPanel Text="حفظ&amp;hellip;" Delay="100" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <asp:SqlDataSource ID="CoverTypes" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                            SelectCommand="SELECT CoverTP, rtrim(CoverName) as CoverName, SubSystem, CoverNo,SysPrevi,
                                                concat(cast(rtrim(SubSystem) as nvarchar) , iif(len(cast(rtrim(CoverNo) as nvarchar))=1,'0',''),cast(rtrim(CoverNo) as nvarchar)) AS TRT
                                                FROM Covers
                                                WHERE (CoverTP IS NOT NULL) and (CoverTP=1)
                                                Order by SubSystem"></asp:SqlDataSource>

                        <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout1" CssClass="formLayout" RightToLeft="False" Theme="Office365">
                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
                            <Items>
                                <dx:LayoutGroup Caption="TREATY DETAILS" ColCount="4" GroupBoxDecoration="HeadingLine" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                                    <Paddings PaddingTop="10px"></Paddings>

                                    <GroupBoxStyle>
                                        <Caption Font-Bold="true" Font-Size="16" ForeColor="#006600" />
                                    </GroupBoxStyle>
                                    <Items>
                                        <dx:LayoutItem Caption="Insurance Cover">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxComboBox ID="Covers" runat="server" ClientInstanceName="Covers" DataSourceID="CoverTypes" NullText="Select Insurance Type" TextField="CoverName" ValueField="TRT" RightToLeft="False">
                                                        <ClientSideEvents SelectedIndexChanged="function OnChanged(s, e) {
                                                                            //debugger;
                                                                            var tyear= getParameterByName('Year');
                                                                            var getTr = s.GetValue();
                                                                            var trtNo= getTr + tyear
                                                                            TreatyNo.SetValue(trtNo);
                                                                            cbp.PerformCallback('GetData');
                                                                            //alert(trtNo);
                                                                            }" />
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Treaty No.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="TreatyNo" runat="server" ClientEnabled="false" ClientInstanceName="TreatyNo" RightToLeft="False">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Inception Date">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxDateEdit runat="server" ID="TRINSDTE" ClientInstanceName="TRINSDTE" Date="2021-01-01" RightToLeft="False">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Expiry Date">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxDateEdit runat="server" ID="TREXPDTE" ClientInstanceName="TREXPDTE" Date="2021-12-31" RightToLeft="False">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Entry Date">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxDateEdit runat="server" Date="2021-01-01" RightToLeft="False" ClientInstanceName="TRSYSDTE" ID="TRSYSDTE">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Treaty Description">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox runat="server" ID="Descrip" Text="/" RightToLeft="False">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Pertfolio Type">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxComboBox ID="Portfolio" runat="server" NullText="Select Portfoilio Type" NullTextDisplayMode="UnfocusedAndFocused" RightToLeft="False">
                                                        <Items>
                                                            <dx:ListEditItem Text="Clean Cut" Value="1" />
                                                            <dx:ListEditItem Text="Under Writing Year" Value="2" />
                                                        </Items>
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Accounts Type">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxComboBox ID="AccType" runat="server" NullText="Select Accounts Type" NullTextDisplayMode="UnfocusedAndFocused" RightToLeft="False">
                                                        <Items>
                                                            <dx:ListEditItem Text="Monthly" Value="1" />
                                                            <dx:ListEditItem Text="Quarterly" Value="2" />
                                                            <dx:ListEditItem Text="Annually" Value="3" />
                                                        </Items>
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Reserve Ratio">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox runat="server" ID="ReserveR" Text="0" RightToLeft="False" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Interest On Reserve">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox runat="server" ID="InterestRRes" Text="0" RightToLeft="False" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Treaty Capacity">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox runat="server" ID="TRCAPCTY" Text="0" RightToLeft="False" DisplayFormatString="##,#00.000" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem ColSpan="1" Caption="">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxButton ID="Save" runat="server" AutoPostBack="False"  Text="SAVE" Width="100%">
                                                        <ClientSideEvents Click="function OnChanged(s, e) {
                                                                                cbp.PerformCallback('SaveData');
                                                                            }" />
                                                    </dx:ASPxButton>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                                <dx:LayoutGroup Caption="TREATY LIMITS" ColCount="4" GroupBoxDecoration="Box" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                                    <Paddings PaddingTop="10px"></Paddings>

                                    <GroupBoxStyle>
                                        <Caption Font-Bold="true" Font-Size="16" ForeColor="#333399" />
                                    </GroupBoxStyle>
                                    <Items>
                                        <dx:LayoutItem ColSpan="4" Caption="Retintion">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="Ret" runat="server" RightToLeft="False" Text="0" Width="100%" DisplayFormatString="##,#00.000" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>

                                        <dx:LayoutItem ColSpan="1" Caption="Q.S">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="QS" runat="server" RightToLeft="False" Text="0" DisplayFormatString="##,#00.000" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem ColSpan="1" Caption="Q.S Comm.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="QSCom" runat="server" ClientInstanceName="QSCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem ColSpan="1" Caption="Q.S Leader Comm">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="LQSCom" runat="server" ClientInstanceName="LQSCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Q.S War Comm.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="WQSCom" runat="server" ClientInstanceName="WQSCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="1ST Surp.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="FSup" runat="server" RightToLeft="False" Text="0" DisplayFormatString="##,#00.000" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="1ST Surp. Comm.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="FSupCom" runat="server" ClientInstanceName="FSupCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="1ST Surp. Leader Comm.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="LFSupCom" runat="server" ClientInstanceName="LFSupCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="1ST Surp. War Comm.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="WFSupCom" runat="server" ClientInstanceName="WFSupCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="2ND Surp.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="SSup" runat="server" RightToLeft="False" ClientInstanceName="SSup" Text="0" DisplayFormatString="##,#00.000" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="2ND Surp. Comm.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="SSupCom" runat="server" ClientInstanceName="SSupCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="2ND Surp. Leader Comm.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="LSSupCom" runat="server" ClientInstanceName="LSSupCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Line Slip" ColSpan="2">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="LS" runat="server" ClientInstanceName="LS" RightToLeft="False" Text="0" DisplayFormatString="##,#00.000" SelectInputTextOnClick="true">
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Line Slip Comm." ColSpan="2">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="LSCom" runat="server" ClientInstanceName="LSCom" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Local Acceptance Ratio %(Capacity)" CaptionStyle-ForeColor="Green" ColSpan="2">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="LAR" runat="server" ClientInstanceName="LAR" RightToLeft="False" Text="0" DisplayFormatString="n2" SelectInputTextOnClick="true">
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                                <dx:LayoutGroup Caption="REINSURANCE PARTICIPANTS" ColCount="4" GroupBoxDecoration="Box" UseDefaultPaddings="false" Paddings-PaddingTop="10" ColumnCount="4">
                                    <Paddings PaddingTop="10px"></Paddings>
                                    <GroupBoxStyle>
                                        <Caption Font-Bold="true" Font-Size="16" ForeColor="#333399" />
                                    </GroupBoxStyle>
                                    <Items>
                                        <dx:LayoutItem ColSpan="4" Caption="">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxGridView ID="ParticipantsGrid" runat="server" Width="100%" DataSourceID="Participants" RightToLeft="False" KeyFieldName="Id" AutoGenerateColumns="False">
                                                        <SettingsPopup>
                                                            <EditForm HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" AllowResize="True" Modal="True"></EditForm>
                                                            <FilterControl AutoUpdatePosition="False">
                                                            </FilterControl>
                                                        </SettingsPopup>
                                                        <SettingsPopup EditForm-HorizontalAlign="WindowCenter" EditForm-VerticalAlign="WindowCenter" EditForm-AllowResize="true" EditForm-Modal="true"></SettingsPopup>
                                                        <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="4" />
                                                        <SettingsText CommandNew="Add New Participant" CommandEdit="تعديل" CommandUpdate="حفظ" CommandCancel="تراجع" CommandDelete="حذف" PopupEditFormCaption="Participant Data" />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn VisibleIndex="6" ShowNewButtonInHeader="True" ShowDeleteButton="true" />
                                                            <dx:GridViewDataColumn FieldName="Id" VisibleIndex="0" Visible="false" />
                                                            <dx:GridViewDataColumn FieldName="TREATYNO" VisibleIndex="0" Visible="false" />
                                                            <dx:GridViewDataTextColumn FieldName="TRSHACOM" VisibleIndex="2" Caption="Share %" PropertiesTextEdit-DisplayFormatString="{0} %" CellStyle-HorizontalAlign="left">
                                                                <PropertiesTextEdit DisplayFormatString="{0} %">
                                                                </PropertiesTextEdit>
                                                                <CellStyle HorizontalAlign="Right">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataCheckColumn FieldName="IsLeader" VisibleIndex="3" Caption="Leaders">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataCheckColumn>

                                                            <dx:GridViewDataCheckColumn FieldName="War" VisibleIndex="4" Caption="War">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataCheckColumn>

                                                            <dx:GridViewDataComboBoxColumn FieldName="TRREINSCO" Caption="Reinsurer" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Left" VisibleIndex="1">
                                                                <PropertiesComboBox EnableSynchronization="false" IncrementalFilteringMode="StartsWith" ClientInstanceName=""
                                                                    DataSourceID="Reinsurers" ValueField="TPNo" TextField="ReinsurerName" ValueType="System.Int32" DataSecurityMode="Strict">
                                                                    <Buttons>
                                                                        <dx:EditButton>
                                                                            <Image Url="../Content/Images/new_copy_16px.png" AlternateText="New" />
                                                                        </dx:EditButton>
                                                                    </Buttons>
                                                                    <ClientSideEvents ButtonClick="OnComboRe_ButtonClick" EndCallback="OnComboRe_EndCallback" />
                                                                </PropertiesComboBox>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                            </dx:GridViewDataComboBoxColumn>
                                                        </Columns>
                                                        <Settings ShowGroupPanel="true" ShowFooter="true" />
                                                        <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="TRSHACOM" SummaryType="Sum" />
                                                        </TotalSummary>
                                                    </dx:ASPxGridView>
                                                    <dx:ASPxPopupControl HeaderText="Reinsurance Participants" EncodeHtml="False" ID="popup" runat="server"
                                                        ClientInstanceName="popup" EnableViewState="False" AllowDragging="true" PopupHorizontalAlign="OutsideRight"
                                                        PopupHorizontalOffset="10" EnableClientSideAPI="true">
                                                        <ContentCollection>
                                                            <dx:PopupControlContentControl>
                                                                <dx:ASPxGridView ID="gridReins" ClientInstanceName="gridReins" runat="server"
                                                                    DataSourceID="Reinsurers" KeyFieldName="TPNo" Width="300px" AutoGenerateColumns="False">
                                                                    <Columns>

                                                                        <dx:GridViewCommandColumn VisibleIndex="0" ShowNewButton="True" ShowEditButton="true" />
                                                                        <dx:GridViewDataColumn FieldName="ReinsurerName" VisibleIndex="0">
                                                                        </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="TPNo" VisibleIndex="2" Visible="false">
                                                                        </dx:GridViewDataColumn>
                                                                    </Columns>
                                                                    <SettingsEditing Mode="Inline" />
                                                                    <SettingsBehavior AllowDragDrop="false" />
                                                                    <ClientSideEvents BeginCallback="OngridReins_BeginCallback" EndCallback="OngridReins_EndCallback" />
                                                                </dx:ASPxGridView>
                                                            </dx:PopupControlContentControl>
                                                        </ContentCollection>
                                                    </dx:ASPxPopupControl>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                        <asp:SqlDataSource ID="Reinsurers" runat="server" ProviderName="System.Data.SqlClient" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                            SelectCommand="SELECT rtrim(TPName) As ReinsurerName,TPNo FROM ExtraInfo WHERE tp='ReCom' ORDER BY TpNo"
                            InsertCommand="NewReinsurer"
                            InsertCommandType="StoredProcedure"
                            UpdateCommand="Update ExtraInfo set TpName=@ReinsurerName WHERE tp='ReCom' and TpNo=@TpNo">
                            <InsertParameters>
                                <asp:Parameter Name="ReinsurerName" Type="String"></asp:Parameter>
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="ReinsurerName" Type="String" />
                                <asp:Parameter Name="TpNo" Type="Int16" />
                            </UpdateParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="Participants" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                            InsertCommand="NewParticipant"
                            InsertCommandType="StoredProcedure"
                            SelectCommand="SELECT Id, dbo.GetExtraCatName(N'ReCom', TRREINSCO) AS TRREINSCO, TRSHACOM, IsLeader,War FROM TRSECFLE WHERE (RTRIM(TREATYNO) = @Treaty)"
                            CancelSelectOnNullParameter="False"
                            DeleteCommand="DELETE FROM TRSECFLE WHERE (Id = @Id)">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="0" Name="Treaty" QueryStringField="Year" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="TRREINSCO" Type="Int16" />
                                <asp:QueryStringParameter Name="TREATYNO" QueryStringField="Year" />
                                <asp:Parameter Name="TRSHACOM" Type="Double" />
                                <asp:Parameter Name="IsLeader" Type="Boolean" DefaultValue="False" />
                                <asp:Parameter Name="War" Type="Boolean" DefaultValue="False" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="Id"></asp:Parameter>
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>