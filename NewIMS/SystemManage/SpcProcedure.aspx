<%@ Page Language="VB" AutoEventWireup="false" Inherits="SystemManage_SpcProcedure" Codebehind="SpcProcedure.aspx.vb" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>≈Ã—«¡«  Œ«’…</title>
    <link href="../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function ReturnToParentPage() {
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup(1);
        }

        function ValidateS(s, e) {
            //if (ASPxClientEdit.ValidateGroup('Data'))
            s.SetText('·≈ „«„ ⁄„·Ì… «·≈·€«¡ Ì—ÃÏ «·÷€ÿ ⁄·Ï  ”ÃÌ·');
            s.SetEnabled(false);
            cbp.PerformCallback('Cancel100');
        }
        function ValidateR(s, e) {
            //if (ASPxClientEdit.ValidateGroup('Data'))
            s.SetText('·≈ „«„ ⁄„·Ì… «·„— œ Ì—ÃÏ «·÷€ÿ ⁄·Ï  ”ÃÌ·');
            s.SetEnabled(false);
            cbp.PerformCallback('Refund');
        }
        function ValidateC(s, e) {
            //if (ASPxClientEdit.ValidateGroup('Data'))
            s.SetText(' „  ⁄„·Ì… «·≈·€«¡');
            s.SetEnabled(false);
            cbp.PerformCallback('ConfirmCancel');

            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup(1);
        }
           function CalcRefund(s, e) {
           //debugger;
           <%-- var Syst = getParameterByName('Sys');
            var Order = document.getElementById('<%=OrderNo.ClientID%>').value;--%>
             if (ASPxClientEdit.ValidateGroup('Data') ) {
                 var dt = RefundFromDate.GetInputElement().value;
                 tmpdate.SetText(dt);
                 //RefundFromDate.SetText(dt);
                 cbp.PerformCallback('CalcRefund|' + dt);

            }
            else
            {

           //cbp.PerformCallback('ConfirmRefund');
                }

                
            }

         function Validate(s, e) {
           //debugger;
           <%-- var Syst = getParameterByName('Sys');
            var Order = document.getElementById('<%=OrderNo.ClientID%>').value;--%>
             if (ASPxClientEdit.ValidateGroup('Data') ) {
                 var dt = RefundFromDate.GetInputElement().value;
                 cbp.PerformCallback('ConfirmRefund|' + dt);

                 var parentWindow = window.parent;
                 parentWindow.SelectAndClosePopup(1);
            }
            else
            {

           //cbp.PerformCallback('ConfirmRefund');
                }

                
            }

    </script>
   
