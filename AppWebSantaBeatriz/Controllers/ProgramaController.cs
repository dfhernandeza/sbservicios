using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorPrograma;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using BibliotecaDeClases.Modelos.Servicios;
using System.Threading.Tasks;
namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class ProgramaController : Controller
    {
        // GET: Programa
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CargarAsignaciones(int idservicio)
        {
            return Json(LoadAsignacionesAsync(idservicio));

        }
        public JsonResult CargarAsignacio(int idasignacion)
        {
            return Json(LoadAsignacionAsync(idasignacion));

        }
        public async Task<JsonResult> EditaAsignacion(int id, int idempleado, DateTime fechainicial, DateTime fechatermino, string responsabilidades)
        {
            try
            {
                await EditarAsignacionAsync(id, idempleado, fechainicial, fechatermino, responsabilidades.ToUpperCheckForNull());

                return Json("La asignación ha sido editada.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> BorraAsignacion(int id)
        {
            try
            {
                await BorrarAsignacionAsync(id);

                return Json("La asignación ha sido eliminada.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> CreaAsignacion(int idtarea, int idempleado, DateTime fechainicial, DateTime fechatermino, string responsabilidades)
        {
            try
            {
                await CrearAsignacionAsync(idtarea, idempleado, fechainicial, fechatermino, responsabilidades.ToUpperCheckForNull(), User.Identity.Name);
                return Json("La asignación ha sido creada.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> CreaAsignacionesAsync(List<ProgramaModel> modelo)
        {
            List<string> respuesta = new List<string>();
            if (modelo == null)
            {
                respuesta.Add("Debe seleccionar al menos una persona");
                return Json(respuesta);
            }

            foreach (var item in modelo)
            {
                var data = await DisponibleAsync(item.IDEmpleado, item.FechaInicial, item.FechaTermino, item.IDTarea, "Nueva");
                if (data != "true")
                {
                    respuesta.Add(item.Empleado + " tiene un conflicto de horario");

                }

            }
            if (respuesta.Count > 0)
            {
                return Json(respuesta);
            }
            else
            {

                foreach (var item in modelo)
                {
                    item.CreadoEn = item.EditadoEn = DateTime.Now;
                    item.CreadoPor = item.EditadoPor = User.Identity.Name;
                    item.Responsabilidades = item.Responsabilidades.ToUpperCheckForNull();
                }
                try
                {

                    await CrearAsignacionAsync(modelo);
                    return Json(true);
                }
                catch (Exception e)
                {
                    return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
                }

            }



        }


        public async Task<JsonResult> EmpleadosEnServicioAsync(int idservicio)
        {
            return Json(await EmpleadosPorServicioAsync(idservicio));
        }
        public async Task<JsonResult> Disponibilidad(int idempleado, DateTime fechainicial, DateTime fechatermino, int idtarea, string origen)
        {
            var data = await DisponibleAsync(idempleado, fechainicial, fechatermino, idtarea, origen);
            if (data == "true")
            {
                return Json(true);

            }
            else
            {
                return Json(data);
            }

        }
    }
}