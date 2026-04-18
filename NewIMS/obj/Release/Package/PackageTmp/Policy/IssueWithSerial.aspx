<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IssueWithSerial.aspx.vb" Inherits="IssueWithSerial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
           function ValidateS(s, e) {
               //if (ASPxClientEdit.ValidateGroup('Data'))
               s.SetText('يرجى الانتظار حتى تظهر شاشة الطباعة');
               s.SetEnabled(false);
                   cbp.PerformCallback('Issue');
           }
    </script>
    <style type="text/css">
        .auto-style1 {
            color: #FF3300;
            text-align: center;
        }

        .auto-style2 {
            font-size: medium;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" />
                <SettingsLoadingPanel Text="   تم الاصدار &amp;hellip; " Delay="0" Enabled="false" ShowImage="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table dir="rtl" style="width: 50%;">
                            <tr>
                                <td>
                                    <dx:ASPxComboBox ID="AvailableSerials" runat="server" DataSourceID="SerialNoSource" TextField="SerialF" ValueField="SerialF"
                                        DropDownStyle="DropDownList" SelectedIndex="0" NullText="0" Caption="الرقم التسلسلي للوثيقة">
                                    </dx:ASPxComboBox>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="Issue" runat="server" AutoPostBack="False" ClientInstanceName="Issue" Text="إصدار">
                                        <ClientSideEvents Click="ValidateS" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <%-- <asp:SqlDataSource ID="SerialNoSource" runat="server" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>'
                                    SelectCommand="with cte as (select SerialF ,SerialT from SUBSYSTEMS where Branch=@Br And SUBSYSNO=@Sys union all select SerialF+1 ,SerialT from cte where SerialF<SerialT) select SerialF from cte where SerialF not in (Select SerialNo From PolicyFile where Branch=@Br AND SubIns=@Sys) order by SerialF option (maxrecursion 0)">
                                    <SelectParameters>
                                        <asp:QueryStringParameter QueryStringField="Br" Name="Br" DbType="String"></asp:QueryStringParameter>
                                        <asp:QueryStringParameter QueryStringField="Sys" Name="Sys" DbType="String"></asp:QueryStringParameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>--%>
                                    <asp:SqlDataSource ID="SerialNoSource" runat="server" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>'
                                        SelectCommand="SerialsDocs" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:QueryStringParameter QueryStringField="Br" Name="Br" DbType="String"></asp:QueryStringParameter>
                                            <asp:QueryStringParameter QueryStringField="Sys" Name="Sys" DbType="String"></asp:QueryStringParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td><strong><span class="auto-style2">يرجى التأكد من أن رقم النموذج في الطابعة مطابق للرقم أعلاه</span></strong></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>
                                        <dx:ASPxLabel ID="AlertSerial" runat="server" CssClass="auto-style2" Text="ASPxLabel" Visible="False">
                                        </dx:ASPxLabel>
                                    </strong>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td><strong>
                                    <dx:ASPxLabel ID="html" runat="server" Text="" Visible="true">
                                    </dx:ASPxLabel>
                                </strong></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>