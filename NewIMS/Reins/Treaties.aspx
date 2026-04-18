<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.master" CodeBehind="Treaties.aspx.vb" Inherits="Treaties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script lang="javascript" type="text/javascript">
        function SelectAndClosePopup() {
            popup.Hide();
            MainGrid.Refresh();
            detailGrid.Refresh();
        }
        function ShowPopup(Parameter) {
            //debugger;
            var TreatyYear = Years.lastSuccessText;
            //alert(customerID);Height="800" Width="800"
            if (Parameter == 'New') {
                //PrintPop.SetSize(1300, 950);
                PrintPop.HeaderText = 'Treaty Form'
                PrintPop.SetContentUrl('TreatyEntry.aspx?Year=' + TreatyYear);
                PrintPop.Show();
            }
            else {
                alert(Parameter);
                ////popup.SetContentUrl('Reseat.aspx?PolNo=' + customerID);
                //popup.SetSize(1300, 950);
                HeaderText = "Print"
                //popup.SetContentUrl('../OutPutManagement/PreView.aspx?Report=/IMSReports/Soa');
                //popup.Show();
                window.open(unescape(location.pathname).substring(0, unescape(location.pathname).lastIndexOf("/Reins")) + '/Reporting/PreViewer.aspx?Report=/IMSReports/Soa');
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
    <asp:SqlDataSource runat="server" ID="YearsSource" ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
        SelectCommand="WITH yearlist  AS (SELECT YEAR(GETDATE())-15 AS year  UNION ALL SELECT   yl.year + 1 AS year FROM yearlist yl  WHERE yl.year + 1 <= YEAR(GETDATE())) SELECT  Year FROM yearlist ORDER BY year DESC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource" runat="server"
        ConnectionString='<%$ ConnectionStrings:IMSDBConnectionString %>'
        SelectCommand="SELECT * FROM TRREGFLE  WHERE (right(TREATYNO,4) = @Year) ">
        <SelectParameters>
            <asp:SessionParameter Name="Year" SessionField="Year" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Table runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell>
                <dx:ASPxHyperLink ID="ASPxHyperLink1" ToolTip="Add/Edit Treaty Details"
                    NavigateUrl="javascript:ShowPopup('New')"
                    ImageUrl="~/Content/Images/editing.png" runat="server" Text="ASPxHyperLink">
                </dx:ASPxHyperLink>
            </asp:TableCell>
            <asp:TableCell>
                <dx:ASPxComboBox ID="Years" ClientInstanceName="Years" runat="server" OnSelectedIndexChanged="Years_SelectedIndexChanged" SelectedIndex="0"
                    AutoPostBack="false" TextField="Year" EnableIncrementalFiltering="True" IncrementalFilteringMode="StartsWith"
                    MaxLength="3" Width="140px" DataSourceID="YearsSource" ValueField="Year">
                    <LoadingPanelStyle ImageSpacing="5px"></LoadingPanelStyle>

                    <ButtonStyle Width="13px"></ButtonStyle>
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {MainGrid.PerformCallback();}" />
                </dx:ASPxComboBox>
            </asp:TableCell>
            <asp:TableCell>
            </asp:TableCell>
            <asp:TableCell>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <dx:ASPxGridView ID="MainGrid" ClientInstanceName="MainGrid" runat="server" DataSourceID="SqlDataSource" KeyFieldName="Id" Width="100%" AutoGenerateColumns="False"
        CssFilePath="~/App_Themes/DevEx/{0}/styles.css" CssPostfix="DevEx" SettingsPager-PageSize="25">
        <Settings ShowFilterRow="True" ShowFilterBar="Visible"></Settings>

        <Columns>
            <dx:GridViewDataColumn FieldName="TREATYNO" VisibleIndex="0" Caption="TREATY No." />
            <dx:GridViewDataColumn FieldName="Descrip" VisibleIndex="1" Caption="Treaty Desciption" />
            <dx:GridViewDataTextColumn FieldName="TRINSDTE" VisibleIndex="2" Caption="Inception Date" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="center">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TREXPDTE" VisibleIndex="3" Caption="Expirity Date" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd" CellStyle-HorizontalAlign="center">
                <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TRCAPCTY" VisibleIndex="4" Caption="Treaty Capacity" PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Center">
                <PropertiesTextEdit DisplayFormatString="N3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TRRETAMT" VisibleIndex="5" Caption="Retention" PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Center">
                <PropertiesTextEdit DisplayFormatString="N3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TRQSRAMT" VisibleIndex="6" Caption="Q/S" PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Center">
                <PropertiesTextEdit DisplayFormatString="N3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TR1STAMT" VisibleIndex="7" Caption="1ST Surp." PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Center">
                <PropertiesTextEdit DisplayFormatString="N3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TR2STAMT" VisibleIndex="8" Caption="2ND Surp." PropertiesTextEdit-DisplayFormatString="N3" CellStyle-HorizontalAlign="Center">
                <PropertiesTextEdit DisplayFormatString="N3">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
        </Columns>
        <Images>
        </Images>
        <ImagesFilterControl>
        </ImagesFilterControl>

        <StylesEditors ButtonEditCellSpacing="0">
            <ProgressBar Height="21px"></ProgressBar>
        </StylesEditors>
    </dx:ASPxGridView>
    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton" ContentUrl="~/Reinsurance/Quarters.aspx"
        EnableViewState="False" PopupElementID="Image1" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="True" Width="700px" Modal="true"
        Height="600px" FooterText=""
        ClientInstanceName="PrintPop" EnableHierarchyRecreation="True"
        CssFilePath="~/App_Themes/Office2003Olive/{0}/styles.css" CssPostfix="Office2003Olive"
        EnableHotTrack="False" SpriteCssFilePath="~/App_Themes/Office2003Olive/{0}/sprite.css">

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
</asp:Content>