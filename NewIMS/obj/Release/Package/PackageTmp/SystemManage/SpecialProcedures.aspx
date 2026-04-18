<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SpecialProcedures.aspx.vb" Inherits=".SpecialProcedures" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>إجراءات خاصة</title>
    <link href="../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function ReturnToParentPage() {
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup(1);
        }
        function ValidateS(s, e) {
            //if (ASPxClientEdit.ValidateGroup('Data'))
            s.SetText('لإتمام عملية الإلغاء يرجى الضغط على تسجيل');
            s.SetEnabled(false);
            cbp.PerformCallback('Cancel100');
        }
        function ValidateR(s, e) {
            //if (ASPxClientEdit.ValidateGroup('Data'))
            // s.SetText('لإتمام عملية المرتد يرجى الضغط على تسجيل');
            //Issue.SetEnabled(false);
            Issue.SetVisible(false);
            flo.SetVisible(false);
            cbp.PerformCallback('Refund');
        }
        function ValidateC(s, e) {
            //if (ASPxClientEdit.ValidateGroup('Data'))
            s.SetText('تمت عملية الإلغاء');
            s.SetEnabled(false);
            cbp.PerformCallback('ConfirmCancel');

            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup(1);
        }
        function CalcRefund(s, e) {
           //debugger;
           <%-- var Syst = getParameterByName('Sys');
            var Order = document.getElementById('<%=OrderNo.ClientID%>').value;--%>
            if (ASPxClientEdit.ValidateGroup('Data')) {
                var dt = RefundFromDate.GetInputElement().value;
                tmpdate.SetText(dt);
                //RefundFromDate.SetText(dt);
                cbp.PerformCallback('CalcRefund|' + dt);

            }
            else {

                //cbp.PerformCallback('ConfirmRefund');
            }

        }

        function Validate(s, e) {
           //debugger;
           <%-- var Syst = getParameterByName('Sys');
            var Order = document.getElementById('<%=OrderNo.ClientID%>').value;--%>
            if (ASPxClientEdit.ValidateGroup('Data')) {
                var dt = RefundFromDate.GetInputElement().value;
                cbp.PerformCallback('ConfirmRefund|' + dt);

                var parentWindow = window.parent;
                parentWindow.SelectAndClosePopup(1);
            }
            else {

                //cbp.PerformCallback('ConfirmRefund');
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="ArabicForm">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" />
                <SettingsLoadingPanel Text="   تم الاصدار &amp;hellip; " Delay="3" Enabled="true" ShowImage="true" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dxeICC">رقم الوثيقة</td>
                                <td>
                                    <dx:ASPxTextBox ID="PolNo" runat="server" Width="230px" ReadOnly="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al" colspan="2">رقم الطلب</td>
                                <td class="auto-style2">
                                    <dx:ASPxTextBox ID="OrderNo" runat="server" ReadOnly="True" Width="190px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dx-al">رقم الملحق</td>
                                <td>
                                    <dx:ASPxTextBox ID="EndNo" runat="server" Width="170px" ReadOnly="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al" colspan="2">رقم الإشعار</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="LoadNo" runat="server" ReadOnly="True" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">اسم المؤمن له</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="CustName" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">رقمه</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="CustNo" runat="server" ClientEnabled="False" ClientInstanceName="CustNo" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">بتاريخ</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="IssuDate" runat="server" Width="170px" ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">&nbsp;</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="tmpdate" runat="server" ClientInstanceName="tmpdate" ClientVisible="False" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">التغطية من</td>
                                <td colspan="2">
                                    <dx:ASPxDateEdit ID="CoverFrom" runat="server" ClientInstanceName="CoverFrom" ClientEnabled="False"
                                        DisplayFormatString="yyyy/MM/dd" 
                                        EditFormat="Custom" EditFormatString="yyyy/MM/dd" RightToLeft="True" Width="170px">
                                    </dx:ASPxDateEdit>
                                    <%--<dx:ASPxTextBox ID="CoverFrom" runat="server" ClientEnabled="False" Width="170px">
                                    </dx:ASPxTextBox>--%>
                                </td>
                                <td class="dx-al">الى</td>
                                <td colspan="2">
                                    <dx:ASPxDateEdit ID="CoverTo" runat="server" ClientInstanceName="CoverTo" ClientEnabled="False"
                                        DisplayFormatString="yyyy/MM/dd" 
                                        EditFormat="Custom" EditFormatString="yyyy/MM/dd" RightToLeft="True" Width="170px">
                                    </dx:ASPxDateEdit>
                                    <%--<dx:ASPxTextBox ID="CoverTo" runat="server" ClientEnabled="False" Width="170px">
                                    </dx:ASPxTextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="Line">&nbsp;</td>
                                <td colspan="2" class="Line">
                                    <dx:ASPxButton ID="Cancel" runat="server" AutoPostBack="False" ClientInstanceName="Issue" Text="إلغاء الوثيقة 100%">
                                        <ClientSideEvents Click="ValidateS" />
                                    </dx:ASPxButton>
                                </td>
                                <td class="Line">
                                    <dx:ASPxButton ID="Refund" runat="server" AutoPostBack="False" ClientInstanceName="Issue" Text="ملحق مرتد">
                                        <ClientSideEvents Click="ValidateR" />
                                    </dx:ASPxButton>
                                </td>
                                <td class="Line" colspan="2">
                                    <span class="auto-style17"><strong>
                                    <dx:ASPxTextBox ID="SerialNo" runat="server" ClientEnabled="False" 
                                        ClientInstanceName="SerialNo" CssClass="2" DisplayFormatString="n0" Text="0" Width="170px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                    </strong></span></td>
                            </tr>
                            <tr>
                                <td class="Line" colspan="6">
                                    <dx:ASPxFormLayout ID="ASPxFormLayout1" ClientInstanceName="flo" runat="server" ClientVisible="false">
                                        <Items>
                                            <dx:LayoutItem ColSpan="1" Caption="مرتد من تاريخ :">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxDateEdit ID="RefundFrom" runat="server" ClientInstanceName="RefundFromDate" DisplayFormatString="yyyy/MM/dd"
                                                            EditFormat="Custom" EditFormatString="yyyy/MM/dd" RightToLeft="True" Width="110px">
                                                        </dx:ASPxDateEdit>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem ColSpan="1" Caption="مرتد بنسبة %">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxTextBox ID="RefundRatio" runat="server" Text="0" DisplayFormatString="n2">
                                                        </dx:ASPxTextBox>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem ColSpan="1" Caption="الوثيقة البديلة ان وجدت">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxTextBox ID="OldPolicy" runat="server" Text="/">
                                                        </dx:ASPxTextBox>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem Caption="" ColSpan="1">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxCheckBox ID="IssuPrmCalc" runat="server" CheckState="Unchecked" Text="إضافة رسوم إصدار ودمغة محررات">
                                                        </dx:ASPxCheckBox>
                                                        <dx:ASPxButton ID="Calc" runat="server" AutoPostBack="False" ClientInstanceName="Calc" Text="احتساب" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="CalcRefund" />
                                                        </dx:ASPxButton>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                        </Items>
                                    </dx:ASPxFormLayout>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">الصافي </td>
                                <td>
                                    <dx:ASPxTextBox ID="NETPRM" runat="server" Text="0" Width="170px" ClientEnabled="false" DisplayFormatString="n3">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <span class="auto-style17"><strong>
                                        <dx:ASPxTextBox ID="Wakala" runat="server" ClientInstanceName="Wakala" CssClass="2" SelectInputTextOnClick="True" Text="0" Width="54px" ClientVisible="false">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </strong></span>
                                </td>
                                <td colspan="2">
                                    <span class="auto-style17"><strong>
                                        <dx:ASPxTextBox ID="NETPRM1" runat="server" ClientEnabled="False" ClientInstanceName="NETPRM1" CssClass="2" Text="0" Width="108px" DisplayFormatString="n3" ClientVisible="false">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </strong></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">دمغة التصرف</td>
                                <td>
                                    <dx:ASPxTextBox ID="TAXPRM" runat="server" ClientEnabled="False" Text="0" Width="170px" DisplayFormatString="n3">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <dx:ASPxComboBox ID="Currency" runat="server" ClientEnabled="False" ClientInstanceName="Currency" DataSourceID="Cur" DropDownStyle="DropDown" RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" Width="110px" ClientVisible="false">
                                        <ClientSideEvents GotFocus="function(s, e) {  }" SelectedIndexChanged="function(s, e) {
                                                    cbp.PerformCallback('ExRate');  }" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="Br" runat="server" ClientEnabled="False" ClientInstanceName="Br" ClientVisible="False" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">ر.الإشراف وارقابة </td>
                                <td>
                                    <dx:ASPxTextBox ID="CONPRM" runat="server" ClientEnabled="False" Text="0" Width="170px" DisplayFormatString="n3">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <asp:SqlDataSource ID="Cur" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Cur' order by TPNo"></asp:SqlDataSource>
                                </td>
                                <td colspan="2">
                                    <span class="auto-style17"><strong>
                                        <dx:ASPxTextBox ID="Commision" runat="server" ClientEnabled="False" ClientInstanceName="Commision" ClientVisible="False" CssClass="2" DisplayFormatString="n3" Text="0" Width="108px">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                    </dx:ASPxTextBox>
                                        <dx:ASPxTextBox ID="NETPRM2" runat="server" ClientEnabled="False" ClientInstanceName="NETPRM2" CssClass="2" Text="0" Width="108px" DisplayFormatString="n3" ClientVisible="false">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </strong></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">دمغة المحررات</td>
                                <td>
                                    <dx:ASPxTextBox ID="STMPRM" runat="server" ClientEnabled="False" Text="0" Width="170px" DisplayFormatString="n3">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <dx:ASPxButton ID="OKRefund" runat="server" AutoPostBack="False" ClientInstanceName="Confirm" Enabled="False" Text="إصدار">
                                        <ClientSideEvents Click="Validate" />
                                    </dx:ASPxButton>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="PayType" runat="server" AutoPostBack="True" CssClass="PremText" Enabled="False" Visible="False" Width="101px">0</asp:TextBox>
                                    <asp:TextBox ID="lastNet" runat="server" AutoPostBack="True" CssClass="PremText" Enabled="False" Visible="False" Width="101px">0</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">مصاريف إصدار</td>
                                <td>
                                    <dx:ASPxTextBox ID="ISSPRM" runat="server" ClientEnabled="False" Text="0" Width="170px" DisplayFormatString="n3">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="AccountNo" runat="server" ClientEnabled="False" ClientInstanceName="AccountNo" CssClass="1" Text="0" Width="110px" 
                                        ClientVisible="false">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <span class="auto-style17"><strong>
                                    <dx:ASPxTextBox ID="CommisionType" runat="server" ClientEnabled="False" 
                                        ClientInstanceName="CommisionT" ClientVisible="False" CssClass="2" DisplayFormatString="n3"
                                        Text="0" Width="108px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                    </strong></span>
                                    <dx:ASPxTextBox ID="ExcRate" runat="server" ClientEnabled="False"
                                        ClientInstanceName="ExcRate" ClientVisible="False" CssClass="2" DisplayFormatString="n3"
                                        Text="0" Width="108px">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="SerialNo" runat="server" AutoPostBack="True" CssClass="PremText" Enabled="False" Visible="False" Width="101px">0</asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">الإجمالي المستحق</td>
                                <td>
                                    <dx:ASPxTextBox ID="TOTPRM" runat="server" Text="0" Width="170px" Font-Bold="True" ForeColor="Red" 
                                        ClientEnabled="false" DisplayFormatString="n3">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="4">
                                    <dx:ASPxButton ID="OK" runat="server" AutoPostBack="False"
                                        ClientInstanceName="Confirm" Text="تسجيل" Enabled="false">
                                        <ClientSideEvents Click="ValidateC" />
                                    </dx:ASPxButton>
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