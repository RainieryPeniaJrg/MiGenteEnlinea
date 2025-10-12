using MiGente_Front.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Web.Services;

namespace MiGente_Front.Services
{
    public class EmpleadosService
    {
        public IQueryable<Empleados> getEmpleados(string userID)
        {
            var db = new migenteEntities();
            var query = db.Empleados.Where(x => x.userID == userID).AsQueryable();

            return query;


        }
        public List<VEmpleados> getVEmpleados(string userID)
        {
            var db = new migenteEntities();
            var query = db.VEmpleados.Where(x => x.userID == userID).ToList();

            return query;


        }
   
        public List<EmpleadosTemporales> getContrataciones(Guid userID)
        {
            using (var db = new migenteEntities())
            {
                return db.EmpleadosTemporales
                    .Where(x => x.userID == userID.ToString()).Include(x=>x.DetalleContrataciones).ToList();

            }
        }
        public Empleados getEmpleadosByID(Guid userID, int id)
        {
            using (var db = new migenteEntities())
            {
                return db.Empleados.Where(x => x.userID == userID.ToString()
                && x.empleadoID == id).Include(x=>x.Empleador_Recibos_Header)
                .FirstOrDefault();

            }
        }
        public List<Remuneraciones> obtenerRemuneraciones(string userID, int empleadoID)
        {
            using (var db = new migenteEntities())
            {
                return db.Remuneraciones.Where(x => x.userID == userID && x.empleadoID == empleadoID).ToList();
            }
        }

