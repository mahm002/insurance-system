<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MPaymentEntry.aspx.vb" Inherits=".MPaymentEntry" %>
 <%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Entry</title>
    <style type="text/css">
        .container {
            width: 95%;
            margin: 20px auto;
            padding: 20px;
        }
        .dxflGroupBox {
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <dx:ASPxFormLayout ID="MainFormLayout" runat="server" Width="100%">
                <Items>
                    <dx:LayoutGroup Caption="Receipt Information">
                        <Items>
                            <dx:LayoutItem Caption="Document No">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtDocNo" runat="server" Width="200px">
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Document Date">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxDateEdit ID="dtDocDate" runat="server" Width="200px">
                                        </dx:ASPxDateEdit>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Customer Name">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtCustomerName" runat="server" Width="300px">
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>

                    <dx:LayoutGroup Caption="Payment Methods">
                        <Items>
                            <dx:LayoutItem ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButton ID="btnAddPayment" runat="server" Text="Add Payment Method" 
                                            AutoPostBack="false" UseSubmitBehavior="false">
                                            <ClientSideEvents Click="function(s, e) { grid.AddNewRow(); }" />
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxGridView ID="gridPaymentDetails" runat="server" AutoGenerateColumns="False" 
                                            KeyFieldName="TempID" OnRowInserting="gridPaymentDetails_RowInserting" 
                                            OnRowUpdating="gridPaymentDetails_RowUpdating" OnRowDeleting="gridPaymentDetails_RowDeleting"
                                            OnCustomCallback="gridPaymentDetails_CustomCallback"
                                            ClientInstanceName="grid" Width="100%">
                                            <SettingsEditing Mode="Inline" />
                                            <Settings ShowFooter="True" />
                                            <SettingsBehavior ConfirmDelete="true" />
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="true" VisibleIndex="0" Width="100" Caption="Actions"/>
                                                <dx:GridViewDataComboBoxColumn FieldName="PaymentType" Caption="Payment Type" VisibleIndex="1" Width="120">
                                                    <PropertiesComboBox>
                                                        <Items>
                                                            <dx:ListEditItem Text="Cash" Value="Cash" />
                                                            <dx:ListEditItem Text="Card" Value="Card" />
                                                            <dx:ListEditItem Text="Cheque" Value="Cheque" />
                                                            <dx:ListEditItem Text="Bank Transfer" Value="Bank Transfer" />
                                                        </Items>
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="Text" />
                                                    </PropertiesComboBox>
                                                </dx:GridViewDataComboBoxColumn>
                                                <dx:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="100">
                                                    <PropertiesTextEdit DisplayFormatString="c2">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="Text" />
                                                    </PropertiesTextEdit>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ReferenceNo" Caption="Reference No" VisibleIndex="3" Width="120"/>
                                                <dx:GridViewDataTextColumn FieldName="BankName" Caption="Bank Name" VisibleIndex="4" Width="120"/>
                                                <dx:GridViewDataTextColumn FieldName="ChequeNo" Caption="Cheque No" VisibleIndex="5" Width="100"/>
                                                <dx:GridViewDataTextColumn FieldName="CardType" Caption="Card Type" VisibleIndex="6" Width="100"/>
                                                <dx:GridViewDataTextColumn FieldName="Note" Caption="Note" VisibleIndex="7" Width="150"/>
                                            </Columns>
                                            <TotalSummary>
                                                <dx:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" DisplayFormat="Total: {0:c2}" />
                                            </TotalSummary>
                                        </dx:ASPxGridView>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>

                    <dx:LayoutGroup Caption="Other Information">
                        <Items>
                            <dx:LayoutItem Caption="Total Amount">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtTotalAmount" runat="server" Width="200px" ReadOnly="true" Text="0.00">
                                            <ReadOnlyStyle BackColor="#F0F0F0" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Notes">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxMemo ID="txtNote" runat="server" Width="100%" Rows="3">
                                        </dx:ASPxMemo>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>

                    <dx:LayoutGroup>
                        <Items>
                            <dx:LayoutItem ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButton ID="btnSave" runat="server" Text="Save Payment" Width="120px" 
                                            OnClick="btnSave_Click">
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" Width="120px" 
                                            Style="margin-left: 10px;" OnClick="btnCancel_Click">
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                </Items>
            </dx:ASPxFormLayout>
        </div>
    </form>
</body>
</html>
