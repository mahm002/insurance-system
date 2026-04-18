<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MedicalRespPharm.aspx.vb" Inherits="MedicalRespPharm" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="10" ShowImage="false" Enabled="false" />
                <PanelCollection>
                    <dx:PanelContent runat="server">

                        <table id="TableForm" border="0" dir="rtl" style="width: 100%;">
                            <tr>
                                <td class="dx-al">
                                    <uc1:PolicyControl runat="server" ID="PolicyControl" />
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">
                                    <%--                              <dx:ASPxRoundPanel runat="server" AllowCollapsingByHeaderClick="True" ID="PolicyData" RightToLeft="True" Width="100%" HorizontalAlign="Center">
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">--%>
                                    <table border="0" dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td class="auto-style2">مكان الميلاد :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="PlaceOfBirth" runat="server" ClientInstanceName="PlaceOfBirth" CssClass="1" Width="170px">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">تاريخ الميلاد :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxDateEdit ID="DateOfBirth" runat="server" DisplayFormatString="yyyy/MM/dd" EditFormat="Custom" EditFormatString="yyyy/MM/dd" UseMaskBehavior="True">
                                                    <ClientSideEvents ValueChanged="ValidateS" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">الجنسية :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxComboBox ID="Nationality" runat="server" ClientInstanceName="Nationality" DataSourceID="Nate1" DropDownStyle="DropDown" RightToLeft="True" TextField="Accessor" ValueField="TPNo" Width="170px">
                                                    <ClientSideEvents SelectedIndexChanged="ValidateS" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField ErrorText="الجنسية مطلوبة" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td class="auto-style2">التخصص :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="Education" runat="server" ClientInstanceName="Education" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="حروف فقط" ValidationExpression="[a-zA-Zا-يءئ ]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">رقم الهوية :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="IDNo" runat="server" ClientInstanceName="IDNo" CssClass="1" Width="170px">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="true" Display="Dynamic">
                                                        <RegularExpression ErrorText="حروف فقط" ValidationExpression="^[A-Za-z0-9ا-يءئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">المهنة :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="Specialist" runat="server" ClientInstanceName="Specialist" CssClass="1" Width="170px">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="حروف فقط" ValidationExpression="[a-zA-Zا-يءئ ]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">مكان العمل :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="PlaceOfWork" runat="server" ClientInstanceName="PlaceOfWork" CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="حروف فقط" ValidationExpression="^[A-Za-z0-9ا-يءإؤةأئ /-\\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">رقم العضوية :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="MemberShipNo" runat="server" ClientInstanceName="MemberShipNo" CssClass="3" Width="170px">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:SqlDataSource ID="Nate1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT TPNo,Accessor,TPName FROM EXTRAINFO where tp='NatE' Order By TPNo"></asp:SqlDataSource>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="ValidateS" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientEnabled="False" ClientInstanceName="Premium" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="ValidateS" />
                                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,3}" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
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