using AppWebSantaBeatriz.Models;
using BibliotecaDeClases.Logica.Cotizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorSeguridad;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using static BibliotecaDeClases.Logica.Funciones;

namespace AplicacionWebSB.Controllers
{
    [Authorize]
    public class CotSeguridadController : Controller
    {
        // GET: CotSeguridad
        public ActionResult Index()
        {
            return View();
        }

      

        public async System.Threading.Tasks.Task<ActionResult> VerSeguridadAsync(int id)

        {
            ViewBag.id = id;
            var data = await LoadCotSeguridadAsync(id);
            if (data.Count == 0)
            {

                List<CotizacionSeguridadModel> seguridades = new List<CotizacionSeguridadModel>();
                return View(seguridades);

            }
            else
            {
                List<CotizacionSeguridadModel> seguridades = new List<CotizacionSeguridadModel>();
                foreach (var row in data)
                {
                    CotizacionSeguridadModel seguridad = new CotizacionSeguridadModel
                    {
                        ID = row.ID,
                        IDCot = row.IDCot,
                        Cantidad = row.Cantidad,
                        Item = row.Item,
                        PUnitario = row.PUnitario


                    };

                    seguridades.Add(seguridad);

                }


                return View(seguridades);



            }

        }

        
        public async System.Threading.Tasks.Task<JsonResult> CotiSeguridadAsync(int idcot, string item, double cantidad, double punitario)
        {
            try
            {
               await CreaCotSeguridadAsync(idcot, item.ToUpperCheckForNull(), cantidad, punitario);

                return Json("Insertado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

           


        }



        public async System.Threading.Tasks.Task<JsonResult> EditarSeguridadAsync(int id, string item, double cantidad, double punitario)
        {
            try
            {
               await ProcesadorSeguridad.EditarSeguridadAsync(id, item.ToUpperCheckForNull(), cantidad, punitario);

                return Json("Editado");
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
           
        }


   

        public async System.Threading.Tasks.Task<JsonResult> BorrarSeguridadAsync(int id)
        {
            try
            {
               await BorraSeguridadAsync(id);

                return Json("Eliminado");
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
         



        }










    }
}