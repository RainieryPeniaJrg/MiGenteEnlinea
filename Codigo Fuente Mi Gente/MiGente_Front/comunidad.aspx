<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" CodeBehind="comunidad.aspx.cs" Inherits="MiGente_Front.Empleador.comunidad" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style>
        @import url('https://fonts.googleapis.com/css2?family=Maven+Pro&display=swap');

        body {
            font-family: 'Maven Pro', sans-serif
        }

        body {
            background-color: #eee
        }

        .add {
            border-radius: 20px
        }

        .card {
            border: none;
            border-radius: 10px;
            transition: all 1s;
            cursor: pointer
        }

            .card:hover {
                -webkit-box-shadow: 3px 5px 17px -4px #777777;
                box-shadow: 3px 5px 17px -4px #777777
            }

        .ratings i {
            color: green
        }

        }

        hr {
            margin: 1rem 0;
            color: inherit;
            background-color: currentColor;
            border: 0;
            opacity: 0.25;
        }

        .star {
            font-size: 20px;
            color: #ccc; /* Color gris para estrella vacía */
            margin-right: 5px; /* Espacio entre estrellas */
        }

            .star.filled {
                color: yellow; /* Color amarillo para estrella llena */
            }

        .avatar {
            border-radius: 50%;
            width: 150px; /* Ajusta el ancho y alto según tus necesidades */
            height: 150px;
            object-fit: cover; /* Para que la imagen se ajuste y no se deforme */
        }

        .profile-image {
            border-radius: 50%;
            width: 150px;
            height: 150px;
            object-fit: cover;
            margin: 0 auto;
            display: block;
        }

        /* Estilo para el overlay */
.overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.8); /* Fondo oscuro semi-transparente */
    display: none; /* Inicialmente oculto */
    justify-content: center;
    align-items: center;
    z-index: 9999; /* Asegura que esté encima de todo */
}

.overlay img {
    max-width: 90%;
    max-height: 90%;
    object-fit: contain; /* Asegura que la imagen se ajuste sin distorsionarse */
    border-radius: 10px;
}

    </style>
