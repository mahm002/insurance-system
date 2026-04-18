<%@ Control Language="VB" AutoEventWireup="true" Inherits="WebUserControl" Strict="false" Explicit="false"   Codebehind="WebUserControl.ascx.vb" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register TagPrefix="eba" Namespace="eba.Web" Assembly="eba.Web" %>
<%@ Register TagPrefix="radcln" Namespace="Telerik.WebControls" Assembly="RadCalendar.Net2" %>

	<head>
	
        <link href="../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
       
        <style type="text/css">
            .style1
            {
                width: 912px;
            }
            .style2
            {
                -moz-border-radius: 10px;
                -webkit-border-radius: 10px 10px;
                -khtml-border-radius: 10px;
                border-radius: 10px;
                text-align: right;
                font-size: 14px;
                font-family: arial;
                font-weight: bold;
                color: #6C7B30;
                height: 29px;
                border-left: 1px solid #000000;
                border-right: 0px solid #000000;
                border-top: 0px solid #000000;
                border-bottom: 1px solid #000000;
                background: url('../Styles/images/dashboard_h_bg_hover.png') repeat-x 0px 0px;
            }
        </style>
       
</head>
	<LINK href="../Styles/MainSiteStyle.css" type="text/css" rel="stylesheet"/>
<script language="javascript" type="text/javascript">
// <!CDATA[


// ]]>
</script>


<div align="center" dir="rtl" >
    <table border="0" cellpadding="0" cellspacing="0" dir="rtl" width="100%" id="TABLE1" >
        <tr>
            <td align="right" dir="rtl" class="CaptionM">
                &nbsp;<span > لحــــــــــادث </span>&nbsp;<asp:TextBox ID="ClmNo" 
                    runat="server" BackColor="LightSteelBlue" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Width="100px" 
                    style="text-align: right"></asp:TextBox>&nbsp;</td>
            <td align="right" dir="rtl" class="CaptionM">
                <asp:HyperLink ID="HyperLink2" runat="server">متفرقات</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="right" 
                style="border-bottom: #359623 thin solid; color: #000080;" class="style1" 
                colspan="2">
                &nbsp;رقم الوثيقــة&nbsp; <span lang="ar-sa">&nbsp;</span><asp:TextBox ID="PolNo" runat="server" onkeypress="SetEvent('pageFooter_EndN','')" BorderColor="ActiveCaption" Width="100px" BackColor="White" AutoPostBack="True" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox> &nbsp; &nbsp;&nbsp; رقم الملحق &nbsp;&nbsp;<asp:TextBox ID="EndNo" runat="server" BackColor="White" ForeColor="Black" Width="54px" CssClass="PremText1" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;&nbsp;
                <asp:Label ID="Label3" runat="server" Text="Label" Visible="False" Width="88px"></asp:Label>&nbsp;<asp:TextBox ID="LoadNo" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                    Width="40px" Visible="False"></asp:TextBox><asp:TextBox ID="groupno" runat="server" AutoPostBack="True" Width="40px" BorderStyle="Solid" BorderWidth="1px" Visible="False"></asp:TextBox>
                &nbsp;&nbsp; تاريخ الإصدار
            <asp:TextBox ID="IssuDate" runat="server" Width="74px" BackColor="White" BorderColor="Transparent" BorderWidth="1px" BorderStyle="Solid"></asp:TextBox>
                <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/images/back.png" 
                    ToolTip="رجوع">HyperLink</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="right" 
                
                
                
                style="border-bottom: #359623 thin solid; height: 24px; width: 100%; color: #000080;" 
                colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" 
                
                
                
                style="border-bottom: #359623 thin solid; height: 24px; width: 100%; color: #000080;" 
                colspan="2">
                &nbsp;اسم الزبــون
                    &nbsp;<asp:TextBox AutoPostBack="true" ID="CustNo" runat="server" onkeypress="SetEvent('pageFooter_AgentNo','')"
                                Width="272px" CssClass="PremText1" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                <asp:TextBox ID="CustName" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Visible="False" Width="73px"></asp:TextBox>
                <span lang="ar-sa">&nbsp; </span>&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" style="border-bottom: #359623 thin solid; color: #000080;" 
                colspan="2">
                &nbsp;تاريخ البلاغ &nbsp;
                <asp:TextBox ID="ClmInfDate" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="0px" Width="88px"></asp:TextBox>
                &nbsp; تاريخ الحادث &nbsp;<asp:TextBox ID="ClmDate" runat="server" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="0px" Width="88px"></asp:TextBox></td>
        </tr>
    </table>