        public void quitarRemuneracion(string userID, int id)
        {
            using (var db = new migenteEntities())
            {
                var toDelete = db.Remuneraciones.Where(x => x.userID == userID && x.id == id).FirstOrDefault();
                if (toDelete!=null)
                {
                    db.Remuneraciones.Remove(toDelete);
                    db.SaveChanges();
                }
            }
        }
        public Empleados guardarEmpleado(Empleados empleado)
        {
            using (var db = new migenteEntities())
            {
                db.Empleados.Add(empleado);
                db.SaveChanges();
                return empleado;

            }

        }
        public Empleados actualizarEmpleado(Empleados empleado)
        {
            using (var db = new migenteEntities())
            {
                var result = db.Empleados.Where(x=>x.empleadoID==empleado.empleadoID).FirstOrDefault();
                if (result!=null)
                {
                    result.fechaInicio = empleado.fechaInicio;
                    result.identificacion=empleado.identificacion;
                    result.Nombre=empleado.Nombre;
                    result.Apellido=empleado.Apellido;
                    result.alias=empleado.alias;
                    result.estadoCivil=empleado.estadoCivil;
                    result.nacimiento=empleado.nacimiento;
                    result.telefono1 = empleado.telefono1;
                    result.telefono2 = empleado.telefono2;
                    result.direccion=empleado.direccion;
                    result.provincia=empleado.provincia;
                    result.municipio=empleado.municipio;
                    result.posicion=empleado.posicion;
                    result.salario=empleado.salario;
                    result.tss=empleado.tss;
                    result.periodoPago=empleado.periodoPago;
                    result.contactoEmergencia = empleado.contactoEmergencia;
                    result.telefonoEmergencia = empleado.telefonoEmergencia;

                }
                db.SaveChanges();
                return empleado;

            }

        }
        public bool ActualizarEmpleado(Empleados empleado)
        {
            using (var db = new migenteEntities())
            {
                var empleadoExistente = db.Empleados.Find(empleado.empleadoID);

                if (empleadoExistente != null)
                {
                    // Actualiza los valores del empleado existente con los valores del nuevo empleado
                    db.Entry(empleadoExistente).CurrentValues.SetValues(empleado);

                    db.SaveChanges();
                    return true;
                }

                return false; // No se encontró el empleado a actualizar
            }
        }
        //public bool guardarNota(Empleados_Notas nota)
        //{
        //    using (var db = new migenteEntities())
        //    {
        //        db.Empleados_Notas.Add(nota);
        //        db.SaveChanges();
        //        return true;
        //    }
        //}
        public int procesarPago(Empleador_Recibos_Header header, List<Empleador_Recibos_Detalle> detalle)
        {

            using (var db = new migenteEntities())
            {
                db.Empleador_Recibos_Header.Add(header);
                db.SaveChanges();
            }

            using (var db1 = new migenteEntities())
            {

                foreach (var item in detalle)
                {
                    item.pagoID = header.pagoID;
                }

                db1.Empleador_Recibos_Detalle.AddRange(detalle);
                db1.SaveChanges();
            }
            return header.pagoID;
        }
        public int procesarPagoContratacion(Empleador_Recibos_Header_Contrataciones header, List<Empleador_Recibos_Detalle_Contrataciones> detalle)
        {

            using (var db = new migenteEntities())
            {
                db.Empleador_Recibos_Header_Contrataciones.Add(header);
                db.SaveChanges();
            }

            using (var db1 = new migenteEntities())
            {

                foreach (var item in detalle)
                {
                    item.pagoID = header.pagoID;
                }

                db1.Empleador_Recibos_Detalle_Contrataciones.AddRange(detalle);
                db1.SaveChanges();
            }

            //update estatus
            if (detalle.Select(x => x.Concepto).FirstOrDefault() == "Pago Final")
            {
                var db3 = new migenteEntities();
                    var det = db3.DetalleContrataciones.Where(X => X.contratacionID == header.contratacionID && X.detalleID==header.detalleID).FirstOrDefault();
                    if (det != null)
                    {
                        det.estatus = 2;
                        db3.SaveChanges();
                    }
            }

            return header.pagoID;
        }
        public List<VRecibosEmpleados> GetEmpleador_Recibos_Empleado(string userID,int empleadoID)
        {
            using (var db = new migenteEntities())
            {
                var resultado = db.VRecibosEmpleados.Where(x => x.empleadoID == empleadoID && x.userID==userID)
                 .ToList();
                return resultado;
            }
        }
        public Empleador_Recibos_Header GetEmpleador_ReciboByPagoID(int pagoID)
        {
            using (var db = new migenteEntities())
            {
                var resultado = db.Empleador_Recibos_Header.Where(x => x.pagoID == pagoID)
                    .Include(h => h.Empleador_Recibos_Detalle)
                    .Include(f => f.Empleados).FirstOrDefault();
                return resultado;
            }
        }
        public Empleador_Recibos_Header_Contrataciones GetContratacion_ReciboByPagoID(int pagoID)
        {
            using (var db = new migenteEntities())
            {
                var resultado = db.Empleador_Recibos_Header_Contrataciones.Where(x => x.pagoID == pagoID)
                    .Include(h => h.Empleador_Recibos_Detalle_Contrataciones)
                    .Include(f => f.EmpleadosTemporales).FirstOrDefault();
                return resultado;
            }
        }

        public bool cancelarTrabajo(int contratacionID, int detalleID)
        {
            using (var db = new migenteEntities())
            {
                DetalleContrataciones detalle = db.DetalleContrataciones.Where(x => x.contratacionID == contratacionID && x.detalleID == detalleID).FirstOrDefault();
                if (detalle != null)
                {
                    detalle.estatus = 3;
                    db.SaveChanges();

                }
                return true;
            }
        }
        public bool eliminarReciboEmpleado(int pagoID)
        {
            using (var db = new migenteEntities())
            {
                var detallesAEliminar = db.Empleador_Recibos_Detalle
                    .Where(d => d.pagoID == pagoID);

                db.Empleador_Recibos_Detalle.RemoveRange(detallesAEliminar);

                db.SaveChanges();
            }

            using (var db1 = new migenteEntities())
            {
                var headerAEliminar = db1.Empleador_Recibos_Header
                    .FirstOrDefault(h => h.pagoID == pagoID);

                if (headerAEliminar != null)
                {
                    db1.Empleador_Recibos_Header.Remove(headerAEliminar);
                    db1.SaveChanges();
                }
            }
            return true;
        }

