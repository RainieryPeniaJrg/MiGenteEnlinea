<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" CodeBehind="AdquirirPlanEmpleador.aspx.cs" Inherits="MiGente_Front.Empleador.AdquirirPlanEmpleador" Async="true" %>
<%@ Register TagPrefix="uc" TagName="termsModal" Src="~/UserControls/TerminosMiGente.ascx" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <meta charset="UTF-8">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #0d6efd;
            color: white;
        }

        .pricing-card {
            border: none;
            border-radius: 10px;
            background-color: #ffffff;
            color: #212529;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
            transition: transform 0.3s;
        }

            .pricing-card:hover {
                transform: scale(1.05);
            }

        .price-btn {
            background-color: #0d6efd;
            border: none;
            color: white;
            transition: background-color 0.3s;
        }

            .price-btn:hover {
                background-color: #0056b3;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    

            <div class="pagetitle">
                <h1>Gestion de Planes</h1>
                <nav>
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><a href="#">Adquirir un Plan</a></li>
                    </ol>
                </nav>

            </div>
    
            <section id="pricing" class="py-2">
                <div class="container">
                    <div class="text-center mb-5">
                        <h2>Nuestros Planes</h2>
                        <p>Accede a los beneficios que Mi Gente en Línea ofrece adquiriendo uno de nuestros planes.</p>
                    </div>
             
                    <div class="row g-4">
                        <div class="col-lg-4 col-md-6">
                            <div class="card pricing-card text-center p-4">
                                <h4>Eres mi Gente</h4>
                                <h2>RD$495.00</h2>
                                <span>/Anual</span>
                                <ul class="list-unstyled mt-3 mb-4">
                                    <li><b>1</b> Administrador</li>
                                    <li><b>1</b> Registro de Empleado</li>
                                    <li>Consultas Ilimitadas</li>
                                    <li><b>12</b> Meses de Data Histórica</li>
                                </ul>
  
                                <button runat="server" id="btnPlan1" onserverclick="btnPlan1_ServerClick" class="btn btn-primary">Adquirir</button>
    
            </div>
                        </div>

                        <div class="col-lg-4 col-md-6">
                            <div class="card pricing-card text-center p-4">
                                <h4>Somos Mi Gente</h4>
                                <h2 runat="server" id="precio2">RD$1,695.00</h2>
                                <span>/Anual</span>
                                <ul class="list-unstyled mt-3 mb-4">
                                    <li><b>1</b> Administrador</li>
                                    <li><b>1</b> Usuario de Consulta</li>
                                    <li><b>5</b> Registros de Empleados</li>
                                    <li>Consultas Ilimitadas</li>
                                    <li><b>12</b> Meses de Data Histórica</li>
                                </ul>
                                <button runat="server" id="btnPlan2" onserverclick="btnPlan2_ServerClick" class="btn btn-primary">Adquirir</button>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-6">
                            <div class="card pricing-card text-center p-4">
                                <h4>Mi Gente Somos Todos</h4>
                                <h2>RD$3,750.00</h2>
                                <span>/Anual</span>
                                <ul class="list-unstyled mt-3 mb-4">
                                    <li><b>1</b> Administrador</li>
                                    <li><b>2</b> Usuarios de Consulta</li>
                                    <li><b>15</b> Registros de Empleados</li>
                                    <li>Consultas Ilimitadas</li>
                                    <li><b>12</b> Meses de Data Histórica</li>
                                    <li><b>Nómina</b> Pre-diseñada para TSS</li>
                                </ul>
                                <button runat="server" id="btnPlan3" onserverclick="btnPlan3_ServerClick" class="btn btn-primary">Adquirir</button>
                            </div>
                        </div>
                    </div>
             
                </div>
            </section>

            <div class="modal fade" id="checkoutModal" tabindex="-1" aria-labelledby="checkoutModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header bg-secondary text-white">
                <h5 class="modal-title text-center" id="checkoutModalLabel">Pago con Tarjeta de Crédito</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
                <div class="col col-12 justify-content-center" style="text-align: center">
                    <img width="200" src="https://gcs-international.com/wp-content/uploads/2017/06/Cardnet-Web.png" />
                </div>
                <div class="form">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="mb-1">
                                <label for="name" class="form-label">Nombre del Titular</label>
                                <dx:BootstrapTextBox ID="txtNombre" ValidationSettings-RequiredField-IsRequired="true" 
                                                     ValidationSettings-ValidationGroup="pay" NullText="Ingrese el Nombre" runat="server"></dx:BootstrapTextBox>
                            </div>
                            <div class="mb-1">
                                <label for="card-number" class="form-label">Número de Tarjeta</label>
                                <dx:BootstrapTextBox ID="txtTarjeta" ValidationSettings-RequiredField-IsRequired="true" 
                                                     ValidationSettings-ValidationGroup="pay" NullText="1234 5678 9012 3456" runat="server"></dx:BootstrapTextBox>
                            </div>
                            <div class="row mb-1">
                                <div class="col">
                                    <label for="expiry-date" class="form-label">Vencimiento</label>
                                    <dx:BootstrapTextBox ID="expiryDate" MaskSettings-Mask="00/00" ValidationSettings-RequiredField-IsRequired="true" 
                                                         ValidationSettings-ValidationGroup="pay" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col">
                                    <label for="ccv" class="form-label">CCV</label>
                                    <dx:BootstrapTextBox ID="txtCVV" ValidationSettings-RequiredField-IsRequired="true" 
                                                         ValidationSettings-ValidationGroup="pay" NullText="123" runat="server"></dx:BootstrapTextBox>
                                </div>
                            </div>
                            <a style=" font-size:smaller; color: darkred">NOTA: Los datos suministrados no seran almacenados en Mi Gente en Linea...</a>
                            <hr />
                            <div class="card position-sticky top-0">
                                <div class="p-3 bg-light bg-opacity-10">
                                    <div class="d-flex justify-content-between mb-1 small">
                                        <label style="font-size: large" class="col-lg-12">Nombre de Plan</label>
                                    </div>
                                    <div>
                                        <label style="font-size: medium" class="col-lg-12" runat="server" id="lbPlanName">Plan</label>
                                    </div>
                                    <hr>
                                    <div class="d-flex justify-content-between mb-1 small">
                                        <span>TOTAL A PAGAR</span> <strong runat="server" id="amount" class="text-dark">224.50</strong>
                                    </div>
                                    <div class="row form-check mb-2 col col-md-12">
                                        <dx:BootstrapCheckBox ID="chkTerminosMiGente" ValidationSettings-ValidationGroup="pay" 
                                                             Text="Acepto los" ValidationSettings-RequiredField-IsRequired="true" runat="server"></dx:BootstrapCheckBox>
                                        <a href="../HtmlTemplates/TerminosMiGente.html" style="margin-top: -5px" id="enlaceTerminos1">Términos y Condiciones</a>
                                    </div>
                                    <div class="row form-check small" runat="server" id="divAutorizacionEmpleadores">
                                        <dx:BootstrapCheckBox ID="chkAutorizacion" ValidationSettings-ValidationGroup="pay" 
                                                             Text="Acepto los" ValidationSettings-RequiredField-IsRequired="true" runat="server"></dx:BootstrapCheckBox>
                                        <label class="form-check-label" style="margin-top: -5px" for="chkAutorizacion">
                                            <a href="../HtmlTemplates/AutorizacionEmpleadores.html" id="enlaceTerminos2">Terminos de inscripcion de Empleadores</a>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <asp:UpdateProgress AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                                    <ProgressTemplate>
                                   <img src="https://media1.giphy.com/media/v1.Y2lkPTc5MGI3NjExZ21zYmY5d2Zoc2UxaGp1dnd1OXh5dnhqazBqZ2FoM2N5cmozbW9qZSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/tVFCT2uq9HL0M58KBd/giphy.webp" 
                    alt="Espere" style="width: 100px; height: 100px;" />
                                        <label>Espere mientras se procesa el pago</label>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <div class="col col-md-12 align-content-end">
                                    <dx:BootstrapButton ID="txtSubmit" Width="100%" runat="server" AutoPostBack="false" Text="Completar Suscripcion"
                                        ValidationGroup="pay" CausesValidation="true" OnClick="txtSubmit_Click">
                                      
                                    </dx:BootstrapButton>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</div>


    <link href="https://cdn3.devexpress.com/jslib/22.1.7/css/dx.light.css" rel="stylesheet" />
<script src="https://cdn3.devexpress.com/jslib/22.1.7/js/dx.all.js"></script>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            var enlaceTerminos1 = document.getElementById("enlaceTerminos1");
            var enlaceTerminos2 = document.getElementById("enlaceTerminos2");
            //var enlaceTerminos3 = document.getElementById("enlaceTerminos3");

            enlaceTerminos1.addEventListener("click", function (e) {
                e.preventDefault(); // Evita que el enlace se abra en la ventana actual

                // Especifica las dimensiones y opciones de la ventana emergente
                var opcionesVentana = "width=800, height=600, resizable=yes, scrollbars=yes";

                // Abre el enlace en una ventana emergente
                window.open(this.href, "Términos y Condiciones", opcionesVentana);
            });


            enlaceTerminos2.addEventListener("click", function (e) {
                e.preventDefault(); // Evita que el enlace se abra en la ventana actual

                // Especifica las dimensiones y opciones de la ventana emergente
                var opcionesVentana = "width=800, height=600, resizable=yes, scrollbars=yes";

                // Abre el enlace en una ventana emergente
                window.open(this.href, "Términos y Condiciones", opcionesVentana);
            });

            //enlaceTerminos3.addEventListener("click", function (e) {
            //    e.preventDefault(); // Evita que el enlace se abra en la ventana actual

            //    // Especifica las dimensiones y opciones de la ventana emergente
            //    var opcionesVentana = "width=800, height=600, resizable=yes, scrollbars=yes";

            //    // Abre el enlace en una ventana emergente
            //    window.open(this.href, "Términos y Condiciones", opcionesVentana);
            //});
        });

        // Función para mostrar alerta de suscripción exitosa
        function mostrarAlertaSuscripcionExitosa() {
            Swal.fire({
                icon: 'success',
                title: '¡Suscripción completada!',
                text: 'Tu suscripción se ha completado correctamente.',
                confirmButtonText: 'OK',  // Cambia el texto del botón de confirmación
            }).then((result) => {
                // Verifica si el usuario hizo clic en "OK"
                if (result.isConfirmed) {
                    // Redirige a otra página
                    window.location = 'miPerfilEmpleador.aspx';
                    window.location.replace = 'miPerfilEmpleador.aspx';

                }
            });
        }

        // Función para mostrar alerta de problema al procesar el pago
        function mostrarAlertaProblemaPago() {
            Swal.fire({
                icon: 'error',
                title: '¡Error al procesar el pago!',
                text: 'Ha ocurrido un problema al procesar tu pago. Por favor, inténtalo de nuevo.',
            });
        }

        function mostrarAlertaTerminos() {
            Swal.fire({
                icon: 'error',
                title: '¡No Puede Continuar con El Registro!',
                text: 'Debe Aceptar todos los Terminos y Condiciones.',
            });
        }

        function openModal() {
            var modal = new bootstrap.Modal(document.getElementById('checkoutModal'));
            modal.show();
        }
        </script>

</asp:Content>
