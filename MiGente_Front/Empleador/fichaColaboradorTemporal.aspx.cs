using Microsoft.VisualStudio.Services.Profile;
using MiGente_Front.Data;
using MiGente_Front.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.Empleador
{
    public partial class fichaColaboradorTemporal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["contratacionID"] = Convert.ToInt32(Request.QueryString["contratacionID"]);
                HttpCookie myCookie = Request.Cookies["login"];

                Session["userID"] = myCookie["migente_userID"];
              HiddenField1.Value= myCookie["migente_userID"];

                obtenerFicha(Convert.ToInt32(Session["contratacionID"]), Session["userID"].ToString());


            }
        }
        private static EmpleadosTemporales empleado;

        void obtenerFicha(int contratacionID, string userID)
            {
                EmpleadosService es = new EmpleadosService();
                empleado = es.obtenerFichaTemporales(contratacionID, userID);
                if (empleado != null)
                {
                    fechaRegistro.InnerText = empleado.fechaRegistro.ToString();
                    if (empleado.tipo == 1)
                    {
                        tipo.InnerText = "Persona Fisica";
                        identificacion.InnerText = empleado.identificacion;
                        nombre.InnerText = empleado.nombre.ToString() + " " + empleado.apellido.ToString();
                        NombreEmpleado.InnerText = empleado.nombre.ToString() + " " + empleado.apellido.ToString();
                    }
                    else
                    {
                        tipo.InnerText = "Empresa";
                        identificacion.InnerText = empleado.rnc;
                        nombre.InnerText = empleado.nombreComercial.ToString();
                        NombreEmpleado.InnerText = empleado.nombreComercial.ToString();
                        nombreRepresentante.InnerText = empleado.nombreRepresentante.ToString();
                        cedulaRepresentante.InnerText = empleado.nombreRepresentante.ToString();

                    }
                    htmlDireccion.InnerText = empleado.direccion;
                    htmlProvincia.InnerText = empleado.provincia;
                    htmlMunucipio.InnerText = empleado.municipio;
                    telefono1.InnerText = empleado.telefono1;
                    telefono2.InnerText = empleado.telefono2;
                  
                    string base64String = empleado.foto;
                    base64String = base64String.Replace("data:image/jpg;base64,", "");

                    byte[] imagenBytes = Convert.FromBase64String(base64String);
                    MemoryStream ms = new MemoryStream(imagenBytes);
                    System.Drawing.Image imagen = System.Drawing.Image.FromStream(ms);


                    //Image1.ImageUrl = "data:image/jpg;base64," + base64String;
                    Image2.ImageUrl = "data:image/jpg;base64," + base64String;
                    //fichaEdicion

                    //ddlTipo.SelectedItem.Value = empleado.tipo.ToString();
                    //txtIdentificacion.Text = empleado.identificacion;
                    //txtNombre.Text = empleado.nombre;
                    //txtApellido.Text = empleado.apellido;
                    //txtAlias.Text = empleado.alias;
                    //txtDireccion.Text = empleado.direccion;
                    //txtProvincia.Text = empleado.provincia;
                    //txtMunicipio.Text = empleado.municipio;
                    //txtTelefono1.Text = empleado.telefono1;
                    //txtTelefono2.Text = empleado.telefono2;
                    //if (empleado.foto != null && empleado.foto != "")
                    //{
                    //    //divFoto.Visible = true;


                    //}




                }
            }


            [WebMethod]
            public static string detalleContratacion(int detalleID)
            {

                //obtener detalle de trabajo
                var result = empleado.DetalleContrataciones.Where(a => a.detalleID == detalleID).FirstOrDefault();
                var jsonSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                DetalleContrataciones det = result;
                JsonConvert.DefaultSettings = () => jsonSettings;
                var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(det);
                return jsonResult;

            }

        protected void gridTrabajos_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "btnDetalles")
            {
                object value = gridTrabajos.GetRowValues(e.VisibleIndex, "detalleID");

                if (value != null)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("detalleContratacion.aspx?detalleID=" + value.ToString() + "&contratacionID=" + Session["contratacionID"]);


                }
            }
        }
        [WebMethod]
        public static string eliminarColaborador(int contratacionID)
        {
            try
            {
                EmpleadosService es = new EmpleadosService();
                es.eliminarEmpleadoTemporal(contratacionID);

                // Luego, muestra un mensaje de éxito
                string mensaje = "El recibo con colaborador " + contratacionID + " ha sido anulado correctamente.";
                return mensaje;
            }
            catch (Exception ex)
            {
                // En caso de error, muestra un mensaje de error
                return "Error al anular el recibo: " + ex.Message;
            }
        }

        protected void btnBaja_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfirmacionEliminar", "ConfirmarEliminarColaborador(" + Session["contratacionID"] + ");", true);

        }

        protected void gridHistorico_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "btnDetalleHistorico")
            {
                object value = gridHistorico.GetRowValues(e.VisibleIndex, "detalleID");

                if (value != null)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("detalleContratacion.aspx?detalleID=" + value.ToString() + "&contratacionID=" + Session["contratacionID"]);


                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            nuevaContratacionTemporal();
        }

        public void nuevaContratacionTemporal()
        {
            DetalleContrataciones det = new DetalleContrataciones();
            det.descripcionCorta = descripcionCortaTrabajo.Text;
            det.descripcionAmpliada = descripcionAmpliada.Text;
            det.fechaInicio = fechaInicio.Date;
            det.fechaFinal = fechaConclusion.Date;
            det.montoAcordado = Convert.ToDecimal(montoAcordado.Text);
            det.esquemaPagos = ddlEsquema.SelectedItem.Value;
            det.contratacionID = Convert.ToInt32(Session["contratacionID"]);
            det.estatus = 1;
            det.calificado = false;
            det.calificacionID = 0;
            EmpleadosService es = new EmpleadosService();
            var result = es.nuevaContratacionTemporal(det);
            if (result)
            {
                string url = "fichaColaboradorTemporal.aspx?contratacionID=" + Session["contratacionID"];
                string script = @"<script>
                    Swal.fire({
                        title: 'Éxito',
                        text: 'Se ha registrado una nueva contratacion de servicios',
                        icon: 'success',
                        confirmButtonText: 'Aceptar'
                    }).then(function() {
                        window.location.href = '" + url + @"';
                    });
                </script>";

                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
            }
        }
    }
}
