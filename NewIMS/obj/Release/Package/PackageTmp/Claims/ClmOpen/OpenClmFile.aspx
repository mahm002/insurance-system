<%@ Page EnableEventValidation="false" Language="VB" AutoEventWireup="true" Inherits="OpenClmFile" CodeBehind="OpenClmFile.aspx.vb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>› Õ „·› Õ«œÀ</title>
    <%-- <link href="../../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />--%>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.quicksearch.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {

            $('input#id_search').quicksearch('table#GridView1 tbody tr');
        })
        //function RetrievePicture(imgCtrl, picid) {
        //    imgCtrl.onload = null;
        //    imgCtrl.src = 'Images/ShowImage.ashx?id=' + picid;
        //}
        function SetEvent(item, IDName) {
            if (document.all) {
                if (event.keyCode == 13) {
                    event.returnValue = false;
                    event.cancel = true;
                    if (IDName == '')
                        document.getElementById(item).focus()
                    else
                        document.getElementById(IDName).object.SetFocus()
                }
            }
        }
        function SetOnSelectEvent(Elm, IDName) {
            if (IDName == '')
                document.getElementById(Elm).focus()
            else
                document.getElementById(IDName).object.SetFocus()
        }

        function openWindow() {
            alert(document.URL);
        }

        function SetClmdte(s, e) {
            var dt = ClmDate.GetInputElement().value;
            tmpdate.SetText(dt);
        }
        function onStartClick(s, e) {
            if (ASPxClientEdit.ValidateGroup('ClmValidation'))
                doOperation('OpenNewClaim');
        }

        function UpdateClick(s, e) {

            cbps.PerformCallback('UpdateMainData');
        }

        function onStartAdd(s, e) {
            doOperation('NewParty');
        }

        function doOperation(name) {
            serverOperations.PerformCallback(name);
        }
        function onCallbackComplete(s, e) {

            if (s.cpShowIssueConfirmBox && s.cpMyAttribute == 'Issuance') {
                pcConfirmIssue.Show();
            }
            if (s.cpMyAttribute == 'Claimed') {
                ClaimedPop.Show();
            }

            s.cpMyAttribute = ''
            s.cpNewWindowUrl = null

        }
        function YesIss_Click() {
            pcConfirmIssue.Hide();
            serverOperations.PerformCallback("Issue");
        }

        function NoIss_Click() {
            pcConfirmIssue.Hide()
        }

        function OnGridFocusedRowChanged() {
            // Query the server for the "DAILYNUM" field from the focused row
            // The single value will be returned to the OnGetRowValues() function
            // grid.GetRowValues(grid.GetFocusedRowIndex(), 'CustName;EndNo;GroupNo;IssuDate;Ret', OnGetRowValues);
            //s.UnselectAllRowsOnPage();
            serverOperations.PerformCallback("Check");
        }

        function OnGetRowValues(value) {
            //alert(value);
            grid.PerformCallback(value);
        }

        function onBeginCallback(s, e) {
            if (e.command == ASPxClientGridViewCallbackCommand.ApplySearchPanelFilter) {
                //s.UnselectAllRowsOnPage();
                //s.SetFocusedRowIndex(-1);
            }
        }
        function onEndCallback(s, e) {
            //debugger;

            //if (s.cpCustName != null) {
            //   // alert(s.cpIdx);
            //    CustName.SetValue(s.cpCustName);
            //    GroupNo.SetValue(s.cpGroupNo);
            //    EndNo.SetValue(s.cpEndNo);
            //    IssDate.SetValue(s.cpIssuDate);
            //    Ret.SetValue(s.cpRet);
            //    //Idxs.SetValue(s.cpIdx);
            //    //idx.SetValue(s.cpidx);

            //    //s.SetEditValue('AccNtnum', s.cpAccnum);
            //    //s.SetEditValue('Debtor', s.cpDebtor);
            //    //s.SetEditValue('Creditor', s.cpCreditor);
            //    //s.SetEditValue('DocNum', s.cpDocNum);
            //}
            //return;
            //s.cpCustName = null;
            //serverOperations.PerformCallback("Check");
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            text-align: left;
            height: 23px;
            width: 271px;
        }

        .auto-style2 {
            text-align: left;
            width: 271px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
          <dx:ASPxCallback ID="Cbps" ClientInstanceName="cbps" runat="server" OnCallback="Cbps_Callback"></dx:ASPxCallback>
        <div>
            <%--<dx:ASPxCallback ID="Cbp" runat="server" ClientInstanceName="serverOperations" OnCallback="Cbp_Callback">
                <ClientSideEvents CallbackComplete="onCallbackComplete" />
                   </dx:ASPxCallback> --%>

            <dx:ASPxCallbackPanel ID="Cbp" runat="server" ClientInstanceName="serverOperations" OnCallback="Cbp_Callback">
                <SettingsLoadingPanel Text="«Õ ”«»&amp;hellip;" Delay="10" ShowImage="false" Enabled="false" />
                <ClientSideEvents EndCallback="onCallbackComplete" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table style="width: 100%;" dir="rtl">
                            <tr>
                                <td
                                    style="background-image: url('../../images/GlossyLBlue.png'); text-align: center; font-weight: 700; border-top-style: solid; border-top-color: #6699FF; color: #FFFFFF;"
                                    height="32px" dir="rtl" colspan="8">»Ì«‰«  «·ÊÀÌﬁ… Ê«·„ƒ„‰ ·Â</td>
                            </tr>
                            <tr>
                                <td style="border-bottom: #359623 thin solid;" class="auto-style1">
                                    <asp:Label ID="Label3" runat="server" BackColor="Transparent" Font-Bold="True" Height="20px" Text="—ﬁ„ «·ÊÀÌﬁ…" Width="139px"></asp:Label>
                                </td>
                                <td align="right" style="border-bottom: #359623 thin solid;" class="auto-style1" colspan="2">
                                    <dx:ASPxTextBox ID="PolNo" runat="server" AutoPostBack="true" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="right" style="height: 23px; border-bottom: #359623 thin solid;">
                                    <dx:ASPxTextBox ID="EndNo" runat="server" Width="60px" ClientVisible="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="height: 23px; border-bottom: #359623 thin solid;" class="dx-al">&nbsp;</td>
                                <td align="right" style="height: 23px; border-bottom: #359623 thin solid;">
                                    <dx:ASPxTextBox ID="LoadNo" runat="server" Width="60px" ClientVisible="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="right" style="height: 23px; border-bottom: #359623 thin solid;">
                                    <dx:ASPxTextBox ID="GroupNo" ClientInstanceName="GroupNo" runat="server" Width="60px" ClientVisible="false">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Idxs" ClientInstanceName="Idxs" runat="server" Width="60px" ClientVisible="false">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-bottom: #359623 thin solid;" class="auto-style1">
                                    <asp:Label ID="Label4" runat="server" BackColor="Transparent" Font-Bold="True" Text="«”„ «·„ƒ„‰" Width="139px"></asp:Label>
                                </td>
                                <td align="right" style="border-bottom: #359623 thin solid;" class="auto-style1" colspan="2">
                                    <%--  <asp:TextBox ID="CustName" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="258px"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="CustName" ClientInstanceName="CustName" runat="server" Width="100%" ClientReadOnly="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td align="right" style="height: 23px; border-bottom: #359623 thin solid;"></td>
                                <td align="right" style="height: 23px; border-bottom: #359623 thin solid;">
                                    <dx:ASPxDateEdit ID="IssDate" runat="server" ClientInstanceName="IssDate" ClientVisible="false"
                                        ClientReadOnly="True" DisplayFormatString="yyyy/MM/dd" RightToLeft="True" Width="150px">
                                        <ValidationSettings>
                                            <ErrorFrameStyle ImageSpacing="4px">
                                                <ErrorTextPaddings PaddingLeft="4px" />
                                            </ErrorFrameStyle>
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </td>
                                <td align="right" style="height: 23px; border-bottom: #359623 thin solid;" colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="border-bottom: #359623 thin solid;" class="auto-style1">
                                    <asp:Label ID="Label5" runat="server" BackColor="Transparent"
                                        Font-Bold="True" Height="20px" Text="—ﬁ„ «·Õ«œÀ"
                                        Width="139px"></asp:Label>
                                </td>
                                <td align="right" style="height: 23px; border-bottom: #359623 thin solid;">
                                    <dx:ASPxTextBox ID="ClmNo" runat="server" Width="170px" ClientReadOnly="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="border-bottom: #359623 thin solid;" class="dx-al" colspan="2">
                                    <%--<strong> «—ÌŒ «·› Õ</strong></td>--%>
                                    <td align="right" style="height: 23px; border-bottom: #359623 thin solid;">&nbsp;</td>
                                    <td style="height: 23px; border-bottom: #359623 thin solid;" colspan="2">
                                        <dx:ASPxTextBox ID="tmpdate" runat="server" ClientVisible="false" ClientEnabled="False" ClientInstanceName="tmpdate" Width="170px">
                                        </dx:ASPxTextBox>
                                    </td>
                            </tr>
                            <tr>
                                <td style="height: 23px;" colspan="8">
                                    <dx:aspxgridview id="DetailsGrd" clientinstancename="grid" runat="server" datasourceid="SqlDataSource1" width="100%" visible="false"
                                        settingsbehavior-allowclienteventsonload="false" onselectionchanged="DetailsGrd_FocusedRowChanged"
                                        oncustomcallback="DetailsGrd_CustomCallback" settingsdatasecurity-allowreadunlistedfieldsfromclientapi="True"
                                        settings-showcolumnheaders="false" enablecallbacks="true">

                                        <columns>
                                        </columns>

                                        <settingsbehavior allowfocusedrow="True" processselectionchangedonserver="true" />
                                        <clientsideevents endcallback="function(s, e) { OnGridFocusedRowChanged(); }" rowclick="" />

                                        <settings showcolumnheaders="False" />

                                        <settingsbehavior processfocusedrowchangedonserver="true" />
                                        <settingsdatasecurity allowdelete="False" allowedit="False" allowinsert="False" />
                                        <settingspopup>
                                            <filtercontrol autoupdateposition="False">
                                            </filtercontrol>
                                        </settingspopup>
                                        <settingssearchpanel visible="True" delay="2500"></settingssearchpanel>
                                    </dx:aspxgridview>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style11" colspan="8">

                                    <asp:Panel ID="pnlClaim" runat="server" Style="display: none;" Width="500px">

                                        <asp:Panel ID="PersonCaption" runat="server" BackImageUrl="~/images/GlossyRed.png" Style="color: #ffffff;">ÌÊÃœ Õ«œÀ ”«»ﬁ</asp:Panel>

                                        <div>
                                        </div>
                                        <div>
                                        </div>
                                        <div class="clearer">
                                        </div>
                                        <div style="white-space: nowrap; text-align: center;">

                                            <asp:Button ID="btnCancelPerson" runat="server" CausesValidation="false" Text="Cancel" />
                                        </div>
                                    </asp:Panel>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server">
                                        <SelectParameters>
                                            <asp:Parameter Name="Policy" DefaultValue="0" />
                                            <asp:Parameter Name="System" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>'>
                                        <SelectParameters>
                                            <asp:FormParameter DefaultValue="0" FormField="PolNo" Name="Policy" />
                                            <asp:FormParameter DefaultValue="" FormField="sys" Name="System" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td align="left"
                                    style="background-image: url('../../images/GlossyLBlue.png'); text-align: center; font-weight: 700; border-top-style: solid; border-top-color: #6699FF; color: #FFFFFF;"
                                    class="auto-style1" height="32px" dir="rtl" colspan="8">
                                    <strong dir="rtl">»Ì«‰«  «·Õ«œÀ </strong>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2"> «—ÌŒ «·Õ«œÀ</td>
                                <td>
                                    <dx:ASPxDateEdit ID="ClmDate" ClientInstanceName="ClmDate" runat="server"
                                        DisplayFormatString="yyyy/MM/dd" RightToLeft="True" Width="150px">
                                        <ClientSideEvents ValueChanged="SetClmdte"></ClientSideEvents>
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <ErrorFrameStyle ImageSpacing="4px">
                                                <ErrorTextPaddings PaddingLeft="4px" />
                                            </ErrorFrameStyle>
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </td>
                                <td class="dx-al" colspan="2"> «—ÌŒ «·»·«€&nbsp;
                                </td>
                                <td colspan="3">

                                    <dx:ASPxDateEdit ID="InfDate" runat="server" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" DisplayFormatString="yyyy/MM/dd" RightToLeft="True" SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css" Width="150px">
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <ErrorFrameStyle ImageSpacing="4px">
                                                <ErrorTextPaddings PaddingLeft="4px" />
                                            </ErrorFrameStyle>
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2">«”„ «·„»·€&nbsp;
                                </td>
                                <td class="auto-style12">
                                    <dx:ASPxTextBox ID="InfName" runat="server" Width="100%">
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al" colspan="2">«”„ «·„⁄«Ì‰&nbsp;
                                </td>
                                <td class="auto-style12" colspan="3">

                                    <dx:ASPxTextBox ID="PrevName" runat="server" Width="100%">
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2">„»·€ «· √„Ì‰</td>
                                <td class="auto-style12">
                                    <dx:ASPxTextBox ID="SumIns" runat="server" Text="0" Width="170px" DisplayFormatString="n3" ClientEnabled="False">
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al" colspan="2">„Êﬁ⁄ «·Õ«œÀ</td>
                                <td class="auto-style12" colspan="3">

                                    <dx:ASPxTextBox ID="ClmPlace" runat="server" Text="0" Width="100%">
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2">”»» «·Õ«œÀ</td>
                                <td class="auto-style12">
                                    <dx:ASPxTextBox ID="ClmReason" runat="server" Text="0" Width="100%">
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="dx-al" colspan="2">«· Õ„·&nbsp; </td>
                                <td class="auto-style12" colspan="3">

                                    <dx:ASPxTextBox ID="Ret" ClientInstanceName="Ret" runat="server" Text="0" Width="100%" ClientEnabled="False">
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2">

                                    <span class="style8">Ê’› «·√÷—«—</span>&nbsp;
                                </td>
                                <td class="auto-style10" colspan="7">
                                    <dx:ASPxTextBox ID="DmgDiscription" runat="server" Height="80px" Width="100%">
                                        <ValidationSettings ValidationGroup="ClmValidation">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2" colspan="2">
                                    <dx:ASPxButton ID="Update" runat="server" AutoPostBack="False" Text=" ÕœÌÀ »Ì«‰«  «·Õ«œÀ">
                                        <ClientSideEvents Click="UpdateClick" />
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="Button1" runat="server" Text="› Õ „·› Õ«œÀ" AutoPostBack="false">
                                        <ClientSideEvents Click="onStartClick" />
                                    </dx:ASPxButton>
                                </td>
                                <td colspan="6">

                                    <dx:ASPxPopupControl ID="ClaimedPop" runat="server" ClientInstanceName="ClaimedPop" HeaderText=" ÌÊÃœ Õ«œÀ ”«»ﬁ " HeaderStyle-BackColor="Red"
                                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="400px">
                                        <HeaderStyle BackColor="Red"></HeaderStyle>
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server">
                                                <table dir="rtl" style="width: 100%">
                                                    <tr>
                                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%">
                                                            <Columns>
                                                                <asp:BoundField DataField="ClmNo" HeaderText="—ﬁ„ «·Õ«œÀ" />
                                                                <asp:BoundField DataField="PolNo" HeaderText="—ﬁ„ «·ÊÀÌﬁ…" />
                                                                <asp:BoundField DataField="GroupNo" HeaderText="«·—ﬁ„ «· ”·”·Ì" />
                                                                <asp:BoundField DataField="ClmDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText=" «—ÌŒ «·Õ«œÀ" HtmlEncode="False" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </tr>
                                                </table>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>

                                    <dx:ASPxPopupControl ID="pcConfirmIssue" runat="server" ClientInstanceName="pcConfirmIssue" HeaderText="› Õ „·› Õ«œÀ" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="false" Width="250px">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server">
                                                <table dir="rtl">
                                                    <tr>
                                                        <td colspan="2" style="align-items: center;"> √ﬂÌœ › Õ „·› ··Õ«œÀ° ›Ì Õ«· «·„Ê«›ﬁ… ·‰ Ì„ﬂ‰ﬂ «· ⁄œÌ· ›Ì «·»Ì«‰« ø </td>
                                                    </tr>
                                                    <tr>
                                                        <td><a href="javascript:YesIss_Click()">„Ê«›ﬁ</a> </td>
                                                        <td><a href="javascript:NoIss_Click()">€Ì— „Ê«›ﬁ</a> </td>
                                                    </tr>
                                                </table>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2" colspan="8">

                                    <table id="TABLE1" cellspacing="0" cellpadding="0" runat="server" visible="True" width="100%" class="mGrid">
                                        <tr>
                                            <td
                                                style="background-image: url('../../images/GlossyLBlue.png'); text-align: center; font-weight: 700; border-top-style: solid; border-top-color: #6699FF; color: #FFFFFF;"
                                                class="auto-style1" dir="rtl" colspan="6">»Ì«‰«  «·„ ÷——Ì‰</td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al"><strong>‰Ê⁄</strong> <strong>«·„ ÷——</strong></td>
                                            <td class="auto-style3" colspan="4">
                                                <dx:ASPxComboBox ID="TPId" runat="server" DataSourceID="TP" SelectedIndex="1" TextField="TPName" ValueField="TpNo" Width="100%">
                                                </dx:ASPxComboBox>
                                            </td>

                                            <td class="auto-style2">&nbsp;</td>
                                            <td class="auto-style3">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al"><strong>«·„«·ﬂ </strong></td>
                                            <td class="auto-style13" colspan="4">
                                                <dx:ASPxTextBox ID="ThirdParty" runat="server" Width="100%">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td class="dx-al">
                                                <strong>
                                                    <dx:ASPxTextBox ID="Asset" runat="server" Caption="«·»Ì«‰ «·„ ÷——	" Width="100%">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al"><strong>&nbsp;Ê’› «·√÷—«—</strong></td>
                                            <td class="auto-style13" colspan="5">
                                                <dx:ASPxTextBox ID="Damage" runat="server" Width="100%">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="dx-al"><strong>ﬁÌ„… «·√÷—«— ( «·«Õ Ì«ÿ «·„ﬁœ—)</strong></td>
                                            <td align="right" class="auto-style13" colspan="4">
                                                <dx:ASPxTextBox ID="Value" runat="server" Text="0" Width="170px">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>

                                            <td class="auto-style3" align="right">
                                                <dx:ASPxButton ID="Button12" runat="server" AutoPostBack="true" Text="≈÷«›…">
                                                    <ClientSideEvents Click="onStartAdd" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style20" colspan="6">
                                                <asp:TextBox ID="Sno" runat="server" BorderColor="Gray" BorderStyle="solid" BorderWidth="1" Font-Names="arial" Height="20px" valign="middle" Visible="False" Width="100px"></asp:TextBox>
                                                <asp:SqlDataSource ID="TP" runat="server"
                                                    ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"></asp:SqlDataSource>
                                                <dx:ASPxGridView ID="ASPxGridView1" runat="server" DataSourceID="SqlDataSource4" Width="100%"
                                                    AutoGenerateColumns="False" KeyFieldName="Sn">
                                                    <SettingsPopup>
                                                        <FilterControl AutoUpdatePosition="False">
                                                        </FilterControl>
                                                    </SettingsPopup>
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0"></dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Sn" ReadOnly="True" VisibleIndex="0" Visible="false">
                                                            <EditFormSettings Visible="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ClmNo" VisibleIndex="1" Visible="false"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="TPID" Caption="‰Ê⁄ «·„ ÷——" ReadOnly="True" VisibleIndex="2">
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ThirdParty" Caption="«·„«·ﬂ" VisibleIndex="3"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Asset" Caption="«·»Ì«‰ «·„ ÷——" VisibleIndex="4"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Damage" Caption="Ê’› «·√÷—«—" VisibleIndex="5"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Value" Caption="«·ﬁÌ„… «· ﬁœÌ—Ì… («·«Õ Ì«ÿÌ)" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="N3">
                                                            <PropertiesTextEdit DisplayFormatString="N3"></PropertiesTextEdit>

                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                </dx:ASPxGridView>
                                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString='<%$ ConnectionStrings:IMS-DBConnectionString %>'
                                                    SelectCommand="SELECT Sn, ClmNo,Damage,iif(thirdparty.TPID = 0,'«·„ƒ„‰ ·Â','ÿ—› À«·À') as TPID,ThirdParty,Asset,[dbo].[GetLastEstimation](ClmNo,TPID) As Value FROM [ThirdParty] WHERE ([ClmNo] = @ClmNo)"
                                                    UpdateCommand="UPDATE [ThirdParty] SET [Asset] = @Asset,[ThirdParty] = @ThirdParty, [Damage] = @Damage WHERE [ClmNo] = @ClmNo AND [Sn] = @Sn">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ClmNo" PropertyName="Text" DefaultValue="0" Name="ClmNo" Type="String"></asp:ControlParameter>
                                                    </SelectParameters>
                                                    <UpdateParameters>
                                                        <asp:Parameter Name="Sn" Type="Int64"></asp:Parameter>
                                                        <asp:Parameter Name="ThirdParty" Type="String"></asp:Parameter>
                                                        <asp:Parameter Name="Asset" Type="String"></asp:Parameter>
                                                        <asp:Parameter Name="Damage" Type="String"></asp:Parameter>
                                                        <asp:ControlParameter ControlID="ClmNo" PropertyName="Text" DefaultValue="0" Name="ClmNo" Type="String"></asp:ControlParameter>
                                                    </UpdateParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:AttachConString %>"
                                                    SelectCommand="SELECT Id, ClmNo, Image, UserName FROM ClaimsAttachments WHERE (ClmNo = @ClmNo)">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ClmNo" DefaultValue="0" Name="ClmNo" PropertyName="Text" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style20">&nbsp;</td>
                                            <td class="auto-style14">&nbsp;</td>
                                            <td class="auto-style15">&nbsp;</td>
                                            <td class="auto-style17">&nbsp;</td>
                                            <td class="auto-style6">&nbsp;</td>
                                            <td class="auto-style6">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left"
                                    style="background-image: url('../../images/GlossyLBlue.png'); text-align: center; font-weight: 700; border-top-style: solid; border-top-color: #6699FF; color: #FFFFFF;"
                                    class="auto-style1" height="32px" dir="rtl" colspan="8">≈—›«ﬁ „·›«  </td>
                            </tr>
                            <tr>
                                <td class="auto-style2" colspan="4">

                                    <asp:FileUpload ID="imgUpload" runat="server" Height="31px" Visible="False" />
                                </td>
                                <td class="auto-style2" colspan="4">

                                    <asp:Button ID="btnSubmit" runat="server" CssClass="CaptionM" Text=" Õ„Ì·" Visible="False" Width="81px" />
                                    <asp:SqlDataSource runat="server" ID="ImagesDataSource" ConnectionString='<%$ ConnectionStrings:AttachDB %>'
                                        SelectCommand="SELECT [Id], [ClmNo], [UserName], [Image] FROM [ClaimsAttachments] WHERE [ClmNo] = @ClmNo">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ClmNo" PropertyName="Text" Name="ClmNo" Type="String"></asp:ControlParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <dx:ASPxImageGallery ID="Gallery" runat="server" ImageContentBytesField="Image" AllowPaging="true"
                                        DataSourceID="ImagesDataSource" Width="100%">
                                        <SettingsFolder ImageCacheFolder="~\Thumb\" />
                                        <SettingsFullscreenViewer NavigationBarVisibility="Always" ShowTextArea="true" AllowMouseWheel="true" />
                                    </dx:ASPxImageGallery>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1" colspan="4">

                                    <asp:Label ID="lblResult" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td class="auto-style1" colspan="4"></td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

            <%--<asp:PlaceHolder ID="PlaceHolder1" runat="server" />
         <%--</ContentTemplate>
           <Triggers>
                <asp:AsyncPostBackTrigger ControlID="PolNo" EventName="TextChanged" />
            </Triggers>
	</asp:UpdatePanel>--%>
        </div>
    </form>
</body>
</html>