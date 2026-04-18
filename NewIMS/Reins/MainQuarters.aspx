<%@ Page Title="" Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="Reins_MainQuarters" CodeBehind="MainQuarters.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script lang="javascript" type="text/javascript">
        function SelectAndClosePopup() {
            popup.Hide();
            MainGrid.Refresh();
            detailGrid.Refresh();
        }
        function ShowPopup(Parameter) {
            //alert(customerID);Height="800" Width="800"
            if (Parameter == 'New') {
                //popup.SetSize(1300, 950);
                PrintPop.HeaderText = 'Quarters Form'
                PrintPop.SetContentUrl('Quarters.aspx');
                PrintPop.Show();
            }
            else {
                alert(Parameter);
                ////popup.SetContentUrl('Reseat.aspx?PolNo=' + customerID);
                //popup.SetSize(1300, 950);
                HeaderText = "Print"
                //popup.SetContentUrl('../OutPutManagement/PreView.aspx?Report=/IMSReports/Soa');
                //popup.Show();
                window.open(unescape(location.pathname).substring(0, unescape(location.pathname).lastIndexOf("/Reins")) + '/Reporting/PreView.aspx?Report=/IMSReports/Soa');
            }
        }
        function puOnInit(s, e) {
            AdjustSize();
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
                if (s.IsVisible())
                    s.UpdatePosition();
            });
        }

        function AdjustSize() {
            var width = Math.max(0, document.documentElement.clientWidth) * 0.98;
            var height = Math.max(0, document.documentElement.clientHeight) * 0.98;
            PrintPop.SetWidth(width);
            PrintPop.SetSize(width, height);
        }
    </script>
    <div>
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <dx:ASPxHyperLink ID="ASPxHyperLink1" ToolTip="New Account Calculate" NavigateUrl="javascript:ShowPopup('New')" ImageUrl="~/images/add.png" runat="server" Text="ASPxHyperLink">
                    </dx:ASPxHyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <dx:ASPxComboBox ID="Years" runat="server" OnSelectedIndexChanged="Years_SelectedIndexChanged"
                        AutoPostBack="false" TextField="Year" EnableIncrementalFiltering="True" IncrementalFilteringMode="StartsWith"
                        MaxLength="3" Width="140px" DataSourceID="YearsSource">
                        <%--<ClientSideEvents SelectedIndexChanged="function(s, e)
                                          { Year.SetValue('');
                                            InsCompany.SetValue('');
                                          }" />--%>
                        <LoadingPanelImage Url="~/App_Themes/DevEx/Editors/Loading.gif"></LoadingPanelImage>

                        <LoadingPanelStyle ImageSpacing="5px"></LoadingPanelStyle>

                        <ButtonStyle Width="13px"></ButtonStyle>
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {MainGrid.PerformCallback();}" />
                    </dx:ASPxComboBox>
                </asp:TableCell>
                <asp:TableCell>
                    <dx:ASPxComboBox ID="Quarters" runat="server" OnSelectedIndexChanged="Quarters_SelectedIndexChanged" AutoPostBack="false"
                        SelectedIndex="0" EnableIncrementalFiltering="True" IncrementalFilteringMode="StartsWith" MaxLength="3" Width="140px">
                        <Items>
                            <dx:ListEditItem Text="Q1" Value="1" />
                            <dx:ListEditItem Text="Q2" Value="2" />
                            <dx:ListEditItem Text="Q3" Value="3" />
                            <dx:ListEditItem Text="Q4" Value="4" />
                        </Items>
                        <%-- <ClientSideEvents SelectedIndexChanged="function(s, e)
                                          { Year.SetValue('');
                                            InsCompany.SetValue('');
                                          }" />--%>
                        <LoadingPanelImage Url="~/App_Themes/DevEx/Editors/Loading.gif"></LoadingPanelImage>

                        <LoadingPanelStyle ImageSpacing="5px"></LoadingPanelStyle>

                        <ButtonStyle Width="13px"></ButtonStyle>
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {MainGrid.PerformCallback();}" />
                    </dx:ASPxComboBox>
                </asp:TableCell>
                <asp:TableCell>
                    <dx:ASPxCheckBox ID="chkSingleExpanded" runat="server" Text="Expand/ Collaps"
                        AutoPostBack="true" OnCheckedChanged="chkSingleExpanded_CheckedChanged" CheckState="checked" CssFilePath="~/App_Themes/DevEx/{0}/styles.css" CssPostfix="DevEx" SpriteCssFilePath="~/App_Themes/DevEx/{0}/sprite.css" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <asp:SqlDataSource runat="server" ID="YearsSource" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
            SelectCommand="SELECT DISTINCT(Year) FROM Soa order by year desc"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource" runat="server"
            ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
            SelectCommand="SELECT DISTINCT Soa.Recom AS Id, EXTRAINFO.TPName AS Reinsurer
            FROM Soa INNER JOIN EXTRAINFO ON Soa.Recom = EXTRAINFO.TPNo
            WHERE (Soa.Year = @Year) AND (EXTRAINFO.TP = 'Recom') AND (Soa.Q = @Q)">
            <SelectParameters>
                <%--             <asp:ControlParameter ControlID="Years" PropertyName="Value" Name="Year" Type="Int32"></asp:ControlParameter>
                <asp:ControlParameter ControlID="Quarters" PropertyName="Value" Name="Q" Type="Int32"></asp:ControlParameter>--%>
                <asp:SessionParameter Name="Year" SessionField="Year" Type="Int32" />
                <asp:SessionParameter Name="Q" SessionField="Q" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <dx:ASPxGridView ID="MainGrid" ClientInstanceName="MainGrid" runat="server" DataSourceID="SqlDataSource" KeyFieldName="Id" Width="100%" AutoGenerateColumns="False"
            CssFilePath="~/App_Themes/DevEx/{0}/styles.css" CssPostfix="DevEx" SettingsPager-PageSize="25">
            <Columns>
                <dx:GridViewDataColumn FieldName="Id" VisibleIndex="0" Visible="false" />
                <dx:GridViewDataColumn FieldName="Reinsurer" VisibleIndex="1" />
            </Columns>

            <StylesEditors ButtonEditCellSpacing="0">
                <ProgressBar Height="21px"></ProgressBar>
            </StylesEditors>
            <Templates>
                <DetailRow>
                    Accounts Ready By Insurance Types for Quarter/
                <dx:ASPxLabel runat="server" Text='<%# Session("Q") %>' Font-Bold="true" />
                    To/
                <dx:ASPxLabel runat="server" Text='<%# Eval("Reinsurer") %>' Font-Bold="true" />
                    <br />
                    <br />
                    <dx:ASPxGridView ID="detailGrid" ClientInstanceName="detailGrid" runat="server" DataSourceID="SqlDetails" KeyFieldName="InsType"
                        Width="100%" OnBeforePerformDataSelect="detailGrid_DataSelect" AutoGenerateColumns="False"
                        CssFilePath="~/App_Themes/Office2003Olive/{0}/styles.css" CssPostfix="Office2003Olive">
                        <Columns>
                            <dx:GridViewDataColumn FieldName="InsType" VisibleIndex="1" />
                            <dx:GridViewDataColumn FieldName="Amount" VisibleIndex="2" />

                            <dx:GridViewDataTextColumn Caption="Print">
                                <DataItemTemplate>
                                    <dx:ASPxHyperLink runat="server" ID="keyFieldLink" OnInit="keyFieldLink_Init"></dx:ASPxHyperLink>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFooter="True" />
                        <TotalSummary>

                            <dx:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                        </TotalSummary>

                        <Images SpriteCssFilePath="~/App_Themes/Office2003Olive/{0}/sprite.css">
                            <LoadingPanelOnStatusBar Url="~/App_Themes/Office2003Olive/GridView/gvLoadingOnStatusBar.gif"></LoadingPanelOnStatusBar>

                            <LoadingPanel Url="~/App_Themes/Office2003Olive/GridView/Loading.gif"></LoadingPanel>
                        </Images>

                        <ImagesFilterControl>
                            <LoadingPanel Url="~/App_Themes/Office2003Olive/Editors/Loading.gif"></LoadingPanel>
                        </ImagesFilterControl>

                        <Styles CssPostfix="Office2003Olive" CssFilePath="~/App_Themes/Office2003Olive/{0}/styles.css">
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                        </Styles>

                        <StylesEditors>

                            <ProgressBar Height="25px"></ProgressBar>
                        </StylesEditors>
                    </dx:ASPxGridView>
                    <asp:SqlDataSource runat="server" ID="SqlDetails" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
                        SelectCommand="SELECT Soa.InsType, EXTRAINFO.TPName, Soa.Q, Sum(Soa.Amount) As Amount
                        FROM Soa INNER JOIN EXTRAINFO ON Soa.Recom = EXTRAINFO.TPNo
                        WHERE (Soa.Year = @Year) AND (EXTRAINFO.TP = 'Recom') AND (Soa.Q = @Q) AND (Soa.Recom=@Id)
                            GROUP BY Soa.InsType, EXTRAINFO.TPName, Soa.Q, Soa.Recom">
                        <SelectParameters>
                            <%-- <asp:ControlParameter ControlID="Years" PropertyName="Value" Name="Year" Type="Int32"></asp:ControlParameter>
                            <asp:ControlParameter ControlID="Quarters" PropertyName="Value" Name="Q" Type="Int32"></asp:ControlParameter>--%>
                            <asp:SessionParameter Name="Year" SessionField="Year" DbType="Int32" />
                            <asp:SessionParameter Name="Q" SessionField="Q" DbType="Int32" />
                            <asp:SessionParameter Name="Id" SessionField="Id" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </DetailRow>
            </Templates>
            <SettingsDetail ShowDetailRow="true" />
        </dx:ASPxGridView>
        <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" AllowResize="True"
            CloseAction="CloseButton" ContentUrl="~/Reinsurance/Quarters.aspx"
            EnableViewState="False" PopupElementID="Image1" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowFooter="True" Width="700px" Modal="true"
            Height="600px" FooterText=""
            ClientInstanceName="PrintPop" EnableHierarchyRecreation="True"
            CssFilePath="~/App_Themes/Office2003Olive/{0}/styles.css" CssPostfix="Office2003Olive"
            EnableHotTrack="False" SpriteCssFilePath="~/App_Themes/Office2003Olive/{0}/sprite.css">
            <LoadingPanelImage Url="~/App_Themes/Office2003Olive/Web/Loading.gif">
            </LoadingPanelImage>
            <ClientSideEvents CloseUp="function(s, e) {MainGrid.PerformCallback();}"
                CloseButtonClick="function(s, e) {MainGrid.PerformCallback();}" Init="puOnInit" />
            <HeaderStyle>
                <Paddings PaddingRight="6px" />
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
</asp:Content>