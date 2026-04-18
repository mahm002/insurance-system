<%@ Page Language="VB" AutoEventWireup="false" Inherits="TakafulyIMS.ClaimsManage_MoClaims_MoFollowup" Codebehind="MoFollowup.aspx.vb" %>
<%@ Register src="../WebUserControl.ascx" tagname="footer1" tagprefix="demos" %>
<%@ Register TagPrefix="eba" Namespace="eba.Web" Assembly="eba.Web" %>
<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

        <link href="~/TabsStyle.css" rel="stylesheet" type="text/css" />
           <script language="javascript" type="text/javascript">
               function SetEvent(item, IDName) {
                   if (document.all) {
                       if (event.keyCode == 13) {
                           event.returnValue = false;
                           event.cancel = true;
                           if (IDName == '')
                               document.getElementById(item).focus()
                           else
                               document.getElementById(IDName).object.SetFocus()
                       }
                   }
               }
               function SetOnSelectEvent(Elm, IDName) {
                   if (IDName == '')
                       document.getElementById(Elm).focus()
                   else
                       document.getElementById(IDName).object.SetFocus()
               }
               function openWindow() {
                   alert(document.URL);
               }
               
           </script>
    

    <style type="text/css">
        .style1
        {
            width: 53%;
            text-align: left;
        }
        .engtd
        {
            text-align: center;
        }
        .style2
        {
            height: 7px;
        }
    </style>
    

</head>
<body>
    <form id="form1" runat="server">
<script type="text/javascript">
    function PanelClick(sender, e) {
    }

    function ActiveTabChanged(sender, e) {
    }
</script>
    <div class="ArabicForm" align="center"> 
<br />
                        
                        <table style="width:100%;" width="50%">
                            <tr>
                                <td dir="rtl">
   <demos:footer1   runat="server" id="pageFooter" EnableTheming="true" EnableViewState="true"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="EngTD" dir="rtl">
                       <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                       ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
                        SelectCommand="SELECT     EXTRAINFO.TP, EXTRAINFO.TPNo, EXTRAINFO.TPName, MoClaimFile.ClmNo, MoClaimFile.GarageRef, MoClaimFile.EstimatedSpare, 
                        MoClaimFile.EstimatedRep, MoClaimFile.Total, MoClaimFile.ThirdParty, MoClaimFile.EntryDate, MoClaimFile.ThirdPartyName
                        FROM         EXTRAINFO INNER JOIN
                        MoClaimFile ON EXTRAINFO.TPNo = MoClaimFile.GarageRef AND EXTRAINFO.TP = 'garages'
                        WHERE     (MoClaimFile.ClmNo = @ClmNo)AND (MoClaimFile.ThirdParty=0) " 
                        UpdateCommand="UPDATE SET">
                        <SelectParameters>
                            <asp:FormParameter DefaultValue="0" FormField="ClmNo" Name="ClmNo" />
                        </SelectParameters>
                        
                       </asp:SqlDataSource>
                        
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
                SelectCommand="SELECT     EXTRAINFO.TP, EXTRAINFO.TPNo, EXTRAINFO.TPName, MoClaimFile.ClmNo, MoClaimFile.GarageRef, MoClaimFile.EstimatedSpare, 
                      MoClaimFile.EstimatedRep, MoClaimFile.Total, MoClaimFile.ThirdParty, MoClaimFile.EntryDate, MoClaimFile.ThirdPartyName
FROM         EXTRAINFO INNER JOIN
                      MoClaimFile ON EXTRAINFO.TPNo = MoClaimFile.GarageRef AND EXTRAINFO.TP = 'garages'
