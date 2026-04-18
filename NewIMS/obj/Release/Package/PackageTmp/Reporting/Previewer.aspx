<%@ Page Language="vb" EnableSessionState="Readonly" AutoEventWireup="true" CodeBehind="Previewer.aspx.vb" Inherits="Previewer" Async="true" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

    <!DOCTYPE html>
    <html lang="en">

<head runat="server">
    <title></title>
    <script src="../scripts/jquery-3.7.1.min.js"></script>
    <style>
        table, html, body, #form1, #div1, #ReportViewer_ctl09_ctl06_ctl00_ctl01 {
            height: 100%;
            /*direction:rtl;*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="div1">
            <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>

            <rsweb:ReportViewer ID="ReportViewer" runat="server" SizeToReportContent="True" Height="" Width="" ProcessingMode="Remote" ZoomPercent="120">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>