<%@ Page Language="VB" AutoEventWireup="false" Inherits="TakafulyIMS.Default3" Codebehind="Default3.aspx.vb" %>


<%@ Register assembly="DevExpress.Web.v21.2" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register src="../MainClaimData.ascx" tagname="MainClaimData" tagprefix="uc1" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function OnTabChanging(s, e) {
            var tabName = (pageControl.GetActiveTab()).name;
            e.cancel = !ASPxClientEdit.ValidateGroup('group' + tabName);
        }
        function OnButtonClick(s, e) {
            var indexTab = (pageControl.GetActiveTab()).index;
            pageControl.SetActiveTab(pageControl.GetTab(indexTab + 1));

        }
        function OnFinishClick(s, e) {
            if (ASPxClientEdit.ValidateGroup('groupTabDate')) {
                var str = '<b>البيانات الرئيسية:</b><br />' + PayTo.GetValue() + '<br />' + SettelementDesc.GetValue() + '<br />' + TPID.GetValue() + '  ' + TPID.GetText() + '<hr />';
                str += '<b>بيانات تفاصيل التسوية:</b><br />' + Paymentdescr.GetValue() + '<br />' + txtValue.GetValue() + '<hr />';
                str += '<b>Date Info:</b><br />' + getShortDate(deAnyDate.GetValue().toString()) + '<hr />';
                popupControl.SetContentHtml(str);
                popupControl.ShowAtElement(pageControl.GetMainElement());
                popupControl.UpdatePositionAtElement(pageControl.GetMainElement());//ShowTabs
            }
        }
        function getShortDate(longDate) {
            var date = new Date(longDate);
            var month = date.getMonth() + 1;
            var str = month.toString() + '/' + date.getDate().toString() + '/' + date.getFullYear().toString();
            return str;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div dir="rtl">
                            <dx:ASPxLabel runat="server" ID="NoLbl"></dx:ASPxLabel>

                            <br />
        <dx:ASPxLabel runat="server" ClientVisible="False" ID="TPidLbl">
        </dx:ASPxLabel>
        <br />
        <dx:ASPxPageControl ID="pageControl" ClientInstanceName="pageControl" runat="server"
            ActiveTabIndex="0" EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" 
            SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css" RightToLeft="True"  Width="100%">
            <loadingpanelimage url="~/App_Themes/PlasticBlue/Web/tcLoading.gif">
            </loadingpanelimage>
            <ClientSideEvents ActiveTabChanging="OnTabChanging" />
            <TabPages>
                <dx:TabPage Name="TabMain" Text="البيانات الرئيسية للتسوية">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <br />
                            <dx:ASPxLabel ID="lblTPID" runat="server" Text="طرف التسوية">
                            </dx:ASPxLabel>
                            <dx:ASPxComboBox ID="TPID" runat="server" ValueField="TPID" TextField="ThirdParty" ValueType="System.String" ClientInstanceName="TPID" DataSourceID="SqlDataTP">
                                    <ValidationSettings ValidationGroup="groupTabMain" ValidateOnLeave="true" SetFocusOnError="true">
                                        <RequiredField IsRequired="true" ErrorText="طرف التسوية مطلوب" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                                <asp:SqlDataSource runat="server" ID="SqlDataTP" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>' 
                                    SelectCommand="SELECT TPID, rtrim(ThirdParty) as ThirdParty FROM ThirdParty WHERE (ClmNo = @ClmNo) AND (TPID=@TPID)">
                                    <SelectParameters>
                                        <asp:QueryStringParameter QueryStringField="ClmNo" DefaultValue="0" Name="ClmNo">
                                        </asp:QueryStringParameter>
                                          <asp:QueryStringParameter QueryStringField="TPID" DefaultValue="0" Name="TPID">
                                        </asp:QueryStringParameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <hr />
                         <dx:ASPxLabel ID="lblPayto" runat="server" Text="المستفيد">
                            </dx:ASPxLabel>
                            / المسند له جبر الضرر<dx:ASPxTextBox ID="PayTo" runat="server" Width="170px" ClientInstanceName="PayTo">
                                <ValidationSettings SetFocusOnError="True" ValidationGroup="groupTabMain">
                                    <RequiredField IsRequired="True" ErrorText="المستفيد مطلوب" />
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                            <hr />
                            <dx:ASPxLabel ID="lblDescr" runat="server" Text="وصف التسوية">
                            </dx:ASPxLabel>
                            <dx:ASPxMemo ID="SettelementDesc" runat="server" Native="True" ClientInstanceName="SettelementDesc" Height="70px" Width="100%">
                                <ValidationSettings SetFocusOnError="true" ValidationGroup="groupTabMain">
                                    <RequiredField IsRequired="true" ErrorText="وصف التسوية مطلوب" />
                                </ValidationSettings>
                            </dx:ASPxMemo>
                             <hr />
                            <dx:ASPxButton ID="btnNextMain" runat="server" Text="التالي" ClientInstanceName="btnNextMain" OnClick="btnMainData_Click"
                                AutoPostBack="false" ValidationGroup="groupTabMain">
                                <ClientSideEvents Click="OnButtonClick" />
                            </dx:ASPxButton>
                            <dx:ASPxValidationSummary ID="validSummaryMain" runat="server" ValidationGroup="groupTabMain" RightToLeft="True">
                            </dx:ASPxValidationSummary>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Name="TabDetails" Text=" تفاصيل التسوية">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <dx:ASPxLabel ID="lblDDescribe" runat="server" Text="وصف التعويض">
                            </dx:ASPxLabel>
                            <dx:ASPxTextBox ID="Paymentdescr" runat="server" ClientInstanceName="Paymentdescr" Width="170px">
                                <ValidationSettings SetFocusOnError="True" ValidationGroup="groupTabDetails">
                                    <%--<RegularExpression ErrorText="Invalid E-Mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />--%>
                                    <RequiredField IsRequired="True" ErrorText="E-Mail is required" />
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                            <hr />
                            <dx:ASPxLabel ID="lblValue" runat="server" Text="القيمة">
                            </dx:ASPxLabel>
                            <dx:ASPxTextBox ID="txtValue" runat="server" Width="170px" ClientInstanceName="txtValue">
                                <ValidationSettings SetFocusOnError="True" ValidationGroup="groupTabDetails">
                                    <%--<RegularExpression ValidationExpression="^([+-]{1})([0-9]{3})$" ErrorText="Invalid ZIP Code" />--%>
                                    <RequiredField IsRequired="true" ErrorText="ZIP Code is required" />
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                              <hr />
                            <dx:ASPxButton ID="Add" runat="server" Text="إضافة" ValidationGroup="groupTabDetails"
                                AutoPostBack="false" OnClick="Add_Click">
                            </dx:ASPxButton>
                         
                            <hr />
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="تفاصيل التعويض">
                            </dx:ASPxLabel>
                            <dx:ASPxGridView ID="GvDetails" ClientInstanceName="GvDetails" runat="server" DataSourceID="SqlDetailssettl" colum
                                AutoGenerateColumns="False" KeyFieldName="Sno" RightToLeft="True" Width="100%">

                                <Columns>
                                    <dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0">
                                        <editbutton visible="True" Text="تعديل">
                                        </editbutton>
                                        <newbutton visible="True" Text="جديد">
                                        </newbutton>
                                        <deletebutton visible="True" Text="مسح">
                                        </deletebutton>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn FieldName="PaymentDesciption" VisibleIndex="1" Caption="البيان" Width="75%"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="2" Caption="القيمة" Width="25%"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Sno" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                            <asp:SqlDataSource ID="SqlDetailssettl" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" 
                                SelectCommand="SELECT PaymentDesciption, Value,Sno FROM DetailSettelement WHERE (ClmNo = @ClmNo) AND (TPID = @TPID) AND (No = @No)" 
                                InsertCommand="INSERT INTO [dbo].[DetailSettelement]  ([ClmNo] ,[TPID] ,[No] ,[PaymentDesciption],[Value]) 
                                                VALUES (@ClmNo,@TPID,@No,@PaymentDesciption,@Value)"
                                UpdateCommand="UPDATE [DetailSettelement] 
                                                SET [ClmNo] = @ClmNo ,[TPID] = @TPID ,[No] =@No ,[PaymentDesciption] = @PaymentDesciption,[Value] = @Value
                                                WHERE ClmNo=@ClmNo And TPID=@TPID and No=@No And Sno=@Sno"
                                DeleteCommand="DELETE FROM [dbo].[DetailSettelement]
                                                WHERE ClmNo=@ClmNo And TPID=@TPID and No=@No And Sno=@Sno"
>
                                <DeleteParameters>
                                     <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                    <asp:ControlParameter ControlID="TPidLbl" PropertyName="Text" DefaultValue="0" Name="TPID"></asp:ControlParameter>
                                    <asp:SessionParameter Name="No" SessionField="No" Type="Int32" />
                                    <asp:Parameter Name="PaymentDesciption" Type="String" />            
			                        <asp:Parameter Name="Value" Type="Double" />
                                    <asp:Parameter Name="Sno" Type="Int32" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                    <asp:ControlParameter ControlID="TPidLbl" PropertyName="Text" DefaultValue="0" Name="TPID"></asp:ControlParameter>
                                    <asp:SessionParameter Name="No" SessionField="No" Type="Int32" />
                                    <asp:Parameter Name="PaymentDesciption" Type="String" />            
			                        <asp:Parameter Name="Value" Type="Double" />
                                </InsertParameters>
                                <SelectParameters>
                                    <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                    <asp:ControlParameter ControlID="TPidLbl" PropertyName="Text" DefaultValue="0" Name="TPID"></asp:ControlParameter>
                                     <asp:SessionParameter Name="No" SessionField="No" Type="Int32" />
                                </SelectParameters>
                                <UpdateParameters>
                                    <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                    <asp:ControlParameter ControlID="TPidLbl" PropertyName="Text" DefaultValue="0" Name="TPID"></asp:ControlParameter>
                                     <asp:SessionParameter Name="No" SessionField="No" Type="Int32" />
                                    <asp:Parameter Name="PaymentDesciption" Type="String" />            
			                        <asp:Parameter Name="Value" Type="Double" />
                                    <asp:Parameter Name="Sno" Type="Int32" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
                         
                            <hr />
                          <dx:ASPxButton ID="btnFinish" runat="server" Text="التالي" ValidationGroup="groupTabDetails"
                                AutoPostBack="false">
                         
                              <clientsideevents click="OnButtonClick" />
                         
                            </dx:ASPxButton>
                            <dx:ASPxValidationSummary ID="validSummaryDetails" ValidationGroup="groupTabDetails"
                                runat="server">
                            </dx:ASPxValidationSummary>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Name="TabDate" Text="Date">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <dx:ASPxLabel ID="lblAnyDate" runat="server" Text="AnyDate">
                            </dx:ASPxLabel>
                            <dx:ASPxDateEdit ID="deAnyDate" runat="server" ClientInstanceName="deAnyDate">
                                <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true">
                                    <RequiredField IsRequired="true" ErrorText="Any Date is required" />
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                            <hr />
                            
                            <dx:ASPxButton ID="btnNext" ValidationGroup="groupTabDate" runat="server" Text="إنهاء"
                                ClientInstanceName="btnNextDate" AutoPostBack="false">
                                <ClientSideEvents Click="OnFinishClick" />
                                
                            </dx:ASPxButton>
                            <dx:ASPxValidationSummary ID="validSummaryDate" runat="server" ValidationGroup="groupTabDate">
                            </dx:ASPxValidationSummary>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
            <paddings paddingleft="5px" paddingright="5px" paddingtop="3px" />
            <contentstyle>
                <border borderwidth="0px"></border>
            </contentstyle>
        </dx:ASPxPageControl>
    </div>
    <dx:ASPxPopupControl ID="popupControl" runat="server" CloseAction="CloseButton" ClientInstanceName="popupControl"
        HeaderText="Summary" PopupHorizontalAlign="OutsideRight" PopupHorizontalOffset="10" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css">
        <LoadingPanelImage Url="~/App_Themes/PlasticBlue/Web/dvLoading.gif"></LoadingPanelImage>

        <CloseButtonStyle>
            <Paddings Padding="0px"></Paddings>
        </CloseButtonStyle>

        <ContentStyle>
            <BorderBottom BorderColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px"></BorderBottom>
        </ContentStyle>

        <HeaderStyle>
            <Paddings PaddingLeft="10px" PaddingTop="4px" PaddingRight="4px" PaddingBottom="4px"></Paddings>
        </HeaderStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
        <br />
    </form>
</body>
</html>