</head>
<body>
    <form id="form1" runat="server">

        <dx:aspxcallbackpanel id="Callback" runat="server" clientinstancename="cbp" oncallback="Callback_Callback">
            
            <SettingsLoadingPanel Text=" «Õ ”«» &amp;hellip; " Delay="100" Enabled="true" ShowImage="true" />
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <table dir="rtl" style="width: 100%;">
                        <tr>
                            <td class="dx-al">—ﬁ„ «·ÊÀÌﬁ…</td>
                            <td>
                                <dx:ASPxTextBox ID="PolNo" runat="server" Width="170px" ClientEnabled="false">
                                </dx:ASPxTextBox>
                            </td>
                            <td class="dx-al" colspan="2">—ﬁ„ «·ÿ·»</td>
                            <td>
                                <dx:ASPxTextBox ID="OrderNo" runat="server" Width="170px">
                                </dx:ASPxTextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="dxeICC">—ﬁ„ «·„·Õﬁ</td>
                            <td>
                                <dx:ASPxTextBox ID="EndNo" runat="server" Width="170px" ClientEnabled="false">
                                </dx:ASPxTextBox>
                            </td>
                            <td class="dx-al" colspan="2">—ﬁ„ «·≈‘⁄«—</td>
                            <td class="auto-style2" colspan="2">
                                <dx:ASPxTextBox ID="LoadNo" runat="server" Width="170px">
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="dxeICC">«”„ «·„ƒ„‰ ·Â</td>
                            <td  colspan="2">
                                <dx:ASPxTextBox ID="CustName" runat="server" Width="100%">
                                </dx:ASPxTextBox>
                            </td>
                            <td class="dx-al">—ﬁ„Â</td>
                            <td colspan="2" >
                                <dx:ASPxTextBox ID="CustNo" runat="server" ClientEnabled="False" ClientInstanceName="CustNo" Width="170px">
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="dxeICC">» «—ÌŒ</td>
                            <td  colspan="2">
                                <dx:ASPxTextBox ID="IssuDate" runat="server" Width="170px">
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style4">&nbsp;</td>
                            <td colspan="2" >
                                <dx:ASPxTextBox ID="tmpdate" ClientInstanceName="tmpdate" runat="server" Width="170px" ClientEnabled="false">
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style2" style="text-align: left">«· €ÿÌ… „‰</td>
                            <td  colspan="2" class="auto-style1">
                                <dx:ASPxTextBox ID="CoverFrom" runat="server" Width="170px" ClientEnabled="false">
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style2" style="text-align: left">«·Ï</td>
                            <td colspan="2">
                                <dx:ASPxTextBox ID="CoverTo" runat="server" Width="170px" ClientEnabled="false">
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Line">
                                &nbsp;</td>
                            <td  class="Line" colspan="2">
                                <dx:ASPxButton ID="Cancel" runat="server" AutoPostBack="False" ClientInstanceName="Issue" Text="≈·€«¡ «·ÊÀÌﬁ… 100%">
                                    <ClientSideEvents Click="ValidateS" />
                                </dx:ASPxButton>
                            </td>
                            <td class="Line">
                                <dx:ASPxButton ID="Refund" runat="server" AutoPostBack="False" ClientInstanceName="Issue" Text="„·Õﬁ „— œ">
                                    <ClientSideEvents Click="ValidateR" />
                                </dx:ASPxButton>
                            </td>
                            <td class="Line" colspan="2">
                                <asp:TextBox ID="SerialNo" runat="server" AutoPostBack="True" CssClass="PremText" Enabled="False" Width="101px">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Line"></td>
                            <td class="Line">
                                <table runat="server" id="Table1" visible="false" dir="rtl" style="width: 100%;">
                                    <tr>
                                        <td class="dxeICC">ÌÕ ”» «·ﬁ”ÿ «·„— œ&nbsp;„‰  «—ÌŒ </td>
                                        <td>
                                            <dx:ASPxDateEdit runat="server" EditFormat="Custom" EditFormatString="yyyy/MM/dd" Width="110px" RightToLeft="True" 
                                                DisplayFormatString="yyyy/MM/dd" ClientInstanceName="RefundFromDate" ID="RefundFrom">
                                                <ClientSideEvents ValueChanged="CalcRefund"></ClientSideEvents>

                                               
                                            </dx:ASPxDateEdit>

                                
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="dxeICC">√Ê »‰”»… «” —Ã«⁄ „‰ ’«›Ì «·ﬁ”ÿ «·√’·Ì :</td>
                                        <td runat="server">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="dxeICC">—ﬁ„ «·ÊÀÌﬁ… «·»œÌ·…</td>
                                        <td >
                                            <dx:ASPxTextBox ID="OldPolicy" runat="server" Width="170px">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
<tr runat="server"><td runat="server" class="dxeICC">
                                            
                                        &nbsp;</td>
    <td runat="server">&nbsp;</td>
