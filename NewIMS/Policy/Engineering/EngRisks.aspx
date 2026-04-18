<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EngRisks.aspx.vb" Inherits="EngRisks" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">

        function OnChange(s, e) {
            
            if (s.GetValue() == null) {
                s.SetValue(0);
            }
            if (scbp.InCallback()) return;
            scbp.PerformCallback('SuminsCHANGE');
        }

        function END_OnEndCallback(s, e) {
            SumIns.SetValue(e.result);
        }
    </script>
<style type="text/css">
    .auto-style1 {
        text-align: right;
    }

    .auto-style2 {
        font-size: xx-small;
    }

    .auto-style5 {
        text-align: left;
    }

    .auto-style7 {
        font-size: 11px;
        text-align: left;
    }

    .auto-style9 {
        font-size: larger;
        text-align: left;
    }

    .auto-style10 {
        text-align: center;
        vertical-align: top;
    }

    .auto-style11 {
        font-size: larger;
    }
</style>
</head>
<body>
 <form id="form1" runat="server">
    <div>
        <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
            <%--<ClientSideEvents BeginCallback=""  />--%>
            <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="0" />
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
                        <tr>
                            <td>
                                <uc1:PolicyControl runat="server" ID="PolicyControl" />
                            </td>
                       
                        </tr>
                        <tr>
                            <td class="dx-al">

                                <table dir="rtl" style="width: 100%;">
                                    <tr>
                                        <td>
                                            <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين" UseSubmitBehavior="False" Width="100%">
                                                <ClientSideEvents Click="function(s, e) {OnChange;
                                                                cbp.PerformCallback('Calc');
                                                                scbp.PerformCallback('SuminsCHANGE');
                                                    }" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td class="auto-style9"><strong>صاحب العمل</strong></td>
                                        <td class="auto-style9" colspan="2">
                                            <dx:ASPxTextBox runat="server" Width="100%" ClientInstanceName="Owner" CssClass="1" ID="Owner">
                                                <ClientSideEvents ValueChanged="Validate"></ClientSideEvents>

                                                <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                    <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+"></RegularExpression>

                                                    <RequiredField IsRequired="True"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style9"><strong>بيان عقد المقاولة</strong></td>
                                        <td class="auto-style9" colspan="2">
                                            <dx:ASPxTextBox runat="server" Width="100%" ClientInstanceName="ProjectDesc" CssClass="1" ID="ProjectDesc">
                                                <ClientSideEvents ValueChanged="Validate"></ClientSideEvents>

                                                <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                    <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+"></RegularExpression>

                                                    <RequiredField IsRequired="True"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style9">رقم العقد</td>
                                        <td class="auto-style9" colspan="2">
                                            <dx:ASPxTextBox ID="ContractNo" runat="server" ClientInstanceName="ContractNo" CssClass="1" Width="100%">
                                                <ClientSideEvents ValueChanged="Validate" />
                                                <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                    <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style9"><strong>موقع الإنشاء</strong></td>
                                        <td class="auto-style9" colspan="2">
                                            <dx:ASPxTextBox runat="server" Width="100%" ClientInstanceName="WorkPlace" CssClass="1" ID="WorkPlace">
                                                <ClientSideEvents ValueChanged="Validate"></ClientSideEvents>

                                                <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                    <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+"></RegularExpression>

                                                    <RequiredField IsRequired="True"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1" colspan="3">
                                            <dx:ASPxCallback ID="ASPxCallbackPanel1" ClientInstanceName="scbp" OnCallback="ASPxCallbackPanel1_Callback" runat="server">
                                                <ClientSideEvents CallbackComplete="END_OnEndCallback" />
                                            </dx:ASPxCallback>
                                            <dx:ASPxRoundPanel ID="ASPxPanel7" runat="server" AllowCollapsingByHeaderClick="True" HeaderText="مدة التغطية" LoadContentViaCallback="True"
                                                RightToLeft="True" Width="100%" ShowCollapseButton="True">
                                                <PanelCollection>
                                                    <dx:PanelContent runat="server">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td class="auto-style1"><strong>فترة التخزين</strong></td>
                                                                <td class="auto-style1"><strong>فترة التشييد</strong></td>
                                                                <td class="auto-style1"><strong>فترة التجارب</strong></td>
                                                                <td class="auto-style1"><strong>فترة الصيانة</strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="StockStart" runat="server" Caption="من" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="100%">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                        </ValidationSettings>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="WorkStart" runat="server" Caption="من" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="100%">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                        </ValidationSettings>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="TestStart" runat="server" Caption="من" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="100%">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                        </ValidationSettings>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="MentStart" runat="server" Caption="من" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="100%">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                        </ValidationSettings>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="StockEnd" runat="server" Caption="إلى" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="100%">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                        </ValidationSettings>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="WorkEnd" runat="server" Caption="إلى" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="100%">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                        </ValidationSettings>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="TestEnd" runat="server" Caption="إلى" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="100%">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                        </ValidationSettings>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxDateEdit ID="MentEnd" runat="server" Caption="إلى" DisplayFormatString="yyyy/MM/dd" NullText="1900/01/01" Width="100%">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                        </ValidationSettings>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1" colspan="3">
                                            <dx:ASPxRoundPanel ID="ASPxPanel5" runat="server" AllowCollapsingByHeaderClick="True" RightToLeft="True" LoadContentViaCallback="true" HeaderText="القسم الأول (الأضرار المادية)" Width="100%" ShowCollapseButton="True">
                                                <PanelCollection>
                                                    <dx:PanelContent runat="server">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td  class="auto-style10" colspan="4"><strong>&nbsp;1-أعمال عقد المقاولة
                                                                    <br />
                                                                    (الأعمال المستديمة والمؤقتة بما في ذلك جميع المواد المستخدمة بها) </strong></td>
                                                                <td class="auto-style10" rowspan="2"><strong><span>2-معدات التشييد</span></strong></td>

                                                                <td class="auto-style10" rowspan="2"><strong>3-آليات الإنشاء<br /> &nbsp;(حسب القائمة المرفقة)</strong></td>
                                                                <td class="auto-style10" rowspan="2"><strong>4 -ممتلكات صاحب العمل </strong></td>

                                                                <td class="auto-style10" rowspan="2"><strong><span>5-تكاليف إزالة الحطام والركام<br /> (الحد الأقصى للتعويض)</span></strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style10" dir="rtl" >1.1 قيمة العقد المقاول</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td class="auto-style10" >1.2&nbsp;مواد يوردها صاحب العمل </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style5" colspan="2">
                                                                    <dx:ASPxTextBox ID="ProjectPdG" runat="server" ClientInstanceName="ProjectPdG" CssClass="2"
                                                                        DisplayFormatString="n3" Text="0" Width="100%">
                                                                        <ClientSideEvents LostFocus="function(s, e) { OnChange(s, e); }" ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td class="auto-style5" colspan="2">
                                                                    <dx:ASPxTextBox ID="ProjectTool" runat="server" ClientInstanceName="ProjectTool" CssClass="2" DisplayFormatString="n3" Text="0" Width="100%">
                                                                        <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="ProjectMachine" runat="server" ClientInstanceName="ProjectMachine" CssClass="2" DisplayFormatString="n3" Text="0" Width="100%">
                                                                        <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="Machines" runat="server" ClientInstanceName="Machines" CssClass="2" DisplayFormatString="n3" Text="0" Width="100%">
                                                                        <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="OwnerProperties" runat="server" ClientInstanceName="OwnerProperties" CssClass="2" DisplayFormatString="n3" Text="0" Width="100%">
                                                                        <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="DebrisRubble" runat="server" ClientInstanceName="DebrisRubble" CssClass="2" DisplayFormatString="n3" Text="0" Width="100%">
                                                                        <ClientSideEvents ValueChanged="function(s, e) { OnChange(s, e); }" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style2" colspan="2">&nbsp;</td>
                                                                <td colspan="2">&nbsp;</td>
                                                                <td class="auto-style5"><strong>إجمالي مبالغ تأمين القسم الأول</strong></td>
                                                                <td colspan="2">
                                                                    <dx:ASPxTextBox ID="SumIns" runat="server" ClientEnabled="False" ClientInstanceName="SumIns" CssClass="2" DisplayFormatString="n3" Text="0" Width="100%">
                                                                        <ClientSideEvents TextChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1" colspan="3">
                                            <dx:ASPxRoundPanel ID="ASPxPanel3" runat="server" AllowCollapsingByHeaderClick="True" RightToLeft="True" LoadContentViaCallback="true" HeaderText="القسم الثاني ( المسؤولية المدنية)" Width="100%" ShowCollapseButton="True">
                                                <PanelCollection>
                                                    <dx:PanelContent runat="server">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td class="auto-style1" colspan="4"><strong>1- الحد الأقصى للمسؤولية تجاه أضرار وخسائر الطرف الثالث (عن الحادث الواحد أو سلسلة من الحوادث الناشئة عن سبب واحد)</strong></td>
                                                                <td class="auto-style1" colspan="2"><strong>2-الحد الأقصى التجميعي للمسؤولية الشركة ( للإصابات البدنية والأضرار المادية خلال مدة التأمين ومهمة بلغ عدد الحوادث )</strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td dir="rtl" class="auto-style5">الإصابات البدنية 1.1</td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="CivWork1" runat="server" ClientInstanceName="CivWork1" CssClass="2" Text="0" Width="109px" DisplayFormatString="n3">
                                                                      <%--  <ClientSideEvents LostFocus="LocalExRateCall"  />--%>
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td dir="rtl" class="auto-style5">الأضرار المادية 1.2</td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="CivWork2" runat="server" ClientInstanceName="CivWork2" CssClass="2"  Text="0" Width="100%" DisplayFormatString="n3">
                                                                       <%-- <ClientSideEvents LostFocus="LocalExRateCall"  />--%>
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="MaxCivWork" runat="server" ClientInstanceName="MaxCivWork" CssClass="2" Text="0" Width="100%" DisplayFormatString="n3">
                                                                       <%-- <ClientSideEvents LostFocus="LocalExRateCall" />--%>
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ErrorText="أرقام فقط" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1" colspan="3">
                                            <dx:ASPxRoundPanel ID="ASPxPanel1" runat="server" AllowCollapsingByHeaderClick="True" RightToLeft="True" LoadContentViaCallback="true" HeaderText="حدود التحمل (القسم الأول)" Width="100%" ShowCollapseButton="True">
                                                <PanelCollection>
                                                    <dx:PanelContent runat="server">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td class="auto-style1" colspan="2">
                                                                    <strong>1 المبلغ الذي يتحمله المؤمن له ويخصم من التعويض,( لكل خسارة علي حده فيما يخص الأعمال موضوع العقد) (البند الأول من القسم الأول)</strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style5">1.1 حوادث القضاء والقدر بما في ذلك الزلازل، العواصف ، الدوامات الأعاصير والفيضان وغمر المياه، تهدم أو انهيار أو هبوط أو ترييح البناء انزلاق الأرض أو الأضرار الناتجة عن المياه مهما كان منشأها.</td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="WorkResValA" runat="server" ClientInstanceName="WorkResValA" CssClass="1" Width="170px" Text="غير مغطى">
                                                                       <%-- <ClientSideEvents ValueChanged="Validate" />--%>
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style5">2.1 الحوادث التي ترجع لأسباب أخري غير تلك الواردة أعلاه</td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="WorkResValB" runat="server" ClientInstanceName="WorkResValB" CssClass="1" Width="170px" Text="غير مغطى">
                                                                        <%--<ClientSideEvents ValueChanged="Validate" />--%>
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style1" colspan="2"><strong>2 المبلغ الذي يتحمله المؤمن له ويخصم من التعويض ( لكل خسارة علي حده فيما يخص معدات وآلات التشييد) ( البندين الثاني والثالث من القسم الأول)</strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style5">1.2 حوادث القضاء والقدر بما في ذلك الزلازل، العواصف ، الدوامات الأعاصير والفيضان وغمر المياه، تهدم أو انهيار أو هبوط أو ترييح البناء انزلاق الأرض أو الأضرار الناتجة عن المياه مهما كان منشأها</td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="MachinResValA" runat="server" ClientInstanceName="MachinResValA" CssClass="1" Width="170px" Text="غير مغطى">
                                                                        <%--<ClientSideEvents ValueChanged="Validate" />--%>
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style5">2.2 الحوادث التي ترجع لأسباب أخري غير تلك الواردة أعلاه</td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="MachinResValB" runat="server" ClientInstanceName="MachinResValB" CssClass="1" Width="170px" Text="غير مغطى">
                                                                        <%--<ClientSideEvents ValueChanged="Validate" />--%>
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1" colspan="3">
                                            <dx:ASPxRoundPanel ID="ASPxPanel6" runat="server" AllowCollapsingByHeaderClick="True" RightToLeft="True" LoadContentViaCallback="true" HeaderText="حدود التحمل( القسم الثاني)" Width="100%" ShowCollapseButton="True">
                                                <PanelCollection>
                                                    <dx:PanelContent runat="server">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td class="auto-style1" colspan="2">
                                                                    <strong>3 المبلغ الذي يتحمله المؤمن له ويخصم من التعويض (لكل خسارة علي حده فيما يخص المسؤولية المدنية)</strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style5">1.3 بالنسبة للأضرار البدنية التي تلحق بالغير</td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="MaxBodyResp" runat="server" ClientInstanceName="MaxBodyResp" CssClass="1" Width="270px" Text="غير مغطى">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style5">2.3 النسبة لإضرار ممتلكات الغير</td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="MaxPropResp" runat="server" ClientInstanceName="MaxPropResp" CssClass="1" Width="270px" Text="غير مغطى">
                                                                        <ClientSideEvents ValueChanged="Validate" />
                                                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="Data">
                                                                            <RegularExpression ValidationExpression="^[A-Za-z0-9ا./-,يءئ \\-]+" />
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            الشروط الخاصة :</td>
                                        <td>
                                            <dx:ASPxTokenBox ID="SpcCond" runat="server" AllowMouseWheel="True" CallbackPageSize="10" RightToLeft="True" TextSeparator="-" Tokens="" ValueSeparator="-" Width="100%">
                                                <ClientSideEvents TokensChanged="function(s, e) { }" />
                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                </ValidationSettings>
                                            </dx:ASPxTokenBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><strong>الملاحق الإلزامية والتغطيات الإضافية</strong></td>
                                        <td>
                                            <dx:ASPxTokenBox ID="Conds" runat="server" AllowCustomTokens="False" AllowMouseWheel="True" CallbackPageSize="10" DataSourceID="EConds" RightToLeft="True" TextField="Condition" Tokens="" ValueField="CondNo" Width="100%">
                                                <ClientSideEvents TokensChanged="Validate" />
                                            </dx:ASPxTokenBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="auto-style11"><strong>صافي القسط :</strong></td>
                                        <td>
                                            <asp:SqlDataSource ID="EConds" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT CondNo, Condition FROM CondFile WHERE (SubSys = @Sys)">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter DefaultValue="04" Name="Sys" QueryStringField="Sys" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                            <dx:ASPxTextBox ID="Premium" runat="server" ClientInstanceName="Premium" CssClass="2" DisplayFormatString="n3"  Text="0" Width="109px">
                                                <ClientSideEvents LostFocus="function(s, e) {OnChange;
                                                                cbp.PerformCallback('Calc'); }"
                                                    TextChanged="Validate" />
                                                <ValidationSettings SetFocusOnError="True">
                                                    <RegularExpression ErrorText="أرقام فقط" />
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="auto-style11">
                                            &nbsp;</td>
                                      
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