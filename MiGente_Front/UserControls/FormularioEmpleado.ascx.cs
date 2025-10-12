using DevExpress.Web;
using MiGente_Front.Data;
using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static MiGente_Front.Empleador.fichaEmpleado;
using static MiGente_Front.FormularioEmpleado;

namespace MiGente_Front
{
    public partial class FormularioEmpleado : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtIngreso.Date = DateTime.Now;
                HttpCookie myCookie = Request.Cookies["login"];
                var uid = myCookie["migente_userID"];
                Image1.ImageUrl = "../assets/img/sin-perfil.jpg";
                divFoto.Visible = true;
                if (Convert.ToBoolean(myCookie["nomina"]))
                {
                    divDeducciones.Visible = true;
                }
                CargarPeriodosPago();
                repeaterRemuneraciones.DataBind();


            }
        
        }
        public void obtenerFicha()
        {
            HttpCookie myCookie = Request.Cookies["login"];
            EmpleadosService es = new EmpleadosService();

            Empleados empleado = es.getEmpleadosByID(Guid.Parse(myCookie["migente_userID"]), Convert.ToInt32(Session["empleadoID"]));
            if (empleado != null)
            {
                dtIngreso.Date = Convert.ToDateTime(empleado.fechaInicio).Date;
                txtIdentificationNo.Text = empleado.identificacion;
                string base64Image = null;

                base64Image = empleado.foto;
                Image1.ImageUrl = base64Image;

                txtNombre.Text = empleado.Nombre;
                txtApellido.Text = empleado.Apellido;
                txtAlias.Text = empleado.alias;
                cbEstadoCivil.Value = empleado.estadoCivil;
                cbEstadoCivil.IsValid = true;
                dtNacimiento.Date = Convert.ToDateTime(empleado.nacimiento).Date;
                txtTelefono1.Text = empleado.telefono1;
                txtTelefono2.Text = empleado.telefono2;
                txtDireccion.Text = empleado.direccion;
                txtProvincia.Text = empleado.provincia;
                txtMunicipio.Text = empleado.municipio;
                txtNombreEmergencia.Text = empleado.contactoEmergencia;
                txtTelefonoEmergencia.Text = empleado.telefonoEmergencia;

                txtPosicion.Text = empleado.posicion;
                txtSalario.Text = Convert.ToDecimal(empleado.salario).ToString("N2");
                txtPeriodoPago.Value = empleado.periodoPago;
                txtPeriodoPago.IsValid = true;
                chkDeducciones.Checked = (bool)empleado.tss;

                //obtener remuneraciones extra
                ObtenerRemuneraciones();

                btnSave.Text = "Actualizar Perfil";

            }
        }
        public void ObtenerRemuneraciones()
        {
            HttpCookie myCookie = Request.Cookies["login"];

            repeaterRemuneraciones.DataSource = es.obtenerRemuneraciones(myCookie["migente_userID"].ToLower(), Convert.ToInt32(Session["empleadoID"]));
            repeaterRemuneraciones.DataBind();
        }
        public void SetModalContent(string content, bool edicion)
        {
            modalTitle.InnerText = content;

            Session["edicion"] = edicion;

            if (edicion)
            {
                obtenerFicha();
            }
        }
        private bool IsValidForm()
        {
            var isValid = true;
            int count = 0;
            isValid = !string.IsNullOrEmpty(txtIdentificationNo.Text);
            txtIdentificationNo.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }
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

            isValid = !string.IsNullOrEmpty(dtNacimiento.Text);
            dtNacimiento.IsValid = isValid;
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
            isValid = !string.IsNullOrEmpty(txtDireccion.Text);
            txtDireccion.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }

            isValid = !string.IsNullOrEmpty(txtProvincia.Text);
            txtProvincia.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }

            isValid = !string.IsNullOrEmpty(txtMunicipio.Text);
            txtMunicipio.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }

            isValid = !string.IsNullOrEmpty(txtPosicion.Text);
            txtPosicion.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }
            isValid = Convert.ToDecimal(txtSalario.Text) > 1;
            txtSalario.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }
            isValid = !string.IsNullOrEmpty(txtNombreEmergencia.Text);
            txtNombreEmergencia.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }
            isValid = !string.IsNullOrEmpty(txtTelefonoEmergencia.Text);
            txtTelefonoEmergencia.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }

            if (count > 0)
            {
                return false;

            }
            return true;

        }
        public void SetModalContent(string title, int? empleadoID = null)
        {

            modalTitle.InnerText = title;

        }
        EmpleadosService es = new EmpleadosService();

        void clearGeneralInfo()
        {

            txtNombre.Text = "";
            txtApellido.Text = "";
            Image1.ImageUrl = "../assets/img/sin-perfil.jpg";
            dtNacimiento.Date = DateTime.Now;
        }
        protected void btnValidateID_Click(object sender, EventArgs e)
        {
            clearGeneralInfo();
            btnValidateID.Enabled = false;
            txtIdentificationNo.Text = txtIdentificationNo.Text.Replace("-", "");


            var result = es.consultarPadron(txtIdentificationNo.Text);
            var padron = result.Result;
            if (padron == null)
            {

                string scriptError = "Swal.fire('Error', 'Numero de Cedula no Encontrado.', 'error');";
                ScriptManager.RegisterStartupScript(
                   UpdatePanel1,
                   UpdatePanel1.GetType(),
                   "SweetAlertError",
                   scriptError,
                   true
               );
                btnValidateID.Enabled = true;

            }
            else
            {

                if (padron != null)
                {

                    string base64String = padron.Photo;
                    base64String = base64String.Replace("data:image/jpg;base64,", "");

                    byte[] imagenBytes = Convert.FromBase64String(base64String);
                    MemoryStream ms = new MemoryStream(imagenBytes);
                    System.Drawing.Image imagen = System.Drawing.Image.FromStream(ms);


                    Image1.ImageUrl = "data:image/jpg;base64," + base64String;
                    txtNombre.Text = padron.Nombre;
                    txtApellido.Text = padron.Apellido1 + " " + padron.Apellido2;
                    dtNacimiento.Date = Convert.ToDateTime(padron.FechaNacimiento);
                }
                btnValidateID.Enabled = true;
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidForm())
            {
                string scriptError = "Swal.fire('Error', 'Todos los campos marcados con * son requeridos.', 'error');";

                ScriptManager.RegisterStartupScript(
                    UpdatePanel1,
                    UpdatePanel1.GetType(),
                    "SweetAlertError",
                    scriptError,
                    true
                );
            }
            else
            {
                guardar();
            }
        }
        public class PeriodoPago
        {
            public string Nombre { get; set; }
            public string Codigo { get; set; }

            public PeriodoPago(string nombre, string codigo)
            {
                Nombre = nombre;
                Codigo = codigo;
            }
        }
        private void CargarPeriodosPago()
        {
            // Lista de objetos PeriodoPago
            var periodos = new List<PeriodoPago>
            {
                new PeriodoPago("Semanal", "1"),
                new PeriodoPago("Quincenal", "2"),
                new PeriodoPago("Mensual", "3")
            };

            // Limpia los ítems antes de agregar nuevos
            txtPeriodoPago.Items.Clear();

            // Agrega los ítems al ComboBox
            foreach (var periodo in periodos)
            {
                txtPeriodoPago.Items.Add(new DevExpress.Web.Bootstrap.BootstrapListEditItem(periodo.Nombre, periodo.Codigo));
            }

            // Selecciona un ítem por defecto (Quincenal)
            txtPeriodoPago.SelectedIndex = 1;
        }
        private void guardar()
        {
            Empleados emp = new Empleados();

            emp.fechaInicio = dtIngreso.Date;
            emp.Activo = true;
            emp.Nombre = txtNombre.Text;
            emp.Apellido = txtApellido.Text;
            emp.alias = txtAlias.Text;
            emp.estadoCivil = (int)cbEstadoCivil.Value;
            emp.identificacion = txtIdentificationNo.Text;
            emp.nacimiento = Convert.ToDateTime(dtNacimiento.Date);
            emp.direccion = txtDireccion.Text;
            emp.provincia = txtProvincia.Text;
            emp.municipio = txtMunicipio.Text;

            emp.telefono1 = txtTelefono1.Text;
            emp.telefono2 = txtTelefono2.Text;
            emp.posicion = txtPosicion.Text;

            emp.salario = txtSalario.Value != null ? Convert.ToDecimal(txtSalario.Text) : 0;
            emp.periodoPago = Convert.ToInt32(txtPeriodoPago.SelectedItem.Value);
            emp.contactoEmergencia = txtNombreEmergencia.Text;
            emp.telefonoEmergencia = txtTelefonoEmergencia.Text;


            emp.tss = chkDeducciones.Checked;


            emp.fechaRegistro = DateTime.Now;


            HttpCookie myCookie = Request.Cookies["login"];
            if (!Convert.ToBoolean(Session["edicion"]))
            {
                emp.userID = myCookie["migente_userID"];

            }
            emp.contrato = false;
            if (Image1.ImageUrl != null)
            {
                emp.foto = Image1.ImageUrl;
            }

            EmpleadosService es = new EmpleadosService();

            if (Convert.ToBoolean(Session["edicion"]))
            {
                emp.empleadoID = Convert.ToInt32(Session["empleadoID"]);
                var result = es.actualizarEmpleado(emp);
                if (result != null)
                {

                    var rem = ActualizarRemuneraciones(result.empleadoID);
                    if (rem)
                    {
                        string script = $@"
                                         Swal.fire({{
                                            title: 'Éxito',
                                            text: 'Perfil de Empleado Actualizado',
                                            icon: 'success',
                                            confirmButtonText: 'Aceptar'
                                        }}).then(function() {{
                                            window.location.href = 'fichaEmpleado.aspx?empleadoID={emp.empleadoID}';
                                        }});
                                    ";

                        ScriptManager.RegisterStartupScript(
                            UpdatePanel1,
                            UpdatePanel1.GetType(),
                            "SweetInfo",
                            script,
                            true
                        );


                    }
                    else
                    {
                        string script = $@"
                    Swal.fire({{
                        title: 'Informacion',
                        text: 'Empleado Actualizado correctamente, pero ocurrio un error almacenando las remuneraciones Extra.',
                        icon: 'warning',
                        confirmButtonText: 'Aceptar'
                    }}).then(function() {{
                        window.location.href = 'fichaEmpleado.aspx?empleadoID={emp.empleadoID}';
                    }});
               ";

                        ScriptManager.RegisterStartupScript(
                        UpdatePanel1,
                        UpdatePanel1.GetType(),
                        "SweetInfo",
                        script,
                        true
                    );

                    }
                }
                else
                {
                    string scriptError = "Swal.fire('Error', 'Ha ocurrido un error en el registro de empleado. Favor contactar a soporte tecnico.', 'error');";

                    ScriptManager.RegisterStartupScript(
                        UpdatePanel1,
                        UpdatePanel1.GetType(),
                        "SweetAlertError",
                        scriptError,
                        true
                    );
                }
            }
            else
            {
                var result = es.guardarEmpleado(emp);

                if (result != null)
                {

                    var rem = guardarRemuneraciones(result.empleadoID);
                    if (rem)
                    {
                        string script = @"
                                        Swal.fire({
                                            title: 'Éxito',
                                            text: 'Registro de nuevo empleado completado',
                                            icon: 'success',
                                            confirmButtonText: 'Aceptar'
                                        }).then(function() {
                                            window.location.href = 'colaboradores.aspx';
                                        });
                                    ";

                        ScriptManager.RegisterStartupScript(
                            UpdatePanel1,
                            UpdatePanel1.GetType(),
                            "SweetInfo",
                            script,
                            true
                        );


                    }
                    else
                    {
                        string script = @"
                    Swal.fire({
                        title: 'Informacion',
                        text: 'Empleado Registrado correctamente, pero ocurrio un error almacenando las remuneraciones Extra.',
                        icon: 'warning',
                        confirmButtonText: 'Aceptar'
                    }).then(function() {
                        window.location.href = 'colaboradores.aspx';
                    });
               ";

                        ScriptManager.RegisterStartupScript(
                        UpdatePanel1,
                        UpdatePanel1.GetType(),
                        "SweetInfo",
                        script,
                        true
                    );

                    }


                }
                else
                {
                    string scriptError = "Swal.fire('Error', 'Ha ocurrido un error en el registro de empleado. Favor contactar a soporte tecnico.', 'error');";

                    ScriptManager.RegisterStartupScript(
                        UpdatePanel1,
                        UpdatePanel1.GetType(),
                        "SweetAlertError",
                        scriptError,
                        true
                    );
                }
            }


        }
        public bool guardarRemuneraciones(int empleadoID)
        {

            HttpCookie myCookie = Request.Cookies["login"];

            // Obtener la lista de la sesión del usuario actual o inicializarla si aún no existe
            List<Remuneraciones> remuneraciones = Session["Remuneraciones"] as List<Remuneraciones>;
            List<Remuneraciones> rem = new List<Remuneraciones>();
            if (remuneraciones != null)
            {

                foreach (var item in remuneraciones)
                {
                    item.empleadoID = empleadoID;
                    rem.Add(item);

                }
                EmpleadosService sr = new EmpleadosService();
                var result = sr.guardarOtrasRemuneraciones(rem);
                Session["Remuneraciones"] = null;
                return result;


            }
            return false;
        }
        public bool ActualizarRemuneraciones(int empleadoID)
        {
            
            HttpCookie myCookie = Request.Cookies["login"];

            // Obtener la lista de la sesión del usuario actual o inicializarla si aún no existe
            List<Remuneraciones> remuneraciones = Session["Remuneraciones"] as List<Remuneraciones>;
            List<Remuneraciones> rem = new List<Remuneraciones>();
            if (remuneraciones != null)
            {

                foreach (var item in remuneraciones)
                {
                    item.empleadoID = empleadoID;
                    rem.Add(item);

                }
                EmpleadosService sr = new EmpleadosService();
                var result = sr.actualizarRemuneraciones(rem, empleadoID);
                Session["Remuneraciones"] = null;
                return result;


            }
            else
            {
                return true;
            }
            return false;
        }
        protected void btnAddOtras_Click(object sender, EventArgs e)
        {
            List<Remuneraciones> remuneraciones = Session["Remuneraciones"] as List<Remuneraciones>;
            if (remuneraciones == null)
            {
                remuneraciones = new List<Remuneraciones>();
                Session["Remuneraciones"] = remuneraciones;
            }
            HttpCookie myCookie = Request.Cookies["login"];



            // Verificar si el servicio ya existe en la lista
            Remuneraciones detail = new Remuneraciones();
            detail.userID = myCookie["migente_userID"];
            detail.descripcion = txtDescOtras.Text;
            detail.monto = Convert.ToDecimal(txtMontoOtras.Text);

            remuneraciones.Add(detail);
            txtDescOtras.Text = "";
            txtMontoOtras.Text = "0";
            repeaterRemuneraciones.DataSource = remuneraciones;
            repeaterRemuneraciones.DataBind();

        }

        protected void btnDeleteOtras_Click(object sender, EventArgs e)
        {
            LinkButton btnDeleteOtras = (LinkButton)sender;
            int id = Convert.ToInt32(btnDeleteOtras.CommandArgument);
            HttpCookie myCookie = Request.Cookies["login"];
            string userID = myCookie["migente_userID"];
            EmpleadosService sr = new EmpleadosService();

            if (id != null)
            {
                if (Convert.ToBoolean(Session["edicion"]))
                {
                    sr.quitarRemuneracion(userID, id);
                    ObtenerRemuneraciones();

                }
            }
        }
    }
}