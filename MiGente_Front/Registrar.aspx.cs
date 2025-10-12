using MiGente_Front.Data;
using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front
{
    public partial class Registrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LimpiarFormulario()
        {

            txtNombre.Value = string.Empty;
            txtApellido.Value = string.Empty;
            txtEmail.Value = string.Empty;

            txtTelefono1.Value = string.Empty;
            txtTelefono2.Value = string.Empty;
        }

        private bool IsValidForm()
        {
            var isValid = true;
            int count = 0;
            isValid = !string.IsNullOrEmpty(txtNombre.Text);
            txtNombre.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }

            isValid = !string.IsNullOrEmpty(txtApellido.Text);
            txtApellido.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }
            isValid = !string.IsNullOrEmpty(cboTipo.Text);
            cboTipo.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }

            isValid = !string.IsNullOrEmpty(txtEmail.Text);
            txtEmail.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }

            isValid = !string.IsNullOrEmpty(txtTelefono1.Text);
            txtTelefono1.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }

            //isValid = !string.IsNullOrEmpty(txtTelefono2.Text);
            //txtTelefono2.IsValid = isValid;
            //if (!isValid)
            //{
            //    count++;
            //}

            if (count > 0)
            {
                return false;

            }
            return true;

        }
        public void guardar()
        {
            if (!IsValidForm())
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "validateForm('toast toastError');", true);
            }
            else if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                SuscripcionesService sr = new SuscripcionesService();

                //validar correo 
                var validarCorreo = sr.validarCorreo(txtEmail.Text);
                if (validarCorreo != null)
                {
                    if ((bool)validarCorreo.isActive)
                    {
                        string script = @"<script>
                    console.log('test');
                        Swal.fire({
                            title: 'Direccion de Correo Existente',
                            text: 'Esta direccion de correo ya fue registrada',
                            icon: 'error',
                            confirmButtonText: 'Aceptar'
                        });
                    </script>";
                        ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
                    }
                    else
                    {
                        // Serializa el objeto Cuentas a JSON
                        string cuentaJson = Newtonsoft.Json.JsonConvert.SerializeObject(validarCorreo);
                        string userID = validarCorreo.userID;
                        string email = validarCorreo.Email;


                        // Crea el script con interpolación de variables
                        string script = $@"
                                <script>
                                   

                                    Swal.fire({{
                                        title: 'Dirección de Correo Existente',
                                        text: 'Esta dirección de correo ya fue registrada',
                                        icon: 'error',
                                        showCancelButton: true,
                                        confirmButtonText: 'Aceptar',
                                        cancelButtonText: 'Reenviar correo de activación'
                                    }}).then((result) => {{
                                            if (result.dismiss === Swal.DismissReason.cancel) {{
                                                // Llama a la función definida en el archivo JS separado
                                                           enviarCorreoActivacion('{userID}', '{email}').then(() => {{
                                                    Swal.fire('Correo reenviado', 'Se ha enviado nuevamente el correo de activación.', 'success');
                                                }}).catch((error) => {{
                                                    Swal.fire('Error', 'Hubo un problema al reenviar el correo.', 'error');
                                                }});
                                            }}
                                        }});
                                    </script>";
                        ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
                    }

                }
                else
                {
                    int tipo = Convert.ToInt32(cboTipo.Value);
                    // Agregar nuevo perfil
                    var perfil = new Cuentas
                    {
                        fechaCreacion = DateTime.Now,
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        Email = txtEmail.Text,
                        userID = Guid.NewGuid().ToString(),
                        Tipo = tipo,
                        telefono1 = txtTelefono1.Text,
                        telefono2 = txtTelefono2.Text,
                        isActive=false
                    };



                    string host = HttpContext.Current.Request.Url.Authority;

                    var result = sr.GuardarPerfil(perfil, host, txtEmail.Text);

                    if (result)
                    {
                        string script = @"<script>
                        Swal.fire({
                            title: 'Éxito',
                            text: 'Su perfil se ha registrado correctamente, verifique su correo electronico para completar el proceso',
                            icon: 'success',
                            confirmButtonText: 'Aceptar'
                        });
                    </script>";

                        ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
                    }

                    LimpiarFormulario();
                }




            }

        }
        [WebMethod]
        public static string EnviarCorreoActivacion(string userID, string email)
        {

            SuscripcionesService sr = new SuscripcionesService();

            string host = HttpContext.Current.Request.Url.Authority;

            sr.enviarCorreoActivacion(host, email,null, userID);
            return "Correo enviado";
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            guardar();

        }
    }
}