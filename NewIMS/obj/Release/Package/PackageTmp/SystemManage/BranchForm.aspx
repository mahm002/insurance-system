<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BranchForm.aspx.vb" Inherits="BranchForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>الفروع</title>
    <script src="../scripts/Scripts.js"></script>
    <script src="../scripts/jquery-latest.min.js"></script>
    <script src="../scripts/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).on('keyup', 'input', function (e) {
            if (e.keyCode == 13 && e.target.type !== 'submit') {
                var inputs = $(e.target).parents("form").eq(0).find('input,a,select,button,textarea').filter(':visible:enabled');
                idx = inputs.index(e.target);
                if (idx == inputs.length - 1) {
                    inputs[0].select()
                } else {
                    inputs[idx + 1].focus();
                    inputs[idx + 1].select();
                }
            }
        });

        function Validate(s, e) {
            if (ASPxClientEdit.ValidateGroup('Data'))
                cbp.PerformCallback('Save');
        }
        document.addEventListener("contextmenu", function (e) {
            e.preventDefault();
        }, false);
    </script>
    <style type="text/css">
        .auto-style1 {
            text-align: left;
            height: 16px;
        }

        .auto-style2 {
            height: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dx-ac">
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents EndCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="100" ShowImage="false" Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dx-al">
                                    <table dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td>اسم الفرع :</td>
                                            <td class="dxeCaptionHACSys">
                                                <dx:ASPxTextBox ID="BranchName" runat="server" Width="350px">
                                                    <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="  "
                                                            ValidationExpression="^[A-Za-z0-9اأإ-يءئ \\-]{5,40}$" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td rowspan="6">
                                                <dx:ASPxBinaryImage ID="Logo" runat="server" EnableServerResize="True">
                                                    <EditingSettings EmptyValueText="تحميل شعار الشركة" Enabled="True">
                                                    </EditingSettings>
                                                </dx:ASPxBinaryImage>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>رمز الفرع :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="BranchNo" runat="server" Width="56px">
                                                    <MaskSettings ErrorText="يرجى إدخال حرفين " Mask=">00\0\0\0" />
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">العنوان :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="Address" runat="server" Width="100%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText=" ( - حروف وأرقام (يحوي علامة " ValidationExpression="^[A-Za-z0-9ا-يءإأئ /-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">رقم الهاتف :</td>
                                            <td class="dx-al">
                                                <dx:ASPxTextBox ID="Telephone" runat="server" Width="100%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">رقم&nbsp; الفاكس :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="FaxNo" runat="server" Width="100%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">البريد الإلكتروني :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="eMail" runat="server" Width="100%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="حروف فقط" ValidationExpression="^[A-Za-z0-9.-_ \\@-.]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">&nbsp;</td>
                                            <td>
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="حفظ" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {
                                                                cbp.PerformCallback('Apply'); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">
                                                <%--<cc1:msgBox ID="MsgBox1" Style="z-index: 103; left: 536px; position: absolute; top: 184px" runat="server"></cc1:msgBox>--%>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="syss" runat="server" ForeColor="White" Visible="False"></asp:TextBox>
                                                <asp:SqlDataSource ID="Branches" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>" SelectCommand="select * from BranchInfo"></asp:SqlDataSource>
                                                <asp:DropDownList ID="BranchesDD" runat="server" DataSourceID="Branches" DataTextField="BranchName" DataValueField="BranchNo" Style="margin-right: 0px" Visible="False" Width="176px">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="LogoDs" runat="server"
                                                    ConnectionString="<%$ ConnectionStrings:IMS-DBConnectionString %>"
                                                    OnUpdating="LogoDs_Updating"
                                                    SelectCommand="SELECT Logo FROM BranchInfo WHERE (BranchNo = @Branch)"
                                                    UpdateCommand="UPDATE BranchInfo SET Logo = @Logo WHERE BranchNo = @Branch">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter DefaultValue="0" Name="Branch" QueryStringField="Br" Type="String" />
                                                    </SelectParameters>
                                                    <UpdateParameters>
                                                        <asp:QueryStringParameter DefaultValue="0" Name="Branch" QueryStringField="Br" Type="String" />
                                                        <asp:Parameter Name="Logo" Type="Object" />
                                                    </UpdateParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>" SelectCommand="select CondNo,Condition From CondFile where SubSys='22'"></asp:SqlDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--                   </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>--%>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>