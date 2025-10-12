using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static MiGente_Front.FormularioEmpleado;

namespace MiGente_Front.Empleador
{
    public partial class nomina : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicializar tablas para Percepciones y Deducciones
                ViewState["Percepciones"] = new DataTable();
                ViewState["Deducciones"] = new DataTable();
            }
        }
        protected void btnAgregarPercepcion_Click(object sender, EventArgs e)
        {
            // Lógica para agregar percepciones
        }

        protected void btnAgregarDeduccion_Click(object sender, EventArgs e)
        {
            // Lógica para agregar deducciones
        }

        protected void gvPercepciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Lógica para eliminar percepciones
        }

        protected void gvDeducciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Lógica para eliminar deducciones
        }

    }
}