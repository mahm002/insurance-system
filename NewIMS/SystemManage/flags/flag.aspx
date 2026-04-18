<%@ Page Language="VB" AutoEventWireup="false" Inherits="Flag" CodeBehind="flag.aspx.vb" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>الأدلة </title>
    <link rel="stylesheet" type="text/css" href="~/OutputManagement/default.css" />
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

    <style type="text/css">
        .auto-style1 {
            text-align: left;
        }

        .auto-style3 {
            position: static;
            height: 10px;
        }
    </style>
</head>
<body onload="SetEvent('pageFooter_CustName','');">
    <form id="Form1" method="post" runat="server" class="EngTD" dir="rtl">
        <div id="pagetop" class="ArabicForm" style="z-index: 101" nowrap="nowrap">
            <table id="TABLE1" width="100%" runat="server"
                visible="false">
                <tbody>
                    <tr>
                        <td colspan="2" style="height: 24px">&nbsp;</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="ArabicForm">
            &nbsp;
            <div style="width: 100%; height: 1px;" class="Line" align="right">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            </div>
            &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
           <table style="padding-right: 1px; padding-left: 1px; padding-bottom: 1px; margin: 1px; padding-top: 1px; width: 100%;"
               id="TABLE2"
               onclick="return TABLE2_onclick()" border="0">
               <tr>
                   <td
                       colspan="2" class="dx-ac">
                       <strong>اضافه ادلة</strong></td>
               </tr>
               <tr>
                   <td class="dx-al">
                       <strong>اسم الدليل</strong></td>
                   <td class="engtd" style="position: static; height: 10px">
                       <dx:ASPxComboBox ID="Extras" runat="server" Width="100%" TextField="descRip" ValueField="tp" OnSelectedIndexChanged="Extras_SelectedIndexChanged" AutoPostBack="true">
                       </dx:ASPxComboBox>
                   </td>
               </tr>
               <tr>
                   <td style="width: 99px; position: static; height: 10px"></td>
                   <td class="auto-style3"
                       style="text-align: center;">
                       <dx:ASPxTextBox ID="TextBox1" runat="server" Width="100%">
                           <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                               <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+"></RegularExpression>

                               <RequiredField IsRequired="True"></RequiredField>
                           </ValidationSettings>
                       </dx:ASPxTextBox>
                   </td>
               </tr>
               <tr>
                   <td style="width: 99px; position: static; height: 10px">
                       <asp:Label ID="Label3" runat="server" Font-Names="Tahoma" CssClass="Caption" Style="text-align: left" Text="سنة الصنع" Visible="False" Width="216px"></asp:Label>
                   </td>
                   <td class="auto-style1">
                       <dx:ASPxTextBox ID="TextBox2" runat="server" Width="100%">
                           <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                               <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+"></RegularExpression>

                               <RequiredField IsRequired="True"></RequiredField>
                           </ValidationSettings>
                       </dx:ASPxTextBox>
                   </td>
               </tr>
               <tr>
                   <td style="width: 99px; position: static; height: 10px">
                       <asp:Label ID="Label2" runat="server" Font-Names="Tahoma" CssClass="Caption" Style="text-align: left" Text="جنسية السفينة" Visible="False" Width="216px"></asp:Label>
                   </td>
                   <td class="auto-style3">
                       <dx:ASPxTextBox ID="TextBox3" runat="server" Width="100%">
                           <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                               <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+"></RegularExpression>

                               <RequiredField IsRequired="True"></RequiredField>
                           </ValidationSettings>
                       </dx:ASPxTextBox>
                   </td>
               </tr>
               <tr>
                   <td style="width: 99px; position: static; height: 10px">
                       <asp:Label ID="Label4" runat="server" Font-Names="Tahoma" CssClass="Caption" Style="text-align: left" Text="مادة الصنع" Visible="False" Width="216px"></asp:Label>
                   </td>
                   <td class="auto-style3">
                       <dx:ASPxTextBox ID="TextBox4" runat="server" Width="100%">
                           <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                               <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+"></RegularExpression>

                               <RequiredField IsRequired="True"></RequiredField>
                           </ValidationSettings>
                       </dx:ASPxTextBox>
                   </td>
               </tr>
               <tr>
                   <td style="width: 99px; position: static; height: 10px">
                       <asp:Label ID="Label5" runat="server" Font-Names="Tahoma" CssClass="Caption" Style="text-align: left" Text="الاستعمال" Visible="False" Width="215px"></asp:Label>
                   </td>
                   <td class="auto-style3">
                       <dx:ASPxTextBox ID="TextBox5" runat="server" Width="100%">
                           <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                               <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+"></RegularExpression>

                               <RequiredField IsRequired="True"></RequiredField>
                           </ValidationSettings>
                       </dx:ASPxTextBox>
                   </td>
               </tr>
               <tr>
                   <td style="width: 99px; position: static; height: 10px">&nbsp;</td>
                   <td class="auto-style3">
                       <asp:Button ID="Button2" runat="server" Text="حفظ" Visible="False"
                           Width="107px" CssClass="CaptionM" />
                   </td>
               </tr>
               <tr>
                   <td colspan="2" style="position: static; height: 10px" dir="rtl">
                       <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1"
                           Width="100%" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                           GridLines="None" AllowPaging="True" PageSize="15" CssClass="mGrid">
                           <Columns>
                               <asp:BoundField DataField="TpNo" HeaderText="الرقم" />
                               <asp:BoundField DataField="TpName" HeaderText="البيان" />
                               <asp:ButtonField ButtonType="Image" CommandName="addnew" ImageUrl="~/images/add.png"
                                   Text="addnew" />
                           </Columns>
                           <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                           <RowStyle BackColor="#E3EAEB" />
                           <EditRowStyle BackColor="#7C6F57" />
                           <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                           <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                           <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                           <AlternatingRowStyle BackColor="White" />
                       </asp:GridView>
                       <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString%>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"></asp:SqlDataSource>
                   </td>
               </tr>
               <tr>
                   <td colspan="2" style="height: 17px">
                       <div style="width: 100%; height: 1px" class="Line">
                       </div>
                       <asp:Button ID="Button1" runat="server" Text="تسجيل" BorderColor="LightSlateGray" BorderStyle="Solid" BorderWidth="2px" Width="153px" PostBackUrl="~/PolicyManagement/CARGO/CARGO.aspx?Submit=1" Visible="False" />&nbsp;&nbsp;
                       &nbsp;&nbsp;
                   </td>
               </tr>
           </table>
            <%--<cc1:msgBox ID="MsgBox1" Style="z-index: 103; left: 536px; position: absolute; top: 184px" runat="server"></cc1:msgBox>--%>
        </div>
    </form>
</body>
</html>