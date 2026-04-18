<%@ Page Language="VB" AutoEventWireup="false" Inherits="Estimate" Codebehind="Estimate.aspx.vb" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head id="Head1" runat="server">
		<title> التقدير الاحتياطي لحادث </title>
		<link rel="stylesheet" type="text/css" href="../../Styles/MainSiteStyle.css" />
        <link href="../../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
           <script lang="javascript" type="text/javascript">
               //function SetEvent(item, IDName) {
               //    if (document.all) {
               //        if (event.keyCode == 13) {
               //            event.returnValue = false;
               //            event.cancel = true;
               //            if (IDName == '')
               //                document.getElementById(item).focus()
               //            else
               //                document.getElementById(IDName).object.SetFocus()
               //        }
               //    }
               //}
               //function SetOnSelectEvent(Elm, IDName) {
               //    if (IDName == '')
               //        document.getElementById(Elm).focus()
               //    else
               //        document.getElementById(IDName).object.SetFocus()
               //}
               //function openWindow() {
               //    alert(document.URL);
               //}
           </script>

	    <style type="text/css">
	        .style1 {
	        }

	        .style3 {
	            width: 478px;
	        }
	        .auto-style1 {
                text-align: center;
                font-size: 14px;
                font-family: arial;
                font-weight: bold;
                color: #ffffff;
                height: 30px;
                border-left: 1px solid #000000;
                border-right: 0px solid #000000;
                border-top: 0px solid #000000;
                border-bottom: 1px solid #000000;
                background: url('../../Styles/images/tilebg_tablecaption1.gif') repeat-x 0px 0px;
            }
            .auto-style2 {
                width: 478px;
                height: 30px;
            }
            .auto-style3 {
                height: 30px;
            }
	    </style>
	</head>
	<body onload="SetEvent('pageFooter_CustName','');">
		<form id="Form1" method="post" runat="server">
		<div id="pagetop" class="ArabicForm" style="Z-INDEX: 101" nowrap="nowrap">
            <TABLE id="TABLE1" width="100%" runat="server"
                    visible="false">
               <TBODY>
                <TR>
                 <TD colspan="2" style="height: 24px">
                     &nbsp;</TD>
                </TR>
               </TBODY>
             </TABLE>
        </div>
		<div align="center" class="ArabicForm">
                       <asp:TextBox ID="count" runat="server" Width="71px" Visible="False"></asp:TextBox>
            <asp:Button ID="Button4" runat="server" Text="Button" Visible="False" />
            &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:TextBox ID="loadno"
                runat="server" Visible="False"></asp:TextBox>
                       &nbsp;<table align="center" id="TABLE2"  border="0" cellpadding="0" cellspacing="0" dir="rtl" width="100%">
               <tr>
                   <td align="left" class="Caption" style="text-align: right">
                       رقم الحادث&nbsp;
                   </td>
                   <td align="right" class="style3">
                       &nbsp;
                       <asp:TextBox ID="clmno" runat="server" Width="128px"></asp:TextBox>&nbsp;&nbsp;&nbsp;</td>
                   <td align="right" class="Caption">
                       رقم الوثيقة </td>
                   <td align="right">
                       <asp:TextBox ID="polno" runat="server" Width="121px"
                           style="margin-right: 0px"></asp:TextBox></td>
               </tr>
               <tr>
                    <td align="left" class="auto-style1" style="text-align: right">
                        التقدير الاحتياطي&nbsp;
                    </td>
                   <td align="right" class="auto-style2" dir="ltr">
                       &nbsp;
                       <asp:TextBox ID="value" runat="server" CssClass="PremText1" onkeypress="return SetEvent('button3','')"
                           TabIndex="7" Width="128px"></asp:TextBox>&nbsp;&nbsp;</td>
                    <td align="left" class="auto-style1">
                        التــــــاريخ&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="right" class="auto-style3">
                        <%--<radcln:raddatepicker id="date1" runat="server" selecteddate="1980-01-01"
                            width="144px" Culture="Arabic (Libya)">
<Calendar FocusedDate="2007-05-08"></Calendar>

<DateInput Title="" PromptChar=" " TitleIconImageUrl="" DisplayPromptChar="_" CatalogIconImageUrl="" TitleUrl="" Description="" DateFormat="dd/MM/yyyy"></DateInput>
</radcln:raddatepicker>--%>
                        <br />
                        <dx:ASPxDateEdit ID="date1" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
               </tr>
               <tr>
                    <td align="left" style="text-align: right" colspan="4">
                       
                            <asp:Button ID="Button5" runat="server" Text="Button"
                            Visible="False" Width="99px" />
                    </td>
               </tr>
               <tr>
                    <td align="left" class="style1" style="text-align: right" colspan="4">
                           <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1"
                            Width="44%" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                            GridLines="None" CssClass="mGrid">
                               <Columns>
                                   <asp:CommandField ShowSelectButton="True" ButtonType="Image"
                                       SelectImageUrl="~/images/txt.png" />
                                   <asp:BoundField DataField="Sn" HeaderText="رقم التقدير" SortExpression="clmno">
                                       <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="value" HeaderText="القيمة">
                                       <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                   </asp:BoundField>
                                   <asp:BoundField DataField="date" HeaderText="التاريخ" DataFormatString="{0:dd/MM/yyyy}">
                                       <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                   </asp:BoundField>
                                   <asp:ButtonField ButtonType="Image" CommandName="LvlPrn"
                                       ImageUrl="~/images/prn1.png" />
                               </Columns>
                               <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                               <RowStyle BackColor="#EFF3FB" />
                               <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                               <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                               <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                               <EditRowStyle BackColor="#2461BF" />
                               <AlternatingRowStyle BackColor="White" />
                           </asp:GridView>
                    </td>
               </tr>
               <tr>
                   <td colspan="4" style="height: 17px">
                       <div style="width: 100%; height: 1px" class="Line">
                           &nbsp;<asp:SqlDataSource ID="SqlDataSource1" runat="server"
                               ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                               SelectCommand="SELECT [value], [date], [Sn] FROM [Estimation]"></asp:SqlDataSource>
                       </div>
                        <asp:Button ID="Button1" runat="server" Text="تسجيل" BorderColor="LightSlateGray" BorderStyle="Solid" BorderWidth="2px" Width="153px" PostBackUrl="~/PolicyManagement/CARGO/CARGO.aspx?Submit=1" Visible="False" />&nbsp;
                       <asp:HyperLink ID="HyperLink2" runat="server" Target="_self"
                           NavigateUrl="~/ClaimsManage/Default.aspx?Request(&quot;sys&quot;)"
                           Visible="False">رجوع</asp:HyperLink>
                        <asp:Button ID="Button2" runat="server" Text="اصــدار" Width="151px" Enabled="False" Visible="False" />
                       &nbsp;
                       <asp:Button ID="Button3" runat="server" Text="موافق" Width="159px" /></td>
               </tr>
            </table>
            </div>
    </form>
</body>
</html>