<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="testingnewcustomer.aspx.vb" Inherits="testingnewcustomer" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v21.2, Version=21.2.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>

<!DOCTYPE html>

<!DOCTYPE html>
<head runat="server">
    <title></title>
    <script type="text/javascript">
         function MainDataValidation(s, e) {
            var selected = e.value;
            //alert(selected);
            console.log("Selected value = " + selected);
            if (selected == null || selected == "") {
                e.isValid = false;
            }
            else {
                e.isValid = true;
                //alert(selected)
            }
        }
        function onInitlkupPartnerGroup(s, e) { }
        function onGotFocuslkupPartnerGroup(s, e) { }
        function OnPartnerGroupTextChanged(s, e) { }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:aspxcallbackpanel id="callbackPanel" runat="server" clientinstancename="cbName">
                <panelcollection>
                    <dx:panelcontent>
                        <table style="width: 100%;" dir="rtl">
                            <tr>
                                <td>
                                    <dx:aspxgridlookup id="Customer" runat="server" autogeneratecolumns="False" caption="المؤمن له"
                                        clientinstancename="lookupCustomerGroup" datasourceid="SqlDataSource" keyfieldname="CustNo" righttoleft="True" textformatstring="{2}" width="500px">
                                        <gridviewproperties>
                                            <settingsbehavior allowfocusedrow="True" allowselectsinglerowonly="True" enablerowhottrack="True" />
                                            <settingspopup>
                                                <filtercontrol autoupdateposition="False">
                                                </filtercontrol>
                                            </settingspopup>
                                            <settingscommandbutton>
                                                <newbutton text="زبون جديد">
                                                </newbutton>
                                            </settingscommandbutton>
                                        </gridviewproperties>
                                        <columns>
                                            <dx:gridviewcommandcolumn showincustomizationform="True" shownewbutton="True" visibleindex="0">
                                            </dx:gridviewcommandcolumn>
                                            <dx:gridviewdatatextcolumn fieldname="AutoKey" readonly="True" showincustomizationform="True" visible="False" visibleindex="0">
                                                <editformsettings visible="False" />
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn fieldname="CustNo" readonly="True" showincustomizationform="True" visible="False" visibleindex="1">
                                                <editformsettings visible="False" />
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="اسم الزبون" fieldname="CustName" showincustomizationform="True" visibleindex="2">
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="الاسم اللاتيني" fieldname="CustNameE" showincustomizationform="True" visibleindex="3">
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="رقم الهاتف" fieldname="TelNo" showincustomizationform="True" visibleindex="4">
                                            </dx:gridviewdatatextcolumn>
                                            <dx:gridviewdatatextcolumn caption="العنوان" fieldname="Address" showincustomizationform="True" visibleindex="5">
                                            </dx:gridviewdatatextcolumn>
                                        </columns>
                                        <gridviewstyles>
                                            <rowhottrack backcolor="#0066CC">
                                            </rowhottrack>
                                        </gridviewstyles>
                                        <clientsideevents gotfocus="onGotFocuslkupPartnerGroup" init="onInitlkupPartnerGroup" textchanged="function(s, e) { OnPartnerGroupTextChanged(s, e); }"
                                            validation="MainDataValidation" />
                                        <clearbutton displaymode="Always">
                                        </clearbutton>
                                        <validationsettings display="Dynamic" errortext="مطلوب." setfocusonerror="True" validationgroup="validationCustomer">
                                        </validationsettings>
                                    </dx:aspxgridlookup>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:aspxbutton id="ASPxButton2" runat="server" autopostback="False" righttoleft="True" text="Save">
                                        <clientsideevents click="function(s, e){
                                                lookupCustomerGroup.Validate();
                                            }" />
                                    </dx:aspxbutton>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </dx:panelcontent>
                </panelcollection>
            </dx:aspxcallbackpanel>
            <asp:SqlDataSource runat="server" ID="SqlDataSource" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                SelectCommand="SELECT [AutoKey],[CustNo], rtrim([CustName]) As CustName, [CustNameE], [TelNo],[Email], [Address] ,[SpecialCase], [AccNo] FROM [CustomerFile]"
                InsertCommand="NewCustomer"
                InsertCommandType="StoredProcedure">
                <InsertParameters>
                    <asp:Parameter Name="CustName" Type="String"></asp:Parameter>
                    <asp:Parameter Name="CustNameE" Type="String"></asp:Parameter>
                    <asp:Parameter Name="TelNo" Type="String"></asp:Parameter>
                    <asp:Parameter Name="Address" Type="String"></asp:Parameter>
                </InsertParameters>
            </asp:SqlDataSource>
            <br />
            <dx:aspxdateedit runat="server" id="ASPxFormLayout_ORIG_BEGIN_DATE" clientinstancename="OriginalBeginDate">
                <clientsideevents datechanged="OnDateChanged" />
            </dx:aspxdateedit>

            <dx:aspxdateedit runat="server" id="ASPxFormLayout_ORIG_END_DATE" clientinstancename="OriginalEndDate">
                <clientsideevents datechanged="OnDateChanged" />
            </dx:aspxdateedit>

            <dx:aspxspinedit runat="server" number="0" height="21px" id="ASPxFormLayout_ORIG_TERM_YEARS"
                clientinstancename="OriginalTermYears">
            </dx:aspxspinedit>
            <br />
            <br />
        </div>
    </form>
</body>