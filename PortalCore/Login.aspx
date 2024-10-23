<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PortalCore.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="صفحه ورود سایت ساز سادین" />
    <meta name="author" content="کامران سادین" />
    <meta name="keyword" content="ورود,لاگین,صفحه ورود,ورود به حساب کاربری,کامران سادین,طراحی سایت,برنامه نویسی" />
    <link rel="shortcut icon" href="/favicon.ico" />
    <title>ورود به مدیریت حساب</title>
    <!-- Bootstrap core CSS -->
    <link href="Admin/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Admin/css/bootstrap-reset.css" rel="stylesheet" />
    <!--external css-->
    <link href="Admin/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <!-- Custom styles for this template -->
    <link href="Admin/css/style.css" rel="stylesheet" />
    <link href="Admin/css/style-responsive.css" rel="stylesheet" />

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
    <script src="Admin/js/html5shiv.js"></script>
    <script src="Admin/js/respond.min.js"></script>
    <![endif]-->
</head>
<body class="login-body">
    <div class="container">
        <form class="form-signin" id="loginFrm" runat="server">

            <h2 class="form-signin-heading">نام کاربری و کلمه عبور را وارد کنید</h2>
            <div class="login-wrap">
                <asp:Label runat="server" ID="lblMessage" CssClass="" Visible="False"></asp:Label>
                <div class="clearfix" style="margin-bottom: 5px;"></div>
                
                <asp:RequiredFieldValidator runat="server" ID="rfvUserName" ForeColor="Red"
                    Text="کلمه عبور خالی است" ControlToValidate="txtUserName"
                    ValidationGroup="Login">*</asp:RequiredFieldValidator>
                
                <asp:TextBox runat="server" ID="txtUserName" class="form-control" placeholder="نام کاربری" autofocus></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ForeColor="Red"
                    Text="رمز ورود خالی است" ControlToValidate="txtPassword"
                    ValidationGroup="Login">*</asp:RequiredFieldValidator>
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" class="form-control" placeholder="کلمه عبور"></asp:TextBox>
                
                <label class="checkbox">
                    <asp:CheckBox runat="server" ID="chkRemember" value="remember-me"/>
                    مرا به خاطر بسپار
                <span class="pull-left">
                    <asp:PlaceHolder runat="server" ID="pnlRecoveryPassword"></asp:PlaceHolder>
                </span>
                </label>

                <asp:Button runat="server" ID="btnLogIn" class="btn btn-lg btn-login btn-block"
                    Text="ورود به حساب کاربری" OnClick="btnLogIn_OnClick" ValidationGroup="Login"/>
                <p>
                    <asp:PlaceHolder runat="server" ID="pnlRegister"></asp:PlaceHolder>
                </p>
                <br/>
                <asp:PlaceHolder runat="server" ID="pnlExternalLogin"></asp:PlaceHolder>
            </div>
        </form>
            <div class="form-signin alert alert-success" style="margin-top: 20px;">
                <label class="">
                    قدرت گرفته از سایت ساز سادین
                </label>
                
            </div>
    </div>
    <script src="Admin/js/jquery.js"></script>
	<script src="Admin/js/bootstrap.min.js"></script>
</body>
</html>