        public bool eliminarReciboContratacion(int pagoID)
        {
            using (var db = new migenteEntities())
            {
                var detallesAEliminar = db.Empleador_Recibos_Detalle_Contrataciones
                    .Where(d => d.pagoID == pagoID);

                db.Empleador_Recibos_Detalle_Contrataciones.RemoveRange(detallesAEliminar);

                db.SaveChanges();
            }

            using (var db1 = new migenteEntities())
            {
                var headerAEliminar = db1.Empleador_Recibos_Header_Contrataciones
                    .FirstOrDefault(h => h.pagoID == pagoID);

                if (headerAEliminar != null)
                {
                    db1.Empleador_Recibos_Header_Contrataciones.Remove(headerAEliminar);
                    db1.SaveChanges();
                }
            }
            return true;
        }

        public bool eliminarEmpleadoTemporal(int contratacionID)
        {

            EmpleadosTemporales tmp = new EmpleadosTemporales();
            using (var dbTmp = new migenteEntities())
            {
                tmp = dbTmp.EmpleadosTemporales.Where(a => a.contratacionID == contratacionID).FirstOrDefault();

            }
            if (tmp != null)
            {
                //verificar si tiene recibos 

                foreach (var recibos in tmp.Empleador_Recibos_Header_Contrataciones)
                {
                    //borrar detalle de recibos si lo encuenta

                    using (var db = new migenteEntities())
                    {
                        var detallesAEliminar = db.Empleador_Recibos_Detalle_Contrataciones
                            .Where(d => d.pagoID == recibos.pagoID);

                        db.Empleador_Recibos_Detalle_Contrataciones.RemoveRange(detallesAEliminar);

                        db.SaveChanges();
                    }

                    //borrar recibos
                    using (var db1 = new migenteEntities())
                    {
                        var headerAEliminar = db1.Empleador_Recibos_Header_Contrataciones
                            .FirstOrDefault(h => h.pagoID == recibos.pagoID);

                        if (headerAEliminar != null)
                        {
                            db1.Empleador_Recibos_Header_Contrataciones.Remove(headerAEliminar);
                            db1.SaveChanges();
                        }
                    }
                }

                //borrarEmpleadoTemporal
                using (var dbEmp = new migenteEntities())
                {
                    var registroAEliminar = dbEmp.EmpleadosTemporales
                        .FirstOrDefault(h => h.contratacionID == contratacionID);

                    if (registroAEliminar != null)
                    {
                        dbEmp.EmpleadosTemporales.Remove(registroAEliminar);
                        dbEmp.SaveChanges();
                    }
                }

            }




            return true;
        }
        public List<VPagosContrataciones> GetEmpleador_RecibosContratacionesByID(int contratacionID,int detalleID)
        {
            using (var db = new migenteEntities())
            {
                var resultado = db.VPagosContrataciones.Where(x => x.contratacionID == contratacionID && x.detalleID==detalleID)
                    .ToList();
                return resultado;
            }
        }

        public bool darDeBaja(int empleadoID, string userID, DateTime fechaBaja, decimal prestaciones, string motivo)
        {
            using (var db = new migenteEntities())
            {
                Empleados empleado = db.Empleados.Where(x => x.empleadoID == empleadoID && x.userID == userID).FirstOrDefault();
                if (empleado != null)
                {
                    empleado.Activo = false;
                    empleado.fechaSalida = fechaBaja.Date;
                    empleado.motivoBaja = motivo;
                    empleado.prestaciones = prestaciones;
                    db.SaveChanges();

                }
                return true;
            }
        }
        public bool nuevoTemporal(EmpleadosTemporales temp, DetalleContrataciones det)
        {
            using (var db = new migenteEntities())
            {
                db.EmpleadosTemporales.Add(temp);
                db.SaveChanges();

            }
            using (var db1 = new migenteEntities())
            {
                det.contratacionID = temp.contratacionID;
                db1.DetalleContrataciones.Add(det);
                db1.SaveChanges();
                return true;
            }
        }

