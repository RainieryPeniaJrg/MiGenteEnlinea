using DevExpress.XtraPrinting;
using MiGente_Front.Data;
using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.Contratista
{
    public partial class index_contratista : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie myCookie = Request.Cookies["login"];
    
                HiddenField1.Value = myCookie["migente_userID"].ToString();

                if (ddlTipoPerfil.SelectedItem.Value == "1")
                {
                    divTipoPersona.Visible = true;
                    divTipoEmpresa.Visible = false;
                }
                else if (ddlTipoPerfil.SelectedItem.Value == "2")
                {
                    divTipoPersona.Visible = false;
                    divTipoEmpresa.Visible = true;
                }
                linqSectores.DataBind();
                ddlSector.DataBind();
                getPerfil();

            }
        }
        ContratistasService sr = new ContratistasService();

        private static int contratistaID;
        public void getPerfil()
        {
            var result = sr.getMiPerfil(HiddenField1.Value.ToString());
            if (result != null)
            {
                txtTitulo.Text = result.titulo;
                if (result.sector != null)
                {
                    ddlSector.SelectedValue = result.sector;

                }
                else
                {
                    ddlSector.SelectedIndex = -1;
                }
                presentacion.Text = result.presentacion;
                txtEmail.Text = result.email;
                ddlTipoPerfil.SelectedItem.Value = result.tipo.ToString();
                txtIdentificacion.Text = result.identificacion;
                if (result.tipo == 1)
                {
                    txtNombre.Text = result.Nombre;
                    txtApellido.Text = result.Apellido;
                }
                else if (result.tipo == 2)
                {
                    txtRazonSocial.Text = result.Nombre;
                }
                telefono1.Text = result.telefono1;
                telefono2.Text = result.telefono2;
                chkWhatsapp1.Checked = (result.whatsapp1 ?? false);
                chkWhatsapp2.Checked = (result.whatsapp2 ?? false);
                if (string.IsNullOrEmpty(result.provincia))
                {
                    comboProvincias.SelectedIndex = -1;
                }
                else
                {
                    comboProvincias.Text = result.provincia;

                }

                if ((bool)result.activo)
                {
                    btnEstatus.Visible = true;
                    btnEstatus.Text = "Desactivar Perfil";
                    btnEstatus.CssClasses.Control = "bg-danger";
                }
                else
                {

                    btnEstatus.Text = "Activar Perfil";
                    btnEstatus.CssClasses.Control = "bg-success";
                }
                profileImage.Src = result.imagenURL ?? "https://via.placeholder.com/150";
                contratistaID = result.contratistaID;
                //obtener servicios
                obtenerServicios(result.contratistaID);


            }
        }
        private void obtenerServicios(int contratistaID)
        {
            var servicios = sr.getServicios(contratistaID);
            if (servicios != null)

            {
                gridServicios.DataSource = servicios;
                gridServicios.DataBind();

            }
        }
        protected void ddlSector_DataBound(object sender, EventArgs e)
        {
            ddlSector.SelectedIndex = -1;
        }

        protected void ddlTipoPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoPerfil.SelectedItem.Value == "1")
            {
                divTipoPersona.Visible = true;
                divTipoEmpresa.Visible = false;
            }
            else if (ddlTipoPerfil.SelectedItem.Value == "2")
            {
                divTipoPersona.Visible = false;
                divTipoEmpresa.Visible = true;
            }
        }

        protected void btnEstatus_Click(object sender, EventArgs e)
        {
            if (btnEstatus.Text == "Activar Perfil")
            {
                sr.DesactivarPerfil(HiddenField1.Value.ToString());
                btnEstatus.Text = "Activar Perfil";
                btnEstatus.CssClasses.Control = "bg-success";

                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "MostrarAlerta3();", true);

            }
            else
            {
                sr.ActivarPerfil(HiddenField1.Value.ToString());
                btnEstatus.Text = "Desactivar Perfil";
                btnEstatus.CssClasses.Control = "bg-danger";

                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "MostrarAlerta2();", true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private void guardarImagen()
        {
            fileUpload.DataBind();

            if (!fileUpload.HasFile)
            {
                return;
            }
            string imgUrl = fileUpload.FileName;
            string nuevoNombre = HiddenField1.Value + Path.GetExtension(imgUrl);
            string fullPath = Path.Combine(Server.MapPath("/Images/Contratista/"), nuevoNombre);

            fileUpload.SaveAs(fullPath);
            profileImage.Src = "../../Images/Contratista/" + nuevoNombre;

            //guardar en base de datos()

        }
        private void guardar()
        {
            Contratistas ct = new Contratistas();
            ct.titulo = txtTitulo.Text;
            ct.sector = ddlSector.SelectedItem.Text;
            ct.presentacion = presentacion.Text;
            ct.email = txtEmail.Text;
            ct.tipo = Convert.ToInt32(ddlTipoPerfil.SelectedItem.Value);
            ct.identificacion = txtIdentificacion.Text;
            if (ddlTipoPerfil.SelectedValue == "1")
            {
                ct.Nombre = txtNombre.Text;
                ct.Apellido = txtApellido.Text;
            }
            else
            {
                ct.Nombre = txtRazonSocial.Text;
            }
            ct.telefono1 = telefono1.Text;
            ct.telefono2 = telefono2.Text;

            ct.whatsapp1 = chkWhatsapp1.Checked;
            ct.whatsapp2 = chkWhatsapp2.Checked;
            ct.experiencia = string.IsNullOrEmpty(txtExperiencia.Text) ? 0 : Convert.ToInt32(txtExperiencia.Text);
            ct.provincia = comboProvincias.Text;
            guardarImagen();
            ct.imagenURL = profileImage.Src;
            sr.GuardarPerfil(ct, HiddenField1.Value.ToString());

            sr.ActivarPerfil(HiddenField1.Value.ToString());
            btnEstatus.Text = "Desactivar Perfil";
            btnEstatus.CssClasses.Control = "bg-danger";


            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "MostrarAlerta();", true);



        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Contratistas_Servicios ser = new Contratistas_Servicios();
            ser.contratistaID = contratistaID;
            ser.detalleServicio = txtServicio.Text;
            sr.agregarServicio(ser);
            obtenerServicios(contratistaID);
        }

        protected void gridServicios_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "btnRemover")
            {
                object value = gridServicios.GetRowValues(e.VisibleIndex, "servicioID");

                if (value != null)
                {
                    // Realizar alguna acción con el valor obtenido

                    sr.removerServicio(Convert.ToInt32(value), contratistaID);
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("index_contratista.aspx");


                }

            }
        }
    }
}