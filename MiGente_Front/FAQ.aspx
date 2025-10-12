﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="MiGente_Front.FAQ" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style>
        body {
            margin-top: 0px;
        }

        .section_padding_130 {
            padding-top: 20px;
            padding-bottom: 130px;
        }

        .faq_area {
            position: relative;
            z-index: 1;
            background-color: #f5f5ff;
        }

        .faq-accordian {
            position: relative;
            z-index: 1;
        }

            .faq-accordian .card {
                position: relative;
                z-index: 1;
                margin-bottom: 1.5rem;
            }

                .faq-accordian .card:last-child {
                    margin-bottom: 0;
                }

                .faq-accordian .card .card-header {
                    background-color: #ffffff;
                    padding: 0;
                    border-bottom-color: #ebebeb;
                }

                    .faq-accordian .card .card-header h6 {
                        cursor: pointer;
                        padding: 1.75rem 2rem;
                        color: #3f43fd;
                        display: -webkit-box;
                        display: -ms-flexbox;
                        display: flex;
                        -webkit-box-align: center;
                        -ms-flex-align: center;
                        -ms-grid-row-align: center;
                        align-items: center;
                        -webkit-box-pack: justify;
                        -ms-flex-pack: justify;
                        justify-content: space-between;
                    }

                        .faq-accordian .card .card-header h6 span {
                            font-size: 1.5rem;
                        }

                        .faq-accordian .card .card-header h6.collapsed {
                            color: #070a57;
                        }

                            .faq-accordian .card .card-header h6.collapsed span {
                                -webkit-transform: rotate(-180deg);
                                transform: rotate(-180deg);
                            }

                .faq-accordian .card .card-body {
                    padding: 1.75rem 2rem;
                }

                    .faq-accordian .card .card-body p:last-child {
                        margin-bottom: 0;
                    }

        @media only screen and (max-width: 575px) {
            .support-button p {
                font-size: 14px;
            }
        }

        .support-button i {
            color: #3f43fd;
            font-size: 1.25rem;
        }

        @media only screen and (max-width: 575px) {
            .support-button i {
                font-size: 1rem;
            }
        }

        .support-button a {
            text-transform: capitalize;
            color: #2ecc71;
        }

        @media only screen and (max-width: 575px) {
            .support-button a {
                font-size: 13px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Banner Section -->
    <div class="banner-section" style="background: url('https://www.screenbeam.com/wp-content/uploads/2018/10/usa-webcash-faq-banner-image.jpg') no-repeat center center; background-size: cover; padding: 40px 0; color: white;">
        <div class="container text-center">
            <h1 class="display-4 fw-bold text-shadow mb-3">Preguntas Frecuentes</h1>
            <p class="lead text-shadow mb-5">Aquí encontrarás todo lo que necesitas para aclarar tus dudas sobre Mi Gente en línea</p>
            <a href="#faq" class="btn btn-primary btn-lg">Explorar FAQ's</a>
        </div>
    </div>

    <!-- FAQ Section -->
    <div class="faq_area section_padding_130" id="faq">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-12 col-sm-8 col-lg-6">
                    <div class="section_heading text-center wow fadeInUp" data-wow-delay="0.2s">
                        <div class="line mb-4"></div>
                        <h3><span>Preguntas </span>Frecuentes de la Comunidad</h3>
                        <p>Aqui encontraras todo lo que necesitas para aclarar tus dudas sobre Mi Gente en linea</p>
                    </div>
                </div>
            </div>
            <div class="row justify-content-center">
                <!-- FAQ Area -->
                <div class="col-12 col-sm-10 col-lg-8">
                    <div class="accordion" id="faqAccordion">
                        <!-- FAQ Item 1 -->
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingOne">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="false" aria-controls="collapseOne">
                                    1. ¿Cómo inscribo a mis empleados en el sistema?
                                </button>
                            </h2>
                            <div id="collapseOne" class="accordion-collapse collapse" aria-labelledby="headingOne" data-bs-parent="#faqAccordion">
                                <div class="accordion-body">
                                    <p>Para inscribir a tus empleados en nuestro sistema, simplemente sigue estos pasos:</p>
                                    <ul>
                                        <li>Accede a tu cuenta en nuestra plataforma.</li>
                                        <li>Ingresa los datos de tus empleados, y ¡listo! No tendrás que volver a completar su información nunca más.</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <!-- FAQ Item 2 -->
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingTwo">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                    2. ¿Qué ventajas ofrece la inscripción de empleados en la nube?
                                </button>
                            </h2>
                            <div id="collapseTwo" class="accordion-collapse collapse" aria-labelledby="headingTwo" data-bs-parent="#faqAccordion">
                                <div class="accordion-body">
                                    <p>Al inscribir a tus empleados en la nube, podrás:</p>
                                    <ul>
                                        <li>Calcular fácilmente los días laborados.</li>
                                        <li>Gestionar las horas extras.</li>
                                        <li>Generar nóminas regulares.</li>
                                        <li>Controlar derechos adquiridos.</li>
                                        <li>Manejar prestaciones laborales.</li>
                                        <li>Emitir recibos de pago.</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <!-- FAQ Item 3 -->
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingThree">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                                    3. ¿Cómo se realiza un contrato de trabajo?
                                </button>
                            </h2>
                            <div id="collapseThree" class="accordion-collapse collapse" aria-labelledby="headingThree" data-bs-parent="#faqAccordion">
                                <div class="accordion-body">
                                    <p>El contrato de trabajo se realiza de acuerdo con las leyes laborales vigentes. En la República Dominicana, se define como un acuerdo mediante el cual una persona se compromete a prestar un servicio personal a otra a cambio de una remuneración. Puede ser por tiempo indefinido, por cierto tiempo o para una obra o servicio determinado.</p>
                                </div>
                            </div>
                        </div>
                        <!-- FAQ Item 4 -->
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingFour">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseFour" aria-expanded="false" aria-controls="collapseFour">
                                    4. ¿Cómo calculo los días laborados y las horas extras de mis empleados?
                                </button>
                            </h2>
                            <div id="collapseFour" class="accordion-collapse collapse" aria-labelledby="headingFour" data-bs-parent="#faqAccordion">
                                <div class="accordion-body">
                                    <p>El cálculo de los días laborados y las horas extras se basa en la jornada de trabajo de cada empleado. Esto implica contabilizar las horas que el trabajador dedica a sus tareas y puede ser diario, semanal, mensual o anual. Este cálculo es fundamental para determinar las condiciones de trabajo y las horas extraordinarias.</p>
                                </div>
                            </div>
                        </div>
                        <!-- FAQ Item 5 -->
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingFive">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseFive" aria-expanded="false" aria-controls="collapseFive">
                                    5. ¿Cómo puedo realizar la nómina regular de mis empleados?
                                </button>
                            </h2>
                            <div id="collapseFive" class="accordion-collapse collapse" aria-labelledby="headingFive" data-bs-parent="#faqAccordion">
                                <div class="accordion-body">
                                    <p>La nómina regular es un documento legal que refleja la cantidad de dinero que un empleado recibe por su trabajo. Incluye el salario bruto y los descuentos obligatorios por ley, así como los descuentos circunstanciales. El salario neto es la cantidad que el empleado recibe después de aplicar estos descuentos.</p>
                                </div>
                            </div>
                        </div>
                        <!-- FAQ Item 6 -->
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingSix">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSix" aria-expanded="false" aria-controls="collapseSix">
                                    6. ¿Cómo se calcula la Regalía Pascual (Salario de Navidad)?
                                </button>
                            </h2>
                            <div id="collapseSix" class="accordion-collapse collapse" aria-labelledby="headingSix" data-bs-parent="#faqAccordion">
                                <div class="accordion-body">
                                    <p>La Regalía Pascual, o salario de navidad, es la duodécima parte del salario ordinario devengado por el trabajador en el año calendario. Se calcula dividiendo entre doce el total de salarios ordinarios devengados, excluyendo las horas extras y la participación en los beneficios de la empresa. En ningún caso puede ser mayor que cinco salarios mínimos legalmente establecidos.</p>
                                </div>
                            </div>
                        </div>
                        <!-- FAQ Item 7 -->
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingSeven">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSeven" aria-expanded="false" aria-controls="collapseSeven">
                                    7. ¿Cómo genero un recibo de pago para mis empleados?
                                </button>
                            </h2>
                            <div id="collapseSeven" class="accordion-collapse collapse" aria-labelledby="headingSeven" data-bs-parent="#faqAccordion">
                                <div class="accordion-body">
                                    <p>Generar un recibo de pago es fundamental para registrar las transacciones entre empleador y empleado. Este documento sirve como comprobante de pago por los servicios prestados y evita futuros reclamos. Debes incluir detalles como la fecha, el monto y la firma de ambas partes.</p>
                                </div>
                            </div>
                        </div>
                        <!-- FAQ Item 8 -->
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingEight">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseEight" aria-expanded="false" aria-controls="collapseEight">
                                    8. ¿Cómo se calculan los Derechos Adquiridos de los empleados?
                                </button>
                            </h2>
                            <div id="collapseEight" class="accordion-collapse collapse" aria-labelledby="headingEight" data-bs-parent="#faqAccordion">
                                <div class="accordion-body">
                                    <p>Los derechos adquiridos de los empleados, tales como vacaciones, regalía pascual, etc., se calculan con base en el tiempo trabajado por el empleado en la empresa. Cada año trabajado suma días de vacaciones, y dependiendo de su antigüedad, el monto de la regalía pascual puede ser mayor.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div> 
            </div>
        </div>
    </div>
</asp:Content>


