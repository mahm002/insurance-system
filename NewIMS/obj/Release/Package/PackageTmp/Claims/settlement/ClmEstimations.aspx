<%@ Page Language="VB" AutoEventWireup="false" Inherits="ClaimsManage_settlement_ClmEstimations" CodeBehind="ClmEstimations.aspx.vb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--OnRowDeleting="Grid_RowDeleting" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" --%>
            <asp:SqlDataSource ID="Garages" runat="server"
                ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>" ProviderName="<%$ ConnectionStrings:IMSDBConnectionString.ProviderName %>"
                SelectCommand="Select * from ExtraInfo where Tp='Garages'"></asp:SqlDataSource>
            <dx:ASPxHyperLink ID="ASPxHyperLink" ImageUrl="~/images/app2.png" NavigateUrl="~/SystemManage/flags/flag.aspx?extra=;'Garages' or" Text="متفرقات" EncodeHtml="false" runat="server"></dx:ASPxHyperLink>
            <dx:ASPxGridView ID="masterGrid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataMain" ClientInstanceName="masterGrid" KeyFieldName="Id"
                Width="100%" RightToLeft="True"
                CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue">
                <Columns>
                    <dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0" ShowEditButton="true" ShowNewButton="true" ShowDeleteButton="true">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="ClmNo" VisibleIndex="0" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataComboBoxColumn Caption="الورشة" CellStyle-Wrap="False" EditFormSettings-Caption="الورشة" FieldName="GarageRef" VisibleIndex="1">
                        <PropertiesComboBox ValueField="TPNo" DataSourceID="Garages" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains" TextField="TpName" ValueType="System.Int32"
                            TextFormatString="{0} {1}" EnableCallbackMode="true" CallbackPageSize="10">
                            <Columns>
                                <dx:ListBoxColumn FieldName="TpNo" Width="20px" />
                                <dx:ListBoxColumn FieldName="TpName" Width="100px" />
                            </Columns>
                        </PropertiesComboBox>
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataTextColumn FieldName="EstimatedRep" VisibleIndex="2" Caption="تقدير اليد العاملة">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="EstimatedSpare" VisibleIndex="3" Caption="تقدير قطع الغيار">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Total" VisibleIndex="4" Caption="المجموع">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="0" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="GarageRef" VisibleIndex="0" Visible="false">
                    </dx:GridViewDataTextColumn>
                </Columns>
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
                <Styles CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue">
                    <Header ImageSpacing="10px" SortingImageSpacing="10px">
                    </Header>
                </Styles>
                <StylesEditors>
                    <CalendarHeader Spacing="11px">
                    </CalendarHeader>
                    <ProgressBar Height="25px">
                    </ProgressBar>
                </StylesEditors>
                <SettingsPager ShowDefaultImages="False">
                    <AllButton Text="All">
                    </AllButton>
                    <NextPageButton Text="Next &gt;">
                    </NextPageButton>
                    <PrevPageButton Text="&lt; Prev">
                    </PrevPageButton>
                </SettingsPager>
                <SettingsDetail ShowDetailRow="True" />
            </dx:ASPxGridView>
            <br />
            <asp:SqlDataSource ID="SqlDataMain" runat="server" ConnectionString="<%$ ConnectionStrings:IMSDBConnectionString %>"
                SelectCommand="SELECT ClmNo,TPName, GarageRef,EstimatedRep,EstimatedSpare,Total,Id
                           FROM  MoClaimFile left outer join
                           EXTRAINFO on MoClaimFile.GarageRef=EXTRAINFO.TPNo and EXTRAINFO.tp='Garages'
                           WHERE (MoClaimFile.ClmNo = @ClmNo) AND (MoClaimFile.TPID = @TPID) AND (MoClaimFile.No = @No) and EXTRAINFO.TPName is not null"
                InsertCommand="INSERT INTO MoClaimFile (ClmNo, TPID, No, GarageRef, EstimatedSpare, EstimatedRep, Total) VALUES (@ClmNo, @TPID, @No, @GarageRef, @EstimatedSpare, @EstimatedRep, @EstimatedSpare + @EstimatedRep)"
                UpdateCommand="UPDATE MoClaimFile
                            SET ClmNo=@ClmNo,TPID = @TPID ,GarageRef =@GarageRef,EstimatedSpare = @EstimatedSpare ,EstimatedRep = @EstimatedRep ,Total = @EstimatedSpare + @EstimatedRep
                            WHERE Id=@Id and ClmNo=@ClmNo AND TPID = @TPID AND No = @No" 
                DeleteCommand="Delete MoClaimFile where Id=@Id">
                <SelectParameters>
                    <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                    <asp:QueryStringParameter DefaultValue="0" Name="TPID" QueryStringField="TPID" />
                    <asp:QueryStringParameter DefaultValue="1" Name="No" QueryStringField="No" />
                </SelectParameters>
                <InsertParameters>
                    <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                    <asp:QueryStringParameter DefaultValue="0" Name="TPID" QueryStringField="TPID" />
                    <asp:QueryStringParameter DefaultValue="1" Name="No" QueryStringField="No" />
                    <asp:Parameter Name="GarageRef" Type="Int32" />
                    <asp:Parameter Name="EstimatedSpare" Type="Double" />
                    <asp:Parameter Name="EstimatedRep" Type="Double" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:QueryStringParameter DefaultValue="0" Name="ClmNo" QueryStringField="ClmNo" />
                    <asp:QueryStringParameter DefaultValue="0" Name="TPID" QueryStringField="TPID" />
                    <asp:QueryStringParameter DefaultValue="1" Name="No" QueryStringField="No" />
                    <asp:Parameter Name="GarageRef" Type="Int32" />
                    <asp:Parameter Name="EstimatedSpare" Type="Double" />
                    <asp:Parameter Name="EstimatedRep" Type="Double" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="Id" Type="Int32" />
                </DeleteParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>