<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true"  EnableViewState="true" CodeBehind="fichaEmpleado.aspx.cs" Inherits="MiGente_Front.Empleador.fichaEmpleado" %>

<%@ Register TagPrefix="uc" TagName="ModalEmpleado" Src="~/UserControls/FormularioEmpleado.ascx" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="sm"></asp:ScriptManager>
    <div class="pagetitle">
        <h1>Gestión de Colaboradores</h1>
        <nav>
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="colaboradores.aspx">Colaboradores</a></li>
                <li class="breadcrumb-item"><a>Ficha de Colaborador</a></li>

            </ol>

        </nav>

    </div>


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
                <div id="divMensajeInactivo" runat="server" class="alert alert-danger" style="display: none">
                    <h4 class="alert-heading">Este empleado se encuentra desactivado</h4>
                    <p>El empleado está desactivado y no puede realizar tareas en este momento. ¿Deseas imprimir el descargo?</p>
                    <button id="btnImprimirDescargo" runat="server" onserverclick="btnImprimirDescargo_ServerClick" class="btn btn-primary">Imprimir Descargo</button>
                </div>

                <div class="row">

                    <div class="col col-md-3 col-12 justify-content-center">

                        <asp:Image ID="Image1" ImageUrl="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png" runat="server" Width="280px" ImageAlign="Middle"
                            CssClass="shadow mt-2" />
                        <hr />
                        <h3 runat="server" id="NombreEmpleado">Empleado</h3>

                    </div>
                    <div class="col col-md-9 col-12 mt-1">
                        <div class="card-box">
                            <h4 class="card-title">Información del Empleado</h4>
                            <hr>
                            <div class="row">
                                <div class="col-md-4">
                                    <p><strong>Fecha de Registro:</strong> <span runat="server" id="fechaRegistro"></span></p>
                                    <p><strong>Fecha de Inicio:</strong> <span runat="server" id="fechaInicio"></span></p>
                                    <p><strong>Identificación:</strong> <span runat="server" id="identificacion"></span></p>
                                    <p><strong>Nombre:</strong> <span runat="server" id="nombre"></span></p>
                                    <p><strong>Salario:</strong> <span runat="server" id="salario"></span></p>

                                </div>

                                <div class="col-md-4">
                                    <p><strong>Direccion:</strong> <span runat="server" id="htmlDireccion"></span></p>
                                    <p><strong>Provincia:</strong> <span runat="server" id="htmlProvincia"></span></p>
                                    <p><strong>Municipio:</strong> <span runat="server" id="htmlMunucipio"></span></p>
                                    <p><strong>Contacto Emergencia:</strong> <span runat="server" id="htmlEmergencia"></span></p>
                                    <dx:BootstrapCheckBox ID="chkTss" ClientReadOnly="true" Text="Aplica para deducciones TSS" runat="server"></dx:BootstrapCheckBox>
                                </div>
                                <div class="col-md-4">
                                    <p><strong>Teléfono 1:</strong> <span runat="server" id="telefono1"></span></p>
                                    <p><strong>Teléfono 2:</strong> <span runat="server" id="telefono2"></span></p>
                                    <p><strong>Período de Pago:</strong> <span runat="server" id="periodoPago"></span></p>
                                    <p><strong style="color: red">Contrato:</strong> <a runat="server" class="btn btn-primary" id="btnContrato" onserverclick="btnContrato_ServerClick" style="color: white; font-weight: bold"><span runat="server" id="contrato"></span></a></p>
                                    <dx:BootstrapCheckBox ID="chkActivo" ClientReadOnly="true" Text="Activo" runat="server"></dx:BootstrapCheckBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Image ID="Image2" runat="server" />
                                </div>
                            </div>
                            <div class="row mt-3">
                                <div class="col-md-12">
                                    <button runat="server" class="mt-1 col-md-2 col-12 btn btn-primary me-2" id="btnEditarPerfil" type="button" onserverclick="btnEditarPerfil_ServerClick">
                                        <i class="bi bi-pencil"></i>Editar</button>
                                    <button type="button" onserverclick="btnBaja_ServerClick" class="mt-1 col-md-2  col-12 btn btn-danger me-2" id="btnBaja" runat="server">
                                        <i class="bi bi-download"></i>Dar de Baja</button>

                                    <button runat="server" class="mt-1  col-md-3  col-12 btn btn-info text-white" type="button" data-bs-toggle="modal" id="btnNota" data-bs-target="#agregarNotaModal">
                                        <i class="bi bi-book"></i>Agregar Nota</button>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 mt-1">
                        <div class="card-box">
                            <h4 class="card-title">Detalle de Pagos</h4>
                            <div class="row">
                                <div class="col-md-3 mt-1">
                                    <dx:BootstrapButton runat="server" Width="100%" ID="btnRealizarPago" AutoPostBack="false" OnClick="btnRealizarPago_Click"  CssClasses-Control="bg-primary" CssClasses-Icon="bi bi-cash" Text="Pago de Salario"></dx:BootstrapButton>
                                </div>
                                <div class="col-md-3 mt-1">
                                    <dx:BootstrapButton runat="server" Width="100%" ID="btnRegalia" CssClasses-Control="bg-warning text-black" OnClick="btnRegalia_Click" CssClasses-Icon="bi bi-cash" Text="Regalia Pascual"></dx:BootstrapButton>

                                </div>
                            </div>

                            <h6 class="card-title">Historial</h6>
                            <hr />
                              <asp:Repeater ID="repeaterPagos"  runat="server">
                <HeaderTemplate>
                    <table class="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <th style="display: none;">ID</th>
                                <th>Fecha de Registro</th>
                                <th>Fecha de Pago</th>
                                <th>Tipo</th>
                                <th>Monto</th>

                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("fechaRegistro") %></td>
                        <td><%# Eval("fechaPago") %></td>
                        <td><%# Eval("tipo") %></td>
                        <td><%# Convert.ToDecimal(Eval("Total")).ToString("N2") %></td>

                        <td>
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="Imprimir" CommandName="Edit" OnClick="Unnamed_Click" CommandArgument='<%# Eval("pagoID") %>' />

                           <asp:Button 
    ID="btnEliminarRecibo" 
    runat="server" 
    CssClass="btn btn-danger" 
    Text="Eliminar Recibo" 
    CommandArgument='<%# Eval("pagoID")  %>' 
    OnClientClick="confirmarEliminacion(this); return false;" 
                                 data-pago-id='<%# Eval("pagoID") %>'/>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
        </table>
                </FooterTemplate>
            </asp:Repeater>

                        </div>
                    </div>

                    <div class="col-md-12 mt-1">
                        <div class="card-box" style="height: 300px">
                            <h4 class="card-title">Historial de Notas</h4>

                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>

    <!-- Modal Nota -->
    <div class="modal fade" id="agregarNotaModal" tabindex="-1" aria-labelledby="agregarNotaModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="agregarNotaModalLabel">Agregar Nota</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtNota" MaxLength="200" runat="server" CssClass="form-control" placeholder="Ingrese la nota"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Nota" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="modalPago" tabindex="-1" aria-labelledby="modalPagoModalLabel" data-backdrop="static" aria-hidden="true">
        <div class=" modal-dialog modal-lg">

            <div class="modal-content">
                <div class="modal-header bg-success text-white">
                    <h4>Realizar Pago a empleado</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanelModal" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-4 col-12">
                                    <dx:BootstrapComboBox runat="server" EnableViewState="true" Enabled="false" ValueType="System.String" Caption="Concepto"  ID="cbConcepto">
                                        <Items>
                                            <dx:BootstrapListEditItem Value="Salario" Text="Salario"></dx:BootstrapListEditItem>
                                            <dx:BootstrapListEditItem Value="Regalia" Text="Regalia Pascual"></dx:BootstrapListEditItem>
                                        </Items>
                                    </dx:BootstrapComboBox>
                                </div>
                                 <div class="col-md-4 col-12">
                                    <dx:BootstrapDateEdit runat="server" Caption="Fecha de Pago" ID="fechaPago">
                                      
                                    </dx:BootstrapDateEdit>
                                </div>
                                <div class=" col-md-4 col-12">
                                       <dx:BootstrapComboBox runat="server" Caption="Periodo" AutoPostBack="true" OnSelectedIndexChanged="cbPeriodo_SelectedIndexChanged" ValueType="System.Int32" ID="cbPeriodo">
                                        <Items>
                                            <dx:BootstrapListEditItem Value="1" Selected="true" Text="Periodo Completo"></dx:BootstrapListEditItem>
                                            <dx:BootstrapListEditItem Value="2" Text="Fraccion de Periodo"></dx:BootstrapListEditItem>
                                        </Items>
                                    </dx:BootstrapComboBox>
                                   
                                </div>
                                <hr class="mt-4" />
                        
                            <label class="bold text-danger" >Descuentos</label>
                            <div class="row align-content-center">

                                <div class="col-md-7 col-12" title="Descripcion de Descuento">
                                    <dx:BootstrapTextBox CssClasses-Caption="text-danger" ValidationSettings-RequiredField-IsRequired="true" Caption="Concepto de Descuento" ValidationSettings-ValidationGroup="nuevoDescuento" ID="descripcionDescuento" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col-md-2 col-9 " title="Monto">
                                    <dx:BootstrapTextBox ID="montoDescuento" CssClasses-NullText="text-danger"  CssClasses-Caption="text-danger" Caption="Monto" NullText="0.00" DisplayFormatString="{0:n2}" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-ValidationGroup="nuevoDescuento" runat="server">
                                    </dx:BootstrapTextBox>
                                </div>
                                <div class="col-md-3 col-2  align-content-end">
                                    <dx:BootstrapButton ID="btnNuevoDescuento" OnClick="btnNuevoDescuento_Click" ValidationGroup="nuevoDescuento"
                                        CausesValidation="true" CssClasses-Control="bg-danger" runat="server" Text="Agregar" AutoPostBack="false" CssClasses-Icon="bi bi-plus"></dx:BootstrapButton>
                                </div>
                                </div>

                                <a class="mt-3">Detalle</a>
                                <br />
                                <dx:BootstrapGridView ID="gridDetallePago" runat="server" AutoGenerateColumns="False">
                                    <Settings ShowFooter="True"></Settings>
                                    <Columns>
                                        <dx:BootstrapGridViewTextColumn Caption="Concepto" Width="70%" FieldName="Concepto"></dx:BootstrapGridViewTextColumn>
                                        <dx:BootstrapGridViewTextColumn Caption="Monto" PropertiesTextEdit-DisplayFormatString="N2" FieldName="Monto"></dx:BootstrapGridViewTextColumn>

                                    </Columns>
                                    <TotalSummary>
                                        <dx:ASPxSummaryItem ShowInColumn="Monto" SummaryType="Sum" FieldName="Monto" ValueDisplayFormat="{0:c2}" Tag="Monto a Pagar"></dx:ASPxSummaryItem>
                                    </TotalSummary>
                                </dx:BootstrapGridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <div class="avatar-group">
                        <button data-bs-dismiss="modal" class="btn btn-danger">Cerrar</button>
                        <dx:BootstrapButton ID="btnGenerar" ValidationGroup="nuevoPago" CausesValidation="true"  CssClasses-Icon="bi bi-save" OnClick="btnGenerar_Click" CssClasses-Control="bg-success" runat="server" AutoPostBack="false" Text="Procesar Pago"></dx:BootstrapButton>
                    </div>
                </div>
            </div>
        </div>
        <a href=""></a>
    </div>
    <uc:ModalEmpleado ID="ModalEmpleado" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />

    <script type="text/javascript">
        function confirmarEliminacion(button) {
            var pagoID = button.getAttribute("data-pago-id"); // Obtener el pagoID del atributo data-pago-id
            console.log(pagoID);
            Swal.fire({
                title: '¿Estás seguro?',
                text: "¿Deseas eliminar el recibo?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Llamar al WebMethod en C#
                    eliminarRecibo(pagoID);
                }
            });
        }

        function eliminarRecibo(pagoID) {
            $.ajax({
                type: "POST",
                url: "fichaEmpleado.aspx/EliminarRecibo",
                data: JSON.stringify({ pagoID: parseInt(pagoID) }), // Asegurar que pagoID sea un número
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    Swal.fire({
                        title: 'Éxito',
                        text: response.d, // Mensaje desde el WebMethod
                        icon: 'success',
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        // Recargar la página después de eliminar el recibo
                        window.location.href = window.location.href;
                    });
                },
                error: function (error) {
                    Swal.fire({
                        title: 'Error',
                        text: 'Hubo un problema al eliminar el recibo.',
                        icon: 'error',
                        confirmButtonText: 'Aceptar'
                    });
                }
            });
        }


        function DarDeBajaEmpleado(empleadoID) {
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
                                empleadoID:empleadoID,
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