        //public bool modificarTemporal(EmpleadosTemporales temp, int contratacionID)
        //{
        //    using (var db = new migenteEntities())
        //    {
        //        var col = db.EmpleadosTemporales.Where(x => x.contratacionID == contratacionID).FirstOrDefault();
        //        if (col != null)
        //        {
        //            col.contratacionID = temp.contratacionID;
        //            col.userID = temp.userID;
        //            col.fechaRegistro = temp.fechaRegistro;
        //            col.tipo = temp.tipo;
        //            col.nombreComercial = temp.nombreComercial;
        //            col.rnc = temp.rnc;
        //            col.identificacion = temp.identificacion;
        //            col.nombre = temp.nombre;
        //            col.apellido = temp.apellido;
        //            col.alias = temp.alias;
        //            col.nombreRepresentante = temp.nombreRepresentante;
        //            col.cedulaRepresentante = temp.cedulaRepresentante;
        //            col.direccion = temp.direccion;
        //            col.provincia = temp.provincia;
        //            col.municipio = temp.municipio;
        //            col.telefono1 = temp.telefono1;
        //            col.telefono2 = temp.telefono2;
        //            col.foto = temp.foto;
        //            db.SaveChanges();

        //        }
        //        return true;
        //    }

        //}

        public bool nuevaContratacionTemporal(DetalleContrataciones det)
        {

            using (var db1 = new migenteEntities())
            {

                db1.DetalleContrataciones.Add(det);
                db1.SaveChanges();
                return true;
            }
        }
        public bool actualizarContratacion(DetalleContrataciones det)
        {

            using (var db = new migenteEntities())
            {

                DetalleContrataciones detalles = db.DetalleContrataciones.Where(x => x.contratacionID == det.contratacionID).FirstOrDefault();
                if (detalles != null)
                {
                    detalles.descripcionCorta = det.descripcionCorta;
                    detalles.descripcionAmpliada = det.descripcionAmpliada;
                    detalles.fechaInicio = det.fechaInicio;
                    detalles.esquemaPagos = det.esquemaPagos;
                    detalles.montoAcordado = det.montoAcordado;
                    detalles.fechaInicio = det.fechaInicio;
                    detalles.fechaFinal = det.fechaFinal;
                    detalles.estatus = det.estatus;
                    db.SaveChanges();

                }
                return true;
            }
        }
        public bool calificarContratacion(int contratacionID, int calificacionID)
        {

            using (var db = new migenteEntities())
            {

                DetalleContrataciones detalles = db.DetalleContrataciones.Where(x => x.contratacionID == contratacionID).FirstOrDefault();
                if (detalles != null)
                {

                    detalles.calificado = true;
                    detalles.calificacionID = calificacionID;

                    db.SaveChanges();

                }
                return true;
            }
        }

        public bool modificarCalificacionDeContratacion(Calificaciones cal)
        {

            using (var db = new migenteEntities())
            {

                var calificacion = db.Calificaciones.Where(x => x.calificacionID == cal.calificacionID).FirstOrDefault();
                if (calificacion != null)
                {
                    calificacion.identificacion = cal.identificacion;
                    calificacion.calificacionID = cal.calificacionID;
                    calificacion.conocimientos = cal.conocimientos;
                    calificacion.cumplimiento = cal.cumplimiento;
                    calificacion.fecha = cal.fecha;
                    calificacion.nombre = cal.nombre;
                    calificacion.puntualidad = cal.puntualidad;
                    calificacion.recomendacion = cal.recomendacion;
                    calificacion.tipo = cal.tipo;
                    calificacion.userID = cal.userID;

                    db.SaveChanges();

                }
                return true;
            }
        }
        public EmpleadosTemporales obtenerFichaTemporales(int contratacionID, string userID)
        {
            using (var db = new migenteEntities())
            {
                var result = db.EmpleadosTemporales.Where(x => x.userID == userID && x.contratacionID == contratacionID)
                    .Include(a => a.DetalleContrataciones).FirstOrDefault();
                return result;
            }
        }
        public List<EmpleadosTemporales> obtenerTodosLosTemporales(string userID)
        {
            List<EmpleadosTemporales> list = new List<EmpleadosTemporales>();

            using (var db = new migenteEntities())
            {
                var result = db.EmpleadosTemporales.Where(x => x.userID == userID)
                    .Include(a => a.DetalleContrataciones).ToList();
                foreach (var item in result)
                {
                    if (item.tipo == 1)
                    {
                        item.nombre = item.nombre + item.apellido;

                    }
                    else if (item.tipo == 2)
                    {
                        item.nombre = item.nombreComercial;
                        item.identificacion = item.rnc;
                    }
                    list.Add(item);
                }


            }
            return list;

        }
        public VContratacionesTemporales obtenerVistaTemporal(int contratacionID, string userID)
        {
            using (var db = new migenteEntities())
            {
                var result = db.VContratacionesTemporales
                    .Where(x => x.userID == userID && x.contratacionID == contratacionID).FirstOrDefault();
                return result;
            }
        }

