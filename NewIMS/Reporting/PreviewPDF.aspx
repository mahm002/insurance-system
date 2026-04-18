<%@ Page Language="vb" EnableSessionState="Readonly" AutoEventWireup="true" CodeBehind="PreviewPDF.aspx.vb" Inherits="PreviewPDF" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html lang="ar-ly" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        html, body, #form1, #div1 {
            height: 100%;
            direction: rtl;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="div1">
            <iframe dir="rtl">

                <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
                <rsweb:ReportViewer ID="ReportViewer" runat="server" SizeToReportContent="True" Height="" Width="">
                </rsweb:ReportViewer>
            </iframe>
        </div>
    </form>
</body>
</html>