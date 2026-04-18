<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DistBulkPol.aspx.vb" Inherits="DistBulkPol" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
        function ReturnToParentPage() {
            //debugger;
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup();
            //var t = ExtPrm.GetValue();
            //window.parent.document.getElementById("EXTPRM").value = t;
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 18px;
        }

        .auto-style2 {
            height: 23px;
        }

        .auto-style3 {
            width: 1053px;
        }

        .auto-style4 {
            text-align: center;
        }

        .auto-style5 {
            height: 18px;
            text-align: center;
        }

        .auto-style6 {
            text-align: left;
        }

        .auto-style7 {
            height: 18px;
            text-align: left;
        }

        .auto-style8 {
            height: 23px;
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" RightToLeft="True" runat="server" Width="100%" HeaderText="توزيع الوثيقة">
                <PanelCollection>
                    <dx:PanelContent>
                        <table style="width: 100%;">
                            <tr>
                                <td class="auto-style1">&nbsp;رقم الوثيقة</td>
                                <td>
                                    <dx:ASPxTextBox ID="PolNo" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="EndNo" runat="server" Height="17px" Width="26px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="LoadNo" runat="server" Height="17px" Width="26px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style7">&nbsp;المؤمن له</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox ID="CustName" runat="server" Height="16px" Width="391px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style4" colspan="3">
                                    <dx:ASPxLabel ID="AlertL" runat="server" BackColor="Red" Style="font-weight: 700" Text="The SumInsured Exceeded the capacity!!!" Visible="False" Width="100%">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">&nbsp;</td>
                                <td>&nbsp; تاريخ الإصدار</td>
                                <td colspan="2">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td class="auto-style1">&nbsp;</td>
                                <td>&nbsp; مبلغ التأمين</td>
                                <td style="text-align: right">صافي القسط</td>
                                <td style="text-align: center" rowspan="2" colspan="2">&nbsp;</td>
                                <td class="auto-style6">قيمة العمولة</td>
                                <td>
                                    <dx:ASPxTextBox ID="Comission" runat="server" ClientInstanceName="DistNet" Width="80px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">&nbsp;</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="IssuDate" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">&nbsp;</td>
                                <td class="auto-style1">&nbsp;</td>
                                <td>
                                    <dx:ASPxTextBox ID="SumInsured" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="NetPRM" runat="server" ClientInstanceName="NETPRM" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="Sys" runat="server" ClientInstanceName="Sys" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="OrderNo" runat="server" ClientInstanceName="OrderNo" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Branch" runat="server" ClientInstanceName="Branch" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td colspan="4" class="auto-style1"></td>
                                <td class="auto-style1" colspan="2">
                                    <dx:ASPxTextBox ID="HNetPRM" runat="server" ClientInstanceName="HNETPRM" Height="16px" Width="94px" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">&nbsp; القسط</td>
                                <td class="auto-style1" colspan="2">قسط الحروب</td>
                                <td class="auto-style1" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="auto-style6"></td>
                                <td colspan="4" class="auto-style7">
                                    <dx:ASPxTextBox ID="HFM" runat="server" ClientInstanceName="HFM" ClientVisible="False" Height="16px" Width="94px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style6" colspan="2">
                                    <dx:ASPxTextBox ID="HDistNet" runat="server" ClientInstanceName="HDistNet" Width="170px" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style6">
                                    <dx:ASPxTextBox ID="DistNet" runat="server" ClientInstanceName="DistNet" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style7" colspan="2">
                                    <dx:ASPxTextBox ID="War" runat="server" ClientInstanceName="War" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style7" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td colspan="4">&nbsp; سعر الصرف</td>
                                <td class="auto-style1" colspan="2">
                                    <dx:ASPxTextBox ID="HWar" runat="server" ClientInstanceName="HWar" Width="170px" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">الاختياري (قسط)</td>
                                <td style="margin-right: 40px; text-align: right;" colspan="2">الاختياري (حروب)</td>
                                <td class="auto-style3" style="margin-right: 40px" colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style1">&nbsp;</td>
                                <td colspan="4">
                                    <dx:ASPxTextBox ID="Exc" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1" colspan="2">&nbsp;
                        <dx:ASPxTextBox ID="HF" runat="server" ClientInstanceName="HF" Height="16px" Width="94px" ClientVisible="False">
                        </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="F" runat="server" ClientInstanceName="F" Height="16px" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="margin-right: 40px" colspan="2">
                                    <dx:ASPxTextBox ID="DistWar" runat="server" ClientInstanceName="DistWar" Width="170px" Text="0">
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="margin-right: 40px" class="auto-style4">&nbsp;احتفاظ خاص (قسط)</td>
                                <td style="margin-right: 40px">
                                    <dx:ASPxTextBox ID="SpRet" runat="server" ClientInstanceName="SpRet" Height="17px" Text="0" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td colspan="4" class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td style="text-align: left" class="auto-style1">نسبة الاختياري %</td>
                                <td style="text-align: right" class="auto-style1">من القسط الأساسي</td>
                                <td style="margin-right: 40px" class="auto-style1" colspan="2">من قسط الحروب</td>
                                <td style="margin-right: 40px" class="auto-style5">احتفاظ خاص (حروب)</td>
                                <td class="auto-style1" style="margin-right: 40px">
                                    <dx:ASPxTextBox ID="WSpRet" runat="server" ClientInstanceName="WSpRet" Height="17px" Text="0" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">&nbsp;</td>
                                <td class="auto-style1" colspan="4">&nbsp;</td>
                                <td class="auto-style1">&nbsp;</td>
                                <td class="auto-style1" style="text-align: left">&nbsp;</td>
                                <td style="text-align: right">
                                    <dx:ASPxTrackBar ID="FacRatio" runat="server" Height="16px" LargeTickInterval="10" Position="0" PositionStart="0" Step="0.0001" Width="100%">
                                        <ClientSideEvents PositionChanged="function(s, e) {
                                                var n=((HNETPRM.GetValue().replace(',',''))- War.GetValue().replace(',',''));
                                                    F.SetValue((parseFloat(n)*s.GetPosition()/100).toFixed(3));
                                                    HFM.SetValue((parseFloat(n)*s.GetPosition()/100).toFixed(3));
                                                    DistNet.SetValue(((HNETPRM.GetValue().replace(',',''))- War.GetValue().replace(',','')- F.GetValue().replace(',','')).toFixed(3));
                                                    SpRet.SetValue(0);
                                                    SpRetRatio.SetPosition(0);
                                                    }" />
                                    </dx:ASPxTrackBar>
                                </td>
                                <td colspan="2" style="margin-right: 40px">
                                    <dx:ASPxTrackBar ID="WarFacRatio" runat="server" Height="16px" LargeTickInterval="10" Position="0" PositionStart="0" Step="0.0001" Style="text-align: right" Width="100%">
                                        <ClientSideEvents PositionChanged="function(s, e) {
                                    var n1=War.GetValue().replace(',','');
                                    DistWar.SetValue((parseFloat(n1)*s.GetPosition()/100).toFixed(3));
                                        }" />
                                    </dx:ASPxTrackBar>
                                </td>
                                <td style="margin-right: 40px">&nbsp;</td>
                                <td style="margin-right: 40px">
                                    <dx:ASPxTrackBar ID="SpRetRatio" runat="server" ClientInstanceName="SpRetRatio" Height="16px" Position="0" PositionStart="0" Step="0.0001" Style="text-align: right" Width="100%">
                                        <ClientSideEvents PositionChanged="function(s, e) {  //debugger;
                if (parseFloat(HF.GetValue()) == parseFloat(HFM.GetValue())) {
                       var sr=HF.GetValue().replace(',','');
                       SpRet.SetValue((parseFloat(sr)*s.GetPosition()/100).toFixed(3));
                       F.SetValue((HF.GetValue().replace(',','')- SpRet.GetValue().replace(',','')).toFixed(3));
                                        }
                        else  {
                        var sr=HFM.GetValue().replace(',','');
                        SpRet.SetValue((parseFloat(sr)*s.GetPosition()/100).toFixed(3));
                        F.SetValue(((HFM.GetValue().replace(',',''))- SpRet.GetValue().replace(',','')).toFixed(3));
                                }

                                                                }" />
                                    </dx:ASPxTrackBar>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>

            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" RightToLeft="True" runat="server"
                Width="100%" HeaderText="إتفاقية إعادة التأمين">
                <PanelCollection>
                    <dx:PanelContent>
                        <table style="width: 100%;">
                            <tr>
                                <td class="style2">&nbsp;الرقم </td>
                                <td>
                                    <dx:ASPxTextBox ID="TreatyNo" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style6" colspan="2">&nbsp;</td>
                                <td class="auto-style6">بيان الإتفاقية</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox ID="CustName0" runat="server" Height="16px" Width="353px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td colspan="2" class="auto-style6">&nbsp;</td>
                                <td class="auto-style6">عدد الإعيان</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox ID="EndCnt" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;أولاً بنوذ الإتفاقية</td>
                                <td class="style1" colspan="2">القسط/مبلغ التأمين</td>
                                <td class="style1">&nbsp;</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="TSumIns" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="TNetPrm" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="CustNo" runat="server" Visible="False" Width="120px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="TOTPRM" runat="server" Visible="False" Width="120px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Cur" runat="server" Visible="False" Width="120px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Cfrom" runat="server" Visible="False" Width="120px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Cto" runat="server" Visible="False" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td class="style1">&nbsp;</td>
                                <td class="auto-style4" colspan="2">العمولات</td>
                                <td align="center" colspan="2">&nbsp;</td>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style2">1.</td>
                                <td class="dxeICC">سعة الإتفاقية</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Capacity" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">للأقساط %</td>
                                <td class="style1">للحروب %</td>
                                <td align="center" colspan="2">التوزيع</td>
                                <td align="center">الحروب</td>
                            </tr>
                            <tr>
                                <td class="auto-style2">2.</td>
                                <td class="auto-style8">إحتفاظ الشركة</td>
                                <td class="auto-style2">
                                    <dx:ASPxTextBox ID="Ret" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style2">&nbsp;</td>
                                <td class="auto-style2">&nbsp;</td>
                                <td align="center" class="auto-style2" colspan="2">
                                    <dx:ASPxTextBox ID="TRet" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" class="auto-style2"></td>
                            </tr>
                            <tr>
                                <td class="style2">3.</td>
                                <td class="dxeICC">مشاركة</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="QS" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="TRQSRCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="TRWQSRCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TQS" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">
                                    <dx:ASPxTextBox ID="wQS" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">4.</td>
                                <td class="dxeICC">فائض أول</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="FirstSup" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="TR1STCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="TRW1STCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TFirstSup" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">
                                    <dx:ASPxTextBox ID="wFirstSup" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">5.</td>
                                <td class="dxeICC">فائض ثاني</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="SecondSup" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="TR2STCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">&nbsp;</td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TSecondSup" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2">6.</td>
                                <td class="auto-style8">لاين سليب (في حالة البحري)</td>
                                <td class="auto-style2">
                                    <dx:ASPxTextBox ID="LineSlip" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style2">
                                    <dx:ASPxTextBox ID="TRLSCOMM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style2">&nbsp;</td>
                                <td align="center" class="auto-style2" colspan="2">
                                    <dx:ASPxTextBox ID="TLineSlip" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" class="auto-style2"></td>
                            </tr>
                            <tr>
                                <td class="style2">7.</td>
                                <td class="dxeICC">إختياري</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Fac" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="FacComm" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">&nbsp;</td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TFac" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">
                                    <dx:ASPxTextBox ID="wFac" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>8.</td>
                                <td>احتفاظ خاص</td>
                                <td class="style1" colspan="2"></td>
                                <td class="style1">&nbsp;</td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TSpRet" runat="server" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1" colspan="2"></td>
                                <td class="auto-style1">&nbsp;</td>
                                <td class="auto-style1" align="center" colspan="2"></td>
                                <td class="auto-style1"></td>
                            </tr>
                            <tr>
                                <td colspan="2">ثانياً : توزيع القسط</td>
                                <td class="style1" colspan="2">
                                    <dx:ASPxButton ID="ASPxButton5" runat="server" Text="توزيع">
                                    </dx:ASPxButton>
                                </td>
                                <td class="style1">&nbsp;</td>
                                <td>
                                    <dx:ASPxLabel ID="AlertL0" runat="server" BackColor="White" Style="font-weight: 700" Text="قيمة الرسوم المضافة" Width="160px">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="ExtPrm" runat="server" ClientInstanceName="ExtPrm" Height="17px" Text="0" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2" colspan="8">
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"
                                        SelectCommand="SELECT PolNo,LoadNo, EndNo,PolD, InsAmt, Net, War, Cur, Exc, Amount, Qs, FirsSup, SecondSup, Elective, LineSlip, Qsw, FirsSupw, Electivew, SpecialRet FROM NetPrm WHERE (PolNo = @PolNo) AND (EndNo = @EndNo) AND (LoadNo = @LoadNo)">
                                        <SelectParameters>
                                            <asp:Parameter DbType="String" Name="PolNo" />
                                            <asp:Parameter DbType="Int32" Name="EndNo" />
                                            <asp:Parameter DbType="Int32" Name="LoadNo" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                        DataSourceID="SqlDataSource1">
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False"></FilterControl>
                                        </SettingsPopup>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="رقم الوثيقة" FieldName="PolNo"
                                                ShowInCustomizationForm="True" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ملحق" FieldName="EndNo"
                                                ShowInCustomizationForm="True" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="إشعار" FieldName="LoadNo"
                                                ShowInCustomizationForm="True" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="صافي القسط" FieldName="Net" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="4">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="مبلغ التأمين" FieldName="InsAmt" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="5">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="إحتفاظ" FieldName="Amount" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="6">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="مشاركة" FieldName="Qs" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="7">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="فائض أول" FieldName="FirsSup" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="8">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="تاريخ الإصدار" FieldName="PolD" ShowInCustomizationForm="True" VisibleIndex="3">
                                                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="فائض ثاني" FieldName="SecondSup" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="9">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="لاين سليب" FieldName="LineSlip" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="9">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="إختياري" FieldName="Elective" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="10">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="قسط الحروب" FieldName="War" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="11">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="مشاركة حروب" FieldName="Qsw" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="12">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="فائض أول حروب" FieldName="FirsSupw" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="13">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="إختياري حروب" FieldName="Electivew" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="14">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="احتفاظ خاص" FieldName="SpecialRet" PropertiesTextEdit-DisplayFormatString="N3"
                                                ShowInCustomizationForm="True" VisibleIndex="15">
                                                <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td class="style1" colspan="2">&nbsp;</td>
                                <td class="style1">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4">
                                    <dx:ASPxButton ID="ASPxButton3" runat="server" Text="حفظ">
                                        <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();

}" />
                                        <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();
}"></ClientSideEvents>
                                    </dx:ASPxButton>
                                </td>
                                <td align="left">&nbsp;</td>
                                <td colspan="3">
                                    <dx:ASPxButton ID="ASPxButton4" runat="server" Text="الغاء">
                                        <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();
}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
        </div>
    </form>
</body>
</html>