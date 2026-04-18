<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HInsaImport.aspx.vb" Inherits="HInsaImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function OnFileUploadComplete(s, e) {
            Grid.PerformCallback();
            //cbp.PerformCallback();
        }
        function ReturnToParentPage() {
            //debugger;
            var parentWindow = window.parent;
            parentWindow.SelectAndClosePopup();
            //var t = ExtPrm.GetValue();
            //window.parent.document.getElementById("EXTPRM").value = t;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dxm-rtl">
            <table id="tbl" border="0" dir="rtl" style="width: 100%;">
                <tr>
                    <td colspan="4">
                        <a href="../../Templates/Health.xlsx" target="_blank">نموذج التحميل</a>
                        <dx:ASPxUploadControl runat="server" ShowProgressPanel="True" ShowUploadButton="True" RightToLeft="False" Width="100%"
                            ID="Upload" BrowseButton-Text="استعراض" UploadButton-Text="تحميل البيانات من الملف المرفق" CancelButton-Text="إلغاء"
                            OnFileUploadComplete="FileUploadComplete">
                            <ValidationSettings AllowedFileExtensions=".xls, .xlsx"></ValidationSettings>

                            <ClientSideEvents UploadingProgressChanged="OnFileUploadComplete"></ClientSideEvents>
                        </dx:ASPxUploadControl>
                    </td>
                </tr>
                <tr>
                    <td class="dx-al">القسط السنوي للشخص الواحد</td>

                    <td class="dx-al">
                        <dx:ASPxTextBox runat="server" Width="50px" ID="Rate"></dx:ASPxTextBox>
                    </td>

                    <td class="dx-al">حدود التغطية للشخص</td>

                    <td>
                        <dx:ASPxTextBox runat="server" Width="170px" ID="SumIns"></dx:ASPxTextBox>
                    </td>
                </tr>

                <tr>
                    <td class="dx-al" colspan="4">
                        <%-- <a href="../../Templates/Health.xlsx" target="_blank">نموذج التحميل</a>--%>
                    </td>
                </tr>
            </table>

            <div class="dxm-rtl">
                <dx:ASPxButton runat="server" Text="حفظ" ID="ASPxButton5" RightToLeft="True">
                    <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();

                        }" />
                </dx:ASPxButton>

                <br />
            </div>

            <dx:ASPxGridView runat="server" ClientInstanceName="Grid" RightToLeft="True" Width="100%" ID="Grid" OnInit="Grid_Init">
                <SettingsPopup>
                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                </SettingsPopup>
            </dx:ASPxGridView>
        </div>
    </form>
</body>
</html>