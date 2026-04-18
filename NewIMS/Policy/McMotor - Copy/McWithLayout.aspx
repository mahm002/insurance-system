<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="McWithLayout.aspx.vb" Inherits="McWithLayout" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dx-ac">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents EndCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="10" ShowImage="false" Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dx-al">
                                    <uc1:PolicyControl runat="server" ID="PolicyControl" />
                                </td>
                            </tr>
                        </table>
                        <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout1" RightToLeft="true" ColCount="4" AlignItemCaptionsInAllGroups="True" LeftAndRightCaptionsWidth="100">
                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700">
                                <GridSettings StretchLastItem="True">
                                </GridSettings>
                            </SettingsAdaptivity>
                            <Items>
                                <dx:LayoutGroup Caption="بيانات المركبة" ColCount="4" GroupBoxDecoration="HeadingLine" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                                    <Paddings PaddingTop="10px"></Paddings>
                                    <GroupBoxStyle>
                                        <Caption Font-Bold="true" Font-Size="16" />
                                    </GroupBoxStyle>
                                    <Items>
                                        <dx:LayoutItem Caption="رقم اللوحة">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="TableNo" runat="server" ClientInstanceName="TableNo" CssClass="1" Width="100%">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءئ \\-]+" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right" ChangeCaptionLocationInAdaptiveMode="True"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="رقم الهيكل" CaptionSettings-HorizontalAlign="Right" CaptionSettings-ChangeCaptionLocationInAdaptiveMode="Default">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="BudyNo" runat="server" ClientInstanceName="BudyNo" CssClass="1" Width="100%">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RegularExpression ErrorText="حروف وأرقام" ValidationExpression="^[A-Za-z0-9 \\-]+" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="نوع المركبة" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="CarType" runat="server" ClientInstanceName="CarType" CssClass="1" Width="100%">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RegularExpression ErrorText="حروف وأرقام" ValidationExpression="^[A-Za-z0-9ا-يءأئ ى \\%/()-.]+" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="اللون" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="CarColor" runat="server" ClientInstanceName="CarColor" CssClass="1" Width="100%">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RegularExpression ErrorText="حروف فقط" ValidationExpression="^[A-Za-z0-9ا-يءأئ ى \\%/()-.]+" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="سنة الصنع" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="MadeYear" runat="server" ClientInstanceName="MadeYear" CssClass="3" Width="100%">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="عدد الركاب" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxSpinEdit ID="PassNo" runat="server" ClientInstanceName="PassNo" MaxValue="100" Number="4" RightToLeft="True" Width="100%">
                                                        <SpinButtons Position="Left" ShowLargeIncrementButtons="True">
                                                        </SpinButtons>
                                                        <ClientSideEvents NumberChanged="ValidateS" />
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="القوة بالحصان" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxSpinEdit ID="Power" runat="server" ClientInstanceName="Power" MaxValue="1800" Number="0" RightToLeft="True" Width="100%">
                                                        <SpinButtons Position="Left" ShowLargeIncrementButtons="True">
                                                        </SpinButtons>
                                                        <ClientSideEvents NumberChanged="ValidateS" />
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="الحمولة بالطن" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxSpinEdit ID="Carry" runat="server" ClientInstanceName="Carry" DecimalPlaces="1" MaxValue="100" Number="0" RightToLeft="True" Width="100%">
                                                        <SpinButtons Position="Left" ShowLargeIncrementButtons="True">
                                                        </SpinButtons>
                                                        <ClientSideEvents NumberChanged="ValidateS" />
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="جهة الترخيص" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="PermZone" runat="server" ClientInstanceName="PermZone" CssClass="1" Text="طرابلس" Width="100%">
                                                        <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                            <RegularExpression ErrorText="حروف وأرقام" ValidationExpression="^[A-Za-zا-يءئ \\-]+" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="نوع الترخيص" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxComboBox ID="PermType" runat="server" ClientInstanceName="PermType" DataSourceID="PermType1" RightToLeft="True" SelectedIndex="0" TextField="TPName" ValueField="TPNo" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="ValidateS" />
                                                        <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                                            <RequiredField ErrorText="نوع الترخيص مطلوب" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="جنسية المركبة" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxComboBox ID="Nation" runat="server" ClientInstanceName="Nation" DataSourceID="Nate1" RightToLeft="True" SelectedIndex="0" TextField="Accessor" ValueField="TPNo" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="ValidateS" />
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RequiredField ErrorText="جنسية المركبة مطلوبة" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="ملاحظات" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="Notes" runat="server" ClientInstanceName="Notes" CssClass="1" Text="/" Width="100%">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RegularExpression ErrorText="*" ValidationExpression="^[A-Za-z0-9ا/-يءئ \\-]+" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="Premium" runat="server" ClientEnabled="False" ClientInstanceName="Premium" ClientVisible="False" CssClass="2" DisplayFormatString="n3" Text="0" Width="110px">
                                                        <ClientSideEvents TextChanged="Validate" />
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,3}" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                    <asp:SqlDataSource ID="Nate1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, RTRIM(Accessor)+'ة' As Accessor FROM EXTRAINFO where tp='NatE' ORDER BY TPNo"></asp:SqlDataSource>
                                                    <asp:SqlDataSource ID="PermType1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='carperm' Order By TpNo"></asp:SqlDataSource>
                                                    <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="ValidateS" />
                                                    </dx:ASPxButton>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption=" يمكنك الإصدار من هنا في حال التأكد من صحة البيانات">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxButton ID="issue" runat="server" AutoPostBack="False" ClientInstanceName="issue" Enabled="False" Text="إصدار" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {
                                                                cbp.PerformCallback('Issue'); }" />
                                                    </dx:ASPxButton>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                    </Items>

                                    <SettingsItemCaptions ChangeCaptionLocationInAdaptiveMode="True" HorizontalAlign="right" />
                                    <SettingsItems HorizontalAlign="Right" ShowCaption="True" VerticalAlign="Top" />
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>