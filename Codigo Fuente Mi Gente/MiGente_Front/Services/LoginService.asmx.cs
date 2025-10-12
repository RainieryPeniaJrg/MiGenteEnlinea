using ClassLibrary_CSharp.Encryption;
using DevExpress.Web.Internal;
using MiGente_Front.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;

namespace MiGente_Front
{
    /// <summary>
    /// Summary description for LoginService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LoginService : System.Web.Services.WebService
    {

        public static HttpCookie myCookie = new HttpCookie("login");

        public int login(string email, string pass)
        {

            using (var db = new migenteEntities())
            {
                Crypt crypt = new Crypt();
                var crypted = crypt.Encrypt(pass);
                var result = db.Credenciales.Where(x => x.email == email && x.password == crypted).FirstOrDefault();
                if (result != null)
                {
                    if (!(bool)result.activo)
                    {
                        return -1;
                    }


                    //Variables de Sesion
                    FormsAuthentication.SetAuthCookie(result.email, false);
                    myCookie["email"] = result.email;
                    myCookie["migente_userID"] = result.userID;

                    //obtener datos de cuenta
                    var cuenta = db.Cuentas.Where(x => x.userID == result.userID).FirstOrDefault();

                    myCookie["nombre"] = cuenta.Nombre + " " + cuenta.Apellido;
                    myCookie["tipo"] = cuenta.Tipo.ToString();

                    //obtener datos de suscripcion
                    var suscripcion = db.Suscripciones.Where(x => x.userID == result.userID).Include(a => a.Planes_empleadores).OrderByDescending(x => x.suscripcionID)
                        .FirstOrDefault();

                    if (suscripcion == null)
                    {

                        myCookie["planID"] = "0";
                    }

                    else
                    {

                        myCookie["planID"] = suscripcion.planID.ToString();
                        myCookie["vencimientoPlan"] = Convert.ToDateTime(suscripcion.vencimiento).ToString("d");
                        myCookie["nomina"] = suscripcion.Planes_empleadores.nomina.ToString();
                        myCookie["empleados"] = suscripcion.Planes_empleadores.empleados.ToString();
                        myCookie["historico"] = suscripcion.Planes_empleadores.historico.ToString();


                    }


                var vPerfil = obtenerPerfil(result.userID);
                string vPerfilSerializado = JsonConvert.SerializeObject(vPerfil);
                myCookie["vPerfil"] = vPerfilSerializado;


                var now = DateTime.Now;
                myCookie.Expires = now.AddDays(1d);



                return 2;
            }


                else
            {
                return 0;
            }


        };


    }
        public void  borrarUsuario(string userID,int credencialID)
        {
            using (var db = new migenteEntities())
            {
                var result = db.Credenciales.Where(a => a.userID == userID && a.id==credencialID).FirstOrDefault();
                db.Credenciales.Remove(result);
                db.SaveChanges();
            }
        }
        public VPerfiles obtenerPerfil(string userID)
    {
        using (var db = new migenteEntities())
        {
            return db.VPerfiles.Where(a => a.userID == userID).FirstOrDefault();

        }
    }
    public VPerfiles obtenerPerfilByEmail(string email)
    {
        using (var db = new migenteEntities())
        {

            var result = db.VPerfiles.Where(a => a.emailUsuario == email).FirstOrDefault();
            return result;
        }
    }
        public List<Credenciales> obtenerCredenciales(string userID)
        {
            using (var db = new migenteEntities())
            {
                return db.Credenciales.Where(a => a.userID == userID).ToList();

            }
        }
        public bool actualizarPerfil(perfilesInfo info, Cuentas cuenta)
        {
            using (var db = new migenteEntities())
            {
                db.Entry(info).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            using (var db1 = new migenteEntities())
            {
                db1.Entry(cuenta).State = System.Data.Entity.EntityState.Modified;
                db1.SaveChanges();

            }
            return true;
        }
        public bool actualizarPerfil1(Cuentas cuenta)
        {
            using (var db = new migenteEntities())
            {
                db.Entry(cuenta).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            using (var db1 = new migenteEntities())
            {
                db1.Entry(cuenta).State = System.Data.Entity.EntityState.Modified;
                db1.SaveChanges();

            }
            return true;
        }
        public bool agregarPerfilInfo(perfilesInfo info)
        {
            using (var db = new migenteEntities())
            {
                db.perfilesInfo.Add(info);
                db.SaveChanges();

                return true;
            }

        }
        public Cuentas getPerfilByID(int cuentaID)
        {
            using (var db = new migenteEntities())
            {
                return db.Cuentas.Where(x => x.cuentaID == cuentaID).FirstOrDefault();

            }
        }
        public bool validarCorreo(string correo)
    {


        using (var db = new migenteEntities())
        {
            var result = db.Cuentas.Where(x => x.Email == correo).FirstOrDefault();

            if (result != null)
            {
                return true;
            }
        };

        return false;
    }
        public VPerfiles getPerfilInfo(Guid userID)
        {
            using (var db = new migenteEntities())
            {
                return db.VPerfiles.Where(x => x.userID == userID.ToString()).FirstOrDefault();

            }
        }


    }
}