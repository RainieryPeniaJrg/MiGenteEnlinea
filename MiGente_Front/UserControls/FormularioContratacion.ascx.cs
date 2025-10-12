using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.Native.CodeCompletion;
using MiGente_Front.Data;
using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.UserControls
{
    public partial class FormularioContratacion : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cbTipo.SelectedIndex=0;
                divPersona.Visible = true;
                dtIngreso.Date=DateTime.Now;
                dtConclusion.Date = DateTime.Now;

            }
        }
        public void SetModalContent(string content, bool edicion = false)
        {
            modalTitle.InnerText = content;

            Session["edicion"] = edicion;

            if (edicion)
            {
                //obtenerFicha();
            }
        }

        protected void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)cbTipo.Value==1)
            {
                divPersona.Visible = true;
                divEmpresa.Visible = false;
            }
            else
            {
                divPersona.Visible = false;
                divEmpresa.Visible = true;
            }
        }
        private bool IsValidForm()
        {
            var isValid = true;
            int count = 0;
           
            if (divPersona.Visible==true)
            {
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
                isValid = !string.IsNullOrEmpty(txtIdentificationNo.Text);
                txtIdentificationNo.IsValid = isValid;
                if (!isValid)
                {
                    count++;
                }
            }
            if (divEmpresa.Visible==true)
            {
                isValid = !string.IsNullOrEmpty(txtRNC.Text);
                txtRNC.IsValid = isValid;
                if (!isValid)
                {
                    count++;
                }

                isValid = !string.IsNullOrEmpty(txtNombreComercial.Text);
                txtApellido.IsValid = isValid;
                if (!isValid)
                {
                    count++;
                }
                isValid = !string.IsNullOrEmpty(txtRNC.Text);
                txtRNC.IsValid = isValid;
                if (!isValid)
                {
                    count++;
                }
                isValid = !string.IsNullOrEmpty(txtCedulaRepresentante.Text);
                txtCedulaRepresentante.IsValid = isValid;
                if (!isValid)
                {
                    count++;
                }
                isValid = !string.IsNullOrEmpty(txtNombreRepresentante.Text);
                txtNombreRepresentante.IsValid = isValid;
                if (!isValid)
                {
                    count++;
                }

            }

            isValid = !string.IsNullOrEmpty(txtDescripcionCorta.Text);
            txtDescripcionCorta.IsValid = isValid;
            if (!isValid)
            {
                count++;
            }
            isValid = !string.IsNullOrEmpty(txtMonto.Text);
            txtMonto.IsValid = isValid;
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

           
            if (count > 0)
            {
                return false;

            }
            return true;

        }
        void clearGeneralInfo()
        {

            txtNombre.Text = "";
            txtApellido.Text = "";
            Image1.ImageUrl = "../assets/img/sin-perfil.jpg";
        }
        EmpleadosService es = new EmpleadosService();
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
                }
                btnValidateID.Enabled = true;
            }

        }
        public int verFijos()
        {
            HttpCookie myCookie = Request.Cookies["login"];

            EmpleadosService es = new EmpleadosService();
            var result = es.getEmpleados(myCookie["migente_userID"].ToString()).Where(x => x.Activo == true).Count();
            return result;
        }
        private void crearNuevo()
        {
            EmpleadosService es = new EmpleadosService();

            //Datos de persona o empresa
            HttpCookie myCookie = Request.Cookies["login"];

            EmpleadosTemporales temp = new EmpleadosTemporales();
            temp.userID = myCookie["migente_userID"].ToString();
            temp.fechaRegistro = DateTime.Now;
            temp.tipo = Convert.ToInt32(cbTipo.SelectedItem.Value);
            if (Convert.ToInt32(cbTipo.SelectedItem.Value) == 1)
            {
                temp.identificacion = txtIdentificationNo.Text;
                temp.nombre = txtNombre.Text;
                temp.apellido = txtApellido.Text;
                temp.alias = txtAlias.Text;
            }
            else if (Convert.ToInt32(cbTipo.SelectedItem.Value) == 2)
            {
                temp.rnc = txtRNC.Text;
                temp.nombreComercial = txtNombreComercial.Text;
                temp.nombreRepresentante = txtNombreRepresentante.Text;
                temp.cedulaRepresentante = txtCedulaRepresentante.Text;
            }
            temp.direccion = txtDireccion.Text;
            temp.provincia = txtProvincia.Text;
            temp.municipio = txtMunicipio.Text;
            temp.telefono1 = txtTelefono1.Text;
            temp.telefono2 = txtTelefono2.Text;

            if (Image1.ImageUrl != null)
            {

                temp.foto = Image1.ImageUrl;
            }


            //Guardar informacion de trabajo

            DetalleContrataciones dc = new DetalleContrataciones();
            dc.descripcionCorta = txtDescripcionCorta.Text;
            dc.descripcionAmpliada = txtDescripcionAmplia.Text;
            dc.fechaInicio = dtIngreso.Date;
            dc.fechaFinal = dtConclusion.Date;
            dc.montoAcordado = Convert.ToDecimal(txtMonto.Text);
            dc.esquemaPagos = cbEsquema.SelectedItem.Text;
            dc.estatus = 1;
            dc.calificado = false;
            dc.calificacionID = 0;
            var result = es.nuevoTemporal(temp, dc);

            if (result)
            {
                string script = @"
                    Swal.fire({
                        title: 'Éxito',
                        text: 'Registro contratacion completada',
                        icon: 'success',
                        confirmButtonText: 'Aceptar'
                    }).then(function() {
                        window.location.href = 'colaboradores.aspx';
                    });
              ";

                ScriptManager.RegisterStartupScript(
                  UpdatePanel1,
                  UpdatePanel1.GetType(),
                  "SweetAlertError",
                  script,
                  true);

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
                crearNuevo();

            }

        }
    }
}