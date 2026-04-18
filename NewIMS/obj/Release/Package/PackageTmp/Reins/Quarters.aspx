<%@ Page Language="VB" Inherits="Reinsurance_Quarters" ClientIDMode="Static" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Quarters.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        //function ReturnToParentPage() {
         //var parentWindow = window.parent;
         // parentWindow.SelectAndClosePopup();
        //}
    </script>
</head>
<body runat="server">
    <form id="form1" runat="server">
        <div id="MainDiv" runat="server">
            <br />
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" RightToLeft="False" runat="server" Width="100%" HeaderText="Quarters">
                <PanelCollection>
                    <dx:PanelContent>
                        <table style="vertical-align: central; align-content: center;" class="auto-style67">
                            <tr>
                                <td class="auto-style7">Quarter</td>
                                <td>
                                    <dx:ASPxComboBox ID="Quarters" runat="server" EnableIncrementalFiltering="True" IncrementalFilteringMode="StartsWith" MaxLength="3" Width="140px">
                                        <Items>
                                            <dx:ListEditItem Text="Q1" Value="1" />
                                            <dx:ListEditItem Text="Q2" Value="2" />
                                            <dx:ListEditItem Text="Q3" Value="3" />
                                            <dx:ListEditItem Text="Q4" Value="4" />
                                        </Items>
                                        <ClientSideEvents SelectedIndexChanged="function(s, e)
                                          { Year.SetValue('');
                                            InsCompany.SetValue('');
                                          }" />
                                    </dx:ASPxComboBox>
                                </td>
                                <td class="auto-style5">Year</td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="Year" runat="server" Width="170px" AutoPostBack="True" ClientInstanceName="Year">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">&#160;</td>
                            </tr>
                            <tr>
                                <td rowspan="3" class="auto-style7">Ins. Type</td>
                                <td rowspan="3">
                                    <dx:ASPxComboBox ID="InsType" runat="server" EnableIncrementalFiltering="True" IncrementalFilteringMode="StartsWith" Width="140px" SelectedIndex="0">
                                        <Items>
                                            <dx:ListEditItem Text="Fire" Value="FR,FF" Selected="true" />
                                            <dx:ListEditItem Text="General Accident" Value="FG,BB,FB,CT,CS,PI,EL,CL,PA" />
                                            <dx:ListEditItem Text="Engineering" Value="CR,ER,CM" />
                                            <dx:ListEditItem Text="HULL" Value="HL" />
                                            <dx:ListEditItem Text="Marine" Value="MC,MB,MA,OC" />
                                            <dx:ListEditItem Text="War" Value="MC,MB,MA,OC" />
                                            <dx:ListEditItem Text="2NDSurplus" Value="CR,ER" />
                                            <dx:ListEditItem Text="LineSlip" Value="OC" />
                                        </Items>
                                        <ClientSideEvents SelectedIndexChanged="function(s, e)
                                          { Year.SetValue('');
                                            InsCompany.SetValue('');
                                          }" />
                                    </dx:ASPxComboBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                                <td class="auto-style1" rowspan="3">

                                    <dx:ASPxComboBox ID="Reinsurer" runat="server" ClientInstanceName="InsCompany" IncrementalFilteringMode="StartsWith" OnSelectedIndexChanged="ASPxGridView1_DataBinding" TextField="TpName" ValueField="TpNo" Width="450px">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e)
                                          {//alert(s.GetSelectedItem(4).value);
                                            //alert(s.GetSelectedItem().texts[0]);
                                            //Share.SetValue(s.GetSelectedItem().texts[2]);
                                            ReInsuranceCo.SetValue(s.GetSelectedItem().texts[0]);
                                            //SharefSurp.SetValue(((Share.GetValue().replace(',','')/100) * TotFsurp.GetValue().replace(',','')).toFixed(3));
                                            //ShareTAmount.SetValue(ShareQs.GetValue()/1 + SharefSurp.GetValue()/1);
                                          }" />
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="TpNo" Width="20px">
                                            </dx:ListBoxColumn>
                                            <dx:ListBoxColumn FieldName="TPName" Width="100%">
                                            </dx:ListBoxColumn>
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </td>
                                <td class="auto-style1" rowspan="3">
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"></asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <dx:ASPxTextBox ID="ReInsuranceCo" runat="server" ClientInstanceName="ReInsuranceCo" ClientVisible="false" Height="0px" Width="16px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: left" class="auto-style6">&nbsp;</td>
                                <td>
                                    <dx:ASPxTextBox ID="TreatyN" runat="server" Height="16px" ClientVisible="false" Width="52px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                                <td class="auto-style1">
                                    <dx:ASPxButton ID="ASPxButton6" runat="server" Text="Collect Data for all Reinsurers" Width="227px">
                                    </dx:ASPxButton>
                                </td>
                                <td class="auto-style1">&nbsp;</td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" RightToLeft="False" runat="server"
                Width="100%" HeaderText="Statement Of Account">
                <PanelCollection>
                    <dx:PanelContent>
                        <br />
                        <table style="width: 100%; vertical-align: central;" dir="ltr">

                            <tr>
                                <td class="auto-style2">
                                    <dx:ASPxTextBox ID="reinscom" runat="server" Visible="true" Width="100%" RightToLeft="False">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style61" dir="ltr">
                                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" ClientInstanceName="grid" RightToLeft="false" Width="100%" AutoGenerateColumns="true">
                                        <SettingsPager PageSize="60">
                                        </SettingsPager>
                                        <Settings ShowFooter="True" />

                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False"></FilterControl>
                                        </SettingsPopup>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem FieldName="AMOUNT" SummaryType="Sum" ValueDisplayFormat="{0:#,###.###}" />
                                        </TotalSummary>
                                        <SettingsBehavior ColumnResizeMode="Control" />
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                        <table dir="ltr" style="width: 19%; vertical-align: central;" id="CCTable" runat="server" visible="false">
                            <tr>
                                <td align="right" class="auto-style3"><strong>Reinsurer&#39;s Share</strong></td>
                                <td align="left" class="auto-style7">
                                    <dx:ASPxTextBox ID="Share" runat="server" ClientInstanceName="Share" Width="109px" Font-Bold="True">
                                        <MaskSettings Mask="&lt;0..100&gt;.&lt;0..99&gt;'\ %'" />
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr runat="server">
                                <td runat="server" align="right" class="auto-style3">Q/S</td>
                                <td runat="server" align="left" class="auto-style7">
                                    <dx:ASPxTextBox ID="ShareQs" runat="server" ClientInstanceName="ShareQs" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="auto-style3">1ST Surplus</td>
                                <td align="left" class="auto-style7">
                                    <dx:ASPxTextBox ID="SharefSurp" runat="server" ClientInstanceName="SharefSurp" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="auto-style2"><strong>Total Amount</strong></td>
                                <td align="left" class="auto-style8">
                                    <dx:ASPxTextBox ID="ShareTAmount" runat="server" ClientInstanceName="ShareTAmount" Font-Bold="True" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">
                                    <dx:ASPxButton ID="ASPxButton4" runat="server" Height="16px" Text="الغاء">
                                        <%--<clientsideevents click="
                        ExcelReport();
