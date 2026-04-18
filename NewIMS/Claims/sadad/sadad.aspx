
<%@ Page Language="VB" AutoEventWireup="false" Inherits="Sadad"  Codebehind="sadad.aspx.vb" %>
<%@ Register TagPrefix="eba" Namespace="eba.Web" Assembly="eba.Web" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register TagPrefix="demos" TagName="Footer1" Src="~/Claims/WebUserControl.ascx"%>

<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server">
		<title> سداد التعويض </title>
		
        <link href="../../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
           <script lang="javascript" type="text/javascript">
            function SetEvent(item,IDName)
            {
                if (document.all){
                 if (event.keyCode == 13)
                  { 
                    event.returnValue=false;
                    event.cancel = true;
                    if (IDName=='') 
                     document.getElementById(item).focus()
                    else
                     document.getElementById(item).object.SetFocus()
                  }
                 }   
            }
             function SetOnSelectEvent(Elm,IDName)
             {
                    if (IDName=='') 
                     Elm.focus();
                    else
                    {
                     document.getElementById(IDName).object.SetFocus()
                     }
             }
      function openWindow()
      {
        alert(document.URL);
      }
function Select1_onclick() {

}

function TABLE2_onclick() {

}

           </script>



	    <style type="text/css">
            .style48
            {
                text-align: center;
                width: 166px;
            }
            .style38
            {
                text-align: center;
            }
            .style49
            {
                text-align: center;
                width: 97px;
            }
            .style50
            {
                border-left: 1px solid #000000;
                border-right: 0px solid #000000;
                border-top: 0px solid #000000;
                border-bottom: 1px solid #000000;
                background: url('../../Styles/images/dolphin_bg-ONR.gif') repeat-x 0px 0px;
                text-align: center;
                font-size: 14px;
                font-family: arial;
                font-weight: bold;
                color: #ffffff;
                height: 26px;
                position: static;
                width: 97px;
            }
            .style51
            {
                border-left: 1px solid #000000;
                border-right: 0px solid #000000;
                border-top: 0px solid #000000;
                border-bottom: 1px solid #000000;
                background: url('../../Styles/images/dolphin_bg-ONR.gif') repeat-x 0px 0px;
                text-align: center;
                font-size: 14px;
                font-family: arial;
                font-weight: bold;
                color: #ffffff;
                height: 26px;
                position: static;
                width: 84px;
            }
            .style52
            {
                width: 95px;
            }
            .style53
            {
                border-left: 1px solid #000000;
                border-right: 0px solid #000000;
                border-top: 0px solid #000000;
                border-bottom: 1px solid #000000;
                background: url('../../Styles/images/dolphin_bg-ONR.gif') repeat-x 0px 0px;
                text-align: center;
                font-size: 14px;
                font-family: arial;
                font-weight: bold;
                color: #ffffff;
                height: 26px;
                position: static;
                width: 108px;
            }
            .style54
            {
                width: 108px;
            }
        </style>



	</head>
	<body onload="SetEvent('pageFooter_CustName','');" >
		<form id="Form1" method="post" runat="server" class="ArabicForm">
		<div id="pagetop" class="ArabicForm" style="Z-INDEX: 101" nowrap="nowrap">
            <TABLE id="TABLE1" width="100%" runat="server" 
                    visible="false">
               <TBODY>
                <TR>
                 <TD colspan="2" style="height: 24px">
                    <asp:TextBox id="MainPageErr" Width="100%" BorderColor="Black" runat="server" ForeColor="Black" BorderWidth="1px" BorderStyle="Solid" valign="bottom" Font-Names="Tahoma" Font-Bold="False" Font-Size="Smaller" BackColor="Khaki" Height="20px"></asp:TextBox>
                 </TD>
                </TR>
               </TBODY>
             </TABLE>	
        </div>	
		<div align="center" class="ArabicForm" dir="rtl" >
            &nbsp;
            <div style="width: 100%; height: 1px;" class="EngTD" align="right" dir="rtl">
                       <eba:Combo ID="GodType" OnSelectEvent="SetOnSelectEvent('FillType','FillType')" runat="server" postbackonselectevent="false" preconfiguredstylesheet="RecordSearch"
                            Style="padding-bottom: 1px; padding-top: 1px" Width="150px" DataTextField="TPNAME" DataValueField="TPNo" Visible="False">
                           <List AllowPaging="True" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;" EnableDatabaseSearch="True" Height="120px" PageSize="7"
                                Width="360px" />
                       </eba:Combo>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<eba:Combo ID="InsCond" OnSelectEvent="SetOnSelectEvent('SpcCond','SpcCond')" runat="server" postbackonselectevent="false" preconfiguredstylesheet="RecordSearch"
                            Style="padding-bottom: 1px; padding-top: 1px" Width="150px" DataTextField="TPNAME" DataValueField="TPNo" Visible="False">
                           <List AllowPaging="True" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;" EnableDatabaseSearch="True" Height="120px" PageSize="7"
                                Width="360px" />
                       </eba:Combo>
                       &nbsp;&nbsp; 
                <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp;&nbsp;
                       <asp:TextBox ID="TextBox4" runat="server" Visible="False"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" BackColor="#6699CC" CssClass="CaptionRED" Font-Bold="False"
                    Font-Names="Tahoma" Font-Size="9pt" ForeColor="White" Height="25px" Text="سداد التعويض"
                    Width="140px"></asp:Label>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            </div>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
           <table align="center"  id="TABLE2" 
                onclick="return TABLE2_onclick()" class="ArabicForm" dir="rtl" >
               <tr>
                    <td align="center" style="width: 144px; height: 26px; position: static;" 
                        class="Caption" dir="rtl">
                        رقم التسويه</td>
                   <td align="right" style="width: 170px; height: 26px; position: static;">
                       <asp:TextBox ID="tasno" runat="server" CssClass="PremText" onkeypress="return SetEvent('ContractNo','')"
                           TabIndex="7" AutoPostBack="True"></asp:TextBox></td>
                    <td align="center" style="width: 153px; height: 26px; position: static;" 
                        class="Caption" dir="rtl">
                        رقم السداد&nbsp;</td>
                    <td align="right" style="width: 133px; height: 26px; position: static;">
                        <asp:TextBox ID="eno" runat="server" CssClass="PremText" onkeypress="return SetEvent('Loads','')"
                            TabIndex="8"></asp:TextBox></td>
                   <td align="center" style="width: 152px; position: static; height: 26px" 
                        class="Caption" dir="rtl">
                       اجمالي التعويض</td>
                   <td align="right" class="style1">
                       <asp:TextBox ID="TextBox2" runat="server" CssClass="PremText"></asp:TextBox></td>
                   <td align="center" style="width: 134px; position: static; height: 26px" 
                        class="Caption" dir="rtl">
                       صافي التعويض</td>
                    <td align="right" style="width: 117px; height: 26px; position: static;">
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="PremText"></asp:TextBox></td>
               </tr>
               <tr>
                   <td align="center" class="Caption" dir="rtl">
                       اسم المستفيد</td>
                   <td align="right" class="style81">
                       <asp:TextBox ID="acname" runat="server" onkeypress="return SetEvent('cash','')" 
                           CssClass="PremText"></asp:TextBox></td>
                   <td align="center" class="Caption" dir="rtl">
                       المبلغ المسدد</td>
                   <td align="right" class="style83">
                       <asp:TextBox ID="cash" runat="server" onkeypress="return SetEvent('disno','')" 
                           CssClass="PremText"></asp:TextBox></td>
                   <td align="center" class="Caption" dir="rtl">
                       رقم ايصال السداد</td>
                   <td align="right" class="style1">
                       <asp:TextBox ID="esno" runat="server" CssClass="PremText"></asp:TextBox></td>
                   <td align="center" class="Caption" dir="rtl">
                       تاريخ السداد</td>
                   <td align="right" class="style86">
                       <radcln:raddatepicker id="RadDatePicker1" runat="server" width="120px">
