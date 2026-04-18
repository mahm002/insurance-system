<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="McWithLayout.aspx.vb" Inherits="McWithLayout" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<%--<%@ Register Src="~/Policy/PolicyUC.ascx" TagPrefix="uc1" TagName="PolicyUC" %>--%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: right;
        }

        .auto-style2 {
            font-size: x-large;
        }
    </style>

    <script>

        var flag = false; // Define global flags properly

        function OnChange(s, e) {
            if (s.GetValue() == null) {
                s.SetValue(0);
            }
            if (scbp.InCallback()) return;
            scbp.PerformCallback('CheckCarIsCovered');
        }

        function END_OnEndCallback(s, e) {
            if (e.result) {
                // Ensure showWarning exists or use alert for testing
                if (typeof showWarning === "function") {
                    showWarning(e.result);
                } else {
                    console.log("Callback Result: " + e.result);
                }
            }
        }

        function cbp_EndCallback(s, e) {
            if (flag) {
                flag = false; // Reset flag to prevent infinite loop
                cbp.PerformCallback();
            }
        }
</script>
    <%--    <script>
        e.result = null;
        function OnChange(s, e) {
            if (s.GetValue() == null) {
                s.SetValue(0);
            }
            else {

            }
            if (scbp.InCallback()) return;
            scbp.PerformCallback('CheckCarIsCovered');
            e.result = null;
        }
        function END_OnEndCallback(s, e) {
            //SumIns.SetValue(e.result);
            if (e.result !== null) {
                //s.SetValue(0);
                //alert(e.result);
                showWarning(e.result);
                e.result = null;
                //alert(checklable.GetValue());
            }
            else {
                //alert(e.result);
                e.result = null;
            }
            e.result = null;

        }
        function cbp_EndCallback(s, e) {
            if (flag) {
                cbp.PerformCallback();
                // Callbackflag = false;
            }
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dx-ac">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents EndCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="0" ShowImage="false" Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dx-al">
                                    <dx:ASPxCallback ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="scbp" OnCallback="ASPxCallbackPanel1_Callback">
                                        <ClientSideEvents CallbackComplete="END_OnEndCallback" />
                                    </dx:ASPxCallback>
                                    <uc1:PolicyControl runat="server" ID="PolicyControl" />
                                    <%--<uc1:PolicyUC runat="server" id="PolicyUC" />--%>
                                </td>
                            </tr>
                        </table>
                        <%--   <dx:aspxcallback id="ASPxCallbackPanel1" clientinstancename="scbp"
                            oncallback="ASPxCallbackPanel1_Callback" runat="server">
                            <clientsideevents callbackcomplete="END_OnEndCallbackMC" />
                        </dx:aspxcallback>--%>

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
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RegularExpression ErrorText=" حروف وأرقام ولا يتجاوز عشرون حرفاً ولا يحتوي مسافات "
                                                                ValidationExpression="^[A-Za-z0-9ا-يءئ\\-]{1,20}$" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionSettings HorizontalAlign="right" ChangeCaptionLocationInAdaptiveMode="True"></CaptionSettings>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="نوع المركبة" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="CarType" runat="server" ClientInstanceName="CarType" CssClass="1" Width="100%">
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
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
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
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
                                        <dx:LayoutItem Caption="رقم الهيكل" CaptionSettings-HorizontalAlign="Right" CaptionSettings-ChangeCaptionLocationInAdaptiveMode="Default">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="BudyNo" runat="server" ClientInstanceName="BudyNo" CssClass="1" Width="100%">
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
                                                            <RegularExpression ErrorText="حروف وأرقام // لا يقل عن أربعة أحرف ولا يتجاوز سبعة عشر حرفاً"
                                                                ValidationExpression="^[A-Za-z0-9\\]{4,17}$" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                        <ClientSideEvents LostFocus="function(s, e) { OnChange(s, e); }" ValueChanged="function(s, e) { OnChange(s, e); }" />
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
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
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
                                                    <dx:ASPxSpinEdit ID="PassNo" runat="server" ClientInstanceName="PassNo" MaxValue="100" Number="4" RightToLeft="True" Width="100%" AllowNull="False">
                                                        <SpinButtons Position="Left" ClientVisible="false">
                                                        </SpinButtons>
                                                        <ClientSideEvents NumberChanged="Validate" />
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
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
                                                    <dx:ASPxSpinEdit ID="Power" runat="server" ClientInstanceName="Power" MaxValue="1800" Number="0" RightToLeft="True" Width="100%" AllowNull="false">
                                                        <SpinButtons Position="Left" ClientVisible="false">
                                                        </SpinButtons>
                                                        <ClientSideEvents NumberChanged="Validate" />
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
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
                                                    <dx:ASPxSpinEdit ID="Carry" runat="server" ClientInstanceName="Carry" DecimalPlaces="1" MaxValue="100" Number="0" RightToLeft="True" Width="100%" AllowNull="false">
                                                        <SpinButtons Position="Left" ClientVisible="false">
                                                        </SpinButtons>
                                                        <ClientSideEvents NumberChanged="Validate" />
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
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
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" ValidationGroup="Data">
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
                                                    <dx:ASPxComboBox ID="PermType" runat="server" ClientInstanceName="PermType"
                                                        ValueType="System.Int32" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                                                        DataSourceID="PermType1" RightToLeft="True" SelectedIndex="0"
                                                        TextField="TPName" ValueField="TPNo" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="ValidateS" />
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" ValidationGroup="Data">
                                                            <RequiredField ErrorText="نوع الترخيص مطلوب" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>

                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="جنسية المركبة" CaptionSettings-HorizontalAlign="right" HelpText="الدولة المسجلة بها وليست الدولة المصنعة" HelpTextStyle-ForeColor="Red">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxComboBox ID="Nation" runat="server" ClientInstanceName="Nation"
                                                        DataSourceID="Nate1" RightToLeft="True" ValueType="System.Int32"
                                                        SelectedIndex="0" TextField="Accessor" ValueField="TPNo" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="ValidateS" />
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom"
                                                            SetFocusOnError="True" ValidationGroup="Data">
                                                            <RequiredField ErrorText="جنسية المركبة مطلوبة" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionSettings HorizontalAlign="right"></CaptionSettings>

                                            <HelpTextStyle ForeColor="Red"></HelpTextStyle>

                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="ملاحظات" CaptionSettings-HorizontalAlign="right">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="Notes" runat="server" ClientInstanceName="Notes" CssClass="1" Text="/" Width="100%">
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
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
                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,3}" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                    <asp:SqlDataSource ID="Nate1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, RTRIM(Accessor)+'ة' As Accessor FROM EXTRAINFO where tp='NatE' ORDER BY TPNo"></asp:SqlDataSource>
                                                    <asp:SqlDataSource ID="PermType1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, TPName FROM EXTRAINFO where tp='carperm' Order By TpNo"></asp:SqlDataSource>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                            <CaptionStyle Font-Bold="True">
                                            </CaptionStyle>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
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
                                                    <%-- <dx:aspxbutton id="issue" runat="server" autopostback="False" clientinstancename="issue"
                                                        enabled="False" text="إصدار" usesubmitbehavior="False">
                                                        <clientsideevents  click="function(s, e) {cbp.PerformCallback('Issue'); }" gotfocus="function(s,e){scbp.PerformCallback('CheckCarIsCovered');}" />
                                                    </dx:aspxbutton>--%>
                                                    <dx:ASPxButton ID="issue" runat="server" AutoPostBack="False"
                                                        ClientInstanceName="issue" Enabled="False" Text="إصدار" UseSubmitBehavior="False" Width="100%">
                                                        <ClientSideEvents Click="function(s, e) {cbp.PerformCallback('Issue'); }"
                                                            GotFocus="function(s,e){scbp.PerformCallback('CheckCarIsCovered');}" />
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