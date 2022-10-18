using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.Traductor;

namespace AppWebSantaBeatriz.Controllers
{
    public class ErroresController : Controller
    {
        // GET: Errores
        public ActionResult VistaError(Error modelo)
        {
            return View(modelo);
        }
    }
}