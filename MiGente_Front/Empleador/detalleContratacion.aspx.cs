using MiGente_Front.Data;
using MiGente_Front.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.Empleador
{
    public partial class detalleContratacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["contratacionID"] = Convert.ToInt32(Request.QueryString["contratacionID"]);
                Session["detalleID"] = Convert.ToInt32(Request.QueryString["detalleID"]);

                HttpCookie myCookie = Request.Cookies["login"];

                HiddenField1.Value = myCookie["migente_userID"];
                Session["userID"] = myCookie["migente_userID"];

                linkVolver.HRef = "fichaColaboradorTemporal.aspx?contratacionID=" + Session["contratacionID"];
                lbCalificar.InnerHtml = "Para calificar el trabajo primero debe finalizar el mismo";

                obtenerFicha(Convert.ToInt32(Session["contratacionID"]), Session["userID"].ToString());
                fechaPago.Date = DateTime.Now;

            }
        }
        private static EmpleadosTemporales empleado = new EmpleadosTemporales();
        void obtenerFicha(int contratacionID, string userID)
        {
            EmpleadosService es = new EmpleadosService();

            empleado = es.obtenerFichaTemporales(contratacionID, userID);
            if (empleado != null)
            {
                if (empleado.tipo == 1)
                {
                    tituloContratacion.InnerText = empleado.nombre.ToString() + " " + empleado.apellido.ToString();
                }
                else
                {
                    tituloContratacion.InnerText = empleado.nombreComercial.ToString();
                }
                //obtenerDetalle
                var detalle = empleado.DetalleContrataciones.Where(x => x.detalleID == Convert.ToInt32(Session["detalleID"])).FirstOrDefault();


                descripcionCortaTrabajo.Text = detalle.descripcionCorta;
                descripcionAmpliada.Text = detalle.descripcionAmpliada;
                fechaInicio.Date = Convert.ToDateTime(detalle.fechaInicio);
                fechaConclusion.Date = Convert.ToDateTime(detalle.fechaFinal);
                montoAcordado.Text = Convert.ToDecimal(detalle.montoAcordado).ToString("N2");
                ddlEsquema.SelectedValue = detalle.esquemaPagos;
               Session["montoAcordado"]  = Convert.ToDecimal(detalle.montoAcordado).ToString("N2");
                //buscarPagos
                obtenerPagos(contratacionID, es);

                //verificar si esta saldo
                if (Convert.ToDecimal(montoPendiente.InnerText) < 1)
                {
                    descripcionCortaTrabajo.Enabled = false;
                    descripcionAmpliada.Enabled = false;
                    fechaInicio.Enabled = false;
                    fechaConclusion.Enabled = false;
                    montoAcordado.Enabled = false;
                    btnActualizar.Enabled = false;
                    btnCancelar.Enabled = false;
                    btnGenerar.Enabled = false;
                    btnPago.Disabled = true;
                    btnFinalizar.Enabled = false;

                }

                //verificar estatus
                if (detalle.estatus > 1)
                {
                    if (detalle.calificacionID != null)
                    {
                        Session["calificacionID"] = (int)detalle.calificacionID;
                    }
                    else
                    {
                        editarCalificacion.Visible = false;
                        lbCalificar.Visible = true;
                    }

                    //descripcionCortaTrabajo.Enabled = false;
                    //descripcionAmpliada.Enabled = false;
                    //fechaInicio.Enabled = false;
                    //fechaConclusion.Enabled = false;
                    //montoAcordado.Enabled = false;
                    //btnActualizar.Enabled = false;
                    //btnCancelar.Enabled = false;
                    //btnGenerar.Enabled = false;
                    btnPago.Disabled = true;
                    //btnFinalizar.Enabled = false;
                    mostrarModalCalificacion((bool)empleado.DetalleContrataciones.Select(x => x.calificado).FirstOrDefault());
                    calificacionActual();
                }
            }
        }
        void calificacionActual()
        {
            editarCalificacion.Visible = true;
            lbCalificar.InnerText = "Calificacion Suministrada";
            EmpleadosService es = new EmpleadosService();
            //obtener calificacion
            var vista = es.obtenerVistaTemporal(Convert.ToInt32(Session["contratacionID"]), Session["userID"].ToString());

            decimal promedio = Convert.ToDecimal(vista.puntualidad + vista.recomendacion + vista.conocimientos + vista.cumplimiento) / 4;
            hiddenCalificacion.Value = promedio.ToString();
        }
        void mostrarModalCalificacion(bool calificado)
        {
            if (!calificado)
            {

                string script = @"<script type='text/javascript'>
                              $(document).ready(function () {
                                $('#modalCalificar').modal('show');
                              });
                            </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowModalScript", script);
            }

        }
        void obtenerPagos(int contratacionID, EmpleadosService es)
        {
            var result = es.GetEmpleador_RecibosContratacionesByID(contratacionID,Convert.ToInt32(Session["detalleID"]));
            if (result != null)
            {
                repeaterPagos.DataSource = result;
                repeaterPagos.DataBind();

                lbMontoAcordado.InnerText = Convert.ToDecimal(Session["montoAcordado"]).ToString("N2");
                pagosRealizados.InnerText = Convert.ToDecimal(result.Sum(a => a.Monto)).ToString("N2");
                montoPendiente.InnerText = (Convert.ToDecimal(Session["montoAcordado"]) - Convert.ToDecimal(result.Sum(a => a.Monto))).ToString("N2");

            }
        }
        public class PagoNomina
        {
            public int contratacionID { get; set; }
            public string Concepto { get; set; }
            public decimal Monto { get; set; }

        }
        static List<PagoNomina> pnL = new List<PagoNomina>();
        [WebMethod]
        [ScriptMethod]
        public static bool Calificar(int contratacionID,string userID,int calificacionID, int puntualidad, int cumplimiento, int conocimientos, int recomendacion)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies["login"];

            Calificaciones cal = new Calificaciones();
            CalificacionesService cs = new CalificacionesService();
            EmpleadosService es = new EmpleadosService();
            // Verificar calificación

            var result = es.obtenerFichaTemporales(contratacionID, userID);
            if (result.DetalleContrataciones.Select(x => x.calificado).FirstOrDefault() == true && calificacionID == 0)
            {

                return false; //Ya fue calificado

            }

            cal.fecha = DateTime.Now;
            cal.userID = myCookie["userID"];
            if (empleado.tipo == 1)
            {
                cal.tipo = "Persona Fisica";
            }
            else if (empleado.tipo == 2)
            {
                cal.tipo = "Empresa";
            }
            if (empleado.tipo == 1)
            {
                cal.identificacion = empleado.identificacion;

            }
            else if (empleado.tipo == 2)
            {
                cal.identificacion = empleado.rnc;

            }

            if (empleado.tipo == 1)
            {
                cal.nombre = empleado.nombre + " " + empleado.apellido;

            }
            else if (empleado.tipo == 2)
            {
                cal.nombre = empleado.nombreComercial;

            }

            cal.puntualidad = puntualidad;
            cal.cumplimiento = cumplimiento;
            cal.conocimientos = conocimientos;
            cal.recomendacion = recomendacion;
            if (calificacionID > 0)
            {
                cal.calificacionID = calificacionID;
                es.modificarCalificacionDeContratacion(cal);
            }
            else
            {
                var calificado = cs.calificarPerfil(cal);

                es.calificarContratacion(contratacionID, calificado.calificacionID);

            }

            return true; // Se cumplen las reglas, devuelve true
        }
        protected string GetStarRating(decimal calificacion = 0)
        {

            StringBuilder starsHtml = new StringBuilder();

            // Calcula la cantidad de estrellas llenas y medias
            int estrellasLlenas = (int)Math.Floor(calificacion);
            int estrellasMedias = (int)Math.Ceiling(calificacion - estrellasLlenas);

            for (int i = 1; i <= 5; i++)
            {
                if (i <= estrellasLlenas)
                {
                    // Estrella llena.
                    starsHtml.Append("<span class='bi bi-star-fill' style='color:darkorange'></span>");
                }
                else if (i <= estrellasLlenas + estrellasMedias)
                {
                    // Estrella media.
                    starsHtml.Append("<span class='bi bi-star-half' style='color:darkorange;margin-right:5px;'></span>");

                }
                else
                {
                    // Estrella vacía.
                    starsHtml.Append("<span class='bi bi-star-o' style='color:darkgray'></span>");
                }
            }

            return starsHtml.ToString();
        }
        protected void editarCalificacion_ServerClick(object sender, EventArgs e)
        {
            CalificacionesService cs = new CalificacionesService();
            var result = cs.getCalificacionByID(Convert.ToInt32(Session["calificacionID"]));
            if (result != null)
            {


                string script = @"<script type='text/javascript'>
                              $(document).ready(function () {
                                $('#modalCalificar').modal('show');
                              });
                            </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowModalScript", script);
            }
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            PagoNomina pn = new PagoNomina();
            pnL.Clear();
            //Buscar items para pago de nomina

            //Salario
            pn.Monto = Convert.ToDecimal(montoPago.Text);
            pn.Concepto = ddlConceptoPago.Text;


            pn.contratacionID = Convert.ToInt32(Session["contratacionID"]);

            pnL.Add(pn);
            montoPago.Text = null;


            gridDetallePago.DataSource = pnL;
            gridDetallePago.DataBind();
            UpdatePanelModal.Update();
        }

        protected void gridDetallePago_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "btnQuitar")
            {
                object value = gridDetallePago.GetRowValues(e.VisibleIndex, "Concepto");

                var r = pnL.Where(a => a.Concepto == value.ToString()).FirstOrDefault();
                pnL.Remove(r);
                gridDetallePago.DataSource = pnL;
                gridDetallePago.DataBind();
                UpdatePanelModal.Update();
            }
        }
        public void procesarPago(string concepto = null)
        {
            if (Convert.ToDecimal(pnL.Sum(a => a.Monto)) > Convert.ToDecimal(montoPendiente.InnerText))
            {
                string url = "fichaColaboradorTemportal.aspx?contratacionID=" + Session["contratacionID"];
                string script = @"<script>
                    Swal.fire({
                        title: 'No Puede aplicar este pago',
                        text: 'No puede aplicar un pago por un monto mayor al pendiente',
                        icon: 'error',
                        confirmButtonText: 'Aceptar'
                    });
                </script>";
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);

                return;
            }
            else if (Convert.ToDecimal(pnL.Sum(a => a.Monto)) < 1)
            {
                string url = "fichaColaboradorTemportal.aspx?contratacionID=" + Session["contratacionID"];
                string script = @"<script>
                    Swal.fire({
                        title: 'No Puede aplicar este pago',
                        text: 'No puede aplicar un pago por un monto 0',
                        icon: 'error',
                        confirmButtonText: 'Aceptar'
                    });
                </script>";
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);

                return;
            }


            Empleador_Recibos_Header_Contrataciones header = new Empleador_Recibos_Header_Contrataciones();
            header.userID = Session["userID"].ToString();
            header.fechaRegistro = DateTime.Now;
            header.fechaPago = fechaPago.Date;
            header.conceptoPago = ddlConceptoPago.Text + " de " + descripcionCortaTrabajo.Text;
            header.contratacionID = Convert.ToInt32(Session["contratacionID"]);
            header.detalleID = Convert.ToInt32(Session["detalleID"]);
            List<Empleador_Recibos_Detalle_Contrataciones> detalleList = new List<Empleador_Recibos_Detalle_Contrataciones>();

            if (string.IsNullOrEmpty(concepto))
            {
                foreach (var item in pnL)
                {
                    Empleador_Recibos_Detalle_Contrataciones detalle = new Empleador_Recibos_Detalle_Contrataciones();

                    detalle.Concepto = item.Concepto;
                    detalle.Monto = item.Monto;
                    detalleList.Add(detalle);
                }
            }
            else
            {

            }


            EmpleadosService sr = new EmpleadosService();
            var result = sr.procesarPagoContratacion(header, detalleList);
            if (result.ToString() != null)
            {

                imprimirReciboPago(result, HiddenField1.Value.ToString());


            }


        }
        public void imprimirReciboPago(int id, string userID)
        {
            HttpCookie myCookie = Request.Cookies["login"];

            string url = "";
            VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);
            if (objetoDesdeCookie.tipoIdentificacion == 1)
            {
                url = "Impresion/PrintViewer.aspx?documento=ReciboPagoPersonaFisica_EmpleadorContratacion&id=" + id + "&userID=" + userID; // Ruta a la página que abrirá la ventana nueva
            }
            else if (objetoDesdeCookie.tipoIdentificacion == 2)
            {
                url = "Impresion/PrintViewer.aspx?documento=ReciboPagoEmpresa_EmpleadorContratacion&id=" + id + "&userID=" + userID; // Ruta a la página que abrirá la ventana nueva

            }
            string script = $"window.open('{url}', '_blank', 'width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPrintWindow", script, true);

            obtenerFicha(Convert.ToInt32(Session["contratacionID"]), userID);
        }
        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            procesarPago();

        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            var detalle = new DetalleContrataciones();
            detalle = empleado.DetalleContrataciones.FirstOrDefault();

            detalle.descripcionCorta = descripcionCortaTrabajo.Text;
            detalle.descripcionAmpliada = descripcionAmpliada.Text;
            detalle.fechaInicio = fechaInicio.Date;
            detalle.fechaFinal = fechaConclusion.Date;
            detalle.montoAcordado = Convert.ToDecimal(montoAcordado.Text);
            detalle.esquemaPagos = ddlEsquema.SelectedValue;

            EmpleadosService es = new EmpleadosService();
            es.actualizarContratacion(detalle);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfirmacionEliminar", "ConfirmarCancelacionTrabajo(" + Session["contratacionID"]+ "," + Session["detalleID"] + ");", true);

        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {

        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            Button btnEliminar = (Button)sender;
            int pagoID = Convert.ToInt32(btnEliminar.CommandArgument);

            // Llama a una función JavaScript para mostrar el diálogo SweetAlert de confirmación
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfirmacionEliminar", "ConfirmarEliminarRecibo(" + pagoID + ");", true);
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string id = btn.CommandArgument;



            HttpCookie myCookie = Request.Cookies["login"];

            string url = "";
            VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);
            if (objetoDesdeCookie.tipoIdentificacion == 1)
            {
                url = "Impresion/PrintViewer.aspx?documento=ReciboPagoPersonaFisica_EmpleadorContratacion&id=" + id + "&userID=" + Session["userID"]; // Ruta a la página que abrirá la ventana nueva
            }
            else if (objetoDesdeCookie.tipoIdentificacion == 2)
            {
                url = "Impresion/PrintViewer.aspx?documento=ReciboPagoEmpresa_EmpleadorContratacion&id=" + id + "&userID=" + Session["userID"]; // Ruta a la página que abrirá la ventana nueva

            }
            string script = $"window.open('{url}', '_blank', 'width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPrintWindow", script, true);
        }

        [WebMethod]
        public static string AnularRecibo(int pagoID)
        {
            try
            {
                EmpleadosService es = new EmpleadosService();
                es.eliminarReciboContratacion(pagoID);

                // Luego, muestra un mensaje de éxito
                string mensaje = "El recibo con ID " + pagoID + " ha sido anulado correctamente.";
                return mensaje;
            }
            catch (Exception ex)
            {
                // En caso de error, muestra un mensaje de error
                return "Error al anular el recibo: " + ex.Message;
            }
        }
        [WebMethod]
        public static string CancelarTrabajo(int contratacionID, int detalleID)
        {
            try
            {
                EmpleadosService es = new EmpleadosService();
                es.cancelarTrabajo(contratacionID, detalleID);

                // Luego, muestra un mensaje de éxito
                string mensaje = "El trabajo con ID " + contratacionID + " ha sido cancelado correctamente.";
                return mensaje;
            }
            catch (Exception ex)
            {
                // En caso de error, muestra un mensaje de error
                return "Error al cancelar el trabajo: " + ex.Message;
            }
        }

    }
}