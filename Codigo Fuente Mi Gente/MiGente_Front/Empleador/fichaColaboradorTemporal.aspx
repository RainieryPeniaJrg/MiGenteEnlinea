<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" CodeBehind="fichaColaboradorTemporal.aspx.cs" Inherits="MiGente_Front.Empleador.fichaColaboradorTemporal" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pagetitle">
        <h1>Gestion de Contratista</h1>
        <nav>
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="colaboradores.aspx">Colaboradores</a></li>
                <li class="breadcrumb-item"><a>Ficha de Contratacion Temporal</a></li>

            </ol>

        </nav>

    </div>
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>

    <style>
        .card-box {
            border: 1px solid #ccc;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }
    </style>

    <div>


        <div class="bg-white pt-5 pb-5 ">
            <div class="container">
        <h2 runat="server" id="NombreEmpleado" class="text-gray">Empleado</h2>
                </div>
            <div class="container">
                <div id="divMensajeInactivo" runat="server" class="alert alert-light" style="display: none">
                    <h4 class="alert-heading">Este empleado se encuentra desactivado</h4>
                    <p>El empleado está desactivado y no puede realizar tareas en este momento. ¿Deseas imprimir el descargo?</p>
                    <button id="btnImprimirDescargo" runat="server" class="btn btn-primary">Imprimir Descargo</button>
                </div>


                <div class="row">
                    <div class="col-md-12 mt-1">
                        <div class="card-box">
                            <h4 class="card-title">Información del Colaborador</h4>
                            <hr>
                            <div class="row">
                                <div class="col-md-3">
                                    <p><strong>Fecha de Registro:</strong> <span runat="server" id="fechaRegistro"></span></p>
                                    <p><strong>Tipo:</strong> <span runat="server" id="tipo"></span></p>
                                    <p><strong>Identificación:</strong> <span runat="server" id="identificacion"></span></p>
                                    <p><strong>Nombre:</strong> <span runat="server" id="nombre"></span></p>

                                </div>

                                <div class="col-md-3">
                                    <p><strong>Direccion:</strong> <span runat="server" id="htmlDireccion"></span></p>
                                    <p><strong>Provincia:</strong> <span runat="server" id="htmlProvincia"></span></p>
                                    <p><strong>Municipio:</strong> <span runat="server" id="htmlMunucipio"></span></p>
                                </div>
                                <div class="col-md-3">
                                    <p><strong>Teléfono 1:</strong> <span runat="server" id="telefono1"></span></p>
                                    <p><strong>Teléfono 2:</strong> <span runat="server" id="telefono2"></span></p>
                                    <div runat="server" visible="false">
                                        <a>Representante Legal</a>
                                        <p><strong>Cedula Representante:</strong> <span runat="server" id="cedulaRepresentante"></span></p>
                                        <p><strong>Nombre Representante:</strong> <span runat="server" id="nombreRepresentante"></span></p>
                                    </div>
                                </div>
                                    <div class="col-md-3">
                                    <asp:Image ID="Image2" runat="server" />
                                    </div>
                            </div>
                            <div class="row mt-3">
                                <div class="col-md-12">
                                    <button runat="server" class="btn btn-primary me-2" id="btnEditarPerfil" type="button" data-bs-toggle="modal" data-bs-target="#modificarColaboradorModal">Editar</button>
                                    <button type="button" class="btn btn-danger me-2" id="btnBaja" onserverclick="btnBaja_ServerClick"  runat="server">Eliminar</button>
                                    <%--<button runat="server" id="btnPago" class="btn btn-success me-2" type="button" data-bs-toggle="modal" data-bs-target="#modalPago">Realizar Pago</button>--%>
                                    <button runat="server" class="btn btn-success" type="button" data-bs-toggle="modal" id="btnNuevoTrabajo" data-bs-target="#agregarTrabajo">Nueva Contratacion</button>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 mt-1">
                        <ul class="nav nav-tabs mb-3" id="tabsEmpleados" role="tablist">
                            <li class="nav-item" role="presentation">
                                <a class="nav-link active" id="trabajosActivos-tab" data-bs-toggle="tab" href="#trabajosActivos" role="tab" aria-controls="trabajosActivos" aria-selected="true">Trabajos En Curso</a>
                            </li>
                            <li class="nav-item" role="presentation">
                                <a class="nav-link" id="historial-tab" data-bs-toggle="tab" href="#historial" role="tab" aria-controls="historial" aria-selected="false">Historial</a>
                            </li>
                        </ul>
                    </div>
                    <div class="tab-content" id="tabContent">
                        <div class="tab-pane fade show active" id="trabajosActivos" role="tabpanel" aria-labelledby="trabajosActivos-tab">

                            <div>
                                <h5>Listado de Trabajos en Curso</h5>

                            </div>
                            <hr />
                            <dx:BootstrapGridView   SettingsBehavior-AllowSelectByRowClick="true" OnCustomButtonCallback="gridTrabajos_CustomButtonCallback" KeyFieldName="detalleID" ID="gridTrabajos" runat="server" AutoGenerateColumns="False"
                                CssClasses-Table="table table-striped table-bordered" DataSourceID="linqTrabajos">
                                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                <Settings ShowFooter="true" />
                                <SettingsDataSecurity AllowEdit="true" />
                                <Columns>
                                    <dx:BootstrapGridViewTextColumn FieldName="detalleID" Visible="false" VisibleIndex="0"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="contratacionID" Visible="false" VisibleIndex="1"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="descripcionCorta" VisibleIndex="2"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="descripcionAmpliada" Visible="false" VisibleIndex="3"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewDateColumn FieldName="fechaInicio" VisibleIndex="4"></dx:BootstrapGridViewDateColumn>
                                    <dx:BootstrapGridViewDateColumn FieldName="fechaFinal" VisibleIndex="5"></dx:BootstrapGridViewDateColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="montoAcordado" PropertiesTextEdit-DisplayFormatString="{0:n2}" VisibleIndex="6"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewComboBoxColumn FieldName="esquemaPagos" VisibleIndex="7">
                                        <PropertiesComboBox>
                                            <Items>
                                                <dx:BootstrapListEditItem Text="100 % Contra Entrega" Value="1"></dx:BootstrapListEditItem>
                                                <dx:BootstrapListEditItem Text="50% Avance/50% Finalizado" Value="2"></dx:BootstrapListEditItem>
                                                <dx:BootstrapListEditItem Text="30% Avance/70% Finalizado" Value="3"></dx:BootstrapListEditItem>
                                            </Items>
                                        </PropertiesComboBox>
                                    </dx:BootstrapGridViewComboBoxColumn>
                                    <dx:BootstrapGridViewCheckColumn FieldName="estatus" Visible="false" VisibleIndex="8"></dx:BootstrapGridViewCheckColumn>
                                    <dx:BootstrapGridViewCommandColumn VisibleIndex="9">
                                        <CustomButtons>
                                            <dx:BootstrapGridViewCommandColumnCustomButton IconCssClass="fa fa-list" CssClass="text-white bg-warning btn btn-warning btnDetalles" ID="btnDetalles" Text="Detalles"></dx:BootstrapGridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dx:BootstrapGridViewCommandColumn>
                                </Columns>
                                <SettingsEditing Mode="EditForm"></SettingsEditing>
                                <CssClasses Row="grid-nowrap-row" Control="grid-borderless" />

                            </dx:BootstrapGridView>
                            <asp:LinqDataSource runat="server" EntityTypeName="" ID="linqTrabajos" ContextTypeName="MiGente_Front.Data.migenteEntities" OrderBy="fechaInicio desc" TableName="DetalleContrataciones" Where="contratacionID == @contratacionID && estatus == @estatus">
                                <WhereParameters>
                                    <asp:QueryStringParameter QueryStringField="contratacionID" DefaultValue="0" Name="contratacionID" Type="Int32"></asp:QueryStringParameter>
                                    <%--<asp:QueryStringParameter QueryStringField="estatus" DefaultValue="0" Name="estatus" Type="Int32"></asp:QueryStringParameter>--%>

                                    <asp:Parameter DefaultValue="1" Name="estatus" Type="Int32"></asp:Parameter>
                                </WhereParameters>
                            </asp:LinqDataSource>
                            <hr />
                        </div>
                        <div class="tab-pane fade show" id="historial" role="tabpanel" aria-labelledby="historial-tab">

                            <div>
                                <h5>Historico de trabajos contratados</h5>
                            </div>
                            <hr />
                            <dx:BootstrapGridView SettingsBehavior-AllowSelectByRowClick="true"  OnCustomButtonCallback="gridHistorico_CustomButtonCallback"  KeyFieldName="detalleID" ID="gridHistorico" runat="server" AutoGenerateColumns="False"
                                CssClasses-Table="table table-striped table-bordered" DataSourceID="linqTrabajos2">
                                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                <Settings ShowFooter="true" />
                                <SettingsDataSecurity AllowEdit="true" />
                                <Columns>
                                    <dx:BootstrapGridViewTextColumn FieldName="detalleID" Visible="true" VisibleIndex="0"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="contratacionID" Visible="false" VisibleIndex="1"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="descripcionCorta" VisibleIndex="2"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="descripcionAmpliada" Visible="false" VisibleIndex="3"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewDateColumn FieldName="fechaInicio" VisibleIndex="4"></dx:BootstrapGridViewDateColumn>
                                    <dx:BootstrapGridViewDateColumn FieldName="fechaFinal" VisibleIndex="5"></dx:BootstrapGridViewDateColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="montoAcordado" PropertiesTextEdit-DisplayFormatString="{0:n2}" VisibleIndex="6"></dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewCheckColumn FieldName="estatus" Visible="false" VisibleIndex="8"></dx:BootstrapGridViewCheckColumn>
                                    <dx:BootstrapGridViewCommandColumn VisibleIndex="9">
                                        <CustomButtons>
                                            <dx:BootstrapGridViewCommandColumnCustomButton IconCssClass="fa fa-list" CssClass="text-white bg-warning btn btn-warning btnDetalles" ID="btnDetalleHistorico" Text="Detalles"></dx:BootstrapGridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dx:BootstrapGridViewCommandColumn>
                                    <dx:BootstrapGridViewComboBoxColumn FieldName="esquemaPagos" VisibleIndex="7">
                                        <PropertiesComboBox>
                                            <Items>
                                                <dx:BootstrapListEditItem Text="100 % Contra Entrega" Value="1"></dx:BootstrapListEditItem>
                                                <dx:BootstrapListEditItem Text="50% Avance/50% Finalizado" Value="2"></dx:BootstrapListEditItem>
                                                <dx:BootstrapListEditItem Text="30% Avance/70% Finalizado" Value="3"></dx:BootstrapListEditItem>
                                            </Items>
                                        </PropertiesComboBox>
                                    </dx:BootstrapGridViewComboBoxColumn>
                                </Columns>
                                <SettingsEditing Mode="EditForm"></SettingsEditing>
                                <CssClasses Row="grid-nowrap-row" Control="grid-borderless" />

                            </dx:BootstrapGridView>
                            <asp:LinqDataSource runat="server" EntityTypeName="" ID="linqTrabajos2" ContextTypeName="MiGente_Front.Data.migenteEntities" OrderBy="fechaInicio desc" TableName="DetalleContrataciones" Where="contratacionID == @contratacionID && estatus > @estatus">
                                <WhereParameters>
                                    <asp:QueryStringParameter QueryStringField="contratacionID" DefaultValue="0" Name="contratacionID" Type="Int32"></asp:QueryStringParameter>
                                    <asp:Parameter DefaultValue="1" Name="estatus" Type="Int32"></asp:Parameter>

                                </WhereParameters>
                            </asp:LinqDataSource>
                            <hr />
                        </div>
                    </div>




                </div>
            </div>
        </div>
           <!-- Modal Nuevo trabajo -->
    <div class="modal fade" id="agregarTrabajo" tabindex="-1" aria-labelledby="agregarTrabajoModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header ">
                    <h5 class="modal-title" id="agregarTrabajoModalLabel">Crear nueva contratacion</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <a>Detalles de Contratacion</a>
                    <div class="mb-3">
                        <label for="nombreContacto" class="form-label">Descripcion Corta</label>
                        <dx:BootstrapTextBox MaxLength="60" ID="descripcionCortaTrabajo" ValidationSettings-RequiredField-IsRequired="true"
                            ValidationSettings-ValidationGroup="nuevaContratacion" runat="server">
                        </dx:BootstrapTextBox>


                    </div>
                    <div class="mb-3">
                        <label for="nombreContacto" class="form-label">Descripcion Ampliada</label>
                        <dx:BootstrapMemo ID="descripcionAmpliada" MaxLength="250" runat="server"></dx:BootstrapMemo>

                    </div>


                    <div class="mb-3">
                        <label for="fechaInicio" class="form-label">Fecha de Inicio</label>
                        <dx:ASPxDateEdit ID="fechaInicio" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-ValidationGroup="nuevaContratacion" CssClass="form-control" runat="server"></dx:ASPxDateEdit>
                    </div>
                    <div class="mb-3">
                        <label for="fechaInicio" class="form-label">Fecha de Conclusión Esperada</label>
                        <dx:ASPxDateEdit ID="fechaConclusion" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-ValidationGroup="nuevaContratacion" CssClass="form-control" runat="server"></dx:ASPxDateEdit>
                    </div>

                    <div class="mb-3">
                        <label for="montoAcordado" class="form-label">Monto Total Acordado</label>
                        <dx:BootstrapTextBox ID="montoAcordado" NullText="0.00" NullTextDisplayMode="UnfocusedAndFocused" ValidationSettings-ValidationGroup="nuevaContratacion" ValidationSettings-RequiredField-IsRequired="true" runat="server"></dx:BootstrapTextBox>


                    </div>

                    <div class="mb-3">
                        <label for="activo" class="form-label">Esquema de Pagos</label>
                        <asp:DropDownList ID="ddlEsquema" CssClass="form-select" ValidationGroup="nuevaContratacion" runat="server">
                            <asp:ListItem Value="1">100 % Contra Entrega</asp:ListItem>
                            <asp:ListItem Selected="True" Value="2">50% Avance/50% Finalizado</asp:ListItem>
                            <asp:ListItem Value="3">30% Avance/70% Finalizado</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <hr />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" CssClass="btn btn-primary" />
                </div>
            </div>
            <asp:HiddenField ID="hiddenDetalleID" runat="server" />
        </div>
    </div>

        <asp:HiddenField ID="HiddenField1" runat="server" />
    </div>
    <script>
        function ConfirmarEliminarColaborador(contratacionID) {
            Swal.fire({
                title: '¿Estás seguro?',
                text: '¿Deseas eliminar este colaborador y toda la informacion asociada?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sí, Eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Llama al método C# para borrar el colaborador
                    PageMethods.eliminarColaborador(contratacionID, OnEliminarColaboradorSuccess);
                }
            });
        }

        // Función que se llama después de borrar el colaborador
        function OnEliminarColaboradorSuccess(response) {
            // Muestra un mensaje de éxito o error
            MostrarMensaje(response, 'success');

            // Recarga la página después de un breve retraso (por ejemplo, 1 segundo)
            setTimeout(function () {
                window.location.href = "ContratacionesTemporales.aspx";
            }, 500); // 1000 milisegundos = 1 segundo
        }

        // Función para mostrar mensajes de SweetAlert
        function MostrarMensaje(mensaje, tipo) {
            Swal.fire(mensaje, '', tipo);
        }
      </script>
    <script type="text/javascript">
        function AbrirPopupImprimirRecibo(pagoID, userID, documento) {

            var url = 'Impresion/PrintViewer.aspx?documento=' + encodeURIComponent(documento) + '&id=' + encodeURIComponent(pagoID) + '&userID=' + encodeURIComponent(userID); // Reemplaza con la URL de tu página popup
            console.log(url);
            var ventanaPopup = window.open(url, 'PopupWindow', 'width=800,height=600,location=no');
        }

        function DarDeBajaEmpleado() {
            console.log('Dar de baja');
            var valorHidden = document.getElementById('<%= HiddenField1.ClientID %>').value;

            // Obtiene la fecha actual en formato YYYY-MM-DD
            var fechaActual = new Date().toISOString().split('T')[0];

            Swal.fire({
                title: '¿Estás seguro?',
                html:
                    '<hr>' +
                    'Esta acción dará de baja al empleado. ¿Estás seguro que deseas continuar?<br>' +
                    '<label for="motivoBaja" class="form-label">Motivo de Baja:</label>' +
                    '<select class="form-select" id="motivoBaja" class="swal2-input" style="text-align: center;">' +
                    '<option value="Desahucio">Desahucio</option>' +
                    '<option value="Renuncia">Renuncia</option>' +
                    '<option value="Despido Justificado">Despido Justificado</option>' +
                    '<option value="Despido Injustificado">Despido Injustificado</option>' +
                    '<option value="Dimisión">Dimisión</option>' +
                    '</select>' +
                    '<label for="prestaciones" class="form-label">Prestaciones Laborales:</label>' +
                    '<input type="number" id="prestaciones" class="form-control" step="any" required style="text-align: center;" value="0.00" oninput="validarPrestaciones(this)" pattern="^\d+(\.\d{1,2})?$">' +
                    '<a href="https://calculo.mt.gob.do/" target="_blank" style="color:red;">Calcular Prestaciones</a>' +
                    '</br>' +
                    '</br>' +
                    '<label for="fechaBaja" class="form-label">Fecha de Salida:</label>' +

                    '<input type="date" id="fechaBaja" class="form-control" style="text-align: center;" value="' + fechaActual + '">',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, dar de baja',
                cancelButtonText: 'Cancelar',
                preConfirm: () => {
                    var motivoSeleccionado = document.getElementById('motivoBaja').value;
                    var prestacionesLaborales = parseFloat(document.getElementById('prestaciones').value);
                    var fechaSeleccionada = document.getElementById('fechaBaja').value;

                    // Verifica que los campos no estén vacíos y que prestaciones sea un número válido
                    if (!motivoSeleccionado || isNaN(prestacionesLaborales) || prestacionesLaborales < 0) {
                        Swal.showValidationMessage('Por favor, completa todos los campos correctamente.');
                    } else {
                        // Llamada AJAX para activar el evento en el servidor.
                        $.ajax({
                            type: 'POST',
                            url: 'fichaEmpleado.aspx/DarDeBaja', // Ajusta la URL según tu estructura.
                            data: JSON.stringify({
                                userID: valorHidden,
                                motivoBaja: motivoSeleccionado,
                                prestaciones: prestacionesLaborales,
                                fechaBaja: fechaSeleccionada
                            }), // Envía los parámetros en formato JSON
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (response) {
                                // Maneja la respuesta del servidor si es necesario.
                                window.location.href = window.location.href;
                                console.log('Evento de dar de baja activado.');
                            },
                            error: function (xhr, status, error) {
                                // Maneja los errores si es necesario.
                                console.error('Error al activar el evento de dar de baja: ' + error);
                            }
                        });
                    }
                }
            });
        }

        function validarPrestaciones(input) {
            // Reemplaza cualquier valor no numérico o negativo por 0.00
            input.value = input.value.replace(/[^0-9.]/g, '');
            if (input.value < 0) {
                input.value = '0.00';
            }
        }


        function mostrarAlertaDesactivado() {
            Swal.fire({
                title: 'Este empleado se encuentra desactivado',
                icon: 'warning',
                html: 'El empleado está desactivado y no puede realizar tareas en este momento. ¿Deseas imprimir el descargo?',
                showCancelButton: true,
                cancelButtonText: 'Cancelar',
                confirmButtonText: 'Imprimir Descargo',
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    $.ajax({
                        type: 'POST',
                        url: 'fichaEmpleado.aspx/imprimirDescargo', // Ajusta la URL según tu estructura.
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (response) {
                            // Realiza acciones adicionales después de la impresión, si es necesario.
                            console.log('Descargo impreso con éxito.');
                        },
                        error: function (xhr, status, error) {
                            // Maneja los errores si es necesario.
                            console.error('Error al imprimir el descargo: ' + error);
                        }
                    });

                }
            });
        }






    </script>
</asp:Content>
