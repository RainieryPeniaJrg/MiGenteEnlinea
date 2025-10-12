<%@ Page Title="" Language="C#" MasterPageFile="~/ContratistaM.Master" AutoEventWireup="true" CodeBehind="MisCalificaciones.aspx.cs" Inherits="MiGente_Front.Contratista.MisCalificaciones" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="pagetitle">
        <h1>Consulta Tus Calificaciones</h1>
        <nav>
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Mis Calificaciones</a></li>
            </ol>
        </nav>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.1.4/dist/sweetalert2.all.min.js"></script>
    <script src="https://kit.fontawesome.com/a076d05399.js"></script>
    <div class="container mt-1 ">
        <div class="section-title justify-content-center" style="text-align: center; overflow-wrap:normal">
        </div>
        <div class="row text-center justify-content-center">
            <div class="col-md-6 mt-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">Consulta tu Perfil</h5>
                        <i class="bi bi-star-fill  bi-5x  text-warning mb-3"></i>
                        <p class="card-text">Conoce como te han calificado tus contratantes</p>
                        <%--<button type="button" data-bs-toggle="modal" data-bs-target="#modalCalificar" class="btn btn-primary">Calificar</button>--%>
                    </div>
                </div>
            </div>
     

        </div>

    </div>
    <div class="container mt-6 align-content-center" style="text-align: center">
        <h3>Mis calificaciones</h3>

  <div class="table-responsive">
    <dx:BootstrapGridView ID="gridCalificaciones" OnHtmlDataCellPrepared="gridCalificaciones_HtmlDataCellPrepared" runat="server" AutoGenerateColumns="False" DataSourceID="linqMisCalificaciones">
        <Columns>
            <dx:BootstrapGridViewTextColumn FieldName="calificacionID" Visible="false" VisibleIndex="0"></dx:BootstrapGridViewTextColumn>
            <dx:BootstrapGridViewDateColumn FieldName="fecha" SortOrder="Descending" Caption="Fecha" VisibleIndex="1"></dx:BootstrapGridViewDateColumn>
            <dx:BootstrapGridViewTextColumn FieldName="userID" Visible="false" VisibleIndex="2"></dx:BootstrapGridViewTextColumn>
            <dx:BootstrapGridViewTextColumn FieldName="tipo" Visible="false" VisibleIndex="3"></dx:BootstrapGridViewTextColumn>
            <dx:BootstrapGridViewTextColumn FieldName="NombreCalificador" Caption="Calificador" VisibleIndex="4"></dx:BootstrapGridViewTextColumn>
            <%--<dx:BootstrapGridViewTextColumn FieldName="nombre" Caption="Nombre" VisibleIndex="5"></dx:BootstrapGridViewTextColumn>--%>
            <dx:BootstrapGridViewTextColumn HorizontalAlign="Center" FieldName="puntualidad" Caption="Puntualidad" VisibleIndex="6">
                <PropertiesTextEdit EncodeHtml="false" />
            </dx:BootstrapGridViewTextColumn>
            <dx:BootstrapGridViewTextColumn HorizontalAlign="Center" FieldName="conocimientos" Caption="Conocimientos" VisibleIndex="7">
                <PropertiesTextEdit EncodeHtml="false" />
            </dx:BootstrapGridViewTextColumn>
            <dx:BootstrapGridViewTextColumn HorizontalAlign="Center" FieldName="cumplimiento" Caption="Cumplimiento" VisibleIndex="8">
                <PropertiesTextEdit EncodeHtml="false" />
            </dx:BootstrapGridViewTextColumn>
           <dx:BootstrapGridViewTextColumn HorizontalAlign="Center" FieldName="recomendacion" Caption="Recomendacion" VisibleIndex="9">
                <PropertiesTextEdit EncodeHtml="false" />
            </dx:BootstrapGridViewTextColumn>
        </Columns>
    </dx:BootstrapGridView>
</div>

        <asp:LinqDataSource runat="server" EntityTypeName="" ID="linqMisCalificaciones" ContextTypeName="MiGente_Front.Data.migenteEntities" TableName="VMisCalificaciones" Where="identificacion == @identificacion">
            <WhereParameters>
                <asp:ControlParameter ControlID="HiddenField1" PropertyName="Value" DefaultValue="0" Name="identificacion" Type="String"></asp:ControlParameter>
            </WhereParameters>
        </asp:LinqDataSource>
    </div>
    
 
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <asp:HiddenField ID="HiddenField1" runat="server" />
</asp:Content>
