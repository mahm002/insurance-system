<%@ Page Language="VB" AutoEventWireup="false" Inherits="Settle" CodeBehind="Settle.aspx.vb" %>

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
                //str += '<b>Date Info:</b><br />' + getShortDate(deAnyDate.GetValue().toString()) + '<hr />';
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
        function ReturnToParentPage() {
            //debugger;
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup();
            //var t = ExtPrm.GetValue();
            //window.parent.document.getElementById("EXTPRM").value = t;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div dir="rtl">
            <dx:ASPxLabel runat="server" ID="NoLbl" ClientVisible="false"></dx:ASPxLabel>
            <dx:ASPxLabel runat="server" ID="TPidLbl" ClientVisible="false"></dx:ASPxLabel>
            <dx:ASPxPageControl ID="pageControl" ClientInstanceName="pageControl" runat="server"
                ActiveTabIndex="0" EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue"
                SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css" RightToLeft="True" Width="100%">
                <LoadingPanelImage Url="~/App_Themes/PlasticBlue/Web/tcLoading.gif">
                </LoadingPanelImage>
                <ClientSideEvents ActiveTabChanging="OnTabChanging" />
                <TabPages>
                    <dx:TabPage Name="TabMain" Text="البيانات الرئيسية للتسوية">
                        <ContentCollection>
                            <dx:ContentControl runat="server">
                                <br />
                                <dx:ASPxLabel ID="lblTPID" runat="server" Text="طرف التسوية">
                                </dx:ASPxLabel>
                                <dx:ASPxComboBox ID="TPID" runat="server" ValueField="TPID" TextField="ThirdParty" ValueType="System.String" ClientInstanceName="TPID" DataSourceID="SqlDataTP" Width="100%">
                                    <ValidationSettings ValidationGroup="groupTabMain" ValidateOnLeave="true" SetFocusOnError="true">
                                        <RequiredField IsRequired="true" ErrorText="طرف التسوية مطلوب" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                                <asp:SqlDataSource runat="server" ID="SqlDataTP" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                                    SelectCommand="SELECT TPID, rtrim(ThirdParty) as ThirdParty FROM ThirdParty WHERE (ClmNo = @ClmNo) AND (TPID=@TPID)">
                                    <SelectParameters>
                                        <asp:QueryStringParameter QueryStringField="ClmNo" DefaultValue="0" Name="ClmNo"></asp:QueryStringParameter>
                                        <asp:QueryStringParameter QueryStringField="TPID" DefaultValue="0" Name="TPID"></asp:QueryStringParameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <hr />
                                <dx:ASPxLabel ID="lblPayto" runat="server" Text="المستفيد">
                                </dx:ASPxLabel>
                                / المسند له جبر الضرر<dx:ASPxTextBox ID="PayTo" runat="server" Width="100%" ClientInstanceName="PayTo">
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
                                <div id="MoDiv" runat="server">

                                    <asp:SqlDataSource ID="SqlDataMain" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" DeleteCommand="Delete MoClaimFile where Id=@Id" InsertCommand="INSERT INTO MoClaimFile (ClmNo, TPID, GarageRef, EstimatedSpare, EstimatedRep, Total) VALUES (@ClmNo, @TPID, @GarageRef, @EstimatedSpare, @EstimatedRep, @EstimatedSpare + @EstimatedRep)" SelectCommand="SELECT ClmNo,TPName, GarageRef,EstimatedRep,EstimatedSpare,Total,Id
                           FROM  MoClaimFile left outer join
                           EXTRAINFO on MoClaimFile.GarageRef=EXTRAINFO.TPNo and EXTRAINFO.tp='Garages'
                           WHERE (MoClaimFile.ClmNo = @ClmNo) AND (MoClaimFile.TPID = @TPID) and EXTRAINFO.TPName is not null"
                                        UpdateCommand="UPDATE MoClaimFile
                            SET ClmNo=@ClmNo,TPID = @TPID ,GarageRef =@GarageRef,EstimatedSpare = @EstimatedSpare ,EstimatedRep = @EstimatedRep ,Total = @EstimatedSpare + @EstimatedRep
                            WHERE Id=@Id and ClmNo=@ClmNo AND TPID = @TPID">
                                        <DeleteParameters>
                                            <asp:Parameter Name="Id" Type="Int32" />
                                        </DeleteParameters>
                                        <InsertParameters>
                                            <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                            <asp:QueryStringParameter DefaultValue="0" Name="TPID" QueryStringField="TPID" />
                                            <asp:Parameter Name="GarageRef" Type="Int32" />
                                            <asp:Parameter Name="EstimatedSpare" Type="Double" />
                                            <asp:Parameter Name="EstimatedRep" Type="Double" />
                                        </InsertParameters>
                                        <SelectParameters>
                                            <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                            <asp:QueryStringParameter DefaultValue="0" Name="TPID" QueryStringField="TPID" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                            <asp:QueryStringParameter DefaultValue="0" Name="TPID" QueryStringField="TPID" />
                                            <asp:Parameter Name="GarageRef" Type="Int32" />
                                            <asp:Parameter Name="EstimatedSpare" Type="Double" />
                                            <asp:Parameter Name="EstimatedRep" Type="Double" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>

                                    <asp:SqlDataSource ID="Garages" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>" SelectCommand="Select * from ExtraInfo where Tp='Garages'"></asp:SqlDataSource>
                                    تقديرات الورش<br />
                                    <dx:ASPxHyperLink ID="ASPxHyperLink" runat="server" EncodeHtml="False" ImageUrl="~/images/app2.png" NavigateUrl="~/SystemManage/flags/flag.aspx?extra=;'Garages' or" Text="متفرقات">
                                    </dx:ASPxHyperLink>
                                    <dx:ASPxGridView ID="masterGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="masterGrid"
                                        CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" DataSourceID="SqlDataMain"
                                        KeyFieldName="Id" RightToLeft="True" Width="100%" SettingsBehavior-ConfirmDelete="true">
                                        <SettingsDetail ShowDetailRow="True" />
                                        <SettingsPager ShowDefaultImages="False">
                                            <AllButton Text="All">
                                            </AllButton>
                                            <NextPageButton Text="Next &gt;">
                                            </NextPageButton>
                                            <PrevPageButton Text="&lt; Prev">
                                            </PrevPageButton>
                                        </SettingsPager>
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False">
                                            </FilterControl>
                                        </SettingsPopup>
                                        <StylesEditors>
                                            <CalendarHeader Spacing="11px">
                                            </CalendarHeader>
                                            <ProgressBar Height="25px">
                                            </ProgressBar>
                                        </StylesEditors>
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowInCustomizationForm="True" ShowNewButton="True" VisibleIndex="0">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn FieldName="ClmNo" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataComboBoxColumn Caption="الورشة" FieldName="GarageRef" ShowInCustomizationForm="True" VisibleIndex="1">
                                                <PropertiesComboBox CallbackPageSize="10" DataSourceID="Garages" EnableCallbackMode="True" TextField="TpName" TextFormatString="{0} {1}" ValueField="TPNo" ValueType="System.Int32">
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="TpNo" Width="20px">
                                                        </dx:ListBoxColumn>
                                                        <dx:ListBoxColumn FieldName="TpName" Width="100px">
                                                        </dx:ListBoxColumn>
                                                    </Columns>
                                                </PropertiesComboBox>
                                                <EditFormSettings Caption="الورشة" />
                                                <CellStyle Wrap="False">
                                                </CellStyle>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataTextColumn Caption="تقدير اليد العاملة" FieldName="EstimatedRep" ShowInCustomizationForm="True" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="تقدير قطع الغيار" FieldName="EstimatedSpare" ShowInCustomizationForm="True" VisibleIndex="3">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="المجموع" FieldName="Total" ShowInCustomizationForm="True" VisibleIndex="4">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="Id" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="GarageRef" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <Styles CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue">
                                            <Header ImageSpacing="10px" SortingImageSpacing="10px">
                                            </Header>
                                        </Styles>
                                        <Images SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css">
                                            <LoadingPanelOnStatusBar Url="~/App_Themes/PlasticBlue/GridView/gvLoadingOnStatusBar.gif">
                                            </LoadingPanelOnStatusBar>
                                            <LoadingPanel Url="~/App_Themes/PlasticBlue/GridView/Loading.gif">
                                            </LoadingPanel>
                                        </Images>
                                        <ImagesFilterControl>
                                            <LoadingPanel Url="~/App_Themes/PlasticBlue/Editors/Loading.gif">
                                            </LoadingPanel>
                                        </ImagesFilterControl>
                                    </dx:ASPxGridView>
                                </div>
                                <hr />
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="تفاصيل التعويض">
                                </dx:ASPxLabel>
                                <dx:ASPxGridView ID="GvDetails" ClientInstanceName="GvDetails" runat="server" DataSourceID="SqlDetailssettl"
                                    AutoGenerateColumns="False" KeyFieldName="Sno" RightToLeft="True" Width="100%" SettingsBehavior-ConfirmDelete="true">

                                    <Settings ShowFooter="True"></Settings>

                                    <SettingsPopup>
                                        <FilterControl AutoUpdatePosition="False"></FilterControl>
                                    </SettingsPopup>

                                    <Columns>
                                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0" ShowEditButton="true" ShowNewButton="true" ShowDeleteButton="true">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="PaymentDesciption" VisibleIndex="1" Caption="البيان" Width="75%"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="2" Caption="القيمة" Width="25%"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Sno" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                    </Columns>

                                    <TotalSummary>
                                        <dx:ASPxSummaryItem ShowInColumn="القيمة" SummaryType="Sum" FieldName="Value" ValueDisplayFormat="n3" DisplayFormat="n3" ShowInGroupFooterColumn="القيمة"></dx:ASPxSummaryItem>
                                    </TotalSummary>
                                </dx:ASPxGridView>
                                <asp:SqlDataSource ID="SqlDetailssettl" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                                    SelectCommand="SELECT No,PaymentDesciption, Value, Sno FROM DetailSettelement
                                                    WHERE ClmNo = @ClmNo AND TPID = @TPID AND No = @No"
                                    InsertCommand="INSERT INTO [dbo].[DetailSettelement]  ([ClmNo] ,[TPID] ,[No] ,[PaymentDesciption],[Value])
                                                VALUES (@ClmNo,@TPID,@No,@PaymentDesciption,@Value)"
                                    UpdateCommand="UPDATE [DetailSettelement]
                                                SET [ClmNo] = @ClmNo ,[TPID] = @TPID ,[No] =@No ,[PaymentDesciption] = @PaymentDesciption,[Value] = @Value
                                                WHERE ClmNo=@ClmNo And TPID=@TPID and No=@No And Sno=@Sno"
                                    DeleteCommand="DELETE FROM [dbo].[DetailSettelement]
                                                WHERE Sno=@Sno">

                                    <SelectParameters>
                                        <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                        <asp:ControlParameter ControlID="TPidLbl" PropertyName="Text" DefaultValue="0" Name="TPID"></asp:ControlParameter>
                                        <asp:ControlParameter ControlID="NoLbl" PropertyName="Text" DefaultValue="0" Name="No" Type="Int32"></asp:ControlParameter>
                                    </SelectParameters>
                                    <InsertParameters>
                                        <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                        <asp:ControlParameter ControlID="TPidLbl" PropertyName="Text" DefaultValue="0" Name="TPID"></asp:ControlParameter>
                                        <asp:ControlParameter ControlID="NoLbl" PropertyName="Text" DefaultValue="0" Name="No" Type="Int32"></asp:ControlParameter>
                                        <asp:Parameter Name="PaymentDesciption" Type="String" />
                                        <asp:Parameter Name="Value" Type="Double" />
                                    </InsertParameters>
                                    <UpdateParameters>
                                        <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                                        <asp:ControlParameter ControlID="TPidLbl" PropertyName="Text" DefaultValue="0" Name="TPID"></asp:ControlParameter>
                                        <asp:ControlParameter ControlID="NoLbl" PropertyName="Text" DefaultValue="0" Name="No" Type="Int32"></asp:ControlParameter>
                                        <asp:Parameter Name="PaymentDesciption" Type="String" />
                                        <asp:Parameter Name="Value" Type="Double" />
                                        <asp:Parameter Name="Sno" Type="Int32" />
                                    </UpdateParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="Sno" Type="Int32" />
                                    </DeleteParameters>
                                </asp:SqlDataSource>

                                <hr />

                                <hr />
                                <dx:ASPxButton ID="btnFinish" runat="server" Text="إنهاء" ValidationGroup="groupTabDetails"
                                    AutoPostBack="false">

                                    <%--<clientsideevents Click="OnFinishClick" />--%>
                                    <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();
}" />
                                </dx:ASPxButton>
                                <dx:ASPxValidationSummary ID="validSummaryDetails" ValidationGroup="groupTabDetails"
                                    runat="server">
                                </dx:ASPxValidationSummary>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
                <Paddings PaddingLeft="5px" PaddingRight="5px" PaddingTop="3px" />
                <ContentStyle>
                    <Border BorderWidth="0px"></Border>
                </ContentStyle>
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