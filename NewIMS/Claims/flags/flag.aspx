
<%@ Page Language="VB" AutoEventWireup="false" Inherits="CFlag" Codebehind="flag.aspx.vb" %>
<%@ Register TagPrefix="eba" Namespace="eba.Web" Assembly="eba.Web" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server">
		<title> Tareq </title>
		<link rel="stylesheet" type="text/css" href="~/OutputManagement/default.css"/>
        <link href="~/MainSiteStyle.css" rel="stylesheet" type="text/css" />
           <script language="javascript" type="text/javascript">
            function SetEvent(item,IDName)
            {
                if (document.all){
                 if (event.keyCode == 13)
                  { 
                    event.returnValue=false;
                    event.cancel = true;
                    if (IDName=='') 
                     document.getElementById(IDName).focus()
                    else
                     document.getElementById(IDName).object.SetFocus()
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

	</head>
	<body onload="SetEvent('pageFooter_CustName','');" >
		<form id="Form1" method="post" runat="server">
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
		<div align="center" class="ArabicForm" >
            &nbsp;
            <div style="width: 100%; height: 1px;" class="Line" align="right">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" BackColor="#6699CC" CssClass="Caption" Font-Bold="False"
                    Font-Names="Tahoma" Font-Size="9pt" ForeColor="White" Height="25px" Text="اضافه ادله"
                    Width="140px"></asp:Label>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            </div>
            &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
           <table align="center" style="padding-right: 1px; padding-left: 1px; padding-bottom: 1px;
                margin: 1px; padding-top: 1px; width: 660px;" id="TABLE2" onclick="return TABLE2_onclick()" border="1">
               <tr>
                   <td align="center" style="width: 11211px; position: static; height: 10px">
                        اسم الدليل</td>
                   <td align="right" style="width: 5710px; position: static; height: 10px" class="engtd">
                       <eba:Combo ID="MarBy" runat="server" postbackonselectevent="false" preconfiguredstylesheet="RecordSearch"
                            Style="padding-bottom: 1px; padding-top: 1px" Width="120px">
                        <List AllowPaging="True" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;" EnableDatabaseSearch="True" Height="120px" PageSize="7"
                                Width="360px" />
                    </eba:Combo>
                   </td>
                   <td align="center" style="width: 9630px; position: static; height: 10px">
                       البيان</td>
                   <td align="right" style="width: 4068px; position: static; height: 10px" class="engtd">
                        <asp:TextBox ID="Margin" runat="server" CssClass="PremText1" onkeypress="return SetEvent('RespVal','')"
                            TabIndex="8" Width="117px" AutoPostBack="True"></asp:TextBox>&nbsp;</td>
               </tr>
               <tr>
                   <td align="center" colspan="4" style="position: static; height: 10px">
                       <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" Width="484px">
                       </asp:GridView>
                       <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                   </td>
               </tr>
               <tr>
                   <td colspan="4" style="height: 17px">
                       <div style="width: 100%; height: 1px" class="Line">
                       </div>
                        <asp:Button ID="Button1" runat="server" Text="تسجيل" BorderColor="LightSlateGray" BorderStyle="Solid" BorderWidth="2px" Width="153px" PostBackUrl="~/PolicyManagement/CARGO/CARGO.aspx?Submit=1" Visible="False" />&nbsp;
                       <asp:HyperLink ID="HyperLink2" runat="server" Target="_self" NavigateUrl="~/PolicyManagement/Default.aspx">رجوع</asp:HyperLink>
                        <asp:Button ID="Button2" runat="server" Text="الغاء الامر" Width="151px" Enabled="False" />
                       &nbsp;
                       <asp:Button ID="Button3" runat="server" Text="موافق" Width="159px" Enabled="False" /></td>
               </tr>
            </table>
            </div>
    </form>
</body>
</html>
