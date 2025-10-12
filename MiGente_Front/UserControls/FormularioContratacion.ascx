<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormularioContratacion.ascx.cs" Inherits="MiGente_Front.UserControls.FormularioContratacion" %>
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

<div class="modal fade modal-xl" id="modalContratacion" data-bs-keyboard="false" tabindex="-1" data-backdrop="static" aria-labelledby="modalEmpleado" aria-hidden="true">

    <div class="modal-dialog">


        <div class="modal-content">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-header text-white " style="background-color: darkgrey">
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
                                    <div class="col col-md-3 col-12">
                                        <dx:BootstrapComboBox ID="cbTipo" AutoPostBack="true"  OnSelectedIndexChanged="cbTipo_SelectedIndexChanged" EnableViewState="true" ValueType="System.Int32" Caption="Tipo de Registro" runat="server">
                                            <Items>
                                                <dx:BootstrapListEditItem Value="1" Text="Persona Fisica" Selected="true"></dx:BootstrapListEditItem>
                                                <dx:BootstrapListEditItem Value="2" Text="Empresa"></dx:BootstrapListEditItem>
                                            </Items>
                                        </dx:BootstrapComboBox>
                                    </div>
                                    <!-- Fecha Inicio -->
                                    <div class="col-md-2 col-12">
                                        <dx:BootstrapDateEdit ID="dtIngreso" Caption="Fecha Inicio" runat="server" />
                                    </div>

                                    <!-- Imagen -->
                                    <div class="col-md-7 col-12 text-end justify-content-end" id="divFoto" runat="server">
                                        <asp:Image ID="Image1" runat="server" Width="110px" ImageAlign="Middle"
                                            CssClass="shadow mt-2" />
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row g-1">

                                <div id="divPersona" class="row" runat="server" visible="false">
                                    <!-- No. Identificación -->
                                    <div class="col-md-2 col-12">
                                        <dx:BootstrapTextBox ID="txtIdentificationNo" Caption="No. Identificación *" runat="server" />
                                    </div>

                                    <!-- Botón Validar -->
                                    <div class="col-md-2 col-12 d-grid align-content-end ">

                                        <dx:BootstrapButton ID="btnValidateID" runat="server" CssClasses-Icon="bi bi-check"
                                            AutoPostBack="false" Text="Validar" OnClick="btnValidateID_Click"
                                            CssClasses-Control="bg-success btn-block " />
                                    </div>


                                    <div class="col col-md-3 col-12">
                                        <dx:BootstrapTextBox ID="txtNombre" Caption="Nombre *" runat="server"></dx:BootstrapTextBox>
                                    </div>
                                    <div class="col col-md-3 col-12">
                                        <dx:BootstrapTextBox ID="txtApellido" Caption="Apellido *" runat="server"></dx:BootstrapTextBox>
                                    </div>
                                    <div class="col col-md-2 col-12">
                                        <dx:BootstrapTextBox ID="txtAlias" Caption="Alias/Apodo" runat="server"></dx:BootstrapTextBox>
                                    </div>

                                </div>
                                <div id="divEmpresa" class="row" runat="server" visible="false">

                                    <div class="col col-md-2 col-12">
                                        <dx:BootstrapTextBox ID="txtRNC" Caption="RNC *" runat="server"></dx:BootstrapTextBox>
                                    </div>
                                    <div class="col col-md-3 col-12">
                                        <dx:BootstrapTextBox ID="txtNombreComercial" Caption="Nombre / Razon Social *" runat="server"></dx:BootstrapTextBox>
                                    </div>
                                    <hr class="mt-3" />
                                    <label style="font-weight: bold">Representante Legal</label>
                                    <div class="col col-md-3 col-12">
                                        <dx:BootstrapTextBox ID="txtNombreRepresentante" Caption="Nombre *" runat="server"></dx:BootstrapTextBox>
                                    </div>
                                    <div class="col col-md-3 col-12">
                                        <dx:BootstrapTextBox ID="txtCedulaRepresentante" Caption="Cedula *" runat="server"></dx:BootstrapTextBox>
                                    </div>
                                </div>
                                <div class="row">
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
                            </div>

                            <hr />
                            <h7 style="font-weight: bold">Detalles de Contratacion</h7>
                            <div class="row">
                                <div class="col col-md-3 col-12">
                                    <dx:BootstrapTextBox ID="txtDescripcionCorta" Caption="Descripcion Corta *" MaxLength="100" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-7 col-12">
                                    <dx:BootstrapTextBox ID="txtDescripcionAmplia" Caption="Descripcion Amplia" MaxLength="250" runat="server"></dx:BootstrapTextBox>
                                </div>
                                <div class="col col-md-2 col-12">
                                    <dx:BootstrapDateEdit ID="dtConclusion" Caption="Fecha de Conclusion" runat="server" />

                                </div>
                                <div class="col col-md-3 col-12">
                            <dx:BootstrapSpinEdit ID="txtMonto"  MinValue="0" Number="0" SpinButtons-Enabled="false"
                                SpinButtons-ClientVisible="false" AllowMouseWheel="false" ClearButton-DisplayMode="Never" 
                                NumberType="Float" CssClasses-Input="text-center" Caption="Monto Total Acordado *" runat="server"></dx:BootstrapSpinEdit>

                                </div>
                                <div class="col col-md-4 col-12">
                                    <dx:BootstrapComboBox ID="cbEsquema" Caption="Esquema de Pagos *" runat="server">
                                    <Items>
                                        <dx:BootstrapListEditItem Value="1" Text="100 % Contra Entrega"></dx:BootstrapListEditItem>
                                        <dx:BootstrapListEditItem Value="2" Selected="true" Text="50% Avance/50% Finalizado"></dx:BootstrapListEditItem>
                                        <dx:BootstrapListEditItem Value="3" Text="30% Avance/70% Finalizado"></dx:BootstrapListEditItem>
                                    
                                    </Items>
                                    </dx:BootstrapComboBox>
                                </div>
                            </div>

                        </div>


                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cerrar</button>

                        <dx:BootstrapButton ID="btnSave" CssClasses-Control="bg-success" OnClick="btnSave_Click" ValidationGroup="nc" CausesValidation="true" runat="server" AutoPostBack="false" Text="Completar Registro"></dx:BootstrapButton>
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
    <asp:HiddenField ID="HiddenField1" runat="server" />
</div>
