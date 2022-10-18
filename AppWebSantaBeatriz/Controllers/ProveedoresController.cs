using AppWebSantaBeatriz.Models.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Proveedores.ProcesadorProveedores;
using static BibliotecaDeClases.Logica.Funciones;
using System.Threading.Tasks;
namespace AppWebSantaBeatriz.Controllers
{
    public class ProveedoresController : Controller
    {
        // GET: Proveedores
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NuevoProveedor()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NuevoProveedor(BibliotecaDeClases.Modelos.Proveedores.ProveedorModel modelo)
        {
            modelo.IDProveedor = GetRut(modelo.IDProveedor);
            modelo.NombreFantasia = modelo.NombreFantasia.ToUpperCheckForNull();
            modelo.NombreProveedor = modelo.NombreProveedor.ToUpperCheckForNull();
            modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
            modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
           await CreaProveedorAsync(modelo);
            return RedirectToAction("VerProveedores");
        }
       
        public async System.Threading.Tasks.Task<ActionResult> VerProveedores()
        {
            var data = await ListadeProveedoresAsync();
            List<ProveedorModel> lista = new List<ProveedorModel>();
            foreach(var row in data)
            {
                lista.Add(new ProveedorModel { IDProveedor = FormatRutView(row.IDProveedor), NombreFantasia = row.NombreFantasia, NombreProveedor = row.NombreProveedor });
            }
            return View(lista);
        
        }
    
        public async Task<JsonResult> FiltrarProveedores(string texto)
        {
            return Json(await FiltroProveedorAsync(texto));
        }
        
        public async Task<JsonResult> NuevoProveedorJS(BibliotecaDeClases.Modelos.Proveedores.ProveedorModel documento)
        {
            documento.IDProveedor = GetRut(documento.IDProveedor);
            documento.EmailProveedor = documento.EmailProveedor.ToUpperCheckForNull();
            documento.OtrosDetalles = documento.OtrosDetalles.ToUpperCheckForNull();
            documento.NombreFantasia = documento.NombreFantasia.ToUpperCheckForNull();
            documento.NombreProveedor = documento.NombreProveedor.ToUpperCheckForNull();
            documento.CreadoEn = documento.EditadoEn = DateTime.Now;
            documento.CreadoPor = documento.EditadoPor = User.Identity.Name;
            return Json(await CreaProveedorAsync(documento));
        }
        
        public async System.Threading.Tasks.Task<JsonResult> TopProveedores()
        {
            return Json(await Top5ProveedoresAsync());
        }
    
    
    
    }
}