<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DailySarf.aspx.vb" Inherits=".DailySarf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../scripts/Scripts.js"></script>
    <%--   <script  src="../scripts/jquery-latest.min.js"></script>--%>
    <script src="../scripts/jquery.min.js"></script>

    <script type="text/javascript">

        function preventBack() {
            window.history.forward();
        }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };

        $(document).on('keyup', 'input,a,select,textarea', function (e) {
            e.preventDefault();
            if (e.keyCode == 13 && e.target.type !== 'submit') {
                //debugger;
                var inputs = $(e.target).parents("form").eq(0).find('input,a,select,textarea').filter(':visible:enabled');
                idx = inputs.index(e.target);
                if (idx == inputs.length - 1) {
                    //debugger;
                    //inputs[0].select()
                    inputs[idx + 1].focus();
                } else {
                    //debugger;
                    //getParameterByName('Sys')==3
                    inputs[idx + 1].focus();
                    if (idx < 8) {
                        inputs[idx + 1].select();
                    } else {
                        //debugger;
                        switch (idx) {
                            case 8: {
                                if (getParameterByName('Sys') == '3') {
                                    inputs[idx + 1].select();
                                }
                                else {
                                    btnShow.DoClick();
                                    $('input:text:first').focus();
                                }
                            }
                            case 8: {
                                if (getParameterByName('Sys') == '3' && idx !== 8) {
                                    btnShow.DoClick();
                                    $('input:text:first').focus();
                                }
                                else {
                                    inputs[idx + 1].select();
                                }
                            }
                        }
                    }
                }
            }
        });

        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }

        function SaveData(s, e) {
            cbp.SetLoadingPanelText("حفظ");
            if (ASPxClientEdit.ValidateGroup('Data')) {
                if (cbp.InCallback()) return;
                cbp.PerformCallback('Save');
            }

        }

        function IssueDoc(s, e) {
            cbp.SetLoadingPanelText("مراجعة واعتماد القيد");
            if (cbp.InCallback()) return;
            cbp.PerformCallback('Issue');
        }

        function AccChange(s, e) {
            cbp.SetLoadingPanelText("التأكد من رقم الحساب");
            if (ASPxClientEdit.ValidateGroup('Data')) {
                if (cbp.InCallback()) return;
                cbp.PerformCallback('AccCheck');
            }
        }

        function OnGridFocusedRowChanged() {
            // Query the server for the "DAILYNUM" field from the focused row
            // The single value will be returned to the OnGetRowValues() function

            grid.GetRowValues(grid.GetFocusedRowIndex(), 'GroupNo;DAILYNUM;RealAccountNo;Dr;Cr;DocNum;CustName;Note;idx', OnGetRowValues);
        }

        function OnGetRowValues(value) {
            //alert(value);
            if (grid.InCallback()) return;
            grid.PerformCallback(value);
        }

        function onEndCallback(s, e) {
            //debugger;
            if (s.cpAccnum != null) {
                //debugger;
                if (s.cpshow == '1') {
                    CustName.SetVisible(true);
                    DocNum.SetVisible(true);
                    DocNum.SetValue(s.cpDocNum);
                    CustName.SetValue(s.cpCustName);
                }
                else {
                    if (s.cpTP == 3) {
                        CustName.SetVisible(false);
                        DocNum.SetVisible(false);
                    }

                }

                AccNtnum.SetValue(s.cpAccnum);
                GroupNo.SetValue(s.cpGroupNo);
                Debtor.SetValue(s.cpDebtor);
                Creditor.SetValue(s.cpCreditor);
                Note.SetValue(s.cpNote);
                idx.SetValue(s.cpidx);
                btnShow.SetText("تعديل");

                //s.SetEditValue('AccNtnum', s.cpAccnum);
                //s.SetEditValue('Debtor', s.cpDebtor);
                //s.SetEditValue('Creditor', s.cpCreditor);
                //s.SetEditValue('DocNum', s.cpDocNum);
            }
            else {
                cbp.PerformCallback();
            }
            //return;
            s.cpAccnum = null
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 33px;
        }

        .auto-style2 {
            font-size: x-large;
        }

        .auto-style5 {
            text-align: left;
            height: 30px;
        }

        .auto-style6 {
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="Callback" runat="server" ClientInstanceName="cbp" OnCallback="Callback_Callback">
                <ClientSideEvents />
                <SettingsLoadingPanel Text="حفظ&amp;hellip;" Delay="0" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table style="width: 100%;" dir="rtl">
                            <tr>
                                <td class="dx-al">رقم اليومية</td>
                                <td>
                                    <dx:ASPxTextBox ID="DailyNum" runat="server" Width="170px" ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>&nbsp;</td>
                                <td class="dx-al">التحليلي</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="AnalsNum" runat="server" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="RecNo" runat="server" Width="170px" ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                    <asp:SqlDataSource ID="Cur" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select TPName,TPNo from EXTRAINFO where TP='Cur' Order By TpNo"></asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style5">تاريخ اليومية</td>
                                <td class="auto-style6">
                                    <dx:ASPxDateEdit ID="DailyDte" runat="server" AutoPostBack="true" OnValueChanged="DailyDte_ValueChanged" DisplayFormatString="yyyy/MM/dd" EditFormatString="yyyy/MM/dd" RightToLeft="True">
                                    </dx:ASPxDateEdit>
                                </td>
                                <td class="auto-style6"></td>
                                <td class="auto-style5">رقم القيد</td>
                                <td colspan="2" class="auto-style6">
                                    <dx:ASPxTextBox ID="DailySRL" runat="server" Width="170px" ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2" class="auto-style6"></td>
                            </tr>
                            <tr>
                                <td class="dx-al">العملة</td>
                                <td>
                                    <dx:ASPxComboBox ID="Currency" runat="server" ClientInstanceName="Currency" DataSourceID="Cur"
                                        DropDownStyle="DropDownList" RightToLeft="True" SelectedIndex="0" TextField="TpName" ValueField="TpNo" Width="170px">
                                        <ClientSideEvents GotFocus="function(s, e) {  }" SelectedIndexChanged="function(s, e) {
                                                    cbp.PerformCallback('ExRate');  }" />
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td>
                                    <dx:ASPxCheckBox ID="Exch" runat="server" AccessibilityLabelText="" Checked="True" CheckState="Checked" ClientInstanceName="Exch" Text="تحويل بسعر الصرف">
                                        <ClientSideEvents ValueChanged="function(s, e) {
                                                    cbp.PerformCallback('Ex');  }" />
                                    </dx:ASPxCheckBox>
                                </td>
                                <td class="dx-al">سعر الصرف</td>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="Exchange" ClientInstanceName="Exchange" runat="server" Width="170px" Text="1">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2">
                                    <asp:SqlDataSource ID="Accds" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        SelectCommand="select rtrim(AccountNo) As AccountNo, replace([AccountNo],'.','')+' - '+rtrim(AccountName) As AccountName From Accounts WHERE AccountNo NOT IN  (SELECT ISNULL(ParentAcc ,'') FROM Accounts) and Level>=4">
                                        <SelectParameters>
                                            <asp:SessionParameter DefaultValue="1" Name="Br" SessionField="BranchAcc" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <div id="Payment" runat="server">
                                        <table style="width: 100%;" dir="rtl">
                                            <tr>
                                                <td class="dx-al" colspan="1">يدفع إلى</td>
                                                <td class="dx-al">
                                                    <dx:ASPxTextBox ID="PayedFor" runat="server" Width="100%">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="dx-al" colspan="1">مبلغ وقدره
                                                </td>
                                                <td class="dx-al">
                                                    <dx:ASPxTextBox ID="PayedValue" runat="server" Width="170px" DisplayFormatString="n3">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">شرح القيد</td>
                                <td colspan="8">
                                    <%--<dx:ASPxTextBox ID="Comment" runat="server" Width="100%" RightToLeft="True" HorizontalAlign="Right" AutoResizeWithContainer="true" ></dx:ASPxTextBox >--%>
                                    <%--<dx:ASPxMemo ID="Comment1" runat="server" Height="70px" Width="100%"
                                </dx:ASPxMemo>>--%>
                                    <dx:ASPxMemo ID="Comment" runat="server" ClientInstanceName="SettelementDesc" Height="70px"
                                        Width="100%">
                                        <ValidationSettings SetFocusOnError="True" ValidationGroup="groupTabMain">
                                            <RequiredField ErrorText="شرح القيد مطلوب" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxMemo>
                                </td>
                            </tr>
                            <tr>
                                <td class="dx-al">
                                    <dx:ASPxLabel ID="Label1" runat="server" ClientVisible="False">
                                    </dx:ASPxLabel>
                                </td>
                                <td colspan="3"><strong>رقم الحساب/ Account Number</strong></td>
                                <td class="dxeICC"><strong>مدين / Debit&nbsp; </strong></td>
                                <td>
                                    <p class="dxeICC">
                                        <strong>دائن / Credit</strong>
                                    </p>
                                </td>
                                <td class="dxeCaptionHACSys">
                                    <strong>ملاحظات</strong></td>
                                <td>
                                    <dx:ASPxTextBox ID="Csum" runat="server" ClientVisible="False" Width="170px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="Dsum" runat="server" ClientVisible="False" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="idx" runat="server" ClientInstanceName="idx" ClientVisible="False" Width="20px">
                                    </dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="GroupNo" runat="server" ClientInstanceName="GroupNo" ClientVisible="False" Width="20px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="3" class="auto-style1">
                                    <%--  OnSelectedIndexChanged="AccNtnum_SelectedIndexChanged"--%>
                                    <dx:ASPxComboBox ID="AccNtnum" runat="server" CallbackPageSize="20" ClientInstanceName="AccNtnum"
                                        DropDownStyle="DropDownList"
                                        IncrementalFilteringMode="Contains" RightToLeft="True"
                                        TextField="AccountName"
                                        ValueField="AccountNo" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="AccChange" />
                                        <%-- <ValidationSettings Display="Dynamic" ValidationGroup="Data">
                                        <RequiredField ErrorText="رقم الحساب مطلوب" IsRequired="false" />
                                    </ValidationSettings>--%>
                                    </dx:ASPxComboBox>
                                </td>
                                <td dir="ltr" class="auto-style1">
                                    <dx:ASPxTextBox ID="Debtor" runat="server" Width="170px" ClientInstanceName="Debtor"
                                        DisplayFormatString="n3" Text="0">
                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]{1,11}(?:\.[0-9]{1,3})?$" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td dir="ltr" class="auto-style1">
                                    <dx:ASPxTextBox ID="Creditor" runat="server" Width="170px" ClientInstanceName="Creditor"
                                        DisplayFormatString="n3" Text="0">
                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidationGroup="Data">
                                            <RegularExpression ErrorText="أرقام فقط" ValidationExpression="^[0-9]{1,11}(?:\.[0-9]{1,3})?$" />
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">
                                    <dx:ASPxTextBox ID="Note" runat="server" ClientInstanceName="Note" Text="/" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style1">
                                    <dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="btnShow" Text="إضافة" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="SaveData" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td colspan="3">
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="False"
                                        OnDeleted="SqlDataSource1_Deleted"
                                        ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                        ProviderName="<%$ ConnectionStrings:IMSAccConnectionString.ProviderName %>"
                                        SelectCommand="SELECT MainJournal.DAILYNUM, MainJournal.DAILYDTE, MainJournal.DAILYSRL, MainJournal.ANALSNUM, MainJournal.DailyPrv, MainJournal.DailyChk,
                                            MainJournal.DailyTyp, MainJournal.CurUser, MainJournal.UpUser, MainJournal.Comment, replace(Journal.AccountNo,'.','') As AccountNo, Journal.AccountNo As RealAccountNo, Journal.CustName,Journal.idx,
                                            Journal.Dr, abs(Journal.cr) As Cr, Journal.Note,
                                            Journal.GroupNo, Journal.DocNum, Accounts.ParentAcc, Accounts.AccountName
                                            FROM Accounts left outer JOIN Journal ON rtrim(Accounts.AccountNo) = rtrim(Journal.AccountNo)
                                            left outer JOIN MainJournal ON Journal.DAILYNUM = MainJournal.DAILYNUM  and MainJournal.dailytyp=Journal.TP
                                                WHERE ((Journal.DAILYNUM = @Daily)) and MainJournal.dailytyp=@typ
                                            order by GroupNo"
                                        DeleteCommand="DELETE FROM Journal WHERE (idx = @Idx)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="DailyNum" PropertyName="Text" DefaultValue="0" Name="Daily"></asp:ControlParameter>
                                            <asp:QueryStringParameter QueryStringField="Sys" DefaultValue="0" Name="typ"></asp:QueryStringParameter>
                                        </SelectParameters>
                                        <DeleteParameters>
                                            <asp:Parameter Name="Idx" Type="Int32" />
                                        </DeleteParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td dir="ltr">
                                    <dx:ASPxTextBox ID="DocNum" runat="server" Caption="رقم الصك" ClientInstanceName="DocNum" ClientVisible="False" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td dir="ltr" colspan="3">
                                    <dx:ASPxTextBox ID="CustName" runat="server" Caption="باسم" ClientInstanceName="CustName" ClientVisible="False" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td class="dxeCaptionHACSys" colspan="2">
                                    <dx:ASPxLabel ID="Errlbl" runat="server" CssClass="auto-style2" ForeColor="Red">
                                    </dx:ASPxLabel>
                                </td>
                                <td class="dxeCaptionHACSys" colspan="2">
                                    <dx:ASPxLabel ID="DiffernceLbl" runat="server" CssClass="auto-style2" ForeColor="Red" Visible="False">
                                    </dx:ASPxLabel>
                                </td>
                                <td colspan="3" dir="ltr">
                                    <dx:ASPxButton ID="btnIssue" runat="server" AutoPostBack="False" ClientInstanceName="btnIssue" ClientVisible="False" Text="إصدار القيد" UseSubmitBehavior="False" Width="100%">
                                        <ClientSideEvents Click="IssueDoc" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <dx:ASPxGridView ID="GridView1" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                                        DataSourceID="SqlDataSource1" KeyFieldName="idx" OnCustomCallback="GridView1_CustomCallback"
                                        OnDataBinding="GridView1_DataBinding"
                                        OnRowDeleted="GridView1_RowDeleted1"
                                        OnRowUpdated="GridView1_RowUpdated"
                                        OnRowInserted="GridView1_RowInserted"
                                        Width="100%">
                                        <ClientSideEvents EndCallback="onEndCallback" />
                                        <SettingsPager PageSize="20">
                                        </SettingsPager>
                                        <Settings ShowFooter="True" />
                                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                        <SettingsCommandButton>
                                            <DeleteButton ButtonType="Image" RenderMode="Image" Text="شطب الصف ">
                                                <Image Url="../Content/Images/Delete.png">
                                                </Image>
                                            </DeleteButton>
                                        </SettingsCommandButton>
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False">
                                            </FilterControl>
                                        </SettingsPopup>
                                        <SettingsSearchPanel Visible="True" />
                                        <SettingsLoadingPanel Mode="Disabled" />
                                        <SettingsText ConfirmDelete="سيتم شطب هذا الصف، هل أنت متأكد؟" />
                                        <Columns>
                                            <dx:GridViewCommandColumn ButtonRenderMode="Image" ButtonType="Image" Name="Commands" ShowDeleteButton="True" ShowEditButton="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="Edit" Text="تحرير">
                                                        <Image Url="~/Content/Images/Edit.png">
                                                        </Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn FieldName="idx" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="GroupNo" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="DAILYNUM" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم الحساب" FieldName="AccountNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Width="10%">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="اسم الحساب" FieldName="AccountName" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Width="35%">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="مدين" FieldName="Dr" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="6">
                                                <PropertiesTextEdit DisplayFormatString="n3">
                                                </PropertiesTextEdit>
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="دائن" FieldName="Cr" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="7">
                                                <PropertiesTextEdit DisplayFormatString="n3">
                                                </PropertiesTextEdit>
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم الصك" FieldName="DocNum" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="8" Width="10%">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="بأسم" FieldName="CustName" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="9" Width="10%">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ملاحظة" FieldName="Note" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="10">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="رقم الحساب" FieldName="RealAccountNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Visible="false">
                                                <EditFormSettings Visible="False" />
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="Dr" ShowInColumn="Dr" ShowInGroupFooterColumn="مدين" SummaryType="Sum" Tag="DBT" ValueDisplayFormat="n3" />
                                            <dx:ASPxSummaryItem DisplayFormat="n3" FieldName="Cr" ShowInColumn="Cr" ShowInGroupFooterColumn="دائن" SummaryType="Sum" Tag="CRD" ValueDisplayFormat="n3" />
                                        </TotalSummary>
                                    </dx:ASPxGridView>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>