</div>
<div id="MoDiv" visible=false runat=server dir="rtl">
           <table align="center" 
                style="padding: 1px; margin: 1px; color: #FFFFFF; font-style: normal;" 
                id="TABLE2" border="0" dir="rtl" 
                cellpadding="0" cellspacing="0" width="100%" 
        visible="False">
               <tr bgcolor="#359623" dir="rtl">
                   <td align="center"  ForeColor="White" class="CaptionRED" colspan="7">
                        <b style="text-align: center">المؤمن له</b></td>
                </tr>
               <tr bgcolor="#359623" dir="rtl">
                   <td align="right"  ForeColor="White" class="Caption">
                        رقم الهيكل</td>
                    <td align="right" ForeColor="White" dir="rtl" 
                       class="Caption">
                        رقم اللوحة</td>
                    <td align="right" ForeColor="White" dir="rtl" 
                       class="Caption">
                        رقم المحرك</td>
                    <td align="right" ForeColor="White" class="Caption">
                        نوع السيارة</td>
                    <td align="right" ForeColor="White" class="Caption">
                        صافي القسط</td>
                    <td align="right" dir="rtl" class="Caption">
                        مبلغ التأمين</td>
                    <td align="right" ForeColor="White" dir="rtl" class="Caption">
                        بند التحمل</td>
                </tr>
               <tr>
                   <td align="right" class="style32" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                        <asp:TextBox ID="BudyNo" runat="server" 
                             TabIndex="7" CssClass="PremText1" 
                            Width="120px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox></td>
                    <td class="style36" align="right" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                        <asp:TextBox ID="TableNo" runat="server" 
                             TabIndex="8" Width="102px" 
                            CssClass="PremText1" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox></td><td align="right" 
                       class="style37" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                        <asp:TextBox ID="EnginNo" runat="server" 
                             TabIndex="9" 
                            CssClass="PremText1" Width="108px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox></td>
                    <td align="right" class="style34" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623">
                        <asp:TextBox ID="cartype" runat="server" Width="95px" BorderStyle="Solid" 
                            BorderWidth="1px"></asp:TextBox>
                            </td>
                    <td align="right" class="style34" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623">
                       <asp:TextBox ID="Premium" runat="server" CssClass="PremText1" 
                           TabIndex="16" Width="120px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                            </td>
                    <td align="right" class="style35" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                       <asp:TextBox ID="SumIns" 
                            runat="server" CssClass="PremText1" 
                           TabIndex="16" Width="120px" AutoPostBack="True" BorderStyle="Solid" 
                            BorderWidth="1px"></asp:TextBox></td>
                    <td align="right" 
                       style="width: 126px; border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623;" 
                       dir="rtl">
                       <asp:TextBox ID="RespVal" runat="server" CssClass="PremText1" 
                           TabIndex="16" Width="120px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                     </td>
                </tr>
            </table>

            </div>
            <div id="MoTp" visible=false runat=server dir="rtl">
                       <table align="center" 
                style="padding: 1px; margin: 1px; color: #FFFFFF; font-style: normal;" 
                id="TABLE3" border="0" dir="rtl" 
                cellpadding="0" cellspacing="0" width="100%" 
        visible="False">
               <tr bgcolor="#359623" dir="rtl">
                   <td align="center"  ForeColor="White" class="CaptionRED" colspan="5">
                        <b style="text-align: center">الطرف الثالث</b></td>
                </tr>
               <tr bgcolor="#359623" dir="rtl">
                   <td align="right"  ForeColor="White" class="Caption">
                        اسم المالك (الطرف الثالث)</td>
                    <td align="right" ForeColor="White" dir="rtl" 
                       class="Caption" colspan="2">
                        البيان</td>
                    <td align="right" ForeColor="White" class="Caption">
                        الأضرار</td>
                    <td align="right" ForeColor="White" dir="rtl" class="Caption">
                        قيمة الأضرار</td>
                </tr>
               <tr>
                   <td align="right" class="style32" 
                       
                       style="border-bottom: thin solid #359623; text-align: center;" 
                       dir="rtl">
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="TP" 
                            DataTextField="ThirdParty" Width="254px" DataValueField="TPID" 
                            AppendDataBoundItems="true" AutoPostBack="True">
                            <asp:ListItem Text="المؤمن له" Value=0 />
                        </asp:DropDownList>
                   </td>
                    <td align="right" 
                       class="style37" 
                       
                       style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #359623" 
                       dir="rtl">
                        &nbsp;</td>
                    <td align="right" class="style34" 
                       
                       style="border-bottom: thin solid #359623; text-align: center;">
                        <asp:TextBox ID="Asset" runat="server" Width="257px" BorderStyle="Solid" 
                            BorderWidth="1px" style="text-align: center"></asp:TextBox>
                       <asp:SqlDataSource ID="TP" runat="server"
                       ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
                        
                        UpdateCommand="UPDATE SET">
                        <SelectParameters>
                            <asp:FormParameter DefaultValue="0" FormField="ClmNo" Name="ClmNo" />
                        </SelectParameters>
                        
                        
                       </asp:SqlDataSource>
                            </td>
                    <td align="right" class="style34" 
                       
                       style="border-bottom: thin solid #359623; text-align: center;">
                       <asp:TextBox ID="Damage" 
                            runat="server" CssClass="PremText1" 
                           TabIndex="16" Width="247px" AutoPostBack="True" BorderStyle="Solid" 
                            BorderWidth="1px"></asp:TextBox>
                            </td>
                    <td align="right" 
                       style="border-bottom: thin solid #359623; width: 126px; text-align: center;" 
                       dir="rtl">
                       <asp:TextBox ID="DMGValue" runat="server" CssClass="PremText1" 
                           TabIndex="16" Width="85px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                     </td>
                </tr>
            </table>
            </div>

            <div id="DIV1" runat="server" visible="false" class="style11">
                <table align="center" style="padding-right: 2px; padding-left: 2px; padding-bottom: 0px; padding-top: 0px" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="width: 126px; height: 43px;">
                            <asp:TextBox ID="CoverFrom" runat="server" class="ComboBoxText" CssClass="PremText1"
                                            Height="22px" type="text" Width="98px" Visible="False"></asp:TextBox></td>
                        <td style="width: 105px; height: 43px;" align="right">
                            &nbsp;<eba:Combo ID="Cover" runat="server" 
                                postbackonselectevent="true" preconfiguredstylesheet="RecordSearch" Style="padding-bottom: 1px;
                                padding-top: 1px" StylesheetURL="~/styles/RecordSearch/combo.css" Width="100px" DataTextField="CoverName" DataValueField="CoverNo" Visible="False">
                                <List AllowPaging="True" CustomHTMLDefinition="${0} &lt;span style='color:#6684A0;font-style:italic;'&gt; ${1} &lt;/span&gt;"
                                    EnableDatabaseSearch="True" Height="120px" PageSize="7" Width="360px" />
                            </eba:Combo>
                            &nbsp;
                            <asp:TextBox ID="CoverTo" runat="server" Width="100px" CssClass="PremText1" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="textbox2" runat="server" Width="127px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox><asp:TextBox ID="OrderNo" runat="server" BorderColor="Black" CssClass="EngTD"
                                Width="100px" BorderStyle="Solid" BorderWidth="1px" BackColor="White" ForeColor="Black"></asp:TextBox>
                            <asp:TextBox ID="tasno" runat="server" Width="80px"></asp:TextBox>
                            <asp:Button ID="Button2" runat="server" Text="Button" /><asp:Button ID="Button1" runat="server" Text="Button" Visible="False" />
                            </td>
                        <td align="left" class="EngTD" style="width: 119px; height: 43px;">
                            &nbsp;<asp:TextBox ID="boardno" runat="server" Width="77px" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="engno" runat="server" Width="75px" Visible="False" Height="22px"></asp:TextBox>
                            <asp:TextBox ID="bodyno" runat="server" Width="65px" Visible="False" 
                                Height="16px"></asp:TextBox>
                            <asp:TextBox ID="persor" runat="server" Width="89px" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="carperm" runat="server" Width="93px" Visible="False"></asp:TextBox>&nbsp;
                            <asp:TextBox ID="madeyear" runat="server" Width="91px" Visible="False"></asp:TextBox><asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                <br />



            </div>




