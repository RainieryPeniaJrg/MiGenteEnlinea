using MiGente_Front.Data;
using MiGente_Front.Services;
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
    public partial class CalificacionDePerfiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie myCookie = Request.Cookies["login"];

                HiddenField1.Value = myCookie["migente_userID"];
                fillEmpleados(myCookie["migente_UserID"].ToString());
            }

        }
        void fillEmpleados(string userID)
        {
            EmpleadosService es = new EmpleadosService();
            var result = es.getVEmpleados(userID).Where(x => x.Activo == false);

            // Asigna la fuente de datos
            ddlPerfil.DataSource = result;
            ddlPerfil.DataTextField = "Nombre"; // Asegúrate de que coincide con las propiedades del objeto
            ddlPerfil.DataValueField = "Identificacion";

            // Realiza el DataBind
            ddlPerfil.DataBind();

            // Agrega una opción por defecto al inicio
            ddlPerfil.Items.Insert(0, new ListItem("-- Seleccione --", ""));
            ddlPerfil.SelectedIndex = 0;
            seleccionDePerfil();
        }


        [WebMethod]
        public static object ConsultarPerfilPorTipoIdentificacion(string identificacion)
        {
            List<VCalificaciones> resultados = new List<VCalificaciones>();
            CalificacionesService cs = new CalificacionesService();
            resultados = cs.getById(identificacion);

            if (resultados.Any())
            {
                int totalPuntualidad = 0;
                int totalCumplimiento = 0;
                int totalConocimientos = 0;
                int totalRecomendacion = 0;


                foreach (var calificacion in resultados)
                {
                    totalPuntualidad += calificacion.puntualidad.Value;
                    totalCumplimiento += calificacion.cumplimiento.Value;
                    totalConocimientos += calificacion.conocimientos.Value;
                    totalRecomendacion += calificacion.recomendacion.Value;

                }

                int cantidadCalificaciones = resultados.Count();
                var res = resultados.SingleOrDefault();
                res.puntualidad = totalPuntualidad / cantidadCalificaciones;
                res.cumplimiento = totalCumplimiento / cantidadCalificaciones;
                res.conocimientos = totalConocimientos / cantidadCalificaciones;
                res.recomendacion = totalRecomendacion / cantidadCalificaciones;

                // Otros campos y asignaciones aquí...


                return res;



            }
            return null;


        }

        [WebMethod]
        [ScriptMethod]
        public static bool Calificar(string tipo, string identificacion, string nombre, int puntualidad, int cumplimiento, int conocimientos, int recomendacion)
        {

            HttpCookie myCookie = HttpContext.Current.Request.Cookies["login"];

            Calificaciones cal = new Calificaciones();
            CalificacionesService cs = new CalificacionesService();

            // Verificar calificación
            var result = cs.getById(identificacion, myCookie["migente_userID"].ToString()).FirstOrDefault();
            if (result != null)
            {
                int diferenciaMeses = ((DateTime.Now.Year - Convert.ToDateTime(result.fecha).Year) * 12) + DateTime.Now.Month - Convert.ToDateTime(result.fecha).Month;
                if (diferenciaMeses < 2)
                {
                    return false; // No se cumple la regla, devuelve false

                }
            }

            cal.fecha = DateTime.Now;
            cal.userID = myCookie["migente_userID"];
            if (tipo=="1")
            {
                tipo = "Persona Fisica";
            }
            else
            {
                tipo = "Empresa";
            }
            cal.tipo = tipo;
            cal.identificacion = identificacion;
            cal.nombre = nombre;
            cal.puntualidad = puntualidad;
            cal.cumplimiento = cumplimiento;
            cal.conocimientos = conocimientos;
            cal.recomendacion = recomendacion;

            cs.calificarPerfil(cal);

            return true; // Se cumplen las reglas, devuelve true
        }

        protected void gridCalificaciones_HtmlDataCellPrepared(object sender, DevExpress.Web.Bootstrap.BootstrapGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "puntualidad")
            {
                int calificacion = Convert.ToInt32(e.GetValue("puntualidad"));
                string stars = GetStarIcons(calificacion);
                e.Cell.Text = stars;
            }
            else if (e.DataColumn.FieldName == "conocimientos")
            {
                int calificacion = Convert.ToInt32(e.GetValue("conocimientos"));
                string stars = GetStarIcons(calificacion);
                e.Cell.Text = stars;
            }
            else if (e.DataColumn.FieldName == "cumplimiento")
            {
                int calificacion = Convert.ToInt32(e.GetValue("cumplimiento"));
                string stars = GetStarIcons(calificacion);
                e.Cell.Text = stars;
            }
            else if (e.DataColumn.FieldName == "recomendacion")
            {
                int calificacion = Convert.ToInt32(e.GetValue("recomendacion"));
                string stars = GetStarIcons(calificacion);
                e.Cell.Text = stars;
            }

        }
        protected string GetStarIcons(int calificacion)
        {
            StringBuilder stars = new StringBuilder();
            for (int i = 0; i < calificacion; i++)
            {
                stars.Append("<i class='bi bi-star-fill' style='color: gold;'></i> ");
            }
            return stars.ToString();
        }

        protected void ddlPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {

            seleccionDePerfil();
        }
        public void seleccionDePerfil()
        {
            btnCalificar.Enabled = true;

            if (ddlPerfil.SelectedIndex >0)
            {

                calif_identificacion.Text = ddlPerfil.SelectedItem.Value;
                calif_nombre.Text = ddlPerfil.SelectedItem.Text;

                Session["identificacion"] = ddlPerfil.SelectedItem.Value;
                Session["nombre"] = ddlPerfil.SelectedItem.Text;


                UpdatePanelModal.Update();
            }
            else
            {
            }

        }
        protected void ddlPerfil_DataBound(object sender, EventArgs e)
        {
            seleccionDePerfil();
        }



    }
}