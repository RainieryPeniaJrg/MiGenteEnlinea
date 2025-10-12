using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.Contratista
{
    public partial class MisCalificaciones : System.Web.UI.Page
    {
        SuscripcionesService ss = new SuscripcionesService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie myCookie = Request.Cookies["login"];

                
                HiddenField1.Value = ss.obtenerCedula(myCookie["migente_userID"].ToString());
            }
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

    }
}