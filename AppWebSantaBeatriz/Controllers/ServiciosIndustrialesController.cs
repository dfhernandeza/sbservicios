using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.ServiciosIndustriales.ProcesadorServiciosIndustriales;
using BibliotecaDeClases.Modelos.ServiciosIndustriales;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;

namespace AppWebSantaBeatriz.Controllers
{
    public class ServiciosIndustrialesController : Controller
    {
        // GET: ServiciosIndustriales
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Electricidad()
        {
            var data = await getFotosElectricidad();
            return View(new PaginaServicioModel { Fotos = data});
        }
        public async Task<ActionResult> Mecanica()
        {
            var data = await getFotosMecanica();
            return View(new PaginaServicioModel { Fotos = data });
        }
        public async Task<ActionResult> Estructuras()
        {
            var data = await getFotosEstrucuturas();
            return View(new PaginaServicioModel { Fotos = data });
        }
        public async Task<ActionResult> Hidraulica()
        {
            var data = await getFotosHidraulica();
            return View(new PaginaServicioModel { Fotos = data });
        }
        public ActionResult Clientes()
        {
            return View();
        }

        public ActionResult Contacto()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contacto(ContactoCorreoModel modelo)
        {
            await nuevoContactoAsync(modelo);
            return RedirectToAction("Home");
        }
    }
}