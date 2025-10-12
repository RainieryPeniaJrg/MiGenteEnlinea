<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" CodeBehind="MiPerfilEmpleador.aspx.cs" Inherits="MiGente_Front.Empleador.MiPerfilEmpleador" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="sm"></asp:ScriptManager>
    <div class="pagetitle">
        <h1>Gestión de Perfil</h1>
        <nav>
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Mi Perfil</a></li>
            </ol>
        </nav>

    </div>
    <div class="row bg-white p-3">


        <div class="col col-md-3 col-12" runat="server" id="divBoton">
            <asp:LinkButton ID="btnAddNew" Width="100%" runat="server" CssClass="btn btn-outline-primary" OnClick="btnAddNew_Click">
              <i class="bi bi-plus-circle"></i> Nuevo Usuario</asp:LinkButton>
        </div>
        <div class="col col-md-12 col-12" visible="false" runat="server" id="divAgotado">
            <label style="color: darkred">ATENCION: Ya ha agotado la cantidad de usuarios permitidos en este plan.</label>
        </div>


    </div>
    <hr />
    <ul class="nav nav-pills mb-3 mt-3" id="pills-tab" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active" id="pills-profile-tab" data-bs-toggle="pill" data-bs-target="#pills-profile" type="button" role="tab" aria-controls="pills-profile" aria-selected="true">
                Datos de Cuenta</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="pills-users-tab" data-bs-toggle="pill" data-bs-target="#pills-users" type="button" role="tab" aria-controls="pills-users" aria-selected="false">Credenciales de Acceso</button>
        </li>

    </ul>

    <div class="tab-content" id="pills-tabContent">

        <%-- Empresa --%>
        <div class="tab-pane fade show active" id="pills-profile" role="tabpanel" aria-labelledby="pills-profile-tab">

            <div class="row">
                <div class="col-lg-12">

                    <div class="card" style="overflow: auto; white-space: nowrap">
                        <div class="card-body">
                            <h5 class="card-title">Mi Perfil</h5>

                            <div>
                                <div class="row">



                                    <div class="mb-3 col col-md-2">
                                        <label for="tipoCuenta" class="form-label">Tipo de Cuenta</label>
                                        <asp:DropDownList ID="tipoCuenta" Enabled="false" runat="server" CssClass="form-select">
                                            <asp:ListItem Text="Empleador" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Contratista" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanelModal" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="mb-3 col col-md-3 mt-2">
                                                    <label for="tipoIdentificacion" class="form-label">Tipo de Perfil</label>
                                                    <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlTipoIdentificacion_SelectedIndexChanged" ID="ddlTipoIdentificacion" runat="server" CssClass="form-select">
                                                        <asp:ListItem Text="Persona Fisica" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Empresa" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="mb-3 col col-md-3 col-12">


                                                    <dx:BootstrapTextBox ID="Nombre" ValidationSettings-RequiredField-IsRequired="true" Caption="Nombre" ValidationSettings-ValidationGroup="perfil" NullText="Ingrese el Nombre" NullTextDisplayMode="UnfocusedAndFocused" runat="server"></dx:BootstrapTextBox>

                                                </div>
                                                <div class="mb-3 col col-md-3 col-12">
                                                    <dx:BootstrapTextBox ID="Apellido" ValidationSettings-RequiredField-IsRequired="true" Caption="Apellido" ValidationSettings-ValidationGroup="perfil" NullText="Ingrese el Apellido" NullTextDisplayMode="UnfocusedAndFocused" runat="server"></dx:BootstrapTextBox>

                                                </div>
                                                <div class="mb-3 col col-md-3 col-12">

                                                    <dx:BootstrapTextBox ValidationSettings-RequiredField-IsRequired="true" Caption="RNC/Cedula" ValidationSettings-ValidationGroup="perfil" ID="txtIdentificacion" runat="server"></dx:BootstrapTextBox>
                                                </div>
                                                <div runat="server" id="divEmpresa" visible="false" class="mb-3 row">
                                                    <div class="mb-3 col col-md-6 col-12">

                                                        <dx:BootstrapTextBox ValidationSettings-RequiredField-IsRequired="true" Caption="Nombre Comercial" ID="nombreComercial" ValidationSettings-ValidationGroup="perfil" NullText="Nombre Comercial" Enabled="false" runat="server"></dx:BootstrapTextBox>
                                                    </div>
                                                    <div class="mb-3 col col-md-12">
                                                        <h4 class="col-12">Representante Legal</h4>

                                                    </div>
                                                    <div class="mb-3 col col-md-3 col-12">

                                                        <dx:BootstrapTextBox ValidationSettings-RequiredField-IsRequired="true" Caption="Nombre" ID="txtNombreGerente" ValidationSettings-ValidationGroup="perfil" NullText="Nombre Representante Legal" Enabled="false" runat="server"></dx:BootstrapTextBox>
                                                    </div>
                                                    <div class="mb-3 col col-md-3 col-12">

                                                        <dx:BootstrapTextBox ValidationSettings-RequiredField-IsRequired="true" Caption="Apellido" ID="txtApellidoGerente" ValidationSettings-ValidationGroup="perfil" NullText="Apellido Representante Legal" Enabled="false" runat="server"></dx:BootstrapTextBox>
                                                    </div>
                                                    <div class="mb-3 col col-md-12 col-12">

                                                        <dx:BootstrapTextBox ValidationSettings-RequiredField-IsRequired="true" ID="txtDireccionGerente" Caption="Direccion del Gerente" ValidationSettings-ValidationGroup="perfil" NullText="Direccion" MaxLength="250" Enabled="false" runat="server"></dx:BootstrapTextBox>
                                                    </div>
                                                </div>
                                                <div class="mb-3 col col-md-4 col-12">

                                                    <dx:BootstrapTextBox ValidationSettings-RequiredField-IsRequired="true" Caption="Email" ValidationSettings-ValidationGroup="perfil" ID="email" runat="server"></dx:BootstrapTextBox>
                                                </div>
                                                <div class="mb-3 col col-md-3 col-12">

                                                    <dx:BootstrapTextBox ID="telefono1" MaskSettings-Mask="(999)-000-0000" Caption="Telefono 1" runat="server"></dx:BootstrapTextBox>
                                                </div>
                                                <div class="mb-3 col col-md-3 col-12">
                                                    <dx:BootstrapTextBox ID="telefono2" MaskSettings-Mask="(999)-000-0000" Caption="Telefono 2" runat="server"></dx:BootstrapTextBox>

                                                </div>
                                                <div class="mb-3 col-md-12 col-12">
                                                    <dx:BootstrapTextBox ValidationSettings-RequiredField-IsRequired="true" Caption="Direccion de Cliente" ValidationSettings-ValidationGroup="perfil" ID="direccion" MaxLength="250" runat="server"></dx:BootstrapTextBox>

                                                </div>
                                                <dx:BootstrapButton ID="btnGuardar" CssClasses-Control="bg-success" runat="server" ValidationGroup="perfil" CausesValidation="true" OnClick="btnGuardar_Click" AutoPostBack="true" Text="Actualizar Perfil"></dx:BootstrapButton>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <hr />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="pills-users" role="tabpanel" aria-labelledby="pills-users-tab">

            <div class="row">
                <div class="col-lg-12">

                    <div class="card" style="overflow: auto; white-space: nowrap">
                        <div class="card-body">
                            <h5 class="card-title">Credenciales de Acceso</h5>

                            <!-- Table with stripped rows -->
                            <div class="datatable-wrapper datatable-loading no-footer sortable searchable fixed-columns">
                                <input type="text" id="search" class="form-control" placeholder="Buscar...">

                                <div class="datatable-container">
                                    <table class="table datatable">
                                        <thead>
                                            <tr>
                                                <th>Acción</th>
                                                <th>Email</th>
                                                <th>Password</th>
                                            </tr>
                                        </thead>
                                        <tbody id="usersTableBody">
                                            <tr id="loadingRow">
                                                <td colspan="12" class="text-center">
                                                    <div class="spinner-border" role="status">
                                                        <span class="visually-hidden">Cargando...</span>
                                                    </div>
                                                </td>
                                            </tr>
                                            <!-- Los datos se llenarán aquí dinámicamente -->
                                        </tbody>
                                    </table>
                                </div>
                                <!-- Pagination -->
                                <nav aria-label="Page navigation">
                                    <ul class="pagination" id="pagination">
                                        <!-- Los botones de paginación se llenarán aquí dinámicamente -->
                                    </ul>
                                </nav>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade modal-sm" id="modalCrearUsuario" tabindex="-1" aria-labelledby="modalCrearUsuarioLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="modalCrearUsuarioLabel">Crear Usuario</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Email</label>
                        <dx:BootstrapTextBox ID="txtEmail" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-ValidationGroup="nu" runat="server" Width="100%" ClientInstanceName="txtEmail" />
                    </div>

                    <div class="mb-3">
                        <label for="txtPassword" class="form-label">Contraseña</label>
                        <dx:BootstrapTextBox ID="txtPassword" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-ValidationGroup="nu" runat="server" Width="100%" Password="true"
                            ClientInstanceName="txtPassword" />
                    </div>

                    <div class="form-check">
                        <dx:BootstrapCheckBox ID="chkActivo" Checked="true" runat="server" Text="Activo" ClientInstanceName="chkActivo" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="Button1_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Bootstrap -->
    <div class="modal fade" id="resetPasswordModal" tabindex="-1" aria-labelledby="resetPasswordLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="resetPasswordLabel">Restablecer Contraseña</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="emailInput" class="form-label">Email</label>

                        <dx:BootstrapTextBox ID="emailTextBox" ClientEnabled="true" runat="server" Enabled="false" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button id="resetPasswordBtn" type="button" runat="server" onserverclick="resetPasswordBtn_ServerClick" class="btn btn-primary">Restablecer</button>
                </div>
            </div>
        </div>
    </div>

      <!-- Modal Bootstrap -->
    <div class="modal fade" id="deleteUserModal" tabindex="-1" aria-labelledby="deleteUserModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="deleteUserModalLabel">Eliminar Usuario</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                       <label style="color:red">Seguro que desea continuar con la accion?</label>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button id="btnDeleteUser" type="button" runat="server" onserverclick="btnDeleteUser_ServerClick" class="btn btn-primary">Eliminar Usuario</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hiddenEmailField" runat="server" />
    <asp:HiddenField ID="hiddenField1" runat="server" />
    <asp:HiddenField ID="hiddenUsuarioID" runat="server" />

    <script>

        $(document).ready(function () {



            var pageIndex = 1;
            var pageSize = 10;
            var hiddenFieldValue = document.getElementById('<%= hiddenField1.ClientID %>').value;
            // Función para cargar los datos
            function loadUsers(pageIndex, searchTerm) {
                $("#loadingRow").show();
                $.ajax({
                    type: "POST",
                    url: "miPerfilEmpleador.aspx/obtenerUsuarios",
                    data: JSON.stringify({ userID: hiddenFieldValue }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        console.log("test");
                        var data = response.d;
                        var credenciales = data.credenciales;
                        var totalRecords = data.totalRecords;

                        // Limpiar la tabla
                        $("#usersTableBody").empty();

                        // Llenar la tabla con los datos recibidos
                        $.each(credenciales, function (index, credencial) {
                            console.log('../' + credencial.imagePath);
                            $("#usersTableBody").append(`
                                <tr>
                      

                                    <td>
                                        <div class="dropdown">
                                            <button class="btn btn-primary btn-sm dropdown-toggle" type="button" id="dropdownMenuButton${credencial.id}" data-bs-toggle="dropdown" aria-expanded="false">
                                                Acciones
                                            </button>
                                            <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton${credencial.id}">
                                             <li> <button
            class="dropdown-item reset-password"
            type="button"
            data-credenciales-email="${credencial.email}">
            <i class="bi bi-key"></i> Reestablecer Contraseña
        </button></li>
                                               
                                                <hr/>
                                                <li class="bg-danger text-white"><button 
            class="dropdown-item delete-user"
            type="button"
            data-credenciales-id="${credencial.id}">
              <i class="bi bi-trash"></i> Eliminar Usuario
        
                                                 
                                                </button></li>
                                            </ul>
                                        </div>
                                    </td>
                                    <td>${credencial.email}</td>
                                    <td>${credencial.password}</td>
                                    <td>${credencial.activo}</td>
                            
                                    </tr>
                            `);
                        });

                        // Actualizar la paginación
                        var totalPages = Math.ceil(totalRecords / pageSize);
                        updatePagination(totalPages, pageIndex);
                    },
                    complete: function () {
                        // Ocultar el spinner de carga cuando la solicitud haya terminado
                        $("#loadingRow").hide();
                    },
                    error: function () {
                        // Mostrar un mensaje de error si la solicitud falla
                        $("#usersTableBody").empty().append(`
                            <tr>
                                <td colspan="12" class="text-center text-danger">Error al cargar los datos.</td>
                            </tr>
                        `);
                        // Ocultar el spinner
                        $("#loadingRow").hide();
                    }
                });
            }
            // Función para actualizar los botones de paginación
            function updatePagination(totalPages, currentPage) {
                $("#pagination").empty();

                for (var i = 1; i <= totalPages; i++) {
                    var activeClass = (i === currentPage) ? 'active' : '';
                    $("#pagination").append(`<li class="page-item ${activeClass}"><a class="page-link" href="#">${i}</a></li>`);
                }

                // Añadir evento click para cambiar de página
                $(".page-link").click(function (e) {
                    e.preventDefault();
                    var newPage = parseInt($(this).text());
                    pageIndex = newPage;
                    loadUsers(pageIndex, $("#search").val());
                });
            }

            // Evento para la búsqueda
            $("#search").on("input", function () {
                loadUsers(1, $(this).val()); // Resetear a la página 1 al hacer una búsqueda
            });

            // Cargar los datos inicialmente
            loadUsers(pageIndex, "");


            $("#usersTableBody").on("click", ".reset-password", function (e) {
                e.preventDefault();
                var email = $(this).data("credenciales-email"); // Obtener el email
                console.log(email);
                // Asignar el email al BootstrapTextBox
                $("#<%= emailTextBox.ClientID %>").text(email);
                $("#<%= emailTextBox.ClientID %>").val(email);
                $("#<%= hiddenEmailField.ClientID %>").val(email);

                // Guardar el ID del usuario en el modal
                $("#resetPasswordModal").data("userId", userId).modal("show");
            });
            $("#usersTableBody").on("click", ".delete-user", function (e) {
                e.preventDefault();
                var userId = $(this).data("credenciales-id"); // Obtener el ID del usuario
                console.log(userId);
                // Asignar el email al BootstrapTextBox
                $("#<%= hiddenUsuarioID.ClientID %>").val(userId);

                // Guardar el ID del usuario en el modal
                $("#deleteUserModal").data("userId", userId).modal("show");
            });

        });

    </script>
</asp:Content>
