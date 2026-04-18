<%@ Page Language="VB" AutoEventWireup="false" Inherits="ClaimsManage_Sattlments_Settlement" Codebehind="Settlement.aspx.vb" %>


<%@ Register src="../MainClaimData.ascx" tagname="MainClaimData" tagprefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        <div>

            <uc1:MainClaimData ID="MainClaimData1" runat="server" />

            <dx:ASPxGridView ID="grid" runat="server" DataSourceID="DataSource" AutoGenerateColumns="False" OnRowUpdating="grid_RowUpdating"
                Width="100%" OnRowInserting="grid_RowInserting" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" KeyFieldName="ClmNo;No;TPID" RightToLeft="True" >
                <SettingsEditing NewItemRowPosition="Bottom" />
                <Columns>
                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
                        <NewButton Visible="True"></NewButton>

                        <DeleteButton Visible="True"></DeleteButton>

                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="ClmNo" VisibleIndex="0" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="No" VisibleIndex="1" Caption="تسوية رقم">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SettelementDesc" VisibleIndex="2" Caption="وصف التسوية">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ThirdParty" VisibleIndex="3" Caption="المستفيد">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Total" VisibleIndex="4" ReadOnly="True" Caption="إجمالي التسوية">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsPager Mode="ShowAllRecords" ShowDefaultImages="False">
                    <AllButton Text="All"></AllButton>

                    <NextPageButton Text="Next &gt;"></NextPageButton>

                    <PrevPageButton Text="&lt; Prev"></PrevPageButton>
                </SettingsPager>

                <Images SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css">
                    <LoadingPanelOnStatusBar Url="~/App_Themes/PlasticBlue/GridView/gvLoadingOnStatusBar.gif"></LoadingPanelOnStatusBar>

                    <LoadingPanel Url="~/App_Themes/PlasticBlue/GridView/Loading.gif"></LoadingPanel>
                </Images>

                <ImagesFilterControl>
                    <LoadingPanel Url="~/App_Themes/PlasticBlue/Editors/Loading.gif"></LoadingPanel>
                </ImagesFilterControl>

                <Styles CssPostfix="PlasticBlue" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css">
                    <Header SortingImageSpacing="10px" ImageSpacing="10px"></Header>
                </Styles>

                <StylesEditors>
                    <CalendarHeader Spacing="11px"></CalendarHeader>

                    <ProgressBar Height="25px"></ProgressBar>
                </StylesEditors>

                <Templates>
                    <EditForm>
                        <div style="padding: 4px 3px 4px">
                            <dx:ASPxPageControl runat="server" ID="pageControl" Width="100%" ActiveTabIndex="0" CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange" SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css" TabSpacing="2px">
                                <TabPages>
                                 <dx:TabPage Text="تفاصيل التسوية" Visible="true">
                                        <ContentCollection>
                                            <dx:ContentControl runat="server">
                                                <dx:ASPxGridView ID="detailGrid" runat="server" KeyFieldName="ClmNo;No;TPID"
                                                    Width="100%" EnablePagingGestures="False" OnBeforePerformDataSelect="detailGrid_DataSelect"
                                                    OnCustomUnboundColumnData="detailGrid_CustomUnboundColumnData" AutoGenerateColumns="True" DataSourceID="SqldetailsData">
                                                     <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="No" VisibleIndex="0" Caption="رقم التسوية" Visible="false"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="PaymentDesciption" VisibleIndex="1" Caption="البند"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="2" Caption="القيمة"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ClmNo" VisibleIndex="3" Visible="false">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="TPID" VisibleIndex="4" Visible="false">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <Settings ShowFooter="True" />

                                                    <Styles Header-Wrap="True" />
                                                    <TotalSummary>
                                                        <dx:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                                                    </TotalSummary>
 <Templates>
                    <EditForm>
                        <div style="padding: 4px 3px 4px">
                            <dx:ASPxPageControl runat="server" ID="pageControl" Width="100%" ActiveTabIndex="0" CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange" SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css" TabSpacing="2px">
                                <TabPages>
                                    <dx:TabPage Text="تفاصيل التسوية" Visible="true">
                                        <ContentCollection>
                                            <dx:ContentControl runat="server">
                                                <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors" runat="server"></dx:ASPxGridViewTemplateReplacement>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>

                                </TabPages>
                                <LoadingPanelImage Url="~/App_Themes/SoftOrange/Web/Loading.gif"></LoadingPanelImage>

                                <Paddings Padding="4px"></Paddings>

                                <ContentStyle>
                                    <Border BorderColor="LightGray" BorderStyle="Solid" BorderWidth="3px"></Border>
                                </ContentStyle>
                            </dx:ASPxPageControl>
                        </div>
                        <div style="text-align: right; padding: 2px">
                            <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                runat="server"></dx:ASPxGridViewTemplateReplacement>
                            <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                runat="server"></dx:ASPxGridViewTemplateReplacement>
                        </div>
                    </EditForm>
                </Templates>
                                                </dx:ASPxGridView>
                <asp:SqlDataSource runat="server" ID="SqldetailsData" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>' 
                  SelectCommand="SELECT No, PaymentDesciption, Value, ClmNo, TPID FROM DetailSettelement"></asp:SqlDataSource>
                  </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                                              <dx:TabPage Text="سداد" Visible="true">
                                        <ContentCollection>
                                            <dx:ContentControl runat="server">
                                                <%--<dx:ASPxMemo runat="server" ID="notesEditor" Text='<%# Eval("PayTo")%>' Width="100%" Height="93px" />--%>
                                                <dx:ASPxComboBox ID="DailyNo" runat="server" EnableIncrementalFiltering="True" IncrementalFilteringMode="Contains" Width="100%" DataSourceID="SqlDataSource" TextField="Comment" ValueField="DAILYNUM" RightToLeft="True">
                                                   <Columns>
                                            <dx:ListBoxColumn FieldName="DAILYNUM" Width="10px" />
                                            <dx:ListBoxColumn FieldName="Comment" />
                                        </Columns>
                                                 </dx:ASPxComboBox>
                                                <asp:SqlDataSource runat="server" ID="SqlDataSource" ConnectionString='<%$ ConnectionStrings:AccConnectionString %>' 
                                                    SelectCommand="SELECT rtrim(DAILYNUM) As DAILYNUM, rtrim(Comment) as Comment FROM DAILYTAB1 WHERE (DailyTyp = 3) 
                                                    AND (DailyPrv = 1) AND (DAILYNUM NOT IN (SELECT DAILYNUM FROM TakafulDB.dbo.MainSattelement))">
                                                </asp:SqlDataSource>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                </TabPages>
                                <LoadingPanelImage Url="~/App_Themes/SoftOrange/Web/Loading.gif"></LoadingPanelImage>

                                <Paddings Padding="4px"></Paddings>

                                <ContentStyle>
                                    <Border BorderColor="LightGray" BorderStyle="Solid" BorderWidth="3px"></Border>
                                </ContentStyle>
                            </dx:ASPxPageControl>
                        </div>
                        <div style="text-align: right; padding: 2px">
                            <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                runat="server"></dx:ASPxGridViewTemplateReplacement>
                            <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                runat="server"></dx:ASPxGridViewTemplateReplacement>
                        </div>
                    </EditForm>
                </Templates>
            </dx:ASPxGridView>

            <dx:ASPxTextBox runat="server" Width="80px" ClientInstanceName="wakala" ID="wakala">
