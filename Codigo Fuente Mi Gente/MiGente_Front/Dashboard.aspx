<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" Async="true" CodeBehind="Dashboard.aspx.cs" Inherits="MiGente_Front.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="sm"></asp:ScriptManager>
    <h4>Menú Principal</h4>

    <div class="row container bg-white p-3">
        <!-- Card de Pagos -->
        <div class="card col col-md-3 col-12 border-1  m-1">
            <div class="card-header">
                <div class="row align-items-center">
                    <div class="col-8">
                        <label>PAGOS</label>
                    </div>
                    <div class="col-4 text-end">
                        <img src="../../Images/payIcon.png" alt="Ícono" class="img-fluid" style="max-height: 40px;">
                    </div>
                    <label style="font-weight: bold">$0.00</label>
                </div>
            </div>
            <div class="card-body">
                <label style="color: darkgray">Historial de Pagos Realizados</label>
            </div>
        </div>
        
        <!-- Card de Empleados -->
        <div class="card col col-md-3 col-12 border-1 m-1">
            <div class="card-header">
                <div class="row align-items-center">
                    <div class="col-8">
                        <label>EMPLEADOS</label>
                    </div>
                    <div class="col-4 text-end">
                        <img src="../../Images/employee.png" alt="Ícono" class="img-fluid" style="max-height: 40px;">
                    </div>
                    <label style="font-weight: bold">0</label>
                </div>
            </div>
            <div class="card-body">
                <label style="color: darkgray">Nomina Actual de Empleados</label>
            </div>
        </div>

        <!-- Card de Calificaciones -->
        <div class="card col col-md-3 col-12 border-1 m-1">
            <div class="card-header">
                <div class="row align-items-center">
                    <div class="col-8">
                        <label>CALIFICACIONES</label>
                    </div>
                    <div class="col-4 text-end">
                        <img src="../../Images/star.png" alt="Ícono" class="img-fluid" style="max-height: 40px;">
                    </div>
                    <label style="font-weight: bold">0</label>
                </div>
            </div>
            <div class="card-body">
                <label style="color: darkgray">Calificaciones Completadas</label>
            </div>
        </div>
    </div>

    <!-- Enlaces Rápidos -->
    <div class="row container bg-white p-3 mt-2 align-content-center justify-content-center">
        <div class="col col-md-3 col-12">
            <asp:LinkButton ID="LinkButton1" Width="100%" runat="server" CssClass="btn btn-primary">
                <i class="bi bi-people-fill"></i> Colaboradores
            </asp:LinkButton>
        </div>
        <div class="col col-md-3 col-12">
            <asp:LinkButton ID="LinkButton2" Width="100%" runat="server" CssClass="btn btn-primary">
                <i class="bi bi-star"></i> Calificación de Perfiles
            </asp:LinkButton>
        </div>
        <div class="col col-md-3 col-12">
            <asp:LinkButton ID="LinkButton3" Width="100%" runat="server" CssClass="btn btn-primary">
                <i class="bi bi-people"></i> Comunidad
            </asp:LinkButton>
        </div>
    </div>

    <!-- Historial de Pagos -->
    <div class="row container bg-white p-3 mt-2">
        <h4>Historial de Pagos</h4>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Fecha</th>
                    <th>Monto</th>
                    <th>Estado</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>12/11/2024</td>
                    <td>$500.00</td>
                    <td>Completado</td>
                </tr>
                <tr>
                    <td>11/11/2024</td>
                    <td>$320.00</td>
                    <td>Completado</td>
                </tr>
                <!-- Aquí puedes agregar más filas según el historial de pagos -->
            </tbody>
        </table>
    </div>

    <!-- Gráfico de Calificaciones -->
    <div class="row container bg-white p-3 mt-2">
        <h4>Gráfico de Calificaciones Completadas</h4>
        <div id="ratingChart" style="height: 300px;"></div>
    </div>

    <!-- Estadísticas en Gráfico -->
    <div class="row container bg-white p-3 mt-2">
        <h4>Estadísticas de Actividad</h4>
        <div id="activityChart" style="height: 300px;"></div>
    </div>

   <script>
       // Aquí puedes agregar las configuraciones para los gráficos utilizando Chart.js o ApexCharts
       var options = {
           chart: {
               type: 'bar',
               height: '300'
           },
           series: [{
               name: 'Calificaciones',
               data: [30, 40, 50, 60, 70, 80]
           }],
           xaxis: {
               categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio']
           }
       };

       var chart = new ApexCharts(document.querySelector("#ratingChart"), options);
       chart.render();

       var activityOptions = {
           chart: {
               type: 'line',
               height: '300'
           },
           series: [{
               name: 'Usuarios Activos',
               data: [12, 25, 34, 46, 56, 70]
           }],
           xaxis: {
               categories: ['Semana 1', 'Semana 2', 'Semana 3', 'Semana 4', 'Semana 5', 'Semana 6']
           }
       };
       function setChartData(calificaciones) {
           var options = {
               chart: {
                   type: 'bar',
                   height: '300'
               },
               series: [{
                   name: 'Calificaciones',
                   data: calificaciones
               }],
               xaxis: {
                   categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio']
               }
           };

           var chart = new ApexCharts(document.querySelector("#ratingChart"), options);
           chart.render();
       }

       var activityChart = new ApexCharts(document.querySelector("#activityChart"), activityOptions);
       activityChart.render();
</script>
</asp:Content>
