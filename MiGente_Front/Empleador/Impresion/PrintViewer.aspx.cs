using DevExpress.Pdf.Native;
using MiGente_Front.Data;
using MiGente_Front.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.Empleador.Impresion
{
    public partial class PrintViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["pagoID"] = Convert.ToInt32(Request.QueryString["id"]);
                Session["migente_userID"]  = Request.QueryString["userID"];
                Session["empleadoID"]  = Convert.ToInt32(Request.QueryString["empleadoID"]);

                LoadHtmlContent(Request.QueryString["documento"]);
            }
        }

        private void LoadHtmlContent(string documento)
        {
            if (documento == "ReciboPagoPersonaFisica_Empleador1")
            {
                //Obtener Datos del Recibo
                EmpleadosService es = new EmpleadosService();
                var result = es.GetEmpleador_ReciboByPagoID(Convert.ToInt32(Session["pagoID"]));
                if (result != null)
                {
                    // Leer el archivo HTML y realizar reemplazos necesarios en el contenido
                    string htmlContent = File.ReadAllText(Server.MapPath("~/Empleador/Impresion/" + documento + ".html"));
                    htmlContent = htmlContent.Replace("#nombreEmpleado#", result.Empleados.Nombre + " " + result.Empleados.Apellido);
                    htmlContent = htmlContent.Replace("#cedula#", result.Empleados.identificacion);
                    htmlContent = htmlContent.Replace("#direccion#", result.Empleados.direccion + ", " + result.Empleados.provincia + ", " + result.Empleados.municipio + ",");


                    HttpCookie myCookie = Request.Cookies["login"];

                    if (myCookie != null)
                    {

                        VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);


                        htmlContent = htmlContent.Replace("#nombreEmpleador#", objetoDesdeCookie.Nombre + " " + objetoDesdeCookie.Apellido);
                        htmlContent = htmlContent.Replace("#cedulaEmpleador#", objetoDesdeCookie.identificacion);
                        htmlContent = htmlContent.Replace("#montoLetras#", NumeroEnLetras.NumerosALetras(Convert.ToDecimal(result.Empleador_Recibos_Detalle.Sum(x => x.Monto))));
                        htmlContent = htmlContent.Replace("#montoNumeros#", Convert.ToDecimal(result.Empleador_Recibos_Detalle.Sum(x => x.Monto)).ToString("N2"));
                        htmlContent = htmlContent.Replace("#conceptoPago#", result.conceptoPago);


                        string tablaHtml = ConvertirListaAHtml(result.Empleador_Recibos_Detalle.ToList()); ;

                        htmlContent = htmlContent.Replace("#distribucion#", tablaHtml);
                        htmlContent = htmlContent.Replace("#cedula#", result.Empleados.identificacion);
                    }




                    // Agregar el contenido HTML al div
                    printContent.InnerHtml = htmlContent;
                }



            }
            else if (documento == "ReciboPagoEmpresa_Empleador1")
            {
                //Obtener Datos del Recibo
                EmpleadosService es = new EmpleadosService();
                var result = es.GetEmpleador_ReciboByPagoID(Convert.ToInt32(Session["pagoID"]));
                if (result != null)
                {
                    // Leer el archivo HTML y realizar reemplazos necesarios en el contenido
                    string htmlContent = File.ReadAllText(Server.MapPath("~/Empleador/Impresion/" + documento + ".html"));
                    htmlContent = htmlContent.Replace("#nombreEmpleado#", result.Empleados.Nombre + " " + result.Empleados.Apellido);
                    htmlContent = htmlContent.Replace("#cedula#", result.Empleados.identificacion);
                    htmlContent = htmlContent.Replace("#direccion#", result.Empleados.direccion + ", " + result.Empleados.provincia + ", " + result.Empleados.municipio + ",");


                    HttpCookie myCookie = Request.Cookies["login"];

                    if (myCookie != null)
                    {

                        VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);

                        htmlContent = htmlContent.Replace("#nombreComercial#", objetoDesdeCookie.nombreComercial);
                        htmlContent = htmlContent.Replace("#rncEmpleador#", objetoDesdeCookie.identificacion);
                        htmlContent = htmlContent.Replace("#direccionEmpresa#", objetoDesdeCookie.direccion);

                        htmlContent = htmlContent.Replace("#nombreGerente#", objetoDesdeCookie.nombreGerente);
                        htmlContent = htmlContent.Replace("#cedulaGerente#", objetoDesdeCookie.cedulaGerente);
                        htmlContent = htmlContent.Replace("#montoLetras#", NumeroEnLetras.NumerosALetras(Convert.ToDecimal(result.Empleador_Recibos_Detalle.Sum(x => x.Monto))));
                        htmlContent = htmlContent.Replace("#montoNumeros#", Convert.ToDecimal(result.Empleador_Recibos_Detalle.Sum(x => x.Monto)).ToString("N2"));
                        htmlContent = htmlContent.Replace("#conceptoPago#", result.conceptoPago);


                        string tablaHtml = ConvertirListaAHtml(result.Empleador_Recibos_Detalle.ToList()); ;

                        htmlContent = htmlContent.Replace("#distribucion#", tablaHtml);
                        htmlContent = htmlContent.Replace("#cedula#", result.Empleados.identificacion);
                    }




                    // Agregar el contenido HTML al div
                    printContent.InnerHtml = htmlContent;
                }



            }
            else if (documento == "ContratoPersonaFisica")
            {
                CultureInfo culture = new CultureInfo("es-ES");
                //Obtener Datos del Recibo
                EmpleadosService es = new EmpleadosService();
                int empleadoID = Convert.ToInt32((Session["empleadoID"]));
                var result = es.getEmpleados(Session["migente_userID"].ToString()).Where(x => x.empleadoID == empleadoID).SingleOrDefault();
                if (result != null)
                {
                    // Leer el archivo HTML y realizar reemplazos necesarios en el contenido
                    string htmlContent = File.ReadAllText(Server.MapPath("~/Empleador/Impresion/" + documento + ".html"));


                    htmlContent = htmlContent.Replace("#nombreEmpleado#", result.Nombre + " " + result.Apellido);
                    htmlContent = htmlContent.Replace("#nombreTrabajador#", result.Nombre + " " + result.Apellido);

                    htmlContent = htmlContent.Replace("#cedulaEmpleado#", result.identificacion);
                    htmlContent = htmlContent.Replace("#direccionEmpleado#", result.direccion + ", " + result.provincia + ", " + result.municipio + ",");
                    htmlContent = htmlContent.Replace("#posicionEmpleado#", result.posicion);

                    htmlContent = htmlContent.Replace("#diaLetra#", Convert.ToDateTime(result.fechaInicio).ToString("dddd", culture));
                    htmlContent = htmlContent.Replace("#diaNumero#", Convert.ToDateTime(result.fechaInicio).Day.ToString());

                    htmlContent = htmlContent.Replace("#mesLetra#", Convert.ToDateTime(result.fechaInicio).ToString("MMMM", culture));

                    htmlContent = htmlContent.Replace("#anoLetra#", NumeroEnLetras.NumerosALetras2(Convert.ToDateTime(result.fechaInicio).Year));
                    htmlContent = htmlContent.Replace("#anoNumero#", Convert.ToDateTime(result.fechaInicio).Year.ToString());


                    htmlContent = htmlContent.Replace("#salarioLetras#", NumeroEnLetras.NumerosALetras(Convert.ToDecimal(result.salario)));
                    htmlContent = htmlContent.Replace("#salarioNumeros#", Convert.ToDecimal(result.salario).ToString("N2"));



                    HttpCookie myCookie = Request.Cookies["login"];

                    if (myCookie != null)
                    {

                        VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);


                        htmlContent = htmlContent.Replace("#nombreEmpleador#", objetoDesdeCookie.Nombre + " " + objetoDesdeCookie.Apellido);
                        htmlContent = htmlContent.Replace("#cedulaEmpleador#", objetoDesdeCookie.identificacion);
                        htmlContent = htmlContent.Replace("#direccionEmpleador#", objetoDesdeCookie.direccion);







                    }




                    // Agregar el contenido HTML al div
                    printContent.InnerHtml = htmlContent;

                }



            }
            else if (documento == "ContratoEmpresa")
            {
                CultureInfo culture = new CultureInfo("es-ES");
                //Obtener Datos del Recibo
                EmpleadosService es = new EmpleadosService();
                int empleadoID = Convert.ToInt32(Session["empleadoID"]);

               var result = es.getEmpleados(Session["migente_userID"].ToString()).Where(x => x.empleadoID == empleadoID).SingleOrDefault();
                if (result != null)
                {
                    // Leer el archivo HTML y realizar reemplazos necesarios en el contenido
                    string htmlContent = File.ReadAllText(Server.MapPath("~/Empleador/Impresion/" + documento + ".html"));


                    htmlContent = htmlContent.Replace("#NombreTrabajador#", result.Nombre + " " + result.Apellido);

                    htmlContent = htmlContent.Replace("#nombreEmpleado#", result.Nombre + " " + result.Apellido);
                    htmlContent = htmlContent.Replace("#cedulaEmpleado#", result.identificacion);
                    htmlContent = htmlContent.Replace("#direccionEmpleado#", result.direccion + ", " + result.provincia + ", " + result.municipio + ",");
                    htmlContent = htmlContent.Replace("#posicionEmpleado#", result.posicion);

                    htmlContent = htmlContent.Replace("#diaLetra#", Convert.ToDateTime(result.fechaInicio).ToString("dddd", culture));
                    htmlContent = htmlContent.Replace("#diaNumero#", Convert.ToDateTime(result.fechaInicio).Day.ToString());

                    htmlContent = htmlContent.Replace("#mesLetra#", Convert.ToDateTime(result.fechaInicio).ToString("MMMM", culture));

                    htmlContent = htmlContent.Replace("#anoLetra#", NumeroEnLetras.NumerosALetras2(Convert.ToDateTime(result.fechaInicio).Year));
                    htmlContent = htmlContent.Replace("#anoNumero#", Convert.ToDateTime(result.fechaInicio).Year.ToString());


                    htmlContent = htmlContent.Replace("#salarioLetras#", NumeroEnLetras.NumerosALetras(Convert.ToDecimal(result.salario)));
                    htmlContent = htmlContent.Replace("#salarioNumeros#", Convert.ToDecimal(result.salario).ToString("N2"));



                    HttpCookie myCookie = Request.Cookies["login"];

                    if (myCookie != null)
                    {

                        VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);

                        htmlContent = htmlContent.Replace("#nombreComercial#", objetoDesdeCookie.nombreComercial);
                        htmlContent = htmlContent.Replace("#rncEmpleador#", objetoDesdeCookie.identificacion);
                        htmlContent = htmlContent.Replace("#direccionEmpleador#", objetoDesdeCookie.direccion);
                        htmlContent = htmlContent.Replace("#nombreGerente#", objetoDesdeCookie.nombreGerente + " " + objetoDesdeCookie.apellidoGerente);

                        htmlContent = htmlContent.Replace("#nombreEmpleador#", objetoDesdeCookie.nombreGerente + " " + objetoDesdeCookie.apellidoGerente);


                        htmlContent = htmlContent.Replace("#cedulaGerente#", objetoDesdeCookie.cedulaGerente);
                        htmlContent = htmlContent.Replace("#direccionGerente#", objetoDesdeCookie.direccionGerente);





                    }




                    // Agregar el contenido HTML al div
                    printContent.InnerHtml = htmlContent;
                }



            }
            else if (documento == "ReciboDescargoEmpresa_Empleador1")
            {
                CultureInfo culture = new CultureInfo("es-ES");
                //Obtener Datos del Recibo
                EmpleadosService es = new EmpleadosService();
                var result = es.GetEmpleador_ReciboByPagoID(Convert.ToInt32(Session["pagoID"]));
                if (result != null)
                {
                    // Leer el archivo HTML y realizar reemplazos necesarios en el contenido
                    string htmlContent = File.ReadAllText(Server.MapPath("~/Empleador/Impresion/" + documento + ".html"));
                    htmlContent = htmlContent.Replace("#nombreEmpleado#", result.Empleados.Nombre + " " + result.Empleados.Apellido);
                    htmlContent = htmlContent.Replace("#cedulaEmpleado#", result.Empleados.identificacion);
                    htmlContent = htmlContent.Replace("#direccionEmpleado#", result.Empleados.direccion + ", " + result.Empleados.provincia + ", " + result.Empleados.municipio + ",");

                    //obtener tiempo laborando
                    var empleado = es.getEmpleadosByID(Guid.Parse(Session["migente_userID"].ToString()), Convert.ToInt32(Session["empleadoID"]));
                    var tiempoLetras = CalcularDiferenciaDeFechasEnLetras(Convert.ToDateTime(empleado.fechaInicio), Convert.ToDateTime(empleado.fechaSalida));

                    htmlContent = htmlContent.Replace("#tiempoLaborando#", tiempoLetras);
                    htmlContent = htmlContent.Replace("#motivoDesvinculacion#", empleado.motivoBaja);
                    htmlContent = htmlContent.Replace("#fechaDesvinculacion#", empleado.fechaSalida.ToString());
                    htmlContent = htmlContent.Replace("#montoLetras#", NumeroEnLetras.NumerosALetras(Convert.ToDecimal(result.Empleador_Recibos_Detalle.Sum(x => x.Monto))));
                    htmlContent = htmlContent.Replace("#montoNumeros#", Convert.ToDecimal(result.Empleador_Recibos_Detalle.Sum(x => x.Monto)).ToString("N2"));
                    htmlContent = htmlContent.Replace("#fechaSalida#", empleado.fechaSalida.ToString());


                    htmlContent = htmlContent.Replace("#diaNumero#", Convert.ToDateTime(DateTime.Now).Day.ToString());

                    htmlContent = htmlContent.Replace("#mesNumero#", Convert.ToDateTime(DateTime.Now).ToString("MMMM", culture));

                    htmlContent = htmlContent.Replace("#anoNumero#", Convert.ToDateTime(DateTime.Now).Year.ToString());


                    HttpCookie myCookie = Request.Cookies["login"];

                    if (myCookie != null)
                    {

                        VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);

                        htmlContent = htmlContent.Replace("#nombreEmpleador#", objetoDesdeCookie.nombreComercial);
                        htmlContent = htmlContent.Replace("#cedulaEmpleador#", objetoDesdeCookie.identificacion);
                        htmlContent = htmlContent.Replace("#direccionEmpleador#", objetoDesdeCookie.direccion);
                        htmlContent = htmlContent.Replace("#nombreGerente#", objetoDesdeCookie.nombreGerente + " " + objetoDesdeCookie.apellidoGerente);



                        htmlContent = htmlContent.Replace("#cedulaGerente#", objetoDesdeCookie.cedulaGerente);
                        htmlContent = htmlContent.Replace("#direccionGerente#", objetoDesdeCookie.direccionGerente);





                    }




                    // Agregar el contenido HTML al div
                    printContent.InnerHtml = htmlContent;
                }



            }
            else if (documento == "ReciboDescargoPersonaFisica_Empleador1")
            {
                CultureInfo culture = new CultureInfo("es-ES");
                //Obtener Datos del Recibo
                EmpleadosService es = new EmpleadosService();
                var result = es.GetEmpleador_ReciboByPagoID(Convert.ToInt32(Session["pagoID"]));
                if (result != null)
                {
                    // Leer el archivo HTML y realizar reemplazos necesarios en el contenido
                    string htmlContent = File.ReadAllText(Server.MapPath("~/Empleador/Impresion/" + documento + ".html"));
                    htmlContent = htmlContent.Replace("#nombreEmpleado#", result.Empleados.Nombre + " " + result.Empleados.Apellido);
                    htmlContent = htmlContent.Replace("#cedulaEmpleado#", result.Empleados.identificacion);
                    htmlContent = htmlContent.Replace("#direccionEmpleado#", result.Empleados.direccion + ", " + result.Empleados.provincia + ", " + result.Empleados.municipio + ",");

                    //obtener tiempo laborando
                    var empleado = es.getEmpleadosByID(Guid.Parse(Session["migente_userID"].ToString()), Convert.ToInt32(Session["empleadoID"]));
                    var tiempoLetras = CalcularDiferenciaDeFechasEnLetras(Convert.ToDateTime(empleado.fechaInicio), Convert.ToDateTime(empleado.fechaSalida));

                    htmlContent = htmlContent.Replace("#tiempoLaborando#", tiempoLetras);
                    htmlContent = htmlContent.Replace("#motivoDesvinculacion#", empleado.motivoBaja);
                    htmlContent = htmlContent.Replace("#fechaDesvinculacion#", empleado.fechaSalida.ToString());
                    htmlContent = htmlContent.Replace("#montoLetras#", NumeroEnLetras.NumerosALetras(Convert.ToDecimal(result.Empleador_Recibos_Detalle.Sum(x => x.Monto))));
                    htmlContent = htmlContent.Replace("#montoNumeros#", Convert.ToDecimal(result.Empleador_Recibos_Detalle.Sum(x => x.Monto)).ToString("N2"));
                    htmlContent = htmlContent.Replace("#fechaSalida#", empleado.fechaSalida.ToString());


                    htmlContent = htmlContent.Replace("#diaNumero#", Convert.ToDateTime(DateTime.Now).Day.ToString());

                    htmlContent = htmlContent.Replace("#mesNumero#", Convert.ToDateTime(DateTime.Now).ToString("MMMM", culture));

                    htmlContent = htmlContent.Replace("#anoNumero#", Convert.ToDateTime(DateTime.Now).Year.ToString());


                    HttpCookie myCookie = Request.Cookies["login"];

                    if (myCookie != null)
                    {

                        VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);

                        htmlContent = htmlContent.Replace("#nombreComercial#", objetoDesdeCookie.nombreComercial);
                        htmlContent = htmlContent.Replace("#rncEmpleador#", objetoDesdeCookie.identificacion);
                        htmlContent = htmlContent.Replace("#direccionEmpleador#", objetoDesdeCookie.direccion);
                        htmlContent = htmlContent.Replace("#nombreGerente#", objetoDesdeCookie.nombreGerente + " " + objetoDesdeCookie.apellidoGerente);



                        htmlContent = htmlContent.Replace("#cedulaGerente#", objetoDesdeCookie.cedulaGerente);
                        htmlContent = htmlContent.Replace("#direccionGerente#", objetoDesdeCookie.direccionGerente);





                    }




                    // Agregar el contenido HTML al div
                    printContent.InnerHtml = htmlContent;
                }



            }
            else if (documento == "ReciboPagoPersonaFisica_EmpleadorContratacion")
            {
                documento = "ReciboPagoPersonaFisica_Empleador1";
                //Obtener Datos del Recibo
                EmpleadosService es = new EmpleadosService();
                var result = es.GetContratacion_ReciboByPagoID(Convert.ToInt32(Session["pagoID"]));
                if (result != null)
                {
                    // Leer el archivo HTML y realizar reemplazos necesarios en el contenido
                    string htmlContent = File.ReadAllText(Server.MapPath("~/Empleador/Impresion/" + documento + ".html"));
                    htmlContent = htmlContent.Replace("#nombreEmpleado#", result.EmpleadosTemporales.nombre + " " + result.EmpleadosTemporales.apellido);
                    htmlContent = htmlContent.Replace("#cedula#", result.EmpleadosTemporales.identificacion);
                    htmlContent = htmlContent.Replace("#direccion#", result.EmpleadosTemporales.direccion + ", " + result.EmpleadosTemporales.provincia + ", " + result.EmpleadosTemporales.municipio + ",");


                    HttpCookie myCookie = Request.Cookies["login"];

                    if (myCookie != null)
                    {

                        VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);


                        htmlContent = htmlContent.Replace("#nombreEmpleador#", objetoDesdeCookie.Nombre + " " + objetoDesdeCookie.Apellido);
                        htmlContent = htmlContent.Replace("#cedulaEmpleador#", objetoDesdeCookie.identificacion);
                        htmlContent = htmlContent.Replace("#montoLetras#", NumeroEnLetras.NumerosALetras(Convert.ToDecimal(result.Empleador_Recibos_Detalle_Contrataciones.Sum(x => x.Monto))));
                        htmlContent = htmlContent.Replace("#montoNumeros#", Convert.ToDecimal(result.Empleador_Recibos_Detalle_Contrataciones.Sum(x => x.Monto)).ToString("N2"));
                        htmlContent = htmlContent.Replace("#conceptoPago#", result.conceptoPago);


                        string tablaHtml = ConvertirListaAHtmlContrataciones(result.Empleador_Recibos_Detalle_Contrataciones.ToList()); ;

                        htmlContent = htmlContent.Replace("#distribucion#", tablaHtml);
                        htmlContent = htmlContent.Replace("#cedula#", result.EmpleadosTemporales.identificacion);
                    }




                    // Agregar el contenido HTML al div
                    printContent.InnerHtml = htmlContent;
                }



            }
            else if (documento == "ReciboPagoEmpresa_EmpleadorContratacion")
            {
                documento = "ReciboPagoEmpresa_Empleador1";
                //Obtener Datos del Recibo
                EmpleadosService es = new EmpleadosService();
                var result = es.GetContratacion_ReciboByPagoID(Convert.ToInt32(Session["pagoID"]));
                if (result != null)
                {
                    // Leer el archivo HTML y realizar reemplazos necesarios en el contenido
                    string htmlContent = File.ReadAllText(Server.MapPath("~/Empleador/Impresion/" + documento + ".html"));
                    htmlContent = htmlContent.Replace("#nombreEmpleado#", result.EmpleadosTemporales.nombre + " " + result.EmpleadosTemporales.apellido);
                    htmlContent = htmlContent.Replace("#cedula#", result.EmpleadosTemporales.identificacion);
                    htmlContent = htmlContent.Replace("#direccion#", result.EmpleadosTemporales.direccion + ", " + result.EmpleadosTemporales.provincia + ", " + result.EmpleadosTemporales.municipio + ",");


                    HttpCookie myCookie = Request.Cookies["login"];

                    if (myCookie != null)
                    {

                        VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);

                        htmlContent = htmlContent.Replace("#nombreComercial#", objetoDesdeCookie.nombreComercial);
                        htmlContent = htmlContent.Replace("#rncEmpleador#", objetoDesdeCookie.identificacion);
                        htmlContent = htmlContent.Replace("#direccionEmpresa#", objetoDesdeCookie.direccion);

                        htmlContent = htmlContent.Replace("#nombreGerente#", objetoDesdeCookie.nombreGerente);
                        htmlContent = htmlContent.Replace("#cedulaGerente#", objetoDesdeCookie.cedulaGerente);
                        htmlContent = htmlContent.Replace("#montoLetras#", NumeroEnLetras.NumerosALetras(Convert.ToDecimal(result.Empleador_Recibos_Detalle_Contrataciones.Sum(x => x.Monto))));
                        htmlContent = htmlContent.Replace("#montoNumeros#", Convert.ToDecimal(result.Empleador_Recibos_Detalle_Contrataciones.Sum(x => x.Monto)).ToString("N2"));
                        htmlContent = htmlContent.Replace("#conceptoPago#", result.conceptoPago);


                        string tablaHtml = ConvertirListaAHtmlContrataciones(result.Empleador_Recibos_Detalle_Contrataciones.ToList()); ;

                        htmlContent = htmlContent.Replace("#distribucion#", tablaHtml);
                        htmlContent = htmlContent.Replace("#cedula#", result.EmpleadosTemporales.identificacion);
                    }




                    // Agregar el contenido HTML al div
                    printContent.InnerHtml = htmlContent;
                }



            }
        }

        public static string ConvertirListaAHtml(List<Empleador_Recibos_Detalle> detalles)
        {
            StringBuilder tablaHtml = new StringBuilder();

            tablaHtml.Append("<table class=\"table table-bordered table-striped\">");
            tablaHtml.Append("<thead class=\"thead-dark\">");
            tablaHtml.Append("<tr><th>Concepto</th><th>Monto</th></tr>");
            tablaHtml.Append("</thead>");
            tablaHtml.Append("<tbody>");

            foreach (var detalle in detalles)
            {
                tablaHtml.Append("<tr>");
                tablaHtml.Append("<td>" + detalle.Concepto + "</td>");
                tablaHtml.Append("<td>" + Convert.ToDecimal(detalle.Monto).ToString("C2") + "</td>");
                tablaHtml.Append("</tr>");
            }
            tablaHtml.Append("<td style='font-weight:bold; position: relative;'>" +
       "<span style='position: absolute; top: 0; left: 0; right: 0; border-top: 2px solid black;'></span>" +
       "Total a Pagar" +
       "<span style='position: absolute; bottom: 0; left: 0; right: 0; border-top: 2px solid black;'></span>" +
       "</td>");
            tablaHtml.Append("<td style='font-weight:bold; position: relative;'>" +
     "<span style='position: absolute; top: 0; left: 0; right: 0; border-top: 2px solid black;'></span>" +
     Convert.ToDecimal(detalles.Sum(x => x.Monto)).ToString("C2") +
     "<span style='position: absolute; bottom: 0; left: 0; right: 0; border-top: 2px solid black;'></span>" +
     "</td>");
            tablaHtml.Append("</tbody>");
            tablaHtml.Append("</table>");

            return tablaHtml.ToString();
        }

        public static string ConvertirListaAHtmlContrataciones(List<Empleador_Recibos_Detalle_Contrataciones> detalles)
        {
            StringBuilder tablaHtml = new StringBuilder();

            tablaHtml.Append("<table class=\"table table-bordered table-striped\">");
            tablaHtml.Append("<thead class=\"thead-dark\">");
            tablaHtml.Append("<tr><th>Concepto</th><th>Monto</th></tr>");
            tablaHtml.Append("</thead>");
            tablaHtml.Append("<tbody>");

            foreach (var detalle in detalles)
            {
                tablaHtml.Append("<tr>");
                tablaHtml.Append("<td>" + detalle.Concepto + "</td>");
                tablaHtml.Append("<td>" + Convert.ToDecimal(detalle.Monto).ToString("C2") + "</td>");
                tablaHtml.Append("</tr>");
            }
            tablaHtml.Append("<td style='font-weight:bold; position: relative;'>" +
       "<span style='position: absolute; top: 0; left: 0; right: 0; border-top: 2px solid black;'></span>" +
       "Total a Pagar" +
       "<span style='position: absolute; bottom: 0; left: 0; right: 0; border-top: 2px solid black;'></span>" +
       "</td>");
            tablaHtml.Append("<td style='font-weight:bold; position: relative;'>" +
     "<span style='position: absolute; top: 0; left: 0; right: 0; border-top: 2px solid black;'></span>" +
     Convert.ToDecimal(detalles.Sum(x => x.Monto)).ToString("C2") +
     "<span style='position: absolute; bottom: 0; left: 0; right: 0; border-top: 2px solid black;'></span>" +
     "</td>");
            tablaHtml.Append("</tbody>");
            tablaHtml.Append("</table>");

            return tablaHtml.ToString();
        }
        public static string CalcularDiferenciaDeFechasEnLetras(DateTime fechaInicio, DateTime fechaFin)
        {
            TimeSpan diferencia = fechaFin - fechaInicio;
            int años = diferencia.Days / 365;
            int meses = (diferencia.Days % 365) / 30;
            int dias = (diferencia.Days % 365) % 30;

            string resultado = "";

            if (años > 0)
            {
                resultado += años + " año(s)";
                if (meses > 0 || dias > 0)
                    resultado += ", ";
            }

            if (meses > 0)
            {
                resultado += meses + " mes(es)";
                if (dias > 0)
                    resultado += " y ";
            }

            if (dias > 0)
            {
                resultado += dias + " día(s)";
            }

            return resultado;
        }

    }
}