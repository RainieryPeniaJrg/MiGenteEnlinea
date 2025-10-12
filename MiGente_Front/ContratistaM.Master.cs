using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front
{
    public partial class ContratistaM : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                var IsAuth = HttpContext.Current.User.Identity.IsAuthenticated;
                if (!IsAuth)
                {
                    Response.Redirect("~/Login.aspx");
                }
                HttpCookie myCookie = Request.Cookies["login"];

                if (string.IsNullOrEmpty(myCookie["migente_userID"].ToString()))
                {
                    Response.Redirect("~/Login.aspx");
                }

                if (myCookie["tipo"] == "1")
                {
                    Response.Redirect("~/Login.aspx");
                }


                lbAcceso.InnerText = myCookie["nombre"];

                var venc = myCookie["vencimientoPlan"];
                if (myCookie["planID"] == "0")
                {
                    string currentPageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

                    if (currentPageName == "AdquirirPlanContratista.aspx")
                    {
                        return;
                    }

                    Response.Redirect("~/Contratista/AdquirirPlanContratista.aspx?planID=4");


                }
                else
                {
                    if (!string.IsNullOrEmpty(myCookie["vencimientoPlan"]))
                    {
                        string currentPageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

                        if (currentPageName == "AdquirirPlanContratista.aspx?planID=4")
                        {
                            return;
                        }

                        if (Convert.ToDateTime(myCookie["vencimientoPlan"]).Date < DateTime.Now.Date)
                        {

                            Response.Redirect("~/Contratista/AdquirirPlanContratista.aspx?planID=4");



                        }
                    }

                }
            }
        }

        protected void closeSession_ServerClick(object sender, EventArgs e)
        {
            // Eliminar cookies de sesión
            Session.Clear();
            Session.Abandon();

            // Eliminar cookies de autenticación si las tienes
            FormsAuthentication.SignOut();

            // Redireccionar a la página de inicio de sesión
            Response.Redirect("../Login.aspx");
        }

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            // Eliminar cookies de sesión
            Session.Clear();
            Session.Abandon();

            // Eliminar cookies de autenticación si las tienes
            FormsAuthentication.SignOut();

            // Redireccionar a la página de inicio de sesión
            Response.Redirect("~/Login.aspx");
        }

    }
}