<Calendar FocusedDate="2007-05-09"></Calendar>

<DateInput Title="" PromptChar=" " TitleIconImageUrl="" DisplayPromptChar="_" CatalogIconImageUrl="" TitleUrl="" Description=""></DateInput>
</radcln:raddatepicker>
                   </td>
               </tr>
               <tr>
                   <td align="center" style="width: 144px; position: static; height: 26px" 
                       class="Caption" dir="rtl">
                       رقم المخالصه</td>
                   <td align="right" style="width: 170px; position: static; height: 26px">
                       <asp:TextBox ID="disno" runat="server" Width="103px" CssClass="PremText1"></asp:TextBox></td>
                   <td align="center" style="width: 153px; position: static; height: 26px" 
                       class="Caption" dir="rtl">
                       تاريخ المخالصه</td>
                   <td align="right" style="width: 133px; position: static; height: 26px">
                       <radcln:raddatepicker id="RadDatePicker2" runat="server" width="120px">
<Calendar FocusedDate="2007-05-09"></Calendar>

<DateInput Title="" PromptChar=" " TitleIconImageUrl="" DisplayPromptChar="_" CatalogIconImageUrl="" TitleUrl="" Description=""></DateInput>
</radcln:raddatepicker>
                   </td>
                   <td align="right" 
                       style="width: 152px; position: static; height: 26px; " class="Caption" 
                       dir="rtl">
                       نوع العملة</td>
                   <td align="right" class="EngTD">
                   <eba:Combo OnSelectEvent="SetOnSelectEvent('DocVal','')" ID="Currency" 
                           runat="server" postbackonselectevent="true" preconfiguredstylesheet="RecordSearch"
                            Style="padding-bottom: 1px; padding-top: 1px" Width="150px" 
                           DataTextField="TPNAME" DataValueField="TPNo" CSSClassName="Cur" Height="">
                           <List AllowPaging="True" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;" EnableDatabaseSearch="True" Height="120px" PageSize="7"
                                Width="150px" />
                       </eba:Combo>
                   </td>
                   <td align="right" 
                       style="width: 134px; position: static; height: 26px; text-align: right;" 
                       class="Caption" dir="rtl">
                       سعر الصرف</td>
                   <td align="left" 
                       style="width: 117px; position: static; height: 26px; text-align: right;">
                       <asp:TextBox ID="Exc" runat="server" style="text-align: right" 
                           CssClass="PremText"></asp:TextBox>
                   </td>
               </tr>
               <tr>
                   <td align="center" colspan="8" style="position: static; height: 21px" dir="rtl">
                       &nbsp;<asp:GridView 
                           ID="GridView1" runat="server" DataSourceID="SqlDataSource1" Width="640px" 
                           AutoGenerateColumns="False" HorizontalAlign="Center" BorderColor="#000099" 
                           CssClass="mGrid">
                           <Columns>
                               <asp:CommandField ShowSelectButton="True" ButtonType="Image" 
                                   SelectImageUrl="~/images/txt.png" />
                               <asp:BoundField DataField="tasno" HeaderText="رقم التسوية" />
                               <asp:BoundField DataField="net" HeaderText="الصافي" />
                               <asp:BoundField DataField="total" HeaderText="الاجمالي" />
                           </Columns>
                           <HeaderStyle BackColor="#3366FF" />
                           <RowStyle HorizontalAlign="Center" />
                       </asp:GridView>
                           <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                               SelectCommand="SELECT [clmno], [tasno], [total], [net] FROM [temp]" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>">
                           </asp:SqlDataSource>
                       <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2"
                           Width="567px" CssClass="mGrid">
                           <Columns>
                               <asp:CommandField ShowSelectButton="True" ButtonType="Image" 
                                   SelectImageUrl="~/images/txt.png" />
                               <asp:BoundField DataField="tasno" HeaderText="رقم التسوية">
                                   <ItemStyle HorizontalAlign="Center" />
                                   <HeaderStyle HorizontalAlign="Center" />
                               </asp:BoundField>
                               <asp:BoundField DataField="esno" HeaderText="رقم الايصال">
                                   <ItemStyle HorizontalAlign="Center" />
                                   <HeaderStyle HorizontalAlign="Center" />
                               </asp:BoundField>
                               <asp:BoundField DataField="total" HeaderText="الاجمالي">
                                   <ItemStyle HorizontalAlign="Center" />
                                   <HeaderStyle HorizontalAlign="Center" />
                               </asp:BoundField>
                               <asp:BoundField DataField="net" HeaderText="الصافي">
                                   <ItemStyle HorizontalAlign="Center" />
                                   <HeaderStyle HorizontalAlign="Center" />
                               </asp:BoundField>
                               <asp:ButtonField ButtonType="Image" CommandName="LvlPrn" ImageUrl="~/images/printerIcon.gif"
                                   Text="Button" />
                           </Columns>
                           <HeaderStyle BackColor="#3366FF" BorderColor="#000099" />
                       </asp:GridView>
                       <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
                   </td>
               </tr>
               <tr>
                   <td colspan="8" style="height: 17px">
                       <div style="width: 100%; height: 1px" class="Line">
                           &nbsp;
                       </div>
                        &nbsp;
                       <asp:HyperLink ID="HyperLink2" runat="server" Target="_self" NavigateUrl="~/ClaimsManage/Default.aspx">رجوع</asp:HyperLink>
                       &nbsp;&nbsp;
                       <asp:Button ID="Button3" runat="server" Text="موافق" Width="159px" />
                       <eba:Combo ID="res" runat="server" postbackonselectevent="false" preconfiguredstylesheet="RecordSearch"
                            Style="padding-bottom: 1px; padding-top: 1px" Width="150px" 
                           Visible="False">
                           <List AllowPaging="True" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;" EnableDatabaseSearch="True" Height="120px" PageSize="7"
                                Width="360px" />
                           <ListColumnDefinitionItems>
                               <eba:ListColumnDefinition CSSClassName="محكمة" DataFieldIndex="0" />
                               <eba:ListColumnDefinition CSSClassName="مخلفات" DataFieldIndex="0" />
                               <eba:ListColumnDefinition CSSClassName="تسوية ودية" DataFieldIndex="0" />
                           </ListColumnDefinitionItems>
                       </eba:Combo>
                   </td>
               </tr>
            </table>
            </div>
        <p style="direction: rtl; text-align: right">
