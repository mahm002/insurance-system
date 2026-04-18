<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ValidateDoc.aspx.vb" Inherits="ValidateDoc" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer" runat="server" SizeToReportContent="True" Height="" Width="">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>