WHERE     (MoClaimFile.ClmNo = @ClmNo)AND (MoClaimFile.ThirdParty=1) " 
                UpdateCommand="UPDATE SET">
                        <SelectParameters>
                            <asp:FormParameter DefaultValue="0" FormField="ClmNo" Name="ClmNo" />
                        </SelectParameters>
                    </asp:SqlDataSource>
		
    <asp:TextBox ID="TPN" runat="server" Visible="False"></asp:TextBox>
    
    <asp:TextBox ID="Name" runat="server" Visible="False"></asp:TextBox>
    
                                </td>
                            </tr>
        </table>
		
        <table width="100%" dir="rtl">
            <tr>
                <td>
    <table dir="rtl" style="width: 98%;" class="Line">
        <tr>
            <td align="right" class="style1">
                 نوع الحادث<br />
            </td>
            <td align="right" width="50%" class="engtd" dir="rtl">
                  <eba:Combo ID="ClmType" runat="server" CSSClassName="MoClmType" 
                    DataTextField="TPNAME" DataValueField="TPNO" 
                    PostBackOnSelectEvent="true" PreconfiguredStylesheet="RecordSearch" 
                    Style="padding-bottom: 1px; padding-top: 1px" 
                    StylesheetURL="styles/RecordSearch/combo.css" TabIndex="1" Width="90%" 
                      DataLanguage="ar">
                    <List AllowPaging="False" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;" 
                        EnableDatabaseSearch="True" Height="120px" Width="100%" PageSize="3" />
                        <ListColumnDefinitionItems>
                            <eba:ListColumnDefinition Align="right" ColumnType="int" CSSClassName="MoClmType" 
                                DataFieldIndex="0" />
                            <eba:ListColumnDefinition ColumnType="String" 
                                CSSClassName="MoClmType" DataFieldIndex="1" />
                        </ListColumnDefinitionItems>
                    </eba:Combo>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                                                       
                 <table align="center" 
                style="padding: 1px; margin: 1px; color: #FFFFFF; font-style: normal;" 
                id="TABLE2" border="0" dir="rtl" 
                cellpadding="0" cellspacing="0" width="85%" 
        visible="False" class="ajax__tab_active">
               <tr bgcolor="#359623" dir="rtl">
                    <td align="center" bgcolor="#5D7B9D" ForeColor="White" dir="rtl" 
                       class="style36" colspan="2">
                        التقدير</td>
                    <td align="center" bgcolor="#5D7B9D" ForeColor="White" class="style34">
                        تقدير الإصلاحات</td>
                    <td align="center" bgcolor="#5D7B9D" ForeColor="White" class="style34">
                        تقدير قطع الغيار</td>
                    <td align="right" bgcolor="#5D7B9D" dir="rtl" class="style35">
                        الفترة المقدرة</td>
                    <td align="right" bgcolor="#5D7B9D" ForeColor="White" dir="rtl">
                        المجموع</td>
                </tr>
               <tr bgcolor="#359623" dir="rtl">
                   <td align="center" bgcolor="White"  ForeColor="White" class="EngTD" colspan="2" 
                       dir="rtl">
                  <eba:Combo ID="GarageRef" runat="server" CSSClassName="Garages" 
                    DataTextField="TPNAME" DataValueField="TPNO" 
                    OnSelectEvent="SetOnSelectEvent('TextBox3','')" 
                    PostBackOnSelectEvent="false" PreconfiguredStylesheet="RecordSearch" 
                    Style="padding-bottom: 1px; padding-top: 1px" 
                    StylesheetURL="styles/RecordSearch/combo.css" TabIndex="1" Width="100%" 
                           DataLanguage="ar">
                    <List AllowPaging="True" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;" 
                        EnableDatabaseSearch="True" Height="120px" Width="100%" PageSize="7" />
                        <ListColumnDefinitionItems>
                            <eba:ListColumnDefinition Align="right" ColumnType="int" CSSClassName="Garages" 
                                DataFieldIndex="0" />
                            <eba:ListColumnDefinition ColumnType="String" 
                                CSSClassName="Garages" DataFieldIndex="1" />
                        </ListColumnDefinitionItems>
                    </eba:Combo>
                   </td>
                    <td align="center" bgcolor="White" ForeColor="White" dir="rtl" 
                       class="style37">
                        <asp:TextBox ID="RepairValue" runat="server" 
                             TabIndex="7" CssClass="PremText1" 
                            Width="90%" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox></td>
                    <td align="center" bgcolor="White" ForeColor="White" class="style34">
                        <asp:TextBox ID="SpareValue" runat="server" 
                             TabIndex="8" Width="90%" 
                            CssClass="PremText1" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox></td>
                    <td align="right" bgcolor="White" ForeColor="White" class="ajax__tab_xp-theme">
                        <asp:TextBox ID="TimeExp" runat="server" 
                             TabIndex="9" 
                            CssClass="PremText1" Width="120px" BorderStyle="Solid" BorderWidth="1px" 
                            Visible="False"></asp:TextBox></td>
                    <td align="right" bgcolor="White" dir="rtl" class="style35">
                        <asp:TextBox ID="Total" runat="server" Width="90%" BorderStyle="Solid" 
                            BorderWidth="1px"></asp:TextBox>
                            </td>
                    <td align="center" bgcolor="White" ForeColor="White" dir="rtl">
                       <asp:Button ID="Button8" runat="server" 
                           Text="إضافة" BorderColor="LightSlateGray" BorderStyle="Solid" BorderWidth="2px" 
                           Width="153px" />
                            </td>
                </tr>
               <tr>
                   <td align="right" class="style32" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                       &nbsp;</td>
                    <td class="style36" align="right" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                        &nbsp;</td><td align="right" 
                       class="style37" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                       &nbsp;</td>
                    <td align="right" class="style34" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623">
                        &nbsp;</td>
                    <td align="right" class="style34" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623">
                        &nbsp;</td>
                    <td align="right" class="style35" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                        &nbsp;</td>
                    <td align="right" 
                       style="width: 126px; border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623;" 
                       dir="rtl">
                        &nbsp;</td>
                </tr>
            </table>
                                                       
            </td>
        </tr>
        <tr>
            <td colspan="2">
     
             <asp:DataGrid ID="GridView1" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" DataSourceID="SqlDataSource1" Font-Name="Verdana" 
        Font-Names="Verdana" Font-Size="10pt" ForeColor="#333333" GridLines="None" 
        HorizontalAlign="Center" Width="85%">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditItemStyle BackColor="#999999" />
        <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" 
            HorizontalAlign="Center" />
        <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:EditCommandColumn ButtonType="LinkButton" CancelText="إلغاء" 
                EditText="تحرير" UpdateText="تعديل">
                <ItemStyle HorizontalAlign="Center" />
            </asp:EditCommandColumn>
            <asp:BoundColumn DataField="TPNAME" Visible="False"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="الورشة">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "Tpname")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <eba:Combo ID="Garage" runat="server" CSSClassName="Garages" Width="100%"
                        DataSource="<%# GetInitialDataSource(10) %>" DataTextField="TPNAME" 
                        DataValueField="TpNo" Mode="Classic" PostBackOnSelectEvent="True"  TextValue=<%#DataBinder.Eval(Container.DataItem, "Tpname")%> 
                        PreconfiguredStylesheet="EBACombo" 
                        style="PADDING-BOTTOM: 1px; PADDING-TOP: 1px">
                        <List AllowPaging="True" EnableDatabaseSearch="True" PageSize="10" 
                            Width="500px" />
                        <TextBox DataFieldIndex="1" />
                        <ListColumnDefinitionItems>
                            <eba:ListColumnDefinition DataFieldIndex="1" Width="100%" />
                        </ListColumnDefinitionItems>
                    </eba:Combo>
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="TPNo" Visible="false">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "TpNo")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TPNO" runat="server" 
                        Text='<%# DataBinder.Eval(Container.DataItem, "TpNo") %>' Width="100px"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="تقدير قطع الغيار">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "estimatedspare")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtspare" runat="server" 
                        Text='<%# DataBinder.Eval(Container.DataItem, "estimatedspare") %>' 
                        Width="100px"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="تقدير اليد العاملة">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "estimatedrep")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtrep" runat="server" 
                        Text='<%# DataBinder.Eval(Container.DataItem, "estimatedrep") %>' 
                        Width="100px">
                                       </asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="المجموع">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "Total")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txttotal" runat="server" 
                        Text='<%# DataBinder.Eval(Container.DataItem, "total") %>' Width="100px">
                                       </asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
     
            </td>
        </tr>
    </table>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
            </tr>
        </table>
		
    <asp:Label ID="lblUpdate" runat="server" Text="Label"></asp:Label>
		
        <br />
		
        <br />
    
    
        <table align="center" cellpadding="0" cellspacing="0" 
            dir="ltr" width="100%">
               <tr>
                   <td align="center" class="style17" colspan="2" dir="rtl">

                       <br />
                       
                       <br />
                   </td>
               </tr>
               <tr>
                   <td align="left" colspan="2" class="Line">
                       &nbsp;</td>
               </tr>
               <tr>
                   <td class="style14">
                       &nbsp;</td>
                   <td style="height: 17px">
                       <div style="width: 100%; height: 1px" class="Line">
                       </div>
                        <asp:Button ID="Button9" runat="server" Text="تسجيل" 
                           BorderColor="LightSlateGray" BorderStyle="Solid" BorderWidth="2px" 
                           Width="153px" PostBackUrl="~/PolicyManagement/MOMOTOR/Issu.aspx?Submit=1" 
                           Enabled="False" />
                        <asp:Button ID="Button10" runat="server" Text="اصــدار" Width="151px" 
                           Enabled="False" />
                       <asp:HyperLink ID="HyperLink2" runat="server" Target="_self" NavigateUrl="~/PolicyManagement/Default.aspx">رجوع</asp:HyperLink></td>
               </tr>
            </table>
<br />
           </div>
      </form>
    </body>
</html>
