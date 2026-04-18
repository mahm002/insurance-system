<%@ Page Language="VB" AutoEventWireup="false" Inherits="OutPutManagement_PreView" CodeBehind="PreView.aspx.vb" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>≈œ«—… «·„Œ—Ã« </title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <rsweb:ReportViewer ID="rptViewer" runat="server" Height="800"
                Width="100%" ProcessingMode="Remote" ShowBackButton="True" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px">
                <ServerReport DisplayName="Demo Report" />
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>