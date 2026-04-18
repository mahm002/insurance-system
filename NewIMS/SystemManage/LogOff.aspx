<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogOff.aspx.vb" Inherits="LogOff" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
	<head>
        <meta content="IE=Edge,chrome=1" />
		<meta charset="utf-8"/>
		<title>ALWATAN IMS SYSTEM</title>
        <meta http-equiv="pragma" content="no-cache" />
		<meta name="generator" content="Bootply" />
		<meta name="viewport" content="width=device-width"/>
		<%--<link href="css/bootstrap.min.css" rel="stylesheet">--%>
        <%--<link href="../Bootstrap/login/css/bootstrap.min.css" rel="stylesheet" />--%>
        <link href="../scripts/bootstrap.min.css" rel="stylesheet" />
		<%--<link href="css/styles.css" rel="stylesheet">--%>
        <link href="../Bootstrap/login/css/styles.css" rel="stylesheet" />
        <%--<link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />--%>
        <style type="text/css">
            .auto-style1 {
                min-height: 16.43px;
                padding: 15px;
                border-bottom: 1px solid #e5e5e5;
                text-align: center;
            }
        </style>
	</head>
	<body>
<!--login modal-->
<div id="loginModal" class="modal show" tabindex="-1" role="dialog" aria-hidden="true">
  <div class="modal-dialog">
  <div class="modal-content">
      <div class="auto-style1">
          <button type="button" onclick="self.close()" class="close" data-dismiss="modal" aria-hidden="true">X</button>
          <asp:Image ID="Image1" runat="server" Height="15%" Width="40%" ImageUrl="~/Bootstrap/login/css/login-box-backg.png" />
      </div>
      <div class="auto-style1">
          <form class="auto-style1" runat="server" >
            <div class="form-group">
              <input type="text" id="usr" runat="server" autocomplete="off" required="required" 
                  autofocus="autofocus" title="اسم المستخدم"  class="form-control input-lg glyphicon-user" placeholder="اسم المستخدم"/>
            </div>
            <div class="form-group">
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              <input type="password" id="pwd" runat="server" required="required" autocomplete="off" class="form-control input-lg" title="كلمة المرور" placeholder="كلمة المرور"/>
            </div>
            <div class="form-group">
                <asp:Button runat="server" CssClass="btn btn-primary btn-lg btn-block" ID="Loginbtn" Text="تسجيل دخول"></asp:Button >
            </div>
          </form>
      </div>
      <div class="modal-footer" aria-disabled="True">
          <div class="col-md-12" >
          <!-- <button class="btn" data-dismiss="modal" aria-hidden="true" >Cancel</button> -->
		  </div>	
      </div>
  </div>
  </div>
</div>
	<!-- script references -->
		<script src="../Bootstrap/login/js/jquery.min.js"></script>
        <script src="../Bootstrap/login/js/bootstrap.min.js"></script>
	</body>
</html>
