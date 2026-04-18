<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TravelInsure.aspx.vb" Inherits="TravelInsure" %>

<%@ Register Src="~/Policy/PolicyControl.ascx" TagPrefix="uc1" TagName="PolicyControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 18px;
        }

        .auto-style2 {
            height: 18px;
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
                                    <table dir="rtl" style="width: 100%;">
                                        <tr>
                                            <td>رقم الجواز :</td>
                                            <td>
                                                <dx:ASPxTextBox ID="PassNo" runat="server" ClientInstanceName="PassNo" CssClass="1" Width="110px">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RegularExpression ValidationExpression="^[A-Za-z0-9 ]+" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>تاريخ الميلاد :</td>
                                            <td>
                                                <dx:ASPxDateEdit ID="BirthDate" runat="server" DisplayFormatString="yyyy/MM/dd" EditFormat="Custom" EditFormatString="yyyy/MM/dd" UseMaskBehavior="true">
                                                    <ClientSideEvents ValueChanged="Validate" />
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>الجنس :</td>
                                            <td>
                                                <dx:ASPxComboBox ID="Gender" runat="server" ClientInstanceName="Gender" DataSourceID="Gender1"
                                                    DropDownStyle="DropDownList" ValueType="System.Int32" RightToLeft="True" TextField="TPName" ValueField="TPNo" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data" Display="Dynamic">
                                                        <RequiredField ErrorText="الجنس مطلوب" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>الجنسية :</td>
                                            <td>
                                                <dx:ASPxComboBox ID="Nation" runat="server" ClientInstanceName="Nation" DataSourceID="Nate1"
                                                    DropDownStyle="DropDownList" ValueType="System.Int32"
                                                    RightToLeft="True" TextField="TPName" ValueField="TPNo" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="Data" Display="Dynamic">
                                                        <RequiredField ErrorText="الجنسية مطلوبة" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al">منطقة التغطية :</td>
                                            <td colspan="3">
                                                <dx:ASPxComboBox ID="Area" runat="server" ClientInstanceName="PermType" DataSourceID="Area1"
                                                    DropDownStyle="DropDownList" RightToLeft="True" ValueType="System.Int32"
                                                    TextField="TPName" ValueField="TPNo" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="Validate" />
                                                    <ValidationSettings ValidationGroup="Data" SetFocusOnError="True" Display="Dynamic">
                                                        <RequiredField ErrorText="منطقة التغطية مطلوبة" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:SqlDataSource ID="Nate1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, rtrim(TPName) +' || '+ rtrim(Accessor) as TPName FROM EXTRAINFO where tp='NatE' ORDER BY TPNo"></asp:SqlDataSource>
                                                <asp:SqlDataSource ID="Area1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, rtrim(TPName) +' || '+ rtrim(Accessor) as TPName FROM EXTRAINFO where tp='AreaT' and TpNo<>3 ORDER BY TPNo"></asp:SqlDataSource>
                                                <asp:SqlDataSource ID="Country1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                                    SelectCommand="SELECT TPNo, rtrim(TPName) +' || '+ rtrim(Accessor) as TPName FROM EXTRAINFO where tp='Country' order by TpNo"></asp:SqlDataSource>
                                                <asp:SqlDataSource ID="Gender1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" SelectCommand="SELECT TPNo, rtrim(TPName) +' || '+ rtrim(Accessor) as TPName FROM EXTRAINFO where tp='Gender' ORDER BY TPNo"></asp:SqlDataSource>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="تخزين">
                                                    <ClientSideEvents Click="Validate" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="Premium" runat="server" ClientEnabled="False" ClientInstanceName="Premium" CssClass="2" Text="0" Width="109px">
                                                    <ClientSideEvents TextChanged="Validate" />
                                                    <ValidationSettings SetFocusOnError="True" Display="Dynamic">
                                                        <RegularExpression ErrorText="أرقام فقط" ValidationExpression="[-+]?[0-9]{0,7}\.?[0-9]{1,3}" />
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td></td>
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