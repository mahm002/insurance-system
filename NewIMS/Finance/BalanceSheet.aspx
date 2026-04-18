<%@ Page Language="VB" AutoEventWireup="false" Inherits="BalanceSheet" Codebehind="BalanceSheet.aspx.vb" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
         <!-- <asp:ListItem Value=" (substring(accntnum,5,1) between '8' and '8' )" >«·ÕÌ«…</asp:ListItem>-->
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            width: 534px;
        }
        .style2
        {
            width: 180px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table height="100%" bgcolor="WhiteSmoke"  width="100%">
            <tr>
                <td>
                    „‰</td>
                <td align="left" class="style1">
                    <radcln:RadDatePicker/ id="Raddatepicker1" runat="server" width="130px">
                    <Calendar runat="server" FocusedDate="2007-02-22">
                    </Calendar>
                    <DateInput CatalogIconImageUrl="" DateFormat="yyyy/MM/dd" Description=""
                        DisplayPromptChar="_" PromptChar=" " Title="" TitleIconImageUrl="" TitleUrl=""></DateInput>
                </radcln:RadDatePicker>
                </td>
                <td class="style2">
                    &nbsp;
                    <asp:DropDownList ID="DropDownList1" runat="server" Visible="true">
                        <asp:ListItem Value=" (substring(accntnum,5,1) between 'M' and 'T' )"> Ã„Ì⁄Ì «·‘—ﬂ…</asp:ListItem>
                        <asp:ListItem Value=" (substring(accntnum,5,1) between 'T' and 'T' )">«·„—ﬂ“ «·—∆Ì”Ì</asp:ListItem>
                       <%-- <asp:ListItem Value=" (substring(accntnum,5,1) between '2' and '2' )">»‰€«“Ì</asp:ListItem>
                        <asp:ListItem Value=" (substring(accntnum,5,1) between '3' and '3' )">„’—« …</asp:ListItem>
                        <asp:ListItem Value=" (substring(accntnum,5,1) between '5' and '5' )">«·“«ÊÌ…</asp:ListItem>--%>
               
                    </asp:DropDownList></td>
                <td style="width: 474px; direction: rtl; text-align: left;">
                            <asp:CheckBox ID="CheckBox1" runat="server" Text=" ﬁ—Ì— «·›⁄·Ì »«· ﬁœÌ—Ì" 
                                TextAlign="Left" />
                            </td>
                <td style="width: 474px; direction: rtl;">
                            <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/images/txt1.png" NavigateUrl="~/Account/Default.aspx"
                                ToolTip="—ÃÊ⁄">HyperLink</asp:HyperLink>
                            </td>
            </tr>
            <tr>
                <td style="height: 29px;">
                    «·Ï</td>
                <td align="left" class="style1">
                    <radcln:raddatepicker id="RadDatePicker2" runat="server" width="130px">
                                <Calendar FocusedDate="2007-02-22" runat="server">
                                </Calendar>
                                <DateInput CatalogIconImageUrl="" DateFormat="yyyy/MM/dd" Description=""
                                    DisplayPromptChar="_" PromptChar=" " Title="" TitleIconImageUrl="" TitleUrl=""></DateInput>
                            </radcln:raddatepicker>
                </td>
                <td style="height: 29px; width: 474px;" colspan="3">
                    <asp:Button  ID="Button1" runat="server" Text="«⁄‹‹œ«œ «·„Ì‹‹“«‰" Font-Names="Tahoma" Font-Size="8pt" />
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/indicator.gif" Visible="False" /></td>
            </tr>
            <tr height="100%">
                <td colspan="5">
                    &nbsp;<asp:ScriptManager ID="ScriptManager1" runat="server" 
                        AsyncPostBackTimeout="0">
        </asp:ScriptManager>
            <rsweb:reportviewer id="rptViewer" runat="server" borderstyle="Solid" borderwidth="1px"
                        font-names="Verdana" font-size="8pt" height="800" processingmode="Remote" showbackbutton="True"
                        width="100%" Visible="False">
                        <SERVERREPORT DisplayName="Demo Report" />

                    </rsweb:reportviewer></td>
        </table>
    </div>
    </form>
</body>
</html>
