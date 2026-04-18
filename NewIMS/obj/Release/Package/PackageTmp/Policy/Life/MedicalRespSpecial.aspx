<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MedicalRespSpecial.aspx.vb" Inherits="MedicalRespSpecial" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            text-align: left;
        }

        .auto-style3 {
            height: 42px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents BeginCallback="" />
                <SettingsLoadingPanel Text="احتساب&amp;hellip;" Delay="70" />
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
                                            <td class="auto-style2">مكان&nbsp; العمل :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="PlaceOfWork" runat="server" ClientInstanceName="PlaceOfWork"
                                                    CssClass="1" Width="100%">
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءأإى ؤئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">تاريخ الميلاد :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxDateEdit ID="DateOfBirth" runat="server" DisplayFormatString="yyyy/MM/dd" EditFormat="Custom" EditFormatString="yyyy/MM/dd" UseMaskBehavior="True">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">رقم العضوية بالنقابة :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="MemberShipNo" runat="server" ClientInstanceName="MemberShipNo"
                                                    CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءأإى ؤئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">رقم إذن المزاولة :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="WorkPermissionNo" runat="server" ClientInstanceName="WorkPermissionNo"
                                                    CssClass="1" Width="100%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءأإى ؤئ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">الجنسية :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxComboBox ID="Nationality" runat="server" ClientInstanceName="Nationality" 
                                                    DataSourceID="Nate1" DropDownStyle="DropDownList" ValueType="System.Int32"
                                                    RightToLeft="True" TextField="Accessor" ValueField="TPNo" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField ErrorText="الجنسية مطلوبة" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td class="auto-style2">التخصص/ المهنة :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxComboBox ID="Specialize" runat="server" ClientInstanceName="Specialize" 
                                                    DataSourceID="Special" OnSelectedIndexChanged="Specialize_SelectedIndexChanged"
                                                    DropDownStyle="DropDownList" RightToLeft="True" ValueType="System.Int32"
                                                    TextField="TpName" ValueField="TPNo" Width="100%" SelectedIndex="0">
                                                    <ClientSideEvents SelectedIndexChanged="Validate" />
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RequiredField ErrorText="التخصص مطلوب" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">التخصص :</td>
                                            <td class="auto-style2">
                                                <dx:ASPxTextBox ID="CustomSP" runat="server" ClientInstanceName="CustomSP" Text="/" CssClass="1" Width="100%" ClientEnabled="false">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Data">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9ا-يءأإى ؤئ/ \\-]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="auto-style2">حدود المسئولية :</td>
                                            <td class="auto-style19">
                                                <dx:ASPxTextBox ID="RespLimit" runat="server" ClientInstanceName="RespLinit" CssClass="1" Text="/" Width="100%">
                                                    <validationsettings display="Dynamic" setfocusonerror="True" validationgroup="Data">
                                                        <regularexpression errortext="*" validationexpression="^[A-Za-z0-9ا/%,-يءئ \\-]+" />
                                                        <requiredfield isrequired="True" />
                                                    </validationsettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <asp:SqlDataSource ID="Nate1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo,Accessor,TPName FROM EXTRAINFO where tp='NatE' Order By TPNo"></asp:SqlDataSource>
                                            </td>
                                            <td class="auto-style3">
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td class="auto-style3"></td>
                                            <td class="auto-style3">
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientInstanceName="Premium" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <validationsettings display="Dynamic" setfocusonerror="True">
                                                        <regularexpression errortext="أرقام فقط" validationexpression="[-+]?[0-9]{0,7}\.?[0-9]{1,3}" />
                                                        <requiredfield isrequired="True" />
                                                    </validationsettings>
                                                </dx:ASPxTextBox>
                                                <asp:SqlDataSource ID="Special" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo,TPName FROM EXTRAINFO where tp='Specialize' Order By TPNo"></asp:SqlDataSource>
                                            </td>
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