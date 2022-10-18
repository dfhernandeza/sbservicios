using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.ProcesadorPersonal;
using AppWebSantaBeatriz.Models;
using static BibliotecaDeClases.Logica.Personal.ProcesadorEspecialidades;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using System.Threading.Tasks;
namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class CotPersonalController : Controller
    {
        // GET: CoPersonal
        public ActionResult Index()
        {
            return View();
      
               
        }
        public JsonResult GetEspecialidades()
        {
            return Json(EspecialidadesAsync());
        }          
        public async Task<JsonResult> EditarPersonal(BibliotecaDeClases.Modelos.CotizacionPersonalModel modelo)
        {
            try
            {
                modelo.EditadoEn = DateTime.Now;
                modelo.EditadoPor = User.Identity.Name;
               await BibliotecaDeClases.Logica.ProcesadorPersonal.EditarPersonalAsync(modelo);

                return Json("Actualizado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
           
        }        
        public async Task<JsonResult> BorrarPersonal(int id)
        {
            try
            {
               await BorraPersonalAsync(id);

                return Json("Eliminado", JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }
          

        }    
        public async System.Threading.Tasks.Task<JsonResult> GetValorHH(int id)
        {
            return Json(await CostoHHAsync(id));
        }
        public async Task<ActionResult> VerPersonal(int id)

        {
            ViewBag.id = id;
            var data = await LoadCotPersonalAsync(id);
            if (data.Count == 0)
            {

                List<Models.CotizacionPersonalModel> cotpersonal = new List<Models.CotizacionPersonalModel>();
                return View(cotpersonal);

            }
            else
            {
                List<Models.CotizacionPersonalModel> personal = new List<Models.CotizacionPersonalModel>();
                foreach (var row in data)
                {
                    Models.CotizacionPersonalModel especialidad = new Models.CotizacionPersonalModel
                    {
                        ID = row.ID,
                        IDCot = row.IDCot,
                        Cantidad = row.Cantidad,
                        Especialidad = row.Especialidad,
                        HH = row.HH,
                        ValorHH = row.ValorHH,
                        IDEspecialidad = row.IDEspecialidad,
                        

                    };

                    personal.Add(especialidad);

                }


                return View(personal);



            }

        } 
        [HttpPost]
        public async Task<JsonResult> NuevoPersonal(BibliotecaDeClases.Modelos.CotizacionPersonalModel modelo)
        {
            try
            {
                modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                await CreaCotPersonalAsync(modelo);

                return Json("Ingresado", JsonRequestBehavior.AllowGet);
            }
           catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }

        

    }

}