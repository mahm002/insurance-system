<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CancelRequest.aspx.vb" Inherits="CancelRequest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function ReturnToParentPage() {
            var parentWindow = window.parent;
            //alert(ASPxClientEdit.ValidateGroup("Reason"));
            if (ASPxClientEdit.ValidateGroup("Reason"))
                parentWindow.SelectAndClosePopup(1);
               }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <table border="0" dir="rtl" style="width: 100%;">
               <tr>
                 <td>
                     <dx:ASPxTextBox ID="ReasonOfCncl" runat="server" Width="100%" Caption="سبب الإلغاء">
                         <ValidationSettings ValidationGroup="Reason" SetFocusOnError="true" Display="Dynamic">
                             <RegularExpression ErrorText="حروف وأرقام" ValidationExpression="^[A-Za-z0-9ا-يءأؤئ ى \\%/()-.]+" />
                             <RequiredField IsRequired="True" />
                         </ValidationSettings> 
                     </dx:ASPxTextBox>
                 </td>
               </tr>
               <tr>
                 <td>
                     <dx:ASPxButton ID="ASPxButton1" runat="server" Text="طلب " OnClick="ASPxButton1_Click" >
                         <ClientSideEvents Click="function(s, e) {
                        ReturnToParentPage();
                        
                        }" />
                     </dx:ASPxButton>

                 </td>
               </tr>
           </table>
        </div>
    </form>
</body>
</html>
