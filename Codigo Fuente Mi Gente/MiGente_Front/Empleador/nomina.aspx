<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" AutoEventWireup="true" CodeBehind="nomina.aspx.cs" Inherits="MiGente_Front.Empleador.nomina" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2 class="text-center mb-4">Gestión de Nómina</h2>
   <div class="container mt-4">
            <h3 class="text-center">Gestión de Novedades de Nómina</h3>
            <div class="card shadow mt-4">
                <div class="card-header bg-primary text-white">
                    Detalle de la Nómina
                </div>
                <div class="card-body">
                    <!-- Información General -->
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label for="txtNombre" class="form-label">Nombre</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label for="ddlEstatus" class="form-label">Estatus</label>
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Activo" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Inactivo" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label for="txtOrigenRecurso" class="form-label">Origen del Recurso</label>
                            <asp:TextBox ID="txtOrigenRecurso" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label for="txtMontoRecurso" class="form-label">Monto de Recurso</label>
                            <asp:TextBox ID="txtMontoRecurso" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Percepciones -->
                    <div class="mt-4">
                        <h5>Percepciones</h5>
                        <div class="row">
                            <div class="col-md-4">
                                <label for="ddlTipoPercepcion" class="form-label">Tipo de Percepción</label>
                                <asp:DropDownList ID="ddlTipoPercepcion" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccione" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Sueldo" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Bonos" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label for="txtImporteGravado" class="form-label">Importe Gravado</label>
                                <asp:TextBox ID="txtImporteGravado" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtObservacionesPercepcion" class="form-label">Observaciones</label>
                                <asp:TextBox ID="txtObservacionesPercepcion" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="text-end mt-3">
                            <asp:Button ID="btnAgregarPercepcion" runat="server" Text="Agregar" CssClass="btn btn-success" />
                        </div>
                        <!-- Tabla de Percepciones -->
                        <div class="mt-3">
                            <asp:GridView ID="gvPercepciones" runat="server" CssClass="table table-bordered table-hover">
                                <Columns>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                    <asp:BoundField DataField="Importe" HeaderText="Importe Gravado" />
                                    <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                    <asp:ButtonField ButtonType="Button" Text="Eliminar" CommandName="Delete" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <!-- Deducciones -->
                    <div class="mt-4">
                        <h5>Deducciones</h5>
                        <div class="row">
                            <div class="col-md-4">
                                <label for="ddlTipoDeduccion" class="form-label">Tipo de Deducción</label>
                                <asp:DropDownList ID="ddlTipoDeduccion" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccione" Value=""></asp:ListItem>
                                    <asp:ListItem Text="ISR" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="IMSS" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label for="txtImporteDeduccion" class="form-label">Importe</label>
                                <asp:TextBox ID="txtImporteDeduccion" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtObservacionesDeduccion" class="form-label">Observaciones</label>
                                <asp:TextBox ID="txtObservacionesDeduccion" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="text-end mt-3">
                            <asp:Button ID="btnAgregarDeduccion" runat="server" Text="Agregar" CssClass="btn btn-success" />
                        </div>
                        <!-- Tabla de Deducciones -->
                        <div class="mt-3">
                            <asp:GridView ID="gvDeducciones" runat="server" CssClass="table table-bordered table-hover">
                                <Columns>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                    <asp:BoundField DataField="Importe" HeaderText="Importe" />
                                    <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                    <asp:ButtonField ButtonType="Button" Text="Eliminar" CommandName="Delete" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

