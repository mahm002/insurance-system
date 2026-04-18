<%@ Page Language="VB" AutoEventWireup="false" Inherits="Reinsurance_DestributeClaim" CodeBehind="DestributeClaim.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>

    <script type="text/javascript">
        function ReturnToParentPage() {
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup();
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 11%;
        }
        .auto-style2 {
            width: 11%;
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" RightToLeft="True" runat="server" Width="100%" HeaderText="توزيع الوثيقة">
                <PanelCollection>
                    <dx:PanelContent>
                        <table style="width: 100%;">
                            <tr>
                                <td class="dxeICC">&nbsp;رقم الحادث</td>
                                <td>
                                    <dx:ASPxTextBox ID="ClmNo" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="EndNo" runat="server" Width="26px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="LoadNo" runat="server" Width="26px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeICC">&nbsp;المؤمن له</td>
                                <td>
                                    <dx:ASPxTextBox ID="CustName" runat="server" Height="16px" Width="353px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">تاريخ فنح الحادث</td>
                                <td colspan="3" class="auto-style2">
                                    <dx:ASPxTextBox ID="IssuDate" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeICC">&nbsp; مبلغ التأمين</td>
                                <td class="auto-style2">
                                    <dx:ASPxTextBox ID="SumInsured" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style2">صافي القسط</td>
                                <td class="auto-style2">
                                    <dx:ASPxTextBox ID="NETPAID" ClientInstanceName="NETPAID" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">رقم الوثيقة</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox ID="PolNo" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style2">حالة الملف</td>
                                <td>
                                    <dx:ASPxTextBox ID="State" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeICC">القسط الموزع</td>
                                <td>
                                    <dx:ASPxTextBox ID="DistNet" runat="server" ClientInstanceName="DistNet" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxeICC">&nbsp; سعر الصرف</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox ID="Exc" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">&nbsp; </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
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
                                    <dx:ASPxTextBox ID="TreatyNo" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">بيان الإتفاقية</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox ID="CustName0" runat="server" Width="353px" Height="16px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td style="text-align: left">عدد الإعيان</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox ID="EndCnt" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;أولاً بنوذ الإتفاقية</td>
                                <td class="style1">القسط/مبلغ التأمين</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="TSumIns" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="TNetPrm" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="CustNo" runat="server" Width="170px" Visible="false">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="TOTPAID" runat="server" Width="170px" Visible="false">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Cur" runat="server" Width="170px" Visible="false">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Cfrom" runat="server" Width="170px" Visible="false">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Cto" runat="server" Width="170px" Visible="false">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">1.</td>
                                <td class="dxeICC">سعة الإتفاقية</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Capacity" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">التوزيع</td>
                                <td align="center">الحروب</td>
                            </tr>
                            <tr>
                                <td class="style2">2.</td>
                                <td class="dxeICC">إحتفاظ الشركة</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Ret" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TRet" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style2">3.</td>
                                <td class="dxeICC">مشاركة</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="QS" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TQS" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">
                                    <dx:ASPxTextBox ID="wQS" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style6">4.</td>
                                <td class="dxeICC">فائض أول</td>
                                <td class="auto-style7">
                                    <dx:ASPxTextBox ID="FirstSup" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2" class="auto-style6">
                                    <dx:ASPxTextBox ID="TFirstSup" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" class="auto-style6">
                                    <dx:ASPxTextBox ID="wFirstSup" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">5.</td>
                                <td class="dxeICC">فائض ثاني</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="SecondSup" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TSecondSup" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style2">6.</td>
                                <td class="dxeICC">لاين سليب في حالة البحري</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="LineSlip" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TLineSlip" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style2">7.</td>
                                <td class="dxeICC">إختياري</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Fac" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TFac" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="TSpRet" runat="server" Width="109px" ClientVisible="false" Text="0">
                           </dx:ASPxTextBox>
                                </td>
                                <td align="center">
                                    <dx:ASPxTextBox ID="wFac" runat="server" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style4">ثانياً : توزيع القسط</td>
                                <td class="auto-style5">
                                    <dx:ASPxButton ID="ASPxButton5" runat="server" Text="توزيع">
                                    </dx:ASPxButton>
                                </td>
                                <td class="auto-style4"></td>
                                <td class="auto-style4"></td>
                                <td class="auto-style4"></td>
                            </tr>
                            <tr>
                                <td class="style2" colspan="6">
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"
                                        SelectCommand="SELECT PolNo,LoadNo, EndNo,PolD, InsAmt, Net, War, Cur, Exc, Amount, Qs, FirsSup, SecondSup, Elective, LineSlip, Qsw, FirsSupw, Electivew,SpecialRet FROM NetPrm WHERE (PolNo = @PolNo) AND (EndNo = @EndNo) AND (LoadNo = @LoadNo)">
                                        <SelectParameters>
                                            <asp:Parameter DbType="String" Name="PolNo" />
                                            <asp:Parameter DbType="Int32" Name="EndNo" />
                                            <asp:Parameter DbType="Int32" Name="LoadNo" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False"
                                        DataSourceID="SqlDataSource1" Width="100%" RightToLeft="True">
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False"></FilterControl>
                                        </SettingsPopup>
                                        <columns>
                                            <dx:gridviewdatatextcolumn caption="رقم الوثيقة / الحادث" fieldname="PolNo"
                                                showincustomizationform="True" visibleindex="0">
                                                <propertiestextedit displayformatstring="N3"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="رقم الملحق" fieldname="EndNo"
                                                showincustomizationform="True" visibleindex="1">
                                                <propertiestextedit displayformatstring="N3"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="رقم الإشعار" fieldname="LoadNo"
                                                showincustomizationform="True" visibleindex="2">
                                                <propertiestextedit displayformatstring="N3"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="صافي التعويض/ الإحتياطي" fieldname="Net"
                                                showincustomizationform="True" visibleindex="4">
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="مبلغ التأمين" fieldname="InsAmt"
                                                showincustomizationform="True" visibleindex="5">
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="إحتفاظ" fieldname="Amount"
                                                showincustomizationform="True" visibleindex="6">
                                                <propertiestextedit displayformatstring="N3"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="مشاركة" fieldname="Qs"
                                                showincustomizationform="True" visibleindex="7">
                                                <propertiestextedit displayformatstring="N3"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="فائض أول" fieldname="FirsSup"
                                                showincustomizationform="True" visibleindex="8">
                                                <propertiestextedit displayformatstring="N3"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="تاريخ فتح الحادث" fieldname="PolD" showincustomizationform="True" visibleindex="3" unboundtype="DateTime">
                                                <propertiestextedit displayformatstring="yyyy/MM/dd"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="فائض ثاني" fieldname="SecondSup"
                                                showincustomizationform="True" visibleindex="9">
                                                <propertiestextedit displayformatstring="N3"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="لاين سليب" fieldname="LineSlip"
                                                showincustomizationform="True" visibleindex="10">
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn fieldname="Elective" showincustomizationform="True" caption="إختياري" visibleindex="11">
                                                <propertiestextedit displayformatstring="N3"></propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="إحتفاظ خاص" fieldname="SpecialRet"
                                                showincustomizationform="True" visibleindex="12">
                                                <propertiestextedit displayformatstring="N3">
                                                </propertiestextedit>
                                            </dx:gridviewdatatextcolumn>
                                        </columns>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td class="style1">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <dx:ASPxButton ID="ASPxButton3" runat="server" Text="حفظ">
                                        <ClientSideEvents Click="function(s, e) { ReturnToParentPage();}"></ClientSideEvents>
                                        <ClientSideEvents Click="function(s, e) { ReturnToParentPage();}" />
                                    </dx:ASPxButton>
                                </td>
                                <td colspan="3">
                                    <dx:ASPxButton ID="ASPxButton4" runat="server" Text="الغاء">
                                        <ClientSideEvents Click="function(s, e) { ReturnToParentPage();}"></ClientSideEvents>
                                        <ClientSideEvents Click="function(s, e) { ReturnToParentPage();}" />
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