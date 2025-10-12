using MiGente_Front.Data;
using MiGente_Front.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.Empleador
{
    public partial class colaboradores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }

     
        protected void btnAddNew_Click(object sender, EventArgs e)
        {

           

        }


        protected void AbrirModalPermanente()
        {
            ModalEmpleado.SetModalContent("Registro de Nuevo Empleado Permanente");
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModalScript", "openModalEmpleado();", true);
        }

        protected void AbrirModalTemporal()
        {
            ModalContratacion.SetModalContent("Registro de Nuevo Contratista Temporal");
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModalScript", "openModalContratacion();", true);
        }

   
        protected void btnNew_ServerClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cbTipoColaborador.Value)==1)
            {
                Session["Remuneraciones"] = null;
                AbrirModalPermanente();
            }
            else
            {
                Session["Remuneraciones"] = null;
                AbrirModalTemporal();
            }
        }
        [WebMethod]
        public static object GetColaboradores(string userID,int pageIndex, int pageSize, string searchTerm)
        {

            // Filtrado por el término de búsqueda
            EmpleadosService es = new EmpleadosService();

            var query = es.getEmpleados(userID);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.Nombre.Contains(searchTerm) || v.identificacion.Contains(searchTerm));
            }

            // Paginación
            var totalRecords = query.Count();
            var colaboradores = query
                .OrderByDescending(v => v.fechaRegistro)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).Where(X=>X.Activo==true)
                .ToList();

            // Paginación
          

            // Retornar los datos y el número total de registros para la paginación
            return new
            {

                colaboradores = colaboradores.Select(p => new
                {
                    p.empleadoID,
                    fechaInicio= Convert.ToDateTime(p.fechaInicio).ToString("o"),
            p.identificacion,
                   Nombre =  p.Nombre + " " + p.Apellido,
                    salario = Convert.ToDecimal(Convert.ToDecimal(p.salario)).ToString("N2"),
                    p.diasPago,
                    p.foto
                }),
                totalRecords = totalRecords
            };
        }
        [WebMethod]
        public static object GetColaboradoresInactivos(string userID, int pageIndex, int pageSize, string searchTerm)
        {

            // Filtrado por el término de búsqueda
            EmpleadosService es = new EmpleadosService();

            var query = es.getEmpleados(userID).Where(x=>x.Activo==false);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.Nombre.Contains(searchTerm) || v.identificacion.Contains(searchTerm));
            }

            // Paginación
            var totalRecords = query.Count();
            var colaboradoresInactivos = query
                .OrderByDescending(v => v.fechaRegistro)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).Where(X => X.Activo == false)
                .ToList();

            // Paginación


            // Retornar los datos y el número total de registros para la paginación
            return new
            {

                colaboradoresInactivos = colaboradoresInactivos.Select(p => new
                {
                    p.empleadoID,
                    fechaInicio = Convert.ToDateTime(p.fechaInicio).Date.ToString("dd-MMM-yyyy"),
                    p.identificacion,
                    Nombre = p.Nombre + " " + p.Apellido,
                    salario = Convert.ToDecimal(Convert.ToDecimal(p.salario)).ToString("N2"),
                    p.diasPago,
                    p.foto,
                    fechaSalida = Convert.ToDateTime(p.fechaSalida).Date.ToString("dd-MMM-yyyy"),
                }),
                totalRecords = totalRecords
            };
        }

        [WebMethod]
        public static object GetContratacionesTemporales(string userID, int pageIndex, int pageSize, string searchTerm,int estatus)
        {

            // Filtrado por el término de búsqueda
            EmpleadosService es = new EmpleadosService();

            var query = es.getContrataciones(Guid.Parse(userID));

            query = query.Where(x => x.DetalleContrataciones.Any(a => a.estatus == estatus)).ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                
                query = (List<Data.EmpleadosTemporales>)query.Where(v => v.nombre.Contains(searchTerm) || v.identificacion.Contains(searchTerm) || v.nombreComercial.Contains(searchTerm));
            }

            // Paginación
            var totalRecords = query.Count();
            var contratacionesTemporales = query
                .OrderByDescending(v => v.fechaRegistro)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Paginación


            // Retornar los datos y el número total de registros para la paginación
            return new
            {

                contratacionesTemporales = contratacionesTemporales.Select(p => new
                {
                    p.contratacionID,
                    fechaRegistro = Convert.ToDateTime(p.fechaRegistro).Date.ToString("dd-MMM-yyyy"),
                    identificacion = p.identificacion + p.rnc,

                    Nombre = p.nombreComercial + p.nombre + " " + p.apellido,
                    p.telefono1,
                    p.telefono2,
                    p.foto,
                }),
                totalRecords = totalRecords
            };
        }

        public void imprimirContratoPersonaFisica_Empleador1(string userID,int empleadoID)
        {
            string url = "Impresion/PrintViewer.aspx?documento=ContratoPersonaFisica&userID=" + userID + "&empleadoID=" + empleadoID; // Ruta a la página que abrirá la ventana nueva
            string script = $"window.open('{url}', '_blank', 'width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPrintWindow", script, true);
        }
        public void imprimirContratoEmpresa_Empleador1(string userID, int empleadoID)
        {
            string url = "Impresion/PrintViewer.aspx?documento=ContratoEmpresa&userID=" + userID + "&empleadoID=" + empleadoID; // Ruta a la página que abrirá la ventana nueva
            string script = $"window.open('{url}', '_blank', 'width=800,height=600');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPrintWindow", script, true);
        }

        protected void VerContrato_Click(object sender, EventArgs e)
        {
            var VerContrato = (Button)sender;
            string empleadoID = VerContrato.CommandArgument;

            // Lógica para redirigir o procesar el contrato
            //obtener tipo de Perfil
            HttpCookie myCookie = Request.Cookies["login"];

            if (myCookie != null)
            {

                VPerfiles objetoDesdeCookie = JsonConvert.DeserializeObject<VPerfiles>(myCookie["vPerfil"]);




                if (objetoDesdeCookie.tipoIdentificacion == 1)
                {
                    imprimirContratoPersonaFisica_Empleador1(Convert.ToString(Session["migente_userID"]), Convert.ToInt32(empleadoID));

                }
                else if (objetoDesdeCookie.tipoIdentificacion == 2)
                {
                    imprimirContratoEmpresa_Empleador1(Session["migente_userID"].ToString(), Convert.ToInt32(empleadoID));

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

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/comunidad.aspx");
        }
    }
}