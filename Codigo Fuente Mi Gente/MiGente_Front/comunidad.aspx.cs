using MiGente_Front.Data;
using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.Empleador
{
    public partial class comunidad : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {

            HttpCookie myCookie = Request.Cookies["login"];

            // Lógica para determinar qué MasterPage usar
            if (Convert.ToInt32(myCookie["tipo"]) == 1)
            {
                this.MasterPageFile = "~/Comunity1.master";
            }
            else
            {
                this.MasterPageFile = "~/Contratista/Comunity2.master";
            }
        }
        private static bool consultando;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                consulta();

            }

        }
        void consulta()
        {
            if (!consultando)
            {

                if (!string.IsNullOrEmpty(cbReferencia.Text)
                    || !string.IsNullOrEmpty(cbUbicacion.Text))
                {
                    string criterio = cbReferencia.Text;
                    string zona = cbUbicacion.Text;
                    if (zona == "Cualquier Ubicacion" && string.IsNullOrEmpty(criterio))
                    {
                        obtenerContratistas();

                    }
                    obtenerContratistas(criterio, zona);
                }
                else
                {
                    obtenerContratistas();

                }
                consultando = false;
            }
            else
            {
                consultando = false;

            }
        }
     
        private void obtenerContratistas(string criterio = null, string ubicacion = null, int calificacion = 0)
        {
            if (string.IsNullOrEmpty(criterio) && string.IsNullOrEmpty(ubicacion))
            {
                contratistas = cs.getTodasUltimos20();
                foreach (var item in contratistas)
                {
                    if (item.tipo == 1)
                    {
                        item.Nombre = item.Nombre + " " + item.Apellido;
                    }
                    if (string.IsNullOrEmpty(item.imagenURL))
                    {
                        item.imagenURL = "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png";
                    }
                }
                tituloBuscador.InnerText = "Ultimas Publicaciones";
            }
            else if (!string.IsNullOrEmpty(criterio) && !string.IsNullOrEmpty(ubicacion))
            {
                contratistas = cs.getConCriterio(criterio, ubicacion);
                foreach (var item in contratistas)
                {
                    if (item.tipo == 1)
                    {
                        item.Nombre = item.Nombre + " " + item.Apellido;
                    }
                    if (string.IsNullOrEmpty(item.imagenURL))
                    {
                        item.imagenURL = "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png";
                    }
                }
                tituloBuscador.InnerText = "Resultados de Busqueda";
            }
            else if (string.IsNullOrEmpty(criterio) && !string.IsNullOrEmpty(ubicacion))
            {
                contratistas = cs.getConCriterio(criterio, ubicacion);
                foreach (var item in contratistas)
                {
                    if (item.tipo == 1)
                    {
                        item.Nombre = item.Nombre + " " + item.Apellido;
                    }
                    if (string.IsNullOrEmpty(item.imagenURL))
                    {
                        item.imagenURL = "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png";
                    }
                }
                tituloBuscador.InnerText = "Resultados de Busqueda";
            }

            repeaterTarjetas.DataSource = contratistas;
            repeaterTarjetas.DataBind();
        }
        private static List<VContratistas> contratistas = new List<VContratistas>();
        private ContratistasService cs = new ContratistasService();
        protected string GetStarRating(decimal calificacion)
        {
            if (string.IsNullOrEmpty(calificacion.ToString()))
            {
                calificacion = 0;
            }
            StringBuilder starsHtml = new StringBuilder();

            // Calcula la cantidad de estrellas llenas y medias
            decimal estrellasLlenas = (decimal)Math.Floor(calificacion);
            decimal estrellasMedias = (decimal)Math.Ceiling(calificacion - estrellasLlenas);

            for (decimal i = 1; i <= 5; i++)
            {
                if (i <= estrellasLlenas)
                {
                    // Estrella llena.
                    starsHtml.Append("<span class='bi bi-star-fill ' style='color:darkorange'></span>");
                }
                else if (i <= estrellasLlenas + estrellasMedias)
                {
                    // Estrella media.
                    starsHtml.Append("<span class='bi bi-star-half' style='color:darkorange;margin-right:5px;'></span>");
                }
                else
                {
                    // Estrella vacía.
                    starsHtml.Append("<span class='bi bi-star' style='color:darkgray'></span>");
                }
            }

            return starsHtml.ToString();
        }

        protected void btnPerfil_Click(object sender, EventArgs e)
        {
            consultando = true;
            System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
            string perfilID = btn.CommandArgument;
            var result = cs.getMiPerfil(perfilID);
            if (result != null)
            {

                if (result.tipo == 1)
                {
                    NombrePerfil.InnerText = result.Nombre + " " + result.Apellido;
                }
                else
                {
                    NombrePerfil.InnerText = result.Nombre;
                }
                presentacion.InnerText = result.presentacion;
                titulo.InnerText = result.titulo;
                email.InnerText = result.email;
                experiencia.InnerText = result.experiencia.ToString();
                telefono1.InnerText = result.telefono1.ToString();
                telefono2.InnerText = result.telefono2.ToString();
                if (result.whatsapp1 == true)
                {
                    whatsapp1.Visible = true;
                    enlaceWhatsapp1.HRef = $"https://api.whatsapp.com/send?phone=" + result.telefono1 + "&text" +
                 "=Hola " + NombrePerfil.InnerText + ".%20Encontre%20tu%20contacto%20en%20la%20comunidad%20de%20Mi%20Gente%20En%20Linea," +
                 "%20me%20gustaria%20conversar%20sobre%20tus%20servicios%20profesionales";
                }
                else
                {
                    whatsapp1.Visible = false;
                }
                if (result.whatsapp2 == true)
                {
                    whatsapp2.Visible = true;
                    enlaceWhatsapp2.HRef = $"https://api.whatsapp.com/send?phone=" + result.telefono2 + "&text" +
                 "=Hola " + NombrePerfil.InnerText + ".%20Encontre%20tu%20contacto%20en%20la%20comunidad%20de%20Mi%20Gente%20En%20Linea," +
                 "%20me%20gustaria%20conversar%20sobre%20tus%20servicios%20profesionales";
                }
                else
                {
                    whatsapp2.Visible = false;
                }

                enlaceTelefono1.HRef = "tel:+" + result.telefono1;
                enlaceTelefono2.HRef = "tel:+" + result.telefono2;
                hiddenCalificacion.Value = result.calificacion.ToString();

                llenarServicios(result.contratistaID);

                imgProfile2.Src = result.imagenURL ?? "https://via.placeholder.com/150";

                string script = @"<script type='text/javascript'>
                              $(document).ready(function () {
                                $('#modalPerfil').modal('show');
                              });
                            </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowModalScript", script);
            }
        }

        void llenarServicios(int contratistaID)
        {
            repeaterServicios.DataSource = cs.getServicios(contratistaID);
            repeaterServicios.DataBind();
        }

        protected void BootstrapComboBox1_Click(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            consulta();
        }
    }
}