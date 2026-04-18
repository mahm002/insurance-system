<%@ Page Language="VB" AutoEventWireup="false" Inherits="Reinsurance_Treaty" Codebehind="Treaty.aspx.vb" %>

<%@ Register assembly="eba.Web" namespace="eba.Web" tagprefix="eba" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<link href="../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
    <title>Untitled Page</title>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
<table id="TABLE1" runat="server" visible="true" style="width:100%;">
                <tbody>
                    <tr>
                        <td colspan="2" style="height: 24px">
                            <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/images/txt1.png"  ToolTip="ÑÌæÚ">HyperLink</asp:HyperLink>
                            <asp:HyperLink ID="HyperLink2" runat="server" ImageUrl="~/images/addnew.gif" NavigateUrl="~/SystemManag/Flags/Flag.aspx"
                                Target="_new" ToolTip="ÇáÃÏáÉ">HyperLink</asp:HyperLink></td>
                    </tr>
                </tbody>
            </table>        <table dir="auto">
            <tr>
                <td colspan="8" style="font-weight: bolder; font-size: 30px; text-transform: capitalize;
                    background-color: #cccccc" class="auto-style6">
                    &nbsp; &nbsp; &nbsp;Treaty Form</td>
            </tr>
            <tr>
                <td  class="auto-style3">
                    &nbsp;</td>
                <td  style="background-color: #6699cc" 
                    class="auto-style23">
                    &nbsp;</td>
                <td class="auto-style5">
                    <dx:ASPxComboBox ID="Covers" ClientInstanceName="Covers" runat="server" DataSourceID="CoverTypes">
                    </dx:ASPxComboBox>
                </td>
                <td  style="background-color: #6699cc" class="auto-style22" colspan="2">
                    &nbsp;</td>
                <td class="auto-style7">
                    &nbsp;</td>
                <td class="auto-style8">
                    &nbsp;</td>
                <td class="auto-style8" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  class="auto-style3">
                </td>
                <td  style="background-color: #6699cc" 
                    class="auto-style23">
                    Ins. Dept</td>
                <td class="auto-style5">
                    <asp:DropDownList ID="TreatyType" runat="server" Width="175px" 
                        AutoPostBack="True" Height="19px" CssClass="mGrid">
                        <asp:ListItem Value="0">Select lnsurance Type</asp:ListItem>
                        <asp:ListItem Value="04">MC/MOC</asp:ListItem>
                        <asp:ListItem Value="0402">MC/MOC (live Stock)</asp:ListItem>
                        <asp:ListItem Value="22">Fire</asp:ListItem>
                        <asp:ListItem Value="14">EAR/CAR</asp:ListItem>
                        <asp:ListItem Value="10">CIS</asp:ListItem>
                        <asp:ListItem Value="11">TPL</asp:ListItem>
                        <asp:ListItem Value="13">CIT</asp:ListItem>
                        <asp:ListItem Value="16">CPM</asp:ListItem>
                        <asp:ListItem Value="12">BBB</asp:ListItem>
                        <asp:ListItem Value="18">Public Liability - WC - EL</asp:ListItem>
                        <asp:ListItem Value="08">FG</asp:ListItem>
                        <asp:ListItem Value="06">HULL</asp:ListItem>
                        <asp:ListItem Value="GA">General Accident (Used 2015 and Before treaties)</asp:ListItem>
                        <asp:ListItem Value="Bu">Burglary (Used 2012 and Before treaties)</asp:ListItem>
                        <asp:ListItem Value="26">Aviation</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="background-color: #6699cc" class="auto-style22" colspan="2">
                    Date</td>
                <td  class="auto-style7">
                    <asp:TextBox ID="TreatyDate" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px"  Width="120px"></asp:TextBox></td>
                <td class="auto-style8">
                </td>
                <td class="auto-style8" dir="rtl">
                </td>
            </tr>
            <tr>
                <td  style="width: 43px">
                </td>
                <td  style="background-color: #6699cc" 
                    class="auto-style24">
                    Treaty No</td>
                <td style="width: 131px">
                    <asp:TextBox ID="TreatyNo" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" Enabled="False"></asp:TextBox></td>
                <td  style="background-color: #6699cc" class="auto-style25" colspan="2">
                    Description</td>
                <td  colspan="2">
                    <asp:TextBox ID="Descrip" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="338px" TabIndex="1"></asp:TextBox></td>
                <td class="auto-style1" dir="rtl">
                </td>
            </tr>
            <tr>
                <td  style="width: 43px">
                </td>
                <td  style="background-color: #6699cc" 
                    class="auto-style24">
                    Inception Date</td>
                <td style="width: 131px">
                    <asp:TextBox ID="TReatyFrom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="2"></asp:TextBox></td>
                <td  style="background-color: #6699cc" class="auto-style25" colspan="2">
                    Expiry Date</td>
                <td  class="auto-style10">
                    <asp:TextBox ID="TreatyTo" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="3"></asp:TextBox></td>
                <td class="auto-style1">
                </td>
                <td class="auto-style1" dir="rtl">
                </td>
            </tr>
            <tr>
                <td  style="width: 43px">
                </td>
                <td  style="background-color: #6699cc" 
                    class="auto-style24">
                    PortFolio Type</td>
                <td style="width: 131px">
                    <asp:DropDownList ID="PortFolio" runat="server" Width="123px" TabIndex="4">
                        <asp:ListItem Value="1">Clean Cut</asp:ListItem>
                        <asp:ListItem Value="2">U/W</asp:ListItem>
                    </asp:DropDownList></td>
                <td  style="background-color: #6699cc" class="auto-style25" colspan="2">
                    Accounts</td>
                <td  class="auto-style10">
                    <asp:DropDownList ID="AccType" runat="server" Width="123px" TabIndex="5">
                        <asp:ListItem Value="1">Monthly</asp:ListItem>
                        <asp:ListItem Value="2">Quartirly</asp:ListItem>
                        <asp:ListItem Value="3">Annual</asp:ListItem>
                    </asp:DropDownList></td>
                <td class="auto-style1">
                </td>
                <td class="auto-style1" dir="rtl">
                </td>
            </tr>
            <tr>
                <td style="width: 43px">
                </td>
                <td  style="background-color: #6699cc" 
                    class="auto-style24">
                    Capacity</td>
                <td style="width: 131px">
                    <asp:TextBox ID="Capacity" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px"  Width="120px" 
                        TabIndex="6"></asp:TextBox></td>
                <td  style="background-color: #6699cc" class="auto-style25" colspan="2" dir="ltr">
                    Interest On Reserve</td>
                <td  class="auto-style10">
                    <asp:TextBox ID="InterestRRes" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="9"></asp:TextBox>
                </td>
                <td class="auto-style1">
                </td>
                <td class="auto-style1" dir="rtl">
                </td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td  style="background-color: #6699cc" 
                    class="auto-style24">
                    Reserve Ratio</td>
                <td style="width: 131px">
                    <asp:TextBox ID="ReserveR" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="9"></asp:TextBox>
                </td>
                <td style="background-color: #6699cc" class="captionaja" colspan="2" dir="ltr">
                    &nbsp;</td>
                <td  class="auto-style10">
                    &nbsp;</td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  class="style1">
                </td>
                <td  style="background-color: #cccccc;" colspan="4" 
                    class="captionaja">
                    Treaty Limits</td>
                <td  style="background-color: #FFFFFF" class="alt">
                    &nbsp;</td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style1" dir="rtl">
                </td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td  class="auto-style16">
                    &nbsp;</td>
                <td class="captionaja" >
                    Amount</td>
                <td  class="captionaja" colspan="2">
                    Comission</td>
                <td  class="auto-style11">
                    Leader
                    Comission</td>
                <td class="captionaja">
                    War Commision</td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td class="auto-style17" 
                    style="background-color: #6699cc">
                    Retintion</td>
                <td>
                    <asp:TextBox ID="Ret" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="7"></asp:TextBox></td>
                <td  class="style6" colspan="2">
                    &nbsp;</td>
                <td  class="auto-style10">
                    &nbsp;</td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td  class="auto-style26" 
                    style="background-color: #6699cc">
                    Q.S</td>
                <td>
                    <asp:TextBox ID="QS" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="8"></asp:TextBox></td>
                <td  class="style6" colspan="2">
                    <asp:TextBox ID="QSCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="9"></asp:TextBox>
                </td>
                <td  class="auto-style10">
                    <asp:TextBox ID="LQSCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="9"></asp:TextBox>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="WQSCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="9"></asp:TextBox>
                </td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td  class="auto-style26" 
                    style="background-color: #6699cc">
                    1ST Surp.</td>
                <td>
                    <asp:TextBox ID="FSup" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px"
                        Width="120px" TabIndex="10"></asp:TextBox></td>
                <td  class="style6" colspan="2">
                    <asp:TextBox ID="FSupCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="11"></asp:TextBox>
                </td>
                <td  class="auto-style10">
                    <asp:TextBox ID="LFSupCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="11"></asp:TextBox>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="WFSupCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="11"></asp:TextBox>
                </td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td  class="auto-style26" 
                    style="background-color: #6699cc">
                    2ND Surp.</td>
                <td>
                    <asp:TextBox ID="SSup" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="12"></asp:TextBox></td>
                <td  class="style6" colspan="2">
                    <asp:TextBox ID="SSupCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="13" AutoPostBack="True"></asp:TextBox>
                </td>
                <td  class="auto-style10">
                    <asp:TextBox ID="LSSupCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="13" AutoPostBack="false"></asp:TextBox>
                </td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td  class="auto-style26" 
                    style="background-color: #6699cc">
                    LineSlip</td>
                <td class="auto-style14">
                    <asp:TextBox ID="LS" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="12"></asp:TextBox></td>
                <td class="auto-style14" colspan="2">
                    <asp:TextBox ID="LsCom" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="13" AutoPostBack="True">0</asp:TextBox>
                </td>
                <td  class="auto-style13">
                    <address>