&nbsp;<table runat=server  align="center" 
                style="padding: 1px; margin: 1px; width: 892px; position: static; height: 103px;" 
                id="Table4" onclick="return TABLE2_onclick()" >
                <tr>
                    <td align="center" class="style53" dir="rtl">
                        رقم المسترد</td>
                   <td align="right" class="style52">
                       &nbsp;<asp:TextBox 
                           ID="TextBox7" runat="server" Width="81px" CssClass="PremText1"></asp:TextBox>
                       </td>
                   <td align="center" class="style51" dir="rtl">
                       قيمة المسترد</td>
                   <td align="right" class="style49">
                       &nbsp;<asp:TextBox ID="TextBox5" runat="server" 
                           Width="91px" CssClass="PremText1"></asp:TextBox>
                   </td>
                   <td align="center" class="Caption" dir="rtl" 
                        style="width: 144px; height: 26px; position: static;">
                       تاريخ المسترد</td>
                   <td align="left" class="style48">
                       <radcln:raddatepicker id="RadDatePicker3" runat="server" width="120px">
<Calendar FocusedDate="2007-05-09"></Calendar>

<DateInput Title="" PromptChar=" " TitleIconImageUrl="" DisplayPromptChar="_" CatalogIconImageUrl="" TitleUrl="" Description=""></DateInput>
</radcln:raddatepicker>
                    </td>
                   <td align="left" class="style38">
                       &nbsp;</td>
                   <td align="right" class="style69">
                   </td>
                </tr>
                <tr>
                    <td align="center" class="style53" bgcolor="White" dir="rtl">
                        نوع الاسترداد</td>
                    <td align="right" bgcolor="White" class="style61" dir="ltr" colspan="2">
                        <asp:TextBox ID="TextBox8" runat="server" CssClass="PremText1" Width="191px"></asp:TextBox>
                    </td>
                    <td align="center" class="style50" bgcolor="White" dir="rtl">
                        الجهة المستردة</td>
                    <td align="right" bgcolor="White" class="style79" dir="rtl" colspan="3">
                        <asp:TextBox ID="TextBox6" runat="server" Width="383px" CssClass="PremText1"></asp:TextBox>
                    </td>
                    <td align="center" bgcolor="White" class="style71" dir="rtl">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" class="style40" bgcolor="White" dir="rtl" 
                        bordercolor="#3333FF" colspan="8">
                       <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:MainDBConnectionString1 %>" 
                            SelectCommand="SELECT [givno], [val], [res], [gidate], [giwho] FROM [MaClmGiv]"></asp:SqlDataSource>
                       <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource3"
                           Width="595px" CssClass="mGrid">
                           <Columns>
                               <asp:CommandField ShowSelectButton="True" SelectImageUrl="~/images/txt.png" 
                                   ButtonType="Image" />
                               <asp:BoundField DataField="givno" HeaderText="رقم المسترد" 
                                   SortExpression="givno">
                                   <HeaderStyle HorizontalAlign="Center" />
                               <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               </asp:BoundField>
                               <asp:BoundField DataField="val" HeaderText="قيمة المسترد" SortExpression="val">
                                   <HeaderStyle HorizontalAlign="Center" />
                               <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               </asp:BoundField>
                               <asp:BoundField DataField="res" HeaderText="نوع الاسترداد" SortExpression="res">
                                   <HeaderStyle HorizontalAlign="Center" />
                               <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               </asp:BoundField>
                               <asp:BoundField DataField="gidate" HeaderText="تاريخ الاسترداد" 
                                   SortExpression="gidate" DataFormatString="{0:dd/MM/yyyy}">
                                   <HeaderStyle HorizontalAlign="Center" />
                               <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               </asp:BoundField>
                               <asp:BoundField DataField="giwho" HeaderText="الجهة المسترد منها" 
                                   SortExpression="giwho">
                               <HeaderStyle HorizontalAlign="Center" />
                               <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                               </asp:BoundField>
                               <asp:ButtonField ButtonType="Image" CommandName="LvlPrn2" 
                                   ImageUrl="~/images/printerIcon.gif" Text="Button" />
                           </Columns>
                           <HeaderStyle BackColor="#3366FF" />
                       </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style54" bgcolor="White" dir="rtl">
                        &nbsp;</td>
                    <td align="center" bgcolor="White" class="style52" dir="rtl">
                        <asp:Button ID="Button4" runat="server" Text="موافق" Width="105px" />
                    </td>
                </tr>
                </table>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    </form>
</body>
</html>
