using MiGente_Front.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MiGente_Front.Services
{
    public class SuscripcionesService
    {
        [WebMethod]
        public bool GuardarPerfil(Cuentas p, string host, string email)
        {

            //try
            //{
            Contratistas c = new Contratistas();

            using (var db = new migenteEntities())
            {
                db.Cuentas.Add(p);

                c.userID = p.userID;
                c.Nombre = p.Nombre;
                c.Apellido = p.Apellido;
                c.email = email;
                c.tipo = 1;
                c.activo = false;
                c.telefono1 = p.telefono1;
                c.fechaIngreso = p.fechaCreacion;
                c.telefono2 = p.telefono2;

                db.SaveChanges();
                             
            };
            enviarCorreoActivacion(host, email,p);

            guardarNuevoContratista(c);
            return true;


        }
        public bool guardarNuevoContratista(Contratistas c)
        {
            using (var db1 = new migenteEntities())
            {
                db1.Contratistas.Add(c);
                try
                {
                    db1.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {

                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }


                }
                return true;
            }
        }
        public void enviarCorreoActivacion(string host, string email, Cuentas p=null,string userID=null)
        {
            //Enviar correo de activacion
            if (p==null)
            {
                migenteEntities db = new migenteEntities();
                p=db.Cuentas.Where(x=>x.userID==userID).FirstOrDefault();
            }
            var perfil = p;
            string url = host + "/Activar.aspx?userID=" + perfil.userID + "&email=" + email;
            EmailSender sender = new EmailSender();
            sender.SendEmailRegistro(perfil.Nombre, perfil.Email, "Bienvenido a Mi Gente", url);

        }

        //public bool guardarNuevoContratista(Contratistas c)
        //{
        //    using (var db1 = new migenteEntities())
        //    {
        //        db1.Contratistas.Add(c);
        //        try
        //        {
        //            // Your code...
        //            // Could also be before try if you know the exception occurs in SaveChanges

        //            db1.SaveChanges();
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            foreach (var eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (var ve in eve.ValidationErrors)
        //                {

        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }


        //        }
        //        return true;
        //    }
        //}
        public bool guardarCredenciales(Credenciales c)
        {


            using (var db = new migenteEntities())
            {
                db.Credenciales.Add(c);
                db.SaveChanges();

                //Enviar correo de activacion

                return true;
            };


        }

        public bool actualizarPass(Credenciales c)
        {


            using (var db = new migenteEntities())
            {
                var result = db.Credenciales.Where(x => x.email == c.email && x.userID == c.userID).FirstOrDefault();
                if (result != null)
                {
                    result.password = c.password;
                }
                db.SaveChanges();

                //Enviar correo de activacion

                return true;
            };


        }
        public bool actualizarCredenciales(Credenciales c)
        {


            using (var db = new migenteEntities())
            {
                var result = db.Credenciales.Where(x => x.email == c.email && x.userID== c.userID).FirstOrDefault();
                if (result != null)
                {
                    result.password = c.password;
                    result.activo=c.activo;
                    result.email=c.email;
                }
                db.SaveChanges();

                //Enviar correo de activacion

                return true;
            };


        }
        public string obtenerCedula(string userID)
        {
            var db = new migenteEntities();
            return db.Contratistas.Where(x => x.userID == userID).Select(x => x.identificacion).FirstOrDefault();
        }
        public bool actualizarPassByID(Credenciales c)
        {


            using (var db = new migenteEntities())
            {
                var result = db.Credenciales.Where(x => x.id == c.id).FirstOrDefault();
                if (result != null)
                {
                    result.password = c.password;
                }
                db.SaveChanges();

                //Enviar correo de activacion

                return true;
            };


        }
        public Cuentas validarCorreo(string correo)
        {


            using (var db = new migenteEntities())
            {
                var result = db.Cuentas.Where(x => x.Email == correo).Include(a=>a.perfilesInfo).FirstOrDefault();

                if (result != null)
                {
                    return result;
                }
            };

            return null;
        }
        public Cuentas validarCorreoCuentaActual(string correo, string userID)
        {


            using (var db = new migenteEntities())
            {
                var result = db.Cuentas.Where(x => x.Email == correo && x.userID==userID).Include(a => a.perfilesInfo).FirstOrDefault();

                if (result != null)
                {
                    return result;
                }
            };

            return null;
        }
        public ObtenerSuscripcion_Result obtenerSuscripcion(string userID)
        {


            using (var db = new migenteEntities())
            {
                var result = db.ObtenerSuscripcion(userID).OrderByDescending(x => x.fechaInicio).FirstOrDefault();

                if (result != null)
                {
                    return result;
                }
            };

            return null;
        }
        public Suscripciones actualizarSuscripcion(Suscripciones suscripcion)
        {
            var db = new migenteEntities();
            var result = db.Suscripciones.Where(x => x.userID == suscripcion.userID).FirstOrDefault();
            if (result != null)
            {
                result.planID = suscripcion.planID;
                result.vencimiento = suscripcion.vencimiento;
                db.SaveChanges();
            }
            return suscripcion;

        }

        public List<Planes_empleadores> obtenerPlanes()
        {



            using (var db = new migenteEntities())
            {
                var result = db.Planes_empleadores.ToList();

                if (result != null)
                {
                    return result;
                }
            };

            return null;
        }
        public List<Planes_Contratistas> obtenerPlanesContratistas()
        {



            using (var db = new migenteEntities())
            {
                var result = db.Planes_Contratistas.ToList();

                if (result != null)
                {
                    return result;
                }
            };

            return null;
        }

        #region Checkout
        //guardar venta
        public bool procesarVenta(Ventas venta)
        {

            using (var db = new migenteEntities())
            {
                db.Ventas.Add(venta);
                db.SaveChanges();
                return true;
            }
        }

        //Guardar Suscripcion
        public bool guardarSuscripcion(Suscripciones suscripciones)
        {
            using (var db = new migenteEntities())
            {
                var result = db.Suscripciones.Add(suscripciones);
                db.SaveChanges();

                return true;
            }
        }
        public List<Ventas> obtenerDetalleVentasBySuscripcion(string userID)
        {
            var db = new migenteEntities();
            var result = db.Ventas.Where(x => x.userID == userID).ToList();
            return result;
        }
        #endregion

    }
}