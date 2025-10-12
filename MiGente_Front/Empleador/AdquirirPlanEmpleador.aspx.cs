using ClassLibrary_CSharp.Encryption;
using MiGente_Front.Data;
using MiGente_Front.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiGente_Front.Empleador
{
    public partial class AdquirirPlanEmpleador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void getPlanes(int planID)
        {
            var result = service.obtenerPlanes().Where(x => x.planID == planID).FirstOrDefault();
            if (result != null)
            {
                lbPlanName.InnerText = result.nombre;
                amount.InnerText = Convert.ToDecimal(result.precio).ToString("N2");
            }


        }
        protected void btnPlan1_ServerClick(object sender, EventArgs e)
        {
            getPlanes(1);
            Session["planID"] = 1;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", "openModal();", true);
        }
        SuscripcionesService service = new SuscripcionesService();
        public static HttpCookie myCookie = new HttpCookie("login");
        private void procesarSuscripcion(bool pago, string idTransaccion, string idempotencyKey, string ip, string card)
        {

            if (pago)
            {
                HttpCookie myCookie = Request.Cookies["login"];


                Ventas ventas = new Ventas();
                ventas.userID = myCookie["migente_userID"].ToString();
                ventas.fecha = DateTime.Now;
                ventas.planID = Convert.ToInt32(Session["planID"]);
                ventas.metodo = 1;
                ventas.precio = Convert.ToDecimal(amount.InnerText);
                ventas.comentario = "Ninguno";
                ventas.idTransaccion = idTransaccion.ToString();
                ventas.idempotencyKey = idempotencyKey.ToString();
                ventas.ip = ip;
                ventas.card = card;

                SuscripcionesService ss = new SuscripcionesService();

                var result = ss.procesarVenta(ventas);
                if (result)
                {

                    Suscripciones suscripciones = new Suscripciones();
                    suscripciones.userID = myCookie["migente_userID"].ToString();
                    suscripciones.planID = Convert.ToInt32(Session["planID"]);
                    suscripciones.vencimiento = DateTime.Now.AddYears(1);


                    //verificar si existen suscripciones anteriores
                    var resultSuscripcion = ss.obtenerSuscripcion(ventas.userID);
                    if (resultSuscripcion != null)
                    {
                        var resultUpdt = ss.actualizarSuscripcion(suscripciones);
                        if (resultUpdt != null)
                        {
                            myCookie["planID"] = Session["planID"].ToString();
                            myCookie["vencimientoPlan"] = DateTime.Now.AddYears(1).ToString();
                            //obtener info de cookies
                            var nombre = myCookie["nombre"];
                            var email = myCookie["email"];
                            Response.SetCookie(myCookie);

                            //Enviar correo
                            EmailSender sender = new EmailSender();
                            sender.SendEmailCompra(nombre, email, "Suscripción completada", lbPlanName.InnerText, Convert.ToDecimal(amount.InnerText), idTransaccion.ToString());


                            string script = @"<script>
                    Swal.fire({
                        title: 'Éxito',
                        text: 'Compra realizada correctamente, en breve recibirá una notificación de correo electrónico con el detalle de la transacción',
                        icon: 'success',
                        confirmButtonText: 'Aceptar'
                    }).then(function() {
                        window.location.href = '~\comunidad.aspx';
                    });
                </script>";

                            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);

                        }
                    }
                    else
                    {

                        //Crear suscripcion

                        var result2 = ss.guardarSuscripcion(suscripciones);
                        if (result2)
                        {
                            myCookie["planID"] = Session["planID"].ToString();
                            myCookie["vencimientoPlan"] = DateTime.Now.AddYears(1).ToString();
                            //obtener info de cookies
                            var nombre = myCookie["nombre"];
                            var email = myCookie["email"];
                            Response.SetCookie(myCookie);

                            //Enviar correo
                            EmailSender sender = new EmailSender();
                            sender.SendEmailCompra(nombre, email, "Suscripción completada", lbPlanName.InnerText, Convert.ToDecimal(amount.InnerText), idTransaccion.ToString());


                            string script = @"<script>
                    Swal.fire({
                        title: 'Éxito',
                        text: 'Compra realizada correctamente, en breve recibirá una notificación de correo electrónico con el detalle de la transacción',
                        icon: 'success',
                        confirmButtonText: 'Aceptar'
                    }).then(function() {
                       window.location.href = '~\comunidad.aspx';
                    });
                </script>";

                            ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
                        }


                    }
                }
            }
        }
        public static bool busy = false;
        protected async void txtSubmit_Click(object sender, EventArgs e)
        {

            if (!chkAutorizacion.Checked) chkAutorizacion.ErrorText = "Debe Aceptar Los Terminos";
            if (!chkTerminosMiGente.Checked) chkTerminosMiGente.ErrorText = "Debe Aceptar Los Terminos";

            if (!chkTerminosMiGente.Checked || !chkAutorizacion.Checked)
            {

                ScriptManager.RegisterStartupScript(this,this.GetType(), "mostrarAlertaTerminos", "mostrarAlertaTerminos()", true);
                return;
            }
            txtSubmit.Enabled = false;
            PaymentService pservice = new PaymentService();
            Crypt crypt = new Crypt();
            string ip = await GetPublicIPAddress();
            busy = true;
            var result = await pservice.Payment(crypt.Encrypt(txtTarjeta.Text), txtCVV.Text, Convert.ToDecimal(amount.InnerText), ip, expiryDate.Text, "MiGentePlan", DateTime.Now.ToString("aaaaMMddhhMMss"));

            if (result.ResponseCode == "00")
            {
                txtSubmit.Enabled = true;
                string script = "mostrarAlertaSuscripcionExitosa();";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarAlertaSuscripcionExitosa", script, true);
                string cardNumberString = txtTarjeta.Text;
                cardNumberString = "****-****-****-" + cardNumberString.Substring(cardNumberString.Length - 4);
                procesarSuscripcion(true, result.PnRef, result.IdempotencyKey, ip, cardNumberString);
            }
            else
            {
                txtSubmit.Enabled = true;
                string script = "mostrarAlertaProblemaPago();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarAlertaProblemaPago", script, true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "openModal();", true);

            }
        }
        public async Task<string> GetPublicIPAddress()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync("https://api.ipify.org");
                    return response;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        protected void btnPlan2_ServerClick(object sender, EventArgs e)
        {
            getPlanes(2);
            Session["planID"] = 2;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", "openModal();", true);
        }

        protected void btnPlan3_ServerClick(object sender, EventArgs e)
        {
            getPlanes(3);
            Session["planID"] = 3;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", "openModal();", true);
        }
    }
}