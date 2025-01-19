<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="POSG_V1.login" %>


<!DOCTYPE html>
<html>
<head>
	<title>UTC | Iniciar Sesión</title>
	<link rel="stylesheet" type="text/css" href="css/login.css">
	<link href="https://fonts.googleapis.com/css?family=Poppins:600&display=swap" rel="stylesheet">
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">


	<meta name="viewport" content="width=device-width, initial-scale=1">
</head>
<body>
	<img class="wave" src="images/login/wave.png">
	<div class="container">
		<div class="img">
			<img src="images/login/bg7.svg">
		</div>
		<div class="login-content">
			<form runat="server">
				<img src="images/login/avatar4.png">
				<h2 class="title">Iniciar Sesion</h2>
           		<div class="input-div one">
           		   <div class="i">
           		   		<i class="fas fa-user"></i>
           		   </div>
           		   <div class="div">
           		   		<h5>Usuario</h5>
           		   		<asp:TextBox ID="txtusuario" runat="server" CssClass="input"></asp:TextBox>
           		   </div>
           		</div>
           		<div class="input-div pass">
           		   <div class="i"> 
           		    	<i class="fas fa-lock"></i>
           		   </div>
           		   <div class="div">
           		    	<h5>Contraseña</h5>
           		    	<asp:TextBox ID="txtpassword" runat="server" TextMode="Password" CssClass="input"></asp:TextBox>
            	   </div>
            	</div>
            	<asp:Button ID="btnIniciar" runat="server" Text="Ingresar" CssClass="btn" OnClick="btnIniciar_Click" />
            	<asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </form>
        </div>
    </div>
    <script type="text/javascript" src="js/login.js"></script>
</body>
</html>
