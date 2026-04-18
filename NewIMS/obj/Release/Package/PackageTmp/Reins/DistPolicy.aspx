<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DistPolicy.aspx.vb" Inherits="DistPolicy" %>

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

        .auto-style9 {
            height: 18px;
            width: 148px;
        }

        .auto-style10 {
            height: 18px;
            text-align: left;
            width: 148px;
        }

        .auto-style11 {
            text-align: left;
            width: 148px;
        }
        .auto-style12 {
            text-align: right;
            height: 18px;
        }
        .auto-style13 {
            height: 30px;
        }
        .auto-style14 {
            text-align: left;
            height: 30px;
        }
        .auto-style15 {
            height: 23px;
            text-align: right;
        }
        .auto-style18 {
            font-size: medium;
            text-align: center;
        }
        .auto-style19 {
            color: #009933;
        }
        .auto-style20 {
            font-size: small;
        }
        .auto-style21 {
            color: #FFFF00;
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
                                <td class="auto-style5" colspan="11">
                                    <dx:ASPxLabel ID="AlertL" runat="server" BackColor="Red" Style="font-weight: 700;" RightToLeft="False" 
                                        Text="THE SUMINSURED EXCEEDED THE CAPACITY OF THE TREATY!!!" Visible="False" Width="100%" CssClass="auto-style21" Height="27px" >
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style10">&nbsp;رقم الوثيقة</td>
                                <td>
                                    <dx:ASPxTextBox ID="PolNo" runat="server" Width="170px" ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="EndNo" runat="server"  Width="26px" ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="LoadNo" runat="server"  Width="26px" ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style7">&nbsp;المؤمن له</td>
                                <td colspan="5">
                                    <dx:ASPxTextBox ID="CustName" runat="server"  Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style9">&nbsp;</td>
                                <td>
                                    &nbsp; تاريخ الإصدار</td>
                                <td colspan="2">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td class="auto-style1">
                                    &nbsp;</td>
                                <td>
                                    &nbsp; مبلغ التأمين</td>
                                <td style="text-align: right">صافي القسط</td>
                                <td rowspan="2" style="text-align: center">&nbsp;</td>
                                <td class="dx-ac">عمولة إصدار </td>
                                <td>
                                    <dx:ASPxButton ID="GoBack" runat="server" ClientVisible="False" Text="عودة لصفحة الإدخال" Width="100%">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style9">&nbsp;</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="IssuDate" runat="server" Width="170px" ReadOnly="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    &nbsp;</td>
                                <td class="auto-style1">&nbsp;</td>
                                <td>
                                    <dx:ASPxTextBox ID="SumInsured" runat="server" Width="170px" ForeColor="#ff0000" Font-Bold="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="NetPRM" runat="server" ClientInstanceName="NETPRM" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <div class="dx-al">
                                        <dx:ASPxTextBox ID="Comission" runat="server" ClientInstanceName="DistNet" Width="80px">
                                        </dx:ASPxTextBox>
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style10">من</td>
                                <td colspan="4" class="auto-style1">
                                    <dx:ASPxTextBox ID="CoverFrom" runat="server" Width="170px" ReadOnly="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1" colspan="2">
                                    <dx:ASPxTextBox ID="HNetPRM" runat="server" ClientInstanceName="HNETPRM" Width="94px" ClientVisible="False" Height="16px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">
                                    &nbsp; القسط الموزع اتفاقي</td>
                                <td class="auto-style1">
                                    قسط الحروب</td>
                                <td class="auto-style1" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style11">إلى</td>
                                <td colspan="4" class="auto-style7">
                                    <dx:ASPxTextBox ID="CoverTo" runat="server" ReadOnly="True" Width="170px">
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
                                <td class="auto-style7">
                                    <dx:ASPxTextBox ID="War" runat="server" ClientInstanceName="War" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style7" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="auto-style9"></td>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                                <td class="auto-style1" colspan="2">
                        <dx:ASPxTextBox ID="HWar" runat="server" ClientInstanceName="HWar" Width="170px" ClientVisible="False">
                        </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">
                                    االقسط الموزع اختياري FAC</td>
                                <td style="margin-right: 40px; text-align: right;">
                                    الاختياري (حروب)</td>
                                <td style="margin-right: 40px" class="auto-style3" colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style10">سعر الصرف</td>
                                <td colspan="4">
                                    <dx:ASPxTextBox ID="Exc" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1" colspan="2">&nbsp;
                                    <dx:ASPxTextBox ID="HF" runat="server" ClientInstanceName="HF" ClientVisible="False" Height="16px" Width="94px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="F" runat="server" ClientInstanceName="F" Height="16px" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="margin-right: 40px;">
                                    <dx:ASPxTextBox ID="DistWar" runat="server" ClientInstanceName="DistWar" Text="0" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="margin-right: 40px" class="auto-style4">&nbsp;احتفاظ خاص (قسط)</td>
                                <td style="margin-right: 40px">
                                    <dx:ASPxTextBox ID="SpRet" runat="server" ClientInstanceName="SpRet" Height="17px" Text="0" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style9"></td>
                                <td class="auto-style1" colspan="4"></td>
                                <td class="auto-style1">
                                </td>
                                <td class="auto-style1" style="text-align: left">نسبة الاختياري %</td>
                                <td style="text-align: right" class="auto-style1">
                                    من القسط الأساسي</td>
                                <td style="margin-right: 40px" class="auto-style1">
                                    من قسط الحروب</td>
                                <td style="margin-right: 40px" class="auto-style5">احتفاظ خاص (حروب)</td>
                                <td style="margin-right: 40px" class="auto-style1">
                                    <dx:ASPxTextBox ID="WSpRet" runat="server" ClientInstanceName="WSpRet" Height="17px" Text="0" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style9">&nbsp;</td>
                                <td class="auto-style1" colspan="4">&nbsp;</td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="HFM" runat="server" ClientInstanceName="HFM" ClientVisible="False" Height="16px" Width="94px">
                                    </dx:ASPxTextBox>
                                </td>
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
                                <td style="margin-right: 40px">
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
                                <td class="dxeICC" rowspan="2">&nbsp;الرقم </td>
                                <td rowspan="2">
                                    <dx:ASPxTextBox ID="TreatyNo" runat="server" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style6" colspan="2" rowspan="2">&nbsp;</td>
                                <td class="auto-style6">&nbsp;</td>
                                <td colspan="3" rowspan="2">
                                    <dx:ASPxTextBox ID="CustName0" runat="server" Height="16px" Width="353px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style6">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style2">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td class="auto-style6" colspan="2">&nbsp;</td>
                                <td class="auto-style6">عدد الإعيان</td>
                                <td colspan="3">
                                    <dx:ASPxTextBox ID="EndCnt" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style18"><strong><span class="auto-style19">&nbsp;أولاً <span class="auto-style20">بنوذ</span> الإتفاقية</span></strong></td>
                                <td class="style1" colspan="2">&nbsp;</td>
                                <td class="dxeICC">مبلغ التأمين</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="TSumIns" runat="server" Width="170px" DisplayFormatString="N3">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="CustNo" runat="server" Visible="False" Width="120px" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="TOTPRM" runat="server"  Width="120px" DisplayFormatString="N3" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Cur" runat="server" Visible="False" Width="120px" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style5" colspan="2">&nbsp;</td>
                                <td align="center" colspan="2" class="auto-style1"></td>
                                <td align="center" class="auto-style1"></td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="auto-style12"><strong>سعة الإتفاقية</strong></td>
                                <td class="auto-style1">
                                    <strong>إحتفاظ الشركة</strong></td>
                                <td class="auto-style1"><strong>مشاركة</strong></td>
                                <td class="auto-style1"><strong>فائض أول</strong></td>
                                <td class="auto-style12"><strong>فائض ثاني</strong></td>
                                <td class="auto-style12"><strong>لاين سليب (في حالة البحري)</strong></td>
                                <td class="auto-style12"><strong>إختياري</strong></td>
                            </tr>
                            <tr>
                                <td class="dxeICC"><strong>حدود الاتفاقية</strong></td>
                                <td class="dxeICC">
                                    <dx:ASPxTextBox ID="Capacity" runat="server" ClientReadOnly="True" ForeColor="#ff0000" Font-Bold="true" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="Ret" runat="server" ClientReadOnly="True" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="QS" runat="server" ClientReadOnly="True" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="FirstSup" runat="server" ClientReadOnly="True" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeCaptionHARSys">
                                    <dx:ASPxTextBox ID="SecondSup" runat="server" ClientReadOnly="True" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeCaptionHARSys">
                                    <dx:ASPxTextBox ID="LineSlip" runat="server" ClientReadOnly="True" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeCaptionHARSys">
                                    <dx:ASPxTextBox ID="Fac" runat="server" ClientReadOnly="True" Width="120px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style8"><strong>العمولات</strong></td>
                                <td class="auto-style8"><strong>للأقساط %</strong></td>
                                <td class="auto-style2">
                                    &nbsp;</td>
                                <td class="auto-style2">
                                    <strong>
                                    <dx:ASPxTextBox ID="TRQSRCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                    </strong>
                                </td>
                                <td class="auto-style2">
                                    <dx:ASPxTextBox ID="TR1STCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style15">
                                    <dx:ASPxTextBox ID="TR2STCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style15">
                                    <dx:ASPxTextBox ID="TRLSCOMM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style15">
                                    <dx:ASPxTextBox ID="FacComm" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <dx:ASPxTextBox ID="TSpRet" runat="server" ClientVisible="False" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dxeICC"><strong>للحروب %</strong></td>
                                <td class="style1">
                                    &nbsp;</td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="TRWQSRCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="style1">
                                    <dx:ASPxTextBox ID="TRW1STCOM" runat="server" Width="50px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <dx:ASPxTextBox ID="TRet" runat="server" ClientVisible="False" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="wFac" runat="server" ClientVisible="False" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="TNetPrm" runat="server" ClientVisible="False" Width="120px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="TQS" runat="server" Height="17px" Width="109px" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center">
                                    <dx:ASPxTextBox ID="wQS" runat="server" Height="17px" Width="109px" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style13">&nbsp;</td>
                                <td class="auto-style14">
                                    <dx:ASPxTextBox ID="TSecondSup" runat="server" ClientVisible="False" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style13">
                                    <dx:ASPxButton ID="ASPxButton5" runat="server" Text="توزيع">
                                    </dx:ASPxButton>
                                </td>
                                <td class="auto-style13">
                                    <dx:ASPxTextBox ID="TFac" runat="server" ClientVisible="False" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style13">
                                    <dx:ASPxTextBox ID="TLineSlip" runat="server" ClientVisible="False" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" colspan="2" class="auto-style13">
                                    <dx:ASPxTextBox ID="TFirstSup" runat="server" Height="17px" Width="109px" ClientVisible="False">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="center" class="auto-style13">
                                    <dx:ASPxTextBox ID="wFirstSup" runat="server" ClientVisible="False" Height="17px" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style18" colspan="2"><strong><span class="auto-style19">ثانياً : توزيع القسط</span></strong></td>
                                <td class="style1" colspan="2">
                                </td>
                                <td class="style1">
                                    &nbsp;</td>
                                <td>&nbsp;<dx:ASPxLabel ID="AlertL0" runat="server" BackColor="White" ClientVisible="False" Style="font-weight: 700" Text="قيمة الرسوم المضافة" Width="160px">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="ExtPrm" runat="server" ClientInstanceName="ExtPrm" ClientVisible="False" EnableClientSideAPI="True" Height="17px" Text="0" Width="109px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2" colspan="8">
                                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Width="100%">
                                        <Settings ShowFooter="True" />
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False">
                                            </FilterControl>
                                        </SettingsPopup>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="رقم الوثيقة" FieldName="PolNo" ShowInCustomizationForm="True" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ملحق" FieldName="EndNo" ShowInCustomizationForm="True" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="إشعار" FieldName="LoadNo" ShowInCustomizationForm="True" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="صافي القسط" FieldName="Net" ShowInCustomizationForm="True" VisibleIndex="4">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="مبلغ التأمين" FieldName="InsAmt" ShowInCustomizationForm="True" VisibleIndex="5">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="إحتفاظ" FieldName="Amount" ShowInCustomizationForm="True" VisibleIndex="6">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="مشاركة" FieldName="Qs" ShowInCustomizationForm="True" VisibleIndex="7">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="فائض أول" FieldName="FirsSup" ShowInCustomizationForm="True" VisibleIndex="8">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="تاريخ الإصدار" FieldName="PolD" ShowInCustomizationForm="True" VisibleIndex="3">
                                                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="فائض ثاني" FieldName="SecondSup" ShowInCustomizationForm="True" VisibleIndex="9">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="لاين سليب" FieldName="LineSlip" ShowInCustomizationForm="True" VisibleIndex="10">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="إختياري" FieldName="Elective" ShowInCustomizationForm="True" VisibleIndex="11">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="قسط الحروب" FieldName="War" ShowInCustomizationForm="True" VisibleIndex="12">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="مشاركة حروب" FieldName="Qsw" ShowInCustomizationForm="True" VisibleIndex="13">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="فائض أول حروب" FieldName="FirsSupw" ShowInCustomizationForm="True" VisibleIndex="14">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="إختياري حروب" FieldName="Electivew" ShowInCustomizationForm="True" VisibleIndex="15">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="احتفاظ خاص" FieldName="SpecialRet" ShowInCustomizationForm="True" VisibleIndex="16">
                                                <PropertiesTextEdit DisplayFormatString="N3">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem DisplayFormat="N3" FieldName="Amount" ShowInColumn="إحتفاظ" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="N3" FieldName="Net" ShowInColumn="صافي القسط" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="N3" FieldName="Qs" ShowInColumn="مشاركة" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="N3" FieldName="FirsSup" ShowInColumn="فائض أول" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="N3" FieldName="InsAmt" ShowInColumn="مبلغ التأمين" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="N3" FieldName="Elective" ShowInColumn="إختياري" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem DisplayFormat="N3" FieldName="SpecialRet" ShowInColumn="احتفاظ خاص" SummaryType="Sum" ValueDisplayFormat="N3" />
                                        </TotalSummary>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    &nbsp;</td>
                                <td>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>" SelectCommand="SELECT PolNo,LoadNo, EndNo,PolD, InsAmt, Net, War, Cur, Exc, Amount, Qs, FirsSup, SecondSup, Elective, LineSlip, Qsw, FirsSupw, Electivew, SpecialRet FROM NetPrm WHERE (PolNo = @PolNo) AND (EndNo = @EndNo) AND (LoadNo = @LoadNo)">
                                        <SelectParameters>
                                            <asp:Parameter DbType="String" Name="PolNo" />
                                            <asp:Parameter DbType="Int32" Name="EndNo" />
                                            <asp:Parameter DbType="Int32" Name="LoadNo" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td colspan="2" class="style1">
                                    &nbsp;</td>
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