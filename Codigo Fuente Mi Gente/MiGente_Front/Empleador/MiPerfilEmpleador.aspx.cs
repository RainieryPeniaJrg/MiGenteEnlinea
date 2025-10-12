using ClassLibrary_CSharp.Encryption;
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
    public partial class MiPerfilEmpleador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie myCookie = Request.Cookies["login"];

                hiddenField1.Value = myCookie["migente_userID"].ToString();
                if (myCookie["planID"] == "2")
                {
                    Session["usuarios"] = 2;
                }
                else if (myCookie["planID"] == "3")
                {
                    Session["usuarios"] = 2;
                }

                if (Convert.ToInt32(Session["usuarios"]) == 0)
                {
                    divBoton.Visible = false;
                    divAgotado.Visible = true;
                }
                obtenerFicha();
            }
        }
        public void obtenerFicha()
        {
            HttpCookie myCookie = Request.Cookies["login"];
            LoginService es = new LoginService();

            var result = es.getPerfilInfo(Guid.Parse(myCookie["migente_userID"]));
            if (result != null)
            {
                txtIdentificacion.Text = result.identificacion;
                ddlTipoIdentificacion.SelectedValue = result.tipoIdentificacion.ToString();
                direccion.Text = result.direccion;
                Session["cuentaID"] = (int)result.cuentaID;

                Nombre.Text = result.Nombre;
                Apellido.Text = result.Apellido;
                nombreComercial.Text = result.nombreComercial;
                email.Text = result.Email;
                telefono1.Text = result.telefono1;
                telefono2.Text = result.telefono2;
                direccion.Text = result.direccion;

                if (!string.IsNullOrEmpty(result.id.ToString()))
                {
                    Session["perfilInfoID"] = (int)result.id;
                }
                if (result.tipoIdentificacion == 2)
                {
                    divEmpresa.Visible = true;

                    nombreComercial.Text = result.nombreComercial;
                    txtNombreGerente.Text = result.nombreGerente;
                    txtApellidoGerente.Text = result.apellidoGerente;
                    txtDireccionGerente.Text = result.direccionGerente;

                }
                else
                {
                    divEmpresa.Visible = false;
                }

                var cantUsuarios = es.obtenerCredenciales(myCookie["migente_userID"].ToString()).Count();
                if (cantUsuarios >= Convert.ToInt32(Session["usuarios"]))
                {
                    divBoton.Visible = false;
                    divAgotado.Visible = true;
                }
                else
                {
                    divBoton.Visible = true;
                    divAgotado.Visible = false;
                }
            }

        }
        protected void ddlTipoIdentificacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoIdentificacion.SelectedItem.Value == "2")
            {
                divEmpresa.Visible = true;
                nombreComercial.Enabled = true;
                txtNombreGerente.Enabled = true;
                txtApellidoGerente.Enabled = true;
                txtDireccionGerente.Enabled = true;
            }
            else
            {
                divEmpresa.Visible = false;
                nombreComercial.Enabled = false;
                txtNombreGerente.Enabled = false;
                txtApellidoGerente.Enabled = false;
                txtDireccionGerente.Enabled = false;

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ActualizarPerfil();
        }
        LoginService ls = new LoginService();

        public void ActualizarPerfil()
        {
            HttpCookie myCookie = Request.Cookies["login"];

            //Perfiles info
            perfilesInfo info = new perfilesInfo();
            info.tipoIdentificacion = Convert.ToInt32(ddlTipoIdentificacion.SelectedItem.Value);
            info.identificacion = txtIdentificacion.Text;
            info.direccion = direccion.Text;
            info.cuentaID = Convert.ToInt32(Session["cuentaID"]);
            info.userID = myCookie["migente_userID"];
            if (ddlTipoIdentificacion.SelectedItem.Value == "2")
            {
                info.nombreComercial = nombreComercial.Text;
                info.nombreGerente = txtNombreGerente.Text;
                info.apellidoGerente = txtApellidoGerente.Text;
                info.direccionGerente = txtDireccionGerente.Text;
            }

            if (Convert.ToInt32(Session["perfilInfoID"]) > 0)
            {
                info.id = (int)Session["perfilInfoID"];
            }

            //Perfil
            Cuentas cuenta = new Cuentas();
            cuenta = ls.getPerfilByID(Convert.ToInt32(Session["cuentaID"]));


            cuenta.Tipo = 1;
            cuenta.Nombre = Nombre.Text;
            cuenta.Apellido = Apellido.Text;


            if (email.Text != cuenta.Email)
            {
                var existeEmail = ls.validarCorreo(email.Text);
                if (existeEmail)
                {
                    string scriptError = "Swal.fire('Error', 'La dirección de correo electrónico ya está registrada.', 'error');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertError", scriptError, true);
                    return;
                }
            }
            cuenta.Email = email.Text;
            cuenta.telefono1 = telefono1.Text;
            cuenta.telefono2 = telefono2.Text;



            if (Convert.ToInt32(Session["cuentaID"]) > 0 && Convert.ToInt32(Session["perfilInfoID"]) > 0)
            {
                ls.actualizarPerfil(info, cuenta);
                string script = "Swal.fire('Perfil Actualizado', 'Los cambios se han guardado correctamente', 'success');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", script, true);
                ActualizarCookiePerfil();
                return;
            }
            if (Convert.ToInt32(Session["perfilInfoID"]) == 0)
            {
                ls.agregarPerfilInfo(info);
            }
            if (Convert.ToInt32(Session["cuentaID"]) > 0)
            {
                ls.actualizarPerfil1(cuenta);
            }
            string script2 = "Swal.fire('Perfil Actualizado', 'Los cambios se han guardado correctamente', 'success');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", script2, true);
            ActualizarCookiePerfil();
            return;
        }

      
        public void ActualizarCookiePerfil()
        {
            HttpCookie myCookie = Request.Cookies["login"];

            if (myCookie != null)
            {

                var vPerfil = ls.obtenerPerfil(myCookie["migente_userID"].ToString());
                string vPerfilSerializado = JsonConvert.SerializeObject(vPerfil);
                myCookie["vPerfil"] = vPerfilSerializado;

                Response.SetCookie(myCookie);

            }

        }

        [WebMethod]
        public static object obtenerUsuarios(string userID)
        {

            // Filtrado por el término de búsqueda
            //obtenerUsuario
            LoginService es = new LoginService();

            var resultUsuarios = es.obtenerCredenciales(userID);
            // Paginación
            var totalRecords = resultUsuarios.Count();
            var credenciales = resultUsuarios
                .OrderByDescending(v => v.id)
                .ToList();

            // Retornar los datos y el número total de registros para la paginación
            return new
            {

                credenciales = credenciales.Select(p => new
                {
                    p.id,
                    p.email,
                    p.password,
                    activo = (bool)p.activo ? "Activo" : "Inactivo"
                }),
                totalRecords = totalRecords
            };
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SuscripcionesService es = new SuscripcionesService();
            HttpCookie myCookie = Request.Cookies["login"];
            Crypt crypt = new Crypt();

            if (!Convert.ToBoolean(Session["editando"]))
            {
                var result = es.validarCorreoCuentaActual(txtEmail.Text, myCookie["migente_userID"].ToString());

                if (result != null)
                {
                    string script2 = "Swal.fire('No se puede agregar el usuario', 'Este Correo ya Existe en esta Suscripcion', 'error');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", script2, true);

                }
                else
                {
                    Credenciales cr = new Credenciales();
                    cr.userID = myCookie["migente_userID"];
                    cr.email = txtEmail.Text;
                    cr.password = crypt.Encrypt(txtPassword.Text);
                    cr.activo = chkActivo.Checked;

                    es.guardarCredenciales(cr);
                    Response.Redirect(Request.RawUrl);
                }
            }
            else
            {
                Credenciales cr = new Credenciales();
                cr.userID = myCookie["migente_userID"];
                cr.email = txtEmail.Text;
                cr.password = crypt.Encrypt(txtPassword.Text);
                cr.activo = chkActivo.Checked;

                es.actualizarCredenciales(cr);
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Session["editando"] = false;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "openModalUsuario();", true);

        }

        protected void resetPasswordBtn_ServerClick(object sender, EventArgs e)
        {
            string host = HttpContext.Current.Request.Url.Authority;
            HttpCookie myCookie = Request.Cookies["login"];

            string url = host + "/landing/activarperfil.aspx?userID=" + myCookie["migente_userID"] + "&email=" + hiddenEmailField.Value.ToLower() + "&resetPass=true";
            EmailSender emailSender = new EmailSender();
            emailSender.SendEmailReset("", hiddenEmailField.Value, "Recuperar Contraseña", url);

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

        protected void btnDeleteUser_ServerClick(object sender, EventArgs e)
        {
            HttpCookie myCookie = Request.Cookies["login"];
            LoginService es = new LoginService();
            es.borrarUsuario(myCookie["migente_userID"], Convert.ToInt32(hiddenUsuarioID.Value));
            Response.Redirect(Request.RawUrl);
        }
    }
}