&nbsp;<strong>Only Marine</strong></address>
                </td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td style="background-color: #cccccc;" colspan="4" 
                    class="captionaja">
                    Treaty Participants</td>
                <td  class="auto-style10">
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                            ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"
                            SelectCommand="SELECT DISTINCT 
                         TRSECFLE.TRREINSCO, TRSECFLE.TRSHACOM, TRSECFLE.TRCOMM, TRSECFLE.IsLeader, EXTRAINFO.TP, EXTRAINFO.TPNo, EXTRAINFO.TPName, EXTRAINFO.value, 
                            EXTRAINFO.Accessor, EXTRAINFO.type, EXTRAINFO.part, 
                         EXTRAINFO.TPName AS ReCom
                        FROM  TRSECFLE INNER JOIN
                         EXTRAINFO ON TRSECFLE.TRREINSCO = EXTRAINFO.TPNo AND EXTRAINFO.TP = 'ReCom'
                        WHERE (RIGHT(TRSECFLE.TREATYNO, 4) = RIGHT(@Treaty, 4))" CancelSelectOnNullParameter="False">
                            <SelectParameters>
                                <asp:Parameter Name="Treaty" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                </td>
                <td class="auto-style1">
                    <asp:SqlDataSource ID="CoverTypes" runat="server" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>' 
                        SelectCommand="SELECT CoverTP, CoverName, SubSystem, CoverNo FROM Covers WHERE (CoverTP IS NOT NULL)">
                    </asp:SqlDataSource>
                </td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td  class="captionaja" style="background-color: #6699CC" colspan="2">
                    Reinsurance Participaints</td>
                <td  style="background-color: #6699cc" class="captionaja">
                    Share %</td>
                <td  style="background-color: #6699cc" class="auto-style20">
                    Is leader</td>
                <td  class="auto-style10">
                    &nbsp;</td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 43px">
                    &nbsp;</td>
                <td  colspan="2">
                    <eba:Combo ID="ReCom" 
                        runat="server" postbackonselectevent="false" preconfiguredstylesheet="RecordSearch"
                            Style="padding-bottom: 1px; padding-top: 1px" Width="100%" 
                        DataTextField="TPNAME" DataValueField="TPNo" CSSClassName="ReCom" 
                        TabIndex="15">
                        <List AllowPaging="True" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;" EnableDatabaseSearch="True" Height="120px" PageSize="7"
                                Width="360px" />
                    </eba:Combo>
                        </td>
                <td  class="style6">
                    <asp:TextBox ID="Share" runat="server" BorderColor="Gray" BorderStyle="Solid"
                        BorderWidth="1px" 
                        Width="120px" TabIndex="16" MaxLength="4"></asp:TextBox>
                </td>
                <td  class="auto-style21">
                    <asp:CheckBox ID="Leader" runat="server" />
                </td>
                <td  class="auto-style10">
                    <asp:Button ID="Button2" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="False"
                        Font-Names="Tahoma" Font-Size="8pt" 
                        TabIndex="17" Text="Add" Width="70px" Height="27px" CssClass="CaptionM" /></td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 43px">
                    &nbsp;</td>
                <td  class="EngTD" colspan="4">
                        <asp:GridView ID="GridView1"  runat="server" AutoGenerateColumns="False" CellPadding="2"
                            ForeColor="#333333" GridLines="None" Width="493px" 
                        DataSourceID="SqlDataSource1" ShowHeader="False" BorderStyle="Solid" 
                        BorderWidth="1px">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:CommandField ButtonType="Image"
                                    SelectImageUrl="~/images/more.gif" ShowSelectButton="True">
                                    <ItemStyle Width="43px" />
                                </asp:CommandField>
                                <asp:BoundField DataField="TRREINSCO">
                                <ItemStyle Font-Names="Tahoma" Font-Size="9pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ReCom" HtmlEncode="False">
                                    <ItemStyle HorizontalAlign="Right" Font-Names="Tahoma" Font-Size="9pt" 
                                    Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TRSHACOM" DataFormatString="{0:0.00}%">
                                    <ItemStyle Font-Names="Tahoma" Font-Size="9pt" Width="80px" 
                                    HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="IsLeader" />
                                <asp:ButtonField ButtonType="Image" CommandName="DelLine" ImageUrl="~/images/delete.gif"
                                    Text="Button" CausesValidation="True" />
                            </Columns>
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <EditRowStyle BackColor="#999999" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="White" Font-Bold="True" ForeColor="White" Height="0px" Width="0px" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <PagerTemplate>
                                &nbsp;
                            </PagerTemplate>
                        </asp:GridView>
                    </td>
                <td  class="auto-style10">
                    &nbsp;</td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style1" dir="rtl">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  class="EngTD" style="width: 43px; height: 47px">
                </td>
                <td  class="auto-style19">
                    <asp:Button ID="Button1" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="False"
                        Font-Names="Tahoma" Font-Size="8pt"
                        TabIndex="14" Text="SAVE" Width="131px" Height="27px" /></td>
                <td  colspan="3" style="vertical-align: top; height: 47px">
                </td>
                <td  style="vertical-align: top; " class="auto-style12">
                </td>
                <td  style="vertical-align: top; " class="auto-style2">
                </td>
                <td  style="vertical-align: top; " class="auto-style2" dir="rtl">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
