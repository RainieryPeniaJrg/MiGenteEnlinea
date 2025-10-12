<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" CodeBehind="MiSuscripcion.aspx.cs" Inherits="MiGente_Front.MiSuscripcion" Async="true"  %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0"  style="color:white">Gestión de Suscripción</h5>
                </div>
                <div class="card-body">
                        <div class="mb-3">
                            <dx:BootstrapTextBox Caption="Plan Actual" ReadOnly="true" ID="txtPlanActual" runat="server"></dx:BootstrapTextBox>
                        </div>
                        <div class="mb-3">
                          <dx:BootstrapTextBox Caption="Fecha de Inicio" ReadOnly="true" ID="txtFechaInicio" runat="server"></dx:BootstrapTextBox>

                        </div>
                        <div class="mb-3" id="proxPago">
                         <dx:BootstrapTextBox Caption="Proximo Pago" ReadOnly="true" ID="txtProximoPago" runat="server"></dx:BootstrapTextBox>

                        </div>
                     
                    <div class="mb-3 " style="margin-block:10px">

                    <%--<dx:BootstrapButton  CssClasses-Control="col-md-4 col-12" ID="btnDetalles" runat="server" AutoPostBack="false" Text="Ver Detalles"></dx:BootstrapButton>--%>
                    <dx:BootstrapButton  CssClasses-Control="col-md-5 col-12 bg-danger" ID="btnCancelar" runat="server" AutoPostBack="false" Text="Cancelar Suscripcion"></dx:BootstrapButton>
                      
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>
        <div class="container mt-2">
            <h5>Historico de Facturacion</h5>
        </div>
        <div class="container mt-2">
       <dx:BootstrapGridView ID="gridPagos" runat="server" AutoGenerateColumns="False">
           <SettingsAdaptivity HideDataCellsAtWindowInnerWidth="Small"></SettingsAdaptivity>
    <Columns>
        <dx:BootstrapGridViewTextColumn FieldName="ventaID" Visible="false" ReadOnly="True" VisibleIndex="0"></dx:BootstrapGridViewTextColumn>
        <dx:BootstrapGridViewDateColumn FieldName="fecha" ReadOnly="True" VisibleIndex="1"></dx:BootstrapGridViewDateColumn>
        <dx:BootstrapGridViewTextColumn FieldName="planID" ReadOnly="True" Caption="Id Plan" VisibleIndex="2"></dx:BootstrapGridViewTextColumn>
        <dx:BootstrapGridViewTextColumn FieldName="precio" PropertiesTextEdit-DisplayFormatString="{0:n2}" ReadOnly="True" VisibleIndex="3"></dx:BootstrapGridViewTextColumn>
        <dx:BootstrapGridViewTextColumn FieldName="card" Caption="Tarjeta" ReadOnly="True" VisibleIndex="4"></dx:BootstrapGridViewTextColumn>
    </Columns>

    <SettingsPager>
        <PageSizeItemSettings Visible="false" />
    </SettingsPager>
</dx:BootstrapGridView>

            
        </div>

<!-- Agrega tu script personalizado aquí -->
<script>
 
</script>

</asp:Content>

