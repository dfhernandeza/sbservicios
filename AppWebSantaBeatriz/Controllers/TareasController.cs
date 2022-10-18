using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorTareas;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using AppWebSantaBeatriz.Models.Servicios;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using AppWebSantaBeatriz.Reportes;
using System.IO;
using BibliotecaDeClases.Modelos.Servicios;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorTransacciones;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorInforme;
namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class TareasController : Controller
    {
        // GET: Tareas


        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> CreaTarea(BibliotecaDeClases.Modelos.Servicios.TareaModel modelo)
        {
            modelo.Nombre.ToUpperCheckForNull();
            modelo.Descripcion.ToUpperCheckForNull();
            modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
            modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
            modelo.CECO.EditadoPor = modelo.CECO.CreadoPor = User.Identity.Name;
            modelo.CECO.EditadoEn = modelo.CECO.CreadoEn = DateTime.Now;
            try
            {
                var id = await CrearTareaAsync(modelo);
                modelo.CECO.IDTarea = id;
     
                await NuevoCecoAsync(modelo.CECO);
                return Json("La tarea ha sido guardada.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }


        }
        public async Task<JsonResult> CargarTarea(int idtarea)
        {
            return Json(await LoadTareaAsync(idtarea));
        }
        public async Task<JsonResult> EditaTarea(int ID, string Nombre, DateTime FechaInicial, DateTime FechaFinal, string Descripcion, int IDEncargado, int IDUbicacion)
        {
            try
            {
                await EditarTareaAsync(ID, Nombre.ToUpperCheckForNull(), FechaInicial, FechaFinal, Descripcion.ToUpperCheckForNull(), IDEncargado, User.Identity.Name, IDUbicacion);
                return Json("La tarea ha sido editada.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }



        }
        public async Task<JsonResult> EliminaTarea(int idtarea)
        {
            try
            {
                await BorrarTarea(idtarea);
                return Json("La tarea ha sido eliminada.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }





        }
        public async Task<JsonResult> ListadoTareasAsync(int idservicio)
        {
            var data = await LoadTareasAsync(idservicio);
            return Json(data);

        }
        public async Task<JsonResult> LimitesTarea(DateTime FechaEntrega, int idtarea)
        {
            var data = await LoadTareaAsync(idtarea);
            if (FechaEntrega <= data.FechaFinal & FechaEntrega >= data.FechaInicial)
            {
                return Json(true);
            }
            else
            {
                return Json("Debe estar dentro de la duración de la tarea principal");
            }

        }
        public async Task<JsonResult> EmpleadosPorTarea(int idtarea)
        {
            return Json(await EmpleadosTarea(idtarea));
        }

        [HttpPost]
        public async Task<JsonResult> NuevosEntregables(IList<Models.Servicios.EntregableModel> entregables)
        {
            List<BibliotecaDeClases.Modelos.Servicios.EntregableModel> listaentregables = new List<BibliotecaDeClases.Modelos.Servicios.EntregableModel>();
            foreach (var row in entregables)
            {
                listaentregables.Add(new BibliotecaDeClases.Modelos.Servicios.EntregableModel
                {
                    IDTarea = row.IDTarea,
                    Entregable = row.Entregable,
                    IDCotizacion = row.IDCotizacion,
                    Cantidad = row.Cantidad,
                    PProduccion = row.PProduccion,
                    PVenta = row.PVenta,
                    Unidad = row.Unidad,
                    CreadoPor = User.Identity.Name
                });
            }
            try
            {
                await EntregablesIngreso(listaentregables);
                return Json("El entregable ha sido guardado.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }


        }
        [HttpPost]
        public async Task<JsonResult> NuevoEntregable(Models.Servicios.EntregableModel entregable)
        {
            List<BibliotecaDeClases.Modelos.Servicios.EntregableMaterialPT> lista = new List<BibliotecaDeClases.Modelos.Servicios.EntregableMaterialPT>();
            foreach (var row in entregable.MaterialesEntregable)
            {
                lista.Add(new BibliotecaDeClases.Modelos.Servicios.EntregableMaterialPT
                {
                    IDCotMaterial = row.IDCotMaterial_EntregableMaterialPT,
                    IDEntregablePT = row.IDEntregablePT_EntregableMaterialPT,
                    Tipo = row.Tipo_EntregableMaterialPT,
                    CreadoPor = User.Identity.Name
                });
            }

            BibliotecaDeClases.Modelos.Servicios.EntregableModel nuevoentregable = new BibliotecaDeClases.Modelos.Servicios.EntregableModel
            {
                Entregable = entregable.Entregable,
                FechaEntrega = entregable.FechaEntrega,
                FechaInicial = entregable.FechaInicial,
                Comentarios = entregable.Comentarios,
                Instrucciones = entregable.Instrucciones,
                IDEncargado = entregable.IDEncargado,
                IDCotizacion = entregable.IDCotizacion,
                IDTarea = entregable.IDTarea,
                Cantidad = entregable.Cantidad,
                Unidad = entregable.Unidad,
                CreadoPor = User.Identity.Name,
                IDCategoriaEntregable = entregable.IDCategoriaEntregable,
                MaterialesEntregable = lista

            };

            try
            {
                await CreaEntregableAsync(nuevoentregable);
                return Json("El entregable ha sido guardado.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }


        }
        public async Task<JsonResult> CargarMaterialesPT(int idcot, int idservicio)
        {
            var data = await ObtenerMaterialesPT(idcot, idservicio);
            return Json(data);
        }
        public async Task<JsonResult> CargarEntregableAsync(int id)
        {
            return Json(await CargaEntregable(id));
        }
        public async Task<JsonResult> CargaMaterialesXEntregableAsync(int identregable)
        {
            return Json(await MaterialesXEntregable(identregable));
        }
        public async Task<JsonResult> EditaEntregableAsync(Models.Servicios.EntregableModel entregable)
        {
            return Json(await EditaEntregable(entregable.ID, entregable.Unidad, entregable.IDEncargado, entregable.Cantidad, entregable.Entregable, entregable.FechaEntrega, entregable.FechaInicial, entregable.Comentarios,
                entregable.Instrucciones, User.Identity.Name));
        }
        public async Task<JsonResult> LoadItemsAFabricarAsync(int idcot)
        {
            var data = await CargaItemsAFabricarAsync(idcot);

            return Json(data);
        }
        public async Task<JsonResult> LoadEntregablesTarea(int idtarea)
        {
            return Json(await EntregablesXTarea(idtarea));
        }
        public async Task<JsonResult> NuevoEMPT(List<Models.Servicios.EntregableMaterialPT> modelo)
        {
            List<BibliotecaDeClases.Modelos.Servicios.EntregableMaterialPT> lista = new List<BibliotecaDeClases.Modelos.Servicios.EntregableMaterialPT>();
            foreach (var row in modelo)
            {
                lista.Add(new BibliotecaDeClases.Modelos.Servicios.EntregableMaterialPT
                {
                    IDCotMaterial = row.IDCotMaterial_EntregableMaterialPT,
                    IDEntregable = row.IDEntregable_EntregableMaterialPT,
                    IDEntregablePT = row.IDEntregablePT_EntregableMaterialPT,
                    CreadoPor = User.Identity.Name,
                    CreadoEn = DateTime.Now,
                    EditadoPor = User.Identity.Name,
                    EditadoEn = DateTime.Now
                });
            }
            try
            {
                await NuevoEntregableMaterialPT(lista);
                return Json("El entregable ha sido guardado.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }


        }
        public async Task<JsonResult> LoadEMPT(int idcot, int idservicio)
        {
            return Json(await CargaMaterialesYPT(idcot, idservicio));
        }
        public async Task<JsonResult> EliminaEntregable(int id)
        {

            try
            {
                return Json(await EliminaEntregableAsync(id));
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public ActionResult CreaTipoTarea()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreaTipoTarea(Models.Servicios.TipoTareaModel modelo)
        {
            if (ModelState.IsValid)
            {
                BibliotecaDeClases.Modelos.Servicios.TipoTareaModel tipoTarea = new BibliotecaDeClases.Modelos.Servicios.TipoTareaModel
                {
                    TipoTarea = modelo.TipoTarea.ToUpperCheckForNull(),
                    Descripcion = modelo.Descripcion.ToUpperCheckForNull(),
                    CreadoPor = User.Identity.Name,
                    EditadoPor = User.Identity.Name,
                    EditadoEn = DateTime.Now,
                    CreadoEn = DateTime.Now

                };
                try
                {

                    await NuevoTipoTarea(tipoTarea);
                    return RedirectToAction("VerTiposTareas");
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
        public async Task<ActionResult> VerTiposTareas()
        {
            var data = await CargarTiposTareas();
            List<Models.Servicios.TipoTareaModel> lista = new List<Models.Servicios.TipoTareaModel>();
            foreach (var row in data)
            {
                lista.Add(new Models.Servicios.TipoTareaModel
                {
                    ID = row.ID,
                    TipoTarea = row.TipoTarea,
                    Descripcion = row.Descripcion
                });
            }

            return View(lista);

        }
        public async Task<ActionResult> EditarTipoTarea(int id)
        {
            var data = await CargarTipoTarea(id);
            Models.Servicios.TipoTareaModel modelo = new Models.Servicios.TipoTareaModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                TipoTarea = data.TipoTarea
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EditarTipoTarea(Models.Servicios.TipoTareaModel modelo)
        {
            if (ModelState.IsValid)
            {
                BibliotecaDeClases.Modelos.Servicios.TipoTareaModel tipoTarea = new BibliotecaDeClases.Modelos.Servicios.TipoTareaModel
                {
                    TipoTarea = modelo.TipoTarea.ToUpperCheckForNull(),
                    Descripcion = modelo.Descripcion.ToUpperCheckForNull(),
                    CreadoPor = User.Identity.Name,
                    EditadoPor = User.Identity.Name,
                    EditadoEn = DateTime.Now,
                    CreadoEn = DateTime.Now,
                    ID = modelo.ID

                };
                try
                {

                    await EditaTipoTarea(tipoTarea);
                    return RedirectToAction("VerTiposTareas");
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
        public async Task<ActionResult> DetalleTipoTarea(int id)
        {
            var data = await CargarTipoTarea(id);
            Models.Servicios.TipoTareaModel modelo = new Models.Servicios.TipoTareaModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                TipoTarea = data.TipoTarea,
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                EditadoEn = data.EditadoEn,
                EditadoPor = data.EditadoPor
            };
            return View(modelo);
        }
        public async Task<ActionResult> EliminarTipoTarea(int id)
        {
            var data = await CargarTipoTarea(id);
            Models.Servicios.TipoTareaModel modelo = new Models.Servicios.TipoTareaModel
            {
                ID = data.ID,
                Descripcion = data.Descripcion,
                TipoTarea = data.TipoTarea,
                CreadoEn = data.CreadoEn,
                CreadoPor = data.CreadoPor,
                EditadoEn = data.EditadoEn,
                EditadoPor = data.EditadoPor
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EliminarTipoTarea(Models.Servicios.TipoTareaModel modelo)
        {
            try
            {

                await EliminaTipoTarea(modelo.ID);
                return RedirectToAction("VerTiposTareas");
            }
            catch (Exception e)
            {
                return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
            }


        }

       


        //==========================================================================================================================
        //Bitacora
        //========================================================================================================================== 
    
        public async Task<JsonResult> ObtenerBitacora(int idtarea)
        {

            return Json(await getBitacora(idtarea));


        }
        public async Task<JsonResult> editBitacora(ActualizacionTareasModel modelo)
        {
            modelo.EditadoEn = DateTime.Now;
            modelo.EditadoPor = User.Identity.Name;
            try
            {
                await editaBitacora(modelo);
                return Json("Bitacora Editada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }


        }

        //==========================================================================================================================
        //Herramientas
        //========================================================================================================================== 
        public async Task<JsonResult> nuevaStHerramienta(List<SolicitudHerramientaModel> modelo)
        {
            foreach (var item in modelo)
            {
                item.CreadoEn = item.EditadoEn = DateTime.Now;
                item.CreadoPor = item.EditadoPor = User.Identity.Name;
                item.IDEstado = 1;
            }

            try
            {
                await insertaSolicitudHerramienta(modelo);
                return Json("Solicitud Insertada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> editaStHerramienta(List<SolicitudHerramientaModel> modelo)
        {

            modelo.FirstOrDefault().EditadoEn = DateTime.Now;
            modelo.FirstOrDefault().EditadoPor = User.Identity.Name;



            try
            {
                await updateSolicitudHerramienta(modelo.FirstOrDefault());
                return Json("Solicitud Actualizada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> eliminaStHerramienta(int id)
        {


            try
            {
                await deleteHerramienta(id);
                return Json("Solicitud Eliminada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> getTools(int idtarea)
        {
            return Json(await getSolicitudesHerramienta(idtarea));
        }
        //==========================================================================================================================
        //Herramientas
        //========================================================================================================================== 
        public async Task<JsonResult> nuevaStEpp(List<SolicitudEppModel> modelo)
        {
            foreach (var item in modelo)
            {
                item.CreadoEn = item.EditadoEn = DateTime.Now;
                item.CreadoPor = item.EditadoPor = User.Identity.Name;
                item.IDEstado = 1;
            }

            try
            {
                await insertaSolicitudEpp(modelo);
                return Json("Solicitud Insertada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> editaStEpp(List<SolicitudEppModel> modelo)
        {

            modelo.FirstOrDefault().EditadoEn = DateTime.Now;
            modelo.FirstOrDefault().EditadoPor = User.Identity.Name;



            try
            {
                await updateSolicitudEpp(modelo.FirstOrDefault());
                return Json("Solicitud Actualizada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> eliminaStEpp(int id)
        {


            try
            {
                await deleteEpp(id);
                return Json("Solicitud Eliminada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> getEpps(int idtarea)
        {
            return Json(await getSolicitudesEpp(idtarea));
        }
        //==========================================================================================================================
        //Dinero
        //========================================================================================================================== 
        public async Task<JsonResult> nuevaStDinero(List<SolicitudDinero> modelo)
        {
            foreach (var item in modelo)
            {
                item.CreadoEn = item.EditadoEn = DateTime.Now;
                item.CreadoPor = item.EditadoPor = User.Identity.Name;
                item.IDEstado = 1;
            }

            try
            {
                await insertaSolicitudDinero(modelo);
                return Json("Solicitud Insertada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> editaStDinero(List<SolicitudDinero> modelo)
        {

            modelo.FirstOrDefault().EditadoEn = DateTime.Now;
            modelo.FirstOrDefault().EditadoPor = User.Identity.Name;



            try
            {
                await updateSolicitudDinero(modelo.FirstOrDefault());
                return Json("Solicitud Actualizada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> eliminaStDinero(int id)
        {


            try
            {
                await deleteSolicitudDinero(id);
                return Json("Solicitud Eliminada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> getSolDinero(int idtarea)
        {
            return Json(await getSolicitudesDinero(idtarea));
        }
        //==========================================================================================================================
        //Equipos de apoyo y Servicios Profesionales Externos
        //========================================================================================================================== 

        public async Task<JsonResult> nuevaStEyS(List<TareaEyS> modelo)
        {
            if (modelo == null)
            {
                return Json(false);
            }
            foreach (var item in modelo)
            {
                item.CreadoEn = item.EditadoEn = DateTime.Now;
                item.CreadoPor = item.EditadoPor = User.Identity.Name;
                item.IDEstado = 1;
            }

            try
            {
                await insertaSolicitudEyS(modelo);
                return Json("Solicitud Insertada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }

        public async Task<JsonResult> editaStEyS(TareaEyS modelo)
        {

            modelo.EditadoEn = DateTime.Now;
            modelo.EditadoPor = User.Identity.Name;



            try
            {
                await updateSolicitudEyS(modelo);
                return Json("Solicitud Actualizada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }

        public ActionResult editaStEySView()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult eliminaStEyS()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> eliminaStEyS(int id)
        {


            try
            {
                await deleteSolicitudEyS(id);
                return Json("Solicitud Eliminada");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> getSolEyS(int idtarea)
        {
            return Json(await getSolicitudesEyS(idtarea));
        }

        //==========================================================================================================================
        //Actualizar progreso tarea
        //========================================================================================================================== 

        public async Task<JsonResult> ActualizarProgresoTareaAsync(ActualizacionTareasModel modelo)
        {
            try
            {
                modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                await actualizarProgresoTareaAsync(modelo);
                return Json("Exito");
            }
            catch (Exception e)
            {

                return Json(e.Message);
            }
            
        }
    }
}