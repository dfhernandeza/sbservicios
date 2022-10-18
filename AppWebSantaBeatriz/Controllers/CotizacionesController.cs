using AppWebSantaBeatriz.Models;
using AppWebSantaBeatriz.Models.Cotizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.ProcesadorCotizaciones;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorEstadoCotizacion;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorServicios;
using static BibliotecaDeClases.Logica.Clientes.ProcesadorClientes;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorTipoCotizacion;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorEquiposServicios;
using static BibliotecaDeClases.Logica.ProcesadorMateriales;
using static BibliotecaDeClases.Logica.ProcesadorPersonal;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorSeguridad;
using static BibliotecaDeClases.Logica.Personal.ProcesadorEspecialidades;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using AppWebSantaBeatriz.Models.Error;
using Newtonsoft.Json;
using System.IO;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorCotizacionesArauco;
using static BibliotecaDeClases.Modelos.Cotizaciones.SubCotizacionModel;
using System.Threading.Tasks;
using static BibliotecaDeClases.Logica.Excel.EditorExcel;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace AplicacionWebSB.Controllers
{
    [Authorize]
    public class CotizacionesController : Controller
    {
        //Cotizaciones de cualquier cliente
        //==========================================================================================================================
    
        public async Task<ActionResult> Crear(int idcotiprincipal, int idcliente)
        {
            var clientes = await ListaClientesAsync();
                CotizacionModel modelo = new CotizacionModel
                {
                    Clientes = clientes,
                    IDTipoCotizacion = 1

                };
                return View(modelo);
           
        }                  
        [HttpPost]
        public async Task<ActionResult> Crear(CotizacionModel model)
        {

            if (ModelState.IsValid)
            {
                try

                {  
                   var data = CreaCotizacion(model.IDCotizacion, model.Servicio.ToUpperCheckForNull(), (DateTime)model.Fecha, model.IDCliente, model.IDSupervisor, model.Detalles.ToUpperCheckForNull(), 
                       model.TiempoEjecucion, model.SolicitudPedido,model.NLicitacion,model.Tasa, model.IDTipoCotizacion ,model.ValidezOferta , User.Identity.Name, false);                                
                 
                 return RedirectToAction("VerCotizacion", "Cotizaciones", new { id = data.ID });
                }
                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));

                }

            }
            else
            {
                var clientes = await ListaClientesAsync();
                model.Clientes = clientes;

                return View(model);
            }

        

        }
         [HttpGet]    
        public async Task<ActionResult> EditarCoti(int id)

        {
            var coti = await CargarCotizacionAsync(id);
            var clientes = await ListaClientesAsync();
            var contactos = await ListaContactosAsync(coti.IDCliente);
            CotizacionModel cotizacion = new CotizacionModel
            {
                
                ID = coti.ID,
                IDCotizacion = coti.IDCotizacion,
                IDCliente = coti.IDCliente,
                Detalles = coti.Detalles,
                Fecha = coti.Fecha,
                Servicio = coti.Servicio,
                IDSupervisor = coti.IDSupervisor,
                TiempoEjecucion = coti.TiempoEjecucion,
                Clientes = clientes ,
                Contactos = contactos 
            };
          


            return View(cotizacion);
        }
        [HttpPost]
        public async Task<ActionResult> EditarCoti(CotizacionModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                  await EditacotizacionAsync(modelo.ID, modelo.IDCotizacion , modelo.Servicio.ToUpperCheckForNull(), modelo.Fecha, modelo.IDCliente, modelo.IDSupervisor, modelo.Detalles.ToUpperCheckForNull(), modelo.TiempoEjecucion, User.Identity.Name);
                    return RedirectToAction("VerCotizaciones");
                }
                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
            }
            else
            {
                return View(modelo);
            }           
                
           
        }

        public async Task<ActionResult> EliminaCotizacion(int id)
        {
            var coti = await CargarCotizacionAsync(id);
            var clientes = await ListaClientesAsync();
            var contactos = await ListaContactosAsync(coti.IDCliente);
            CotizacionModel cotizacion = new CotizacionModel
            {

                ID = coti.ID,
                IDCotizacion = coti.IDCotizacion,
                IDCliente = coti.IDCliente,
                Detalles = coti.Detalles,
                Fecha = coti.Fecha,
                Servicio = coti.Servicio,
                IDSupervisor = coti.IDSupervisor,
                TiempoEjecucion = coti.TiempoEjecucion,
                Clientes = clientes,
                Contactos = contactos
            };



            return View(cotizacion);

        }
        [HttpPost]
        public async Task<ActionResult> EliminaCotizacion(CotizacionModel modelo)
        {
            try
            {
               await BibliotecaDeClases.Logica.ProcesadorCotizaciones.EliminaCotizacionAsync(modelo.ID);
                return RedirectToAction("VerCotizaciones");
            }
            catch(Exception e)
            {
                var datos = Traducir(e.Message).Result.FirstOrDefault().Translations[0];

                ErrorModel error = new ErrorModel
                {
                    Mensaje = datos.Text
                };

                return RedirectToAction("VistaError", "Errores", error);
            }
            
        }

        public async Task<JsonResult> CambiaEstado(int idcoti, int idestado)
        {
           await CambiarEstadoCotizacionAsync(idcoti, idestado);
            return Json("Estado cambiado");
        }     
        public async Task<ActionResult> VerCotizacion(int id)
        {


            var datos = LoadCotizacionAsync(id);
            var materiales =  LoadCotMaterialesAsync(id);
            var personal =  LoadCotPersonalAsync(id);
            var equipos =  LoadCotEquiposAsync(id);
            var seguridad =  LoadCotSeguridadAsync(id);
            var especialidad =  TablaEspecialidadesAsync();
            var estados =  LoadEstadosAsync();
            var cat = ListaCategorias();
            await Task.WhenAll(datos, materiales, personal, equipos, seguridad, especialidad, estados, cat);

            List<CotizacionEquiposModel> listaequipos = new List<CotizacionEquiposModel>();
            foreach(var row in equipos.Result)
            {
                listaequipos.Add(new CotizacionEquiposModel { ID = row.ID, Cantidad = row.Cantidad, IDCot = row.IDCot, Item = row.Item, PUnitario = row.PUnitario, Unidad = row.Unidad });
            }

        
            List<CotizacionMaterialesModel> listamaterial = new List<CotizacionMaterialesModel>();
            foreach(var row in materiales.Result)
            {
                listamaterial.Add(new CotizacionMaterialesModel { ID = row.ID, Cantidad = row.Cantidad, IDCot = row.IDCot, Item = row.Item, PUnitario = row.PUnitario, Unidad = row.Unidad, IDCategoria = row.IDCategoria });
            }

            
            List<CotizacionPersonalModel> listapersonal = new List<CotizacionPersonalModel>();
            foreach (var row in personal.Result)
            {
                    listapersonal.Add(new CotizacionPersonalModel { ID = row.ID, Cantidad = row.Cantidad, IDCot = row.IDCot, Especialidad = row.Especialidad,
                    ValorHH = row.ValorHH, HH = row.HH, IDEspecialidad = row.IDEspecialidad });
            }

            
            List<CotizacionSeguridadModel> listaseguridad = new List<CotizacionSeguridadModel>();
            foreach (var row in seguridad.Result)
            {
                listaseguridad.Add(new CotizacionSeguridadModel { ID = row.ID, Cantidad = row.Cantidad, IDCot = row.IDCot, Item = row.Item, PUnitario = row.PUnitario});
            }

          
            List<EspecialidadModel> listaespecialidades = new List<EspecialidadModel>();
            foreach(var row in especialidad.Result)
            {
                listaespecialidades.Add(new EspecialidadModel { ID = row.ID, Especialidad = row.Especialidad });
            }


            var fpropia = await ListaProductoFabricacionPropiaAsync(id);
            List<SubCotizacionModel> listafpropia = new List<SubCotizacionModel>();
            foreach(var row in fpropia)
            {
                listafpropia.Add(new SubCotizacionModel { ID = row.ID, Servicio = row.Servicio, Unidad = row.Unidad, Cantidad = row.Cantidad, Total = row.Total, Utilidad = row.Utilidad });
            }

            var data = await datos;

            CotizacionModel cotizacion = new CotizacionModel
            {
                ID = data.ID,
                IDCotizacion = data.IDCotizacion,
                Cliente = data.Cliente,
                IDCliente = data.IDCliente,
                Fecha = data.Fecha,
                Servicio = data.Servicio,
                Supervisor = data.Supervisor,
                TiempoEjecucion = data.TiempoEjecucion,
                Detalles = data.Detalles,
                TotalMateriales = data.TotalMateriales,
                TotalPersonal  = data.TotalPersonal,
                TotalSeguridad = data.TotalSeguridad,
                TotalEquipos = data.TotalEquipos,
                TotalSubCotizaciones = data.TotalSubCotizaciones,
                Estados = estados.Result,
                Estado = data.Estado,
                SolicitudPedido = data.SolicitudPedido,
                ListaEquipos = listaequipos,
                ListaMateriales= listamaterial,
                ListaPersonal = listapersonal,
                ListaSeguridad = listaseguridad,
                Especialidades = listaespecialidades,
                ListaProductosPropios = listafpropia,
                CategoriasMRE = cat.Result,
                GastosGenerales = data.GastosGenerales
                

            };



            return View(cotizacion);
        }
        public async Task<ActionResult> VerCotizaciones()
        {


            var data = await LoadCotizacionesAsync();
            List<CotizacionModel> cotizaciones = new List<CotizacionModel>();
            foreach(var row in data)
            {
                CotizacionModel cotizacion = new CotizacionModel

                {
                   ID = row.ID,
                   IDCotizacion = row.IDCotizacion,
                   Cliente = row.Cliente,
                   Fecha = row.Fecha,
                   Servicio = row.Servicio,
                   Supervisor = row.Supervisor,
                   TiempoEjecucion = row.TiempoEjecucion,
                   Detalles = row.Detalles,
                   Total = row.Total,
                   Estado = row.Estado



                 };

                cotizaciones.Add(cotizacion);
            }

            
            return View(cotizaciones);
        }
        public ActionResult NuevaCotizacionFabricacionPropia(int idcotiprincipal, string cotiprincipal, string origen)
        {
            SubCotizacionModel modelo = new SubCotizacionModel
            {
                
                IDTipoCotizacion = 2,
                IDCotizacionPrincipal = idcotiprincipal,
                CotizacionPrincipal = cotiprincipal,
                Cantidad = null,
                Origen = origen

            };
            return View(modelo);
        }
        [HttpPost]
        public ActionResult NuevaCotizacionFabricacionPropia(SubCotizacionModel model)
        {
            if (ModelState.IsValid)
            {
                try

                {
                    var data = CreaCotizacionProductoPropio(model.Servicio.ToUpperCheckForNull(),model.Detalles.ToUpperCheckForNull(), model.IDCotizacionPrincipal,User.Identity.Name, model.Unidad.ToUpperCheckForNull(), (double)model.Cantidad);
                    if(model.Origen == "Arauco")
                    {
                        return RedirectToAction("VerProductoFabricacionPropia", "CotizacionArauco", new { id = data.ID });
                    }
                    else
                    {

                    }
                    return RedirectToAction("VerProductoFabricacionPropia", "Cotizaciones", new { id = data.ID });
                }
                catch (Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));

                }

            }
            else
            {
                
                return View(model);
            }



        }

        public async Task<ActionResult> EditarCotizacionFabricacionPropia(int id, string origen)
        {
            var data = new BibliotecaDeClases.Modelos.Cotizaciones.SubCotizacionModel();
            if (origen == "Arauco")
            {
                 data = await CargaSubCotizacionAraucoAsync(id);
                return View(new SubCotizacionModel { ID = data.ID, CotizacionPrincipal = data.CotizacionPrincipal, Producto = data.Producto, Detalles = data.Detalles, 
                Unidad = data.Unidad, Cantidad = data.Cantidad, IDCotizacionPrincipal = data.IDCotizacionPrincipal, Origen = "Arauco" });

            }
            else
            {
                 data = await LoadSubCotizacionAsync(id);
                return View(new SubCotizacionModel
                {
                    ID = data.ID,
                    CotizacionPrincipal = data.CotizacionPrincipal,
                    Producto = data.Servicio,
                    Detalles = data.Detalles,
                    Unidad = data.Unidad,
                    Cantidad = data.Cantidad,
                    IDCotizacionPrincipal = data.IDCotizacionPrincipal
                });
            }
                
           
        }

        [HttpPost]
        public async Task<ActionResult> EditarCotizacionFabricacionPropia(SubCotizacionModel modelo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                   await EditaCotizacionProductoPropioAsync(modelo.ID, modelo.Servicio.ToUpperCheckForNull(), modelo.Detalles.ToUpperCheckForNull(), modelo.Unidad.ToUpperCheckForNull(), (double)modelo.Cantidad, User.Identity.Name);
                    if(modelo.Origen == "Arauco")
                    {
                        return RedirectToAction("Detalles", "CotizacionArauco", new { id = modelo.IDCotizacionPrincipal });
                    }
                    else
                    {
                        return RedirectToAction("VerCotizacion", "Cotizaciones", new { id = modelo.IDCotizacionPrincipal });
                    }
                    

                }
                catch (Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
            }
            else
            {
                return View(modelo);
            }
        }

        public async Task<ActionResult> VerProductoFabricacionPropia(int id, string origen)

        {
        
            var data = await CargaSubCotizacionAsync(id);           
          


            var equipos = await LoadCotEquiposAsync(id);
            List<CotizacionEquiposModel> listaequipos = new List<CotizacionEquiposModel>();
            foreach (var row in equipos)
            {
                listaequipos.Add(new CotizacionEquiposModel { ID = row.ID, Cantidad = row.Cantidad, IDCot = row.IDCot, Item = row.Item, PUnitario = row.PUnitario, Unidad = row.Unidad });
            }

            var materiales = await LoadCotMaterialesAsync(id);
            List<CotizacionMaterialesModel> listamaterial = new List<CotizacionMaterialesModel>();
            foreach (var row in materiales)
            {
                listamaterial.Add(new CotizacionMaterialesModel { ID = row.ID, Cantidad = row.Cantidad, IDCot = row.IDCot, Item = row.Item, PUnitario = row.PUnitario, Unidad = row.Unidad });
            }

            var personal = await LoadCotPersonalAsync(id);
            List<CotizacionPersonalModel> listapersonal = new List<CotizacionPersonalModel>();
            foreach (var row in personal)
            {
                listapersonal.Add(new CotizacionPersonalModel
                {
                    ID = row.ID,
                    Cantidad = row.Cantidad,
                    IDCot = row.IDCot,
                    Especialidad = row.Especialidad,
                    ValorHH = row.ValorHH,
                    HH = row.HH,
                    IDEspecialidad = row.IDEspecialidad
                });
            }

            var seguridad = await LoadCotSeguridadAsync(id);
            List<CotizacionSeguridadModel> listaseguridad = new List<CotizacionSeguridadModel>();
            foreach (var row in seguridad)
            {
                listaseguridad.Add(new CotizacionSeguridadModel { ID = row.ID, Cantidad = row.Cantidad, IDCot = row.IDCot, Item = row.Item, PUnitario = row.PUnitario });
            }

            var especialidad = await TablaEspecialidadesAsync();
            List<EspecialidadModel> listaespecialidades = new List<EspecialidadModel>();
            foreach (var row in especialidad)
            {
                listaespecialidades.Add(new EspecialidadModel { ID = row.ID, Especialidad = row.Especialidad });
            }

            SubCotizacionModel modelo = new SubCotizacionModel
            {
                ID = data.ID,
                Cantidad = data.Cantidad,
                CotizacionPrincipal = data.CotizacionPrincipal,
                IDCotizacionPrincipal = data.IDCotizacionPrincipal,
                Unidad = data.Unidad,
                Producto = data.Producto,
                Detalles = data.Detalles,
                ListaMateriales = listamaterial,
                ListaEquipos = listaequipos,
                ListaPersonal = listapersonal,
                ListaSeguridad = listaseguridad,
                Especialidades = listaespecialidades,
                Total = data.Total,
                TotalMateriales = data.TotalMateriales,
                TotalPersonal = data.TotalPersonal,
                TotalSeguridad = data.TotalSeguridad,
                TotalEquipos = data.TotalEquipos,
                Servicio = data.Servicio,
                Utilidad = data.Utilidad

            };


            return View(modelo);
        }

        public async Task<JsonResult> CambiaUtilidadCotizacion(int id, decimal utilidad)
        {
           return Json(await CambiaUtilidadAsync(id, utilidad));
        }
         public async Task<JsonResult> getUtilidadCotizacion(int id)
        {
           return Json(await getUtilidad(id));
        }
        public async Task<JsonResult> CambiaGGCotizacion(int id, float gg)
        {
            return Json(await CambiaGGAsync(id, gg));
        }
        public async Task<JsonResult> EliminaItemFabricacionPropia(int id, int idprincipal)
        {
            try
            {                
               await BibliotecaDeClases.Logica.ProcesadorCotizaciones.EliminaCotizacionAsync(id);
                return Json(RedirectToAction("VerCotizacion", "Cotizaciones", new { id = idprincipal }));
            }
            catch(Exception e)
            {
                var datos = Traducir(e.Message).Result.FirstOrDefault().Translations[0];

                ErrorModel error = new ErrorModel
                {
                    Mensaje = datos.Text
                };

                return Json(RedirectToAction("VistaError", "Errores", error));
            }
        }
        //Estados
        //=======================================================================================================================
        public ActionResult NuevoEstadoCotizacion()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> NuevoEstadoCotizacion(EstadoCotizacionModel model)
        {
            if (ModelState.IsValid)
            {

                  try
                  {
                     await CrearEstadoCotizacionAsync(model.Estado.ToUpperCheckForNull(), model.Descripcion.ToUpperCheckForNull(), User.Identity.Name.ToString());
                      return RedirectToAction("VerEstadosCotizaciones");
                  }
                  catch(Exception e)
                  {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }


            }
            else
            {
                return View(model);

            }
            

            

        }

        public async Task<ActionResult> VerEstadosCotizaciones()
        {
            List<EstadoCotizacionModel> lista = new List<EstadoCotizacionModel>();
            var data = await CargarEstadosCotizacionesAsync();
            foreach(var row in data)
            {
                lista.Add(new EstadoCotizacionModel { ID = row.ID, Estado = row.Estado, Descripcion = row.Descripcion });
            }

            return View(lista);


        }
        public async Task<ActionResult> EditaEstadoCotizacion(int id)
        {
            var data = await CargarEstadoCotizacionAsync(id);

            EstadoCotizacionModel modelo = new EstadoCotizacionModel
            {
                ID = data.FirstOrDefault().ID,
                Estado = data.FirstOrDefault().Estado,
                Descripcion = data.FirstOrDefault().Descripcion
            };

           

            return View(modelo);



            
        }

        [HttpPost]
        public async Task<ActionResult> EditaEstadoCotizacion(EstadoCotizacionModel modelo)

        {
            if (ModelState.IsValid)
            {
                try
                {
                  await  EditarEstadoCotizacionAsync(modelo.ID, modelo.Estado.ToUpperCheckForNull(), modelo.Descripcion.ToUpperCheckForNull(), User.Identity.Name.ToString());
                    return RedirectToAction("VerEstadosCotizaciones");
                }
                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
                
            }
            else
            {
                return View(modelo);
            }
          
        }
        public async Task<ActionResult> EliminaEstadCotizacion(int id)
        {
            var data = await CargarEstadoCotizacionAsync(id);

            EstadoCotizacionModel modelo = new EstadoCotizacionModel
            {
                ID = data.FirstOrDefault().ID,
                Estado = data.FirstOrDefault().Estado,
                Descripcion = data.FirstOrDefault().Descripcion
            };



            return View(modelo);




        }

        [HttpPost]
        public async Task<ActionResult> EliminaEstadCotizacion(EstadoCotizacionModel modelo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                   await EliminarEstadoCotizacionAsync(modelo.ID);
                   return RedirectToAction("VerEstadosCotizaciones");
                }
                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));

                }
                


            }
            else
            {
                return View(modelo);
            }



           
        }

        public async Task<ActionResult> CambiaEstadoCotizacion(int idcotizacion)
        {
          await  CambiarEstadoCotizacionAsync(idcotizacion, 3);
            var info = await PresupuestoAsync(idcotizacion);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Create", "Servicios", new { id = idcotizacion, pptomateriales = info.PresupuestoMateriales, pptopersonal = info.PresupuestoPersonal, pptoequipos = info.PresupuestoEquiposServicios, pptoseguridad = info.PresupuestoSeguridad });
            return Json(new { Url = redirectUrl });
            
        
        }

        //Tipos
        //=======================================================================================================================

        public ActionResult CrearTipoCotizacion()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CrearTipoCotizacion(TipoCotizacionModel modelo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                   await NuevoTipoCotizacionAsync(modelo.TipoCotizacion.ToUpperCheckForNull(), modelo.Descripcion.ToUpperCheckForNull(), User.Identity.Name);
                    return RedirectToAction("VerTiposCotizaciones");
                }
                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
                
            }
            else
            {
                return View(modelo);
            }
            
            
        }
        public async Task<ActionResult> EditaTipoCotizacion(int id)
        {
            var data = await CargarTipoCotizacionAsync(id);
            TipoCotizacionModel modelo = new TipoCotizacionModel { ID = data.ID, Descripcion = data.Descripcion, TipoCotizacion = data.TipoCotizacion };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EditaTipoCotizacion(TipoCotizacionModel modelo)
        {
            if (ModelState.IsValid)
            {

                try
                {
                   await  EditarTipoCotizacionAsync(modelo.ID, modelo.TipoCotizacion.ToUpperCheckForNull(), modelo.Descripcion.ToUpperCheckForNull(), User.Identity.Name);
                     return RedirectToAction("VerTiposCotizaciones");
                }

                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }


            }
            else
            {
                return View(modelo);
            }
        }

        public async Task<ActionResult> DetalleTipoCotizacion(int id)
        {
            var data = await CargarTipoCotizacionAsync(id);
            TipoCotizacionModel modelo = new TipoCotizacionModel {
                ID = data.ID, Descripcion = data.Descripcion,
                TipoCotizacion = data.TipoCotizacion, 
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                EditadoEn = data.EditadoEn,
                EditadoPor = data.EditadoPor
            };
            return View(modelo);
        }

        public async Task<ActionResult> EliminaTipoCotizacion(int id)
        {
            var data = await CargarTipoCotizacionAsync(id);
            TipoCotizacionModel modelo = new TipoCotizacionModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                TipoCotizacion = data.TipoCotizacion,
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                EditadoEn = data.EditadoEn,
                EditadoPor = data.EditadoPor
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EliminaTipoCotizacion(TipoCotizacionModel modelo)
        {
           
         
            try
            {
              await  EliminarTipoCotizacionAsync(modelo.ID);
                return RedirectToAction("VerTiposCotizaciones");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }

            }

        public async Task<ActionResult> VerTiposCotizaciones()
        {
            var data = await ListaTipoCotizacionAsync();
            List<TipoCotizacionModel> lista = new List<TipoCotizacionModel>();
            foreach(var row in data)
            {
                lista.Add(new TipoCotizacionModel { ID = row.ID, TipoCotizacion = row.TipoCotizacion, Descripcion = row.Descripcion });
            }

            return View(lista);
        }


        public async Task<FileResult> GetExcelFileAsync(int id)
        {
            var libro = await ReturnExcelSBFileAsync(id);
            return File(libro, "application/xlsx", "Cotización.xlsx");
         
        }

    }

}
