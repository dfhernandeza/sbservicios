using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorEquiposServicios;
using static BibliotecaDeClases.Logica.Funciones;
using AppWebSantaBeatriz.Models;
using AppWebSantaBeatriz.Models.Error;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;


namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class CotEquiposServiciosController : Controller
    {
        // GET: CotEquiposServicios
        public ActionResult Index()
        {
            return View();
        }
        public async System.Threading.Tasks.Task<ActionResult> VerEquipos(int id)

        {
            ViewBag.id = id;
            var data = await LoadCotEquiposAsync(id);
            if (data.Count == 0)
            {

                List<Models.CotizacionEquiposModel> equipos = new List<Models.CotizacionEquiposModel>();
                return View(equipos);

            }
            else
            {
                List<Models.CotizacionEquiposModel> equipos = new List<Models.CotizacionEquiposModel>();
                foreach (var row in data)
                {
                    Models.CotizacionEquiposModel equipo = new Models.CotizacionEquiposModel
                    {
                        ID = row.ID,
                        IDCot = row.IDCot,
                        Cantidad = row.Cantidad,
                        Item = row.Item,
                        PUnitario = row.PUnitario,
                        Unidad = row.Unidad,

                    };

                    equipos.Add(equipo);

                }


                return View(equipos);



            }

        }     
        public async System.Threading.Tasks.Task<JsonResult> CotiEquiposServicios(int idcot, string item, string unidad, decimal cantidad, decimal punitario)
        {

            try
            {
                await CreaCotEquiposAsync(idcot, item.ToUpperCheckForNull(), unidad.ToUpperCheckForNull(), cantidad, punitario, User.Identity.Name );
                 return Json("Ingresado");
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));

            }
            

       

        }  
        public async System.Threading.Tasks.Task<JsonResult> EditarEquiposServicios(int idcot, string item, string unidad, decimal cantidad, decimal punitario)
        {
            try
            {
               await EditaEquipoAsync(idcot, item.ToUpperCheckForNull(), unidad.ToUpperCheckForNull(), cantidad, punitario, User.Identity.Name);
                return Json("Editado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }       

        }
    
        public async System.Threading.Tasks.Task<JsonResult> BorrarEquiposServicios(int id)
        {
            try
            {
               await BorraEquipoAsync(id);
                return Json("Eliminado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
            

        }


    }
}