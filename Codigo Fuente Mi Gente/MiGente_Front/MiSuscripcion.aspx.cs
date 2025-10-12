using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front
{
    public partial class MiSuscripcion : System.Web.UI.Page
    {
        SuscripcionesService ss = new SuscripcionesService();
        protected void Page_PreInit(object sender, EventArgs e)
        {

            HttpCookie myCookie = Request.Cookies["login"];

            // Lógica para determinar qué MasterPage usar
            if (Convert.ToInt32(myCookie["tipo"]) == 1)
            {
                this.MasterPageFile = "~/Platform.master";
            }
            else
            {
                this.MasterPageFile = "~/ContratistaM.master";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var result = ss.obtenerDetalleVentasBySuscripcion(Request.Cookies["login"]["migente_userID"]);
                if (result != null)
                {
                    gridPagos.DataSource = result;
                    gridPagos.DataBind();
                }
                getSuscripcion();
            }
        }
        public void getSuscripcion()
        {
            HttpCookie myCookie = Request.Cookies["login"];
            var result = ss.obtenerSuscripcion(myCookie["migente_userID"].ToString());
            if (result != null)
            {
                txtPlanActual.Text = result.nombre;
                txtProximoPago.Text = Convert.ToDateTime(result.ProximoPago).Date.ToString("MMM-dd-yyyy");
                txtFechaInicio.Text = Convert.ToDateTime(result.fechaInicio).Date.ToString("MMM-dd-yyyy").ToString();
            }
        }
    }
}