</asp:Content>

   <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="justify-content-center mb-5 text-center" style="margin-top:-200px" >
         <h4 class="text-white" style="font-weight:bold" runat="server" id="tituloBuscador">Encuentra al Colaborador Perfecto</h4>
         <label class="text-white">Apoyate de una comunidad de profesionales certificados</label>
     </div>
      <div class="row col col-md-12 col-12 justify-content-center" style="padding-block:40px; " >
    <div class="row col col-md-10 col-12 bg-white" style="box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1); padding-block:40px;margin-top:-30px; border-radius:10px " >

        <h5 style="color:darkgrey; font-weight:bold">Criterio de Busqueda</h5>
        <div class="row">
            <div class="col col-md-7 col-12">
            <dx:BootstrapTextBox ID="cbReferencia" NullText="Ingrese un Texto de Referencia" Caption="Criterio" Width="100%" runat="server"></dx:BootstrapTextBox>
                </div>
                <div class="col col-md-4 col-12">
                    <dx:BootstrapComboBox ID="cbUbicacion" ValueField="provinciaID" TextField="nombre" NullText="Seleccione la Ubicacion" Caption="Ubicacion" Width="100%" runat="server" DataSourceID="linqCiudades" ValueType="System.Int32"></dx:BootstrapComboBox>
               <asp:LinqDataSource runat="server" EntityTypeName="" ID="linqCiudades" ContextTypeName="MiGente_Front.Data.migenteEntities" OrderBy="provinciaID" TableName="Provincias"></asp:LinqDataSource>
               
                    </div>
              <div class="col col-md-1 col-12 align-content-end">
            <dx:BootstrapButton ID="btnBuscar"  Text="" CssClasses-Icon="bi bi-search" CssClasses-Control="bg-primary" OnClick="btnBuscar_Click" Width="100%" runat="server"></dx:BootstrapButton>
                </div>
        </div>
        </div>
    </div>
    <div class="row mt-3">
        <h5>Ultimas Publicaciones</h5>
        <a>Encuentra lo que buscas gracias a nuestra comunidad de profesionales calificados</a>
        <hr class="mt-3"/>


          <div class="row col col-md-12 col-sm-12">

    <asp:Repeater ID="repeaterTarjetas" runat="server">
    <ItemTemplate>
        <div class="col-md-3 col-sm-6 mb-4">
            <div class="card p-3 text-center h-100 shadow-sm">
                <!-- Imagen circular con ajustes para adaptarse al tamaño -->
                <div class="img mb-3 d-flex justify-content-center align-items-center" style="width: 120px; height: 120px; overflow: hidden; border-radius: 50%; margin: 0 auto;">
                    <img src='<%# Eval("imagenURL") %>' class="img-fluid" alt="Imagen de <%# Eval("Nombre") %>" style="width: 100%; height: 100%; object-fit: cover;">
                </div>

                <!-- Nombre del profesional -->
                <h5 class="mb-1 fw-bold text-truncate"><%# Eval("Nombre") %></h5>
                <!-- Título -->
                <p class="text-muted mb-2"><%# Eval("titulo") %></p>

                <!-- Calificación y registros -->
                <div class="ratings mt-2" title="Perfil calificado: <%# Eval("total_registros") %> veces">
                    <%# GetStarRating((decimal)Eval("calificacion")) %>
                    <small class="text-muted ms-1">(<%# Eval("total_registros") %>)</small>
                </div>

                <!-- Botón para ver el perfil -->
                <div class="mt-4">
                    <asp:Button ID="btnPerfil" runat="server" OnClick="btnPerfil_Click" CssClass="btn btn-success btn-sm text-uppercase"
                        Text="Ver Perfil" CommandArgument='<%# Eval("userID") %>' />
                </div>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>


       <div class="modal fade" id="modalPerfil" tabindex="-1" aria-labelledby="modalPerfilLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content">
            <!-- Encabezado del modal -->
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title" id="modalPerfilLabel">Perfil Profesional</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>

            <!-- Cuerpo del modal -->
            <div class="modal-body">
                <!-- Información principal -->
                <div class="text-center"style="padding-block:10px; background-size:cover ; background-image:url('images/bgprofile.jpg')">
                    <div class="rounded-circle overflow-hidden mb-3" style="width: 120px; height: 120px; margin: 0 auto;">
                        <img src="#" id="imgProfile2" runat="server" alt="Imagen de Perfil" 
                             class="img-fluid" style="object-fit: cover;">
                    </div>
                    <h5 class="fw-bold text-white" id="NombrePerfil" runat="server">Nombre</h5>
                    <div class="rating mt-2 text-white">
                        <%= GetStarRating(Convert.ToDecimal(hiddenCalificacion.Value)) %>
                        <span class="ms-2 text-white"><%= hiddenCalificacion.Value %></span>
                    </div>
                    <asp:HiddenField ID="hiddenCalificacion"  Value="0" runat="server" />
                    <p class="mt-3 text-white" id="presentacion" runat="server">Presentación de ejemplo</p>
                </div>

                <!-- Navegación por pestañas -->
                <ul class="nav nav-tabs mt-4" role="tablist">
                    <li class="nav-item">
                        <button class="nav-link active" id="datosGenerales-tab" data-bs-toggle="tab" 
                                data-bs-target="#datosGenerales" type="button" role="tab" 
                                aria-controls="datosGenerales" aria-selected="true">Datos Generales</button>
                    </li>
                    <li class="nav-item">
                        <button class="nav-link" id="servicios-tab" data-bs-toggle="tab" 
                                data-bs-target="#servicios" type="button" role="tab" 
                                aria-controls="servicios" aria-selected="false">Catálogo de Servicios</button>
                    </li>
                </ul>

                <!-- Contenido de las pestañas -->
                <div class="tab-content mt-3">
                    <!-- Pestaña de Datos Generales -->
                    <div class="tab-pane fade show active" id="datosGenerales" role="tabpanel" aria-labelledby="datosGenerales-tab">
                        <div class="row" >
                            <!-- Columna izquierda -->
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <strong class="form-label">Título de Perfil:</strong>
                                    <p id="titulo" runat="server">Título de ejemplo</p>
                                </div>
                                <div class="mb-3">
                                    <strong class="form-label">Email:</strong>
                                    <p id="email" runat="server">correo@example.com</p>
                                </div>
                                <div class="mb-3">
                                    <strong class="form-label">Años de Experiencia:</strong>
                                    <p id="experiencia" runat="server">5 años</p>
                                </div>
                            </div>

                            <!-- Columna derecha -->
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <strong class="form-label">Teléfono 1:</strong>
                                    <a id="enlaceWhatsapp1" runat="server" href="#" class="text-decoration-none">
                                        <img id="whatsapp1" runat="server" src="../Images/whatsapp.png" 
                                             alt="WhatsApp" width="24" height="24" class="me-2" visible="false" />
                                    </a>
                                  <a id="enlaceTelefono1" runat="server" href="#" style="display: block; color: black">
                                        <span id="telefono1" runat="server">(000)-000-0000</span>
                                      </a>
                                </div>
                                <div class="mb-3">
                                    <strong class="form-label">Teléfono 2:</strong>
                                    <a id="enlaceWhatsapp2" runat="server" href="#" class="text-decoration-none">
                                        <img id="whatsapp2" runat="server" src="../Images/whatsapp.png" 
                                             alt="WhatsApp" width="24" height="24" class="me-2" visible="false" />
                                    </a>
                                      <a id="enlaceTelefono2" runat="server" href="#" style="display: block; color: black">   
                                        <span id="telefono2" runat="server">(000)-000-0000</span>
                               </a>
                                    </div>
                            </div>
                        </div>
                    </div>

                    <!-- Pestaña de Catálogo de Servicios -->
                    <div class="tab-pane fade" id="servicios" role="tabpanel" aria-labelledby="servicios-tab">
                        <div class="table-responsive mt-3">
                            <asp:Repeater ID="repeaterServicios" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Detalle del Servicio</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("detalleServicio") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Pie del modal -->
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>
        </div>
    </div>
</div>


    </div>

    </div>

       <script>
           // Mostrar la imagen en el overlay
           function showOverlay(imageURL) {
               var overlay = document.getElementById("overlay");
               var overlayImage = document.getElementById("overlayImage");
               overlay.style.display = "flex"; // Muestra el overlay
               overlayImage.src = imageURL; // Establece la imagen que se mostrará
           }

           // Cerrar el overlay al hacer clic en cualquier parte del fondo
           function closeOverlay() {
               var overlay = document.getElementById("overlay");
               overlay.style.display = "none"; // Oculta el overlay
           }
       </script>
</asp:Content>