        //public bool verificarIdentificacion(int tipo, string identificacion, string userID)
        //{
        //    using (var db = new migenteEntities())
        //    {
        //        if (tipo == 1)
        //        {
        //            var result = db.EmpleadosTemporales.Where(x => x.userID == userID && x.identificacion == identificacion).FirstOrDefault();
        //            if (result != null)
        //            {
        //                return true;
        //            }
        //        }
        //        else if (tipo == 2)
        //        {
        //            var result = db.EmpleadosTemporales.Where(x => x.userID == userID && x.rnc == identificacion).FirstOrDefault();
        //            if (result != null)
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    }


        //}

        public async Task<PadronModel> consultarPadron(string cedula)
        {
            HttpClient client = new HttpClient();
            // Ruta para autenticación (login)
            string loginUrl = "https://abcportal.online/Sigeinfo/public/api/login";
            var loginContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "131345042"),
                new KeyValuePair<string, string>("password", "1313450422022@*SRL")
                });

            var loginResponse = await client.PostAsync(loginUrl, loginContent).ConfigureAwait(false);
            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();


            // Asegúrate de manejar la respuesta de autenticación según tus necesidades.

            if (loginResponse.IsSuccessStatusCode)
            {
                // Ruta para búsqueda de individuo (search_individuo)
                string searchUrl = "https://abcportal.online/Sigeinfo/public/api/individuo/" + cedula;

                var jsonResponse = JObject.Parse(loginResponseContent);

                // Buscar el token dentro de la estructura JSON
                var token = jsonResponse["token"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var searchResponse = await client.GetAsync(searchUrl);
                var searchResponseContent = await searchResponse.Content.ReadAsStringAsync();

                if (searchResponse.IsSuccessStatusCode)
                {
                    try
                    {

                        PadronModel padronModel = JsonConvert.DeserializeObject<PadronModel>(searchResponseContent);
                        return padronModel;

                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public bool guardarOtrasRemuneraciones(List<Remuneraciones> rem)
        {
            using (migenteEntities db = new migenteEntities())
            {
                db.Remuneraciones.AddRange(rem);
                db.SaveChanges();
                return true;
            }


        }
        public bool actualizarRemuneraciones(List<Remuneraciones> rem,int empleadoID)
        {
            using (migenteEntities db = new migenteEntities())
            {
                var result = db.Remuneraciones.Where(x => x.empleadoID == empleadoID).FirstOrDefault();
                if (result != null)
                {
                    db.Remuneraciones.Remove(result);
                    db.SaveChanges();
                }
          
            }
            using (migenteEntities db1 = new migenteEntities())
            {
                db1.Remuneraciones.AddRange(rem);
                db1.SaveChanges();
                return true;
            }

        }



        public List<Deducciones_TSS> deducciones()
        {
            var db = new migenteEntities();
            return db.Deducciones_TSS.ToList();
        }

    }
}