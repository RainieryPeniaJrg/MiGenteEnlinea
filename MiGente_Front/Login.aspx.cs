using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front
{
    public partial class Login : System.Web.UI.Page
    {
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si existen parámetros en la URL
                if (Request.QueryString.HasKeys())
                {
                    txtEmail.Value = Request.QueryString["email"].ToString();

                    Acceder(Request.QueryString["email"].ToString(), Request.QueryString["pass"].ToString());
                }
                else
                {
                    // La página no tiene parámetros en la URL
                    // Realiza las acciones correspondientes aquí
                }

            }
            // Eliminar cookies de sesión
            Session.Clear();
            Session.Abandon();

            // Eliminar cookies de autenticación si las tienes
            FormsAuthentication.SignOut();

            // Redireccionar a la página de inicio de sesión
        }
        public static HttpCookie paginasCookie = new HttpCookie("paginas");
       
        public void Acceder(string username,string password)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            LoginService service = new LoginService();
            var result = service.login(username.Trim().ToLower(), password.Trim());
            if (result == 0)
            {
                string script = @"<script>
                        Swal.fire({
                            title: 'Incorrecto',
                            text: 'Usuario o contraseña incorrectos',
                            icon: 'error',
                            confirmButtonText: 'Aceptar'
                        });
                    </script>";
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
            }
            else if (result == -1)
            {
                string script = @"<script>
                        Swal.fire({
                            title: 'No Activado',
                            text: 'Este perfil aun no se encuentra activo. Favor revise su email',
                            icon: 'error',
                            confirmButtonText: 'Aceptar'
                        });
                    </script>";
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
            }
            else if (result == 2)
            {
             

                Session["nombre"] = LoginService.myCookie["nombre"];
                Response.Cookies.Add(paginasCookie);
                Response.Cookies.Add(LoginService.myCookie);
                if (LoginService.myCookie["tipo"].ToString() == "1")
                {

                    Response.Redirect("~/comunidad.aspx");

                }
                else if (LoginService.myCookie["tipo"].ToString() == "2")
                {
                    Response.Redirect("~/Contratista/index_contratista.aspx");

                }


            }
            else if (result == 3)
            {
                string script = @"<script>
                        Swal.fire({
                            title: 'No existe',
                            text: 'Los datos suministrados no estan asociados a ninguna cuenta existente',
                            icon: 'error',
                            confirmButtonText: 'Aceptar'
                        });
                    </script>";
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
            }
        }
        protected void btnSendSolicitud_Click(object sender, EventArgs e)
        {
            //obtenerUserID
            LoginService service = new LoginService();

            var result = service.obtenerPerfilByEmail(txtForgotPass.Text.Trim().ToLower());
            if (result != null)
            {
                string host = HttpContext.Current.Request.Url.Authority;

                string url = host + "/landing/activarperfil.aspx?userID=" + result.userID + "&email=" + txtEmail.Value.Trim().ToLower() + "&resetPass=true";
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmailReset(result.Nombre, result.Email, "Recuperar Contraseña", url);

                string script = @"<script>
                        Swal.fire({
                            title: 'Restablecer Password',
                            text: 'Te hemos enviado un correo electronico para reestablecer tu acceso',
                            icon: 'info',
                            confirmButtonText: 'Aceptar'
                        });
                    </script>";
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
            }
            else
            {
                string script = @"<script>
                        Swal.fire({
                            title: 'No existe',
                            text: 'Los datos suministrados no estan asociados a ninguna cuenta existente',
                            icon: 'error',
                            confirmButtonText: 'Aceptar'
                        });
                    </script>";
                ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
            }

        }

  

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registrar.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Acceder(txtEmail.Value, txtPassword.Value);

        }
    }
}