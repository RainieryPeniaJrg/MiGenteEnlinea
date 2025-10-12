<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormularioEmpleado.ascx.cs" EnableViewState="true"  Inherits="MiGente_Front.FormularioEmpleado" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<style>
    .modal {
        z-index: 9999;
    }

    .swal2-container {
        z-index: 10000;
    }
</style>
<!-- SweetAlert2 CSS -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10/dist/sweetalert2.min.css">

<!-- SweetAlert2 JavaScript -->
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@10/dist/sweetalert2.min.js"></script>

<div class="modal fade modal-xl" id="modalEmpleado" data-bs-keyboard="false" tabindex="-1" data-backdrop="static" aria-labelledby="modalEmpleado" aria-hidden="true">

    <div class="modal-dialog">


        <div class="modal-content">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-header text-white " style="background-color: #0088BF">
                        <h5 class="modal-title" runat="server" id="modalTitle">Registro Empleado</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div class="modal-body">

                        <div class="container-fluid">
                            <div class="container p-4 bg-light shadow rounded">
                                <div class="row align-items-center g-3">

                                    <!-- Spinner de Progreso -->
                                    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                                        <ProgressTemplate>
                                            <div class="d-flex justify-content-center align-items-center" style="height: 50px;">
                                                <div class="spinner-border text-primary me-2" role="status">
                                                    <span class="visually-hidden">Cargando...</span>
                                                </div>
                                                <span class="text-secondary">Procesando...</span>
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                    <!-- Fecha Inicio -->
                                    <div class="col-md-3 col-12">
                                        <dx:BootstrapDateEdit ID="dtIngreso" Caption="Fecha Inicio" runat="server" />
                                    </div>

                                    <!-- No. Identificación -->
                                    <div class="col-md-3 col-12">
                                        <dx:BootstrapTextBox ID="txtIdentificationNo" Caption="No. Identificación *" runat="server" />
                                    </div>

                                    <!-- Botón Validar -->
                                    <div class="col-md-3 col-12 d-grid " style="margin-top: 50px;">

                                        <dx:BootstrapButton ID="btnValidateID" runat="server" CssClasses-Icon="bi bi-check-lg"
                                            OnClick="btnValidateID_Click" AutoPostBack="false" Text="Validar"
                                            CssClasses-Control="btn btn-primary btn-block " />
                                    </div>

                                    <!-- Imagen -->
                                    <div class="col-md-3 col-12 text-center" id="divFoto" runat="server" visible="false">
                                        <asp:Image ID="Image1" runat="server" Width="130px" ImageAlign="Middle"
                                            CssClass="shadow mt-2" />
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row g-1">

                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtNombre" Caption="Nombre *" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtApellido" Caption="Apellido *" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtAlias" Caption="Alias/Apodo" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapComboBox ID="cbEstadoCivil"  EnableViewState="true" ValueType="System.Int32" Caption="Estado Civil " runat="server">
                                        <Items>
                                            <dx:BootstrapListEditItem Value="1" Text="Soltero" Selected="true"></dx:BootstrapListEditItem>
                                            <dx:BootstrapListEditItem Value="2" Text="Casado"></dx:BootstrapListEditItem>
                                        </Items>
                                    </dx:BootstrapComboBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapDateEdit ID="dtNacimiento" Caption="Fecha Nacimiento *" runat="server"></dx:BootstrapDateEdit>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtTelefono1" Caption="Telefono 1 *" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtTelefono2" Caption="Telefono 2" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-6 col-12">
                                    <dx:BootstrapTextBox ID="txtDireccion" Caption="Dirección *" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtProvincia" Caption="Provincia *" runat="server">
                                    </dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtMunicipio" Caption="Municipio *" runat="server">
                                    </dx:BootstrapTextBox>
                                </div>
                            </div>
                            <h7 class="mt-1" style="font-weight: bold">Contacto de Emergencia</h7>
                            <div class="row">
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtNombreEmergencia" Caption="Nombre *" runat="server">
                                    </dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapTextBox ID="txtTelefonoEmergencia" Caption="Telefono *" runat="server">
                                    </dx:BootstrapTextBox>
                                </div>
                            </div>
                            <hr />
                            <h7 style="font-weight: bold">Detalles de Contratacion</h7>

                            <div class="row mt-1">

                                <div class="col col-md-3 col-12">
                                    <dx:BootstrapTextBox ID="txtPosicion" Caption="Posicion que Ocupa *" runat="server"></dx:BootstrapTextBox>
                                </div>

                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapSpinEdit ID="txtSalario"  MinValue="0" Number="0" SpinButtons-Enabled="false" SpinButtons-ClientVisible="false" AllowMouseWheel="false" ClearButton-DisplayMode="Never" NumberType="Float" CssClasses-Input="text-center" Caption="Salario Bruto *" runat="server"></dx:BootstrapSpinEdit>

                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapComboBox ID="txtPeriodoPago" Caption="Periodo de Pago" ValueType="System.Int32" EnableViewState="true" runat="server">
                                   
                                    </dx:BootstrapComboBox>

                                </div>
                                <div class="col col-md-3 col-12 align-content-end" runat="server" id="divDeducciones" visible="false">
                                    <dx:BootstrapCheckBox ID="chkDeducciones" Text="Aplica para Deducciones TSS" CssClasses-Control="form-check" runat="server"></dx:BootstrapCheckBox>
                                </div>
                            </div>
                            <hr style="color: lightgray" />
                            <h8 style="font-weight: bold">Otras Remuneraciones</h8>
                            <div class="row mt-1 g-1">
                                <div class="col col-md-6 col-12">
                                    <dx:BootstrapTextBox ID="txtDescOtras" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-ValidationGroup="or" Caption="Descripcion" runat="server"></dx:BootstrapTextBox>
                                </div>

                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapSpinEdit ID="txtMontoOtras" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-ValidationGroup="or" MinValue="0" Number="0" SpinButtons-Enabled="false" SpinButtons-ClientVisible="false" AllowMouseWheel="false" ClearButton-DisplayMode="Never" NumberType="Float" CssClasses-Input="text-center" Caption="Monto" runat="server"></dx:BootstrapSpinEdit>

                                </div>
                                <div class="col col-md-2 col-12 align-content-end">
                                    <dx:BootstrapButton ID="btnAddOtras" CssClasses-Control="bg-success" OnClick="btnAddOtras_Click" ValidationGroup="or" CssClasses-Icon="bi bi-plus" runat="server" AutoPostBack="false" Text="Agregar"></dx:BootstrapButton>

                                </div>

                                <asp:Repeater ID="repeaterRemuneraciones" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Descripcion</th>
                                                    <th>Monto</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("descripcion") %></td>
                                            <td><%# Eval("monto") %></td>
                                            <td class=" mx-auto">
                                                <asp:LinkButton runat="server" ID="btnDeleteOtras" CssClass="btn btn-danger"
                                                    CommandArgument='<%# Eval("id") %>' OnClick="btnDeleteOtras_Click">
                                                                <i class="bi bi-trash"></i>
                                                </asp:LinkButton>
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


                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cerrar</button>

                        <dx:BootstrapButton ID="btnSave" CssClasses-Control="bg-success" ValidationGroup="nc" CausesValidation="true" runat="server" OnClick="btnSave_Click" AutoPostBack="false" Text="Completar Registro"></dx:BootstrapButton>
                    </div>
                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>
    <script>

        function validateForm() {


            var alertToast = new bootstrap.Toast(document.getElementById('alertToast'));
            alertToast.show();
            return false; // Evitar el envío del formulario si los campos están vacíos
        }

    </script>
</div>
