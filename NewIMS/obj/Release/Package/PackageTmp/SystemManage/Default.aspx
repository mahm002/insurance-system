<%@ Page Language="vb" MasterPageFile="~/Main.master" Inherits="SysDefault" ClassName="Default_aspx" CodeBehind="~/SystemManage/Default.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <%--<table  cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="vertical-align: top">
                    <radCln:RadDatePicker id="RadDatePicker1" Runat="server" Width="307px" Height="13px" Font-Names="Rod" Font-Bold="False" BorderStyle="None">
                    <PopupButton HoverImageUrl="~/images/longbutton.gif" ImageUrl="~/images/longbutton.gif"></PopupButton>
                    <Calendar FocusedDate="2007-03-25" Skin="web20">
                        <FastNavigationSettings CancelButtonCaption="الغاء" OkButtonCaption="موافق" TodayButtonCaption="اليوم" />
                    </Calendar>
                    <DateInput Font-Size="7pt" Font-Names="Tahoma" Font-Bold="True" Width="307px" ForeColor="Gray" BorderStyle="Solid" Title="" AutoPostBack="True" PromptChar=" " BackColor="#C0FFFF" TitleIconImageUrl="" DisplayPromptChar="_" BorderColor="Black" DateFormat="dddd yyyy/MM/dd" BorderWidth="1px" CatalogIconImageUrl="" TitleUrl="" Description=""></DateInput>
                    </radCln:RadDatePicker>
                </td>
                <td align="left">
            </td>
            </tr>
            <tr>
                <td colspan="2" style="border-bottom: thin groove;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="20%">
                              </td>
                <td align="left" width="80%">
               </td>
            </tr>
            <tr>
            <td align="right">
                    <asp:HyperLink id="HyperLink2" ToolTip="تعـديــل" runat="server" ImageUrl="~/Images/copy.gif" Target="_self"></asp:HyperLink> |
                    <asp:HyperLink id="HyperLink3" Target="_self" ToolTip="جديـد" runat="server" ImageUrl="~/Images/new.gif" ></asp:HyperLink>
                    <asp:TextBox id="OrderNo" runat="server" visible="true" Width="100px" Height="20px" valign="middle" Font-Names="arial" BorderWidth="1" BorderStyle="solid" BorderColor="Gray"></asp:TextBox>
                </td>
                <td align="left" width="20%">
                    <asp:imagebutton  ImageUrl="~/images/inserttable.gif" ToolTip="القيد المحاسبي"  id="Journal" runat="server" CausesValidation="False" ></asp:imagebutton>
                    <asp:imagebutton  ImageUrl="~/images/IsseClip.gif" Visible="false" ToolTip="حافظة الإنتاج"  id="IBTM" runat="server" CausesValidation="False" ></asp:imagebutton>
                </td>
            </tr>
        </table>--%>
    <%--<table width="100%"  cellpadding="1">
    <tr>
      <td>
        <asp:GridView ID="GridView1" Font-Size="8pt" runat="server" AllowSorting="True"
            AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333"
            GridLines="None" Height="37px" HorizontalAlign="Right" PageSize="7" Width="100%">
            <PagerSettings FirstPageImageUrl="~/images/arrow_back.gif" LastPageImageUrl="~/images/arrow_forward.gif"
                NextPageImageUrl="~/images/bullet.gif" PageButtonCount="7" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/images/txt.gif" />
                <asp:ButtonField ButtonType="Image" CommandName="Print" ImageUrl="~/images/print.gif"
                    Text="Button" />
                <asp:BoundField DataField="DailyNum" HeaderText="رقم القيد">
                    <ItemStyle HorizontalAlign="Right" Width="160px" />
                    <HeaderStyle Font-Names="Tahoma" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DailyDTe" HeaderText="تاريخ القيد" HtmlEncode="False"  DataFormatString="{0:dd/MM/yyyy}">
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                    <HeaderStyle Font-Names="Tahoma" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField ApplyFormatInEditMode="True" DataField="DailySRL"
                    HeaderText="رقـم القيد">
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                    <HeaderStyle Font-Names="Tahoma" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="AnalsNum" HeaderText="الرقم التحليلي">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Font-Names="Tahoma" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DailyPrv" HeaderText="حالة القيد">
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                    <HeaderStyle Font-Names="Tahoma" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Comment" HeaderText="شرح القيد">
                    <ItemStyle HorizontalAlign="Center" Width="500px" />
                    <HeaderStyle Font-Names="Tahoma" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:ButtonField ButtonType="Image" ImageUrl="~/images/Accept.gif" Text="Special" CommandName="Special" />
                <asp:ButtonField ButtonType="Image" Visible=false ImageUrl="~/images/delete.gif" Text="DelReq" CommandName="DelReq" />
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#C00000" BorderColor="#316AC5" BorderStyle="Solid" BorderWidth="1px"
                Font-Bold="True" ForeColor="#E0E0E0" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle  CssClass="gridhead" Height="10" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString%>">
        </asp:SqlDataSource>
      </td>
    </tr>
  </table>--%>

    <%--  <table>
    <tr>
      <td>
        <h3 class=""></h3>
      </td>
    </tr>
    <tr  visible=false>
      <td  style=" color:White; width:100%">
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
      </td>
    </tr>
  </table>
			<cc1:msgBox id="MsgBox1" style="Z-INDEX: 103; LEFT: 536px; POSITION: absolute; TOP: 184px" runat="server"></cc1:msgBox>--%>
</asp:Content>