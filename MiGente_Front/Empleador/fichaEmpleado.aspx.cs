using DevExpress.Pdf.Native;
using DevExpress.Web;
using MiGente_Front.Data;
using MiGente_Front.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static MiGente_Front.FormularioEmpleado;

namespace MiGente_Front.Empleador
{
    public partial class fichaEmpleado : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fechaPago.Date = DateTime.Now;
                Session["empleadoID"] = Request.QueryString["empleadoID"];

                HttpCookie myCookie = Request.Cookies["login"];
                Session["migente_userID"] = myCookie["migente_userID"];
                HiddenField1.Value = myCookie["migente_userID"];

                if (myCookie["planID"] != "3") chkTss.Enabled = false; chkTss.Enabled = false;
                obtenerFicha();

                if (!chkActivo.Checked)
                {
                    // Llama a la función para mostrar la alerta
                    //ScriptManager.RegisterStartupScript(this, GetType(), "MostrarAlerta", "mostrarAlertaDesactivado();", true);
                    divMensajeInactivo.Style["display"] = "block";

                }
                LlenarRecibos();

            }
        }

 
        public enum periodosPago
        {
            Semanal = 1,
            Quincenal = 2,
            Mensual = 3
        }

        public void obtenerFicha()
        {
            HttpCookie myCookie = Request.Cookies["login"];
            EmpleadosService es = new EmpleadosService();

            Empleados empleado = es.getEmpleadosByID(Guid.Parse(myCookie["migente_userID"]), Convert.ToInt32(Session["empleadoID"]));
            if (empleado != null)
            {
                Session["empleadoID"] = empleado.empleadoID;
                fechaRegistro.InnerText = Convert.ToDateTime(empleado.fechaRegistro).ToString("dd-MMM-yyyy");
                fechaInicio.InnerText = Convert.ToDateTime(empleado.fechaInicio).ToString("dd-MMM-yyyy");
                identificacion.InnerText = empleado.identificacion;
                nombre.InnerText = empleado.Nombre + " " + empleado.Apellido;
                salario.InnerText = Convert.ToDecimal(empleado.salario).ToString("N2");
                telefono1.InnerText = empleado.telefono1;
                telefono2.InnerText = empleado.telefono2;
                periodoPago.InnerText = Enum.GetName(typeof(periodosPago), empleado.periodoPago);

                contrato.InnerText = "Generar Contrato";
                chkActivo.Checked = (bool)empleado.Activo;
                chkTss.Checked = (bool)empleado.tss;

                NombreEmpleado.InnerText = nombre.InnerText;

                htmlDireccion.InnerText = empleado.direccion;
                htmlProvincia.InnerText = empleado.provincia;
                htmlMunucipio.InnerText = empleado.municipio;
                htmlEmergencia.InnerText = empleado.contactoEmergencia + " - " + empleado.telefonoEmergencia;

                string base64Image = null;

                base64Image = empleado.foto;
               Image1.ImageUrl=base64Image;

                //obtener remuneraciones extra
                List<Remuneraciones> remuneraciones = Session["Remuneraciones"] as List<Remuneraciones>;
                if (remuneraciones == null)
                {
                    remuneraciones = es.obtenerRemuneraciones(myCookie["migente_userID"], Convert.ToInt32(Session["empleadoID"]));
                    Session["Remuneraciones"] = remuneraciones;
                }



            }
        }

        protected void btnEditarPerfil_ServerClick(object sender, EventArgs e)
        {
            ModalEmpleado.SetModalContent("Modificar Ficha de Empleado Permanente",true);

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModalScript", "openModalEmpleado();", true);


        }
        public void imprimirContratoPersonaFisica_Empleador1(string userID)
        {
            string url = "Impresion/PrintViewer.aspx?documento=ContratoPersonaFisica&userID=" + userID + "&empleadoID=" + Session["empleadoID"]; // Ruta a la página que abrirá la ventana nueva
            string script = $"window.open('{url}', '_blank', 'width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPrintWindow", script, true);
        }
        public void imprimirContratoEmpresa_Empleador1(string userID)
        {
            string url = "Impresion/PrintViewer.aspx?documento=ContratoEmpresa&userID=" + userID + "&empleadoID=" + Session["empleadoID"]; // Ruta a la página que abrirá la ventana nueva
            string script = $"window.open('{url}', '_blank', 'width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPrintWindow", script, true);
        }
        protected void btnContrato_ServerClick(object sender, EventArgs e)
        {
            //obtener tipo de Perfil
            HttpCookie myCookie = Request.Cookies["login"];

            if (myCookie != null)
            {

                VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);




                if (objetoDesdeCookie.tipoIdentificacion == 1)
                {
                    imprimirContratoPersonaFisica_Empleador1(Convert.ToString(Session["migente_userID"]));

                }
                else if (objetoDesdeCookie.tipoIdentificacion == 2)
                {
                    imprimirContratoEmpresa_Empleador1(Session["migente_userID"].ToString());

                }
                else
                {      // El perfil no está completo, muestra un SweetAlert
                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert",
                        "swal('Perfil incompleto', 'Debes completar tu perfil antes de emitir contratos.', 'warning')" +
                        ".then(function() " +
                        "{ window.location = 'miPerfilEmpleador.aspx'; });",
                        true);

                }
            }
        }

        protected void btnRealizarPago_Click(object sender, EventArgs e)
        {
            fechaPago.Date = DateTime.Now;
            cbConcepto.Value= "Salario";
            cbPeriodo.Enabled = true;
            armarNovedad();

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModalScript", "openModalPagoEmpleado();", true);

        }
        public class PagoNomina
        {
            public int EmpleadoId { get; set; }
            public string Concepto { get; set; }
            public decimal Monto { get; set; }

        }
        static List<PagoNomina> pnL = new List<PagoNomina>();

        public void armarNovedad()
        {
            EmpleadosService es = new EmpleadosService();



            PagoNomina pn = new PagoNomina();
          
            pnL.Clear();

            //Buscar items para pago de nomina

            //Salario
            int dividendo = 1;
            if (cbConcepto.Text == "Salario")
            {
                if (periodoPago.InnerText == "Semanal")
                {
                    dividendo = 4;
                }
                if (periodoPago.InnerText == "Quincenal")
                {
                    dividendo = 2;
                }
                if (periodoPago.InnerText == "Mensual")
                {
                    dividendo = 1;
                }
            }
            bool fraccion = false;

            decimal dividendoFraccion = Convert.ToDecimal("23.83");

            DateTime dtfechaInicio = new DateTime(Convert.ToDateTime(fechaInicio.InnerText).Year, Convert.ToDateTime(fechaInicio.InnerText).Month, Convert.ToDateTime(fechaInicio.InnerText).Day); // Reemplaza con tu fecha de inicio
            DateTime fechaFin = new DateTime(Convert.ToDateTime(fechaPago.Date).Year, Convert.ToDateTime(fechaPago.Date).Month, Convert.ToDateTime(fechaPago.Date).Day);   // Reemplaza con tu fecha de fin

            TimeSpan diferencia = fechaFin - dtfechaInicio;


            int diasTrabajados = diferencia.Days;

            if (Convert.ToInt32(cbPeriodo.Value) == 2)
            {
                fraccion = true;
            }

            if (fraccion)
            {

                pn.Monto = (Convert.ToDecimal(salario.InnerText) / dividendoFraccion) * diasTrabajados;
                if (cbConcepto.Value == "Regalia")
                {
                    pn.Concepto = "Fraccion de Regalia Pascual";

                }
                else
                {
                    pn.Concepto = "Fraccion de Salario";

                }

            }
            else if (!fraccion)
            {
                pn.Monto = Convert.ToDecimal(salario.InnerText) / dividendo;
                if (cbConcepto.Value == "Regalia")
                {
                    pn.Concepto = "Regalia Pascual";
                }
                else
                {
                    pn.Concepto = "Salario Bruto";

                }
            }

            pn.EmpleadoId = Convert.ToInt32(Session["empleadoID"]);

            pnL.Add(pn);

            if (cbConcepto.SelectedIndex==0)
            {

                //cargar remuneraciones extra

                if (cbConcepto.Value != "Regalia")
                {

            HttpCookie myCookie = Request.Cookies["login"];

            var remuneraciones = es.obtenerRemuneraciones(myCookie["migente_userID"].ToLower(), Convert.ToInt32(Session["empleadoID"]));
            foreach (var item in remuneraciones)
            {
                pn = new PagoNomina();

                if (fraccion)
                {

                    pn.Monto = (Convert.ToDecimal(item.monto) / dividendoFraccion) * diasTrabajados;
                }
                else if (!fraccion)
                {
                    pn.Monto = Convert.ToDecimal(item.monto) / dividendo;
                }

                pn.Concepto = item.descripcion;
                pn.EmpleadoId = Convert.ToInt32(Session["empleadoID"]);

                pnL.Add(pn);
            }

            }






            if ((bool)chkTss.Checked)
            {


                //get deducciones de ley
                var resultDeducciones = es.deducciones();


                if (resultDeducciones != null)
                {
                    foreach (var item in resultDeducciones)
                    {
                        pn = new PagoNomina();
                        pn.Concepto = item.descripcion;
                        pn.EmpleadoId = Convert.ToInt32(Session["empleadoID"]);

                        gridDetallePago.DataSource = pnL;

                        gridDetallePago.DataBind();
                        UpdatePanelModal.Update();

                        if (fraccion)
                        {

                            pn.Monto = ((Convert.ToDecimal(salario.InnerText) / dividendoFraccion) * Convert.ToDecimal(item.porcentaje / 100)) * -1;

                            pn.Concepto = "Fraccion de " + item.descripcion.ToString();

                        }
                        else if (!fraccion)
                        {
                            pn.Monto = (Convert.ToDecimal(salario.InnerText) * Convert.ToDecimal(item.porcentaje / 100)) * -1;

                            pn.Concepto = item.descripcion;

                        }

                        pn.EmpleadoId = Convert.ToInt32(Session["empleadoID"]);

                        pnL.Add(pn);
                    }
                }


            }
            }


            gridDetallePago.DataSource = pnL;
            gridDetallePago.DataBind();
            UpdatePanelModal.Update();
        }

        protected void cbPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            armarNovedad();
        }

        protected void btnNuevoDescuento_Click(object sender, EventArgs e)
        {

            //obtener remuneraciones extra
            //List<PagoNomina> pnL = Session["PagoNomina"] as List<PagoNomina>;
          


            PagoNomina pn = new PagoNomina();
            pn.Concepto = descripcionDescuento.Text;
            pn.Monto = Convert.ToDecimal(montoDescuento.Text) * -1;
            pn.EmpleadoId = Convert.ToInt32(Session["empleadoID"]);

         
            pnL.Add(pn);
            gridDetallePago.DataSource = pnL;
            Session["PagoNomina"] = pnL;

            gridDetallePago.DataBind();
            UpdatePanelModal.Update();
            descripcionDescuento.Text = "";
            montoDescuento.Text = "0.00";

   

        }

        public void procesarPago(string concepto = null)
        {
            Empleador_Recibos_Header header = new Empleador_Recibos_Header();
            header.userID = Session["migente_userID"].ToString();
            header.fechaRegistro = DateTime.Now;
            header.fechaPago = fechaPago.Date;
            header.tipo = Convert.ToInt32(cbPeriodo.Value);
            header.conceptoPago = cbConcepto.Text;
            header.empleadoID = Convert.ToInt32(Session["empleadoID"]);

            List<Empleador_Recibos_Detalle> detalleList = new List<Empleador_Recibos_Detalle>();

            if (string.IsNullOrEmpty(concepto))
            {
                foreach (var item in pnL)
                {
                    Empleador_Recibos_Detalle detalle = new Empleador_Recibos_Detalle();

                    detalle.Concepto = item.Concepto;
                    detalle.Monto = item.Monto;
                    detalleList.Add(detalle);
                }
            }
            else
            {
                Empleador_Recibos_Detalle detalle = new Empleador_Recibos_Detalle();

                //detalle.Concepto = "Descargo";
                //detalle.Monto = Convert.ToDecimal(hiddenPrestaciones.Value);
                detalleList.Add(detalle);
            }


            EmpleadosService sr = new EmpleadosService();
            var result = sr.procesarPago(header, detalleList);
            if (result.ToString() != null)
            {

                imprimirReciboPago(result, Session["migente_userID"].ToString());


            }
            LlenarRecibos();

        }
        public void imprimirReciboPago(int id, string userID)
        {
            HttpCookie myCookie = Request.Cookies["login"];

            string url = "";
            VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);
            if (objetoDesdeCookie.tipoIdentificacion == 1)
            {
                url = "Impresion/PrintViewer.aspx?documento=ReciboPagoPersonaFisica_Empleador1&id=" + id + "&userID=" + userID; // Ruta a la página que abrirá la ventana nueva
            }
            else if (objetoDesdeCookie.tipoIdentificacion == 2)
            {
                url = "Impresion/PrintViewer.aspx?documento=ReciboPagoEmpresa_Empleador1&id=" + id + "&userID=" + userID; // Ruta a la página que abrirá la ventana nueva

            }
            string script = $"window.open('{url}', '_blank', 'width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPrintWindow", script, true);
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            procesarPago();

        }

        public void LlenarRecibos()
        {
            EmpleadosService es = new EmpleadosService();
            HttpCookie myCookie = Request.Cookies["login"];

            var result = es.GetEmpleador_Recibos_Empleado(myCookie["migente_UserID"], Convert.ToInt32(Session["empleadoID"]));
            repeaterPagos.DataSource=result;
            repeaterPagos.DataBind();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Button Unnamed = (Button)sender;
            int id = Convert.ToInt32(Unnamed.CommandArgument);
            imprimirReciboPago(id, Session["migente_userID"].ToString());
        }

        [WebMethod]
        public static string EliminarRecibo(int pagoID)
        {
            EmpleadosService es = new EmpleadosService();
            es.eliminarReciboEmpleado(pagoID);
            return "Recibo eliminado con éxito";
        }

        protected void btnRegalia_Click(object sender, EventArgs e)
        {
            fechaPago.Date = DateTime.Now;
            cbConcepto.Value="Regalia";
            cbPeriodo.Value = 1;
            cbPeriodo.Enabled = false;
            armarNovedad();

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModalScript", "openModalPagoEmpleado();", true);
        }

        protected void btnBaja_ServerClick(object sender, EventArgs e)
        {

            string script = $"DarDeBajaEmpleado('{Session["empleadoID"]}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarConfirmacionScript", script, true);
        }
        [WebMethod]
        public static bool imprimirDescargo()
        {

            fichaEmpleado fichaEmpleado = new fichaEmpleado();
            fichaEmpleado.imprimirReciboDescargoDescargo();

            return true;
        }
        [WebMethod]
        public static bool DarDeBaja(int empleadoID,string userID, string motivoBaja, decimal prestaciones, DateTime fechaBaja)
        {
            
            EmpleadosService es = new EmpleadosService();
            es.darDeBaja(empleadoID, userID, fechaBaja, prestaciones, motivoBaja);
            //procesar recibo de pago de prestaciones
            fichaEmpleado fichaEmpleado = new fichaEmpleado();
            fichaEmpleado.procesarPagoDescargo(prestaciones);
            return true;
        }
        public void procesarPagoDescargo(decimal prestaciones)
        {
            Empleador_Recibos_Header header = new Empleador_Recibos_Header();
            header.userID = Session["migente_userID"].ToString();
            header.fechaRegistro = DateTime.Now;
            header.fechaPago = DateTime.Now;
            header.tipo = 1;
            header.conceptoPago = "Prestaciones Laborales";
            header.empleadoID = Convert.ToInt32(Session["empleadoID"]);

            List<Empleador_Recibos_Detalle> detalleList = new List<Empleador_Recibos_Detalle>();


            Empleador_Recibos_Detalle detalle = new Empleador_Recibos_Detalle();

            detalle.Concepto = "Descargo";
            detalle.Monto = prestaciones;
            detalleList.Add(detalle);


            EmpleadosService sr = new EmpleadosService();
            var result = sr.procesarPago(header, detalleList);
            if (result.ToString() != null)
            {



            }

        }
        public void imprimirReciboDescargoDescargo()
        {
            EmpleadosService es = new EmpleadosService();
            var recibo = es.getEmpleadosByID(Guid.Parse(Session["migente_userID"].ToString()), Convert.ToInt32(Session["empleadoID"])).Empleador_Recibos_Header.Where(a => a.conceptoPago == "Prestaciones Laborales").FirstOrDefault();
            HttpCookie myCookie = Request.Cookies["login"];
            string url = "";
            VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);
            if (objetoDesdeCookie.tipoIdentificacion == 1)
            {
                url = "Impresion/PrintViewer.aspx?documento=ReciboDescargoPersonaFisica_Empleador1&id=" + recibo.pagoID + "&userID=" + Session["migente_userID"].ToString() + "&empleadoID=" + Convert.ToInt32(Session["empleadoID"]); // Ruta a la página que abrirá la ventana nueva
            }
            else if (objetoDesdeCookie.tipoIdentificacion == 2)
            {
                url = "Impresion/PrintViewer.aspx?documento=ReciboDescargoEmpresa_Empleador1&id=" + recibo.pagoID + "&userID=" + Session["migente_userID"].ToString() + "&empleadoID=" + Convert.ToInt32(Session["empleadoID"]); // Ruta a la página que abrirá la ventana nueva

            }
            string script = $"window.open('{url}', '_blank', 'width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPrintWindow", script, true);
        }

        protected void btnImprimirDescargo_ServerClick(object sender, EventArgs e)
        {
            imprimirReciboDescargoDescargo();

        }
    }
}