using MiGente_Front.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.UserControls
{
    public partial class abogadoBot : System.Web.UI.UserControl
    {
      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Asegura que solo se ejecute cuando se carga la página por primera vez
            {
             
            }
        }
        // Método WebMethod para obtener la respuesta del bot
      
    }
}