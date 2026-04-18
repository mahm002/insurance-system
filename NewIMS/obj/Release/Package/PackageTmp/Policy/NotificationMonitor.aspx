<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="NotificationMonitor.aspx.vb" Inherits="NotificationMonitor" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notification Service Monitor</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Notification Service Monitor</h1>
            
            <asp:Button ID="btnCheckNow" runat="server" Text="Check Now" OnClick="btnCheckNow_Click" />
            <asp:Button ID="btnViewLogs" runat="server" Text="View Logs" OnClick="btnViewLogs_Click" />
            
            <br /><br />
            
            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
            
            <br /><br />
            
            <asp:GridView ID="gvLogs" runat="server" Visible="false" AutoGenerateColumns="true" CssClass="gridview">
            </asp:GridView>
        </div>
    </form>
</body>
</html>