" />--%>
                                    </dx:ASPxButton>
                                </td>
                                <td class="auto-style8">
                                    <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Save" OnClick="SaveCC">
                                        <%-- <clientsideevents click="function(s, e) {
                        ReturnToParentPage();
}" />--%>
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                        <table id="UWTable" visible="false" runat="server" dir="ltr" style="width: 30%; vertical-align: central; height: 37px;">
                            <tr>
                                <td align="right" class="auto-style10"><strong>Total Amount</strong></td>
                                <td align="left" class="auto-style11">
                                    <dx:ASPxTextBox ID="ShareTAmount0" runat="server" ClientInstanceName="ShareTAmount" Font-Bold="True" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td runat="server" align="left" class="auto-style11">
                                    <asp:Label ID="Label1" runat="server" Text="" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style10">
                                    <dx:ASPxButton ID="ASPxButton7" runat="server" Height="16px" Text="PDF Export" OnClick="SaveUW">
                                        <%--  <clientsideevents click="function(s, e) {
                        ReturnToParentPage();
}" />--%>
                                    </dx:ASPxButton>
                                </td>
                                <td class="auto-style11">
                                    <dx:ASPxButton ID="ASPxButton8" runat="server" Text="Export" OnClick="SaveCC">
                                        <%-- <clientsideevents click="function(s, e) {
                        ReturnToParentPage();
}" />--%>
                                    </dx:ASPxButton>
                                </td>
                                <td runat="server" class="auto-style11"></td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
        </div>
    </form>
</body>
</html>