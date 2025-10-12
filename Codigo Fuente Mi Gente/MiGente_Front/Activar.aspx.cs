using ClassLibrary_CSharp.Encryption;
using MiGente_Front.Data;
using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front
{
    public partial class Activar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["migente_userID"] = Request.QueryString["userID"];
                Session["email"]= Request.QueryString["email"];
                Session["reset"]= Convert.ToBoolean(Request.QueryString["resetPass"]);

                txtEmail.Value = Session["email"].ToString();
    
            }
        }

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["email"].ToString())
               || string.IsNullOrEmpty(txtPassword.Value.Trim())
              )
            {
                return;
            }
            if (IsValid)
            {
                Crypt crypt = new Crypt();
                Credenciales cr = new Credenciales();
                cr.userID = Session["migente_userID"].ToString();
                cr.password = crypt.Encrypt(txtPassword.Value.Trim());
                cr.email = Session["email"].ToString();
                cr.activo = true;

                SuscripcionesService ser = new SuscripcionesService();
                bool result = false;
                if (Convert.ToBoolean(Session["reset"]))
                {
                    result = ser.actualizarPass(cr);

                }
                else
                {
                    result = ser.guardarCredenciales(cr);

                }

                if (result)
                {
                    string redirectUrl = $"Login.aspx?email={Request.QueryString["email"].ToString()}&pass={txtPassword.Value.Trim()}";
                    string script = $@"
            <script type='text/javascript'>
                Swal.fire({{
                    title: 'Usuario activado correctamente',
                    text: 'Redirigiendo en 3 segundos...',
                    icon: 'success',
                    timer: 3000,
                    showConfirmButton: false,
                    timerProgressBar: true,
                    didOpen: () => {{
                        Swal.showLoading()
                    }}
                }}).then(() => {{
                    window.location.href = '{redirectUrl}';
                }});
            </script>";

                    Page.ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
                }
            }

        }
    }
}