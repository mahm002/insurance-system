<%@ Page Language="VB" AutoEventWireup="false" Inherits="UserForm" CodeBehind="UserForm.aspx.vb" %>

<head runat="server">
    <title>„” Œœ„Ì‰ </title>
    <link rel="shortcut icon" href="~/include/LOGO.ico" />
    <link href="../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
    <script lang="javascript" type="text/javascript">
     
        document.addEventListener("contextmenu", function (e) {
            e.preventDefault();
        }, false);
    </script>
    <style type="text/css">
        .auto-style1 {
            text-align: left;
            height: 30px;
        }

        .auto-style2 {
            text-align: right;
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dx-ac">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents EndCallback="" />
                <SettingsLoadingPanel Text="«Õ ”«»&amp;hellip;" Delay="100" ShowImage="false" Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table dir="rtl" style="width: 100%">
                            <tr>
                                <td class="auto-style1" dir="rtl" style="text-align: left">«”„ «·„” Œœ„</td>
                                <td>
                                    <dx:ASPxTextBox ID="AccountName" runat="server" Width="100%">
                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="  "
                                                ValidationExpression="^[A-Za-z0-9«√≈.-Ì¡∆ \\-]{5,50}$" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">«·√‰Ÿ„…</td>
                                <td dir="ltr" colspan="3">
                                    <dx:ASPxComboBox ID="RootSystems" runat="server" ClientInstanceName="RootSystems" EnableCallbackMode="True" OnSelectedIndexChanged="RootSystems_SelectedIndexChanged" RightToLeft="True" SelectedIndex="0" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { e.processOnServer=true; }" />
                                        <Items>
                                            <dx:ListEditItem Selected="True" Text="≈œ«—… «·≈’œ«—« " Value="1" />
                                            <dx:ListEditItem Text="≈œ«—… «·„ÿ«·»« " Value="2" />
                                            <dx:ListEditItem Text="«·Õ”«»« " Value="3" />
                                            <dx:ListEditItem Text="«·≈⁄«œ…" Value="4" />
                                            <dx:ListEditItem Text="«·√‰Ÿ„… «·≈œ«—Ì…" Value="5" />
                                            <dx:ListEditItem Text="«œ«—… «·‰Ÿ«„" Value="6" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al" dir="rtl">«”„ «·œŒÊ·</td>
                                <td>
                                    <dx:ASPxTextBox ID="AccountLogIn" runat="server" Width="100%">
                                       <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="  "
                                                ValidationExpression="^[A-Za-z0-9«√≈.-Ì¡∆ \\-]{5,50}$" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al">
                                    <asp:Label ID="Label3" runat="server" Text="«·›—⁄/«·ÊﬂÌ·"></asp:Label>
                                </td>
                                <td dir="ltr" colspan="3">
                                    <dx:ASPxComboBox ID="BranchesD" runat="server" DataSourceID="Branches" OnSelectedIndexChanged="RootSystems_SelectedIndexChanged" RightToLeft="True" SelectedIndex="0" TextField="BranchName" ValueField="BranchNo" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { e.processOnServer=true; }" />
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1" dir="rtl" colspan="2">
                                    <dx:ASPxTextBox ID="PassWord" runat="server" Caption="ﬂ·„… «·„—Ê—" Password="True" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="Õ—Ê› Ê√—ﬁ«„ Ê√‰ ·«  Õ ÊÌ ⁄·Ï „”«›« " ValidationExpression="^[0-9a-zA-Z#@«-Ì\s]+" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeICC" rowspan="2">
                                    <dx:ASPxLabel ID="lblMessage" runat="server">
                                    </dx:ASPxLabel>
                                </td>
                                <td dir="ltr" colspan="3" class="auto-style2">
                                    <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="Õ›Ÿ" UseSubmitBehavior="False" Width="100%">
                                        <ClientSideEvents Click="function(s, e) {
                                cbp.PerformCallback('Apply'); }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al" dir="rtl" colspan="2">
                                    <dx:ASPxTextBox ID="ConfirmPass" runat="server" Caption=" √ﬂÌœ ﬂ·„… «·„—Ê—" Password="True" Width="100%">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="Õ—Ê› Ê√—ﬁ«„ Ê√‰ ·«  Õ ÊÌ ⁄·Ï „”«›« " ValidationExpression="^[0-9a-zA-Z#@«-Ì\s]+" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td dir="ltr" colspan="3" class="dxeCaptionHARSys">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="6" class="dx-ac">
                                    <dx:ASPxCheckBoxList ID="SystemsList" runat="server"
                                        DataSourceID="SqlDataSource1" RepeatLayout="Table" RepeatColumns="3"
                                        TextField="SubSysName"
                                        ValueField="SubSysValue" Width="100%" RightToLeft="True">
                                    </dx:ASPxCheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="auto-style1" dir="rtl">«·„” ÊÏ</td>
                                <td colspan="4">
                                    <dx:ASPxComboBox ID="Rule" runat="server" ValueType="System.Int32" 
                                        DropDownStyle="DropDownList">
                                        <Items>
                                            <dx:ListEditItem Text="„ÊŸ›" Value="1" Selected="true" />
                                            <dx:ListEditItem Text="—∆Ì” ﬁ”„" Value="3" Selected="false" />
                                            <dx:ListEditItem Text="„œÌ— ≈œ«—…" Value="4" Selected="false" />
                                            <dx:ListEditItem Text="„”ƒÊ· ‰Ÿ«„" Value="5" Selected="false" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                    <%--                        <asp:DropDownList ID="DropDownList1" runat="server" Width="157px">
                            <asp:ListItem Selected="True" Value="1">„ÊŸ›</asp:ListItem>
                            <asp:ListItem Value="3">—∆Ì” ﬁ”„</asp:ListItem>
                            <asp:ListItem Value="4">„œÌ— ≈œ«—…</asp:ListItem>
                            <asp:ListItem Value="5">„”ƒÊ· ‰Ÿ«„</asp:ListItem>
                        </asp:DropDownList>--%>
                                </td>
                                <td colspan="1" style="height: 24px">
                                    <dx:ASPxBinaryImage ID="Signature" runat="server" EnableServerResize="True" Width="100%">
                                        <EditingSettings EmptyValueText=" Õ„Ì·  ÊﬁÌ⁄ «·„” Œœ„" Enabled="True">
                                        </EditingSettings>
                                    </dx:ASPxBinaryImage>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="EngTD" dir="rtl">&nbsp;</td>
                                <td colspan="3" class="dx-ar">
                                    <asp:TextBox ID="MaxIssu" Text="0" runat="server" ForeColor="Black" Visible="false">0</asp:TextBox>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"></asp:SqlDataSource>
                                    <asp:TextBox ID="syss" runat="server" ForeColor="White" Visible="False" Width="203px"></asp:TextBox>
                                    <asp:SqlDataSource ID="Branches" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"></asp:SqlDataSource>
                                </td>
                                <td align="right" style="height: 24px" colspan="2">
                                    <%--<asp:Button ID="Button1" runat="server" Text="Õ›Ÿ" Width="120px"
                        BorderStyle="Solid" BorderWidth="1px" TabIndex="8"  />--%>
                                    <asp:SqlDataSource ID="SignDs" runat="server" ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                                        OnUpdating="SignDs_Updating"
                                        SelectCommand="SELECT Signature FROM AccountFile WHERE (AccountNo = @Log)"
                                        UpdateCommand="UPDATE AccountFile SET Signature = @Signature WHERE AccountNo = @Log">
                                        <SelectParameters>
                                            <asp:QueryStringParameter DefaultValue="0" Name="Log" QueryStringField="log" Type="Int32" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:QueryStringParameter DefaultValue="0" Name="Log" QueryStringField="log" Type="Int32" />
                                            <asp:Parameter Name="Signature" Type="Object" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>