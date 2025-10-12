<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registrar.aspx.cs" Inherits="MiGente_Front.Registrar" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="assets/js/registrar.js"></script>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Crea tu Cuenta</title>
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
            overflow: auto;
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
                        <h4>Registra tu Cuenta</h4>
                        <hr />
                        <div class="">
                            <div id="loginForm" class="login-form animate__animated animate__flipInY">
                                <a href="https://www.migenteenlinea.do" target="_parent">
                                    <div style="text-align: center" class="d-md-none">

                                        <img src="../Images/logoMiGene.png" />
                                    </div>
                                </a>

                                <div>
                                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                                    <div aria-live="polite" aria-atomic="true" class="position-relative">
                                        <div class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 1050;">
                                            <div id="alertToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true" style="min-width: 300px; background-color: #f8d7da; border-color: #f5c6cb; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);">
                                                <div class="toast-header" style="background-color: #f5c6cb;">
                                                    <strong class="me-auto text-danger">¡Atención!</strong>
                                                    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
                                                </div>
                                                <div class="toast-body text-danger">
                                                    Los campos marcados con * Son obligatorios
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                    
                                            <dx:BootstrapComboBox ID="cboTipo" Caption="Tipo de Cuenta" NullText="Seleccione el Tipo de Perfil" runat="server">
                                                <Items>
                                                    <dx:BootstrapListEditItem Text="Empleador" Value="1" />
                                                    <dx:BootstrapListEditItem Text="Ofertante" Value="2" />
                                                </Items>
                                            </dx:BootstrapComboBox>

                                            <dx:BootstrapTextBox ID="txtNombre" Caption="Nombre" runat="server"></dx:BootstrapTextBox>
                                            <dx:BootstrapTextBox ID="txtApellido" Caption="Apellido" runat="server"></dx:BootstrapTextBox>

                                            <dx:BootstrapTextBox ID="txtEmail" Caption="Email" runat="server"></dx:BootstrapTextBox>


                                            <dx:BootstrapTextBox ID="txtTelefono1" Caption="Telefono 1" runat="server"></dx:BootstrapTextBox>

                                            <dx:BootstrapTextBox ID="txtTelefono2" Caption="Telefono 2" runat="server"></dx:BootstrapTextBox>


                                            <div class="col-sm-12 col-lg-12 my-2">
                                                <dx:BootstrapButton ID="btnRegistrar" runat="server" AutoPostBack="false" Width="100%" OnClick="btnRegistrar_Click" CssClasses-Control="bg-primary" CssClasses-Icon="bi bi-new" Text="Crear Cuenta"></dx:BootstrapButton>
                                            </div>

                                            </div>
                     
                                </div>



                            </div>

                        </div>
                    </div>
                </div>
            </div>

    </form>


      <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>


        function validateForm() {


            var alertToast = new bootstrap.Toast(document.getElementById('alertToast'));
            alertToast.show();
            return false; // Evitar el envío del formulario si los campos están vacíos
        }


    </script>
</body>
</html>