</tr>
                                </table>
                            </td>
                            <td class="Line" colspan="2">
                                <%--<radCln:RadDatePicker ID="RadDatePicker1" runat="server" Culture="en-US" SelectedDate="1980-01-01" SharedCalendarID="" Width="100px">
                                    <Calendar runat="server" FocusedDate="2007-02-21">
                                    </Calendar>
                                    <DateInput CatalogIconImageUrl="" DateFormat="yyyy/MM/dd" Description="" DisplayPromptChar="_" PromptChar=" " Title="" TitleIconImageUrl="" TitleUrl=""></DateInput>
                                </radCln:RadDatePicker>--%>
                            </td>
                            <td class="Line" colspan="2"></td>
                        </tr>
                        <tr>
                            <td class="dxeICC">’«›Ì «·ﬁ”ÿ</td>
                            <td>
                                <dx:ASPxTextBox ID="NETPRM" runat="server" Text="0" Width="170px" ClientEnabled="false" ForeColor="#ff0000" Font-Bold="true">
                                </dx:ASPxTextBox>
                            </td>
                            <td colspan="2">
                                <dx:ASPxComboBox ID="Currency" runat="server" ClientEnabled="False" ClientInstanceName="Currency" DataSourceID="Cur" DropDownStyle="DropDown" RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" Width="110px">
                                    <ClientSideEvents GotFocus="function(s, e) {  }" SelectedIndexChanged="function(s, e) {
                                                    cbp.PerformCallback('ExRate');  }" />
                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="lastNet" runat="server" AutoPostBack="True" CssClass="PremText"
                                    Enabled="False" Width="101px" Visible="False">0</asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="dxeICC">œ„€… «· ’—›</td>
                            <td>
                                <dx:ASPxTextBox ID="TAXPRM" runat="server" Text="0" Width="170px" ClientEnabled="false" ForeColor="#ff0000" Font-Bold="false">
                                </dx:ASPxTextBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="Exchange" runat="server" AutoPostBack="True" CssClass="PremText" Enabled="False" Width="101px">0</asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="PayType" runat="server" AutoPostBack="True" CssClass="PremText" Enabled="False" Visible="False" Width="101px">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="dxeICC">—.«·≈‘—«› Ê«—ﬁ«»…</td>
                            <td>
                                <dx:ASPxTextBox ID="CONPRM" runat="server" Text="0" Width="170px" ClientEnabled="false" ForeColor="#ff0000" Font-Bold="false">
                                </dx:ASPxTextBox>
                            </td>
                            <td colspan="2">
                                <asp:SqlDataSource ID="Cur" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Cur'"></asp:SqlDataSource>
                            </td>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="dxeICC">œ„€… «·„Õ——« </td>
                            <td>
                                <dx:ASPxTextBox ID="STMPRM" runat="server" Text="0" Width="170px" ClientEnabled="false" ForeColor="#ff0000" Font-Bold="false">
                                </dx:ASPxTextBox>
                            </td>
                            <td colspan="2">&nbsp;</td>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="dxeICC">„’«—Ì› ≈’œ«—</td>
                            <td>
                                <dx:ASPxTextBox ID="ISSPRM" runat="server" Text="0" Width="170px" ClientEnabled="false" ForeColor="#ff0000" Font-Bold="false">
                                </dx:ASPxTextBox>
                            </td>
                            <td colspan="2">
                                <dx:ASPxButton ID="OKRefund" runat="server" AutoPostBack="False" ClientInstanceName="Confirm" Enabled="False" Text="≈’œ«—">
                                    <ClientSideEvents Click="Validate" />
                                </dx:ASPxButton>
                            </td>
                            <td colspan="2">
                                <dx:ASPxTextBox ID="Br" runat="server" ClientEnabled="False" ClientInstanceName="Br" ClientVisible="false" Width="170px">
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="dxeICC">«·≈Ã„«·‹‹‹‹‹‹‹‹‹‹‹Ì</td>
                            <td>
                                <dx:ASPxTextBox ID="TOTPRM" runat="server" Text="0" Width="170px" ClientEnabled="false" ForeColor="#ff0000" Font-Bold="true" >
                                </dx:ASPxTextBox>
                            </td>
                            <td colspan="4">
                                <dx:ASPxButton ID="OK" runat="server" AutoPostBack="False" ClientInstanceName="Confirm" Text=" ”ÃÌ·" Enabled="false">
                                    <ClientSideEvents Click="ValidateC" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:aspxcallbackpanel>
        
<%--    </div>--%>
    </form>
</body>
    </html>

