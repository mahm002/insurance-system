<%@ Page Language="VB" AutoEventWireup="true" Inherits="_PolicyManagement_MOMOTOR_GroupSearch" EnableEventValidation="false" Culture="ar-LY" CodeBehind="GroupSearch.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>بحث عن الرقم المتسلسل للسيارة</title>
    <link href="../../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .waitingdiv {
            font-size: 93%;
            margin-bottom: 1em;
            margin-top: 0.2em;
            padding: 8px 12px;
            width: 2.7em;
        }

        #form1 {
            width: 100%;
        }

        .style4 {
            width: 167px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" dir="rtl" lang="ar">
        <asp:ScriptManager ID="scriptmanager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var state = document.getElementById('loadingdiv').style.display;
            if (state == 'block') {
                document.getElementById('loadingdiv').style.display = 'none';
            } else {
                document.getElementById('loadingdiv').style.display = 'block';
            }
            args.get_postBackElement().disabled = true;
        }
        </script>
        <div>

            <asp:UpdatePanel ID="PnlUsrDetails" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>أدخل رقم الهيكل:
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="BudyNo" runat="server" AutoPostBack="true"
                                    OnTextChanged="BudyNo_TextChanged" BorderStyle="Solid" BorderWidth="1px" />
                            </td>
                            <td rowspan="4">
                                <div class="waitingdiv" id="loadingdiv" style="display: none; margin-left: 5.3em">
                                    <img src="../../images/loading.gif" alt="Loading" />
                                </div>
                                <div id="checkusername" runat="server" visible="false"
                                    style="clip: rect(auto, auto, auto, auto)">
                                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333"
                                        GridLines="None" CssClass="mGrid">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                    <asp:Image ID="imgstatus" runat="server" Width="17px" />
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">أدخل رقم اللوحة:</td>
                            <td class="style4">
                                <asp:TextBox ID="TableNo" runat="server" AutoPostBack="true"
                                    OnTextChanged="TableNo_TextChanged" BorderStyle="Solid" BorderWidth="1px" />
                            </td>
                        </tr>
                        <tr>
                            <td>أدخل رقم المحرك</td>
                            <td class="style4">
                                <asp:TextBox ID="EngineNo" runat="server" AutoPostBack="true"
                                    OnTextChanged="EngineNo_TextChanged" BorderStyle="Solid" BorderWidth="1px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/images/back.png"
                                    ToolTip="رجوع">HyperLink</asp:HyperLink>
                            </td>
                            <td class="style4">
                                <asp:CheckBox ID="Check" runat="server" AutoPostBack="True"
                                    Text="بحث عن حوادث" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:Label ID="Output" runat="server" />
    </form>
</body>
</html>