<ValidationSettings SetFocusOnError="True">
<RequiredField IsRequired="True" ErrorText="نسبة القبول"></RequiredField>
</ValidationSettings>
</dx:ASPxTextBox>

            <br />
            <asp:SqlDataSource ID="DataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                SelectCommand="SELECT        MainSattelement.TPID, MainSattelement.ClmNo, MainSattelement.No, SUM(DetailSettelement.Value) AS Total, MainSattelement.DAILYNUM, MainSattelement.SettelementDesc, ThirdParty.ThirdParty
FROM            MainSattelement INNER JOIN
                         DetailSettelement ON DetailSettelement.ClmNo = MainSattelement.ClmNo AND DetailSettelement.TPID = MainSattelement.TPID AND DetailSettelement.No = MainSattelement.No INNER JOIN
                         ThirdParty ON MainSattelement.ClmNo = ThirdParty.ClmNo AND MainSattelement.TPID = ThirdParty.TPID
WHERE        (ThirdParty.ClmNo = @ClmNo) AND (ThirdParty.TPID = @TPID)
GROUP BY MainSattelement.DAILYNUM, MainSattelement.SettelementDesc, ThirdParty.ThirdParty, MainSattelement.No, MainSattelement.ClmNo, MainSattelement.TPID">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="ClmNo" DefaultValue="0" Name="ClmNo"></asp:QueryStringParameter>
                    <asp:QueryStringParameter QueryStringField="TPID" DefaultValue="0" Name="TPID"></asp:QueryStringParameter>
                </SelectParameters>
            </asp:SqlDataSource>

        </div>
    </form>
</body>
</html>
