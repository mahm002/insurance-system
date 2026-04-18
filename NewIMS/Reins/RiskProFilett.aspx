<%@ Page Language="VB" AutoEventWireup="false" Inherits="Reinsurance_RiskProFilett" CodeBehind="RiskProFilett.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 107px;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <link href="../Scripts/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/bootstrap.min.js"></script>
    <link href="../Scripts/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=TreatyType]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="TABLE1" runat="server" visible="true" width="100%">
                <tbody>
                    <tr>
                        <td colspan="2" style="height: 24px">
                            <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/images/undo.gif" NavigateUrl="~/ReIns/default.aspx"
                                ToolTip="رجوع">HyperLink</asp:HyperLink>
                            <asp:HyperLink ID="HyperLink2" runat="server" ImageUrl="~/images/addnew.gif" NavigateUrl="~/SystemManag/Flags/Flag.aspx"
                                Target="_new" ToolTip="رجوع">HyperLink</asp:HyperLink></td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tr>
                    <td colspan="7" style="font-weight: bolder; font-size: 30px; text-transform: capitalize; height: 79px; background-color: #cccccc">&nbsp; &nbsp; &nbsp;Risk Profile</td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px"></td>
                    <td align="left" style="width: 133px; background-color: #6699cc">Main Ins.Dept</td>
                    <td style="width: 131px">
                        <asp:DropDownList ID="Sys" runat="server" Width="123px" AutoPostBack="True" DataSourceID="MainSys" DataTextField="SYSNAMEL" DataValueField="SYSNO">
                        </asp:DropDownList>
                    </td>
                    <td align="left" class="style6">&nbsp;</td>
                    <td align="left" class="auto-style1">&nbsp;</td>
                    <td style="width: 112px">
                        <asp:DropDownList ID="TreatyType0" runat="server" Width="123px"
                            AutoPostBack="True" Visible="False">
                            <asp:ListItem Value="1">LYD</asp:ListItem>
                            <asp:ListItem Value="2">USD</asp:ListItem>
                            <asp:ListItem Value="11">Euro</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 104px"></td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">&nbsp;</td>
                    <td align="left" style="width: 133px; background-color: #6699cc">SubIns. Dept</td>
                    <td style="width: 131px">

                        <asp:ListBox ID="TreatyType" runat="server" SelectionMode="Multiple" DataSourceID="SubSystems" DataTextField="SUBSYSNAME" DataValueField="SUBSYSNO">
                            <%--<asp:ListItem Value="04" Text="Marine Cargo" />
                        <asp:ListItem Value="06" Text="hull" />
                        <asp:ListItem Value="08" Text="FG" />
                        <asp:ListItem Value="10" Text="CIS" />
                        <asp:ListItem Value="11" text="TPL" />
                        <asp:ListItem Value="12" text="BBB" />
                        <asp:ListItem Value="13" Text="CIT" />
                        <asp:ListItem Value="18" Text="EL" />
                        <asp:ListItem Value="14" Text="CAR" />
                        <asp:ListItem Value="15" Text="EAR" />
                        <asp:ListItem Value="22" Text="Fire" />
                        <asp:ListItem Value="21" Text="Burglary" />--%>
                            <%--                        <asp:ListItem Text="Mango" Value="1" />
                        <asp:ListItem Text="Apple" Value="2" />
                        <asp:ListItem Text="Banana" Value="3" />
                <asp:ListItem Text="Guava" Value="4" />
                <asp:ListItem Text="Orange" Value="5" />--%>
                        </asp:ListBox>
                    </td>
                    <td align="left" class="style6">&nbsp;</td>
                    <td align="left" class="auto-style1">
                        <asp:Button Text="Submit" runat="server" OnClick="Submit" Visible="False" />
                    </td>
                    <td style="width: 112px">&nbsp;</td>
                    <td style="width: 104px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left" class="style1"></td>
                    <td align="left" style="background-color: #cccccc;" colspan="3">
                        <br />
                        Sum Insured</td>
                    <td align="left" class="auto-style1">&nbsp;</td>
                    <td class="style4">&nbsp;</td>
                    <td class="style5">

                        <asp:SqlDataSource ID="MainSys" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                            SelectCommand="SELECT [SYSNO], [SYSNAMEL] FROM [SYSTEMS] where Sysno in (1,3,6,9)"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">&nbsp;</td>
                    <td align="left" class="EngTD" style="width: 133px; height: 23px; background-color: #6699cc">From</td>
                    <td>
                        <asp:TextBox ID="SumInsF" runat="server" BorderColor="Gray" BorderStyle="Solid"
                            BorderWidth="1px"
                            Width="120px" TabIndex="7"></asp:TextBox></td>
                    <td align="left" class="style6">&nbsp;</td>
                    <td align="left" class="auto-style1">&nbsp;</td>
                    <td style="width: 112px">&nbsp;</td>
                    <td style="width: 104px">

                        <asp:SqlDataSource ID="SubSystems" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                            SelectCommand="SELECT [SUBSYSNO], [MAINSYS], [SUBSYSNAME], [SUBSYSNAMEL] FROM [SUBSYSTEMS] WHERE ([MAINSYS] = @MAINSYS) and branch=[dbo].[MainCenter]()">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="Sys" PropertyName="SelectedValue" DefaultValue="1" Name="MAINSYS" Type="Int16"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">&nbsp;</td>
                    <td align="left" class="EngTD" style="width: 133px; height: 23px; background-color: #6699cc">To</td>
                    <td>
                        <asp:TextBox ID="SumInsT" runat="server" BorderColor="Gray" BorderStyle="Solid"
                            BorderWidth="1px"
                            Width="120px" TabIndex="8"></asp:TextBox></td>
                    <td align="left" class="style6">&nbsp;</td>
                    <td align="left" class="auto-style1">&nbsp;</td>
                    <td style="width: 112px">&nbsp;</td>
                    <td style="width: 104px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">&nbsp;</td>
                    <td align="left" class="EngTD" style="width: 133px; height: 23px; background-color: #6699cc">Increase By</td>
                    <td>
                        <asp:TextBox ID="SumInsBy" runat="server" BorderColor="Gray" BorderStyle="Solid"
                            BorderWidth="1px"
                            Width="120px" TabIndex="9"></asp:TextBox></td>
                    <td align="left" class="style6">&nbsp;</td>
                    <td align="left" class="auto-style1">&nbsp;</td>
                    <td style="width: 112px">&nbsp;</td>
                    <td style="width: 104px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">&nbsp;</td>
                    <td align="left" style="background-color: #cccccc;" colspan="3">
                        <br />
                        Issued Interval</td>
                    <td align="left" class="auto-style1">&nbsp;</td>
                    <td style="width: 112px">&nbsp;</td>
                    <td style="width: 104px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">&nbsp;</td>
                    <td align="left" class="EngTD" style="width: 133px; height: 23px; background-color: #6699cc">From</td>
                    <td>
                        <asp:TextBox ID="DFrom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                            BorderWidth="1px"
                            Width="120px" TabIndex="10"></asp:TextBox></td>
                    <td align="left" class="style6">&nbsp;</td>
                    <td align="left" style="width: 107px">&nbsp;</td>
                    <td style="width: 112px">&nbsp;</td>
                    <td style="width: 104px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">&nbsp;</td>
                    <td align="left" class="EngTD" style="width: 133px; height: 23px; background-color: #6699cc">To</td>
                    <td>
                        <asp:TextBox ID="DTo" runat="server" BorderColor="Gray" BorderStyle="Solid"
                            BorderWidth="1px"
                            Width="120px" TabIndex="11"></asp:TextBox></td>
                    <td align="left" class="style6">&nbsp;</td>
                    <td align="left" style="width: 107px">&nbsp;</td>
                    <td style="width: 112px">&nbsp;</td>
                    <td style="width: 104px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">&nbsp;</td>
                    <td align="left" class="EngTD" colspan="3">
                        <asp:Button ID="Button1" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="False"
                            Font-Names="Tahoma" Font-Size="8pt"
                            TabIndex="14" Text="Calculate" Width="131px" Height="27px" />
                    </td>
                    <td align="left" style="width: 107px">&nbsp;</td>
                    <td style="width: 112px">&nbsp;</td>
                    <td style="width: 104px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" class="EngTD" style="width: 43px; height: 47px"></td>
                    <td align="center" class="EngTD" style="width: 133px; height: 47px">&nbsp;</td>
                    <td align="center" colspan="2" style="vertical-align: top; height: 47px"></td>
                    <td align="center" style="vertical-align: top; width: 107px; height: 47px"></td>
                    <td align="center" style="vertical-align: top; width: 112px; height: 47px"></td>
                    <td align="center" style="vertical-align: top; height: 47px"></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>