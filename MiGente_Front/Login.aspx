<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MiGente_Front.Login" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar Sesión</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.css" />
    <link href="assets/css/animated.css" rel="stylesheet" />
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Styles/animated.css" rel="stylesheet" />

            <!-- SweetAlert2 CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10/dist/sweetalert2.min.css" />

    <!-- SweetAlert2 JavaScript -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10/dist/sweetalert2.min.js"></script>
    <style>
        /* Contenedor principal */
        .login-container {
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        /* Imagen de fondo */
        .login-bg {
            background-image: url('Images/MainBanner2.jpg'); /* Imagen detrás */
            background-size: contain;
            background-position: left;
            background-repeat: no-repeat;
            background-attachment: fixed; /* Fija el fondo */
            height: 100vh;
            overflow: hidden; /* Evita scroll */
        }


        /* Formulario centrado */
        .login-card {
            background-color: rgba(255, 255, 255, 0.85); /* Transparencia */
            backdrop-filter: blur(10px); /* Efecto blur */
            z-index: 2;
            border-radius: 12px;
            padding: 30px;
            width: 100%;
            max-width: 400px;
        }

        /* Estilo responsivo para móviles */
        @media (max-width: 767.98px) {
            .login-bg {
                display: none; /* Oculta el fondo en móviles */
            }

            .login-container {
                padding: 15px;
                background-color: #f8f9fa; /* Fondo claro para mayor legibilidad */
            }
        }

        /* Evitar scroll en toda la página */
        html, body {
            height: 100%;
            margin: 0;
            padding: 0;
            overflow: hidden;
        }

        .form-group {
            margin-bottom: 20px;
        }

            .form-group label {
                display: block;
                font-weight: bold;
                margin-bottom: 5px;
            }

        .animated-form {
            display: none;
        }
    </style>
    <script>


        function toggleDivs(hideDivId, showDivId) {
            var hideDiv = document.getElementById(hideDivId);
            var showDiv = document.getElementById(showDivId);

            if (hideDiv && showDiv) {
                hideDiv.style.display = 'none';
                showDiv.style.display = 'block';
            }
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row">
                <!-- Imagen de fondo con superposición -->
                <div class="col-lg-8 d-none d-lg-block position-relative login-bg">
                    <div class="overlay"></div>

                    <img src="Images/logoMiGene.png" width="200" />
                </div>

                <!-- Formulario de Login -->
                <div class="col-lg-4 col-12 login-container">
                    <div class="login-card shadow-lg">
                        <div class="">
                            <div id="loginForm" class="login-form animate__animated animate__flipInY">
                                <a href="https://www.migenteenlinea.do" target="_parent">
                                    <div style="text-align: center" class="d-md-none">

                                        <img src="../Images/logoMiGene.png" />
                                    </div>
                                </a>


                                <div>
                                    <div class="form-group">
                                        <label for="txtEmail">Email</label>
                                        <input runat="server" type="text" id="txtEmail" class="form-control" placeholder="Correo electrónico" />
                                    </div>

                                    <div class="form-group">
                                        <label for="txtPassword">Contraseña</label>
                                        <div class="input-group">
                                            <input runat="server" type="password" id="txtPassword" class="form-control" placeholder="Contraseña" />
                                            <div class="input-group-append">
                                                <button id="btnTogglePassword" class="btn btn-outline-secondary" type="button" onclick="togglePasswordVisibility()">
                                                    <i id="eyeIcon" class="fa fa-eye"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="form-group col col-md-5 col-12">
                                        <dx:BootstrapButton UseSubmitBehavior="true" ID="btnAcceso"  runat="server" Width="100%" AutoPostBack="false" OnClick="LinkButton2_Click" CssClasses-Control="bg-primary" Text="Acceder"></dx:BootstrapButton>
                                    
                                           <%-- <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click" CssClass="btn btn-primary" Width="100%">
                                               Acceder</asp:LinkButton>--%>

                                        </div>
                                        <div class="form-group col col-md-7 col-12">
                                            <asp:LinkButton ID="LinkButton1" OnClick="LinkButton1_Click" runat="server" CssClass="btn btn-outline-secondary" Width="100%">Crear Nueva Cuenta</asp:LinkButton>

                                        </div>
                                    </div>
                                    <div class="form-group text-center">
                                        <a href="#" onclick="toggleDivs('loginForm','forgotPasswordForm')" class="text-primary">¿Olvidaste tu contraseña?</a>
                                    </div>
                                </div>
                            </div>

                            <div id="forgotPasswordForm" class="login-form animated-form animate__animated animate__flipInX">
                                <h4 class="text-center">Recuperar contraseña</h4>
                                <hr />

                                <div>
                                    <div class="form-group">
                                        <label for="txtForgotEmail">Correo electrónico</label>
                                        <dx:BootstrapTextBox ID="txtForgotPass" NullText="Correo Electronico" runat="server"></dx:BootstrapTextBox>

                                    </div>

                                    <div class="form-group">
                                        <dx:BootstrapButton ID="btnSendSolicitud" runat="server" Width="100%" AutoPostBack="false" OnClick="btnSendSolicitud_Click" Text="Enviar Solicitud"></dx:BootstrapButton>
                                    </div>

                                    <div class="form-group text-center">
                                        <a href="#" onclick="toggleDivs('forgotPasswordForm','loginForm')" class="text-primary">Volver al inicio de sesión</a>
                                    </div>
                                </div>
                            </div>


                        </div>

                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        window.addEventListener('load', function () {
            document.getElementById('loginButton').disabled = true;
            document.getElementById('forgotButton').disabled = true;
        });

        function togglePasswordVisibility() {
            var passwordField = document.getElementById("txtPassword");
            var btnShowPassword = document.getElementById("btnShowPassword");

            if (passwordField.type === "password") {
                passwordField.type = "text";
                btnShowPassword.textContent = "Ocultar";
            } else {
                passwordField.type = "password";
                btnShowPassword.textContent = "Mostrar";
            }
        }

    </script>
</body>
</html>
