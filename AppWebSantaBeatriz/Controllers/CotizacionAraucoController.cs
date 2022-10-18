using AppWebSantaBeatriz.Models.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Clientes.ProcesadorClientes;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorCotizacionesArauco;
using static BibliotecaDeClases.Logica.Traducción.TraductorProcesador;
using AppWebSantaBeatriz.Models.Error;
using static BibliotecaDeClases.Logica.ProcesadorMateriales;
using static BibliotecaDeClases.Logica.ProcesadorPersonal;
using AppWebSantaBeatriz.Models;
using static BibliotecaDeClases.Logica.ProcesadorCotizaciones;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorSeguridad;
using static BibliotecaDeClases.Logica.Cotizaciones.ProcesadorEquiposServicios;
using static BibliotecaDeClases.Logica.Personal.ProcesadorEspecialidades;
using static BibliotecaDeClases.Logica.Excel.EditorExcel;
using BibliotecaDeClases.Modelos.Cotizaciones;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorTransacciones;

namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class CotizacionAraucoController : Controller
    {
        public async Task<ActionResult> CreaCotizacion(int idubicacion)
        {
            var listacontactos = await ListaDeContactosAsync(1);
         

            CotizacionAraucoModel modelo = new CotizacionAraucoModel
            {
                Supervisores = listacontactos,
                IDUbicacion = idubicacion,

            };

            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> CreaCotizacion(CotizacionAraucoModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                    modelo.EditadoPor = modelo.CreadoPor = User.Identity.Name;
                    modelo.IDEstado = 1;
                   var data = await NuevaCotizacion(modelo);
                    return RedirectToAction("Detalles",new {id = data.ID });
                }
                catch(Exception e)
                {
                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));

                }
            }
            else
            {
                var listacontactos = await ListaDeContactosAsync(1);
              
                modelo.Supervisores = listacontactos;
                return View(modelo);
            }
       
        
        
        
        }    
        public async Task<ActionResult> Detalles(int id)
        {
           var info =  CargarCotizacionAraucoSimpleAsync(id);
            var data =  LoadMaterialesySubcotAsync(id);
            var personal = LoadCotPersonalAsync(id);
            var equipo = LoadCotEquiposAsync(id);
            var cat = ListaCategorias();
           await Task.WhenAll(info, data, personal, equipo, cat);
            var materiales = await data;
            var personales = await personal;
            var equipos = await equipo;
            var cats = await cat;
         

            var especialidades = await TablaEspecialidadesAsync();
           

            var estados = await LoadEstadosAsync();
            CotizacionAraucoModel modelo = new CotizacionAraucoModel
            {
                MaterialesExc = materiales,
                SolicitudPedido = info.Result.SolicitudPedido,
                NLicitacion = info.Result.NLicitacion,
                IngenieroContratos = info.Result.IngenieroContratos,
                Fecha = info.Result.Fecha,
                ValidezOferta = info.Result.ValidezOferta,
                IDCotizacion = info.Result.IDCotizacion,
                TiempoEjecucion = info.Result.TiempoEjecucion,
                Detalles = info.Result.Detalles,
                Notas = info.Result.Notas,
                ID = info.Result.ID,
                Personal = personales,
                Equipos = equipos,
                Especialidades = especialidades,
                Utilidad = info.Result.Utilidad,
                GastosGenerales = info.Result.GastosGenerales,
                Estados = estados,
                IDEstado = info.Result.IDEstado,
                CategoriasMRE = cats,
                NRFPAriba = info.Result.NRFPAriba

            };

            return View(modelo);

        }   
        public async Task<JsonResult> BorraSubCoti(int id)
        {
            try
            {
                return Json(await EliminaSubCotiAsync(id));
            }
           catch(Exception e)
            {
                return Json(RedirectToAction("VistaError", "Errores", FuncionError(e.Message)));
            }

        }
        public async Task<JsonResult> MaterialesySubcot(int id)
        {
            return Json(await LoadMaterialesySubcot(id));
        }
        public async Task<JsonResult> PptoPersonal(int id)
        {
            return Json(await LoadCotPersonalAsync(id));
        }
        public async Task<JsonResult> PptoEYS(int id)
        {
            return Json(await LoadCotEquiposAsync(id));       
        }     
        public async Task<ActionResult> VerProductoFabricacionPropia(int id)

        {

            var dataTask =  CargaSubCotizacionAraucoAsync(id);
            var equiposTask = LoadCotEquiposAsync(id);
            var materialesTask = LoadCotMaterialesAsync(id);
            var personalTask = LoadCotPersonalAsync(id);
            var seguirdadTask = LoadCotSeguridadAsync(id);
            var especialidadesTask = TablaEspecialidadesAsync();
            await Task.WhenAll(dataTask, equiposTask, materialesTask, personalTask, seguirdadTask, especialidadesTask);
            var modelo = await dataTask;
            modelo.ListaEquipos = await equiposTask;
            modelo.ListaMateriales = await materialesTask;
            modelo.ListaPersonal = await personalTask;
            modelo.ListaSeguridad = await seguirdadTask;
            modelo.Especialidades = await especialidadesTask;

            return View(modelo);
        }     
        public async Task<FileResult> GetExcelFileAsync(int id)
        {
            var libro = await ReturnExcelFileAsync(id);
            return File(libro, "application/xlsx", "Cotización.xlsx");
            //await CreaExcel(id);
            //byte[] excel = System.IO.File.ReadAllBytes(HttpContext.Server.MapPath("~/Content/Formatos/Formato1.xlsx"));
            //string contentType = MimeMapping.GetMimeMapping(HttpContext.Server.MapPath("~/Content/Formatos/Formato1.xlsx"));
            //return File(excel, contentType, "Cotización.xlsx"); 
        }
        public async Task<FileResult> GetExcelOfertaTecnicaFileAsync(int id)
        {
            var libro = await getExcelOfTecnicaAsync(id);
            return File(libro, "application/xlsx", "OfertaTécnica.xlsx");
            //await CreaExcel(id);
            //byte[] excel = System.IO.File.ReadAllBytes(HttpContext.Server.MapPath("~/Content/Formatos/Formato1.xlsx"));
            //string contentType = MimeMapping.GetMimeMapping(HttpContext.Server.MapPath("~/Content/Formatos/Formato1.xlsx"));
            //return File(excel, contentType, "Cotización.xlsx"); 
        }
        public async Task<ActionResult> GetPDFFileOFTAsync(int id)

        {
            //return File(await CreaPDFStream(id), "application/pdf", "Cotización.pdf");
            return File(await CreaPDFStreamOFT(id), "application/pdf");
        }
        public async Task<ActionResult> GetPDFFileAsync(int id)

        {
            //return File(await CreaPDFStream(id), "application/pdf", "Cotización.pdf");
            return File(await CreaPDFStream(id), "application/pdf");
        }
        public async Task<ActionResult> PanelCotizaciones()
        {
            
            var data = await CargarCotizacionesAsync();
                    

            ViewBag.Ubicaciones = await UbicacionesAraucoAsync();

            List<CotizacionAraucoModel> modelo = new List<CotizacionAraucoModel>();
            foreach(var row in data)
            {
                modelo.Add(new CotizacionAraucoModel
                {
                    ID = row.ID,
                    IDCotizacion = row.IDCotizacion,
                    Fecha = row.Fecha,
                    Detalles = row.Detalles,
                    TotalOferta = row.TotalOferta,
                    IngenieroContratos = row.IngenieroContratos
                    
                });
            }
            return View(modelo);
;        }
        public async Task<ActionResult> EdicionCotizacion(int id)
        {
            var contactos = await ListaDeContactosAsync(1);
         

            var data = await CargarCotizacionAraucoSimpleAsync(id);
            CotizacionAraucoModel modelo = new CotizacionAraucoModel
            {
                ID = data.ID,
                IDSupervisor = data.IDSupervisor,
                NLicitacion = data.NLicitacion,
                SolicitudPedido = data.SolicitudPedido,
                Fecha = data.Fecha,
                ValidezOferta = data.ValidezOferta,
                IDCotizacion = data.IDCotizacion,
                TiempoEjecucion = data.TiempoEjecucion,
                Detalles = data.Detalles,
                Notas = data.Notas,
                Supervisores = contactos,
                DATOE = data.DATOE,
                NRFPAriba = data.NRFPAriba
            };

            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EdicionCotizacion(CotizacionAraucoModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    modelo.CreadoEn = modelo.EditadoEn = DateTime.Now;
                    modelo.CreadoPor = modelo.EditadoPor = User.Identity.Name;
                    await EditaCotizacionAsync(modelo);
                    return RedirectToAction("Detalles", new { id = modelo.ID });
                }
                catch (Exception e)
                {

                    return RedirectToAction("VistaError", "Errores", FuncionError(e.Message));
                }
            }
            else
            {
                var contactos = await ListaDeContactosAsync(1);

                modelo.Supervisores = contactos;

                return View(modelo);
            }
            
        }
        public async Task<JsonResult> Data(int id)
        {
            return Json(await LoadMaterialesySubcot(id));
        }
        public async Task<JsonResult> getItemAutocomplete(string term)        
        {
            var data = await getItemsCot(term);
            string[] items = data.Select(x => x.Item).ToArray();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> getPriceAuto(string item)
        {
            var data = await getPriceCot(item);
            return Json(data);
        }
        public async Task<JsonResult> getItemES(string term)        
        {
            var data = await getItemsAuto(term);
            string[] items = data.Select(x => x.Item).ToArray();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> getPreciosES(string item)
        {
            return Json(await getPriceUnitAuto(item), JsonRequestBehavior.AllowGet);
        }

    }
}