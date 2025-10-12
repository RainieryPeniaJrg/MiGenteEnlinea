<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" CodeBehind="colaboradores.aspx.cs" Inherits="MiGente_Front.Empleador.colaboradores" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<%@ Register TagPrefix="uc" TagName="ModalEmpleado" Src="~/UserControls/FormularioEmpleado.ascx" %>
<%@ Register TagPrefix="uc" TagName="ModalContratacion" Src="~/UserControls/FormularioContratacion.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="sm"></asp:ScriptManager>
    <div class="pagetitle">
        <h1>Gestión de Colaboradores</h1>
        <nav>
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Colaboradores</a></li>
            </ol>
        </nav>

    </div>
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
    <asp:UpdatePanel runat="server" ID="udt1">
        <ContentTemplate>
            <div class="row bg-white p-3">


                <div class="col col-md-3 col-12">
                    <asp:LinkButton ID="btnAddNew" Width="100%" runat="server" data-bs-toggle="modal" data-bs-target="#modalTipoColaborador" CssClass="btn btn-outline-primary">
              <i class="bi bi-plus-circle"></i> Registrar Nuevo</asp:LinkButton>
                </div>
                <div class="col col-md-3 col-12" id="divNomina" runat="server" visible="false">
                    <asp:LinkButton ID="btnNomina" Width="100%"   runat="server" CssClass="btn btn-outline-warning">
              <i class="bi bi-cash"></i> Gestion de Nomina</asp:LinkButton>
                </div>
                <div class="col col-md-3 col-12">
                    <asp:LinkButton ID="LinkButton2" OnClick="LinkButton2_Click" Width="100%" runat="server" CssClass="btn btn-outline-success">
              <i class="bi bi-people"></i> Consultar Comunidad</asp:LinkButton>
                </div>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <hr />
    <ul class="nav nav-pills mb-3 mt-3" id="pills-tab1" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active" id="pills-fijos-tab" data-bs-toggle="pill" data-bs-target="#pills-fijos" type="button" role="tab" aria-controls="pills-fijos" aria-selected="true">
                Empleados Fijos</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="pills-temp-tab" data-bs-toggle="pill" data-bs-target="#pills-temp" type="button" role="tab" aria-controls="pills-temp" aria-selected="false">Contrataciones Temporales</button>
        </li>

    </ul>
    <div class="tab-content" id="pills-tabContent1">

        <%-- Empresa --%>
        <div class="tab-pane fade show active" id="pills-fijos" role="tabpanel" aria-labelledby="pills-fijos-tab">
            <hr />
            <ul class="nav nav-pills mb-3 mt-3" id="pills-tab" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="pills-active-tab" data-bs-toggle="pill" data-bs-target="#pills-active" type="button" role="tab" aria-controls="pills-active" aria-selected="true">
                        Colaboradores Activos</button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="pills-inact-tab" data-bs-toggle="pill" data-bs-target="#pills-inact" type="button" role="tab" aria-controls="pills-inact" aria-selected="false">Historial</button>
                </li>

            </ul>
            <div class="tab-content" id="pills-tabContent">

                <div class="tab-pane fade show active" id="pills-active" role="tabpanel" aria-labelledby="pills-active-tab">

                    <div class="row">
                        <div class="col-lg-12">

                            <div class="card" style="overflow: auto; white-space: nowrap">
                                <div class="card-body">
                                    <h5 class="card-title">Administra tus Colaboradores</h5>

                                    <!-- Table with stripped rows -->
                                    <div class="datatable-wrapper datatable-loading no-footer sortable searchable fixed-columns" style="min-height: 300px">
                                        <input type="text" id="search" class="form-control" placeholder="Buscar...">

                                        <div class="datatable-container">
                                            <table class="table datatable">
                                                <thead>
                                                    <tr>
                                                        <th>Foto</th>
                                                        <th>Acciónes</th>
                                                        <th>Fecha Inicio</th>
                                                        <th>Identificacion</th>
                                                        <th>Nombre</th>
                                                        <th>Salario</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="colaboradoresTableBody">
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
                                        <%--  <!-- Pagination -->
                                <nav aria-label="Page navigation">
                                    <ul class="pagination" id="pagination">
                                        <!-- Los botones de paginación se llenarán aquí dinámicamente -->
                                    </ul>
                                </nav>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="tab-panel fade" id="pills-inact" role="tabpanel" aria-labelledby="pills-inact-tab">
                    <div class="row">
                        <div class="col-lg-12">

                            <div class="card" style="overflow: auto; white-space: nowrap">
                                <div class="card-body">
                                    <h5 class="card-title " style="color: darkred">Colaboradores Inactivos</h5>

                                    <!-- Table with stripped rows -->
                                    <div class="datatable-wrapper datatable-loading no-footer sortable searchable fixed-columns" style="min-height: 300px">
                                        <input type="text" id="search2" class="form-control" placeholder="Buscar...">

                                        <div class="datatable-container">
                                            <table class="table datatable">
                                                <thead>
                                                    <tr>
                                                        <th>Foto</th>
                                                        <th>Acciónes</th>
                                                        <th>Fecha Inicio</th>
                                                        <th>Fecha Salida</th>
                                                        <th>Identificacion</th>
                                                        <th>Nombre</th>
                                                        <th>Salario</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="colaboradoresInactivosTableBody">
                                                    <tr id="loadingRow2">
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
                                        <%--  <!-- Pagination -->
                                <nav aria-label="Page navigation">
                                    <ul class="pagination" id="pagination">
                                        <!-- Los botones de paginación se llenarán aquí dinámicamente -->
                                    </ul>
                                </nav>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="pills-temp" role="tabpanel" aria-labelledby="pills-temp-tab">
            <ul class="nav nav-pills mb-3 mt-3" id="pills-tab2" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="pills-contr-tab" data-bs-toggle="pill" data-bs-target="#pills-contr" type="button" role="tab" aria-controls="pills-contr" aria-selected="true">
                        Contratos en Curso</button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="pills-contrH-tab" data-bs-toggle="pill" data-bs-target="#pills-contrH" type="button" role="tab" aria-controls="pills-contrH" aria-selected="false">Historial</button>
                </li>

            </ul>


            <div class="tab-content" id="pills-tabContent2">
                <div class="tab-pane fade show active" id="pills-contr" role="tabpanel" aria-labelledby="pills-temp-tab">

                    <div class="row">
                        <div class="col-lg-12">

                            <div class="card" style="overflow: auto; white-space: nowrap">
                                <div class="card-body">
                                    <h5 class="card-title">Gestión de Contrataciones Temporales</h5>
                                    <label>
                                        Registrar y gestiona la contratacion de servicios de una manera centralizada y simple.
                                    </label>
                                    <hr />
                                    <!-- Table with stripped rows -->
                                    <div class="datatable-wrapper datatable-loading no-footer sortable searchable fixed-columns" style="min-height: 300px">
                                        <input type="text" id="search3" class="form-control" placeholder="Buscar...">

                                        <div class="datatable-container">
                                            <table class="table datatable">
                                                <thead>
                                                    <tr>
                                                        <th>Foto</th>
                                                        <th>Acciónes</th>
                                                        <th>Fecha Registro</th>
                                                        <th>Nombre</th>
                                                        <th>RNC/Cedula</th>
                                                        <th>Telefono 1</th>
                                                        <th>Telefono 2</th>

                                                    </tr>
                                                </thead>
                                                <tbody id="colaboradoresTempTableBody">
                                                    <tr id="loadingRow3">
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
                                        <%--  <!-- Pagination -->
                                <nav aria-label="Page navigation">
                                    <ul class="pagination" id="pagination">
                                        <!-- Los botones de paginación se llenarán aquí dinámicamente -->
                                    </ul>
                                </nav>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="tab-pane fade" id="pills-contrH" role="tabpanel" aria-labelledby="pills-temp-tab">

                    <div class="row">
                        <div class="col-lg-12">

                            <div class="card" style="overflow: auto; white-space: nowrap">
                                <div class="card-body">
                                    <h5 class="card-title">Historial de Contrataciones Temporales</h5>
                                    <label>
                                      Gestiona tu historico de servicios de una manera centralizada y simple.
                                    </label>
                                    <hr />
                                    <!-- Table with stripped rows -->
                                    <div class="datatable-wrapper datatable-loading no-footer sortable searchable fixed-columns" style="min-height: 300px">
                                        <input type="text" id="search4" class="form-control" placeholder="Buscar...">

                                        <div class="datatable-container">
                                            <table class="table datatable">
                                                <thead>
                                                    <tr>
                                                        <th>Foto</th>
                                                        <th>Acciónes</th>
                                                        <th>Fecha Registro</th>
                                                        <th>Nombre</th>
                                                        <th>RNC/Cedula</th>
                                                        <th>Telefono 1</th>
                                                        <th>Telefono 2</th>

                                                    </tr>
                                                </thead>
                                                <tbody id="colaboradoresTempHTableBody">
                                                    <tr id="loadingRow4">
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
                                        <%--  <!-- Pagination -->
                                <nav aria-label="Page navigation">
                                    <ul class="pagination" id="pagination">
                                        <!-- Los botones de paginación se llenarán aquí dinámicamente -->
                                    </ul>
                                </nav>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- Modal Bootstrap -->
    <div class="modal fade modal-sm" id="modalTipoColaborador" tabindex="-1" aria-labelledby="modalTipoColaboradorModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="modalTipoColaboradorlLabel">Tipo de Contratación</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <dx:BootstrapComboBox ID="cbTipoColaborador" runat="server">
                            <Items>
                                <dx:BootstrapListEditItem Text="Empleado Fijo" Selected="true" Value="1"></dx:BootstrapListEditItem>
                                <dx:BootstrapListEditItem Text="Contratista Temporal" Value="2"></dx:BootstrapListEditItem>

                            </Items>
                        </dx:BootstrapComboBox>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cancelar</button>
                    <button id="btnNew" type="button" runat="server" class="btn btn-success" onserverclick="btnNew_ServerClick"><i class="bi bi-check"></i>Comenzar</button>
                </div>
            </div>
        </div>
    </div>
    <uc:ModalEmpleado ID="ModalEmpleado" runat="server" />
    <uc:ModalContratacion ID="ModalContratacion" runat="server" />

    <script>

        function validateForm() {


            var alertToast = new bootstrap.Toast(document.getElementById('alertToast'));
            alertToast.show();
            return false; // Evitar el envío del formulario si los campos están vacíos
        }
        // Función para obtener el valor de una cookie específica
        function getCookieValue(cookieName) {
            const cookies = document.cookie.split(';');

            for (let i = 0; i < cookies.length; i++) {
                const cookie = cookies[i].trim();

                if (cookie.startsWith(cookieName + "=")) {
                    return decodeURIComponent(cookie.substring(cookieName.length + 1));
                }
            }
            return null;
        }


        $(document).ready(function () {
            var pageIndex = 1;
            var pageSize = 10;
            const loginCookie = getCookieValue("login");

            var userID = null;



            // Función para cargar los datos
            function loadColaboradores(pageIndex, searchTerm) {

                // Obtener el valor de la cookie "login"
                //console.log(loginCookie);
                if (loginCookie) {
                    // Dividir la cookie en pares clave-valor
                    const cookieParts = loginCookie.split('&');


                    // Buscar el valor de "userID"
                    for (let part of cookieParts) {
                        const [key, value] = part.trim().split('=');
                        if (key === "migente_userID") {
                            userID = value;
                            break;
                        }
                    }

                    //console.log("User ID:", userID);
                } else {
                    console.log("La cookie 'login' no existe.");
                }


                $("#loadingRow").show();
                $.ajax({
                    type: "POST",
                    url: "colaboradores.aspx/GetColaboradores",
                    data: JSON.stringify({ userID: userID, pageIndex: pageIndex, pageSize: pageSize, searchTerm: searchTerm }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        //console.log("test");
                        var data = response.d;
                        var colaboradores = data.colaboradores;
                        var totalRecords = data.totalRecords;

                        // Limpiar la tabla
                        $("#colaboradoresTableBody").empty();

                        // Llenar la tabla con los datos recibidos
                        $.each(colaboradores, function (index, colaborador) {
                            //console.log('../' + colaborador.foto);
                            $("#colaboradoresTableBody").append(`
                                <tr>
                      
                              <td><img src='${colaborador.foto}' width="50" height="50" /> </td>

                                    <td>
                                        <div class="dropdown">
                                            <button class="btn btn-primary btn-sm dropdown-toggle" type="button" id="dropdownMenuButton${colaborador.empleadoID}" data-bs-toggle="dropdown" aria-expanded="false">
                                                Acciones
                                            </button>
                                            <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton${colaborador.empleadoID}">
                                           
                                                <li><a class="dropdown-item" href="fichaEmpleado.aspx?empleadoID=${colaborador.empleadoID}">
                                                    <i class="bi bi-person-circle"></i> Ver Perfil
                                                </a></li>
                                                   
                                                <hr/>
                                                
                                            </ul>
                                        </div>
                                    </td>
                                    <td>${new Date(colaborador.fechaInicio).toLocaleDateString()}</td>
                                    <td>${colaborador.identificacion}</td>
                                    <td>${colaborador.Nombre}</td>
                                     <td>${colaborador.salario}</td>
                            
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
                        $("#colaboradoresTableBody").empty().append(`
                            <tr>
                                <td colspan="12" class="text-center text-danger">Error al cargar los datos.</td>
                            </tr>
                        `);
                        // Ocultar el spinner
                        $("#loadingRow").hide();
                    }
                });
            }
            function loadColaboradoresInactivos(pageIndex, searchTerm) {

                // Obtener el valor de la cookie "login"
                //console.log(loginCookie);
                if (loginCookie) {
                    // Dividir la cookie en pares clave-valor
                    const cookieParts = loginCookie.split('&');

                    // Buscar el valor de "userID"
                    for (let part of cookieParts) {
                        const [key, value] = part.trim().split('=');
                        if (key === "migente_userID") {
                            userID = value;
                            break;
                        }
                    }

                    //console.log(pageIndex);
                    //console.log(pageSize);
                    //console.log(searchTerm);
                } else {
                    console.log("La cookie 'login' no existe.");
                }

                //console.log("User ID:", userID);

                $("#loadingRow2").show();
                $.ajax({
                    type: "POST",
                    url: "colaboradores.aspx/GetColaboradoresInactivos",
                    data: JSON.stringify({ userID: userID, pageIndex: pageIndex, pageSize: pageSize, searchTerm: searchTerm }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        //console.log("test");
                        var data2 = response.d;
                        //console.log(data2);
                        var colaboradoresInactivos = data2.colaboradoresInactivos;
                        var totalRecords = data2.totalRecords;
                        //console.log(colaboradoresInactivos);

                        // Limpiar la tabla
                        $("#colaboradoresInactivosTableBody").empty();

                        // Llenar la tabla con los datos recibidos
                        $.each(colaboradoresInactivos, function (index, colaboradorInactivo) {
                            //console.log('../' + colaborador.foto);
                            $("#colaboradoresInactivosTableBody").append(`
                                <tr>
                      
                              <td><img src='${colaboradorInactivo.foto}' width="50" height="50" /> </td>

                                    <td>
                                        <div class="dropdown">
                                            <button class="btn btn-primary btn-sm dropdown-toggle" type="button" id="dropdownMenuButton${colaboradorInactivo.empleadoID}" data-bs-toggle="dropdown" aria-expanded="false">
                                                Acciones
                                            </button>
                                            <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton${colaboradorInactivo.empleadoID}">
                                           
                                                <li><a class="dropdown-item" href="fichaEmpleado.aspx?empleadoID=${colaboradorInactivo.empleadoID}">
                                                    <i class="bi bi-person-circle"></i> Ver Perfil
                                                </a></li>
                                                          
                                                <hr/>
                                           
                                            </ul>
                                        </div>
                                    </td>
                                    <td>${new Date(colaboradorInactivo.fechaInicio).toLocaleDateString()}</td>
                                     <td>${colaboradorInactivo.fechaSalida}</td>
                                    <td>${colaboradorInactivo.identificacion}</td>
                                    <td>${colaboradorInactivo.Nombre}</td>
                                     <td>${colaboradorInactivo.salario}</td>
                            
                                    </tr>
                            `);
                        });

                        // Actualizar la paginación
                    },
                    complete: function () {
                        // Ocultar el spinner de carga cuando la solicitud haya terminado
                        console.log("error");

                        $("#loadingRow2").hide();
                    },
                    error: function () {
                        // Mostrar un mensaje de error si la solicitud falla
                        $("#colaboradoresInactivosTableBody").empty().append(`
                            <tr>
                                <td colspan="12" class="text-center text-danger">Error al cargar los datos.</td>
                            </tr>
                        `);
                        // Ocultar el spinner
                        $("#loadingRow2").hide();
                    }
                });
            }



            //**************** Contrataciones temporales ******************//
            function loadContTemporales(pageIndex, searchTerm) {

                // Obtener el valor de la cookie "login"
                //console.log(loginCookie);
                if (loginCookie) {
                    // Dividir la cookie en pares clave-valor
                    const cookieParts = loginCookie.split('&');

                    // Buscar el valor de "userID"
                    for (let part of cookieParts) {
                        const [key, value] = part.trim().split('=');
                        if (key === "migente_userID") {
                            userID = value;
                            break;
                        }
                    }

                    //console.log(pageIndex);
                    //console.log(pageSize);
                    //console.log(searchTerm);
                } else {
                    console.log("La cookie 'login' no existe.");
                }

                //console.log("User ID:", userID);

                $("#loadingRow3").show();
                $.ajax({
                    type: "POST",
                    url: "colaboradores.aspx/GetContratacionesTemporales",
                    data: JSON.stringify({ userID: userID, pageIndex: pageIndex, pageSize: pageSize, searchTerm: searchTerm, estatus:1 }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        //console.log("test");
                        var data3 = response.d;
                        //console.log(data3);
                        var contratacionesTemporales = data3.contratacionesTemporales;
                        var totalRecords = data3.totalRecords;
                        //console.log(contratacionesTemporales);

                        // Limpiar la tabla
                        $("#colaboradoresTempTableBody").empty();

                        // Llenar la tabla con los datos recibidos
                        $.each(contratacionesTemporales, function (index, contratacionTemporal) {
                            //console.log('../' + colaborador.foto);
                            $("#colaboradoresTempTableBody").append(`
                                <tr>
                      
                              <td><img src='${contratacionTemporal.foto}' width="50" height="50" /> </td>

                                    <td>
                                        <div class="dropdown">
                                            <button class="btn btn-primary btn-sm dropdown-toggle" type="button" id="dropdownMenuButton${contratacionTemporal.contratacionID}" data-bs-toggle="dropdown" aria-expanded="false">
                                                Acciones
                                            </button>
                                            <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton${contratacionTemporal.contratacionID}">
                                           
                                                <li><a class="dropdown-item" href="fichaColaboradorTemporal.aspx?estatus=1&contratacionID=${contratacionTemporal.contratacionID}">
                                                    <i class="bi bi-person-circle"></i> Ver Perfil
                                                </a></li>
                                                  
                                                <hr/>
                                           
                                            </ul>
                                        </div>
                                    </td>
                                    <td>${new Date(contratacionTemporal.fechaRegistro).toLocaleDateString()}</td>
                                    <td>${contratacionTemporal.Nombre}</td>

                                    <td>${contratacionTemporal.identificacion}</td>
                                     <td>${contratacionTemporal.telefono1}</td>
                                     <td>${contratacionTemporal.telefono2}</td>

                            
                                    </tr>
                            `);
                        });

                        // Actualizar la paginación
                    },
                    complete: function () {
                        // Ocultar el spinner de carga cuando la solicitud haya terminado
                        //console.log("error");

                        $("#loadingRow3").hide();
                    },
                    error: function () {
                        // Mostrar un mensaje de error si la solicitud falla
                        $("#colaboradoresTempTableBody").empty().append(`
                            <tr>
                                <td colspan="12" class="text-center text-danger">Error al cargar los datos.</td>
                            </tr>
                        `);
                        // Ocultar el spinner
                        $("#loadingRow3").hide();
                    }
                });
            }


            //**************** HISTORIAL Contrataciones temporales ******************//
            function loadContTemporalesH(pageIndex, searchTerm) {

                // Obtener el valor de la cookie "login"
                //console.log(loginCookie);
                if (loginCookie) {
                    // Dividir la cookie en pares clave-valor
                    const cookieParts = loginCookie.split('&');

                    // Buscar el valor de "userID"
                    for (let part of cookieParts) {
                        const [key, value] = part.trim().split('=');
                        if (key === "migente_userID") {
                            userID = value;
                            break;
                        }
                    }

                    //console.log(pageIndex);
                    //console.log(pageSize);
                    //console.log(searchTerm);
                } else {
                    console.log("La cookie 'login' no existe.");
                }

                //console.log("User ID:", userID);

                $("#loadingRow4").show();
                $.ajax({
                    type: "POST",
                    url: "colaboradores.aspx/GetContratacionesTemporales",
                    data: JSON.stringify({ userID: userID, pageIndex: pageIndex, pageSize: pageSize, searchTerm: searchTerm, estatus:2 }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        //console.log("test");
                        var data3 = response.d;
                        //console.log(data3);
                        var contratacionesTemporales = data3.contratacionesTemporales;
                        var totalRecords = data3.totalRecords;
                        //console.log(contratacionesTemporales);

                        // Limpiar la tabla
                        $("#colaboradoresTempHTableBody").empty();

                        // Llenar la tabla con los datos recibidos
                        $.each(contratacionesTemporales, function (index, contratacionTemporal) {
                            //console.log('../' + colaborador.foto);
                            $("#colaboradoresTempHTableBody").append(`
                                <tr>
                      
                              <td><img src='${contratacionTemporal.foto}' width="50" height="50" /> </td>

                                    <td>
                                        <div class="dropdown">
                                            <button class="btn btn-primary btn-sm dropdown-toggle" type="button" id="dropdownMenuButton${contratacionTemporal.contratacionID}" data-bs-toggle="dropdown" aria-expanded="false">
                                                Acciones
                                            </button>
                                            <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton${contratacionTemporal.contratacionID}">
                                           
                                                <li><a class="dropdown-item" href="fichaColaboradorTemporal.aspx?estatus=2&contratacionID=${contratacionTemporal.contratacionID}">
                                                    <i class="bi bi-person-circle"></i> Ver Perfil
                                                </a></li>
                                                  
                                                <hr/>
                                           
                                            </ul>
                                        </div>
                                    </td>
                                    <td>${new Date(contratacionTemporal.fechaRegistro).toLocaleDateString()}</td>
                                    <td>${contratacionTemporal.Nombre}</td>

                                    <td>${contratacionTemporal.identificacion}</td>
                                     <td>${contratacionTemporal.telefono1}</td>
                                     <td>${contratacionTemporal.telefono2}</td>

                            
                                    </tr>
                            `);
                        });

                        // Actualizar la paginación
                    },
                    complete: function () {
                        // Ocultar el spinner de carga cuando la solicitud haya terminado
                        //console.log("error");

                        $("#loadingRow4").hide();
                    },
                    error: function () {
                        // Mostrar un mensaje de error si la solicitud falla
                        $("#colaboradoresTempHTableBody").empty().append(`
                            <tr>
                                <td colspan="12" class="text-center text-danger">Error al cargar los datos.</td>
                            </tr>
                        `);
                        // Ocultar el spinner
                        $("#loadingRow4").hide();
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
                    loadColaboradores(pageIndex, $("#search2").val());
                });
            }

            // Evento para la búsqueda
            $("#search").on("input", function () {
                loadColaboradores(1, $(this).val()); // Resetear a la página 1 al hacer una búsqueda

            });

            // Cargar los datos inicialmente
            loadColaboradores(pageIndex, "");
            loadColaboradoresInactivos(pageIndex, "");
            loadContTemporales(pageIndex, "");
            loadContTemporalesH(pageIndex, "");



        });





    </script>
</asp:Content>
