<%@ Page Language="VB" AutoEventWireup="false" Inherits="CloseClmFile" CodeBehind="CloseClmFile.aspx.vb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>إقفال ملف حادث </title>

    <script lang="javascript" type="text/javascript">
               function SetOnSelectEvent(Elm, IDName) {
                   if (IDName == '')
                       document.getElementById(Elm).focus()
                   else
                       document.getElementById(IDName).object.SetFocus()
               }
               function SetEvent(item, IDName) {
                   if (document.all) {
                       if (event.keyCode == 13) {
                           event.returnValue = false;
                           event.cancel = true;
                           if (IDName == '')
                               document.getElementById(item).focus()
                           else
                               document.getElementById(IDName).object.SetFocus()
                       }
                   }
               }
    </script>
</head>
<body>
    <form id="form1" runat="server" dir="rtl">
        <div>
            <table dir="rtl">
                <tr>
                    <td colspan="2">
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="لا يمكن إقفال الملف لوجود تسويات غير مسددة" RightToLeft="True" ClientVisible="false">
                        </dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>اقفال ملف حادث رقم</td>
                    <td>
                        <dx:ASPxTextBox ID="ClmNo" runat="server" Width="120px" ClientEnabled="false">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>حاله الملف</td>
                    <td>
                        <%--  <asp:Label runat="server" Font-Bold="True" ID="Label2"></asp:Label>
                       <strong>--%>

                        <dx:ASPxTextBox ID="Label2" runat="server" Width="120px" ClientEnabled="false">
                        </dx:ASPxTextBox>
                        <%--</strong>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxButton runat="server" Text="إقفال" ID="ASPxButton6"></dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton runat="server" Text="رفض( اقفال بدون تسوية)" ID="ASPxButton7" Width="200px"></dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>