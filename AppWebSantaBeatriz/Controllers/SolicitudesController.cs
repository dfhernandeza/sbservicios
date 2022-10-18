using System;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorSolicitudes;
using System.Threading.Tasks;
namespace AppWebSantaBeatriz.Controllers
{
     [Authorize]
    public class SolicitudesController : Controller
    {
       public async Task<ActionResult> VerSolicitudes(){
           var data = await ListaSolicitudes();
           ViewBag.datos = data;
           return View(data);
       }

        public async Task<JsonResult> getDataSolicitudes(){
            return Json(await ListaSolicitudes());
        }


    }
}